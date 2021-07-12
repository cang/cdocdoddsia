using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using SIA.UI.Controls;
using SIA.UI.Controls.Dialogs;

namespace SIA.UI.Dialogs
{
	/// <summary>
	/// Summary description for DlgSendTraceLog.
	/// </summary>
	public class DlgErrorReportDetail : DialogBase
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtFromEmail;
		private System.Windows.Forms.TextBox txtToEmail;
		private System.Windows.Forms.RichTextBox txtTraceLogInfo;
		private System.Windows.Forms.Button btnSend;
		private System.Windows.Forms.Button btnClose;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private string _traceLogFileName = "";
		private string _consoleFileName = "";

		protected DlgErrorReportDetail()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		public DlgErrorReportDetail(string traceLogFileName, string consoleFileName)
		{
			_traceLogFileName = traceLogFileName;
			_consoleFileName = consoleFileName;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgErrorReportDetail));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtFromEmail = new System.Windows.Forms.TextBox();
			this.txtToEmail = new System.Windows.Forms.TextBox();
			this.txtTraceLogInfo = new System.Windows.Forms.RichTextBox();
			this.btnSend = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(4, 4);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(36, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "From:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(4, 28);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(36, 20);
			this.label2.TabIndex = 0;
			this.label2.Text = "To:";
			// 
			// txtFromEmail
			// 
			this.txtFromEmail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFromEmail.Location = new System.Drawing.Point(44, 4);
			this.txtFromEmail.Name = "txtFromEmail";
			this.txtFromEmail.Size = new System.Drawing.Size(432, 20);
			this.txtFromEmail.TabIndex = 1;
			this.txtFromEmail.Text = "";
			// 
			// txtToEmail
			// 
			this.txtToEmail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtToEmail.Location = new System.Drawing.Point(44, 28);
			this.txtToEmail.Name = "txtToEmail";
			this.txtToEmail.ReadOnly = true;
			this.txtToEmail.Size = new System.Drawing.Size(432, 20);
			this.txtToEmail.TabIndex = 1;
			this.txtToEmail.Text = "support@siglaz.com";
			// 
			// txtTraceLogInfo
			// 
			this.txtTraceLogInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtTraceLogInfo.Location = new System.Drawing.Point(4, 52);
			this.txtTraceLogInfo.Name = "txtTraceLogInfo";
			this.txtTraceLogInfo.Size = new System.Drawing.Size(468, 192);
			this.txtTraceLogInfo.TabIndex = 2;
			this.txtTraceLogInfo.Text = "richTextBox1";
			// 
			// btnSend
			// 
			this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSend.Location = new System.Drawing.Point(316, 248);
			this.btnSend.Name = "btnSend";
			this.btnSend.TabIndex = 3;
			this.btnSend.Text = "&Send";
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.Location = new System.Drawing.Point(400, 248);
			this.btnClose.Name = "btnClose";
			this.btnClose.TabIndex = 3;
			this.btnClose.Text = "&Close";
			// 
			// DlgErrorReportDetail
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(478, 276);
			this.Controls.Add(this.btnSend);
			this.Controls.Add(this.txtTraceLogInfo);
			this.Controls.Add(this.txtFromEmail);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtToEmail);
			this.Controls.Add(this.btnClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DlgErrorReportDetail";
			this.Text = "Error Report Detail";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
