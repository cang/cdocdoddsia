#define SIA_PRODUCT

using System;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;

using SIA.Common.Analysis;
using SIA.SystemLayer;
using SIA.Workbench.Common;

using SIA.UI.Controls;
using SIA.UI.Controls.Automation.Commands;
using SIA.UI.Controls.Automation.Dialogs;
using SIA.UI.Controls.Utilities;
using SIA.Plugins.Common;


namespace SIA.UI.Controls.Automation.Steps 
{
	/// <summary>
	/// Summary description for LoadSingleImageStep.
	/// </summary>
	public class LoadSingleImageStep : ProcessStep3 
	{
		private const string displayName = "Load Single Image From File";
		private const string description = "Load Single Image From File";

        public bool KeepOrginalFileName = false;
        public string FileNameFormat = string.Empty;
        [XmlIgnore]
		public string FilePath = string.Empty;

		#region Constructors and Destructors

		public LoadSingleImageStep()
            : base(Guid.NewGuid().ToString(), displayName, description, ProcessStepCategories.Input,
			null, null, 
			null, ValidationKeys.SingleImage)
		{
			this._hasSettings = true;
			this._hasCustomSettingsDialog = true;



            _enabled = false;
		}

		#endregion

		#region Properties	
		
        #endregion

		#region Methods

		public override void Validate()
		{
		}

		public override void Validate(IAutomationWorkspace workspace)
		{
			base.Validate(workspace);
		}

		public override void Execute(WorkingSpace workingSpace) 
		{
			if (workingSpace == null)
				throw new ArgumentNullException("workingSpace is not initialized");
			
			using (LoadImageCommand command = new LoadImageCommand(null)) 
			{
				// initialize command settings
				LoadImageCommandSettings settings = new LoadImageCommandSettings();

                // parse the file name from working space
                settings.FileName = StringParser.Parse(this.FileNameFormat, workingSpace);

				// execute command
				command.AutomationRun(new object[]{settings});				

				// save the result image - old version compatible
                workingSpace.Image = command.Image;

                if (!KeepOrginalFileName)
				    workingSpace.ProcessingFileName = settings.FileName;

				// new version - save the loaded image into storage bag
				workingSpace["IMAGE"] = command.Image;
			}
		}

		public override bool ShowSettingsDialog(System.Windows.Forms.IWin32Window owner) 
		{
            //using (OpenFileDialog dlg = CommonDialogs.OpenImageFileDialog("Select an image file.."))
            //{
            //    // do not need to verify file is exist
            //    dlg.CheckFileExists = false;

            //    if (this._filePath != null && this._filePath != string.Empty)
            //        dlg.FileName = this._filePath;

            //    if (DialogResult.OK == dlg.ShowDialog(owner))
            //    {
            //        this._filePath = dlg.FileName;
            //        return true;
            //    }

            //    return false;
            //}

            using (DlgLoadSingleImageStepSettings dlg = new DlgLoadSingleImageStepSettings())
            {
                dlg.KeepOrginalFileName = this.KeepOrginalFileName;
                dlg.FileNameFormat = this.FileNameFormat;

                if (dlg.ShowDialog(owner) == DialogResult.OK)
                {
                    this.KeepOrginalFileName = dlg.KeepOrginalFileName;
                    this.FileNameFormat = dlg.FileNameFormat;
                    return true;
                }
            }

            return false;
		}

		#endregion		
	}
}
