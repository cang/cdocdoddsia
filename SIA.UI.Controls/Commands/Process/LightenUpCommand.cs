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

namespace SIA.UI.Controls.Commands
{
	/// <summary>
	/// Summary description for LightenUpCommand.
	/// </summary>
	public class LightenUpCommand : RasterCommand
	{
		public LightenUpCommand(IProgressCallback callback) : base(callback)
		{
		}

		protected override void ValidateArguments(params object[] args)
		{
			if (args == null)
				throw new ArgumentNullException("arguments");
			if (args.Length < 1)
				throw new ArgumentException("Not enough arguments", "arguments");
			if (args[0] is CommonImage == false)
				throw new ArgumentException("Argument type does not match. Arguments[0] must be CommonImage", "arguments");
		}

		protected override void OnRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;

			this.SetStatusRange(0, 100);
			this.SetStatusText("Lightening image...");

			this.LightenUp(image);
		}

		protected virtual void LightenUp(CommonImage image)
		{
			try
			{
				image.LightenUp();
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
