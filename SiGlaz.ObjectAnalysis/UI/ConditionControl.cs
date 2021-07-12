using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using SiGlaz.ObjectAnalysis.Common;
//using SSA.SystemFrameworks;
//using System.Reflection;

//using FPPCommon;
//using FPPCommon.Objects.Rules;

namespace SiGlaz.ObjectAnalysis.UI
{
	/// <summary>
	/// Summary description for UserControl1.
	/// </summary>
	public class ConditionControl : System.Windows.Forms.UserControl
	{
        #region Members
        //public int type = 1;  
        //public ArrayList alData = new ArrayList();
        public ArrayList alDataOperation = new ArrayList();
        public DataTable	dtCondition=new DataTable("Condition");
		private string [] m_varList = null;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Panel pTop;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnEdit;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.Button btnRemoveAll;
		private System.Windows.Forms.Panel panel3;
		public System.Windows.Forms.DataGrid dgCondition;
		public System.Windows.Forms.DataGridTableStyle dataGridTableStyle1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button btnCondition;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn1;

		private FilterCondition _filterCondition = null;

        #endregion
       
        #region Contructor / Dispose
		public ConditionControl()
		{
			InitializeComponent();
			InitData();
		}
		public ConditionControl(string[] _varList):this()
		{
			m_varList = _varList;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
				if (m_varList != null)
					m_varList = null;
			}
			base.Dispose( disposing );
		}
		

		private void InitData()
		{
			DataColumn dlColumn=new DataColumn("Condition");			
			//dlColumn.Caption="OR Condition";
			dtCondition.Columns.Add(dlColumn);

			DataGridTextBoxColumn coltextbox=new DataGridTextBoxColumn();
			coltextbox.Width=450;
			//coltextbox.HeaderText = "OR Condition";
			coltextbox.MappingName="Condition";			
			dataGridTableStyle1.GridColumnStyles.Add(coltextbox);

			dgCondition.DataSource=dtCondition;			

			dgCondition_Click(null,EventArgs.Empty);
		}


        #endregion

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.dataGridTextBoxColumn1 = new System.Windows.Forms.DataGridTextBoxColumn();
			this.pTop = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.btnCondition = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnEdit = new System.Windows.Forms.Button();
			this.btnRemove = new System.Windows.Forms.Button();
			this.btnRemoveAll = new System.Windows.Forms.Button();
			this.dgCondition = new System.Windows.Forms.DataGrid();
			this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
			this.panel1 = new System.Windows.Forms.Panel();
			this.pTop.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgCondition)).BeginInit();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// dataGridTextBoxColumn1
			// 
			this.dataGridTextBoxColumn1.Format = "";
			this.dataGridTextBoxColumn1.FormatInfo = null;
			this.dataGridTextBoxColumn1.MappingName = "Condition";
			this.dataGridTextBoxColumn1.Width = 2000;
			// 
			// pTop
			// 
			this.pTop.Controls.Add(this.panel3);
			this.pTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.pTop.Location = new System.Drawing.Point(0, 0);
			this.pTop.Name = "pTop";
			this.pTop.Size = new System.Drawing.Size(656, 248);
			this.pTop.TabIndex = 4;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.panel2);
			this.panel3.Controls.Add(this.dgCondition);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point(0, 0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(656, 248);
			this.panel3.TabIndex = 10;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.btnCondition);
			this.panel2.Controls.Add(this.btnAdd);
			this.panel2.Controls.Add(this.btnEdit);
			this.panel2.Controls.Add(this.btnRemove);
			this.panel2.Controls.Add(this.btnRemoveAll);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel2.Location = new System.Drawing.Point(568, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(88, 248);
			this.panel2.TabIndex = 2;
			// 
			// btnCondition
			// 
			this.btnCondition.Location = new System.Drawing.Point(8, 144);
			this.btnCondition.Name = "btnCondition";
			this.btnCondition.TabIndex = 6;
			this.btnCondition.Text = "Condition";
			this.btnCondition.Click += new System.EventHandler(this.btnCondition_Click);
			// 
			// btnAdd
			// 
			this.btnAdd.Location = new System.Drawing.Point(8, 12);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(72, 24);
			this.btnAdd.TabIndex = 2;
			this.btnAdd.Text = "Add";
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnEdit
			// 
			this.btnEdit.Enabled = false;
			this.btnEdit.Location = new System.Drawing.Point(8, 44);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.Size = new System.Drawing.Size(72, 24);
			this.btnEdit.TabIndex = 3;
			this.btnEdit.Text = "Edit";
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			// 
			// btnRemove
			// 
			this.btnRemove.Enabled = false;
			this.btnRemove.Location = new System.Drawing.Point(8, 80);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.Size = new System.Drawing.Size(72, 24);
			this.btnRemove.TabIndex = 4;
			this.btnRemove.Text = "Remove";
			this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
			// 
			// btnRemoveAll
			// 
			this.btnRemoveAll.Enabled = false;
			this.btnRemoveAll.Location = new System.Drawing.Point(8, 112);
			this.btnRemoveAll.Name = "btnRemoveAll";
			this.btnRemoveAll.Size = new System.Drawing.Size(72, 24);
			this.btnRemoveAll.TabIndex = 5;
			this.btnRemoveAll.Text = "Remove All";
			this.btnRemoveAll.Click += new System.EventHandler(this.btnRemoveAll_Click);
			// 
			// dgCondition
			// 
			this.dgCondition.CaptionText = "OR Condition";
			this.dgCondition.DataMember = "";
			this.dgCondition.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgCondition.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dgCondition.Location = new System.Drawing.Point(0, 0);
			this.dgCondition.Name = "dgCondition";
			this.dgCondition.PreferredColumnWidth = 300;
			this.dgCondition.ReadOnly = true;
			this.dgCondition.Size = new System.Drawing.Size(656, 248);
			this.dgCondition.TabIndex = 1;
			this.dgCondition.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
																									this.dataGridTableStyle1});
			this.dgCondition.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgCondition_KeyDown);
			this.dgCondition.Click += new System.EventHandler(this.dgCondition_Click);
			this.dgCondition.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgCondition_KeyPress);
			this.dgCondition.Enter += new System.EventHandler(this.dgCondition_Enter);
			// 
			// dataGridTableStyle1
			// 
			this.dataGridTableStyle1.ColumnHeadersVisible = false;
			this.dataGridTableStyle1.DataGrid = this.dgCondition;
			this.dataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGridTableStyle1.MappingName = "Condition";
			this.dataGridTableStyle1.ReadOnly = true;
			this.dataGridTableStyle1.RowHeadersVisible = false;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.pTop);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(656, 240);
			this.panel1.TabIndex = 0;
			// 
			// ConditionControl
			// 
			this.Controls.Add(this.panel1);
			this.Name = "ConditionControl";
			this.Size = new System.Drawing.Size(656, 240);
			this.Leave += new System.EventHandler(this.ConditionControl_Leave);
			this.pTop.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgCondition)).EndInit();
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region UpdateData
		private void ClearControl()
		{
			if (dtCondition == null)
				return;
			dtCondition.Clear();
		}
		private void UpdateData2Ctrl()
		{
			ClearControl();
            if (_filterCondition == null)
            {
                return;
            }
            UseOrAnd = _filterCondition.IsOrAnd;
            DataRow row;
            alDataOperation = _filterCondition.ArrayDataCondition;
            for (int i = 0; i < alDataOperation.Count; i++)
            {
                row = dtCondition.NewRow();
                row[0] = alDataOperation[i].ToString();
                dtCondition.Rows.Add(row);
            }
		}		
		private void UpdateDataFromCtrl()
		{
            if (_filterCondition == null)
                return;
            alDataOperation.Clear();
            foreach (DataRow _row in dtCondition.Rows)
            {
                alDataOperation.Add(_row[0].ToString());
            }
            _filterCondition.StringConditionOperation = ConditionString;
		}		

		#endregion
		//Add New
		#region GUI
		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			SubConditionDlg dlg=new SubConditionDlg(m_varList);
			dlg.bOrAnd=bOrAnd;
			if( dlg.ShowDialog(this) == DialogResult.OK)
			{
				DataRow dr=dtCondition.NewRow();
				dr[0]=dlg.Condition;
				dtCondition.Rows.Add(dr);

				//operation				
				alDataOperation.Add(dlg.ConditionOpreration);
				dgCondition_Click(null,EventArgs.Empty);
			}
            //if (_rule != null)
            //    _rule.Modified = true;
		}
		
		private void btnEdit_Click(object sender, System.EventArgs e)
		{
			if( dgCondition.CurrentRowIndex <0 || dgCondition.CurrentRowIndex>= dtCondition.Rows.Count)
				return;
			string ssss=dtCondition.Rows[dgCondition.CurrentRowIndex][0].ToString();
			SubConditionDlg dlg=new SubConditionDlg(m_varList);
			dlg.bOrAnd=bOrAnd;
			//dlg.Condition=ssss;
			string oldStr = alDataOperation[dgCondition.CurrentRowIndex].ToString();
			try
			{
				dlg.Condition = oldStr;
			}
			catch{};
			string newStr = null;
			if( dlg.ShowDialog(this) == DialogResult.OK)
			{
				DataRow dr=dtCondition.Rows[dgCondition.CurrentRowIndex];
				dr[0]=dlg.Condition;

				//operation
				newStr = dlg.Condition;
			}
			if (oldStr != newStr)
			{
				alDataOperation[dgCondition.CurrentRowIndex]= newStr;
                //if (_rule != null)
                //    _rule.Modified = true;
			}

		}

		public void RemoveData()
		{
			try
			{
				if( dgCondition.CurrentRowIndex <0 || dgCondition.CurrentRowIndex>= dtCondition.Rows.Count)
					return;
				int rowIndex = dgCondition.CurrentRowIndex;
				dtCondition.Rows.RemoveAt(dgCondition.CurrentRowIndex);

				//operation
				alDataOperation.RemoveAt(rowIndex);
				dgCondition_Click(null,EventArgs.Empty);
			}
			catch {};
		}
		private void btnRemove_Click(object sender, System.EventArgs e)
		{
			RemoveData();
		}

		private void btnRemoveAll_Click(object sender, System.EventArgs e)
		{
			dtCondition.Rows.Clear();
			//operation
			alDataOperation.Clear();
			dgCondition_Click(null,EventArgs.Empty);
		}

		private void dgCondition_Click(object sender, System.EventArgs e)
		{
			btnRemoveAll.Enabled=dtCondition.Rows.Count>0;
			btnEdit.Enabled=btnRemove.Enabled= ( dgCondition.CurrentRowIndex >=0 && dgCondition.CurrentRowIndex< dtCondition.Rows.Count);
            //if (_rule != null)
            //    _rule.Modified = true;
		}
		
		private void dgCondition_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{			
			dgCondition_Click(null,EventArgs.Empty);			
		}

		private void dgCondition_Enter(object sender, System.EventArgs e)
		{
			btnRemoveAll.Enabled=dtCondition.Rows.Count>0;
			btnEdit.Enabled=btnRemove.Enabled= ( dgCondition.CurrentRowIndex >=0 && dgCondition.CurrentRowIndex< dtCondition.Rows.Count);
		}
		#endregion

		#region Properties
		private bool bOrAnd=true;

		private void btnCondition_Click(object sender, System.EventArgs e)
		{
			FilterOption dlg=new  FilterOption();
			dlg.UseOrAnd=UseOrAnd;
			if( dlg.ShowDialog(this)==DialogResult.OK )
			{
				if( UseOrAnd != dlg.UseOrAnd )
				{
					UseOrAnd = dlg.UseOrAnd;
				}
			}

		}

		private void dgCondition_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
		
		}

		private void ConditionControl_Leave(object sender, System.EventArgs e)
		{
			UpdateDataFromCtrl();
		}

		public	string ConditionString
		{
			get
			{
				string	sResult="";
				foreach(DataRow dr in dtCondition.Rows)
				{
					if( dr[0] is DBNull)
					{
						return null;
					}
					if(sResult=="")
						sResult= dr[0].ToString();
					else
					{
						if( bOrAnd )
							sResult+= " OR " + dr[0].ToString();
						else
							sResult+= " AND " + dr[0].ToString();
					}
				}
				return sResult;
			}
		}

		public	string ConditionStringOperation
		{
			get
			{
				string	sResult="";
				for (int i=0 ;i<alDataOperation.Count;i++ )
				{
					if( alDataOperation[i] == null)
					{
						return null;
					}
					if(sResult=="")
						sResult= alDataOperation[i].ToString();
					else
					{
						if( bOrAnd )
							sResult+= " | " + alDataOperation[i].ToString();
						else
							sResult+= " & " + alDataOperation[i].ToString();
					}
				}
				return sResult;
			}
		}

		public bool UseOrAnd
		{
			get
			{
				return bOrAnd;
			}
			set
			{
				if(bOrAnd!=value)
				{
					btnRemoveAll_Click(null,null);
				}
				bOrAnd=value;
				if(bOrAnd)
					dgCondition.CaptionText="OR Condition";
				else
					dgCondition.CaptionText="AND Condition";
			}
		}

        public FilterCondition FilterCondition
        {
            get
            {
                return _filterCondition;
            }
            set
            {
                _filterCondition = value;
                UpdateData2Ctrl();
            }
        }
		#endregion		

	}
}
