#define DEBUG_METETIME_

#if GPU_SUPPORTED

/* for optimize 
GradientMapMatcherGPU Prepare :: 4768
GradientMapMatcherGPU kernelCalcGradientMap :: 991
GradientMapMatcherGPU kernelCalcGradientMap :: 412
GradientMapMatcherGPU cbPatternGradientMapEx.Read :: 12889
GradientMapMatcherGPU CalcPatternGradientMap :: 2727
GradientMapMatcherGPU CalcSamplePivotOffsets :: 6
GradientMapMatcherGPU CalcWeightMap :: 97
GradientMapMatcherGPU Divice Memory :: 49
GradientMapMatcherGPU CalcGradient :: 11190
GradientMapMatcherGPU ReadFromDeviceTo :: 794
GradientMapMatcherGPU 3For :: 17401
GradientMapMatcherGPU In Match :: 97
*/


using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Diagnostics;
using Cloo;
using System.Runtime.InteropServices;
using SiGlaz.Cloo;

namespace SIA.Algorithms.Preprocessing.Matching
{
    unsafe public class GradientMapMatcherGPU : GradientMapMatcher
    {
        int _nsample; //pattern length
        int _npattern;//sample lenght
        int _nps3;//pattern sample length 3 dimension
        int _b;
        int _r;
        int _nout;

        DeviceBuffer<ushort> cbPatternData = null;
        DeviceBuffer<ushort> cbSampleData= null;
        DeviceBuffer<double> cbPatternSampleGradientMap;
        //DeviceBuffer<double> cbPatternGradientMapEx;
        DeviceBuffer<double> cbOutput;

        static ComputeKernel kernelCalcGradient;
        static ComputeKernel kernelCalcGradientMap;
        //static ComputeKernel kernelCalcGradientMapEx;

        public DeviceBuffer<ushort> SampleDataDeviceBuffer { get; set; }

        public GradientMapMatcherGPU(
            int featureCount, int pivotCount)
            : base(featureCount, pivotCount)
        {

        }

        protected void Prepare()
        {

#if DEBUG_METETIME
            DebugMeteTime dm = new DebugMeteTime();
#endif

#if DEBUG_METETIME
            dm.AddLine(" GradientMapMatcherGPU.Compile :");
#endif
            _nsample = _sampleWidth * _sampleHeight;
            _npattern = _patternWidth * _patternHeight;
            _nps3 = 3 * _npattern + 3 * _nsample;

            cbPatternData = DeviceBuffer<ushort>.CreateHostBufferReadOnly(_npattern, (IntPtr)_patternData);

            cbSampleData = SampleDataDeviceBuffer;
            if (cbSampleData==null)
                cbSampleData = DeviceBuffer<ushort>.CreateHostBufferReadOnly(_nsample, (IntPtr)_sampleData);

#if DEBUG_METETIME
            
            dm.AddLine(" GradientMapMatcherGPU : Copy Data Device:" );
#endif

            //temp data
            //if (cbPatternSampleGradientMap == null)
            {
                cbPatternSampleGradientMap = DeviceBuffer<double>.CreateBufferReadWrite(_nps3);
            }

#if DEBUG_METETIME
            
            dm.AddLine(" GradientMapMatcherGPU : new PatternSampleGradientMap :" );
#endif

//            //output to calc indices
//            cbPatternGradientMapEx = DeviceBuffer<double>.CreateBufferReadWrite(_npattern);

//#if DEBUG_METETIME
            
//            dm.AddLine(" GradientMapMatcherGPU : new cbPatternGradientMapEx :" );
//#endif

            _r = _sampleWidth - _patternWidth - 2;
            _b = _sampleHeight - _patternHeight - 2;
            _nout = _b * _r;

            //if (cbOutput == null)
            {
                cbOutput = DeviceBuffer<double>.CreateBufferReadWrite(_nout * 2);
            }

#if DEBUG_METETIME
            
            dm.AddLine("GradientMapMatcherGPU : new _Output :" );
#endif

            //create kernel
            if (kernelCalcGradient == null)
            {
                //DeviceProgram.InitCL();
                DeviceProgram.Compile(src);
                kernelCalcGradient = DeviceProgram.CreateKernel("CalcGradient");
                kernelCalcGradientMap = DeviceProgram.CreateKernel("CalcGradientMap");
                //kernelCalcGradientMapEx = DeviceProgram.CreateKernel("CalcGradientMapEx");
            }

#if DEBUG_METETIME
            dm.AddLine("GradientMapMatcherGPU : CreateKernel :");
            dm.Write2Debug(true);
#endif
        }

        protected void FreeDeviceBuffer()
        {
            if(cbPatternData!=null)
            {
                cbPatternData.Dispose();
                cbPatternData = null;
            }

            if (SampleDataDeviceBuffer==null && cbSampleData != null)
            {
                cbSampleData.Dispose();
                cbSampleData = null;
            }

            if (cbPatternSampleGradientMap != null)
            {
                cbPatternSampleGradientMap.Dispose();
                cbPatternSampleGradientMap = null;
            }

            //if (cbPatternGradientMapEx != null)
            //{
            //    cbPatternGradientMapEx.Dispose();
            //    cbPatternGradientMapEx = null;
            //}

            if (cbOutput != null)
            {
                cbOutput.Dispose();
                cbOutput = null;
            }
           
        }

        unsafe protected override void Match(
          ref double confidence, ref int offsetX, ref int offsetY)
        {
            if(!DeviceProgram.GPUSupported)
            {
                base.Match(ref confidence, ref offsetX, ref offsetY);
                return;
            }


#if DEBUG_METETIME
            DebugMeteTime dm = new DebugMeteTime();
#endif

            Prepare();

#if DEBUG_METETIME
             dm.AddLine("GradientMapMatcherGPU Prepare :" );
#endif

            //CalcGradientMap(_sampleData, _sampleWidth, _sampleHeight, ref sampleGradientMap);
            DeviceProgram.ExecuteKernel(kernelCalcGradientMap,
                new ComputeMemory[]{ cbSampleData
                    , DeviceBuffer<int>.CreateHostBufferReadOnly(new int[]{_sampleWidth,_sampleHeight} )
                    ,cbPatternSampleGradientMap
                    , DeviceBuffer<int>.CreateHostBufferReadOnly(new int[]{_npattern*3} )
                }, new int[] { _sampleWidth - 2, _sampleHeight - 2 });
            DeviceProgram.Finish();

#if DEBUG_METETIME

            dm.AddLine("GradientMapMatcherGPU kernelCalcGradientMap :");
#endif

            int[] pivotIndices = null;

            //CalcPatternGradientMap(_patternData, _patternWidth, _patternHeight,ref patternGradientMap, out pivotIndices);
            //DeviceProgram.ExecuteKernel(kernelCalcGradientMapEx,
            //    new ComputeMemory[]{ cbPatternData
            //        , DeviceBuffer<int>.CreateHostBufferReadOnly(new int[]{_patternWidth,_patternHeight} )
            //        ,cbPatternSampleGradientMap
            //        ,cbPatternGradientMapEx
            //        , DeviceBuffer<int>.CreateHostBufferReadOnly(new int[]{0} )
            //    }, new int[] { _patternWidth - 2, _patternHeight - 2 });

            DeviceProgram.ExecuteKernel(kernelCalcGradientMap,
                new ComputeMemory[]{ cbPatternData
                    , DeviceBuffer<int>.CreateHostBufferReadOnly(new int[]{_patternWidth,_patternHeight} )
                    ,cbPatternSampleGradientMap
                    , DeviceBuffer<int>.CreateHostBufferReadOnly(new int[]{0} )
                }, new int[] { _patternWidth - 2, _patternHeight - 2 });

            DeviceProgram.Finish();


#if DEBUG_METETIME

            dm.AddLine("GradientMapMatcherGPU kernelCalcGradientMap :");
#endif

            //double[] gradientMapEx = cbPatternGradientMapEx.Read(null);
            double[] gradientMapEx = cbPatternSampleGradientMap.Read(_npattern * 2, _npattern, null);

#if DEBUG_METETIME
            dm.AddLine("GradientMapMatcherGPU cbPatternGradientMapEx.Read :");
#endif

            CalcPatternGradientMap(gradientMapEx, _patternWidth, _patternHeight, out pivotIndices);

#if DEBUG
            _pivotIndices = pivotIndices;
#endif

#if DEBUG_METETIME
            
            dm.AddLine("GradientMapMatcherGPU CalcPatternGradientMap :" );
#endif

            int nPivots = pivotIndices.Length;
            int[] samplePivotOffsets = CalcSamplePivotOffsets(pivotIndices);

#if DEBUG_METETIME
            
            dm.AddLine("GradientMapMatcherGPU CalcSamplePivotOffsets :" );
#endif

            // calc weight set
            _weightSet = CalcWeightMap(pivotIndices);

            double v, sqrIntensity;
            int index;

            BestList gradientBestList = new BestList(_pivotCount);
            BestList intensityBestList = new BestList(_pivotCount);
            BestList complexBestList = new BestList(_pivotCount);

#if DEBUG_METETIME
            
            dm.AddLine("GradientMapMatcherGPU CalcWeightMap :" );
#endif

            int[] pivotIndicesList = new int[pivotIndices.Length + samplePivotOffsets.Length];
            Array.Copy(pivotIndices, 0, pivotIndicesList, 0, pivotIndices.Length);
            Array.Copy(samplePivotOffsets, 0, pivotIndicesList, pivotIndices.Length, samplePivotOffsets.Length);

            DeviceBuffer<int> vPivotIncides = DeviceBuffer<int>.CreateHostBufferReadOnly(pivotIndicesList);

#if DEBUG_METETIME
            
            dm.AddLine("GradientMapMatcherGPU Divice Memory :" );
#endif

            DeviceProgram.ExecuteKernel(kernelCalcGradient,
                new ComputeMemory[]{
                    cbPatternData,cbSampleData,cbPatternSampleGradientMap,vPivotIncides,cbOutput
                        //sample offset,p0,p1,p2,s0,s1,s2,pivotoffset,sample width,povot number,output offset
                        , DeviceBuffer<int>.CreateHostBufferReadOnly(new int[]{ 0,_npattern,2*_npattern,3*_npattern,3*_npattern + _nsample,3*_npattern + 2*_nsample, pivotIndices.Length,_sampleWidth,nPivots,_nout})
                }, new int[] { _r, _b });
            DeviceProgram.Finish();
            

#if DEBUG_METETIME
            
            dm.AddLine("GradientMapMatcherGPU CalcGradient :" );
#endif

            double[] _Output = cbOutput.Read(null);

#if DEBUG_METETIME
            
            dm.AddLine("GradientMapMatcherGPU ReadFromDeviceTo :" );
#endif

            int indexzero;
            for (int y = 1; y <= _b; y++)
            {
                index = y * _sampleWidth + 1;
                indexzero = (y - 1) * _r;
                for (int x = 1; x <= _r; x++, index++, indexzero++)
                {
                    v = _Output[indexzero];
                    sqrIntensity = _Output[indexzero + _nout];

                    gradientBestList.Add(v, index);
                    intensityBestList.Add(sqrIntensity, index);
                    complexBestList.Add(v * Math.Sqrt(sqrIntensity), index);
                }
            }
            _Output = null;

#if DEBUG_METETIME
            
            dm.AddLine("GradientMapMatcherGPU 3For :" );
#endif

            List<int> tmp = new List<int>();
            Hashtable lookupTable = new Hashtable(_pivotCount * 3);

            foreach (int bestIndice in gradientBestList.Indices)
            {
                if (lookupTable.ContainsKey(bestIndice))
                    continue;

                lookupTable.Add(bestIndice, bestIndice);

                tmp.Add(bestIndice);
            }
            foreach (int bestIndice in intensityBestList.Indices)
            {
                if (lookupTable.ContainsKey(bestIndice))
                    continue;

                lookupTable.Add(bestIndice, bestIndice);

                tmp.Add(bestIndice);
            }
            foreach (int bestIndice in complexBestList.Indices)
            {
                if (lookupTable.ContainsKey(bestIndice))
                    continue;

                lookupTable.Add(bestIndice, bestIndice);

                tmp.Add(bestIndice);
            }
            lookupTable.Clear(); lookupTable = null;
            _bestIndices = tmp.ToArray();

            int nInterestedPoints = _bestIndices.Length;
            _interestedXList = new int[nInterestedPoints];
            _interestedYList = new int[nInterestedPoints];

            for (int i = 0; i < nInterestedPoints; i++)
            {
                _interestedXList[i] = _bestIndices[i] % _sampleWidth;
                _interestedYList[i] = _bestIndices[i] / _sampleWidth;
            }

            confidence = 1.0;
            int bestIndx = 0;

            offsetX = _interestedXList[bestIndx];
            offsetY = _interestedYList[bestIndx];
            offsetX = (int)(offsetX + _patternWidth * 0.5 - _sampleWidth * 0.5);
            offsetY = (int)(offsetY + _patternHeight * 0.5 - _sampleHeight * 0.5);
#if DEBUG_METETIME
            
            dm.AddLine("GradientMapMatcherGPU In Match :" );
            dm.Write2Debug(true);
#endif

            FreeDeviceBuffer();
        }


        unsafe protected void CalcPatternGradientMap(double[] gradientMapEx,
            int width, int height, out int[] pivotIndices)
        {
            int b = height - 2;
            int r = width - 2;
            double vc;
            ushort* tmp1 = null, tmp2 = null, tmp3 = null;
            double* pG0 = null, pG1 = null, pG2 = null;

            int n = b * r;
            int[] vals = new int[n];
            double[] keys = new double[n];

            int i = 0;
            for (int y = 1; y <= b; y++)
            {
                int index = y * width + 1;
                for (int x = 1; x <= r; x++)
                {
                    vc = gradientMapEx[index];
                    keys[i] = vc;
                    vals[i++] = index;
                    index++;
                }
            }

            Array.Sort(keys, vals);


            pivotIndices = new int[_featureCount];
            for (i = 0; i < _featureCount; i++)
            {
                pivotIndices[i] = vals[n - i - 1];
            }

            keys = null;
            vals = null;


            //BestList bestList = new BestList(_featureCount);
            //int b = height - 2;
            //int r = width - 2;
            //double vc;
            //ushort* tmp1 = null, tmp2 = null, tmp3 = null;
            //double* pG0 = null, pG1 = null, pG2 = null;

            //for (int y = 1; y <= b; y++)
            //{
            //    int index = y * width + 1;
            //    for (int x = 1; x <= r; x++)
            //    {
            //        vc = gradientMapEx[index];
            //        bestList.Add(vc, index);
            //        index++;
            //    }
            //}
            //pivotIndices = bestList.Indices;
        }


        private const string src = @"
#pragma OPENCL EXTENSION cl_khr_fp64 : enable
//#pragma OPENCL EXTENSION cl_khr_byte_addressable_store : enable

__kernel void CalcGradient(
__global __read_only ushort* pdata, //pattern data
__global __read_only ushort* sdata, //sample data
__global __read_only double* psgrad, //pattern sample gradient
__global __read_only int* pivotIdx,//Pivot Indices, Pivot Offset Indices
__global double* out,// output
__constant int* offset //p0,p1,p2,s0,s1,s2,pivotoffset,sample width,povot number,output offset

)
{
  int x = get_global_id(0);
  int y = get_global_id(1);
  int w = get_global_size(0);
   
  double v,m,e,sqrIntensity, diffIntensity;
  int sampleIndex,patternIndex;
  
  int p0 = offset[0];
  int p1 = offset[1];
  int p2 = offset[2];
  int s0 = offset[3];
  int s1 = offset[4];
  int s2 = offset[5];
  int pivotoff = offset[6];
  int sw = offset[7];
  int pn = offset[8];

  double2 g0,g1;
 
  v = 0;
  sqrIntensity = 0;

  int index = (y + 1) * sw + (x + 1);
  for(int i=0;i<pn;i++)
  {
    sampleIndex = index + pivotIdx[pivotoff + i]; //need localize
    patternIndex = pivotIdx[i];// need localize

    g0.y = psgrad[patternIndex + p0];
    g0.x = psgrad[patternIndex + p1];
    g1.y = psgrad[sampleIndex + s0];
    g1.x = psgrad[sampleIndex + s1];
    
    m = psgrad[patternIndex + p2] + psgrad[sampleIndex + s2];
    e = m*0.5f - distance(g0,g1);//fast_distance
   
    v += e;

    diffIntensity = 255 - abs(sdata[sampleIndex] - pdata[patternIndex]);
    sqrIntensity = diffIntensity * diffIntensity + sqrIntensity;
  }
  
  index = y* w + x;
  out[index] = v;
  out[index + offset[9] ] = sqrIntensity ;
}

__kernel void CalcGradientMap(
__global __read_only ushort* psdata, //pattern data/sample data
__constant int *psize,// pattern w, pattern h
__global __write_only double* psgrad, //pattern sample gradient
__constant int* poffset //psgrad offset
)
{
  int x = get_global_id(0);
  int y = get_global_id(1);
  
  int w = psize[0];
  int h = psize[1];
  int n = w*h;
  
  int i1 = y * w + x;
  int i2 = i1 + w;
  int i3 = i2 + w;

  //int iout0 = poffset[0] + (y+1)*w + x + 1;
  int iout0 = poffset[0] + i1 + w +  1;
  int iout1 = iout0 + n;
  int iout2 = iout1 + n;

  //ushort tmp1, tmp2 , tmp3 ;
  double gx, gy;  
 
  gy = psdata[i2] + psdata[i2+1] + psdata[i2+2] - psdata[i1] - psdata[i1+1] - psdata[i1+2];
  gx = psdata[i1+2] + psdata[i2+2] + psdata[i3+2] - psdata[i1] - psdata[i2] - psdata[i3];
      
  psgrad[iout0] = gy;
  psgrad[iout1] = gx;
  psgrad[iout2] = sqrt(gx * gx + gy * gy);
} 

/*
__kernel void CalcGradientMapEx(
__global __read_only ushort* psdata, //pattern data, sample data
__constant int *psize,// pattern w, pattern h
__global __write_only double* psgrad, //pattern sample gradient
__global __write_only double* psgradex, //sample sqr
__constant int* poffset //psdata offset,psgrad offset
)
{
  int x = get_global_id(0);
  int y = get_global_id(1);
  
  int w = psize[0];
  int h = psize[1];
  int n = w*h;

  int i1 = y * w + x;
  int i2 = i1 + w;
  int i3 = i2 + w;

  //int i0 = (y+1)*w + x + 1;
  int i0 = i1 + w + 1;
  int iout0 = poffset[0] + i0;
  int iout1 = iout0 + n;
  int iout2 = iout1 + n;

  //ushort tmp1, tmp2 , tmp3 ;
  double gx, gy;
  
  gy = psdata[i2] + psdata[i2+1] + psdata[i2+2] - psdata[i1] - psdata[i1+1] - psdata[i1+2];
  gx = psdata[i1+2] + psdata[i2+2] + psdata[i3+2] - psdata[i1] - psdata[i2] - psdata[i3];

  double sqrMagnitude = gx * gx + gy * gy;

  psgrad[iout0] = gy;
  psgrad[iout1] = gx;
  psgrad[iout2] = sqrt(sqrMagnitude);
  psgradex[i0] = sqrMagnitude;
} 
*/


";

    }
}

#endif