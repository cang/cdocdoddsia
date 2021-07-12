#define SIA_PRODUCT

using System;
using System.Collections.Generic;
using System.Text;

namespace SIA.UI.Controls.Automation
{
    /// <summary>
    /// Factory class provides built-in categories for process steps.
    /// </summary>
    public sealed class ProcessStepCategories
    {
        public const string Input = "Input";
        public const string Preprocessing = "Pre-processing";
        public const string Analysis = "Analysis";
        public const string Output = "Output";

        public static string[] AllCategories
        {
            get
            {
                return new string[] {
                    Input,
                    Preprocessing,
                    Analysis,
                    Output,
                };
            }
        }
    }
}
