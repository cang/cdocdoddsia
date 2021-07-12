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
	/// Summary description for ExtBckgndFFTCommand.
	/// </summary>
	public class ExtBckgndFFTCommand : RasterCommand
	{
		public ExtBckgndFFTCommand(IProgressCallback callback) : base(callback)
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
			if (args[1] is int == false)
				throw new ArgumentException("Argument type does not match. Arguments[1] must be integer", "arguments");
			if (args[2] is float == false)
				throw new ArgumentException("Argument type does not match. Arguments[2] must be float", "arguments");
		}

		protected override void OnRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;
			int threshold = (int)args[1];
			float cutoff = (float)args[2];

			this.SetStatusRange(0, 100);
			this.SetStatusText("Extracting Global Background using FFT...");

			this.ExtractGlobalBackground(image, threshold, cutoff);			
		}

		protected virtual void ExtractGlobalBackground(CommonImage image, int threshold, float cutoff)
		{
			try
			{
				image.ExtractGlobalBackgroundByFFT(threshold, cutoff);
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
