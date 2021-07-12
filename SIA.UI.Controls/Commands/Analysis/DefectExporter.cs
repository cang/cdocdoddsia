//#define REPORT_SON

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using SIA.Common.Analysis;
using SiGlaz.Common.ImageAlignment;
using SiGlaz.Common.Object;
using SiGlaz.Common.ABSDefinitions;
using SIA.Algorithms.Preprocessing.Alignment;

namespace SIA.UI.Controls.Commands.Analysis
{
    public class DefectExporter
    {
        public static void SaveAsText(
            ArrayList objList, MetrologySystem ms, 
            string fileName, string image_processing_file, string customizedName)
        {
            using (StreamWriter writer = File.CreateText(fileName))
            {
                if (objList == null || objList.Count == 0)
                {
                    writer.WriteLine(string.Format("Total Anomalies: 0"));
                    writer.Write(string.Format("Image file path: {0}", image_processing_file));
                    return;
                }

                StringBuilder stringBuilder = new StringBuilder();

                stringBuilder.Append(string.Format("Total Anomalies: {0}", objList.Count));
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(string.Format("Image file path: {0}", image_processing_file));
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(string.Format("Unit: {0}", "micron"));
                stringBuilder.Append(Environment.NewLine);

                int len = objList.Count;
                int index = 1;

                string format_12_pad_right = "{0,-12}";
                string format_25_pad_right = "{0,-25}";

                stringBuilder.Append(string.Format("Anomaly list:"));
                stringBuilder.Append(Environment.NewLine);

                stringBuilder.Append(string.Format(format_12_pad_right, "Index"));
                stringBuilder.Append(string.Format(format_25_pad_right, "Signature Name"));
                stringBuilder.Append(string.Format(format_12_pad_right, "X-Centroid"));
                stringBuilder.Append(string.Format(format_12_pad_right, "Y-Centroid"));
                stringBuilder.Append(string.Format(format_12_pad_right, "Area"));
                stringBuilder.Append(string.Format(format_12_pad_right, "Perimeter"));
                stringBuilder.Append(string.Format(format_25_pad_right, "Number of Pixels"));
                stringBuilder.Append(string.Format(format_25_pad_right, "Average Intensity"));
                stringBuilder.Append(string.Format(format_25_pad_right, "Integrated Intensity"));

                stringBuilder.Append(Environment.NewLine);

                foreach (DetectedObject obj in objList)
                {
                    string sigName = "";
                    if (customizedName != "")
                    {
                        sigName = customizedName;
                    }
                    else
                    {
                        if (obj is DetectedObjectEx)
                            sigName = (obj as DetectedObjectEx).SignatureName;
                        else
                            sigName = ((SiGlaz.Common.ABSDefinitions.eDefectType)obj.ObjectTypeId).ToString();
                    }

                    EllipticalDensityShapeObject ellipticalObj =
                        EllipticalDensityShapeObject.FromDetectedObject(obj, ms);

                    stringBuilder.Append(string.Format(format_12_pad_right, index.ToString()));
                    stringBuilder.Append(string.Format(format_25_pad_right, sigName));
                    stringBuilder.Append(string.Format(format_12_pad_right, ellipticalObj.CenterX.ToString("0.##")));
                    stringBuilder.Append(string.Format(format_12_pad_right, ellipticalObj.CenterY.ToString("0.##")));
                    stringBuilder.Append(string.Format(format_12_pad_right, obj.Area.ToString("0.##")));
                    stringBuilder.Append(string.Format(format_12_pad_right, obj.Perimeter.ToString("0.##")));
                    stringBuilder.Append(string.Format(format_25_pad_right, obj.NumPixels.ToString("0")));
                    stringBuilder.Append(string.Format(format_25_pad_right, (obj.TotalIntensity / obj.NumPixels).ToString("0.##")));
                    stringBuilder.Append(string.Format(format_25_pad_right, obj.TotalIntensity.ToString("0")));

                    if (index++ < len)
                        stringBuilder.Append(Environment.NewLine);
                }

                writer.Write(stringBuilder.ToString());

                writer.Flush();
            }
        }

        public static void SaveAsCSV(
            ArrayList objList, MetrologySystem ms, 
            string fileName, string image_processing_file, string customizedName)
        {
            StreamWriter writer = null;

            try
            {
#if REPORT_SON
                string path = Path.GetDirectoryName(fileName);
                string name = Path.GetFileNameWithoutExtension(fileName);
                string ext = Path.GetExtension(fileName);
                fileName = Path.Combine(
                    path, string.Format("{0}_{1}{2}", name, objList.Count, ext));
#endif

                writer = File.CreateText(fileName);
                StringBuilder stringBuilder = new StringBuilder();

                int _len = objList.Count;
                int index = 1;

                stringBuilder.Append(string.Format("Summary:"));
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(string.Format(",Image file path:,{0}", image_processing_file));
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(string.Format(",Total Anomalies:,{0}", objList.Count));
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(string.Format(",Metrology System:"));
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(string.Format(",Unit:,{0}", "micron"));
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(string.Format(",Unit Scale:, [{0:0.####} pixels] = [{1:0.####} microns]", ms.CurrentUnit.PixelVal, ms.CurrentUnit.UnitVal));
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(string.Format(",Coordinate System:"));
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(string.Format(",Orientation:,{0}", ms.CurrentCoordinateSystem.Orientation.ToString()));
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(string.Format(",Origin:,(x = {0:0.####}; y = {1:0.####}) (pixel)", ms.CurrentCoordinateSystem.DrawingOriginX, ms.CurrentCoordinateSystem.DrawingOriginY));
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(string.Format(",Axis-X Rotation Angle:,{0:0.####} (degre)", ms.CurrentCoordinateSystem.DrawingAngle));
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(Environment.NewLine);

                stringBuilder.Append(string.Format("Details:"));
                stringBuilder.Append(Environment.NewLine);

                stringBuilder.Append("Index, Signature Name, X-Centroid, Y-Centroid, Area, Perimeter, Number of Pixels, Average Intensity, Integrated Intensity");

                stringBuilder.Append(Environment.NewLine);

                foreach (DetectedObject obj in objList)
                {
                    string sigName = "";
                    if (customizedName != "")
                    {
                        sigName = customizedName;
                    }
                    else
                    {
                        if (obj is DetectedObjectEx)
                            sigName = (obj as DetectedObjectEx).SignatureName;
                        else
                            sigName = ((SiGlaz.Common.ABSDefinitions.eDefectType)obj.ObjectTypeId).ToString();
                    }

                    EllipticalDensityShapeObject ellipticalObj =
                        EllipticalDensityShapeObject.FromDetectedObject(obj, ms);

                    stringBuilder.Append(
                        index.ToString() + "," +
                        sigName + "," +
                        ellipticalObj.CenterX.ToString("0.##") + "," +
                        ellipticalObj.CenterY.ToString("0.##") + "," +
                        obj.Area.ToString("0.##") + "," +
                        obj.Perimeter.ToString("0.##") + "," +
                        obj.NumPixels.ToString("0") + "," +
                        (obj.TotalIntensity / obj.NumPixels).ToString("0.##") + "," +
                        obj.TotalIntensity.ToString("0"));

                    if (index++ < _len)
                        stringBuilder.Append(Environment.NewLine);
                }

                writer.Write(stringBuilder.ToString());
            }
            finally
            {
                if (writer != null)
                    writer.Close();
                writer = null;
            }
        }

        public static void SaveAlignmentReportAsCSV(
            MetrologySystem ms, AlignmentResult alignResult,
            string fileName, string image_processing_file)
        {
            StreamWriter writer = null;

            try
            {
                string status = "";
#if REPORT_SON
                status = "FAILED";
                if (alignResult.MatchedConfidences != null &&
                    alignResult.MatchedConfidences.Length > 0)
                {
                    double threshold = 0.5;

                    int len = alignResult.MatchedConfidences.Length;
                    int count = 0;
                    for (int i = 0; i < len; i++, i++)
                    {
                        if (alignResult.MatchedConfidences[i] > threshold)
                            count++;
                        if (count >= 4)
                            break;
                    }

                    if (count >= 4)
                    {
                        status = "PASSED";
                    }
                }
#endif

                string path = Path.GetDirectoryName(fileName);
                string name = Path.GetFileNameWithoutExtension(fileName);
                string ext = Path.GetExtension(fileName);
                fileName = Path.Combine(
                    path, string.Format("{0}_{1}{2}", name, status, ext));

                writer = File.CreateText(fileName);
                StringBuilder stringBuilder = new StringBuilder();

                stringBuilder.Append(string.Format("Image file path:,{0}", image_processing_file));
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(string.Format("Metrology System:"));
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(string.Format("Unit:,{0}", "micron"));
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(string.Format("Unit Scale:, [{0:0.####} pixels] = [{1:0.####} microns]", ms.CurrentUnit.PixelVal, ms.CurrentUnit.UnitVal));
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(string.Format("Coordinate System:"));
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(string.Format("Orientation:,{0}", ms.CurrentCoordinateSystem.Orientation.ToString()));
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(string.Format("Origin:,(x = {0:0.####}; y = {1:0.####}) (pixel)", ms.CurrentCoordinateSystem.DrawingOriginX, ms.CurrentCoordinateSystem.DrawingOriginY));
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(string.Format("Axis-X Rotation Angle:,{0:0.####} (degre)", ms.CurrentCoordinateSystem.DrawingAngle));
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(Environment.NewLine);

                stringBuilder.Append(string.Format("Detected Marker Details:"));
                stringBuilder.Append(Environment.NewLine);

                stringBuilder.Append("Index, X-Centroid, Y-Centroid, Matched Confidence, Status");

                stringBuilder.Append(Environment.NewLine);

                if (alignResult.MatchedConfidences != null && 
                    alignResult.MatchedConfidences.Length > 0)
                {
                    double threshold = 0.5;

                    int len = alignResult.MatchedConfidences.Length;
                    int index = 1;
                    for (int i = 0; i < len; i++, index++)
                    {
                        stringBuilder.Append(
                            index.ToString() + "," +
                            alignResult.MatchedXList[i].ToString("0.####") + "," +
                            alignResult.MatchedYList[i].ToString("0.####") + "," +
                            alignResult.MatchedConfidences[i].ToString("0.####") + "," +
                            (alignResult.MatchedConfidences[i] > threshold ? "Passed" : "Failed"));

                        if (index < len)
                            stringBuilder.Append(Environment.NewLine);
                    }
                }

                writer.Write(stringBuilder.ToString());
            }
            finally
            {
                if (writer != null)
                    writer.Close();
                writer = null;
            }
        }
    }
}
