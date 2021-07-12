#define TEST_

#define PARALLEL

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SDD = System.Drawing.Drawing2D;

using SIA.Common;
using SIA.IPEngine;
using SIA.SystemLayer;
using SiGlaz.Common;
using SiGlaz.Common.ImageAlignment;
using SiGlaz.Algorithms.Core;
using SiGlaz.Preprocessing.Interpolation;
using SiGlaz.Preprocessing.Matching;


namespace SiGlaz.Preprocessing.Alignment
{
    public class Aligner
    {
        private Settings _settings = null;

        public Aligner(Settings settings)
        {
            Init(settings);
        }

        public void Init(Settings settings)
        {
            _settings = settings;
        }

        public AlignmentResult Align_ABS(GreyDataImage image)
        {
            if (image == null)
                return null;
            double slope = 0, topC = 0, bottomC = 0;
            ScanBoundaryVertical(image, _settings.KeyColumns, ref slope, ref topC, ref bottomC);

            int X_topleft = 0, Y_topleft = 0, X_bottomleft = 0, Y_bottomleft = 0;
            FindCorner(image, topC, bottomC, slope, ref X_topleft, ref Y_topleft, ref X_bottomleft, ref Y_bottomleft);

            int shiftXDraft = X_topleft;
            int shiftYDraft = Y_topleft;

            AlignmentResult result = new AlignmentResult(slope, 0, 0);
            float preferredWidth = _settings.NewWidth;
            float preferredDeltaX = (float)(preferredWidth / Math.Sqrt(1 + slope * slope));

            SDD.Matrix draftMatrix = new System.Drawing.Drawing2D.Matrix(new RectangleF(0, 0, _settings.NewWidth, _settings.NewHeight),
            new System.Drawing.PointF[] {
                new System.Drawing.PointF(X_topleft, Y_topleft), //upper-left in Cartesian
                new System.Drawing.PointF(X_topleft + preferredDeltaX, (float)(Y_topleft + preferredDeltaX * slope)),
                new System.Drawing.PointF(X_bottomleft, Y_bottomleft)});

            if (_settings.SampleCount < 6)
            {
                #region Single Keypoint method
                int shiftX = 0, shiftY = 0;
                int roiWidth = _settings.SampleWidth + 2 * _settings.SampleExpandWidth;
                int roiHeight = _settings.SampleHeight + 2 * _settings.SampleExpandHeight;
                int roiTop = _settings.SampleYCoordinates[0] - _settings.SampleExpandHeight;
                int roiLeft = _settings.SampleXCoordinates[0] - _settings.SampleExpandWidth;
                SDD.Matrix getMatrix = draftMatrix.Clone();
                unsafe
                {
                    ushort* roiData = GetROIData(image, getMatrix,
                        roiTop,
                        roiLeft,
                        roiWidth,
                        roiHeight);
                    fixed (ushort* pSample = _settings.SampleData[0])
                    {
#if TEST && DEBUG
                        using (System.IO.FileStream fs = new System.IO.FileStream(@"e:\temp\abs.bin", System.IO.FileMode.Create, System.IO.FileAccess.Write))
                        using (System.IO.BinaryWriter bin = new System.IO.BinaryWriter(fs))
                        {
                            bin.Write(roiWidth);
                            bin.Write(roiHeight);
                            int len = roiWidth * roiHeight;
                            ushort* proi = roiData;
                            for (int i = 0; i < len; i++, proi++)
                                bin.Write(*proi);

                            bin.Write(_settings.SampleWidth);
                            bin.Write(_settings.SampleHeight);
                            len = _settings.SampleWidth * _settings.SampleHeight;
                            proi = pSample;
                            for (int i = 0; i < len; i++, proi++)
                                bin.Write(*proi);
                        }
#endif

                        FindKeyPoint(roiData, roiWidth, roiHeight, pSample,
                            _settings.SampleWidth, _settings.SampleHeight,
                            ref shiftX, ref shiftY);
                    }
                }

                draftMatrix.Invert();
                draftMatrix.Translate(-shiftX, -shiftY, SDD.MatrixOrder.Append);
                draftMatrix.Invert();

                PointF[] resPoints = new PointF[] {
                new PointF(0, 0), new PointF(_settings.NewWidth, 0), new PointF(0, _settings.NewHeight)};
                draftMatrix.TransformPoints(resPoints);
                result.SourceCoordinates = resPoints;

                #endregion
            }
            else
            {
                #region Multiple Keypoints

                SDD.Matrix transform = AlignUsingMultipeKeypoints_ABS(image, draftMatrix);
                //SDD.Matrix transform = AlignUsingMultipeKeypoints2(image, draftMatrix, shiftXDraft, shiftYDraft);
                PointF[] resPoints = new PointF[] {
                new PointF(0, 0), new PointF(_settings.NewWidth, 0), new PointF(0, _settings.NewHeight)};
                transform.TransformPoints(resPoints);
                result.SourceCoordinates = resPoints;

                #endregion
            }

            return result;
        }

        public AlignmentResult Align_PoleTip(GreyDataImage image)
        {
            if (image == null)
                return null;
            double rotationAngle = 0.0, shiftX = 0.0, shiftY = 0;
            double anchorX=0.0, anchorY=0.0;
            //ScanBoundaryHorizontal_PoleTip(image, _settings.KeyRowsPoleTip, ref rotationAngle, ref shift);
            ScanWhiteRegion_PoleTip(image,_settings, ref anchorX, ref anchorY);

            shiftX = anchorX- _settings.AnchorX;
            shiftY = anchorY - _settings.AnchorY;


            AlignmentResult result = new AlignmentResult(0, 0, rotationAngle);
            SDD.Matrix draftMatrix = new SDD.Matrix();
            draftMatrix.Translate((float)shiftX, (float)shiftY);
            //draftMatrix.Rotate((float)rotationAngle);

            SDD.Matrix transform = AlignUsingMultipeKeypoints_PoleTip(image, draftMatrix);
            //SDD.Matrix transform = draftMatrix;
            PointF[] resPoints = new PointF[] {
                new PointF(0, 0), new PointF(_settings.NewWidth, 0), new PointF(0, _settings.NewHeight)};
            transform.TransformPoints(resPoints);
            result.SourceCoordinates = resPoints;

            return result;
        }

        #region ABS Draft step
        public double GetRotationAngle(int[] colsIdx, int[] data)
        {
            int lengthCol = colsIdx.Length;
            int lengthData = data.Length;
            double deltaX = colsIdx[lengthCol - 1] - colsIdx[0];
            double deltaY = data[lengthData - 1] - data[0];

            double angle = Math.Atan2(deltaY, deltaX);
            return angle;
        }

        private unsafe void FindCorner(GreyDataImage image, double topC, double bottomC, double rotationAngle, ref int X_topleft, ref int Y_topleft, ref int X_bottomleft, ref int Y_bottomleft)
        {
            int width = image._width;
            int height = image._height;
            ushort thresGrey = (ushort)_settings.IntensityThreshold;

            int deltaC = (int)(bottomC - topC);
            int offsetC = deltaC / 10;
            int beginC = (int)topC + offsetC;

            int length = deltaC - offsetC * 2;

            int[] xCoordsLeft = new int[length];
            ushort data = 0;

            ushort* greyBuffer = image._aData;

            int indexPixel = beginC * width;
            for (int i = 0; i < length; i++)
            {
                //indexPixel = (beginC + i) * width;
                for (int j = 0; j < width; j++)
                {
                    //data = image.getPixel(j, beginC + i);
                    data = greyBuffer[indexPixel + j];
                    if (data > thresGrey)
                    {
                        xCoordsLeft[i] = j;
                        break;
                    }
                }
                indexPixel += width;
            }

            double avgX = 0.0;
            double avgY = 0.0;
            for (int i = 0; i < length; i++)
            {
                avgX += xCoordsLeft[i];
                avgY += beginC + i;
            }
            avgX /= length;
            avgY /= length;

            double x, y;

            // y = ax + b => b = y - ax;
            double a0 = -1.0 / rotationAngle;
            double b0 = avgY - avgX * a0;

            double a1 = rotationAngle;
            double b1 = topC;
            x = (b1 - b0) / (a0 - a1);
            y = a0 * x + b0;
            X_topleft = (int)x;
            Y_topleft = (int)y;

            double a2 = rotationAngle;
            double b2 = bottomC;
            x = (b2 - b0) / (a0 - a2);
            y = a0 * x + b0;
            X_bottomleft = (int)x;
            Y_bottomleft = (int)y;
        }

        private unsafe void ScanBoundaryVertical(GreyDataImage image, int[] colsIdx, ref double rotationAngle, ref double topC, ref double bottomC)
        {
            if (colsIdx == null)
                return;

            int width = image._width;
            int height = image._height;

            int length = colsIdx.Length;
            int[] yCoordsTop = new int[length];
            int[] yCoordsBottom = new int[length];
            ushort data = 0;

            ushort thresGrey = (ushort)_settings.IntensityThreshold;

            ushort* greyBuffer = image._aData;

            int indexPixel = 0;
            for (int j = 0; j < length; j++)
            {
                indexPixel = 0;
                for (int i = 0; i < height; i++)
                {
                    //data = imgOriginal_Gray.Data[i, j, depth];
                    //data = image.getPixel(colsIdx[j], i);
                    //indexPixel = i * width;
                    data = greyBuffer[indexPixel + colsIdx[j]];
                    if (data > thresGrey)
                    {
                        yCoordsTop[j] = i;
                        break;
                    }
                    indexPixel += width;
                }
                indexPixel = (height - 1) * width;
                for (int i = height - 1; i >= 0; i--)
                {
                    //data = imgOriginal_Gray.Data[i, j, depth];
                    //data = image.getPixel(colsIdx[j], i);
                    //indexPixel = i * width;
                    data = greyBuffer[indexPixel + colsIdx[j]];
                    if (data > thresGrey)
                    {
                        yCoordsBottom[j] = i;
                        break;
                    }
                    indexPixel -= width;
                }
            }

            if (EstimateHorizontalLinesByLSQ(colsIdx, yCoordsTop, colsIdx, yCoordsBottom,
                ref rotationAngle, ref topC, ref bottomC) != 0)
                throw new Exception("Estimate Horizontal Lines by LSQ failed");
        }

        private int EstimateHorizontalLinesByLSQ(int[] topXCoords, int[] topYCoords, int[] bottomXCoords, int[] bottomYCoords,
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
                if (MatrixUtils.unsafePseudoInverse(A, ref pInvA) != 0)
                    throw new System.ExecutionEngineException("Pseudoinverse of A was not defined");
                float[,] x = null;
                if (MatrixUtils.unsafeA_TperB(pInvA, b, ref x) != 0)
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

        private int EstimateHorizontalLinesByLSQ(int[] XCoords, int[] YCoords,
            ref double angle, ref double C)
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
                if (MatrixUtils.unsafePseudoInverse(A, ref pInvA) != 0)
                    throw new System.ExecutionEngineException("Pseudoinverse of A was not defined");
                float[,] x = null;
                if (MatrixUtils.unsafeA_TperB(pInvA, b, ref x) != 0)
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

        private unsafe ushort* GetROIData(GreyDataImage image, SDD.Matrix transform, int top, int left, int width, int height)
        {
            GreyDataImage result = new GreyDataImage(width, height);

            transform.Translate(left, top, System.Drawing.Drawing2D.MatrixOrder.Prepend);

            ImageInterpolator.AffineTransform(InterpolationMethod.Bilinear, image, transform, result);
            return result._aData;
        }

        private unsafe void FindKeyPoint(ushort* data, int width, int height,
            ushort* dataMask, int widthMask, int heightMask,
            ref int xMatch, ref int yMatch)
        {
            int i = 0, j = 0, u = 0, v = 0;

            double deltaIntensity = 0;
            double minDeltaIntensity = double.MaxValue;

            for (i = 0; i + heightMask < height; i++)
            {
                for (j = 0; j + widthMask < width; j++)
                {
                    deltaIntensity = 0;
                    ushort* pMask = dataMask;
                    ushort* pData = null;
                    for (u = 0; u < heightMask; u++)
                    {
                        pData = data + (i + u) * width + j;
                        for (v = 0; v < widthMask; v++, pMask++, pData++)
                        {
                            ushort ppppmask = *pMask;
                            int delta = (*pMask - *pData);
                            delta *= delta;
                            delta *= *pMask;
                            deltaIntensity += delta;
                        }
                    }

                    if (deltaIntensity < minDeltaIntensity)
                    {
                        minDeltaIntensity = deltaIntensity;
                        xMatch = j;
                        yMatch = i;
                    }
                }
            }

            int expWidth = (width - widthMask) / 2;
            int expHeight = (height - heightMask) / 2;

            xMatch -= expWidth;
            yMatch -= expHeight;
        }
        #endregion

        #region Pole Tip Draft step

        private unsafe void ScanBoundaryHorizontal_PoleTip(GreyDataImage image, int[] rowsIdx, ref double rotationAngle, ref double shift)
        {
            if (rowsIdx == null)
                return;

            int width = image._width;
            int height = image._height;

            int length = rowsIdx.Length;
            int[] xCoordsRight = new int[length];
            int avgCoords_Smooth = 0;

            ushort thresGrey = (ushort)_settings.IntensityThreshold;
            ushort* greyBuffer = image._aData;

            int MARGIN_RIGHT = 5;

            for (int i = 0; i < length; i++)
            {
                ushort* pData = greyBuffer + ((rowsIdx[i] - 1) * width) - MARGIN_RIGHT;
                for (int j = width - MARGIN_RIGHT - 1; j >= 0; j--, pData--)
                {
                    if (*pData > thresGrey)
                    {
                        xCoordsRight[i] = j;
                        avgCoords_Smooth += j;
                        break;
                    }
                }
            }
            avgCoords_Smooth /= length;

            if (EstimateHorizontalLinesByLSQ(rowsIdx, xCoordsRight,
                ref rotationAngle, ref shift) != 0) // Reverse X, Y because our line is Vertical, function line is Horizontal
                throw new Exception("Estimate Horizontal Lines by LSQ failed");

            shift = shift - width;
        }

        public unsafe void ScanWhiteRegion_PoleTip(GreyDataImage image, Settings settings, ref double anchorX, ref double anchorY)
        {
            ScanWhiteRegion_PoleTip(image, settings.TopScanWhite, settings.BottomScanWhite, settings.LeftScanWhite, settings.RightScanWhite, ref anchorX, ref anchorY);
        }

        public unsafe void ScanWhiteRegion_PoleTip(GreyDataImage image, int topScan, int bottomScan, int leftScan, int rightScan, ref double anchorX, ref double anchorY)
        {
            anchorX = 0;
            anchorY = 0;
            int width = image._width;
            int height = image._height;

            ushort thresGrey = (ushort)_settings.IntensityThreshold;
            ushort* greyBuffer = image._aData;

            double sumX = 0;
            double sumY = 0;
            int count = 0;
            for (int i = topScan; i < bottomScan; i++)
            {
                ushort* pDataInc = greyBuffer + i * width + leftScan;
                ushort* pDataDec = greyBuffer + i * width + rightScan;
                int jDec = rightScan;
                bool leftFound = false;
                bool rightFound = false;
                for (int j = leftScan; j < rightScan; j++, pDataInc++, pDataDec--, jDec--)
                {
                    if (leftFound && rightFound)
                        break;
                    if (!leftFound && *pDataInc > thresGrey)
                    {
                        sumX += j;
                        sumY += i;
                        count++;
                        leftFound = true;
                    }

                    if (!rightFound && *pDataDec > thresGrey)
                    {
                        sumX += jDec;
                        sumY += i;
                        count++;
                        rightFound = true;
                    }
                }
            }

            if (count == 0)
                return;

            anchorX = sumX / count;
            anchorY = sumY / count;
        }

        #endregion

        #region Fine tuning step

        private double EstimateShiftUsingCoorelationCoef(ushort[] maskData, int maskWidth, int maskHeight, double maskMean, double maskStd,
            ushort[] roiData, int roiWidth, int roiHeight, ref int shiftX, ref int shiftY)
        {
            if (maskWidth > roiWidth || maskHeight > roiHeight)
                throw new System.ArgumentException("Invalid parameters");

            double[,] corrCoeff = MatrixUtils.CorrelationCoefficient(
                maskData,
                maskMean, maskStd,
                roiData,
                maskWidth,
                roiWidth);

            if (corrCoeff == null)
                throw new System.ExecutionEngineException("Correlation Coefficient Matrix was not defined");
            double maxCorr = MatrixUtils.unsafeFindMaxElement(
                corrCoeff,
                ref shiftY,
                ref shiftX);

            shiftX -= (roiWidth - maskWidth) / 2;
            shiftY -= (roiHeight - maskHeight) / 2;

            return maxCorr;
        }

        private double EstimateShiftUsingCoorelationCoefWeightSet(ushort[] maskData, int maskWidth, int maskHeight, double maskMean, double maskStd,
            ushort[] roiData, int roiWidth, int roiHeight, double[] weightset, ref int shiftX, ref int shiftY)
        {
            if (maskWidth > roiWidth || maskHeight > roiHeight)
                throw new System.ArgumentException("Invalid parameters");

            double[] ws = (double[])weightset.Clone();
            MatrixUtils.NormalizeWeightSet(ws, maskWidth);
            //double[,] corrCoeff = MatrixUtils.CorrelationCoefficientWeightSet(
            //    maskData,
            //    maskMean, maskStd,
            //    roiData,
            //    maskWidth,
            //    roiWidth,
            //    ws);
            double[,] corrCoeff = MatrixUtils.CorrelationCoefficientWeightSet(
                maskData,
                roiData,
                maskWidth,
                roiWidth,
                ws);
#if TEST && DEBUG
            using (System.IO.FileStream fs = new System.IO.FileStream(@"d:\temp\corr.bin",
                System.IO.FileMode.Create, System.IO.FileAccess.Write))
            using (System.IO.BinaryWriter bin = new System.IO.BinaryWriter(fs))
            unsafe
            {
                int length0 = corrCoeff.GetLength(0);
                int length1 = corrCoeff.GetLength(0);
                bin.Write(length0);
                bin.Write(length1);
                for (int i = 0; i < length0; i++)
                    for (int j = 0; j < length1; j++)
                    {
                        bin.Write(corrCoeff[i, j]);
                    }
            }
#endif


            if (corrCoeff == null)
                throw new System.ExecutionEngineException("Correlation Coefficient Matrix was not defined");
            double maxCorr = MatrixUtils.unsafeFindMaxElement(
                corrCoeff,
                ref shiftY,
                ref shiftX);

            shiftX -= (roiWidth - maskWidth) / 2;
            shiftY -= (roiHeight - maskHeight) / 2;

            return maxCorr;
        }

        public SDD.Matrix AlignUsingMultipeKeypoints_ABS(GreyDataImage image, SDD.Matrix draft)
        {
#if PARALLEL
            return AlignUsingMultipeKeypoints_ABS_Parallely(image, draft);
#endif
            List<int> shiftXList = new List<int>();
            List<int> shiftYList = new List<int>();

            List<int> affectedXList = new List<int>();
            List<int> affectedYList = new List<int>();

            List<double> corrCoeffList = new List<double>();

            int roiWidth = _settings.SampleWidth + 2 * _settings.SampleExpandWidth;
            int roiHeight = _settings.SampleHeight + 2 * _settings.SampleExpandHeight;
            int halfRoiWidth = roiWidth / 2;
            int halfRoiHeight = roiHeight / 2;

            ushort[] roiData = new ushort[roiWidth * roiHeight];

            for (int isample = 0; isample < _settings.SampleCount; isample++)
            {
                SDD.Matrix getDraft = draft.Clone();
                getDraft.Translate(_settings.SampleXCoordinates[isample] - halfRoiWidth,
                    _settings.SampleYCoordinates[isample] - halfRoiHeight, System.Drawing.Drawing2D.MatrixOrder.Prepend);

                unsafe
                {
                    fixed (ushort* proiData = roiData)
                    {

                        Interpolation.ImageInterpolator.AffineTransform(InterpolationMethod.Bilinear,
                            image._aData, image._width, image._height, getDraft, proiData, roiWidth, roiHeight);
                    }
                }

                int shiftX = 0, shiftY = 0;
                double corrCoeff = EstimateShiftUsingCoorelationCoef(_settings.SampleData[isample], _settings.SampleWidth, _settings.SampleHeight,
                    _settings.SampleMeans[isample], _settings.SampleStds[isample] * _settings.SampleStds[isample],
                    roiData, roiWidth, roiHeight, ref shiftX, ref shiftY);
                //double corrCoeff = EstimateShiftUsingCoorelationCoefWeightSet(_settings.SampleData[isample], _settings.SampleWidth, _settings.SampleHeight,
                //    _settings.SampleMeans[isample], _settings.SampleStds[isample] * _settings.SampleStds[isample],
                //    roiData, roiWidth, roiHeight, _settings.SampleWeightSet[isample], ref shiftX, ref shiftY);

#if TEST && DEBUG
                using (System.IO.FileStream fs = new System.IO.FileStream(@"d:\temp\abs.bin" + (isample+1).ToString(), 
                    System.IO.FileMode.Create, System.IO.FileAccess.Write))
                using (System.IO.BinaryWriter bin = new System.IO.BinaryWriter(fs))
                unsafe
                {
                    bin.Write(roiWidth);
                    bin.Write(roiHeight);
                    int len = roiWidth * roiHeight;
                    fixed (ushort* proi = roiData, psample = _settings.SampleData[isample])
                    {
                        ushort* pproi = proi;
                        for (int i = 0; i < len; i++, pproi++)
                            bin.Write(*pproi);

                        bin.Write(_settings.SampleWidth);
                        bin.Write(_settings.SampleHeight);
                        bin.Write(shiftX/* - _settings.SampleExpandWidth*/);
                        bin.Write(shiftY/* - _settings.SampleExpandHeight*/);
                        len = _settings.SampleWidth * _settings.SampleHeight;
                        pproi = psample;
                        for (int i = 0; i < len; i++, pproi++)
                            bin.Write(*pproi);
                    }
                }
#endif

                corrCoeffList.Add(corrCoeff);
                if (corrCoeff < _settings.MinCoorelationCoefficient)
                    continue;

                affectedXList.Add(_settings.SampleXCoordinates[isample]);
                affectedYList.Add(_settings.SampleYCoordinates[isample]);
                shiftXList.Add(shiftX);
                shiftYList.Add(shiftY);
            }

            if (shiftXList.Count < _settings.MinAffectedKeypoints)
                return draft.Clone();


            SDD.Matrix fineTune = EstimateAffineOfKeypoints(affectedXList.ToArray(), affectedYList.ToArray(),
                shiftXList.ToArray(), shiftYList.ToArray());

            SDD.Matrix result = fineTune.Clone();
            result.Multiply(draft, System.Drawing.Drawing2D.MatrixOrder.Append);

#if TEST && DEBUG
            PointF[] test = new PointF[_settings.SampleCount];
            for (int i = 0; i < _settings.SampleCount; i++)
            {
                test[i] = new PointF(_settings.SampleXCoordinates[i], _settings.SampleYCoordinates[i]);
            }
            result.TransformPoints(test);
            for (int i = 0; i < _settings.SampleCount; i++)
            {
                Console.WriteLine("{0},{1};...", test[i].X, test[i].Y);
            }
#endif
            return result;
        }

        public SDD.Matrix AlignUsingMultipeKeypoints_ABS_Parallely(GreyDataImage image, SDD.Matrix draft)
        {
            List<int> shiftXList = new List<int>();
            List<int> shiftYList = new List<int>();

            List<int> affectedXList = new List<int>();
            List<int> affectedYList = new List<int>();

            List<double> corrCoeffList = new List<double>();

            int roiWidth = _settings.SampleWidth + 2 * _settings.SampleExpandWidth;
            int roiHeight = _settings.SampleHeight + 2 * _settings.SampleExpandHeight;
            int halfRoiWidth = roiWidth / 2;
            int halfRoiHeight = roiHeight / 2;

            int nSamples = _settings.SampleCount;

            int[] xArray = new int[nSamples];
            int[] yArray = new int[nSamples];
            int[] shiftXArray = new int[nSamples];
            int[] shiftYArray = new int[nSamples];
            bool[] maskArray = new bool[nSamples];
            SDD.Matrix[] clonedDrafts = new SDD.Matrix[nSamples];
            for (int i = 0; i < nSamples; i++)
            {
                clonedDrafts[i] = draft.Clone();
            }

            //for (int isample = 0; isample < _settings.SampleCount; isample++)
            Parallel.For(0, nSamples, delegate(int isample)
            {
                SDD.Matrix getDraft = clonedDrafts[isample];
                getDraft.Translate(_settings.SampleXCoordinates[isample] - halfRoiWidth,
                    _settings.SampleYCoordinates[isample] - halfRoiHeight, System.Drawing.Drawing2D.MatrixOrder.Prepend);

                ushort[] roiData = new ushort[roiWidth * roiHeight];
                unsafe
                {
                    fixed (ushort* proiData = roiData)
                    {
                        Interpolation.ImageInterpolator.AffineTransform(InterpolationMethod.Bilinear,
                            image._aData, image._width, image._height, getDraft, proiData, roiWidth, roiHeight);
                    }
                }
                if (getDraft != null)
                {
                    getDraft.Dispose();
                    getDraft = null;
                    clonedDrafts[isample] = null;
                }


                //int shiftX = 0, shiftY = 0;
                //double corrCoeff = EstimateShiftUsingCoorelationCoef(_settings.SampleData[isample], _settings.SampleWidth, _settings.SampleHeight,
                //    _settings.SampleMeans[isample], _settings.SampleStds[isample] * _settings.SampleStds[isample],
                //    roiData, roiWidth, roiHeight, ref shiftX, ref shiftY);
                int shiftX = 0, shiftY = 0;
                double corrCoeff = 0;
                GradientMapMatcher ratingProcessor = new GradientMapMatcher();
                unsafe
                {
                    fixed (ushort* patternData = _settings.SampleData[isample], proiData = roiData)
                    {
                        ratingProcessor.Match(
                            proiData, roiWidth, roiHeight,
                            patternData, _settings.SampleWidth, _settings.SampleHeight, 0,
                            out corrCoeff, out shiftX, out shiftY);                        
                    }
                }

                CorrelationMatcher matcher = new CorrelationMatcher();
                matcher.Match(
                    roiData, roiWidth, roiHeight,
                    _settings.SampleData[isample], _settings.SampleWidth, _settings.SampleHeight, 0,
                    ratingProcessor.InterestedXList, ratingProcessor.InterestedYList,
                    out corrCoeff, out shiftX, out shiftY);                

                // set null here, hope that GC will collect successfully
                roiData = null;

                // in parallel mode, not collect this value
                // corrCoeffList.Add(corrCoeff);
                if (corrCoeff < _settings.MinCoorelationCoefficient)
                {
                    maskArray[isample] = false;

                    // nothing here
                    //continue;
                }
                else
                {
                    maskArray[isample] = true;

                    xArray[isample] = _settings.SampleXCoordinates[isample];
                    yArray[isample] = _settings.SampleYCoordinates[isample];
                    shiftXArray[isample] = shiftX;
                    shiftYArray[isample] = shiftY;
                }
            });

            // update affected list here
            for (int isample = 0; isample < nSamples; isample++)
            {
                if (!maskArray[isample])
                    continue;

                affectedXList.Add(xArray[isample]);
                affectedYList.Add(yArray[isample]);
                shiftXList.Add(shiftXArray[isample]);
                shiftYList.Add(shiftYArray[isample]);
            }

            if (shiftXList.Count < _settings.MinAffectedKeypoints)
                return draft.Clone();

            SDD.Matrix fineTune = EstimateAffineOfKeypoints(affectedXList.ToArray(), affectedYList.ToArray(),
                shiftXList.ToArray(), shiftYList.ToArray());

            SDD.Matrix result = fineTune.Clone();
            result.Multiply(draft, System.Drawing.Drawing2D.MatrixOrder.Append);

            return result;
        }

        public SDD.Matrix AlignUsingMultipeKeypoints_PoleTip(GreyDataImage image, SDD.Matrix draft)
        {
            List<int> shiftXList = new List<int>();
            List<int> shiftYList = new List<int>();

            List<int> affectedXList = new List<int>();
            List<int> affectedYList = new List<int>();

            List<double> corrCoeffList = new List<double>();

            int roiWidth = _settings.SampleWidth + 2 * _settings.SampleExpandWidth;
            int roiHeight = _settings.SampleHeight + 2 * _settings.SampleExpandHeight;
            int halfRoiWidth = roiWidth / 2;
            int halfRoiHeight = roiHeight / 2;

            ushort[] roiData = new ushort[roiWidth * roiHeight];

            for (int isample = 0; isample < _settings.SampleCount; isample++)
            {
                SDD.Matrix getDraft = draft.Clone();
                getDraft.Translate(_settings.SampleXCoordinates[isample] - halfRoiWidth,
                    _settings.SampleYCoordinates[isample] - halfRoiHeight, System.Drawing.Drawing2D.MatrixOrder.Prepend);

                unsafe
                {
                    fixed (ushort* proiData = roiData)
                    {

                        Interpolation.ImageInterpolator.AffineTransform(InterpolationMethod.Bilinear,
                            image._aData, image._width, image._height, getDraft, proiData, roiWidth, roiHeight);
                    }
                }

                int shiftX = 0, shiftY = 0;
                //double corrCoeff = EstimateShiftUsingCoorelationCoef(_settings.SampleData[isample], _settings.SampleWidth, _settings.SampleHeight,
                //    _settings.SampleMeans[isample], _settings.SampleStds[isample] * _settings.SampleStds[isample],
                //    roiData, roiWidth, roiHeight, ref shiftX, ref shiftY);
                double corrCoeff = EstimateShiftUsingCoorelationCoefWeightSet(_settings.SampleData[isample], _settings.SampleWidth, _settings.SampleHeight,
                    _settings.SampleMeans[isample], _settings.SampleStds[isample] * _settings.SampleStds[isample],
                    roiData, roiWidth, roiHeight, _settings.SampleWeightSet[isample], ref shiftX, ref shiftY);

#if TEST && DEBUG
                using (System.IO.FileStream fs = new System.IO.FileStream(@"d:\temp\abs.bin" + (isample+1).ToString(), 
                    System.IO.FileMode.Create, System.IO.FileAccess.Write))
                using (System.IO.BinaryWriter bin = new System.IO.BinaryWriter(fs))
                unsafe
                {
                    bin.Write(roiWidth);
                    bin.Write(roiHeight);
                    int len = roiWidth * roiHeight;
                    fixed (ushort* proi = roiData, psample = _settings.SampleData[isample])
                    {
                        ushort* pproi = proi;
                        for (int i = 0; i < len; i++, pproi++)
                            bin.Write(*pproi);

                        bin.Write(_settings.SampleWidth);
                        bin.Write(_settings.SampleHeight);
                        bin.Write(shiftX/* - _settings.SampleExpandWidth*/);
                        bin.Write(shiftY/* - _settings.SampleExpandHeight*/);
                        len = _settings.SampleWidth * _settings.SampleHeight;
                        pproi = psample;
                        for (int i = 0; i < len; i++, pproi++)
                            bin.Write(*pproi);
                    }
                }
#endif

                corrCoeffList.Add(corrCoeff);
                if (corrCoeff < _settings.MinCoorelationCoefficient)
                    continue;

                affectedXList.Add(_settings.SampleXCoordinates[isample]);
                affectedYList.Add(_settings.SampleYCoordinates[isample]);
                shiftXList.Add(shiftX);
                shiftYList.Add(shiftY);
            }

            if (shiftXList.Count < _settings.MinAffectedKeypoints)
                return draft.Clone();


            SDD.Matrix fineTune = EstimateAffineOfKeypoints(affectedXList.ToArray(), affectedYList.ToArray(),
                shiftXList.ToArray(), shiftYList.ToArray());

            SDD.Matrix result = fineTune.Clone();
            result.Multiply(draft, System.Drawing.Drawing2D.MatrixOrder.Append);

#if TEST && DEBUG
            PointF[] test = new PointF[_settings.SampleCount];
            for (int i = 0; i < _settings.SampleCount; i++)
            {
                test[i] = new PointF(_settings.SampleXCoordinates[i], _settings.SampleYCoordinates[i]);
            }
            result.TransformPoints(test);
            for (int i = 0; i < _settings.SampleCount; i++)
            {
                Console.WriteLine("{0},{1};...", test[i].X, test[i].Y);
            }
#endif
            return result;
        }

        private SDD.Matrix AlignUsingMultipeKeypoints2(GreyDataImage image, SDD.Matrix draft, int shiftXDraft, int shiftYDraft)
        {
            List<int> shiftXList = new List<int>();
            List<int> shiftYList = new List<int>();

            List<int> affectedXList = new List<int>();
            List<int> affectedYList = new List<int>();

            int roiWidth = _settings.SampleWidth + 2 * _settings.SampleExpandWidth;
            int roiHeight = _settings.SampleHeight + 2 * _settings.SampleExpandHeight;
            int halfRoiWidth = roiWidth / 2;
            int halfRoiHeight = roiHeight / 2;

            ushort[] roiData = new ushort[roiWidth * roiHeight];

            for (int isample = 0; isample < _settings.SampleCount; isample++)
            {
                SDD.Matrix transformMat = new System.Drawing.Drawing2D.Matrix();
                transformMat.Translate(shiftXDraft, shiftYDraft, System.Drawing.Drawing2D.MatrixOrder.Prepend);
                transformMat.Translate(_settings.SampleXCoordinates[isample] - halfRoiWidth,
                    _settings.SampleYCoordinates[isample] - halfRoiHeight, System.Drawing.Drawing2D.MatrixOrder.Prepend);


                //SDD.Matrix getDraft = draft.Clone();
                //getDraft.Translate(_settings.SampleXCoordinates[isample] - halfRoiWidth,
                //    _settings.SampleYCoordinates[isample] - halfRoiHeight, System.Drawing.Drawing2D.MatrixOrder.Prepend);

                unsafe
                {
                    fixed (ushort* proiData = roiData)
                    {

                        Interpolation.ImageInterpolator.AffineTransform(InterpolationMethod.Bilinear,
                            image._aData, image._width, image._height, transformMat, proiData, roiWidth, roiHeight);
                    }
                }

                int shiftX = 0, shiftY = 0;
                double corrCoeff = EstimateShiftUsingCoorelationCoef(_settings.SampleData[isample], _settings.SampleWidth, _settings.SampleHeight,
                    _settings.SampleMeans[isample], _settings.SampleStds[isample] * _settings.SampleStds[isample],
                    roiData, roiWidth, roiHeight, ref shiftX, ref shiftY);


                if (corrCoeff < _settings.MinCoorelationCoefficient)
                    continue;

                affectedXList.Add(_settings.SampleXCoordinates[isample]);
                affectedYList.Add(_settings.SampleYCoordinates[isample]);
                shiftXList.Add(shiftX + shiftXDraft);
                shiftYList.Add(shiftY + shiftYDraft);
            }

            //if (shiftXList.Count < _settings.MinAffectedKeypoints)
            //return draft.Clone();


            SDD.Matrix fineTune = EstimateAffineOfKeypoints(affectedXList.ToArray(), affectedYList.ToArray(),
                shiftXList.ToArray(), shiftYList.ToArray());

            SDD.Matrix result = fineTune.Clone();
            //result.Multiply(draft, System.Drawing.Drawing2D.MatrixOrder.Append);

            return result;
        }

        private SDD.Matrix EstimateAffineOfKeypoints(int[] keyPointX, int[] keyPointY,
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
            if (MatrixUtils.unsafePseudoInverse(A, ref pInvA) != 0)
                throw new System.ExecutionEngineException("Pseudo inverse of A was not defined");
            float[,] x = null;
            if (MatrixUtils.unsafeA_TperB(pInvA, b, ref x) != 0)
                throw new System.ExecutionEngineException("Multiplication failed");

            SDD.Matrix result = new System.Drawing.Drawing2D.Matrix(x[0, 0], x[2, 0], x[1, 0], x[3, 0], x[4, 0], x[5, 0]);

            return result;
        }

        #endregion

        #region IR
        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <param name="beginTop"></param>
        /// <param name="endBottom"></param>
        /// <param name="border">image margin, in some case grey value is 255 because of the radius of convolution kernel on previous step.</param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public int ScanIRVerticalLine(GreyDataImage image, int beginTop, int endBottom, int border, int threshold, ref double angle, ref double shift)
        {
            int width = image._width;
            int height = image._height;
            ushort data = 0;

            int length = endBottom - beginTop + 1;
            int[] Coords = new int[length];
            int avgCoords = 0;

            int i, j;

            unsafe
            {
                ushort* greyBuffer = image._aData;
                greyBuffer += beginTop * width;
                for (i = 0; i < length; i++)
                {
                    for (j = border; j < width; j++)
                    {
                        data = greyBuffer[j];
                        if (data > threshold)
                            break;
                    }
                    Coords[i] = j;
                    avgCoords += j;
                    greyBuffer += width;
                }
                avgCoords /= length;
            }


            int count = 0;
            int avgCoords_Smooth = 0;
            int thresDiff = (int)(width * 0.03);
            List<int> XCoords = new List<int>();
            List<int> YCoords = new List<int>();
            for (i = 0; i < length; i++)
            {
                if (Math.Abs(Coords[i] - avgCoords) > thresDiff)
                    continue;
                avgCoords_Smooth += Coords[i];
                count++;

                YCoords.Add(i + beginTop);
                XCoords.Add(Coords[i]);
            }
            avgCoords_Smooth /= count;

            if (EstimateHorizontalLinesByLSQ(XCoords.ToArray(), YCoords.ToArray(),
                ref angle, ref shift) != 0)
                throw new Exception("Estimate Horizontal Lines by LSQ failed");

            return avgCoords_Smooth;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <param name="beginLeft"></param>
        /// <param name="endRight"></param>
        /// <param name="border">image margin, in some case grey value is 255 because of the radius of convolution kernel on previous step.</param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public int ScanIRHorizontalLine(GreyDataImage image, int beginLeft, int endRight, int border, int threshold, ref double angle, ref double shift)
        {
            int width = image._width;
            int height = image._height;
            ushort data = 0;

            int length = endRight - beginLeft + 1;
            int[] Coords = new int[length];
            int avgCoords = 0;

            int i, j;

            unsafe
            {
                ushort* greyBuffer = image._aData;
                greyBuffer += beginLeft;
                for (j = 0; j < length; j++)
                {
                    ushort* greyBufferCol = greyBuffer + border * width;
                    for (i = 0; i < height; i++, greyBufferCol += width)
                    {
                        data = *greyBufferCol;
                        if (data > threshold)
                            break;
                    }
                    Coords[j] = i;
                    avgCoords += i;
                    greyBuffer++;
                }
                avgCoords /= j;
            }


            int count = 0;
            int avgCoords_Smooth = 0;
            int thresDiff = (int)(height * 0.03);
            List<int> XCoords = new List<int>();
            List<int> YCoords = new List<int>();
            for (i = 0; i < length; i++)
            {
                if (Math.Abs(Coords[i] - avgCoords) > thresDiff)
                    continue;
                avgCoords_Smooth += Coords[i];
                count++;

                YCoords.Add(Coords[i]);
                XCoords.Add(i + beginLeft);
            }
            avgCoords_Smooth /= count;

            if (EstimateHorizontalLinesByLSQ(XCoords.ToArray(), YCoords.ToArray(),
                ref angle, ref shift) != 0)
                throw new Exception("Estimate Horizontal Lines by LSQ failed");

            return avgCoords_Smooth;
        }
        #endregion


        #region UI Configuration
        public void DetectDraftABSRegion(GreyDataImage image, RectRegion region)
        {
            if (image == null)
                return;

            double slope = 0, topC = 0, bottomC = 0;
            ScanBoundaryVertical(image, _settings.KeyColumns, ref slope, ref topC, ref bottomC);

            int X_topleft = 0, Y_topleft = 0, X_bottomleft = 0, Y_bottomleft = 0;
            FindCorner(image, topC, bottomC, slope, ref X_topleft, ref Y_topleft, ref X_bottomleft, ref Y_bottomleft);

            int shiftXDraft = X_topleft;
            int shiftYDraft = Y_topleft;

            // update information
            region.X = X_topleft;
            region.Y = Y_topleft;
            double orientation = Math.Atan(-slope);
            region.Orientation = -180.0 * orientation / Math.PI;
        }
        #endregion UI Configuration
    }
}
