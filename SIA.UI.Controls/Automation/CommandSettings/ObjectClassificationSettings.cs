using System;
using System.Collections.Generic;
using System.Text;
using SIA.UI.Controls.Commands;
using System.IO;
using System.Windows.Forms;
using SiGlaz.Common;

namespace SIA.UI.Controls.Automation.Commands
{
    public class ObjectClassificationSettings : AutoCommandSettings
    {
        public static string SIADefaultFilePath
        {
            get
            {
                string userPath = Application.UserAppDataPath;
                string filePath =
                    Path.Combine(userPath,
                    string.Format("SIA\\Settings\\ObjectClassificationSettings.xml"));
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
                    string.Format("SIAW\\Settings\\ObjectClassificationSettings.xml"));
                PathHelper.CreateMissingFolderAuto(filePath);

                return filePath;
            }
        }

        public bool ClassifyDarkObject = true;
        public bool ClassifyBrightObject = true;
        public bool ClassifyDarkObjectAcrossBoundary = true;
        public bool ClassifyBrightObjectAcrossBoundary = true;

        public ObjectClassificationSettings()
        {
        }

        public override void Copy(RasterCommandSettings settings)
        {
            if (settings is ObjectClassificationSettings == false)
                throw new ArgumentException(
                    "Settings is not an instance of ObjectClassificationSettings", "settings");

            base.Copy(settings);

            ObjectClassificationSettings cmdSettings = settings as ObjectClassificationSettings;

            ClassifyDarkObject = cmdSettings.ClassifyDarkObject;
            ClassifyBrightObject = cmdSettings.ClassifyBrightObject;;
            ClassifyDarkObjectAcrossBoundary = cmdSettings.ClassifyDarkObjectAcrossBoundary;
            ClassifyBrightObjectAcrossBoundary = cmdSettings.ClassifyBrightObjectAcrossBoundary;
        }

        public override void Validate()
        {
        }

        public static ObjectClassificationSettings Deserialize(string filePath)
        {
            try
            {
                ObjectClassificationSettings filterSettings =
                    RasterCommandSettingsSerializer.Deserialize(
                        filePath, 
                        typeof(ObjectClassificationSettings)) as ObjectClassificationSettings;

                return filterSettings;
            }
            catch
            {
                return null;
            }
        }
    }
}
