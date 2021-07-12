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
	public enum FlipType : int
	{
		Horizontal = 1,
		Vertical = 2,
	}
	/// <summary>
	/// Summary description for FlipImageCommand.
	/// </summary>
	public class FlipImageCommand : RasterCommand
	{
		public FlipImageCommand(IProgressCallback callback) : base(callback)
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
			FlipType type = (FlipType)args[1];

			this.SetStatusRange(0, 100);
			this.SetStatusText("Flipping image...");

			this.FlipImage(image, type);			
		}

		protected virtual void FlipImage(CommonImage image, FlipType type)
		{
			try
			{
				switch(type)
				{
					case FlipType.Horizontal:
						image.FlipHoz();
						break;
					case FlipType.Vertical:
						image.FlipVer();
						break;
				}
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
