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
    public class DetectContaminationsCommand : RasterCommand
    {
        private ArrayList _detectedObjects = null;        

        public DetectContaminationsCommand(IProgressCallback callback)
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
                    "ABS_Contamination_NNModel.nns");

                this.SetStatusText("Recognizing...");

                int[] xList = null, yList = null;
                Recognize(nnsfile, featureSpace, 
                    featureSpace.ContaminationMask, 1, ref xList, ref yList);

                if (xList.Length == 0)
                    return;

                string detectedObjectFileSettings = 
                    Path.Combine(
                    SettingsHelper.GetDefaultABSInspectionSettingsFolderPath(), 
                    ContaminationsDetectionSettings.DefaultFileName);
                ContaminationsDetectionSettings settings =
                    ContaminationsDetectionSettings.Deserialize(detectedObjectFileSettings);
                double threshold = settings.LowerIntensityThreshold;

                binaryImage = Binarize(image, mask, xList, yList, threshold);

                _detectedObjects = ObjectDetection.DetectObject(image, binaryImage);
                ObjectDetection.UpdateObjectTypeId(_detectedObjects, (int)eDefectType.DarkObject);
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
            string nnsfile, FeatureSpace featureSpace, 
            int[] mask, int valMask, ref int[] xList, ref int[] yList)
        {
            xList = null;
            yList = null;

            NNClassifier classifier = new NNClassifier();
            classifier.InitNNController(nnsfile);

            int[] classIds = 
                classifier.ClassifyParallely(featureSpace.Features);

            if (classIds == null || classIds.Length == 0)
                return;

            List<int> xBlkList = new List<int>(100);
            List<int> yBlkList = new List<int>(100);
            int n = featureSpace.Features.Count;
            for (int i = 0; i < n; i++)
            {
                if (classIds[i] <= 0 || mask[i] != valMask)
                    continue;

                xBlkList.Add(featureSpace.XInterestedPoints[i]);
                yBlkList.Add(featureSpace.YInterestedPoints[i]);
            }

            xList = xBlkList.ToArray();
            yList = yBlkList.ToArray();
        }

        public static unsafe BinaryImage Binarize(
            CommonImage image, IMask mask, int[] xList, int[] yList,
            double threshold)
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

            int n = xList.Length;
            for (int i = 0; i < n; i++)
            {
                yStart = yList[i] - halfKernelSz;
                xStart = xList[i] - halfKernelSz;
                yEnd = yStart + kernelSize - 1;
                xEnd = xStart + kernelSize - 1;

                for (y = yStart; y <= yEnd; y++)
                {
                    index = y * w + xStart;
                    for (x = xStart; x <= xEnd; x++, index++)
                    {
                        if (mask[x, y] <= 0)
                            continue;

                        if (grayBuffer[index] <= threshold)
                            buffer[index] = true;
                    }
                }
            }

            return binary;
        }        
    }
}
