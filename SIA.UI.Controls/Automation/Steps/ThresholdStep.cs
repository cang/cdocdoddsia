using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.Windows.Forms;

using SIA.Common.Analysis;
using SIA.Common.Imaging;

using SIA.UI.Controls.Automation.Commands;
using SIA.UI.Controls.Automation.Utilities;
using SIA.UI.Controls.Automation.Dialogs;
using SIA.UI.Controls.Utilities;
using SIA.Plugins.Common;

namespace SIA.UI.Controls.Automation.Steps
{
	/// <summary>
	/// Summary description for ThresholdStep.
	/// </summary>
	public class ThresholdStep : ProcessStep3
	{
        private const string id = "78F943E1-4E39-40a2-9A42-3AA55AA1DFAA";
		private const string displayName = "Threshold";
		private const string description = "Threshold";

		#region Constructors and Destructors

		public ThresholdStep()
			: base(id, displayName, description, ProcessStepCategories.Preprocessing,
			typeof(ThresholdCommand), typeof(ThresholdCommandSettings),
			ValidationKeys.SingleImage, null)
		{			
		}

		#endregion Constructors and Destructors

		#region Public Properties

		[XmlIgnore]
		public ThresholdCommandSettings ThresholdCommandSettings
		{
			get
			{
				return (ThresholdCommandSettings)_settings;
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
          	
			using (ThresholdCommand command = new ThresholdCommand(null))
				command.AutomationRun(new object[]{workingSpace.Image, this._settings});				
		}

		#endregion Overriden methods		
	}
}
