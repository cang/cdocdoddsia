using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using SIA.UI.Controls;
using System.Diagnostics;

namespace SIA.UI.Components
{
	/// <summary>
	/// A grid control that support load from a datatable.
	/// </summary>
	public class GridDataTable : SourceGrid2.GridVirtual
    {
        #region Fields

        private System.Windows.Forms.MenuItem _mnuDelete;
        private DataTable _dataTable;

        private CellColumnTemplate[] _dataCell;
        private CellHeaderDataTable _headerCell;
        private CellColHeaderDataTable _colHeaderCell;
        private CellRowHeaderDataTable _rowHeaderCell;

        private GridDataTableStyle _style = GridDataTableStyle.Default;
        private bool _enableEdit = true;
        private bool _enableDelete = true;

        //array of DataRow
        private ArrayList _dataSelection = null;
        private string _sort;
		
        #endregion

        #region Properties

        public DataTable DataTable
        {
            get { return _dataTable; }
        }

        public GridDataTableStyle GridStyle
        {
            get { return _style; }
        }

        public bool EnableEdit
        {
            get { return _enableEdit; }
            set
            {
                _enableEdit = value;
                RefreshCellsStyle();
            }
        }

        public bool EnableDelete
        {
            get { return _enableDelete; }
            set
            {
                _enableDelete = value;
                RefreshCellsStyle();
            }
        }

        public DataRow FocusDataRow
        {
            get
            {
                if (FocusCellPosition.IsEmpty() || FocusCellPosition.Row < FixedRows || (FocusCellPosition.Row - 1) >= _dataSelection.Count)
                    return null;
                else
                {
                    return (DataRow)_dataSelection[FocusCellPosition.Row - 1];
                }
            }
        }

        public DataColumn FocusDataColumn
        {
            get
            {
                if (FocusCellPosition.IsEmpty() || FocusCellPosition.Column < FixedColumns || (FocusCellPosition.Column - 1) >= _dataCell.Length)
                    return null;
                else
                {
                    return _dataCell[FocusCellPosition.Column - 1].DataColumn;
                }
            }
        }

        #endregion

        #region Constructor and destructor

        public GridDataTable()
        {            
            this.ContextMenuStyle = SourceGrid2.ContextMenuStyle.CopyPasteSelection;
            
			Selection.AutoClear = false;
			Selection.ClearCells += new EventHandler(Selection_ClearCells);
            
            _mnuDelete = new MenuItem("Delete", new EventHandler(MenuDelete_Click));
            Selection.ContextMenuItems = new SourceGrid2.MenuCollection();
            Selection.ContextMenuItems.Add(_mnuDelete);
        }

        protected override void Dispose(bool disposing)
        {
            UnloadDataSource();

            base.Dispose(disposing);
        }

        #endregion

        #region Methods

        public virtual void UnloadDataSource()
		{
			if (_dataTable != null)
			{
				_dataTable.RowChanged -= new DataRowChangeEventHandler(dataTable_RowChanged);
				_dataTable.RowDeleted -= new DataRowChangeEventHandler(dataTable_RowDeleted);

				_dataTable = null;
			}
			_sort = null;
		}

		public virtual void LoadDataSource(DataTable table)
		{
            LoadDataSource(table, GridDataTableStyle.Default, GetColumnsFromDataTable(table, _enableEdit));
		}

        public virtual void LoadDataSource(DataTable table, GridDataTableStyle style, CellColumnTemplate[] dataColumns)
		{
			//unload data source
			UnloadDataSource();

            _dataTable = table;
			
            _dataTable.RowChanged += new DataRowChangeEventHandler(dataTable_RowChanged);
			_dataTable.RowDeleted += new DataRowChangeEventHandler(dataTable_RowDeleted);

			_style = style;

			if ( (style & GridDataTableStyle.ColumnHeader) == GridDataTableStyle.ColumnHeader)
				FixedRows = 1;
			else
				FixedRows = 0;
			if ( (style & GridDataTableStyle.RowHeader) == GridDataTableStyle.RowHeader)
				FixedColumns = 1;
			else
				FixedColumns = 0;

			Redim(_dataTable.Rows.Count+FixedRows, dataColumns.Length+FixedColumns);
			Selection.SelectionMode = SourceGrid2.GridSelectionMode.Row;

			//Col Header Cell Template
			_colHeaderCell = new CellColHeaderDataTable(_dataTable);
			_colHeaderCell.BindToGrid(this);

			//Row Header Cell Template
			_rowHeaderCell = new CellRowHeaderDataTable();
			_rowHeaderCell.BindToGrid(this);

			//Header Cell Template (0,0 cell)
			_headerCell = new CellHeaderDataTable();
			_headerCell.BindToGrid(this);

			//Data Cell Template (one for each column
			_dataCell = dataColumns;
			for (int i = 0; i < _dataCell.Length; i++)
				_dataCell[i].BindToGrid(this);

			RefreshDataSelection();
		}

		public void DeleteFocusDataRow()
        {
            try
            {
                if (FocusDataRow != null)
                {
                    if (MessageBoxEx.ConfirmYesNo("Are you sure to delete selected row?"))
                    {
                        FocusDataRow.Delete();
                        RefreshDataSelection();
                    }
                }
            }
            catch (Exception exp)
            {
                Trace.WriteLine(exp);
            }
        }

        public void AddDataRow()
        {
            DataRow l_NewRow = _dataTable.NewRow();
            _dataTable.Rows.Add(l_NewRow);
            _dataSelection.Add(l_NewRow);
            Rows.Insert(Rows.Count);
            Rows[Rows.Count - 1].Focus();
        }

        #endregion

        #region Override Routines

        protected virtual void RefreshDataSelection()
        {
            _dataSelection = new ArrayList(_dataTable.Select(null, _sort));
            RowsCount = _dataSelection.Count + FixedRows;
        }

        public override SourceGrid2.Cells.ICellVirtual GetCell(int p_iRow, int p_iCol)
		{
			try
			{
				if (_dataTable!=null)
				{
					if (p_iRow<FixedRows && p_iCol<FixedColumns)
						return _headerCell;
					else if (p_iRow < FixedRows)
						return _colHeaderCell;
					else if (p_iCol < FixedColumns)
						 return _rowHeaderCell;
					else
						return _dataCell[p_iCol - FixedColumns];
				}
				else
					return null;
			}
			catch(Exception err)
			{
				System.Diagnostics.Debug.Assert(false, err.Message);
				return null;
			}		
		}

		public override void SetCell(int iRow, int iCol, SourceGrid2.Cells.ICellVirtual Cell)
		{
			throw new ApplicationException("Cannot set cell for this kind of grid");
		}

		protected override void OnSortingRangeRows(SourceGrid2.SortRangeRowsEventArgs e)
		{
			base.OnSortingRangeRows (e);

			string sortMode;

			if (e.Ascending)
				sortMode = " ASC";
			else
				sortMode = " DESC";

			_sort = _dataTable.Columns[e.AbsoluteColKeys-FixedColumns].ColumnName + sortMode;

			RefreshDataSelection();
        }

        #endregion

        #region Event handlers

        private void Selection_ClearCells(object sender, EventArgs e)
		{
			if ( (_style & GridDataTableStyle.KeyCancDeleteRow) == GridDataTableStyle.KeyCancDeleteRow &&
				_enableDelete)
				DeleteFocusDataRow();
		}

		private void MenuDelete_Click(object sender, EventArgs e)
		{
			if (_enableDelete)
				DeleteFocusDataRow();
		}

		private void dataTable_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			//InvalidateCells();
		}

		private void dataTable_RowDeleted(object sender, DataRowChangeEventArgs e)
		{
			InvalidateCells();
        }

        #endregion

        #region Internal Helpers

        private void RefreshCellsStyle()
        {
            if (_dataCell != null)
            {
                for (int i = 0; i < _dataCell.Length; i++)
                    _dataCell[i].EnableEdit = _enableEdit;
            }

            if (_enableDelete)
                _mnuDelete.Enabled = true;
            else
                _mnuDelete.Enabled = false;
        }


        #endregion

        #region Cell class

        public class CellColumnTemplate : SourceGrid2.Cells.Virtual.CellVirtual
		{
			private DataColumn m_DataColumn;
			private string m_ColumnCaption;

			public CellColumnTemplate(DataColumn p_DataColumn, string p_ColumnCaption)
			{
				m_ColumnCaption = p_ColumnCaption;
				m_DataColumn = p_DataColumn;

				DataModel = SourceGrid2.Utility.CreateDataModel(m_DataColumn.DataType); 
				DataModel.AllowNull = m_DataColumn.AllowDBNull;
				DataModel.NullDisplayString = "<NULL>";
			}

			public string ColumnCaption
			{
				get{return m_ColumnCaption;}
				set{m_ColumnCaption = value;}
			}

			public DataColumn DataColumn
			{
				get{return m_DataColumn;}
			}

			public override bool EnableEdit
			{
				get{return base.EnableEdit & !m_DataColumn.ReadOnly;}
				set
				{
					base.EnableEdit = value & !m_DataColumn.ReadOnly;
				}
			}

			public override object GetValue(SourceGrid2.Position p_Position)
			{
				GridDataTable l_GridDataTable = ((GridDataTable)Grid);
				DataRow l_Row = (DataRow)(l_GridDataTable._dataSelection[p_Position.Row-Grid.FixedRows]);
				object tmp = l_Row[m_DataColumn];
				if (System.DBNull.Value == tmp)
					return null;
				else
					return tmp;
			}

			public override void SetValue(SourceGrid2.Position p_Position, object p_Value)
			{
				GridDataTable l_GridDataTable = ((GridDataTable)Grid);
				DataRow l_Row = (DataRow)(l_GridDataTable._dataSelection[p_Position.Row-Grid.FixedRows]);
				if (p_Value == null)
					l_Row[m_DataColumn] = System.DBNull.Value;
				else
					l_Row[m_DataColumn] = p_Value;

				OnValueChanged(new SourceGrid2.PositionEventArgs(p_Position, this));
			}
		}

		private class CellHeaderDataTable : SourceGrid2.Cells.Virtual.Header
		{
			public CellHeaderDataTable()
			{
			}

			public override object GetValue(SourceGrid2.Position p_Position)
			{
				return null;
			}

			public override void SetValue(SourceGrid2.Position p_Position, object p_Value)
			{
				throw new ApplicationException("This cell cannot be modified");
			}		
		}

		private class CellRowHeaderDataTable : SourceGrid2.Cells.Virtual.RowHeader
		{
			public CellRowHeaderDataTable()
			{
			}

			public override object GetValue(SourceGrid2.Position p_Position)
			{
				return p_Position.Row;
			}

			public override void SetValue(SourceGrid2.Position p_Position, object p_Value)
			{
				throw new ApplicationException("This cell cannot be modified");
			}	
		}

		private class CellColHeaderDataTable : SourceGrid2.Cells.Virtual.ColumnHeader
		{
			private DataTable m_DataTable;
			public CellColHeaderDataTable(DataTable p_DataTable)
			{
				m_DataTable = p_DataTable;
			}

			public override object GetValue(SourceGrid2.Position p_Position)
			{
				return GetColCaption(p_Position.Column);
			}

			public override void SetValue(SourceGrid2.Position p_Position, object p_Value)
			{
				throw new ApplicationException("This cell cannot be modified");
			}

			private string m_ColumnSort = null;
			private bool m_bAscending = false;

			private string GetColCaption(int p_Column)
			{
				return ((GridDataTable)Grid)._dataCell[p_Column-Grid.FixedColumns].ColumnCaption;
			}

			public override SourceGrid2.SortStatus GetSortStatus(SourceGrid2.Position p_Position)
			{
				if (GetColCaption(p_Position.Column) == m_ColumnSort)
				{
					if (m_bAscending)
						return new SourceGrid2.SortStatus (SourceGrid2.GridSortMode.Ascending, true);
					else
						return new SourceGrid2.SortStatus (SourceGrid2.GridSortMode.Descending, true);
				}
				else
					return new SourceGrid2.SortStatus (SourceGrid2.GridSortMode.None, true);
			}

			public override void SetSortMode(SourceGrid2.Position p_Position, SourceGrid2.GridSortMode p_Mode)
			{
				if (p_Mode == SourceGrid2.GridSortMode.Ascending)
				{
					m_ColumnSort = GetColCaption(p_Position.Column);
					m_bAscending = true;
				}
				else if (p_Mode == SourceGrid2.GridSortMode.Descending)
				{
					m_ColumnSort = GetColCaption(p_Position.Column);
					m_bAscending = false;
				}
				else
					m_ColumnSort = null;
			}
		}

		#endregion

        public static CellColumnTemplate[] GetColumnsFromDataTable(DataTable p_Table, bool p_EnableEdit)
        {
            CellColumnTemplate[] l_Cells;
            //Data Cell Template (one for each column
            l_Cells = new CellColumnTemplate[p_Table.Columns.Count];
            for (int c = 0; c < p_Table.Columns.Count; c++)
            {
                l_Cells[c] = new CellColumnTemplate(p_Table.Columns[c], p_Table.Columns[c].Caption);
                l_Cells[c].EnableEdit = p_EnableEdit;
            }
            return l_Cells;
        }

        public enum GridDataTableStyle
		{
			None = 0,
			KeyCancDeleteRow = 4,
			RowHeader = 16,
			ColumnHeader = 32,
			Default = KeyCancDeleteRow|RowHeader|ColumnHeader
		}
	}
}

