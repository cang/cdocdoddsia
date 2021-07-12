#define DEBUG_METETIME_

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Diagnostics;

namespace SIA.Algorithms.Preprocessing.Matching
{
    public class GradientMapMatcher : SimpleMatcher
    {
        protected int[] _bestIndices = null;
        protected int[] _interestedXList = null;
        public int[] InterestedXList
        {
            get { return _interestedXList; }
        }
        protected int[] _interestedYList = null;
        public int[] InterestedYList
        {
            get { return _interestedYList; }
        }
        protected double[] _weightSet = null;
        public double[] WeightSet
        {
            get { return _weightSet; }
        }

#if DEBUG
        protected int[] _pivotIndices = null;
        public int[] PivotIndices
        {
            get { return _pivotIndices; }
        }
#endif

        protected int _featureCount = 29;
        protected int _pivotCount = 31;
        public GradientMapMatcher(
            int featureCount, int pivotCount)
        {
            _featureCount = featureCount;
            _pivotCount = pivotCount;
        }

        unsafe protected override void Match(
            ref double confidence, ref int offsetX, ref int offsetY)
        {
            double[][] patternGradientMap = new double[3][];
            double[][] sampleGradientMap = new double[3][];

#if DEBUG_METETIME
            StringBuilder sbTrace = new StringBuilder();
            Stopwatch sw = new Stopwatch(); sw.Start();
#endif

            CalcGradientMap(
                _sampleData, _sampleWidth, _sampleHeight, ref sampleGradientMap);

#if DEBUG_METETIME            
            sw.Stop(); sbTrace.AppendLine(" CalcGradientMap :" + sw.ElapsedTicks); sw.Reset(); sw.Start();
#endif

            int[] pivotIndices = null;
            CalcPatternGradientMap(
                _patternData, _patternWidth, _patternHeight,
                ref patternGradientMap, out pivotIndices);
#if DEBUG
            _pivotIndices = pivotIndices;
#endif

#if DEBUG_METETIME
            sw.Stop(); sbTrace.AppendLine(" CalcPatternGradientMap :" + sw.ElapsedTicks); sw.Reset(); sw.Start();
#endif

            int nPivots = pivotIndices.Length;
            int[] samplePivotOffsets = CalcSamplePivotOffsets(pivotIndices);

            // calc weight set
            _weightSet = CalcWeightMap(pivotIndices);

#if DEBUG_METETIME
            sw.Stop(); sbTrace.AppendLine(" CalcSamplePivotOffsetsCalcWeightMap :" + sw.ElapsedTicks); sw.Reset(); sw.Start();
#endif

            double v, gx0, gy0, gx1, gy1, gx, gy, m, e, sqrIntensity, diffIntensity;
            int sampleIndex, patternIndex, index;
            int r = _sampleWidth - _patternWidth - 2;
            int b = _sampleHeight - _patternHeight - 2;

            BestList gradientBestList = new BestList(_pivotCount);
            BestList intensityBestList = new BestList(_pivotCount);
            BestList complexBestList = new BestList(_pivotCount);

            double[] patternGradientMap0 = patternGradientMap[0];
            double[] patternGradientMap1 = patternGradientMap[1];
            double[] patternGradientMap2 = patternGradientMap[2];
            double[] sampleGradientMap0 = sampleGradientMap[0];
            double[] sampleGradientMap1 = sampleGradientMap[1];
            double[] sampleGradientMap2 = sampleGradientMap[2];

            for (int y = 1; y <= b; y++)
            {
                index = y * _sampleWidth + 1;
                for (int x = 1; x <= r; x++, index++)
                {
                    v = 0;
                    sqrIntensity = 0;
                    for (int i = 0; i < nPivots; i++)
                    {
                        sampleIndex = index + samplePivotOffsets[i];
                        patternIndex = pivotIndices[i];

                        //gy0 = patternGradientMap[0][patternIndex];
                        //gx0 = patternGradientMap[1][patternIndex];
                        //gy1 = sampleGradientMap[0][sampleIndex];
                        //gx1 = sampleGradientMap[1][sampleIndex];
                        gy0 = patternGradientMap0[patternIndex];
                        gx0 = patternGradientMap1[patternIndex];
                        gy1 = sampleGradientMap0[sampleIndex];
                        gx1 = sampleGradientMap1[sampleIndex];

                        gx = gx0 - gx1;
                        gy = gy0 - gy1;

                        m =
                            patternGradientMap2[patternIndex] +
                            sampleGradientMap2[sampleIndex];
                        //patternGradientMap[2][patternIndex] + 
                        //sampleGradientMap[2][sampleIndex];

                        e = m * 0.5 - Math.Sqrt(gx * gx + gy * gy);

                        v += e;

                        diffIntensity = 255 -
                            Math.Abs(_sampleData[sampleIndex] - _patternData[patternIndex]);

                        sqrIntensity += diffIntensity * diffIntensity;
                    }

                    gradientBestList.Add(v, index);
                    intensityBestList.Add(sqrIntensity, index);
                    complexBestList.Add(v * Math.Sqrt(sqrIntensity), index);
                }
            }

#if DEBUG_METETIME
            sw.Stop(); sbTrace.AppendLine(" 4For :" + sw.ElapsedTicks);
            Debug.WriteLine(sbTrace.ToString());
#endif

            //_bestIndices = gradientBestList.Indices;
            List<int> tmp = new List<int>();
            Hashtable lookupTable = new Hashtable(_pivotCount * 3);

            foreach (int bestIndice in gradientBestList.Indices)
            {
                if (lookupTable.ContainsKey(bestIndice))
                    continue;

                lookupTable.Add(bestIndice, bestIndice);

                tmp.Add(bestIndice);
            }
            foreach (int bestIndice in intensityBestList.Indices)
            {
                if (lookupTable.ContainsKey(bestIndice))
                    continue;

                lookupTable.Add(bestIndice, bestIndice);

                tmp.Add(bestIndice);
            }
            foreach (int bestIndice in complexBestList.Indices)
            {
                if (lookupTable.ContainsKey(bestIndice))
                    continue;

                lookupTable.Add(bestIndice, bestIndice);

                tmp.Add(bestIndice);
            }
            lookupTable.Clear(); lookupTable = null;
            _bestIndices = tmp.ToArray();

            int nInterestedPoints = _bestIndices.Length;
            _interestedXList = new int[nInterestedPoints];
            _interestedYList = new int[nInterestedPoints];

            for (int i = 0; i < nInterestedPoints; i++)
            {
                _interestedXList[i] = _bestIndices[i] % _sampleWidth;
                _interestedYList[i] = _bestIndices[i] / _sampleWidth;
            }

            confidence = 1.0;
            int bestIndx = 0;

            offsetX = _interestedXList[bestIndx];
            offsetY = _interestedYList[bestIndx];
            offsetX = (int)(offsetX + _patternWidth * 0.5 - _sampleWidth * 0.5);
            offsetY = (int)(offsetY + _patternHeight * 0.5 - _sampleHeight * 0.5);
        }


        protected int[] CalcSamplePivotOffsets(int[] pivotIndices)
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

        protected double[] CalcWeightMap(int[] pivotIndices)
        {
            int n = _patternWidth * _patternHeight;
            double[] weightSet = new double[n];


            double pivot_score = 9; // kernel 3 x 3

            int nPivots = pivotIndices.Length;
            double totalPivotWeight = n * 0.9; //0.65;
            totalPivotWeight = pivot_score * nPivots;


            double w0 = (n - totalPivotWeight) / (n - nPivots);
            double w1 = totalPivotWeight / nPivots;

            for (int i = 0; i < n; i++)
                weightSet[i] = w0;
            for (int i = 0; i < nPivots; i++)
                weightSet[pivotIndices[i]] = w1;

            return weightSet;
        }

        unsafe protected void CalcGradientMap(
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

        unsafe protected void CalcPatternGradientMap(
            ushort* data, int width, int height,
            ref double[][] gradientMap, out int[] pivotIndices)
        {
            gradientMap[0] = new double[width * height];
            gradientMap[1] = new double[width * height];
            gradientMap[2] = new double[width * height];

            BestList bestList = new BestList(_featureCount);

            //List<int> bestList2 = new List<int>(2000);

            /*
             * fixed method: Sobel operator
             */
            int b = height - 2;
            int r = width - 2;
            double gx, gy, sqrMagnitude, vc;
            double w1 = 0.6;
            double w2 = 1.0 - w1;

            //double t0 = FindThreshold(_patternData, _patternWidth * _patternHeight, 0.005);
            //double t = FindThreshold(_patternData, _patternWidth * _patternHeight, 0.035);

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

                        vc = sqrMagnitude;
                        bestList.Add(vc, index);

                        index++;
                    }
                }
                pG0 = null; pG1 = null;
                tmp1 = null; tmp2 = null; tmp3 = null;
            }

            pivotIndices = bestList.Indices;

            //pivotIndices = bestList2.ToArray();
        }

        unsafe protected double FindThreshold(
            ushort* data, int length, double amout)
        {
            int[] hist = new int[255];
            ushort* tmp = data;
            for (int i = 0; i < length; i++, tmp++)
            {
                hist[*tmp]++;
            }

            double v = amout * length;
            double s = 0;
            for (int i = 0; i <= 255; i++)
            {
                if (s + hist[i] < v)
                {
                    s += hist[i];
                    continue;
                }

                return (i - 0.5);
            }

            return 35;
        }
    }
}
