using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Windows.Forms;

using SIA.UI.Controls.Utilities;
using System.Windows.Forms.DataVisualization.Charting;

namespace SIA.UI.Controls.UserControls
{
	/// <summary>
	/// Summary description for HistogramViewer.
	/// </summary>
	public class HistogramViewer : System.Windows.Forms.UserControl
	{
		#region Fields member

		private PointF _pointF = new PointF(0,0);
		private bool _bShowCoordinatesChange = false;

		#endregion Fields member
        private SiGlaz.UI.CustomControls.Chart.Plot2DSurface plot2DSurface;

		#region Windows Form Members

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#endregion

		#region Constructor and destructor

		public HistogramViewer()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
            plot2DSurface.Serie.ChartType = SeriesChartType.Area;
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.plot2DSurface = new SiGlaz.UI.CustomControls.Chart.Plot2DSurface();
            ((System.ComponentModel.ISupportInitialize)(this.plot2DSurface)).BeginInit();
            this.SuspendLayout();
            // 
            // plot2DSurface
            // 
            this.plot2DSurface.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plot2DSurface.Location = new System.Drawing.Point(0, 0);
            this.plot2DSurface.Name = "plot2DSurface";
            this.plot2DSurface.Size = new System.Drawing.Size(492, 288);
            this.plot2DSurface.TabIndex = 0;
            title1.Name = "Title1";
            this.plot2DSurface.Title = title1;
            // 
            // HistogramViewer
            // 
            this.Controls.Add(this.plot2DSurface);
            this.Name = "HistogramViewer";
            this.Size = new System.Drawing.Size(492, 288);
            ((System.ComponentModel.ISupportInitialize)(this.plot2DSurface)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		#region Properties

		

		public PointF MousePoint
		{
			get
			{
				return _pointF;
			}
			set
			{
				_pointF = value;
			}
		}
		
		public bool RightMenu
		{
			get
			{
                return false;
				//return (_plotSurface.RightMenu != null);
			}
			set
			{
                //if(value == true)
                //{					
                //    _plotSurface.RightMenu = NPlot.Windows.PlotSurface2D.DefaultContextMenu;
                //    _plotSurface.RightMenu.Menu.MenuItems.RemoveAt(0);
                //    _plotSurface.RightMenu.MenuItems.RemoveAt(0);
                //    _plotSurface.RightMenu.Menu.MenuItems[0].Text = "Show Coordinates";
                //    _plotSurface.RightMenu.Menu.MenuItems[4].Text = "Copy Chart To Clipboard";
                //    _plotSurface.RightMenu.Menu.Popup += new System.EventHandler(this.RightMenu__Popup);											
                //}
                //else
                //{
                //    _plotSurface.RightMenu = null;
                //}
			}
		}

		#endregion

		#region Methods
		
		public void AddLinePlot(object dataSpectrumX, object dataSpectrumY, string label, Color color, float thickness)
		{
            //// Linear spectrum
            //LinePlot lpOriginal = null;
            //try
            //{
            //    lpOriginal = new LinePlot();
				
            //    lpOriginal.OrdinateData = dataSpectrumY;
            //    lpOriginal.AbscissaData = dataSpectrumX;
            //    lpOriginal.Pen = new Pen(color , thickness);
            //    lpOriginal.Label = label;
			
            //    //_plotSurface.Add( lpOriginal );				
            //}
            //catch (System.Exception exp)
            //{
            //    lpOriginal = null;
            //    throw exp;
            //}			
		}

		public void AddHistogramPlot(object dataSpectrumX, object dataSpectrumY, 
            bool bStyle, string label, Color color, Color[] colors, float thickness)
		{
            // Histogram plot
            

            //HistogramPlotEx hisPlot = null;
            try
            {
                float[] xPoints = (dataSpectrumX != null ? (float[])dataSpectrumX : null);
                float[] yPoints = (dataSpectrumY != null ? (float[])dataSpectrumY : null);
                plot2DSurface.UpdateData(xPoints, yPoints, false);

                float max = yPoints[0];
                for (int i = yPoints.Length - 1; i >= 0; i--)
                {
                    if (max < yPoints[i])
                    {
                        max = yPoints[i];
                    }
                }
                plot2DSurface.ChartArea.AxisY.Maximum = (float)Math.Round(max) + 2;
                //plot2DSurface.Title.Text = label;


                //hisPlot = new HistogramPlotEx();
                //hisPlot.HistogramPlotGreyImageStyle = bStyle;
                //hisPlot.OrdinateData = dataSpectrumY;
                //hisPlot.AbscissaData = dataSpectrumX;
                //hisPlot.Filled = true;
                //hisPlot.Pen = new Pen(color, thickness);
                //hisPlot.RectangleBrush = new RectangleBrushes.HorizontalCenterFade(color, color);
                //hisPlot.BaseWidth = 0.5f;
                //hisPlot.Label = label;

                //// update for pseudoColor
                //if (colors != null)
                //    hisPlot.HistogramPlotGreyImageColors = colors;

                //// insert created plot
                //_plotSurface.Add(hisPlot);

                //_plotSurface.XAxis1.Color = ControlPaint.DarkDark(Color.Gray);
                //if (_plotSurface.XAxis2 != null)
                //    _plotSurface.XAxis2.Hidden = true;

                //_plotSurface.YAxis1.Color = ControlPaint.DarkDark(Color.Gray);
                //_plotSurface.TitleColor = ControlPaint.DarkDark(Color.Gray);
            }
            catch (System.Exception exp)
            {
                //hisPlot = null;
                throw exp;
            }			
		}

		public void AddVertialInteraction(Color clrVertical)
		{
            //PlotSurface2D.Interactions.VerticalGuideline _verticalGuideline = null;
            //try
            //{
            //    //_verticalGuideline = new PlotSurface2D.Interactions.VerticalGuideline(clrVertical);
            //    //_plotSurface.AddInteraction(_verticalGuideline);
            //}
            //catch (System.Exception exp)
            //{
            //    _verticalGuideline = null;
            //    throw exp;
            //}
		}

		public void AddHorizontalInteraction(Color clrHorizontal)
		{
            //PlotSurface2D.Interactions.HorizontalGuideline _horizontalGuideline = null;
            //try
            //{
            //    _horizontalGuideline = new PlotSurface2D.Interactions.HorizontalGuideline(clrHorizontal);
            //    _plotSurface.AddInteraction(_horizontalGuideline);
            //}
            //catch (System.Exception exp)
            //{
            //    _horizontalGuideline = null;
            //    throw exp;
            //}
		}

		public void AddLabelsAndTitle(string labelX, string labelY, string title)
		{
            if (labelX != null && labelX.Length > 0)
                plot2DSurface.ChartArea.AxisX.Title = labelX;
            plot2DSurface.ChartArea.AxisY.LabelStyle = new LabelStyle();
            plot2DSurface.ChartArea.AxisY.LabelStyle.Format = "{0:0.##}%";
            plot2DSurface.Title.Text = title;
		}

		public void AddLegend(int numberIntemsVertically, int xOffset, int yOffset)
		{
            //// add legend
            //Legend l = null;
            //try
            //{
            //    l = new Legend();
            //    l.NumberItemsVertically = numberIntemsVertically;
            //    l.AttachTo(NPlot.PlotSurface2D.XAxisPosition.Bottom, NPlot.PlotSurface2D.YAxisPosition.Left);
            //    l.HorizontalEdgePlacement = NPlot.Legend.Placement.Outside;
            //    l.VerticalEdgePlacement = NPlot.Legend.Placement.Inside;
            //    l.XOffset = xOffset;
            //    l.YOffset = yOffset;
            //    l.BorderStyle = NPlot.LegendBase.BorderType.Line;
            //    _plotSurface.Legend = l;
            //}
            //catch (System.Exception exp)
            //{
            //    l = null;
            //    throw exp;
            //}
		}

		public void ExportChartImage(string fileName)
		{
            //if (_plotSurface == null)
            //    throw new ArgumentNullException("_plotSurface");

            //ImageFormat format = kUtils.BitmapFormatFromFileName(fileName);
            //int width = this.Width;
            //int height = this.Height;
            //using (Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb))
            //{
            //    using (Graphics graph = Graphics.FromImage(bitmap))
            //    {
            //        graph.Clear(Color.White);
            //        Rectangle rect = new Rectangle(0, 0, width-1, height-1);
            //        _plotSurface.Draw(graph, rect);
            //    }

            //    bitmap.Save(fileName, format);
            //}
		}

		public Bitmap ExportChartBitmap(int width, int height)
		{
            //if (_plotSurface == null)
            //    throw new ArgumentNullException("_plotSurface");

            //Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            //using (Graphics graph = Graphics.FromImage(bitmap))
            //{
            //    graph.Clear(Color.White);
            //    Rectangle rect = new Rectangle(0, 0, width-1, height-1);
            //    _plotSurface.Draw(graph, rect);
            //}
            //return bitmap;

            return null;
		}		
		
        public void DoCommandPrint()
		{
            //if(_plotSurface != null)
            //    _plotSurface.Print(false);
		}

		public void DoCommandPrintPreview()
		{
            //if(_plotSurface != null)
            //    _plotSurface.Print(true);
		}

		public void DoCommandCopyChartToClipboard()
		{
            //if(_plotSurface != null)
            //{
            //    _plotSurface.CopyToClipboard();
            //}
		}

		public void DoCommandCopyDataToClipboard()
		{
            //if(_plotSurface != null)
            //{
            //    _plotSurface.CopyDataToClipboard();
            //}
		}

		public void PlotSurfaceRefresh()
		{
            //if (_plotSurface != null)
            //    _plotSurface.Refresh();
		}

		public void UpdatePlotSurface()
		{			
            //// Clear Screen
            //_plotSurface.Clear();

            //// add a horizontal grid. 
            //Grid fineGrid = new Grid();			
            //fineGrid.VerticalGridType = Grid.GridType.Fine;
            //fineGrid.HorizontalGridType = Grid.GridType.Fine;
            //fineGrid.MajorGridPen.Color = ControlPaint.Light(Color.Gray);
            //fineGrid.MinorGridPen.Color = ControlPaint.LightLight(Color.Gray);
            //_plotSurface.Add( fineGrid );			

            //// set white back color
            //_plotSurface.PlotBackColor = Color.White;
		}

		
		#endregion Internal Helpers

		#region Events

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
		}

		private void PlotSurface_MouseMove(object sender, MouseEventArgs e)
		{
            //if (_plotSurface.ShowCoordinates)
            //{
            //    if(e.X != _pointF.X || e.Y != _pointF.Y)
            //    {
            //        _pointF.X = e.X;
            //        _pointF.Y = e.Y;
            //        this.PlotSurfaceRefresh();
            //    }
            //}
            //else
            //{
            //    if(e.X != _pointF.X || e.Y != _pointF.Y)
            //    {
            //        _pointF.X = e.X;
            //        _pointF.Y = e.Y;
            //    }
            //}

            //if(_bShowCoordinatesChange)
            //{
            //    this.PlotSurfaceRefresh();
            //    _bShowCoordinatesChange = false;
            //}
		}

		private void RightMenu__Popup(object sender, System.EventArgs e)
		{
			_bShowCoordinatesChange = true;
		}

		#endregion
	}
}
