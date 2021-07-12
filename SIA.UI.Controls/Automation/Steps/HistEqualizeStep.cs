using System;
using System.Xml.Serialization;
using System.IO;

using SIA.UI.Controls.Automation.Commands;

namespace SIA.UI.Controls.Automation.Steps
{
    public class HistEqualizeStep : ProcessStep3
    {
        private const string displayName = "Histogram Equalization";
        private const string description = "Histogram Equalization";

		#region Constructors and Destructors

        public HistEqualizeStep()
            : base(Guid.NewGuid().ToString(), displayName, description, ProcessStepCategories.Preprocessing,
            typeof(HistEqualizeCommand), null,
			ValidationKeys.SingleImage, null)
		{			
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

            using (HistEqualizeCommand command = new HistEqualizeCommand(null))
			{
				command.AutomationRun(new object[]{workingSpace.Image});
			}
		}


		#endregion
    }
}
