using System;
using System.Drawing;
using System.Data;
using System.IO;
using System.Threading;
using System.Diagnostics;

using SIA.Common;

using SIA.SystemFrameworks;
using SIA.SystemFrameworks.UI;

using SIA.IPEngine;
using SIA.SystemLayer;

using SIA.UI.Controls.Utilities;

using TYPE = System.UInt16;

using SIA.UI.Controls.Commands;

namespace SIA.UI.Controls.Automation.Commands
{
	/// <summary>
	/// Summary description for GbcFFTCommand.
	/// </summary>
	public class GbcFFTCommand : AutoCommand
	{
		public GbcFFTCommand(IProgressCallback callback) : base(callback)
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
			if (args[1] is GbcFFTCommandSettings == false)
				throw new ArgumentException("Argument type does not match. Arguments[1] must be GbcFFTCommandSettings", "arguments");
		}

		protected override void OnRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;

			GbcFFTCommandSettings cmdSettings = (GbcFFTCommandSettings)args[1];

			int threshold = cmdSettings.Threshold;
			float cutoff = cmdSettings.CutOff;

			this.SetStatusRange(0, 100);
			this.SetStatusText("Perform Global Background Correction using FFT...");

			this.GlobalBackgroundCorrection(image, threshold, cutoff);			
		}

		public override void AutomationRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;

			GbcFFTCommandSettings cmdSettings = (GbcFFTCommandSettings)args[1];

			int threshold = cmdSettings.Threshold;
			float cutoff = cmdSettings.CutOff;

			this.GlobalBackgroundCorrection(image, threshold, cutoff);			
		}

		protected virtual void GlobalBackgroundCorrection(CommonImage image, int threshold, float cutoff)
		{
			try
			{
				image.GlobalBackgroundCorrection(eGlobalBackgroundCorrectionType.FastFourierTransform, (TYPE)threshold, cutoff);
			}
			catch
			{
				throw;
			}
			finally
			{
			}
		}
	}
}
