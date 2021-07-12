using System;
using System.Windows.Forms;
using System.Drawing;

using SIA.UI.Components;
using SIA.UI.Components.Helpers;

namespace SIA.UI.MaskEditor.DrawTools
{
	/// <summary>
	/// Ellipse graphic object
	/// </summary>
    public class DrawEllipse : SiGlaz.Common.DrawEllipse
	{
		public DrawEllipse() : base()
		{
		}

        public DrawEllipse(float x, float y, float width, float height) : base(x, y, width, height)
        {
        }

        public DrawEllipse(SiGlaz.Common.DrawEllipse obj)
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
