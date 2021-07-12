using System;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Globalization;
using System.Drawing.Drawing2D;
using System.Collections;

using SIA.UI.Components.Helpers;

namespace SIA.UI.MaskEditor.DrawTools
{
	/// <summary>
	/// Polygon graphic object
	/// </summary>
    public class DrawPolygon : SiGlaz.Common.DrawPolygon
	{
		public DrawPolygon() : base()
		{
		}

        public DrawPolygon(float x1, float y1, float x2, float y2) : base (x1, y1, x2, y2)
        {
        }

        public DrawPolygon(SiGlaz.Common.DrawPolygon obj)
            : base(obj)
		{
		}

		public override RectangleF GetHandleRectangle(int handleNumber)
		{
			PointF point = GetHandle(handleNumber);
			float size = 3;

			if (this.Container != null && this.Container.MaskEditor != null && this.Container.MaskEditor.Transform != null)
			{
				Transformer transformer = new Transformer(this.Container.MaskEditor.Transform);
				size = transformer.LengthToLogical(size);				
			}

			return new RectangleF(point.X - size, point.Y - size, 2*size, 2*size);
		}
	}
}
