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
	/// Name : DlgVarianceFilter
	/// Description : User interface for Variance Filter
	/// Thread Support : True
	/// Persistence Data : True
	/// </summary>
	public class DlgVarianceFilter : DialogPreviewBase
	{
		#region member attributes
		public SIA.UI.Components.ImagePreview _imagePreview; 
		
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.NumericUpDown nudRadius;
		private System.Windows.Forms.GroupBox groupBoxPreview;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.Button btnPreview;
		private System.Windows.Forms.GroupBox grpParameters;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lblRadius;
		private System.Windows.Forms.Label lblWeight;
		private System.Windows.Forms.NumericUpDown nudWeight;
		private System.Windows.Forms.Button btnSaveSettings;
		private System.Windows.Forms.Button btnLoadSettings;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#endregion

		#region constructor and destructor

		public DlgVarianceFilter(SIA.UI.Controls.ImageWorkspace owner) : base(owner, true)
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
		
		public float Radius
		{
			get
			{
				return (float)nudRadius.Value;
			}
		}

		public float Weight
		{
			get {return (float)nudWeight.Value;}
			set 
			{
				nudWeight.Value = (Decimal)value;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgVarianceFilter));
			this.lblRadius = new System.Windows.Forms.Label();
			this.nudRadius = new System.Windows.Forms.NumericUpDown();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.grpParameters = new System.Windows.Forms.GroupBox();
			this.lblWeight = new System.Windows.Forms.Label();
			this.nudWeight = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.groupBoxPreview = new System.Windows.Forms.GroupBox();
			this.btnReset = new System.Windows.Forms.Button();
			this._imagePreview = new SIA.UI.Components.ImagePreview();
			this.btnPreview = new System.Windows.Forms.Button();
			this.btnSaveSettings = new System.Windows.Forms.Button();
			this.btnLoadSettings = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.nudRadius)).BeginInit();
			this.grpParameters.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudWeight)).BeginInit();
			this.groupBoxPreview.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblRadius
			// 
			this.lblRadius.Location = new System.Drawing.Point(8, 16);
			this.lblRadius.Name = "lblRadius";
			this.lblRadius.Size = new System.Drawing.Size(46, 20);
			this.lblRadius.TabIndex = 0;
			this.lblRadius.Text = "Radius :";
			this.lblRadius.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// nudRadius
			// 
			this.nudRadius.DecimalPlaces = 2;
			this.nudRadius.Location = new System.Drawing.Point(60, 16);
			this.nudRadius.Maximum = new System.Decimal(new int[] {
																	  10,
																	  0,
																	  0,
																	  0});
			this.nudRadius.Minimum = new System.Decimal(new int[] {
																	  5,
																	  0,
																	  0,
																	  65536});
			this.nudRadius.Name = "nudRadius";
			this.nudRadius.Size = new System.Drawing.Size(52, 20);
			this.nudRadius.TabIndex = 14;
			this.nudRadius.Value = new System.Decimal(new int[] {
																	15,
																	0,
																	0,
																	65536});
			this.nudRadius.ValueChanged += new System.EventHandler(this.nudRadius_ValueChanged);
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
			// grpParameters
			// 
			this.grpParameters.Controls.Add(this.lblRadius);
			this.grpParameters.Controls.Add(this.nudRadius);
			this.grpParameters.Controls.Add(this.lblWeight);
			this.grpParameters.Controls.Add(this.nudWeight);
			this.grpParameters.Controls.Add(this.label3);
			this.grpParameters.Location = new System.Drawing.Point(4, 312);
			this.grpParameters.Name = "grpParameters";
			this.grpParameters.Size = new System.Drawing.Size(272, 44);
			this.grpParameters.TabIndex = 17;
			this.grpParameters.TabStop = false;
			this.grpParameters.Text = "Parameters";
			// 
			// lblWeight
			// 
			this.lblWeight.Location = new System.Drawing.Point(136, 16);
			this.lblWeight.Name = "lblWeight";
			this.lblWeight.Size = new System.Drawing.Size(46, 20);
			this.lblWeight.TabIndex = 0;
			this.lblWeight.Text = "Weight :";
			this.lblWeight.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// nudWeight
			// 
			this.nudWeight.DecimalPlaces = 1;
			this.nudWeight.Location = new System.Drawing.Point(184, 16);
			this.nudWeight.Name = "nudWeight";
			this.nudWeight.Size = new System.Drawing.Size(56, 20);
			this.nudWeight.TabIndex = 14;
			this.nudWeight.Value = new System.Decimal(new int[] {
																	500,
																	0,
																	0,
																	65536});
			this.nudWeight.ValueChanged += new System.EventHandler(this.nudWeight_ValueChanged);
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(244, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(24, 20);
			this.label3.TabIndex = 0;
			this.label3.Text = "(%)";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
			// btnSaveSettings
			// 
			this.btnSaveSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.btnSaveSettings.Location = new System.Drawing.Point(284, 92);
			this.btnSaveSettings.Name = "btnSaveSettings";
			this.btnSaveSettings.Size = new System.Drawing.Size(84, 23);
			this.btnSaveSettings.TabIndex = 19;
			this.btnSaveSettings.Text = "Save Settings";
			this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
			// 
			// btnLoadSettings
			// 
			this.btnLoadSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.btnLoadSettings.Enabled = false;
			this.btnLoadSettings.Location = new System.Drawing.Point(284, 64);
			this.btnLoadSettings.Name = "btnLoadSettings";
			this.btnLoadSettings.Size = new System.Drawing.Size(84, 23);
			this.btnLoadSettings.TabIndex = 20;
			this.btnLoadSettings.Text = "Load Settings";
			// 
			// DlgVarianceFilter
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(374, 360);
			this.Controls.Add(this.btnLoadSettings);
			this.Controls.Add(this.btnSaveSettings);
			this.Controls.Add(this.grpParameters);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.groupBoxPreview);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Location = new System.Drawing.Point(10, 50);
			this.Name = "DlgVarianceFilter";
			this.ShowInTaskbar = false;
			this.Text = "Variance Filter";
			((System.ComponentModel.ISupportInitialize)(this.nudRadius)).EndInit();
			this.grpParameters.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.nudWeight)).EndInit();
			this.groupBoxPreview.ResumeLayout(false);
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
			nudRadius.Enabled = false;
			nudWeight.Enabled = false;
		}
		
		protected override void UnlockUserInputObjects()
		{
			btnReset.Enabled = true;
			btnPreview.Enabled = true;
			nudRadius.Enabled = true;
			nudWeight.Enabled = true;
		}		

		#endregion

		#region DialogBase override
		
		protected override object OnGetDefaultValue(System.Windows.Forms.Control ctrl)
		{
			if (ctrl == nudRadius)
				return 1.0F;
			else if (ctrl == nudWeight)
				return 100.0F;
			return null;
		}

		#endregion

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			nudWeight.Minimum = (Decimal)0.0F;
			nudWeight.Maximum = (Decimal)100.0F;
		}
	
		#endregion

		#region operation routines
		
		public override void ApplyToCommonImage(SIA.SystemLayer.CommonImage image)
		{
			if (image == null)
				throw new System.ArgumentNullException("Invalid parameter");
			image.FilterVariance(this.Radius);
		}

		#endregion

		#region event handlers
		
		private void nudRadius_ValueChanged(object sender, System.EventArgs e)
		{
			this.ApplyToPreview();
		}

		private void nudWeight_ValueChanged(object sender, System.EventArgs e)
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

		#endregion

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
					
						using (FilterVarianceCommandSettings cmdSettings = new FilterVarianceCommandSettings(this.Radius))
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
	}
}
