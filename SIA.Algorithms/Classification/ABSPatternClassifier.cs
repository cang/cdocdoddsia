using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SDD = System.Drawing.Drawing2D;

using SiGlaz.Common;
using SIA.Algorithms.Preprocessing.Interpolation;

namespace SIA.Algorithms.Classification
{
    public class ABSPatternClassifier
    {
        private List<LineClassifyingInput> _lineClassifyingInput = null;

        public List<LineClassifyingInput> LineClassifyingInput
        {
            get { return _lineClassifyingInput; }
        }

        public ABSPatternClassifier()
        {
        }

        public ABSPatternClassifier(LinePatternLibrary library)
        {
            Init(library);
        }

        private unsafe SDD.Matrix[] CreatePattern(ushort* data, int width, int height, 
            List<LineClassifyingInput> inputs)
        {
            SDD.Matrix[] transformMats = new SDD.Matrix[inputs.Count];
            //int index = -1;
            //foreach (LineClassifyingInput lci in inputs)
            for (int i = 0; i < inputs.Count; i++ )
            {
                LineClassifyingInput lci = inputs[i];

                double rotation = Math.Atan2(lci.RealEndY - lci.RealBeginY, lci.RealEndX - lci.RealBeginX);
                double halfWidth = lci.Thickness / 2;
                double xTopLeft = 0;
                double yTopLeft = 0;
                double xTopRight = 0;
                double yTopRight = 0;
                double xBottomLeft = 0;
                double yBottomLeft = 0;

                GetCoordinates(lci.RealBeginX, lci.RealBeginY, lci.RealEndX, lci.RealEndY, halfWidth, ref xTopLeft, ref yTopLeft, ref xBottomLeft, ref yBottomLeft, ref xTopRight, ref yTopRight);

                //Console.WriteLine("{4}-----({0}, {1} --> ({2}, {3})", xTopLeft, yTopLeft, xTopRight, yBottomLeft, i);

                int roiWidth = (int)lci.Length;
                int roiHeight = (int)(lci.Thickness);

                //index++;
                transformMats[i] = new SDD.Matrix(new RectangleF(0, 0, roiWidth, roiHeight),
                    new PointF[] {
                                new PointF((float)xTopLeft, (float)yTopLeft),
                                new PointF((float)xTopRight, (float)yTopRight),
                                new PointF((float)xBottomLeft, (float)yBottomLeft)
                                });
                lci.Pattern = new ushort[roiWidth * roiHeight];
                fixed (ushort* proiData = lci.Pattern)
                {
                    SIA.Algorithms.Preprocessing.Interpolation.ImageInterpolator.AffineTransform(InterpolationMethod.Bilinear,
                        data, width, height, transformMats[i], proiData, roiWidth, roiHeight);
                }
            }
            return transformMats;
        }

        private void GetCoordinates(double xa, double ya, double xb, double yb, double distance,
            ref double xtop, ref double ytop, ref double xbottom, ref double ybottom, ref double xright, ref double yright)
        {
            double a = -1 / (Math.Sqrt(yb * yb - 2.0 * yb * ya + ya * ya + xb * xb - 2.0 * xb * xa + xa * xa)) * (-yb + ya);
            double b = 1 / (Math.Sqrt(yb * yb - 2.0 * yb * ya + ya * ya + xb * xb - 2.0 * xb * xa + xa * xa)) * (-xb + xa);
            double c = -1 / (Math.Sqrt(yb * yb - 2.0 * yb * ya + ya * ya + xb * xb - 2.0 * xb * xa + xa * xa)) * xa * yb + 1 / 
                (Math.Sqrt(yb * yb - 2.0 * yb * ya + ya * ya + xb * xb - 2.0 * xb * xa + xa * xa)) * ya * xb;

            double x1 = -(-b * b * xa + b * a * ya + c * a + distance * a);
            double y1 = (-b * c - a * b * xa + a * a * ya - b * distance);

            double x2 = -(-b * b * xa + b * a * ya + c * a - distance * a);
            double y2 = (-b * c - a * b * xa + a * a * ya + b * distance);

            double x3 = -(-b * b * xb + b * a * yb + c * a - distance * a);
            double y3 = (-b * c - a * b * xb + a * a * yb + b * distance);

            //double x4 = -(-b * b * xb + b * a * yb + c * a + distance * a);
            //double y4 = (-b * c - a * b * xb + a * a * yb - b * distance);

            xtop = x2;
            ytop = y2;
            xbottom = x1;
            ybottom = y1;
            xright = x3;
            yright = y3;
        }

        public unsafe void GetLineClassifyingInput(ushort* data, int width, int height,
            List<LineClassifyingInput> inputs)
        {
            CreatePattern(data, width, height, inputs);
        }

        public void Init(LinePatternLibrary library)
        {
            _lineClassifyingInput = CreateInput(library);
        }

        public List<LineClassifyingInput> CreateInput(LinePatternLibrary library)
        {
            int numData = library.Begins.Length;
            List<LineClassifyingInput> lineClassifyingInput = new List<LineClassifyingInput>();

            for (int i = 0; i < numData; i++)
            {
                LineClassifyingInput line = new LineClassifyingInput();
                line.RealBeginX = library.Begins[i].X;
                line.RealBeginY = library.Begins[i].Y;
                line.RealEndX = library.Ends[i].X;
                line.RealEndY = library.Ends[i].Y;
                line.Thickness = library.Width;
                float deltaX = library.Begins[i].X - library.Ends[i].X;
                float deltaY = library.Begins[i].Y - library.Ends[i].Y;
                line.Length = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
                lineClassifyingInput.Add(line);
            }

            return lineClassifyingInput;
        }

        public unsafe List<RectangleF> Classify(ushort* data, int width, int height)
        {
            SDD.Matrix[] transformMats = CreatePattern(data, width, height, _lineClassifyingInput);
            
            LineClassifer lineClassifer = new LineClassifer();
            lineClassifer.Classify(_lineClassifyingInput);

            List<RectangleF> listRectResult = new List<RectangleF>();
            int index = -1;
            foreach (LineClassifyingInput lci in _lineClassifyingInput)
            {                    
                index++;
                if (lci.DetectedPosition == null)
                    continue;

                for (int i = 0; i < lci.DetectedPosition.Count;i++ )
                {
                    RectangleF rect = lci.DetectedPosition[i];
                    PointF[] pts = new PointF[] { new PointF(rect.X + rect.Width * 0.5f, rect.Y + rect.Height * 0.5f) };
                    transformMats[index].TransformPoints(pts);

                    float radius = (float)(Math.Sqrt(rect.Width * rect.Width + rect.Height * rect.Height) / 2);

                    lci.DetectedPosition[i] = new RectangleF(pts[0].X - radius, pts[0].Y - radius, 2 * radius, 2 * radius);
                }
                listRectResult.AddRange(lci.DetectedPosition);
            }

            return listRectResult;
        }
    }
}
