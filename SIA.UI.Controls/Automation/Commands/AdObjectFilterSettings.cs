using System;
using System.Collections.Generic;
using System.Text;
using SIA.Common.KlarfExport;
using SIA.UI.Controls.Commands;
using System.Windows.Forms;
using System.IO;

using SiGlaz.Common;
using SiGlaz.ObjectAnalysis.Common;

namespace SIA.UI.Controls.Automation.Commands
{
    public class AdObjectFilterSettings : AutoCommandSettings
    {
        public static string SIADefaultFilePath
        {
            get
            {
                string userPath = Application.UserAppDataPath;
                string filePath =
                    Path.Combine(userPath,
                    string.Format("SIA\\Settings\\AdObjectFilterSettings.xml"));
                PathHelper.CreateMissingFolderAuto(filePath);

                return filePath;
            }
        }

        public static string SIAWDefaultFilePath
        {
            get
            {
                string userPath = Application.UserAppDataPath;
                string filePath =
                    Path.Combine(userPath,
                    string.Format("SIAW\\Settings\\AdObjectFilterSettings.xml"));
                PathHelper.CreateMissingFolderAuto(filePath);

                return filePath;
            }
        }


        //public bool ApplyFilterForDarkObject = false;
        //public ObjectFilterArguments DarkObjectFilterSettings = new ObjectFilterArguments();

        //public bool ApplyFilterForBrightObject = false;
        //public ObjectFilterArguments BrightObjectFilterSettings = new ObjectFilterArguments();

        //public bool ApplyFilterForDarkObjectAcrossBoundary = false;
        //public ObjectFilterArguments DarkObjectAcrossBoundaryFilterSettings = new ObjectFilterArguments();

        //public bool ApplyFilterForBrightObjectAcrossBoundary = false;
        //public ObjectFilterArguments BrightObjectAcrossBoundaryFilterSettings = new ObjectFilterArguments();

        private MDCCParamFilter _MDCCParamFilter = new MDCCParamFilter();

        public AdObjectFilterSettings()
        {
        }

        public override void Copy(RasterCommandSettings settings)
        {
            if (settings is AdObjectFilterSettings == false)
                throw new ArgumentException(
                    "Settings is not an instance of AdObjectFilterSettings", "settings");

            base.Copy(settings);

            AdObjectFilterSettings cmdSettings = settings as AdObjectFilterSettings;

            _MDCCParamFilter = cmdSettings._MDCCParamFilter;
            cmdSettings._MDCCParamFilter.CopyTo(_MDCCParamFilter);

            //ApplyFilterForDarkObject =
            //    cmdSettings.ApplyFilterForDarkObject;

            //cmdSettings.DarkObjectFilterSettings.CopyTo(DarkObjectFilterSettings);

            //ApplyFilterForBrightObject =
            //    cmdSettings.ApplyFilterForBrightObject;

            //cmdSettings.BrightObjectFilterSettings.CopyTo(BrightObjectFilterSettings);

            //ApplyFilterForDarkObjectAcrossBoundary =
            //    cmdSettings.ApplyFilterForDarkObjectAcrossBoundary;

            //cmdSettings.DarkObjectAcrossBoundaryFilterSettings.CopyTo(
            //    DarkObjectAcrossBoundaryFilterSettings);

            //ApplyFilterForBrightObjectAcrossBoundary =
            //    cmdSettings.ApplyFilterForBrightObjectAcrossBoundary;

            //cmdSettings.BrightObjectAcrossBoundaryFilterSettings.CopyTo(
            //    BrightObjectAcrossBoundaryFilterSettings);

        }

        public MDCCParamFilter ParamFilter
        {
            get
            {
                return _MDCCParamFilter;
            }

            set
            {
                _MDCCParamFilter = value;
            }
        }

        public override void Validate()
        {
        }

        public static AdObjectFilterSettings Deserialize(string filePath)
        {
            try
            {
                AdObjectFilterSettings filterSettings =
                    RasterCommandSettingsSerializer.Deserialize(
                        filePath, typeof(AdObjectFilterSettings)) as AdObjectFilterSettings;

                return filterSettings;                
            }
            catch
            {
                return null;
            }
        }
    }
}
