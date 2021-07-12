using System;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Globalization;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Collections.Generic;

namespace SiGlaz.Common
{
	/// <summary>
	/// Polygon graphic object
	/// </summary>
	public class DrawPolygon 
        : DrawLine
	{
        public List<MetaVertex> pointArray;         // list of points
        
        private const string entryLength = "Length";
        private const string entryPoint = "Point";


		public DrawPolygon()
		{
            pointArray = new List<MetaVertex>();
            Initialize();
		}

        public DrawPolygon(float x1, float y1, float x2, float y2)
        {
            pointArray = new List<MetaVertex>();
            pointArray.Add(new MetaVertex(x1, y1));
            pointArray.Add(new MetaVertex(x2, y2));

            Initialize();
        }

		public DrawPolygon(DrawPolygon obj) : base(obj)
		{
			pointArray = new List<MetaVertex>(obj.pointArray);
			this.Initialize();
		}

        public PointF[] ToPointFArray()
        {
            PointF[] pfa = new PointF[pointArray.Count];
            for (int i = 0; i < pointArray.Count; i++)
            {
                pfa[i] = pointArray[i].pt;
            }
            return pfa;
        }
        public Point[] ToPointArray()
        {
            Point[] pa = new Point[pointArray.Count];
            for (int i = 0; i < pointArray.Count; i++)
            {
                pa[i] = Point.Round(pointArray[i].pt);
            }
            return pa;
        }

        public List<MetaVertex> FromPointFArray(PointF[] pfa)
        {
            List<MetaVertex> lmv = new List<MetaVertex>();
            for (int i = 0; i < pfa.Length; i++)
            {
                lmv.Add(new MetaVertex(pfa[i]));
            }
            return lmv;
        }

        public override string GetDrawingType()
        {
            return "Polygon";
        }

        public override void Draw(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Pen pen = new Pen(Color, PenWidth);
			Brush brush = new SolidBrush(BrushColor);

			if (pointArray.Count > 0)
				pointArray.Add(pointArray[0]);

			PointF[] pts = ToPointFArray();

            if (Container.MetroSys != null)
            {
                Container.MetroSys.ToPixel(ref pts);
            }

			g.FillPolygon(brush, pts);
			g.DrawPolygon(pen, pts);

			if (pointArray.Count > 1)
				pointArray.RemoveAt(pointArray.Count-1);

            #region Draw Auto-vertice
            Pen penAutoVertex = new Pen(Color.Red, 3);
            for (int i = 0; i < pointArray.Count; i++)
            {
                MetaVertex m = pointArray[i];
                if (m.IsAutoVertex)
                {
                    g.DrawEllipse(penAutoVertex, pts[i].X - 10, pts[i].Y - 10, 21, 21);
                    g.DrawEllipse(penAutoVertex, pts[i].X - 1, pts[i].Y - 1, 3, 3);
                }
            }
            penAutoVertex.Dispose();
            #endregion

            pen.Dispose();
			brush.Dispose();
        }

        public override GraphicsPath CreateGraphicsPath()
        {
            GraphicsPath path = new GraphicsPath();
            PointF[] pts = this.ToPointFArray();
            path.AddPolygon(pts);
            return path;
        }

		public override void Draw(SIA.Common.Mask.IMask mask)
		{
			if (pointArray.Count > 0)
				pointArray.Add(pointArray[0]);

            Point[] pts = this.ToPointArray();
			mask.FillPolygon(pts);
		}

        public void AddPoint(PointF point)
        {
            pointArray.Add(new MetaVertex(point));
        }

		public int Count
		{
			get
			{
				return pointArray.Count;
			}
		}

		public PointF this[int index]
		{
			get
			{
				return (PointF)(pointArray[index].pt);
			}			
		}

        public override int HandleCount
        {
            get
            {
                return pointArray.Count;
            }
        }

		public override RectangleF BoundingRectangle
		{
			get
			{
				float min_x=float.MaxValue, min_y=float.MaxValue, max_x=float.MinValue, max_y=float.MinValue;
				foreach(MetaVertex mv in pointArray)
				{
					min_x = Math.Min(min_x, mv.pt.X);
                    min_y = Math.Min(min_y, mv.pt.Y);
                    max_x = Math.Max(max_x, mv.pt.X);
                    max_y = Math.Max(max_y, mv.pt.Y);
				}
				//return new Rectangle(min_x, min_y, max_x, max_y);
                return RectangleF.FromLTRB(min_x, min_y, max_x, max_y);
			}
		}

        /// <summary>
        /// Get handle point by 1-based number
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public override PointF GetHandle(int handleNumber)
        {
            if ( handleNumber < 1 )
                handleNumber = 1;

            if ( handleNumber > pointArray.Count )
                handleNumber = pointArray.Count;

            if (Container.MetroSys != null)
                return Container.MetroSys.ToPixel(pointArray[handleNumber - 1].pt);
            return pointArray[handleNumber - 1].pt;
        }

        public override Cursor GetHandleCursor(int handleNumber)
        {
            return Resources.ApplicationCursors.DrawPolygon;
        }

        public override void MoveHandleTo(PointF point, int handleNumber)
        {
            if ( handleNumber < 1 )
                handleNumber = 1;

            if ( handleNumber > pointArray.Count)
                handleNumber = pointArray.Count;

            pointArray[handleNumber-1].pt = point;

            Invalidate();
        }

        public override void Move(float deltaX, float deltaY)
        {
            int n = pointArray.Count;
            PointF point;

            for ( int i = 0; i < n; i++ )
            {
                point = new PointF( pointArray[i].pt.X + deltaX, pointArray[i].pt.Y + deltaY);
                pointArray[i].pt = point;
            }

            Invalidate();
        }

		public override bool PointInObject(PointF point)
		{
			if (pointArray.Count < 3) return false;

			PointF[] pts = this.ToPointFArray();
			GraphicsPath path = new GraphicsPath();
			path.AddPolygon(pts);
			bool bRes = path.IsVisible(point);
			path.Dispose();
			return bRes;
		}

        public override void AddToGraphicsPath(GraphicsPath path)
        {
            PointF[] pts = this.ToPointFArray();
            path.AddPolygon(pts);
        }

		public override void Transform(Matrix transform)
		{
			if (pointArray.Count <= 0) return;
            PointF[] pts = this.ToPointFArray();
			transform.TransformPoints(pts);
			pointArray.Clear();
            pointArray.AddRange(FromPointFArray(pts));
		}

		public override DrawObject Copy()
		{
			DrawPolygon cloneObject = new DrawPolygon();
			//cloneObject.pointArray = new List<MetaVertex>(this.pointArray);
            cloneObject.pointArray = new List<MetaVertex>();
            foreach (MetaVertex m in this.pointArray)
                cloneObject.pointArray.Add(new MetaVertex(m));

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
                entryLength, orderNumber),
                pointArray.Count);

            int i = 0;
            foreach ( MetaVertex m in pointArray )
            {
                m.SaveToStream(info, orderNumber, i++);
            }

            base.SaveToStream (info, orderNumber);  // ??
        }

        public override void LoadFromStream(System.Runtime.Serialization.SerializationInfo info, int orderNumber)
        {
            int n = info.GetInt32(
                String.Format(CultureInfo.InvariantCulture,
                "{0}{1}",
                entryLength, orderNumber));

            for ( int i = 0; i < n; i++ )
            {
                MetaVertex m = new MetaVertex();
                m.LoadFromStream(info, orderNumber, i);

                pointArray.Add(m);
            }

            base.LoadFromStream (info, orderNumber);
        }

        public override void Serialize(System.IO.BinaryWriter writer)
        {
            base.Serialize(writer);

            writer.Write(pointArray.Count);

            int i = 0;
            foreach (MetaVertex m in pointArray)
            {
                m.Serialize(writer);
            }
        }

        public override void Deserialize(System.IO.BinaryReader reader)
        {
            base.Deserialize(reader);

            int n = reader.ReadInt32();

            for (int i = 0; i < n; i++)
            {
                MetaVertex m = MetaVertex.Deserialize(reader);
                pointArray.Add(m);
            }
        }

        /// <summary>
        /// Create graphic object used for hit test
        /// </summary>
        protected override void CreateObjects()
        {
            if ( AreaPath != null )
                return;

            // Create closed path which contains all polygon vertexes
            AreaPath = new GraphicsPath();

            float x1 = 0, y1 = 0;     // previous pofloat
            float x2, y2;             // current pofloat

            IEnumerator enumerator = pointArray.GetEnumerator();

            if ( enumerator.MoveNext() )
            {
                x1 = ((MetaVertex)enumerator.Current).pt.X;
                y1 = ((MetaVertex)enumerator.Current).pt.Y;
            }

            while ( enumerator.MoveNext() )
            {
                x2 = ((MetaVertex)enumerator.Current).pt.X;
                y2 = ((MetaVertex)enumerator.Current).pt.Y;

                AreaPath.AddLine(x1, y1, x2, y2);

                x1 = x2;
                y1 = y2;
            }

            AreaPath.CloseFigure();

            // Create region from the path
            AreaRegion = new Region(AreaPath);
        }
        
	}

    public class MetaVertex
    {
        public bool IsAutoVertex = false;
        public PointF pt = new PointF();
        public PointF ptSample = new PointF();
        public Size sizeSample = new Size(0, 0);
        public ushort[] SampleData = null;

        public MetaVertex() { }
        public MetaVertex(PointF _pt)
        {
            pt.X = _pt.X;
            pt.Y = _pt.Y;
        }
        public MetaVertex(float x, float y)
        {
            pt.X = x;
            pt.Y = y;
        }
        public MetaVertex(MetaVertex m)
        {
            IsAutoVertex = m.IsAutoVertex;
            pt.X = m.pt.X; pt.Y = m.pt.Y;
            ptSample.X = m.ptSample.X; ptSample.Y = m.ptSample.Y;
            sizeSample.Width = m.sizeSample.Width; sizeSample.Height = sizeSample.Height;
            if (m.SampleData != null)
                SampleData = (ushort[])m.SampleData.Clone();
        }

        public void SaveToStream(System.Runtime.Serialization.SerializationInfo info, int orderNumber, int vertexNumber)
        {
            info.AddValue(String.Format(CultureInfo.InvariantCulture, "{0}{1}-{2}",
                "AutoVertex", orderNumber, vertexNumber), IsAutoVertex);

            info.AddValue(String.Format(CultureInfo.InvariantCulture, "{0}{1}-{2}",
                "Point", orderNumber, vertexNumber), pt);

            info.AddValue(String.Format(CultureInfo.InvariantCulture, "{0}{1}-{2}",
                    "PointSample", orderNumber, vertexNumber), ptSample);

            if (IsAutoVertex)
            {
                info.AddValue(String.Format(CultureInfo.InvariantCulture, "{0}{1}-{2}",
                        "SampleSize", orderNumber, vertexNumber), sizeSample);

                if (SampleData != null)
                    info.AddValue(String.Format(CultureInfo.InvariantCulture, "{0}{1}-{2}",
                        "SampleData", orderNumber, vertexNumber), SampleData);
            }
        }

        public void LoadFromStream(System.Runtime.Serialization.SerializationInfo info, int orderNumber, int vertexNumber)
        {
            IsAutoVertex = info.GetBoolean(String.Format(CultureInfo.InvariantCulture, "{0}{1}-{2}",
                "AutoVertex", orderNumber, vertexNumber));

            pt = (PointF)info.GetValue(String.Format(CultureInfo.InvariantCulture, "{0}{1}-{2}",
                "Point", orderNumber, vertexNumber), typeof(PointF));

            ptSample = (PointF)info.GetValue(String.Format(CultureInfo.InvariantCulture, "{0}{1}-{2}",
                    "PointSample", orderNumber, vertexNumber), typeof(PointF));

            if (IsAutoVertex)
            {
                sizeSample = (Size)info.GetValue(String.Format(CultureInfo.InvariantCulture, "{0}{1}-{2}",
                    "SampleSize", orderNumber, vertexNumber), typeof(Size));

                try
                {
                    SampleData = (ushort[])info.GetValue(String.Format(CultureInfo.InvariantCulture, "{0}{1}-{2}",
                        "SampleData", orderNumber, vertexNumber), typeof(ushort[]));
                }
                catch
                { }
            }
        }

        public void Serialize(System.IO.BinaryWriter writer)
        {
            if (SampleData == null || sizeSample.Width == 0 || sizeSample.Height == 0)
                IsAutoVertex = false;

            writer.Write(IsAutoVertex);
            writer.Write(pt.X);
            writer.Write(pt.Y);

            writer.Write(ptSample.X);
            writer.Write(ptSample.Y);
            if (IsAutoVertex)
            {
                writer.Write(sizeSample.Width);
                writer.Write(sizeSample.Height);
                
                for (int i = 0; i < SampleData.Length; i++)
                    writer.Write(SampleData[i]);
            }
        }

        public static MetaVertex Deserialize(System.IO.BinaryReader reader)
        {
            bool isAutoVertex = reader.ReadBoolean();
            PointF point = new PointF(reader.ReadSingle(), reader.ReadSingle());
            MetaVertex m = new MetaVertex(point);
            m.IsAutoVertex = isAutoVertex;

            m.ptSample = new PointF(reader.ReadSingle(), reader.ReadSingle());

            if (isAutoVertex)
            {
                m.sizeSample = new Size(reader.ReadInt32(), reader.ReadInt32());
                int len = m.sizeSample.Width * m.sizeSample.Height;
                if (len > 0)
                {
                    m.SampleData = new ushort[len];
                    for (int i = 0; i < len; i++)
                        m.SampleData[i] = reader.ReadUInt16();
#if _test_
                    Bitmap bmp = new Bitmap(m.sizeSample.Width, m.sizeSample.Height);
                    for (int x = 0; x < m.sizeSample.Width; x++)
                        for (int y = 0; y < m.sizeSample.Height; y++)
                        {
                            int val = (int)m.SampleData[y*m.sizeSample.Width + x];
                            bmp.SetPixel(x, y, Color.FromArgb(val, val, val));
                        }
                    bmp.Save("d:\\test.bmp");
#endif
                }
            }
            return m;
        }
    }
}
