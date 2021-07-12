using System;
using System.Collections;
using System.Drawing;
using System.Data;
using System.IO;
using System.Threading;
using System.Diagnostics;

using SIA.Common.KlarfExport;
using SIA.SystemFrameworks;
using SIA.SystemFrameworks.UI;

using SIA.IPEngine;
using SIA.SystemLayer;
using SIA.SystemLayer.ObjectExtraction;

using SIA.UI.Controls.Utilities;

using SIA.UI.Controls.Commands;
using SIA.Common.Analysis;

namespace SIA.UI.Controls.Automation.Commands
{
    public class ExtGlobalBckgCommand : AutoCommand
    {
        public ExtGlobalBckgCommand(IProgressCallback callback)
            : base(callback)
        {
        }

        protected override void UninitClass()
        {
            base.UninitClass();
        }

        public override bool CanAbort
        {
            get { return true; }
        }

        protected override void ValidateArguments(params object[] args)
        {
            if (args == null)
                throw new ArgumentNullException("arguments");
            if (args.Length < 2)
                throw new ArgumentException("Not enough arguments", "arguments");
            if (args[0] is CommonImage == false)
                throw new ArgumentException("Argument type does not match. Arguments[0] must be CommonImage", "arguments");
            if (args[1] is ExtGlobalBckgSettings == false)
                throw new ArgumentException("Argument type does not match. Arguments[1] must be ExtGlobalBckgSettings", "arguments");
        }
        
        protected override void OnRun(params object[] args)
        {
            CommonImage image = args[0] as CommonImage;
            ExtGlobalBckgSettings settings = (ExtGlobalBckgSettings)args[1];

            this.SetStatusRange(0, 100);
            this.SetStatusText("Extracting Global Background...");

            this.Extract(image, settings);

            this.SetStatusValue(100);
        }

        protected virtual void Extract(CommonImage image, ExtGlobalBckgSettings settings)
        {
            if (settings == null)
                return;

            switch (settings.Method)
            {
                case eExtGlobalBckgMethod.FFT:
                    image.ExtractGlobalBackgroundByFFT(settings.FFT_Threshold, settings.FFT_CutOff);
                    break;
                case eExtGlobalBckgMethod.Erosion:
                    image.ExtractGlobalBackgroundByErosion(settings.NumberOfErosionFilters);
                    break;
            }
        }

        public override void AutomationRun(params object[] args)
        {
            CommonImage image = args[0] as CommonImage;
            ExtGlobalBckgSettings settings = (ExtGlobalBckgSettings)args[1];

            this.Extract_AutomationMode(image, settings);
        }

        protected virtual void Extract_AutomationMode(CommonImage image, ExtGlobalBckgSettings settings)
        {
            using (CommandProgressLocker locker = new CommandProgressLocker())
            {
                Extract(image, settings);
            }
        }
    }
}
