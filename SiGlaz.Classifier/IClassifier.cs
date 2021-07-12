using System;
using System.Collections.Generic;
using System.Text;
using SiGlaz.FeatureProcessing;

namespace SiGlaz.Classifier
{
    public interface IClassifier
    {
        int Classify(IFeatureVector fv);
        int[] Classify(FeatureVectorCollection fvs);
    }
}
