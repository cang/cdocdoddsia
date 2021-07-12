using System;
using System.Windows.Forms;
using System.Drawing;


namespace SiGlaz.Common
{
	/// <summary>
	/// Helper class used to show properties
	/// for one or more graphic objects
	/// </summary>
	public class GraphicsProperties
	{
        private Color color;
        private int penWidth;
		private Color _brushColor;
        private bool colorDefined;
        private bool penWidthDefined;
		private bool _brushColorDefined;

        public GraphicsProperties()
        {
            color = Color.Black;
            penWidth = 1;
			_brushColor = Color.Black;
            colorDefined = false;
            penWidthDefined = false;
			_brushColorDefined = false;
        }

        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }

        public int PenWidth
        {
            get
            {
                return penWidth;
            }
            set
            {
                penWidth = value;
            }
        }

		public Color BrushColor
		{
			get
			{
				return _brushColor;
			}
			set
			{
				_brushColor = value;
			}
		}

        public bool ColorDefined
        {
            get
            {
                return colorDefined;
            }
            set
            {
                colorDefined = value;
            }
        }

        public bool PenWidthDefined
        {
            get
            {
                return penWidthDefined;
            }
            set
            {
                penWidthDefined = value;
            }
        }
		
		public bool BrushColorDefined
		{
			get
			{
				return _brushColorDefined;
			}
			set
			{
				_brushColorDefined = value;
			}
		}

	}
}
