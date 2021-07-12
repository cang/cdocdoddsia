using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.ComponentModel;
using System.Runtime.InteropServices;

using SIA.Common;

using SIA.SystemFrameworks;
using SIA.SystemFrameworks.UI;

using SIA.Common.Native;
using SIA.UI.Components;
using SIA.Common.IPLFacade;

namespace SIA.UI.Components.Renders
{
	/// <summary>
	/// The NormalRasterImageRender provides functionality for rendering raster image
	/// </summary>
	public class NormalRasterImageRender 
        : IRasterImageRender, IDisposable	
	{
		#region member attributes
		
		private const String NAME = "GDI Raster Image Render";
		private const String DESCRIPTION = "Normal GDI Image Render";

		private IRasterImageViewer _imageViewer = null;

		private bool _isDirty = true;
		private bool _isColorMapTableDirty = true;
		private System.Drawing.Bitmap _memBuffer = null;
		private int[] _palette = null;
		private Color[] _colors = null;
		private float[] _positions = null;
		private bool _autoFitGrayScale = false;
		private DataRange _viewRange = DataRange.Empty;

		private EventHandler imageChangeEventHandler = null;
		public event EventHandler ViewRangeChanged = null;
		
		#endregion

		#region interface IRasterImageRender implementation
		public virtual string Name
		{
			get {return NAME;}
		}

		public virtual string Description
		{
			get {return DESCRIPTION;}
		}

        public IRasterImageViewer ImageViewer
		{
			get {return _imageViewer;}
			set 
			{
				if (_imageViewer != null)
					this._imageViewer.ImageChanged -= imageChangeEventHandler;
				_imageViewer = value;
				OnImageViewerChanged();
			}
		}

		public virtual bool IsDirty
		{
			get {return _isDirty;}
			set
			{
				_isDirty = value;
				_isColorMapTableDirty = value;
				OnIsDirtyChanged();
			}
		}

        public virtual bool IsColorMapTableDirty
        {
            get { return _isColorMapTableDirty; }
            set
            {
                _isColorMapTableDirty = value;
            }
        }

		protected virtual void OnIsDirtyChanged()
		{

		}

		public bool AutoFitGrayScale
		{
			get {return _autoFitGrayScale;}
			set
			{
				_autoFitGrayScale = value;
				OnAutoFitGrayScaleChanged();
			}
		}

		protected virtual void OnAutoFitGrayScaleChanged()
		{
			this.IsDirty = true;
		}

		public DataRange ViewRange
		{
			get {return _viewRange;}
			set 
			{
				_viewRange = value;
				OnViewRangeChanged();
			}
		}

		protected virtual void OnViewRangeChanged()
		{
			this.IsDirty = true;

			if (this.ViewRangeChanged != null)
				this.ViewRangeChanged(this, EventArgs.Empty);
		}

        public virtual void UpdateColorMapTable()
        {
            this._isColorMapTableDirty = true;
            this.OnUpdateColorMapTable();
        }

		public virtual void UpdateColorMapTable(Color[] colors, float[] positions)
		{
			if (this._imageViewer == null || this._imageViewer.IsImageAvailable == false)
				return;

			IRasterImage image = this._imageViewer.Image;
			if (_isColorMapTableDirty || _colors == null || _positions == null ||
				_colors.Length != colors.Length || _positions.Length != positions.Length)
			{
				_isColorMapTableDirty = true;
				_colors = (Color[])colors.Clone();
				_positions = (float[])positions.Clone();
			}
			else 
			{
				if (_isColorMapTableDirty == false && _colors.Length == colors.Length)
				{
					for (int i=0; i<colors.Length; i++)
					{
						if (colors[i].ToArgb() - _colors[i].ToArgb() != 0)
						{
							_isColorMapTableDirty = true;
							break;
						}
					}
				}

				if (_isColorMapTableDirty == false && _positions.Length == positions.Length)
				{
					for (int i=0; i<positions.Length; i++)
					{
						if (positions[i] != _positions[i])
						{
							_isColorMapTableDirty = true;
							break;
						}
					}
				}
			}

            this.OnUpdateColorMapTable();
		}

        protected virtual void OnUpdateColorMapTable()
        {
            if (this._imageViewer == null || this._imageViewer.IsImageAvailable == false)
                return;

            if (_isColorMapTableDirty)
            {
                IRasterImage image = this._imageViewer.Image;
                int MaxColorRange = (int)image.MAXGRAYVALUE;
                int MinColorRange = (int)image.MINGRAYVALUE;
                int minView = 0, maxView = 0;

                if (_autoFitGrayScale)
                {
                    image.FitViewToDataRange();
                    _viewRange = new DataRange((int)image.MinCurrentView, (int)image.MaxCurrentView);
                }
                else
                {
                    if (this._viewRange.Equals(DataRange.Empty))
                        _viewRange = new DataRange((int)image.MinCurrentView, (int)image.MaxCurrentView);
                }

                if (_viewRange.Maximum > 255)
                    _viewRange.Maximum = 255;

                minView = (int)_viewRange.Minimum;
                maxView = (int)_viewRange.Maximum;

                int NumColors = MaxColorRange - MinColorRange + 1;
                int NumViewColors = maxView - minView + 1;

                try
                {
                    System.Diagnostics.Debug.Assert(NumColors >= NumViewColors);
                    NumViewColors = Math.Max(NumViewColors, 2);

                    int[] GradientColors = CreateGradientColors(NumViewColors, _colors, _positions);
                    this._palette = new int[NumColors];

                    int minColor = GradientColors[0];
                    int maxColor = GradientColors[GradientColors.Length - 1];

                    for (int color = 0; color < NumColors; color++)
                    {
                        if (color <= minView)
                        {
                            _palette[color] = minColor;
                            continue;
                        }
                        else if (color >= maxView)
                        {
                            _palette[color] = maxColor;
                            continue;
                        }
                        else
                        {
                            _palette[color] = GradientColors[color - minView];
                        }
                    }

                    // reset update Color LUT flag
                    _isColorMapTableDirty = false;
                }
                finally
                {
                }
            }
        }

		public virtual bool CanRender(RasterImageViewer viewer)
		{		
			const int MaxWidth = 2000;
			const int MaxHeight = 2000;
			bool bResult = false;
			bResult = viewer != null && viewer.Image != null && viewer.Image.Width < MaxWidth && viewer.Image.Height < MaxHeight;
			return bResult;
		}

		public virtual void Paint(Graphics graph, RectangleF src, RectangleF srcClip, RectangleF dst, RectangleF dstClip)
		{
			if (this._imageViewer == null || this._imageViewer.IsImageAvailable == false)
				return;

			IRasterImage image = this._imageViewer.Image;			
			
			GraphicsState state = graph.Save();			
			try
			{
				if (this.IsDirty || _isColorMapTableDirty)
					this.CreateMemBuffer(image);
				GraphicsUnit units = GraphicsUnit.Pixel;
				
				graph.InterpolationMode = InterpolationMode.NearestNeighbor;
				graph.PixelOffsetMode = PixelOffsetMode.Half;

				RectangleF rcSrc = new RectangleF(0, 0, image.Width, image.Height);
				graph.DrawImage(_memBuffer, dst, rcSrc, units);
			}
			finally
			{
				graph.Restore(state);
			}
		}

		public virtual bool CanPrint(IRasterImage image)
		{
			return image != null;
		}

		public virtual void Print(IRasterImage image, Graphics graph, RectangleF src, RectangleF srcClip, RectangleF dst, RectangleF dstClip)
		{
			if (image == null)
				throw new ArgumentNullException("image");
			if (graph == null)
				throw new ArgumentNullException("graph");
			
			GraphicsState state = graph.Save();			
			try
			{
				if (this.IsDirty || _isColorMapTableDirty)
					this.CreateMemBuffer(image);
				
				lock (_memBuffer)
				{
					graph.InterpolationMode = InterpolationMode.NearestNeighbor;
					graph.PixelOffsetMode = PixelOffsetMode.Half;
					graph.DrawImage(_memBuffer, dst, src, GraphicsUnit.Pixel);
				}
			}
			catch
			{
				throw ;
			}
			finally
			{
				graph.Restore(state);
			}
		}

		#endregion

		#region interface ICloneable implementation

		public System.Object Clone()
		{
			return this.MemberwiseClone();
		}

		#endregion

		#region public properties
		public int[] Palette
		{
			get {return _palette;}
			set 
			{
				_palette = value;
				OnPaletteChanged();
			}
		}

		protected virtual void OnPaletteChanged()
		{
			this._isColorMapTableDirty = true;
		}
		#endregion

		#region constructor and destructors
		
		public NormalRasterImageRender()
		{
			imageChangeEventHandler = new EventHandler(ImageViewer_ImageChanged);
		}

		public virtual void Dispose()
		{
			imageChangeEventHandler = null;
			DestroyMemBuffer();
		}

		#endregion

		#region virtual routines

		protected virtual void OnImageViewerChanged()
		{
			if (this._imageViewer != null)
				this._imageViewer.ImageChanged += imageChangeEventHandler;
		}

		#endregion

		#region internal helpers

		private void ImageViewer_ImageChanged(object sender, EventArgs e)
		{
			DestroyMemBuffer();
			this._isColorMapTableDirty = true;
			this._colors = null;
			this._positions = null;
			this._palette = null;
		}

		private void CreateMemBuffer(IRasterImage image)
		{
			DestroyMemBuffer();			

			int width = image.Width;
			int height = image.Height;
		
			try
			{
				_memBuffer = image.CreateBitmap(this._palette, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
				_isColorMapTableDirty = false;
				IsDirty = false;
			}
			catch(System.Exception exp)
			{
				if (_memBuffer != null)
					_memBuffer.Dispose();
				_memBuffer = null;

				throw exp;
			}
		}

		private void DestroyMemBuffer()
		{
			if (_memBuffer != null)
				_memBuffer.Dispose();
			_memBuffer = null;
		}

		protected int[] CreateGradientColors(int numColors, Color[] stopColors, float[] stopPositions)
		{
			ArrayList result = new ArrayList();
			int numStops = stopColors.Length;
			
			for (int i=0; i<numStops-1; i++)
			{
				Color stopColor = stopColors[i];
				int startIndex = (int)Math.Max(0, Math.Floor(stopPositions[i]*numColors));
				int endIndex = (int)Math.Min(numColors-1, Math.Ceiling(stopPositions[i+1]*numColors));
				int[] colors = this.Interpolate(endIndex-startIndex+1, stopColors[i], stopColors[i+1]);
				result.AddRange(colors);
			}

			return (int[])result.ToArray(typeof(int));
		}

		private int[] Interpolate(int numColors, Color clrStart, Color clrStop)
		{
			int[] result = new int[numColors];
			for (int i=0; i<numColors; i++)
			{
				float ratio = i*1.0F/numColors;

				float alpha = clrStop.A*ratio + clrStart.A*(1-ratio);
				float red = clrStop.R*ratio + clrStart.R*(1-ratio);
				float green = clrStop.G*ratio + clrStart.G*(1-ratio);
				float blue = clrStop.B*ratio + clrStart.B*(1-ratio);
			
				result[i] = Color.FromArgb((int)Math.Round(alpha), (int)Math.Round(red), (int)Math.Round(green), (int)Math.Round(blue)).ToArgb();
			}

			return result;
		}
		#endregion

		
	}
}
