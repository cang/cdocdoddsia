using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SiGlaz.Algorithms.Core;

namespace SiGlaz.Preprocessing.Matching
{
    public class CorrelationMatcher : SimpleMatcher
    {
        protected override void Match(ref double confidence, ref int offsetX, ref int offsetY)
        {

        }

        public void Match(ushort[] sampleData, int widthSample, int heightSample,
            ushort[] patternData, int widthPattern, int heightPattern,
            double threshold,
            out double confidenceMax,
            out int offsetX,
            out int offsetY)
        {
            confidenceMax = 0.0;
            offsetX = 0;
            offsetY = 0;

            double[] weightset = new double[widthPattern * heightPattern];
            for (int i = 0; i < weightset.Length; i++)
                weightset[i] = 1.0;

            //double[,] corrCoeff = MatrixUtils.CorrelationCoefficientWeightSet(
            //    patternData,
            //    sampleData,
            //    widthPattern,
            //    widthSample,
            //    weightset);
            double[,] corrCoeff = CorrelationCoefficientByRingWeightSet(
                patternData,
                sampleData,
                widthPattern,
                widthSample,
                weightset,
                threshold,
                3);

            if (corrCoeff == null)
                throw new System.ExecutionEngineException("Correlation Coefficient Matrix was not defined");
            double maxCorr = MatrixUtils.unsafeFindMaxElement(
                corrCoeff,
                ref offsetY,
                ref offsetX);

            offsetX -= (widthSample - widthPattern) / 2;
            offsetY -= (heightSample - heightPattern) / 2;

            confidenceMax = maxCorr;
        }

        public void Match(ushort[] sampleData, int widthSample, int heightSample,
            ushort[] patternData, int widthPattern, int heightPattern,
            double threshold,
            int[] interestedX, int[] interestedY,
            out double confidenceMax,
            out int offsetX,
            out int offsetY)
        {
            confidenceMax = 0.0;
            offsetX = 0;
            offsetY = 0;

            double[] weightset = new double[widthPattern * heightPattern];
            for (int i = 0; i < weightset.Length; i++)
                weightset[i] = 1.0;

            double[,] corrCoeff = CorrelationCoefficientWeightSet(
                patternData,
                sampleData,
                widthPattern,
                widthSample,
                weightset,
                interestedX,interestedY);

            if (corrCoeff == null)
                throw new System.ExecutionEngineException("Correlation Coefficient Matrix was not defined");
            double maxCorr = MatrixUtils.unsafeFindMaxElement(
                corrCoeff,
                ref offsetY,
                ref offsetX);

            offsetX -= (widthSample - widthPattern) / 2;
            offsetY -= (heightSample - heightPattern) / 2;

            confidenceMax = maxCorr;
        }

        public double[,] CorrelationCoefficientWeightSet(ushort[] op1, ushort[] op2, int stride1, int stride2, double[] weightset, int[] interestedX, int[] interestedY)
        {
            int h1 = op1.Length / stride1;
            int h2 = op2.Length / stride2;
            if (h1 == 0 || h2 < h1 || stride2 < stride1)
                throw new System.ArgumentException("Operand size is invalid");
            double[,] result = new double[h2 - h1, stride2 - stride1];

            int len = interestedX.Length;
            for (int i=0;i<len;i++)
            {
                int left = interestedX[i];
                int top = interestedY[i];
                result[top, left] = MatrixUtils.CorrelationCoefficientWeightSet(op1, op2, stride1, stride2, left, top, weightset);
            }
            return result;
        }

        public double[,] CorrelationCoefficientWeightSet(ushort[] op1, ushort[] op2, int stride1, int stride2, double[] weightset)
        {
            int h1 = op1.Length / stride1;
            int h2 = op2.Length / stride2;
            if (h1 == 0 || h2 < h1 || stride2 < stride1)
                throw new System.ArgumentException("Operand size is invalid");
            double[,] result = new double[h2 - h1, stride2 - stride1];
            for (int top = 0; top < h2 - h1; top++)
                for (int left = 0; left < stride2 - stride1; left++)
                    result[top, left] = MatrixUtils.CorrelationCoefficientWeightSet(op1, op2, stride1, stride2, left, top, weightset);

            return result;
        }

        public double[,] CorrelationCoefficientByRingWeightSet(ushort[] op1, ushort[] op2, int stride1, int stride2, double[] weightset, double threshold, int radius_step)
        {
            int[][] matSpiral = null;
            List<Point> offsetList = null;
            List<Int32> radiusList = null;
            int sizeSpiral = stride2 - stride1;
            CreateSpiralMatrix(sizeSpiral, ref matSpiral, ref offsetList, ref radiusList);
            if (matSpiral == null)
                throw new System.ArgumentException("Operand size is invalid");

            int h1 = op1.Length / stride1;
            int h2 = op2.Length / stride2;
            if (h1 == 0 || h2 < h1 || stride2 < stride1)
                throw new System.ArgumentException("Operand size is invalid");
            double[,] result = new double[h2 - h1, stride2 - stride1];


            int len = radiusList.Count;
            int radiusMax = radiusList[0];
            int radiusMin = radiusList[len - 1];
            double[] maxCorr_perRing = new double[radiusMax / radius_step + 1]; // 1 ring contains "step" radius

            double maxCorrTemp = 0.0;
            int iRing = 0;
            for (int iRadius = len - 1; iRadius >= 0; iRadius--)
            {
                int r = radiusList[iRadius];
                if ( r - radiusMin >= radius_step)
                {
                    maxCorr_perRing[iRing] = maxCorrTemp;

                    radiusMin = r;
                    iRing++;
                    maxCorrTemp = 0.0;
                }

                if (iRing > 0
                    && maxCorr_perRing[iRing] < maxCorr_perRing[iRing - 1]
                    && maxCorr_perRing[iRing - 1] > threshold)
                {
                    break;
                }
                int left = offsetList[iRadius].X;
                int top = offsetList[iRadius].Y;
                result[top, left] = MatrixUtils.CorrelationCoefficientWeightSet(op1, op2, stride1, stride2, left, top, weightset);
                if (result[top, left] > maxCorrTemp)
                {
                    maxCorrTemp = result[top, left];
                }
            }

            return result;
        }

        #region Support Functions
        public bool CreateSpiralMatrix(int n, ref int[][] result, ref List<Point> offsetList, ref List<Int32> radiusList)
        {
            result = new int[n][];
            for (int i = n - 1; i >= 0; i--)
                result[i] = new int[n];
            offsetList = new List<Point>();
            radiusList = new List<Int32>();

            int pos = 0;
            int count = n;
            int value = -n;
            int sum = -1;

            int m = n;
            int radius = n / 2 + 1;
            int radius_ele = 4 * m - 4; // m^2 - (m-2)^2;
            int count_ele = 1;

            do
            {
                value = -1 * value / n;
                for (int i = 0; i < count; i++)
                {
                    sum += value;
                    AddSpiralEle(ref result, ref offsetList, ref radiusList, sum / n, sum % n, pos++, ref count_ele, ref radius_ele, ref radius, ref m);
                }
                value *= n;
                count--;
                for (int i = 0; i < count; i++)
                {
                    sum += value;
                    AddSpiralEle(ref result, ref offsetList, ref radiusList, sum / n, sum % n, pos++, ref count_ele, ref radius_ele, ref radius, ref m);
                }
            } while (count > 0);

            return true;
        }
        public void AddSpiralEle(ref int[][] result, ref List<Point> offsetList, ref List<Int32> radiusList, int i, int j, int value, ref int count_ele, ref int radius_ele, ref int radius, ref int m)
        {
            result[i][j] = value;
            offsetList.Add(new Point(i, j));
            radiusList.Add(radius);
            count_ele++;
            if (count_ele > radius_ele)
            {
                m -= 2;
                radius_ele = 4 * m - 4; // m^2 - (m-2)^2;
                count_ele = 1;
                radius--;
            }
        }
        #endregion
    }
}
