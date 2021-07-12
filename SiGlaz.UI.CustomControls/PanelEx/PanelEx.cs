using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SiGlaz.UI.CustomControls
{
	/// <summary>
	/// Summary description for PanelEx.
	/// </summary>
	public class PanelEx : Panel
	{
		private Color _borderColor = Color.FromArgb(0, 45, 150);

		public PanelEx() : base()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint (e);
			
			using (Pen pen = new Pen(_borderColor, 1.0f))
			{
				e.Graphics.DrawRectangle(pen, 0, 0, (float)this.Width-1.0f, (float)this.Height-1.0f);
			}
		}
	}
}
