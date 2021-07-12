using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;

using SIA.UI.Controls.Automation.Dialogs;
using SIA.UI.Controls.Automation.Commands;
using SIA.UI.Controls.Automation;

using SiGlaz.UI.CustomControls.XPTable;
using SiGlaz.UI.CustomControls.XPTable.Editors;
using SiGlaz.UI.CustomControls.XPTable.Models;
using SiGlaz.UI.CustomControls.XPTable.Events;

using SIA.UI.Controls;
using SIA.UI.Controls.Common;
using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.Utilities;

using SIA.UI.Controls.Automation.Steps;
using SIA.UI.Controls.Commands;

using SIA.Plugins.Common;

using SIA.Workbench.Common;

namespace SIA.Workbench.UserControls
{
	/// <summary>
	/// Summary description for ScriptTable.
	/// </summary>
    internal class ScriptTable : Table
	{
		public const int colIndex = 0;
		public const int colSelected = 1;
		public const int colStep = 2;
		public const int colSettings = 3;

		private TableModel tableModel = new TableModel();
		private ColumnModel columnModel = new ColumnModel();

		public int StepCount
		{
			get {return tableModel.Rows.Count;}
		}
		

		public event EventHandler SettingsModified = null;

		public ScriptTable() : base()
		{
			this.InitClass();
		}

		
		public void InitClass()
		{
			try
			{
				this.BeginUpdate();

				// 1
				TextColumn textColumnNo = new TextColumn("No.", 30);
				textColumnNo.Alignment = ColumnAlignment.Center;
				textColumnNo.Sortable = false;
				textColumnNo.Editable = false;

				// 2
				CheckBoxColumn checkboxColumnStep = new CheckBoxColumn("", 15);
				checkboxColumnStep.Alignment = ColumnAlignment.Center;
				checkboxColumnStep.Sortable = false;

				//3
				ComboBoxColumn comboboxColumnStep = new ComboBoxColumn("Step Name", 570);
				comboboxColumnStep.Sortable = false;
				
				//4
				ButtonColumn buttonColumnSetting = new ButtonColumn("Settings", 50);
				buttonColumnSetting.Alignment = ColumnAlignment.Center;
				buttonColumnSetting.Sortable  = false;

				this.ColumnModel = columnModel;
				columnModel.Columns.AddRange(new Column[] {
															  textColumnNo,
															  checkboxColumnStep,
															  comboboxColumnStep,
															  buttonColumnSetting});
			
				// create table model if it's null
				this.TableModel = tableModel;

				// Set row height
				this.tableModel.RowHeight = 21;

				this.FullRowSelect = true;

				// add default empty row
				//this.AddEmptyRow();
			}
			catch (System.Exception exp)
			{
				Trace.WriteLine(exp.Message + exp.StackTrace);
			}
			finally
			{
				this.EndUpdate();
			}
		}

		public void UninitClass()
		{

		}


		public void LoadFromScript(Script script)
		{
			try
			{
				// signal begin update table
				this.BeginUpdate();

				// clear old rows
				this.tableModel.Rows.Clear();
			
				if (script.ProcessSteps.Count > 0)
				{
					// append new rows
					foreach (ProcessStep step in script.ProcessSteps)
						this.AddProcessStep(step);
				}
				else
				{
					// append default load image step
					ProcessStep step = new LoadImageStep();
					this.AddProcessStep(step);
				}
			}     
			catch (System.Exception exp)
			{
				Trace.WriteLine(exp);
			}
			finally
			{
				// signal complete update table
				this.EndUpdate();
			}
		}
		
		public void UpdateStepIndex()
		{
			for (int rowIndex=0; rowIndex < tableModel.Rows.Count; rowIndex++)
			{
				IProcessStep step = this.GetProcessStep(rowIndex);
				step.Index = rowIndex;
			}
		}
		
		public void AddProcessStep(IProcessStep step)
		{
			if (step == null)
				throw new ArgumentNullException("step");
			
			Row row = this.AddEmptyRow();
			// set processing step name
			row.Cells[colStep].Text = step.DisplayName;
			// set processing step instance
			row.Cells[colStep].Data = step;
			// set process step settings data
			row.Cells[colSettings].Data = step.Settings;
			// set status of settings column
			row.Cells[colSettings].Enabled = step.HasSettings;
		}

		public void RemoveSelectedProcessSteps()
		{
			foreach(Row item in this.tableModel.Rows)
			{
				if( item.Cells[colSelected].Checked)
				{
					// clean up processing step runtime type
					item.Cells[colStep].Data = null;

					// clean up settings reference for column settings button
					item.Cells[colSettings].Data = null;
				}		
			}	

			this.RemoveSelectedRows();
		}


		public void MoveSelStepUp()
		{
			if (this.tableModel.Selections.SelectedItems.GetLength(0) != 1)			
				return;
			
			Row selRow = this.tableModel.Selections.SelectedItems[0];
			// keep the first row
			// if (selRow.Index == 1) 
			//	return;

			int index = selRow.Index-1;
			this.tableModel.Rows.Remove(selRow);
			this.tableModel.Rows.Insert(index, selRow);
			this.tableModel.Selections.SelectCell(index, 2);
			
			// re-index rows
			foreach(Row item in this.tableModel.Rows)
				item.Cells[colIndex].Text =(item.Index+ 1).ToString();;				
		}

		public void MoveSelStepDown()
		{
			if (this.tableModel.Selections.SelectedItems.GetLength(0) != 1)
				return;

			Row selRow = this.tableModel.Selections.SelectedItems[0];
			// calculate next row index 
			int index = selRow.Index+1;
			// remove the selected row
			this.tableModel.Rows.Remove(selRow);
			// insert the remove row into new position
			this.tableModel.Rows.Insert(index, selRow);
			// select the new poisition
			this.tableModel.Selections.SelectCell(index, 2);
			
			// re-index rows
			foreach(Row item in this.tableModel.Rows)
			{
				string text = (item.Index+1).ToString();
				item.Cells[colIndex].Text = text;
			}
		}

		
		protected virtual Row AddEmptyRow()
		{
			int iRows = this.tableModel.Rows.Count+1;
			
			Row row  = null;
			try
			{
				DataItemCollection items = this.GetProcessStepsDataSource(false);
				Cell cell = null;
				row = new Row();

				// create index cell
				cell = new Cell(iRows.ToString());
				row.Cells.Add(cell);

				// create selectable cell
				cell = new Cell("", false);
				row.Cells.Add(cell);

				// create step selection cell
				cell = new Cell("");
				row.Cells.Add(cell);

				// create setting cell
				cell = new Cell("...");
				row.Cells.Add(cell);
			}
			catch 
			{
				throw;
			}
			finally
			{
				if (row != null)
					this.tableModel.Rows.Add(row);
			}		
	
			return row;
		}

		protected virtual void RemoveSelectedRows()
		{
			ArrayList selItems = new ArrayList();
			foreach(Row item in this.tableModel.Rows)
			{
				if( item.Cells[colSelected].Checked )
					selItems.Add(item);
			}

			this.tableModel.Rows.RemoveRange(selItems.ToArray(typeof(Row)) as Row[]);

			foreach(Row item in this.tableModel.Rows)
				item.Cells[colIndex].Text = (item.Index + 1).ToString();
		}

		
		protected override void OnBeginEditing(CellEditEventArgs e)
		{
			base.OnBeginEditing (e);

			if (e.Editor is ComboBoxCellEditor)
			{
				int iRow = e.Row;

				// handle event for ComboBox column
				if (e.Column == colStep)
				{
					// initializes drop down combo box
					bool bOnlyLoadStep = false;//iRow == 0;
					DataItemCollection items =  this.GetProcessStepsDataSource(bOnlyLoadStep);
					
					ComboBoxCellEditor  cbCell = (ComboBoxCellEditor)e.Editor;
					cbCell.DropDownStyle = DropDownStyle.DropDownList;
					// fill items with data source
					cbCell.DataSource =	 items;
					cbCell.DisplayMember = "DisplayMember";
					cbCell.ValueMember = "ValueMember";
				}
			}
		}

		protected override void OnEditingStopped(CellEditEventArgs e)
		{
			base.OnEditingStopped (e);

			if (e.Editor is ComboBoxCellEditor)
			{
				int iRow = e.Row;

				// processing for ComboBox control
				if (e.Column == colStep)
				{
					bool bOnlyLoadStep = iRow == 0;

					// initializes combo box
					DataItemCollection items =  this.GetProcessStepsDataSource(bOnlyLoadStep);
					
					ComboBoxCellEditor  cbCell = (ComboBoxCellEditor)e.Editor;
					
					// retrieve selected item's value
					DataItem item = cbCell.SelectedItem as DataItem;
					if (item != null)
					{
						IProcessStep step = item.ValueMember as IProcessStep;
						e.Cell.Data = step;
					}
				}
			}
		}

		protected override void OnCellPropertyChanged(CellEventArgs e)
		{
			base.OnCellPropertyChanged (e);

			// check if cell is StepType column
			if (e.Cell.Index == colStep)
			{				
				// process for non-settable step ex: Invert Image Step
				IProcessStep step = this.GetProcessStep(e.Row);
				if (step != null)
				{
					Cell cell = this.GetCell(e.Row, colSettings);
					cell.Enabled = step.HasSettings;
				}
			}
		}

		protected override void OnCellButtonClicked(CellButtonEventArgs e)
		{
			base.OnCellButtonClicked (e);

			// show settings dialog
			if (e.Cell.Index == ScriptTable.colSettings)
			{ 
				try
				{
					IProcessStep step = this.GetProcessStep(e.Row);
					bool result = step.ShowSettingsDialog(this);
					if (result && this.CanRaiseEvents && this.SettingsModified != null)
						this.SettingsModified(this, EventArgs.Empty);
				}
				catch (System.Exception exp)
				{
					Trace.WriteLine(exp);
				}
			}
		}


		protected override void OnEditingCancelled(CellEditEventArgs e)
		{
			base.OnEditingCancelled (e);

			// check if cell is StepType column
			if (e.Cell.Index == colStep)
			{				
				// process for non-settable step ex: Invert Image Step
				IProcessStep step = this.GetProcessStep(e.Row);
				if (step != null && step.HasSettings)
					this.tableModel.Rows[e.Row].Cells[3].Enabled = step.HasSettings;				
				else
					this.tableModel.Rows[e.Row].Cells[3].Enabled = false;
			}			
		}

		public virtual Row GetRow(int rowIndex)
		{
			if (this.tableModel == null)
				throw new ArgumentNullException("tableModel");
			if (rowIndex<0 || rowIndex>=this.tableModel.Rows.Count)
				throw new ArgumentOutOfRangeException("rowIndex");
			return this.tableModel.Rows[rowIndex];
		}

		public virtual Cell GetCell(int rowIndex, int colIndex)
		{
			Row row = this.GetRow(rowIndex);
			if (colIndex<0 || colIndex > row.Cells.Count)
				throw new ArgumentOutOfRangeException("colIndex");
			return row.Cells[colIndex];
		}

		public virtual IProcessStep GetProcessStep(int rowIndex)
		{
			Row row = this.GetRow(rowIndex);
			return row.Cells[colStep].Data as IProcessStep;
		}

		protected virtual DataItemCollection GetProcessStepsDataSource(bool nonRemovable)
		{
			DataItemCollection items = new DataItemCollection();
			Type type = null;

			ProcessStepInfo[] stepInfos = ProcessStepManager.GetRegistedProcessSteps();
			foreach (ProcessStepInfo stepInfo in stepInfos)
			{
				type = stepInfo.Type;
				IProcessStep step = Activator.CreateInstance(type) as IProcessStep;
				if (nonRemovable && !step.Removable)
					items.Add(step.DisplayName, step);
				else if (!nonRemovable && step.Removable)
					items.Add(step.DisplayName, step);
			}		

			//sort items by display name
			IComparer comparer = new DisplayMemberComparer();
			items.Sort(comparer);

			return items;
		}
	}
}
 