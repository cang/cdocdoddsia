using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using SIA.SystemLayer;
using SIA.UI.Controls.Commands;
using SIA.UI.Controls.Utilities;

namespace SIA.UI.Controls.Dialogs
{
	/// <summary>
	/// Name : DlgResize
	/// Description : User interface for Image Rotation
	/// Thread Support : True
	/// Persistence Data : True
	/// </summary>
	public class DlgResize : DialogBase
	{
		#region Windows Form member

		private System.Windows.Forms.GroupBox grpParameters;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblResampling;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox cbResampling;
		private System.Windows.Forms.RadioButton byPercentage;
		private System.Windows.Forms.RadioButton byAbsoluteSize;
		private System.Windows.Forms.CheckBox chkMaintainAspectRatio;
		private System.Windows.Forms.NumericUpDown nudWidth;
		private System.Windows.Forms.NumericUpDown nudHeight;
		private System.Windows.Forms.NumericUpDown nudPercentage;
		private System.Windows.Forms.Panel pnlByAbsoluteSize;
		private System.Windows.Forms.Panel pnlByPercentage;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion

		#region Member Fields

		private ResizeImageCommandSettings _settings = new ResizeImageCommandSettings();
		private CommonImage _image = null;
		private int _updateCounter = 0;
		
		#endregion

		#region constructor/destructor
		
		public DlgResize(CommonImage image) : base()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// initialize resampling methods
			this.InitializeResamplingType();

			// initialize image size range
			nudWidth.Minimum = nudHeight.Minimum = 1;
			
            //nudWidth.Value = nudHeight.Value = 1;
			
            nudWidth.Maximum = (decimal)Math.Max(10000, image.Width);
			nudHeight.Maximum = (decimal)Math.Max(10000, image.Height);

			nudWidth.Value = Convert.ToDecimal(image.Width);
			nudHeight.Value = Convert.ToDecimal(image.Height);

			nudPercentage.Maximum = 1000;
			nudPercentage.Minimum = 1;
			nudPercentage.Value = 100;

			_image = image;			
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			// release reference
			this._image = null;
			this._settings = null;

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgResize));
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.grpParameters = new System.Windows.Forms.GroupBox();
			this.lblResampling = new System.Windows.Forms.Label();
			this.cbResampling = new System.Windows.Forms.ComboBox();
			this.byPercentage = new System.Windows.Forms.RadioButton();
			this.byAbsoluteSize = new System.Windows.Forms.RadioButton();
			this.nudPercentage = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.chkMaintainAspectRatio = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.nudWidth = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.nudHeight = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.pnlByAbsoluteSize = new System.Windows.Forms.Panel();
			this.pnlByPercentage = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.nudPercentage)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudWidth)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudHeight)).BeginInit();
			this.pnlByAbsoluteSize.SuspendLayout();
			this.pnlByPercentage.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(58, 192);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 24);
			this.btnOK.TabIndex = 7;
			this.btnOK.Text = "OK";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(154, 192);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 24);
			this.btnCancel.TabIndex = 8;
			this.btnCancel.Text = "Cancel";
			// 
			// grpParameters
			// 
			this.grpParameters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.grpParameters.Location = new System.Drawing.Point(-1, 184);
			this.grpParameters.Name = "grpParameters";
			this.grpParameters.Size = new System.Drawing.Size(336, 4);
			this.grpParameters.TabIndex = 6;
			this.grpParameters.TabStop = false;
			// 
			// lblResampling
			// 
			this.lblResampling.Location = new System.Drawing.Point(4, 4);
			this.lblResampling.Name = "lblResampling";
			this.lblResampling.Size = new System.Drawing.Size(72, 20);
			this.lblResampling.TabIndex = 0;
			this.lblResampling.Text = "Resampling:";
			this.lblResampling.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbResampling
			// 
			this.cbResampling.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbResampling.Location = new System.Drawing.Point(76, 4);
			this.cbResampling.Name = "cbResampling";
			this.cbResampling.Size = new System.Drawing.Size(204, 21);
			this.cbResampling.TabIndex = 1;
			// 
			// byPercentage
			// 
			this.byPercentage.Location = new System.Drawing.Point(8, 36);
			this.byPercentage.Name = "byPercentage";
			this.byPercentage.Size = new System.Drawing.Size(104, 20);
			this.byPercentage.TabIndex = 2;
			this.byPercentage.Text = "By Percentage:";
			this.byPercentage.CheckedChanged += new System.EventHandler(this.byPercentage_CheckedChanged);
			// 
			// byAbsoluteSize
			// 
			this.byAbsoluteSize.Location = new System.Drawing.Point(8, 64);
			this.byAbsoluteSize.Name = "byAbsoluteSize";
			this.byAbsoluteSize.Size = new System.Drawing.Size(116, 20);
			this.byAbsoluteSize.TabIndex = 4;
			this.byAbsoluteSize.Text = "By Absolute Size:";
			this.byAbsoluteSize.CheckedChanged += new System.EventHandler(this.byAbsoluteSize_CheckedChanged);
			// 
			// nudPercentage
			// 
			this.nudPercentage.Location = new System.Drawing.Point(4, 4);
			this.nudPercentage.Name = "nudPercentage";
			this.nudPercentage.Size = new System.Drawing.Size(68, 20);
			this.nudPercentage.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(76, 4);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(16, 20);
			this.label1.TabIndex = 1;
			this.label1.Text = "%";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// chkMaintainAspectRatio
			// 
			this.chkMaintainAspectRatio.Location = new System.Drawing.Point(4, 8);
			this.chkMaintainAspectRatio.Name = "chkMaintainAspectRatio";
			this.chkMaintainAspectRatio.Size = new System.Drawing.Size(248, 20);
			this.chkMaintainAspectRatio.TabIndex = 0;
			this.chkMaintainAspectRatio.Text = "Maintain aspect ratio";
			this.chkMaintainAspectRatio.CheckedChanged += new System.EventHandler(this.chkMaintainAspectRatio_CheckedChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(28, 36);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(44, 20);
			this.label2.TabIndex = 1;
			this.label2.Text = "Width:";
			// 
			// nudWidth
			// 
			this.nudWidth.Location = new System.Drawing.Point(80, 36);
			this.nudWidth.Name = "nudWidth";
			this.nudWidth.Size = new System.Drawing.Size(100, 20);
			this.nudWidth.TabIndex = 2;
			this.nudWidth.ValueChanged += new System.EventHandler(this.nudWidth_ValueChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(28, 64);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(44, 20);
			this.label3.TabIndex = 4;
			this.label3.Text = "Height:";
			// 
			// nudHeight
			// 
			this.nudHeight.Location = new System.Drawing.Point(80, 64);
			this.nudHeight.Name = "nudHeight";
			this.nudHeight.Size = new System.Drawing.Size(100, 20);
			this.nudHeight.TabIndex = 5;
			this.nudHeight.ValueChanged += new System.EventHandler(this.nudHeight_ValueChanged);
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label4.Location = new System.Drawing.Point(184, 36);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(44, 20);
			this.label4.TabIndex = 3;
			this.label4.Text = "(pixel)";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label5.Location = new System.Drawing.Point(184, 64);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(44, 20);
			this.label5.TabIndex = 6;
			this.label5.Text = "(pixel)";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pnlByAbsoluteSize
			// 
			this.pnlByAbsoluteSize.Controls.Add(this.label3);
			this.pnlByAbsoluteSize.Controls.Add(this.chkMaintainAspectRatio);
			this.pnlByAbsoluteSize.Controls.Add(this.label4);
			this.pnlByAbsoluteSize.Controls.Add(this.nudHeight);
			this.pnlByAbsoluteSize.Controls.Add(this.nudWidth);
			this.pnlByAbsoluteSize.Controls.Add(this.label5);
			this.pnlByAbsoluteSize.Controls.Add(this.label2);
			this.pnlByAbsoluteSize.Location = new System.Drawing.Point(15, 88);
			this.pnlByAbsoluteSize.Name = "pnlByAbsoluteSize";
			this.pnlByAbsoluteSize.Size = new System.Drawing.Size(256, 92);
			this.pnlByAbsoluteSize.TabIndex = 5;
			// 
			// pnlByPercentage
			// 
			this.pnlByPercentage.Controls.Add(this.label1);
			this.pnlByPercentage.Controls.Add(this.nudPercentage);
			this.pnlByPercentage.Location = new System.Drawing.Point(112, 32);
			this.pnlByPercentage.Name = "pnlByPercentage";
			this.pnlByPercentage.Size = new System.Drawing.Size(156, 28);
			this.pnlByPercentage.TabIndex = 3;
			// 
			// DlgResize
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(286, 220);
			this.Controls.Add(this.pnlByPercentage);
			this.Controls.Add(this.pnlByAbsoluteSize);
			this.Controls.Add(this.byPercentage);
			this.Controls.Add(this.cbResampling);
			this.Controls.Add(this.lblResampling);
			this.Controls.Add(this.grpParameters);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.byAbsoluteSize);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DlgResize";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Resize";
			((System.ComponentModel.ISupportInitialize)(this.nudPercentage)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudWidth)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudHeight)).EndInit();
			this.pnlByAbsoluteSize.ResumeLayout(false);
			this.pnlByPercentage.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region public properties

		public ResizeImageCommandSettings Settings
		{
			get {
				return _settings;
			}
			set {
				_settings = value;
			}
		}

		#endregion
	
		#region event handlers

		private void byPercentage_CheckedChanged(object sender, System.EventArgs e)
		{
			if (byPercentage.Checked == false)
				return ;

			if (this.BeginUpdate())
			{
				try
				{
					// enable panel by Percentage
					pnlByPercentage.Enabled = true;

					// disable panel by Absolute Size
					pnlByAbsoluteSize.Enabled = false;
				}
				finally
				{
					this.EndUpdate();
				}				
			}
		}

		private void byAbsoluteSize_CheckedChanged(object sender, System.EventArgs e)
		{
			if (byAbsoluteSize.Checked == false)
				return;

			if (this.BeginUpdate())
			{
				try
				{
					// disable panel by Percentage
					pnlByPercentage.Enabled = false;

					// enable panel by Absolute Size
					pnlByAbsoluteSize.Enabled = true;
				}
				finally
				{
					this.EndUpdate();
				}				
			}
		}

		private void chkMaintainAspectRatio_CheckedChanged(object sender, System.EventArgs e)
		{
			if (chkMaintainAspectRatio.Checked == false)
				return ;

			if (this.BeginUpdate())
			{
				try
				{
					// reset width and height
					nudWidth.Value = Convert.ToDecimal(this._image.Width);
					nudHeight.Value = Convert.ToDecimal(this._image.Height);
				}
				finally
				{
					this.EndUpdate();
				}				
			}
		}

		private void nudWidth_ValueChanged(object sender, System.EventArgs e)
		{
			if (this.BeginUpdate())
			{
				try
				{
					// if maintain aspect ratio
					if (chkMaintainAspectRatio.Checked)
					{
						int newHeight = (int)Math.Round(Convert.ToSingle(nudWidth.Value)*_image.Height / _image.Width);
						nudHeight.Value = Convert.ToDecimal(newHeight);
					}
				}
				finally
				{
					this.EndUpdate();
				}				
			}
		}

		private void nudHeight_ValueChanged(object sender, System.EventArgs e)
		{
			if (this.BeginUpdate())
			{
				try
				{
					// if maintain aspect ratio
					if (chkMaintainAspectRatio.Checked)
					{
						int newWidth = (int)Math.Round(Convert.ToSingle(nudHeight.Value)*_image.Width / _image.Height);
						nudWidth.Value = Convert.ToDecimal(newWidth);
					}
				}
				finally
				{
					this.EndUpdate();
				}				
			}
		}

		#endregion

		#region override routines

		protected override void OnLoad(EventArgs e)
		{
			this.UpdateData(false);

			base.OnLoad (e);			
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed (e);

			if (this.DialogResult == DialogResult.OK)
				this.UpdateData(true);
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing (e);

			// update data
			if (this.Validate())
				this.UpdateData(true);
		}

		protected override void OnValidating(CancelEventArgs e)
		{
			base.OnValidating (e);

			if (byPercentage.Checked)
			{
				if (nudPercentage.Value <= 0)
				{
					MessageBoxEx.Error("Invalid percentage value. Please try again");
					e.Cancel = true;
				}
			}
			else 
			{
				if (nudWidth.Value == 0 || nudHeight.Value == 0)
				{
					MessageBoxEx.Error("Invalid destination image size's value. Please try again");
					e.Cancel = true;
				}

			}
		}

		#region DialogBase override
		
		protected override object OnGetDefaultValue(System.Windows.Forms.Control ctrl)
		{
			if (ctrl == cbResampling)
				return 0;
			else if (ctrl == byPercentage)
				return true;
			else if (ctrl == byAbsoluteSize)
				return false;
			else if (ctrl == nudPercentage)
				return Convert.ToDecimal(100);
			else if (ctrl == chkMaintainAspectRatio)
				return false;
			return null;
		}

		#endregion

		#endregion

		#region Internal Helpers

		private void InitializeResamplingType()
		{
			cbResampling.BeginUpdate();
			cbResampling.Items.Add("Nearest Neighbor");
			cbResampling.Items.Add("Bicubic");
			cbResampling.SelectedIndex = 0;
			cbResampling.EndUpdate();
		}

		private bool BeginUpdate()
		{
			if (_updateCounter > 0)
				return false;
			
			_updateCounter++;

			return true;
		}

		private void EndUpdate()
		{
			_updateCounter--;
		}

		private void UpdateData(bool bSaveAndValidate)
		{
			if (bSaveAndValidate)
			{
				this._settings.SamplingType = this.cbResampling.SelectedIndex;
				if (byPercentage.Checked)
				{
					this._settings.ResizeBy = ResizeBy.Percentage;
					this._settings.Percentage = Convert.ToSingle(nudPercentage.Value);
				}
				else if (byAbsoluteSize.Checked)
				{
					this._settings.ResizeBy = ResizeBy.AbsoluteSize;
					this._settings.MaintainAspectRatio = chkMaintainAspectRatio.Checked;
					this._settings.Width = Convert.ToInt32(nudWidth.Value);
					this._settings.Height = Convert.ToInt32(nudHeight.Value);
				}
			}
			else
			{
				this.cbResampling.SelectedIndex = this._settings.SamplingType;
				if (this._settings.ResizeBy == ResizeBy.Percentage)
				{
					byPercentage.Checked = true;
					nudPercentage.Value = Convert.ToDecimal(this._settings.Percentage);
				}
				else
				{
					byAbsoluteSize.Checked = true;
					chkMaintainAspectRatio.Checked = this._settings.MaintainAspectRatio;
					nudWidth.Value = Convert.ToDecimal(this._settings.Width);
					nudHeight.Value = Convert.ToDecimal(this._settings.Height);
				}
			}
		}

		#endregion

		
	}
}
