using System;
using System.Collections.Generic;
using System.Text;

namespace SiGlaz.Common.ImageAlignment
{
    public class ABSAlignmentSettings : AlignmentSettings
    {
        public const string DefaultFileName_ABS = "ABSAlignment.settings";

        public new static int CurrentVersion
        {
            get
            {
                return 2;
            }
        }

        public double IntensityThreshold_High = 200;

        public ABSAlignmentSettings() : base()
        {
            KeyRows = new int[] {400, 420, 440, 460, 480, 500, 
                520, 540, 560, 580, 600, 620, 640, 660, 680, 700, 720, 740, 760, 780, 800, 820, 840, 860, 880, 900, 920, 940, 960, 980, 
                1000, 1020, 1040, 1060, 1080, 1100, 1120, 1140, 1160, 1180, 1200, 1220, 1240, 1260, 1280, 1300, 1320, 1340, 1360, 1380, 
                1400, 1420, 1440, 1460, 1480, 1500, 1520, 1540, 1560, 1580, 1600, 1620, 1640, 1660, 1680, 1700};
        }

        public override string DefaultFileName
        {
            get
            {
                return DefaultFileName_ABS;
            }
        }

        #region Serialize and deserialize
        protected new void BaseSerialize(System.IO.BinaryWriter bin)
        {
            base.BaseSerialize(bin); bin.Write(CurrentVersion);

            bin.Write(IntensityThreshold_High);
        }

        protected new void BinDeserialize(System.IO.BinaryReader bin)
        {
            base.BinDeserialize(bin); int version = bin.ReadInt32();

            IntensityThreshold_High = bin.ReadDouble();
        }

        public new static ABSAlignmentSettings Deserialize(string filename)
        {
            return ParameterBase.BaseDeserialize(filename) as ABSAlignmentSettings;
        }
        #endregion Serialize and deserialize
    }
}
