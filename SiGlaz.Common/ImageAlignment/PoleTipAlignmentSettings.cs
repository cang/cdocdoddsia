using System;
using System.Collections.Generic;
using System.Text;

namespace SiGlaz.Common.ImageAlignment
{
    public class PoleTipAlignmentSettings : AlignmentSettings
    {
        public const string DefaultFileName_PoleTip = "PoleTipAlignment.settings";

        public double ExpandedRight
        {
            get
            {                
                return 115;
            }
        }

        public new static int CurrentVersion
        {
            get
            {
                return 1;
            }
        }

        public PoleTipAlignmentSettings()
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
            //SampleCount = 9;                            //ABS = 22
            SampleCount = 0;                            //ABS = 22
        }

        public override string DefaultFileName
        {
            get
            {
                return DefaultFileName_PoleTip;
            }
        }

        #region Serialize and deserialize
        protected new void BaseSerialize(System.IO.BinaryWriter bin)
        {
            base.BaseSerialize(bin); bin.Write(CurrentVersion);
        }

        protected new void BinDeserialize(System.IO.BinaryReader bin)
        {
            base.BinDeserialize(bin); int version = bin.ReadInt32();
        }

        public new static PoleTipAlignmentSettings Deserialize(string filename)
        {
            return ParameterBase.BaseDeserialize(filename) as PoleTipAlignmentSettings;
        }
        #endregion Serialize and deserialize
    }
}
