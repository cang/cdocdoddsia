using System;
using System.Collections.Generic;
using System.Text;

namespace SiGlaz.Common.Pattern
{
    public class CorrelationPattern : ParameterBase
    {
        public new int CurrentVersion
        {
            get
            {
                return 1;
            }
        }

        public int Width = 0;
        public int Height = 0;
        public int ExpandWidth = 0;
        public int ExpandHeight = 0;
        public int Top = 0;
        public int Left = 0;
        public ushort[] Data = null;
        public double[] WeightSet = null;
        public double Mean = 0.0;
        public double Std = 0.0;

        public void PrepareDefaultWeightSet()
        {
            if (WeightSet != null)
                return;
            int len = Data.Length;
            WeightSet = new double[len];
            for (int i = 0; i < len; i++)
                WeightSet[i] = 1.0;
        }

        public unsafe void CreateWeightSet(ushort* data, int width, int height, ushort thres, int vote)
        {
            if (width != Width || height != Height)
            {
                PrepareDefaultWeightSet();
                return;
            }
            WeightSet = new double[width * height];
            ushort* pData = data;
            int wsIdx = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++, pData++, wsIdx++)
                {
                    if (*pData < thres)
                        WeightSet[wsIdx] = vote;
                    else
                        WeightSet[wsIdx] = 1;
                }
            }
        }

        public void ComputeMeanStd()
        {
            if (Mean != 0.0 && Std != 0.0)
                return;

            double lenInv = 1.0 / Data.Length;
            double mean = 0;
            double ss = 0;

            for (int i = 0; i < Data.Length; i++)
            {
                double temp = Data[i];
                Mean += temp * lenInv;
                ss += temp * temp;
            }
            Std = Math.Sqrt((ss * lenInv - mean * mean));
            Mean = mean;
        }

        protected override void BaseSerialize(System.IO.BinaryWriter bin)
        {
            PrepareDefaultWeightSet();
            ComputeMeanStd();

            base.BaseSerialize(bin); bin.Write(CurrentVersion);

            bin.Write(Width);
            bin.Write(Height);
            bin.Write(ExpandWidth);
            bin.Write(ExpandHeight);
            bin.Write(Top);
            bin.Write(Left);
            BinarySerializationCommon.Write(bin, Data);
            BinarySerializationCommon.Write(bin, WeightSet);
            bin.Write(Mean);
            bin.Write(Std);
        }

        protected override void BinDeserialize(System.IO.BinaryReader bin)
        {
            base.BinDeserialize(bin); int version = bin.ReadInt32();

            Width = bin.ReadInt32();
            Height = bin.ReadInt32();
            ExpandWidth = bin.ReadInt32();
            ExpandHeight = bin.ReadInt32();
            Top = bin.ReadInt32();
            Left = bin.ReadInt32();
            Data = BinarySerializationCommon.ReadUshortArray(bin);
            WeightSet = BinarySerializationCommon.ReadDoubleArray(bin);
            Mean = bin.ReadDouble();
            Std = bin.ReadDouble();
        }

        public new static CorrelationPattern Deserialize(System.IO.BinaryReader bin)
        {
            return ParameterBase.BaseDeserialize(bin) as CorrelationPattern;
        }
    }
}

