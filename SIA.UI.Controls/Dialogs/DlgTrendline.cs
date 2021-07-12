using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

using SIA.Common;
using SIA.Common.Mathematics;

using SIA.UI.Controls.Utilities;

namespace SIA.UI.Controls.Dialogs
{
	/// <summary>
	/// Name : DlgTrendline
	/// Description : User interface for Trend Line 
	/// Thread Support : True
	/// Persistence Data : True
	/// </summary>
	public class DlgTrendline : DialogBase
	{
		#region Windows Form member attributes
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.NumericUpDown nudOrder;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.PictureBox picMovingAverage;
		public System.Windows.Forms.PictureBox picExponential;
		public System.Windows.Forms.PictureBox picPower;
		private System.Windows.Forms.PictureBox picPolynomial;
		private System.Windows.Forms.PictureBox picLinear;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.PictureBox picCurent ;
		public System.Windows.Forms.NumericUpDown nudPeriod;
		private System.Windows.Forms.ImageList imageListTrendLine;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private UserControls.LineChart mLineChart;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.PictureBox picMiddleAverage;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.RadioButton rdoAverage;
		private System.Windows.Forms.RadioButton rdoMean;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.TextBox txtStandarDeviation;
		private System.Windows.Forms.NumericUpDown numericMultiplicationValue;
		private System.Windows.Forms.Label lblValue;
		private System.Windows.Forms.RadioButton rdoStandarDeviation;
		private System.Windows.Forms.RadioButton rdoValue;
		private System.Windows.Forms.NumericUpDown ValueNU;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.NumericUpDown numericKernel;
		private System.Windows.Forms.NumericUpDown numericMultiDeviation;
		private System.Windows.Forms.CheckBox chkKernel;
		private System.Windows.Forms.Label FunctionDev;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.GroupBox groupBox6;
		private System.Windows.Forms.CheckBox chkManualMaxMin;
		private System.Windows.Forms.Label label14;
		public System.Windows.Forms.NumericUpDown numericMinNoiseThreshold;
		private System.Windows.Forms.Label lblMinNoiseThreshold;
		public System.Windows.Forms.NumericUpDown numericMaxNoiseThreshold;

		#endregion

		#region member attributes

		private DlgLineProfile _dlgLineProfile;
		private eTrendlineType _trendlineType;
		private string cRadioBtn = null ;
		private TrendLineFormat _trendlineInfo = null;
		public System.Windows.Forms.CheckBox _bHeuristic;
		private bool _bInitializingDefaultValue = true;
		
		#endregion

		#region constructor and destructor

		public DlgTrendline()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			cRadioBtn = rdoAverage.Checked? rdoAverage.Text:rdoMean.Text;

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}
		public DlgLineProfile DlgLineProfile
		{
			get
			{
				return _dlgLineProfile;
			}
			set
			{
				_dlgLineProfile = value;
			}
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
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgTrendline));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this._bHeuristic = new System.Windows.Forms.CheckBox();
			this.label11 = new System.Windows.Forms.Label();
			this.picMiddleAverage = new System.Windows.Forms.PictureBox();
			this.picLinear = new System.Windows.Forms.PictureBox();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.nudPeriod = new System.Windows.Forms.NumericUpDown();
			this.label8 = new System.Windows.Forms.Label();
			this.nudOrder = new System.Windows.Forms.NumericUpDown();
			this.label7 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.picMovingAverage = new System.Windows.Forms.PictureBox();
			this.label5 = new System.Windows.Forms.Label();
			this.picExponential = new System.Windows.Forms.PictureBox();
			this.label6 = new System.Windows.Forms.Label();
			this.picPower = new System.Windows.Forms.PictureBox();
			this.label3 = new System.Windows.Forms.Label();
			this.picPolynomial = new System.Windows.Forms.PictureBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.imageListTrendLine = new System.Windows.Forms.ImageList(this.components);
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.numericKernel = new System.Windows.Forms.NumericUpDown();
			this.rdoMean = new System.Windows.Forms.RadioButton();
			this.rdoAverage = new System.Windows.Forms.RadioButton();
			this.label12 = new System.Windows.Forms.Label();
			this.chkKernel = new System.Windows.Forms.CheckBox();
			this.numericMultiDeviation = new System.Windows.Forms.NumericUpDown();
			this.label15 = new System.Windows.Forms.Label();
			this.txtStandarDeviation = new System.Windows.Forms.TextBox();
			this.numericMultiplicationValue = new System.Windows.Forms.NumericUpDown();
			this.lblValue = new System.Windows.Forms.Label();
			this.rdoStandarDeviation = new System.Windows.Forms.RadioButton();
			this.rdoValue = new System.Windows.Forms.RadioButton();
			this.ValueNU = new System.Windows.Forms.NumericUpDown();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label9 = new System.Windows.Forms.Label();
			this.FunctionDev = new System.Windows.Forms.Label();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.label14 = new System.Windows.Forms.Label();
			this.numericMinNoiseThreshold = new System.Windows.Forms.NumericUpDown();
			this.chkManualMaxMin = new System.Windows.Forms.CheckBox();
			this.numericMaxNoiseThreshold = new System.Windows.Forms.NumericUpDown();
			this.lblMinNoiseThreshold = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudPeriod)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudOrder)).BeginInit();
			this.groupBox5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericKernel)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericMultiDeviation)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericMultiplicationValue)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ValueNU)).BeginInit();
			this.groupBox2.SuspendLayout();
			this.groupBox6.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericMinNoiseThreshold)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericMaxNoiseThreshold)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this._bHeuristic);
			this.groupBox1.Controls.Add(this.label11);
			this.groupBox1.Controls.Add(this.picMiddleAverage);
			this.groupBox1.Controls.Add(this.picLinear);
			this.groupBox1.Controls.Add(this.pictureBox1);
			this.groupBox1.Controls.Add(this.nudPeriod);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.nudOrder);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.picMovingAverage);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.picExponential);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.picPower);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.picPolynomial);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(308, 244);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Trend/Regression Type";
			// 
			// _bHeuristic
			// 
			this._bHeuristic.Location = new System.Drawing.Point(12, 221);
			this._bHeuristic.Name = "_bHeuristic";
			this._bHeuristic.Size = new System.Drawing.Size(104, 20);
			this._bHeuristic.TabIndex = 19;
			this._bHeuristic.Tag = "DEFECUL";
			this._bHeuristic.Text = "Automatic";
			this._bHeuristic.CheckedChanged += new System.EventHandler(this.Heuristic_CheckedChanged);
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(147, 162);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(88, 26);
			this.label11.TabIndex = 18;
			this.label11.Text = "Middle Point Moving Average";
			this.label11.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// picMiddleAverage
			// 
			this.picMiddleAverage.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.picMiddleAverage.Image = ((System.Drawing.Image)(resources.GetObject("picMiddleAverage.Image")));
			this.picMiddleAverage.Location = new System.Drawing.Point(164, 106);
			this.picMiddleAverage.Name = "picMiddleAverage";
			this.picMiddleAverage.Size = new System.Drawing.Size(54, 47);
			this.picMiddleAverage.TabIndex = 17;
			this.picMiddleAverage.TabStop = false;
			this.picMiddleAverage.Tag = "6";
			// 
			// picLinear
			// 
			this.picLinear.Image = ((System.Drawing.Image)(resources.GetObject("picLinear.Image")));
			this.picLinear.Location = new System.Drawing.Point(12, 20);
			this.picLinear.Name = "picLinear";
			this.picLinear.Size = new System.Drawing.Size(54, 47);
			this.picLinear.TabIndex = 0;
			this.picLinear.TabStop = false;
			this.picLinear.Tag = "0";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(88, 20);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(54, 47);
			this.pictureBox1.TabIndex = 16;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.Tag = "1";
			// 
			// nudPeriod
			// 
			this.nudPeriod.Location = new System.Drawing.Point(178, 197);
			this.nudPeriod.Maximum = new System.Decimal(new int[] {
																	  10000,
																	  0,
																	  0,
																	  0});
			this.nudPeriod.Minimum = new System.Decimal(new int[] {
																	  2,
																	  0,
																	  0,
																	  0});
			this.nudPeriod.Name = "nudPeriod";
			this.nudPeriod.Size = new System.Drawing.Size(56, 20);
			this.nudPeriod.TabIndex = 15;
			this.nudPeriod.Value = new System.Decimal(new int[] {
																	2,
																	0,
																	0,
																	0});
			this.nudPeriod.ValueChanged += new System.EventHandler(this.nudPeriod_ValueChanged);
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(136, 201);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(40, 16);
			this.label8.TabIndex = 14;
			this.label8.Text = "Period:";
			// 
			// nudOrder
			// 
			this.nudOrder.Location = new System.Drawing.Point(50, 197);
			this.nudOrder.Maximum = new System.Decimal(new int[] {
																	 10,
																	 0,
																	 0,
																	 0});
			this.nudOrder.Minimum = new System.Decimal(new int[] {
																	 2,
																	 0,
																	 0,
																	 0});
			this.nudOrder.Name = "nudOrder";
			this.nudOrder.Size = new System.Drawing.Size(56, 20);
			this.nudOrder.TabIndex = 13;
			this.nudOrder.Value = new System.Decimal(new int[] {
																   2,
																   0,
																   0,
																   0});
			this.nudOrder.ValueChanged += new System.EventHandler(this.nudOrder_ValueChanged);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(12, 201);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(36, 16);
			this.label7.TabIndex = 12;
			this.label7.Text = "Order:";
			this.label7.Click += new System.EventHandler(this.label7_Click);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(91, 162);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(48, 26);
			this.label4.TabIndex = 11;
			this.label4.Text = "Moving Average";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// picMovingAverage
			// 
			this.picMovingAverage.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.picMovingAverage.Image = ((System.Drawing.Image)(resources.GetObject("picMovingAverage.Image")));
			this.picMovingAverage.Location = new System.Drawing.Point(88, 106);
			this.picMovingAverage.Name = "picMovingAverage";
			this.picMovingAverage.Size = new System.Drawing.Size(54, 47);
			this.picMovingAverage.TabIndex = 10;
			this.picMovingAverage.TabStop = false;
			this.picMovingAverage.Tag = "5";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(7, 162);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(64, 16);
			this.label5.TabIndex = 9;
			this.label5.Text = "Exponential";
			// 
			// picExponential
			// 
			this.picExponential.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.picExponential.Image = ((System.Drawing.Image)(resources.GetObject("picExponential.Image")));
			this.picExponential.Location = new System.Drawing.Point(12, 106);
			this.picExponential.Name = "picExponential";
			this.picExponential.Size = new System.Drawing.Size(54, 47);
			this.picExponential.TabIndex = 8;
			this.picExponential.TabStop = false;
			this.picExponential.Tag = "4";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(249, 78);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(36, 16);
			this.label6.TabIndex = 7;
			this.label6.Text = "Power";
			// 
			// picPower
			// 
			this.picPower.Image = ((System.Drawing.Image)(resources.GetObject("picPower.Image")));
			this.picPower.Location = new System.Drawing.Point(240, 20);
			this.picPower.Name = "picPower";
			this.picPower.Size = new System.Drawing.Size(54, 47);
			this.picPower.TabIndex = 6;
			this.picPower.TabStop = false;
			this.picPower.Tag = "3";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(161, 78);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(60, 16);
			this.label3.TabIndex = 5;
			this.label3.Text = "Polynomial";
			// 
			// picPolynomial
			// 
			this.picPolynomial.Image = ((System.Drawing.Image)(resources.GetObject("picPolynomial.Image")));
			this.picPolynomial.Location = new System.Drawing.Point(164, 20);
			this.picPolynomial.Name = "picPolynomial";
			this.picPolynomial.Size = new System.Drawing.Size(54, 47);
			this.picPolynomial.TabIndex = 4;
			this.picPolynomial.TabStop = false;
			this.picPolynomial.Tag = "2";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(84, 78);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(63, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "Logarithmic";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(21, 78);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(36, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Linear";
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(180, 287);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 7;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(276, 287);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 8;
			this.btnCancel.Text = "Cancel";
			// 
			// imageListTrendLine
			// 
			this.imageListTrendLine.ImageSize = new System.Drawing.Size(54, 47);
			this.imageListTrendLine.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTrendLine.ImageStream")));
			this.imageListTrendLine.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.numericKernel);
			this.groupBox5.Controls.Add(this.rdoMean);
			this.groupBox5.Controls.Add(this.rdoAverage);
			this.groupBox5.Controls.Add(this.label12);
			this.groupBox5.Controls.Add(this.chkKernel);
			this.groupBox5.Location = new System.Drawing.Point(320, 8);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(201, 72);
			this.groupBox5.TabIndex = 1;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Kernel Properties";
			// 
			// numericKernel
			// 
			this.numericKernel.Location = new System.Drawing.Point(112, 21);
			this.numericKernel.Maximum = new System.Decimal(new int[] {
																		  2048,
																		  0,
																		  0,
																		  0});
			this.numericKernel.Minimum = new System.Decimal(new int[] {
																		  1,
																		  0,
																		  0,
																		  0});
			this.numericKernel.Name = "numericKernel";
			this.numericKernel.Size = new System.Drawing.Size(64, 20);
			this.numericKernel.TabIndex = 1;
			this.numericKernel.Value = new System.Decimal(new int[] {
																		20,
																		0,
																		0,
																		0});
			this.numericKernel.Validating += new System.ComponentModel.CancelEventHandler(this.numericKernel_Validating);
			this.numericKernel.ValueChanged += new System.EventHandler(this.numericKernel_ValueChanged);
			// 
			// rdoMean
			// 
			this.rdoMean.Location = new System.Drawing.Point(112, 44);
			this.rdoMean.Name = "rdoMean";
			this.rdoMean.Size = new System.Drawing.Size(80, 19);
			this.rdoMean.TabIndex = 3;
			this.rdoMean.Text = "Mean";
			// 
			// rdoAverage
			// 
			this.rdoAverage.Checked = true;
			this.rdoAverage.Location = new System.Drawing.Point(37, 44);
			this.rdoAverage.Name = "rdoAverage";
			this.rdoAverage.Size = new System.Drawing.Size(83, 17);
			this.rdoAverage.TabIndex = 2;
			this.rdoAverage.TabStop = true;
			this.rdoAverage.Text = "Average";
			this.rdoAverage.CheckedChanged += new System.EventHandler(this.rdoAverage_CheckedChanged);
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(37, 24);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(65, 16);
			this.label12.TabIndex = 0;
			this.label12.Text = "Kernel Size:";
			// 
			// chkKernel
			// 
			this.chkKernel.Checked = true;
			this.chkKernel.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkKernel.Location = new System.Drawing.Point(8, -5);
			this.chkKernel.Name = "chkKernel";
			this.chkKernel.Size = new System.Drawing.Size(120, 24);
			this.chkKernel.TabIndex = 10;
			this.chkKernel.Text = "Kernel Properties";
			this.chkKernel.Visible = false;
			this.chkKernel.CheckedChanged += new System.EventHandler(this.chkKernel_CheckedChanged);
			// 
			// numericMultiDeviation
			// 
			this.numericMultiDeviation.DecimalPlaces = 2;
			this.numericMultiDeviation.Enabled = false;
			this.numericMultiDeviation.Location = new System.Drawing.Point(112, 41);
			this.numericMultiDeviation.Maximum = new System.Decimal(new int[] {
																				  65535,
																				  0,
																				  0,
																				  0});
			this.numericMultiDeviation.Minimum = new System.Decimal(new int[] {
																				  65535,
																				  0,
																				  0,
																				  -2147483648});
			this.numericMultiDeviation.Name = "numericMultiDeviation";
			this.numericMultiDeviation.Size = new System.Drawing.Size(64, 20);
			this.numericMultiDeviation.TabIndex = 3;
			this.numericMultiDeviation.Validating += new System.ComponentModel.CancelEventHandler(this.numericMultiDeviation_Validating);
			this.numericMultiDeviation.ValueChanged += new System.EventHandler(this.numericMultiDeviation_ValueChanged);
			// 
			// label15
			// 
			this.label15.AutoSize = true;
			this.label15.Location = new System.Drawing.Point(8, 256);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(147, 16);
			this.label15.TabIndex = 4;
			this.label15.Text = "Function Standard Deviation";
			// 
			// txtStandarDeviation
			// 
			this.txtStandarDeviation.AutoSize = false;
			this.txtStandarDeviation.Location = new System.Drawing.Point(424, 288);
			this.txtStandarDeviation.Name = "txtStandarDeviation";
			this.txtStandarDeviation.ReadOnly = true;
			this.txtStandarDeviation.Size = new System.Drawing.Size(88, 20);
			this.txtStandarDeviation.TabIndex = 9;
			this.txtStandarDeviation.Text = "0";
			this.txtStandarDeviation.Visible = false;
			// 
			// numericMultiplicationValue
			// 
			this.numericMultiplicationValue.DecimalPlaces = 2;
			this.numericMultiplicationValue.Increment = new System.Decimal(new int[] {
																						 5,
																						 0,
																						 0,
																						 65536});
			this.numericMultiplicationValue.Location = new System.Drawing.Point(424, 144);
			this.numericMultiplicationValue.Maximum = new System.Decimal(new int[] {
																					   50,
																					   0,
																					   0,
																					   0});
			this.numericMultiplicationValue.Minimum = new System.Decimal(new int[] {
																					   50,
																					   0,
																					   0,
																					   -2147483648});
			this.numericMultiplicationValue.Name = "numericMultiplicationValue";
			this.numericMultiplicationValue.Size = new System.Drawing.Size(88, 20);
			this.numericMultiplicationValue.TabIndex = 3;
			this.numericMultiplicationValue.Value = new System.Decimal(new int[] {
																					 15,
																					 0,
																					 0,
																					 65536});
			this.numericMultiplicationValue.Visible = false;
			this.numericMultiplicationValue.ValueChanged += new System.EventHandler(this.numericMultiplicationValue_ValueChanged);
			// 
			// lblValue
			// 
			this.lblValue.Location = new System.Drawing.Point(412, 192);
			this.lblValue.Name = "lblValue";
			this.lblValue.Size = new System.Drawing.Size(100, 15);
			this.lblValue.TabIndex = 21;
			this.lblValue.Visible = false;
			// 
			// rdoStandarDeviation
			// 
			this.rdoStandarDeviation.CausesValidation = false;
			this.rdoStandarDeviation.Location = new System.Drawing.Point(37, 42);
			this.rdoStandarDeviation.Name = "rdoStandarDeviation";
			this.rdoStandarDeviation.Size = new System.Drawing.Size(75, 24);
			this.rdoStandarDeviation.TabIndex = 1;
			this.rdoStandarDeviation.Text = "Deviation:";
			this.rdoStandarDeviation.CheckedChanged += new System.EventHandler(this.rdoStandarDeviation_CheckedChanged);
			// 
			// rdoValue
			// 
			this.rdoValue.CausesValidation = false;
			this.rdoValue.Checked = true;
			this.rdoValue.Location = new System.Drawing.Point(37, 17);
			this.rdoValue.Name = "rdoValue";
			this.rdoValue.Size = new System.Drawing.Size(56, 24);
			this.rdoValue.TabIndex = 0;
			this.rdoValue.TabStop = true;
			this.rdoValue.Text = "Value:";
			this.rdoValue.CheckedChanged += new System.EventHandler(this.rdoValue_CheckedChanged);
			// 
			// ValueNU
			// 
			this.ValueNU.DecimalPlaces = 2;
			this.ValueNU.Location = new System.Drawing.Point(112, 17);
			this.ValueNU.Maximum = new System.Decimal(new int[] {
																	65535,
																	0,
																	0,
																	0});
			this.ValueNU.Minimum = new System.Decimal(new int[] {
																	65535,
																	0,
																	0,
																	-2147483648});
			this.ValueNU.Name = "ValueNU";
			this.ValueNU.Size = new System.Drawing.Size(64, 20);
			this.ValueNU.TabIndex = 2;
			this.ValueNU.Value = new System.Decimal(new int[] {
																  3,
																  0,
																  0,
																  0});
			this.ValueNU.ValueChanged += new System.EventHandler(this.ValueNU_ValueChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label9);
			this.groupBox2.Controls.Add(this.rdoStandarDeviation);
			this.groupBox2.Controls.Add(this.rdoValue);
			this.groupBox2.Controls.Add(this.ValueNU);
			this.groupBox2.Controls.Add(this.numericMultiDeviation);
			this.groupBox2.Location = new System.Drawing.Point(320, 85);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(201, 81);
			this.groupBox2.TabIndex = 2;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Constant";
			// 
			// label9
			// 
			this.label9.BackColor = System.Drawing.Color.Transparent;
			this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(163)));
			this.label9.Location = new System.Drawing.Point(98, 65);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(88, 16);
			this.label9.TabIndex = 19;
			this.label9.Text = "(kernel std . dev)";
			// 
			// FunctionDev
			// 
			this.FunctionDev.Location = new System.Drawing.Point(156, 256);
			this.FunctionDev.Name = "FunctionDev";
			this.FunctionDev.Size = new System.Drawing.Size(136, 16);
			this.FunctionDev.TabIndex = 5;
			// 
			// groupBox4
			// 
			this.groupBox4.Location = new System.Drawing.Point(8, 276);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(904, 4);
			this.groupBox4.TabIndex = 6;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "groupBox4";
			// 
			// groupBox6
			// 
			this.groupBox6.Controls.Add(this.label14);
			this.groupBox6.Controls.Add(this.numericMinNoiseThreshold);
			this.groupBox6.Controls.Add(this.chkManualMaxMin);
			this.groupBox6.Controls.Add(this.numericMaxNoiseThreshold);
			this.groupBox6.Controls.Add(this.lblMinNoiseThreshold);
			this.groupBox6.Location = new System.Drawing.Point(320, 172);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(201, 80);
			this.groupBox6.TabIndex = 3;
			this.groupBox6.TabStop = false;
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point(37, 51);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(29, 16);
			this.label14.TabIndex = 15;
			this.label14.Text = "Max:";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// numericMinNoiseThreshold
			// 
			this.numericMinNoiseThreshold.Location = new System.Drawing.Point(112, 23);
			this.numericMinNoiseThreshold.Maximum = new System.Decimal(new int[] {
																					 1410065408,
																					 2,
																					 0,
																					 0});
			this.numericMinNoiseThreshold.Name = "numericMinNoiseThreshold";
			this.numericMinNoiseThreshold.Size = new System.Drawing.Size(64, 20);
			this.numericMinNoiseThreshold.TabIndex = 5;
			this.numericMinNoiseThreshold.Value = new System.Decimal(new int[] {
																				   5,
																				   0,
																				   0,
																				   0});
			this.numericMinNoiseThreshold.ValueChanged += new System.EventHandler(this.numericMinNoiseThreshold_ValueChanged);
			// 
			// chkManualMaxMin
			// 
			this.chkManualMaxMin.Checked = true;
			this.chkManualMaxMin.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkManualMaxMin.Location = new System.Drawing.Point(8, -6);
			this.chkManualMaxMin.Name = "chkManualMaxMin";
			this.chkManualMaxMin.Size = new System.Drawing.Size(152, 24);
			this.chkManualMaxMin.TabIndex = 4;
			this.chkManualMaxMin.Text = "Manual Noise Threshold";
			this.chkManualMaxMin.CheckedChanged += new System.EventHandler(this.chkManualMaxMin_CheckedChanged);
			// 
			// numericMaxNoiseThreshold
			// 
			this.numericMaxNoiseThreshold.Location = new System.Drawing.Point(112, 47);
			this.numericMaxNoiseThreshold.Maximum = new System.Decimal(new int[] {
																					 1410065408,
																					 2,
																					 0,
																					 0});
			this.numericMaxNoiseThreshold.Minimum = new System.Decimal(new int[] {
																					 1,
																					 0,
																					 0,
																					 0});
			this.numericMaxNoiseThreshold.Name = "numericMaxNoiseThreshold";
			this.numericMaxNoiseThreshold.Size = new System.Drawing.Size(64, 20);
			this.numericMaxNoiseThreshold.TabIndex = 6;
			this.numericMaxNoiseThreshold.Value = new System.Decimal(new int[] {
																				   1,
																				   0,
																				   0,
																				   0});
			this.numericMaxNoiseThreshold.ValueChanged += new System.EventHandler(this.numericMaxNoiseThreshold_ValueChanged);
			// 
			// lblMinNoiseThreshold
			// 
			this.lblMinNoiseThreshold.AutoSize = true;
			this.lblMinNoiseThreshold.Location = new System.Drawing.Point(37, 27);
			this.lblMinNoiseThreshold.Name = "lblMinNoiseThreshold";
			this.lblMinNoiseThreshold.Size = new System.Drawing.Size(26, 16);
			this.lblMinNoiseThreshold.TabIndex = 13;
			this.lblMinNoiseThreshold.Text = "Min:";
			this.lblMinNoiseThreshold.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// DlgTrendline
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(530, 317);
			this.Controls.Add(this.groupBox6);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.groupBox5);
			this.Controls.Add(this.FunctionDev);
			this.Controls.Add(this.label15);
			this.Controls.Add(this.txtStandarDeviation);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.numericMultiplicationValue);
			this.Controls.Add(this.lblValue);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DlgTrendline";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Trendline Format";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TrendlineDlg_KeyDown);
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TrendlineDlg_KeyPress);
			this.Load += new System.EventHandler(this.TrendlineDlg_Load);
			this.Layout += new System.Windows.Forms.LayoutEventHandler(this.TrendlineDlg_Layout);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TrendlineDlg_KeyUp);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.nudPeriod)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudOrder)).EndInit();
			this.groupBox5.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numericKernel)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericMultiDeviation)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericMultiplicationValue)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ValueNU)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox6.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numericMinNoiseThreshold)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericMaxNoiseThreshold)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region public properties

		public eTrendlineType TrenlineType
		{
			get { return GetTrendlineType();}
			set {_trendlineType = value;}
		}
		public eKernelType KernelType
		{
			get { return GetKernelType();}
			set {KernelType = value;}
		}

		public int Order
		{
			get {return (int)nudOrder.Value; }
		}

		public int Period
		{
			get {return (int)nudPeriod.Value; } 
		}
		public int KernelSize
		{
			get {return (int)numericKernel.Value; } 
		}
		public double MultipleDeviation
		{
			get {return (double)numericMultiDeviation.Value; } 
		}
		public double ConstantValue
		{
			get
			{
				return (double)ValueNU.Value;
			}

		}

		public bool ValueCheck
		{
			get
			{
				return rdoValue.Checked ;
			}
			set
			{
				rdoValue.Checked = value ;
			}
		}
		public bool StandarDevitionCheck
		{
			get
			{
				return rdoStandarDeviation.Checked ;
			}
			set
			{
				rdoStandarDeviation.Checked = value ;
			}
		}

		public double StandarDevitionValue
		{
			get
			{
				return double.Parse(txtStandarDeviation.Text) ;
			}
			set
			{
				txtStandarDeviation.Text = value.ToString() ;
			}
		}
		public double MaxNoiseThresHold
		{
			get
			{
				return (double)numericMaxNoiseThreshold.Value ;
			}
			set
			{
				numericMaxNoiseThreshold.Value =Decimal.Parse(value.ToString()) ;
			}
		}
		public double MinNoiseThreshold
		{
			get
			{
				return (double)numericMinNoiseThreshold.Value ;
			}
			set
			{
				numericMinNoiseThreshold.Value =Decimal.Parse(value.ToString()) ;
			}
		}

		public bool ManualMaxMinCheck
		{
			get
			{
				return chkManualMaxMin.Checked ;
			}
			set
			{
				chkManualMaxMin.Checked = value ;
			}
		}
		public UserControls.LineChart LineChart
		{
			get
			{
				return mLineChart;
			}
			set
			{
				mLineChart = value;
			}
		}

		public double MultiplicationValue
		{
			get
			{
				return double.Parse(numericMultiplicationValue.Value.ToString()) ;
			}
			set
			{
				numericMultiplicationValue.Value = Decimal.Parse(value.ToString()) ;
			}
		}

		public string LabelMultiplicationValue
		{
			get
			{
				return lblValue.Text ;
			}
			set
			{
				lblValue.Text = value ;
			}
		}

		public String DeviationText
		{
			get
			{
				return FunctionDev.Text;
			}
			set
			{
				FunctionDev.Text = value;
			}
		}

		#endregion

		#region event handlers
		private void TrendlineDlg_Load(object sender, System.EventArgs e)
		{		
			_trendlineInfo =  _dlgLineProfile.LoadTrendLineInfo();

			SetCurPicbox();
			SetPicImage(picCurent,true);	
			EnableControl(picCurent);

			SetDefaultValue();

			Decimal min,max;
			min = Convert.ToDecimal(_dlgLineProfile.Image.RasterImage.MINGRAYVALUE + 1 );
			min = Math.Min(Math.Max(min,numericMinNoiseThreshold.Value),Convert.ToDecimal(_dlgLineProfile.Image.MaxGreyValue));
			numericMinNoiseThreshold.Value = min ;

			max = Convert.ToDecimal(_dlgLineProfile.Image.MaxGreyValue);
			max = Math.Max(max,255);
			max = numericMaxNoiseThreshold.Value < numericMinNoiseThreshold.Value ? max:numericMaxNoiseThreshold.Value;
			max = max > Convert.ToDecimal(_dlgLineProfile.Image.MaxGreyValue) ? Convert.ToDecimal(_dlgLineProfile.Image.MaxGreyValue):max;
			numericMaxNoiseThreshold.Value = Math.Max(max,numericMaxNoiseThreshold.Minimum);

			numericKernel.Maximum = Convert.ToDecimal(_dlgLineProfile.Image.Width	);
			numericKernel.Value = numericKernel.Value > numericKernel.Maximum?  numericKernel.Maximum:numericKernel.Value ;

			this.picLinear.Click += new System.EventHandler(this.PictureBox_OnClick);
			this.picPolynomial.Click += new System.EventHandler(this.PictureBox_OnClick);
			this.picMovingAverage.Click += new System.EventHandler(this.PictureBox_OnClick);
			this.picPower.Click += new System.EventHandler(this.PictureBox_OnClick);
			this.picExponential.Click += new System.EventHandler(this.PictureBox_OnClick);
			this.picMovingAverage.Click += new System.EventHandler(this.PictureBox_OnClick);
			this.pictureBox1.Click += new System.EventHandler(this.PictureBox_OnClick);
			this.picMiddleAverage.Click += new System.EventHandler(this.PictureBox_OnClick);

			//////////////////////////////////////////////////////////////////////////
			ValueNU.Enabled = rdoValue.Checked ;
			txtStandarDeviation.Enabled = numericMultiplicationValue.Enabled = rdoStandarDeviation.Checked ;
			//updateLineChart();
			
		}

		private void label7_Click(object sender, System.EventArgs e)
		{
		
		}

		private void PictureBox_OnClick(object sender, System.EventArgs e)
		{			
			SetPicImage(picCurent,false);			
			SetPicImage(((PictureBox)sender),true);
		
			EnableControl((PictureBox)sender);

			picCurent = (PictureBox)sender;
			updateLineChart();
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			//			if (!Validate_MultiDeviationValue()&& rdoStandarDeviation.Checked)
			//				return;
			if (chkManualMaxMin.Checked)
				if(!CheckMaxMinValue())return;

			if ( nudOrder.Enabled )
				if ( !kUtils.IsInputValueValidate( nudOrder ) ) return;

			if ( nudPeriod.Enabled )
				if ( !kUtils.IsInputValueValidate( nudPeriod ) ) return;

			float  x;
			try
			{				
				x = float.Parse(numericMinNoiseThreshold.Text );		
				
			}
			catch
			{
				errorProvider1.SetError(numericMinNoiseThreshold, "Min Noise Threshold is not correct");
				MessageBoxEx.Error("Min Threshold is not empty");
				numericMinNoiseThreshold.Select();
				return ;
			}					

			try
			{
				x = float.Parse(numericMaxNoiseThreshold.Text );
			}
			catch
			{
				errorProvider1.SetError(numericMaxNoiseThreshold, "Max Noise Threshold is not correct");
				MessageBoxEx.Error("Max Threshold is not empty");
				numericMaxNoiseThreshold.Select();
				return ;
			}			
		
			_dlgLineProfile.UpdateTrendLineInfo();
			_dlgLineProfile.SaveTrendLineInfo();
			DialogResult = DialogResult.OK;			
			//			mLineChart.getTrendLine().addConstant(this.ConstantValue);
		}

		private void label9_Click(object sender, System.EventArgs e)
		{
		
		}

		private void TrendlineDlg_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.Control)
			{
				if(e.KeyCode == Keys.A)
				{
					//int i = 0;
					_trendlineInfo._Automatic = !_trendlineInfo._Automatic;
					if(_trendlineInfo._Type ==1)
						_trendlineInfo._Type = 0;
					else
						_trendlineInfo._Type = 1;
					updateLineChart();

				}
			}
		}

		private void TrendlineDlg_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
		
			if(e.KeyChar == 1)
			{
				//if(e.KeyChar == Keys.A)
			{

			}
			}
		}

		private void TrendlineDlg_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.Control)
			{
				if(e.KeyCode == Keys.A)
				{
				}
			}		
		}

		private void TrendlineDlg_Layout(object sender, System.Windows.Forms.LayoutEventArgs e)
		{
		
		}

		private void Heuristic_CheckedChanged(object sender, System.EventArgs e)
		{
			if(_bHeuristic.Checked)
			{
				foreach ( Control ctrl in groupBox1.Controls )
				{
					if ( ctrl.GetType() == typeof( PictureBox ) && ctrl.Tag != null)
					{
						if ( System.Convert.ToInt32( ctrl.Tag ) != 2 )
						{
							((PictureBox)ctrl).Image = imageListTrendLine.Images[14];
							ctrl.Enabled	= false;
						}
					}
				}
				if ( picCurent != null )
					if ( picCurent != picPolynomial )
					{
						picCurent = picPolynomial;
						SetPicImage(picCurent,true);
						EnableControl(picCurent);
					}
				_trendlineInfo._Automatic = true;
				updateLineChart();
			}
			else
			{
				foreach ( Control ctrl in groupBox1.Controls )
				{
					if ( ctrl.GetType() == typeof( PictureBox ) && ctrl.Tag  != null)
					{
						if ( System.Convert.ToInt32(  ctrl.Tag ) != 2)
						{
							ctrl.Enabled	= true;
							((PictureBox)ctrl).Image = imageListTrendLine.Images[System.Convert.ToInt32( ((PictureBox)ctrl).Tag)];
						}
						
					}
				}
				groupBox5.Enabled = true;
				groupBox2.Enabled = true;
				groupBox6.Enabled = true;
				_trendlineInfo._Automatic = false;
				updateLineChart();

			}
		}

		private void rdoAverage_CheckedChanged(object sender, System.EventArgs e)
		{
			if (rdoAverage.Checked)
				cRadioBtn = rdoAverage.Text ;
			else
				cRadioBtn = rdoMean.Text ;
			updateLineChart();
		}

		private void numericKernel_ValueChanged(object sender, System.EventArgs e)
		{
			updateLineChart();
		}

		private void numericMultiDeviation_ValueChanged(object sender, System.EventArgs e)
		{
			updateLineChart();
		}

		private void numericKernel_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			updateLineChart();
		}

		private void numericMultiDeviation_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = (Validate_MultiDeviationValue(true) == false);
			updateLineChart();
		}

		private void chkKernel_CheckedChanged(object sender, System.EventArgs e)
		{
			numericKernel.Enabled = chkKernel.Checked;
			numericMultiDeviation.Enabled = chkKernel.Checked;
			rdoAverage.Enabled = chkKernel.Checked;
			rdoMean.Enabled = chkKernel.Checked;

			updateLineChart();
		}

		private void rdoValue_CheckedChanged(object sender, System.EventArgs e)
		{
			if (numericMultiDeviation.Enabled == false)
				Validate_MultiDeviationValue(false);
		}

		private void rdoStandarDeviation_CheckedChanged(object sender, System.EventArgs e)
		{
			updateLineChart();
			ValueNU.Enabled = rdoValue.Checked ;
			numericMultiDeviation.Enabled = rdoStandarDeviation.Checked ;
			numericMultiplicationValue.Enabled = rdoStandarDeviation.Checked ;			
		}

		private void nudOrder_ValueChanged(object sender, System.EventArgs e)
		{
			updateLineChart();			
		}

		private void nudPeriod_ValueChanged(object sender, System.EventArgs e)
		{
			updateLineChart();
		
		}

		private void ValueNU_ValueChanged(object sender, System.EventArgs e)
		{
			updateLineChart();
		}

		private void numericMinNoiseThreshold_ValueChanged(object sender, System.EventArgs e)
		{
			updateLineChart();
		
		}

		private void numericMaxNoiseThreshold_ValueChanged(object sender, System.EventArgs e)
		{
			updateLineChart();
		
		}

		private void numericMultiplicationValue_ValueChanged(object sender, System.EventArgs e)
		{
			updateLineChart();
		}

		private void chkManualMaxMin_CheckedChanged(object sender, System.EventArgs e)
		{
			ShowManualMaxMin(chkManualMaxMin.Checked);
			updateLineChart();			
		}		
			
		

		#endregion

		#region internal routines

		private void SetCurPicbox()
		{
			switch ( _trendlineType.ToString() )
			{
				case "Linear":
					picCurent = picLinear;
					return;
				case "Logarithmic":
					picCurent = pictureBox1;
					return;
				case "Polynomial":
					picCurent = picPolynomial;
					return;
				case "Power":
					picCurent = picPower;
					return;
				case "Exponential":
					picCurent = picExponential;
					return;
				case "MiddleAverage":
					picCurent = picMiddleAverage;
					return ;
			}

			picCurent = picMovingAverage;
			
		}

		public void NumericUpDownAssign(NumericUpDown nud, Decimal value)
		{
			if (nud.Minimum > value)
				nud.Value = nud.Minimum;
			else if (nud.Maximum < value)
				nud.Value = nud.Maximum;
			else 
				nud.Value = value;
		}

		public void SetDefaultValue()
		{
			if (_trendlineInfo != null)
			{
				try
				{
					// enable initializing flag
					_bInitializingDefaultValue = true;

					this.rdoStandarDeviation.Checked = _trendlineInfo.Deviation_Checked;
					this.rdoValue.Checked = _trendlineInfo.Value_Checked;
					//this.numericKernel.Value = Convert.ToDecimal(_trendlineInfo.Kernel_Size);
					NumericUpDownAssign(numericKernel, Convert.ToDecimal(_trendlineInfo.Kernel_Size));
					NumericUpDownAssign(ValueNU, Convert.ToDecimal(_trendlineInfo.Constant_Value));
					NumericUpDownAssign(numericMultiDeviation, Convert.ToDecimal(_trendlineInfo.MultiDeviation));
					NumericUpDownAssign(nudOrder,Convert.ToDecimal( _trendlineInfo.Order_Value));
					NumericUpDownAssign(nudPeriod,Convert.ToDecimal(_trendlineInfo.Period_Value));
					if (_trendlineInfo.Kernel_Type == eKernelType.Average)
						rdoAverage.Checked = true;
					else
						if (_trendlineInfo.Kernel_Type == eKernelType.Mean)
							rdoMean.Checked = true;
					//nudOrder.Value = Convert.ToDecimal( _trendlineInfo.Order_Value);
					//nudPeriod.Value = Convert.ToDecimal(_trendlineInfo.Period_Value);
					chkManualMaxMin.Checked = _trendlineInfo.ManualMaxMin_Checked;
					numericMinNoiseThreshold.Value = Convert.ToDecimal(_trendlineInfo.MinNoiseThreshold);
					numericMaxNoiseThreshold.Value = Convert.ToDecimal(_trendlineInfo.MaxNoiseThreshold);
					SetPicImage(picCurent,false);
					_trendlineType = _trendlineInfo.Trenline_Type;
					SetCurPicbox();										
					SetPicImage(picCurent,true);	
					EnableControl(picCurent);

					// disable initializing flag
					_bInitializingDefaultValue = false;
					updateLineChart();

				}
				catch(System.Exception exp)
				{
					System.Diagnostics.Debug.WriteLine(exp.ToString());
				}
			}
		}

		private int GetImageIndex(PictureBox picBox,bool OnClicked )
		{
			if ( OnClicked ) return int.Parse(picBox.Tag.ToString()) + 7;
			return int.Parse(picBox.Tag.ToString());
		}
		
		public void DisableControl(bool AllControl)
		{
			if ( AllControl )
			{
				foreach ( Control ctrl in groupBox1.Controls )
				{
					if ( ctrl.GetType() == typeof( PictureBox ) && ctrl.Tag != null)
						((PictureBox)ctrl).Image = imageListTrendLine.Images[12];
					ctrl.Enabled	= false;
				}
				btnOK.Enabled	= false;
				btnCancel.Enabled	= true;

			}
			else 
			{
				picPower.Enabled		= false;
				picPower.Image = imageListTrendLine.Images[14];
				picExponential.Enabled	= false;
				picExponential.Image = imageListTrendLine.Images[14];
				
			}
		}

		private void EnableControl(PictureBox sender)
		{
			nudOrder.Enabled	= sender.Tag.ToString() == "2" ;
			nudPeriod.Enabled = false;
			if (sender.Tag.ToString() == "5" || sender.Tag.ToString() == "6" )
				nudPeriod.Enabled	= true ;

		}

		private void SetPicImage(PictureBox picBox,bool OnClicked)
		{	
			if ( picBox.Enabled == false ) return;
			picBox.Image = imageListTrendLine.Images[GetImageIndex(picBox,OnClicked)];
		}

		public bool CheckMaxMinValue()
		{
			if (numericMinNoiseThreshold.Value > numericMaxNoiseThreshold.Value)
			{
				errorProvider1.SetError(numericMaxNoiseThreshold, "Max Threshold must be greeter than Min Threshold");
				MessageBoxEx.Error("Max Threshold must be greeter than Min Threshold");
				return false ;
			}
			else
			{
				errorProvider1.SetError(numericMaxNoiseThreshold, "");
				return true ;
			}
			
		}

		private eKernelType GetKernelType()
		{
			
			switch (cRadioBtn)
			{
				case "Average":
					//return eTrendlineType.Polynomial;
					return eKernelType.Average;				
			}
			return eKernelType.Mean;
		}
		
		private eTrendlineType GetTrendlineType()
		{
			if(picCurent == null) return eTrendlineType.Linear;
			switch (picCurent.Tag.ToString())
			{
				case "0":
					//return eTrendlineType.Linear;
					return eTrendlineType.Linear;
				case "1":
					//return eTrendlineType.Logarithmic;
					return eTrendlineType.Logarithmic;
				case "2":
					//return eTrendlineType.Polynomial;
					return eTrendlineType.Polynomial;
				case "3":
					//return eTrendlineType.Power;
					return eTrendlineType.Power;
				case "4":
					//return eTrendlineType.Exponential;
					return eTrendlineType.Exponential;
				case "6":
					return eTrendlineType.MiddleAverage;
			}
			return eTrendlineType.MovingAverage;
		}

		private void ShowManualMaxMin(bool show)
		{
			numericMinNoiseThreshold.Enabled = show;
			numericMaxNoiseThreshold.Enabled = show;
		}
		
		public void updateLineChart()
		{
			if (mLineChart != null && this._bInitializingDefaultValue == false)
			{
				mLineChart.updateTrendLine();
				mLineChart.ThresholdVisible = true;
			}
		}

		public void UpdateTrendLineInfo(TrendLineFormat info)
		{
			info.Constant_Value = this.ConstantValue;
			info.Deviation = this.StandarDevitionValue;
			info.Deviation_Checked = this.StandarDevitionCheck;
			info.ManualMaxMin_Checked = this.ManualMaxMinCheck;
			info.MaxNoiseThreshold = this.MaxNoiseThresHold;
			info.MinNoiseThreshold = this.MinNoiseThreshold;
			info.Multiplication = this.MultiplicationValue;
			info.Order_Value = this.Order;
			info.Period_Value = this.Period;
			info.Trenline_Type = this.TrenlineType;
			info.Value_Checked = this.ValueCheck;
			info.Period_Value = this.Period;
			info.Kernel_Size = this.KernelSize;
			info.Kernel_Type = this.KernelType;
			info.MultiDeviation = this.MultipleDeviation;
			info.radio_Checked = cRadioBtn;
			info.Kernel_Checked = chkKernel.Checked;
			info.BmpCur = GetImageIndex(picCurent,true);
		}
		private bool Validate_MultiDeviationValue(bool bShowMessage)
		{
			const Decimal Min = (Decimal)0.0f;
			const Decimal Max = (Decimal)10.0f;
	
			bool bValid = (numericMultiDeviation.Value >= Min && numericMultiDeviation.Value <= Max);
			if (bValid == false ) 
			{				
				if (bShowMessage)
					MessageBoxEx.Error("Input value must be a decimal between 0.00 and 10.00");

				if (numericMultiDeviation.Value	< Min)
					numericMultiDeviation.Value = Min;
				else 
					numericMultiDeviation.Value = Max;
			}

			return bValid;
		}

		
		#endregion
	}
}
