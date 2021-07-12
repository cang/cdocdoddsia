using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.ComponentModel;
using System.Runtime.InteropServices;

using SIA.Common;
using SIA.SystemFrameworks;
using SIA.SystemFrameworks.UI;
using SIA.Common.Native;
using SIA.UI.Components;


namespace SIA.UI.Components.Renders
{
	/// <summary>
	/// Factory class provides creation of the render for raster image
	/// </summary>
	public sealed class RasterImageRenderFactory 
	{
		private static IRasterImageRender defaultRender = new NormalRasterImageRender();
		private static ArrayList registeredRenders = null;

        /// <summary>
        /// Gets the default render
        /// </summary>
        public static IRasterImageRender DefaultRender
        {
            get
            {
                return RasterImageRenderFactory.defaultRender;
            }
        }

        /// <summary>
        /// Gets list of registered renders
        /// </summary>
		private static ArrayList Renders
		{
			get
			{
				if (registeredRenders == null)
					InitializeRenders();
				return registeredRenders;
			}
		}

        /// <summary>
        /// registers for built-in raster image renders
        /// </summary>
		private static void InitializeRenders()
		{
			RegisterRender(new NormalRasterImageRender());
			RegisterRender(new HugeRasterImageRender());
		}

        /// <summary>
        /// Registers the specified render
        /// </summary>
        /// <param name="render">The render to register</param>
		private static void RegisterRender(IRasterImageRender render)
		{
			if (render == null)
				throw new System.ArgumentNullException("Invalid parameter");
			if (registeredRenders == null)
				registeredRenders = new ArrayList();
			registeredRenders.Add(render);
		}

        /// <summary>
        /// Creates the render for the specified image viewer
        /// </summary>
        /// <param name="viewer">The image viewer to enumerate</param>
        /// <returns>The raster image render for the specified image viewer if succeeded, otherwise null</returns>
		public static IRasterImageRender CreateRender(RasterImageViewer viewer)
		{
			ArrayList renders = RasterImageRenderFactory.Renders;
			foreach(IRasterImageRender render in renders)
				render.ImageViewer = null;

			foreach(IRasterImageRender render in renders)
			{
				if (render.CanRender(viewer))
					return (IRasterImageRender)render.Clone();
			}

			return null;
		}

        /// <summary>
        /// Creates the render for printing devices of the specified image
        /// </summary>
        /// <param name="image">The image to enumerate</param>
        /// <returns>An instance of the render if succeeded, otherwise null</returns>
		public static IRasterImageRender CreatePrintRender(IRasterImage image)
		{
			ArrayList renders = RasterImageRenderFactory.Renders;

            foreach (IRasterImageRender render in renders)
            {
                if (render.CanPrint(image))
                    return (IRasterImageRender)render.Clone();
            }

			return null;
		}

        /// <summary>
        /// Creates the render for previewing of the specified image viewer
        /// </summary>
        /// <param name="viewer">The image viewer for creation</param>
        /// <returns>An instance of the render</returns>
		public static IRasterImageRender CreatePreviewRender(RasterImageViewer viewer)
		{
			PreviewRasterImageRender render = new PreviewRasterImageRender();
			render.ViewRange = viewer.RasterImageRender.ViewRange;
			return render;
		}
	}

	
}
