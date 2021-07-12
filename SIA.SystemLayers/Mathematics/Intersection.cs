using System;
using System.Collections;
using System.Drawing;

namespace SIA.SystemLayer.Mathematics
{
    /// <summary>
    /// The Intersection class provide functions for finding intersection between
    /// . A circle and a line
    /// . A circle and a circle
    /// . A circle and a polygon
    /// </summary>
	public class Intersection
	{
		private string _status = string.Empty;
		private ArrayList _points = new ArrayList();

		public String Status
		{
			get {return _status;}
		}

		public PointF[] PointFs
		{
			get {return (PointF[])_points.ToArray(typeof(PointF));}
		}

		public Intersection(string status)
		{
			_status = status;
		}

		public static Intersection IntersectCircleLine(PointF center, double radius, PointF pt1, PointF pt2)
		{
			ArrayList points = new ArrayList();
			Intersection result = null;
			PointF a1 = pt1, a2 = pt2, c = center;
			double r = radius;
			double a  = (a2.X - a1.X) * (a2.X - a1.X) +
				(a2.Y - a1.Y) * (a2.Y - a1.Y);
			double b  = 2 * ( (a2.X - a1.X) * (a1.X - c.X) +
				(a2.Y - a1.Y) * (a1.Y - c.Y)   );
			double cc = c.X*c.X + c.Y*c.Y + a1.X*a1.X + a1.Y*a1.Y -
				2 * (c.X * a1.X + c.Y * a1.Y) - r*r;
			double deter = b*b - 4*a*cc;

			if ( deter < 0 ) 
			{
				result = new Intersection("Outside");				
			} 
			else if ( deter == 0 ) 
			{
				result = new Intersection("Tangent");
				// NOTE: should calculate this point
			} 
			else 
			{
				double e  = Math.Sqrt(deter);
				double u1 = ( -b + e ) / ( 2*a );
				double u2 = ( -b - e ) / ( 2*a );

				if ( (u1 < 0 || u1 > 1) && (u2 < 0 || u2 > 1) ) 
				{
					if ( (u1 < 0 && u2 < 0) || (u1 > 1 && u2 > 1) ) 
					{
						result = new Intersection("Outside");
					} 
					else 
					{
						result = new Intersection("Inside");
					}
				} 
				else 
				{
					result = new Intersection("Intersection");

					if ( 0 <= u1 && u1 <= 1)
						result._points.Add( Lerp(a1, a2, u1));// a1.lerp(a2, u1) );

					if ( 0 <= u2 && u2 <= 1)
						result._points.Add( Lerp(a1, a2, u2));// a1.lerp(a2, u2) );
				}
			}
    
			return result;
		}

		public static Intersection IntersectCircleCircle(PointF c1, double r1, PointF c2, double r2)
		{
			Intersection result = null;
    
			// Determine minimum and maximum radii where circles can intersect
			double r_max = r1 + r2;
			double r_min = Math.Abs(r1 - r2);
    
			// Determine actual distance between circle circles
			double c_dist = Distance(c1, c2);

			if ( c_dist > r_max ) 
			{
				result = new Intersection("Outside");
			} 
			else if ( c_dist < r_min ) 
			{
				result = new Intersection("Inside");
			} 
			else 
			{
				result = new Intersection("Intersection");

				double a = (r1*r1 - r2*r2 + c_dist*c_dist) / ( 2*c_dist );
				double h = Math.Sqrt(r1*r1 - a*a);
				PointF p = Lerp(c1, c2, a/c_dist);
				double b = h / c_dist;

				result._points.Add(
					new PointF(
					(float)(p.X - b * (c2.Y - c1.Y)),
					(float)(p.Y + b * (c2.X - c1.X))
					));
				result._points.Add(
					new PointF(
					(float)(p.X + b * (c2.Y - c1.Y)),
					(float)(p.Y - b * (c2.X - c1.X))
					)
					);
			}

			return result;
		}

		public static Intersection IntersectionCirclePolygon(PointF c, double r, PointF[] points)
		{
			Intersection result = new Intersection("No Intersection");
			int length = points.Length;
			Intersection inter = null;

			for ( int i = 0; i < length; i++ ) 
			{
				PointF a1 = points[i];
				PointF a2 = points[(i+1) % length];

				inter = Intersection.IntersectCircleLine(c, r, a1, a2);
				result._points.Add(inter._points);
			}

			if ( result._points.Count > 0 )
				result._status = "Intersection";
			else
				result._status = inter._status;

			return result;
		}

		private static PointF Lerp(PointF from, PointF to, double frac)
		{
			return new PointF((float)Lerp(from.X, to.X, frac), (float)Lerp(from.Y, to.Y, frac));
		}

		private static double Lerp(double from, double to, double frac)
		{
			//return (from * (1 - frac) + to * frac);
			return from + (to-from) * frac;
		}

		private static double Distance(PointF from, PointF to)
		{
			double square = (from.X-to.X)*(from.X-to.X) + (from.Y-to.Y)*(from.Y-to.Y);
			if (square == 0)
				throw new System.Exception("Invalid parameter");
			return Math.Sqrt(square);
		}
	}
}
