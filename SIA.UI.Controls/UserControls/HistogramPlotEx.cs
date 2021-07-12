using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

using NPlot;

namespace SIA.UI.Controls.UserControls
{
	/// <summary>
	/// Provides ability to draw histogram plots.
	/// </summary>
	public class HistogramPlotEx : BaseSequencePlot, IPlot, ISequencePlot
	{
		private System.Drawing.Pen _drawPen = new Pen(Color.Black);
		private IRectangleBrush _rectBrush = new RectangleBrushes.Solid( Color.Black );
		private HistogramPlotEx _stackedTo;
		private float _baseWidth = 1.0f;
		private bool _center = true;
		private bool _isStacked;
		private double _baseOffset;
		private bool _filled = false;
		private Color[] _colors = null;
		private bool _bGreyImageStyle = true;

		/// <summary>
		/// Set/Get the brush to use if the histogram is filled.
		/// </summary>
		public IRectangleBrush RectangleBrush
		{
			get
			{
				return _rectBrush;
			}
			set
			{
				_rectBrush = value;
			}

		}

		/// <summary>
		/// The width of the histogram bar as a proportion of the data spacing 
		/// (in range 0.0 - 1.0).
		/// </summary>
		public float BaseWidth
		{
			get
			{
				return _baseWidth;
			}
			set
			{
				if (value > 0.0 && value <= 1.0)
				{
					_baseWidth = value;
				}
				else
				{
					throw new NPlotException( "Base width must be between 0.0 and 1.0" );
				}
			}
		}

		/// <summary>
		/// If true, each histogram column will be centered on the associated abscissa value.
		/// If false, each histogram colum will be drawn between the associated abscissa value, and the next abscissa value.
		/// Default value is true.
		/// </summary>
		public bool Center
		{
			set
			{
				_center = value;
			}
			get
			{
				return _center;
			}
		}

		/// <summary>
		/// If this histogram plot has another stacked on top, this will be true. Else false.
		/// </summary>
		public bool IsStacked
		{
			get
			{
				return _isStacked;
			}
		}
		
		/// <summary>
		/// The pen used to draw the plot
		/// </summary>
		public System.Drawing.Pen Pen
		{
			get
			{
				return _drawPen;
			}
			set
			{
				_drawPen = value;
			}
		}

		/// <summary>
		/// The color of the pen used to draw lines in this plot.
		/// </summary>
		public System.Drawing.Color Color
		{
			set
			{
				if (_drawPen != null)
				{
					_drawPen.Color = value;
				}
				else
				{
					_drawPen = new Pen(value);
				}
			}
			get
			{
				return _drawPen.Color;
			}
		}

		/// <summary>
		/// Horizontal position of histogram columns is offset by this much (in world coordinates).
		/// </summary>
		public double BaseOffset
		{
			set
			{
				_baseOffset = value;
			}
			get
			{
				return _baseOffset;
			}
		}

		/// <summary>
		/// Whether or not the histogram columns will be filled.
		/// </summary>
		public bool Filled
		{
			get 
			{
				return _filled;
			}
			set
			{
				_filled = value;
			}
		}


		/// <summary>
		/// Pseudo color for rendering data
		/// </summary>
		public Color[] HistogramPlotGreyImageColors
		{
			set
			{
				this._colors = value;
			}
		}

		/// <summary>
		/// Gets or sets the flag specified drawing the histogram by using pseudo color
		/// </summary>
		public bool HistogramPlotGreyImageStyle
		{
			get
			{
				return this._bGreyImageStyle;
			}
			set
			{
				this._bGreyImageStyle = value;
			}
		}
 
		/// <summary>
		/// Constructor
		/// </summary>
		public HistogramPlotEx()
		{
		}


		/// <summary>
		/// Renders the histogram.
		/// </summary>
		/// <param name="g">The Graphics surface on which to draw</param>
		/// <param name="xAxis">The X-Axis to draw against.</param>
		/// <param name="yAxis">The Y-Axis to draw against.</param>
		public void Draw( Graphics g, PhysicalAxis xAxis, PhysicalAxis yAxis )
		{
			GraphicsState state = null;

			try
			{
				// save graphics state
				state = g.Save();

				// set smoothing mode
				g.SmoothingMode = SmoothingMode.AntiAlias;

				double scaleDx = xAxis.PixelWorldLength;
				double devValue = 1/xAxis.PixelWorldLength;
				
				// draw using graphics path
				if (devValue > 4.0) 
					this.DrawColumns(g, xAxis, yAxis);
				else
					this.DrawPoints(g, xAxis, yAxis);
			}
			finally
			{
				if (state != null)
					g.Restore(state);
				state = null;
			}
		}

		private void DrawColumns(Graphics g, PhysicalAxis xAxis, PhysicalAxis yAxis)
		{
			SequenceAdapter data = new SequenceAdapter( this.DataSource, this.DataMember, this.OrdinateData, this.AbscissaData );

			ArrayList points = new ArrayList();
			ArrayList rects = new ArrayList();
			float yoff;
			float orgX = xAxis.WorldToPhysical(0, false).X;
			float orgY = yAxis.WorldToPhysical(0, false).Y;
			PointF ptOrg = new PointF(orgX, orgY);
		
			for ( int i=0; i<data.Count; ++i )
			{
				// (1) determine the top left hand point of the bar (assuming not centered)
				PointD p1 = data[i];
				if ( double.IsNaN(p1.X) || double.IsNaN(p1.Y) )
					continue;
				
				// (2) determine the top right hand point of the bar (assuming not centered)
				PointD p2;
				if (i+1 != data.Count)
				{
					p2 = data[i+1];
					if ( double.IsNaN(p2.X) || double.IsNaN(p2.Y) )
						continue;
					p2.Y = p1.Y;
				}
				else if (i != 0)
				{
					p2 = data[i-1];
					if ( double.IsNaN(p2.X) || double.IsNaN(p2.Y) )
						continue;
					double offset = p1.X - p2.X;
					p2.X = p1.X + offset;
					p2.Y = p1.Y;
				}
				else
				{
					double offset = 1.0f;
					p2.X = p1.X + offset;
					p2.Y = p1.Y;
				}

				// (3) now account for plots this may be stacked on top of.
				HistogramPlotEx currentPlot = this;
				yoff = 0.0f;
				double yval = 0.0f;
				while (currentPlot._isStacked)
				{
					SequenceAdapter stackedToData = new SequenceAdapter(
						currentPlot._stackedTo.DataSource, 
						currentPlot._stackedTo.DataMember,
						currentPlot._stackedTo.OrdinateData, 
						currentPlot._stackedTo.AbscissaData );

					yval += stackedToData[i].Y;
					yoff = yAxis.WorldToPhysical( yval, false ).Y;
					p1.Y += stackedToData[i].Y;
					p2.Y += stackedToData[i].Y;
					currentPlot = currentPlot._stackedTo;
				}

				// (4) now account for centering
				if ( _center )
				{
					double offset = ( p2.X - p1.X ) / 2.0f;
					p1.X -= offset;
					p2.X -= offset;
				}

				// (5) now account for BaseOffset (shift of bar sideways).
				p1.X += _baseOffset;
				p2.X += _baseOffset;

				// (6) now get physical coordinates of top two points.
				PointF xPos1 = xAxis.WorldToPhysical( p1.X, false );
				PointF yPos1 = yAxis.WorldToPhysical( p1.Y, false );
				PointF xPos2 = xAxis.WorldToPhysical( p2.X, false );
				PointF yPos2 = yAxis.WorldToPhysical( p2.Y, false );

				if (_isStacked)
				{
					currentPlot = this;
					while (currentPlot._isStacked)
						currentPlot = currentPlot._stackedTo;

					this._baseWidth = currentPlot._baseWidth;
				}

				float width = xPos2.X - xPos1.X;
				float height;
				if (_isStacked)
					height = -yPos1.Y+yoff;
				else
					height = -yPos1.Y+orgY;
					
				float xoff = (1.0f - _baseWidth)/2.0f*width;
					
				RectangleF rcBin = new RectangleF( (float)(xPos1.X+xoff), (float)yPos1.Y, 
					(float)(width-2*xoff), (float)height);
				rects.Add(rcBin);
			}

			try
			{
				Color clrDraw = ControlPaint.DarkDark(Color.Gray);
				Color clrFill = Color.FromArgb(0x80, ControlPaint.Light(Color.Gray));

				if (this.Filled)
				{
					using (Brush brush = new SolidBrush(clrFill))
					{
						foreach (RectangleF rect in rects)
						{
							if (rect.Width > 0 && rect.Height > 0)
								g.FillRectangle(brush, rect);
						}
					}
				}

				using (Pen pen = new Pen(clrDraw, this.Pen.Width))
				{
					foreach (RectangleF rect in rects)
					{
						if (rect.Width > 0 && rect.Height > 0)
							g.DrawRectangle(pen, rect.Left, rect.Top, rect.Width, rect.Height);
					}
				}
			}
			catch (System.Exception exp)
			{
				Trace.WriteLine(exp);
			}
		}

		private void DrawPoints(Graphics g, PhysicalAxis xAxis, PhysicalAxis yAxis)
		{
			SequenceAdapter data = new SequenceAdapter( this.DataSource, this.DataMember, this.OrdinateData, this.AbscissaData );

			float orgX = xAxis.WorldToPhysical(0, false).X;
			float orgY = yAxis.WorldToPhysical(0, false).Y;
			PointF ptOrg = new PointF(orgX, orgY);

			ArrayList points = new ArrayList();
			for (int i=0; i<data.Count; i++)
			{
				PointD dataPoint = data[i];
				if (double.IsNaN(dataPoint.X) || double.IsNaN(dataPoint.Y))
					continue;
				
				// (1) now account for BaseOffset (shift of bar sideways).
				dataPoint.X += _baseOffset;

				// (2) now get physical coordinates of top two points.
				PointF xPos = xAxis.WorldToPhysical( dataPoint.X, false );
				PointF yPos = yAxis.WorldToPhysical( dataPoint.Y, false );

				float xValue = xPos.X;
				float yValue = yPos.Y;
				points.Add(new PointF(xValue, yValue));
			}

			if (points.Count > 2)
			{
				Color clrFill = Color.FromArgb(0x80, Color.Gray);
				Color clrDraw = ControlPaint.DarkDark(Color.Gray);

				using (GraphicsPath path = new GraphicsPath())
				{	
					if (this.Filled)
					{
						PointF pt = (PointF)points[0];
						points.Insert(0, new PointF(pt.X, orgY));
						pt = (PointF)points[points.Count-1];
						points.Add(new PointF(pt.X, orgY));
						path.AddPolygon((PointF[])points.ToArray(typeof(PointF)));

						using (Brush brush = new SolidBrush(clrFill))
							g.FillPath(brush, path);

						points.RemoveAt(0);
						points.RemoveAt(points.Count-1);
					}

					//path.Reset();
					//path.AddPolygon((PointF[])points.ToArray(typeof(PointF)));

					using (Pen pen = new Pen(clrDraw, 1.0F))
						g.DrawPath(pen, path);
				}
			}
		}

		/// <summary>
		/// Returns an x-axis that is suitable for drawing this plot.
		/// </summary>
		/// <returns>A suitable x-axis.</returns>
		public Axis SuggestXAxis()
		{
			SequenceAdapter data = 
				new SequenceAdapter( this.DataSource, this.DataMember, this.OrdinateData, this.AbscissaData );

			Axis a = data.SuggestXAxis();

			PointD p1;
			PointD p2;
			PointD p3;
			PointD p4;
			if (data.Count < 2)
			{
				p1 = data[0];
				p1.X -= 1.0;
				p2 = data[0];
				p3 = p1;
				p4 = p2;
			}
			else
			{
				p1 = data[0];
				p2 = data[1];
				p3 = data[data.Count-2];
				p4 = data[data.Count-1];
			}

			double offset1;
			double offset2;

			if (!_center)
			{
				offset1 = 0.0f;
				offset2 = p4.X - p3.X;
			}
			else
			{
				offset1 = (p2.X - p1.X)/2.0f;
				offset2 = (p4.X - p3.X)/2.0f;
			}

			a.WorldMin -= offset1;
			a.WorldMax += offset2;

			return a;
		}

		/// <summary>
		/// Returns a y-axis that is suitable for drawing this plot.
		/// </summary>
		/// <returns>A suitable y-axis.</returns>
		public Axis SuggestYAxis()
		{

			if ( this._isStacked )
			{
				double tmpMax = 0.0f;
				ArrayList adapterList = new ArrayList();

				HistogramPlotEx currentPlot = this;
				do
				{
					adapterList.Add( new SequenceAdapter( 
						currentPlot.DataSource,
						currentPlot.DataMember,
						currentPlot.OrdinateData, 
						currentPlot.AbscissaData )
					);
				} while ((currentPlot = currentPlot._stackedTo) != null);
				
				SequenceAdapter[] adapters =
					(SequenceAdapter[])adapterList.ToArray(typeof(SequenceAdapter));
				
				for (int i=0; i<adapters[0].Count; ++i)
				{
					double tmpHeight = 0.0f;
					for (int j=0; j<adapters.Length; ++j)
					{
						tmpHeight += adapters[j][i].Y;
					}
					tmpMax = Math.Max(tmpMax, tmpHeight);
				}

				Axis a = new LinearAxis(0.0f,tmpMax);
				// TODO make 0.08 a parameter.
				a.IncreaseRange( 0.08 );
				return a;
			}
			else
			{
				SequenceAdapter data = 
					new SequenceAdapter( this.DataSource, this.DataMember, this.OrdinateData, this.AbscissaData );

				return data.SuggestYAxis();
			}
		}
		
		/// <summary>
		/// Stack the histogram to another HistogramPlotEx.
		/// </summary>
		public void StackedTo(HistogramPlotEx plot)
		{
			SequenceAdapter data = 
				new SequenceAdapter( this.DataSource, this.DataMember, this.OrdinateData, this.AbscissaData );

			SequenceAdapter hpData = 
				new SequenceAdapter( plot.DataSource, plot.DataMember, plot.OrdinateData, plot.AbscissaData );

				if ( plot != null )
				{
					_isStacked = true;
					if ( hpData.Count != data.Count )
						throw new NPlotException("Can stack HistogramPlotEx data only with the same number of datapoints.");

					for ( int i=0; i < data.Count; ++i )
					{
						if ( data[i].X != hpData[i].X )
						{
							throw new NPlotException("Can stack HistogramPlotEx data only with the same X coordinates.");
						}
						if ( hpData[i].Y < 0.0f)
						{
							throw new NPlotException("Can stack HistogramPlotEx data only with positive Y coordinates.");
						}
					}
				}
				_stackedTo = plot;
		}

		/// <summary>
		/// Draws a representation of this plot in the legend.
		/// </summary>
		/// <param name="g">The graphics surface on which to draw.</param>
		/// <param name="startEnd">A rectangle specifying the bounds of the area in the legend set aside for drawing.</param>
		public void DrawInLegend( Graphics g, Rectangle startEnd )
		{
			if (Filled)
				g.FillRectangle( _rectBrush.Get(startEnd), startEnd );			
			g.DrawRectangle(Pen, startEnd.X, startEnd.Y, startEnd.Width, startEnd.Height );
		}

    }
}
