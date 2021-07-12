using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Drawing;
using System.Drawing.Drawing2D;

using SiGlaz.Algorithms.Core;

namespace SiGlaz.ObjectAnalysis.Common
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

        public static bool Compare(double x, COMPARE_OPERATOR oper, double y)
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
            X = 0, // difference in X Coordinate
            Y, // difference in Y Coordinate
            D_E_XY, // center-to-center distance
            EllipseDistance, // point-to-point distance
            Eccentricity, // difference in eccentricity
            Major, // difference in major length
            Minor, // difference in minor length
            Orientation, // difference in orientation

            //Radius, // difference in radius from center
            //Angle, // difference in angle to center
            //MaxAngleDeviationToBaseLine, // max angular deviation from the center-to-center line		            

            //CircleEstimationRadius, // radius of the estimated circle
            //CircleEstimationDeviation, // radius variance of the estimated circle

            Elongation, // difference in elongation


            // detected object
            //RectBoundWidth, // ,{"difference in rectangle boundary width","pixel"}
            //RectBoundHeight, // ,{"difference in rectangle boundary height","pixel"}
            Area,    // ,{"difference in area","pixel^2"}
            Perimeter,    // ,{"difference in perimeter","pixel"}
            PixelCount,    // ,{"difference in pixel count",""}

                // intensity
            AverageIntensity,    // ,{"difference in average intensity","intensity"}
            //MinIntensity,    // ,{"difference in min intensity","intensity"}
            //MaxIntensity,    // ,{"difference in max intensity","intensity"}
            IntegratedIntensity,    // ,{"difference in integrated intensity","intensity"}
            ObjectType,    // ,{"is the same classified object type", ""}



			//the hidden dimensions
			DevPtPProjX,
			DevPtPProjY,
			NONE,
			ALLSQUARED,
			//Elongation,
			DieRelativeDistance,
			MaxHorizontalTolerance,
			MinHorizontalTolerance,
			MinMajor
		}
		[XmlIgnore]
		public static string[,] _humankeylist =  
            //new string[,]{
            //    {"difference in X-Centroid Coordinate","pixel"},
            //    {"difference in Y-Centroid Coordinate","pixel"},
            //    {"center-to-center distance","pixel"},
            //    {"point-to-point distance","pixel"},
            //    {"difference in eccentricity",""},
            //    {"difference in major length","pixel"},
            //    {"difference in minor length","pixel"},
            //    {"difference in orientation","degree(s)"},

            //    //{"difference in radius from center","pixel"},
            //    {"difference in angle to center","degree(s)"},
            //    {"max angular deviation from the center-to-center line","degree(s)"}
            //    //,{"radius of the estimated circle","pixel"}
            //    //,{"radius variance of the estimated circle","pixel"}
            //    ,{"difference in elongation",""}

            //    // detected object
            //    ,{"difference in rectangle boundary width","pixel"}
            //    ,{"difference in rectangle boundary height","pixel"}
            //    ,{"difference in area","pixel^2"}
            //    ,{"difference in perimeter","pixel"}
            //    ,{"difference in pixel count",""}

            //    // intensity
            //    ,{"difference in average intensity","intensity"}
            //    //,{"difference in min intensity","intensity"}
            //    //,{"difference in max intensity","intensity"}
            //    ,{"difference in integrated intensity","intensity"}
            //    ,{"same classified object type", ""}


            //    //,{"deviation of X project of Point to Point distance", "pixel"}
            //    //,{"deviation of Y project of Point to Point distance", "pixel"}
            //};
        new string[,]{
				{"difference in X-Centroid Coordinate","micron"},
				{"difference in Y-Centroid Coordinate","micron"},
				{"center-to-center distance","micron"},
				{"point-to-point distance","micron"},
				{"difference in eccentricity",""},
				{"difference in major length","micron"},
				{"difference in minor length","micron"},
				{"difference in orientation","degree(s)"},

				//{"difference in radius from center","pixel"},
				//{"difference in angle to center","degree(s)"},
				//{"max angular deviation from the center-to-center line","degree(s)"}
				//,{"radius of the estimated circle","pixel"}
				//,{"radius variance of the estimated circle","pixel"}
                //,
                {"difference in elongation",""}

                // detected object
                //,{"difference in rectangle boundary width","micron"}
                //,{"difference in rectangle boundary height","micron"}
                ,{"difference in area","micron^2"}
                ,{"difference in perimeter","micron"}
                ,{"difference in pixel count",""}

                // intensity
                ,{"difference in average intensity","intensity"}
                //,{"difference in min intensity","intensity"}
                //,{"difference in max intensity","intensity"}
                ,{"difference in integrated intensity","intensity"}
                ,{"same classified object type", ""}


				//,{"deviation of X project of Point to Point distance", "pixel"}
				//,{"deviation of Y project of Point to Point distance", "pixel"}
			};        

        public static bool IsTheSameCondition(int conditionId)
        {
            if (conditionId == (int)LHS_KEYS.ObjectType)
                return true;
            return false;
        }

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

            public bool IsIdentifyWidth(CONDITION other)
            {
                if (other == null)
                    return false;
                if (LHS != other.LHS)
                    return false;
                if (Operator != other.Operator)
                    return false;
                if (RHS != other.RHS)
                    return false;
                return true;
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
            return false;

            //return (key == LHS_KEYS.CircleEstimationDeviation ||
            //        key == LHS_KEYS.CircleEstimationRadius);
		}


		[XmlElement(typeof(MDCCParam.CONDITION))]
		public ArrayList Conditions = new ArrayList();

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

        public bool IsIdentifyWidth(ArrayList conditions, int dfsLevel)
        {
            if (DFSLevel != dfsLevel)
                return false;

            if (conditions == null)
                return false;

            if (Conditions.Count != conditions.Count)
                return false;

            int n = Conditions.Count;
            for (int i = 0; i < n; i++)
            {
                MDCCParam.CONDITION c1 = Conditions[i] as MDCCParam.CONDITION;
                MDCCParam.CONDITION c2 = conditions[i] as MDCCParam.CONDITION;

                if (c1 == c2)
                    continue;

                if (c1 == null && c2 != null)
                    return false;

                if (!c1.IsIdentifyWidth(c2))
                    return false;
            }

            return true;
        }
    }

	#endregion
}
