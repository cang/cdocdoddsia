using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using SIA.IPEngine;
using SIA.SystemLayer;

namespace SiGlaz.Preprocessing.Interpolation
{
    public enum InterpolationMethod
    {
        None,
        NearestNeibourgh,
        Bilinear,
        Bicubic
    }

    public class ImageInterpolator
    {
        public static GreyDataImage AffineTransform(InterpolationMethod method, GreyDataImage image, PointF[] source, int newWidth, int newHeight,
            ref System.Drawing.Drawing2D.Matrix inverseTransform)
        {
            RectangleF dest = new RectangleF(0, 0, newWidth, newHeight);
            inverseTransform = new System.Drawing.Drawing2D.Matrix(dest, source);
            MyMatrix transform = new MyMatrix(inverseTransform);

            GreyDataImage result = new GreyDataImage(newWidth, newHeight);
            switch (method)
            {
                case InterpolationMethod.NearestNeibourgh:
                    throw new Exception("Not implemented yet");
                    break;

                case InterpolationMethod.Bilinear:
                    BilinearInterpolate(image, transform, result);
                    break;

                case InterpolationMethod.Bicubic:
                    throw new Exception("Not implemented yet");
                    break;

                default:
                    throw new Exception("Invalid interpolation method");

            }

            return result;
        }

        public static void AffineTransform(InterpolationMethod method, GreyDataImage image, 
            System.Drawing.Drawing2D.Matrix transform, GreyDataImage output)
        {
            MyMatrix mytransform = new MyMatrix(transform);
            
            switch (method)
            {
                case InterpolationMethod.NearestNeibourgh:
                    throw new Exception("Not implemented yet");
                    break;

                case InterpolationMethod.Bilinear:
                    BilinearInterpolate(image, mytransform, output);
                    break;

                case InterpolationMethod.Bicubic:
                    throw new Exception("Not implemented yet");
                    break;

                default:
                    throw new Exception("Invalid interpolation method");

            }
        }

        public unsafe static void AffineTransform(InterpolationMethod method, ushort* srcData, int srcWidth, int srcHeight,
             System.Drawing.Drawing2D.Matrix transform, ushort* destData, int newWidth, int newHeight)
        {
            MyMatrix mytransform = new MyMatrix(transform);

            switch (method)
            {
                case InterpolationMethod.NearestNeibourgh:
                    throw new Exception("Not implemented yet");
                    break;

                case InterpolationMethod.Bilinear:
                    BilinearInterpolate(srcData, srcWidth, srcHeight, mytransform, 
                        destData, newWidth, newHeight);
                    break;

                case InterpolationMethod.Bicubic:
                    throw new Exception("Not implemented yet");
                    break;

                default:
                    throw new Exception("Invalid interpolation method");

            }
        }

        private unsafe static void BilinearInterpolate(GreyDataImage image, MyMatrix transform, GreyDataImage result)
        {
            ushort* srcData = image._aData;
            ushort* destData = result._aData;
            
            //double dx1, dx2, dy1, dy2;
            //int x, y;            

            int newHeight = result._height;
            int newWidth = result._width;
            int srcWidth = image._width;
            int srcHeight = image._height;
            int srcWidth1 = srcWidth - 1;
            int srcHeight1 = srcHeight - 1;

            //pDestData = destData;
            //for (y = 0; y < newHeight; y++)
            Parallel.For(0, newHeight, delegate(int y)
            {
                double dx1, dx2, dy1, dy2;
                int x;

                ushort* pDestData = null;
                ushort* pSourceData = null;

                pDestData = (destData + y * newWidth);

                for (x = 0; x < newWidth; x++, pDestData++)
                {
                    float orgX = x + 0.5f, orgY = y + 0.5f;
                    transform.TransformPoints(ref orgX, ref orgY);

                    int ix = (int)Math.Floor(orgX);
                    int iy = (int)Math.Floor(orgY);

                    if (ix < 1)
                        ix = 1;
                    else if (ix >= srcWidth1)
                        ix = srcWidth1;

                    if (iy < 1)
                        iy = 1;
                    else if (iy >= srcHeight1)
                        iy = srcHeight1;

                    if (ix == 1 || ix == srcWidth1 || iy == 0 || iy == srcHeight1)
                    {
                        *pDestData = *(srcData + iy * srcWidth + ix);
                        continue;
                    }

                    dx2 = orgX - ix;
                    dx1 = 1.0 - dx2;

                    dy2 = orgY - iy;
                    dy1 = 1.0 - dy2;

                    pSourceData = srcData + iy * srcWidth + ix;
                    float p11 = *(pSourceData);
                    float p12 = *(pSourceData + 1);
                    float p21 = *(pSourceData + srcWidth);
                    float p22 = *(pSourceData + srcWidth + 1);
                    *pDestData = (ushort)(p11 * dy1 * dx1 + p12 * dy1 * dx2 + p21 * dy2 * dx1 + p22 * dy2 * dx2);
                }
            });
        }

        private unsafe static void BilinearInterpolate(ushort* srcData, int srcWidth, int srcHeight,
            MyMatrix transform, ushort* destData, int newWidth, int newHeight)
        {
            ushort* pDestData = null;
            ushort* pSourceData = null;

            double dx1, dx2, dy1, dy2;

            int x, y;

            int srcWidth1 = srcWidth - 1;
            int srcHeight1 = srcHeight - 1;

            pDestData = destData;
            for (y = 0; y < newHeight; y++)
            {
                for (x = 0; x < newWidth; x++, pDestData++)
                {
                    float orgX = x + 0.5f, orgY = y + 0.5f;
                    transform.TransformPoints(ref orgX, ref orgY);

                    int ix = (int)Math.Floor(orgX);
                    int iy = (int)Math.Floor(orgY);

                    if (ix < 1)
                        ix = 1;
                    else if (ix >= srcWidth1)
                        ix = srcWidth1;

                    if (iy < 1)
                        iy = 1;
                    else if (iy >= srcHeight1)
                        iy = srcHeight1;

                    if (ix == 1 || ix == srcWidth1 || iy == 1 || iy == srcHeight1)
                    {
                        *pDestData = *(srcData + iy * srcWidth + ix);
                        continue;
                    }

                    dx2 = orgX - ix;
                    dx1 = 1.0 - dx2;

                    dy2 = orgY - iy;
                    dy1 = 1.0 - dy2;

                    pSourceData = srcData + iy * srcWidth + ix;
                    float p11 = *(pSourceData);
                    float p12 = *(pSourceData + 1);
                    float p21 = *(pSourceData + srcWidth);
                    float p22 = *(pSourceData + srcWidth + 1);
                    *pDestData = (ushort)(p11 * dy1 * dx1 + p12 * dy1 * dx2 + p21 * dy2 * dx1 + p22 * dy2 * dx2);
                }
            }
        }

    }
}
