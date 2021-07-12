using System;
using System.Collections.Generic;
using System.Text;

namespace SiGlaz.ObjectAnalysis.UI
{
    public class ucObjectExportingStep : ucObjectAnalyzerStep
    {
        public const int DefaultStepId = 6;

        public ucObjectExportingStep()
            : base(DefaultStepId)
        {
            chkStatus.Text = "Specify output option(s)";
        }
    }
}
