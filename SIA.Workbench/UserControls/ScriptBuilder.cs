using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Data;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

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
	/// Summary description for ScriptBuilder.
	/// </summary>
    internal class ScriptBuilder : ScriptViewer
	{		
		#region Fields

		private bool _isDropping = false;
		private int _dropItemIndex = -1;

		private int _dragItemIndex = -1;

		#endregion

		#region Events

		public event EventHandler ProcessStepInserted = null;
		public event EventHandler SelectedProcessStepsRemoved = null;
		public event EventHandler SelectedProcessStepMoved = null;
		public event EventHandler ProcessStepSettingsChanged = null;

		#endregion 

		#region Properties

		public bool SelectionHasSettings
		{
			get
			{
				ListBoxItem item = this.SelectedItem as ListBoxItem;
				return item != null && item.ProcessStep != null && item.ProcessStep.HasSettings;
			}
		}

		public bool CanPasteFromClipboard
		{
			get
			{
				IDataObject dataObject = Clipboard.GetDataObject();
				return dataObject.GetDataPresent("ListBoxItemArray");
			}
		}

		#endregion

		#region Constructor and destructor
		
		public ScriptBuilder()
		{
			
		}

		#endregion

		#region Methods

		public void ShowStepSettings()
		{
			if (this.SelectedItem != null)
			{
				ListBoxItem item = this.SelectedItem as ListBoxItem;
				ProcessStep step = item.ProcessStep;
				if (step.HasSettings)
				{
					if (step.ShowSettingsDialog(this.FindForm()))
					{
						if (this.ProcessStepSettingsChanged != null)
							this.ProcessStepSettingsChanged(this, EventArgs.Empty);
					}
				}
			}
		}

		public void RemoveSelectedSteps()
		{
			this.OnRemoveSelectedSteps();
		}

		public void CutSelectedSteps()
		{
			this.CopySelectedSteps();
			this.RemoveSelectedSteps();
		}

		public void CopySelectedSteps()
		{
			if (this.SelectedItems.Count <= 0)
				return;

			ArrayList items = new ArrayList();
			foreach (ListBoxItem item in this.SelectedItems)
			{
				if (item.ProcessStep == null || item.StepInfo == null)
					continue;
				items.Add(item);
			}

			if (items.Count <= 0)
				return;

			ListBoxItem[] lbItems = (ListBoxItem[])items.ToArray(typeof(ListBoxItem));
			CustomDataObject dataObject = new CustomDataObject("ListBoxItemArray");
			dataObject.SetData(lbItems);
			Clipboard.SetDataObject(dataObject);
		}

		public void PasteStepsFromClipboard()
		{
			try
			{
				IDataObject dataObject = Clipboard.GetDataObject();
				if (!dataObject.GetDataPresent("ListBoxItemArray"))
					return ;				

				this.ClearSelected();

				Type type = typeof(ListBoxItem[]);
				object data = dataObject.GetData(type);
				ListBoxItem[] lbItems = (ListBoxItem[])data; 
				foreach (ListBoxItem item in lbItems)
				{
					int index = this.Items.Count-1;
					this.Items.Insert(index, item);
					this.SetSelected(index, true);
				}								
			}
			catch (Exception exp)
			{
				Trace.WriteLine(exp);
			}
		}


		#endregion

		#region Override Routines

		protected override void OnDragEnter(DragEventArgs e)
		{
			base.OnDragEnter (e);
			
			if (e.Data.GetDataPresent(typeof(ProcessStepInfo)))
			{
				Point pt = this.PointToClient(new Point(e.X, e.Y));
				int index = this.IndexFromPoint(pt);
				if (index != ListBox.NoMatches && index > 0)
				{
					_dropItemIndex = index;
					e.Effect = e.AllowedEffect;
					_isDropping = true;
					this.Invalidate(true);
				}				
			}
		}

		protected override void OnDragOver(DragEventArgs e)
		{
			base.OnDragOver (e);

			if (e.Data.GetDataPresent(typeof(ProcessStepInfo)))
			{
				Point pt = this.PointToClient(new Point(e.X, e.Y));
				int index = this.IndexFromPoint(pt);
				if (index != ListBox.NoMatches && index > 0)
				{
					_dropItemIndex = index;
					e.Effect = e.AllowedEffect;
					_isDropping = true;
					this.Invalidate(true);
				}				

			}
		}

		protected override void OnDragLeave(EventArgs e)
		{
			base.OnDragLeave (e); 
			_dropItemIndex = -1;
			_isDropping = false;
			this.Invalidate(true);
		}

		protected override void OnDragDrop(DragEventArgs e)
		{
			base.OnDragDrop (e);

			if (e.Data.GetDataPresent(typeof(ProcessStepInfo)))
			{
				Point pt = this.PointToClient(new Point(e.X, e.Y));
				int index = this.IndexFromPoint(pt);
				if (index != ListBox.NoMatches && index > 0)
				{
					ProcessStepInfo stepInfo = e.Data.GetData(typeof(ProcessStepInfo)) as ProcessStepInfo;
					if (stepInfo != null)
					{
						if (e.AllowedEffect == DragDropEffects.Copy)
						{
							e.Effect = DragDropEffects.Copy;
							_dropItemIndex = index;

							this.OnInsertProcessStep(_dropItemIndex, stepInfo);
						}
						else if (e.AllowedEffect == DragDropEffects.Move)
						{
							ListBoxItem lbItem = null;
							int insertIndex = index;
							int removeIndex = -1;

							for (int i=1; i<this.Items.Count-1; i++)
							{
								ListBoxItem item = this.Items[i] as ListBoxItem;
								if (item == null)
									continue;
								if (item.StepInfo.ID == stepInfo.ID)
								{
									lbItem = item;
									removeIndex = i;
									break;
								}
							}

							this.OnMoveProcessStep(lbItem, insertIndex, removeIndex);							
						}
					}		
					else
					{
						e.Effect = DragDropEffects.None;
					}
				}				

			}

			_isDropping = false;
			_dropItemIndex = -1;
			this.Invalidate(true);
		}
		

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown (e);

			if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
			{
				int index = this.IndexFromPoint(e.X, e.Y);
				if (index > 0 && index < this.Items.Count-1)
				{
					_dragItemIndex = index;
				}
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove (e);

			if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
			{
				if (_dragItemIndex > 0 && _dragItemIndex < this.Items.Count-1)
				{
					int index = this.IndexFromPoint(e.X, e.Y);
					if (index != _dragItemIndex)
					{
						ListBoxItem item = this.Items[_dragItemIndex] as ListBoxItem;
						this.DoDragDrop(item.StepInfo, DragDropEffects.Move);
						_dragItemIndex = -1;
					}
				}
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp (e);
		}


		protected override void OnDoubleClick(EventArgs e)
		{
			base.OnDoubleClick (e);

			this.ShowStepSettings();
		}


		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown (e);

			if (e.KeyCode == Keys.Delete)
				this.OnRemoveSelectedSteps();
		}



		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			base.OnDrawItem (e);

			ListBoxItem item = this.Items[e.Index] as ListBoxItem;
			if (item == null)
				return;

			ProcessStepInfo stepInfo = item.StepInfo;
			
			Graphics graph = e.Graphics;
			SmoothingMode smoothingMode = graph.SmoothingMode;
			graph.SmoothingMode = SmoothingMode.HighQuality;
			graph.InterpolationMode = InterpolationMode.Bicubic;

			Rectangle boundRect = e.Bounds;
			MeasureItemEventArgs args = new MeasureItemEventArgs(e.Graphics, e.Index);
			this.OnMeasureItem(args);
            			
			float width = args.ItemWidth - itemMargins[0] - itemMargins[2];
			float height = args.ItemHeight - itemMargins[1] - itemMargins[3];
			float x = boundRect.X + (boundRect.Width - width)*0.5F;
			float y = boundRect.Y + (boundRect.Height - height)*0.5F;
			float radius = 0.2F * Math.Min(width, height);

			RectangleF itemRect = new RectangleF(x, y, width, height);

			if (_isDropping && e.Index == _dropItemIndex)
			{
				PointF pt = new PointF(itemRect.X + itemRect.Width*0.5F, boundRect.Y);
				PointF pt1 = new PointF(pt.X-10, pt.Y-10);
				PointF pt2 = new PointF(pt.X+10, pt.Y+10);
				PointF pt3 = new PointF(pt.X-10, pt.Y+10);
				PointF pt4 = new PointF(pt.X+10, pt.Y-10);

				using (Pen pen = new Pen(Color.Yellow, 2.0F))
				{
					graph.DrawLine(pen, pt1, pt2);
					graph.DrawLine(pen, pt3, pt4);
				}
			}

			graph.SmoothingMode = smoothingMode;
		}

		#endregion

		#region Virtual Methods

		protected virtual void OnMoveProcessStep(ListBoxItem lbItem, int insertIndex, int removeIndex)
		{
			this.BeginUpdate();

			// clear selection
			this.ClearSelected();

			if (removeIndex > insertIndex)
			{
				this.Items.RemoveAt(removeIndex);
				this.Items.Insert(insertIndex, lbItem);
				this.SelectedIndex = insertIndex;
			}
			else if (removeIndex < insertIndex)
			{
				this.Items.Insert(insertIndex, lbItem);
				this.SelectedIndex = insertIndex;
				this.Items.RemoveAt(removeIndex);
			}

			this.EndUpdate();

			if (this.SelectedProcessStepMoved != null)
				this.SelectedProcessStepMoved(this, EventArgs.Empty);
		}

		protected virtual void OnInsertProcessStep(int index, ProcessStepInfo stepInfo)
		{
			this.BeginUpdate();

			// try to create process step
			ProcessStep step = Activator.CreateInstance(stepInfo.Type) as ProcessStep;
			ListBoxItem item = new ListBoxItem(stepInfo, step);
			this.Items.Insert(index, item);
			
			// clear selection
			this.ClearSelected();

			// select added step
			this.SelectedIndex = index;

			this.EndUpdate();

			if (this.ProcessStepInserted != null)
				this.ProcessStepInserted(this, EventArgs.Empty);
		}

		protected virtual void OnRemoveSelectedSteps()
		{
			this.BeginUpdate();

			while (this.SelectedItems.Count > 0)
			{
				int index = this.Items.IndexOf(this.SelectedItems[0]);
				if (index > 0 && index < this.Items.Count-1)
					this.Items.Remove(this.SelectedItems[0]);
				else
					this.SetSelected(index, false);
			}

			this.EndUpdate();

			if (this.SelectedProcessStepsRemoved != null)
				this.SelectedProcessStepsRemoved(this, EventArgs.Empty);
		}


		#endregion
	}
}
