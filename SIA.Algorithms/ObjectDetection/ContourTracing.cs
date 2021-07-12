using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace SIA.Algorithms.ObjectDetection
{    
    using ChainCodes = List<int>;
    using Polygon = List<Point>;
    using PolygonF = List<PointF>;
    using PolygonList = List<List<Point>>;
    using PolygonFList = List<List<PointF>>;

    public class Blob
    {
        public int Label = 0;
        public Contour ExtraContour = null;
        public List<Contour> InternalContours = new List<Contour>();

        public Blob(int label, int startX, int startY)
        {
            Label = label;
            ExtraContour = new Contour(startX, startY);
        }

        public PolygonList ToPolygons()
        {
            PolygonList pl = new List<List<Point>>();

            pl.Add(ExtraContour.ToPolygon());
            if (InternalContours != null && InternalContours.Count > 0)
            {
                for (int i = 0; i < InternalContours.Count; i++)
                {
                    pl.Add(InternalContours[i].ToPolygon());
                }
            }

            return pl;
        }

        public PolygonFList ToPolygonFs()
        {
            PolygonFList pl = new List<List<PointF>>();

            pl.Add(ExtraContour.ToPolygonF());
            if (InternalContours != null && InternalContours.Count > 0)
            {
                for (int i = 0; i < InternalContours.Count; i++)
                {
                    pl.Add(InternalContours[i].ToPolygonF());
                }
            }

            return pl;
        }

        public void ExtractGeometricFeatures(ref PolygonList pl,
            ref double area, ref double perimeter, ref double circularity)
        {
            area = 0; perimeter = 0; ; circularity = 0;
            pl = new List<List<Point>>();
            pl.Add(ExtraContour.ToPolygon());
            ExtraContour.ExtractGeometricFeatures(pl[0], ref area, ref perimeter, ref circularity);
            if (InternalContours != null && InternalContours.Count > 0)
            {
                double ta = 0, tp = 0, tc = 0;
                for (int i = 0; i < InternalContours.Count; i++)
                {
                    Polygon hp = InternalContours[i].ToPolygon();
                    pl.Add(hp);
                    InternalContours[i].ExtractGeometricFeatures(
                        hp, ref ta, ref tp, ref tc);
                    area -= ta;
                    perimeter += tp;
                }

                // circularity is similar/alias compactness
                circularity = (perimeter * perimeter / area) - 4.0 * Math.PI;
                circularity = Math.Max(0, circularity);
            }
        }

        public void ExtractGeometricFeatures(ref PolygonFList pl,
            ref double area, ref double perimeter, ref double circularity)
        {
            area = 0; perimeter = 0; ; circularity = 0;
            pl = new List<List<PointF>>();
            pl.Add(ExtraContour.ToPolygonF());
            ExtraContour.ExtractGeometricFeatures(pl[0], ref area, ref perimeter, ref circularity);
            if (InternalContours != null && InternalContours.Count > 0)
            {
                double ta = 0, tp = 0, tc = 0;
                for (int i = 0; i < InternalContours.Count; i++)
                {
                    PolygonF hp = InternalContours[i].ToPolygonF();
                    pl.Add(hp);
                    InternalContours[i].ExtractGeometricFeatures(
                        hp, ref ta, ref tp, ref tc);
                    area -= ta;
                    perimeter += tp;
                }

                // circularity is similar/alias compactness
                circularity = (perimeter * perimeter / area) - 4.0 * Math.PI;
                circularity = Math.Max(0, circularity);
            }
        }
    }

    public class Contour
    {
        private static int[][] chainCodeOffsets =
            new int[][]
            { 
              new int[] { 0, -1 },
              new int[] { 1, -1 },
			  new int[] { 1,  0 },
			  new int[] { 1,  1 },
			  new int[] { 0,  1 },
			  new int[] {-1,  1 },
			  new int[] {-1,  0 },
			  new int[] {-1, -1 }
            };        

        public int StartX = 0;
        public int StartY = 0;

        public Contour(int startX, int startY)
        {
            StartX = startX;
            StartY = startY;
        }

        public ChainCodes ChainCodes = new ChainCodes();

        public Polygon ToPolygon()
        {
            Polygon p = new List<Point>(ChainCodes.Count + 1);
            p.Add(new Point(StartX, StartY));
            if (ChainCodes.Count == 0)
                return p;

            int n = ChainCodes.Count, lastCode = ChainCodes[0];
            int x = StartX + chainCodeOffsets[lastCode][0];
            int y = StartY + chainCodeOffsets[lastCode][1];            
            for (int i = 1; i < n; i++)
            {
                if (lastCode != ChainCodes[i])
                // it means that the tracing contour has been changed direction
                {
                    p.Add(new Point(x, y));
                    lastCode = ChainCodes[i];
                }

                x += chainCodeOffsets[lastCode][0];
                y += chainCodeOffsets[lastCode][1];
            }

            return p;
        }

        public PolygonF ToPolygonF()
        {
            PolygonF p = new List<PointF>(ChainCodes.Count + 1);
            p.Add(new PointF(StartX, StartY));
            if (ChainCodes.Count == 0)
                return p;

            int n = ChainCodes.Count, lastCode = ChainCodes[0];
            int x = StartX + chainCodeOffsets[lastCode][0];
            int y = StartY + chainCodeOffsets[lastCode][1];
            for (int i = 1; i < n; i++)
            {
                if (lastCode != ChainCodes[i])
                // it means that the tracing contour has been changed direction
                {
                    p.Add(new PointF(x, y));
                    lastCode = ChainCodes[i];
                }

                x += chainCodeOffsets[lastCode][0];
                y += chainCodeOffsets[lastCode][1];
            }

            return p;
        }

        public void ExtractGeometricFeatures(Polygon p,
            ref double area, ref double perimeter, ref double circularity)
        {
            GeometricHelper.ExtractGeometricFeatures(
                p, ref area, ref perimeter, ref circularity);
        }

        public void ExtractGeometricFeatures(PolygonF p,
            ref double area, ref double perimeter, ref double circularity)
        {
            GeometricHelper.ExtractGeometricFeatures(
                p, ref area, ref perimeter, ref circularity);
        }
    }    

    public class ContourTracing
    {
        // Chain code:
        // 7 0 1
        // 6   2
        // 5 4 3
        private const int UP = 0;
        private const int UP_RIGHT = 1;
        private const int RIGHT = 2;
        private const int DOWN_RIGHT = 3;
        private const int DOWN = 4;
        private const int DOWN_LEFT = 5;
        private const int LEFT = 6;
        private const int UP_LEFT = 7;

        /// <summary>
        /// This is quick-lookup table for tracing on extra contour.
        /// </summary>
        private static int[][][] tracingExtra = new int[][][]
        {
            new int[][]{ new int[]{-1, -1, 3, UP_LEFT   }, new int[]{ 0, -1, 0, UP   }, new int[]{ 1, -1, 0, UP_RIGHT   } },
            new int[][]{ new int[]{ 1, -1, 0, UP_RIGHT  }, new int[]{ 1,  0, 1, RIGHT}, new int[]{ 1,  1, 1, DOWN_RIGHT } },
            new int[][]{ new int[]{ 1,  1, 1, DOWN_RIGHT}, new int[]{ 0,  1, 2, DOWN }, new int[]{-1,  1, 2, DOWN_LEFT  } },
            new int[][]{ new int[]{-1,  1, 2, DOWN_LEFT }, new int[]{-1,  0, 3, LEFT }, new int[]{-1, -1, 3, UP_LEFT    } }
	    };

        /// <summary>
        /// This is quick-lookup table for tracing on internal contour.
        /// </summary>
        private static int[][][] tracingInternal = new int[][][]
        { 
            new int[][]{ new int[]{ 1, -1, 3, UP_RIGHT   }, new int[]{ 0, -1, 0, UP   }, new int[]{-1, -1, 0, UP_LEFT    } },
            new int[][]{ new int[]{-1, -1, 0, UP_LEFT    }, new int[]{-1,  0, 1, LEFT }, new int[]{-1,  1, 1, DOWN_LEFT  } },
            new int[][]{ new int[]{-1,  1, 1, DOWN_LEFT  }, new int[]{ 0,  1, 2, DOWN }, new int[]{ 1,  1, 2, DOWN_RIGHT } },
            new int[][]{ new int[]{ 1,  1, 2, DOWN_RIGHT }, new int[]{ 1,  0, 3, RIGHT}, new int[]{ 1, -1, 3, UP_RIGHT   } }
        };        

        /// <summary>
        /// This is implementation of Labeling Algorithm.
        /// Labeling algorithm has varients, here based on 
        /// "A linear-time component-labeling algorithm using contour tracing technique" of
        /// Fu Chang, Chun-Jen Chen and Chi-Jen Lu.
        /// http://www.iis.sinica.edu.tw/papers/fchang/1362-F.pdf
        /// Abstract:
        /// A new linear time algorithm is presented in this article that 
        /// simultaneously la-bels connected components 
        /// (to be referred to merely as components in this paper) and 
        /// their contours in binary images. 
        /// The main step of this algorithm is to use a con-tour tracing technique 
        /// to detect the external contour and 
        /// possible internal contours of each component, 
        /// and also to identify and label the interior area of each component. 
        /// Labeling is done in a single pass over the image, 
        /// while contour points are revisited more than once, 
        /// but no more than a constant number of times. Moreover, 
        /// no re-labeling is required throughout the entire process, 
        /// as it is required by other algo-rithms. 
        /// Experimentation on various types of images 
        /// (characters, halftone pictures, photographs, newspaper, etc.) 
        /// shows that our method outperforms methods that use the equivalence technique. 
        /// Our algorithm not only labels components but also 
        /// ex-tracts component contours and sequential orders of contour points, 
        /// which can be use-ful for many applications.
        /// </summary>
        /// <param name="binImage"> processing binarized image. </param>
        /// <param name="width"> width-of processing image. </param>
        /// <param name="height"> height-of processing image. </param>
        /// <param name="labelImage"> out labeling image, which presents blobs. </param>
        /// <param name="blobs"> all blobs. </param>
        /// <importance>
        /// Please make sure that all assumptions below are claimed before passing data
        /// 1. Assumption 1: the first row (y=0) is empty (background)
        /// 2. Assumption 2: the first column (x=0) is empty (background)
        /// 3. Assumption 3: the last row (y=height-1) is empty (background)
        /// 4. Assumption 4: the last column (x=width-1) is empty (background)
        /// </importance>
        public static void PerformLabel(
            bool[] binImage, int width, int height,
            int[] labelImage, List<Blob> blobs)
        {
            unsafe
            {
                fixed (bool* pBinImage = binImage)
                fixed (int* pLabelImage = labelImage)
                {
                    PerformLabel(pBinImage, width, height, pLabelImage, blobs);
                }
            }
        }

        /// <summary>
        /// This is implementation of Labeling Algorithm.
        /// Labeling algorithm has varients, here based on 
        /// "A linear-time component-labeling algorithm using contour tracing technique" of
        /// Fu Chang, Chun-Jen Chen and Chi-Jen Lu.
        /// http://www.iis.sinica.edu.tw/papers/fchang/1362-F.pdf
        /// Abstract:
        /// A new linear time algorithm is presented in this article that 
        /// simultaneously la-bels connected components 
        /// (to be referred to merely as components in this paper) and 
        /// their contours in binary images. 
        /// The main step of this algorithm is to use a con-tour tracing technique 
        /// to detect the external contour and 
        /// possible internal contours of each component, 
        /// and also to identify and label the interior area of each component. 
        /// Labeling is done in a single pass over the image, 
        /// while contour points are revisited more than once, 
        /// but no more than a constant number of times. Moreover, 
        /// no re-labeling is required throughout the entire process, 
        /// as it is required by other algo-rithms. 
        /// Experimentation on various types of images 
        /// (characters, halftone pictures, photographs, newspaper, etc.) 
        /// shows that our method outperforms methods that use the equivalence technique. 
        /// Our algorithm not only labels components but also 
        /// ex-tracts component contours and sequential orders of contour points, 
        /// which can be use-ful for many applications.
        /// </summary>
        /// <param name="binImage"> processing binarized image. </param>
        /// <param name="width"> width-of processing image. </param>
        /// <param name="height"> height-of processing image. </param>
        /// <param name="labelImage"> out labeling image, which presents blobs. </param>
        /// <param name="blobs"> all blobs. </param>
        /// <importance>
        /// Please make sure that all assumptions below are claimed before passing data
        /// 1. Assumption 1: the first row (y=0) is empty (background)
        /// 2. Assumption 2: the first column (x=0) is empty (background)
        /// 3. Assumption 3: the last row (y=height-1) is empty (background)
        /// 4. Assumption 4: the last column (x=width-1) is empty (background)
        /// </importance>
        unsafe public static void PerformLabel(
            bool* binImage, int width, int height, int* labelImage, List<Blob> blobs)
        {
            #region Calc quick-lookup-offsets here
            int[][][] tracingExtraOffsets = new int[4][][];
            int[][][] tracingInternalOffsets = new int[4][][];
            for (int j = 0; j < 4; j++)
            {
                tracingExtraOffsets[j] = new int[3][];
                tracingInternalOffsets[j] = new int[3][];
                for (int k = 0; k < 3; k++)
                {
                    tracingExtraOffsets[j][k] = new int[] { 
                        tracingExtra[j][k][1] * width + tracingExtra[j][k][0],
                        tracingExtra[j][k][2], tracingExtra[j][k][3]
                    };

                    tracingInternalOffsets[j][k] = new int[] { 
                        tracingInternal[j][k][1] * width + tracingInternal[j][k][0],
                        tracingInternal[j][k][2], tracingInternal[j][k][3]
                    };
                }
            }
            #endregion Calc quick-lookup-offsets here

            int x, y, xEnd = width - 2, yEnd = height - 2, index = 0;
            bool labeled = false, endOfContour = false, found = false;
            int direction, iAttempt, i, tIndx, tIndx2;
            int label = 0, lastLabel = 0;
            Blob lastBlob = null, blob = null;
            Contour contour = null;
            ChainCodes chainCodes = null;

            for (y = 1; y <= yEnd; y++) // foreach row
            {
                // importance: assumed that the first/last row (y=0, y=height-1) is empty
                // fixed issue later

                // importance: assumed that the first/last column (x=0, x=width-1) is empty
                // fixed issue later

                // why I have to assume those?
                // They make me sure that I will not have to 
                // check out of bound of any scaning pixel in algorithm

                // go to pixel in which is begin of row-y
                index = y * width + 1;

                for (x = 1; x <= xEnd; x++, index++) // foreach pixel
                {
                    if (!binImage[index]) continue;

                    labeled = labelImage[index] > 0;

                    // if current-pixel is not labeled and up-pixel is background
                    if (labelImage[index] == 0 && !binImage[index - width])
                    {
                        labeled = true;

                        // new blob found
                        label++;

                        // update label-image
                        labelImage[index] = label;

                        // crate new blob
                        blob = new Blob(label, x, y);
                        contour = blob.ExtraContour; chainCodes = contour.ChainCodes;
                        blobs.Add(blob);

                        // update last blob
                        lastLabel = label;
                        lastBlob = blob;

                        // tracing extra contour
                        direction = 1; tIndx = index; endOfContour = false;                        
                        do
                        {
                            for (iAttempt = 0; iAttempt < 3; iAttempt++)
                            {
                                found = false;
                                for (i = 0; i < 3; i++)
                                {
                                    tIndx2 = tIndx + tracingExtraOffsets[direction][i][0];
                                    // here, i'm sure that tIndx2 is inside image
                                    if (binImage[tIndx2])
                                    {
                                        found = true;
                                        chainCodes.Add(tracingExtraOffsets[direction][i][2]);
                                        tIndx = tIndx2;
                                        direction = tracingExtraOffsets[direction][i][1];
                                        break;
                                    }
                                    else
                                    {
                                        labelImage[tIndx2] = int.MaxValue;
                                    }
                                }
                                if (!found)
                                    direction = (direction + 1) % 4;
                                else
                                {
                                    labelImage[tIndx] = label;
                                    break;
                                }

                                endOfContour = (index == tIndx && direction == 1);
                                if (endOfContour)
                                    break;
                            }
                        }
                        while (!endOfContour);
                    }

                    // if down-pixel is: background and not labeled
                    if (!binImage[index + width] && labelImage[index + width] == 0)
                    {
                        labeled = true;

                        if (labelImage[index] == 0)
                        {
                            label = labelImage[index - 1];
                            labelImage[index] = label;
                        }
                        else
                        {
                            label = labelImage[index];
                        }

                        if (label == lastLabel)
                        {
                            blob = lastBlob;
                        }
                        else
                        {
                            blob = blobs[label - 1];
                            lastLabel = label;
                            lastBlob = blob;
                        }

                        contour = new Contour(x, y); chainCodes = contour.ChainCodes;
                        // tracing internal contour
                        direction = 3; tIndx = index; endOfContour = false;                        
                        do
                        {
                            for (iAttempt = 0; iAttempt < 3; iAttempt++)
                            {
                                found = false;
                                for (i = 0; i < 3; i++)
                                {
                                    tIndx2 = tIndx + tracingInternalOffsets[direction][i][0];
                                    // here, i'm sure that tIndx2 is inside image
                                    if (binImage[tIndx2])
                                    {
                                        found = true;
                                        chainCodes.Add(tracingInternalOffsets[direction][i][2]);
                                        tIndx = tIndx2;
                                        direction = tracingInternalOffsets[direction][i][1];
                                        break;
                                    }
                                    else
                                    {
                                        labelImage[tIndx2] = int.MaxValue;
                                    }
                                }
                                if (!found)
                                    direction = (direction + 1) % 4;
                                else
                                {
                                    if (labelImage[tIndx] == 0)
                                    {
                                        labelImage[tIndx] = label;
                                    }

                                    break;
                                }
                            }
                        }
                        while (tIndx != index);

                        // add internal contour
                        blob.InternalContours.Add(contour);
                    }

                    // if current-pixel is not labeled
                    if (!labeled)
                    {
                        label = labelImage[index - 1];
                        labelImage[index] = label;
                        if (label != lastLabel)
                        {
                            blob = blobs[label - 1];
                            lastLabel = label;
                            lastBlob = blob;
                        }
                    }
                }
            }
        }
    }

    public class GeometricHelper
    {
        private static double[][] fastPt2PtDistances =
            new double[][]
            {
                new double[] {Math.Sqrt(2.0), 1, Math.Sqrt(2.0)},
                new double[] {1, 0, 1},
                new double[] {Math.Sqrt(2.0), 1, Math.Sqrt(2.0)}
            };

        #region Polygon
        public static void ExtractGeometricFeatures(Polygon p,
            ref double area, ref double perimeter, ref double circularity)
        {
            area = 1.0; perimeter = 1.0;
            if (p.Count == 2)
            {
                perimeter = Math.Sqrt(
                    (p[0].X - p[1].X) * (p[0].X - p[1].X) +
                    (p[0].Y - p[1].Y) * (p[0].Y - p[1].Y));
            }
            else if (p.Count > 2)
            {
                area = 0; perimeter = 0;
                int nPts = p.Count;
                Point lastPt = p[nPts - 1]; // tail-of-circle-ring
                for (int i = 0; i < nPts - 1; i++)
                {
                    area += lastPt.X * p[i].Y - lastPt.Y * p[i].X;
                    lastPt = p[i];

                    perimeter +=
                        fastPt2PtDistances[p[i].X - p[i + 1].X + 1][p[i].Y - p[i + 1].Y];
                }

                area += lastPt.X * p[nPts - 1].Y - lastPt.Y * p[nPts - 1].X;
                area = area * 0.5;
            }

            // circularity is similar/alias compactness
            circularity = (perimeter * perimeter / area) - 4.0 * Math.PI;
            circularity = Math.Max(0, circularity);
        }

        private static void SimplifyPolygon(
            Polygon p, int i1, int i2, bool[] pnUseFlag, double delta)
        {
            int endIndex = (i2 < 0) ? p.Count : i2;

            if (Math.Abs(i1 - endIndex) <= 1)
                return;

            Point firstPoint = p[i1];
            Point lastPoint = (i2 < 0) ? p[0] : p[i2];

            double furtherDistance = 0.0, d;
            int furtherIndex = 0;

            for (int i = i1 + 1; i < endIndex; i++)
            {
                d = DistanceLinePoint(firstPoint, lastPoint, p[i], true);

                if ((d >= delta) && (d > furtherDistance))
                {
                    furtherDistance = d;
                    furtherIndex = i;
                }
            }

            if (furtherIndex > 0)
            {
                pnUseFlag[furtherIndex] = true;

                SimplifyPolygon(p, i1, furtherIndex, pnUseFlag, delta);
                SimplifyPolygon(p, furtherIndex, i2, pnUseFlag, delta);
            }
        }

        public static Polygon SimplifyPolygon(Polygon p, double delta)
        {
            Polygon newp = null;
            try
            {
                double x = p[0].X, y = p[0].Y, furtherDistance = 0.0, d2;
                int n = p.Count, furtherIndex = 0;
                for (int i = 1; i < n; i++)
                {
                    d2 = (x - p[i].X) * (x - p[i].X) + (y - p[i].Y) * (y - p[i].Y);
                    if (d2 > furtherDistance)
                    {
                        furtherDistance = d2;
                        furtherIndex = i;
                    }
                }
                furtherDistance = Math.Sqrt(furtherDistance);

                if (furtherDistance < delta)
                {
                    newp = new Polygon(1); newp.Add(p[0]);
                }
                else
                {
                    bool[] pnUseFlag = new bool[n]; pnUseFlag[0] = true;
                    SimplifyPolygon(p, 0, furtherIndex, pnUseFlag, delta);
                    SimplifyPolygon(p, furtherIndex, -1, pnUseFlag, delta);

                    newp = new Polygon(p.Count);
                    for (int i = 0; i < n; i++)
                    {
                        if (!pnUseFlag[i]) continue;

                        newp.Add(p[i]);
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
            }

            return newp;
        }
        #endregion Polygon

        #region PolygonF
        public static void ExtractGeometricFeatures(PolygonF p,
            ref double area, ref double perimeter, ref double circularity)
        {
            area = 1.0; perimeter = 1.0;
            if (p.Count == 2)
            {
                perimeter = Math.Sqrt(
                    (p[0].X - p[1].X) * (p[0].X - p[1].X) +
                    (p[0].Y - p[1].Y) * (p[0].Y - p[1].Y));
            }
            else if (p.Count > 2)
            {
                area = 0; perimeter = 0;
                int nPts = p.Count;
                PointF lastPt = p[nPts - 1]; // tail-of-circle-ring
                for (int i = 0; i < nPts - 1; i++)
                {
                    area += lastPt.X * p[i].Y - lastPt.Y * p[i].X;
                    lastPt = p[i];

                    perimeter +=
                        //fastPt2PtDistances[(int)(p[i].X - p[i + 1].X + 1)][(int)(p[i].Y - p[i + 1].Y)];
                        fastPt2PtDistances[(int)Math.Round(p[i].X - p[i + 1].X + 1)][(int)Math.Round(p[i].Y - p[i + 1].Y)];
                }

                area += lastPt.X * p[nPts - 1].Y - lastPt.Y * p[nPts - 1].X;
                area = area * 0.5;
            }

            // circularity is similar/alias compactness
            circularity = (perimeter * perimeter / area) - 4.0 * Math.PI;
            circularity = Math.Max(0, circularity);
        }

        private static void SimplifyPolygon(
            PolygonF p, int i1, int i2, bool[] pnUseFlag, double delta)
        {
            int endIndex = (i2 < 0) ? p.Count : i2;

            if (Math.Abs(i1 - endIndex) <= 1)
                return;

            PointF firstPoint = p[i1];
            PointF lastPoint = (i2 < 0) ? p[0] : p[i2];

            double furtherDistance = 0.0, d;
            int furtherIndex = 0;

            for (int i = i1 + 1; i < endIndex; i++)
            {
                d = DistanceLinePoint(firstPoint, lastPoint, p[i], true);

                if ((d >= delta) && (d > furtherDistance))
                {
                    furtherDistance = d;
                    furtherIndex = i;
                }
            }

            if (furtherIndex > 0)
            {
                pnUseFlag[furtherIndex] = true;

                SimplifyPolygon(p, i1, furtherIndex, pnUseFlag, delta);
                SimplifyPolygon(p, furtherIndex, i2, pnUseFlag, delta);
            }
        }

        public static PolygonF SimplifyPolygon(PolygonF p, double delta)
        {
            PolygonF newp = null;
            try
            {
                double x = p[0].X, y = p[0].Y, furtherDistance = 0.0, d2;
                int n = p.Count, furtherIndex = 0;
                for (int i = 1; i < n; i++)
                {
                    d2 = (x - p[i].X) * (x - p[i].X) + (y - p[i].Y) * (y - p[i].Y);
                    if (d2 > furtherDistance)
                    {
                        furtherDistance = d2;
                        furtherIndex = i;
                    }
                }
                furtherDistance = Math.Sqrt(furtherDistance);

                if (furtherDistance < delta)
                {
                    newp = new PolygonF(1); newp.Add(p[0]);
                }
                else
                {
                    bool[] pnUseFlag = new bool[n]; pnUseFlag[0] = true;
                    SimplifyPolygon(p, 0, furtherIndex, pnUseFlag, delta);
                    SimplifyPolygon(p, furtherIndex, -1, pnUseFlag, delta);

                    newp = new PolygonF(p.Count);
                    for (int i = 0; i < n; i++)
                    {
                        if (!pnUseFlag[i]) continue;

                        newp.Add(p[i]);
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
            }

            return newp;
        }
        #endregion PolygonF

        #region Point-Point-Distances
        public static double DotProductPoints(Point a, Point b, Point c)
	    {
		    double abx = b.X-a.X;
		    double aby = b.Y-a.Y;
		    double bcx = c.X-b.X;
		    double bcy = c.Y-b.Y;

		    return abx*bcx + aby*bcy;
	    }

        public static double CrossProductPoints(Point a, Point b, Point c)
	    {
		    double abx = b.X-a.X;
		    double aby = b.Y-a.Y;
		    double acx = c.X-a.X;
		    double acy = c.Y-a.Y;

		    return abx*acy - aby*acx;
	    }

        public static double DistancePointPoint(Point a, Point b)
	    {
		    double abx = a.X-b.X;
		    double aby = a.Y-b.Y;

		    return Math.Sqrt(abx*abx + aby*aby);
	    }

        public static double DistanceLinePoint(Point a, Point b, Point c, bool isSegment)
	    {
		    if (isSegment)
		    {
			    double dot1 = DotProductPoints(a, b, c);
			    if (dot1 > 0) return DistancePointPoint(b, c);

			    double dot2 = DotProductPoints(b, a, c);
			    if(dot2 > 0) return DistancePointPoint(a, c);
		    }

		    return Math.Abs(CrossProductPoints(a,b,c) / DistancePointPoint(a,b));
        }
        #endregion Point-Point-Distances

        #region PointF-PointF-Distances
        public static double DotProductPoints(PointF a, PointF b, PointF c)
        {
            double abx = b.X - a.X;
            double aby = b.Y - a.Y;
            double bcx = c.X - b.X;
            double bcy = c.Y - b.Y;

            return abx * bcx + aby * bcy;
        }

        public static double CrossProductPoints(PointF a, PointF b, PointF c)
        {
            double abx = b.X - a.X;
            double aby = b.Y - a.Y;
            double acx = c.X - a.X;
            double acy = c.Y - a.Y;

            return abx * acy - aby * acx;
        }

        public static double DistancePointPoint(PointF a, PointF b)
        {
            double abx = a.X - b.X;
            double aby = a.Y - b.Y;

            return Math.Sqrt(abx * abx + aby * aby);
        }

        public static double DistanceLinePoint(PointF a, PointF b, PointF c, bool isSegment)
        {
            if (isSegment)
            {
                double dot1 = DotProductPoints(a, b, c);
                if (dot1 > 0) return DistancePointPoint(b, c);

                double dot2 = DotProductPoints(b, a, c);
                if (dot2 > 0) return DistancePointPoint(a, c);
            }

            return Math.Abs(CrossProductPoints(a, b, c) / DistancePointPoint(a, b));
        }
        #endregion PointF-PointF-Distances
    }
}
