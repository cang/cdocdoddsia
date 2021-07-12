using System;
using System.Collections.Generic;
using System.Text;

namespace SiGlaz.FeatureProcessing.Textural
{
    public class HaralickTextureExtrator : TextureExtractorBase
    {
        #region Constants
        public const int AngularSeconMomentIndx = 0;
        public const int ContrastIdx = 1;
        public const int CorrelationIdx = 2;
        public const int VarianceIdx = 3;
        public const int InverseDifferenceMomentIdx = 4;
        public const int SumAverageIdx = 5;
        public const int SumVarianceIdx = 6;
        public const int SumEntropyIdx = 7;
        public const int EntropyIdx = 8;
        public const int DifferenceVarianceIdx = 9;
        public const int DifferenceEntropyIdx = 10;
        public const int InformationMeasueOfCorrelation1Idx = 11;
        public const int InformationMeasueOfCorrelation2Idx = 12;
        public const int HomogeneityIdx = 13;
        #endregion Constants


        #region Member fields
        private double[] _p_xsuby = null;
        private int[] _xsuby_mask = null;
        private int[] _xsuby = null;
        private int _xsuby_count = 0;

        private double[] _p_xaddy = null;
        private int[] _xaddy_mask = null;
        private int[] _xaddy = null;
        private int _xaddy_count = 0;

        private double[] _p_x = null;
        private int[] _x_mask = null;
        private int[] _x = null;
        private int _x_count = 0;

        private double[] _p_y = null;
        private int[] _y_mask = null;
        private int[] _y = null;
        private int _y_count = 0;


        private double _asm = 0; // angular second moment (energy)
        private double _contrast = 0; // contrast
        private double _correlation = 0; // correlation
        private double _homogeneity = 0; // homogeneity
        private double _entropy = 0; // entropy
        private double _variance = 0; // sum of squares
        private double _idm = 0; // inverse difference moment
        private double _avg = 0; // sum average
        private double _sumVariance = 0; // sum variance
        private double _sumEntropy = 0; // sum entropy
        private double _diffVariance = 0; // difference variance
        private double _diffEntropy = 0; // difference entropy
        private double _imc1 = 0; // information measure of correlation1
        private double _imc2 = 0; // information measure of correlation1

        private double _mu = 0;
        private double _muSrc = 0;
        private double _muDst = 0;
        private double _muXsubY = 0;
        private double _sigmaSrc = 0;
        private double _sigmaDst = 0;
        private double _hx = 0;
        private double _hy = 0;
        private double _hxy1 = 0;
        private double _hxy2 = 0;
        private double _xAddy_Avg = 0;
        #endregion Member fields


        #region Properties
        public override int FeatureCount
        {
            get { return 14; }
        }
        #endregion Properties

        #region Contructors and destructors
        protected HaralickTextureExtrator()
        {
        }

        public HaralickTextureExtrator(
            int kernelWidth, int kernelHeight, int grayLevel)
            : base(kernelWidth, kernelHeight, grayLevel)
        {
            _invSqrGrayLevel = grayLevel - 1;
            _invSqrGrayLevel = 1.0 / (_invSqrGrayLevel * _invSqrGrayLevel);

            // allocate memory for temporate varialbes
            _p_xaddy = new double[2 * grayLevel + 2];
            _xaddy_mask = new int[2 * grayLevel + 2];
            _xaddy = new int[2 * grayLevel + 2];
            _xaddy_count = 1;

            _p_xsuby = new double[2 * grayLevel + 2];
            _xsuby_mask = new int[grayLevel + 2];
            _xsuby = new int[grayLevel + 2];
            _xsuby_count = 1;

            _p_x = new double[2 * grayLevel + 2];
            _x_mask = new int[2 * grayLevel + 2];
            _x = new int[2 * grayLevel + 2];
            _x_count = 1;

            _p_y = new double[2 * grayLevel + 2];
            _y_mask = new int[2 * grayLevel + 2];
            _y = new int[2 * grayLevel + 2];
            _y_count = 1;
        }
        #endregion Contructors and destructors


        #region Public methods
        public override void Extract(double[] textures)
        {
            if (CoGrayItemCount == 2)
            {
                ExtractForTrivialCase(textures);
                return;
            }

            // reset working space
            this.ResetWorkingSpace();

            //double pij, abs_i_j, v1, v2, v, p;
            double pij, v1, v2, v, p, pxi, pyj, pxpy, logpxpy;
            int cgIndx = 0, srcGrayVal, dstGrayVal;
            int abs_i_j, iaddj;

            for (cgIndx = 1; cgIndx < CoGrayItemCount; cgIndx++)
            {
                pij = CoGrayVotes[cgIndx];
                srcGrayVal = FirstGrays[cgIndx] + 1;
                dstGrayVal = SecondGrays[cgIndx] + 1;

                abs_i_j = (srcGrayVal >= dstGrayVal ? (srcGrayVal - dstGrayVal) : (dstGrayVal - srcGrayVal));

                // 0. Angular Secon Moment / Energy
                _asm += pij * pij;

                // 1. Contrast
                _contrast += abs_i_j * abs_i_j * pij;

                // 2. Correlation
                _muSrc += srcGrayVal * pij;
                _muDst += dstGrayVal * pij;

                // 13. Homogeneity
                _homogeneity += pij / (1 + abs_i_j);

                // 8. Entropy
                _entropy -= pij * Math.Log(pij);

                // 5. sum average
                _avg += srcGrayVal * pij;

                // calc mu
                _mu += pij * srcGrayVal;

                // calc XsubY
                if (_xsuby_mask[abs_i_j] == 0)
                {
                    _xsuby_mask[abs_i_j] = _xsuby_count;
                    _p_xsuby[_xsuby_count] = pij;
                    _xsuby[_xsuby_count] = abs_i_j;
                    _xsuby_count++;
                }
                else
                {
                    _p_xsuby[_xsuby_mask[abs_i_j]] += pij;
                }

                // calc XaddY
                iaddj = srcGrayVal + dstGrayVal;
                if (_xaddy_mask[iaddj] == 0)
                {
                    _xaddy_mask[iaddj] = _xaddy_count;
                    _p_xaddy[_xaddy_count] = pij;
                    _xaddy[_xaddy_count] = iaddj;
                    _xaddy_count++;
                }
                else
                {
                    _p_xaddy[_xaddy_mask[iaddj]] += pij;
                }

                // calc hx, hy, hxy1, hxy2
                // calc px
                if (_x_mask[srcGrayVal] == 0)
                {
                    _x_mask[srcGrayVal] = _x_count;
                    _p_x[_x_count] = pij;
                    _x[_x_count] = srcGrayVal;
                    _x_count++;
                }
                else _p_x[_x_mask[srcGrayVal]] += pij;
                // calc py
                if (_y_mask[dstGrayVal] == 0)
                {
                    _y_mask[dstGrayVal] = _y_count;
                    _p_y[_y_count] = pij;
                    _y[_y_count] = dstGrayVal;
                    _y_count++;
                }
                else _p_y[_y_mask[dstGrayVal]] += pij;

            }

            // calc hx, hy
            for (cgIndx = 1; cgIndx < _x_count; cgIndx++)
            {
                _hx -= _p_x[cgIndx] * Math.Log(_p_x[cgIndx]);
            }
            for (cgIndx = 1; cgIndx < _y_count; cgIndx++)
            {
                _hy -= _p_y[cgIndx] * Math.Log(_p_y[cgIndx]);
            }


            for (cgIndx = 1; cgIndx < CoGrayItemCount; cgIndx++)
            {
                pij = CoGrayVotes[cgIndx];

                srcGrayVal = FirstGrays[cgIndx] + 1;
                dstGrayVal = SecondGrays[cgIndx] + 1;

                v1 = srcGrayVal - _muSrc;
                v2 = dstGrayVal - _muDst;

                // calc sigma source (sigma_x)
                _sigmaSrc += v1 * v1 * pij;
                // calc sigma destination (sigma_y)
                _sigmaDst += v2 * v2 * pij;

                // 2. calc correlation
                _correlation += v1 * v2 * pij;

                // 3. calc variance
                _variance += (srcGrayVal - _mu) * (srcGrayVal - _mu) * pij;

                // calc hxy1, hxy2
                pxi = _p_x[_x_mask[srcGrayVal]];
                pyj = _p_y[_y_mask[dstGrayVal]];
                pxpy = pxi * pyj;
                logpxpy = Math.Log(pxpy);
                _hxy1 -= pij * logpxpy;
                _hxy2 -= pxpy * logpxpy;

            }
            // calc sigma
            _sigmaSrc = Math.Sqrt(_sigmaSrc);
            _sigmaDst = Math.Sqrt(_sigmaDst);
            //calc correlation
            _correlation /= _sigmaSrc * _sigmaDst;

            // calc mu_XsubY
            for (cgIndx = 1; cgIndx < _xsuby_count; cgIndx++)
            {
                _muXsubY += _xsuby[cgIndx] * _p_xsuby[cgIndx];
            }

            // x sub y
            for (cgIndx = 1; cgIndx < _xsuby_count; cgIndx++)
            {
                p = _p_xsuby[cgIndx];
                v = _xsuby[cgIndx];

                // 4. inverse difference moment
                _idm += _p_xsuby[cgIndx] / (1 + v * v);

                // 9. difference variance
                _diffVariance += (v - _muXsubY) * (v - _muXsubY) * p;

                // 10. difference entropy
                _diffEntropy -= p * Math.Log(p);
            }

            // x add y
            for (cgIndx = 1; cgIndx < _xaddy_count; cgIndx++)
            {
                p = _p_xaddy[cgIndx];
                v = _xaddy[cgIndx];

                //// 5. sum average
                //_sumAvg += v * p;
                _xAddy_Avg += v * p;

                // 7. sum entropy
                _sumEntropy -= p * Math.Log(p);
            }

            // calc sum variance
            for (cgIndx = 1; cgIndx < _xaddy_count; cgIndx++)
            {
                p = _p_xaddy[cgIndx];
                v = _xaddy[cgIndx];

                // 6. sum variance
                //_sumVariance += (v - _sumAvg) * (v - _sumAvg) * p;
                _sumVariance += (v - _xAddy_Avg) * (v - _xAddy_Avg) * p;
                //_sumVariance += (v - _sumEntropy) * (v - _sumEntropy) * p;
            }


            // calc information measure correlation1
            _imc1 = (_entropy - _hxy1) / Math.Max(_hx, _hy);

            // calc information measure correlation2
            _imc2 = Math.Sqrt(Math.Abs(1 - Math.Exp(-2.0 * (_hxy2 - _entropy))));
            //_imc2 = Math.Sqrt(Math.Abs(1 - Math.Exp(2.0 * (_hxy2 - _entropy))));

            // normalize to [0, 1] and storage texture features
            this.NormalizeAndUpateFeatures(textures);
        }

        protected override void ExtractForTrivialCase(double[] textures)
        {

        }

        protected override void NormalizeAndUpateFeatures(double[] textures)
        {
            //public const int AngularSeconMomentIndx = 0;
            //public const int ContrastIdx = 1;
            //public const int CorrelationIdx = 2;
            //public const int VarianceIdx = 3;
            //public const int InverseDifferenceMomentIdx = 4;
            //public const int SumAverageIdx = 5;
            //public const int SumVarianceIdx = 6;
            //public const int SumEntropyIdx = 7;
            //public const int EntropyIdx = 8;
            //public const int DifferenceVarianceIdx = 9;
            //public const int DifferenceEntropyIdx = 10;
            //public const int InformationMeasueOfCorrelation1Idx = 11;
            //public const int InformationMeasueOfCorrelation2Idx = 12;
            //public const int HomogeneityIdx = 13;

            // normalize to [0, 1] and storage texture features 
            textures[AngularSeconMomentIndx] = _asm;
            textures[ContrastIdx] = _contrast * _invSqrGrayLevel * 5.0;
            textures[CorrelationIdx] = (_correlation + 1.0) * 0.5;
            textures[VarianceIdx] = _variance * _invGrayLevel;
            textures[InverseDifferenceMomentIdx] = _idm;
            //textures[SumAverageIdx] = _sumAvg * _invGrayLevel;
            textures[SumAverageIdx] = _avg * _invGrayLevel;
            textures[SumVarianceIdx] = _sumVariance * _invGrayLevel * 0.5;
            textures[SumEntropyIdx] = _sumEntropy / 5.0;
            textures[EntropyIdx] = _entropy / 5.0;
            textures[DifferenceVarianceIdx] = _diffVariance * _invGrayLevel;
            textures[DifferenceEntropyIdx] = _diffEntropy / 5.0;
            textures[InformationMeasueOfCorrelation1Idx] = (_imc1 + 1.0) * 0.5;
            //textures[InformationMeasueOfCorrelation2Idx] = _imc2;
            textures[InformationMeasueOfCorrelation2Idx] = _imc2 * 0.05;
            textures[HomogeneityIdx] = _homogeneity;
        }

        protected override void ResetWorkingSpace()
        {
            // reset mask
            for (int i = 1; i < _xaddy_count; i++)
            {
                _xaddy_mask[_xaddy[i]] = 0;
            }
            _xaddy_count = 1;
            for (int i = 1; i < _xsuby_count; i++)
            {
                _xsuby_mask[_xsuby[i]] = 0;
            }
            _xsuby_count = 1;
            for (int i = 1; i < _x_count; i++)
            {
                _x_mask[_x[i]] = 0;
            }
            _x_count = 1;
            for (int i = 1; i < _y_count; i++)
            {
                _y_mask[_y[i]] = 0;
            }
            _y_count = 1;


            // reset
            _asm = 0; // angular second moment (energy)
            _contrast = 0; // contrast
            _correlation = 0; // correlation
            _homogeneity = 0; // homogeneity
            _entropy = 0; // entropy
            _variance = 0; // sum of squares
            _idm = 0; // inverse difference moment
            //_sumAvg = 0; // sum average
            _avg = 0;
            _sumVariance = 0; // sum variance
            _sumEntropy = 0; // sum entropy
            _diffVariance = 0; // difference variance
            _diffEntropy = 0; // difference entropy
            _imc1 = 0; // information measure of correlation1
            _imc2 = 0; // information measure of correlation1

            // temp
            _mu = 0;
            _muSrc = 0;
            _muDst = 0;
            _muXsubY = 0;
            _sigmaSrc = 0;
            _sigmaDst = 0;
            _hx = 0;
            _hy = 0;
            _hxy1 = 0;
            _hxy2 = 0;
            _xAddy_Avg = 0;
        }
        #endregion Public methods












        public void ExtractEx(double[] textures)
        {
            if (CoGrayItemCount == 2)
            {
                ExtractForTrivialCase(textures);
                return;
            }

            // reset working space
            this.ResetWorkingSpace();

            //double pij, abs_i_j, v1, v2, v, p;
            double pij, v1, v2, v, p, pxi, pyj, pxpy, logpxpy;
            int cgIndx = 0, srcGrayVal, dstGrayVal;
            int abs_i_j, iaddj;

            for (cgIndx = 1; cgIndx < CoGrayItemCount; cgIndx++)
            {
                pij = CoGrayVotes[cgIndx];
                srcGrayVal = FirstGrays[cgIndx] + 1;
                dstGrayVal = SecondGrays[cgIndx] + 1;

                abs_i_j = (srcGrayVal >= dstGrayVal ? (srcGrayVal - dstGrayVal) : (dstGrayVal - srcGrayVal));

                // 0. Angular Secon Moment / Energy
                _asm += pij * pij;

                // 1. Contrast
                _contrast += abs_i_j * abs_i_j * pij;

                // 2. Correlation
                _muSrc += srcGrayVal * pij;
                _muDst += dstGrayVal * pij;

                // 13. Homogeneity
                _homogeneity += pij / (1 + abs_i_j);

                // 8. Entropy
                _entropy -= pij * Math.Log(pij);

                // 5. sum average
                _avg += srcGrayVal * pij;

                // calc mu
                _mu += pij * srcGrayVal;

                // calc XsubY
                if (_xsuby_mask[abs_i_j] == 0)
                {
                    _xsuby_mask[abs_i_j] = _xsuby_count;
                    _p_xsuby[_xsuby_count] = pij;
                    _xsuby[_xsuby_count] = abs_i_j;
                    _xsuby_count++;
                }
                else
                {
                    _p_xsuby[_xsuby_mask[abs_i_j]] += pij;
                }

                // calc XaddY
                iaddj = srcGrayVal + dstGrayVal;
                if (_xaddy_mask[iaddj] == 0)
                {
                    _xaddy_mask[iaddj] = _xaddy_count;
                    _p_xaddy[_xaddy_count] = pij;
                    _xaddy[_xaddy_count] = iaddj;
                    _xaddy_count++;
                }
                else
                {
                    _p_xaddy[_xaddy_mask[iaddj]] += pij;
                }

                // calc hx, hy, hxy1, hxy2
                // calc px
                if (_x_mask[srcGrayVal] == 0)
                {
                    _x_mask[srcGrayVal] = _x_count;
                    _p_x[_x_count] = pij;
                    _x[_x_count] = srcGrayVal;
                    _x_count++;
                }
                else _p_x[_x_mask[srcGrayVal]] += pij;
                // calc py
                if (_y_mask[dstGrayVal] == 0)
                {
                    _y_mask[dstGrayVal] = _y_count;
                    _p_y[_y_count] = pij;
                    _y[_y_count] = dstGrayVal;
                    _y_count++;
                }
                else _p_y[_y_mask[dstGrayVal]] += pij;

            }

            // calc hx, hy
            for (cgIndx = 1; cgIndx < _x_count; cgIndx++)
            {
                _hx -= _p_x[cgIndx] * Math.Log(_p_x[cgIndx]);
            }
            for (cgIndx = 1; cgIndx < _y_count; cgIndx++)
            {
                _hy -= _p_y[cgIndx] * Math.Log(_p_y[cgIndx]);
            }


            for (cgIndx = 1; cgIndx < CoGrayItemCount; cgIndx++)
            {
                pij = CoGrayVotes[cgIndx];

                srcGrayVal = FirstGrays[cgIndx] + 1;
                dstGrayVal = SecondGrays[cgIndx] + 1;

                v1 = srcGrayVal - _muSrc;
                v2 = dstGrayVal - _muDst;

                // calc sigma source (sigma_x)
                _sigmaSrc += v1 * v1 * pij;
                // calc sigma destination (sigma_y)
                _sigmaDst += v2 * v2 * pij;

                // 2. calc correlation
                _correlation += v1 * v2 * pij;

                // 3. calc variance
                _variance += (srcGrayVal - _mu) * (srcGrayVal - _mu) * pij;

                // calc hxy1, hxy2
                pxi = _p_x[_x_mask[srcGrayVal]];
                pyj = _p_y[_y_mask[dstGrayVal]];
                pxpy = pxi * pyj;
                logpxpy = Math.Log(pxpy);
                _hxy1 -= pij * logpxpy;
                _hxy2 -= pxi * pyj * logpxpy;

            }
            // calc sigma
            _sigmaSrc = Math.Sqrt(_sigmaSrc);
            _sigmaDst = Math.Sqrt(_sigmaDst);
            //calc correlation
            _correlation /= _sigmaSrc * _sigmaDst;

            // calc mu_XsubY
            for (cgIndx = 1; cgIndx < _xsuby_count; cgIndx++)
            {
                _muXsubY += _xsuby[cgIndx] * _p_xsuby[cgIndx];
            }

            // x sub y
            for (cgIndx = 1; cgIndx < _xsuby_count; cgIndx++)
            {
                p = _p_xsuby[cgIndx];
                v = _xsuby[cgIndx];

                // 4. inverse difference moment
                _idm += _p_xsuby[cgIndx] / (1 + v * v);

                // 9. difference variance
                _diffVariance += (v - _muXsubY) * (v - _muXsubY) * p;

                // 10. difference entropy
                _diffEntropy -= p * Math.Log(p);
            }

            // x add y
            for (cgIndx = 1; cgIndx < _xaddy_count; cgIndx++)
            {
                p = _p_xaddy[cgIndx];
                v = _xaddy[cgIndx];

                //// 5. sum average
                //_sumAvg += v * p;
                _xAddy_Avg += v * p;

                // 7. sum entropy
                _sumEntropy -= p * Math.Log(p);
            }

            // calc sum variance
            for (cgIndx = 1; cgIndx < _xaddy_count; cgIndx++)
            {
                p = _p_xaddy[cgIndx];
                v = _xaddy[cgIndx];

                // 6. sum variance
                //_sumVariance += (v - _sumAvg) * (v - _sumAvg) * p;
                _sumVariance += (v - _xAddy_Avg) * (v - _xAddy_Avg) * p;
                //_sumVariance += (v - _sumEntropy) * (v - _sumEntropy) * p;
            }


            // calc information measure correlation1
            _imc1 = (_entropy - _hxy1) / Math.Max(_hx, _hy);

            // calc information measure correlation2
            _imc2 = Math.Sqrt(Math.Abs(1 - Math.Exp(-2.0 * (_hxy2 - _entropy))));

            // normalize to [0, 1] and storage texture features
            this.NormalizeAndUpateFeatures(textures);
        }
    }
}
