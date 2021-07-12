using System;
using System.IO;
using System.Collections;
using System.Xml.Serialization;
using System.Drawing;
using System.Drawing.Drawing2D;

using SiGlaz.Algorithms.Core;

namespace SiGlaz.Common
{
	public enum QUERY_TYPE : byte
	{
		NONE = 0,
		OR,
		AND
	}
	
	public interface IDefectNumber
	{
		int GetDefectNumber();
	}

	#region Float Compare
	public enum COMPARE_OPERATOR : byte
	{
		EQUAL = 0,
		NOTEQUAL,
		GREATER,
		GREATEREQUAL,
		LESS,
		LESSEQUAL,
		NONE
	}

	public sealed class QueryOperator
	{
		private static string[] _humanString = new string[]{
															   " equals ", 
															   " does not equal ",
															   " is greater than ",
															   " is greater than or equal to ",
															   " is less than ",
															   " is less than or equal to"
														   };
		public static string[] HumanStringList(COMPARE_OPERATOR _oper)
		{
			return _humanString;

		}
		public static string[] OperStringList(COMPARE_OPERATOR _oper)
		{
			return new string[]{
								   "==", 
								   "<>",
								   ">>",
								   ">=",
								   "<<",
								   "<="
							   };

		}

		public static string HumanString(COMPARE_OPERATOR _oper)
		{
//			return _humanString[(byte)_oper];
			switch (_oper)
			{
				case COMPARE_OPERATOR.NONE:
					return string.Empty;
				case COMPARE_OPERATOR.EQUAL:
					return " equals ";
				case COMPARE_OPERATOR.NOTEQUAL:
					return " does not equal ";
				case COMPARE_OPERATOR.GREATER:
					return " is greater than ";
				case COMPARE_OPERATOR.GREATEREQUAL:
					return " is greater than or equal to ";
				case COMPARE_OPERATOR.LESS:
					return " is less than ";
				case COMPARE_OPERATOR.LESSEQUAL:
					return " is less than or equal to ";
				default:
					throw new System.ArgumentException("Invalid Comparision Operator");
			}
		}
		public static string OperString(COMPARE_OPERATOR _oper)
		{
			switch (_oper)
			{
				case COMPARE_OPERATOR.NONE:
					return string.Empty;
				case COMPARE_OPERATOR.EQUAL:
					return "=";
				case COMPARE_OPERATOR.NOTEQUAL:
					return "<>";
				case COMPARE_OPERATOR.GREATER:
					return ">";
				case COMPARE_OPERATOR.GREATEREQUAL:
					return ">=";
				case COMPARE_OPERATOR.LESS:
					return "<";
				case COMPARE_OPERATOR.LESSEQUAL:
					return "<=";
				default:
					throw new System.ArgumentException("Invalid Comparision Operator");
			}
		}
		public static COMPARE_OPERATOR GetOper(string _oper)
		{
			switch (_oper)
			{
				case "=":
					return COMPARE_OPERATOR.EQUAL;
				case "<>":
					return COMPARE_OPERATOR.NOTEQUAL;
				case ">":
					return COMPARE_OPERATOR.GREATER;
				case ">=":
					return COMPARE_OPERATOR.GREATEREQUAL;
				case "<":
					return COMPARE_OPERATOR.LESS;
				case "<=":
					return COMPARE_OPERATOR.LESSEQUAL;
				default:
					throw new System.ArgumentException("Invalid Comparision Operator");
			}
		}
		public static bool Compare(float x, COMPARE_OPERATOR oper, float y)
		{
			switch (oper)
			{
				case COMPARE_OPERATOR.EQUAL:
					return x == y;
				case COMPARE_OPERATOR.NOTEQUAL:
					return x != y;
				case COMPARE_OPERATOR.GREATER:
					return x > y;
				case COMPARE_OPERATOR.GREATEREQUAL:
					return x >= y;
				case COMPARE_OPERATOR.LESS:
					return x < y;
				case COMPARE_OPERATOR.LESSEQUAL:
					return x <= y;
				default:
					throw new System.ArgumentException("Invalid operator");
			}
		}
	}

	#endregion

	#region String compare
	public enum STRING_COMPARE_OPERATOR : byte
	{
		EQUAL = 0,
		NOTEQUAL,
		LIKE,
		NOTLINE,
		NONE
	}
	#endregion

	#region MDCC Condition
	public class MDCCParam : ICloneable,IHumanCondition
	{
		public enum LHS_KEYS
		{
			X=0,
			Y,
			D_E_XY,
			EllipseDistance,
			Eccentricity,
			Major,
			Minor,
			Orientation,
			Radius,
			Angle,
			MaxAngleDeviationToBaseLine,
			CircleEstimationRadius,
			CircleEstimationDeviation,
			//the hidden dimensions
			DevPtPProjX,
			DevPtPProjY,
			NONE,
			ALLSQUARED,
			Elongation,
			DieRelativeDistance,
			MaxHorizontalTolerance,
			MinHorizontalTolerance,
			MinMajor
		}
		[XmlIgnore]
		public static string[,] _humankeylist =  new string[,]{
																{"difference in X Coordinate","µm"},
																{"difference in Y Coordinate","µm"},
																{"center-to-center distance","µm"},
																{"point-to-point distance","µm"},
																{"difference in eccentricity",""},
																{"difference in major length","µm"},
																{"difference in minor length","µm"},
																{"difference in major axis orientation","degree(s)"},
																{"difference in radius from wafer center","µm"},
																{"difference in angle to wafer center","degree(s)"},
																{"max angular deviation from the center-to-center line","degree(s)"}
																,{"radius of the estimated circle","µm"}
																,{"radius variance of the estimated circle","µm"}
																//,{"deviation of X project of Point to Point distance", "µm"}
																//,{"deviation of Y project of Point to Point distance", "µm"}
															};
		public class CONDITION
		{
			public LHS_KEYS LHS;
			public COMPARE_OPERATOR Operator;
			public float RHS;
			public CONDITION() : this(LHS_KEYS.NONE,COMPARE_OPERATOR.LESS,0f)
			{
			}
			public CONDITION(LHS_KEYS key,COMPARE_OPERATOR oper,float rhs)
			{
				LHS = key;
				Operator = oper;
				RHS = rhs;
			}
		}
	
		public bool Used = false;
		public bool IsAndCondition = true;
		/// <summary>
		/// The level of Deep First Search [2..3]. Default = 2.
		/// </summary>
		public int DFSLevel = 2;

		[XmlIgnore]
		public float DieSizeX;

		[XmlIgnore]
		public float DieSizeY;

		public int FixedXProjPtP;
		public int FixedYProjPtP;

		public bool CheckConditionIsExtension()
		{
			if (Conditions == null || Conditions.Count == 0)
				return false;
			foreach (CONDITION condition in Conditions)
			{
				if (CheckConditionIsExtension(condition.LHS))
					return true;
			}
			return false;
		}
		public static bool CheckConditionIsExtension(LHS_KEYS key)
		{
			return (key == LHS_KEYS.CircleEstimationDeviation ||
					key == LHS_KEYS.CircleEstimationRadius);
		}


		[XmlElement(typeof(MDCCParam.CONDITION))]
		public ArrayList Conditions;

		public MDCCParam()
		{
		}

		public bool Serialize(string fn)
		{
			FileStream fs=null;
			try
			{
				fs=new FileStream(fn,FileMode.Create);
				XmlSerializer s = new XmlSerializer(typeof(MDCCParam));
				s.Serialize(fs,this);
				return true;
			}
			catch
			{
				return false;
			}
			finally
			{
				if(fs!=null)
					fs.Close();
			}
		}
		
		public static MDCCParam Deserialize(string fn)
		{
			FileStream fs=null;
			XmlSerializer s =null;
			try
			{
				fs=new FileStream(fn,FileMode.Open);
				s = new XmlSerializer(typeof( MDCCParam));
				MDCCParam Sctrl = (MDCCParam) s.Deserialize(fs);				
				return Sctrl;
			}
			catch
			{				
				return null;
			}
			finally
			{
				if(s!=null)
				{
					s=null;
				}
				if(fs!=null)
					fs.Close();
			}
		}

		#region ICloneable Members

		public object Clone()
		{
			MDCCParam result = (MDCCParam)MemberwiseClone();
			if (this.Conditions != null)
			{
				result.Conditions = new ArrayList(this.Conditions.Count);
				foreach (CONDITION _cond in this.Conditions)
				{
					result.Conditions.Add(new CONDITION(_cond.LHS,_cond.Operator,_cond.RHS));
				}
			}
			return result;
		}
		
		#endregion


        #region IHumanCondition Members

        public string GetHumanString()
        {
            throw new NotImplementedException();
        }

        public string[] GetKeyList()
        {
            throw new NotImplementedException();
        }

        public string[] GetHumanKeyList()
        {
            int keyListCount = _humankeylist.GetLength(0);
            string[] result = new string[keyListCount];
            for (int i = 0; i < keyListCount; i++)
            {
                result[i] = _humankeylist[i, 0];
            }

            return result;
        }

        public string[] GetHumanUnitList()
        {
            int keyListCount = _humankeylist.GetLength(0);
            string[] result = new string[keyListCount];
            for (int i = 0; i < keyListCount; i++)
            {
                result[i] = _humankeylist[i, 1];
            }

            return result;
        }

        #endregion
    }

	#endregion

	#region DefectNumber Comparer
	public class DefectNumberComparer : IComparer
	{
		#region IComparer Members

		public int Compare(object x, object y)
		{
			if (x == null && y == null)
				return 0;
			if (x == null && y != null)
				return -1;
			if (x != null && y == null)
				return 1;
			int x1 = (x as IDefectNumber).GetDefectNumber();
			int y1 = (y as IDefectNumber).GetDefectNumber();
			if (x1 == y1)
				return 0;
			if (x1 > y1)
				return 1;
			return - 1;
		}

		#endregion
	}
	#endregion
}
