using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.ComponentModel;
using System.Drawing;

namespace SiGlaz.UI.CustomControls.Chart
{
    public class Plot2DSurface : 
        System.Windows.Forms.DataVisualization.Charting.Chart
    {
        #region Member fields
        private double _verticalLine = 0;
        public event EventHandler XValueChanged = null;

        protected Series _serie = null;
        protected ChartArea _chartArea = null;
        protected DataPointCollection _data = null;
        protected Title _title = null;
        #endregion Member fields

        #region Properties
        public int VerticalLineOffset
        {
            get
            {
                try
                {
                    return (int)_verticalLine;
                }
                catch
                {
                    return 0;
                }
            }
        }

        [Browsable(false)]
        public ChartArea ChartArea
        {
            get { return _chartArea; }
        }

        [Browsable(false)]
        public Series Serie
        {
            get { return _serie; }
        }

        [Browsable(false)]
        public DataPointCollection Data
        {
            get { return _data; }
        }

        [Browsable(false)]
        public Title Title
        {
            set { _title = value; }
            get { return _title; }
        }
        #endregion

        #region Constructors and destructors
        public Plot2DSurface()
        {
            Initialize();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private void Initialize()
        {
            this.SuspendLayout();

            _title = new Title();
            this.Titles.Add(_title);

            _chartArea = new ChartArea("LineChartArea");
            
            _chartArea.AxisY.Minimum = 0;
            _chartArea.AxisY.Maximum = 255;
            
            _chartArea.AxisX.IsStartedFromZero = true;

            _chartArea.CursorX.IsUserEnabled = true;
            _chartArea.CursorX.LineColor = Color.Aqua;
            _chartArea.CursorY.IsUserEnabled = true;
            _chartArea.CursorY.LineColor = Color.Aqua;            

            this.ChartAreas.Add(_chartArea);

            _serie = new Series();
            _serie.ChartType = SeriesChartType.FastLine;
            _serie.IsVisibleInLegend = false;
            _serie.BorderWidth = 1;
            _serie.BorderColor = Color.Red;
            _serie.IsXValueIndexed = true;
            _serie.MarkerSize = 3;
            _serie.MarkerColor = Color.Blue;

            this.Series.Add(_serie);

            this.ResumeLayout(false);

            // get default data points
            _data = _serie.Points;
        }
        #endregion Constructors and destructors

        #region Override routines
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            // Set input focus to the chart control
            this.Focus();

            UpdateVerticalLine(false);
        }

        private PointF _curPosition = PointF.Empty;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.X == _curPosition.X && _curPosition.Y == e.Y)
                return;

            if (_curPosition.X != e.X)
            {
                _curPosition.X = e.X;

                _chartArea.CursorX.SetCursorPixelPosition(_curPosition, false);

                UpdateVerticalLine(false);
            }

            if (_curPosition.Y != e.Y)
            {
                _curPosition.Y = e.Y;

                _chartArea.CursorY.SetCursorPixelPosition(_curPosition, false);
            }
        }
        #endregion Override routines

        #region Methods
        public void ResetCursorPosition(int nOldPoint)
        {
            int nPoint = _data.Count;
            if (nPoint > 0 && nOldPoint > 0)
            {
                double ratio = _chartArea.CursorX.Position / nOldPoint;
                _chartArea.CursorX.SetCursorPosition(ratio * nPoint);

                UpdateVerticalLine(true);
            }
        }

        public void UpdateVerticalLine(bool isAlwaysUpdated)
        {
            double verticalLine = _chartArea.CursorX.Position;

            if (verticalLine == _verticalLine)
            {
                if (!isAlwaysUpdated)
                    return;
            }
            else
                _verticalLine = verticalLine;
                        
            if (XValueChanged != null)
                this.XValueChanged(this, EventArgs.Empty);
        }        

        public virtual void UpdateData(Color color, int borderWidth, double[] yPoints)
        {
            this.SuspendLayout();

            _data.Clear();

            _serie.BorderColor = color;
            _serie.BorderWidth = borderWidth;
            _serie.Points.AddY(yPoints);

            this.ResumeLayout(false);
        }

        public virtual void UpdateData(float[] xPoints, float[] yPoints, bool decreasingXValue)
        {
            this.SuspendLayout();

            int nOldPoint = _data.Count;
            _data.Clear();

            if (xPoints != null && yPoints != null)
            {
                int n = xPoints.Length;
                for (int i = 0; i < n; i++)
                    _data.AddXY(xPoints[i], yPoints[i]);

                if (_chartArea.AxisX.IsReversed != decreasingXValue)
                    _chartArea.AxisX.IsReversed = decreasingXValue;

                //if (_chartArea.AxisX.LabelStyle.Format != "{0:0.##}")
                //    _chartArea.AxisX.LabelStyle.Format = "{0:0.##}";
            }

            this.ResumeLayout(false);

            ResetCursorPosition(nOldPoint);
        }

        public virtual void UpdateData(int[] xPoints, int[] yPoints, bool decreasingXValue)
        {
            this.SuspendLayout();

            _data.Clear();

            if (xPoints != null & yPoints != null)
            {
                int n = xPoints.Length;
                for (int i = 0; i < n; i++)
                    _data.AddXY(xPoints[i], yPoints[i]);

                if (_chartArea.AxisX.IsReversed != decreasingXValue)
                    _chartArea.AxisX.IsReversed = decreasingXValue;
            }
            this.ResumeLayout(false);

        }
        #endregion Methods
    }      
}
