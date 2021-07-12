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
using SIA.Algorithms.Classification;
using SiGlaz.Common.ABSDefinitions;

namespace SIA.UI.Controls.Commands
{
    public class DetectLinePatternCommand : RasterCommand
    {
        private ArrayList _detectedObjects = null;

        public DetectLinePatternCommand(IProgressCallback callback)
            : base(callback)
		{
		}

        public override object[] GetOutput()
        {
            return new object[] { _detectedObjects };
        }

        public override bool CanAbort
        {
            get { return false; }
        }

        protected override void ValidateArguments(params object[] args)
        {
            if (args == null)
                throw new ArgumentNullException("arguments");
            if (args.Length < 1)
                throw new ArgumentException("Not enough arguments", "arguments");
            if (args[0] is CommonImage == false)
                throw new ArgumentException("Argument type does not match. Arguments[0] must be CommonImage", "arguments");
            //if (args[1] is LinePatternLibrary == false)
            //    throw new ArgumentException("Argument type does not match. Arguments[1] must be LinePatternLibrary", "arguments");
        }

        protected override void OnRun(params object[] args)
        {
            CommonImage image = args[0] as CommonImage;
            
            BinaryImage binaryImage = null;
            try
            {
                _detectedObjects = new ArrayList();
                foreach (string filename in LinePatternLibrary.MultiplePatternFilenames)
                {
                    LinePatternLibrary settings = LinePatternLibrary.Deserialize(Path.Combine(
                        SettingsHelper.GetDefaultABSInspectionSettingsFolderPath(),
                        filename));
                    if (settings == null)
                        continue;

                    this.SetStatusText("Recognizing...");

                    List<RectangleF> results = Classify(image, settings);

                    if (results == null || results.Count == 0)
                        continue;

                    double threshold = settings.Threshold;
                    binaryImage = GetBinary(settings, image, results);

                    ArrayList iterResult = 
                        ObjectDetection.DetectObject(image, binaryImage);
                    if (iterResult != null && iterResult.Count > 0)
                    {
                        UpdateDefectTypeAndFilter(iterResult, settings);
                        _detectedObjects.AddRange(iterResult);
                    }
                }
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

        public static List<RectangleF> Classify(CommonImage image, LinePatternLibrary settings)
        {            
            ABSPatternClassifier lineClassifier = new ABSPatternClassifier(settings);
            unsafe
            {
                List<RectangleF> result = lineClassifier.Classify(image.RasterImage._aData, image.RasterImage._width, image.RasterImage._height);
                if (result == null || result.Count == 0)
                    return null;
                return result;
            }
        }

        public static unsafe BinaryImage GetBinary(LinePatternLibrary settings, CommonImage image, List<RectangleF> results)
        {
            int w = image.Width;
            int h = image.Height;
            BinaryImage binary = new BinaryImage(w, h);
            bool* buffer = (bool*)binary.Buffer.ToPointer();

            ushort* grayBuffer = image.RasterImage._aData;
            int yStart = 0;
            int yEnd = 0;
            int xStart = 0;
            int xEnd = 0;
            int x, y, index;
            int threshold = (int)settings.Threshold;
            int thresholdPositive = (int)settings.ThresholdMax;

            int n = results.Count;
            for (int i = 0; i < n; i++)
            {
                yStart = (int)results[i].Top;
                xStart = (int)results[i].Left;
                yEnd = (int)results[i].Bottom;
                xEnd = (int)results[i].Right;

                for (y = yStart; y <= yEnd; y++)
                {
                    if (y < 0)
                        continue;
                    if (y >= h)
                        break;
                    index = y * w + xStart;
                    for (x = xStart; x <= xEnd; x++, index++)
                    {
                        if (x < 0)
                            continue;
                        if (x >= w)
                            break;

                        if (grayBuffer[index] <= threshold || grayBuffer[index] >= thresholdPositive)
                            buffer[index] = true;
                    }
                }
            }

            return binary;
        }

        public static unsafe BinaryImage GetBinary_Center(LinePatternLibrary settings, CommonImage image, List<RectangleF> results)
        {
            int w = image.Width;
            int h = image.Height;
            BinaryImage binary = new BinaryImage(w, h);
            bool* buffer = (bool*)binary.Buffer.ToPointer();

            ushort* grayBuffer = image.RasterImage._aData;
            int yStart = 0;
            int yEnd = 0;
            int xStart = 0;
            int xEnd = 0;
            int x, y, index;
            int threshold = (int)settings.Threshold;

            int n = results.Count;
            for (int i = 0; i < n; i++)
            {
                yStart = (int)results[i].Top;
                xStart = (int)results[i].Left;
                yEnd = (int)results[i].Bottom;
                xEnd = (int)results[i].Right;

                int yMid = (yStart + yEnd) / 2;
                int xMid = (xStart + xEnd) / 2;
                xStart = xMid - 2;
                xEnd = xMid + 2;
                yStart = yMid - 2;
                yEnd = yMid + 2;

                for (y = yStart; y <= yEnd; y++)
                {
                    if (y < 0)
                        continue;
                    if (y >= h)
                        break;
                    index = y * w + xStart;
                    for (x = xStart; x <= xEnd; x++, index++)
                    {
                        if (x < 0)
                            continue;
                        if (x >= w)
                            break;

                        buffer[index] = true;
                    }
                }
            }

            return binary;
        }

        public static void UpdateDefectTypeAndFilter(
            ArrayList defectList, LinePatternLibrary settings)
        {
            if (defectList == null || defectList.Count == 0)
                return;

            double val = 0;
            double threshold1 = settings.Threshold + 5;
            double threshold2 = settings.ThresholdMax - 5;
            for (int i = defectList.Count - 1; i >= 0; i--)
            {
                DetectedObject defect = defectList[i] as DetectedObject;                

                val = defect.TotalIntensity / defect.NumPixels;
                if (val < threshold1 && defect.NumPixels < settings.MinPointCountNegative)
                {
                    defectList.RemoveAt(i);
                    continue;
                }

                if (val > threshold2 && defect.NumPixels < settings.MinPointCountPositive)
                {
                    defectList.RemoveAt(i);
                    continue;
                }

                if (val < threshold1)
                {
                    defect.ObjectTypeId = (int)eDefectType.DarkObjectAcrossBoundary;
                }
                else defect.ObjectTypeId = (int)eDefectType.BrightObjectAcrossBoundary;
            }
        }

    }
}
