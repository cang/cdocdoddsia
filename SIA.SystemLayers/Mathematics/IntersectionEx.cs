using System;
using System.Collections;
using System.Drawing;

namespace SIA.SystemLayer.Mathematics
{
	public enum IntersectStatus : int
	{
		NoIntersection = 0,
		Intersect,
		Outside,
		Inside,
		Tangent,
		Coincident,	// line only
		Parallel // line only
	}
		
    /// <summary>
    /// The IntersectionEx class provides functions for finding the intersection between
    /// . Line to Line
    /// . Line to Polygon
    /// . Circle and Rectangle
    /// . Circle and Line
    /// . Polygon an Rectangle
    /// </summary>
    /// <remarks>The IntersectionEx and Intersection class seem are duplicated.</remarks>
	public class IntersectionEx
	{
		private IntersectStatus _status = IntersectStatus.NoIntersection;
		private ArrayList _points = new ArrayList();

		public IntersectStatus IntersectStatus
		{
			get {return _status;}
			set {_status = value;}
		}

		public PointF[] Points
		{
			get {return (PointF[])_points.ToArray(typeof(PointF)); }
		}

		public IntersectionEx()
		{
		}

		public IntersectionEx(IntersectStatus status)
		{
			_status = status;
		}

		public IntersectionEx(IntersectStatus status, PointF[] points)
		{
			_status = status;
			_points.AddRange(points);
		}

		public void AddPoint(PointF pt)
		{
			_points.Add(pt);
		}

		public void AddPoint(PointF[] pts)
		{
			_points.AddRange(pts);
		}

		public static IntersectionEx LineLine(PointF a1, PointF a2, PointF b1, PointF b2)
		{
			IntersectionEx result = null;
    
			float ua_t = (b2.X - b1.X) * (a1.Y - b1.Y) - (b2.Y - b1.Y) * (a1.X - b1.X);
			float ub_t = (a2.X - a1.X) * (a1.Y - b1.Y) - (a2.Y - a1.Y) * (a1.X - b1.X);
			float u_b  = (b2.Y - b1.Y) * (a2.X - a1.X) - (b2.X - b1.X) * (a2.Y - a1.Y);

			if ( u_b != 0 ) 
			{
				float ua = ua_t / u_b;
				float ub = ub_t / u_b;

				if ( 0 <= ua && ua <= 1 && 0 <= ub && ub <= 1 ) 
				{
					result = new IntersectionEx(IntersectStatus.Intersect);
					result.AddPoint(
						new PointF(
						a1.X + ua * (a2.X - a1.X),
						a1.Y + ua * (a2.Y - a1.Y)
						)
						);
				} 
				else 
				{
					result = new IntersectionEx(IntersectStatus.NoIntersection);
				}
			} 
			else 
			{
				if ( ua_t == 0 || ub_t == 0 ) 
				{
					result = new IntersectionEx(IntersectStatus.Coincident);
				} 
				else 
				{
					result = new IntersectionEx(IntersectStatus.Parallel);
				}
			}

			return result; 
		}

		public static IntersectionEx LinePolygon(PointF pt1, PointF pt2, PointF[] points)
		{
			IntersectionEx result = new IntersectionEx();
			int length = points.Length;

			for ( int i = 0; i < length; i++ ) 
			{
				PointF b1 = points[i];
				PointF b2 = points[(i+1) % length];
				IntersectionEx inter = IntersectionEx.LineLine(pt1, pt2, b1, b2);

				result.AddPoint(inter.Points);
			}

			if ( result.Points.Length > 0 ) 
				result.IntersectStatus = IntersectStatus.Intersect;

			return result; 
		}

        public static IntersectionEx CircleRectangle(PointF center, float radius, 
            float left, float top, float right, float bottom)
        {
            PointF min = new PointF(left, top);
            PointF max = new PointF(right, bottom);
            PointF topRight = new PointF(max.X, min.Y);
            PointF bottomLeft = new PointF(min.X, max.Y);

            IntersectionEx inter1 = CircleLine(center, radius, min, topRight);
            IntersectionEx inter2 = CircleLine(center, radius, topRight, max);
            IntersectionEx inter3 = CircleLine(center, radius, max, bottomLeft);
            IntersectionEx inter4 = CircleLine(center, radius, bottomLeft, min);

            IntersectionEx result = new IntersectionEx();

            result.AddPoint(inter1.Points);
            result.AddPoint(inter2.Points);
            result.AddPoint(inter3.Points);
            result.AddPoint(inter4.Points);

            if (result.Points.Length > 0)
                result.IntersectStatus = IntersectStatus.Intersect;
            else
                result.IntersectStatus = inter1.IntersectStatus;

            return result;
        }

		public static IntersectionEx CircleRectangle(PointF center, float radius, RectangleF rect)
		{
			PointF min = new PointF(rect.Left, rect.Top);
			PointF max = new PointF(rect.Right, rect.Bottom);
			PointF topRight   = new PointF( max.X, min.Y );
			PointF bottomLeft = new PointF( min.X, max.Y );
    
			IntersectionEx inter1 = CircleLine(center, radius, min, topRight);
			IntersectionEx inter2 = CircleLine(center, radius, topRight, max);
			IntersectionEx inter3 = CircleLine(center, radius, max, bottomLeft);
			IntersectionEx inter4 = CircleLine(center, radius, bottomLeft, min);
    
			IntersectionEx result = new IntersectionEx();

			result.AddPoint(inter1.Points);
			result.AddPoint(inter2.Points);
			result.AddPoint(inter3.Points);
			result.AddPoint(inter4.Points);

			if ( result.Points.Length > 0 )
				result.IntersectStatus = IntersectStatus.Intersect;
			else
				result.IntersectStatus = inter1.IntersectStatus;

			return result; 
		}

		public static IntersectionEx CircleLine(PointF center, float radius, PointF pt1, PointF pt2)
		{
			IntersectionEx result;
			float a  = (pt2.X - pt1.X) * (pt2.X - pt1.X) +
				(pt2.Y - pt1.Y) * (pt2.Y - pt1.Y);
			float b  = 2 * ( (pt2.X - pt1.X) * (pt1.X - center.X) +
				(pt2.Y - pt1.Y) * (pt1.Y - center.Y)   );
			float cc = center.X*center.X + center.Y*center.Y + pt1.X*pt1.X + pt1.Y*pt1.Y -
				2 * (center.X * pt1.X + center.Y * pt1.Y) - radius*radius;
			float deter = b*b - 4*a*cc;

			if ( deter < 0 ) 
			{
				result = new IntersectionEx(IntersectStatus.Outside);
			} 
			else if ( deter == 0 ) 
			{
				result = new IntersectionEx(IntersectStatus.Tangent);
				// NOTE: should calculate this point
			} 
			else 
			{
				float e  = (float)Math.Sqrt(deter);
				float u1 = ( -b + e ) / ( 2*a );
				float u2 = ( -b - e ) / ( 2*a );

				if ( (u1 < 0 || u1 > 1) && (u2 < 0 || u2 > 1) ) 
				{
					if ( (u1 < 0 && u2 < 0) || (u1 > 1 && u2 > 1) ) 
					{
						result = new IntersectionEx(IntersectStatus.Outside);
					} 
					else 
					{
						result = new IntersectionEx(IntersectStatus.Inside);
					}
				} 
				else 
				{
					result = new IntersectionEx(IntersectStatus.Intersect);

					if ( 0 <= u1 && u1 <= 1)
						result.AddPoint( Lerp(pt1, pt2, u1));//  pt1.lerp(pt2, u1) );

					if ( 0 <= u2 && u2 <= 1)
						result.AddPoint( Lerp(pt1, pt2, u2));// pt1.lerp(pt2, u2) );
				}
			}
    
			return result; 
		}


		public static IntersectionEx PolygonRectangle(PointF[] points, RectangleF rect)
		{
			PointF min        = new PointF(rect.Left, rect.Top);
			PointF max        = new PointF(rect.Right, rect.Bottom);
			PointF topRight   = new PointF( max.X, min.Y );
			PointF bottomLeft = new PointF( min.X, max.Y );
    
			IntersectionEx inter1 = IntersectionEx.LinePolygon(min, topRight, points);
			IntersectionEx inter2 = IntersectionEx.LinePolygon(topRight, max, points);
			IntersectionEx inter3 = IntersectionEx.LinePolygon(max, bottomLeft, points);
			IntersectionEx inter4 = IntersectionEx.LinePolygon(bottomLeft, min, points);
    
			IntersectionEx result = new IntersectionEx();

			result.AddPoint(inter1.Points);
			result.AddPoint(inter2.Points);
			result.AddPoint(inter3.Points);
			result.AddPoint(inter4.Points);

			if ( result.Points.Length > 0 )
				result.IntersectStatus = IntersectStatus.Intersect;

			return result; 
		}

		public static PointF Lerp(PointF pt1, PointF pt2, float t)
		{
			float xPos = pt1.X + (pt2.X - pt1.X) * t;
			float yPos = pt1.Y + (pt2.Y - pt1.Y) * t;
			return new PointF(xPos, yPos);
		}
	}
}
