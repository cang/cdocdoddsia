using System;
using System.Collections.Generic;
using System.Text;

namespace SIA.Algorithms
{
    public class SimpleImageUtilities
    {
        unsafe public static void Copy(
            ushort* dst, int dstWidth, int dstHeight, 
            int l, int t, 
            int* src, int srcWidth, int srcHeight)
        {
            int r = l + srcWidth - 1;
            r = (r < dstWidth ? r : dstWidth - 1);
            int b = t + srcHeight;
            b = (b < dstHeight ? b : dstHeight - 1);

            int dstIndex = 0, srcIndex = 0;
            for (int y = t; y <= b; y++)
            {
                dstIndex = y * dstWidth + l;
                srcIndex = (y - t) * srcWidth;
                for (int x = l; x <= r; x++, dstIndex++, srcIndex++)
                {
                    *(dst + dstIndex) = (ushort)*(src + srcIndex);
                }
            }
        }
    }
}
