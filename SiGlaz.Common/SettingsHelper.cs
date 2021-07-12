using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace SiGlaz.Common
{
    public class SettingsHelper
    {
        public const string ABSInspectionSettingsFolderName = "ABS_Inspection_Settings";
        public const string PoleTipInspectionSettingsFolderName = "PoleTip_Inspection_Settings";

        public static string GetDefaultABSInspectionSettingsFolderPath()
        {
            string appPath = Path.GetDirectoryName(Application.ExecutablePath);

            string path = Path.Combine(appPath, ABSInspectionSettingsFolderName);
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
            catch
            {
                // nothing here
            }

            return path;
        }

        public static string GetDefaultPoleTipInspectionSettingsFolderPath()
        {
            string appPath = Path.GetDirectoryName(Application.ExecutablePath);

            string path = Path.Combine(appPath, PoleTipInspectionSettingsFolderName);
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
            catch
            {
                // nothing here
            }

            return path;
        }
    }
}
