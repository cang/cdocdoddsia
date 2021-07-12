using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using System.Diagnostics;

using SIA.Common;
using SIA.SystemLayer;

using SIA.UI.Controls.Commands;
using SIA.UI.Controls.Utilities;
using SIA.UI.Controls.Automation.Commands;

using TYPE = System.UInt16;

namespace SIA.UI.Controls.Dialogs
{
	/// <summary>
	/// Name : DlgGlobalBackgroundCorrection
	/// Description : User interface for Global background correction
	/// Thread Support : False
	/// Persistence Data : True
	/// </summary>
	public class DlgGlobalBackgroundCorrection : SIA.UI.Controls.Dialogs.DialogBase
	{
		#region Windows Form members
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		public System.Windows.Forms.RadioButton radRefImages;
		public System.Windows.Forms.RadioButton radErosion;
		public System.Windows.Forms.NumericUpDown nudErosCount;
		public System.Windows.Forms.RadioButton radLowPass;
		public System.Windows.Forms.NumericUpDown nudFFTCutoff;
		public System.Windows.Forms.NumericUpDown nudThreshold;
		private System.Windows.Forms.Panel panelHeader;
		private System.Windows.Forms.ListBox lbItems;
		private System.Windows.Forms.Panel panelRefImages;
		private System.Windows.Forms.Button btnMoveDown;
		private System.Windows.Forms.Button btnMoveUp;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.ImageList buttonImages;
		private System.ComponentModel.IContainer components;
		public System.Windows.Forms.RadioButton radUnsharp;
		private System.Windows.Forms.Label lblCutOffUnit;
		private System.Windows.Forms.Label lblThresholdUnit;
		private System.Windows.Forms.GroupBox grpParameters;
		private System.Windows.Forms.Label lblThreshold;
		private System.Windows.Forms.Label lblCutOff;
		private System.Windows.Forms.Button BtnUnsharp;
		#endregion

		#region member attributes

		public SIA.SystemLayer.CommonImage _originImage;
		private System.Windows.Forms.Button btnSaveSettings;
		private System.Windows.Forms.Button btnLoadSettings;
		public SIA.Common.UnsharpParam _unsharpParam;

		#endregion

		#region Properties
		public eGlobalBackgroundCorrectionType Type
		{
			get
			{
				if (radLowPass.Checked)
					return eGlobalBackgroundCorrectionType.FastFourierTransform;
				else if (radErosion.Checked)
					return eGlobalBackgroundCorrectionType.ErosionFilter;
				else if (radUnsharp.Checked)
					return eGlobalBackgroundCorrectionType.UnsharpFilter;
				else 
					return eGlobalBackgroundCorrectionType.ReferenceImages;
			}

            set
            {
                switch (value)
                {
                    case eGlobalBackgroundCorrectionType.FastFourierTransform:
                        radLowPass.Checked = true;
                        break;                    
                    case eGlobalBackgroundCorrectionType.ErosionFilter:
                        radErosion.Checked = true;
                        break;
                    case eGlobalBackgroundCorrectionType.UnsharpFilter:
                        radUnsharp.Checked = true;
                        break;
                    case eGlobalBackgroundCorrectionType.ReferenceImages:
                        radRefImages.Checked = true;
                        break;                    
                }
            }
		}

		public int ErosionNumber
		{
			get
			{
				return (int)nudErosCount.Value;
			}
			set
			{
				nudErosCount.Value = value;
			}
		}

		public TYPE Threshold
		{
			get { return TYPE.Parse( nudThreshold.Text );}
            set { nudThreshold.Value = (decimal)value; }
		}

		public float CutOff
		{
			get { return float.Parse(nudFFTCutoff.Text); }
            set { nudFFTCutoff.Value = (decimal)value; }
		}

		public  bool LowPass
		{
			get { return radLowPass.Checked; }
		}

		public String[] ImagePaths
		{
			get
			{
				string[] path = null;
				if (lbItems.Items.Count > 0)
				{
					path = new string[lbItems.Items.Count];
					int index = 0;
					foreach(string item in lbItems.Items)
						path[index++] = item;
				}
				return path;
			}
		}

		public UnsharpParam UnsharpParam
		{
			get {return _unsharpParam;}
            set 
            {
                try
                {
                    _unsharpParam = value;
                }
                catch
                {
                }
            }
		}

		#endregion

		#region constructor and destructor
		public DlgGlobalBackgroundCorrection()
		{
			InitializeComponent();
		}

		public DlgGlobalBackgroundCorrection(SIA.SystemLayer.CommonImage image) : this()
		{
			_originImage = image;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgGlobalBackgroundCorrection));
			this.nudErosCount = new System.Windows.Forms.NumericUpDown();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.grpParameters = new System.Windows.Forms.GroupBox();
			this.BtnUnsharp = new System.Windows.Forms.Button();
			this.nudThreshold = new System.Windows.Forms.NumericUpDown();
			this.nudFFTCutoff = new System.Windows.Forms.NumericUpDown();
			this.radLowPass = new System.Windows.Forms.RadioButton();
			this.radRefImages = new System.Windows.Forms.RadioButton();
			this.radErosion = new System.Windows.Forms.RadioButton();
			this.lblThreshold = new System.Windows.Forms.Label();
			this.lblCutOff = new System.Windows.Forms.Label();
			this.panelRefImages = new System.Windows.Forms.Panel();
			this.lbItems = new System.Windows.Forms.ListBox();
			this.panelHeader = new System.Windows.Forms.Panel();
			this.btnMoveDown = new System.Windows.Forms.Button();
			this.buttonImages = new System.Windows.Forms.ImageList(this.components);
			this.btnMoveUp = new System.Windows.Forms.Button();
			this.btnRemove = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.radUnsharp = new System.Windows.Forms.RadioButton();
			this.lblCutOffUnit = new System.Windows.Forms.Label();
			this.lblThresholdUnit = new System.Windows.Forms.Label();
			this.btnSaveSettings = new System.Windows.Forms.Button();
			this.btnLoadSettings = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.nudErosCount)).BeginInit();
			this.grpParameters.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudThreshold)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudFFTCutoff)).BeginInit();
			this.panelRefImages.SuspendLayout();
			this.panelHeader.SuspendLayout();
			this.SuspendLayout();
			// 
			// nudErosCount
			// 
			this.nudErosCount.Location = new System.Drawing.Point(168, 96);
			this.nudErosCount.Name = "nudErosCount";
			this.nudErosCount.Size = new System.Drawing.Size(72, 20);
			this.nudErosCount.TabIndex = 6;
			this.nudErosCount.Tag = "DEFAULT";
			this.nudErosCount.Value = new System.Decimal(new int[] {
																	   4,
																	   0,
																	   0,
																	   0});
			this.nudErosCount.ValueChanged += new System.EventHandler(this.nErosionFilters_ValueChanged);
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(292, 4);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(88, 23);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.OKBtn_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(292, 32);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(88, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			// 
			// grpParameters
			// 
			this.grpParameters.Controls.Add(this.BtnUnsharp);
			this.grpParameters.Controls.Add(this.nudThreshold);
			this.grpParameters.Controls.Add(this.nudFFTCutoff);
			this.grpParameters.Controls.Add(this.radLowPass);
			this.grpParameters.Controls.Add(this.radRefImages);
			this.grpParameters.Controls.Add(this.radErosion);
			this.grpParameters.Controls.Add(this.nudErosCount);
			this.grpParameters.Controls.Add(this.lblThreshold);
			this.grpParameters.Controls.Add(this.lblCutOff);
			this.grpParameters.Controls.Add(this.panelRefImages);
			this.grpParameters.Controls.Add(this.radUnsharp);
			this.grpParameters.Controls.Add(this.lblCutOffUnit);
			this.grpParameters.Controls.Add(this.lblThresholdUnit);
			this.grpParameters.Location = new System.Drawing.Point(4, 0);
			this.grpParameters.Name = "grpParameters";
			this.grpParameters.Size = new System.Drawing.Size(280, 352);
			this.grpParameters.TabIndex = 0;
			this.grpParameters.TabStop = false;
			this.grpParameters.Text = "Settings";
			// 
			// BtnUnsharp
			// 
			this.BtnUnsharp.Enabled = false;
			this.BtnUnsharp.Location = new System.Drawing.Point(167, 121);
			this.BtnUnsharp.Name = "BtnUnsharp";
			this.BtnUnsharp.Size = new System.Drawing.Size(72, 20);
			this.BtnUnsharp.TabIndex = 9;
			this.BtnUnsharp.Text = "Settings";
			this.BtnUnsharp.Click += new System.EventHandler(this.btnUnsharp_Click);
			// 
			// nudThreshold
			// 
			this.nudThreshold.Location = new System.Drawing.Point(96, 44);
			this.nudThreshold.Maximum = new System.Decimal(new int[] {
																		 1000000,
																		 0,
																		 0,
																		 0});
			this.nudThreshold.Name = "nudThreshold";
			this.nudThreshold.Size = new System.Drawing.Size(72, 20);
			this.nudThreshold.TabIndex = 2;
			this.nudThreshold.Tag = "DEFAULT";
			this.nudThreshold.Value = new System.Decimal(new int[] {
																	   1,
																	   0,
																	   0,
																	   0});
			this.nudThreshold.ValueChanged += new System.EventHandler(this.ndThreshold_ValueChanged);
			// 
			// nudFFTCutoff
			// 
			this.nudFFTCutoff.DecimalPlaces = 3;
			this.nudFFTCutoff.Increment = new System.Decimal(new int[] {
																		   1,
																		   0,
																		   0,
																		   196608});
			this.nudFFTCutoff.Location = new System.Drawing.Point(96, 68);
			this.nudFFTCutoff.Name = "nudFFTCutoff";
			this.nudFFTCutoff.Size = new System.Drawing.Size(72, 20);
			this.nudFFTCutoff.TabIndex = 4;
			this.nudFFTCutoff.Tag = "DEFAULT";
			this.nudFFTCutoff.Value = new System.Decimal(new int[] {
																	   2,
																	   0,
																	   0,
																	   0});
			this.nudFFTCutoff.ValueChanged += new System.EventHandler(this.ndFFTCutoff_ValueChanged);
			// 
			// radLowPass
			// 
			this.radLowPass.Location = new System.Drawing.Point(8, 16);
			this.radLowPass.Name = "radLowPass";
			this.radLowPass.Size = new System.Drawing.Size(160, 20);
			this.radLowPass.TabIndex = 0;
			this.radLowPass.Tag = "DEFAULT";
			this.radLowPass.Text = "Low - Pass FFT Filters";
			this.radLowPass.CheckedChanged += new System.EventHandler(this.rdLowPass_CheckedChanged);
			// 
			// radRefImages
			// 
			this.radRefImages.BackColor = System.Drawing.SystemColors.Control;
			this.radRefImages.Location = new System.Drawing.Point(8, 144);
			this.radRefImages.Name = "radRefImages";
			this.radRefImages.Size = new System.Drawing.Size(160, 20);
			this.radRefImages.TabIndex = 8;
			this.radRefImages.Tag = "DEFAULT";
			this.radRefImages.Text = "Reference Image:";
			this.radRefImages.CheckedChanged += new System.EventHandler(this.radioButtonImage_CheckedChanged);
			// 
			// radErosion
			// 
			this.radErosion.BackColor = System.Drawing.SystemColors.Control;
			this.radErosion.Checked = true;
			this.radErosion.Location = new System.Drawing.Point(8, 96);
			this.radErosion.Name = "radErosion";
			this.radErosion.Size = new System.Drawing.Size(160, 20);
			this.radErosion.TabIndex = 5;
			this.radErosion.TabStop = true;
			this.radErosion.Tag = "DEFAULT";
			this.radErosion.Text = "Number of Erosion Filters:";
			this.radErosion.CheckedChanged += new System.EventHandler(this.radioButtonConstant_CheckedChanged);
			// 
			// lblThreshold
			// 
			this.lblThreshold.Location = new System.Drawing.Point(12, 44);
			this.lblThreshold.Name = "lblThreshold";
			this.lblThreshold.Size = new System.Drawing.Size(80, 20);
			this.lblThreshold.TabIndex = 1;
			this.lblThreshold.Text = "Threshold:";
			this.lblThreshold.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblCutOff
			// 
			this.lblCutOff.Location = new System.Drawing.Point(12, 68);
			this.lblCutOff.Name = "lblCutOff";
			this.lblCutOff.Size = new System.Drawing.Size(80, 20);
			this.lblCutOff.TabIndex = 3;
			this.lblCutOff.Text = "FFT Cutoff:";
			this.lblCutOff.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// panelRefImages
			// 
			this.panelRefImages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.panelRefImages.Controls.Add(this.lbItems);
			this.panelRefImages.Controls.Add(this.panelHeader);
			this.panelRefImages.Location = new System.Drawing.Point(8, 168);
			this.panelRefImages.Name = "panelRefImages";
			this.panelRefImages.Size = new System.Drawing.Size(264, 176);
			this.panelRefImages.TabIndex = 0;
			// 
			// lbItems
			// 
			this.lbItems.BackColor = System.Drawing.SystemColors.Control;
			this.lbItems.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbItems.HorizontalScrollbar = true;
			this.lbItems.Location = new System.Drawing.Point(0, 28);
			this.lbItems.Name = "lbItems";
			this.lbItems.Size = new System.Drawing.Size(264, 147);
			this.lbItems.TabIndex = 1;
			this.lbItems.TabStop = false;
			this.lbItems.DoubleClick += new System.EventHandler(this.lbItems_DoubleClick);
			this.lbItems.SelectedIndexChanged += new System.EventHandler(this.lbItems_SelectedIndexChanged);
			// 
			// panelHeader
			// 
			this.panelHeader.BackColor = System.Drawing.SystemColors.Control;
			this.panelHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelHeader.Controls.Add(this.btnMoveDown);
			this.panelHeader.Controls.Add(this.btnMoveUp);
			this.panelHeader.Controls.Add(this.btnRemove);
			this.panelHeader.Controls.Add(this.btnAdd);
			this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelHeader.ForeColor = System.Drawing.SystemColors.ControlText;
			this.panelHeader.Location = new System.Drawing.Point(0, 0);
			this.panelHeader.Name = "panelHeader";
			this.panelHeader.Size = new System.Drawing.Size(264, 28);
			this.panelHeader.TabIndex = 0;
			// 
			// btnMoveDown
			// 
			this.btnMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnMoveDown.BackColor = System.Drawing.SystemColors.Control;
			this.btnMoveDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnMoveDown.ImageIndex = 3;
			this.btnMoveDown.ImageList = this.buttonImages;
			this.btnMoveDown.Location = new System.Drawing.Point(238, 2);
			this.btnMoveDown.Name = "btnMoveDown";
			this.btnMoveDown.Size = new System.Drawing.Size(22, 22);
			this.btnMoveDown.TabIndex = 3;
			this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
			// 
			// buttonImages
			// 
			this.buttonImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth16Bit;
			this.buttonImages.ImageSize = new System.Drawing.Size(16, 16);
			this.buttonImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("buttonImages.ImageStream")));
			this.buttonImages.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// btnMoveUp
			// 
			this.btnMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnMoveUp.BackColor = System.Drawing.SystemColors.Control;
			this.btnMoveUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnMoveUp.ImageIndex = 2;
			this.btnMoveUp.ImageList = this.buttonImages;
			this.btnMoveUp.Location = new System.Drawing.Point(214, 2);
			this.btnMoveUp.Name = "btnMoveUp";
			this.btnMoveUp.Size = new System.Drawing.Size(22, 22);
			this.btnMoveUp.TabIndex = 2;
			this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
			// 
			// btnRemove
			// 
			this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRemove.BackColor = System.Drawing.SystemColors.Control;
			this.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnRemove.ImageIndex = 1;
			this.btnRemove.ImageList = this.buttonImages;
			this.btnRemove.Location = new System.Drawing.Point(190, 2);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.Size = new System.Drawing.Size(22, 22);
			this.btnRemove.TabIndex = 1;
			this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAdd.BackColor = System.Drawing.SystemColors.Control;
			this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnAdd.ImageIndex = 0;
			this.btnAdd.ImageList = this.buttonImages;
			this.btnAdd.Location = new System.Drawing.Point(166, 2);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(22, 22);
			this.btnAdd.TabIndex = 0;
			this.btnAdd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// radUnsharp
			// 
			this.radUnsharp.BackColor = System.Drawing.SystemColors.Control;
			this.radUnsharp.Location = new System.Drawing.Point(8, 120);
			this.radUnsharp.Name = "radUnsharp";
			this.radUnsharp.Size = new System.Drawing.Size(160, 20);
			this.radUnsharp.TabIndex = 7;
			this.radUnsharp.Tag = "";
			this.radUnsharp.Text = "Unsharp Masking";
			this.radUnsharp.CheckedChanged += new System.EventHandler(this.radUnsharp_CheckedChanged);
			// 
			// lblCutOffUnit
			// 
			this.lblCutOffUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCutOffUnit.Location = new System.Drawing.Point(172, 68);
			this.lblCutOffUnit.Name = "lblCutOffUnit";
			this.lblCutOffUnit.Size = new System.Drawing.Size(80, 20);
			this.lblCutOffUnit.TabIndex = 1;
			this.lblCutOffUnit.Text = "(%)";
			this.lblCutOffUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblThresholdUnit
			// 
			this.lblThresholdUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblThresholdUnit.Location = new System.Drawing.Point(172, 44);
			this.lblThresholdUnit.Name = "lblThresholdUnit";
			this.lblThresholdUnit.Size = new System.Drawing.Size(80, 20);
			this.lblThresholdUnit.TabIndex = 1;
			this.lblThresholdUnit.Text = "(Intensity)";
			this.lblThresholdUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnSaveSettings
			// 
			this.btnSaveSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.btnSaveSettings.Location = new System.Drawing.Point(292, 88);
			this.btnSaveSettings.Name = "btnSaveSettings";
			this.btnSaveSettings.Size = new System.Drawing.Size(88, 23);
			this.btnSaveSettings.TabIndex = 12;
			this.btnSaveSettings.Text = "Save Settings";
			this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
			// 
			// btnLoadSettings
			// 
			this.btnLoadSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.btnLoadSettings.Enabled = true;
			this.btnLoadSettings.Location = new System.Drawing.Point(292, 60);
			this.btnLoadSettings.Name = "btnLoadSettings";
			this.btnLoadSettings.Size = new System.Drawing.Size(88, 23);
			this.btnLoadSettings.TabIndex = 11;
			this.btnLoadSettings.Text = "Load Settings";
			this.btnLoadSettings.Click += new System.EventHandler(this.btnLoadSettings_Click);
			// 
			// DlgGlobalBackgroundCorrection
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(386, 352);
			this.Controls.Add(this.btnSaveSettings);
			this.Controls.Add(this.btnLoadSettings);
			this.Controls.Add(this.grpParameters);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DlgGlobalBackgroundCorrection";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Global Background Correction";
			this.Load += new System.EventHandler(this.GradIntensityDlg_Load);
			((System.ComponentModel.ISupportInitialize)(this.nudErosCount)).EndInit();
			this.grpParameters.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.nudThreshold)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudFFTCutoff)).EndInit();
			this.panelRefImages.ResumeLayout(false);
			this.panelHeader.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region internal helpers
		private bool InputValidate()
		{
			if ( LowPass && ! kUtils.IsInputValueValidate(nudThreshold))return false;

			if ( LowPass && ! kUtils.IsInputValueValidate(nudFFTCutoff))return false;

			if (this.LowPass && nudThreshold.Value <= 0)
			{
				MessageBoxEx.Error("Threshold value must be greater than 0.");
				return false;
			}

			if ( radErosion.Checked && !kUtils.IsInputValueValidate(nudErosCount)) return false;

			if ( radRefImages.Checked && lbItems.Items.Count == 0)
			{
				MessageBoxEx.Warning("Please select reference images.");				
				btnAdd.Select();
				return false;
			}
			return true;
		}

		private void OnCheckChange(bool bScaleImage)
		{
		}
		#endregion

		#region event handler
		private void OKBtn_Click(object sender, System.EventArgs e)
		{
			if ( !InputValidate()) return;			
			PersistenceDefault obj	= new PersistenceDefault(this);
			obj.Save();
			this.DialogResult = DialogResult.OK ;			
		}

		private void GradIntensityDlg_Load(object sender, System.EventArgs e)
		{
			// initialize edit list box
			InitializeEditListBox();

			kUtils.SetMinMax( nudThreshold,_originImage ); 
			
			// refresh GUI 
			rdLowPass_CheckedChanged(this, new System.EventArgs());
			radioButtonConstant_CheckedChanged(this, new System.EventArgs());
			radioButtonImage_CheckedChanged(this, new System.EventArgs());
			radUnsharp_CheckedChanged(this, EventArgs.Empty);
			_unsharpParam = new UnsharpParam();
		}
		
		private void rdLowPass_CheckedChanged(object sender, System.EventArgs e)
		{
			nudThreshold.Enabled = nudFFTCutoff.Enabled = radLowPass.Checked;			 
			if ( !radLowPass.Checked ) return;
			OnCheckChange( false );
		}

		private void radioButtonConstant_CheckedChanged(object sender, System.EventArgs e)
		{
			nudErosCount.Enabled = radErosion.Checked;
			if ( !radErosion.Checked ) return;
			OnCheckChange( false );
		}

		private void radioButtonImage_CheckedChanged(object sender, System.EventArgs e)
		{			
			this.panelRefImages.Enabled = radRefImages.Checked;
			OnCheckChange( true );
		}			

		private void ndThreshold_ValueChanged(object sender, System.EventArgs e)
		{
			OnCheckChange( false );
		}

		private void ndFFTCutoff_ValueChanged(object sender, System.EventArgs e)
		{
			OnCheckChange( false );
		}

		private void nErosionFilters_ValueChanged(object sender, System.EventArgs e)
		{
			OnCheckChange( false );
		}

		private void btnUnsharp_Click(object sender, System.EventArgs e)
		{
			//_unsharpParam
			using (DlgUnsharpGlobalBackground dlg  = new DlgUnsharpGlobalBackground())
			{
                dlg.Minimum = _unsharpParam._min;
                dlg.Maximum = _unsharpParam._max;                
                dlg.KernelRadius = _unsharpParam._radius;
                dlg.FilterType = _unsharpParam._type;

				if(dlg.ShowDialog() == DialogResult.OK)
				{
					_unsharpParam._min = (float)dlg.Minimum;
					_unsharpParam._max = (float)dlg.Maximum;
					_unsharpParam._type = dlg.FilterType;
					_unsharpParam._radius = dlg.KernelRadius;
				}
			}
		}

		private void radUnsharp_CheckedChanged(object sender, System.EventArgs e)
		{
			BtnUnsharp.Enabled = radUnsharp.Checked;
			if ( !radUnsharp.Checked ) return;
			OnCheckChange( false );

		}
		
		#region Edit List Box Handlers

		private void InitializeEditListBox()
		{
			UpdateEditListBox();

			this.panelRefImages.Enabled = radRefImages.Checked;
		}

		private void UpdateEditListBox()
		{
			btnRemove.Enabled = (lbItems.SelectedIndex >= 0 && lbItems.SelectedIndex < lbItems.Items.Count);
			btnMoveUp.Enabled = (lbItems.SelectedIndex > 0 && lbItems.SelectedIndex < lbItems.Items.Count);
			btnMoveDown.Enabled = (lbItems.SelectedIndex >= 0 && lbItems.SelectedIndex < lbItems.Items.Count-1);
		}

		private void btnAdd_Click(object sender, System.EventArgs e)
		{		
			//OpenFileDialog dlg = kUtils.kOpenImageFiles("Select image...", true);
			OpenFileDialog dlg = CommonDialogs.OpenImageFileDialog("Select an image", CommonDialogs.ImageFileFilter.AllSupportedImageFormat);
			dlg.Multiselect = true;
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				SIA.SystemLayer.CommonImage image = null;
				kUtils.kBeginWaitCursor();
				foreach (string filename in dlg.FileNames)
				{
					int index = lbItems.Items.IndexOf(filename);
					if (index < 0 || index >= lbItems.Items.Count)
					{
						try
						{
							if (kUtils.IsFitFile(filename) || kUtils.IsImageFile(filename))
								image = SIA.SystemLayer.CommonImage.FromFile(filename);
							
							if (image != null)
							{
								if (image.Width == _originImage.Width && image.Height == _originImage.Height)
								{
									lbItems.Items.Add(filename);
								}
								else
								{
									throw new System.Exception("because the reference image does not equal the current image in size.");
								}
							}
							else
								throw new System.Exception("because of invalid file format.");
						}
						catch(Exception exp) 
						{ 
							HandleOpenFileError(exp, filename);
						}
						finally
						{
							if (image!=null) 
							{
								image.Dispose();
								image = null;
							}
						}
					}
					else
						MessageBoxEx.Warning("File \"" + filename + "\" is already selected.");
				}
				kUtils.kEndWaitCursor();
			}
		}

		private void btnRemove_Click(object sender, System.EventArgs e)
		{
			if (lbItems.SelectedIndex < 0 || lbItems.SelectedIndex >= lbItems.Items.Count) return;
			int selIndex = lbItems.SelectedIndex;
			lbItems.Items.RemoveAt(selIndex);	
			if (selIndex < lbItems.Items.Count)
				lbItems.SelectedIndex = selIndex; 
			else if (lbItems.Items.Count > 0)
				lbItems.SelectedIndex = lbItems.Items.Count-1;
		}

		private void btnMoveUp_Click(object sender, System.EventArgs e)
		{
			if (lbItems.SelectedIndex < 1 || lbItems.SelectedIndex >= lbItems.Items.Count) return;
			object selItem = lbItems.SelectedItem;
			int selIndex = lbItems.SelectedIndex;
			lbItems.Items.Insert(selIndex-1, selItem);
			lbItems.Items.RemoveAt(selIndex+1);
			lbItems.SelectedIndex = selIndex-1;
		}

		private void btnMoveDown_Click(object sender, System.EventArgs e)
		{
			if (lbItems.SelectedIndex < 0 || lbItems.SelectedIndex >= lbItems.Items.Count-1) return;
			object selItem = lbItems.SelectedItem;
			int selIndex = lbItems.SelectedIndex;
			lbItems.Items.RemoveAt(selIndex);
			if (selIndex+1 < lbItems.Items.Count)
			{
				lbItems.Items.Insert(selIndex+1, selItem);
				lbItems.SelectedIndex = selIndex+1;
			}
			else
			{
				lbItems.SelectedIndex = lbItems.Items.Add(selItem);
			}
		}

		private void lbItems_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			// refresh user interface
			UpdateEditListBox();
		}

		private void lbItems_DoubleClick(object sender, System.EventArgs e)
		{		
		}
		#endregion

		#region Error handlers

		private void HandleOpenFileError(Exception exp, string filename)
		{
			MessageBoxEx.Error("Fail to open file " + filename + " " + exp.Message);
		}

		private void HandleGenericError(Exception exp)
		{
			MessageBoxEx.Error("Generic error has occured\n" + "Reason:" + exp.Message);
		}

		#endregion


		#endregion

		#region DialogBase override

		public void ApplyToCommonImage(SIA.SystemLayer.CommonImage commonImage, bool bScaleImage)
		{
			try
			{
				if (LowPass == true)	// using FFT Method
				{
					commonImage.GlobalBackgroundCorrection(eGlobalBackgroundCorrectionType.FastFourierTransform, 
						this.Threshold, this.CutOff);
				}
				else if (radErosion.Checked == true) // using Erosion Method
				{
					commonImage.GlobalBackgroundCorrection(eGlobalBackgroundCorrectionType.ErosionFilter,
						this.ErosionNumber);
				}
				else if (radUnsharp.Checked == true) // using Unsharp Masking Methodology
				{
					
					commonImage.GlobalBackgroundCorrection(eGlobalBackgroundCorrectionType.UnsharpFilter, _unsharpParam);
				}
				else if (radRefImages.Checked == true)	// using Reference Images
				{
					String[] file_paths = this.ImagePaths;
					int sel_image = -1;
					sel_image = (int)commonImage.GlobalBackgroundCorrection(eGlobalBackgroundCorrectionType.ReferenceImages, 
						file_paths);
					
					if (file_paths.Length > 1 && sel_image >= 0 && sel_image < file_paths.Length)
					{
#if _SHOW_MESSAGE
						MessageBoxEx.Info("File \"" + file_paths[sel_image] + "\" is selected as best background.");
#endif
					}
				}
			}
			catch (Exception exp)
			{
				MessageBoxEx.Error(exp.Message);
			}
		}

		
		protected override object OnGetDefaultValue(Control ctrl)
		{
			if (ctrl is RadioButton)
			{
				if (ctrl == radLowPass)
					return false;
				else if (ctrl == radErosion)
					return true;
				else if (ctrl == radUnsharp)
					return false;
				else if (ctrl == radRefImages)
					return false;
			}
			else 
			{
				if (ctrl == nudThreshold)
					return nudThreshold.Minimum;
				else if (ctrl == nudFFTCutoff)
					return 2.0F;
				else if (ctrl == nudErosCount)
					return 2.0F;
			}

			return null;
		}
		
		#endregion

		private void btnLoadSettings_Click(object sender, System.EventArgs e)
		{
			LoadFromXml();	
		}

		private void btnSaveSettings_Click(object sender, System.EventArgs e)
		{
			if (this.InputValidate())
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
			finally
			{

			}
		}

		private void LoadFromXml()
		{
			using (OpenFileDialog dlg = CommonDialogs.OpenXmlFileDialog("Select a xml settings file..."))
			{
				if (DialogResult.OK == dlg.ShowDialog(this))
					this.XmlDeserialize(dlg.FileName);
			}
		}

		#region Xml serialize and deserialize

		public void XmlSerialize(String fileName)
		{
			eGlobalBackgroundCorrectionType gbcType = this.Type;					
			
			GbcCommandSettings cmdSettings = null;
			try
			{
				switch (gbcType)
				{
					case eGlobalBackgroundCorrectionType.ErosionFilter:
						cmdSettings = new GbcCommandSettings(this.ErosionNumber);
						break;
					case eGlobalBackgroundCorrectionType.FastFourierTransform:
						cmdSettings = new GbcCommandSettings(this.Threshold, this.CutOff);
						break;
					case eGlobalBackgroundCorrectionType.ReferenceImages:
						cmdSettings = new GbcCommandSettings(this.ImagePaths);
						break;
					case eGlobalBackgroundCorrectionType.UnsharpFilter:
						cmdSettings = new GbcCommandSettings(this.UnsharpParam);
						break;
				}

				// call RasterCommandSettingsSerializer to serialize
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

		public void XmlDeserialize(String fileName)
		{
            GbcCommandSettings cmdSettings = null;
            try
            {
                cmdSettings =
                    (GbcCommandSettings)SIA.UI.Controls.Automation.RasterCommandSettingsSerializer.Deserialize(
                    fileName, typeof(GbcCommandSettings));

                if (cmdSettings != null)
                {
                    eGlobalBackgroundCorrectionType gbcType = cmdSettings.Type;

                    switch (gbcType)
                    {
                        case eGlobalBackgroundCorrectionType.ErosionFilter:
                            this.ErosionNumber = cmdSettings.NumPass;
                            break;
                        case eGlobalBackgroundCorrectionType.FastFourierTransform:
                            this.Threshold = (ushort)cmdSettings.Threshold;
                            this.CutOff = cmdSettings.CutOff;
                            break;
                        case eGlobalBackgroundCorrectionType.ReferenceImages:
                            //cmdSettings = new GbcCommandSettings(this.ImagePaths);
                            break;
                        case eGlobalBackgroundCorrectionType.UnsharpFilter:
                            this.UnsharpParam = cmdSettings.UnsharpSettings;
                            break;
                    }

                    this.Type = gbcType;
                }
            }
            catch
            {
                if (cmdSettings != null)
                    cmdSettings.Dispose();
            }
		}

		#endregion Xml serialize and deserialize
	}
}
