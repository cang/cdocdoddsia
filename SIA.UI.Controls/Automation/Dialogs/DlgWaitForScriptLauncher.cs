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

namespace SIA.UI.Controls.Automation.Dialogs
{
	/// <summary>
	/// Summary description for DlgWaitForScriptLauncher.
	/// </summary>
	public class DlgWaitForScriptLauncher : DialogBase
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.PictureBox _pictureBox;
		private System.Windows.Forms.Button btnStop;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;


		private bool _isStopped;

		public bool IsStopped
		{
			get {return _isStopped;}
		}

		public DlgWaitForScriptLauncher()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgWaitForScriptLauncher));
			this.btnStop = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this._pictureBox = new System.Windows.Forms.PictureBox();
			this.SuspendLayout();
			// 
			// btnStop
			// 
			this.btnStop.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnStop.Location = new System.Drawing.Point(132, 48);
			this.btnStop.Name = "btnStop";
			this.btnStop.TabIndex = 0;
			this.btnStop.Text = "Stop";
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(52, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(280, 32);
			this.label1.TabIndex = 1;
			this.label1.Text = "Please wait in a minute. The script is being processed. Press Stop to abort the o" +
				"peration.";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// _pictureBox
			// 
			this._pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("_pictureBox.Image")));
			this._pictureBox.Location = new System.Drawing.Point(8, 8);
			this._pictureBox.Name = "_pictureBox";
			this._pictureBox.Size = new System.Drawing.Size(32, 32);
			this._pictureBox.TabIndex = 10;
			this._pictureBox.TabStop = false;
			// 
			// DlgWaitForScriptLauncher
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnStop;
			this.ClientSize = new System.Drawing.Size(338, 76);
			this.Controls.Add(this._pictureBox);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnStop);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DlgWaitForScriptLauncher";
			this.Text = "Please wait...";
			this.ResumeLayout(false);

		}
		#endregion

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
		}

		private void btnStop_Click(object sender, System.EventArgs e)
		{
			_isStopped = true;
		}

	}
}
