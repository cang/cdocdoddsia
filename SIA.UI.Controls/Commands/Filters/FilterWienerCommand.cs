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
using SIA.Algorithms.FeatureProcessing.Filters;

namespace SIA.UI.Controls.Commands
{
    public class FilterWienerCommand : RasterCommand
	{
        public FilterWienerCommand(IProgressCallback callback)
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
			if (args[1] is int == false)
				throw new ArgumentException("Argument type does not match. Arguments[1] must be integer", "arguments");
            if (args[2] is int == false)
                throw new ArgumentException("Argument type does not match. Arguments[1] must be integer", "arguments");
			if (args[3] is bool == false)
				throw new ArgumentException("Argument type does not match. Arguments[1] must be boolean", "arguments");
            if (args[4] is double == false)
                throw new ArgumentException("Argument type does not match. Arguments[1] must be double", "arguments");
		}

		protected override void OnRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;
			int kernelWidth = (int)args[1];
			int kernelHeight = (int)args[2];
            bool isAuto = (bool)args[3];
            double noiseLevel = (double)args[4];

			this.SetStatusRange(0, 100);
			this.SetStatusText("Filtering image...");

            FilterWiener(image, kernelWidth, kernelHeight, isAuto, noiseLevel);
		}

        unsafe public static void FilterWiener(
            CommonImage image, int kernelWidth, int kernelHeight, bool isAuto, double noiseLevel)
		{
			try
			{
                FilterWiener(
                    image.RasterImage, kernelWidth, kernelHeight, isAuto, noiseLevel);
			}
			catch
			{
				throw;
			}
			finally
			{
			}
		}

        unsafe public static void FilterWiener(
            GreyDataImage image, int kernelWidth, int kernelHeight, bool isAuto, double noiseLevel)
        {
            try
            {

#if DEBUG
                Stopwatch sw = new Stopwatch(); sw.Start();
#endif

                WienerProcessor processor = null;


#if GPU_SUPPORTED
                processor = new WienerProcessorGPU(
                    image._aData, image.Width, image.Height,
                    kernelWidth, kernelHeight);
#else
                processor = new WienerProcessor(
                    image._aData, image.Width, image.Height,
                    kernelWidth, kernelHeight);
#endif


                processor.Filter(isAuto,noiseLevel);

                processor = null;
#if DEBUG
                sw.Stop(); Debug.WriteLine("FilterWiener :" + sw.ElapsedTicks);
#endif

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
