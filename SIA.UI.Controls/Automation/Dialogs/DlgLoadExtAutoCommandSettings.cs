using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using SIA.UI.Controls.Utilities;
using SIA.UI.Controls.Dialogs;

namespace SIA.UI.Controls.Automation.Dialogs
{
	/// <summary>
	/// Summary description for DlgLoadSettings.
	/// </summary>
	public class DlgLoadExtAutoCommandSettings : DialogBase
	{
		#region Windows Form Members
		private System.Windows.Forms.TextBox txtSettingsFile;
		private System.Windows.Forms.Label labelSettingsFile;
		private System.Windows.Forms.Button btnLoadSettings;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.GroupBox groupBox1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion

		#region Members

		private Type _stepSettingsType = null;
		private string _stepName = "";
		private string _filePath = "";

		#endregion

		#region Constructor and destructor

		protected DlgLoadExtAutoCommandSettings()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		public DlgLoadExtAutoCommandSettings(string filePath, string stepName, Type stepSettingsType)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			_filePath = filePath;
			_stepName = stepName;
			_stepSettingsType = stepSettingsType;
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgLoadExtAutoCommandSettings));
			this.labelSettingsFile = new System.Windows.Forms.Label();
			this.txtSettingsFile = new System.Windows.Forms.TextBox();
			this.btnLoadSettings = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.SuspendLayout();
			// 
			// labelSettingsFile
			// 
			this.labelSettingsFile.Location = new System.Drawing.Point(8, 12);
			this.labelSettingsFile.Name = "labelSettingsFile";
			this.labelSettingsFile.Size = new System.Drawing.Size(72, 16);
			this.labelSettingsFile.TabIndex = 0;
			this.labelSettingsFile.Text = "Settings File:";
			// 
			// txtSettingsFile
			// 
			this.txtSettingsFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtSettingsFile.Location = new System.Drawing.Point(84, 8);
			this.txtSettingsFile.Name = "txtSettingsFile";
			this.txtSettingsFile.Size = new System.Drawing.Size(298, 20);
			this.txtSettingsFile.TabIndex = 1;
			this.txtSettingsFile.Text = "";
			// 
			// btnLoadSettings
			// 
			this.btnLoadSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnLoadSettings.Location = new System.Drawing.Point(390, 8);
			this.btnLoadSettings.Name = "btnLoadSettings";
			this.btnLoadSettings.Size = new System.Drawing.Size(28, 20);
			this.btnLoadSettings.TabIndex = 2;
			this.btnLoadSettings.Text = "...";
			this.btnLoadSettings.Click += new System.EventHandler(this.btnLoadSettings_Click);
			// 
			// btnOK
			// 
			this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(133, 46);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 4;
			this.btnOK.Text = "OK";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(218, 46);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(0, 34);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(426, 4);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			// 
			// DlgLoadExtAutoCommandSettings
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(426, 76);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnLoadSettings);
			this.Controls.Add(this.txtSettingsFile);
			this.Controls.Add(this.labelSettingsFile);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DlgLoadExtAutoCommandSettings";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Load Settings";
			this.ResumeLayout(false);

		}
		#endregion

		#region Properties
		
		public String FilePath
		{
			get
			{
				return _filePath;
			}

			set {
				_filePath = value;
				OnFilePathChanged();
			}
		}

		private void OnFilePathChanged()
		{
			txtSettingsFile.Text = _filePath;
			this.Invalidate(true);
		}

		#endregion
		
		#region Event Handlers

		private void btnLoadSettings_Click(object sender, System.EventArgs e)
		{
			try
			{
				using (OpenFileDialog dlg = CommonDialogs.OpenProcessStepSettingsFileDialog("Load settings from file..."))
				{
					if (dlg.ShowDialog() == DialogResult.OK)
					{
						if (this.ValidateSettingsFile(dlg.FileName, _stepSettingsType, _stepName))
							this.txtSettingsFile.Text = dlg.FileName;					
					}
				}
			}
			catch (System.Exception exp)
			{
				throw exp;
			}
		}

		#endregion

		#region Override Routines

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			// refresh user interface items
			this.UpdateData(false);
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing (e);

			if (this.DialogResult == DialogResult.OK)
			{
				if (this.ValidateData() == false)
					e.Cancel = true;
				else // update when data is valid
					this.UpdateData(true);
			}
		}



		#endregion

		#region Static Methods

		public static bool ValidateSettingFileFormat(string filePath, Type type)
		{
			bool result = false;
			bool useMessageBox = RasterCommandSettingsSerializer.MessageBox;
			try
			{
				// disable message box
				RasterCommandSettingsSerializer.MessageBox = false;

				using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					AutoCommandSettings settings = (AutoCommandSettings)RasterCommandSettingsSerializer.Deserialize(stream, type);
					result = settings != null;
				}
			}
			catch
			{
				return false;
			}
			finally
			{
				// restore message box
				RasterCommandSettingsSerializer.MessageBox = useMessageBox;
			}

			return result;
		}

		private bool ValidateSettingsFile(string fileName, Type type, string stepName)
		{
			if (fileName == "")
			{
				MessageBoxEx.Error("Settings file was not specified. Please try again");
				return false;
			}

			if (File.Exists(fileName) == false)
			{
				MessageBoxEx.Error("Settings file does not exist. Please try again");
				return false;
			}

			if (DlgLoadExtAutoCommandSettings.ValidateSettingFileFormat(fileName, type) == false)
			{
				MessageBoxEx.Error("The specified file does not contain settings for the " + stepName + " .");
				return false;
			}
			
			return true;
		}

		#endregion

		#region Internal Helpers

		private bool ValidateData()
		{
			if (this.ValidateSettingsFile(this.txtSettingsFile.Text, _stepSettingsType, _stepName) == false)
				return false;

			return true;
		}

		private void UpdateData(bool bSaveAndValidate)
		{
			if (bSaveAndValidate)
			{
				this._filePath = txtSettingsFile.Text;
			}
			else
			{
				txtSettingsFile.Text = this._filePath;
			}
		}

		#endregion

		
	}
}
