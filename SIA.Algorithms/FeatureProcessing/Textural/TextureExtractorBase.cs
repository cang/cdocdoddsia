using System;
using System.Collections.Generic;
using System.Text;

namespace SIA.Algorithms.FeatureProcessing.Textural
{
    public abstract class TextureExtractorBase
    {
        public int[] CoGrayMatrix = null;
        public double[] CoGrayVotes = null;
        public int[] FirstGrays = null;
        public int[] SecondGrays = null;
        public int CoGrayItemCount = 0;

        protected double _invGrayLevel = 0;
        protected double _invSqrGrayLevel = 0;

        protected TextureExtractorBase()
        {
        }

        protected TextureExtractorBase(
            int kernelWidth, int kernelHeight, int grayLevel)
            : this()
        {
            _invGrayLevel = 1.0 / grayLevel;
            _invSqrGrayLevel = _invGrayLevel * _invGrayLevel;

            CoGrayMatrix = new int[grayLevel * grayLevel];

            int length = kernelWidth * kernelHeight + 1;

            CoGrayVotes = new double[length];
            FirstGrays = new int[length];
            SecondGrays = new int[length];
        }

        public abstract void Extract(double[] textures);

        protected abstract void ExtractForTrivialCase(double[] textures);

        protected abstract void NormalizeAndUpateFeatures(double[] textures);

        protected abstract void ResetWorkingSpace();

        public abstract int FeatureCount { get; }
    }
}
