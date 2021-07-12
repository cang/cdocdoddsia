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

namespace SIA.UI.Controls.Commands
{
	/// <summary>
	/// Summary description for ExtBckgndErosionCommand.
	/// </summary>
	public class ExtBckgndErosionCommand : RasterCommand
	{
		public ExtBckgndErosionCommand(IProgressCallback callback) : base(callback)
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
			if (args[1] is int == false)
				throw new ArgumentException("Argument type does not match. Arguments[1] must be integer", "arguments");
		}

		protected override void OnRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;
			int num_pass = (int)args[1];

			this.SetStatusRange(0, 100);
			this.SetStatusText("Extracting Global Background using Erosion filter...");

			this.ExtractGlobalBackground(image, num_pass);			
		}

		protected virtual void ExtractGlobalBackground(CommonImage image, int num_pass)
		{
			try
			{
				image.ExtractGlobalBackgroundByErosion(num_pass);
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
