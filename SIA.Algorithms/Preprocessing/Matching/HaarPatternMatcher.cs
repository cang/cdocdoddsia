using System;
using System.Collections.Generic;
using System.Text;
using SiGlaz.Common.ImageAlignment;
using SIA.SystemFrameworks;

namespace SIA.Algorithms.Preprocessing.Matching
{
    public class HaarPatternMatcher : SimpleMatcher
    {
        protected int[] _bestIndices = null;
        protected int[] _interestedXList = null;
        public int[] InterestedXList
        {
            get { return _interestedXList; }
        }
        protected int[] _interestedYList = null;
        public int[] InterestedYList
        {
            get { return _interestedYList; }
        }

        protected HaarPattern _haarPattern = null;
        protected int _pivotCount = 31;

        protected BestList _pivotList;
        public BestList PivotList
        {
            get { return _pivotList; }
            set { _pivotList = value; }
        }

        public HaarPatternMatcher(HaarPattern haarPattern, int pivotCount)
        {
            _haarPattern = haarPattern;
            _pivotCount = pivotCount;
        }

        protected override void Match(ref double confidence, ref int offsetX, ref int offsetY)
        {
            double[] integralImage = new double[_sampleWidth * _sampleHeight];
            unsafe
            {
                fixed (double* pDst = integralImage)
                    Utilities.CalcImageIntegral(_sampleData, _sampleWidth, _sampleHeight, pDst);
            }

            // build pattern
            _haarPattern.BuildEmbededParameters(_sampleWidth);
            double eigenVal = _haarPattern.EigenVal; // eigen val of pattern
            PivotList = new BestList(_pivotCount);

            int l = 1;
            int t = 1;
            int r = _sampleWidth - _patternWidth - 1;
            int b = _sampleHeight - _patternHeight - 1;
            int scanningIndex = 0;
            double val;

            if (!_roi.IsEmpty)
            {
                l = _roi.Left + 1;
                t = _roi.Top + 1;
                r = _roi.Right - _patternWidth - 1;
                b = _roi.Bottom - _patternHeight - 1;
            }

            // this is sequence matching, parallel matching will be implemented later.
            
            for (int y = t; y <= b; y++)
            //Parallel.For(t, b + 1 , delegate(int y)
            {
                scanningIndex = y * _sampleWidth + l;
                for (int x = l; x <= r; x++, scanningIndex++)
                {
                    try
                    {
                        val = _haarPattern.CalcEigenValue(integralImage, _sampleWidth, scanningIndex);
                        PivotList.Add(val, scanningIndex);
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine(scanningIndex);
                    }

                }
            }//);

            _bestIndices = PivotList.Indices;
            _interestedXList = new int[_bestIndices.Length];
            _interestedYList = new int[_bestIndices.Length];
            for (int i = _bestIndices.Length - 1; i >= 0; i--)
            {
                _interestedXList[i] = _bestIndices[i] % _sampleWidth;
                _interestedYList[i] = _bestIndices[i] / _sampleWidth;
            }

            // update the best match
            confidence = PivotList.BestVals[0];
            confidence = 1 - (Math.Abs(confidence - eigenVal) / eigenVal);
            offsetX = _interestedXList[0];
            offsetY = _interestedYList[0];
            //offsetX = (int)(offsetX + _patternWidth * 0.5 - _sampleWidth * 0.5);
            //offsetY = (int)(offsetY + _patternHeight * 0.5 - _sampleHeight * 0.5);
        }
    }
}
