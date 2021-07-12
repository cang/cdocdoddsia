using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using SAC = SiGlaz.Algorithms.Core;
using SiGlaz.Common;

namespace SiGlaz.ObjectAnalysis.UI
{
	/// <summary>
	/// Summary description for SPCriterionDlg.
	/// </summary>
	public class SPValueDlg : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.NumericUpDown nudValue;
		private System.Windows.Forms.Label lblUnit;
		private System.Windows.Forms.Label lblDescription;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public SPValueDlg()
		{
			InitializeComponent();
		}

		public MDCCParam.CONDITION con;
		public COMPARE_OPERATOR cprDefault;

		public SPValueDlg(MDCCParam.CONDITION con,COMPARE_OPERATOR cprDefault)
		{
			InitializeComponent();

			this.con=con;
			this.cprDefault=cprDefault;

			lblUnit.Text = MDCCParam._humankeylist[(int)con.LHS,1];
			lblDescription.Text = MDCCParam._humankeylist[(int)con.LHS,0] + QueryOperator.HumanString(cprDefault);

			nudValue.Minimum=0;
			nudValue.Maximum=1000000000000;
			nudValue.Value=Convert.ToDecimal(con.RHS);
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnCancel = new System.Windows.Forms.Button();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.lblDescription = new System.Windows.Forms.Label();
			this.nudValue = new System.Windows.Forms.NumericUpDown();
			this.lblUnit = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.nudValue)).BeginInit();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(150, 104);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 15;
			this.btnCancel.Text = "Cancel";
			// 
			// groupBox4
			// 
			this.groupBox4.Location = new System.Drawing.Point(-164, 88);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(656, 8);
			this.groupBox4.TabIndex = 16;
			this.groupBox4.TabStop = false;
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(70, 104);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 14;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// lblDescription
			// 
			this.lblDescription.Location = new System.Drawing.Point(8, 8);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Size = new System.Drawing.Size(284, 32);
			this.lblDescription.TabIndex = 19;
			this.lblDescription.Text = "&Value:";
			// 
			// nudValue
			// 
			this.nudValue.DecimalPlaces = 3;
			this.nudValue.Location = new System.Drawing.Point(8, 48);
			this.nudValue.Name = "nudValue";
			this.nudValue.Size = new System.Drawing.Size(152, 20);
			this.nudValue.TabIndex = 20;
			// 
			// lblUnit
			// 
			this.lblUnit.AutoSize = true;
			this.lblUnit.Location = new System.Drawing.Point(172, 50);
			this.lblUnit.Name = "lblUnit";
			this.lblUnit.Size = new System.Drawing.Size(0, 16);
			this.lblUnit.TabIndex = 21;
			// 
			// SPValueDlg
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(294, 140);
			this.Controls.Add(this.lblUnit);
			this.Controls.Add(this.nudValue);
			this.Controls.Add(this.lblDescription);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.btnOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SPValueDlg";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Set Value";
			((System.ComponentModel.ISupportInitialize)(this.nudValue)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion


		private void btnOK_Click(object sender, System.EventArgs e)
		{
			con.RHS=Convert.ToSingle(nudValue.Value);
			con.Operator=cprDefault;

			DialogResult=DialogResult.OK;
			Close();
		}
	}
}
