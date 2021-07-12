using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace SiGlaz.ObjectAnalysis.Common
{
    public class MDCCParamFilterLibrary
    {
        public string LibraryName = "";
        MDCCParamFilter _item = new MDCCParamFilter();
        public MDCCParamFilter Item
        {
            get
            {
                if (_item == null)
                    _item = new MDCCParamFilter();
                return _item;
            }
            set
            {
                if (_item != value)
                {
                    _item = value;
                    if (_item == null)
                        _item = new MDCCParamFilter();
                }
            }
        }
       
        public MDCCParamFilterLibrary()
        {
        }

        public MDCCParamFilterLibrary(string libName)
        {
            LibraryName = libName;
        }

        public MDCCParamFilterLibrary(
            string libName, MDCCParamFilter item)
            : this(libName)
        {
            this.Item = item;
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

        public static MDCCParamFilterLibrary Deserialize(string fn)
        {
            FileStream fs = null;
            XmlSerializer s = null;
            try
            {
                fs = new FileStream(fn, FileMode.Open);
                s = new XmlSerializer(typeof(MDCCParamFilterLibrary));
                MDCCParamFilterLibrary Sctrl = (MDCCParamFilterLibrary)s.Deserialize(fs);
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
    }

    public class MDCCParamFilter
    {
        public string FilterName = "";
        public QUERY_TYPE TypeQuery;
        public string StrQuery;

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
        
        public MDCCParamFilter()
        {
        }

        public MDCCParamFilter(
            string filterName, QUERY_TYPE typeQuery, String strQuery, MDCCParam condition)
        {
            FilterName = filterName;
            TypeQuery = typeQuery;
            StrQuery = strQuery;
            _condition = condition;
        }

        public bool Serialize(string fn)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(fn, FileMode.Create);
                XmlSerializer s = new XmlSerializer(typeof(MDCCParamFilter));
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

        public static MDCCParamFilter Deserialize(string fn)
        {
            FileStream fs = null;
            XmlSerializer s = null;
            try
            {
                fs = new FileStream(fn, FileMode.Open);
                s = new XmlSerializer(typeof(MDCCParamFilter));
                MDCCParamFilter Sctrl = (MDCCParamFilter)s.Deserialize(fs);
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

        public void CopyFrom(MDCCParamFilter src)
        {
            this.FilterName = src.FilterName;
            this.TypeQuery = src.TypeQuery;
            this.StrQuery = src.StrQuery;
            this._condition = src._condition;
        }

        public void CopyTo(MDCCParamFilter dst)
        {
            dst.FilterName = this.FilterName;
            dst.TypeQuery = this.TypeQuery;
            dst.StrQuery = this.StrQuery;
            dst._condition = this._condition;
        }

        public bool IsIdentifyWith(MDCCParamFilter other)
        {
            if (other == null)
                return false;
            if (FilterName != other.FilterName)
                return false;
            if (TypeQuery != other.TypeQuery)
                return false;
            if (StrQuery != other.StrQuery)
                return false;

            return this.Condition.IsIdentifyWidth(
                other.Condition.Conditions, other.Condition.DFSLevel);
        }
    }
}
