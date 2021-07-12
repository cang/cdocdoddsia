using System;
using System.Collections.Generic;
using System.Text;
using SIA.Algorithms.FeatureProcessing;

namespace SIA.Algorithms.Classification
{
    public interface IClassifier
    {
        int Classify(IFeatureVector fv);
        int[] Classify(FeatureVectorCollection fvs);
    }
}
