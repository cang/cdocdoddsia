using System;
using System.Xml.Serialization;
using System.Collections;

using SIA.Common;
using SIA.Common.Analysis;

using SIA.Plugins.Common;

using SIA.UI.Controls.Automation.Commands;
using SIA.UI.Controls.Automation.Dialogs;
using System.Windows.Forms;
using SIA.UI.Controls.Dialogs;
using System.IO;
using System.Text;

namespace SIA.UI.Controls.Automation.Steps
{
    public class ExportDefectListStep : ProcessStep3
	{
		private const string displayName = "Export Anomaly List";
        private const string description = "Export Anomaly List";

		#region Constructors and Destructors

        public ExportDefectListStep()
			: base(Guid.NewGuid().ToString(), displayName, description, ProcessStepCategories.Output,
            typeof(ExportDefectListCommand), typeof(ExportDefectListSettings), 
			ValidationKeys.SingleImage, null)
		{
            _enabled = true;
		}

		#endregion Constructors and Destructors

		#region Public Member

        [XmlElement(typeof(ExportDefectListSettings))]
        public ExportDefectListSettings ExportDefectListSettings
        {
            get
            {
                return (ExportDefectListSettings)_settings;
            }
            set
            {
                _settings = value;
            }
        }

		#endregion Public Member

		#region Overriden methods
		
		public override void Execute(WorkingSpace workingSpace)
		{
			if (workingSpace == null)
				throw new ArgumentNullException("workingSpace is not initialized");

            if (workingSpace.Image == null)
                throw new ArgumentNullException("Image was not loaded");
            if (_settings == null)
                throw new System.ArgumentException("Settings is empty", "settings");

            if (workingSpace.AlignmentResult == null ||
                workingSpace.DetectedSystem == null)
                throw new System.Exception("Cannot detect metrology system.");

            // create output file name
            ExportDefectListSettings settings = this.ExportDefectListSettings;
            string outputFilePath = StringParser.Parse(settings.FileNameFormat, workingSpace);

            using (ExportDefectListCommand command = new ExportDefectListCommand(null))
			{
                ArrayList results = workingSpace.DetectedObjects;
				command.AutomationRun(
                    new object[] {
                        outputFilePath, 
                        workingSpace.ProcessingFileName, 
                        results, workingSpace.DetectedSystem,
                        (settings.KeepName ? "" : settings.CustomizedName)});
            }
		}        
        
        public override bool ShowSettingsDialog(System.Windows.Forms.IWin32Window owner)
        {
            ExportDefectListSettings settings = this.ExportDefectListSettings;
            if (settings == null)
                settings = new ExportDefectListSettings();
            else
            {
                ExportDefectListSettings newSettings = new ExportDefectListSettings();
                newSettings.Copy(settings);
                settings = newSettings;
            }

            using (DlgExportDefectListSettings dlg = new DlgExportDefectListSettings(settings))
            {
                if (dlg.ShowDialog(owner) == DialogResult.OK)
                {
                    _settings = dlg.Settings;

                    return true;
                }
            }

            return false;
        }
		#endregion Overriden methods			
	}
}
