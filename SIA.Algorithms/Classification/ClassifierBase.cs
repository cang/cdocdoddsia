using System;
using System.Collections.Generic;
using System.Text;

namespace SIA.Algorithms.Classification
{
    public abstract class ClassifierBase : IClassifier
    {
        #region IClassifier Members

        public virtual int Classify(SIA.Algorithms.FeatureProcessing.IFeatureVector fv)
        {
            throw new NotImplementedException();
        }

        public virtual int[] Classify(SIA.Algorithms.FeatureProcessing.FeatureVectorCollection fvs)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
