using System;
using System.Collections.Generic;
using System.Text;
using SiGlaz.Common.Pattern;

namespace SiGlaz.Common.ImageAlignment
{
    public class DepoAlignmentSettings : AlignmentSettings
    {
        public const string DefaultFileName_Depo = "DepoAlignment.settings";

        public List<HaarPattern> HaarPatterns;

        public List<CorrelationPattern> CorrPatterns;

        public int NumPadCol;
        public int NumPadRow;

        public new static int CurrentVersion
        {
            get
            {
                return 1;
            }
        }

        public DepoAlignmentSettings()
        {
            IntensityThreshold = 30;
            MinCoorelationCoefficient = 0.5;
            MinAffectedKeypoints = 8;
            KeyColumns = new int[] {160, 180, 200, 220, 240, 260, 280, 300, 320, 340, 360, 380, 400, 420, 440, 460, 480, 500, 
                520, 540, 560, 580, 600, 620, 640, 660, 680, 700, 720, 740, 760, 780, 800, 820, 840, 860, 880, 900, 920, 940, 960, 980, 
                1000, 1020, 1040, 1060, 1080, 1100, 1120, 1140};

            KeyRows = new int[] {380, 400, 420, 440, 460, 480, 500, 
                520, 540, 560, 580, 600, 620, 640 };

            NewWidth = 1235;
            NewHeight = 402;
            SampleWidth = 61;
            SampleHeight = 61;
            SampleExpandWidth = 10;
            SampleExpandHeight = 10;

            NumPadCol = 6;
            NumPadRow = 2;
        }

        public override string DefaultFileName
        {
            get
            {
                return DefaultFileName_Depo;
            }
        }

        #region Serialize and deserialize
        public override void Serialize(System.IO.BinaryWriter bin)  
        {
            BaseSerialize(bin);
        }

        protected new void BaseSerialize(System.IO.BinaryWriter bin)
        {
            base.BaseSerialize(bin); bin.Write(CurrentVersion);

            if (HaarPatterns == null)
            {
                bin.Write(0);
            }
            else
            {
                bin.Write(HaarPatterns.Count);
                foreach (HaarPattern ptn in HaarPatterns)
                    ptn.Serialize(bin);
            }
            if (CorrPatterns == null)
            {
                bin.Write(0);
            }
            else
            {
                bin.Write(CorrPatterns.Count);
                foreach (CorrelationPattern ptn in CorrPatterns)
                    ptn.Serialize(bin);
            }

            bin.Write(NumPadRow);
            bin.Write(NumPadCol);
            
        }

        protected new void BinDeserialize(System.IO.BinaryReader bin)
        {
            base.BinDeserialize(bin); int version = bin.ReadInt32();

            int len;

            len = bin.ReadInt32();
            HaarPatterns = new List<HaarPattern>();
            if (len != 0)
            {
                for (int i = 0; i < len; i++)
                    HaarPatterns.Add(HaarPattern.Deserialize(bin));
            }

            len = bin.ReadInt32();
            CorrPatterns = new List<CorrelationPattern>();
            if (len != 0)
            {
                for (int i = 0; i < len; i++)
                    CorrPatterns.Add(CorrelationPattern.Deserialize(bin));
            }
            NumPadRow = bin.ReadInt32();
            NumPadCol = bin.ReadInt32();
        }

        public new static DepoAlignmentSettings Deserialize(string filename)
        {
            DepoAlignmentSettings settings = new DepoAlignmentSettings();
            using (System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            using (System.IO.BinaryReader bin = new System.IO.BinaryReader(fs))
            {
                string typeName = bin.ReadString();
                if (typeName == null || typeName == string.Empty || typeName == nullstr)
                    return null;

                settings.BinDeserialize(bin);
                settings.OnDeserialized();
            }
            return settings;
        }
        #endregion Serialize and deserialize
    }
}

