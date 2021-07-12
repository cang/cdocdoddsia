#define DEBUG_METETIME_

#define APPLY_ADAPTIVE_FILTER

#define POLE_TIP_TEST

#define TEST__


/* for optimize
CreateDeviceBuffer: 67
CompareImageToRefImage: 285135
ObjectDetectionHelper.Preparing: 3091
DetectObject: 132095

Totals :420388
*/ 

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
using SiGlaz.Common.ImageAlignment;
using SIA.Algorithms.Preprocessing.Alignment;
using System.Drawing.Drawing2D;
using SIA.Algorithms;

namespace SIA.UI.Controls.Commands
{
    public class DetectAnomaliesCommand : AutoCommand
    {
        private ArrayList _resultObjects = null;
        public DetectAnomaliesCommand(IProgressCallback callback)
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
            //if (args[1] is string == false)
            //    throw new ArgumentException("Argument type does not match. Arguments[1] must be string", "arguments");
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
            eGoldenImageMethod method = eGoldenImageMethod.Median;
            double darkThreshold = 80;
            double brightThreshold = 190;
            method = (args.Length >= 3 ? ((eGoldenImageMethod)args[2]) : method);
            darkThreshold = (args.Length >= 4 ? ((double)args[3]) : darkThreshold);
            brightThreshold = (args.Length >= 5 ? ((double)args[4]) : brightThreshold);


            AlignmentResult alignmentResult = null;
            if (args.Length >= 6 && args[5] != null)
                alignmentResult = args[5] as AlignmentResult;

            SiGlaz.Common.GraphicsList regions = null;
            if (args.Length >= 7)
            {
                regions = (args[6] == null ? null : (args[6] as SiGlaz.Common.GraphicsList));
            }

            CommonImage image = args[0] as CommonImage;
            MetrologySystemReference refFile = null;
            if (args[1] != null && args[1] is string)
            {
                string refFilePath = (string)args[1];
                refFile = MetrologySystemReference.Deserialize(refFilePath);
            }
            else if (args[1] != null && args[1] is MetrologySystemReference)
            {
                refFile = args[1] as MetrologySystemReference;
            }

            ArrayList ret = DetectObject(
                refFile, alignmentResult, image, method, darkThreshold, brightThreshold, regions);

            return ret;
        }

        public static ArrayList DetectObject(
            MetrologySystemReference refFile, AlignmentResult alignmentResult,
            CommonImage sampleImage,
            eGoldenImageMethod method, double darkThreshold, double brightThreshold, SiGlaz.Common.GraphicsList regions)
        {

#if DEBUG_METETIME
            DebugMeteTime dm = new DebugMeteTime();
#endif

#if GPU_SUPPORTED
            sampleImage.CreateDeviceBuffer();//temporary will be moved in to CommonImage
            #if DEBUG_METETIME
            dm.AddLine("CreateDeviceBuffer");
            #endif
#endif

            BinaryImage binarizedImage = null;
            GraphicsPath dotnetPaths = null;
            Region dotnetRegions = null;
            try
            {
                if (regions != null)
                {
                    dotnetPaths = regions.GetGraphicsPath();
                    dotnetRegions = new Region(dotnetPaths);
                }

                int[] interestedPixels = null;

#if GPU_SUPPORTED
                if (alignmentResult == null)
                    interestedPixels = ReferenceImageProcessorEx.CompareImageToRefImage(
                        refFile, sampleImage, darkThreshold, brightThreshold, dotnetRegions);
                else
                    interestedPixels = ReferenceImageProcessorEx.CompareImageToRefImage(
                        refFile, alignmentResult,sampleImage, darkThreshold, brightThreshold, dotnetRegions);
#else
                if (alignmentResult == null)
                    interestedPixels = ReferenceImageProcessorEx.CompareImageToRefImage(
                        refFile, sampleImage.RasterImage, darkThreshold, brightThreshold, dotnetRegions);
                else
                    interestedPixels = ReferenceImageProcessorEx.CompareImageToRefImage(
                        refFile, alignmentResult, sampleImage.RasterImage, darkThreshold, brightThreshold, dotnetRegions);
#endif



#if DEBUG_METETIME
                dm.AddLine("CompareImageToRefImage");
#endif
                if (interestedPixels == null || interestedPixels.Length == 0)
                    return new ArrayList();
#if TEST
                System.Diagnostics.Trace.WriteLine(
                    string.Format("Interested pixelsssssssss 1: {0}", interestedPixels.Length));
#endif

                Rectangle[] rois = null;
                int[] roiIdxStarts = null;
                int[] roiIdxEnds = null;

                ObjectDetectionHelper.Preparing(
                    sampleImage.Width, sampleImage.Height,
                    ref interestedPixels, ref rois, ref roiIdxStarts, ref roiIdxEnds);

#if DEBUG_METETIME
                dm.AddLine("ObjectDetectionHelper.Preparing");
#endif

#if TEST
                System.Diagnostics.Trace.WriteLine(
                    string.Format("Interested pixelsssssssss 2: {0}", interestedPixels.Length));
#endif


                ArrayList result =
                    SIA.Algorithms.ObjectDetector.DetectObject(
                    sampleImage.RasterImage, interestedPixels, rois, roiIdxStarts, roiIdxEnds);

#if DEBUG_METETIME
                dm.AddLine("DetectObject");
                dm.Write2Debug(true);
#endif

#if TEST
                System.Diagnostics.Trace.WriteLine(
                    string.Format("Counttttttttttttttttttttt 1: {0}", result.Count));
#endif

                if (result == null || result.Count == 0)
                    return new ArrayList();

                // update object type
                darkThreshold = 127.5;
                brightThreshold = 127.5;
                SIA.Algorithms.ObjectDetector.UpdateObjectTypeId(
                    result,
                    (int)eDefectType.DarkObject, darkThreshold,
                    (int)eDefectType.BrightObject, brightThreshold);

                // filter dark object(s) are too small
                SimpleFilterCommand.FilterTooSmallObject(
                    result, (int)eDefectType.DarkObject, 3.5);

                // filter bright object(s) are too small
                SimpleFilterCommand.FilterTooSmallObject(
                    result, (int)eDefectType.BrightObject, 8.5);



#if TEST
                System.Diagnostics.Trace.WriteLine(
                    string.Format("Counttttttttttttttttttttt 2: {0}", result.Count));
#endif

                return result;
            }
            catch
            {
                throw;
            }
            finally
            {
#if GPU_SUPPORTED
                sampleImage.DisposeDeviceBuffer();//temporary will be moved in to CommonImage
#endif

                if (binarizedImage != null)
                {
                    binarizedImage.Dispose();
                    binarizedImage = null;
                }

                if (dotnetRegions != null)
                {
                    dotnetRegions.Dispose();
                    dotnetRegions = null;
                }

                if (dotnetPaths != null)
                {
                    dotnetPaths.Dispose();
                    dotnetPaths = null;
                }
            }
        }
    }
}
