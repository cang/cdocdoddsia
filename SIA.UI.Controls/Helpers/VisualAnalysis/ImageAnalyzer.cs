using System;
using System.Collections.Generic;
using System.Text;
using SIA.UI.Components;
using System.Windows.Forms;
using SiGlaz.Common.ImageAlignment;
using System.Drawing;

namespace SIA.UI.Controls.Helpers.VisualAnalysis
{
    public class ImageAnalyzer : BaseVisualAnalyzer
    {
        private RasterViewerInteractiveMode _recentMode = RasterViewerInteractiveMode.Select;
        private RasterViewerInteractiveMode _interactiveMode = RasterViewerInteractiveMode.Select;

        protected MeasurementVisualizer _measurementVisualizer = null;
        protected float _measurementDistance = 0;
        public float MeasurementDistance
        {
            get
            {
                if (_interactiveMode != RasterViewerInteractiveMode.Measurement)
                    return -1;

                return _measurementDistance;
            }
            set
            {
                _measurementDistance = value;
            }
        }        

        #region Metrology System
        protected MetrologySystem _metrologySystem = null;
        protected bool _isVisibleCoordinateSystem = false;
        public MetrologySystem MetrologySystem
        {
            get { return _metrologySystem; }
        }
        public SiGlaz.Common.GraphicsList Regions = null;
        public void SetMetrologySystem(MetrologySystem ms)
        {
            if (ms == null)
            {
                _metrologySystem = null;
                return;
            }

            if (_metrologySystem == null)
                _metrologySystem = new MetrologySystem();

            _metrologySystem.CurrentUnit.CopyFrom(ms.CurrentUnit);
            _metrologySystem.CurrentCoordinateSystem.CopyFrom(ms.CurrentCoordinateSystem);

            _metrologySystem.RebuildTransformer();

            _workspace.Invalidate(true);
        }

        public void SetCoordinateSystemVisibleFlag(bool isVisible)
        {
            _isVisibleCoordinateSystem = isVisible;
        }

        public bool DrawCoordinateSystem
        {
            get
            {
                return (_isVisibleCoordinateSystem && _metrologySystem != null);
            }
        }
        #endregion Metrology System

        public RasterViewerInteractiveMode InteractiveMode
        {
            get { return _interactiveMode; }
            set
            {
                _recentMode = _interactiveMode;
                _interactiveMode = value;
            }
        }

        public ImageAnalyzer(ImageWorkspace workspace)
            : base(workspace)
        {
            _measurementVisualizer = new MeasurementVisualizer(workspace, this);
        }

        public override void Activate()
        {
            base.Activate();

            // update interactive mode
            this.Workspace.ImageViewer.InteractiveMode = this._interactiveMode;

            // update cursor
            this.UpdateCursor();
        }

        public override void Deactivate()
        {
            base.Deactivate();

            // reset interactive mode
            this.Workspace.ImageViewer.InteractiveMode = RasterViewerInteractiveMode.Select;

            // update cursor
            this.UpdateCursor();
        }

        private void UpdateCursor()
        {
            switch (InteractiveMode)
            {
                case RasterViewerInteractiveMode.Pan:
                    this.Cursor = LocalResources.Cursors.Hand;
                    break;
                case RasterViewerInteractiveMode.ZoomTo:
                    this.Cursor = LocalResources.Cursors.ZoomIn; ;
                    break;
                case RasterViewerInteractiveMode.ZoomOut:
                    this.Cursor = LocalResources.Cursors.ZoomOut;
                    break;
                default:
                    this.Cursor = Cursors.Default;
                    break;
            }
        }

        public override void Render(System.Drawing.Graphics graph, System.Drawing.Rectangle rcClip)
        {
            base.Render(graph, rcClip);

            if (DrawCoordinateSystem)
            {
                using (System.Drawing.Drawing2D.Matrix transform = _workspace.ImageViewer.Transform)
                {
                    _metrologySystem.CurrentCoordinateSystem.Draw(
                        graph,
                        new RectangleF(0, 0, _workspace.Image.Width, _workspace.Image.Height),
                        transform);

                    if (Regions != null)
                    {
                        Regions.MetroSys = _metrologySystem;

                        System.Drawing.Drawing2D.GraphicsState gState = graph.Save();
                        try
                        {
                            graph.Transform = transform;
                            Regions.Draw(graph);
                        }
                        catch
                        {
                        }
                        finally
                        {
                            graph.Restore(gState);
                        }
                    }
                }
            }

            if (_measurementVisualizer != null &&
                _interactiveMode == RasterViewerInteractiveMode.Measurement)
            {
                _measurementVisualizer.Render(graph, rcClip);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (_measurementVisualizer != null &&
                _interactiveMode == RasterViewerInteractiveMode.Measurement)
            {
                MouseEventArgsEx args = e as MouseEventArgsEx;
                _measurementVisualizer.MouseDown(args);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if ((e.Button & MouseButtons.Right) == MouseButtons.Right)
            {
                switch (this.InteractiveMode)
                {
                    case RasterViewerInteractiveMode.Pan:
                    case RasterViewerInteractiveMode.ZoomTo:
                    case RasterViewerInteractiveMode.ZoomOut:
                        this.Workspace.RestoreRecentAnalyzer();
                        if (this.Active)
                            this.Workspace.SelectMode();
                        this.Workspace.AppWorkspace.UpdateUI();
                        break;
                   
                    case RasterViewerInteractiveMode.Select:
                    default:
                        break;
                }            
            }

            else if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                switch (this.InteractiveMode)
                {
                    case RasterViewerInteractiveMode.Measurement:
                        if (_measurementVisualizer != null &&
                            _interactiveMode == RasterViewerInteractiveMode.Measurement)
                        {
                            MouseEventArgsEx args = e as MouseEventArgsEx;
                            _measurementVisualizer.MouseUp(args);
                        }
                        break;
                }
            }

        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            this.UpdateCursor();

            if (_measurementVisualizer != null &&
                _interactiveMode == RasterViewerInteractiveMode.Measurement)
            {
                MouseEventArgsEx args = e as MouseEventArgsEx;
                _measurementVisualizer.MouseMove(args);
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (_measurementVisualizer != null &&
                _interactiveMode == RasterViewerInteractiveMode.Measurement)
            {
                MouseEventArgsEx args = e as MouseEventArgsEx;
                _measurementVisualizer.MouseClick(args);
            }
        }

        public MetrologySystem CurrentMetrologySystem
        {
            get
            {
                if (DrawCoordinateSystem)
                    return _metrologySystem;

                MetrologyAnalyzer metrologyAnalyzer = _workspace.GetAnalyzer("MetrologyAnalyzer") as MetrologyAnalyzer;

                if (metrologyAnalyzer == null || !metrologyAnalyzer.Visible)
                    return null;

                return metrologyAnalyzer.MetrologySystemWindow.MetrologySystem;
            }
        }
    }
}
