#define APPLY_ADAPTIVE_FILTER

#define POLE_TIP_TEST

using System;
using System.Collections.Generic;
using System.Text;
using SIA.SystemLayer;
using SIA.UI.Controls.Automation;
using SIA.SystemFrameworks.UI;
using System.Collections;
using SIA.SystemLayer.ImageProcessing;
using SIA.IPEngine;
using System.Drawing;
using SiGlaz.Common.ABSDefinitions;
using SIA.Algorithms.ReferenceFile;

namespace SIA.UI.Controls.Commands
{
    public class DetectObjectUsingGoldenImageCommand : AutoCommand
	{
        private ArrayList _resultObjects = null;
        public DetectObjectUsingGoldenImageCommand(IProgressCallback callback) 
            : base(callback)
		{
		}

        public override object[] GetOutput()
        {
            return new object[] { _resultObjects };
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
			_resultObjects = DetectObject(args);
		}

		public override void AutomationRun(params object[] args)
		{
			_resultObjects = DetectObject(args);
		}

        protected virtual ArrayList DetectObject(params object[] args)
        {
            // all parameters should be stored in settings
            eGoldenImageMethod method = eGoldenImageMethod.None;
            double darkThreshold = 80;
            double brightThreshold = 190;
            method = (args.Length >= 3 ? ((eGoldenImageMethod)args[2]) : method);
            darkThreshold = (args.Length >= 4 ? ((double)args[3]) : darkThreshold);
            brightThreshold = (args.Length >= 5 ? ((double)args[4]) : brightThreshold);

            CommonImage image = args[0] as CommonImage;
            string goldenImageFilePath = (string)args[1];

            using (Bitmap bmp = Image.FromFile(goldenImageFilePath) as Bitmap)
            {
                if (bmp.Width != image.Width ||
                    bmp.Height != image.Height)
                {
                    throw new System.Exception(
                        "Golden Image is not the same size with the processing image!");
                }

                using (GreyDataImage goldenImage = new GreyDataImage(bmp))
                {
                    return DetectObject(
                        goldenImage, image, method, darkThreshold, brightThreshold);
                }
            }
        }

        public static ArrayList DetectObject(
            GreyDataImage goldenImage, CommonImage sampleImage, 
            eGoldenImageMethod method, double darkThreshold, double brightThreshold)
        {
            GoldenImageProcessor processor = new GoldenImageProcessor();

#if POLE_TIP_TEST && !RELEASE
            processor.SetKernelSize(19, 19); // Pole tip
            processor.SetKernelSize(25, 25); // Pole tip
#else
            processor.SetKernelSize(7, 7); // ABS
            throw new System.Exception("Testing PoleTip...");
#endif



            try
            {
#if DEBUG
                DateTime started = DateTime.Now;
#endif

#if APPLY_ADAPTIVE_FILTER
                CommonImage filteredImage = new CommonImage(sampleImage.RasterImage, true);                
#else
                CommonImage filteredImage = sampleImage;
#endif

#if DEBUG
                DateTime finished = DateTime.Now;
                Console.WriteLine(string.Format("Clone: {0}", (finished - started).TotalMilliseconds));

                double duration = (finished - started).TotalMilliseconds;

                started = DateTime.Now;
#endif

                {
#if DEBUG
                    started = DateTime.Now;
#endif

#if APPLY_ADAPTIVE_FILTER
                    FilterWienerCommand.FilterWiener(filteredImage, 9, 9, false, 0.01);
#else
#endif                    

#if DEBUG
                    finished = DateTime.Now;
                    Console.WriteLine(string.Format("Filter: {0}", (finished - started).TotalMilliseconds));

                    duration = (finished - started).TotalMilliseconds;

                    started = DateTime.Now;
#endif

#if APPLY_ADAPTIVE_FILTER
                    using (BinaryImage binImage = processor.Binarize(
                        goldenImage, filteredImage.RasterImage, sampleImage.RasterImage, darkThreshold, brightThreshold))
#else
                    using (BinaryImage binImage = processor.Binarize(
                        goldenImage, filteredImage.RasterImage, darkThreshold, brightThreshold))
#endif
                    {
#if DEBUG
                        finished = DateTime.Now;
                        Console.WriteLine(string.Format("Binarize: {0}", (finished - started).TotalMilliseconds));
                        duration = (finished - started).TotalMilliseconds;
                        started = DateTime.Now;
#endif

#if APPLY_ADAPTIVE_FILTER
                        // dispose temp image
                        filteredImage.Dispose();
                        filteredImage = null;
#else
#endif

                        ArrayList result = ObjectDetection.DetectObject(sampleImage, binImage);
                        if (result == null || result.Count == 0)
                            return new ArrayList();

#if DEBUG
                        finished = DateTime.Now;
                        Console.WriteLine(string.Format("Detect: {0}", (finished - started).TotalMilliseconds));
                        duration = (finished - started).TotalMilliseconds;
                        started = DateTime.Now;
#endif

                        // update object type
                        darkThreshold = brightThreshold - 2;
                        ObjectDetection.UpdateObjectTypeId(result,
                            (int)eDefectType.DarkObject, darkThreshold,
                            (int)eDefectType.BrightObject, brightThreshold - 1);

                        // filter dark object(s) are too small
                        SimpleFilterCommand.FilterTooSmallObject(
                            result, (int)eDefectType.DarkObject, 3.5);

                        // filter bright object(s) are too small
                        SimpleFilterCommand.FilterTooSmallObject(
                            result, (int)eDefectType.BrightObject, 8.5);                        

                        return result;
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                processor = null;
            }
        }
	}
}
