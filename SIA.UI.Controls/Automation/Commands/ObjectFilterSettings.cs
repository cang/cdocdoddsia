using System;
using System.Collections.Generic;
using System.Text;
using SIA.Common.KlarfExport;
using SIA.UI.Controls.Commands;
using System.Windows.Forms;
using System.IO;
using SiGlaz.Common;

namespace SIA.UI.Controls.Automation.Commands
{
    public class ObjectFilterSettings : AutoCommandSettings
    {
        public static string SIADefaultFilePath
        {
            get
            {
                string userPath = Application.UserAppDataPath;
                string filePath = 
                    Path.Combine(userPath, 
                    string.Format("SIA\\Settings\\ObjectFilterSettings.xml"));
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
                    string.Format("SIAW\\Settings\\ObjectFilterSettings.xml"));
                PathHelper.CreateMissingFolderAuto(filePath);

                return filePath;
            }
        }


        public bool ApplyFilterForDarkObject = false;
        public ObjectFilterArguments DarkObjectFilterSettings = new ObjectFilterArguments();

        public bool ApplyFilterForBrightObject = false;
        public ObjectFilterArguments BrightObjectFilterSettings = new ObjectFilterArguments();

        public bool ApplyFilterForDarkObjectAcrossBoundary = false;
        public ObjectFilterArguments DarkObjectAcrossBoundaryFilterSettings = new ObjectFilterArguments();

        public bool ApplyFilterForBrightObjectAcrossBoundary = false;
        public ObjectFilterArguments BrightObjectAcrossBoundaryFilterSettings = new ObjectFilterArguments();

        public ObjectFilterSettings()
        {
        }

        public override void Copy(RasterCommandSettings settings)
        {
            if (settings is ObjectFilterSettings == false)
                throw new ArgumentException(
                    "Settings is not an instance of ObjectFilterSettings", "settings");

            base.Copy(settings);

            ObjectFilterSettings cmdSettings = settings as ObjectFilterSettings;

            ApplyFilterForDarkObject = 
                cmdSettings.ApplyFilterForDarkObject;

            cmdSettings.DarkObjectFilterSettings.CopyTo(DarkObjectFilterSettings);

            ApplyFilterForBrightObject = 
                cmdSettings.ApplyFilterForBrightObject;

            cmdSettings.BrightObjectFilterSettings.CopyTo(BrightObjectFilterSettings);

            ApplyFilterForDarkObjectAcrossBoundary = 
                cmdSettings.ApplyFilterForDarkObjectAcrossBoundary;

            cmdSettings.DarkObjectAcrossBoundaryFilterSettings.CopyTo(
                DarkObjectAcrossBoundaryFilterSettings);

            ApplyFilterForBrightObjectAcrossBoundary = 
                cmdSettings.ApplyFilterForBrightObjectAcrossBoundary;

            cmdSettings.BrightObjectAcrossBoundaryFilterSettings.CopyTo(
                BrightObjectAcrossBoundaryFilterSettings);
            
        }

        public override void Validate()
        {
        }

        public static ObjectFilterSettings Deserialize(string filePath)
        {
            try
            {
                ObjectFilterSettings filterSettings =
                    RasterCommandSettingsSerializer.Deserialize(
                        filePath, typeof(ObjectFilterSettings)) as ObjectFilterSettings;

                return filterSettings;
            }
            catch
            {
                return null;
            }
        }
    }
}
