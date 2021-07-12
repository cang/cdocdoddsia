using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using SIA.UI.Controls.Utilities;

namespace SIA.UI.Controls.Dialogs
{
	/// <summary>
	/// Name : DlgCoordinateSettings
	/// Description : User interface for Physical Coordinate Settings operation
	/// Thread Support : False
	/// Persistence Data : True
	/// </summary>
	public class DlgCoordinateSettings : SIA.UI.Controls.Dialogs.DialogBase
	{
		#region Windows Form members
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown nudPhysicalAngle;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown nudWaferDiameter;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion

		#region public properties

		public float PhysicalAngle
		{
			get { return (float)nudPhysicalAngle.Value; } 
			set 
			{
				value = (float)Math.Max(nudPhysicalAngle.Minimum, Math.Min(nudPhysicalAngle.Maximum, (Decimal)value));
				nudPhysicalAngle.Value = (Decimal)value;
				OnPhysicalAngleChanged();
			}
		}

		public float WaferDiameter
		{
			get {return (float)nudWaferDiameter.Value;}
			set 
			{
				value = (float)Math.Max(nudWaferDiameter.Minimum, Math.Min(nudWaferDiameter.Maximum, (Decimal)value));
				nudWaferDiameter.Value = (Decimal)value;
				OnWaferDiameterChanged();
			}
		}

		
		#endregion

		#region constructor and destructor
		public DlgCoordinateSettings()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgCoordinateSettings));
			this.nudPhysicalAngle = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this.nudWaferDiameter = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.nudPhysicalAngle)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudWaferDiameter)).BeginInit();
			this.SuspendLayout();
			// 
			// nudPhysicalAngle
			// 
			this.nudPhysicalAngle.Location = new System.Drawing.Point(164, 36);
			this.nudPhysicalAngle.Maximum = new System.Decimal(new int[] {
											     360,
											     0,
											     0,
											     0});
			this.nudPhysicalAngle.Name = "nudPhysicalAngle";
			this.nudPhysicalAngle.Size = new System.Drawing.Size(64, 20);
			this.nudPhysicalAngle.TabIndex = 5;
			this.nudPhysicalAngle.ValueChanged += new System.EventHandler(this.ndCooedinate_ValueChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(4, 36);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(156, 20);
			this.label1.TabIndex = 1;
			this.label1.Text = "Physical Angle Orientation:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(64, 76);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 2;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(152, 76);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox1.Location = new System.Drawing.Point(-12, 68);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(315, 4);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(4, 12);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(156, 20);
			this.label2.TabIndex = 1;
			this.label2.Text = "Real Wafer Diameter:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// nudWaferDiameter
			// 
			this.nudWaferDiameter.DecimalPlaces = 1;
			this.nudWaferDiameter.Increment = new System.Decimal(new int[] {
											       1,
											       0,
											       0,
											       65536});
			this.nudWaferDiameter.Location = new System.Drawing.Point(164, 12);
			this.nudWaferDiameter.Maximum = new System.Decimal(new int[] {
											     1000,
											     0,
											     0,
											     0});
			this.nudWaferDiameter.Name = "nudWaferDiameter";
			this.nudWaferDiameter.Size = new System.Drawing.Size(64, 20);
			this.nudWaferDiameter.TabIndex = 0;
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(232, 12);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(31, 20);
			this.label3.TabIndex = 1;
			this.label3.Text = "mm";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label4.Location = new System.Drawing.Point(232, 36);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(56, 20);
			this.label4.TabIndex = 1;
			this.label4.Text = "degree(s)";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// DlgCoordinateSettings
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(290, 104);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.nudPhysicalAngle);
			this.Controls.Add(this.nudWaferDiameter);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DlgCoordinateSettings";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Coordinate Settings";
			((System.ComponentModel.ISupportInitialize)(this.nudPhysicalAngle)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudWaferDiameter)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region event handler
		private void ndCooedinate_ValueChanged(object sender, System.EventArgs e)
		{
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			if ( !kUtils.IsInputValueValidate( nudPhysicalAngle )) return;
			DialogResult = DialogResult.OK; 
		}
		#endregion

		#region internal helpers
		
		protected virtual void OnPhysicalAngleChanged()
		{

		}

		protected virtual void OnWaferDiameterChanged()
		{
		}

		#endregion

		#region DialogBase override

		protected override object OnGetDefaultValue(Control ctrl)
		{
			return null;
		}

		#endregion

		
	}
}
