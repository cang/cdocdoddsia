using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

using SIA.UI.Controls;
using SIA.UI.Components.Common;
using SIA.UI.Components.Renders;
using SIA.SystemLayer;
using SIA.UI.Components;

namespace SIA.UI.Controls.UserControls
{
	/// <summary>
	/// Summary description for Navigator.
	/// </summary>
	public class ucNavigator : System.Windows.Forms.UserControl
	{
		private enum InteractiveMode
		{
			None = 0,
			Pan
		}

		#region Constant
		private const int WIDTH = 100;
		private const int HEIGHT = 100;
		private const int DEFAULT_TIMEOUT = 100;
		#endregion Constant

		#region Member fields
		ImageViewer _imageViewer = null;
		CommonImage _image = null;		
		Rectangle _positionRect = Rectangle.Empty;

		IRasterImageRender _render = null;
		float _opacity = 50;
		
		Bitmap _cachedBitmap = null;
		Rectangle _cachedRect = Rectangle.Empty;

		private Point[] _interactiveLine = new Point[2];
		private InteractiveMode _interactiveMode = InteractiveMode.Pan;
		private bool _isInteractiveModeBusy = false;

		public event EventHandler BeginInteractiveMode = null;
		public event EventHandler EndInteractiveMode = null;
		public event EventHandler ChangeImagePreview = null;
		private Rectangle _positionRectBound = Rectangle.Empty;
		public int WorkingTop = 0;
		public int WorkingLeft = 0;
		public int BorderWidth = 3;		
		#endregion Member fields

		#region Window member fields
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion Window member fields

		#region Constructors and Destructors
		public ucNavigator()
		{			
			InitializeComponent();
                                                                                                                        			
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}

				if (_image != null)
				{
					_image.Dispose();
					_image = null;
				}

				if (_cachedBitmap != null)
				{
					_cachedBitmap.Dispose();
					_cachedBitmap = null;
				}
			}
			base.Dispose( disposing );
		}
		#endregion Constructors and Destructors

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Name = "ucNavigator";
			this.Size = new System.Drawing.Size(100, 100);
			this.Load += new System.EventHandler(this.OnLoad);
			//this.MouseDown += new MouseEventHandler(ucNavigator_MouseDown);
		}
		#endregion

		#region Properties
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
			get
			{
				return _imageViewer;
			}
			set
			{
				_imageViewer = value;
				// OnImageViewerChanged();
			}
		}

//		protected virtual void OnImageViewerChanged()
//		{
//			// refresh image thumbnail
//			this.UpdateImageThumbnail();
//		}

		public Rectangle PositionRect
		{
			get
			{
				return _positionRect;
			}

			set
			{
				_positionRect = value;
				this.Invalidate(true);
			}
		}

		public float Opacity
		{
			get {return _opacity;}
			set {_opacity = value;}
		}
                

		public Rectangle RectangleThumbnail
		{
			get
			{
				return _cachedRect;
			}
		}		

		public Rectangle RectBound
		{
			get { return _positionRectBound; }
			set 
			{
				_positionRectBound = value;
				this.OnRectBoundChanged();
			}
		}

		private void OnRectBoundChanged()
		{
			this.Location = _positionRectBound.Location;
			this.Width = _positionRectBound.Width;
			this.Height = _positionRectBound.Height;
			WorkingLeft = this.Left + (_positionRectBound.Width - WIDTH)/2;
			WorkingTop = this.Top + (_positionRectBound.Height - HEIGHT)/2;
		}
		#endregion Properties

		#region Public Methods
		public void Draw(Graphics graph)
		{
			if (_image == null || _image.RasterImage == null)
				return;
						
			GraphicsState state = null;
			Bitmap bitmap = null;
			try
			{
				state = graph.Save();
				if (_cachedBitmap == null)
				{
					IRasterImageRender render = this.RasterImageRender;
					PseudoColor pseudoColor = this.ImageViewer.PseudoColor;
					render.UpdateColorMapTable(pseudoColor.Colors, pseudoColor.Positions);
			
					// rendering thumbnail
					int[] palette = this.RasterImageRender.Palette;
					_cachedBitmap = _image.RasterImage.CreateBitmap(palette, PixelFormat.Format24bppRgb);
				}

				// render images
				bitmap = _cachedBitmap;
				if (bitmap != null)
				{
					Point pt = this.DestinationLocation();
								
					ColorMatrix clrMat = new ColorMatrix();
					clrMat.Matrix00 = clrMat.Matrix11 = clrMat.Matrix22 = clrMat.Matrix44 = 1.0F;
					clrMat.Matrix33 = this._opacity * 0.01F;
			
					ImageAttributes imageAttr = new ImageAttributes();
					imageAttr.SetColorMatrix(clrMat);
					Rectangle destRect = _cachedRect;
					graph.DrawImage(bitmap, destRect, 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel, imageAttr);
				}

				// rendering boundary
				int borderWidth = this.BorderWidth;
				Rectangle rcBound = new Rectangle(
					this.RectBound.Left+borderWidth, 
					this.RectBound.Top+borderWidth,
					this.RectBound.Width-2*(borderWidth-1), 
					this.RectBound.Height-2*(borderWidth-1));
				rcBound.Inflate(-borderWidth, -borderWidth);
				ControlPaint.DrawBorder(graph, this.RectBound, 
					SystemColors.ControlDark, borderWidth, ButtonBorderStyle.Inset, 
					SystemColors.ControlDark, borderWidth, ButtonBorderStyle.Inset, 
					SystemColors.ControlLight, borderWidth, ButtonBorderStyle.Inset,
					SystemColors.ControlLight, borderWidth, ButtonBorderStyle.Inset);
				rcBound.Inflate(borderWidth, borderWidth);
				ControlPaint.DrawBorder(graph, this.RectBound, 
					SystemColors.ControlDark, borderWidth, ButtonBorderStyle.Outset, 
					SystemColors.ControlDark, borderWidth, ButtonBorderStyle.Outset, 
					SystemColors.ControlLight, borderWidth, ButtonBorderStyle.Outset,
					SystemColors.ControlLight, borderWidth, ButtonBorderStyle.Outset);

				// rendering preview image navigation
				using (Pen pen = new Pen(Color.Yellow, 1))
				{					
					int w = _positionRect.Width;
					if (w > 1) w--;
					int h = _positionRect.Height;
					if (h > 1) h--;
					graph.DrawRectangle(pen, _positionRect.Left+_cachedRect.Left, _positionRect.Top+_cachedRect.Top, w, h);
				}				
			}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);
			}
			finally
			{					
				graph.Restore(state);
				bitmap = null;
			}
		}

		#endregion

		#region Override methods

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= 0x00000020;
				return cp;
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint (e);
		}

		protected override void OnPaintBackground(PaintEventArgs args)
		{
		}
		
		#endregion Override methods

		#region Methods
		#endregion Methods

		#region Event handlers
		private void OnLoad(object sender, System.EventArgs e)
		{			
		}

		#endregion Event handlers

		#region Internal helpers
		public void UpdateImageThumbnail()
		{
			// create one time only
			if (_imageViewer != null && _imageViewer.Image != null && _image == null)
			{
				_image = _imageViewer.Image.CreateScaledImage(WIDTH, HEIGHT);
				Point pt = this.DestinationLocation();
				_cachedRect = new Rectangle(pt.X+this.WorkingLeft, pt.Y+this.WorkingTop, _image.Width, _image.Height);
			}
		}

		private Point DestinationLocation()
		{
			int width = _image.Width;
			int height = _image.Height;

			int dstWidth = WIDTH;
			int dstHeight = HEIGHT;

			int dst_left = 0;
			int dst_top = 0;
			if (width < dstWidth)
				dst_left = (int)((dstWidth-width)*0.5f);
			if (height < dstHeight)
				dst_top = (int)((dstHeight-height)*0.5f);

			return new Point(dst_left, dst_top);
		}
		#endregion Internal helpers

		#region Change Rectangle Preview Event
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Left) != MouseButtons.Left)
			{
				if (this._isInteractiveModeBusy)
					this.OnCancelInteractiveMode();
			}
			else
			{
				if (!IsInRect(e.X, e.Y))
				{
					if (_interactiveMode == InteractiveMode.Pan)
					{						
						OnPanImage(e.X, e.Y);
						this.OnEndInteractiveMode();
						_interactiveLine[0] = Point.Empty;
						_interactiveLine[1] = Point.Empty;
					}
//					if (this._isInteractiveModeBusy)
//						this.OnCancelInteractiveMode();
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
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (this._isInteractiveModeBusy)
			{
				switch (_interactiveMode)
				{
					case InteractiveMode.Pan:
						RasterViewerLineEventArgs args = new RasterViewerLineEventArgs(RasterViewerInteractiveStatus.Working, _interactiveLine[1], new Point(e.X, e.Y));
						OnInteractivePan(args);
						break;
					default:
						break;
				}
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
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
				Point offset = new Point(0, 0);
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
			}
		}

		private void OnPanImage(Point begin, Point end)
		{
			int offsetX = end.X - begin.X;
			int offsetY = end.Y - begin.Y;

			if (offsetX != 0 || offsetY != 0)
			{
				int xMin = 0;
				int xMax = this._cachedRect.Width-1;
				int yMin = 0;
				int yMax = this._cachedRect.Height-1;

				int w = _positionRect.Width;
				int h = _positionRect.Height;
				int t, l;
				t = _positionRect.Top + offsetY;
				if (t < yMin)
					t = yMin;
				if (t >  yMax-h+1)
					t = yMax-h+1;
				
				l = _positionRect.Left + offsetX;
				if (l < xMin)
					l = xMin;
				if (l >  xMax-w+1)
					l = xMax-w+1;

				_positionRect = new Rectangle(l, t, w, h);
				
				// raise rectangle position change
				this.RaiseChangeImagePreviewEvent();
			}
		}

		private void OnPanImage(int x, int y)
		{			
			int xMin = this._cachedRect.Left - this.Left;
			int xMax = xMin + this._cachedRect.Width-1;
			int yMin = this._cachedRect.Top - this.Top;
			int yMax = yMin + this._cachedRect.Height-1;

			if (x < xMin || x > xMax || y < yMin || y > yMax)
				return;

			x -= xMin;
			y -= yMin;

			xMin = 0;
			xMax = this._cachedRect.Width-1;
			yMin = 0;
			yMax = this._cachedRect.Height-1;

			int w = _positionRect.Width;
			int h = _positionRect.Height;
			int t, l;
			t =  y - h/2;				
			if (t < yMin)
				t = yMin;
			if (t >  yMax-h+1)
				t = yMax-h+1;
				
			l = x - w/2;
			if (l < xMin)
				l = xMin;
			if (l >  xMax-w+1)
				l = xMax-w+1;

			_positionRect = new Rectangle(l, t, w, h);
				
			// raise rectangle position change
			this.RaiseChangeImagePreviewEvent();
		}
		
		private void RefreshWorkingImage()
		{
			if (_image == null)
			{
				if (_imageViewer != null && _imageViewer.Image != null)
					_image = _imageViewer.Image.CreateScaledImage(WIDTH, HEIGHT);
			}			
		}

		private void RaiseChangeImagePreviewEvent()
		{
			if (ChangeImagePreview != null)
				this.ChangeImagePreview(this, System.EventArgs.Empty);
		}

		private bool IsInRect(int x, int y)
		{
			x -= this._cachedRect.Left - this.Left;
			y -= this._cachedRect.Top - this.Top;
			if (x < _positionRect.Left || x > _positionRect.Right || y < _positionRect.Top || y > _positionRect.Bottom)
				return false;
			return true;
		}
		#endregion
	}
}
