using System;
using System.Collections.Generic;
using System.Text;

namespace SiGlaz.Classifier
{
    public abstract class ClassifierBase : IClassifier
    {
        #region IClassifier Members

        public virtual int Classify(SiGlaz.FeatureProcessing.IFeatureVector fv)
        {
            throw new NotImplementedException();
        }

        public virtual int[] Classify(SiGlaz.FeatureProcessing.FeatureVectorCollection fvs)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
