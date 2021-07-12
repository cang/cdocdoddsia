using System;
using System.Xml.Serialization;
using System.IO;

using SIA.UI.Controls.Automation.Commands;

namespace SIA.UI.Controls.Automation.Steps
{
	/// <summary>
	/// Summary description for GaussianFilterStep.
	/// </summary>
	public class GaussianFilterStep : ProcessStep3
	{
		private const string displayName = "Gaussian Filter";
		private const string description = "Gaussian Filter";

		#region Constructors and Destructors

		public GaussianFilterStep()
            : base(Guid.NewGuid().ToString(), displayName, description, ProcessStepCategories.Preprocessing,
			typeof(GaussianFilterCommand), null,
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

			using (GaussianFilterCommand command = new GaussianFilterCommand(null))
				command.AutomationRun(new object[]{workingSpace.Image});
		}

		#endregion
	}
}
