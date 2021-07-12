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
	/// Summary description for GbcRefImageCommand.
	/// </summary>
	public class GbcRefImageCommand : AutoCommand
	{
		public GbcRefImageCommand(IProgressCallback callback) : base(callback)
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
			if (args[1] is GbcRefImageCommandSettings == false)
				throw new ArgumentException("Argument type does not match. Arguments[1] must be GbcRefImageCommandSettings", "arguments");
		}

		protected override void OnRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;

			GbcRefImageCommandSettings cmdSettings = (GbcRefImageCommandSettings)args[1];
			
			string[] FilePaths = cmdSettings.FilePaths;

			this.SetStatusRange(0, 100);
			this.SetStatusText("Perform Global Background Correction using reference images...");

			this.GlobalBackgroundCorrection(image, FilePaths);			
		}

		public override void AutomationRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;

			GbcRefImageCommandSettings cmdSettings = (GbcRefImageCommandSettings)args[1];
			
			string[] FilePaths = cmdSettings.FilePaths;

			this.GlobalBackgroundCorrection(image, FilePaths);			
		}

		protected virtual void GlobalBackgroundCorrection(CommonImage image, string[] FilePaths)
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
	}
}
