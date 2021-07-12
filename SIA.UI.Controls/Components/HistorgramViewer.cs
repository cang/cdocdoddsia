using System;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;

using SIA.Common;
using SIA.Common.Imaging;
using SIA.UI.Controls;
using SIA.UI.Controls.Common;

using SIA.SystemFrameworks;


namespace SIA.UI.Components
{
	/// <summary>
	/// Summary description for Histogram.
	/// </summary>
	public class HistorgramViewer : System.Windows.Forms.Control
	{
		private const int MAX_PIXEL_INTENSITY = 65535;

		public enum Type
		{
			Normal,
			Logarithmic
		}

		#region member attributes
		
		private SIA.SystemFrameworks.Histogram _histogram = null;
		private Type _type = Type.Normal;
		
		private System.Drawing.Image _memBuffer = null;
		private bool _memBufferDirty = true;

		private int _leftMargin = 1, _rightMargin = 1, _topMargin = 1, _bottomMargin = 1;

		#endregion

		#region public properties

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public SIA.SystemFrameworks.Histogram Histogram
		{
			get {return _histogram;}
			set
			{
				_histogram = value;
				OnHistogramChanged();
			}
		}

		protected virtual void OnHistogramChanged()
		{
			_memBufferDirty = true;
		}

		public Type ChartType
		{
			get {return _type;}
			set
			{
				_type = value;
				OnChartTypeChanged();
			}
		}

		protected virtual void OnChartTypeChanged()
		{
			_memBufferDirty = true;
		}

		public Rectangle RenderableRectangle
		{
			get 
			{
				Rectangle rcClient = this.ClientRectangle;
				return Rectangle.FromLTRB(rcClient.Left + _leftMargin, rcClient.Top + _topMargin, rcClient.Right + _rightMargin, rcClient.Bottom + _bottomMargin);
			}
		}

		#endregion

		#region constructor and destructor

		public HistorgramViewer()
		{
			InitializeComponents();
		}

		private void InitializeComponents()
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_memBuffer != null)
				{
					_memBuffer.Dispose();
					_memBuffer = null;
				}
			}

			base.Dispose (disposing);
		}

		#endregion

		#region override routines
		
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint (e);

			if (_histogram != null)
			{
				if (this._memBufferDirty == true)
					this.RenderHistogram();
				if (_memBuffer != null)
				{
					Rectangle dst = Rectangle.FromLTRB(_leftMargin,_topMargin,
						this.Size.Width - _rightMargin, this.Size.Height - _bottomMargin);
					Rectangle src = new Rectangle(0, 0, _memBuffer.Width, _memBuffer.Height);
					e.Graphics.SmoothingMode = SmoothingMode.HighSpeed;
					e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
					e.Graphics.DrawImage(_memBuffer, dst, src, GraphicsUnit.Pixel);
				}
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove (e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown (e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp (e);
		}

		#endregion

		#region internal helper
		private void RenderHistogram()
		{
			int[] data = this.Histogram.Data;
			int minThreshold = 0;
			int maxThreshold = data.Length - 1;
			
			int buf_width = data.Length;
			int buf_height = (int)(this.Histogram.MaxCount - this.Histogram.MinCount + 1);


			Graphics graph = null; 
			Matrix transform = new Matrix(1, 0, 0, -1, 0, 0);
			Pen chartPen = Pens.Black;

			try
			{
				Rectangle rcMemBuffer = new Rectangle(0, 0, buf_width, buf_height);
				Rectangle rcDraw = new Rectangle(0, 0, this.Width, this.Height);
				
				if (_memBuffer != null)
					_memBuffer.Dispose();
				_memBuffer = new Bitmap(rcDraw.Width, rcDraw.Height, PixelFormat.Format24bppRgb);
				if (_memBuffer == null)
					throw new System.OutOfMemoryException();
				graph = Graphics.FromImage(this._memBuffer);

				float scaleDX = (float)rcDraw.Width / (float)(rcMemBuffer.Width);
				float scaleDY = (float)rcDraw.Height / (float)(rcMemBuffer.Height);
				transform.Translate(0.0F, (float)rcMemBuffer.Height, MatrixOrder.Append);
				transform.Scale(scaleDX, scaleDY, MatrixOrder.Append);

				graph.Transform = transform;
				graph.SmoothingMode = SmoothingMode.None;
				graph.InterpolationMode = InterpolationMode.NearestNeighbor;

				graph.Clear(this.BackColor);

				for (int i=minThreshold; i<=maxThreshold; i++)
				{
					int value = data[i];
					graph.DrawLine(chartPen, new Point(i, 0), new Point(i, value));
				}
			}
			catch(System.Exception exp)
			{
				throw exp;
			}
			finally
			{
				if (graph != null) 
					graph.Dispose();
				transform.Dispose();

#if DEBUG
				_memBuffer.Save(Application.StartupPath + @"\membuffer.jpg", ImageFormat.Jpeg);
#endif
			}
		}

		private bool RefreshMemBuffer()
		{
			try 
			{
				int[] data = this.Histogram.Data;
				int minThreshold = 0;
				int maxThreshold = data.Length - 1;

				/* initializes drawing tools */
				if (_memBuffer != null) 
					_memBuffer.Dispose();
				_memBuffer = new System.Drawing.Bitmap(this.Width, this.Height);
				
				System.Drawing.Graphics		graph = System.Drawing.Graphics.FromImage(_memBuffer);
				System.Drawing.Pen			chartPen = new System.Drawing.Pen(this.ForeColor, 1.0f);
				System.Drawing.SolidBrush	chartBrush = new System.Drawing.SolidBrush(this.ForeColor);
				System.Drawing.Brush		rulerBrush = null;
				System.Drawing.Font			chartFont = this.Font;
				
				// clear back color
				graph.Clear(this.BackColor);

				/* computes parameter for rendering */
				bool bLogChart = this.ChartType == Type.Logarithmic;

				double minCount = (double)UInt32.MaxValue;
				double maxCount = (double)UInt32.MinValue;
				for (int i=minThreshold; i<=maxThreshold; i++)
				{
					minCount = Math.Min(minCount, data[i]);
					maxCount = Math.Max(maxCount, data[i]);
				}
				minCount = (double)(bLogChart ? (minCount!=0 ? Math.Log(minCount, Math.E) : 0) : minCount);
				maxCount = (double)(bLogChart ? Math.Log(maxCount, Math.E) : maxCount);

				double iCount = 0;
				int threshold = 0;
				float fYScaleFactor = 1.0f;
				float fXScaleFactor = 1.0f;
				SizeF unitSize = new SizeF(1.0F, 1.0F);
				SizeF textSize, spaceSize;
				PointF textPos = new PointF(.0f, .0f);

				Rectangle rcDraw = this.RenderableRectangle;
				float left    = rcDraw.Left;
				float top	  = rcDraw.Top;
				float right   = rcDraw.Right;
				float bottom  = rcDraw.Bottom;
				float width	  = rcDraw.Width;
				float height  = rcDraw.Height;

				/* render chart's border */
				graph.DrawRectangle(chartPen, rcDraw.Left, rcDraw.Top, rcDraw.Width-1, rcDraw.Height-1);
				/* render chart content */
				fYScaleFactor = (float)height/(float)(maxCount-minCount);			
				fXScaleFactor = (float)width/(float)(maxThreshold-minThreshold);
				spaceSize = new SizeF(unitSize.Width * fXScaleFactor, unitSize.Height * fYScaleFactor);
				
				for (threshold=minThreshold; threshold<=maxThreshold; threshold++)
				{
					if (threshold >=0 && threshold<data.Length)
					{
						iCount = bLogChart ? (double)(Math.Log(data[threshold], Math.E)) : (double)(data[threshold]);
						if ((int)(iCount*fYScaleFactor)>0) 
						{
							ArrayList arPoints = new ArrayList();
							arPoints.Add(new PointF((float)(left+(threshold-minThreshold)*fXScaleFactor), (float)bottom));
							arPoints.Add(new PointF((float)(left+(threshold-minThreshold)*fXScaleFactor), (float)(bottom-(iCount*fYScaleFactor))));
							arPoints.Add(new PointF((float)(left+(threshold-minThreshold)*fXScaleFactor), (float)bottom));
						
							PointF[] drawPoints = (PointF[])arPoints.ToArray(typeof(PointF));
							graph.DrawLines(chartPen, drawPoints);						
						}
					}
				}

				/* render chart's ruler */
				Pen rulerPen = chartPen;
				graph.DrawLine(rulerPen,  left-1, top, left-1, bottom+1);			// vertical line
				graph.DrawLine(rulerPen,  left-1, bottom+1, right+1, bottom+1);		// horizontal line

				/* compute ruler's unit */
				int YRange = (int)(maxCount - minCount);
				int XRange = maxThreshold - minThreshold;
				unitSize = graph.MeasureString(((int)MAX_PIXEL_INTENSITY).ToString(), chartFont);
				float unitHeight = (float)(5.0f*unitSize.Height);
				float unitWidth  = (float)(5.0f*unitSize.Width);
				int HorzStep = (int)(YRange / unitHeight);
				int VertStep = (int)(XRange / unitWidth);
				/* render ruler's text */
				rulerBrush = chartBrush;
				
				/* horizontal ruler */
				textSize = graph.MeasureString(minThreshold.ToString(), chartFont);
				graph.DrawString(minThreshold.ToString(), chartFont, rulerBrush,
					left-1.0f-textSize.Width/2.0f, bottom+2.0f, StringFormat.GenericDefault);
				textSize = graph.MeasureString(maxThreshold.ToString(), chartFont);
				graph.DrawString(maxThreshold.ToString(), chartFont, rulerBrush,
					right-textSize.Width/2.0f, bottom+2.0f, StringFormat.GenericDefault);

				/* vertical ruler */
				textSize = graph.MeasureString(((int)maxCount).ToString(), chartFont);
				float XPos = left - 3.0f - textSize.Width;
				float YPos = top - textSize.Height/2.0f;
				graph.DrawLine(chartPen, left-3.0f, top, left-1.0f, top);
				graph.DrawString(((int)maxCount).ToString(), chartFont, 
					chartBrush, XPos, YPos, StringFormat.GenericDefault);

				textSize = graph.MeasureString(((int)minCount).ToString(), chartFont);
				XPos = left - 3.0f - textSize.Width;
				YPos = bottom - textSize.Height/2.0f;
				graph.DrawLine(chartPen, left-3.0f, bottom, left-1.0f, bottom);
				graph.DrawString(((int)minCount).ToString(), chartFont,
					chartBrush, XPos, YPos, StringFormat.GenericDefault);
				
				rulerPen.Dispose();
				rulerBrush.Dispose();

				this._memBufferDirty = false;
				return true;
			}
			catch(Exception exp)
			{
				Trace.WriteLine(exp.ToString());
				return false;
			}
		}


		#endregion
	}
}
