using System;
using System.Collections.Generic;
using System.Text;
using SIA.UI.Controls.Commands;
using System.Xml.Serialization;
using SIA.UI.Controls.Automation.Commands;
using SIA.UI.Controls.Dialogs;
using SIA.SystemLayer;
using SiGlaz.Common.ImageAlignment;
using SIA.Algorithms.Preprocessing.Alignment;
using SIA.Algorithms.Regions;
using System.IO;

namespace SIA.UI.Controls.Automation.Steps
{
    public class DetectCoordinateStep : ProcessStep3
    {
        private const string displayName = "Detect Coordinate System";
		private const string description = "Performing Coordinate System Detection.";

		#region Constructors and Destructors

        public DetectCoordinateStep()
            : base(Guid.NewGuid().ToString(), displayName, description, ProcessStepCategories.Analysis,
            typeof(DetectCoordinateSystemCommand), typeof(MetrologySystemReferenceSettings),
			ValidationKeys.SingleImage, null)
		{            
		}

		#endregion

		#region Properties
        [XmlElement(typeof(MetrologySystemReferenceSettings))]
        public MetrologySystemReferenceSettings MetrologySystemReferenceSettings
        {
            get
            {
                return (MetrologySystemReferenceSettings)_settings;
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
                throw new System.ArgumentException("Settings is empty", "settings");

            MetrologySystemReference refFile =
                MetrologySystemReference.Deserialize(this.MetrologySystemReferenceSettings.FilePath);
            workingSpace.RefFile = refFile;

            using (DetectCoordinateSystemCommand command = new DetectCoordinateSystemCommand(null))
            {
                command.AutomationRun(new object[] { workingSpace.Image, refFile });
                object[] output = command.GetOutput();

                workingSpace.DetectedSystem = output[0] as MetrologySystem;
                workingSpace.AlignmentResult = output[1] as AlignmentResult;
                //workingSpace.DetectedRegions =
                //    ABSRegionProcessor.CorrectRegions(
                //        refFile, 
                //        workingSpace.DetectedSystem, 
                //        workingSpace.AlignmentResult, 
                //        workingSpace.Image.RasterImage);
                //workingSpace.DetectedRegions.MetroSys = workingSpace.DetectedSystem;
            }
		}

        public override bool ShowSettingsDialog(System.Windows.Forms.IWin32Window owner)
        {
            MetrologySystemReferenceSettings settings = this.MetrologySystemReferenceSettings;
            if (settings == null)
                settings = new MetrologySystemReferenceSettings();
            else
            {
                MetrologySystemReferenceSettings newSettings = new MetrologySystemReferenceSettings();
                newSettings.Copy(settings);
                settings = newSettings;
            }

            using (DlgMetrologySystemReferenceBrowser dlg = new DlgMetrologySystemReferenceBrowser(settings))
            {
                if (dlg.ShowDialog(owner) == System.Windows.Forms.DialogResult.OK)
                {
                    this._settings = dlg.Settings;

                    if (File.Exists(this.MetrologySystemReferenceSettings.FilePath))
                    {
                        MetrologySystemReference refFile = 
                            MetrologySystemReference.Deserialize(
                            this.MetrologySystemReferenceSettings.FilePath);
                        if (refFile != null && refFile.Regions == null)
                        {
                            System.Windows.Forms.MessageBox.Show(
                                owner, 
                                string.Format("The Reference File: {0} has no region.", this.MetrologySystemReferenceSettings.FilePath),
                                "Warning", 
                                System.Windows.Forms.MessageBoxButtons.OK, 
                                System.Windows.Forms.MessageBoxIcon.Warning);
                        }
                    }

                    return true;
                }
            }
            return false;
        }
		#endregion
    }
}
