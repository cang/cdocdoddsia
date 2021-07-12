#define SUPPORED_REAL_COORDINATE__

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SiGlaz.Common.ImageAlignment;
using SIA.Algorithms.Preprocessing.Alignment;
using SIA.UI.Controls.UserControls;
using SIA.UI.Components.Helpers;
using SIA.UI.MaskEditor;
using SIA.Algorithms.ReferenceFile;
using SIA.UI.Controls.Helpers.VisualAnalysis;

namespace SIA.UI.Controls.Dialogs
{
    public partial class DlgMetrologyCoordinateSystem : Form
    {
        #region Member fields
        public event EventHandler WindowHided;
        public event EventHandler CalibrationCompleted;
        public event EventHandler CalibrationRequestCompleted;
        public event EventHandler Calibrate;
        public event EventHandler SelectedMarkerChanged;

        private ImageWorkspace _workspace = null;

        public MetrologySystem MetrologySystem = new MetrologySystem();
        public List<MarkerPoint> KeyPoints = new List<MarkerPoint>();
        private SiGlaz.Common.GraphicsList _regions = null;
        public SiGlaz.Common.GraphicsList Regions
        {
            get { return _regions; }
        }

        public DlgCalibration CalibrationWindow = null;
        #endregion Member fields

        #region Hint Processor
        protected AlignmentSettings _hintParameters = null;
        protected AlignerBase _hintProcessor = null;
        public AlignerBase HintProcessor
        {
            get { return _hintProcessor; }
        }

        public ucUnitConfiguration UnitConfiguration
        {
            get { return ucUnitConfiguration1; }
        }

        public void UpdateHintProcessor(string model)
        {
            AlignmentSettings settings = AlignmentSettings.Deserialize(model);
            this.UpdateHintProcessor(settings);
        }

        public void UpdateHintProcessor(AlignmentSettings settings)
        {
            _hintParameters = settings;

            if (settings is ABSAlignmentSettings)
                _hintProcessor = new ABSAligner(settings);
            else
                _hintProcessor = new PoleTipAligner(settings);
        }
        #endregion Hint Processor

        #region Constructors and destructors
        public DlgMetrologyCoordinateSystem(ImageWorkspace workspace)
        {
            InitializeComponent();

            // initialize top level window
            this.TopLevel = true;

            _workspace = workspace;

            if (_workspace != null)
            {
                this.Owner = _workspace.FindForm();
            }

            this.Initialize();
        }

        protected virtual void Initialize()
        {
            this.InitializeMenuItemClicked();

            this.InitializeMetrologyUnit();

            this.InitializeCoordinateSystem();

            this.InitializeMarkerList();

            this.InitializeHintProcessor();

            MetrologySystem.DataChanged += new EventHandler(MetrologySystem_DataChanged);

#if SUPPORED_REAL_COORDINATE
            this.refineToolStripMenuItem.Visible = true;
#else
            this.refineToolStripMenuItem.Visible = false;
#endif

            createRegionsToolStripMenuItem.Text = "Create/Edit Regions";
        }

        protected virtual void InitializeMenuItemClicked()
        {
            openToolStripMenuItem.Click += new EventHandler(mn_ItemClicked);
            saveToolStripMenuItem.Click += new EventHandler(mn_ItemClicked);
            applyToWorkspaceToolStripMenuItem.Click += new EventHandler(mn_ItemClicked);
            importToolStripMenuItem.Click += new EventHandler(mn_ItemClicked);
            createReferenceFileToolStripMenuItem.Click += new EventHandler(mn_ItemClicked);
            exitToolStripMenuItem.Click += new EventHandler(mn_ItemClicked);
            calibrateToolStripMenuItem.Click += new EventHandler(mn_ItemClicked);
            hintMarkersToolStripMenuItem.Click += new EventHandler(mn_ItemClicked);
            refineToolStripMenuItem.Click += new EventHandler(mn_ItemClicked);
            createRegionsToolStripMenuItem.Click += new EventHandler(mn_ItemClicked);
        }

        void MetrologySystem_DataChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }
        #endregion Constructors and destructors

        #region Override routines
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            ucUnitConfiguration1.Update(true);
            ucCoordinateSystem1.Update(true);
            ucMarkerList1.Update(true);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // cancel the closing operation just hide this window out
            e.Cancel = true;
            this.Visible = false;

            if (WindowHided != null)
                this.WindowHided(this, EventArgs.Empty);

            ImageAnalyzer imageAnalyzer = _workspace.GetAnalyzer("ImageAnalyzer") as ImageAnalyzer;
            imageAnalyzer.SetCoordinateSystemVisibleFlag(true);

            _workspace.AppWorkspace.UpdateUI();

            _workspace.Focus();
            _workspace.Invalidate(true);

            base.OnClosing(e);
        }
        #endregion Override routines

        #region Metrology Unit
        protected virtual void InitializeMetrologyUnit()
        {
            ucUnitConfiguration1.CurrentUnit = MetrologySystem.CurrentUnit;
            ucUnitConfiguration1.CalibrationClicked += new EventHandler(ucUnitConfiguration1_CalibrationClicked);
            CalibrationWindow = new DlgCalibration(_workspace, this);
        }

        public virtual void DoCalibration()
        {
            DlgCalibration dlg = this.CalibrationWindow;

            // register closed event here
            dlg.CalibrationCompleted += new EventHandler(dlg_CalibrationCompleted);

            // update info here

            // show dlg
            dlg.Visible = true;

            // hide metrology system dialogue
            this.Visible = false;

            _workspace.Invalidate(true);
        }

        public virtual void CompletedCalibration()
        {
            if (CalibrationRequestCompleted != null)
                this.CalibrationRequestCompleted(this, EventArgs.Empty);
        }

        void ucUnitConfiguration1_CalibrationClicked(object sender, EventArgs e)
        {
            DoCommandCalibrate();
        }

        void dlg_CalibrationCompleted(object sender, EventArgs e)
        {
            DlgCalibration dlg = sender as DlgCalibration;
            // get infor from DlgCalibration here

            // update to metrology unit
            float pixelVal = dlg.Pixel;
            float unitVal = dlg.Unit;
            MetrologySystem.CurrentUnit.UpdateScale(pixelVal, unitVal);
            ucUnitConfiguration1.Update(true);

            // unregister event here
            dlg.CalibrationCompleted -= new EventHandler(dlg_CalibrationCompleted);

            // show metrology system dialogue
            this.Visible = true;

            if (CalibrationCompleted != null)
            {
                CalibrationCompleted(this, EventArgs.Empty);
            }

            _workspace.Invalidate(true);
        }
        #endregion Metrology Unit

        #region Coordinate System
        protected virtual void InitializeCoordinateSystem()
        {
            ucCoordinateSystem1.CoordinateSystem = MetrologySystem.CurrentCoordinateSystem;
            ucCoordinateSystem1.CoordinateSystemChanged += new EventHandler(ucCoordinateSystem1_MetrologySystemChanged);
        }

        void ucCoordinateSystem1_MetrologySystemChanged(object sender, EventArgs e)
        {
            this.MetrologySystem.RaiseDataChangedEvent();

            _workspace.Invalidate(true);
        }
        #endregion Coordinate System

        #region Regions
        private void MaskEditor_ApplyMask(object sender, MaskEditorApplyMaskEventArgs args)
        {
            _regions = args.GraphicsList;
            if (_regions != null)
                _regions.UnselectAll();

            _workspace.AppWorkspace.UpdateUI();

            _workspace.Invalidate(true);
        }
        private void btnRegionEditor_Click(object sender, EventArgs e)
        {
            DoCommandCreateRegions();
        }
        #endregion

        #region Marker List
        protected virtual void InitializeMarkerList()
        {
            ucMarkerList1.MetrologySystem = MetrologySystem;
            ucMarkerList1.SelectedMarkerChanged += new EventHandler(ucMarkerList1_SelectedMarkerChanged);
            ucMarkerList1.DeletedMarker += new EventHandler(ucMarkerList1_DeletedMarker);
            ucMarkerList1.DeletedAllMarkers += new EventHandler(ucMarkerList1_DeletedAllMarkers);
            ucMarkerList1.MarkerHintClicked += new EventHandler(ucMarkerList1_MarkerHintClicked);
            ucMarkerList1.NavigatedToMarker += new EventHandler(ucMarkerList1_NavigatedToMarker);
        }

        void ucMarkerList1_NavigatedToMarker(object sender, EventArgs e)
        {
            int index = ucMarkerList1.SelectedMarkerIndex;
            if (index < 0) return;

            if (_workspace.ImageViewer.ScaleFactor < 1.0f)
                _workspace.ImageViewer.ScaleFactor = 1.0f;

            using (Transformer transformer = this._workspace.ImageViewer.Transformer)
            {
                MarkerPoint marker = MetrologySystem.Markers[index];
                double x = marker.DrawingX;
                double y = marker.DrawingY;

                Point pt = Point.Round(transformer.PointToPhysical(new Point((int)x, (int)y)));
                this._workspace.ImageViewer.CenterAtPoint(pt);
            }
        }

        void ucMarkerList1_MarkerHintClicked(object sender, EventArgs e)
        {
            DoCommandHint();
        }

        void ucMarkerList1_DeletedAllMarkers(object sender, EventArgs e)
        {
            MetrologySystem.Markers.Clear();
            ucMarkerList1.Update(true);

            RaiseSelectedMarkerChanged();

            _workspace.Invalidate(true);
        }

        void ucMarkerList1_DeletedMarker(object sender, EventArgs e)
        {
            int index = ucMarkerList1.SelectedMarkerIndex;
            if (index < 0) return;

            MetrologySystem.Markers.RemoveAt(index);
            ucMarkerList1.Update(true);

            RaiseSelectedMarkerChanged();
        }

        void ucMarkerList1_SelectedMarkerChanged(object sender, EventArgs e)
        {
            int index = ucMarkerList1.SelectedMarkerIndex;
            if (index < 0) return;

            RaiseSelectedMarkerChanged();
        }

        protected virtual void RaiseSelectedMarkerChanged()
        {
            if (SelectedMarkerChanged != null)
            {
                this.SelectedMarkerChanged(this, EventArgs.Empty);
            }
        }

        public virtual MarkerPoint SelectedMarker
        {
            get
            {
                int index = ucMarkerList1.SelectedMarkerIndex;
                if (index < 0) return null;

                return MetrologySystem.Markers[index];
            }

            set
            {
                if (value == null)
                {
                    ucMarkerList1.SelectedMarkerIndex = -1;
                    return;
                }

                int index = MetrologySystem.Markers.IndexOf(value);
                if (index < 0)
                {
                    ucMarkerList1.SelectedMarkerIndex = -1;
                    return;
                }

                ucMarkerList1.SelectedMarkerIndex = index;
            }
        }

        public virtual void UpdateMarkerLocation(MarkerPoint marker)
        {
            ucMarkerList1.UpdateMarkerLocation(marker);
        }
        #endregion Marker List

        #region Hint Processor
        protected virtual void InitializeHintProcessor()
        {
            AlignmentSettings defaultSettings = new ABSAlignmentSettings();
            UpdateHintProcessor(defaultSettings);
        }
        #endregion Hint Processor

        #region Events
        private void btnSave_Click(object sender, EventArgs e)
        {
            this.DoCommandSaveMetrologySytem();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            this.DoCommandLoadMetrologySystem();
        }

        private void btnCreateReferenceFile_Click(object sender, EventArgs e)
        {
            this.DoCommandCreateReferenceFile();
        }

        private void btnRefine_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void mn_ItemClicked(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = sender as ToolStripMenuItem;
            if (clickedItem.Name == openToolStripMenuItem.Name)
            {
                DoCommandLoadMetrologySystem();
            }
            else if (clickedItem.Name == saveToolStripMenuItem.Name)
            {
                DoCommandSaveMetrologySytem();
            }
            else if (clickedItem.Name == exitToolStripMenuItem.Name)
            {
                DoCommandClose();
            }
            else if (clickedItem.Name == importToolStripMenuItem.Name)
            {
                DoCommandImport();
            }
            else if (clickedItem.Name == createReferenceFileToolStripMenuItem.Name)
            {
                DoCommandCreateReferenceFile();
            }
            else if (clickedItem.Name == calibrateToolStripMenuItem.Name)
            {
                DoCommandCalibrate();
            }
            else if (clickedItem.Name == hintMarkersToolStripMenuItem.Name)
            {
                DoCommandHint();
            }
            else if (clickedItem.Name == refineToolStripMenuItem.Name)
            {
                DoCommandRefine();
            }
            else if (clickedItem.Name == createRegionsToolStripMenuItem.Name)
            {
                DoCommandCreateRegions();
            }
            else if (clickedItem.Name == applyToWorkspaceToolStripMenuItem.Name)
            {
                DoCommandApply();
            }
        }
        #endregion Events

        #region Methods
        #endregion Methods

        #region DoCommands
        protected virtual void DoCommandLoadMetrologySystem()
        {            
            try
            {
                using (OpenFileDialog dlg = new OpenFileDialog())
                {
                    dlg.Title = "Please specify file name";
                    dlg.Filter = MetrologySystem.Filter;
                    dlg.RestoreDirectory = true;

                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        MetrologySystem ms = MetrologySystem.Deserialize(dlg.FileName);
                        MetrologySystem.CopyFrom(ms);

                        this.Update(true);
                    }
                }
            }
            catch (System.Exception exp)
            {
                this.ShowError(string.Format(
                    "Failed to load!\nMessage: {0}\nStackTrace: {1}",
                    exp.Message, exp.StackTrace));
            }
            finally
            {
            }
        }

        public virtual void DoCommandSaveMetrologySytem()
        {
            try
            {
                using (SaveFileDialog dlg = new SaveFileDialog())
                {
                    dlg.Title = "Please specify file name";
                    dlg.Filter = MetrologySystem.Filter;
                    dlg.RestoreDirectory = true;

                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        MetrologySystem.Serialize(dlg.FileName);
                    }
                }
            }
            catch (System.Exception exp)
            {
                this.ShowError(string.Format(
                    "Failed to save!\nMessage: {0}\nStackTrace: {1}",
                    exp.Message, exp.StackTrace));
            }
            finally
            {
            }
        }

        public virtual void DoCommandCreateReferenceFile()
        {
            if (MetrologySystem.Markers.Count < 6)
            {
                DialogResult dlgResult = MessageBox.Show(
                    this,
                    string.Format("At least 6 markers should be defined for detecting Coordinate System automatically.\nDo you want to continue creating Reference File with {0} marker(s)?", MetrologySystem.Markers.Count),
                    "Warning",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (dlgResult == DialogResult.No)
                    return;
            }

            ABSMetrologySystemReference referenceFile =
                    ABSReferenceFileProcessor.CreateReferenceFile(
                    _workspace.Image.RasterImage, MetrologySystem, Regions);

            DlgCreateReferenceFile dlg = 
                new DlgCreateReferenceFile(_workspace.Image, MetrologySystem, referenceFile);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
            }
            else
            {
                return;
            }


            //try
            //{
            //    if (MetrologySystem.Markers.Count < 6)
            //    {
            //        DialogResult dlgResult = MessageBox.Show(
            //            this, 
            //            string.Format("At least 6 markers should be defined for detecting Coordinate System automatically.\nDo you want to continue creating Reference File with {0} marker(s)?", MetrologySystem.Markers.Count), 
            //            "Warning", 
            //            MessageBoxButtons.YesNo, 
            //            MessageBoxIcon.Warning);
                    
            //        if (dlgResult == DialogResult.No)
            //            return;
            //    }

            //    ABSMetrologySystemReference referenceFile =
            //        ABSReferenceFileProcessor.CreateReferenceFile(
            //        _workspace.Image.RasterImage, MetrologySystem);

            //    using (SaveFileDialog dlgBrowser = new SaveFileDialog())
            //    {
            //        dlgBrowser.Title = "Please specify file name";
            //        dlgBrowser.Filter = MetrologySystemReference.FileFilter;
            //        dlgBrowser.RestoreDirectory = true;

            //        if (dlgBrowser.ShowDialog(this) == DialogResult.OK)
            //        {
            //            referenceFile.Serialize(dlgBrowser.FileName);
            //        }
            //    }

                
            //}
            //catch (System.Exception exp)
            //{
            //    this.ShowError(string.Format(
            //        "Failed to save!\nMessage: {0}\nStackTrace: {1}",
            //        exp.Message, exp.StackTrace));
            //}
            //finally
            //{
            //}

            //using (Bitmap bmp = _workspace.Image.CreateBitmap())
            //{
            //    using (Graphics grph = Graphics.FromImage(bmp))
            //    {
            //        using (Pen pen = new Pen(Color.Red, 2.0f))
            //        {
            //            using (System.Drawing.Drawing2D.Matrix m = 
            //                new System.Drawing.Drawing2D.Matrix())
            //            {
            //                float x = referenceFile.DeviceLeft;
            //                float y = referenceFile.DeviceTop;
            //                m.RotateAt(
            //                    -referenceFile.DeviceOrientation,
            //                    new PointF(x, y));

            //                PointF pt = new PointF(x, y);
            //                PointF[] pts = new PointF[] { 
            //                    new PointF(x + 1000, y),
            //                    new PointF(x, y + 1000)
            //                };

            //                m.TransformPoints(pts);

            //                grph.DrawLine(pen, pt, pts[0]);
            //                grph.DrawLine(pen, pt, pts[1]);
            //            }
            //        }
            //    }

            //    bmp.Save(@"D:\temp\xyratex\abs_draft.bmp");
            //}
        }

        private void DoCommandClose()
        {
            this.Close();
        }

        private void DoCommandHint()
        {
            MetrologySystemReference refFile = 
                ABSMetrologySystemReference.GetDefaultABSMetrologySystemReference();

            if (refFile == null)
            {
                string filePath = @"D:\WorkingSpace\ABS\Main\Docs\Requirements\Team\initial_ref_file.msr";

                using (OpenFileDialog dlg = new OpenFileDialog())
                {
                    dlg.Title = "Please specify file name";
                    dlg.Filter = MetrologySystemReference.FileFilter;
                    dlg.RestoreDirectory = true;

                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        filePath = dlg.FileName;
                        //referenceFile.Serialize(dlg.FileName);
                    }
                    else return;
                }

                refFile =
                        ABSMetrologySystemReference.Deserialize(filePath);
            }

            if (refFile == null)
            {
                MessageBox.Show("Cannot load default parameters!");
                return;
            }

            AlignerBase aligner = new ABSAligner(refFile.AlignmentSettings);

            double leftRoi = 0, topRoi = 0, widthRoi = 0, heightRoi = 0;
            double angleRoi = 0;
            aligner.ScanBoundary(
                    _workspace.Image.RasterImage,
                    ref leftRoi, ref topRoi, ref widthRoi, ref heightRoi, ref angleRoi);
            
            double scaleX = widthRoi / refFile.AlignmentSettings.NewWidth;
            double scaleY = heightRoi / refFile.AlignmentSettings.NewHeight;
            refFile.AlignmentSettings.UpdateSampleCoordinateByScale(scaleX, scaleY);
            // because this is hint-step
            // i will expand more scan area
            // Cong.
            if (widthRoi > 100 && heightRoi > 100)
            {
                scaleX = refFile.AlignmentSettings.NewWidth / widthRoi;
                scaleY = refFile.AlignmentSettings.NewHeight / heightRoi;
                scaleX = Math.Max(2, scaleX);
                scaleY = Math.Max(2, scaleY);

                refFile.AlignmentSettings.SampleExpandWidth = 
                    (int)(refFile.AlignmentSettings.SampleExpandWidth * scaleX);

                refFile.AlignmentSettings.SampleExpandHeight =
                    (int)(refFile.AlignmentSettings.SampleExpandHeight * scaleY);
            }

            AlignmentResult alignResult = aligner.Align(_workspace.Image.RasterImage);
            float newWidth = refFile.AlignmentSettings.NewWidth;
            float newHeight = refFile.AlignmentSettings.NewHeight;
            float newAngle = alignResult.GetRotateAngle(newWidth, newHeight);
            float newLeft = alignResult.GetLeft(newWidth, newHeight);
            float newTop = alignResult.GetTop(newWidth, newHeight);

            CoordinateSystem cs = MetrologySystem.CurrentCoordinateSystem;
            PointF[] pts = new PointF[] { PointF.Empty };            
            pts[0] = refFile.TransformToLTDeviceCoordinate(
                refFile.MetrologySystem.CurrentCoordinateSystem.GetOriginPointF());

            // transform to abs-left-top coordinate
            pts[0] =
                MetrologySystemReference.TransformToImageCoordinate(pts[0], newLeft, newTop, newAngle);

            // update marker list
            int sampleCount = refFile.AlignmentSettings.SampleCount;
            List<MarkerPoint> detectedMarkers = new List<MarkerPoint>(sampleCount);
            for (int iSample = 0; iSample < sampleCount; iSample++)
            {
                if (alignResult.MatchedConfidences[iSample] < alignResult.HintThresholdConfidence)
                    continue;
                
                MarkerRegion marker = new MarkerRegion();
                marker.DrawingX = (float)alignResult.MatchedXList[iSample];
                marker.DrawingY = (float)alignResult.MatchedYList[iSample];

                detectedMarkers.Add(marker);
            }

            if (detectedMarkers.Count > 0)
            {
                MetrologySystem.Markers.Clear();
                MetrologySystem.Markers.AddRange(detectedMarkers);
                ucMarkerList1.Update(true);

                _workspace.Invalidate(true);
            }
            else
            {
                MessageBox.Show(
                    this, 
                    "Auto-hint marker processor cannot detect marker.", 
                    "Hint Marker", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }            
        }

        private void DoCommandCalibrate()
        {
            if (Calibrate != null)
            {
                this.Calibrate(this, EventArgs.Empty);
            }
        }

        private void DoCommandImport()
        {
            try
            {
                string filePath = @"D:\WorkingSpace\ABS\Main\Docs\Requirements\Team\initial_ref_file.msr";

                using (OpenFileDialog dlg = new OpenFileDialog())
                {
                    dlg.Title = "Please specify file name";
                    dlg.Filter = MetrologySystemReference.FileFilter;
                    dlg.RestoreDirectory = true;

                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        filePath = dlg.FileName;
                        //referenceFile.Serialize(dlg.FileName);
                    }
                    else return;
                }

                ABSMetrologySystemReference refFile =
                        ABSMetrologySystemReference.Deserialize(filePath);

                MetrologySystem.CopyFrom(refFile.MetrologySystem);
                ucUnitConfiguration1.Update(true);
                ucCoordinateSystem1.Update(true);
                ucMarkerList1.Update(true);
                _regions = SiGlaz.Common.GraphicsList.FromBytes(refFile.Regions);

                _workspace.AppWorkspace.UpdateUI();
                _workspace.Invalidate(true);
            }
            catch
            {
            }
        }

        private void DoCommandRefine()
        {

        }

        private void DoCommandCreateRegions()
        {
            using (SIA.UI.MaskEditor.MaskEditor dlg = new SIA.UI.MaskEditor.MaskEditor(_workspace.Image, _workspace.ImageViewer.RasterImageRender))
            {
                // register for apply mask event
                dlg.ApplyMask += new ApplyMask(MaskEditor_ApplyMask);

                MetrologySystem tmp = new MetrologySystem();
                tmp.CurrentUnit.CopyFrom(MetrologySystem.CurrentUnit);
                tmp.CurrentCoordinateSystem.CopyFrom(MetrologySystem.CurrentCoordinateSystem);
                tmp.RebuildTransformer();
                dlg.MetrologySystem = tmp;

                dlg.WindowState = FormWindowState.Maximized;

                dlg.InitialRegions = (_regions != null) ? _regions.Clone() : null;
                //dlg.InitialRegions = _regions;

                // show mask editor window
                dlg.ShowDialog(_workspace);                

                // unregister for apply mask event
                dlg.ApplyMask -= new ApplyMask(MaskEditor_ApplyMask);
            }
        }

        private void DoCommandApply()
        {
            ImageAnalyzer analyzer = _workspace.GetAnalyzer("ImageAnalyzer") as ImageAnalyzer;
            analyzer.SetCoordinateSystemVisibleFlag(true);
            analyzer.SetMetrologySystem(MetrologySystem);

            this.Close();
        }
        #endregion DoCommands

        #region Helpers
        protected virtual void Update(bool toControl)
        {
            if (toControl)
            {
                ucCoordinateSystem1.Update(true);
                ucMarkerList1.Update(true);

                _workspace.Invalidate(true);
            }
            else
            {
            }
        }

        protected virtual void ShowError(string msg)
        {
            MessageBox.Show(
                this, msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        #endregion Helpers
    }
}
