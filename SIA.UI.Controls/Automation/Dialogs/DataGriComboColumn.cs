using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;


namespace SIA.UI.Controls.Automation.Dialogs
{
	public class DataGriComboColumn : DataGridColumnStyle 
	{
		public DataGridComboBox myCombo = new DataGridComboBox() ;

        

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

		public DataGriComboColumn() : base() 
		{
			myCombo.Visible = false;

            myCombo.Validating += new System.ComponentModel.CancelEventHandler(this.myCombo_Validating);
		}

		public void Setdata(DataTable mDataTable,string DisplayMembe)
		{
			myCombo.DataSource = mDataTable;
			myCombo.DisplayMember =DisplayMembe;
			myCombo.ValueMember = DisplayMembe;
		}

		protected override void Abort(int rowNum)
		{
			isEditing = false;
			myCombo.Validated -=new EventHandler(ComboBoxValueChanged);
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

			foreach(DataGridColumnStyle col in DataGridTableStyle.GridColumnStyles)
			{
				if( col.GetType()==typeof( DataGriComboColumn) )
					((DataGriComboColumn)col).myCombo.Visible=false;
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
				myCombo.Text  = value;
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
			int		widest		= 0;
			SizeF		stringSize	= new SizeF(0, 0);
			foreach (string text in myCombo.GetDisplayText()) 
			{
				stringSize	= g.MeasureString(text, base.DataGridTableStyle.DataGrid.Font);
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
				g,bounds, 
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
			g.FillRectangle(backBrush,rect);
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
	public class DataGridComboBox : ComboBox 
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
			string	text			= string.Empty;
			int		memIndex		= -1;
			try 
			{
				base.BeginUpdate();
				memIndex					= base.SelectedIndex;
				base.SelectedIndex	= index;
				text						= base.SelectedValue.ToString();
				base.SelectedIndex	= memIndex;
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
			string	text			= string.Empty;
			int		memIndex		= -1;
			try 
			{
				base.BeginUpdate();
				memIndex					= base.SelectedIndex;
				base.SelectedIndex	= index;
				text						= base.SelectedItem.ToString();
				base.SelectedIndex	= memIndex;
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
			string	text			= string.Empty;
			int		memIndex		= -1;
			try 
			{
				base.BeginUpdate();
				memIndex					= base.SelectedIndex;
				base.SelectedValue	= value.ToString();
				text						= base.SelectedItem.ToString();
				base.SelectedIndex	= memIndex;
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
			string[]	text			= new string[base.Items.Count];
			int		memIndex		= -1;
			try 
			{
				base.BeginUpdate();
				memIndex					= base.SelectedIndex;
				for (int index = 0; index < base.Items.Count; index++) 
				{
					base.SelectedIndex	= index;
					text[index]				= base.SelectedItem.ToString();
				}
				base.SelectedIndex	= memIndex;
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
}