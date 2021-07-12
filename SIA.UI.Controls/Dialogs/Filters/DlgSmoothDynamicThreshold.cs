using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using SIA.Common;
using SIA.Common.KlarfExport;

namespace SIA.UI.Controls.Dialogs
{
	/// <summary>
	/// Name : DlgSmoothDynamicThreshold
	/// Description : User interface for Smooth Dynamic Threshold
	/// Thread Support : false
	/// Persistence Data : True
	/// </summary>
	public class DlgSmoothDynamicThreshold : DialogBase
	{
		#region Windows Form members

		public System.Windows.Forms.NumericUpDown nudKernelSize;
		private System.Windows.Forms.Label Radius;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown nudConstant;
		private System.Windows.Forms.GroupBox separator;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOk;
		public System.Windows.Forms.RadioButton radGauss;
		public System.Windows.Forms.RadioButton radMean;
		private System.Windows.Forms.Label lblFilterType;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#endregion
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;

		#region member attributes
		
		private SmoothThresholdParameters _settings = null;

		#endregion

		#region constructor and destructor
		public DlgSmoothDynamicThreshold()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		public DlgSmoothDynamicThreshold(SmoothThresholdParameters settings)
		{
			if (settings == null)
				throw new System.ArgumentNullException("Invalid parameter");
			_settings = settings;

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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgSmoothDynamicThreshold));
			this.nudKernelSize = new System.Windows.Forms.NumericUpDown();
			this.Radius = new System.Windows.Forms.Label();
			this.radGauss = new System.Windows.Forms.RadioButton();
			this.radMean = new System.Windows.Forms.RadioButton();
			this.nudConstant = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.separator = new System.Windows.Forms.GroupBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.lblFilterType = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.nudKernelSize)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudConstant)).BeginInit();
			this.SuspendLayout();
			// 
			// nudKernelSize
			// 
			this.nudKernelSize.Location = new System.Drawing.Point(86, 72);
			this.nudKernelSize.Maximum = new System.Decimal(new int[] {
																		  15,
																		  0,
																		  0,
																		  0});
			this.nudKernelSize.Minimum = new System.Decimal(new int[] {
																		  1,
																		  0,
																		  0,
																		  0});
			this.nudKernelSize.Name = "nudKernelSize";
			this.nudKernelSize.Size = new System.Drawing.Size(63, 20);
			this.nudKernelSize.TabIndex = 9;
			this.nudKernelSize.Tag = "DEFAULT";
			this.nudKernelSize.Value = new System.Decimal(new int[] {
																		2,
																		0,
																		0,
																		0});
			// 
			// Radius
			// 
			this.Radius.Location = new System.Drawing.Point(17, 72);
			this.Radius.Name = "Radius";
			this.Radius.Size = new System.Drawing.Size(68, 20);
			this.Radius.TabIndex = 2;
			this.Radius.Text = "Kernel Size:";
			this.Radius.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// radGauss
			// 
			this.radGauss.Location = new System.Drawing.Point(43, 48);
			this.radGauss.Name = "radGauss";
			this.radGauss.Size = new System.Drawing.Size(127, 20);
			this.radGauss.TabIndex = 1;
			this.radGauss.Tag = "DEFAULT";
			this.radGauss.Text = "Gaussian Filter";
			// 
			// radMean
			// 
			this.radMean.Checked = true;
			this.radMean.Location = new System.Drawing.Point(43, 24);
			this.radMean.Name = "radMean";
			this.radMean.Size = new System.Drawing.Size(127, 20);
			this.radMean.TabIndex = 0;
			this.radMean.TabStop = true;
			this.radMean.Tag = "DEFAULT";
			this.radMean.Text = "Mean Filter";
			// 
			// nudConstant
			// 
			this.nudConstant.Location = new System.Drawing.Point(86, 96);
			this.nudConstant.Name = "nudConstant";
			this.nudConstant.Size = new System.Drawing.Size(63, 20);
			this.nudConstant.TabIndex = 19;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(17, 96);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(68, 20);
			this.label1.TabIndex = 13;
			this.label1.Text = "Constant:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// separator
			// 
			this.separator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.separator.Location = new System.Drawing.Point(-47, 126);
			this.separator.Name = "separator";
			this.separator.Size = new System.Drawing.Size(320, 3);
			this.separator.TabIndex = 16;
			this.separator.TabStop = false;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(120, 134);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(78, 23);
			this.btnCancel.TabIndex = 18;
			this.btnCancel.Text = "Cancel";
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(28, 134);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(78, 23);
			this.btnOk.TabIndex = 17;
			this.btnOk.Text = "OK";
			// 
			// lblFilterType
			// 
			this.lblFilterType.Location = new System.Drawing.Point(23, 4);
			this.lblFilterType.Name = "lblFilterType";
			this.lblFilterType.Size = new System.Drawing.Size(83, 20);
			this.lblFilterType.TabIndex = 13;
			this.lblFilterType.Text = "Filter Type:";
			this.lblFilterType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(153, 72);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(57, 20);
			this.label2.TabIndex = 2;
			this.label2.Text = "(pixels)";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(153, 96);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(57, 20);
			this.label3.TabIndex = 2;
			this.label3.Text = "(Intensity)";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// DlgSmoothDynamicThreshold
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(226, 161);
			this.Controls.Add(this.separator);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.nudConstant);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.nudKernelSize);
			this.Controls.Add(this.Radius);
			this.Controls.Add(this.radGauss);
			this.Controls.Add(this.radMean);
			this.Controls.Add(this.lblFilterType);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label3);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DlgSmoothDynamicThreshold";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Smooth Threshold Settings";
			((System.ComponentModel.ISupportInitialize)(this.nudKernelSize)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudConstant)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region override routines
		protected override void OnLoad(EventArgs e)
		{
			nudKernelSize.Minimum = (Decimal)2.0F;
			nudKernelSize.Maximum = (Decimal)30.0F;

			nudConstant.Minimum = (Decimal)0.0F;
			nudConstant.Maximum = Decimal.MaxValue;

			try
			{
				this.UpdateData(false);
			}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);
			}

			base.OnLoad (e);
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			try
			{
				this.UpdateData(true);
			}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);
			}

			base.OnClosing (e);
		}

		
		protected override object OnGetDefaultValue(Control ctrl)
		{
			if (ctrl == radMean)
				return true;
			else if (ctrl == radGauss)
				return false;
			else if (ctrl == nudKernelSize)
				return (Decimal)2.0F;
			else if (ctrl == nudConstant)
				return 0.0F;

			return null;
		}

		#endregion

		#region internal helpers

		private void UpdateData(bool bSaveAndValidate)
		{
			if (_settings != null)
			{
				SmoothThresholdParameters parameters = _settings;

				if (bSaveAndValidate)
				{				
					if (radMean.Checked)
						parameters.FilterType = SmoothFilterType.Mean;
					else 
						parameters.FilterType = SmoothFilterType.Gaussian;
					parameters.KernelRadius = (int)Math.Round((nudKernelSize.Value + 1) / 2);
					parameters.Constant = (float)nudConstant.Value;
				}
				else
				{
					radMean.Checked = parameters.FilterType == SmoothFilterType.Mean;
					radGauss.Checked = parameters.FilterType == SmoothFilterType.Gaussian;
					nudKernelSize.Value = (Decimal)(parameters.KernelRadius * 2);
					nudConstant.Value = (Decimal)(parameters.Constant);
				}
			}			
		}
		#endregion
	}
}
