using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

using SIA.Common;
using SIA.Common.Analysis;
using SIA.Common.KlarfExport;
using SIA.Common.KlarfExport.BinningLibrary;
using SIA.Common.Mathematics;
using SIA.Common.Utility;

using SIA.SystemLayer;

using SIA.UI.Controls.Utilities;

namespace SIA.UI.Controls.Dialogs
{
	public enum ViewTrendlineType
	{
		Trendline,
		BackGroundTrendline
	}

	/// <summary>
	/// Name : DlgTrendlineFormat
	/// Description : User interface for Trend Line Format
	/// Thread Support : True
	/// Persistence Data : True
	/// </summary>
	public class DlgTrendlineFormat : DialogBase
	{
		#region Windows Form Members
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.NumericUpDown nudOrder;
		private System.Windows.Forms.Label label8;

		private System.Windows.Forms.PictureBox picCurent ;
		public System.Windows.Forms.NumericUpDown nudPeriod;
		private System.Windows.Forms.ImageList imageListTrendLine;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.NumericUpDown ValueNU;
		private System.Windows.Forms.Label lblMinNoiseThreshold;
		public System.Windows.Forms.NumericUpDown numericMinNoiseThreshold;
		public System.Windows.Forms.NumericUpDown numericMaxNoiseThreshold;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.CheckBox chkManualMaxMin;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private UserControls.LineChart _lineChart;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.TextBox txtStandarDeviation;
		private System.Windows.Forms.NumericUpDown numericMultiplicationValue;
		private System.Windows.Forms.RadioButton rdoStandarDeviation;
		private System.Windows.Forms.RadioButton rdoValue;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.NumericUpDown numericKernel;
		private System.Windows.Forms.RadioButton rdoMean;
		private System.Windows.Forms.RadioButton rdoAverage;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.CheckBox chkKernel;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.NumericUpDown numericMultiDeviation;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.GroupBox groupBox6;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.PictureBox picPolynomial;
		private System.Windows.Forms.PictureBox picMiddleAverage;
		
		#endregion

		#region member attributes

		private CommonImage _image = null;
		public ViewTrendlineType _viewType = ViewTrendlineType.Trendline;
		private eKernelType _kernelType;
		private eTrendlineType _trendLineType;
		private TrendLineFormat _trendlineFormat;

		#endregion

		#region public properties

		public eKernelType KernelType
		{
			get { return GetKernelType();}
			set {_kernelType = value;}
		}
		public int Order
		{
			get {return (int)nudOrder.Value; }
		}

		public int Period
		{
			get {return (int)nudPeriod.Value; } 
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
				return _lineChart;
			}
			set
			{
				_lineChart = value;
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

		public eTrendlineType TrenlineType
		{
			get { return GetTrendlineType();}
			set {_trendLineType = value;}
		}

		#endregion

		#region constructor and destructor

		public DlgTrendlineFormat()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			_trendlineFormat = new TrendLineFormat();
			_kernelType = eKernelType.Average;
		}
		
		public DlgTrendlineFormat(TrendLineFormat format, CommonImage image)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			_trendlineFormat = format;
			_viewType = ViewTrendlineType.BackGroundTrendline;
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

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgTrendlineFormat));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.picMiddleAverage = new System.Windows.Forms.PictureBox();
			this.picPolynomial = new System.Windows.Forms.PictureBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.label11 = new System.Windows.Forms.Label();
			this.nudPeriod = new System.Windows.Forms.NumericUpDown();
			this.label8 = new System.Windows.Forms.Label();
			this.nudOrder = new System.Windows.Forms.NumericUpDown();
			this.label7 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.imageListTrendLine = new System.Windows.Forms.ImageList(this.components);
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.rdoValue = new System.Windows.Forms.RadioButton();
			this.rdoStandarDeviation = new System.Windows.Forms.RadioButton();
			this.ValueNU = new System.Windows.Forms.NumericUpDown();
			this.numericMultiDeviation = new System.Windows.Forms.NumericUpDown();
			this.label9 = new System.Windows.Forms.Label();
			this.chkManualMaxMin = new System.Windows.Forms.CheckBox();
			this.label14 = new System.Windows.Forms.Label();
			this.numericMinNoiseThreshold = new System.Windows.Forms.NumericUpDown();
			this.lblMinNoiseThreshold = new System.Windows.Forms.Label();
			this.numericMaxNoiseThreshold = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.numericMultiplicationValue = new System.Windows.Forms.NumericUpDown();
			this.txtStandarDeviation = new System.Windows.Forms.TextBox();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.numericKernel = new System.Windows.Forms.NumericUpDown();
			this.rdoMean = new System.Windows.Forms.RadioButton();
			this.rdoAverage = new System.Windows.Forms.RadioButton();
			this.label12 = new System.Windows.Forms.Label();
			this.chkKernel = new System.Windows.Forms.CheckBox();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudPeriod)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudOrder)).BeginInit();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ValueNU)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericMultiDeviation)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericMinNoiseThreshold)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericMaxNoiseThreshold)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericMultiplicationValue)).BeginInit();
			this.groupBox5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericKernel)).BeginInit();
			this.groupBox6.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.picMiddleAverage);
			this.groupBox1.Controls.Add(this.picPolynomial);
			this.groupBox1.Controls.Add(this.groupBox4);
			this.groupBox1.Controls.Add(this.label11);
			this.groupBox1.Controls.Add(this.nudPeriod);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.nudOrder);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Location = new System.Drawing.Point(5, 4);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(175, 240);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Trend/Regression Type";
			// 
			// picMiddleAverage
			// 
			this.picMiddleAverage.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.picMiddleAverage.Image = ((System.Drawing.Image)(resources.GetObject("picMiddleAverage.Image")));
			this.picMiddleAverage.Location = new System.Drawing.Point(16, 144);
			this.picMiddleAverage.Name = "picMiddleAverage";
			this.picMiddleAverage.Size = new System.Drawing.Size(54, 47);
			this.picMiddleAverage.TabIndex = 1803;
			this.picMiddleAverage.TabStop = false;
			this.picMiddleAverage.Tag = "6";
			// 
			// picPolynomial
			// 
			this.picPolynomial.Image = ((System.Drawing.Image)(resources.GetObject("picPolynomial.Image")));
			this.picPolynomial.Location = new System.Drawing.Point(16, 28);
			this.picPolynomial.Name = "picPolynomial";
			this.picPolynomial.Size = new System.Drawing.Size(54, 47);
			this.picPolynomial.TabIndex = 1802;
			this.picPolynomial.TabStop = false;
			this.picPolynomial.Tag = "2";
			// 
			// groupBox4
			// 
			this.groupBox4.Location = new System.Drawing.Point(-44, 112);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(240, 8);
			this.groupBox4.TabIndex = 1801;
			this.groupBox4.TabStop = false;
			// 
			// label11
			// 
			this.label11.BackColor = System.Drawing.Color.Transparent;
			this.label11.Location = new System.Drawing.Point(16, 196);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(92, 28);
			this.label11.TabIndex = 3;
			this.label11.Text = "Middle Point Moving  Average";
			// 
			// nudPeriod
			// 
			this.nudPeriod.Location = new System.Drawing.Point(108, 168);
			this.nudPeriod.Maximum = new System.Decimal(new int[] {
																	  2048,
																	  0,
																	  0,
																	  0});
			this.nudPeriod.Minimum = new System.Decimal(new int[] {
																	  1,
																	  0,
																	  0,
																	  0});
			this.nudPeriod.Name = "nudPeriod";
			this.nudPeriod.Size = new System.Drawing.Size(56, 20);
			this.nudPeriod.TabIndex = 5;
			this.nudPeriod.Value = new System.Decimal(new int[] {
																	1,
																	0,
																	0,
																	0});
			this.nudPeriod.ValueChanged += new System.EventHandler(this.nudPeriod_ValueChanged);
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(108, 148);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(40, 16);
			this.label8.TabIndex = 4;
			this.label8.Text = "Period:";
			// 
			// nudOrder
			// 
			this.nudOrder.Location = new System.Drawing.Point(108, 56);
			this.nudOrder.Maximum = new System.Decimal(new int[] {
																	 10,
																	 0,
																	 0,
																	 0});
			this.nudOrder.Minimum = new System.Decimal(new int[] {
																	 1,
																	 0,
																	 0,
																	 0});
			this.nudOrder.Name = "nudOrder";
			this.nudOrder.Size = new System.Drawing.Size(56, 20);
			this.nudOrder.TabIndex = 2;
			this.nudOrder.Value = new System.Decimal(new int[] {
																   1,
																   0,
																   0,
																   0});
			this.nudOrder.ValueChanged += new System.EventHandler(this.nudOrder_ValueChanged);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(108, 36);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(36, 16);
			this.label7.TabIndex = 1;
			this.label7.Text = "Order:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(16, 84);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(60, 16);
			this.label3.TabIndex = 0;
			this.label3.Text = "Polynomial";
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnOK.Location = new System.Drawing.Point(119, 254);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 8;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(203, 254);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 9;
			this.btnCancel.Text = "Cancel";
			// 
			// imageListTrendLine
			// 
			this.imageListTrendLine.ImageSize = new System.Drawing.Size(54, 47);
			this.imageListTrendLine.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTrendLine.ImageStream")));
			this.imageListTrendLine.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.rdoValue);
			this.groupBox2.Controls.Add(this.rdoStandarDeviation);
			this.groupBox2.Controls.Add(this.ValueNU);
			this.groupBox2.Controls.Add(this.numericMultiDeviation);
			this.groupBox2.Controls.Add(this.label9);
			this.groupBox2.Location = new System.Drawing.Point(184, 76);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(200, 84);
			this.groupBox2.TabIndex = 6;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Constants";
			// 
			// rdoValue
			// 
			this.rdoValue.Checked = true;
			this.rdoValue.Location = new System.Drawing.Point(25, 16);
			this.rdoValue.Name = "rdoValue";
			this.rdoValue.Size = new System.Drawing.Size(76, 20);
			this.rdoValue.TabIndex = 0;
			this.rdoValue.TabStop = true;
			this.rdoValue.Text = "Value:";
			// 
			// rdoStandarDeviation
			// 
			this.rdoStandarDeviation.Location = new System.Drawing.Point(25, 40);
			this.rdoStandarDeviation.Name = "rdoStandarDeviation";
			this.rdoStandarDeviation.Size = new System.Drawing.Size(76, 20);
			this.rdoStandarDeviation.TabIndex = 2;
			this.rdoStandarDeviation.Text = "Deviation:";
			this.rdoStandarDeviation.CheckedChanged += new System.EventHandler(this.rdoStandarDeviation_CheckedChanged);
			// 
			// ValueNU
			// 
			this.ValueNU.DecimalPlaces = 2;
			this.ValueNU.Location = new System.Drawing.Point(112, 16);
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
			this.ValueNU.TabIndex = 1;
			this.ValueNU.Value = new System.Decimal(new int[] {
																  3,
																  0,
																  0,
																  0});
			this.ValueNU.ValueChanged += new System.EventHandler(this.ValueNU_ValueChanged);
			// 
			// numericMultiDeviation
			// 
			this.numericMultiDeviation.DecimalPlaces = 2;
			this.numericMultiDeviation.Enabled = false;
			this.numericMultiDeviation.Location = new System.Drawing.Point(112, 40);
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
			// label9
			// 
			this.label9.BackColor = System.Drawing.Color.Transparent;
			this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(163)));
			this.label9.Location = new System.Drawing.Point(96, 64);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(96, 16);
			this.label9.TabIndex = 3;
			this.label9.Text = "(kernel std . dev.)";
			// 
			// chkManualMaxMin
			// 
			this.chkManualMaxMin.Checked = true;
			this.chkManualMaxMin.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkManualMaxMin.Location = new System.Drawing.Point(8, -5);
			this.chkManualMaxMin.Name = "chkManualMaxMin";
			this.chkManualMaxMin.Size = new System.Drawing.Size(108, 24);
			this.chkManualMaxMin.TabIndex = 1;
			this.chkManualMaxMin.Text = "Noise Threshold";
			this.chkManualMaxMin.CheckedChanged += new System.EventHandler(this.chkManualMaxMin_CheckedChanged);
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(25, 52);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(75, 20);
			this.label14.TabIndex = 4;
			this.label14.Text = "Max:";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// numericMinNoiseThreshold
			// 
			this.numericMinNoiseThreshold.Location = new System.Drawing.Point(112, 24);
			this.numericMinNoiseThreshold.Maximum = new System.Decimal(new int[] {
																					 1410065408,
																					 2,
																					 0,
																					 0});
			this.numericMinNoiseThreshold.Name = "numericMinNoiseThreshold";
			this.numericMinNoiseThreshold.Size = new System.Drawing.Size(64, 20);
			this.numericMinNoiseThreshold.TabIndex = 3;
			this.numericMinNoiseThreshold.Value = new System.Decimal(new int[] {
																				   7,
																				   0,
																				   0,
																				   0});
			this.numericMinNoiseThreshold.ValueChanged += new System.EventHandler(this.numericMinNoiseThreshold_ValueChanged);
			// 
			// lblMinNoiseThreshold
			// 
			this.lblMinNoiseThreshold.Location = new System.Drawing.Point(25, 24);
			this.lblMinNoiseThreshold.Name = "lblMinNoiseThreshold";
			this.lblMinNoiseThreshold.Size = new System.Drawing.Size(75, 20);
			this.lblMinNoiseThreshold.TabIndex = 2;
			this.lblMinNoiseThreshold.Text = "Min:";
			this.lblMinNoiseThreshold.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// numericMaxNoiseThreshold
			// 
			this.numericMaxNoiseThreshold.Location = new System.Drawing.Point(112, 52);
			this.numericMaxNoiseThreshold.Maximum = new System.Decimal(new int[] {
																					 1410065408,
																					 2,
																					 0,
																					 0});
			this.numericMaxNoiseThreshold.Name = "numericMaxNoiseThreshold";
			this.numericMaxNoiseThreshold.Size = new System.Drawing.Size(64, 20);
			this.numericMaxNoiseThreshold.TabIndex = 0;
			this.numericMaxNoiseThreshold.Value = new System.Decimal(new int[] {
																				   3,
																				   0,
																				   0,
																				   0});
			this.numericMaxNoiseThreshold.ValueChanged += new System.EventHandler(this.numericMaxNoiseThreshold_ValueChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(12, 436);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(14, 18);
			this.label1.TabIndex = 30;
			this.label1.Text = "X";
			// 
			// numericMultiplicationValue
			// 
			this.numericMultiplicationValue.DecimalPlaces = 2;
			this.numericMultiplicationValue.Enabled = false;
			this.numericMultiplicationValue.Increment = new System.Decimal(new int[] {
																						 5,
																						 0,
																						 0,
																						 65536});
			this.numericMultiplicationValue.Location = new System.Drawing.Point(28, 468);
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
																					 1,
																					 0,
																					 0,
																					 0});
			// 
			// txtStandarDeviation
			// 
			this.txtStandarDeviation.AutoSize = false;
			this.txtStandarDeviation.Location = new System.Drawing.Point(24, 444);
			this.txtStandarDeviation.Name = "txtStandarDeviation";
			this.txtStandarDeviation.ReadOnly = true;
			this.txtStandarDeviation.Size = new System.Drawing.Size(88, 20);
			this.txtStandarDeviation.TabIndex = 24;
			this.txtStandarDeviation.Text = "0";
			this.txtStandarDeviation.Visible = false;
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
			this.groupBox5.Location = new System.Drawing.Point(184, 4);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(200, 68);
			this.groupBox5.TabIndex = 3;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Kernel Properties";
			// 
			// numericKernel
			// 
			this.numericKernel.Location = new System.Drawing.Point(112, 20);
			this.numericKernel.Minimum = new System.Decimal(new int[] {
																		  1,
																		  0,
																		  0,
																		  0});
			this.numericKernel.Name = "numericKernel";
			this.numericKernel.Size = new System.Drawing.Size(64, 20);
			this.numericKernel.TabIndex = 2;
			this.numericKernel.Value = new System.Decimal(new int[] {
																		21,
																		0,
																		0,
																		0});
			// 
			// rdoMean
			// 
			this.rdoMean.Location = new System.Drawing.Point(104, 44);
			this.rdoMean.Name = "rdoMean";
			this.rdoMean.Size = new System.Drawing.Size(71, 20);
			this.rdoMean.TabIndex = 4;
			this.rdoMean.Text = "Mean";
			// 
			// rdoAverage
			// 
			this.rdoAverage.Checked = true;
			this.rdoAverage.Location = new System.Drawing.Point(25, 44);
			this.rdoAverage.Name = "rdoAverage";
			this.rdoAverage.Size = new System.Drawing.Size(71, 20);
			this.rdoAverage.TabIndex = 3;
			this.rdoAverage.TabStop = true;
			this.rdoAverage.Text = "Average";
			this.rdoAverage.CheckedChanged += new System.EventHandler(this.rdoAverage_CheckedChanged);
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(25, 20);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(79, 20);
			this.label12.TabIndex = 1;
			this.label12.Text = "Kernel Size:";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// chkKernel
			// 
			this.chkKernel.Checked = true;
			this.chkKernel.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkKernel.Location = new System.Drawing.Point(8, -5);
			this.chkKernel.Name = "chkKernel";
			this.chkKernel.Size = new System.Drawing.Size(120, 24);
			this.chkKernel.TabIndex = 0;
			this.chkKernel.Text = "Kernel Property";
			this.chkKernel.Visible = false;
			// 
			// groupBox6
			// 
			this.groupBox6.Controls.Add(this.numericMaxNoiseThreshold);
			this.groupBox6.Controls.Add(this.lblMinNoiseThreshold);
			this.groupBox6.Controls.Add(this.numericMinNoiseThreshold);
			this.groupBox6.Controls.Add(this.label14);
			this.groupBox6.Controls.Add(this.chkManualMaxMin);
			this.groupBox6.Location = new System.Drawing.Point(184, 164);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(200, 80);
			this.groupBox6.TabIndex = 7;
			this.groupBox6.TabStop = false;
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox3.Location = new System.Drawing.Point(-239, 244);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(868, 8);
			this.groupBox3.TabIndex = 32;
			this.groupBox3.TabStop = false;
			// 
			// DlgTrendlineFormat
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(390, 280);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox6);
			this.Controls.Add(this.groupBox5);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtStandarDeviation);
			this.Controls.Add(this.numericMultiplicationValue);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DlgTrendlineFormat";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Trendline Format";
			this.Load += new System.EventHandler(this.DlgTrendlineFormat_Load);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.nudPeriod)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudOrder)).EndInit();
			this.groupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.ValueNU)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericMultiDeviation)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericMinNoiseThreshold)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericMaxNoiseThreshold)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericMultiplicationValue)).EndInit();
			this.groupBox5.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numericKernel)).EndInit();
			this.groupBox6.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region event handlers
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			LoadDataTrenLineFormat();
			this.Icon = new Icon(this.GetType(),"Icon.icon.ico");
			
			SetCurPicbox();
			SetPicImage(picCurent,true);	
			EnableControl(picCurent);

			//		Decimal min,max;
			if (_image != null)
			{
				numericMinNoiseThreshold.Minimum = Convert.ToDecimal(_image.RasterImage.MINGRAYVALUE+1);
				numericMinNoiseThreshold.Maximum = Convert.ToDecimal(_image.RasterImage.MAXGRAYVALUE);
				numericMaxNoiseThreshold.Minimum = Convert.ToDecimal(_image.RasterImage.MINGRAYVALUE+1);
				numericMaxNoiseThreshold.Maximum = Convert.ToDecimal(_image.RasterImage.MAXGRAYVALUE);
			}
								
			Decimal minVal, maxVal;
			minVal = Convert.ToDecimal(CustomConfiguration.GetValues("MINTHRESHOLD",1));
			maxVal = Convert.ToDecimal(CustomConfiguration.GetValues("MAXTHRESHOLD",65535));

			minVal = Math.Min(Math.Max(numericMinNoiseThreshold.Value, numericMinNoiseThreshold.Minimum), numericMinNoiseThreshold.Maximum);
			maxVal = Math.Max(Math.Min(numericMaxNoiseThreshold.Value, numericMaxNoiseThreshold.Maximum), numericMaxNoiseThreshold.Minimum);
		
			
			numericMinNoiseThreshold.Value = minVal;
			numericMaxNoiseThreshold.Value = maxVal;


			this.picPolynomial.Click += new System.EventHandler(this.PictureBox_OnClick);
			this.picMiddleAverage.Click += new System.EventHandler(this.PictureBox_OnClick);
			//
			//			//////////////////////////////////////////////////////////////////////////
			ValueNU.Enabled = rdoValue.Checked ;
		}


		private void DlgTrendlineFormat_Load(object sender, System.EventArgs e)
		{		
			
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			if(!Validate_MultiDeviationValue() && rdoStandarDeviation.Checked)
			{
				numericMultiDeviation.Focus();
				numericMultiDeviation.Select(0, numericMultiDeviation.Text.Length);
				return;
			}
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

			DialogResult = DialogResult.OK;			

			AddData2TrendLineFormat();			

			if (_viewType == ViewTrendlineType.Trendline )
				_trendlineFormat.Save();
			else 
				_trendlineFormat.SaveBackGroundTrendline();
		}

		
		private void PictureBox_OnClick(object sender, System.EventArgs e)
		{			
			SetPicImage(picCurent,false);			
			SetPicImage(((PictureBox)sender),true);
		
			EnableControl((PictureBox)sender);

			picCurent = (PictureBox)sender;
			updateLineChart();
		}

		private void chkManualMaxMin_CheckedChanged(object sender, System.EventArgs e)
		{
			ShowManualMaxMin(chkManualMaxMin.Checked);
			updateLineChart();
		}		
		private void rdoStandarDeviation_CheckedChanged(object sender, System.EventArgs e)
		{
			updateLineChart();
			ValueNU.Enabled = rdoValue.Checked ;
			numericMultiplicationValue.Enabled = rdoStandarDeviation.Checked ;
			numericMultiDeviation.Enabled = rdoStandarDeviation.Checked ;
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

		private void rdoAverage_CheckedChanged(object sender, System.EventArgs e)
		{
			if (rdoAverage.Checked)
				KernelType = eKernelType.Average;
			else
				KernelType = eKernelType.Mean;

		}

		private void numericMultiDeviation_ValueChanged(object sender, System.EventArgs e)
		{
			//Validate_MultiDeviationValue();
		}
		private bool Validate_MultiDeviationValue()
		{
			const Decimal Min = (Decimal)0.0f;
			const Decimal Max = (Decimal)10.0f;

			bool bValid = (numericMultiDeviation.Value >= Min && numericMultiDeviation.Value <= Max);
			if (bValid == false) 
			{				
				MessageBoxEx.Error("Input value must be a decimal between 0.00 and 10.00");

				if (numericMultiDeviation.Value	< Min)
					numericMultiDeviation.Value = Min;
				else 
					numericMultiDeviation.Value = Max;
			}

			return bValid;
		}

		private void numericMultiDeviation_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			//e.Cancel = (Validate_MultiDeviationValue() == false);
		}
	
		
		#endregion

		#region internal helpers
		
		private void SetCurPicbox()
		{
			switch ( _trendLineType.ToString() )
			{
				case "Polynomial":
					picCurent = picPolynomial;
					return;
			}
			picCurent = picMiddleAverage;
			
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
//			else 
//			{
//				picPower.Enabled		= false;
//				picPower.Image = imageListTrendLine.Images[12];
//				picExponential.Enabled	= false;
//				picExponential.Image = imageListTrendLine.Images[12];
//				
//			}
		}

		private void EnableControl(PictureBox sender)
		{
			nudOrder.Enabled	= sender.Tag.ToString() == "2" ;
			nudPeriod.Enabled	= sender.Tag.ToString() == "6" ;
		}

		private void SetPicImage(PictureBox picBox,bool OnClicked)
		{	
			if ( picBox.Enabled == false ) return;
			picBox.Image = imageListTrendLine.Images[GetImageIndex(picBox,OnClicked)];
		}

		private void AddData2TrendLineFormat()
		{
			_trendlineFormat.Order_Value = Convert.ToDouble(nudOrder.Value) ;
			_trendlineFormat.Period_Value = Convert.ToDouble(nudPeriod.Value) ;
			_trendlineFormat.Constant_Value = Convert.ToDouble(ValueNU.Value);
			_trendlineFormat.ManualMaxMin_Checked = Convert.ToBoolean(chkManualMaxMin.Checked);
			_trendlineFormat.MinNoiseThreshold = Convert.ToDouble(numericMinNoiseThreshold.Value);
			_trendlineFormat.MaxNoiseThreshold = Convert.ToDouble(numericMaxNoiseThreshold.Value);
			_trendlineFormat.Deviation_Checked = Convert.ToBoolean(rdoStandarDeviation.Checked);
			_trendlineFormat.Value_Checked = Convert.ToBoolean(rdoValue.Checked);

			try
			{
				_trendlineFormat.Deviation = Convert.ToDouble(txtStandarDeviation.Text);				
			}
			catch
			{
				errorProvider1.SetError(txtStandarDeviation, "Standard Deviation is not correct");
				MessageBoxEx.Error("Standard Deviation is not correct");				
				return ;
			}
			_trendlineFormat.Multiplication = Convert.ToDouble(numericMultiplicationValue.Value);
			//Tam update 25
			_trendlineFormat.Kernel_Checked = chkKernel.Checked ;
			_trendlineFormat.Kernel_Size =Convert.ToDouble(numericKernel.Value);
			_trendlineFormat.Kernel_Type = GetKernelType();
			_trendlineFormat.MultiDeviation = Convert.ToDouble(numericMultiDeviation.Value);

			_trendlineFormat.Trenline_Type = GetTrendlineType();
		}

	

		private void LoadDataTrenLineFormat()
		{
			if ( _viewType == ViewTrendlineType.Trendline )
				_trendlineFormat.Load();
			else _trendlineFormat.LoadBackGroundTrendline();

			Decimal x;
			x = Convert.ToDecimal(_trendlineFormat.Order_Value);
			x = Math.Min(Math.Max(x , nudOrder.Minimum),nudOrder.Maximum);
			nudOrder.Value = x ;

			x = Convert.ToDecimal(_trendlineFormat.Period_Value) ;
			x = Math.Min(Math.Max(x , nudPeriod.Minimum),nudPeriod.Maximum);
			nudPeriod.Value = x;

			x = Convert.ToDecimal(_trendlineFormat.Constant_Value) ;
			ValueNU.Value = Math.Min(Math.Max(x , ValueNU.Minimum),ValueNU.Maximum);

			chkManualMaxMin.Checked = Convert.ToBoolean(_trendlineFormat.ManualMaxMin_Checked) ;
			
			x = Convert.ToDecimal(_trendlineFormat.MinNoiseThreshold) ;
			numericMinNoiseThreshold.Value = Math.Min(Math.Max(x,numericMinNoiseThreshold.Minimum),numericMinNoiseThreshold.Maximum);

			x =  Convert.ToDecimal(_trendlineFormat.MaxNoiseThreshold) ;
			numericMaxNoiseThreshold.Value = Math.Min(Math.Max(x,numericMaxNoiseThreshold.Minimum),numericMaxNoiseThreshold.Maximum);
			
			x = Convert.ToDecimal(_trendlineFormat.Deviation);
			txtStandarDeviation.Text = x.ToString() ;
		
			x = Convert.ToDecimal(_trendlineFormat.Multiplication) ;
			numericMultiplicationValue.Value = x;
			
			rdoStandarDeviation.Checked = Convert.ToBoolean(_trendlineFormat.Deviation_Checked);
			
			rdoValue.Checked = Convert.ToBoolean(_trendlineFormat.Value_Checked);
	
			_trendLineType = _trendlineFormat.Trenline_Type ;
			////Tam update 25
			chkKernel.Checked = Convert.ToBoolean(_trendlineFormat.Kernel_Checked);
			numericKernel.Value =Convert.ToDecimal(_trendlineFormat.Kernel_Size);
			_kernelType  = _trendlineFormat.Kernel_Type;
			rdoAverage.Checked = _kernelType == eKernelType.Average ? true:false;
			rdoMean.Checked    = _kernelType == eKernelType.Mean    ? true:false;
			numericMultiDeviation.Value = Convert.ToDecimal(_trendlineFormat.MultiDeviation) ;
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
			switch (_kernelType)
			{
				case eKernelType.Average:
					return eKernelType.Average;
			}
			return eKernelType.Mean;
		}
		
		private eTrendlineType GetTrendlineType()
		{
			switch (picCurent.Tag.ToString())
			{
				case "2":
					//return eTrendlineType.Polynomial;
					return eTrendlineType.Polynomial;				
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
			if (_lineChart != null)
			{
				_lineChart.updateTrendLine();
				_lineChart.ThresholdVisible = true;
			}
		}
		
		#endregion
	}
}
