using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SiGlaz.UI.CustomControls.ScrollBar
{
	public enum eArrowType
	{
		Up = 0,
		Down = 1,
		Previous = 2,
		Next = 3
	}
	
	/// <summary>
	/// Summary description for ArrowEx.
	/// </summary>
	public class ArrowEx
	{
		private Color _arrowColor = Color.Blue;
		private Color _arrowBorderColor = Color.White;
		private Color _bkColorStart = Color.FromArgb(218, 234, 253);
		private Color _bkColorEnd = Color.FromArgb(136, 174, 228);

		private eArrowType _arrowType = eArrowType.Up;

		private Rectangle _bounds = Rectangle.Empty;

		public ArrowEx(eArrowType arrowType)
		{	
		}
	}
}
