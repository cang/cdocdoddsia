using System;
using System.Collections.Generic;
using System.Text;
using SiGlaz.Common.ABSInspectionSettings;
using System.IO;
using SiGlaz.Common;

namespace SIA.Algorithms.FeatureProcessing.Helpers
{
    public class LinePatternTexturalInfoHelper
    {
        public const string DefaultSettingFileName = "LinePatternTexturalFeatureExtractionSettings.settings";

        private static TexturalExtractorSettings _settings = null;
        public static TexturalExtractorSettings Settings
        {
            get
            {
                if (_settings == null)
                {
                    string filePath = Path.Combine(
                        SettingsHelper.GetDefaultABSInspectionSettingsFolderPath(), DefaultSettingFileName);

#if DEBUG
                    if (!File.Exists(filePath))
                    {
                        TexturalExtractorSettings settings = 
                            new TexturalExtractorSettings();

                        settings.KernelSize = 33;
                        settings.Serialize(filePath);
                    }
#endif

                    _settings = TexturalExtractorSettings.Deserialize(filePath);
                }

                return _settings;
            }
        }

        public static void GetInterestedPoints(
            int width, int height, TexturalExtractorSettings settings,
            ref int[] xInterestedPoints, ref int[] yInterestedPoints)
        {
            if (settings == null)
                settings = LinePatternTexturalInfoHelper.Settings;

            int w = width;
            int h = height;

            int kernelSize = settings.KernelSize;
            int halfKernelSz = kernelSize >> 1;
            int yStart = settings.PaddingTop;
            int yEnd = h - settings.PaddingBottom - 1;
            int xStart = settings.PaddingLeft;
            int xEnd = w - settings.PaddingRight - 1;

            double stepFactor = settings.JumpStepFactor;
            int yStep = (int)(kernelSize * stepFactor);
            int xStep = (int)(kernelSize * stepFactor);
            int n = (w / xStep) * (h / yStep) + 1000;
            List<int> xList = new List<int>(n);
            List<int> yList = new List<int>(n);

            for (int y = yStart; y <= yEnd; y += yStep)
            {
                for (int x = xStart; x <= xEnd; x += xStep)
                {
                    xList.Add(x);
                    yList.Add(y);
                }
            }

            xInterestedPoints = xList.ToArray();
            yInterestedPoints = yList.ToArray();
        }
    }
}
