using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace SiGlaz.Common.ImageAlignment
{
    public class ScaleLineVisualizer
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
                    _normalPen = new Pen(Color.Blue, _thickness);
                    _normalPen.Alignment = PenAlignment.Center;
                    _normalPen.Brush = Brushes.Blue;
                    
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
                    _highlightPen = new Pen(Color.Red, _thickness);
                    _highlightPen.Alignment = PenAlignment.Center;
                    _highlightPen.Brush = Brushes.Red;
                }
                return _highlightPen;
            }
        }
    }

    public class ScaleLine
    {
        public PointF Point1 = new PointF();
        public PointF Point2 = new PointF();
        public float PixelDistance = 0;
        public float UnitDistance = 0;

        public ScaleLine(PointF p1, PointF p2)
        {
            Point1 = p1;
            Point2 = p2;
        }

        public ScaleLine Clone()
        {
            ScaleLine line = new ScaleLine();
            line.Point1 = this.Point1;
            line.Point2 = this.Point2;
            line.PixelDistance = this.PixelDistance;
            line.UnitDistance = this.UnitDistance;

            return line;
        }

        public ScaleLine()
        {
        }

        public void Serialize(BinaryWriter bin)
        {
            bin.Write(Point1.X);
            bin.Write(Point1.Y);
            bin.Write(Point2.X);
            bin.Write(Point2.Y);
            bin.Write(PixelDistance);
            bin.Write(UnitDistance);
        }

        public void Deserialize(BinaryReader bin)
        {
            Point1.X = bin.ReadSingle();
            Point1.Y = bin.ReadSingle();
            Point2.X = bin.ReadSingle();
            Point2.Y = bin.ReadSingle();
            PixelDistance = bin.ReadSingle();
            UnitDistance = bin.ReadSingle();
        }        
    }

    public class MetrologyRuler : ParameterBase
    {
        public const string Description = "Metrology Ruler";
        public const string FileExt = "mrf";
        public const string Filter = "Metrology Ruler Format (*.mrf)|*.mrf";


        protected float _pixelRatio = 0;
        protected float _unitRatio = 0;
        protected List<ScaleLine> _scaleLines = new List<ScaleLine>();


        public MetrologyRuler(List<ScaleLine> lines, float pixelRatio, float unitRatio)
        {
            _pixelRatio = pixelRatio;
            _unitRatio = unitRatio;
            _scaleLines = lines;
        }

        public MetrologyRuler()
        {
        }

        #region Properties
        public float UnitRatio
        {
            get { return _unitRatio; }
        }

        public float PixelRatio
        {
            get { return _pixelRatio; }
        }

        public List<ScaleLine> ScaleLines
        {
            get { return _scaleLines; }
        }
        #endregion

        #region Lines
        public virtual void AddLine(ScaleLine line)
        {
            _scaleLines.Add(line);
        }

        public virtual void Remove(ScaleLine line)
        {
            _scaleLines.Remove(line);
        }

        public virtual void RemoveAt(int index)
        {
            _scaleLines.RemoveAt(index);
        }

        public virtual void ClearLines()
        {
            _scaleLines.Clear();
        }
        #endregion

        #region Serialize and deserialize
        public void Serialize(BinaryWriter bin)
        {
            //base.BaseSerialize(bin);

            bin.Write(_pixelRatio);
            bin.Write(_unitRatio);

            int n = _scaleLines.Count;
            bin.Write(n);
            for (int i = 0; i < n; i++)
                _scaleLines[i].Serialize(bin);
        }

        public void Deserialize(BinaryReader bin)
        {
            //this.BaseDeserialize(bin);

            _pixelRatio = bin.ReadSingle();
            _unitRatio = bin.ReadSingle();

            int n = bin.ReadInt32();
            for (int i = 0; i < n; i++)
            {
                ScaleLine line = new ScaleLine();
                line.Deserialize(bin);
                _scaleLines.Add(line);
            }
        }
        #endregion
    }
}
