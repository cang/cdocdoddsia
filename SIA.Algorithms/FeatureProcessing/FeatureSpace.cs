using System;
using System.Collections.Generic;
using System.Text;

namespace SIA.Algorithms.FeatureProcessing
{
    public class FeatureSpace
    {
        public int[] XInterestedPoints = null;
        public int[] YInterestedPoints = null;
        public int[] ContaminationMask = null;
        public double[][] Raws = null;
        public int FeatureCount = 0;
        public FeatureVectorCollection Features = new FeatureVectorCollection();

        public FeatureSpace()
        {
        }
    }
}
