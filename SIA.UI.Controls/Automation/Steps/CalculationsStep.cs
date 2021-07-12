using System;
using System.Xml.Serialization;
using System.Collections;

using SIA.Common.Analysis;
using SIA.UI.Controls.Automation.Commands;
using SIA.UI.Controls.Commands;

namespace SIA.UI.Controls.Automation.Steps
{
	/// <summary>
	/// Implements the calculation step
	/// </summary>
	public class CalculationsStep : ProcessStep3
	{
		private const string displayName = "Calculation";
		private const string description = "Performing calculation opertion.";

		#region Constructors and Destructors

		public CalculationsStep()
			: base(Guid.NewGuid().ToString(), displayName, description, ProcessStepCategories.Preprocessing,
			typeof(CalculationCommand), typeof(CalculationCommandSettings), 
			ValidationKeys.SingleImage, null)
		{	
		}

		#endregion Constructors and Destructors

		#region Public Member
		
		[XmlIgnore]
		public CalculationCommandSettings CalculationCommandSettings
		{
			get
			{
				return (CalculationCommandSettings)_settings;
			}
			set
			{
				_settings = value;
			}
		}

		#endregion Public Member

		#region Overriden methods
		
		public override void Execute(WorkingSpace workingSpace)
		{
			if (workingSpace == null)
				throw new ArgumentNullException("workingSpace is not initialized");
			if (workingSpace.Image == null)
				throw new ArgumentNullException("Image was not loaded");

			if (_settings == null)
				throw new System.ArgumentException("Settings is null", "settings");
          
			using (CalculationCommand command = new CalculationCommand(null))
			{
				command.AutomationRun(new object[]{workingSpace.Image, _settings});				
			}
		}

		#endregion Overriden methods			
	}
}
