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
	/// Summary description for ScriptExecution.
	/// </summary>
    internal class ScriptExecution : ScriptViewer
	{
		#region Fields

		private string _scriptFilePath = string.Empty;
		private string _filePath = string.Empty;
		
		private int _processingItemIndex = -1;
		private string _status = null;
		private int _percentage = 0;

		#endregion

		#region Constructor and destructor

		public ScriptExecution()
		{
            //itemHeight = 40;
		}

		#endregion

		#region Methods

		public void BeginExecution(string scriptFilePath)
		{
			_scriptFilePath = scriptFilePath;
			_processingItemIndex = -1;
			_percentage = 0;
		}

		public void EndExecution(string scriptFilePath)
		{
			_scriptFilePath = null;
			_processingItemIndex = -1;
			_percentage = 0;
			this.Invalidate(true);
		}

		public void BeginProcessFile(string filePath)
		{
			_filePath = filePath;
			_processingItemIndex = -1;
			_percentage = 0;
		}

		public void EndProcessFile(string filePath)
		{
			_filePath = null;
			_processingItemIndex = -1;
			_percentage = 0;
			this.Invalidate(true);
		}

		public void UpdateProcessStepStatus(int index, string status, int percentage, 
            DateTime startTime, DateTime endTime)
		{
			if (index >= 0)
				_processingItemIndex = index+1;

			if (_processingItemIndex>0 && _processingItemIndex<this.Items.Count-1)
			{
				int lbIndex = _processingItemIndex;
				ListBoxItem item = this.Items[lbIndex] as ListBoxItem;
                
                // update item runtime properties
                item.StartTime = startTime;
                item.EndTime = endTime;
                
				_status = status;
				_percentage = percentage;

				// scroll to processing item
				this.SelectedIndex = _processingItemIndex; 
			}

			this.Invalidate(true);
		}


		#endregion

		#region Override routines

		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			if (e.Index == _processingItemIndex)
				DrawProcessingStep(e);
            else if (e.Index < _processingItemIndex)
                DrawProcessedStep(e);
            else
            {
                ListBoxItem item = this.Items[e.Index] as ListBoxItem;
                if (item != null && item.Duration.TotalMilliseconds > 0)
                    DrawProcessedStep(e);
			    else 
                    DrawUnprocessStep(e);
            }
		}

		protected virtual void DrawProcessingStep(DrawItemEventArgs e)
		{
            ListBoxItem item = this.Items[e.Index] as ListBoxItem;
            if (item == null)
                return;

            drawText = false;

			base.DrawProcessStep(e);

			Graphics graph = e.Graphics;
			SmoothingMode smoothingMode = graph.SmoothingMode;
			graph.SmoothingMode = SmoothingMode.HighQuality;
			graph.InterpolationMode = InterpolationMode.Bicubic;

			try
			{
				Rectangle boundRect = e.Bounds;
				MeasureItemEventArgs args = new MeasureItemEventArgs(e.Graphics, e.Index);
				this.OnMeasureItem(args);
            			
				float width = args.ItemWidth - itemMargins[0] - itemMargins[2];
				float height = args.ItemHeight - itemMargins[1] - itemMargins[3];
				float x = boundRect.X + (boundRect.Width - width)*0.5F;
				float y = boundRect.Y + (boundRect.Height - height)*0.5F;
				float radius = 0.2F * Math.Min(width, height);
				RectangleF itemRect = new RectangleF(x, y, width, height);

				RectangleF statusRect = RectangleF.FromLTRB(itemRect.Left+radius, itemRect.Bottom-radius, itemRect.Right-radius, itemRect.Bottom);
				//graph.DrawRectangle(Pens.Black, statusRect.X, statusRect.Y, statusRect.Width, statusRect.Height);

                // draw process step image
				Image image = Resource.ProcessImage;
				x = statusRect.X;
				y = statusRect.Y +  + (statusRect.Height-image.Height)*0.5F;
				graph.DrawImage(image, x, y);
			
                // draw progress bar
				RectangleF progressBarRect = 
                    RectangleF.FromLTRB(
                    statusRect.Left + image.Width, statusRect.Top, 
                    statusRect.Right, statusRect.Bottom);
				progressBarRect.Inflate(-2*itemPadding, -2*itemPadding);
                progressBarRect = textRect;
				DrawProgressBar(graph, progressBarRect, _percentage/100.0F);

                // update input link
				Color oldColor = inputLinkPen.Color;
				inputLinkPen.Color = Color.FromArgb(0xFF, 0, 227, 0);
				if (e.Index > 0)
					this.DrawLink(graph, boundRect, itemRect, true);
				inputLinkPen.Color = oldColor;
			}
			catch (Exception exp)
			{
				Trace.WriteLine(exp);
			}
			finally
			{
				graph.SmoothingMode = smoothingMode;

                drawText = true;
			}
		}
		
		protected virtual void DrawProcessedStep(DrawItemEventArgs e)
		{
			base.DrawProcessStep(e);

			Graphics graph = e.Graphics;
			SmoothingMode smoothingMode = graph.SmoothingMode;
			graph.SmoothingMode = SmoothingMode.HighQuality;
			graph.InterpolationMode = InterpolationMode.Bicubic;

			try
			{
				Rectangle boundRect = e.Bounds;
				MeasureItemEventArgs args = new MeasureItemEventArgs(e.Graphics, e.Index);
				this.OnMeasureItem(args);
            			
				float width = args.ItemWidth - itemMargins[0] - itemMargins[2];
				float height = args.ItemHeight - itemMargins[1] - itemMargins[3];
				float x = boundRect.X + (boundRect.Width - width)*0.5F;
				float y = boundRect.Y + (boundRect.Height - height)*0.5F;
				float radius = 0.2F * Math.Min(width, height);
				RectangleF itemRect = new RectangleF(x, y, width, height);

				if (e.Index > 0 && e.Index < this.Items.Count-1)
				{
					//RectangleF statusRect = RectangleF.FromLTRB(itemRect.Left+radius, itemRect.Bottom-radius, itemRect.Right-radius, itemRect.Bottom);
					////graph.DrawRectangle(Pens.Black, statusRect.X, statusRect.Y, statusRect.Width, statusRect.Height);
					//
					//Image image = Resource.ProcessImage;
					//x = statusRect.X;
					//y = statusRect.Y +  + (statusRect.Height-image.Height)*0.5F;
					//graph.DrawImage(image, x, y);
					//			
					//RectangleF progressBarRect = RectangleF.FromLTRB(statusRect.Left + image.Width, statusRect.Top, statusRect.Right, statusRect.Bottom);
					//progressBarRect.Inflate(-2*itemPadding, -2*itemPadding);
					//DrawProgressBar(graph, progressBarRect, 1);
				}

                // draw duration
                ListBoxItem item = this.Items[e.Index] as ListBoxItem;
                string duration = item.Duration.TotalSeconds.ToString("0.##") + " s";
                using (Font font = new Font(nodeFont, FontStyle.Regular))
                {
                    SizeF szText = graph.MeasureString(duration, font);
                    using (Brush textBrush = new SolidBrush(ControlPaint.Dark(Color.DarkGray)))
                        graph.DrawString(duration, font, textBrush, itemRect.Right - radius - szText.Width, itemRect.Top + radius, StringFormat.GenericDefault);
                }


				// update input link
				Color oldColor = inputLinkPen.Color;
				inputLinkPen.Color = Color.FromArgb(0xFF, 0, 227, 0);
				if (e.Index > 0)
					this.DrawLink(graph, boundRect, itemRect, true);
				inputLinkPen.Color = oldColor;

				// update output link
				oldColor = outputLinkPen.Color;
				outputLinkPen.Color = Color.FromArgb(0xFF, 0, 227, 0);
				if (e.Index < this.Items.Count-1)
					this.DrawLink(graph, boundRect, itemRect, false);
				outputLinkPen.Color = oldColor;

			}
			catch (Exception exp)
			{
				Trace.WriteLine(exp);
			}
			finally
			{
				graph.SmoothingMode = smoothingMode;
			}
		}

		protected virtual void DrawUnprocessStep(DrawItemEventArgs e)
		{
			base.DrawProcessStep(e);
		}

		protected virtual void DrawProgressBar(Graphics graph, RectangleF rect, float percentage)
		{
			Color clrFinish = Color.FromArgb(0xFF, 0, 227, 0);
			RectangleF finishRect = rect;
			finishRect.Width = rect.Width*percentage;
			using (GraphicsPath path = this.GetConePath(finishRect))
			{
				using (Brush brush = new SolidBrush(clrFinish))
					graph.FillPath(brush, path);
			}

			using (GraphicsPath path = this.GetConePath(rect))
			{
				RectangleF boundRect = rect;
				boundRect.Inflate(4, 4);
				using (LinearGradientBrush brush = new LinearGradientBrush(boundRect, Color.DarkGray, Color.White, 90))
				{
					using (Pen pen = new Pen(brush, 1.5F))
					{
						graph.DrawPath(pen, path);
					}
				}
			}
		}

		protected GraphicsPath GetConePath(RectangleF rect)
		{
			float radius = rect.Height*0.5F;

			GraphicsPath path = new GraphicsPath();
            try
            {
                path.AddLine(rect.X + radius, rect.Y, rect.Right - radius, rect.Y);
                path.AddArc(rect.Right - radius, rect.Y, 2 * radius, 2 * radius, -90, 180);
                path.AddLine(rect.Right - radius, rect.Y + 2 * radius, rect.Left + radius, rect.Y + 2 * radius);
                path.AddArc(rect.X, rect.Y, 2 * radius, 2 * radius, 90, 180);
                //path.CloseFigure();
            }
            catch
            { }
            finally
            {
                if (path != null)
                {
                    path.CloseFigure();
                }
            }

			return path;
		}

		#endregion
	}
}
