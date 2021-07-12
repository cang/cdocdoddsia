using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SiGlaz.Common
{
	/// <summary>
	/// Ellipse graphic object
	/// </summary>
	public class DrawEllipse 
        : DrawRectangle
	{
		public DrawEllipse()
		{
            SetRectangle(0, 0, 1, 1);
            Initialize();
		}

        public DrawEllipse(float x, float y, float width, float height)
        {
            Rectangle = new RectangleF(x, y, width, height);
            Initialize();
        }

		public DrawEllipse(DrawEllipse obj) : base(obj)
		{

		}

        public override void Draw(Graphics g)
        {
            Pen pen = new Pen(Color, PenWidth);
			Brush brush = new SolidBrush(BrushColor);

			RectangleF rectDraw = DrawRectangle.GetNormalizedRectangle(Rectangle);

            GraphicsPath path = CreateGraphicsPath();
            if (Container.MetroSys != null)
                path.Transform(Container.MetroSys.InvTransformer);
            g.FillPath(brush, path);
            g.DrawPath(pen, path);
            //g.FillEllipse(brush, rectDraw);
            //g.DrawEllipse(pen, rectDraw);

            pen.Dispose();
			brush.Dispose();
        }

        public override GraphicsPath CreateGraphicsPath()
        {
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(this.Rectangle);

            return path;
        }

		public override void Draw(SIA.Common.Mask.IMask mask)
		{
			RectangleF rectDraw = DrawRectangle.GetNormalizedRectangle(Rectangle);
			mask.FillEllipse(rectDraw);
		}

		public override DrawObject Copy()
		{
			DrawEllipse cloneObject = new DrawEllipse();
			cloneObject.Rectangle = this.Rectangle;
			cloneObject.Color = this.Color;
			cloneObject.BrushColor = this.BrushColor;
			cloneObject.TrackerBrushColor = this.TrackerBrushColor;
			cloneObject.TrackerPenColor = this.TrackerPenColor;
			cloneObject.PenWidth = this.PenWidth;

            cloneObject.Initialized = this.Initialized;
            cloneObject.Description = this.Description;
			return cloneObject;
		}		

		public override Cursor GetHandleCursor(int handleNumber)
		{
            return Resources.ApplicationCursors.DrawEllipse;
			/*if (handleNumber<1 || handleNumber>8)
				return Resources.ApplicationCursors.DrawEllipse;
			return base.GetHandleCursor (handleNumber);*/
		}

	}
}
