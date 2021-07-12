using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Globalization;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

namespace SiGlaz.Common
{
	/// <summary>
	/// Line graphic object
	/// </summary>
	public class DrawLine : DrawObject
	{
        private PointF startPoint;
        private PointF endPoint;

        private const string entryStart = "Start";
        private const string entryEnd = "End";

        /// <summary>
        ///  Graphic objects for hit test
        /// </summary>
        private GraphicsPath areaPath = null;
        private Pen areaPen = null;
        private Region areaRegion = null;


		public DrawLine()
		{
            startPoint.X = 0;
            startPoint.Y = 0;
            endPoint.X = 1;
            endPoint.Y = 1;

            Initialize();
		}

        public DrawLine(float x1, float y1, float x2, float y2)
        {
            startPoint.X = x1;
            startPoint.Y = y1;
            endPoint.X = x2;
            endPoint.Y = y2;

            Initialize();
        }

		public DrawLine(DrawLine obj) : base(obj)
		{
			startPoint = obj.startPoint;
			endPoint = obj.endPoint;

			Initialize();
		}


        public override void Draw(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Pen pen = new Pen(Color, PenWidth);

            g.DrawLine(pen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);

            pen.Dispose();
        }

		public override void Draw(SIA.Common.Mask.IMask mask)
		{
			mask.DrawLine(startPoint, endPoint);
		}

        public override int HandleCount
        {
            get
            {
                return 2;
            }
        }

        /// <summary>
        /// Get handle point by 1-based number
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public override PointF GetHandle(int handleNumber)
        {
            if ( handleNumber == 1 )
                return startPoint;
            else
                return endPoint;
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
                    if (GetHandleRectangle(i).Contains(ptPix))
                    {
                        return i;
                    }
                }
            }

            if ( PointInObject(point) )
                return 0;

            return -1;
        }

        public override bool PointInObject(PointF point)
        {
            CreateObjects();

            return AreaRegion.IsVisible(point);
        }

        public override bool IntersectsWith(RectangleF rectangle)
        {
            CreateObjects();

            return AreaRegion.IsVisible(rectangle);
        }

        public override Cursor GetHandleCursor(int handleNumber)
        {
            switch ( handleNumber )
            {
                case 1:
                case 2:
                    return Cursors.SizeAll;
                default:
                    return Resources.ApplicationCursors.DrawLine;
            }
        }

        public override void MoveHandleTo(PointF point, int handleNumber)
        {
            if ( handleNumber == 1 )
                startPoint = point;
            else
                endPoint = point;

            Invalidate();
        }

		public override void Transform(Matrix transform)
		{
			PointF[] pts = new PointF[] { (PointF)startPoint, (PointF)endPoint};
			transform.TransformPoints(pts);
            //startPoint = Point.Round(pts[0]);
            //endPoint = Point.Round(pts[1]);
            startPoint = pts[0];
            endPoint = pts[1];

		}

        public override void Move(float deltaX, float deltaY)
        {
            startPoint.X += deltaX;
            startPoint.Y += deltaY;

            endPoint.X += deltaX;
            endPoint.Y += deltaY;

            Invalidate();
        }

		public override DrawObject	Copy()
		{
			DrawLine cloneObject = new DrawLine();
			cloneObject.startPoint = this.startPoint;
			cloneObject.endPoint = this.endPoint;

			cloneObject.Color = this.Color;
			cloneObject.BrushColor = this.BrushColor;
			cloneObject.TrackerBrushColor = this.TrackerBrushColor;
			cloneObject.TrackerPenColor = this.TrackerPenColor;
			cloneObject.PenWidth = this.PenWidth;

            cloneObject.Initialized = this.Initialized;
            cloneObject.Description = this.Description;
			return cloneObject;
		}

        public override void SaveToStream(System.Runtime.Serialization.SerializationInfo info, int orderNumber)
        {
            info.AddValue(
                String.Format(CultureInfo.InvariantCulture,
                "{0}{1}",
                entryStart, orderNumber),
                startPoint);

            info.AddValue(
                String.Format(CultureInfo.InvariantCulture,
                "{0}{1}",
                entryEnd, orderNumber),
                endPoint);

            base.SaveToStream (info, orderNumber);
        }

        public override void LoadFromStream(SerializationInfo info, int orderNumber)
        {
            startPoint = (PointF)info.GetValue(
                String.Format(CultureInfo.InvariantCulture,
                "{0}{1}",
                entryStart, orderNumber),
                typeof(PointF));

            endPoint = (PointF)info.GetValue(
                String.Format(CultureInfo.InvariantCulture,
                "{0}{1}",
                entryEnd, orderNumber),
                typeof(PointF));

            base.LoadFromStream (info, orderNumber);
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);

            writer.Write(startPoint.X);
            writer.Write(startPoint.Y);
            writer.Write(endPoint.X);
            writer.Write(endPoint.Y);
        }

        public override void Deserialize(BinaryReader reader)
        {
            base.Deserialize(reader);

            startPoint = new PointF(reader.ReadSingle(), reader.ReadSingle());
            endPoint = new PointF(reader.ReadSingle(), reader.ReadSingle());
        }

        /// <summary>
        /// Invalidate object.
        /// When object is invalidated, path used for hit test
        /// is released and should be created again.
        /// </summary>
        protected void Invalidate()
        {
            if ( AreaPath != null )
            {
                AreaPath.Dispose();
                AreaPath = null;
            }

            if ( AreaPen != null )
            {
                AreaPen.Dispose();
                AreaPen = null;
            }

            if ( AreaRegion != null )
            {
                AreaRegion.Dispose();
                AreaRegion = null;
            }
        }

        /// <summary>
        /// Create graphic objects used from hit test.
        /// </summary>
        protected virtual void CreateObjects()
        {
            if ( AreaPath != null )
                return;

            // Create path which contains wide line
            // for easy mouse selection
            AreaPath = new GraphicsPath();
            AreaPen = new Pen(Color.Black, 7);
            AreaPath.AddLine(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
            AreaPath.Widen(AreaPen);

            // Create region from the path
            AreaRegion = new Region(AreaPath);
        }

        protected GraphicsPath AreaPath
        {
            get
            {
                return areaPath;
            }
            set
            {
                areaPath = value;
            }
        }

        protected Pen AreaPen
        {
            get
            {
                return areaPen;
            }
            set
            {
                areaPen = value;
            }
        }

        protected Region AreaRegion
        {
            get
            {
                return areaRegion;
            }
            set
            {
                areaRegion = value;
            }
        }

		public override RectangleF BoundingRectangle
		{
			get
			{
				float min_x = Math.Min(startPoint.X, endPoint.X);
                float min_y = Math.Min(startPoint.Y, endPoint.Y);
                float max_x = Math.Max(startPoint.X, endPoint.X);
                float max_y = Math.Max(startPoint.Y, startPoint.Y);
				return new RectangleF(min_x, min_y, max_x, max_y);
			}
		}

	}
}
