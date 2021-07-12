using System;
using System.Xml;
using System.Xml.Serialization;

using SIA.Common.Analysis;
using SIA.UI.Controls.Automation.Commands;
using SIA.SystemLayer;


namespace SIA.UI.Controls.Automation.Steps
{
	/// <summary>
	/// Summary description for FilterRankStep.
	/// </summary>
	public class FilterRankStep : ProcessStep3
	{
        private const string id = "56EBEA6B-52AE-43d1-A446-606F0F728CD2";
		private const string displayName = "Rank Filter";
		private const string description = "Apply rank filter operation";

		#region Constructor and destructor

		public FilterRankStep()
            : base(id, displayName, description, ProcessStepCategories.Preprocessing,
			typeof(FilterRankCommand), typeof(FilterRankCommandSettings),
			ValidationKeys.SingleImage, null)
		{			
		}
		
		#endregion

		#region Properties
		
		[XmlIgnore]
		public FilterRankCommandSettings SelfSettings
		{
			get
			{
				return (FilterRankCommandSettings)_settings;
			}
			set
			{
				_settings = value;
			}
		}

		#endregion

		public override void Execute(WorkingSpace workingSpace)
		{
			if (workingSpace == null)
				throw new ArgumentNullException("workingSpace is not initialized");
			if (workingSpace.Image == null)
				throw new ArgumentNullException("Image was not loaded");
			if (_settings == null)
				throw new System.ArgumentException("Settings is null", "settings");

			using (FilterRankCommand detectWaferboundCommand = new FilterRankCommand(null))
			{
				detectWaferboundCommand.AutomationRun(new object[]{workingSpace.Image, _settings});
			}
		}

	}
}
