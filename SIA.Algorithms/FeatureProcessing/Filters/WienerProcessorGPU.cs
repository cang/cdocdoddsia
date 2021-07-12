#define DEBUG_METETIME_

#if GPU_SUPPORTED

using System;
using System.Collections.Generic;
using System.Text;
using SIA.SystemFrameworks;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using SiGlaz.Cloo;
using Cloo;

namespace SIA.Algorithms.FeatureProcessing.Filters
{
    unsafe public class WienerProcessorGPU : WienerProcessor, IDisposable
    {
        //temp members
        int _imageLength;
        DeviceBuffer<ushort> cbData = null;
        DeviceBuffer<double> cbIntegral;
        DeviceBuffer<double> cbSIntegral;

        static ComputeKernel kernelCalcImageIntegralsRow;
        static ComputeKernel kernelCalcImageIntegralsCol;
        static ComputeKernel kernelWienerFilterPixel;
        static ComputeKernel kernelCalcNoisePowerCol;

        bool availableData = false;

        public WienerProcessorGPU(
            ushort* imageData, int width, int height,
            int kernelWidth, int kernelHeight)
            : base(imageData,width,height,kernelWidth,kernelHeight)
        {
            _imageLength = width * height;
        }

        public WienerProcessorGPU(
           ushort* imageData,DeviceBuffer<ushort> deviceBuff, int width, int height,
           int kernelWidth, int kernelHeight)
            : base(imageData, width, height, kernelWidth, kernelHeight)
        {
            _imageLength = width * height;
            availableData = deviceBuff != null;
            cbData = deviceBuff;
        }

        private void NewDeviceBuffer()
        {

#if DEBUG_METETIME
            DebugMeteTime dm = new DebugMeteTime();
#endif
            if (!availableData)
                cbData = DeviceBuffer<ushort>.CreateHostBufferReadWrite(_imageLength, (IntPtr)_imageData);
            //cbData.Write(0, _imageLength, (IntPtr)_imageData, null);

#if DEBUG_METETIME
            dm.AddLine("WienerProcessorGPU : Data");
#endif
            cbIntegral = DeviceBuffer<double>.CreateBufferReadWrite(_imageLength);
            cbSIntegral = DeviceBuffer<double>.CreateBufferReadWrite(_imageLength);

#if DEBUG_METETIME
            dm.AddLine("WienerProcessorGPU : Temp Data");
            dm.Write2Debug(true);
#endif

            if (kernelWienerFilterPixel==null)
            {
                //DeviceProgram.InitCL();
                DeviceProgram.Compile(src);
                kernelCalcImageIntegralsRow = DeviceProgram.CreateKernel("CalcImageIntegralsRow");
                kernelCalcImageIntegralsCol = DeviceProgram.CreateKernel("CalcImageIntegralsCol");
                kernelWienerFilterPixel = DeviceProgram.CreateKernel("WienerFilterPixel");
                kernelCalcNoisePowerCol = DeviceProgram.CreateKernel("CalcNoisePowerCol");
            }

        }

        protected override void CalcImageIntegrals()
        {
            if (!DeviceProgram.GPUSupported)
            {
                base.CalcImageIntegrals();
                return;
            }

#if DEBUG_METETIME
            DebugMeteTime dm = new DebugMeteTime();
#endif

            //Need Optimize more this Kernal 
            DeviceProgram.ExecuteKernel( kernelCalcImageIntegralsRow, new ComputeMemory[] { 
                cbData
               ,cbIntegral
               ,cbSIntegral 
               , DeviceBuffer<int>.CreateHostBufferReadOnly(new int[]{_imageWidth,_imageHeight})
                }, _imageHeight);
            DeviceProgram.Finish();

#if DEBUG_METETIME
            dm.AddLine("WienerProcessorGPU : kernelCalcImageIntegralsRow");
#endif

            DeviceProgram.ExecuteKernel(kernelCalcImageIntegralsCol, new ComputeMemory[] { cbIntegral, cbSIntegral 
                , DeviceBuffer<int>.CreateHostBufferReadOnly(new int[]{_imageWidth,_imageHeight})}
                , _imageWidth);
            DeviceProgram.Finish();

#if DEBUG_METETIME
            dm.AddLine("WienerProcessorGPU : kernelCalcImageIntegralsCol");
            dm.Write2Debug(true);
#endif
        }

        public override void Filter(bool isAuto, double noiseLevel)
        {
            if(!DeviceProgram.GPUSupported)
            {
                base.Filter(isAuto, noiseLevel);
                return;
            }

            NewDeviceBuffer();

#if DEBUG_METETIME
            DebugMeteTime dm = new DebugMeteTime();
#endif

            CalcImageIntegrals();

#if DEBUG_METETIME
            dm.AddLine("WienerProcessorGPU : CalcImageIntegrals");
#endif

            double noisePower = noiseLevel*255*255;
            if (isAuto)
                noisePower = CalcNoisePower();

#if DEBUG_METETIME
            dm.AddLine("WienerProcessorGPU : CalcNoisePower");
#endif

            FilterPixel(noiseLevel, noisePower);

#if DEBUG_METETIME
            dm.AddLine("WienerProcessorGPU : Filter");
            dm.Write2Debug(true);
#endif

            FreeDeviceBuffer();
        }

        private void FilterPixel(double noiseLevel, double noisePower)
        {
            //prepare var
            int kHalfWidth = _kernelWidth >> 1;
            int kHlfHeight = _kernelHeight >> 1;
            int xStart = kHalfWidth + 1;
            int yStart = kHlfHeight + 1;
            int xEnd = _imageWidth - kHalfWidth - 1;
            int yEnd = _imageHeight - kHlfHeight - 1;

            int kernelLength = _kernelWidth * _kernelHeight;
            int refIndxLT = -kHlfHeight * _imageWidth - kHalfWidth;
            int refIndxRT = refIndxLT + _kernelWidth - 1;
            int refIndxLB = -refIndxRT;
            int refIndxRB = -refIndxLT;

            refIndxLT -= _imageWidth + 1;
            refIndxRT -= _imageWidth;
            refIndxLB--;

#if DEBUG_METETIME
            DebugMeteTime dm = new DebugMeteTime();
#endif

            //call process
            DeviceProgram.ExecuteKernel(kernelWienerFilterPixel,
                new ComputeMemory[]{
                    cbData,cbIntegral,cbSIntegral,
                    DeviceBuffer<int>.CreateHostBufferReadOnly(new int[]{_imageWidth,_imageHeight}),
                    DeviceBuffer<int>.CreateHostBufferReadOnly(new int[]{xStart,xEnd,yStart,yEnd}),
                    DeviceBuffer<int>.CreateHostBufferReadOnly(new int[]{refIndxLT,refIndxRT,refIndxLB,refIndxRB}),
                    DeviceBuffer<int>.CreateHostBufferReadOnly(new int[]{kernelLength}),
                    DeviceBuffer<double>.CreateHostBufferReadOnly(new double[]{noisePower})
                }, new int[] { _imageWidth, _imageHeight });
            DeviceProgram.Finish();


#if DEBUG_METETIME
            dm.AddLine("WienerProcessorGPU : kernelWienerFilterPixel");
#endif

            //vData.ReadFromDeviceTo(lpData);
            //read data
            if (!availableData && cbData != null)
                cbData.Read( (IntPtr)_imageData, null);


#if DEBUG_METETIME
            dm.AddLine("WienerProcessorGPU : Read kernelWienerFilterPixel");
            dm.Write2Debug(true);
#endif

        }

        protected override double CalcNoisePower()
        {
            if (!DeviceProgram.GPUSupported)
            {
                return base.CalcNoisePower();
            }

#if DEBUG_METETIME
            DebugMeteTime dm = new DebugMeteTime();
#endif

            //prepare var
            int kHalfWidth = _kernelWidth >> 1;
            int kHlfHeight = _kernelHeight >> 1;
            int xStart = kHalfWidth + 1;
            int yStart = kHlfHeight + 1;
            int xEnd = _imageWidth - kHalfWidth - 1;
            int yEnd = _imageHeight - kHlfHeight - 1;

            int kernelLength = _kernelWidth * _kernelHeight;
            int refIndxLT = -kHlfHeight * _imageWidth - kHalfWidth;
            int refIndxRT = refIndxLT + _kernelWidth - 1;
            int refIndxLB = -refIndxRT;
            int refIndxRB = -refIndxLT;

            refIndxLT -= _imageWidth + 1;
            refIndxRT -= _imageWidth;
            refIndxLB--;

            //DeviceBuffer<double> bufNoise = DeviceBuffer<double>.CreateBufferReadWrite(_imageHeight);
            DeviceBuffer<double> bufNoise = DeviceBuffer<double>.CreateBufferReadWrite(_imageWidth);

#if DEBUG_METETIME
            dm.AddLine("WienerProcessorGPU : bufNoise");
#endif

            //call process
            DeviceProgram.ExecuteKernel(kernelCalcNoisePowerCol,
               new ComputeMemory[]{
                    cbIntegral,cbSIntegral,bufNoise,
                    DeviceBuffer<int>.CreateHostBufferReadOnly(new int[]{_imageWidth,_imageHeight})
                    ,DeviceBuffer<int>.CreateHostBufferReadOnly(new int[]{xStart,xEnd,yStart,yEnd})
                    ,DeviceBuffer<int>.CreateHostBufferReadOnly(new int[]{refIndxLT,refIndxRT,refIndxLB,refIndxRB})
                    ,DeviceBuffer<int>.CreateHostBufferReadOnly(new int[]{kernelLength})
                }, _imageWidth);
            DeviceProgram.Finish();

#if DEBUG_METETIME
            dm.AddLine("WienerProcessorGPU : kernelCalcNoisePowerCol");
#endif

            double[] lpRet = bufNoise.Read(null);
           
            double noisePower = 0;
            //for (int i = yStart; i <= yEnd; i++)
            for (int i = xStart; i <= xEnd; i++)
            {
                noisePower += lpRet[i];
            }

            //lpRet = null;
            //vNoise.Dispose(); vNoise = null;
            //bufNoise.Dispose(); bufNoise = null;

#if DEBUG_METETIME
            dm.AddLine("WienerProcessorGPU : vNoise Read");
            dm.Write2Debug(true);
#endif

            return noisePower / (_imageWidth * _imageHeight);
        }

        #region IDisposable Members

        ~WienerProcessorGPU()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            GC.KeepAlive(this);
        }

        protected void FreeDeviceBuffer()
        {
            //private byte[] lpData;
            if (!availableData && cbData != null)
            {
                cbData.Dispose();
                cbData = null;
            }

            if (cbIntegral != null)
            {
                cbIntegral.Dispose();
                cbIntegral = null;
            }

            if (cbSIntegral != null)
            {
                cbSIntegral.Dispose();
                cbSIntegral = null;
            }
        }

        //protected abstract void Dispose(bool manual);
        /// <summary>
        /// Releases the associated OpenCL object.
        /// </summary>
        /// <param name="manual"> Specifies the operation mode of this method. </param>
        /// <remarks> <paramref name="manual"/> must be <c>true</c> if this method is invoked directly by the application. </remarks>
        protected void Dispose(bool manual)
        {
            if (manual)
            {
                FreeDeviceBuffer();
            }
        }

        #endregion

        private const string src = @"
#pragma OPENCL EXTENSION cl_khr_fp64 : enable

__kernel void CalcImageIntegralsRow(
__global __read_only ushort* data
,__global __write_only double* sum
,__global __write_only double *ssum
,__constant int* psize)
{
   int y = get_global_id(0);
   int w = psize[0]; 

   int index = y*w;
   int maxindex = index + w;

   //first column      
   double intensity = data[index];
   double prevsum = intensity;
   double prevsssum = intensity*intensity;

   sum[index] = prevsum;
   ssum[index] = prevsssum;

   index++;
   for (;index<maxindex;index++)
   {
      intensity = data[index];
      prevsum+=intensity;
      prevsssum+=intensity*intensity;
      sum[index] = prevsum;
      ssum[index] = prevsssum;
   }
}

__kernel void CalcImageIntegralsCol(
__global double* sum
,__global double *ssum
,__constant int* psize)
{
   int x = get_global_id(0);
   int w = psize[0];
   int h = psize[1];
   int index;
   int i;
   
   for(int y=1;y<h;y++)
   {
        index = y*w+x;
        i = index - w;
        sum[index] += sum[i];
        ssum[index] += ssum[i];
   }
}

__kernel void WienerFilterPixel(
__global ushort* data
,__global __read_only double* sum
,__global __read_only double *ssum,
__constant int* psize,
__constant int* prect,
__constant int* prectkernal,
__constant int* pKernelLength,
__constant double* pnoisePower)
{
   int x = get_global_id(0);
   int y = get_global_id(1);
   int w = psize[0];
   
   int xStart = prect[0];
   int xEnd = prect[1];
   int yStart = prect[2];
   int yEnd = prect[3];
   int refIndxLT = prectkernal[0];
   int refIndxRT = prectkernal[1];
   int refIndxLB = prectkernal[2];
   int refIndxRB = prectkernal[3];
   int kernelLength= pKernelLength[0];
   double noisePower=pnoisePower[0];
   
   if( x < xStart 
   || x > xEnd  
   || y < yStart 
   || y > yEnd) return;
 
   
   int index;
   
   double localMean, localVar;
   double intensity, newIntensity;
   
   index = y * w + x;
   //for (x = xStart ; x <= xEnd ; x++, index++)
   {
    // calc local mean
     localMean = sum[index + refIndxRB] -
                                sum[index + refIndxRT] -
                                sum[index + refIndxLB] +
                                sum[index + refIndxLT];
     localMean /= kernelLength;
                    
     // calc local varianc
     localVar = ssum[index + refIndxRB] -
                                ssum[index + refIndxRT] -
                                ssum[index + refIndxLB] +
                                ssum[index + refIndxLT];
     localVar /= kernelLength;
     localVar -= localMean * localMean;
   
     // apply filter
     intensity = data[index];
     newIntensity = intensity - localMean;
     
     intensity = (localVar > noisePower ? (localVar - noisePower) : 0);
     
     if (localVar < noisePower)
       localVar = noisePower;
       
     newIntensity = intensity * (newIntensity / localVar) + localMean;
     data[index] = (short)newIntensity;//ushort
   }
}


__kernel void CalcNoisePowerCol(
__global __read_only double* sum
,__global __read_only double *ssum
,__global __write_only double *pnoisePower
,__constant int* psize
,__constant int* prect
,__constant int* prectkernal
,__constant int* pKernelLength)
{
   int x = get_global_id(0);
   int w = psize[0];
   int h = psize[1];
   
   int xStart = prect[0];
   int xEnd = prect[1];
   int yStart = prect[2];
   int yEnd = prect[3];
   int refIndxLT = prectkernal[0];
   int refIndxRT = prectkernal[1];
   int refIndxLB = prectkernal[2];
   int refIndxRB = prectkernal[3];
   int kernelLength= pKernelLength[0];
   double noisePower=0;
   
   if( x < xStart 
   || x > xEnd) return;
   
   double localMean, localVar;
   int index = yStart * w + x;
   for (int y = yStart ; y <= yEnd ; y++, index+=w )
   {
    // calc local mean
     localMean = sum[index + refIndxRB] -
                                sum[index + refIndxRT] -
                                sum[index + refIndxLB] +
                                sum[index + refIndxLT];
     localMean /= kernelLength;
                    
     // calc local varianc
     localVar = ssum[index + refIndxRB] -
                                ssum[index + refIndxRT] -
                                ssum[index + refIndxLB] +
                                ssum[index + refIndxLT];
     localVar /= kernelLength;
     
     noisePower += (localVar - localMean * localMean);
   }
   
   pnoisePower[x] = noisePower;
}


";


    }
}

#endif