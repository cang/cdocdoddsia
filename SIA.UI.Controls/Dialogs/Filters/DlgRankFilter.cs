using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;

using SIA.UI.Controls;
using SIA.UI.Controls.UserControls;

using SIA.UI.Controls.Utilities;
using SIA.UI.Controls.Automation.Commands;

namespace SIA.UI.Controls.Dialogs
{
	/// <summary>
	/// Name : DlgRankFilter
	/// Description : User interface for Rank Filter
	/// Thread Support : True
	/// Persistence Data : True
	/// </summary>
	public class DlgRankFilter : DialogPreviewBase
	{		
		#region member attributes
		public SIA.UI.Components.ImagePreview _imagePreview; 
		
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.GroupBox groupBoxPreview;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.Button btnPreview;
		private System.Windows.Forms.GroupBox grpConvolutionMatrixType;
		private System.Windows.Forms.Label lblPass;
		private System.Windows.Forms.NumericUpDown ndEnPass;
		private System.Windows.Forms.RadioButton rd5_5;
		private System.Windows.Forms.RadioButton rd7_7;
		private System.Windows.Forms.RadioButton rd3_3;
		private System.Windows.Forms.GroupBox groupBoxTypeFilter;
		private System.Windows.Forms.RadioButton radMin;
		private System.Windows.Forms.RadioButton radMax;
		private System.Windows.Forms.RadioButton radMedian;
		private System.Windows.Forms.RadioButton radMean;
		private System.Windows.Forms.Button btnSaveSettings;
		private System.Windows.Forms.Button btnLoadSettings;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#endregion

		#region constructor and destructor
		public DlgRankFilter(ImageWorkspace owner) : base(owner, true)
		{
			InitializeComponent();

			InitRadioButtonEvent();	
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

		#region public properties		
		public int FilterType
		{
			get
			{
				if(radMin.Checked)
					return 0;
				if(radMax.Checked)
					return 1;
				if(radMedian.Checked)
					return 2;
				if(radMean.Checked)
					return 3;
				
				return 0;
			}
            set
            {
                switch (value)
                {
                    case 0:
                        radMin.Checked = true;
                        break;
                    case 1:
                        radMax.Checked = true;
                        break;
                    case 2:
                        radMedian.Checked = true;
                        break;
                    case 3:
                        radMean.Checked = true;
                        break;
                    default:
                        radMedian.Checked = true;
                        break;
                }
            }
		}

		public int Kernel
		{
			get
			{
				if(rd3_3.Checked)
					return 3;
				if(rd5_5.Checked)
					return 5;
				if(rd7_7.Checked)
					return 7;

				return 3;
			}
            set
            {
                switch (value)
                {
                    case 3:
                        rd3_3.Checked = true;
                        break;
                    case 5:
                        rd5_5.Checked = true;
                        break;
                    case 7:
                        rd7_7.Checked = true;
                        break;
                    default:
                        rd3_3.Checked = true;
                        break;
                }
            }
		}

		public int Pass
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
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgRankFilter));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBoxPreview = new System.Windows.Forms.GroupBox();
            this.btnReset = new System.Windows.Forms.Button();
            this._imagePreview = new SIA.UI.Components.ImagePreview();
            this.btnPreview = new System.Windows.Forms.Button();
            this.grpConvolutionMatrixType = new System.Windows.Forms.GroupBox();
            this.lblPass = new System.Windows.Forms.Label();
            this.ndEnPass = new System.Windows.Forms.NumericUpDown();
            this.rd5_5 = new System.Windows.Forms.RadioButton();
            this.rd7_7 = new System.Windows.Forms.RadioButton();
            this.rd3_3 = new System.Windows.Forms.RadioButton();
            this.groupBoxTypeFilter = new System.Windows.Forms.GroupBox();
            this.radMean = new System.Windows.Forms.RadioButton();
            this.radMedian = new System.Windows.Forms.RadioButton();
            this.radMax = new System.Windows.Forms.RadioButton();
            this.radMin = new System.Windows.Forms.RadioButton();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.btnLoadSettings = new System.Windows.Forms.Button();
            this.groupBoxPreview.SuspendLayout();
            this.grpConvolutionMatrixType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ndEnPass)).BeginInit();
            this.groupBoxTypeFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(284, 8);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(84, 23);
            this.btnOK.TabIndex = 15;
            this.btnOK.Text = "OK";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(284, 36);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(84, 23);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Cancel";
            // 
            // groupBoxPreview
            // 
            this.groupBoxPreview.Controls.Add(this.btnReset);
            this.groupBoxPreview.Controls.Add(this._imagePreview);
            this.groupBoxPreview.Controls.Add(this.btnPreview);
            this.groupBoxPreview.Location = new System.Drawing.Point(4, 4);
            this.groupBoxPreview.Name = "groupBoxPreview";
            this.groupBoxPreview.Size = new System.Drawing.Size(272, 304);
            this.groupBoxPreview.TabIndex = 18;
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
            this._imagePreview.BackColor = System.Drawing.Color.Black;
            this._imagePreview.ImageViewer = null;
            this._imagePreview.Location = new System.Drawing.Point(8, 16);
            this._imagePreview.Name = "_imagePreview";
            this._imagePreview.PreviewRectangle = new System.Drawing.Rectangle(0, 0, 0, 0);
            this._imagePreview.Size = new System.Drawing.Size(256, 256);
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
            // grpConvolutionMatrixType
            // 
            this.grpConvolutionMatrixType.Controls.Add(this.lblPass);
            this.grpConvolutionMatrixType.Controls.Add(this.ndEnPass);
            this.grpConvolutionMatrixType.Controls.Add(this.rd5_5);
            this.grpConvolutionMatrixType.Controls.Add(this.rd7_7);
            this.grpConvolutionMatrixType.Controls.Add(this.rd3_3);
            this.grpConvolutionMatrixType.Location = new System.Drawing.Point(4, 388);
            this.grpConvolutionMatrixType.Name = "grpConvolutionMatrixType";
            this.grpConvolutionMatrixType.Size = new System.Drawing.Size(272, 84);
            this.grpConvolutionMatrixType.TabIndex = 19;
            this.grpConvolutionMatrixType.TabStop = false;
            this.grpConvolutionMatrixType.Tag = "gEnOption";
            this.grpConvolutionMatrixType.Text = "Options";
            // 
            // lblPass
            // 
            this.lblPass.Location = new System.Drawing.Point(172, 20);
            this.lblPass.Name = "lblPass";
            this.lblPass.Size = new System.Drawing.Size(43, 20);
            this.lblPass.TabIndex = 3;
            this.lblPass.Text = "Pass:";
            this.lblPass.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ndEnPass
            // 
            this.ndEnPass.Location = new System.Drawing.Point(220, 20);
            this.ndEnPass.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.ndEnPass.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ndEnPass.Name = "ndEnPass";
            this.ndEnPass.Size = new System.Drawing.Size(40, 20);
            this.ndEnPass.TabIndex = 4;
            this.ndEnPass.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ndEnPass.ValueChanged += new System.EventHandler(this.ndEnPass_ValueChanged);
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
            // groupBoxTypeFilter
            // 
            this.groupBoxTypeFilter.Controls.Add(this.radMean);
            this.groupBoxTypeFilter.Controls.Add(this.radMedian);
            this.groupBoxTypeFilter.Controls.Add(this.radMax);
            this.groupBoxTypeFilter.Controls.Add(this.radMin);
            this.groupBoxTypeFilter.Location = new System.Drawing.Point(4, 312);
            this.groupBoxTypeFilter.Name = "groupBoxTypeFilter";
            this.groupBoxTypeFilter.Size = new System.Drawing.Size(272, 72);
            this.groupBoxTypeFilter.TabIndex = 20;
            this.groupBoxTypeFilter.TabStop = false;
            this.groupBoxTypeFilter.Text = "Type Filter";
            // 
            // radMean
            // 
            this.radMean.Location = new System.Drawing.Point(192, 44);
            this.radMean.Name = "radMean";
            this.radMean.Size = new System.Drawing.Size(68, 20);
            this.radMean.TabIndex = 3;
            this.radMean.Text = "Mean";
            // 
            // radMedian
            // 
            this.radMedian.Location = new System.Drawing.Point(192, 20);
            this.radMedian.Name = "radMedian";
            this.radMedian.Size = new System.Drawing.Size(68, 20);
            this.radMedian.TabIndex = 2;
            this.radMedian.Text = "Median";
            // 
            // radMax
            // 
            this.radMax.Location = new System.Drawing.Point(12, 44);
            this.radMax.Name = "radMax";
            this.radMax.Size = new System.Drawing.Size(68, 20);
            this.radMax.TabIndex = 1;
            this.radMax.Text = "Max";
            // 
            // radMin
            // 
            this.radMin.Checked = true;
            this.radMin.Location = new System.Drawing.Point(12, 20);
            this.radMin.Name = "radMin";
            this.radMin.Size = new System.Drawing.Size(68, 20);
            this.radMin.TabIndex = 0;
            this.radMin.TabStop = true;
            this.radMin.Text = "Min";
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveSettings.Location = new System.Drawing.Point(284, 92);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(84, 23);
            this.btnSaveSettings.TabIndex = 29;
            this.btnSaveSettings.Text = "Save Settings";
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // btnLoadSettings
            // 
            this.btnLoadSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadSettings.Enabled = true;
            this.btnLoadSettings.Location = new System.Drawing.Point(284, 64);
            this.btnLoadSettings.Name = "btnLoadSettings";
            this.btnLoadSettings.Size = new System.Drawing.Size(84, 23);
            this.btnLoadSettings.TabIndex = 30;
            this.btnLoadSettings.Text = "Load Settings";
            this.btnLoadSettings.Click += new EventHandler(btnLoadSettings_Click);
            // 
            // DlgRankFilter
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(374, 476);
            this.Controls.Add(this.btnLoadSettings);
            this.Controls.Add(this.btnSaveSettings);
            this.Controls.Add(this.groupBoxTypeFilter);
            this.Controls.Add(this.grpConvolutionMatrixType);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBoxPreview);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(10, 50);
            this.Name = "DlgRankFilter";
            this.ShowInTaskbar = false;
            this.Text = "Rank Filters";
            this.groupBoxPreview.ResumeLayout(false);
            this.grpConvolutionMatrixType.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ndEnPass)).EndInit();
            this.groupBoxTypeFilter.ResumeLayout(false);
            this.ResumeLayout(false);

		}        
		#endregion

		#region override routines

		#region DialogPreviewBase override		
		public override SIA.UI.Components.ImagePreview GetPreviewer()
		{
			return this._imagePreview;
		}

		protected override void LockUserInputObjects()
		{
			btnReset.Enabled = false;
			btnPreview.Enabled = false;			
		}
		
		protected override void UnlockUserInputObjects()
		{
			btnReset.Enabled = true;
			btnPreview.Enabled = true;			
		}		

		#endregion

		#region DialogBase override		
		protected override object OnGetDefaultValue(System.Windows.Forms.Control ctrl)
		{
//			if (ctrl == nudRadius)
//				return 1.0F;
//			else if (ctrl == nudWeight)
//				return 100.0F;
			return null;
		}

		#endregion

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
		}
		#endregion

		#region operation routines		
		public override void ApplyToCommonImage(SIA.SystemLayer.CommonImage image)
		{
			if (image == null)
				throw new System.ArgumentNullException("Invalid parameter");			

			int typeFilter = this.FilterType;
			int kerel = this.Kernel;
			int pass = this.Pass;
			image.FilterRank(typeFilter, kerel, pass);
		}

		#endregion

		#region event handlers		
		private void ndEnPass_ValueChanged(object sender, System.EventArgs e)
		{
			this.ApplyToPreview();
		}

		private void btnReset_Click(object sender, System.EventArgs e)
		{
			this.ResetPreview();
		}

		private void btnPreview_Click(object sender, System.EventArgs e)
		{
			this.ApplyToPreview();
		}

		private void OnRadioButtonCheckChange(object sender, System.EventArgs e)
		{	
			if(!((RadioButton)sender).Checked) 
				return;
			ApplyToPreview();			
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

		public void InitRadioButtonEvent()
		{
			AddRadioButtonEvent(this);
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
					
						using (FilterRankCommandSettings cmdSettings = new FilterRankCommandSettings(this.FilterType, this.Kernel, this.Pass))
						{						
							SIA.UI.Controls.Automation.RasterCommandSettingsSerializer.Serialize(filename, cmdSettings);
						}
					}
				}
			}
			catch(System.Exception exp)
			{
				throw exp;
			}
		}

        void btnLoadSettings_Click(object sender, EventArgs e)
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

        private void LoadAsXml()
        {
            try
            {
                using (OpenFileDialog dlg = CommonDialogs.OpenXmlFileDialog("Load Settings"))
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {                        
                        String filename = dlg.FileName;

                        FilterRankCommandSettings cmdSettings =
                            (FilterRankCommandSettings)SIA.UI.Controls.Automation.RasterCommandSettingsSerializer.Deserialize(
                            filename, typeof(FilterRankCommandSettings));

                        if (cmdSettings != null)
                        {
                            this.Kernel = cmdSettings.SzKernel;
                            this.Pass = cmdSettings.NumPass;
                            this.FilterType = cmdSettings.TypeFilter;                            
                        }
                    }
                }
            }
            catch (System.Exception exp)
            {
                throw exp;
            }
        }
		#endregion		
	}
}
