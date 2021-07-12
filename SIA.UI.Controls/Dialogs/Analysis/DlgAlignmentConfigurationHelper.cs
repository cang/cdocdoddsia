using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SiGlaz.Common.ImageAlignment;
using SiGlaz.Common;
using SIA.Algorithms.Preprocessing.Alignment;
using System.Drawing.Drawing2D;
using SIA.UI.Components.Helpers;
using SIA.IPEngine;
using SIA.Algorithms.Preprocessing.Interpolation;
using System.Drawing.Imaging;

namespace SIA.UI.Controls.Dialogs
{
    public partial class DlgAlignmentConfigurationHelper : Form //FloatingFormBase
    {
        #region Member fields
        private AlignmentSettings _defaultAlignmentSettings = null;
        private AlignerBase _defaultAligner = null;
        public bool IsABSAlignment
        {
            get
            {
                return (_defaultAlignmentSettings != null &&
                    _defaultAlignmentSettings is ABSAlignmentSettings);
            }
        }

        public void UpdateSettings(
            AlignmentSettings settings, bool isABSAlignmentSettings)
        {
            if (_defaultAlignmentSettings is ABSAlignmentSettings && isABSAlignmentSettings)
                return;

            _defaultAlignmentSettings = settings;

            if (isABSAlignmentSettings)
            {
                _defaultAligner = new ABSAligner(_defaultAlignmentSettings);
            }
            else
            {
                _defaultAligner = new PoleTipAligner(_defaultAlignmentSettings);
            }

            InitializeHintProcessor();

            _workspace.Invalidate(true);
        }

        private ImageWorkspace _workspace = null;

        private UIAlignmentSettings _alignmentSettings = null;
        public UIAlignmentSettings AlignmentSettings
        {
            get { return _alignmentSettings; }
            set
            {
                if (_alignmentSettings != value)
                {
                    _alignmentSettings = value;

                    _alignmentSettings.AddedNewSample += new EventHandler(AlignmentSettings_AddedNewSample);
                    _alignmentSettings.SelectedSampleChanged += new EventHandler(AlignmentSettings_SelectedSampleChanged);
                    _alignmentSettings.SelectedSampleRegionLocationChanged += new EventHandler(AlignmentSettings_SelectedSampleRegionLocationChanged);
                }
            }
        }

        #endregion Member fields

        #region Constructors and destructors
        public DlgAlignmentConfigurationHelper(ImageWorkspace workSpace)
            : base()
        {
            InitializeComponent();

            listView.View = View.Details;
            listView.MultiSelect = false;

            // initialize top level window
            this.TopLevel = true;

            _workspace = workSpace;

            if (_workspace != null)
            {
                this.Owner = _workspace.FindForm();
            }

            InitializeHintProcessor();            
        }

        void AlignmentSettings_AddedNewSample(object sender, EventArgs e)
        {
            UpdateSampleRegion(
                _alignmentSettings.ABSRegion,
                _alignmentSettings.SelectedIndex,
                _alignmentSettings.SampleXCoordinates[_alignmentSettings.SelectedIndex],
                _alignmentSettings.SampleYCoordinates[_alignmentSettings.SelectedIndex]);
        }

        void AlignmentSettings_SelectedSampleChanged(object sender, EventArgs e)
        {
            if (listView.SelectedIndices != null)
                    listView.SelectedIndices.Clear();

            if (_alignmentSettings.SelectedIndex < 0)
            {
            }
            else
            {
                listView.SelectedIndices.Add(_alignmentSettings.SelectedIndex);
                listView.Update();
            }
        }

        void AlignmentSettings_SelectedSampleRegionLocationChanged(object sender, EventArgs e)
        {
            int index = _alignmentSettings.SelectedIndex;
            if (index < 0)
                return;

            UpdateSampleRegion(
                _alignmentSettings.ABSRegion, index,
                _alignmentSettings.SampleXCoordinates[index],
                _alignmentSettings.SampleYCoordinates[index]);
        }

        private void InitializeHintProcessor()
        {
            if (_defaultAlignmentSettings == null)
            {
                //_defaultAlignmentSettings = Settings.GetDefaultABSAlignmentSettings();
                //_defaultAlignmentSettings.IsABSAlignmentSettings = true;
                _defaultAlignmentSettings = new ABSAlignmentSettings();
            }

            if (_defaultAligner == null)
            {
                _defaultAligner = new ABSAligner(_defaultAlignmentSettings);
            }

            if (_alignmentSettings == null || 
                listView.Items.Count > 0)
            {
                ClearSampleList();
            }

            this.AlignmentSettings = new UIAlignmentSettings();
            //_alignmentSettings.ABSRegion.ExpandedRight = _defaultAlignmentSettings.ExpandedRight;

            DefaultABSRegion();
            DefaultSampleSize();
            //DefaultSampleRegions();
        }

        private void DefaultABSRegion()
        {
            _alignmentSettings.ABSRegion = new RectRegion();
            //_alignmentSettings.ABSRegion.ExpandedRight = _defaultAlignmentSettings.ExpandedRight;
            _alignmentSettings.ABSRegion.Width = _defaultAlignmentSettings.NewWidth;
            _alignmentSettings.ABSRegion.Height = _defaultAlignmentSettings.NewHeight;
            UpdateABSRegion(_alignmentSettings.ABSRegion, true);
        }

        private void DefaultSampleRegions()
        {
            _alignmentSettings.SampleXCoordinates.Clear();
            _alignmentSettings.SampleYCoordinates.Clear();

            _alignmentSettings.SampleXCoordinates.AddRange(_defaultAlignmentSettings.SampleXCoordinates);
            _alignmentSettings.SampleYCoordinates.AddRange(_defaultAlignmentSettings.SampleYCoordinates);

            UpdateSampleList(
                _alignmentSettings.ABSRegion,
                _alignmentSettings.SampleXCoordinates,
                _alignmentSettings.SampleYCoordinates, true);
        }

        private void DefaultSampleSize()
        {
            _alignmentSettings.SampleWidth = _defaultAlignmentSettings.SampleWidth;
            _alignmentSettings.SampleHeight = _defaultAlignmentSettings.SampleHeight;

            UpdateSampleSize(
                ref _alignmentSettings.SampleWidth,
                ref _alignmentSettings.SampleHeight, true);
        }
        #endregion Constructors and destructors

        #region Override routines
        protected override void OnClosing(CancelEventArgs e)
        {
            // cancel the closing operation just hide this window out
            e.Cancel = true;
            this.Visible = false;

            _workspace.Focus();
            _workspace.Invalidate(true);

            base.OnClosing(e);
        }
        #endregion Override routines
              
        #region ABS Region
        private void btnABSRegionApplyChanges_Click(object sender, EventArgs e)
        {
            UpdateABSRegion(
                _alignmentSettings.ABSRegion, false);

            UpdateSampleList(
                _alignmentSettings.ABSRegion,
                _alignmentSettings.SampleXCoordinates,
                _alignmentSettings.SampleYCoordinates, true);

            _workspace.Invalidate(true);
        }

        private void btnHint_Click(object sender, EventArgs e)
        {
            if (_workspace == null ||
                _workspace.Image == null ||
                _workspace.Image.RasterImage == null)
                return;

            if (_defaultAligner == null)
                return;

            _defaultAligner.DetectDraftRegion(
                _workspace.Image.RasterImage, _alignmentSettings.ABSRegion);

            UpdateABSRegion(_alignmentSettings.ABSRegion, true);

            UpdateSampleList(
                _alignmentSettings.ABSRegion,
                _alignmentSettings.SampleXCoordinates,
                _alignmentSettings.SampleYCoordinates, true);

            _workspace.Invalidate(true);
        }

        private void UpdateABSRegion(RectRegion region, bool toControl)
        {
            try
            {
                if (toControl)
                {
                    nudX.Value = (decimal)region.X;
                    nudY.Value = (decimal)region.Y;
                    nudRegionWidth.Value = (decimal)region.Width;
                    nudRegionHeight.Value = (decimal)region.Height;
                    nudOrientation.Value = (decimal)region.Orientation;
                }
                else
                {
                    region.X = (double)nudX.Value;
                    region.Y = (double)nudY.Value;
                    region.Width = (double)nudRegionWidth.Value;
                    region.Height = (double)nudRegionHeight.Value;
                    region.Orientation = (double)nudOrientation.Value;
                }
            }
            catch
            {
            }
        }
        #endregion ABS Region

        #region Sample Regions
        private void btnSampleSizeApplyChanges_Click(object sender, EventArgs e)
        {
            UpdateSampleSize(
                ref _alignmentSettings.SampleWidth,
                ref _alignmentSettings.SampleHeight, false);

            _workspace.Invalidate(true);
        }

        private void btnSampleListDefault_Click(object sender, EventArgs e)
        {
            DefaultSampleRegions();

            _alignmentSettings.SelectedIndex = -1;

            _workspace.Invalidate(true);            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listView.SelectedIndices.Count > 0)
            {
                int index = _alignmentSettings.SelectedIndex;
                _alignmentSettings.RemoveAt(index);
                UpdateSampleList(
                    _alignmentSettings.ABSRegion,
                    _alignmentSettings.SampleXCoordinates,
                    _alignmentSettings.SampleYCoordinates,
                    true);
                _workspace.Invalidate(true);
            }
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            _alignmentSettings.RemoveAll();
            UpdateSampleList(
                _alignmentSettings.ABSRegion,
                _alignmentSettings.SampleXCoordinates,
                _alignmentSettings.SampleYCoordinates,
                true);
            _workspace.Invalidate(true);
        }
       
        private void InteractiveModeChanged(object sender, EventArgs e)
        {
            _alignmentSettings.InteractiveMode = 
                (rdSelection.Checked ? eInteractiveMode.Selection : eInteractiveMode.Insertion);
        }

        private void UpdateSampleSize(
            ref int sampleWidth, ref int sampleHeight, bool toControl)
        {
            if (toControl)
            {
                nudSampleWidth.Value = (decimal)sampleWidth;
                nudSampleHeight.Value = (decimal)sampleHeight;
            }
            else
            {
                sampleWidth = (int)nudSampleWidth.Value;
                sampleHeight = (int)nudSampleHeight.Value;
            }
        }

        private void UpdateSampleList(
            RectRegion region,
            List<int> xCoordinates, List<int> yCoordinates,
            bool toControl)
        {
            if (toControl)
            {
            }
            else
            {
            }

            this.ClearSampleList();

            using (System.Drawing.Drawing2D.Matrix m = region.Transform)
            {
                int n = xCoordinates.Count;
                for (int i = 0; i < n; i++)
                {
                    double x = region.X + xCoordinates[i];
                    double y = region.Y + yCoordinates[i];
                    PointF[] pts = new PointF[] { new PointF((float)x, (float)y) };
                    m.TransformPoints(pts);

                    x = pts[0].X;
                    y = pts[0].Y;

                    InsertItem(i + 1, x, y);
                }
            }
        }

        private void InsertItem(int id, double x, double y)
        {
            // create new item
            ListViewItem item =
                new ListViewItem(
                    new string[] { id.ToString(), x.ToString("0.00"), y.ToString("0.00") });

            // create item image list here
            Image thumbnail = _alignmentSettings.GetThumbnails(
                _workspace.Image.RasterImage, id - 1, 
                thumbnails.ImageSize.Width, thumbnails.ImageSize.Height);
            if (thumbnail != null)
            {
                thumbnails.Images.Add(thumbnail);
            }
            else
            {
                thumbnails.Images.Add(new Bitmap(
                    thumbnails.ImageSize.Width, thumbnails.ImageSize.Height));
            }

            // update image for new item
            item.ImageIndex = id - 1;

            // add new item to list
            listView.Items.Add(item);

            this.listView.Update();
        }

        private void UpdateSampleRegion(
            RectRegion region, int index, double x, double y)
        {
            using (System.Drawing.Drawing2D.Matrix m = region.Transform)
            {
                PointF[] pts = new PointF[] { new PointF((float)x, (float)y) };
                m.TransformPoints(pts);

                x = pts[0].X;
                y = pts[0].Y;
            }

            if (index == listView.Items.Count) // new item
            {
                InsertItem(index + 1, x, y);
                _alignmentSettings.RaiseSelectedSampleChanged();
            }
            else
            {
                // update item here
                listView.Items[index].SubItems[1].Text = x.ToString("0.00");
                listView.Items[index].SubItems[2].Text = y.ToString("0.00");

                // update thumbnail
                Image oldThumbnail = thumbnails.Images[index];
                if (oldThumbnail != null)
                    oldThumbnail.Dispose();

                Image thumbnail = _alignmentSettings.GetThumbnails(
                _workspace.Image.RasterImage, index,
                thumbnails.ImageSize.Width, thumbnails.ImageSize.Height);
                if (thumbnail != null)
                {
                    thumbnails.Images[index] = thumbnail;
                }
                else
                {
                    thumbnails.Images[index] = new Bitmap(
                        thumbnails.ImageSize.Width, thumbnails.ImageSize.Height);
                }
            }

            this.Invalidate(true);
        }
        #endregion Sample Regions

        #region Save & Load
        private void Update(bool toControl)
        {
            UpdateABSRegion(_alignmentSettings.ABSRegion, toControl);
            UpdateSampleSize(
                ref _alignmentSettings.SampleWidth, 
                ref _alignmentSettings.SampleHeight, toControl);

            if (toControl)
            {
                UpdateSampleList(
                    _alignmentSettings.ABSRegion,
                    _alignmentSettings.SampleXCoordinates,
                    _alignmentSettings.SampleYCoordinates, toControl);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog dlg = new OpenFileDialog())
                {
                    dlg.Title = "Choose file name";
                    dlg.Filter = "Aligment configuration (*.align)|*.align";
                    dlg.RestoreDirectory = true;
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        UIAlignmentSettings newSettings = UIAlignmentSettings.Deserialize(dlg.FileName);

                        this.AlignmentSettings = newSettings;
                        this.Update(true);
                        _alignmentSettings.SelectedIndex = -1;
                        _workspace.Invalidate(true);

                        MessageBox.Show("Successfully!");
                    }
                }
            }
            catch (System.Exception exp)
            {
                MessageBox.Show(
                    this,
                    string.Format("Failed to load from file!\nMessage:\n{0}", exp.Message),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog dlg = new SaveFileDialog())
                {
                    dlg.Title = "Specify file name";
                    dlg.Filter = "Aligment configuration (*.align)|*.align";
                    dlg.RestoreDirectory = true;
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        this.Update(false);

                        _alignmentSettings.Serialize(dlg.FileName);

                        MessageBox.Show("Successfully!");
                    }
                }
            }
            catch (System.Exception exp)
            {
                MessageBox.Show(
                    this, 
                    string.Format("Failed to save to file!\nMessage:\n{0}", exp.Message), 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                this.Update(false);

                int t = 7;
                if (!(_defaultAlignmentSettings is ABSAlignmentSettings))
                    t = 4;

                if (_alignmentSettings.SampleXCoordinates.Count < t ||
                    _alignmentSettings.SampleYCoordinates.Count < t)
                {
                    MessageBox.Show(
                    this,
                    string.Format("At least 7 sample regions should been defined!"),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (SaveFileDialog dlg = new SaveFileDialog())
                {
                    dlg.Title = "Specify file name";
                    dlg.Filter = "Aligment settings (*.settings)|*.settings";
                    dlg.RestoreDirectory = true;
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        Settings alignmentSettings = 
                            _alignmentSettings.ToAlignmentSettings(
                            _workspace.Image.RasterImage, _defaultAlignmentSettings is ABSAlignmentSettings);
                        alignmentSettings.Serialize(dlg.FileName);

                        MessageBox.Show("Successfully!");
                    }
                }
            }
            catch (System.Exception exp)
            {
                MessageBox.Show(
                    this,
                    string.Format("Failed to export to settings!\nMessage:\n{0}", exp.Message),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count == 0)
            {
                _alignmentSettings.SelectedIndex = -1;
            }
            else
            {
                _alignmentSettings.SelectedIndex = listView.SelectedIndices[0];
            }
            _workspace.Invalidate(true);
        }

        private void listView_DoubleClick(object sender, EventArgs e)
        {
            if (listView.SelectedIndices.Count > 0)
            {
                _alignmentSettings.SelectedIndex = listView.SelectedIndices[0];

                if (_workspace.ImageViewer.ScaleFactor < 1.0f)
                    _workspace.ImageViewer.ScaleFactor = 1.0f;

                using (Transformer transformer = this._workspace.ImageViewer.Transformer)
                {
                    int index = _alignmentSettings.SelectedIndex;

                    double x = 
                        _alignmentSettings.ABSRegion.X + 
                        _alignmentSettings.SampleXCoordinates[index];
                    double y = 
                        _alignmentSettings.ABSRegion.Y +
                        _alignmentSettings.SampleYCoordinates[index];

                    using (System.Drawing.Drawing2D.Matrix m = _alignmentSettings.ABSRegion.Transform)
                    {
                        PointF[] pts = new PointF[] { new PointF((float)x, (float)y) };
                        m.TransformPoints(pts);
                        x = pts[0].X;
                        y = pts[0].Y;
                    }

                    Point pt = Point.Round(transformer.PointToPhysical(new Point((int)x, (int)y)));
                    this._workspace.ImageViewer.CenterAtPoint(pt);
                }
            }
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            DefaultABSRegion();
            DefaultSampleSize();
            DefaultSampleRegions();

            _alignmentSettings.SelectedIndex = -1;

            _workspace.Invalidate(true);
        }

        private void ClearSampleList()
        {
            listView.Items.Clear();

            foreach (Image image in thumbnails.Images)
            {
                if (image != null)
                {
                    image.Dispose();
                }
            }
            thumbnails.Images.Clear();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExportRegion_Click(object sender, EventArgs e)
        {
#if DEBUG
            SIA.Algorithms.SIADebugger.AutoSacleImages(
                @"d:\WorkingSpace\ABS\Main\Docs\Requirements\Data\Depo Images\Analyze\EditedData",
                @"d:\WorkingSpace\ABS\Main\Docs\Requirements\Data\Depo Images\Analyze\NewEditedData",
                //750, 536);
                1400, 1000);
            return;
#endif

            this.UpdateABSRegion(AlignmentSettings.ABSRegion, false);

            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Title = "Specify file name";
                dlg.RestoreDirectory = true;
                dlg.Filter = "Bitmap (*.bmp)|*.bmp";
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    using (Image bmp = AlignmentSettings.GetROIImage(
                        _workspace.Image.RasterImage))
                    {
                        bmp.Save(dlg.FileName);
                    }
                }
            }
        }
    }

    public enum eInteractiveMode
    {
        Selection = 0,
        Insertion = 1
    }

    public class UIAlignmentSettings : ParameterBase
    {
        public new int CurrentVersion
        {
            get
            {
                return 1;
            }
        }

        public eInteractiveMode InteractiveMode = eInteractiveMode.Selection;

        public RectRegion ABSRegion = new RectRegion();
        public int SampleWidth = 41;
        public int SampleHeight = 41;
        public List<int> SampleXCoordinates = new List<int>();
        public List<int> SampleYCoordinates = new List<int>();
        public int SelectedIndex = -1;

        public event EventHandler AddedNewSample;
        public event EventHandler SelectedSampleChanged;
        public event EventHandler SelectedSampleRegionLocationChanged;        

        public UIAlignmentSettings()
        {
        }

        public void RaiseSelectedSampleRegionLocationChanged()
        {
            if (SelectedSampleRegionLocationChanged != null)
                SelectedSampleRegionLocationChanged(this, EventArgs.Empty);
        }

        public void RaiseSelectedSampleChanged()
        {
            if (SelectedSampleChanged != null)
                SelectedSampleChanged(this, EventArgs.Empty);
        }

        public void AddNew(int x, int y)
        {
            SampleXCoordinates.Add(x);
            SampleYCoordinates.Add(y);

            SelectedIndex = SampleXCoordinates.Count - 1;

            if (AddedNewSample != null)
                AddedNewSample(this, EventArgs.Empty);
        }

        public void RemoveAt(int index)
        {
            if (index >= 0 && index < SampleXCoordinates.Count)
                SampleXCoordinates.RemoveAt(index);

            if (index >= 0 && index < SampleYCoordinates.Count)
                SampleYCoordinates.RemoveAt(index);

            SelectedIndex = -1;
        }

        public void RemoveAll()
        {
            SampleXCoordinates.Clear();
            SampleYCoordinates.Clear();

            SelectedIndex = -1;
        }

        protected override void BaseSerialize(System.IO.BinaryWriter bin)
        {
            base.BaseSerialize(bin); bin.Write(CurrentVersion);

            ABSRegion.Serialize(bin);
            bin.Write(SampleWidth);
            bin.Write(SampleHeight);

            BinarySerializationCommon.Write(bin, SampleXCoordinates.ToArray());
            BinarySerializationCommon.Write(bin, SampleYCoordinates.ToArray());
        }

        protected override void BinDeserialize(System.IO.BinaryReader bin)
        {
            base.BinDeserialize(bin); int version = bin.ReadInt32();

            ABSRegion = RectRegion.Deserialize(bin);
            SampleWidth = bin.ReadInt32();
            SampleHeight = bin.ReadInt32();

            SampleXCoordinates.AddRange(BinarySerializationCommon.ReadIntArray(bin));
            SampleYCoordinates.AddRange(BinarySerializationCommon.ReadIntArray(bin));
        }

        public static UIAlignmentSettings Deserialize(string filename)
        {
            return ParameterBase.BaseDeserialize(filename) as UIAlignmentSettings;
        }

        public Settings ToAlignmentSettings(GreyDataImage src, bool isABSSettings)
        {
            Settings settings = new Settings();
            settings.IsABSAlignmentSettings = isABSSettings;
            settings.ResetSettings(isABSSettings);

            settings.NewWidth = (int)ABSRegion.Width;
            settings.NewHeight = (int)ABSRegion.Height;
            settings.SampleWidth = SampleWidth;
            settings.SampleHeight = SampleHeight;
            settings.SampleXCoordinates = SampleXCoordinates.ToArray();
            settings.SampleYCoordinates = SampleYCoordinates.ToArray();

            int n = SampleXCoordinates.Count;

            ushort[][] sampleData = new ushort[n][];
            
            using (GreyDataImage tmpImage = new GreyDataImage(SampleWidth, SampleHeight))
            {
                for (int i = 0; i < n; i++)
                {
                    unsafe
                    {
                        using (System.Drawing.Drawing2D.Matrix m = new System.Drawing.Drawing2D.Matrix())
                        {
                            float offsetX = 
                                (float)ABSRegion.X + 
                                SampleXCoordinates[i] - 
                                (float)(SampleWidth * 0.5) + 
                                0.5f;
                            float offsetY =
                                (float)ABSRegion.Y + 
                                SampleYCoordinates[i] - 
                                (float)(SampleHeight * 0.5) +
                                0.5f;

                            m.Translate(offsetX, offsetY);
                            m.RotateAt((float)ABSRegion.Orientation, 
                                new PointF((float)ABSRegion.X, (float)ABSRegion.Y));

                            ImageInterpolator.AffineTransform(
                                InterpolationMethod.Bilinear, src, m, tmpImage);

                            sampleData[i] =
                                UnsafePointerToArray(
                                tmpImage._aData, SampleWidth * SampleHeight);
                        }
                    }

#if DEBUG
                    //using (Bitmap bmp = tmpImage.CreateBitmap())
                    //{
                    //    using (Graphics grph = Graphics.FromImage(bmp))
                    //    {
                    //        using (Pen pen = new Pen(Color.Red, 1.0f))
                    //        {
                    //            grph.DrawLine(
                    //                pen, SampleWidth*0.5f, 0, SampleWidth*0.5f, SampleHeight);

                    //            grph.DrawLine(
                    //                pen, 0, SampleHeight * 0.5f, SampleWidth,  0.5f * SampleHeight);

                    //            //tmpImage.SaveImage(
                    //            //    string.Format("D:\\temp\\test\\{0}.bmp", i), SIA.Common.eImageFormat.Bmp);
                    //        }
                    //    }

                    //    bmp.Save(string.Format("D:\\temp\\test\\{0}.bmp", i));
                    //}
#endif
                }
            }

            settings.SampleData = sampleData;
            settings.SampleCount = n;

            //settings.ComputeMean_Stds();

            return settings;
        }

        public Image GetROIImage(GreyDataImage image)
        {
            Image roiImage = null;

            try
            {
                int width = (int)ABSRegion.Width;
                int height = (int)ABSRegion.Height;

                using (GreyDataImage roiGreyDataImage = new GreyDataImage(width, height))
                {
                    using (System.Drawing.Drawing2D.Matrix
                        transform = new System.Drawing.Drawing2D.Matrix())
                    {
                        transform.Translate((float)ABSRegion.X, (float)ABSRegion.Y);
                        transform.RotateAt((float)ABSRegion.Orientation,
                            new PointF((float)ABSRegion.X, (float)ABSRegion.Y));

                        ImageInterpolator.AffineTransform(
                                    InterpolationMethod.Bilinear,
                                    image, transform, roiGreyDataImage);

                        roiImage = roiGreyDataImage.CreateBitmap();
                    }
                }
            }
            catch
            {
                roiImage = null;
            }

            return roiImage;
        }

        public Image GetThumbnails(GreyDataImage src, int index, int width, int height)
        {
            if (src == null)
                return new Bitmap(width, height);

            int orgWidth = width;
            int orgHeight = height;

            Image image = null;
            try
            {
                width = (int)Math.Max(width, SampleWidth);
                height = (int)Math.Max(height, SampleHeight);

                float offsetX =
                                (float)ABSRegion.X +
                                SampleXCoordinates[index] -
                                (float)(width * 0.5) +
                                0.5f;
                float offsetY =
                    (float)ABSRegion.Y +
                    SampleYCoordinates[index] -
                    (float)(height * 0.5) +
                    0.5f;

                using (GreyDataImage tmpImage = new GreyDataImage(width, height))
                {
                    unsafe
                    {
                        using (System.Drawing.Drawing2D.Matrix m = new System.Drawing.Drawing2D.Matrix())
                        {
                            m.Translate(offsetX, offsetY);
                            m.RotateAt((float)ABSRegion.Orientation,
                                new PointF((float)ABSRegion.X, (float)ABSRegion.Y));

                            ImageInterpolator.AffineTransform(
                                InterpolationMethod.Bilinear, src, m, tmpImage);
                        }
                    }

                    image = new Bitmap(orgWidth, orgHeight, PixelFormat.Format24bppRgb);

                    using (Bitmap bmp = tmpImage.CreateBitmap())
                    {
                        using (Graphics grph = Graphics.FromImage(bmp))
                        {
                            using (Pen pen = new Pen(Color.Red, 1.0f))
                            {
                                float l = (float)((width - SampleWidth) * 0.5);
                                float t = (float)((height - SampleHeight) * 0.5);

                                grph.DrawLine(
                                    pen, l + SampleWidth * 0.5f, t, l + SampleWidth * 0.5f, t + SampleHeight);

                                grph.DrawLine(
                                    pen, l, t + SampleHeight * 0.5f, l + SampleWidth, t + 0.5f * SampleHeight);

                                grph.DrawRectangle(pen, l, t,
                                    (float)SampleWidth, (float)SampleHeight);
                            }
                        }

                        using (Graphics grph = Graphics.FromImage(image))
                        {
                            Rectangle srcRect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                            Rectangle dstRect = new Rectangle(0, 0, image.Width, image.Height);
                            grph.DrawImage(bmp, dstRect, srcRect, GraphicsUnit.Pixel);
                        }
                    }
                }
            }
            catch
            {
                if (image != null)
                    image.Dispose();

                image = new Bitmap(orgWidth, orgHeight);
            }

            return image;
        }

        unsafe public static ushort[] UnsafePointerToArray(ushort* src, int length)
        {
            ushort[] a = new ushort[length];

            for (int i = 0; i < length; i++)
            {
                a[i] = src[i];
            }

            return a;
        }
    }
}
