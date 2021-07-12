using System;
using System.Collections;

namespace SiGlaz.ObjectAnalysis.Engine
{
    /// <summary>
    /// Summary description for StringConditionParse.
    /// </summary>
    public class StringConditionParse
    {
        public struct ConditionItem
        {
            public string fieldname;
            public string criteria;
            public object value;
        }
        public StringConditionParse()
        {
        }

        /// <summary>
        /// ParseSimple_OR_AND_Clause
        /// </summary>
        /// <param name="contidion"></param>
        /// <returns>is a ArrayList of OR array</returns>
        public static ArrayList ParseSimple_OR_AND_Clause(string condition)
        {
            ArrayList alOr = new ArrayList();


            //Analyzer condition
            condition = condition.ToUpper();
            condition = condition.Replace("==", "=");
            condition = condition.Replace("<>", "!=");
            condition = condition.Replace(" OR ", " | ");
            condition = condition.Replace(" AND ", " & ");


            string[] sOr = condition.Split("|".ToCharArray());
            foreach (string subquery in sOr)
            {
                if (subquery.Trim() == "")
                    continue;
                ArrayList alAnd = new ArrayList();

                string[] sAnd = subquery.Split("&".ToCharArray());
                foreach (string sExp in sAnd)
                {
                    string fieldname = "";
                    string operation = "";
                    object obj = null;
                    try
                    {
                        GetExpressOfEndQuery(sExp, ref fieldname, ref operation, ref obj);
                    }
                    catch
                    {
                        return null;
                    }
                    ConditionItem item;
                    item.fieldname = fieldname.Trim().ToUpper();
                    item.criteria = operation.Trim();
                    item.value = obj;
                    alAnd.Add(item);
                }
                alOr.Add(alAnd);

                //Vu~ fix mem
                for (int i = 0; i < sAnd.Length; i++)
                    sAnd[i] = null;
                sAnd = null;
            }
            return alOr;
        }


        public static ArrayList ParseSimple_AND_OR_Clause(string condition)
        {
            string ss = condition.ToUpper();
            ss = ss.Replace(" AND ", " @ ");
            ss = ss.Replace(" OR ", " AND ");
            ss = ss.Replace(" @ ", " OR ");
            return ParseSimple_OR_AND_Clause(ss);
        }


        public static void GetExpressOfEndQuery(string expr, ref string fieldname, ref string operation, ref object value)
        {
            try
            {
                expr = expr.Trim();
                if (expr.IndexOf(">=") >= 0)
                {
                    string[] sss = expr.Split(">=".ToCharArray());
                    fieldname = sss[0];
                    operation = ">=";
                    value = Convert.ToSingle(sss[sss.Length - 1]);

                    //Vu~ fix mem
                    for (int i = 0; i < sss.Length; i++)
                        sss[i] = null;
                    sss = null;
                }
                else if (expr.IndexOf("<=") >= 0)
                {
                    string[] sss = expr.Split("<=".ToCharArray());
                    fieldname = sss[0];
                    operation = "<=";
                    value = Convert.ToSingle(sss[sss.Length - 1]);

                    //Vu~ fix mem
                    for (int i = 0; i < sss.Length; i++)
                        sss[i] = null;
                    sss = null;
                }
                else if (expr.IndexOf("!=") >= 0)
                {
                    string[] sss = expr.Split("!=".ToCharArray());
                    fieldname = sss[0];
                    operation = "!=";
                    value = Convert.ToSingle(sss[sss.Length - 1]);

                    //Vu~ fix mem
                    for (int i = 0; i < sss.Length; i++)
                        sss[i] = null;
                    sss = null;
                }
                else if (expr.IndexOf("=") >= 0)
                {
                    string[] sss = expr.Split("=".ToCharArray());
                    fieldname = sss[0];
                    operation = "=";
                    if (sss[sss.Length - 1].GetType() == typeof(System.String))
                        value = Convert.ToString(sss[sss.Length - 1]);
                    else
                        value = Convert.ToSingle(sss[sss.Length - 1]);

                    //Vu~ fix mem
                    for (int i = 0; i < sss.Length; i++)
                        sss[i] = null;
                    sss = null;
                }
                else if (expr.IndexOf(">") >= 0)
                {
                    string[] sss = expr.Split(">".ToCharArray());
                    fieldname = sss[0];
                    operation = ">";
                    value = Convert.ToSingle(sss[sss.Length - 1]);

                    //Vu~ fix mem
                    for (int i = 0; i < sss.Length; i++)
                        sss[i] = null;
                    sss = null;
                }
                else if (expr.IndexOf("<") >= 0)
                {
                    string[] sss = expr.Split("<".ToCharArray());
                    fieldname = sss[0];
                    operation = "<";
                    value = Convert.ToSingle(sss[sss.Length - 1]);

                    //Vu~ fix mem
                    for (int i = 0; i < sss.Length; i++)
                        sss[i] = null;
                    sss = null;
                }
                else
                {
                    throw new Exception("Invalid Simple Expression at " + expr + Environment.NewLine + "Formular : [varible] + [operator] + [constant].");
                }
                fieldname = fieldname.Trim();
                operation = operation.Trim();
            }
            catch
            {
                throw new Exception("Invalid Simple Expression at " + expr + Environment.NewLine + "Formular : [varible] + [operator] + [constant].");
            }
        }
        public static string ConditionString(ConditionItem _item)
        {
            return _item.fieldname + _item.criteria + _item.value.ToString();
        }


//        #region Object Routines
//        public static bool MatchAndCondition(ArrayList _alAnd, object Operand)
//        {
//            if (_alAnd == null || _alAnd.Count == 0)
//            {
//#if (TRACE)
//                Console.WriteLine("You're trying to match the empty condition");
//#endif
//                return true;
//            }
//            if (_alAnd[0].GetType() != typeof(StringConditionParse.ConditionItem))
//                throw new System.ArgumentException("Invalid condition format");
//            foreach (StringConditionParse.ConditionItem _item in _alAnd)
//            {
//                if (!StringConditionParse.MatchQuery(_item, Operand))
//                    return false;
//            }
//            return true;
//        }
//        public static bool MatchOrCondition(ArrayList _alOr, object Operand)
//        {
//            if (_alOr == null || _alOr.Count == 0)
//            {
//#if (TRACE)
//                Console.WriteLine("You're trying to match the empty condition");
//#endif
//                return true;
//            }
//            if (_alOr[0].GetType() != typeof(StringConditionParse.ConditionItem))
//                throw new System.ArgumentException("Invalid condition format");
//            foreach (StringConditionParse.ConditionItem _item in _alOr)
//            {
//                if (StringConditionParse.MatchQuery(_item, Operand))
//                    return true;
//            }
//            return false;
//        }
//        public static bool MatchOrAndCondition(ArrayList _alOrAnd, object Operand)
//        {
//            if (_alOrAnd == null || _alOrAnd.Count == 0)
//            {
//#if (TRACE)
//                Console.WriteLine("You're trying to match the empty condition");
//#endif
//                return true;
//            }
//            if (_alOrAnd[0].GetType() != typeof(ArrayList))
//                throw new System.ArgumentException("Invalid condition format");
//            foreach (ArrayList _item in _alOrAnd)
//            {
//                if (StringConditionParse.MatchAndCondition(_item, Operand))
//                    return true;
//            }
//            return false;
//        }
//        public static bool MatchAndOrCondition(ArrayList _alAndOr, object Operand)
//        {
//            if (_alAndOr == null || _alAndOr.Count == 0)
//            {
//#if (TRACE)
//                Console.WriteLine("You're trying to match the empty condition");
//#endif
//                return true;
//            }
//            if (_alAndOr[0].GetType() != typeof(ArrayList))
//                throw new System.ArgumentException("Invalid condition format");
//            foreach (ArrayList _item in _alAndOr)
//            {
//                if (!StringConditionParse.MatchOrCondition(_item, Operand))
//                    return false;
//            }
//            return true;
//        }
//        public static bool MatchQuery(ConditionItem query, object Operand)
//        {
//            if (Operand.GetType() != typeof(ClusterInfor))
//                throw new System.ArgumentException(string.Format("Sorry. We can't compare an instance of {0} type.", Operand.GetType().ToString()));
//            object _valueOp = (Operand as FPPCommon.ClusterInfor)[query.fieldname];
//            if (_valueOp == null)
//                throw new System.ArgumentException(string.Format("The field name {0} is not found.", query.fieldname));
//            if (query.criteria == ">")
//            {
//                return (Convert.ToSingle(_valueOp) > Convert.ToSingle(query.value));
//            }
//            else if (query.criteria == "<")
//            {
//                return (Convert.ToSingle(_valueOp) < Convert.ToSingle(query.value));
//            }
//            else if (query.criteria == "<=")
//            {
//                return (Convert.ToSingle(_valueOp) <= Convert.ToSingle(query.value));
//            }
//            else if (query.criteria == ">=")
//            {
//                return (Convert.ToSingle(_valueOp) >= Convert.ToSingle(query.value));
//            }
//            else if (query.criteria == "=")
//            {
//                return (Convert.ToSingle(_valueOp) == Convert.ToSingle(query.value));
//            }
//            else
//                throw new System.ArgumentException(string.Format("Sorry. We can't execute comparing criteria {0}", query.criteria));
//        }
//        #endregion

//        #region Featureable routines
//        public static bool MatchAndCondition(ArrayList _alAnd, Featureable Operand)
//        {
//            if (_alAnd == null || _alAnd.Count == 0)
//            {
//#if (TRACE)
//                Console.WriteLine("You're trying to match the empty condition");
//#endif
//                return true;
//            }
//            if (_alAnd[0].GetType() != typeof(StringConditionParse.ConditionItem))
//                throw new System.ArgumentException("Invalid condition format");
//            foreach (StringConditionParse.ConditionItem _item in _alAnd)
//            {
//                if (!StringConditionParse.MatchQuery(_item, Operand))
//                    return false;
//            }
//            return true;
//        }
//        public static bool MatchOrCondition(ArrayList _alOr, Featureable Operand)
//        {
//            if (_alOr == null || _alOr.Count == 0)
//            {
//#if (TRACE)
//                Console.WriteLine("You're trying to match the empty condition");
//#endif
//                return true;
//            }
//            if (_alOr[0].GetType() != typeof(StringConditionParse.ConditionItem))
//                throw new System.ArgumentException("Invalid condition format");
//            foreach (StringConditionParse.ConditionItem _item in _alOr)
//            {
//                if (StringConditionParse.MatchQuery(_item, Operand))
//                    return true;
//            }
//            return false;
//        }
//        public static bool MatchOrAndCondition(ArrayList _alOrAnd, Featureable Operand)
//        {
//            if (_alOrAnd == null || _alOrAnd.Count == 0)
//            {
//#if (TRACE)
//                Console.WriteLine("You're trying to match the empty condition");
//#endif
//                return true;
//            }
//            if (_alOrAnd[0].GetType() != typeof(ArrayList))
//                throw new System.ArgumentException("Invalid condition format");
//            foreach (ArrayList _item in _alOrAnd)
//            {
//                if (StringConditionParse.MatchAndCondition(_item, Operand))
//                    return true;
//            }
//            return false;
//        }
//        public static bool MatchAndOrCondition(ArrayList _alAndOr, Featureable Operand)
//        {
//            if (_alAndOr == null || _alAndOr.Count == 0)
//            {
//#if (TRACE)
//                Console.WriteLine("You're trying to match the empty condition");
//#endif
//                return true;
//            }
//            if (_alAndOr[0].GetType() != typeof(ArrayList))
//                throw new System.ArgumentException("Invalid condition format");
//            foreach (ArrayList _item in _alAndOr)
//            {
//                if (!StringConditionParse.MatchOrCondition(_item, Operand))
//                    return false;
//            }
//            return true;
//        }
//        public static bool MatchQuery(ConditionItem query, Featureable Operand)
//        {
//            object _valueOp = Operand[query.fieldname];
//            if (_valueOp == null)
//                throw new System.ArgumentException(string.Format("The field name {0} is not found.", query.fieldname));
//            if (query.criteria == ">")
//            {
//                return (Convert.ToSingle(_valueOp) > Convert.ToSingle(query.value));
//            }
//            else if (query.criteria == "<")
//            {
//                return (Convert.ToSingle(_valueOp) < Convert.ToSingle(query.value));
//            }
//            else if (query.criteria == "<=")
//            {
//                return (Convert.ToSingle(_valueOp) <= Convert.ToSingle(query.value));
//            }
//            else if (query.criteria == ">=")
//            {
//                return (Convert.ToSingle(_valueOp) >= Convert.ToSingle(query.value));
//            }
//            else if (query.criteria == "=")
//            {
//                return (Convert.ToSingle(_valueOp) == Convert.ToSingle(query.value));
//            }
//            else
//                throw new System.ArgumentException(string.Format("Sorry. We can't execute comparing criteria {0}", query.criteria));
//        }
//        #endregion
    }
}
