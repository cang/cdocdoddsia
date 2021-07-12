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
    public class CalibrationVisualizer
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
        private DlgMetrologyCoordinateSystem _dlg = null;
        private MetrologySystem _metrologySystem = null;
        private CoordinateSystem _coordinateSystem = null;
        private MetrologyUnitBase _metrologyUnit = null;
        private MetrologyAnalyzer _container = null;

        private PointF[] _interactiveLine = new PointF[2];
        protected PointF[] _lastPoints = null;
        private PointF _begin = PointF.Empty, _end = PointF.Empty;        
        private InteractiveMode _interactiveMode = InteractiveMode.Normal;

        private int _selectedLineIndx = -1;
        private List<ScaleLine> _scaleLines = null;
        private ScaleLine _curLine = null;

        private bool IsInteractiveModeBusy
        {
            get { return _container.IsInteractiveModeBusy; }
            set { _container.IsInteractiveModeBusy = value; }
        }

        public CalibrationVisualizer(ImageWorkspace workspace, DlgMetrologyCoordinateSystem window, MetrologyAnalyzer container)
        {
            _workspace = workspace;
            _dlg = window;
            _metrologySystem = _dlg.MetrologySystem;
            _coordinateSystem = _metrologySystem.CurrentCoordinateSystem;
            _metrologyUnit = _metrologySystem.CurrentUnit;
            _container = container;

            _scaleLines = new List<ScaleLine>(3);

            _dlg.CalibrationWindow.SelectedLineChanged += new EventHandler(dlgCalibration_SelectedLineChanged);
            _dlg.CalibrationWindow.OnDeleteLine += new EventHandler(dlgCalibration_DeleteRow);
            _dlg.CalibrationWindow.OnDeleteAll += new EventHandler(dlgCalibration_DeleteAllRows);
            _dlg.CalibrationWindow.OnDrawLines += new EventHandler(dlgCalibration_DrawLines);

        }

        public void Render(Graphics graph, Rectangle rcClip)
        {
            GraphicsState gState = graph.Save();
            try
            {
                graph.SmoothingMode = SmoothingMode.HighQuality;

                using (Transformer transformer = _workspace.ImageViewer.Transformer)
                {
                    DrawLines(_scaleLines, transformer, graph);

                    if (_curLine != null)
                        DrawHighlightLine(_curLine, transformer, graph);
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

        private void DrawLines(List<ScaleLine> lines, Transformer transformer, Graphics graph)
        {
            Pen pen = ScaleLineVisualizer.NormalPen;
            for (int ip = 0; ip < lines.Count; ip++)
                Draw(lines[ip], transformer, graph, pen, pen.Brush);
        }

        private void DrawHighlightLine(ScaleLine line, Transformer transformer, Graphics graph)
        {
            Pen pen = ScaleLineVisualizer.HighlightPen;
            Draw(line, transformer, graph, pen, pen.Brush);
        }

        private void Draw(ScaleLine line, Transformer transformer, Graphics graph, Pen pen, Brush brush)
        {
            PointF[] pts = new PointF[] { line.Point1, line.Point2 };
            
            // transform to viewport
            pts = transformer.PointToPhysical(pts);
            graph.DrawLine(pen, pts[0], pts[1]);

            float x = 0, y = 0, size = 4;

            x = pts[0].X - size * 0.5F;
            y = pts[0].Y - size * 0.5F;
            graph.FillRectangle(brush, x, y, size, size);

            x = pts[1].X - size * 0.5F;
            y = pts[1].Y - size * 0.5F;
            graph.FillRectangle(brush, x, y, size, size);        
                
        }

       
        public void MouseDown(MouseEventArgsEx e)
        {
            try
            {
                if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
                {
                    PointF pt = new PointF(e.PointF.X, e.PointF.Y);
                   
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
                        PointF pt = new PointF(e.PointF.X, e.PointF.Y);

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
                        PointF pt = new PointF(e.PointF.X, e.PointF.Y);
                       
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

                    if (_scaleLines.Count > 0)
                        _selectedLineIndx = _scaleLines.Count - 1;

                    // refresh drawing
                    _workspace.Invalidate(true);
                    return;
                }

                if (this.IsInteractiveModeBusy == false)
                {
                    HitTestInfo htInfo = null;
                    int lineIndx = GetSelectedLine(e, ref htInfo);

                    if (lineIndx < 0)
                    {
                        _interactiveMode = InteractiveMode.Normal;
                        _dlg.CalibrationWindow.SelectedLineIndex = lineIndx;
                    }
                    else
                    {
                        _selectedLineIndx = lineIndx;
                        _curLine = _scaleLines[lineIndx].Clone();
                        _begin = _curLine.Point1;
                        _end = _curLine.Point2;

                        if (htInfo.Status == HitTestStatus.Begin)
                            _interactiveMode = InteractiveMode.AdjustBeginPoint;
                        else if (htInfo.Status == HitTestStatus.End)
                            _interactiveMode = InteractiveMode.AdjustEndPoint;
                        else if (htInfo.Status == HitTestStatus.Edge)
                            _interactiveMode = InteractiveMode.Move;
                    }
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

        private int GetSelectedLine(RasterViewerLineEventArgs e, ref HitTestInfo htInfo)
        {
            if (_scaleLines.Count == 0)
                return -1;

            for (int ip = 0; ip < _scaleLines.Count; ip++)
            {
                htInfo = HitTest(_scaleLines[ip].Point1, _scaleLines[ip].Point2, e.BeginF);
                if (htInfo.Status == HitTestStatus.Outside)
                    continue;

                return ip;
            }
            return -1;
        }

        private HitTestInfo HitTest(PointF begin, PointF end, PointF pt)
        {
            using (Transformer transformer = _workspace.ImageViewer.Transformer)
            {
                PointF[] pts = new PointF[] { begin, end, pt };
                pts = transformer.PointToPhysical(pts);
                float size = 4;
                float penWidth = 10;

                if (begin == PointF.Empty && end == PointF.Empty)
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

        private void CheckSpecialPoint()
        {
            PointF begin = GetSpecialPoint(_begin.X, _begin.Y);
            if (begin!=PointF.Empty)
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
      
        private void Update()
        {
            bool isNewLine = UpdateLine();
            UpdateDistances();
          
            if (_curLine != null)
            {
                if (isNewLine)
                {
                    _scaleLines.Add(_curLine);
                    _selectedLineIndx = _scaleLines.Count - 1;
                }
                else
                {
                    _scaleLines[_selectedLineIndx] = _curLine;
                    _workspace.Invalidate(true);
                }
            }
            else
            {
                if (_selectedLineIndx >= 0)
                {
                    _curLine = _scaleLines[_selectedLineIndx].Clone();
                    _workspace.Invalidate(true);
                }
            }

            UpdateCalibrationWindow();

            // update container mode
            if (_interactiveMode!=InteractiveMode.Normal)
                _interactiveMode = InteractiveMode.Normal;
        }

        private bool UpdateLine()
        {
            bool isNewLine = false;

            CheckSpecialPoint();            

            if (_interactiveMode == InteractiveMode.Normal)
            {
                if (_begin != PointF.Empty && _end != PointF.Empty)
                {
                    _curLine = new ScaleLine(_begin, _end);
                    isNewLine = true;
                }
            }
            else 
            {
                _curLine.Point1 = _begin;
                _curLine.Point2 = _end;
            }

            _workspace.Invalidate(true);
            return isNewLine;
        }

        private void UpdateDistances()
        {
            float pixelDistance = CalculateDistanceInPixel();

            if (pixelDistance <= 0)
            {
                _curLine = null;
                _workspace.Invalidate(true);
                return;
            }

            if (_interactiveMode == InteractiveMode.Normal)
            {
                float unitDistance = GetUnitDistance(pixelDistance);

                if (unitDistance <= 0)
                {
                    _curLine = null;
                    _workspace.Invalidate(true);
                }
                else
                {
                    _curLine.PixelDistance = pixelDistance;
                    _curLine.UnitDistance = unitDistance;
                }
            }
            else if (_interactiveMode != InteractiveMode.Move)
            {
                float unitDistance = GetUnitDistance(pixelDistance, _curLine.UnitDistance);

                if (unitDistance <= 0)
                {
                    _curLine = _scaleLines[_selectedLineIndx].Clone();
                    _workspace.Invalidate(true);
                }
                else
                {
                    _curLine.PixelDistance = pixelDistance;
                    _curLine.UnitDistance = unitDistance;
                }
            }
        }

        private float GetUnitDistance(float pixelDistance)
        {
            
            DlgCalibrationValueInput dlgValueInput = new DlgCalibrationValueInput(_metrologyUnit.ShortName, pixelDistance);
            if (dlgValueInput.ShowDialog() == DialogResult.OK)
                return dlgValueInput.Value;

            return -1;
        }

        private float GetUnitDistance(float pixelDistance, float unitDistance)
        {
            DlgCalibrationValueInput dlgValueInput =
                new DlgCalibrationValueInput(_metrologyUnit.ShortName, pixelDistance, unitDistance);
            if (dlgValueInput.ShowDialog() == DialogResult.OK)
                return dlgValueInput.Value;

            return -1;
        }

        private float CalculateDistanceInPixel()
        {
            PointF p1 = _curLine.Point1;
            PointF p2 = _curLine.Point2;
            return (float)Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }

        private void UpdateCalibrationWindow()
        {
            _dlg.CalibrationWindow.Update(_curLine, _selectedLineIndx);
        }

        void dlgCalibration_SelectedLineChanged(object sender, EventArgs e)
        {
            int lineIndx = Convert.ToInt32(sender);

            if (lineIndx < 0)
                return;

            if (_selectedLineIndx != lineIndx)
            {
                _selectedLineIndx = lineIndx;
                _curLine = _scaleLines[_selectedLineIndx].Clone();
                _workspace.Invalidate(true);                
            }
            
        }

        void dlgCalibration_DeleteRow(object sender, EventArgs e)
        {
            int lineIndx = Convert.ToInt32(sender);
            _scaleLines.RemoveAt(lineIndx);

            lineIndx = _dlg.CalibrationWindow.SelectedLineIndex;
            _curLine = lineIndx < 0 ? null : _scaleLines[lineIndx];
            _workspace.Invalidate(true);
        }

        void dlgCalibration_DeleteAllRows(object sender, EventArgs e)
        {
            _scaleLines.Clear();
            _selectedLineIndx = -1;
            _curLine = null;
            _workspace.Invalidate(true);
        }

        void dlgCalibration_DrawLines(object sender, EventArgs e)
        {
            if (sender == null || (sender as List<ScaleLine>).Count == 0)
                return;

            List<ScaleLine> lines = sender as List<ScaleLine>;
            foreach (ScaleLine line in lines)
                _scaleLines.Add(line);

            _selectedLineIndx = 0;
            _curLine = _scaleLines[_selectedLineIndx];
            _workspace.Invalidate(true);
        }
    }
}
