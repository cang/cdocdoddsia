using System.Drawing;

namespace SIA.UI.Components
{
    /// <summary>
    /// Encapsulates the list of point
    /// </summary>
    public class RasterPointCollection 
        : System.Collections.CollectionBase
    {
		public RasterPointCollection()
		{
		}


		public Point this[int index]
		{
			get
			{
				return (Point) base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}
 
		public int Add(Point pt)
		{
			return base.List.Add(pt);
		}

		public void AddRange(Point[] pts)
		{
			for (int i=0; i<pts.Length; i++)
				base.List.Add(pts[i]);
		}

		public void AddRange(PointF[] pts)
		{
			for (int i=0; i<pts.Length; i++)
				base.List.Add(Point.Round(pts[i]));
		}
 
		public bool Contains(Point pt)
		{
			return base.List.Contains(pt);
		}
 
		public int IndexOf(Point pt)
		{
			return base.List.IndexOf(pt);
		}
 
		public void Insert(int index, Point pt)
		{
			base.List.Insert(index, pt);
		}
 
		public void Remove(Point pt)
		{
			base.List.Remove(pt);
		}

		public Point[] ToPoints()
		{
			Point[] pointArray1 = new Point[this.Count];
			for (int num1 = 0; num1 < this.Count; num1++)
			{
				pointArray1[num1] = this[num1];
			}
			return pointArray1;
		}

		public PointF[] ToPointFs()
		{
			PointF[] pointArray1 = new PointF[this.Count];
			for (int num1 = 0; num1 < this.Count; num1++)
				pointArray1[num1] = this[num1];
			
			return pointArray1;
		}
 
    } // class RasterPointCollection

}

