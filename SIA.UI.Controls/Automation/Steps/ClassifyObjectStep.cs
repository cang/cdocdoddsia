using System;
using System.Xml.Serialization;
using System.Collections;

using SIA.Common.Analysis;
using SIA.UI.Controls.Automation.Commands;
using SIA.SystemLayer;
using SIA.Algorithms.FeatureProcessing;
using SIA.UI.Controls.Dialogs;

namespace SIA.UI.Controls.Automation.Steps
{
    public class ClassifyObjectStep : ProcessStep3
    {
        private const string displayName = "Classify Objects";
        private const string description = "Objects Classification.";

		#region Constructors and Destructors

        public ClassifyObjectStep()
            : base(Guid.NewGuid().ToString(), displayName, description, ProcessStepCategories.Analysis,
            typeof(ClassifyObjectCommand), typeof(ObjectClassificationSettings),
			ValidationKeys.SingleImage, null)
		{
            _enabled = false;
		}

		#endregion

		#region Properties			
        [XmlElement(typeof(ObjectClassificationSettings))]
        public ObjectClassificationSettings ObjectClassificationSettings
        {
            get
            {
                return (ObjectClassificationSettings)_settings;
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

            if (workingSpace.DetectedObjects == null)
                workingSpace.DetectedObjects = new DetectedObjectCollection();

            ObjectClassificationSettings settings = this._settings as ObjectClassificationSettings;
            FeatureSpace featureSpace = workingSpace.FeatureSpace;
            using (ClassifyObjectCommand command = new ClassifyObjectCommand(null))
            {
                command.AutomationRun(new object[] { workingSpace.Image, featureSpace, settings});
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

                if (featureSpace == null && output != null && output.Length >= 2)
                {
                    featureSpace = 
                        (output[1] == null ? (new FeatureSpace()) : (output[0] as FeatureSpace));
                }
            }

            workingSpace["DETECTEDOBJECTS"] = workingSpace.DetectedObjects;
		}

        public override bool ShowSettingsDialog(System.Windows.Forms.IWin32Window owner)
        {
            ObjectClassificationSettings settings = this.ObjectClassificationSettings;
            if (settings == null)
                settings = new ObjectClassificationSettings();
            using (DlgObjectClassification dlg = new DlgObjectClassification(settings))
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
