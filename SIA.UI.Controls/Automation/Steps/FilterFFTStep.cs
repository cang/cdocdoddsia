using System;
using System.Xml.Serialization;
using System.Collections;

using SIA.Common.Analysis;
using SIA.UI.Controls.Automation.Commands;

namespace SIA.UI.Controls.Automation.Steps
{
	/// <summary>
	/// Summary description for FilterFFTStep.
	/// </summary>
	public class FilterFFTStep : ProcessStep3
	{
		private const string displayName = "FFT (Fast Fourier Transform) Filter";
		private const string description = "Fourier transform filter operation";

		#region Constructors and Destructors

		public FilterFFTStep()
            : base(Guid.NewGuid().ToString(), displayName, description, ProcessStepCategories.Preprocessing,
			typeof(FilterFFTCommand), typeof(FilterFFTCommandSettings),
			ValidationKeys.SingleImage, null)
		{			
		}

		#endregion Constructors and Destructors

		#region Public Member

		[XmlIgnore]
		public FilterFFTCommandSettings SelfSettings
		{
			get
			{
				return (FilterFFTCommandSettings)_settings;
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

			using (FilterFFTCommand command = new FilterFFTCommand(null))
			{
				command.AutomationRun(new object[]{workingSpace.Image, _settings});				
			}
		}
		#endregion Overriden methods		
	}
}
