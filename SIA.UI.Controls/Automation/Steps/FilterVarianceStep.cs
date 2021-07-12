using System;
using System.Xml.Serialization;
using System.Collections;

using SIA.Common.Analysis;
using SIA.UI.Controls.Automation.Commands;

namespace SIA.UI.Controls.Automation.Steps
{
	/// <summary>
	/// Summary description for FilterVarianceStep.
	/// </summary>
	public class FilterVarianceStep : ProcessStep3
	{
		private const string displayName = "Variance Filter";
		private const string description = "variance filter";

		#region Constructors and Destructors

		public FilterVarianceStep()
            : base(Guid.NewGuid().ToString(), displayName, description, ProcessStepCategories.Preprocessing,
			typeof(FilterVarianceCommand), typeof(FilterVarianceCommandSettings),
			ValidationKeys.SingleImage, null)
		{			
			this._enabled = false;
		}

		#endregion Constructors and Destructors

		#region Public Member

		[XmlIgnore]
		public FilterVarianceCommandSettings FilterVarianceCommandSettings
		{
			get
			{
				return (FilterVarianceCommandSettings)_settings;
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

			using (FilterVarianceCommand command = new FilterVarianceCommand(null))
			{
				command.AutomationRun(new object[]{workingSpace.Image, _settings});				
			}
		}
		#endregion Overriden methods		
	}
}
