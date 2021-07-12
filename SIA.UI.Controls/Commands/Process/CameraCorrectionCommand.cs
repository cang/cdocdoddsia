using System;
using System.Collections;
using System.Drawing;
using System.Data;
using System.IO;
using System.Threading;
using System.Diagnostics;

using SIA.SystemFrameworks;

using SIA.SystemFrameworks.UI;

using SIA.IPEngine;
using SIA.SystemLayer;
using SIA.SystemLayer.ObjectExtraction;

using SIA.UI.Controls.Utilities;
using SIA.UI.Controls.Automation;

namespace SIA.UI.Controls.Commands
{
	/// <summary>
	/// Represents camera correction command
	/// </summary>
	public class CameraCorrectionCommand 
        : RasterCommand
	{	
		public CameraCorrectionCommand(IProgressCallback callback) 
            : base(callback)
		{
		}

		protected override void ValidateArguments(params object[] args)
		{
			if (args == null)
				throw new ArgumentNullException("arguments");
			if (args.Length < 5)
				throw new ArgumentException("Not enough arguments", "arguments");
			if (args[0] is CommonImage == false)
				throw new ArgumentException("Argument type does not match. Arguments[0] must be CommonImage", "arguments");
			if (args[1] is float == false)
				throw new ArgumentException("Argument type does not match. Arguments[1] must be float", "arguments");
			if (args[2] is PointF == false)
				throw new ArgumentException("Argument type does not match. Arguments[1] must be PointF", "arguments");
			if (args[3] is float[] == false)
				throw new ArgumentException("Argument type does not match. Arguments[1] must be float[]", "arguments");
			if (args[4] is bool == false)
				throw new ArgumentException("Argument type does not match. Arguments[1] must be bool", "arguments");
		}

		protected override void OnRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;
			float focalLength = (float)args[1];
			PointF principalPoint = (PointF)args[2];
			float[] coeffs = (float[])args[3];
			bool inter = (bool)args[4];
			
			this.SetStatusRange(0, 100);
			this.SetStatusText("Correcting camera...");

			this.CorrectCamera(image, focalLength, principalPoint, coeffs, inter);
		}

		protected virtual void CorrectCamera(CommonImage image, float focalLength, PointF principalPoint, float[] distortionCoeffs, bool interpolation)
		{
			try
			{
				image.RemoveRadialDistortion(focalLength, principalPoint, distortionCoeffs, interpolation);
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
