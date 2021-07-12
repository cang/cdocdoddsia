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
	/// Summary description for DlgLaunchExternalApplicationSettings.
	/// </summary>
	public class DlgLaunchExternalApplicationSettings : DialogBase
	{
		#region Windows Form Members

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnBrowseFile;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtFilePath;
		private System.Windows.Forms.TextBox txtArguments;
		private System.Windows.Forms.TextBox txtWorkingDir;
		private System.Windows.Forms.CheckBox chkWaitForExit;
		private System.Windows.Forms.Button btnBrowseWorkingDir;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#endregion

		#region Constructor and Destructor
		
		public DlgLaunchExternalApplicationSettings()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();			
		}

		public DlgLaunchExternalApplicationSettings(LaunchExternalApplicationCommandSettings settings)
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

		private LaunchExternalApplicationCommandSettings _settings = new LaunchExternalApplicationCommandSettings();

		#endregion

		#region Properties

		public LaunchExternalApplicationCommandSettings Settings
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgLaunchExternalApplicationSettings));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.txtFilePath = new System.Windows.Forms.TextBox();
			this.btnBrowseFile = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtArguments = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtWorkingDir = new System.Windows.Forms.TextBox();
			this.btnBrowseWorkingDir = new System.Windows.Forms.Button();
			this.chkWaitForExit = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(-5, 200);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(572, 4);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(200, 212);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 7;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(288, 212);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 8;
			this.btnCancel.Text = "Cancel";
			// 
			// txtFilePath
			// 
			this.txtFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFilePath.Location = new System.Drawing.Point(8, 32);
			this.txtFilePath.Name = "txtFilePath";
			this.txtFilePath.Size = new System.Drawing.Size(520, 20);
			this.txtFilePath.TabIndex = 1;
			this.txtFilePath.Text = "";
			// 
			// btnBrowseFile
			// 
			this.btnBrowseFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseFile.Location = new System.Drawing.Point(531, 32);
			this.btnBrowseFile.Name = "btnBrowseFile";
			this.btnBrowseFile.Size = new System.Drawing.Size(25, 20);
			this.btnBrowseFile.TabIndex = 2;
			this.btnBrowseFile.Text = "...";
			this.btnBrowseFile.Click += new System.EventHandler(this.btnBrowseFile_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(408, 23);
			this.label1.TabIndex = 9;
			this.label1.Text = "Application file path:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 60);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(408, 23);
			this.label2.TabIndex = 9;
			this.label2.Text = "Arguments:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtArguments
			// 
			this.txtArguments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtArguments.Location = new System.Drawing.Point(8, 88);
			this.txtArguments.Name = "txtArguments";
			this.txtArguments.Size = new System.Drawing.Size(520, 20);
			this.txtArguments.TabIndex = 1;
			this.txtArguments.Text = "";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 116);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(408, 23);
			this.label3.TabIndex = 9;
			this.label3.Text = "Working Directory:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtWorkingDir
			// 
			this.txtWorkingDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtWorkingDir.Location = new System.Drawing.Point(8, 144);
			this.txtWorkingDir.Name = "txtWorkingDir";
			this.txtWorkingDir.Size = new System.Drawing.Size(520, 20);
			this.txtWorkingDir.TabIndex = 1;
			this.txtWorkingDir.Text = "";
			// 
			// btnBrowseWorkingDir
			// 
			this.btnBrowseWorkingDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseWorkingDir.Location = new System.Drawing.Point(532, 144);
			this.btnBrowseWorkingDir.Name = "btnBrowseWorkingDir";
			this.btnBrowseWorkingDir.Size = new System.Drawing.Size(25, 20);
			this.btnBrowseWorkingDir.TabIndex = 2;
			this.btnBrowseWorkingDir.Text = "...";
			this.btnBrowseWorkingDir.Click += new System.EventHandler(this.btnBrowseWorkingDir_Click);
			// 
			// chkWaitForExit
			// 
			this.chkWaitForExit.Location = new System.Drawing.Point(8, 172);
			this.chkWaitForExit.Name = "chkWaitForExit";
			this.chkWaitForExit.Size = new System.Drawing.Size(552, 24);
			this.chkWaitForExit.TabIndex = 10;
			this.chkWaitForExit.Text = "Wait for exit";
			// 
			// DlgLaunchExternalApplicationSettings
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(562, 240);
			this.Controls.Add(this.chkWaitForExit);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtFilePath);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnBrowseFile);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtArguments);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtWorkingDir);
			this.Controls.Add(this.btnBrowseWorkingDir);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DlgLaunchExternalApplicationSettings";
			this.Text = "Launch External Application Settings";
			this.ResumeLayout(false);

		}
		#endregion

		#region Methods
		#endregion

		#region Event Handlers
		
		private void btnBrowseFile_Click(object sender, System.EventArgs e)
		{
			string fileName = txtFilePath.Text;
			string initDir = "";
			if (File.Exists(fileName))
				initDir = Path.GetDirectoryName(txtFilePath.Text);
			
			using (OpenFileDialog dlg = CommonDialogs.OpenFileDialog("Application", 
					   "Select an application file path", "Executable Files (*.exe)|*.exe|All Files (*.*)|*.*", 0, 
					   initDir, fileName)) 
			{
				try
				{
					if (DialogResult.OK == dlg.ShowDialog(this))
						txtFilePath.Text = dlg.FileName;
				}
				catch (System.Exception exp)
				{
					 MessageBoxEx.Error("Failed to browse for external application file path: " + exp.Message);
				}
			}
		}

		private void btnBrowseWorkingDir_Click(object sender, System.EventArgs e)
		{
			using (FolderBrowserDialog dlg = CommonDialogs.SelectFolderDialog("Please select a working folder"))
			{
				try
				{
					dlg.SelectedPath = txtWorkingDir.Text;
					if (DialogResult.OK == dlg.ShowDialog(this))
						txtWorkingDir.Text = dlg.SelectedPath;
				}
				catch (System.Exception exp)
				{
					MessageBoxEx.Error("Failed to browse for working directory: " + exp.Message);
				}
			}
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
			string filePath = txtFilePath.Text;
			if (filePath == string.Empty)
			{
				MessageBoxEx.Error("External application file path was not specified.");
				return false;
			}

			if (File.Exists(filePath) == false)
			{
				MessageBoxEx.Error("Invalid application file path or not exists.");
				return false;
			}

			return true;
		}

		protected virtual void UpdateData(bool bSaveAndValidate)
		{
			if (bSaveAndValidate)
			{
				this._settings.FileName = txtFilePath.Text;
				this._settings.Arguments = txtArguments.Text;
				this._settings.WorkingDirectory = txtWorkingDir.Text;
				this._settings.WaitForExit = chkWaitForExit.Checked;
			}
			else
			{				
				txtFilePath.Text = this._settings.FileName;
				txtArguments.Text = this._settings.Arguments;
				txtWorkingDir.Text = this._settings.WorkingDirectory;
				chkWaitForExit.Checked = this._settings.WaitForExit;
			}
		}

		#endregion				

		
	}
}
