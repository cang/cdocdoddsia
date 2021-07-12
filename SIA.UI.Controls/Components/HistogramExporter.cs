using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using SIA.Common.Analysis;
using SIA.UI.Controls.UserControls;

namespace SIA.UI.Controls.Components
{
	public enum HistogramType
	{
		Linear = 0,
		SemiLogX,
	}

	public enum BinOptions
	{
		BinWidth = 0,
		NumBins
	}

	public enum DisplayType
	{
		Automatic = 0,
		Custom = 1
	}

	public class HistogramCustomOptions 
	{
		public int OptionType;
		public double MinView;
		public double MaxView;
		public BinOptions BinOptions;
		public double BinWidth;
		public int NumBins;
	}

	/// <summary>
	/// Summary description for HistogramExporter.
	/// </summary>
	public class HistogramExporter : IDisposable
	{
		#region Fields

		private double _orgBinMinValue = double.MaxValue;
		private double _orgBinMaxValue = double.MinValue;
		private double _minValue = double.MaxValue;
		private double _maxValue = double.MinValue;
		private int _numObjects = 0;

		#endregion

		#region Properties
		public double OriginalBinMinValue
		{
			get {return this._orgBinMinValue;}
		}

		public double OriginalBinMaxValue
		{
			get {return this._orgBinMaxValue;}
		}

		public double Minimum
		{
			get {return this._minValue;}
		}

		public double Maximum
		{
			get {return this._maxValue;}
		}

		public int NumStatisticalObjects
		{
			get {return this._numObjects;}
		}

		#endregion

		#region Constructor and destructor
		public HistogramExporter()
		{
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{

		}

		#endregion

		#region Methods

		public void UpdateHistogramViewer(HistogramViewer histViewer, ArrayList objects, int dataSourceType, 
			HistogramType histType, DisplayType dispType,  HistogramCustomOptions options)
		{
			double[] data = null;
			double[] values = null;
			double[] categories = null;
			int nBins = 65535;

			if (objects == null || objects.Count == 0)
			{
				categories = new double[nBins];
				values = new double[nBins];
				this.RefreshChart(histViewer, categories, values, dataSourceType, histType);
				return;
			}
			
			// extracts histogram data from object list
			data = this.GetData(objects, dataSourceType);

			// estimates data range
			this.EstimateDataRange(data);

			if (dispType == DisplayType.Automatic)
			{
				if (histType == HistogramType.Linear)				
					values = this.NormalizeLogarithIntegratedIntensityHistogram(data, _orgBinMinValue, _orgBinMaxValue, nBins, ref categories);
				else if (histType == HistogramType.SemiLogX)
					 values = this.NormalizeHistogram(data, _orgBinMinValue, _orgBinMaxValue, nBins, ref categories);
			}
			else if (dispType == DisplayType.Custom)
			{
				int nBinsMax = 65536;
				double min_range = options.MinView;
				double max_range = options.MaxView;
				int numBins = options.NumBins;
				double binWidth = options.BinWidth;

				if (histType == HistogramType.Linear)	
				{
					if (options.BinOptions == BinOptions.BinWidth)
					{
						int nBinsDraft = (int)Math.Ceiling((max_range-min_range)/binWidth) + 1;
						if (nBinsDraft > nBinsMax)
							throw new ArgumentOutOfRangeException(string.Format("The calculated number of bins ({0}) is too big.", nBinsDraft));
							
						values = this.NormalizeHistogram(data, min_range, max_range, binWidth, ref categories);
					}
					else if (options.BinOptions == BinOptions.NumBins)
					{
						if (numBins > nBinsMax)
							throw new ArgumentOutOfRangeException(string.Format("Number of bins should be less than or equal {0}.", nBinsMax));

						values = this.NormalizeHistogram(data, min_range, max_range, numBins, ref categories);
					}
				}
				else if (histType == HistogramType.SemiLogX) 
				{
					if (numBins > nBinsMax)
						throw new ArgumentException(string.Format("Number of bins should be less than or equal {0}.", nBinsMax));
					values = this.NormalizeLogarithIntegratedIntensityHistogram(data, min_range, max_range, numBins, ref categories);
				}
			}

			this.RefreshChart(histViewer, categories, values, dataSourceType, histType);
		}

		public Bitmap ExportHistogram(ArrayList objects, int dataSourceType, 
			HistogramType histType, DisplayType dispType,  HistogramCustomOptions options, int width, int height)
		{
			using (HistogramViewer viewer = new HistogramViewer())
			{
				this.UpdateHistogramViewer(viewer, objects, dataSourceType, histType, dispType, options);
				return viewer.ExportChartBitmap(width, height);
			}
		}

		#endregion

		#region Internal Methods

		private void EstimateDataRange(double[] data)
		{
			int count = data.Length;

			_orgBinMinValue = double.MaxValue;
			_orgBinMaxValue = double.MinValue;

			double binValue = 0;
			
			for (int i=0; i<count; i++)
			{
				binValue = data[i];
				if (binValue < _orgBinMinValue)
					_orgBinMinValue = binValue;
				if (binValue > _orgBinMaxValue)
					_orgBinMaxValue = binValue;
			}
		}

		private double[] NormalizeHistogram(double[] data, double min, double max, double binWidth, ref double[] bins)
		{
			double[] hist = null;

			try
			{
				bins = GetLinearBins(min, max, binWidth);
				if (bins == null || bins.Length == 0)
					return null;

				int nBins = bins.Length;

				hist = new double[nBins];
				if (hist == null || hist.Length == 0)
					throw new System.OutOfMemoryException("Not enough memory.");

				_minValue = min;
				_maxValue = max;
				_numObjects = 0;

				double binValue = 0;

				int n = data.Length;
				for (int i=0; i<n; i++)
				{
					binValue = data[i];

					if (binValue < min || binValue > max)
						continue;

					if (_minValue > binValue)
						_minValue = binValue;
					if (_maxValue < binValue)
						_maxValue = binValue;
					_numObjects++;

					hist[(int)((binValue-min)/binWidth)]++;
				}
			}
			catch
			{
				hist = null;
				return hist;
			}
			finally
			{
			}
			return hist;
		}

		private double[] NormalizeHistogram(double[] data, double min, double max, int nBins, ref double[] bins)
		{
			if (nBins < 1)
				return null;

			_minValue = min;
			_maxValue = max;
			_numObjects = 0;

			double binValue = 0;

			double[] hist = null;
			try
			{
				hist = new double[nBins];
				if (hist == null || hist.Length == 0)
					throw new System.OutOfMemoryException("Not enough memory.");

				double binWidth = 0;
				bins = GetLinearBins(min, max, nBins, ref binWidth);
				if (bins == null || bins.Length == 0)
					return null;

				int n = data.Length;
				for (int i=0; i<n; i++)
				{
					binValue = data[i];
					if (binValue < min || binValue > max)
						continue;

					if (_minValue > binValue)
						_minValue = binValue;
					if (_maxValue < binValue)
						_maxValue = binValue;
					_numObjects++;

					hist[(int)((binValue-min)/binWidth)]++;
				}
			}
			catch
			{
				hist = null;
				return hist;
			}
			finally
			{
			}
			return hist;
		}

		private double[] NormalizeLogarithIntegratedIntensityHistogram(double[] data, double min, double max, int nBins, ref double[] bins)
		{
			
			double[] hist = new double[nBins];
			try
			{				
				double lmin = 0;
				if (min > 0)
					lmin = Math.Log10(min);					
				double lmax = 0;
				if (max > 0)
					lmax = Math.Log10(max);					
				double binwidth = 0;
				bins = GetLinearBins(lmin, lmax, nBins, ref binwidth);
				if (bins == null || bins.Length == 0)
					return null;

				_minValue = min;
				_maxValue = max;
				_numObjects = 0;

				int nObjects = data.Length;
				for (int i=0; i<nObjects; i++)
				{
					double binValue = data[i];
					if (binValue < min || binValue > max)
						continue;
					if (_minValue > binValue)
						_minValue = binValue;
					if (_maxValue < binValue)
						_maxValue = binValue;
					_numObjects++;

					double log_of_integratedIntensity = 0;
					if (binValue > 0)
						log_of_integratedIntensity = Math.Log10(binValue);
					int index = (int)((log_of_integratedIntensity - lmin)/binwidth);
					hist[index]++;
				}
			}
			catch
			{
				hist = null;
				return hist;
			}
			finally
			{
			}
			return hist;
		}


		private double[] GetLinearBins(double min, double max, int nBins, ref double binwidth)
		{
			if (max < min)
				return null;

			if (nBins < 1)
				return null;

			binwidth = (max-min+1)/nBins;
			
			double[] bins = new double[nBins];
			bins[0] = min;
			for (int i=1; i<nBins; i++)
				bins[i] = bins[i-1] + binwidth;
			
			return bins;
		}

		private double[] GetLinearBins(double min, double max, double binWidth)
		{
			if (max < min || binWidth <= 0)
				return null;
			int nBins = (int)Math.Ceiling((max-min)/binWidth) + 1;
			ArrayList bins = new ArrayList(nBins);			
			double bin = min;
			while(bin <= max)
			{
				bins.Add(bin);
				bin += binWidth;
			}

			return (double[])bins.ToArray(typeof(double));
		}

		private void RefreshChart(HistogramViewer histViewer, double[] categories, double[] values, int dataSourceType, HistogramType histType)
		{
			try			
			{
				if (categories == null || values == null)
				{
					// empty chart with 1 element ==> should fix this problem
					categories = new double[1];
					values = new double[1];
				}

				// initialize plot surface
				histViewer.UpdatePlotSurface();
				// create new histogram plot
				histViewer.AddHistogramPlot(categories, values, true, "Histogram", Color.Blue, null, 1.0f);
				// add label and title				
				string xAxisTitle = "XAxis";
				string yAxisTitle = "YAxis";
				string chartTitle = "Chart Title";

				this.GetChartText(ref chartTitle, ref xAxisTitle, ref yAxisTitle, dataSourceType, histType);
				
				histViewer.AddLabelsAndTitle(xAxisTitle, yAxisTitle, chartTitle);
				// enable displaying coordinate
				histViewer.ShowCoordinates = true;
				// allow context menu
				histViewer.RightMenu = true;				
				
				// add vertical interaction line
				// histViewer.AddVertialInteraction(Color.DarkGreen);				
				// add horizontal interaction line
				// histViewer.AddHorizontalInteraction(Color.DarkGreen);							

				// refresh plot surface
				histViewer.PlotSurfaceRefresh();
			}
			catch
			{
				// empty chart viewer data
				histViewer.UpdatePlotSurface();

				// re-throw exception
				throw;
			}
			finally
			{
			}
		}

		private void RefreshChart(HistogramViewer histViewer, double[] categories, double[] yVals, bool logarithmicHistogram)
		{			
			try			
			{
				if (categories == null || yVals == null)
				{
					// empty chart with 1 element ==> should fix this problem
					categories = new double[1];
					yVals = new double[1];
				}

				// initialize plot surface
				histViewer.UpdatePlotSurface();
				// create new histogram plot
				histViewer.AddHistogramPlot(categories, yVals, true, "Histogram", Color.Blue, null, 1.0f);
				// add label and title				
				string xAxisTitle = "XAxis";
				string yAxisTitle = "YAxis";
				string chartTitle = "Chart Title";
				
				histViewer.AddLabelsAndTitle(xAxisTitle, yAxisTitle, chartTitle);
				// enable displaying coordinate
				histViewer.ShowCoordinates = true;
				// allow context menu
				histViewer.RightMenu = true;				
				// add vertical interaction line
				histViewer.AddVertialInteraction(Color.DarkGreen);				
				// add horizontal interaction line
				histViewer.AddHorizontalInteraction(Color.DarkGreen);							
				// refresh plot surface
				histViewer.PlotSurfaceRefresh();
			}
			catch
			{
				// empty chart viewer data
				histViewer.UpdatePlotSurface();

				// re-throw exception
				throw;
			}
			finally
			{
			}
		}

		private double[] GetData(ArrayList objects, int dataSourceType)
		{
			if (objects == null)
				throw new ArgumentNullException("objects");
			
			int numObjects = objects.Count;
			double[] data = new double[numObjects];

			for (int i=0; i<numObjects; i++)		
			{
				DetectedObject detObj = objects[i] as DetectedObject;
                if (dataSourceType == 0)
                    data[i] = (double)detObj.TotalIntensity;
                else if (dataSourceType == 1)
                    data[i] = (double)detObj.NumPixels;
                else if (dataSourceType == 2)
                    data[i] = (double)detObj.Area;
                else if (dataSourceType == 3)
                    data[i] = (double)detObj.Perimeter;
                else if (dataSourceType == 4)
                    data[i] = (double)detObj.RectBound.Width;
                else if (dataSourceType == 5)
                    data[i] = (double)detObj.RectBound.Height;
                else
                    throw new ArgumentOutOfRangeException("dataSourceType", dataSourceType, "Invalid data source type");
			}

			return data;
		}

		private void GetChartText(ref string title, ref string xAxis, ref string yAxis, int dataSourceType, HistogramType histType)
		{
			if (dataSourceType == 0)
			{
				title = histType == HistogramType.Linear ? "Integrated Intensity Histogram" : "Logarithmic Integrated Intensity Histogram";
				xAxis = histType == HistogramType.Linear ? "Integrated Intensity" : "Log(Integrated Intensity)";
				yAxis = "Frequency";
			}
			else if (dataSourceType == 1)
			{
				title = histType == HistogramType.Linear ? "Number of Pixels Histogram" : "Logarithmic Number of Pixels Histogram";
				xAxis = histType == HistogramType.Linear ? "Number of Pixels" : "Log(Number of Pixels)";
				yAxis = "Frequency";
			}
			else if (dataSourceType == 2)
			{
				title = histType == HistogramType.Linear ? "Area Histogram" : "Logarithmic Area Histogram";
				xAxis = histType == HistogramType.Linear ? "Area" : "Log(Area)";
				yAxis = "Frequency";
			}
			else if (dataSourceType == 3)
			{
				title = histType == HistogramType.Linear ? "Perimeter Histogram" : "Logarithmic Perimeter Histogram";
				xAxis = histType == HistogramType.Linear ? "Perimeter" : "Log(Perimeter)";
				yAxis = "Frequency";
			}
            else if (dataSourceType == 4)
            {
                title = histType == HistogramType.Linear ? "Boundary Width Histogram" : "Logarithmic Boundary Width Histogram";
                xAxis = histType == HistogramType.Linear ? "Boundary Width" : "Log(Boundary Width)";
                yAxis = "Frequency";
            }
            else if (dataSourceType == 5)
            {
                title = histType == HistogramType.Linear ? "Boundary Height Histogram" : "Logarithmic Boundary Height Histogram";
                xAxis = histType == HistogramType.Linear ? "Boundary Height" : "Log(Boundary Height)";
                yAxis = "Frequency";
            }
            else
                throw new ArgumentOutOfRangeException("dataSourceType", dataSourceType, "Invalid data source type");
		}

		#endregion
	}
}
