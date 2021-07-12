using System;
using System.Xml.Serialization;
using System.Collections;

using SIA.Common.Analysis;
using SIA.UI.Controls.Automation.Commands;

namespace SIA.UI.Controls.Automation.Steps
{
	/// <summary>
	/// Summary description for KernelFilterStep.
	/// </summary>
	public class KernelFilterStep : ProcessStep3
	{
		private const string displayName = "Kernel Filter";
		private const string description = "Kernel Filter";

		#region Constructors and Destructors
		public KernelFilterStep()
            : base(Guid.NewGuid().ToString(), displayName, description, ProcessStepCategories.Preprocessing,
			typeof(KernelFilterCommand), typeof(KernelFilterCommandSettings),
			ValidationKeys.SingleImage, null)
		{			
		}

		#endregion Constructors and Destructors

		#region Public Member

		[XmlIgnore]
		public KernelFilterCommandSettings KernelFilterCommandSettings
		{
			get
			{
				return (KernelFilterCommandSettings)_settings;
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
          	
			using (KernelFilterCommand command = new KernelFilterCommand(null))
			{
				command.AutomationRun(new object[]{workingSpace.Image, _settings});				
			}
		}

		#endregion Overriden methods			
	}
}
