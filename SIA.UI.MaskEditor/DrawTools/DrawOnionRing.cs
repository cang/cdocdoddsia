using System;
using System.Globalization;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Diagnostics;

using SIA.UI.Components;
using SIA.UI.Components.Helpers;

namespace SIA.UI.MaskEditor.DrawTools
{
	/// <summary>
	/// Summary description for DrawOnionRing.
	/// </summary>
    public class DrawOnionRing : SiGlaz.Common.DrawOnionRing
	{
		public DrawOnionRing() : base()
		{
		}

		public DrawOnionRing(float x, float y, float width, float height) : base(x, y, width, height)
		{
		}

        public DrawOnionRing(SiGlaz.Common.DrawOnionRing obj)
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
