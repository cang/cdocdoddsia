//#define GPU_SUPPORTED
//#define DEBUG_METETIME

using System;
using System.Drawing;
using System.Data;
using System.IO;
using System.Threading;
using System.Diagnostics;

using SIA.SystemFrameworks;
using SIA.SystemFrameworks.UI;

using SIA.IPEngine;
using SIA.SystemLayer;

using SIA.UI.Controls.Utilities;

namespace SIA.UI.Controls.Automation.Commands 
{
	/// <summary>
	/// Summary description for ThresholdCommand.
	/// </summary>
	public class ThresholdCommand : AutoCommand 
	{
		public ThresholdCommand(IProgressCallback callback) : base(callback) 
		{			
		}

		protected override void UninitClass() 
		{
			base.UninitClass ();
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
			if (args[1] is ThresholdCommandSettings == false)
				throw new ArgumentException("Argument type does not match. Arguments[0] must be ThresholdCommandSettings", "arguments");
		}

		public override object[] GetOutput() 
		{
			return null;
		}

		protected override void OnRun(params object[] args) 
		{
			// nothing
		}

		public override void AutomationRun(params object[] args) 
		{
			CommonImage image = args[0] as CommonImage;
			ThresholdCommandSettings cmdSettings = args[1] as ThresholdCommandSettings;
			
			this.SetStatusRange(0, 100);
			this.SetStatusText("Thresholding image...");

#if DEBUG_METETIME
            DebugMeteTime dm = new DebugMeteTime();
#endif

            this.Threshold(image, cmdSettings);

#if DEBUG_METETIME
            dm.AddLine("ThresholdCommand:Run");
            dm.Write2Debug(true);
#endif
        }

#if GPU_SUPPORTED
		protected virtual void Threshold(CommonImage image, ThresholdCommandSettings cmdSettings) 
		{
			if (image == null || cmdSettings == null)
				return;

            //this code will be removed in feature
            bool bCreateBuff = !image.HasDeviceBuffer;
            if (bCreateBuff)
                image.CreateDeviceBuffer();

			if (cmdSettings.Type == eThresholdType.Intensity) 
			{
				bool zeroMinimum = cmdSettings.ZeroMinimum;
				int minimum = -1;
				if (cmdSettings.UseMinimum)
					minimum = cmdSettings.Minimum;
				bool zeroMaximum = cmdSettings.ZeroMaximum;
				int maximum = -1;
				if (cmdSettings.UseMaximum)
					maximum = cmdSettings.Maximum;

				image.kThresholdGPU(minimum, zeroMinimum, maximum, zeroMaximum);
			}
			else 
			{
				float from = 0.0f;
				if (cmdSettings.RemoveDeadPixel)
					from = cmdSettings.From;
				float to = 100.0f;
				if (cmdSettings.RemoveHotPixel)
					to = cmdSettings.To;
				bool zeroHotPixel = cmdSettings.ZeroTo;
				bool zeroDeadPixel = cmdSettings.ZeroFrom;

				if (from <= 0 && to >= 100)
					return;

				image.ThresholdByPercentileGPU(from, to, zeroDeadPixel, zeroHotPixel);
			}

            //this code will be removed in feature
            image.ReadDataFromDeviceBuffer();
            if (bCreateBuff)
                image.DisposeDeviceBuffer();

		}
#else
        protected virtual void Threshold(CommonImage image, ThresholdCommandSettings cmdSettings)
        {
            if (image == null || cmdSettings == null)
                return;

            if (cmdSettings.Type == eThresholdType.Intensity)
            {
                bool zeroMinimum = cmdSettings.ZeroMinimum;
                int minimum = -1;
                if (cmdSettings.UseMinimum)
                    minimum = cmdSettings.Minimum;
                bool zeroMaximum = cmdSettings.ZeroMaximum;
                int maximum = -1;
                if (cmdSettings.UseMaximum)
                    maximum = cmdSettings.Maximum;

                image.kThreshold(minimum, zeroMinimum, maximum, zeroMaximum);
            }
            else
            {
                float from = 0.0f;
                if (cmdSettings.RemoveDeadPixel)
                    from = cmdSettings.From;
                float to = 100.0f;
                if (cmdSettings.RemoveHotPixel)
                    to = cmdSettings.To;
                bool zeroHotPixel = cmdSettings.ZeroTo;
                bool zeroDeadPixel = cmdSettings.ZeroFrom;

                if (from <= 0 && to >= 100)
                    return;

                image.ThresholdByPercentile(from, to, zeroDeadPixel, zeroHotPixel);
            }
        }
#endif
	}
}
