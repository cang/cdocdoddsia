using System.Drawing;

namespace SIA.UI.Components
{

    public class RasterViewerRectangleEventArgs : SIA.UI.Components.RasterViewerInteractiveEventArgs
    {

        private System.Drawing.Rectangle _rectangle;

        public System.Drawing.Rectangle Rectangle
        {
            get
            {
                return _rectangle;
            }
        }

        public RasterViewerRectangleEventArgs()
        {
            _rectangle = System.Drawing.Rectangle.Empty;
        }

        public RasterViewerRectangleEventArgs(SIA.UI.Components.RasterViewerInteractiveStatus status, System.Drawing.Rectangle rc) : base(status)
        {
            _rectangle = rc;
        }

    } // class RasterViewerRectangleEventArgs

}

