using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace SIA.SystemLayer.Mathematics
{
    /// <summary>
    /// The Linear class provide functions for linear interpolation
    /// </summary>
	public class Linear
	{
		private double _a, _b, _coeff;

		/// <summary>
		/// Returns the slope of the regression line
		/// </summary>
		public double Slope
		{
			get {return _b;}
		}

		/// <summary>
		/// Returns the intercept on the Y axis of the regression line
		/// </summary>
		public double Intercept
		{
			get {return _a;}
		}

		/// <summary>
		/// /! Returns the linear regression coefficient
		/// The regression coefficient indicated how well linear regression fits to the
		/// original data. It is an expression of error in the fitting and is defined as:
 
		/// \f[ r = \frac{S_{xy}}{\sqrt{S_{x} \cdot S_{y}}} \f]
 
		/// This varies from 0 (no linear trend) to 1 (perfect linear fit). If \f$ |S_y| =
		/// 0\f$ and \f$ |S_x| \neq 0 \f$,   then \e r is considered to be equal to 1.
		/// </summary>
		public double Coefficients
		{
			get {return _coeff;}
		}

		public double Value(double x)
		{
			return _a + _b*x;
		}
		
		public Linear(double[] x, double[] y)
		{
			InitClass(x, y);
		}

		public Linear(PointF[] pts)
		{
			InitClass(pts);	
		}

		private void InitClass(double[] x, double[] y)
		{
			if (x == null || y == null)
				throw new System.ArgumentNullException("Invalid parameter");
			if (x.Length == 0 || x.Length != y.Length)
				throw new System.ArgumentException("Invalid parameter");
			int n = x.Length;
			// calculate the averages of arrays x and y
			double xa = 0, ya = 0;
			for (int i = 0; i < n; i++) 
			{
				xa += x[i];
				ya += y[i];
			}
			xa /= n;
			ya /= n;
 
			// calculate auxiliary sums
			double xx = 0, yy = 0, xy = 0;
			for (int i = 0; i < n; i++) 
			{
				double tmpx = x[i] - xa, tmpy = y[i] - ya;
				xx += tmpx * tmpx;
				yy += tmpy * tmpy;
				xy += tmpx * tmpy;
			}
 
			// calculate regression line parameters
 
			// make sure slope is not infinite
			Debug.Assert(System.Math.Abs(xx) != 0);
 
			_b = xy / xx;
			_a = ya - _b * xa;
			_coeff = (System.Math.Abs(yy) == 0) ? 1 : xy / System.Math.Sqrt(xx * yy);
		}

		private void InitClass(PointF[] pts)
		{
			if (pts == null)
				throw new System.ArgumentNullException("Invalid parameter");
			int n = pts.Length;
			// calculate the averages of arrays x and y
			double xa = 0, ya = 0;
			for (int i = 0; i < n; i++) 
			{
				xa += pts[i].X;
				ya += pts[i].Y;
			}
			xa /= n;
			ya /= n;
 
			// calculate auxiliary sums
			double xx = 0, yy = 0, xy = 0;
			for (int i = 0; i < n; i++) 
			{
				double tmpx = pts[i].X - xa, tmpy = pts[i].Y - ya;
				xx += tmpx * tmpx;
				yy += tmpy * tmpy;
				xy += tmpx * tmpy;
			}
 
			// calculate regression line parameters
 
			// make sure slope is not infinite
			Debug.Assert(System.Math.Abs(xx) != 0);
 
			_b = xy / xx;
			_a = ya - _b * xa;
			_coeff = (System.Math.Abs(yy) == 0) ? 1 : xy / System.Math.Sqrt(xx * yy);
		}
	}
}