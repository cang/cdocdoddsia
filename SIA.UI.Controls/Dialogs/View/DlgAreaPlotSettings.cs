using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using SIA.UI.Controls.UserControls;
using SIA.UI.Controls.Helpers;
using SIA.UI.Controls.Helpers.VisualAnalysis;
using SIA.UI.Controls.Helpers.VisualAnalysis.DataProfilers;

namespace SIA.UI.Controls.Dialogs
{
	/// <summary>
	/// Name : DlgAreaPlotSettings
	/// Description : User interface for Area Plot Settings
	/// Thread Support : None
	/// Persistence Data : False
	/// </summary>
	public class DlgAreaPlotSettings : DialogBase
	{
		#region constants
		
		#endregion

		#region Windows Form member attributes
		private System.Windows.Forms.GroupBox groupBox1;
		public System.Windows.Forms.RadioButton radioSolid;
		public System.Windows.Forms.RadioButton radioWireFrame;
		private System.Windows.Forms.GroupBox groupBox2;
		public System.Windows.Forms.NumericUpDown numericUpDownRelX;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		public System.Windows.Forms.NumericUpDown numericUpDownRelY;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.CheckBox chMaintain;
		private System.Windows.Forms.GroupBox groupBox3;
		public System.Windows.Forms.CheckBox chAutoScale;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		public System.Windows.Forms.NumericUpDown ndMax;
		public System.Windows.Forms.NumericUpDown ndMin;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		
		#region member attributes
	
		private AreaPlotSettings _settings = null;
		
		#endregion

		#region public properties

		public AreaPlotSettings Settings
		{
			get {return _settings;}
		}

		#endregion

		#region constructor and destructor
		
		public DlgAreaPlotSettings()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		public DlgAreaPlotSettings(AreaPlotSettings settings)
			: this()
		{
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			this._settings = settings.Clone() as AreaPlotSettings;
			
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgAreaPlotSettings));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radioWireFrame = new System.Windows.Forms.RadioButton();
			this.radioSolid = new System.Windows.Forms.RadioButton();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.chMaintain = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.numericUpDownRelY = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.numericUpDownRelX = new System.Windows.Forms.NumericUpDown();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label6 = new System.Windows.Forms.Label();
			this.ndMax = new System.Windows.Forms.NumericUpDown();
			this.ndMin = new System.Windows.Forms.NumericUpDown();
			this.chAutoScale = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownRelY)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownRelX)).BeginInit();
			this.groupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ndMax)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ndMin)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radioWireFrame);
			this.groupBox1.Controls.Add(this.radioSolid);
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(176, 44);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Rendering";
			// 
			// radioWireFrame
			// 
			this.radioWireFrame.Location = new System.Drawing.Point(80, 16);
			this.radioWireFrame.Name = "radioWireFrame";
			this.radioWireFrame.Size = new System.Drawing.Size(88, 24);
			this.radioWireFrame.TabIndex = 1;
			this.radioWireFrame.Text = "Wire Frame";
			// 
			// radioSolid
			// 
			this.radioSolid.Location = new System.Drawing.Point(12, 16);
			this.radioSolid.Name = "radioSolid";
			this.radioSolid.Size = new System.Drawing.Size(48, 24);
			this.radioSolid.TabIndex = 0;
			this.radioSolid.Text = "Solid";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.chMaintain);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.numericUpDownRelY);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.numericUpDownRelX);
			this.groupBox2.Location = new System.Drawing.Point(8, 56);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(176, 100);
			this.groupBox2.TabIndex = 2;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Resolution";
			// 
			// chMaintain
			// 
			this.chMaintain.Checked = true;
			this.chMaintain.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chMaintain.Location = new System.Drawing.Point(12, 16);
			this.chMaintain.Name = "chMaintain";
			this.chMaintain.TabIndex = 2;
			this.chMaintain.Text = "Maintain aspect";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label4.Location = new System.Drawing.Point(116, 72);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(48, 20);
			this.label4.TabIndex = 5;
			this.label4.Text = "(pixels)";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(116, 44);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(48, 20);
			this.label3.TabIndex = 4;
			this.label3.Text = "(pixels)";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(12, 75);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(15, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "Y:";
			// 
			// numericUpDownRelY
			// 
			this.numericUpDownRelY.Location = new System.Drawing.Point(40, 72);
			this.numericUpDownRelY.Minimum = new System.Decimal(new int[] {
																			  1,
																			  0,
																			  0,
																			  0});
			this.numericUpDownRelY.Name = "numericUpDownRelY";
			this.numericUpDownRelY.Size = new System.Drawing.Size(72, 20);
			this.numericUpDownRelY.TabIndex = 4;
			this.numericUpDownRelY.Value = new System.Decimal(new int[] {
																			1,
																			0,
																			0,
																			0});
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(12, 47);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(15, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "X:";
			// 
			// numericUpDownRelX
			// 
			this.numericUpDownRelX.Location = new System.Drawing.Point(40, 44);
			this.numericUpDownRelX.Minimum = new System.Decimal(new int[] {
																			  1,
																			  0,
																			  0,
																			  0});
			this.numericUpDownRelX.Name = "numericUpDownRelX";
			this.numericUpDownRelX.Size = new System.Drawing.Size(72, 20);
			this.numericUpDownRelX.TabIndex = 3;
			this.numericUpDownRelX.Value = new System.Decimal(new int[] {
																			1,
																			0,
																			0,
																			0});
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(196, 16);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 8;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.button1_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(196, 52);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 9;
			this.btnCancel.Text = "Cancel";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.label6);
			this.groupBox3.Controls.Add(this.ndMax);
			this.groupBox3.Controls.Add(this.ndMin);
			this.groupBox3.Controls.Add(this.chAutoScale);
			this.groupBox3.Controls.Add(this.label5);
			this.groupBox3.Controls.Add(this.label7);
			this.groupBox3.Controls.Add(this.label8);
			this.groupBox3.Location = new System.Drawing.Point(8, 160);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(176, 100);
			this.groupBox3.TabIndex = 7;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Intensity axis settings";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(12, 75);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(29, 16);
			this.label6.TabIndex = 9;
			this.label6.Text = "Max:";
			// 
			// ndMax
			// 
			this.ndMax.Location = new System.Drawing.Point(40, 72);
			this.ndMax.Maximum = new System.Decimal(new int[] {
																  65536,
																  0,
																  0,
																  0});
			this.ndMax.Name = "ndMax";
			this.ndMax.Size = new System.Drawing.Size(72, 20);
			this.ndMax.TabIndex = 7;
			// 
			// ndMin
			// 
			this.ndMin.Location = new System.Drawing.Point(40, 44);
			this.ndMin.Maximum = new System.Decimal(new int[] {
																  65536,
																  0,
																  0,
																  0});
			this.ndMin.Name = "ndMin";
			this.ndMin.Size = new System.Drawing.Size(72, 20);
			this.ndMin.TabIndex = 6;
			// 
			// chAutoScale
			// 
			this.chAutoScale.Checked = true;
			this.chAutoScale.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chAutoScale.Location = new System.Drawing.Point(12, 16);
			this.chAutoScale.Name = "chAutoScale";
			this.chAutoScale.Size = new System.Drawing.Size(92, 24);
			this.chAutoScale.TabIndex = 5;
			this.chAutoScale.Text = "Auto scale";
			this.chAutoScale.CheckedChanged += new System.EventHandler(this.chAutoScale_CheckedChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(12, 47);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(26, 16);
			this.label5.TabIndex = 8;
			this.label5.Text = "Min:";
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label7.Location = new System.Drawing.Point(112, 44);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(56, 20);
			this.label7.TabIndex = 5;
			this.label7.Text = "(Intensity)";
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label8.Location = new System.Drawing.Point(112, 72);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(56, 20);
			this.label8.TabIndex = 5;
			this.label8.Text = "(Intensity)";
			// 
			// DlgAreaPlotSettings
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(278, 268);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupBox2);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DlgAreaPlotSettings";
			this.Text = "Profile Settings";
			this.Load += new System.EventHandler(this.AreaPlotSetting_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownRelY)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownRelX)).EndInit();
			this.groupBox3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.ndMax)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ndMin)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region override routines

		protected override object OnGetDefaultValue(Control ctrl)
		{
			if (ctrl == chMaintain)
				return true;
			return null;
		}
		
		#endregion

		#region virtual routines


		#endregion

		#region event handlers

		private void button1_Click(object sender, System.EventArgs e)
		{
			this.UpdateData(true);
		}

		private void AreaPlotSetting_Load(object sender, System.EventArgs e)
		{
			this.numericUpDownRelX.ValueChanged += new System.EventHandler(OnXY_ValueChanged);
			this.numericUpDownRelY.ValueChanged += new System.EventHandler(OnXY_ValueChanged);
			this.numericUpDownRelX.KeyUp += new System.Windows.Forms.KeyEventHandler(OnXY_KeyUp);
			this.numericUpDownRelY.KeyUp += new System.Windows.Forms.KeyEventHandler(OnXY_KeyUp);			
			EnableMinMax();
		}

		private void OnXY_ValueChanged(object sender, System.EventArgs e)
		{
			if ( !chMaintain.Checked ) return;
			try
			{
				if( (NumericUpDown)sender == numericUpDownRelX )
					numericUpDownRelY.Value	 =  numericUpDownRelX .Value;
				else	numericUpDownRelX .Value =  numericUpDownRelY.Value ;
			}
			catch{};
		}

		private void OnXY_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if ( !chMaintain.Checked ) return;
			try
			{
				if( (NumericUpDown)sender == numericUpDownRelX )
					numericUpDownRelY.Value	 =  numericUpDownRelX .Value;
				else	numericUpDownRelX .Value =  numericUpDownRelY.Value ;
			}
			catch{};
		}

		private void chAutoScale_CheckedChanged(object sender, System.EventArgs e)
		{
			EnableMinMax();
		}

		#endregion

		#region public routines

		public void EnableMinMax()
		{
			ndMin.Enabled = ndMax.Enabled = !chAutoScale.Checked;
		}

		#endregion

		#region internal routines

		private void UpdateData(bool saveAndValidate)
		{
			if (saveAndValidate)
			{
				_settings.AutoScale = chAutoScale.Checked;
				_settings.Mininum = Convert.ToSingle(ndMin.Value);
				_settings.Maximum = Convert.ToSingle(ndMax.Value);

				if (radioSolid.Checked)
					_settings.RenderMode = RenderMode.Solid;
				else
					_settings.RenderMode = RenderMode.Wireframe;

				_settings.XRes = Convert.ToSingle(numericUpDownRelX.Value);
				_settings.YRes = Convert.ToSingle(numericUpDownRelY.Value);
			}
			else
			{
				chAutoScale.Checked = _settings.AutoScale;
				ndMin.Value = Convert.ToDecimal(_settings.Mininum);
				ndMax.Value = Convert.ToDecimal(_settings.Maximum);
				radioSolid.Checked = _settings.RenderMode == RenderMode.Solid;
				radioWireFrame.Checked = _settings.RenderMode == RenderMode.Wireframe;
				numericUpDownRelX.Value = Convert.ToDecimal(_settings.XRes);
				numericUpDownRelY.Value = Convert.ToDecimal(_settings.YRes);
			}
		}

		#endregion

		
	}
}
