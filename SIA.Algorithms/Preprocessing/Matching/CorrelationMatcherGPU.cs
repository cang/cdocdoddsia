#define DEBUG_METETIME_

#if GPU_SUPPORTED

/* for optimize
CorrelationMatcherGPU :  InitCL  Compile CreateKernel:: 1
CorrelationMatcherGPU :  init cbInterested:: 3
CorrelationMatcherGPU :  cbInterested:: 63
CorrelationMatcherGPU :  cbOp1:: 11
CorrelationMatcherGPU :  cbOp2:: 1
CorrelationMatcherGPU :  cbWeigthSet:: 27
CorrelationMatcherGPU :  cbRet:: 10
CorrelationMatcherGPU :  kernelCorrelationCoefficientWeightSetRow:: 1444
CorrelationMatcherGPU :  Read:: 1063
CorrelationMatcherGPU :  new double:: 602
CorrelationMatcherGPU :  Dispose:: 123
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SiGlaz.Algorithms.Core;
using SiGlaz.Common.Pattern;
using SiGlaz.Cloo;
using Cloo;
using System.Diagnostics;

namespace SIA.Algorithms.Preprocessing.Matching
{
    public class CorrelationMatcherGPU : CorrelationMatcher
    {
        public DeviceBuffer<ushort> SampleDataDeviceBuffer { get; set; }

        public CorrelationMatcherGPU(CorrelationPattern corrPtn)
            : base(corrPtn)
        {
        }

        public CorrelationMatcherGPU()
        {
        }

        static ComputeKernel kernelCorrelationCoefficientWeightSetRow;
        unsafe public override double[,] CorrelationCoefficientWeightSet(ushort[] op1, ushort[] op2, int stride1, int stride2, double[] weightset, int[] interestedX, int[] interestedY)
        {
            if(!DeviceProgram.GPUSupported)
                return CorrelationCoefficientWeightSet(op1, op2, stride1, stride2, weightset, interestedX, interestedY);

            int op2l;
            if (SampleDataDeviceBuffer != null)
                op2l = (int)SampleDataDeviceBuffer.Count;
            else
                op2l = op2.Length;

            int h1 = op1.Length / stride1;
            int h2 = op2l / stride2;

            int rw = stride2 - stride1;
            int rh = h2 - h1;

            if (h1 == 0 || h2 < h1 || stride2 < stride1)
                throw new System.ArgumentException("Operand size is invalid");

#if DEBUG_METETIME
            DebugMeteTime dm = new DebugMeteTime();
#endif
            if (kernelCorrelationCoefficientWeightSetRow == null)
            {
                //DeviceProgram.InitCL();
                DeviceProgram.Compile(src);
                kernelCorrelationCoefficientWeightSetRow = DeviceProgram.CreateKernel("CorrelationCoefficientWeightSetRow");
            }
#if DEBUG_METETIME
            dm.AddLine("CorrelationMatcherGPU :  InitCL  Compile CreateKernel:" );
#endif
            int len = interestedX.Length,j=0;
            int[] interested = new int[len*2];
            for (int i = 0; i < len; i++)
            {
                interested[j++] = interestedX[i];
                interested[j++] = interestedY[i];
            }

#if DEBUG_METETIME
            dm.AddLine("CorrelationMatcherGPU :  init cbInterested:" );
#endif            
            DeviceBuffer<int> cbInterested = DeviceBuffer<int>.CreateHostBufferReadOnly(interested);
#if DEBUG_METETIME
            dm.AddLine("CorrelationMatcherGPU :  cbInterested:" );
#endif

            DeviceBuffer<ushort> cbOp1 = DeviceBuffer<ushort>.CreateHostBufferReadOnly(op1);
#if DEBUG_METETIME
            dm.AddLine("CorrelationMatcherGPU :  cbOp1:" );
#endif
            DeviceBuffer<ushort> cbOp2 = SampleDataDeviceBuffer;
            if(cbOp2==null)
                cbOp2 = DeviceBuffer<ushort>.CreateHostBufferReadOnly(op2);

#if DEBUG_METETIME
            dm.AddLine("CorrelationMatcherGPU :  cbOp2:" );
#endif
            DeviceBuffer<double> cbWeigthSet = DeviceBuffer<double>.CreateHostBufferReadOnly(weightset);
#if DEBUG_METETIME
            dm.AddLine("CorrelationMatcherGPU :  cbWeigthSet:" );
#endif
            DeviceBuffer<double> cbRet1 = DeviceBuffer<double>.CreateBufferWriteOnly(len*h1*5);
#if DEBUG_METETIME
            dm.AddLine("CorrelationMatcherGPU :  cbRet:" );
#endif

            DeviceProgram.ExecuteKernel(kernelCorrelationCoefficientWeightSetRow
                , new ComputeMemory[]{
                    cbInterested
                    ,cbOp1,DeviceBuffer<int>.CreateHostBufferReadOnly(new int[]{op1.Length})
                    ,cbOp2,DeviceBuffer<int>.CreateHostBufferReadOnly(new int[]{op2l})
                    ,DeviceBuffer<int>.CreateHostBufferReadOnly(new int[]{stride1})
                    ,DeviceBuffer<int>.CreateHostBufferReadOnly(new int[]{stride2})
                    ,cbWeigthSet
                    ,cbRet1
                }, new int[] { len, h1 });

            DeviceProgram.Finish();

#if DEBUG_METETIME
            dm.AddLine("CorrelationMatcherGPU :  kernelCorrelationCoefficientWeightSetRow:");
#endif

            double[] ret1 = cbRet1.Read(null);


#if DEBUG_METETIME
            dm.AddLine("CorrelationMatcherGPU :  Read:" );
#endif
            double[,] result = new double[h2 - h1, stride2 - stride1];

            int N = op1.Length;
            for (int i = 0; i < len; i++)
            {
                int idx = i * h1 * 5;
                double Sx = 0, Sy = 0, Sxx = 0, Syy = 0, Sxy = 0;
                for (int ii = 0; ii < h1; ii++)
                {
                    Sx += ret1[idx++];
                    Sy += ret1[idx++];
                    Sxx += ret1[idx++];
                    Syy += ret1[idx++];
                    Sxy += ret1[idx++];
                }

                Sx /= N;
                Sy /= N;
                Sxx -= N * Sx * Sx;
                Syy -= N * Sy * Sy;
                Sxy -= N * Sx * Sy;

                result[interestedY[i], interestedX[i]] = Sxy / Math.Sqrt(Sxx * Syy);
            }

#if DEBUG_METETIME
            dm.AddLine("CorrelationMatcherGPU :  new double:" );
#endif

            //Free Device Buffer
            cbRet1.Dispose();
            cbWeigthSet.Dispose();
            cbOp1.Dispose();

            if (SampleDataDeviceBuffer == null && cbOp2!=null)
                cbOp2.Dispose();

            cbInterested.Dispose();


#if DEBUG_METETIME
            dm.AddLine("CorrelationMatcherGPU :  Dispose:" );
            dm.Write2Debug(true);
#endif
            return result;
        }

        private const string src = @"

#pragma OPENCL EXTENSION cl_khr_fp64 : enable
//#pragma OPENCL EXTENSION cl_khr_byte_addressable_store : enable

__kernel void CorrelationCoefficientWeightSetRow( 
__global __read_only int2* interested
,__global __read_only ushort* op1
,__global int* lo1
,__global __read_only ushort* op2
,__global int* lo2
,__global int* stride1
,__global int* stride2
,__global __read_only double* weightset
,__global __write_only double* ret
)
{
    int idx = get_global_id(0);
    int i  = get_global_id(1);
    
	int left = interested[idx].x;
	int top = interested[idx].y;

    int w1 = *stride1;
    int w2 = *stride2;

	int h1 = *lo1 / w1;
	double Sx = 0;
	double Sy = 0;
	double Sxx = 0;
	double Sxy = 0;
	double  Syy = 0;
	double  v1 = 0;
	double  v2 = 0;
	double ww = 0;

    int lIndex1 = i*w1;
    int lIndex2 = (i + top) * w2 + left;
	for (int j=0; j < w1; j++, lIndex1++, lIndex2++)
	{
		v1 = op1[lIndex1];
		v2 = op2[lIndex2];
		ww = weightset[lIndex1];

		Sx += v1*ww;
		Sy += v2*ww;
		Sxy += v1 * v2;
		Sxx += v1 * v1;
		Syy += v2 * v2;
    }
       
    i = (h1*idx + i)*5;
    ret[i++] = Sx;
    ret[i++] = Sy;
    ret[i++] = Sxx;
    ret[i++] = Syy;
    ret[i++] = Sxy;
}



";
    }
}


#endif