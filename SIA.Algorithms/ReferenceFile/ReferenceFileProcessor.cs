#define TEST____

using System;
using System.Collections.Generic;
using System.Text;
using SIA.IPEngine;
using SIA.Algorithms.Preprocessing.Interpolation;
using System.Drawing;
using System.IO;
using SiGlaz.Common.ImageAlignment;
using SIA.Algorithms.Preprocessing.Alignment;

namespace SIA.Algorithms.ReferenceFile
{
    public abstract class ReferenceFileProcessor
    {
        public static void GetSampleData(
            GreyDataImage greyImage,
            float deviceLeft, float deviceTop, float deviceOrientation,
            float x, float y, int w, int h, ushort[] roi, ref float ox, ref float oy)
        {
            //x += 0.5f;
            //y += 0.5f;
            PointF[] pts = new PointF[] { new PointF(x, y) };

            using (GreyDataImage tmpImage = new GreyDataImage(w, h))
            {
                unsafe
                {
                    using (System.Drawing.Drawing2D.Matrix m = new System.Drawing.Drawing2D.Matrix())
                    {
                        m.Translate(deviceLeft, deviceTop);
                        m.Rotate(-deviceOrientation);

//#if DEBUG && TEST
//#if DEBUG && TEST
//                        using (GreyDataImage roieeee = new GreyDataImage(
//                            2237, 1380))
//                        {
//                            ImageInterpolator.AffineTransform(
//                                            InterpolationMethod.Bilinear,
//                                            greyImage, m, roieeee);
//                            roieeee.SaveImage(
//                                string.Format("D:\\temp\\test\\sample_{0}.bmp", "test_finetune44444"),
//                                SIA.Common.eImageFormat.Bmp);
//                        }
//#endif
//#endif



                        m.Invert();

                        m.TransformPoints(pts);
                        ox = pts[0].X;
                        oy = pts[0].Y;

#if DEBUG && TEST


                        System.Diagnostics.Trace.WriteLine(
                            string.Format(
                            "ox: {0:0.######} ---  oy: {1:0.######} ========", ox, oy));
#endif

                        // update left-top of marker in ABS device coordinate
                        pts[0].X = ox - w * 0.5f;
                        pts[0].Y = oy - h * 0.5f;
                        
                        m.Invert();

                        // transform to image - coordinate
                        m.TransformPoints(pts);

                        m.Reset();

                        m.Translate(pts[0].X, pts[0].Y);
                        m.Rotate(-deviceOrientation);

                        // to here: 

                        ImageInterpolator.AffineTransform(
                            InterpolationMethod.Bilinear, greyImage, m, tmpImage);

                        Utilities.Copy(tmpImage._aData, roi);
                    }
                }

#if DEBUG && TEST
                string folderPath = @"D:\temp\Xyratex\Samples\";
                string[] files = Directory.GetFiles(folderPath);
                int id = 0;
                if (files != null && files.Length > 0)
                    id = files.Length;
                string fileName = Path.Combine(folderPath, string.Format("{0}.bmp", id));
                tmpImage.SaveImage(fileName, SIA.Common.eImageFormat.Bmp);
#endif
            }
        }

        public static CoordinateSystem GetCoordinateSystem(
            MetrologySystemReference refFile, AlignmentResult alignResult)
        {
            CoordinateSystem cs = new CoordinateSystem();

            float newWidth = refFile.AlignmentSettings.NewWidth;
            float newHeight = refFile.AlignmentSettings.NewHeight;
            float newAngle = alignResult.GetRotateAngle(newWidth, newHeight);
            float newLeft = alignResult.GetLeft(newWidth, newHeight);
            float newTop = alignResult.GetTop(newWidth, newHeight);

            PointF[] pts = new PointF[] { PointF.Empty };
            pts[0] = refFile.TransformToLTDeviceCoordinate(
                refFile.MetrologySystem.CurrentCoordinateSystem.GetOriginPointF());

            // transform to abs-left-top coordinate
            pts[0] =
                MetrologySystemReference.TransformToImageCoordinate(pts[0], newLeft, newTop, newAngle);

            // update coordinate system
            cs.DrawingOriginX = pts[0].X;
            cs.DrawingOriginY = pts[0].Y;
            cs.DrawingAngle =
                newAngle -
                (refFile.DeviceOrientation - refFile.MetrologySystem.CurrentCoordinateSystem.DrawingAngle);

            return cs;
        }

        public static List<MarkerPoint> GetMarkers(
            MetrologySystemReference refFile, AlignmentResult alignResult)
        {
            float newWidth = refFile.AlignmentSettings.NewWidth;
            float newHeight = refFile.AlignmentSettings.NewHeight;
            float newAngle = alignResult.GetRotateAngle(newWidth, newHeight);
            float newLeft = alignResult.GetLeft(newWidth, newHeight);
            float newTop = alignResult.GetTop(newWidth, newHeight);

            PointF[] pts = new PointF[] { PointF.Empty };

            int sampleCount = refFile.AlignmentSettings.SampleCount;
            List<MarkerPoint> detectedMarkers = new List<MarkerPoint>(sampleCount);
            using (System.Drawing.Drawing2D.Matrix m = new System.Drawing.Drawing2D.Matrix())
            {
                m.Translate(newLeft, newTop);
                m.Rotate(-newAngle);

                for (int iSample = 0; iSample < sampleCount; iSample++)
                {
                    pts[0].X = (float)refFile.AlignmentSettings.SampleXCoordinates[iSample];
                    pts[0].Y = (float)refFile.AlignmentSettings.SampleYCoordinates[iSample];

                    m.TransformPoints(pts);

                    MarkerRegion marker = new MarkerRegion();
                    marker.DrawingX = pts[0].X;
                    marker.DrawingY = pts[0].Y;

                    detectedMarkers.Add(marker);
                }
            }

            return detectedMarkers;
        }

        public static MetrologySystem GetMetrologySystem(
            MetrologySystemReference refFile, AlignmentResult alignResult)
        {
            MetrologySystem ms = new MetrologySystem();

            ms.CurrentUnit.CopyFrom(refFile.MetrologySystem.CurrentUnit);
            ms.CurrentCoordinateSystem.CopyFrom(GetCoordinateSystem(refFile, alignResult));
            ms.Markers.AddRange(GetMarkers(refFile, alignResult));

            return ms;
        }

        public static GreyDataImage GetImage(MetrologySystemReference refFile)
        {
            return GreyImageWrapper.FromRawBytes(refFile.ReferenceImage);
        }
    }    
}
