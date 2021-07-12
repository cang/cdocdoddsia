using System;
using System.Drawing;
using System.Collections;

using SIA.IPEngine;
using SIA.Common.Analysis;

namespace SIA.SystemLayer.PatternRecogition
{
    /// <summary>
    /// This class is nolonger used
    /// </summary>
	[Obsolete]
	public class Clustering
	{
		const int Empty = 0;
		const int Type1 = 1;
		const int Type2 = 2;
		const int Inside = 3;
		const int Outside = 4;
		public Clustering()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static ArrayList TraceObjects(ArrayList list1, ArrayList list2, int width, int height)
		{
			if (list1 == null || list1.Count <= 0)
				return null;

			ArrayList objects = new ArrayList();
			
			RectangleF rect = (RectangleF)list1[0];
			int szX = (int)Math.Abs(rect.Right - rect.Left);
			int szY = (int)Math.Abs(rect.Bottom - rect.Top);
			int sx = width/szX;
			int sy = height/szY;
			
			int [,]mask = new int[sx, sy];

			for (int i=0; i<sx; i++)
			{
				for (int j=0; j<sy; j++)
				{
					mask[i, j] = Empty;
				}
			}

			int nums = list1.Count;
			for (int i=0; i<nums; i++)
			{
				rect = (RectangleF)list1[i];
				int x = (int)(Math.Min(rect.Left, rect.Right)/szX);
				int y = (int)(Math.Min(rect.Top, rect.Bottom)/szY);
				mask[x, y] = Type1;
			}
			
			if (list2 != null)
			{
				nums = list2.Count;
				for (int i=0; i<nums; i++)
				{
					rect = (RectangleF)list2[i];
					int x = (int)(Math.Min(rect.Left, rect.Right)/szX);
					int y = (int)(Math.Min(rect.Top, rect.Bottom)/szY);
					mask[x, y] = Type2;
				}
			}

			nums = list1.Count;
			for (int i=0; i<nums; i++)
			{
				rect = (RectangleF)list1[i];
				int x = (int)(Math.Min(rect.Left, rect.Right)/szX);
				int y = (int)(Math.Min(rect.Top, rect.Bottom)/szY);
				
				DetectedObject obj = TraceObjects(mask, x, y, szX, szY);

				if (obj != null)
				{
					objects.Add(obj);
				}
			}

			return objects;
		}

		public static DetectedObject TraceObjects(int [,]mask, int x, int y, float szX, float szY)
		{
			// 3: is inside
			// 4: is outside
			if (mask[x, y] == Inside || mask[x, y] == Outside)
				return null;

			ArrayList contour = new ArrayList();

			float halfX = szX/2;
			float halfY = szY/2;

			Point point = Point.Empty;
			contour.Add(new Point(x, y));

			mask[x, y] = Inside;

			Queue stack = new Queue();
						
			int i = 0;
			int j = 0;

			int sx = mask.GetLength(0);
			int sy = mask.GetLength(1);

			//start the loop
			for (j=y-1; j<=y+1; j++)
			{
				for (i=x-1; i<=x+1; i++)
				{				
					if (i < 0 || i >= sx)
						continue;
					if (j < 0 || j >= sy)
						continue;

					if (mask[i, j] == Inside || mask[i, j] == Outside)
						continue;
					
					stack.Enqueue(new Point(i, j));
				}
			}

			//call next item on queue
			while (stack.Count > 0)
			{
				point = (Point)stack.Dequeue();
				x = point.X;
				y = point.Y;

				if (mask[x, y] == Inside || mask[x, y] == Outside)
					continue;
		
				if (mask[x, y] != Empty)
				{					
					contour.Add(new Point(x, y));

					mask[x, y] = Inside;

					// push
					for (j=y-1; j<=y+1; j++)
					{
						for (i=x-1; i<=x+1; i++)
						{				
							if (i < 0 || i >= sx)
								continue;
							if (j < 0 || j >= sy)
								continue;

							if (mask[i, j] == Inside || mask[i, j] == Outside)
								continue;
					
							stack.Enqueue(new Point(i, j));
						}
					}
				}
				else
				{
					mask[x, y] = Outside;
				}		
			}
			
			int nPoints = contour.Count;
			
			DetectedObject obj = null;

			if (nPoints > 0)
			{
				float [,]vertices = new float[nPoints, 2];
				for (i=0; i<nPoints; i++)
				{
					vertices[i, 0] = ((Point)contour[i]).X;
					vertices[i, 1] = ((Point)contour[i]).Y;
				}

				float max_distance = 3.0f;
				float dwidth = 1f;
				PointF []out_contour = null;
				SIA.SystemFrameworks.MatrixUtils.GetContour(vertices, max_distance, out out_contour, dwidth);
				nPoints = out_contour.Length;
				if (nPoints > 0)
				{
					obj = new DetectedObject();
					float []pX = new float[nPoints];
					float []pY = new float[nPoints];						
					pX[0] = out_contour[0].X*szX+halfX;
					pY[0] = out_contour[0].Y*szY+halfY;				
					float lelf = pX[0];
					float right = pX[0];
					float top = pY[0];
					float bottom = pY[0];
					for (i=1; i<nPoints; i++)
					{						
						pX[i] = out_contour[i].X*szX+halfX;
						pY[i] = out_contour[i].Y*szY+halfY;
					
						if (lelf > pX[i])
							lelf = pX[i];
						if (right < pX[i])
							right = pX[i];
						if (top > pY[i])
							top = pY[i];
						if (bottom < pY[i])
							bottom = pY[i];
					}
					
					bool []pHole = new bool[1];
					pHole[0] = false;
					int []pContours = new int[1];
					pContours[0] = nPoints-1;
				
					obj.PolygonBoundary = new PolygonExData(pX, pY, nPoints, pHole, pContours, 1);
					obj.RectBound = new RectangleF(lelf, top, right-lelf, bottom-top);	
				}						
			}

			if (contour != null)
				contour.Clear();
			
			return obj;
		}
	}
}
