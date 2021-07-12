using System;
using System.Collections.Generic;
using System.Text;
using SIA.UI.Controls.Dialogs;
using SiGlaz.Common.ImageAlignment;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SIA.UI.Controls.Helpers.VisualAnalysis
{
    public class MetrologyAnalyzer : BaseVisualAnalyzer
    {
        #region Member fields
        public bool ShowCoordinate = false;
        public float XCoordinate = 0;
        public float YCoordinate = 0;

        private DlgMetrologyCoordinateSystem _dlg = null;
        private MetrologySystem _metrologySystem = null;
        private MetrologyUnitBase _currentUnit = null;
        private CoordinateSystem _coordinateSystem = null;        

        private CoordinateSystem _movingOrigin = null;
        private MarkerRegion _currentMarker = null;

        private CalibrationVisualizer _calibrationVisualizer = null;

        private bool _drawCoordinateSystem = true;
        private RectangleF _rangeOfSpace = RectangleF.Empty;
        private bool _isMouseDown = false;
        private int _oldX = 0;
        private int _oldY = 0;
        private PointF _pt = PointF.Empty;
        private PointF[] _onePts = new PointF[] { PointF.Empty };
        private PointF[] _twoPts = new PointF[] { PointF.Empty, PointF.Empty };        

        private eMetrologySystemInteractiveMode _mode = eMetrologySystemInteractiveMode.None;
        #endregion Member fields

        #region Properties
        public DlgMetrologyCoordinateSystem MetrologySystemWindow
        {
            get 
            {
                // update infor here
                if (_dlg != null)
                {
                    // _currentUnit = _dlgMetrologySystem.
                    // _coordinateSystem
                    // _movingOrigin
                }

                return _dlg; 
            }
        }

        public eMetrologySystemInteractiveMode MetrologyInteractiveMode
        {
            get { return _mode; }
        }

        public bool IsChangeOrigin
        {
            get
            {
                return (Visible && _mode == eMetrologySystemInteractiveMode.MoveOrigin);
            }
        }

        public bool IsDrawNewMarker
        {
            get
            {
                return (Visible && _mode == eMetrologySystemInteractiveMode.DrawNewMarker);
            }
        }

        public bool IsCalibration
        {
            get
            {
                return (Visible && _mode == eMetrologySystemInteractiveMode.Calibrate);
            }
        }

        public bool DrawCoordinateSystem
        {
            get { return _drawCoordinateSystem; }
        }
        #endregion Properties

        #region Constructors and destructors
        public MetrologyAnalyzer(ImageWorkspace workspace)
            : base(workspace)
        {
            _interactiveOnlySelectMode = true;

            _dlg = new DlgMetrologyCoordinateSystem(workspace);
            _dlg.WindowHided += new EventHandler(_dlg_WindowHided);
            _dlg.Calibrate += new EventHandler(_dlg_Calibrate);
            _dlg.SelectedMarkerChanged += new EventHandler(_dlg_SelectedMarkerChanged);
            _dlg.CalibrationCompleted += new EventHandler(_dlg_CalibrationCompleted);

            _metrologySystem = _dlg.MetrologySystem;
            _currentUnit = _dlg.MetrologySystem.CurrentUnit;
            _coordinateSystem = _dlg.MetrologySystem.CurrentCoordinateSystem;            
            _dlg.MetrologySystem.DataChanged += new EventHandler(MetrologySystem_DataChanged);

            // calibration visualizer
            _calibrationVisualizer = new CalibrationVisualizer(_workspace, _dlg, this);

            // image changed
            _workspace.ImageViewer.ImageChanged += new EventHandler(ImageViewer_ImageChanged);

            // default current coordinate system
            if (_workspace.Image != null)
            {
                _coordinateSystem.DrawingOriginX = _workspace.Image.Width * 0.5f;
                _coordinateSystem.DrawingOriginY = _workspace.Image.Height * 0.5f;
            }
        }

        void _dlg_CalibrationCompleted(object sender, EventArgs e)
        {
            if (_mode == eMetrologySystemInteractiveMode.Calibrate)
            {
                ResetInteractiveMode();

                _workspace.AppWorkspace.UpdateUI();
            }
        }

        void _dlg_SelectedMarkerChanged(object sender, EventArgs e)
        {
            if (_mode == eMetrologySystemInteractiveMode.DrawNewMarker)
                return;

            if (!_workspace.IsSelect)
                return;

            MarkerPoint marker = _dlg.SelectedMarker;
            if (marker != _currentMarker)
            {
                if (marker == null)
                    _currentMarker = null;
                else
                    _currentMarker = marker as MarkerRegion;

                _workspace.Invalidate(true);
            }
        }

        void _dlg_Calibrate(object sender, EventArgs e)
        {
            this.Calibrate();
        }

        void _dlg_WindowHided(object sender, EventArgs e)
        {
            try
            {
                ResetInteractiveMode();

                this.Visible = false;

                if (_workspace != null)
                    _workspace.AppWorkspace.UpdateUI();
            }
            catch
            {
            }
        }

        void MetrologySystem_DataChanged(object sender, EventArgs e)
        {
            if (_movingOrigin != null)
            {
                _movingOrigin.DrawingAngle = _coordinateSystem.DrawingAngle;
            }

            _metrologySystem.RebuildTransformer();
        }

        void ImageViewer_ImageChanged(object sender, EventArgs e)
        {
            if (_workspace != null && _workspace.Image != null)
            {
                _rangeOfSpace = new RectangleF(0, 0,
                    _workspace.Image.Width, _workspace.Image.Height);
            }
        }
        #endregion Constructors and destructors
               
        #region Interactive mode functionals
        public void ChangeOrigin()
        {
            if (!Workspace.IsExtraInteractiveMode)
                Workspace.ExtraMode();

            this.InternalResetInteractiveMode();
            _mode = eMetrologySystemInteractiveMode.MoveOrigin;

            _movingOrigin = new CoordinateSystem();
            _movingOrigin.CopyFrom(_coordinateSystem);
            _movingOrigin.UpdateVisualizer(true);

            _dlg.CompletedCalibration();

            this.InteractiveModeChanged();
        }
        public void DrawNewMarker()
        {
            if (!Workspace.IsExtraInteractiveMode)
                Workspace.ExtraMode();

            this.InternalResetInteractiveMode();
            _mode = eMetrologySystemInteractiveMode.DrawNewMarker;

            _currentMarker = new MarkerRegion();

            _dlg.CompletedCalibration();

            this.InteractiveModeChanged();
        }

        public void Calibrate()
        {
            if (!Workspace.IsExtraInteractiveMode)
                Workspace.ExtraMode();

            this.InternalResetInteractiveMode();
            _mode = eMetrologySystemInteractiveMode.Calibrate;

            _workspace.AppWorkspace.UpdateUI();

            _dlg.DoCalibration();

            this.InteractiveModeChanged();
        }

        public override void ResetInteractiveMode()
        {
 	         this.Workspace.SelectMode();

             this.InternalResetInteractiveMode();

             this.InteractiveModeChanged();
        }

        public virtual void DisableInteractiveMode()
        {
            _mode = eMetrologySystemInteractiveMode.None;
        }

        public virtual void InternalResetInteractiveMode()
        {
            _mode = eMetrologySystemInteractiveMode.None;

            _currentMarker = null;
            _movingOrigin = null;

            //_dlg.CompletedCalibration();
        }

        protected virtual void InteractiveModeChanged()
        {
            ShowCoordinate = 
                (_visible && 
                (_mode == eMetrologySystemInteractiveMode.None || 
                _mode == eMetrologySystemInteractiveMode.DrawNewMarker));

            if (_metrologySystem != null)
            {
                _metrologySystem.RebuildTransformer();
            }
        }
        #endregion Interactive mode functionals

        #region Override routines
        public override bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                base.Visible = value;
            }
        }

        protected override void OnVisibleChanged()
        {
            base.OnVisibleChanged();

            try
            {                
                if (!_visible)
                {
                    if (_dlg.CalibrationWindow.Visible)
                        _dlg.CalibrationWindow.Close();
                }

                _dlg.Visible = _visible;
            }
            catch
            {
            }

            InteractiveModeChanged();
        }

        public override void Render(Graphics graph, Rectangle rcClip)
        {
            if (!_dlg.Visible && !_dlg.CalibrationWindow.Visible)
                return;

            using (Matrix transform = _workspace.ImageViewer.Transform)
            {
                //base.Render(graph, rcClip);
                if (_coordinateSystem != null)
                    _coordinateSystem.Draw(graph, _rangeOfSpace, transform);

                if (_workspace.IsExtraInteractiveMode &&
                    _mode == eMetrologySystemInteractiveMode.MoveOrigin &&
                    _movingOrigin != null)
                {
                    _movingOrigin.Draw(graph, _rangeOfSpace, transform);
                }

                // draw all markers
                if (_metrologySystem != null)
                {
                    _coordinateSystem.DrawMarkers(
                        graph, _rangeOfSpace, _workspace.ImageViewer.Transform,
                        _workspace.ImageViewer.ScaleFactor, _metrologySystem.Markers);
                }

                // highlight marker will be drawn after others drawn.
                if (_currentMarker != null)
                {
                    _coordinateSystem.DrawMarker(
                        graph, _rangeOfSpace, _workspace.ImageViewer.Transform,
                        _workspace.ImageViewer.ScaleFactor, _currentMarker);
                }

                if (_dlg.CalibrationWindow.Visible &&
                    _calibrationVisualizer != null)
                {
                    _calibrationVisualizer.Render(graph, rcClip);
                }

                if (_workspace.ShowRegion && _dlg.Regions != null)
                {
                    GraphicsState gState = graph.Save();
                    try
                    {
                        graph.Transform = _workspace.ImageViewer.Transform;

                        _metrologySystem.RebuildTransformer();
                        _dlg.Regions.MetroSys = _metrologySystem;
                        _dlg.Regions.Draw(graph);
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
        
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!(_workspace.IsExtraInteractiveMode || _workspace.IsSelect))
                return;

            MouseEventArgsEx args = e as MouseEventArgsEx;
            if (args == null) return;

            if (e.Button == MouseButtons.Left && !_isMouseDown)
                _isMouseDown = true;

            switch (_mode)
            {
                case eMetrologySystemInteractiveMode.None:
                    MarkerPoint marker = FindMarkerUnderMouse(args);
                    if (marker != _currentMarker)
                    {
                        _currentMarker = marker as MarkerRegion;
                        _dlg.SelectedMarker = _currentMarker;

                        _workspace.Invalidate(true);
                    }
                    break;
                case eMetrologySystemInteractiveMode.MoveOrigin:
                    break;
                case eMetrologySystemInteractiveMode.DrawNewMarker:                   
                    break;
                case eMetrologySystemInteractiveMode.Calibrate:
                    _calibrationVisualizer.MouseDown(args);
                    break;
                default:
                    break;
            }
        }
        
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!(_workspace.IsExtraInteractiveMode || 
                _workspace.IsSelect))
                return;

            MouseEventArgsEx args = e as MouseEventArgsEx;
            if (args == null) return;

            if (_mode == eMetrologySystemInteractiveMode.None ||
                _mode == eMetrologySystemInteractiveMode.DrawNewMarker)
            {
                XCoordinate = args.PointF.X;
                YCoordinate = args.PointF.Y;

                _metrologySystem.ToRealCoordinate(ref XCoordinate, ref YCoordinate);
            }

            switch (_mode)
            {
                case eMetrologySystemInteractiveMode.None:
                    if (_isMouseDown)
                    {
                        bool redraw = false;
                        if (_currentMarker != null)
                        {
                            _currentMarker.DrawingX = args.PointF.X;
                            _currentMarker.DrawingY = args.PointF.Y;

                            _dlg.UpdateMarkerLocation(_currentMarker);

                            redraw = true;                            
                        }

                        if (redraw)
                            _workspace.Invalidate(true);
                    }
                    break;
                case eMetrologySystemInteractiveMode.MoveOrigin:
                    PerformMoveOrigin(args);
                    break;
                case eMetrologySystemInteractiveMode.DrawNewMarker:
                    if (_currentMarker != null)
                    {
                        _currentMarker.DrawingX = args.PointF.X;
                        _currentMarker.DrawingY = args.PointF.Y;

                        _workspace.Invalidate(true);
                    }
                    break;
                case eMetrologySystemInteractiveMode.Calibrate:
                    _calibrationVisualizer.MouseMove(args);
                    break;
                default:
                    break;
            }
        }
        
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (_workspace == null || !(_workspace.IsExtraInteractiveMode || _workspace.IsSelect))
                return;

            MouseEventArgsEx args = e as MouseEventArgsEx;
            if (args == null) return;            

            switch (_mode)
            {
                case eMetrologySystemInteractiveMode.None:
                    break;
                case eMetrologySystemInteractiveMode.MoveOrigin:
                    break;
                case eMetrologySystemInteractiveMode.DrawNewMarker:                    
                    break;
                case eMetrologySystemInteractiveMode.Calibrate:
                    _calibrationVisualizer.MouseUp(args);
                    break;
                default:
                    break;
            }

            // reset mouse button state
            _isMouseDown = false;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (!(_workspace.IsExtraInteractiveMode || _workspace.IsSelect))
                return;

            MouseEventArgsEx args = e as MouseEventArgsEx;
            if (args == null || (e.Button & MouseButtons.Left) != MouseButtons.Left)
                return;

            switch (_mode)
            {
                case eMetrologySystemInteractiveMode.None:                    
                    MarkerPoint marker = FindMarkerUnderMouse(args);
                    if (marker != _currentMarker)
                    {
                        _currentMarker = marker as MarkerRegion;
                        _dlg.SelectedMarker = _currentMarker;

                        _workspace.Invalidate(true);
                    }
                    break;
                case eMetrologySystemInteractiveMode.MoveOrigin:
                    PerformChangeOriginLocation(args);
                    break;
                case eMetrologySystemInteractiveMode.DrawNewMarker:
                    PerformAddMarker(args);
                    break;
                case eMetrologySystemInteractiveMode.Calibrate:
                    _calibrationVisualizer.MouseClick(args);
                    break;
                default:
                    break;
            }
        }
        #endregion Override routines

        #region Virtual routines
        protected virtual void PerformMoveOrigin(MouseEventArgsEx args)
        {
            // update drawing pixel-coordinate
            _pt.X = args.PointF.X;
            _pt.Y = args.PointF.Y;

            if (_pt.X != _movingOrigin.DrawingOriginX ||
                _pt.Y != _movingOrigin.DrawingOriginY)
            {
                _movingOrigin.DrawingOriginX = _pt.X;
                _movingOrigin.DrawingOriginY = _pt.Y;
                _movingOrigin.Update();
                _workspace.Invalidate(true);
            }
        }

        protected virtual void PerformChangeOriginLocation(MouseEventArgsEx args)
        {
            if (!_workspace.IsExtraInteractiveMode)
                return;

            _coordinateSystem.DrawingOriginX = args.PointF.X;
            _coordinateSystem.DrawingOriginY = args.PointF.Y;
            _coordinateSystem.Update();

            _coordinateSystem.RaiseDataChangedEvent();

            _workspace.Invalidate(true);
        }
               
        protected virtual void PerformMoveMarker(MouseEventArgsEx args)
        {
            
        }

        protected virtual void PerformAddMarker(MouseEventArgsEx args)
        {
            _metrologySystem.AddMarker(_currentMarker); // current marker is highlight            
            MarkerRegion currentMarker = _currentMarker; 

            this.ResetInteractiveMode();
            _currentMarker = currentMarker;
            _dlg.SelectedMarker = _currentMarker;

            _workspace.AppWorkspace.UpdateUI();
            _workspace.Invalidate(true);
        }

        protected virtual void FindObjectUnderMouse(MouseEventArgsEx args)
        {

        }

        protected virtual MarkerPoint FindMarkerUnderMouse(MouseEventArgsEx args)
        {
            if (_metrologySystem == null)
                return null;

            return _metrologySystem.FindMarker(args.PointF.X, args.PointF.Y);
        }        
        #endregion Virtual routines
    }
}
