using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using SIA.UI.Controls.Commands;
using SiGlaz.Common.ImageAlignment;
using SIA.Algorithms.ReferenceFile;
using System.Collections;
using SIA.Common.Analysis;
using SIA.UI.Controls.Automation.Commands;
using SIA.UI.Controls.Dialogs;
using SIA.Algorithms.Regions;

namespace SIA.UI.Controls.Automation.Steps
{
    public class DetectAnomaliesStep : ProcessStep3
    {
        private const string displayName = "Detect Anomalies";
		private const string description = "Anomalies Detection.";

		#region Constructors and Destructors

        public DetectAnomaliesStep()
            : base(Guid.NewGuid().ToString(), displayName, description, ProcessStepCategories.Analysis,
            typeof(DetectAnomaliesCommand), typeof(DetectAnomaliesSettings), 
            ValidationKeys.SingleImage, null)
		{
		}
		#endregion

		#region Properties
        [XmlElement(typeof(DetectAnomaliesSettings))]
        public DetectAnomaliesSettings DetectAnomaliesSettings
        {
            get
            {
                return (DetectAnomaliesSettings)_settings;
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

            if (workingSpace.AlignmentResult == null || 
                workingSpace.DetectedSystem == null)
                throw new System.Exception("Cannot detect metrology system.");

            if (workingSpace.DetectedObjects == null)
                workingSpace.DetectedObjects = new DetectedObjectCollection();

            MetrologySystemReference refFile = workingSpace.RefFile;            
            eGoldenImageMethod method = AnomalyDetectorDefinition.Method;
            double darkThreshold = AnomalyDetectorDefinition.ABSDarkThreshold;
            double brightThreshold = AnomalyDetectorDefinition.ABSBrightThreshold;

            DetectAnomaliesSettings settings = this.DetectAnomaliesSettings;
            if (settings.ApplyRegions && refFile.Regions == null)
                return;
                
            if (settings.ApplyRegions && workingSpace.DetectedRegions == null)
            {
                workingSpace.DetectedRegions =
                    ABSRegionProcessor.CorrectRegions(
                        refFile,
                        workingSpace.DetectedSystem,
                        workingSpace.AlignmentResult,
                        workingSpace.Image.RasterImage);
                if (workingSpace.DetectedRegions != null)
                    workingSpace.DetectedRegions.MetroSys = workingSpace.DetectedSystem;
            }

            using (DetectAnomaliesCommand command = new DetectAnomaliesCommand(null))
            {
                if (settings.ApplyRegions)
                {
                    command.AutomationRun(
                        new object[]{
                        workingSpace.Image, 
                        refFile, method, darkThreshold, brightThreshold, 
                        workingSpace.AlignmentResult, workingSpace.DetectedRegions});
                }
                else
                {
                    command.AutomationRun(
                        new object[]{
                        workingSpace.Image, 
                        refFile, method, darkThreshold, brightThreshold, 
                        workingSpace.AlignmentResult});
                }

                object[] output = command.GetOutput();
                if (output == null || output.Length == 0 ||
                    output[0] == null || output[0].GetType() != typeof(ArrayList) || (output[0] as ArrayList).Count == 0)
                {
                    //workingSpace.DetectedObjects = new DetectedObjectCollection();
                }
                else
                {
                    if ((output[0] as ArrayList)[0].GetType() != typeof(DetectedObject))
                        throw new System.ExecutionEngineException("The output element is not an instance of DetectedObject type");

                    ArrayList result = output[0] as ArrayList;
                    workingSpace.DetectedSystem.CurrentUnit.UpdateObjectInfo(result);

                    workingSpace.DetectedObjects.AddRange(result);
                }
            }

            workingSpace["DETECTEDOBJECTS"] = workingSpace.DetectedObjects;

		}

        public override bool ShowSettingsDialog(System.Windows.Forms.IWin32Window owner)
        {
            DetectAnomaliesSettings settings = this.DetectAnomaliesSettings;
            if (settings == null)
                settings = new DetectAnomaliesSettings();
            else
            {
                DetectAnomaliesSettings newSettings = new DetectAnomaliesSettings();
                newSettings.Copy(settings);
                settings = newSettings;
            }

            using (DlgDetectAnomaliesSettings dlg =
                new DlgDetectAnomaliesSettings(settings))
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
