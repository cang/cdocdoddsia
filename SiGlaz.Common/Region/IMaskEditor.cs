using System;

namespace SiGlaz.Common
{
	public enum DrawToolType
	{
		Pointer,
		Rectangle,
		Ellipse,
		Line,
		Polygon,
		OnionRing,
		NumberOfDrawTools
	};

	/// <summary>
	/// Summary description for IMaskEditor.
	/// </summary>
	public interface IMaskEditor 
	{
		string FileName {get; set;}
		bool AppliedMask {get; set;}

		GraphicsList GraphicsList {get;}
		System.Drawing.Drawing2D.Matrix Transform {get;}

		event EventHandler MaskChanged;
		event EventHandler AppliedMaskChanged;
	}
}
