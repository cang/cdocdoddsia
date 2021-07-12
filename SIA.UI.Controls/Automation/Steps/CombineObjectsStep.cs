using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using SIA.UI.Controls.Commands.Analysis;
using SIA.UI.Controls.Automation.CommandSettings;
using System.Collections;
using SIA.Common.Analysis;
using SIA.UI.Controls.Dialogs.Analysis;
using SiGlaz.Common.Object;

namespace SIA.UI.Controls.Automation.Steps
{
    public class CombineObjectsStep : ProcessStep3
    {
        private const string displayName = "Combine Objects";
		private const string description = "Combine Objects.";

		#region Constructors and Destructors

        public CombineObjectsStep()
            : base(Guid.NewGuid().ToString(), displayName, description, ProcessStepCategories.Analysis,
            typeof(CombineObjectsCommand), typeof(CombineObjectsCommandSettings), 
            ValidationKeys.SingleImage, null)
		{
		}
		#endregion

		#region Properties
        [XmlElement(typeof(CombineObjectsCommandSettings))]
        public CombineObjectsCommandSettings CombineObjectCommandSettings
        {
            get
            {
                return (CombineObjectsCommandSettings)_settings;
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
            {
                workingSpace.DetectedObjects = new DetectedObjectCollection();
                return;
            }

            ArrayList objList = new ArrayList(workingSpace.DetectedObjects.Count);
            objList.AddRange(workingSpace.DetectedObjects);

            CombineObjectsCommandSettings settings = this.CombineObjectCommandSettings;
            using (CombineObjectsCommand command = new CombineObjectsCommand(null))
            {
                command.AutomationRun(
                        new object[]{
                        objList,
                        settings.SelectedConditions,
                        workingSpace.DetectedSystem});

                object[] output = command.GetOutput();
                if (output == null || output.Length == 0 ||
                    output[0] == null || output[0].GetType() != typeof(ArrayList) || (output[0] as ArrayList).Count == 0)
                {
                    workingSpace.DetectedObjects.Clear();
                }
                else
                {
                    if ((output[0] as ArrayList)[0].GetType() != typeof(DetectedObject) && 
                        (output[0] as ArrayList)[0].GetType() != typeof(DetectedObjectEx))
                        throw new System.ExecutionEngineException("The output element is not an instance of DetectedObject type");

                    ArrayList result = output[0] as ArrayList;
                    workingSpace.DetectedObjects.Clear();
                    workingSpace.DetectedObjects.AddRange(result);
                }
            }

            workingSpace["DETECTEDOBJECTS"] = workingSpace.DetectedObjects;

		}

        public override bool ShowSettingsDialog(System.Windows.Forms.IWin32Window owner)
        {
            CombineObjectsCommandSettings settings = this.CombineObjectCommandSettings;
            if (settings == null)
                settings = new CombineObjectsCommandSettings();
            else
            {
                CombineObjectsCommandSettings newSettings = new CombineObjectsCommandSettings();
                newSettings.Copy(settings);
                settings = newSettings;
            }

            using (DlgCombineObjects dlg = new DlgCombineObjects(settings))
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
