using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace SiGlaz.ObjectAnalysis.Common
{
    public class MDCCParamLibrary : ICloneable
    {
        public string LibraryName = "";
        private List<MDCCParamItem> _items = new List<MDCCParamItem>();
        public List<MDCCParamItem> Items
        {
            get
            {
                if (_items == null)
                    _items = new List<MDCCParamItem>();
                return _items;
            }
            set
            {
                if (_items != value)
                {
                    _items = value;
                    if (_items == null)
                        _items = new List<MDCCParamItem>();
                }
            }
        }

        [XmlIgnore]
        public bool[] States
        {
            get
            {
                if (this.Items.Count == 0)
                    return null;

                bool[] states = new bool[this.Items.Count];
                for (int i = _items.Count - 1; i >= 0; i--)
                {
                    states[i] = _items[i].ApplyCombination;
                }
                return states;
            }

            set
            {
                if (value == null)
                    return;
                int n = (int)Math.Min(value.Length, this.Items.Count);
                for (int i = 0; i < n; i++)
                {
                    _items[i].ApplyCombination = value[i];
                }
            }
        }

        public MDCCParamLibrary()
        {
        }

        public MDCCParamLibrary(string libName)
        {
            LibraryName = libName;
        }

        public MDCCParamLibrary(
            string libName, List<MDCCParamItem> items)
            : this(libName)
        {
            this.Items = items;
        }

        public bool Serialize(string fn)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(fn, FileMode.Create);
                XmlSerializer s = new XmlSerializer(typeof(MDCCParamLibrary));
                s.Serialize(fs, this);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        public static MDCCParamLibrary Deserialize(string fn)
        {
            FileStream fs = null;
            XmlSerializer s = null;
            try
            {
                fs = new FileStream(fn, FileMode.Open);
                s = new XmlSerializer(typeof(MDCCParamLibrary));
                MDCCParamLibrary Sctrl = (MDCCParamLibrary)s.Deserialize(fs);
                return Sctrl;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (s != null)
                {
                    s = null;
                }
                if (fs != null)
                    fs.Close();
            }
        }

        public MDCCParamItem IsExisted(MDCCParamItem item)
        {
            if (item == null)
                return null;

            if (this.Items.Count == 0)
                return null;

            string ruleName = item.RuleName.Trim().ToLower();
            foreach (MDCCParamItem i in _items)
            {
                if (i.RuleName.Trim().ToLower() == ruleName)
                    return i;
            }

            return null;
        }

        public MDCCParamItem IsExisted(MDCCParamItem item, int exceptIndex)
        {
            if (item == null)
                return null;

            if (this.Items.Count == 0)
                return null;

            string ruleName = item.RuleName.Trim().ToLower();
            for (int i = _items.Count - 1; i >= 0; i--)
            {
                if (i == exceptIndex)
                    continue;
                if (_items[i].RuleName.Trim().ToLower() == ruleName)
                    return _items[i];
            }            

            return null;
        }

        #region ICloneable Members

        public object Clone()
        {
            MDCCParamLibrary clonedResult = (MDCCParamLibrary)MemberwiseClone();
            clonedResult.Items = new List<MDCCParamItem>(_items.Count);
            foreach (MDCCParamItem item in _items)
            {
                clonedResult.Items.Add(item.Clone() as MDCCParamItem);
            }

            return clonedResult;
        }

        #endregion
    }

    public class MDCCParamItem : ICloneable
    {
        public string RuleName = "";
        public string SignatureName = "";
        private MDCCParam _condition = null;
        public MDCCParam Condition
        {
            get
            {
                if (_condition == null)
                {
                    _condition = new MDCCParam();
                }
                return _condition;
            }
            set
            {
                if (_condition != value)
                {
                    _condition = value;
                    if (_condition == null)
                        _condition = new MDCCParam();
                }
            }
        }
        public bool ApplyCombination = false;
        
        public MDCCParamItem()
        {
        }

        public MDCCParamItem(
            string ruleName, string signatureName, MDCCParam condition)
        {
            RuleName = ruleName;
            SignatureName = signatureName;
            this.Condition = condition;
        }

        public bool Serialize(string fn)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(fn, FileMode.Create);
                XmlSerializer s = new XmlSerializer(typeof(MDCCParamItem));
                s.Serialize(fs, this);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        public static MDCCParamItem Deserialize(string fn)
        {
            FileStream fs = null;
            XmlSerializer s = null;
            try
            {
                fs = new FileStream(fn, FileMode.Open);
                s = new XmlSerializer(typeof(MDCCParamItem));
                MDCCParamItem Sctrl = (MDCCParamItem)s.Deserialize(fs);
                return Sctrl;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (s != null)
                {
                    s = null;
                }
                if (fs != null)
                    fs.Close();
            }
        }

        public void CopyFrom(MDCCParamItem src)
        {
            RuleName = src.RuleName;
            SignatureName = src.SignatureName;
            this.Condition = src.Condition;
        }

        public void CopyTo(MDCCParamItem dst)
        {
            dst.RuleName = RuleName;
            dst.SignatureName = SignatureName;
            dst.Condition = Condition;
        }

        public bool IsIdentifyWith(MDCCParamItem other)
        {
            if (other == null)
                return false;
            if (RuleName != other.RuleName)
                return false;
            if (SignatureName != other.SignatureName)
                return false;

            return this.Condition.IsIdentifyWidth(
                other.Condition.Conditions, other.Condition.DFSLevel);
        }

        #region ICloneable Members

        public object Clone()
        {
            MDCCParamItem clonedResult = (MDCCParamItem)MemberwiseClone();
            if (_condition == null)
                clonedResult.Condition = null;
            else
                clonedResult.Condition = _condition.Clone() as MDCCParam;

            return clonedResult;
        }

        #endregion
    }
}
