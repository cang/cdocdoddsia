using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using SiGlaz.Common;
using SiGlaz.Common.ImageAlignment;
using SIA.IPEngine;
using SIA.Algorithms.Preprocessing.Alignment;
using SIA.SystemFrameworks;

namespace SIA.Algorithms.ReferenceFile
{
    public class ABSReferenceFileProcessor : ReferenceFileProcessor
    {
        public static ABSMetrologySystemReference
            CreateReferenceFile(GreyDataImage greyImage, MetrologySystem metrologySystem, GraphicsList regions)
        {
            ABSMetrologySystemReference referenceFile = null;

            try
            {
                #region Alignment Settings
                ABSAlignmentSettings alignmentSettings = new ABSAlignmentSettings();

                ABSAligner aligner = new ABSAligner(alignmentSettings);
                RectRegion absRegion = new RectRegion();
                aligner.DetectDraftRegion(greyImage, absRegion);

                float deviceLeft = (float)absRegion.X;
                float deviceTop = (float)absRegion.Y;
                float deviceOrientation = -(float)absRegion.Orientation;

                ABSAlignmentSettings absAlignmentSettings = new ABSAlignmentSettings();
                absAlignmentSettings.SampleCount = metrologySystem.Markers.Count;
                if (absAlignmentSettings.SampleCount > 0)
                {
                    absAlignmentSettings.SampleWidth = 71; //(int)(metrologySystem.Markers[0] as MarkerRegion).DrawingWidth;
                    absAlignmentSettings.SampleHeight = 71; //(int)(metrologySystem.Markers[0] as MarkerRegion).DrawingHeight;
                    int length = absAlignmentSettings.SampleWidth * absAlignmentSettings.SampleHeight;
                    absAlignmentSettings.SampleData = new ushort[absAlignmentSettings.SampleCount][];
                    absAlignmentSettings.SampleXCoordinates = new double[absAlignmentSettings.SampleCount];
                    absAlignmentSettings.SampleYCoordinates = new double[absAlignmentSettings.SampleCount];
                    for (int i = 0; i < absAlignmentSettings.SampleCount; i++)
                    {
                        absAlignmentSettings.SampleData[i] = new ushort[length];
                    }

                    //Parallel.For(0, absAlignmentSettings.SampleCount, delegate(int iSample)
                    for (int iSample = 0; iSample < absAlignmentSettings.SampleCount; iSample++)
                    {
                        float ox = 0, oy = 0;
                        GetSampleData(
                            greyImage,
                            deviceLeft, deviceTop, deviceOrientation,
                            metrologySystem.Markers[iSample].DrawingX,
                            metrologySystem.Markers[iSample].DrawingY,
                            absAlignmentSettings.SampleWidth, absAlignmentSettings.SampleHeight,
                            absAlignmentSettings.SampleData[iSample], ref ox, ref oy);
                        //absAlignmentSettings.SampleXCoordinates[iSample] = (int)Math.Round(ox);
                        //absAlignmentSettings.SampleYCoordinates[iSample] = (int)Math.Round(oy);
                        absAlignmentSettings.SampleXCoordinates[iSample] = ox;
                        absAlignmentSettings.SampleYCoordinates[iSample] = oy;
                    }
                    //);

                    absAlignmentSettings.SampleWeightSet = new double[absAlignmentSettings.SampleCount][];
                }
                absAlignmentSettings.PrepareSetting();
                #endregion Alignment Settings

                #region Reference Image
                byte[] refImage = GreyImageWrapper.ToRawBytes(greyImage);
                #endregion Reference Image

                #region Regions
                byte[] refRegions = null;
                if (regions != null)
                {
                    for (int iRegion = 0; iRegion < regions.Count; iRegion++)
                    {
                        if (regions[iRegion].GetDrawingType() == "Polygon")
                        {
                            SiGlaz.Common.DrawPolygon polygon = (SiGlaz.Common.DrawPolygon)regions[iRegion];
                            foreach (MetaVertex m in polygon.pointArray)
                            {
                                PointF ptImage = metrologySystem.ToPixel(m.pt);
                                if (m.IsAutoVertex)
                                {
                                    m.sizeSample.Width = absAlignmentSettings.SampleWidth;
                                    m.sizeSample.Height = absAlignmentSettings.SampleHeight;
                                    m.SampleData = new ushort[m.sizeSample.Width * m.sizeSample.Height];
                                    float ox = 0, oy = 0;
                                    GetSampleData(
                                        greyImage,
                                        deviceLeft, deviceTop, deviceOrientation,
                                        ptImage.X, ptImage.Y,
                                        absAlignmentSettings.SampleWidth, absAlignmentSettings.SampleHeight,
                                        m.SampleData, ref ox, ref oy);
                                    m.ptSample.X = ox;
                                    m.ptSample.Y = oy;
                                }
                                else
                                {
                                    using (System.Drawing.Drawing2D.Matrix mat = new System.Drawing.Drawing2D.Matrix())
                                    {
                                        mat.Translate(deviceLeft, deviceTop);
                                        mat.Rotate(-deviceOrientation);
                                        mat.Invert();
                                        PointF[] pts = new PointF[] { ptImage };
                                        mat.TransformPoints(pts);
                                        m.ptSample.X = pts[0].X;
                                        m.ptSample.Y = pts[0].Y;
                                    }
                                }
                            }
                        }
                    }

                    refRegions = regions.ToBytes();
                }
                #endregion Regions

                // create instance
                referenceFile =
                    new ABSMetrologySystemReference(
                    deviceLeft, deviceTop, deviceOrientation,
                    metrologySystem, absAlignmentSettings, refImage, refRegions);
            }
            catch (System.Exception exp)
            {
                referenceFile = null;
                throw exp;
            }
            finally
            {
            }

            return referenceFile;
        }
    }
}
