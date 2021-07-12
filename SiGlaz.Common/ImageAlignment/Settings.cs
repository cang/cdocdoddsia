using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SiGlaz.Common.ImageAlignment
{
    public class Settings : ParameterBase
    {
        public bool IsABSAlignmentSettings = true;
        public double ExpandedRight
        {
            get
            {
                if (IsABSAlignmentSettings)
                    return 0;
                return 115;
            }
        }


        public const string DefaultFileName_ABS         = "Alignment.settings";
        public const string DefaultFileName_PoleTip     = "Alignment_PoleTip.settings";

        public new static int CurrentVersion
        {
            get
            {
                return 6;
            }
        }

        public double IntensityThreshold = 50;                  //ABS = 50, PoleTip = 200
        public double MinCoorelationCoefficient = 0.5;          //ABS = 0.5, PoleTip = 0.5
        public int MinAffectedKeypoints = 8;                    //ABS = 8, PoleTip = 4
        public int[] KeyColumns = new int[] {160, 180, 200, 220, 240, 260, 280, 300, 320, 340, 360, 380, 400, 420, 440, 460, 480, 500, 
                520, 540, 560, 580, 600, 620, 640, 660, 680, 700, 720, 740, 760, 780, 800, 820, 840, 860, 880, 900, 920, 940, 960, 980, 
                1000, 1020, 1040, 1060, 1080, 1100, 1120, 1140, 1160, 1180, 1200, 1220, 1240, 1260, 1280, 1300, 1320, 1340, 1360, 1380, 
                1400, 1420, 1440, 1460, 1480, 1500, 1520, 1540, 1560, 1580, 1600, 1620, 1640, 1660, 1680, 1700, 1720, 1740, 1760, 1780, 
                1800, 1820, 1840, 1860, 1880, 1900, 1920, 1940, 1960, 1980, 2000, 2020, 2040, 2060, 2080, 2100, 2120, 2140, 2160, 2180, 2200};
        public int[] KeyRowsPoleTip = new int[] {60, 80, 100, 120, 140, 160, 180, 200, 220, 240, 260, 280, 300, 320, 340, 360, 380, 400, 420, 440, 460, 480, 500, 
                520, 540, 560, 580, 600, 620, 640, 660, 680, 700, 720, 740, 760, 780, 800, 820, 840, 860, 880, 900, 920, 940, 960, 980, 
                1000, 1020, 1040, 1060, 1080, 1100, 1120, 1140, 1160, 1180, 1200, 1220, 1240, 1260, 1280, 1300, 1320, 1340, 1360, 1380, 
                1400, 1420, 1440, 1460, 1480, 1500, 1520, 1540, 1560, 1580, 1600, 1620, 1640, 1660, 1680, 1700, 1720, 1740, 1760, 1780, 
                1800, 1820, 1840, 1860, 1880, 1900};
        public int NewWidth = 2237;                             //ABS = 2237, PoleTip = 2456
        public int NewHeight = 1380;                            //ABS = 1380, PoleTip = 2058
        public double AnchorX = 0;
        public double AnchorY = 0;        
        public int LeftScanWhite = 0;                           //PoleTip = 1700
        public int RightScanWhite = 0;                          //PoleTip = 2300         
        public int TopScanWhite = 0;                            //PoleTip = 600
        public int BottomScanWhite = 0;                         //PoleTip = 1300
        public int SampleExpandWidth = 20;                      //ABS = 20, PoleTip = 10
        public int SampleExpandHeight = 20;                     //ABS = 20, PoleTip = 10
        public int SampleWidth = 41;                            //ABS = 41, PoleTip = 151
        public int SampleHeight = 41;                           //ABS = 41, PoleTip = 151
        public int SampleCount = 22;                            //ABS = 22
        public int[] SampleXCoordinates = new int[] { 62, 62, 191, 189, 191, 191, 190, 266, 472, 466, 466, 466, 763, 763, 855, 857, 1125, 1125, 1306, 1306, 1306, 1306 };       //ABS
        public int[] SampleYCoordinates = new int[] { 680, 720, 684, 717, 133, 301, 1102, 1290, 393, 676, 722, 1011, 1111, 1281, 116, 287, 599, 655, 114, 609, 782, 1275 };     //ABS
        public ushort[][] SampleData = null;
        public double[][] SampleWeightSet = null;
        public double[] SampleMeans;
        public double[] SampleStds;
        

        protected override void BaseSerialize(System.IO.BinaryWriter bin)
        {
            PrepareSetting();

            base.BaseSerialize(bin); bin.Write(CurrentVersion);
            bin.Write(IntensityThreshold);
            bin.Write(MinCoorelationCoefficient);
            bin.Write(MinAffectedKeypoints);
            BinarySerializationCommon.Write(bin, KeyColumns);
            BinarySerializationCommon.Write(bin, KeyRowsPoleTip);
            bin.Write(NewWidth);
            bin.Write(NewHeight);
            bin.Write(AnchorX);
            bin.Write(AnchorY);
            bin.Write(LeftScanWhite);
            bin.Write(RightScanWhite);
            bin.Write(TopScanWhite);
            bin.Write(BottomScanWhite);
            bin.Write(SampleExpandWidth);
            bin.Write(SampleExpandHeight);
            bin.Write(SampleWidth);
            bin.Write(SampleHeight);
            bin.Write(SampleCount);
            BinarySerializationCommon.Write(bin, SampleXCoordinates);
            BinarySerializationCommon.Write(bin, SampleYCoordinates);
            for (int i = 0; i < SampleCount; i++)
                BinarySerializationCommon.Write(
                    bin, (SampleData == null ? null : SampleData[i]));
            for (int i = 0; i < SampleCount; i++)
            {
                BinarySerializationCommon.Write(
                    bin, (SampleWeightSet == null ? null : SampleWeightSet[i]));
            }
            BinarySerializationCommon.Write(
                bin, SampleMeans);
            BinarySerializationCommon.Write(
                bin, SampleStds);

            // version: >= 6
            bin.Write(IsABSAlignmentSettings);            
        }

        protected override void BinDeserialize(System.IO.BinaryReader bin)
        {
            base.BinDeserialize(bin); int version = bin.ReadInt32();

            IntensityThreshold = bin.ReadDouble();
            MinCoorelationCoefficient = bin.ReadDouble();
            MinAffectedKeypoints = bin.ReadInt32();
            KeyColumns = BinarySerializationCommon.ReadIntArray(bin);
            KeyRowsPoleTip = BinarySerializationCommon.ReadIntArray(bin);
            NewWidth = bin.ReadInt32();
            NewHeight = bin.ReadInt32();
            if (version >= 5)
            {
                AnchorX = bin.ReadDouble();
                AnchorY = bin.ReadDouble();
                LeftScanWhite = bin.ReadInt32();
                RightScanWhite = bin.ReadInt32();
                TopScanWhite = bin.ReadInt32();
                BottomScanWhite = bin.ReadInt32();
            }

            SampleExpandWidth = bin.ReadInt32();
            SampleExpandHeight = bin.ReadInt32();
            SampleWidth = bin.ReadInt32();
            SampleHeight = bin.ReadInt32();
            SampleCount = bin.ReadInt32();
            SampleXCoordinates = BinarySerializationCommon.ReadIntArray(bin);
            SampleYCoordinates = BinarySerializationCommon.ReadIntArray(bin);
            SampleData = new ushort[SampleCount][];
            for (int i =0; i < SampleCount; i++)
                SampleData[i] = BinarySerializationCommon.ReadUshortArray(bin);
            SampleWeightSet = new double[SampleCount][];
            for (int i = 0; i < SampleCount; i++)
            {
                SampleWeightSet[i] = BinarySerializationCommon.ReadDoubleArray(bin);
            }
            SampleMeans = BinarySerializationCommon.ReadDoubleArray(bin);
            SampleStds = BinarySerializationCommon.ReadDoubleArray(bin);

#if DEBUG
            SampleExpandWidth = 40;
            SampleExpandHeight = 40;
#endif
            SampleExpandWidth = 40;
            SampleExpandHeight = 40;


            // version: >= 6
            if (version >= 6)
            {
                IsABSAlignmentSettings = bin.ReadBoolean();
            }
        }

        public static Settings Deserialize(string filename)
        {
            return ParameterBase.BaseDeserialize(filename) as Settings;
        }

        public static Settings GetDefaultABSAlignmentSettings()
        {
            Settings settings =
                    Settings.Deserialize(
                    Path.Combine(
                    SettingsHelper.GetDefaultABSInspectionSettingsFolderPath(), 
                    Settings.DefaultFileName_ABS));
            return settings;
        }

        public static Settings GetDefaultPoleTipAlignmentSettings()
        {
            Settings settings =
                    Settings.Deserialize(
                    Path.Combine(
                    SettingsHelper.GetDefaultPoleTipInspectionSettingsFolderPath(),
                    Settings.DefaultFileName_PoleTip));

#if DEBUG
            settings.NewWidth = 2115; //1905; //2115;
            settings.NewHeight = 1978;
#else
            throw new System.Exception("AHhhhhhhhhhhhhhhhhh");
#endif
            return settings;
        }

        public void PrepareSetting()
        {
            SampleCount = SampleXCoordinates.Length;

            if (SampleData == null)
                return;
            SampleMeans = new double[SampleData.Length];
            SampleStds = new double[SampleData.Length];
            for (int i = 0; i < SampleData.Length; i++)
            {
                if (SampleData[i] == null)
                    continue;

                double mean = 0;
                double ss = 0;
                double lenInv = 1.0 / SampleData[i].Length;

                for (int j = 0; j < SampleData[i].Length; j++)
                {
                    double temp = SampleData[i][j];
                    mean += temp * lenInv;
                    ss += temp * temp;
                }
                SampleStds[i] = Math.Sqrt((ss * lenInv - mean * mean));
                SampleMeans[i] = mean;
            }
        }

        public void ResetSettings(bool isABSSettings)
        {
            if (isABSSettings)
            {
            }
            else
            {
                IntensityThreshold = 200;                  //ABS = 50, PoleTip = 200
                MinCoorelationCoefficient = 0.5;          //ABS = 0.5, PoleTip = 0.5
                MinAffectedKeypoints = 4;                    //ABS = 8, PoleTip = 4

                LeftScanWhite = 1700;                           //PoleTip = 1700
                RightScanWhite = 2300;                          //PoleTip = 2300         
                TopScanWhite = 600;                            //PoleTip = 600
                BottomScanWhite = 1300;                         //PoleTip = 1300
                SampleExpandWidth = 40;                      //ABS = 20, PoleTip = 10
                SampleExpandHeight = 40;                     //ABS = 20, PoleTip = 10
                SampleWidth = 91;                            //ABS = 41, PoleTip = 151
                SampleHeight = 91;                           //ABS = 41, PoleTip = 151
                SampleCount = 9;                            //ABS = 22
            }
        }
    }
}
