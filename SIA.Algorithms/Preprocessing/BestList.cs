using System;
using System.Collections.Generic;
using System.Text;

namespace SIA.Algorithms.Preprocessing
{
    public class BestList
    {
        private int _budget = 11;
        private double _weakVal = double.MinValue;
        private int _count = 0;
        private double[] _sortedVals = null;
        private int[] _indices = null;
        private VComparer _comparer = new VComparer();

        public double[] BestVals
        {
            get
            {
                return _sortedVals;
            }
        }

        public int[] Indices
        {
            get
            {
                if (_count == _budget)
                    return _indices;

                int[] indices = new int[_count];
                Array.Copy(_indices, 0, indices, 0, _count);

                return indices;
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

            for (int i = _budget - 1; i > found_index; i--)
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
}
