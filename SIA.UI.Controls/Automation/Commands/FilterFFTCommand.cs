//#define DEBUG_METETIME

using System;
using System.Drawing;
using System.Data;
using System.IO;
using System.Threading;
using System.Diagnostics;

using SIA.SystemFrameworks;
using SIA.SystemFrameworks.UI;
using SIA.Common.Imaging.Filters;

using SIA.IPEngine;
using SIA.SystemLayer;

using SIA.UI.Controls.Utilities;

using SIA.UI.Controls.Commands;
using SIA.Algorithms;

namespace SIA.UI.Controls.Automation.Commands
{
	/// <summary>
	/// Summary description for FilterFFTCommand.
	/// </summary>
	public class FilterFFTCommand : AutoCommand
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
			if (args.Length < 2)
				throw new ArgumentException("Not enough arguments", "arguments");
			if (args[0] is CommonImage == false)
				throw new ArgumentException("Argument type does not match. Arguments[0] must be CommonImage", "arguments");
			if (args[1] is FilterFFTCommandSettings == false)
				throw new ArgumentException("Argument type does not match. Arguments[1] must be FilterFFTCommandSettings", "arguments");			
		}

		protected override void OnRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;
			FilterFFTCommandSettings cmdSettings = (FilterFFTCommandSettings)args[1];

			FFTFilterType type = cmdSettings.Type;
			float cutoff = cmdSettings.CutOff;
			float weight = cmdSettings.Weight;

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

		public override void AutomationRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;
			FilterFFTCommandSettings cmdSettings = (FilterFFTCommandSettings)args[1];

			FFTFilterType type = cmdSettings.Type;
			float cutoff = cmdSettings.CutOff;
			float weight = cmdSettings.Weight;

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
			//String temp_path = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".rde";
			
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
				
				// force Garbage Collector to collect defects object
				GC.Collect();
				GC.Collect();
				GC.WaitForPendingFinalizers();
			}
		}

    }
}
