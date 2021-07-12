using System;
using System.Xml.Serialization;
using System.Collections;

using SIA.Common.Analysis;
using SIA.UI.Controls.Automation.Commands;
using SIA.SystemLayer;
using SIA.Algorithms.FeatureProcessing;
using SiGlaz.Common;
using System.IO;

namespace SIA.UI.Controls.Automation.Steps
{
    public class DetectOverPatternStep : ProcessStep3
    {
        private const string displayName = "Classify Over Pattern";
        private const string description = "Over Pattern Classification.";

		#region Constructors and Destructors

        public DetectOverPatternStep()
            : base(Guid.NewGuid().ToString(), displayName, description, ProcessStepCategories.Analysis,
            typeof(DetectLinePatternCommand), null,
			ValidationKeys.SingleImage, null)
		{
            _enabled = false;
		}

		#endregion

		#region Properties			
		
		#endregion

		#region Methods		

		public override void Execute(WorkingSpace workingSpace)
		{
			if (workingSpace == null)
				throw new ArgumentNullException("workingSpace is not initialized");
			if (workingSpace.Image == null)
				throw new ArgumentNullException("Image was not loaded");

            if (workingSpace.DetectedObjects == null)
                workingSpace.DetectedObjects = new DetectedObjectCollection();

            using (DetectLinePatternCommand command = new DetectLinePatternCommand(null))
            {
                foreach (string filename in LinePatternLibrary.MultiplePatternFilenames)
                {
                    LinePatternLibrary settings = LinePatternLibrary.Deserialize(
                        Path.Combine(SettingsHelper.GetDefaultABSInspectionSettingsFolderPath(), filename));

                    command.AutomationRun(new object[] { workingSpace.Image, settings });
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
            }

            workingSpace["DETECTEDOBJECTS"] = workingSpace.DetectedObjects;
		}
		#endregion
    }
}
