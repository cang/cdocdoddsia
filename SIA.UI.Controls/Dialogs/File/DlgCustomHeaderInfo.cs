using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using SIA.Common;
using SIA.SystemLayer;
using SIA.UI.Controls.UserControls;
using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.Utilities;
using SIA.Common.IPLFacade;

namespace SIA.UI.Controls.Dialogs
{
	/// <summary>
	/// Summary description for DlgCustomHeaderInfo.
	/// </summary>
	public class DlgCustomHeaderInfo : DialogBase
	{
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;

        private RasterImagePropertyItem _properties = null;
		private System.Windows.Forms.ComboBox cbType;
		private System.Windows.Forms.TextBox txtComment;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtvalue;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Label label1;
        private Label label13;
		

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public DlgCustomHeaderInfo()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();			

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		public DlgCustomHeaderInfo(RasterImagePropertyItem properties)
		{	
			InitializeComponent();			
			_properties= properties;
			txtName.Text = _properties.Key;
			cbType.Text = GetType(_properties.DataType);
			if(_properties.Value==null)
				txtvalue.Text="";
			else
                txtvalue.Text = _properties.Value.ToString();
			txtComment.Text = _properties.Comment;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgCustomHeaderInfo));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtvalue = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(232, 188);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "&OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(320, 188);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.Items.AddRange(new object[] {
            "Byte",
            "Boolean",
            "Short",
            "UShort",
            "UInt",
            "Int",
            "ULong",
            "Long",
            "Float",
            "Double",
            "String"});
            this.cbType.Location = new System.Drawing.Point(68, 34);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(148, 21);
            this.cbType.TabIndex = 3;
            this.cbType.SelectedIndexChanged += new System.EventHandler(this.cbType_SelectedIndexChanged);
            // 
            // txtComment
            // 
            this.txtComment.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtComment.Location = new System.Drawing.Point(68, 87);
            this.txtComment.Multiline = true;
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(324, 84);
            this.txtComment.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(8, 87);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 21);
            this.label5.TabIndex = 6;
            this.label5.Text = "Comment:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtvalue
            // 
            this.txtvalue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtvalue.Location = new System.Drawing.Point(68, 61);
            this.txtvalue.Name = "txtvalue";
            this.txtvalue.Size = new System.Drawing.Size(324, 20);
            this.txtvalue.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 21);
            this.label3.TabIndex = 4;
            this.label3.Text = "Value:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 21);
            this.label2.TabIndex = 2;
            this.label2.Text = "Type:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtName.Location = new System.Drawing.Point(68, 8);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(324, 20);
            this.txtName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label13.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label13.Location = new System.Drawing.Point(-170, 179);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(741, 2);
            this.label13.TabIndex = 33;
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DlgCustomHeaderInfo
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(400, 218);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.cbType);
            this.Controls.Add(this.txtComment);
            this.Controls.Add(this.txtvalue);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DlgCustomHeaderInfo";
            this.Text = "Custom header infomation";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.DlgCustomHeaderInfo_Closing);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		#region Properties

		public RasterImagePropertyItem Property
		{
			get 
			{ 
				_properties = new RasterImagePropertyItem(txtName.Text,
					GetType(cbType.Text), ToType(cbType.Text, txtvalue.Text), false, txtComment.Text);					
				return _properties; 
			} 
		}
		#endregion

		#region UI Command

		private bool CheckInfo()
		{
			bool result = true;	
			if (txtName.Text == "")
			{
				result = false;
				MessageBoxEx.Error("Please enter keyword name.");
				txtName.Select();
			}			

			RasterImagePropertyItemType dataType = (RasterImagePropertyItemType)GetType(cbType.Text);
			if (dataType == RasterImagePropertyItemType.Boolean)
			{
				string str = txtvalue.Text.ToUpper();
				if (str != "TRUE" && str != "FALSE")
				{
					result = false;
					MessageBoxEx.Error("Value must be TRUE or FALSE.");
					txtvalue.Select();
				}
			}
			else if (dataType == RasterImagePropertyItemType.Byte)
			{
				try
				{
					Convert.ToByte(txtvalue.Text);
				}
				catch 
				{
					result = false;
					MessageBoxEx.Error("Invalid byte value.");
					txtvalue.Select();
				}
			}
			else if (dataType == RasterImagePropertyItemType.Short)
			{
				try
				{
					Convert.ToInt16(txtvalue.Text);
				}
				catch 
				{
					result = false;
					MessageBoxEx.Error("Invalid short value.");
					txtvalue.Select();
				}
			}
			else if (dataType == RasterImagePropertyItemType.UShort)
			{
				try
				{
					Convert.ToUInt16(txtvalue.Text);
				}
				catch 
				{
					result = false;
					MessageBoxEx.Error("Invalid ushort value.");
					txtvalue.Select();
				}
			}
			else if (dataType == RasterImagePropertyItemType.Int)
			{
				try
				{
					Convert.ToInt32(txtvalue.Text);
				}
				catch 
				{
					result = false;
					MessageBoxEx.Error("Invalid int value.");
					txtvalue.Select();
				}
			}
			else if (dataType == RasterImagePropertyItemType.UInt)
			{
				try
				{
					Convert.ToUInt32(txtvalue.Text);
				}
				catch 
				{
					result = false;
					MessageBoxEx.Error("Invalid uint value.");
					txtvalue.Select();
				}
			}
			else if (dataType == RasterImagePropertyItemType.Long)
			{
				try
				{
					Convert.ToInt32(txtvalue.Text);
				}
				catch 
				{
					result = false;
					MessageBoxEx.Error("Invalid long value.");
					txtvalue.Select();
				}
			}
			else if (dataType == RasterImagePropertyItemType.ULong)
			{
				try
				{
					Convert.ToUInt32(txtvalue.Text);
				}
				catch 
				{
					result = false;
					MessageBoxEx.Error("Invalid ulong value.");
					txtvalue.Select();
				}
			}
			else if (dataType == RasterImagePropertyItemType.Float)
			{
				try
				{
					Convert.ToSingle(txtvalue.Text);
				}
				catch 
				{
					result = false;
					MessageBoxEx.Error("Invalid float value.");
					txtvalue.Select();
				}
			}
			else if (dataType == RasterImagePropertyItemType.Double)
			{
				try
				{
					Convert.ToDouble(txtvalue.Text);
				}
				catch 
				{
					result = false;
					MessageBoxEx.Error("Invalid double value.");
					txtvalue.Select();
				}
			}

			return result;
		}


		public static bool	IsNumber(string exp)
		{
			try
			{
				exp = exp.Replace(",", "");
				double.Parse(exp);
				return true;
			}
			catch
			{
				return false;
			}
		}

		
		public int GetType(string datatype)
		{

			switch(datatype)       
			{         
				case "Byte":   
					return 11;					
				case "Boolean":            
					return 14;
				case "Short":            
					return 21;
				case "UShort":            
					return 20;
				case "UInt":
					return 30;
				case "Int":
					return 31;
				case "ULong":
					return 40;
				case "Long":
					return 41;
				case "Float":
					return 42;
				case "Double":
					return 82;
				case "String":
					return 16;
				default:            
					return -1;
			}		

		}


		public string GetType(int datavalue)
		{

			switch (datavalue)       
			{         
				case 11:   
					return "Byte";					
				case 14:            
					return "Boolean";
				case 20:            
					return "UShort";
				case 30:
					return "UInt";
				case 31:
					return "Int";
				case 40:
					return "ULong";
				case 41:
					return "Long";
				case 42:
					return "Float";
				case 82:
					return "Double";
				case 16:
					return "String";
				case 21:            
					return "Short";
				default:            
					return string.Empty;
			}		
		}

		public object ToType(string datatype, string value)
		{
			switch(datatype)       
			{         
				case "Byte":   
					return Convert.ToByte(value);		
				case "Boolean":            
					return value.ToUpper() == "TRUE" ? true : false;
				case "Short":            
					return Convert.ToInt16(value);
				case "UShort":            
					return Convert.ToUInt16(value);
				case "Int":
				case "Long":
					return Convert.ToInt32(value);
				case "UInt":
				case "ULong":
					return Convert.ToUInt32(value);
				case "Float":
					return Convert.ToSingle(value);
				case "Double":
					return Convert.ToDouble(value);
				case "String":
				default:
					return value.ToString();
			}		
		}


		#endregion

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}


		private void btnOK_Click(object sender, System.EventArgs e)
		{
			
		}

		private void DlgCustomHeaderInfo_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (this.DialogResult == DialogResult.OK)
			{
				if (!CheckInfo())
					e.Cancel = true;
			}
		}

		private void cbType_TextChanged(object sender, System.EventArgs e)
		{
			
		}

		private void cbType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			// reset value
			txtvalue.Text = "";
		}
	}
}

