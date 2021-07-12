using System;
using System.Xml.Serialization;
using System.Collections;

using SIA.Common.Analysis;
using SIA.UI.Controls.Automation.Commands;
using SIA.SystemLayer;
using SIA.IPEngine;
using System.IO;
using SIA.UI.Controls.Dialogs;

namespace SIA.UI.Controls.Automation.Steps
{
    public class AlignABSStep : ProcessStep3
    {
        private const string displayName = "Align ABS";
		private const string description = "Performing alignment Air Bearing Surface.";

		#region Constructors and Destructors

        public AlignABSStep()
            : base(Guid.NewGuid().ToString(), displayName, description, ProcessStepCategories.Preprocessing,
            typeof(AlignABSCommand), typeof(AlignABSCommandSettings),
			ValidationKeys.SingleImage, null)
		{
            _enabled = false;
		}

		#endregion

		#region Properties			
        [XmlElement(typeof(AlignABSCommandSettings))]
        public AlignABSCommandSettings AlignABSCommandSettings
        {
            get
            {
                return (AlignABSCommandSettings)_settings;
            }
            set
            {
                _settings = value;
            }
        }
		#endregion

		#region Methods		

		public override void Execute(WorkingSpace workingSpace)
		{
			if (workingSpace == null)
				throw new ArgumentNullException("workingSpace is not initialized");
			if (workingSpace.Image == null)
				throw new ArgumentNullException("Image was not loaded");
            if (_settings == null)
                throw new System.ArgumentException("Settings is null", "settings");

            SiGlaz.Common.ImageAlignment.Settings settings = null;

            if (this.AlignABSCommandSettings.UseDefault)
                settings =
                    SiGlaz.Common.ImageAlignment.Settings.Deserialize(Path.Combine(
                    SiGlaz.Common.SettingsHelper.GetDefaultABSInspectionSettingsFolderPath(),
                    SiGlaz.Common.ImageAlignment.Settings.DefaultFileName_ABS));
            else
                settings =
                    SiGlaz.Common.ImageAlignment.Settings.Deserialize(
                    this.AlignABSCommandSettings.FilePath);

            CommonImage alignedImage = null;
            using (AlignABSCommand command = new AlignABSCommand(null))
            {
                command.AutomationRun(new object[] { workingSpace.Image, settings });
                object[] output = command.GetOutput();
                GreyDataImage alignedImageRaster = output[0] as GreyDataImage;
                alignedImage = new CommonImage(alignedImageRaster);
            }
            CommonImage oldImage = workingSpace.Image;
            workingSpace.Image = alignedImage;

            // dispose old image
            if (oldImage != null)
            {
                oldImage.Dispose();
                oldImage = null;
            }
		}

        public override bool ShowSettingsDialog(System.Windows.Forms.IWin32Window owner)
        {
            AlignABSCommandSettings settings = this.AlignABSCommandSettings;
            if (settings == null)
                settings = new AlignABSCommandSettings();
            else
            {
                AlignABSCommandSettings newSettings = new AlignABSCommandSettings();
                newSettings.Copy(settings);
                settings = newSettings;
            }
            using (DlgABSAlignment dlg = new DlgABSAlignment(settings))
            {
                if (dlg.ShowDialog(owner) == System.Windows.Forms.DialogResult.OK)
                {
                    this._settings = dlg.Settings;

                    return true;
                }
            }
            return false;
        }
		#endregion		
    }
}
