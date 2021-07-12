using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using SIA.IPEngine;
using SIA.SystemLayer;

using TYPE = System.UInt16; 

namespace SIA.UI.Controls.Dialogs
{
	/// <summary>
	/// Name : DlgIntensityHistogram
	/// Description : User interface for Clear Wafer Boundary 
	/// Thread Support : None
	/// Persistence Data : True
	/// </summary>
	public class DlgIntensityHistogram : DialogBase
	{
		#region constants
		
		#endregion

		#region Windows Form member attributes
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.GroupBox grpThreshold;
		private System.Windows.Forms.Label lblMin;
		private System.Windows.Forms.Label lblMax;
		private System.Windows.Forms.PictureBox picHistogram;
		private System.Windows.Forms.NumericUpDown nudMinThreshold;
		private System.Windows.Forms.NumericUpDown nudMaxThreshold;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cbHistType;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion
		
		#region member attributes

		/// <summary>  
		/// private constant value
		/// </summary>
		private double		MAX_PIXEL_INTENSITY = 32768;	
		private double		MIN_PIXEL_INTENSITY = 0;
		
		private double		m_iMinThreshold = 0;
		private double		m_iMaxThreshold = 0;

		private Color		chartForeColor = Color.Black;
		private Color		chartBackgroundColor = Color.Gray;
		private Font		chartFont = new Font("Tahoma", 7.0f);
		// Margin
		private int			chartLeftMargin	  = 20;
		private int			chartRightMargin  = 10;
		private int			chartTopMargin	  = 15;
		private int			chartBottomMargin = 15;
		
		
		private SIA.SystemLayer.CommonImage m_image = null;	
		private SIA.IPEngine.kHistogram m_histogram = null;
		private Bitmap		m_bmpChart;
		private double[]	m_arHistogram = null;
		
		#endregion

		#region public properties

		

		#endregion

		#region constructor and destructor
		
		public DlgIntensityHistogram(CommonImage image)
		{
			InitializeComponent();
			
			if (image==null)
				throw new System.ArgumentNullException("invalid parameter");
			
			m_image =image;
			MAX_PIXEL_INTENSITY = m_image.RasterImage.MAXGRAYVALUE;
			MIN_PIXEL_INTENSITY = m_image.RasterImage.MINGRAYVALUE;
			m_iMaxThreshold = m_image.RasterImage.MaxCurrentView;
			m_iMinThreshold = m_image.RasterImage.MinCurrentView;
			m_histogram = m_image.Histogram;
			m_arHistogram = m_histogram.Histogram; 
		}
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
					components.Dispose();
				if (m_histogram!=null)
					m_histogram.Dispose();
			}
			base.Dispose( disposing );
		}

		
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgIntensityHistogram));
			this.grpThreshold = new System.Windows.Forms.GroupBox();
			this.cbHistType = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.nudMinThreshold = new System.Windows.Forms.NumericUpDown();
			this.lblMin = new System.Windows.Forms.Label();
			this.lblMax = new System.Windows.Forms.Label();
			this.nudMaxThreshold = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.btnClose = new System.Windows.Forms.Button();
			this.picHistogram = new System.Windows.Forms.PictureBox();
			this.grpThreshold.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudMinThreshold)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMaxThreshold)).BeginInit();
			this.SuspendLayout();
			// 
			// grpThreshold
			// 
			this.grpThreshold.Controls.Add(this.cbHistType);
			this.grpThreshold.Controls.Add(this.label1);
			this.grpThreshold.Controls.Add(this.nudMinThreshold);
			this.grpThreshold.Controls.Add(this.lblMin);
			this.grpThreshold.Controls.Add(this.lblMax);
			this.grpThreshold.Controls.Add(this.nudMaxThreshold);
			this.grpThreshold.Controls.Add(this.label2);
			this.grpThreshold.Location = new System.Drawing.Point(4, 200);
			this.grpThreshold.Name = "grpThreshold";
			this.grpThreshold.Size = new System.Drawing.Size(424, 72);
			this.grpThreshold.TabIndex = 0;
			this.grpThreshold.TabStop = false;
			this.grpThreshold.Text = "Options";
			// 
			// cbHistType
			// 
			this.cbHistType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbHistType.Items.AddRange(new object[] {
															"Normal",
															"Log"});
			this.cbHistType.Location = new System.Drawing.Point(119, 16);
			this.cbHistType.Name = "cbHistType";
			this.cbHistType.Size = new System.Drawing.Size(115, 21);
			this.cbHistType.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 21);
			this.label1.TabIndex = 0;
			this.label1.Text = "Histogram Type:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// nudMinThreshold
			// 
			this.nudMinThreshold.Location = new System.Drawing.Point(140, 44);
			this.nudMinThreshold.Name = "nudMinThreshold";
			this.nudMinThreshold.Size = new System.Drawing.Size(56, 20);
			this.nudMinThreshold.TabIndex = 4;
			this.nudMinThreshold.ValueChanged += new System.EventHandler(this.OnThresholdChanged);
			// 
			// lblMin
			// 
			this.lblMin.Location = new System.Drawing.Point(116, 44);
			this.lblMin.Name = "lblMin";
			this.lblMin.Size = new System.Drawing.Size(26, 20);
			this.lblMin.TabIndex = 3;
			this.lblMin.Text = "Min:";
			this.lblMin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblMax
			// 
			this.lblMax.Location = new System.Drawing.Point(204, 44);
			this.lblMax.Name = "lblMax";
			this.lblMax.Size = new System.Drawing.Size(29, 20);
			this.lblMax.TabIndex = 5;
			this.lblMax.Text = "Max:";
			this.lblMax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// nudMaxThreshold
			// 
			this.nudMaxThreshold.Location = new System.Drawing.Point(236, 44);
			this.nudMaxThreshold.Name = "nudMaxThreshold";
			this.nudMaxThreshold.Size = new System.Drawing.Size(56, 20);
			this.nudMaxThreshold.TabIndex = 6;
			this.nudMaxThreshold.ValueChanged += new System.EventHandler(this.OnThresholdChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(12, 44);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 20);
			this.label2.TabIndex = 2;
			this.label2.Text = "Intensity Range:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// btnClose
			// 
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnClose.Location = new System.Drawing.Point(176, 276);
			this.btnClose.Name = "btnClose";
			this.btnClose.TabIndex = 1;
			this.btnClose.Text = "&Close";
			// 
			// picHistogram
			// 
			this.picHistogram.BackColor = System.Drawing.Color.Gray;
			this.picHistogram.Location = new System.Drawing.Point(4, 4);
			this.picHistogram.Name = "picHistogram";
			this.picHistogram.Size = new System.Drawing.Size(424, 192);
			this.picHistogram.TabIndex = 4;
			this.picHistogram.TabStop = false;
			// 
			// DlgIntensityHistogram
			// 
			this.AcceptButton = this.btnClose;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(432, 304);
			this.Controls.Add(this.picHistogram);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.grpThreshold);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DlgIntensityHistogram";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Histogram";
			this.Load += new System.EventHandler(this.OnLoad);
			this.grpThreshold.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.nudMinThreshold)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMaxThreshold)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region override routines
		
		private void OnLoad(object sender, System.EventArgs e)
		{	
			this.cbHistType.SelectedIndex = 0;
			this.cbHistType.SelectedIndexChanged += new System.EventHandler(this.OnHistType_Changed);
			
			/* initialize Chart settings */
			Graphics graph = picHistogram.CreateGraphics();
			long maxX = (long)MAX_PIXEL_INTENSITY;
			SizeF sizeText = graph.MeasureString(MAX_PIXEL_INTENSITY.ToString(), chartFont);
			chartLeftMargin  = (int)(1.5f*sizeText.Width);
			chartRightMargin = (int)(1.5f*sizeText.Width);
			chartTopMargin = (int)(1.5f*sizeText.Height);
			chartBottomMargin = (int)(1.5f*sizeText.Height);
			graph.Dispose();
			
			/* initialize GUI Settings */
			nudMinThreshold.Minimum = (Decimal)MIN_PIXEL_INTENSITY;
			nudMinThreshold.Maximum = (Decimal)MAX_PIXEL_INTENSITY;

			nudMaxThreshold.Minimum = (Decimal)MIN_PIXEL_INTENSITY;
			nudMaxThreshold.Maximum = (Decimal)MAX_PIXEL_INTENSITY;

			nudMinThreshold.Value = (Decimal)m_iMinThreshold;
			nudMaxThreshold.Value = (Decimal)m_iMaxThreshold;

			if (true==RenderHistogramChart((int)m_iMinThreshold, (int)m_iMaxThreshold))
			{
				picHistogram.Image = m_bmpChart;
				picHistogram.SizeMode = PictureBoxSizeMode.StretchImage;
			}
		}

		#endregion

		#region virtual routines


		#endregion

		#region event handlers
		private void OnThresholdChanged(object sender, System.EventArgs e)
		{
			if (nudMaxThreshold.Value <= (nudMinThreshold.Value+10))
			{
				nudMaxThreshold.Value = (int)m_iMaxThreshold;
				nudMinThreshold.Value = (int)m_iMinThreshold;
			}

			bool bRedrawChart = false;
			bRedrawChart = (m_iMinThreshold!=(int)nudMinThreshold.Value) || (m_iMaxThreshold!=(int)nudMaxThreshold.Value);
			if (bRedrawChart && true==RenderHistogramChart((int)nudMinThreshold.Value, (int)nudMaxThreshold.Value)) 
			{
				m_iMinThreshold = (int)nudMinThreshold.Value;
				m_iMaxThreshold = (int)nudMaxThreshold.Value;
				picHistogram.Image = m_bmpChart;
				picHistogram.SizeMode = PictureBoxSizeMode.StretchImage;
			}
		}

		private void OnHistType_Changed(object sender, System.EventArgs e)
		{
			if (RenderHistogramChart((int)m_iMinThreshold, (int)m_iMaxThreshold))
			{
				m_iMinThreshold = (int)nudMinThreshold.Value;
				m_iMaxThreshold = (int)nudMaxThreshold.Value;
				picHistogram.Image = m_bmpChart;
				picHistogram.SizeMode = PictureBoxSizeMode.StretchImage;
			}
		}
		#endregion

		#region internal routines

		private bool RenderHistogramChart(int iMinThreshold, int iMaxThreshold)
		{
			try 
			{
				/* initializes drawing tools */
				if (m_bmpChart!=null) m_bmpChart.Dispose();
				m_bmpChart = new System.Drawing.Bitmap(picHistogram.Width, picHistogram.Height);
				
				System.Drawing.Graphics		grpChart = System.Drawing.Graphics.FromImage(m_bmpChart);
				System.Drawing.Pen			chartPen = new System.Drawing.Pen(chartForeColor, 1.0f);
				System.Drawing.SolidBrush	chartBrush = new System.Drawing.SolidBrush(chartForeColor);
				System.Drawing.Brush		rulerBrush = null;
				grpChart.Clear(chartBackgroundColor);

				/* computes parameter for rendering */
				bool bLogChart = (cbHistType.SelectedIndex!=0);
				double iMinCount = (double)UInt32.MaxValue;
				double iMaxCount = (double)UInt32.MinValue;
				for (int i=iMinThreshold; i<=iMaxThreshold; i++)
				{
					iMinCount = Math.Min(iMinCount, m_arHistogram[i]);
					iMaxCount = Math.Max(iMaxCount, m_arHistogram[i]);
				}
				iMinCount = (double)(bLogChart ? (iMinCount!=0 ? Math.Log(iMinCount, Math.E) : 0) : iMinCount);
				iMaxCount = (double)(bLogChart ? Math.Log(iMaxCount, Math.E) : iMaxCount);

				double iCount = 0;
				int iThreshold = 0;
				float fYScaleFactor = 1.0f;
				float fXScaleFactor = 1.0f;
				SizeF unitSize = new SizeF(1.0F, 1.0F);
				SizeF textSize, spaceSize;
				PointF textPos = new PointF(.0f, .0f);

				Rectangle rcContent = new Rectangle(0, 0, picHistogram.Width, picHistogram.Height);
				float left    = rcContent.Left   + chartLeftMargin;
				float top	  = rcContent.Top	 + chartTopMargin;
				float right   = rcContent.Right	 - chartRightMargin;
				float bottom  = rcContent.Bottom - chartBottomMargin;
				float width	  = rcContent.Width  - (chartLeftMargin+chartRightMargin);
				float height  = rcContent.Height - (chartTopMargin+chartBottomMargin);

				/* render chart's border */
				grpChart.DrawRectangle(chartPen, rcContent.Left, rcContent.Top, rcContent.Width-1, rcContent.Height-1);
				/* render chart content */
				fYScaleFactor = (float)height/(float)(iMaxCount-iMinCount);			
				fXScaleFactor = (float)width/(float)(iMaxThreshold-iMinThreshold);
				spaceSize = new SizeF(unitSize.Width * fXScaleFactor, unitSize.Height * fYScaleFactor);

				//				grpChart.SetClip(new RectangleF(left, rcContent.Top, width, rcContent.Height), System.Drawing.Drawing2D.CombineMode.Replace);
				
				for (iThreshold=iMinThreshold; iThreshold<=iMaxThreshold; iThreshold++)
				{
					if (iThreshold >=0 && iThreshold<m_arHistogram.Length)
					{
						iCount = bLogChart ? (double)(Math.Log(m_arHistogram[iThreshold], Math.E)) : (double)(m_arHistogram[iThreshold]);
						if ((int)(iCount*fYScaleFactor)>0) 
						{
							ArrayList arPoints = new ArrayList();
							arPoints.Add(new PointF((float)(left+(iThreshold-iMinThreshold)*fXScaleFactor), (float)bottom));
							arPoints.Add(new PointF((float)(left+(iThreshold-iMinThreshold)*fXScaleFactor), (float)(bottom-(iCount*fYScaleFactor))));
							arPoints.Add(new PointF((float)(left+(iThreshold-iMinThreshold)*fXScaleFactor), (float)bottom));
						
							PointF[] drawPoints = (PointF[])arPoints.ToArray(typeof(PointF));
							grpChart.DrawLines(chartPen, drawPoints);						
						}
					}
				}

				//				PointF[] drawPoints = (PointF[])arPoints.ToArray(typeof(PointF));
				//				grpChart.DrawLines(chartPen, drawPoints);

				//				grpChart.ResetClip();

				/* render chart's ruler */
				Pen rulerPen = chartPen;
				grpChart.DrawLine(rulerPen,  left-1, top, left-1, bottom+1);			// vertical line
				grpChart.DrawLine(rulerPen,  left-1, bottom+1, right+1, bottom+1);		// horizontal line

				/* compute ruler's unit */
				int YRange = (int)(iMaxCount - iMinCount);
				int XRange = iMaxThreshold - iMinThreshold;
				unitSize = grpChart.MeasureString(((int)MAX_PIXEL_INTENSITY).ToString(), chartFont);
				float unitHeight = (float)(5.0f*unitSize.Height);
				float unitWidth  = (float)(5.0f*unitSize.Width);
				int HorzStep = (int)(YRange / unitHeight);
				int VertStep = (int)(XRange / unitWidth);
				/* render ruler's text */
				rulerBrush = chartBrush;
				
				/* horizontal ruler */
				textSize = grpChart.MeasureString(iMinThreshold.ToString(), chartFont);
				grpChart.DrawString(iMinThreshold.ToString(), chartFont, rulerBrush,
					left-1.0f-textSize.Width/2.0f, bottom+2.0f, StringFormat.GenericDefault);
				textSize = grpChart.MeasureString(iMaxThreshold.ToString(), chartFont);
				grpChart.DrawString(iMaxThreshold.ToString(), chartFont, rulerBrush,
					right-textSize.Width/2.0f, bottom+2.0f, StringFormat.GenericDefault);

				/* vertical ruler */
				textSize = grpChart.MeasureString(((int)iMaxCount).ToString(), chartFont);
				float XPos = left - 3.0f - textSize.Width;
				float YPos = top - textSize.Height/2.0f;
				grpChart.DrawLine(chartPen, left-3.0f, top, left-1.0f, top);
				grpChart.DrawString(((int)iMaxCount).ToString(), chartFont, 
					chartBrush, XPos, YPos, StringFormat.GenericDefault);

				textSize = grpChart.MeasureString(((int)iMinCount).ToString(), chartFont);
				XPos = left - 3.0f - textSize.Width;
				YPos = bottom - textSize.Height/2.0f;
				grpChart.DrawLine(chartPen, left-3.0f, bottom, left-1.0f, bottom);
				grpChart.DrawString(((int)iMinCount).ToString(), chartFont,
					chartBrush, XPos, YPos, StringFormat.GenericDefault);
				
				rulerPen.Dispose();
				rulerBrush.Dispose();
				return true;
			}
			catch(Exception exp)
			{
				Trace.WriteLine(exp.ToString());
				return false;
			}
		}

		#endregion
	}
}
