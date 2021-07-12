using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using SIA.Plugins.Common;
using SIA.UI.Controls.Helpers.VisualAnalysis;
using SIA.SystemLayer;
using System.Diagnostics;
using SIA.UI.Components;
using SiGlaz.Common.ImageAlignment;
using SIA.Algorithms.Preprocessing.Alignment;

namespace SIA.UI.Controls
{
    /// <summary>
    /// Provides functionality for managing visual analyzer
    /// </summary>
    public class AnalysisWorkspace 
        : DocumentWorkspace
    {
        #region Fields

        private Dictionary<string, Type> _registeredAnalyzers = new Dictionary<string, Type>();

        private Dictionary<string, IVisualAnalyzer> _analyzers = null;
        private IVisualAnalyzer _activeAnalyzer = null;
        private IVisualAnalyzer _recentAnalyzer = null;

        private List<IVisualAnalyzer> _analyzerList = new List<IVisualAnalyzer>();
        private ImageAnalyzer _imageAnalyzer = null;
        private MetrologyAnalyzer _metrologyAnalyzer = null;

        protected string _refFilePath = "";
        protected MetrologySystemReference _refFile = null;
        public MetrologySystemReference RefFile
        {
            get { return _refFile; }
        }
        public bool SetRefFile(string refFilePath)
        {
            bool bSucceed = false;

            try
            {
                MetrologySystemReference refFile = 
                    MetrologySystemReference.Deserialize(refFilePath);
                if (refFile == null)
                    return false;

                _refFile = refFile;
                _refFilePath = refFilePath;
            }
            catch (System.Exception exp)
            {
                bSucceed = false;
                throw exp;
            }

            return bSucceed;
        }

        protected AlignmentResult _alignmentResult = null;
        public AlignmentResult AlignmentResult
        {
            get { return _alignmentResult; }
            set { _alignmentResult = value; }
        }

        public MetrologySystem GetCurrentMetrologySystem()
        {
            if (_imageAnalyzer == null)
                return null;

            return _imageAnalyzer.MetrologySystem;
        }

        public SiGlaz.Common.GraphicsList DetectedRegions
        {
            get 
            {
                return _imageAnalyzer.Regions;
            }
        }

        private bool _showRegion = true;
        #endregion

        #region Properties

        /// <summary>
        /// Gets the registered analyzers
        /// </summary>
        public List<IVisualAnalyzer> Analyzers
        {
            get
            {
                return _analyzerList;
            }
        }

        /// <summary>
        /// Gets the boolean value indicates that the default image analyzer is in select mode
        /// </summary>
        public bool IsSelect
        {
            get
            {
                if (_imageAnalyzer != null && _imageAnalyzer == _activeAnalyzer)
                    return _imageAnalyzer.InteractiveMode == RasterViewerInteractiveMode.Select;

                return false;
            }
        }
        
        /// <summary>
        /// Gets the boolean value indicates whether the default image analyzer is in pan mode
        /// </summary>
        public bool IsPan
        {
            get
            {
                if (_imageAnalyzer != null && _imageAnalyzer == _activeAnalyzer)
                    return _imageAnalyzer.InteractiveMode == RasterViewerInteractiveMode.Pan;

                return false;
            }
        }

        /// <summary>
        /// Gets the boolean value indicates whether the default image analyzer is in zoom in mode
        /// </summary>
        public bool IsZoomIn
        {
            get
            {
                if (_imageAnalyzer != null && _imageAnalyzer == _activeAnalyzer)
                    return _imageAnalyzer.InteractiveMode == RasterViewerInteractiveMode.ZoomTo;

                return false;
            }
        }

        /// <summary>
        /// Gets the boolean value indicates whether the default image analyzer is in zoom out mode
        /// </summary
        public bool IsZoomOut
        {
            get
            {
                if (_imageAnalyzer != null && _imageAnalyzer == _activeAnalyzer)
                    return _imageAnalyzer.InteractiveMode == RasterViewerInteractiveMode.ZoomOut;

                return false;
            }
        }

        public bool IsMeasurementMode
        {
            get
            {
                if (_imageAnalyzer != null && _imageAnalyzer == _activeAnalyzer)
                    return _imageAnalyzer.InteractiveMode == RasterViewerInteractiveMode.Measurement;

                return false;
            }
        }

        /// <summary>
        /// Gets the boolean value indicates whether the default image analyzer is in extra mode
        /// </summary
        public bool IsExtraInteractiveMode
        {
            get
            {
                if (_imageAnalyzer != null && _imageAnalyzer == _activeAnalyzer)
                    return _imageAnalyzer.InteractiveMode == RasterViewerInteractiveMode.Extra;

                return false;
            }
        }

        public bool ShowRegion
        {
            get { return _showRegion; }
            set 
            { 
                _showRegion = value;
                this.Invalidate(true);
            }
        }

        public bool HasRegion
        {
            get
            {
                if (_imageAnalyzer != null && _imageAnalyzer.DrawCoordinateSystem)
                    return true;

                if (_metrologyAnalyzer != null &&
                    _metrologyAnalyzer.MetrologySystemWindow.Regions != null &&
                    _metrologyAnalyzer.MetrologySystemWindow.Regions.Count > 0)
                    return true;

                return false;
            }
        }

        /// <summary>
        /// Gets or sets the active analyzer
        /// </summary>
        //public IVisualAnalyzer ActiveAnalyzer
        protected IVisualAnalyzer ActiveAnalyzer
        {
            get { return _activeAnalyzer; }
            set
            {
                //if (_activeAnalyzer != value)
                {
                    this.OnDeactivate(_activeAnalyzer);

                    _recentAnalyzer = _activeAnalyzer;
                    _activeAnalyzer = value;

                    this.OnActivate(_activeAnalyzer);
                }
            }
        }

        /// <summary>
        /// Activates the specified analyzer
        /// </summary>
        /// <param name="analyzer">The analyzer to active</param>
        protected virtual void OnActivate(IVisualAnalyzer analyzer)
        {
            if (analyzer != null)
            {
                analyzer.Visible = true;
                analyzer.CursorChanged += new EventHandler(ActiveAnalyzer_CursorChanged);
                analyzer.Activate();
            }
        }

        /// <summary>
        /// Deactivates the specified analyzer
        /// </summary>
        /// <param name="analyzer">The analyzer to deactive</param>
        private void OnDeactivate(IVisualAnalyzer analyzer)
        {
            if (analyzer != null)
            {
                analyzer.Deactivate();
                analyzer.CursorChanged -= new EventHandler(ActiveAnalyzer_CursorChanged);
            }
        }

        #endregion

        #region Constructor and destructor

        public AnalysisWorkspace(AppWorkspace appWorkspace)
            : base(appWorkspace)
        {
            this.InitializeComponent();

            // register for built-in analyzers
            this.RegisterAnalyzer("ImageAnalyzer", typeof(ImageAnalyzer));            
            this.RegisterAnalyzer("MaskViewer", typeof(MaskViewer));
            this.RegisterAnalyzer("DataProfileHelper", typeof(DataProfileHelper));
            this.RegisterAnalyzer("ObjectAnalyzer", typeof(ObjectAnalyzer));
            //this.RegisterAnalyzer("AlignmentConfigurationHelper", typeof(AlignmentConfigurationHelper));
            this.RegisterAnalyzer("MetrologyAnalyzer", typeof(MetrologyAnalyzer));            
        }

        protected override void Dispose(bool disposing)
        {
            if (this._activeAnalyzer != null)
                this._activeAnalyzer.Dispose();
            this._activeAnalyzer = null;

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // AnalysisWorkspace
            // 
            this.Name = "AnalysisWorkspace";
            this.Size = new System.Drawing.Size(311, 229);
            this.ResumeLayout(false);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Override to initialize the built-in analyzer
        /// </summary>
        /// <param name="image"></param>
        public override void CreateWorkspace(CommonImage image)
        {
            base.CreateWorkspace(image);

            // initialize visual analyzers
            this.OnInitializeVisualAnalyzers();
        }

        /// <summary>
        /// Overrides to uninitialize the registered analyzers
        /// </summary>
        public override void DestroyWorkspace()
        {
            base.DestroyWorkspace();

            // uninitialize visual analyzers
            this.OnUninitializeVisualAnalyzers();
        }

        /// <summary>
        /// Gets the analyzer by the specified ID
        /// </summary>
        /// <param name="id">The id of the analyzer</param>
        /// <returns>An instance of the analyzer if found, otherwise false</returns>
        public IVisualAnalyzer GetAnalyzer(string id)
        {
            if (_analyzers == null)
                return null;
            if (_analyzers.ContainsKey(id))
                return _analyzers[id];
            return null;
        }

        /// <summary>
        /// Registers the analyzer by the specified id and type of the analyzer
        /// </summary>
        /// <param name="id">The id of the analyzer</param>
        /// <param name="type">Type of the analyzer</param>
        public void RegisterAnalyzer(string id, Type type)
        {
            _registeredAnalyzers.Add(id, type);
        }

        /// <summary>
        /// Unregister analyzer by the specified id
        /// </summary>
        /// <param name="id">The string value contains the id of the analyzer to unregister</param>
        public void UnregisterAnalyzer(string id)
        {
            if (_registeredAnalyzers.ContainsKey(id))
                _registeredAnalyzers.Remove(id);
        }

        public void InitWorkspace()
        {
            try
            {
                ResetAnalyzer();

                InitMode();
            }
            catch
            {
            }
        }

        public void ResetAnalyzer()
        {
            try
            {
                foreach (IVisualAnalyzer analyzer in this.Analyzers)
                {
                    analyzer.Deactivate();
                }
            }
            catch
            {
            }
        }

        public void InitMode()
        {
            try
            {
                SelectMode();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Set the image analyzer to select mode
        /// </summary>
        public void SelectMode()
        {
            if (IsSelect) return;

            if (_metrologyAnalyzer != null && _metrologyAnalyzer.MetrologySystemWindow != null)
            {
                _metrologyAnalyzer.DisableInteractiveMode();
            }

            if (_imageAnalyzer != null)
            {
                _imageAnalyzer.InteractiveMode = RasterViewerInteractiveMode.Select;
                this.ActiveAnalyzer = _imageAnalyzer;
            }

            this.Invalidate(true);
        }

        /// <summary>
        /// Set the image analyzer to pan mode
        /// </summary>
        public void PanMode()
        {
            if (IsPan) return;

            if (_metrologyAnalyzer != null)
                _metrologyAnalyzer.DisableInteractiveMode();

            if (_imageAnalyzer != null)
            {
                _imageAnalyzer.InteractiveMode = RasterViewerInteractiveMode.Pan;
                this.ActiveAnalyzer = _imageAnalyzer;
            }

            this.Invalidate(true);
        }

        /// <summary>
        /// Set the image analyzer to zoom in mode
        /// </summary>
        public void ZoomInMode()
        {
            if (IsZoomIn) return;

            if (_metrologyAnalyzer != null)
                _metrologyAnalyzer.DisableInteractiveMode();

            if (_imageAnalyzer != null)
            {
                _imageAnalyzer.InteractiveMode = RasterViewerInteractiveMode.ZoomTo;
                this.ActiveAnalyzer = _imageAnalyzer;
            }

            this.Invalidate(true);
        }

        /// <summary>
        /// Set the image analyzer to zoom out mode
        /// </summary>
        public void ZoomOutMode()
        {
            if (IsZoomOut) return;

            if (_metrologyAnalyzer != null)
                _metrologyAnalyzer.DisableInteractiveMode();

            if (_imageAnalyzer != null)
            {
                _imageAnalyzer.InteractiveMode = RasterViewerInteractiveMode.ZoomOut;
                this.ActiveAnalyzer = _imageAnalyzer;
            }

            this.Invalidate(true);
        }

        public void MeasurementMode()
        {
            if (IsMeasurementMode) return;

            if (_metrologyAnalyzer != null && _metrologyAnalyzer.MetrologySystemWindow != null)
            {
                _metrologyAnalyzer.DisableInteractiveMode();
            }

            if (_imageAnalyzer != null)
            {
                _imageAnalyzer.InteractiveMode = RasterViewerInteractiveMode.Measurement;
                this.ActiveAnalyzer = _imageAnalyzer;
            }

            this.Invalidate(true);
        }

        /// <summary>
        /// Set the image analyzer to extra mode
        /// </summary>
        public void ExtraMode()
        {
            if (IsExtraInteractiveMode) return;

            if (_metrologyAnalyzer != null)
                _metrologyAnalyzer.DisableInteractiveMode();

            if (_imageAnalyzer != null)
            {
                _imageAnalyzer.InteractiveMode = RasterViewerInteractiveMode.Extra;
                this.ActiveAnalyzer = _imageAnalyzer;
            }

            this.Invalidate(true);
        }

        /// <summary>
        /// Restores the recent used analyzer
        /// </summary>
        public void RestoreRecentAnalyzer()
        {
            if (_recentAnalyzer != null)
                this.ActiveAnalyzer = _recentAnalyzer;
        }
        
        #endregion

        #region Event Handlers

        private void ActiveAnalyzer_CursorChanged(object sender, EventArgs e)
        {
            if (_activeAnalyzer != null)
                this.DocumentView.Cursor = this._activeAnalyzer.Cursor;
        }

        #endregion

        #region Internal Helpers

        private void OnInitializeVisualAnalyzers()
        {
            _activeAnalyzer = null;
            _analyzers = new Dictionary<string, IVisualAnalyzer>();

            foreach (string id in _registeredAnalyzers.Keys)
            {
                Type type = _registeredAnalyzers[id];
                IVisualAnalyzer analyzer = this.OnCreateAnalyzer(type);
                if (analyzer != null)
                {
                    _analyzers.Add(id, analyzer);
                    _analyzerList.Add(analyzer);

                    if (analyzer is ImageAnalyzer)
                    {
                        _imageAnalyzer = analyzer as ImageAnalyzer;
                    }
                    else if (analyzer is MetrologyAnalyzer)
                    {
                        _metrologyAnalyzer = analyzer as MetrologyAnalyzer;
                    }
                }
            }
        }

        private void OnUninitializeVisualAnalyzers()
        {
            if (this._analyzers != null)
            {
                foreach (IVisualAnalyzer analyzer in _analyzers.Values)
                {
                    if (analyzer is MetrologyAnalyzer)
                    {
                        try
                        {
                            Form window = (analyzer as MetrologyAnalyzer).MetrologySystemWindow.CalibrationWindow;
                            window.Close();
                            window.Dispose();

                            window = (analyzer as MetrologyAnalyzer).MetrologySystemWindow;
                            window.Close();
                            window.Dispose();
                        }
                        catch
                        {
                        }
                    }

                    this.OnDestroyAnalyzer(analyzer);
                }

                _analyzers.Clear();                
                _analyzers = null;

                _analyzerList.Clear();

                _imageAnalyzer = null;
                _metrologyAnalyzer = null;
            }

            this._activeAnalyzer = null;
        }

        protected virtual IVisualAnalyzer OnCreateAnalyzer(Type type)
        {
            try
            {
                // create an instance of analyzer
                IVisualAnalyzer analyzer = Activator.CreateInstance(type, this) as IVisualAnalyzer;
                
                // load persistence data
                analyzer.LoadPersistenceData();

                return analyzer;
            }
            catch (Exception exp)
            {
                Trace.WriteLine(exp);
                return null;
            }
        }

        protected virtual void OnDestroyAnalyzer(IVisualAnalyzer analyzer)
        {
            try
            {
                if (analyzer != null)
                {
                    // save persistence data
                    analyzer.SavePersistenceData();

                    // release analyzer data
                    analyzer.Dispose();
                }
            }
            catch (Exception exp)
            {
                Trace.WriteLine(exp);
            }
        }


        #endregion
    }
}
