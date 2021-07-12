using System;

namespace SiGlaz.UI.CustomControls
{
	/// <summary>
	/// Summary description for ScrollBarParams.
	/// </summary>
	public class ScrollBarParams
	{
		public bool Visible = false;
		public bool Enable = false;		
		public double ThumbSpanPositionFactor = 0;
		public double ThumbSpanSizeFactor = 0;

		public ScrollBarParams()
		{
		}

		public ScrollBarParams(bool visible, bool enable, 
			double thumbSpanPositionFactor, double thumbSpanSizeFactor)
		{
			Visible = visible;
			Enable = enable;
			ThumbSpanPositionFactor = thumbSpanPositionFactor;
			ThumbSpanSizeFactor = thumbSpanSizeFactor;
		}
	}
}
