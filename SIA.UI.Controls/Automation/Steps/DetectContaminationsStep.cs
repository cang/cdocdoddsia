using System;
using System.Xml.Serialization;
using System.Collections;

using SIA.Common.Analysis;
using SIA.UI.Controls.Automation.Commands;
using SIA.SystemLayer;
using SIA.Algorithms.FeatureProcessing;

namespace SIA.UI.Controls.Automation.Steps
{
    public class DetectContaminationsStep : ProcessStep3
    {
        private const string displayName = "Classify Contamination";
        private const string description = "Contaminations Classification.";

		#region Constructors and Destructors

        public DetectContaminationsStep()
            : base(Guid.NewGuid().ToString(), displayName, description, ProcessStepCategories.Analysis,
            typeof(DetectContaminationsCommand), null,
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

            FeatureSpace featureSpace = workingSpace.FeatureSpace;
            using (DetectContaminationsCommand command = new DetectContaminationsCommand(null))
            {
                command.AutomationRun(new object[] { workingSpace.Image, featureSpace});
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
		#endregion
    }
}
