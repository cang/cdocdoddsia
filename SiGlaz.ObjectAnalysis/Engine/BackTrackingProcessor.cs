#if DEBUG
	#define PRIV_TRACE_
	#define SOL_TRACE_
	#define ARC_BEST_
	#define TRACE_TXT_
	#define NEW
#endif
using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using SiGlaz.ObjectAnalysis.Common;
using SiGlaz.Algorithms.Core;

namespace SiGlaz.ObjectAnalysis.Engine
{
	#region BackTrackingProcessor
	/// <summary>
	/// BackTrackingProcessor: The abstract class for back tracking algorithm.
	/// All of its descendant must realizes the methods ...
	/// Feature Space Structure: 
	///		[X, Y, Orientation, Density, Major, Minor]
	/// </summary>
	public abstract class BackTrackingProcessor
	{
		protected BasePoint [] m_sourceSpace;
		protected bool OrderedSolution = false;
		//private ArrayList m_remainList;
		private bool [] _maskMatrix;
		private bool _poping = false;
		protected bool [] _fixed_maskMatrix;
		private TOPO_DETECT_MODE _detectMode = TOPO_DETECT_MODE.NORMAL;
		public TOPO_DETECT_MODE DetectMode
		{
			get
			{
				return _detectMode;
			}
			set
			{
				_detectMode = value;
			}
		}

		protected int m_NumPoints;
		protected ArrayList _current;
		public BackTrackingProcessor()
		{
		}
		public BackTrackingProcessor(BasePoint [] _sourceSpace):this()
		{
			m_sourceSpace = _sourceSpace;
			m_NumPoints = m_sourceSpace.GetLength(0);
		}
		protected virtual void Reset()
		{
			if (_maskMatrix == null ||
				_maskMatrix.Length != m_NumPoints)
			{
				_maskMatrix = new bool[m_NumPoints];
			}
			else
				Array.Clear(_maskMatrix,0,m_NumPoints);

			if (_fixed_maskMatrix == null ||
				_fixed_maskMatrix.Length != m_NumPoints)
			{
				_fixed_maskMatrix = new bool[m_NumPoints];
			}
			else
				Array.Clear(_fixed_maskMatrix,0,m_NumPoints);


			if (_current == null)
				_current = new ArrayList();
			else
				_current.Clear();

//			if (m_remainList == null)
//				m_remainList = new ArrayList(m_NumPoints);
//			else
//			{
//				m_remainList.Clear();
//				m_remainList.Capacity = m_NumPoints;
//			}
//			for (int i=0;i<m_NumPoints;i++)
//			{
//				m_remainList.Add(i);
//			}

		}
		protected abstract bool CheckAdding(int _iPoint);
		protected abstract bool FoundCondition();
		protected abstract bool AddSolution(ArrayList result);
		protected int Last
		{
			get
			{
				return (int)_current[_current.Count - 1];
			}
			set
			{
				_current[_current.Count -1] = value;
			}
		}
		public ArrayList Solution
		{
			get
			{
				return _current;
			}
		}
		protected virtual void Push(int _iPoint)
		{
			_current.Add(_iPoint);
			//m_remainList.Remove(_iPoint);
			_maskMatrix[_iPoint] = true;
			_poping = false;
		}
		protected virtual int Pop()
		{		
			//m_remainList.Add((int)_current[_current.Count-1]);
			int result = Last;
			_maskMatrix[result] = false;
			_current.RemoveAt(_current.Count-1);
			_poping = true;
			return result;
		}
		protected bool NextAvailable(ref int _z)
		{
			if (!OrderedSolution || _poping)
				_z ++;
			else
				_z = 0;
			while (_z < m_NumPoints && 
				(_fixed_maskMatrix[_z] || 
				_maskMatrix[_z] || 
				!CheckAdding(_z))
				)
				_z ++;
			return ((_z < m_NumPoints) && (!_fixed_maskMatrix[_z]));
		}
		public int FindOneSolution()
		{
			if (m_NumPoints == 0)
				return 0;
			int result = 0;
			for (int i=0;i<m_NumPoints;i++)
			{
				result = FindOneSolution(i);
				if (result > 0)
					return result;
			}
			return 0;
		}
		public int FindOneSolution(int _initPoint)
		{
			if (m_NumPoints == 0)
				return 0;
			try
			{
				Reset();
				int _tmp =_initPoint;
				Push(_initPoint);
				while (true)
				{
#if SOL_TRACE
					foreach (int _z in _current)
					{
						Console.Write("{0}\t",_z.ToString());
					}
					Console.WriteLine("");
#endif
					if (NextAvailable(ref _tmp))
					{
						Push(_tmp);
					}
					else
					{
						if (FoundCondition())
						{
#if SOL_TRACE
							Console.WriteLine("Found solution.");
#endif
							return 1;
						}
						if (_current.Count == 0)
							break;
						_tmp = Pop();
					}
				}
			}
#if PRIV_TRACE
			catch (System.Exception exp)
			{
				Console.WriteLine(exp.Message);
#else
			catch
			{
#endif
				return -1;
			}
			return 0;
		}
		public ArrayList FindAllSolution()
		{
			if (m_NumPoints == 0)
				return null;
			ArrayList result = new ArrayList();
			_poping = false;
			try
			{
				Reset();
				int _tmp =-1;
				while (true)
				{
#if SOL_TRACE 
					foreach (int _z in _current)
					{
						Console.Write("{0}\t",_z.ToString());
					}
					Console.WriteLine("");
#endif
					if (NextAvailable(ref _tmp))
					{
						Push(_tmp);
					}
					else
					{
						if (!_poping && 
							FoundCondition())
						{
							AddSolution(result);
						}
						if (_current.Count == 0)
							break;
						_tmp = Pop();
					}
				}
			}
#if PRIV_TRACE 
			catch (System.Exception exp)
			{
				Console.WriteLine(exp.Message);
#else
			catch
			{
#endif
				return null;
			}
			if (result.Count == 0)
				result = null;
			return result;
		}
	}
/*
	public abstract class RatingBTProcessor: BackTrackingProcessor
	{
		protected ArrayList _queue = null;
		public RatingBTProcessor(BasePoint[] _soureSpace) : base (_sourceSpace)
		{
			_queue = new ArrayList();
		}
		protected void PushInQueue(int _iPoint)
		{

		}
		protected abstract bool RatingQueue();
		public new ArrayList FoundAllSolution()
		{
			if (m_NumPoints == 0)
				return null;
			ArrayList result = new ArrayList();
			_poping = false;
			try
			{
				Reset();
				int _tmp =-1;
				while (true)
				{
#if SOL_TRACE 
					foreach (int _z in _current)
					{
						Console.Write("{0}\t",_z.ToString());
					}
					Console.WriteLine("");
#endif
					if (NextAvailable(ref _tmp))
					{
						PushInQueue(_tmp);
					}
					else
					{

						if (!_poping && 
							FoundCondition())
						{
							AddSolution(result);
						}
						if (_current.Count == 0)
							break;
						_tmp = Pop();
					}
				}
			}
			catch (System.Exception exp)
			{
#if PRIV_TRACE 
				Console.WriteLine(exp.Message);
#endif
				return null;
			}
			if (result.Count == 0)
				result = null;
			return result;
		}
		
	}
	
*/
#endregion

	#region TopologyBackTracking
	public abstract class TopologyBackTracking : BackTrackingProcessor
	{
		private TOPOLOGY _Topo;
		public TOPOLOGY Topo
		{
			get
			{
				return _Topo;
			}
		}
		public TopologyBackTracking(BasePoint [] _featureSpace, TOPOLOGY _topo):base(_featureSpace)
		{
			_Topo = _topo;
		}
		protected override bool AddSolution(ArrayList result)
		{
			result.Add(_current.Clone());
			(result[result.Count-1] as ArrayList).TrimToSize();
#if SOL_TRACE
			Console.WriteLine("Found solution.");
#endif
			foreach (int _ip in _current)
			{
				_fixed_maskMatrix[_ip] = true;
			}
			return true;
		}
	}
	#endregion

	#region LineBackTracking
	public class LineBackTracking : TopologyBackTracking
	{
		private static double _degree_mul = 180.0 / Math.PI;
		private static double _radian_mul = Math.PI / 180.0;
		private class TmpLineFeature
		{
			private BasePoint[] m_sourceSpace = null;
			private int N;
			public int NumPoints
			{
				get
				{
					return N;
				}
			}
			private int _lastAdded;
			public int LastAdded
			{
				get
				{
					return _lastAdded;
				}
			}
			public double Sx2=0D; 
			public double Sy2=0D;
			public double Sxy=0D;
			public double Sx=0D;
			public double Sy=0D;
			private float _minX = 0f;
			private float _minY = 0f;
			private float _maxX = 0f;
			private float _maxY = 0f;
			private int _iminX = 0, _iminY = 0, _imaxX = 0, _imaxY = 0;
			private BasePoint _beginBasePoint;
			public BasePoint BeginPoint
			{
				get
				{
					return _beginBasePoint;
				}
			}
			private BasePoint _endBasePoint;
			public BasePoint EndPoint
			{
				get
				{
					return _endBasePoint;
				}
			}

			/// <summary>
			/// Ax + By + C = 0
			/// </summary>
			public int ComputeABC(ref double A, ref double B, ref double C, ref double _distanceDev)
			{
				if (N < 2)
				{
					return -1;
				}
				double _demx = (N*Sx2-Sx*Sx);
				double _demy = (N*Sy2-Sy*Sy);
				double a = 0.0, b = 0.0;
				if (_demx > _demy) //y = ax + b
				{
					a = (N*Sxy-Sx*Sy) / _demx;
					b = (-Sx*Sxy+Sy*Sx2) /_demx;
					double _dem = Math.Sqrt(1 + a*a);
					if (a > 0)
					{
						A = a / _dem;
						B = -1.0 / _dem;
						C = b / _dem;
					}
					else
					{
						A = -a / _dem;
						B = 1.0 / _dem;
						C = -b / _dem;
					}
				}
				else //x = ay + b;
				{
					a = (N*Sxy-Sx*Sy) / _demy;
					b = (-Sy*Sxy+Sx*Sy2) / _demy;
					double _dem = Math.Sqrt(1 + a*a);
					A = 1 / _dem;
					B = -a / _dem;
					C = -b / _dem;
				}
				_distanceDev =
					A*A*Sx2 + B*B* Sy2 + N*C*C + 2.0*A*B*Sxy + 2.0*A*C*Sx + 2.0*B*C*Sy;
				_distanceDev = Math.Sqrt(_distanceDev / N);

				return 0;
			}
			
			public int ComputeX0Y0X1Y1(ref float _x0, ref float _y0, ref float _x1, ref float _y1, ref double _distanceDev)
			{
				double A = 0D, B = 0D, C = 0D;
				if (ComputeABC(ref A, ref B, ref C, ref _distanceDev) != 0)
					return -1;
				if (Math.Abs(A) < Math.Abs(B)) //alpha < 45 degrees
				{
					_x0 = _minX;
					_y0 = (float)(- (_x0*A + C) / B);
					_beginBasePoint = m_sourceSpace[_iminX];
					_x1 = _maxX;
					_y1 = (float)(- (_x1*A + C) / B);
					_endBasePoint = m_sourceSpace[_imaxX];
				}
				else //alpha >= 45 degrees
				{
					_y0 = _minY;
					_x0 = (float)(- (_y0*B + C) / A);
					_beginBasePoint = m_sourceSpace[_iminY];
					_y1 = _maxY;
					_x1 = (float)(- (_y1*B + C) / A);
					_endBasePoint = m_sourceSpace[_imaxY];
				}
				return 0;
			}
			
			public TmpLineFeature(BasePoint[] _fSpace)
			{
				m_sourceSpace = _fSpace;
				Reset();
			}
			public void Reset()
			{
				N = 0;
				_lastAdded = -1;
				Sx2=0D;
				Sy2=0D;
				Sxy=0D;
				Sx=0D;
				Sy=0D;
				_minX = float.MaxValue;
				_minY = float.MaxValue;
				_maxX = float.MinValue;
				_maxY = float.MinValue;
			}
			public void Add(int _ip)
			{
				Add(_ip,true);
			}
			public void Add(int _ip, bool _update_last_ip)
			{			
				double _x=0D, _y=0D;
				
				_x = m_sourceSpace[_ip].X;
				_y =  m_sourceSpace[_ip].Y;

				if (_x < _minX)
				{
					_minX = (float)_x;
					_iminX = _ip;
				}
				if (_x > _maxX)
				{
					_maxX = (float)_x;
					_imaxX = _ip;
				}

				if (_y < _minY)
				{
					_minY = (float)_y;
					_iminY = _ip;
				}
				if (_y > _maxY)
				{
					_maxY = (float)_y;
					_imaxY = _ip;
				}

				Sx2 += _x*_x;
				Sy2 += _y*_y;

				Sxy += _x*_y;
				Sx += _x;
				Sy += _y;
				N++;			
				if (_update_last_ip)
					_lastAdded = _ip;
#if PRIV_TRACE
				Console.WriteLine("Added - N {0}\t i: {1} \tx: {2} \t Sx: {3}",
					N.ToString(), _ip.ToString(),_x.ToString(), Sx.ToString());
#endif
			}
			public void Subtract(int _ip)
			{
				Subtract(_ip, true);
			}
			public void Subtract(int _ip, bool _update_last_ip)
			{
				
				if (_update_last_ip && _lastAdded > 0 && _lastAdded != _ip)
					throw new System.Exception("The subtracting point and the last added one are not equal");
				if (N <= 0)
					throw new System.Exception("Please check algorithm. You are trying to subtract from empty set");
				double _x=0D, _y=0D;
				
				_x = m_sourceSpace[_ip].X;
				_y =  m_sourceSpace[_ip].Y;

				Sx2 -= _x*_x;
				Sy2 -= _y*_y;

				Sxy -= _x*_y;
				Sx -= _x;
				Sy -= _y;
				N--;			

				if (_update_last_ip)
					_lastAdded = -1;
#if PRIV_TRACE
				Console.WriteLine("Subtracted - N {0}\t i: {1} x: {2} \t Sx: {3}",
					N.ToString(), _ip.ToString(), _x.ToString(), Sx.ToString());
#endif
			}

		}
		public LineParam TopoParam;
		private TmpLineFeature _tmpLineFeature = null;
		double _A = 0.0, _B = 0.0, _C = 0.0, _distDev = 0D;
		double _d = 0.0, _x = 0D, _y = 0D;
		double [] secondXArray = null;
		double [] secondYArray = null;
		double _max_length;
		double _k_angle_error = 1.0;

		//fixed parameters
		double _L = 50000.0;
		double _very_high_ecc = 0.98;
		double _maxL2 = 50.0;
		double _minL2 = 2.0;
		//////////////////////////////////////////////////////////////////////////

		private bool _solution_added = false;
		private bool _current_estimated = false;
		public LineBackTracking(BasePoint [] _featureSpace):base(_featureSpace, TOPOLOGY.LINE)
		{
			TopoParam = new LineParam();
			_tmpLineFeature = new TmpLineFeature(m_sourceSpace);
			_max_length = float.MinValue;
			if (m_NumPoints > 0)
			{
				secondXArray = new double[m_NumPoints];
				secondYArray = new double[m_NumPoints];
				int _count = 0;
				foreach (EllipsePoint _p in m_sourceSpace)
				{
					if (_max_length < _p.Length)
						_max_length = _p.Length;
					secondXArray[_count] = (_p.X + _L * Math.Cos(_p.Orientation * _radian_mul));
					secondYArray[_count] = (_p.Y - _L * Math.Sin(_p.Orientation * _radian_mul));
					_count++;
				}
			}
			_maxL2 *= _max_length * _max_length;
			_minL2 *= _max_length * _max_length;
		}
		public LineBackTracking(BasePoint [] _featureSpace, LineParam _param, TOPO_DETECT_MODE _detectMode) : this(_featureSpace)
		{
			this.TopoParam.CopyFrom(_param);
			DetectMode = _detectMode;
			if (DetectMode == TOPO_DETECT_MODE.VERY_FEW)
				_k_angle_error *= 2.0;
		}
		protected override bool FoundCondition()
		{		
			if (!_current_estimated )
				return false;
			if (DetectMode == TOPO_DETECT_MODE.VERY_FEW &&
				_current.Count < 4)
				return false;
			if (_current.Count < TopoParam.MinNumPoints)
				return false;
			return true;
		}
		protected override bool AddSolution(ArrayList result)
		{
			bool bresult = base.AddSolution(result);
			if (bresult)
			{
				_solution_added = true;
			}
			return bresult;
		}

		protected override bool CheckAdding(int _iPoint)
		{
			if (_iPoint >= m_NumPoints)
				return false;
			int nCount = _current.Count;
			if (nCount == 0)
			{
				_solution_added = false;
				return true;
			}
			if (_solution_added)
				return false;
			double _angular_error;
			if (nCount == 1)
			{
				double _or1 = m_sourceSpace[(int)_current[0]].Orientation;
				double _ecc1 = (m_sourceSpace[(int)_current[0]] as EllipsePoint).Eccentricity;

				double _or2 = m_sourceSpace[_iPoint].Orientation;
				double _ecc2 = (m_sourceSpace[_iPoint] as EllipsePoint).Eccentricity;


				double _d2 = (m_sourceSpace[(int)_current[0]].X - m_sourceSpace[_iPoint].X)*(m_sourceSpace[(int)_current[0]].X - m_sourceSpace[_iPoint].X) +
					(m_sourceSpace[(int)_current[0]].Y - m_sourceSpace[_iPoint].Y)*(m_sourceSpace[(int)_current[0]].Y - m_sourceSpace[_iPoint].Y);
				if (_ecc1 < _very_high_ecc || _ecc2 < _very_high_ecc)
				{
					if (_d2 < _minL2 || _d2 > _maxL2)
						return false;
				}
				else
				{
					if (_d2 > _maxL2)
						return false;
				}

				_tmpLineFeature.Add(_iPoint, false);
				int _res = _tmpLineFeature.ComputeABC(ref _A, ref _B, ref _C, ref _distDev);
				_tmpLineFeature.Subtract(_iPoint, false);

				if ( _res != 0)
				{
#if DEBUG
					Console.WriteLine("Compute ABC failed.");
#endif
					return false;
				}
				_angular_error = (secondXArray[_iPoint] * _A + secondYArray[_iPoint] * _B + _C) / _L;
				if (Math.Abs(_angular_error) > TopoParam.MaxAngularError *_k_angle_error)
					return false;

				_angular_error =( secondXArray[(int)_current[0]] * _A + secondYArray[(int)_current[0]] * _B + _C) / _L;
				if (Math.Abs(_angular_error) > TopoParam.MaxAngularError *_k_angle_error)
					return false;
				
				return true;
			}
			//nCount >= 2
			if (!_current_estimated)
			{
#if DEBUG
				Console.WriteLine("Please check algorithm, current feature is not estimated.");
#endif
				return false;
			}
			_x = m_sourceSpace[_iPoint].X;
			_y = m_sourceSpace[_iPoint].Y;
			_d = _A * _x + _B * _y + _C;
			if (DetectMode == TOPO_DETECT_MODE.VERY_FEW) 
			{
				if (Math.Abs(_d) >= 
					(TopoParam.MaxDistantError +  _distDev + (m_sourceSpace[_iPoint] as EllipsePoint).Minor))
					return false;
			}
			if (Math.Abs(_d) > (TopoParam.MaxDistantError))
				return false;
			_angular_error = secondXArray[_iPoint] * _A + secondYArray[_iPoint] * _B + _C;
			_angular_error = (_angular_error - _d) / _L;
			if (Math.Abs(_angular_error) > TopoParam.MaxAngularError * _k_angle_error)
				return false;

			return true;
		}
		protected override void Push(int _iPoint)
		{
			base.Push(_iPoint);
			_tmpLineFeature.Add(_iPoint);
			_current_estimated = 
				_tmpLineFeature.ComputeABC(ref _A, ref _B, ref _C,  ref _distDev) == 0;
		}
		protected override int Pop()
		{
			int result = base.Pop();
			_tmpLineFeature.Subtract(result);
			_current_estimated = 
				_tmpLineFeature.ComputeABC(ref _A, ref _B, ref _C, ref _distDev) == 0;
			return result;
		}

		public int GetLineFeature(ArrayList _solution, ref LineFeature _line_feature)
		{
			if (_solution == null || _solution.Count < 2 || 
				_solution[0].GetType() != typeof(int) || _line_feature == null)
				return -1;
			TmpLineFeature _tmp = new TmpLineFeature(m_sourceSpace);
			foreach (int _iP in _solution)
			{
				_tmp.Add(_iP);
			}
			float X0 = 0f, Y0 = 0f, X1 = 0f, Y1 = 0f;
			double _distanceDev = 0D;
			_tmp.ComputeX0Y0X1Y1(ref X0, ref Y0, ref X1, ref Y1, ref _distanceDev);
			_line_feature.Update(X0, Y0, X1, Y1);
			_line_feature.DistanceStd = (float)_distanceDev;
			if (_line_feature.BasePoints == null)
				_line_feature.BasePoints = new ArrayList(_solution.Count);
			else
			{
				_line_feature.BasePoints.Clear();
				_line_feature.BasePoints.Capacity = _solution.Count;
			}

			double _maxMinor = double.MinValue, _avrMinor = 0.0;
#if DEBUG
			Console.WriteLine("X\t Y \t O");
#endif
			foreach (int _iP in _solution)
			{
				if (((EllipsePoint)m_sourceSpace[_iP]).Minor > _maxMinor)
					_maxMinor = ((EllipsePoint)m_sourceSpace[_iP]).Minor;
				_avrMinor += ((EllipsePoint)m_sourceSpace[_iP]).Minor;
				_line_feature.BasePoints.Add(m_sourceSpace[_iP]);
#if DEBUG
				Console.WriteLine("{0}\t {1} \t {2}",
					((EllipsePoint)m_sourceSpace[_iP]).X.ToString(),
					((EllipsePoint)m_sourceSpace[_iP]).Y.ToString(),
					((EllipsePoint)m_sourceSpace[_iP]).Orientation.ToString());
#endif
			}
			_avrMinor /= _solution.Count;
			_line_feature.Width = (float)(_avrMinor+_distanceDev*3.0);
			//_line_feature.Width = (float)(_maxMinor+_distanceDev*1.0);
			_line_feature.BasePoints.TrimToSize();
			return 0;
		}
		public EllipsePoint GetPoint(int _iPoint)
		{
			if (_iPoint > m_NumPoints || _iPoint < 0)
				return null;
			return (EllipsePoint)m_sourceSpace[_iPoint];
		}

	}
#endregion

	#region ArcBackTracking
	public class ArcBackTracking : TopologyBackTracking
	{
		private class TmpArcFeature
		{
			private BasePoint[] m_sourceSpace = null;
			private int N;
			public int NumPoints
			{
				get
				{
					return N;
				}
			}
			private int _lastAdded;
			public int LastAdded
			{
				get
				{
					return _lastAdded;
				}
			}
			public double Sx3=0D; 
			public double Sy3=0D;
			public double Sx2=0D;
			public double Sy2=0D;
			public double Sx2y=0D;
			public double Sy2x=0D;
			public double Sxy=0D;
			public double Sx=0D;
			public double Sy=0D;

			public double X0
			{
				get
				{
					return (	0.5*(N*Sxy*Sy3-N*Sy2x*Sy2-N*Sx3*Sy2+N*Sxy*Sx2y+Sx2*Sx*Sy2-Sx2*Sxy*Sy+Sx*Sy2*Sy2-Sy*Sx*Sx2y-Sxy*Sy*Sy2+Sy*Sy*Sx3-Sy*Sx*Sy3+Sy*Sy*Sy2x)) /
						(-2*Sy*Sx*Sxy+Sxy*Sxy*N+Sy*Sy*Sx2-Sy2*N*Sx2+Sy2*Sx*Sx);
				}
			}
			public double Y0
			{
				get
				{
					return (-0.5*(Sy*Sx*Sx3+Sy*Sx*Sy2x+Sxy*Sx*Sx2+Sxy*Sx*Sy2-Sxy*N*Sx3-Sxy*N*Sy2x-Sy*Sx2*Sx2-Sy*Sy2*Sx2+Sx2y*N*Sx2-Sx2y*Sx*Sx+Sy3*N*Sx2-Sy3*Sx*Sx))/
						(-2*Sy*Sx*Sxy+Sxy*Sxy*N+Sy*Sy*Sx2-Sy2*N*Sx2+Sy2*Sx*Sx);
				}
			}
			public double R
			{
				get
				{
					double MapleGenVar5 = 4.0*Sx2*Sxy*Sxy*Sxy*Sxy*N+5.0*Sxy*Sxy*Sx*Sx*Sy2*Sy2+Sxy*Sxy*Sx*Sx*Sx2*Sx2+Sxy*Sxy*N*N*Sx3*Sx3+Sxy*Sxy*N*N*Sy2x*Sy2x+Sy*Sy*Sx*Sx*Sy2x*Sy2x+Sy*Sy*Sx*Sx*Sx3*Sx3+2.0*N*N*Sx3*Sy2*Sy2*Sy2x-2.0*N*Sx3*Sy2*Sy2*Sy2*Sx-2.0*N*Sx3*Sx3*Sy2*Sy*Sy-2.0*N*Sy2x*Sy2*Sy2*Sy2*Sx-2.0*N*Sy2x*Sy2x*Sy2*Sy*Sy+2.0*N*N*Sxy*Sxy*Sx2y*Sy3-6.0*Sxy*Sy*Sy*Sy*Sx2*Sy2x-6.0*Sxy*Sy*Sy*Sy*Sx2*Sx3+6.0*Sxy*Sxy*Sy*Sy*Sx2*Sy2-2.0*Sx*Sx*Sy2*Sy2*Sy*Sx2y-2.0*Sx*Sx*Sy2*Sy2*Sy*Sy3+2.0*Sx*Sy2*Sy2*Sy2x*Sy*Sy+2.0*Sx*Sy2*Sy2*Sx3*Sy*Sy-2.0*Sx*Sy2*Sy2*Sy2*Sxy*Sy+2.0*Sy*Sy*Sx*Sx*Sx2y*Sy3-2.0*Sy*Sy*Sy*Sx*Sx2y*Sy2x-2.0*Sy*Sy*Sy*Sx*Sx2y*Sx3-2.0*Sy*Sy*Sy*Sx*Sy3*Sy2x-2.0*Sy*Sy*Sy*Sx*Sy3*Sx3-2.0*Sy2x*Sy*Sy*Sy*Sxy*Sy2+Sx*Sx*Sy2*Sy2*Sy2*Sy2+Sy2x*Sy2x*Sy*Sy*Sy*Sy+Sx3*Sx3*Sy*Sy*Sy*Sy+N*N*Sx3*Sx3*Sy2*Sy2+N*N*Sy2x*Sy2x*Sy2*Sy2+N*N*Sxy*Sxy*Sx2y*Sx2y+N*N*Sxy*Sxy*Sy3*Sy3-3.0*Sx2*Sx2*Sx*Sx*Sy2*Sy2-2.0*Sx2*Sx*Sx*Sy2*Sy2*Sy2+5.0*Sxy*Sxy*Sy*Sy*Sx2*Sx2+Sy*Sy*Sx*Sx*Sx2y*Sx2y+Sy*Sy*Sx*Sx*Sy3*Sy3+2.0*Sy2x*Sy*Sy*Sy*Sy*Sx3;
					double MapleGenVar6 = MapleGenVar5+Sxy*Sxy*Sy*Sy*Sy2*Sy2-2.0*Sy*Sy*Sx2*Sx2*Sx2*Sy2-3.0*Sy*Sy*Sy2*Sy2*Sx2*Sx2+Sx2y*Sx2y*N*N*Sx2*Sx2+2.0*Sx2y*Sx*Sx*Sx*Sx*Sy3+Sy3*Sy3*N*N*Sx2*Sx2+4.0*Sy2*Sy2*Sy2*N*Sx2*Sx2+4.0*Sy2*Sxy*Sxy*Sxy*Sxy*N+4.0*Sy2*Sy2*N*Sx2*Sx2*Sx2-2.0*N*N*Sx3*Sy2*Sxy*Sx2y-2.0*N*N*Sx3*Sy2*Sxy*Sy3-6.0*N*Sx3*Sy2*Sy2*Sx2*Sx+8.0*N*Sx3*Sy2*Sxy*Sy*Sx2+2.0*N*Sx3*Sy2*Sy*Sx*Sx2y+2.0*N*Sx3*Sy2*Sy*Sx*Sy3-4.0*N*Sx3*Sy2*Sy2x*Sy*Sy+2.0*N*Sx3*Sy2*Sy2*Sxy*Sy-2.0*N*N*Sy2x*Sy2*Sxy*Sx2y-2.0*N*N*Sy2x*Sy2*Sxy*Sy3;
					double MapleGenVar4 = MapleGenVar6-6.0*N*Sy2x*Sy2*Sy2*Sx2*Sx+8.0*N*Sy2x*Sy2*Sxy*Sy*Sx2+2.0*N*Sy2x*Sy2*Sy*Sx*Sx2y+2.0*N*Sy2x*Sy2*Sy*Sx*Sy3+2.0*N*Sy2x*Sy2*Sy2*Sxy*Sy+8.0*N*Sxy*Sx2y*Sx2*Sx*Sy2+2.0*N*Sxy*Sxy*Sx2y*Sy*Sx2+2.0*N*Sxy*Sx2y*Sx*Sy2*Sy2-2.0*N*Sxy*Sx2y*Sx2y*Sy*Sx-4.0*N*Sxy*Sx2y*Sy*Sx*Sy3+2.0*N*Sxy*Sx2y*Sy2x*Sy*Sy+2.0*N*Sxy*Sx2y*Sx3*Sy*Sy-2.0*N*Sxy*Sxy*Sx2y*Sy*Sy2+8.0*N*Sxy*Sy3*Sx2*Sx*Sy2+2.0*N*Sxy*Sxy*Sy3*Sy*Sx2+2.0*N*Sxy*Sy3*Sx*Sy2*Sy2-2.0*N*Sxy*Sy3*Sy3*Sy*Sx+2.0*N*Sxy*Sy3*Sy2x*Sy*Sy+2.0*N*Sxy*Sy3*Sx3*Sy*Sy-2.0*N*Sxy*Sxy*Sy3*Sy*Sy2+2.0*Sx2*Sx2*Sx*Sy2*Sxy*Sy;  
					MapleGenVar6 = MapleGenVar4+4.0*Sx2*Sx*Sx*Sy2*Sy*Sx2y+4.0*Sx2*Sx*Sx*Sy2*Sy*Sy3+4.0*Sx2*Sx*Sy2*Sy2x*Sy*Sy+4.0*Sx2*Sx*Sy2*Sx3*Sy*Sy+2.0*Sx2*Sx*Sy2*Sy2*Sxy*Sy-10.0*Sxy*Sy*Sy*Sx2*Sx*Sx2y-10.0*Sxy*Sy*Sy*Sx2*Sx*Sy3+2.0*Sy*Sy*Sx*Sx2y*Sxy*Sy2+2.0*Sy*Sy*Sx*Sy3*Sxy*Sy2-2.0*Sx3*Sy*Sy*Sy*Sxy*Sy2+Sy*Sy*Sx2*Sx2*Sx2*Sx2+Sx2y*Sx2y*Sx*Sx*Sx*Sx+Sy3*Sy3*Sx*Sx*Sx*Sx+2.0*Sy*Sy*Sx*Sx*Sx3*Sy2x-2.0*Sy*Sy*Sx*Sx3*Sx2*Sx2-2.0*Sy*Sx*Sx*Sx*Sx3*Sx2y-2.0*Sy*Sx*Sx*Sx*Sx3*Sy3-2.0*Sy*Sy*Sx*Sy2x*Sx2*Sx2-2.0*Sy*Sx*Sx*Sx*Sy2x*Sx2y;      
					MapleGenVar5 = MapleGenVar6-2.0*Sy*Sx*Sx*Sx*Sy2x*Sy3+6.0*Sxy*Sxy*Sx*Sx*Sx2*Sy2-2.0*Sxy*Sx*Sx2*Sx2*Sx2*Sy-2.0*Sxy*Sx*Sx*Sx*Sx2*Sx2y-2.0*Sxy*Sx*Sx*Sx*Sx2*Sy3-6.0*Sxy*Sx*Sx*Sx*Sy2*Sx2y-6.0*Sxy*Sx*Sx*Sx*Sy2*Sy3+2.0*Sxy*Sxy*N*N*Sx3*Sy2x-2.0*Sy*Sx2*Sx2*Sx2*Sx2y*N+2.0*Sy*Sx2*Sx2*Sx2y*Sx*Sx-2.0*Sy*Sx2*Sx2*Sx2*Sy3*N+2.0*Sy*Sx2*Sx2*Sy3*Sx*Sx-2.0*Sx2y*Sx2y*N*Sx2*Sx*Sx+2.0*Sx2y*N*N*Sx2*Sx2*Sy3-2.0*Sy3*Sy3*N*Sx2*Sx*Sx+2.0*Sy*Sx*Sx*Sx3*Sxy*Sx2-10.0*Sy*Sx*Sx*Sx3*Sxy*Sy2-2.0*Sy*Sx*Sx3*Sx3*Sxy*N-4.0*Sy*Sx*Sx3*Sxy*N*Sy2x+2.0*Sy*Sx*Sx3*Sx2y*N*Sx2;      
					MapleGenVar6 = MapleGenVar5+2.0*Sy*Sx*Sx3*Sy3*N*Sx2+2.0*Sy*Sx*Sx*Sy2x*Sxy*Sx2-10.0*Sy*Sx*Sx*Sy2x*Sxy*Sy2-2.0*Sy*Sx*Sy2x*Sy2x*Sxy*N+2.0*Sy*Sx*Sy2x*Sx2y*N*Sx2+2.0*Sy*Sx*Sy2x*Sy3*N*Sx2-2.0*Sxy*Sxy*Sx*Sx2*N*Sx3-2.0*Sxy*Sxy*Sx*Sx2*N*Sy2x+2.0*Sxy*Sx*Sx2*Sx2*Sx2y*N+2.0*Sxy*Sx*Sx2*Sx2*Sy3*N+2.0*Sxy*Sxy*Sx*Sy2*N*Sx3+2.0*Sxy*Sxy*Sx*Sy2*N*Sy2x+2.0*Sxy*N*Sx3*Sy*Sx2*Sx2-2.0*Sxy*N*N*Sx3*Sx2y*Sx2+2.0*Sxy*N*Sx3*Sx2y*Sx*Sx-2.0*Sxy*N*N*Sx3*Sy3*Sx2+2.0*Sxy*N*Sx3*Sy3*Sx*Sx+2.0*Sxy*N*Sy2x*Sy*Sx2*Sx2-2.0*Sxy*N*N*Sy2x*Sx2y*Sx2+2.0*Sxy*N*Sy2x*Sx2y*Sx*Sx;
					double MapleGenVar3 = MapleGenVar6-2.0*Sxy*N*N*Sy2x*Sy3*Sx2+2.0*Sxy*N*Sy2x*Sy3*Sx*Sx-6.0*Sy*Sy2*Sx2*Sx2*Sx2y*N-6.0*Sy*Sy2*Sx2*Sx2*Sy3*N-4.0*Sx2y*N*Sx2*Sy3*Sx*Sx+4.0*Sx3*Sy2*Sy2*Sx*Sx*Sx+4.0*Sy2x*Sy2*Sy2*Sx*Sx*Sx-4.0*Sx*N*Sxy*Sxy*Sxy*Sx2y-4.0*Sx*N*Sxy*Sxy*Sxy*Sy3-8.0*Sxy*Sxy*N*Sy2*Sx2*Sx2-8.0*Sxy*Sxy*N*Sy2*Sy2*Sx2+4.0*Sx2y*Sx2*Sx2*Sy*Sy*Sy+4.0*Sy3*Sx2*Sx2*Sy*Sy*Sy-4.0*Sy*Sxy*Sxy*Sxy*N*Sx3-4.0*Sy*Sxy*Sxy*Sxy*N*Sy2x+8.0*Sxy*Sxy*Sx2y*Sy*Sx*Sx+8.0*Sxy*Sxy*Sy3*Sy*Sx*Sx-8.0*Sx*Sxy*Sxy*Sxy*Sy*Sx2+8.0*Sx*Sy2x*Sy*Sy*Sxy*Sxy+8.0*Sx*Sx3*Sy*Sy*Sxy*Sxy-8.0*Sx*Sxy*Sxy*Sxy*Sy*Sy2;
					MapleGenVar4 = 1/(2.0*Sy*Sx*Sxy-Sxy*Sxy*N-Sy*Sy*Sx2+Sy2*N*Sx2-Sy2*Sx*Sx);      
					double MapleGenVar2 = Math.Sqrt(MapleGenVar3)*MapleGenVar4;    
					double t0 = 0.5*MapleGenVar2;
					return t0;
				}
			}
			public double GetR4inR()
			{
				double a = X0;
				double b = Y0;
				double R2 = R*R;
				double R4 = -2.0*Sy2*R2+4.0*a*Sx*R2-4.0*a*Sx*b*b+8.0*a*Sxy*b-2.0*N*a*a*R2-2.0*N*b*b*R2+2.0*N*a*a*b*b+
					4.0*Sy*b*R2-4.0*a*a*Sy*b-4.0*Sx2y*b+2.0*Sx2y*Sx2y+2.0*a*a*Sy2+6.0*Sy2*b*b+Sx*Sx*Sx*Sx-4.0*a*a*a*Sx+
					N*a*a*a*a+N*b*b*b*b+N*R2*R2-4.0*a*Sx3+6.0*a*a*Sx2+2.0*Sx2*b*b-2.0*Sx2*R2-4.0*Sy*b*b*b+
					Sy*Sy*Sy*Sy-4.0*Sy3*b-4.0*a*Sy2x;
				return Math.Sqrt(Math.Sqrt(R4/N));
			}
			public TmpArcFeature(BasePoint[] _fSpace)
			{
				m_sourceSpace = _fSpace;
				Reset();
			}
			public void Reset()
			{
				N = 0;
				_lastAdded = -1;
				Sx3=0D; 
				Sy3=0D;
				Sx2=0D;
				Sy2=0D;
				Sx2y=0D;
				Sy2x=0D;
				Sxy=0D;
				Sx=0D;
				Sy=0D;
			}
			public void Add(double _x, double _y)
			{
				double x3=0D, x2=0D, y3=0D, y2=0D;
				
				x2 = _x*_x;
				x3 = x2*_x;

				y2 = _y*_y;
				y3 = y2*_y;

				Sx3 += x3;
				Sy3 += y3;

				Sx2 += x2;
				Sy2 += y2;

				Sx2y += x2*_y;
				Sy2x += y2*_x;

				Sxy += _x*_y;
				Sx += _x;
				Sy += _y;
				N++;
#if PRIV_TRACE
				Console.WriteLine("Added - N {0}\t i: {1} \tx: {2} \t Sx: {3}",
					N.ToString(), _ip.ToString(),_x.ToString(), Sx.ToString());
#endif
			}
			public void Add(int _ip)
			{
				Add(_ip,true);
			}
			public void Add(int _ip, bool _update_last_ip)
			{
				Add(_ip, _update_last_ip, 0.0, 0.0);
			}
			public void Add(int _ip, bool _update_last_ip, double _mx, double _my)
			{			
				double _x=0D, _y=0D, x3=0D, x2=0D, y3=0D, y2=0D;
				
				_x = m_sourceSpace[_ip].X - _mx;
				_y =  m_sourceSpace[_ip].Y - _my;

				x2 = _x*_x;
				x3 = x2*_x;

				y2 = _y*_y;
				y3 = y2*_y;

				Sx3 += x3;
				Sy3 += y3;

				Sx2 += x2;
				Sy2 += y2;

				Sx2y += x2*_y;
				Sy2x += y2*_x;

				Sxy += _x*_y;
				Sx += _x;
				Sy += _y;
				N++;
				if (_update_last_ip)
					_lastAdded = _ip;
#if PRIV_TRACE
				Console.WriteLine("Added - N {0}\t i: {1} \tx: {2} \t Sx: {3}",
					N.ToString(), _ip.ToString(),_x.ToString(), Sx.ToString());
#endif
			}
			public void Subtract(int _ip)
			{
				Subtract(_ip, true);
			}
			public void Subtract(int _ip, bool _update_last_ip)
			{
				
				if (_update_last_ip && _lastAdded > 0 && _lastAdded != _ip)
					throw new System.Exception("The subtracting point and the last added one are not equal");
				if (N <= 0)
					throw new System.Exception("Please check algorithm. You are trying to subtract from empty set");
				double _x=0D, _y=0D, x3=0D, x2=0D, y3=0D, y2=0D;
				
				_x = m_sourceSpace[_ip].X;
				_y =  m_sourceSpace[_ip].Y;

				x2 = _x*_x;
				x3 = x2*_x;

				y2 = _y*_y;
				y3 = y2*_y;

				Sx3 -= x3;
				Sy3 -= y3;

				Sx2 -= x2;
				Sy2 -= y2;

				Sx2y -= x2*_y;
				Sy2x -= y2*_x;

				Sxy -= _x*_y;
				Sx -= _x;
				Sy -= _y;

				N--;
				if (_update_last_ip)
					_lastAdded = -1;
#if PRIV_TRACE
				Console.WriteLine("Subtracted - N {0}\t i: {1} x: {2} \t Sx: {3}",
					N.ToString(), _ip.ToString(), _x.ToString(), Sx.ToString());
#endif
			}
		}
		private ArcParam topoParam;
		#region Param Mapping
		float TopoParam_FirstPercentageMinEcc;
		float TopoParam_FirstPercentageMinPoints;
		float TopoParam_FirstPercentageValue;
		float TopoParam_InitPoints;
		float TopoParam_MaxAddingRadiusErrorRatio;
		float TopoParam_MaxAddingTanResidualStd;
		float TopoParam_MaxFoundRadiusErrorRatio;
		float TopoParam_MaxFoundTanResidualStd;
		float TopoParam_MaxRadius;
		float TopoParam_MinNumPoints;
		float TopoParam_MinRadius;
		
		#endregion Param Mapping
		private TmpArcFeature _tmpArcFeature;
		private ArcFeature _EstimatedArcFeature;
		private ArcFeature currentArcFeature;
		private bool _ArcFeatureEstimated = false;
		private bool currentArcEstimated = false;
		private bool veryFewDetected = false;
		private bool arcVeryThinDetected = false;
		private bool sparseModeDetected = false;

		private float maxLengthLimited = 0f;

		float[] secondXArray = null;
		float[] secondYArray = null;

		//fixed parameters
		const double _degree_mul = 180.0/Math.PI;
		double lConstant = double.NaN;
		double _very_high_ecc = double.NaN;
		double _max_dis2 = double.NaN;
		double _min_dis2 = double.NaN;
		double veryFewKTan = double.NaN;
		double _R_fixed_error = double.NaN;

		//////////////////////////////////////////////////////////////////////////

		private bool _solution_added = false;

#if ARC_BEST
		public ArrayList _best = null;
		float _tan_res_std_min = 0f;
		public ArcFeature _best_feature;
#endif

		#region CopyParamToInternal()
		private void CopyParamToInternal()
		{
			TopoParam_FirstPercentageMinEcc = topoParam.FirstPercentageMinEcc;
			TopoParam_FirstPercentageMinPoints = topoParam.FirstPercentageMinPoints;
			TopoParam_FirstPercentageValue = topoParam.FirstPercentageValue;
			TopoParam_InitPoints = topoParam.InitPoints;
			TopoParam_MaxAddingRadiusErrorRatio = topoParam.MaxAddingRadiusErrorRatio;
			TopoParam_MaxAddingTanResidualStd = topoParam.MaxAddingTanResidualStd;
			TopoParam_MaxFoundRadiusErrorRatio = topoParam.MaxFoundRadiusErrorRatio;
			TopoParam_MaxFoundTanResidualStd = topoParam.MaxFoundTanResidualStd;
			TopoParam_MaxRadius = topoParam.MaxRadius;
			TopoParam_MinNumPoints = topoParam.MinNumPoints;
			TopoParam_MinRadius = topoParam.MinRadius;

			lConstant = topoParam.LConstant;
			_very_high_ecc = topoParam.VeryHighEcc;
			_max_dis2 = topoParam.MaxDis2Ratio;
			_min_dis2 = topoParam.MinDis2Ratio;
			veryFewKTan = topoParam.VeryFewKTan;
			_R_fixed_error = topoParam.RFixedError;
		}
		#endregion

		#region Constructors
		private ArcBackTracking(BasePoint[] _featureSpace):this(_featureSpace, null, TOPO_DETECT_MODE.NORMAL){}	
		public ArcBackTracking(BasePoint[] _featureSpace, ArcParam _param, TOPO_DETECT_MODE _detectMode) : base(_featureSpace,TOPOLOGY.ARC)
		{
			this.topoParam = new ArcParam();
			if (_param != null)
				this.topoParam.CopyFrom(_param);

			CopyParamToInternal();

			_tmpArcFeature = new TmpArcFeature(this.m_sourceSpace);
			_EstimatedArcFeature = new ArcFeature();
			currentArcFeature = new ArcFeature();

 #if ARC_BEST
			_best = new ArrayList();
			_tan_res_std_min = float.MaxValue;
			_best_feature = new ArcFeature();
#endif

			maxLengthLimited = float.MinValue;
			if (m_NumPoints > 0)
			{
				secondXArray = new float[m_NumPoints];
				secondYArray = new float[m_NumPoints];
				int _count = 0;
				foreach (EllipsePoint _p in m_sourceSpace)
				{
					if (maxLengthLimited < _p.Length)
						maxLengthLimited = _p.Length;
					secondXArray[_count] = (float)(_p.X + lConstant * Math.Cos(_p.Orientation / 180.0 * Math.PI));
					secondYArray[_count] = (float)(_p.Y - lConstant * Math.Sin(_p.Orientation / 180.0 * Math.PI));
					_count++;
				}
			}
			_max_dis2 *= maxLengthLimited * maxLengthLimited;
			_min_dis2 *= maxLengthLimited * maxLengthLimited;

			DetectMode = _detectMode;
			sparseModeDetected =( DetectMode &  TOPO_DETECT_MODE.SPARSE) != TOPO_DETECT_MODE.NONE;
			if (sparseModeDetected)
			{
				_max_dis2 = 8.0 * maxLengthLimited * maxLengthLimited;
				_min_dis2 = 0.0;
				return;
			}
			if (_detectMode == TOPO_DETECT_MODE.AUTO)
			{
				if (m_NumPoints < TopoParam_MinNumPoints * 3 )
				{
					_max_dis2 = double.MaxValue; // suppress checking L_min L_max if number of points is small
					_min_dis2 = _min_dis2 / 3.0;
				}
				if (m_NumPoints <= TopoParam_MinNumPoints * 1.5)
				{
					veryFewDetected = true;
				}
			}		
			else
			{
				if (DetectMode == TOPO_DETECT_MODE.FEW)
				{
					//					_max_dis2 = double.MaxValue; // suppress checking L_min L_max if number of points is small
					//					_min_dis2 = _min_dis2 / 3.0;
				}
				if (DetectMode == TOPO_DETECT_MODE.VERY_FEW)
				{
					veryFewDetected = true;					
				}
			}			
		}
		public ArcBackTracking(BasePoint[] _featureSpace, ArcParam _param) : 
			this(_featureSpace, _param, TOPO_DETECT_MODE.AUTO){}
		#endregion

		#region GetPoint(int _iPoint)
		public EllipsePoint GetPoint(int _iPoint)
		{
			if (_iPoint > m_NumPoints || _iPoint < 0)
				return null;
			return (EllipsePoint)m_sourceSpace[_iPoint];
		}

		#endregion

		#region GetArcFeature(ArrayList _points, ArcFeature result)

        public static double WaferRadius = 150000;

		public int GetArcFeature(ArrayList _points, ArcFeature result)
		{
			if (sparseModeDetected)
			{
				if (_points == null || _points.Count < 2 || _points[0].GetType() != typeof(int) || result == null)
					return -1;
			}
			else
			{
				if (_points == null || _points.Count < 3 || _points[0].GetType() != typeof(int) || result == null)
					return -1;
			}

			TmpArcFeature _tmp = new TmpArcFeature(m_sourceSpace);
			double _mx = 0.0;
			double _my = 0.0;
			foreach (int _ip in _points)
			{
				_mx += m_sourceSpace[_ip].X;
				_my += m_sourceSpace[_ip].Y;
			}
			_mx /= _points.Count;
			_my /= _points.Count;

			EllipsePoint _el = null;
			if (!sparseModeDetected || _points.Count > 2)
			{
				foreach (int _ip in _points)
					_tmp.Add(_ip, false,_mx, _my);
			}
			else //sparse mode
			{
				_el = null;
				double _c = 0.0, _dx = 0.0, _dy = 0.0;
				foreach (int _ip in _points)
				{
					_el = (EllipsePoint)m_sourceSpace[_ip];
					_c = Math.Sqrt(_el.Length * _el.Length - _el.Minor * _el.Minor) /2;
					_dx = _c * Math.Cos(-_el.Orientation * Math.PI / 180.0);
					_dy = _c * Math.Sin(-_el.Orientation * Math.PI / 180.0);
					_tmp.Add((double)_el.X - _dx - _mx, (double)_el.Y - _dy - _my);
					_tmp.Add((double)_el.X + _dx - _mx, (double)_el.Y + _dy - _my);
					_tmp.Add((double)_el.X  - _mx, (double)_el.Y - _my);
				}
			}

			double X0 = _tmp.X0 + _mx;
			double Y0 = _tmp.Y0 + _my;
			double R = _tmp.R;

			if (double.IsNaN(X0) || double.IsNaN(Y0) || double.IsNaN(R) ||
				double.IsInfinity(X0) || double.IsInfinity(Y0) || double.IsInfinity(R))
			{
#if DEBUG
				Console.WriteLine("Getting arc feature failed");
#endif
				return -1;
			}

			double sumL2R2 = 0.0, doubleLR = 0.0, 
				secondX = 0.0, secondY = 0.0;

			double SR2 = 0D,_r = 0D;
			double _cos_residual_sum = 0D, _cos_residual_s2 = 0D;
			double _cos_tmp = 0D;

			double _x = 0D, _y = 0D;
			int N = _points.Count;
			double _max_major = 0.0;
			double _max_minor = 0.0;
			double _avr_minor = 0.0;
			EllipsePoint _begin = null,_end = null;
			if (result.BasePoints == null)
				result.BasePoints = new ArrayList(_points.Count);
			else
				result.BasePoints.Clear();

			//bool _local_arc_very_thin = true;
			foreach (int _ip in _points)
			{
				_el = (EllipsePoint)m_sourceSpace[_ip];
				result.BasePoints.Add(_el);
				if (_el.Eccentricity < _very_high_ecc)
					arcVeryThinDetected = false;

				if (_el.Length > _max_major)
					_max_major = _el.Length;
				if (!float.IsNaN(_el.Minor))
				{
					if (_el.Minor > _max_minor)
						_max_minor = _el.Minor;
					_avr_minor += _el.Minor;
				}
				_x = _el.X;
				_y =  _el.Y;

				_r = Math.Sqrt((_x - X0)*(_x - X0) + (_y - Y0)*(_y - Y0));
				SR2 += (_r-R)*(_r-R);

				sumL2R2 = lConstant*lConstant + _r*_r;
				doubleLR = 2.0*lConstant*_r;

				secondX = secondXArray[_ip] - X0;
				secondY = secondYArray[_ip] - Y0;
				_cos_tmp =  (sumL2R2 - secondX*secondX - secondY*secondY)/doubleLR;

				_cos_residual_sum += _cos_tmp;
				_cos_residual_s2 += _cos_tmp * _cos_tmp;
			}
			_max_major /= 2.0;
			_avr_minor /= _points.Count;
			_max_minor /= 2;
			if (_max_minor <= 0f)
				_max_minor = 10f;
			if (_avr_minor <= 0f)
				_avr_minor = 10f;

			result.X = (float)(X0);
			result.Y = (float)(Y0);
			result.Radius = (float)(R);
			result.RadiusError = (float)(Math.Sqrt(SR2/N));

			result.TangentialResidualMean = (float)(_cos_residual_sum / N);
			result.TangentialResidualStd = (float)
				(Math.Sqrt(_cos_residual_s2/N - result.TangentialResidualMean*result.TangentialResidualMean));
			if (sparseModeDetected)
			{
				result.Width = (float)
					(_max_minor + _max_major*(Math.Abs(result.TangentialResidualMean)+1.0*result.TangentialResidualStd) +
					result.Radius * TopoParam_MaxAddingRadiusErrorRatio);
			}
			else 
			{
				result.Width = (float)
					(_avr_minor + _max_major*(Math.Abs(result.TangentialResidualMean)+1.0*result.TangentialResidualStd));
				//			result.Width = (float)
				//				(_max_minor + _max_major*(Math.Abs(result.TangentialResidualMean)+1.0*result.TangentialResidualStd));
			}


			#region Estimate StartAngle and SweepAngle
			if (!sparseModeDetected || _points.Count > 2)
			{
				EllipsePoint _p = null;
				double _dis = 0.0, _min_dis = double.MaxValue, _max_dis = double.MinValue;
				foreach (int _iP in _points)
				{
					_p = (EllipsePoint)m_sourceSpace[_iP];
					//dis = (A-m_x)*(y-B) - (B-m_y)*(x-A);
					_dis = (result.X - _mx)*(_p.Y - result.Y) - (result.Y - _my)*(_p.X - result.X);
					if (_dis < _min_dis)
					{
						_min_dis = _dis;
						_begin = _p;
					}
					if (_dis > _max_dis)
					{
						_max_dis = _dis;
						_end = _p;
					}
				}
				double _startAngle = (Math.Atan2(_begin.Y - result.Y,_begin.X - result.X) * _degree_mul);
				double _endAngle = (Math.Atan2(_end.Y - result.Y,_end.X - result.X) * _degree_mul);
				if (_startAngle < 0.0)
					_startAngle += 360.0;
				if (_endAngle < 0.0)
					_endAngle += 360.0;
				int _i_test = 0;
				while (m_sourceSpace[(int)_points[_i_test]] == _begin || 
					m_sourceSpace[(int)_points[_i_test]] == _end)
					_i_test++;
				EllipsePoint _test_point = (EllipsePoint)m_sourceSpace[(int)_points[_i_test]];
				double _test_angle = (Math.Atan2(_test_point.Y - result.Y,_test_point.X - result.X) * _degree_mul);
				if (_test_angle< 0.0)
					_test_angle += 360.0;
			
				if (_startAngle > _endAngle)
				{
					double _temp = _endAngle;
					_endAngle = _startAngle;
					_startAngle = _temp;

					//swap the ends of arc
					_test_point = _end;
					_end = _begin;
					_begin = _test_point;
					//////////////////////////////////////////////////////////////////////////
				
				}
				if (_test_angle < _startAngle || _test_angle > _endAngle)
				{
					result.StartAngle = (float)_endAngle;
					result.SweepAngle = (float)(360.0 - _endAngle + _startAngle);
					result.BeginPoint = _end;
					result.EndPoint = _begin;
				}
				else
				{
					result.StartAngle = (float)_startAngle;
					result.SweepAngle = (float)(_endAngle - _startAngle);
					result.BeginPoint = _begin;
					result.EndPoint = _end;
				}
				// define edges
				double y0 = result.Y, x0 = result.X;
                double R1_2 = result.Radius, R_2 = WaferRadius;
				R1_2 *= R1_2;
				R_2 *= R_2;
				double ss = -y0*y0*y0*y0+2.0*R_2*x0*x0+2.0*R_2*R1_2-R1_2*R1_2+2.0*x0*x0*R1_2-R_2*R_2+
					2.0*R_2*y0*y0-x0*x0*x0*x0+2.0*y0*y0*R1_2-2.0*x0*x0*y0*y0;
				double _as = -1.0;
				if (ss > 0.0)
				{
					ss = Math.Sqrt(ss);
				
					double E1x = (x0*y0*y0-y0*ss+R_2*x0+x0*x0*x0-x0*R1_2)/(x0*x0+y0*y0)/2.0;
					double E2x = (x0*y0*y0+y0*ss+R_2*x0+x0*x0*x0-x0*R1_2)/(x0*x0+y0*y0)/2.0;

					double E1y = (R_2*y0-y0*R1_2+y0*y0*y0+x0*x0*y0+x0*ss)/(x0*x0+y0*y0)/2.0;
					double E2y = (R_2*y0-y0*R1_2+y0*y0*y0+x0*x0*y0-x0*ss)/(x0*x0+y0*y0)/2.0;

					double _a1 = Math.Atan2(E1y - y0,E1x - x0) * _degree_mul;
					double _a2 = Math.Atan2(E2y - y0,E2x - x0) * _degree_mul;
					if (_a1 < 0)
						_a1 += 360.0;
					if (_a2 < 0)
						_a2 += 360.0;

					if (_a2 < _a1)
					{
						_as = _a1;
						_a1 = _a2;
						_a2 = _as;
					}
					if (result.StartAngle <= _a2)
						_as = _a1;
					else
						_as = _a2;
				}
				else
				{
#if DEBUG
					Console.WriteLine("Edge detect failed.");
#endif
				}
				//////////////////////////////////////////////////////////////////////////////
				//expand StartAngle and SweepAngle if SweepAngle < 180degrees
				//if (result.SweepAngle <= 60f && !_local_arc_very_thin)
				if (result.SweepAngle <= 60f)
				{
					result.StartAngle -= result.SweepAngle;
					result.SweepAngle *= 3f;
				}
				else if (result.SweepAngle < 180.0f)
				{
					float _deltaAngle = (180f - result.SweepAngle) / 2.0f;
					result.StartAngle -= _deltaAngle;
					result.SweepAngle = 180f;
				}
//				if (_as > 0.0 && result.StartAngle < _as)
//					result.StartAngle = (float)_as;
//				else 
					if (result.StartAngle < 0f)
					result.StartAngle += 360f;
			}
			else //sparse mode & number of points = 2
			{
				if (_points.Count != 2)
					throw new System.ArgumentException("Please check algorithm. The number of base points does not equal to 2");
				
				_begin = (EllipsePoint)m_sourceSpace[(int)_points[0]];
				_end = (EllipsePoint)m_sourceSpace[(int)_points[1]];

				double _startAngle = (Math.Atan2(_begin.Y - result.Y,_begin.X - result.X) * _degree_mul);
				double _endAngle = (Math.Atan2(_end.Y - result.Y,_end.X - result.X) * _degree_mul);
				if (_startAngle < 0.0)
					_startAngle += 360.0;
				if (_endAngle < 0.0)
					_endAngle += 360.0;
				
				if (_endAngle < _startAngle)//swap
				{
					_endAngle += _startAngle;
					_startAngle = _endAngle - _startAngle;
					_endAngle = _endAngle - _startAngle;
					_begin = (EllipsePoint)m_sourceSpace[(int)_points[1]];
					_end = (EllipsePoint)m_sourceSpace[(int)_points[0]];
				}

				if (_endAngle - _startAngle <= 180.0)
				{
					result.StartAngle = (float)_startAngle;
					result.SweepAngle = (float)(_endAngle - _startAngle);
					result.BeginPoint = _begin;
					result.EndPoint = _end;
				}
				else
				{
					result.StartAngle = (float)_endAngle;
					result.SweepAngle = (float)(_startAngle + 360.0 - _endAngle);
					result.BeginPoint = _end;
					result.EndPoint = _begin;
				}

				//expand StartAngle and SweepAngle if SweepAngle < 180degrees
				if (result.SweepAngle <= 36f)
				{
					result.StartAngle -= result.SweepAngle * 2.0f;
					result.SweepAngle *= 5f;
				}
				else if (result.SweepAngle < 180.0f)
				{
					float _deltaAngle = (180f - result.SweepAngle) / 2.0f;
					result.StartAngle -= _deltaAngle;
					result.SweepAngle = 180f;
				}
			}
		
			#endregion


			return 0;
		}
	
		#endregion

		#region GetArcFeature(ArrayList _points, ArcFeature result, bool _from3points_only)
		public int GetArcFeature(ArrayList _points, ArcFeature result, bool _from3points_only)
		{
			if (_points == null || _points.Count < 3 || !_from3points_only)
				return 1;
			double x1 = m_sourceSpace[(int)_points[0]].X ;
			double y1 = m_sourceSpace[(int)_points[0]].Y ;

			double x2 = m_sourceSpace[(int)_points[1]].X ;
			double y2 = m_sourceSpace[(int)_points[1]].Y ;

			double x3 = m_sourceSpace[(int)_points[2]].X ;
			double y3 = m_sourceSpace[(int)_points[2]].Y ;


			// Center of the circle containing 3 points
			double _dem = (-y3*x1+y3*x2+y2*x1+x3*y1-x3*y2-x2*y1);
			if (Math.Abs(_dem) < double.Epsilon)
				return 2;
			double X = (-y3*x1*x1+y2*x1*x1+y1*y1*y2-y2*y3*y3-y2*x3*x3+y1*y3*y3-y1*y2*y2-y1*x2*x2+y2*y2*y3+y1*x3*x3+x2*x2*y3-y1*y1*y3)/
				_dem/2.0;
			double Y = -(x3*x3*x1-x3*x3*x2+y3*y3*x1-y3*y3*x2+x3*x2*x2-x3*x1*x1-x2*x2*x1-y2*y2*x1+x2*y1*y1+x3*y2*y2-x3*y1*y1+x2*x1*x1)/
				_dem/2.0;
			double R = Math.Sqrt((x1-X)*(x1-X) + (y1-Y)*(y1-Y));

			result.X = (float)X;
			result.Y = (float)Y;
			result.Radius = (float)R;

			return 0;

		}
		#endregion

		public ArcFeature CurrentArcFeature
		{
			get
			{
				if (_ArcFeatureEstimated)
					return currentArcFeature;
				else
					return null;
			}
		}

		
		#region CheckAddingSparse(int _iPoint)
		private bool CheckAddingSparse(int pointIndex)
		{
			if (!sparseModeDetected)
				return false;
			if (_current.Count == 0)
			{
				_ArcFeatureEstimated = false;
				_solution_added = false;			
				return 
					((EllipsePoint)m_sourceSpace[pointIndex]).Eccentricity >= _very_high_ecc;
			}
			if (_solution_added)
				return false;
			if (_current.Count == 1)
			{
				double _x1 = m_sourceSpace[(int)_current[0]].X ;
				double _y1 = m_sourceSpace[(int)_current[0]].Y ;

				double _x2 = m_sourceSpace[pointIndex].X ;
				double _y2 = m_sourceSpace[pointIndex].Y ;
				double _ecc2 = (m_sourceSpace[pointIndex] as EllipsePoint).Eccentricity;

				_ArcFeatureEstimated = false;
				double _d1 = (_x1 - _x2)*(_x1 - _x2) + (_y1 - _y2)*(_y1 - _y2);
				if (_ecc2 < _very_high_ecc || _d1 > _max_dis2)
					return false;
				ArrayList _tmp_pts = new ArrayList(2);
				_tmp_pts.Add(_current[0]);
				_tmp_pts.Add(pointIndex);
				try
				{
					if (this.GetArcFeature(_tmp_pts,_EstimatedArcFeature) != 0)
						return false;
					if (_EstimatedArcFeature.Radius > TopoParam_MaxRadius ||
						_EstimatedArcFeature.Radius < TopoParam_MinRadius)
						return false;
					_ArcFeatureEstimated = true;
					currentArcFeature.CopyFrom(_EstimatedArcFeature);
					return true;
				}
				catch
				{
					throw;
				}
				finally
				{
					if (_tmp_pts != null)
					{
						_tmp_pts.Clear();
						_tmp_pts = null;
					}
				}
			}
			else //Count >= 2
			{
				double _dx = m_sourceSpace[pointIndex].X - currentArcFeature.X;
				double _dy = m_sourceSpace[pointIndex].Y - currentArcFeature.Y;

				double R = Math.Sqrt(_dx * _dx + _dy*_dy);
				if ( R > currentArcFeature.Radius * (1f + TopoParam_MaxAddingRadiusErrorRatio) ||
					R < currentArcFeature.Radius * (1f -  TopoParam_MaxAddingRadiusErrorRatio))
					return false;


				double sumL2R2 = lConstant*lConstant + R*R;
				double doubleLR = 2.0*lConstant*R;

				double secondX = secondXArray[pointIndex] - currentArcFeature.X;
				double secondY = secondYArray[pointIndex] - currentArcFeature.Y;
				double _t =  (sumL2R2 - secondX*secondX - secondY*secondY) / doubleLR;

				if ((veryFewDetected && Math.Abs(_t)  > TopoParam_MaxAddingTanResidualStd * veryFewKTan) 
					|| (Math.Abs(_t)  > TopoParam_MaxAddingTanResidualStd))
					return false;

				_tmpArcFeature.Add(pointIndex, false);
				double newR = _tmpArcFeature.R;
				_tmpArcFeature.Subtract(pointIndex, false);
				if (newR < TopoParam_MinRadius ||
					newR > TopoParam_MaxRadius)
					return false;

				return true; 
			}
		}
		#endregion

		#region Overriden functions
		protected override bool CheckAdding(int _iPoint)
		{
			if (_iPoint >= m_NumPoints)
				return false;		
			if (sparseModeDetected)
				return CheckAddingSparse(_iPoint);

			if (_current.Count == 0)
			{
				_ArcFeatureEstimated = false;
				_solution_added = false;
				return true;
			}
			if (_solution_added)
				return false;
			if (_current.Count == 1)
			{
				if (veryFewDetected)
					return true;
				double _x1 = m_sourceSpace[(int)_current[0]].X ;
				double _y1 = m_sourceSpace[(int)_current[0]].Y ;
				double _ecc1 = (m_sourceSpace[(int)_current[0]] as EllipsePoint).Eccentricity;

				double _x2 = m_sourceSpace[_iPoint].X ;
				double _y2 = m_sourceSpace[_iPoint].Y ;
				double _ecc2 = (m_sourceSpace[_iPoint] as EllipsePoint).Eccentricity;

				_ArcFeatureEstimated = false;
				double _d1 = (_x1 - _x2)*(_x1 - _x2) + (_y1 - _y2)*(_y1 - _y2);
				if (_ecc1 > _very_high_ecc && _ecc2 >_very_high_ecc && _d1 <= _max_dis2) // suppress checking L-min if ecc is very high
					return true;
				
				return (_d1>= _min_dis2) & (_d1 <= _max_dis2);			
			}
			if (_current.Count == 2) 
			{
				double x1 = m_sourceSpace[(int)_current[0]].X ;
				double y1 = m_sourceSpace[(int)_current[0]].Y ;
				double _ecc1 = (m_sourceSpace[(int)_current[0]] as EllipsePoint).Eccentricity;

				double x2 = m_sourceSpace[(int)_current[1]].X ;
				double y2 = m_sourceSpace[(int)_current[1]].Y ;
				double _ecc2 = (m_sourceSpace[(int)_current[1]] as EllipsePoint).Eccentricity;

				double x3 = m_sourceSpace[_iPoint].X ;
				double y3 = m_sourceSpace[_iPoint].Y ;
				double _ecc3 = (m_sourceSpace[_iPoint] as EllipsePoint).Eccentricity;

				double _d1 = (x3 - x2)*(x3 - x2) + (y3 - y2)*(y3 - y2) ;
				double _d2 = (x3 - x1)*(x3 - x1) + (y3 - y1)*(y3 - y1) ;

				if (_ecc1 <= _very_high_ecc ||  _ecc2  <= _very_high_ecc || _ecc3 < _very_high_ecc) // check L-min if ecc is not very high
				{
					bool _check_distance = veryFewDetected || (Math.Min(_d1,_d2) >= _min_dis2)  &
						(Math.Max(_d1,_d2) <= _max_dis2);
					if (! _check_distance)
						return false;
					arcVeryThinDetected = false;
				}
				else
				{
					if (!veryFewDetected && Math.Max(_d1,_d2) > _max_dis2)
						return false;
					arcVeryThinDetected = true;
				}
				// Center of the circle containing 3 points
				double _dem = (-y3*x1+y3*x2+y2*x1+x3*y1-x3*y2-x2*y1);
				if (Math.Abs(_dem) < double.Epsilon)
					return false;
				double X = (-y3*x1*x1+y2*x1*x1+y1*y1*y2-y2*y3*y3-y2*x3*x3+y1*y3*y3-y1*y2*y2-y1*x2*x2+y2*y2*y3+y1*x3*x3+x2*x2*y3-y1*y1*y3)/
					_dem/2.0;
				double Y = -(x3*x3*x1-x3*x3*x2+y3*y3*x1-y3*y3*x2+x3*x2*x2-x3*x1*x1-x2*x2*x1-y2*y2*x1+x2*y1*y1+x3*y2*y2-x3*y1*y1+x2*x1*x1)/
					_dem/2.0;
				double R = Math.Sqrt((x1-X)*(x1-X) + (y1-Y)*(y1-Y));
				if (R > TopoParam_MaxRadius ||
					R < TopoParam_MinRadius)
					return false;
				
				double sumL2R2 = lConstant*lConstant + R*R;
				double doubleLR = 2.0*lConstant*R;				
				
				double secondX = secondXArray[(int)_current[0]] - X;
				double secondY = secondYArray[(int)_current[0]] - Y;
				double _t1 =  (sumL2R2 - secondX*secondX - secondY*secondY)/doubleLR;

				secondX = secondXArray[(int)_current[1]] - X;
				secondY = secondYArray[(int)_current[1]] - Y;
				double _t2 =  (sumL2R2 - secondX*secondX - secondY*secondY)/doubleLR;

				secondX = secondXArray[_iPoint] - X;
				secondY = secondYArray[_iPoint] - Y;
				double _t3 =  (sumL2R2 - secondX*secondX - secondY*secondY)/doubleLR;

				double _max_t = Math.Max(Math.Max(Math.Abs(_t1),Math.Abs(_t2)),Math.Abs(_t3));
				
				double m_t = (_t1 + _t2 + _t3) / 3.0;
				double m_t_error = Math.Sqrt((_t1*_t1 + _t2*_t2 + _t3*_t3) / 3.0 - m_t*m_t);

//				bool result = (m_t_error <= (TopoParam_MaxFoundTanResidualStd) && 
//					Math.Abs(m_t) <=  TopoParam_MaxAddingTanResidualStd );

				bool result = (veryFewDetected && _max_t <=  TopoParam_MaxAddingTanResidualStd * veryFewKTan ) ||
					(_max_t <=  TopoParam_MaxAddingTanResidualStd );

				if (!result)
					return false;

				_EstimatedArcFeature.X = (float)X;
				_EstimatedArcFeature.Y = (float)Y;
				_EstimatedArcFeature.Radius = (float)R;
				_EstimatedArcFeature.TangentialResidualMean = (float)m_t;
				_EstimatedArcFeature.TangentialResidualStd = (float)m_t_error;
				_ArcFeatureEstimated = true;

#if SOL_TRACE 
			if (_ArcFeatureEstimated)
				Console.WriteLine("Radius Error Ratio: {0} \t Tan Residual - Mean: {1} \t Std: {2}", 
					_EstimatedArcFeature.RadiusErrorRatio.ToString(),
					_EstimatedArcFeature.TangentialResidualMean.ToString(),
					_EstimatedArcFeature.TangentialResidualStd.ToString());
#endif

#if ARC_BEST
				if (_EstimatedArcFeature.Radius >= TopoParam_MinRadius && 
					_EstimatedArcFeature.Radius <= TopoParam_MaxRadius &&
					_EstimatedArcFeature.RadiusErrorRatio <= TopoParam_MaxAddingRadiusErrorRatio &&
					_EstimatedArcFeature.TangentialResidualStd < _tan_res_std_min)
				{
					_best.Clear();
					_best.AddRange(_current);
					_best.Add(_iPoint);
					_tan_res_std_min = _EstimatedArcFeature.TangentialResidualStd;
					_best_feature.CopyFrom(_EstimatedArcFeature);
				}
#endif
				return true;

			}
			else //Count > 3
			{
//				if (!currentArcEstimated)
//				{
//#if DEBUG
//					Console.WriteLine("Please check algorithm. Current solution is not estimated.");
//#endif
//					return false;
//				}
				double _dx = m_sourceSpace[_iPoint].X - currentArcFeature.X;
				double _dy = m_sourceSpace[_iPoint].Y - currentArcFeature.Y;

				double R = Math.Sqrt(_dx * _dx + _dy*_dy);
				if ( R > currentArcFeature.Radius * (1f + TopoParam_MaxAddingRadiusErrorRatio) + _R_fixed_error ||
					R < currentArcFeature.Radius * (1f -  TopoParam_MaxAddingRadiusErrorRatio) - _R_fixed_error)
					return false;


				double sumL2R2 = lConstant*lConstant + R*R;
				double doubleLR = 2.0*lConstant*R;

				double secondX = secondXArray[_iPoint] - currentArcFeature.X;
				double secondY = secondYArray[_iPoint] - currentArcFeature.Y;
				double _t =  (sumL2R2 - secondX*secondX - secondY*secondY)/doubleLR;

				if ((veryFewDetected && Math.Abs(_t)  > TopoParam_MaxAddingTanResidualStd * veryFewKTan) 
					|| (Math.Abs(_t)  > TopoParam_MaxAddingTanResidualStd))
					return false;

				_tmpArcFeature.Add(_iPoint, false);
				double newR = _tmpArcFeature.R;
				_tmpArcFeature.Subtract(_iPoint, false);
				if (newR < TopoParam_MinRadius ||
					newR > TopoParam_MaxRadius)
					return false;

				return true; 
			}

		}
		protected override bool FoundCondition()
		{
			if (sparseModeDetected)
			{
				return _current.Count >=2;
			}

			if (arcVeryThinDetected && 
				(_current.Count >= 4 ||
				(_current.Count >= 3 && GetDefectCount() > 10))
				)
				return true;
			if (veryFewDetected && 
				(_current.Count >=5 || (_current.Count >=4 && GetDefectCount() > 10)) ||
				(_current.Count >=2 && m_sourceSpace.Length < TopoParam_MinNumPoints * 1.2))
				return true;
			
			if (_current.Count >= TopoParam_MinNumPoints)
				return true;


			return false;

		}
		protected override bool AddSolution(ArrayList result)
		{
			bool bresult = base.AddSolution(result);
			if (bresult)
			{
				_solution_added = true;
			}
			return bresult;

			#region old code 20060515 - 17-00
//			if (result == null)
//				return false;
//			if (result.Count == 0)
//			{
//#if TRACE
//				if (!currentArcEstimated)
//					Console.WriteLine("Please check algorithm. Current is not estimated.");
//#endif
//				bool _added = base.AddSolution(result);
//				if (!_added)
//					return false;
//				return true;
//			}
//			//select the best solution from overlaid solutions
//			int _overlap_count = 0;
//			ArrayList _last_added_solution = (ArrayList)(result[result.Count-1]);
//			int _last_count = _last_added_solution.Count;
//			int _current_count = _current.Count;
//			int ia = 0, ib = 0;
//			//Please be sure that the last added solution and the current one are ascending sorted
//			while (ia < _last_count && ib < _current_count)
//			{
//				if ((int)_current[ib] < (int)_last_added_solution[ia])
//				{
//					ib++;
//				}
//				else if ((int)_current[ib] > (int)_last_added_solution[ia])
//				{
//					ia++;
//				}
//				else
//				{
//					ia++;
//					ib++;
//					_overlap_count++;
//				}
//			}
//			if (_overlap_count >= 3 && //the same circle
//				_current.Count <= _last_count)
//				return false;
//			if (_overlap_count < 3) // add new solutions
//			{
//				bool _added = base.AddSolution(result);
//				if (!_added)
//					return false;						
//				return true;
//			}
//		
//			_last_added_solution.Clear();
//			_last_added_solution.AddRange(_current);
//			return true;
			#endregion
		}

		protected override void Push(int _iPoint)
		{
			base.Push (_iPoint);
			_tmpArcFeature.Add(_iPoint);
			if (_current.Count == 3)
			{
				currentArcFeature.CopyFrom(_EstimatedArcFeature);
			}
			if (_current.Count >= 4)
			{
				currentArcFeature.X = (float)_tmpArcFeature.X0;
				currentArcFeature.Y = (float)_tmpArcFeature.Y0;
				currentArcFeature.Radius = (float)_tmpArcFeature.R;
			}
		}
		protected override int Pop()
		{		
			int result = base.Pop();
			_tmpArcFeature.Subtract(result);
#if SOL_TRACE 
			if (_current.Count == 0)
			{
				Console.WriteLine("Sx = {0}\t Sy={1}", _tmpArcFeature.Sx.ToString(), _tmpArcFeature.Sy.ToString());
			}
#endif
			if (_current.Count >= 4)
			{
				currentArcFeature.X = (float)_tmpArcFeature.X0;
				currentArcFeature.Y = (float)_tmpArcFeature.Y0;
				currentArcFeature.Radius = (float)_tmpArcFeature.R;
			}
			return result;
		}

		protected override void Reset()
		{
			_tmpArcFeature.Reset();
			base.Reset();
		}
		#endregion

		#region GetDefectCount()
		private int GetDefectCount()
		{
			if (_current == null || _current.Count == 0)
				return 0;
			double _defcount = 0.0;
			EllipsePoint _el = null;
			foreach (int _ip in _current)
			{
				_el = (EllipsePoint)m_sourceSpace[_ip];
				_defcount += _el.Length * _el.Minor * Math.PI / 4.0 * _el.Density;
			}
			return (int)_defcount;
		}

		#endregion

		#region EstimateCurrentAddingiPoint(int _adding_point)
		private int EstimateCurrentAddingiPoint(int _adding_point)
		{
			if (_current == null || _current.Count < 2)
			{
				currentArcEstimated = false;
				return 1;
			}
			int N = _current.Count;

			_tmpArcFeature.Add(_adding_point);
			double X0 = _tmpArcFeature.X0;
			double Y0 = _tmpArcFeature.Y0;

			double SR2 = 0D,SR = 0D,_r = 0D;
			double _tan_residual_sum = 0D, _tan_residual_s2 = 0D;
			double _tan_tmp = 0D;

			double _x = 0D, _y = 0D;

			foreach (int _ip in _current)
			{
				_x = m_sourceSpace[_ip].X;
				_y =  m_sourceSpace[_ip].Y;

				_tan_tmp = Math.Atan((_y - Y0)/(_x - X0))*180D/Math.PI - m_sourceSpace[_ip].Orientation;
				_tan_residual_sum += _tan_tmp;
				_tan_residual_s2 += _tan_tmp*_tan_tmp;

				_r = Math.Sqrt((_x - X0)*(_x - X0) + (_y - Y0)*(_y - Y0));
				SR += _r;
				SR2 += _r*_r;
			}
			_x = m_sourceSpace[_adding_point].X;
			_y =  m_sourceSpace[_adding_point].Y;

			_tan_tmp = Math.Atan((_y - Y0)/(_x - X0))*180D/Math.PI - m_sourceSpace[_adding_point].Orientation;
			_tan_residual_sum += _tan_tmp;
			_tan_residual_s2 += _tan_tmp*_tan_tmp;

			_r = Math.Sqrt((_x - X0)*(_x - X0) + (_y - Y0)*(_y - Y0));
			SR += _r;
			SR2 += _r*_r;

			_EstimatedArcFeature.X = (float)(X0);
			_EstimatedArcFeature.Y = (float)(Y0);
			_EstimatedArcFeature.Radius = (float)(SR / (N+1) );
			_EstimatedArcFeature.RadiusError = (float)(Math.Sqrt(SR2/(N+1) - _EstimatedArcFeature.Radius*_EstimatedArcFeature.Radius));

			_EstimatedArcFeature.TangentialResidualMean = (float)(_tan_residual_sum / (N+1));
			_EstimatedArcFeature.TangentialResidualStd = (float)
				(Math.Sqrt(_tan_residual_s2/(N+1) - _EstimatedArcFeature.TangentialResidualMean*_EstimatedArcFeature.TangentialResidualMean));
			_ArcFeatureEstimated = true;

			_tmpArcFeature.Subtract(_adding_point);
			return 0;
		}
		#endregion

	}
	#endregion

	#region ConnectivityTracking
	public interface IConnectivityComparer
	{
		bool IsConnected(object x, object y);
		bool IsConnected(object x, object y, object z);
		bool IsSuppressible(object x, object y);
		void Init(IHumanCondition Conditions);
	}

	public class ConnectTracking : TopologyBackTracking
	{
		private IConnectivityComparer _Comparer;
		private int[] _label;
		public int[] Label
		{
			get
			{
				return _label;
			}
		}
		public ConnectTracking(BasePoint[] _source,IConnectivityComparer _comparer) : base(_source,TOPOLOGY.NONE)
		{
			if (_source == null)
				_label = null;
			else
				_label = new int[_source.Length];
			_Comparer = _comparer;
		}
		protected override bool AddSolution(ArrayList result)
		{
			return base.AddSolution(result);
		}
		protected override bool CheckAdding(int _iPoint)
		{
			
			if (_current.Count == 0)
				return true;
			for (int i = 0; i < _current.Count ; i ++)
				if (_Comparer.IsConnected(m_sourceSpace[_iPoint], m_sourceSpace[(int)_current[i]]))
					return true;
			return false;
		}
		protected override bool FoundCondition()
		{
			return true;
		}
		protected override int Pop()
		{
			return base.Pop ();
		}

		protected override void Push(int _iPoint)
		{
			base.Push (_iPoint);
		}
		protected override void Reset()
		{
			base.Reset ();
		}

	}
	#endregion

	#region DFS ConnectTracking
	public class DFSConnectTracking
	{
		protected IConnectivityComparer _Comparer;
		protected int[] _label;
		public int[] Label
		{
			get
			{
				return _label;
			}
		}
		protected BasePoint[] m_sourceSet;
		protected int hyperpointNumber;

		protected bool[,] _bConnectMatrix;
		protected virtual void initConnectedMatrix()
		{
			_bConnectMatrix = new bool[hyperpointNumber,hyperpointNumber];
			for (int i = 0; i < hyperpointNumber; i++)
			{
				_bConnectMatrix[i,i] = true;
				for (int j=i+1;j<hyperpointNumber; j++)
				{
					bool bres = _Comparer.IsConnected(m_sourceSet[i],m_sourceSet[j]);
					_bConnectMatrix[i,j] = bres;
					_bConnectMatrix[j,i] = bres;
				}
			}
		}
		public DFSConnectTracking(BasePoint[] _source, IConnectivityComparer _comparer)
		{
			if (_source == null)
				return;
			_label = new int[_source.Length];
			_Comparer = _comparer;
			m_sourceSet = _source;
			hyperpointNumber = m_sourceSet.Length;
			initConnectedMatrix();
		}
		public virtual int AssignLabel()
		{
			if (_label == null)
				return 0;
			Array.Clear(_label,0,_label.Length);
			if (m_sourceSet == null || m_sourceSet.Length == 0)
				return 0;

			ArrayList mSet = new ArrayList(hyperpointNumber);
			Stack _stack = new Stack(hyperpointNumber);
			for (int i=0; i < hyperpointNumber; i++)
				mSet.Add(i);

			int icurrent = 0,current=0,lcurrent = 0, last = 0;
			while (mSet.Count > 0)
			{
				if ( icurrent <= mSet.Count-1)
				{
					current = (int)mSet[icurrent];
					if (_stack.Count == 0) //perform new cluster
					{
						_label[current] = ++ lcurrent;
						_stack.Push(current);
						mSet.RemoveAt(icurrent);
						icurrent = 0;
						last = current;
					}
					else
					{
						if (_bConnectMatrix[current,last])
						{
							_label[current] = lcurrent;
							_stack.Push(current);
							mSet.RemoveAt(icurrent);
							icurrent = 0;
							last = current;
						}
						else //current points is not connected
							icurrent++;
					}				
				}
				else
				{
					_stack.Pop();
					if (_stack.Count > 0)
						last = (int)_stack.Peek();
					icurrent = 0;
				}
			}

			return 0;
		}
	}

	#endregion

	#region Tristate : byte
	public enum Tristate : byte
	{
		Unknown = 0,
		True,
		False
	}
	#endregion

	#region DFSConnectTracking3Points : DFSConnectTracking

	public class DFSConnectTracking3Points : DFSConnectTracking
	{
		public DFSConnectTracking3Points(
            BasePoint[] source, IConnectivityComparer comparer) : base(source, comparer)
		{
		}

		protected override void initConnectedMatrix()
		{
			base.initConnectedMatrix();
		}
		#region int AssignLabel()
		public override int AssignLabel()
		{
			int returnCode = base.AssignLabel();
			if (returnCode != 0)
				return returnCode;
			try
			{
				int nGroups = MatrixUtils.PackLabelMatrix(ref _label, -1);
				int lastGroupIndex = nGroups;
				for (int iGroup = nGroups; iGroup > 0; iGroup--)
				{
					//DateTime start = DateTime.Now;

					lastGroupIndex = reclassify(iGroup, lastGroupIndex);

					//Console.WriteLine("reclassify {0}", DateTime.Now - start);
				}
                MatrixUtils.PackLabelMatrix(ref _label, -1);
				return 0;
			}
			catch (System.Exception exp) 
			{
#if DEBUG
				Console.WriteLine(exp.Message);
				Console.WriteLine(exp.StackTrace);
#endif
				return 1;
			}
		}

		#endregion

		#region fillConnectMatrix3Points(ref Tristate [,,] connectMatrix, ref int[] defNum, IntCollection groupHyperPointIndex, int nPoints, bool initFull)
		private void fillConnectMatrix3Points(ref Tristate [,,] connectMatrix, IntCollection groupHyperPointIndex, int nPoints, bool initFull)
		{
			connectMatrix = new Tristate[nPoints, nPoints, nPoints];		
			for (int i = 0; i < nPoints; i++)
			{
				connectMatrix[i, i, i] = Tristate.False;
				for (int j = i+1; j < nPoints; j++)
				{
					connectMatrix[i, i, j] = 
						connectMatrix[i, j, i] =
						connectMatrix[j, i, i] =
						_bConnectMatrix[groupHyperPointIndex[i], groupHyperPointIndex[j]] ? Tristate.True : Tristate.False;

                    if (!_bConnectMatrix[groupHyperPointIndex[i], groupHyperPointIndex[j]] &&
                        _Comparer.IsSuppressible(m_sourceSet[groupHyperPointIndex[i]], m_sourceSet[groupHyperPointIndex[j]]))
                        for (int k = 0; k < nPoints; k++)
                            assignAllPermutation(connectMatrix, i, j, k, Tristate.False);

					if (initFull)
					{
						for (int k=j+1; k < nPoints; k++)
						{
							assignAllPermutation(connectMatrix, i, j, k,
								_Comparer.IsConnected(m_sourceSet[groupHyperPointIndex[i]],
								m_sourceSet[groupHyperPointIndex[j]],
								m_sourceSet[groupHyperPointIndex[k]]) ? Tristate.True : Tristate.False);
						}
					}
				}
			}

			#region if DEBUG && TRACE
			/*
#if DEBUG && TRACE
			using (StreamWriter txt = new StreamWriter(@"D:\temp\true3pointConnected.txt", false))
			{
				txt.WriteLine("Number of hyper-point: {0}", nPoints);
				for (int point = 0; point < nPoints; point++)
					txt.WriteLine("({0}, {1})", m_sourceSet[groupHyperPointIndex[point]].X, m_sourceSet[groupHyperPointIndex[point]].Y);
				for (int i = 0; i < nPoints; i++)
					for (int j = i+1; j < nPoints; j++)
						for (int k = j+1; k < nPoints; k++)
							if (connectMatrix[i, j, k])
								txt.WriteLine("{0} - {1} - {2}", i, j, k);
			} 
#endif
			   */
			#endregion

		}

		#endregion

		#region fillInitialSet(ref IntCollection true3PointSet, Tristate [,,] connectMatrix, IntCollection groupHyperPointIndex, int nPoints)
		private void fillInitialSet(ref IntCollection true3PointSet, Tristate [,,] connectMatrix, IntCollection groupHyperPointIndex, int nPoints)
		{
			true3PointSet = new IntCollection(nPoints);
			int i1 = 0, i2 = 0, i3 = 0;
			while (true)
			{
				if (i3 >= nPoints)
					i3 = ++i2;
				if (i2 >= nPoints)
					i3 = i2 = ++i1;
				if (i1 >= nPoints || i2 >= nPoints || i3 >= nPoints)
					break;
				if (connectMatrix[i1, i2, i3] == Tristate.Unknown)
				{
					assignAllPermutation(connectMatrix, i1, i2, i3, 
						_Comparer.IsConnected(m_sourceSet[groupHyperPointIndex[i1]],
						m_sourceSet[groupHyperPointIndex[i2]],
						m_sourceSet[groupHyperPointIndex[i3]]) ? Tristate.True : Tristate.False);
				}
				if (connectMatrix[i1, i2, i3] == Tristate.True)
				{
					true3PointSet.Add(i1);
					if (i2 != i1)
						true3PointSet.Add(i2);
					if (i3 != i2)
						true3PointSet.Add(i3);
					break;
				}
				i3++;
			}
		}

		#endregion

		#region growUpTrue3PointSet(ref int lastAdded, IntCollection true3PointSet, Tristate [,,] connectMatrix, IntCollection groupHyperPointIndex, IntCollection reducedHP, int nPoints)
		private bool growUpTrue3PointSet(IntCollection true3PointSet, Tristate [,,] connectMatrix, IntCollection groupHyperPointIndex, IntCollection reducedHP, int nPoints)
		{
#if DEBUG_
			using (StreamWriter txt = new StreamWriter(@"D:\temp\trace_3point.txt", true))
			{
				foreach (int ipoint in true3PointSet)
					txt.Write(" {0}", ipoint);
				txt.WriteLine();
			}
#endif
			int ikLow = 0, ikHigh = 0;
			for (int iaddingPoint = 0; iaddingPoint < reducedHP.Count; iaddingPoint++)
			{
				int addingPoint = reducedHP[iaddingPoint];
				
				int iHigh = 	true3PointSet.Count - 1;		
				while (iHigh > 1 && true3PointSet[iHigh] < addingPoint)
					iHigh--;
				for (; iHigh < true3PointSet.Count; iHigh++)
				{
					ikHigh = true3PointSet[iHigh];

					for (int iLow = 0; iLow < iHigh; iLow++)
					{
						ikLow = true3PointSet[iLow];
#if DEBUG_
						using (StreamWriter txt = new StreamWriter(@"D:\temp\trace_3point.txt", true))
							txt.WriteLine("{0,4} - {1,4} - {2,4} ... ", addingPoint, ikLow, ikHigh);
#endif
						if (connectMatrix[ikLow, ikHigh, addingPoint] == Tristate.Unknown)
						{
							assignAllPermutation(connectMatrix, ikLow, ikHigh, addingPoint, 
								_Comparer.IsConnected(m_sourceSet[groupHyperPointIndex[ikLow]],
								m_sourceSet[groupHyperPointIndex[ikHigh]],
								m_sourceSet[groupHyperPointIndex[addingPoint]]) ? Tristate.True : Tristate.False);
						}

						if (connectMatrix[ikLow, ikHigh, addingPoint] == Tristate.True)
						{
#if DEBUG_
							using (StreamWriter txt = new StreamWriter(@"D:\temp\trace_3point.txt", true))
								txt.WriteLine("Found {0,4} - {1,4} - {2,4} ... ", addingPoint, ikLow, ikHigh);
#endif
							true3PointSet.Add(addingPoint);
							reducedHP.RemoveAt(iaddingPoint);
							return true;
						}
					}
				}
			}
			return false;
		}

		#endregion

		#region sortHyperPointIndexByDefectNumber(IntCollection groupHyperPointIndex)
		private void sortHyperPointIndexByDefectNumber(IntCollection groupHyperPointIndex)
		{
			if (groupHyperPointIndex == null || groupHyperPointIndex.Count <= 1)
				return;
			int[] groupHPIndexArray = (int[])groupHyperPointIndex.ToArray();
			BasePoint[] basepointCollection = new BasePoint[groupHyperPointIndex.Count];
			try
			{
				for (int iPointIndex = 0; iPointIndex < groupHyperPointIndex.Count; iPointIndex++)
				{
					basepointCollection[iPointIndex] = m_sourceSet[groupHyperPointIndex[iPointIndex]];
				}
				Array.Sort(basepointCollection, groupHPIndexArray, BasePointComparers.PointCountComparer);
				Array.Reverse(groupHPIndexArray);
				groupHyperPointIndex.Clear();
				foreach (int ipoint in groupHPIndexArray)
					groupHyperPointIndex.Add(ipoint);
			}
			catch {}
			finally
			{
				if (basepointCollection != null)
				{
					Array.Clear(basepointCollection, 0, basepointCollection.Length);
					basepointCollection = null;
				}
			}

		}
		#endregion

		#region reclassify(int iGroup, int lastGroupIndex)
		private int reclassify(int iGroup, int lastGroupIndex)
		{
			if (_label == null || iGroup < 1 || iGroup > hyperpointNumber)
				return lastGroupIndex;
			IntCollection groupHyperPointIndex = new IntCollection(hyperpointNumber);
			for (int iHyperPointIndex = 0; iHyperPointIndex < hyperpointNumber; iHyperPointIndex++)
				if (_label[iHyperPointIndex] == iGroup)
					groupHyperPointIndex.Add(iHyperPointIndex);

			sortHyperPointIndexByDefectNumber(groupHyperPointIndex);

			Tristate[,,] bConnectMatrix3Points = null;	
			fillConnectMatrix3Points(ref bConnectMatrix3Points, groupHyperPointIndex, groupHyperPointIndex.Count, false);

			return reclassifyInner(groupHyperPointIndex, bConnectMatrix3Points, lastGroupIndex);
		}

		#endregion

		#region assignAllPermutation(Tristate[,,] matrix, int i1, int i2, int i3, Tristate connectValue)
		private void assignAllPermutation(Tristate[,,] matrix, int i1, int i2, int i3, Tristate connectValue)
		{
			matrix[i1, i2, i3]  = 
				matrix[i1, i3, i2] =
				matrix[i2, i1, i3] =
				matrix[i2, i3, i1] =
				matrix[i3, i1, i2] =
				matrix[i3, i2, i1] = connectValue;
		}

		#endregion

		#region reclassifyInner(IntCollection groupHyperPointIndex, Tristate [,,] bConnectMatrix3Points, int lastGroupIndex)
		private int reclassifyInner(IntCollection groupHyperPointIndex, Tristate [,,] bConnectMatrix3Points, int lastGroupIndex)
		{
			if (groupHyperPointIndex.Count < 2)
				return lastGroupIndex;
			if (groupHyperPointIndex.Count == 2)
			{
				if (_bConnectMatrix[groupHyperPointIndex[0], groupHyperPointIndex[1]])
					return lastGroupIndex;
				_label[groupHyperPointIndex[1]] = ++lastGroupIndex;
				return lastGroupIndex;
			}
			if (groupHyperPointIndex.Count == 3)
			{
				bConnectMatrix3Points[0, 1, 2] = _Comparer.IsConnected(
					m_sourceSet[groupHyperPointIndex[0]],
					m_sourceSet[groupHyperPointIndex[1]],
					m_sourceSet[groupHyperPointIndex[2]]) ? Tristate.True : Tristate.False;
				if (bConnectMatrix3Points[0, 1, 2] == Tristate.True)
				{
					return lastGroupIndex;
				}
				else //reclassify
				{
					#region reclassify a group of 3 points
					int sumDefectNum = -1;
					int selectedVariant = -1;
					if (_bConnectMatrix[groupHyperPointIndex[0], groupHyperPointIndex[1]] && 
						(m_sourceSet[groupHyperPointIndex[0]].PointCount + m_sourceSet[groupHyperPointIndex[1]].PointCount) > sumDefectNum)
					{
						sumDefectNum = m_sourceSet[groupHyperPointIndex[0]].PointCount + m_sourceSet[groupHyperPointIndex[1]].PointCount;
						selectedVariant = 0;
					}
					if (_bConnectMatrix[groupHyperPointIndex[2], groupHyperPointIndex[1]] && 
						(m_sourceSet[groupHyperPointIndex[2]].PointCount + m_sourceSet[groupHyperPointIndex[1]].PointCount) > sumDefectNum)
					{
						sumDefectNum = m_sourceSet[groupHyperPointIndex[2]].PointCount + m_sourceSet[groupHyperPointIndex[1]].PointCount;
						selectedVariant = 1;
					}
					if (_bConnectMatrix[groupHyperPointIndex[0], groupHyperPointIndex[2]] && 
						(m_sourceSet[groupHyperPointIndex[0]].PointCount + m_sourceSet[groupHyperPointIndex[2]].PointCount) > sumDefectNum)
					{
						sumDefectNum = m_sourceSet[groupHyperPointIndex[0]].PointCount + m_sourceSet[groupHyperPointIndex[2]].PointCount;
						selectedVariant = 2;
					}
					switch (selectedVariant)
					{
						case -1:
							_label[groupHyperPointIndex[1]] = ++lastGroupIndex;
							_label[groupHyperPointIndex[2]] = ++lastGroupIndex;
							return lastGroupIndex;
						case 0: //0,1 belong to one group
							_label[groupHyperPointIndex[2]] = ++lastGroupIndex;
							return lastGroupIndex;
						case 1: //2,1 belong to one group
							_label[groupHyperPointIndex[0]] = ++lastGroupIndex;
							return lastGroupIndex;
						case 2: //0,2 belong to one group
							_label[groupHyperPointIndex[1]] = ++lastGroupIndex;
							return lastGroupIndex;
					}
					#endregion
				}
			}
			
			#region reclassify a group of 3+ points
			int nGroupHyperPoint = groupHyperPointIndex.Count;
			IntCollection true3PointSet = null; 
			fillInitialSet(ref true3PointSet, bConnectMatrix3Points, groupHyperPointIndex, nGroupHyperPoint);

			if (true3PointSet == null || true3PointSet.Count < 2)
			{
				for (int i = 0; i < nGroupHyperPoint; i++)
					_label[groupHyperPointIndex[i]] = ++lastGroupIndex;
				return lastGroupIndex;
			}	

			IntCollection reducedHP = new IntCollection(nGroupHyperPoint);
			for (int i = 0; i < nGroupHyperPoint; i++)
				reducedHP.Add(i);
			for (int i = 0; i < true3PointSet.Count; i++)
				reducedHP.Remove(true3PointSet[i]);

			while (growUpTrue3PointSet(true3PointSet, bConnectMatrix3Points, groupHyperPointIndex, reducedHP, nGroupHyperPoint));

			lastGroupIndex++;
			foreach (int foundPoint in true3PointSet)
				_label[groupHyperPointIndex[foundPoint]] = lastGroupIndex;
			true3PointSet.Clear();
			true3PointSet = null;
			
			#region recursive!!!!
			int newGroupCount = reducedHP.Count;
			Tristate [,,] newConnectMatrix3Point = new Tristate[newGroupCount, newGroupCount, newGroupCount];
			IntCollection newGroupHyperPointIndex = new IntCollection(newGroupCount);
			for (int i1 = 0; i1 < newGroupCount; i1++)
			{
				newGroupHyperPointIndex.Add(groupHyperPointIndex[reducedHP[i1]]);
				for (int i2 = 0; i2 < newGroupCount; i2++)
					for (int i3 = 0; i3 < newGroupCount; i3++)
						newConnectMatrix3Point[i1, i2, i3] = bConnectMatrix3Points[reducedHP[i1], reducedHP[i2], reducedHP[i3]];
			}
			
			if (groupHyperPointIndex != null)
			{
				groupHyperPointIndex.Clear();
				groupHyperPointIndex = null;
			}
			bConnectMatrix3Points = null;

			return reclassifyInner(newGroupHyperPointIndex, newConnectMatrix3Point, lastGroupIndex);
			#endregion

			#endregion
		}
	
		#endregion


	}

	#endregion

}
