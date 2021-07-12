using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace SIA.Algorithms.Preprocessing.Matching
{
    unsafe public abstract class SimpleMatcher : IMatcher
    {
        protected int _sampleWidth = 0;
        protected int _sampleHeight = 0;        
        protected ushort* _sampleData = null;

        protected int _patternWidth = 0;
        protected int _patternHeight = 0;
        protected ushort* _patternData = null;

        protected Rectangle _roi = new Rectangle();


        public unsafe void Match(
            ushort* sampleData, int widthSample, int heightSample,
            ushort* patternData, int widthPattern, int heightPattern,
            Rectangle roi,
            double threshold, out double confidence, out int offsetX, out int offsetY)
        {
            _roi = roi;
            Match(
                sampleData, widthSample, heightSample, 
                patternData, widthPattern, heightPattern,
                threshold, out confidence, out offsetX, out offsetY);
        }

        public unsafe void Match(
            ushort* sampleData, int widthSample, int heightSample, 
            ushort* patternData, int widthPattern, int heightPattern, 
            double threshold, out double confidence, out int offsetX, out int offsetY)
        {
            _patternWidth = widthPattern;
            _patternHeight = heightPattern;
            _patternData = patternData;

            _sampleWidth = widthSample;
            _sampleHeight = heightSample;
            _sampleData = sampleData;

            confidence = 0;
            offsetX = 0;
            offsetY = 0;

            Match(ref confidence, ref offsetX, ref offsetY);
        }

        protected abstract void Match(
            ref double confidence, ref int offsetX, ref int offsetY);

        protected virtual int[] CalcItemOffsets(
            int stride, int kernelWidth, int kernelHeight)
        {
            int halfWidth = kernelWidth >> 1;
            int halfHeight = kernelHeight >> 1;

            int[] offsets = new int[kernelWidth * kernelHeight];

            int index = 0;
            for (int y = -halfHeight; y <= halfHeight; y++)
            {
                for (int x = -halfWidth; x <= halfWidth; x++, index++)
                {
                    offsets[index] = y * stride + x;
                }
            }

            return offsets;
        }
    }
}
