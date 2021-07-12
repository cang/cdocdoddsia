using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using SIA.UI.Controls.Dialogs;

namespace SIA.UI.Controls.Automation.Dialogs
{
	/// <summary>
	/// Summary description for DefineOutputFileName.
	/// </summary>
	public class DefineOutputFileName : DialogBase
	{
		private System.Windows.Forms.DataGrid dataGrid1;
		public System.Windows.Forms.RadioButton radSuffix;
		public System.Windows.Forms.TextBox txtText;
		public System.Windows.Forms.RadioButton radPrefix;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.DataGridTableStyle dataGridTableStyle1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.GroupBox gbText;
		private System.Windows.Forms.CheckBox chkDefineText;
		private bool _isKLARF = true;
		public System.Windows.Forms.CheckBox checkBoxUniqueCode;

        private bool _isSignature  = false;
		

		public DefineOutputFileName(bool isKLARF ,bool isSignature)
		{
			_isKLARF = isKLARF;
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
             _isSignature = isSignature;
			InitData();
           
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

		DataTable dtCondition=new DataTable("Condition");
		private void InitData()
		{
			
		
			dataGridTableStyle1.GridColumnStyles.Clear();

			DataGriComboColumn colTemplate=new DataGriComboColumn();
			colTemplate.myCombo.DropDownStyle = ComboBoxStyle.DropDownList;
			DataTable dtField=new DataTable();
			dtField.Columns.Add(new DataColumn("FieldName"));			

			DataRow dr;

//			dr=dtField.NewRow();
//			dr[0]="Model";
//			dtField.Rows.Add(dr);
//
//			dr=dtField.NewRow();
//			dr[0]="Equipment ID";
//			dtField.Rows.Add(dr);
//			
//			dr=dtField.NewRow();
//			dr[0]="Device ID";
//			dtField.Rows.Add(dr);
//
//			dr=dtField.NewRow();
//			dr[0]="Lot ID";
//			dtField.Rows.Add(dr);
//			
//			dr=dtField.NewRow();
//			dr[0]="Step ID";
//			dtField.Rows.Add(dr);
//
//			dr=dtField.NewRow();
//			dr[0]="Setup ID";
//			dtField.Rows.Add(dr);
//			
//			dr=dtField.NewRow();
//			dr[0]="Slot";
//			dtField.Rows.Add(dr);
//
//			dr=dtField.NewRow();
//			dr[0]="Wafer ID";
//			dtField.Rows.Add(dr);

			dr =dtField.NewRow();
			dr[0]="Source File Name";
			dtField.Rows.Add(dr);

//			dr=dtField.NewRow();
//			dr[0]="Schema File Name";
//			dtField.Rows.Add(dr);	
//			
//            if ( _isSignature  )
//            {
//                dr=dtField.NewRow();
//                dr[0]="Recognize Signature";
//                dtField.Rows.Add(dr);	
//            }		

			dr=dtField.NewRow();
			dr[0]="Current Date";
			dtField.Rows.Add(dr);

			dr=dtField.NewRow();
			dr[0]="Current Time";
			dtField.Rows.Add(dr);

			colTemplate.Setdata(dtField,"FieldName");
			colTemplate.Width=180;
			colTemplate.MappingName="Defect Properties";
			colTemplate.HeaderText="Included Field";
			dataGridTableStyle1.GridColumnStyles.Add(colTemplate);			

			
			dataGridTableStyle1.AllowSorting = false;

			DataColumn dcColumn=new DataColumn("Defect Properties");
			
			dtCondition.Columns.Add(dcColumn);			

			dataGrid1.DataSource=dtCondition;
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.dataGrid1 = new System.Windows.Forms.DataGrid();
			this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
			this.txtText = new System.Windows.Forms.TextBox();
			this.gbText = new System.Windows.Forms.GroupBox();
			this.radSuffix = new System.Windows.Forms.RadioButton();
			this.radPrefix = new System.Windows.Forms.RadioButton();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.chkDefineText = new System.Windows.Forms.CheckBox();
			this.checkBoxUniqueCode = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
			this.gbText.SuspendLayout();
			this.SuspendLayout();
			// 
			// dataGrid1
			// 
			this.dataGrid1.CaptionVisible = false;
			this.dataGrid1.DataMember = "";
			this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGrid1.Location = new System.Drawing.Point(4, 4);
			this.dataGrid1.Name = "dataGrid1";
			this.dataGrid1.Size = new System.Drawing.Size(264, 196);
			this.dataGrid1.TabIndex = 5;
			this.dataGrid1.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
																								  this.dataGridTableStyle1});
			this.dataGrid1.Validating += new System.ComponentModel.CancelEventHandler(this.dataGrid1_Validating);
			this.dataGrid1.Navigate += new System.Windows.Forms.NavigateEventHandler(this.dataGrid1_Navigate);
			this.dataGrid1.CurrentCellChanged += new System.EventHandler(this.dataGrid1_CurrentCellChanged);
			this.dataGrid1.Enter += new System.EventHandler(this.dataGrid1_Enter);
			// 
			// dataGridTableStyle1
			// 
			this.dataGridTableStyle1.DataGrid = this.dataGrid1;
			this.dataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGridTableStyle1.MappingName = "Condition";
			// 
			// txtText
			// 
			this.txtText.Enabled = false;
			this.txtText.Location = new System.Drawing.Point(100, 8);
			this.txtText.MaxLength = 30;
			this.txtText.Name = "txtText";
			this.txtText.Size = new System.Drawing.Size(168, 20);
			this.txtText.TabIndex = 4;
			this.txtText.Text = "";
			// 
			// gbText
			// 
			this.gbText.Controls.Add(this.radPrefix);
			this.gbText.Controls.Add(this.radSuffix);
			this.gbText.Enabled = false;
			this.gbText.Location = new System.Drawing.Point(4, 32);
			this.gbText.Name = "gbText";
			this.gbText.Size = new System.Drawing.Size(264, 48);
			this.gbText.TabIndex = 3;
			this.gbText.TabStop = false;
			this.gbText.Enter += new System.EventHandler(this.gbText_Enter);
			// 
			// radSuffix
			// 
			this.radSuffix.Location = new System.Drawing.Point(19, 29);
			this.radSuffix.Name = "radSuffix";
			this.radSuffix.Size = new System.Drawing.Size(56, 15);
			this.radSuffix.TabIndex = 3;
			this.radSuffix.Text = "Suffix";
			// 
			// radPrefix
			// 
			this.radPrefix.Checked = true;
			this.radPrefix.Location = new System.Drawing.Point(19, 9);
			this.radPrefix.Name = "radPrefix";
			this.radPrefix.Size = new System.Drawing.Size(56, 15);
			this.radPrefix.TabIndex = 2;
			this.radPrefix.TabStop = true;
			this.radPrefix.Text = "Prefix";
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(57, 208);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 6;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(141, 208);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 7;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// chkDefineText
			// 
			this.chkDefineText.Location = new System.Drawing.Point(4, 9);
			this.chkDefineText.Name = "chkDefineText";
			this.chkDefineText.Size = new System.Drawing.Size(96, 16);
			this.chkDefineText.TabIndex = 1;
			this.chkDefineText.Text = "Included Text:";
			this.chkDefineText.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			// 
			// checkBoxUniqueCode
			// 
			this.checkBoxUniqueCode.Enabled = false;
			this.checkBoxUniqueCode.Location = new System.Drawing.Point(4, 88);
			this.checkBoxUniqueCode.Name = "checkBoxUniqueCode";
			this.checkBoxUniqueCode.Size = new System.Drawing.Size(124, 16);
			this.checkBoxUniqueCode.TabIndex = 1;
			this.checkBoxUniqueCode.Text = "Include unique code";
			// 
			// DefineOutputFileName
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(272, 236);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.dataGrid1);
			this.Controls.Add(this.checkBoxUniqueCode);
			this.Controls.Add(this.gbText);
			this.Controls.Add(this.chkDefineText);
			this.Controls.Add(this.txtText);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DefineOutputFileName";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Define";
			this.Load += new System.EventHandler(this.DefineOutputFileName_Load);
			((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
			this.gbText.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void dataGrid1_Enter(object sender, System.EventArgs e)
		{
			if (dataGrid1.CurrentRowIndex ==  dtCondition.Rows.Count)
			{
				DataRow DR= dtCondition.NewRow();				
				DR[0]= "Model";
				dtCondition.Rows.Add(DR);
			}
		}

		private void dataGrid1_CurrentCellChanged(object sender, System.EventArgs e)
		{
			if (dataGrid1.CurrentRowIndex ==  dtCondition.Rows.Count)
			{
				DataRow DR= dtCondition.NewRow();				
				DR[0]= "Model";
				dtCondition.Rows.Add(DR);
			}
		}
		public static bool IsFilenameCorrect(string filename)
		{
			if (filename.IndexOf("\\")>=0)	return false;		
			if (filename.IndexOf("/")>=0)	return false;		
			if (filename.IndexOf("*")>=0)	return false;		
			if (filename.IndexOf("?")>=0)	return false;		
			if (filename.IndexOf("<")>=0)	return false;		
			if (filename.IndexOf(">")>=0)	return false;		
			if (filename.IndexOf("|")>=0)	return false;			
			if (filename.IndexOf(":")>=0)	return false;			
			if (filename.IndexOf("\"")>=0)	return false;
			return true;
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			if(checkBoxUniqueCode.Checked)
			{
				DialogResult = DialogResult.OK;
				return;
			}

			if (chkDefineText.Checked && txtText.Text == "" )
			{
				MessageBoxEx.Error("Input data was not specified.");
				txtText.Focus();
				txtText.Select(0,txtText.Text.Length);
				return;
			}
			if  ( txtText.Text != "" && _isKLARF )
			{
				bool check = IsFilenameCorrect(txtText.Text );					
				string  charaters = @"\ / : * ? <> | ";
				string charater = " \"";
				if(!check) 
				{
					MessageBoxEx.Error("File name cannot contain any of the following charaters : "+charaters +charater);
					txtText.Focus();
					txtText.Select(0,txtText.Text.Length);
					return;
				}
			}
			if  ( _isKLARF && !chkDefineText.Checked && dtCondition.Rows.Count <= 0 )
			{			
				MessageBoxEx.Error("Input file name or select as least one field.");
				txtText.Focus();
				txtText.Select(0,txtText.Text.Length);
				return;
			}

			DialogResult = DialogResult.OK;
			
		}

		private void checkBox1_CheckedChanged(object sender, System.EventArgs e)
		{
			gbText.Enabled = txtText.Enabled = chkDefineText.Checked;
			
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
		
		}

		private void gbText_Enter(object sender, System.EventArgs e)
		{
		
		}

		private void dataGrid1_Navigate(object sender, System.Windows.Forms.NavigateEventArgs ne)
		{
		
		}

        private void dataGrid1_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ((DataTable)dataGrid1.DataSource).AcceptChanges();
        }

		private void DefineOutputFileName_Load(object sender, System.EventArgs e)
		{
			checkBoxUniqueCode.Checked=false;
		}

		public ArrayList FiterField
		{
			get
			{
				ArrayList al = new ArrayList();
				foreach ( DataRow dr in dtCondition.Rows )									
					al.Add( dr[0] );				
				return al;
			}
			set 
			{
				for(int i = 0; i <value.Count ; i++)
				{
					DataRow dr =dtCondition.NewRow();
					dr[0] = value[i].ToString();
					dtCondition.Rows.Add(dr);
				}		
			}
	
		}

		public string DefineText
		{
			get { return txtText.Text ; }
			set { txtText.Text = value; }
		}

		public bool Prefix
		{
			get { return radPrefix.Checked ; }
			set 
			{ 
				radPrefix.Checked = value;
				radSuffix.Checked = !value;
			}
		}

		public bool HasDefineText
		{
			get { return chkDefineText.Checked ; }
			set { chkDefineText.Checked = value; }
		}

		public bool HasUniqueCode
		{
			get { return checkBoxUniqueCode.Checked ; }
			set 
			{
				checkBoxUniqueCode.Enabled=true;
				checkBoxUniqueCode.Checked = value;
			}
		}

		
	}
}
