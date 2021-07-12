using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Drawing;


namespace SiGlaz.Common
{
    public class LinePatternLibrary : ParameterBase
    {
        public static string DefaultFilename = "LinePattern.settings";
        public static string[] MultiplePatternFilenames = new string[] {
            "LineMiddleWidthTopLeft.settings",
            "LineThin.settings",
            "LineThick.settings"};

        public new static int CurrentVersion
        {
            get
            {
                return 1;
            }
        }
        
        public const string SignatureMiddleWidthTopLeft = "LineMiddleWidthTopLeft";
        public const string SignatureThin = "LineThin";
        public const string SignatureVeryThick = "LineThick";

        public string SignatureName;
        public float Width;
        public PointF[] Begins;
        public PointF[] Ends;
        public float Threshold = 50;
        public float ThresholdMax = 220;
        public float MinPointCountNegative = 19;
        public float MinPointCountPositive = 53;


        protected override void BaseSerialize(System.IO.BinaryWriter bin)
        {
            base.BaseSerialize(bin); bin.Write(CurrentVersion);
            bin.Write(SignatureName);
            bin.Write(Threshold);                 
            bin.Write(Width);
            BinarySerializationCommon.Write(bin, Begins);
            BinarySerializationCommon.Write(bin, Ends);

        }

        protected override void BinDeserialize(System.IO.BinaryReader bin)
        {
            base.BinDeserialize(bin); int version = bin.ReadInt32();
            SignatureName = bin.ReadString();
            Threshold = bin.ReadSingle();
            Width = bin.ReadSingle();
            Begins = BinarySerializationCommon.ReadPointFArray(bin);
            Ends = BinarySerializationCommon.ReadPointFArray(bin);
        }

        public static LinePatternLibrary Deserialize(string filename)
        {
            if (!File.Exists(filename))
                return null;

            return ParameterBase.BaseDeserialize(filename) as LinePatternLibrary;
        }

        public void LoadFromText(string filename)
        {
            using (StreamReader stream = new StreamReader(filename))
            {
                SignatureName = stream.ReadLine();
                Width = float.Parse(stream.ReadLine());
                List<PointF> beginList = new List<PointF>();
                List<PointF> endList = new List<PointF>();
                string line = null;
                while ((line = stream.ReadLine()) != null)
                {
                    string[] values = line.Split(',');
                    beginList.Add(new PointF(float.Parse(values[0]), float.Parse(values[1])));
                    endList.Add(new PointF(float.Parse(values[2]), float.Parse(values[3])));
                }

                Begins = beginList.ToArray();
                Ends = endList.ToArray();
            }
        }
    }
}
