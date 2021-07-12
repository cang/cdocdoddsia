//#define DEBUG_METETIME

using System;
using System.Collections.Generic;
using System.Text;
using Cloo;
using System.Diagnostics;

namespace SiGlaz.Cloo
{
    public class DeviceFFTFloat
    {
        #region source 
        public const string src = @"

#pragma OPENCL EXTENSION cl_khr_fp64 : enable
#define M_PI 3.14159265358979f
#define FFT_K 0.41421356237309515f

//Global size is x.Length/2, Scale = 1 for direct, 1/N to inverse (iFFT)
__kernel void Conjugate(__global float2 * x, __global const float * Scale)
{
   int i = get_global_id(0);
   
   float temp = Scale[0];
   float2 t = (float2)(temp, -temp);

   x[i] *= t;
}


// Return a*EXP(-I*PI*1/2) = a*(-I)
float2 mul_p1q2(float2 a) { return (float2)(a.y,-a.x); }

// Return a^2
float2 sqr_1(float2 a)
{ return (float2)(a.x*a.x-a.y*a.y,2.0f*a.x*a.y); }

// Return the 2x DFT2 of the four complex numbers in A
// If A=(a,b,c,d) then return (a',b',c',d') where (a',c')=DFT2(a,c)
// and (b',d')=DFT2(b,d).
float8 dft2_4(float8 a) { return (float8)(a.lo+a.hi,a.lo-a.hi); }

// Return the DFT of 4 complex numbers in A
float8 dft4_4(float8 a)
{
  // 2x DFT2
  float8 x = dft2_4(a);
  // Shuffle, twiddle, and 2x DFT2
  return dft2_4((float8)(x.lo.lo,x.hi.lo,x.lo.hi,mul_p1q2(x.hi.hi)));
}

// Complex product, multiply vectors of complex numbers

#define MUL_RE(a,b) (a.even*b.even - a.odd*b.odd)
#define MUL_IM(a,b) (a.even*b.odd + a.odd*b.even)

float2 mul_1(float2 a,float2 b)
{ float2 x; x.even = MUL_RE(a,b); x.odd = MUL_IM(a,b); return x; }

float4 mul_2(float4 a,float4 b)
{ float4 x; x.even = MUL_RE(a,b); x.odd = MUL_IM(a,b); return x; }

// Return the DFT2 of the two complex numbers in vector A
float4 dft2_2(float4 a) { return (float4)(a.lo+a.hi,a.lo-a.hi); }

// Return cos(alpha)+I*sin(alpha)  (3 variants)
float2 exp_alpha_1(float alpha)
{
  float cs,sn;
  // sn = sincos(alpha,&cs);  // sincos
  //cs = native_cos(alpha); sn = native_sin(alpha);  // native sin+cos
  cs = cos(alpha); sn = sin(alpha); // sin+cos
  return (float2)(cs,sn);
}

/*
// T = N/4 = number of threads.
// P is the length of input sub-sequences, 1,4,16,...,N/4.
__kernel void fft_radix4(__global const float2 * x,__global float2 * y,__global int * pp)
{
  int p = pp[0];
  int t = get_global_size(0); // number of threads
  int i = get_global_id(0); // current thread
  int k = i & (p-1); // index in input sequence, in 0..P-1
  // Inputs indices are I+{0,1,2,3}*T
  x += i;
  // Output indices are J+{0,1,2,3}*P, where
  // J is I with two 0 bits inserted at bit log2(P)
  y += ((i-k)<<2) + k;

  // Load and twiddle inputs
  // Twiddling factors are exp(_I*PI*{0,1,2,3}*K/2P)
  float alpha = -M_PI*(float)k/(float)(2*p);

// Load and twiddle
float2 u0 = x[0];
float2 u1 = mul_1(exp_alpha_1(alpha),x[t]);
float2 u2 = mul_1(exp_alpha_1(2*alpha),x[2*t]);
float2 u3 = mul_1(exp_alpha_1(3*alpha),x[3*t]);

// 2x DFT2 and twiddle
float2 v0 = u0 + u2;
float2 v1 = u0 - u2;
float2 v2 = u1 + u3;
float2 v3 = mul_p1q2(u1 - u3); // twiddle

// 2x DFT2 and store
y[0] = v0 + v2;
y[p] = v1 + v3;
y[2*p] = v0 - v2;
y[3*p] = v1 - v3;

}
*/


// mul_p*q*(a) returns a*EXP(-I*PI*P/Q)
#define mul_p0q1(a) (a)

#define mul_p0q2 mul_p0q1
//float2  mul_p1q2(float2 a) { return (float2)(a.y,-a.x); }

__constant float SQRT_1_2 = 0.707106781186548; // cos(Pi/4)
#define mul_p0q4 mul_p0q2
float2  mul_p1q4(float2 a) { return (float2)(SQRT_1_2)*(float2)(a.x+a.y,-a.x+a.y); }
#define mul_p2q4 mul_p1q2
float2  mul_p3q4(float2 a) { return (float2)(SQRT_1_2)*(float2)(-a.x+a.y,-a.x-a.y); }

__constant float COS_8 = 0.923879532511287; // cos(Pi/8)
__constant float SIN_8 = 0.382683432365089; // sin(Pi/8)
#define mul_p0q8 mul_p0q4
float2  mul_p1q8(float2 a) { return mul_1((float2)(COS_8,-SIN_8),a); }
#define mul_p2q8 mul_p1q4
float2  mul_p3q8(float2 a) { return mul_1((float2)(SIN_8,-COS_8),a); }
#define mul_p4q8 mul_p2q4
float2  mul_p5q8(float2 a) { return mul_1((float2)(-SIN_8,-COS_8),a); }
#define mul_p6q8 mul_p3q4
float2  mul_p7q8(float2 a) { return mul_1((float2)(-COS_8,-SIN_8),a); }

// Compute in-place DFT2 and twiddle
#define DFT2_TWIDDLE(a,b,t) { float2 tmp = t(a-b); a += b; b = tmp; }

/*
// T = N/16 = number of threads.
// P is the length of input sub-sequences, 1,16,256,...,N/16.
__kernel void fft_radix16(__global const float2 * x,__global float2 * y, __global int * pp)
{
  int p = pp[0];
  int t = get_global_size(0); // number of threads
  int i = get_global_id(0); // current thread


//////  y[i] = 2*x[i];
//////  return;

  int k = i & (p-1); // index in input sequence, in 0..P-1
  // Inputs indices are I+{0,..,15}*T
  x += i;
  // Output indices are J+{0,..,15}*P, where
  // J is I with four 0 bits inserted at bit log2(P)
  y += ((i-k)<<4) + k;

  // Load
  float2 u[16];
  for (int m=0;m<16;m++) u[m] = x[m*t];

  // Twiddle, twiddling factors are exp(_I*PI*{0,..,15}*K/4P)
  float alpha = -M_PI*(float)k/(float)(8*p);
  for (int m=1;m<16;m++) u[m] = mul_1(exp_alpha_1(m*alpha),u[m]);

  // 8x in-place DFT2 and twiddle (1)
  DFT2_TWIDDLE(u[0],u[8],mul_p0q8);
  DFT2_TWIDDLE(u[1],u[9],mul_p1q8);
  DFT2_TWIDDLE(u[2],u[10],mul_p2q8);
  DFT2_TWIDDLE(u[3],u[11],mul_p3q8);
  DFT2_TWIDDLE(u[4],u[12],mul_p4q8);
  DFT2_TWIDDLE(u[5],u[13],mul_p5q8);
  DFT2_TWIDDLE(u[6],u[14],mul_p6q8);
  DFT2_TWIDDLE(u[7],u[15],mul_p7q8);

  // 8x in-place DFT2 and twiddle (2)
  DFT2_TWIDDLE(u[0],u[4],mul_p0q4);
  DFT2_TWIDDLE(u[1],u[5],mul_p1q4);
  DFT2_TWIDDLE(u[2],u[6],mul_p2q4);
  DFT2_TWIDDLE(u[3],u[7],mul_p3q4);
  DFT2_TWIDDLE(u[8],u[12],mul_p0q4);
  DFT2_TWIDDLE(u[9],u[13],mul_p1q4);
  DFT2_TWIDDLE(u[10],u[14],mul_p2q4);
  DFT2_TWIDDLE(u[11],u[15],mul_p3q4);

  // 8x in-place DFT2 and twiddle (3)
  DFT2_TWIDDLE(u[0],u[2],mul_p0q2);
  DFT2_TWIDDLE(u[1],u[3],mul_p1q2);
  DFT2_TWIDDLE(u[4],u[6],mul_p0q2);
  DFT2_TWIDDLE(u[5],u[7],mul_p1q2);
  DFT2_TWIDDLE(u[8],u[10],mul_p0q2);
  DFT2_TWIDDLE(u[9],u[11],mul_p1q2);
  DFT2_TWIDDLE(u[12],u[14],mul_p0q2);
  DFT2_TWIDDLE(u[13],u[15],mul_p1q2);

  // 8x DFT2 and store (reverse binary permutation)
  y[0]    = u[0]  + u[1];
  y[p]    = u[8]  + u[9];
  y[2*p]  = u[4]  + u[5];
  y[3*p]  = u[12] + u[13];
  y[4*p]  = u[2]  + u[3];
  y[5*p]  = u[10] + u[11];
  y[6*p]  = u[6]  + u[7];
  y[7*p]  = u[14] + u[15];
  y[8*p]  = u[0]  - u[1];
  y[9*p]  = u[8]  - u[9];
  y[10*p] = u[4]  - u[5];
  y[11*p] = u[12] - u[13];
  y[12*p] = u[2]  - u[3];
  y[13*p] = u[10] - u[11];
  y[14*p] = u[6]  - u[7];
  y[15*p] = u[14] - u[15];
}

// N = size of input, power of 2. = 16
// T = N/2 = number of threads. = 8
// I = index of current thread, in 0..T-1. = 0..7
// DFT step, input is X[N] and output is Y[N]. 
// P is the length of input sub-sequences, 1,2,4,...,N/2. = 1,2,4,8
// Cell (S,K) is stored at index S*P+K. = 
__kernel void fft_radix2(__global const float2* x,__global float2* y,__global int* pp)
{
  int i = get_global_id(0); // number of threads = 0..7
  int t = get_global_size(0); // current thread = 8
  int p = pp[0]; // = 1
  int k = i & (p-1); // index in input sequence, in 0..P-1 = 0,0,...
  x += i; // input offset = 

  y += (i<<1) - k; // output offset
  float2 m1 = mul_1(exp_alpha_1(   -M_PI*(float)k/(float)p    ),x[t]);
  float4 u = dft2_2(  (float4)(x[0],m1)    );

  y[0] = u.lo;
  y[p] = u.hi;
}
*/

__kernel void fft_radix2_row(__global const float2* x,__global float2* y,__global int* pp)
{
    int i = get_global_id(0); // number of threads = 0..7
    int iy = get_global_id(1); // row
    int w = pp[1];

    int t = get_global_size(0); // current thread = 8
    int p = pp[0]; // = 1
    int k = i & (p-1); // index in input sequence, in 0..P-1 = 0,0,...
    
    x += i + (iy<<w); // input offset = 

    y += (i<<1) - k + (iy<<w); // output offset
    
    float2 m1 = mul_1(exp_alpha_1(   -M_PI*(float)k/(float)p    ),x[t]);
    float4 u = dft2_2(  (float4)(x[0],m1)    );

    y[0] = u.lo;
    y[p] = u.hi;
}

__kernel void fft_radix2_col(__global const float2* x,__global float2* y,__global int* pp)
{
    int i = get_global_id(1); // number of threads = 0..7
    int ix = get_global_id(0); // col
    int w = pp[1];

    int t = get_global_size(1); // current thread = 8
    int p = pp[0]; // = 1
    int k = i & (p-1); // index in input sequence, in 0..P-1 = 0,0,...
    
    x += (i<<w) + ix; // input offset = 

    y +=  (((i<<1) - k)<<w) + ix; // output offset
    
    float2 m1 = mul_1(exp_alpha_1(   -M_PI*(float)k/(float)p    ),x[(t<<w)]);
    float4 u = dft2_2(  (float4)(x[0],m1)    );

    y[0] = u.lo;
    y[(p<<w)] = u.hi;
}


// T = N/16 = number of threads.
// P is the length of input sub-sequences, 1,16,256,...,N/16.
__kernel void fft_radix16_row(__global const float2 * x,__global float2 * y, __global int * pp)
{
    int p = pp[0];
    int w = pp[1];
    int t = get_global_size(0); // number of threads
    int i = get_global_id(0); // current thread
    int iy = get_global_id(1);

    int k = i & (p-1); // index in input sequence, in 0..P-1
    // Inputs indices are I+{0,..,15}*T
    x += i + (iy<<w);
    // Output indices are J+{0,..,15}*P, where
    // J is I with four 0 bits inserted at bit log2(P)
    y += ((i-k)<<4) + k + (iy<<w);
  
     // Load
    float2 u[16];
    for (int m=0;m<16;m++) u[m] = x[m*t];

    // Twiddle, twiddling factors are exp(_I*PI*{0,..,15}*K/4P)
    float alpha = -M_PI*(float)k/(float)(8*p);
    for (int m=1;m<16;m++) u[m] = mul_1(exp_alpha_1(m*alpha),u[m]);

    // 8x in-place DFT2 and twiddle (1)
    DFT2_TWIDDLE(u[0],u[8],mul_p0q8);
    DFT2_TWIDDLE(u[1],u[9],mul_p1q8);
    DFT2_TWIDDLE(u[2],u[10],mul_p2q8);
    DFT2_TWIDDLE(u[3],u[11],mul_p3q8);
    DFT2_TWIDDLE(u[4],u[12],mul_p4q8);
    DFT2_TWIDDLE(u[5],u[13],mul_p5q8);
    DFT2_TWIDDLE(u[6],u[14],mul_p6q8);
    DFT2_TWIDDLE(u[7],u[15],mul_p7q8);

    // 8x in-place DFT2 and twiddle (2)
    DFT2_TWIDDLE(u[0],u[4],mul_p0q4);
    DFT2_TWIDDLE(u[1],u[5],mul_p1q4);
    DFT2_TWIDDLE(u[2],u[6],mul_p2q4);
    DFT2_TWIDDLE(u[3],u[7],mul_p3q4);
    DFT2_TWIDDLE(u[8],u[12],mul_p0q4);
    DFT2_TWIDDLE(u[9],u[13],mul_p1q4);
    DFT2_TWIDDLE(u[10],u[14],mul_p2q4);
    DFT2_TWIDDLE(u[11],u[15],mul_p3q4);

    // 8x in-place DFT2 and twiddle (3)
    DFT2_TWIDDLE(u[0],u[2],mul_p0q2);
    DFT2_TWIDDLE(u[1],u[3],mul_p1q2);
    DFT2_TWIDDLE(u[4],u[6],mul_p0q2);
    DFT2_TWIDDLE(u[5],u[7],mul_p1q2);
    DFT2_TWIDDLE(u[8],u[10],mul_p0q2);
    DFT2_TWIDDLE(u[9],u[11],mul_p1q2);
    DFT2_TWIDDLE(u[12],u[14],mul_p0q2);
    DFT2_TWIDDLE(u[13],u[15],mul_p1q2);

    // 8x DFT2 and store (reverse binary permutation)
    y[0]    = u[0]  + u[1];
    y[p]    = u[8]  + u[9];
    y[2*p]  = u[4]  + u[5];
    y[3*p]  = u[12] + u[13];
    y[4*p]  = u[2]  + u[3];
    y[5*p]  = u[10] + u[11];
    y[6*p]  = u[6]  + u[7];
    y[7*p]  = u[14] + u[15];
    y[8*p]  = u[0]  - u[1];
    y[9*p]  = u[8]  - u[9];
    y[10*p] = u[4]  - u[5];
    y[11*p] = u[12] - u[13];
    y[12*p] = u[2]  - u[3];
    y[13*p] = u[10] - u[11];
    y[14*p] = u[6]  - u[7];
    y[15*p] = u[14] - u[15];
}


// T = N/16 = number of threads.
// P is the length of input sub-sequences, 1,16,256,...,N/16.
__kernel void fft_radix16_col(__global const float2 * x,__global float2 * y, __global int * pp)
{
    int p = pp[0];
    int w = pp[1];
    int t = get_global_size(1); // number of threads
    int i = get_global_id(1); // current thread
    int ix = get_global_id(0);

    int k = i & (p-1); // index in input sequence, in 0..P-1
    // Inputs indices are I+{0,..,15}*T
    x += (i<<w) + ix;
    // Output indices are J+{0,..,15}*P, where
    // J is I with four 0 bits inserted at bit log2(P)
    y += ( (((i-k)<<4)+k) <<w)   + ix;
    
     // Load
    float2 u[16];
    for (int m=0;m<16;m++) u[m] = x[( (m*t)<<w ) ];

    // Twiddle, twiddling factors are exp(_I*PI*{0,..,15}*K/4P)
    float alpha = -M_PI*(float)k/(float)(8*p);
    for (int m=1;m<16;m++) u[m] = mul_1(exp_alpha_1(m*alpha),u[m]);

    // 8x in-place DFT2 and twiddle (1)
    DFT2_TWIDDLE(u[0],u[8],mul_p0q8);
    DFT2_TWIDDLE(u[1],u[9],mul_p1q8);
    DFT2_TWIDDLE(u[2],u[10],mul_p2q8);
    DFT2_TWIDDLE(u[3],u[11],mul_p3q8);
    DFT2_TWIDDLE(u[4],u[12],mul_p4q8);
    DFT2_TWIDDLE(u[5],u[13],mul_p5q8);
    DFT2_TWIDDLE(u[6],u[14],mul_p6q8);
    DFT2_TWIDDLE(u[7],u[15],mul_p7q8);

    // 8x in-place DFT2 and twiddle (2)
    DFT2_TWIDDLE(u[0],u[4],mul_p0q4);
    DFT2_TWIDDLE(u[1],u[5],mul_p1q4);
    DFT2_TWIDDLE(u[2],u[6],mul_p2q4);
    DFT2_TWIDDLE(u[3],u[7],mul_p3q4);
    DFT2_TWIDDLE(u[8],u[12],mul_p0q4);
    DFT2_TWIDDLE(u[9],u[13],mul_p1q4);
    DFT2_TWIDDLE(u[10],u[14],mul_p2q4);
    DFT2_TWIDDLE(u[11],u[15],mul_p3q4);

    // 8x in-place DFT2 and twiddle (3)
    DFT2_TWIDDLE(u[0],u[2],mul_p0q2);
    DFT2_TWIDDLE(u[1],u[3],mul_p1q2);
    DFT2_TWIDDLE(u[4],u[6],mul_p0q2);
    DFT2_TWIDDLE(u[5],u[7],mul_p1q2);
    DFT2_TWIDDLE(u[8],u[10],mul_p0q2);
    DFT2_TWIDDLE(u[9],u[11],mul_p1q2);
    DFT2_TWIDDLE(u[12],u[14],mul_p0q2);
    DFT2_TWIDDLE(u[13],u[15],mul_p1q2);

    // 8x DFT2 and store (reverse binary permutation)
    y[0]    = u[0]  + u[1];
    y[(p<<w)]    = u[8]  + u[9];
    y[(p<<(w+1))]  = u[4]  + u[5];//2 = (p*w) <<1 = (p<<w) <<1 = p << (w+1)
    y[(p<<(w+1)) + (p<<w)]  = u[12] + u[13];//3 = 2*p*w + p*w
    y[(p<<(w+2))]  = u[2]  + u[3];//4*p*w
    y[(p<<(w+2)) + (p<<w)]  = u[10] + u[11];//5  = 4 + 1
    y[(p<<(w+2)) + (p<<(w+1))]  = u[6]  + u[7]; //6  = 4 + 2
    y[(p<<(w+2)) + (p<<(w+1)) + (p<<w)]  = u[14] + u[15];//7 = 6 + 1
    y[(p<<(w+3))]  = u[0]  - u[1];//8*p*w
    y[(p<<(w+3)) + (p<<w)]  = u[8]  - u[9];//9 = 8 + 1
    y[(p<<(w+3)) + (p<<(w+1))] = u[4]  - u[5];//10 = 8+2
    y[(p<<(w+3)) + (p<<(w+1)) + (p<<w)] = u[12] - u[13];//11 = 8 + 3
    y[(p<<(w+3)) + (p<<(w+2)) ] = u[2]  - u[3];//12 = 8 + 4
    y[(p<<(w+3)) + (p<<(w+2)) + (p<<w)] = u[10] - u[11];//13 = 8 + 4 + 1
    y[(p<<(w+4)) - (p<<(w+1))] = u[6]  - u[7];//14 = 16 - 2
    y[(p<<(w+4)) - (p<<w)] = u[14] - u[15];//16 - 1
}


__kernel void fft_copy2device(__global const ushort* pin,__global float2* pout,__global int* pp)
{
    int x = get_global_id(0); // col
    int y = get_global_id(1); 
    int w0 = get_global_size(0); 
    int w = pp[0];
    pout[(y<<w) + x] = (float2)( (float)pin[y*w0+x],0);
}

/*
__kernel void fft_copy2data(__global const float2* pin,__global ushort* pout,__global int* pp)
{
    int x = get_global_id(0); // col
    int y = get_global_id(1); 
    int w0 = get_global_size(0); 
    int w = pp[0];
    pout[y*w0+x] = (ushort)(pin[(y<<w)+x].x);
}
*/

__kernel void fft_cutoff_hipass(
__global float2* pin
,__global int* pp //w,h,XOrigial ,YOrigial 
,__global double2* pr //xRadius2,yRadius2

)
{
    int x = get_global_id(0); // col
    int y = get_global_id(1); 
    int w = pp[0];

    int XOriginal= pp[2];
    int YOriginal= pp[3];
    
    double2 Radius2 = pr[0];
    double hh;

    double xx = x - XOriginal;
    double yy = y - YOriginal;

    double ratio = sqrt(xx*xx/Radius2.x + yy*yy/Radius2.y);

    if (ratio == 0) 
        hh = 0;
    else
        hh = 1.0f / (1 + FFT_K * pow(1/ratio, 2));

    int index = y*w+x;
    pin[index] = pin[index]*(float)hh;
    //pin[index].x = pin[index].x*hh;
    //pin[index].y = pin[index].y*hh;
}


__kernel void fft_cutoff_lopass(
__global float2* pin
,__global int* pp //w,h,XOrigial ,YOrigial 
,__global double* pr //xRadius2,yRadius2

)
{
    int x = get_global_id(0); // col
    int y = get_global_id(1); 
    int w = pp[0];

    int XOriginal= pp[2];
    int YOriginal= pp[3];
    
    double2 Radius2 = pr[0];

    double xx = x - XOriginal;
    double yy = y - YOriginal;

    double ratio = sqrt(xx*xx/Radius2.x + yy*yy/Radius2.y);

    double hh = (double)1 / (1 + FFT_K * pow(ratio, 2));

    int index = y*w+x;
    pin[index] = pin[index]*(float)hh;
    //pin[index].x = pin[index].x*hh;
    //pin[index].y = pin[index].y*hh;
}

__kernel void fft_mix_lopass(
__global const float2* pin
,__global ushort* pout
,__global const int* pp //w,h
,__global const double* pr //weightf,inv_weightf,addedValue

)
{
    int x = get_global_id(0); // col
    int y = get_global_id(1); 
    int w0 = get_global_size(0);
    int w = pp[0];
    
    int index = y*w0+x;
    
    double inv_weightf = pr[1];
    double weightf= pr[0];
    
    double val = pin[(y<<w)+x].x;//double val = pin[y*w+x].x;
    int ivalue = (int)(pout[index] * inv_weightf + val * weightf);
    if( ivalue > 255)
        ivalue = 255;
    else if( ivalue < 0)
        ivalue =0;
        
    pout[index] = ivalue;
}


__kernel void fft_mix_hipass(
__global const float2* pin
,__global __write_only ushort* pout
,__global const int* pp //w
,__global const double* pr //weightf,inv_weightf,addedValue

)
{
    int x = get_global_id(0); // col
    int y = get_global_id(1); 
    int w0 = get_global_size(0);
    int w = pp[0];
    
    int index = y*w0+x;
    double addedValue = pr[2];
    double inv_weightf = pr[1];
    double weightf= pr[0];
    
    double val = pin[(y<<w)+x].x + addedValue;
    int ivalue = (int)(pout[index]*(inv_weightf) + val * weightf);
    if( ivalue > 255)
        ivalue = 255;
    else if( ivalue < 0)
        ivalue =0;
        
    pout[index] = ivalue;
}

__kernel void FFT_SumData(
__global __read_only ushort* idata
,__global const int* pp //w
,__global __write_only double* odata
)
{
   int w = pp[0] ;
   int y = get_global_id(0);

   double sum = 0;
   int index = y*w;
   for(int i=0;i<w;i++)
   {
      sum+= idata[index++];
   }
   odata[y] = sum;
}

__kernel void FFT_SumReal(
__global __read_only float2* idata
,__global const int* pp //w
,__global __write_only double* odata
)
{
   int w = pp[0] ;
   int y = get_global_id(0);

   double sum = 0;
   int index = y*w;
   for(int i=0;i<w;i++)
   {
      sum+= idata[index++].x;
   }
   odata[y] = sum;
}

__kernel void SwapQuarter(
__global float2* idata
,__global const int* pp //w,h,nw,nh
)
{
    //swap (0,0) & (1,1)
    int w = pp[0] ;
    int h = pp[1] ;
  
    int x = get_global_id(0);
    int y = get_global_id(1);

    int idx0 = (y<<pp[2]) +x;//int idx0 = y*w+x;
    int idx = ( (y + (h>>1) ) << pp[2]) + x + (w>>1);//int idx = (y + (h>>1) )*w + x + (w>>1);
    float2 tmp = idata[idx0];
    idata[idx0] = idata[idx];
    idata[idx] = tmp;

    //swap (0,1) & (1,0)
    idx0 = (y<<pp[2]) + x + (w>>1);//idx0 = y*w + x + (w>>1);
    idx = ((y + (h>>1) ) << pp[2]) + x;//idx = (y + (h>>1) )*w + x;
    tmp = idata[idx0];
    idata[idx0] = idata[idx];
    idata[idx] = tmp;
}

";
        #endregion source 

        private static ComputeKernel kernelfft_radix2_row, kernelfft_radix2_col, kernelConjugate;
        private static ComputeKernel kernelfft_radix16_row, kernelfft_radix16_col;

        private static ComputeKernel kernelfft_cutoff_lopass, kernelfft_cutoff_hipass;
        private static ComputeKernel kernelfft_mix_lopass, kernelfft_mix_hipass;
        private static ComputeKernel kernelfft_SumData, kernelfft_SumReal;
        public static ComputeKernel kernelfft_copy2device;//, kernelfft_copy2data;
        public static ComputeKernel kernelfft_SwapQuarter;
        
        public void InitKernels()
        {
            if (kernelConjugate==null)
            {
                DeviceProgram.InitCL();
                try
                {
                    DeviceProgram.Compile(src);
                }
                catch
                {
                }

                kernelfft_radix2_row = DeviceProgram.CreateKernel("fft_radix2_row");
                kernelfft_radix2_col = DeviceProgram.CreateKernel("fft_radix2_col");
                kernelConjugate = DeviceProgram.CreateKernel("Conjugate");
                kernelfft_cutoff_lopass = DeviceProgram.CreateKernel("fft_cutoff_lopass");
                kernelfft_cutoff_hipass = DeviceProgram.CreateKernel("fft_cutoff_hipass");
                kernelfft_mix_lopass = DeviceProgram.CreateKernel("fft_mix_lopass");
                kernelfft_mix_hipass = DeviceProgram.CreateKernel("fft_mix_hipass");
                kernelfft_SumData = DeviceProgram.CreateKernel("FFT_SumData");
                kernelfft_SumReal = DeviceProgram.CreateKernel("FFT_SumReal");
                kernelfft_copy2device = DeviceProgram.CreateKernel("fft_copy2device");
                //kernelfft_copy2data = DeviceProgram.CreateKernel("fft_copy2data");
                kernelfft_SwapQuarter = DeviceProgram.CreateKernel("SwapQuarter");
                kernelfft_radix16_row = DeviceProgram.CreateKernel("fft_radix16_row");
                kernelfft_radix16_col = DeviceProgram.CreateKernel("fft_radix16_col");
            }
        }

        #region Base Radix 2

        public float[] FFT_2D_Radix2(float[] buff ,int w,int h)
        {
            DeviceBuffer<float> cbIn = DeviceBuffer<float>.CreateBufferReadOnly(buff);
            DeviceBuffer<float> cbOut = DeviceBuffer<float>.CreateBufferReadOnly(cbIn.Count);

            FFT_2D_Radix2(ref cbIn, ref cbOut, w, h);

            float[] ret = cbOut.Read(null);

            cbIn.Dispose();
            cbOut.Dispose();

            return ret;
        }

        public float[] iFFT_2D_Radix2(float[] buff, int w, int h)
        {
            DeviceBuffer<float> cbIn = DeviceBuffer<float>.CreateHostBufferReadOnly(buff);
            DeviceBuffer<float> cbOut = DeviceBuffer<float>.CreateBufferReadOnly(cbIn.Count);

            iFFT_2D_Radix2(ref cbIn, ref cbOut, w, h);

            float[] ret = cbOut.Read(null);

            cbIn.Dispose();
            cbOut.Dispose();

            return ret;
        }

        public void FFT_2D_Radix2(ref DeviceBuffer<float> cbBuff, ref DeviceBuffer<float> cbRet, int w, int h)
        {
            if (cbBuff == null) return;

            InitKernels();

            DeviceBuffer<int> cbParam = DeviceBuffer<int>.CreateBufferReadOnly(2);


            /// ROWWWWWWWWWWWWWWWWWWWWWWWWWWWWW
            int nn = (int)Math.Log(w, 2);
            nn = 1 << nn;
            if (nn != w) throw new Exception("Number of elements should be a power of 2 ( vector length should be 2*pow(2,n) )");

            ComputeMemory[] args = new ComputeMemory[] { cbBuff, cbRet, cbParam };
            ComputeMemory[] args2 = new ComputeMemory[] { cbRet, cbBuff, cbParam };
            bool usar2 = true;

            int[] p = new int[] { 1, (int)Math.Log(w, 2) };
            cbParam.Write(p, null);
            int n = w >> 1;
            while (p[0] <= n)
            {
                usar2 = !usar2;
                if (usar2)
                    DeviceProgram.ExecuteKernel(kernelfft_radix2_row, args2, new int[] { n, h });
                else
                    DeviceProgram.ExecuteKernel(kernelfft_radix2_row, args, new int[] { n, h });

                DeviceProgram.Finish();

                p[0] = p[0] << 1;
                cbParam.Write(p, null);
            }

            if (!usar2)
            {
                DeviceBuffer<float> temp = cbBuff;
                cbBuff = cbRet; cbRet = temp;
            }


            /// COLLLLLLLL
            nn = (int)Math.Log(h, 2);
            nn = 1 << nn;
            if (nn != h) throw new Exception("Number of elements should be a power of 2 ( vector length should be 2*pow(2,n) )");

            args = new ComputeMemory[] { cbBuff, cbRet, cbParam };
            args2 = new ComputeMemory[] { cbRet, cbBuff, cbParam };
            usar2 = true;

            p[0] = 1;
            cbParam.Write(p, null);
            n = h >> 1;

            while (p[0] <= n)
            {
                usar2 = !usar2;
                if (usar2)
                    DeviceProgram.ExecuteKernel(kernelfft_radix2_col, args2, new int[] { w, n });
                else
                    DeviceProgram.ExecuteKernel(kernelfft_radix2_col, args, new int[] { w, n });

                DeviceProgram.Finish();

                p[0] = p[0] << 1;
                cbParam.Write(p, null);
            }

            if (usar2)
            {
                DeviceBuffer<float> temp = cbBuff;
                cbBuff = cbRet; cbRet = temp;
            }

            cbParam.Dispose();
        }

        public void iFFT_2D_Radix2(ref DeviceBuffer<float> cbBuff,ref DeviceBuffer<float> cbRet, int w, int h)
        {
            if (cbBuff == null) return;

            InitKernels();

            int n = (int)cbBuff.Count;

            DeviceBuffer<float> cbParam = DeviceBuffer<float>.CreateBufferReadOnly(1);

            float[] scale = new float[] { 1 };
            cbParam.Write(scale, null);

            DeviceProgram.ExecuteKernel(kernelConjugate,
                new ComputeMemory[] {
                    cbBuff,
                    cbParam
                }, n >> 1);
            DeviceProgram.Finish();


            FFT_2D_Radix2(ref cbBuff,ref cbRet,w, h);

            scale[0] = 1 / (float)(n >> 1);
            cbParam.Write(scale, null);

            DeviceProgram.ExecuteKernel(kernelConjugate,
                new ComputeMemory[] {
                    cbRet,
                    cbParam
                }, n >> 1);
            DeviceProgram.Finish();

            cbParam.Dispose();
        }

        #endregion

        #region Base Radix 16
        public float[] FFT_2D_Radix16(float[] buff, int w, int h)
        {
            DeviceBuffer<float> cbIn = DeviceBuffer<float>.CreateBufferReadOnly(buff);
            DeviceBuffer<float> cbOut = DeviceBuffer<float>.CreateBufferReadOnly(cbIn.Count);

            FFT_2D_Radix16(ref cbIn, ref cbOut, w, h);

            float[] ret = cbOut.Read(null);

            cbIn.Dispose();
            cbOut.Dispose();

            return ret;
        }

        public float[] iFFT_2D_Radix16(float[] buff, int w, int h)
        {
            DeviceBuffer<float> cbIn = DeviceBuffer<float>.CreateHostBufferReadOnly(buff);
            DeviceBuffer<float> cbOut = DeviceBuffer<float>.CreateBufferReadOnly(cbIn.Count);

            iFFT_2D_Radix16(ref cbIn, ref cbOut, w, h);

            float[] ret = cbOut.Read(null);

            cbIn.Dispose();
            cbOut.Dispose();

            return ret;
        }

        public void FFT_2D_Radix16(ref DeviceBuffer<float> cbBuff, ref DeviceBuffer<float> cbRet, int w, int h)
        {
            if (cbBuff == null) return;

            InitKernels();

            DeviceBuffer<int> cbParam = DeviceBuffer<int>.CreateBufferReadOnly(2);

            /// ROWWWWWWWWWWWWWWWWWWWWWWWWWWWWW
            int nn = (int)Math.Log(w, 16);
            nn = 1 << (nn << 2);
            if (nn != w) throw new Exception("Number of elements should be a power of 16 ( vector length should be 2*pow(16,n) )");

            ComputeMemory[] args = new ComputeMemory[] { cbBuff, cbRet, cbParam };
            ComputeMemory[] args2 = new ComputeMemory[] { cbRet, cbBuff, cbParam };
            bool usar2 = true;

            int[] p = new int[] { 1, (int)Math.Log(w, 2) };
            cbParam.Write(p, null);
            int n = w >> 4;
            while (p[0] <= n)
            {
                usar2 = !usar2;
                if (usar2)
                    DeviceProgram.ExecuteKernel(kernelfft_radix16_row, args2, new int[] { n, h });
                else
                    DeviceProgram.ExecuteKernel(kernelfft_radix16_row, args, new int[] { n, h });

                DeviceProgram.Finish();

                p[0] = p[0] << 4;
                cbParam.Write(p, null);
            }

            if (!usar2)
            {
                DeviceBuffer<float> temp = cbBuff;
                cbBuff = cbRet; cbRet = temp;
            }


            /// COLLLLLLLL
            nn = (int)Math.Log(h, 16);
            nn = 1 << (nn << 2);
            if (nn != h) throw new Exception("Number of elements should be a power of 16 ( vector length should be 2*pow(16,n) )");

            args = new ComputeMemory[] { cbBuff, cbRet, cbParam };
            args2 = new ComputeMemory[] { cbRet, cbBuff, cbParam };
            usar2 = true;

            p[0] = 1;
            cbParam.Write(p, null);
            n = h >> 4;

            while (p[0] <= n)
            {
                usar2 = !usar2;
                if (usar2)
                    DeviceProgram.ExecuteKernel(kernelfft_radix16_col, args2, new int[] { w, n });
                else
                    DeviceProgram.ExecuteKernel(kernelfft_radix16_col, args, new int[] { w, n });

                DeviceProgram.Finish();

                p[0] = p[0] << 4;
                cbParam.Write(p, null);
            }

            if (usar2)
            {
                DeviceBuffer<float> temp = cbBuff;
                cbBuff = cbRet; cbRet = temp;
            }

            cbParam.Dispose();
        }

        public void iFFT_2D_Radix16(ref DeviceBuffer<float> cbBuff, ref DeviceBuffer<float> cbRet, int w, int h)
        {
            if (cbBuff == null) return;

            InitKernels();

            int n = (int)cbBuff.Count;

            DeviceBuffer<float> cbParam = DeviceBuffer<float>.CreateBufferReadOnly(1);

            float[] scale = new float[] { 1 };
            cbParam.Write(scale, null);

            DeviceProgram.ExecuteKernel(kernelConjugate,
                new ComputeMemory[] {
                    cbBuff,
                    cbParam
                }, n >> 1);
            DeviceProgram.Finish();


            FFT_2D_Radix16(ref cbBuff, ref cbRet, w, h);

            scale[0] = 1 / (float)(n >> 1);
            cbParam.Write(scale, null);

            DeviceProgram.ExecuteKernel(kernelConjugate,
                new ComputeMemory[] {
                    cbRet,
                    cbParam
                }, n >> 1);
            DeviceProgram.Finish();

            cbParam.Dispose();
        }
        #endregion

        #region FFT filter

        public void FFT_2D_Radix(int radiusbase,ref DeviceBuffer<float> cbBuff, ref DeviceBuffer<float> cbRet, int w, int h)
        {
            if (radiusbase == 2)
                FFT_2D_Radix2(ref cbBuff, ref cbRet, w, h);
            else if(radiusbase == 16)
                FFT_2D_Radix16(ref cbBuff, ref cbRet, w, h);
        }

        public void iFFT_2D_Radix(int radiusbase, ref DeviceBuffer<float> cbBuff, ref DeviceBuffer<float> cbRet, int w, int h)
        {
            if (radiusbase == 2)
                iFFT_2D_Radix2(ref cbBuff, ref cbRet, w, h);
            else if (radiusbase == 16)
                iFFT_2D_Radix16(ref cbBuff, ref cbRet, w, h);
        }

        public void SwapQuarter(DeviceBuffer<float> cbBuff, int w, int h)
        {
            if (cbBuff == null) return;
            InitKernels();

            DeviceBuffer<int> cbPp = DeviceBuffer<int>.CreateHostBufferReadOnly(new int[] { w, h, (int)Math.Log(w,2),(int)Math.Log(h,2) });

            DeviceProgram.ExecuteKernel(kernelfft_SwapQuarter,
                    new ComputeMemory[]{
                        cbBuff,
                        cbPp
                    }, new int[] { (w >> 1), (h >> 1) });

            DeviceProgram.Finish();
        }


        /// <summary>
        /// CutOff
        /// </summary>
        /// <param name="cbBuff"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="cutoff"></param>
        /// <param name="type"> HighPass = 0,LowPass = 1,</param>
        public void CutOff(DeviceBuffer<float> cbBuff, int w,int h,int wfft, int hfft, float cutoff, int type)
        {
	        float cutoffValue = cutoff / 100.0F;
	        double xRadius2 = (double)wfft;
	        double yRadius2 = (double)hfft;
	        xRadius2  *= cutoffValue;
	        xRadius2 *= xRadius2;
	        yRadius2  *= cutoffValue;
	        yRadius2 *= yRadius2;
            int XOrigial = (int)(wfft/ 2 - 0.5);
            int YOrigial = (int)(hfft/ 2 - 0.5);

            DeviceBuffer<int> cbPp = DeviceBuffer<int>.CreateHostBufferReadOnly(new int[]{wfft,hfft,XOrigial,YOrigial});
            DeviceBuffer<double> cbPr = DeviceBuffer<double>.CreateHostBufferReadOnly(new double[]{xRadius2,yRadius2});

            if (type == 1)//low pass
                DeviceProgram.ExecuteKernel(kernelfft_cutoff_lopass,
                    new ComputeMemory[]{
                        cbBuff,
                        cbPp,
                        cbPr
                    }, new int[] {wfft,hfft }); 
            else //hi pass
                DeviceProgram.ExecuteKernel(kernelfft_cutoff_hipass,
                    new ComputeMemory[]{
                        cbBuff,
                        cbPp,
                        cbPr
                    }, new int[] { wfft, hfft });

            DeviceProgram.Finish();

            cbPr.Dispose();
            cbPp.Dispose();
        }


        /// <param name="type"> HighPass = 0,LowPass = 1,</param>
        public void Mix(DeviceBuffer<ushort> cbData,int w,int h
            ,DeviceBuffer<float> cbBuff,int wfft,int hfft
            , float weight, int type)
        {
            float weightf = weight / 100.0F;
            float inv_weightf = 1.0F - weightf;

            DeviceBuffer<int> cbPp = DeviceBuffer<int>.CreateBufferReadOnly(1);
            DeviceBuffer<double> cbPr = DeviceBuffer<double>.CreateHostBufferReadOnly(new double[] { weightf, inv_weightf, 0 });

            if (type == 0)//hi pass
            {
                //get real avg
                cbPp.Write(new int[] { wfft }, null);
                DeviceBuffer<double> cbSumReal = DeviceBuffer<double>.CreateBufferReadOnly(hfft);
                DeviceProgram.ExecuteKernel(kernelfft_SumReal,
                        new ComputeMemory[]{
                        cbBuff,
                        cbPp,
                        cbSumReal,
                    }, hfft );

                DeviceProgram.Finish();
                double[] sumreal= cbSumReal.Read(null);
                double avgreal = 0;
                for(int i=0;i<hfft;i++)
                {
                    avgreal += sumreal[i];
                }
                avgreal /= (wfft * hfft);
                cbSumReal.Dispose();


                //get data avg
                cbPp.Write(new int[] {w},null);
                DeviceBuffer<double> cbSumData = DeviceBuffer<double>.CreateBufferReadOnly(h);
                DeviceProgram.ExecuteKernel(kernelfft_SumData,
                        new ComputeMemory[]{
                        cbData,
                        cbPp,
                        cbSumData,
                    }, h);

                DeviceProgram.Finish();
                double[] sumdata= cbSumData.Read(null);
                double avgdata= 0;
                for (int i = 0; i < h; i++)
                {
                    avgdata += sumdata[i];
                }
                avgdata /= (w * h);
                cbSumData.Dispose();


                //get addedValue
                double addedValue = avgdata  - avgreal;
                cbPr.Write(new double[] { weightf, inv_weightf, addedValue }, null);
            }

            cbPp.Write(new int[] { (int)Math.Log(wfft,2) }, null);
            if (type == 1)//low pass
                DeviceProgram.ExecuteKernel(kernelfft_mix_lopass,
                        new ComputeMemory[]{
                        cbBuff,
                        cbData,
                        cbPp,
                        cbPr
                    }, new int[] { w, h });
            else //hi pass
                DeviceProgram.ExecuteKernel(kernelfft_mix_hipass,
                     new ComputeMemory[]{
                        cbBuff,
                        cbData,
                        cbPp,
                        cbPr
                    }, new int[] { w, h });

            DeviceProgram.Finish();

            cbPp.Dispose();
            cbPr.Dispose();
        }


        public void FFTApply(DeviceBuffer<ushort> cbData, int w, int h, float weight, float cutoff, int type)
        {
            int rbase = 16;
            int aw = (int)Math.Ceiling(Math.Log(w, rbase));
            int ah = (int)Math.Ceiling(Math.Log(h, rbase));
            int wfft = (int)Math.Pow(rbase,aw);
            int hfft = (int)Math.Pow(rbase,ah);
            int nn = wfft * hfft;
            


#if DEBUG_METETIME
            StringBuilder sbTrace = new StringBuilder();
            Stopwatch sw = new Stopwatch(); sw.Start();
#endif
            InitKernels();

#if DEBUG_METETIME
            sw.Stop(); sbTrace.AppendLine("InitKernels :" + sw.ElapsedTicks); sw.Reset();sw.Start();
#endif
            //create temp float buffer  for FFT
            DeviceBuffer<float> cbIn = DeviceBuffer<float>.CreateBufferReadOnly(nn*2);
            DeviceBuffer<float> cbOut = DeviceBuffer<float>.CreateBufferReadOnly(nn*2);

#if DEBUG_METETIME
            sw.Stop(); sbTrace.AppendLine("Create Device Memory :" + sw.ElapsedTicks); sw.Reset(); sw.Start();
#endif
            //copy data into cbIn
            DeviceBuffer<int> cbPp = DeviceBuffer<int>.CreateHostBufferReadOnly(new int[] { (int)Math.Log(wfft,2)});
            DeviceProgram.ExecuteKernel(kernelfft_copy2device,
                    new ComputeMemory[]{
                        cbData,
                        cbIn,
                        cbPp,
                    }, new int[] { w, h});
            DeviceProgram.Finish();

#if DEBUG_METETIME
            sw.Stop(); sbTrace.AppendLine("Copy Data into device :" + sw.ElapsedTicks); sw.Reset(); sw.Start();
#endif
            SwapQuarter(cbIn, wfft, hfft);

#if DEBUG_METETIME
            sw.Stop(); sbTrace.AppendLine("SwapQuarter1 :" + sw.ElapsedTicks); sw.Reset(); sw.Start();
#endif
            ////exec FFT
            FFT_2D_Radix(rbase,ref cbIn, ref cbOut, wfft, hfft);

#if DEBUG_METETIME
            sw.Stop(); sbTrace.AppendLine("FFT_2D_Radix :" + sw.ElapsedTicks); sw.Reset(); sw.Start();
#endif
            SwapQuarter(cbOut, wfft, hfft);

#if DEBUG_METETIME
            sw.Stop(); sbTrace.AppendLine("SwapQuarter2 :" + sw.ElapsedTicks); sw.Reset(); sw.Start();
#endif
            ////CutOff
            CutOff(cbOut,w,h,wfft, hfft, cutoff, type);

#if DEBUG_METETIME
            sw.Stop(); sbTrace.AppendLine("CutOff :" + sw.ElapsedTicks); sw.Reset(); sw.Start();
#endif
            SwapQuarter(cbOut, wfft, hfft);

#if DEBUG_METETIME
            sw.Stop(); sbTrace.AppendLine("SwapQuarter3 :" + sw.ElapsedTicks); sw.Reset(); sw.Start();
#endif
            //Invert FFT
            iFFT_2D_Radix(rbase,ref cbOut, ref cbIn, wfft, hfft);

#if DEBUG_METETIME
            sw.Stop(); sbTrace.AppendLine("iFFT_2D_Radix :" + sw.ElapsedTicks); sw.Reset(); sw.Start();
#endif
            SwapQuarter(cbIn, wfft, hfft);

#if DEBUG_METETIME
            sw.Stop(); sbTrace.AppendLine("SwapQuarter4 :" + sw.ElapsedTicks); sw.Reset(); sw.Start();
#endif
            ////Mix
            Mix(cbData, w, h, cbIn, wfft, hfft, weight, type);

#if DEBUG_METETIME
            sw.Stop(); sbTrace.AppendLine("Mix :" + sw.ElapsedTicks); sw.Reset(); sw.Start();
#endif           
            //Free device memory
            cbPp.Dispose();
            cbIn.Dispose();
            cbOut.Dispose();

#if DEBUG_METETIME
            sw.Stop(); sbTrace.AppendLine("Dispose :" + sw.ElapsedTicks); 
            Trace.WriteLine(sbTrace.ToString());
#endif
            //Finish :)
        }

        #endregion

    }
}
