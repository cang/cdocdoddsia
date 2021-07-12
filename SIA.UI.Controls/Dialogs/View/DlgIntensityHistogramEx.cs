using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using SIA.SystemFrameworks;
using SIA.IPEngine;
using SIA.SystemLayer;
using SIA.UI.Components.Common;


using TYPE = System.UInt16; 

namespace SIA.UI.Controls.Dialogs
{
	/// <summary>
	/// Name : DlgIntensityHistogramEx
	/// Description : User interface for Clear Wafer Boundary 
	/// Thread Support : None
	/// Persistence Data : True
	/// </summary>
	public class DlgIntensityHistogramEx : DialogBase
	{
		#region Windows Form members
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuExit;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem menuCopyChartToClipboard;
		private System.Windows.Forms.MenuItem menuCopyDataToClipboard;
		private System.Windows.Forms.MenuItem menuView;
		private System.Windows.Forms.MenuItem menuShowCoordinates;
		private System.Windows.Forms.TextBox txtVariance;
		private System.Windows.Forms.TextBox txtTotalPixels;
		private System.Windows.Forms.TextBox txtMedian;
		private System.Windows.Forms.TextBox txtMean;
		private System.Windows.Forms.TextBox txtStdDev;
		private System.Windows.Forms.TextBox txtMaximum;
		private System.Windows.Forms.TextBox txtMinimum;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.NumericUpDown nudMinViewRange;
		private System.Windows.Forms.Label lblMin;
		private System.Windows.Forms.Label lblMax;
		private System.Windows.Forms.NumericUpDown nudMaxViewRange;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.GroupBox grpSummary;
		private System.Windows.Forms.GroupBox grpHistogramPlot;
		private SIA.UI.Controls.UserControls.HistogramViewer histogramViewer;
		private System.Windows.Forms.MainMenu mainMenu;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion

		#region Member Fields

		private int _numBins = 0;
		private int[] _values = null;
		private int _maxGrayValue = 0;
		private int _minGrayValue = 0;
		private int _minCount = 0;
		private int _maxCount = 0;
		private int _maxValue = 0;
		private int _minValue = 0;
		private double _mean = 0;
		private int _medianValue = 0;
		private double _stdDev = 0;
		private double _variance = 0;
		private long _totalPixels = 0;
		private System.Windows.Forms.CheckBox chkAutoFit;
		
		private int _ignoreChartUpdate = 0;
		#endregion

		#region Properties

		public int NumBins
		{
			get {return _numBins;}
		}

		public int[] Values 
		{
			get {return _values;}
		}

		public int Maximum
		{
			get {return _maxValue;}
		}

		public int Minimum
		{
			get {return _minValue;}
		}

		public double Mean
		{
			get {return _mean;}
		}

		public int Median
		{
			get {return _medianValue;}
		}

		public double StdDev
		{
			get {return _stdDev;}
		}

		public double Variance
		{
			get {return _variance;}
		}

		public long TotalPixels
		{
			get {return _totalPixels;}
		}

		public int MinViewRange
		{
			get {return Convert.ToInt32(nudMinViewRange.Value);}
		}

		public int MaxViewRange
		{
			get {return Convert.ToInt32(nudMaxViewRange.Value);}
		}

		#endregion

		#region Constructor and destructor

		public DlgIntensityHistogramEx(PseudoColor pseudoColor, CommonImage image, kHistogram histogram) : this()
		{
			double[] values = histogram.Histogram;
			this._maxValue = (int)histogram.Max;
			this._minValue = (int)histogram.Min;
			this._maxCount = (int)histogram.MaxCount;
			this._minCount = (int)histogram.MinCount;
			this._medianValue = (int)histogram.Median;
			this._mean = (double)histogram.Mean;
			this._stdDev = (double)histogram.StdDev;
			this._variance = (double)histogram.Var;
			this._totalPixels = image.Length;

			this._maxGrayValue = (int)image.MaxGreyValue;
			this._minGrayValue = (int)image.MinGreyValue;
			
			this._numBins = values.Length;
			this._values = new int[_numBins];
			for (int i=0; i<_numBins; i++)
				_values[i] = (int)Math.Round(values[i]);
		}

		public DlgIntensityHistogramEx(PseudoColor pseudoColor, CommonImage image, Histogram histogram) : this()
		{
			int[] values = histogram.Data;
			this._maxValue = (int)histogram.MaxValue;
			this._minValue = (int)histogram.MinValue;
			this._maxCount = (int)histogram.MaxCount;
			this._minCount = (int)histogram.MinCount;
			this._medianValue = (int)histogram.MedianValue;
			this._mean = (double)histogram.Mean;
			this._stdDev = (double)histogram.Stddev;
			this._variance = (double)histogram.Variance;
			this._totalPixels = histogram.TotalPixel;
			if (_totalPixels < 0)
				_totalPixels = image.Length;
		
			this._maxGrayValue = (int)image.MaxGreyValue;
			this._minGrayValue = (int)image.MinGreyValue;
			
			this._numBins = values.Length;
			this._values = (int[])values.Clone();			
		}

		protected DlgIntensityHistogramEx()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if ( disposing )
				if (components != null) 
					components.Dispose();
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgIntensityHistogramEx));
			this.histogramViewer = new SIA.UI.Controls.UserControls.HistogramViewer();
			this.mainMenu = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuExit = new System.Windows.Forms.MenuItem();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this.menuCopyChartToClipboard = new System.Windows.Forms.MenuItem();
			this.menuCopyDataToClipboard = new System.Windows.Forms.MenuItem();
			this.menuView = new System.Windows.Forms.MenuItem();
			this.menuShowCoordinates = new System.Windows.Forms.MenuItem();
			this.grpSummary = new System.Windows.Forms.GroupBox();
			this.txtVariance = new System.Windows.Forms.TextBox();
			this.txtTotalPixels = new System.Windows.Forms.TextBox();
			this.txtMedian = new System.Windows.Forms.TextBox();
			this.txtMean = new System.Windows.Forms.TextBox();
			this.txtStdDev = new System.Windows.Forms.TextBox();
			this.txtMaximum = new System.Windows.Forms.TextBox();
			this.txtMinimum = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.grpHistogramPlot = new System.Windows.Forms.GroupBox();
			this.nudMinViewRange = new System.Windows.Forms.NumericUpDown();
			this.lblMin = new System.Windows.Forms.Label();
			this.lblMax = new System.Windows.Forms.Label();
			this.nudMaxViewRange = new System.Windows.Forms.NumericUpDown();
			this.label8 = new System.Windows.Forms.Label();
			this.chkAutoFit = new System.Windows.Forms.CheckBox();
			this.grpSummary.SuspendLayout();
			this.grpHistogramPlot.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudMinViewRange)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMaxViewRange)).BeginInit();
			this.SuspendLayout();
			// 
			// histogramViewer
			// 
			this.histogramViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.histogramViewer.Location = new System.Drawing.Point(8, 16);
			this.histogramViewer.MousePoint = ((System.Drawing.PointF)(resources.GetObject("histogramViewer.MousePoint")));
			this.histogramViewer.Name = "histogramViewer";
			this.histogramViewer.RightMenu = false;
			this.histogramViewer.Size = new System.Drawing.Size(470, 200);
			this.histogramViewer.TabIndex = 0;
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.menuItem1,
																					 this.menuItem8,
																					 this.menuView});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuExit});
			this.menuItem1.Text = "File";
			// 
			// menuExit
			// 
			this.menuExit.Index = 0;
			this.menuExit.Shortcut = System.Windows.Forms.Shortcut.AltF4;
			this.menuExit.Text = "&Close";
			this.menuExit.Click += new System.EventHandler(this.MenuItem_Click);
			// 
			// menuItem8
			// 
			this.menuItem8.Index = 1;
			this.menuItem8.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuCopyChartToClipboard,
																					  this.menuCopyDataToClipboard});
			this.menuItem8.Text = "Edit";
			// 
			// menuCopyChartToClipboard
			// 
			this.menuCopyChartToClipboard.Index = 0;
			this.menuCopyChartToClipboard.Text = "Copy &Chart To Clipboard";
			this.menuCopyChartToClipboard.Click += new System.EventHandler(this.MenuItem_Click);
			// 
			// menuCopyDataToClipboard
			// 
			this.menuCopyDataToClipboard.Index = 1;
			this.menuCopyDataToClipboard.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
			this.menuCopyDataToClipboard.Text = "Copy &Data To Clipboard";
			this.menuCopyDataToClipboard.Click += new System.EventHandler(this.MenuItem_Click);
			// 
			// menuView
			// 
			this.menuView.Index = 2;
			this.menuView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.menuShowCoordinates});
			this.menuView.Text = "View";
			// 
			// menuShowCoordinates
			// 
			this.menuShowCoordinates.Checked = true;
			this.menuShowCoordinates.Index = 0;
			this.menuShowCoordinates.Text = "Show Coordinates";
			this.menuShowCoordinates.Click += new System.EventHandler(this.MenuItem_Click);
			// 
			// grpSummary
			// 
			this.grpSummary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.grpSummary.Controls.Add(this.txtVariance);
			this.grpSummary.Controls.Add(this.txtTotalPixels);
			this.grpSummary.Controls.Add(this.txtMedian);
			this.grpSummary.Controls.Add(this.txtMean);
			this.grpSummary.Controls.Add(this.txtStdDev);
			this.grpSummary.Controls.Add(this.txtMaximum);
			this.grpSummary.Controls.Add(this.txtMinimum);
			this.grpSummary.Controls.Add(this.label1);
			this.grpSummary.Controls.Add(this.label2);
			this.grpSummary.Controls.Add(this.label3);
			this.grpSummary.Controls.Add(this.label4);
			this.grpSummary.Controls.Add(this.label5);
			this.grpSummary.Controls.Add(this.label6);
			this.grpSummary.Controls.Add(this.label7);
			this.grpSummary.Location = new System.Drawing.Point(8, 293);
			this.grpSummary.Name = "grpSummary";
			this.grpSummary.Size = new System.Drawing.Size(486, 132);
			this.grpSummary.TabIndex = 1;
			this.grpSummary.TabStop = false;
			this.grpSummary.Text = "Summary";
			// 
			// txtVariance
			// 
			this.txtVariance.AutoSize = false;
			this.txtVariance.Location = new System.Drawing.Point(308, 44);
			this.txtVariance.Name = "txtVariance";
			this.txtVariance.ReadOnly = true;
			this.txtVariance.Size = new System.Drawing.Size(136, 24);
			this.txtVariance.TabIndex = 7;
			this.txtVariance.Text = "1234";
			// 
			// txtTotalPixels
			// 
			this.txtTotalPixels.AutoSize = false;
			this.txtTotalPixels.Location = new System.Drawing.Point(80, 98);
			this.txtTotalPixels.Name = "txtTotalPixels";
			this.txtTotalPixels.ReadOnly = true;
			this.txtTotalPixels.Size = new System.Drawing.Size(136, 24);
			this.txtTotalPixels.TabIndex = 13;
			this.txtTotalPixels.Text = "1234";
			// 
			// txtMedian
			// 
			this.txtMedian.AutoSize = false;
			this.txtMedian.Location = new System.Drawing.Point(80, 42);
			this.txtMedian.Name = "txtMedian";
			this.txtMedian.ReadOnly = true;
			this.txtMedian.Size = new System.Drawing.Size(136, 24);
			this.txtMedian.TabIndex = 5;
			this.txtMedian.Text = "1234";
			// 
			// txtMean
			// 
			this.txtMean.AutoSize = false;
			this.txtMean.Location = new System.Drawing.Point(308, 16);
			this.txtMean.Name = "txtMean";
			this.txtMean.ReadOnly = true;
			this.txtMean.Size = new System.Drawing.Size(136, 24);
			this.txtMean.TabIndex = 3;
			this.txtMean.Text = "1234";
			// 
			// txtStdDev
			// 
			this.txtStdDev.AutoSize = false;
			this.txtStdDev.Location = new System.Drawing.Point(308, 72);
			this.txtStdDev.Name = "txtStdDev";
			this.txtStdDev.ReadOnly = true;
			this.txtStdDev.Size = new System.Drawing.Size(136, 24);
			this.txtStdDev.TabIndex = 11;
			this.txtStdDev.Text = "1234";
			// 
			// txtMaximum
			// 
			this.txtMaximum.AutoSize = false;
			this.txtMaximum.Location = new System.Drawing.Point(80, 14);
			this.txtMaximum.Name = "txtMaximum";
			this.txtMaximum.ReadOnly = true;
			this.txtMaximum.Size = new System.Drawing.Size(136, 24);
			this.txtMaximum.TabIndex = 1;
			this.txtMaximum.Text = "1234";
			// 
			// txtMinimum
			// 
			this.txtMinimum.AutoSize = false;
			this.txtMinimum.Location = new System.Drawing.Point(80, 70);
			this.txtMinimum.Name = "txtMinimum";
			this.txtMinimum.ReadOnly = true;
			this.txtMinimum.Size = new System.Drawing.Size(136, 24);
			this.txtMinimum.TabIndex = 9;
			this.txtMinimum.Text = "1234";
			// 
			// label1
			// 
			this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label1.Location = new System.Drawing.Point(8, 98);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(70, 24);
			this.label1.TabIndex = 12;
			this.label1.Text = "Total Pixels :";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label2.Location = new System.Drawing.Point(8, 42);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(70, 24);
			this.label2.TabIndex = 4;
			this.label2.Text = "Median :";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label3.Location = new System.Drawing.Point(228, 72);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(70, 24);
			this.label3.TabIndex = 10;
			this.label3.Text = "Std Dev. :";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label4.Location = new System.Drawing.Point(228, 44);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(70, 24);
			this.label4.TabIndex = 6;
			this.label4.Text = "Variance :";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label5
			// 
			this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label5.Location = new System.Drawing.Point(228, 16);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(70, 24);
			this.label5.TabIndex = 2;
			this.label5.Text = "Mean :";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label6
			// 
			this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label6.Location = new System.Drawing.Point(8, 14);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(70, 24);
			this.label6.TabIndex = 0;
			this.label6.Text = "Maximum :";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label7
			// 
			this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label7.Location = new System.Drawing.Point(8, 70);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(70, 24);
			this.label7.TabIndex = 8;
			this.label7.Text = "Minimum :";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// grpHistogramPlot
			// 
			this.grpHistogramPlot.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.grpHistogramPlot.Controls.Add(this.chkAutoFit);
			this.grpHistogramPlot.Controls.Add(this.nudMinViewRange);
			this.grpHistogramPlot.Controls.Add(this.lblMin);
			this.grpHistogramPlot.Controls.Add(this.lblMax);
			this.grpHistogramPlot.Controls.Add(this.nudMaxViewRange);
			this.grpHistogramPlot.Controls.Add(this.label8);
			this.grpHistogramPlot.Controls.Add(this.histogramViewer);
			this.grpHistogramPlot.Location = new System.Drawing.Point(8, 4);
			this.grpHistogramPlot.Name = "grpHistogramPlot";
			this.grpHistogramPlot.Size = new System.Drawing.Size(486, 281);
			this.grpHistogramPlot.TabIndex = 0;
			this.grpHistogramPlot.TabStop = false;
			this.grpHistogramPlot.Text = "Histogram Plot";
			// 
			// nudMinViewRange
			// 
			this.nudMinViewRange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.nudMinViewRange.Location = new System.Drawing.Point(136, 228);
			this.nudMinViewRange.Name = "nudMinViewRange";
			this.nudMinViewRange.Size = new System.Drawing.Size(108, 20);
			this.nudMinViewRange.TabIndex = 3;
			this.nudMinViewRange.ValueChanged += new System.EventHandler(this.ViewRange_ValueChanged);
			// 
			// lblMin
			// 
			this.lblMin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblMin.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.lblMin.Location = new System.Drawing.Point(104, 224);
			this.lblMin.Name = "lblMin";
			this.lblMin.Size = new System.Drawing.Size(28, 24);
			this.lblMin.TabIndex = 2;
			this.lblMin.Text = "Min:";
			this.lblMin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblMax
			// 
			this.lblMax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblMax.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.lblMax.Location = new System.Drawing.Point(256, 224);
			this.lblMax.Name = "lblMax";
			this.lblMax.Size = new System.Drawing.Size(32, 24);
			this.lblMax.TabIndex = 4;
			this.lblMax.Text = "Max:";
			this.lblMax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// nudMaxViewRange
			// 
			this.nudMaxViewRange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.nudMaxViewRange.Location = new System.Drawing.Point(292, 228);
			this.nudMaxViewRange.Maximum = new System.Decimal(new int[] {
																			255,
																			0,
																			0,
																			0});
			this.nudMaxViewRange.Name = "nudMaxViewRange";
			this.nudMaxViewRange.Size = new System.Drawing.Size(108, 20);
			this.nudMaxViewRange.TabIndex = 5;
			this.nudMaxViewRange.Value = new System.Decimal(new int[] {
																		  255,
																		  0,
																		  0,
																		  0});
			this.nudMaxViewRange.ValueChanged += new System.EventHandler(this.ViewRange_ValueChanged);
			// 
			// label8
			// 
			this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label8.Location = new System.Drawing.Point(8, 224);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(88, 24);
			this.label8.TabIndex = 1;
			this.label8.Text = "Intensity Range:";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// chkAutoFit
			// 
			this.chkAutoFit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkAutoFit.Location = new System.Drawing.Point(12, 252);
			this.chkAutoFit.Name = "chkAutoFit";
			this.chkAutoFit.Size = new System.Drawing.Size(384, 24);
			this.chkAutoFit.TabIndex = 6;
			this.chkAutoFit.Text = "Auto fit min and max intensity value";
			this.chkAutoFit.CheckedChanged += new System.EventHandler(this.chkAutoFit_CheckedChanged);
			// 
			// DlgIntensityHistogramEx
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(502, 451);
			this.Controls.Add(this.grpHistogramPlot);
			this.Controls.Add(this.grpSummary);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = true;
			this.Menu = this.mainMenu;
			this.MinimizeBox = true;
			this.Name = "DlgIntensityHistogramEx";
			this.Text = "Intensity Histogram";
			this.grpSummary.ResumeLayout(false);
			this.grpHistogramPlot.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.nudMinViewRange)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMaxViewRange)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region Public Methods

		#endregion

		#region Override Routines

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

            this.Menu = null;

			this._ignoreChartUpdate++;
			this.nudMinViewRange.Minimum = this.nudMaxViewRange.Minimum = Decimal.MinValue;
			this.nudMinViewRange.Maximum = this.nudMaxViewRange.Maximum = Decimal.MaxValue;
			this.nudMinViewRange.Value = Convert.ToDecimal(this._minGrayValue);
			this.nudMaxViewRange.Value = Convert.ToDecimal(this._maxGrayValue);
			this._ignoreChartUpdate--;
			
			// refresh chart for first load            
			this.RefreshData();

            this.MinimumSize = new Size(500, 500);
		}


		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed (e);
		}


		#endregion

		#region Event Handlers

		private void MenuItem_Click(object sender, System.EventArgs e)
		{	
			if (sender == menuExit)
				DoCommandClose();
			else if (sender == menuShowCoordinates)
				DoCommandShowCoordinates();	
			else if (sender == menuCopyChartToClipboard)
				DoCommandCopyChartToClipboard();
			else if (sender == menuCopyDataToClipboard)
				DoCommandCopyDataToClipboard();			
		}


		private void ViewRange_ValueChanged(object sender, System.EventArgs e)
		{
			this.RefreshData();
		}


		private void chkAutoFit_CheckedChanged(object sender, System.EventArgs e)
		{
			if (chkAutoFit.Checked)
				this.AutoFitDataRange();

			nudMaxViewRange.Enabled = chkAutoFit.Enabled;
			nudMinViewRange.Enabled = chkAutoFit.Enabled;
		}

		#endregion

		#region Command handlers

		private void DoCommandClose()
		{
			this.Close();
		}

		private void DoCommandShowCoordinates()
		{
			this.menuShowCoordinates.Checked = !this.menuShowCoordinates.Checked;			
			histogramViewer.PlotSurfaceRefresh();
		}
		
		private void DoCommandPrintPreview()
		{
			histogramViewer.DoCommandPrintPreview();
		}

		private void DoCommandPrint()
		{
			histogramViewer.DoCommandPrint();
		}

		private void DoCommandCopyChartToClipboard()
		{
			histogramViewer.DoCommandCopyChartToClipboard();
		}
	
		private void DoCommandCopyDataToClipboard()
		{
			histogramViewer.DoCommandCopyDataToClipboard();
		}

		#endregion

		#region Internal Helpers

		private void AutoFitDataRange()
		{
			this._ignoreChartUpdate++;
			this.nudMinViewRange.Minimum = this.nudMaxViewRange.Minimum = Decimal.MinValue;
			this.nudMinViewRange.Maximum = this.nudMaxViewRange.Maximum = Decimal.MaxValue;
			this.nudMinViewRange.Value = Convert.ToDecimal(this._minValue);
			this.nudMaxViewRange.Value = Convert.ToDecimal(this._maxValue);
			this._ignoreChartUpdate--;

			// refresh chart for first load
			this.RefreshData();
		}
	
		private void RefreshData()
		{
			if (this._ignoreChartUpdate != 0)
				return;

			this._ignoreChartUpdate++;

			// retrieve view range
			int minViewRange = this.MinViewRange;
			int maxViewRange = this.MaxViewRange;
			int range = Math.Abs(maxViewRange - minViewRange + 1);
				
			// fills data values
			float[] categories = new float[range];
			float[] values = new float[range];
			for (int i=0; i<range; i++)
			{
				categories[i] = i + minViewRange;
                //values[i] = (this._values[i + minViewRange]);
				values[i] = (this._values[i+minViewRange] * 100.0f / _totalPixels);
                //if (values[i] != 0)
                //if (this._values[i + minViewRange] != 0)
                //{
                //    values[i] = (float)Math.Log(this._values[i + minViewRange], Math.E);
                //}
			}
			
			// refresh chart data
			this.RefreshChart(categories, values);

			// refresh summary items
			string format = "0.000";
			txtMaximum.Text = this._maxValue.ToString();
			txtMinimum.Text = this._minValue.ToString();
			txtMean.Text = this._minValue.ToString(format);
			txtMedian.Text = this._medianValue.ToString();
			txtTotalPixels.Text = this._totalPixels.ToString();
			txtVariance.Text = this._variance.ToString(format);
			txtStdDev.Text = this._stdDev.ToString(format);
			txtMean.Text = this._mean.ToString(format);

			// refresh view range
			nudMinViewRange.Minimum = _minGrayValue;
			nudMinViewRange.Maximum = maxViewRange-1;
						
			nudMaxViewRange.Minimum = minViewRange+1;
			nudMaxViewRange.Maximum = _maxGrayValue;

			this._ignoreChartUpdate--;
		}

		private void RefreshChart(float[] categories, float[] yVals)
		{
			try
			{
				// initialize plot surface
				histogramViewer.UpdatePlotSurface();
				// create new histogram plot
				histogramViewer.AddHistogramPlot(categories, yVals, true, "Histogram", Color.Blue, null, 1.0f);
				// add label and title
				histogramViewer.AddLabelsAndTitle("Intensity", "Number Of Pixels", "Histogram Statistic");									
				// enable diplaying coordinate
				//histogramViewer.ShowCoordinates = true;
				// alow context menu
				histogramViewer.RightMenu = true;				
				// add vertical interaction line
				histogramViewer.AddVertialInteraction(Color.DarkGreen);				
				// add horizontal interaction line
				histogramViewer.AddHorizontalInteraction(Color.DarkGreen);							
				// refresh plot surface
				histogramViewer.PlotSurfaceRefresh();
			}
			catch
			{
				// empty chart viewer data
				histogramViewer.UpdatePlotSurface();

				// re-throw exception
				throw;
			}
			finally
			{
			}
		}

		#endregion		
	}
}
