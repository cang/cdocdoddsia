using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;


namespace SIA.Algorithms.Preprocessing.Alignment
{
    public class AlignmentResult
    {
        public double Angle;
        public double CenterX;
        public double CenterY;
        public PointF[] SourceCoordinates;

        public int NewWidth = 0;
        public int NewHeight = 0;

        public double[] MatchedXList = null;
        public double[] MatchedYList = null;
        public double[] MatchedConfidences = null;
        public double HintThresholdConfidence = 0.6;        

        public AlignmentResult(double centerX, double centerY, double angle)
        {
            Angle = angle;
            CenterX = centerX;
            CenterY = centerY;
        }

        public float GetRotateAngle(float width, float height)
        {
            float angle = 0;
            using (System.Drawing.Drawing2D.Matrix m = new System.Drawing.Drawing2D.Matrix(
                    new RectangleF(0, 0, width, height), SourceCoordinates))
            {
                //using (MyMatrix myMatrix = new MyMatrix(m))
                MyMatrix myMatrix = new MyMatrix(m);
                {
                    angle = (float)myMatrix.GetRotation();
                }
            }

            return angle;
        }

        public float GetLeft(float width, float height)
        {
            float left = 0;
            using (System.Drawing.Drawing2D.Matrix m = new System.Drawing.Drawing2D.Matrix(
                    new RectangleF(0, 0, width, height), SourceCoordinates))
            {
                left = m.OffsetX;
            }

            return left;
        }

        public float GetTop(float width, float height)
        {
            float top = 0;
            using (System.Drawing.Drawing2D.Matrix m = new System.Drawing.Drawing2D.Matrix(
                new RectangleF(0, 0, width, height), SourceCoordinates))
            {
                top = m.OffsetY;
            }

            return top;
        }

        public System.Drawing.Drawing2D.Matrix GetDeviceToImageTransformer()
        {
            float deviceLeft = this.GetLeft(NewWidth, NewHeight);
            float deviceTop = this.GetTop(NewWidth, NewHeight);
            float angle = this.GetRotateAngle(NewWidth, NewHeight);

            System.Drawing.Drawing2D.Matrix transformer = new System.Drawing.Drawing2D.Matrix();
            transformer.Translate(deviceLeft, deviceTop);
            transformer.Rotate(-angle);            

            return transformer;
        }
    }
}
