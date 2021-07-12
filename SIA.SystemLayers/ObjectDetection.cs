using System;
using System.Collections.Generic;
using System.Text;
using SIA.SystemFrameworks.UI;
using SIA.Common.Analysis;
using System.Collections;
using SIA.IPEngine;
using System.Drawing;
using System.Diagnostics;
using SIA.SystemLayer.ObjectExtraction.Utilities;

namespace SIA.SystemLayer
{
    public class ObjectDetection_
    {
        public static ArrayList DetectObject(CommonImage image, BinaryImage binary_image)
        {
            ArrayList detectedObjects = new ArrayList();

            // extract object from thresholded image
            IntPtr binary_buffer = binary_image.Buffer;

            int width = image.RasterImage.Width;
            int height = image.RasterImage.Height;

            // update progress callback
            CommandProgress.SetText("Detecting objects ...");

#if DEBUG
            DateTime started = DateTime.Now;
#endif

            // detect objects				
            DetectedObjectCollection objects = 
                SIA.IPEngine.ObjectDetector.FindObjects(width, height, binary_buffer);

            // update callback status
            if (CommandProgress.IsAborting)
                throw new CommandAbortedException();

            // update progress callback
            CommandProgress.SetText("Retreiving objects external data information...");
            CommandProgress.SetRange(0, 100);

#if DEBUG
            DateTime finished = DateTime.Now;
            Console.WriteLine(string.Format("Retrieve info: {0}", (finished - started).TotalMilliseconds));

            double duration = (finished - started).TotalMilliseconds;

            started = DateTime.Now;
#endif

            //[Nov 20 2006: calculate object external data such as: numPixels and totalIntensity
            if (objects.Count > 0)
            {
                //improving only:
                int maxNumContours = 0, maxNumPoints = 0;
                foreach (DetectedObject obj in objects)
                {
                    if (obj.PolygonBoundary != null)
                    {
                        if (obj.PolygonBoundary.nContours > maxNumContours)
                            maxNumContours = obj.PolygonBoundary.nContours;
                        if (obj.PolygonBoundary.nPoints > maxNumPoints)
                            maxNumPoints = obj.PolygonBoundary.nPoints;
                    }
                }

                try
                {
                    PolygonEx poly = new PolygonEx(maxNumContours, maxNumPoints, false);
                    PolygonExData polyData = null;

                    float numPixels = 0, totalIntensity = 0;
                    int num_objects = objects.Count;

                    for (int i = 0; i < num_objects; i++)
                    {
                        DetectedObject obj = objects[i] as DetectedObject;
                        if (obj == null)
                            continue;

                        if (obj.PolygonBoundary == null)
                            continue;

                        // update callback status
                        if (CommandProgress.IsAborting)
                            throw new CommandAbortedException();

                        try
                        {
                            polyData = obj.PolygonBoundary;
                            if (polyData != null)
                            {
                                poly.UpdatePolygonEx(polyData);

                                poly.Intialize();

                                numPixels = 0; totalIntensity = 0;
                                PointF gravity = PointF.Empty;
                                //poly.GetObjectExtData(image.RasterImage, ref numPixels, ref totalIntensity);
                                poly.GetObjectExtData(image.RasterImage, polyData.ExtPoints, ref numPixels, ref totalIntensity, ref gravity);
                                obj.NumPixels = numPixels; obj.TotalIntensity = totalIntensity;
                                obj.Gravity = gravity;

                                // correct area and perimeter of objects by total intensity
                                if (obj.Area == 0)
                                    obj.Area = numPixels;
                                if (obj.Perimeter == 0)
                                    obj.Perimeter = numPixels;
                            }
                        }
                        catch (System.Exception exp)
                        {
                            Trace.WriteLine(exp);
                        }
                    }

                    CommandProgress.StepTo(100);

                    if (poly != null)
                        poly.Dispose();
                }
                catch (System.OutOfMemoryException exp)
                {
                    Trace.WriteLine(exp);
                }

                if (objects != null && objects.Count > 0)
                {
                    foreach (DetectedObject obj in objects)
                    {
                        float l = obj.RectBound.Left;
                        float t = obj.RectBound.Top;
                        float b = obj.RectBound.Bottom;
                        float r = obj.RectBound.Right;

                        obj.RectBound = new RectangleF(
                            l, t, r - l + 1.0f, b - t + 1.0f);
                    }
                }

                detectedObjects.AddRange(objects);

                CommandProgress.StepTo(100);
            }

#if DEBUG
            finished = DateTime.Now;
            Console.WriteLine(string.Format("Retrieve info: {0}", (finished - started).TotalMilliseconds));

            duration = (finished - started).TotalMilliseconds;
#endif

            return detectedObjects;
        }

        public static void UpdateObjectTypeId(ArrayList objects, int objectTypeId)
        {
            if (objects == null || objects.Count <= 0)
                return;

            foreach (DetectedObject obj in objects)
            {
                obj.ObjectTypeId = objectTypeId;
            }
        }

        public static void UpdateObjectTypeId(
            ArrayList objects, 
            int darkObjectId, double darkThreshold, 
            int brightObjectId, double brightThreshold)
        {
            if (objects == null || objects.Count <= 0)
                return;

            foreach (DetectedObject obj in objects)
            {
                double avgIntensity = obj.TotalIntensity / obj.NumPixels;
                if (avgIntensity <= darkThreshold)
                    obj.ObjectTypeId = darkObjectId;
                else if (avgIntensity >= brightThreshold)
                    obj.ObjectTypeId = brightObjectId;
            }
        }
    }
}
