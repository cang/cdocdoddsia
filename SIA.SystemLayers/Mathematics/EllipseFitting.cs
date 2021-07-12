using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SIA.SystemLayer.Mathematics
{
    /// <summary>
    /// The EllipseFitting class provides methods for finding the fitted ellipse of a list of points.
    /// </summary>
	public class EllipseFitting
	{
		public enum Methods 
		{
			FPF,
			Taubin,
			Bookstein
		};


		private Methods _method = Methods.Bookstein;
		private ArrayList _points = null;
		private ArrayList _resultPoints = new ArrayList();
		private double[] _vector = null;

		private PointF[] FitEllipse(PointF[] points, int num_result_points)
		{
			// initialize point list
			if (this._points != null)
				this._points.Clear();
			else
				this._points = new ArrayList();
			this._points.AddRange(points);
			
			// execute fitting algorithm
			this.ExecuteAlgorithm();

			// generate result points
			int num_points = _points.Count;
			this.GenerateResultPoints(num_points);
			return (PointF[])_resultPoints.ToArray(typeof(PointF));
		}

		public void FitEllipse(PointF[] points, ref RectangleF rcBound, ref double angle)
		{
			try
			{
#if false
				OpenCVDotNet.CvBox2D box = new OpenCVDotNet.CvBox2D();
				OpenCVDotNet.CvPoint2D32f[] pts = new OpenCVDotNet.CvPoint2D32f[points.Length];
				for (int i=0; i<pts.Length; i++)
					pts[i] = points[i];
				float[] vector = new float[6];
				
				// fit ellipse
				OpenCVDotNet.cv.cvFitEllipse(pts, ref box, out vector);
				this._vector = new double[vector.Length+1];
				for (int i=0; i<vector.Length; i++)
					this._vector[i+1] = (double)vector[i];

				float width = box.size.width;
				float height = box.size.height;
				float size = Math.Min(width, height);//(width + height)*0.5F;
				float left = box.center.x - size*0.5F;
				float top = box.center.y - size*0.5F;
				rcBound = new RectangleF(left, top, size, size);

				angle = box.angle;
				return;
#endif
				// initialize point list
				if (this._points != null)
					this._points.Clear();
				else
					this._points = new ArrayList();
				this._points.AddRange(points);

				// execute fitting algorithm
				ExecuteAlgorithm();

				// calculate result wafer boundary and angle
				PhongConic(ref rcBound, ref angle);	
			}
			catch (Exception)
			{
				throw;
			}
		}

		private double[,] GenerateContrainMatrix()
		{
			double[,] result = new double[7,7];

			switch (_method)
			{
				case Methods.FPF:
					result[1,3]=-2;
					result[2,2]=1;
					result[3,1]=-2;
					break;
				case Methods.Taubin:
					break;
				case Methods.Bookstein:
					result[1,1]=2;
					result[2,2]=1;
					result[3,3]=2;
					break;
				default:
					break;
			}

			return result;
		}

		private double[,] GenerateDesignMatrix(ArrayList points)
		{
			int num_points = points.Count;
			double[,] result = new double[num_points+1,7];
			double tx, ty;
			for (int i=1; i<=num_points; i++)
			{
				tx = ((PointF)points[i-1]).X;
				ty = ((PointF)points[i-1]).Y;
				result[i,1] = tx*tx;
				result[i,2] = tx*ty;
				result[i,3] = ty*ty;
				result[i,4] = tx;
				result[i,5] = ty;
				result[i,6] = 1.0f;
			}
			return result;
		}

		private void SolveConicPolynomial(double[] vector, ref PointF center, ref float angle)
		{
			double[,] A = new double[3,3];
			double[,] Ai = new double[3,3];
			double[,] Aib = new double[3,2];
			double[,] b = new double[3,2];
			double[,] r1 = new double[2,2];
			double Ao, Ax, Ay, Axx, Ayy, Axy;

			Ao = vector[6];
			Ax = vector[4];
			Ay = vector[5];
			Axx = vector[1];
			Ayy = vector[3];
			Axy = vector[2];

			A[1,1] = Axx;    A[1,2] = Axy/2;
			A[2,1] = Axy/2;  A[2,2] = Ayy;
			b[1,1] = Ax; b[2,1] = Ay;

			// inverse A matrix
			Inverse(A, Ai, 2);
			
			AperB(Ai, b, Aib, 2, 2, 2, 1);
			A_TperB(b, Aib, r1, 2, 1, 2, 1);
			r1[1,1] = r1[1,1] - 4*Ao;

			// compute result center point of ellipse
			center.X = (float)(-Aib[1,1]*0.5F);
			center.Y = (float)(-Aib[2,1]*0.5F);
			
			// Generate normals linspace
			int npts = 2;
			int i,j;
			double kk;
			double[,] u = new double[3,npts+1];
			u[1,1] = Math.Cos(0.0F);
			u[2,1] = Math.Sin(0.0F);
 
			u[1,2] = Math.Cos(Math.PI/2.0F);
			u[2,2] = Math.Sin(Math.PI/2.0F);

			double[,] Aiu = new double[3, npts+1];
			double[,] uAiu = new double[3,npts+1];
			AperB(Ai, u, Aiu, 2, 2, 2, npts);
			for (i=1; i<=2; i++)
				for (j=1; j<=npts; j++)
					uAiu[i,j] = u[i,j] * Aiu[i,j];
		
			double[] lambda = new double[npts+1];
			for (j=1; j<=npts; j++) 
			{
				if ( (kk=(r1[1,1] / (uAiu[1,j]+uAiu[2,j]))) >= 0.0)
					lambda[j] = Math.Sqrt(kk);
				else
					lambda[j] = -1.0; 
			}

			// Builds up B and L
			double[,] L = new double[3,npts+1];
			double[,] B = new double[3,npts+1];

			for (j=1; j<=npts; j++)
				L[1,j] = L[2,j] = lambda[j];

			for (j=1; j<=npts; j++) 
			{
				B[1,j] = b[1,1];
				B[2,j] = b[2,1]; 
			}
	
			double[,] ss1 = new double[3,npts+1];
			double[,] ss2 = new double[3,npts+1];
			for (j=1; j<=npts; j++) 
			{
				ss1[1,j] = 0.5 * (  L[1,j] * u[1,j] - B[1,j]);
				ss1[2,j] = 0.5 * (  L[2,j] * u[2,j] - B[2,j]);
				ss2[1,j] = 0.5 * ( -L[1,j] * u[1,j] - B[1,j]);
				ss2[2,j] = 0.5 * ( -L[2,j] * u[2,j] - B[2,j]); 
			}
	
			
			double[,] Xpos = new double[3,npts+1];
			double[,] Xneg = new double[3,npts+1];

			AperB(Ai,ss1,Xpos,2,2,2,npts);
			AperB(Ai,ss2,Xneg,2,2,2,npts);

			_resultPoints.Clear();
			_resultPoints.Add(new Point((int)Xpos[1,1], (int)Xpos[2,1]));
			_resultPoints.Add(new Point((int)Xpos[1,2], (int)Xpos[2,2]));
			_resultPoints.Add(new Point((int)Xneg[1,1], (int)Xneg[2,1]));
			_resultPoints.Add(new Point((int)Xneg[1,2], (int)Xneg[2,2]));
		}
		
		public void GenerateResultPoints(int nptsk, double[,] points)
		{
			if (_vector == null)
				throw new System.Exception("Invalid operation");

			double[] pvec = _vector;
			int npts = nptsk/2;
			double[,] u = new double[3,npts+1];
			double[,] Aiu = new double[3,npts+1];
			double[,] L = new double[3,npts+1];
			double[,] B = new double[3,npts+1];
			double[,] Xpos = new double[3,npts+1];
			double[,] Xneg = new double[3,npts+1];
			double[,] ss1 = new double[3,npts+1];
			double[,] ss2 = new double[3,npts+1];
			double[] lambda = new double[npts+1];
			double[,] uAiu = new double[3,npts+1];
			double[,] A = new double[3,3];
			double[,] Ai = new double[3,3];
			double[,] Aib = new double[3,2];
			double[,] b = new double[3,2];
			double[,] r1 = new double[2,2];
			double Ao, Ax, Ay, Axx, Ayy, Axy;

			double pi = 3.14781;
			double theta;
			int i;
			int j;
			double kk;

			Ao = pvec[6];
			Ax = pvec[4];
			Ay = pvec[5];
			Axx = pvec[1];
			Ayy = pvec[3];
			Axy = pvec[2];

			A[1,1] = Axx;    A[1,2] = Axy/2;
			A[2,1] = Axy/2;  A[2,2] = Ayy;
			b[1,1] = Ax; b[2,1] = Ay;

			// Generate normals linspace
			for (i=1, theta=0.0; i<=npts; i++, theta+=(pi/npts)) 
			{
				u[1,i] = Math.Cos(theta);
				u[2,i] = Math.Sin(theta); 
			}

			Inverse(A,Ai,2);

			AperB(Ai,b,Aib,2,2,2,1);
			A_TperB(b,Aib,r1,2,1,2,1);
			r1[1,1] = r1[1,1] - 4*Ao;

			AperB(Ai,u,Aiu,2,2,2,npts);
			for (i=1; i<=2; i++)
				for (j=1; j<=npts; j++)
					uAiu[i,j] = u[i,j] * Aiu[i,j];

			for (j=1; j<=npts; j++) 
			{
				if ( (kk=(r1[1,1] / (uAiu[1,j]+uAiu[2,j]))) >= 0.0)
					lambda[j] = Math.Sqrt(kk);
				else
					lambda[j] = -1.0; 
			}
	
			// Builds up B and L
			for (j=1; j<=npts; j++)
				L[1,j] = L[2,j] = lambda[j];

			for (j=1; j<=npts; j++) 
			{
				B[1,j] = b[1,1];
				B[2,j] = b[2,1]; 
			}
	
			for (j=1; j<=npts; j++) 
			{
				ss1[1,j] = 0.5 * (  L[1,j] * u[1,j] - B[1,j]);
				ss1[2,j] = 0.5 * (  L[2,j] * u[2,j] - B[2,j]);
				ss2[1,j] = 0.5 * ( -L[1,j] * u[1,j] - B[1,j]);
				ss2[2,j] = 0.5 * ( -L[2,j] * u[2,j] - B[2,j]); 
			}
	
			AperB(Ai,ss1,Xpos,2,2,2,npts);
			AperB(Ai,ss2,Xneg,2,2,2,npts);
	
			for (j=1; j<=npts; j++) 
			{
				if (lambda[j]==-1.0) 
				{
					points[1,j] = -1.0;
					points[2,j] = -1.0;
					points[1,j+npts] = -1.0;
					points[2,j+npts] = -1.0;
				}
				else 
				{
					points[1,j] = Xpos[1,j];
					points[2,j] = Xpos[2,j];
					points[1,j+npts] = Xneg[1,j];
					points[2,j+npts] = Xneg[2,j];
				}
			}

			_resultPoints = new ArrayList(nptsk);
			for(i=0; i<nptsk; i++)
				_resultPoints.Add(new PointF((float)points[1, i+1], (float)points[2, i+1]));
		}

		public PointF[] GenerateResultPoints(int nptsk)
		{
			if (_vector == null)
				throw new System.Exception("Invalid operation");
			double[,] points = new double[3, nptsk+1];
			double[] pvec = _vector;
			int npts=nptsk/2;
			double[,] u = new double[3,npts+1];
			double[,] Aiu = new double[3,npts+1];
			double[,] L = new double[3,npts+1];
			double[,] B = new double[3,npts+1];
			double[,] Xpos = new double[3,npts+1];
			double[,] Xneg = new double[3,npts+1];
			double[,] ss1 = new double[3,npts+1];
			double[,] ss2 = new double[3,npts+1];
			double[] lambda = new double[npts+1];
			double[,] uAiu = new double[3,npts+1];
			double[,] A = new double[3,3];
			double[,] Ai = new double[3,3];
			double[,] Aib = new double[3,2];
			double[,] b = new double[3,2];
			double[,] r1 = new double[2,2];
			double Ao, Ax, Ay, Axx, Ayy, Axy;

			double pi = 3.14781;
			double theta;
			int i;
			int j;
			double kk;

			Ao = pvec[6];
			Ax = pvec[4];
			Ay = pvec[5];
			Axx = pvec[1];
			Ayy = pvec[3];
			Axy = pvec[2];

			A[1,1] = Axx;    A[1,2] = Axy/2;
			A[2,1] = Axy/2;  A[2,2] = Ayy;
			b[1,1] = Ax; b[2,1] = Ay;

			// Generate normals linspace
			for (i=1, theta=0.0; i<=npts; i++, theta+=(pi/npts)) 
			{
				u[1,i] = Math.Cos(theta);
				u[2,i] = Math.Sin(theta); 
			}

			Inverse(A,Ai,2);

			AperB(Ai,b,Aib,2,2,2,1);
			A_TperB(b,Aib,r1,2,1,2,1);
			r1[1,1] = r1[1,1] - 4*Ao;

			AperB(Ai,u,Aiu,2,2,2,npts);
			for (i=1; i<=2; i++)
				for (j=1; j<=npts; j++)
					uAiu[i,j] = u[i,j] * Aiu[i,j];

			for (j=1; j<=npts; j++) 
			{
				if ( (kk=(r1[1,1] / (uAiu[1,j]+uAiu[2,j]))) >= 0.0)
					lambda[j] = Math.Sqrt(kk);
				else
					lambda[j] = -1.0; 
			}
	
			// Builds up B and L
			for (j=1; j<=npts; j++)
				L[1,j] = L[2,j] = lambda[j];
			for (j=1; j<=npts; j++) 
			{
				B[1,j] = b[1,1];
				B[2,j] = b[2,1]; 
			}
	
			for (j=1; j<=npts; j++) 
			{
				ss1[1,j] = 0.5 * (  L[1,j] * u[1,j] - B[1,j]);
				ss1[2,j] = 0.5 * (  L[2,j] * u[2,j] - B[2,j]);
				ss2[1,j] = 0.5 * ( -L[1,j] * u[1,j] - B[1,j]);
				ss2[2,j] = 0.5 * ( -L[2,j] * u[2,j] - B[2,j]); 
			}
	
			AperB(Ai,ss1,Xpos,2,2,2,npts);
			AperB(Ai,ss2,Xneg,2,2,2,npts);
	
			for (j=1; j<=npts; j++) 
			{
				if (lambda[j]==-1.0) 
				{
					points[1,j] = -1.0;
					points[2,j] = -1.0;
					points[1,j+npts] = -1.0;
					points[2,j+npts] = -1.0;
				}
				else 
				{
					points[1,j] = Xpos[1,j];
					points[2,j] = Xpos[2,j];
					points[1,j+npts] = Xneg[1,j];
					points[2,j+npts] = Xneg[2,j];
				}
			}

			_resultPoints = new ArrayList(nptsk);
			for(i=0; i<nptsk; i++)
				_resultPoints.Add(new PointF((float)points[1, i+1], (float)points[2, i+1]));

			return (PointF[])_resultPoints.ToArray(typeof(PointF));
		}

		public double PseudoDistance(PointF pt)
		{
			if (_vector == null)
				throw new System.Exception("Ellipse was not detected");
			
			double Ao, Ax, Ay, Axx, Ayy, Axy;
			Ao = _vector[6];
			Ax = _vector[4];
			Ay = _vector[5];
			Axx = _vector[1];
			Ayy = _vector[3];
			Axy = _vector[2];

			return Axx*(pt.X * pt.X) + Ayy*(pt.Y*pt.Y) + Axy*(pt.X*pt.Y) + Ax*pt.X + Ay*pt.Y + Ao;
		}

		public void PhongConic(ref RectangleF rcBound, ref double angle)
		{
			if (_vector == null)
				throw new System.Exception("Ellipse was not detected");

			double[,] A = new double[3,3];
			double[,] Ai = new double[3,3];
			double[,] Aib = new double[3,2];
			double[,] b = new double[3,2];
			double[,] r1 = new double[2,2];
			double Ao, Ax, Ay, Axx, Ayy, Axy;

			Ao = _vector[6];
			Ax = _vector[4];
			Ay = _vector[5];
			Axx = _vector[1];
			Ayy = _vector[3];
			Axy = _vector[2];

			A[1,1] = Axx;    A[1,2] = (Axy/2);
			A[2,1] = (Axy/2);  A[2,2] = Ayy;
			b[1,1] = Ax; b[2,1] = Ay;				

			// invert matrix
			Inverse(A,Ai,2);

			double scale_factor = 1.0F;

			AperB(Ai,b,Aib,2,2,2,1);
			A_TperB(b,Aib,r1,2,1,2,1);
			r1[1,1] = r1[1,1] - 4*Ao;

			double [] s_min_max = new double [4];
			double [] T = new double [4];
			double Var1, Var2;

			double Ai11 = Ai[1,1];
			double Ai11x2 = Ai11*Ai11;
			double Ai11x3 = Ai11*Ai11*Ai11;
			double Ai11x4 = Ai11*Ai11*Ai11*Ai11;

			Var1 = Ai11 * (Ai11x2 - 2.0*Ai11*Ai[2,2] + 4.0*Ai[1,2]*Ai[1,2] + Ai[2,2]*Ai[2,2] + Math.Sqrt(Ai11x4 - 4.0*Ai11x3*Ai[2,2] + 4.0*Ai11x2*Ai[1,2]*Ai[1,2] + 6.0*Ai11x2*Ai[2,2]*Ai[2,2] - 8.0*Ai11*Ai[2,2]*Ai[1,2]*Ai[1,2]-4.0*Ai11*Ai[2,2]*Ai[2,2]*Ai[2,2]+4.0*Ai[2,2]*Ai[2,2]*Ai[1,2]*Ai[1,2]+Ai[2,2]*Ai[2,2]*Ai[2,2]*Ai[2,2]))/(Ai11x2-2.0*Ai11*Ai[2,2]+Ai[2,2]*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2])/2.0;      
			Var2 = Ai[1,2] * Math.Sqrt(2.0) * Math.Sqrt((Ai11x2-2.0*Ai11*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2]+Ai[2,2]*Ai[2,2]+Math.Sqrt(Ai11x4-4.0*Ai11x3*Ai[2,2]+4.0*Ai11x2*Ai[1,2]*Ai[1,2]+6.0*Ai11x2*Ai[2,2]*Ai[2,2]-8.0*Ai11*Ai[2,2]*Ai[1,2]*Ai[1,2]-4.0*Ai11*Ai[2,2]*Ai[2,2]*Ai[2,2]+4.0*Ai[2,2]*Ai[2,2]*Ai[1,2]*Ai[1,2]+Ai[2,2]*Ai[2,2]*Ai[2,2]*Ai[2,2]))/(Ai11x2-2.0*Ai11*Ai[2,2]+Ai[2,2]*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2]))*Math.Sqrt(1.0-(Ai11x2-2.0*Ai11*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2]+Ai[2,2]*Ai[2,2]+Math.Sqrt(Ai11x4-4.0*Ai11x3*Ai[2,2]+4.0*Ai11x2*Ai[1,2]*Ai[1,2]+6.0*Ai11x2*Ai[2,2]*Ai[2,2]-8.0*Ai11*Ai[2,2]*Ai[1,2]*Ai[1,2]-4.0*Ai11*Ai[2,2]*Ai[2,2]*Ai[2,2]+4.0*Ai[2,2]*Ai[2,2]*Ai[1,2]*Ai[1,2]+Ai[2,2]*Ai[2,2]*Ai[2,2]*Ai[2,2]))/(Ai11x2-2.0*Ai11*Ai[2,2]+Ai[2,2]*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2])/2.0)+Ai[2,2]*(1.0-(Ai11x2-2.0*Ai11*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2]+Ai[2,2]*Ai[2,2]+Math.Sqrt(Ai11x4-4.0*Ai11x3*Ai[2,2]+4.0*Ai11x2*Ai[1,2]*Ai[1,2]+6.0*Ai11x2*Ai[2,2]*Ai[2,2]-8.0*Ai11*Ai[2,2]*Ai[1,2]*Ai[1,2]-4.0*Ai11*Ai[2,2]*Ai[2,2]*Ai[2,2]+4.0*Ai[2,2]*Ai[2,2]*Ai[1,2]*Ai[1,2]+Ai[2,2]*Ai[2,2]*Ai[2,2]*Ai[2,2]))/(Ai11x2-2.0*Ai11*Ai[2,2]+Ai[2,2]*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2])/2.0);      
			s_min_max[0]= Var1+Var2;      

			Var1 = Ai11*(Ai11x2-2.0*Ai11*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2]+Ai[2,2]*Ai[2,2]+Math.Sqrt(Ai11x4-4.0*Ai11x3*Ai[2,2]+4.0*Ai11x2*Ai[1,2]*Ai[1,2]+6.0*Ai11x2*Ai[2,2]*Ai[2,2]-8.0*Ai11*Ai[2,2]*Ai[1,2]*Ai[1,2]-4.0*Ai11*Ai[2,2]*Ai[2,2]*Ai[2,2]+4.0*Ai[2,2]*Ai[2,2]*Ai[1,2]*Ai[1,2]+Ai[2,2]*Ai[2,2]*Ai[2,2]*Ai[2,2]))/(Ai11x2-2.0*Ai11*Ai[2,2]+Ai[2,2]*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2])/2.0;      
			Var2 = -Ai[1,2]*Math.Sqrt(2.0)*Math.Sqrt((Ai11x2-2.0*Ai11*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2]+Ai[2,2]*Ai[2,2]+Math.Sqrt(Ai11x4-4.0*Ai11x3*Ai[2,2]+4.0*Ai11x2*Ai[1,2]*Ai[1,2]+6.0*Ai11x2*Ai[2,2]*Ai[2,2]-8.0*Ai11*Ai[2,2]*Ai[1,2]*Ai[1,2]-4.0*Ai11*Ai[2,2]*Ai[2,2]*Ai[2,2]+4.0*Ai[2,2]*Ai[2,2]*Ai[1,2]*Ai[1,2]+Ai[2,2]*Ai[2,2]*Ai[2,2]*Ai[2,2]))/(Ai11x2-2.0*Ai11*Ai[2,2]+Ai[2,2]*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2]))*Math.Sqrt(1.0-(Ai11x2-2.0*Ai11*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2]+Ai[2,2]*Ai[2,2]+Math.Sqrt(Ai11x4-4.0*Ai11x3*Ai[2,2]+4.0*Ai11x2*Ai[1,2]*Ai[1,2]+6.0*Ai11x2*Ai[2,2]*Ai[2,2]-8.0*Ai11*Ai[2,2]*Ai[1,2]*Ai[1,2]-4.0*Ai11*Ai[2,2]*Ai[2,2]*Ai[2,2]+4.0*Ai[2,2]*Ai[2,2]*Ai[1,2]*Ai[1,2]+Ai[2,2]*Ai[2,2]*Ai[2,2]*Ai[2,2]))/(Ai11x2-2.0*Ai11*Ai[2,2]+Ai[2,2]*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2])/2.0)+Ai[2,2]*(1.0-(Ai11x2-2.0*Ai11*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2]+Ai[2,2]*Ai[2,2]+Math.Sqrt(Ai11x4-4.0*Ai11x3*Ai[2,2]+4.0*Ai11x2*Ai[1,2]*Ai[1,2]+6.0*Ai11x2*Ai[2,2]*Ai[2,2]-8.0*Ai11*Ai[2,2]*Ai[1,2]*Ai[1,2]-4.0*Ai11*Ai[2,2]*Ai[2,2]*Ai[2,2]+4.0*Ai[2,2]*Ai[2,2]*Ai[1,2]*Ai[1,2]+Ai[2,2]*Ai[2,2]*Ai[2,2]*Ai[2,2]))/(Ai11x2-2.0*Ai11*Ai[2,2]+Ai[2,2]*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2])/2.0);      
			s_min_max[1]= Var1+Var2;      
				
			Var1 = Ai11*(Ai11x2-2.0*Ai11*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2]+Ai[2,2]*Ai[2,2]-Math.Sqrt(Ai11x4-4.0*Ai11x3*Ai[2,2]+4.0*Ai11x2*Ai[1,2]*Ai[1,2]+6.0*Ai11x2*Ai[2,2]*Ai[2,2]-8.0*Ai11*Ai[2,2]*Ai[1,2]*Ai[1,2]-4.0*Ai11*Ai[2,2]*Ai[2,2]*Ai[2,2]+4.0*Ai[2,2]*Ai[2,2]*Ai[1,2]*Ai[1,2]+Ai[2,2]*Ai[2,2]*Ai[2,2]*Ai[2,2]))/(Ai11x2-2.0*Ai11*Ai[2,2]+Ai[2,2]*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2])/2.0;      
			Var2 = Ai[1,2]*Math.Sqrt(2.0)*Math.Sqrt((Ai11x2-2.0*Ai11*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2]+Ai[2,2]*Ai[2,2]-Math.Sqrt(Ai11x4-4.0*Ai11x3*Ai[2,2]+4.0*Ai11x2*Ai[1,2]*Ai[1,2]+6.0*Ai11x2*Ai[2,2]*Ai[2,2]-8.0*Ai11*Ai[2,2]*Ai[1,2]*Ai[1,2]-4.0*Ai11*Ai[2,2]*Ai[2,2]*Ai[2,2]+4.0*Ai[2,2]*Ai[2,2]*Ai[1,2]*Ai[1,2]+Ai[2,2]*Ai[2,2]*Ai[2,2]*Ai[2,2]))/(Ai11x2-2.0*Ai11*Ai[2,2]+Ai[2,2]*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2]))*Math.Sqrt(1.0-(Ai11x2-2.0*Ai11*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2]+Ai[2,2]*Ai[2,2]-Math.Sqrt(Ai11x4-4.0*Ai11x3*Ai[2,2]+4.0*Ai11x2*Ai[1,2]*Ai[1,2]+6.0*Ai11x2*Ai[2,2]*Ai[2,2]-8.0*Ai11*Ai[2,2]*Ai[1,2]*Ai[1,2]-4.0*Ai11*Ai[2,2]*Ai[2,2]*Ai[2,2]+4.0*Ai[2,2]*Ai[2,2]*Ai[1,2]*Ai[1,2]+Ai[2,2]*Ai[2,2]*Ai[2,2]*Ai[2,2]))/(Ai11x2-2.0*Ai11*Ai[2,2]+Ai[2,2]*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2])/2.0)+Ai[2,2]*(1.0-(Ai11x2-2.0*Ai11*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2]+Ai[2,2]*Ai[2,2]-Math.Sqrt(Ai11x4-4.0*Ai11x3*Ai[2,2]+4.0*Ai11x2*Ai[1,2]*Ai[1,2]+6.0*Ai11x2*Ai[2,2]*Ai[2,2]-8.0*Ai11*Ai[2,2]*Ai[1,2]*Ai[1,2]-4.0*Ai11*Ai[2,2]*Ai[2,2]*Ai[2,2]+4.0*Ai[2,2]*Ai[2,2]*Ai[1,2]*Ai[1,2]+Ai[2,2]*Ai[2,2]*Ai[2,2]*Ai[2,2]))/(Ai11x2-2.0*Ai11*Ai[2,2]+Ai[2,2]*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2])/2.0);      
			s_min_max[2]= Var1+Var2;      
				
			Var1 = Ai11*(Ai11x2-2.0*Ai11*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2]+Ai[2,2]*Ai[2,2]-Math.Sqrt(Ai11x4-4.0*Ai11x3*Ai[2,2]+4.0*Ai11x2*Ai[1,2]*Ai[1,2]+6.0*Ai11x2*Ai[2,2]*Ai[2,2]-8.0*Ai11*Ai[2,2]*Ai[1,2]*Ai[1,2]-4.0*Ai11*Ai[2,2]*Ai[2,2]*Ai[2,2]+4.0*Ai[2,2]*Ai[2,2]*Ai[1,2]*Ai[1,2]+Ai[2,2]*Ai[2,2]*Ai[2,2]*Ai[2,2]))/(Ai11x2-2.0*Ai11*Ai[2,2]+Ai[2,2]*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2])/2.0;      
			Var2 = -Ai[1,2]*Math.Sqrt(2.0)*Math.Sqrt((Ai11x2-2.0*Ai11*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2]+Ai[2,2]*Ai[2,2]-Math.Sqrt(Ai11x4-4.0*Ai11x3*Ai[2,2]+4.0*Ai11x2*Ai[1,2]*Ai[1,2]+6.0*Ai11x2*Ai[2,2]*Ai[2,2]-8.0*Ai11*Ai[2,2]*Ai[1,2]*Ai[1,2]-4.0*Ai11*Ai[2,2]*Ai[2,2]*Ai[2,2]+4.0*Ai[2,2]*Ai[2,2]*Ai[1,2]*Ai[1,2]+Ai[2,2]*Ai[2,2]*Ai[2,2]*Ai[2,2]))/(Ai11x2-2.0*Ai11*Ai[2,2]+Ai[2,2]*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2]))*Math.Sqrt(1.0-(Ai11x2-2.0*Ai11*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2]+Ai[2,2]*Ai[2,2]-Math.Sqrt(Ai11x4-4.0*Ai11x3*Ai[2,2]+4.0*Ai11x2*Ai[1,2]*Ai[1,2]+6.0*Ai11x2*Ai[2,2]*Ai[2,2]-8.0*Ai11*Ai[2,2]*Ai[1,2]*Ai[1,2]-4.0*Ai11*Ai[2,2]*Ai[2,2]*Ai[2,2]+4.0*Ai[2,2]*Ai[2,2]*Ai[1,2]*Ai[1,2]+Ai[2,2]*Ai[2,2]*Ai[2,2]*Ai[2,2]))/(Ai11x2-2.0*Ai11*Ai[2,2]+Ai[2,2]*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2])/2.0)+Ai[2,2]*(1.0-(Ai11x2-2.0*Ai11*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2]+Ai[2,2]*Ai[2,2]-Math.Sqrt(Ai11x4-4.0*Ai11x3*Ai[2,2]+4.0*Ai11x2*Ai[1,2]*Ai[1,2]+6.0*Ai11x2*Ai[2,2]*Ai[2,2]-8.0*Ai11*Ai[2,2]*Ai[1,2]*Ai[1,2]-4.0*Ai11*Ai[2,2]*Ai[2,2]*Ai[2,2]+4.0*Ai[2,2]*Ai[2,2]*Ai[1,2]*Ai[1,2]+Ai[2,2]*Ai[2,2]*Ai[2,2]*Ai[2,2]))/(Ai11x2-2.0*Ai11*Ai[2,2]+Ai[2,2]*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2])/2.0);      
			s_min_max[3]= Var1+Var2;

			T[0] = Math.Sqrt(2.0)*Math.Sqrt((Ai11x2-2.0*Ai11*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2]+Ai[2,2]*Ai[2,2]+Math.Sqrt((Ai11x2-2.0*Ai11*Ai[2,2]+Ai[2,2]*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2])*Math.Pow(Ai11-Ai[2,2],2.0)))/(Ai11x2-2.0*Ai11*Ai[2,2]+Ai[2,2]*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2]))/2.0;      
			T[1] = -Math.Sqrt(2.0)*Math.Sqrt((Ai11x2-2.0*Ai11*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2]+Ai[2,2]*Ai[2,2]+Math.Sqrt((Ai11x2-2.0*Ai11*Ai[2,2]+Ai[2,2]*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2])*Math.Pow(Ai11-Ai[2,2],2.0)))/(Ai11x2-2.0*Ai11*Ai[2,2]+Ai[2,2]*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2]))/2.0;
			T[2] = Math.Sqrt(2.0)*Math.Sqrt((Ai11x2-2.0*Ai11*Ai[2,2]+Ai[2,2]*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2]-Math.Sqrt((Ai11x2-2.0*Ai11*Ai[2,2]+Ai[2,2]*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2])*Math.Pow(Ai11-Ai[2,2],2.0)))/(Ai11x2-2.0*Ai11*Ai[2,2]+Ai[2,2]*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2]))/2.0;
			T[3] = -Math.Sqrt(2.0)*Math.Sqrt((Ai11x2-2.0*Ai11*Ai[2,2]+Ai[2,2]*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2]-Math.Sqrt((Ai11x2-2.0*Ai11*Ai[2,2]+Ai[2,2]*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2])*Math.Pow(Ai11-Ai[2,2],2.0)))/(Ai11x2-2.0*Ai11*Ai[2,2]+Ai[2,2]*Ai[2,2]+4.0*Ai[1,2]*Ai[1,2]))/2.0;

			int iMax = -1;
			int iMin = -1;
			double s_min_value = double.MaxValue;
			double s_max_value = double.MinValue;
			for (int i=0;i<4;i++)
			{
				if (s_min_max[i] > 0)
				{
					if (s_min_max[i] < s_min_value)
					{
						s_min_value = s_min_max[i];
						iMax = i;
					}
					if (s_min_max[i] > s_max_value)
					{
						s_max_value = s_min_max[i];
						iMin = i;
					}
				}
			}

			if (iMax < 0 || iMin < 0)
			{
				string message = "Invalid fitting data: Vector {";
				for (int i=0; i<_vector.Length; i++)
					message += _vector[i].ToString() + (i == _vector.Length-1 ? "" : ",");
				message += "}";
				throw new System.ExecutionEngineException(message);
			}

			double theta0 = Math.Acos(T[iMax]);
			double [,] U_f = new double[3,3];
			double L_max = Math.Sqrt(r1[1,1]/s_min_value);
			double L_min = Math.Sqrt(r1[1,1]/s_max_value);

			U_f[1,1] = T[iMax]*L_max;
			U_f[1,2] = T[iMin]*L_min;
			U_f[2,1] = Math.Sqrt(1-T[iMax]*T[iMax])*L_max;
			U_f[2,2] = Math.Sqrt(1-T[iMin]*T[iMin])*L_min;
			double [,] MajorMinor = new double [3,3];
			AperB(Ai,U_f,MajorMinor,2,2,2,2);

			double major_axis = Math.Sqrt(MajorMinor[1,1]*MajorMinor[1,1] + MajorMinor[2,1]*MajorMinor[2,1]) * 1.0F * scale_factor;
			double minor_axis = Math.Sqrt(MajorMinor[1,2]*MajorMinor[1,2] + MajorMinor[2,2]*MajorMinor[2,2]) * 1.0F * scale_factor;

			float x = (float)(-Aib[1,1]*0.5F * scale_factor);
			float y = (float)(-Aib[2,1]*0.5F * scale_factor);
			angle = (double)((theta0 * 180.0F) / Math.PI);			

			rcBound = new RectangleF(PointF.Empty, new SizeF((float)major_axis, (float)minor_axis));
			float offsetX = (float)(x-major_axis/2.0F);
			float offsetY = (float)(y-minor_axis/2.0F);
			rcBound.Offset(offsetX, offsetY);			
		}

		private void ExecuteAlgorithm()
		{
			int num_points = _points.Count;
			// initialize constrain matrix
			double[,] ConstraintMatrix = this.GenerateContrainMatrix();
			// initialize design matrix
			double[,] DesignMatrix = this.GenerateDesignMatrix(_points);

			// generate scatter matrix
			double[,] temp = new double[7,7];
			double[,] L = new double[7,7];
			double[,] InvL = new double[7,7];
			double[,] ScatterMatrix = new double[7,7];
			A_TperB(DesignMatrix, DesignMatrix, ScatterMatrix, num_points, 6, num_points, 6);
			Cholesky(ScatterMatrix, 6, L);
			Inverse(L, InvL, 6);

			AperB_T(ConstraintMatrix, InvL, temp, 6, 6, 6, 6);
			AperB(InvL, temp, ConstraintMatrix, 6, 6, 6, 6);

			double[,] V = new double[7,7];
			double[] d = new double[7];
			int nrot = 0;
			Jacobi(ConstraintMatrix, 6, d, V, nrot);

			double[,] SolutionMatrix = new double[7,7];
			A_TperB(InvL, V, SolutionMatrix, 6, 6, 6, 6);

			// normalize solution matrix
			for (int j=1; j<=6; j++)  /* Scan columns */
			{
				double mod = 0.0; 
				for (int i=1; i<=6; i++)
					mod += SolutionMatrix[i,j]*SolutionMatrix[i,j];
				for (int i=1; i<=6; i++)
					SolutionMatrix[i,j] /=  Math.Sqrt(mod);
			}

			// minimizing 
			double zero = 10e-20;
			double minev = 10e+20;
			int  solind=0;

			switch (_method)
			{
				case Methods.Bookstein:  // smallest eigenvalue
					for (int i=1; i<=6; i++)
						if (d[i]<minev && Math.Abs(d[i])>zero)
							solind = i;
					break;
				case Methods.FPF:
					for (int i=1; i<=6; i++)
						if (d[i]<0 && Math.Abs(d[i])>zero)
							solind = i;
					break;
				default:
					break;
			}

			// Now fetch the right solution
			double[] vector = new double[7];
			for (int j=1; j<=6; j++)
				vector[j] = SolutionMatrix[j, solind];
			_vector = vector;
		}

		private void AperB(double[,] A, double[,] B, double[,] res,
			int righA, int colA, int righB, int colB) 
		{
			int p,q,l;
			for (p=1;p<=righA;p++)
				for (q=1;q<=colB;q++)
				{ 
					res[p,q]=0.0;
					for (l=1;l<=colA;l++)
						res[p,q]=res[p,q]+A[p,l]*B[l,q];
				}
		}

		private void A_TperB(double[,] A, double[,]  B, double[,] res,
			int righA, int colA, int righB, int colB) 
		{
			int p,q,l;
			for (p=1;p<=colA;p++)
				for (q=1;q<=colB;q++)
				{
					res[p,q]=0.0;
					for (l=1;l<=righA;l++)
						res[p,q]=res[p,q]+A[l,p]*B[l,q];
				}
		}

		private void AperB_T(double[,] A, double[,] B, double[,] res,
			int righA, int colA, int righB, int colB) 
		{
			int p,q,l;
			for (p=1;p<=colA;p++)
				for (q=1;q<=colB;q++)
				{
					res[p,q]=0.0;
					for (l=1;l<=righA;l++)
						res[p,q]=res[p,q]+A[p,l]*B[q,l];
				}
		}


		private void Cholesky(double[,] a, int n, double[,] l)
		{
			int i,j,k;
			double sum;
			double []p = new double[n+1];

			for (i=1; i<=n; i++)  
			{
				for (j=i; j<=n; j++)  
				{
					for (sum=a[i,j],k=i-1;k>=1;k--) sum -= a[i,k]*a[j,k];
					if (i == j) 
					{
						if (sum<=0.0)
						{}
						else
							p[i]=Math.Sqrt(sum); 
					}
					else
					{
						a[j,i]=sum/p[i];
					}
				}
			}
			for (i=1; i<=n; i++)
				for (j=i; j<=n; j++)
					if (i==j)
						l[i,i] = p[i];
					else
					{
						l[j,i]=a[j,i];
						l[i,j]=0.0;
					}
		}

		private void Jacobi(double[,] a, int n, double []d , double[,] v, int nrot)
		{
			int j,iq,ip,i;
			double tresh,theta,tau,t,sm,s,h,g,c;

			double []b = new double[n+1];
			double []z = new double[n+1];

			for(ip=1;ip<=n;ip++) 
			{
				for (iq=1;iq<=n;iq++) v[ip,iq]=0.0;
				v[ip,ip]=1.0;
			}
			for (ip=1;ip<=n;ip++) 
			{
				b[ip]=d[ip]=a[ip,ip];
				z[ip]=0.0;
			}
			nrot=0;
			for (i=1;i<=50;i++) 
			{
				sm=0.0;
				for (ip=1;ip<=n-1;ip++) 
				{
					for (iq=ip+1;iq<=n;iq++)
						sm += Math.Abs(a[ip,iq]);
				}
				if (sm == 0.0) 
				{
					/*    free_vector(z,1,n);
						free_vector(b,1,n);  */
					return;
				}
				if (i < 4)
					tresh=0.2*sm/(n*n);
				else
					tresh=0.0;
				for (ip=1;ip<=n-1;ip++) 
				{
					for (iq=ip+1;iq<=n;iq++) 
					{
						g=100.0*Math.Abs(a[ip,iq]);
						if (i > 4 && Math.Abs(d[ip])+g == Math.Abs(d[ip])
							&& Math.Abs(d[iq])+g == Math.Abs(d[iq]))
							a[ip,iq]=0.0;
						else if (Math.Abs(a[ip,iq]) > tresh) 
						{
							h=d[iq]-d[ip];
							if (Math.Abs(h)+g == Math.Abs(h))
								t=(a[ip,iq])/h;
							else 
							{
								theta=0.5*h/(a[ip,iq]);
								t=1.0/(Math.Abs(theta)+Math.Sqrt(1.0+theta*theta));
								if (theta < 0.0) t = -t;
							}
							c=1.0/Math.Sqrt(1+t*t);
							s=t*c;
							tau=s/(1.0+c);
							h=t*a[ip,iq];
							z[ip] -= h;
							z[iq] += h;
							d[ip] -= h;
							d[iq] += h;
							a[ip,iq]=0.0;
							for (j=1;j<=ip-1;j++) 
							{
								Rotate(a,j,ip,j,iq,tau,s);
							}
							for (j=ip+1;j<=iq-1;j++) 
							{
								Rotate(a,ip,j,j,iq,tau,s);
							}
							for (j=iq+1;j<=n;j++) 
							{
								Rotate(a,ip,j,iq,j,tau,s);
							}
							for (j=1;j<=n;j++) 
							{
								Rotate(v,j,ip,j,iq,tau,s);
							}
							++nrot;
						}
					}
				}
				for (ip=1;ip<=n;ip++) 
				{
					b[ip] += z[ip];
					d[ip]=b[ip];
					z[ip]=0.0;
				}
			}
		}


		private int Inverse(double[,] TB, double[,] InvB, int N) 
		{
			int k,i,j,p,q;
			double mult;
			double D,temp;
			double maxpivot;
			int npivot;
			double[,] B = new double [N+1,N+2];
			double[,] A = new double [N+1,2*N+2];
			double[,] C = new double [N+1,N+1];
			double eps = 10e-20;


			for(k=1;k<=N;k++)
				for(j=1;j<=N;j++)
					B[k,j]=TB[k,j];

			for (k=1;k<=N;k++)
			{
				for (j=1;j<=N+1;j++)
					A[k,j]=B[k,j];
				for (j=N+2;j<=2*N+1;j++)
					A[k,j]=(float)0;
				A[k,k-1+N+2]=(float)1;
			}
			for (k=1;k<=N;k++)
			{
				maxpivot=Math.Abs((double)A[k,k]);
				npivot=k;
				for (i=k;i<=N;i++)
					if (maxpivot<Math.Abs((double)A[i,k]))
					{
						maxpivot=Math.Abs((double)A[i,k]);
						npivot=i;
					}
				if (maxpivot>=eps)
				{
					if (npivot!=k)
						for (j=k;j<=2*N+1;j++)
						{
							temp=A[npivot,j];
							A[npivot,j]=A[k,j];
							A[k,j]=temp;
						} ;
					D=A[k,k];
					for (j=2*N+1;j>=k;j--)
						A[k,j]=A[k,j]/D;
					for (i=1;i<=N;i++)
					{
						if (i!=k)
						{
							mult=A[i,k];
							for (j=2*N+1;j>=k;j--)
								A[i,j]=A[i,j]-mult*A[k,j] ;
						}
					}
				}
				else
				{ 
					return(-1);
				};
			}
			/**   Copia il risultato nella matrice InvB  ***/
			for (k=1,p=1;k<=N;k++,p++)
				for (j=N+2,q=1;j<=2*N+1;j++,q++)
					InvB[p,q]=A[k,j];
			return 0;
		}            

		private void Rotate(double[,] a, int i, int j, int k, int l, double tau, double s)
		{
			double g,h;
			g=a[i,j];h=a[k,l];a[i,j]=g-s*(h+g*tau);
			a[k,l]=h+s*(g-h*tau);
		}
	}
}
