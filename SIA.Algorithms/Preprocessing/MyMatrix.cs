using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SIA.Algorithms.Preprocessing
{
	/// <summary>
	/// Summary description for MyMatrix.
	/// </summary>
	public class MyMatrix : IDisposable
	{
        public Matrix _intMatrix = new Matrix();
		public Matrix OwnMatrix
		{
			get
			{
				return _intMatrix;
			}
		}

		public float _m11;
        public float _m12;
        public float _m21;
        public float _m22;
        public float _dx;
        public float _dy;

        public MyMatrix(Matrix matrix)
        {
            _intMatrix = matrix.Clone();
            CopyMatrixToInternal();
        }

        public void Dispose()
        {
            _intMatrix.Dispose();
        }

		public MyMatrix()
		{		
			CopyMatrixToInternal();
		}

        public void Reset()
        {
            _intMatrix.Reset();
            CopyMatrixToInternal();
        }

		public void Rotate(float angle)
		{
			_intMatrix.Rotate(angle);
			CopyMatrixToInternal();
		}

		public void Rotate(float angle, MatrixOrder order)
		{
			_intMatrix.Rotate(angle, order);
			CopyMatrixToInternal();
		}

		public void Translate(float offsetX, float offsetY)
		{
			_intMatrix.Translate(offsetX, offsetY);
			CopyMatrixToInternal();
		}

		public void Translate(float offsetX, float offsetY, MatrixOrder order)
		{
			_intMatrix.Translate(offsetX, offsetY, order);
			CopyMatrixToInternal();
		}

		public void RotateAt(float angle, PointF point)
		{
			_intMatrix.RotateAt(angle, point);
			CopyMatrixToInternal();
		}

		public void RotateAt(float angle, PointF point, MatrixOrder order)
		{
			_intMatrix.RotateAt(angle, point, order);
			CopyMatrixToInternal();
		}

		public void Scale(float scaleX, float scaleY)
		{
			_intMatrix.Scale(scaleX, scaleY);
			CopyMatrixToInternal();
		}

		public void Scale(float scaleX, float scaleY, MatrixOrder order)
		{
			_intMatrix.Scale(scaleX, scaleY, order);
			CopyMatrixToInternal();
		}

		public void Invert()
		{
			_intMatrix.Invert();
			CopyMatrixToInternal();
		}

		public MyMatrix Clone()
		{
			MyMatrix result = this.MemberwiseClone() as MyMatrix;
			result._intMatrix = this._intMatrix.Clone() as Matrix;

			return result;
		}

		private void CopyMatrixToInternal()
		{
			_m11 = _intMatrix.Elements[0];
			_m12 = _intMatrix.Elements[1];
			_m21 = _intMatrix.Elements[2];
			_m22 = _intMatrix.Elements[3];
			_dx = _intMatrix.Elements[4];
			_dy = _intMatrix.Elements[5];
		}

		public void TransformPoints(ref float x, ref float y)
		{
			float oldx = x;
			float oldy = y;
			x = _m11 * oldx + _m21 * oldy + _dx;
			y = _m12 * oldx + _m22 * oldy + _dy;
		}

		public void TransformPoints(ref int x, ref int y)
		{
			float oldx = x;
			float oldy = y;
			x = (int)Math.Round(_m11 * oldx + _m21 * oldy + _dx);
			y = (int)Math.Round(_m12 * oldx + _m22 * oldy + _dy);
		}        

		public void TransformPoints(float[] x, float[] y)
		{
			if (x == null || y == null)
				return;

			for (int i = x.Length - 1; i >= 0; i--)
			{
				float oldx = x[i];
				float oldy = y[i];
				x[i] = _m11 * oldx + _m21 * oldy + _dx;
				y[i] = _m12 * oldx + _m22 * oldy + _dy;
			}
		}

        public void TransformPoints(double[] x, double[] y)
        {
            if (x == null || y == null)
                return;

            for (int i = x.Length - 1; i >= 0; i--)
            {
                double oldx = x[i];
                double oldy = y[i];
                x[i] = _m11 * oldx + _m21 * oldy + _dx;
                y[i] = _m12 * oldx + _m22 * oldy + _dy;
            }
        }

		public void TransformPoints(float[] x, float[] y, float[] newx, float[] newy)
		{
			if (x == null || y == null)
				return;

			for (int i = x.Length - 1; i >= 0; i--)
			{
				float oldx = x[i];
				float oldy = y[i];
				newx[i] = _m11 * oldx + _m21 * oldy + _dx;
				newy[i] = _m12 * oldx + _m22 * oldy + _dy;
			}
		}

        public void TransformPoints(ref Point pt)
        {
            float oldx = pt.X;
            float oldy = pt.Y;

            pt.X = (int)Math.Round(_m11 * oldx + _m21 * oldy + _dx);
            pt.Y = (int)Math.Round(_m12 * oldx + _m22 * oldy + _dy);
        }

        public void TransformPoints(ref PointF ptf)
        {
            float oldx = ptf.X;
            float oldy = ptf.Y;

            ptf.X = (_m11 * oldx + _m21 * oldy + _dx);
            ptf.Y = (_m12 * oldx + _m22 * oldy + _dy);
        }

        public void TransformPoints(Point[] pts)
        {
            float oldx, oldy;
            for (int i = pts.Length - 1; i >= 0; i--)
            {
                oldx = pts[i].X;
                oldy = pts[i].Y;

                pts[i].X = (int)Math.Round(_m11 * oldx + _m21 * oldy + _dx);
                pts[i].Y = (int)Math.Round(_m12 * oldx + _m22 * oldy + _dy);
            }
        }

        public void TransformPoints(PointF[] pts)
        {
            float oldx, oldy;
            for (int i = pts.Length - 1; i >= 0; i--)
            {
                oldx = pts[i].X;
                oldy = pts[i].Y;

                pts[i].X = (_m11 * oldx + _m21 * oldy + _dx);
                pts[i].Y = (_m12 * oldx + _m22 * oldy + _dy);
            }
        }

        public float GetRotation()
        {
            PointF[] pts = new PointF[] { PointF.Empty, new PointF(1, 0) };
            Matrix mat = new Matrix();
            mat.Multiply(_intMatrix);
            mat.Translate(-_intMatrix.OffsetX, -_intMatrix.OffsetY);
            mat.TransformPoints(pts);
            mat.Dispose();
            return -(float)(Math.Atan2(pts[1].Y - pts[0].Y, pts[1].X - pts[0].X) * 180.0 / Math.PI);
        }

        public void GetScale(out float scaleX, out float scaleY)
        {
            float rotation = (float)GetRotation();
            PointF[] pts = new PointF[] { PointF.Empty, new PointF(1, 0) };
            Matrix mat = new Matrix();
            mat.Rotate(rotation);
            mat.Multiply(_intMatrix);
            mat.TransformPoints(pts);
            mat.Dispose();
            scaleX = pts[1].X - pts[0].X;
            scaleY = pts[1].Y - pts[0].Y;
        }

        public void GetShear(out float shearX, out float shearY)
        {
            float rotation = (float)GetRotation();
            PointF[] pts = new PointF[] { PointF.Empty, new PointF(1, 0) };
            Matrix mat = new Matrix();
            mat.Rotate(rotation);
            mat.Multiply(_intMatrix);
            mat.TransformPoints(pts);
            mat.Dispose();
            shearX = pts[1].X - 1.0f;
            shearY = pts[1].Y - 1.0f;    
        }
	}
}
