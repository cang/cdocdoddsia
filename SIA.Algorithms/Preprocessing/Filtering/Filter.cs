using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using SIA.Common;
using SIA.IPEngine;

namespace SIA.Algorithms.Preprocessing.Filtering
{
    public class Filter
    {
        #region Enums
        public enum MorphologyModes
        {
            HitAndMiss = 0,
            Thinning = 1,
            Thickening = 2
        }

        #endregion

        #region Kernels

        public static int[,] Kernel_Mean = new int[,] { 
            { 1, 1, 1 }, 
            { 1, 1, 1 }, 
            { 1, 1, 1 } };

        public static int[,] Kernel_Blur = new int[,] {
            { 1, 2, 3, 2, 1 },
            { 2, 4, 5, 4, 2 },
            { 3, 5, 6, 5, 3 },
            { 2, 4, 5, 4, 2 },
            { 1, 2, 3, 2, 1 } };

        public static int[,] Kernel_GaussianBlur = new int[,] {
            { 1, 4, 7, 4, 1 },
            { 4, 16, 26, 16, 4 },
            { 7, 26, 41, 26, 7 },
            { 4, 16, 26, 16, 4 },
            { 1, 4, 7, 4, 1 } };

        public static int[,] Kernel_Edge = new int[,] {
            {  0, -1,  0 },
            { -1,  4, -1 },
            {  0, -1,  0 } };

        public static int[,] Kernel_Sharpen = new int[,] {
            {  0, -1,  0 },
            { -1,  5, -1 },
            {  0, -1,  0 } };

        public static int[,] Kernel_SobelX5 = new int[,] {
            {  -1, -2, 0, 2, 1},
            {  -4, -8, 0, 8, 4},
            {  -6, -12, 0, 12, 6},
            {  -4, -8, 0, 8, 4},
            {  -1, -2, 0, 2, 1} };

        public static int[,] Kernel_SobelX3 = new int[,] {
            { -1, 0, 1 },
            { -2, 0, 2 },
            { -1, 0, 1 } };

        public static int[,] Kernel_SobelY3 = new int[,] {
            { -1, -2, -1 },
            { 0, 0, 0 },
            { 1, 2, 1 } };

        public static int[,] Kernel_SobelY5 = new int[,] {
            {  -1, -4, -6, -4, -1},
            {  -2, -8, -12, -8, -2},
            {  0, 0, 0, 0, 0},
            {  2, 8, 12, 8, 2},
            {  1, 4, 6, 4, 1} };

        public static int[,] Kernel_Morphology0 = new int[,] { 
            { 0, 0, 0 }, 
            { -1, 1, -1 }, 
            { 1, 1, 1 } };
        public static int[,] Kernel_Morphology1 = new int[,] { 
            { -1, 0, 0 }, 
            { 1, 1, 0 }, 
            { -1, 1, -1 } };
        public static int[,] Kernel_Morphology2 = new int[,] { 
            { 1, -1, 0 },
            { 1, 1, 0 }, 
            { 1, -1, 0 } };
        public static int[,] Kernel_Morphology3 = new int[,] { 
            { -1, 1, -1 }, 
            { 1, 1, 0 }, 
            { -1, 0, 0 } };
        public static int[,] Kernel_Morphology4 = new int[,] { 
            { 1, 1, 1 }, 
            { -1, 1, -1 }, 
            { 0, 0, 0 } };
        public static int[,] Kernel_Morphology5 = new int[,] { 
            { -1, 1, -1 }, 
            { 0, 1, 1 }, 
            { 0, 0, -1 } };
        public static int[,] Kernel_Morphology6 = new int[,] { 
            { 0, -1, 1 }, 
            { 0, 1, 1 }, 
            { 0, -1, 1 } };
        public static int[,] Kernel_Morphology7 = new int[,] { 
            { 0, 0, -1 }, 
            { 0, 1, 1 }, 
            { -1, 1, -1 } };

        public static int[,] Kernel_Morphology_Erosion = new int[,] { 
            { 1, 1, 1 }, 
            { 1, 1, 1 }, 
            { 1, 1, 1 } };

        public static int[,] Kernel_Morphology_Dilation = new int[,] { 
            { 0, 0, 0 }, 
            { 0, 0, 0 }, 
            { 0, 0, 0 } };

        #endregion

        public static int GetKernelDivisor(int[,] kernel)
        {
            int divisor = 0;
            for (int i = 0, n = kernel.GetLength(0); i < n; i++)
            {
                for (int j = 0, k = kernel.GetLength(1); j < k; j++)
                {
                    divisor += kernel[i, j];
                }
            }
            if (divisor == 0)
                divisor = 1;
            return divisor;
        }

        public static Rectangle GetFullRectImage(GreyDataImage image)
        {
            return new Rectangle(new Point(0, 0), new Size(image._width, image._height));
        }

        public static GreyDataImage Median(GreyDataImage image, int kernelsize, Rectangle rect)
        {
            GreyDataImage image_New = new GreyDataImage(image._width, image._height);

            int pixelSize = 1;

            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = startX + rect.Width;
            int stopY = startY + rect.Height;

            int srcStride = image._width;
            int dstStride = image_New._width;
            int srcOffset = srcStride - rect.Width * pixelSize;
            int dstOffset = dstStride - rect.Width * pixelSize;


            ushort[] g = new ushort[kernelsize * kernelsize];
            // loop and array indexes
            int i, j, t;
            // processing square's radius
            int radius = kernelsize >> 1;
            // number of elements
            int c;

            unsafe
            {

                ushort* src = image._aData;
                ushort* dst = image_New._aData;

                // for each line
                for (int y = startY; y < stopY; y++)
                {
                    // for each pixel
                    for (int x = startX; x < stopX; x++, src++, dst++)
                    {
                        c = 0;

                        // for each kernel row
                        for (i = -radius; i <= radius; i++)
                        {
                            t = y + i;

                            // skip row
                            if (t < startY)
                                continue;
                            // break
                            if (t >= stopY)
                                break;

                            // for each kernel column
                            for (j = -radius; j <= radius; j++)
                            {
                                t = x + j;

                                // skip column
                                if (t < startX)
                                    continue;

                                if (t < stopX)
                                {
                                    g[c++] = src[i * srcStride + j];
                                }
                            }
                        }
                        // sort elements
                        Array.Sort(g, 0, c);
                        // get the median
                        *dst = g[c >> 1];
                    }
                    src += srcOffset;
                    dst += dstOffset;
                }
            }

            return image_New;
        }

        public static GreyDataImage Convolution(GreyDataImage image, int[,] kernel)
        {
            return Convolution(image, kernel, GetFullRectImage(image));
        }

        public static GreyDataImage Convolution(GreyDataImage image, int[,] kernel, Rectangle rect)
        {
            GreyDataImage image_New = new GreyDataImage(image._width, image._height);

            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = rect.Left + rect.Width;
            int stopY = rect.Top + rect.Height;

            int srcStride = image._width;
            int dstStride = image_New._width;
            int srcOffset = srcStride - rect.Width;
            int dstOffset = dstStride - rect.Width;

            int size = kernel.GetLength(0);
            int kernelSize = kernel.Length;

            // loop and array indexes
            int i, j, temp, k, ir, jr;
            // kernel's radius
            int radius = size >> 1;
            long gray, div;

            unsafe
            {
                ushort* src = image._aData;
                ushort* dst = image_New._aData;
                for (int y = startY; y < stopY; y++)
                {
                    for (int x = startX; x < stopX; x++, src++, dst++)
                    {
                        gray = div = 0;

                        for (i = 0; i < size; i++)
                        {
                            ir = i - radius;
                            temp = y + ir;

                            if (temp < startY) continue;
                            if (temp >= stopY) break;

                            for (j = 0; j < size; j++)
                            {
                                jr = j - radius;
                                temp = x + jr;

                                if (temp < startX) continue;
                                if (temp < stopX)
                                {
                                    k = kernel[i, j];
                                    div += k;
                                    gray += k * src[ir * srcStride + jr];
                                }
                            }
                        }

                        if (div != 0)
                            gray /= div;
                        *dst = (ushort)((gray > 255) ? 255 : ((gray < 0) ? 0 : gray));

                    }
                    src += srcOffset;
                    dst += dstOffset;
                }
            }
            return image_New;
        }

        public static GreyDataImage ThresholdBinary(GreyDataImage image, int threshold)
        {
            return ThresholdBinary(image, threshold, false, GetFullRectImage(image));
        }

        public static GreyDataImage ThresholdBinary(GreyDataImage image, int threshold, bool invert, Rectangle rect)
        {
            GreyDataImage image_New = new GreyDataImage(image._width, image._height);

            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = rect.Left + rect.Width;
            int stopY = rect.Top + rect.Height;

            int srcStride = image._width;
            int dstStride = image_New._width;
            int srcOffset = srcStride - rect.Width;
            int dstOffset = dstStride - rect.Width;

            unsafe
            {
                ushort* src = image._aData;
                ushort* dst = image_New._aData;
                for (int y = startY; y < stopY; y++)
                {
                    for (int x = startX; x < stopX; x++, src++, dst++)
                    {
                        *dst = (ushort)(invert ?
                                                ((*src > threshold) ? 0 : 255) : 
                                                ((*src > threshold) ? 255 : 0));
                    }
                    src += srcOffset;
                    dst += dstOffset;
                }
            }

            return image_New;
        }

        public static GreyDataImage MorphologicalOperator(GreyDataImage image, int[,] kernel, MorphologyModes mode)
        {
            return MorphologicalOperator(image, kernel, mode, GetFullRectImage(image));
        }

        public static GreyDataImage MorphologicalOperator(GreyDataImage image, int[,] kernel, MorphologyModes mode, Rectangle rect)
        {
            GreyDataImage image_New = new GreyDataImage(image._width, image._height);

            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = rect.Left + rect.Width;
            int stopY = rect.Top + rect.Height;

            int srcStride = image._width;
            int dstStride = image_New._width;
            int srcOffset = srcStride - rect.Width;
            int dstOffset = dstStride - rect.Width;

            int size = kernel.GetLength(0);
            int kernelSize = kernel.Length;

            ushort[] hitValue = new ushort[3] { 255, 0, 255 };
            ushort[] missValue = new ushort[3] { 0, 0, 0 };
            int modeIndex = (int)mode;

            // loop and array indexes
            int i, j, ir, jr, sv;
            // pixel value
            ushort gray, v;
            // kernel's radius
            int radius = size >> 1;

            unsafe
            {
                ushort* src = image._aData;
                ushort* dst = image_New._aData;
                // for each line
                for (int y = startY; y < stopY; y++)
                {
                    // for each pixel
                    for (int x = startX; x < stopX; x++, src++, dst++)
                    {
                        missValue[1] = missValue[2] = *src;
                        gray = 255;

                        // for each structuring element's row
                        for (i = 0; i < size; i++)
                        {
                            ir = i - radius;

                            // for each structuring element's column
                            for (j = 0; j < size; j++)
                            {
                                jr = j - radius;

                                // get structuring element's value
                                sv = kernel[i, j];

                                // skip "don't care" values
                                if (sv == -1)
                                    continue;

                                // check, if we outside
                                if (
                                    (y + ir < startY) || (y + ir >= stopY) ||
                                    (x + jr < startX) || (x + jr >= stopX)
                                    )
                                {
                                    // if it so, the result is zero,
                                    // because it was required pixel
                                    gray = 0;
                                    break;
                                }

                                // get source image value
                                v = src[ir * srcStride + jr];

                                if (
                                    ((sv != 0) || (v != 0)) &&
                                    ((sv != 1) || (v != 255))
                                    )
                                {
                                    // failed structuring element mutch
                                    gray = 0;
                                    break;
                                }
                            }

                            if (gray == 0)
                                break;
                        }
                        // result pixel
                        *dst = (gray == 255) ? hitValue[modeIndex] : missValue[modeIndex];
                    }
                    src += srcOffset;
                    dst += dstOffset;
                }
            }

            return image_New;
        }

        public static GreyDataImage Subtraction(GreyDataImage image1, GreyDataImage image2, bool absolute, Rectangle rect)
        {
            if (image1._width != image2._width
                || image1._height != image2._height)
                return image1;

            GreyDataImage imgSub = new GreyDataImage(image1._width, image1._height);

            int startX = rect.Left;
            int startY = rect.Top;
            int stopX = rect.Left + rect.Width;
            int stopY = rect.Top + rect.Height;

            int stride = image1._width;
            int offset = stride - rect.Width;

            // pixel value
            ushort gray = 0;

            unsafe
            {
                ushort* src = image1._aData;
                ushort* dst = image2._aData;
                ushort* sub = imgSub._aData;
                // for each line
                for (int y = startY; y < stopY; y++)
                {
                    // for each pixel
                    for (int x = startX; x < stopX; x++, src++, dst++, sub++)
                    {
                        if (absolute && *src < *dst)
                        {
                            gray = (ushort)(*dst - *src);
                            gray -= *src;
                        }
                        else
                        {
                            gray = (ushort)(*src - *dst);
                            gray -= *dst;
                        }
                        *sub = (ushort)((gray < 0) ? 0 : gray);
                    }
                    src += offset;
                    dst += offset;
                    sub += offset;
                }
            }

            return imgSub;
        }

    }
}
