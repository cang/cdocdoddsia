using System;
using System.Collections.Generic;
using System.Text;

namespace SIA.Algorithms.FeatureProcessing
{
    public interface IFeatureDescriptor
    {
        string Version { get; }
        string FeatureName { get; set; }
        string FeatureDecrtiption { get; set; }
        int FeatureCount { get; set; }
        double MinValue { get; set; }
        double MaxValue { get; set; }
    }

    public class FeatureDescriptor : IFeatureDescriptor
    {
        #region Member fields
        protected string _version = "1.0";
        protected string _featureName = "";
        protected string _featureDescription = "";
        protected int _featureCount = 0;
        protected double _minValue = 0;
        protected double _maxValue = 1;
        #endregion Member fields

        #region IFeatureDescriptor Members
        public string Version
        {
            get { return _version; }
        }

        public string FeatureName
        {
            get
            {
                return _featureName;
            }
            set
            {
                _featureName = value;
            }
        }

        public string FeatureDecrtiption
        {
            get
            {
                return _featureDescription;
            }
            set
            {
                _featureDescription = value;
            }
        }

        public int FeatureCount
        {
            get
            {
                return _featureCount;
            }
            set
            {
                _featureCount = value;
            }
        }

        public double MinValue
        {
            get
            {
                return _minValue;
            }
            set
            {
                _minValue = value;
            }
        }

        public double MaxValue
        {
            get
            {
                return _maxValue;
            }
            set
            {
                _maxValue = value;
            }
        }

        #endregion
    }

    public class FeatureDescriptorCollection : List<IFeatureDescriptor>
    {
        public FeatureDescriptorCollection()
        {
        }

        public FeatureDescriptorCollection(int capacity)
        {
            base.Capacity = capacity;
        }
    }
}
