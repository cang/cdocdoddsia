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
using System.Drawing.Imaging;
using System.Drawing;
using SiGlaz.Common;

namespace SIA.UI.Controls.Automation.Steps
{
    public class OverlayDefectOnABSStep : ProcessStep3
	{
        private const string displayName = "Export overlaid anomalies on image to File";
        private const string description = "Export overlaid anomalies on image to File";

		#region Constructors and Destructors
        public OverlayDefectOnABSStep()
            : base(Guid.NewGuid().ToString(), displayName, description, ProcessStepCategories.Output,
            typeof(OverlayDefectOnABSCommand), typeof(SaveImageCommandSettings),
			ValidationKeys.SingleImage, null)
		{
            _enabled = true;
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

            // execute command
            using (OverlayDefectOnABSCommand command = new OverlayDefectOnABSCommand(null))
            {
                ArrayList defectList = workingSpace.DetectedObjects;
                if (defectList == null)
                    defectList = new ArrayList();

                command.AutomationRun(new object[] { workingSpace.Image, defectList });
                object[] output = command.GetOutput();
                if (output != null && output.Length > 0 && output[0] != null)
                {
                    try
                    {
                        // retrieve step settings
                        SaveImageCommandSettings settings = this.SaveImageCommandSettings;

                        // create output file name
                        settings.FileName = StringParser.Parse(settings.FileNameFormat, workingSpace);
                        PathHelper.CreateMissingFolderAuto(settings.FileName);

                        ImageFormat format = ImageFormat.Bmp;
                        switch (settings.Format)
                        {
                            case eImageFormat.Gif:
                                format = ImageFormat.Gif;
                                break;
                            case eImageFormat.Jpeg:
                                format = ImageFormat.Jpeg;
                                break;
                            case eImageFormat.Png:
                                format = ImageFormat.Png;
                                break;
                            case eImageFormat.Tiff:
                                format = ImageFormat.Tiff;
                                break;
                            default:
                                break;
                        }

                        (output[0] as Image).Save(settings.FileName, format);
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        (output[0] as Image).Dispose();
                        output[0] = null;
                    }
                }
            }
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
