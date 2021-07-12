using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Threading;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using SIA.Common;
using SIA.Common.Analysis;
using SIA.SystemLayer;

using SIA.UI.Controls;
using SIA.UI.Controls.Common;
//CONG using SIA.UI.Controls.Components;
using SIA.UI.Controls.UserControls;
using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.Helpers;
using SIA.UI.Controls.Helpers.VisualAnalysis;
using SIA.UI.Controls.Utilities;

using SIA.UI.Components;
using SIA.UI.Components.Helpers;


namespace SIA.UI.Controls.Helpers.VisualAnalysis.DataProfilers
{
	/// <summary>
	/// Summary description for VerticalLineProfile.
	/// </summary>
	public class VerticalLineProfile : LineProfile
	{
		private enum InteractiveMode
		{
			Normal,
			Move,
		}
			
		private InteractiveMode _interactiveMode = InteractiveMode.Normal;
		
		public VerticalLineProfile(DataProfileHelper container)
			: base(container)
		{
		}

		#region IDataProfiler Members

		public override void Render(Graphics graph, Rectangle rcClip)
		{
			ImageWorkspace workspace = this.Workspace;
			ImageViewer viewer = workspace.ImageViewer;
			Transformer transformer = new Transformer(viewer.Transform);

			if (_begin != PointF.Empty || _end != PointF.Empty)
			{
				PointF[] pts = new PointF[] {_begin, _end};
				pts = transformer.PointToPhysical(pts);
				using (Pen pen = new Pen(Color.Red, 1.0F))
				{
					pen.Alignment = PenAlignment.Center;
					graph.DrawLine(pen, pts[0], pts[1]);
				}

				if (_selected != PointF.Empty)
				{
					float size = transformer.LengthToPhysical(1.0F);
					PointF pt = transformer.PointToPhysical(_selected);
					size = Math.Max(4.0F, size);
					float x = (int)Math.Floor(pt.X), y = (int)Math.Floor(pt.Y);
					graph.FillRectangle(Brushes.Aqua, x, y, size, size);
				}
			}
		}

		public override void MouseDown(MouseEventArgs e)
		{
		}

		public override void MouseMove(MouseEventArgs e)
		{
			ImageWorkspace workspace = this.Workspace;
			ImageViewer viewer = workspace.ImageViewer;
			Transformer transformer = new Transformer(viewer.Transform);

			if (this.IsInteractiveModeBusy == false)
			{
				PointF pt = new PointF(e.X, e.Y);
				if (e is MouseEventArgsEx)
					pt = ((MouseEventArgsEx)e).PointF;
				DataProfileHelper.HitTestInfo htInfo = this.HitTest(pt);
				if (htInfo.Status == DataProfileHelper.HitTestStatus.Outside)
					this.Cursor = Cursors.Default;
				else if (htInfo.Status == DataProfileHelper.HitTestStatus.Edge)
					this.Cursor = LocalResources.Cursors.DrawCircle;
			}
		}

		public override void MouseUp(MouseEventArgs e)
		{
		}

		public override void InteractiveLine(RasterViewerLineEventArgs e)
		{
			ImageWorkspace workspace = this.Workspace;
			ImageViewer viewer = workspace.ImageViewer;
			Transformer transformer = new Transformer(viewer.Transform);
			CommonImage image = this.Image;

			if (e.Cancel)
			{
				_begin = PointF.Empty;
				_end = PointF.Empty;

				// refresh drawing
				this.Workspace.Invalidate(true);
				return;
			}

			if (this.IsInteractiveModeBusy == false)
			{
				DataProfileHelper.HitTestInfo htInfo = this.HitTest(e.BeginF);
				if (htInfo.Status == DataProfileHelper.HitTestStatus.Outside)
					this._interactiveMode = InteractiveMode.Normal;
				else if (htInfo.Status == DataProfileHelper.HitTestStatus.Edge)
					this._interactiveMode = InteractiveMode.Move;
			}
			

			if (this._interactiveMode == InteractiveMode.Normal)
			{
				float x = 0;
				PointF[] pts = new PointF[2];

				switch (e.Status)
				{
					case RasterViewerInteractiveStatus.Begin:

						if (e.BeginF.X >= 0 && e.BeginF.X < image.Width)
						{
							this.IsInteractiveModeBusy = true;
							this.Cursor = LocalResources.Cursors.DrawCircle;

							pts[0] = new PointF(e.BeginF.X, 0);
							pts[1] = new PointF(e.BeginF.X, image.Height);
							pts = transformer.PointToPhysical(pts);
							workspace.DrawHelper.DrawXorLine(pts[0], pts[1]);
							_lastPoints = pts;
						}

						break;

					case RasterViewerInteractiveStatus.End:
						// release interactive mode busy
						this.IsInteractiveModeBusy = false;

						x =  Math.Min(image.Width-1, Math.Max(0, e.EndF.X));
						this._begin = new PointF(x, 0);
						this._end = new PointF(x, image.Height);

						// clear temporary points
						_lastPoints = null;

						// update data profile
						this.UpdateDataProfile(_begin, _end);

						// refresh drawing
						workspace.ImageViewer.Invalidate(true);
						break;
					case RasterViewerInteractiveStatus.Working:	

						x =  Math.Min(image.Width-1, Math.Max(0, e.EndF.X));

						if (_lastPoints != null)
							workspace.DrawHelper.DrawXorLine(_lastPoints[0], _lastPoints[1]);

						pts[0] = new PointF(x, 0);
						pts[1] = new PointF(x, image.Height);
						PointF[] pts2 = transformer.PointToPhysical(pts);
						workspace.DrawHelper.DrawXorLine(pts2[0], pts2[1]);
						_lastPoints = pts2;

						// update data profile
						this.UpdateDataProfile(pts[0], pts[1]);

						break;
				}
			}
			else if (this._interactiveMode == InteractiveMode.Move)
			{
				float x=0;
				float dx=0;
				PointF[] pts = new PointF[2];

				switch (e.Status)
				{
					case RasterViewerInteractiveStatus.Begin:
						if (e.BeginF.X >= 0 && e.BeginF.X < image.Width)
						{
							this.IsInteractiveModeBusy = true;
							this.Cursor = LocalResources.Cursors.DrawCircle;
							
							pts = new PointF[] { _begin, _end};
							pts = transformer.PointToPhysical(pts);
							workspace.DrawHelper.DrawXorLine(pts[0], pts[1]);
							_lastPoints = pts;
						}
						break;

					case RasterViewerInteractiveStatus.End:
						// release interactive mode busy
						this.IsInteractiveModeBusy = false;
						
						x =  Math.Min(image.Width-1, Math.Max(0, e.EndF.X));
						dx = x - e.BeginF.X;
						this._begin.X = _begin.X + dx;
						this._end.X = _end.X + dx;

						_lastPoints = null;

						// update data profile
						this.UpdateDataProfile(_begin, _end);

						// update container mode
						this._interactiveMode = InteractiveMode.Normal;

						this.Cursor = Cursors.Default;

						// refresh drawing
						workspace.ImageViewer.Invalidate(true);
						break; 
					case RasterViewerInteractiveStatus.Working:	
						if (_lastPoints != null)
							workspace.DrawHelper.DrawXorLine(_lastPoints[0], _lastPoints[1]);

						x =  Math.Min(image.Width-1, Math.Max(0, e.EndF.X));
						dx = x - e.BeginF.X;

						pts = new PointF[2];
						pts[0] = new PointF(_begin.X + dx, _begin.Y);
						pts[1] = new PointF(_end.X + dx, _end.Y);
						PointF[] pts2 = transformer.PointToPhysical(pts);
						workspace.DrawHelper.DrawXorLine(pts2[0], pts2[1]);                        
						_lastPoints = pts2;

						// update data profile
						this.UpdateDataProfile(pts[0], pts[1]);

						break;
				}
			}
		}

		public override void UpdateSelectedValue(object abscissaValue, object ordinaryValue)
		{
            try
            {
                this._selected = (Point)abscissaValue;
                this.Workspace.Invalidate(true);
            }
            catch
            {
            }
		}

		#endregion

		public DataProfileHelper.HitTestInfo HitTest(PointF pt)
		{
			ImageWorkspace workspace = this.Workspace;
			ImageViewer viewer = workspace.ImageViewer;
			Transformer transformer = new Transformer(viewer.Transform);

			PointF[] pts = new PointF[] {_begin, _end, pt};
			pts = transformer.PointToPhysical(pts);
			float penWidth = 10;
			
			if (_begin == PointF.Empty && _end == PointF.Empty)
				return new DataProfileHelper.HitTestInfo(DataProfileHelper.HitTestStatus.Outside, pt);
			
			using (Pen pen = new Pen(Color.Red, penWidth))
			{
				pen.Alignment = PenAlignment.Center;

				using (GraphicsPath path = new GraphicsPath())
				{
					path.AddLine(pts[0], pts[1]);
					if (path.IsOutlineVisible(pts[2], pen))
						return new DataProfileHelper.HitTestInfo(DataProfileHelper.HitTestStatus.Edge, pt);
					else
						return new DataProfileHelper.HitTestInfo(DataProfileHelper.HitTestStatus.Outside, pt);
				}
			}
		}

		public override void DisplaySettingsWindow()
		{
			using (DlgLineProfileSettings dlg = new DlgLineProfileSettings(this.Settings))
			{
				if (DialogResult.OK == dlg.ShowDialog(this.Container.DlgLineProfile))
				{
					// update internal settings
					this.Settings = dlg.Settings;
					// update profile data
					this.UpdateDataProfile(this._begin, this._end);
				}
			}
		}

		protected override void UpdateDataProfile(PointF begin, PointF end)
		{
			if (begin == PointF.Empty && end == PointF.Empty)
			{
			}
			else
			{
				float[] data = new float[10];
				Point[] categories = new Point[10];
				
				// sampling data
				this.SamplingData(begin, end, ref data, ref categories);

				// update plot
                this.DlgLineProfile.UpdateLinePlot(this.Settings, begin, end, data, categories);
			}
		}

		protected override void SamplingData(PointF ptBegin, PointF ptEnd, ref float[] data, ref Point[] categories)
		{
			CommonImage image = this.Image;
			int x1 = (int)Math.Floor(ptBegin.X), y1 = (int)Math.Floor(ptBegin.Y);
			int x2 = (int)Math.Floor(ptEnd.X), y2 = (int)Math.Floor(ptEnd.Y);

			Point[] pts = CommonImage.GetLinePoints(new Point(x1, y1), new Point(x2, y2));
			data = new float[pts.Length];
			categories = new Point[pts.Length];
			for (int i=0; i<pts.Length; i++)
			{
				int x = pts[i].X, y = pts[i].Y;
				float value = 0;
				if (x>=0 && x<image.Width && y>=0 && y<image.Height)
					value = image.GetPixel(x, y);
				categories[i] = pts[i];
				data[i] = value;
			}

			if (this.Settings != null)
				this.Settings.UpdateDataBySettings(data);
		}

	}
}
