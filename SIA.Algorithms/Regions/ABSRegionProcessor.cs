using System;
using System.Collections.Generic;
using System.Text;
using SiGlaz.Common.ImageAlignment;
using SIA.Algorithms.Preprocessing.Alignment;
using SIA.IPEngine;
using System.Drawing;
using SiGlaz.Common;

namespace SIA.Algorithms.Regions
{
    public class ABSRegionProcessor
    {
        public static SiGlaz.Common.GraphicsList CorrectRegions(
            MetrologySystemReference refFile, 
            MetrologySystem detectedSystem, AlignmentResult csAlignmentResult, 
            GreyDataImage greyImage)
        {
            if (refFile.Regions == null)
                return null;

            GraphicsList regions = GraphicsList.FromBytes(refFile.Regions);
            ABSAlignmentSettings ras = new ABSAlignmentSettings();
            ras.SampleExpandWidth = 15;
            ras.SampleExpandHeight = 15;
            ras.SampleWidth = 71; //(int)(metrologySystem.Markers[0] as MarkerRegion).DrawingWidth;
            ras.SampleHeight = 71; //(int)(metrologySystem.Markers[0] as MarkerRegion).DrawingHeight;
            int length = ras.SampleWidth * ras.SampleHeight;

            List<AlignmentResult> regAlignmentResults = new List<AlignmentResult>(regions.Count);
            for (int iRegion = 0; iRegion < regions.Count; iRegion++)
            {
                if (regions[iRegion].GetDrawingType() == "Polygon")
                {
                    SiGlaz.Common.DrawPolygon polygon = (SiGlaz.Common.DrawPolygon)regions[iRegion];

                    List<MetaVertex> allAutoVertex = new List<MetaVertex>();

                    foreach (MetaVertex m in polygon.pointArray)
                        if (m.IsAutoVertex)
                            allAutoVertex.Add(m);

                    ras.SampleCount = allAutoVertex.Count;
                    ras.SampleData = new ushort[ras.SampleCount][];
                    ras.SampleXCoordinates = new double[ras.SampleCount];
                    ras.SampleYCoordinates = new double[ras.SampleCount];
                    for (int iAutoVertex = 0; iAutoVertex < ras.SampleCount; iAutoVertex++)
                    {
                        MetaVertex m = allAutoVertex[iAutoVertex];
                        ras.SampleData[iAutoVertex] = m.SampleData;
                        ras.SampleXCoordinates[iAutoVertex] = m.ptSample.X;
                        ras.SampleYCoordinates[iAutoVertex] = m.ptSample.Y;
                    }

                    ras.SampleWeightSet = new double[ras.SampleCount][];
                    ras.PrepareSetting();

                    ABSAligner regionAligner = (ABSAligner)AlignerFactory.CreateInstance(ras);
                    AlignmentResult regionAlignResult = null; //regionAligner.Align(image.RasterImage);
                    // add to global
                    regAlignmentResults.Add(regionAlignResult);

                    double[] xList = null;
                    double[] yList = null;
                    double[] confidences = null;
                    regionAligner.DetectKeyPoints(greyImage, csAlignmentResult, ref regionAlignResult,
                        ref xList, ref yList, ref confidences);
                    System.Drawing.Drawing2D.Matrix alignMatrix = regionAlignResult.GetDeviceToImageTransformer();

                    PointF[] pts = new PointF[polygon.pointArray.Count];
                    for (int iVertex = 0; iVertex < polygon.pointArray.Count; iVertex++)
                    {
                        MetaVertex m = polygon.pointArray[iVertex];
                        pts[iVertex].X = m.ptSample.X;
                        pts[iVertex].Y = m.ptSample.Y;
                    }

                    alignMatrix.TransformPoints(pts); // golden device to image
                    detectedSystem.Transformer.TransformPoints(pts); // image to real

                    for (int iVertex = 0; iVertex < polygon.pointArray.Count; iVertex++)
                    {
                        MetaVertex m = polygon.pointArray[iVertex];
                        m.pt.X = pts[iVertex].X;
                        m.pt.Y = pts[iVertex].Y;
                    }
                }
            }

            return regions;
        }
    }
}
