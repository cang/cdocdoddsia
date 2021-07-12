using System;
using System.Xml.Serialization;
using System.Collections;

using SIA.Common;
using SIA.Common.Analysis;

using SIA.Plugins.Common;

using SIA.UI.Controls.Automation.Commands;
using SIA.UI.Controls.Automation.Dialogs;
using System.Windows.Forms;
using SIA.UI.Controls.Dialogs;
using System.IO;
using System.Text;

namespace SIA.UI.Controls.Automation.Steps
{
    /// <summary>
    /// Summary description for ExtGlobalBckgStep.
    /// </summary>
    public class ExtGlobalBckgStep : ProcessStep3
	{
		private const string displayName = "Extract Global Background";
		private const string description = "Extract Global Background";

		#region Constructors and Destructors

        public ExtGlobalBckgStep()
            : base(Guid.NewGuid().ToString(), displayName, description, ProcessStepCategories.Preprocessing,
			typeof(ExtGlobalBckgCommand), typeof(ExtGlobalBckgSettings),
			ValidationKeys.SingleImage, null)
		{			
		}

		#endregion Constructors and Destructors

		#region Public Member

		[XmlIgnore]
        public ExtGlobalBckgSettings ExtGlobalBckgSettings
		{
			get
			{
                return (ExtGlobalBckgSettings)_settings;
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

            using (ExtGlobalBckgCommand command = new ExtGlobalBckgCommand(null))
			{
				command.AutomationRun(new object[]{workingSpace.Image, _settings});				
			}
		}

		#endregion Overriden methods		
	}
}
