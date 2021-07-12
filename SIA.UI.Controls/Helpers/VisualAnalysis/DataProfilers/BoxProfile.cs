using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Threading;
using System.Text;

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
	/// Summary description for BoxProfile.
	/// </summary>
	public class BoxProfile : BaseDataProfiler
	{
		private enum InteractiveMode
		{
			Normal,
			Move,
		}
			
		private InteractiveMode _interactiveMode = InteractiveMode.Normal;
		private BoxProfileSettings _settings = new BoxProfileSettings();

		public BoxProfileSettings Settings
		{
			get {return _settings;}
			set {_settings = value;}
		}


		public bool IsInteractiveModeBusy
		{
			get {return this.Container.IsInteractiveModeBusy;}
			set {this.Container.IsInteractiveModeBusy = value;}
		}

		public RectangleF Rectangle
		{
			get
			{
				float left = Math.Min(_begin.X, _end.X);
				float top = Math.Min(_begin.Y, _end.Y);
				float right = Math.Max(_begin.X, _end.X);
				float bottom = Math.Max(_begin.Y, _end.Y);

				return new RectangleF(left, top, right-left+1, bottom-top+1);
			}
		}

		public BoxProfile(DataProfileHelper container)
			: base(container)
		{
		}

		#region IDataProfiler Members

		public override void Render(Graphics graph, Rectangle rcClip)
		{
			ImageWorkspace workspace = this.Workspace;
			ImageViewer viewer = workspace.ImageViewer;
			Transformer transformer = new Transformer(viewer.Transform);

			RectangleF rect = this.Rectangle;

			if (rect != RectangleF.Empty)
			{
				rect = transformer.RectangleToPhysical(rect);
				using (Pen pen = new Pen(Color.Red, 1.0F))
				{
					pen.Alignment = PenAlignment.Center;
					graph.DrawRectangle(pen, rect.Left, rect.Top, rect.Width, rect.Height);
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
				else if (htInfo.Status == DataProfileHelper.HitTestStatus.Inside)
					this.Cursor = Cursors.SizeAll;
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
			PointF[] pts = new PointF[2];

			if (e.Cancel)
			{
				//_begin = PointF.Empty;
				//_end = PointF.Empty;

				// refresh drawing
				this.Workspace.Invalidate(true);
				return;
			}
			
			if (this.IsInteractiveModeBusy == false)
			{
				DataProfileHelper.HitTestInfo htInfo = this.HitTest(e.BeginF);
				if (htInfo.Status == DataProfileHelper.HitTestStatus.Outside)
					this._interactiveMode = InteractiveMode.Normal;
				else if (htInfo.Status == DataProfileHelper.HitTestStatus.Inside)
					this._interactiveMode = InteractiveMode.Move;
				else if (htInfo.Status == DataProfileHelper.HitTestStatus.Edge)
					this._interactiveMode = InteractiveMode.Move;
			}
			

			if (this._interactiveMode == InteractiveMode.Normal)
			{
				switch (e.Status)
				{
					case RasterViewerInteractiveStatus.Begin:
						this.IsInteractiveModeBusy = true;
                        this.Cursor = LocalResources.Cursors.DrawCircle;

						pts[0] = e.BeginF;
						pts[1] = e.BeginF;
						pts = transformer.PointToPhysical(pts);
						workspace.DrawHelper.DrawXorRectangle(this.RectangleFromPoints(pts[0], pts[1]));
						_lastPoints = pts;

						break;

					case RasterViewerInteractiveStatus.End:
						// release interactive mode busy
						this.IsInteractiveModeBusy = false;

						this._begin = e.BeginF;
						this._end = e.EndF;

						// clear temporary points
						_lastPoints = null;

						// update data profile
						this.UpdateDataProfile(_begin, _end);

						// refresh drawing
						workspace.ImageViewer.Invalidate(true);
						break;
					case RasterViewerInteractiveStatus.Working:	
						if (_lastPoints != null)
							workspace.DrawHelper.DrawXorRectangle(this.RectangleFromPoints(_lastPoints[0], _lastPoints[1]));

						pts[0] = e.BeginF;
						pts[1] = e.EndF;
						
						// update workspace
						PointF[] pts2 = transformer.PointToPhysical(pts);
						workspace.DrawHelper.DrawXorRectangle(this.RectangleFromPoints(pts2[0], pts2[1]));
						_lastPoints = pts2;
						
						// update data profile
						this.UpdateDataProfile(pts[0], pts[1]);

						break;
				}
			}
			else if (this._interactiveMode == InteractiveMode.Move)
			{
				float dx=0,dy=0;
				
				switch (e.Status)
				{
					case RasterViewerInteractiveStatus.Begin:
						this.IsInteractiveModeBusy = true;
						this.Cursor = Cursors.SizeAll;
						
						pts[0] = _begin;
						pts[1] = _end;
						pts = transformer.PointToPhysical(pts);
						workspace.DrawHelper.DrawXorRectangle(this.RectangleFromPoints(pts[0], pts[1]));
						
						// update temporary points
						_lastPoints = pts;
						break;

					case RasterViewerInteractiveStatus.End:
						// release interactive mode busy
						this.IsInteractiveModeBusy = false;
						
						dx = e.EndF.X - e.BeginF.X;
						dy = e.EndF.Y - e.BeginF.Y;

						_begin = new PointF(_begin.X + dx, _begin.Y + dy);
						_end = new PointF(_end.X + dx, _end.Y + dy);

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
							workspace.DrawHelper.DrawXorRectangle(this.RectangleFromPoints(_lastPoints[0], _lastPoints[1]));

						dx = e.EndF.X - e.BeginF.X;
						dy = e.EndF.Y - e.BeginF.Y;

						pts[0] = new PointF(_begin.X + dx, _begin.Y + dy);
						pts[1] = new PointF(_end.X + dx, _end.Y + dy);

						// update workspace
						PointF[] pts2 = transformer.PointToPhysical(pts);
						workspace.DrawHelper.DrawXorRectangle(this.RectangleFromPoints(pts2[0], pts2[1]));
						_lastPoints = pts2;

						// update data profile
						this.UpdateDataProfile(pts[0], pts[1]);

						break;
				}
			}
		}

		public override void UpdateSelectedValue(object abscissaValue, object ordinaryValue)
		{
			
		}

		public override void DisplaySettingsWindow()
		{
		}

		public override void Export()
		{
			using (SaveFileDialog dlg = CommonDialogs.SaveCsvFileDialog("Export data as..."))
			{
				if (DialogResult.OK == dlg.ShowDialog(this.Container.DlgLineProfile))
				{
					try
					{
						this.Export(dlg.FileName);
						MessageBoxEx.Info("Data was exported successfully.");
					}
					catch (Exception exp)
					{
						Trace.WriteLine(exp);
						MessageBoxEx.Error("Failed to export data: " + exp.Message);
					}
					
				}
			}
		}

		public override void Update()
		{
			this.UpdateDataProfile(this._begin, this._end);
		}

		#endregion

		public DataProfileHelper.HitTestInfo HitTest(PointF pt)
		{
			ImageWorkspace workspace = this.Workspace;
			ImageViewer viewer = workspace.ImageViewer;
			Transformer transformer = new Transformer(viewer.Transform);

			float penWidth = 10;
			RectangleF rect = this.Rectangle;
			rect = transformer.RectangleToPhysical(rect);
			pt = transformer.PointToPhysical(pt);
			
			if (rect == RectangleF.Empty)
				return new DataProfileHelper.HitTestInfo(DataProfileHelper.HitTestStatus.Outside, pt);
			
			using (Pen pen = new Pen(Color.Red, penWidth))
			{
				pen.Alignment = PenAlignment.Center;

				using (GraphicsPath path = new GraphicsPath())
				{
					path.AddRectangle(rect);
					if (path.IsOutlineVisible(pt, pen))
						return new DataProfileHelper.HitTestInfo(DataProfileHelper.HitTestStatus.Edge, pt);
					else if (path.IsVisible(pt))
						return new DataProfileHelper.HitTestInfo(DataProfileHelper.HitTestStatus.Inside, pt);
					else
						return new DataProfileHelper.HitTestInfo(DataProfileHelper.HitTestStatus.Outside, pt);
				}
			}
		}

		protected virtual void UpdateDataProfile(PointF begin, PointF end)
		{			
		}

		protected virtual void SamplingData(BoxProfileOptions options, RectangleF rect, ref float[] data, ref float[] categories)
		{
		}

		protected virtual void Export(string filePath)
		{
			using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
			{
				float[] data = null, categories = null;
				float left = Math.Min(_begin.X, _end.X);
				float top = Math.Min(_begin.Y, _end.Y);
				float right = Math.Max(_begin.X, _end.X);
				float bottom = Math.Max(_begin.Y, _end.Y);

				RectangleF rect = new RectangleF(left, top, right-left+1, bottom-top+1);
				BoxProfileOptions options = this.DlgLineProfile.BoxProfileOptions;

				this.SamplingData(options, rect, ref data, ref categories);

				if (data != null && categories != null)
				{
					for (int i=0; i<data.Length; i++)
					{
						if (i==0)
							writer.WriteLine("Index,Intensity");
						writer.WriteLine(String.Format("{0},{1}", categories[i], data[i]));
					}
				}
			}
		}

		private RectangleF RectangleFromPoints(PointF pt1, PointF pt2)
		{
			float left = Math.Min(pt1.X, pt2.X);
			float top = Math.Min(pt1.Y, pt2.Y);
			float right = Math.Max(pt1.X, pt2.X);
			float bottom = Math.Max(pt1.Y, pt2.Y);

			return new RectangleF(left, top, right-left+1, bottom-top+1);
		}
	}
}
