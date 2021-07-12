using System;
using System.Collections.Generic;
using System.Text;

namespace SiGlaz.Common.ABSInspectionSettings
{
    public class TexturalExtractorSettings : ParameterBase
    {
        public const string DefaultFileName = "TexturalExtractorSettings.settings";

        public new static int CurrentVersion
        {
            get
            {
                return 1;
            }
        }
        
        public int Padding = 1;
        public int KernelSize = 23;        
        public double JumpStepFactor = 0.85;
        public int GreyLevel = 64;
        public int[] OffsetXList = new int[] { 1, 0 };
        public int[] OffsetYList = new int[] { 1, 1 };
        public int ABSImageWidth = 0;
        public int ABSImageHeight = 0;

        public int PaddingLeft
        {
            get
            {
                int x = Min(OffsetXList);
                if (x >= 0)
                    x = 0;

                x = KernelSize / 2 - x;

                return x;
            }
        }

        public int PaddingRight
        {
            get
            {
                int x = Max(OffsetXList);
                if (x <= 0)
                    x = 0;
                
                x = KernelSize / 2 + x;

                return x;
            }
        }

        public int PaddingTop
        {
            get
            {
                int y = Min(OffsetYList);
                if (y >= 0)
                    y = 0;

                y = KernelSize / 2 - y;

                return y;
            }
        }

        public int PaddingBottom
        {
            get
            {
                int y = Max(OffsetYList);
                if (y <= 0)
                    y = 0;

                y = KernelSize / 2 + y;

                return y;
            }
        }

        private int Min(int[] a)
        {
            int min = a[0];
            for (int i=a.Length-1; i>=1; i--)
                if (min > a[i])
                    min = a[i];
            return min;
        }

        private int Max(int[] a)
        {
            int max = a[0];
            for (int i = a.Length - 1; i >= 1; i--)
                if (max < a[i])
                    max = a[i];
            return max;
        }

        protected override void BaseSerialize(System.IO.BinaryWriter bin)
        {
            base.BaseSerialize(bin); bin.Write(CurrentVersion);
            bin.Write(Padding);
            bin.Write(KernelSize);
            bin.Write(JumpStepFactor);
            bin.Write(GreyLevel);
            BinarySerializationCommon.Write(bin, OffsetXList);
            BinarySerializationCommon.Write(bin, OffsetYList);
            bin.Write(ABSImageWidth);
            bin.Write(ABSImageHeight);
        }

        protected override void BinDeserialize(System.IO.BinaryReader bin)
        {
            base.BinDeserialize(bin); int version = bin.ReadInt32();

            Padding = bin.ReadInt32();
            KernelSize = bin.ReadInt32();
            JumpStepFactor = bin.ReadDouble();
            GreyLevel = bin.ReadInt32();
            OffsetXList = BinarySerializationCommon.ReadIntArray(bin);
            OffsetYList = BinarySerializationCommon.ReadIntArray(bin);
            ABSImageWidth = bin.ReadInt32();
            ABSImageHeight = bin.ReadInt32();
        }

        public static TexturalExtractorSettings Deserialize(string filename)
        {
            return ParameterBase.BaseDeserialize(filename) as TexturalExtractorSettings;
        }
    }
}
