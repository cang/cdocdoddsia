using System;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace SIA.UI.Controls
{
	/// <summary>
	/// Customized from MouseEventArgs class provides the location of the mouse in PointF structure
	/// </summary>
	public class MouseEventArgsEx 
        : MouseEventArgs
	{
		private PointF _pt;

		public PointF PointF
		{
			get {return _pt;}
		}

        public MouseEventArgsEx(MouseEventArgs e, PointF logPoint)
            : base(e.Button, e.Clicks, e.X, e.Y, e.Delta)
        {
            this._pt = logPoint;
        }

		public MouseEventArgsEx(MouseButtons button, int clicks, PointF pt, int delta)
			: base(button, clicks, (int)Math.Round(pt.X), (int)Math.Round(pt.Y), delta)
		{
			this._pt = pt;
		}
	}
}
