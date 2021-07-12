using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using SIA.UI.Controls.Utilities;
using SIA.UI.Controls.Helpers.VisualAnalysis.DataProfilers;

namespace SIA.UI.Controls.Dialogs
{
	/// <summary>
	/// Name : DlgLineProfileSettings
	/// Description : User interface for Line Profile Settings
	/// Thread Support : None
	/// Persistence Data : False
	/// </summary>
	public class DlgLineProfileSettings : DialogBase
	{
		#region Windows Form member attributes

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		public System.Windows.Forms.NumericUpDown nuMin;
		public System.Windows.Forms.NumericUpDown nuMax;
		public System.Windows.Forms.CheckBox chkAuto;
		public System.Windows.Forms.CheckBox chkLorarithmic;
		private System.Windows.Forms.Button button_Cancel;
		private System.Windows.Forms.Button button_OK;
		private System.Windows.Forms.GroupBox grpSettings;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.Label label26;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private LineProfileSettings _settings = null;

		#endregion

		#region Properties

		public LineProfileSettings Settings
		{
			get {return this._settings;}
		}

		#endregion
		
		#region constructor and destructor
		
		public DlgLineProfileSettings()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		public DlgLineProfileSettings(LineProfileSettings settings)
			: this()
		{
			// initialize items
			this._settings = settings.Clone() as LineProfileSettings;

			// update ui elements
			this.UpdateData(false);
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgLineProfileSettings));
			this.grpSettings = new System.Windows.Forms.GroupBox();
			this.chkLorarithmic = new System.Windows.Forms.CheckBox();
			this.nuMax = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.nuMin = new System.Windows.Forms.NumericUpDown();
			this.chkAuto = new System.Windows.Forms.CheckBox();
			this.button_Cancel = new System.Windows.Forms.Button();
			this.button_OK = new System.Windows.Forms.Button();
			this.label25 = new System.Windows.Forms.Label();
			this.label26 = new System.Windows.Forms.Label();
			this.grpSettings.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nuMax)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nuMin)).BeginInit();
			this.SuspendLayout();
			// 
			// grpSettings
			// 
			this.grpSettings.Controls.Add(this.label25);
			this.grpSettings.Controls.Add(this.label26);
			this.grpSettings.Controls.Add(this.chkLorarithmic);
			this.grpSettings.Controls.Add(this.nuMax);
			this.grpSettings.Controls.Add(this.label2);
			this.grpSettings.Controls.Add(this.label1);
			this.grpSettings.Controls.Add(this.nuMin);
			this.grpSettings.Controls.Add(this.chkAuto);
			this.grpSettings.Location = new System.Drawing.Point(4, 4);
			this.grpSettings.Name = "grpSettings";
			this.grpSettings.Size = new System.Drawing.Size(184, 128);
			this.grpSettings.TabIndex = 0;
			this.grpSettings.TabStop = false;
			this.grpSettings.Text = "Intensity axis settings";
			// 
			// chkLorarithmic
			// 
			this.chkLorarithmic.Location = new System.Drawing.Point(8, 104);
			this.chkLorarithmic.Name = "chkLorarithmic";
			this.chkLorarithmic.Size = new System.Drawing.Size(110, 16);
			this.chkLorarithmic.TabIndex = 2;
			this.chkLorarithmic.Text = " Logarithmic plot ";
			this.chkLorarithmic.CheckedChanged += new System.EventHandler(this.chkLorarithmic_CheckedChanged);
			// 
			// nuMax
			// 
			this.nuMax.Enabled = false;
			this.nuMax.Location = new System.Drawing.Point(40, 52);
			this.nuMax.Maximum = new System.Decimal(new int[] {
																  1215752192,
																  23,
																  0,
																  0});
			this.nuMax.Name = "nuMax";
			this.nuMax.Size = new System.Drawing.Size(76, 20);
			this.nuMax.TabIndex = 0;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 52);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(32, 20);
			this.label2.TabIndex = 2;
			this.label2.Text = "Max:";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(32, 20);
			this.label1.TabIndex = 1;
			this.label1.Text = "Min:";
			// 
			// nuMin
			// 
			this.nuMin.Enabled = false;
			this.nuMin.Location = new System.Drawing.Point(40, 24);
			this.nuMin.Maximum = new System.Decimal(new int[] {
																  1215752192,
																  23,
																  0,
																  0});
			this.nuMin.Name = "nuMin";
			this.nuMin.Size = new System.Drawing.Size(76, 20);
			this.nuMin.TabIndex = 1;
			// 
			// chkAuto
			// 
			this.chkAuto.Checked = true;
			this.chkAuto.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAuto.Location = new System.Drawing.Point(8, 80);
			this.chkAuto.Name = "chkAuto";
			this.chkAuto.Size = new System.Drawing.Size(88, 16);
			this.chkAuto.TabIndex = 1;
			this.chkAuto.Text = "Auto scale";
			this.chkAuto.CheckedChanged += new System.EventHandler(this.chkAuto_CheckedChanged);
			// 
			// button_Cancel
			// 
			this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button_Cancel.Location = new System.Drawing.Point(195, 37);
			this.button_Cancel.Name = "button_Cancel";
			this.button_Cancel.TabIndex = 2;
			this.button_Cancel.Text = "Cancel";
			// 
			// button_OK
			// 
			this.button_OK.Location = new System.Drawing.Point(195, 9);
			this.button_OK.Name = "button_OK";
			this.button_OK.TabIndex = 1;
			this.button_OK.Text = "OK";
			this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
			// 
			// label25
			// 
			this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label25.Location = new System.Drawing.Point(120, 24);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(56, 20);
			this.label25.TabIndex = 6;
			this.label25.Text = "(Intensity)";
			this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label26
			// 
			this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label26.Location = new System.Drawing.Point(120, 52);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(56, 20);
			this.label26.TabIndex = 5;
			this.label26.Text = "(Intensity)";
			this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// DlgLineProfileSettings
			// 
			this.AcceptButton = this.button_OK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.button_Cancel;
			this.ClientSize = new System.Drawing.Size(278, 137);
			this.Controls.Add(this.button_Cancel);
			this.Controls.Add(this.button_OK);
			this.Controls.Add(this.grpSettings);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DlgLineProfileSettings";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Profile Settings";
			this.grpSettings.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.nuMax)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nuMin)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region event handlers

		private void chkAuto_CheckedChanged(object sender, System.EventArgs e)
		{
			nuMin.Enabled=nuMax.Enabled=!chkAuto.Checked;
		}

		private void chkLorarithmic_CheckedChanged(object sender, System.EventArgs e)
		{

		}

		private void button_OK_Click(object sender, System.EventArgs e)
		{
			if ( !kUtils.IsAllValueValidate ( grpSettings )) 
				return;
			
			if(!chkAuto.Checked)
			{
				if( nuMin.Value >=nuMax.Value - 5)
				{
					MessageBoxEx.Error("Invalid data range.");
					return;
				}
			}

			// update and validate data 
			this.UpdateData(true);
			
			DialogResult = DialogResult.OK;
			Close();
		}

		#endregion

		#region Internal Helpers

		private void UpdateData(bool saveAndValidate)
		{
			if (this._settings == null)
				return;

			if (saveAndValidate)
			{
				this._settings.AutoScale = chkAuto.Checked;
				this._settings.LogarithmicPlot = chkLorarithmic.Checked;
				this._settings.Mininum = Convert.ToSingle(nuMin.Value);
				this._settings.Maximum = Convert.ToSingle(nuMax.Value);
			}
			else
			{
				chkAuto.Checked = this._settings.AutoScale;
				chkLorarithmic.Checked = this._settings.LogarithmicPlot;
				nuMin.Value = Convert.ToDecimal(this._settings.Mininum);
				nuMax.Value = Convert.ToDecimal(this._settings.Maximum);
			}
		}

		#endregion
	}
}
