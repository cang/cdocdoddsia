using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SiGlaz.Preprocessing
{
	/// <summary>
	/// Summary description for MyMatrix.
	/// </summary>
	public class MyMatrix
	{
		private Matrix _intMatrix = new Matrix();
		public Matrix OwnMatrix
		{
			get
			{
				return _intMatrix;
			}
		}

		private float _m11;
		private float _m12;
		private float _m21;
		private float _m22;
		private float _dx;
		private float _dy;

        public MyMatrix(Matrix matrix)
        {
            _intMatrix = matrix;
            CopyMatrixToInternal();
        }

		public MyMatrix()
		{		
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

	}
}
