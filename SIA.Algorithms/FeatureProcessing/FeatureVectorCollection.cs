using System;
using System.Collections.Generic;
using System.Text;

namespace SIA.Algorithms.FeatureProcessing
{
    public class FeatureVectorCollection : List<IFeatureVector>
    {
        public FeatureVectorCollection()
        {
        }

        public FeatureVectorCollection(int capacity)
        {
            base.Capacity = capacity;
        }
    }
}
