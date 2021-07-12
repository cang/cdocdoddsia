using System;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading;
using System.Diagnostics;

using SIA.Common;
using SIA.Common.Imaging;
using SIA.Common.Mathematics;

using SIA.SystemLayer;

using SIA.UI.Controls;
using SIA.UI.Controls.Common;

using SIA.SystemFrameworks;
using SiGlaz.UI.CustomControls.Chart;
//using System.Windows.Forms.DataVisualization.Charting;

namespace SIA.UI.Controls.UserControls
{
	public enum PlotType
	{
		Line,
		HorizontalLine,
		VerticalLine,
		HorizontalBox,
		VerticalBox,
		AreaPlot
	}

	public enum BoxProfileOptions
	{
		Minimum,
		Maximum,
		Mean,
		StdDev,
	}

	public enum RenderMode
	{
		Solid,	
		Wireframe,
	}

	/// <summary>
	/// Summary description for DataPlot.
	/// </summary>
	public class DataPlot : UserControl
	{
		object _syncObject = new object();
        Plot2DSurface _plotChart;

		ControlDemo.OpenglScene _areaPlot;
		PlotType _plotType = PlotType.Line;
        bool _autoScale = false;
        int _YAxisMinValue = 0;
        int _YAxisMaxValue = ushort.MaxValue;

		Array _ordinaryData = null;
		Array _abscissaData = null;
		object _selOrdinaryValue = null;
		object _selAbscissaValue = null;
     
        
		TrendLineFormat _trendlineFormat = null;

		public TrendLineFormat TrendLineFormat
		{
			get {return _trendlineFormat;}
			set {_trendlineFormat = value;}
		}

		public event EventHandler SelectedValueChanged = null;

        public Plot2DSurface PlotChart
        {
            get { return _plotChart; }
        }

		public object SelectedOrdinaryValue
		{
			get {return _selOrdinaryValue;}
		}

		public object SelectedAbscissaData
		{
			get {return _selAbscissaValue;}
		}
		
		public PlotType PlotType
		{
			get {return _plotType;}
			set 
			{
				_plotType = value;
				OnPlotTypeChanged();
			}
		}

		protected virtual void OnPlotTypeChanged()
		{
			if (_plotType == PlotType.AreaPlot)
			{
                this._plotChart.Visible = false;
				this._areaPlot.Visible = true;
			}
			else
			{
                this._plotChart.Visible = true;
				this._areaPlot.Visible = false;
			}

			// reset line plot
			this.UpdateLinePlot();
			// reset area plot
			this.UpdateAreaPlot();
		}

        public bool AutoScaleYAxis
        {
            get { return _autoScale; }
            set { _autoScale = value; }
        }

        public int YAxisMinValue
        {
            get { return _YAxisMinValue; }
            set { _YAxisMinValue = value; }
        }

        public int YAxisMaxValue
        {
            get { return _YAxisMaxValue; }
            set { _YAxisMaxValue = value; }
        }

		public DataPlot() 
			: base()
		{
			this.InitializeComponent();

            this._plotChart.XValueChanged += new EventHandler(VerticalLine_ValueChanged);
		}

		private void InitializeComponent()
		{
            this._areaPlot = new ControlDemo.OpenglScene();
            this._plotChart = new Plot2DSurface();
            ((System.ComponentModel.ISupportInitialize)(this._plotChart)).BeginInit();
            this.SuspendLayout();
            // 
            // _areaPlot
            // 
            this._areaPlot.Dock = System.Windows.Forms.DockStyle.Fill;
            this._areaPlot.Location = new System.Drawing.Point(0, 0);
            this._areaPlot.Name = "_areaPlot";
            this._areaPlot.Size = new System.Drawing.Size(240, 176);
            this._areaPlot.TabIndex = 3;
            // 
            // _plotChart
            // 
            this._plotChart.BorderlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(59)))), ((int)(((byte)(105)))));
            this._plotChart.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            this._plotChart.BorderSkin.BackColor = System.Drawing.Color.Red;
            this._plotChart.BorderSkin.BackSecondaryColor = System.Drawing.Color.Blue;
            this._plotChart.BorderSkin.BorderColor = System.Drawing.Color.Yellow;
            this._plotChart.BorderSkin.SkinStyle = System.Windows.Forms.DataVisualization.Charting.BorderSkinStyle.Emboss;
            this._plotChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this._plotChart.Location = new System.Drawing.Point(0, 0);
            this._plotChart.Name = "_plotChart";
            this._plotChart.Size = new System.Drawing.Size(240, 176);
            this._plotChart.TabIndex = 4;
            this._plotChart.Text = "_plotChart";
            // 
            // DataPlot
            // 
            this.Controls.Add(this._plotChart);
            this.Controls.Add(this._areaPlot);
            this.Name = "DataPlot";
            this.Size = new System.Drawing.Size(240, 176);
            ((System.ComponentModel.ISupportInitialize)(this._plotChart)).EndInit();
            this.ResumeLayout(false);

		}
     
		protected override void Dispose(bool disposing)
		{
			base.Dispose (disposing);

            if (this._plotChart != null)
            {
                this._plotChart.XValueChanged -= new EventHandler(VerticalLine_ValueChanged);
                this._plotChart.Dispose();
                this._plotChart = null;
            }

            if (this._areaPlot != null)
            {
                this._areaPlot.Dispose();
                this._areaPlot = null;
            }
        }

        private void VerticalLine_ValueChanged(object sender, EventArgs e)
        {
            lock (_syncObject)
            {
                try
                {
                    int index = _plotChart.VerticalLineOffset;
                    if (index >= 0 && index < this._abscissaData.Length)
                    {
                        this._selAbscissaValue = this._abscissaData.GetValue(index);
                        this._selOrdinaryValue = this._ordinaryData.GetValue(index);

                        if (this.SelectedValueChanged != null)
                            this.SelectedValueChanged(this, EventArgs.Empty);
                    }
                }
                catch (System.Exception exp)
                {
                    Trace.WriteLine(exp);
                }
            }
        }

        public void UpdateLinePlot()
        {
            lock (_syncObject)
            {

                this._plotChart.Title.Text = "Data Profile";
                this._plotChart.Serie.Color = Color.Red;
                this._plotChart.Serie.BorderWidth = 1;
            }
        }

       
        //for line
        public virtual void UpdateLinePlot(string title, float[] intensities, Point[] points)
        {
            lock (_syncObject)
            {
                try
                {
                    // update internal fields
                    this._ordinaryData = intensities;
                    this._abscissaData = points;

                    // update chart properties
                    this._plotChart.Title.Text = title;
                    
                    
                    // determine x-axis belong to x or y
                    Point pt1 = points[0];
                    Point pt2 = points[points.Length - 1];
                    float dx = Math.Abs(pt2.X - pt1.X);
                    float dy = Math.Abs(pt2.Y - pt1.Y);

                    this._plotChart.ChartArea.AxisX.Title = dx > dy ? "Pixel Along Location X" : "Pixel Along Location Y"; ;
                    this._plotChart.ChartArea.AxisY.Title = "Intensity";                    

                    float[] xPoints = new float[intensities.Length];
                    if (dx > dy)
                    {                           
                        for (int ip = 0; ip < intensities.Length; ip++)
                            xPoints[ip] = points[ip].X;
                    }
                    else
                    {
                        for (int ip = 0; ip < intensities.Length; ip++)
                            xPoints[ip] = points[ip].Y;
                    }

                    bool isReversed = xPoints[0] > xPoints[xPoints.Length-1];
                    _plotChart.UpdateData(xPoints, intensities, isReversed);
             
                    
                    // add trend line plot
                    if (_trendlineFormat != null)
                    {
                        double[] values = this.CreateTrendLineData();
                        if (values != null)
                            this._plotChart.UpdateData(Color.Black, 1, values);
                    }
                }
                catch (System.Exception exp)
                {
                    Trace.WriteLine(exp);
                }
                finally
                {
                    this.Invalidate(true);   
                }
            }
        }

        //for box
        public void UpdateLinePlot(string title, float[] intensities, float[] points)
        {
            lock (_syncObject)
            {
                try
                {
                    // update internal fields
                    this._ordinaryData = intensities;
                    this._abscissaData = points;


                    // update chart properties
                    this._plotChart.Title.Text = title;

                   
                    this._plotChart.ChartArea.AxisX.Title = "Statistic Values";
                    this._plotChart.ChartArea.AxisY.Title = "Intensity";

                    if (points != null)
                    {
                        for (int i = points.Length - 1; i >= 0; i--)
                            points[i] = (float)Math.Round(points[i]);
                    }

                    this._plotChart.UpdateData(points, intensities, false);
             
                    // add trend line plot
                    if (_trendlineFormat != null)
                    {
                        double[] values = this.CreateTrendLineData();
                        if (values != null)
                            this._plotChart.UpdateData(Color.Black, 1, values);                        
                    }
                }
                catch (System.Exception exp)
                {
                    Trace.WriteLine(exp);
                }
                finally
                {
                    this.Invalidate(true);   
                }
            }
        }

        public void UpdateAreaPlot()
		{
			_areaPlot.InitGridData(0, 0, 100, 100);

            try
            {
                _areaPlot.UpdateGridData();
            }
            catch
            {
                // failed on the first loading
                // ignore
                _areaPlot.UpdateGridData();
            }

			_areaPlot.EndInitGridData();
			_areaPlot.Refresh();
			_areaPlot.Render();
		}

        public unsafe void UpdateAreaPlot(string title, RenderMode mode, float resX, float resY, Rectangle rect, float[] data)
        {
            try
            {
                _areaPlot.SetRenderMode(mode == RenderMode.Solid ? 1 : 0);
                _areaPlot.SetResolution((int)Math.Floor(resX), (int)Math.Floor(resY));
                int left = rect.X, right = left + rect.Width;
                int top = rect.Y, bottom = top + rect.Height;
                int height = rect.Height, width = rect.Width;
                int index = 0;

                _areaPlot.InitGridData(left, top, right, bottom);

                fixed (float* pData = data)
                {                    
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            _areaPlot.SetGridData(x + left, y + top, data[index++]);
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                Trace.WriteLine(exp);
            }
            finally
            {
                _areaPlot.UpdateGridData();
                _areaPlot.EndInitGridData();
                _areaPlot.Refresh();
                _areaPlot.Render();
            }
        }

		public unsafe void UpdateAreaPlot(string title, RenderMode mode, float resX, float resY, 
            int left, int top, int right, int bottom, float[] data)
		{
			try
			{
				_areaPlot.SetRenderMode(mode == RenderMode.Solid ? 1 : 0);
				_areaPlot.SetResolution((int)Math.Floor(resX), (int)Math.Floor(resY)); 
				_areaPlot.InitGridData(left, top, right, bottom);
				
				fixed (float* pData = data)
                {
                    int index = 0;
                    for (int y = top; y <= bottom; y++)
                    {
                        for (int x = left; x <= right; x++)
                        {
                            _areaPlot.SetGridData(x, y, pData[index++]);
                        }
                    }
                }
			}
			finally
			{
				_areaPlot.UpdateGridData(); 
				_areaPlot.EndInitGridData();
				_areaPlot.Refresh();
				_areaPlot.Render();
			}
		}

		public double[] CreateTrendLineData()
		{
			return this.CreateTrendLineData(_trendlineFormat);
		}

		public double[] CreateTrendLineData(TrendLineFormat trendLineFormat)
		{
			if (_trendlineFormat == null || _ordinaryData == null)
				return null;

			float[] data = (float[])_ordinaryData;
			double[] values = new double[data.Length];
			for (int i=0; i<data.Length; i++)
				values[i] = data[i];

			Trendline trendline = null;
			switch (trendLineFormat.Trenline_Type)
			{
				case eTrendlineType.Logarithmic:
					trendline = Trendline.createLogarithm(values, trendLineFormat);
					break;
				case eTrendlineType.Exponential:
					trendline = Trendline.createExponential(values, trendLineFormat);
					break;
				case eTrendlineType.MiddleAverage:
					trendline = Trendline.createMiddleMovingAverageTrendLine(values, trendLineFormat);
					break;
				case eTrendlineType.MovingAverage:
					trendline = Trendline.createMovingAverageParameter(values, trendLineFormat);
					break;
				case eTrendlineType.Linear:
				default:
					trendline = Trendline.createLinear(values);
					break;
			}

			return trendline.getYValueArray();
		}
    }
}
