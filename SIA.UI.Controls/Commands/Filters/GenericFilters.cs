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
using SIA.Algorithms;

namespace SIA.UI.Controls.Commands
{
	/// <summary>
	/// Summary description for EdgeDetectionCommand.
	/// </summary>
	public class FilterEdgeDetectionCommand : RasterCommand
	{
		public FilterEdgeDetectionCommand(IProgressCallback callback) : base(callback)
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

			this.EdgeDetection(image);
		}

		protected virtual void EdgeDetection(CommonImage image)
		{
			try
			{
				image.FilterOutline();
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

	/// <summary>
	/// Summary description for Emboss90Command.
	/// </summary>
	public class FilterEmboss90Command : RasterCommand
	{
		public FilterEmboss90Command(IProgressCallback callback) : base(callback)
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

			this.Emboss90(image);
		}

		protected virtual void Emboss90(CommonImage image)
		{
			try
			{
				image.FilterEmboss90();
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

	/// <summary>
	/// Summary description for Emboss135Command.
	/// </summary>
	public class FilterEmboss135Command : RasterCommand
	{
		public FilterEmboss135Command(IProgressCallback callback) : base(callback)
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

			this.Emboss135(image);
		}

		protected virtual void Emboss135(CommonImage image)
		{
			try
			{
				image.FilterEmboss135();
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

	/// <summary>
	/// Summary description for GaussianCommand.
	/// </summary>
	public class FilterGaussianCommand : RasterCommand
	{
		public FilterGaussianCommand(IProgressCallback callback) : base(callback)
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
			this.SetStatusText("Gaussian filtering...");

#if DEBUG_METETIME
            DebugMeteTime dm = new DebugMeteTime();
#endif

			this.Gaussian(image);

#if DEBUG_METETIME
            dm.AddLine("FilterGaussianCommand:Run");
            dm.Write2Debug(true);
#endif

        }

#if GPU_SUPPORTED
		protected virtual void Gaussian(CommonImage image)
		{
			try
			{
                //this code will be removed in feature
                bool bCreateBuff = !image.HasDeviceBuffer;
                if (bCreateBuff)
                    image.CreateDeviceBuffer();

				image.GaussianSmoothFilterGPU();

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
        protected virtual void Gaussian(CommonImage image)
		{
			try
			{
				image.GaussianSmoothFilter();
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

	/// <summary>
	/// Summary description for LaplacianCommand.
	/// </summary>
	public class FilterLaplacianCommand : RasterCommand
	{
		public FilterLaplacianCommand(IProgressCallback callback) : base(callback)
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

			this.Laplacian(image);
		}

		protected virtual void Laplacian(CommonImage image)
		{
			try
			{
				image.LaplasianFilter();
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

	/// <summary>
	/// Summary description for SharpeningCommand.
	/// </summary>
	public class FilterSharpeningCommand : RasterCommand
	{
		public FilterSharpeningCommand(IProgressCallback callback) : base(callback)
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

			this.Sharpening(image);
		}

		protected virtual void Sharpening(CommonImage image)
		{
			try
			{
				image.ShapreningFilter();
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


	/// <summary>
	/// Summary description for SmoothCommand.
	/// </summary>
	public class FilterSmoothCommand : RasterCommand
	{
		public FilterSmoothCommand(IProgressCallback callback) : base(callback)
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

#if DEBUG_METETIME
            DebugMeteTime dm = new DebugMeteTime();
#endif
            this.Smooth(image);

#if DEBUG_METETIME
            dm.AddLine("FilterSmoothCommand:Run");
            dm.Write2Debug(true);
#endif

        }

#if GPU_SUPPORTED
		protected virtual void Smooth(CommonImage image)
		{
			try
			{
                //this code will be removed in feature
                bool bCreateBuff = !image.HasDeviceBuffer;
                if (bCreateBuff)
                    image.CreateDeviceBuffer();

				// image.SmoothFilter();
				// [20070618 Cong];  wrapper kFilter function
				eMaskType maskType = eMaskType.kMASK_SMOOTH;
				eMatrixType matrixType = eMatrixType.kMATRIX_3x3;
				int num_pass = 1;
				float threshold = 0.0f;
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
        protected virtual void Smooth(CommonImage image)
        {
            try
            {
                // image.SmoothFilter();
                // [20070618 Cong];  wrapper kFilter function
                eMaskType maskType = eMaskType.kMASK_SMOOTH;
                eMatrixType matrixType = eMatrixType.kMATRIX_3x3;
                int num_pass = 1;
                float threshold = 0.0f;
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
