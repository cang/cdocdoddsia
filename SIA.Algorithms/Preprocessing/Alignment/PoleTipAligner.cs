#define TEST__

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
    public class PoleTipAligner : AlignerBase
    {
        public PoleTipAligner(AlignmentSettings settings)
            : base(settings)
        {
        }

        private double _threshold = 45;
        private double _snap_distance = 60;

        #region Draft
        public override void PerformDraftAlignment(
            ref AlignmentResult result, ref SDD.Matrix drafTransform)
        {
            GreyDataImage image = _image;

            double rotationAngle = 0.0, shiftX = 0.0, shiftY = 0;
            double anchorX = 0.0, anchorY = 0.0;
            ScanWhiteRegion_PoleTip(image, _settings, ref anchorX, ref anchorY);

            //double threshold = _threshold;
            //double snap_distance = _snap_distance;
            //double vertical_slope = 0, c = 0;
            //EstimateVerticalLine(image, threshold,
            //    (int)(anchorX - 3 * snap_distance), 35, 
            //    (int)(anchorX - snap_distance), image.Height - 1,
            //    ref vertical_slope, ref c);

            //double slope = 0;
            //if (vertical_slope != 0)
            //    slope = - 1.0 / vertical_slope;
            
            double left = anchorX - (_settings.NewWidth - (_settings as PoleTipAlignmentSettings).ExpandedRight);
            double top = anchorY - _settings.NewHeight;
            top = (image.Height - _settings.NewHeight) * 0.5;

            double right = left + _settings.NewWidth;
            double bottom = top + _settings.NewHeight;

            result = new AlignmentResult(left, top, rotationAngle);
            
            drafTransform = new SDD.Matrix();
            drafTransform.Translate((float)left, (float)top);
        }

        protected virtual void FindLeftTopCorner(
            double orientation, 
            double x, double y, 
            double w, double h, ref double left, ref double top)
        {
            top = y - h;
            left = x - w;

            using (SDD.Matrix m = new SDD.Matrix())
            {
                m.RotateAt((float)orientation, new PointF((float)x, (float)y));
                PointF[] pts = new PointF[] { new PointF((float)left, (float)top) };
                m.TransformPoints(pts);
                left = pts[0].X;
                top = pts[0].Y;
            }
        }

        protected virtual unsafe void ScanWhiteRegion_PoleTip(
            GreyDataImage image, AlignmentSettings settings, ref double anchorX, ref double anchorY)
        {
            ScanWhiteRegion_PoleTip(
                image, 
                settings.TopScanWhite, settings.BottomScanWhite, 
                settings.LeftScanWhite, settings.RightScanWhite, 
                ref anchorX, ref anchorY);
        }

        protected virtual unsafe void ScanWhiteRegion_PoleTip(
            GreyDataImage image,
            int topScan, int bottomScan, int leftScan, int rightScan,
            ref double anchorX, ref double anchorY)
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

        protected virtual unsafe void EstimateVerticalLine(
            GreyDataImage image, double threshold, 
            int left, int top, int right, int bottom,
            ref double slope, ref double c)
        {
            int nKeyPoints = 200;
            int step = (bottom - top + 1) / nKeyPoints;
            nKeyPoints--;

            int w = image.Width;
            int h = image.Height;

            List<int> yPoints = new List<int>(nKeyPoints);
            List<int> xPoints = new List<int>(nKeyPoints);


            double minDiff = 0; // 20

            ushort* data = image._aData;
            int index = 0;
            for (int iKeyPoint = 0, y = top; 
                y <= bottom && y < h && iKeyPoint < nKeyPoints; y += step, iKeyPoint++)
            {
                index = y * w + right;
                for (int x = right; x >= left; x--, index--)
                {
                    if (data[index] > threshold &&
                        data[index - 1] - data[index] > minDiff)
                    {
                        yPoints.Add(image.Height/2 - y);
                        xPoints.Add(x);
                        break;
                    }
                }
            }

#if DEBUG
            SIADebugger.SaveDetectedLineAndImage(
                image, xPoints.ToArray(), yPoints.ToArray(),
                -1, -1,
                @"D:\temp\test\right_lines.bmp");
#endif

            slope = 0;
            c = 0;
            EstimateLineByLSQ(xPoints.ToArray(), yPoints.ToArray(), ref slope, ref c);

#if DEBUG
            SIADebugger.SaveDetectedLineAndImage(
                image, xPoints.ToArray(), yPoints.ToArray(),
                slope, c,
                @"D:\temp\test\right_lines2.bmp");
#endif
        }
        #endregion Draft

        #region Fine tune step
        protected override void MatchKeyPoint(
            int isample, ushort[] roiData, double[] weightSet,
            System.Drawing.Drawing2D.Matrix transform,
            ref double confidence, ref int shiftX, ref int shiftY)
        {
            // default for ABS: weightSet = null
            double[] absWeightSet = weightSet;

            bool useMatcher = true;
            if (useMatcher)
            {

                //GoldenImageMatcher matcher = new GoldenImageMatcher(5, 5);

                //base.MatchKeyPoint(
                //   isample, roiData, absWeightSet,
                //   transform, matcher,
                //   ref confidence, ref shiftX, ref shiftY);

                //matcher = null;

                int featureCount = 131;
                int pivotCount = 71;

                featureCount = 271;//571;
                pivotCount = 71;

                GoldenImageMatching(
                    isample, roiData, absWeightSet,
                    transform,
                    featureCount, pivotCount,
                    ref confidence, ref shiftX, ref shiftY);

                return;
            }


            bool applyDraft = true;
            //applyDraft = false;
            if (applyDraft)
            {
                int featureCount = 131;
                int pivotCount = 71;

                featureCount = 5571;
                pivotCount = 571;

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

        protected virtual void GoldenImageMatching(
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

            int kernelWidth = 3;
            int kernelHeight = 3;
            GoldenImageMatcher matcher = new GoldenImageMatcher(
                //kernelWidth, kernelHeight, ratingProcessor.InterestedXList, ratingProcessor.InterestedYList);
                kernelWidth, kernelHeight, null, null);
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
                ratingProcessor.PivotIndices,
                bestMatchs,
                string.Format("D:\\temp\\test\\gradientmap_sample_pattern_{0}__{1}.bmp", isample, confidence.ToString("0.00000")));

            confidence = 1.0;
#endif

            // set null here, hope that GC will collect successfully
            roiData = null;
        }
        #endregion Fine tune step

        public override void DetectDraftRegion(GreyDataImage image, RectRegion region)
        {
            if (image == null)
                return;

            double rotationAngle = 0.0, shiftX = 0.0, shiftY = 0;
            double anchorX = 0.0, anchorY = 0.0;
            ScanWhiteRegion_PoleTip(image, _settings, ref anchorX, ref anchorY);

            //double threshold = _threshold;
            //double snap_distance = _snap_distance;
            //double vertical_slope = 0, c = 0;
            //EstimateVerticalLine(image, threshold,
            //    (int)(anchorX - 3 * snap_distance), 35,
            //    (int)(anchorX - snap_distance), image.Height - 1,
            //    ref vertical_slope, ref c);

            //// horizontal line slope
            //double slope = -1 / vertical_slope;

            //double left = 0, top = 0;
            //double y = image.Height * 0.5;
            //double x = (y - c) / vertical_slope;
            
            //double vertical_orientation = Math.Atan(-vertical_slope);
            //vertical_orientation = -180.0 * vertical_orientation / Math.PI;

            //FindLeftTopCorner(
            //    vertical_orientation, 
            //    x, y, 
            //    _settings.NewWidth , _settings.NewHeight * 0.5, 
            //    ref left, ref top);


            double left = anchorX - (_settings.NewWidth - (_settings as PoleTipAlignmentSettings).ExpandedRight);
            double top = anchorY - _settings.NewHeight;
            top = (image.Height - _settings.NewHeight) * 0.5;
            double slope = rotationAngle;




            // update information
            region.X = left;
            region.Y = top;
            region.ExpandedRight = (_settings as PoleTipAlignmentSettings).ExpandedRight; //0;
            double orientation = Math.Atan(-slope); // default 0
            region.Orientation = -180.0 * orientation / Math.PI;
        }
    }
}
