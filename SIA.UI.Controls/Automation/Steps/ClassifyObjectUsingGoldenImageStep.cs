using System;
using System.Collections.Generic;
using System.Text;
using SIA.UI.Controls.Automation.Commands;
using SIA.UI.Controls.Commands;
using System.Xml.Serialization;
using SIA.SystemLayer.ImageProcessing;
using System.Collections;
using SIA.Common.Analysis;
using SIA.UI.Controls.Dialogs;
using SIA.Algorithms.ReferenceFile;

namespace SIA.UI.Controls.Automation.Steps
{
    public class ClassifyObjectUsingGoldenImageStep : ProcessStep3
    {
        //private const string displayName = "Classify Primitive Object(s) using Golden Image";
        //private const string description = "Classify Primitive Object(s) using Golden Image";
        private const string displayName = "Detect Anomalies";
        private const string description = "Detect Anomalies";

        #region Constructors and Destructors

        public ClassifyObjectUsingGoldenImageStep()
            : base(Guid.NewGuid().ToString(), displayName, description, ProcessStepCategories.Analysis,
            typeof(DetectObjectUsingGoldenImageCommand), typeof(DetectObjectUsingGoldenImageSettings),
            ValidationKeys.SingleImage, ValidationKeys.DetectedObjects)
        {
            _enabled = false;
        }

        #endregion

        #region Public Member
        [XmlElement(typeof(DetectObjectUsingGoldenImageSettings))]
        public DetectObjectUsingGoldenImageSettings DetectObjectUsingGoldenImageSettings
        {
            get
            {
                return (DetectObjectUsingGoldenImageSettings)_settings;
            }
            set
            {
                _settings = value;
            }
        }

        #endregion

        #region Overriden methods

        public override void Execute(WorkingSpace workingSpace)
        {
            if (workingSpace == null)
                throw new ArgumentNullException("workingSpace is not initialized");
            if (workingSpace.Image == null)
                throw new ArgumentNullException("Image was not loaded");
            if (_settings == null)
                throw new System.ArgumentException("Settings is null", "settings");

            if (workingSpace.DetectedObjects == null)
                workingSpace.DetectedObjects = new DetectedObjectCollection();

            string goldenImageFilePath =
                this.DetectObjectUsingGoldenImageSettings.GoldenImageFilePath;
            eGoldenImageMethod method = eGoldenImageMethod.None;
            double darkThreshold = 92.5;
            double brightThreshold = 150;

            using (
                DetectObjectUsingGoldenImageCommand command = 
                    new DetectObjectUsingGoldenImageCommand(null))
            {
                command.AutomationRun(
                    new object[]{
                        workingSpace.Image, 
                        goldenImageFilePath, method, darkThreshold, brightThreshold});
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

                    workingSpace.DetectedObjects.AddRange(output[0] as ArrayList);
                }
            }

            workingSpace["DETECTEDOBJECTS"] = workingSpace.DetectedObjects;
        }

        public override bool ShowSettingsDialog(System.Windows.Forms.IWin32Window owner)
        {
            DetectObjectUsingGoldenImageSettings 
                settings = this.DetectObjectUsingGoldenImageSettings;
            if (settings == null)
                settings = new DetectObjectUsingGoldenImageSettings();

            using (DlgFileBrowser dlg = new DlgFileBrowser())
            {
                dlg.FilePath = settings.GoldenImageFilePath;
                if (dlg.ShowDialog(owner) == System.Windows.Forms.DialogResult.OK)
                {
                    settings.GoldenImageFilePath = dlg.FilePath;
                    this._settings = settings;
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
