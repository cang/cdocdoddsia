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
using SIA.UI.Components.Renders;

namespace SIA.UI.Components.Renders
{
	/// <summary>
	/// The PreviewRasterImageRender class provides functionality for rendering preview of a raster image
	/// </summary>
	public class PreviewRasterImageRender
        : NormalRasterImageRender
	{
		#region member attributes
		
		private const String NAME = "GDI Preview Raster Image Render";
		private const String DESCRIPTION = "Preview image render";

		Bitmap _memBuffer = null;

		#endregion

		#region constructor and destructors
		public PreviewRasterImageRender() : base()
		{
		}

		public override void Dispose()
		{
			base.Dispose ();

			if (_memBuffer != null)
			{
				_memBuffer.Dispose();	
				_memBuffer = null;
			}
		}


		#endregion

		#region override routines

		public override string Name
		{
			get
			{
				return NAME;
			}
		}

		public override string Description
		{
			get
			{
				return DESCRIPTION;
			}
		}

		public override bool CanRender(RasterImageViewer viewer)
		{
			return false;
		}

		public override void Paint(System.Drawing.Graphics graph, System.Drawing.RectangleF src, System.Drawing.RectangleF srcClip, System.Drawing.RectangleF dst, System.Drawing.RectangleF dstClip)
		{
			if (this.ImageViewer == null || this.ImageViewer.IsImageAvailable == false)
				return;

			IRasterImage image = this.ImageViewer.Image;			
			
			// source image container
			GraphicsState state = graph.Save();
			try
			{
				GraphicsUnit units = GraphicsUnit.Pixel;
				graph.InterpolationMode = InterpolationMode.NearestNeighbor;
				graph.PixelOffsetMode = PixelOffsetMode.Half;

				Rectangle srcRect = Rectangle.Round(src);
				RectangleF dstRect = dst;

				if (this.IsDirty)
				{
					if (_memBuffer != null)
					{
						_memBuffer.Dispose();
						_memBuffer = null;
					}

					_memBuffer = image.CreateBitmap(this.Palette, srcRect, dstRect, PixelFormat.Format24bppRgb);
					this.IsDirty = false;
				}

				graph.DrawImage(_memBuffer, dstRect, new RectangleF(0, 0, _memBuffer.Width, _memBuffer.Height), units);
			}
			catch(System.Exception exp)
			{
				throw exp;
			}
			finally
			{
				// flush graphics
				graph.Flush(FlushIntention.Sync);
				// restore last saved graphics state
				graph.Restore(state);
				graph = null;
			}
		}

		#endregion
	}
}
