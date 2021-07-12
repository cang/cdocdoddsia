using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using SIA.Common;
using SIA.Common.Analysis;
using SIA.Common.KlarfExport;
using SIA.Common.Mathematics;
using SIA.Common.Utility;

namespace SIA.UI.Components.Helpers
{
    /// <summary>
    /// Transformer class encapsulates transformation matrix
    /// </summary>
	public class Transformer 
        : IDisposable
    {
        private bool _autoDisposeMatrix = false;
        private Matrix _matrix;

        public Matrix Transform
        {
            get
            {
                return _matrix;
            }
        }

        public bool AutoDisposeTransform
        {
            get { return _autoDisposeMatrix; }
            set { _autoDisposeMatrix = value; }
        }

        public Transformer()
        {
            _matrix = new Matrix();
            _autoDisposeMatrix = true;
        }

        public Transformer(Matrix transform)
        {
            _matrix = transform;
            _autoDisposeMatrix = false;
        }

        public Transformer(Matrix transform, bool autoDispose)
        {
            _matrix = transform;
            _autoDisposeMatrix = autoDispose;
        }

        ~Transformer()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_matrix != null && _autoDisposeMatrix)
                _matrix.Dispose();
            _matrix = null;
        }

        protected virtual void OnSetTransform(Matrix matrix, bool autoDispose)
        {
            if (_matrix != null && _autoDisposeMatrix)
                _matrix.Dispose();

            _matrix = matrix;
            _autoDisposeMatrix = autoDispose;
        }

		public float LengthToLogical(float length)
		{
			if (this._matrix != null)
			{
				using (Matrix matrix = this._matrix.Clone())
				{
					matrix.Invert();
					return TransformLength(length, matrix);
				}
			}
			return length;
		}
 
		public float LengthToPhysical(float length)
		{
			if (this._matrix != null)
				return TransformLength(length, this._matrix);

			return length;
		}
 
		public PointF[] PointToLogical(PointF[] pts)
		{
			if (this._matrix != null && this._matrix.IsInvertible)
			{
				using (Matrix matrix = this._matrix.Clone())
				{
					matrix.Invert();
					return TransformPoints(pts, matrix);
				}
			}
			return pts;
		}

		public PointF[] PointToLogical(Point[] pts)
		{
			PointF[] input = new PointF[pts.Length];
			for (int i=0; i<pts.Length; i++)
				input[i] = pts[i];
			return PointToLogical(input);
		}

		public PointF PointToLogical(PointF pt)
		{
			PointF[] input = new PointF[1] { pt } ;
			PointF[] output = this.PointToLogical(input);
			return output[0];
		}
 
		public PointF PointToPhysical(PointF pt)
		{
			PointF[] input = new PointF[1] { pt } ;
			PointF[] output = this.PointToPhysical(input);
			return output[0]; 
		}
 
		public PointF[] PointToPhysical(PointF[] pts)
		{
			if (this._matrix != null)
				return TransformPoints(pts, this._matrix);
			return pts;
		}

		public PointF[] PointToPhysical(Point[] pts)
		{
			PointF[] input = new PointF[pts.Length];
			for (int i=0; i<pts.Length; i++)
				input[i] = pts[i];
			return PointToPhysical(input);
		}
 
		public RectangleF RectangleToLogical(RectangleF rc)
		{
			if (this._matrix != null)
			{
				using (Matrix matrix = this._matrix.Clone())
				{
					matrix.Invert();
					return TransformRectangle(rc, matrix);
				}
			}
			return rc;
		}
 
		public RectangleF RectangleToPhysical(RectangleF rc)
		{
			if (this._matrix != null)
			{
				return TransformRectangle(rc, this._matrix);
			}
			return rc;
		}
 
		
		public static float TransformLength(float length, Matrix matrix)
		{
			int sign = Math.Sign(length);
			PointF[] input = new PointF[2] { PointF.Empty, new PointF(length, 0f) } ;
			PointF[] output = input;
			matrix.TransformVectors(output);
			float dx = output[1].X - output[0].X;
			float dy = output[1].Y - output[0].Y;
			return (((float) Math.Sqrt((double) ((dx * dx) + (dy * dy)))) * sign);
		}

		public static PointF TransformPoint(PointF pt, Matrix matrix)
		{
			if (matrix == null)
				throw new ArgumentNullException("matrix");

			float xPos = matrix.Elements[0]*pt.X + matrix.Elements[1]*pt.Y + matrix.Elements[4];
			float yPos = matrix.Elements[2]*pt.X + matrix.Elements[3]*pt.Y + matrix.Elements[5];
			
			return new PointF(xPos, yPos);
		}
 
		public static PointF[] TransformPoints(PointF[] pts, Matrix matrix)
		{
			PointF[] output = pts.Clone() as PointF[];
			matrix.TransformPoints(output);
			return output;
		}
 
		public static RectangleF TransformRectangle(RectangleF rc, Matrix matrix)
		{
			PointF[] input = new PointF[2] { new PointF(rc.Left, rc.Top), new PointF(rc.Right, rc.Bottom) } ;
			PointF[] output = input;
			matrix.TransformPoints(output);
			return RectangleF.FromLTRB(Math.Min(output[0].X, output[1].X), Math.Min(output[0].Y, output[1].Y), 
				Math.Max(output[0].X, output[1].X), Math.Max(output[0].Y, output[1].Y));
		}

		public static PointF TransformPoint(PointF pt, float m11, float m12, float m21, float m22, float dx, float dy)
		{
			float xPos = m11*pt.X + m12*pt.Y + dx;
			float yPos = m21*pt.X + m22*pt.Y + dy;
			return new PointF(xPos, yPos);
		}

		public static PointF Rotate(PointF pt, float angle)
		{
			return RotateAt(pt, PointF.Empty, angle);
		}
		
		public static PointF RotateAt(PointF pt, PointF center, float angle)
		{
			using (Matrix matrix = new Matrix())
			{
				matrix.RotateAt(angle, center, MatrixOrder.Append);
				PointF[] pts = new PointF[] {pt};
				matrix.TransformPoints(pts);
				return pts[0];
			}
        }

        #region Matrix wrapper class
        
        public void Scale(float scaleDx, float scaleDy, MatrixOrder matrixOrder)
        {
            this._matrix.Scale(scaleDx, scaleDy, matrixOrder);
        }

        #endregion
    } 


	
}

