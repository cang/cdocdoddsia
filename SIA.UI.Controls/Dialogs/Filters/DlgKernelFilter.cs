using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using SIA.SystemLayer;
using SIA.UI.Components;
using SIA.UI.Controls.Utilities;

using SIA.Common;
using SIA.UI.Controls.Automation;
using SIA.UI.Controls.Automation.Commands;
using SIA.UI.Controls.Commands;

namespace SIA.UI.Controls.Dialogs
{
	/// <summary>
	/// Name : DlgKernelFilter
	/// Description : User interface for Kernel Filters
	/// Thread Support : True
	/// Persistence Data : True
	/// </summary>
	public class DlgKernelFilter : SIA.UI.Controls.Dialogs.DialogPreviewBase
	{
		#region Window Form members
		private System.Windows.Forms.GroupBox grpPreview;
		private System.Windows.Forms.TabPage tabEnhancement;
		private System.Windows.Forms.TabPage tabMorphology;
		private System.Windows.Forms.RadioButton rdHighGausian;
		private System.Windows.Forms.RadioButton rdScrupt;
		private System.Windows.Forms.RadioButton rdWell;
		private System.Windows.Forms.RadioButton rdLaplace;
		private System.Windows.Forms.RadioButton rdHorizontalEdge;
		private System.Windows.Forms.RadioButton rdHighPass;
		private System.Windows.Forms.RadioButton rdLowPass;
		private System.Windows.Forms.RadioButton rdVerticalEdge;
		private System.Windows.Forms.RadioButton rdUnsharp;
		private System.Windows.Forms.RadioButton rdTopHat;
		private System.Windows.Forms.RadioButton rdCustom;
		private System.Windows.Forms.RadioButton rd5_5;
		private System.Windows.Forms.RadioButton rd7_7;
		private System.Windows.Forms.RadioButton rd3_3;
		private System.Windows.Forms.NumericUpDown ndEnPass;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.RadioButton rdDilation;
		private System.Windows.Forms.RadioButton rdClosing;
		private System.Windows.Forms.RadioButton rdMoCustom;
		private System.Windows.Forms.RadioButton rdErosion;
		private System.Windows.Forms.RadioButton rd3_3Cross;
		private System.Windows.Forms.RadioButton rd5_5Ring;
		private System.Windows.Forms.RadioButton rd2_2sq;
		private System.Windows.Forms.RadioButton rdOpening;
		private System.Windows.Forms.RadioButton rd5_5Circle;
		private System.Windows.Forms.RadioButton rd7_7Circle;
		private System.Windows.Forms.RadioButton rd3_3Ring;
		private System.Windows.Forms.RadioButton rd7_7Ring;
		private System.Windows.Forms.RadioButton rd3_3Sq;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.GroupBox gMorOption;
		private System.Windows.Forms.GroupBox gMorFilter;
		private System.Windows.Forms.NumericUpDown ndMorPass;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblPass;
		private System.Windows.Forms.RadioButton rdHotPixel;
		private System.Windows.Forms.RadioButton rdDeadPixel;
		private System.Windows.Forms.NumericUpDown nudTheshold;
		private System.Windows.Forms.Label lblThreshold;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnCusFilter;
		private System.Windows.Forms.RadioButton rdFlattenBackground;
		private System.Windows.Forms.RadioButton rdMedian;
		
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.Button btnPreview;
		private System.Windows.Forms.TabControl tabOptions;
		private System.Windows.Forms.GroupBox grpConvolutionMatrixType;
		private System.Windows.Forms.GroupBox grpConvolutionType;
		
		private SIA.UI.Components.ImagePreview _imagePreview;
		
		private RadioButton curEnFilterRd;
		private RadioButton curEnOptionRd;
		private RadioButton curMorFilterRd;		
		private RadioButton curMorOptionRd;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion 

		#region member attributes
		
		private bool arrEnFilterValidate = false;
		private bool arrMorFilterValidate = false;

		private int [,] arrEnFilter = null;
		private System.Windows.Forms.Button btnLoadSettings;
		private System.Windows.Forms.Button btnSaveSettings;
		private ArrayList arrMorFilter = new ArrayList();

		#endregion

		#region public Properties
		public int EnPass
		{
			get
			{
				return (int)ndEnPass.Value;
			}
            set
            {
                try
                {
                    ndEnPass.Value = (decimal)value;
                }
                catch
                {
                }

            }
		}

		public int MorPass
		{
			get
			{
				return (int)ndMorPass.Value;
			}
            set
            {
                try
                {
                    ndMorPass.Value = (decimal)value;
                }
                catch
                {
                }
            }
		}

		public int[,] CustomMatrix
		{
			get {return arrEnFilter;}
			set {arrEnFilter = value;}
		}

		public float Threshold
		{
			get
			{
				return (float)nudTheshold.Value/100;
			}
            set
            {
                try
                {
                    nudTheshold.Value = (decimal)(value*100);
                }
                catch
                {
                }

            }
		}

		public bool IsApplyConvolution
		{
			get 
            {
                return tabOptions.SelectedIndex == 0;
            }
            set
            {
                if (value)
                    tabOptions.SelectedIndex = 0;
                else
                    tabOptions.SelectedIndex = 1;
            }
		}

		#endregion

		#region constructtor/destructor
		public DlgKernelFilter(SIA.UI.Controls.ImageWorkspace owner) : base(owner, true)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			
			InitRadioButtonEvent();

            btnCusFilter.Enabled = rdCustom.Checked;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgKernelFilter));
			this.grpPreview = new System.Windows.Forms.GroupBox();
			this._imagePreview = new SIA.UI.Components.ImagePreview();
			this.btnPreview = new System.Windows.Forms.Button();
			this.btnReset = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.tabOptions = new System.Windows.Forms.TabControl();
			this.tabEnhancement = new System.Windows.Forms.TabPage();
			this.grpConvolutionMatrixType = new System.Windows.Forms.GroupBox();
			this.lblPass = new System.Windows.Forms.Label();
			this.ndEnPass = new System.Windows.Forms.NumericUpDown();
			this.rd5_5 = new System.Windows.Forms.RadioButton();
			this.rd7_7 = new System.Windows.Forms.RadioButton();
			this.rd3_3 = new System.Windows.Forms.RadioButton();
			this.nudTheshold = new System.Windows.Forms.NumericUpDown();
			this.lblThreshold = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.grpConvolutionType = new System.Windows.Forms.GroupBox();
			this.rdMedian = new System.Windows.Forms.RadioButton();
			this.btnCusFilter = new System.Windows.Forms.Button();
			this.rdLowPass = new System.Windows.Forms.RadioButton();
			this.rdVerticalEdge = new System.Windows.Forms.RadioButton();
			this.rdUnsharp = new System.Windows.Forms.RadioButton();
			this.rdTopHat = new System.Windows.Forms.RadioButton();
			this.rdCustom = new System.Windows.Forms.RadioButton();
			this.rdHighPass = new System.Windows.Forms.RadioButton();
			this.rdHorizontalEdge = new System.Windows.Forms.RadioButton();
			this.rdLaplace = new System.Windows.Forms.RadioButton();
			this.rdWell = new System.Windows.Forms.RadioButton();
			this.rdScrupt = new System.Windows.Forms.RadioButton();
			this.rdHighGausian = new System.Windows.Forms.RadioButton();
			this.rdHotPixel = new System.Windows.Forms.RadioButton();
			this.rdDeadPixel = new System.Windows.Forms.RadioButton();
			this.tabMorphology = new System.Windows.Forms.TabPage();
			this.gMorOption = new System.Windows.Forms.GroupBox();
			this.rd7_7Circle = new System.Windows.Forms.RadioButton();
			this.rd3_3Ring = new System.Windows.Forms.RadioButton();
			this.rd7_7Ring = new System.Windows.Forms.RadioButton();
			this.rd3_3Sq = new System.Windows.Forms.RadioButton();
			this.rd5_5Circle = new System.Windows.Forms.RadioButton();
			this.label4 = new System.Windows.Forms.Label();
			this.ndMorPass = new System.Windows.Forms.NumericUpDown();
			this.rd3_3Cross = new System.Windows.Forms.RadioButton();
			this.rd5_5Ring = new System.Windows.Forms.RadioButton();
			this.rd2_2sq = new System.Windows.Forms.RadioButton();
			this.gMorFilter = new System.Windows.Forms.GroupBox();
			this.rdOpening = new System.Windows.Forms.RadioButton();
			this.rdDilation = new System.Windows.Forms.RadioButton();
			this.rdClosing = new System.Windows.Forms.RadioButton();
			this.rdMoCustom = new System.Windows.Forms.RadioButton();
			this.rdErosion = new System.Windows.Forms.RadioButton();
			this.rdFlattenBackground = new System.Windows.Forms.RadioButton();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnLoadSettings = new System.Windows.Forms.Button();
			this.btnSaveSettings = new System.Windows.Forms.Button();
			this.grpPreview.SuspendLayout();
			this.tabOptions.SuspendLayout();
			this.tabEnhancement.SuspendLayout();
			this.grpConvolutionMatrixType.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ndEnPass)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudTheshold)).BeginInit();
			this.grpConvolutionType.SuspendLayout();
			this.tabMorphology.SuspendLayout();
			this.gMorOption.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ndMorPass)).BeginInit();
			this.gMorFilter.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpPreview
			// 
			this.grpPreview.Controls.Add(this._imagePreview);
			this.grpPreview.Controls.Add(this.btnPreview);
			this.grpPreview.Controls.Add(this.btnReset);
			this.grpPreview.Location = new System.Drawing.Point(6, 4);
			this.grpPreview.Name = "grpPreview";
			this.grpPreview.Size = new System.Drawing.Size(266, 272);
			this.grpPreview.TabIndex = 0;
			this.grpPreview.TabStop = false;
			this.grpPreview.Text = "Preview";
			// 
			// _imagePreview
			// 
			this._imagePreview.ImageViewer = null;
			this._imagePreview.Location = new System.Drawing.Point(9, 16);
			this._imagePreview.Name = "_imagePreview";
			this._imagePreview.PreviewRectangle = new System.Drawing.Rectangle(0, 0, 0, 0);
			this._imagePreview.Size = new System.Drawing.Size(248, 220);
			this._imagePreview.TabIndex = 0;
			// 
			// btnPreview
			// 
			this.btnPreview.Location = new System.Drawing.Point(72, 244);
			this.btnPreview.Name = "btnPreview";
			this.btnPreview.Size = new System.Drawing.Size(60, 23);
			this.btnPreview.TabIndex = 2;
			this.btnPreview.Text = "Preview";
			this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
			// 
			// btnReset
			// 
			this.btnReset.Location = new System.Drawing.Point(8, 244);
			this.btnReset.Name = "btnReset";
			this.btnReset.Size = new System.Drawing.Size(60, 23);
			this.btnReset.TabIndex = 1;
			this.btnReset.Text = "Reset";
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// btnOk
			// 
			this.btnOk.Location = new System.Drawing.Point(280, 8);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(88, 23);
			this.btnOk.TabIndex = 1;
			this.btnOk.Text = "OK";
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// tabOptions
			// 
			this.tabOptions.Controls.Add(this.tabEnhancement);
			this.tabOptions.Controls.Add(this.tabMorphology);
			this.tabOptions.Location = new System.Drawing.Point(8, 284);
			this.tabOptions.Name = "tabOptions";
			this.tabOptions.SelectedIndex = 0;
			this.tabOptions.Size = new System.Drawing.Size(264, 300);
			this.tabOptions.TabIndex = 0;
            this.tabOptions.SelectedIndexChanged += new EventHandler(tabOptions_SelectedIndexChanged);
			// 
			// tabEnhancement
			// 
			this.tabEnhancement.Controls.Add(this.grpConvolutionMatrixType);
			this.tabEnhancement.Controls.Add(this.grpConvolutionType);
			this.tabEnhancement.Location = new System.Drawing.Point(4, 22);
			this.tabEnhancement.Name = "tabEnhancement";
			this.tabEnhancement.Size = new System.Drawing.Size(256, 274);
			this.tabEnhancement.TabIndex = 0;
			this.tabEnhancement.Text = "Enhancement";
			// 
			// grpConvolutionMatrixType
			// 
			this.grpConvolutionMatrixType.Controls.Add(this.lblPass);
			this.grpConvolutionMatrixType.Controls.Add(this.ndEnPass);
			this.grpConvolutionMatrixType.Controls.Add(this.rd5_5);
			this.grpConvolutionMatrixType.Controls.Add(this.rd7_7);
			this.grpConvolutionMatrixType.Controls.Add(this.rd3_3);
			this.grpConvolutionMatrixType.Controls.Add(this.nudTheshold);
			this.grpConvolutionMatrixType.Controls.Add(this.lblThreshold);
			this.grpConvolutionMatrixType.Controls.Add(this.label1);
			this.grpConvolutionMatrixType.Location = new System.Drawing.Point(4, 191);
			this.grpConvolutionMatrixType.Name = "grpConvolutionMatrixType";
			this.grpConvolutionMatrixType.Size = new System.Drawing.Size(248, 92);
			this.grpConvolutionMatrixType.TabIndex = 0;
			this.grpConvolutionMatrixType.TabStop = false;
			this.grpConvolutionMatrixType.Tag = "gEnOption";
			this.grpConvolutionMatrixType.Text = "Options";
			// 
			// lblPass
			// 
			this.lblPass.Location = new System.Drawing.Point(128, 20);
			this.lblPass.Name = "lblPass";
			this.lblPass.Size = new System.Drawing.Size(43, 20);
			this.lblPass.TabIndex = 3;
			this.lblPass.Text = "Pass:";
			this.lblPass.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// ndEnPass
			// 
			this.ndEnPass.Location = new System.Drawing.Point(176, 20);
			this.ndEnPass.Maximum = new System.Decimal(new int[] {
										     20,
										     0,
										     0,
										     0});
			this.ndEnPass.Minimum = new System.Decimal(new int[] {
										     1,
										     0,
										     0,
										     0});
			this.ndEnPass.Name = "ndEnPass";
			this.ndEnPass.Size = new System.Drawing.Size(40, 20);
			this.ndEnPass.TabIndex = 4;
			this.ndEnPass.Value = new System.Decimal(new int[] {
										   1,
										   0,
										   0,
										   0});
			// 
			// rd5_5
			// 
			this.rd5_5.Location = new System.Drawing.Point(12, 40);
			this.rd5_5.Name = "rd5_5";
			this.rd5_5.Size = new System.Drawing.Size(56, 16);
			this.rd5_5.TabIndex = 1;
			this.rd5_5.Tag = "5x5";
			this.rd5_5.Text = "5 x 5";
			// 
			// rd7_7
			// 
			this.rd7_7.Location = new System.Drawing.Point(12, 60);
			this.rd7_7.Name = "rd7_7";
			this.rd7_7.Size = new System.Drawing.Size(56, 16);
			this.rd7_7.TabIndex = 2;
			this.rd7_7.Tag = "7x7";
			this.rd7_7.Text = "7 x 7";
			// 
			// rd3_3
			// 
			this.rd3_3.Checked = true;
			this.rd3_3.Location = new System.Drawing.Point(12, 20);
			this.rd3_3.Name = "rd3_3";
			this.rd3_3.Size = new System.Drawing.Size(56, 16);
			this.rd3_3.TabIndex = 0;
			this.rd3_3.TabStop = true;
			this.rd3_3.Tag = "3x3";
			this.rd3_3.Text = "3 x 3";
			// 
			// nudTheshold
			// 
			this.nudTheshold.Location = new System.Drawing.Point(176, 48);
			this.nudTheshold.Minimum = new System.Decimal(new int[] {
											1,
											0,
											0,
											0});
			this.nudTheshold.Name = "nudTheshold";
			this.nudTheshold.Size = new System.Drawing.Size(40, 20);
			this.nudTheshold.TabIndex = 6;
			this.nudTheshold.Value = new System.Decimal(new int[] {
										      50,
										      0,
										      0,
										      0});
			// 
			// lblThreshold
			// 
			this.lblThreshold.Location = new System.Drawing.Point(100, 48);
			this.lblThreshold.Name = "lblThreshold";
			this.lblThreshold.Size = new System.Drawing.Size(72, 20);
			this.lblThreshold.TabIndex = 5;
			this.lblThreshold.Text = "Threshold:";
			this.lblThreshold.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(216, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(28, 20);
			this.label1.TabIndex = 7;
			this.label1.Text = "(%)";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// grpConvolutionType
			// 
			this.grpConvolutionType.Controls.Add(this.rdMedian);
			this.grpConvolutionType.Controls.Add(this.btnCusFilter);
			this.grpConvolutionType.Controls.Add(this.rdLowPass);
			this.grpConvolutionType.Controls.Add(this.rdVerticalEdge);
			this.grpConvolutionType.Controls.Add(this.rdUnsharp);
			this.grpConvolutionType.Controls.Add(this.rdTopHat);
			this.grpConvolutionType.Controls.Add(this.rdCustom);
			this.grpConvolutionType.Controls.Add(this.rdHighPass);
			this.grpConvolutionType.Controls.Add(this.rdHorizontalEdge);
			this.grpConvolutionType.Controls.Add(this.rdLaplace);
			this.grpConvolutionType.Controls.Add(this.rdWell);
			this.grpConvolutionType.Controls.Add(this.rdScrupt);
			this.grpConvolutionType.Controls.Add(this.rdHighGausian);
			this.grpConvolutionType.Controls.Add(this.rdHotPixel);
			this.grpConvolutionType.Controls.Add(this.rdDeadPixel);
			this.grpConvolutionType.Location = new System.Drawing.Point(4, 4);
			this.grpConvolutionType.Name = "grpConvolutionType";
			this.grpConvolutionType.Size = new System.Drawing.Size(248, 188);
			this.grpConvolutionType.TabIndex = 0;
			this.grpConvolutionType.TabStop = false;
			this.grpConvolutionType.Tag = "gEnFilter";
			this.grpConvolutionType.Text = "Filter Types";
			// 
			// rdMedian
			// 
			this.rdMedian.Location = new System.Drawing.Point(12, 140);
			this.rdMedian.Name = "rdMedian";
			this.rdMedian.Size = new System.Drawing.Size(104, 16);
			this.rdMedian.TabIndex = 14;
			this.rdMedian.Tag = "Median";
			this.rdMedian.Text = "Median";
			// 
			// btnCusFilter
			// 
			this.btnCusFilter.Location = new System.Drawing.Point(80, 162);
			this.btnCusFilter.Name = "btnCusFilter";
			this.btnCusFilter.Size = new System.Drawing.Size(60, 20);
			this.btnCusFilter.TabIndex = 13;
			this.btnCusFilter.Text = "Set Filter";
			this.btnCusFilter.Click += new System.EventHandler(this.btnCusFilter_Click);
			// 
			// rdLowPass
			// 
			this.rdLowPass.Location = new System.Drawing.Point(132, 20);
			this.rdLowPass.Name = "rdLowPass";
			this.rdLowPass.Size = new System.Drawing.Size(104, 16);
			this.rdLowPass.TabIndex = 1;
			this.rdLowPass.Tag = "LowPass";
			this.rdLowPass.Text = "Low-Pass";
			// 
			// rdVerticalEdge
			// 
			this.rdVerticalEdge.Location = new System.Drawing.Point(132, 40);
			this.rdVerticalEdge.Name = "rdVerticalEdge";
			this.rdVerticalEdge.Size = new System.Drawing.Size(104, 16);
			this.rdVerticalEdge.TabIndex = 3;
			this.rdVerticalEdge.Tag = "VerticalEdge";
			this.rdVerticalEdge.Text = "Vertical Edge";
			// 
			// rdUnsharp
			// 
			this.rdUnsharp.Location = new System.Drawing.Point(132, 60);
			this.rdUnsharp.Name = "rdUnsharp";
			this.rdUnsharp.Size = new System.Drawing.Size(104, 16);
			this.rdUnsharp.TabIndex = 5;
			this.rdUnsharp.Tag = "Unsharp";
			this.rdUnsharp.Text = "Unsharp";
			// 
			// rdTopHat
			// 
			this.rdTopHat.Location = new System.Drawing.Point(132, 80);
			this.rdTopHat.Name = "rdTopHat";
			this.rdTopHat.Size = new System.Drawing.Size(104, 16);
			this.rdTopHat.TabIndex = 7;
			this.rdTopHat.Tag = "TopHat";
			this.rdTopHat.Text = "Top Hat";
			// 
			// rdCustom
			// 
			this.rdCustom.Location = new System.Drawing.Point(12, 162);
			this.rdCustom.Name = "rdCustom";
			this.rdCustom.Size = new System.Drawing.Size(72, 20);
			this.rdCustom.TabIndex = 12;
			this.rdCustom.Tag = "EnCustom";
			this.rdCustom.Text = "Custom...";
			// 
			// rdHighPass
			// 
			this.rdHighPass.Location = new System.Drawing.Point(12, 20);
			this.rdHighPass.Name = "rdHighPass";
			this.rdHighPass.Size = new System.Drawing.Size(104, 16);
			this.rdHighPass.TabIndex = 0;
			this.rdHighPass.Tag = "HighPass";
			this.rdHighPass.Text = "High-Pass";
			// 
			// rdHorizontalEdge
			// 
			this.rdHorizontalEdge.Location = new System.Drawing.Point(12, 40);
			this.rdHorizontalEdge.Name = "rdHorizontalEdge";
			this.rdHorizontalEdge.Size = new System.Drawing.Size(104, 16);
			this.rdHorizontalEdge.TabIndex = 2;
			this.rdHorizontalEdge.Tag = "HorizontalEdge";
			this.rdHorizontalEdge.Text = "Horizontal Edge";
			// 
			// rdLaplace
			// 
			this.rdLaplace.Location = new System.Drawing.Point(12, 60);
			this.rdLaplace.Name = "rdLaplace";
			this.rdLaplace.Size = new System.Drawing.Size(104, 16);
			this.rdLaplace.TabIndex = 4;
			this.rdLaplace.Tag = "Laplace";
			this.rdLaplace.Text = "Laplace";
			// 
			// rdWell
			// 
			this.rdWell.Location = new System.Drawing.Point(12, 80);
			this.rdWell.Name = "rdWell";
			this.rdWell.Size = new System.Drawing.Size(104, 16);
			this.rdWell.TabIndex = 6;
			this.rdWell.Tag = "Well";
			this.rdWell.Text = "Well";
			// 
			// rdScrupt
			// 
			this.rdScrupt.Location = new System.Drawing.Point(12, 100);
			this.rdScrupt.Name = "rdScrupt";
			this.rdScrupt.Size = new System.Drawing.Size(104, 16);
			this.rdScrupt.TabIndex = 8;
			this.rdScrupt.Tag = "Scrupt";
			this.rdScrupt.Text = "Scrupt";
			// 
			// rdHighGausian
			// 
			this.rdHighGausian.Checked = true;
			this.rdHighGausian.Location = new System.Drawing.Point(132, 100);
			this.rdHighGausian.Name = "rdHighGausian";
			this.rdHighGausian.Size = new System.Drawing.Size(104, 16);
			this.rdHighGausian.TabIndex = 9;
			this.rdHighGausian.TabStop = true;
			this.rdHighGausian.Tag = "HighGausian";
			this.rdHighGausian.Text = "High Gaussian";
			// 
			// rdHotPixel
			// 
			this.rdHotPixel.Location = new System.Drawing.Point(12, 120);
			this.rdHotPixel.Name = "rdHotPixel";
			this.rdHotPixel.Size = new System.Drawing.Size(104, 16);
			this.rdHotPixel.TabIndex = 10;
			this.rdHotPixel.Tag = "HotPixel";
			this.rdHotPixel.Text = "Hot Pixel";
			// 
			// rdDeadPixel
			// 
			this.rdDeadPixel.Location = new System.Drawing.Point(132, 120);
			this.rdDeadPixel.Name = "rdDeadPixel";
			this.rdDeadPixel.Size = new System.Drawing.Size(104, 16);
			this.rdDeadPixel.TabIndex = 11;
			this.rdDeadPixel.Tag = "DeadPixel";
			this.rdDeadPixel.Text = "Dead Pixel";
			// 
			// tabMorphology
			// 
			this.tabMorphology.Controls.Add(this.gMorOption);
			this.tabMorphology.Controls.Add(this.gMorFilter);
			this.tabMorphology.Location = new System.Drawing.Point(4, 22);
			this.tabMorphology.Name = "tabMorphology";
			this.tabMorphology.Size = new System.Drawing.Size(256, 274);
			this.tabMorphology.TabIndex = 1;
			this.tabMorphology.Text = "Morphology";
			// 
			// gMorOption
			// 
			this.gMorOption.Controls.Add(this.rd7_7Circle);
			this.gMorOption.Controls.Add(this.rd3_3Ring);
			this.gMorOption.Controls.Add(this.rd7_7Ring);
			this.gMorOption.Controls.Add(this.rd3_3Sq);
			this.gMorOption.Controls.Add(this.rd5_5Circle);
			this.gMorOption.Controls.Add(this.label4);
			this.gMorOption.Controls.Add(this.ndMorPass);
			this.gMorOption.Controls.Add(this.rd3_3Cross);
			this.gMorOption.Controls.Add(this.rd5_5Ring);
			this.gMorOption.Controls.Add(this.rd2_2sq);
			this.gMorOption.Location = new System.Drawing.Point(8, 108);
			this.gMorOption.Name = "gMorOption";
			this.gMorOption.Size = new System.Drawing.Size(240, 144);
			this.gMorOption.TabIndex = 1;
			this.gMorOption.TabStop = false;
			this.gMorOption.Tag = "gMorOption";
			this.gMorOption.Text = "Options";
			// 
			// rd7_7Circle
			// 
			this.rd7_7Circle.Location = new System.Drawing.Point(132, 88);
			this.rd7_7Circle.Name = "rd7_7Circle";
			this.rd7_7Circle.Size = new System.Drawing.Size(104, 20);
			this.rd7_7Circle.TabIndex = 7;
			this.rd7_7Circle.Tag = "7x7Circle";
			this.rd7_7Circle.Text = "7 x 7 Circle";
			// 
			// rd3_3Ring
			// 
			this.rd3_3Ring.Location = new System.Drawing.Point(132, 40);
			this.rd3_3Ring.Name = "rd3_3Ring";
			this.rd3_3Ring.Size = new System.Drawing.Size(104, 20);
			this.rd3_3Ring.TabIndex = 3;
			this.rd3_3Ring.Tag = "3x3Ring ";
			this.rd3_3Ring.Text = "3 x 3 Ring";
			// 
			// rd7_7Ring
			// 
			this.rd7_7Ring.Location = new System.Drawing.Point(132, 64);
			this.rd7_7Ring.Name = "rd7_7Ring";
			this.rd7_7Ring.Size = new System.Drawing.Size(104, 20);
			this.rd7_7Ring.TabIndex = 5;
			this.rd7_7Ring.Tag = "7x7Ring";
			this.rd7_7Ring.Text = "7 x 7 Ring";
			// 
			// rd3_3Sq
			// 
			this.rd3_3Sq.Location = new System.Drawing.Point(132, 16);
			this.rd3_3Sq.Name = "rd3_3Sq";
			this.rd3_3Sq.Size = new System.Drawing.Size(104, 20);
			this.rd3_3Sq.TabIndex = 1;
			this.rd3_3Sq.Tag = "3x3Sq";
			this.rd3_3Sq.Text = "3 x 3 Square";
			// 
			// rd5_5Circle
			// 
			this.rd5_5Circle.Location = new System.Drawing.Point(8, 88);
			this.rd5_5Circle.Name = "rd5_5Circle";
			this.rd5_5Circle.Size = new System.Drawing.Size(104, 20);
			this.rd5_5Circle.TabIndex = 6;
			this.rd5_5Circle.Tag = "5x5Circle";
			this.rd5_5Circle.Text = "5 x 5 Circle";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 116);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(44, 20);
			this.label4.TabIndex = 8;
			this.label4.Text = "Pass:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// ndMorPass
			// 
			this.ndMorPass.Location = new System.Drawing.Point(60, 116);
			this.ndMorPass.Maximum = new System.Decimal(new int[] {
										      20,
										      0,
										      0,
										      0});
			this.ndMorPass.Minimum = new System.Decimal(new int[] {
										      1,
										      0,
										      0,
										      0});
			this.ndMorPass.Name = "ndMorPass";
			this.ndMorPass.Size = new System.Drawing.Size(68, 20);
			this.ndMorPass.TabIndex = 9;
			this.ndMorPass.Value = new System.Decimal(new int[] {
										    1,
										    0,
										    0,
										    0});
			// 
			// rd3_3Cross
			// 
			this.rd3_3Cross.Location = new System.Drawing.Point(8, 40);
			this.rd3_3Cross.Name = "rd3_3Cross";
			this.rd3_3Cross.Size = new System.Drawing.Size(104, 20);
			this.rd3_3Cross.TabIndex = 2;
			this.rd3_3Cross.Tag = "3x3Cross";
			this.rd3_3Cross.Text = "3 x 3 Cross";
			// 
			// rd5_5Ring
			// 
			this.rd5_5Ring.Location = new System.Drawing.Point(8, 64);
			this.rd5_5Ring.Name = "rd5_5Ring";
			this.rd5_5Ring.Size = new System.Drawing.Size(104, 20);
			this.rd5_5Ring.TabIndex = 4;
			this.rd5_5Ring.Tag = "5x5Ring";
			this.rd5_5Ring.Text = "5 x 5 Ring";
			// 
			// rd2_2sq
			// 
			this.rd2_2sq.Checked = true;
			this.rd2_2sq.Location = new System.Drawing.Point(8, 16);
			this.rd2_2sq.Name = "rd2_2sq";
			this.rd2_2sq.Size = new System.Drawing.Size(104, 20);
			this.rd2_2sq.TabIndex = 0;
			this.rd2_2sq.TabStop = true;
			this.rd2_2sq.Tag = "2x2sq";
			this.rd2_2sq.Text = "2 x 2 Square";
			// 
			// gMorFilter
			// 
			this.gMorFilter.Controls.Add(this.rdOpening);
			this.gMorFilter.Controls.Add(this.rdDilation);
			this.gMorFilter.Controls.Add(this.rdClosing);
			this.gMorFilter.Controls.Add(this.rdMoCustom);
			this.gMorFilter.Controls.Add(this.rdErosion);
			this.gMorFilter.Controls.Add(this.rdFlattenBackground);
			this.gMorFilter.Location = new System.Drawing.Point(4, 4);
			this.gMorFilter.Name = "gMorFilter";
			this.gMorFilter.Size = new System.Drawing.Size(248, 96);
			this.gMorFilter.TabIndex = 0;
			this.gMorFilter.TabStop = false;
			this.gMorFilter.Tag = "gMorFilter";
			this.gMorFilter.Text = "Filter";
			// 
			// rdOpening
			// 
			this.rdOpening.Location = new System.Drawing.Point(132, 44);
			this.rdOpening.Name = "rdOpening";
			this.rdOpening.Size = new System.Drawing.Size(104, 20);
			this.rdOpening.TabIndex = 3;
			this.rdOpening.Tag = "Opening";
			this.rdOpening.Text = "Opening";
			// 
			// rdDilation
			// 
			this.rdDilation.Location = new System.Drawing.Point(132, 20);
			this.rdDilation.Name = "rdDilation";
			this.rdDilation.Size = new System.Drawing.Size(104, 20);
			this.rdDilation.TabIndex = 1;
			this.rdDilation.Tag = "Dilation";
			this.rdDilation.Text = "Dilation";
			// 
			// rdClosing
			// 
			this.rdClosing.Location = new System.Drawing.Point(12, 44);
			this.rdClosing.Name = "rdClosing";
			this.rdClosing.Size = new System.Drawing.Size(104, 20);
			this.rdClosing.TabIndex = 2;
			this.rdClosing.Tag = "Closing";
			this.rdClosing.Text = "Closing";
			// 
			// rdMoCustom
			// 
			this.rdMoCustom.Enabled = false;
			this.rdMoCustom.Location = new System.Drawing.Point(132, 68);
			this.rdMoCustom.Name = "rdMoCustom";
			this.rdMoCustom.Size = new System.Drawing.Size(104, 20);
			this.rdMoCustom.TabIndex = 4;
			this.rdMoCustom.Tag = "MorCustom";
			this.rdMoCustom.Text = "Custom...";
			this.rdMoCustom.Visible = false;
			// 
			// rdErosion
			// 
			this.rdErosion.Checked = true;
			this.rdErosion.Location = new System.Drawing.Point(12, 20);
			this.rdErosion.Name = "rdErosion";
			this.rdErosion.Size = new System.Drawing.Size(104, 20);
			this.rdErosion.TabIndex = 0;
			this.rdErosion.TabStop = true;
			this.rdErosion.Tag = "Erosion";
			this.rdErosion.Text = "Erosion";
			// 
			// rdFlattenBackground
			// 
			this.rdFlattenBackground.Location = new System.Drawing.Point(12, 68);
			this.rdFlattenBackground.Name = "rdFlattenBackground";
			this.rdFlattenBackground.Size = new System.Drawing.Size(120, 20);
			this.rdFlattenBackground.TabIndex = 4;
			this.rdFlattenBackground.Tag = "MorFlattenBackground";
			this.rdFlattenBackground.Text = "Flatten Background";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(280, 36);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(88, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			// 
			// btnLoadSettings
			// 
			this.btnLoadSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.btnLoadSettings.Enabled = true;
			this.btnLoadSettings.Location = new System.Drawing.Point(280, 64);
			this.btnLoadSettings.Name = "btnLoadSettings";
			this.btnLoadSettings.Size = new System.Drawing.Size(88, 23);
			this.btnLoadSettings.TabIndex = 10;
			this.btnLoadSettings.Text = "Load Settings";
			this.btnLoadSettings.Click += new System.EventHandler(this.btnLoadSettings_Click);
			// 
			// btnSaveSettings
			// 
			this.btnSaveSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.btnSaveSettings.Location = new System.Drawing.Point(280, 92);
			this.btnSaveSettings.Name = "btnSaveSettings";
			this.btnSaveSettings.Size = new System.Drawing.Size(88, 23);
			this.btnSaveSettings.TabIndex = 11;
			this.btnSaveSettings.Text = "Save Settings";
			this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
			// 
			// DlgKernelFilter
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(378, 592);
			this.Controls.Add(this.btnSaveSettings);
			this.Controls.Add(this.btnLoadSettings);
			this.Controls.Add(this.tabOptions);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.grpPreview);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DlgKernelFilter";
			this.ShowInTaskbar = false;
			this.Text = "Kernel Filters";
			this.Load += new System.EventHandler(this.DlgKernelFilter_Load);
			this.grpPreview.ResumeLayout(false);
			this.tabOptions.ResumeLayout(false);
			this.tabEnhancement.ResumeLayout(false);
			this.grpConvolutionMatrixType.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.ndEnPass)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudTheshold)).EndInit();
			this.grpConvolutionType.ResumeLayout(false);
			this.tabMorphology.ResumeLayout(false);
			this.gMorOption.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.ndMorPass)).EndInit();
			this.gMorFilter.ResumeLayout(false);
			this.ResumeLayout(false);

		}        
		#endregion

		#region event handlers

		private void OnRadioButtonCheckChange(object sender, System.EventArgs e)
		{	
			if(!((RadioButton)sender).Checked) return;

			switch(((RadioButton)sender).Parent.Tag.ToString())
			{
				case "gEnFilter":
					curEnFilterRd = ((RadioButton)sender);	
					UpdateConvolutionTab(curEnFilterRd);
					break;
				case "gEnOption":
					curEnOptionRd = ((RadioButton)sender);	
					//					UpdateConvolutionTab(curEnOptionRd);
					break;
				case "gMorFilter":
					curMorFilterRd = ((RadioButton)sender);	
					UpdateMorphologyTab(curMorOptionRd);
					break;
				case "gMorOption":
					curMorOptionRd = ((RadioButton)sender);	
					break;				
			}

			if( ((RadioButton)sender).Tag.ToString() == "MorCustom" )
			{

			}

			this.btnCusFilter.Enabled = ((RadioButton)sender == this.rdCustom);			
		}
		
		private void btnCusFilter_Click(object sender, System.EventArgs e)
		{
			if (!rdCustom.Checked)
				rdCustom.Checked = true;
			
			using (DlgCustomFilter dlg = new DlgCustomFilter())
			{
				try
				{
                    //dlg.MatrixArr = this.CustomMatrix;
					if (dlg.ShowDialog() == DialogResult.OK)
					{	
						arrEnFilter = dlg.MatrixArr;
						arrEnFilterValidate = dlg.MatrixValidation;
					}
				}
				catch(System.Exception exp)
				{
					throw exp;
				}
				finally
				{
				}
			}
		}

		private void btnPreview_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (ValidateInputParameters()==true)
				{
					this.ApplyToPreview();
				}
			}
			catch(Exception ex)
			{
				Trace.WriteLine(ex.Message);
			}
		}

		private void btnReset_Click(object sender, System.EventArgs e)
		{
			this.ResetPreview();		
		}

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			if (ValidateInputParameters())
			{
				this.DialogResult = DialogResult.OK;
			}
		}

		private void DlgKernelFilter_Load(object sender, System.EventArgs e)
		{
			/* set default selected */
			rdHighPass.Checked = true;
			rdErosion.Checked = true;

			curEnFilterRd	= rdHighGausian;
			curEnOptionRd	= rd3_3;
			curMorFilterRd	= rdErosion;
			curMorOptionRd	= rd2_2sq;	

			/* initialize preview user control*/
			//_imagePreview.CommonImage = this.CommomImage;
			//_imagePreview.RefreshCommonImage();
		}

        void tabOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnCusFilter.Enabled = rdCustom.Checked;
        }

		private void btnLoadSettings_Click(object sender, System.EventArgs e)
		{
            try
            {
                LoadAsXml();
            }
            catch (System.Exception exp)
            {
                MessageBoxEx.Error("Failed to load settings: " + exp.Message);
            }
		}

		private void btnSaveSettings_Click(object sender, System.EventArgs e)
		{			
			try
			{
				SaveAsXml();
			}
			catch (System.Exception exp)
			{
				MessageBoxEx.Error("Failed to save settings: " + exp.Message);
			}
		}
		#endregion

		#region override routines

		#region DialogPreviewBase override
		public override SIA.UI.Components.ImagePreview GetPreviewer()
		{
			return _imagePreview;
		}

		public override void ApplyToCommonImage(SIA.SystemLayer.CommonImage imagePreview)
		{			
			try
			{
				kUtils.kBeginWaitCursor();

				bool ConvolutionSelected = (tabOptions.SelectedIndex==0);
				eMaskType convType = this.Selected_ConvolutionType;
				eMatrixType convMatrixType = this.Selected_ConvolutionMatrixType;
				eMorphType morphType = this.Selected_MorphologyType;
				eMatrixType morphMatrixType = this.Selected_MorphologyMatrixType;
				
				if (ConvolutionSelected)	
				{
					if (convType != eMaskType.kMASK_UNKNOWN)
					{
						//imagePreview.kApplyFilter(convType, convMatrixType, (int)ndEnPass.Value);
						imagePreview.kApplyFilter(convType, convMatrixType, (int)ndEnPass.Value,this.Threshold);
					}
					else
					{
						if (this.CustomMatrix != null)
							imagePreview.kApplyFilter(this.CustomMatrix, (int)ndEnPass.Value);
					}
				}
				else
					imagePreview.kApplyFilter(morphType, morphMatrixType, (int)ndMorPass.Value);

				kUtils.kEndWaitCursor();
			} 
			catch (Exception e)
			{
				MessageBoxEx.Error(e.Message);
			}			
		}
		

		protected override void LockUserInputObjects()
		{
		}

		protected override void UnlockUserInputObjects()
		{
		}

		#endregion

		#region DialogBase override

		protected override object OnGetDefaultValue(Control ctrl)
		{
			// tab Enhancement
			if (ctrl == rdHighPass) 
				return true;
			else if (ctrl == rdLowPass)
				return false;
			else if (ctrl == rdHorizontalEdge)
				return false;
			else if (ctrl == rdVerticalEdge)
				return false;
			else if (ctrl == rdLaplace)
				return false;
			else if (ctrl == rdUnsharp)
				return false;
			else if (ctrl == rdWell)
				return false;
			else if (ctrl == rdTopHat)
				return false;
			else if (ctrl == rdScrupt)
				return false;
			else if (ctrl == rdHighGausian)
				return false;
			else if (ctrl == rdHotPixel)
				return false;
			else if (ctrl == rdDeadPixel)
				return false;
			else if (ctrl == rdHorizontalEdge)
				return false;
			else if (ctrl == rdMedian)
				return false;
			else if (ctrl == rdCustom)
				return false;

				// tab Options
			else if (ctrl == rd3_3)
				return true;
			else if (ctrl == rd5_5)
				return false;
			else if (ctrl == rd7_7)
				return false;
			else if (ctrl == ndEnPass)
				return (Decimal)1.0F;
			else if (ctrl == nudTheshold)
				return (Decimal)50.0F;

				// tab Morphology
			else if (ctrl == rdErosion)
				return true;
			else if (ctrl == rdDilation)
				return false;
			else if (ctrl == rdClosing)
				return false;
			else if (ctrl == rdOpening)
				return false;
			else if (ctrl == rdFlattenBackground)
				return false;
			else if (ctrl == rdCustom)
				return false;

				// tab Options
			else if (ctrl == rd2_2sq)
				return true;
			else if (ctrl == rd3_3Sq)
				return false;
			else if (ctrl == rd3_3Cross)
				return false;
			else if (ctrl == rd3_3Ring)
				return false;
			else if (ctrl == rd5_5Ring)
				return false;
			else if (ctrl == rd7_7Ring)
				return false;
			else if (ctrl == rd5_5Circle)
				return false;
			else if (ctrl == rd7_7Circle)
				return false;
			else if (ctrl == ndMorPass)
				return (Decimal)1.0F;

			return null;
		}


		#endregion

		#endregion

		#region internal helpers
		public void InitRadioButtonEvent()
		{
			AddRadioButtonEvent(this);
		}

		private void AddRadioButtonEvent(Control control)
		{
			foreach(Control ctrl in control.Controls )
			{
				if ( ctrl.GetType() == typeof(RadioButton))
				{
					((RadioButton)ctrl).CheckedChanged += new EventHandler(this.OnRadioButtonCheckChange);		
				}
				if ( ctrl.Controls.Count > 0 ) AddRadioButtonEvent(ctrl);
			}
		}
		
		private void ShowEnFilterDlg()
		{
		}

		private void ShowMorFilterDlg()
		{	
		}

		private bool ValidateInputParameters()
		{	
			/* retrieve selected tab */
			if (tabOptions.SelectedTab == tabEnhancement)
			{
				if (curEnFilterRd.Tag.ToString() == "EnCustom" )
				{
					if ( !arrEnFilterValidate ) 
					{
						MessageBoxEx.Error("Invalid custom filter");
						return false;
					}
				}

				if (!ndEnPass.Validate())
				{
					MessageBoxEx.Error("Invalid number of pass");
					ndEnPass.Select();
					return false;
				}
			}
			else if (tabOptions.SelectedTab == tabMorphology)
			{
				if ( curMorFilterRd.Tag.ToString() == "MorCustom" )
				{
					if ( !arrMorFilterValidate ) 
					{
						MessageBoxEx.Error("Morphology custom matrix is invalid.");
						return false;
					}
				}

				if (!ndMorPass.Validate())
				{
					MessageBoxEx.Error("Number of Pass is invalid.");
					ndMorPass.Select();
					return false;
				}
			}


			return true;
		}
		
		private bool IsInputNumeric( string text)
		{
			try
			{
				System.Convert.ToDouble( text);
				return true;
			}
			catch
			{
				return false;
			}
		}

		RadioButton GetRadioButtonOf(eMaskType maskType)
		{
			RadioButton result = null;
			switch (maskType)
			{
				case eMaskType.kMASK_HIGHGAUSS:		
					result = rdHighGausian;
					break;
				case eMaskType.kMASK_HIGHPASS:		
					result = rdHighPass;
					break;
				case eMaskType.kMASK_LOWPASS:	
					result = rdLowPass;
					break;
				case eMaskType.kMASK_HORZEDGE:	
					result = rdHorizontalEdge;
					break;
				case eMaskType.kMASK_VERTEDGE:	
					result = rdVerticalEdge;
					break;
				case eMaskType.kMASK_LAPLACE:	
					result = rdLaplace;
					break;
				case eMaskType.kMASK_UNSHARP:	
					result = rdUnsharp;
					break;
				case eMaskType.kMASK_WELL:		
					result = rdWell;
					break;
				case eMaskType.kMASK_TOPHAT:	
					result = rdTopHat;
					break;
				case eMaskType.kMASK_SCULPT:
					result = rdScrupt;
					break;
				case eMaskType.kMASK_HOTPIXEL:
					result = rdHotPixel;
					break;
				case eMaskType.kMASK_DEADPIXEL:
					result = rdDeadPixel;
					break;
				case eMaskType.kMASK_MEDIAN:
					result = rdMedian;
					break;
                case eMaskType.kMASK_UNKNOWN:
                    result = rdCustom;
                    break;
				default:
					break;
			}
			return result;
		}

		private RadioButton GetRadioButtonOf(eMatrixType matrixType)
		{
			RadioButton result = null;
			switch (matrixType)
			{
				case eMatrixType.kMATRIX_3x3:
					result = rd3_3;
					break;
				case eMatrixType.kMATRIX_5x5:
					result = rd5_5;
					break;
				case eMatrixType.kMATRIX_7x7:
					result = rd7_7;
					break;
				case eMatrixType.kMATRIX_CIRCLE5x5:
					result = rd5_5Circle;
					break;
				case eMatrixType.kMATRIX_CIRCLE7x7:
					result = rd7_7Circle;
					break;
				case eMatrixType.kMATRIX_CROSS3x3:
					result = rd3_3Cross;
					break;
				case eMatrixType.kMATRIX_RING3x3:
					result = rd3_3Ring;
					break;
				case eMatrixType.kMATRIX_RING5x5:
					result = rd5_5Ring;
					break;
				case eMatrixType.kMATRIX_RING7x7:
					result = rd7_7Ring;
					break;
				case eMatrixType.kMATRIX_SQUARE2x2:
					result = rd2_2sq;
					break;
				case eMatrixType.kMATRIX_SQUARE3x3:
					result = rd3_3Sq;
					break;
			}
			return result;
		}

		private RadioButton GetRadioButtonOf(eMorphType morphType)
		{
			RadioButton result = null;
			switch (morphType)
			{
				case eMorphType.kMORPH_FLAT_EROSION:
					result = rdErosion;
					break;
				case eMorphType.kMORPH_FLAT_DILATION:
					result = rdDilation;
					break;
				case eMorphType.kMORPH_FLAT_CLOSING:
					result = rdClosing;
					break;
				case eMorphType.kMORPH_FLAT_OPENING:
					result = rdOpening;
					break;
                case eMorphType.kMORPH_FLATTEN:
                    result = rdFlattenBackground;
                    break;
			}
			return result;
		}

		private eMaskType GetSelectedConvolutionType()
		{
			eMaskType result = eMaskType.kMASK_UNKNOWN;
			if (rdHighGausian.Checked == true)
				result = eMaskType.kMASK_HIGHGAUSS;
			else if (rdHighPass.Checked == true)
				result = eMaskType.kMASK_HIGHPASS;
			else if (rdLowPass.Checked == true)
				result = eMaskType.kMASK_LOWPASS;
			else if (rdHorizontalEdge.Checked == true)
				result = eMaskType.kMASK_HORZEDGE;
			else if (rdVerticalEdge.Checked == true)
				result = eMaskType.kMASK_VERTEDGE;
			else if (rdLaplace.Checked == true)
				result = eMaskType.kMASK_LAPLACE;
			else if (rdUnsharp.Checked == true)
				result = eMaskType.kMASK_UNSHARP;
			else if (rdWell.Checked == true)
				result = eMaskType.kMASK_WELL;
			else if (rdTopHat.Checked == true)
				result = eMaskType.kMASK_TOPHAT;
			else if (rdScrupt.Checked == true)
				result = eMaskType.kMASK_SCULPT;
			else if (rdHotPixel.Checked == true)
				result = eMaskType.kMASK_HOTPIXEL;
			else if (rdDeadPixel.Checked == true)
				result = eMaskType.kMASK_DEADPIXEL;
			else if(rdMedian.Checked == true)
				result = eMaskType.kMASK_MEDIAN;
				
			return result;
		}

		private eMatrixType GetSelectedConvolutionMatrixType()
		{
			eMatrixType result = eMatrixType.kMATRIX_UNKNOWN;
			if (rd3_3.Checked)
				result = eMatrixType.kMATRIX_3x3;
			else if (rd5_5.Checked)
				result = eMatrixType.kMATRIX_5x5;
			else if (rd7_7.Checked)
				result = eMatrixType.kMATRIX_7x7;
			return result;
		}

		private eMorphType GetSelectedMorphologyType()
		{
			eMorphType result = eMorphType.kMORPH_UNKNOWN;
			if (rdErosion.Checked)
				result = eMorphType.kMORPH_FLAT_EROSION;
			else if (rdDilation.Checked)
				result = eMorphType.kMORPH_FLAT_DILATION;
			else if (rdClosing.Checked)
				result = eMorphType.kMORPH_FLAT_CLOSING;
			else if (rdOpening.Checked)
				result = eMorphType.kMORPH_FLAT_OPENING;
			else if (rdFlattenBackground.Checked)
				result = eMorphType.kMORPH_FLATTEN;
			return result;
		}

		private eMatrixType GetSelectedMorphologyMatrixType()
		{
			eMatrixType result = eMatrixType.kMATRIX_UNKNOWN;
			if (rd5_5Circle.Checked)
				result = eMatrixType.kMATRIX_CIRCLE5x5;
			if (rd7_7Circle.Checked)
				result = eMatrixType.kMATRIX_CIRCLE7x7;
			if (rd3_3Cross.Checked)
				result = eMatrixType.kMATRIX_CROSS3x3;
			if (rd3_3Ring.Checked)
				result = eMatrixType.kMATRIX_RING3x3;
			if (rd5_5Ring.Checked)
				result = eMatrixType.kMATRIX_RING5x5;
			if (rd7_7Ring.Checked)
				result = eMatrixType.kMATRIX_RING7x7;
			if (rd2_2sq.Checked)
				result = eMatrixType.kMATRIX_SQUARE2x2;
			if (rd3_3Sq.Checked)
				result = eMatrixType.kMATRIX_SQUARE3x3;
			return result;
		}

		public eMaskType Selected_ConvolutionType
		{
			get 
			{
				return GetSelectedConvolutionType();
			}
			set
			{
				RadioButton radioButton = GetRadioButtonOf(value);
				if (radioButton!=null)
					radioButton.Checked = true;
			}
		}

		public eMatrixType Selected_ConvolutionMatrixType
		{
			get
			{
				return GetSelectedConvolutionMatrixType();
			}
			set
			{			
				RadioButton radioButton = GetRadioButtonOf(value);
				if (radioButton!=null)
					radioButton.Checked = true;
			}
		}

		public eMorphType Selected_MorphologyType
		{
			get
			{
				return GetSelectedMorphologyType();
			}
			set
			{			
				RadioButton radioButton = GetRadioButtonOf(value);
				if (radioButton!=null)
					radioButton.Checked = true;
			}
		}

		public eMatrixType Selected_MorphologyMatrixType
		{
			get
			{
				return GetSelectedMorphologyMatrixType();
			}
			set
			{			
				RadioButton radioButton = GetRadioButtonOf(value);
				if (radioButton!=null)
					radioButton.Checked = true;
			}
		}

		private void UpdateConvolutionTab(RadioButton sender)
		{
			eMaskType convoleType = this.Selected_ConvolutionType;
			if (convoleType != eMaskType.kMASK_UNKNOWN)
			{
				rd3_3.Enabled = false;
				rd5_5.Enabled = false;
				rd7_7.Enabled = false;
				ndEnPass.Enabled = false;
				nudTheshold.Enabled = false;

				if (convoleType==eMaskType.kMASK_HOTPIXEL || convoleType==eMaskType.kMASK_DEADPIXEL)
				{
					nudTheshold.Enabled = true;
				}
				else
				{
					eMatrixType matrixType = this.Selected_ConvolutionMatrixType;
					RadioButton selectedOption = this.GetRadioButtonOf(matrixType);
				
					eMatrixType[] matrixTypes = SIA.SystemLayer.kKernelFilters.QueryMatrixTypeSupported(convoleType);
					foreach (eMatrixType type in matrixTypes)
					{
						RadioButton radioButton = this.GetRadioButtonOf(type);
						if (radioButton!=null) 
							radioButton.Enabled = true;
					}

					if (!selectedOption.Enabled)
					{
						if (rd3_3.Enabled) rd3_3.Checked = true;
						else if (rd5_5.Enabled) rd5_5.Checked = true;
						else if (rd7_7.Enabled) rd7_7.Checked = true;
					}
					ndEnPass.Enabled = true;
				}
			}
			else if (sender == rdCustom)
			{
				rd3_3.Enabled = false;
				rd5_5.Enabled = false;
				rd7_7.Enabled = false;

                ndEnPass.Enabled = true;
                nudTheshold.Enabled = false;
			}
		}

		private void UpdateMorphologyTab(RadioButton sender)
		{
			eMorphType morphType = this.Selected_MorphologyType;
			if (morphType != eMorphType.kMORPH_UNKNOWN)
			{
				rd2_2sq.Enabled = true;
				rd3_3Sq.Enabled = true;
				rd3_3Cross.Enabled = true;
				rd5_5Circle.Enabled = true;
				rd7_7Circle.Enabled = true;
				rd3_3Ring.Enabled = true;
				rd5_5Ring.Enabled = true;
				rd7_7Ring.Enabled = true;
			}
			else if (sender == rdMoCustom)
			{
				rd2_2sq.Enabled = false;
				rd3_3Sq.Enabled = false;
				rd3_3Cross.Enabled = false;
				rd5_5Circle.Enabled = false;
				rd7_7Circle.Enabled = false;
				rd3_3Ring.Enabled = false;
				rd5_5Ring.Enabled = false;
				rd7_7Ring.Enabled = false;
			}
		}

		private void SaveAsXml()
		{
			try
			{
				using (SaveFileDialog dlg = CommonDialogs.SaveXmlFileDialog("Save Settings"))
				{
					dlg.FileName = "Untitled";

					if (dlg.ShowDialog() == DialogResult.OK)
					{
						if (File.Exists(dlg.FileName.ToString()))
						{
							System.IO.FileAttributes fileAttribs =System.IO.File.GetAttributes(dlg.FileName.ToString()); 
							if((fileAttribs & System.IO.FileAttributes.ReadOnly) == System.IO.FileAttributes.ReadOnly)
							{					
								MessageBoxEx.Error("The file is read only and can not be overridden.");
								return ;
							} 
						}
						String filename = dlg.FileName;
					
						XmlSerialize(filename);
					}
				}
			}
			catch(System.Exception exp)
			{
				throw exp;
			}
		}

		private void XmlSerialize(String fileName)
		{			
			// retrieve filter type
			eKernelFilterType filterType = eKernelFilterType.Morphology;
			if (this.IsApplyConvolution)
			{
				if (this.Selected_ConvolutionType == eMaskType.kMASK_UNKNOWN)
					filterType = eKernelFilterType.CustConvolution;
				else
					filterType = eKernelFilterType.Convolution;
			}

			// initialize data
			KernelFilterCommandSettings cmdSettings = null;

			try
			{
				switch (filterType)
				{
					case eKernelFilterType.Convolution:
						cmdSettings = new KernelFilterCommandSettings(new ConvolutionCommandSettings(this.Selected_ConvolutionType, this.Selected_ConvolutionMatrixType, this.EnPass, this.Threshold));
						break;
					case eKernelFilterType.CustConvolution:
						cmdSettings = new KernelFilterCommandSettings(new CustConvolutionCommandSettings(this.CustomMatrix, this.EnPass));
						break;
					case eKernelFilterType.Morphology:
						cmdSettings = new KernelFilterCommandSettings(new MorphologyCommandSettings(this.Selected_MorphologyType, this.Selected_MorphologyMatrixType, this.MorPass));
						break;
				}

				// call raster command settings serializer to serialize
				if (cmdSettings != null)
				{
					SIA.UI.Controls.Automation.RasterCommandSettingsSerializer.Serialize(fileName, cmdSettings);
				}
			}
			finally
			{
				if (cmdSettings != null)
					cmdSettings.Dispose();
			}
		}

        private void LoadAsXml()
        {
            try
            {
                using (OpenFileDialog dlg = CommonDialogs.OpenXmlFileDialog("Load Settings"))
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        String filename = dlg.FileName;

                        XmlDeserialize(filename);
                    }
                }
            }
            catch (System.Exception exp)
            {
                throw exp;
            }
        }

        private void XmlDeserialize(String fileName)
        {
            KernelFilterCommandSettings cmdSettings = null;
            try
            {
                cmdSettings =
                    (KernelFilterCommandSettings)SIA.UI.Controls.Automation.RasterCommandSettingsSerializer.Deserialize(
                    fileName, typeof(KernelFilterCommandSettings));

                if (cmdSettings != null)
                {
                    if (cmdSettings.MorphologySettings != null)
                    {
                        this.IsApplyConvolution = false;

                        this.MorPass = cmdSettings.MorphologySettings.NumPass;
                        this.Selected_MorphologyType = cmdSettings.MorphologySettings.MaskType;
                        this.Selected_MorphologyMatrixType = cmdSettings.MorphologySettings.MatrixType;                                               
                    }
                    else
                    {
                        this.IsApplyConvolution = true;

                        if (cmdSettings.Convolution != null)
                        {
                            this.EnPass = cmdSettings.Convolution.NumPass;
                            this.Threshold = cmdSettings.Convolution.Threshold;
                            this.Selected_ConvolutionType = cmdSettings.Convolution.MaskType;
                            this.Selected_ConvolutionMatrixType = cmdSettings.Convolution.MatrixType;
                        }
                        else if (cmdSettings.CustConvolution != null)
                        {
                            this.EnPass = cmdSettings.CustConvolution.NumPass;
                            ArrayList m = cmdSettings.CustConvolution.Matrix;
                            if (m != null)
                            {
                                int n = m.Count;
                                int d = (int)Math.Sqrt(n);
                                int[,] a = new int[d, d];
                                for (int y = 0; y < d; y++)
                                {
                                    for (int x = 0; x < d; x++)
                                    {
                                        a[y, x] = (int)m[y * d + x];
                                    }
                                }
                                this.CustomMatrix = a;
                            }
                            else
                            {
                                this.CustomMatrix = new int[3, 3];
                            }

                            //rdCustom.Checked = true;
                            this.Selected_ConvolutionType = eMaskType.kMASK_UNKNOWN;
                        }                        
                    }
                }
            }
            finally
            {
                if (cmdSettings != null)
                {
                    cmdSettings.Dispose();
                    cmdSettings = null;
                }
            }
        }
		#endregion		
	}
}
