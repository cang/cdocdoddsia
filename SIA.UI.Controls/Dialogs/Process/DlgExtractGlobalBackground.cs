using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

using SIA.UI.Controls;
using SIA.UI.Controls.Utilities;
using SIA.UI.Controls.Automation.Commands;
using System.Diagnostics;

namespace SIA.UI.Controls.Dialogs
{
	/// <summary>
	/// Name : DlgExtractGlobalBackground
	/// Description : User interface for Extract Global Background
	/// Thread Support : None
	/// Persistence Data : True
	/// </summary>
	public class DlgExtractGlobalBackground : DialogBase
	{
		#region constants
		
		#endregion

		#region Windows Form member attributes

		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.GroupBox groupBox4;
		public System.Windows.Forms.NumericUpDown ndThreshold;
		public System.Windows.Forms.NumericUpDown ndFFTCutoff;
		public System.Windows.Forms.RadioButton rdLowPass;
		public System.Windows.Forms.RadioButton radioButtonConstant;
		public System.Windows.Forms.NumericUpDown nErosionFilters;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lblCutOffUnit;
		private System.Windows.Forms.Label lblThresholdUnit;
        private Button btnLoadSettings;
        private Button btnSaveSettings;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgExtractGlobalBackground));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ndThreshold = new System.Windows.Forms.NumericUpDown();
            this.ndFFTCutoff = new System.Windows.Forms.NumericUpDown();
            this.rdLowPass = new System.Windows.Forms.RadioButton();
            this.radioButtonConstant = new System.Windows.Forms.RadioButton();
            this.nErosionFilters = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.lblCutOffUnit = new System.Windows.Forms.Label();
            this.lblThresholdUnit = new System.Windows.Forms.Label();
            this.btnLoadSettings = new System.Windows.Forms.Button();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ndThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndFFTCutoff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nErosionFilters)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(261, 41);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(261, 12);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(88, 23);
            this.btnOk.TabIndex = 8;
            this.btnOk.Text = "OK";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.ndThreshold);
            this.groupBox4.Controls.Add(this.ndFFTCutoff);
            this.groupBox4.Controls.Add(this.rdLowPass);
            this.groupBox4.Controls.Add(this.radioButtonConstant);
            this.groupBox4.Controls.Add(this.nErosionFilters);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.lblCutOffUnit);
            this.groupBox4.Controls.Add(this.lblThresholdUnit);
            this.groupBox4.Location = new System.Drawing.Point(5, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(247, 126);
            this.groupBox4.TabIndex = 27;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Settings";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(28, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "FFT Cutoff:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ndThreshold
            // 
            this.ndThreshold.Location = new System.Drawing.Point(96, 44);
            this.ndThreshold.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.ndThreshold.Name = "ndThreshold";
            this.ndThreshold.Size = new System.Drawing.Size(72, 20);
            this.ndThreshold.TabIndex = 1;
            this.ndThreshold.Tag = "DEFAULT";
            this.ndThreshold.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // ndFFTCutoff
            // 
            this.ndFFTCutoff.DecimalPlaces = 3;
            this.ndFFTCutoff.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.ndFFTCutoff.Location = new System.Drawing.Point(96, 68);
            this.ndFFTCutoff.Name = "ndFFTCutoff";
            this.ndFFTCutoff.Size = new System.Drawing.Size(72, 20);
            this.ndFFTCutoff.TabIndex = 2;
            this.ndFFTCutoff.Tag = "DEFAULT";
            this.ndFFTCutoff.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // rdLowPass
            // 
            this.rdLowPass.Checked = true;
            this.rdLowPass.Location = new System.Drawing.Point(8, 16);
            this.rdLowPass.Name = "rdLowPass";
            this.rdLowPass.Size = new System.Drawing.Size(160, 20);
            this.rdLowPass.TabIndex = 0;
            this.rdLowPass.TabStop = true;
            this.rdLowPass.Tag = "DEFAULT";
            this.rdLowPass.Text = "Low - Pass FFT Filters";
            this.rdLowPass.CheckedChanged += new System.EventHandler(this.rdLowPass_CheckedChanged);
            // 
            // radioButtonConstant
            // 
            this.radioButtonConstant.Location = new System.Drawing.Point(8, 96);
            this.radioButtonConstant.Name = "radioButtonConstant";
            this.radioButtonConstant.Size = new System.Drawing.Size(156, 20);
            this.radioButtonConstant.TabIndex = 3;
            this.radioButtonConstant.Tag = "DEFAULT";
            this.radioButtonConstant.Text = "Number of Erosion Filters:";
            this.radioButtonConstant.CheckedChanged += new System.EventHandler(this.radioButtonConstant_CheckedChanged);
            // 
            // nErosionFilters
            // 
            this.nErosionFilters.Location = new System.Drawing.Point(168, 96);
            this.nErosionFilters.Name = "nErosionFilters";
            this.nErosionFilters.Size = new System.Drawing.Size(72, 20);
            this.nErosionFilters.TabIndex = 4;
            this.nErosionFilters.Tag = "DEFAULT";
            this.nErosionFilters.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(28, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "Threshold:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCutOffUnit
            // 
            this.lblCutOffUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCutOffUnit.Location = new System.Drawing.Point(168, 68);
            this.lblCutOffUnit.Name = "lblCutOffUnit";
            this.lblCutOffUnit.Size = new System.Drawing.Size(64, 20);
            this.lblCutOffUnit.TabIndex = 29;
            this.lblCutOffUnit.Text = "(%)";
            this.lblCutOffUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblThresholdUnit
            // 
            this.lblThresholdUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThresholdUnit.Location = new System.Drawing.Point(168, 44);
            this.lblThresholdUnit.Name = "lblThresholdUnit";
            this.lblThresholdUnit.Size = new System.Drawing.Size(64, 20);
            this.lblThresholdUnit.TabIndex = 28;
            this.lblThresholdUnit.Text = "(Intensity)";
            this.lblThresholdUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnLoadSettings
            // 
            this.btnLoadSettings.Location = new System.Drawing.Point(261, 80);
            this.btnLoadSettings.Name = "btnLoadSettings";
            this.btnLoadSettings.Size = new System.Drawing.Size(88, 23);
            this.btnLoadSettings.TabIndex = 28;
            this.btnLoadSettings.Text = "Load Settings";
            this.btnLoadSettings.Click += new System.EventHandler(this.btnLoadSettings_Click);
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Location = new System.Drawing.Point(261, 109);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(88, 23);
            this.btnSaveSettings.TabIndex = 29;
            this.btnSaveSettings.Text = "Save Settings";
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // DlgExtractGlobalBackground
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(358, 136);
            this.Controls.Add(this.btnSaveSettings);
            this.Controls.Add(this.btnLoadSettings);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DlgExtractGlobalBackground";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Extract Global Background";
            this.Load += new System.EventHandler(this.ErosionImageDlg_Load);
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ndThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndFFTCutoff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nErosionFilters)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion
		
		#region member attributes
		
		private SIA.SystemLayer.CommonImage currenData;
		private string errorMess = string.Empty;
		
		#endregion

		#region public properties

		public double Threshold
		{
			get
			{
				return double.Parse(ndThreshold.Value.ToString());
			}
		}
		public float Cutoff
		{
			get
			{
				return float.Parse(ndFFTCutoff.Value.ToString());
			}
		}

		public int ErosionFilters
		{
			get
			{
				return int.Parse(nErosionFilters.Value.ToString());
			}
		}


		#endregion

		#region constructor and destructor
		
		public DlgExtractGlobalBackground( SIA.SystemLayer.CommonImage data )
		{
			//
			// Required for Windows Form Designer support
			//
			currenData = data;
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

		#region override routines

		protected override object OnGetDefaultValue(Control ctrl)
		{
			if (ctrl == rdLowPass)	
				return true;
			else if (ctrl == radioButtonConstant)
				return false;
			else if (ctrl == ndThreshold)
				return (Decimal)1;
			else if (ctrl == ndFFTCutoff)
				return (Decimal)2;
			else if (ctrl == nErosionFilters)
				return (Decimal)4;
			return null;
		}
		
		#endregion

		#region virtual routines


		#endregion

		#region event handlers

		private void ErosionImageDlg_Load(object sender, System.EventArgs e)
		{
			kUtils.SetMinMax( ndThreshold,currenData ); 

			ndThreshold.Enabled = rdLowPass.Checked;
			ndFFTCutoff.Enabled = rdLowPass.Checked;
			nErosionFilters.Enabled = !rdLowPass.Checked;
		}
				
		private void btnOk_Click(object sender, System.EventArgs e)
		{	
			kUtils.kBeginWaitCursor();
			
			if ( !InputValidate()) 
			{
				if ( errorMess != string.Empty )
					MessageBoxEx.Error(errorMess);
				errorMess = string.Empty;
				kUtils.kEndWaitCursor();
				return;	
			}
			
			this.DialogResult = DialogResult.OK;
			kUtils.kEndWaitCursor();
		}
		
		private void rdLowPass_CheckedChanged(object sender, System.EventArgs e)
		{
			if (!rdLowPass.Checked)return;
			ndThreshold.Enabled = true;
			ndFFTCutoff.Enabled = true;
			nErosionFilters.Enabled = false;
		}

		private void radioButtonConstant_CheckedChanged(object sender, System.EventArgs e)
		{
			if(!radioButtonConstant.Checked)return;
			ndThreshold.Enabled = false;
			ndFFTCutoff.Enabled = false;
			nErosionFilters.Enabled = true;
			//FileNameTxt.Enabled = false;
			
		}

		private void radioButtonDefect_CheckedChanged(object sender, System.EventArgs e)
		{
			//if(!radioButtonDefect.Checked)return;
			ndThreshold.Enabled = false;
			ndFFTCutoff.Enabled = false;
			nErosionFilters.Enabled = false;
			//FileNameTxt.Enabled = true;
		}

		#endregion

		#region internal routines

		private bool InputValidate()
		{
			if ( rdLowPass.Checked  && ! kUtils.IsInputValueValidate(ndThreshold))return false;

			if ( rdLowPass.Checked  && ! kUtils.IsInputValueValidate(ndFFTCutoff))return false;

			if ( radioButtonConstant.Checked && !kUtils.IsInputValueValidate(nErosionFilters)) return false;

			return true;
		}

		public int GetCurrentChecked()
		{
			if (radioButtonConstant.Checked)return 1;
			//if (radioButtonDefect.Checked)return 2;
			return 0;
		}

		#endregion

        private void btnLoadSettings_Click(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            try
            {
                using (OpenFileDialog dlg = CommonDialogs.OpenXmlFileDialog("Load settings"))
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        String filename = dlg.FileName;

                        ExtGlobalBckgSettings settings = ExtGlobalBckgSettings.Deserialize(filename);

                        Update(ref settings, true);

                        this.Invalidate(true);
                    }
                }
            }
            catch (System.Exception exp)
            {
                Trace.WriteLine(exp);
                MessageBoxEx.Error("Failed to load settings: " + exp.Message);
            }
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void SaveSettings()
        {
            try
            {
                if (InputValidate() == false)
                {
                    MessageBox.Show(this, "Settings are invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                using (SaveFileDialog dlg = CommonDialogs.SaveXmlFileDialog("Save settings as..."))
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        if (File.Exists(dlg.FileName.ToString()))
                        {
                            System.IO.FileAttributes fileAttribs = System.IO.File.GetAttributes(dlg.FileName.ToString());
                            if ((fileAttribs & System.IO.FileAttributes.ReadOnly) == System.IO.FileAttributes.ReadOnly)
                            {
                                MessageBoxEx.Error("The file is read only and can not be overridden.");
                                return;
                            }
                        }

                        String filename = dlg.FileName;

                        ExtGlobalBckgSettings settings = null;
                        this.Update(ref settings, false);

                        settings.Serialize(filename);
                    }
                }
            }
            catch (System.Exception exp)
            {
                Trace.WriteLine(exp);
                MessageBoxEx.Error("Failed to save settings: " + exp.Message);
            }
        }

        private void Update(ref ExtGlobalBckgSettings settings, bool toControl)
        {
            if (toControl)
            {
                if (settings == null)
                    return;

                ndThreshold.Value = (decimal)Math.Max(
                    (float)ndThreshold.Minimum, Math.Min((float)ndThreshold.Maximum, settings.FFT_Threshold));
                ndFFTCutoff.Value = (decimal)Math.Max(
                    (float)ndFFTCutoff.Minimum, Math.Min((float)ndFFTCutoff.Maximum, settings.FFT_CutOff));
                nErosionFilters.Value = (decimal)Math.Max(
                    (float)nErosionFilters.Minimum, Math.Min((float)nErosionFilters.Maximum, settings.NumberOfErosionFilters));

                rdLowPass.Checked = (settings.Method == eExtGlobalBckgMethod.FFT);
                radioButtonConstant.Checked = (settings.Method == eExtGlobalBckgMethod.Erosion);

                this.Invalidate(true);
            }
            else
            {
                this.Invalidate(true);

                if (radioButtonConstant.Checked)
                {
                    settings = new ExtGlobalBckgSettings((int)nErosionFilters.Value);
                }
                else
                {
                    settings = new ExtGlobalBckgSettings(
                        (float)ndThreshold.Value, (float)ndFFTCutoff.Value);
                }
            }
        }
	}
}
