using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;

using SIA.UI.Components.Printing;

namespace SIA.UI.Components.Dialogs
{
	internal class DlgPrintStatus : System.Windows.Forms.Form
	{
		internal DlgPrintStatus(BackgroundThread backgroundThread, string dialogTitle)
		{
			this.InitializeComponent();
			this.backgroundThread = backgroundThread;
			this.Text = dialogTitle;
			base.MinimumSize = base.Size;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.btnCancel.Enabled = false;
			this.lblStatus.Text = PrintControllerWithDlgPrintStatus.PrintControllerWithStatusDialog_Canceling;
			this.backgroundThread.canceled = true;
			this.backgroundThread.Cancel();
		}

		private void InitializeComponent()
		{
			this.lblStatus = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblStatus
			// 
			this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lblStatus.Location = new System.Drawing.Point(8, 16);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(240, 64);
			this.lblStatus.TabIndex = 1;
			this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(88, 88);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Text = PrintControllerWithDlgPrintStatus.PrintControllerWithStatusDialog_Cancel;
			this.btnCancel.TabIndex = 0;
			this.btnCancel.Click += new EventHandler(btnCancel_Click);
			// 
			// DlgPrintStatus
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(256, 122);
			this.ControlBox = false;
			this.Controls.Add(this.lblStatus);
			this.Controls.Add(this.btnCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DlgPrintStatus";
			this.Text = PrintControllerWithDlgPrintStatus.PrintControllerWithStatusDialog_DialogTitlePrint;
			this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
			this.ResumeLayout(false);

		}


		private BackgroundThread backgroundThread;
		private Button btnCancel;
		internal Label lblStatus;
	}
}
