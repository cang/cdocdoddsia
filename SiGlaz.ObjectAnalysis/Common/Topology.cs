#define CHECK_COMPATIBLE_
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;

using SiGlaz.Algorithms.Core;

namespace SiGlaz.ObjectAnalysis.Common
{
	public enum SPLINE_TYPE : byte
	{
		NONE = 0,
		B,
		BEZIER,
		CUBIC,
		QUADRATIC
	}

	public class TopologyTypeItem
	{
		public TOPOLOGY TopoType;
		public override string ToString()
		{
			switch (TopoType)
			{
				case TOPOLOGY.NONE:
					return "None";
				case TOPOLOGY.LINE:
					return "Line";
				case TOPOLOGY.ARC:
					return "Arc";
				case TOPOLOGY.CIRCLE:
					return "CIRCLE";
				case TOPOLOGY.GRID:
					return "GRID";
				case TOPOLOGY.MANUAL:
					return "MANUAL";
			}
			return string.Empty;
		}
		public TopologyTypeItem(TOPOLOGY _topo)
		{
			TopoType = _topo;

		}
	}
	#region Discrete Parameter
	public abstract class DiscreteParameter
	{
		public string Name;
		[XmlIgnore]
		public abstract object StandardValue
		{
			get;
			set;
		}
		[XmlIgnore]
		public object CurrentValue
		{
			get
			{
				if (Level < 0 || Level > NumOfLevels)
					return LevelValues.GetValue(LevelDefault);
				else
					return LevelValues.GetValue(Level);
			}
			set
			{
				if (Level < 0 || Level > NumOfLevels)
					LevelValues.SetValue(value,LevelDefault);
				else
					LevelValues.SetValue(value,Level);

			}
		}
		public int Level;
		public string[] LevelNames;
		[XmlIgnore]
		public abstract Array LevelValues
		{
			get;
			set;
		}
		public int LevelDefault;
		[XmlIgnore]
		public int NumOfLevels
		{
			get
			{
				if (LevelNames == null)
					return 0;
				return LevelNames.Length;
			}
		}
		public DiscreteParameter() :this(string.Empty, null, 0)
		{
		}
		public DiscreteParameter(string _paramName, string[] _levelNames, int _levelDefault)
		{
			Name = _paramName;
			LevelNames = _levelNames;
			LevelDefault = _levelDefault;
			Level = LevelDefault;
		}
		public override string ToString()
		{
			return Name;
		}
		public abstract void Dispose();
		public abstract DiscreteParameter Clone();
	}
	public class FloatDiscreteParameter: DiscreteParameter
	{
		private float m_standardValue;
		[XmlIgnore]
		public override object StandardValue
		{
			get
			{
				return m_standardValue;
			}
			set
			{
				m_standardValue = (float)value;
			}
		}
		public float[] m_levelValues;
		[XmlIgnore]
		public override Array LevelValues
		{
			get
			{
				return m_levelValues;
			}
			set
			{
				if (value.Length != NumOfLevels )
					throw new System.ArgumentException("Lengths of value array and name array are not equal.");
				m_levelValues = (float[])value;
			}
		}
		public FloatDiscreteParameter():this(string.Empty, null, 0, 1f, null)
		{
		}
		public FloatDiscreteParameter(string _paramName, string [] _levelNames, int _levelDefault, float _stdValue, float [] _levelValues) :
			base(_paramName, _levelNames, _levelDefault)
		{
			m_standardValue = _stdValue;
			m_levelValues = _levelValues;
		}

		public override void Dispose()
		{
			m_levelValues = null;
			LevelNames = null;
		}
		public override DiscreteParameter Clone()
		{
			FloatDiscreteParameter result = new FloatDiscreteParameter(
				Name,(string[])LevelNames.Clone(),LevelDefault,m_standardValue,(float[])m_levelValues.Clone());
			return (DiscreteParameter)result;
		}

	}
	public class IntDiscreteParameter: DiscreteParameter
	{
		private int m_standardValue;
		[XmlIgnore]
		public override object StandardValue
		{
			get
			{
				return m_standardValue;
			}
			set
			{
				m_standardValue = (int)value;
			}
		}
		public int[] m_levelValues;
		[XmlIgnore]
		public override Array LevelValues
		{
			get
			{
				return m_levelValues;
			}
			set
			{
				if (value.Length != NumOfLevels )
					throw new System.ArgumentException("Lengths of value array and name array are not equal.");
				m_levelValues = (int[])value;
			}
		}
		public IntDiscreteParameter() : this(string.Empty, null, 0, 1, null)
		{
		}
		public IntDiscreteParameter(string _paramName, string [] _levelNames, int _levelDefault, int _stdValue, int [] _levelValues) :
			base(_paramName, _levelNames, _levelDefault)
		{
			m_standardValue = _stdValue;
			m_levelValues = _levelValues;
		}
		public override void Dispose()
		{
			m_levelValues = null;
			LevelNames = null;
		}
		public override DiscreteParameter Clone()
		{
			IntDiscreteParameter result = new IntDiscreteParameter(
				Name,(string[])LevelNames.Clone(),LevelDefault,m_standardValue,(int[])m_levelValues.Clone());
			return (DiscreteParameter)result;
		}


	}
	#endregion

	#region Topology Parameters
	public abstract class TopoParam
	{
		private TOPOLOGY m_TopoType;
		public TOPOLOGY TopoType
		{
			get
			{
				return m_TopoType;
			}
		}
		public abstract DiscreteParameter[] Parameters
		{
			get;
		}
		protected TopoParam(TOPOLOGY _topo)
		{
			m_TopoType = _topo;
		}
		public override string ToString()
		{
			switch (TopoType)
			{
				case TOPOLOGY.NONE:
					return "None";
				case TOPOLOGY.LINE:
					return "Line";
				case TOPOLOGY.CIRCLE:
					return "CIRCLE";
				case TOPOLOGY.GRID:
					return "GRID";
			}
			return string.Empty;
		}
		public abstract void Dispose();
	}
	public class NoneParam: TopoParam
	{
		public NoneParam():base(TOPOLOGY.NONE)
		{
		}
		public override void Dispose()
		{

		}
		public override DiscreteParameter[] Parameters
		{
			get
			{
				return null;
			}
		}


	}
	public class LineParam : TopoParam
	{
		private DiscreteParameter[] _discreteParameters;

		[XmlElement(typeof(IntDiscreteParameter)),
		XmlElement(typeof(FloatDiscreteParameter))]
		public DiscreteParameter[] DiscreteParameters
		{
			get
			{
				return _discreteParameters;
			}
			set
			{
				_discreteParameters = value;
			}
		}
		
		[XmlIgnore]
		public int InitPoints
		{
			get
			{
				return (int)(_discreteParameters[0] as IntDiscreteParameter).CurrentValue;
			}
			set
			{
				_discreteParameters[0].CurrentValue = value;
			}
		}
		[XmlIgnore]
		public int MinNumPoints
		{
			get
			{
				return (int)(_discreteParameters[1] as IntDiscreteParameter).CurrentValue;
			}
			set
			{
				_discreteParameters[1].CurrentValue = value;
			}
		}
		[XmlIgnore]
		public float FirstPercentageValue
		{
			get
			{
				return (float)(_discreteParameters[2] as FloatDiscreteParameter).CurrentValue;
			}
			set
			{
				_discreteParameters[2].CurrentValue = value;
			}
		}

		[XmlIgnore]
		public float FirstPercentageMinEcc
		{
			get
			{
				return (float)(_discreteParameters[3] as FloatDiscreteParameter).CurrentValue;
			}
			set
			{
				_discreteParameters[3].CurrentValue = value;
			}

		}

		[XmlIgnore]
		public int FirstPercentageMinPoints
		{
			get
			{
				return (int)(_discreteParameters[4] as IntDiscreteParameter).CurrentValue;
			}
			set
			{
				_discreteParameters[4].CurrentValue = value;
			}
		}
		[XmlIgnore]
		public float MaxLength
		{
			get
			{
				return (float)(_discreteParameters[5] as FloatDiscreteParameter).CurrentValue;
			}
			set
			{
				_discreteParameters[5].CurrentValue = value;
			}
		}
		[XmlIgnore]
		public float MinLength
		{
			get
			{
				return (float)(_discreteParameters[6] as FloatDiscreteParameter).CurrentValue;
			}
			set
			{
				_discreteParameters[6].CurrentValue = value;
			}
		}

		[XmlIgnore]
		public float MaxDistantError
		{
			get
			{
				return (float)(_discreteParameters[7] as FloatDiscreteParameter).CurrentValue;
			}
			set
			{
				_discreteParameters[7].CurrentValue = value;
			}
		}

		[XmlIgnore]
		public float MaxAngularError
		{
			get
			{
				return (float)(_discreteParameters[8] as FloatDiscreteParameter).CurrentValue;
			}
			set
			{
				_discreteParameters[8].CurrentValue = value;
			}
		}


		public LineParam():base(TOPOLOGY.LINE)
		{
			_discreteParameters = 
				new DiscreteParameter[9] {
											 new IntDiscreteParameter("Initial number of points",new string[] {"Many","Normal","Few"},1,1,new int[] {3,2,1}),
											 new IntDiscreteParameter("Minimum number of points",new string[] {"Many","Normal","Few"},1,7,new int[] {9,7,5}),
											 new FloatDiscreteParameter("1st Percentage Value ",new string[] {"High","Medium","Low"},1,80f,new float[] {1f,0.8f,0.5f}),
											 new FloatDiscreteParameter("1st Percentage Min Ecc ",new string[] {"High","Medium","Low"},1,0.8f,new float[] {0.9f,0.8f,0.7f}),
											 new IntDiscreteParameter("1st Percentage Min Points ",new string[] {"High","Medium","Low"},1,5,new int[] {8,5,3}),
											 new FloatDiscreteParameter("Max Length",new string[] {"High","Medium","Low"},1,150000f,new float[] {200000f,150000f,100000f}),
											 new FloatDiscreteParameter("Min Length",new string[] {"High","Medium","Low"},1,30000f,new float[] {50000f,30000f,10000f}),
											 new FloatDiscreteParameter("Max Distant Error",new string[] {"High","Medium","Low"},1,800f,new float[] {1000f,800f,400f}),
											 new FloatDiscreteParameter("Max Angular Error",new string[] {"High","Medium","Low"},1,0.05f,new float[] {0.1f,0.05f,0.02f})
										 };
		}
		public void CopyFrom(LineParam _src)
		{
			if (_src == null)
				return;
			for (int i=0;i<_discreteParameters.Length;i++)
			{
				if (_discreteParameters[i] != null)
				{
					_discreteParameters[i].Dispose();
				}
				_discreteParameters[i] = _src.Parameters[i].Clone();
			}
		}
		public override DiscreteParameter[] Parameters
		{
			get
			{
				return _discreteParameters;
			}
		}

		public override void Dispose()
		{
			foreach (DiscreteParameter _param in _discreteParameters)
			{
				if (_param == null)
					continue;
				_param.Dispose();
			}
			_discreteParameters = null;
		}

	}
	
	public class ArcParam : TopoParam
	{
		#region Check Conditions
		public bool RemoveAdjacentArcs = true;
		public bool DetectTapped = true;
		public bool CheckLocalDefectRatio = true;
		#endregion Check Conditions

		public double LConstant = 50000.0f;//L constant for computing the second point
		public double VeryHighEcc = 0.95; //The lowwer threhold of Very High Ecc mode
		public double MaxDis2Ratio = 49;// 180.0f; // Squared Maximum allowed distance between two hyperpoints while tracking
		public double MinDis2Ratio = 2.0; // Squared Miniimum allowed distance between two hyperpoints while tracking
		public double VeryFewKTan = 6.0; //Tan multiplication coefficient in Very Few Mode
		public double RFixedError = 100.0;//Fixed allowed Radius error

		#region Tracking Condition
		public double MinReducedLengthRatio = 0.1; //all of hyper-points having the length less than (THIS value * max length) are discarded
		public double MaxWidthLengthRatio = 0.007; //
		public double MinDistanceFromWaferCenter = 15000.0; //for discarding centered arc (they are like circle)
		
		public double MinLocalDefectRatio = 0.30;
		public double WidthLocalDefectRatio = 10.0;
		public double WidthLocalFixed = 20000;
		#endregion Tracking Condition

		#region Tapped scratch detect
		public double TappedAngle = 5.0; //Tap angle in degrees
		public double TappedLengthRatio = 0.5; //Tap length ratio
		public double TappedN1Min = 1.0;
		#endregion

		public double MinConfidenceLevel = 0.4;//The minimum confidence level of arc. This value use in both cases of using and don't using Confidence Test


		private DiscreteParameter[] _discreteParameters;
		
		[XmlElement(typeof(IntDiscreteParameter)),
		XmlElement(typeof(FloatDiscreteParameter))]
		public DiscreteParameter[] DiscreteParameters
		{
			get
			{
				return _discreteParameters;
			}
			set
			{
				_discreteParameters = value;
			}
		}
		[XmlIgnore]
		public int InitPoints
		{
			get
			{
				return (int)(_discreteParameters[0] as IntDiscreteParameter).CurrentValue;
			}
			set
			{
				_discreteParameters[0].CurrentValue = value;
			}
		}

		[XmlIgnore]
		public int MinNumPoints
		{
			get
			{
				return (int)(_discreteParameters[1] as IntDiscreteParameter).CurrentValue;
			}
			set
			{
				_discreteParameters[1].CurrentValue = value;
			}
		}

		[XmlIgnore]
		public float MinRadius
		{
			get
			{
				return (float)(_discreteParameters[2] as FloatDiscreteParameter).CurrentValue;
			}
			set
			{
				_discreteParameters[2].CurrentValue = value;
			}
		}

		[XmlIgnore]
		public float MaxRadius
		{
			get
			{
				return (float)(_discreteParameters[3] as FloatDiscreteParameter).CurrentValue;
			}
			set
			{
				_discreteParameters[3].CurrentValue = value;
			}
		}

		[XmlIgnore]
		public float MaxAddingRadiusErrorRatio
		{
			get
			{
				return (float)(_discreteParameters[4] as FloatDiscreteParameter).CurrentValue;
			}
			set
			{
				_discreteParameters[4].CurrentValue = value;
			}
		}

		[XmlIgnore]
		public float MaxFoundRadiusErrorRatio
		{
			get
			{
				return (float)(_discreteParameters[5] as FloatDiscreteParameter).CurrentValue;
			}
			set
			{
				_discreteParameters[5].CurrentValue = value;
			}
		}


		[XmlIgnore]
		public float MaxAddingTanResidualStd
		{
			get
			{
				return (float)(_discreteParameters[6] as FloatDiscreteParameter).CurrentValue;
			}
			set
			{
				_discreteParameters[6].CurrentValue = value;
			}
		}

		[XmlIgnore]
		public float MaxFoundTanResidualStd
		{
			get
			{
				return (float)(_discreteParameters[7] as FloatDiscreteParameter).CurrentValue;
			}
			set
			{
				_discreteParameters[7].CurrentValue = value;
			}
		}


		[XmlIgnore]
		public float FirstPercentageValue
		{
			get
			{
				return (float)(_discreteParameters[8] as FloatDiscreteParameter).CurrentValue;
			}
			set
			{
				_discreteParameters[8].CurrentValue = value;
			}
		}

		[XmlIgnore]
		public float FirstPercentageMinEcc
		{
			get
			{
				return (float)(_discreteParameters[9] as FloatDiscreteParameter).CurrentValue;
			}
			set
			{
				_discreteParameters[9].CurrentValue = value;
			}
		}

		[XmlIgnore]
		public int FirstPercentageMinPoints
		{
			get
			{
				return (int)(_discreteParameters[10] as IntDiscreteParameter).CurrentValue;
			}
			set
			{
				_discreteParameters[10].CurrentValue = value;
			}
		}



		public ArcParam():base(TOPOLOGY.ARC)
		{
			_discreteParameters = 
				new DiscreteParameter[11] {
											  new IntDiscreteParameter("Initial number of points",new string[] {"Many","Normal","Few"},1,1,new int[] {3,2,1}),
											  new IntDiscreteParameter("Minimum number of points",new string[] {"Many","Normal","Few"},1,7,new int[] {9,7,5}),
											  new FloatDiscreteParameter("Minimum Radius",new string[] {"High","Medium","Low"},1,20000f,new float[]  {100000f,75000f,30000f}),
											  new FloatDiscreteParameter("Maximum Radius",new string[] {"High","Medium","Low"},1,75000f,new float[] {450000f,300000f,200000f}),
											  new FloatDiscreteParameter("Maximum Adding StdR/R Ratio ",new string[] {"High","Medium","Low"},1,0.1f,new float[] {0.2f,0.1f,0.05f}),
											  new FloatDiscreteParameter("Maximum Found StdR/R Ratio ",new string[] {"High","Medium","Low"},1,0.05f,new float[] {0.1f,0.05f,0.01f}),
											  new FloatDiscreteParameter("Maximum Adding Tangential Residual ",new string[] {"High","Medium","Low"},1,3f,new float[] {0.1f,0.08f,0.05f}),
											  new FloatDiscreteParameter("Maximum Found Tangential Residual ",new string[] {"High","Medium","Low"},1,1.5f,new float[] {0.1f,0.08f,0.05f}),
											  new FloatDiscreteParameter("1st Percentage Value ",new string[] {"High","Medium","Low"},1,80f,new float[] {1f,0.8f,0.5f}),
											  new FloatDiscreteParameter("1st Percentage Min Ecc ",new string[] {"High","Medium","Low"},1,0.8f,new float[] {0.9f,0.8f,0.7f}),
											  new IntDiscreteParameter("1st Percentage Min Points ",new string[] {"High","Medium","Low"},1,5,new int[] {8,5,3})
										  };
		}
		public void CopyFrom(ArcParam _src)
		{
			if (_src == null)
				return;
			for (int i=0;i<_discreteParameters.Length;i++)
			{
				if (_discreteParameters[i] != null)
				{
					_discreteParameters[i].Dispose();
				}
				_discreteParameters[i] = _src.Parameters[i].Clone();
			}
			this.RemoveAdjacentArcs = _src.RemoveAdjacentArcs;
			this.DetectTapped = _src.DetectTapped;
			this.CheckLocalDefectRatio = _src.CheckLocalDefectRatio;

			this.LConstant = _src.LConstant;
			this.VeryHighEcc = _src.VeryHighEcc;
			this.MaxDis2Ratio = _src.MaxDis2Ratio;
			this.MinDis2Ratio = _src.MinDis2Ratio;
			this.VeryFewKTan = _src.VeryFewKTan;
			this.RFixedError = _src.RFixedError;

			this.MinReducedLengthRatio = _src.MinReducedLengthRatio;
			this.MaxWidthLengthRatio = _src.MaxWidthLengthRatio;
			this.MinDistanceFromWaferCenter = _src.MinDistanceFromWaferCenter;
			
			this.MinLocalDefectRatio = _src.MinLocalDefectRatio;
			this.WidthLocalDefectRatio = _src.WidthLocalDefectRatio;
			this.WidthLocalFixed = _src.WidthLocalFixed;

			this.TappedAngle = _src.TappedAngle;
			this.TappedLengthRatio = _src.TappedLengthRatio;
			this.TappedN1Min = _src.TappedN1Min;

		}
		public override DiscreteParameter[] Parameters
		{
			get
			{
				return _discreteParameters;
			}
		}

		public override void Dispose()
		{
			foreach (DiscreteParameter _param in _discreteParameters)
			{
				if (_param == null)
					continue;
				_param.Dispose();
			}
			_discreteParameters = null;
		}

	}

	/*
	public class ManualParam : TopoParam
	{
		private DiscreteParameter[] _discreteParameters;
		public override void Dispose()
		{
			if (_discreteParameters == null)
				return;
			foreach (DiscreteParameter _param in _discreteParameters)
			{
				if (_param == null)
					continue;
				_param.Dispose();
			}
			_discreteParameters = null;			
		}

		public override DiscreteParameter[] Parameters
		{
			get
			{
				return _discreteParameters;
			}
		}

		public ManualParam():base(TOPOLOGY.MANUAL)
		{
			_discreteParameters = null;
		}
	}
	*/
	#endregion

	#region TopologyFeatures


	public abstract class ScratchFeature : IDisposable
	{
		public ArrayList DefectList = null;
		public ArrayList BasePoints = null;
		public BasePoint BeginPoint = null;
		public BasePoint EndPoint = null;
		public double Confidence;

		public string Tag= string.Empty;

		public void CopyFrom(ScratchFeature _src)
		{
			if (_src.DefectList == null)
				this.DefectList = null;
			else
				this.DefectList = (ArrayList)_src.DefectList.Clone();
			
			if (_src.BasePoints == null)
				this.BasePoints = null;
			else
				this.BasePoints = (ArrayList)_src.BasePoints.Clone();
			
			this.BeginPoint = _src.BeginPoint;

			this.EndPoint = _src.EndPoint;
			this.Confidence = _src.Confidence;
			
			this.Tag = _src.Tag;
		}
		public void AddBasePoint(BasePoint basepoint)
		{
			if (basepoint == null)
				return;
			if (BasePoints == null)
				BasePoints = new ArrayList();
			BasePoints.Add(basepoint);
		}
		#region IDisposable Members

		public void Dispose()
		{
			if (DefectList != null)
			{
				DefectList.Clear();
				DefectList = null;
			}
			if (BasePoints != null)
			{
				BasePoints.Clear();
				BasePoints = null;
			}
			BeginPoint = null;
			EndPoint = null;
			Tag = string.Empty;
		}

		#endregion
	}
	public  class ArcFeature : ScratchFeature
	{
		public int NumPoints;
		public float X = 0f;
		public float Y = 0f;
		public float Radius = 0f;
		public float RadiusError = 0f;
		public float RadiusErrorRatio
		{
			get
			{
				if (Radius <= float.Epsilon)
					return float.PositiveInfinity;
				return RadiusError / Radius;
			}
		}
		public float TangentialResidualMean = 0f;
		public float TangentialResidualStd = 0f;
		public float StartAngle = 0f;
		public float SweepAngle = 360f;
		public float Width = 0f;
		public float Length
		{
			get
			{
				return (float)(Radius * Math.PI * SweepAngle / 180.0);
			}
		}

		public float N1 = 0f;
		public float N4 = 0f;
		public float NTap1 = 0f;
		public float NTap2 = 0f;
		public float NTap
		{
			get
			{
				return NTap1 + NTap2;
			}
		}
		public ArcFeature()
		{
		}
		public void CopyFrom(ArcFeature _src)
		{
			if (_src == null)
				return;
			this.NumPoints = _src.NumPoints;
			this.X = _src.X;
			this.Y = _src.Y;
			this.Radius = _src.Radius;
			this.RadiusError = _src.RadiusError;
			this.TangentialResidualMean = _src.TangentialResidualMean;
			this.TangentialResidualStd = _src.TangentialResidualStd;
			this.StartAngle = _src.StartAngle;
			this.SweepAngle = _src.SweepAngle;
			this.Width = _src.Width;

			this.N1 = _src.N1;
			this.N4 = _src.N4;
			this.NTap1 = _src.NTap1;
			this.NTap2 = _src.NTap2;

			base.CopyFrom(_src);
		}
	}
	public class ArcSparseFeature : ArcFeature
	{
	}

	public class LineFeature : ScratchFeature
	{
		public int NumPoints;

		private float _X0;
		private float _Y0;
		private float _X1;
		private float _Y1;
		public float X0
		{
			get
			{
				return _X0;
			}
		}
		public float Y0
		{
			get
			{
				return _Y0;
			}
		}
		public float X1
		{
			get
			{
				return _X1;
			}
		}
		public float Y1
		{
			get
			{
				return _Y1;
			}
		}


		private double _A;
		private double _B;
		private double _C;
		public double A
		{
			get
			{
				return _A;
			}
		}
		public double B
		{
			get
			{
				return _B;
			}
		}
		public double C
		{
			get
			{
				return _C;
			}
		}

		
		public float LocalTan;
		public float DistanceMean;
		public float DistanceStd;
		public float Width;
		public float Length
		{
			get
			{
				return (float)Math.Sqrt((_X0 - _X1)*(_X0 - _X1) + (_Y0 - _Y1)*(_Y0 - _Y1));
			}
		}

		/// <summary>
		///Compute normalized general equation of Line
		/// Ax+By+C = 0
		/// </summary>
		/// <returns></returns>
		private int ComputeABC()
		{
			//			A:     T[0][0] = -1/(sqrt(Y1*Y1-2.0*Y1*Y0+Y0*Y0+X1*X1-2.0*X1*X0+X0*X0))*(-Y1+Y0);
			//			B:     T[0][0] = 1/(sqrt(Y1*Y1-2.0*Y1*Y0+Y0*Y0+X1*X1-2.0*X1*X0+X0*X0))*(-X1+X0);
			//			C:     T[0][0] = -1/(sqrt(Y1*Y1-2.0*Y1*Y0+Y0*Y0+X1*X1-2.0*X1*X0+X0*X0))*X0*Y1+1/(sqrt(Y1*Y1-2.0*Y1*Y0+Y0*Y0+X1*X1-2.0*X1*X0+X0*X0))*Y0*X1;
			double dx = _X1 - _X0;
			double dy = _Y1 - _Y0;
			double _dem = 1.0 / Math.Sqrt(dx * dx + dy * dy);
			if (_dem < double.Epsilon)
				return 1;
			_A = _dem*dy;
			_B = - _dem*dx;
			_C = -(_A*_X0 + _B*_Y0);
			if (_A < 0.0)
			{
				_A = - _A;
				_B = - _B;
				_C = - _C;
			}
			return 0;
		}
		public float BoundTop
		{
			get
			{
				return Math.Min(_Y0,_Y1);
			}
		}
		public float BoundLeft
		{
			get
			{
				return Math.Min(_X0,_X1);
			}
		}
		public float BoundBottom
		{
			get
			{
				return Math.Max(_Y0,_Y1);
			}
		}
		public float BoundRight
		{
			get
			{
				return Math.Max(_X0,_X1);
			}
		}

		public int Update(float _x0, float _y0, float _x1, float _y1)
		{
			_X0 = _x0;
			_Y0 = _y0;
			_X1 = _x1;
			_Y1 = _y1;
			return ComputeABC();
		}
		public LineFeature()
		{
		}
		public LineFeature(PointF _p1, PointF _p2, float _distanceStd) : this (_p1.X, _p1.Y, _p2.X, _p2.Y,0f,0f, _distanceStd)
		{			
		}
		public LineFeature(float _x0, float _y0, float _x1, float _y1)
		{
			Update(_x0, _y0, _x1, _y1);
		}
		public LineFeature(float _x0, float _y0, float _x1, float _y1, float _localTan, float _distanceMean, float _distanceStd):
			this(_x0, _y0, _x1, _y1)
		{
			LocalTan = _localTan;
			DistanceMean = _distanceMean;
			DistanceStd = _distanceStd;
		}


		public LineFeature Clone()
		{
			LineFeature _result = new LineFeature(X0, Y0, X1, Y1, LocalTan, DistanceMean, DistanceStd);
			_result.Width = this.Width;
			_result.CopyFrom(this);
			return _result;
		}
		public double Distance(float x, float y)
		{
			return _A*x + _B*y + _C;
		}
		public bool CheckInner(float x, float y)
		{
			if (x < X0 || x > X1 || y < Y0 || y<Y1)
				return false;
			if (Distance(x,y) > Width)
				return false;

			return true;
		}
	}
	public class ManualScratchFeature : ScratchFeature
	{
		public ArrayList Directions = null;		
		int _maxPoints = 20;
		public int MaxSplinePoints
		{
			get
			{
				return _maxPoints;
			}
		}
		public double[,] SplineXCoefficients = null;
		public double[,] SplineYCoefficients = null;
		int _splineNPoints = 0;
		public int SplineNumPoints
		{
			get
			{
				return _splineNPoints;
			}
		}
				
		public ManualScratchFeature(int _max_points)
		{
			Directions = new ArrayList();
			_maxPoints = _max_points;
		}
		public void CopyFrom(ManualScratchFeature _src)
		{
			if (_src == null)
#if DEBUG
				throw new System.ArgumentException("You can't copy from null-object");
#endif
			if (Directions != null)
			{
				Directions.Clear();
				Directions = null;
			}
			if (_src.Directions != null)
				this.Directions = (ArrayList)_src.Directions.Clone();
#if DEBUG
			if (_src.MaxSplinePoints != this._maxPoints)
				Console.WriteLine("The MaxSplinePoints of source and destination objects are not the same");
#endif
			if (SplineXCoefficients == null)
				SplineXCoefficients = new double[MaxSplinePoints,4];
			else
				Array.Clear(SplineXCoefficients,0,SplineXCoefficients.Length);
			if (_src.SplineXCoefficients != null)
			{
				for (int i=0;i<_src.SplineNumPoints && i<this.MaxSplinePoints;i++)
				{
					for (int j=0;j<SplineXCoefficients.GetLength(1);j++)
					{
						SplineXCoefficients[i,j] = _src.SplineXCoefficients[i,j];
					}
				}
			}

			if (SplineYCoefficients == null)
				SplineYCoefficients = new double[MaxSplinePoints,4];
			else
				Array.Clear(SplineYCoefficients,0,SplineYCoefficients.Length);
			if (_src.SplineYCoefficients != null)
			{
				for (int i=0;i<_src.SplineNumPoints && i<this.MaxSplinePoints;i++)
				{
					for (int j=0;j<SplineYCoefficients.GetLength(1);j++)
					{
						SplineYCoefficients[i,j] = _src.SplineYCoefficients[i,j];
					}
				}
			}


			base.CopyFrom((ScratchFeature)_src);
		}
		public new void Dispose()
		{
			if (Directions != null)
			{
				Directions.Clear();
				Directions = null;
			}
			if (SplineXCoefficients != null)
				Array.Clear(SplineXCoefficients,0,SplineXCoefficients.Length);

			if (SplineYCoefficients != null)
				Array.Clear(SplineYCoefficients,0,SplineYCoefficients.Length);

			base.Dispose();
		}
	}
	public class ShortScratchFeature : ScratchFeature
	{
		public float Length;
		public float Width;
		public float Orientation;
		public PointF[] Convex;
		public ShortScratchFeature()
		{
		}
		public void CopyFrom(ShortScratchFeature _src)
		{
			if (_src == null)
				return;
			this.Length = _src.Length;
			this.Width = _src.Width;
			this.Orientation = _src.Orientation;
			this.Convex = (PointF[])_src.Convex.Clone();
			base.CopyFrom(_src);
		}
		public new void Dispose()
		{
			if (Convex != null)
			{
				Array.Clear(Convex,0,Convex.Length);
				Convex = null;
			}
			base.Dispose();
		}


	}
	#endregion

	#region Comparers
	public class ScratchFeatureComparers
	{
		public static ConfidentComparer ConfidentComparer
		{
			get
			{
				return new ConfidentComparer();
			}
		}
		public static LengthScratchComparer LengthScratchComparer
		{
			get
			{
				return new LengthScratchComparer();
			}
		}
		public static WidthScratchComparer WidthScratchComparer
		{
			get
			{
				return new WidthScratchComparer();
			}
		}
	}
	public class BasePointComparers
	{
		public static LengthComparer LengthComparer
		{
			get
			{
				return new LengthComparer();
			}
		}
		public static PointCountComparer PointCountComparer
		{
			get
			{
				return new PointCountComparer();
			}
		}

		public static XComparer XComparer
		{
			get
			{
				return new XComparer();
			}
		}
		public static YComparer YComparer
		{
			get
			{
				return new YComparer();
			}
		}
		public static EccentricityComparer EccentricityComparer
		{
			get
			{
				return new EccentricityComparer();
			}
		}
		public static OrientationComparer OrientationComparer
		{
			get
			{
				return new OrientationComparer();
			}
		}
		public static AbsOrientationComparer AbsOrientationComparer
		{
			get
			{
				return new AbsOrientationComparer();
			}
		}
		public static DensityComparer DensityComparer
		{
			get
			{
				return new DensityComparer();
			}
		}
	}
	public class LengthComparer: IComparer
	{
		#region IComparer Members

		public int Compare(object x, object y)
		{
#if CHECK_COMPATIBLE
			float _v_x = 0f;
			float _v_y = 0f;
			try
			{
				_v_x = (x as BasePoint).Length;
				_v_y = (y as BasePoint).Length;
			}
			catch
			{
				throw new System.ArgumentException("Operands are not BasePoint type a instance of BasePoint type");
			}
#else //for improving performance
			float _v_x = (x as BasePoint).Length;
			float _v_y = (y as BasePoint).Length;
#endif
			if (_v_x > _v_y)
				return 1;
			else if (_v_x <_v_y)
				return -1;
			else
				return 0;
		}

		#endregion

	}

	public class PointCountComparer: IComparer
	{
		#region IComparer Members

		public int Compare(object x, object y)
		{
#if CHECK_COMPATIBLE
			float _v_x = 0f;
			float _v_y = 0f;
			try
			{
				_v_x = (x as BasePoint).PointCount;
				_v_y = (y as BasePoint).PointCount;
			}
			catch
			{
				throw new System.ArgumentException("Operands are not BasePoint type a instance of BasePoint type");
			}
#else //for improving performance
			float _v_x = (x as BasePoint).PointCount;
			float _v_y = (y as BasePoint).PointCount;
#endif
			if (_v_x > _v_y)
				return 1;
			else if (_v_x <_v_y)
				return -1;
			else
				return 0;
		}

		#endregion

	}

	public class XComparer: IComparer
	{
		public static XComparer Comparer
		{
			get
			{
				return new XComparer();
			}
		}

		#region IComparer Members

		public int Compare(object x, object y)
		{
#if CHECK_COMPATIBLE
			float _v_x = 0f;
			float _v_y = 0f;
			try
			{
				_v_x = (x as BasePoint).X;
				_v_y = (y as BasePoint).X;
			}
			catch
			{
				throw new System.ArgumentException("Operands are not BasePoint type a instance of BasePoint type");
			}
#else //for improving performance
			float _v_x = (x as BasePoint).X;
			float _v_y = (y as BasePoint).X;
#endif
			if (_v_x > _v_y)
				return 1;
			else if (_v_x <_v_y)
				return -1;
			else
				return 0;
		}

		#endregion

	}
	public class YComparer: IComparer
	{
		#region IComparer Members

		public int Compare(object x, object y)
		{
#if CHECK_COMPATIBLE
			float _v_x = 0f;
			float _v_y = 0f;
			try
			{
				_v_x = (x as BasePoint).Y;
				_v_y = (y as BasePoint).Y;
			}
			catch
			{
				throw new System.ArgumentException("Operands are not BasePoint type a instance of BasePoint type");
			}
#else //for improving performance
			float _v_x = (x as BasePoint).Y;
			float _v_y = (y as BasePoint).Y;
#endif
			if (_v_x > _v_y)
				return 1;
			else if (_v_x <_v_y)
				return -1;
			else
				return 0;
		}

		#endregion

	}
	public class EccentricityComparer: IComparer
	{
		#region IComparer Members

		public int Compare(object x, object y)
		{
#if CHECK_COMPATIBLE
			float _v_x = 0f;
			float _v_y = 0f;
			try
			{
				_v_x = (x as EllipsePoint).Eccentricity;
				_v_y = (y as EllipsePoint).Eccentricity;
			}
			catch
			{
				throw new System.ArgumentException("Operands are not BasePoint type a instance of EllipsePoint type");
			}
#else //for improving performance
			float _v_x = (x as EllipsePoint).Eccentricity;
			float _v_y = (y as EllipsePoint).Eccentricity;
#endif
			if (_v_x > _v_y)
				return 1;
			else if (_v_x <_v_y)
				return -1;
			else
				return 0;
		}

		#endregion

	}

	public class OrientationComparer: IComparer
	{
		#region IComparer Members

		public int Compare(object x, object y)
		{
#if CHECK_COMPATIBLE
			float _v_x = 0f;
			float _v_y = 0f;
			try
			{
				_v_x = (x as EllipsePoint).Orientation;
				_v_y = (y as EllipsePoint).Orientation;
			}
			catch
			{
				throw new System.ArgumentException("Operands are not BasePoint type a instance of EllipsePoint type");
			}
#else //for improving performance
			float _v_x = (x as EllipsePoint).Orientation;
			float _v_y = (y as EllipsePoint).Orientation;
#endif
			if (_v_x > _v_y)
				return 1;
			else if (_v_x <_v_y)
				return -1;
			else
				return 0;
		}

		#endregion

	}

	public class AbsOrientationComparer: IComparer
	{
		#region IComparer Members

		public int Compare(object x, object y)
		{
#if CHECK_COMPATIBLE
			float _v_x = 0f;
			float _v_y = 0f;
			try
			{
				_v_x = Math.Abs((x as EllipsePoint).Orientation);
				_v_y = Math.Abs((y as EllipsePoint).Orientation);
			}
			catch
			{
				throw new System.ArgumentException("Operands are not BasePoint type a instance of EllipsePoint type");
			}
#else //for improving performance
			float _v_x = Math.Abs((x as EllipsePoint).Orientation);
			float _v_y = Math.Abs((y as EllipsePoint).Orientation);
#endif
			if (_v_x > _v_y)
				return 1;
			else if (_v_x <_v_y)
				return -1;
			else
				return 0;
		}
		#endregion

	}

	public class ShiftedOrientationComparer: IComparer
	{
		private  float _shiftAngle = 0f;
		public ShiftedOrientationComparer(float _shift)
		{
			_shiftAngle = _shift;
		}
		#region IComparer Members

		public int Compare(object x, object y)
		{
#if CHECK_COMPATIBLE
			float _v_x = 0f;
			float _v_y = 0f;
			try
			{
				_v_x = (x as EllipsePoint).Orientation;
				_v_y = (y as EllipsePoint).Orientation;
			}
			catch
			{
				throw new System.ArgumentException("Operands are not BasePoint type a instance of EllipsePoint type");
			}
#else //for improving performance		
			float _v_x = (x as EllipsePoint).Orientation;
			float _v_y = (y as EllipsePoint).Orientation;
#endif

			if (_v_x >= _shiftAngle)
				_v_x -= _shiftAngle;
			else
				_v_x = _v_x + 180f - _shiftAngle;

			if (_v_y >= _shiftAngle)
				_v_y -= _shiftAngle;
			else
				_v_y = _v_y + 180f - _shiftAngle;

			if (_v_x > _v_y)
				return 1;
			else if (_v_x <_v_y)
				return -1;
			else
				return 0;
		}

		#endregion
	}


	public class DensityComparer: IComparer
	{
		#region IComparer Members

		public int Compare(object x, object y)
		{
#if CHECK_COMPATIBLE
			float _v_x = 0f;
			float _v_y = 0f;
			try
			{
				_v_x = (x as EllipsePoint).Density;
				_v_y = (y as EllipsePoint).Density;
			}
			catch
			{
				throw new System.ArgumentException("Operands are not BasePoint type a instance of EllipsePoint type");
			}
#else //for improving performance
			float _v_x = (x as EllipsePoint).Density;
			float _v_y = (y as EllipsePoint).Density;
#endif
			if (_v_x > _v_y)
				return 1;
			else if (_v_x <_v_y)
				return -1;
			else
				return 0;
		}

		#endregion

	}

	public class ConfidentComparer: IComparer
	{
		#region IComparer Members

		public int Compare(object x, object y)
		{
#if CHECK_COMPATIBLE
			double _v_x = 0f;
			double _v_y = 0f;
			try
			{
				_v_x = (x as ScratchFeature).Confidence;
				_v_y = (y as ScratchFeature).Confidence;
			}
			catch
			{
				throw new System.ArgumentException("Operands are not BasePoint type a instance of EllipsePoint type");
			}
#else //for improving performance
			double _v_x = (x as ScratchFeature).Confidence;
			double _v_y = (y as ScratchFeature).Confidence;
#endif
			if (_v_x > _v_y)
				return 1;
			else if (_v_x <_v_y)
				return -1;
			else
				return 0;
		}

		#endregion

	}

	public class LengthScratchComparer: IComparer
	{
		#region IComparer Members

		public int Compare(object x, object y)
		{
#if CHECK_COMPATIBLE
			double _v_x = 0f;
			double _v_y = 0f;
			try
			{
				_v_x = (x as LineFeature).Length;
				_v_y = (y as LineFeature).Length;
			}
			catch
			{
				throw new System.ArgumentException("Operands are not BasePoint type a instance of EllipsePoint type");
			}
#else //for improving performance
			double _v_x = (x as LineFeature).Length;
			double _v_y = (y as LineFeature).Length;
#endif
			if (_v_x > _v_y)
				return 1;
			else if (_v_x <_v_y)
				return -1;
			else
				return 0;
		}
		#endregion
	}
	public class WidthScratchComparer: IComparer
	{
		#region IComparer Members

		public int Compare(object x, object y)
		{
#if CHECK_COMPATIBLE
			double _v_x = 0f;
			double _v_y = 0f;
			try
			{
				_v_x = (x as LineFeature).Width;
				_v_y = (y as LineFeature).Width;
			}
			catch
			{
				throw new System.ArgumentException("Operands are not BasePoint type a instance of EllipsePoint type");
			}
#else //for improving performance
			double _v_x = (x as LineFeature).Width;
			double _v_y = (y as LineFeature).Width;
#endif
			if (_v_x > _v_y)
				return 1;
			else if (_v_x <_v_y)
				return -1;
			else
				return 0;
		}
		#endregion
	}
	public class BasePointsLengthComparer : IComparer
	{
		#region IComparer Members

		public int Compare(object x, object y)
		{
#if CHECK_COMPATIBLE
			int _v_x = 0;
			int _v_y = 0;
			try
			{
				_v_x = (x as ScratchFeature).BasePoints.Count;
				_v_y = (x as ScratchFeature).BasePoints.Count;
			}
			catch
			{
				throw new System.ArgumentException("Operands are not BasePoint type a instance of EllipsePoint type");
			}
#else //for improving performance		
			int _v_x = 0, _v_y = 0;

			if (x != null && (x as ScratchFeature).BasePoints != null)
				_v_x = (x as ScratchFeature).BasePoints.Count;

			if (y != null && (y as ScratchFeature).BasePoints != null)
				_v_y = (y as ScratchFeature).BasePoints.Count;

#endif
			if (_v_x > _v_y)
				return 1;
			else if (_v_x <_v_y)
				return -1;
			else
				return 0;
		}

		#endregion

	}

}
#endregion
