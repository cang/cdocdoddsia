using System;
using System.Collections.Generic;
using System.Text;
using SIA.SystemFrameworks.UI;
using SIA.Common.Analysis;
using System.Collections;
using SIA.IPEngine;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Drawing2D;

namespace SIA.Algorithms
{
    using SimpleCluster = System.Collections.Generic.List<int>;

    public class ObjectDetector
    {
        public static ArrayList DetectObject(GreyDataImage image, BinaryImage binary_image)
        {
            ArrayList detectedObjects = new ArrayList();

            // extract object from thresholded image
            IntPtr binary_buffer = binary_image.Buffer;

            int width = image.Width;
            int height = image.Height;

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
                    using (PolygonEx poly = new PolygonEx(maxNumContours, maxNumPoints, false))
                    {
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
                                    poly.GetObjectExtData(image, polyData.ExtPoints, ref numPixels, ref totalIntensity, ref gravity);
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
                    }

                    CommandProgress.StepTo(100);
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

        public static ArrayList DetectObject(
            GreyDataImage image, int[] interestedPixels,
            Rectangle[] rois, int[] roiIdxStarts, int[] roiIdxEnds)
        {
            ArrayList detectedObjects = new ArrayList();

            try
            {
                int nROIs = rois.Length;
                List<DetectedObject>[] tmps = new List<DetectedObject>[nROIs];

                //SIA.SystemFrameworks.Parallel.For(0, nROIs, delegate(int iROI)
                for (int iROI = 0; iROI < nROIs; iROI++)
                {
                    if (rois[iROI].Width >= 1 && rois[iROI].Height >= 1)
                    {
                        tmps[iROI] = DetectObject(image,
                            rois[iROI], interestedPixels,
                            roiIdxStarts[iROI], roiIdxEnds[iROI]);
                    }
                }
                //);

                for (int i = 0; i < nROIs; i++)
                {
                    if (tmps[i] == null)
                        continue;

                    detectedObjects.AddRange(tmps[i]);
                }
            }
            catch
            {
                if (detectedObjects != null)
                    detectedObjects.Clear();

                throw;
            }


            return detectedObjects;
        }

        private static List<DetectedObject> DetectObject(BinaryImage binary_image)
        {
            List<DetectedObject> detectedObjects = new List<DetectedObject>();

            // extract object from thresholded image
            IntPtr binary_buffer = binary_image.Buffer;

            // detect objects				
            DetectedObjectCollection objects =
                SIA.IPEngine.ObjectDetector.FindObjects(
                binary_image.Width, binary_image.Height, binary_buffer);

            if (objects != null && objects.Count > 0)
                detectedObjects.AddRange(
                    objects.ToArray(typeof(DetectedObject)) as DetectedObject[]);

            return detectedObjects;
        }
        
        public static List<DetectedObject> DetectObject(
            GreyDataImage originGreyImage, 
            BinaryImage roi_image, int roiLeft, int roiTop)
        {
            List<DetectedObject> detectedObjects = new List<DetectedObject>();

            // detect objects				
            DetectedObjectCollection objects = null;
            unsafe
            {
                objects = SIA.IPEngine.ObjectDetector.FindObjects(
                    roi_image.Width, roi_image.Height, (bool*)roi_image.BinarayData);
            }

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
                    using (PolygonEx poly = new PolygonEx(maxNumContours, maxNumPoints, false))
                    {
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
                                    obj.Offset(roiLeft, roiTop);

                                    poly.UpdatePolygonEx(polyData);

                                    poly.Intialize();

                                    numPixels = 0; totalIntensity = 0;
                                    PointF gravity = PointF.Empty;
                                    //poly.GetObjectExtData(image.RasterImage, ref numPixels, ref totalIntensity);
                                    poly.GetObjectExtData(originGreyImage, polyData.ExtPoints, ref numPixels, ref totalIntensity, ref gravity);
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
                    }
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

                detectedObjects.AddRange(
                    objects.ToArray(typeof(DetectedObject)) as DetectedObject[]);
            }

            return detectedObjects;
        }

        private static List<DetectedObject> DetectObject(
            GreyDataImage image,
            Rectangle roi, int[] interestedPixels, int start, int end)
        {
            List<DetectedObject> detectedObjects = null;

            int stride = image.Width;
            int l = roi.Left;
            int t = roi.Top;
            int w = roi.Width;
            int h = roi.Height;
            int x, y;
            using (BinaryImage binImage = new BinaryImage(w, h))
            {
                // fill image
                unsafe
                {                    
                    bool* buffer = (bool*)binImage.BinarayData;                
                    for (int pixel = start; pixel <= end; pixel++)
                    {
                        y = (interestedPixels[pixel] / stride) - t;
                        x = (interestedPixels[pixel] % stride) - l;
                        buffer[y * w + x] = true;
                    }
                    buffer = null;
                }

                //binImage.Save(@"D:\temp\test_bin_contour.bin");

                detectedObjects = DetectObject(image, binImage, l, t);
            }

            return detectedObjects;
        }

        #region Update object information
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
        #endregion Update object information
    }

    public class ObjectDetectionHelper
    {
        public static int[] KeepPixelsInsideRegions(
            int imageStride, int[] pixels, GraphicsPath path)
        {
            if (path == null || pixels == null) return pixels;

            List<int> interestedPixels = new List<int>(pixels.Length);

            using (Region region = new Region(path))
            {
                int x, y;
                int n = pixels.Length;
                for (int i = 0; i < n; i++)
                {
                    x = pixels[i] % imageStride;
                    y = pixels[i] / imageStride;

                    if (region.IsVisible(x, y))
                    {
                        interestedPixels.Add(pixels[i]);
                    }
                }
            }

            if (interestedPixels.Count == 0)
                return null;

            return interestedPixels.ToArray();
        }

        private static int rastering_size = 16;
        
        public static void Preparing(
            int imageWidth, int imageHeight, ref int[] interestedPixels,
            ref Rectangle[] rois, ref int[] roiIdxStarts, ref int[] roiIdxEnds)
        {
            try
            {
                int nPixels = interestedPixels.Length;
                int nRows = (imageHeight + rastering_size - 1) / rastering_size;
                int nCols = (imageWidth + rastering_size - 1) / rastering_size;

                int x, y, block_id;
                int[] map = new int[nPixels];
                bool[] blocks = new bool[nRows * nCols];
                for (int i = 0; i < nPixels; i++)
                {
                    x = (interestedPixels[i] % imageWidth) / rastering_size;
                    y = (interestedPixels[i] / imageWidth) / rastering_size;
                    block_id = y * nCols + x;
                    map[i] = block_id;
                    blocks[block_id] = true;
                }

                // sort-by block id
                Array.Sort(map, interestedPixels);
                // segment here
                int[] segments = Utilities.Segments(map, nPixels);
                int[] block_map = new int[blocks.Length];
                int nSegments = segments.Length;
                int segIdxStart = 0;
                for (int i = 0; i < nSegments; i++)
                {
                    // get end segment
                    int segIdxEnd = segments[i];

                    // update to block map
                    block_map[map[segIdxStart]] = i + 1; // when access will descrease

                    // go to next segment
                    segIdxStart = segIdxEnd + 1;
                }

                int[] xList = null, yList = null;
                int[] labelMatrix = null;
                int nClusters = 0;
                float maxDistance = (float)Math.Sqrt(3);

                SiGlaz.Algorithms.Core.MatrixUtils.GetConnectedComponent(
                    blocks, nCols, maxDistance, ref nClusters, ref xList, ref yList, ref labelMatrix);

                rois = new Rectangle[nClusters];
                roiIdxStarts = new int[nClusters];
                roiIdxEnds = new int[nClusters];

                SimpleCluster[] clusters = new SimpleCluster[nClusters];
                for (int i = 0; i < nClusters; i++)
                    clusters[i] = new SimpleCluster();
                int cluster_id;
                for (int i = labelMatrix.Length - 1; i >= 0; i--)
                {
                    if (labelMatrix[i] < 1)
                        continue;

                    cluster_id = labelMatrix[i] - 1;
                    block_id = yList[i] * nCols + xList[i];
                    SimpleCluster cluster = clusters[cluster_id];
                    int segment_id = block_map[block_id] - 1;
                    int start = 0;
                    if (segment_id > 0)
                        start = segments[segment_id - 1] + 1;
                    int end = segments[segment_id];
                    for (int j = start; j <= end; j++)
                    {
                        cluster.Add(interestedPixels[j]);
                    }
                }

                List<int> pixels = new List<int>(nPixels);

                // update info
                for (int i = 0; i < nClusters; i++)
                {
                    int l = int.MaxValue;
                    int t = int.MaxValue;
                    int r = int.MinValue;
                    int b = int.MinValue;


                    roiIdxStarts[i] = pixels.Count;
                    SimpleCluster cluster = clusters[i];
                    for (int j = cluster.Count - 1; j >= 0; j--)
                    {
                        x = cluster[j] % imageWidth;
                        y = cluster[j] / imageWidth;

                        if (l > x) l = x;
                        if (r < x) r = x;
                        if (t > y) t = y;
                        if (b < y) b = y;
                    }
                    pixels.AddRange(cluster);
                    roiIdxEnds[i] = pixels.Count - 1;

                    l -= 1; if (l < 0) l = 0;
                    t -= 1; if (t < 0) t = 0;
                    r += 1; if (r >= imageWidth) r = imageWidth - 1;
                    b += 1; if (b >= imageHeight) b = imageHeight - 1;
                    rois[i] = new Rectangle(l, t, r - l + 1, b - t + 1);
                }

                interestedPixels = pixels.ToArray();
            }
            catch
            {
                throw;
            }
        }
    }
}
