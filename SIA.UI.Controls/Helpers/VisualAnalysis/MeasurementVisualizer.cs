using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Windows.Forms;
using SiGlaz.Common.ImageAlignment;
using SIA.UI.Controls.Dialogs;
using SIA.UI.Components.Helpers;
using SIA.UI.Components;
using SiGlaz.Algorithms.Core;

namespace SIA.UI.Controls.Helpers.VisualAnalysis
{
    public class MeasurementVisualizer
    {
          protected enum HitTestStatus
        {
            Begin,
            End,
            Edge,
            Outside,
        }

        protected class HitTestInfo
        {
            public HitTestStatus Status = HitTestStatus.Outside;
            public PointF Point = PointF.Empty;

            public HitTestInfo(HitTestStatus status, PointF pt)
            {
                this.Status = status;
                this.Point = pt;
            }
        }

        private enum InteractiveMode
        {
            Normal,
            Move,
            AdjustBeginPoint,
            AdjustEndPoint,
        }

        private ImageWorkspace _workspace = null;      
        private ImageAnalyzer _container = null;
        private MetrologySystem _metrologySystem = null;
        private CoordinateSystem _coordinateSystem = null;
        private MetrologyUnitBase _metrologyUnit = null;

        private PointF[] _interactiveLine = new PointF[2];
        protected PointF[] _lastPoints = null;
        private PointF _begin = PointF.Empty, _end = PointF.Empty;        
        private InteractiveMode _interactiveMode = InteractiveMode.Normal;
        private ScaleLine previousLine = null;

        private bool IsInteractiveModeBusy
        {
            get { return _container.IsInteractiveModeBusy; }
            set { _container.IsInteractiveModeBusy = value; }
        }

        public MeasurementVisualizer(ImageWorkspace workspace, ImageAnalyzer container)
        {
            _workspace = workspace;
            _container = container;           
        }
   
        public void Render(Graphics graph, Rectangle rcClip)
        {
            GraphicsState gState = graph.Save();
            try
            {
                graph.SmoothingMode = SmoothingMode.HighQuality;

                using (Transformer transformer = _workspace.ImageViewer.Transformer)
                {
                    if (_begin != PointF.Empty && _end != PointF.Empty)
                    {
                        PointF[] pts = new PointF[] { _begin, _end };                        
                        pts = transformer.PointToPhysical(pts);

                        using (Pen pen = new Pen(Color.Red, 1.0f))
                        {
                            graph.DrawLine(pen, pts[0], pts[1]);

                            float x = 0, y = 0, size = 4;
                            x = pts[0].X - size * 0.5F;
                            y = pts[0].Y - size * 0.5F;
                            graph.FillRectangle(Brushes.Red, x, y, size, size);

                            x = pts[1].X - size * 0.5F;
                            y = pts[1].Y - size * 0.5F;
                            graph.FillRectangle(Brushes.Red, x, y, size, size);
                        }
                    }
                }
            }
            catch
            {
            }
            finally
            {
                graph.Restore(gState);
            }
        }
               
        public void MouseDown(MouseEventArgsEx e)
        {
            try
            {
                if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
                {
                    PointF pt = new PointF(e.X, e.Y);
                   
                    RasterViewerLineEventArgs args = new RasterViewerLineEventArgs(
                        RasterViewerInteractiveStatus.Begin, pt, PointF.Empty);
                    OnInteractiveLine(args);                 
                }
            }
            catch (System.Exception exp)
            {
                Trace.WriteLine(exp);
            }
        }

        public void MouseMove(MouseEventArgsEx e)
        {
            try
            {
                if (this.IsInteractiveModeBusy)
                {
                    if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
                    {
                        PointF pt = new PointF(e.X, e.Y);

                        RasterViewerLineEventArgs args = new RasterViewerLineEventArgs(
                            RasterViewerInteractiveStatus.Working, _interactiveLine[0], pt);
                        OnInteractiveLine(args);
                    }
                }
            }
            catch (System.Exception exp)
            {
                Trace.WriteLine(exp);
            }
        }

        public void MouseUp(MouseEventArgsEx e)
        {
            try
            {
                if (this.IsInteractiveModeBusy)
                {
                    if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
                    {
                        PointF pt = new PointF(e.X, e.Y);
                       
                        RasterViewerLineEventArgs args = new RasterViewerLineEventArgs(
                            RasterViewerInteractiveStatus.End, _interactiveLine[0], pt);
                        OnInteractiveLine(args);

                        Update();
                    }
                }
            }
            catch (System.Exception exp)
            {
                Trace.WriteLine(exp);
            }
        }

        public void MouseClick(MouseEventArgsEx args)
        {
        }
      
        private void OnInteractiveLine(RasterViewerLineEventArgs e)
        {
            if (e.Cancel)
            {
                this._interactiveLine = new PointF[2];
                return;
            }

            switch (e.Status)
            {
                case RasterViewerInteractiveStatus.Begin:
                    this._interactiveLine[0] = e.BeginF;
                    this._interactiveLine[1] = e.BeginF;
                    break;
                case RasterViewerInteractiveStatus.End:
                    this._interactiveLine[0] = e.BeginF;
                    this._interactiveLine[1] = e.EndF;
                    break;
                case RasterViewerInteractiveStatus.Working:
                    this._interactiveLine[0] = e.BeginF;
                    this._interactiveLine[1] = e.EndF;
                    break;
            }

            InteractiveLine(e);
        }

        private void InteractiveLine(RasterViewerLineEventArgs e)
        {
            using (Transformer transformer = new Transformer(_workspace.ImageViewer.Transform))
            {
                PointF[] pts = new PointF[2];

                if (e.Cancel)
                {
                    _begin = PointF.Empty;
                    _end = PointF.Empty;

                    // refresh drawing
                    _workspace.Invalidate(true);
                    return;
                }

                if (this.IsInteractiveModeBusy == false)
                {
                    HitTestInfo htInfo = HitTest(e.BeginF);
                    if (htInfo.Status == HitTestStatus.Begin)
                        _interactiveMode = InteractiveMode.AdjustBeginPoint;
                    else if (htInfo.Status == HitTestStatus.End)
                        _interactiveMode = InteractiveMode.AdjustEndPoint;
                    else if (htInfo.Status == HitTestStatus.Edge)
                        _interactiveMode = InteractiveMode.Move;
                }

                if (_interactiveMode == InteractiveMode.Normal)
                    AdjustNormal(e, transformer, pts);

                else if (this._interactiveMode == InteractiveMode.Move)
                    AdjustMove(e, transformer, pts);

                else if (this._interactiveMode == InteractiveMode.AdjustBeginPoint)
                    AdjustBeginPoint(e, transformer, pts);

                else if (this._interactiveMode == InteractiveMode.AdjustEndPoint)
                    AdjustEndPoint(e, transformer, pts);
            }
        }

        private HitTestInfo HitTest(PointF pt)
        {
            using (Transformer transformer = _workspace.ImageViewer.Transformer)
            {
                PointF[] pts = new PointF[] { _begin, _end, pt };
                pts = transformer.PointToPhysical(pts);
                float size = 4;
                float penWidth = 10;

                if (_begin == PointF.Empty && _end == PointF.Empty)
                    return new HitTestInfo(HitTestStatus.Outside, pt);

                using (Pen pen = new Pen(Color.Red, penWidth))
                {
                    pen.Alignment = PenAlignment.Center;

                    using (GraphicsPath path = new GraphicsPath())
                    {
                        RectangleF rect = new RectangleF(pts[0].X - size * 0.5F, pts[0].Y - size * 0.5F, size, size);
                        path.AddRectangle(rect);
                        if (path.IsVisible(pts[2]))
                            return new HitTestInfo(HitTestStatus.Begin, pt);
                    }

                    using (GraphicsPath path = new GraphicsPath())
                    {
                        RectangleF rect = new RectangleF(pts[1].X - size * 0.5F, pts[1].Y - size * 0.5F, size, size);
                        path.AddRectangle(rect);
                        if (path.IsVisible(pts[2]))
                            return new HitTestInfo(HitTestStatus.End, pt);
                    }

                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddLine(pts[0], pts[1]);
                        if (path.IsOutlineVisible(pts[2], pen))
                            return new HitTestInfo(HitTestStatus.Edge, pt);
                        else
                            return new HitTestInfo(HitTestStatus.Outside, pt);
                    }
                }
            }
        }

        private void AdjustNormal(RasterViewerLineEventArgs e, Transformer transformer, PointF[] pts)
        {
            switch (e.Status)
            {
                case RasterViewerInteractiveStatus.Begin:
                    // set interactive mode busy
                    this.IsInteractiveModeBusy = true;
                    pts[0] = e.BeginF;
                    pts[1] = e.BeginF;
                    pts = transformer.PointToPhysical(pts);
                    _workspace.DrawHelper.DrawXorLine(pts[0], pts[1]);
                    _lastPoints = pts;
                    break;

                case RasterViewerInteractiveStatus.End:
                    this.IsInteractiveModeBusy = false;

                    _begin = e.BeginF;
                    _end = e.EndF;

                    // clear temporary points
                    _lastPoints = null;

                    // refresh drawing
                    _workspace.ImageViewer.Invalidate(true);
                    break;
                case RasterViewerInteractiveStatus.Working:
                    if (_lastPoints != null)
                        _workspace.DrawHelper.DrawXorLine(_lastPoints[0], _lastPoints[1]);

                    pts[0] = e.BeginF;
                    pts[1] = e.EndF;

                    // render temporary points
                    pts = transformer.PointToPhysical(pts);
                    _workspace.DrawHelper.DrawXorLine(pts[0], pts[1]);

                    // update temporary points
                    _lastPoints = pts;
                    break;
            }
        }

        private void AdjustMove(RasterViewerLineEventArgs e, Transformer transformer, PointF[] pts)
        {
            float dx = 0, dy = 0;

            switch (e.Status)
            {
                case RasterViewerInteractiveStatus.Begin:
                    // set interactive mode busy
                    this.IsInteractiveModeBusy = true;
                    pts[0] = _begin;
                    pts[1] = _end;
                    pts = transformer.PointToPhysical(pts);
                    _workspace.DrawHelper.DrawXorLine(pts[0], pts[1]);
                    // update temporary points
                    _lastPoints = pts;
                    break;

                case RasterViewerInteractiveStatus.End:
                    // release interactive mode busy
                    this.IsInteractiveModeBusy = false;

                    dx = e.EndF.X - e.BeginF.X;
                    dy = e.EndF.Y - e.BeginF.Y;

                    _begin.X = _begin.X + dx;
                    _begin.Y = _begin.Y + dy;

                    _end.X = _end.X + dx;
                    _end.Y = _end.Y + dy;

                    // clear temporary points
                    _lastPoints = null;

                    // refresh drawing
                    _workspace.ImageViewer.Invalidate(true);
                    break;
                case RasterViewerInteractiveStatus.Working:
                    if (_lastPoints != null)
                        _workspace.DrawHelper.DrawXorLine(_lastPoints[0], _lastPoints[1]);

                    dx = e.EndF.X - e.BeginF.X;
                    dy = e.EndF.Y - e.BeginF.Y;

                    pts[0] = new PointF(_begin.X + dx, _begin.Y + dy);
                    pts[1] = new PointF(_end.X + dx, _end.Y + dy);

                    // render temporary points
                    pts = transformer.PointToPhysical(pts);
                    _workspace.DrawHelper.DrawXorLine(pts[0], pts[1]);

                    // update temporary points
                    _lastPoints = pts;

                    break;
            }
        }

        private void AdjustBeginPoint(RasterViewerLineEventArgs e, Transformer transformer, PointF[] pts)
        {
            float dx = 0, dy = 0;

            switch (e.Status)
            {
                case RasterViewerInteractiveStatus.Begin:
                    // set interactive mode busy
                    this.IsInteractiveModeBusy = true;
                    pts[0] = _begin;
                    pts[1] = _end;
                    pts = transformer.PointToPhysical(pts);
                    _workspace.DrawHelper.DrawXorLine(pts[0], pts[1]);
                    // update temporary points
                    _lastPoints = pts;
                    break;

                case RasterViewerInteractiveStatus.End:
                    // release interactive mode busy
                    this.IsInteractiveModeBusy = false;
                    _interactiveLine[1] = e.EndF;

                        dx = e.EndF.X - e.BeginF.X;
                        dy = e.EndF.Y - e.BeginF.Y;

                        _begin.X = _begin.X + dx;
                        _begin.Y = _begin.Y + dy;


                    // clear temporary points
                    _lastPoints = null;

                    // refresh drawing
                    _workspace.ImageViewer.Invalidate(true);
                    break;
                case RasterViewerInteractiveStatus.Working:
                    if (_lastPoints != null)
                        _workspace.DrawHelper.DrawXorLine(_lastPoints[0], _lastPoints[1]);

                    dx = e.EndF.X - e.BeginF.X;
                    dy = e.EndF.Y - e.BeginF.Y;

                    pts = new PointF[2];
                    pts[0] = new PointF(_begin.X + dx, _begin.Y + dy);
                    pts[1] = new PointF(_end.X, _end.Y);


                    // render temporary points
                    pts = transformer.PointToPhysical(pts);
                    _workspace.DrawHelper.DrawXorLine(pts[0], pts[1]);


                    // update temporary points
                    _lastPoints = pts;
                    break;
            }
        }

        private void AdjustEndPoint(RasterViewerLineEventArgs e, Transformer transformer, PointF[] pts)
        {
            float dx = 0, dy = 0;

            switch (e.Status)
            {
                case RasterViewerInteractiveStatus.Begin:
                    // set interactive mode busy
                    this.IsInteractiveModeBusy = true;
                    pts[0] = _begin;
                    pts[1] = _end;
                    pts = transformer.PointToPhysical(pts);
                    _workspace.DrawHelper.DrawXorLine(pts[0], pts[1]);
                    // update temporary points
                    _lastPoints = pts;
                    break;

                case RasterViewerInteractiveStatus.End:
                    // release interactive mode busy
                    this.IsInteractiveModeBusy = false;
                    _interactiveLine[1] = e.EndF;

                        dx = e.EndF.X - e.BeginF.X;
                        dy = e.EndF.Y - e.BeginF.Y;

                        _end.X = _end.X + dx;
                        _end.Y = _end.Y + dy;
                    

                    // clear temporary points
                    _lastPoints = null;

                    // refresh drawing
                    _workspace.ImageViewer.Invalidate(true);
                    break;
                case RasterViewerInteractiveStatus.Working:
                    if (_lastPoints != null)
                        _workspace.DrawHelper.DrawXorLine(_lastPoints[0], _lastPoints[1]);

                    dx = e.EndF.X - e.BeginF.X;
                    dy = e.EndF.Y - e.BeginF.Y;

                    pts = new PointF[2];
                    pts[0] = new PointF(_begin.X, _begin.Y);
                    pts[1] = new PointF(_end.X + dx, _end.Y + dy);


                    // render temporary points
                    pts = transformer.PointToPhysical(pts);
                    _workspace.DrawHelper.DrawXorLine(pts[0], pts[1]);

                    // update temporary points
                    _lastPoints = pts;
                    break;
            }
        }

        private void Update()
        {
            if (_metrologySystem == null)
            {
                _metrologySystem = _container.CurrentMetrologySystem;
                _coordinateSystem = _metrologySystem.CurrentCoordinateSystem;
                _metrologyUnit = _metrologySystem.CurrentUnit;
            }

            UpdateLines();

            UpdateDistances();
      

            // update container mode
            if (_interactiveMode!=InteractiveMode.Normal)
                _interactiveMode = InteractiveMode.Normal;
        }

        private void UpdateLines()
        {
            CheckSpecialPoint();
            _workspace.Invalidate(true);           
        }

        private void CheckSpecialPoint()
        {
            PointF begin = GetSpecialPoint(_begin.X, _begin.Y);
            if (begin != PointF.Empty)
                _begin = begin;

            PointF end = GetSpecialPoint(_end.X, _end.Y);
            if (end != PointF.Empty)
                _end = end;
        }

        private PointF GetSpecialPoint(float x, float y)
        {
            PointF pt = PointF.Empty;

            MarkerPoint marker = _metrologySystem.FindMarker(x, y);
            if (marker != null)
            {
                pt.X = marker.DrawingX;
                pt.Y = marker.DrawingY;
            }
            else if (_coordinateSystem.Contains(x, y, _workspace.ImageViewer.ScaleFactor))
            {
                pt.X = _coordinateSystem.DrawingOriginX;
                pt.Y = _coordinateSystem.DrawingOriginY;
            }

            return pt;
        }
       
        private void UpdateDistances()
        {
            float pixelDistance = CalculatePixelDistance();

            if (pixelDistance <= 0)
                _workspace.Invalidate(true);

            float inputPixelDistance = _metrologyUnit.PixelVal;
            float inputUnitDistance = _metrologyUnit.UnitVal;

            float unitDistance = pixelDistance * inputUnitDistance / inputPixelDistance;;
            if (unitDistance > 0)
            {
                _container.MeasurementDistance = unitDistance;
                previousLine = new ScaleLine(_begin, _end);
                previousLine.PixelDistance = pixelDistance;
                previousLine.UnitDistance = unitDistance;
            }
            else
            {
                _begin = previousLine.Point1;
                _end = previousLine.Point2;
                _container.MeasurementDistance = (float)previousLine.UnitDistance;
                _workspace.Invalidate(true);
            }          
           
        }

        private float CalculatePixelDistance()
        {
            return (float)Math.Sqrt((_begin.X - _end.X) * (_begin.X - _end.X) + (_begin.Y - _end.Y) * (_begin.Y - _end.Y));
        }
    }
}
