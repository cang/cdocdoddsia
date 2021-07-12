using System;
using System.Drawing;
using System.Data;
using System.IO;
using System.Threading;
using System.Diagnostics;

using SIA.Common;
using SIA.Common.Imaging;

using SIA.SystemFrameworks;
using SIA.SystemFrameworks.UI;

using SIA.IPEngine;
using SIA.SystemLayer;

using SIA.UI.Controls.Utilities;

using SIA.UI.Controls.Commands;
using SiGlaz.Common;

namespace SIA.UI.Controls.Automation.Commands
{
	/// <summary>
	/// Summary description for SaveImageCommand.
	/// </summary>
	public class SaveImageCommand : AutoCommand
	{
		public SaveImageCommand(IProgressCallback callback) : base(callback)
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
			if (args[1] is SaveImageCommandSettings == false)
				throw new ArgumentException("Argument type does not match. Arguments[1] must be SaveImageCommandSettings", "arguments");
		}

		protected override void OnRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;
			SaveImageCommandSettings cmdSettings = (SaveImageCommandSettings)args[1];

			string fileName = cmdSettings.FileName;
			eImageFormat format = cmdSettings.Format;

			this.SetStatusRange(0, 100);
			this.SetStatusText("Saving image...");

            //PathHelper.CreateMissingFolderAuto(fileName);

			this.SaveImage(image, fileName, format);			
		}

		public override void AutomationRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;
			SaveImageCommandSettings cmdSettings = (SaveImageCommandSettings)args[1];

			string fileName = cmdSettings.FileName;
			eImageFormat format = cmdSettings.Format;

            using (CommandProgressLocker locker = new CommandProgressLocker())
                this.SaveImage(image, fileName, format);			
		}

		protected virtual void SaveImage(CommonImage image, string fileName, eImageFormat format)
		{
            //string dir = Path.GetDirectoryName(fileName);
            //if (Directory.Exists(dir) == false)
            //    Directory.CreateDirectory(dir);

            //Trace.WriteLine("Save file to: " + fileName);

            PathHelper.CreateMissingFolderAuto(fileName);

            image.SaveImage(fileName, format);
		}
	}
}
