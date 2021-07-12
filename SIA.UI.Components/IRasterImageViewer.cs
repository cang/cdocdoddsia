using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using SIA.Common;

namespace SIA.UI.Components
{
    /// <summary>
    /// Provides the declaration of raster image viewer
    /// </summary>
    public interface IRasterImageViewer
    {
        /// <summary>
        /// Gets the transformation from logical to physical coordinate
        /// </summary>
        Matrix Transform { get;}

        /// <summary>
        /// The raster image for displaying
        /// </summary>
        IRasterImage Image { get; }

        /// <summary>
        /// Gets boolean value indicates the raster image is available
        /// </summary>
        bool IsImageAvailable { get;}

        /// <summary>
        /// Gets the client rectangle
        /// </summary>
        Rectangle ClientRectangle {get;}

        /// <summary>
        /// Raises when the raster image is changed
        /// </summary>
        event EventHandler ImageChanged;
    }
}
