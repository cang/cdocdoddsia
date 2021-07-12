using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using SIA.Common;
using SIA.SystemLayer;

using SIA.UI.Controls;
using SIA.UI.Controls.Utilities;
using SIA.UI.Controls.Commands;

using SIA.UI.Components;
using SIA.UI.Components.Helpers;

using SIA.UI.Controls.Automation.Commands;

namespace SIA.UI.Controls.Dialogs
{
	/// <summary>
	/// Summary description for DlgFlipRotate.
	/// </summary>
	public class DlgFlipRotate : DialogBase
	{
		#region Windows Form Members

		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnLoadSettings;
		private System.Windows.Forms.Button btnSaveSettings;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.RadioButton flipVert;
		private System.Windows.Forms.RadioButton flipHorz;
		private System.Windows.Forms.RadioButton rot90CW;
		private System.Windows.Forms.RadioButton rot90CCW;
		private System.Windows.Forms.RadioButton rot180;
		private System.Windows.Forms.RadioButton rotByAngle;
		private System.Windows.Forms.NumericUpDown nudAngle;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		
		#endregion

		#region Member fields

		private FlipRotateImageCommandSettings _settings = new FlipRotateImageCommandSettings();

		#endregion

		#region Properties

		public FlipRotateImageCommandSettings Settings
		{
			get {return _settings;}
		}

		#endregion

		#region Constructor and destructor

		public DlgFlipRotate()
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgFlipRotate));
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnLoadSettings = new System.Windows.Forms.Button();
			this.btnSaveSettings = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.flipVert = new System.Windows.Forms.RadioButton();
			this.flipHorz = new System.Windows.Forms.RadioButton();
			this.rot90CW = new System.Windows.Forms.RadioButton();
			this.rot90CCW = new System.Windows.Forms.RadioButton();
			this.rot180 = new System.Windows.Forms.RadioButton();
			this.rotByAngle = new System.Windows.Forms.RadioButton();
			this.nudAngle = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudAngle)).BeginInit();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(332, 204);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(72, 24);
			this.btnCancel.TabIndex = 13;
			this.btnCancel.Text = "Cancel";
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(252, 204);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(72, 24);
			this.btnOK.TabIndex = 12;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(-44, 192);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(498, 4);
			this.groupBox1.TabIndex = 20;
			this.groupBox1.TabStop = false;
			// 
			// btnLoadSettings
			// 
			this.btnLoadSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnLoadSettings.Location = new System.Drawing.Point(4, 204);
			this.btnLoadSettings.Name = "btnLoadSettings";
			this.btnLoadSettings.Size = new System.Drawing.Size(92, 24);
			this.btnLoadSettings.TabIndex = 14;
			this.btnLoadSettings.Text = "Load Settings";
			this.btnLoadSettings.Visible = false;
			this.btnLoadSettings.Click += new System.EventHandler(this.btnLoadSettings_Click);
			// 
			// btnSaveSettings
			// 
			this.btnSaveSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnSaveSettings.Location = new System.Drawing.Point(104, 204);
			this.btnSaveSettings.Name = "btnSaveSettings";
			this.btnSaveSettings.Size = new System.Drawing.Size(92, 24);
			this.btnSaveSettings.TabIndex = 15;
			this.btnSaveSettings.Text = "Save Settings";
			this.btnSaveSettings.Visible = false;
			this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Location = new System.Drawing.Point(80, 12);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(336, 4);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.label1);
			this.groupBox3.Location = new System.Drawing.Point(80, 80);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(336, 4);
			this.groupBox3.TabIndex = 5;
			this.groupBox3.TabStop = false;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(192, 16);
			this.label1.Name = "label1";
			this.label1.TabIndex = 0;
			this.label1.Text = "label1";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(4, 4);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(68, 16);
			this.label2.TabIndex = 0;
			this.label2.Text = "Flip image";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(4, 72);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(84, 16);
			this.label3.TabIndex = 4;
			this.label3.Text = "Rotate image";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// flipVert
			// 
			this.flipVert.Location = new System.Drawing.Point(16, 24);
			this.flipVert.Name = "flipVert";
			this.flipVert.Size = new System.Drawing.Size(104, 20);
			this.flipVert.TabIndex = 2;
			this.flipVert.Text = "Flip Vertical";
			// 
			// flipHorz
			// 
			this.flipHorz.Location = new System.Drawing.Point(16, 48);
			this.flipHorz.Name = "flipHorz";
			this.flipHorz.Size = new System.Drawing.Size(104, 20);
			this.flipHorz.TabIndex = 3;
			this.flipHorz.Text = "Flip Horizontal";
			// 
			// rot90CW
			// 
			this.rot90CW.Location = new System.Drawing.Point(16, 92);
			this.rot90CW.Name = "rot90CW";
			this.rot90CW.Size = new System.Drawing.Size(176, 20);
			this.rot90CW.TabIndex = 6;
			this.rot90CW.Text = "Rotate 90 Degree Clockwise";
			// 
			// rot90CCW
			// 
			this.rot90CCW.Location = new System.Drawing.Point(16, 116);
			this.rot90CCW.Name = "rot90CCW";
			this.rot90CCW.Size = new System.Drawing.Size(212, 20);
			this.rot90CCW.TabIndex = 7;
			this.rot90CCW.Text = "Rotate 90 Degree Counter-Clockwise";
			// 
			// rot180
			// 
			this.rot180.Location = new System.Drawing.Point(16, 140);
			this.rot180.Name = "rot180";
			this.rot180.Size = new System.Drawing.Size(208, 20);
			this.rot180.TabIndex = 8;
			this.rot180.Text = "Rotate 180 Degree";
			// 
			// rotByAngle
			// 
			this.rotByAngle.Location = new System.Drawing.Point(16, 164);
			this.rotByAngle.Name = "rotByAngle";
			this.rotByAngle.Size = new System.Drawing.Size(68, 20);
			this.rotByAngle.TabIndex = 9;
			this.rotByAngle.Text = "Rotate";
			// 
			// nudAngle
			// 
			this.nudAngle.DecimalPlaces = 2;
			this.nudAngle.Increment = new System.Decimal(new int[] {
																	   1,
																	   0,
																	   0,
																	   65536});
			this.nudAngle.Location = new System.Drawing.Point(92, 164);
			this.nudAngle.Maximum = new System.Decimal(new int[] {
																	 360,
																	 0,
																	 0,
																	 0});
			this.nudAngle.Minimum = new System.Decimal(new int[] {
																	 360,
																	 0,
																	 0,
																	 -2147483648});
			this.nudAngle.Name = "nudAngle";
			this.nudAngle.Size = new System.Drawing.Size(140, 20);
			this.nudAngle.TabIndex = 10;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(236, 164);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(100, 20);
			this.label4.TabIndex = 11;
			this.label4.Text = "(Degree)";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// DlgFlipRotate
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(410, 232);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.nudAngle);
			this.Controls.Add(this.flipVert);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btnLoadSettings);
			this.Controls.Add(this.btnSaveSettings);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.flipHorz);
			this.Controls.Add(this.rot90CW);
			this.Controls.Add(this.rot90CCW);
			this.Controls.Add(this.rot180);
			this.Controls.Add(this.rotByAngle);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DlgFlipRotate";
			this.Text = "Flip/Rotate image";
			this.groupBox3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.nudAngle)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region Override routines

		protected override void OnLoad(EventArgs e)
		{
			// refresh ui elements
			this.UpdateData(false);

			// restore last settings
			base.OnLoad (e);
		}

		protected override object OnGetDefaultValue(Control ctrl)
		{
			if (ctrl == nudAngle)
				return 0;
			else if (ctrl == this.flipVert)
				return true;
			else if (ctrl == this.flipHorz)
				return false;
			else if (ctrl == this.rot90CW)
				return false;
			else if (ctrl == this.rot90CCW)
				return false;
			else if (ctrl == this.rot180)
				return false;
			else if (ctrl == this.rotByAngle)
				return false;
			return base.OnGetDefaultValue (ctrl);
		}

		#endregion

		#region Event Handlers

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			// update settings
			this.UpdateData(true);
		}

		private void btnLoadSettings_Click(object sender, System.EventArgs e)
        {
            // load settings from file
			FlipRotateImageCommandSettings settings = this.LoadCommandSettings(_settings.GetType()) as FlipRotateImageCommandSettings;
			
            // load settings from file
			if (settings != null)
			{
                // update current settings
                this._settings = settings;

				// update ui elements
				this.UpdateData(false);
			}
		}

		private void btnSaveSettings_Click(object sender, System.EventArgs e)
		{
			// update settings fields
			this.UpdateData(true);

			// save settings to file
			this.SaveCommandSettings(this._settings);
		}

		#endregion

		#region internal helpers

		private void UpdateData(bool saveAndValidate)
		{
			if (saveAndValidate)
			{
				FlipRotateImageCommandSettings.Actions type = FlipRotateImageCommandSettings.Actions.FlipHorizontal;
				float angle = 0;

				if (flipVert.Checked)
					type = FlipRotateImageCommandSettings.Actions.FlipVertical;
				else if (flipHorz.Checked)
					type = FlipRotateImageCommandSettings.Actions.FlipHorizontal;
				else if (rot90CW.Checked)
					type = FlipRotateImageCommandSettings.Actions.Rotate90CW;
				else if (rot90CCW.Checked)
					type = FlipRotateImageCommandSettings.Actions.Rotate90CCW;
				else if (rot180.Checked)
					type = FlipRotateImageCommandSettings.Actions.Rotate180;
				else if (rotByAngle.Checked)
				{
					type = FlipRotateImageCommandSettings.Actions.RotateByAngle;
					angle = Convert.ToSingle(nudAngle.Value);
				}

				_settings.ActionType = (int)type;
				_settings.RotateAngle = angle;
			}
			else
			{
				FlipRotateImageCommandSettings.Actions type = (FlipRotateImageCommandSettings.Actions)_settings.ActionType;
				float angle = 0;

				if (type == FlipRotateImageCommandSettings.Actions.FlipHorizontal)
					flipVert.Checked = true;
				else if (type == FlipRotateImageCommandSettings.Actions.FlipVertical)
					flipHorz.Checked = true;
				else if (type == FlipRotateImageCommandSettings.Actions.Rotate90CW)
					rot90CW.Checked = true;
				else if (type == FlipRotateImageCommandSettings.Actions.Rotate90CCW)
					rot90CCW.Checked = true;
				else if (type == FlipRotateImageCommandSettings.Actions.Rotate180)
					rot180.Checked = true;
				else if (type == FlipRotateImageCommandSettings.Actions.RotateByAngle)
				{
					rotByAngle.Checked = true;
					angle = Convert.ToSingle(nudAngle.Value);
				}
			}
		}
		
		#endregion
		
	}
}
