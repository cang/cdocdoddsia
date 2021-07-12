using System;
using System.Windows.Forms;
using System.Drawing;

using SIA.Common;

namespace SIA.UI.Controls.UserControls
{
	/// <summary>
	/// Summary description for PropertyListView.
	/// </summary>
	public class PropertyListView : ListView 
	{
		#region Windows Form members
		private System.Windows.Forms.ColumnHeader colKey;
		private System.Windows.Forms.ColumnHeader colType;
		private System.Windows.Forms.ColumnHeader colValue;
		private System.Windows.Forms.ColumnHeader colComment;

		private System.Windows.Forms.TextBox editBox;	
		private System.Windows.Forms.ComboBox cmbBox;

		#endregion

		#region Member Fields
		
		private int X=0;
		private int Y=0;
		private string subItemText ;
		private int subItemSelected = 0 ;
		private ListViewItem listVewItem;

		private RasterImagePropertyCollection _properties;

		#endregion

		#region Public Properties

		public RasterImagePropertyCollection Properties
		{
			get {return _properties;}
			set 
			{
				_properties = null;
				if (value != null)
					_properties = value.Clone() as RasterImagePropertyCollection;
				OnPropertiesChanged();
			}
		}

		protected virtual void OnPropertiesChanged()
		{
			this.UpdateData(false);
		}

		#endregion
		
		#region Constructor and destructor

		public PropertyListView()
		{
			this.InitializeComponent();

			if (!this.Controls.Contains(editBox))
			{
				this.editBox.Visible = false;
				this.Controls.Add(editBox);
			}

			if (!this.Controls.Contains(cmbBox))
			{
				this.cmbBox.Visible = false;
				this.Controls.Add(cmbBox);
			}
		}

		#endregion

		#region Windows Form generated code
		private void InitializeComponent()
		{
			this.colKey = new System.Windows.Forms.ColumnHeader();
			this.colType = new System.Windows.Forms.ColumnHeader();
			this.colValue = new System.Windows.Forms.ColumnHeader();
			this.colComment = new System.Windows.Forms.ColumnHeader();
			this.editBox = new System.Windows.Forms.TextBox();
			this.cmbBox = new System.Windows.Forms.ComboBox();
			// 
			// colKey
			// 
			this.colKey.Text = "Name";
			this.colKey.Width = 100;
			// 
			// colType
			// 
			this.colType.Text = "Type";
			this.colType.Width = 70;
			// 
			// colValue
			// 
			this.colValue.Text = "Value";
			this.colValue.Width = 165;
			// 
			// colComment
			// 
			this.colComment.Text = "Comment";
			this.colComment.Width = 296;
			// 
			// editBox
			// 
			this.editBox.AcceptsReturn = true;
			this.editBox.Location = new System.Drawing.Point(17, 17);
			this.editBox.Name = "editBox";
			this.editBox.TabIndex = 0;
			this.editBox.Text = "";
			this.editBox.LostFocus += new System.EventHandler(this.editBox_LostFocus);
			this.editBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editBox_KeyPress);
			// 
			// cmbBox
			// 
			this.cmbBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbBox.Location = new System.Drawing.Point(105, 17);
			this.cmbBox.Name = "cmbBox";
			this.cmbBox.TabIndex = 0;
			this.cmbBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbBox_KeyPress);
			this.cmbBox.SelectedIndexChanged += new System.EventHandler(this.cmbBox_SelectedIndexChanged);
			this.cmbBox.LostFocus += new System.EventHandler(this.cmbBox_LostFocus);
			// 
			// PropertyListView
			// 
			this.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																			  this.colKey,
																			  this.colType,
																			  this.colValue,
																			  this.colComment});
			this.FullRowSelect = true;
			this.GridLines = true;
			this.Size = new System.Drawing.Size(0, 0);
			this.View = System.Windows.Forms.View.Details;
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PropertyListView_MouseDown);
			//this.DoubleClick += new System.EventHandler(this.PropertyListView_DoubleClick);

		}

		#endregion

		#region Event Handlers

		private void cmbBox_KeyPress(object sender , System.Windows.Forms.KeyPressEventArgs e)
		{
			if ( e.KeyChar == 13 || e.KeyChar == 27 )
			{
				cmbBox.Hide();
			}
		}

		private void cmbBox_SelectedIndexChanged(object sender , System.EventArgs e)
		{
			int sel = cmbBox.SelectedIndex;
			if ( sel >= 0 )
			{
				string itemSel = cmbBox.Items[sel].ToString();
				listVewItem.SubItems[subItemSelected].Text = itemSel;
			}
		}

		private void cmbBox_LostFocus(object sender , System.EventArgs e)
		{
			cmbBox.Hide() ;
		}
	
		private void editBox_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if ( e.KeyChar == 13 ) 
			{
				listVewItem.SubItems[subItemSelected].Text = editBox.Text;
				editBox.Hide();
			}

			if ( e.KeyChar == 27 ) 
				editBox.Hide();
		}

		private void editBox_LostFocus(object sender, System.EventArgs e)
		{
			listVewItem.SubItems[subItemSelected].Text = editBox.Text;
			editBox.Hide();
		}

		public void PropertyListView_DoubleClick(object sender, System.EventArgs e)
		{
			// Check the subitem clicked .
			int nStart = X ;
			int spos = 0 ; 
			int epos = 0;//this.Columns[0].Width ;
			for ( int i=0; i < this.Columns.Count ; i++)
			{
				epos += this.Columns[i].Width;

				if ( nStart > spos && nStart < epos ) 
				{
					subItemSelected = i ;
					break; 
				}
				
				spos = epos ; 
				//epos += this.Columns[i].Width;
			}
			
			subItemText = listVewItem.SubItems[subItemSelected].Text ;
			RasterImagePropertyItem item = listVewItem.Tag as RasterImagePropertyItem;
			ColumnHeader column = this.Columns[subItemSelected];

			if (item != null)
			{
				if (column == colValue)
				{	
					Type dataType = null;
					if (item.Value == null)
						dataType = typeof(string);
					else
						dataType = item.Value.GetType();
                    
					if (dataType == typeof(string))
					{
						Rectangle r = new Rectangle(spos , listVewItem.Bounds.Y , epos , listVewItem.Bounds.Bottom);
						editBox.Size  = new System.Drawing.Size(epos - spos , listVewItem.Bounds.Bottom-listVewItem.Bounds.Top);
						editBox.Location = new System.Drawing.Point(spos , listVewItem.Bounds.Y);
						editBox.Show() ;
						editBox.Text = subItemText;
						editBox.SelectAll() ;
						editBox.Focus();
					}
				}
				else if (column == colComment)
				{
					Rectangle r = new Rectangle(spos , listVewItem.Bounds.Y , epos , listVewItem.Bounds.Bottom);
					editBox.Size  = new System.Drawing.Size(epos - spos , listVewItem.Bounds.Bottom-listVewItem.Bounds.Top);
					editBox.Location = new System.Drawing.Point(spos , listVewItem.Bounds.Y);
					editBox.Show() ;
					editBox.Text = subItemText;
					editBox.SelectAll() ;
					editBox.Focus();
				}
			}
			
//			// Great Britan
//			string Row0 = listVewItem.SubItems[0].Text ;
//			string colName = this.Columns[subItemSelected].Text ;
//			if ( Row0 == "SORIMAT" && colName =="Value") 
//			{
//				cmbBox.Items.Clear();				
//				cmbBox.Items.Add("NOTCH");
//				cmbBox.Items.Add("FLAT");
//				Rectangle r = new Rectangle(spos , listVewItem.Bounds.Y , epos , listVewItem.Bounds.Bottom);
//				cmbBox.Size  = new System.Drawing.Size(epos - spos , listVewItem.Bounds.Bottom-listVewItem.Bounds.Top);
//				cmbBox.Location = new System.Drawing.Point(spos , listVewItem.Bounds.Y);
//				cmbBox.Show() ;
//				cmbBox.Text = subItemText;
//				cmbBox.SelectAll() ;
//				cmbBox.Focus();
//			}
//			else if (Row0=="ORIMARKL" && colName =="Value")
//			{
//				cmbBox.Items.Clear();				
//				cmbBox.Items.Add("UP");
//				cmbBox.Items.Add("DOWN");
//				cmbBox.Items.Add("LEFT");
//				cmbBox.Items.Add("RIGHT");
//		
//				Rectangle r = new Rectangle(spos , listVewItem.Bounds.Y , epos , listVewItem.Bounds.Bottom);
//				cmbBox.Size  = new System.Drawing.Size(epos - spos , listVewItem.Bounds.Bottom-listVewItem.Bounds.Top);
//				cmbBox.Location = new System.Drawing.Point(spos , listVewItem.Bounds.Y);
//				cmbBox.Show() ;
//				cmbBox.Text = subItemText;
//				cmbBox.SelectAll() ;
//				cmbBox.Focus();
//			}
//			else if (Row0=="INSPORI" && colName =="Value")
//			{
//				cmbBox.Items.Clear();				
//				cmbBox.Items.Add("UP");
//				cmbBox.Items.Add("DOWN");
//				cmbBox.Items.Add("LEFT");
//				cmbBox.Items.Add("RIGHT");
//		
//				Rectangle r = new Rectangle(spos , listVewItem.Bounds.Y , epos , listVewItem.Bounds.Bottom);
//				cmbBox.Size  = new System.Drawing.Size(epos - spos , listVewItem.Bounds.Bottom-listVewItem.Bounds.Top);
//				cmbBox.Location = new System.Drawing.Point(spos , listVewItem.Bounds.Y);
//				cmbBox.Show() ;
//				cmbBox.Text = subItemText;
//				cmbBox.SelectAll() ;
//				cmbBox.Focus();
//			}
//			else if (Row0=="COORMIRR" && colName =="Value")
//			{
//				cmbBox.Items.Clear();				
//				cmbBox.Items.Add("YES");
//				cmbBox.Items.Add("NO");				
//		
//				Rectangle r = new Rectangle(spos , listVewItem.Bounds.Y , epos , listVewItem.Bounds.Bottom);
//				cmbBox.Size  = new System.Drawing.Size(epos - spos , listVewItem.Bounds.Bottom-listVewItem.Bounds.Top);
//				cmbBox.Location = new System.Drawing.Point(spos , listVewItem.Bounds.Y);
//				cmbBox.Show() ;
//				cmbBox.Text = subItemText;
//				cmbBox.SelectAll() ;
//				cmbBox.Focus();
//			}
//			else if (Row0=="INSPAREA" && colName =="Value")
//			{
//				cmbBox.Items.Clear();				
//				cmbBox.Items.Add("FRONT");
//				cmbBox.Items.Add("BACK");				
//				cmbBox.Items.Add("EDGE");				
//		
//				Rectangle r = new Rectangle(spos , listVewItem.Bounds.Y , epos , listVewItem.Bounds.Bottom);
//				cmbBox.Size  = new System.Drawing.Size(epos - spos , listVewItem.Bounds.Bottom-listVewItem.Bounds.Top);
//				cmbBox.Location = new System.Drawing.Point(spos , listVewItem.Bounds.Y);
//				cmbBox.Show() ;
//				cmbBox.Text = subItemText;
//				cmbBox.SelectAll() ;
//				cmbBox.Focus();
//			}
//			else if (colName =="Value")			
//			{
//				Rectangle r = new Rectangle(spos , listVewItem.Bounds.Y , epos , listVewItem.Bounds.Bottom);
//				editBox.Size  = new System.Drawing.Size(epos - spos , listVewItem.Bounds.Bottom-listVewItem.Bounds.Top);
//				editBox.Location = new System.Drawing.Point(spos , listVewItem.Bounds.Y);
//				editBox.Show() ;
//				editBox.Text = subItemText;
//				editBox.SelectAll() ;
//				editBox.Focus();
//			}
		}

		public void PropertyListView_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			listVewItem = this.GetItemAt(e.X , e.Y);
			X = e.X ;
			Y = e.Y ;
		}

		#endregion

		#region Override Routines

		#endregion

		#region Internal Helpers

		public void UpdateData(bool bSaveAndValidate)
		{
			try
			{
				if (bSaveAndValidate)
				{
					RasterImagePropertyCollection properties = new RasterImagePropertyCollection();

					foreach (ListViewItem item in this.Items)
					{
						RasterImagePropertyItem property = item.Tag as RasterImagePropertyItem;
						if (property == null)
							continue;
						properties.Add(property);
					}

					this._properties = properties;
				}
				else
				{
					this.BeginUpdate();
					this.Items.Clear();

					if (this._properties == null)
						return;

					foreach (RasterImagePropertyItem item in this._properties)
					{
						string key = item.Key;
						int dataType = item.DataType;
						string value = "";
						if (item.Value != null)
							value = item.Value.ToString();
						string comment = item.Comment;
					
						ListViewItem newItem = new ListViewItem(new string[] {key, GetType(dataType), value, comment});
						newItem.Tag = item;
						this.Items.Add(newItem);
					}
				}
			}
			finally
			{
				this.EndUpdate();
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

			switch(datavalue)       
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


		#endregion
		
	}

	


}
