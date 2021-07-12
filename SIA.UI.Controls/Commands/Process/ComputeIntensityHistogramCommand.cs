using System;
using System.Drawing;
using System.Data;
using System.IO;
using System.Threading;
using System.Diagnostics;

using SIA.SystemLayer;
using SIA.SystemFrameworks.UI;
using SIA.IPEngine;

namespace SIA.UI.Controls.Commands
{
	/// <summary>
	/// Summary description for ShowIntensityHistogramCommand.
	/// </summary>
	public class ComputeIntensityHistogramCommand : RasterCommand
	{
		private kHistogram _histogram = null;

		public ComputeIntensityHistogramCommand(IProgressCallback callback) : base(callback)
		{
			//
			// TODO: Add constructor logic here
			//
		}

		protected override void ValidateArguments(params object[] args)
		{
			if (args == null)
				throw new ArgumentNullException("arguments");
			if (args.Length < 1)
				throw new ArgumentException("Not enough arguments", "arguments");
			if (args[0] is CommonImage == false)
				throw new ArgumentException("Argument type does not match. Arguments[0] must be EdgeExclusionZone", "arguments");
		}

		protected override void OnRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;			
			this.SetStatusRange(0, 100);			
			_histogram = image.Histogram;
		}

		public override object[] GetOutput()
		{
			return new object[] {_histogram};
		}
	}
}
