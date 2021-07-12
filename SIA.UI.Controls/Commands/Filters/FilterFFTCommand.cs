//#define DEBUG_METETIME


using System;
using System.Drawing;
using System.Data;
using System.IO;
using System.Threading;
using System.Diagnostics;

using SIA.Common;
using SIA.Common.Imaging;
using SIA.Common.Imaging.Filters;

using SIA.SystemFrameworks;
using SIA.SystemFrameworks.UI;

using SIA.IPEngine;
using SIA.SystemLayer;

using SIA.UI.Controls.Utilities;
using SIA.Algorithms;

namespace SIA.UI.Controls.Commands
{
	/// <summary>
	/// Summary description for FilterFFTCommand.
	/// </summary>
	public class FilterFFTCommand : RasterCommand
	{
		public FilterFFTCommand(IProgressCallback callback) : base(callback)
		{
		}

		public override bool CanAbort
		{
			get {return true;}
		}


		protected override void ValidateArguments(params object[] args)
		{
			if (args == null)
				throw new ArgumentNullException("arguments");
			if (args.Length < 3)
				throw new ArgumentException("Not enough arguments", "arguments");
			if (args[0] is CommonImage == false)
				throw new ArgumentException("Argument type does not match. Arguments[0] must be CommonImage", "arguments");
			if (args[1] is FFTFilterType == false)
				throw new ArgumentException("Argument type does not match. Arguments[1] must be FFTFilterType", "arguments");
			if (args[2] is float == false)
				throw new ArgumentException("Argument type does not match. Arguments[2] must be float", "arguments");
			if (args[3] is float == false)
				throw new ArgumentException("Argument type does not match. Arguments[3] must be float", "arguments");
		}

		protected override void OnRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;
			FFTFilterType type = (FFTFilterType)args[1];
			float cutoff = (float)args[2];
			float weight = (float)args[3];

			this.SetStatusRange(0, 100);
			this.SetStatusText("Filtering image...");

#if DEBUG_METETIME
            DebugMeteTime dm = new DebugMeteTime();
#endif

			this.FourierTransform(image, type, cutoff, weight);

#if DEBUG_METETIME
            dm.AddLine("FilterFFTCommand:Run");
            dm.Write2Debug(true);
#endif

		}

        protected virtual void FourierTransform(CommonImage image, FFTFilterType type, float cutoff, float weight)
		{
			String temp_path = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".rde";
			
			try
			{
				// [2006-12-19] Notes by Khoa:
				// FFT Filter do not need to save data for rollback because the 
				// Filter is automatically flush image data to disk before 
				// filtering image

				// save current data for rollback later
				// image.QuickSave(temp_path);

				// apply Fourier transform
				image.FFTFilter(type, cutoff, weight);
			}
			catch
			{
				//// rollback analyzed data
				//if (File.Exists(temp_path))
				//{
				//	image.QuickLoad(temp_path);
				//	File.Delete(temp_path);
				//}

				// rethrow exception
				throw;
			}
			finally
			{
				//// clear saved data
				//if (File.Exists(temp_path))
				//	File.Delete(temp_path);
			}
		}

    }
}
