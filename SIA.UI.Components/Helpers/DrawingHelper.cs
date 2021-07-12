using System;
using System.Drawing;

namespace SIA.UI.Components.Helpers
{
    /// <summary>
    /// Utility class provides functionality for calculating
    /// 2D drawing operations
    /// </summary>
    public sealed class DrawingHelper
    {
		public static float FindAngle(PointF pt1, PointF pt2)
		{
			float single1 = pt2.X - pt1.X;
			float single2 = pt2.Y - pt1.Y;
			if (single2 == 0)
			{
				if (pt2.X > pt1.X)
				{
					return 0f;
				}
				return 180f;
			}
			if (single1 == 0)
			{
				if (pt2.Y > pt1.Y)
				{
					return 90f;
				}
				return 270f;
			}
			double num1 = (Math.Atan((double) Math.Abs((float) (single2 / single1))) * 180) / Math.Acos(-1);
			if ((pt2.X < pt1.X) && (pt2.Y > pt1.Y))
			{
				num1 = 180 - num1;
			}
			if ((pt2.X < pt1.X) && (pt2.Y < pt1.Y))
			{
				num1 += 180;
			}
			if ((pt2.X > pt1.X) && (pt2.Y < pt1.Y))
			{
				num1 = 360 - num1;
			}
			return (float) num1;
		}

		public static Rectangle FixRectangle(Rectangle rc)
		{
			if ((rc.Left <= rc.Right) && (rc.Top <= rc.Bottom))
			{
				return rc;
			}
			return Rectangle.FromLTRB(Math.Min(rc.Left, rc.Right), Math.Min(rc.Top, rc.Bottom), Math.Max(rc.Left, rc.Right), Math.Max(rc.Top, rc.Bottom));
		}
 
		public static RectangleF FixRectangle(RectangleF rc)
		{
			if ((rc.Left <= rc.Right) && (rc.Top <= rc.Bottom))
			{
				return rc;
			}
			return RectangleF.FromLTRB(Math.Min(rc.Left, rc.Right), Math.Min(rc.Top, rc.Bottom), Math.Max(rc.Left, rc.Right), Math.Max(rc.Top, rc.Bottom));
		}
 

 
		public static PointF[] GetBoundingPoints(RectangleF rc)
		{
			return new PointF[4] { new PointF(rc.Left, rc.Top), new PointF(rc.Right, rc.Top), new PointF(rc.Right, rc.Bottom), new PointF(rc.Left, rc.Bottom) } ;
		}
 
		public static Rectangle GetBoundingRectangle(RectangleF rc)
		{
			return DrawingHelper.FixRectangle(new Rectangle((int) rc.Left, (int) rc.Top, ((int) Math.Ceiling((double) rc.Width)) + 1, ((int) Math.Ceiling((double) rc.Height)) + 1));
		}
 
		public static RectangleF GetBoundingRectangle(PointF[] pts)
		{
			if (pts.Length == 2)
			{
				return RectangleF.FromLTRB(Math.Min(pts[0].X, pts[1].X), Math.Min(pts[0].Y, pts[1].Y), Math.Max(pts[0].X, pts[1].X), Math.Max(pts[0].Y, pts[1].Y));
			}
			if (pts.Length == 4)
			{
				return RectangleF.FromLTRB(Math.Min(pts[0].X, Math.Min(pts[1].X, Math.Min(pts[2].X, pts[3].X))), Math.Min(pts[0].Y, Math.Min(pts[1].Y, Math.Min(pts[2].Y, pts[3].Y))), Math.Max(pts[0].X, Math.Max(pts[1].X, Math.Max(pts[2].X, pts[3].X))), Math.Max(pts[0].Y, Math.Max(pts[1].Y, Math.Max(pts[2].Y, pts[3].Y))));
			}
			float single1 = pts[0].X;
			float single2 = pts[0].Y;
			float single3 = single1;
			float single4 = single2;
			for (int num1 = 1; num1 < pts.Length; num1++)
			{
				single1 = Math.Min(single1, pts[num1].X);
				single2 = Math.Min(single2, pts[num1].Y);
				single3 = Math.Max(single3, pts[num1].X);
				single4 = Math.Max(single4, pts[num1].Y);
			}
			return DrawingHelper.FixRectangle(RectangleF.FromLTRB(single1, single2, single3, single4));
		}
 

		public static Rectangle GetBoundingRectangle(Point center, Size size)
		{
			return new Rectangle(center.X - (size.Width / 2), center.Y - (size.Height / 2), size.Width, size.Height);
		}
 

		public static RectangleF GetBoundingRectangle(PointF pt1, PointF pt2)
		{
			return RectangleF.FromLTRB(Math.Min(pt1.X, pt2.X), Math.Min(pt1.Y, pt2.Y), Math.Max(pt1.X, pt2.X), Math.Max(pt1.Y, pt2.Y));
		}
 

		public static RectangleF GetBoundingRectangle(PointF center, SizeF size)
		{
			return new RectangleF(center.X - (size.Width / 2f), center.Y - (size.Height / 2f), size.Width, size.Height);
		}
 

		public static PointF GetCenterPoint(RectangleF rc)
		{
			return new PointF(rc.Left + (rc.Width / 2f), rc.Top + (rc.Height / 2f));
		}
 

		public static PointF GetCenterPoint(PointF pt1, PointF pt2)
		{
			float single1 = Math.Min(pt1.X, pt2.X);
			float single2 = Math.Min(pt1.Y, pt2.Y);
			float single3 = Math.Max(pt1.X, pt2.X);
			float single4 = Math.Max(pt1.Y, pt2.Y);
			return new PointF(single1 + ((single3 - single1) / 2f), single2 + ((single4 - single2) / 2f));
		}
 

		public static bool IsNonEmptyRectangle(Rectangle rc)
		{
			if (rc.Width != 0)
			{
				return (rc.Height != 0);
			}
			return false;
		}
 

		public static bool IsNonEmptyRectangle(RectangleF rc)
		{
			if (rc.Width != 0f)
			{
				return (rc.Height != 0f);
			}
			return false;
		}
 

		public static bool PointsAreEqual(PointF pt1, PointF pt2, float delta)
		{
			if (Math.Abs((float) (pt1.X - pt2.X)) < delta)
			{
				return (Math.Abs((float) (pt1.Y - pt2.Y)) < delta);
			}
			return false;
		}
 
    } // class DrawingHelper

}

