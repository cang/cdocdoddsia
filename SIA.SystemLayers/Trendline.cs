using System;
using System.Collections;
using System.Drawing;

using SIA.Common;
using SIA.Common.Analysis;
using SIA.Common.Mathematics;
using SIA.Common.KlarfExport;

using SIA.IPEngine;

namespace SIA.SystemLayer
{
    /// <summary>
    /// This class was used for trendline analysis and it is nolonger use.
    /// </summary>
	[Obsolete]
	public class Trendline
	{
		private int mType;
        private SIA.IPEngine.Function m_Function;
		private double mStartDefinitionRange;
		private double mEndDefinitionRange;
		private ArrayList mResultPoints;

		public Trendline()
		{
		}

		public Trendline(ArrayList defects,int nLevel)
		{
			PolynomialOneVariant poly ;							
			poly = new PolynomialOneVariant(nLevel,defects,0);			
			m_Function = poly;		
		}

		public float GetStd ( double y,ArrayList defects)
		{
			if  ( defects.Count <= 0 ) return 0;
			double sum2 = 0;
			for( int i=0;i<defects.Count;i++ )
			{
				sum2 += ( y - (( DefectElement)defects[i]).Y ) * ( y - (( DefectElement)defects[i]).Y );
			}

			return (float)Math.Sqrt( sum2/defects.Count );
		}


		//Tan update  12-19-2005
		public Trendline(double[] y,TrendLineFormat TrendlineInfo)
		{
			PolynomialOneVariant poly = new PolynomialOneVariant((int)TrendlineInfo.Order_Value);
			if (TrendlineInfo.Kernel_Checked)
				y = GetDataKernel(y,TrendlineInfo);
			if(!TrendlineInfo._Automatic)
				poly.computeCoefsByLeastSquare(y,(int)TrendlineInfo.Order_Value);
			else
				poly.ComputeCoefsAutomatic(y,(int)TrendlineInfo.Order_Value);
			m_Function =poly;
			mStartDefinitionRange = 0;
			mEndDefinitionRange = y.Length -1;			
		}
		//type = 0 : PolynomialOneVariant
		//type = 1 : Discrete
		public Trendline(double[] y,TrendLineFormat TrendlineInfo,int type)
		{
			if(type == 0)
			{
				PolynomialOneVariant poly = new PolynomialOneVariant((int)TrendlineInfo.Order_Value);
				if (TrendlineInfo.Kernel_Checked)
					y = GetDataKernel(y,TrendlineInfo);
				if(!TrendlineInfo._Automatic)
					poly.computeCoefsByLeastSquare(y,(int)TrendlineInfo.Order_Value);
				else
					poly.ComputeCoefsAutomatic(y,(int)TrendlineInfo.Order_Value);
				m_Function =poly;
				mStartDefinitionRange = 0;
				mEndDefinitionRange = y.Length -1;			
			}
			else if(type == 1)
			{
				ArrayList ar = new ArrayList();
				for(int i = 0; i< y.Length; ++i)
				{
					ar.Add(new PointF(i,(float)y[i]));
				}
				PolynomialOneVariant poly = new PolynomialOneVariant((int)TrendlineInfo.Order_Value);
				double con = 0;
				bool resultfun = poly.CreatePSL(ar,ref con);
				if(!resultfun) return;
				ar= poly.GetUpOverToDefectThreshold(con,ar);
				mType = type;
				mResultPoints = ar;
			}
		}

		private static double[] GetDataKernel(double[] y,TrendLineFormat TrendlineInfo)
		{
				LineData lData = new LineData(y);
				switch(TrendlineInfo.radio_Checked) 
				{
					case "Average":
						return  lData.Average(TrendlineInfo.Kernel_Size,TrendlineInfo);
					
				}  	
				return lData.Mean(TrendlineInfo.Kernel_Size,TrendlineInfo);
		}

		public Trendline(double[] y,eTrendlineType trend_line)
		{
			if(trend_line == eTrendlineType.Linear)
			{
				
			}
			else if(trend_line == eTrendlineType.Logarithmic)
			{
				
			}
		}

		public static Trendline createLinear(double[] y)
		{
			Trendline result = new Trendline();
			result.createLinearFunction( y);
			result.StartRange = 0;
			result.EndRange = y.Length;
			return result;
		}
		public static Trendline createLogarithm(double[] y)
		{
			Trendline result = new Trendline();		
			result.createLogarithmFunction(y);
			result.StartRange = 0;
			result.EndRange = y.Length;
			return result;
		}
		public static Trendline createLogarithm(double[] y,TrendLineFormat TrendlineInfo)
		{
			Trendline result = new Trendline();
			//Tan update 12-21-2005
			if (TrendlineInfo.Kernel_Checked)
				y = GetDataKernel(y,TrendlineInfo);

			result.createLogarithmFunction(y);
			result.StartRange = 0;
			result.EndRange = y.Length;
			return result;
		}
		public static Trendline createPower(double[] y)
		{
			Trendline result = new Trendline();
			
			result.createPowerFunction(y);
			result.StartRange = 0;
			result.EndRange = y.Length;
			return result;
		}
		public static Trendline createPower(double[] y,TrendLineFormat TrendlineInfo)
		{
			Trendline result = new Trendline();
			//Tan update 12-21-2005
			if (TrendlineInfo.Kernel_Checked)
				y = GetDataKernel(y,TrendlineInfo);;

			result.createPowerFunction(y);
			result.StartRange = 0;
			result.EndRange = y.Length;
			return result;
		}
		public static Trendline createExponential(double[] y)
		{
			Trendline result = new Trendline();			
			result.createExponentialFunction(y);
			result.StartRange = 0;
			result.EndRange = y.Length;
			return result;
		}
		public static Trendline createExponential(double[] y,TrendLineFormat TrendlineInfo)
		{
			Trendline result = new Trendline();
			//Tan update 12-21-2005
			if (TrendlineInfo.Kernel_Checked)
				y = GetDataKernel(y,TrendlineInfo);
			result.createExponentialFunction(y);
			result.StartRange = 0;
			result.EndRange = y.Length;
			return result;
		}
		public void createLogarithmFunction(double[] y)
		{
			m_Function = new LogarithmicFunction(y);
		}

		public void createPowerFunction(double[] y)
		{
			m_Function = new PowerFunction(y);
		}
		public void createExponentialFunction(double[] y)
		{
			m_Function = new ExponentialFunction(y);
		}
		public double StartRange
		{
			get
			{
				return mStartDefinitionRange;
			}
			set
			{
				mStartDefinitionRange = value;
			}
		}
		public double EndRange
		{
			get
			{
				return mEndDefinitionRange;
			}
			set
			{
				mEndDefinitionRange = value;
			}
		}

		public void createLinearFunction(double[] y)
		{
			this.m_Function = new PolynomialOneVariant(1);
			((PolynomialOneVariant)m_Function).computeCoefsByLeastSquare(y,1);
		}
		public static Trendline createMiddleMovingAverageTrendLine(double[] y,TrendLineFormat TrendlineInfo)
		{
			int period = (int)TrendlineInfo.Period_Value;
			Trendline result = new Trendline();
			//Tan update 12-21-2005
			if (TrendlineInfo.Kernel_Checked)
				y = GetDataKernel(y,TrendlineInfo);

			result.createMiddleMovingAverage(y,period);
			result.StartRange = 0;
			result.EndRange = y.Length-1;
			return result;
		}
		public static Trendline createMiddleMovingAverageTrendLine(double[] y,int period)
		{
			Trendline result = new Trendline();			

			result.createMiddleMovingAverage(y,period);
			result.StartRange = 0;
			result.EndRange = y.Length-1;
			return result;
		}
		public void createMiddleMovingAverage(double[] y,int period)
		{
			this.m_Function = new MiddleMovingAverageTransform(y,period);
			
		}
		public double[] m_input;
		public int m_period;
		public MovingAverageTransform mMovingAverare;
		public static Trendline createMovingAverageParameter(double[] input, int period)
		{
			Trendline tl = new Trendline();
			MovingAverageTransform mt = new MovingAverageTransform(period);
			tl.m_input = input;
			tl.m_period = period;
			tl.mMovingAverare = mt;
			return tl;
		}		
		public static Trendline createMovingAverageParameter(double[] input,TrendLineFormat TrendlineInfo)
		{
			int period = (int)TrendlineInfo.Period_Value;
			Trendline tl = new Trendline();
			//Tan update 12-21-2005
			if (TrendlineInfo.Kernel_Checked)
				input = GetDataKernel(input,TrendlineInfo);
			MovingAverageTransform mt = new MovingAverageTransform(period);
			tl.m_input = input;
			tl.m_period = period;
			tl.mMovingAverare = mt;
			return tl;
		}		

		public MovingAverageTransform MovingAverage
		{
			get
			{
				return mMovingAverare;
			}
		}
		public  double[] createMovingAverage()
		{
			return mMovingAverare.getYArray(m_input);
		}
		public void addConstant(double con)
		{
			if(m_Function != null)
				m_Function.addConstant(con);
			else
				MovingAverage.addConstant(con);
		}
		public double[] getYValueArray()
		{
			double[] result=null;
			//case of over defect thereshold
			if(mType == 1)
			{
				result = new double[mResultPoints.Count];
				for(int i =0; i< mResultPoints.Count; ++i)
				{
					result[i] = ((PointF)mResultPoints[i]).Y;
				}
				return result;
			}
			
			if(m_Function != null)
			{
				result  = m_Function.getYArray((int)mStartDefinitionRange,(int)mEndDefinitionRange);
			}
			else if(MovingAverage != null)
				result = MovingAverage.getYArray(m_input);
			return result;
		}
		public Expression getExpression()
		{
			return m_Function.get_Expression();
		}
		public double getValue(double x)
		{
			double result=0;
			if(m_Function != null)
				result  = m_Function.getValue(x);
			else if(MovingAverage != null)
				result = MovingAverage.getValue(x);
			return result;

		}
	}
}
