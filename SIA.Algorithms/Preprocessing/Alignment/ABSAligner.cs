#define TEST____

using System;
using System.Collections.Generic;
using System.Text;
using SiGlaz.Common.ImageAlignment;
using SIA.IPEngine;
using SDD = System.Drawing.Drawing2D;
using System.Drawing;
using SIA.Algorithms.Preprocessing.Matching;
using SIA.Algorithms.Preprocessing.Interpolation;

namespace SIA.Algorithms.Preprocessing.Alignment
{
    public class ABSAligner : AlignerBase
    {
        private ABSAlignmentSettings settings_ABS;
        public ABSAligner(AlignmentSettings settings)
            : base(settings)
        {
            settings_ABS = settings as ABSAlignmentSettings;
        }

        #region Draft
        public override void PerformDraftAlignment(
            ref AlignmentResult result, ref SDD.Matrix drafTransform)
        {
            GreyDataImage image = _image;

            double slope = 0, topC = 0, bottomC = 0;
            ScanBoundaryVertical(
                image, _settings.KeyColumns, 
                ref slope, ref topC, ref bottomC);

            int X_topleft = 0, Y_topleft = 0, X_bottomleft = 0, Y_bottomleft = 0;
            FindCorner(
                image, topC, bottomC, slope, 
                ref X_topleft, ref Y_topleft, 
                ref X_bottomleft, ref Y_bottomleft);

            int shiftXDraft = X_topleft;
            int shiftYDraft = Y_topleft;

            result = new AlignmentResult(0, 0, slope); // slope is not angle, angle = atan(slope)            

            
            double horizontalSlope = slope;
            float preferredWidth = _settings.NewWidth;
            float preferredDeltaX = (float)(preferredWidth / Math.Sqrt(1 + horizontalSlope * horizontalSlope));

            double verticalSlope = -slope;
            float preferredHeight = _settings.NewHeight;
            float preferredDeltaY = (float)(preferredHeight / Math.Sqrt(1 + horizontalSlope * horizontalSlope));

            drafTransform = new SDD.Matrix(
                new RectangleF(0, 0, _settings.NewWidth, _settings.NewHeight),
            new System.Drawing.PointF[] {
                new System.Drawing.PointF(X_topleft, Y_topleft), //upper-left in Cartesian

                new System.Drawing.PointF( // top - right
                    X_topleft + preferredDeltaX, 
                    (float)(Y_topleft + preferredDeltaX * horizontalSlope)),

                new System.Drawing.PointF( // bottom - left
                    (float)(X_topleft + preferredDeltaY * verticalSlope), 
                    Y_topleft + preferredDeltaY) 
            });
        }

        protected virtual unsafe void ScanBoundaryVertical(
            GreyDataImage image, int[] colsIdx, 
            ref double rotationAngle, ref double topC, ref double bottomC)
        {
            if (colsIdx == null)
                return;

            int width = image._width;
            int height = image._height;

            int length = colsIdx.Length;
            List<int> yCoordsTop = new List<int>();
            List<int> yCoordsBottom = new List<int>();
            ushort data = 0;

            ushort thresGrey = (ushort)settings_ABS.IntensityThreshold;
            ushort thresGrey2 = (ushort)settings_ABS.IntensityThreshold_High;

            ushort* greyBuffer = image._aData;

            int indexPixel = 0;
            for (int j = 0; j < length; j++)
            {
                indexPixel = 0;
                yCoordsTop.Add(0);
                for (int i = 0; i < height; i++)
                {
                    data = greyBuffer[indexPixel + colsIdx[j]];
                    if (data > thresGrey && data <thresGrey2)
                    {
                        yCoordsTop[j] = i;
                        break;
                    }
                    indexPixel += width;
                }
                
                indexPixel = (height - 1) * width;
                yCoordsBottom.Add(0);
                for (int i = height - 1; i >= 0; i--)
                {
                    data = greyBuffer[indexPixel + colsIdx[j]];
                    if (data > thresGrey && data < thresGrey2)
                    {
                        yCoordsBottom[j] = i;
                        break;
                    }
                    indexPixel -= width;
                }
            }

            List<int> colsIdxTop = new List<int>(colsIdx);
            List<int> colsIdxBottom = new List<int>(colsIdx);

            if (EstimateHorizontalLinesByLSQ(colsIdxTop.ToArray(), yCoordsTop.ToArray(), colsIdxBottom.ToArray(), yCoordsBottom.ToArray(),
                ref rotationAngle, ref topC, ref bottomC) != 0)
                throw new Exception("Estimate Horizontal Lines by LSQ failed");

            for (int i = length - 1; i >= 0; i--)
            {
                double distanceTop = Math.Abs(DistancePointToLine(colsIdxTop[i], yCoordsTop[i], rotationAngle, topC));
                double distanceBottom = Math.Abs(DistancePointToLine(colsIdxBottom[i], yCoordsBottom[i], rotationAngle, bottomC));
                distanceTop /= topC;
                distanceBottom /= bottomC;


                if (distanceTop > settings_ABS.LQSDistancePercentThreshold)
                {
                    colsIdxTop.RemoveAt(i);
                    yCoordsTop.RemoveAt(i);
                }
                if (distanceBottom > settings_ABS.LQSDistancePercentThreshold)
                {
                    colsIdxBottom.RemoveAt(i);
                    yCoordsBottom.RemoveAt(i);
                }
            }

            if (EstimateHorizontalLinesByLSQ(colsIdxTop.ToArray(), yCoordsTop.ToArray(), colsIdxBottom.ToArray(), yCoordsBottom.ToArray(),
                ref rotationAngle, ref topC, ref bottomC) != 0)
                throw new Exception("Estimate Horizontal Lines by LSQ failed");
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

        private unsafe void FindCorner(
            GreyDataImage image, double topC, 
            double bottomC, double rotationAngle, 
            ref int X_topleft, ref int Y_topleft, 
            ref int X_bottomleft, ref int Y_bottomleft)
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
        #endregion Draft

        #region Fine tune step
        private int _featureCount = 137;
        private int _pivotCount = 51;

        protected override void ResetPrivateParamsAfterPruning()
        {
            _pivotCount = 31;
        }

        protected override void MatchKeyPoint(
            int isample, ushort[] roiData, double[] weightSet, 
            System.Drawing.Drawing2D.Matrix transform, 
            ref double confidence, ref int shiftX, ref int shiftY)
        {
            // default for ABS: weightSet = null
            double[] absWeightSet = null;

            bool applyDraft = true;
            if (applyDraft)
            {
                int featureCount = _featureCount;
                int pivotCount = _pivotCount;
                GradientMapMatching(
                    isample, roiData, absWeightSet,
                    transform,
                    featureCount, pivotCount,
                    ref confidence, ref shiftX, ref shiftY);
            }
            else
            {
                base.MatchKeyPoint(
                    isample, roiData, absWeightSet,
                    transform,
                    ref confidence, ref shiftX, ref shiftY);
            }
        }

        protected override SDD.Matrix FineTune(GreyDataImage image, SDD.Matrix draft)
        {
            return AlignUsingMultipeKeypointsParallely(image, draft);
        }
        #endregion Fine tune step

        public override void DetectDraftRegion(GreyDataImage image, RectRegion region)
        {
            if (image == null)
                return;

            double slope = 0, topC = 0, bottomC = 0;
            ScanBoundaryVertical(
                image, _settings.KeyColumns, ref slope, ref topC, ref bottomC);

            int X_topleft = 0, Y_topleft = 0, X_bottomleft = 0, Y_bottomleft = 0;
            FindCorner(
                image, topC, bottomC, slope, 
                ref X_topleft, ref Y_topleft, 
                ref X_bottomleft, ref Y_bottomleft);

            int shiftXDraft = X_topleft;
            int shiftYDraft = Y_topleft;

            // update information
            region.X = X_topleft;
            region.Y = Y_topleft;
            region.ExpandedRight = 0;
            double orientation = Math.Atan(-slope);
            region.Orientation = -180.0 * orientation / Math.PI;
        }
    }
}
