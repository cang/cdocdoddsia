using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using SIA.UI.Controls;
using SIA.UI.Controls.Common;
using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.Utilities;

using SIA.UI.Controls.Automation;
using SIA.UI.Controls.Automation.Commands;

namespace SIA.UI.Controls.Automation.Dialogs
{
	/// <summary>
	/// Summary description for DlgLoadImageStepSettings.
	/// </summary>
	public class DlgLoadImageStepSettings : DialogBase
	{
		#region Windows Form Members

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.RadioButton radFileName;
		private System.Windows.Forms.RadioButton radFolder;
		private System.Windows.Forms.TextBox txtFileName;
		private System.Windows.Forms.Button btnBrowseFile;
		private System.Windows.Forms.Button btnBrowseFolder;
		private System.Windows.Forms.TextBox txtFilePath;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#endregion

		#region Constructor and Destructor
		
		public DlgLoadImageStepSettings()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();			
		}

		public DlgLoadImageStepSettings(LoadImageCommandSettings settings)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.Settings = settings;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#endregion

		#region Fields

		private LoadImageCommandSettings _settings = new LoadImageCommandSettings();

		#endregion

		#region Properties

		public LoadImageCommandSettings Settings
		{
			get {return _settings;}
			set {
				_settings = value;
				OnSettingsChanged();
			}
		}

		protected virtual void OnSettingsChanged()
		{
			this.UpdateData(false);
		}

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgLoadImageStepSettings));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.radFileName = new System.Windows.Forms.RadioButton();
			this.radFolder = new System.Windows.Forms.RadioButton();
			this.txtFileName = new System.Windows.Forms.TextBox();
			this.btnBrowseFile = new System.Windows.Forms.Button();
			this.btnBrowseFolder = new System.Windows.Forms.Button();
			this.txtFilePath = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.groupBox1.Location = new System.Drawing.Point(0, 108);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(384, 4);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(112, 116);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 7;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(200, 116);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 8;
			this.btnCancel.Text = "Cancel";
			// 
			// radFileName
			// 
			this.radFileName.Checked = true;
			this.radFileName.Location = new System.Drawing.Point(4, 8);
			this.radFileName.Name = "radFileName";
			this.radFileName.Size = new System.Drawing.Size(132, 20);
			this.radFileName.TabIndex = 0;
			this.radFileName.TabStop = true;
			this.radFileName.Text = "Load image from file";
			this.radFileName.Click += new System.EventHandler(this.radLoadImage_Click);
			// 
			// radFolder
			// 
			this.radFolder.Location = new System.Drawing.Point(4, 56);
			this.radFolder.Name = "radFolder";
			this.radFolder.Size = new System.Drawing.Size(164, 20);
			this.radFolder.TabIndex = 3;
			this.radFolder.Text = "Load all images in folder";
			this.radFolder.Click += new System.EventHandler(this.radFolder_Click);
			// 
			// txtFileName
			// 
			this.txtFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFileName.Location = new System.Drawing.Point(16, 32);
			this.txtFileName.Name = "txtFileName";
			this.txtFileName.Size = new System.Drawing.Size(332, 20);
			this.txtFileName.TabIndex = 1;
			this.txtFileName.Text = "";
			// 
			// btnBrowseFile
			// 
			this.btnBrowseFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseFile.Location = new System.Drawing.Point(355, 32);
			this.btnBrowseFile.Name = "btnBrowseFile";
			this.btnBrowseFile.Size = new System.Drawing.Size(24, 20);
			this.btnBrowseFile.TabIndex = 2;
			this.btnBrowseFile.Text = "...";
			this.btnBrowseFile.Click += new System.EventHandler(this.btnBrowseFile_Click);
			// 
			// btnBrowseFolder
			// 
			this.btnBrowseFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseFolder.Enabled = false;
			this.btnBrowseFolder.Location = new System.Drawing.Point(355, 80);
			this.btnBrowseFolder.Name = "btnBrowseFolder";
			this.btnBrowseFolder.Size = new System.Drawing.Size(24, 20);
			this.btnBrowseFolder.TabIndex = 5;
			this.btnBrowseFolder.Text = "...";
			this.btnBrowseFolder.Click += new System.EventHandler(this.btnBrowseFolder_Click);
			// 
			// txtFilePath
			// 
			this.txtFilePath.Location = new System.Drawing.Point(16, 80);
			this.txtFilePath.Name = "txtFilePath";
			this.txtFilePath.Size = new System.Drawing.Size(332, 20);
			this.txtFilePath.TabIndex = 9;
			this.txtFilePath.Text = "";
			// 
			// DlgLoadImageStepSettings
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(386, 144);
			this.Controls.Add(this.radFileName);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.radFolder);
			this.Controls.Add(this.txtFileName);
			this.Controls.Add(this.txtFilePath);
			this.Controls.Add(this.btnBrowseFile);
			this.Controls.Add(this.btnBrowseFolder);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DlgLoadImageStepSettings";
			this.Text = "Load Image Settings";
			this.ResumeLayout(false);

		}
		#endregion

		#region Methods
		#endregion

		#region Event Handlers
		
		private void btnBrowseFile_Click(object sender, System.EventArgs e)
		{
			using (OpenImageFile dlg = FileTypes.OpenImageFileDialog("Select an image...", txtFileName.Text))
			{
				try
				{
					if (DialogResult.OK == dlg.ShowDialog(this))
						txtFileName.Text = dlg.FileName;
				}
				catch (System.Exception exp)
				{
					 MessageBoxEx.Error("Failed to browse for image. " + exp.Message);
				}
			}
		}

		private void btnBrowseFolder_Click(object sender, System.EventArgs e)
		{
			using (FolderBrowserDialog dlg = CommonDialogs.SelectFolderDialog("Please select a folder"))
			{
				try
				{
					dlg.SelectedPath = txtFilePath.Text;
					if (DialogResult.OK == dlg.ShowDialog(this))
						txtFilePath.Text = dlg.SelectedPath;
				}
				catch (System.Exception exp)
				{
					MessageBoxEx.Error("Failed to browse for folder. " + exp.Message);
				}
			}
		}

		private void radLoadImage_Click(object sender, System.EventArgs e)
		{			
			radLoadImage_Checked();
		}

		private void radLoadImage_Checked()
		{
			this.radFileName.Checked = true;
			this.btnBrowseFile.Enabled = this.radFileName.Checked;
		
			this.radFolder.Checked = !this.radFileName.Checked;
			this.btnBrowseFolder.Enabled = this.radFolder.Checked;			
		}

		private void radFolder_Click(object sender, System.EventArgs e)
		{			
			radFolder_Checked();
		}
		
		private void radFolder_Checked()
		{
			this.radFolder.Checked = true;
			this.btnBrowseFolder.Enabled = this.radFolder.Checked;
			
			this.radFileName.Checked = !this.radFolder.Checked;
			this.btnBrowseFile.Enabled = this.radFileName.Checked;
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			
		}

		#endregion

		#region Override Routines

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			// refresh controls' data
			this.UpdateData(false);
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing (e);

			if (this.DialogResult == DialogResult.OK)
			{
				// validate data
				if (this.ValidateData() == false)
					e.Cancel = true;
				else // update data when valid
					this.UpdateData(true);
			}
		}

		
		#endregion

		#region Internal Helpers

		protected virtual bool ValidateData()
		{
			InputFileType fileType = InputFileType.File;
			if (radFolder.Checked)
				fileType = InputFileType.Path;
			
			if (fileType == InputFileType.File)
			{
				String fileName = txtFileName.Text;
				if (fileName == null || fileName.Length == 0)
				{
					MessageBoxEx.Error("Image file was not specified. Please try again.");
					return false;
				}

				if (File.Exists(fileName) == false)
				{
					MessageBoxEx.Error("Image file was not found. Please try again.");
					return false;
				}
			}
			else
			{
				String filePath = txtFilePath.Text;
				if (filePath == null || filePath.Length == 0)
				{
					MessageBoxEx.Error("Image folder was not specified. Please try again.");
					return false;
				}

				if (Directory.Exists(filePath) == false)
				{
					MessageBoxEx.Error("Image folder was not found. Please try again");
					return false;	
				}
			}

			return true;
		}

		protected virtual void UpdateData(bool bSaveAndValidate)
		{
			if (bSaveAndValidate)
			{
				if (radFileName.Checked)
					this._settings.InputFileType = InputFileType.File;
				else if (radFolder.Checked)
					this._settings.InputFileType = InputFileType.Path;
				this._settings.FileName = txtFileName.Text;
				this._settings.FilePath = txtFilePath.Text;
			}
			else
			{				
				radFileName.Checked = (this._settings.InputFileType == InputFileType.File);
				btnBrowseFile.Enabled = radFileName.Checked;
				radFolder.Checked = (this._settings.InputFileType == InputFileType.Path);
				btnBrowseFolder.Enabled = radFolder.Checked;
				txtFileName.Text = this._settings.FileName;
				txtFilePath.Text = this._settings.FilePath;
			}
		}

		#endregion				
	}
}
