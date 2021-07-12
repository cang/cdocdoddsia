using System;
using System.Collections;

namespace SiGlaz.Common
{
	public enum TRAINING_METHOD : byte
	{
		WAFER_AS_SAMPLE = 0,
		USING_LIBRARY,
		SCAN_ALL,
		MANUAL
	}
	public enum DETECT_METHOD : byte
	{
		SIMILARITY = 0,
		TOPOLOGY
	}
	/// <summary>
	/// Summary description for D2Features.
	/// </summary>
	public class D2Features
	{
		#region Common Members
		public static readonly string Version = "1.0F";
		public static readonly float WaferStandardRadius = 350000f;
		public static int Rank
		{
			get
			{
				if (VariableList == null)
					return 0;
				return VariableList.Length;
			}
		}
		public object this[string _fieldname]
		{
			get
			{
				return GetValue(_fieldname);
			}
			set
			{
				SetValue(_fieldname,value);
			}
		}
		public object this[int i]
		{
			get
			{
				if (i > Rank)
					return null;
				return this[VariableList[i]];
			}
		}
		public static float[,] GetFeatureMatrix(ArrayList alFeatures)
		{
			if (alFeatures == null || alFeatures.Count == 0)
				return null;
			float [,] result = new float[alFeatures.Count,Rank];
			for (int i= 0;i<alFeatures.Count;i++)
				for (int j = 0;j<Rank;j++)
					result[i,j] = (float)(alFeatures[i] as D2Features).GetValue(VariableList[j]);
			return result;
		}

		#endregion

		#region Specific Members
		public int ID;
		public float MajorAxis;
		public float MinorAxis;
		private float _ecc;
		public float Ecc
		{
			get
			{
				return _ecc;
			}
		}
		public float Orient;
		public float Density;
		public float FromCenterRadius;
		public float FromCenterAngle;
		private float _X;
		public float X
		{
			get
			{
				return _X;
			}
		}
		private float _Y;
		public float Y
		{
			get
			{
				return _Y;
			}
		}
		public float _BasePoint;
		public float BasePoint
		{
			get
			{
				return _BasePoint;
			}
		}
		public D2Features(): this(0,0f,0f,0f,0f,0f,0f)
		{
		}
		public void UpdateRelDimension()
		{
			//Calculate Eccentricity
			if (MajorAxis == 0 || MinorAxis > MajorAxis)
				_ecc = float.NaN;
			else
				_ecc = (float)Math.Sqrt(1 - (MinorAxis/MajorAxis)*(MinorAxis / MajorAxis));

			//Calculate X
			_X = (float)(FromCenterRadius * Math.Cos(FromCenterAngle / 180 * Math.PI));

			//Calculate X
			_Y = (float)(FromCenterRadius * Math.Sin(FromCenterAngle / 180 * Math.PI));

			//Calculate base point
			if (FromCenterRadius > WaferStandardRadius)
				_BasePoint = float.NaN;
			else
			{
				double _cosa = Math.Cos(Orient);
				double _sina = Math.Sin(Orient);
				if (Math.Abs(_cosa) > Math.Abs(_sina))
				{
					double _a = _sina / _cosa;
					double _b = Y - _a * X;
					double _xBasePoint = -_a*_b + Math.Sqrt(WaferStandardRadius * WaferStandardRadius * (_a * _a + 1) - _b*_b);
					double _yBasePoint = _a * _xBasePoint + _b;
					_BasePoint = (float)Math.Atan2(_yBasePoint, _xBasePoint);
				}
				else
				{
					double _a = _cosa / _sina;
					double _b = X - _a * Y;
					double _yBasePoint = -_a*_b + Math.Sqrt(WaferStandardRadius * WaferStandardRadius * (_a * _a + 1) - _b*_b);
					double _xBasePoint = _a * _yBasePoint + _b;
					_BasePoint = (float)Math.Atan2(_yBasePoint, _xBasePoint);
				}
			}
		}
		public D2Features(int _id, float _major, float _minor, float _orient, float _density, float _radius, float _angle)
		{
			ID = _id;
			MajorAxis = _major;
			MinorAxis = _minor;
			Orient = _orient;
			Density = _density;
			FromCenterRadius = _radius;
			FromCenterAngle = _angle;
			UpdateRelDimension();
		}
		public static readonly string [] VariableList = new string[] {
																		"MAJOR_AXIS_LENGTH",
																		 "MINOR_AXIS_LENGTH",
																		"ECCENTRICITY",
																		"ORIENTATION",
																		"DENSITY",
																		"FROM_CENTER_RADIUS",
																		"FROM_CENTER_ANGLE",
																		"X",
																		"Y",
																		"BASE_POINT"};

//		public static readonly float [] LowerBound = new float[] {
//																		0,		//"MAJOR_AXIS_LENGTH",
//																		0,		//"ECCENTRICITY",
//																		-90,	//"ORIENTATION",
//																		0,		//"DENSITY",
//																		0,		//"FROM_CENTER_RADIUS",
//																		-180	//"FROM_CENTER_ANGLE"
//																 };
//		public static readonly float [] UpperBound = new float[] {
//																	 float.PositiveInfinity,		//MAJOR_AXIS_LENGTH,
//																	 1,		//ECCENTRICITY,
//																	 90,	//ORIENTATION,
//																	 float.PositiveInfinity,//DENSITY,
//																	 150000,		//FROM_CENTER_RADIUS - micron
//																	 180	//FROM_CENTER_ANGLE
//																 };

		public object GetValue(string _fieldname)
		{
			switch (_fieldname)
			{
				case "MAJOR_AXIS_LENGTH":
					return this.MajorAxis;
				case "MINOR_AXIS_LENGTH":
					return this.MinorAxis;
				case "ECCENTRICITY":
					return this.Ecc;
				case "ORIENTATION":
					return this.Orient;
				case "DENSITY":
					return this.Density;
				case "FROM_CENTER_RADIUS":
					return this.FromCenterRadius;
				case "FROM_CENTER_ANGLE":
					return this.FromCenterAngle;
				case "X":
					return this.X;
				case "Y":
					return this.Y;
				case "BASE_POINT":
					return this.BasePoint;
				default:
					return null;
			}
		}
		public void SetValue(string _fieldname, object _value)
		{
			switch (_fieldname)
			{
				case "MAJOR_AXIS_LENGTH":
					this.MajorAxis = (float)_value;
					break;
				case "MINOR_AXIS_LENGTH":
					this.MinorAxis = (float)_value;
					break;
				case "ORIENTATION":
					this.Orient = (float)_value;
					break;
				case "DENSITY":
					this.Density = (float)_value;
					break;
				case "FROM_CENTER_RADIUS":
					this.FromCenterRadius = (float)_value;
					break;
				case "FROM_CENTER_ANGLE":
					this.FromCenterAngle = (float)_value;
					break;
				default:
					return;
			}
		}

		#endregion

		#region Serialize
		public void SerializeBinary(System.IO.BinaryWriter bw)
		{
			for (int i=0;i<D2Features.VariableList.Length;i++)
			{
				bw.Write((float)this[D2Features.VariableList[i]]);
			}
		}
		public static D2Features DeserializeBinary(System.IO.BinaryReader br, string _version)
		{
			if (_version != D2Features.Version)
				throw new System.Exception("Deserializing D2Features is interrupt. Version conflict.");

			D2Features _d2F = new D2Features();
			for (int i=0;i<D2Features.VariableList.Length;i++)
			{
				_d2F[D2Features.VariableList[i]] = br.ReadSingle();
			}
			_d2F.UpdateRelDimension();
			return _d2F;
		}
		public D2Features Copy()
		{
			D2Features result = new D2Features();
			foreach (string _fname in D2Features.VariableList)
			{
				result[_fname] = this[_fname];
			}
			result.UpdateRelDimension();
			return result;			
		}
		#endregion

	}

	public class D3Features : IComparable
	{
		public static readonly string Version = "1.0F";
		
		#region D3 Specific Properties Struct
		public struct D3StatsProps
		{
			public float Min;
			public float Max;
			public float Mean;
			public float Std;
			public D3StatsProps(float _min, float _max, float _mean, float _std)
			{
				Min = _min;
				Max = _max;
				Mean = _mean;
				Std = _std;
			}
		}

		#endregion

		public D3StatsProps[] StatsProps = null;
		public string ImageFileName = string.Empty;
		public int CategoryID;

		private ArrayList _alD2FeaturePoints = null;
		private string [] _varList = null;
		private int _rank = 0;

		public D3Features()  : base()
		{
			_varList = D3Features.VariableList();
			_rank = _varList.Length;
			StatsProps = new D3StatsProps[_rank];
		}
		public static string[] VariableList()
		{
			string [] result = new string[D2Features.VariableList.Length];
			Array.Copy(D2Features.VariableList,result,D2Features.VariableList.Length);
			return result;			
		}
		public ArrayList D2FeaturePointList
		{
			get
			{
				return _alD2FeaturePoints;
			}
			set
			{
				if (value != null &&
					value.Count > 0 &&
					value[0].GetType() != typeof(D2Features))
					return;
				_alD2FeaturePoints = value;
				if (value == null)
					return;
				_alD2FeaturePoints.TrimToSize();
//				GetFeatureFromObject(_alD2FeaturePoints);
			}
		}
		public int PointCount
		{
			get
			{
				if (_alD2FeaturePoints == null)
					return 0;
				return _alD2FeaturePoints.Count;
			}
		}
		private float _radius;
		private float _overallDensity;
		public void ComputeRadiusAndDensity()
		{
			if (_alD2FeaturePoints == null || _alD2FeaturePoints.Count == 0)
			{
				_radius = 0f;
				_overallDensity = 0f;
				return;
			}
			else
			{
				float _area_overall = 0f, _area = 0f, _nDef_overall = 0f, _nDef = 0f;

				float _minX = float.MaxValue,
					_minY = float.MaxValue,
					_maxX = float.MinValue,
					_maxY = float.MinValue;
				float _x, _y;
				for (int i=0;i<_alD2FeaturePoints.Count;i++)
				{
					_x = Convert.ToSingle((_alD2FeaturePoints[i] as D2Features)["X"]);
					_y = Convert.ToSingle((_alD2FeaturePoints[i] as D2Features)["Y"]);
					if (_minX > _x) _minX = _x;
					if (_maxX < _x) _maxX = _x;
					if (_minY > _y) _minY = _y;
					if (_maxY < _y) _maxY = _y;

					_area = (float)(Convert.ToDouble((_alD2FeaturePoints[i] as D2Features)["MAJOR_AXIS_LENGTH"]) *
						Convert.ToDouble((_alD2FeaturePoints[i] as D2Features)["MINOR_AXIS_LENGTH"]) * Math.PI);
					_nDef = Convert.ToSingle((_alD2FeaturePoints[i] as D2Features)["DENSITY"]) * _area;
					_area_overall += _area;
					_nDef_overall += _nDef;
				}
				_radius = (float)Math.Sqrt((_maxX - _minX)*(_maxX - _minX) + (_maxY - _minY)*(_maxY - _minY)) +
					Convert.ToSingle((_alD2FeaturePoints[0] as D2Features)["MAJOR_AXIS_LENGTH"]);
				_overallDensity = _nDef_overall / _area_overall;
			}
		}
		public float Radius
		{
			get
			{
				return _radius;
			}
		}
		public float OverallDensity
		{
			get
			{
				return _overallDensity;
			}
		}

		public int GetFeatureFromObject()
		{
			return GetFeatureFromObject(_alD2FeaturePoints);
		}
		public int GetFeatureFromObject(D2Features[] _objD2Features)
		{
			if (_objD2Features == null || _objD2Features.Length == 0)
			{
#if TRACE
				Console.WriteLine("Object is empty");
#endif
				return 0;
			}
			if (StatsProps == null)
			{
#if TRACE
				Console.WriteLine("Property array is not created");
#endif
				return 1;
			}

			float _min, _max, _mean, _std, _tmp;
			for (int j=0; j< D2Features.Rank; j++)
			{
				_min = float.MaxValue;
				_max = float.MinValue;
				_mean = 0;
				_std = 0;
				for (int i=0; i<_objD2Features.Length; i++)
				{
					_tmp = (float)_objD2Features[i][D2Features.VariableList[j]];
					if (_tmp < _min) _min = _tmp;
					if (_tmp > _max) _max = _tmp;
					_mean += _tmp;
					_std += _tmp*_tmp;
				}
				_mean /= _objD2Features.Length;
				_std /= _objD2Features.Length; _std -= _mean*_mean;
				if (_std <= 0f)
					_std = 0f;
				else
					_std = (float)Math.Sqrt(_std);
				StatsProps[j] = new D3StatsProps(_min,_max,_mean, _std);
			}
			return 0;
		}
		public int GetFeatureFromObject(ArrayList _objD2Features)
		{
			if (_objD2Features == null || _objD2Features.Count == 0)
			{
#if TRACE
				Console.WriteLine("Object is empty");
#endif
				return 0;
			}
			if (StatsProps == null)
			{
#if TRACE
				Console.WriteLine("Property array is not created");
#endif
				return 1;
			}

			float _min, _max, _mean, _std, _tmp;
			for (int j=0; j< D2Features.Rank; j++)
			{
				_min = float.MaxValue;
				_max = float.MinValue;
				_mean = 0;
				_std = 0;
				for (int i=0; i<_objD2Features.Count; i++)
				{
					_tmp = (float)(_objD2Features[i] as D2Features)[D2Features.VariableList[j]];
					if (_tmp < _min) _min = _tmp;
					if (_tmp > _max) _max = _tmp;
					_mean += _tmp;
					_std += _tmp*_tmp;
				}
				_mean /= _objD2Features.Count;
				_std /= _objD2Features.Count; _std -= _mean*_mean; 
				if (_std <= 0f)
					_std = 0f;
				else
					_std = (float)Math.Sqrt(_std);
				StatsProps[j] = new D3StatsProps(_min,_max,_mean, _std);
			}
			ComputeRadiusAndDensity();
			return 0;
		}
		public D3StatsProps GetStatsProps(string _fName)
		{
			int _fID = Array.IndexOf(D2Features.VariableList, _fName);
			if (_fID < 0)
				throw new System.ArgumentException(string.Format("Field name {0} is not exist.", _fName));
			return StatsProps[_fID];
		}
			 
		#region Serialize
		public void SerializeBinary(System.IO.BinaryWriter bw)
		{
			bw.Write(ImageFileName);

			if (_alD2FeaturePoints == null)
				bw.Write((int)0);
			else
			{
				bw.Write((int)_alD2FeaturePoints.Count);		
				foreach (D2Features _d2F in _alD2FeaturePoints)
				{
					_d2F.SerializeBinary(bw);
				}
			}
		
			for (int j=0;j<StatsProps.Length;j++)
			{
				bw.Write(StatsProps[j].Min);
				bw.Write(StatsProps[j].Max);
				bw.Write(StatsProps[j].Mean);
				bw.Write(StatsProps[j].Std);
			}

		}
		public static D3Features DeserializeBinary(System.IO.BinaryReader br, string _version)
		{
			if (_version != D3Features.Version)
				throw new System.Exception("Deserialize D3Feature is interrupt. Version conflict");

			D3Features _d3F = new D3Features();
			_d3F.ImageFileName = br.ReadString();
			int _count = br.ReadInt32();
			ArrayList _alD2FeaturePoints = new ArrayList(_count);
			for (int i=0;i<_count;i++)
			{
				_alD2FeaturePoints.Add(D2Features.DeserializeBinary(br,D3Features.Version));
			}
			_d3F.D2FeaturePointList = _alD2FeaturePoints;
			for (int j=0;j<_d3F.StatsProps.Length;j++)
			{
				_d3F.StatsProps[j].Min = br.ReadSingle();
				_d3F.StatsProps[j].Max = br.ReadSingle();
				_d3F.StatsProps[j].Mean = br.ReadSingle();
				_d3F.StatsProps[j].Std = br.ReadSingle();
			}
			_d3F.ComputeRadiusAndDensity();
			return _d3F;
		}
		#endregion

		#region IComparable Members

		public int CompareTo(object obj)
		{
			if (obj == null)
				return 1;
			if (obj.GetType() != typeof(D3Features))
				throw new System.ArgumentException("Can not compare two object of different types.");
			if (this.PointCount > (obj as D3Features).PointCount)
				return 1;
			else if (this.PointCount < (obj as D3Features).PointCount)
				return -1;
			else
				return 0;
		}

		#endregion
	}
}
