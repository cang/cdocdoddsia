using System;
using System.Collections.Generic;
using System.Text;

namespace SIA.Algorithms.FeatureProcessing.Textural
{
    using IntCollection = List<int>;
    using SIA.SystemFrameworks;

    public class TextureProcessor
    {
        #region Member fields

        private byte[] _data;
        private int _length = 0;
        private int _dataStride;
        private int _height;
        private int _sampleWidth = 0;
        private int _sampleHeight = 0;
        private int _halfSampleWidth = 0;
        private int _halfSampleHeight = 0;
        private int _padding = 1;

        private GrayCoMatrixOperator _grayCoMatrixOperator = null;
        #endregion Member fields

        #region Contructors and destructors
        public TextureProcessor(
            byte[] data, int datastride, int sampleWidth, int sampleHeight)
        {
            if (data == null)
                throw new ArgumentException("Data must not be null", "data");

            _data = data;
            _dataStride = datastride;
            _length = _data.Length;
            if (_length % _dataStride != 0)
                throw new ArgumentException("Data size is invalid", "data");
            _height = _length / _dataStride;
            _sampleWidth = sampleWidth;
            _sampleHeight = sampleHeight;
            _halfSampleWidth = sampleWidth / 2;
            _halfSampleHeight = sampleHeight / 2;

            int greyLevel = 128;
            greyLevel = 64;
            _grayCoMatrixOperator = new GrayCoMatrixOperator(
                _data, _dataStride, _sampleWidth, _sampleHeight, greyLevel);
        }

        public TextureProcessor(
            byte[] data, int datastride, int sampleWidth, int sampleHeight, int greyLevel)
        {
            if (data == null)
                throw new ArgumentException("Data must not be null", "data");

            _data = data;
            _dataStride = datastride;
            _length = _data.Length;
            if (_length % _dataStride != 0)
                throw new ArgumentException("Data size is invalid", "data");
            _height = _length / _dataStride;
            _sampleWidth = sampleWidth;
            _sampleHeight = sampleHeight;
            _halfSampleWidth = sampleWidth / 2;
            _halfSampleHeight = sampleHeight / 2;
            
            _grayCoMatrixOperator = new GrayCoMatrixOperator(
                _data, _dataStride, _sampleWidth, _sampleHeight, greyLevel);
        }
        #endregion Contructors and destructors

        #region Extract Features
        private double[] _temp = null;
        public double[] ExtractFeatures(int curx, int cury)
        {
            int offsetx = 1;
            int offsety = 0;

            if (_temp == null)
                _temp = new double[14];

            double[] features = new double[14 * 2];

            offsetx = 1;
            offsety = 0;

            if (_grayCoMatrixOperator.CoProperties(curx, cury, offsetx, offsety, _temp) > 0)
                return null;

            Array.Copy(_temp, 0, features, 0, 14);

            offsetx = 1;
            offsety = 1;

            if (_grayCoMatrixOperator.CoProperties(curx, cury, offsetx, offsety, _temp) > 0)
                return null;

            Array.Copy(_temp, 0, features, 14, 14);

            return features;
        }

        public bool ExtractFeatures(int curx, int cury, double[] features)
        {
            int offsetx = 1;
            int offsety = 0;

            if (_temp == null)
                _temp = new double[14];

            offsetx = 1;
            offsety = 0;

            if (_grayCoMatrixOperator.CoProperties(curx, cury, offsetx, offsety, _temp) > 0)
                return false;

            Array.Copy(_temp, 0, features, 0, 14);

            offsetx = 1;
            offsety = 1;

            if (_grayCoMatrixOperator.CoProperties(curx, cury, offsetx, offsety, _temp) > 0)
                return false;

            Array.Copy(_temp, 0, features, 14, 14);

            return true;
        }

        public double[][] ExtractFeatures(ref int[] x, ref int[] y)
        {
            List<double[]> features = new List<double[]>();

            IntCollection xs = new IntCollection();
            IntCollection ys = new IntCollection();
            int curx = _padding + _halfSampleWidth, cury = 0;

            double step_factor = 0.8;
            int stepx = (int)(_sampleWidth * step_factor);
            int stepy = (int)(_sampleHeight * step_factor);
            while (curx < _dataStride)
            {
                cury = _padding + _halfSampleHeight;
                while (cury < _height)
                {
                    if (IsValid(curx, cury))
                    {
                        double[] featureVector = ExtractFeatures(curx, cury);
                        if (featureVector != null)
                        {
                            xs.Add(curx);
                            ys.Add(cury);
                            features.Add(featureVector);
                        }
                    }

                    cury += stepy;
                }

                curx += stepx;
            }

            x = xs.ToArray();
            y = ys.ToArray();

            return features.ToArray();
        }

        public void ExtractFeatures(int[] xList, int[] yList, ref double[][] features)
        {
            int n = xList.Length;
            features = new double[n][];
            int curx, cury;
            for (int i = 0; i < n; i++)
            {
                curx = xList[i];
                cury = yList[i];

                features[i] = ExtractFeatures(curx, cury);
            }
        }

        public void ExtractFeatures(
            int[] xList, int[] yList, int startIdx, int endIdx,
            ref double[][] features)
        {            
            int curx, cury;
            for (int i = startIdx; i <= endIdx; i++)
            {
                curx = xList[i];
                cury = yList[i];

                ExtractFeatures(curx, cury, features[i]);
            }
        }

        public void ExtractFeatures(
            int[] xList, int[] yList, int[] mask, int startIdx, int endIdx,
            ref double[][] features)
        {
            int curx, cury;
            for (int i = startIdx; i <= endIdx; i++)
            {
                if (mask[i] < 0)
                    continue;

                curx = xList[i];
                cury = yList[i];

                ExtractFeatures(curx, cury, features[i]);
            }
        }
        #endregion Extract Features

        #region Extract Features Parallely
        public static void ExtractFeaturesParallely(byte[] data, int imageWidth, 
            int kernelSize, int greyLevel, int[] xList, int[] yList, double[][] features)
        {
            int n = xList.Length;            

            int nThreads = Parallel.ThreadsCount;
            if (nThreads > n)
                nThreads = 1;
            int nFVsPerThread = n / nThreads;            
            int[] idxStartThreads = new int[nThreads];
            int[] idxEndThreads = new int[nThreads];
            TextureProcessor[] processors = new TextureProcessor[nThreads];
            for (int i = 0; i < nThreads; i++)
            {
                processors[i] = new TextureProcessor(
                    data, imageWidth, kernelSize, kernelSize, greyLevel);

                idxStartThreads[i] = i * nFVsPerThread;
                idxEndThreads[i] = idxStartThreads[i] + nFVsPerThread - 1;
            }
            Parallel.For(0, nThreads, delegate(int iThread)
            {
                ExtractFeatures(processors[iThread],
                    xList, yList, idxStartThreads[iThread], idxEndThreads[iThread], features);
            });
            int idxStartRemains = idxEndThreads[nThreads - 1] + 1;
            int idxEndRemains = n - 1;
            if (idxEndRemains >= idxStartRemains)
            {
                Parallel.For(idxStartRemains, idxEndRemains, delegate(int iRemain)
                {
                    ExtractFeatures(
                        processors[iRemain - idxStartRemains], 
                        xList, yList, iRemain, iRemain, features);
                });
            }
        }

        private static void ExtractFeatures(TextureProcessor processor,
            int[] xList, int[] yList, int startIdx, int endIdx, double[][] features)
        {
            processor.ExtractFeatures(
                xList, yList, startIdx, endIdx, ref features);
        }

        public static void ExtractFeaturesParallely(byte[] data, int imageWidth,
            int kernelSize, int greyLevel, int[] xList, int[] yList, int[] mask, double[][] features)
        {
            int n = xList.Length;

            int nThreads = Parallel.ThreadsCount;
            if (nThreads > n)
                nThreads = 1;
            int nFVsPerThread = n / nThreads;
            int[] idxStartThreads = new int[nThreads];
            int[] idxEndThreads = new int[nThreads];
            TextureProcessor[] processors = new TextureProcessor[nThreads];
            for (int i = 0; i < nThreads; i++)
            {
                processors[i] = new TextureProcessor(
                    data, imageWidth, kernelSize, kernelSize, greyLevel);

                idxStartThreads[i] = i * nFVsPerThread;
                idxEndThreads[i] = idxStartThreads[i] + nFVsPerThread - 1;
            }
            Parallel.For(0, nThreads, delegate(int iThread)
            {
                ExtractFeatures(processors[iThread],
                    xList, yList, mask, idxStartThreads[iThread], idxEndThreads[iThread], features);
            });
            int idxStartRemains = idxEndThreads[nThreads - 1] + 1;
            int idxEndRemains = n - 1;
            if (idxEndRemains >= idxStartRemains)
            {
                Parallel.For(idxStartRemains, idxEndRemains, delegate(int iRemain)
                {
                    ExtractFeatures(
                        processors[iRemain - idxStartRemains],
                        xList, yList, mask, iRemain, iRemain, features);
                });
            }
        }

        private static void ExtractFeatures(TextureProcessor processor,
            int[] xList, int[] yList, int[] mask, int startIdx, int endIdx, double[][] features)
        {
            processor.ExtractFeatures(
                xList, yList, mask, startIdx, endIdx, ref features);
        }
        #endregion Extract Features Parallely

        #region Helpers
        private void Gain(float[] values, float gainFactor)
        {
            if (values == null)
                return;

            for (int i = values.Length - 1; i >= 0; i--)
            {
                values[i] *= gainFactor;
            }
        }

        public bool IsValid(int x, int y)
        {
            return (
                x > (_halfSampleWidth + _padding) &&
                (x + _halfSampleWidth + _padding) < _dataStride &&
                y > (_halfSampleHeight + _padding) &&
                (y + _halfSampleHeight + _padding) < _height);
        }
        #endregion Helpers
    }
}
