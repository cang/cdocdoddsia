//#define GPU_SUPPORTED
//#define DEBUG_METETIME


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
using SIA.UI.Controls.Automation;
using SIA.Algorithms;

namespace SIA.UI.Controls.Commands
{
	/// <summary>
	/// Represents the kernel filter command
	/// </summary>
	public class ConvolutionCommand 
        : AutoCommand
	{
		public ConvolutionCommand(IProgressCallback callback) : base(callback)
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
			if (args[1] is ConvolutionCommandSettings == false)
				throw new ArgumentException("Argument type does not match. Arguments[1] must be ConvolutionCommandSettings", "arguments");			
		}

		protected override void OnRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;
			ConvolutionCommandSettings cmdSettings = (ConvolutionCommandSettings)args[1];
			
			eMaskType maskType = cmdSettings.MaskType;
			eMatrixType matrixType = cmdSettings.MatrixType;
			int num_pass = cmdSettings.NumPass;
			float threshold = cmdSettings.Threshold;

			this.SetStatusRange(0, 100);
			this.SetStatusText("Filtering image...");

#if DEBUG_METETIME
            DebugMeteTime dm = new DebugMeteTime();
#endif

			this.Convolution(image, maskType, matrixType, num_pass, threshold);

#if DEBUG_METETIME
            dm.AddLine("ConvolutionCommand:Run");
            dm.Write2Debug(true);
#endif

		}

		public override void AutomationRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;
			ConvolutionCommandSettings cmdSettings = (ConvolutionCommandSettings)args[1];
			
			eMaskType maskType = cmdSettings.MaskType;
			eMatrixType matrixType = cmdSettings.MatrixType;
			int num_pass = cmdSettings.NumPass;
			float threshold = cmdSettings.Threshold;

			this.Convolution(image, maskType, matrixType, num_pass, threshold);
		}

#if GPU_SUPPORTED
		protected virtual void Convolution(CommonImage image, eMaskType maskType, eMatrixType matrixType, int num_pass, float threshold)
		{
			try
			{
                //this code will be removed in feature
                bool bCreateBuff = !image.HasDeviceBuffer;
                if (bCreateBuff)
                    image.CreateDeviceBuffer();

				image.kApplyFilterGPU(maskType, matrixType, num_pass, threshold);

                //this code will be removed in feature
                image.ReadDataFromDeviceBuffer();
                if (bCreateBuff)
                    image.DisposeDeviceBuffer();
			}
			catch
			{
				throw;
			}
			finally
			{
			}
		}
#else
        protected virtual void Convolution(CommonImage image, eMaskType maskType, eMatrixType matrixType, int num_pass, float threshold)
		{
			try
			{
				image.kApplyFilter(maskType, matrixType, num_pass, threshold);
			}
			catch
			{
				throw;
			}
			finally
			{
			}
		}
#endif
    }
}
