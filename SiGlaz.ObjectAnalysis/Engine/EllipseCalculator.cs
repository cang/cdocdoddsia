using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using SiGlaz.Algorithms.Core;
using SiGlaz.Common.Object;


namespace SiGlaz.ObjectAnalysis.Engine
{
    public class EllipseCalculator : IDisposable
    {
        private int N = 20;
        private float[] thetas;
        Matrix eMatrix1 = new Matrix();
        Matrix eMatrix2 = new Matrix();
        PointF[] eP1;
        PointF[] eP2;

        public EllipseCalculator(int n)
        {
            N = n;
            Init();
        }
        public EllipseCalculator()
        {
            Init();
        }
        private void Init()
        {
            eP1 = new PointF[N + 1];
            eP2 = new PointF[N + 1];
            for (int i = 0; i <= N; i++)
            {
                eP1[i] = PointF.Empty;
                eP2[i] = PointF.Empty;
            }

            thetas = new float[N];
            for (int i = 0; i < N; i++)
                thetas[i] = (float)(Math.PI * i * 2 / N);
        }
        private float Distance2PointsR(float x1, float y1, float x2, float y2)
        {
            return ((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }
        private float Distance2Lines(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {
            float dx, dy, c;

            dx = x2 - x1;
            dy = y2 - y1;
            c = y2 * x1 - y1 * x2;
            if ((y3 * dx - x3 * dy + c) * (y4 * dx - x4 * dy + c) <= 0)
            {
                dx = x4 - x3;
                dy = y4 - y3;
                c = y4 * x3 - y3 * x4;
                if ((y1 * dx - x1 * dy + c) * (y2 * dx - x2 * dy + c) <= 0)
                    return 0f;
            }

            float distance = Distance2PointsR(x1, y1, x3, y3);
            distance = Math.Min(distance, Distance2PointsR(x1, y1, x4, y4));
            distance = Math.Min(distance, Distance2PointsR(x2, y2, x3, y3));
            distance = Math.Min(distance, Distance2PointsR(x2, y2, x4, y4));
            return (float)Math.Sqrt(distance);
        }
        public double Distance2Ellipse(
            EllipticalDensityShapeObject p1, EllipticalDensityShapeObject p2)
        {
            eMatrix1.Reset();
            eMatrix1.Rotate(p1.Orientation, MatrixOrder.Append);
            eMatrix1.Translate((float)p1.CenterX, (float)p1.CenterY, MatrixOrder.Append);

            eMatrix2.Reset();
            eMatrix2.Rotate(p2.Orientation, MatrixOrder.Append);
            eMatrix2.Translate((float)p2.CenterX, (float)p2.CenterY, MatrixOrder.Append);

            for (int i = 0; i < N; i++)
            {
                eP1[i].X = (float)Math.Cos(thetas[i]) * p1.Length * 0.5f;
                eP1[i].Y = (float)(Math.Sin(thetas[i]) * p1.MinorLength * 0.5);
                eP2[i].X = (float)Math.Cos(thetas[i]) * p2.Length * 0.5f;
                eP2[i].Y = (float)(Math.Sin(thetas[i]) * p2.MinorLength * 0.5);
            }
            eMatrix1.TransformPoints(eP1);
            eMatrix2.TransformPoints(eP2);

            eP1[N] = eP1[0];
            eP2[N] = eP2[0];

            float distance = float.MaxValue;
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                {
                    distance = Math.Min(distance, Distance2Lines(
                        eP1[i].X, eP1[i].Y, eP1[i + 1].X, eP1[i + 1].Y,
                        eP2[j].X, eP2[j].Y, eP2[j + 1].X, eP2[j + 1].Y));
                    if (distance <= 0f)
                        return distance;
                }
            return distance;

        }
        #region IDisposable Members

        public void Dispose()
        {
            if (thetas != null)
            {
                Array.Clear(thetas, 0, thetas.Length);
                thetas = null;
            }
            if (eP1 != null)
            {
                Array.Clear(eP1, 0, eP1.Length);
                eP1 = null;
            }
            if (eP2 != null)
            {
                Array.Clear(eP2, 0, eP2.Length);
                eP2 = null;
            }
            if (eMatrix1 != null)
            {
                eMatrix1.Dispose();
                eMatrix1 = null;
            }
            if (eMatrix2 != null)
            {
                eMatrix2.Dispose();
                eMatrix2 = null;
            }
        }

        #endregion
    }

}
