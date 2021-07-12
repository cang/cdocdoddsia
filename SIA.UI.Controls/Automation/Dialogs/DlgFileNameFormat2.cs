using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using SIA.UI.Controls;
using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.Utilities;

namespace SIA.UI.Controls.Automation.Dialogs
{
	/// <summary>
	/// Summary description for DlgFileNameFormat2.
	/// </summary>
	public class DlgFileNameFormat2 : DialogBase
	{
		private SIA.UI.Controls.Automation.Dialogs.ucMaskStringEditor ucMaskStringEditor1;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.GroupBox groupBox1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public string FileNameFormat
		{
			get {return ucMaskStringEditor1.FileNameFormat;}
		}

		public int SelectedFileTypeIndex
		{
			get {return ucMaskStringEditor1.SelectedFileTypeIndex;}
		}

		public DlgFileNameFormat2(string fileNameFormat, IFileType fileType)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// initialize file name format control
			this.ucMaskStringEditor1.FileNameFormat = fileNameFormat;
			this.ucMaskStringEditor1.FileTypes = new IFileType[] {fileType};
			this.ucMaskStringEditor1.SelectedFileTypeIndex = 1;
		}

		public DlgFileNameFormat2(string fileNameFormat, IFileType[] fileTypes, int selFileType)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// initialize file name format control
			this.ucMaskStringEditor1.FileNameFormat = fileNameFormat;
			this.ucMaskStringEditor1.FileTypes = fileTypes;
			this.ucMaskStringEditor1.SelectedFileTypeIndex = selFileType;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgFileNameFormat2));
            this.ucMaskStringEditor1 = new SIA.UI.Controls.Automation.Dialogs.ucMaskStringEditor();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // ucMaskStringEditor1
            // 
            this.ucMaskStringEditor1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ucMaskStringEditor1.BrowseFileDialogTitle = "Save file as...";
            this.ucMaskStringEditor1.FileNameFormat = "";
            this.ucMaskStringEditor1.FileTypes = null;
            this.ucMaskStringEditor1.Location = new System.Drawing.Point(4, 4);
            this.ucMaskStringEditor1.Name = "ucMaskStringEditor1";
            this.ucMaskStringEditor1.SaveFileDialog = true;
            this.ucMaskStringEditor1.SelectedFileType = null;
            this.ucMaskStringEditor1.SelectedFileTypeIndex = -1;
            this.ucMaskStringEditor1.Size = new System.Drawing.Size(682, 88);
            this.ucMaskStringEditor1.SupportOnlyTextFile = false;
            this.ucMaskStringEditor1.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(351, 104);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(68, 24);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(271, 104);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(68, 24);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Location = new System.Drawing.Point(-6, 96);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(702, 4);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // DlgFileNameFormat2
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(690, 130);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ucMaskStringEditor1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(800, 168);
            this.MinimumSize = new System.Drawing.Size(596, 168);
            this.Name = "DlgFileNameFormat2";
            this.Text = "File Name Format";
            this.ResumeLayout(false);

		}
		#endregion
	}
}
