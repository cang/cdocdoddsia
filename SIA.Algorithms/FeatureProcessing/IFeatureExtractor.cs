﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SIA.Algorithms.FeatureProcessing
{
    public interface IFeatureExtractor
    {
        FeatureVectorCollection Extract(object arg);

        FeatureVectorCollection Extract(object[] args);
    }
}
