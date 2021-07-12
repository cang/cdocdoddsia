using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SiGlaz.Common.ImageAlignment
{
    public class MarkerVisualizer
    {
        protected static float _thickness = 1.0f;
        protected static Pen _normalPen = null;
        protected static Pen _highlightPen = null;

        public static Pen NormalPen
        {
            get
            {
                if (_normalPen == null)
                {
                    _normalPen = new Pen(Color.Blue, 1.0f);
                }

                return _normalPen;
            }
        }

        public static Pen HighlightPen
        {
            get
            {
                if (_highlightPen == null)
                {
                    _highlightPen = new Pen(Color.Red, 1.0f);
                }

                return _highlightPen;
            }
        }
    }

    public class MarkerPoint : ParameterBase
    {
        public int Version
        {
            get { return 1; }
        }

        // graphics coordinate
        public float DrawingX = 0;
        public float DrawingY = 0;

        // physical coordinate
        public double PhysicalX = 0;
        public double PhysicalY = 0;

        protected override void BaseSerialize(System.IO.BinaryWriter bin)
        {
            base.BaseSerialize(bin); bin.Write(CurrentVersion);

            bin.Write(DrawingX);
            bin.Write(DrawingY);
            bin.Write(PhysicalX);
            bin.Write(PhysicalY);
        }

        protected override void BinDeserialize(System.IO.BinaryReader bin)
        {
            base.BinDeserialize(bin); int version = bin.ReadInt32();

            DrawingX = bin.ReadSingle();
            DrawingY = bin.ReadSingle();
            PhysicalX = bin.ReadDouble();
            PhysicalY = bin.ReadDouble();
        }

        public static MarkerPoint Deserialize(System.IO.BinaryReader bin)
        {
            return ParameterBase.BaseDeserialize(bin) as MarkerPoint;
        }

        public static MarkerPoint Deserialize(string filename)
        {
            return ParameterBase.BaseDeserialize(filename) as MarkerPoint;
        }

        public void CalcError(MarkerPoint mp, ref double error)
        {
            double dx = DrawingX - mp.DrawingX;
            double dy = DrawingY - mp.DrawingY;

            error = Math.Sqrt(dx * dx + dy * dy);
        }
    }

    public class MarkerRegion : MarkerPoint
    {
        public int Version
        {
            get { return 1; }
        }

        // graphics unit
        public float DrawingWidth = 41;
        public float DrawingHeight = 41;

        // temporary variable
        protected PointF[] _pts = new PointF[] 
        { 
            PointF.Empty, PointF.Empty, PointF.Empty, PointF.Empty 
        };

        public virtual bool Contains(float x, float y)
        {
            double sqrRadius = (x - DrawingX) * (x - DrawingX) + (y - DrawingY) * (y - DrawingY);

            return sqrRadius <= DrawingWidth * DrawingHeight;
        }

        protected new void BaseSerialize(System.IO.BinaryWriter bin)
        {
            base.BaseSerialize(bin); bin.Write(CurrentVersion);

            bin.Write(DrawingWidth);
            bin.Write(DrawingHeight);            
        }

        protected new void BinDeserialize(System.IO.BinaryReader bin)
        {
            base.BinDeserialize(bin); int version = bin.ReadInt32();

            DrawingWidth = bin.ReadSingle();
            DrawingHeight = bin.ReadSingle();
        }

        public new static MarkerRegion Deserialize(System.IO.BinaryReader bin)
        {
            return ParameterBase.BaseDeserialize(bin) as MarkerRegion;
        }

        public new static MarkerRegion Deserialize(string filename)
        {
            return ParameterBase.BaseDeserialize(filename) as MarkerRegion;
        }        
    }

    public class MarkerSample : MarkerRegion
    {
        public int Version
        {
            get { return 1; }
        }

        // graphics unit
        public double ExpandingWidth = 40;
        public double ExpandingHeight = 40;

        protected new void BaseSerialize(System.IO.BinaryWriter bin)
        {
            base.BaseSerialize(bin); bin.Write(CurrentVersion);

            bin.Write(DrawingWidth);
            bin.Write(DrawingHeight);
        }

        protected new void BinDeserialize(System.IO.BinaryReader bin)
        {
            base.BinDeserialize(bin); int version = bin.ReadInt32();

            ExpandingWidth = bin.ReadDouble();
            ExpandingHeight = bin.ReadDouble();
        }

        public new static MarkerSample Deserialize(System.IO.BinaryReader bin)
        {
            return ParameterBase.BaseDeserialize(bin) as MarkerSample;
        }

        public new static MarkerSample Deserialize(string filename)
        {
            return ParameterBase.BaseDeserialize(filename) as MarkerSample;
        }
    }
}
