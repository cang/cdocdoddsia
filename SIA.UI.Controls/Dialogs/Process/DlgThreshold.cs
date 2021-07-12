using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using SIA.SystemLayer;
using SIA.IPEngine;

using SIA.UI.Controls;
using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.Utilities;
using SIA.UI.Controls.Automation.Commands;

using SIA.Plugins.Common;

namespace SIA.UI.Controls.Dialogs
{
	/// <summary>
	/// Name : DlgThreshold
	/// Description : User interface for Stretch Color
	/// Thread Support : True
	/// Persistence Data : True
	/// </summary>
	public class DlgThreshold : DialogPreviewBase
	{
		#region Windows Form members
		private System.Windows.Forms.GroupBox groupBoxPreview;
		private SIA.UI.Components.ImagePreview _imagePreview;
		private System.Windows.Forms.Button button_Cancel;
		private System.Windows.Forms.Button button_OK;
		private System.Windows.Forms.CheckBox chkZeroMaximum;
		private System.Windows.Forms.NumericUpDown nudMaximum;
		private System.Windows.Forms.CheckBox chkMaximum;
		private System.Windows.Forms.CheckBox chkMinimum;
		private System.Windows.Forms.CheckBox chkZeroMinimum;
		private System.Windows.Forms.NumericUpDown nudMinimum;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.Button btnPreview;
		private System.Windows.Forms.RadioButton radIntensity;
		private System.Windows.Forms.RadioButton radPercentile;
		private System.Windows.Forms.CheckBox chkRemoveDeadPixel;
		private System.Windows.Forms.NumericUpDown nudHotPixel;
		private System.Windows.Forms.CheckBox chkRemoveHotPixel;
		private System.Windows.Forms.NumericUpDown nudDeadPixel;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox chkZeroDead;
		private System.Windows.Forms.CheckBox chkZeroHot;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#endregion

		#region member attributes
		
		CommonImage _image = null;		
		kHistogram _histogram = null;
		double _totalIntensity = -1;
		private System.Windows.Forms.Button btnDefaultSettings;
		private System.Windows.Forms.Button btnLoadSettings;
		private System.Windows.Forms.Button btnSaveSettings;
		private System.Windows.Forms.Label lbMax;
		private System.Windows.Forms.Label lbMin;
		private System.Windows.Forms.Label lbHot;
		private System.Windows.Forms.Label lbDead;

		ThresholdCommandSettings _settings = null;
		#endregion

		#region public properties
		public bool ZeroMaximum
		{
			get {return chkMaximum.Checked && chkZeroMaximum.Checked;}
		}

		public bool ZeroMinimum
		{
			get {return chkMinimum.Checked && chkZeroMinimum.Checked;}
		}

//		public double Maximum
//		{
//			get { return m_Maximum; }
//			set { m_Maximum = value; }
//		}
//
//		public double Minimum
//		{
//			get {return m_Minimum;}
//			set {m_Minimum = value;}
//		}

		public bool UseMinimum
		{
			get
			{
				return this.chkMinimum.Checked;
			}

			set
			{
				chkMinimum.Checked = value;
				OnMinmumCheckedChanged();
			}
		}

		private void OnMinmumCheckedChanged()
		{
			nudMinimum.Enabled = chkZeroMinimum.Enabled = chkMinimum.Checked;
		}

		public bool UseMaximum
		{
			get
			{
				return this.chkMaximum.Checked;
			}

			set
			{
				chkMaximum.Checked = value;
				OnMinmumCheckedChanged();
			}
		}

		private void OnMaximumCheckedChanged()
		{
			nudMaximum.Enabled = chkZeroMaximum.Enabled = chkMaximum.Checked;
		}

		public bool MaxIntensityEnable
		{
			get 
			{
				return chkMaximum.Enabled;
			}

			set
			{
				chkMaximum.Enabled= value;
				nudMaximum.Enabled = chkZeroMaximum.Enabled = (chkMaximum.Enabled && chkMaximum.Checked);				
			}
		}

		public bool MinIntensityEnable
		{
			get 
			{
				return chkMinimum.Enabled;
			}

			set
			{
				chkMinimum.Enabled= value;				
				nudMinimum.Enabled = chkZeroMinimum.Enabled = (chkMinimum.Enabled && chkMinimum.Checked);
			}
		}

		public bool RemoveHotPixelEnable
		{
			get 
			{
				return chkRemoveHotPixel.Enabled;
			}

			set
			{
				chkRemoveHotPixel.Enabled= value;
				nudHotPixel.Enabled = chkZeroHot.Enabled = (chkRemoveHotPixel.Enabled && chkRemoveHotPixel.Checked);
			}
		}

		public bool RemoveDeadPixelEnable
		{
			get 
			{
				return chkRemoveDeadPixel.Enabled;
			}

			set
			{
				chkRemoveDeadPixel.Enabled= value;
				nudDeadPixel.Enabled = chkZeroDead.Enabled = (chkRemoveDeadPixel.Enabled && chkRemoveDeadPixel.Checked);
			}
		}

		public bool ThresholdByIntensity
		{
			get
			{
				return radIntensity.Checked;
			}

			set
			{
				radIntensity.Checked = value;
				MaxIntensityEnable = MinIntensityEnable = radIntensity.Checked;				
			}
		}

		public bool ThresholdByPercentile
		{
			get
			{
				return radPercentile.Checked;
			}

			set
			{
				radPercentile.Checked = value;
				RemoveHotPixelEnable = RemoveDeadPixelEnable = radPercentile.Checked;				
			}
		}

		public bool RemoveHotPixel
		{
			get
			{
				return chkRemoveHotPixel.Checked;
			}

			set
			{
				this.chkRemoveHotPixel.Checked = value;
				OnRemoveHotPixelCheckedChanged();
			}
		}

		private void OnRemoveHotPixelCheckedChanged()
		{
			nudHotPixel.Enabled = chkZeroHot.Enabled = chkRemoveHotPixel.Checked;
		}

		public bool RemoveDeadPixel
		{
			get
			{
				return chkRemoveDeadPixel.Checked;
			}

			set
			{
				this.chkRemoveDeadPixel.Checked = value;
				OnRemoveDeadPixelCheckedChanged();
			}
		}

		private void OnRemoveDeadPixelCheckedChanged()
		{
			nudDeadPixel.Enabled = chkZeroDead.Enabled = chkRemoveDeadPixel.Checked;
		}

		private float From
		{
			get 
			{ 
				if (chkRemoveDeadPixel.Checked)
					return (float)this.nudDeadPixel.Value;
				return 0;
			}			
		}

		private float To
		{
			get 
			{ 
				if (chkRemoveHotPixel.Checked)
					return (100.0f - (float)this.nudHotPixel.Value);
				return 100.0f;
			}			
		}
		#endregion

		#region constructor and destructor
		public DlgThreshold(IDocWorkspace owner) : base(owner, true)
		{
			InitializeComponent();

			InitClass();

            _image = owner.Image;

            nudMinimum.Minimum = nudMaximum.Minimum = 0;
			nudMaximum.Maximum = nudMaximum.Maximum = (int)Math.Min(ushort.MaxValue, _image.MaxGreyValue);
			
			this.AutoPreview = true;

            this.UpdateToControl(true);
		}

		public DlgThreshold(IDocWorkspace owner, ThresholdCommandSettings settings) : base(owner, true)
		{
			InitializeComponent();

			InitClass();
			
			_image = owner.Image;

			_settings = settings;			

			nudMinimum.Minimum = nudMaximum.Minimum = 0;
            nudMaximum.Maximum = nudMaximum.Maximum = (int)Math.Min(ushort.MaxValue, _image.MaxGreyValue);

            this.AutoPreview = true;

			this.UpdateToControl(true);
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

				if (_histogram != null)
				{
					_histogram.Dispose();
					_histogram = null;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgThreshold));
			this.groupBoxPreview = new System.Windows.Forms.GroupBox();
			this.btnReset = new System.Windows.Forms.Button();
			this._imagePreview = new SIA.UI.Components.ImagePreview();
			this.btnPreview = new System.Windows.Forms.Button();
			this.button_Cancel = new System.Windows.Forms.Button();
			this.button_OK = new System.Windows.Forms.Button();
			this.chkZeroMaximum = new System.Windows.Forms.CheckBox();
			this.nudMaximum = new System.Windows.Forms.NumericUpDown();
			this.chkMaximum = new System.Windows.Forms.CheckBox();
			this.chkMinimum = new System.Windows.Forms.CheckBox();
			this.chkZeroMinimum = new System.Windows.Forms.CheckBox();
			this.nudMinimum = new System.Windows.Forms.NumericUpDown();
			this.radIntensity = new System.Windows.Forms.RadioButton();
			this.radPercentile = new System.Windows.Forms.RadioButton();
			this.chkRemoveDeadPixel = new System.Windows.Forms.CheckBox();
			this.nudHotPixel = new System.Windows.Forms.NumericUpDown();
			this.chkRemoveHotPixel = new System.Windows.Forms.CheckBox();
			this.nudDeadPixel = new System.Windows.Forms.NumericUpDown();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lbDead = new System.Windows.Forms.Label();
			this.lbHot = new System.Windows.Forms.Label();
			this.lbMin = new System.Windows.Forms.Label();
			this.lbMax = new System.Windows.Forms.Label();
			this.chkZeroDead = new System.Windows.Forms.CheckBox();
			this.chkZeroHot = new System.Windows.Forms.CheckBox();
			this.btnDefaultSettings = new System.Windows.Forms.Button();
			this.btnLoadSettings = new System.Windows.Forms.Button();
			this.btnSaveSettings = new System.Windows.Forms.Button();
			this.groupBoxPreview.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudMaximum)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMinimum)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudHotPixel)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudDeadPixel)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBoxPreview
			// 
			this.groupBoxPreview.Controls.Add(this.btnReset);
			this.groupBoxPreview.Controls.Add(this._imagePreview);
			this.groupBoxPreview.Controls.Add(this.btnPreview);
			this.groupBoxPreview.Location = new System.Drawing.Point(6, 4);
			this.groupBoxPreview.Name = "groupBoxPreview";
			this.groupBoxPreview.Size = new System.Drawing.Size(358, 304);
			this.groupBoxPreview.TabIndex = 0;
			this.groupBoxPreview.TabStop = false;
			this.groupBoxPreview.Text = "Preview";
			// 
			// btnReset
			// 
			this.btnReset.Location = new System.Drawing.Point(8, 276);
			this.btnReset.Name = "btnReset";
			this.btnReset.Size = new System.Drawing.Size(64, 23);
			this.btnReset.TabIndex = 1;
			this.btnReset.Text = "Reset";
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// _imagePreview
			// 
			this._imagePreview.ImageViewer = null;
			this._imagePreview.Location = new System.Drawing.Point(8, 16);
			this._imagePreview.Name = "_imagePreview";
			this._imagePreview.PreviewRectangle = new System.Drawing.Rectangle(0, 0, 0, 0);
			this._imagePreview.Size = new System.Drawing.Size(344, 256);
			this._imagePreview.TabIndex = 0;
			// 
			// btnPreview
			// 
			this.btnPreview.Location = new System.Drawing.Point(80, 276);
			this.btnPreview.Name = "btnPreview";
			this.btnPreview.Size = new System.Drawing.Size(64, 23);
			this.btnPreview.TabIndex = 2;
			this.btnPreview.Text = "Preview";
			this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
			// 
			// button_Cancel
			// 
			this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button_Cancel.Location = new System.Drawing.Point(368, 36);
			this.button_Cancel.Name = "button_Cancel";
			this.button_Cancel.Size = new System.Drawing.Size(92, 23);
			this.button_Cancel.TabIndex = 3;
			this.button_Cancel.Text = "Cancel";
			// 
			// button_OK
			// 
			this.button_OK.Location = new System.Drawing.Point(368, 8);
			this.button_OK.Name = "button_OK";
			this.button_OK.Size = new System.Drawing.Size(92, 23);
			this.button_OK.TabIndex = 2;
			this.button_OK.Text = "OK";
			this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
			// 
			// chkZeroMaximum
			// 
			this.chkZeroMaximum.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.chkZeroMaximum.Location = new System.Drawing.Point(284, 44);
			this.chkZeroMaximum.Name = "chkZeroMaximum";
			this.chkZeroMaximum.Size = new System.Drawing.Size(68, 20);
			this.chkZeroMaximum.TabIndex = 2;
			this.chkZeroMaximum.Tag = "DEFAULT";
			this.chkZeroMaximum.Text = "Zero Out";
			// 
			// nudMaximum
			// 
			this.nudMaximum.Location = new System.Drawing.Point(168, 44);
			this.nudMaximum.Name = "nudMaximum";
			this.nudMaximum.Size = new System.Drawing.Size(56, 20);
			this.nudMaximum.TabIndex = 1;
			this.nudMaximum.Tag = "DEFAULT";
			this.nudMaximum.ValueChanged += new System.EventHandler(this.OnThreshold_Changed);
			// 
			// chkMaximum
			// 
			this.chkMaximum.Checked = true;
			this.chkMaximum.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkMaximum.Location = new System.Drawing.Point(32, 44);
			this.chkMaximum.Name = "chkMaximum";
			this.chkMaximum.Size = new System.Drawing.Size(132, 20);
			this.chkMaximum.TabIndex = 0;
			this.chkMaximum.Tag = "DEFAULT";
			this.chkMaximum.Text = "Maximum Threshold:";
			this.chkMaximum.CheckedChanged += new System.EventHandler(this.chkMaximum_CheckedChanged);
			// 
			// chkMinimum
			// 
			this.chkMinimum.Location = new System.Drawing.Point(32, 68);
			this.chkMinimum.Name = "chkMinimum";
			this.chkMinimum.Size = new System.Drawing.Size(132, 20);
			this.chkMinimum.TabIndex = 3;
			this.chkMinimum.Tag = "DEFAULT";
			this.chkMinimum.Text = "Minimum Threshold:";
			this.chkMinimum.CheckedChanged += new System.EventHandler(this.chkMinimum_CheckedChanged);
			// 
			// chkZeroMinimum
			// 
			this.chkZeroMinimum.Enabled = false;
			this.chkZeroMinimum.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.chkZeroMinimum.Location = new System.Drawing.Point(284, 68);
			this.chkZeroMinimum.Name = "chkZeroMinimum";
			this.chkZeroMinimum.Size = new System.Drawing.Size(68, 20);
			this.chkZeroMinimum.TabIndex = 5;
			this.chkZeroMinimum.Tag = "DEFAULT";
			this.chkZeroMinimum.Text = "Zero Out";
			// 
			// nudMinimum
			// 
			this.nudMinimum.Enabled = false;
			this.nudMinimum.Location = new System.Drawing.Point(168, 68);
			this.nudMinimum.Name = "nudMinimum";
			this.nudMinimum.Size = new System.Drawing.Size(56, 20);
			this.nudMinimum.TabIndex = 4;
			this.nudMinimum.Tag = "DEFAULT";
			this.nudMinimum.ValueChanged += new System.EventHandler(this.OnThreshold_Changed);
			// 
			// radIntensity
			// 
			this.radIntensity.Checked = true;
			this.radIntensity.Location = new System.Drawing.Point(12, 20);
			this.radIntensity.Name = "radIntensity";
			this.radIntensity.Size = new System.Drawing.Size(84, 20);
			this.radIntensity.TabIndex = 6;
			this.radIntensity.TabStop = true;
			this.radIntensity.Text = "By Intensity";
			this.radIntensity.CheckedChanged += new System.EventHandler(this.ThresholdByIntensity_CheckedChanged);
			// 
			// radPercentile
			// 
			this.radPercentile.Location = new System.Drawing.Point(12, 100);
			this.radPercentile.Name = "radPercentile";
			this.radPercentile.Size = new System.Drawing.Size(92, 20);
			this.radPercentile.TabIndex = 11;
			this.radPercentile.Text = "By Percentile";
			this.radPercentile.CheckedChanged += new System.EventHandler(this.ThresholdByPercentile_CheckedChanged);
			// 
			// chkRemoveDeadPixel
			// 
			this.chkRemoveDeadPixel.Enabled = false;
			this.chkRemoveDeadPixel.Location = new System.Drawing.Point(32, 148);
			this.chkRemoveDeadPixel.Name = "chkRemoveDeadPixel";
			this.chkRemoveDeadPixel.Size = new System.Drawing.Size(132, 20);
			this.chkRemoveDeadPixel.TabIndex = 9;
			this.chkRemoveDeadPixel.Tag = "DEFAULT";
			this.chkRemoveDeadPixel.Text = "Remove Dead Pixel:";
			// 
			// nudHotPixel
			// 
			this.nudHotPixel.DecimalPlaces = 2;
			this.nudHotPixel.Enabled = false;
			this.nudHotPixel.Location = new System.Drawing.Point(168, 124);
			this.nudHotPixel.Name = "nudHotPixel";
			this.nudHotPixel.Size = new System.Drawing.Size(56, 20);
			this.nudHotPixel.TabIndex = 8;
			this.nudHotPixel.Tag = "DEFAULT";
			// 
			// chkRemoveHotPixel
			// 
			this.chkRemoveHotPixel.Checked = true;
			this.chkRemoveHotPixel.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkRemoveHotPixel.Enabled = false;
			this.chkRemoveHotPixel.Location = new System.Drawing.Point(32, 124);
			this.chkRemoveHotPixel.Name = "chkRemoveHotPixel";
			this.chkRemoveHotPixel.Size = new System.Drawing.Size(132, 20);
			this.chkRemoveHotPixel.TabIndex = 7;
			this.chkRemoveHotPixel.Tag = "DEFAULT";
			this.chkRemoveHotPixel.Text = "Remove Hot Pixel:";
			// 
			// nudDeadPixel
			// 
			this.nudDeadPixel.DecimalPlaces = 2;
			this.nudDeadPixel.Enabled = false;
			this.nudDeadPixel.Location = new System.Drawing.Point(168, 148);
			this.nudDeadPixel.Name = "nudDeadPixel";
			this.nudDeadPixel.Size = new System.Drawing.Size(56, 20);
			this.nudDeadPixel.TabIndex = 10;
			this.nudDeadPixel.Tag = "DEFAULT";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.lbDead);
			this.groupBox1.Controls.Add(this.lbHot);
			this.groupBox1.Controls.Add(this.lbMin);
			this.groupBox1.Controls.Add(this.lbMax);
			this.groupBox1.Controls.Add(this.chkZeroDead);
			this.groupBox1.Controls.Add(this.chkZeroHot);
			this.groupBox1.Controls.Add(this.radPercentile);
			this.groupBox1.Controls.Add(this.chkRemoveDeadPixel);
			this.groupBox1.Controls.Add(this.nudHotPixel);
			this.groupBox1.Controls.Add(this.chkRemoveHotPixel);
			this.groupBox1.Controls.Add(this.nudDeadPixel);
			this.groupBox1.Controls.Add(this.chkZeroMinimum);
			this.groupBox1.Controls.Add(this.chkZeroMaximum);
			this.groupBox1.Controls.Add(this.nudMaximum);
			this.groupBox1.Controls.Add(this.radIntensity);
			this.groupBox1.Controls.Add(this.chkMaximum);
			this.groupBox1.Controls.Add(this.nudMinimum);
			this.groupBox1.Controls.Add(this.chkMinimum);
			this.groupBox1.Location = new System.Drawing.Point(8, 320);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(356, 180);
			this.groupBox1.TabIndex = 14;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Settings";
			// 
			// lbDead
			// 
			this.lbDead.AutoSize = true;
			this.lbDead.Location = new System.Drawing.Point(228, 152);
			this.lbDead.Name = "lbDead";
			this.lbDead.Size = new System.Drawing.Size(22, 16);
			this.lbDead.TabIndex = 19;
			this.lbDead.Text = "(%)";
			// 
			// lbHot
			// 
			this.lbHot.AutoSize = true;
			this.lbHot.Location = new System.Drawing.Point(228, 128);
			this.lbHot.Name = "lbHot";
			this.lbHot.Size = new System.Drawing.Size(22, 16);
			this.lbHot.TabIndex = 18;
			this.lbHot.Text = "(%)";
			// 
			// lbMin
			// 
			this.lbMin.AutoSize = true;
			this.lbMin.Location = new System.Drawing.Point(228, 72);
			this.lbMin.Name = "lbMin";
			this.lbMin.Size = new System.Drawing.Size(53, 16);
			this.lbMin.TabIndex = 17;
			this.lbMin.Text = "(intensity)";
			// 
			// lbMax
			// 
			this.lbMax.AutoSize = true;
			this.lbMax.Location = new System.Drawing.Point(228, 48);
			this.lbMax.Name = "lbMax";
			this.lbMax.Size = new System.Drawing.Size(53, 16);
			this.lbMax.TabIndex = 16;
			this.lbMax.Text = "(intensity)";
			// 
			// chkZeroDead
			// 
			this.chkZeroDead.Enabled = false;
			this.chkZeroDead.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.chkZeroDead.Location = new System.Drawing.Point(284, 148);
			this.chkZeroDead.Name = "chkZeroDead";
			this.chkZeroDead.Size = new System.Drawing.Size(68, 20);
			this.chkZeroDead.TabIndex = 15;
			this.chkZeroDead.Tag = "DEFAULT";
			this.chkZeroDead.Text = "Zero Out";
			// 
			// chkZeroHot
			// 
			this.chkZeroHot.Enabled = false;
			this.chkZeroHot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.chkZeroHot.Location = new System.Drawing.Point(284, 124);
			this.chkZeroHot.Name = "chkZeroHot";
			this.chkZeroHot.Size = new System.Drawing.Size(68, 20);
			this.chkZeroHot.TabIndex = 14;
			this.chkZeroHot.Tag = "DEFAULT";
			this.chkZeroHot.Text = "Zero Out";
			// 
			// btnDefaultSettings
			// 
			this.btnDefaultSettings.Location = new System.Drawing.Point(368, 64);
			this.btnDefaultSettings.Name = "btnDefaultSettings";
			this.btnDefaultSettings.Size = new System.Drawing.Size(92, 23);
			this.btnDefaultSettings.TabIndex = 15;
			this.btnDefaultSettings.Text = "Default Settings";
			this.btnDefaultSettings.Click += new System.EventHandler(this.btnDefaultSettings_Click);
			// 
			// btnLoadSettings
			// 
			this.btnLoadSettings.Location = new System.Drawing.Point(368, 92);
			this.btnLoadSettings.Name = "btnLoadSettings";
			this.btnLoadSettings.Size = new System.Drawing.Size(92, 23);
			this.btnLoadSettings.TabIndex = 16;
			this.btnLoadSettings.Text = "Load Settings";
			this.btnLoadSettings.Click += new System.EventHandler(this.btnLoadSettings_Click);
			// 
			// btnSaveSettings
			// 
			this.btnSaveSettings.Location = new System.Drawing.Point(368, 120);
			this.btnSaveSettings.Name = "btnSaveSettings";
			this.btnSaveSettings.Size = new System.Drawing.Size(92, 23);
			this.btnSaveSettings.TabIndex = 17;
			this.btnSaveSettings.Text = "Save Settings";
			this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
			// 
			// DlgThreshold
			// 
			this.AcceptButton = this.button_OK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.button_Cancel;
			this.ClientSize = new System.Drawing.Size(466, 507);
			this.Controls.Add(this.btnSaveSettings);
			this.Controls.Add(this.btnLoadSettings);
			this.Controls.Add(this.btnDefaultSettings);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupBoxPreview);
			this.Controls.Add(this.button_Cancel);
			this.Controls.Add(this.button_OK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DlgThreshold";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Threshold";
			this.groupBoxPreview.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.nudMaximum)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMinimum)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudHotPixel)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudDeadPixel)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region override routines
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
		}

		protected override void OnLoadPersistenceData(IDictionary storage)
		{
			try 
			{
				//nudMaximum.Maximum = (Decimal)_image.MaxGreyValue;
				//nudMaximum.Minimum = (Decimal)_image.MinGreyValue;
				
				//nudMinimum.Maximum = (Decimal)_image.MaxGreyValue;
				//nudMinimum.Minimum = (Decimal)_image.MinGreyValue;
			}
			catch
			{
				throw;
			}	

			base.OnLoadPersistenceData (storage);
		}

		protected override void OnSavePersistenceData(IDictionary storage)
		{
			base.OnSavePersistenceData (storage);
		}

		public override void ApplyToCommonImage(SIA.SystemLayer.CommonImage image)
		{
			if (ThresholdByIntensity)
			{
				bool zeroMinimum = ZeroMinimum;
				int minimum = -1;
				if (chkMinimum.Checked)
					minimum = (int)nudMinimum.Value;
				bool zeroMaximum = ZeroMaximum;
				int maximum = -1;
				if (chkMaximum.Checked)
					maximum = (int)nudMaximum.Value;

				if (minimum == -1 && maximum == -1)
					return;
				if (minimum != -1 && maximum != -1 && minimum > maximum)
					return;
				
				image.kThreshold(minimum, zeroMinimum, maximum, zeroMaximum);
			}
			else
			{
				if (_histogram == null)
				{
					this.Previewer.LockedMouseEvents = true;
					_histogram = _image.Histogram;
					this.Previewer.LockedMouseEvents = false;
				}

				float fromPercentage = this.From;
				float toPercentage = this.To;

				if (fromPercentage <= 0 && toPercentage>=100)
					return;

				if (_histogram != null)
				{					
					bool zeroHotPixel = chkZeroHot.Checked;
					bool zeroDeadPixel = chkZeroDead.Checked;
					PreviewThresholdByPercentile(image, _histogram,  fromPercentage, toPercentage, zeroDeadPixel, zeroHotPixel);					
				}
			}
		}

		protected override void LockUserInputObjects()
		{
			
		}

		protected override void UnlockUserInputObjects()
		{
			
		}

		public override SIA.UI.Components.ImagePreview GetPreviewer()
		{
			return _imagePreview;
		}
		#endregion

		#region event handlers
		private void btnReset_Click(object sender, System.EventArgs e)
		{
			this.ResetPreview();
		}

		private void btnPreview_Click(object sender, System.EventArgs e)
		{
			if ( !InputValidate()) 
				return;

			this.ApplyToPreview();
		}		

		private void button_OK_Click(object sender, System.EventArgs e)
		{
			if ( !InputValidate()) return;

			this.UpdateToControl(false);

			this.DialogResult = DialogResult.OK;
		}

		private void chkMaximum_CheckedChanged(object sender, System.EventArgs e)
		{
			OnMaximumCheckedChanged();
			UpdateLabel();
			this.ApplyToPreview();
		}

		private void chkMinimum_CheckedChanged(object sender, System.EventArgs e)
		{
			OnMinmumCheckedChanged();
			UpdateLabel();
			this.ApplyToPreview();
		}

		private void chkRemoveHotPixel_CheckedChanged(object sender, System.EventArgs e)
		{
			OnRemoveHotPixelCheckedChanged();
			UpdateLabel();
			this.ApplyToPreview();
		}

		private void chkRemoveDeadPixel_CheckedChanged(object sender, System.EventArgs e)
		{
			OnRemoveDeadPixelCheckedChanged();
			UpdateLabel();
			this.ApplyToPreview();
		}

		private void ThresholdByIntensity_CheckedChanged(object sender, System.EventArgs e)
		{
			UpdateMethod_GUI(radIntensity.Checked);			
			this.ApplyToPreview();
		}

		private void ThresholdByPercentile_CheckedChanged(object sender, System.EventArgs e)
		{
			UpdateMethod_GUI(radIntensity.Checked);			
			this.ApplyToPreview();
		}

		private void UpdateMethod_GUI(bool bByIntensity)
		{
			ThresholdByIntensity = bByIntensity;
			ThresholdByPercentile = !bByIntensity;

			UpdateLabel();
		}

		private void UpdateLabel()
		{
			lbHot.Enabled = (radPercentile.Checked && chkRemoveHotPixel.Checked);
			lbDead.Enabled = (radPercentile.Checked && chkRemoveDeadPixel.Checked);
			
			lbMin.Enabled = (radIntensity.Checked && chkMinimum.Checked);
			lbMax.Enabled = (radIntensity.Checked && chkMaximum.Checked);
		}

		private void ThresholdValueChanged(object sender, System.EventArgs e)
		{
			this.ApplyToPreview();
		}

		private void ZeroOutCheckedChanged(object sender, System.EventArgs e)
		{
			this.ApplyToPreview();	
		}

		private void btnDefaultSettings_Click(object sender, System.EventArgs e)
		{
			try
			{
				DefaultSettings();
			}
			catch (System.Exception exp)
			{
				MessageBoxEx.Error("Failed to set default settings: " + exp.Message);
			}		
		}

		private void DefaultSettings()
		{			
			try
			{
				using (ThresholdCommandSettings cmdSettings = new ThresholdCommandSettings(
					       true,
					       (int)_image.MinCurrentView,
					       true,
					       true,
					       (int)_image.MaxCurrentView,
					       true))
				{
					_settings = cmdSettings.Clone() as ThresholdCommandSettings;
					
					this.UpdateToControl(true);
				}				
			}
			catch (System.Exception exp)
			{
				throw exp;
			}
		}

		private void btnLoadSettings_Click(object sender, System.EventArgs e)
		{
			try
			{
				LoadSettings();
			}
			catch (System.Exception exp)
			{
				MessageBoxEx.Error("Failed to load settings: " + exp.Message);
			}
		}

		private void LoadSettings()
		{			
			try
			{
				using (OpenFileDialog dlg = CommonDialogs.OpenXmlFileDialog("Load settings"))
				{              					
					if( dlg.ShowDialog()==DialogResult.OK)
					{
						String filename = dlg.FileName;

						using (SIA.UI.Controls.Commands.RasterCommandSettings cmdSettings = 
							       SIA.UI.Controls.Automation.RasterCommandSettingsSerializer.Deserialize(filename, typeof(ThresholdCommandSettings)))
						{						
							_settings = cmdSettings.Clone() as ThresholdCommandSettings;
							this.UpdateToControl(true);						
						}						
					}
				}
			}
			catch (System.Exception exp)
			{
				throw exp;
			}
		}

		private void btnSaveSettings_Click(object sender, System.EventArgs e)
		{
			try
			{
				SaveSettings();
			}
			catch (System.Exception exp)
			{
				MessageBoxEx.Error("Failed to save settings: " + exp.Message);
			}		
		}

		private void SaveSettings()
		{
			try
			{
				using (SaveFileDialog  dlg = CommonDialogs.SaveXmlFileDialog("Save Settings"))
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
					
						this.UpdateToControl(false);
											
						SIA.UI.Controls.Automation.RasterCommandSettingsSerializer.Serialize(filename, _settings);
					}
				}
			}
			catch(System.Exception exp)
			{
				throw exp;
			}
		}
		#endregion

		#region internal helpers
		private void InitClass()
		{
			this.nudHotPixel.ValueChanged += new System.EventHandler(this.ThresholdValueChanged);
			this.nudDeadPixel.ValueChanged += new System.EventHandler(this.ThresholdValueChanged);
			this.nudMaximum.ValueChanged += new System.EventHandler(this.ThresholdValueChanged);
			this.nudMinimum.ValueChanged += new System.EventHandler(this.ThresholdValueChanged);

			this.chkRemoveHotPixel.CheckedChanged += new System.EventHandler(this.chkRemoveHotPixel_CheckedChanged);
			this.chkRemoveDeadPixel.CheckedChanged += new System.EventHandler(this.chkRemoveDeadPixel_CheckedChanged);

		        this.chkZeroDead.CheckedChanged += new System.EventHandler(this.ZeroOutCheckedChanged);
			this.chkZeroHot.CheckedChanged += new System.EventHandler(this.ZeroOutCheckedChanged);
			this.chkZeroMinimum.CheckedChanged += new System.EventHandler(this.ZeroOutCheckedChanged);
			this.chkZeroMaximum.CheckedChanged += new System.EventHandler(this.ZeroOutCheckedChanged);
		}

		private bool InputValidate()
		{			
			if (ThresholdByIntensity)
			{
				if (chkMaximum.Checked && !kUtils.IsInputValueValidate(nudMaximum)) 
					return false;
				if (chkMinimum.Checked && !kUtils.IsInputValueValidate(nudMinimum)) 
					return false;
				if (chkMaximum.Checked && chkMinimum.Checked &&
					nudMaximum.Value <= nudMinimum.Value)
				{
					MessageBoxEx.Error("Invalid threshold value or Max Threshold must be greater than Min Threshold");
					return false;
				}
			}
			else
			{
				if (chkRemoveHotPixel.Checked && !kUtils.IsInputValueValidate(nudHotPixel)) 
					return false;
				if (chkRemoveDeadPixel.Checked && !kUtils.IsInputValueValidate(nudDeadPixel)) 
					return false;
				if (chkRemoveHotPixel.Checked && chkRemoveDeadPixel.Checked &&
					this.From > this.To)
				{
					MessageBoxEx.Error("Invalid threshold value.");
					return false;
				}
			}

			return true;
		}

		private void OnThreshold_Changed(object sender, System.EventArgs e)
		{
		}

		private void UpdateToControl(bool bUpdateToControl)
		{
			if (bUpdateToControl)
			{
                // Always uppdate settings's value before updating another parameters
                this.nudMinimum.Value = (decimal)Math.Max(
                    0, Math.Min(_settings.Minimum, _image.MaxGreyValue));
                this.nudMaximum.Value = (decimal)Math.Max(
                    0, Math.Min(_settings.Maximum, _image.MaxGreyValue));
                this.nudHotPixel.Value = (decimal)Math.Max(
                    0, Math.Min(100, (100.0f - _settings.To)));
                this.nudDeadPixel.Value = (decimal)Math.Max(
                    0, Math.Min(100, _settings.From));
				
				this.UseMinimum = _settings.UseMinimum;
				this.UseMaximum = _settings.UseMaximum;				
				this.chkZeroMinimum.Checked = _settings.ZeroMinimum;                
				this.chkZeroMaximum.Checked = _settings.ZeroMaximum;
                this.ThresholdByIntensity = (_settings.Type == eThresholdType.Intensity);

				
				this.RemoveDeadPixel = _settings.RemoveDeadPixel;
				this.RemoveHotPixel = _settings.RemoveHotPixel;				
				this.chkZeroHot.Checked = _settings.ZeroTo;				
				this.chkZeroDead.Checked = _settings.ZeroFrom;
                this.ThresholdByPercentile = (_settings.Type == eThresholdType.Percentile);

				UpdateLabel();

				this.Invalidate(true);

				this.ApplyToPreview();
			}
			else
			{
				this.Invalidate(true);

				if (ThresholdByIntensity)
					_settings.Type = eThresholdType.Intensity;
				else
					_settings.Type = eThresholdType.Percentile;
				
				_settings.UseMinimum = this.UseMinimum;
				_settings.UseMaximum = this.UseMaximum;
				_settings.Minimum =(int) this.nudMinimum.Value;
				_settings.ZeroMinimum = this.chkZeroMinimum.Checked;
				_settings.Maximum = (int)this.nudMaximum.Value;
				_settings.ZeroMaximum = this.chkZeroMaximum.Checked;
				
				_settings.RemoveDeadPixel = this.RemoveDeadPixel;
				_settings.RemoveHotPixel = this.RemoveHotPixel;
				_settings.To = 100.0f - (float)this.nudHotPixel.Value;
				_settings.ZeroTo = this.chkZeroHot.Checked;
				_settings.From = (float)this.nudDeadPixel.Value;
				_settings.ZeroFrom = this.chkZeroDead.Checked;
			}
		}

		private void PreviewThresholdByPercentile(CommonImage image, kHistogram histogram, float fromPercentage, float toPercentage, bool zeroFrom, bool zeroTo)
		{			
			if (histogram == null)
				return;

			double []hist = histogram.Histogram;
			if (_totalIntensity == -1)
			{
				_totalIntensity = 0;
				int nBins = hist.Length;
				for (int i=0; i<nBins; i++)
				{
					_totalIntensity += hist[i];
				}
			}
			int rankMin = (int)(_totalIntensity*fromPercentage*0.01);
			int rankMax = (int)(_totalIntensity*toPercentage*0.01);
			float minThreshold = 0;
			float maxThreshold = 0;
			FindRangeThreshold(ref minThreshold, ref maxThreshold, hist, rankMin, rankMax);

			int minimum = -1;
			if (fromPercentage > 0)
				minimum = (int)minThreshold;
			int maximum = -1;
			if (toPercentage < 100)
				maximum = (int)maxThreshold;

			image.kThreshold(minimum, zeroFrom, maximum, zeroTo);			
		}

		public void FindRangeThreshold(ref float minThreshold, ref float maxThreshold, double []hist, int rankMin, int rankMax)
		{
			bool bFoundMin = false;
			bool bFoundMax = false;

			if (rankMin <= 0)
			{
				minThreshold = 0;
				bFoundMin = true;
			}

			if (rankMax <= 0)
			{
				maxThreshold = 0;
				bFoundMax = true;
			}
			
			if (!bFoundMin || !bFoundMax)
			{
				double total = 0;
				int nBins = hist.Length;

				for (int i=0; i<nBins; i++)
				{
					total += hist[i];				
					if (!bFoundMin && total >= rankMin)
					{
						minThreshold = (float)i;
						bFoundMin = true;
					}

					if (!bFoundMax && total >= rankMax)
					{
						maxThreshold = (float)i;
						return;
					}
				}
				if (!bFoundMin)
					minThreshold = (float)nBins-1;
				if (!bFoundMax)
					maxThreshold = (float)nBins-1;
			}
		}
		#endregion						
	}		
}
