using System;
using System.Collections.Generic;
using System.Text;
using SIA.UI.Controls.Dialogs;
using System.Drawing;
using System.Drawing.Drawing2D;
using SiGlaz.Common.ImageAlignment;
using SIA.UI.Components.Helpers;
using System.Windows.Forms;

namespace SIA.UI.Controls.Helpers.VisualAnalysis
{
    public class AlignmentConfigurationHelper : BaseVisualAnalyzer
    {
        private DlgAlignmentConfigurationHelper _dlgConfiguration = null;

        public DlgAlignmentConfigurationHelper Configuration
        {
            get { return _dlgConfiguration; }
        }

        public AlignmentConfigurationHelper(ImageWorkspace workspace)
            : base(workspace)
        {
            _dlgConfiguration = new DlgAlignmentConfigurationHelper(workspace);

            if (workspace != null)
            {
                workspace.ImageViewer.MouseClick += new MouseEventHandler(ImageViewer_MouseClick);
                workspace.ImageViewer.MouseDown += new MouseEventHandler(ImageViewer_MouseDown);
                workspace.ImageViewer.MouseUp += new MouseEventHandler(ImageViewer_MouseUp);
            }
        }

        void ImageViewer_MouseUp(object sender, MouseEventArgs e)
        {
            ProcessMouseUp();
        }
        
        void ImageViewer_MouseDown(object sender, MouseEventArgs e)
        {
            ForceActiveWindowIfItVisible();
        }

        private void ForceActiveWindowIfItVisible()
        {
            if (Workspace == null)
                return;

            if (Workspace.ActiveAnalyzer is ImageAnalyzer)
            {
                if (
                    (Workspace.ActiveAnalyzer as ImageAnalyzer).InteractiveMode != 
                    SIA.UI.Components.RasterViewerInteractiveMode.None)
                {
                    return;
                }
            }

            if (_dlgConfiguration != null && _dlgConfiguration.Visible)
            {
                if (Workspace.ActiveAnalyzer != this)
                {
                    Workspace.ActiveAnalyzer = this;
                }
            }
        }

        void ImageViewer_MouseClick(object sender, MouseEventArgs e)
        {
            if (_dlgConfiguration == null ||
                _dlgConfiguration.Visible == false ||
                _dlgConfiguration.AlignmentSettings == null)
                return;

            UIAlignmentSettings settings = _dlgConfiguration.AlignmentSettings;

            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                PointF pt = new PointF(e.X, e.Y);
                if (e is MouseEventArgsEx == false)
                {
                    pt = this.Workspace.ImageViewer.Transformer.PointToLogical(pt);
                }

                switch (settings.InteractiveMode)
                {
                    case eInteractiveMode.Selection:
                        int index = FindSampleRegion(pt);
                        if (index != settings.SelectedIndex)
                        {
                            settings.SelectedIndex = index;

                            settings.RaiseSelectedSampleChanged();

                            Workspace.Invalidate(true);
                        }
                        break;
                    case eInteractiveMode.Insertion:

                        double x = pt.X;
                        double y = pt.Y;
                        using (Matrix m = new Matrix())
                        {
                            m.RotateAt(
                                -(float)settings.ABSRegion.Orientation,
                                new PointF(
                                    (float)settings.ABSRegion.X,
                                    (float)settings.ABSRegion.Y));
                            PointF[] pts = new PointF[] { pt };
                            x = pts[0].X;
                            y = pts[0].Y;
                        }

                        if (x < settings.ABSRegion.X ||
                            y < settings.ABSRegion.Y ||
                            x > settings.ABSRegion.X + settings.ABSRegion.Width ||
                            y > settings.ABSRegion.Y + settings.ABSRegion.Height)
                        {
                        }
                        else
                        {
                            // add new here
                            settings.AddNew(
                                (int)(x - settings.ABSRegion.X), 
                                (int)(y - settings.ABSRegion.Y));

                            Workspace.Invalidate(true);
                        }

                        break;

                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_dlgConfiguration != null)
                _dlgConfiguration.Dispose();
            _dlgConfiguration= null;

            base.Dispose(disposing);
        }

        public override void Activate()
        {
            base.Activate();
        }

        protected override void OnVisibleChanged()
        {
            base.OnVisibleChanged();
        }

        public override void Render(Graphics graph, Rectangle rcClip)
        {
            base.Render(graph, rcClip);

            RenderRegion(graph, rcClip);

            RenderSampleRegions(graph, rcClip);

            if (_dlgConfiguration != null &&
                _dlgConfiguration.Visible &&
                _dlgConfiguration.AlignmentSettings != null &&
                _dlgConfiguration.AlignmentSettings.SelectedIndex >= 0)
            {
                RenderSelectedSampleRegion(graph, rcClip);
            }
        }

        private bool _isMouseDown = false;
        private int _oldX = 0;
        private int _oldY = 0;

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left && !_isMouseDown)
            {
                _isMouseDown = true;
                _oldX = e.X;
                _oldY = e.Y;                
            }
        }

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_isMouseDown &&
                _dlgConfiguration.AlignmentSettings.SelectedIndex >= 0 && 
                (_oldX != e.X || _oldY != e.Y))
            {
                UIAlignmentSettings settings = _dlgConfiguration.AlignmentSettings;
                settings.SampleXCoordinates[settings.SelectedIndex] += (int)(e.X - _oldX);
                settings.SampleYCoordinates[settings.SelectedIndex] += (int)(e.Y - _oldY);

                settings.RaiseSelectedSampleRegionLocationChanged();

                _oldX = e.X;
                _oldY = e.Y;

                Workspace.Invalidate(true);
            }
        }

        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseUp(e);

            ProcessMouseUp();
        }

        private void ProcessMouseUp()
        {
            try
            {
                if (_isMouseDown)
                    _isMouseDown = false;
            }
            catch
            {
            }
        }

        protected override void OnInteractiveLine(SIA.UI.Components.RasterViewerLineEventArgs e)
        {
            base.OnInteractiveLine(e);
        }       

        #region Helpers
        private void RenderRegion(
            Graphics graph, Rectangle rcClip)
        {
            if (_dlgConfiguration == null || _dlgConfiguration.Visible == false)
                return;

            UIAlignmentSettings settings = _dlgConfiguration.AlignmentSettings;

            using (Transformer transformer = this.Workspace.ImageViewer.Transformer)
            {
                GraphicsState state = graph.Save();
                try
                {
                    RectRegion region = settings.ABSRegion;
                    using (GraphicsPath path = new GraphicsPath())
                    {
                        RectangleF rect = new RectangleF(
                            (float)region.X - 0.5f, (float)region.Y - 0.5f,
                            (float)region.Width, (float)region.Height);
                        path.AddRectangle(rect);

                        path.AddLine(
                            rect.Right - (float)region.ExpandedRight, rect.Top,
                            rect.Right - (float)region.ExpandedRight, rect.Bottom);
                        path.CloseAllFigures();

                        using (Matrix m = region.Transform)
                        {
                            path.Transform(m);
                        }

                        path.Transform(transformer.Transform);

                        using (Pen pen = region.Pen)
                        {
                            graph.SmoothingMode = SmoothingMode.HighQuality;
                            graph.DrawPath(pen, path);
                        }
                    }
                }
                catch
                {
                }
                finally
                {
                    if (state != null)
                        graph.Restore(state);
                }
            }
        }

        private void RenderSampleRegions(Graphics graph, Rectangle rcClip)
        {
            if (_dlgConfiguration == null || _dlgConfiguration.Visible == false)
                return;

            UIAlignmentSettings settings = _dlgConfiguration.AlignmentSettings;
            RectRegion absRegion = settings.ABSRegion;
            double sampleWidth = settings.SampleWidth;
            double sampleHeight = settings.SampleHeight;
            List<int> xCoordinates = settings.SampleXCoordinates;
            List<int> yCoordinates = settings.SampleYCoordinates;

            if (xCoordinates == null || 
                yCoordinates == null || 
                xCoordinates.Count != yCoordinates.Count)
                return;

            using (Transformer transformer = this.Workspace.ImageViewer.Transformer)
            {
                GraphicsState state = graph.Save();
                graph.SmoothingMode = SmoothingMode.HighQuality;

                try
                {                    
                    using (GraphicsPath path = new GraphicsPath())
                    {
                        int n = xCoordinates.Count;
                        for (int i = 0; i < n; i++)
                        {
                            double x = absRegion.X + xCoordinates[i] - sampleWidth * 0.5;
                            double y = absRegion.Y + yCoordinates[i] - sampleHeight * 0.5;

                            RenderSampleRegion(path, x, y, sampleWidth, sampleHeight);
                        }

                        using (Matrix m = absRegion.Transform)
                        {
                            path.Transform(m);
                        }

                        path.Transform(transformer.Transform);

                        using (Pen pen = new Pen(Color.Blue, 1.0f))
                        {                            
                            graph.DrawPath(pen, path);
                        }
                    }
                }
                catch
                {
                }
                finally
                {
                    if (state != null)
                        graph.Restore(state);
                }
            }
        }

        private void RenderSampleRegion(
            GraphicsPath path, double x, double y, double sampleWidth, double sampleHeight)
        {
            // rectangle
            path.AddRectangle(
                new RectangleF((float)x, (float)y, 
                    (float)sampleWidth, (float)sampleHeight));

            // seed point
            x = x + sampleWidth * 0.5;
            y = y + sampleHeight * 0.5;
            double dWidth = 4;
            double dHeight = 4;
            dWidth = sampleWidth * 0.5;
            dHeight = sampleHeight * 0.5;
            path.AddLine(
                new PointF((float)x, (float)(y - dHeight)),
                new PointF((float)x, (float)(y + dHeight)));
            path.CloseFigure();
            path.AddLine(
                new PointF((float)(x - dWidth), (float)y),
                new PointF((float)(x + dWidth), (float)y));
            path.CloseFigure();
        }

        private int FindSampleRegion(PointF pt)
        {
            if (_dlgConfiguration == null || _dlgConfiguration.Visible == false)
                return -1;

            UIAlignmentSettings settings = _dlgConfiguration.AlignmentSettings;
            RectRegion absRegion = settings.ABSRegion;
            double sampleWidth = settings.SampleWidth;
            double sampleHeight = settings.SampleHeight;
            List<int> xCoordinates = settings.SampleXCoordinates;
            List<int> yCoordinates = settings.SampleYCoordinates;

            if (xCoordinates == null || 
                yCoordinates == null || 
                xCoordinates.Count != yCoordinates.Count)
                return -1;

            using (Matrix m = new Matrix())
            {
                m.RotateAt(-(float)absRegion.Orientation, 
                    new PointF((float)absRegion.X, (float)absRegion.Y));
                PointF[] pts = new PointF[] { pt };
                m.TransformPoints(pts);
                pt = pts[0];
            }

            double x = pt.X;
            double y = pt.Y;

            int index = -1;

            double offsetLeft = absRegion.X - sampleWidth * 0.5;
            double offsetTop = absRegion.Y - sampleHeight * 0.5;
            double offsetRight = offsetLeft + sampleWidth;
            double offsetBottom = offsetTop + sampleHeight;
            for (int i = xCoordinates.Count - 1; i >= 0; i--)
            {
                if (x < xCoordinates[i] + offsetLeft ||
                    y < yCoordinates[i] + offsetTop ||
                    x > xCoordinates[i] + offsetRight ||
                    y > yCoordinates[i] + offsetBottom)
                    continue;

                return i;
            }

            return index;
        }

        private void RenderSelectedSampleRegion(
            Graphics graph, Rectangle rcClip)
        {
            if (_dlgConfiguration == null || _dlgConfiguration.Visible == false)
                return;

            UIAlignmentSettings settings = _dlgConfiguration.AlignmentSettings;
            RectRegion absRegion = settings.ABSRegion;
            double sampleWidth = settings.SampleWidth;
            double sampleHeight = settings.SampleHeight;
            List<int> xCoordinates = settings.SampleXCoordinates;
            List<int> yCoordinates = settings.SampleYCoordinates;

            if (xCoordinates == null ||
                yCoordinates == null ||
                xCoordinates.Count != yCoordinates.Count || 
                xCoordinates.Count <= settings.SelectedIndex)
                return;

            using (Transformer transformer = this.Workspace.ImageViewer.Transformer)
            {
                GraphicsState state = graph.Save();
                graph.SmoothingMode = SmoothingMode.HighQuality;

                try
                {
                    using (GraphicsPath path = new GraphicsPath())
                    {
                        int i = settings.SelectedIndex;
                        double x = absRegion.X + xCoordinates[i] - sampleWidth * 0.5;
                        double y = absRegion.Y + yCoordinates[i] - sampleHeight * 0.5;

                        RenderSampleRegion(path, x, y, sampleWidth, sampleHeight);

                        using (Matrix m = absRegion.Transform)
                        {
                            path.Transform(m);
                        }

                        path.Transform(transformer.Transform);

                        using (Pen pen = new Pen(Color.Red, 1.0f))
                        {
                            graph.DrawPath(pen, path);
                        }
                    }
                }
                catch
                {
                }
                finally
                {
                    if (state != null)
                        graph.Restore(state);
                }
            }
        }
        #endregion Helpers
    }    
}
