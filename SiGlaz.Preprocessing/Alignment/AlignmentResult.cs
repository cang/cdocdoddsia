using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;


namespace SiGlaz.Preprocessing.Alignment
{
    public class AlignmentResult
    {
        public double Angle;
        public double CenterX;
        public double CenterY;
        public PointF[] SourceCoordinates;

        public AlignmentResult(double centerX, double centerY, double angle)
        {
            Angle = angle;
            CenterX = centerX;
            CenterY = centerY;
        }
    }
}
