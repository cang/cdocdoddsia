using System;
using System.Collections.Generic;
using System.Text;

namespace SIA.Algorithms.FeatureProcessing.Textural
{
    public class GrayCoMatrixOperator
    {
        #region Member Fields
        private TextureExtractorBase _extractor = null;

        private byte[] _grayData;
        private int _dataStride;
        private int _dataHeight;

        private int _normalizedBitsShift = 0;
        private int _normalizedBitsGrayLevel = 0;
        private int _grayLevel;
        private int[] _grayCoMatrix = null;
        private int _grayCoMatrixLength = 0;
        private int _offsetCoGrayIndex = 0;
        private double[] _coGrayVotes = null;
        private int[] _firstGrays = null;
        private int[] _secondGrays = null;
        private int _coGrayItemCount = 0;

        private int _kernelWidth = 7;
        private int _kernelHeight = 7;
        private int _halfKernelWidth = 3;
        private int _halfKernelHeight = 3;
        private double _pixelCount = 0;
        private double _invPixelCount = 0.020408163265306122448979591836735;
        #endregion Member fields

        #region Constructors and Destructors
        public GrayCoMatrixOperator(
            byte[] grayData, int stride, int kerneWidth, int kernelHeight)
            : this(grayData, stride, kerneWidth, kernelHeight, 128)
        {
        }

        public GrayCoMatrixOperator(
            byte[] grayData, int stride,
            int kerneWidth, int kernelHeight, int grayLevel)
        {
            _halfKernelWidth = kerneWidth >> 1;
            _halfKernelHeight = kernelHeight >> 1;

            _kernelWidth = (_halfKernelWidth << 1) + 1;
            _kernelHeight = (_halfKernelHeight << 1) + 1;

            Initialize(grayData, stride, grayLevel);
        }

        private void Initialize(byte[] grayData, int stride, int grayLevel)
        {
            if (grayData == null)
                throw new ArgumentNullException("Data is null");

            _grayData = grayData;
            _dataStride = stride;

            if (_grayData.Length % _dataStride != 0)
                throw new System.Exception(
                    string.Format(
                    "Data Length {0} and Stride {1} are incompatible",
                    _grayData.Length, _dataStride));

            _dataHeight = _grayData.Length / _dataStride;

            _grayLevel = grayLevel;
            _normalizedBitsGrayLevel = (int)Math.Log(grayLevel, 2);
            _normalizedBitsShift = 8 - _normalizedBitsGrayLevel;
            _grayCoMatrixLength = _grayLevel * _grayLevel;


            _pixelCount = _kernelWidth * _kernelHeight;
            _invPixelCount = 1.0 / _pixelCount;

            _extractor =
                new HaralickTextureExtrator(
                _kernelWidth, _kernelHeight, _grayLevel);

            _grayCoMatrix = _extractor.CoGrayMatrix;
            _coGrayVotes = _extractor.CoGrayVotes;
            _firstGrays = _extractor.FirstGrays;
            _secondGrays = _extractor.SecondGrays;

            _pixelCount = _kernelWidth * _kernelHeight;
            _invPixelCount = 1.0 / _pixelCount;
        }
        #endregion Constructors and Destructors

        #region Calc co-occurence matrix
        private void CreateCoMatrix(int left, int top)
        {
            // reset co-occurence matrix
            for (int iCoGray = 1; iCoGray < _coGrayItemCount; iCoGray++)
            {
                _grayCoMatrix[_firstGrays[iCoGray] * _grayLevel + _secondGrays[iCoGray]] = 0;
            }
            //Array.Clear(_grayCoMatrix, 0, _grayCoMatrixLength);
            _coGrayItemCount = 1;

            int firstKernelRowIndex = top * _dataStride + left;
            int index;
            int i, j, grayCoMatrixRefIndex, coGrayVoteIndex;
            for (j = 0; j < _kernelHeight; j++, firstKernelRowIndex += _dataStride)
            {
                index = firstKernelRowIndex;

                for (i = 0; i < _kernelWidth; i++, index++)
                {
                    grayCoMatrixRefIndex =
                        ((_grayData[index] >> _normalizedBitsShift) << _normalizedBitsGrayLevel) |
                        (_grayData[index + _offsetCoGrayIndex] >> _normalizedBitsShift);

                    if (_grayCoMatrix[grayCoMatrixRefIndex] == 0)
                    {
                        coGrayVoteIndex = _coGrayItemCount++;
                        _grayCoMatrix[grayCoMatrixRefIndex] = coGrayVoteIndex;

                        //_coGrayVotes[coGrayVoteIndex] = 1.0;
                        _coGrayVotes[coGrayVoteIndex] = _invPixelCount;

                        _firstGrays[coGrayVoteIndex] =
                            _grayData[index] >> _normalizedBitsShift;
                        _secondGrays[coGrayVoteIndex] =
                            _grayData[index + _offsetCoGrayIndex] >> _normalizedBitsShift;

                        continue;
                    }

                    //_coGrayVotes[_grayCoMatrix[grayCoMatrixRefIndex]] += 1.0;
                    _coGrayVotes[_grayCoMatrix[grayCoMatrixRefIndex]] += _invPixelCount;
                }
            }

            _extractor.CoGrayItemCount = _coGrayItemCount;
        }
        #endregion Calc co-occurencee matrix

        #region Co-occurence properties
        public int CoProperties(
            int centerWindowX, int centerWindowY,
            int offsetX, int offsetY, double[] properties)
        {
            return CoPropertiesFromLT(
                centerWindowX - _halfKernelWidth,
                centerWindowY - _halfKernelHeight,
                offsetX, offsetY, properties);
        }

        public int CoPropertiesFromLT(int left, int top,
            int offsetX, int offsetY, double[] properties)
        {
            _offsetCoGrayIndex = offsetY * _dataStride + offsetX;
            this.CreateCoMatrix(left, top);

            if (_coGrayItemCount == 2)
                return 1;

            _extractor.Extract(properties);

            return 0;
        }
        #endregion Co-occurence properties
    }
}
