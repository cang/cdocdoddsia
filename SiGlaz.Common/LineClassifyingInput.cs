using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace SiGlaz.Common
{
    public class LineClassifyingInput
    {
        #region For Classification Engine
        public float Thickness;
        public float Length;
        public ushort[] Pattern;
        
        /// <summary>
        /// Refer to SiGlaz.Common.LinePatternLibrary's const for signature name
        /// </summary>
        public List<String> DetectingSignatureNames;
        #endregion

        #region For back reference
        public float RealBeginX;
        public float RealBeginY;
        public float RealEndX;
        public float RealEndY;
        #endregion

        #region Output
        public List<RectangleF> DetectedPosition;
        public List<String> DetectedSignatures;
        #endregion

    }
}
