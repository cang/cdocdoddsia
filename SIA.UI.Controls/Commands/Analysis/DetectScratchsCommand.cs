using System;
using System.Collections.Generic;
using System.Text;
using SIA.SystemFrameworks.UI;
using SIA.Common.Analysis;
using System.Drawing;
using SIA.SystemLayer;
using System.Collections;
using SIA.Algorithms.FeatureProcessing.Textural;
using SIA.Algorithms.FeatureProcessing;
using SIA.Algorithms.Classification.NN;
using SIA.SystemLayer.Mask;
using SiGlaz.RDE.Ex.Mask;
using SIA.Common.Mask;
using SIA.IPEngine;
using SIA.SystemLayer.ObjectExtraction.Utilities;
using System.Diagnostics;
using SIA.Algorithms.FeatureProcessing.Helpers;
using System.IO;
using SiGlaz.Common;
using SiGlaz.Common.ABSInspectionSettings;
using SiGlaz.Common.ABSDefinitions;

namespace SIA.UI.Controls.Commands
{
    public class DetectScratchsCommand : RasterCommand
    {
        private ArrayList _detectedObjects = null;

        public DetectScratchsCommand(IProgressCallback callback)
            : base(callback)
		{
		}

        public override object[] GetOutput()
        {
            return new object[] {_detectedObjects};
        }

        public override bool CanAbort
        {
            get { return false; }
        }

        protected override void ValidateArguments(params object[] args)
        {
            if (args == null)
                throw new ArgumentNullException("arguments");

            if (args[0] is CommonImage == false)
                throw new ArgumentException("Argument type does not match. Arguments[0] must be CommonImage", "arguments");
        }

        protected override void OnRun(params object[] args)
        {
            CommonImage image = args[0] as CommonImage;
            
            BinaryImage binaryImage = null;
            try
            {
                IMask mask = ContaminationTexturalInfoHelper.Mask;

                this.SetStatusText("Extracting Features...");
                FeatureSpace featureSpace = 
                    FeatureExtractor.ExtractFeatures_For_ABSContaminationsDetection(image);
                if (featureSpace == null)
                    return;

                string nnsfile = Path.Combine(
                    SettingsHelper.GetDefaultABSInspectionSettingsFolderPath(),
                    "ABS_Scratch_NNModel.nns");

                this.SetStatusText("Recognizing...");
                int[] xList = null, yList = null;
                Recognize(nnsfile, featureSpace, ref xList, ref yList);
                if (xList.Length == 0)
                    return;

                string detectedObjectFileSettings = 
                    Path.Combine(
                    SettingsHelper.GetDefaultABSInspectionSettingsFolderPath(), 
                    ContaminationsDetectionSettings.DefaultFileName);
                ContaminationsDetectionSettings settings =
                    ContaminationsDetectionSettings.Deserialize(detectedObjectFileSettings);
                double threshold = settings.HigherIntensityThreshold;
                int sampleSize = settings.SampleSize;
                Rectangle poleRect = 
                    new Rectangle(
                    settings.PoleX, settings.PoleY, 
                    settings.PoleWidth, settings.PoleHeight);

                binaryImage = Binarize(
                    image, mask, xList, yList, threshold, sampleSize, poleRect);

                _detectedObjects = ObjectDetection.DetectObject(image, binaryImage);
                _detectedObjects = UpdateDefectTypeAndFilter(_detectedObjects);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (binaryImage != null)
                {
                    binaryImage.Dispose();
                    binaryImage = null;
                }
            }
        }

        public static void Recognize(
            string nnsfile, FeatureSpace featureSpace, ref int[] xList, ref int[] yList)
        {
            xList = null;
            yList = null;

            NNClassifier classifier = new NNClassifier();
            classifier.InitNNController(nnsfile);

            int[] classIds = classifier.ClassifyParallely(featureSpace.Features);
            if (classIds == null || classIds.Length == 0)
                return;

            List<int> xBlkList = new List<int>(100);
            List<int> yBlkList = new List<int>(100);
            int n = featureSpace.Features.Count;
            for (int i = 0; i < n; i++)
            {
                if (classIds[i] <= 1)
                    continue;

                xBlkList.Add(featureSpace.XInterestedPoints[i]);
                yBlkList.Add(featureSpace.YInterestedPoints[i]);
            }

            xList = xBlkList.ToArray();
            yList = yBlkList.ToArray();
        }

        public static unsafe BinaryImage Binarize(
            CommonImage image, IMask mask, int[] xList, int[] yList,
            double threshold, int sampleSize, Rectangle poleRect)
        {
            int w = image.Width;
            int h = image.Height;
            BinaryImage binary = new BinaryImage(w, h);
            bool* buffer = (bool*)binary.Buffer.ToPointer();

            ushort* grayBuffer = image.RasterImage._aData;
            int kernelSize = ContaminationTexturalInfoHelper.TexturalExtractorSettings.KernelSize;
            int yStart = 0;
            int yEnd = 0;
            int xStart = 0;
            int xEnd = 0;
            int halfKernelSz = kernelSize >> 1;
            int x, y, index;

            if (sampleSize < kernelSize)
                sampleSize = kernelSize;
            int halfSampleSize = sampleSize >> 1;
            //int poleLeft = poleRect.Left;
            //int poleTop = poleRect.Top;
            //int poleRight = poleRect.Right;
            //int poleBottom = poleRect.Bottom;

            int n = xList.Length;
            for (int i = 0; i < n; i++)
            {
                yStart = yList[i] - halfSampleSize;
                xStart = xList[i] - halfSampleSize;
                yEnd = yStart + sampleSize - 1;
                xEnd = xStart + sampleSize - 1;

                for (y = yStart; y <= yEnd; y++)
                {
                    if (y < 0 || y >= h)
                        continue;

                    index = y * w + xStart;
                    for (x = xStart; x <= xEnd; x++, index++)
                    {
                        //if (mask[x, y] <= 0)
                        //    continue;

                        if (x < 0 || x >= w)
                            continue;

                        if (poleRect.Contains(x, y))
                            continue;

                        if (grayBuffer[index] >= threshold)
                            buffer[index] = true;
                    }
                }
            }

            return binary;
        }

        public static ArrayList UpdateDefectTypeAndFilter(ArrayList defectList)
        {
            if (defectList == null)
                return defectList;

            double minSize = 7;
            double minPixelCount = 50;

            int defectTypeId = (int)eDefectType.BrightObject;

            int n = defectList.Count;
            for (int i = n - 1; i >= 0; i--)
            {
                DetectedObject obj = defectList[i] as DetectedObject;
                if (obj.NumPixels < minPixelCount ||
                    (obj.RectBound.Width < minSize && obj.RectBound.Height < minSize))
                {
                    defectList.RemoveAt(i);
                }

                obj.ObjectTypeId = defectTypeId;
            }

            return defectList;
        }
    }
}
