using System;
using System.Collections.Generic;
using System.Text;

namespace SiGlaz.Preprocessing.Matching
{
    public class GradientMapMatcher : SimpleMatcher
    {
        private int[] _bestIndices = null;
        private int[] _interestedXList = null;
        public int[] InterestedXList
        {
            get { return _interestedXList; }
        }
        private int[] _interestedYList = null;
        public int[] InterestedYList
        {
            get { return _interestedYList; }
        }

        unsafe protected override void Match(
            ref double confidence, ref int offsetX, ref int offsetY)
        {
            double[][] patternGradientMap = new double[3][];
            double[][] sampleGradientMap = new double[3][];

            CalcGradientMap(
                _sampleData, _sampleWidth, _sampleHeight, ref sampleGradientMap);

            int[] pivotIndices = null;
            CalcPatternGradientMap(
                _patternData, _patternWidth, _patternHeight, 
                ref patternGradientMap, out pivotIndices);

            int nPivots = pivotIndices.Length;
            int[] samplePivotOffsets = CalcSamplePivotOffsets(pivotIndices);

            double v, gx0, gy0, gx1, gy1, gx, gy, m, e, sqrIntensity, diffIntensity;
            int sampleIndex, patternIndex, index;
            int r = _sampleWidth - _patternWidth - 2;
            int b = _sampleHeight - _patternHeight - 2;

            SimpleMatcher.BestList gradientBestList = new BestList(31);
            SimpleMatcher.BestList intensityBestList = new BestList(31);
            SimpleMatcher.BestList complexBestList = new BestList(31);

            for (int y = 1; y <= b; y++)
            {
                index = y * _sampleWidth + 1;
                for (int x = 1; x <= r ; x++, index++)
                {
                    v = 0;
                    sqrIntensity = 0;
                    for (int i = 0; i < nPivots; i++)
                    {
                        sampleIndex = index + samplePivotOffsets[i];
                        patternIndex = pivotIndices[i];

                        gy0 = patternGradientMap[0][patternIndex];
                        gx0 = patternGradientMap[1][patternIndex];
                        gy1 = sampleGradientMap[0][sampleIndex];
                        gx1 = sampleGradientMap[1][sampleIndex];

                        gx = gx0 - gx1;
                        gy = gy0 - gy1;

                        m = 
                            patternGradientMap[2][patternIndex] + 
                            sampleGradientMap[2][sampleIndex];

                        e = m * 0.5 - Math.Sqrt(gx * gx + gy * gy);

                        v += e;

                        diffIntensity = 
                            255 - 
                            Math.Abs(_sampleData[sampleIndex] - _patternData[patternIndex]);

                        sqrIntensity += diffIntensity * diffIntensity;
                    }

                    gradientBestList.Add(v, index);
                    intensityBestList.Add(sqrIntensity, index);
                    complexBestList.Add(v * sqrIntensity, index);
                }
            }

            //_bestIndices = gradientBestList.Indices;
            
            List<int> tmp = new List<int>();
            tmp.AddRange(gradientBestList.Indices);
            tmp.AddRange(intensityBestList.Indices);
            tmp.AddRange(complexBestList.Indices);
            _bestIndices = tmp.ToArray();

            int nInterestedPoints = _bestIndices.Length;
            _interestedXList = new int[nInterestedPoints];
            _interestedYList = new int[nInterestedPoints];

            for (int i = 0; i < nInterestedPoints; i++)
            {
                _interestedXList[i] = _bestIndices[i] % _sampleWidth;
                _interestedYList[i] = _bestIndices[i] / _sampleWidth;
            }
        }

        private int[] CalcSamplePivotOffsets(int[] pivotIndices)
        {
            int nPivots = pivotIndices.Length;
            int[] samplePivotOffsets = new int[nPivots];
            for (int i = 0; i < nPivots; i++)
            {
                int x = pivotIndices[i] % _patternWidth;
                int y = pivotIndices[i] / _patternHeight;

                samplePivotOffsets[i] = y * _sampleWidth + x;
            }

            return samplePivotOffsets;
        }

        unsafe private void CalcGradientMap(
            ushort* data, int width, int height, ref double[][] gradientMap)
        {
            gradientMap[0] = new double[width * height];
            gradientMap[1] = new double[width * height];
            gradientMap[2] = new double[width * height];

            /*
             * fixed method: Sobel operator
             */
            int b = height - 2;
            int r = width - 2;
            double gx, gy;

            fixed (double* p0 = gradientMap[0], p1 = gradientMap[1], p2 = gradientMap[2])
            {
                ushort* tmp1 = null, tmp2 = null, tmp3 = null;
                double* pG0 = null, pG1 = null, pG2 = null;
                for (int y = 1; y <= b; y++)
                {
                    tmp1 = data + (y - 1) * width;
                    tmp2 = tmp1 + width;
                    tmp3 = tmp2 + width;
                    pG0 = p0 + y * width + 1;
                    pG1 = p1 + y * width + 1;
                    pG2 = p2 + y * width + 1;

                    for (int x = 1; x <= r; x++, tmp1++, tmp2++, tmp3++, pG0++, pG1++, pG2++)
                    {
                        gy =
                            (*tmp2 + *(tmp2 + 1) + *(tmp2 + 2)) -
                            (*tmp1 + *(tmp1 + 1) + *(tmp1 + 2));
                        gx =
                            (*(tmp1 + 2) + *(tmp2 + 2) + *(tmp3 + 2)) -
                            (*tmp1 + *tmp2 + *tmp3);

                        *pG0 = gy;
                        *pG1 = gx;
                        *pG2 = Math.Sqrt(gx * gx + gy * gy);
                    }
                }
                pG0 = null; pG1 = null;
                tmp1 = null; tmp2 = null; tmp3 = null;
            }
        }

        unsafe private void CalcPatternGradientMap(
            ushort* data, int width, int height, 
            ref double[][] gradientMap, out int[] pivotIndices)
        {
            gradientMap[0] = new double[width * height];
            gradientMap[1] = new double[width * height];
            gradientMap[2] = new double[width * height];

            SimpleMatcher.BestList bestList = new BestList(29);

            /*
             * fixed method: Sobel operator
             */
            int b = height - 2;
            int r = width - 2;
            double gx, gy, sqrMagnitude;
            
            fixed (double* p0 = gradientMap[0], p1 = gradientMap[1], p2 = gradientMap[2])
            {
                ushort* tmp1 = null, tmp2 = null, tmp3 = null;
                double* pG0 = null, pG1 = null, pG2 = null;
                for (int y = 1; y <= b; y++)
                {
                    tmp1 = data + (y - 1) * width;
                    tmp2 = tmp1 + width;
                    tmp3 = tmp2 + width;
                    pG0 = p0 + y * width + 1;
                    pG1 = p1 + y * width + 1;
                    pG2 = p2 + y * width + 1;

                    int index = y * width + 1;

                    for (int x = 1; x <= r; x++, tmp1++, tmp2++, tmp3++, pG0++, pG1++, pG2++)
                    {
                        gy =
                            (*tmp2 + *(tmp2 + 1) + *(tmp2 + 2)) -
                            (*tmp1 + *(tmp1 + 1) + *(tmp1 + 2));
                        gx =
                            (*(tmp1 + 2) + *(tmp2 + 2) + *(tmp3 + 2)) -
                            (*tmp1 + *tmp2 + *tmp3);

                        
                        sqrMagnitude = gx * gx + gy * gy;
                        
                        *pG0 = gy;
                        *pG1 = gx;
                        *pG2 = Math.Sqrt(sqrMagnitude);
                        
                        // best list
                        bestList.Add(sqrMagnitude, index);
                        index++;
                    }
                }
                pG0 = null; pG1 = null;
                tmp1 = null; tmp2 = null; tmp3 = null;
            }

            pivotIndices = bestList.Indices;
        }
    }
}
