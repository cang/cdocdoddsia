#define TEST

using System;
using System.Collections.Generic;
using System.Text;
using SiGlaz.Common.ImageAlignment;
using SIA.IPEngine;
using SDD = System.Drawing.Drawing2D;
using System.Drawing;
using SIA.Algorithms.Preprocessing.Matching;
using SIA.Algorithms.Preprocessing.Interpolation;
using SiGlaz.Common.Pattern;

namespace SIA.Algorithms.Preprocessing.Alignment
{
    public class DepoAligner : AlignerBase
    {
        public DepoAligner(AlignmentSettings settings)
            : base(settings)
        {
        }

        #region Draft

        public unsafe GreyDataImage TestDraft(ref AlignmentResult result, ref SDD.Matrix drafTransform)
        {
            GreyDataImage image = _image;
            DepoAlignmentSettings depoSettings = _settings as DepoAlignmentSettings;

            double[] slope = new double[4];
            double leftC = 0, rightC = 0, topC = 0, bottomC = 0;
            float xTopLeft = 0, yTopLeft = 0, xTopRight = 0, yTopRight = 0, xBottomLeft = 0, yBottomLeft = 0;

            ScanBoundary_OLD(image, _settings.KeyColumns, _settings.KeyRows, ref slope, ref topC, ref bottomC, ref leftC, ref rightC);
            LineIntersect(slope[0], topC, slope[2], leftC, ref xTopLeft, ref yTopLeft);
            LineIntersect(slope[0], topC, slope[3], rightC, ref xTopRight, ref yTopRight);
            LineIntersect(slope[1], bottomC, slope[2], leftC, ref xBottomLeft, ref yBottomLeft);

            drafTransform = new SDD.Matrix
                (
                    new RectangleF(0, 0, _settings.NewWidth, _settings.NewHeight),
                    new System.Drawing.PointF[] 
                        {
                            new System.Drawing.PointF(xTopLeft, yTopLeft), //upper-left in Cartesian
                            new System.Drawing.PointF(xTopRight, yTopRight),
                            new System.Drawing.PointF(xBottomLeft, yBottomLeft)
                        }
                );
            
            int newWidth = (int)Math.Sqrt(Math.Pow(xTopLeft - xTopRight, 2.0) + Math.Pow(yTopLeft - yTopRight, 2.0));
            int newHeight = (int)Math.Sqrt(Math.Pow(xTopLeft - xBottomLeft, 2.0) + Math.Pow(yTopLeft - yBottomLeft, 2.0));

            PointF[] resPoints = new PointF[] 
            { 
                new PointF(xTopLeft, yTopLeft),
                new PointF(xTopRight, yTopRight),
                new PointF(xBottomLeft, yBottomLeft)
            };

            GreyDataImage imgRes = ImageInterpolator.AffineTransform(InterpolationMethod.Bilinear, image, resPoints,
                   newWidth, newHeight, ref drafTransform);

            // Use haar matcher
            int offsetX = 0, offsetY = 0;
            int width = imgRes._width;
            int height = imgRes._height;
            int len = width*height;
            int stride = width;

            ushort* pData = imgRes._aData;
            ushort[] data = new ushort[width * height];
            for (int i = 0; i < len; i++, pData++)
                data[i] = *pData;
            
            double score;
            Rectangle roi = new Rectangle(0, 0, width, height);
            foreach (HaarPattern ptn in depoSettings.HaarPatterns)
            {
                HaarPatternMatcher ptnMatcher = new HaarPatternMatcher(ptn, 100);
                fixed (ushort* pPtnData = ptn.Data)
                {
                    ptnMatcher.Match(imgRes._aData, width, height,
                        pPtnData, ptn.Width, ptn.Height,
                        roi,
                        0.0, out score, out offsetX, out offsetY);
                }

                int[] itrXList = ptnMatcher.InterestedXList;
                int[] itrYList = ptnMatcher.InterestedYList;
            }

            // Use correlation matcher
            
            CorrelationPattern corrPtn = depoSettings.CorrPatterns[0];
            CorrelationMatcher corrMatcher = new CorrelationMatcher(corrPtn);
            
            int roiWidth = corrPtn.Width + 2 * corrPtn.ExpandWidth;
            int roiHeight = corrPtn.Height + 2 * corrPtn.ExpandHeight;
            int halfRoiWidth = roiWidth / 2;
            int halfRoiHeight = roiHeight / 2;

            SDD.Matrix transform = drafTransform.Clone();
            transform.Translate(offsetX - halfRoiWidth, offsetY - halfRoiHeight);
            ushort[] roiData = new ushort[roiWidth * roiHeight];

            fixed (ushort* proiData = roiData)
            {
                Interpolation.ImageInterpolator.AffineTransform(
                    InterpolationMethod.Bilinear,
                    _image._aData, _image._width, _image._height,
                    drafTransform,
                    proiData, roiWidth, roiHeight);
            }

            double corrCoeff = 0;
            int shiftX = 0, shiftY = 0;
            corrCoeff =
                EstimateShiftUsingCoorelationCoef(
                corrPtn.Data, corrPtn.Width, corrPtn.Height,
                corrPtn.Mean, corrPtn.Std * corrPtn.Std,
                roiData, roiWidth, roiHeight, corrPtn.WeightSet, ref shiftX, ref shiftY);


            result = new AlignmentResult(xTopLeft, yTopLeft, slope[0]); // slope is not angle, angle = atan(slope)        
            return imgRes;
        }

        public GreyDataImage DraftAlign(ref AlignmentResult result, ref SDD.Matrix drafTransform)
        {
            double[] slope = new double[4];
            double leftC = 0, rightC = 0, topC = 0, bottomC = 0;
            float xTopLeft = 0, yTopLeft = 0, xTopRight = 0, yTopRight = 0, xBottomLeft = 0, yBottomLeft = 0;

            ScanBoundary_OLD(_image, _settings.KeyColumns, _settings.KeyRows, ref slope, ref topC, ref bottomC, ref leftC, ref rightC);
            LineIntersect(slope[0], topC, slope[2], leftC, ref xTopLeft, ref yTopLeft);
            LineIntersect(slope[0], topC, slope[3], rightC, ref xTopRight, ref yTopRight);
            LineIntersect(slope[1], bottomC, slope[2], leftC, ref xBottomLeft, ref yBottomLeft);

            drafTransform = new SDD.Matrix
                (
                    new RectangleF(0, 0, _settings.NewWidth, _settings.NewHeight),
                    new System.Drawing.PointF[] 
                        {
                            new System.Drawing.PointF(xTopLeft, yTopLeft), //upper-left in Cartesian
                            new System.Drawing.PointF(xTopRight, yTopRight),
                            new System.Drawing.PointF(xBottomLeft, yBottomLeft)
                        }
                );

            int newWidth = (int)Math.Sqrt(Math.Pow(xTopLeft - xTopRight, 2.0) + Math.Pow(yTopLeft - yTopRight, 2.0));
            int newHeight = (int)Math.Sqrt(Math.Pow(xTopLeft - xBottomLeft, 2.0) + Math.Pow(yTopLeft - yBottomLeft, 2.0));

            PointF[] resPoints = new PointF[] 
            { 
                new PointF(xTopLeft, yTopLeft),
                new PointF(xTopRight, yTopRight),
                new PointF(xBottomLeft, yBottomLeft)
            };

            //GreyDataImage imgRes = ImageInterpolator.AffineTransform(InterpolationMethod.Bilinear, _image, resPoints,
            //       newWidth, newHeight, ref drafTransform);
            GreyDataImage imgRes = ImageInterpolator.AffineTransform(InterpolationMethod.Bilinear, _image, resPoints,
                   _settings.NewWidth, _settings.NewHeight, ref drafTransform);

            MyMatrix myMat = new MyMatrix(drafTransform);

            result = new AlignmentResult(0, 0, myMat.GetRotation());

            return imgRes;
        }

        public override void PerformDraftAlignment(ref AlignmentResult result, ref SDD.Matrix drafTransform)
        {
            DraftAlign(ref result, ref drafTransform);
        }
        
        #endregion

        public void DetectPatterns(GreyDataImage imgDraft, Rectangle roi, int iPtn)
        {
            unsafe
            {
                DepoAlignmentSettings depoSettings = _settings as DepoAlignmentSettings;
                
                int width = imgDraft._width;
                int height = imgDraft._height;
                int len = width * height;
                int stride = width;

                ushort* pData = imgDraft._aData;
                ushort[] data = new ushort[width * height];
                for (int i = 0; i < len; i++, pData++)
                    data[i] = *pData;

                double score;
                // Use haar matcher
                int offsetX = 0, offsetY = 0;
                HaarPattern haarPtn = depoSettings.HaarPatterns[iPtn];
                HaarPatternMatcher haarMatcher = new HaarPatternMatcher(haarPtn, 100);
                fixed (ushort* pPtnData = haarPtn.Data)
                {
                    haarMatcher.Match(imgDraft._aData, width, height,
                        pPtnData, haarPtn.Width, haarPtn.Height,
                        roi,
                        0.0, out score, out offsetX, out offsetY);
                }

#if DEBUG
                Console.WriteLine(String.Format("Confidence:{0} X:{1} Y:{2}", score, offsetX, offsetY));
#endif

                int[] bestXList = haarMatcher.InterestedXList;
                int[] bestYList = haarMatcher.InterestedYList;

                // Use correlation matcher
                CorrelationPattern corrPtn = depoSettings.CorrPatterns[iPtn];
                CorrelationMatcher corrMatcher = new CorrelationMatcher(corrPtn);

                int roiWidth = corrPtn.Width + 2 * corrPtn.ExpandWidth;
                int roiHeight = corrPtn.Height + 2 * corrPtn.ExpandHeight;
                int halfRoiWidth = roiWidth / 2;
                int halfRoiHeight = roiHeight / 2;

                SDD.Matrix transform = new SDD.Matrix();
                offsetX += haarPtn.ShiftToCenterX - halfRoiWidth;
                offsetY += haarPtn.ShiftToCenterY - halfRoiHeight;
                transform.Translate(offsetX, offsetY);
                ushort[] roiData = new ushort[roiWidth * roiHeight];

                fixed (ushort* proiData = roiData)
                {
                    Interpolation.ImageInterpolator.AffineTransform(
                        InterpolationMethod.Bilinear,
                        imgDraft._aData, imgDraft._width, imgDraft._height,
                        transform,
                        proiData, roiWidth, roiHeight);
                }

#if DEBUG && TEST
                SIADebugger.SaveImage(roiData, roiWidth, roiHeight,
                    string.Format("D:\\temp\\test\\scan_area_{0}_L{1}_T{2}.bmp", iPtn, roi.Left, roi.Top));
#endif


                double corrCoeff = 0;
                int shiftX = 0, shiftY = 0;
                corrCoeff = EstimateShiftUsingCoorelationCoef(
                                corrPtn.Data, corrPtn.Width, corrPtn.Height,
                                corrPtn.Mean, corrPtn.Std * corrPtn.Std,
                                roiData, roiWidth, roiHeight, 
                                corrPtn.WeightSet, 
                                ref shiftX, ref shiftY);

                offsetX += halfRoiWidth + shiftX;
                offsetY += halfRoiHeight + shiftY;

#if DEBUG && TEST
                ushort[] resData = new ushort[corrPtn.Width * corrPtn.Height];
                transform = new SDD.Matrix();
                transform.Translate(offsetX - corrPtn.Width / 2, offsetY - corrPtn.Height / 2);
                fixed (ushort* pResData = resData, pCorrData = corrPtn.Data)
                {
                    Interpolation.ImageInterpolator.AffineTransform(
                        InterpolationMethod.Bilinear,
                        imgDraft._aData, imgDraft._width, imgDraft._height,
                        transform,
                        pResData, corrPtn.Width, corrPtn.Height);

                    GreyDataImage img1 = new GreyDataImage(corrPtn.Width, corrPtn.Height, pResData);
                    GreyDataImage img2 = new GreyDataImage(corrPtn.Width, corrPtn.Height, pCorrData);
                    GreyDataImage imgSub = Filtering.Filter.Subtraction(img2, img1, true, new Rectangle(0, 0, corrPtn.Width, corrPtn.Height));
                    //imgSub.SaveToBitmap(string.Format("D:\\temp\\test\\subtraction_{0}.bmp", iPtn));
                    img2.SaveToBitmap(string.Format("D:\\temp\\test\\wscorr_{0}.bmp", iPtn));
                }
                SIADebugger.SaveImage(resData, corrPtn.Width, corrPtn.Height,
                    string.Format("D:\\temp\\test\\result_{2}_L{0}_T{1}.bmp", roi.Left, roi.Top, iPtn));
                
#endif

#if DEBUG
                Console.WriteLine(String.Format("Coor:{0} X:{1} Y:{2}", corrCoeff, offsetX, offsetY));
#endif
            }
        }

        public override void DetectDraftRegion(GreyDataImage image, RectRegion region)
        {
        }
    }
}

