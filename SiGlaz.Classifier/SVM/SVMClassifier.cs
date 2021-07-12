using System;
using System.Collections.Generic;
using System.Text;

namespace SiGlaz.Classifier.SVM
{
    public class SVMClassifier : IClassifier
    {
        #region IClassifier Members

        public int Classify(SiGlaz.FeatureProcessing.IFeatureVector fv)
        {
            throw new NotImplementedException();
        }

        public int[] Classify(SiGlaz.FeatureProcessing.FeatureVectorCollection fvs)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
