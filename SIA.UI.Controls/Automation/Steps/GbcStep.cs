using System;
using System.Xml.Serialization;
using System.Collections;

using SIA.Common.Analysis;
using SIA.UI.Controls.Automation.Commands;

namespace SIA.UI.Controls.Automation.Steps
{
	/// <summary>
	/// Summary description for GbcStep.
	/// </summary>
	public class GbcStep : ProcessStep3
	{
		private const string displayName = "Global Background Correction";
		private const string description = "Global Background Correction";

		#region Constructors and Destructors

		public GbcStep()
            : base(Guid.NewGuid().ToString(), displayName, description, ProcessStepCategories.Preprocessing,
			typeof(GbcCommand), typeof(GbcCommandSettings),
			ValidationKeys.SingleImage, null)
		{			
		}

		#endregion Constructors and Destructors

		#region Public Member

		[XmlIgnore]
		public GbcCommandSettings GbcCommandSettings
		{
			get
			{
				return (GbcCommandSettings)_settings;
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
          
			using (GbcCommand command = new GbcCommand(null))
			{
				command.AutomationRun(new object[]{workingSpace.Image, _settings});				
			}
		}

		#endregion Overriden methods		
	}
}
