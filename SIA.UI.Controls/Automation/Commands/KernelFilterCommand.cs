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

using SIA.Common;
using SIA.UI.Controls.Commands;
using SIA.Algorithms;

namespace SIA.UI.Controls.Automation.Commands
{
	/// <summary>
	/// Summary description for KernelFilterCommand.
	/// </summary>
	public class KernelFilterCommand : AutoCommand
	{
		public KernelFilterCommand(IProgressCallback callback) : base(callback)
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
			if (args[1] is KernelFilterCommandSettings == false)
				throw new ArgumentException("Argument type does not match. Arguments[1] must be KernelFilterCommandSettings", "arguments");			
		}

		protected override void OnRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;
			KernelFilterCommandSettings cmdSettings = (KernelFilterCommandSettings)args[1];

#if DEBUG_METETIME
            DebugMeteTime dm = new DebugMeteTime();
#endif

			switch (cmdSettings.Type)
			{
				case eKernelFilterType.Convolution:
					eMaskType maskType1 = cmdSettings.Convolution.MaskType;
					eMatrixType matrixType1 = cmdSettings.Convolution.MatrixType;
					int num_pass1 = cmdSettings.Convolution.NumPass;
					float threshold = cmdSettings.Convolution.Threshold;

					this.Convolution(image, maskType1, matrixType1, num_pass1, threshold);					
					break;
				case eKernelFilterType.CustConvolution:
					int[,] matrix = cmdSettings.CustConvolution.GetMatrix();//cmdSettings.CustConvolution.Matrix;
					int num_pass2 = cmdSettings.CustConvolution.NumPass;
					
					this.CustConvolution(image, matrix, num_pass2);
					break;
				case eKernelFilterType.Morphology:
					eMorphType maskType2 = cmdSettings.MorphologySettings.MaskType;
					eMatrixType matrixType2 = cmdSettings.MorphologySettings.MatrixType;
					int num_pass3 = cmdSettings.MorphologySettings.NumPass;
					
					this.Morphology(image, maskType2, matrixType2, num_pass3);
					break;
			}

#if DEBUG_METETIME
            dm.AddLine("KernelFilterCommand:Run");
            dm.Write2Debug(true);
#endif
		}

		public override void AutomationRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;
			KernelFilterCommandSettings cmdSettings = (KernelFilterCommandSettings)args[1];

#if DEBUG_METETIME
            DebugMeteTime dm = new DebugMeteTime();
#endif

			switch (cmdSettings.Type)
			{
				case eKernelFilterType.Convolution:
					eMaskType maskType1 = cmdSettings.Convolution.MaskType;
					eMatrixType matrixType1 = cmdSettings.Convolution.MatrixType;
					int num_pass1 = cmdSettings.Convolution.NumPass;
					float threshold = cmdSettings.Convolution.Threshold;

					this.Convolution(image, maskType1, matrixType1, num_pass1, threshold);					
					break;
				case eKernelFilterType.CustConvolution:
					int[,] matrix = cmdSettings.CustConvolution.GetMatrix();//cmdSettings.CustConvolution.Matrix;
					int num_pass2 = cmdSettings.CustConvolution.NumPass;
					
					this.CustConvolution(image, matrix, num_pass2);
					break;
				case eKernelFilterType.Morphology:
					eMorphType maskType2 = cmdSettings.MorphologySettings.MaskType;
					eMatrixType matrixType2 = cmdSettings.MorphologySettings.MatrixType;
					int num_pass3 = cmdSettings.MorphologySettings.NumPass;
					
					this.Morphology(image, maskType2, matrixType2, num_pass3);
					break;
			}

#if DEBUG_METETIME
            dm.AddLine("KernelFilterCommand:Run");
            dm.Write2Debug(true);
#endif
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

		protected virtual void CustConvolution(CommonImage image, int[,] matrix, int num_pass)
		{
			try
			{
                //this code will be removed in feature
                bool bCreateBuff = !image.HasDeviceBuffer;
                if (bCreateBuff)
                    image.CreateDeviceBuffer();

                image.kApplyFilterGPU(matrix, num_pass);

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

		protected virtual void Morphology(CommonImage image, eMorphType maskType, eMatrixType matrixType, int num_pass)
		{
			try
			{
                //this code will be removed in feature
                bool bCreateBuff = !image.HasDeviceBuffer;
                if (bCreateBuff)
                    image.CreateDeviceBuffer();

                image.kApplyFilterGPU(maskType, matrixType, num_pass);

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

		protected virtual void CustConvolution(CommonImage image, int[,] matrix, int num_pass)
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

		protected virtual void Morphology(CommonImage image, eMorphType maskType, eMatrixType matrixType, int num_pass)
		{
			try
			{
				image.kApplyFilter(maskType, matrixType, num_pass);
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
