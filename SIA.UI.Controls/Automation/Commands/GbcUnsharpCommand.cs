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

namespace SIA.UI.Controls.Automation.Commands
{
	/// <summary>
	/// Summary description for GbcUnsharpCommand.
	/// </summary>
	public class GbcUnsharpCommand : AutoCommand
	{
		public GbcUnsharpCommand(IProgressCallback callback) : base(callback)
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
			if (args[1] is GbcUnsharpCommandSettings == false)
				throw new ArgumentException("Argument type does not match. Arguments[1] must be GbcUnsharpCommandSettings", "arguments");
		}

		protected override void OnRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;

			GbcUnsharpCommandSettings cmdSettings = (GbcUnsharpCommandSettings)args[1];
			
			UnsharpParam param = cmdSettings.UnsharpSettings;

			this.SetStatusRange(0, 100);
			this.SetStatusText("Perform Global Background Correction using unsharp masking methodology...");

			this.GlobalBackgroundCorrection(image, param);			
		}

		public override void AutomationRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;

			GbcUnsharpCommandSettings cmdSettings = (GbcUnsharpCommandSettings)args[1];
			
			UnsharpParam param = cmdSettings.UnsharpSettings;

			this.GlobalBackgroundCorrection(image, param);			
		}

		protected virtual void GlobalBackgroundCorrection(CommonImage image, UnsharpParam args)
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
