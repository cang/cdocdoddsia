#define POLE_TIP_TEST


using System;
using System.Collections.Generic;
using System.Text;
using SIA.SystemLayer;
using SIA.IPEngine;
using SIA.SystemFrameworks.UI;
using SIA.SystemLayer.ImageProcessing;
using System.Drawing;
using SIA.Algorithms.ReferenceFile;

namespace SIA.UI.Controls.Commands
{
    public class SubtractGoldenImageCommand : RasterCommand
	{
        private GreyDataImage _result = null;
        public SubtractGoldenImageCommand(IProgressCallback callback) 
            : base(callback)
		{
		}

        public override object[] GetOutput()
        {
            return new object[] { _result };
        }

		protected override void ValidateArguments(params object[] args)
		{
			if (args == null)
				throw new ArgumentNullException("arguments");
			if (args.Length < 2)
				throw new ArgumentException("Not enough arguments", "arguments");
			if (args[0] is CommonImage == false)
                throw new ArgumentException("Argument type does not match. Arguments[0] must be CommonImage", "arguments");
			if (args[1] is string == false)
				throw new ArgumentException("Argument type does not match. Arguments[1] must be string", "arguments");
		}

		protected override void OnRun(params object[] args)
		{
            SubtractWithGoldenImage(args);
		}

        private void SubtractWithGoldenImage(params object[] args)
        {
            this.SetStatusText("Subtracting Golden Image...");

            CommonImage image = args[0] as CommonImage;
            string goldenImageFile = (string)args[1];

            GoldenImageProcessor processor = new GoldenImageProcessor();

#if POLE_TIP_TEST && !RELEASE
            processor.SetKernelSize(19, 19); // Pole tip
            processor.SetKernelSize(25, 25); // Pole tip
#else
            processor.SetKernelSize(7, 7); // ABS
            throw new System.Exception("Testing PoleTip...");
#endif

            using (Bitmap bmp = Image.FromFile(goldenImageFile) as Bitmap)
            {
                if (bmp.Width != image.Width ||
                    bmp.Height != image.Height)
                {
                    throw new System.Exception(
                        "Golden Image is not the same size with the processing image!");
                }

                using (GreyDataImage goldenImage = new GreyDataImage(bmp))
                {
                    GreyDataImage sampleImage = image.RasterImage;
                    processor.Subract(goldenImage, ref sampleImage);
                    _result = sampleImage;
                }
            }
            processor = null;
        }
	}
}
