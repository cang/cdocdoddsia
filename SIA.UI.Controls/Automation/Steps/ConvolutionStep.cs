using System;
using System.Xml.Serialization;
using System.Collections;

using SIA.Common.Analysis;
using SIA.UI.Controls.Automation.Commands;
using SIA.UI.Controls.Commands;

namespace SIA.UI.Controls.Automation.Steps
{
	/// <summary>
	/// Summary description for ConvolutionStep.
	/// </summary>
	public class ConvolutionStep : ProcessStep3
	{
		private const string displayName = "Convolution";
		private const string description = "performing convolution operation.";

		#region Constructors and Destructors

		public ConvolutionStep()
            : base(Guid.NewGuid().ToString(), displayName, description, ProcessStepCategories.Preprocessing,
			typeof(ConvolutionCommand), typeof(ConvolutionCommandSettings),
			ValidationKeys.SingleImage, null)
		{
			this._enabled = false;
		}

		#endregion Constructors and Destructors

		#region Public Member
		
		[XmlIgnore]
		public ConvolutionCommandSettings ConvolutionCommandSettings
		{
			get
			{
				return (ConvolutionCommandSettings)_settings;
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
          
			using (ConvolutionCommand command = new ConvolutionCommand(null))
			{
				command.AutomationRun(new object[]{workingSpace.Image, _settings});				
			}
		}

		#endregion Overriden methods
	}
}
