using System;
using System.Text;
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
	/// Summary description for LineProfile.
	/// </summary>
	public class LineProfile : BaseDataProfiler
	{
		private enum InteractiveMode
		{
			Normal,
			Move,
			AdjustBeginPoint,
			AdjustEndPoint,
		}
			
		private LineProfileSettings _settings = new LineProfileSettings();
		private InteractiveMode _interactiveMode = InteractiveMode.Normal;
		private PointF[] _interactiveLine = new PointF[2];
		
		private Thread _workerThread = null;
		private object _syncObject = new object();
		private PointF _pt1 = PointF.Empty, _pt2 = PointF.Empty;
		private ManualResetEvent _waitThread = new ManualResetEvent(false);
		
		public bool IsInteractiveModeBusy
		{
			get {return this.Container.IsInteractiveModeBusy;}
			set {this.Container.IsInteractiveModeBusy = value;}
		}

		public LineProfileSettings Settings
		{
			get {return _settings;}
			set {_settings = value;}
		}

		
		#region Constructor and destructor

		public LineProfile(DataProfileHelper container)
			: base(container)
		{
		}


		#endregion
		
		#region IDataProfiler Members

		public override void Render(Graphics graph, Rectangle rcClip)
		{
			ImageWorkspace workspace = this.Workspace;
			ImageViewer viewer = workspace.ImageViewer;
			Transformer transformer = new Transformer(viewer.Transform);

			if (_begin != PointF.Empty && _end != PointF.Empty)
			{
				PointF[] pts = new PointF[] {_begin, _end};
				pts = transformer.PointToPhysical(pts);
				using (Pen pen = new Pen(Color.Red, 1.0F))
				{
					pen.Alignment = PenAlignment.Center;
					graph.DrawLine(pen, pts[0], pts[1]);
					float x=0,y=0,size=4;
					x = pts[0].X - size*0.5F;
					y = pts[0].Y - size*0.5F;
					graph.FillRectangle(Brushes.Red, x, y, size, size);

					x = pts[1].X - size*0.5F;
					y = pts[1].Y - size*0.5F;
					graph.FillRectangle(Brushes.Red, x, y, size, size);
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
				else if (htInfo.Status == DataProfileHelper.HitTestStatus.Begin)
					this.Cursor = Cursors.SizeAll;
				else if (htInfo.Status == DataProfileHelper.HitTestStatus.End)
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
			PointF[] pts = new PointF[2];

			if (e.Cancel)
			{
				_begin = PointF.Empty;
				_end = PointF.Empty;

				// refresh drawing
				workspace.Invalidate(true);
				return;
			}
			
			if (this.IsInteractiveModeBusy == false)
			{
				DataProfileHelper.HitTestInfo htInfo = this.HitTest(e.BeginF);
				if (htInfo.Status == DataProfileHelper.HitTestStatus.Outside)
					this._interactiveMode = InteractiveMode.Normal;
				else if (htInfo.Status == DataProfileHelper.HitTestStatus.Begin)
					this._interactiveMode = InteractiveMode.AdjustBeginPoint;
				else if (htInfo.Status == DataProfileHelper.HitTestStatus.End)
					this._interactiveMode = InteractiveMode.AdjustEndPoint;
				else if (htInfo.Status == DataProfileHelper.HitTestStatus.Edge)
					this._interactiveMode = InteractiveMode.Move;
			}			 

			// clear interactive point
			_selected = PointF.Empty;

			if (this._interactiveMode == InteractiveMode.Normal)
			{
				switch (e.Status)
				{
					case RasterViewerInteractiveStatus.Begin:
						// set interactive mode busy
						this.IsInteractiveModeBusy = true;
						pts[0] = e.BeginF;
						pts[1] = e.BeginF;
						pts = transformer.PointToPhysical(pts);
						workspace.DrawHelper.DrawXorLine(pts[0], pts[1]);
						_lastPoints = pts;
						this.Cursor = LocalResources.Cursors.DrawCircle;
						break;

					case RasterViewerInteractiveStatus.End:
						this.IsInteractiveModeBusy = false;

						this._begin = e.BeginF;
						this._end = e.EndF;

						// update point by keyboard state
						this.UpdatePointByKeyboardState(ref _begin, ref _end);

						// update data profile
						this.UpdateDataProfile(_begin, _end);

						// clear temporary points
						_lastPoints = null;

						// refresh drawing
						workspace.ImageViewer.Invalidate(true);
						break;
					case RasterViewerInteractiveStatus.Working:	
						if (_lastPoints != null)
							workspace.DrawHelper.DrawXorLine(_lastPoints[0], _lastPoints[1]);

						pts[0] = e.BeginF;
						pts[1] = e.EndF;

						// update point by keyboard state
						this.UpdatePointByKeyboardState(ref pts);

						// update temporary data profile
						this.UpdateDataProfile(pts[0], pts[1]);

						// render temporary points
						pts = transformer.PointToPhysical(pts);
						workspace.DrawHelper.DrawXorLine(pts[0], pts[1]);

						// update temporary points
						_lastPoints = pts;
						break;
				}
			}
			else if (this._interactiveMode == InteractiveMode.Move)
			{
				float dx=0, dy=0;

				switch (e.Status)
				{
					case RasterViewerInteractiveStatus.Begin:
						// set interactive mode busy
						this.IsInteractiveModeBusy = true;
						pts[0] = _begin;
						pts[1] = _end;
						pts = transformer.PointToPhysical(pts);
						workspace.DrawHelper.DrawXorLine(pts[0], pts[1]);
						// update temporary points
						_lastPoints = pts;
						this.Cursor = LocalResources.Cursors.DrawCircle;
						break;

					case RasterViewerInteractiveStatus.End:
						// release interactive mode busy
						this.IsInteractiveModeBusy = false;
						
						dx = e.EndF.X - e.BeginF.X;
						dy = e.EndF.Y - e.BeginF.Y;

						this._begin.X = _begin.X + dx;
						this._begin.Y = _begin.Y + dy;

						this._end.X = _end.X + dx;
						this._end.Y = _end.Y + dy;

						// clear temporary points
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

						dx = e.EndF.X - e.BeginF.X;
						dy = e.EndF.Y - e.BeginF.Y;

						pts[0] = new PointF(_begin.X + dx, _begin.Y + dy);
						pts[1] = new PointF(_end.X + dx, _end.Y + dy);
						// update temporary data profile
						this.UpdateDataProfile(pts[0], pts[1]);

						// render temporary points
						pts = transformer.PointToPhysical(pts);
						workspace.DrawHelper.DrawXorLine(pts[0], pts[1]);
						
						// update temporary points
						_lastPoints = pts;

						break;
				}
			}	
			else if (this._interactiveMode == InteractiveMode.AdjustBeginPoint)
			{
				float dx=0, dy=0;

				switch (e.Status)
				{
					case RasterViewerInteractiveStatus.Begin:
						// set interactive mode busy
						this.IsInteractiveModeBusy = true;
						pts[0] = _begin;
						pts[1] = _end;
						pts = transformer.PointToPhysical(pts);
						workspace.DrawHelper.DrawXorLine(pts[0], pts[1]);
						// update temporary points
						_lastPoints = pts;
						this.Cursor = Cursors.SizeAll;
						break;

					case RasterViewerInteractiveStatus.End:
						// release interactive mode busy
						this.IsInteractiveModeBusy = false;
						this._interactiveLine[1] = e.EndF;
					
						dx = e.EndF.X - e.BeginF.X;
						dy = e.EndF.Y - e.BeginF.Y;

						this._begin.X = _begin.X + dx;
						this._begin.Y = _begin.Y + dy;

						// update point by keyboard state
						this.UpdatePointByKeyboardState(ref _end, ref _begin);

						// clear temporary points
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

						dx = e.EndF.X - e.BeginF.X;
						dy = e.EndF.Y - e.BeginF.Y;

						pts = new PointF[2];
						pts[0] = new PointF(_begin.X + dx, _begin.Y + dy);
						pts[1] = new PointF(_end.X, _end.Y);

						// update point by keyboard state
						this.UpdatePointByKeyboardState(ref pts[1], ref pts[0]);

						// update temporary data profile
						this.UpdateDataProfile(pts[0], pts[1]);

						// render temporary points
						pts = transformer.PointToPhysical(pts);
						workspace.DrawHelper.DrawXorLine(pts[0], pts[1]);


						// update temporary points
						_lastPoints = pts;
						break;
				}
			}	
			else if (this._interactiveMode == InteractiveMode.AdjustEndPoint)
			{
				float dx=0, dy=0;

				switch (e.Status)
				{
					case RasterViewerInteractiveStatus.Begin:
						// set interactive mode busy
						this.IsInteractiveModeBusy = true;
						pts[0] = _begin;
						pts[1] = _end;
						pts = transformer.PointToPhysical(pts);
						workspace.DrawHelper.DrawXorLine(pts[0], pts[1]);
						// update temporary points
						_lastPoints = pts;
						this.Cursor = Cursors.SizeAll;
						break;

					case RasterViewerInteractiveStatus.End:
						// release interactive mode busy
						this.IsInteractiveModeBusy = false;
						this._interactiveLine[1] = e.EndF;
					
						dx = e.EndF.X - e.BeginF.X;
						dy = e.EndF.Y - e.BeginF.Y;

						this._end.X = _end.X + dx;
						this._end.Y = _end.Y + dy;

						// clear temporary points
						_lastPoints = null;

						// update point by keyboard state
						this.UpdatePointByKeyboardState(ref _begin, ref _end);

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

						dx = e.EndF.X - e.BeginF.X;
						dy = e.EndF.Y - e.BeginF.Y;

						pts = new PointF[2];
						pts[0] = new PointF(_begin.X, _begin.Y);
						pts[1] = new PointF(_end.X + dx, _end.Y + dy);

						// update point by keyboard state
						this.UpdatePointByKeyboardState(ref pts[0], ref pts[1]);

						// update temporary data profile
						this.UpdateDataProfile(pts[0], pts[1]);

						// render temporary points
						pts = transformer.PointToPhysical(pts);
						workspace.DrawHelper.DrawXorLine(pts[0], pts[1]);
						
						// update temporary points
						_lastPoints = pts;
						break;
				}
			}	
		}

		public override void UpdateSelectedValue(object abscissaValue, object ordinaryValue)
		{
            try
            {
                float indexF = (float)ordinaryValue;
                Point pt = (Point)abscissaValue;
                this._selected = pt;
                // refresh drawing
                this.Workspace.ImageViewer.Invalidate(true);
            }
            catch
            {
            }
		}

		
		public override void DisplaySettingsWindow()
		{
			using (DlgLineProfileSettings dlg = new DlgLineProfileSettings(this._settings))
			{
				if (DialogResult.OK == dlg.ShowDialog(this.Container.DlgLineProfile))
				{
					// update internal settings
					this._settings = dlg.Settings;
					// update profile data
					this.UpdateDataProfile(this._begin, this._end);
				}
			}
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
		
		#region Internal Helpers

		private DataProfileHelper.HitTestInfo HitTest(PointF pt)
		{
			ImageWorkspace workspace = this.Workspace;
			ImageViewer viewer = workspace.ImageViewer;
			Transformer transformer = new Transformer(viewer.Transform);

			PointF[] pts = new PointF[] {_begin, _end, pt};
			pts = transformer.PointToPhysical(pts);
			float size = 4;
			float penWidth = 10;
			
			if (_begin == PointF.Empty && _end == PointF.Empty)
				return new DataProfileHelper.HitTestInfo(DataProfileHelper.HitTestStatus.Outside, pt);
			
			using (Pen pen = new Pen(Color.Red, penWidth))
			{
				pen.Alignment = PenAlignment.Center;

				using (GraphicsPath path = new GraphicsPath())
				{
					RectangleF rect = new RectangleF(pts[0].X-size*0.5F, pts[0].Y-size*0.5F, size, size);
					path.AddRectangle(rect);
					if (path.IsVisible(pts[2]))
						return new DataProfileHelper.HitTestInfo(DataProfileHelper.HitTestStatus.Begin, pt);
				}

				using (GraphicsPath path = new GraphicsPath())
				{
					RectangleF rect = new RectangleF(pts[1].X-size*0.5F, pts[1].Y-size*0.5F, size, size);
					path.AddRectangle(rect);
					if (path.IsVisible(pts[2]))
						return new DataProfileHelper.HitTestInfo(DataProfileHelper.HitTestStatus.End, pt);
				}
				
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

		protected virtual void Export(string filePath)
		{
			using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
			{
				float[] data = new float[10];
				Point[] categories = new Point[10];

				// sampling data
				this.SamplingData(this._begin, this._end, ref data, ref categories);

				for (int i=0; i<data.Length; i++)
				{
					if (i==0)
						writer.WriteLine("X,Y,Intensity");
					writer.WriteLine(String.Format("{0},{1},{2}", categories[i].X, categories[i].Y, data[i]));
				}
			}
		}

        protected virtual void UpdateDataProfile(PointF begin, PointF end)
		{
			if (this._workerThread != null && this._workerThread.IsAlive)
			{
				Thread thread = this._workerThread;
				thread.Abort();
				thread.Join();
			}

			lock (_syncObject)
			{
				this._pt1 = begin;
				this._pt2 = end;

                WorkerThread();

                //this._waitThread.Reset();
                //this._workerThread = new Thread(new ThreadStart(this.WorkerThread));
                //this._workerThread.Start();
                //this._waitThread.WaitOne();
			}
		}

		private void WorkerThread()
		{
			try
			{
				PointF begin = this._pt1;
				PointF end = this._pt2;

				float[] data = new float[10];
				Point[] categories = new Point[10];

				// signal thread started
				this._waitThread.Set();
				
				// sampling data
				this.SamplingData(begin, end, ref data, ref categories);

				// update plot
				this.DlgLineProfile.UpdateLinePlot(this._settings, begin, end, data, categories);
			} 
			catch (ThreadAbortException)
			{
			}
			catch (Exception exp)
			{
				Trace.WriteLine(exp);
			}
			finally
			{
				lock(_syncObject)
					_workerThread = null;
			}
		}

		protected virtual void SamplingData(PointF ptBegin, PointF ptEnd, ref float[] data, ref Point[] categories)
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

			if (this._settings != null)
				this._settings.UpdateDataBySettings(data);
		}

		private void UpdatePointByKeyboardState(ref PointF[] pts)
		{
			this.UpdatePointByKeyboardState(ref pts[0], ref pts[1]);
		}

		private void UpdatePointByKeyboardState(ref PointF begin, ref PointF end)
		{
			// snapping when shift key is pressed
			if ((Form.ModifierKeys & Keys.Shift) == Keys.Shift)
			{
				float dx = Math.Abs(end.X - begin.X);
				float dy = Math.Abs(end.Y - begin.Y);
				if (dx < dy)
					end = new PointF(begin.X, end.Y);
				else if (dx > dy)
					end = new PointF(end.X, begin.Y);
			}
		}

		#endregion
	}
}
