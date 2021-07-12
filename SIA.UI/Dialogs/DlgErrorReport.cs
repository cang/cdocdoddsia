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
	/// Summary description for DlgConfirmSendTraceLog.
	/// </summary>
	public class DlgErrorReport : DialogBase
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox grpSeparator;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btnSendReport;
		private System.Windows.Forms.Button btnDontSend;
		private System.Windows.Forms.LinkLabel lnkViewReport;
		private System.Windows.Forms.CheckBox chkDoNotAsk;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public DlgErrorReport()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgErrorReport));
			this.label1 = new System.Windows.Forms.Label();
			this.btnSendReport = new System.Windows.Forms.Button();
			this.btnDontSend = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.grpSeparator = new System.Windows.Forms.GroupBox();
			this.label4 = new System.Windows.Forms.Label();
			this.lnkViewReport = new System.Windows.Forms.LinkLabel();
			this.chkDoNotAsk = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.White;
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(392, 40);
			this.label1.TabIndex = 0;
			this.label1.Text = "The application has generate several errors while running.";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnSendReport
			// 
			this.btnSendReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSendReport.Location = new System.Drawing.Point(204, 162);
			this.btnSendReport.Name = "btnSendReport";
			this.btnSendReport.Size = new System.Drawing.Size(104, 23);
			this.btnSendReport.TabIndex = 1;
			this.btnSendReport.Text = "&Send error report";
			// 
			// btnDontSend
			// 
			this.btnDontSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDontSend.Location = new System.Drawing.Point(312, 162);
			this.btnDontSend.Name = "btnDontSend";
			this.btnDontSend.Size = new System.Drawing.Size(76, 23);
			this.btnDontSend.TabIndex = 1;
			this.btnDontSend.Text = "&Don\'t send";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Location = new System.Drawing.Point(10, 76);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(376, 44);
			this.label2.TabIndex = 0;
			this.label2.Text = "We have created an error report that you can send to help us improve Real-time De" +
				"fect ExtractorWe will treat this report as confidential and anonymous.";
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(10, 52);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(376, 20);
			this.label3.TabIndex = 0;
			this.label3.Text = "Please tell SiGlaz about this problem.";
			// 
			// grpSeparator
			// 
			this.grpSeparator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.grpSeparator.Location = new System.Drawing.Point(-10, 40);
			this.grpSeparator.Name = "grpSeparator";
			this.grpSeparator.Size = new System.Drawing.Size(412, 4);
			this.grpSeparator.TabIndex = 2;
			this.grpSeparator.TabStop = false;
			this.grpSeparator.Text = "groupBox1";
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.label4.Location = new System.Drawing.Point(10, 124);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(222, 16);
			this.label4.TabIndex = 0;
			this.label4.Text = "To see what data this error report contains,";
			// 
			// lnkViewReport
			// 
			this.lnkViewReport.Location = new System.Drawing.Point(228, 124);
			this.lnkViewReport.Name = "lnkViewReport";
			this.lnkViewReport.Size = new System.Drawing.Size(100, 16);
			this.lnkViewReport.TabIndex = 3;
			this.lnkViewReport.TabStop = true;
			this.lnkViewReport.Text = "click here";
			// 
			// chkDoNotAsk
			// 
			this.chkDoNotAsk.Location = new System.Drawing.Point(8, 160);
			this.chkDoNotAsk.Name = "chkDoNotAsk";
			this.chkDoNotAsk.Size = new System.Drawing.Size(124, 23);
			this.chkDoNotAsk.TabIndex = 4;
			this.chkDoNotAsk.Text = "Do not ask again";
			// 
			// DlgErrorReport
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(392, 188);
			this.Controls.Add(this.chkDoNotAsk);
			this.Controls.Add(this.lnkViewReport);
			this.Controls.Add(this.grpSeparator);
			this.Controls.Add(this.btnSendReport);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnDontSend);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label4);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DlgErrorReport";
			this.Text = "SiGlaz Image Analyzer";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
