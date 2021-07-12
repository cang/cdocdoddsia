using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SIA.Common;
using SIA.Common.IPLFacade;


namespace SIA.UI.Components
{
    /// <summary>
    /// The IRasterImageRender provides declaration of the render used for rendering the IRasterImage
    /// </summary>
    public interface IRasterImageRender
        : ICloneable, IDisposable
    {
        /// <summary>
        /// Gets name of the render
        /// </summary>
        string Name { get;}

        /// <summary>
        /// Gets short description of the render
        /// </summary>
        string Description { get;}

        /// <summary>
        /// Gets or sets boolean value indicates whether the render should be updated
        /// </summary>
        bool IsDirty { get; set;}

        /// <summary>
        /// Gets or sets boolean value indicates whether the color map table should be updated
        /// </summary>
        bool IsColorMapTableDirty { get; set;}

        /// <summary>
        /// Gets or sets the image viewer associated with this render
        /// </summary>
        IRasterImageViewer ImageViewer { get; set;}

        /// <summary>
        /// Gets or sets boolean value indicates whether the intensity range for rendering is auto fitted.
        /// </summary>
        bool AutoFitGrayScale { get; set;}

        /// <summary>
        /// Gets or sets the intensity range for rendering the image
        /// </summary>
        DataRange ViewRange { get; set;}

        /// <summary>
        /// Raised when the ViewRange is changed
        /// </summary>
        event EventHandler ViewRangeChanged;

        /// <summary>
        /// Forces update color map table
        /// </summary>
        void UpdateColorMapTable();

        /// <summary>
        /// Updates the color map table by the specified colors and stop positions
        /// </summary>
        /// <param name="colors">The color array to update</param>
        /// <param name="positions">The associated stop positions to update</param>
        void UpdateColorMapTable(Color[] colors, float[] positions);

        /// <summary>
        /// Checks whether this render can render the specified image viewer
        /// </summary>
        /// <param name="viewer">The image viewer to check</param>
        /// <returns>True if it can otherwise false</returns>
        bool CanRender(RasterImageViewer viewer);

        /// <summary>
        /// Renders the image into the specified graphics
        /// </summary>
        /// <param name="graph">The graphics to render</param>
        /// <param name="src">The source rectangle in logical coordinate</param>
        /// <param name="srcClip">The clip rectangle in logical coordinate</param>
        /// <param name="dst">The destination rectangle in physical coordinate</param>
        /// <param name="dstClip">The clip rectangle in physical coordinate</param>
        void Paint(Graphics graph, RectangleF src, RectangleF srcClip, RectangleF dst, RectangleF dstClip);

        /// <summary>
        /// Checks whether this render can print the specified image
        /// </summary>
        /// <param name="image">The image to check</param>
        /// <returns>True if it can otherwise false</returns>
        bool CanPrint(IRasterImage image);

        /// <summary>
        /// Prints the specified image to the graphic
        /// </summary>
        /// <param name="image">The image to print</param>
        /// <param name="graph">The destination graphic object</param>
        /// <param name="src">The source rectangle in logical coordinate</param>
        /// <param name="srcClip">The clip rectangle in logical coordinate</param>
        /// <param name="dst">The destination rectangle in physical coordinate</param>
        /// <param name="dstClip">The clip rectangle in physical coordinate</param>
        void Print(IRasterImage image, Graphics graph, RectangleF src, RectangleF srcClip, RectangleF dst, RectangleF dstClip);
    }
}
