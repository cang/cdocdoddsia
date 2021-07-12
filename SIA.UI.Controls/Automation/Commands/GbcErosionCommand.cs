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
	/// Summary description for GbcErosionCommand.
	/// </summary>
	public class GbcErosionCommand : AutoCommand
	{
		public GbcErosionCommand(IProgressCallback callback) : base(callback)
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
			if (args[1] is GbcErosionCommandSettings == false)
				throw new ArgumentException("Argument type does not match. Arguments[1] must be GbcErosionCommandSettings", "arguments");
		}

		protected override void OnRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;

			GbcErosionCommandSettings cmdSettings = (GbcErosionCommandSettings)args[1];

			int num_pass = cmdSettings.NumPass;

			this.SetStatusRange(0, 100);
			this.SetStatusText("Perform Global Background Correction using Erosion filter...");

			this.GlobalBackgroundCorrection(image, num_pass);			
		}

		public override void AutomationRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;

			GbcErosionCommandSettings cmdSettings = (GbcErosionCommandSettings)args[1];

			int num_pass = cmdSettings.NumPass;

			this.GlobalBackgroundCorrection(image, num_pass);			
		}

		protected virtual void GlobalBackgroundCorrection(CommonImage image, int num_pass)
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
	}
}
