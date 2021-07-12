using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using SIA.UI.Components;

namespace SIA.UI.Controls.Dialogs
{
	/// <summary>
	/// Name : DlgTranslate
	/// Description : User interface for Image Rotation
	/// Thread Support : True
	/// Persistence Data : True
	/// </summary>
	public class DlgTranslate : DialogBase
	{
		#region Windows Form member

		private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.GroupBox grpParameters;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblResampling;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox cbResampling;
		private System.Windows.Forms.RadioButton byPercentage;
		private System.Windows.Forms.RadioButton bySize;
		private System.Windows.Forms.CheckBox chkMaintainAspectRatio;
		private System.Windows.Forms.NumericUpDown nudWidth;
		private System.Windows.Forms.NumericUpDown nudHeight;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion

		#region constructor/destructor
		
		public DlgTranslate() 
            : base()
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgTranslate));
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.grpParameters = new System.Windows.Forms.GroupBox();
			this.lblResampling = new System.Windows.Forms.Label();
			this.cbResampling = new System.Windows.Forms.ComboBox();
			this.byPercentage = new System.Windows.Forms.RadioButton();
			this.bySize = new System.Windows.Forms.RadioButton();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.chkMaintainAspectRatio = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.nudWidth = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.nudHeight = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudWidth)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudHeight)).BeginInit();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(58, 180);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 24);
			this.btnOK.TabIndex = 2;
			this.btnOK.Text = "OK";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(154, 180);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 24);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// grpParameters
			// 
			this.grpParameters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.grpParameters.Location = new System.Drawing.Point(-1, 172);
			this.grpParameters.Name = "grpParameters";
			this.grpParameters.Size = new System.Drawing.Size(336, 4);
			this.grpParameters.TabIndex = 4;
			this.grpParameters.TabStop = false;
			// 
			// lblResampling
			// 
			this.lblResampling.Location = new System.Drawing.Point(4, 4);
			this.lblResampling.Name = "lblResampling";
			this.lblResampling.Size = new System.Drawing.Size(72, 20);
			this.lblResampling.TabIndex = 5;
			this.lblResampling.Text = "Resampling:";
			this.lblResampling.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbResampling
			// 
			this.cbResampling.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbResampling.Location = new System.Drawing.Point(76, 4);
			this.cbResampling.Name = "cbResampling";
			this.cbResampling.Size = new System.Drawing.Size(204, 21);
			this.cbResampling.TabIndex = 6;
			// 
			// byPercentage
			// 
			this.byPercentage.Location = new System.Drawing.Point(8, 36);
			this.byPercentage.Name = "byPercentage";
			this.byPercentage.Size = new System.Drawing.Size(104, 20);
			this.byPercentage.TabIndex = 7;
			this.byPercentage.Text = "By Percentage:";
			// 
			// bySize
			// 
			this.bySize.Location = new System.Drawing.Point(8, 64);
			this.bySize.Name = "bySize";
			this.bySize.Size = new System.Drawing.Size(116, 20);
			this.bySize.TabIndex = 7;
			this.bySize.Text = "By Absolute Size:";
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.Location = new System.Drawing.Point(120, 36);
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.TabIndex = 8;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(244, 36);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(16, 20);
			this.label1.TabIndex = 5;
			this.label1.Text = "%";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// chkMaintainAspectRatio
			// 
			this.chkMaintainAspectRatio.Location = new System.Drawing.Point(32, 88);
			this.chkMaintainAspectRatio.Name = "chkMaintainAspectRatio";
			this.chkMaintainAspectRatio.Size = new System.Drawing.Size(248, 24);
			this.chkMaintainAspectRatio.TabIndex = 9;
			this.chkMaintainAspectRatio.Text = "Maintain aspect ratio";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(48, 116);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(44, 20);
			this.label2.TabIndex = 5;
			this.label2.Text = "Width:";
			// 
			// nudWidth
			// 
			this.nudWidth.Location = new System.Drawing.Point(100, 116);
			this.nudWidth.Name = "nudWidth";
			this.nudWidth.Size = new System.Drawing.Size(100, 20);
			this.nudWidth.TabIndex = 8;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(48, 144);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(44, 20);
			this.label3.TabIndex = 5;
			this.label3.Text = "Height:";
			// 
			// nudHeight
			// 
			this.nudHeight.Location = new System.Drawing.Point(100, 144);
			this.nudHeight.Name = "nudHeight";
			this.nudHeight.Size = new System.Drawing.Size(100, 20);
			this.nudHeight.TabIndex = 8;
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label4.Location = new System.Drawing.Point(204, 116);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(44, 20);
			this.label4.TabIndex = 5;
			this.label4.Text = "(pixel)";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label5.Location = new System.Drawing.Point(204, 144);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(44, 20);
			this.label5.TabIndex = 5;
			this.label5.Text = "(pixel)";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// DlgTranslate
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(286, 208);
			this.Controls.Add(this.chkMaintainAspectRatio);
			this.Controls.Add(this.numericUpDown1);
			this.Controls.Add(this.byPercentage);
			this.Controls.Add(this.cbResampling);
			this.Controls.Add(this.lblResampling);
			this.Controls.Add(this.grpParameters);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.bySize);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.nudWidth);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.nudHeight);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label5);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DlgTranslate";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Translate";
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudWidth)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudHeight)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region public properties
		#endregion
	
		#region event handlers

		#endregion

		#region override routines

		#region DialogBase override
		
		protected override object OnGetDefaultValue(System.Windows.Forms.Control ctrl)
		{
			return null;
		}

		#endregion

		#endregion
    }
}
