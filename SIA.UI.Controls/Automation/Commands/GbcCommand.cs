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

using SIA.UI.Controls.Commands;

using TYPE = System.UInt16;

namespace SIA.UI.Controls.Automation.Commands
{
	/// <summary>
	/// Summary description for GbcCommand.
	/// </summary>
	public class GbcCommand : AutoCommand
	{
		public GbcCommand(IProgressCallback callback) : base(callback)
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
			if (args[1] is GbcCommandSettings == false)
				throw new ArgumentException("Argument type does not match. Arguments[1] must be GbcCommandSettings", "arguments");
		}

		protected override void OnRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;

			GbcCommandSettings cmdSettings = (GbcCommandSettings)args[1];

			eGlobalBackgroundCorrectionType type = cmdSettings.Type;

			switch (type)
			{
				case eGlobalBackgroundCorrectionType.ErosionFilter:
					int num_pass = cmdSettings.NumPass;
					this.ErosionGlobalBackgroundCorrection(image, num_pass);
					break;
				case eGlobalBackgroundCorrectionType.FastFourierTransform:
					int threshold = cmdSettings.Threshold;
					float cutoff = cmdSettings.CutOff;
					this.FFTGlobalBackgroundCorrection(image, threshold, cutoff);
					break;
				case eGlobalBackgroundCorrectionType.ReferenceImages:
					string[] FilePaths = cmdSettings.FilePaths;
					this.RefImageGlobalBackgroundCorrection(image, FilePaths);
					break;
				case eGlobalBackgroundCorrectionType.UnsharpFilter:
					UnsharpParam param = cmdSettings.UnsharpSettings;
					this.UnsharpGlobalBackgroundCorrection(image, param);
					break;
			}
		}

		public override void AutomationRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;

			GbcCommandSettings cmdSettings = (GbcCommandSettings)args[1];

			eGlobalBackgroundCorrectionType type = cmdSettings.Type;

			switch (type)
			{
				case eGlobalBackgroundCorrectionType.ErosionFilter:
					int num_pass = cmdSettings.NumPass;
					this.ErosionGlobalBackgroundCorrection(image, num_pass);
					break;
				case eGlobalBackgroundCorrectionType.FastFourierTransform:
					int threshold = cmdSettings.Threshold;
					float cutoff = cmdSettings.CutOff;
					this.FFTGlobalBackgroundCorrection(image, threshold, cutoff);
					break;
				case eGlobalBackgroundCorrectionType.ReferenceImages:
					string[] FilePaths = cmdSettings.FilePaths;
					this.RefImageGlobalBackgroundCorrection(image, FilePaths);
					break;
				case eGlobalBackgroundCorrectionType.UnsharpFilter:
					UnsharpParam param = cmdSettings.UnsharpSettings;
					this.UnsharpGlobalBackgroundCorrection(image, param);
					break;
			}		
		}

		protected virtual void ErosionGlobalBackgroundCorrection(CommonImage image, int num_pass)
		{
			try
			{
				image.GlobalBackgroundCorrection(eGlobalBackgroundCorrectionType.ErosionFilter, num_pass);
			}
			catch
			{
				throw;
			}
			finally
			{
			}
		}

		protected virtual void FFTGlobalBackgroundCorrection(CommonImage image, int threshold, float cutoff)
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

		protected virtual void RefImageGlobalBackgroundCorrection(CommonImage image, string[] FilePaths)
		{
			try
			{
				image.GlobalBackgroundCorrection(eGlobalBackgroundCorrectionType.ReferenceImages, FilePaths);
			}
			catch
			{
				throw;
			}
			finally
			{
			}
		}
		protected virtual void UnsharpGlobalBackgroundCorrection(CommonImage image, UnsharpParam args)
		{
			try
			{
				image.GlobalBackgroundCorrection(eGlobalBackgroundCorrectionType.UnsharpFilter, args);
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
