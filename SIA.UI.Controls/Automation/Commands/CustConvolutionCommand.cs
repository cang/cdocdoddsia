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

using SIA.UI.Controls.Commands;

namespace SIA.UI.Controls.Automation.Commands
{
	/// <summary>
	/// Summary description for CustConvolutionCommand.
	/// </summary>
	public class CustConvolutionCommand : AutoCommand
	{
		public CustConvolutionCommand(IProgressCallback callback) : base(callback)
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
			if (args[1] is CustConvolutionCommandSettings == false)
				throw new ArgumentException("Argument type does not match. Arguments[1] must be CustConvolutionCommandSettings", "arguments");			
		}

		protected override void OnRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;
			CustConvolutionCommandSettings cmdSettings = (CustConvolutionCommandSettings)args[1];

			int[,] matrix = cmdSettings.GetMatrix();//cmdSettings.Matrix;
			int num_pass = cmdSettings.NumPass;
			
			this.SetStatusRange(0, 100);
			this.SetStatusText("Filtering image...");

			this.Convolution(image, matrix, num_pass);
		}

		public override void AutomationRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;
			CustConvolutionCommandSettings cmdSettings = (CustConvolutionCommandSettings)args[1];

			int[,] matrix = cmdSettings.GetMatrix();//cmdSettings.Matrix;
			int num_pass = cmdSettings.NumPass;
						
			this.Convolution(image, matrix, num_pass);
		}

		protected virtual void Convolution(CommonImage image, int[,] matrix, int num_pass)
		{
			try
			{
				image.kApplyFilter(matrix, num_pass);
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
