using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using SIA.Common;
using SIA.Common.Imaging;

using SIA.UI.Controls.Automation;
using SIA.UI.Controls.Automation.Commands;
using SIA.UI.Controls.Automation.Utilities;

using SIA.UI.Controls.Common;
using SIA.UI.Controls.Utilities;
using SIA.UI.Controls.Dialogs;

namespace SIA.UI.Controls.Automation.Dialogs
{
	/// <summary>
	/// Summary description for DlgLoadImageStep.
	/// </summary>
	public class DlgLoadSingleImageStepSettings
        : DialogBase
	{
		#region Members

        #endregion

		#region Window Form Members

        private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private ucMaskStringEditor ucFileNameFormat;
        private CheckBox chkKeepOrgFile;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#endregion Window Form Members

        public bool KeepOrginalFileName
        {
            get { return chkKeepOrgFile.Checked; }
            set { chkKeepOrgFile.Checked = value; }
        }

        public string FileNameFormat
        {
            get { return this.ucFileNameFormat.FileNameFormat; }
            set { this.ucFileNameFormat.FileNameFormat = value; }
        }

		#region Constructor and destructor

		public DlgLoadSingleImageStepSettings()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
            ucFileNameFormat.FileTypes = FileTypes.ImageFileTypes;
            ucFileNameFormat.SelectedFileType = FileTypes.Fits;
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
		
		#region Properties

		#endregion Properties

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgLoadSingleImageStepSettings));
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.ucFileNameFormat = new SIA.UI.Controls.Automation.Dialogs.ucMaskStringEditor();
            this.chkKeepOrgFile = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Location = new System.Drawing.Point(-6, 124);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(703, 10);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(353, 137);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(265, 137);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "&OK";
            // 
            // ucFileNameFormat
            // 
            this.ucFileNameFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ucFileNameFormat.BrowseFileDialogTitle = "Select a file...";
            this.ucFileNameFormat.FileNameFormat = "";
            this.ucFileNameFormat.FileTypes = null;
            this.ucFileNameFormat.Location = new System.Drawing.Point(0, 1);
            this.ucFileNameFormat.Name = "ucFileNameFormat";
            this.ucFileNameFormat.SaveFileDialog = false;
            this.ucFileNameFormat.SelectedFileType = null;
            this.ucFileNameFormat.SelectedFileTypeIndex = 0;
            this.ucFileNameFormat.Size = new System.Drawing.Size(691, 94);
            this.ucFileNameFormat.SupportOnlyTextFile = false;
            this.ucFileNameFormat.TabIndex = 12;
            // 
            // chkKeepOrgFile
            // 
            this.chkKeepOrgFile.AutoSize = true;
            this.chkKeepOrgFile.Location = new System.Drawing.Point(12, 101);
            this.chkKeepOrgFile.Name = "chkKeepOrgFile";
            this.chkKeepOrgFile.Size = new System.Drawing.Size(139, 17);
            this.chkKeepOrgFile.TabIndex = 13;
            this.chkKeepOrgFile.Text = "Keep Original File Name";
            this.chkKeepOrgFile.UseVisualStyleBackColor = true;
            // 
            // DlgLoadSingleImageStepSettings
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(692, 167);
            this.Controls.Add(this.chkKeepOrgFile);
            this.Controls.Add(this.ucFileNameFormat);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DlgLoadSingleImageStepSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Load Single Image Step";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

	}
		
}
