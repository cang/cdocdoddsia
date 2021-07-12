using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SIA.SystemFrameworks.UI;
using SIA.SystemLayer;
using System.Collections;
using System.Drawing.Drawing2D;
using SIA.Common.Analysis;
using System.Diagnostics;
using SiGlaz.Common.ABSDefinitions;

namespace SIA.UI.Controls.Commands
{
    public class OverlayDefectOnABSCommand : RasterCommand
    {
        private Image _image = null;

        public OverlayDefectOnABSCommand(IProgressCallback callback)
            : base(callback)
		{
		}

        public override object[] GetOutput()
        {
            return new object[] { _image };
        }

        public override bool CanAbort
        {
            get { return false; }
        }

        protected override void ValidateArguments(params object[] args)
        {
            if (args == null)
                throw new ArgumentNullException("arguments");

            if (args[0] is CommonImage == false)
                throw new ArgumentException("Argument type does not match. Arguments[0] must be CommonImage", "arguments");
        }

        protected override void OnRun(params object[] args)
        {
            CommonImage image = args[0] as CommonImage;
            ArrayList defectList = args[1] as ArrayList;

            this.SetStatusText("Overalying defect on ABS...");
            _image = OverlayDefectOnImage(image, defectList);
        }

        public static Image OverlayDefectOnImage(CommonImage image, ArrayList defectList)
        {
            Image overlaidImage = image.CreateBitmap();

            double thresholdContamination = 95;
            double thresholdScratch = 200;
            double val = 0;
            int iDefectType = 0;

            Color[] colors = DefectVisualizer.Colors;
                //new Color[] { Color.Pink, Color.Red, Color.Green, Color.Blue, Color.Yellow };
            Pen[] polyPens = new Pen[colors.Length];
            Pen[] rectPens = new Pen[colors.Length];
            for (int i = 0; i < colors.Length; i++)
            {
                polyPens[i] = new Pen(colors[i], 1.0f);
                rectPens[i] = new Pen(colors[i], 2.0f);
            }

            using (Graphics grph = Graphics.FromImage(overlaidImage))
            //using (Brush brush = new SolidBrush(Color.FromArgb(100, Color.Green)))
            //using (Pen polyPen = new Pen(Color.Green, 1.0f))
            //using (Pen rectPen = new Pen(Color.Red, 2.0f))
            //using (Pen polyPen2 = new Pen(Color.Blue, 1.0f))
            //using (Pen rectPen2 = new Pen(Color.Yellow, 2.0f))
            {                
                foreach (DetectedObject defect in defectList)
                {
                    int iPen = defect.ObjectTypeId + 1;
                    if (iPen < 0) iPen = 0;
                    else if (iPen >= colors.Length) iPen = colors.Length - 1;

                    Pen p1 = polyPens[iPen];
                    Pen p2 = rectPens[iPen];

                    using (GraphicsPath gp = ObjectToGraphicsPath(defect))
                    {
                        //grph.FillPath(brush, gp);

                        grph.DrawPath(p1, gp);
                    }

                    float left = defect.RectBound.Left - 5;
                    float top = defect.RectBound.Top - 5;
                    float width = defect.RectBound.Width + 10;
                    float height = defect.RectBound.Height + 10;

                    grph.DrawRectangle(p2, left, top, width, height);
                }
            }

            for (int i = 0; i < colors.Length; i++)
            {
                if (polyPens[i] != null)
                {
                    polyPens[i].Dispose();
                    polyPens[i] = null;
                }

                if (rectPens[i] != null)
                {
                    rectPens[i].Dispose();
                    rectPens[i] = null;
                }
            }

            return overlaidImage;
        }

        public static GraphicsPath ObjectToGraphicsPath(DetectedObject obj)
        {
            return SIA.UI.Controls.Helpers.VisualAnalysis.ObjectAnalyzer.ObjectToGraphicsPath(obj);
        }
    }
}
