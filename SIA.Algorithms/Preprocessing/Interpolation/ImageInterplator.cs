#define DEBUG_METETIME_

/* for optimize
ImageInterpolator : CreateHostBufferReadOnly intput: 1
ImageInterpolator : CreateHostBufferReadOnly output: 124
ImageInterpolator : kernelBilinearInterpolatePixel: 27966
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using SIA.IPEngine;
using SIA.SystemLayer;
using SIA.SystemFrameworks;
using SiGlaz.Cloo;
using Cloo;

namespace SIA.Algorithms.Preprocessing.Interpolation
{
    public enum InterpolationMethod
    {
        None,
        NearestNeibourgh,
        Bilinear,
        Bicubic
    }

    public class ImageInterpolator
    {
        public static GreyDataImage AffineTransform(InterpolationMethod method, GreyDataImage image, PointF[] source, int newWidth, int newHeight,
            ref System.Drawing.Drawing2D.Matrix inverseTransform)
        {
            RectangleF dest = new RectangleF(0, 0, newWidth, newHeight);
            inverseTransform = new System.Drawing.Drawing2D.Matrix(dest, source);
            MyMatrix transform = new MyMatrix(inverseTransform);

            GreyDataImage result = new GreyDataImage(newWidth, newHeight);
            switch (method)
            {
                case InterpolationMethod.NearestNeibourgh:
                    throw new Exception("Not implemented yet");
                    break;

                case InterpolationMethod.Bilinear:
                    BilinearInterpolate(image, transform, result);
                    break;

                case InterpolationMethod.Bicubic:
                    throw new Exception("Not implemented yet");
                    break;

                default:
                    throw new Exception("Invalid interpolation method");

            }

            return result;
        }

        public static void AffineTransform(InterpolationMethod method, GreyDataImage image, 
            System.Drawing.Drawing2D.Matrix transform, GreyDataImage output)
        {
            MyMatrix mytransform = new MyMatrix(transform);
            
            switch (method)
            {
                case InterpolationMethod.NearestNeibourgh:
                    throw new Exception("Not implemented yet");
                    break;

                case InterpolationMethod.Bilinear:
                    BilinearInterpolate(image, mytransform, output);
                    break;

                case InterpolationMethod.Bicubic:
                    throw new Exception("Not implemented yet");
                    break;

                default:
                    throw new Exception("Invalid interpolation method");

            }
        }

        public unsafe static void AffineTransform(InterpolationMethod method, ushort* srcData, int srcWidth, int srcHeight,
             System.Drawing.Drawing2D.Matrix transform, ushort* destData, int newWidth, int newHeight)
        {
            MyMatrix mytransform = new MyMatrix(transform);

            switch (method)
            {
                case InterpolationMethod.NearestNeibourgh:
                    throw new Exception("Not implemented yet");
                    break;

                case InterpolationMethod.Bilinear:
                    BilinearInterpolate(srcData, srcWidth, srcHeight, mytransform, destData, newWidth, newHeight);
                    break;

                case InterpolationMethod.Bicubic:
                    throw new Exception("Not implemented yet");
                    break;

                default:
                    throw new Exception("Invalid interpolation method");

            }
        }

        private unsafe static void BilinearInterpolate(GreyDataImage image, MyMatrix transform, GreyDataImage result)
        {
            ushort* srcData = image._aData;
            ushort* destData = result._aData;
            
            //double dx1, dx2, dy1, dy2;
            //int x, y;            

            int newHeight = result._height;
            int newWidth = result._width;
            int srcWidth = image._width;
            int srcHeight = image._height;
            int srcWidth1 = srcWidth - 1;
            int srcHeight1 = srcHeight - 1;

            //pDestData = destData;
            //for (y = 0; y < newHeight; y++)
            Parallel.For(0, newHeight, delegate(int y)
            {
                double dx1, dx2, dy1, dy2;
                int x;

                ushort* pDestData = null;
                ushort* pSourceData = null;

                pDestData = (destData + y * newWidth);

                for (x = 0; x < newWidth; x++, pDestData++)
                {
                    float orgX = x + 0.5f, orgY = y + 0.5f;
                    transform.TransformPoints(ref orgX, ref orgY);

                    int ix = (int)Math.Floor(orgX);
                    int iy = (int)Math.Floor(orgY);

                    if (ix < 1)
                        ix = 1;
                    else if (ix >= srcWidth1)
                        ix = srcWidth1;

                    if (iy < 1)
                        iy = 1;
                    else if (iy >= srcHeight1)
                        iy = srcHeight1;

                    if (ix == 1 || ix == srcWidth1 || iy == 0 || iy == srcHeight1)
                    {
                        *pDestData = *(srcData + iy * srcWidth + ix);
                        continue;
                    }

                    dx2 = orgX - ix;
                    dx1 = 1.0 - dx2;

                    dy2 = orgY - iy;
                    dy1 = 1.0 - dy2;

                    pSourceData = srcData + iy * srcWidth + ix;
                    float p11 = *(pSourceData);
                    float p12 = *(pSourceData + 1);
                    float p21 = *(pSourceData + srcWidth);
                    float p22 = *(pSourceData + srcWidth + 1);
                    *pDestData = (ushort)(p11 * dy1 * dx1 + p12 * dy1 * dx2 + p21 * dy2 * dx1 + p22 * dy2 * dx2);
                }
            });
        }

        private unsafe static void BilinearInterpolate(ushort* srcData, int srcWidth, int srcHeight,
            MyMatrix transform, ushort* destData, int newWidth, int newHeight)
        {
            ushort* pDestData = null;
            ushort* pSourceData = null;

            double dx1, dx2, dy1, dy2;

            int x, y;

            int srcWidth1 = srcWidth - 1;
            int srcHeight1 = srcHeight - 1;

            pDestData = destData;
            for (y = 0; y < newHeight; y++)
            {
                for (x = 0; x < newWidth; x++, pDestData++)
                {
                    float orgX = x + 0.5f, orgY = y + 0.5f;
                    transform.TransformPoints(ref orgX, ref orgY);

                    int ix = (int)Math.Floor(orgX);
                    int iy = (int)Math.Floor(orgY);

                    if (ix < 1)
                        ix = 1;
                    else if (ix >= srcWidth1)
                        ix = srcWidth1;

                    if (iy < 1)
                        iy = 1;
                    else if (iy >= srcHeight1)
                        iy = srcHeight1;

                    if (ix == 1 || ix == srcWidth1 || iy == 1 || iy == srcHeight1)
                    {
                        *pDestData = *(srcData + iy * srcWidth + ix);
                        continue;
                    }

                    dx2 = orgX - ix;
                    dx1 = 1.0 - dx2;

                    dy2 = orgY - iy;
                    dy1 = 1.0 - dy2;

                    pSourceData = srcData + iy * srcWidth + ix;
                    float p11 = *(pSourceData);
                    float p12 = *(pSourceData + 1);
                    float p21 = *(pSourceData + srcWidth);
                    float p22 = *(pSourceData + srcWidth + 1);
                    *pDestData = (ushort)(p11 * dy1 * dx1 + p12 * dy1 * dx2 + p21 * dy2 * dx1 + p22 * dy2 * dx2);
                }
            }
        }


#region GPU_SUPPORTED

#if GPU_SUPPORTED
        #region cl source code

        //need to check this kernerl : cost 37615 tick :(
        private const string srcBilinearInterpolate = @"
#pragma OPENCL EXTENSION cl_khr_fp64 : enable
//#pragma OPENCL EXTENSION cl_khr_byte_addressable_store : enable

__kernel void BilinearInterpolatePixel(
__global __read_only ushort* idata //input data
,__global __write_only ushort* odata //output data
,__global __read_only int* param//w,h,new with,new height
,__global __read_only float* mt//m11,m12,m21,m22,dx,dy
)
{
    int srcWidth = param[0];
    int srcHeight = param[1];
    int newWidth = param[2];
    int newHeight = param[3];
    int srcWidth1 = srcWidth - 1;
    int srcHeight1 = srcHeight - 1;

    int x = get_global_id(0);
    int y = get_global_id(1);

    float orgX = x + 0.5f, orgY = y + 0.5f;

    //transform
    float oldx = orgX;
    float oldy = orgY;
    orgX = mt[0] * oldx + mt[2] * oldy + mt[4];
    orgY = mt[1] * oldx + mt[3] * oldy + mt[5];

    int ix = (int)floor(orgX);
    int iy = (int)floor(orgY);

    if (ix < 1)
      ix = 1;
    else if (ix >= srcWidth1)
      ix = srcWidth1;

    if (iy < 1)
      iy = 1;
    else if (iy >= srcHeight1)
      iy = srcHeight1;
      
    if (ix == 1 || ix == srcWidth1 || iy == 0 || iy == srcHeight1)
    {
      odata[y * newWidth + x] = idata[iy * srcWidth + ix];
      return;
    }

    double dx2 = orgX - ix;
    double dx1 = 1.0 - dx2;

    double dy2 = orgY - iy;
    double dy1 = 1.0 - dy2;

    int i = iy * srcWidth + ix;
    float p11 = idata[i];
    float p12 = idata[i+1];
    float p21 = idata[i+srcWidth];
    float p22 = idata[i+srcWidth+1];

    i = y * newWidth + x;
    odata[i] = (ushort)(p11 * dy1 * dx1 + p12 * dy1 * dx2 + p21 * dy2 * dx1 + p22 * dy2 * dx2);
}


        ";
        #endregion cl source code
        static ComputeKernel kernelBilinearInterpolatePixel;
        public static DeviceBuffer<ushort> AffineTransform(InterpolationMethod method, DeviceBuffer<ushort> cbData, Size srcSize, Size desSize
             , System.Drawing.Drawing2D.Matrix transform)
        {
            MyMatrix mytransform = new MyMatrix(transform);
            switch (method)
            {
                case InterpolationMethod.NearestNeibourgh:
                    throw new Exception("Not implemented yet");
                    break;

                case InterpolationMethod.Bilinear:
                    return BilinearInterpolateGPU(cbData, srcSize, desSize, mytransform);
                    break;

                case InterpolationMethod.Bicubic:
                    throw new Exception("Not implemented yet");
                    break;

                default:
                    throw new Exception("Invalid interpolation method");

            }
        }

        private unsafe static DeviceBuffer<ushort> BilinearInterpolateGPU(DeviceBuffer<ushort> cbData, Size srcSize, Size desSize, MyMatrix transform)
        {
            if (kernelBilinearInterpolatePixel == null)
            {
                //DeviceProgram.InitCL();
                DeviceProgram.Compile(srcBilinearInterpolate);
                kernelBilinearInterpolatePixel = DeviceProgram.CreateKernel("BilinearInterpolatePixel");
            }

            //Init buffer
#if DEBUG_METETIME
            DebugMeteTime dm = new DebugMeteTime();
#endif
            DeviceBuffer<ushort> cbIn = cbData;

#if DEBUG_METETIME
            dm.AddLine("ImageInterpolator : CreateHostBufferReadOnly intput");
#endif

            DeviceBuffer<ushort> cbOut = DeviceBuffer<ushort>.CreateBufferWriteOnly(desSize.Width*desSize.Height);

#if DEBUG_METETIME
            dm.AddLine("ImageInterpolator : CreateHostBufferReadOnly output");
#endif
            int newHeight = desSize.Height;
            int newWidth = desSize.Width;
            int srcWidth = srcSize.Width;
            int srcHeight = srcSize.Height;

            //need optimize here
            DeviceProgram.ExecuteKernel(kernelBilinearInterpolatePixel,
                new ComputeMemory[]{
                    cbIn,cbOut
                    ,DeviceBuffer<int>.CreateHostBufferReadOnly(new int[]{srcWidth,srcHeight,newWidth,newHeight})
                    ,DeviceBuffer<float>.CreateHostBufferReadOnly(transform._intMatrix.Elements)
                },
                new int[] { newWidth, newHeight }
                );

            DeviceProgram.Finish();

#if DEBUG_METETIME
            dm.AddLine("ImageInterpolator : kernelBilinearInterpolatePixel");
            dm.Write2Debug(true);
#endif

            if (cbData == null)
                cbIn.Dispose();

            return cbOut;
        }


#endif
#endregion GPU_SUPPORTED

    }
}
