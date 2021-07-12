using System;
using System.Collections.Generic;
using System.Text;

namespace SiGlaz.Preprocessing.Matching
{
    unsafe public abstract class SimpleMatcher : IMatcher
    {
        protected int _sampleWidth = 0;
        protected int _sampleHeight = 0;        
        protected ushort* _sampleData = null;

        protected int _patternWidth = 0;
        protected int _patternHeight = 0;
        protected ushort* _patternData = null;

        public unsafe void Match(
            ushort* sampleData, int widthSample, int heightSample, 
            ushort* patternData, int widthPattern, int heightPattern, 
            double threshold, out double confidence, out int offsetX, out int offsetY)
        {
            _patternWidth = widthPattern;
            _patternHeight = heightPattern;
            _patternData = patternData;

            _sampleWidth = widthSample;
            _sampleHeight = heightSample;
            _sampleData = sampleData;

            confidence = 0;
            offsetX = 0;
            offsetY = 0;

            Match(ref confidence, ref offsetX, ref offsetY);
        }

        protected abstract void Match(
            ref double confidence, ref int offsetX, ref int offsetY);

        #region internal class
        internal class BestList
        {
            private int _budget = 11;
            private double _weakVal = double.MinValue;
            private int _count = 0;
            private double[] _sortedVals = null;
            private int[] _indices = null;
            private VComparer _comparer = new VComparer();

            public double[] BestVals
            {
                get {
                    return _sortedVals;
                }
            }

            public int[] Indices
            {
                get
                {
                    return _indices;
                }
            }

            public BestList(int budget)
            {
                _budget = budget;
                _sortedVals = new double[_budget];
                _indices = new int[_budget];
            }

            public void Add(double val, int index)
            {
                if (_count < _budget)
                {
                    _indices[_count] = index;
                    _sortedVals[_count++] = val;

                    if (_count == _budget)
                    {
                        Array.Sort(_sortedVals, _indices);
                        Array.Reverse(_sortedVals);
                        Array.Reverse(_indices);

                        _weakVal = _sortedVals[_budget - 1];
                    }

                    return;
                }

                if (_weakVal >= val)
                    return;

                int found_index = 
                    Array.BinarySearch<double>(
                    _sortedVals, val, _comparer);

                if (found_index < 0)
                {
                    found_index = ~found_index;
                    if (found_index >= _budget)
                        return;
                }                

                for (int i = _budget-1; i > found_index; i--)
                {
                    _sortedVals[i] = _sortedVals[i - 1];
                    _indices[i] = _indices[i - 1];
                }

                _sortedVals[found_index] = val;
                _indices[found_index] = index;
            }

            class VComparer : IComparer<double>
            {                
                public int Compare(double x, double y)
                {
                    return y.CompareTo(x);
                }
            }
        }
        #endregion internal class
    }
}
