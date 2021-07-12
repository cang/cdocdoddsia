using System;
using System.Collections.Generic;
using System.Text;

namespace SIA.Algorithms.Classification.SVM
{
    public class SVMClassifier : IClassifier
    {
        #region IClassifier Members

        public int Classify(SIA.Algorithms.FeatureProcessing.IFeatureVector fv)
        {
            throw new NotImplementedException();
        }

        public int[] Classify(SIA.Algorithms.FeatureProcessing.FeatureVectorCollection fvs)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
