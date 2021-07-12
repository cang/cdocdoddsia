using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SIA.UI.Controls.Dialogs
{
	/// <summary>
	/// Summary description for DlgThresholdParameter.
	/// </summary>
	public class DlgThresholdParameter : System.Windows.Forms.Form
	{
		int _threshold = 0;

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown nudThreshold;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public DlgThresholdParameter()
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
			this.label1 = new System.Windows.Forms.Label();
			this.nudThreshold = new System.Windows.Forms.NumericUpDown();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.nudThreshold)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(60, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Threshold:";
			// 
			// nudThreshold
			// 
			this.nudThreshold.Location = new System.Drawing.Point(76, 9);
			this.nudThreshold.Maximum = new System.Decimal(new int[] {
																		 255,
																		 0,
																		 0,
																		 0});
			this.nudThreshold.Name = "nudThreshold";
			this.nudThreshold.Size = new System.Drawing.Size(60, 20);
			this.nudThreshold.TabIndex = 23;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(80, 44);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(52, 24);
			this.btnCancel.TabIndex = 42;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(12, 44);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(52, 24);
			this.btnOK.TabIndex = 41;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// DlgThresholdParameter
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(144, 78);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.nudThreshold);
			this.Controls.Add(this.label1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DlgThresholdParameter";
			this.Text = "Threshold Parameter";
			((System.ComponentModel.ISupportInitialize)(this.nudThreshold)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion		

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			_threshold = (int)this.nudThreshold.Value;
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{		
		}

		public int Threshold
		{
			get { return _threshold; }
		}
	}
}
