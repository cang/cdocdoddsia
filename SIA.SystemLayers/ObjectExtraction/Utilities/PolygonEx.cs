using System;
using System.Drawing;
using System.Collections;
using System.Diagnostics;

using SIA.IPEngine;
using SIA.Common.Analysis;

namespace SIA.SystemLayer.ObjectExtraction.Utilities
{
	/// <summary>
	/// The PolygonEx class provides functionality for processing the detected objects properties
    /// </summary>
	public class PolygonEx : IDisposable
	{
		#region field members
		
		private int _nPoints = 0;
		private float[] _pX = null;
		private float[] _pY = null;
		private int _nContours = 0;
		private int[] _pIndexEndPointContours = null; //index of the end point of contour
		private int[] _pIndexPoints = null;
		private int[] _pIndexLefts = null;
		private int[] _pIndexRights = null;

		//helpers
		private float[] _pSlopes = null;
		private float[] xIntersect = null;
		private float[] xEdge = null;
		private float[] xIntersectTmp = null;
		private int nEdgeCount = 0;
		private int[] pEdgeCount = null;
		//private int[] pIndexEx = null;

		private float[] xAreaDefects = null;
		private float[] xLineDefects = null;

		//rectangle bound ex
		private float centerX = 0;
		private float centerY = 0;
		private float radius = 0;
		private float angle = 0;
		private float markAngle = 0;		
		private float dXFlat = 0;
		private float dYFlat = 0;
		private float cFlat = 0;
		private bool hasFlat = false;
		private bool bInside = false;
		private float sqrRadius = 0;		

		private float cosAngle = 0;
		private float sinAngle = 0;
		private float floatX = 0;
		private float floatY = 0;
		private PointF point1 = new PointF(0, 0);
		private PointF point2 = new PointF(0, 0);
		
		#endregion

		#region Constructors and Destructors

		public PolygonEx()
		{
			//
			// TODO: Add constructor logic here
			//
			_nPoints = 0;
			_pX = null;
			_pY = null;
			_nContours = 0;
			_pIndexEndPointContours = null;
			
			_pIndexPoints = null;
			_pIndexLefts = null;
			_pIndexRights = null;
			_pSlopes = null;

			xIntersect = null;
			xEdge = null;
			xIntersectTmp = null;
			//pIndexEx = null;
		}

		public PolygonEx(int maxNumContours, int maxNumPoints, bool bExtract)
		{
			_nPoints = maxNumPoints;
			_nContours = maxNumContours;
			
			_pX = new float[_nPoints];
			_pY = new float[_nPoints];
			_pIndexEndPointContours = new int[_nContours];

			_pIndexPoints = new int[_nPoints];
			_pIndexLefts = new int[_nPoints];
			_pIndexRights = new int[_nPoints];
			_pSlopes = new float[_nPoints];
				
			xIntersect = new float[2*_nPoints];
			xEdge = new float[2*_nPoints];
			xIntersectTmp = new float[2*_nPoints];
			pEdgeCount = new int[_nPoints];	
			
			if(bExtract)
			{
				//pIndexEx = new int[2*_nPoints];
				xAreaDefects = new float[4*_nPoints];
				xLineDefects = new float[2*_nPoints];
			}
		}


		public PolygonEx(PolygonExData polyData)
		{
			this._nPoints = polyData.nPoints;
			this._nContours = polyData.nContours;
			
			this._pX = new float[this._nPoints];
			this._pY = new float[this._nPoints];
			this._pIndexEndPointContours = new int[this._nContours];
				
			Array.Copy(polyData.pX, this._pX, this._nPoints);
			Array.Copy(polyData.pY, this._pY, this._nPoints);
			Array.Copy(polyData.pIndexEndPointContours, this._pIndexEndPointContours, this._nContours);				
			
			this._pIndexPoints = null;
			this._pIndexLefts = null;
			this._pIndexRights = null;
			this._pSlopes = null;
		}

		public PolygonEx(float[] pX, float[] pY, int nPoints, int[] pContours, int nContours)
		{
			_nPoints = nPoints;
			_pX = pX;
			_pY = pY;
			_pIndexEndPointContours = pContours;
			_nContours = nContours;

			_pIndexPoints = null;
			_pIndexLefts = null;
			_pIndexRights = null;
			_pSlopes = null;
		}

		public PolygonEx(PolygonEx _poly)
		{
			this._nPoints = _poly._nPoints;
			this._nContours = _poly._nContours;
			this._pX = new float[this._nPoints];
			this._pY = new float[this._nPoints];
			this._pIndexEndPointContours = new int[this._nContours];
			for(int i=0; i<this._nContours; i++)
				this._pIndexEndPointContours[i] = _poly._pIndexEndPointContours[i];
			int start = 0, end = start;
			for(int i=0; i<this._nContours; i++)
			{
				end = this._pIndexEndPointContours[i];
				for(int j=start; j<=end; j++)
				{
					this._pX[j] = _poly._pX[j];
					this._pY[j] = _poly._pY[j];
				}
				start = end+1;
			}
			_pIndexPoints = null;
			_pSlopes = null;
		}

		public void Dispose()
		{
			if(_pX != null)
				_pX = null;

			if(_pY != null)
				_pY = null;

			if(_pIndexEndPointContours != null)
				_pIndexEndPointContours = null;

			if(_pIndexPoints != null)
				_pIndexPoints = null;

			if(_pIndexLefts != null)
				_pIndexLefts = null;

			if(_pIndexRights != null)
				_pIndexRights = null;

			if(_pSlopes != null)
				_pSlopes = null;			
		}

		public void UpdatePolygonEx(PolygonExData polyData)
		{
			this._nPoints = polyData.nPoints;
			this._nContours = polyData.nContours;
			if(_nContours > 0)
			{					 
				Array.Copy(polyData.pX, this._pX, this._nPoints);
				Array.Copy(polyData.pY, this._pY, this._nPoints);
				Array.Copy(polyData.pIndexEndPointContours, this._pIndexEndPointContours, this._nContours);				
			}		
		}

		#endregion

		#region properties
		public float CenterX
		{
			set { centerX = value; }
		}

		public float CenterY
		{
			set { centerY = value; }
		}

		public float Radius
		{
			set { 
				radius = value; 
				sqrRadius = radius*radius;
			}
		}

		public float Angle
		{
			set { 
				angle = value;
				cosAngle = (float)Math.Cos(angle);
				sinAngle = (float)Math.Sin(angle);	
			}
		}

		public float MarkAngle
		{
			set { markAngle = value; }
		}

		public bool HasFlat
		{
			set { hasFlat = value; }
		}

		public bool Inside
		{
			set { bInside = value; }
		}

		public float DX
		{
			set { dXFlat = value; }
		}

		public float DY
		{
			set { dYFlat = value; }
		}

		public float C
		{
			set { cFlat = value; }
		}		

		public PointF Point1
		{
			set { point1 = value; }
		}

		public PointF Point2
		{
			set { point2 = value; }
		}

		#endregion properties

		#region Rotate
		
		public void Rotate(ref float x, ref float y)
		{			
			floatX = x - centerX;
			floatY = centerY - y;
			
			x = floatX*cosAngle - floatY*sinAngle + centerX;
			y = centerY - (floatY*cosAngle + floatX*sinAngle);
		}

		public void Rotate()
		{			
			try
			{				
				for(int i=0; i<_nPoints; i++)
				{
					Rotate(ref _pX[i], ref _pY[i]);
				}								
			}
			catch (System.Exception exp)
			{
				Trace.WriteLine(exp);
			}
		}

		#endregion Rotate

		#region methods processing
		
		private void QuickSort(int start, int end)
		{
			//using binary-sort
			if(start > end) return;
			
			int i = start, j = end, tmp;
			float y = _pY[_pIndexPoints[(i+j)/2]];

			do
			{
				while(_pY[_pIndexPoints[i]] < y)
					i++;
				while(_pY[_pIndexPoints[j]] > y)
					j--;
				if(i<=j)
				{
					tmp = _pIndexPoints[i];
					_pIndexPoints[i] = _pIndexPoints[j];
					_pIndexPoints[j] = tmp;
					
					i++;
					j--;
				}
			}
			while (i < j);
		
			if(start < j)
				QuickSort(start, j);
			if(end > i)
				QuickSort(i, end);
		}

		private void QuickSort(ref float[] _data, int start, int end)
		{
			//using binary-sort
			if(start > end) return;
			
			int i = start, j = end;
			float x = _data[(i+j)/2], tmp;

			do
			{
				while(_data[i] < x)
					i++;
				while(_data[j] > x)
					j--;
				if(i<=j)
				{
					tmp = _data[i];
					_data[i] = _data[j];
					_data[j] = tmp;
					
					i++;
					j--;
				}
			}
			while (i < j);
		
			if(start < j)
				QuickSort(ref _data, start, j);
			if(end > i)
				QuickSort(ref _data, i, end);
		}

		private void SortEdge(ref float[] _data, int start, int end)
		{
			//using binary-sort
			if(start > end) return;
			
			int i = start, j = end;
			float x = _data[2*((i+j)/2)], tmp;

			do
			{
				while(_data[2*i] < x)
					i++;
				while(_data[2*j] > x)
					j--;
				if(i<=j)
				{
					tmp = _data[2*i];
					_data[2*i] = _data[2*j];
					_data[2*j] = tmp;

					tmp = _data[2*i+1];
					_data[2*i+1] = _data[2*j+1];
					_data[2*j+1] = tmp;
					
					i++;
					j--;
				}
			}
			while (i < j);
		
			if(start < j)
				SortEdge(ref _data, start, j);
			if(end > i)
				SortEdge(ref _data, i, end);
		}

		//duplicate with SortEdge
		private void Sort_xIntersect(ref float[] _data, int start, int end)
		{
			//using binary-sort
			if(start > end) return;
			
			int i = start, j = end;
			float x = _data[2*((i+j)/2)], tmp;

			do
			{
			while(_data[2*i] < x)
				i++;
			while(_data[2*j] > x)
				j--;
				if(i<=j)
				{
					tmp = _data[2*i];
					_data[2*i] = _data[2*j];
					_data[2*j] = tmp;

					tmp = _data[2*i+1];
					_data[2*i+1] = _data[2*j+1];
					_data[2*j+1] = tmp;
					
					i++;
					j--;
				}
			}
			while (i < j);
		
			if(start < j)
				SortEdge(ref _data, start, j);
			if(end > i)
				SortEdge(ref _data, i, end);
		}

		private void MergeEdge(ref float[] _data, ref int nEdges)
		{
			for(int i=nEdges-2; i>0; i-=2)
			{
				if(_data[i] <= _data[i-1])
				{					
					if(_data[i-1] < _data[i+1])
						_data[i-1] = _data[i+1];

					nEdges -= 2;

					for(int j=i; j<=nEdges-1; j++)
					{
						_data[j] = _data[j+2];						
					}					
				}
			}
		}

		//duplicate with MergeEdge
		private void Merge_xIntersect(ref float[] _data, ref int n_xIntersect)
		{
			for(int i=n_xIntersect-2; i>0; i-=2)
			{
				if(_data[i] <= _data[i-1])
				{					
					if(_data[i-1] < _data[i+1])
						_data[i-1] = _data[i+1];

					n_xIntersect -= 2;

					for(int j=i; j<=n_xIntersect-1; j++)
					{
						_data[j] = _data[j+2];						
					}					
				}
			}
		}
		
		private int QuickSearch(float[] _data, float x, int start, int end)
		{
			int mid = 0;
			while(start <= end)
			{
				mid = (start+end)/2;
				if(_data[mid] == x)
				{
					while(mid+1 <= end && _data[mid+1] == x)
					{
						mid++;
					}
					return mid;
				}

				if(_data[mid] > x)
					end = mid-1;
				else
					start = mid+1;
			}
			return -1;
		}

		public void EnhanceIndexs()
		{
			if(_nContours <= 0 || _nPoints <= 0)
				return;

			//using binary-sort
			try
			{
				if(_pIndexPoints == null)
					_pIndexPoints = new int[_nPoints];

				for(int i=0; i<_nPoints; i++)
					_pIndexPoints[i] = i;

				QuickSort(0, _nPoints-1);
			}
			catch (System.Exception exp)
			{
				throw exp;
			}
		}

		public void Internal_Initialize()
		{
			if(_nContours <= 0 || _nPoints <= 0)
				return;
			try
			{	
				if(_pSlopes == null)
					_pSlopes = new float[_nPoints];

				float x1, y1, x2, y2;
				int start = 0, end = start;
				for(int i=0; i<_nContours; i++)
				{
					end = _pIndexEndPointContours[i];
					y1 = _pY[start];
					x1 = _pX[start];
					for(int j=start+1; j<=end; j++)
					{
						y2 = _pY[j];
						x2 = _pX[j];
						if(y1 == y2)
							_pSlopes[j-1] = 0.0f;
						else
							_pSlopes[j-1] = (x2 - x1)/(y2 - y1);
						x1 = x2; y1 = y2;
					}

					y2 = _pY[start]; x2 = _pX[start];
					y1 = _pY[end]; x1 = _pX[end];
					if(y1 == y2)
						_pSlopes[end] = 0.0f;
					else
						_pSlopes[end] = (x2 - x1)/(y2 - y1);

					start = end + 1;
				}

				if(_pIndexPoints == null)
				{
					_pIndexPoints = new int[_nPoints];
					_pIndexLefts = new int[_nPoints];
					_pIndexRights = new int[_nPoints];

					xIntersect = new float[2*_nPoints];
					xEdge = new float[2*_nPoints];
					xIntersectTmp = new float[2*_nPoints];
					
					pEdgeCount = new int[_nPoints];
				}

				start = 0; end = start;
				for(int i=0; i<_nContours; i++)
				{
					end = _pIndexEndPointContours[i];
					
					_pIndexPoints[start] = start;
					
					if(_pY[end] < _pY[start])
						_pIndexLefts[start] = -1;
					else
						_pIndexLefts[start] = end;
					if(_pY[start+1] < _pY[start])
						_pIndexRights[start] = -1;
					else 
						_pIndexRights[start] = start+1;
					
					for(int j=start+1; j<end; j++)
					{
						_pIndexPoints[j] = j;
						if(_pY[j-1] < _pY[j])
							_pIndexLefts[j] = -1;
						else
							_pIndexLefts[j] = j-1;
						if(_pY[j+1] < _pY[j])
							_pIndexRights[j] = -1;
						else
							_pIndexRights[j] = j+1;
					}

					_pIndexPoints[end] = end;

					if(_pY[end-1] < _pY[end])
						_pIndexLefts[end] = -1;
					else
						_pIndexLefts[end] = end-1;
					if(_pY[start] < _pY[end])
						_pIndexRights[end] = -1;
					else 
						_pIndexRights[end] = start;

					start = end+1;
				}
			}
			catch (System.Exception exp)
			{
				throw exp;
			}
		}

		public float Slope(int i1, int i2)
		{
			try
			{
				if (Math.Abs(i1 - i2) != 1)
				{
					return _pSlopes[Math.Max(i1, i2)];
				}

				return _pSlopes[Math.Min(i1, i2)];
			}
			catch (System.IndexOutOfRangeException exp)
			{
				throw exp;
			}
		}

		private bool CheckEdge(ref float[] _data, int lenght, float x1, float x2)
		{
			for(int i=0; i<lenght; i += 2)
				if( (_data[i] <= x1 && _data[i+1] >= x2) )
					return true;
			return false;
		}
		
		
		public void Intialize()
		{
			Internal_Initialize();

			EnhanceIndexs();
		}

		private bool CheckInWaferBoundary(ref float x, ref float y)
		{
			if(bInside)
				return true;
			
			float xx = x-centerX;
			float yy = y-centerY;

			if(xx*xx + yy*yy > sqrRadius)
				return false;
						
			if(hasFlat && cFlat*(-dXFlat*yy-dYFlat*xx+cFlat) < 0)
				return false;
			
			return true;
		}

		private bool CheckIn(float x, float y)
		{
			float xx = x-centerX;
			float yy = y-centerY;

			if(xx*xx + yy*yy > sqrRadius)
				return false;
						
			if(hasFlat && cFlat*(-dXFlat*yy-dYFlat*xx+cFlat) < 0)
				return false;

			return true;
		}

		private PointF Intersect(float x1, float y1, float x2, float y2)
		{
			// (x2, y2) inside wafer
			PointF intersect = new PointF(x2, y2);
			
			x1 = x1 - centerX;
			y1 = centerY - y1;
			x2 = x2 - centerX;
			y2 = centerY - y2;
			
			if(hasFlat)
			{
				// check intersect with flat
				if ((y1*dXFlat - x1*dYFlat + cFlat)*(y2*dXFlat - x2*dYFlat + cFlat) <= 0)
				{
					float dx, dy, c;
					dx = x2-x1;
					dy = y2-y1;
					c = y2*x1-y1*x2;
					if ((point1.Y*dx - point1.X*dy + c)*(point2.Y*dx - point2.X*dy + c) <= 0)
					{
						if(x1 == x2)
						{
							if(point1.X == point2.X)
							{
								intersect.X = point1.X;
								intersect.Y = y2;
							}
							else
							{
								intersect.X = x2;
								intersect.Y = intersect.X*dy/dx - c;
							}
						}
						else
						{
							if(point1.X == point2.X)
							{
								intersect.X = point1.X;
								intersect.Y = intersect.X*dy/dx - c;
							}
							else
							{
								if(dy/dx == dYFlat/dXFlat)
								{
									intersect.X = x2;
									intersect.Y = y2;
								}
								else
								{
									intersect.X = (c-cFlat)/(dy/dx - dYFlat/dXFlat);
									intersect.Y = intersect.X*dy/dx - c;
								}
							}							
						}

						intersect.X += centerX;
						if(y1 < 0)
							intersect.Y = intersect.Y + centerY;
						else
							intersect.Y = centerY - intersect.Y;
							
						return intersect;
					}
				}																					  								
			}
			
			if (x1 == x2)
			{
				intersect.X = x1 + centerX;
				intersect.Y = (float)Math.Sqrt(radius*radius - x1*x1);				
			}
			else
			{
				float a = (y2-y1)/(x2-x1);
				float b = (x2*y1-x1*y2)/(x2-x1);
				float sqrtDelta = (float)Math.Sqrt(4*a*a*b*b - 4*(a*a+1)*(b*b-radius*radius));
				float xTmp1 = (-b + sqrtDelta)/(2*a);
				float xTmp2 = (-b - sqrtDelta)/(2*a);
				if ((xTmp1-x1)*(xTmp2-x2) <= 0)
				{
					intersect.X = xTmp1 + centerX;
					intersect.Y = (float)Math.Sqrt(radius*radius - xTmp1*xTmp1);					
				}
				else
				{
					intersect.X = xTmp2 + centerX;
					intersect.Y = (float)Math.Sqrt(radius*radius - xTmp2*xTmp2);					
				}
			}

			if(y1 < 0)
				intersect.Y = intersect.Y + centerY;
			else
				intersect.Y = centerY - intersect.Y;

			return intersect;
		}

		#endregion
		
		#region current version		
		public void GetObjectExtData(GreyDataImage imgData, PointExData extPoints, ref float numPixels, ref float totalIntensity, ref PointF gravity)
		{
			numPixels = 0.0f;
			totalIntensity = 0.0f;
			gravity = PointF.Empty;

			try
			{
				if (extPoints != null && extPoints.nContours > 0)
				{
					int nPoint = extPoints.nPoints;
					int nContour = extPoints.nContours;
					float[] pX = extPoints.pX;
					float[] pY = extPoints.pY;
					int[] pContour = extPoints.pIndexEndPointContours;

					int start = 0, end = start, i = 0;
					for (; i < nContour; i++)
					{
						end = pContour[i];
						if (end == start)
						{
							numPixels++;
							ushort value = imgData.getPixel((int)pX[start], (int)pY[start]);
							totalIntensity += value;
							gravity.X += value * pX[start];
							gravity.Y += value * pY[start];
						}
						else if (end == start + 1)
						{
							if (pY[start] == pY[end])
							{
								int x1 = (int)pX[start], x2 = (int)pX[end];
								int y = (int)pY[start];
								if (x1 > x2)
								{
									x1 = (int)pX[end]; x2 = (int)pX[start];
								}
								for (; x1 <= x2; x1++)
								{
									numPixels++;
									ushort value = imgData.getPixel(x1, y);
									totalIntensity += value;
									gravity.X += value * x1;
									gravity.Y += value * y;
								}
							}
							else
							{
								float slope = (pX[end] - pX[start]) / (pY[end] - pY[start]);
								float y1 = pY[start], y2 = pY[end], x;
								if (y1 > y2)
								{
									y1 = pX[end]; y2 = pX[start];
								}
								for (; y1 <= y2; y1 += 1.0f)
								{
									x = pX[start] + (y1 - pY[start]) * slope;
									numPixels++;
									ushort value = imgData.getPixel((int)x, (int)y1);
									totalIntensity += value;
									gravity.X += value * x;
									gravity.Y += value * y1;
								}
							}
						}

						start = end + 1;
					}
				}

				if (_nContours > 0)
				{
					//find index of point is Ymin, and index of point is Ymax
					int indStart = 0, indEnd = 0;
					int i = 0;
					int indLeft, indRight;
					float yCurrent = 0;

					float Ymin = _pY[_pIndexPoints[0]];
					float Ymax = _pY[_pIndexPoints[_nPoints - 1]];
					float yI = 0;

					int nIntersect = 0, nEdge = 0, nIntersectTmp = 0;

					indStart = 0;
					indEnd = indStart;

					yCurrent = Ymin;

					float yLeft = 0f, yRight = 0f;
					int xStart, xEnd, y = 0, nEdgeTmp = _nPoints - 1; ;

					nEdgeCount = 0;

					//Loop over scan lines
					for (; yCurrent <= Ymax; yCurrent += 1.0f)
					{
						y = (int)yCurrent;

						#region Main statement
						//found all edges, which intersect current scan line; and compute x intersect value		
						try
						{
							while (indEnd < _nPoints &&
								_pY[_pIndexPoints[indEnd]] <= yCurrent)
							{
								indEnd++;
							}
						}
						catch (System.IndexOutOfRangeException exp)
						{
							throw exp;
						}

						try
						{
							while (true)
							{
								if (indStart >= indEnd)
									break;
								indLeft = _pIndexLefts[_pIndexPoints[indStart]];
								indRight = _pIndexRights[_pIndexPoints[indStart]];
								if (indLeft >= 0 && indRight >= 0 &&
									_pY[indLeft] < yCurrent && _pY[indRight] < yCurrent)
									indStart++;
								else
									break;
							}
						}
						catch (System.IndexOutOfRangeException exp)
						{
							throw exp;
						}

						if (indStart >= _nPoints)
						{
							if (totalIntensity > 0)
							{
								gravity.X /= totalIntensity;
								gravity.Y /= totalIntensity;
							}
							return;
						}

						nIntersect = 0;
						nEdge = 0;

						try
						{
							for (i = indStart; i < indEnd; i++)
							{
								indLeft = _pIndexLefts[_pIndexPoints[i]];
								indRight = _pIndexRights[_pIndexPoints[i]];

								yI = _pY[_pIndexPoints[i]];

								if (indLeft >= 0)
								{
									yLeft = _pY[indLeft];
									if (yI == yCurrent)
									{
										//intersect at I 
										xIntersect[nIntersect++] = _pX[_pIndexPoints[i]];
										if (_pY[indLeft] == yCurrent)
										{
											float x1, x2;
											if (_pX[indLeft] >= _pX[_pIndexPoints[i]])
											{
												x1 = _pX[_pIndexPoints[i]];
												x2 = _pX[indLeft];
											}
											else
											{
												x1 = _pX[indLeft];
												x2 = _pX[_pIndexPoints[i]];
											}

											if (nEdge == 0 || (xEdge[nEdge - 2] != x1 && xEdge[nEdge - 1] != x2))
											{
												xEdge[nEdge++] = x1;
												xEdge[nEdge++] = x2;
											}
										}

										if (nEdgeCount == 0 || (pEdgeCount[nEdgeCount - 1] < i))
										{
											pEdgeCount[nEdgeCount++] = i;
										}
									}
									else if ((yI - yCurrent) * (yLeft - yCurrent) < 0)
									{
										//intersect at a point located between I and Right
										float slope = Slope(_pIndexPoints[i], indLeft);
										xIntersect[nIntersect++] = _pX[_pIndexPoints[i]] + (yCurrent - _pY[_pIndexPoints[i]]) * slope;

									}
									else
									{
										//no intersection
									}
								}

								if (indRight >= 0)
								{
									yRight = _pY[indRight];
									if (yI == yCurrent)
									{
										//intersect at I 
										xIntersect[nIntersect++] = _pX[_pIndexPoints[i]];
										if (_pY[indRight] == yCurrent)
										{
											float x1, x2;
											if (_pX[indRight] >= _pX[_pIndexPoints[i]])
											{
												x1 = _pX[_pIndexPoints[i]];
												x2 = _pX[indRight];
											}
											else
											{
												x1 = _pX[indRight];
												x2 = _pX[_pIndexPoints[i]];
											}

											if (nEdge == 0 || (xEdge[nEdge - 2] != x1 && xEdge[nEdge - 1] != x2))
											{
												xEdge[nEdge++] = x1;
												xEdge[nEdge++] = x2;
											}
										}

										if (nEdgeCount == 0 || (pEdgeCount[nEdgeCount - 1] < i))
										{
											pEdgeCount[nEdgeCount++] = i;
										}
									}
									else if ((yI - yCurrent) * (yRight - yCurrent) < 0)
									{
										//intersect at a point located between I and Right
										float slope = Slope(_pIndexPoints[i], indRight);
										xIntersect[nIntersect++] = _pX[_pIndexPoints[i]] + (yCurrent - _pY[_pIndexPoints[i]]) * slope;
									}
									else
									{
										//no intersection
									}
								}
							}
						}
						catch (System.IndexOutOfRangeException exp)
						{
							throw exp;
						}

						#endregion

						#region Collect points
						if (nIntersect > 0)
							QuickSort(ref xIntersect, 0, nIntersect - 1);

						nIntersectTmp = 0;
						if (nEdge > 0)
						{
							for (i = 0; i < nEdge; i++)
							{
								//collect points on edge
								xIntersectTmp[nIntersectTmp++] = xEdge[i];
							}

							SortEdge(ref xEdge, 0, nEdge / 2 - 1);

							MergeEdge(ref xEdge, ref nEdge);

							for (i = 0; i < nEdge; i += 2)
							{
								//collect points on edge
								xStart = (int)xEdge[i];
								xEnd = (int)xEdge[i + 1];

								for (; xStart <= xEnd; xStart++)
								{
									numPixels++;
									ushort value = imgData.getPixel(xStart, y);
									totalIntensity += value;
									gravity.X += value * xStart;
									gravity.Y += value * y;

								}
							}
						}

						//Improved
						if (nIntersect > 0 && nEdge <= 0)
						{
							int xPassed = -1;
							for (i = 0; i < nIntersect; i += 2)
							{
								xStart = (int)xIntersect[i];
								if (xStart == xPassed)
									xStart++;

								xEnd = (int)xIntersect[i + 1];

								for (; xStart <= xEnd; xStart++)
								{
									numPixels++;
									ushort value = imgData.getPixel(xStart, y);
									totalIntensity += value;
									gravity.X += value * xStart;
									gravity.Y += value * y;
								}
								xPassed = xEnd;
							}
						}
						else if (nIntersect > 0)
						{
							if (nIntersectTmp > 0)
								QuickSort(ref xIntersectTmp, 0, nIntersectTmp - 1);
							float jStart, jEnd;
							int iFind = 0;
							for (i = 0; i < nIntersect; i += 2)
							{
								if (CheckEdge(ref xEdge, nEdge, xIntersect[i], xIntersect[i + 1]))
									continue;
								jStart = xIntersect[i];
								jEnd = xIntersect[i + 1];
								if (nIntersectTmp > 0)
								{
									int tmp = QuickSearch(xIntersectTmp, jStart, iFind, nIntersectTmp - 1);
									if (tmp >= 0)
									{
										iFind = tmp;
										jStart += 1.0f;
									}
									tmp = QuickSearch(xIntersectTmp, jEnd, iFind, nIntersectTmp - 1);
									if (tmp >= 0)
									{
										iFind = tmp;
										jEnd -= 1.0f;
									}
								}

								xStart = (int)jStart;
								xEnd = (int)jEnd;

								for (; xStart <= xEnd; xStart++)
								{
									numPixels++;
									ushort value = imgData.getPixel(xStart, y);
									totalIntensity += value;
									gravity.X += value * xStart;
									gravity.Y += value * y;
								}
							}
						}
						#endregion
					}


					//here, xIntersect includes x-coordinates
					//xIntersectTmp include y-coordinates
					nIntersect = 0;
					for (i = 0; i < _nPoints; i++)
					{
						indLeft = _pIndexLefts[i];
						indRight = _pIndexRights[i];
						if (indLeft == -1 && indRight == -1)
						{
							bool IsIn = false;
							int j = 0;
							while (j < nIntersect)
							{
								if (xIntersect[j] == _pX[i] && xIntersectTmp[j] == _pY[i])
								{
									IsIn = true;
									break;
								}
								j++;
							}

							j = 0;
							while (j < nEdgeCount && IsIn == false)
							{
								if (_pX[i] == _pX[_pIndexPoints[pEdgeCount[j]]] && _pY[i] == _pY[_pIndexPoints[pEdgeCount[j]]])
								{
									IsIn = true;
								}
								j++;
							}

							if (IsIn == false)
							{
								numPixels++;
								ushort value = imgData.getPixel((int)_pX[i], (int)_pY[i]);
								totalIntensity += value;
								gravity.X += value * _pX[i];
								gravity.Y += value * _pY[i];


								//add to
								xIntersect[nIntersect] = _pX[i];
								xIntersectTmp[nIntersect] = _pY[i];
								nIntersect++;
							}
						}
					}
				}
			}
			catch (System.Exception exp)
			{
				throw exp;
			}
			if (totalIntensity > 0)
			{
				gravity.X /= totalIntensity;
				gravity.Y /= totalIntensity;
			}
		}

		public ArrayList GetObjectExtData(GreyDataImage imgData, PointExData extPoints)
		{
			ArrayList points = new ArrayList();

			try
			{		
				if(extPoints != null && extPoints.nContours > 0)
				{
					int nPoint = extPoints.nPoints;
					int nContour = extPoints.nContours;
					float[] pX = extPoints.pX;
					float[] pY = extPoints.pY;
					int[] pContour = extPoints.pIndexEndPointContours;

					int start = 0, end = start, i=0;
					for(; i<nContour; i++)
					{
						end = pContour[i];
						if(end == start)
						{
							Point p = new Point((int)pX[start], (int)pY[start]);
							points.Add(p);
						}
						else if(end == start+1)
						{							
							if(pY[start] == pY[end])
							{
								int x1 = (int)pX[start], x2 = (int)pX[end];
								int y = (int)pY[start];
								if(x1 > x2)
								{
									x1 = (int)pX[end]; x2 = (int)pX[start];
								}																									
								for(; x1<=x2; x1++)
								{
									Point p = new Point(x1, y);
									points.Add(p);									
								}
							}
							else
							{
								float slope = (pX[end] - pX[start])/(pY[end] - pY[start]);
								float y1 = pY[start], y2 = pY[end], x;								
								if(y1 > y2)
								{
									y1 = pX[end]; y2 = pX[start];
								}
								for(; y1<=y2; y1 += 1.0f)
								{					
									x = pX[start] + (y1 - pY[start]) * slope;
									Point p = new Point((int)x, (int)y1);
									points.Add(p);
								}
							}
						}

						start = end + 1;
					}
				}

				if(_nContours > 0)
				{
					//find index of point is Ymin, and index of point is Ymax
					int indStart = 0, indEnd = 0;
					int i = 0;
					int indLeft, indRight;
					float yCurrent = 0;

					float Ymin = _pY[_pIndexPoints[0]];
					float Ymax = _pY[_pIndexPoints[_nPoints - 1]];
					float yI = 0;

					int nIntersect = 0, nEdge = 0, nIntersectTmp = 0;

					indStart = 0;
					indEnd = indStart;
			
					yCurrent = Ymin;

					float yLeft = 0f, yRight = 0f;
					int xStart, xEnd, y = 0, nEdgeTmp = _nPoints-1;;

					nEdgeCount = 0;

					//Loop over scan lines
					for (; yCurrent <= Ymax; yCurrent += 1.0f)
					{
						y = (int)yCurrent;

						#region Main statement
						//found all edges, which intersect current scan line; and compute x intersect value		
						try
						{
							while (indEnd < _nPoints &&
								_pY[_pIndexPoints[indEnd]] <= yCurrent)
							{
								indEnd++;
							}
						}
						catch (System.IndexOutOfRangeException exp)
						{
							throw exp;
						}

						try
						{
							while (true)
							{
								if (indStart >= indEnd)
									break;
								indLeft = _pIndexLefts[_pIndexPoints[indStart]];
								indRight = _pIndexRights[_pIndexPoints[indStart]];
								if (indLeft >= 0 && indRight >= 0 &&
									_pY[indLeft] < yCurrent && _pY[indRight] < yCurrent)
									indStart++;
								else
									break;
							}
						}
						catch (System.IndexOutOfRangeException exp)
						{
							throw exp;
						}

						if (indStart >= _nPoints)
							return points;

						nIntersect = 0;
						nEdge = 0;

						try
						{
							for (i = indStart; i < indEnd; i++)
							{
								indLeft = _pIndexLefts[_pIndexPoints[i]];
								indRight = _pIndexRights[_pIndexPoints[i]];

								yI = _pY[_pIndexPoints[i]];

								if (indLeft >= 0)
								{
									yLeft = _pY[indLeft];
									if (yI == yCurrent)
									{
										//intersect at I 
										xIntersect[nIntersect++] = _pX[_pIndexPoints[i]];
										if(_pY[indLeft] == yCurrent)
										{
											float x1, x2;
											if(_pX[indLeft] >= _pX[_pIndexPoints[i]])
											{
												x1 = _pX[_pIndexPoints[i]];
												x2 = _pX[indLeft];
											}
											else 
											{
												x1 = _pX[indLeft];
												x2 = _pX[_pIndexPoints[i]];
											}

											if( nEdge == 0 || (xEdge[nEdge-2] != x1 && xEdge[nEdge-1] != x2) )
											{
												xEdge[nEdge++] = x1;
												xEdge[nEdge++] = x2;
											}
										}

										if( nEdgeCount == 0 || (pEdgeCount[nEdgeCount-1] < i) )
										{
											pEdgeCount[nEdgeCount++] = i;
										}
									}
									else if ((yI - yCurrent) * (yLeft - yCurrent) < 0)
									{
										//intersect at a point located between I and Right
										float slope = Slope(_pIndexPoints[i], indLeft);
										xIntersect[nIntersect++] = _pX[_pIndexPoints[i]] + (yCurrent - _pY[_pIndexPoints[i]]) * slope;

									}
									else
									{
										//no intersection
									}
								}

								if (indRight >= 0)
								{
									yRight = _pY[indRight];
									if (yI == yCurrent)
									{
										//intersect at I 
										xIntersect[nIntersect++] = _pX[_pIndexPoints[i]];
										if(_pY[indRight] == yCurrent)
										{
											float x1, x2;
											if(_pX[indRight] >= _pX[_pIndexPoints[i]])
											{
												x1 = _pX[_pIndexPoints[i]];
												x2 = _pX[indRight];
											}
											else 
											{
												x1 = _pX[indRight];
												x2 = _pX[_pIndexPoints[i]];
											}

											if( nEdge == 0 || (xEdge[nEdge-2] != x1 && xEdge[nEdge-1] != x2) )
											{
												xEdge[nEdge++] = x1;
												xEdge[nEdge++] = x2;
											}
										}
										
										if( nEdgeCount == 0 || (pEdgeCount[nEdgeCount-1] < i) )
										{
											pEdgeCount[nEdgeCount++] = i;
										}
									}
									else if ((yI - yCurrent) * (yRight - yCurrent) < 0)
									{
										//intersect at a point located between I and Right
										float slope = Slope(_pIndexPoints[i], indRight);
										xIntersect[nIntersect++] = _pX[_pIndexPoints[i]] + (yCurrent - _pY[_pIndexPoints[i]]) * slope;
									}
									else
									{
										//no intersection
									}
								}							
							}
						}
						catch (System.IndexOutOfRangeException exp)
						{
							throw exp;
						}

						#endregion

						#region Collect points
						if(nIntersect > 0)
							QuickSort(ref xIntersect, 0, nIntersect - 1);

						nIntersectTmp = 0;
						if(nEdge > 0)
						{
							for(i=0; i<nEdge; i++)
							{
								//collect points on edge
								xIntersectTmp[nIntersectTmp++] = xEdge[i];							
							}

							SortEdge(ref xEdge, 0, nEdge/2-1);

							MergeEdge(ref xEdge, ref nEdge);

							for(i=0; i<nEdge; i+=2)
							{
								//collect points on edge
								xStart = (int)xEdge[i];							
								xEnd = (int)xEdge[i+1];
						
								for (; xStart <= xEnd; xStart++)
								{
									Point p = new Point(xStart, y);
									points.Add(p);									
								}
							}
						}

						//Improved
						if (nIntersect > 0 && nEdge <= 0)
						{	
							int xPassed = -1;
							for (i = 0; i < nIntersect; i += 2)
							{													
								xStart = (int)xIntersect[i];
								if(xStart == xPassed)
									xStart++;
						
								xEnd = (int)xIntersect[i + 1];

								for (; xStart <= xEnd; xStart++)
								{
									Point p = new Point(xStart, y);
									points.Add(p);
								}
								xPassed = xEnd;
							}
						}
						else if(nIntersect > 0)
						{
							if(nIntersectTmp > 0)
								QuickSort(ref xIntersectTmp, 0, nIntersectTmp-1);
							float jStart, jEnd;
							int iFind = 0;
							for (i = 0; i < nIntersect; i += 2)
							{
								if( CheckEdge(ref xEdge, nEdge, xIntersect[i], xIntersect[i+1]) )
									continue;
								jStart = xIntersect[i];
								jEnd = xIntersect[i+1];
								if(nIntersectTmp > 0)
								{
									int tmp = QuickSearch(xIntersectTmp, jStart, iFind, nIntersectTmp-1);
									if(tmp >= 0)
									{
										iFind = tmp;
										jStart += 1.0f;
									}
									tmp = QuickSearch(xIntersectTmp, jEnd, iFind, nIntersectTmp-1);
									if(tmp >= 0)
									{
										iFind = tmp;
										jEnd -= 1.0f;
									}
								}
						
								xStart = (int)jStart;
								xEnd = (int)jEnd;

								for (; xStart <= xEnd; xStart++)
								{
									Point p = new Point(xStart, y);
									points.Add(p);
								}				
							}
						}
						#endregion
					}


					//here, xIntersect includes x-coordinates
					//xIntersectTmp include y-coordinates
					nIntersect = 0;
					for(i = 0; i<_nPoints; i++)
					{	
						indLeft = _pIndexLefts[i];
						indRight = _pIndexRights[i];
						if(indLeft == -1 && indRight == -1)
						{							
							bool IsIn = false;
							int j = 0;
							while(j < nIntersect)
							{
								if(xIntersect[j] == _pX[i] && xIntersectTmp[j] == _pY[i])
								{
									IsIn = true;
									break;
								}
								j++;
							}

							j = 0;
							while(j < nEdgeCount && IsIn == false)
							{
								if(_pX[i] == _pX[_pIndexPoints[pEdgeCount[j]]] && _pY[i] == _pY[_pIndexPoints[pEdgeCount[j]]])
								{
									IsIn = true;
								}
								j++;
							}

							if(IsIn == false)
							{
								Point p = new Point((int)_pX[i], (int)_pY[i]);
								points.Add(p);

								//add to
								xIntersect[nIntersect] = _pX[i];
								xIntersectTmp[nIntersect] = _pY[i];
								nIntersect++;
							}
						}					
					}
				}			
			}
			catch (System.Exception exp)
			{
				if (points != null)
					points.Clear();
				throw exp;
			}

			return points;
		}

		public void GetPointDefects(ArrayList defects, PointExData extPoints)
		{
			if (bInside) // object is inside wafer
			{
				#region inside
				float x = 0, y = 0, nps = 0;
				
				try
				{				
					if (_nPoints > 0) 
						nps += (float)_nPoints;
					
					for (int i=0; i<_nPoints; i++)
					{
						x += _pX[i];
						y += _pY[i];
					}

					if (extPoints != null)
					{						
						for (int i=0; i<extPoints.nPoints; i++)
						{
							x += extPoints.pX[i];
							y += extPoints.pY[i];
						}
						nps += (float)extPoints.nPoints;
					}

					// make sure that objects is in wafer and exist really.
					if (nps > 0)
					{					
						x = x/nps;
						y = y/nps;
					
						// Add defect
						ClusterEx cluster = new ClusterEx();
						cluster.Rectangle = new RectangleF(x-0.5f, y-0.5f, 1.0f, 1.0f);
						defects.Add(cluster);
					}
				}
				catch (System.Exception)
				{
					throw;
				}				
				#endregion inside
			}
			else // object lies wafer boundary
			{
				#region object acrosses the wafer boundary
				float x = 0, y = 0, nps = 0;

				int start = 0;
				int end = start;
				int previous = 0;
				int next = 0;

				for(int i=0; i<_nContours; i++)
				{
					end = _pIndexEndPointContours[i];
					
					for(int j=start; j<=end; j++)
					{
						if (CheckIn(_pX[j], _pY[j]))
						{
							x += _pX[j];
							y += _pY[j];
							nps  += 1.0f;
						}
						else
						{							
							if(j > start)
								previous = j-1;
							else
								previous = end;
							if(j < end)
								next = j+1;
							else
								next = start;
							if (CheckIn(_pX[previous], _pY[previous]))
							{
								PointF intersect = Intersect(_pX[j], _pY[j], _pX[previous], _pY[previous]);
								x += intersect.X;
								y += intersect.Y;
								nps  += 1.0f;
							}

							if (CheckIn(_pX[next], _pY[next]))
							{
								PointF intersect = Intersect(_pX[j], _pY[j], _pX[next], _pY[next]);
								x += intersect.X;
								y += intersect.Y;
								nps  += 1.0f;
							}
						}
					}					
					start = end+1;
				}

				if (extPoints != null)
				{					
					start = 0;
					end = 0;
					
					for (int i=0; i<extPoints.nContours; i++)
					{
						end = extPoints.pIndexEndPointContours[i];

						for (int j=start; j<=end; j++)
						{
							if (CheckIn(extPoints.pX[j], extPoints.pY[j]))
							{
								x += extPoints.pX[j];
								y += extPoints.pY[j];
								nps  += 1.0f;
							}
							else
							{							
								if(j > start)
									previous = j-1;
								else
									previous = end;
								if(j < end)
									next = j+1;
								else
									next = start;
								if (CheckIn(extPoints.pX[previous], extPoints.pY[previous]))
								{
									PointF intersect = Intersect(extPoints.pX[j], extPoints.pY[j], extPoints.pX[previous], extPoints.pY[previous]);
									x += intersect.X;
									y += intersect.Y;
									nps  += 1.0f;
								}

								if (CheckIn(extPoints.pX[next], extPoints.pY[next]))
								{
									PointF intersect = Intersect(extPoints.pX[j], extPoints.pY[j], extPoints.pX[next], extPoints.pY[next]);
									x += intersect.X;
									y += intersect.Y;
									nps  += 1.0f;
								}
							}
						}

						start = end+1;						
					}					
				}

				// make sure that objects is in wafer and exist really.
				if (nps > 0)
				{					
					x = x/nps;
					y = y/nps;

					// Add defect
					ClusterEx cluster = new ClusterEx();
					cluster.Rectangle = new RectangleF(x-0.5f, y-0.5f, 1.0f, 1.0f);
					defects.Add(cluster);
				}
				#endregion object acrosses the wafer boundary
			}
		}

		public void GetAreaDefects(ArrayList defects, PointExData extPoints, float sizCell)
		{			
			try
			{		
				float halfSize = sizCell/2;

				#region Process for small contours
				if(extPoints != null && extPoints.nContours > 0)
				{
					int nPoint = extPoints.nPoints;
					int nContour = extPoints.nContours;
					float[] pX = extPoints.pX;
					float[] pY = extPoints.pY;
					int[] pContour = extPoints.pIndexEndPointContours;

					int start = 0, end = start, i=0;
					float xx, yy;
					for(; i<nContour; i++)
					{
						end = pContour[i];
						if(end == start)
						{
							xx = GetCenterCell(pX[start], sizCell)-halfSize;							
							yy = GetCenterCell(pY[start], sizCell)-halfSize;
							//check in wafer boundary
							if (CheckInWaferBoundary(ref xx, ref yy))
							{
								ClusterEx defect = new ClusterEx();
								defect.Rectangle = new RectangleF(xx, yy, sizCell, sizCell);							
								defects.Add(defect);
							}
						}
						else if(end == start+1)
						{							
							if(pY[start] == pY[end])
							{
								float x1 = GetCenterCell(pX[start], sizCell), x2 = GetCenterCell(pX[end], sizCell);
								float y = GetCenterCell(pY[start], sizCell);
								if(x1 > x2)
								{
									x1 = GetCenterCell(pX[end], sizCell); x2 = GetCenterCell(pX[start], sizCell);
								}			
								
								for(; x1<=x2; x1 += sizCell)
								{
									xx = x1-halfSize;
									yy = y-halfSize;
									if (CheckInWaferBoundary(ref xx, ref yy))
									{
										ClusterEx defect = new ClusterEx();
										defect.Rectangle = new RectangleF(xx, yy, sizCell, sizCell);							
										defects.Add(defect);
									}
								}								
							}
							else
							{
								float slope = (pX[end] - pX[start])/(pY[end] - pY[start]);
								float y1 = GetCenterCell(pY[start], sizCell), y2 = GetCenterCell(pY[end], sizCell);								
								if(y1 > y2)
								{
									y1 = GetCenterCell(pY[end], sizCell); y2 = GetCenterCell(pY[start], sizCell);
								}
								for(; y1<=y2; y1 += sizCell)
								{									
									xx = GetCenterCell(pX[start] + (y1 - pY[start]) * slope, sizCell) - halfSize;
									yy = y1-halfSize;
									if (CheckInWaferBoundary(ref xx, ref yy))
									{
										ClusterEx defect = new ClusterEx();
										defect.Rectangle = new RectangleF(xx, yy, sizCell, sizCell);							
										defects.Add(defect);
									}
								}
							}
						}

						start = end + 1;
					}
				}

				#endregion

				if(_nContours > 0)
				{
					#region Initialize

					// find index of point is Ymin, and index of point is Ymax
					int indStart = 0, indEnd = 0;
					int i = 0;
					int indLeft, indRight;
					float yCurrent = 0;

					float yLineMin = _pY[_pIndexPoints[0]];
					float yLineMax = _pY[_pIndexPoints[_nPoints - 1]];
					
					float yI = 0;

					int nIntersect = 0, nEdge = 0, nIntersectTmp = 0;

					indStart = 0;
					indEnd = indStart;
			
					yCurrent = yLineMin;

					float yLeft = 0f, yRight = 0f;
					float xStart, xEnd, y = 0;
					int nEdgeTmp = _nPoints-1;

					// for area defects
					float yAreaMin = GetCenterCell(yLineMin, sizCell);
					float yAreaMax = GetCenterCell(yLineMax, sizCell);
					float yAreaCurrent = yAreaMin;
					int n_xAreaDefects = 0, n_xLineDefects = 0;

					float yLineStart=0, yLineEnd = yLineStart;

					nEdgeCount = 0;

					#endregion

					// Loop over scan lines

					for(; yAreaCurrent <= yAreaMax; yAreaCurrent += sizCell)
					{
						
						yLineEnd = yAreaCurrent + sizCell;
						n_xAreaDefects = 0;

						#region Process for an area line
						
						// Process for an area line
						for (; yCurrent <yLineEnd && yCurrent <= yLineMax; yCurrent += 1.0f)
						{
							y = yCurrent;
							n_xLineDefects = 0;								

							#region Main statement
							// found all edges, which intersect current scan line; and compute x intersect value		
							try
							{
								while (indEnd < _nPoints &&
									_pY[_pIndexPoints[indEnd]] <= yCurrent)
								{
									indEnd++;
								}
							}
							catch (System.IndexOutOfRangeException exp)
							{
								Trace.WriteLine(exp);
							}

							try
							{
								while (true)
								{
									if (indStart >= indEnd)
										break;
									indLeft = _pIndexLefts[_pIndexPoints[indStart]];
									indRight = _pIndexRights[_pIndexPoints[indStart]];
									if (indLeft >= 0 && indRight >= 0 &&
										_pY[indLeft] < yCurrent && _pY[indRight] < yCurrent)
										indStart++;
									else
										break;
								}
							}
							catch (System.IndexOutOfRangeException exp)
							{
								Trace.WriteLine(exp);
							}

							if (indStart >= _nPoints)
								return;

							nIntersect = 0;
							nEdge = 0;
							nEdgeCount = 0;

							try
							{
								for (i = indStart; i < indEnd; i++)
								{
									indLeft = _pIndexLefts[_pIndexPoints[i]];
									indRight = _pIndexRights[_pIndexPoints[i]];

									yI = _pY[_pIndexPoints[i]];

									#region Left
									if (indLeft >= 0)
									{
										yLeft = _pY[indLeft];
										if (yI == yCurrent)
										{
											//intersect at I 
											xIntersect[nIntersect++] = _pX[_pIndexPoints[i]];
											if(_pY[indLeft] == yCurrent)
											{
												float x1, x2;
												if(_pX[indLeft] >= _pX[_pIndexPoints[i]])
												{
													x1 = _pX[_pIndexPoints[i]];
													x2 = _pX[indLeft];
												}
												else 
												{
													x1 = _pX[indLeft];
													x2 = _pX[_pIndexPoints[i]];
												}

												if( nEdge == 0 || (xEdge[nEdge-2] != x1 && xEdge[nEdge-1] != x2) )
												{
													xEdge[nEdge++] = x1;
													xEdge[nEdge++] = x2;
												}
											}											
										}
										else if ((yI - yCurrent) * (yLeft - yCurrent) < 0)
										{
											//intersect at a point located between I and Right
											float slope = Slope(_pIndexPoints[i], indLeft);
											xIntersect[nIntersect++] = _pX[_pIndexPoints[i]] + (yCurrent - _pY[_pIndexPoints[i]]) * slope;

										}										
									}
									#endregion

									#region Right
									if (indRight >= 0)
									{
										yRight = _pY[indRight];
										if (yI == yCurrent)
										{
											//intersect at I 
											xIntersect[nIntersect++] = _pX[_pIndexPoints[i]];
											if(_pY[indRight] == yCurrent)
											{
												float x1, x2;
												if(_pX[indRight] >= _pX[_pIndexPoints[i]])
												{
													x1 = _pX[_pIndexPoints[i]];
													x2 = _pX[indRight];
												}
												else 
												{
													x1 = _pX[indRight];
													x2 = _pX[_pIndexPoints[i]];
												}

												if( nEdge == 0 || (xEdge[nEdge-2] != x1 && xEdge[nEdge-1] != x2) )
												{
													xEdge[nEdge++] = x1;
													xEdge[nEdge++] = x2;
												}
											}										
										}
										else if ((yI - yCurrent) * (yRight - yCurrent) < 0)
										{
											//intersect at a point located between I and Right
											float slope = Slope(_pIndexPoints[i], indRight);
											xIntersect[nIntersect++] = _pX[_pIndexPoints[i]] + (yCurrent - _pY[_pIndexPoints[i]]) * slope;
										}										
									}
									#endregion
									  
									if(yI == yCurrent && indLeft < 0 && indRight < 0)
										if(nEdgeCount == 0 || pEdgeCount[nEdgeCount-1] < i)
											pEdgeCount[nEdgeCount++] = i;
								}
							}
							catch (System.IndexOutOfRangeException exp)
							{
								Trace.WriteLine(exp);
							}

							#endregion

							#region Collect points

							if(nIntersect > 0)
								QuickSort(ref xIntersect, 0, nIntersect - 1);

							nIntersectTmp = 0;
							if(nEdge > 0)
							{
								for(i=0; i<nEdge; i++)
								{
									//collect points on edge
									xIntersectTmp[nIntersectTmp++] = xEdge[i];							
								}

								SortEdge(ref xEdge, 0, nEdge/2-1);

								MergeEdge(ref xEdge, ref nEdge);

								for(i=0; i<nEdge; i+=2)
								{
									//collect points on edge
									xStart = xEdge[i];							
									xEnd = xEdge[i+1];
									
									xLineDefects[n_xLineDefects++] = xStart;
									xLineDefects[n_xLineDefects++] = xEnd;
								}
							}

							//Improved
							if (nIntersect > 0 && nEdge <= 0)
							{	
								float xPassed = -1;
								for (i = 0; i < nIntersect; i += 2)
								{													
									xStart = xIntersect[i];
									if(xStart == xPassed)
										xStart++;
						
									xEnd = xIntersect[i + 1];
									
									xLineDefects[n_xLineDefects++] = xStart;
									xLineDefects[n_xLineDefects++] = xEnd;

									xPassed = xEnd;									
								}
							}
							else if(nIntersect > 0)
							{
								if(nIntersectTmp > 0)
									QuickSort(ref xIntersectTmp, 0, nIntersectTmp-1);
								float jStart, jEnd;
								int iFind = 0;
								for (i = 0; i < nIntersect; i += 2)
								{
									if( CheckEdge(ref xEdge, nEdge, xIntersect[i], xIntersect[i+1]) )
										continue;
									jStart = xIntersect[i];
									jEnd = xIntersect[i+1];
									if(nIntersectTmp > 0)
									{
										int tmp = QuickSearch(xIntersectTmp, jStart, iFind, nIntersectTmp-1);
										if(tmp >= 0)
										{
											iFind = tmp;
											jStart += 1.0f;
										}
										tmp = QuickSearch(xIntersectTmp, jEnd, iFind, nIntersectTmp-1);
										if(tmp >= 0)
										{
											iFind = tmp;
											jEnd -= 1.0f;
										}
									}
						
									xStart = jStart;
									xEnd = jEnd;
									
									xLineDefects[n_xLineDefects++] = xStart;
									xLineDefects[n_xLineDefects++] = xEnd;
								}
							}

							//check for edges, which don't have any Left Edge or Right Edge
							if(nEdgeCount > 0)
							{
								for(i=0; i<nEdgeCount; i++)
								{
									xLineDefects[n_xLineDefects++] = _pX[_pIndexPoints[pEdgeCount[i]]];
									xLineDefects[n_xLineDefects++] = _pX[_pIndexPoints[pEdgeCount[i]]];
								}
							}

							if(n_xLineDefects > 0)
							{
								for(i=0; i<n_xLineDefects; i++)
									xAreaDefects[n_xAreaDefects++] = xLineDefects[i];
							
								//Sort by x							
								Sort_xIntersect(ref xAreaDefects, 0, n_xAreaDefects/2-1);

								//Merge							
								Merge_xIntersect(ref xAreaDefects, ref n_xAreaDefects);
							}

							//end of collecting points
							#endregion
						}
					
						float xIntersectPassed = -1;
						y = yAreaCurrent - halfSize;
												
						if(y >= centerY - radius && y <= centerY + radius)
						{							
							float xx, yy;							
							for (i = 0; i < n_xAreaDefects; i += 2)
							{													
								xStart = GetCenterCell(xAreaDefects[i], sizCell) - halfSize;
								if(xAreaDefects[i] == xIntersectPassed)
									xStart += sizCell;
							
								xEnd = GetCenterCell(xAreaDefects[i + 1], sizCell) - halfSize;
										
								if(bInside)
								{
									for(; xStart <= xEnd; xStart += sizCell)
									{			
										ClusterEx defect = new ClusterEx();
										defect.Rectangle = new RectangleF(xStart, y, sizCell, sizCell);							
										defects.Add(defect);
									}
								}
								else
								{
									for(; xStart <= xEnd; xStart += sizCell)
									{
										xx = xStart;
										yy = y;
										//Rotate(ref xx, ref yy);

										//check in wafer boundary
										if((centerX-xx)*(centerX-xx) > sqrRadius - (yy-centerY)*(yy-centerY))
											continue;
										if(hasFlat && cFlat*(dXFlat*(centerY-yy)-dYFlat*(xx-centerX)+cFlat) < 0)
											continue;
										
										ClusterEx defect = new ClusterEx();
										defect.Rectangle = new RectangleF(xx, yy, sizCell, sizCell);							
										defects.Add(defect);
									}
								}

								xIntersectPassed = xAreaDefects[i+1];
							}						
						}

						//end of processing for an area line
						#endregion
					}
				}			
			}
			catch (System.Exception exp)
			{
				Trace.WriteLine(exp);				
			}
		}
		
		#endregion		

		#region Helpers

		public int Round(int x, int sizeRound)
		{
			if (x % sizeRound == 0)
				return x;
			return (x + (sizeRound - x % sizeRound));
		}
		
		public int RoundDown(int x, int sizeRound)
		{
			return (x - x % sizeRound);
		}

		public int RoundUp(int x, int sizeRound)
		{
			if (x % sizeRound == 0)
				return x;
			return (x + (sizeRound - x % sizeRound));
		}

		public float RoundDown(float x, float sizeRound)
		{
			float dY = (float)((int)(x/sizeRound));
			if(dY*sizeRound < x)
				return ( (dY)*sizeRound );
			return x;			
		}

		public float RoundUp(float x, float sizeRound)
		{
			float dY = (float)((int)(x/sizeRound));
			if(dY*sizeRound < x)
				return ((dY+1)*sizeRound);
			return x;
		}

		public float GetCenterCell(float x, float sizeCell)
		{
			float dY = (float)((int)(x/sizeCell));
			return (dY*sizeCell);
		}

		public void DrawPolygon(Graphics g)
		{
			Pen pen = new Pen(Color.Red, 1);
			float x, y;
			int start=0, end;
			for(int i=0; i<_nContours; i++)
			{
				end = _pIndexEndPointContours[i];
				x = _pX[end];
				y = _pY[end];
				for(int j=start; j<=end; j++)
				{
					g.DrawLine(pen, x, y, _pX[j], _pY[j]);
					x = _pX[j]; y = _pY[j];
				}
				start = end+1;
			}
		}

		public void Scale(float scale)
		{
			int start=0, end;
			for(int i=0; i<_nContours; i++)
			{
				end = _pIndexEndPointContours[i];
				for(int j=start; j<=end; j++)
				{
					_pX[j] *= scale; 
					_pY[j] *= scale;
				}
				start = end+1;
			}
		}

		#endregion
	}
}
