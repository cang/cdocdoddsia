using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace SIA.SystemLayer.ImageProcessing
{
    //public class SimpleImageIO
    //{
    //    unsafe public static bool Load(
    //        string filePath, ref byte[] data, ref int width, ref int height)
    //    {
    //        using (Image image = Image.FromFile(filePath))
    //        {
    //            return Load(image, ref data, ref width, ref height);
    //        }
    //    }

    //    unsafe public static bool Load(
    //        Image image, ref byte[] data, ref int width, ref int height)
    //    {
    //        PixelFormat pxFormat = image.PixelFormat;

    //        GraphicsUnit unit = GraphicsUnit.Pixel;
    //        RectangleF boundsF = image.GetBounds(ref unit);

    //        Rectangle bounds = new Rectangle(
    //            (int)boundsF.X, (int)boundsF.Y, (int)boundsF.Width, (int)boundsF.Height);

    //        BitmapData bitmapData =
    //            (image as Bitmap).LockBits(bounds, ImageLockMode.ReadWrite, pxFormat);

    //        try
    //        {
    //            width = image.Width;
    //            height = image.Height;
    //            int length = width * height;

    //            if (data == null || data.Length != length)
    //                data = new byte[length];

    //            byte[] dst = (byte[])data;

    //            int realWidth = (int)Math.Abs(bitmapData.Stride);
    //            int bytenum = realWidth / width;
    //            int reserved = realWidth - bytenum * width;

    //            byte* src = (byte*)bitmapData.Scan0.ToPointer();
    //            {
    //                int index = 0;
    //                switch (bytenum)
    //                {
    //                    case 1:
    //                        for (int y = 0; y < height; y++)
    //                        {
    //                            for (int x = 0; x < width; x++, index++, src++)
    //                            {
    //                                dst[index] = (byte)(*(src));
    //                            }
    //                            src += reserved;
    //                        }
    //                        break;
    //                    case 3:
    //                        for (int y = 0; y < height; y++)
    //                        {
    //                            for (int x = 0; x < width; x++, index++, src += 3)
    //                            {
    //                                dst[index] = (byte)((*(src) * 29 + *(src + 1) * 150 + *(src + 2) * 77 + 128) / 256);
    //                            }
    //                            src += reserved;
    //                        }
    //                        break;
    //                    case 4:
    //                        for (int y = 0; y < height; y++)
    //                        {
    //                            for (int x = 0; x < width; x++, index++, src += 4)
    //                            {
    //                                dst[index] = (byte)((*(src) * 29 + *(src + 1) * 150 + *(src + 2) * 77 + 128) / 256);
    //                            }
    //                            src += reserved;
    //                        }
    //                        break;
    //                }
    //            }
    //        }
    //        catch
    //        {
    //            throw;
    //        }
    //        finally
    //        {
    //            if (image != null && bitmapData != null)
    //            {
    //                (image as Bitmap).UnlockBits(bitmapData);
    //            }
    //        }

    //        return true;
    //    }

    //    unsafe public static bool Load(
    //        string filePath, 
    //        ref byte[] regionData, int l, int t,
    //        int regionWidth, int regionHeight)
    //    {
    //        using (Image image = Image.FromFile(filePath))
    //        {
    //            return Load(image, ref regionData, l, t, regionWidth, regionHeight);
    //        }
    //    }

    //    unsafe public static bool Load(
    //        Image image, 
    //        ref byte[] regionData, int l, int t,
    //        int regionWidth, int regionHeight)
    //    {
    //        PixelFormat pxFormat = image.PixelFormat;

    //        GraphicsUnit unit = GraphicsUnit.Pixel;
    //        RectangleF boundsF = image.GetBounds(ref unit);

    //        Rectangle bounds = new Rectangle(
    //            (int)boundsF.X, (int)boundsF.Y, (int)boundsF.Width, (int)boundsF.Height);

    //        BitmapData bitmapData =
    //            (image as Bitmap).LockBits(bounds, ImageLockMode.ReadWrite, pxFormat);

    //        try
    //        {
    //            int regionDataLength = regionWidth * regionHeight;

    //            int width = image.Width;
    //            int height = image.Height;

    //            if (regionData == null || regionData.Length != regionDataLength)
    //                regionData = new byte[regionDataLength];

    //            byte[] dst = (byte[])regionData;

    //            int realWidth = (int)Math.Abs(bitmapData.Stride);
    //            int bytenum = realWidth / width;
    //            int reserved = realWidth - bytenum * width;

    //            int bottom = t + regionHeight - 1;
    //            bottom = (bottom < height ? bottom : height-1);
    //            int right = l + regionWidth - 1;
    //            right = (right < width ? right : width - 1);

    //            byte* src = (byte*)bitmapData.Scan0.ToPointer();
    //            {                                        
    //                int offsetLeft = l * bytenum;
    //                byte* orgSrcTmp = src;
    //                int index = 0;
    //                switch (bytenum)
    //                {
    //                    case 1:
    //                        for (int y = t; y <= bottom; y++)
    //                        {
    //                            index = (y - t) * regionWidth;
    //                            src = orgSrcTmp + y * realWidth + offsetLeft;
    //                            for (int x = l; x <= right; x++, index++, src++)
    //                            {
    //                                dst[index] = (byte)(*(src));
    //                            }
    //                        }
    //                        break;

    //                    case 3:
    //                        for (int y = t; y <= bottom; y++)
    //                        {
    //                            index = (y - t) * regionWidth;
    //                            src = orgSrcTmp + y * realWidth + offsetLeft;
    //                            for (int x = l; x <= right; x++, index++, src += 3)
    //                            {
    //                                dst[index] = (byte)((*(src) * 29 + *(src + 1) * 150 + *(src + 2) * 77 + 128) / 256);
    //                            }
    //                        }
    //                        break;
    //                    case 4:
    //                        for (int y = t; y <= bottom; y++)
    //                        {
    //                            index = (y - t) * regionWidth;
    //                            src = orgSrcTmp + y * realWidth + offsetLeft;
    //                            for (int x = l; x <= right; x++, index++, src += 4)
    //                            {
    //                                dst[index] = (byte)((*(src) * 29 + *(src + 1) * 150 + *(src + 2) * 77 + 128) / 256);
    //                            }
    //                        }
    //                        break;
    //                }
    //                orgSrcTmp = null;
    //            }
    //        }
    //        catch
    //        {
    //            throw;
    //        }
    //        finally
    //        {
    //            if (image != null && bitmapData != null)
    //            {
    //                (image as Bitmap).UnlockBits(bitmapData);
    //            }
    //        }

    //        return true;
    //    }
    //}
}
