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

using SIA.UI.Controls.Automation.Commands;
using SIA.UI.Controls.Utilities;
using SIA.Algorithms;


namespace SIA.UI.Controls.Commands
{
	/// <summary>
	/// Summary description for ThresholdCommand.
	/// </summary>
	public class ThresholdCommand : RasterCommand
	{
		public ThresholdCommand(IProgressCallback callback) : base(callback)
		{
		}

		protected override void ValidateArguments(params object[] args)
		{
			if (args == null)
				throw new ArgumentNullException("arguments");
			if (args.Length < 2)
				throw new ArgumentException("Not enough arguments", "arguments");
			if (args[0] is CommonImage == false)
				throw new ArgumentException("Argument type does not match. Arguments[0] must be CommonImage", "arguments");
			if (args[1] is ThresholdCommandSettings  == false)
				throw new ArgumentException("Argument type does not match. Arguments[1] must be bool", "arguments");			
		}

		protected override void OnRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;
			ThresholdCommandSettings settings = (ThresholdCommandSettings)args[1];

			this.SetStatusRange(0, 100);
			this.SetStatusText("Thresholding image...");

#if DEBUG_METETIME
            DebugMeteTime dm = new DebugMeteTime();
#endif

			this.Threshold(image, settings);

#if DEBUG_METETIME
            dm.AddLine("ThresholdCommand:Run");
            dm.Write2Debug(true);
#endif

		}


#if GPU_SUPPORTED
        protected virtual void Threshold(CommonImage image, ThresholdCommandSettings settings)
        {
            if (image == null || settings == null)
                return;

            //this code will be removed in feature
            bool bCreateBuff = !image.HasDeviceBuffer;
            if (bCreateBuff)
                image.CreateDeviceBuffer();

            if (settings.Type == eThresholdType.Intensity)
            {
                bool zeroMinimum = settings.ZeroMinimum;
                int minimum = -1;
                if (settings.UseMinimum)
                    minimum = settings.Minimum;
                bool zeroMaximum = settings.ZeroMaximum;
                int maximum = -1;
                if (settings.UseMaximum)
                    maximum = settings.Maximum;

                image.kThresholdGPU(minimum, zeroMinimum, maximum, zeroMaximum);

            }
            else
            {
                float from = 0.0f;
                if (settings.RemoveDeadPixel)
                    from = settings.From;
                float to = 100.0f;
                if (settings.RemoveHotPixel)
                    to = settings.To;
                bool zeroHotPixel = settings.ZeroTo;
                bool zeroDeadPixel = settings.ZeroFrom;

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
        protected virtual void Threshold(CommonImage image, ThresholdCommandSettings settings)
		{
			if (image == null || settings == null)
				return;

			if (settings.Type == eThresholdType.Intensity) 
			{
				bool zeroMinimum = settings.ZeroMinimum;
				int minimum = -1;
				if (settings.UseMinimum)
					minimum = settings.Minimum;
				bool zeroMaximum = settings.ZeroMaximum;
				int maximum = -1;
				if (settings.UseMaximum)
					maximum = settings.Maximum;

				image.kThreshold(minimum, zeroMinimum, maximum, zeroMaximum);
			}
			else 
			{
				float from = 0.0f;
				if (settings.RemoveDeadPixel)
					from = settings.From;
				float to = 100.0f;
				if (settings.RemoveHotPixel)
					to = settings.To;
				bool zeroHotPixel = settings.ZeroTo;
				bool zeroDeadPixel = settings.ZeroFrom;

				if (from <= 0 && to >= 100)
					return;

				image.ThresholdByPercentile(from, to, zeroDeadPixel, zeroHotPixel);
			}
		}
#endif

    }
}
