using System;
using System.Xml.Serialization;
using System.Collections;

using SIA.Common.Analysis;
using SIA.UI.Controls.Automation.Commands;
using SIA.SystemLayer;
using SIA.Algorithms.FeatureProcessing;

namespace SIA.UI.Controls.Automation.Steps
{
    public class ExtractFeatureStep : ProcessStep3
    {
        private const string displayName = "Extract Features";
		private const string description = "Feature Extraction.";

		#region Constructors and Destructors

        public ExtractFeatureStep()
            : base(Guid.NewGuid().ToString(), displayName, description, ProcessStepCategories.Analysis,
            typeof(ExtractFeatureCommand), null,
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

            workingSpace.FeatureSpace = null;
            using (ExtractFeatureCommand command = new ExtractFeatureCommand(null))
            {
                command.AutomationRun(new object[] { workingSpace.Image });
                object[] output = command.GetOutput();
                if (output == null || output.Length == 0 ||
                    output[0] == null || output[0].GetType() != typeof(FeatureSpace))
                {
                    workingSpace.FeatureSpace = new FeatureSpace();
                }
                else
                {
                    workingSpace.FeatureSpace = output[0] as FeatureSpace;
                }
            }
		}
		#endregion		
    }
}
