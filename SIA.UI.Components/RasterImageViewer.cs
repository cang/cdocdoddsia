using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using SIA.Common;
using SIA.Common.Native;

using SIA.SystemFrameworks;
using SIA.SystemFrameworks.UI;

using SIA.UI.Components;
using SIA.UI.Components.Helpers;
using SIA.UI.Components.Renders;

namespace SIA.UI.Components
{
	public enum RasterViewerCenterMode
	{
		// Fields
		Both = 3,
		Horizontal = 1,
		None = 0,
		Vertical = 2
	}

	public enum RasterViewerInteractiveMode : uint
	{
		// Fields
		Select = 0,
		Pan = 1,
		CenterAt = 2,
		ZoomTo = 3,
		Region = 4,
		ZoomOut = 7,
		CustomPoint,
		CustomLine,
		CustomRectangle,
		CustomPoints,
		UserMode,
        Measurement,
        Extra
	} 

	public enum RasterViewerInteractiveRegionType
	{
		Rectangle,
		Ellipse,
		Freehand
	}

	public enum RasterViewerSizeMode
	{
		Normal,
		Fit,
		FitWidth,
		Stretch,
		FitIfLarger
	}

	[System.Flags]	
	public enum RasterClipboardCopyFlags
	{
		None = 0,
		Empty = 1,
		Dib = 2,
		Bitmap = 4,
		Palette = 8,
		Region = 16
	}


	//public delegate void RasterImagePanViewerEventHandler(object sender, RasterImagePanViewerEventArgs e);
	public delegate void RasterViewerPointEventHandler(object sender, RasterViewerPointEventArgs e);
	public delegate void RasterViewerLineEventHandler(object sender, RasterViewerLineEventArgs e);
	public delegate void RasterViewerRectangleEventHandler(object sender, RasterViewerRectangleEventArgs e);
	public delegate void RasterViewerPointsEventHandler(object sender, RasterViewerPointsEventArgs e);
	//public delegate void RasterImageChangedEventHandler(object sender, SIA.UI.Components.RasterImageChangedEventArgs e);

    /// <summary>
    /// Provides functionality for displaying and user interaction of IRasterImage
    /// </summary>
	[ToolboxBitmap(typeof(System.Drawing.Bitmap))]
	public class RasterImageViewer
        : ScrollableControl, IRasterImageViewer
	{	
		#region member attributes
		
        private bool _autoDisposeImages;
		private bool _autoResetScaleFactor;
		private bool _autoResetScrollPosition;
		
        private ViewerBorderPadding _borderPadding;
		private BorderStyle _borderStyle;
		
        private RasterViewerCenterMode _centerMode;

        private float _scaleFactor;
        protected float _currentXScaleFactor;
		protected float _currentYScaleFactor;

        private bool _frameIsPartOfView;
        private Color _frameColor;
        private SizeF _frameSize;
		
        private bool _frameShadowIsPartOfView;
        private Color _frameShadowColor;
        private SizeF _frameShadowSize;
		
        protected int _ignorePaintCounter;
		protected int _ignoreSizeChangedCounter;
		
		/// <summary>
		/// This member reference to an raster image
		/// </summary>
		private IRasterImage _image;
		
		/// <summary>
		/// This member specifies the current interactive mode
		/// </summary>
		private RasterViewerInteractiveMode _interactiveMode;

		private RasterViewerInteractiveMode _origInteractiveMode = RasterViewerInteractiveMode.Select;

		/// <summary>
		/// This member is the interactive point in physical coordinate
		/// </summary>
		protected Point _interactivePoint;

		/// <summary>
		/// This member is the list of interactive points in physical coordinate
		/// </summary>
		protected RasterPointCollection _interactivePoints;
		
		/// <summary>
		/// This member is the interactive rectangle in physical coordinate
		/// </summary>
		protected Rectangle _interactiveRectangle;

		/// <summary>
		/// This member specifies whether the interactive rectangle has been draw using XOR Pen
		/// </summary>
		private bool _interactiveRectangleDrawn;

		/// <summary>
		/// This member specified the type of <b>InteractiveRegionMode</b>
		/// </summary>
		private RasterViewerInteractiveRegionType _interactiveRegionType;
		//private RasterRegionCombineMode _interactiveRegionCombineMode;

		/// <summary>
		/// This member contains the position when the mouse is down.
		/// </summary>
		private Point _ptMouseDown = Point.Empty;

		/// <summary>
		/// This member contains the last position while the mouse is moving.
		/// </summary>
		private Point _ptMouseMove = Point.Empty;

		/// <summary>
		/// This member contains the position when the mouse is released.
		/// </summary>
		private Point _ptMouseUp = Point.Empty;
		
		/// <summary>
		/// This flags specifies if the cursor is clipped or not when begin an interaction
		/// </summary>
		private bool _isCursorClipped;

		/// <summary>
		/// This flags specifies if there are and interaction working.
		/// </summary>
		private bool _isInteractiveModeBusy;


		private Rectangle _rcClip;
		private RasterViewerSizeMode _sizeMode;

        private object _syncTransform = new object();
        private Matrix _transform;

		private float _minPixelScaleFactor = 0.025F;
		private float _maxPixelScaleFactor = 0.1F;

		private Container components;

		#endregion

		#region public event handler

		public event EventHandler AutoDisposeImagesChanged;
		public event EventHandler AutoResetScaleFactorChanged;
		public event EventHandler AutoResetScrollPositionChanged;
		public event EventHandler BorderStyleChanged;
		public event EventHandler CenterModeChanged;
		public event EventHandler DoubleBufferChanged;
		public event EventHandler FrameColorChanged;
		public event EventHandler FrameIsPartOfViewChanged;
		public event EventHandler FrameShadowColorChanged;
		public event EventHandler FrameShadowIsPartOfViewChanged;
		public event EventHandler FrameShadowSizeChanged;
		public event EventHandler FrameSizeChanged;
		public event EventHandler ImageChanged;

		public event EventHandler InteractiveModeChanged;
		public event EventHandler InteractiveModeEnded;
		public event EventHandler InteractiveRegionCombineModeChanged;

		public event RasterViewerRectangleEventHandler	InteractiveSelectByRectangle;
		public event RasterViewerPointEventHandler		InteractiveCenterAt;		
		public event RasterViewerLineEventHandler		InteractivePan;		
		public event RasterViewerRectangleEventHandler	InteractiveRegionEllipse;
		public event RasterViewerPointsEventHandler		InteractiveRegionFreehand;
		public event RasterViewerRectangleEventHandler	InteractiveRegionRectangle;
		public event RasterViewerRectangleEventHandler	InteractiveZoomTo;
		public event RasterViewerPointEventHandler		InteractiveZoomOut;

		public event RasterViewerPointEventHandler		InteractiveCustomPoint;
		public event RasterViewerLineEventHandler		InteractiveCustomLine;
		public event RasterViewerRectangleEventHandler	InteractiveCustomRectangle;
		public event RasterViewerPointsEventHandler		InteractiveCustomPoints;

		public event EventHandler InteractiveRegionTypeChanged;
		public event EventHandler MagnifyGlassChanged;
		public event EventHandler PaintPropertiesChanged;

		public event PaintEventHandler PostTransformPaint;
		public event PaintEventHandler PostViewPaint;
		public event PaintEventHandler PreTransformPaint;
		public event PaintEventHandler PreViewPaint;
		
		public event EventHandler ScaleFactorChanged;
		public new event EventHandler Scroll;
		public event EventHandler SizeModeChanged;
		public event EventHandler TransformChanged;

		#endregion

		#region public properties

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Category("RasterImageViewer")]
		public new Point AutoScrollPosition
		{
			get
			{
				return base.AutoScrollPosition;
			}
			set
			{
				Point point = AutoScrollPosition;
				base.AutoScrollPosition = value;
				if (point != AutoScrollPosition)
					OnScroll(EventArgs.Empty);
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Category("RasterImageViewer")]
		private bool IsMovingFloater
		{
			get
			{
				if (IsInteractiveModeBusy)
					return (int)InteractiveMode == 6;
				return false;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Category("RasterImageViewer")]
		private SizeF RealImageSize
		{
			get
			{
				SizeF sizeF2;

				using (Graphics graphics = CreateGraphics())
				{
					double d1 = 1.0, d2 = 1.0;
					double d3 = (double)ImageDpiX;
					double d4 = (double)ImageDpiY;
					if ((double)graphics.DpiX != d3)
						d1 = (double)graphics.DpiX / d3;
					if ((double)graphics.DpiY != d4)
						d2 = (double)graphics.DpiY / d4;
					SizeF sizeF1 = ImageSize;
					if ((d1 != 1.0) || (d2 != 1.0))
						sizeF1 = new SizeF((float)(d1 * (double)sizeF1.Width), (float)(d2 * (double)sizeF1.Height));
					sizeF2 = sizeF1;
				}
				return sizeF2;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Browsable(true), Category("RasterImageViewer")]
		public virtual bool AutoDisposeImages
		{
			get
			{
				return _autoDisposeImages;
			}
			set
			{
				if (value != AutoDisposeImages)
				{
					_autoDisposeImages = value;
					OnAutoDisposeImagesChanged(EventArgs.Empty);
				}
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Browsable(true), Category("RasterImageViewer")]
		public virtual bool AutoResetScaleFactor
		{
			get
			{
				return _autoResetScaleFactor;
			}
			set
			{
				if (value != AutoResetScaleFactor)
				{
					_autoResetScaleFactor = value;
					OnAutoResetScaleFactorChanged(EventArgs.Empty);
				}
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Browsable(true), Category("RasterImageViewer")]
		public virtual bool AutoResetScrollPosition
		{
			get
			{
				return _autoResetScrollPosition;
			}
			set
			{
				if (value != AutoResetScrollPosition)
				{
					_autoResetScrollPosition = value;
					OnAutoResetScrollPositionChanged(EventArgs.Empty);
				}
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Browsable(true), Category("RasterImageViewer")]
		public virtual ViewerBorderPadding BorderPadding
		{
			get
			{
				return _borderPadding;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Browsable(true), Category("RasterImageViewer")]
		public virtual BorderStyle BorderStyle
		{
			get
			{
				return _borderStyle;
			}
			set
			{
				if (value != BorderStyle)
				{
					_borderStyle = value;
					OnBorderStyleChanged(EventArgs.Empty);
				}
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Browsable(true), Category("RasterImageViewer")]
		public virtual RasterViewerCenterMode CenterMode
		{
			get
			{
				return _centerMode;
			}
			set
			{
				if (value != CenterMode)
				{
					_centerMode = value;
					OnCenterModeChanged(EventArgs.Empty);
				}
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Category("RasterImageViewer")]
		public virtual float CurrentXScaleFactor
		{
			get
			{
				return _currentXScaleFactor;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Category("RasterImageViewer")]
		public virtual float CurrentYScaleFactor
		{
			get
			{
				return _currentYScaleFactor;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Browsable(true), Category("RasterImageViewer")]
		public virtual bool DoubleBuffer
		{
			get
			{
				return GetStyle(ControlStyles.DoubleBuffer);
			}
			set
			{
				if (value != DoubleBuffer)
				{
					SetStyle(ControlStyles.DoubleBuffer, value);
					OnDoubleBufferChanged(EventArgs.Empty);
				}
			}
		}

		
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Category("RasterImageViewer")]
		public virtual IRasterImage Image
		{
			get {return _image;}
			set
			{
				if (_image != null)
				{
					if ((value != _image) && AutoDisposeImages)
					{
						_image.Dispose();
						_image = null;
					}
				}
				_image = value;
				OnImageChanged(EventArgs.Empty);
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Category("RasterImageViewer")]
		public virtual float ImageDpiX
		{
			get
			{
				// return (float)(Image != null ? Image.XResolution : 96);
				// default Dpi
				return 96;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Category("RasterImageViewer")]
		public virtual float ImageDpiY
		{
			get
			{
				//return (float)(Image != null ? Image.YResolution : 96);
				// default Dpi
				return 96;
			}
		}

		
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Category("RasterImageViewer")]
		public virtual SizeF ImageSize
		{
			get
			{
				SizeF sizeF = SizeF.Empty;
				if (Image != null)
					sizeF = new SizeF((float)Image.Width, (float)Image.Height);
				return sizeF;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Category("RasterImageViewer")]
		public virtual RasterViewerInteractiveMode InteractiveMode
		{
			get
			{
				return _interactiveMode;
			}
			set
			{
				if (value != InteractiveMode)
				{
					if (IsInteractiveModeBusy)
						CancelInteractiveMode();
					_interactiveMode = value;
					OnInteractiveModeChanged(EventArgs.Empty);
				}
			}
		}

		public virtual void PushInteractiveMode()
		{
			if (_origInteractiveMode == RasterViewerInteractiveMode.Select)
			{
				_origInteractiveMode = InteractiveMode;
			}
		}

		public virtual RasterViewerInteractiveMode PopInteractiveMode()
		{
			RasterViewerInteractiveMode ret = _origInteractiveMode;
			_origInteractiveMode = RasterViewerInteractiveMode.Select;
			return ret;
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Category("RasterImageViewer")]
		public virtual RasterViewerInteractiveRegionType InteractiveRegionType
		{
			get
			{
				return _interactiveRegionType;
			}
			set
			{
				if (value != InteractiveRegionType)
				{
					_interactiveRegionType = value;
					OnInteractiveRegionTypeChanged(EventArgs.Empty);
				}
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Category("RasterImageViewer")]
		public virtual bool IsImageAvailable
		{
			get
			{
				return Image != null;
			}
		}
	
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Category("RasterImageViewer")]
		public virtual bool IsInteractiveModeBusy
		{
			get
			{
				return _isInteractiveModeBusy;
			}
		}

		
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Category("RasterImageViewer")]
		public virtual RectangleF LogicalViewRectangle
		{
			get
			{
				SizeF sizeF = ImageSize;
				return new RectangleF(PointF.Empty, sizeF);
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Category("RasterImageViewer")]
		public virtual Rectangle PhysicalViewRectangle
		{
			get
			{
				PointF[] pointF = DrawingHelper.GetBoundingPoints(new RectangleF(PointF.Empty, ImageSize));
				lock (_syncTransform)
					_transform.TransformPoints(pointF);
				return Rectangle.Round(DrawingHelper.GetBoundingRectangle(pointF));
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Browsable(true), Category("RasterImageViewer")]
		public virtual float ScaleFactor
		{
			get
			{
				return _scaleFactor;
			}
			set
			{
				if (value <= 0.0F)
					throw new ArgumentOutOfRangeException("ScaleFactor must be greated than 0");
				if (value != ScaleFactor)
				{
					_scaleFactor = value;
					OnScaleFactorChanged(EventArgs.Empty);
				}
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Browsable(true), Category("RasterImageViewer")]		
		public virtual RasterViewerSizeMode SizeMode
		{
			get
			{
				return _sizeMode;
			}
			set
			{
				if (value != SizeMode)
				{
					_sizeMode = value;
					OnSizeModeChanged(EventArgs.Empty);
				}
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Category("RasterImageViewer")]		
		public Matrix Transform
		{
			get
			{
                lock (_syncTransform)
                    if (_transform != null)
                        return _transform.Clone() as Matrix;
                    return null;
			}
		}

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Category("RasterImageViewer")]
        public virtual Transformer Transformer
        {
            get
            {
                Matrix matrix = this.Transform;
                return new Transformer(matrix, true);
            }
        }

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Browsable(true), Category("RasterImageViewer")]		
		public virtual Color FrameColor
		{
			get
			{
				return _frameColor;
			}
			set
			{
				Color color = FrameColor;
				if (value.ToArgb() != color.ToArgb())
				{
					_frameColor = value;
					OnFrameColorChanged(EventArgs.Empty);
				}
			}
		}
		
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Browsable(true), Category("RasterImageViewer")]		
		public virtual bool FrameIsPartOfView
		{
			get
			{
				return _frameIsPartOfView;
			}
			set
			{
				if (value != FrameIsPartOfView)
				{
					_frameIsPartOfView = value;
					OnFrameIsPartOfViewChanged(EventArgs.Empty);
				}
			}
		}
		
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Browsable(true), Category("RasterImageViewer")]		
		public virtual Color FrameShadowColor
		{
			get
			{
				return _frameShadowColor;
			}
			set
			{
				Color color = FrameShadowColor;
				if (value.ToArgb() != color.ToArgb())
				{
					_frameShadowColor = value;
					OnFrameShadowColorChanged(EventArgs.Empty);
				}
			}
		}
				
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Browsable(true), Category("RasterImageViewer")]		
		public virtual bool FrameShadowIsPartOfView
		{
			get
			{
				return _frameShadowIsPartOfView;
			}
			set
			{
				if (value != FrameShadowIsPartOfView)
				{
					_frameShadowIsPartOfView = value;
					OnFrameShadowIsPartOfViewChanged(EventArgs.Empty);
				}
			}
		}
				
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Category("RasterImageViewer")]
		public virtual SizeF FrameShadowSize
		{
			get
			{
				return _frameShadowSize;
			}
			set
			{
				if ((value.Width < 0.0F) || (value.Height < 0.0F))
					throw new ArgumentOutOfRangeException("FrameShadowSize cannot be less than 0");
				if (value != FrameShadowSize)
				{
					_frameShadowSize = value;
					OnFrameShadowSizeChanged(EventArgs.Empty);
				}
			}
		}
				
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Category("RasterImageViewer")]
		public virtual SizeF FrameSize
		{
			get
			{
				return _frameSize;
			}
			set
			{
				if ((value.Width < 0.0F) || (value.Height < 0.0F))
					throw new ArgumentOutOfRangeException("FrameSize cannot be less than 0");
				if (value != FrameSize)
				{
					_frameSize = value;
					OnFrameSizeChanged(EventArgs.Empty);
				}
			}
		}


		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Category("RasterImageViewer")]
		public virtual IRasterImageRender RasterImageRender
		{
			get
			{
				throw new System.Exception("Unimplemented routine");
			}
		}

		public float MinPixelScaleFactor 
		{
			get {return _minPixelScaleFactor;}
			set 
			{
				_minPixelScaleFactor = value;
				OnMinPixelScaleFactorChanged();
			}
		}

		protected virtual void OnMinPixelScaleFactorChanged()
		{
		}

		public float MaxPixelScaleFactor 
		{
			get {return _maxPixelScaleFactor;}
			set 
			{
				_maxPixelScaleFactor = value;
				OnMaxPixelScaleFactorChanged();
			}
		}

		protected virtual void OnMaxPixelScaleFactorChanged()
		{
		}

		#endregion

		#region constructor and destructor

		public RasterImageViewer()
		{
			components = null;
			InitializeComponent();
			InitClass();
		}


		protected override void Dispose(bool disposing)
		{
            if (disposing)
            {
                lock (_syncTransform)
                {
                    if (this._transform != null)
                    {
                        this._transform.Dispose();
                        this._transform = null;
                    }
                }

				if (this._image != null && this.AutoDisposeImages)
				{
					this._image.Dispose();
					this._image = null;
				}

				if ((this._image != null) && this.AutoDisposeImages)
				{
					this._image.Dispose();
					this._image = null;
				}
				
				if (this.components != null)
				{
					this.components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
 

		#endregion

		#region public operations

		public virtual void BeginUpdate()
		{
			this._ignorePaintCounter++;
		}
 
		public virtual void EndUpdate()
		{
			if (_ignorePaintCounter <= 0)
				throw new InvalidOperationException();
			_ignorePaintCounter--;
			if (_ignorePaintCounter <= 0)
				Invalidate();
		}

		public virtual void CenterAtPoint(Point pt)
		{
			if ((this.AutoScroll && this.IsImageAvailable) && this.PhysicalViewRectangle.Contains(pt))
			{
				PointF ptClient = new PointF((float) pt.X, (float) pt.Y);
				PointF ptCenter = new PointF(((float) base.ClientRectangle.Width) / 2f, ((float) base.ClientRectangle.Height) / 2f);
				Point ptScroll = new Point(-this.AutoScrollPosition.X + ((int) (ptClient.X - ptCenter.X)), -this.AutoScrollPosition.Y + ((int) (ptClient.Y - ptCenter.Y)));
				this.AutoScrollPosition = ptScroll;
				this.CalculateTransform();
				base.Invalidate(); 
			}
		}

		public virtual void RegionToFloater()
		{
		}

		public virtual void ZoomToRectangle(Rectangle rc)
		{
			if ((rc.Width < 1) || (rc.Height < 1))
				throw new InvalidOperationException();

			rc = DrawingHelper.FixRectangle(rc);
			Rectangle rcClient = ClientRectangle;
			Size sizeClient = rcClient.Size;
			bool bContinue = false;
			Size size = Size.Empty;
			Rectangle rect = rc;
			rc = Rectangle.Empty;
			bool scrollX = false, scrollY = false;
			Rectangle rcPhysicalView = PhysicalViewRectangle;
			do
			{
				bContinue = false;
				size.Width = MulDiv(sizeClient.Height, rect.Width, rect.Height);
				size.Height = sizeClient.Height;
				if (size.Width > sizeClient.Width)
				{
					size.Width = sizeClient.Width;
					size.Height = MulDiv(sizeClient.Width, rect.Height, rect.Width);
				}
				rc = new Rectangle(0, 0, MulDiv(rcPhysicalView.Width, size.Width, rect.Width), MulDiv(rcPhysicalView.Height, size.Height, rect.Height));
				if (AutoScroll)
				{
					if ((rc.Width > sizeClient.Width) && !scrollX)
					{
						scrollX = bContinue = true;
					}
					if ((rc.Height > sizeClient.Height) && scrollY)
					{
						scrollY = bContinue = true;
					}
				}
			} while (bContinue);
			
			rc.Offset(-MulDiv(rect.Left - rcPhysicalView.Left, size.Width, rect.Width) + ((sizeClient.Width - size.Width) / 2), -MulDiv(rect.Top - rcPhysicalView.Top, size.Height, rect.Height) + ((sizeClient.Height - size.Height) / 2));
			
			SizeF sizeF = ImageSize; 
			double scaleFactor = (double)rc.Width / (double)sizeF.Width;
			if (scaleFactor > 0.0)
			{
				BeginUpdate();

				if (this.IsValidScaleFactor((float)scaleFactor) == true)
				{
					this.ScaleFactor = (float)scaleFactor;
					this.SizeMode = RasterViewerSizeMode.Normal;
					CalculateTransform();
				
					Point point = new Point(-rc.Left, -rc.Top);
					this.AutoScrollPosition = point;
					CalculateTransform();
				}
				
				EndUpdate();
			}
		}

		public virtual void ZoomToFit()
		{
           this.ZoomToFit(this.ImageSize);
		}

		public virtual void ZoomToFit(SizeF viewSize)
		{
			float scaleFactor = this.ScaleFactor;
			SizeF sizeFrame = this.FrameIsPartOfView ? this.FrameSize : SizeF.Empty;
			SizeF sizeFrameShadow = this.FrameShadowIsPartOfView ? this.FrameShadowSize : SizeF.Empty;
			
			float margin_left = this.BorderPadding.Left + sizeFrame.Width;
			float margin_top = this.BorderPadding.Top + sizeFrame.Height;
			float margin_right = (this.BorderPadding.Right + sizeFrame.Width) + sizeFrameShadow.Width;
			float margin_bottom = (this.BorderPadding.Bottom + sizeFrame.Height) + sizeFrameShadow.Height;

            SizeF clientSize = new Size(this.Width, this.Height);
			RectangleF rcClient = new RectangleF(margin_left, margin_top, clientSize.Width - (margin_left + margin_right), clientSize.Height - (margin_top + margin_bottom));
			
			if (rcClient.Width <= rcClient.Height)
			{
				scaleFactor = rcClient.Height / viewSize.Height;
				if ((scaleFactor * viewSize.Width) > rcClient.Width)
					scaleFactor = rcClient.Width / viewSize.Width;
			}
			
			scaleFactor = rcClient.Width / viewSize.Width;
			if ((scaleFactor * viewSize.Height) > rcClient.Height)
			{
				scaleFactor = rcClient.Height / viewSize.Height;
			}

			this.ScaleFactor = scaleFactor;
		}

		public virtual void ZoomActualSize()
		{
			this.ScaleFactor = 1.0F;
		}

		public virtual void ZoomOutAtPoint(Point pt)
		{
			const float defaultScale = 0.1F;
			float imageWidth = this.Image.Width;
			float imageHeight = this.Image.Height;

            // compute new scale factor
            using (Transformer transformer = this.Transformer)
            {
                // invoke begin update
                this.BeginUpdate();

                // center at point
                this.CenterAtPoint(pt);

                // retrieve client rectangle
                RectangleF srcClient = this.ClientRectangle;

                // calculate rectangle in logical coordinate
                srcClient = transformer.RectangleToLogical(srcClient);

                // inflate physical rectangle
                SizeF szDelta = new SizeF(srcClient.Width * defaultScale, srcClient.Height * defaultScale);
                srcClient.Inflate(szDelta);

                // rounding rectangle
                srcClient = new RectangleF((int)Math.Floor(srcClient.Left), (int)Math.Floor(srcClient.Top), (int)Math.Ceiling(srcClient.Width), (int)Math.Ceiling(srcClient.Height));

                // calculate rectangle in physical coordinate
                srcClient = transformer.RectangleToPhysical(srcClient);

                // rounding rectangle
                Rectangle rcZoomTo = new Rectangle((int)Math.Floor(srcClient.Left), (int)Math.Floor(srcClient.Top), (int)Math.Ceiling(srcClient.Width), (int)Math.Ceiling(srcClient.Height));
                this.ZoomToRectangle(rcZoomTo);

                // invoke end update
                this.EndUpdate();
            }
		}	

		public virtual void ZoomInAtPoint(Point pt)
		{
			const float defaultScale = 0.1F;

            using (Transformer transformer = this.Transformer)
            {
                this.BeginUpdate();

                // center at point
                this.CenterAtPoint(pt);

                // compute new scale factor
                RectangleF srcClient = transformer.RectangleToLogical(this.ClientRectangle);

                SizeF szDelta = new SizeF(-srcClient.Width * defaultScale, -srcClient.Height * defaultScale);
                srcClient.Inflate(szDelta);
                Rectangle rcZoomTo = Rectangle.Round(transformer.RectangleToPhysical(srcClient));
                this.ZoomToRectangle(rcZoomTo);

                this.EndUpdate();
            }
		}


        /// <summary>
        /// Pan view by dx and dy
        /// </summary>
        /// <param name="dx">horizontal offset in logical coordinate</param>
        /// <param name="dy">vertical offset in logical coordinate</param>
        public virtual void PanView(float dx, float dy)
        {
            int xOff = (int)(-this.AutoScrollPosition.X - dx);
            int yOff = (int)(-this.AutoScrollPosition.Y - dy);

            this.BeginUpdate();
            this.AutoScrollPosition = new Point(xOff, yOff);
            this.CalculateTransform();
            this.EndUpdate();
        }

		#endregion

		#region virtual routines

		protected virtual void OnRectangle(RasterViewerRectangleEventArgs e)
		{
			if (e.Cancel)
			{
				this.EraseControlPaintRectangle();
			}
			else
			{
				switch (e.Status)
				{
					case RasterViewerInteractiveStatus.Begin:
					{
						this._interactiveRectangle = e.Rectangle;
						this.BeginInteractiveMode(false);
						this.ControlPaintRectangle();
						break;
					}
					case RasterViewerInteractiveStatus.Working:
					{
						this.ControlPaintRectangle();
						this._interactiveRectangle = e.Rectangle;
						this.ControlPaintRectangle();
						break;
					}
					case RasterViewerInteractiveStatus.End:
					{
						Rectangle rcInteractive = this._interactiveRectangle;
						this.EndInteractiveMode();

						rcInteractive = DrawingHelper.FixRectangle(rcInteractive);
						if ((rcInteractive.Width > 0) && (rcInteractive.Height > 1))
						{
							switch (this.InteractiveMode)
							{
								case RasterViewerInteractiveMode.ZoomTo:
									this.ZoomToRectangle(rcInteractive);
									break;
								default:
									break;
							}
						}
						else
						{
							switch (this.InteractiveMode)
							{
								case RasterViewerInteractiveMode.ZoomTo:
									this.ZoomInAtPoint(new Point(rcInteractive.Left, rcInteractive.Top));
									break;
								default:
									break;
							}
						}
						break;
					}
				}
			}
		}
 
		protected virtual void OnPoints(RasterViewerPointsEventArgs e)
		{
			if (e.Cancel)
				this.EraseControlPaintPoints();
			else 
			{
				switch (e.Status)
				{
					case RasterViewerInteractiveStatus.Begin:
						this.BeginInteractiveMode(true);
						this.ControlPaintPoints();
						break;
					case RasterViewerInteractiveStatus.Working:
						this.ControlPaintPoints();
						break;
					case RasterViewerInteractiveStatus.End:
						this.EndInteractiveMode();
						break;
					default:
						break;
				}
			}
		}

		protected virtual void OnAutoDisposeImagesChanged(EventArgs e)
		{
			if (AutoDisposeImagesChanged != null)
				AutoDisposeImagesChanged(this, e);
		}

		protected virtual void OnAutoResetScaleFactorChanged(EventArgs e)
		{
			if (AutoResetScaleFactorChanged != null)
				AutoResetScaleFactorChanged(this, e);
		}

		protected virtual void OnAutoResetScrollPositionChanged(EventArgs e)
		{
			if (this.AutoResetScrollPositionChanged != null)
			{
				this.AutoResetScrollPositionChanged(this, e);
			}
		}
 
		protected virtual void OnBorderStyleChanged(EventArgs e)
		{
			UpdateStyles();
			Invalidate();
			if (BorderStyleChanged != null)
				BorderStyleChanged(this, e);
		}

		protected virtual void OnCenterModeChanged(EventArgs e)
		{
			this.CalculateTransform();
			base.Invalidate();
			if (this.CenterModeChanged != null)
			{
				this.CenterModeChanged(this, e);
			}
		}
 
		protected virtual void OnDoubleBufferChanged(EventArgs e)
		{
			base.Invalidate();
			if (this.DoubleBufferChanged != null)
			{
				this.DoubleBufferChanged(this, e);
			}
		}
 		
		protected virtual void OnImageChanged(EventArgs e)
		{
			this.BeginUpdate();
			if (this.AutoResetScaleFactor)
			{
				this.ScaleFactor = 1f;
			}
			if (this.AutoResetScrollPosition)
			{
				this.AutoScrollPosition = Point.Empty;
			}
			this.CalculateTransform();
			this.EndUpdate();
			
			if (this.ImageChanged != null)
			{
				this.ImageChanged(this, e);
			}
		}
 
		
		protected virtual void OnInteractiveCenterAt(RasterViewerPointEventArgs e)
		{
			if (InteractiveCenterAt != null)
				InteractiveCenterAt(this, e);
			if (!e.Cancel)
			{
				RasterViewerInteractiveStatus rasterViewerInteractiveStatus = e.Status;
				if (rasterViewerInteractiveStatus == RasterViewerInteractiveStatus.End)
					CenterAtPoint(e.Point);
			}
			if (((int)e.Status == 2) && !e.Cancel)
				OnInteractiveModeEnded(EventArgs.Empty);
		}
		
		protected virtual void OnInteractiveModeChanged(EventArgs e)
		{
			if (this.InteractiveModeChanged != null)
			{
				this.InteractiveModeChanged(this, e);
			}
		}
 
		protected virtual void OnInteractiveModeEnded(EventArgs e)
		{
			if (this.InteractiveModeEnded != null)
			{
				this.InteractiveModeEnded(this, e);
			}
		}

	
		protected virtual void OnInteractiveSelectByRectangle(RasterViewerRectangleEventArgs e)
		{
			if (this.InteractiveSelectByRectangle != null)
				this.InteractiveSelectByRectangle(this, e);
			
			this.OnRectangle(e);

			if ((e.Status == RasterViewerInteractiveStatus.End) && !e.Cancel)
			{
				this.OnInteractiveModeEnded(EventArgs.Empty);
			}
		}
 
		protected virtual void OnInteractivePan(RasterViewerLineEventArgs e)
		{
			if (this.InteractivePan != null)
			{
				this.InteractivePan(this, e);
			}

			if (!e.Cancel)
			{
				switch (e.Status)
				{
					case RasterViewerInteractiveStatus.Begin:
					{
						this.BeginInteractiveMode(false);
						this._interactivePoint = e.Begin;
						break;
					}
					case RasterViewerInteractiveStatus.Working:
					{
						Point point1 = new Point((-this.AutoScrollPosition.X + e.Begin.X) - e.End.X, (-this.AutoScrollPosition.Y + e.Begin.Y) - e.End.Y);
						this.BeginUpdate();
						this.AutoScrollPosition = point1;
						this.CalculateTransform();
						this.EndUpdate();
						this._interactivePoint = e.End;
						break;
					}
					case RasterViewerInteractiveStatus.End:
					{
						this.EndInteractiveMode();
						break;
					}
				}
			}

			if ((e.Status == RasterViewerInteractiveStatus.End) && !e.Cancel)
			{
				this.OnInteractiveModeEnded(EventArgs.Empty);
			}
		}
 
		protected virtual void OnInteractiveRegionCombineModeChanged(EventArgs e)
		{
			if (this.InteractiveRegionCombineModeChanged != null)
			{
				this.InteractiveRegionCombineModeChanged(this, e);
			}
		}
 
		protected virtual void OnInteractiveRegionRectangle(RasterViewerRectangleEventArgs e)
		{
			if (InteractiveRegionRectangle != null)
				InteractiveRegionRectangle(this, e);
			
			OnRectangle(e);

			if ((e.Status == RasterViewerInteractiveStatus.End) && !e.Cancel)
				OnInteractiveModeEnded(EventArgs.Empty);
		}

		protected virtual void OnInteractiveRegionEllipse(RasterViewerRectangleEventArgs e)
		{
			if (this.InteractiveRegionEllipse != null)
				this.InteractiveRegionEllipse(this, e);
			
			this.OnRectangle(e);

			if ((e.Status == RasterViewerInteractiveStatus.End) && !e.Cancel)
				this.OnInteractiveModeEnded(EventArgs.Empty);
		}
 
		protected virtual void OnInteractiveRegionFreehand(RasterViewerPointsEventArgs e)
		{
			if (this.InteractiveRegionFreehand != null)
				this.InteractiveRegionFreehand(this, e);
			
			this.OnPoints(e);
			
			if ((e.Status == RasterViewerInteractiveStatus.End) && !e.Cancel)
				this.OnInteractiveModeEnded(EventArgs.Empty);
		}
 
		protected virtual void OnInteractiveRegionTypeChanged(EventArgs e)
		{
			if (InteractiveRegionTypeChanged != null)
				InteractiveRegionTypeChanged(this, e);
		}

		protected virtual void OnInteractiveZoomTo(RasterViewerRectangleEventArgs e)
		{
			if (this.InteractiveZoomTo != null)
				this.InteractiveZoomTo(this, e);
			
			this.OnRectangle(e);

			if ((e.Status == RasterViewerInteractiveStatus.End) && !e.Cancel)
			{
				this.OnInteractiveModeEnded(EventArgs.Empty);
			}
		}

		protected virtual void OnInteractiveZoomOut(RasterViewerPointEventArgs e)
		{
			if (this.InteractiveZoomOut != null)
				this.InteractiveZoomOut(this, e);

			if (!e.Cancel)
			{
				RasterViewerInteractiveStatus rasterViewerInteractiveStatus = e.Status;
				if (rasterViewerInteractiveStatus == RasterViewerInteractiveStatus.End)
					this.ZoomOutAtPoint(e.Point);
			}

			if (((int)e.Status == 2) && !e.Cancel)
				OnInteractiveModeEnded(EventArgs.Empty);
		}
 

		protected virtual void OnInteractiveCustomPoint(RasterViewerPointEventArgs e)
		{
			if (this.InteractiveCustomPoint != null)
				this.InteractiveCustomPoint(this, e);

			switch (e.Status)
			{
				case RasterViewerInteractiveStatus.Begin:
				{
					this._interactivePoint = e.Point;
					this.BeginInteractiveMode(true);
					break;
				}
				case RasterViewerInteractiveStatus.Working:
				{
					break;
				}
				case RasterViewerInteractiveStatus.End:
				{
					this.EndInteractiveMode();
					break;
				}
			}
			
			if ((e.Status == RasterViewerInteractiveStatus.End) && !e.Cancel)
				this.OnInteractiveModeEnded(EventArgs.Empty);
		}

		protected virtual void OnInteractiveCustomLine(RasterViewerLineEventArgs e)
		{
			if (this.InteractiveCustomLine != null)
				this.InteractiveCustomLine(this, e);

			switch (e.Status)
			{
				case RasterViewerInteractiveStatus.Begin:
				{
					this.BeginInteractiveMode(true);
					break;
				}
				case RasterViewerInteractiveStatus.Working:
				{
					break;
				}
				case RasterViewerInteractiveStatus.End:
				{
					this.EndInteractiveMode();
					break;
				}
			}
			
			if ((e.Status == RasterViewerInteractiveStatus.End) && !e.Cancel)
				this.OnInteractiveModeEnded(EventArgs.Empty);
		}

		protected virtual void OnInteractiveCustomRectangle(RasterViewerRectangleEventArgs e)
		{
			if (this.InteractiveCustomRectangle != null)
				this.InteractiveCustomRectangle(this, e);

			switch (e.Status)
			{
				case RasterViewerInteractiveStatus.Begin:
				{
					this.BeginInteractiveMode(true);
					this._interactiveRectangle = e.Rectangle;
					break;
				}
				case RasterViewerInteractiveStatus.Working:
				{
					break;
				}
				case RasterViewerInteractiveStatus.End:
				{
					this.EndInteractiveMode();
					break;
				}
			}
			
			if ((e.Status == RasterViewerInteractiveStatus.End) && !e.Cancel)
				this.OnInteractiveModeEnded(EventArgs.Empty);
		}

		protected virtual void OnInteractiveCustomPoints(RasterViewerPointsEventArgs e)
		{
			if (this.InteractiveCustomPoints != null)
				this.InteractiveCustomPoints(this, e);

			switch (e.Status)
			{
				case RasterViewerInteractiveStatus.Begin:
				{
					this.BeginInteractiveMode(true);
					break;
				}
				case RasterViewerInteractiveStatus.Working:
				{
					break;
				}
				case RasterViewerInteractiveStatus.End:
				{
					this.EndInteractiveMode();
					break;
				}
			}
			
			if ((e.Status == RasterViewerInteractiveStatus.End) && !e.Cancel)
				this.OnInteractiveModeEnded(EventArgs.Empty);
		}


		protected virtual void OnMagnifyGlassChanged(EventArgs e)
		{
			if (MagnifyGlassChanged != null)
				MagnifyGlassChanged(this, e);
		}

		protected virtual void OnPostViewPaint(PaintEventArgs e)
		{
			if (this.IsImageAvailable && ((this.FrameSize != SizeF.Empty) || (this.FrameShadowSize != SizeF.Empty)))
			{
				PointF[] points;
				SizeF imageSize = this.ImageSize;
				SizeF frameSize = new SizeF(this.FrameSize.Width / this.CurrentXScaleFactor, this.FrameSize.Height / this.CurrentYScaleFactor);
				SizeF frameShadowSize = new SizeF(this.FrameShadowSize.Width / this.CurrentXScaleFactor, this.FrameShadowSize.Height / this.CurrentYScaleFactor);
				
				SmoothingMode mode = e.Graphics.SmoothingMode;
				if (frameShadowSize != SizeF.Empty)
				{
					using (Brush brush = new SolidBrush(this.FrameShadowColor))
					{
						points = new PointF[2] { new PointF(imageSize.Width + frameSize.Width, frameShadowSize.Height), new PointF((imageSize.Width + frameSize.Width) + frameShadowSize.Width, (imageSize.Height + frameSize.Height) + frameShadowSize.Height) } ;
						PointF[] pts = points;
						e.Graphics.FillRectangle(brush, RectangleF.FromLTRB(pts[0].X, pts[0].Y, pts[1].X, pts[1].Y));
						points = new PointF[2] { new PointF(frameShadowSize.Width, imageSize.Height + frameSize.Height), new PointF(imageSize.Width + frameSize.Width, (imageSize.Height + frameSize.Height) + frameShadowSize.Height) } ;
						pts = points;
						e.Graphics.FillRectangle(brush, RectangleF.FromLTRB(pts[0].X, pts[0].Y, pts[1].X, pts[1].Y));
					}
				}
				if (frameSize != SizeF.Empty)
				{
					using (Brush brush = new SolidBrush(this.FrameColor))
					{
						points = new PointF[2] { new PointF(-frameSize.Width, -frameSize.Height), new PointF(imageSize.Width + frameSize.Width, 0f) } ;
						PointF[] pts = points;
						e.Graphics.FillRectangle(brush, RectangleF.FromLTRB(pts[0].X, pts[0].Y, pts[1].X, pts[1].Y));
						points = new PointF[2] { new PointF(imageSize.Width, -frameSize.Height), new PointF(imageSize.Width + frameSize.Width, imageSize.Height + frameSize.Height) } ;
						pts = points;
						e.Graphics.FillRectangle(brush, RectangleF.FromLTRB(pts[0].X, pts[0].Y, pts[1].X, pts[1].Y));
						points = new PointF[2] { new PointF(-frameSize.Width, imageSize.Height), new PointF(imageSize.Width + frameSize.Width, imageSize.Height + frameSize.Height) } ;
						pts = points;
						e.Graphics.FillRectangle(brush, RectangleF.FromLTRB(pts[0].X, pts[0].Y, pts[1].X, pts[1].Y));
						points = new PointF[2] { new PointF(-frameSize.Width, -frameSize.Height), new PointF(0f, imageSize.Height + frameSize.Height) } ;
						pts = points;
						e.Graphics.FillRectangle(brush, RectangleF.FromLTRB(pts[0].X, pts[0].Y, pts[1].X, pts[1].Y));
					}
				}

				if (mode != e.Graphics.SmoothingMode)
				{
					e.Graphics.SmoothingMode = mode;
				}
			}
			if (this.PostViewPaint != null)
			{
				this.PostViewPaint(this, e);
			}
		} 
	
		protected virtual void OnPaint(PaintEventArgs e, RectangleF src, RectangleF srcClip, RectangleF dest, RectangleF destClip)
		{
//			if (IsImageAvailable)
//			{
//				if (Image != null)
//				{
//					RasterPaintProperties rasterPaintProperties = PaintProperties;
//					rasterPaintProperties.UseDpi = false;
//					rasterPaintProperties.UseClipWithGdiEngine = false;
//					Image.Paint(e.Graphics, src, srcClip, dest, destClip, rasterPaintProperties);
//				}
//				if (AnimateRegion && Image != null && Image.HasRegion)
//					DrawFramedRegion(e.Graphics);
//				if (AnimateFloater && IsFloaterVisible && !IsMovingFloater)
//					DrawFramedFloater(e.Graphics);
//			}
		}

		protected virtual void OnPaintPropertiesChanged(EventArgs e)
		{
			Invalidate();
			if (PaintPropertiesChanged != null)
				PaintPropertiesChanged(this, e);
		}

		protected virtual void OnPostTransformPaint(PaintEventArgs e)
		{
			if (PostTransformPaint != null)
				PostTransformPaint(this, e);
		}

		protected virtual void OnPreTransformPaint(PaintEventArgs e)
		{
			if (this.PreTransformPaint != null)
			{
				this.PreTransformPaint(this, e);
			}
		}
 
		protected virtual void OnPreViewPaint(PaintEventArgs e)
		{
			if (this.PreViewPaint != null)
			{
				this.PreViewPaint(this, e);
			}
		}
 
		protected virtual void OnScaleFactorChanged(EventArgs e)
		{
			this.CalculateTransform();
			base.Invalidate();
			if (this.ScaleFactorChanged != null)
			{
				this.ScaleFactorChanged(this, e);
			}
		}
 
		protected virtual void OnScroll(EventArgs e)
		{
			this.OnPreScroll(e);

			PointF[] pts = null;
			
			// fix bug of drawing interactive rectangle
			if (this.IsInteractiveModeBusy)
			{
				if (this._interactiveRectangle != Rectangle.Empty)
				{
					// convert physical rectangle to logical rectangle
					pts = new PointF[] {_ptMouseDown, _ptMouseMove};
					using (Transformer transformer = this.Transformer)
                        pts = transformer.PointToLogical(pts);

					// clear old drawn interactive rectangle
					this.EraseControlPaintRectangle();
				}
			}

			// calculates transform
			this.CalculateTransform();

			// refresh drawing
			base.Invalidate();

			// raise scroll event
			if (this.Scroll != null)
			{
				this.Scroll(this, EventArgs.Empty);
			}

			// update interactive rectangle
			if (this.IsInteractiveModeBusy)
			{
				if (pts != null)
				{
					// convert point from logical to physical 
					using (Transformer transformer = this.Transformer)
                        pts = transformer.PointToPhysical(pts);
			
					// update interactive rectangle
					_interactiveRectangle = Rectangle.Round(RectangleF.FromLTRB(pts[0].X, pts[0].Y, pts[1].X, pts[1].Y));
				}
			}

			this.OnPostScroll(e);
		}

		protected virtual void OnPreScroll(EventArgs e)
		{

		}

		protected virtual void OnPostScroll(EventArgs e)
		{
		}
 		
		protected virtual void OnSizeModeChanged(EventArgs e)
		{
			CalculateTransform();
			Invalidate();
			if (SizeModeChanged != null)
				SizeModeChanged(this, e);
		}

		protected virtual void OnTransformChanged(EventArgs e)
		{
			if (TransformChanged != null)
				TransformChanged(this, null);
		}


		protected virtual void OnFrameColorChanged(EventArgs e)
		{
			base.Invalidate();
			if (this.FrameColorChanged != null)
			{
				this.FrameColorChanged(this, e);
			}
		}
		 
		protected virtual void OnFrameIsPartOfViewChanged(EventArgs e)
		{
			CalculateTransform();
			Invalidate();
			if (FrameIsPartOfViewChanged != null)
				FrameIsPartOfViewChanged(this, e);
		}
				
		protected virtual void OnFrameShadowColorChanged(EventArgs e)
		{
			base.Invalidate();
			if (this.FrameShadowColorChanged != null)
			{
				this.FrameShadowColorChanged(this, e);
			}
		}
				 
		protected virtual void OnFrameShadowIsPartOfViewChanged(EventArgs e)
		{
			CalculateTransform();
			Invalidate();
			if (FrameShadowIsPartOfViewChanged != null)
				FrameShadowIsPartOfViewChanged(this, e);
		}
				
		protected virtual void OnFrameShadowSizeChanged(EventArgs e)
		{
			this.CalculateTransform();
			base.Invalidate();
			if (this.FrameShadowSizeChanged != null)
			{
				this.FrameShadowSizeChanged(this, e);
			}
		}
				 
		protected virtual void OnFrameSizeChanged(EventArgs e)
		{
			CalculateTransform();
			Invalidate();
			if (FrameSizeChanged != null)
				FrameSizeChanged(this, e);
		}
		

		#endregion

		#region override routines
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams paramerters = base.CreateParams;
				switch (this.BorderStyle)
				{
					case BorderStyle.None:
					{
						paramerters.Style &= -8388609;
						paramerters.ExStyle &= -513;
						return paramerters;
					}
					case BorderStyle.FixedSingle:
					{
						paramerters.Style |= 0x800000;
						paramerters.ExStyle &= -513;
						return paramerters;
					}
					case BorderStyle.Fixed3D:
					{
						paramerters.Style &= -8388609;
						paramerters.ExStyle |= 0x200;
						return paramerters;
					}
				}
				return paramerters;
			}
		}
 

		protected override void OnLostFocus(EventArgs e)
		{
			this.CancelInteractiveMode();
			base.OnLostFocus(e);
		}
 
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.Focus();

			#region handle mouse down event 

			if (this.IsImageAvailable)
			{
				if ((e.Button & MouseButtons.Left) == MouseButtons.Right)
				{
					if (this.IsInteractiveModeBusy)
					{
						this.CancelInteractiveMode();
					}
				}
				else
				{
					if (this.IsInteractiveModeBusy)
					{
						this.CancelInteractiveMode();
					}
					else
					{
						if (e.Button == MouseButtons.Middle)
						{
							this.PushInteractiveMode();
							this.InteractiveMode = RasterViewerInteractiveMode.Pan;
						}
						switch (this.InteractiveMode)
						{
							case RasterViewerInteractiveMode.Select:
							{
								#region interactive selection
								//RasterViewerRectangleEventArgs args = new RasterViewerRectangleEventArgs(RasterViewerInteractiveStatus.Begin, new Rectangle(e.X, e.Y, 0, 0));
								//this.OnInteractiveSelectByRectangle(args);
								break;
								#endregion 
							}
							case RasterViewerInteractiveMode.Pan:
							{
								#region interactive Pan View
								if (base.HScroll || base.VScroll)
								{
									Point point = new Point(e.X, e.Y);
									if (this.PhysicalViewRectangle.Contains(point))
									{
										RasterViewerLineEventArgs args = new RasterViewerLineEventArgs(RasterViewerInteractiveStatus.Begin, point, point);
										this.OnInteractivePan(args);
									}
								}
								break;
								#endregion
							}
							case RasterViewerInteractiveMode.CenterAt:
							{
								#region interactive Center view at point
								Rectangle rcPhysical = this.PhysicalViewRectangle;
								Point point = new Point(e.X, e.Y);
								if (rcPhysical.Contains(point))
								{
									RasterViewerPointEventArgs args = new RasterViewerPointEventArgs(RasterViewerInteractiveStatus.End, point);
									this.OnInteractiveCenterAt(args);
								}
								break;
								#endregion
							}
							case RasterViewerInteractiveMode.ZoomTo:
							{
								#region interactive zoom in rectangle
								Rectangle rcPhysical = this.PhysicalViewRectangle;
								Point point = new Point(e.X, e.Y);
								if (rcPhysical.Contains(point))
								{
									RasterViewerRectangleEventArgs args = new RasterViewerRectangleEventArgs(RasterViewerInteractiveStatus.Begin, new Rectangle(point.X, point.Y, 0, 0));
									this.OnInteractiveZoomTo(args);
								}
								break;
								#endregion
							}
							case RasterViewerInteractiveMode.ZoomOut:
							{
								#region interactive zoom out
								Rectangle rcPhysical = this.PhysicalViewRectangle;
								Point point = new Point(e.X, e.Y);
								if (rcPhysical.Contains(point))
								{
									RasterViewerPointEventArgs args = new RasterViewerPointEventArgs(RasterViewerInteractiveStatus.End, point);
									this.OnInteractiveZoomOut(args);
								}
								break;
								#endregion
							}
							case RasterViewerInteractiveMode.Region:
							{
								#region interactive region handler

								switch (this.InteractiveRegionType)
								{
									case RasterViewerInteractiveRegionType.Rectangle:
									case RasterViewerInteractiveRegionType.Ellipse:
									{
										Rectangle rcPhysical = this.PhysicalViewRectangle;
										Point point = new Point(e.X, e.Y);
										if (rcPhysical.Contains(point))
										{
											RasterViewerRectangleEventArgs args = new RasterViewerRectangleEventArgs(RasterViewerInteractiveStatus.Begin, new Rectangle(point.X, point.Y, 0, 0));
											if (this.InteractiveRegionType != RasterViewerInteractiveRegionType.Rectangle)
												this.OnInteractiveRegionEllipse(args);
											else
												this.OnInteractiveRegionRectangle(args);
										}
										break;
									}
									case RasterViewerInteractiveRegionType.Freehand:
									{
										Rectangle rcPhysical = this.PhysicalViewRectangle;
										Point point = new Point(e.X, e.Y);
										if (rcPhysical.Contains(point))
										{
											this._interactivePoints.Clear();
											this._interactivePoints.Add(point);
											RasterViewerPointsEventArgs args = new RasterViewerPointsEventArgs(RasterViewerInteractiveStatus.Begin, this._interactivePoints);
											this.OnInteractiveRegionFreehand(args);
										}
										break;
									}

									default:
										break;
								}
								break;

								#endregion
							}
							case RasterViewerInteractiveMode.CustomPoint:
							{
								#region interactive custom point
								Rectangle rcPhysical = this.PhysicalViewRectangle;
								Point point = new Point(e.X, e.Y);
								if (rcPhysical.Contains(point))
								{
									RasterViewerPointEventArgs args = new RasterViewerPointEventArgs(RasterViewerInteractiveStatus.Begin, point);
									this.OnInteractiveCustomPoint(args);
								}
								break;
								#endregion
							}
							case RasterViewerInteractiveMode.CustomLine:
							{
								#region interactive custom point
								Rectangle rcPhysical = this.PhysicalViewRectangle;
								Point point = new Point(e.X, e.Y);
								if (rcPhysical.Contains(point))
								{
									RasterViewerLineEventArgs args = new RasterViewerLineEventArgs(RasterViewerInteractiveStatus.Begin, point, point);
									this.OnInteractiveCustomLine(args);
								}
								break;
								#endregion
							}
							case RasterViewerInteractiveMode.CustomRectangle:
							{
								#region interactive custom rectangle
								Rectangle rcPhysical = this.PhysicalViewRectangle;
								Point point = new Point(e.X, e.Y);
								if (rcPhysical.Contains(point))
								{
									RasterViewerRectangleEventArgs args = new RasterViewerRectangleEventArgs(RasterViewerInteractiveStatus.Begin, new Rectangle(point, Size.Empty));
									this.OnInteractiveCustomRectangle(args);
								}
								break;
								#endregion
							}
							case RasterViewerInteractiveMode.CustomPoints:
							{
								#region interactive custom rectangle
								Rectangle rcPhysical = this.PhysicalViewRectangle;
								Point point = new Point(e.X, e.Y);
								if (rcPhysical.Contains(point))
								{
									this._interactivePoints.Clear();
									this._interactivePoints.Add(point);

									RasterViewerPointsEventArgs args = new RasterViewerPointsEventArgs(RasterViewerInteractiveStatus.Begin, _interactivePoints);
									this.OnInteractiveCustomPoints(args);
								}
								break;
								#endregion
							}

							default:
								break;
						}
					}
				}
			}

			#endregion

			base.OnMouseDown(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			// initialize mouse event args
			this._mouseArgs = e;

			// check interactive mode
			if (this.IsInteractiveModeBusy)
			{
				switch (this.InteractiveMode)
				{
					case RasterViewerInteractiveMode.Select:
					{
						#region interactive selection
						//Rectangle rcMouse = Rectangle.FromLTRB(this._interactiveRectangle.Left, this._interactiveRectangle.Top, e.X, e.Y);
						//RasterViewerRectangleEventArgs args = new RasterViewerRectangleEventArgs(RasterViewerInteractiveStatus.Working, rcMouse);
						//this.OnInteractiveSelectByRectangle(args);
						break;
						#endregion
					}
					case RasterViewerInteractiveMode.Pan:
					{
						#region interactive Pan
						if (((this._interactivePoint.X - e.X) != 0) || ((this._interactivePoint.Y - e.Y) != 0))
						{
							RasterViewerLineEventArgs args = new RasterViewerLineEventArgs(RasterViewerInteractiveStatus.Working, this._interactivePoint, new Point(e.X, e.Y));
							this.OnInteractivePan(args);
						}
						break;
						#endregion
					}
					case RasterViewerInteractiveMode.ZoomTo:
					{
						#region interactive ZoomTo
						Rectangle rcMouse = Rectangle.FromLTRB(this._interactiveRectangle.Left, this._interactiveRectangle.Top, e.X, e.Y);
						RasterViewerRectangleEventArgs args = new RasterViewerRectangleEventArgs(RasterViewerInteractiveStatus.Working, rcMouse);
						this.OnInteractiveZoomTo(args);
						break;
						#endregion
					}
					case RasterViewerInteractiveMode.Region:
					{
						#region interactive region
						switch (this.InteractiveRegionType)
						{
							case RasterViewerInteractiveRegionType.Rectangle:
							case RasterViewerInteractiveRegionType.Ellipse:
							{
								Rectangle rect = Rectangle.FromLTRB(this._interactiveRectangle.Left, this._interactiveRectangle.Top, e.X, e.Y);
								RasterViewerRectangleEventArgs arg = new RasterViewerRectangleEventArgs(RasterViewerInteractiveStatus.Working, rect);
								if (this.InteractiveRegionType == RasterViewerInteractiveRegionType.Rectangle)
									this.OnInteractiveRegionRectangle(arg);
								else
									this.OnInteractiveRegionEllipse(arg);

								break;
							}
							case RasterViewerInteractiveRegionType.Freehand:
							{
								if (this._interactivePoints.Count > 0)
								{
									Point pt1 = new Point(e.X, e.Y);
									Point pt2 = this._interactivePoints[this._interactivePoints.Count - 1];
									if (pt2.X == pt1.X)
									{
										pt2 = this._interactivePoints[this._interactivePoints.Count - 1];
										if (pt2.Y == pt1.Y)
											break;
									}

									this._interactivePoints.Add(pt1);
									
									RasterViewerPointsEventArgs args = new RasterViewerPointsEventArgs(RasterViewerInteractiveStatus.Working, this._interactivePoints);
									this.OnInteractiveRegionFreehand(args);
								}
								break;
							}
						}
						break;
						#endregion
					}
					case RasterViewerInteractiveMode.CustomPoint:
					{
						#region interactive custom point
						
						if (((this._interactivePoint.X - e.X) != 0) || ((this._interactivePoint.Y - e.Y) != 0))
						{
							RasterViewerPointEventArgs args = new RasterViewerPointEventArgs(RasterViewerInteractiveStatus.Working, new Point(e.X, e.Y));
							this.OnInteractiveCustomPoint(args);
						}

						break;
						#endregion
					}
					case RasterViewerInteractiveMode.CustomLine:
					{
						#region interactive custom line
						if (((this._interactivePoint.X - e.X) != 0) || ((this._interactivePoint.Y - e.Y) != 0))
						{
							RasterViewerLineEventArgs args = new RasterViewerLineEventArgs(RasterViewerInteractiveStatus.Working, this._interactivePoint, new Point(e.X, e.Y));
							this.OnInteractiveCustomLine(args);
						}
						break;
						#endregion
					}
					case RasterViewerInteractiveMode.CustomRectangle:
					{
						#region interactive custom rectangle
						Rectangle rect = Rectangle.FromLTRB(this._interactiveRectangle.Left, this._interactiveRectangle.Top, e.X, e.Y);
						RasterViewerRectangleEventArgs arg = new RasterViewerRectangleEventArgs(RasterViewerInteractiveStatus.Working, rect);
						this.OnInteractiveCustomRectangle(arg);
						break;
						#endregion
					}
					case RasterViewerInteractiveMode.CustomPoints:
					{
						#region interactive custom points
						if (this._interactivePoints.Count > 0)
						{
							Point pt1 = new Point(e.X, e.Y);
							Point pt2 = this._interactivePoints[this._interactivePoints.Count - 1];
							if (pt2.X == pt1.X)
							{
								pt2 = this._interactivePoints[this._interactivePoints.Count - 1];
								if (pt2.Y == pt1.Y)
									break;
							}

							this._interactivePoints.Add(pt1);
									
							RasterViewerPointsEventArgs args = new RasterViewerPointsEventArgs(RasterViewerInteractiveStatus.Working, this._interactivePoints);
							this.OnInteractiveCustomPoints(args);
						}
						break;
						#endregion
					}

					default:
						break;
				}
			}

			base.OnMouseMove(e);
		}
 
		protected override void OnMouseUp(MouseEventArgs e)
		{
			// check interactive mode
			if (this.IsInteractiveModeBusy)
			{
				switch (this.InteractiveMode)
				{
					case RasterViewerInteractiveMode.Select:
					{
						#region interactive selection
						//RasterViewerRectangleEventArgs args = new RasterViewerRectangleEventArgs(RasterViewerInteractiveStatus.End, this._interactiveRectangle);
						//this.OnInteractiveSelectByRectangle(args);
						break;
						#endregion
					}
					case RasterViewerInteractiveMode.Pan:
					{
						#region RasterViewerInteractiveMode.Pan
						RasterViewerLineEventArgs args = new RasterViewerLineEventArgs(RasterViewerInteractiveStatus.End, this._interactivePoint, Point.Empty);
						this.OnInteractivePan(args);
						if ( e.Button == MouseButtons.Middle )
						{
							this.InteractiveMode = this.PopInteractiveMode();
						}
						break;	
						#endregion
					}
					case RasterViewerInteractiveMode.ZoomTo:
					{
						#region RasterViewerInteractiveMode.ZoomTo
						RasterViewerRectangleEventArgs args = new RasterViewerRectangleEventArgs(RasterViewerInteractiveStatus.End, this._interactiveRectangle);
						this.OnInteractiveZoomTo(args);
						break;	
						#endregion
					}
					case RasterViewerInteractiveMode.Region:
					{
						#region RasterViewerInteractiveMode.Region
						switch (this.InteractiveRegionType)
						{
							case RasterViewerInteractiveRegionType.Rectangle:
							case RasterViewerInteractiveRegionType.Ellipse:
							{
								RasterViewerRectangleEventArgs args = new RasterViewerRectangleEventArgs(RasterViewerInteractiveStatus.End, this._interactiveRectangle);
								if (this.InteractiveRegionType == RasterViewerInteractiveRegionType.Rectangle)
									this.OnInteractiveRegionRectangle(args);
								else
									this.OnInteractiveRegionEllipse(args);
								break;
							}
							case RasterViewerInteractiveRegionType.Freehand:
							{
								RasterViewerPointsEventArgs args = new RasterViewerPointsEventArgs(RasterViewerInteractiveStatus.End, this._interactivePoints);
								this.OnInteractiveRegionFreehand(args);
								break;
							}
						}
						break;
						#endregion
					}
					case RasterViewerInteractiveMode.CustomPoint:
					{
						#region interactive custom point
						RasterViewerPointEventArgs args = new RasterViewerPointEventArgs(RasterViewerInteractiveStatus.End, _interactivePoint);
						this.OnInteractiveCustomPoint(args);						
						break;
						#endregion
					}
					case RasterViewerInteractiveMode.CustomLine:
					{
						#region interactive custom line
						RasterViewerLineEventArgs args = new RasterViewerLineEventArgs(RasterViewerInteractiveStatus.End, this._interactivePoint, new Point(e.X, e.Y));
						this.OnInteractiveCustomLine(args);
						break;
						#endregion
					}
					case RasterViewerInteractiveMode.CustomRectangle:
					{
						#region interactive custom rectangle
						Rectangle rect = Rectangle.FromLTRB(this._interactiveRectangle.Left, this._interactiveRectangle.Top, e.X, e.Y);
						RasterViewerRectangleEventArgs arg = new RasterViewerRectangleEventArgs(RasterViewerInteractiveStatus.End, rect);
						this.OnInteractiveCustomRectangle(arg);
						break;
						#endregion
					}
					case RasterViewerInteractiveMode.CustomPoints:
					{
						#region interactive custom points
						RasterViewerPointsEventArgs args = new RasterViewerPointsEventArgs(RasterViewerInteractiveStatus.End, this._interactivePoints);
						this.OnInteractiveCustomPoints(args);
						break;
						#endregion
					}
					default:
						break;
				}
			}

			base.OnMouseUp(e);
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			if ( !this.IsInteractiveModeBusy )
			{
				Rectangle rcPhysical = this.PhysicalViewRectangle;
				Point point = new Point(e.X, e.Y);
				if (rcPhysical.Contains(point))
				{
					if ( e.Delta < 0 )
					{
						this.ZoomOutAtPoint(point);
					}
					else
					{
						this.ZoomInAtPoint(point);
					}
				}
			}
			base.OnMouseWheel(e);
		}
 

		protected override void OnPaint(PaintEventArgs e)
		{
			if (this._ignorePaintCounter <= 0)
			{
                Graphics graph = e.Graphics;

                RectangleF rcSource, rcSrcClip, rcDest, rcDstClip;

                using (Matrix transform = this.Transform)
                {
                    // invoke pre-transform paint
                    this.OnPreTransformPaint(e);

                    // save current graph state
                    GraphicsState state = graph.Save();

                    // apply transformation
                    graph.MultiplyTransform(transform);

                    // invoke pre-view paint
                    this.OnPreViewPaint(e);

                    // invoke post-view paint
                    this.OnPostViewPaint(e);

                    // restore last state
                    graph.Restore(state);

                    if (this.Image != null)
                        rcSource = new RectangleF(0f, 0f, (float)this.Image.Width, (float)this.Image.Height);
                    else
                        rcSource = RectangleF.Empty;

                    rcSrcClip = rcSource;

                    SizeF sizeImage = this.ImageSize;                    
                    using (Transformer transformer = this.Transformer)
                        rcDest = transformer.RectangleToPhysical(rcSource);

                    rcDstClip = (RectangleF)e.ClipRectangle;
                    rcDstClip.Inflate(1, 1);

                    this.OnPaint(e, rcSource, rcSrcClip, rcDest, rcDstClip);

#if DISPLAY_PIXEL_GRID
				    // render pixel grid
				    this.OnPaintGrid(e, rcSource);

				    // render mouse location in pixel grid
				    this.OnPaintMouseLoc(e);
#endif

                    // invoke post transform paint
                    this.OnPostTransformPaint(e);

                    base.OnPaint(e);
                }
			}
		}
 
		protected override void OnSizeChanged(EventArgs e)
		{
			if (_ignoreSizeChangedCounter <= 0)
				CalculateTransform();
			base.OnSizeChanged(e);
		}

		protected override void WndProc(ref Message m)
		{
			switch (m.Msg)
			{
				case NativeMethods.WM_LBUTTONDOWN:
				case NativeMethods.WM_MBUTTONDOWN:
				case NativeMethods.WM_RBUTTONDOWN:
					if (this.IsInteractiveModeBusy == false)
						_ptMouseDown = new Point(NativeMethods.LOWORD(m.LParam.ToInt32()), NativeMethods.HIWORD(m.LParam.ToInt32()));
					break;
				case NativeMethods.WM_MOUSEMOVE:
					if (this.IsInteractiveModeBusy == true)
						_ptMouseMove = new Point(NativeMethods.LOWORD(m.LParam.ToInt32()), NativeMethods.HIWORD(m.LParam.ToInt32()));
					break;
				case NativeMethods.WM_LBUTTONUP:
				case NativeMethods.WM_MBUTTONUP:
				case NativeMethods.WM_RBUTTONUP:
					if (this.IsInteractiveModeBusy == true)
						_ptMouseUp = new Point(NativeMethods.LOWORD(m.LParam.ToInt32()), NativeMethods.HIWORD(m.LParam.ToInt32()));
					break;
				default:
					break;
			}

			base.WndProc(ref m);
			

			if ((m.Msg == 276) || (m.Msg == 277) || (m.Msg == 522))
				OnScroll(EventArgs.Empty);
		}

		#endregion

		#region internal helpers

		private void InitClass()
		{
			base.SetStyle(ControlStyles.ResizeRedraw, true);
			base.SetStyle(ControlStyles.ContainerControl, true);
			base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			base.SetStyle(ControlStyles.UserPaint, true);
			base.SetStyle(ControlStyles.DoubleBuffer, true);

			this._borderStyle = BorderStyle.Fixed3D;
			this._scaleFactor = 1f;
            
            lock (_syncTransform)
                this._transform = new Matrix();

			this._borderPadding = new ViewerBorderPadding();
			this._autoResetScaleFactor = true;
			this._autoResetScrollPosition = true;
			this._sizeMode = RasterViewerSizeMode.Normal;
			this._centerMode = RasterViewerCenterMode.None;
			this._frameSize = SizeF.Empty;
			this._frameColor = Color.Black;
			this._frameIsPartOfView = true;
			this._frameShadowSize = SizeF.Empty;
			this._frameShadowIsPartOfView = true;
			this._frameShadowColor = Color.FromArgb(0x80, 0, 0, 0);
			this._ignorePaintCounter = 0;
			this._ignoreSizeChangedCounter = 0;
			this._currentXScaleFactor = this._scaleFactor;
			this._currentYScaleFactor = this._scaleFactor;
			this._interactiveMode = RasterViewerInteractiveMode.Select;
			this._isInteractiveModeBusy = false;
			this._isCursorClipped = false;
			this._interactivePoint = Point.Empty;
			this._interactiveRectangle = Rectangle.Empty;
			this._interactiveRegionType = RasterViewerInteractiveRegionType.Rectangle;
			this._interactiveRectangleDrawn = false;
			this._interactivePoints = new RasterPointCollection();
			
			base.UpdateStyles();

			this.AutoScroll = true;
			this._borderPadding.Changed += new EventHandler(this.BorderPadding_Changed);
			this._image = null;
			this._autoDisposeImages = true;
			
            base.Size = new Size(0x80, 0x80);
		}
 
		private void InitializeComponent()
		{
			components = new Container();
		}


		private void BeginClipCursor()
		{
			Rectangle rectangle1 = Rectangle.Intersect(DrawingHelper.FixRectangle(this.PhysicalViewRectangle), base.ClientRectangle);
			rectangle1 = base.RectangleToScreen(rectangle1);
			for (Control control1 = base.Parent; control1 != null; control1 = control1.Parent)
			{
				rectangle1 = Rectangle.Intersect(rectangle1, base.Parent.RectangleToScreen(base.Parent.ClientRectangle));
				if (control1 is Form)
				{
					Form form1 = control1 as Form;
					if (form1.IsMdiChild && (form1.Owner != null))
					{
						rectangle1 = Rectangle.Intersect(rectangle1, form1.Owner.RectangleToScreen(form1.Owner.ClientRectangle));
					}
				}
			}

			this._rcClip = Cursor.Clip;
			Cursor.Clip = rectangle1;
			this._isCursorClipped = true;
		}
 
		private void EndClipCursor()
		{
			if (_isCursorClipped)
			{
				Cursor.Clip = _rcClip;
				_isCursorClipped = false;
				_rcClip = Rectangle.Empty;
			}
		}

		protected virtual void BeginInteractiveMode(bool clipCursor)
		{
			this._isInteractiveModeBusy = true;
			base.Capture = true;
			if (clipCursor)
			{
				this.BeginClipCursor();
			}
		}
 
		protected virtual void EndInteractiveMode()
		{
			if (IsInteractiveModeBusy)
			{
				Capture = false;
				_isInteractiveModeBusy = false;
				EraseControlPaintRectangle();
				EraseControlPaintPoints();
				EndClipCursor();
			}
		}

		protected virtual void CancelInteractiveMode()
		{
			if (this.IsInteractiveModeBusy)
			{
				this.EndInteractiveMode();
				switch (this.InteractiveMode)
				{
					case RasterViewerInteractiveMode.Pan:
					{
						RasterViewerLineEventArgs args = new RasterViewerLineEventArgs(RasterViewerInteractiveStatus.End, this._interactivePoint, Point.Empty);
						args.Cancel = true;
						this.OnInteractivePan(args);
						return;
					}
					case RasterViewerInteractiveMode.CenterAt:
					case RasterViewerInteractiveMode.ZoomTo:
					case RasterViewerInteractiveMode.ZoomOut:
					{
						return;
					}
					case RasterViewerInteractiveMode.Region:
					{
						switch (this.InteractiveRegionType)
						{
							case RasterViewerInteractiveRegionType.Rectangle:
							case RasterViewerInteractiveRegionType.Ellipse:
							{
								RasterViewerRectangleEventArgs args3 = new RasterViewerRectangleEventArgs(RasterViewerInteractiveStatus.End, this._interactiveRectangle);
								args3.Cancel = true;
								if (this.InteractiveRegionType == RasterViewerInteractiveRegionType.Rectangle)
								{
									this.OnInteractiveRegionRectangle(args3);
									return;
								}
								this.OnInteractiveRegionEllipse(args3);
								return;
							}
							case RasterViewerInteractiveRegionType.Freehand:
							{
								RasterViewerPointsEventArgs args4 = new RasterViewerPointsEventArgs(RasterViewerInteractiveStatus.End, this._interactivePoints);
								args4.Cancel = true;
								this.OnInteractiveRegionFreehand(args4);
								return;
							}
						}
						return;
					}
					
				}
			}
		}
 
		
		public virtual void CalculateTransform()
		{
			this.CalculateTransform(0, 0);
			this.CalculateTransform(base.HScroll ? SystemInformation.VerticalScrollBarWidth : 0, base.VScroll ? SystemInformation.HorizontalScrollBarHeight : 0);
			this.OnTransformChanged(EventArgs.Empty);
		}
 				
		protected virtual void CalculateTransform(int dx, int dy)
		{
			this.CalculateTransform(this.ImageSize, dx, dy);
		}

		protected virtual void CalculateTransform(SizeF viewSize, int dx, int dy)
		{
			float scaleFactor = 1.0F;

            this._ignoreSizeChangedCounter++;
			
            base.SuspendLayout();

            // initialize new tranformation matrix
            Matrix matrix = new Matrix();
			
			SizeF sizeView = viewSize;
			SizeF sizeFrame = this.FrameIsPartOfView ? this.FrameSize : SizeF.Empty;
			SizeF sizeFrameShadow = this.FrameShadowIsPartOfView ? this.FrameShadowSize : SizeF.Empty;
			
			float margin_left = this.BorderPadding.Left + sizeFrame.Width;
			float margin_top = this.BorderPadding.Top + sizeFrame.Height;
			float margin_right = (this.BorderPadding.Right + sizeFrame.Width) + sizeFrameShadow.Width;
			float margin_bottom = (this.BorderPadding.Bottom + sizeFrame.Height) + sizeFrameShadow.Height;
			
			float scaleDx = this._currentXScaleFactor;
			float scaleDy = this._currentYScaleFactor;
			
			this._currentXScaleFactor = this.ScaleFactor;
			this._currentYScaleFactor = this.ScaleFactor;
			
			RectangleF rcClient = new RectangleF(margin_left, margin_top, base.ClientSize.Width - (margin_left + margin_right), base.ClientSize.Height - (margin_top + margin_bottom));
			
			if (!base.VScroll)
				rcClient.Width -= dx;
			if (!base.HScroll)
				rcClient.Height -= dy;

			RasterViewerSizeMode sizeMode = this.SizeMode;
			if (sizeMode == RasterViewerSizeMode.FitIfLarger)
			{
				if ((sizeView.Width > rcClient.Width) || (sizeView.Height > rcClient.Height))
					sizeMode = RasterViewerSizeMode.Fit;
				else
					sizeMode = RasterViewerSizeMode.Normal;
			}

			switch (sizeMode)
			{
				case RasterViewerSizeMode.Normal:
					break;
				case RasterViewerSizeMode.Fit:
				{
					if ((sizeView.Width <= 0f) || (sizeView.Height <= 0f))
					{
						this._currentXScaleFactor = scaleDx;
						this._currentYScaleFactor = scaleDy;
						break;
					}
					else if ((rcClient.Width <= 0f) || (rcClient.Height <= 0f))
					{
						this._currentXScaleFactor = scaleDx;
						this._currentYScaleFactor = scaleDy;
						break;
					}
					
					if (rcClient.Width <= rcClient.Height)
					{
						scaleFactor = rcClient.Height / sizeView.Height;
						if ((scaleFactor * sizeView.Width) > rcClient.Width)
							scaleFactor = rcClient.Width / sizeView.Width;
					}
					else
					{
						scaleFactor = rcClient.Width / sizeView.Width;
						if ((scaleFactor * sizeView.Height) > rcClient.Height)
							scaleFactor = rcClient.Height / sizeView.Height;
					}
					
					this._currentXScaleFactor *= scaleFactor;
					this._currentYScaleFactor *= scaleFactor;

					break;
				}
				case RasterViewerSizeMode.FitWidth:
				{
					if ((sizeView.Width > 0f) && (sizeView.Height > 0f))
					{
						scaleFactor = rcClient.Width / sizeView.Width;
						this._currentXScaleFactor *= scaleFactor;
						this._currentYScaleFactor *= scaleFactor;
					}
					break;
				}
				case RasterViewerSizeMode.Stretch:
				{
					if ((sizeView.Width > 0f) && (sizeView.Height > 0f))
					{
						this._currentXScaleFactor *= (rcClient.Width / sizeView.Width);
						this._currentYScaleFactor *= (rcClient.Height / sizeView.Height);
					}
					break;
				}
				default:
					break;
			}

			if (this._currentXScaleFactor <= 0)
				this._currentXScaleFactor = scaleDx;
			if (this._currentYScaleFactor <= 0)
				this._currentYScaleFactor = scaleDy;

			matrix.Scale(this._currentXScaleFactor, this._currentYScaleFactor, MatrixOrder.Append);
			
			sizeView = viewSize;
			PointF[] points = DrawingHelper.GetBoundingPoints(new RectangleF(PointF.Empty, sizeView));
			matrix.TransformPoints(points);
			RectangleF rcBound = DrawingHelper.GetBoundingRectangle(points);

			if (this.CenterMode != RasterViewerCenterMode.None)
			{
				float xOff = 0f;
				if (((this.CenterMode == RasterViewerCenterMode.Horizontal) || (this.CenterMode == RasterViewerCenterMode.Both)) && (rcClient.Width > rcBound.Width))
					xOff = (rcClient.Width - rcBound.Width) * 0.5f;
				
				float yOff = 0f;
				if (((this.CenterMode == RasterViewerCenterMode.Vertical) || (this.CenterMode == RasterViewerCenterMode.Both)) && (rcClient.Height > rcBound.Height))
					yOff = (rcClient.Height - rcBound.Height) * 0.5f;
				
				matrix.Translate(xOff, yOff, MatrixOrder.Append);
			}

			if (this.AutoScroll)
			{
				SizeF szBoundF = new SizeF(rcBound.Width, rcBound.Height);
				szBoundF.Width += (margin_left + margin_right);
				szBoundF.Height += (margin_top + margin_bottom);
				Size szBound = new Size((int) (szBoundF.Width + 0.5f), (int) (szBoundF.Height + 0.5f));
				
				if ((szBound.Width < base.ClientSize.Width) && (szBound.Height < base.ClientSize.Height))
				{
					base.AutoScrollMinSize = Size.Empty;
					this.AutoScrollPosition = Point.Empty;
					matrix.Translate(-rcBound.Left, -rcBound.Top, MatrixOrder.Append);
				}
				else
				{
					base.AutoScrollMinSize = szBound;
					PointF ptScroll = (PointF) this.AutoScrollPosition;
					ptScroll.X *= -1f; ptScroll.Y *= -1f;

					SizeF szScrollF = new SizeF(base.AutoScrollMinSize.Width - rcClient.Width, base.AutoScrollMinSize.Height - rcClient.Height);
					if ((ptScroll.X > szScrollF.Width) || (ptScroll.Y > szScrollF.Height))
						this.AutoScrollPosition = new Point((int) Math.Min(ptScroll.X, szScrollF.Width), (int) Math.Min(ptScroll.Y, szScrollF.Height));

					matrix.Translate(this.AutoScrollPosition.X - rcBound.Left, this.AutoScrollPosition.Y - rcBound.Top, MatrixOrder.Append);
				}
			}
			else
			{
				matrix.Translate(-rcBound.Left, -rcBound.Top, MatrixOrder.Append);
			}
			
            matrix.Translate(margin_left, margin_top, MatrixOrder.Append);

            // copy transform to current transform
            lock (_syncTransform)
            {
                _transform.Reset();
                _transform.Multiply(matrix);

                // release temporary matrix
                matrix.Dispose();
            }
			
            base.ResumeLayout();
			this._ignoreSizeChangedCounter--;
		}


		#region rubber band helpers

		private void ControlPaintPoints()
		{
			Point point1 = Point.Empty;
			for (int i = 0; i < (_interactivePoints.Count - 1); i++)
			{
				if (i == 0)
				{
					point1 = PointToScreen(_interactivePoints[i]);
				}
				else
				{
					Point point2 = PointToScreen(_interactivePoints[i]);
					GDIHelper.DrawReversibleLine(this, point1, point2, Color.Transparent);
					point1 = point2;
				}
			}
		}

        private void ControlPaintRectangle()
		{
			Rectangle rectangle = DrawingHelper.FixRectangle(_interactiveRectangle);
			rectangle.Width++;
			rectangle.Height++;
			rectangle = RectangleToScreen(rectangle);
			GDIHelper.DrawReversibleFrame(this, rectangle, Color.Transparent, FrameStyle.Dashed);
			_interactiveRectangleDrawn = true;
		}

		
		private void EraseControlPaintPoints()
		{
			base.Invalidate();
		}

        public void ControlPaintPoints(Point[] points)
        {
            for (int i = 0; i < points.Length - 1; i++)
            {
                Point pt1 = PointToScreen(points[i]);
                Point pt2 = PointToScreen(points[i + 1]);
                GDIHelper.DrawReversibleLine(this, pt1, pt2, Color.Transparent);
            }
        }

        public void ControlPaintRectangle(Rectangle rect)
        {
            Rectangle rectangle = DrawingHelper.FixRectangle(rect);
            rectangle.Width++;
            rectangle.Height++;
            rectangle = RectangleToScreen(rectangle);
            GDIHelper.DrawReversibleFrame(this, rectangle, Color.Transparent, FrameStyle.Dashed);
        }

        
		#endregion
 
		private void EraseControlPaintRectangle()
		{
			if (this._interactiveRectangleDrawn)
			{
				Rectangle rectangle = DrawingHelper.FixRectangle(_interactiveRectangle);
				rectangle.Width++;
				rectangle.Height++;
				rectangle = RectangleToScreen(rectangle);
				GDIHelper.DrawReversibleFrame(this, rectangle, Color.Transparent, FrameStyle.Dashed);
				_interactiveRectangleDrawn = false;
			}
		} 

		private int MulDiv(int x1, int x2, int x3)
		{
			return (int) ((x1 * x2) / ((long) x3));
		}
 		

		public virtual bool IsValidScaleFactor(float scaleFactor)
		{			
			if (this.IsImageAvailable == false)
				return false;

			bool result = true;

			if (scaleFactor < 1.0F)
			{
				using (Transformer transformer = new Transformer())
                {
					transformer.Scale(scaleFactor, scaleFactor, MatrixOrder.Append);
					
                    float maxImageSize = Math.Max(this.ImageSize.Width, this.ImageSize.Height);
					float maxClientSize = Math.Max(this.ClientSize.Width, this.ClientSize.Height);

					float physicalImageSize = transformer.LengthToPhysical(maxImageSize);
					result = physicalImageSize / maxClientSize > this._minPixelScaleFactor;// 0.025;
				}
			}
			else if (scaleFactor > 1.0F)
			{
				using (Transformer transformer = new Transformer())
                {
					transformer.Scale(scaleFactor, scaleFactor, MatrixOrder.Append);

                    float maxPhysicalPixelWidth = transformer.LengthToPhysical(1.0F);
					float minClientSize = Math.Min(this.ClientSize.Width, this.ClientSize.Height);
					result = (maxPhysicalPixelWidth / minClientSize) < this._maxPixelScaleFactor;// 0.8F;
				}
			}

			return result;
		}

		
		#region event handlers

		private void BorderPadding_Changed(object sender, EventArgs e)
		{
			CalculateTransform();
			Invalidate();
		}
		
		#endregion

		#endregion

		#region unit converter

		/// <summary>
		/// Convert a rectangle from pixels to inches
		/// </summary>
		/// <param name="rect">Rectangle measured in pixels</param>
		/// <returns>Rectangle measured in inches</returns>
		public RectangleF RectangleToInches(RectangleF rect)
		{
			float scaleDx = 1.0F/(float)this.ImageDpiX;
			float scaleDy = 1.0F/(float)this.ImageDpiY;
			return new RectangleF(rect.X*scaleDx, rect.Y*scaleDy, rect.Width*scaleDx, rect.Height*scaleDy);
		}

		/// <summary>
		/// Convert a rectangle from pixels to hundredth of an inch
		/// </summary>
		/// <param name="rect">Rectangle measured in pixels</param>
		/// <returns>Rectangle measured in hundredth of an inch</returns>
		public RectangleF RectangleToHundredthInches(RectangleF rect)
		{
			float scaleDx = 100.0F/(float)this.ImageDpiX;
			float scaleDy = 100.0F/(float)this.ImageDpiY;
			return new RectangleF(rect.X*scaleDx, rect.Y*scaleDy, rect.Width*scaleDx, rect.Height*scaleDy);
		}

		/// <summary>
		/// Convert a rectangle from inches to pixels
		/// </summary>
		/// <param name="rect">Rectangle measured in inches</param>
		/// <returns>Rectangle measured in pixels</returns>
		public RectangleF InchesRectangleToPixels(RectangleF rect)
		{
			float scaleDx = (float)this.ImageDpiX;
			float scaleDy = (float)this.ImageDpiY;
			return new RectangleF(rect.X*scaleDx, rect.Y*scaleDy, rect.Width*scaleDx, rect.Height*scaleDy);
		}

		/// <summary>
		/// Convert a rectangle from hundredths of an inch to pixels
		/// </summary>
		/// <param name="rect">Rectangle measured in hundredths of an inch</param>
		/// <returns>Rectangle measured in pixels</returns>
		public RectangleF HundrethInchesRectangleToPixels(RectangleF rect)
		{
			float scaleDx = (float)this.ImageDpiX/100.0F;
			float scaleDy = (float)this.ImageDpiY/100.0F;
			return new RectangleF(rect.X*scaleDx, rect.Y*scaleDy, rect.Width*scaleDx, rect.Height*scaleDy);
		}

		#endregion

		#region Grid Helper for debuging

		private MouseEventArgs _mouseArgs;

		private void OnPaintMouseLoc(PaintEventArgs e)
		{
			if (_mouseArgs == null)
				return;

			bool enabled = Form.ModifierKeys == Keys.Control;
			if (!enabled)
				return;
			
			Graphics graph = e.Graphics;
			PointF pt = new PointF(_mouseArgs.X, _mouseArgs.Y);

            using (Transformer transformer = this.Transformer)
            {
                pt = transformer.PointToLogical(pt);

                float left = (float)Math.Floor(pt.X);
                float top = (float)Math.Floor(pt.Y);

                RectangleF rect = new RectangleF(left, top, 1, 1);
                rect = transformer.RectangleToPhysical(rect);
                graph.FillRectangle(Brushes.Red, rect);
            }
		}

		private void OnPaintGrid(PaintEventArgs e, RectangleF rcSource)
		{
			Graphics graph = e.Graphics;
			bool enabled = Form.ModifierKeys == Keys.Control;
			if (!enabled)
				return;

            using (Transformer transformer = this.Transformer)
            {
                float left = rcSource.Left, right = rcSource.Right;
                float top = rcSource.Top, bottom = rcSource.Bottom;

                // draws vertical lines
                for (float start = (float)Math.Floor(left); start < Math.Ceiling(right); start += 1)
                {
                    PointF[] pts = new PointF[2];
                    pts[0] = new PointF(start, top);
                    pts[1] = new PointF(start, bottom);

                    pts = transformer.PointToPhysical(pts);
                    graph.DrawLine(Pens.Green, pts[0], pts[1]);
                }

                // draws vertical lines
                for (float start = (float)Math.Floor(top); start < Math.Ceiling(bottom); start += 1)
                {
                    PointF[] pts = new PointF[2];
                    pts[0] = new PointF(left, start);
                    pts[1] = new PointF(right, start);

                    pts = transformer.PointToPhysical(pts);
                    graph.DrawLine(Pens.Green, pts[0], pts[1]);
                }
            }
		}

		#endregion
	} 
}
