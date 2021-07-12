using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace SiGlaz.ObjectAnalysis.Common
{
    public class FilterCondition
    {
        public bool IsOrAnd = true;

        private string _stringConditionOperation = "";
        public string StringConditionOperation
        {
            get { return _stringConditionOperation; }
            set { _stringConditionOperation = value; }
        }

        private ArrayList _arrayDataCondition = new ArrayList();
        public ArrayList ArrayDataCondition
        {
            get 
            { 
                return _arrayDataCondition; 
            }
            set
            {
                if (_arrayDataCondition != value)
                {
                    _arrayDataCondition.Clear();
                    if (value != null)
                        _arrayDataCondition.AddRange(value);
                }
            }
        }

        
    }
}
