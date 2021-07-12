#define TEST__

#define PARALLEL

using System;
using System.Collections.Generic;
using System.Text;
using SiGlaz.Common.ImageAlignment;
using SIA.IPEngine;
using SiGlaz.Algorithms.Core;
using SIA.Algorithms.Preprocessing.Interpolation;
using SIA.SystemLayer;
using System.Drawing;
using SDD = System.Drawing.Drawing2D;
using SIA.Algorithms.Preprocessing.Matching;
using SIA.SystemFrameworks;

namespace SIA.Algorithms.Preprocessing.Alignment
{
    public abstract class AlignerBase
    {
        protected AlignmentSettings _settings = null;
        protected GreyDataImage _image = null;

        protected double[] _matchedXList = null;
        protected double[] _matchedYList = null;
        protected double[] _matchedConfiences = null;

        #region Initialize

        protected AlignerBase(AlignmentSettings settings)
        {
            Init(settings);
        }

        protected virtual void Init(AlignmentSettings settings)
        {
            _settings = settings;
        }

        public void InitImage(GreyDataImage image)
        {
            _image = image;
        }

        public AlignmentResult Align(GreyDataImage image)
        {
            _image = image;

            return Align();
        }

        #endregion

        protected virtual AlignmentResult Align()
        {
            AlignmentResult result = null;
            SDD.Matrix draftTransform = null;

            try
            {
                PerformDraftAlignment(ref result, ref draftTransform);

#if DEBUG && TEST
                using (GreyDataImage roi = new GreyDataImage(_settings.NewWidth, _settings.NewHeight))
                {
                    ImageInterpolator.AffineTransform(
                                    InterpolationMethod.Bilinear, _image, draftTransform, roi);
                    roi.SaveImage(
                        string.Format("D:\\temp\\test\\sample_{0}.bmp", "test_finetune1"),
                        SIA.Common.eImageFormat.Bmp);
                }
#endif

                PerformCorrectDraftResult(ref result, draftTransform);
            }
            catch
            {

            }

#if DEBUG && TEST
            using (GreyDataImage roi = new GreyDataImage(_settings.NewWidth, _settings.NewHeight))
            {
                ImageInterpolator.AffineTransform(
                                InterpolationMethod.Bilinear, _image, draftTransform, roi);
                roi.SaveImage(
                    string.Format("D:\\temp\\test\\sample_{0}.bmp", "test_finetune2"),
            SIA.Common.eImageFormat.Bmp);
            }
#endif

            return result;
        }

        public abstract void PerformDraftAlignment(
            ref AlignmentResult result, ref SDD.Matrix drafTransform);

        protected virtual void PerformCorrectDraftResult(
            ref AlignmentResult draftResult, SDD.Matrix draftTransform)
        {
            SDD.Matrix transform = null;
            if (_settings.SampleCount < 4) // Single Keypoint method
            {
                transform = draftTransform.Clone();

                //throw new System.NotImplementedException();
            }
            else // Multiple Keypoints
            {
                transform = FineTune(_image, draftTransform);
            }

            PointF[] resPoints = new PointF[] {
                    new PointF(0, 0), 
                    new PointF(_settings.NewWidth, 0), 
                    new PointF(0, _settings.NewHeight)};

            transform.TransformPoints(resPoints);
            draftResult.SourceCoordinates = resPoints;

            draftResult.NewWidth = _settings.NewWidth;
            draftResult.NewHeight = _settings.NewHeight;

            using (MyMatrix myMatrix = new MyMatrix(draftTransform))
            {
                if (_matchedXList != null && _matchedYList != null)
                {
                    myMatrix.TransformPoints(_matchedXList, _matchedYList);
                }
            }
            draftResult.MatchedXList = _matchedXList;
            draftResult.MatchedYList = _matchedYList;
            draftResult.MatchedConfidences = _matchedConfiences;

#if DEBUG && TEST
            draftTransform.Reset();
            draftTransform.Multiply(transform);
#endif
        }

        #region Draft step

        protected virtual unsafe void ScanBoundary_OLD(
            GreyDataImage image, int[] colsIdx, int[] rowsIdx,
            ref double[] rotation, ref double topC, ref double bottomC, ref double leftC, ref double rightC)
        {

            int width = image._width;
            int height = image._height;

            int lengthC = colsIdx.Length;
            List<int> yCoordsTop = new List<int>();
            List<int> yCoordsBottom = new List<int>();
            List<int> xCoordsColIdx = new List<int>(colsIdx);

            int lengthR = rowsIdx.Length;
            List<int> xCoordsLeft = new List<int>();
            List<int> xCoordsRight = new List<int>();
            List<int> yCoordsRowIdx = new List<int>(rowsIdx);

            rotation = new double[4];

            ushort thresGrey = (ushort)_settings.IntensityThreshold;
            ushort* greyBuffer = image._aData;

            int i, j;
            // Scan Top and Bottom
            for (j = 0; j < lengthC; j++)
            {
                greyBuffer = image._aData + (colsIdx[j]);
                for (i = 0; i < height; i++, greyBuffer += width)
                {
                    if (*greyBuffer > thresGrey)
                        break;
                }
                yCoordsTop.Add(i);

                greyBuffer = image._aData + colsIdx[j] + (height - 1) * width;
                for (i = height - 1; i >= 0; i--, greyBuffer -= width)
                {
                    if (*greyBuffer > thresGrey)
                        break;
                }
                yCoordsBottom.Add(i);
            }
            // Remove noise
            for (i = lengthC - 1; i >= 0; i--)
            {
                if (yCoordsTop[i] > _settings.MaxBoundaryTop
                    || yCoordsBottom[i] < _settings.MaxBoundaryBottom)
                {
                    yCoordsTop.RemoveAt(i);
                    yCoordsBottom.RemoveAt(i);
                    xCoordsColIdx.RemoveAt(i);
                }
            }
            EstimateLineByLSQ(xCoordsColIdx.ToArray(), yCoordsTop.ToArray(), ref rotation[0], ref topC);
            EstimateLineByLSQ(xCoordsColIdx.ToArray(), yCoordsBottom.ToArray(), ref rotation[1], ref bottomC);

            // Scan Left and Right
            for (i = 0; i < lengthR; i++)
            {
                greyBuffer = image._aData + (rowsIdx[i] * width);
                for (j = 0; j < width; j++, greyBuffer++)
                {
                    if (*greyBuffer > thresGrey)
                        break;
                }
                xCoordsLeft.Add(j);

                greyBuffer = image._aData + ((rowsIdx[i] + 1) * width);
                for (j = width - 1; j > 0; j--, greyBuffer--)
                {
                    if (*greyBuffer > thresGrey)
                        break;
                }
                xCoordsRight.Add(j);
            }
            // Remove noise
            for (i = lengthR - 1; i >= 0; i--)
            {
                if (xCoordsLeft[i] > _settings.MaxBoundaryLeft
                    || xCoordsRight[i] < _settings.MaxBoundaryRight)
                {
                    xCoordsLeft.RemoveAt(i);
                    xCoordsRight.RemoveAt(i);
                    yCoordsRowIdx.RemoveAt(i);
                }
            }
            EstimateLineByLSQ(xCoordsLeft.ToArray(), yCoordsRowIdx.ToArray(), ref rotation[2], ref leftC);
            EstimateLineByLSQ(xCoordsRight.ToArray(), yCoordsRowIdx.ToArray(), ref rotation[3], ref rightC);
        }

        public virtual unsafe void ScanBoundary(
            GreyDataImage image,
            ref double leftRoi, ref double topRoi,
            ref double widthRoi, ref double heightRoi,
            ref double angleRoi)
        {
            double[] rotation;
            double leftC = 0, rightC = 0, topC = 0, bottomC = 0;
            float xTopLeft = 0, yTopLeft = 0, xTopRight = 0, yTopRight = 0, xBottomLeft = 0, yBottomLeft = 0;

            int i, j;

            int width = image._width;
            int height = image._height;

            int stepCol = width / 100;
            int stepRow = height / 100;

            int width_1_3 = width / 3;
            int height_1_3 = height / 3;
            int width_2_3 = width * 2 / 3;
            int height_2_3 = height * 2 / 3;

            // Prepare Top, Bottom Horizontal Line
            List<int> yCoordsTop = new List<int>();
            List<int> yCoordsBottom = new List<int>();
            List<int> xCoordsTop = new List<int>();
            List<int> xCoordsBottom = new List<int>();

            List<int> colIdx = new List<int>();
            for (j = 0; j < width; j += stepCol)
                colIdx.Add(j);
            int lengthC = colIdx.Count;

            // Prepare Left, Right Vertical Line
            List<int> xCoordsLeft = new List<int>();
            List<int> xCoordsRight = new List<int>();
            List<int> yCoordsLeft = new List<int>();
            List<int> yCoordsRight = new List<int>();

            List<int> rowIdx = new List<int>();
            for (i = 0; i < height; i += stepRow)
                rowIdx.Add(i);
            int lengthR = rowIdx.Count;

            rotation = new double[4];

            ushort thresGrey = (ushort)_settings.IntensityThreshold;
            ushort* greyBuffer = image._aData;

            double LQSDistancePercentThreshold = _settings.LQSDistancePercentThreshold;


            // Scan Top and Bottom
            for (j = 0; j < lengthC; j++)
            {
                greyBuffer = image._aData + (colIdx[j]);
                for (i = 0; i < height; i++, greyBuffer += width)
                {
                    if (*greyBuffer > thresGrey)
                        break;
                }
                if (i < height_1_3)
                {
                    yCoordsTop.Add(i);
                    xCoordsTop.Add(colIdx[j]);
                }

                greyBuffer = image._aData + colIdx[j] + (height - 1) * width;
                for (i = height - 1; i >= 0; i--, greyBuffer -= width)
                {
                    if (*greyBuffer > thresGrey)
                        break;
                }
                if (i > height_2_3)
                {
                    yCoordsBottom.Add(i);
                    xCoordsBottom.Add(colIdx[j]);
                }
            }
            EstimateLineByLSQ(xCoordsTop.ToArray(), yCoordsTop.ToArray(), ref rotation[0], ref topC);
            EstimateLineByLSQ(xCoordsBottom.ToArray(), yCoordsBottom.ToArray(), ref rotation[1], ref bottomC);

            // Remove noise
            for (i = xCoordsTop.Count - 1; i >= 0; i--)
            {
                double distanceTop = Math.Abs(DistancePointToLine(xCoordsTop[i], yCoordsTop[i], rotation[0], topC));
                distanceTop /= height;

                if (distanceTop > LQSDistancePercentThreshold)
                {
                    yCoordsTop.RemoveAt(i);
                    xCoordsTop.RemoveAt(i);
                }
            }
            for (i = xCoordsBottom.Count - 1; i >= 0; i--)
            {
                double distanceBottom = Math.Abs(DistancePointToLine(xCoordsBottom[i], yCoordsBottom[i], rotation[1], bottomC));
                distanceBottom /= height;

                if (distanceBottom > LQSDistancePercentThreshold)
                {
                    yCoordsBottom.RemoveAt(i);
                    xCoordsBottom.RemoveAt(i);
                }
            }
            EstimateLineByLSQ(xCoordsTop.ToArray(), yCoordsTop.ToArray(), ref rotation[0], ref topC);
            EstimateLineByLSQ(xCoordsBottom.ToArray(), yCoordsBottom.ToArray(), ref rotation[1], ref bottomC);


            // Scan Left and Right
            for (i = 0; i < lengthR; i++)
            {
                greyBuffer = image._aData + (rowIdx[i] * width);
                for (j = 0; j < width; j++, greyBuffer++)
                {
                    if (*greyBuffer > thresGrey)
                        break;
                }
                if (j < width_1_3)
                {
                    xCoordsLeft.Add(j);
                    yCoordsLeft.Add(rowIdx[i]);
                }

                greyBuffer = image._aData + ((rowIdx[i] + 1) * width);
                for (j = width - 1; j > 0; j--, greyBuffer--)
                {
                    if (*greyBuffer > thresGrey)
                        break;
                }
                if (j > width_2_3)
                {
                    xCoordsRight.Add(j);
                    yCoordsRight.Add(rowIdx[i]);
                }
            }
            EstimateLineByLSQ(xCoordsLeft.ToArray(), yCoordsLeft.ToArray(), ref rotation[2], ref leftC);
            EstimateLineByLSQ(xCoordsRight.ToArray(), yCoordsRight.ToArray(), ref rotation[3], ref rightC);
            // Remove noise
            for (i = xCoordsLeft.Count - 1; i >= 0; i--)
            {
                double distanceLeft = Math.Abs(DistancePointToLine(xCoordsLeft[i], yCoordsLeft[i], rotation[2], leftC));
                distanceLeft /= width;

                if (distanceLeft > LQSDistancePercentThreshold)
                {
                    xCoordsLeft.RemoveAt(i);
                    yCoordsLeft.RemoveAt(i);
                }
            }
            for (i = xCoordsRight.Count - 1; i >= 0; i--)
            {
                double distanceRight = Math.Abs(DistancePointToLine(xCoordsRight[i], yCoordsRight[i], rotation[3], rightC));
                distanceRight /= width;

                if (distanceRight > LQSDistancePercentThreshold)
                {
                    xCoordsRight.RemoveAt(i);
                    yCoordsRight.RemoveAt(i);
                }
            }
            EstimateLineByLSQ(xCoordsLeft.ToArray(), yCoordsLeft.ToArray(), ref rotation[2], ref leftC);
            EstimateLineByLSQ(xCoordsRight.ToArray(), yCoordsRight.ToArray(), ref rotation[3], ref rightC);

            // Detect Corners
            LineIntersect(rotation[0], topC, rotation[2], leftC, ref xTopLeft, ref yTopLeft);
            LineIntersect(rotation[0], topC, rotation[3], rightC, ref xTopRight, ref yTopRight);
            LineIntersect(rotation[1], bottomC, rotation[2], leftC, ref xBottomLeft, ref yBottomLeft);

            // Return Value
            leftRoi = xTopLeft;
            topRoi = yTopLeft;


            double xCoordsRight_AVG = 0;
            double yCoordsRight_AVG = 0;
            for (i = xCoordsRight.Count - 1; i >= 0; i--)
            {
                xCoordsRight_AVG += xCoordsRight[i];
                yCoordsRight_AVG += yCoordsRight[i];
            }
            xCoordsRight_AVG /= xCoordsRight.Count;
            yCoordsRight_AVG /= yCoordsRight.Count;

            double xCoordsBottom_AVG = 0;
            double yCoordsBottom_AVG = 0;
            for (i = xCoordsBottom.Count - 1; i >= 0; i--)
            {
                xCoordsBottom_AVG += xCoordsBottom[i];
                yCoordsBottom_AVG += yCoordsBottom[i];
            }
            xCoordsBottom_AVG /= xCoordsBottom.Count;
            yCoordsBottom_AVG /= yCoordsBottom.Count;

            widthRoi = Math.Abs(DistancePointToLine((int)xCoordsRight_AVG, (int)yCoordsRight_AVG, rotation[2], leftC));
            heightRoi = Math.Abs(DistancePointToLine((int)xCoordsBottom_AVG, (int)yCoordsBottom_AVG, rotation[0], topC));

            //widthRoi = (int)Math.Sqrt(Math.Pow(xTopLeft - xTopRight, 2.0) + Math.Pow(yTopLeft - yTopRight, 2.0));
            //heightRoi = (int)Math.Sqrt(Math.Pow(xTopLeft - xBottomLeft, 2.0) + Math.Pow(yTopLeft - yBottomLeft, 2.0));
            angleRoi = (rotation[0] + rotation[1]) / 2;
        }


        #endregion

        #region Least-Square method

        protected virtual int EstimateHorizontalLinesByLSQ(
            int[] topXCoords, int[] topYCoords, int[] bottomXCoords, int[] bottomYCoords,
            ref double angle, ref double topC, ref double bottomC)
        {
            try
            {
                int len1 = topXCoords.Length;
                int len2 = bottomXCoords.Length;

                float[,] A = new float[len1 + len2, 3];
                float[,] b = new float[len1 + len2, 1];

                for (int i = 0; i < len1; i++)
                {
                    A[i, 0] = topXCoords[i];
                    A[i, 1] = 1;
                    b[i, 0] = topYCoords[i];
                }
                for (int i = 0; i < len2; i++)
                {
                    A[i + len1, 0] = bottomXCoords[i];
                    A[i + len1, 2] = 1;
                    b[i + len1, 0] = bottomYCoords[i];
                }

                float[,] pInvA = null;
                if (SiGlaz.Algorithms.Core.MatrixUtils.unsafePseudoInverse(A, ref pInvA) != 0)
                    throw new System.ExecutionEngineException("Pseudoinverse of A was not defined");
                float[,] x = null;
                if (SiGlaz.Algorithms.Core.MatrixUtils.unsafeA_TperB(pInvA, b, ref x) != 0)
                    throw new System.ExecutionEngineException("Multiplication failed");

                angle = x[0, 0];
                topC = x[1, 0];
                bottomC = x[2, 0];

                return 0;
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.ToString());
                return -1;
            }
        }

        protected virtual int EstimateLineByLSQ(
            int[] XCoords, int[] YCoords, ref double angle, ref double C)
        {
            try
            {
                int len1 = XCoords.Length;

                float[,] A = new float[len1, 2];
                float[,] b = new float[len1, 1];

                for (int i = 0; i < len1; i++)
                {
                    A[i, 0] = XCoords[i];
                    A[i, 1] = 1;
                    b[i, 0] = YCoords[i];
                }

                float[,] pInvA = null;
                if (SiGlaz.Algorithms.Core.MatrixUtils.unsafePseudoInverse(A, ref pInvA) != 0)
                    throw new System.ExecutionEngineException("Pseudoinverse of A was not defined");
                float[,] x = null;
                if (SiGlaz.Algorithms.Core.MatrixUtils.unsafeA_TperB(pInvA, b, ref x) != 0)
                    throw new System.ExecutionEngineException("Multiplication failed");

                angle = x[0, 0];
                C = x[1, 0];

                return 0;
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.ToString());
                return -1;
            }

        }

        #endregion Least-Square method

        #region Fine tuning step

        protected virtual unsafe void DetectCrossHaarPatternParallely(GreyDataImage image, Rectangle roi, double[] integral, HaarPattern pattern, ref int shiftX, ref int shiftY)
        {
            if (roi.Width < pattern.Width || roi.Height < pattern.Height)
                return;

            ushort* data = image._aData;
            int width = image._width;
            int height = image._height;
            int widthRoi = roi.Width;
            int heightRoi = roi.Height;
            int widthPtn = pattern.Width;
            int heightPtn = pattern.Height;
            int[] posRectPoints = pattern.PositiveRectPoints;
            int len = posRectPoints.Length;

            double[,] scores = new double[heightRoi - heightPtn, widthRoi - widthPtn];

            //for (int i = heightRoi - heightPtn - 1; i >= 0; i--)
            Parallel.For(0, heightRoi - heightPtn, delegate(int i)
            {
                for (int j = widthRoi - widthPtn - 1; j >= 0; j--)
                {
                    int ii = i + roi.Top;
                    int jj = j + roi.Left;

                    double TL = 0.0, TR = 0.0, BL = 0.0, BR = 0.0;
                    double sumPosRect = 0.0;

                    for (int k = 0; k < len; k += 4)
                    {
                        TL = integral[(ii + posRectPoints[k]) * width + jj + posRectPoints[k + 1]];
                        TR = integral[(ii + posRectPoints[k]) * width + jj + posRectPoints[k + 3]];
                        BL = integral[(ii + posRectPoints[k + 2]) * width + jj + posRectPoints[k + 1]];
                        BR = integral[(ii + posRectPoints[k + 2]) * width + jj + posRectPoints[k + 3]];

                        sumPosRect += (BR + TL - TR - BL);
                    }

                    TL = integral[ii * width + jj];
                    TR = integral[ii * width + jj + widthPtn];
                    BL = integral[(ii + heightPtn) * width + jj];
                    BR = integral[(ii + heightPtn) * width + jj + widthPtn];
                    double sumRect = (BR + TL - TR - BL);
                    scores[i, j] = sumPosRect - (sumRect - sumPosRect);
                }
            });

            double maxCorr = SiGlaz.Algorithms.Core.MatrixUtils.unsafeFindMaxElement(
                scores,
                ref shiftY,
                ref shiftX);
        }

        protected virtual unsafe void DetectCrossHaarPattern(GreyDataImage image, double[] integral, HaarPattern pattern, ref int shiftX, ref int shiftY)
        {
            int i, j, k;
            double TL = 0.0, TR = 0.0, BL = 0.0, BR = 0.0;

            ushort* data = image._aData;
            int width = image._width, height = image._height;

            int widthPtn = pattern.Width;
            int heightPtn = pattern.Height;
            int[] posRectPoints = pattern.PositiveRectPoints;
            int len = posRectPoints.Length;

            double[,] scores = new double[height - heightPtn, width - widthPtn];

            for (i = height - heightPtn - 1; i >= 0; i--)
            {
                for (j = width - widthPtn - 1; j >= 0; j--)
                {
                    double sumPosRect = 0.0;
                    for (k = 0; k < len; k += 4)
                    {
                        TL = integral[(i + posRectPoints[k]) * width + j + posRectPoints[k + 1]];
                        TR = integral[(i + posRectPoints[k]) * width + j + posRectPoints[k + 3]];
                        BL = integral[(i + posRectPoints[k + 2]) * width + j + posRectPoints[k + 1]];
                        BR = integral[(i + posRectPoints[k + 2]) * width + j + posRectPoints[k + 3]];

                        sumPosRect += (BR + TL - TR - BL);
                    }

                    TL = integral[i * width + j];
                    TR = integral[i * width + j + widthPtn];
                    BL = integral[(i + heightPtn) * width + j];
                    BR = integral[(i + heightPtn) * width + j + widthPtn];
                    double sumRect = (BR + TL - TR - BL);
                    scores[i, j] = sumPosRect - (sumRect - sumPosRect);
                }
            }

            double maxCorr = SiGlaz.Algorithms.Core.MatrixUtils.unsafeFindMaxElement(
                scores,
                ref shiftY,
                ref shiftX);
        }

        protected virtual void MatchKeyPoint(
            int isample, ushort[] roiData, double[] weightSet,
            SDD.Matrix transform,
            ref double confidence, ref int shiftX, ref int shiftY)
        {
            int roiWidth = _settings.SampleWidth + 2 * _settings.SampleExpandWidth;
            int roiHeight = _settings.SampleHeight + 2 * _settings.SampleExpandHeight;
            float halfRoiWidth = roiWidth * 0.5f;
            float halfRoiHeight = roiHeight * 0.5f;

            if (roiData == null)
                roiData = new ushort[roiWidth * roiHeight];

            transform.Translate(
                (float)_settings.SampleXCoordinates[isample] - halfRoiWidth,
                (float)_settings.SampleYCoordinates[isample] - halfRoiHeight,
                SDD.MatrixOrder.Prepend);

            unsafe
            {
                fixed (ushort* proiData = roiData)
                {
                    Interpolation.ImageInterpolator.AffineTransform(
                        InterpolationMethod.Bilinear,
                        _image._aData, _image._width, _image._height,
                        transform,
                        proiData, roiWidth, roiHeight);
                }
            }

            shiftX = 0; shiftY = 0;
            confidence =
                EstimateShiftUsingCoorelationCoef(
                _settings.SampleData[isample], _settings.SampleWidth, _settings.SampleHeight,
                _settings.SampleMeans[isample], _settings.SampleStds[isample] * _settings.SampleStds[isample],
                roiData, roiWidth, roiHeight, weightSet, ref shiftX, ref shiftY);


#if DEBUG && TEST
            SIADebugger.SaveImage(
                roiData, roiWidth, roiHeight,
                string.Format("D:\\temp\\test\\sample_{0}.bmp", isample));

            SIADebugger.SaveImage(
                _settings.SampleData[isample], _settings.SampleWidth, _settings.SampleHeight,
                string.Format("D:\\temp\\test\\pattern_{0}.bmp", isample));

            SIADebugger.SaveImages(
                roiData, roiWidth, roiHeight, 
                _settings.SampleData[isample], _settings.SampleWidth, _settings.SampleHeight,
                null, null,
                string.Format("D:\\temp\\test\\sample_pattern_{0}.bmp", isample));
#endif
        }

        protected virtual void MatchKeyPoint(
            int isample, ushort[] roiData, double[] weightSet,
            SDD.Matrix transform, SimpleMatcher matcher,
            ref double confidence, ref int shiftX, ref int shiftY)
        {
            int roiWidth = _settings.SampleWidth + 2 * _settings.SampleExpandWidth;
            int roiHeight = _settings.SampleHeight + 2 * _settings.SampleExpandHeight;
            float halfRoiWidth = roiWidth * 0.5f;
            float halfRoiHeight = roiHeight * 0.5f;

            if (roiData == null)
                roiData = new ushort[roiWidth * roiHeight];

            transform.Translate(
                (float)_settings.SampleXCoordinates[isample] - halfRoiWidth,
                (float)_settings.SampleYCoordinates[isample] - halfRoiHeight,
                SDD.MatrixOrder.Prepend);

            unsafe
            {
                fixed (ushort* proiData = roiData)
                {
                    Interpolation.ImageInterpolator.AffineTransform(
                        InterpolationMethod.Bilinear,
                        _image._aData, _image._width, _image._height,
                        transform,
                        proiData, roiWidth, roiHeight);
                }
            }

            unsafe
            {
                fixed (ushort* patternData = _settings.SampleData[isample], proiData = roiData)
                {
                    matcher.Match(
                        proiData, roiWidth, roiHeight,
                        patternData, _settings.SampleWidth, _settings.SampleHeight, 0,
                        out confidence, out shiftX, out shiftY);
                }
            }

#if DEBUG && TEST
            SIADebugger.SaveImage(
                roiData, roiWidth, roiHeight,
                string.Format("D:\\temp\\test\\sample_{0}.bmp", isample));

            SIADebugger.SaveImage(
                _settings.SampleData[isample], _settings.SampleWidth, _settings.SampleHeight,
                string.Format("D:\\temp\\test\\pattern_{0}.bmp", isample));

            int[] bestMatchs = new int[] { (shiftY + roiHeight / 2) * roiWidth + (shiftX + roiWidth / 2) };
            SIADebugger.SaveImages(
                roiData, roiWidth, roiHeight,
                _settings.SampleData[isample], _settings.SampleWidth, _settings.SampleHeight,
                null,
                bestMatchs,
                string.Format("D:\\temp\\test\\gradientmap_sample_pattern_{0}__{1}.bmp", isample, confidence.ToString("0.00000")));


            //confidence = 1.0;
#endif
        }

        protected virtual void GradientMapMatching(
            int isample, ushort[] roiData, double[] weightSet,
            System.Drawing.Drawing2D.Matrix transform,
            int featureCount, int pivotCount,
            ref double confidence, ref int shiftX, ref int shiftY)
        {
            int roiWidth = _settings.SampleWidth + 2 * _settings.SampleExpandWidth;
            int roiHeight = _settings.SampleHeight + 2 * _settings.SampleExpandHeight;
            float halfRoiWidth = roiWidth * 0.5f;
            float halfRoiHeight = roiHeight * 0.5f;

            if (roiData == null)
                roiData = new ushort[roiWidth * roiHeight];

            transform.Translate(
                (float)_settings.SampleXCoordinates[isample] - halfRoiWidth,
                (float)_settings.SampleYCoordinates[isample] - halfRoiHeight,
                SDD.MatrixOrder.Prepend);

            unsafe
            {
                fixed (ushort* proiData = roiData)
                {
                    Interpolation.ImageInterpolator.AffineTransform(
                        InterpolationMethod.Bilinear,
                        _image._aData, _image._width, _image._height,
                        transform,
                        proiData, roiWidth, roiHeight);
                }
            }

#if DEBUG && TEST
            SIADebugger.SaveImage(
                roiData, roiWidth, roiHeight,
                string.Format("D:\\temp\\test\\sample_{0}.bmp", isample));
#endif

            GradientMapMatcher ratingProcessor =
                new GradientMapMatcher(featureCount, pivotCount);
            unsafe
            {
                fixed (ushort* patternData = _settings.SampleData[isample], proiData = roiData)
                {
                    ratingProcessor.Match(
                        proiData, roiWidth, roiHeight,
                        patternData, _settings.SampleWidth, _settings.SampleHeight, 0,
                        out confidence, out shiftX, out shiftY);
                }
            }

            double[] refinedWeightSet = ratingProcessor.WeightSet;
            refinedWeightSet = weightSet;

            CorrelationMatcher matcher = new CorrelationMatcher();
            matcher.Match(
                roiData, roiWidth, roiHeight,
                _settings.SampleData[isample], _settings.SampleWidth, _settings.SampleHeight,
                refinedWeightSet, //weightSet, 
                0,
                ratingProcessor.InterestedXList, ratingProcessor.InterestedYList,
                out confidence, out shiftX, out shiftY);

#if DEBUG && TEST
            //SIADebugger.SaveImage(
            //    roiData, roiWidth, roiHeight,
            //    string.Format("D:\\temp\\test\\sample_{0}.bmp", isample));

            SIADebugger.SaveImage(
                _settings.SampleData[isample], _settings.SampleWidth, _settings.SampleHeight,
                string.Format("D:\\temp\\test\\pattern_{0}.bmp", isample));

            int[] bestMatchs = new int[] { (shiftY + roiHeight/2) * roiWidth + (shiftX + roiWidth/2) };
            SIADebugger.SaveImages(
                roiData, roiWidth, roiHeight,
                _settings.SampleData[isample], _settings.SampleWidth, _settings.SampleHeight,
                ratingProcessor.PivotIndices,
                bestMatchs,
                string.Format("D:\\temp\\test\\gradientmap_sample_pattern_{0}.bmp", isample));
#endif

            // set null here, hope that GC will collect successfully
            roiData = null;
        }

        protected double EstimateShiftUsingCoorelationCoef(
            ushort[] maskData, int maskWidth, int maskHeight, double maskMean, double maskStd,
            ushort[] roiData, int roiWidth, int roiHeight, ref int shiftX, ref int shiftY)
        {
            if (maskWidth > roiWidth || maskHeight > roiHeight)
                throw new System.ArgumentException("Invalid parameters");

            double[,] corrCoeff = SiGlaz.Algorithms.Core.MatrixUtils.CorrelationCoefficient(
                maskData,
                maskMean, maskStd,
                roiData,
                maskWidth,
                roiWidth);

            if (corrCoeff == null)
                throw new System.ExecutionEngineException("Correlation Coefficient Matrix was not defined");
            double maxCorr = SiGlaz.Algorithms.Core.MatrixUtils.unsafeFindMaxElement(
                corrCoeff,
                ref shiftY,
                ref shiftX);

            shiftX -= (roiWidth - maskWidth) / 2;
            shiftY -= (roiHeight - maskHeight) / 2;

            return maxCorr;
        }

        protected double EstimateShiftUsingCoorelationCoef(
            ushort[] maskData, int maskWidth, int maskHeight, double maskMean, double maskStd,
            ushort[] roiData, int roiWidth, int roiHeight, double[] weightset, ref int shiftX, ref int shiftY)
        {
            if (maskWidth > roiWidth || maskHeight > roiHeight)
                throw new System.ArgumentException("Invalid parameters");

            if (weightset == null)
            {
                return EstimateShiftUsingCoorelationCoef(
                    maskData, maskWidth, maskHeight, maskMean, maskStd,
                    roiData, roiWidth, roiHeight, ref shiftX, ref shiftY);
            }

            double[] ws = (double[])weightset.Clone();
            SiGlaz.Algorithms.Core.MatrixUtils.NormalizeWeightSet(ws, maskWidth);

            double[,] corrCoeff = SiGlaz.Algorithms.Core.MatrixUtils.CorrelationCoefficientWeightSet(
                maskData,
                roiData,
                maskWidth,
                roiWidth,
                ws);

            if (corrCoeff == null)
                throw new System.ExecutionEngineException("Correlation Coefficient Matrix was not defined");
            double maxCorr = SiGlaz.Algorithms.Core.MatrixUtils.unsafeFindMaxElement(
                corrCoeff,
                ref shiftY,
                ref shiftX);

            shiftX -= (roiWidth - maskWidth) / 2;
            shiftY -= (roiHeight - maskHeight) / 2;

            return maxCorr;
        }

        protected virtual SDD.Matrix FineTune(GreyDataImage image, SDD.Matrix draft)
        {
            //return AlignUsingMultipeKeypoints(image, draft);
            return AlignUsingMultipeKeypointsParallely(image, draft);
        }

        protected virtual void MatchKeypointsParallely(
            int idxBegin, int length, SDD.Matrix[] clonedDrafts,
            double deltaX, double deltaY,
            double[] xArray, double[] yArray,
            double[] shiftXArray, double[] shiftYArray,
            bool[] maskArray, double[] confidences)
        {
#if !PARALLEL && DEBUG
            for (int isample = idxBegin; isample < idxBegin + length; isample++)
#endif

#if PARALLEL
            Parallel.For(idxBegin, idxBegin + length, delegate(int isample)
#endif
            {
                int shiftX = 0, shiftY = 0;
                double corrCoeff = 0;

                using (SDD.Matrix getDraft = clonedDrafts[isample])
                {
                    // save
                    double seedX = _settings.SampleXCoordinates[isample];
                    double seedY = _settings.SampleYCoordinates[isample];
                    try
                    {
                        _settings.SampleXCoordinates[isample] += deltaX;
                        _settings.SampleYCoordinates[isample] += deltaY;

                        MatchKeyPoint(
                            isample, null, _settings.SampleWeightSet[isample],
                            getDraft,
                            ref corrCoeff, ref shiftX, ref shiftY);

                        shiftX = (int)Math.Round(shiftX + deltaX);
                        shiftY = (int)Math.Round(shiftY + deltaY);
                    }
                    finally
                    {
                        _settings.SampleXCoordinates[isample] = seedX;
                        _settings.SampleYCoordinates[isample] = seedY;
                    }
                }

                // confident
                confidences[isample] = corrCoeff;

                xArray[isample] = _settings.SampleXCoordinates[isample];
                yArray[isample] = _settings.SampleYCoordinates[isample];
                shiftXArray[isample] = shiftX;
                shiftYArray[isample] = shiftY;

                if (corrCoeff < _settings.MinCoorelationCoefficient)
                {
                    maskArray[isample] = false;
                }
                else
                {
                    maskArray[isample] = true;
                }
            }
#if PARALLEL
);
#endif

        }

        protected virtual SDD.Matrix AlignUsingMultipeKeypointsParallely(
            GreyDataImage image, SDD.Matrix draft)
        {
            // Initialize
            int nSamples = _settings.SampleCount;

            double[] xArray = new double[nSamples];
            double[] yArray = new double[nSamples];
            double[] shiftXArray = new double[nSamples];
            double[] shiftYArray = new double[nSamples];
            bool[] maskArray = new bool[nSamples];
            double[] confidences = new double[nSamples];
            SDD.Matrix[] clonedDrafts = new SDD.Matrix[nSamples];
            for (int i = 0; i < nSamples; i++)
            {
                clonedDrafts[i] = draft.Clone();
            }

            double deltaX = 0;
            double deltaY = 0;

            // save
            int oldExpandingWidth = _settings.SampleExpandWidth;
            int oldExpandingHeight = _settings.SampleExpandHeight;

            // Match Keypoints
            int idxBeginMatch = 0;
            bool bIsDraftHighConfidence = true;
            if (_settings.SampleCount > _settings.FineTuneNumSampleMatchFirstly &&
                _settings.FineTuneNumSampleMatchFirstly > 0)
            {
                _settings.SampleExpandWidth = (int)(_settings.SampleExpandWidth * 1.25);
                _settings.SampleExpandHeight = (int)(_settings.SampleExpandHeight * 1.25);

                MatchKeypointsParallely(
                    idxBeginMatch, _settings.FineTuneNumSampleMatchFirstly,
                    clonedDrafts, deltaX, deltaY,
                    xArray, yArray, shiftXArray, shiftYArray, maskArray, confidences);
                idxBeginMatch = _settings.FineTuneNumSampleMatchFirstly;

                for (int isample = 0; isample < _settings.FineTuneNumSampleMatchFirstly; isample++)
                {
                    deltaX += shiftXArray[isample];
                    deltaY += shiftYArray[isample];

#if TEST
                    System.Diagnostics.Trace.WriteLine(
                        string.Format("shiftX: {0}", shiftXArray[isample]));
                    System.Diagnostics.Trace.WriteLine(
                        string.Format("shiftY: {0}", shiftYArray[isample]));
#endif

                    if (confidences[isample] < _settings.FineTuneHighConfThres)
                    {
#if TEST
                        System.Diagnostics.Trace.WriteLine(        
                            string.Format("Confidenceeeeeeeee: {0}", confidences[isample]));
#endif

                        bIsDraftHighConfidence = false;
                        break;
                    }
                }


                if (bIsDraftHighConfidence)
                {
#if TEST
                    System.Diagnostics.Trace.WriteLine("We wonnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnn");                    
#endif

                    deltaX = deltaX / _settings.FineTuneNumSampleMatchFirstly;
                    deltaY = deltaY / _settings.FineTuneNumSampleMatchFirstly;

#if TEST
                    System.Diagnostics.Trace.WriteLine(
                        string.Format("deltaX: {0}", deltaX));
                    System.Diagnostics.Trace.WriteLine(
                        string.Format("deltaY: {0}", deltaY));
#endif

                    _settings.SampleExpandWidth = (int)(_settings.SampleExpandWidth * _settings.FineTuneExpandDecreaseFactor);
                    _settings.SampleExpandHeight = (int)(_settings.SampleExpandHeight * _settings.FineTuneExpandDecreaseFactor);

                    // I'm very expect that hence the location only around pivot markers
                    // reduce computing cost by descreasing scanning area :)
                    if (_settings.SampleExpandWidth > 15)
                        _settings.SampleExpandWidth = 15;
                    if (_settings.SampleExpandHeight > 15)
                        _settings.SampleExpandHeight = 15;

                    ResetPrivateParamsAfterPruning();
                }
                else
                {
                    deltaX = 0;
                    deltaY = 0;
                }

                MatchKeypointsParallely(
                    idxBeginMatch, nSamples - _settings.FineTuneNumSampleMatchFirstly,
                    clonedDrafts, deltaX, deltaY,
                    xArray, yArray, shiftXArray, shiftYArray, maskArray, confidences);
                // restore
                _settings.SampleExpandWidth = oldExpandingWidth;
                _settings.SampleExpandHeight = oldExpandingHeight;
            }
            else
            {
                MatchKeypointsParallely(
                    idxBeginMatch, nSamples,
                    clonedDrafts, deltaX, deltaY,
                    xArray, yArray, shiftXArray, shiftYArray, maskArray, confidences);
            }



            List<float> shiftXList = new List<float>();
            List<float> shiftYList = new List<float>();

            List<float> affectedXList = new List<float>();
            List<float> affectedYList = new List<float>();

            _matchedXList = xArray;
            _matchedYList = yArray;
            _matchedConfiences = confidences;

            // update affected list here
            for (int isample = 0; isample < nSamples; isample++)
            {
                if (maskArray[isample])
                {
                    affectedXList.Add((float)xArray[isample]);
                    affectedYList.Add((float)yArray[isample]);
                    shiftXList.Add((float)shiftXArray[isample]);
                    shiftYList.Add((float)shiftYArray[isample]);
                }

                _matchedXList[isample] += shiftXArray[isample];
                _matchedYList[isample] += shiftYArray[isample];
            }

            confidences = null;
            int minAffectedKeypoints = _settings.MinAffectedKeypoints;
            minAffectedKeypoints = 4;
            if (shiftXList.Count < minAffectedKeypoints)
                return draft.Clone();

#if TEST
            System.Diagnostics.Trace.WriteLine(
                string.Format(
                "We wonnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnn: {0}/{1}", 
                shiftXList.Count, _settings.SampleCount));
#endif

            SDD.Matrix fineTune = EstimateAffineOfKeypoints(affectedXList.ToArray(), affectedYList.ToArray(),
                shiftXList.ToArray(), shiftYList.ToArray());

            SDD.Matrix result = fineTune.Clone();
            result.Multiply(draft, System.Drawing.Drawing2D.MatrixOrder.Append);

            return result;
        }

        protected virtual void ResetPrivateParamsAfterPruning()
        {
        }

        protected virtual SDD.Matrix EstimateAffineOfKeypoints(int[] keyPointX, int[] keyPointY,
            int[] shiftX, int[] shiftY)
        {
            int len = keyPointX.Length;

            float[,] A = new float[2 * len, 6];
            float[,] b = new float[2 * len, 1];
            int row = -1;
            for (int i = 0; i < len; i++)
            {
                row++;
                A[row, 0] = keyPointX[i];
                A[row, 1] = keyPointY[i];
                A[row, 4] = 1;
                b[row, 0] = keyPointX[i] + shiftX[i];

                row++;
                A[row, 2] = keyPointX[i];
                A[row, 3] = keyPointY[i];
                A[row, 5] = 1;
                b[row, 0] = keyPointY[i] + shiftY[i];
            }

            float[,] pInvA = null;
            if (SiGlaz.Algorithms.Core.MatrixUtils.unsafePseudoInverse(A, ref pInvA) != 0)
                throw new System.ExecutionEngineException("Pseudo inverse of A was not defined");
            float[,] x = null;
            if (SiGlaz.Algorithms.Core.MatrixUtils.unsafeA_TperB(pInvA, b, ref x) != 0)
                throw new System.ExecutionEngineException("Multiplication failed");

            SDD.Matrix result = new System.Drawing.Drawing2D.Matrix(x[0, 0], x[2, 0], x[1, 0], x[3, 0], x[4, 0], x[5, 0]);

            return result;
        }

        protected virtual SDD.Matrix EstimateAffineOfKeypoints(float[] keyPointX, float[] keyPointY,
            float[] shiftX, float[] shiftY)
        {
            int len = keyPointX.Length;

            float[,] A = new float[2 * len, 6];
            float[,] b = new float[2 * len, 1];
            int row = -1;
            for (int i = 0; i < len; i++)
            {
                row++;
                A[row, 0] = keyPointX[i];
                A[row, 1] = keyPointY[i];
                A[row, 4] = 1;
                b[row, 0] = keyPointX[i] + shiftX[i];

                row++;
                A[row, 2] = keyPointX[i];
                A[row, 3] = keyPointY[i];
                A[row, 5] = 1;
                b[row, 0] = keyPointY[i] + shiftY[i];
            }

            float[,] pInvA = null;
            if (SiGlaz.Algorithms.Core.MatrixUtils.unsafePseudoInverse(A, ref pInvA) != 0)
                throw new System.ExecutionEngineException("Pseudo inverse of A was not defined");
            float[,] x = null;
            if (SiGlaz.Algorithms.Core.MatrixUtils.unsafeA_TperB(pInvA, b, ref x) != 0)
                throw new System.ExecutionEngineException("Multiplication failed");

            SDD.Matrix result = new System.Drawing.Drawing2D.Matrix(x[0, 0], x[2, 0], x[1, 0], x[3, 0], x[4, 0], x[5, 0]);

            return result;
        }
        #endregion Fine tuning step

        #region Helpers
        protected virtual unsafe ushort* GetROIData(
            GreyDataImage image, SDD.Matrix transform, int top, int left, int width, int height)
        {
            GreyDataImage result = new GreyDataImage(width, height);

            transform.Translate(left, top, System.Drawing.Drawing2D.MatrixOrder.Prepend);

            ImageInterpolator.AffineTransform(InterpolationMethod.Bilinear, image, transform, result);
            return result._aData;
        }

        public abstract void DetectDraftRegion(GreyDataImage image, RectRegion region);

        protected void LineIntersect(double a0, double b0, double a1, double b1, ref float x, ref float y)
        {
            x = (float)((b1 - b0) / (a0 - a1));
            y = (float)(a0 * x + b0);
        }

        protected void LineIntersect(double a0, double b0, double a1, double b1, ref double x, ref double y)
        {
            x = ((b1 - b0) / (a0 - a1));
            y = (a0 * x + b0);
        }

        protected double DistancePointToLine(int x, int y, double slope, double intercept)
        {
            double a = slope;
            double b = -1;
            double c = intercept;

            double d = (a * x + b * y + c) / Math.Sqrt(a * a + b * b);

            return d;
        }

        #endregion Helpers

        #region DEBUG Helpers
#if DEBUG
#endif
        #endregion DEBUG Helpers


        #region Detect key points
        public void DetectKeyPoints(
            GreyDataImage image, AlignmentResult alignmentResult,
            ref AlignmentResult ras,
            ref double[] xList, ref double[] yList, ref double[] confidences)
        {
            SDD.Matrix draftTransform = null;
            try
            {
                _image = image;

                draftTransform = new System.Drawing.Drawing2D.Matrix(
                    new RectangleF(0, 0, _settings.NewWidth, _settings.NewHeight),
                    alignmentResult.SourceCoordinates);

                ras = new AlignmentResult(0, 0, 0);
                PerformCorrectDraftResult(ref ras, draftTransform);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (draftTransform != null)
                {
                    draftTransform.Dispose();
                    draftTransform = null;
                }
            }
        }
        #endregion Detect key points
    }
}
