using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace SiGlaz.Common.ImageAlignment
{
    public enum eCoordinateSystemOrientation
    {
        Unknow = -1,
        Northeast = 0,
        Northwest = 1,
        Southwest = 2,
        Southeast = 3
    }

    public class CoordinateSystemVisualizer
    {
        public const float MinRange = -10000;
        public const float MaxRange = 10000;

        protected static float _thickness = 2.0f;
        protected static Pen[] _originPens = null;
        protected static Pen[] _axisPens = null;
        protected static Font _font = null;

        protected static float _originSize = 25.0f;
        public static float OriginSize
        {
            get { return _originSize; }
        }
        public static float Radius
        {
            get
            {
                return (_originSize * 0.5f + _thickness);
            }
        }

        public static float Thickness
        {
            get { return _thickness; }
        }

        public static Pen[] OriginPens
        {
            get
            {
                if (_originPens == null)
                {
                    _originPens = new Pen[] { 
                        new Pen(Color.Red, _thickness), 
                        new Pen(Color.Blue, _thickness) };

                    _originPens[1].DashStyle = DashStyle.Dash;
                }

                return _originPens;
            }
        }

        public static Pen[] AxisPens
        {
            get
            {
                if (_axisPens == null)
                {
                    _axisPens = new Pen[] { 
                        new Pen(Color.Red, _thickness), 
                        new Pen(Color.Blue, _thickness) };

                    _axisPens[1].DashStyle = DashStyle.Dash;
                }

                return _axisPens;
            }
        }

        public static Font Font
        {
            get
            {
                if (_font == null)
                {
                    _font = new Font("Arial", 11f, FontStyle.Bold);                    
                }

                return _font;
            }
        }
    }

    public class CoordinateSystem : ParameterBase
    {
        #region Member fields
        protected Pen _originPen = null;
        protected Pen _axisPen = null;
        protected float _originSize = 25;
        protected PointF[] _pt = new PointF[] { PointF.Empty };
        protected PointF[] _tmpPts = 
            new PointF[] { PointF.Empty, PointF.Empty};

        public eCoordinateSystemOrientation 
            Orientation = eCoordinateSystemOrientation.Northeast;
        public float DrawingOriginX = 0;
        public float DrawingOriginY = 0;
        public float DrawingAngle = 0;//0;

        protected Matrix _graphicsTransformer = new Matrix();

        public event EventHandler DataChanged;
        #endregion Member fields

        #region Constructors and destructors
        public CoordinateSystem()
        {
            UpdateVisualizer(false);

#if DEBUG
            DrawingOriginX = 100.0f;
            DrawingOriginY = 1747.0f;
            DrawingAngle = 0.15f;
#endif

            this.Update();
        }

        public void UpdateVisualizer(bool isInteractive)
        {
            int index = (isInteractive ? 1 : 0);

            _originPen = CoordinateSystemVisualizer.OriginPens[index];
            _axisPen = CoordinateSystemVisualizer.AxisPens[index];
            _originSize = CoordinateSystemVisualizer.OriginSize;
        }

        public void Update()
        {
            // calc temp variables

            // update transformer
            _graphicsTransformer.Reset();
            _graphicsTransformer.RotateAt(
                -DrawingAngle, GetOriginPointF());
        }
        #endregion Constructors and destructors

        #region Properties
        public Matrix GraphicsTransformer
        {
            get { return _graphicsTransformer; }
        }        

        public Point GetCeilOriginPoint()
        {
            return new Point((int)DrawingOriginX, (int)DrawingOriginY);
        }

        public Point GetRoundOriginPoint()
        {
            return new Point(
                (int)Math.Round(DrawingOriginX), 
                (int)Math.Round(DrawingOriginY));
        }

        public PointF GetOriginPointF()
        {
            return new PointF(DrawingOriginX, DrawingOriginY);
        }

        public Matrix GetGraphicsTransform()
        {
            Matrix m = new Matrix();
            m.RotateAt(DrawingAngle, GetOriginPointF());
            return new Matrix();
        }
        #endregion Properties

        #region Methods
        public virtual void RaiseDataChangedEvent()
        {
            if (DataChanged != null)
                this.DataChanged(this, EventArgs.Empty);
        }

        public virtual bool Contains(float x, float y, float scale)
        {
            double sqrRadius = (x - DrawingOriginX) * (x - DrawingOriginX) + (y - DrawingOriginY) * (y - DrawingOriginY);
                       
            sqrRadius = sqrRadius * scale * scale;

            float radius = CoordinateSystemVisualizer.Radius;
            return sqrRadius <= radius * radius;
        }

        #endregion Methods

        #region Draw coordinate system
        protected virtual void Draw(Graphics dstGrph, RectangleF rangeOfSpace)
        {

        }

        public virtual void Draw(
            Graphics dstGraph, RectangleF rangeOfSpace, Matrix transformer)
        {
            GraphicsState gState = dstGraph.Save();
            try
            {
                dstGraph.SmoothingMode = SmoothingMode.HighQuality;

                DrawXAxis(dstGraph, rangeOfSpace, transformer);
                DrawYAxis(dstGraph, rangeOfSpace, transformer);
                DrawOrigin(dstGraph, transformer);
            }
            catch
            {
            }
            finally
            {
                dstGraph.Restore(gState);
            }
        }
        #endregion Draw coordinate system

        #region Draw origin and axes
        protected virtual void DrawOrigin(Graphics dstGraph, Matrix transformer)
        {
            // calc origin position in viewing device
            _tmpPts[0].X = DrawingOriginX;
            _tmpPts[0].Y = DrawingOriginY;
            transformer.TransformPoints(_tmpPts);

            //using (GraphicsPath path = new GraphicsPath())
            //{
            //    float x = _tmpPts[0].X;
            //    float y = _tmpPts[0].Y;
            //    float radius = _originSize * 0.5f;

            //    float angleStart = -DrawingAngle;
            //    switch (Orientation)
            //    {
            //        case eCoordinateSystemOrientation.Northeast:
            //            angleStart -= 0;
            //            break;
            //        case eCoordinateSystemOrientation.Northwest:
            //            angleStart -= 90;
            //            break;
            //        case eCoordinateSystemOrientation.Southwest:
            //            angleStart -= 180;
            //            break;
            //        case eCoordinateSystemOrientation.Southeast:
            //            angleStart -= 270;
            //            break;
            //        default:
            //            break;
            //    }

            //    path.AddArc(x - radius, y - radius, _originSize, _originSize, angleStart, 90);
            //    //path.AddLine(x, y - radius, x, y);
            //    //path.AddLine(x, y, x + radius, y);
            //    path.CloseAllFigures();

            //    //using (Matrix m = new Matrix())
            //    //{
            //    //    switch (Orientation)
            //    //    {
            //    //        case eCoordinateSystemOrientation.Northeast:
            //    //            //m.RotateAt(DrawingAngle, new PointF(x, y));
            //    //            //m.RotateAt(DrawingAngle - 90, new PointF(x, y));
            //    //            //m.RotateAt(DrawingAngle - 180, new PointF(x, y));
            //    //            m.RotateAt(DrawingAngle - 270, new PointF(x, y));
            //    //            break;
            //    //        case eCoordinateSystemOrientation.Northwest:
            //    //            m.RotateAt(DrawingAngle - 90, new PointF(x, y));
            //    //            break;
            //    //        case eCoordinateSystemOrientation.Southwest:
            //    //            m.RotateAt(DrawingAngle - 180, new PointF(x, y));
            //    //            break;
            //    //        case eCoordinateSystemOrientation.Southeast:
            //    //            m.RotateAt(DrawingAngle - 270, new PointF(x, y));
            //    //            break;
            //    //        default:
            //    //            break;
            //    //    }

            //    //    path.Transform(m);
            //    //}

            //    //using (Brush brush = new SolidBrush(Color.FromArgb(80, Color.Green)))
            //    //{
            //    //    dstGraph.FillPath(brush, path);
            //    //}
            //}

            // draw origin
            if (_originPen != null)
            {
                dstGraph.DrawEllipse(_originPen,
                    _tmpPts[0].X - _originSize * 0.5f,
                    _tmpPts[0].Y - _originSize * 0.5f, _originSize, _originSize);
            }
        }

        protected virtual void DrawXAxis(
            Graphics dstGraph, RectangleF rangeOfSpace, Matrix transformer)
        {
            //float left = rangeOfSpace.Left;
            //float right = rangeOfSpace.Right;

            float left = CoordinateSystemVisualizer.MinRange;
            float right = CoordinateSystemVisualizer.MaxRange;

            _tmpPts[0].X = (DrawingOriginX > left ? left : DrawingOriginX);
            _tmpPts[0].Y = DrawingOriginY;

            _tmpPts[1].X = (DrawingOriginX < right ? right : DrawingOriginX);
            _tmpPts[1].Y = DrawingOriginY;

            DrawAxis(dstGraph, transformer, _tmpPts);
        }

        protected virtual void DrawYAxis(
            Graphics dstGraph, RectangleF rangeOfSpace, Matrix transformer)
        {
            //float top = rangeOfSpace.Top;
            //float bottom = rangeOfSpace.Bottom;

            float top = CoordinateSystemVisualizer.MinRange;
            float bottom = CoordinateSystemVisualizer.MaxRange;

            _tmpPts[0].X = DrawingOriginX;
            _tmpPts[0].Y = (DrawingOriginY > top ? top : DrawingOriginY);

            _tmpPts[1].X = DrawingOriginX;
            _tmpPts[1].Y = (DrawingOriginY < bottom ? bottom : DrawingOriginY);

            DrawAxis(dstGraph, transformer, _tmpPts);
        }

        protected virtual void DrawAxis(
            Graphics dstGraph, Matrix transformer, PointF[] pts)
        {
            // metrology system correction
            _graphicsTransformer.TransformPoints(pts);

            // transform to viewport
            transformer.TransformPoints(pts);

            // draw on device
            if (_axisPen != null)
            {
                dstGraph.DrawLine(_axisPen, pts[0], pts[1]);
            }
        }
        #endregion Draw origin and axes

        #region Draw markers
        public virtual void DrawMarker(
            Graphics dstGraph, RectangleF rangeOfSpace, Matrix transformer, float scale, MarkerRegion marker)
        {
            GraphicsState gState = dstGraph.Save();
            try
            {
                dstGraph.SmoothingMode = SmoothingMode.HighQuality;

                DrawMarker(
                    dstGraph, rangeOfSpace, transformer, scale, marker,
                    MarkerVisualizer.HighlightPen, DrawingAngle);
            }
            catch
            {
            }
            finally
            {
                dstGraph.Restore(gState);
            }
        }

        public virtual void DrawMarkers(
            Graphics dstGraph, RectangleF rangeOfSpace, 
            Matrix transformer, float scale, List<MarkerPoint> markers)
        {
            GraphicsState gState = dstGraph.Save();
            try
            {
                dstGraph.SmoothingMode = SmoothingMode.HighQuality;

                foreach (MarkerRegion marker in markers)
                {
                    DrawMarker(
                        dstGraph, rangeOfSpace, transformer, scale, marker,
                        MarkerVisualizer.NormalPen, DrawingAngle);
                }
            }
            catch
            {
            }
            finally
            {
                dstGraph.Restore(gState);
            }
        }

        public virtual void DrawMarker(
            Graphics dstGraph, RectangleF rangeOfSpace, Matrix transformer, float scale,
            MarkerRegion marker, Pen pen, float angle)
        {
            _pt[0].X = marker.DrawingX;
            _pt[0].Y = marker.DrawingY;
            transformer.TransformPoints(_pt);

            using (GraphicsPath path = new GraphicsPath())
            {
                //if (scale < 1) scale = 1;
                float w = marker.DrawingWidth * scale;
                float h = marker.DrawingHeight * scale;

                float x = _pt[0].X - w * 0.5f;
                float y = _pt[0].Y - h * 0.5f;
                path.AddEllipse(x, y, w, h);

                path.AddLine(_pt[0].X, y, _pt[0].X, y + h);
                path.CloseFigure();

                path.AddLine(x, _pt[0].Y, x + w, _pt[0].Y);
                path.CloseFigure();

                using (Matrix m = new Matrix())
                {
                    m.RotateAt(-angle, _pt[0]);
                    path.Transform(m);
                }

                dstGraph.DrawPath(pen, path);
            }
        }
        #endregion Draw markers

        #region Copy functions
        private void CopyTo(CoordinateSystem dst)
        {
            dst.DrawingOriginX = DrawingOriginX;
            dst.DrawingOriginY = DrawingOriginY;
            dst.DrawingAngle = DrawingAngle;

            dst.Update();
        }

        public void CopyFrom(CoordinateSystem dst)
        {
            DrawingOriginX = dst.DrawingOriginX;
            DrawingOriginY = dst.DrawingOriginY;
            DrawingAngle = dst.DrawingAngle;

            this.Update();
        }
        #endregion Copy functions

        #region Serialize and deserialize
        public int Version
        {
            get { return 1; }
        }

        protected override void BaseSerialize(System.IO.BinaryWriter bin)
        {
            base.BaseSerialize(bin); bin.Write(CurrentVersion);

            bin.Write((int)Orientation);
            bin.Write(DrawingOriginX);
            bin.Write(DrawingOriginY);
            bin.Write(DrawingAngle);
        }

        protected override void BinDeserialize(System.IO.BinaryReader bin)
        {
            base.BinDeserialize(bin); int version = bin.ReadInt32();

            Orientation = (eCoordinateSystemOrientation)bin.ReadInt32();
            DrawingOriginX = bin.ReadSingle();
            DrawingOriginY = bin.ReadSingle();
            DrawingAngle = bin.ReadSingle();

            this.Update();
        }

        public static CoordinateSystem Deserialize(System.IO.BinaryReader bin)
        {
            return ParameterBase.BaseDeserialize(bin) as CoordinateSystem;
        }

        public static CoordinateSystem Deserialize(string filename)
        {
            return ParameterBase.BaseDeserialize(filename) as CoordinateSystem;
        }
        #endregion Serialize and deserialize

        public void CalcError(CoordinateSystem cs,
            ref double originError, ref double angleError)
        {
            double dx = DrawingOriginX - cs.DrawingOriginX;
            double dy = DrawingOriginY - cs.DrawingOriginY;

            originError = Math.Sqrt(dx * dx + dy * dy);

            angleError = Math.Abs(DrawingAngle - cs.DrawingAngle);
        }
    }
}
