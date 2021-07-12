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
	/// Summary description for DlgTrendline2.
	/// </summary>
	public class DlgTrendline2 : DialogBase
	{
		#region Windows Form members

		private System.Windows.Forms.GroupBox groupBox6;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.CheckBox chkManualMaxMin;
		private System.Windows.Forms.Label lblMinNoiseThreshold;
		private System.Windows.Forms.GroupBox groupBox1;
		public System.Windows.Forms.CheckBox _bHeuristic;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.PictureBox picMiddleAverage;
		private System.Windows.Forms.PictureBox picLinear;
		public System.Windows.Forms.NumericUpDown nudPeriod;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.NumericUpDown nudOrder;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.PictureBox picMovingAverage;
		private System.Windows.Forms.Label label5;
		public System.Windows.Forms.PictureBox picExponential;
		private System.Windows.Forms.Label label6;
		public System.Windows.Forms.PictureBox picPower;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.PictureBox picPolynomial;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label FunctionDev;
		private System.Windows.Forms.NumericUpDown numericMultiplicationValue;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ImageList images;
		private System.Windows.Forms.PictureBox picLogarithmic;

		#endregion

		#region Fields

		private PictureBox _selectedTrendlineType = null;
		private System.Windows.Forms.NumericUpDown nudKernelSize;
		private System.Windows.Forms.RadioButton radKernelMean;
		private System.Windows.Forms.RadioButton radKernelAvg;
		private System.Windows.Forms.RadioButton radConstDev;
		private System.Windows.Forms.RadioButton radConstVal;
		private System.Windows.Forms.NumericUpDown nudConstVal;
		private System.Windows.Forms.NumericUpDown nudConstDev;
		public System.Windows.Forms.NumericUpDown nudNoiseMin;
		public System.Windows.Forms.NumericUpDown nudNoiseMax;
		private TrendLineFormat _trendlineFormat = null;

		#endregion

		#region Properties

		public TrendLineFormat TrendLineFormat
		{
			get {return _trendlineFormat;}
		}

		public eTrendlineType TrendLineType
		{
			get
			{
				return (eTrendlineType)_selectedTrendlineType.Tag;
			}
			set
			{
				SetTrendLineType(value);
				OnTrendLineTypeChanged();
			}
		}

		protected virtual void OnTrendLineTypeChanged()
		{
		}

		#endregion

		#region Constructor and destructor
		
		internal DlgTrendline2()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		public DlgTrendline2(TrendLineFormat trendlineFormat)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			this._trendlineFormat = trendlineFormat;

			// update data
			this.UpdateData(false);
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgTrendline2));
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.label14 = new System.Windows.Forms.Label();
			this.nudNoiseMin = new System.Windows.Forms.NumericUpDown();
			this.chkManualMaxMin = new System.Windows.Forms.CheckBox();
			this.nudNoiseMax = new System.Windows.Forms.NumericUpDown();
			this.lblMinNoiseThreshold = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this._bHeuristic = new System.Windows.Forms.CheckBox();
			this.label11 = new System.Windows.Forms.Label();
			this.picMiddleAverage = new System.Windows.Forms.PictureBox();
			this.picLinear = new System.Windows.Forms.PictureBox();
			this.picLogarithmic = new System.Windows.Forms.PictureBox();
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
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.nudKernelSize = new System.Windows.Forms.NumericUpDown();
			this.radKernelMean = new System.Windows.Forms.RadioButton();
			this.radKernelAvg = new System.Windows.Forms.RadioButton();
			this.label12 = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.images = new System.Windows.Forms.ImageList(this.components);
			this.btnOK = new System.Windows.Forms.Button();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.label15 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label9 = new System.Windows.Forms.Label();
			this.radConstDev = new System.Windows.Forms.RadioButton();
			this.radConstVal = new System.Windows.Forms.RadioButton();
			this.nudConstVal = new System.Windows.Forms.NumericUpDown();
			this.nudConstDev = new System.Windows.Forms.NumericUpDown();
			this.FunctionDev = new System.Windows.Forms.Label();
			this.numericMultiplicationValue = new System.Windows.Forms.NumericUpDown();
			this.groupBox6.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudNoiseMin)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudNoiseMax)).BeginInit();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudPeriod)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudOrder)).BeginInit();
			this.groupBox5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudKernelSize)).BeginInit();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudConstVal)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudConstDev)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericMultiplicationValue)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox6
			// 
			this.groupBox6.Controls.Add(this.label14);
			this.groupBox6.Controls.Add(this.nudNoiseMin);
			this.groupBox6.Controls.Add(this.chkManualMaxMin);
			this.groupBox6.Controls.Add(this.nudNoiseMax);
			this.groupBox6.Controls.Add(this.lblMinNoiseThreshold);
			this.groupBox6.Location = new System.Drawing.Point(320, 188);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(201, 80);
			this.groupBox6.TabIndex = 25;
			this.groupBox6.TabStop = false;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(12, 51);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(92, 16);
			this.label14.TabIndex = 15;
			this.label14.Text = "Maximum:";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// nudNoiseMin
			// 
			this.nudNoiseMin.Location = new System.Drawing.Point(112, 23);
			this.nudNoiseMin.Maximum = new System.Decimal(new int[] {
																		1410065408,
																		2,
																		0,
																		0});
			this.nudNoiseMin.Name = "nudNoiseMin";
			this.nudNoiseMin.Size = new System.Drawing.Size(64, 20);
			this.nudNoiseMin.TabIndex = 5;
			this.nudNoiseMin.Value = new System.Decimal(new int[] {
																	  5,
																	  0,
																	  0,
																	  0});
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
			// nudNoiseMax
			// 
			this.nudNoiseMax.Location = new System.Drawing.Point(112, 47);
			this.nudNoiseMax.Maximum = new System.Decimal(new int[] {
																		1410065408,
																		2,
																		0,
																		0});
			this.nudNoiseMax.Minimum = new System.Decimal(new int[] {
																		1,
																		0,
																		0,
																		0});
			this.nudNoiseMax.Name = "nudNoiseMax";
			this.nudNoiseMax.Size = new System.Drawing.Size(64, 20);
			this.nudNoiseMax.TabIndex = 6;
			this.nudNoiseMax.Value = new System.Decimal(new int[] {
																	  1,
																	  0,
																	  0,
																	  0});
			// 
			// lblMinNoiseThreshold
			// 
			this.lblMinNoiseThreshold.Location = new System.Drawing.Point(12, 27);
			this.lblMinNoiseThreshold.Name = "lblMinNoiseThreshold";
			this.lblMinNoiseThreshold.Size = new System.Drawing.Size(92, 16);
			this.lblMinNoiseThreshold.TabIndex = 13;
			this.lblMinNoiseThreshold.Text = "Minimum:";
			this.lblMinNoiseThreshold.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this._bHeuristic);
			this.groupBox1.Controls.Add(this.label11);
			this.groupBox1.Controls.Add(this.picMiddleAverage);
			this.groupBox1.Controls.Add(this.picLinear);
			this.groupBox1.Controls.Add(this.picLogarithmic);
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
			this.groupBox1.Size = new System.Drawing.Size(308, 260);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Trend/Regression Type";
			// 
			// _bHeuristic
			// 
			this._bHeuristic.Location = new System.Drawing.Point(12, 228);
			this._bHeuristic.Name = "_bHeuristic";
			this._bHeuristic.Size = new System.Drawing.Size(104, 20);
			this._bHeuristic.TabIndex = 11;
			this._bHeuristic.Tag = "DEFECUL";
			this._bHeuristic.Text = "Automatic";
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(147, 162);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(88, 26);
			this.label11.TabIndex = 6;
			this.label11.Text = "Middle Point Moving Average";
			this.label11.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// picMiddleAverage
			// 
			this.picMiddleAverage.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.picMiddleAverage.Image = ((System.Drawing.Image)(resources.GetObject("picMiddleAverage.Image")));
			this.picMiddleAverage.Location = new System.Drawing.Point(164, 104);
			this.picMiddleAverage.Name = "picMiddleAverage";
			this.picMiddleAverage.Size = new System.Drawing.Size(54, 47);
			this.picMiddleAverage.TabIndex = 17;
			this.picMiddleAverage.TabStop = false;
			this.picMiddleAverage.Tag = "6";
			this.picMiddleAverage.Click += new System.EventHandler(this.picTrendLine_Clicked);
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
			this.picLinear.Click += new System.EventHandler(this.picTrendLine_Clicked);
			// 
			// picLogarithmic
			// 
			this.picLogarithmic.Image = ((System.Drawing.Image)(resources.GetObject("picLogarithmic.Image")));
			this.picLogarithmic.Location = new System.Drawing.Point(88, 20);
			this.picLogarithmic.Name = "picLogarithmic";
			this.picLogarithmic.Size = new System.Drawing.Size(54, 47);
			this.picLogarithmic.TabIndex = 16;
			this.picLogarithmic.TabStop = false;
			this.picLogarithmic.Tag = "1";
			this.picLogarithmic.Click += new System.EventHandler(this.picTrendLine_Clicked);
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
			this.nudPeriod.TabIndex = 10;
			this.nudPeriod.Value = new System.Decimal(new int[] {
																	2,
																	0,
																	0,
																	0});
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(136, 197);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(40, 20);
			this.label8.TabIndex = 9;
			this.label8.Text = "Period:";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
			this.nudOrder.TabIndex = 8;
			this.nudOrder.Value = new System.Decimal(new int[] {
																   2,
																   0,
																   0,
																   0});
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(12, 197);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(36, 20);
			this.label7.TabIndex = 7;
			this.label7.Text = "Order:";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(91, 162);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(48, 26);
			this.label4.TabIndex = 5;
			this.label4.Text = "Moving Average";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// picMovingAverage
			// 
			this.picMovingAverage.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.picMovingAverage.Image = ((System.Drawing.Image)(resources.GetObject("picMovingAverage.Image")));
			this.picMovingAverage.Location = new System.Drawing.Point(88, 104);
			this.picMovingAverage.Name = "picMovingAverage";
			this.picMovingAverage.Size = new System.Drawing.Size(54, 47);
			this.picMovingAverage.TabIndex = 10;
			this.picMovingAverage.TabStop = false;
			this.picMovingAverage.Tag = "5";
			this.picMovingAverage.Click += new System.EventHandler(this.picTrendLine_Clicked);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(7, 162);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(64, 16);
			this.label5.TabIndex = 4;
			this.label5.Text = "Exponential";
			// 
			// picExponential
			// 
			this.picExponential.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.picExponential.Image = ((System.Drawing.Image)(resources.GetObject("picExponential.Image")));
			this.picExponential.Location = new System.Drawing.Point(12, 104);
			this.picExponential.Name = "picExponential";
			this.picExponential.Size = new System.Drawing.Size(54, 47);
			this.picExponential.TabIndex = 8;
			this.picExponential.TabStop = false;
			this.picExponential.Tag = "4";
			this.picExponential.Click += new System.EventHandler(this.picTrendLine_Clicked);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(249, 78);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(36, 16);
			this.label6.TabIndex = 3;
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
			this.picPower.Click += new System.EventHandler(this.picTrendLine_Clicked);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(161, 78);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(60, 16);
			this.label3.TabIndex = 2;
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
			this.picPolynomial.Click += new System.EventHandler(this.picTrendLine_Clicked);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(84, 78);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(63, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "Logarithmic";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(21, 78);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(36, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Linear";
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.nudKernelSize);
			this.groupBox5.Controls.Add(this.radKernelMean);
			this.groupBox5.Controls.Add(this.radKernelAvg);
			this.groupBox5.Controls.Add(this.label12);
			this.groupBox5.Location = new System.Drawing.Point(320, 8);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(201, 72);
			this.groupBox5.TabIndex = 1;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Kernel";
			// 
			// nudKernelSize
			// 
			this.nudKernelSize.Location = new System.Drawing.Point(88, 21);
			this.nudKernelSize.Maximum = new System.Decimal(new int[] {
																		  2048,
																		  0,
																		  0,
																		  0});
			this.nudKernelSize.Minimum = new System.Decimal(new int[] {
																		  1,
																		  0,
																		  0,
																		  0});
			this.nudKernelSize.Name = "nudKernelSize";
			this.nudKernelSize.Size = new System.Drawing.Size(88, 20);
			this.nudKernelSize.TabIndex = 1;
			this.nudKernelSize.Value = new System.Decimal(new int[] {
																		20,
																		0,
																		0,
																		0});
			// 
			// radKernelMean
			// 
			this.radKernelMean.Location = new System.Drawing.Point(98, 48);
			this.radKernelMean.Name = "radKernelMean";
			this.radKernelMean.Size = new System.Drawing.Size(83, 19);
			this.radKernelMean.TabIndex = 3;
			this.radKernelMean.Text = "Mean";
			// 
			// radKernelAvg
			// 
			this.radKernelAvg.Checked = true;
			this.radKernelAvg.Location = new System.Drawing.Point(12, 48);
			this.radKernelAvg.Name = "radKernelAvg";
			this.radKernelAvg.Size = new System.Drawing.Size(83, 17);
			this.radKernelAvg.TabIndex = 2;
			this.radKernelAvg.TabStop = true;
			this.radKernelAvg.Text = "Average";
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(8, 24);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(75, 16);
			this.label12.TabIndex = 0;
			this.label12.Text = "Kernel Size:";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(276, 308);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 31;
			this.btnCancel.Text = "Cancel";
			// 
			// images
			// 
			this.images.ImageSize = new System.Drawing.Size(54, 47);
			this.images.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("images.ImageStream")));
			this.images.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(180, 308);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 30;
			this.btnOK.Text = "OK";
			// 
			// groupBox4
			// 
			this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox4.Location = new System.Drawing.Point(2, 296);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(524, 4);
			this.groupBox4.TabIndex = 29;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "groupBox4";
			// 
			// label15
			// 
			this.label15.AutoSize = true;
			this.label15.Location = new System.Drawing.Point(8, 276);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(147, 16);
			this.label15.TabIndex = 27;
			this.label15.Text = "Function Standard Deviation";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label9);
			this.groupBox2.Controls.Add(this.radConstDev);
			this.groupBox2.Controls.Add(this.radConstVal);
			this.groupBox2.Controls.Add(this.nudConstVal);
			this.groupBox2.Controls.Add(this.nudConstDev);
			this.groupBox2.Location = new System.Drawing.Point(320, 88);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(201, 96);
			this.groupBox2.TabIndex = 4;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Constant";
			// 
			// label9
			// 
			this.label9.BackColor = System.Drawing.Color.Transparent;
			this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(163)));
			this.label9.Location = new System.Drawing.Point(98, 76);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(88, 16);
			this.label9.TabIndex = 4;
			this.label9.Text = "(kernel std . dev)";
			// 
			// radConstDev
			// 
			this.radConstDev.CausesValidation = false;
			this.radConstDev.Location = new System.Drawing.Point(8, 48);
			this.radConstDev.Name = "radConstDev";
			this.radConstDev.Size = new System.Drawing.Size(75, 24);
			this.radConstDev.TabIndex = 2;
			this.radConstDev.Text = "Deviation:";
			// 
			// radConstVal
			// 
			this.radConstVal.CausesValidation = false;
			this.radConstVal.Checked = true;
			this.radConstVal.Location = new System.Drawing.Point(8, 20);
			this.radConstVal.Name = "radConstVal";
			this.radConstVal.Size = new System.Drawing.Size(75, 24);
			this.radConstVal.TabIndex = 0;
			this.radConstVal.TabStop = true;
			this.radConstVal.Text = "Value:";
			// 
			// nudConstVal
			// 
			this.nudConstVal.DecimalPlaces = 2;
			this.nudConstVal.Location = new System.Drawing.Point(88, 24);
			this.nudConstVal.Maximum = new System.Decimal(new int[] {
																		65535,
																		0,
																		0,
																		0});
			this.nudConstVal.Minimum = new System.Decimal(new int[] {
																		65535,
																		0,
																		0,
																		-2147483648});
			this.nudConstVal.Name = "nudConstVal";
			this.nudConstVal.Size = new System.Drawing.Size(88, 20);
			this.nudConstVal.TabIndex = 1;
			this.nudConstVal.Value = new System.Decimal(new int[] {
																	  3,
																	  0,
																	  0,
																	  0});
			// 
			// nudConstDev
			// 
			this.nudConstDev.DecimalPlaces = 2;
			this.nudConstDev.Enabled = false;
			this.nudConstDev.Location = new System.Drawing.Point(88, 52);
			this.nudConstDev.Maximum = new System.Decimal(new int[] {
																		65535,
																		0,
																		0,
																		0});
			this.nudConstDev.Minimum = new System.Decimal(new int[] {
																		65535,
																		0,
																		0,
																		-2147483648});
			this.nudConstDev.Name = "nudConstDev";
			this.nudConstDev.Size = new System.Drawing.Size(88, 20);
			this.nudConstDev.TabIndex = 3;
			// 
			// FunctionDev
			// 
			this.FunctionDev.Location = new System.Drawing.Point(156, 276);
			this.FunctionDev.Name = "FunctionDev";
			this.FunctionDev.Size = new System.Drawing.Size(136, 16);
			this.FunctionDev.TabIndex = 28;
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
			this.numericMultiplicationValue.TabIndex = 5;
			this.numericMultiplicationValue.Value = new System.Decimal(new int[] {
																					 15,
																					 0,
																					 0,
																					 65536});
			this.numericMultiplicationValue.Visible = false;
			// 
			// DlgTrendline2
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(528, 336);
			this.Controls.Add(this.groupBox5);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.label15);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.FunctionDev);
			this.Controls.Add(this.numericMultiplicationValue);
			this.Controls.Add(this.groupBox6);
			this.Controls.Add(this.groupBox1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DlgTrendline2";
			this.Text = "Trend Line Settings";
			this.groupBox6.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.nudNoiseMin)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudNoiseMax)).EndInit();
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.nudPeriod)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudOrder)).EndInit();
			this.groupBox5.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.nudKernelSize)).EndInit();
			this.groupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.nudConstVal)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudConstDev)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericMultiplicationValue)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region Event handlers

		private void picTrendLine_Clicked(object sender, System.EventArgs e)
		{
			PictureBox[] ctrls = new PictureBox[]
				{
					picLinear, picLogarithmic, picPolynomial, picPower, picExponential, picMovingAverage, picMiddleAverage
				};

			// update image of all picture boxes
			for (int i=0; i<ctrls.Length; i++)
			{
				// set active trend line image
				if (ctrls[i] == sender)
					ctrls[i].Image = images.Images[i+ctrls.Length];
				else
					ctrls[i].Image = images.Images[i];
				ctrls[i].Tag = (eTrendlineType)i;
			}

			// set active trend line type
			this._selectedTrendlineType = sender as PictureBox;			
		}

		private void chkManualMaxMin_CheckedChanged(object sender, System.EventArgs e)
		{
			nudNoiseMax.Enabled = nudNoiseMin.Enabled = chkManualMaxMin.Checked;
		}

		#endregion

		#region Internal Helpers
	
		private void UpdateData(bool saveAndValidate)
		{
			if (saveAndValidate)
			{
				_trendlineFormat.Trenline_Type = this.TrendLineType;
				_trendlineFormat.Order_Value = Convert.ToDouble(nudOrder.Value);
				_trendlineFormat.Period_Value = Convert.ToDouble(nudPeriod.Value);

				_trendlineFormat.Kernel_Checked = true;
				_trendlineFormat.Kernel_Size = Convert.ToDouble(nudKernelSize.Value);
				if (radKernelAvg.Checked)
					_trendlineFormat.Kernel_Type = eKernelType.Average;
				else
					_trendlineFormat.Kernel_Type = eKernelType.Mean;
			}
			else
			{	
				this.SetTrendLineType(_trendlineFormat.Trenline_Type);

				nudOrder.BeginInit();
				nudOrder.Value = Convert.ToDecimal(_trendlineFormat.Order_Value);
				nudOrder.EndInit();
				
				nudPeriod.BeginInit();
				nudPeriod.Value = Convert.ToDecimal(_trendlineFormat.Period_Value);
				nudPeriod.EndInit();
				
				nudKernelSize.Value = Convert.ToDecimal(_trendlineFormat.Kernel_Size);
				radKernelAvg.Checked = (eKernelType.Average == _trendlineFormat.Kernel_Type);
				radKernelMean.Checked = (eKernelType.Mean == _trendlineFormat.Kernel_Type);
				
				radConstVal.Checked = _trendlineFormat.Value_Checked;
				nudConstVal.Enabled = radConstVal.Checked;

				nudConstVal.BeginInit();
				nudConstVal.Value = Convert.ToDecimal(_trendlineFormat.Constant_Value);
				nudConstVal.EndInit();
				
				radConstDev.Checked = _trendlineFormat.Deviation_Checked;
				nudConstDev.Enabled = radConstDev.Checked;

				nudConstDev.BeginInit();
				nudConstDev.Value = Convert.ToDecimal(_trendlineFormat.Deviation);
				nudConstDev.EndInit();

				chkManualMaxMin.Checked = _trendlineFormat.ManualMaxMin_Checked;
				nudNoiseMax.Enabled = nudNoiseMin.Enabled = chkManualMaxMin.Checked;

				nudNoiseMin.BeginInit();
				nudNoiseMin.Value = Convert.ToDecimal(_trendlineFormat.MinNoiseThreshold);
				nudNoiseMin.EndInit();
				
				nudNoiseMax.BeginInit();
				nudNoiseMax.Value = Convert.ToDecimal(_trendlineFormat.MaxNoiseThreshold);
				nudNoiseMax.EndInit();
			}

		}
		
		private void SetTrendLineType(eTrendlineType type)
		{
			PictureBox[] ctrls = new PictureBox[]
				{
					picLinear, picLogarithmic, picPolynomial, picPower, picExponential, picMovingAverage, picMiddleAverage
				};

			// update image of all picture boxes
			for (int i=0; i<ctrls.Length; i++)
			{
				// set active trend line image
				if (i == (int)type)
				{
					ctrls[i].Image = images.Images[i+ctrls.Length];
					// set active trend line type
					this._selectedTrendlineType = ctrls[i];
				}
				else
					ctrls[i].Image = images.Images[i];
				ctrls[i].Tag = (eTrendlineType)i;
			}
		}

		
		
		#endregion
	}
}
