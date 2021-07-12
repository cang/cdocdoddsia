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
	/// Summary description for StatusTable.
	/// </summary>
    internal class StatusTable : Table
	{
		#region Constants
		public const int colIndex		= 0;
		public const int colName		= 1;
		public const int colProgress	= 2;
		public const int colStartTime	= 3;
		public const int colEndTime		= 4;
		#endregion

		#region member fields
		private TableModel tableModel = new TableModel();
		private ColumnModel columnModel = new ColumnModel();
		#endregion

		#region Properties

		public int StepCount
		{
			get {return tableModel.Rows.Count;}
		}		

		#endregion

		#region constructor and destructor

		public StatusTable() : base()
		{
			this.InitClass();
		}


		#endregion
		
		protected virtual void InitClass()
		{
			try
			{
				this.BeginUpdate();

				// 1. No. Column
				TextColumn textColumnNo = new TextColumn("No.", 30);
				textColumnNo.Sortable = false;
				textColumnNo.Editable = false;

				// 2. Step Name Column
				TextColumn stepName = new TextColumn("Step Name", 300);
				stepName.Sortable = false;
				stepName.Editable = false;

				// 3. Step Progress Column
				ProgressBarColumn stepProgress = new ProgressBarColumn("Step Progress", 100);			
				stepProgress.Sortable = false;
				stepProgress.Editable = false;
				
				//4. Start Time
				TextColumn startTime = new TextColumn("Start Time", 120);
				startTime.Sortable = false;
				startTime.Editable = false;

				//5. End Time
				TextColumn endTime = new TextColumn("End Time", 120);
				endTime.Sortable = false;
				endTime.Editable = false;

				this.ColumnModel = columnModel;
				columnModel.Columns.AddRange(new Column[] {
															  textColumnNo,
															  stepName,
															  stepProgress,
															  startTime,
															  endTime});
			
				// create table model if it's null
				this.TableModel = tableModel;

				// Set row height
				this.tableModel.RowHeight = 21;

				// enable full row select
				this.FullRowSelect = true;
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

		protected virtual void UninitClass()
		{

		}

		
		public void AddProcessStep(string name, int percentage, string startTime, string endTime)
		{
			Row row  = null;
			try
			{
				int  index = this.TableModel.Rows.Count + 1;
                			
				row = new Row(new Cell[] {	new Cell(index.ToString()),
											new Cell(name),
											new Cell(percentage),									
											new Cell(startTime),
											new Cell(endTime)});
			}
			catch
			{
				throw;
			}
			finally
			{
				if (row != null)						
					this.TableModel.Rows.Add(row);
				row = null;
			}			
		}

		public void CreateProgressStatus(Script script)
		{
			try
			{				
				tableModel.Rows.Clear();				
				
				if (script != null)
				{
					int numSteps = script.ProcessSteps.Count;
					for (int iStep = 0; iStep < numSteps; iStep++)
					{
						string stepName = script.ProcessSteps[iStep].DisplayName;
						this.AddProcessStep(stepName, 0, string.Empty, string.Empty);					
					}
				}
			}
			catch
			{
			}
		}

		public void ResetProgressStatus(bool bResetAll)
		{
			TableModel tableModel = this.TableModel;
			if (tableModel == null || tableModel.Rows == null || tableModel.Rows.Count == 0)
				return;

			try
			{
				int numRows = tableModel.Rows.Count;
				for (int i=0; i<numRows; i++)				
				{
					this.SetProgress(i, 0);
					this.SetStartTime(i, string.Empty);
					this.SetEndTime(i, string.Empty);
				}
			}
			catch
			{
			}
		}

		
		public string GetName(int row)
		{
			return this.GetTextDataCell(row, colName);
		}
		
		public void SetProgress(int row, int value)
		{
			this.SetIntDataCell(row, colProgress, value);
		}

		public int GetProgress(int row)
		{
			return this.GetIntDataCell(row, colProgress);
		}

		
		public void SetStartTime(int row, string value)
		{
			this.SetTextDataCell(row, colStartTime, value);
		}

		public string GetStartTime(int row)
		{
			return this.GetTextDataCell(row, colStartTime);
		}
		

		public void SetEndTime(int row, string value)
		{
			this.SetTextDataCell(row, colEndTime, value);
		}

		public string GetEndTime(int row)
		{
			return this.GetTextDataCell(row, colEndTime);
		}

		
		private void SetIntDataCell(int row, int col, int data)
		{
			Cell cell = this.GetCell(row, col);
			cell.Data = data;
		}

		private int GetIntDataCell(int row, int col)
		{
			Cell cell = this.GetCell(row, col);
			if (cell.Data == null)
				return 0;
			return (int)cell.Data;
		}

		
		private void SetTextDataCell(int row, int col, string data)
		{
			Cell cell = this.GetCell(row, col);
			cell.Text = data;
		}

		private string GetTextDataCell(int row, int col)
		{
			Cell cell = this.GetCell(row, col);
			if (cell.Text == null)
				return string.Empty;
			return cell.Text as string;
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
	}
}
 