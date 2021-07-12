using System;
using System.Collections.Generic;
using System.Text;

namespace SiGlaz.Preprocessing.Matching
{
    interface IMatcher
    {
        unsafe void Match(ushort* sampleData, int widthSample, int heightSample,
            ushort* patternData, int widthPattern, int heightPattern,
            double threshold,
            out double confidence,
            out int offsetX,
            out int offsetY);
    }
}
