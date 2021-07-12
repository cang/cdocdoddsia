using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SiGlaz.Common.ImageAlignment;
using SIA.SystemLayer;
using SIA.UI.Controls.Commands;
using SIA.Algorithms.Preprocessing.Alignment;
using SIA.Algorithms.ReferenceFile;

namespace SIA.UI.Controls.Dialogs
{
    public partial class DlgCreateReferenceFile : Form
    {
        private MetrologySystemReference _referenceFile = null;
        private MetrologySystem _definedSystem = null;
        private CommonImage _image = null;
        private MetrologySystem _detectedSystem = null;

        private double _originError = 0;
        private double _angleError = 0;
        private double[] _markerErrors = null;
        private double _markerError = 0;

        public DlgCreateReferenceFile(
            CommonImage image, MetrologySystem definedSystem,
            MetrologySystemReference referenceFile)
        {
            InitializeComponent();

            _image = image;
            _definedSystem = definedSystem;
            _referenceFile = referenceFile;

            DetectCoordinateSystem();
        }

        private void DetectCoordinateSystem()
        {
            AlignerBase aligner = new ABSAligner(_referenceFile.AlignmentSettings);
            AlignmentResult alignResult = aligner.Align(_image.RasterImage);

            _detectedSystem =
                ReferenceFileProcessor.GetMetrologySystem(_referenceFile, alignResult);

            _detectedSystem.CalcError(_definedSystem, 
                ref _originError, ref _angleError, ref _markerError, ref _markerErrors);
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.UpdateInfo();
        }

        private void UpdateInfo()
        {
            // origin
            lbOrigin.Text = 
                string.Format("({0:0.###}; {1:0.###}) (pixel)", 
                _detectedSystem.CurrentCoordinateSystem.DrawingOriginX,
                _detectedSystem.CurrentCoordinateSystem.DrawingOriginY);
            lbOriginError.Text =
                string.Format("Tolerance: {0:0.###} (pixel)", _originError);

            // angle
            lbAngle.Text = 
                string.Format("{0:0.###} (degree)", 
                _detectedSystem.CurrentCoordinateSystem.DrawingAngle);
            lbAngleError.Text =
                string.Format("Tolerance: {0:0.####} (degree)", _angleError);

            // update marker errror
            lbMarkerError.Text =
                string.Format("Detected Marker Tolerance: {0:0.####} (pixel)", _markerError);

            // markers
            int n = (int)Math.Min(
                _detectedSystem.Markers.Count, _definedSystem.Markers.Count);

            for (int i = 0; i < n; i++)
            {
                MarkerPoint marker = _detectedSystem.Markers[i];
                ListViewItem item = new ListViewItem(
                    new string[] { 
                        "",
                        string.Format("{0:0.##}", marker.DrawingX),
                        string.Format("{0:0.##}", marker.DrawingY),
                        string.Format("{0:0.####}", _markerErrors[i])});
                item.Text = string.Format("{0}", i+1);

                listView1.Items.Add(item);
            }

            int factor = 4;
            float scale = 1.0f / factor;
            int newWidth = _image.Width / factor;
            int newHeight = _image.Height / factor;
            Image thumbnail = new Bitmap(newWidth, newHeight);
            using (Graphics grph = Graphics.FromImage(thumbnail))
            {
                RectangleF dstRect = new RectangleF(0, 0, newWidth, newHeight);
                RectangleF srcRect = new RectangleF(0, 0, _image.Width, _image.Height);
                using (Bitmap bmp = _image.CreateBitmap())
                {
                    grph.DrawImage(bmp, dstRect, srcRect, GraphicsUnit.Pixel);
                }

                using (System.Drawing.Drawing2D.Matrix transform = new System.Drawing.Drawing2D.Matrix())
                {
                    transform.Scale(scale, scale);

                    RectangleF rect = new RectangleF(0, 0, _image.Width, _image.Height);
                    
                    // draw coordinate
                    _detectedSystem.CurrentCoordinateSystem.Draw(grph, rect, transform);

                    // draw all markers
                    _detectedSystem.CurrentCoordinateSystem.DrawMarkers(
                        grph, rect, transform, scale, _detectedSystem.Markers);

                    if (_referenceFile.Regions != null)
                    {
                        SiGlaz.Common.GraphicsList regions = SiGlaz.Common.GraphicsList.FromBytes(_referenceFile.Regions);
                        _detectedSystem.RebuildTransformer();
                        regions.MetroSys = _detectedSystem;
                        System.Drawing.Drawing2D.Matrix bkTransform = grph.Transform;
                        grph.Transform = transform;
                        regions.Draw(grph);
                        grph.Transform = bkTransform;
                    }
                }
            }

            // update to picture box
            picThumbnail.Image = thumbnail;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog dlg = new SaveFileDialog())
                {
                    dlg.Title = "Please specify file name";
                    dlg.Filter = MetrologySystemReference.FileFilter;
                    dlg.RestoreDirectory = true;

                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        _referenceFile.Serialize(dlg.FileName);
                    }
                    else return;
                }

                this.Close();
            }
            catch (System.Exception exp)
            {
                ShowError(exp.Message);
            }
        }

        protected virtual void ShowError(string msg)
        {
            MessageBox.Show(
                this, msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
