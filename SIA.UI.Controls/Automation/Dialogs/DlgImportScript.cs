using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.Utilities;
using SIA.UI.Controls.Automation.Utilities;

namespace SIA.UI.Controls.Automation.Dialogs
{
	/// <summary>
	/// Summary description for DlgImportScript.
	/// </summary>
	public class DlgImportScript : DialogBase
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtPackageFile;
		private System.Windows.Forms.TextBox txtImportDir;
		private System.Windows.Forms.Button btnBrowseFile;
		private System.Windows.Forms.Button btnChooseDir;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public string PackagePath
		{
			get {return txtPackageFile.Text;}
		}

		public string ImportDir
		{
			get {return txtImportDir.Text;}
		}

		public DlgImportScript()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgImportScript));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtPackageFile = new System.Windows.Forms.TextBox();
			this.txtImportDir = new System.Windows.Forms.TextBox();
			this.btnBrowseFile = new System.Windows.Forms.Button();
			this.btnChooseDir = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(0, 114);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(488, 4);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(159, 126);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 7;
			this.btnOK.Text = "OK";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(255, 126);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 8;
			this.btnCancel.Text = "Cancel";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(4, 4);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(420, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Script Package:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(4, 60);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(420, 23);
			this.label2.TabIndex = 3;
			this.label2.Text = "Destination Folder:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtPackageFile
			// 
			this.txtPackageFile.Location = new System.Drawing.Point(8, 32);
			this.txtPackageFile.Name = "txtPackageFile";
			this.txtPackageFile.Size = new System.Drawing.Size(420, 20);
			this.txtPackageFile.TabIndex = 1;
			this.txtPackageFile.Text = "";
			// 
			// txtImportDir
			// 
			this.txtImportDir.Location = new System.Drawing.Point(8, 88);
			this.txtImportDir.Name = "txtImportDir";
			this.txtImportDir.Size = new System.Drawing.Size(420, 20);
			this.txtImportDir.TabIndex = 4;
			this.txtImportDir.Text = "";
			// 
			// btnBrowseFile
			// 
			this.btnBrowseFile.Location = new System.Drawing.Point(436, 32);
			this.btnBrowseFile.Name = "btnBrowseFile";
			this.btnBrowseFile.Size = new System.Drawing.Size(44, 23);
			this.btnBrowseFile.TabIndex = 2;
			this.btnBrowseFile.Text = "...";
			this.btnBrowseFile.Click += new System.EventHandler(this.btnBrowseFile_Click);
			// 
			// btnChooseDir
			// 
			this.btnChooseDir.Location = new System.Drawing.Point(436, 88);
			this.btnChooseDir.Name = "btnChooseDir";
			this.btnChooseDir.Size = new System.Drawing.Size(44, 23);
			this.btnChooseDir.TabIndex = 5;
			this.btnChooseDir.Text = "...";
			this.btnChooseDir.Click += new System.EventHandler(this.btnBrowseDir_Click);
			// 
			// DlgImportScript
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(488, 156);
			this.Controls.Add(this.btnBrowseFile);
			this.Controls.Add(this.txtPackageFile);
			this.Controls.Add(this.txtImportDir);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btnChooseDir);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DlgImportScript";
			this.Text = "Import script from file...";
			this.ResumeLayout(false);

		}
		#endregion

		private void btnBrowseDir_Click(object sender, System.EventArgs e)
		{ 
			using (FolderBrowserDialog dlg = CommonDialogs.SelectFolderDialog("Please select a destination folder"))
			{
				if (dlg.ShowDialog(this) == DialogResult.OK)
					txtImportDir.Text = dlg.SelectedPath;
			}
		}

		private void btnBrowseFile_Click(object sender, System.EventArgs e)
		{
			using (OpenFileDialog dlg = CommonDialogs.OpenPackageFileDialog("Select a package file..."))
			{
				if (DialogResult.OK == dlg.ShowDialog(this))
				{
					txtPackageFile.Text = dlg.FileName;
				}
			}
		}
	}
}
