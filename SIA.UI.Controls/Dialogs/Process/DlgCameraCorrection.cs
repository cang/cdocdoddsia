using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using SIA.Common;
using SIA.SystemLayer;

using SIA.UI.Controls;
using SIA.UI.Controls.Utilities;
using SIA.UI.Controls.Commands;

using SIA.UI.Components;
using SIA.UI.Components.Helpers;

using SIA.UI.Controls.Automation.Commands;

namespace SIA.UI.Controls.Dialogs
{
	/// <summary>
	/// Summary description for DlgCameraCorrection.
	/// </summary>
	public class DlgCameraCorrection : DialogBase
	{
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.ToolTip _toolTip;
		private System.Windows.Forms.Button btnCancel;
		public System.Windows.Forms.TextBox distCoeff_5;
		private System.Windows.Forms.CheckBox interpolation;
		public System.Windows.Forms.TextBox distCoeff_2;
		public System.Windows.Forms.TextBox skewCoeffs;
		public System.Windows.Forms.TextBox distCoeff_4;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label22;
		public System.Windows.Forms.NumericUpDown prinPtY;
		public System.Windows.Forms.NumericUpDown prinPtX;
		public System.Windows.Forms.TextBox distCoeff_3;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.Label label27;
		public System.Windows.Forms.TextBox distCoeff_1;
		public System.Windows.Forms.TextBox focalLength;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.ComponentModel.IContainer components;

		#region Members Fields
		private CommonImage _image = null;
		#endregion

		#region Properties

		public float FocalLength
		{
			get {return Convert.ToSingle(focalLength.Text);}
		}

		public PointF PrincipalPoint
		{
			get {return new PointF(Convert.ToSingle(prinPtX.Value), Convert.ToSingle(prinPtY.Value));}
		}

		public bool Interpolation
		{
			get {return interpolation.Checked;}
		}

		public float SkewCoeffs
		{
			get {return Convert.ToSingle(skewCoeffs.Text);}
		}
		
		public float[] DistanceCoeffs
		{
			get 
			{
				float coeff1 = Convert.ToSingle(distCoeff_1.Text);
				float coeff2 = Convert.ToSingle(distCoeff_2.Text);
				float coeff3 = Convert.ToSingle(distCoeff_3.Text);
				float coeff4 = Convert.ToSingle(distCoeff_4.Text);
				float coeff5 = Convert.ToSingle(distCoeff_5.Text);
				return new float[] {coeff1, coeff2, coeff3, coeff4, coeff5};
			}
		}

		
		#endregion

		public DlgCameraCorrection(CommonImage image) : base()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
	
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			_image = image;
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
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgCameraCorrection));
			this.btnOK = new System.Windows.Forms.Button();
			this._toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.btnCancel = new System.Windows.Forms.Button();
			this.distCoeff_5 = new System.Windows.Forms.TextBox();
			this.interpolation = new System.Windows.Forms.CheckBox();
			this.distCoeff_2 = new System.Windows.Forms.TextBox();
			this.skewCoeffs = new System.Windows.Forms.TextBox();
			this.distCoeff_4 = new System.Windows.Forms.TextBox();
			this.label19 = new System.Windows.Forms.Label();
			this.label22 = new System.Windows.Forms.Label();
			this.prinPtY = new System.Windows.Forms.NumericUpDown();
			this.prinPtX = new System.Windows.Forms.NumericUpDown();
			this.distCoeff_3 = new System.Windows.Forms.TextBox();
			this.label26 = new System.Windows.Forms.Label();
			this.label27 = new System.Windows.Forms.Label();
			this.distCoeff_1 = new System.Windows.Forms.TextBox();
			this.focalLength = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.label20 = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			((System.ComponentModel.ISupportInitialize)(this.prinPtY)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.prinPtX)).BeginInit();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(101, 204);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(84, 23);
			this.btnOK.TabIndex = 19;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(205, 204);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(84, 23);
			this.btnCancel.TabIndex = 20;
			this.btnCancel.Text = "Cancel";
			// 
			// distCoeff_5
			// 
			this.distCoeff_5.Location = new System.Drawing.Point(306, 140);
			this.distCoeff_5.Name = "distCoeff_5";
			this.distCoeff_5.Size = new System.Drawing.Size(72, 20);
			this.distCoeff_5.TabIndex = 16;
			this.distCoeff_5.Text = "-0.003";
			// 
			// interpolation
			// 
			this.interpolation.Location = new System.Drawing.Point(12, 168);
			this.interpolation.Name = "interpolation";
			this.interpolation.Size = new System.Drawing.Size(121, 20);
			this.interpolation.TabIndex = 17;
			this.interpolation.Text = "Interpolation";
			// 
			// distCoeff_2
			// 
			this.distCoeff_2.Location = new System.Drawing.Point(87, 140);
			this.distCoeff_2.Name = "distCoeff_2";
			this.distCoeff_2.Size = new System.Drawing.Size(69, 20);
			this.distCoeff_2.TabIndex = 13;
			this.distCoeff_2.Text = "-0.48";
			// 
			// skewCoeffs
			// 
			this.skewCoeffs.Enabled = false;
			this.skewCoeffs.Location = new System.Drawing.Point(140, 84);
			this.skewCoeffs.Name = "skewCoeffs";
			this.skewCoeffs.Size = new System.Drawing.Size(72, 20);
			this.skewCoeffs.TabIndex = 10;
			this.skewCoeffs.Text = "3550";
			// 
			// distCoeff_4
			// 
			this.distCoeff_4.Location = new System.Drawing.Point(232, 140);
			this.distCoeff_4.Name = "distCoeff_4";
			this.distCoeff_4.Size = new System.Drawing.Size(71, 20);
			this.distCoeff_4.TabIndex = 15;
			this.distCoeff_4.Text = "-0.003";
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(8, 56);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(121, 20);
			this.label19.TabIndex = 6;
			this.label19.Text = "Focal Length:";
			this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label22
			// 
			this.label22.Location = new System.Drawing.Point(8, 4);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(121, 20);
			this.label22.TabIndex = 0;
			this.label22.Text = "Principal Point:";
			this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// prinPtY
			// 
			this.prinPtY.Location = new System.Drawing.Point(223, 28);
			this.prinPtY.Maximum = new System.Decimal(new int[] {
																	32768,
																	0,
																	0,
																	0});
			this.prinPtY.Name = "prinPtY";
			this.prinPtY.Size = new System.Drawing.Size(92, 20);
			this.prinPtY.TabIndex = 4;
			this.prinPtY.Value = new System.Decimal(new int[] {
																  1024,
																  0,
																  0,
																  0});
			// 
			// prinPtX
			// 
			this.prinPtX.Location = new System.Drawing.Point(47, 28);
			this.prinPtX.Maximum = new System.Decimal(new int[] {
																	32768,
																	0,
																	0,
																	0});
			this.prinPtX.Name = "prinPtX";
			this.prinPtX.Size = new System.Drawing.Size(92, 20);
			this.prinPtX.TabIndex = 2;
			this.prinPtX.Value = new System.Decimal(new int[] {
																  1024,
																  0,
																  0,
																  0});
			// 
			// distCoeff_3
			// 
			this.distCoeff_3.Location = new System.Drawing.Point(159, 140);
			this.distCoeff_3.Name = "distCoeff_3";
			this.distCoeff_3.Size = new System.Drawing.Size(70, 20);
			this.distCoeff_3.TabIndex = 14;
			this.distCoeff_3.Text = "-0.007";
			// 
			// label26
			// 
			this.label26.Enabled = false;
			this.label26.Location = new System.Drawing.Point(8, 84);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(121, 20);
			this.label26.TabIndex = 9;
			this.label26.Text = "Skew Coefficient:";
			this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label27
			// 
			this.label27.Location = new System.Drawing.Point(8, 112);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(121, 20);
			this.label27.TabIndex = 11;
			this.label27.Text = "Distortion Coefficients:";
			this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// distCoeff_1
			// 
			this.distCoeff_1.Location = new System.Drawing.Point(12, 140);
			this.distCoeff_1.Name = "distCoeff_1";
			this.distCoeff_1.Size = new System.Drawing.Size(72, 20);
			this.distCoeff_1.TabIndex = 12;
			this.distCoeff_1.Text = "0.28";
			// 
			// focalLength
			// 
			this.focalLength.Location = new System.Drawing.Point(140, 56);
			this.focalLength.Name = "focalLength";
			this.focalLength.Size = new System.Drawing.Size(72, 20);
			this.focalLength.TabIndex = 7;
			this.focalLength.Text = "3550.0";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(23, 32);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(24, 20);
			this.label6.TabIndex = 1;
			this.label6.Text = "X=";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(195, 28);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(20, 20);
			this.label14.TabIndex = 3;
			this.label14.Text = "Y=";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label20
			// 
			this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label20.Location = new System.Drawing.Point(212, 56);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(32, 20);
			this.label20.TabIndex = 8;
			this.label20.Text = "(mm)";
			this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label21
			// 
			this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label21.Location = new System.Drawing.Point(139, 28);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(48, 20);
			this.label21.TabIndex = 2;
			this.label21.Text = "(pixels)";
			this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label23
			// 
			this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label23.Location = new System.Drawing.Point(319, 28);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(48, 20);
			this.label23.TabIndex = 5;
			this.label23.Text = "(pixels)";
			this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Location = new System.Drawing.Point(-71, 192);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(532, 4);
			this.groupBox2.TabIndex = 18;
			this.groupBox2.TabStop = false;
			// 
			// DlgCameraCorrection
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(390, 232);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.distCoeff_2);
			this.Controls.Add(this.distCoeff_5);
			this.Controls.Add(this.skewCoeffs);
			this.Controls.Add(this.distCoeff_4);
			this.Controls.Add(this.label19);
			this.Controls.Add(this.prinPtY);
			this.Controls.Add(this.prinPtX);
			this.Controls.Add(this.distCoeff_3);
			this.Controls.Add(this.label26);
			this.Controls.Add(this.label27);
			this.Controls.Add(this.distCoeff_1);
			this.Controls.Add(this.focalLength);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label14);
			this.Controls.Add(this.label20);
			this.Controls.Add(this.label22);
			this.Controls.Add(this.label21);
			this.Controls.Add(this.label23);
			this.Controls.Add(this.interpolation);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DlgCameraCorrection";
			this.Text = "Camera Correction";
			((System.ComponentModel.ISupportInitialize)(this.prinPtY)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.prinPtX)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		protected override object OnGetDefaultValue(Control ctrl)
		{
			if (ctrl == prinPtX)
				return Convert.ToDecimal(1024);
			else if (ctrl == prinPtY)
				return Convert.ToDecimal(1024);
			else if (ctrl == focalLength)
				return "3550";
			else if (ctrl == distCoeff_1)
				return "0.28";
			else if (ctrl == distCoeff_2)
				return "-0.48";
			else if (ctrl == distCoeff_3)
				return "-0.007";
			else if (ctrl == distCoeff_4)
				return "0.003";
			else if (ctrl == distCoeff_5)
				return "0.0";
			else if (ctrl == interpolation)
				return false;

			return base.OnGetDefaultValue (ctrl);
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{

		}

		protected override void OnClosing(CancelEventArgs e)
		{
			if (this.DialogResult == DialogResult.OK && this.ValidateParameters() == false)
				e.Cancel = true;
			base.OnClosing (e);
		}

		private bool ValidateParameters()
		{
			bool result = false;
			int imgWidth = this._image.Width;
			int imgHeight = this._image.Height;

			if (!this.IsInRange(prinPtX.Value, 0, imgWidth) || !this.IsInRange(prinPtY.Value, 0, imgHeight))
				this.DoError("Invalid principal point. Principal point must be within the image boundary.");
			else if (!IsNumber(this.focalLength.Text))
				this.DoError("Invalid focal length value.");
			else if (!IsNumber(this.distCoeff_1.Text) || !IsNumber(this.distCoeff_2.Text) || 
					 !IsNumber(this.distCoeff_3.Text) || !IsNumber(this.distCoeff_4.Text) || !IsNumber(this.distCoeff_5.Text))
			{
				this.DoError("Invalid distance coefficients value.");
			}
			else
			{
				result = true; // passed all tests.
			}

			return result;
		}

		private bool IsInRange(Decimal value, float min, float max)
		{
			float val = Convert.ToSingle(value);
			return min<=val && val<=max;
		}

		private bool IsNumber(string text)
		{
			try 
			{
				Convert.ToDouble(text);
				return true;
			}
			catch 
			{
				return false;
			}
		}

		private void DoError(string msg)
		{
			MessageBoxEx.Error(msg);
		}

		private void btnSaveSettings_Click(object sender, System.EventArgs e)
		{
		
		}

		private void btnLoadSettings_Click(object sender, System.EventArgs e)
		{
		
		}
	}
}
