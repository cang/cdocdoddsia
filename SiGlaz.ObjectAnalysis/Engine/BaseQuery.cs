using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

using SiGlaz.Common.Object;
using SiGlaz.ObjectAnalysis.Common;

namespace SiGlaz.ObjectAnalysis.Engine
{
    public abstract class BaseQuery
    {
        public struct CONDITION
        {
            public string LHS;
            public COMPARE_OPERATOR Operator;
            public float RHS;
            public CONDITION(string key, COMPARE_OPERATOR oper, float rhs)
            {
                LHS = key;
                Operator = oper;
                RHS = rhs;
            }
        }
        public abstract float GetValue(string key, object obj);

        private bool _defaultValue = false;
        public bool DefaultValue
        {
            get
            {
                return _defaultValue;
            }
            set
            {
                _defaultValue = value;
            }
        }

        private QUERY_TYPE queryType = QUERY_TYPE.NONE;
        public QUERY_TYPE QueryType
        {
            get
            {
                return queryType;
            }
            set
            {
                queryType = value;
            }
        }
        private ArrayList _1stLevel;

        [XmlElement(typeof(BaseQuery.CONDITION))]
        public ArrayList RootQuery
        {
            get
            {
                return _1stLevel;
            }
        }

        public bool ParseQuery(string query, QUERY_TYPE type)
        {
            if (type != QUERY_TYPE.OR && type != QUERY_TYPE.AND)
                return false;
            queryType = type;
            try
            {
                char[] _1st_splitter = null;
                char[] _2nd_splitter = null;
                if (type == QUERY_TYPE.AND)
                {
                    _1st_splitter = "&".ToCharArray();
                    _2nd_splitter = "|".ToCharArray();
                }
                else
                {
                    _1st_splitter = "|".ToCharArray();
                    _2nd_splitter = "&".ToCharArray();
                }
                Regex _regex = new Regex(@"(?<LHS>[^<>=]+)(?<oper>[<>=]{1,2})(?<RHS>[^<>=]+)");
                Match m = null;
                string[] _1st_level_string = query.Split(_1st_splitter);
                _1stLevel = new ArrayList(_1st_level_string.Length);
                foreach (string _string in _1st_level_string)
                {
                    string[] _2ndItem_strings = _string.Split(_2nd_splitter);
                    if (_2ndItem_strings == null || _2ndItem_strings.Length == 0)
                        continue;
                    ArrayList items = new ArrayList(_2ndItem_strings.Length);
                    foreach (string _s in _2ndItem_strings)
                    {
                        m = _regex.Match(_s);
                        if (!m.Success)
                            return false;
                        items.Add(new CONDITION(
                            m.Groups["LHS"].Value.Trim(),
                            QueryOperator.GetOper(m.Groups["oper"].Value),
                            float.Parse(m.Groups["RHS"].Value)));
                    }
                    _1stLevel.Add(items);
                }

            }
            catch
            {
                return false;
            }

            return true;
        }

        private static string ExpandText(Match m)
        {
            return " " + m.ToString() + " ";
        }
        public static string ConvertToOldTypeQuery(string query)
        {
            if (query == null || query == string.Empty)
                return string.Empty;
            Regex _reg = new Regex("[<>=|&]{1,2}");
            return _reg.Replace(query, new MatchEvaluator(ExpandText));
        }
        public static string ConvertFromOldTypeQuery(string query)
        {
            if (query == null || query == string.Empty)
                return string.Empty;
            return query.Replace(" ", "");
        }
        public bool CheckQuery(object obj)
        {
            if (queryType == QUERY_TYPE.NONE)
                throw new System.ArgumentException("Query is not initialized");
            if (_1stLevel == null || _1stLevel.Count == 0)
                return _defaultValue;

            if (queryType == QUERY_TYPE.AND)
            {
                foreach (ArrayList _2nd_qr in _1stLevel)
                {
                    bool _res = false;
                    foreach (CONDITION _cond in _2nd_qr)
                        if (_res = QueryOperator.Compare(
                            GetValue(_cond.LHS, obj), _cond.Operator, _cond.RHS))
                            break;
                    if (!_res)
                        return false;
                }
                return true;
            }
            else
            {
                foreach (ArrayList _2nd_qr in _1stLevel)
                {
                    bool _res = true;
                    foreach (CONDITION _cond in _2nd_qr)
                        if (!(_res = QueryOperator.Compare(
                            GetValue(_cond.LHS, obj), _cond.Operator, _cond.RHS)))
                            break;
                    if (_res)
                        return true;
                }
                return false;
            }
        }
    }



    #region ObjectQuery
    public class ObjectQuery : BaseQuery
    {
        public ObjectQuery()
            : base()
        {
        }
        public override float GetValue(string key, object cluster)
        {
            if (cluster.GetType() != typeof(EllipticalDensityShapeObject))
                throw new System.ArgumentException("The input object is not an instance of EllipticalDensityShapeObject type");
            return Convert.ToSingle((cluster as EllipticalDensityShapeObject)[key]);
        }
    }
    #endregion

}
