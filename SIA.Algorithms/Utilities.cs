using System;
using System.Collections.Generic;
using System.Text;
using SIA.IPEngine;
using System.IO;
using System.Drawing;

namespace SIA.Algorithms
{
    using Polygon = List<Point>;
    using PolygonF = List<PointF>;

    public class Utilities
    {
        unsafe public static void CalcImageIntegral(GreyDataImage image, out double[] integral)
        {
            ushort* data = image._aData;
            int width = image._width;
            int height = image._height;
            integral = new double[width * height];
            fixed (double* pIntegral = integral)
            {
                Utilities.CalcImageIntegral(data, width, height, pIntegral);
            }
        }

        unsafe public static void CalcImageIntegral(
            ushort* pSrc, int width, int height, double* pDst)
        {
            double max_intensity = 255;

            // get data's length
            int length = width * height;
            double inv_max_intensity = 1.0 / max_intensity;

            // create integral image buffer

            // temporate variables
            int x, y, index;
            double intensity = 0;

            // calculate integral image
            {
                // first item
                // normalize intensity
                intensity = pSrc[0] * inv_max_intensity;
                pDst[0] = intensity;

                // first line
                for (index = 1; index < width; index++)
                {
                    // normalize intensity
                    intensity = pSrc[index] * inv_max_intensity;

                    // calc integral
                    pDst[index] = pDst[index - 1] + intensity;
                }

                // first column
                for (index = width, y = 1; y < height; y++, index += width)
                {
                    // normalize intensity
                    intensity = pSrc[index] * inv_max_intensity;

                    // calc integral
                    pDst[index] = pDst[index - width] + intensity;
                }

                // remains
                int x_1y_1 = -width - 1;
                int xy_1 = -width;
                int x_1y = -1;
                for (index = width, y = 1; y < height; y++)
                {
                    index += 1;
                    for (x = 1; x < width; x++, index++)
                    {
                        // normalize intensity
                        intensity = pSrc[index] * inv_max_intensity;

                        // calc integral
                        pDst[index] =
                            pDst[index + x_1y] + pDst[index + xy_1] -
                            pDst[index + x_1y_1] + intensity;
                    }
                }
            }
        }

        unsafe public static void Copy(ushort[] src, ushort* dst)
        {
            int l = src.Length;
            for (int i = 0; i < l; i++)
                dst[i] = src[i];
        }

        unsafe public static void Copy(ushort* src, ushort[] dst)
        {
            int l = dst.Length;
            for (int i = 0; i < l; i++)
                dst[i] = src[i];
        }

        unsafe public static ushort[] UnsafePointerToArray(ushort* src, int length)
        {
            ushort[] a = new ushort[length];

            for (int i = 0; i < length; i++)
            {
                a[i] = src[i];
            }

            return a;
        }

        public static int[] Segments(int[] a, int n)
        {
            List<int> segments = new List<int>(1000);

            int end = n;
            int run = 0;
            int v = a[0];
            while (run < end)
            {
                if (a[run] == v)
                {
                    // increase index
                    run++;
                    continue;
                }
                // add new end of contour
                segments.Add(run - 1);

                // reset new blk index value
                v = a[run];

                // increase index
                run++;
            }

            // add new end of contour
            segments.Add(end - 1);

            return segments.ToArray();
        }

        public static Rectangle GetBound(Rectangle[] bounds)
        {
            if (bounds == null || bounds.Length == 0)
                return Rectangle.Empty;

            int l = int.MaxValue;
            int t = int.MaxValue;
            int r = int.MinValue;
            int b = int.MinValue;

            foreach (Rectangle bound in bounds)
            {
                l = Math.Min(l, bound.Left);
                t = Math.Min(t, bound.Top);
                r = Math.Max(r, bound.Right);
                b = Math.Max(b, bound.Bottom);
            }

            return new Rectangle(l, t, r - l + 1, b - t + 1);
        }
    }

    public class ParallelHelper
    {
        public static void ManageThreadingSpace(
            int from, int to,
            ref int nThreads, ref int[] starts, ref int[] ends)
        {
            nThreads = System.Environment.ProcessorCount;
            starts = new int[nThreads];
            ends = new int[nThreads];

            int amount = to - from + 1;
            int amountPerThread = amount / nThreads;
            for (int iThread = 0; iThread < nThreads; iThread++)
            {
                starts[iThread] = iThread * amountPerThread + from;
                ends[iThread] = starts[iThread] + amountPerThread - 1;
            }
            ends[nThreads - 1] = to - 1;
        }

        public static Region[] CreateInstances(Region src, int n)
        {
            Region[] regions = new Region[n];

            for (int i = 0; i < n; i++)
            {
                regions[i] = src.Clone();
            }

            return regions;
        }

        public static void DisposeObject(IDisposable disposableObj)
        {
            if (disposableObj != null)
            {
                disposableObj.Dispose();
            }
        }

        public static void DisposeAll(Array a)
        {
            try
            {
                foreach (IDisposable disposableObj in a)
                    DisposeObject(disposableObj);
            }
            catch
            {
            }
        }
    }

    public class GreyImageWrapper
    {
        public static GreyDataImage FromRawBytes(byte[] bytes)
        {
            GreyDataImage image = null;

            try
            {
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    using (BinaryReader reader = new BinaryReader(ms))
                    {
                        image = new GreyDataImage();
                        image.QuickLoad(reader);
                    }
                }
            }
            catch (System.Exception exp)
            {
                if (image != null)
                {
                    image.Dispose();
                    image = null;
                }

                throw exp;
            }
            finally
            {
            }

            return image;
        }

        public static byte[] ToRawBytes(GreyDataImage image)
        {
            byte[] bytes = null;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (BinaryWriter writer = new BinaryWriter(ms))
                    {
                        image.QuickSave(writer);
                    }

                    bytes = ms.ToArray();
                }
            }
            catch (System.Exception exp)
            {
                bytes = null;

                throw exp;
            }
            finally
            {
            }

            return bytes;
        }
    }    
}
