using System;
using System.Drawing;

namespace SIA.UI.Components
{
    public class RasterViewerLineEventArgs 
        : RasterViewerInteractiveEventArgs
    {
		private PointF _begin;
        private PointF _end;

		public Point Begin
		{
			get {return Point.Round(_begin);}
		}

		public Point End
		{
			get {return Point.Round(_end);}
		}

		public PointF BeginF
		{
			get
			{
				return _begin;
			}
		}

        public PointF EndF
        {
            get
            {
                return _end;
            }
        }

        public RasterViewerLineEventArgs()
        {
            _begin = Point.Empty;
            _end = Point.Empty;
        }

        public RasterViewerLineEventArgs(RasterViewerInteractiveStatus status, Point beginPoint, Point endPoint) : base(status)
        {
            _begin = beginPoint;
            _end = endPoint;
        }

		public RasterViewerLineEventArgs(RasterViewerInteractiveStatus status, PointF beginPoint, PointF endPoint) : base(status)
		{
			_begin = beginPoint;
			_end = endPoint;
		}

    } // class RasterViewerLineEventArgs

}

