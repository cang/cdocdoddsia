using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SIA.UI.Controls.Automation.Commands
{
    [Serializable]
    public class MetrologySystemReferenceSettings : AutoCommandSettings
    {
        public string FilePath = "";

        public override void Validate()
        {
            if (FilePath == "" || !File.Exists(FilePath))
            {
                throw new ArgumentException("File path is invalid!", "MetrologySystemReferenceSettings");
            }
        }

        public override void Copy(SIA.UI.Controls.Commands.RasterCommandSettings settings)
        {
            if (settings is MetrologySystemReferenceSettings == false)
                throw new ArgumentException("Settings is not an instance of MetrologySystemReferenceSettings", "settings");

            base.Copy(settings);

            MetrologySystemReferenceSettings cmdSettings = (MetrologySystemReferenceSettings)settings;

            FilePath = cmdSettings.FilePath;
        }
    }
}
