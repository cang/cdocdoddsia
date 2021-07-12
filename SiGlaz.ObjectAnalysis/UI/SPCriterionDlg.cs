using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using SiGlaz.ObjectAnalysis.Common;

namespace SiGlaz.ObjectAnalysis.UI
{
	/// <summary>
	/// Summary description for SPCriterionDlg.
	/// </summary>
	public class SPCriterionDlg : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		public System.Windows.Forms.ComboBox cboCondition;
		private System.Windows.Forms.NumericUpDown nudValue;
		private System.Windows.Forms.Label lblUnit;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public SPCriterionDlg()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			InitData();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		public MDCCParam.CONDITION con;
        public bool _isLimitedCombinationCompareOperator = false;
		public SPCriterionDlg(MDCCParam.CONDITION con) :
            this(con, false)
		{			
		}

        public SPCriterionDlg(
            MDCCParam.CONDITION con, bool isLimitedCombinationCompareOperator)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            this.con = con;
            _isLimitedCombinationCompareOperator = isLimitedCombinationCompareOperator;

            lblUnit.Text = MDCCParam._humankeylist[(int)con.LHS, 1];
            InitData();

            for (int i = 0; i < cboCondition.Items.Count; i++)
                if (((MyItem)cboCondition.Items[i]).con == con.Operator)
                    cboCondition.SelectedIndex = i;

            if (cboCondition.Items.Count > 0 &&
                cboCondition.SelectedIndex < 0)
            {
                cboCondition.SelectedIndex = cboCondition.Items.Count-1;
            }

            nudValue.Minimum = 0;
            nudValue.Maximum = 1000000000000;
            nudValue.Value = Convert.ToDecimal(con.RHS);
        }

		public class MyItem
		{
			public COMPARE_OPERATOR con;
			public MyItem(COMPARE_OPERATOR con)
			{
				this.con=con;
			}
			public override string ToString()
			{
				return QueryOperator.HumanString(con);
			}
		}

		public void InitData()
		{
			cboCondition.Items.Clear();

			COMPARE_OPERATOR []le=(COMPARE_OPERATOR [])Enum.GetValues(typeof(COMPARE_OPERATOR));
			foreach(COMPARE_OPERATOR eo in le)
			{
				if( eo==COMPARE_OPERATOR.NONE) continue;

                if (_isLimitedCombinationCompareOperator)
                {
                    switch (eo)
                    {
                        case COMPARE_OPERATOR.GREATER:
                        case COMPARE_OPERATOR.GREATEREQUAL:
                        case COMPARE_OPERATOR.NONE:
                        case COMPARE_OPERATOR.NOTEQUAL:
                            continue;
                        default:
                            break;
                    }
                }

				MyItem item=new MyItem(eo);
				cboCondition.Items.Add(item);
			}
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
            this.label3 = new System.Windows.Forms.Label();
            this.cboCondition = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.nudValue = new System.Windows.Forms.NumericUpDown();
            this.lblUnit = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudValue)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(168, 120);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "Cancel";
            // 
            // groupBox4
            // 
            this.groupBox4.Location = new System.Drawing.Point(-152, 104);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(656, 8);
            this.groupBox4.TabIndex = 16;
            this.groupBox4.TabStop = false;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(88, 120);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 14;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "&Condition:";
            // 
            // cboCondition
            // 
            this.cboCondition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCondition.Location = new System.Drawing.Point(16, 26);
            this.cboCondition.Name = "cboCondition";
            this.cboCondition.Size = new System.Drawing.Size(232, 21);
            this.cboCondition.TabIndex = 18;
            this.cboCondition.Tag = "DEFAULT";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "&Value:";
            // 
            // nudValue
            // 
            this.nudValue.DecimalPlaces = 3;
            this.nudValue.Location = new System.Drawing.Point(16, 72);
            this.nudValue.Name = "nudValue";
            this.nudValue.Size = new System.Drawing.Size(152, 20);
            this.nudValue.TabIndex = 20;
            // 
            // lblUnit
            // 
            this.lblUnit.AutoSize = true;
            this.lblUnit.Location = new System.Drawing.Point(172, 76);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new System.Drawing.Size(0, 13);
            this.lblUnit.TabIndex = 21;
            // 
            // SPCriterionDlg
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(330, 152);
            this.Controls.Add(this.lblUnit);
            this.Controls.Add(this.nudValue);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboCondition);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SPCriterionDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SPCriterionDlg";
            ((System.ComponentModel.ISupportInitialize)(this.nudValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion


		private void btnOK_Click(object sender, System.EventArgs e)
		{
			if( cboCondition.SelectedIndex<0) 
			{
				MessageBox.Show("Please select condition",Text,MessageBoxButtons.OK,MessageBoxIcon.Error);
				return;
			}

			con.Operator=((MyItem)cboCondition.SelectedItem).con;
			con.RHS=Convert.ToSingle(nudValue.Value);

			DialogResult=DialogResult.OK;

			Close();
		
		}
	}
}
