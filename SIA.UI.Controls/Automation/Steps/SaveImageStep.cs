using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.Windows.Forms;

using SIA.Common;
using SIA.Common.Analysis;
using SIA.Common.Imaging;

using SIA.UI.Controls;
using SIA.UI.Controls.Automation.Commands;
using SIA.UI.Controls.Automation.Utilities;
using SIA.UI.Controls.Automation.Dialogs;
using SIA.UI.Controls.Utilities;
using System.Diagnostics;

namespace SIA.UI.Controls.Automation.Steps
{
	/// <summary>
	/// Summary description for SaveImageStep.
	/// </summary>
	public class SaveImageStep : ProcessStep3
	{
		private const string displayName = "Save Image to File";
		private const string description = "Save Image to File";

		#region Constructors and Destructors
		public SaveImageStep()
            : base(Guid.NewGuid().ToString(), displayName, description, ProcessStepCategories.Output,
			typeof(SaveImageCommand), typeof(SaveImageCommandSettings),
			ValidationKeys.SingleImage, null)
		{			
		}

		#endregion Constructors and Destructors

		#region Public Member
		[XmlElement(typeof(SaveImageCommandSettings))]
		public SaveImageCommandSettings SaveImageCommandSettings
		{
			get
			{
				return (SaveImageCommandSettings)_settings;
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

            // retrieve step settings
			SaveImageCommandSettings settings = this.SaveImageCommandSettings;
			
            // create output file name
			settings.FileName = StringParser.Parse(settings.FileNameFormat, workingSpace);
			
            // execute command
			using (SaveImageCommand command = new SaveImageCommand(null))
				command.AutomationRun(new object[]{workingSpace.Image, _settings});
		}

		public override bool ShowSettingsDialog(System.Windows.Forms.IWin32Window owner)
		{
			SaveImageCommandSettings settings = this.SaveImageCommandSettings;
			if (settings == null)
				settings = new SaveImageCommandSettings();

			IFileType[] fileTypes = FileTypes.ImageFileTypes;
			int selFileType = -1;
			for (int i=0; i<fileTypes.Length; i++)
			{
				ImageFileType fileType = fileTypes[i] as ImageFileType;
				if (fileType.ImageFormat == settings.Format)
				{
					selFileType = i;
					break;
				}
			}
			
			using (DlgFileNameFormat2 dlg = new DlgFileNameFormat2(settings.FileNameFormat, fileTypes, selFileType))
			{
				if (DialogResult.OK == dlg.ShowDialog(owner))
				{
					settings.FileNameFormat = dlg.FileNameFormat;
					int selIndex = dlg.SelectedFileTypeIndex;
					if (selIndex >= 0 && selIndex < fileTypes.Length)
					{
						// update settings
						ImageFileType fileType = fileTypes[selIndex] as ImageFileType;
						settings.Format = fileType.ImageFormat;

						// save settings to step
						this._settings = settings;
					}

					return true;
				}
			}

			return false;
		}


		#endregion Overriden methods		
	}
}
