using System;
using System.Collections.Generic;
using System.Text;

namespace SiGlaz.ObjectAnalysis.UI
{
    public class ucObjectFilterStep : ucObjectAnalyzerStep
    {
        public const int DefaultStepId = 4;

        public ucObjectFilterStep()
            : base(DefaultStepId)
        {
            chkStatus.Text = "Apply filtering object(s)";
        }
    }
}
