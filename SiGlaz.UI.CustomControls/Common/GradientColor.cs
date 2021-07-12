using System;
using System.Drawing;

namespace SiGlaz.UI.CustomControls
{
	/// <summary>
	/// Summary description for GradientColor.
	/// </summary>
	public class GradientColor
	{
		private Color _start = Color.Blue;
		private Color _end = Color.SkyBlue;

		public Color Start
		{
			get { return _start; }
			set { _start = value; }
		}

		public Color End
		{
			get { return _end; }
			set { _end = value; }
		}

		public GradientColor()
		{
		}

		public GradientColor(Color start, Color end)
		{
			_start = start;
			_end = end;
		}

        //public static Color[][] GradientColorCollection = new Color[][]//color collection
        //{
        //    new Color[]{Color.FromArgb(227,239,255),Color.FromArgb(135,173,228)},//image band
        //    new Color[]{Color.FromArgb(195,218,249),Color.FromArgb(158,190,245)},//mainmenu band
        //    new Color[]{Color.FromArgb(255,244,204),Color.FromArgb(255,214,154)}//selection/hotlight
        //};


        public static int ImageBandIdx = 0;
        public static int MainMenuBandIdx = 1;
        public static int HighlightBandIdx = 2;
        public static int PushedBandIdx = 3;

        public static Color[][] GradientColorCollection = new Color[][]//color collection
	    {
		    new Color[]{
                Color.FromArgb(227,239,255),
                Color.FromArgb(135,173,228)},//image band


		    new Color[]{
                Color.FromArgb(218,234,253),
                Color.FromArgb(135,174,228)},//mainmenu band


		    new Color[]{
                Color.FromArgb(255,244,204),
                Color.FromArgb(255,214,154)},//selection/hotlight


            new Color[]{
                Color.FromArgb(255,244,204),
                Color.FromArgb(255,214,154)},//pushed
	    };
	}

    
}
