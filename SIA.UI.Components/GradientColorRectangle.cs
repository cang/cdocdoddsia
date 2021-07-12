using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections;

namespace SIA.UI.Components
{
	/// <summary>
	/// The GradientColorRectangle class is used for displaying the gradient colors
	/// </summary>
	public class GradientColorRectangle 
        : System.Windows.Forms.Control
	{
		private ColorBlend _colorBlend = new ColorBlend();

        /// <summary>
        /// Gets the ColorBlend used for displaying
        /// </summary>
		public ColorBlend ColorBlend
		{
			get {return _colorBlend;}
		}

		public GradientColorRectangle()
		{
			InitializeComponents();

			_colorBlend = new ColorBlend(2);
			_colorBlend.Colors[0] = Color.Black;
			_colorBlend.Positions[0] = 0.0F;
			_colorBlend.Colors[1] = Color.White;
			_colorBlend.Positions[1] = 1.0F;
		}

		private void InitializeComponents()
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.Selectable, true);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint (e);		
			
			Graphics graph = e.Graphics;
			Rectangle rcDraw = this.ClientRectangle;
			LinearGradientBrush brush = new LinearGradientBrush(rcDraw, Color.Black, Color.White, 0.0F, false);
			brush.InterpolationColors = _colorBlend;			
			graph.FillRectangle(brush, rcDraw);
			brush.Dispose();
		}
	}
}
