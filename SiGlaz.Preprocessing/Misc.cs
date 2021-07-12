using System;
using System.Collections.Generic;
using System.Text;

using SIA.IPEngine;

namespace SiGlaz.Preprocessing
{
    public class Misc
    {
        public static unsafe void GetData(GreyDataImage image, int x, int y, int width, int height, ushort[] output)
        {
            ushort* src = image._aData;
            int stride = image._width;
            ushort* pSrc = null, pDest = null;
            fixed (ushort* dest = output)
            {
                pDest = dest;
                for (int i = 0; i < height; i++)
                {
                    pSrc = src + (y + i) * stride + x;
                    for (int j = 0; j < width; j++, pDest++, pSrc++)
                    {
                        *pDest = *pSrc;
                    }
                }
            }
        }

        public static unsafe void GetData(GreyDataImage image, int x, int y, int width, int height, ushort[] output, 
            ref double mean, ref double std)
        {
            mean = 0;
            double ss = 0;
            ushort* src = image._aData;
            int stride = image._width;
            double len = (width * height);
            double lenInv = 1.0 / len;
            ushort* pSrc = null, pDest = null;
            fixed (ushort* dest = output)
            {
                pDest = dest;
                for (int i = 0; i < height; i++)
                {
                    pSrc = src + (y + i) * stride + x;
                    for (int j = 0; j < width; j++, pDest++, pSrc++)
                    {
                        *pDest = *pSrc;
                        double temp = *pDest;
                        mean += temp * lenInv;
                        ss += temp * temp;
                    }
                }
            }

            std = Math.Sqrt((ss * lenInv - mean * mean));
        }

    }
}
