using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SIA.UI.Controls.Automation.Commands
{
    [Serializable]
    public class AlignABSCommandSettings : AutoCommandSettings
    {
        public bool UseDefault = true;
        public string FilePath = "";

        public override void Validate()
        {
            if (!UseDefault)
            {
                if (FilePath == "" ||
                    !File.Exists(FilePath))
                {
                    throw new ArgumentException("File path is invalid!", "AlignABSCommandSettings");
                }
            }
        }

        public override void Copy(SIA.UI.Controls.Commands.RasterCommandSettings settings)
        {
            if (settings is AlignABSCommandSettings == false)
                throw new ArgumentException("Settings is not an instance of AlignABSCommandSettings", "settings");

            base.Copy(settings);

            AlignABSCommandSettings cmdSettings = (AlignABSCommandSettings)settings;

            UseDefault = cmdSettings.UseDefault;
            FilePath = cmdSettings.FilePath;
        }
    }
}
