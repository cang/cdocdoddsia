using System;
using System.Globalization;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace SiGlaz.Common
{
	/// <summary>
	/// Summary description for DrawOnionRing.
	/// </summary>
	public class DrawOnionRing : DrawObject
	{
		private RectangleF _Rectangle;
		private float _Radius;
		private float _Ratio;
		private PointF _pointLast = Point.Empty;

		private const string entryOnionRing_Rectangle	= "OnionRing-Rectangle";
		private const string entryOnionRing_Radius		= "OnionRing-Radius";

		#region Properties

		public RectangleF Rectangle
		{
			get {return _Rectangle;}
			set{_Rectangle = value;}
		}

		public float Radius 
		{
			get{return _Radius;}
			set{_Radius = value;}
		}

		public float Ratio
		{
			get {return _Ratio;}
		}

		public Point Center
		{
			get{return Point.Round(CenterF);}			
		}

		public PointF CenterF
		{
			get
			{
				return new PointF((float)(_Rectangle.Left + (float)_Rectangle.Width/2.0f),
					(float)(_Rectangle.Top  + (float)_Rectangle.Height/2.0f));
			}
		}

		public override RectangleF BoundingRectangle
		{
			get
			{
				return _Rectangle;
			}
		}

		#endregion

        
		public DrawOnionRing()
		{
			SetRectangle(0, 0, 1, 1);
			Initialize();
		}

        
		public DrawOnionRing(float x, float y, float width, float height)
		{
			_Rectangle.X = x;
			_Rectangle.Y = y;
			_Rectangle.Width = width;
			_Rectangle.Height = height;
			_Radius = 1;
			Initialize();
		}

		public DrawOnionRing(DrawOnionRing obj) : base(obj)
		{
			_Rectangle = obj._Rectangle;
			_Radius = obj._Radius;
			_Ratio = obj._Ratio;
			Initialize();
		}


		/// <summary>
		/// Draw Onion Ring
		/// </summary>
		/// <param name="g"></param>
		public override void Draw(Graphics g)
		{
			Pen pen = new Pen(Color, PenWidth);
			Brush brush = new SolidBrush(BrushColor);

			RectangleF  rcDraw = DrawRectangle.GetNormalizedRectangle(_Rectangle);
			float radius = Math.Max(_Radius, 1);
			float left = rcDraw.Left + Math.Min(radius, rcDraw.Width/2);
			float top  = rcDraw.Top  + Math.Min(radius, rcDraw.Height/2);
			float right = rcDraw.Right - Math.Min(radius, rcDraw.Width/2);
			float bottom = rcDraw.Bottom - Math.Min(radius, rcDraw.Height/2);
			RectangleF rcClip = DrawRectangle.GetNormalizedRectangle(left, top, right, bottom);

            GraphicsPath drawPath = new GraphicsPath();
            drawPath.AddEllipse(rcDraw);
			GraphicsPath clipPath = new GraphicsPath();
			clipPath.AddEllipse(rcClip);
            if (Container.MetroSys != null)
            {
                drawPath.Transform(Container.MetroSys.InvTransformer);
                clipPath.Transform(Container.MetroSys.InvTransformer);
            }
			g.SetClip(clipPath, CombineMode.Exclude);
            g.FillPath(brush, drawPath);
            g.ResetClip();
            g.DrawPath(pen, drawPath);
            g.DrawPath(pen, clipPath);

            //g.FillEllipse(brush, rcDraw);
            //g.DrawEllipse(pen, rcClip);
            //g.DrawEllipse(pen, rcDraw);
			
			//g.ResetClip();
			pen.Dispose();
			brush.Dispose();
			clipPath.Dispose();
		}

		public override void Draw(SIA.Common.Mask.IMask mask)
		{
			RectangleF  rcDraw = DrawRectangle.GetNormalizedRectangle(_Rectangle);
			float radius = Math.Max(_Radius, 1);
			float left = rcDraw.Left + Math.Min(radius, rcDraw.Width/2);
			float top  = rcDraw.Top  + Math.Min(radius, rcDraw.Height/2);
			float right = rcDraw.Right - Math.Min(radius, rcDraw.Width/2);
			float bottom = rcDraw.Bottom - Math.Min(radius, rcDraw.Height/2);
			RectangleF rcClip = DrawRectangle.GetNormalizedRectangle(left, top, right, bottom);
			mask.FillRing(rcDraw, rcClip);
		}

		public override void DrawTracker(Graphics g)
		{
			if (!Selected)
				return;

			try
			{
				SolidBrush brush = new SolidBrush(TrackerBrushColor);
				Pen pen = new Pen(TrackerPenColor, 0);
				RectangleF rcBound = GetBounds();

                if (rcBound.IsEmpty == false)
                {
                    GraphicsPath path = new GraphicsPath();
                    path.AddRectangle(rcBound);
                    if (Container.MetroSys != null)
                        path.Transform(Container.MetroSys.InvTransformer);
                    g.DrawPath(pen, path);
                    //g.DrawRectangle(pen, rcBound.Left, rcBound.Top, rcBound.Width, rcBound.Height);
                }

				for ( int i=1; i <= HandleCount; i++ )
				{
					RectangleF rectHandle = GetHandleRectangle(i);
                    if (i < 5)
                    {
                        g.FillRectangle(brush, rectHandle.X, rectHandle.Y, rectHandle.Width, rectHandle.Height);
                        g.DrawRectangle(pen, rectHandle.X, rectHandle.Y, rectHandle.Width, rectHandle.Height);
                    }
                    else
                        g.FillEllipse(Brushes.Yellow, rectHandle);
				}

				brush.Dispose();
				pen.Dispose();

				// draw horizontal and vertical axis
				RectangleF rcDraw = DrawRectangle.GetNormalizedRectangle(_Rectangle);
				float XCenter = (float)(rcDraw.Left + (float)rcDraw.Width/2.0f);
				float YCenter = (float)(rcDraw.Top  + (float)rcDraw.Height/2.0f);
				float delta = (float)((float)rcDraw.Width/2.0f - _Radius);
				RectangleF rcClip = new RectangleF(XCenter - delta, YCenter - delta, 2*delta, 2*delta);
			
				Pen redPen = new Pen(Color.Red, 0);
				redPen.DashStyle = DashStyle.DashDot;
                GraphicsPath axisPath = new GraphicsPath();
                axisPath.AddLine(rcDraw.Left, YCenter, rcDraw.Right, YCenter);
                axisPath.StartFigure();
                axisPath.AddLine(XCenter, rcDraw.Top, XCenter, rcDraw.Bottom);
                if (Container.MetroSys != null)
                    axisPath.Transform(Container.MetroSys.InvTransformer);
                g.DrawPath(redPen, axisPath);
                //g.DrawLine(redPen, rcDraw.Left, YCenter, rcDraw.Right, YCenter);
                //g.DrawLine(redPen, XCenter, rcDraw.Top, XCenter, rcDraw.Bottom);
				redPen.Dispose();			
			}
			catch (System.Exception exp)
			{
				System.Diagnostics.Debug.WriteLine(exp.ToString());
			}

		}

		protected void SetRectangle(float x, float y, float width, float height)
		{
			_Rectangle.X = x;
			_Rectangle.Y = y;
			_Rectangle.Width = width;
			_Rectangle.Height = height;

	//		Rectangle rectNorm = DrawRectangle.GetNormalizedRectangle(_Rectangle);
	//		if (_Radius > Math.Max(1, Math.Min(rectNorm.Width, rectNorm.Height)))
	//			_Radius = Math.Max(1, Math.Min(rectNorm.Width, rectNorm.Height));
		}

		public override RectangleF GetBounds()
		{
			return DrawRectangle.GetNormalizedRectangle(_Rectangle);
		}

		/// <summary>
		/// Get number of handles
		/// </summary>
		public override int HandleCount
		{
			get
			{
				return 8;
			}
		}


		/// <summary>
		/// Get handle point by 1-based number
		/// </summary>
		/// <param name="handleNumber"></param>
		/// <returns></returns>
		public override PointF GetHandle(int handleNumber)
		{
			RectangleF rcDraw = DrawRectangle.GetNormalizedRectangle(_Rectangle);
			float x, y, xCenter, yCenter;

			xCenter = rcDraw.X + rcDraw.Width/2;
			yCenter = rcDraw.Y + rcDraw.Height/2;
			x = rcDraw.X;
			y = rcDraw.Y;
			

			switch ( handleNumber )
			{
				case 1:
					x = rcDraw.X;
					y = rcDraw.Y;
					break;
				case 2:
					x = rcDraw.Right;
					y = rcDraw.Y;
					break;
				case 3:
					x = rcDraw.Right;
					y = rcDraw.Bottom;
					break;
				case 4:
					x = rcDraw.Left;
					y = rcDraw.Bottom;
					break;
				case 5:
					x = rcDraw.Left + Math.Min(_Radius, rcDraw.Width/2);
					y = yCenter;
					break;
				case 6:
					x = xCenter; 
					y = rcDraw.Top + Math.Min(_Radius, rcDraw.Height/2);
					break;
				case 7:
					x = rcDraw.Right - Math.Min(_Radius, rcDraw.Width/2);
					y = yCenter;
					break;
				case 8:
					x = xCenter;
					y = rcDraw.Bottom - Math.Min(_Radius, rcDraw.Height/2);
					break;
			}

			//return Point.Round(new PointF(x, y));
            if (Container.MetroSys != null)
                return Container.MetroSys.ToPixel(new PointF(x, y));
            return new PointF(x, y);
		}

		/// <summary>
		/// Hit test.
		/// Return value: -1 - no hit
		///                0 - hit anywhere
		///                > 1 - handle number
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		public override int HitTest(PointF point)
		{
			if (Selected)
			{
                PointF ptPix = point;
                if (Container.MetroSys != null)
                    ptPix = Container.MetroSys.ToPixel(ptPix);
				for ( int i = 1; i <= HandleCount; i++ )
				{
					if ( GetHandleRectangle(i).Contains(ptPix) )
						return i;
				}
			}

			if ( PointInObject(point) )
				return 0;

			return -1;
		}


        public override bool PointInObject(PointF point)
		{
			float left = (float)(_Rectangle.Left + _Radius);
			float top = (float)(_Rectangle.Top  + _Radius);
			float right = (float)(_Rectangle.Right - _Radius);
			float bottom = (float)(_Rectangle.Bottom - _Radius);
			RectangleF rcClip = new RectangleF(left, top, right-left, bottom-top);

			GraphicsPath path = new GraphicsPath();
			path.AddEllipse(this._Rectangle);
			path.AddEllipse(rcClip);
			bool bRes = path.IsVisible(point);
			path.Dispose();
			return bRes;
		}

		/// <summary>
		/// Get cursor for the handle
		/// </summary>
		/// <param name="handleNumber"></param>
		/// <returns></returns>
		public override Cursor GetHandleCursor(int handleNumber)
		{
            return Resources.ApplicationCursors.DrawEllipse;
			/*switch ( handleNumber )
			{
				case 1:
					return Cursors.SizeNWSE;
				case 2:
					return Cursors.SizeNESW;
				case 3:
					return Cursors.SizeNWSE;
				case 4:
					return Cursors.SizeNESW;
				case 5:
					return Cursors.SizeWE;
				case 6:
					return Cursors.SizeNS;
				case 7:
					return Cursors.SizeWE;
				case 8:
					return Cursors.SizeNS;
				default:
					return Resources.ApplicationCursors.DrawOnionRing;
			}*/
		}

		public override void MoveHandleTo(PointF point, int handleNumber)
		{
			float left = Rectangle.Left;
			float top = Rectangle.Top;
			float right = Rectangle.Right;
			float bottom = Rectangle.Bottom;

			float subLeft = _Rectangle.Left - _Radius;
			float subTop  = _Rectangle.Top  - _Radius;
			float subRight = _Rectangle.Right - _Radius;
			float subBottom = _Rectangle.Bottom - _Radius;

			float OldRadius = _Radius;	
			float OldWidth  = Math.Abs(_Rectangle.Right - _Rectangle.Left);
			float OldHeight  = Math.Abs(_Rectangle.Bottom - _Rectangle.Top);
			if (_Ratio <= 0 && OldWidth > 0) 
				_Ratio =  OldRadius / OldWidth;
			
			float dx = 0;
			float dy = 0;

			if (_pointLast != Point.Empty)
			{
				dx = point.X - _pointLast.X ;
				dy = point.Y - _pointLast.Y;
			}		
			_pointLast.X = point.X;
			_pointLast.Y = point.Y;

			switch ( handleNumber )
			{
				case 1:
					left = point.X;
					top = point.Y;
					break;
				case 2:
					top = point.Y;
					right = point.X;
					break;
				case 3:
					right = point.X;
					bottom = point.Y;
					break;
				case 4:
					left = point.X;
					bottom = point.Y;
					break;
				case 5:			
					_Radius += dx;
					break;
				case 6:
					_Radius += dy;
					break;
				case 7:
					_Radius -= dx;
					break;
				case 8:
					_Radius -= dy;
					break;
				default:
					break;
			}

			if (handleNumber > 4 && (_Radius < 1 || _Radius > Math.Min(_Rectangle.Width/2, _Rectangle.Height/2)))
				_Radius = OldRadius;

			if ((System.Windows.Forms.Control.ModifierKeys & Keys.Shift) == Keys.Shift && handleNumber!= 5 && handleNumber!= 6 && handleNumber!= 7 && handleNumber!= 8)
			{
				float width  = right - left;
				float height = bottom - top;
					
				if (width >0 && height >0 || width < 0 && height < 0)
				{
					if (width < height )
					{
						height = width;
						if (handleNumber == 1 || handleNumber == 2)
							top  += ( bottom - top) - (right - left);
						else
						{
							if (handleNumber == 4)
								bottom += ( bottom - top) - (right - left);
						}
					}
					else
					{
						width = height;
						if (handleNumber == 1 || handleNumber == 4)
							left += (right - left) - ( bottom - top);	
						else
						{
							if (handleNumber == 2)
								right  += (right - left) - ( bottom - top) ;
						}
					}
				}
				else				
				{
					if (width < height )
					{
						height = -(width);
						if (handleNumber == 1 || handleNumber == 2)
							top  += ( bottom - top) + (right - left);
						else
						{
							if (handleNumber == 4)
								bottom += ( bottom - top) + (right - left);
						}
							
					}
					else
					{
						width = -(height);
						if (handleNumber == 1 || handleNumber == 4)
							left += (right - left) + ( bottom - top);
						else
						{
							if (handleNumber == 2)
								right  += (right - left) + ( bottom - top) ;
						}
							
					}
				}
	
				// enable keep radius aspect ratio
				if (handleNumber <= 4 && OldWidth > 0 && Math.Abs(width) > 0)
					this._Radius = Math.Abs(width) * _Ratio;
				
				SetRectangle(left, top, width, height);				
			}
			else
			{
				// enable keep radius aspect ratio
				if (handleNumber <= 4 && OldWidth > 0)
					//this._Radius = (float)(Math.Abs(right-left) * OldRadius) / (float)OldWidth;
					this._Radius = Math.Abs(right-left)*_Ratio;
				
				SetRectangle(left, top, right - left, bottom - top);
			}

			// update ratio
			if (handleNumber > 4 && this._Rectangle.Width > 0)
				_Ratio = _Radius / this._Rectangle.Width;
		}
		
		public override bool IntersectsWith(RectangleF rectangle)
		{
			return Rectangle.IntersectsWith(rectangle);
		}

		/// <summary>
		/// Move object
		/// </summary>
		/// <param name="deltaX"></param>
		/// <param name="deltaY"></param>
		public override void Move(float deltaX, float deltaY)
		{
			_Rectangle.X += deltaX;
			_Rectangle.Y += deltaY;
		}

		public override void Dump()
		{
			base.Dump ();

			Trace.WriteLine("rectangle.X = " + _Rectangle.X.ToString(CultureInfo.InvariantCulture));
			Trace.WriteLine("rectangle.Y = " + _Rectangle.Y.ToString(CultureInfo.InvariantCulture));
			Trace.WriteLine("rectangle.Width = " + _Rectangle.Width.ToString(CultureInfo.InvariantCulture));
			Trace.WriteLine("rectangle.Height = " + _Rectangle.Height.ToString(CultureInfo.InvariantCulture));
		}

		public override void SetLastPoint(PointF pt)
		{
			this._pointLast = pt;
		}

		/// <summary>
		/// Normalize rectangle
		/// </summary>
		public override void Normalize()
		{
			_Rectangle = DrawRectangle.GetNormalizedRectangle(_Rectangle);
		}


		/// <summary>
		/// Transform objects
		/// </summary>
		/// <param name="transform"></param>
		public override void Transform(Matrix transform)
		{
			float left = _Rectangle.Left;
			float top  = _Rectangle.Top;
			float right = _Rectangle.Right;
			float bottom = _Rectangle.Bottom;
			
			float XCenter = (float)(_Rectangle.Left + (float)_Rectangle.Width/2.0f);
			float YCenter = (float)(_Rectangle.Top  + (float)_Rectangle.Height/2.0f);
			float delta = (float)((float)_Rectangle.Width/2.0f - _Radius);
			
			float subLeft = XCenter - delta;
			float subTop = YCenter - delta;
			float subRight = subLeft + 2*delta;
			float subBottom = subTop + 2*delta;
			
			PointF[] pts = new PointF[] { 
										 new PointF(left, top),
										 new PointF(right, bottom),
										 new PointF(subLeft, subTop),
										 new PointF(subRight, subBottom)
									    };
			transform.TransformPoints(pts);

			left	= pts[0].X;
			top		= pts[0].Y;
			right	= pts[1].X;
			bottom	= pts[1].Y;

			subLeft		= pts[2].X;
			subTop		= pts[2].Y;
			subRight	= pts[3].X;
			subBottom	= pts[3].Y;

			RectangleF newRectangle = new RectangleF(left, top, right-left, bottom-top);
			RectangleF subRectangle = new RectangleF(subLeft, subTop, subRight-subLeft, subBottom-subTop);
			if (Math.Abs(newRectangle.Width - subRectangle.Width) > 0)
				_Radius = (float)(newRectangle.Width - subRectangle.Width) / 2.0f;
			//this._Rectangle = Rectangle.Round(newRectangle);
            this._Rectangle = newRectangle;
		}

		public override DrawObject Copy()
		{
			DrawOnionRing cloneObject = new DrawOnionRing();
			cloneObject._Rectangle = this._Rectangle;
			cloneObject._Radius = this._Radius;
			cloneObject.Color = this.Color;
			cloneObject.BrushColor = this.BrushColor;
			cloneObject.TrackerBrushColor = this.TrackerBrushColor;
			cloneObject.TrackerPenColor = this.TrackerPenColor;
			cloneObject.PenWidth = this.PenWidth;

            cloneObject.Initialized = this.Initialized;
            cloneObject.Description = this.Description;
			return cloneObject;
		}

		/// <summary>
		/// Save object to serialization stream
		/// </summary>
		/// <param name="info"></param>
		/// <param name="orderNumber"></param>
		public override void SaveToStream(System.Runtime.Serialization.SerializationInfo info, int orderNumber)
		{
			info.AddValue(
				String.Format(CultureInfo.InvariantCulture,
				"{0}{1}",
				entryOnionRing_Rectangle, orderNumber),
				this._Rectangle);
			info.AddValue(
				String.Format(CultureInfo.InvariantCulture,
				"{0}{1}",
				entryOnionRing_Radius, orderNumber),
				this._Radius);

			base.SaveToStream(info, orderNumber);
		}

		/// <summary>
		/// LOad object from serialization stream
		/// </summary>
		/// <param name="info"></param>
		/// <param name="orderNumber"></param>
		public override void LoadFromStream(SerializationInfo info, int orderNumber)
		{
			this._Rectangle = (RectangleF)info.GetValue(
				String.Format(CultureInfo.InvariantCulture,
				"{0}{1}",
				entryOnionRing_Rectangle, orderNumber),
				typeof(RectangleF));

			this._Radius = info.GetSingle(
				String.Format(CultureInfo.InvariantCulture,
				"{0}{1}",
				entryOnionRing_Radius, orderNumber));

			base.LoadFromStream (info, orderNumber);
		}

        public override void Serialize(System.IO.BinaryWriter writer)
        {
            base.Serialize(writer);

            writer.Write(this._Rectangle.Left);
            writer.Write(this._Rectangle.Top);
            writer.Write(this._Rectangle.Width);
            writer.Write(this._Rectangle.Height);
            writer.Write(this._Radius);
        }

        public override void Deserialize(System.IO.BinaryReader reader)
        {
            base.Deserialize(reader);

            this._Rectangle = new RectangleF(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            this._Radius = reader.ReadSingle();
        }
	}
}
