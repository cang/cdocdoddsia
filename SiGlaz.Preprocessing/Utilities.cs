using System;
using System.Collections.Generic;
using System.Text;

namespace SiGlaz.Preprocessing
{
    public class Utilities
    {
        unsafe public static void CalcImageIntegrals(
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
    }
}
