using System;
using System.Collections.Generic;
using System.Text;

using SIA.IPEngine;
using System.Drawing;
using SIA.SystemFrameworks;

namespace SIA.Algorithms.ReferenceFile
{
    public enum eGoldenImageMethod
    {
        None = 0,
        Average = 1,
        Median = 2,
        Min = 3,
        Max = 4,
        DiffToAverage = 5
    }

    public class AnomalyDetectorDefinition
    {
        public static eGoldenImageMethod Method = eGoldenImageMethod.Median;
        public static double ABSDarkThreshold = 92.5;
        public static double ABSBrightThreshold = 150;

        public static int ABSKernelWidthSubtraction = 11; // 9;
        public static int ABSKernelHeightSubtraction = 11; // 9;

        public static int ABSKernelWidthFilter = 11;
        public static int ABSKernelHeightFilter = 11;
        public static bool ABSAutoNoiseFilter = false;
        public static double ABSNoiseLevelFilter = 0.01;
    }

    unsafe public class GoldenImageCreator
    {
        private int _width = 0;
        private int _height = 0;
        private int _nImages = 0;
        private byte[][] _inputRegions = null;
        private int[] _outputRegion = null;

        public GreyDataImage CreateGoldenImage(
            string[] sampleFiles, eGoldenImageMethod method)
        {
            GreyDataImage goldenImage = null;

            _nImages = sampleFiles.Length;

            int imageWidth = 0;
            int imageHeight = 0;
            using (System.Drawing.Image image = System.Drawing.Image.FromFile(sampleFiles[0]))
            {
                imageWidth = image.Width;
                imageHeight = image.Height;
            }

            int[] x = null, y = null;
            int nRegions = 0;
            GetRegions(imageWidth, imageHeight, ref nRegions, ref x, ref y);
            _outputRegion = new int[_width * _height];
            _inputRegions = new byte[_nImages][];
            for (int imageIndx = 0; imageIndx < _nImages; imageIndx++)
            {
                _inputRegions[imageIndx] = new byte[_width * _height];
            }

            goldenImage = new GreyDataImage(imageWidth, imageHeight);
            ushort* pDst = goldenImage._aData;
            for (int regionIndx = 0; regionIndx < nRegions; regionIndx++)
            {
                int l = x[regionIndx];
                int t = y[regionIndx];

                for (int imageIndx = 0; imageIndx < _nImages; imageIndx++)
                {
                    SimpleImageIO.Load(sampleFiles[imageIndx],
                        ref _inputRegions[imageIndx], l, t, _width, _height);
                }

                switch (method)
                {
                    case eGoldenImageMethod.Average:
                        CreateAverageImage(
                            _outputRegion, true, _inputRegions, _nImages, _width, _height);
                        break;
                    case eGoldenImageMethod.Median:
                        CreateMedianImage(
                            _outputRegion, false, _inputRegions, _nImages, _width, _height);
                        break;
                    case eGoldenImageMethod.Min:
                        CreateMinImage(
                            _outputRegion, true, _inputRegions, _nImages, _width, _height);
                        break;
                    case eGoldenImageMethod.Max:
                        CreateMaxImage(
                            _outputRegion, true, _inputRegions, _nImages, _width, _height);
                        break;
                    case eGoldenImageMethod.DiffToAverage:
                        CreateDiffToAverage(
                            _outputRegion, false, _inputRegions, _nImages, _width, _height);
                        break;
                    default:
                        throw new System.Exception(
                            string.Format("Not support method: {0}", method.ToString()));
                }

                fixed (int* pOutputRegion = _outputRegion)
                {
                    SimpleImageUtilities.Copy(
                        pDst, imageWidth, imageHeight,
                        l, t, pOutputRegion, _width, _height);
                }
            }

            return goldenImage;
        }

        private void GetRegions(
            int imageWidth, int imageHeight, ref int nRegions, ref int[] x, ref int[] y)
        {
            int baseSize = 512;
            _width = imageWidth / ((imageWidth + baseSize - 1) / baseSize);
            _height = imageHeight / ((imageHeight + baseSize - 1) / baseSize);

            int n = (imageWidth + _width - 1) / _width;
            int m = (imageHeight + _height - 1) / _height;
            nRegions = n * m;
            x = new int[nRegions];
            y = new int[nRegions];
            int index = 0;
            for (int j = 0; j < m; j++)
            {
                for (int i = 0; i < n; i++, index++)
                {
                    x[index] = i * _width;
                    y[index] = j * _height;
                }
            }
        }

        public void LoadInputRegions(string[] sampleFiles, int x, int y)
        {

        }


        #region Create Average Image
        public static void CreateAverageImage(
            int[] avgImage, bool resetOutput, byte[][] images, int nImages, int width, int height)
        {
            int length = width * height;
            if (resetOutput)
            {
                int v = (int)(nImages >> 1);
                for (int i = 0; i < length; i++)
                    avgImage[i] = v;
            }

            // how to access memory optimal here
            // how to prevent miss-cache for memory cache limitation
            // before passing images, image-size should be considered...
            // at the time, maximum only 2 images is loading to memory...
            // output image always loads...
            for (int imageIndx = 0; imageIndx < nImages; imageIndx++)
            {
                byte[] src = images[imageIndx];
                for (int i = 0; i < length; i++)
                {
                    avgImage[i] += src[i]; // assumed that max intensity is 255;
                }
                src = null;
            }

            for (int i = 0; i < length; i++)
            {
                avgImage[i] = (int)(avgImage[i] / nImages);
            }
        }

        public static void CreateAverageImage(
            ushort* avgImage, bool resetOutput, ushort** images, int nImages, int width, int height)
        {
            int length = width * height;
            if (resetOutput)
            {
                ushort v = (ushort)(nImages >> 1);
                for (int i = 0; i < length; i++)
                    *(avgImage + i) = v;
            }

            // how to access memory optimal here
            // how to prevent miss-cache for memory cache limitation
            // before passing images, image-size should be considered...
            // at the time, maximum only 2 images is loading to memory...
            // output image always loads...
            for (int imageIndx = 0; imageIndx < nImages; imageIndx++)
            {
                ushort* src = images[imageIndx];
                for (int i = 0; i < length; i++)
                {
                    *(avgImage + i) += *(src + i); // assumed that max intensity is 255;
                }
                src = null;
            }

            for (int i = 0; i < length; i++)
            {
                *(avgImage + i) = (ushort)(*(avgImage + i) / nImages);
            }
        }
        #endregion Create Average Image

        public static void CreateMinImage(
            int[] avgImage, bool resetOutput, byte[][] images, int nImages, int width, int height)
        {
            int length = width * height;
            if (resetOutput)
            {
                for (int i = 0; i < length; i++)
                    avgImage[i] = 255;
            }

            // how to access memory optimal here
            // how to prevent miss-cache for memory cache limitation
            // before passing images, image-size should be considered...
            // at the time, maximum only 2 images is loading to memory...
            // output image always loads...
            for (int imageIndx = 0; imageIndx < nImages; imageIndx++)
            {
                byte[] src = images[imageIndx];
                for (int i = 0; i < length; i++)
                {
                    if (avgImage[i] > src[i])
                        avgImage[i] = src[i];
                }
                src = null;
            }
        }

        public static void CreateMaxImage(
            int[] avgImage, bool resetOutput, byte[][] images, int nImages, int width, int height)
        {
            int length = width * height;
            if (resetOutput)
            {
                for (int i = 0; i < length; i++)
                    avgImage[i] = 0;
            }

            // how to access memory optimal here
            // how to prevent miss-cache for memory cache limitation
            // before passing images, image-size should be considered...
            // at the time, maximum only 2 images is loading to memory...
            // output image always loads...
            for (int imageIndx = 0; imageIndx < nImages; imageIndx++)
            {
                byte[] src = images[imageIndx];
                for (int i = 0; i < length; i++)
                {
                    if (avgImage[i] < src[i])
                        avgImage[i] = src[i];
                }
                src = null;
            }
        }

        #region Create Median Image
        // how to access memory optimal here
        // how to prevent miss-cache for memory cache limitation
        // before passing images, image-size should be considered...
        // at the time, maximum only 2 images is loading to memory...
        // output image always loads...
        public static void CreateMedianImage(
            int[] medianImage, bool resetOutput, byte[][] images, int nImages, int width, int height)
        {
            int length = width * height;
            if (resetOutput)
            {
                for (int i = 0; i < length; i++)
                    medianImage[i] = 0;
            }

            int middle = nImages >> 1;
            Parallel.For(0, height, delegate(int y)
            {
                int[] a = new int[nImages];
                int index = y * width;
                for (int x = 0; x < width; x++, index++)
                {
                    for (int imageIndx = 0; imageIndx < nImages; imageIndx++)
                    {
                        a[imageIndx] = images[imageIndx][index];
                    }

                    Array.Sort(a);

                    medianImage[index] = a[middle];
                }
            });
        }
        #endregion Create Median Image

        #region min difference to average
        public static void CreateDiffToAverage(
            int[] medianImage, bool resetOutput, byte[][] images, int nImages, int width, int height)
        {
            int length = width * height;
            if (resetOutput)
            {
                for (int i = 0; i < length; i++)
                    medianImage[i] = 0;
            }

            int middle = nImages >> 1;
            Parallel.For(0, height, delegate(int y)
            {
                int[] a = new int[nImages];
                int index = y * width;
                double avg = 0;
                double min_absv = double.MaxValue;
                double abs_v;
                ushort intensity = 0;
                for (int x = 0; x < width; x++, index++)
                {
                    avg = 0;
                    for (int imageIndx = 0; imageIndx < nImages; imageIndx++)
                    {
                        a[imageIndx] = images[imageIndx][index];
                        avg += images[imageIndx][index];
                    }

                    avg = avg / nImages;

                    intensity = 0;
                    min_absv = double.MaxValue;
                    for (int imageIndx = 0; imageIndx < nImages; imageIndx++)
                    {
                        abs_v = avg - images[imageIndx][index];
                        if (abs_v < min_absv)
                        {
                            min_absv = abs_v;
                            intensity = images[imageIndx][index];
                        }
                    }
                    //Array.Sort(a);

                    //medianImage[index] = a[middle];
                    medianImage[index] = intensity;
                }
            });
        }
        #endregion min difference to average

        #region Subtract Golden Image

        #endregion Subtract Golden Image
    }
}
