using System;
using System.Collections.Generic;
using System.Text;
using SIA.Common.Mask;
using SIA.SystemLayer;
using SIA.IPEngine;
using SIA.SystemLayer.Mask;
using System.IO;
using SiGlaz.Common;
using SiGlaz.Common.ABSInspectionSettings;
using SiGlaz.Common.ImageAlignment;

namespace SiGlaz.FeatureProcessing.Helpers
{
    public class ContaminationTexturalInfoHelperPoleTip
    {
        #region Member fields
        public const string DefaultMaskFileName = "PoleTip_Contamination_Region.msk";


        private static int _imageWidth = 0;
        private static int _imageHeight = 0;

        private static string _poletipSettingsPath = "";

        private static Settings _poletipAlignmentSettings = null;

        private static CommonImage _image = null;

        private static string _maskFile = "";
        private static IMask _mask = null;

        private static string _texturalExtractorSettingsFile = "";
        private static TexturalExtractorSettings _texturalExtractorSettings = null;

        private static int[] _xInterestedPoints = null;
        private static int[] _yInterestedPoints = null;
        private static int[] _contaminationMask = null;
        #endregion Member fields

        #region Public Methods
        public static bool Init()
        {
            int[] xInterestedPoints = ContaminationTexturalInfoHelper.XInterestedPoints;
            int[] yInterestedPoints = ContaminationTexturalInfoHelper.YInterestedPoints;

            return true;
        }

        public static string PoleTipSettingsPath
        {
            get
            {
                if (_poletipSettingsPath == "")
                {
                    _poletipSettingsPath =
                        SettingsHelper.GetDefaultPoleTipInspectionSettingsFolderPath();
                }
                return _poletipSettingsPath;
            }
        }

        public static Settings PoleTipAlignmentSettings
        {
            get
            {
                if (_poletipAlignmentSettings == null)
                {
                    string poletipSettingsPath = PoleTipSettingsPath;

                    string filePath =
                        Path.Combine(poletipSettingsPath, Settings.DefaultFileName_PoleTip);

                    _poletipAlignmentSettings = Settings.Deserialize(filePath);
                }

                return _poletipAlignmentSettings;
            }
        }

        public static TexturalExtractorSettings TexturalExtractorSettings
        {
            get
            {
                if (_texturalExtractorSettings == null)
                {
                    string poletipSettingsPath = PoleTipSettingsPath;

                    _texturalExtractorSettingsFile =
                        Path.Combine(poletipSettingsPath, TexturalExtractorSettings.DefaultFileName);

#if DEBUG
                    if (!File.Exists(_texturalExtractorSettingsFile))
                    {
                        TexturalExtractorSettings temp = new TexturalExtractorSettings();
                        temp.Serialize(_texturalExtractorSettingsFile);
                    }
#endif

                    _texturalExtractorSettings =
                        TexturalExtractorSettings.Deserialize(_texturalExtractorSettingsFile);
                }

                return _texturalExtractorSettings;
            }
        }

        public static IMask Mask
        {
            get
            {
                if (_mask == null)
                    _mask = CreateMask();
                return _mask;
            }
        }

        public static int[] XInterestedPoints
        {
            get
            {
                if (_xInterestedPoints == null)
                {
                    UpdateInterestedPoints();
                }

                return _xInterestedPoints;
            }
        }

        public static int[] YInterestedPoints
        {
            get
            {
                if (_yInterestedPoints == null)
                {
                    UpdateInterestedPoints();
                }

                return _yInterestedPoints;
            }
        }

        public static int[] ContaminationsMask
        {
            get
            {
                if (_contaminationMask == null)
                {
                    UpdateInterestedPoints();
                }

                return _contaminationMask;
            }
        }
        #endregion Public Methods

        #region Helpers
        private static CommonImage Image
        {
            get
            {
                if (_image == null)
                {
                    _image = CreateImage();
                }
                return _image;
            }
        }

        private static IMask CreateMask()
        {
            string poletipSettingsPath =
                SettingsHelper.GetDefaultPoleTipInspectionSettingsFolderPath();

            _maskFile = Path.Combine(poletipSettingsPath, DefaultMaskFileName);

            CommonImage image = Image;
            MaskHelper helper = new MaskHelper(image);
            IMask mask = helper.CreateMask(_maskFile);

            if (_image != null)
            {
                _image.Dispose();
                _image = null;
            }

            return mask;
        }

        private static CommonImage CreateImage()
        {
            // get image size
            Settings absAlignmentSettings = PoleTipAlignmentSettings;
            _imageWidth = absAlignmentSettings.NewWidth;
            _imageHeight = absAlignmentSettings.NewHeight;

            // create image
            GreyDataImage greyImage = new GreyDataImage(_imageWidth, _imageHeight);
            CommonImage image = new CommonImage(greyImage);

            return image;
        }

        private static void UpdateInterestedPoints()
        {
            IMask mask = Mask;

            int w = _imageWidth;
            int h = _imageHeight;

            TexturalExtractorSettings settings =
                ContaminationTexturalInfoHelperPoleTip.TexturalExtractorSettings;

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
            List<int> contaminationMask = new List<int>(n);
            int checkContamination = 0;

            for (int y = yStart; y <= yEnd; y += yStep)
            {
                for (int x = xStart; x <= xEnd; x += xStep)
                {
                    //if (mask[x, y] <= 0)
                    //    continue;

                    //xList.Add(x);
                    //yList.Add(y);

                    xList.Add(x);
                    yList.Add(y);

                    checkContamination = (mask[x, y] > 0) ? 1 : 0;
                    contaminationMask.Add(checkContamination);
                }
            }

            _xInterestedPoints = xList.ToArray();
            _yInterestedPoints = yList.ToArray();
            _contaminationMask = contaminationMask.ToArray();
        }
        #endregion Helpers
    }
}
