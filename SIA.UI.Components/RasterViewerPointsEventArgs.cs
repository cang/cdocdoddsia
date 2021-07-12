//using Leadtools;

namespace SIA.UI.Components
{

    public class RasterViewerPointsEventArgs : SIA.UI.Components.RasterViewerInteractiveEventArgs
    {
        private SIA.UI.Components.RasterPointCollection _points;

        public SIA.UI.Components.RasterPointCollection Points
        {
            get
            {
                return _points;
            }
        }

        public RasterViewerPointsEventArgs()
        {
            _points = null;
        }

        public RasterViewerPointsEventArgs(SIA.UI.Components.RasterViewerInteractiveStatus status, SIA.UI.Components.RasterPointCollection points) : base(status)
        {
            _points = points;
        }

    } // class RasterViewerPointsEventArgs

}

