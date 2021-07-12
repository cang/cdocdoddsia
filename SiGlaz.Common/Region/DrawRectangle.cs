using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;

namespace SiGlaz.Common
{
	/// <summary>
	/// Rectangle graphic object
	/// </summary>
	public class DrawRectangle : DrawObject
	{
        private RectangleF rectangle;
		private PointF pointLast = PointF.Empty;
		// Entry names for serialization
        private const string entryRectangle = "Rect";

        public RectangleF Rectangle
        {
            get
            {
                return rectangle;
            }
            set
            {
                rectangle = value;
            }
        }

		public override RectangleF BoundingRectangle
		{
			get
			{
				return rectangle;
			}
		}
        
		public DrawRectangle()
		{
            SetRectangle(0, 0, 1,1);
            Initialize();
		}

        
        public DrawRectangle(float x, float y, float width, float height)
        {
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;
            Initialize();
        }

		public DrawRectangle(DrawRectangle obj) : base(obj)
		{
			rectangle = obj.rectangle;
			Initialize();
		}


        /// <summary>
        /// Draw rectangle
        /// </summary>
        /// <param name="g"></param>
        public override void Draw(Graphics g)
        {
            Pen pen = new Pen(Color, PenWidth);
			Brush brush = new SolidBrush(BrushColor);

            GraphicsPath path = CreateGraphicsPath();
            if (Container.MetroSys != null)
                path.Transform(Container.MetroSys.InvTransformer);
            g.FillPath(brush, path);
            g.DrawPath(pen, path);

            ////g.FillRectangle(brush, rectDraw);
            ////g.DrawRectangle(pen, rectDraw.Left, rectDraw.Top, rectDraw.Width, rectDraw.Height);			

            pen.Dispose();
			brush.Dispose();
        }

        public override GraphicsPath CreateGraphicsPath()
        {
            GraphicsPath path = base.CreateGraphicsPath();

            path.AddRectangle(DrawRectangle.GetNormalizedRectangle(this.Rectangle));

            return path;
        }

		public override void Draw(SIA.Common.Mask.IMask mask)
		{
			RectangleF rectDraw = DrawRectangle.GetNormalizedRectangle(this.Rectangle);
			mask.FillRectangle(rectDraw);
		}

        protected void SetRectangle(float x, float y, float width, float height)
        {
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;
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
            float x, y, xCenter, yCenter;

            xCenter = rectangle.X + rectangle.Width/2;
            yCenter = rectangle.Y + rectangle.Height/2;
            x = rectangle.X;
            y = rectangle.Y;

            switch ( handleNumber )
            {
                case 1:
                    x = rectangle.X;
                    y = rectangle.Y;
                    break;
                case 2:
                    x = xCenter;
                    y = rectangle.Y;
                    break;
                case 3:
                    x = rectangle.Right;
                    y = rectangle.Y;
                    break;
                case 4:
                    x = rectangle.Right;
                    y = yCenter;
                    break;
                case 5:
                    x = rectangle.Right;
                    y = rectangle.Bottom;
                    break;
                case 6:
                    x = xCenter;
                    y = rectangle.Bottom;
                    break;
                case 7:
                    x = rectangle.X;
                    y = rectangle.Bottom;
                    break;
                case 8:
                    x = rectangle.X;
                    y = yCenter;
                    break;
            }

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
            if ( Selected )
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
            return rectangle.Contains(point);
        }
        

		/// <summary>
		/// Override Get Bound rectangle
		/// </summary>
		/// <returns> bounding rectangle</returns>
		public override RectangleF GetBounds()
		{
			return rectangle;
		}

		/// <summary>
        /// Get cursor for the handle
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public override Cursor GetHandleCursor(int handleNumber)
        {
            return Resources.ApplicationCursors.DrawRectangle;
            /*switch ( handleNumber )
            {
                case 1:
                    return Cursors.SizeNWSE;
                case 2:
                    return Cursors.SizeNS;
                case 3:
                    return Cursors.SizeNESW;
                case 4:
                    return Cursors.SizeWE;
                case 5:
                    return Cursors.SizeNWSE;
                case 6:
                    return Cursors.SizeNS;
                case 7:
                    return Cursors.SizeNESW;
                case 8:
                    return Cursors.SizeWE;
                default:
                    return Resources.ApplicationCursors.DrawRectangle;
            }*/
        }

		
        /// <summary>
        /// Move handle to new point (resizing)
        /// </summary>
        /// <param name="point"></param>
        /// <param name="handleNumber"></param>
        public override void MoveHandleTo(PointF point, int handleNumber)
        {
            float left = Rectangle.Left;
            float top = Rectangle.Top;
            float right = Rectangle.Right;
            float bottom = Rectangle.Bottom;

			switch ( handleNumber )
            {
                case 1:					
                    left = point.X;
                    top = point.Y;
//					if (pointLast != Point.Empty )
//					{
//						dx = (pointLast.X - left) - (right - left)  ;
//						dy = (pointLast.Y - top ) - (bottom - top)  ;
//					}					
                    break;
                case 2:
                    top = point.Y;
                    break;
                case 3:
					top = point.Y;
					right = point.X;
					break;
                case 4:
                    right = point.X;
                    break;
                case 5:
					right = point.X;
                    bottom = point.Y;
                    break;
                case 6:
                    bottom = point.Y;
                    break;
                case 7:
                    left = point.X;
                    bottom = point.Y;
                    break;
                case 8:
                    left = point.X;
                    break;
            }
			
            //SetRectangle(left, top, right - left, bottom - top);
			if ((System.Windows.Forms.Control.ModifierKeys & Keys.Shift) == Keys.Shift && handleNumber!= 2 && handleNumber!= 4 && handleNumber!= 6 && handleNumber!= 8)
			{
				
				float width  = right - left;
				float height = bottom - top;
					
				Console.WriteLine("{0},{1}", width, height);
				if (width >0 && height >0 || width < 0 && height < 0)
				{
					if (width < height )
					{
						height = width;
						if (handleNumber == 1 || handleNumber == 3)
							top  += ( bottom - top) - (right - left);
						else
						{
							if (handleNumber == 7)
								 bottom += ( bottom - top) - (right - left);
						}
					}
					else
					{
						width = height;
						if (handleNumber == 1 || handleNumber == 7)
							left += (right - left) - ( bottom - top);
						else
						{
								if (handleNumber == 3)
								right  += (right - left) - ( bottom - top) ;
						}
					}
				}
				else				
				{
					if (width < height )
					{
						height = -(width);
						if (handleNumber == 1 || handleNumber == 3)
							top  += ( bottom - top) + (right - left);
						else
						{
							if (handleNumber == 7)
								bottom += ( bottom - top) + (right - left);
						}
					}
					else
					{
						width = -(height);
						if (handleNumber == 1 || handleNumber == 7)
							left += (right - left) + ( bottom - top);
						else
						{
							if (handleNumber == 3)
								right  += (right - left) + ( bottom - top) ;
						}
					}
				}

				
					SetRectangle(left, top, width, height);				
			}
			else
				SetRectangle(left, top, right - left, bottom - top);

			
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
            rectangle.X += deltaX;
            rectangle.Y += deltaY;
        }

        public override void Dump()
        {
            base.Dump ();

            Trace.WriteLine("rectangle.X = " + rectangle.X.ToString(CultureInfo.InvariantCulture));
            Trace.WriteLine("rectangle.Y = " + rectangle.Y.ToString(CultureInfo.InvariantCulture));
            Trace.WriteLine("rectangle.Width = " + rectangle.Width.ToString(CultureInfo.InvariantCulture));
            Trace.WriteLine("rectangle.Height = " + rectangle.Height.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Normalize rectangle
        /// </summary>
        public override void Normalize()
        {
            rectangle = DrawRectangle.GetNormalizedRectangle(rectangle);
        }

		public override void Transform(Matrix transform)
		{
			float left = rectangle.Left;
			float top  = rectangle.Top;
			float right = rectangle.Right;
			float bottom = rectangle.Bottom;
			
			PointF[] pts = new PointF[] {new PointF(left, top),
									     new PointF(right, bottom)};
			transform.TransformPoints(pts);

			//rectangle = DrawRectangle.GetNormalizedRectangle(Point.Round(pts[0]), Point.Round(pts[1]));
            rectangle = DrawRectangle.GetNormalizedRectangle(pts[0], pts[1]);
		}

		public override DrawObject Copy()
		{
			DrawRectangle cloneObject = new DrawRectangle();
			cloneObject.Rectangle = this.Rectangle;
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
                entryRectangle, orderNumber),
                rectangle);

            base.SaveToStream (info, orderNumber);
        }

        /// <summary>
        /// LOad object from serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public override void LoadFromStream(SerializationInfo info, int orderNumber)
        {
            rectangle = (RectangleF)info.GetValue(
                String.Format(CultureInfo.InvariantCulture,
                "{0}{1}",
                entryRectangle, orderNumber),
                typeof(RectangleF));

            base.LoadFromStream (info, orderNumber);
        }

        public override void Serialize(System.IO.BinaryWriter writer)
        {
            base.Serialize(writer);

            writer.Write(rectangle.Left);
            writer.Write(rectangle.Top);
            writer.Write(rectangle.Width);
            writer.Write(rectangle.Height);
        }

        public override void Deserialize(System.IO.BinaryReader reader)
        {
            base.Deserialize(reader);

            rectangle = new RectangleF(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        #region Helper Functions

        public static Rectangle GetNormalizedRectangle(int x1, int y1, int x2, int y2)
        {
            if ( x2 < x1 )
            {
                int tmp = x2;
                x2 = x1;
                x1 = tmp;
            }

            if ( y2 < y1 )
            {
                int tmp = y2;
                y2 = y1;
                y1 = tmp;
            }

            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }

		public static RectangleF GetNormalizedRectangle(float x1, float y1, float x2, float y2)
		{
			if ( x2 < x1 )
			{
				float tmp = x2;
				x2 = x1;
				x1 = tmp;
			}

			if ( y2 < y1 )
			{
				float tmp = y2;
				y2 = y1;
				y1 = tmp;
			}

			return new RectangleF(x1, y1, x2 - x1, y2 - y1);
		}

        public static Rectangle GetNormalizedRectangle(Point p1, Point p2)
        {
            return GetNormalizedRectangle(p1.X, p1.Y, p2.X, p2.Y);
        }
        public static RectangleF GetNormalizedRectangle(PointF p1, PointF p2)
        {
            return GetNormalizedRectangle(p1.X, p1.Y, p2.X, p2.Y);
        }

        public static Rectangle GetNormalizedRectangle(Rectangle r)
        {
            return GetNormalizedRectangle(r.X, r.Y, r.X + r.Width, r.Y + r.Height);
        }
        public static RectangleF GetNormalizedRectangle(RectangleF r)
        {
            return GetNormalizedRectangle(r.X, r.Y, r.X + r.Width, r.Y + r.Height);
        }

        #endregion

    }
}
