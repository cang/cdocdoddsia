using System;
using System.Collections.Generic;
using System.Text;

namespace SiGlaz.ObjectAnalysis.UI
{
    public class ucObjectClassificationStep : ucObjectAnalyzerStep
    {
        public const int DefaultStepId = 5;

        public ucObjectClassificationStep()
            : base(DefaultStepId)
        {
            chkStatus.Text = "Apply object classification";
        }
    }
}
