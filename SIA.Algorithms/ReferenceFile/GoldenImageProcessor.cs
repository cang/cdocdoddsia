#define DEBUG_METETIME_

/* for optimize
GoldenImageProcessor : SpeciallySubtract : Preprocess: 13
GoldenImageProcessor : SpeciallySubtract : cbGolden: 1
GoldenImageProcessor : SpeciallySubtract : cbSample: 0
GoldenImageProcessor : SpeciallySubtract : cbOrigin: 1
GoldenImageProcessor : SpeciallySubtract : cbPixel: 122
GoldenImageProcessor : SpeciallySubtract : cbkItemOffsets: 11
GoldenImageProcessor : SpeciallySubtract : kernelSpeciallySubtractPixel: 23349
GoldenImageProcessor : SpeciallySubtract : kernelSpeciallySubtractGetData: 7576
GoldenImageProcessor : SpeciallySubtract : Read: 18680
GoldenImageProcessor : SpeciallySubtract : Remain: 3035
GoldenImageProcessor : SpeciallySubtract : Free Device Buffer: 193
Totals :52981
*/ 

using System;
using System.Collections.Generic;
using System.Text;

using SIA.IPEngine;
using System.Drawing;
using SIA.SystemFrameworks;
using SIA.SystemLayer;
using System.Drawing.Drawing2D;
using Cloo;
using SiGlaz.Cloo;
using System.Collections;

namespace SIA.Algorithms.ReferenceFile
{
    unsafe public class GoldenImageProcessor
    {
        private int _dockRightSize = 57;

        private int _marginLeft = 5;
        private int _marginTop = 23;
        private int _marginRight = 3;
        private int _marginBottom = 23;

        private int _kernelWidth = 5;
        private int _kernelHeight = 5;

        private int[] _kItemOffsets = null;

        private int _imageWidth = 0;
        private int _imageHeight = 0;

        private Region _regions = null;
        public Region Regions
        {
            get { return _regions; }
            set { _regions = value; }
        }


        public static double DarkIntensity = 75.5;
        public static double BrightIntensity = 179.5;


        private GreyDataImage Subract(
            GreyDataImage goldenImage, GreyDataImage sample)
        {
            _imageWidth = goldenImage.Width;
            _imageHeight = goldenImage.Height;

            GreyDataImage output =
                new GreyDataImage(_imageWidth, _imageHeight);

            Subtract(
                goldenImage._aData, sample._aData,
                output._aData, _imageWidth, _imageHeight);

            return output;
        }

        public static void Subtract(
            ushort* golden, ushort* sample, int width, int height)
        {
            Subtract(golden, sample, sample, width, height);
        }

        private static int[] CalcSpecialItemOffsets(
            int imageWidth, int kernelWidth, int kernelHeight)
        {
            int[] xItems = new int[] {
                0, 
                -1, 0, 1, 
                -2, -1, 0, 1, 2, 
                -1, 0, 1,
                0
            };
            int[] yItems = new int[] {
                -2, 
                -1, -1, -1, 
                0, 0, 0, 0, 0, 
                1, 1, 1, 
                2 };

            //CreateItemOffsets(kernelWidth, kernelHeight, ref xItems, ref yItems);
            CreatePriorityRingItemOffsets(kernelWidth, kernelHeight, ref xItems, ref yItems);

            return CalcItemOffsets(imageWidth, xItems, yItems);
        }

        private static void CreateItemOffsets(
            int kernelWidth, int kernelHeight, ref int[] xItems, ref int[] yItems)
        {
            int halfWidth = kernelWidth >> 1;
            int halfHeight = kernelHeight >> 1;

            int n = kernelWidth * kernelHeight;
            xItems = new int[n];
            yItems = new int[n];

            int index = 0;
            for (int y = -halfHeight; y <= halfHeight; y++)
            {
                for (int x = -halfWidth; x <= halfWidth; x++, index++)
                {
                    xItems[index] = x;
                    yItems[index] = y;
                }
            }
        }

        private static void CreatePriorityRingItemOffsets(
            int kernelWidth, int kernelHeight, ref int[] xItems, ref int[] yItems)
        {
            int halfWidth = kernelWidth >> 1;
            int halfHeight = kernelHeight >> 1;

            int n = kernelWidth * kernelHeight;
            xItems = new int[n];
            yItems = new int[n];

            int index = 1;
            for (int ring = 1; ring <= halfHeight; ring++)
            {
                CreateRingItemOffsets(ring, ref xItems, ref yItems, ref index);
            }
        }

        private static void CreateRingItemOffsets(
            int ring, ref int[] xItems, ref int[] yItems, ref int index)
        {
            // top-line
            CreateItemOffsets(-ring, -ring, 1, 0, ring, -ring, ref xItems, ref yItems, ref index);
            // bottom-line
            CreateItemOffsets(-ring, ring, 1, 0, ring, ring, ref xItems, ref yItems, ref index);
            // left-line
            CreateItemOffsets(-ring, -(ring - 1), 0, 1, -ring, ring - 1, ref xItems, ref yItems, ref index);
            // right-line
            CreateItemOffsets(ring, -(ring - 1), 0, 1, ring, ring - 1, ref xItems, ref yItems, ref index);
        }

        private static void CreateItemOffsets(
            int x, int y, int ex, int ey, int maxX, int maxY,
            ref int[] xItems, ref int[] yItems, ref int index)
        {
            ex = 1; ey = 1;
            for (int iy = y; iy <= maxY; iy += ey)
            {
                for (int ix = x; ix <= maxX; ix += ex)
                {
                    xItems[index] = ix;
                    yItems[index] = iy;
                    index++;
                }
            }
        }

        private static int[] CalcItemOffsets(
            int imageWidth, int[] xItems, int[] yItems)
        {
            int n = xItems.Length;
            int[] kItemOffsets = new int[n];
            for (int i = 0; i < n; i++)
            {
                int offset = yItems[i] * imageWidth + xItems[i];
                kItemOffsets[i] = offset;
            }

            return kItemOffsets;
        }

        private const int linesPerThread = 10;
        public static void Subtract(
            ushort* golden, ushort* sample, ushort* output, int width, int height)
        {
            Parallel.For(0, height, delegate(int y)
            {
                int index = y * width;
                int v = 0;
                for (int x = 0; x < width; x++, index++)
                {
                    v = 128 + sample[index] - golden[index];
                    if (v < 0)
                        output[index] = 0;
                    else if (v > 255)
                        output[index] = 255;
                    else
                        output[index] = (ushort)v;
                }
            });
        }


        /**
         * All using functions....
         * 
         * **/
        public void SetKernelSize(int kernelWidth, int kernelHeight)
        {
            _kernelWidth = kernelWidth;
            _kernelHeight = kernelHeight;
        }


        public void Subract(
            GreyDataImage goldenImage, ref GreyDataImage sample)
        {
            _imageWidth = goldenImage.Width;
            _imageHeight = goldenImage.Height;

            //Subtract(
            //    goldenImage._aData, sample._aData, _imageWidth, _imageHeight);

#if RELEASE

            throw new System.Exception("Testing Pole-tip...");

            _kernelWidth = 7;
            _kernelHeight = 7;
#endif

            SpeciallySubtract(goldenImage.
                _aData, sample._aData, sample._aData,
                _imageWidth, _imageHeight, _kernelWidth, _kernelHeight);
        }

        public static void SpeciallySubtract(
            ushort* golden,
            ushort* sample, ushort* output,
            int width, int height, int kernelWidth, int kernelHeight)
        {
            int[] kItemOffsets =
                CalcSpecialItemOffsets(width, kernelWidth, kernelHeight);
            int nItemOffsets = kItemOffsets.Length;

            int marginX = kernelWidth >> 1;
            int marginY = kernelHeight >> 1;
            int l = marginX;
            int t = marginY;
            int r = width - marginX - 1;
            int b = height - marginY - 1;

            Parallel.For(t, b + 1, delegate(int y)
            {
                int index = y * width + l;
                int v = 0, absv = 0;
                int min_absv = int.MaxValue;
                int min_v = 0;
                for (int x = l; x <= r; x++, index++)
                {
                    min_absv = int.MaxValue;
                    min_v = 0;
                    for (int itemIndx = 0; itemIndx < nItemOffsets; itemIndx++)
                    {
                        v = sample[index] - golden[index + kItemOffsets[itemIndx]];
                        absv = Math.Abs(v);

                        if (absv < min_absv)
                        {
                            min_absv = absv;
                            min_v = v;
                        }
                    }

                    v = min_v;

                    v += 128;

                    if (v < 0)
                        output[index] = 0;
                    else if (v > 255)
                        output[index] = 255;
                    else
                        output[index] = (ushort)v;
                }
            });
        }

        public static void SpeciallySubtract(
            ushort* golden,
            ushort* sample, ushort* originData,
            bool* output,
            int width, int height, int kernelWidth, int kernelHeight,
            int marginLeft, int marginTop, int marginRight, int marginBottom,
            double darkThreshold, double brightThreshold)
        {
            int[] kItemOffsets =
                CalcSpecialItemOffsets(width, kernelWidth, kernelHeight);
            int nItemOffsets = kItemOffsets.Length;

            int marginX = kernelWidth >> 1;
            int marginY = kernelHeight >> 1;

            marginLeft = Math.Max(marginX, marginLeft);
            marginRight = Math.Max(marginX, marginRight);
            marginTop = Math.Max(marginY, marginTop);
            marginBottom = Math.Max(marginY, marginBottom);

            int l = marginLeft;
            int t = marginTop;
            int r = width - marginRight - 1;
            int b = height - marginBottom - 1;

            double brokenThreshold = 128 - darkThreshold;
            brokenThreshold = Math.Min(brokenThreshold, brightThreshold - 128);

            Parallel.For(t, b + 1, delegate(int y)
            {
                int index = y * width + l;
                int v = 0, absv = 0;
                int min_absv = int.MaxValue;
                int min_v = 0;
                bool canBreak = false;

                for (int x = l; x <= r; x++, index++)
                {
                    min_v = sample[index] - golden[index + kItemOffsets[0]];
                    min_absv = Math.Abs(min_v);

                    if (min_absv < brokenThreshold)
                        continue;

                    canBreak = false;

                    for (int itemIndx = 1; itemIndx < nItemOffsets; itemIndx++)
                    {
                        v = sample[index] - golden[index + kItemOffsets[itemIndx]];
                        absv = Math.Abs(v);

                        if (absv < min_absv)
                        {
                            if (absv < brokenThreshold)
                            {
                                canBreak = true;
                                break;
                            }

                            min_absv = absv;
                            min_v = v;
                        }
                    }

                    if (canBreak)
                        continue;

                    v = min_v + 128;
                    if (v < darkThreshold)
                    {
                        if (originData[index] < DarkIntensity)
                        {
                            output[index] = true;
                        }
                    }
                    else if (v > brightThreshold)
                    {
                        if (originData[index] > BrightIntensity)
                        {
                            output[index] = true;
                        }
                    }
                }
                //}
            });
        }

        public BinaryImage Binarize(
            GreyDataImage goldenImage, 
            GreyDataImage filteredImage, GreyDataImage originImage,
            double darkThreshold, double brightThreshold)
        {
            BinaryImage binImage = null;

            _imageWidth = goldenImage.Width;
            _imageHeight = goldenImage.Height;

            binImage =
                new BinaryImage(_imageWidth, _imageHeight);
            bool* buffer = (bool*)binImage.Buffer.ToPointer();

            _marginLeft = Math.Max(_marginLeft, _kernelWidth >> 1);
            _marginRight = Math.Max(_marginRight, _kernelWidth >> 1);
            _marginTop = Math.Max(_marginTop, _kernelHeight >> 1);
            _marginBottom = Math.Max(_marginBottom, _kernelHeight >> 1);

            int marginLeft = _marginLeft;
            int marginRight = _marginRight + _dockRightSize;
            int marginTop = _marginTop;
            int marginBottom = _marginBottom;

            SpeciallySubtract(
                goldenImage._aData, 
                filteredImage._aData, originImage._aData,
                buffer, _imageWidth, _imageHeight,
                _kernelWidth, _kernelHeight,
                marginLeft, marginTop, marginRight, marginBottom,
                darkThreshold, brightThreshold);

            return binImage;
        }

        public static bool ValidateImageSize(
            string[] sampleImages, ref string msg)
        {
            try
            {
                int w = 0;
                int h = 0;
                using (Image image = Image.FromFile(sampleImages[0]))
                {
                    w = image.Width;
                    h = image.Height;
                }

                for (int i = 1; i < sampleImages.Length; i++)
                {
                    using (Image image = Image.FromFile(sampleImages[i]))
                    {
                        if (w != image.Width ||
                            h != image.Height)
                        {
                            msg = "Image(s) are not the same size!";
                            return false;
                        }
                    }
                }
            }
            catch (System.Exception exp)
            {
                msg = exp.Message;
                return false;
            }
            finally
            {
                GC.Collect();
            }

            return true;
        }

        public static bool ValidateImageSize(
            string imageFile, int width, int height, ref string msg)
        {
            try
            {
                using (Image image = Image.FromFile(imageFile))
                {
                    if (image.Width != width ||
                        image.Height != height)
                    {
                        msg = "Image is not the same size!";
                        return false;
                    }
                }
            }
            catch (System.Exception exp)
            {
                msg = exp.Message;
                return false;
            }

            return true;
        }

        public int[] GetInterestedPixels(
            GreyDataImage goldenImage,
            GreyDataImage filteredImage, GreyDataImage originImage,
            double darkThreshold, double brightThreshold)
        {            
            _imageWidth = goldenImage.Width;
            _imageHeight = goldenImage.Height;

            int[] interestedPixels = null;

            _marginLeft = Math.Max(_marginLeft, _kernelWidth >> 1);
            _marginRight = Math.Max(_marginRight, _kernelWidth >> 1);
            _marginTop = Math.Max(_marginTop, _kernelHeight >> 1);
            _marginBottom = Math.Max(_marginBottom, _kernelHeight >> 1);

            int marginLeft = _marginLeft;
            int marginRight = _marginRight + _dockRightSize;
            int marginTop = _marginTop;
            int marginBottom = _marginBottom;

            if (_regions == null)
            {
                SpeciallySubtract(
                    goldenImage._aData,
                    filteredImage._aData, originImage._aData,
                    ref interestedPixels,
                    _imageWidth, _imageHeight,
                    _kernelWidth, _kernelHeight,
                    marginLeft, marginTop, marginRight, marginBottom,
                    darkThreshold, brightThreshold);
            }
            else
            {
                SpeciallySubtract(
                    goldenImage._aData,
                    filteredImage._aData, originImage._aData,
                    ref interestedPixels,
                    _imageWidth, _imageHeight,
                    _kernelWidth, _kernelHeight,
                    marginLeft, marginTop, marginRight, marginBottom,
                    darkThreshold, brightThreshold, _regions);
            }

            return interestedPixels;
        }

        public static void SpeciallySubtract(
            ushort* golden,
            ushort* sample, ushort* originData,
            ref int[] interestedPixels,
            int width, int height, int kernelWidth, int kernelHeight,
            int marginLeft, int marginTop, int marginRight, int marginBottom,
            double darkThreshold, double brightThreshold)
        {
            int[] kItemOffsets =
                CalcSpecialItemOffsets(width, kernelWidth, kernelHeight);
            int nItemOffsets = kItemOffsets.Length;

            int marginX = kernelWidth >> 1;
            int marginY = kernelHeight >> 1;

            marginLeft = Math.Max(marginX, marginLeft);
            marginRight = Math.Max(marginX, marginRight);
            marginTop = Math.Max(marginY, marginTop);
            marginBottom = Math.Max(marginY, marginBottom);

            int l = marginLeft;
            int t = marginTop;
            int r = width - marginRight - 1;
            int b = height - marginBottom - 1;

            double brokenThreshold = 128 - darkThreshold;
            brokenThreshold = Math.Min(brokenThreshold, brightThreshold - 128);

            int[] pixels = new int[width * height];
            int[] counter = new int[height];
            Parallel.For(t, b + 1, delegate(int y)
            {
                int index = y * width + l;
                int v = 0, absv = 0;
                int min_absv = int.MaxValue;
                int min_v = 0;
                bool canBreak = false;

                int interestedLineOffset = y * width;
                
                for (int x = l; x <= r; x++, index++)
                {
                    min_v = sample[index] - golden[index + kItemOffsets[0]];
                    min_absv = Math.Abs(min_v);

                    if (min_absv < brokenThreshold)
                        continue;

                    canBreak = false;

                    for (int itemIndx = 1; itemIndx < nItemOffsets; itemIndx++)
                    {
                        v = sample[index] - golden[index + kItemOffsets[itemIndx]];
                        absv = Math.Abs(v);

                        if (absv < min_absv)
                        {
                            if (absv < brokenThreshold)
                            {
                                canBreak = true;
                                break;
                            }

                            min_absv = absv;
                            min_v = v;
                        }
                    }

                    if (canBreak)
                        continue;

                    v = min_v + 128;
                    if (v < darkThreshold)
                    {
                        if (originData[index] < DarkIntensity)
                        {
                            pixels[interestedLineOffset + (counter[y]++)] = index;
                        }
                    }
                    else if (v > brightThreshold)
                    {
                        if (originData[index] > BrightIntensity)
                        {
                            pixels[interestedLineOffset + (counter[y]++)] = index;
                        }
                    }
                }
                //}
            }
            );

            #region Update interested pixels here
            int interestedCount = 0;
            for (int y = 0; y < height; y++)
                interestedCount += counter[y];
            interestedPixels = new int[interestedCount];
            int count = 0;
            for (int y = 0; y < height; y++)
            {
                if (counter[y] <= 0)
                    continue;

                Array.Copy(pixels, y * width, interestedPixels, count, counter[y]);
                count += counter[y];
            }
            #endregion Update interested pixels here
        }

        public static void SpeciallySubtract(
            ushort* golden,
            ushort* sample, ushort* originData,
            ref int[] interestedPixels,
            int width, int height, int kernelWidth, int kernelHeight,
            int marginLeft, int marginTop, int marginRight, int marginBottom,
            double darkThreshold, double brightThreshold, Region regions)
        {
            int[] kItemOffsets =
                CalcSpecialItemOffsets(width, kernelWidth, kernelHeight);
            int nItemOffsets = kItemOffsets.Length;

            int marginX = kernelWidth >> 1;
            int marginY = kernelHeight >> 1;

            marginLeft = Math.Max(marginX, marginLeft);
            marginRight = Math.Max(marginX, marginRight);
            marginTop = Math.Max(marginY, marginTop);
            marginBottom = Math.Max(marginY, marginBottom);

            int l = marginLeft;
            int t = marginTop;
            int r = width - marginRight - 1;
            int b = height - marginBottom - 1;

            double brokenThreshold = 128 - darkThreshold;
            brokenThreshold = Math.Min(brokenThreshold, brightThreshold - 128);

            int[] pixels = new int[width * height];
            int[] counter = new int[height];

            int nThreads = 0;
            int[] starts = null, ends = null;
            ParallelHelper.ManageThreadingSpace(t, b, ref nThreads, ref starts, ref ends);
            Region[] clonedRegions = ParallelHelper.CreateInstances(regions, nThreads);
            Parallel.For(0, nThreads, delegate(int iThread)
            {
                int yStart = starts[iThread];
                int yEnd = ends[iThread];
                Region region = clonedRegions[iThread];
                for (int y = yStart; y <= yEnd; y++)
                {
                    int index = y * width + l;
                    int v = 0, absv = 0;
                    int min_absv = int.MaxValue;
                    int min_v = 0;
                    bool canBreak = false;

                    int interestedLineOffset = y * width;

                    for (int x = l; x <= r; x++, index++)
                    {
                        min_v = sample[index] - golden[index + kItemOffsets[0]];
                        min_absv = Math.Abs(min_v);

                        if (min_absv < brokenThreshold)
                            continue;

                        if (!region.IsVisible(x, y))
                            continue;

                        canBreak = false;

                        for (int itemIndx = 1; itemIndx < nItemOffsets; itemIndx++)
                        {
                            v = sample[index] - golden[index + kItemOffsets[itemIndx]];
                            absv = Math.Abs(v);

                            if (absv < min_absv)
                            {
                                if (absv < brokenThreshold)
                                {
                                    canBreak = true;
                                    break;
                                }

                                min_absv = absv;
                                min_v = v;
                            }
                        }

                        if (canBreak)
                            continue;

                        v = min_v + 128;
                        if (v < darkThreshold)
                        {
                            if (originData[index] < DarkIntensity)
                            {
                                pixels[interestedLineOffset + (counter[y]++)] = index;
                            }
                        }
                        else if (v > brightThreshold)
                        {
                            if (originData[index] > BrightIntensity)
                            {
                                pixels[interestedLineOffset + (counter[y]++)] = index;
                            }
                        }
                    }
                }
            }
            );
            ParallelHelper.DisposeAll(clonedRegions); clonedRegions = null;

            #region Update interested pixels here
            int interestedCount = 0;
            for (int y = 0; y < height; y++)
                interestedCount += counter[y];
            interestedPixels = new int[interestedCount];
            int count = 0;
            for (int y = 0; y < height; y++)
            {
                if (counter[y] <= 0)
                    continue;

                Array.Copy(pixels, y * width, interestedPixels, count, counter[y]);
                count += counter[y];
            }
            #endregion Update interested pixels here
        }




#if GPU_SUPPORTED
        public int[] GetInterestedPixels(
             DeviceBuffer<ushort> goldenImage
            ,DeviceBuffer<ushort> filteredImage
            ,CommonImage originImage
            ,double darkThreshold, double brightThreshold)
        {
            _imageWidth = originImage.Width;
            _imageHeight = originImage.Height;

            int[] interestedPixels = null;

            _marginLeft = Math.Max(_marginLeft, _kernelWidth >> 1);
            _marginRight = Math.Max(_marginRight, _kernelWidth >> 1);
            _marginTop = Math.Max(_marginTop, _kernelHeight >> 1);
            _marginBottom = Math.Max(_marginBottom, _kernelHeight >> 1);

            int marginLeft = _marginLeft;
            int marginRight = _marginRight + _dockRightSize;
            int marginTop = _marginTop;
            int marginBottom = _marginBottom;

            if (_regions == null)
            {
                SpeciallySubtract(
                    null,goldenImage,
                    null, filteredImage,
                    originImage.RasterImage._aData, originImage.cbImage,
                    ref interestedPixels,
                    _imageWidth, _imageHeight,
                    _kernelWidth, _kernelHeight,
                    marginLeft, marginTop, marginRight, marginBottom,
                    darkThreshold, brightThreshold);
            }
            else
            {
                SpeciallySubtract(
                    null,goldenImage,
                    null, filteredImage,
                    originImage.RasterImage._aData, originImage.cbImage,
                    ref interestedPixels,
                    _imageWidth, _imageHeight,
                    _kernelWidth, _kernelHeight,
                    marginLeft, marginTop, marginRight, marginBottom,
                    darkThreshold, brightThreshold, _regions);
            }
            return interestedPixels;
        }


        public static void SpeciallySubtract(
            ushort* golden, DeviceBuffer<ushort> dGolden,
            ushort* sample, DeviceBuffer<ushort> dSample,
            ushort* originData, DeviceBuffer<ushort> dOrigin,
            ref int[] interestedPixels,
            int width, int height, int kernelWidth, int kernelHeight,
            int marginLeft, int marginTop, int marginRight, int marginBottom,
            double darkThreshold, double brightThreshold)
        {
           SpeciallySubtract( golden, dGolden,
            sample,  dSample,
            originData, dOrigin,
            ref interestedPixels,
            width, height, kernelWidth, kernelHeight,
            marginLeft, marginTop, marginRight, marginBottom,
            darkThreshold, brightThreshold,null);
        }

        public static ComputeKernel kernelSpeciallySubtractPixel;
        public static ComputeKernel kernelSpeciallySubtractGetData;
        public static void SpeciallySubtract(
            ushort* golden, DeviceBuffer<ushort> dGolden,
            ushort* sample, DeviceBuffer<ushort> dSample,
            ushort* originData, DeviceBuffer<ushort> dOrigin,
            ref int[] interestedPixels,
            int width, int height, int kernelWidth, int kernelHeight,
            int marginLeft, int marginTop, int marginRight, int marginBottom,
            double darkThreshold, double brightThreshold, Region regions)
        {
#if DEBUG_METETIME
            DebugMeteTime dm = new DebugMeteTime();
#endif
            int[] kItemOffsets =
                CalcSpecialItemOffsets(width, kernelWidth, kernelHeight);
            int nItemOffsets = kItemOffsets.Length;

            int marginX = kernelWidth >> 1;
            int marginY = kernelHeight >> 1;

            marginLeft = Math.Max(marginX, marginLeft);
            marginRight = Math.Max(marginX, marginRight);
            marginTop = Math.Max(marginY, marginTop);
            marginBottom = Math.Max(marginY, marginBottom);

            int l = marginLeft;
            int t = marginTop;
            int r = width - marginRight - 1;
            int b = height - marginBottom - 1;

            double brokenThreshold = 128 - darkThreshold;
            brokenThreshold = Math.Min(brokenThreshold, brightThreshold - 128);

            if (kernelSpeciallySubtractPixel == null)
            {
                //DeviceProgram.InitCL();
                DeviceProgram.Compile(src);
                kernelSpeciallySubtractPixel = DeviceProgram.CreateKernel("SpeciallySubtractPixel");
                kernelSpeciallySubtractGetData = DeviceProgram.CreateKernel("SpeciallySubtractGetData");
            }

#if DEBUG_METETIME
            dm.AddLine("GoldenImageProcessor : SpeciallySubtract : Preprocess");
#endif
            //Init Varible 
            DeviceBuffer<ushort> cbGolden = dGolden;
            if (cbGolden == null)
                cbGolden = DeviceBuffer<ushort>.CreateHostBufferReadOnly(width * height, (IntPtr)golden);

#if DEBUG_METETIME
            dm.AddLine("GoldenImageProcessor : SpeciallySubtract : cbGolden");
#endif
            DeviceBuffer<ushort> cbSample = dSample;
            if (cbSample == null)
                cbSample = DeviceBuffer<ushort>.CreateHostBufferReadOnly(width * height, (IntPtr)sample);

#if DEBUG_METETIME
            dm.AddLine("GoldenImageProcessor : SpeciallySubtract : cbSample");
#endif
            DeviceBuffer<ushort> cbOrigin = dOrigin;
            if (cbOrigin == null)
                cbOrigin = DeviceBuffer<ushort>.CreateHostBufferReadOnly(width * height, (IntPtr)originData);

#if DEBUG_METETIME
            dm.AddLine("GoldenImageProcessor : SpeciallySubtract : cbOrigin");
#endif
            DeviceBuffer<bool> cbPixelBool = DeviceBuffer<bool>.CreateBufferReadWrite(width * height);
            DeviceBuffer<int> cbPixel = DeviceBuffer<int>.CreateBufferReadWrite(width * height);
            DeviceBuffer<int> cbCounter = DeviceBuffer<int>.CreateBufferReadWrite(width);

#if DEBUG_METETIME
            dm.AddLine("GoldenImageProcessor : SpeciallySubtract : cbPixel");
#endif
            DeviceBuffer<int> cbkItemOffsets = DeviceBuffer<int>.CreateHostBufferReadOnly(kItemOffsets);

#if DEBUG_METETIME
            dm.AddLine("GoldenImageProcessor : SpeciallySubtract : cbkItemOffsets");
#endif
            DeviceBuffer<int> cbParam = DeviceBuffer<int>.CreateHostBufferReadOnly(new int[] { l, t, r, b, width, height, nItemOffsets });

            //Call process kernelSpeciallySubtractPixel
            DeviceProgram.ExecuteKernel(kernelSpeciallySubtractPixel,
                new ComputeMemory[]{
                    cbSample,cbGolden,cbOrigin,cbPixelBool,cbkItemOffsets
                    ,cbParam
                    ,DeviceBuffer<double>.CreateHostBufferReadOnly(new double[]{darkThreshold,brightThreshold,brokenThreshold,DarkIntensity,BrightIntensity})
                }, new int[]{ r-l+1,b-t+1 } );
            DeviceProgram.Finish();

#if DEBUG_METETIME
            dm.AddLine("GoldenImageProcessor : SpeciallySubtract : kernelSpeciallySubtractPixel");
#endif

            //Call process kernelSpeciallySubtractGetData
            DeviceProgram.ExecuteKernel(kernelSpeciallySubtractGetData,
                new ComputeMemory[]{
                    cbPixelBool,cbPixel,cbCounter
                    ,DeviceBuffer<int>.CreateHostBufferReadOnly(new int[]{width,height})
                }, width );
            DeviceProgram.Finish();

#if DEBUG_METETIME
            dm.AddLine("GoldenImageProcessor : SpeciallySubtract : kernelSpeciallySubtractGetData");
#endif

            //read output
            int[] counter = cbCounter.Read(null);

            int cc = 0;
            int maxrow = 0;
            foreach (int c in counter)
            {
                if (maxrow < c) maxrow = c;
                cc += c;

            }
            int[] outbuff = new int[width * maxrow];
            fixed(void *p = outbuff)
            {
                cbPixel.Read(0, width * maxrow, (IntPtr)p, null);
            }

#if DEBUG_METETIME
            dm.AddLine("GoldenImageProcessor : SpeciallySubtract : Read");
#endif
            #region Update interested pixels here
            List<int> lRet = new List<int>();
            for(int x=0;x<width;x++)
            {
                if (counter[x] <= 0) continue;
                for(int y=0;y< counter[x] ;y++)
                {
                    int val = outbuff[y * width + x];
                    int yy = val / width;

                    if (regions!=null && !regions.IsVisible(x, yy)) continue;

                    lRet.Add(val);
                }
            }
            interestedPixels = lRet.ToArray();

#if DEBUG_METETIME
            dm.AddLine("GoldenImageProcessor : SpeciallySubtract : Remain");
#endif
          
            #endregion Update interested pixels here

            //free device buffer
            if (cbPixelBool != null)
                cbPixelBool.Dispose();
            if (cbPixel != null)
                cbPixel.Dispose();
            if (cbkItemOffsets != null)
                cbkItemOffsets.Dispose();
            if (cbCounter != null)
                cbCounter.Dispose();

            if (cbGolden != null && dGolden == null)
                cbGolden.Dispose();

            if (cbOrigin != null && dOrigin == null)
                cbOrigin.Dispose();

            if (cbSample != null && dSample == null)
                cbSample.Dispose();

#if DEBUG_METETIME
            dm.AddLine("GoldenImageProcessor : SpeciallySubtract : Free Device Buffer");
            dm.Write2Debug(true);
#endif
        }


#region kernel source 
        private static string src = @"
#pragma OPENCL EXTENSION cl_khr_fp64 : enable

__kernel void SpeciallySubtractPixel(
__global __read_only ushort* data
,__global __read_only ushort* golden
,__global __read_only ushort* originData
,__global __write_only bool* pixels
,__global __read_only int* kItemOffsets
,__global __read_only int* param //l,t,r,b,w,h,nItemOffsets
,__global __read_only double* threshold//darkThreshold,brightThreshold,brokenThreshold,DarkIntensity,BrightIntensity
)
{
    int x = get_global_id(0);//col
    int y = get_global_id(1);//row    
    
    double darkThreshold = threshold[0];
    double brightThreshold = threshold[1];
    double brokenThreshold = threshold[2];
    double DarkIntensity= threshold[3];
    double BrightIntensity= threshold[4];

    int nItemOffsets = param[6];
    int l = param[0];
    int t = param[1];
    int b = param[3];
    int w = param[4];
    
    x = x + l;
    y = y + t;

    int index = y*w + x;
    //pixels[index] = false;

    int min_v = data[index] - golden[index + kItemOffsets[0]];
    int min_absv = abs(min_v);

    if (min_absv < brokenThreshold)
      return;//skip this point

    int v = 0, absv = 0;
    for (int itemIndx = 1; itemIndx < nItemOffsets; itemIndx++)
    {
        v = data[index] - golden[index + kItemOffsets[itemIndx]];
        absv = abs(v);

        if (absv < min_absv)
        {
          if (absv < brokenThreshold)
            return;//skip this point

          min_absv = absv;
          min_v = v;
        }
    }
   
    v = min_v + 128;
    if (v < darkThreshold)
    {
      if (originData[index] < DarkIntensity)
      {
        pixels[index] = true;
      }
    }
    else if (v > brightThreshold)
    {
      if (originData[index] > BrightIntensity)
      {
        pixels[index] = true;
      }
    }
}


__kernel void SpeciallySubtractGetData(
__global __read_only bool* pixelsIn
,__global __write_only int* pixelsOut
,__global __write_only int* counter
,__global __read_only int* param //w,h
)
{
    int x = get_global_id(0);//col
    int w = param[0];
    int h = param[1];

    int countpixel =0;
    int index;
    for(int y=0;y<h;y++)
    {
        index = y*w+x;
        if( pixelsIn[index] == true )
        {
            pixelsOut[ countpixel*w+x ] = index;
            countpixel++;
        }
    }
    counter[x] = countpixel;
}

";
#endregion

#endif

    }
}
