using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Resources;
using System.Reflection;

namespace SiGlaz.ObjectAnalysis.UI
{
	/// <summary>
	/// Summary description for UserControl1.
	/// </summary>
	public class OptionsSelectionControl : System.Windows.Forms.UserControl
	{
        #region Members
        public int type = 1;  
        public ArrayList alData = new ArrayList();
        //public ArrayList alDataOperation = new ArrayList();
        public ArrayList alTampleData = new ArrayList();
        public DataTable	dtCondition=new DataTable("Condition");

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnEdit;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.Button btnRemoveAll;
		public System.Windows.Forms.DataGrid dgCondition;
		public System.Windows.Forms.DataGridTableStyle dataGridTableStyle1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
        private Button btnChangeCondition;
        private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn1;
        #endregion
       
        #region Contructor / Dispose
		public OptionsSelectionControl()
		{
			InitializeComponent();
			InitData();
//			HelpClass.Init();
//			HelpClass.InitializeHelp("KLARF_FILE_PATH",ref HelpClass.helpprovider1,this);
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnRemoveAll = new System.Windows.Forms.Button();
            this.dgCondition = new System.Windows.Forms.DataGrid();
            this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnChangeCondition = new System.Windows.Forms.Button();
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
            // panel2
            // 
            this.panel2.Controls.Add(this.btnChangeCondition);
            this.panel2.Controls.Add(this.btnAdd);
            this.panel2.Controls.Add(this.btnEdit);
            this.panel2.Controls.Add(this.btnRemove);
            this.panel2.Controls.Add(this.btnRemoveAll);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(432, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(88, 240);
            this.panel2.TabIndex = 2;
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
            this.btnRemove.Location = new System.Drawing.Point(8, 76);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(72, 24);
            this.btnRemove.TabIndex = 4;
            this.btnRemove.Text = "Remove";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnRemoveAll
            // 
            this.btnRemoveAll.Enabled = false;
            this.btnRemoveAll.Location = new System.Drawing.Point(8, 108);
            this.btnRemoveAll.Name = "btnRemoveAll";
            this.btnRemoveAll.Size = new System.Drawing.Size(72, 24);
            this.btnRemoveAll.TabIndex = 5;
            this.btnRemoveAll.Text = "Remove All";
            this.btnRemoveAll.Click += new System.EventHandler(this.btnRemoveAll_Click);
            // 
            // dgCondition
            // 
            this.dgCondition.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgCondition.CaptionText = "OR Condition";
            this.dgCondition.DataMember = "";
            this.dgCondition.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dgCondition.Location = new System.Drawing.Point(0, 0);
            this.dgCondition.Name = "dgCondition";
            this.dgCondition.PreferredColumnWidth = 300;
            this.dgCondition.ReadOnly = true;
            this.dgCondition.Size = new System.Drawing.Size(432, 240);
            this.dgCondition.TabIndex = 1;
            this.dgCondition.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
            this.dataGridTableStyle1});
            this.dgCondition.Enter += new System.EventHandler(this.dgCondition_Enter);
            this.dgCondition.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgCondition_KeyPress);
            this.dgCondition.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgCondition_KeyDown);
            this.dgCondition.Click += new System.EventHandler(this.dgCondition_Click);
            // 
            // dataGridTableStyle1
            // 
            this.dataGridTableStyle1.DataGrid = this.dgCondition;
            this.dataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGridTableStyle1.MappingName = "Condition";
            this.dataGridTableStyle1.ReadOnly = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.dgCondition);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(520, 240);
            this.panel1.TabIndex = 0;
            // 
            // btnChangeCondition
            // 
            this.btnChangeCondition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChangeCondition.Location = new System.Drawing.Point(8, 138);
            this.btnChangeCondition.Name = "btnChangeCondition";
            this.btnChangeCondition.Size = new System.Drawing.Size(72, 23);
            this.btnChangeCondition.TabIndex = 28;
            this.btnChangeCondition.Text = "Condition";
            this.btnChangeCondition.Click += new System.EventHandler(this.btnChangeCondition_Click);
            // 
            // OptionsSelectionControl
            // 
            this.Controls.Add(this.panel1);
            this.Name = "OptionsSelectionControl";
            this.Size = new System.Drawing.Size(520, 240);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgCondition)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#region UpdateData
		public void UpdateData( bool update ,ArrayList data,ArrayList dataOperation)
		{
			DataRow row;
			if ( !update )
			{
				alData = data;
				//alDataOperation = dataOperation;
				for( int i=0;i<alData.Count;i++)
				{
					row = dtCondition.NewRow();
					row[0] = alData[i].ToString();
					dtCondition.Rows.Add( row );
				}

				for( int j=0;j<dataOperation.Count;j++)
				{
					alTampleData.Add( dataOperation[j]);
				}
			}
			else
			{
				alData.Clear();					
				foreach( DataRow _row in  dtCondition.Rows )
				{					
					alData.Add( _row[0].ToString() );									
				}
			}

		}		

		#endregion
		//Add New
		#region GUI
		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			OptionsSelectionHeaderField dlg=new OptionsSelectionHeaderField(type);
			dlg.bOrAnd=bOrAnd;
			if( dlg.ShowDialog(this) == DialogResult.OK)
			{
				dlg.UpdateDatatable();
				DataRow dr=dtCondition.NewRow();
				dr[0]=dlg.Condition;
				dtCondition.Rows.Add(dr);

				//operation				
				//alDataOperation.Add(dlg.ConditionOpreration);
				dgCondition_Click(null,EventArgs.Empty);
			}
		}
		
		private void btnEdit_Click(object sender, System.EventArgs e)
		{
			if( dgCondition.CurrentRowIndex <0 || dgCondition.CurrentRowIndex>= dtCondition.Rows.Count)
				return;
			string ssss=dtCondition.Rows[dgCondition.CurrentRowIndex][0].ToString();
			OptionsSelectionHeaderField dlg=new OptionsSelectionHeaderField(type);
			dlg.bOrAnd=bOrAnd;
			//dlg.Condition=ssss;
			try
			{
				dlg.ConditionOpreration = ssss;//dgCondition.row .r.CurrentRowIndex].ToString();
			}
			catch{};
			if( dlg.ShowDialog(this) == DialogResult.OK)
			{
				dlg.UpdateDatatable();
				DataRow dr=dtCondition.Rows[dgCondition.CurrentRowIndex];
				dr[0]=dlg.Condition;

				//operation
				//alDataOperation[dgCondition.CurrentRowIndex]= dlg.ConditionOpreration;							
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
				//alDataOperation.RemoveAt(rowIndex);
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
			//alDataOperation.Clear();
			dgCondition_Click(null,EventArgs.Empty);
		}

		private void dgCondition_Click(object sender, System.EventArgs e)
		{
			btnRemoveAll.Enabled=dtCondition.Rows.Count>0;
			btnEdit.Enabled=btnRemove.Enabled= ( dgCondition.CurrentRowIndex >=0 && dgCondition.CurrentRowIndex< dtCondition.Rows.Count);
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

		private void dgCondition_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			
		}


		#region Properties
		private bool bOrAnd=true;

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

		public	string ConditionExpression
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
							sResult+= " | " + dr[0].ToString();
						else
							sResult+= " & " + dr[0].ToString();
					}
				}

				sResult=sResult.Replace(" OR "," | ");
				sResult=sResult.Replace(" AND "," & ");

				return sResult;
			}
			set
			{
				dtCondition.Rows.Clear();
				if(value==null ||  value==string.Empty)
					return;
				string ss=value;

				char csep=UseOrAnd?'|':'&';
				string []ls=ss.Split(csep);
				DataRow row;
				foreach(string sss in ls)
				{
					ss=sss.Trim();
					ss=ss.Replace(" | "," OR ");
					ss=ss.Replace(" & "," AND ");
					row = dtCondition.NewRow();
					row[0] = ss;
					dtCondition.Rows.Add( row );
				}
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
		#endregion		

        private void btnChangeCondition_Click(object sender, EventArgs e)
        {
            FilterOption dlg = new FilterOption();
            dlg.UseOrAnd = UseOrAnd;
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                if (UseOrAnd != dlg.UseOrAnd)
                {
                    UseOrAnd = dlg.UseOrAnd;
                }
            }
        }

	}
}
