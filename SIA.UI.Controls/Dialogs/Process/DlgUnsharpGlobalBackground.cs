using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using SIA.UI.Controls.Utilities;

namespace SIA.UI.Controls.Dialogs
{
	/// <summary>
	/// Name : DlgUnsharpGlobalBackground
	/// Description : User interface for Un-sharp Global Background Settings
	/// Thread Support : True
	/// Persistence Data : True
	/// </summary>
	public class DlgUnsharpGlobalBackground  :  DialogBase
	{
		#region Windows Form member attributes
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label Radius;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		public System.Windows.Forms.RadioButton rdMean;
		public System.Windows.Forms.RadioButton rdGauss;
		public System.Windows.Forms.NumericUpDown nudMin;
		public System.Windows.Forms.NumericUpDown nudMax;
		public System.Windows.Forms.NumericUpDown nudRadius;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox grpContainers;
		private System.Windows.Forms.Label lblThresholdUnit;
		private System.Windows.Forms.Label label2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion

		#region constructor and destructor
		
		public DlgUnsharpGlobalBackground()
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
		
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgUnsharpGlobalBackground));
			this.nudMin = new System.Windows.Forms.NumericUpDown();
			this.nudMax = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.nudRadius = new System.Windows.Forms.NumericUpDown();
			this.Radius = new System.Windows.Forms.Label();
			this.rdGauss = new System.Windows.Forms.RadioButton();
			this.rdMean = new System.Windows.Forms.RadioButton();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.grpContainers = new System.Windows.Forms.GroupBox();
			this.lblThresholdUnit = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.nudMin)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMax)).BeginInit();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudRadius)).BeginInit();
			this.grpContainers.SuspendLayout();
			this.SuspendLayout();
			// 
			// nudMin
			// 
			this.nudMin.Location = new System.Drawing.Point(66, 16);
			this.nudMin.Maximum = new System.Decimal(new int[] {
																   255,
																   0,
																   0,
																   0});
			this.nudMin.Name = "nudMin";
			this.nudMin.Size = new System.Drawing.Size(57, 20);
			this.nudMin.TabIndex = 5;
			this.nudMin.Tag = "DEFAULT";
			this.nudMin.Value = new System.Decimal(new int[] {
																 25,
																 0,
																 0,
																 0});
			// 
			// nudMax
			// 
			this.nudMax.Location = new System.Drawing.Point(66, 40);
			this.nudMax.Maximum = new System.Decimal(new int[] {
																   255,
																   0,
																   0,
																   0});
			this.nudMax.Name = "nudMax";
			this.nudMax.Size = new System.Drawing.Size(57, 20);
			this.nudMax.TabIndex = 8;
			this.nudMax.Tag = "DEFAULT";
			this.nudMax.Value = new System.Decimal(new int[] {
																 220,
																 0,
																 0,
																 0});
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(10, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(51, 20);
			this.label1.TabIndex = 7;
			this.label1.Text = "Max:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.nudRadius);
			this.groupBox1.Controls.Add(this.Radius);
			this.groupBox1.Controls.Add(this.rdGauss);
			this.groupBox1.Controls.Add(this.rdMean);
			this.groupBox1.Location = new System.Drawing.Point(10, 64);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(234, 100);
			this.groupBox1.TabIndex = 9;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Filter Type";
			// 
			// nudRadius
			// 
			this.nudRadius.Location = new System.Drawing.Point(104, 72);
			this.nudRadius.Maximum = new System.Decimal(new int[] {
																	  15,
																	  0,
																	  0,
																	  0});
			this.nudRadius.Minimum = new System.Decimal(new int[] {
																	  1,
																	  0,
																	  0,
																	  0});
			this.nudRadius.Name = "nudRadius";
			this.nudRadius.Size = new System.Drawing.Size(60, 20);
			this.nudRadius.TabIndex = 9;
			this.nudRadius.Tag = "DEFAULT";
			this.nudRadius.Value = new System.Decimal(new int[] {
																	2,
																	0,
																	0,
																	0});
			// 
			// Radius
			// 
			this.Radius.Location = new System.Drawing.Point(20, 72);
			this.Radius.Name = "Radius";
			this.Radius.Size = new System.Drawing.Size(83, 20);
			this.Radius.TabIndex = 2;
			this.Radius.Text = "Kernel Radius:";
			this.Radius.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// rdGauss
			// 
			this.rdGauss.Location = new System.Drawing.Point(13, 45);
			this.rdGauss.Name = "rdGauss";
			this.rdGauss.Size = new System.Drawing.Size(107, 20);
			this.rdGauss.TabIndex = 1;
			this.rdGauss.Tag = "DEFAULT";
			this.rdGauss.Text = "Gaussian Filter";
			// 
			// rdMean
			// 
			this.rdMean.Checked = true;
			this.rdMean.Location = new System.Drawing.Point(13, 21);
			this.rdMean.Name = "rdMean";
			this.rdMean.Size = new System.Drawing.Size(87, 21);
			this.rdMean.TabIndex = 0;
			this.rdMean.TabStop = true;
			this.rdMean.Tag = "DEFAULT";
			this.rdMean.Text = "Mean Filter";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(264, 36);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 11;
			this.btnCancel.Text = "Cancel";
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(264, 8);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 10;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(10, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(51, 20);
			this.label3.TabIndex = 12;
			this.label3.Text = "Min:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// grpContainers
			// 
			this.grpContainers.Controls.Add(this.groupBox1);
			this.grpContainers.Controls.Add(this.label3);
			this.grpContainers.Controls.Add(this.nudMax);
			this.grpContainers.Controls.Add(this.label1);
			this.grpContainers.Controls.Add(this.nudMin);
			this.grpContainers.Controls.Add(this.lblThresholdUnit);
			this.grpContainers.Controls.Add(this.label2);
			this.grpContainers.Location = new System.Drawing.Point(4, 4);
			this.grpContainers.Name = "grpContainers";
			this.grpContainers.Size = new System.Drawing.Size(252, 172);
			this.grpContainers.TabIndex = 9;
			this.grpContainers.TabStop = false;
			this.grpContainers.Text = "Settings";
			// 
			// lblThresholdUnit
			// 
			this.lblThresholdUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblThresholdUnit.Location = new System.Drawing.Point(128, 40);
			this.lblThresholdUnit.Name = "lblThresholdUnit";
			this.lblThresholdUnit.Size = new System.Drawing.Size(64, 20);
			this.lblThresholdUnit.TabIndex = 29;
			this.lblThresholdUnit.Text = "(Intensity)";
			this.lblThresholdUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(128, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 20);
			this.label2.TabIndex = 29;
			this.label2.Text = "(Intensity)";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// DlgUnsharpGlobalBackground
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(346, 180);
			this.Controls.Add(this.grpContainers);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DlgUnsharpGlobalBackground";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Un-sharp Settings";
			this.Load += new System.EventHandler(this.DlgUnsharpGlobalBackground_Load);
			((System.ComponentModel.ISupportInitialize)(this.nudMin)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMax)).EndInit();
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.nudRadius)).EndInit();
			this.grpContainers.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region public properties

		public float Minimum
		{
			get {return (float)nudMin.Value;}
			set {nudMin.Value = (Decimal)value;}
		}

		public float Maximum
		{
			get {return (float)nudMax.Value;}
			set {nudMax.Value = (Decimal)value;}
		}

		public int FilterType
		{
			get {return this.rdGauss.Checked ? 1 : 0;}
			set
			{
				rdGauss.Checked = value != 0;
				rdMean.Checked = value == 0;
			}
		}

		public int KernelRadius
		{
			get {return (int)this.nudRadius.Value;}
			set {this.nudRadius.Value = (Decimal)value;}
		}
		

		#endregion

		#region event handlers

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			
		}
		
		private void DlgUnsharpGlobalBackground_Load(object sender, System.EventArgs e)
		{

		}

		#endregion

		#region internal routines

		private bool UpdateData(bool bSaveAndValidate)
		{
			bool result = false;
			if (bSaveAndValidate)
			{
				if (this.Minimum > this.Maximum)
				{
					MessageBoxEx.Error("Invalid minimum and maximum values");
					return false;
				}
				result = true;
			}
			else
				result = true;

			return result;
		}

		#endregion

		#region override routines

		protected override object OnGetDefaultValue(Control ctrl)
		{
			if (ctrl == nudMin)
				return (Decimal)25;
			else if (ctrl == nudMax)
				return (Decimal)220;
			else if(ctrl == rdMean)
				return true;
			else if(ctrl == rdGauss)
				return false;
			else if(ctrl == nudRadius)
				return (Decimal)2;


			return null;
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			if (this.DialogResult == DialogResult.OK)
				e.Cancel = this.UpdateData(true) == false;
			base.OnClosing (e);
		}


		#endregion
		
	}
}
