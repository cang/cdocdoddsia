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

namespace SIA.UI.Controls.Commands
{
	/// <summary>
	/// Summary description for SaveImageCommand.
	/// </summary>
	public class SaveImageCommand : RasterCommand
	{
		public SaveImageCommand(IProgressCallback callback) : base(callback)
		{
		}

		protected override void ValidateArguments(params object[] args)
		{
			if (args == null)
				throw new ArgumentNullException("arguments");
			if (args.Length < 3)
				throw new ArgumentException("Not enough arguments", "arguments");
			if (args[0] is CommonImage == false)
				throw new ArgumentException("Argument type does not match. Arguments[0] must be CommonImage", "arguments");
			if (args[1] is String == false)
				throw new ArgumentException("Argument type does not match. Arguments[1] must be String", "arguments");
			if (args[2] is eImageFormat == false)
				throw new ArgumentException("Argument type does not match. Arguments[1] must be eImageFormat", "arguments");
		}

		protected override void OnRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;
			string fileName = args[1] as String;
			eImageFormat format = (eImageFormat)args[2];

			this.SetStatusRange(0, 100);
			this.SetStatusText("Saving image...");

            this.SaveImage(image, fileName, format);			
		}

		protected virtual void SaveImage(CommonImage image, string fileName, eImageFormat format)
		{
			image.SaveImage(fileName, format);
		}
	}
}
