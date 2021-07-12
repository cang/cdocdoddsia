using System;
using System.Collections.Generic;
using System.Text;

namespace SiGlaz.ObjectAnalysis.UI
{
    public class ucPrimitiveObjectFilterStep : ucObjectAnalyzerStep
    {
        public const int DefaultStepId = 2;

        public ucPrimitiveObjectFilterStep()
            : base(DefaultStepId)
        {
            chkStatus.Text = "Apply filtering primitive object(s)";
        }
    }
}
