using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Runtime.CompilerServices;

using SIA.Common;
using SIA.SystemFrameworks;
using SIA.UI.Components.Renders;

namespace SIA.UI.Components
{
	public class RasterImagePrinter
	{
		#region public events
		
		public event EventHandler CenterModeChanged;
		public event EventHandler ImageRectangleChanged;
		public event EventHandler PageRectangleChanged;
		public event EventHandler SizeModeChanged;
		public event EventHandler UseDpiChanged;

		#endregion

		#region constructor and destructor
		
		public RasterImagePrinter(CommonImageViewer viewer)
		{
			if (viewer == null)
				throw new ArgumentNullException("viewer");

			this._viewer = viewer;
			this._sizeMode = viewer.SizeMode;
			this._centerMode = viewer.CenterMode;
			this._imageRectangle = new Rectangle(0, 0, viewer.Image.Width, viewer.Image.Height);
			this._pageRectangle = RectangleF.Empty;
		}

		#endregion

		#region overridable routines

		protected virtual void OnCenterModeChanged(EventArgs e)
		{
			if (this.CenterModeChanged != null)
			{
				this.CenterModeChanged(this, e);
			}
		}

		protected virtual void OnImageRectangleChanged(EventArgs e)
		{
			if (this.ImageRectangleChanged != null)
			{
				this.ImageRectangleChanged(this, e);
			}
		}

		protected virtual void OnPageRectangleChanged(EventArgs e)
		{
			if (this.PageRectangleChanged != null)
			{
				this.PageRectangleChanged(this, e);
			}
		}

		protected virtual void OnSizeModeChanged(EventArgs e)
		{
			if (this.SizeModeChanged != null)
			{
				this.SizeModeChanged(this, e);
			}
		}

		protected virtual void OnUseDpiChanged(EventArgs e)
		{
			if (this.UseDpiChanged != null)
				this.UseDpiChanged(this, e);
		}
		
		protected void Prepare(int width, int height, float dpiX, float dpiY, PrintPageEventArgs e)
		{
			SizeF size;
			float scaleFactor;
			RectangleF rcDest;
			Graphics graphics = e.Graphics;
			
			Rectangle rcSource = this.ImageRectangle.IsEmpty ? new Rectangle(0, 0, width, height) : this.ImageRectangle;
			
			if (this.UseDpi)
			{
				size = new SizeF(rcSource.Width * (graphics.DpiX / dpiX), rcSource.Height * (graphics.DpiY / dpiY));
			}
			else
			{
				size = new SizeF((float) rcSource.Width, (float) rcSource.Height);
			}
			
			float XRes = graphics.DpiX / 100f;
			float YRes = graphics.DpiY / 100f;

			RectangleF rcPage = this.PageRectangle.IsEmpty ? ((RectangleF) e.MarginBounds) : this.PageRectangle;
			//rcPage = new RectangleF(rcPage.Left * XRes, rcPage.Top * YRes, (rcPage.Width * XRes) - 1f, (rcPage.Height * YRes) - 1f);
			float scaleDx = 1f;
			float scaleDy = 1f;
			
			RasterViewerSizeMode sizeMode = this.SizeMode;
			if (sizeMode == RasterViewerSizeMode.FitIfLarger)
			{
				if ((size.Width > rcPage.Width) || (size.Height > rcPage.Height))
				{
					sizeMode = RasterViewerSizeMode.Fit;
				}
				else
				{
					sizeMode = RasterViewerSizeMode.Normal;
				}
			}

			switch (sizeMode)
			{
				case RasterViewerSizeMode.Normal:
				case RasterViewerSizeMode.FitWidth:
					break;
				case RasterViewerSizeMode.Fit:
					if (((size.Width <= 0f) || (size.Height <= 0f)) || ((rcPage.Width <= 0f) || (rcPage.Height <= 0f)))
					{
						break;
					}
					else if (rcPage.Width <= rcPage.Height)
					{
						scaleFactor = rcPage.Height / size.Height;
						if ((scaleFactor * size.Width) > rcPage.Width)
							scaleFactor = rcPage.Width / size.Width;						
						scaleDx = scaleDy = scaleFactor;
					}
					else
					{
						scaleFactor = rcPage.Width / size.Width;
						if ((scaleFactor * size.Height) > rcPage.Height)
							scaleFactor = rcPage.Height / size.Height;

						scaleDx = scaleDy = scaleFactor;
					}
					break;

				case RasterViewerSizeMode.Stretch:
					if ((size.Width > 0f) && (size.Height > 0f))
					{
						scaleDx = rcPage.Width / size.Width;
						scaleDy = rcPage.Height / size.Height;
					}
					break;

				default:
					break;
			}
			
			rcDest = new RectangleF(0f, 0f, size.Width * scaleDx, size.Height * scaleDy);
			
			if ((this.CenterMode == RasterViewerCenterMode.Horizontal) || (this.CenterMode == RasterViewerCenterMode.Both))
				rcDest.X = (rcPage.Width - rcDest.Width) / 2f;
			
			if ((this.CenterMode == RasterViewerCenterMode.Vertical) || (this.CenterMode == RasterViewerCenterMode.Both))
				rcDest.Y = (rcPage.Height - rcDest.Height) / 2f;
			
			rcDest.X += rcPage.Left;
			rcDest.Y += rcPage.Top;
			
			this._sourceRectangle = Rectangle.Round((RectangleF) rcSource);
			this._destinationRectangle = Rectangle.Round(rcDest);
			this._destinationClipRectangle = Rectangle.Round(rcPage);
		}

		#endregion

		#region methods

		public virtual void Print(PrintPageEventArgs e)
		{
			if (e == null)
				throw new ArgumentNullException("e");
			if (_viewer == null || _viewer.IsImageAvailable == false)
				throw new InvalidOperationException();

			IRasterImage image = _viewer.Image.RasterImage;
			Graphics graphics = e.Graphics;
			GraphicsState state = graphics.Save();
			int XRes = 0x60;//image.XResolution;
			int YRes = 0x60;//image.YResolution;
			
			// prepare printing
			this.Prepare(image.Width, image.Height, (float) XRes, (float) YRes, e);
			
			try
			{
				IRasterImageRender render = _viewer.RasterImageRender;
				render.Print(image, graphics, this.SourceRectangle, this.SourceRectangle, this.DestinationRectangle, this.DestinationClipRectangle);
			}
			finally
			{
				graphics.Restore(state);
			}
		}	

		#endregion

		#region properties

		public RasterViewerCenterMode CenterMode
		{
			get
			{
				return this._centerMode;
			}
			set
			{
				if (value != this.CenterMode)
				{
					this._centerMode = value;
					this.OnCenterModeChanged(EventArgs.Empty);
				}
			}
		}

		protected Rectangle DestinationClipRectangle
		{
			get
			{
				return this._destinationClipRectangle;
			}
		}

		protected Rectangle DestinationRectangle
		{
			get
			{
				return this._destinationRectangle;
			}
		}

		public Rectangle ImageRectangle
		{
			get
			{
				return this._imageRectangle;
			}
			set
			{
				if (value != this.ImageRectangle)
				{
					this._imageRectangle = value;
					this.OnImageRectangleChanged(EventArgs.Empty);
				}
			}
		}

		public RectangleF PageRectangle
		{
			get
			{
				return this._pageRectangle;
			}
			set
			{
				if (value != this.PageRectangle)
				{
					this._pageRectangle = value;
					this.OnPageRectangleChanged(EventArgs.Empty);
				}
			}
		}

		public RasterViewerSizeMode SizeMode
		{
			get
			{
				return this._sizeMode;
			}
			set
			{
				if (value != this.SizeMode)
				{
					this._sizeMode = value;
					this.OnSizeModeChanged(EventArgs.Empty);
				}
			}
		}

		public bool UseDpi
		{
			get {return _useDpi;}
			set
			{
				_useDpi = value;
				OnUseDpiChanged(EventArgs.Empty);
			}
		}

		protected Rectangle SourceRectangle
		{
			get
			{
				return this._sourceRectangle;
			}
		}

		#endregion
		
		#region fields
		
		private const int _defaultXResolution = 0x60;
		private const int _defaultYResolution = 0x60;

		
		private Rectangle _destinationClipRectangle;
		private Rectangle _destinationRectangle;
		private Rectangle _imageRectangle;
		private RectangleF _pageRectangle;
		private RasterViewerCenterMode _centerMode;
		private RasterViewerSizeMode _sizeMode;
		private Rectangle _sourceRectangle;
		private bool _useDpi = false;
		private CommonImageViewer _viewer = null;
		#endregion
        
	}
}

