using System;
using System.Collections.Generic;
using System.Text;

namespace SiGlaz.FeatureProcessing
{
    public class FeatureVector : IFeatureVector
    {
        #region Member fields
        protected int _featureCount = 0;
        protected double[] _features;
        protected int _classId = -1;
        #endregion Member fields

        #region Constructors and destructors
        public FeatureVector()
        {
        }

        public FeatureVector(double[] features, int featureCount)
        {
            _features = features;
            _featureCount = featureCount;
        }

        public FeatureVector(double[] features, int featureCount, int classId)
            : this(features, featureCount)
        {
            _classId = classId;
        }

        public FeatureVector(float[] features, int featureCount, int classId)
        {
            _featureCount = featureCount;
            _classId = classId;
            _features = new double[_featureCount];
            for (int i = 0; i < _featureCount; i++)
            {
                _features[i] = features[i];
            }
        }
        #endregion Constructors and destructors

        #region IFeatureVector Members

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

        public double[] Features
        {
            get
            {
                return _features;
            }
            set
            {
                _features = value;
            }
        }

        public int ClassId
        {
            get
            {
                return _classId;
            }
            set
            {
                _classId = value;
            }
        }

        #endregion

        #region IFeatureVector Members

        public virtual void SaveAsBinary(System.IO.BinaryWriter writer)
        {
            writer.Write(_classId);
            writer.Write(_featureCount);
            for (int i = 0; i < _featureCount; i++)
            {
                writer.Write(_features[i]);
            }
        }

        public virtual void LoadFromBinary(System.IO.BinaryReader reader)
        {
            _classId = reader.ReadInt32();
            _featureCount = reader.ReadInt32();
            _features = new double[_featureCount];
            for (int i = 0; i < _featureCount; i++)
            {
                _features[i] = reader.ReadDouble();
            }
        }

        #endregion
    }
}
