using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using SiGlaz.ObjectAnalysis.Engine;
using SiGlaz.ObjectAnalysis.Common;
using SiGlaz.Common.Object;

namespace SiGlaz.ObjectAnalysis.UI
{
	/// <summary>
	/// Summary description for OptionsSelectionHeaderField.
	/// </summary>
	public class SubConditionDlg : System.Windows.Forms.Form
	{
		#region Members
		private System.Windows.Forms.DataGrid dataGrid1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.DataGridTableStyle dataGridTableStyle1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public	bool bOrAnd=true;
		#endregion		

		#region construtor/destructor
		public SubConditionDlg()
		{
			InitializeComponent();
			InitData();
		}
		public SubConditionDlg(string[] _varList)
		{
			InitializeComponent();
			if (_varList == null)
				InitData(); // default
			else
				InitData(_varList);
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SubConditionDlg));
			this.dataGrid1 = new System.Windows.Forms.DataGrid();
			this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
			this.SuspendLayout();
			// 
			// dataGrid1
			// 
			this.dataGrid1.CaptionVisible = false;
			this.dataGrid1.DataMember = "";
			this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGrid1.Location = new System.Drawing.Point(8, 8);
			this.dataGrid1.Name = "dataGrid1";
			this.dataGrid1.Size = new System.Drawing.Size(568, 296);
			this.dataGrid1.TabIndex = 0;
			this.dataGrid1.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
																								  this.dataGridTableStyle1});
			this.dataGrid1.CurrentCellChanged += new System.EventHandler(this.dataGrid1_CurrentCellChanged);
			this.dataGrid1.Enter += new System.EventHandler(this.dataGrid1_Enter);
			// 
			// dataGridTableStyle1
			// 
			this.dataGridTableStyle1.DataGrid = this.dataGrid1;
			this.dataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGridTableStyle1.MappingName = "Condition";
			// 
			// groupBox1
			// 
			this.groupBox1.Location = new System.Drawing.Point(-24, 304);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(624, 8);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(244, 328);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(140, 328);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 4;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// SubConditionDlg
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(584, 360);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.dataGrid1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SubConditionDlg";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "AND Condition";
			this.Load += new System.EventHandler(this.OptionsSelectionHeaderField_Load);
			((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion
		
		#region Properties
		public string ConditionOpreration
		{
			get
			{
				string sOperation;
				string	sResult="";
				foreach(DataRow dr in dtCondition.Rows)
				{
					if( dr[0] is DBNull || dr[1]is DBNull || dr[2]is DBNull )
					{
						return null;
					}
					sOperation  = dr[1].ToString().ToUpper();
					sOperation = sOperation == "LIKE" ? "~" : sOperation == "NOT LIKE" ? "!~" : sOperation;

					sOperation = sOperation == "<>" ? "!=" :  sOperation;
					
					if(sResult=="")
					{
						
						sResult= dr[0].ToString() + " " + sOperation+ " " + dr[2].ToString();
					}
					else
					{
						if(bOrAnd )
							sResult+= " & " + dr[0].ToString() + " " + sOperation + " " + dr[2].ToString();
						else
							sResult+= " | " + dr[0].ToString() + " " + sOperation + " " + dr[2].ToString();
					}

					//Vu~ fix mem
					sOperation=null;
				}
				return sResult;
			}
			set
			{
				string query=value.ToString() + " ";;//.ToUpper().Replace("AND"," & ");
				
				string []sAnd;
				if(bOrAnd)
					sAnd=query .Split("&".ToCharArray());
				else
					sAnd=query .Split("|".ToCharArray());

				foreach(string sExp in sAnd)
				{  
					string fieldname="";
					string operation="";
					object obj=null;					
					StringConditionParse.GetExpressOfEndQuery(sExp,ref fieldname,ref operation,ref obj);
					DataRow dr=dtCondition.NewRow();
					dr[0]=Convert.ToString(fieldname.Trim());
					dr[1]=Convert.ToString(operation.Trim());
					dr[2]=Convert.ToString(obj);
					dtCondition.Rows.Add(dr);
					
				}

				//Vu~ fix mem
				query=null;
				for(int i=0;i>sAnd.Length;i++)
					sAnd[i]=null;
				sAnd=null;
			}
		}
		public	string	Condition
		{
			get
			{
				string	sResult="";
				foreach(DataRow dr in dtCondition.Rows)
				{
					if( dr[0] is DBNull || dr[1]is DBNull || dr[2]is DBNull )
					{
						return null;
					}
					if(sResult=="")
						sResult= dr[0].ToString() + " " + dr[1].ToString() + " " + dr[2].ToString();
					else
					{
						if(bOrAnd )
							sResult+= " AND " + dr[0].ToString() + " " + dr[1].ToString() + " " + dr[2].ToString();
						else
							sResult+= " OR " + dr[0].ToString() + " " + dr[1].ToString() + " " + dr[2].ToString();
					}
				}
				return sResult;
			}
			set
			{
				string query = null;
				string []sAnd;
				if( bOrAnd)
				{
					query=value.ToString().ToUpper().Replace(" AND ","&");
					sAnd=query.Trim().Split("&".ToCharArray());
				}
				else
				{
					query=value.ToString().ToUpper().Replace(" OR ","|");
					sAnd=query.Trim().Split("|".ToCharArray());
				}

				foreach(string sExp in sAnd)
				{  
					string fieldname="";
					string operation="";
					object obj=null;
					StringConditionParse.GetExpressOfEndQuery(sExp.Trim(),ref fieldname,ref operation,ref obj);
					DataRow dr=dtCondition.NewRow();
					dr[0]=Convert.ToString(fieldname.Trim());
					dr[1]=Convert.ToString(operation.Trim());
					dr[2]=Convert.ToString(obj);
					dtCondition.Rows.Add(dr);
				}

				//Vu~ fix mem
				query=null;
				for(int i=0;i<sAnd.Length;i++)
					sAnd[i]=null;

				sAnd=null;
			}
		}

		#endregion

		#region function InitData
		DataTable dtCondition=new DataTable("Condition");
		private void InitData()
		{
            InitData(EllipticalDensityShapeObject.VaribleList);
		}
		private void InitData(string[] _varList)
		{
			dataGridTableStyle1.GridColumnStyles.Clear();

			DataGriComboColumn colTemplate=new DataGriComboColumn();
			colTemplate.myCombo.DropDownStyle = ComboBoxStyle.DropDownList;
			DataTable dtField=new DataTable();
			dtField.Columns.Add(new DataColumn("FieldName"));

			DataRow dr=null;
			for (int i=0; i< _varList.Length;i++)
			{
				dr=dtField.NewRow();
				dr[0]= _varList[i];
				dtField.Rows.Add(dr);
			}

           

			colTemplate.Setdata(dtField,"FieldName");
			colTemplate.Width=280;
			colTemplate.MappingName="Defect Properties";
			colTemplate.HeaderText="Selected Field";
			dataGridTableStyle1.GridColumnStyles.Add(colTemplate);

			dtField=new DataTable();
			colTemplate=new DataGriComboColumn();
			colTemplate.myCombo.DropDownStyle = ComboBoxStyle.DropDownList;
			dtField=new DataTable();
			dtField.Columns.Add(new DataColumn("Criteria"));
			
			string []sss={"LIKE","=",">",">=","<="};
			for(int i = 0; i < sss.Length; i++)
			{
				dr=dtField.NewRow();
				dr[0]=sss[i];
				dtField.Rows.Add(dr);
			}
			colTemplate.Setdata(dtField,"Criteria");
			colTemplate.Width=80;
			colTemplate.MappingName="Criteria";
			colTemplate.HeaderText="Criteria";
			dataGridTableStyle1.GridColumnStyles.Add(colTemplate);

            DataGridTextBoxColumn coltextbox=new DataGridTextBoxColumn();
			coltextbox.Width=180;
			coltextbox.MappingName="Value";
			coltextbox.HeaderText="Value";
			dataGridTableStyle1.GridColumnStyles.Add(coltextbox);

			dataGridTableStyle1.AllowSorting = false;

			DataColumn dcColumn=new DataColumn("Defect Properties");
			
			dtCondition.Columns.Add(dcColumn);

			dcColumn=new DataColumn("Criteria");			
			dtCondition.Columns.Add(dcColumn);

			dcColumn=new DataColumn("Value",Type.GetType("System.String"));			
			dtCondition.Columns.Add(dcColumn);

			dataGrid1.DataSource=dtCondition;
		}
		#endregion

        #region Methods
		private bool Validation( ref string errmess)
		{
			int  index=-1;
			foreach ( DataRow row in dtCondition.Rows )
			{
				index++;
				try
				{
					if( row[0] is DBNull || row[1]is DBNull || row[2]is DBNull )
					{
						dataGrid1.Select(index);
						errmess = " Value cannot be 'null'";
						return false;
					}

					if ( ((string)row[2]).IndexOf("=")>=0 ||
						((string)row[2]).IndexOf(">=")>=0 ||	
						((string)row[2]).IndexOf("<=")>=0 ||
						((string)row[2]).IndexOf(">")>=0 ||
						((string)row[2]).IndexOf("<")>=0 ||
						((string)row[2]).IndexOf("~")>=0 
						)
					{
						dataGrid1.Select(index);
						errmess = " Value cannot contain: =,>,<,>=,<=,~.";
						return false;
					}

				}
				catch 
				{ 
					dataGrid1.Select(index);
					return false;
				}
			}
			return true;
		}

        #endregion

        #region GUI Events
        private void OptionsSelectionHeaderField_Load(object sender, System.EventArgs e)
        {
            if(  bOrAnd)
                this.Text = "AND Condition";
            else
                this.Text = "OR Condition";
        }
		
		private void btnOK_Click(object sender, System.EventArgs e)
		{
			string errMess = "";
			if( !Validation( ref errMess) || Condition==null || Condition=="")
			{
				MessageBox.Show("Input value is invalid!\n" + errMess,Application.ProductName);
			}
			else
			{
				DialogResult=DialogResult.OK;
				Close();
			}
		}

		private void ResetDataGridTableStyle(ArrayList alCriteria)
		{
			DataTable dtField=new DataTable();
			dtField.Columns.Add(new DataColumn("FieldName"));			
			dtField=new DataTable();			
			dtField=new DataTable();
			dtField.Columns.Add(new DataColumn("Criteria"));
			
			for(int i = 0; i < alCriteria.Count; i++)
			{
				DataRow dr=dtField.NewRow();
				dr[0]=alCriteria[i];
				dtField.Rows.Add(dr);
			}
			((DataGriComboColumn)dataGridTableStyle1.GridColumnStyles[1]).Setdata(dtField,"Criteria");
	
			
			dataGridTableStyle1.GridColumnStyles.RemoveAt(2);			

			DataGridTextBoxColumn coltextbox=new DataGridTextBoxColumn();
			coltextbox.Width=180;
			coltextbox.MappingName="Value";
			coltextbox.HeaderText="Value";
				
			dataGridTableStyle1.GridColumnStyles.Add(coltextbox);
		}
		
		private void dataGrid1_CurrentCellChanged(object sender, System.EventArgs e)
		{
			int colIdex = dataGrid1.CurrentCell.ColumnNumber;					

			ArrayList al = new ArrayList();
		
			if (colIdex  == 0 || colIdex  == 1 || colIdex == 2)
			{	
				al.Add("=");
				al.Add("<>");
				al.Add(">");					
				al.Add("<");					
				al.Add(">=");
				al.Add("<=");
//				if ( al.Count <=0 ) 
//				{
//					al.Add("LIKE");												
//					al.Add("=");						
//				}
				ResetDataGridTableStyle(al);
			}
		}

		private void dataGrid1_Enter(object sender, System.EventArgs e)
		{

		}
		
		#endregion

        #region internal class
        internal class DataGriComboColumn : DataGridColumnStyle
        {
            public DataGridComboBox myCombo = new DataGridComboBox();



            // The isEditing field tracks whether or not the user is
            // editing data with the hosted control.
            private bool isEditing;

            public ComboBox ComboBox
            {
                get
                {
                    return myCombo;
                }
            }

            public DataGriComboColumn()
                : base()
            {
                myCombo.Visible = false;

                myCombo.Validating += new System.ComponentModel.CancelEventHandler(this.myCombo_Validating);
            }

            public void Setdata(DataTable mDataTable, string DisplayMembe)
            {
                myCombo.DataSource = mDataTable;
                myCombo.DisplayMember = DisplayMembe;
                myCombo.ValueMember = DisplayMembe;
            }

            protected override void Abort(int rowNum)
            {
                isEditing = false;
                myCombo.Validated -= new EventHandler(ComboBoxValueChanged);
                myCombo.Hide();
                Invalidate();
            }

            protected override bool Commit
                (CurrencyManager dataSource, int rowNum)
            {
                myCombo.Bounds = Rectangle.Empty;
                myCombo.Hide();
                myCombo.Validated -= new EventHandler(ComboBoxValueChanged);
                if (!isEditing)
                    return true;
                isEditing = false;

                try
                {
                    string value = myCombo.Text;
                    SetColumnValueAtRow(dataSource, rowNum, value);
                }
                catch (Exception)
                {
                    Abort(rowNum);
                    return false;
                }

                Invalidate();
                return true;
            }

            protected override void Edit(
                CurrencyManager source,
                int rowNum,
                Rectangle bounds,
                bool readOnly,
                string instantText,
                bool cellIsVisible)
            {

                foreach (DataGridColumnStyle col in DataGridTableStyle.GridColumnStyles)
                {
                    if (col.GetType() == typeof(DataGriComboColumn))
                        ((DataGriComboColumn)col).myCombo.Visible = false;
                }

                string value = GetColumnValueAtRow(source, rowNum).ToString();

                if (cellIsVisible)
                {
                    myCombo.Bounds = bounds;//new Rectangle	(bounds.X + 2, bounds.Y + 2,bounds.Width - 4, bounds.Height - 4);
                    myCombo.Text = value;
                    myCombo.Visible = true;
                    myCombo.Validated += new EventHandler(ComboBoxValueChanged);
                }
                else
                {
                    myCombo.Text = value;
                    myCombo.Visible = false;
                }

                if (myCombo.Visible)
                    DataGridTableStyle.DataGrid.Invalidate(bounds);
            }

            void myCombo_Validating(object sender, System.ComponentModel.CancelEventArgs e)
            {
                isEditing = true;
                base.ColumnStartedEditing(myCombo);
            }


            //		protected override Size GetPreferredSize(
            //			Graphics g, 
            //			object value) 
            //		{
            //			return new Size(100, myCombo.PreferredHeight + 4);
            //		}

            protected override Size GetPreferredSize(Graphics g, object val)
            {
                // Return the preferred width.
                // Iterate through all display texts in the dropdown, and measure each
                // text width.
                int widest = 0;
                SizeF stringSize = new SizeF(0, 0);
                foreach (string text in myCombo.GetDisplayText())
                {
                    stringSize = g.MeasureString(text, base.DataGridTableStyle.DataGrid.Font);
                    if (stringSize.Width > widest)
                    {
                        widest = (int)Math.Ceiling(stringSize.Width);
                    }
                }

                return new Size(widest + 25, myCombo.PreferredHeight + 2);
            } // GetPreferredSize


            protected override int GetMinimumHeight()
            {
                return myCombo.PreferredHeight + 2;
            }

            protected override int GetPreferredHeight(Graphics g,
                object value)
            {
                //return myCombo.PreferredHeight + 4;
                return FontHeight + 2;
            }

            protected override void Paint(Graphics g,
                Rectangle bounds,
                CurrencyManager source,
                int rowNum)
            {
                Paint(g, bounds, source, rowNum, false);
            }
            protected override void Paint(
                Graphics g,
                Rectangle bounds,
                CurrencyManager source,
                int rowNum,
                bool alignToRight)
            {
                Paint(
                    g, bounds,
                    source,
                    rowNum,
                    Brushes.Red,
                    Brushes.Blue,
                    alignToRight);
            }
            protected override void Paint(
                Graphics g,
                Rectangle bounds,
                CurrencyManager source,
                int rowNum,
                Brush backBrush,
                Brush foreBrush,
                bool alignToRight)
            {
                string Combo = (string)
                    GetColumnValueAtRow(source, rowNum).ToString();
                Rectangle rect = bounds;
                g.FillRectangle(backBrush, rect);
                rect.Offset(0, 2);
                rect.Height -= 2;
                g.DrawString(Combo,
                    this.DataGridTableStyle.DataGrid.Font,
                    foreBrush, rect);
            }

            protected override void SetDataGridInColumn(DataGrid value)
            {
                base.SetDataGridInColumn(value);
                if (myCombo.Parent != null)
                {
                    myCombo.Parent.Controls.Remove
                        (myCombo);
                }
                if (value != null)
                {
                    value.Controls.Add(myCombo);
                }
            }
            private void ComboBoxValueChanged(object sender, EventArgs e)
            {
                this.isEditing = true;
                base.ColumnStartedEditing(myCombo);
            }
        }
        internal class DataGridComboBox : ComboBox
        {
            private const int WM_KEYUP = 0x101;

            protected override void WndProc(ref System.Windows.Forms.Message message)
            {
                // Ignore keyup to avoid problem with tabbing and dropdown list.
                if (message.Msg == WM_KEYUP)
                {
                    return;
                }

                base.WndProc(ref message);
            } // WndProc

            public string GetValueText(int index)
            {
                // Validate the index.
                if ((index < 0) && (index >= base.Items.Count))
                    throw new IndexOutOfRangeException("Invalid index.");

                // Get the text.
                string text = string.Empty;
                int memIndex = -1;
                try
                {
                    base.BeginUpdate();
                    memIndex = base.SelectedIndex;
                    base.SelectedIndex = index;
                    text = base.SelectedValue.ToString();
                    base.SelectedIndex = memIndex;
                }
                catch
                {
                }
                finally
                {
                    base.EndUpdate();
                }

                return text;
            } // GetValueText

            public string GetDisplayText(int index)
            {
                // Validate the index.
                if ((index < 0) && (index >= base.Items.Count))
                    throw new IndexOutOfRangeException("Invalid index.");

                // Get the text.
                string text = string.Empty;
                int memIndex = -1;
                try
                {
                    base.BeginUpdate();
                    memIndex = base.SelectedIndex;
                    base.SelectedIndex = index;
                    text = base.SelectedItem.ToString();
                    base.SelectedIndex = memIndex;
                }
                catch
                {
                }
                finally
                {
                    base.EndUpdate();
                }

                return text;
            } // GetDisplayText

            public string GetDisplayText(object value)
            {
                // Get the text.
                string text = string.Empty;
                int memIndex = -1;
                try
                {
                    base.BeginUpdate();
                    memIndex = base.SelectedIndex;
                    base.SelectedValue = value.ToString();
                    text = base.SelectedItem.ToString();
                    base.SelectedIndex = memIndex;
                }
                catch
                {
                }
                finally
                {
                    base.EndUpdate();
                }

                return text;
            } // GetDisplayText

            public string[] GetDisplayText()
            {
                // Get the text.
                string[] text = new string[base.Items.Count];
                int memIndex = -1;
                try
                {
                    base.BeginUpdate();
                    memIndex = base.SelectedIndex;
                    for (int index = 0; index < base.Items.Count; index++)
                    {
                        base.SelectedIndex = index;
                        text[index] = base.SelectedItem.ToString();
                    }
                    base.SelectedIndex = memIndex;
                }
                catch
                {
                }
                finally
                {
                    base.EndUpdate();
                }

                return text;
            } // GetDisplayText

        } // DataGridComboBox
        #endregion internal class
    }
}
