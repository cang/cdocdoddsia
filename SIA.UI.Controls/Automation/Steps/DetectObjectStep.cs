#define SIA_PRODUCT

using System;
using System.Xml.Serialization;
using System.Collections;

using SIA.Common;
using SIA.Common.Analysis;

using SIA.Plugins.Common;

using SIA.UI.Controls.Automation.Commands;
using System.Windows.Forms;
using SIA.UI.Controls.Dialogs;

namespace SIA.UI.Controls.Automation.Steps
{
	/// <summary>
	/// Summary description for DetectObjectStep.
	/// </summary>
	public class DetectObjectStep : ProcessStep3
	{
		private const string displayName = "Detect Objects";
		private const string description = "detecting objects";

		#region Constructors and Destructors

		public DetectObjectStep()
            : base(Guid.NewGuid().ToString(), displayName, description, ProcessStepCategories.Analysis,
            typeof(DetectObjectCommand), typeof(DetectObjectCommandSettings), 
            ValidationKeys.SingleImage, ValidationKeys.DetectedObjects)
		{
#if SIA_PRODUCT
            _enabled = false;
#endif
		}

		#endregion

		#region Public Member
		
		[XmlIgnore]
		public DetectObjectCommandSettings DetectObjectCommandSettings
		{
			get
			{
				return (DetectObjectCommandSettings)_settings;
			}
			set
			{
				_settings = value;
			}
		}

		#endregion

		#region Overriden methods

		public override void Execute(WorkingSpace workingSpace)
		{
			if (workingSpace == null)
				throw new ArgumentNullException("workingSpace is not initialized");
			if (workingSpace.Image == null)
				throw new ArgumentNullException("Image was not loaded");
			if (_settings == null)
				throw new System.ArgumentException("Settings is null", "settings");

			using (DetectObjectCommand command = new DetectObjectCommand(null))
			{
				command.AutomationRun(new object[]{workingSpace.Image, _settings});
				object[] output = command.GetOutput();
				if (output == null || output.Length == 0 ||
					output[0] == null || output[0].GetType() != typeof(ArrayList) || (output[0] as ArrayList).Count == 0)
				{
					workingSpace.DetectedObjects = new DetectedObjectCollection();
				}
				else
				{
					if ((output[0] as ArrayList)[0].GetType() != typeof(DetectedObject))
						throw new System.ExecutionEngineException("The output element is not an instance of DetectedObject type");

					workingSpace.DetectedObjects = new SIA.Common.Analysis.DetectedObjectCollection((output[0] as ArrayList).Count);
					workingSpace.DetectedObjects.AddRange(output[0] as ArrayList);
					workingSpace["DETECTEDOBJECTS"] = workingSpace.DetectedObjects;
				}
			}
		}

		#endregion
	}
}
