using System;
using System.Collections.Generic;
using System.Text;
using SiGlaz.UI.CustomControls;
using System.Drawing;
using SIA.UI.Controls;
using System.Windows.Forms;
using SIA.SystemLayer;
using SIA.UI.Components.Helpers;
using SIA.UI.Controls.Helpers.VisualAnalysis;

namespace SIA.UI.Controls
{
    internal class SIAStatusBarEx : StatusEx
    {
        private MainFrame _appWorkspace = null;
        public MainFrame appWorkspace
        {
            get { return _appWorkspace; }
        }

        private SeparatorStatusEx _intensitySeparator = null;
        private SeparatorStatusEx _pixelLocSeparator = null;
        private SeparatorStatusEx _imageSizeSeparator = null;
        private SeparatorStatusEx _coordinateSeparator = null;
        private SeparatorStatusEx _measurementSeparator = null;
        
        public StateEx _state = null;
        public StateEx _intensity = null;        
        public MousePositionStatusEx _pixelLoc = null;
        public ViewerSizeStatusEx _imageSize = null;
        public StateEx _zoom = null;
        public StateEx _coordinate = null;
        public StateEx _measurement = null;

        private int padding = 2;
        private int _zoomWidth = 70;
        private int _imageSizeWidth = 100;
        private int _pixelLocWidth = 135;
        private int _coordinateWidth = 180;
        private int _measurementWidth = 180;
        private int _intensityWidth = 80;
        private int _separatorWidth = 10;

        private int[] _itemSizes = null;
        private StatusExItem[] _itemList = null;

        private SIAStatusBarEx()
            : base()
		{
			Initialize();
		}

        public new string Text
        {
            get
            {
                if (_state != null)
                    return _state.Text;
                return "";
            }

            set
            {
                if (_state != null)
                {
                    _state.Text = value;
                }
            }
        }

        public SIAStatusBarEx(MainFrame appWorkspace) : this()
        {
            _appWorkspace = appWorkspace;            
        }

        public void RegisterImageViewerEvents()
        {
            // register for events
            ImageWorkspace workspace =
                this._appWorkspace.DocumentWorkspace as ImageWorkspace;
            if (workspace != null)
            {
                workspace.ImageViewer.MouseMove += new MouseEventHandler(ImageViewer_MouseMove);
                workspace.ImageViewer.ScaleFactorChanged += new EventHandler(ImageViewer_ScaleFactorChanged);
            }
        }

        private void ImageViewer_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                ImageViewer imageViewer = sender as ImageViewer;
                if (imageViewer.Image == null) return;

                CommonImage image = imageViewer.Image;
                using (Transformer transformer = imageViewer.Transformer)
                {
                    // convert point from physical coordinate to image coordinate
                    PointF ptMouse = transformer.PointToLogical(new PointF(e.X, e.Y));

                    int xPos = (int)Math.Round(ptMouse.X);
                    int yPos = (int)Math.Round(ptMouse.Y);
                    ushort intensity = 0;
                    if (image.IsValidPixel(xPos, yPos))
                        intensity = image.getPixel(xPos, yPos);

                    // TODO: CONG
                    PointF coordinate = new PointF(xPos, yPos);
                    ImageAnalyzer analyzer = _appWorkspace.ImageView.GetAnalyzer("ImageAnalyzer") as ImageAnalyzer;
                    if (analyzer.DrawCoordinateSystem)
                    {
                        coordinate = analyzer.MetrologySystem.ToRealCoordinate(coordinate);
                        _coordinate.Text =
                            string.Format("Coordinate: {0}; {1}",
                            coordinate.X.ToString("0.###"), coordinate.Y.ToString("0.###"));
                    }
                    else
                    {
                        MetrologyAnalyzer metrologyAnalyzer =
                            _appWorkspace.ImageView.GetAnalyzer("MetrologyAnalyzer") as MetrologyAnalyzer;
                        if (metrologyAnalyzer.ShowCoordinate)
                        {
                            _coordinate.Text =
                                            string.Format("Coordinate: {0}; {1}",
                                            metrologyAnalyzer.XCoordinate.ToString("0.###"), metrologyAnalyzer.YCoordinate.ToString("0.###"));
                        }
                        else
                        {
                            _coordinate.Text = "Coordinate: N/A";
                        }
                    }

                    if (analyzer != null && _appWorkspace.ImageView.IsMeasurementMode)
                    {
                        _measurement.Text =
                            string.Format("Measurement: {0:0.####} {1}",
                            analyzer.MeasurementDistance, analyzer.CurrentMetrologySystem.CurrentUnit.ShortName);
                    }
                    else
                    {
                        _measurement.Text = "Measurement: N/A";
                    }

                    // update pixel and wafer coordinate
                    this.UpdateData(ptMouse, coordinate, intensity);
                }
            }
            catch
            {
            }
        }

        private void ImageViewer_ScaleFactorChanged(object sender, EventArgs e)
        {
            this.UpdateData();
        }

        public void UpdateData()
        {
            ImageWorkspace workspace = this.appWorkspace.ImageView;
            bool isEmpty = workspace.Empty;

            if (isEmpty)
            {
                _pixelLoc.Pt = new PointF(-1, -1);
                _intensity.Text = "Intensity:N/A";
                _imageSize.Pt = new PointF(-1, -1);
                _zoom.Text = "Zoom: N/A";
            }
            else
            {
                CommonImage image = workspace.Image;

                Size size = 
                    new Size(image.Width, image.Height);

                float scaleFactor = 
                    workspace.ImageViewer.ScaleFactor * 100.0F;

                this.UpdateData(size, scaleFactor);
            }
        }

        public void UpdateData(Size size, float zoomScale)
        {
            _imageSize.Pt = new PointF(size.Width, size.Height);

            _zoom.Text = 
                String.Format(
                "Zoom: {0}%", zoomScale.ToString("#0"));
        }

        public void UpdateData(PointF pixelLoc, PointF waferLoc, int intensity)
        {
            _pixelLoc.Pt = pixelLoc;
            _intensity.Text = "Intensity: " + intensity.ToString();
        }

        private void Initialize()
        {
            this.SetStyle(
                ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();

            this.BkColorEnd = 
                Color.FromArgb(((System.Byte)(136)), ((System.Byte)(174)), ((System.Byte)(228)));
            this.BkColorStart = 
                Color.FromArgb(((System.Byte)(218)), ((System.Byte)(234)), ((System.Byte)(253)));
            this.BorderColor = 
                Color.FromArgb(((System.Byte)(0)), ((System.Byte)(45)), ((System.Byte)(150)));
            
            // zoom
            _zoom = new StateEx("ZOOM", Rectangle.Empty, "Zoom: N/A");

            // image size
            _imageSizeSeparator = new SeparatorStatusEx(Rectangle.Empty);
            _imageSize = new ViewerSizeStatusEx(Rectangle.Empty, PointF.Empty, null);

            // pixel location
            _pixelLocSeparator = new SeparatorStatusEx(Rectangle.Empty);
            _pixelLoc = new MousePositionStatusEx(Rectangle.Empty, new PointF(-1, -1), null);

            // intensity
            _intensitySeparator = new SeparatorStatusEx(Rectangle.Empty);
            _intensity = new StateEx("INTENSITY", Rectangle.Empty, "Intensity: N/A");

            // coordinate
            _coordinateSeparator = new SeparatorStatusEx(Rectangle.Empty);
            _coordinate = new StateEx("COORDINATE", Rectangle.Empty, "Coordinate: N/A");

            // measurement
            _measurementSeparator = new SeparatorStatusEx(Rectangle.Empty);
            _measurement = new StateEx("MEASUREMENT", Rectangle.Empty, "Measurement: N/A");

            // state
            _state = new StateEx("STATE", Rectangle.Empty, "");

            _itemList = new StatusExItem[] {
                _zoom, 
                _imageSizeSeparator, _imageSize,
                _pixelLocSeparator, _pixelLoc,
                _intensitySeparator, _intensity,
                _coordinateSeparator, _coordinate,
                _measurementSeparator, _measurement,
                _state
            };

            _itemSizes = new int[] {
                _zoomWidth,
                _separatorWidth, _imageSizeWidth,
                _separatorWidth, _pixelLocWidth,
                _separatorWidth, _intensityWidth,
                _separatorWidth, _coordinateWidth,
                _separatorWidth, _measurementWidth,
                0
            };

            for (int i = 0; i < _itemList.Length; i++)
            {
                this.AddItem(_itemList[i]);
            }

            this.CorrectItemBounds();
        }

        protected override void CorrectItemBounds()
        {
            try
            {
                base.CorrectItemBounds();

                int right = this.Width - padding;
                int left = right;

                int top = padding;
                int h = this.Height - 2*padding;
                
                for (int i = 0; i < _itemList.Length; i++)
                {
                    if (right <= padding)
                        break;

                    StatusExItem item = _itemList[i];
                    int w  = _itemSizes[i];
                    if (w == 0)
                    {
                        item.Bounds = new Rectangle(padding, top, right - padding, h);
                    }
                    else
                    {
                        item.Bounds = new Rectangle(right - w, top, w, h);
                    }
                    right -= w + 1;
                }
            }
            catch (System.Exception exp)
            {
                Console.WriteLine(exp.Message);
                Console.WriteLine(exp.StackTrace);
            }
        }
    }
}
