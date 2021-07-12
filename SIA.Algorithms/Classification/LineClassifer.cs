using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using SiGlaz.Common;
using System.IO;
using SIA.IPEngine;
using SIA.Algorithms.FeatureProcessing;
using SiGlaz.Common.ABSInspectionSettings;
using SIA.Algorithms.FeatureProcessing.Helpers;
using SIA.Algorithms.Classification.NN;

namespace SIA.Algorithms.Classification
{
    public class LineClassifer
    {
        public const string DefaultNNModelFileName = "ABS_LinePattern_NNModel.nns";

        public LineClassifer()
        {
        }

        public void Classify(List<LineClassifyingInput> input)
        {
            if (input == null || input.Count == 0)
                return;

            foreach (LineClassifyingInput lp in input)
            {
                Classify(lp);
            }
        }

        public static void WriteInputToBitmap(List<LineClassifyingInput> input,
            string outputFolder)
        {
            if (input == null || input.Count == 0)
                return;

            if (!Directory.Exists(outputFolder))
                throw new Exception(outputFolder + " does not exist");

            for (int i = 0; i < input.Count; i++)
            {
                LineClassifyingInput sample = input[i];
                GreyDataImage image = null;
                unsafe
                {
                    fixed (ushort* pdata = sample.Pattern)
                    {
                        image = new GreyDataImage((int)sample.Length, (int)sample.Thickness, pdata);
                    }
                }
                //image.SaveToBitmap(Path.Combine(outputFolder, i.ToString("000") + ".bmp"));
                image.SaveImage(Path.Combine(outputFolder, i.ToString("000") + ".bmp"), SIA.Common.eImageFormat.Bmp);
            }
        }

        #region classify uing Neural Network
        private void Classify(LineClassifyingInput input)
        {
            ushort[] data = input.Pattern;

            // extract features
            FeatureSpace featureSpace = 
                ExtractFeatures(input.Pattern, (int)input.Length, (int)input.Thickness);

            if (featureSpace == null|| featureSpace.Features == null || featureSpace.Features.Count == 0)
                return;

            // Neural Network classification
            string nnsfile = Path.Combine(
                SettingsHelper.GetDefaultABSInspectionSettingsFolderPath(), DefaultNNModelFileName);
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
                if (classIds[i] <= 0)
                    continue;

                xBlkList.Add(featureSpace.XInterestedPoints[i]);
                yBlkList.Add(featureSpace.YInterestedPoints[i]);
            }

            if (xBlkList.Count == 0)
                return;

            int sampleSize = (int)input.Thickness;
            List<RectangleF> detectedPositions = 
                Merge(xBlkList, yBlkList, sampleSize);

            input.DetectedPosition = detectedPositions;
        }

        private List<RectangleF> Merge(
            List<int> xBlkList, List<int> yBlkList, int sampleSize)
        {
            List<RectangleF> rects = new List<RectangleF>(xBlkList.Count);

            int halfSampleSize = sampleSize >> 1;
            int left = xBlkList[0] - halfSampleSize;
            int top = 0;
            int w = sampleSize;
            int h = sampleSize;

            RectangleF currentRect = new RectangleF(left, top, w, h);
            rects.Add(currentRect);
            int i = 1, n = xBlkList.Count;
            while (i < n)
            {
                left = xBlkList[i] - halfSampleSize;
                if (currentRect.Contains(xBlkList[i], yBlkList[i]))
                {
                    currentRect.Width = 
                        Math.Max(currentRect.Width, left + w - currentRect.Left);
                    currentRect.Height =
                        Math.Max(currentRect.Height, h);
                }
                else
                {
                    currentRect = new RectangleF(left, 0, w, h);
                    rects.Add(currentRect);
                }

                i++;
            }

            return rects;
        }

        private FeatureSpace ExtractFeatures(
            ushort[] data, int width, int height)
        {
            FeatureSpace featureSpace = null;

            int[] xInterestedPoints = null;
            int[] yInterestedPoints = null;
            TexturalExtractorSettings settings = LinePatternTexturalInfoHelper.Settings;
            LinePatternTexturalInfoHelper.GetInterestedPoints(
                width, height, settings, ref xInterestedPoints, ref yInterestedPoints);

            featureSpace =
                FeatureExtractor.ExtractFeatures_For_ABSLinePatternDetection(
                data, width, height, xInterestedPoints, yInterestedPoints, settings);

            return featureSpace;
        }
        #endregion classify uing Neural Network
    }
}
