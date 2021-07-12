using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using SIA.Common.Analysis;

namespace SiGlaz.Common.ImageAlignment
{
    public enum eSupportedUnit
    {
        Pixel = 0,
        Micron = 1,
        Nanometer = 2,
        Millimeter = 3,
        Centimeter = 4,
        Meter = 5
    }

    //public class MetrologyUnit
    //{
    //    protected static MetrologyUnitBase _current = new MicronUnit(1);
    //    public static MetrologyUnitBase Current
    //    {
    //        get
    //        {
    //            if (_current == null)
    //                _current = new PixelUnit();
    //            return _current;
    //        }
    //    }
    //}

    public interface IMetrologyUnit
    {
        string FullName { get; }
        string ShortName { get; }

        float FromPixel(float v);
        float ToPixel(float v);
    }

    public class MetrologyUnitBase : ParameterBase
    {
        public int Version
        {
            get { return 1; }
        }

        #region Member fields
        protected string _fullName = "";
        protected string _shortName = "";
        protected float _pixelVal = 1;
        protected float _unitVal = 1;
        protected float _toPixelFactor = 1;
        protected float _fromPixelFactor = 1;
        #endregion Member fields

        #region Constructors and destructors
        public MetrologyUnitBase()
        {
        }

        public MetrologyUnitBase(
            string fullName, string shortName, float pixelVal, float unitVal)
        {
            _fullName = fullName;
            _shortName = shortName;

            this.UpdateScale(pixelVal, unitVal);
        }
        #endregion Constructors and destructors

        #region Properties/Methods
        public string FullName
        {
            get { return _fullName; }
        }

        public string ShortName
        {
            get { return _shortName; }
        }

        public virtual float FromPixel(float v)
        {
            return _fromPixelFactor * v;
        }

        public virtual float ToPixel(float v)
        {
            return _toPixelFactor * v;
        }

        public virtual float PixelVal
        {
            get { return _pixelVal; }
        }

        public virtual float UnitVal
        {
            get { return _unitVal; }
        }

        public virtual void UpdateScale(float pixelVal, float unitVal)
        {
            _pixelVal = pixelVal;
            _unitVal = unitVal;

            _toPixelFactor = _unitVal / _pixelVal;
            _fromPixelFactor = _pixelVal / _unitVal;
        }

        public virtual void UpdateObjectInfo(ArrayList objList)
        {
            if (objList == null) return;

            foreach (DetectedObject obj in objList)
            {
                double val = obj.Perimeter;
                obj.Perimeter = (val * _unitVal) / _pixelVal;
                obj.Area = (val * _unitVal * _unitVal) / (_pixelVal * _pixelVal);
            }
        }
        #endregion Properties/Methods

        #region Copy data
        public void CopyFrom(MetrologyUnitBase mu)
        {
            _fullName = mu.FullName;
            _shortName = mu.ShortName;

            this.UpdateScale(mu.PixelVal, mu.UnitVal);
        }
        #endregion Copy data

        #region Serialize & deserialize
        protected override void BaseSerialize(System.IO.BinaryWriter bin)
        {
            base.BaseSerialize(bin); bin.Write(CurrentVersion);

            bin.Write(_fullName);
            bin.Write(_shortName);
            bin.Write(_pixelVal);
            bin.Write(_unitVal);
            bin.Write(_toPixelFactor);
            bin.Write(_fromPixelFactor);
        }

        protected override void BinDeserialize(System.IO.BinaryReader bin)
        {
            base.BinDeserialize(bin); int version = bin.ReadInt32();

            _fullName = bin.ReadString();
            _shortName = bin.ReadString();
            _pixelVal = bin.ReadSingle();
            _unitVal = bin.ReadSingle();
            _toPixelFactor = bin.ReadSingle();
            _fromPixelFactor = bin.ReadSingle();
        }

        public static MetrologyUnitBase Deserialize(System.IO.BinaryReader bin)
        {
            return ParameterBase.BaseDeserialize(bin) as MetrologyUnitBase;
        }

        public static MetrologyUnitBase Deserialize(string filename)
        {
            return ParameterBase.BaseDeserialize(filename) as MetrologyUnitBase;
        }
        #endregion Serialize & deserialize
    }

    public class PixelUnit
    {
        public const string FULLNAME = "pixel";
        public const string SHORTNAME = "pixel";

        public static MetrologyUnitBase CreateInstance()
        {
            return new MetrologyUnitBase(FULLNAME, SHORTNAME, 1, 1);
        }
    }

    public class NanometerUnit
    {
        public const string FULLNAME = "nanometer";
        public const string SHORTNAME = "nn";

        public static MetrologyUnitBase CreateInstance(float pixelVal, float unitVal)
        {
            return new MetrologyUnitBase(FULLNAME, SHORTNAME, pixelVal, unitVal);
        }
    }

    public class MicronUnit
    {
        public const string FULLNAME = "micron";
        public const string SHORTNAME = "micron";

        public static MetrologyUnitBase CreateInstance(float pixelVal, float unitVal)
        {
            return new MetrologyUnitBase(FULLNAME, SHORTNAME, pixelVal, unitVal);
        }
    }

    public class MillimeterUnit
    {
        public const string FULLNAME = "millimeter";
        public const string SHORTNAME = "mm";

        public static MetrologyUnitBase CreateInstance(float pixelVal, float unitVal)
        {
            return new MetrologyUnitBase(FULLNAME, SHORTNAME, pixelVal, unitVal);
        }
    }
}
