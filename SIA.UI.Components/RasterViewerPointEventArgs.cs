using System.Drawing;

namespace SIA.UI.Components
{

    public class RasterViewerPointEventArgs : SIA.UI.Components.RasterViewerInteractiveEventArgs
    {
        private System.Drawing.PointF _point;

        public System.Drawing.Point Point
        {
            get
            {
                return Point.Round(_point);
            }
        }

		public System.Drawing.PointF PointF
		{
			get
			{
				return _point;
			}
		}

        public RasterViewerPointEventArgs()
        {
            _point = System.Drawing.PointF.Empty;
        }

        public RasterViewerPointEventArgs(SIA.UI.Components.RasterViewerInteractiveStatus status, System.Drawing.PointF pt) : base(status)
        {
            _point = pt;
        }

    } // class RasterViewerPointEventArgs

}

