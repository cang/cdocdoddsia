using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;

using SIA.Common;
using SIA.Common.Mathematics;

using SIA.UI.Controls;
using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.Utilities;

namespace SIA.UI.Controls.UserControls
{
	public delegate void XValueChange(double x,double y);

	/// <summary>
	/// Summary description for LineChart.
	/// </summary>
	public class LineChart : System.Windows.Forms.UserControl
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#region member attributes
		private		double[]	_values;
		private		PointF[]	adrawpoint;

		private		double		_minx = 0;
		private		double		_maxx = 1000;
		private		double		_miny = 0;
		private		double		_maxy = 1000;

		private		double		_minRealX;
		private		double		_maxRealX;

		private		int			_deltaX = 10;
		private		int			_deltaY = 5;
		private		int			_subDeltaX = 5;
		private		int			_subDeltaY = 5;

		private		string		_captionX = String.Empty;
		private		string		_captionY = String.Empty;

		private		Color		_clientColor = Color.White;
		private		Pen			_penLine = null;
		private		Pen			_penCoordinate = null;
		private		Pen			_penStep = null;
		private		Pen			_penSubStep = null;
		private		Pen			_penSeparator = null;
		private		Pen			_penThresholdLine = null;
		private		Font		_fontCoordinate = null;
		private		Font		_fontCaption = null;
		private		Brush		_brushCommon = null;
		private		Brush		_brushCaption = null;
		
		//private		bool		breflectevent=true;

		private const int	MARGIN_TOP = 10;
		private const int	MARGIN_RIGHT = 20;
		private const int	MARGIN_LEFT = 70;
		private const int	MARGIN_BOTTOM = 50;

		private Point	_axisOrg;
		private Point	_axisX;
		private Point	_axisY;
		private Point	_ptMouseCurrent = Point.Empty;

		private Rectangle	_clientRectangle;
		private Rectangle	_validRectangle;

		private bool _useLogarithmic = false;

		private double delta=0;
		private double _logMin=0;
		private double logmax=0;


		#region threshold chart

		private			PointF[]	_thresholdPoints=null;
		private			bool		_thresholdVisible = false;
		private			int			_order = 6;	// default is 6
		private			ArrayList	_arCoeffs = null;
		private			eTrendlineType  _trendLineType;
		private			int _trendLinePeriod;
		private			int _trendLineOrder;
		private			Expression _expression;
		private			bool m_bMovingAverage;
		private			int mXStart;
		private			SIA.SystemLayer.Trendline mTrendline;
		private			DlgTrendline mTrendlineDlg = null;
		private         TrendLineFormat _trendLineInfo;

		#endregion

		#endregion

		#region properties
		public TrendLineFormat TrendLineInfor
		{
			get {return _trendLineInfo;}
			set {_trendLineInfo = value;}
		}

		[DefaultValue( false)]
		public bool ThresholdVisible
		{
			get{return _thresholdVisible;}
			set
			{
				_thresholdVisible = value;
				OnThresholdVisibleChanged();
			}
		}

		protected void OnThresholdVisibleChanged()
		{
			PrepareData();
			Invalidate();
		}
		
		public eTrendlineType TrendLineType
		{
			get {return _trendLineType;}
			set
			{
				_trendLineType=value;
				OnTrendlineTypeChanged();
			}
		}

		protected void OnTrendlineTypeChanged()
		{
			PrepareData();
			Invalidate();
		}

		public int TrendLineOrder
		{
			get {return _trendLineOrder;}
			set 
			{
				_trendLineOrder = value;
				OnTrendLineOrderChanged();
			}
		}

		protected void OnTrendLineOrderChanged()
		{
		}

		public int TrendLinePeriod
		{
			get {return _trendLinePeriod;}
			set 
			{
				_trendLinePeriod = value;
				OnTrendLinePeriodChanged();
			}
		}

		protected void OnTrendLinePeriodChanged()
		{
		}

		[DefaultValue(false)]
		public ArrayList Coeffs
		{
			get 
			{
				return _arCoeffs;
			}
			set
			{
				_arCoeffs = value;
				OnCoeffsChanged();
			}
		}

		protected void OnCoeffsChanged()
		{
		}

		[DefaultValue(false)]
		public bool Logarithmic
		{
			get{return _useLogarithmic;}
			set
			{
				_useLogarithmic=value;
				OnLogarithmicChanged();
			}
		}

		protected void OnLogarithmicChanged()
		{
		}

//		[DefaultValue(true)]
//		public bool ReflectEvent
//		{
//			get
//			{
//				return breflectevent;
//			}
//			set
//			{
//				breflectevent=value;
//			}
//		}


		[Browsable(false)]
		public double[]	Values
		{
			get{return _values;}
			set
			{
				_values = value;
				OnValuesChanged();
			}
		}

		protected void OnValuesChanged()
		{			
			PrepareData();
			Invalidate();
		}

		[Browsable(false)]
		public int Order
		{
			get{return _order;}
			set
			{
				_order = value;
				OnOrderChanged();
			}
		}

		protected void OnOrderChanged()
		{
			PrepareData();
			Invalidate();
		}

		public string CaptionX 
		{
			get{return _captionX;}
			set
			{
				_captionX = value;
				OnCaptionXChanged();
			}
		}

		protected void OnCaptionXChanged()
		{	
			Invalidate();
		}

		public string CaptionY 
		{
			get{return _captionY;}
			set
			{
				_captionY = value;
				Invalidate();
			}
		}
		
		protected void OnCaptionYChanged()
		{	
			Invalidate();
		}

		public double MinX
		{
			get{return _minx;}
			set{_minx = value;}
		}

		public double MinY
		{
			get{return _miny;}
			set{_miny = value;}
		}
		
		public double MaxX
		{
			get{return _maxx;}
			set{_maxx = value;}
		}

		public double MaxY
		{
			get{return _maxy;}
			set{_maxy = value;}
		}

		public int	SessionX
		{
			get{return _deltaX;}
			set{_deltaX = value;}
		}

		public int	SessionY
		{
			get{return _deltaY;}
			set{_deltaY = value;}
		}

		public int	SubSessionX
		{
			get{return _subDeltaX;}
			set{_subDeltaX = value;}
		}

		public int	SubSessionY
		{
			get{return _subDeltaY;}
			set{_subDeltaY = value;}
		}

		public Color ClientBackColor
		{
			get{return _clientColor;}
			set{_clientColor = value;}
		}

		public int IsDataValid
		{
			get {return isDataValid();}
		}
		
		public int DataValues
		{
			get
			{
				if ( _values != null ) 
				{				
					return _values.Length;
				}
				return 2;
			}
			
		}
		#endregion

		#region Constructor/Destructor

		public LineChart()
		{
			InitializeComponent();
			
			// initialize control styles
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);

			// initialize drawing resources
			_clientColor = Color.White;
			_penLine = Pens.Red;
			_penCoordinate = Pens.Black; 
			_penStep = Pens.Yellow; 
			_penSubStep = Pens.Black; 
			_penSeparator = Pens.Green; 
			_penThresholdLine = Pens.DarkGreen; 
			_fontCoordinate = new Font("Arial", 8);
			_fontCaption = new Font("Arial", 8);
			_brushCommon = Brushes.Black; 
			_brushCaption = Brushes.Blue; 

			_penStep = new Pen(Color.Black, 1); 
			_penStep.DashStyle = DashStyle.Dash;

			_trendLineInfo = new TrendLineFormat();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();

				if (_penStep != null)
					_penStep.Dispose();
				if (_fontCoordinate != null)
					_fontCoordinate.Dispose();
				if (_fontCaption != null)
					_fontCaption.Dispose();
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
			// 
			// LineChart
			// 
			this.Name = "LineChart";
			this.Size = new System.Drawing.Size(584, 336);
			this.Resize += new System.EventHandler(this.LineChart_Resize);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.LineChart_Paint);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LineChart_MouseMove);
			this.MouseLeave += new System.EventHandler(this.LineChart_MouseLeave);

		}
		#endregion

		#region drawing rountines

		public void DrawExpression(Graphics graph)
		{
			if(_expression == null ) return;
			
			Font fntNormal = new Font("Arial", 10);
			Font fntSuperScript = new Font("Arial", 8);
			
			try
			{
				float left = this.Left;
				float top  = this.Bottom;
				SizeF size;
			
				for (int idx=0; idx<_expression.Count; idx++)
				{
					ExpressionElement ele = _expression.getElement(idx);
					string sExpression = ele.getString();
					switch(ele.CoefType)
					{
						case COEFS_TYPE.NORMAL:
						case COEFS_TYPE.COEF_SIGN :
						case COEFS_TYPE.COEFFICIENT:
							size = graph.MeasureString(sExpression, fntNormal);
							top = this.Bottom - size.Height;
							if (sExpression.Length > 0 && idx > 0)
							{
								left -= graph.MeasureString(sExpression.Substring(0,1), fntNormal).Width/2;
							}
							graph.DrawString(sExpression, fntNormal, Brushes.Green, left, top, System.Drawing.StringFormat.GenericDefault);
							left += size.Width;	
							break;
						case COEFS_TYPE.POWER_SIGN:
						case COEFS_TYPE.POWER :						
							size = graph.MeasureString(sExpression, fntSuperScript);
							if(sExpression.Length > 0)
							{
								left -= graph.MeasureString(sExpression.Substring(0,1), fntNormal).Width/2;
							}

							graph.DrawString(sExpression, fntSuperScript, Brushes.Red, left, top, System.Drawing.StringFormat.GenericDefault);
							left += size.Width;
							break;
					}
				}
			}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);
			}
			finally
			{
				fntNormal.Dispose();
				fntSuperScript.Dispose();
			}
		}

		private void DrawSeparator(Graphics graph)
		{
			if( _ptMouseCurrent!=Point.Empty)
			{
				graph.DrawLine(_penSeparator, new Point(_ptMouseCurrent.X, _axisY.Y), new Point(_ptMouseCurrent.X, _axisOrg.Y));
			}
		}

		private void DrawStepX(Graphics graph)
		{
			StringFormat v=new StringFormat();
			v.Alignment=StringAlignment.Center;
			float stepdrawx=(float)( (float)(_axisX.X-_axisOrg.X)/_deltaX);
			float substepdrawx=(float)(stepdrawx/_subDeltaX);
			for(int i=0;i<=_deltaX;i++)
			{
				graph.DrawLine(_penCoordinate,new PointF(_axisOrg.X + i*stepdrawx,_axisOrg.Y+5),new PointF(_axisOrg.X + i*stepdrawx,_axisOrg.Y));
				graph.DrawString( ((this.MinX+ (this.MaxX-this.MinX)*i/_deltaX)).ToString("0.##"),
					_fontCoordinate,_brushCommon,
					new PointF(_axisOrg.X + i*stepdrawx,_axisOrg.Y+6),
					v);

				//draw grid
				graph.DrawLine(_penStep,new PointF(_axisOrg.X + i*stepdrawx,_axisOrg.Y),new PointF(_axisOrg.X + i*stepdrawx,_axisY.Y));

				//draw sub step
				if( i>0)
					for(int j=0;j<_subDeltaX;j++)
						graph.DrawLine(_penCoordinate,new PointF(_axisOrg.X + i*stepdrawx-j*substepdrawx,_axisOrg.Y+3),new PointF(_axisOrg.X + i*stepdrawx-j*substepdrawx,_axisOrg.Y));
			}
		}

		private void DrawStepY(Graphics graph)
		{
			StringFormat hr2l = new StringFormat(StringFormatFlags.DirectionRightToLeft);
			hr2l.LineAlignment = StringAlignment.Center;
			float stepdrawy = (float)((float)(_axisY.Y-_axisOrg.Y)/_deltaY);
			float substepdrawy = (float)(stepdrawy/_subDeltaY);
			for(int i=0; i<=_deltaY; i++)
			{
				graph.DrawLine(_penCoordinate,new PointF(_axisOrg.X,_axisOrg.Y + i*stepdrawy),new PointF(_axisOrg.X-5,_axisOrg.Y + i*stepdrawy));

				if (!this.Logarithmic)
					graph.DrawString( ((this.MinY+ (this.MaxY-this.MinY)*i/_deltaY)).ToString("0.##"),
						_fontCoordinate,_brushCommon,
						new PointF(_axisOrg.X-6,_axisOrg.Y + i*stepdrawy),
						hr2l);
				else
					graph.DrawString( ((int)Math.Pow(10,i+_logMin)).ToString(),
						_fontCoordinate,_brushCommon,
						new PointF(_axisOrg.X-6,_axisOrg.Y + i*stepdrawy),
						hr2l);

				//draw grid
				graph.DrawLine(_penStep,new PointF(_axisOrg.X,_axisOrg.Y + i*stepdrawy),new PointF(_axisX.X,_axisOrg.Y + i*stepdrawy));

				//draw substep
				if( i>0)
					for(int j=1;j<_subDeltaY;j++)
						graph.DrawLine(_penCoordinate,new PointF(_axisOrg.X,_axisOrg.Y + i*stepdrawy - j*substepdrawy),new PointF(_axisOrg.X-3,_axisOrg.Y + i*stepdrawy - j*substepdrawy));
			}
		}

		private void DrawData(Graphics graph)
		{
			if(adrawpoint!=null && adrawpoint.Length>1)
			{
				PointF pt1, pt2;
				pt1 = PointF.Empty;
				pt2 = PointF.Empty;
				try
				{
					Rectangle rcclip=_validRectangle;
					rcclip.X-=1;
					rcclip.Y-=1;
					rcclip.Width+=2;
					rcclip.Height+=2;
					graph.SetClip(rcclip);

					//graph.DrawLines(_penLine,adrawpoint);
					for(int i=1;i<adrawpoint.Length;i++)
					{
						graph.DrawLine(_penLine,adrawpoint[i-1],adrawpoint[i]);
						if (this.ThresholdVisible && _thresholdPoints!=null)
						{
							if(m_bMovingAverage)
							{
								if(i -1 >= mXStart)
								{
								
									pt1 = _thresholdPoints[i-1 - mXStart];
									pt2 = _thresholdPoints[i - mXStart];
									graph.DrawLine(_penThresholdLine,pt1,pt2);
								}

							}
							else
							{
							
								pt1 = _thresholdPoints[i-1];
								pt2 = _thresholdPoints[i];
								graph.DrawLine(_penThresholdLine,_thresholdPoints[i-1],_thresholdPoints[i]);
							}
						}
					}

					graph.ResetClip();
				}
				catch(Exception exp)
				{
					Trace.WriteLine(exp);
					Trace.WriteLine(pt1.ToString());
					Trace.WriteLine(pt2.ToString());
				}
			}
		}

		private void DrawCaption(Graphics graph)
		{
			StringFormat c = new StringFormat();
			c.LineAlignment = StringAlignment.Center;
			c.Alignment = StringAlignment.Center;
			Matrix mtold = graph.Transform;
			Matrix mt1 = new Matrix();
			mt1.RotateAt(270,new Point(10,_axisY.Y+(_axisOrg.Y-_axisY.Y)/2));
			graph.Transform = mt1;
			graph.DrawString(this.CaptionY, _fontCaption, _brushCaption,new Point(10,_axisY.Y+(_axisOrg.Y-_axisY.Y)/2),c);
			graph.Transform = mtold;
			graph.DrawString(this.CaptionX, _fontCaption, _brushCaption,new Point(_axisOrg.X+(_axisX.X-_axisOrg.X)/2, _clientRectangle.Height-20),c);

			if (this.ThresholdVisible &&  (this.Coeffs != null))
			{
				
				Font fntNormal = new Font("Arial", 10);
				Font fntSuperScript = new Font("Arial", 8);
				string Expression = "Y = ";
				float left = this.Left;
				float top  = this.Bottom;
				SizeF size;
				for (int idx=0; idx<this.Coeffs.Count; idx++)
				{
					if (idx == 0)
					{
						size = graph.MeasureString(Expression, fntNormal);
						top = this.Bottom - size.Height;
						graph.DrawString(Expression, fntNormal, Brushes.Green, left, top, System.Drawing.StringFormat.GenericDefault);
						left += size.Width;						
					}

					Expression = kUtils.convertFloatPoint((double)this.Coeffs[idx]);
					Expression += (idx==0 ? "" : "*x");
					size = graph.MeasureString(Expression, fntNormal);
					graph.DrawString(Expression, fntNormal, Brushes.Green, left, top, System.Drawing.StringFormat.GenericDefault);
					left += size.Width;

					if (idx > 0)
					{
						Expression = (idx).ToString();
						size = graph.MeasureString(Expression, fntSuperScript);
						left -= size.Width / 2;
						graph.DrawString(Expression, fntSuperScript, Brushes.Red, left, top, System.Drawing.StringFormat.GenericDefault);
						left += size.Width;
					}			
		
					if (idx < this.Coeffs.Count-1)
					{
						Expression = " + ";
						size = graph.MeasureString(Expression, fntNormal);
						graph.DrawString(Expression, fntNormal, Brushes.Green, left, top, System.Drawing.StringFormat.GenericDefault);
						left += size.Width;
					}
				}
				fntNormal.Dispose();
				fntSuperScript.Dispose();
			}
		}

		private void Draw(Graphics graph)
		{
			// fill client region
			//graph.FillRectangle(new SolidBrush(_clientColor), ClientRectangle);//new Rectangle(_axisY,new Size(_axisX.X-_axisOrg.X,_axisOrg.Y-_axisY.Y)));
			graph.Clear(_clientColor);

			// draw step x
			DrawStepX(graph);

			// draw step y
			DrawStepY(graph);

			// draw coordinates
			graph.DrawLines(_penCoordinate, new Point[]{_axisY, _axisOrg, _axisX});

			// draw data
			DrawData(graph);			

			// draw caption
			DrawCaption(graph);
					
			// draw separator
			DrawSeparator(graph);
			
			// draw expression
			DrawExpression(graph);
		}
		#endregion

		#region handle events

		public  event XValueChange	mousechange;
		public  event EventHandler  reset;

		private void LineChart_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			// if (!breflectevent) return;
			if (adrawpoint==null || adrawpoint.Length<=1 ) return;
			if (e.X >= _minRealX && e.X <=_maxRealX && e.Y>=_axisY.Y && e.Y<=_axisOrg.Y)
			{
				if( _ptMouseCurrent.X!=e.X)
				{
					_ptMouseCurrent=new Point(e.X,e.Y);
					Invalidate();
					if( mousechange!=null)
					{
						int pos=Array.BinarySearch( adrawpoint,new PointF(e.X,e.Y),cpr);
						if( pos >= 0)
							mousechange(pos,this.Values[pos]);
					}
				}
			}
			else
			{
				_ptMouseCurrent=Point.Empty;
				Invalidate();
				if( reset!=null)
					reset(null,EventArgs.Empty);
			}
		}

		private void LineChart_Resize(object sender, System.EventArgs e)
		{
			PrepareData();
			Invalidate();
		}

		private void PrepareData()
		{
			_clientRectangle = new Rectangle(new Point(0,0), this.Size);
			_axisOrg=new Point(_clientRectangle.Left + MARGIN_LEFT, _clientRectangle.Bottom - MARGIN_BOTTOM);
			_axisX=new Point(_clientRectangle.Right - MARGIN_RIGHT, _clientRectangle.Bottom - MARGIN_BOTTOM);
			_axisY=new Point(_clientRectangle.Left + MARGIN_LEFT, _clientRectangle.Top + MARGIN_TOP);
			_validRectangle = new Rectangle(_axisY,new Size(_axisX.X-_axisOrg.X,_axisOrg.Y-_axisY.Y));
			if (this.MaxY<=this.MinY) 
			{
			
				adrawpoint=null;
				_thresholdPoints=null;
			}
			else if(this.Values == null)
			{
				adrawpoint = null;
				_expression = null;
				mTrendline = null;
			}
			else if( this.Values!=null)
			{
				adrawpoint=new PointF[this.Values.Length];
				_minRealX=double.MaxValue;
				_maxRealX=double.MinValue;
				FPointX.delta=(_axisX.X-_axisOrg.X)/this.MaxX;

				//prepare data for threshold
				ArrayList coeffs = new ArrayList();
				double []athreshold = null;
				string exp = string.Empty;
	
				m_bMovingAverage = false;
				
				_expression = null;
				prepareDynamicThresholdAndExpression(ref athreshold);
				this.Coeffs = coeffs;

				if(athreshold!=null)
					_thresholdPoints=new PointF[athreshold.Length];
				else
					_thresholdPoints=null;

				if (this.Logarithmic)
				{
					_logMin=logmax=0;
					if( this.MinY>=1) 
						_logMin=(double)( (int)(Math.Log10(this.MinY)) );
					if( this.MaxY>=1) 
						logmax=(double)( (int)(Math.Log10(this.MaxY)+1) );
					delta=logmax-_logMin;

					if(delta<=0) 
					{
						adrawpoint=null;
						return;
					}

					//re call step
					SessionY=(int)delta;
				}
				for(int i=0;i<this.Values.Length;i++)
				{
					adrawpoint[i].X= (float)(_axisOrg.X + i*(_axisX.X-_axisOrg.X)/this.MaxX);
					if(athreshold!=null)
					{
						if(m_bMovingAverage)
						{
							if(i >= mXStart)
								_thresholdPoints[i - mXStart].X=adrawpoint[i].X;
						}
						else
							_thresholdPoints[i].X=adrawpoint[i].X;
					}

					if (!this.Logarithmic)
					{
						adrawpoint[i].Y= (float)(_axisOrg.Y - (this.Values[i]-this.MinY)*(_axisOrg.Y-_axisY.Y)/(this.MaxY-this.MinY));

						if(athreshold!=null)
						{
							if(m_bMovingAverage)
							{
								if(i >= mXStart)
								{
									_thresholdPoints[i-mXStart].Y=(float)(_axisOrg.Y - (athreshold[i-mXStart]-this.MinY)*(_axisOrg.Y-_axisY.Y)/(this.MaxY-this.MinY));
									
								}
							}
							else
								_thresholdPoints[i].Y=(float)(_axisOrg.Y - (athreshold[i]-this.MinY)*(_axisOrg.Y-_axisY.Y)/(this.MaxY-this.MinY));
						}
					}
					else
					{
						if(this.Values[i]>=1)
						{
							adrawpoint[i].Y= 
								(float)(_axisOrg.Y - (Math.Log10(this.Values[i]) - _logMin)*(_axisOrg.Y-_axisY.Y)/delta);
							if(athreshold!=null)
							{
								
								if(m_bMovingAverage)
								{
									if(i >= mXStart)
									{
										
										_thresholdPoints[i - mXStart].Y= 
											(float)(_axisOrg.Y - (Math.Log10(Math.Max(athreshold[i - mXStart],1)) - _logMin)*(_axisOrg.Y-_axisY.Y)/delta);
									
									}
								}
								else
								{
								
									_thresholdPoints[i].Y= 
										(float)(_axisOrg.Y - (Math.Log10(Math.Max(athreshold[i],1)) - _logMin)*(_axisOrg.Y-_axisY.Y)/delta);
								}
							}

						}
						else
						{
							adrawpoint[i].Y= 
								(float)(_axisOrg.Y - (-_logMin)*(_axisOrg.Y-_axisY.Y)/delta);
							if(athreshold!=null)
							{
								if(m_bMovingAverage)
								{
									if(i >= mXStart)
									{
										_thresholdPoints[i - mXStart].Y= 
											(float)(_axisOrg.Y - (-_logMin)*(_axisOrg.Y-_axisY.Y)/delta);
									
									}
								}
								else
								{
						
									_thresholdPoints[i].Y= 
										(float)(_axisOrg.Y - (-_logMin)*(_axisOrg.Y-_axisY.Y)/delta);
								}
							}

						}
					}
					if(_minRealX> adrawpoint[i].X)	_minRealX= adrawpoint[i].X;
					if(_maxRealX<adrawpoint[i].X)	_maxRealX=adrawpoint[i].X;
				}
			}
			else
			{
				adrawpoint=null;
				_thresholdPoints=null;
			}

		
	}


		private void LineChart_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Draw(e.Graphics);
		}


		FPointX cpr=new FPointX();

		private void LineChart_MouseLeave(object sender, System.EventArgs e)
		{
			_ptMouseCurrent=Point.Empty;
			Invalidate();
			if( reset!=null)
				reset(null,EventArgs.Empty);
		}
	
		class FPointX : IComparer
		{
			public static double delta;
			int IComparer.Compare(object a, object b)
			{
				PointF c1=(PointF)a;
				PointF c2=(PointF)b;
				if( c1.X < c2.X -delta/2 )
					return -1;
				else if(c1.X > c2.X+ delta/2)
					return 1;
				else
					return 0;
			}
		}


		#endregion	

		private int  isDataValid()
		{
			if ( this.Values != null && this.Values.Length > 1 ) 
			{
				for( int i=0; i<this.Values.Length; i++ ) 
				{
					if ( this.Values[i] <= 0) return 0;
				}
			}
			else return -1;
			return 1;
		}
		
		public void SaveTrendLineInfo()
		{
			_trendLineInfo.SaveLineProfile();
		}

		public void UpdateTrendLineInfo()
		{
			if(mTrendlineDlg != null)
			{
				mTrendlineDlg.UpdateTrendLineInfo(_trendLineInfo);
			}
			
		}

		public TrendLineFormat LoadTrendLineInfo()
		{
			_trendLineInfo.LoadLineProfile();
			return _trendLineInfo;
		}
		public SIA.SystemLayer.Trendline getTrendLine()
		{
			return mTrendline;
		}
		public void setTrendlineDlg(DlgTrendline dlg)
		{
			mTrendlineDlg = dlg;
		}
		public DlgTrendline TRENDLINE_DLG
		{
			get
			{
				return mTrendlineDlg;
			}
			set
			{
				mTrendlineDlg = value;
			}
		}
		public double[] thresholdData()
		{
			double[] result = null;
			double minT = mTrendlineDlg.MinNoiseThreshold;
			double maxT = mTrendlineDlg.MaxNoiseThresHold;
			if(this.Values!= null && this.Values.Length > 0)
			{
				if(mTrendlineDlg != null){
				
				result = new double[this.Values.Length];
				for(int i = 0; i < result.Length; i++)
				{
					result[i] = this.Values[i];
					if(this.Values[i] < minT)
					{
						result[i] = minT;
					}
					if(this.Values[i] > maxT)
					{
						result[i] = maxT; 
					}
				}
				}
			}
			return result;

		}
		public bool HasTrendline()
		{
			bool result = true;
			if(this.Values == null || (this.Values != null && this.Values.Length == 1))
			{
				result = false;
			}
			return result;
		}
		public string getTrendLineValueString(double x)
		{
			string result="";
			double value_result = TrendlineValue(x);
			result = value_result .ToString();
			return result;
		}
		public double TrendlineValue(double x)
		{
			double result = 0;
			if(mTrendline != null)
			{
				result = mTrendline.getValue(x);
			}
			return result;
		}
		public void prepareDynamicThresholdAndExpression(ref double[] athreshold)
		{
			if (this.ThresholdVisible)
			{
				if (this.Values != null && this.Values.Length > 2)
				{
					updateTrendLine();
					if (_trendLineInfo != null && _trendLineInfo.Trenline_Type != eTrendlineType.MovingAverage)
					{
									
						if(mTrendline != null)
							athreshold = mTrendline.getYValueArray(); 
						if(_expression != null)
							_expression.mergeNormalAndPower();
					}
					else if(_trendLineInfo != null && _trendLineInfo.Trenline_Type == eTrendlineType.MovingAverage)
					{
						if(mTrendline != null)
						{
							athreshold = mTrendline.createMovingAverage();
							mXStart = mTrendline.MovingAverage.getXValueStart();
							_expression = null;
						}
					}
				}
			}

		}

		public void updateTrendLine()
		{
			double[] operate_data;			
			if(mTrendlineDlg != null)
			{
				mTrendlineDlg.UpdateTrendLineInfo(_trendLineInfo);
			}
			
			if(_trendLineInfo.ManualMaxMin_Checked && mTrendlineDlg != null)
			{
				operate_data = thresholdData();
			}
			else
			{
				operate_data = this.Values;
			}
			switch (_trendLineInfo.Trenline_Type)
			{
				case eTrendlineType.Linear:					
					this.TrendLineOrder = 1;							
					_trendLineInfo.Order_Value = 1;							
					mTrendline = new SIA.SystemLayer.Trendline(operate_data,_trendLineInfo);
					if(_trendLineInfo.Value_Checked)
						mTrendline.addConstant(_trendLineInfo.Constant_Value);
					_expression = mTrendline.getExpression();
					break;
				case eTrendlineType.Logarithmic:
					mTrendline = SIA.SystemLayer.Trendline.createLogarithm(operate_data,_trendLineInfo);
					if(_trendLineInfo.Value_Checked)								
						mTrendline.addConstant(_trendLineInfo.Constant_Value);
					_expression = mTrendline.getExpression();
					break;;
				case eTrendlineType.Polynomial:
					if(_trendLineInfo._Type == 1)//over grass
					{
						mTrendline = new SIA.SystemLayer.Trendline(operate_data,_trendLineInfo,_trendLineInfo._Type);
						break;
					}
					this.TrendLineOrder= (int)mTrendlineDlg.Order;							
					mTrendline = new SIA.SystemLayer.Trendline(operate_data,_trendLineInfo);
					if(_trendLineInfo.Value_Checked)
						mTrendline.addConstant(_trendLineInfo.Constant_Value);
					_expression = mTrendline.getExpression();
					break;
				case eTrendlineType.Power :
					mTrendline = SIA.SystemLayer.Trendline.createPower(operate_data,_trendLineInfo);
					if(_trendLineInfo.Value_Checked)
						mTrendline.addConstant(_trendLineInfo.Constant_Value);
					_expression = mTrendline.getExpression();
					break;;
				case eTrendlineType.Exponential:
					mTrendline = SIA.SystemLayer.Trendline.createExponential(operate_data,_trendLineInfo);
					if(_trendLineInfo.Value_Checked)
						mTrendline.addConstant(_trendLineInfo.Constant_Value);
					_expression = mTrendline.getExpression();
					break;;
				case eTrendlineType.MovingAverage:
					mTrendline = SIA.SystemLayer.Trendline.createMovingAverageParameter(operate_data,_trendLineInfo);
					if(_trendLineInfo.Value_Checked)
						mTrendline.addConstant(_trendLineInfo.Constant_Value);
					m_bMovingAverage = true;
					break;	
				case eTrendlineType.MiddleAverage:
					mTrendline = SIA.SystemLayer.Trendline.createMiddleMovingAverageTrendLine(operate_data,_trendLineInfo);
					if(_trendLineInfo.Value_Checked)
						mTrendline.addConstant(_trendLineInfo.Constant_Value);
					_expression = mTrendline.getExpression();
					break;;
							
			}
			if( mTrendlineDlg!= null && _trendLineInfo.Trenline_Type!= eTrendlineType.MovingAverage)
			{
				double[] trendline_data = mTrendline.getYValueArray();
				if(trendline_data != null)
				{
					double deviation = SIA.Common.Utility.Utils.Deviation(trendline_data,operate_data);
					mTrendlineDlg.DeviationText = Convert.ToString(Math.Round(deviation,2));
				}
			}
			else if(mTrendlineDlg!= null && _trendLineInfo.Trenline_Type== eTrendlineType.MovingAverage)
			{
				double[] trendline_data = mTrendline.getYValueArray();
				int xstart = mTrendline.MovingAverage.getXValueStart();
				double deviation = SIA.Common.Utility.Utils.Deviation(trendline_data,xstart,operate_data,0);
				mTrendlineDlg.DeviationText = Convert.ToString(Math.Round(deviation,2));
			}
		}
	}
}