using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using SIA.Common.Imaging.Filters;

using SIA.SystemFrameworks;
using SIA.SystemFrameworks.UI;

using SIA.UI.Components;
using SIA.UI.Components.Helpers;

using SIA.UI.Controls.Utilities;
using SIA.UI.Controls.Automation.Commands;

namespace SIA.UI.Controls.Dialogs
{
	/// <summary>
	/// Name : DlgFFTFilter
	/// Description : User interface for Fourier Filter
	/// Thread Support : True
	/// Persistence Data : True
	/// </summary>
	public class DlgFFTFilter : SIA.UI.Controls.Dialogs.DialogPreviewBase
	{
		#region Windows Form members
		private System.Windows.Forms.ErrorProvider errorProvider1;
		public System.Windows.Forms.NumericUpDown nudCutOff;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBoxPreview;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.GroupBox grpParameters;
		private System.Windows.Forms.TrackBar tbCutOff;
		private SIA.UI.Components.ImagePreview _imagePreview;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown nudWeight;

		public System.Windows.Forms.RadioButton radHighPass;
		public System.Windows.Forms.RadioButton radLowPass;
		public System.Windows.Forms.Label label1;
		public System.Windows.Forms.Button OKButton;
		public System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnPreview;
		private System.Windows.Forms.Button btnSaveSettings;
		private System.Windows.Forms.Button btnLoadSettings;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		
		#endregion

		#region constructor and destructor
		public DlgFFTFilter(SIA.UI.Controls.ImageWorkspace owner) : base(owner, true)
		{
			//
			// Required for Windows Form Designer support
			//
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

		#region public properties

		public FFTFilterType FilterType
		{
			get 
			{
				return radHighPass.Checked ? FFTFilterType.HighPass : FFTFilterType.LowPass;
			}
            set
            {
                radHighPass.Checked = (value == FFTFilterType.HighPass);
                radLowPass.Checked = (value == FFTFilterType.LowPass);
            }
		}

		public float Weight
		{
			get { return (float) nudWeight.Value ;}
            set
            {
                try
                {
                    nudWeight.Value = (decimal)value;
                }
                catch
                {
                }
            }
		}

		public float CutOff
		{
			get {return (float)nudCutOff.Value;}
            set
            {
                try
                {
                    nudCutOff.Value = (decimal)value;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgFFTFilter));
			this.radHighPass = new System.Windows.Forms.RadioButton();
			this.radLowPass = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.OKButton = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.nudCutOff = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBoxPreview = new System.Windows.Forms.GroupBox();
			this._imagePreview = new SIA.UI.Components.ImagePreview();
			this.btnReset = new System.Windows.Forms.Button();
			this.btnPreview = new System.Windows.Forms.Button();
			this.grpParameters = new System.Windows.Forms.GroupBox();
			this.label4 = new System.Windows.Forms.Label();
			this.nudWeight = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tbCutOff = new System.Windows.Forms.TrackBar();
			this.btnSaveSettings = new System.Windows.Forms.Button();
			this.btnLoadSettings = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.nudCutOff)).BeginInit();
			this.groupBoxPreview.SuspendLayout();
			this.grpParameters.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudWeight)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbCutOff)).BeginInit();
			this.SuspendLayout();
			// 
			// radHighPass
			// 
			this.radHighPass.Location = new System.Drawing.Point(6, 60);
			this.radHighPass.Name = "radHighPass";
			this.radHighPass.Size = new System.Drawing.Size(122, 16);
			this.radHighPass.TabIndex = 0;
			this.radHighPass.Tag = "DEFAULT";
			this.radHighPass.Text = "High-Pass";
			this.radHighPass.CheckedChanged += new System.EventHandler(this.radHighPass_CheckedChanged);
			// 
			// radLowPass
			// 
			this.radLowPass.Checked = true;
			this.radLowPass.Location = new System.Drawing.Point(144, 60);
			this.radLowPass.Name = "radLowPass";
			this.radLowPass.Size = new System.Drawing.Size(122, 16);
			this.radLowPass.TabIndex = 1;
			this.radLowPass.TabStop = true;
			this.radLowPass.Tag = "DEFAULT";
			this.radLowPass.Text = "Low-Pass";
			this.radLowPass.CheckedChanged += new System.EventHandler(this.radLowPass_CheckedChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 83);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(44, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Cut off:";
			// 
			// OKButton
			// 
			this.OKButton.Location = new System.Drawing.Point(284, 12);
			this.OKButton.Name = "OKButton";
			this.OKButton.Size = new System.Drawing.Size(88, 23);
			this.OKButton.TabIndex = 2;
			this.OKButton.Text = "OK";
			this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(284, 40);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(88, 23);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// nudCutOff
			// 
			this.nudCutOff.DecimalPlaces = 3;
			this.nudCutOff.Increment = new System.Decimal(new int[] {
																		1,
																		0,
																		0,
																		131072});
			this.nudCutOff.Location = new System.Drawing.Point(68, 80);
			this.nudCutOff.Name = "nudCutOff";
			this.nudCutOff.Size = new System.Drawing.Size(60, 20);
			this.nudCutOff.TabIndex = 3;
			this.nudCutOff.Tag = "DEFAULT";
			this.nudCutOff.Value = new System.Decimal(new int[] {
																	50,
																	0,
																	0,
																	0});
			this.nudCutOff.ValueChanged += new System.EventHandler(this.UpdatePreviewImage);
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(132, 83);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(25, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "(%)";
			// 
			// groupBoxPreview
			// 
			this.groupBoxPreview.Controls.Add(this._imagePreview);
			this.groupBoxPreview.Controls.Add(this.btnReset);
			this.groupBoxPreview.Controls.Add(this.btnPreview);
			this.groupBoxPreview.Location = new System.Drawing.Point(4, 7);
			this.groupBoxPreview.Name = "groupBoxPreview";
			this.groupBoxPreview.Size = new System.Drawing.Size(272, 304);
			this.groupBoxPreview.TabIndex = 0;
			this.groupBoxPreview.TabStop = false;
			this.groupBoxPreview.Text = "Preview";
			// 
			// _imagePreview
			// 
			this._imagePreview.ImageViewer = null;
			this._imagePreview.Location = new System.Drawing.Point(8, 16);
			this._imagePreview.Name = "_imagePreview";
			this._imagePreview.PreviewRectangle = new System.Drawing.Rectangle(0, 0, 0, 0);
			this._imagePreview.Size = new System.Drawing.Size(256, 256);
			this._imagePreview.TabIndex = 2;
			// 
			// btnReset
			// 
			this.btnReset.Location = new System.Drawing.Point(8, 276);
			this.btnReset.Name = "btnReset";
			this.btnReset.Size = new System.Drawing.Size(60, 23);
			this.btnReset.TabIndex = 1;
			this.btnReset.Text = "Reset";
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// btnPreview
			// 
			this.btnPreview.Location = new System.Drawing.Point(76, 276);
			this.btnPreview.Name = "btnPreview";
			this.btnPreview.Size = new System.Drawing.Size(60, 23);
			this.btnPreview.TabIndex = 1;
			this.btnPreview.Text = "Preview";
			this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
			// 
			// grpParameters
			// 
			this.grpParameters.Controls.Add(this.label4);
			this.grpParameters.Controls.Add(this.nudWeight);
			this.grpParameters.Controls.Add(this.label3);
			this.grpParameters.Controls.Add(this.groupBox1);
			this.grpParameters.Controls.Add(this.tbCutOff);
			this.grpParameters.Controls.Add(this.radHighPass);
			this.grpParameters.Controls.Add(this.radLowPass);
			this.grpParameters.Controls.Add(this.nudCutOff);
			this.grpParameters.Controls.Add(this.label1);
			this.grpParameters.Controls.Add(this.label2);
			this.grpParameters.Location = new System.Drawing.Point(4, 312);
			this.grpParameters.Name = "grpParameters";
			this.grpParameters.Size = new System.Drawing.Size(272, 120);
			this.grpParameters.TabIndex = 1;
			this.grpParameters.TabStop = false;
			this.grpParameters.Text = "Parameters";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label4.Location = new System.Drawing.Point(132, 24);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(28, 13);
			this.label4.TabIndex = 9;
			this.label4.Text = "(%)";
			// 
			// nudWeight
			// 
			this.nudWeight.DecimalPlaces = 1;
			this.nudWeight.Location = new System.Drawing.Point(68, 20);
			this.nudWeight.Name = "nudWeight";
			this.nudWeight.Size = new System.Drawing.Size(60, 20);
			this.nudWeight.TabIndex = 8;
			this.nudWeight.Tag = "DEFAULT";
			this.nudWeight.ValueChanged += new System.EventHandler(this.nudWeight_ValueChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(8, 24);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(43, 16);
			this.label3.TabIndex = 7;
			this.label3.Text = "Weight:";
			// 
			// groupBox1
			// 
			this.groupBox1.Location = new System.Drawing.Point(4, 44);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(264, 8);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			// 
			// tbCutOff
			// 
			this.tbCutOff.AutoSize = false;
			this.tbCutOff.LargeChange = 20;
			this.tbCutOff.Location = new System.Drawing.Point(8, 100);
			this.tbCutOff.Maximum = 100;
			this.tbCutOff.Name = "tbCutOff";
			this.tbCutOff.Size = new System.Drawing.Size(256, 16);
			this.tbCutOff.SmallChange = 10;
			this.tbCutOff.TabIndex = 5;
			this.tbCutOff.TickStyle = System.Windows.Forms.TickStyle.None;
			this.tbCutOff.Value = 50;
			this.tbCutOff.ValueChanged += new System.EventHandler(this.UpdatePreviewImage);
			// 
			// btnSaveSettings
			// 
			this.btnSaveSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.btnSaveSettings.Location = new System.Drawing.Point(284, 96);
			this.btnSaveSettings.Name = "btnSaveSettings";
			this.btnSaveSettings.Size = new System.Drawing.Size(88, 23);
			this.btnSaveSettings.TabIndex = 11;
			this.btnSaveSettings.Text = "Save Settings";
			this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
			// 
			// btnLoadSettings
			// 
			this.btnLoadSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.btnLoadSettings.Enabled = true;
			this.btnLoadSettings.Location = new System.Drawing.Point(284, 68);
			this.btnLoadSettings.Name = "btnLoadSettings";
			this.btnLoadSettings.Size = new System.Drawing.Size(88, 23);
			this.btnLoadSettings.TabIndex = 12;
			this.btnLoadSettings.Text = "Load Settings";
            this.btnLoadSettings.Click += new EventHandler(btnLoadSettings_Click);
			// 
			// DlgFFTFilter
			// 
			this.AcceptButton = this.OKButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(378, 436);
			this.Controls.Add(this.btnLoadSettings);
			this.Controls.Add(this.btnSaveSettings);
			this.Controls.Add(this.grpParameters);
			this.Controls.Add(this.groupBoxPreview);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.OKButton);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DlgFFTFilter";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Fourier Transform";
			this.Load += new System.EventHandler(this.FFTDialog_Load);
			((System.ComponentModel.ISupportInitialize)(this.nudCutOff)).EndInit();
			this.groupBoxPreview.ResumeLayout(false);
			this.grpParameters.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.nudWeight)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbCutOff)).EndInit();
			this.ResumeLayout(false);

		}        
		#endregion

		#region event handlers
		
		private void btnReset_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			this.ResetPreview();
			Cursor.Current = Cursors.Default;
		}

		private void btnPreview_Click(object sender, System.EventArgs e)
		{
			this.ApplyToPreview();
		}

		private void radLowPass_CheckedChanged(object sender, System.EventArgs e)
		{
			if ( !radLowPass.Checked ) return;
			UpdatePreviewImage(sender,e);
		}		

		private bool OkbtnClicked = true;
		private void OKButton_Click(object sender, System.EventArgs e)
		{
			OkbtnClicked  = false;
			if ( !kUtils.IsAllValueValidate(grpParameters))
			{
				OkbtnClicked  = true;
				return;
			}
			this.DialogResult = DialogResult.OK;
			OkbtnClicked  = true;
		}

		private bool loaded = false;
		private void FFTDialog_Load(object sender, System.EventArgs e)
		{			
			Init();
		}

		public void Init()
		{
			this.nudWeight.ValueChanged += new System.EventHandler(this.UpdatePreviewImage);
			loaded = true;
		}

		private void radHighPass_CheckedChanged(object sender, System.EventArgs e)
		{
			if ( !radHighPass.Checked ) return;
			UpdatePreviewImage(sender,e);
		}

		private void nudWeight_ValueChanged(object sender, System.EventArgs e)
		{
			UpdatePreviewImage(sender,e);
		}	
		
		#endregion

		#region override routines

		#region DialogPreviewBase override

		public override SIA.UI.Components.ImagePreview GetPreviewer()
		{
			return _imagePreview;
		}

		protected override void LockUserInputObjects()
		{			
			btnPreview.Enabled = false;
			btnReset.Enabled = false;
			radHighPass.Enabled = false;
			radLowPass.Enabled = false;
			nudCutOff.Enabled = false;
			nudWeight.Enabled = false;
			tbCutOff.Enabled = false;
		}

		protected override void UnlockUserInputObjects()
		{	
			btnPreview.Enabled = true;
			btnReset.Enabled = true;
			radHighPass.Enabled = true;
			radLowPass.Enabled = true;
			nudCutOff.Enabled = true;
			nudWeight.Enabled = true;
			tbCutOff.Enabled = true;
		}

		/// <summary>
		/// Apply Fourier transform.
		/// </summary>
		/// <param name="image">Image to filter Fourier transform.</param>
		public override void ApplyToCommonImage(SIA.SystemLayer.CommonImage image)
		{	
			if (image == null)
				throw new System.ArgumentNullException("Invalid parameter");

			try
			{
				image.FFTFilter(this.FilterType, this.CutOff, this.Weight);
			}
			catch(System.OutOfMemoryException exp)
			{
				throw exp;
			}
			catch(System.ApplicationException exp)
			{
				throw exp;
			}
			catch (System.Exception exp)
			{
				System.Diagnostics.Trace.WriteLine(exp);
			}
			finally
			{
			}
		}

		#endregion

		#region DialogBase override
		
		protected override object OnGetDefaultValue(Control ctrl)
		{
			if (ctrl == nudWeight)
				return (Decimal)80.0F;
			else if (ctrl == radHighPass)
				return (bool)false;
			else if (ctrl == radLowPass)
				return (bool)true;
			else if (ctrl == nudCutOff)
				return (Decimal)1.5F;

			return null;
		}

		#endregion
		
		#endregion

		#region internal helpers

		private void UpdatePreviewImage(object sender, System.EventArgs e)
		{
			if (sender == nudCutOff)
				tbCutOff.Value = (int)nudCutOff.Value;
			else
				if (sender == tbCutOff) 
			{
				if( loaded && OkbtnClicked )
					nudCutOff.Value = tbCutOff.Value;
			}
			
			this.ApplyToPreview();
		}

		#endregion

		private void btnSaveSettings_Click(object sender, System.EventArgs e)
		{
			SaveAsXML();
		}

		private void SaveAsXML()
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
					
						using (FilterFFTCommandSettings cmdSettings = new FilterFFTCommandSettings(this.FilterType, this.CutOff, this.Weight))
						{						
							SIA.UI.Controls.Automation.RasterCommandSettingsSerializer.Serialize(filename, cmdSettings);
						}
					}
				}
			}
			catch(System.Exception exp)
			{
                MessageBoxEx.Error("Failed to save settings: " + exp.Message);
			}
		}

        void btnLoadSettings_Click(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            try
            {
                using (OpenFileDialog dlg = CommonDialogs.OpenXmlFileDialog("Load Settings"))
                {
                    //dlg.FileName = "Untitled";

                    if (dlg.ShowDialog() == DialogResult.OK)
                    {                        
                        String filename = dlg.FileName;

                        FilterFFTCommandSettings cmdSettings =
                            (FilterFFTCommandSettings)SIA.UI.Controls.Automation.RasterCommandSettingsSerializer.Deserialize(
                            filename, typeof(FilterFFTCommandSettings));

                        if (cmdSettings != null)
                        {                            
                            this.CutOff = cmdSettings.CutOff;
                            this.Weight = cmdSettings.Weight;
                            this.FilterType = cmdSettings.Type;
                        }
                    }
                }
            }
            catch (System.Exception exp)
            {
                MessageBoxEx.Error("Failed to load settings: " + exp.Message);
            }
        }
	}
}
