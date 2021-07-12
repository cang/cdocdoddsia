using System;
using System.Collections.Generic;
using System.Text;
using SIA.Common.Analysis;
using System.Drawing;

namespace SiGlaz.Common.Object
{
    /// <summary>
    /// Need to review
    /// </summary>
    public class DetectedObjectEx : DetectedObject
    {
        public string SignatureName = "SuprerObject";
        public List<DetectedObject> PrimitiveObjects = 
            new List<DetectedObject>();

        public DetectedObjectEx(
            List<DetectedObject> primitiveObjects)
        {
            PrimitiveObjects = primitiveObjects;

            Merge();
        }

        protected void Merge()
        {
            try
            {
                List<DetectedObject> objList = PrimitiveObjects;
                int nObjs = objList.Count;

                double area = 0;
                double perimeter = 0;
                double numberPixels = 0;
                double totalIntensity = 0;
                PointF gravity = PointF.Empty;

                // rect bound
                double l = double.MaxValue;
                double t = double.MaxValue;
                double r = double.MinValue;
                double b = double.MinValue;

                foreach (DetectedObject obj in objList)
                {
                    area += obj.Area;
                    perimeter += obj.Perimeter;
                    numberPixels += obj.NumPixels;
                    totalIntensity += obj.TotalIntensity;
                    gravity.X += obj.Gravity.X;
                    gravity.Y += obj.Gravity.Y;
                    // rect bound
                    l = Math.Min(l, obj.RectBound.Left);
                    t = Math.Min(t, obj.RectBound.Top);
                    r = Math.Max(r, obj.RectBound.Right);
                    b = Math.Max(b, obj.RectBound.Bottom);                    
                }

                // update detected object properties
                this.Area = area;
                this.Perimeter = perimeter;
                this.NumPixels = numberPixels;
                this.TotalIntensity = totalIntensity;
                this.Gravity = new PointF(gravity.X / nObjs, gravity.Y / nObjs);
                this.RectBound = RectangleF.FromLTRB((float)l, (float)t, (float)r, (float)b);

                this.ObjectTypeId =
                    (int)(SiGlaz.Common.ABSDefinitions.eDefectType.SuperObject);
            }
            catch
            {
                throw;
            }
        }

        public ContourCollection Contours
        {
            get { throw new System.NotFiniteNumberException("not support!"); }
            set { throw new System.NotFiniteNumberException("not support!"); }
        }

        public new PolygonExData PolygonBoundary
        {
            get { throw new System.NotFiniteNumberException("not support!"); }
            set { throw new System.NotFiniteNumberException("not support!"); }
        }
    }
}
