using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace SiGlaz.Common.ImageAlignment
{
    public class RectRegion : ParameterBase
    {
        public new int CurrentVersion
        {
            get
            {
                return 1;
            }
        }

        public double X = 0;
        public double Y = 0;
        public double Width = 2237;
        public double Height = 1380;
        public double Orientation = 0;

        public double ExpandedRight = 0;

        public Color BorderColor = Color.Red;
        public float Thickness = 2.0f;

        public Pen Pen
        {
            get
            {
                return new Pen(BorderColor, Thickness);
            }
        }

        public Matrix Transform
        {
            get
            {
                Matrix m = new Matrix();
                
                m.RotateAt(
                    (float)Orientation, new PointF((float)X, (float)Y));

                return m;
            }
        }

        protected override void BaseSerialize(System.IO.BinaryWriter bin)
        {
            base.BaseSerialize(bin); bin.Write(CurrentVersion);
            
            bin.Write(X);
            bin.Write(Y);
            bin.Write(Width);
            bin.Write(Height);
            bin.Write(Orientation);
        }

        protected override void BinDeserialize(System.IO.BinaryReader bin)
        {
            base.BinDeserialize(bin); int version = bin.ReadInt32();

            X = bin.ReadDouble();
            Y = bin.ReadDouble();
            Width = bin.ReadDouble();
            Height = bin.ReadDouble();
            Orientation = bin.ReadDouble();
        }

        public static RectRegion Deserialize(System.IO.BinaryReader bin)
        {
            return ParameterBase.BaseDeserialize(bin) as RectRegion;
        }

        public static RectRegion Deserialize(string filename)
        {
            return ParameterBase.BaseDeserialize(filename) as RectRegion;
        }
    }
}
