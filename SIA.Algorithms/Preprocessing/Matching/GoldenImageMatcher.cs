using System;
using System.Collections.Generic;
using System.Text;
using SIA.SystemLayer;

namespace SIA.Algorithms.Preprocessing.Matching
{
    public class GoldenImageMatcher : SimpleMatcher
    {
        protected int _kernelWidth = 5;
        protected int _kernelHeight = 5;

        protected int[] _patternOffsets = null;
        protected int[] _sampleOffsets = null;

        protected int[] _xInterests = null;
        protected int[] _yInterests = null;

        public GoldenImageMatcher(int kernelWidth, int kernelHeight)
        {
            _kernelWidth = kernelWidth;
            _kernelHeight = kernelHeight;
        }

        public GoldenImageMatcher(
            int kernelWidth, int kernelHeight,
            int[] xInterests, int[] yInterests)
        {
            _kernelWidth = kernelWidth;
            _kernelHeight = kernelHeight;

            _xInterests = xInterests;
            _yInterests = yInterests;
        }

        protected override void Match(ref double confidence, ref int offsetX, ref int offsetY)
        {
            _patternOffsets = CalcItemOffsets(
                _patternWidth, _kernelWidth, _kernelHeight);
            _sampleOffsets = CalcItemOffsets(
                _sampleWidth, _kernelWidth, _kernelHeight);

            offsetX = 0;
            offsetY = 0;
            confidence = double.MinValue;

            int l = 0;
            int t = 0;
            int r = _sampleWidth - _patternWidth - 1;
            int b = _sampleHeight - _patternHeight - 1;
            double[,] confidences = new double[b - t + 1, r - l + 1];

            if (_xInterests == null || _yInterests == null)
            {
                //Parallel.For(t, b + 1, delegate(int y)
                for (int y=t; y<=b; y++)
                {
                    for (int x = l; x <= r; x++)
                    {
                        confidences[y, x] = Match(x, y);
                    }
                }
                //);
            }
            else
            {
                int n = Math.Min(_xInterests.Length, _yInterests.Length);
                //Parallel.For(0, n, delegate(int i)
                for (int i=0; i<n; i++)
                {
                    int x = _xInterests[i];
                    int y = _yInterests[i];
                    confidences[y, x] = Match(x, y);
                }
                //);
            }

            for (int y = 0; y <= b - t; y++)
            {
                for (int x = 0; x <= r - l; x++)
                {
                    if (confidences[y, x] > confidence)
                    {
                        confidence = confidences[y,x];
                        offsetX = x;
                        offsetY = y;
                    }
                }
            }

            offsetX += (_patternWidth - _sampleWidth) / 2;
            offsetY += (_patternHeight - _sampleHeight) / 2;
                        
            //confidence = 1.0; // test only
        }

        unsafe protected virtual double Match(int xSeed, int ySeed)
        {
            double err = 0;

            int kHalfWidth = _kernelWidth >> 1;
            int kHalfHeight = _kernelHeight >> 1;
            int l = kHalfWidth;
            int t = kHalfHeight;
            int r = _patternWidth - kHalfWidth - 1;
            int b = _patternHeight - kHalfHeight - 1;

            int kLength = _kernelWidth * _kernelHeight;

            int patIndx = 0;
            int samIndx = 0;
            double v, absv, min_absv;

            for (int y = t; y <= b; y++)
            {
                patIndx = y * _patternWidth + l;
                samIndx = (ySeed + (y - t)) * _sampleWidth + (xSeed + (l - l));
                for (int x = l; x <= r; x++, patIndx++, samIndx++)
                {
                    v = _sampleData[samIndx];
                    min_absv = double.MaxValue;
                    for (int i = 0; i < kLength; i++)
                    {
                        absv = Math.Abs(v - _patternData[patIndx + _patternOffsets[i]]);

                        if (absv < min_absv)
                        {                            
                            min_absv = absv;
                            if (min_absv < 15)
                                break;
                        }
                    }

                    err += min_absv * min_absv;
                }
            }

            err = err / ((b - t + 1) * (r - l + 1));

            return (255.0 - Math.Sqrt(err));
        }
    }
}
