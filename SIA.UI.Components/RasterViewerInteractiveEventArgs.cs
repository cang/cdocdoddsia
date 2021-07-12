using System;

namespace SIA.UI.Components
{
	/// <summary>
	/// Encapsulates type of interaction
	/// </summary>
	public enum RasterViewerInteractiveStatus
	{
		Begin,
		Working,
		End
	}

    /// <summary>
    /// Provides basic implemendation for image viewer interaction
    /// </summary>
    public class RasterViewerInteractiveEventArgs : System.EventArgs
    {
        private bool _cancel;
        private RasterViewerInteractiveStatus _status;

        public bool Cancel
        {
            get
            {
                return _cancel;
            }
            set
            {
                _cancel = value;
            }
        }

        public SIA.UI.Components.RasterViewerInteractiveStatus Status
        {
            get
            {
                return _status;
            }
        }

        public RasterViewerInteractiveEventArgs()
        {
            _status = SIA.UI.Components.RasterViewerInteractiveStatus.Begin;
            _cancel = false;
        }

        public RasterViewerInteractiveEventArgs(SIA.UI.Components.RasterViewerInteractiveStatus status)
        {
            _status = status;
            _cancel = false;
        }

    } // class RasterViewerInteractiveEventArgs

}

