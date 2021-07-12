#define USING_AVG_QUICK_CHECK__

using System;
using System.Collections.Generic;
using System.Text;

using SIA.IPEngine;
using System.Drawing;
using SIA.SystemFrameworks;

namespace SIA.SystemLayer.ImageProcessing
{
    //public enum eGoldenImageMethod
    //{
    //    None = 0,
    //    Average = 1,
    //    Median = 2,
    //    Min = 3,
    //    Max = 4,
    //    DiffToAverage = 5
    //}

    //unsafe public class GoldenImageCreator
    //{
    //    private int _width = 0;
    //    private int _height = 0;
    //    private int _nImages = 0;
    //    private byte[][] _inputRegions = null;
    //    private int[] _outputRegion = null;

    //    public GreyDataImage CreateGoldenImage(
    //        string[] sampleFiles, eGoldenImageMethod method)
    //    {
    //        GreyDataImage goldenImage = null;

    //        _nImages = sampleFiles.Length;

    //        int imageWidth = 0;
    //        int imageHeight = 0;
    //        using (System.Drawing.Image image = System.Drawing.Image.FromFile(sampleFiles[0]))
    //        {
    //            imageWidth = image.Width;
    //            imageHeight = image.Height;
    //        }

    //        int[] x = null, y = null;
    //        int nRegions = 0;
    //        GetRegions(imageWidth, imageHeight, ref nRegions, ref x, ref y);
    //        _outputRegion = new int[_width * _height];
    //        _inputRegions = new byte[_nImages][];
    //        for (int imageIndx = 0; imageIndx < _nImages; imageIndx++)
    //        {
    //            _inputRegions[imageIndx] = new byte[_width * _height];
    //        }

    //        goldenImage = new GreyDataImage(imageWidth, imageHeight);
    //        ushort* pDst = goldenImage._aData;
    //        for (int regionIndx = 0; regionIndx < nRegions; regionIndx++)
    //        {
    //            int l = x[regionIndx];
    //            int t = y[regionIndx];

    //            for (int imageIndx = 0; imageIndx < _nImages; imageIndx++)
    //            {
    //                SimpleImageIO.Load(sampleFiles[imageIndx],
    //                    ref _inputRegions[imageIndx],l, t, _width, _height);
    //            }

    //            switch (method)
    //            {
    //                case eGoldenImageMethod.Average:
    //                    CreateAverageImage(
    //                        _outputRegion, true, _inputRegions, _nImages, _width, _height);
    //                    break;
    //                case eGoldenImageMethod.Median:
    //                    CreateMedianImage(
    //                        _outputRegion, false, _inputRegions, _nImages, _width, _height);
    //                    break;
    //                case eGoldenImageMethod.Min:
    //                    CreateMinImage(
    //                        _outputRegion, true, _inputRegions, _nImages, _width, _height);
    //                    break;
    //                case eGoldenImageMethod.Max:
    //                    CreateMaxImage(
    //                        _outputRegion, true, _inputRegions, _nImages, _width, _height);
    //                    break;
    //                case eGoldenImageMethod.DiffToAverage:
    //                    CreateDiffToAverage(
    //                        _outputRegion, false, _inputRegions, _nImages, _width, _height);
    //                    break;
    //                default:
    //                    throw new System.Exception(
    //                        string.Format("Not support method: {0}", method.ToString()));
    //            }

    //            fixed (int* pOutputRegion = _outputRegion)
    //            {
    //                SimpleImageUtilities.Copy(
    //                    pDst, imageWidth, imageHeight, 
    //                    l, t, pOutputRegion, _width, _height);
    //            }
    //        }

    //        return goldenImage;
    //    }

    //    private void GetRegions(
    //        int imageWidth, int imageHeight, ref int nRegions, ref int[] x, ref int[] y)
    //    {
    //        int baseSize = 512;
    //        _width = imageWidth / ((imageWidth + baseSize - 1) / baseSize);
    //        _height = imageHeight / ((imageHeight + baseSize - 1) / baseSize);

    //        int n = (imageWidth + _width - 1) / _width;
    //        int m = (imageHeight + _height - 1) / _height;
    //        nRegions = n * m;
    //        x = new int[nRegions];
    //        y = new int[nRegions];
    //        int index = 0;
    //        for (int j = 0; j < m; j++)
    //        {
    //            for (int i = 0; i < n; i++, index++)
    //            {
    //                x[index] = i * _width;
    //                y[index] = j * _height;
    //            }
    //        }
    //    }

    //    public void LoadInputRegions(string[] sampleFiles, int x, int y)
    //    {

    //    }


    //    #region Create Average Image
    //    public static void CreateAverageImage(
    //        int[] avgImage, bool resetOutput, byte[][] images, int nImages, int width, int height)
    //    {
    //        int length = width * height;
    //        if (resetOutput)
    //        {
    //            int v = (int)(nImages >> 1);
    //            for (int i = 0; i < length; i++)
    //                avgImage[i] = v;
    //        }

    //        // how to access memory optimal here
    //        // how to prevent miss-cache for memory cache limitation
    //        // before passing images, image-size should be considered...
    //        // at the time, maximum only 2 images is loading to memory...
    //        // output image always loads...
    //        for (int imageIndx = 0; imageIndx < nImages; imageIndx++)
    //        {
    //            byte[] src = images[imageIndx];
    //            for (int i = 0; i < length; i++)
    //            {
    //                avgImage[i] += src[i]; // assumed that max intensity is 255;
    //            }
    //            src = null;
    //        }

    //        for (int i = 0; i < length; i++)
    //        {
    //            avgImage[i] = (int)(avgImage[i] / nImages);
    //        }
    //    }

    //    public static void CreateAverageImage(
    //        ushort* avgImage, bool resetOutput, ushort** images, int nImages, int width, int height)
    //    {
    //        int length = width * height;
    //        if (resetOutput)
    //        {
    //            ushort v = (ushort)(nImages >> 1);
    //            for (int i = 0; i < length; i++)
    //                *(avgImage + i) = v;
    //        }

    //        // how to access memory optimal here
    //        // how to prevent miss-cache for memory cache limitation
    //        // before passing images, image-size should be considered...
    //        // at the time, maximum only 2 images is loading to memory...
    //        // output image always loads...
    //        for (int imageIndx = 0; imageIndx < nImages; imageIndx++)
    //        {
    //            ushort* src = images[imageIndx];
    //            for (int i = 0; i < length; i++)
    //            {
    //                *(avgImage + i) += *(src + i); // assumed that max intensity is 255;
    //            }
    //            src = null;
    //        }

    //        for (int i = 0; i < length; i++)
    //        {
    //            *(avgImage + i) = (ushort)(*(avgImage + i) / nImages);
    //        }
    //    }
    //    #endregion Create Average Image

    //    public static void CreateMinImage(
    //        int[] avgImage, bool resetOutput, byte[][] images, int nImages, int width, int height)
    //    {
    //        int length = width * height;
    //        if (resetOutput)
    //        {
    //            for (int i = 0; i < length; i++)
    //                avgImage[i] = 255;
    //        }

    //        // how to access memory optimal here
    //        // how to prevent miss-cache for memory cache limitation
    //        // before passing images, image-size should be considered...
    //        // at the time, maximum only 2 images is loading to memory...
    //        // output image always loads...
    //        for (int imageIndx = 0; imageIndx < nImages; imageIndx++)
    //        {
    //            byte[] src = images[imageIndx];
    //            for (int i = 0; i < length; i++)
    //            {
    //                if (avgImage[i] > src[i])
    //                    avgImage[i] = src[i];
    //            }
    //            src = null;
    //        }
    //    }

    //    public static void CreateMaxImage(
    //        int[] avgImage, bool resetOutput, byte[][] images, int nImages, int width, int height)
    //    {
    //        int length = width * height;
    //        if (resetOutput)
    //        {
    //            for (int i = 0; i < length; i++)
    //                avgImage[i] = 0;
    //        }

    //        // how to access memory optimal here
    //        // how to prevent miss-cache for memory cache limitation
    //        // before passing images, image-size should be considered...
    //        // at the time, maximum only 2 images is loading to memory...
    //        // output image always loads...
    //        for (int imageIndx = 0; imageIndx < nImages; imageIndx++)
    //        {
    //            byte[] src = images[imageIndx];
    //            for (int i = 0; i < length; i++)
    //            {
    //                if (avgImage[i] < src[i])
    //                    avgImage[i] = src[i];
    //            }
    //            src = null;
    //        }
    //    }

    //    #region Create Median Image
    //    // how to access memory optimal here
    //    // how to prevent miss-cache for memory cache limitation
    //    // before passing images, image-size should be considered...
    //    // at the time, maximum only 2 images is loading to memory...
    //    // output image always loads...
    //    public static void CreateMedianImage(
    //        int[] medianImage, bool resetOutput, byte[][] images, int nImages, int width, int height)
    //    {
    //        int length = width * height;
    //        if (resetOutput)
    //        {
    //            for (int i = 0; i < length; i++)
    //                medianImage[i] = 0;
    //        }

    //        int middle = nImages >> 1;
    //        Parallel.For(0, height, delegate(int y)
    //        {
    //            int[] a = new int[nImages];
    //            int index = y * width;
    //            for (int x = 0; x < width; x++, index++)
    //            {
    //                for (int imageIndx = 0; imageIndx < nImages; imageIndx++)
    //                {
    //                    a[imageIndx] = images[imageIndx][index];
    //                }

    //                Array.Sort(a);

    //                medianImage[index] = a[middle];
    //            }
    //        });
    //    }
    //    #endregion Create Median Image

    //    #region min difference to average
    //    public static void CreateDiffToAverage(
    //        int[] medianImage, bool resetOutput, byte[][] images, int nImages, int width, int height)
    //    {
    //        int length = width * height;
    //        if (resetOutput)
    //        {
    //            for (int i = 0; i < length; i++)
    //                medianImage[i] = 0;
    //        }

    //        int middle = nImages >> 1;
    //        Parallel.For(0, height, delegate(int y)
    //        {
    //            int[] a = new int[nImages];
    //            int index = y * width;
    //            double avg = 0;
    //            double min_absv = double.MaxValue;
    //            double abs_v;
    //            ushort intensity = 0;
    //            for (int x = 0; x < width; x++, index++)
    //            {
    //                avg = 0;
    //                for (int imageIndx = 0; imageIndx < nImages; imageIndx++)
    //                {
    //                    a[imageIndx] = images[imageIndx][index];
    //                    avg += images[imageIndx][index];
    //                }

    //                avg = avg / nImages;

    //                intensity = 0;
    //                min_absv = double.MaxValue;
    //                for (int imageIndx = 0; imageIndx < nImages; imageIndx++)
    //                {
    //                    abs_v = avg - images[imageIndx][index];
    //                    if (abs_v < min_absv)
    //                    {
    //                        min_absv = abs_v;
    //                        intensity = images[imageIndx][index];
    //                    }
    //                }
    //                //Array.Sort(a);

    //                //medianImage[index] = a[middle];
    //                medianImage[index] = intensity;
    //            }
    //        });
    //    }
    //    #endregion min difference to average

    //    #region Subtract Golden Image

    //    #endregion Subtract Golden Image
    //}

//    unsafe public class GoldenImageProcessor
//    {
//        private int _dockRightSize = 57;

//        private int _marginLeft = 5;
//        private int _marginTop = 23;
//        private int _marginRight = 3;
//        private int _marginBottom = 23;

//        private int _kernelWidth = 5;        
//        private int _kernelHeight = 5;

//        private int[] _kItemOffsets = null;

//        private int _imageWidth = 0;
//        private int _imageHeight = 0;

        

//        private GreyDataImage Subract(
//            GreyDataImage goldenImage, GreyDataImage sample)
//        {
//            _imageWidth = goldenImage.Width;
//            _imageHeight = goldenImage.Height;

//            GreyDataImage output = 
//                new GreyDataImage(_imageWidth, _imageHeight);

//            Subtract(
//                goldenImage._aData, sample._aData, 
//                output._aData, _imageWidth, _imageHeight);

//            return output;
//        }

//        public static void Subtract(
//            ushort* golden, ushort* sample, int width, int height)
//        {
//            Subtract(golden, sample, sample, width, height);
//        }

//        private static int[] CalcSpecialItemOffsets(
//            int imageWidth, int kernelWidth, int kernelHeight)
//        {
//            int[] xItems = new int[] {
//                0, 
//                -1, 0, 1, 
//                -2, -1, 0, 1, 2, 
//                -1, 0, 1,
//                0
//            };
//            int[] yItems = new int[] {
//                -2, 
//                -1, -1, -1, 
//                0, 0, 0, 0, 0, 
//                1, 1, 1, 
//                2 };

//            //CreateItemOffsets(kernelWidth, kernelHeight, ref xItems, ref yItems);
//            CreatePriorityRingItemOffsets(kernelWidth, kernelHeight, ref xItems, ref yItems);

//            return CalcItemOffsets(imageWidth, xItems, yItems);
//        }

//        private static void CreateItemOffsets(
//            int kernelWidth, int kernelHeight, ref int[] xItems, ref int[] yItems)
//        {
//            int halfWidth = kernelWidth >> 1;
//            int halfHeight = kernelHeight >> 1;

//            int n = kernelWidth * kernelHeight;
//            xItems = new int[n];
//            yItems = new int[n];

//            int index = 0;
//            for (int y = -halfHeight; y <= halfHeight; y++)
//            {
//                for (int x = -halfWidth; x <= halfWidth; x++, index++)
//                {
//                    xItems[index] = x;
//                    yItems[index] = y;
//                }
//            }
//        }

//        private static void CreatePriorityRingItemOffsets(
//            int kernelWidth, int kernelHeight, ref int[] xItems, ref int[] yItems)
//        {
//            int halfWidth = kernelWidth >> 1;
//            int halfHeight = kernelHeight >> 1;

//            int n = kernelWidth * kernelHeight;
//            xItems = new int[n];
//            yItems = new int[n];

//            int index = 1;
//            for (int ring = 1; ring <= halfHeight; ring++)
//            {
//                CreateRingItemOffsets(ring, ref xItems, ref yItems, ref index);
//            }
//        }

//        private static void CreateRingItemOffsets(
//            int ring, ref int[] xItems, ref int[] yItems, ref int index)
//        {
//            // top-line
//            CreateItemOffsets(-ring, -ring, 1, 0, ring, -ring, ref xItems, ref yItems, ref index);
//            // bottom-line
//            CreateItemOffsets(-ring, ring, 1, 0, ring, ring, ref xItems, ref yItems, ref index);
//            // left-line
//            CreateItemOffsets(-ring, -(ring - 1), 0, 1, -ring, ring-1, ref xItems, ref yItems, ref index);
//            // right-line
//            CreateItemOffsets(ring, -(ring - 1), 0, 1, ring, ring - 1, ref xItems, ref yItems, ref index);
//        }

//        private static void CreateItemOffsets(
//            int x, int y, int ex, int ey, int maxX, int maxY, 
//            ref int[] xItems, ref int[] yItems, ref int index)
//        {
//            ex = 1; ey = 1;
//            for (int iy = y; iy <= maxY; iy += ey)
//            {
//                for (int ix = x; ix <= maxX; ix += ex)
//                {
//                    xItems[index] = ix;
//                    yItems[index] = iy;
//                    index++;
//                }
//            }
//        }

//        private static int[] CalcItemOffsets(
//            int imageWidth, int[] xItems, int[] yItems)
//        {
//            int n = xItems.Length;
//            int[] kItemOffsets = new int[n];
//            for (int i = 0; i < n; i++)
//            {
//                int offset = yItems[i] * imageWidth + xItems[i];
//                kItemOffsets[i] = offset;
//            }

//            return kItemOffsets;
//        }

//        private const int linesPerThread = 10;
//        public static void Subtract(
//            ushort* golden, ushort* sample, ushort* output, int width, int height)
//        {
//            Parallel.For(0, height, delegate(int y)
//            {
//                int index = y * width;
//                int v = 0;
//                for (int x = 0; x < width; x++, index++)
//                {
//                    v = 128 + sample[index] - golden[index];
//                    if (v < 0)
//                        output[index] = 0;
//                    else if (v > 255)
//                        output[index] = 255;
//                    else
//                        output[index] = (ushort)v;
//                }
//            });
//        }

        
        
















        
        
//        /**
//         * All using functions....
//         * 
//         * **/





//        public void SetKernelSize(int kernelWidth, int kernelHeight)
//        {
//            _kernelWidth = kernelWidth;
//            _kernelHeight = kernelHeight;
//        }







//        public void Subract(
//            GreyDataImage goldenImage, ref GreyDataImage sample)
//        {
//            _imageWidth = goldenImage.Width;
//            _imageHeight = goldenImage.Height;

//            //Subtract(
//            //    goldenImage._aData, sample._aData, _imageWidth, _imageHeight);

//#if RELEASE

//            throw new System.Exception("Testing Pole-tip...");

//            _kernelWidth = 7;
//            _kernelHeight = 7;
//#endif

//            SpeciallySubtract(goldenImage.
//                _aData, sample._aData, sample._aData,
//                _imageWidth, _imageHeight, _kernelWidth, _kernelHeight);
//        }
        
        
        
        
//        public static void SpeciallySubtract(
//            ushort* golden, 
//            ushort* sample, ushort* output, 
//            int width, int height, int kernelWidth, int kernelHeight)
//        {
//            int[] kItemOffsets = 
//                CalcSpecialItemOffsets(width, kernelWidth, kernelHeight);
//            int nItemOffsets = kItemOffsets.Length;

//            int marginX = kernelWidth >> 1;
//            int marginY = kernelHeight >> 1;
//            int l = marginX;
//            int t = marginY;
//            int r = width - marginX - 1;
//            int b = height - marginY - 1;

//            Parallel.For(t, b + 1, delegate(int y)
//            {
//                int index = y * width + l;
//                int v = 0, absv = 0;
//                int min_absv = int.MaxValue;
//                int min_v = 0;
//                for (int x = l; x <= r; x++, index++)
//                {
//                    min_absv = int.MaxValue;
//                    min_v = 0;
//                    for (int itemIndx = 0; itemIndx < nItemOffsets; itemIndx++)
//                    {
//                        v = sample[index] - golden[index + kItemOffsets[itemIndx]];
//                        absv = Math.Abs(v);

//                        if (absv < min_absv)
//                        {
//                            min_absv = absv;
//                            min_v = v;
//                        }
//                    }

//                    v = min_v;

//                    v += 128;

//                    if (v < 0)
//                        output[index] = 0;
//                    else if (v > 255)
//                        output[index] = 255;
//                    else
//                        output[index] = (ushort)v;
//                }
//            });
//        }

//        public static void SpeciallySubtract(
//            ushort* golden,
//            ushort* sample, bool* output,
//            int width, int height, int kernelWidth, int kernelHeight, 
//            int marginLeft, int marginTop, int marginRight, int marginBottom,
//            double darkThreshold, double brightThreshold)
//        {
//#if USING_AVG_QUICK_CHECK
//            #region integral image
//            double[] integralImage = new double[width * height];
//            fixed (double* pIntegralImage = integralImage)
//                Utilities.CalcImageIntegrals(golden, width, height, pIntegralImage);
//            double kernelLength = kernelWidth * kernelHeight;
//            int refIndxLT = -(kernelHeight / 2) * (width + 1);
//            int refIndxRT = refIndxLT + kernelWidth - 1;
//            int refIndxLB = -refIndxRT;
//            int refIndxRB = -refIndxLT;

//            refIndxLT -= width + 1;
//            refIndxRT -= width;
//            refIndxLB--;
//            double max_intensity = 255;
//            double inv_max_intensity = 1.0 / max_intensity;

//            double localMean = 0;
//            double intensity_local_factor = max_intensity / kernelLength;
//            #endregion integral image
//#endif

//            int[] kItemOffsets =
//                CalcSpecialItemOffsets(width, kernelWidth, kernelHeight);
//            int nItemOffsets = kItemOffsets.Length;

//            int marginX = kernelWidth >> 1;
//            int marginY = kernelHeight >> 1;

//            marginLeft = Math.Max(marginX, marginLeft);
//            marginRight = Math.Max(marginX, marginRight);
//            marginTop = Math.Max(marginY, marginTop);
//            marginBottom = Math.Max(marginY, marginBottom);

//            int l = marginLeft;
//            int t = marginTop;
//            int r = width - marginRight - 1;
//            int b = height - marginBottom - 1;

//            double brokenThreshold = 128 - darkThreshold;
//            brokenThreshold = Math.Min(brokenThreshold, brightThreshold - 128);

//            //int numYPerStep = 50; // one thread will perform numYPerStep lines
//            //int nStep = (b + 1 - t) / numYPerStep + 1;
//            //if ((b + 1 - t) % numYPerStep != 0)
//            //{
//            //    nStep += 1;
//            //}

//            //Parallel.For(0, nStep, delegate(int step)
//            //{
//            //    int yStart = step * numYPerStep + t;
//            //    int yEnd = yStart + numYPerStep - 1;
//            //    if (yEnd > b) yEnd = b;

//            //    int index = yStart * width + l;
//            //    int v = 0, absv = 0;
//            //    int min_absv = int.MaxValue;
//            //    int min_v = 0;
//            //    bool canBreak = false;

//            //    for (int y = yStart; y <= yEnd; y++)
//            //    {
//            Parallel.For(t, b + 1, delegate(int y)
//            {
//                int index = y * width + l;
//                int v = 0, absv = 0;
//                int min_absv = int.MaxValue;
//                int min_v = 0;
//                bool canBreak = false;

//                for (int x = l; x <= r; x++, index++)
//                {

//#if USING_AVG_QUICK_CHECK
//                    // calc local mean
//                    localMean = integralImage[index + refIndxRB] -
//                                integralImage[index + refIndxRT] -
//                                integralImage[index + refIndxLB] +
//                                integralImage[index + refIndxLT];
//                    localMean *= intensity_local_factor;
//                    double diffLocalMean = Math.Abs(sample[index] - localMean);
//                    if (diffLocalMean < brokenThreshold)
//                        break;
//#endif

//                    min_v = sample[index] - golden[index + kItemOffsets[0]];
//                    min_absv = Math.Abs(min_v);

//                    if (min_absv < brokenThreshold)
//                        continue;

//                    canBreak = false;

//                    for (int itemIndx = 1; itemIndx < nItemOffsets; itemIndx++)
//                    {
//                        v = sample[index] - golden[index + kItemOffsets[itemIndx]];
//                        absv = Math.Abs(v);

//                        if (absv < min_absv)
//                        {
//                            if (absv < brokenThreshold)
//                            {
//                                canBreak = true;
//                                break;
//                            }

//                            min_absv = absv;
//                            min_v = v;
//                        }
//                    }

//                    if (canBreak)
//                        continue;

//                    v = min_v + 128;
//                    if (v < darkThreshold)
//                        output[index] = true;
//                    else if (v > brightThreshold)
//                        output[index] = true;
//                }
//                //}
//            });
//        }

//        public BinaryImage Binarize(
//            GreyDataImage goldenImage, GreyDataImage sample, 
//            double darkThreshold, double brightThreshold)
//        {
//            BinaryImage binImage = null;

//            _imageWidth = goldenImage.Width;
//            _imageHeight = goldenImage.Height;

//            binImage =
//                new BinaryImage(_imageWidth, _imageHeight);
//            bool* buffer = (bool*)binImage.Buffer.ToPointer();

//#if RELEASE

//            throw new System.Exception("Testing Pole-tip...");

//            _kernelWidth = 7; // ABS
//            _kernelHeight = 7; // ABS
//#endif
           
//            //_kernelWidth = 19; // Pole-tip
//            //_kernelHeight = 19; // Pole-tip

//            _marginLeft = Math.Max(_marginLeft, _kernelWidth >> 1);
//            _marginRight = Math.Max(_marginRight, _kernelWidth >> 1);
//            _marginTop = Math.Max(_marginTop, _kernelHeight >> 1);
//            _marginBottom = Math.Max(_marginBottom, _kernelHeight >> 1);

//            int marginLeft = _marginLeft;
//            int marginRight = _marginRight + _dockRightSize;
//            int marginTop = _marginTop;
//            int marginBottom = _marginBottom;

//            SpeciallySubtract(goldenImage._aData, sample._aData, 
//                buffer, _imageWidth, _imageHeight, 
//                _kernelWidth, _kernelHeight, 
//                marginLeft, marginTop, marginRight, marginBottom,
//                darkThreshold, brightThreshold);

//            return binImage;
//        }

//        public static bool ValidateImageSize(
//            string[] sampleImages, ref string msg)
//        {
//            try
//            {
//                int w = 0;
//                int h = 0;
//                using (Image image = Image.FromFile(sampleImages[0]))
//                {
//                    w = image.Width;
//                    h = image.Height;
//                }

//                for (int i = 1; i < sampleImages.Length; i++)
//                {
//                    using (Image image = Image.FromFile(sampleImages[i]))
//                    {
//                        if (w != image.Width ||
//                            h != image.Height)
//                        {
//                            msg = "Image(s) are not the same size!";
//                            return false;
//                        }
//                    }
//                }
//            }
//            catch (System.Exception exp)
//            {
//                msg = exp.Message;
//                return false;
//            }
//            finally
//            {
//                GC.Collect();
//            }

//            return true;
//        }

//        public static bool ValidateImageSize(
//            string imageFile, int width, int height, ref string msg)
//        {
//            try
//            {
//                using (Image image = Image.FromFile(imageFile))
//                {
//                    if (image.Width != width ||
//                        image.Height != height)
//                    {
//                        msg = "Image is not the same size!";
//                        return false;
//                    }
//                }
//            }
//            catch (System.Exception exp)
//            {
//                msg = exp.Message;
//                return false;
//            }

//            return true;
//        }
//    }
}
