using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SiGlaz.FeatureProcessing
{
    public interface IFeatureVector
    {
        int FeatureCount { get; set; }
        double[] Features { get; set; }
        int ClassId { get; set; }

        void SaveAsBinary(BinaryWriter writer);
        void LoadFromBinary(BinaryReader reader);
    }
}
