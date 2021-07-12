using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;

using SIA.SystemLayer;
using SIA.UI.Components;

using TYPE = System.Int16;
using SIA.IPEngine;

using SIA.Common.IPLFacade;

namespace SIA.UI.Controls.UserControls
{
	/// <summary>
	/// Summary description for kGraphHistogram.
	/// </summary>
	public unsafe class kGraphHistogram : System.Windows.Forms.UserControl
	{
		#region member Windows Form attributes
		
		protected System.Windows.Forms.PictureBox pictureBox;
		private SIA.UI.Controls.UserControls.kSlider slider;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		
		#endregion

		#region member attributes

		protected SIA.SystemLayer.CommonImage		_image = null;
        protected System.Drawing.Bitmap _bitmap;
        protected System.Double[] _histogram = null;
        protected DataRange _dataRange = DataRange.Empty;
        protected System.Int32 _minValue = 0;
        protected System.Int32 _maxValue = 0;
        protected System.Double _maxBins = 0;
        protected System.Double _minBins = 0;

        protected int _selRangeMax = 0;
        protected int _selRangeMin = 0;
        protected int _ignoreUpdate = 0;
		
		#endregion

		#region constructor and destructor
		
		public kGraphHistogram()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (_bitmap != null)
				{
					_bitmap.Dispose();
					_bitmap = null;
				}

				if (components != null)
					components.Dispose();
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
			this.pictureBox = new System.Windows.Forms.PictureBox();
			this.slider = new SIA.UI.Controls.UserControls.kSlider();
			this.SuspendLayout();
			// 
			// pictureBox
			// 
			this.pictureBox.BackColor = System.Drawing.Color.Black;
			this.pictureBox.Location = new System.Drawing.Point(7, 0);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(270, 130);
			this.pictureBox.TabIndex = 0;
			this.pictureBox.TabStop = false;
			// 
			// slider
			// 
			this.slider.Location = new System.Drawing.Point(0, 132);
			this.slider.Lock = false;
			this.slider.MaxValue = 100;
			this.slider.MinValue = 0;
			this.slider.Name = "slider";
			this.slider.ShowTrack = false;
			this.slider.Size = new System.Drawing.Size(284, 14);
			this.slider.TabIndex = 1;
			this.slider.LeftValueChanged += new SIA.UI.Controls.UserControls.kSlider.ValueChangedEventHandler(this.RaiseLeftValueChanged);
			this.slider.RightValueChanged += new SIA.UI.Controls.UserControls.kSlider.ValueChangedEventHandler(this.RaiseRightValueChanged);
			this.slider.LeftValueChanging += new SIA.UI.Controls.UserControls.kSlider.ValueChangingEventHandler(this.RaiseLeftValueChanging);
			this.slider.RightValueChanging += new SIA.UI.Controls.UserControls.kSlider.ValueChangingEventHandler(this.RaiseRightValueChanging);
			// 
			// kGraphHistogram
			// 
			this.Controls.Add(this.slider);
			this.Controls.Add(this.pictureBox);
			this.Name = "kGraphHistogram";
			this.Size = new System.Drawing.Size(284, 160);
			this.ResumeLayout(false);

		}
		#endregion

		#region public event

		public delegate void ValueChangingEventHandler(Object sender, int val);
		public delegate void ValueChangedEventHandler(Object sender, int val);

		[Browsable(true), Category("Action")]
		public event ValueChangingEventHandler LeftValueChanging;

		[Browsable(true), Category("Action")]
		public event ValueChangedEventHandler LeftValueChanged;

		[Browsable(true), Category("Action")]
		public event ValueChangingEventHandler RightValueChanging;

		[Browsable(true), Category("Action")]
		public event ValueChangedEventHandler RightValueChanged;

		#endregion

		#region public properties

		public SIA.SystemLayer.CommonImage Image
		{
			get {return _image;}
			set
			{
                _image = value;
                
                if (_image != null)
                {
                    Minimum = (int)_image.RasterImage.MINGRAYVALUE;
                    Maximum = (int)_image.RasterImage.MAXGRAYVALUE;
                    SelectRangeMin = (int)_image.RasterImage.MinCurrentView;
                    SelectRangeMax = (int)_image.RasterImage.MaxCurrentView;
                }

                ReloadHistogram();
			}
		}

		public int Minimum
		{
			get {return _minValue;}
			set 
			{
				_minValue = value;
				UpdateHistogramImage();
			}
		}

		public int Maximum
		{
			get {return _maxValue;}
			set 
			{
				_maxValue = value;
				UpdateHistogramImage();
			}
		}

		public int SelectRangeMin
		{
			get 
			{
				return _selRangeMin; 
			}
			set 
			{
				_selRangeMin = value;
				OnSelectRangeMinChanged();
			}
		}

		protected virtual void OnSelectRangeMinChanged()
		{
			slider.SetLeftThumbPos(_selRangeMin);
		}

		public int SelectRangeMax
		{
			get 
			{
				return _selRangeMax;
			}
			set 
			{
				_selRangeMax = value;
				OnSelectRangeMaxChanged();
			}
		}

		protected virtual void OnSelectRangeMaxChanged()
		{
			slider.SetRightThumbPos(_selRangeMax);
		}

		public bool Lock
		{
			get {return slider.Lock;}
			set {slider.Lock = value;}
		}

		public DataRange DataRange
		{
			get {return _dataRange;}
		}

		#endregion

		#region override routines

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			// initialize histogram
			_minValue = 0;
			_maxValue = 0;
			if (_histogram != null)
			{
				_minValue = 0;
				_maxValue = _histogram.Length-1;
			}

			ReloadHistogram();
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			try
			{
				this.SuspendLayout();

				pictureBox.Location = new Point(slider.ThumbSize.Width, 0);
				pictureBox.Width    = this.Width - 2*slider.ThumbSize.Width;
				pictureBox.Height   = this.Height - slider.ThumbSize.Height;
			
				slider.Location = new Point(0, pictureBox.Bottom);
				slider.Width = this.Width;			
			}
			finally
			{
				this.ResumeLayout();
			}

			base.OnSizeChanged (e);
			
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged (e);

			if (this.Visible == true)
			{
				slider.SetLeftThumbPos(_selRangeMin);
				slider.SetRightThumbPos(_selRangeMax);
			}
		}


		#endregion

		#region public methods

		public void BeginUpdate()
		{
			this._ignoreUpdate++;
		}

		public void EndUpdate()
		{
			this._ignoreUpdate--;

			// force reload histogram
			this.ReloadHistogram();
		}

        #endregion

		#region event handlers

		private void RaiseLeftValueChanged(object sender, int val)
		{
			this._selRangeMin = val;

			if (LeftValueChanged!=null) 
				LeftValueChanged(this, val);
		}
		
		private void RaiseLeftValueChanging(object sender, int val)
		{
			if (LeftValueChanging!=null) 
				LeftValueChanging(this, val);
		}

		private void RaiseRightValueChanged(object sender, int val)
		{
			this._selRangeMax = val;

			if (RightValueChanged!=null) 
				RightValueChanged(this, val);
		}

		private void RaiseRightValueChanging(object sender, int val)
		{
			if (RightValueChanging!=null) 
				RightValueChanging(this, val);
		}
		
		#endregion

		#region internal helpers

        protected virtual void CalculateHistogram(CommonImage image, ref double minCount, ref double maxCount, 
            ref DataRange dataRange, out double[] histogram)
        {
            int length = image.Length;
            ushort* buffer = (ushort*)image.RasterImage.Buffer.ToPointer();

            int start = -1, finish = -1;

            minCount = int.MaxValue;
            maxCount = int.MinValue;

            int numbins = (int)(image.MaxGreyValue - image.MinGreyValue + 1);
            histogram = new double[numbins];
            
            fixed (double* pHist = histogram)
            {
                for (int i = 0; i < length; i++)
                    pHist[buffer[i]] += 1;

                double exp = Math.E;

                for (int i = 0; i < numbins; i++)
                {
                    pHist[i] = Math.Log(pHist[i] + 1.0F, exp);
                    minCount = Math.Min(minCount, pHist[i]);
                    maxCount = Math.Max(maxCount, pHist[i]);
                }

                for (int i = 0; i < numbins; i++)
                {
                    if (start < 0 && pHist[i] > 0)
                    {
                        start = i;
                        break;
                    }
                }

                for (int i = numbins - 1; i >= 0; i--)
                {
                    if (finish < 0 & pHist[i] > 0)
                    {
                        finish = i;
                        break;
                    }
                }
            }

            dataRange = new DataRange(start, finish);
        }

		protected virtual void ReloadHistogram()
		{
			if (this._ignoreUpdate != 0)
				return;

			if (_image != null)
			{
                try
                {
                    // calculate histogram
                    this.CalculateHistogram(_image, ref _minBins, ref _maxBins, ref _dataRange, out _histogram);

                    slider.Enabled = true;
                    slider.MinValue = 0;
                    slider.MaxValue = _maxValue;

                    slider.SetLeftThumbPos(this._selRangeMin);
                    slider.SetRightThumbPos(this._selRangeMax);

                    // refresh the histogram image
                    UpdateHistogramImage();
                }
                catch (Exception exp)
                {
                    Trace.WriteLine(exp);
                }
			}	
			else
			{
                pictureBox.Image = null;
				slider.Enabled = false;				
			}
		}

        protected virtual void UpdateHistogramImage()
		{
			if (this._ignoreUpdate != 0)
				return;

			if (_histogram != null)
			{
				try
				{
                    if (_bitmap == null)
                    {
                        _bitmap = new Bitmap(pictureBox.Width, pictureBox.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                        pictureBox.Image = _bitmap;
                    }
					
                    using (Graphics graph = Graphics.FromImage(_bitmap))
						this.DrawHistogram(graph, _histogram, _maxBins, _minValue, _maxValue-_minValue+1);
                    
                    //  update picture box
                    pictureBox.Image = _bitmap;
				}
				catch (System.Exception exp)
				{
					Trace.WriteLine(exp);
				}				
			}
			else
			{
				pictureBox.Image = null;
			}
		}

		private void DrawHistogram(Graphics graph, double[] histogram, Double maxCount, int start, int count)
		{
			try 
			{	
				if (start < 0 || start >= histogram.Length)
					return;
				if (count > histogram.Length)
					return;
				if (start+count >= histogram.Length)
					count = histogram.Length-start;

				/* render histogram chart */
				float maxWidth	= count;
				float maxHeight = (float)maxCount;		
		
				int drawWidth  = pictureBox.Width;
				int drawHeight = pictureBox.Height;				
				
				float xScale = (float)drawWidth  / (float)(maxWidth);
				float yScale = (float)drawHeight / (float)(maxHeight);
                
                fixed (double* pHist = histogram)
                {
                    int begin = start, end = count + start;
                    float x1 = 0, y1 = 0, x2 = 0, y2 = 0;

                    using (System.Drawing.Drawing2D.Matrix matrix = new System.Drawing.Drawing2D.Matrix())
                    {
                        matrix.Scale(xScale, yScale);
                        graph.Transform = matrix;

                        graph.Clear(Color.Black);

                        for (int i = begin; i < end; i++)
                        {
                            x1 = (i + start); y1 = maxHeight;
                            x2 = (i + start); y2 = (maxHeight - (float)pHist[i]);

                            graph.DrawLine(Pens.White, x1, y1, x2, y2);
                        }
                    }
                }
			}
			catch(Exception exp)
			{
				Trace.WriteLine(exp);
			}
		}


		#endregion
		
	}
}
