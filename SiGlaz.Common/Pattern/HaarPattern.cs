using System;
using System.Collections.Generic;
using System.Text;

namespace SiGlaz.Common.ImageAlignment
{
    /// <summary>
    /// This class presents Haar pattern structure
    /// Usage:
    /// - Specify [width, height] of pattern
    /// - Define PositiveRectPoints
    /// - For each image, the BuildEmbededParameters function always should be called first.
    /// - For each scanning position, call CalcEigenValue function (assumed that Integral Image has been calculated).
    /// </summary>
    public class HaarPattern : ParameterBase
    {
        public new int CurrentVersion
        {
            get
            {
                return 1;
            }
        }

        protected int _width = 0;
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        protected int _height = 0;
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        protected int _shiftToCenterX = 0;
        public int ShiftToCenterX
        {
            get { return _shiftToCenterX; }
            set { _shiftToCenterX = value; }
        }

        protected int _shiftToCenterY = 0;
        public int ShiftToCenterY
        {
            get { return _shiftToCenterY; }
            set { _shiftToCenterY = value; }
        }


        protected double _eigenVal = 0;
        public double EigenVal
        {
            get { return _eigenVal; }
            set { _eigenVal = value; }
        }

        /// <summary>
        /// each rect included 4 value:
        /// 0. t - 1, l - 1
        /// 1. t - 1, r
        /// 2. b, l - 1
        /// 3. b, r
        /// </summary>
        protected int[] _positiveRectPoints = null;
        public int[] PositiveRectPoints
        {
            get { return _positiveRectPoints; }
            set { _positiveRectPoints = value; }
        }
        public int PositiveRectCount
        {
            get 
            {
                if (_positiveRectPoints == null)
                    return 0;

                return _positiveRectPoints.Length / 4; 
            }
        }

        protected ushort[] _data = null;
        public ushort[] Data
        {
            get { return _data; }
            set { _data = value; }
        }

        // Embbed Parameters
        protected int _positiveRectPointsLength = 0;
        protected double _positiveFactor = 0;
        protected double _negativeFactor = 0;

        /// <summary>
        /// Each offset is calucated based-on left-top of scanning position
        /// </summary>
        protected int[] _positiveRectOffsets = null;
        protected int _offset_T_1_L_1 = 0;
        protected int _offset_T_1_R = 0;
        protected int _offset_B_L_1 = 0;
        protected int _offset_B_R = 0;
        
        /// <summary>
        /// Calcualte all parameters to computing eigen value in corresponding the current processing image.
        /// If processing image is changed, it should called to re-calculate.
        /// </summary>
        /// <param name="imageStride"></param>
        public virtual void BuildEmbededParameters(int imageStride)
        {
            // calucate offsets
            _offset_T_1_L_1 = -imageStride - 1;
            _offset_T_1_R = -imageStride + _width;
            _offset_B_L_1 = imageStride * _height - 1;
            _offset_B_R = imageStride * _height + _width;
            _positiveRectPointsLength = _positiveRectPoints.Length;
            _positiveRectOffsets = new int[_positiveRectPointsLength];

            // calculate positive factor
            double positiveSum = 0;
            for (int i = 0; i < _positiveRectPointsLength; i += 4)
            {
                int r = _positiveRectPoints[i + 3];
                int b = _positiveRectPoints[i + 2];

                int t = _positiveRectPoints[i];
                int l = _positiveRectPoints[i + 1];

                int w = r - l + 1;
                int h = b - t + 1;

                // accumulate positive item
                positiveSum += w * h;

                // calculate offset for each positive rect
                _positiveRectOffsets[i] = (t - 1) * imageStride + (l - 1);
                _positiveRectOffsets[i + 1] = (t - 1) * imageStride + r;
                _positiveRectOffsets[i + 2] = b * imageStride + (l - 1);
                _positiveRectOffsets[i + 3] = b * imageStride + r;
            }
            // update positive factor
            _positiveFactor = 1.0 - (positiveSum / (_width * _height));
            //_positiveFactor = 1.0;

            // calculate negative factor
            _negativeFactor = 1.0 - _positiveFactor;
            //_negativeFactor = 1.0;
        }

        /// <summary>
        /// Calculate eigen value when known values of positive and negative rects.
        /// </summary>
        /// <param name="positiveVal"> Value of Positive Rects. </param>
        /// <param name="negativeVal"> Value of Negative Rects. </param>
        /// <returns> A double value presents the Eigen value of pattern at scanning position. </returns>
        public virtual double CalcEigenValue(double positiveVal, double negativeVal)
        {
            return (positiveVal * _positiveFactor - negativeVal * _negativeFactor);
        }

        /// <summary>
        /// Calculate eigen value of pattern at scanning position
        /// </summary>
        /// <param name="integralImage"> Integral image of processing image. </param>
        /// <param name="imageStride"> Image stride. </param>
        /// <param name="scanningIndex"> Index of scanning position. </param>
        /// <returns> A double value presents the Eigen value of pattern at scanning position.  </returns>
        public virtual double CalcEigenValue(
            double[] integralImage, int imageStride, int scanningIndex)
        {
            double positiveIntensities = 0;
            for (int i = 0; i < _positiveRectPointsLength; i += 4)
            {
                positiveIntensities +=
                    (
                    integralImage[scanningIndex + _positiveRectOffsets[i + 3]] -
                    integralImage[scanningIndex + _positiveRectOffsets[i + 1]] -
                    integralImage[scanningIndex + _positiveRectOffsets[i + 2]] +
                    integralImage[scanningIndex + _positiveRectOffsets[i]]
                    );
            }
            double totalIntensities =
                (
                integralImage[scanningIndex + _offset_B_R] -
                integralImage[scanningIndex + _offset_T_1_R] -
                integralImage[scanningIndex + _offset_B_L_1] +
                integralImage[scanningIndex + _offset_T_1_L_1]
                );

            return 
                (positiveIntensities * _positiveFactor - 
                (totalIntensities - positiveIntensities) * _negativeFactor);
        }

        /// <summary>
        /// Calculate eigen value of self pattern.
        /// </summary>
        public virtual double CalcEigenValue()
        {
            if (Data != null)
                return CalcEigenValue(Data);
            return 0.0;
        }

        /// <summary>
        /// Calculate eigen value of pattern.
        /// </summary>
        /// <param name="image"> Pattern data. </param>
        /// <returns> A double value presents the Eigen value of pattern. </returns>
        public virtual double CalcEigenValue(double[] image)
        {
            if (image == null || image.Length != _width * _height)
                throw new System.Exception("Input is invalid!");

            double positiveIntensities = 0;
            for (int i = 0; i < _positiveRectPointsLength; i += 4)
            {
                int r = _positiveRectPoints[i + 3];
                int b = _positiveRectPoints[i + 2];

                int t = _positiveRectPoints[i];
                int l = _positiveRectPoints[i + 1];

                for (int y = t; y <= b; y++)
                {
                    int index = y * _width + l;
                    for (int x = l; x <= r; x++, index++)
                    {
                        positiveIntensities += image[index];
                    }
                }
            }
            double totalIntensities = 0;
            int imageLength = _width * _height;
            for (int i = 0; i < imageLength; i++)
            {
                totalIntensities += image[i];
            }

            return
                (positiveIntensities * _positiveFactor -
                (totalIntensities - positiveIntensities) * _negativeFactor);
        }

        /// <summary>
        /// Calculate eigen value of pattern.
        /// </summary>
        /// <param name="image"> Pattern data. </param>
        /// <returns> A double value presents the Eigen value of pattern. </returns>
        public virtual double CalcEigenValue(ushort[] image)
        {
            if (image == null || image.Length != _width * _height)
                throw new System.Exception("Input is invalid!");

            double positiveIntensities = 0;
            for (int i = 0; i < _positiveRectPointsLength; i += 4)
            {
                int r = _positiveRectPoints[i + 3];
                int b = _positiveRectPoints[i + 2];

                int t = _positiveRectPoints[i];
                int l = _positiveRectPoints[i + 1];

                for (int y = t; y <= b; y++)
                {
                    int index = y * _width + l;
                    for (int x = l; x <= r; x++, index++)
                    {
                        positiveIntensities += image[index];
                    }
                }
            }
            double totalIntensities = 0;
            int imageLength = _width * _height;
            for (int i = 0; i < imageLength; i++)
            {
                totalIntensities += image[i];
            }

            return
                (positiveIntensities * _positiveFactor -
                (totalIntensities - positiveIntensities) * _negativeFactor) / 255.0;
        }

        protected override void BaseSerialize(System.IO.BinaryWriter bin)
        {
            base.BaseSerialize(bin); bin.Write(CurrentVersion);

            bin.Write(Width);
            bin.Write(Height);
            bin.Write(ShiftToCenterX);
            bin.Write(ShiftToCenterY);
            bin.Write(EigenVal);
            BinarySerializationCommon.Write(bin, PositiveRectPoints);
            BinarySerializationCommon.Write(bin, Data);
        }

        protected override void BinDeserialize(System.IO.BinaryReader bin)
        {
            base.BinDeserialize(bin); int version = bin.ReadInt32();

            Width = bin.ReadInt32();
            Height = bin.ReadInt32();
            ShiftToCenterX = bin.ReadInt32();
            ShiftToCenterY = bin.ReadInt32();
            EigenVal = bin.ReadDouble();
            PositiveRectPoints = BinarySerializationCommon.ReadIntArray(bin);
            Data = BinarySerializationCommon.ReadUshortArray(bin);
        }

        public new static HaarPattern Deserialize(System.IO.BinaryReader bin)
        {
            return ParameterBase.BaseDeserialize(bin) as HaarPattern;
        }

    }
}

