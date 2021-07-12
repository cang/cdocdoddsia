using System;
using System.Xml.Serialization;
using System.IO;

using SIA.UI.Controls.Automation.Commands;

namespace SIA.UI.Controls.Automation.Steps
{
	/// <summary>
	/// Summary description for SmoothFilterStep.
	/// </summary>
	public class SmoothFilterStep : ProcessStep3
	{
		private const string displayName = "Smooth Filter";
		private const string description = "smooth filtering";

		#region Constructors and Destructors

		public SmoothFilterStep()
            : base(Guid.NewGuid().ToString(), displayName, description, ProcessStepCategories.Preprocessing,
			typeof(SmoothFilterCommand), null,
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

			using (SmoothFilterCommand command = new SmoothFilterCommand(null))
				command.AutomationRun(new object[]{workingSpace.Image});

		}

		#endregion
	}
}
