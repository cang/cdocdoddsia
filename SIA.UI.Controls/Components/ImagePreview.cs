using System;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

using SIA.Common;
using SIA.Common.Utility;

using SIA.UI.Controls;
using SIA.UI.Controls.Common;

using SIA.SystemFrameworks;


using SIA.UI.Components.Common;
using SIA.UI.Components.Helpers;
using SIA.UI.Components.Renders;
using SIA.UI.Controls.UserControls;
using SIA.SystemLayer;

namespace SIA.UI.Components
{
	/// <summary>
	/// Summary description for ImagePreview.
	/// </summary>
	public class ImagePreview : System.Windows.Forms.Control
	{
		private const string keyRecentPreviewRectangle = "recentPreviewRectangle";
		private const int DEFAULT_TIMEOUT = 100;

		private enum InteractiveMode
		{
			None = 0,
			Pan
		}

		#region member attributes

        private object _syncObject = new object();
		private IRasterImageRender _render = null;
		private ImageViewer _imageViewer = null;
		private Rectangle _rectPreview = Rectangle.Empty;
		private SIA.SystemLayer.CommonImage _image = null;

		private Point[] _interactiveLine = new Point[2];
		private InteractiveMode _interactiveMode = InteractiveMode.Pan;
		private bool _isInteractiveModeBusy = false;

		public event EventHandler BeginInteractiveMode = null;
		public event EventHandler EndInteractiveMode = null;
		public event EventHandler ChangeImagePreview = null;

		private ucNavigator _navigator = null;
		private bool _bNavigatorVisible = false;

		public bool DrawWaferBound = false;
		public PointF WaferCenter = PointF.Empty;
		public float WaferRadius = 0;
		public bool LockedMouseEvents = false;
		#endregion

		#region public properties

		protected PreviewRasterImageRender RasterImageRender
		{
			get
			{
				if (_render == null)
				{
					_render = RasterImageRenderFactory.CreatePreviewRender(_imageViewer);
					_render.ImageViewer = this._imageViewer;
				}
				return (PreviewRasterImageRender)_render;
			}
		}
		
		public ImageViewer ImageViewer
		{
			get {return _imageViewer;}
			set 
			{
				_imageViewer = value;
				OnImageViewerChanged();
			}
		}

		protected virtual void OnImageViewerChanged()
		{
			// reset image render
			_render = null;

			// reset source rectangle
			if (_imageViewer != null)
			{
				// retrieved lasted view rectangle
				if (this.LoadRecentViewRectangle() == false)
				{
					if (this._imageViewer.Image.Width > this.Width && 
						this._imageViewer.Image.Height > this.Height) 
					{
						// center preview image
						int left = (int)Math.Floor((this._imageViewer.Image.Width - this.Width) * 0.5F);
						int top = (int)Math.Floor((this._imageViewer.Image.Height - this.Height) * 0.5F);
						_rectPreview = new Rectangle(left, top, this.Width, this.Height);
					}
					else
					{
						// retrieved default view rectangle
						int width = Math.Min(this.Width, _imageViewer.Image.Width);
						int height = Math.Min(this.Height, _imageViewer.Image.Height);
						_rectPreview = new Rectangle(0, 0, width, height);
					}
				}
			}
			else
				_rectPreview = Rectangle.Empty;

			// raise image changed event
			this.RaiseChangeImagePreviewEvent();

			// refresh navigator image
			this.Navigator.ImageViewer = _imageViewer;

			this.UpdateNavigatorSize();

			// refresh navigator properties
			this.RefreshNavigator();

			this.Invalidate(true);
		}

		public bool IsImageAvailable
		{
			get {return _imageViewer != null && _imageViewer.Image != null && _rectPreview != Rectangle.Empty;}
		}

		public Rectangle PreviewRectangle
		{
			get {return _rectPreview;}
			set 
			{
				_rectPreview = value;
				OnPreviewRectangleChanged();
			}
		}

		protected virtual void OnPreviewRectangleChanged()
		{
			if (IsImageAvailable)
				this.RasterImageRender.IsDirty = true;
		}


		public ucNavigator Navigator
		{
			get {return _navigator;}
		}

		private bool NavigatorVisible
		{
			get
			{
				return _bNavigatorVisible;
			}

			set
			{
				_bNavigatorVisible = value;
				this._navigator.Visible = _bNavigatorVisible;
			}
		}
		#endregion

		#region constructor and destructor
		public ImagePreview()
		{
			this.InitializeComponents();

			this.InitNavigator();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose (disposing);

			if (_image != null)
				_image.Dispose();
		}

		private void InitializeComponents()
		{
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
		}

		#endregion

		#region public operation
		
		public CommonImage LockPreviewRectangle()
		{
			this.RefreshWorkingImage();
			if (Monitor.TryEnter(_syncObject, DEFAULT_TIMEOUT) == true)
				return _image;
			return null;
		}

		public void UnlockPreviewRectangle()
		{
            Monitor.Exit(_syncObject);
		}

		public void Reset()
		{
			if (_image != null)
			{
				this.RefreshWorkingImage();
			}
		}

		public bool IsPreviewImage(CommonImage image)
		{
			return image == _image;
		}

		#endregion

		#region override routines		
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint (e);

			if (IsImageAvailable)
			{
				try
				{
					Rectangle src = _rectPreview;
					Rectangle dst = this.ClientRectangle;

					int src_width = _imageViewer.Image.Width;
					int src_height = _imageViewer.Image.Height;
					int dst_left = 0, dst_top = 0;
					int dst_width = 0, dst_height = 0;
					
					if (src_width < this.Width)
					{
						dst_left = (int)((this.Width - src_width) / 2.0F);
						dst_width = src_width;
					}
					else
					{
						dst_left = 0;
						dst_width = this.Width;
					}

					if (src_height < this.Height)
					{
						dst_top = (int)((this.Height - src_height) / 2.0F);
						dst_height = src_height;
					}
					else
					{
						dst_top = 0;
						dst_height = this.Height;
					}

					dst = new Rectangle(dst_left, dst_top, dst_width, dst_height);
					src = new Rectangle(src.Left, src.Top, dst_width, dst_height);
					
					if (this._isInteractiveModeBusy || _image == null)
					{
						IRasterImageRender render = this.RasterImageRender;
						PseudoColor pseudoColor = this.ImageViewer.PseudoColor;
						render.UpdateColorMapTable(pseudoColor.Colors, pseudoColor.Positions);
						render.Paint(e.Graphics, src, src, dst, dst);
					}
					else
					{
						Graphics graph = e.Graphics;
						GraphicsState state = graph.Save();
						Bitmap bitmap = null;
						try
						{
							IRasterImageRender render = this.RasterImageRender;
							PseudoColor pseudoColor = this.ImageViewer.PseudoColor;
							render.UpdateColorMapTable(pseudoColor.Colors, pseudoColor.Positions);

							if (_image != null && _image.RasterImage != null)
							{
								int[] palette = this.RasterImageRender.Palette;
								bitmap = _image.RasterImage.CreateBitmap(palette, PixelFormat.Format24bppRgb);
								if (bitmap != null)
									graph.DrawImage(bitmap, dst.Location);
							}							
						}
						catch(System.Exception exp)
						{
							Trace.WriteLine(exp);
						}
						finally
						{
							if (bitmap != null)
								bitmap.Dispose();						
						}
						graph.Restore(state);
					}

					// draw wafer bound
					if (DrawWaferBound)
					{
						float t = (float)Math.Max(_rectPreview.Top, 0);
						float l = (float)Math.Max(_rectPreview.Left, 0);
						t = WaferCenter.Y - WaferRadius - t;
						l = WaferCenter.X - WaferRadius - l;
						using (Pen pen = new Pen(Color.Red, 3))
						{
							e.Graphics.DrawEllipse(pen, l, t, WaferRadius*2, WaferRadius*2);
						}
					}

					// draw navigator
					if (_bNavigatorVisible)
						this.Navigator.Draw(e.Graphics);					
				}
				catch(System.Exception exp)
				{
					Trace.WriteLine(exp);
				}
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown (e);

			if (LockedMouseEvents)
				return;

			if ((e.Button & MouseButtons.Left) != MouseButtons.Left)
			{
				if (this._isInteractiveModeBusy)
					this.OnCancelInteractiveMode();
			}
			else
			{
				switch (_interactiveMode)
				{
					case InteractiveMode.Pan:
						RasterViewerLineEventArgs args = new RasterViewerLineEventArgs(RasterViewerInteractiveStatus.Begin, new Point(e.X, e.Y), new Point(e.X, e.Y));
						OnInteractivePan(args);
						break;
					default:
						break;
				}
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove (e);

			if (LockedMouseEvents)
				return;

			if (this._isInteractiveModeBusy)
			{
				switch (_interactiveMode)
				{
					case InteractiveMode.Pan:
						RasterViewerLineEventArgs args = new RasterViewerLineEventArgs(RasterViewerInteractiveStatus.Working, _interactiveLine[0], new Point(e.X, e.Y));
						OnInteractivePan(args);
						break;
					default:
						break;
				}
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp (e);

			if (LockedMouseEvents)
				return;

			switch (_interactiveMode)
			{
				case InteractiveMode.Pan:
					RasterViewerLineEventArgs args = new RasterViewerLineEventArgs(RasterViewerInteractiveStatus.End, _interactiveLine[0], new Point(e.X, e.Y));
					OnInteractivePan(args);
					break;
				default:
					break;
			}
		}

		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus (e);

			if (this._isInteractiveModeBusy)
				this.OnCancelInteractiveMode();			
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated (e);
		}

		protected override void OnHandleDestroyed(EventArgs e)
		{
			base.OnHandleDestroyed (e);

			// save recent preview rectangle
			this.SaveRecentViewRectangle();
		}
		#endregion

		#region internal helpers
		private void InitNavigator()
		{
			this._navigator = new ucNavigator();			
			this._navigator.Location = Point.Empty;
			this.Controls.Add(_navigator);
			this._navigator.BringToFront();
			this._navigator.ChangeImagePreview += new System.EventHandler(this.OnImagePreviewChanged);

			this.NavigatorVisible = false;
		}

		private void UpdateNavigatorSize()
		{
			if (_navigator != null && this._imageViewer != null)
			{
				int width = this._imageViewer.Image.Width;
				int height = this._imageViewer.Image.Height;
				if (this.Width < width || this.Height < height)
				{					
					int borderWidth = this._navigator.BorderWidth;
					this._navigator.RectBound = new Rectangle(
						this.Left + this.Width - this._navigator.Width-10-2*borderWidth, 
						this.Top + this.Height - this._navigator.Height-17-2*borderWidth, 
						100+2*borderWidth, 
						100+2*borderWidth);
					this._navigator.UpdateImageThumbnail();	
					this.NavigatorVisible = true;										

					this.RefreshNavigator();

					this.Invalidate(true);
				}
			}	
		}

		private void RefreshNavigator()
		{
			if (this._navigator == null || this.ImageViewer == null)
				return;

			Rectangle rectThumbnail = this._navigator.RectangleThumbnail;			
			Rectangle rect =  this.PreviewRectangle;
			int t, l, w, h;
			if (rect.Top < 0)
			{
				t = rectThumbnail.Top - this._navigator.WorkingTop;
				h = (int)(rect.Height*rectThumbnail.Height*1.0f/this.ImageViewer.Image.Height);
				if (h > rectThumbnail.Height)
					h = rectThumbnail.Height;
			}
			else
			{
				h = (int)(rect.Height*rectThumbnail.Height*1.0f/this.ImageViewer.Image.Height);
				if (h <= 0)
					h = 1;
				t = (int)(rect.Top*rectThumbnail.Height*1.0f/this.ImageViewer.Image.Height);
//				if (t < rectThumbnail.Top - this._navigator.WorkingTop)
//					t = rectThumbnail.Top - this._navigator.WorkingTop;
			}

			if (rect.Left < 0)
			{
				l = rectThumbnail.Left - this._navigator.WorkingLeft;
				w = (int)(rect.Width*rectThumbnail.Width*1.0f/this.ImageViewer.Image.Width);
				if (w > rectThumbnail.Width)
					w = rectThumbnail.Width;
			}
			else
			{
				w = (int)(rect.Width*rectThumbnail.Width*1.0f/this.ImageViewer.Image.Width);
				if (w <= 0)
					w = 1;
				l = (int)(rect.Left*rectThumbnail.Width*1.0f/this.ImageViewer.Image.Width);
//				if (l < rectThumbnail.Left - this._navigator.WorkingLeft)
//					l = rectThumbnail.Left - this._navigator.WorkingLeft;
			}

			Rectangle rectPosition = new Rectangle(l, t, w, h);
			this._navigator.PositionRect = rectPosition;
		}

		private void OnBeginInteractiveMode()
		{
			if (this._isInteractiveModeBusy == false)
			{
				this._isInteractiveModeBusy = true;
				this.Capture = true;
				this.Cursor = Cursors.Hand;

				if (this.BeginInteractiveMode != null)
					this.BeginInteractiveMode(this, EventArgs.Empty);
			}
		}

		private void OnEndInteractiveMode()
		{
			// refresh working image
			RefreshWorkingImage();

			if (this._isInteractiveModeBusy == true)
			{
				this._isInteractiveModeBusy = false;
				this.Capture = false;
				this.Cursor = Cursors.Default;

				if (this.EndInteractiveMode != null)
					this.EndInteractiveMode(this, EventArgs.Empty);
			}
		}

		private void OnCancelInteractiveMode()
		{
			if (this._isInteractiveModeBusy == true)
			{
				this.OnEndInteractiveMode();
				switch(this._interactiveMode)
				{
					case InteractiveMode.Pan:
						RasterViewerLineEventArgs args = new RasterViewerLineEventArgs();
						args.Cancel = true;
						OnInteractivePan(args);
						break;
					default:
						break;
				}
			}
		}

		private void OnInteractivePan(RasterViewerLineEventArgs args)
		{
			if (args.Cancel == false)
			{
				switch (args.Status)
				{
					case RasterViewerInteractiveStatus.Begin:
						this.OnBeginInteractiveMode();
						_interactiveLine[0] = args.Begin;
						_interactiveLine[1] = args.Begin;
						break;
					case RasterViewerInteractiveStatus.Working:
						_interactiveLine[0] = _interactiveLine[1];
						_interactiveLine[1] = args.End;					
						OnPanImage(_interactiveLine[0], _interactiveLine[1]);	
						break;
					case RasterViewerInteractiveStatus.End:
						OnPanImage(_interactiveLine[0], _interactiveLine[1]);
						this.OnEndInteractiveMode();						
						_interactiveLine[0] = Point.Empty;
						_interactiveLine[1] = Point.Empty;
						break;
					default:
						break;
				}

				// raise event for image preview rectangle changed
				this.RaiseChangeImagePreviewEvent();

				// refresh navigator
				this.RefreshNavigator();

				this.Invalidate(true);
			}
		}

		private void OnPanImage(Point begin, Point end)
		{
			int offsetX = begin.X - end.X;
			int offsetY = begin.Y - end.Y;

			if (offsetX != 0 || offsetY != 0)
			{
				int src_right = _imageViewer.Image.Width-1;
				int src_bottom = _imageViewer.Image.Height-1;
				Rectangle rcClient = this.PreviewRectangle;
				
				if (_imageViewer.Image.Width > this.Width)
				{
					rcClient.Offset(offsetX, 0);
					if (rcClient.Left < 0)
						rcClient.Offset(-rcClient.Left, 0);
					else if (rcClient.Right > src_right)
						rcClient.Offset(src_right-rcClient.Right, 0);
				}

				if (_imageViewer.Image.Height > this.Height)
				{
					rcClient.Offset(0, offsetY);
					if (rcClient.Top < 0)
						rcClient.Offset(0, -rcClient.Top);
					else if (rcClient.Bottom > src_bottom)
						rcClient.Offset(0, src_bottom-rcClient.Bottom);
				}
				
				this.PreviewRectangle = rcClient;		
			}
		}

		private void RefreshWorkingImage()
		{
			if (_image == null)
			{
				if (_imageViewer != null && _imageViewer.Image != null)
					_image = _imageViewer.Image.CropImage(_rectPreview);
			}
			else if (Monitor.TryEnter(_syncObject, DEFAULT_TIMEOUT) == true)
			{
				try
				{
					if (_image != null)
						_image.Dispose();
					_image = _imageViewer.Image.CropImage(_rectPreview);			
				}
				catch(System.Exception exp)
				{
					Trace.WriteLine(exp);
				}
				finally
				{
					Monitor.Exit(_syncObject);
				}
			}
		}

		private bool LoadRecentViewRectangle()
		{
			string str = (string)CustomConfiguration.GetValues(keyRecentPreviewRectangle);
			if (str != null && str!=string.Empty)
			{				
				string[] args = str.Split(new char[]{';'});
				int left = Convert.ToInt32(args[0]);
				int top = Convert.ToInt32(args[1]);
				int width = Convert.ToInt32(args[2]);
				int height = Convert.ToInt32(args[3]);

				int imgWidth = this._imageViewer.Image.Width;
				int imgHeight = this._imageViewer.Image.Height;
				
				width = Math.Min(this.Width, imgWidth);
				height = Math.Min(this.Height, imgHeight);

				if (left + width > imgWidth)
					left = Math.Max(0, imgWidth - width);
				if (top + height > imgHeight)
					top = Math.Max(0, imgHeight - height);

				_rectPreview = new Rectangle(left, top, width, height);
				return true;
			}

			return false;
		}

		private void SaveRecentViewRectangle()
		{
			string values = string.Format("{0};{1};{2};{3}", _rectPreview.Left, _rectPreview.Top, _rectPreview.Width, _rectPreview.Height);
			CustomConfiguration.SetValues(keyRecentPreviewRectangle, values);
		}

		private void RaiseChangeImagePreviewEvent()
		{
			if (ChangeImagePreview != null)
				this.ChangeImagePreview(this, System.EventArgs.Empty);
		}

		private void OnImagePreviewChanged(object sender, System.EventArgs e)
		{
			Rectangle rect = new Rectangle(
				this._navigator.PositionRect.Left + this._navigator.RectangleThumbnail.Left - this._navigator.WorkingLeft,
				this._navigator.PositionRect.Top + this._navigator.RectangleThumbnail.Top - this._navigator.WorkingTop,
				this._navigator.PositionRect.Width,
				this._navigator.PositionRect.Height);

			Rectangle rectThumbnail = this._navigator.RectangleThumbnail;
			if (rectThumbnail == Rectangle.Empty || rect == Rectangle.Empty)
				return;

			int imageWidth = this._imageViewer.Image.Width;
			int imageHeight = this._imageViewer.Image.Height;

			int w = _rectPreview.Width;
			if (w > imageWidth)
				w = imageWidth;
			int h = _rectPreview.Height;
			if (h > imageHeight)
				h = imageHeight;

			int l = (int)(rect.Left*imageWidth*1.0f/rectThumbnail.Width);
			if (l < 0)
				l = 0;
			int t = (int)(rect.Top*imageHeight*1.0f/rectThumbnail.Height);
			if (t < 0)
				t = 0;
			
			if (l > imageWidth - w)
				l = imageWidth - w;
			
			if (t > imageHeight - h)
				t = imageHeight - h;			
			_rectPreview = new Rectangle(l, t, w, h);
			
//			this._navigator.PositionRect = new Rectangle(
//				this._navigator.PositionRect.Left - this._navigator.RectangleThumbnail.Left + this._navigator.WorkingLeft,
//				this._navigator.PositionRect.Top - this._navigator.RectangleThumbnail.Top + this._navigator.WorkingTop,
//				this._navigator.PositionRect.Width,
//				this._navigator.PositionRect.Height);

			this.RefreshWorkingImage();
			this.PreviewRectangle = _rectPreview;
			this.Invalidate(true);
		}
		#endregion

		#region event helper
		#endregion
	}
}
