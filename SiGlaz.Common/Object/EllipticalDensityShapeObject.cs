using System;
using System.Collections.Generic;
using System.Text;
using SIA.Common.Analysis;
using SiGlaz.Algorithms.Core;
using System.Collections;
using System.Drawing;
using SiGlaz.Common.ImageAlignment;

namespace SiGlaz.Common.Object
{
    public class EllipticalDensityShapeObject : BasePoint
    {
        public DetectedObject DetectedObject = null;

        public double CenterX = 0;
        public double CenterY = 0;

        public double Eccentricity = 0;
        public double MajorLength = 0;
        public double MinorLength = 0;

        public double MajorAxisOrientation = 0;

        public double AngleToCenter = 0; // degree

        public double Elongation = 0;

        public double AverageIntensity = 0;

        public void UpdateSpatialFeatures()
        {
            double[] xpoints = null, ypoints = null;
            if (DetectedObject is DetectedObjectEx)
                GetPoints(DetectedObject as DetectedObjectEx, ref xpoints, ref ypoints);
            else
                GetPoints(DetectedObject, ref xpoints, ref ypoints);

            MatrixUtils.ComputeEllipseParam(ref xpoints, ref ypoints, ref CenterX, ref CenterY,
                ref MajorLength, ref MinorLength, ref Eccentricity, ref MajorAxisOrientation);

            if (MinorLength < 1)
                MinorLength = 1; // small value
            if (MajorLength < 1)
                MajorLength = 1; // small value

            AngleToCenter = Math.Atan2(CenterY, CenterX) * 180 / Math.PI;

            if (MinorLength == 0)
                Elongation = MajorLength;
            else
                Elongation = MajorLength / MinorLength;

            AverageIntensity = 
                DetectedObject.TotalIntensity / DetectedObject.NumPixels;
        }

        protected virtual void GetPoints(
            DetectedObject detectedObject, 
            ref double[] xPoints, ref double[] yPoints)
        {
            PolygonExData polyExData = detectedObject.PolygonBoundary;
            SimplePolygonProcessor poly = 
                new SimplePolygonProcessor(
                polyExData.nContours, polyExData.nPoints, true);
            
            poly.UpdatePolygonExData(polyExData);

            poly.Intialize();

            ArrayList points = poly.GetObjectExtData(polyExData.ExtPoints);

            Point pt;
            int nPoints = points.Count;
            xPoints = new double[nPoints];
            yPoints = new double[nPoints];
            for (int i = 0; i < nPoints; i++)
            {
                pt = (Point)points[i];
                xPoints[i] = pt.X;
                yPoints[i] = pt.Y;
            }
        }

        protected virtual void GetPoints(
            DetectedObjectEx detectedObject,
            ref double[] xPoints, ref double[] yPoints)
        {
            List<double> xList = new List<double>();
            List<double> yList = new List<double>();

            foreach (DetectedObject primitiveObject in detectedObject.PrimitiveObjects)
            {
                double[] localXPoints = null;
                double[] localYPoints = null;
                GetPoints(primitiveObject, ref localXPoints, ref localYPoints);
                if (localXPoints != null && localYPoints != null)
                {
                    xList.AddRange(localXPoints);
                    yList.AddRange(localYPoints);
                }
            }

            xPoints = xList.ToArray();
            xList.Clear();
            xList = null;

            yPoints = yList.ToArray();
            yList.Clear();
            yList = null;
        }

        private EllipticalDensityShapeObject()
        {
        }

        private EllipticalDensityShapeObject(
            DetectedObject detectedObject)
            : base()
        {
            DetectedObject = detectedObject;
        }

        public virtual void UpdateMetrologyInfo(MetrologySystem ms)
        {
            if (ms != null)
            {
                PointF[] pts = new PointF[] { new PointF((float)CenterX, (float)CenterY) };
                ms.ToRealCoordinate(ref pts);
                CenterX = pts[0].X;
                CenterY = pts[0].Y;

                MajorLength = MajorLength * ms.CurrentUnit.UnitVal / ms.CurrentUnit.PixelVal;
                MinorLength = MinorLength * ms.CurrentUnit.UnitVal / ms.CurrentUnit.PixelVal;
            }
        }

        //public static EllipticalDensityShapeObject 
        //    FromDetectedObject(DetectedObject detectedObject)
        //{
        //    EllipticalDensityShapeObject objEx = null;

        //    try
        //    {
        //        objEx = new EllipticalDensityShapeObject(detectedObject);
        //        objEx.UpdateSpatialFeatures();                
        //    }
        //    catch
        //    {
        //        objEx = null;
        //    }
        //    finally
        //    {
        //    }

        //    return objEx;
        //}

        public static EllipticalDensityShapeObject
            FromDetectedObject(DetectedObject detectedObject, MetrologySystem ms)
        {
            EllipticalDensityShapeObject objEx = null;

            try
            {
                objEx = new EllipticalDensityShapeObject(detectedObject);
                objEx.UpdateSpatialFeatures();
                objEx.UpdateMetrologyInfo(ms);
            }
            catch
            {
                objEx = null;
            }
            finally
            {
            }

            return objEx;
        }

        #region Base Point Implementations
        public override float Length
        {
            get
            {
                return (float)MajorLength;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override float Orientation
        {
            get
            {
                return (float)MajorAxisOrientation;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override float X
        {
            get
            {
                return (float)CenterX;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override float Y
        {
            get
            {
                return (float)CenterY;
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        #endregion Base Point Implementations

        public static string[] VaribleList
        {
            get
            {
                List<string> alResult = new List<string>();

                alResult.Add("CLUSTERID");
                alResult.Add("OBJECTTYPE");
                alResult.Add("KNN_NUMBER");

                alResult.Add("IP_NEIGHBOR_DISTANCE");
                alResult.Add("IP_DENSITY");
                alResult.Add("IP_SKIP_NUMBER_UNIT_UNDER");

                alResult.Add("KNN_MIN_DISTANCE");
                alResult.Add("KNN_MAX_DISTANCE");
                alResult.Add("KNN_AVG_DISTANCE");
                alResult.Add("DEFECT_NUM");
                alResult.Add("DEFECTDIE_NUM");
                alResult.Add("OBJECT_ANGLE");
                alResult.Add("BOUND_HEIGHT");
                alResult.Add("BOUND_WIDTH");
                alResult.Add("BOUND_AREA");
                alResult.Add("SHAPE_AREA");
                alResult.Add("SHAPE_PERIMETER");
                alResult.Add("SHAPE_ELONGATION");
                alResult.Add("SHAPE_LENGTH");
                alResult.Add("SHAPE_THICK");
                alResult.Add("DEN_SHAPE_TAN_ANGLE");
                alResult.Add("DEN_SHAPE_DEFECT_AREA_RATIO");
                alResult.Add("DEN_SHAPE_MAJORMINOR_RATIO");
                alResult.Add("DEN_SHAPE_ROUNDNESS");
                alResult.Add("DEN_SHAPE_MINORMAJOR_RATIO");
                alResult.Add("DEN_SHAPE_XCENTROID");
                alResult.Add("DEN_SHAPE_YCENTROID");
                alResult.Add("DEN_SHAPE_MAJOR_LENGTH");
                alResult.Add("DEN_SHAPE_MINOR_LENGTH");
                alResult.Add("DEN_SHAPE_ECCENTRICITY");
                alResult.Add("DEN_SHAPE_MAJOR_ORIENTATION");
                alResult.Add("DEN_SHAPE_CENTROID_DISTANCE");

                //20061125
                alResult.Add("DEN_SHAPE_DIE_REL_X");
                alResult.Add("DEN_SHAPE_DIE_REL_Y");
                alResult.Add("DEN_SHAPE_DIE_REL_DISTANCE");

                //Added by Phong
                alResult.Add("DEN_SHAPE_CENTROID_DISTANCE_FROM_WAFER_BOUND");
                //alResult.Add("DEN_DIST_GRP_NUM");
                alResult.Add("DEN_DIST_GRP_MEAN1");
                alResult.Add("DEN_DIST_GRP_MEAN2");
                alResult.Add("DEN_DIST_GRP_MEAN3");
                alResult.Add("DEN_DIST_GRP_MEAN4");
                alResult.Add("DEN_DIST_GRP_MEAN5");
                alResult.Add("DEN_DIST_GRP_MEAN6");
                alResult.Add("DEN_DIST_GRP_STDDEV1");
                alResult.Add("DEN_DIST_GRP_STDDEV2");
                alResult.Add("DEN_DIST_GRP_STDDEV3");
                alResult.Add("DEN_DIST_GRP_STDDEV4");
                alResult.Add("DEN_DIST_GRP_STDDEV5");
                alResult.Add("DEN_DIST_GRP_STDDEV6");

                alResult.Add("DISCRETE_DEN_DIST_GRP_NUM");

                alResult.Add("DISCRETE_DEN_DIST_GRP_MEAN1");
                alResult.Add("DISCRETE_DEN_DIST_GRP_MEAN2");
                alResult.Add("DISCRETE_DEN_DIST_GRP_MEAN3");
                alResult.Add("DISCRETE_DEN_DIST_GRP_MEAN4");
                alResult.Add("DISCRETE_DEN_DIST_GRP_MEAN5");
                alResult.Add("DISCRETE_DEN_DIST_GRP_MEAN6");

                alResult.Add("DISCRETE_DEN_DIST_GRP_STDDEV1");
                alResult.Add("DISCRETE_DEN_DIST_GRP_STDDEV2");
                alResult.Add("DISCRETE_DEN_DIST_GRP_STDDEV3");
                alResult.Add("DISCRETE_DEN_DIST_GRP_STDDEV4");
                alResult.Add("DISCRETE_DEN_DIST_GRP_STDDEV5");
                alResult.Add("DISCRETE_DEN_DIST_GRP_STDDEV6");                
                //new
                alResult.Add("SHAPE_ROUNDNESS");
                alResult.Add("SHAPE_RATIO");
                alResult.Add("NORMALIZED_DENSITY");

                alResult.Add("ORTHOGONALITY");

                alResult.Add("SOLIDNESS");


                alResult.Sort();
                return alResult.ToArray();
            }
        }

        public static string[] VaribleListAfterReducing
        {
            get
            {
                List<String> alResult = new List<String>();
                alResult.Add("X-Centroid Coordinate(micron)");
                alResult.Add("Y-Centroid Coordinate(micron)");
                alResult.Add("Eccentricity");
                alResult.Add("Major length(micron)");
                alResult.Add("Minor length(micron)");
                alResult.Add("Orientation(degree)");
                //alResult.Add("Angle to center(degree)");
                alResult.Add("Elongation");

                // detected object
                //alResult.Add("Rectangle boundary width(micron)");
                //alResult.Add("Rectangle boundary height(micron)");
                alResult.Add("Area(micron^2)");
                alResult.Add("Perimeter(micron)");
                alResult.Add("Pixel count");

                // intensity
                alResult.Add("Average intensity");
                alResult.Add("Integrated intensity");
                //alResult.Add("Object type");

                return alResult.ToArray();
            }
        }

        public new object this[string fieldname]
        {
            get
            {
                switch (fieldname)
                {
                    case "X-Centroid Coordinate(micron)":
                        return this.X;
                    case "Y-Centroid Coordinate(micron)":
                        return this.X;
                    case "Eccentricity":
                        return this.Eccentricity;
                    case "Major length(micron)":
                        return this.MajorLength;
                    case "Minor length(micron)":
                        return this.MinorLength;
                    case "Orientation(degree)":
                        return this.Orientation;
                    //case "Angle to center(degree)":
                    //    return this.AngleToCenter;
                    case "Elongation":
                        return this.Elongation;
                    
                    //case "Rectangle boundary width(micron)":
                    //    return this.DetectedObject.RectBound.Width;
                    //case "Rectangle boundary height(micron)":
                    //    return this.DetectedObject.RectBound.Height;
                    case "Area(micron^2)":
                        return this.DetectedObject.Area;
                    case "Perimeter(micron)":
                        return this.DetectedObject.Perimeter;
                    case "Pixel count":
                        return this.DetectedObject.NumPixels;
                    
                    case "Average intensity":
                        return this.AverageIntensity;
                    case "Integrated intensity":
                        return this.DetectedObject.TotalIntensity;
                    //case "Object type":




                    //case "SHAPE_AREA":
                    //    return DetectedObject.Area;
                    //case "BOUND_AREA":
                    //    return this.boundarea;
                    //case "BOUND_HEIGHT":
                    //    return this.boundheight;
                    //case "BOUND_WIDTH":
                    //    return this.boundwidth;
                    //case "DEN_SHAPE_CENTROID_DISTANCE":
                    //    return this.fParams.mainEllipse.Radius;
                    //case "CLUSTERID":
                    //    return this.clusterID;
                    //case "DEFECTDIE_NUM":
                    //    return this.fParams.DefectDieNumber;
                    //case "DEFECT_NUM":
                    //    return this.DefectNumber;
                    //case "DEN_DIST_GRP_NUM":
                    //    return this.fParams.dDistribution.NumberOfClass;
                    //case "DISCRETE_DEN_DIST_GRP_NUM":
                    //    return this.fParams.dDistribution.NumberOfClass;
                    //case "DISCRETE_DEN_DIST_GRP_MEAN1":
                    //    return this.fParams.DiscretedDistribution.Mean[0];
                    //case "DISCRETE_DEN_DIST_GRP_STDDEV1":
                    //    return this.fParams.DiscretedDistribution.Std[0];
                    //case "DISCRETE_DEN_DIST_GRP_MEAN2":
                    //    return this.fParams.DiscretedDistribution.Mean[1];
                    //case "DISCRETE_DEN_DIST_GRP_STDDEV2":
                    //    return this.fParams.DiscretedDistribution.Std[1];
                    //case "DISCRETE_DEN_DIST_GRP_MEAN3":
                    //    return this.fParams.DiscretedDistribution.Mean[2];
                    //case "DISCRETE_DEN_DIST_GRP_STDDEV3":
                    //    return this.fParams.DiscretedDistribution.Std[2];
                    //case "DISCRETE_DEN_DIST_GRP_MEAN4":
                    //    return this.fParams.DiscretedDistribution.Mean[3];
                    //case "DISCRETE_DEN_DIST_GRP_STDDEV4":
                    //    return this.fParams.DiscretedDistribution.Std[3];
                    //case "DISCRETE_DEN_DIST_GRP_MEAN5":
                    //    return this.fParams.DiscretedDistribution.Mean[4];
                    //case "DISCRETE_DEN_DIST_GRP_STDDEV5":
                    //    return this.fParams.DiscretedDistribution.Std[4];
                    //case "DISCRETE_DEN_DIST_GRP_MEAN6":
                    //    return this.fParams.DiscretedDistribution.Mean[5];
                    //case "DISCRETE_DEN_DIST_GRP_STDDEV6":
                    //    return this.fParams.DiscretedDistribution.Std[5];
                    //case "DEN_SHAPE_ECCENTRICITY":
                    //    return this.fParams.mainEllipse.Eccentricity;
                    //case "DEN_SHAPE_DIE_REL_X":
                    //    return this.fParams.DieRelativeX;
                    //case "DEN_SHAPE_DIE_REL_Y":
                    //    return this.fParams.DieRelativeY;
                    //case "DEN_SHAPE_DIE_REL_DISTANCE":
                    //    return this.fParams.DieRelativeDistance;

                    //case "SHAPE_ELONGATION":
                    //    return this.elongation;
                    //case "DEN_DIST_GRP_MEAN1":
                    //    return this.fParams.dDistribution.Mean[0];
                    //case "DEN_DIST_GRP_STDDEV1":
                    //    return this.fParams.dDistribution.Std[0];
                    //case "DEN_DIST_GRP_MEAN2":
                    //    return this.fParams.dDistribution.Mean[1];
                    //case "DEN_DIST_GRP_STDDEV2":
                    //    return this.fParams.dDistribution.Std[1];
                    //case "DEN_DIST_GRP_MEAN3":
                    //    return this.fParams.dDistribution.Mean[2];
                    //case "DEN_DIST_GRP_STDDEV3":
                    //    return this.fParams.dDistribution.Std[2];

                    //case "DEN_DIST_GRP_MEAN4":
                    //    return this.fParams.dDistribution.Mean[3];
                    //case "DEN_DIST_GRP_STDDEV4":
                    //    return this.fParams.dDistribution.Std[3];
                    //case "DEN_DIST_GRP_MEAN5":
                    //    return this.fParams.dDistribution.Mean[4];
                    //case "DEN_DIST_GRP_STDDEV5":
                    //    return this.fParams.dDistribution.Std[4];
                    //case "DEN_DIST_GRP_MEAN6":
                    //    return this.fParams.dDistribution.Mean[5];
                    //case "DEN_DIST_GRP_STDDEV6":
                    //    return this.fParams.dDistribution.Std[5];

                    //case "DEN_SHAPE_TAN_ANGLE":
                    //    return this.fParams.mainEllipse.TangentialAngle;

                    //case "IP_NEIGHBOR_DISTANCE":
                    //    return this.IP_NeighborDistance;
                    //case "IP_DENSITY":
                    //    return this.IP_Density;
                    //case "IP_SKIP_NUMBER_UNIT_UNDER":
                    //    return this.IP_SkipUnits;

                    //case "KNN_NUMBER":
                    //    return this.IP_k;
                    //case "KNN_AVG_DISTANCE":
                    //    return this.KnnAverageDistance;
                    //case "KNN_MAX_DISTANCE":
                    //    return this.KnnMaxDistance;
                    //case "KNN_MIN_DISTANCE":
                    //    return this.KnnMinDistance;
                    //case "SHAPE_LENGTH":
                    //    return this.length;
                    //case "DEN_SHAPE_MAJOR_LENGTH":
                    //    return this.fParams.mainEllipse.MajorAxisLength;
                    //case "DEN_SHAPE_MINOR_LENGTH":
                    //    return this.fParams.mainEllipse.MinorAxisLength;
                    //case "DEN_SHAPE_DEFECT_AREA_RATIO":
                    //    return this.fParams.Density;
                    //case "OBJECT_ANGLE":
                    //    return this.fParams.mainEllipse.AngleWithNotch;
                    //case "OBJECTTYPE":
                    //    return this.sObjectType;
                    //case "SHAPE_PERIMETER":
                    //    return this.perimeter;
                    //case "SHAPE_THICK":
                    //    return this.thick;
                    //case "DEN_SHAPE_XCENTROID":
                    //    return this.fParams.mainEllipse.CentroidByNotch.X;
                    //case "DEN_SHAPE_YCENTROID":
                    //    return this.fParams.mainEllipse.CentroidByNotch.Y;
                    //case "DEN_SHAPE_MAJOR_ORIENTATION":
                    //    return this.fParams.mainEllipse.OrientationWithNotch;
                    //case "DEN_SHAPE_MAJORMINOR_RATIO":
                    //    return this.fParams.mainEllipse.Elongation;
                    //case "DEN_SHAPE_ROUNDNESS":
                    //    return this.fParams.mainEllipse.Roundness;
                    //case "DEN_SHAPE_MINORMAJOR_RATIO":
                    //    return this.MinorMajorRatio;

                    //case "SHAPE_ROUNDNESS":
                    //    return this.RoundNess;

                    //case "DEN_SHAPE_CENTROID_DISTANCE_FROM_WAFER_BOUND":
                    //    return this.fParams.mainEllipse.RadiusFromWaferBound;

                    //case "MICRO_STRUCT_ANGLE_MIN":
                    //    return this.fParams.fMicroStructure.AngleMicroMin;
                    //case "MICRO_STRUCT_ANGLE_MAX":
                    //    return this.fParams.fMicroStructure.AngleMicroMax;
                    //case "MICRO_STRUCT_ANGLE_MEAN":
                    //    return this.fParams.fMicroStructure.AngleMicroMean;
                    //case "MICRO_STRUCT_ANGLE_STD":
                    //    return this.fParams.fMicroStructure.AngleMicroStd;
                    //case "MICRO_STRUCT_ANGLE_ENTROPY":
                    //    return this.fParams.fMicroStructure.AngleMicroRand;


                    //case "MICRO_STRUCT_XDRATIO_MIN":
                    //    return this.fParams.fMicroStructure.X_D_RatioMin;
                    //case "MICRO_STRUCT_XDRATIO_MAX":
                    //    return this.fParams.fMicroStructure.X_D_RatioMax;
                    //case "MICRO_STRUCT_XDRATIO_MEAN":
                    //    return this.fParams.fMicroStructure.X_D_RatioMean;
                    //case "MICRO_STRUCT_XDRATIO_STD":
                    //    return this.fParams.fMicroStructure.X_D_RatioStd;


                    //case "MICRO_STRUCT_YDRATIO_MIN":
                    //    return this.fParams.fMicroStructure.Y_D_RatioMin;
                    //case "MICRO_STRUCT_YDRATIO_MAX":
                    //    return this.fParams.fMicroStructure.Y_D_RatioMax;
                    //case "MICRO_STRUCT_YDRATIO_MEAN":
                    //    return this.fParams.fMicroStructure.Y_D_RatioMean;
                    //case "MICRO_STRUCT_YDRATIO_STD":
                    //    return this.fParams.fMicroStructure.Y_D_RatioStd;

                    //case "SHAPE_RATIO":
                    //    return this.ShapeRatio;
                    //case "NORMALIZED_DENSITY":
                    //    return this.NormalizedDensity;
                    //case "ORTHOGONALITY":
                    //    return this.Orthogonality;

                    //case "SOLIDNESS":
                    //    return this.Solidness;


                    default:
                        throw new System.Exception(
                            string.Format("Not supported the variable: {0}", fieldname));
                }
                return null;
            }
        }
    }
}
