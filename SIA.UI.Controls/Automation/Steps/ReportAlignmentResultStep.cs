using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using SIA.UI.Controls.Commands;
using SIA.UI.Controls.Automation.Dialogs;
using System.Windows.Forms;
using SIA.Algorithms;
using SIA.UI.Controls.Commands.Analysis;


namespace SIA.UI.Controls.Automation.Steps
{
    [Serializable]
    public class ReportAlignmentResultSettings : AutoCommandSettings
    {
        #region member fields        
        public String FileNameFormat = "";
		#endregion member fields

		#region Constructors ans Deconstructors
		public ReportAlignmentResultSettings()
		{			
		}

		public ReportAlignmentResultSettings(string fileNameFormat)
		{
            FileNameFormat = fileNameFormat;
		}
		#endregion Constructors ans Deconstructors

		#region Properties
		#endregion Properties

		#region Methods
		
		public override void Copy(RasterCommandSettings settings)
		{
            if (settings is ReportAlignmentResultSettings == false)
                throw new ArgumentException("Settings is not an instance of ReportAlignmentResultSettings", "settings");

			base.Copy(settings);

            ReportAlignmentResultSettings cmdSettings = (ReportAlignmentResultSettings)settings;
            this.FileNameFormat = (String)cmdSettings.FileNameFormat.Clone();
		}

		public override void Validate()
		{			
		}		
	
		#endregion Methods

		#region Serializable && Deserialize
		#endregion Serializable && Deserialize		
    }

    public class ReportAlignmentResultCommand : AutoCommand
    {
        public override void AutomationRun(params object[] args)
        {
            
        }

        protected override void ValidateArguments(params object[] args)
        {
            
        }

        protected override void OnRun(params object[] args)
        {
            
        }
    }

    public class ReportAlignmentResultStep : ProcessStep3
    {
        private const string displayName = "Report Alignment Result";
        private const string description = "Report Alignment Result";

		#region Constructors and Destructors

        public ReportAlignmentResultStep()
			: base(Guid.NewGuid().ToString(), displayName, description, ProcessStepCategories.Output,
            typeof(ReportAlignmentResultCommand), typeof(ReportAlignmentResultSettings), 
			ValidationKeys.SingleImage, null)
		{
            _enabled = false;
		}

		#endregion Constructors and Destructors

        [XmlElement(typeof(ReportAlignmentResultSettings))]
        public ReportAlignmentResultSettings ReportAlignmentResultSettings
        {
            get
            {
                return (ReportAlignmentResultSettings)_settings;
            }
            set
            {
                _settings = value;
            }
        }

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
            ReportAlignmentResultSettings settings = this.ReportAlignmentResultSettings;
            string outputFilePath = StringParser.Parse(settings.FileNameFormat, workingSpace);

            string imageFile = workingSpace.ProcessingFileName;

            try
            {
                SiGlaz.Common.PathHelper.CreateMissingFolderAuto(outputFilePath);
                DefectExporter.SaveAlignmentReportAsCSV(
                    workingSpace.DetectedSystem, 
                    workingSpace.AlignmentResult, outputFilePath, imageFile);
            }
            catch
            {
                
            }
        }

        public override bool ShowSettingsDialog(System.Windows.Forms.IWin32Window owner)
        {
            ReportAlignmentResultSettings settings = this.ReportAlignmentResultSettings;
            if (settings == null)
                settings = new ReportAlignmentResultSettings();
            else
            {
                ReportAlignmentResultSettings newSettings = new ReportAlignmentResultSettings();
                newSettings.Copy(settings);
                settings = newSettings;
            }

            using (DlgFileNameFormat dlg = new DlgFileNameFormat(
                DlgFileNameFormat.CSV_FILE, settings.FileNameFormat))
            {
                if (dlg.ShowDialog(owner) == DialogResult.OK)
                {
                    if (_settings == null)
                        _settings = new ReportAlignmentResultSettings();
                    (_settings as ReportAlignmentResultSettings).FileNameFormat = dlg.Data;

                    return true;
                }
            }

            return false;
        }
    }
}
