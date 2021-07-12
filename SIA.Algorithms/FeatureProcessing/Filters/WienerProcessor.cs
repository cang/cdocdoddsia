using System;
using System.Collections.Generic;
using System.Text;
using SIA.SystemFrameworks;

namespace SIA.Algorithms.FeatureProcessing.Filters
{
    unsafe public class WienerProcessor
    {
        protected int _kernelWidth = 3;
        protected int _kernelHeight = 3;

        protected int _imageWidth = 0;
        protected int _imageHeight = 0;
        protected ushort* _imageData = null;

        protected const double max_intensity = 255;

        protected double[] _imageIntegral = null;
        protected double[] _squareImageIntegral = null;


        public WienerProcessor(
            ushort* imageData, int width, int height, 
            int kernelWidth, int kernelHeight)
        {
            _imageData = imageData;
            _imageWidth = width;
            _imageHeight = height;
            _kernelWidth = kernelWidth;
            _kernelHeight = kernelHeight;
        }

        protected virtual void CalcImageIntegrals()
        {
            // get data's length
            int width = _imageWidth;
            int height = _imageHeight;
            int length = width * height;
            double inv_max_intensity = 1.0 / max_intensity;

            // create integral image buffer
            _imageIntegral = new double[length];
            _squareImageIntegral = new double[length];

            // temporate variables
            int x, y, index;

            ushort* pSrc = _imageData;
            double intensity = 0;

            // calculate integral image
            fixed (double* pI1 = _imageIntegral, pI2 = _squareImageIntegral)
            {
                // first item
                // normalize intensity
                intensity = pSrc[0] * inv_max_intensity;
                pI1[0] = intensity;
                pI2[0] = intensity * intensity;

                // first line
                for (index = 1; index < width; index++)
                {
                    // normalize intensity
                    intensity = pSrc[index] * inv_max_intensity;

                    // calc integral
                    pI1[index] = pI1[index - 1] + intensity;
                    pI2[index] = pI2[index - 1] + intensity * intensity;
                }

                // first column
                for (index = width, y = 1; y < height; y++, index += width)
                {
                    // normalize intensity
                    intensity = pSrc[index] * inv_max_intensity;

                    // calc integral
                    pI1[index] = pI1[index - width] + intensity;
                    pI2[index] = pI2[index - width] + intensity * intensity;
                }

                // remains
                int x_1y_1 = -width - 1;
                int xy_1 = -width;
                int x_1y = -1;
                for (index = width, y = 1; y < height; y++)
                {
                    index += 1;
                    for (x = 1; x < width; x++, index++)
                    {
                        // normalize intensity
                        intensity = pSrc[index] * inv_max_intensity;

                        // calc integral
                        pI1[index] =
                            pI1[index + x_1y] + pI1[index + xy_1] - 
                            pI1[index + x_1y_1] + intensity;
                        pI2[index] =
                            pI2[index + x_1y] + pI2[index + xy_1] -
                            pI2[index + x_1y_1] + intensity * intensity;
                    }
                }
            }
        }

        protected virtual double CalcNoisePower()
        {
            double noisePower = 0;

            int kHalfWidth = _kernelWidth >> 1;
            int kHlfHeight = _kernelHeight >> 1;
            int xStart = kHalfWidth + 1;
            int yStart = kHlfHeight + 1;
            int xEnd = _imageWidth - kHalfWidth - 1;
            int yEnd = _imageHeight - kHlfHeight - 1;

                        
            int left = xStart;
            int right = xEnd;
            int nThreads = Parallel.ThreadsCount;
            int[] tops = new int[nThreads];
            int[] bottoms = new int[nThreads];
            double[] noiseTmps = new double[nThreads];
            int nLinePerThread = (yEnd - yStart + 1) / nThreads;
            int top = yStart;
            int bottom = top + nLinePerThread - 1;
            for (int iThread = 0; iThread < nThreads; iThread++)
            {
                tops[iThread] = top;
                bottoms[iThread] = bottom;

                top += nLinePerThread;
                bottom += nLinePerThread;
            }
            bottoms[nThreads - 1] = yEnd;

            Parallel.For(0, nThreads, delegate(int iThread)
            {
                noiseTmps[iThread] =
                    CalcNoisePower(left, tops[iThread], right, bottoms[iThread]);
            });

            for (int iThread = 0; iThread<nThreads; iThread++)
                noisePower += noiseTmps[iThread];

            noisePower = noisePower / (_imageWidth * _imageHeight);

            return noisePower;
        }

        private double CalcNoisePower(
            int left, int top, int right, int bottom)
        {
            double noisePower = 0;

            double kernelLength = _kernelWidth * _kernelHeight;
            int refIndxLT = -(_kernelHeight/2) * (_imageWidth + 1);
            int refIndxRT = refIndxLT + _kernelWidth - 1;
            int refIndxLB = -refIndxRT;
            int refIndxRB = -refIndxLT;

            refIndxLT -= _imageWidth + 1;
            refIndxRT -= _imageWidth;
            refIndxLB--;

            int x, y, index;
            double localMean, localVar;
            for (y = top; y <= bottom; y++)
            {
                index = y * _imageWidth + left;
                for (x = left; x <= right; x++, index++)
                {
                    localMean = _imageIntegral[index + refIndxRB] -
                                _imageIntegral[index + refIndxRT] -
                                _imageIntegral[index + refIndxLB] +
                                _imageIntegral[index + refIndxLT];
                    localMean /= kernelLength;

                    localVar = _squareImageIntegral[index + refIndxRB] -
                                _squareImageIntegral[index + refIndxRT] -
                                _squareImageIntegral[index + refIndxLB] +
                                _squareImageIntegral[index + refIndxLT];
                    localVar /= kernelLength;

                    noisePower += (localVar - localMean * localMean);
                }
            }

            return noisePower;
        }

        public virtual void Filter(bool isAuto, double noiseLevel)
        {
            CalcImageIntegrals();

            double noisePower = noiseLevel;
            if (isAuto)
                noisePower = CalcNoisePower();

            int kHalfWidth = _kernelWidth >> 1;
            int kHlfHeight = _kernelHeight >> 1;
            int xStart = kHalfWidth + 1;
            int yStart = kHlfHeight + 1;
            int xEnd = _imageWidth - kHalfWidth - 1;
            int yEnd = _imageHeight - kHlfHeight - 1;


            int left = xStart;
            int right = xEnd;
            int nThreads = Parallel.ThreadsCount;
            int[] tops = new int[nThreads];
            int[] bottoms = new int[nThreads];
            double[] noiseTmps = new double[nThreads];
            int nLinePerThread = (yEnd - yStart + 1) / nThreads;
            int top = yStart;
            int bottom = top + nLinePerThread - 1;
            for (int iThread = 0; iThread < nThreads; iThread++)
            {
                tops[iThread] = top;
                bottoms[iThread] = bottom;

                top += nLinePerThread;
                bottom += nLinePerThread;
            }
            bottoms[nThreads - 1] = yEnd;

            Parallel.For(0, nThreads, delegate(int iThread)
            {
                Filter(noisePower, left, tops[iThread], right, bottoms[iThread]);
            });

            // clear data
            _imageIntegral = null;
            _squareImageIntegral = null;
        }

        private void Filter(
            double noisePower, int left, int top, int right, int bottom)
        {
            double kernelLength = _kernelWidth * _kernelHeight;
            int refIndxLT = -(_kernelHeight / 2) * (_imageWidth + 1);
            int refIndxRT = refIndxLT + _kernelWidth - 1;
            int refIndxLB = -refIndxRT;
            int refIndxRB = -refIndxLT;

            refIndxLT -= _imageWidth + 1;
            refIndxRT -= _imageWidth;
            refIndxLB--;

            double inv_max_intensity = 1.0/max_intensity;

            int x, y, index;
            double localMean, localVar;
            double intensity, newIntensity;
            for (y = top; y <= bottom; y++)
            {
                index = y * _imageWidth + left;
                for (x = left; x <= right; x++, index++)
                {
                    // calc local mean
                    localMean = _imageIntegral[index + refIndxRB] -
                                _imageIntegral[index + refIndxRT] -
                                _imageIntegral[index + refIndxLB] +
                                _imageIntegral[index + refIndxLT];
                    localMean /= kernelLength;

                    // calc local variance
                    localVar = _squareImageIntegral[index + refIndxRB] -
                                _squareImageIntegral[index + refIndxRT] -
                                _squareImageIntegral[index + refIndxLB] +
                                _squareImageIntegral[index + refIndxLT];
                    localVar /= kernelLength;

                    localVar -= localMean * localMean;

                    // apply filter
                    intensity = _imageData[index] * inv_max_intensity;

                    newIntensity = intensity - localMean;
                    
                    intensity = (localVar > noisePower ? (localVar - noisePower) : 0);
                    
                    if (localVar < noisePower)
                        localVar = noisePower;

                    newIntensity = intensity * (newIntensity / localVar) + localMean;

                    _imageData[index] = (ushort)(max_intensity * newIntensity);
                }
            }
        }
    }
}
