using System;
using System.Xml.Serialization;
using System.IO;

using SIA.UI.Controls.Automation.Commands;

namespace SIA.UI.Controls.Automation.Steps
{
	/// <summary>
	/// Summary description for InvertStep.
	/// </summary>
	public class InvertStep : ProcessStep3
	{
		private const string displayName = "Invert image";
		private const string description = "Invert image";

		#region Constructors and Destructors

		public InvertStep()
            : base(Guid.NewGuid().ToString(), displayName, description, ProcessStepCategories.Preprocessing,
			typeof(InvertCommand), null,
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

			using (InvertCommand command = new InvertCommand(null))
			{
				command.AutomationRun(new object[]{workingSpace.Image});
			}
		}


		#endregion
	}
}
