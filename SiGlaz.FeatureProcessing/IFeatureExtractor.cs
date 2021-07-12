using System;
using System.Collections.Generic;
using System.Text;

namespace SiGlaz.FeatureProcessing
{
    public interface IFeatureExtractor
    {
        FeatureVectorCollection Extract(object arg);

        FeatureVectorCollection Extract(object[] args);
    }
}
