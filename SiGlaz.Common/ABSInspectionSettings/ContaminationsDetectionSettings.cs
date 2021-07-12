using System;
using System.Collections.Generic;
using System.Text;

namespace SiGlaz.Common.ABSInspectionSettings
{
    public class ContaminationsDetectionSettings : ParameterBase
    {
        public const string DefaultFileName = "ContaminationsDetectionSettings.settings";

        public new static int CurrentVersion
        {
            get
            {
                return 1;
            }
        }

        public double LowerIntensityThreshold = 90;
        public double HigherIntensityThreshold = 150;

        public double LowerIntensityThreshold_PoleTip = 30;
        public double HigherIntensityThreshold_PoleTip = 220;

        public int SampleSize = 65;
        public int PoleX = 2190;
        public int PoleY = 645;
        public int PoleWidth = 2235 - 2190 + 1;
        public int PoleHeight = 755 - 645 + 1;

        protected override void BaseSerialize(System.IO.BinaryWriter bin)
        {
            base.BaseSerialize(bin); bin.Write(CurrentVersion);
            bin.Write(LowerIntensityThreshold);
            bin.Write(HigherIntensityThreshold);
            bin.Write(SampleSize);
            bin.Write(PoleX);
            bin.Write(PoleY);
            bin.Write(PoleWidth);
            bin.Write(PoleHeight);
        }

        protected override void BinDeserialize(System.IO.BinaryReader bin)
        {
            base.BinDeserialize(bin); int version = bin.ReadInt32();

            LowerIntensityThreshold = bin.ReadDouble();
            HigherIntensityThreshold = bin.ReadDouble();

            SampleSize = bin.ReadInt32();
            PoleX = bin.ReadInt32();
            PoleY = bin.ReadInt32();
            PoleWidth = bin.ReadInt32();
            PoleHeight = bin.ReadInt32();

            PoleX -= 15;
            PoleWidth += 15;
            PoleY = 0;
            PoleHeight = 1380;
        }

        public static ContaminationsDetectionSettings Deserialize(string filename)
        {
            return ParameterBase.BaseDeserialize(filename) as ContaminationsDetectionSettings;
        }
    }
}
