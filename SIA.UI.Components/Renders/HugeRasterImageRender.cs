using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

using SIA.Common;

using SIA.SystemFrameworks;

using SIA.UI.Components;
using SIA.UI.Components.Helpers;

namespace SIA.UI.Components.Renders
{
	/// <summary>
	/// The HugeRasterImageRender provides functionality for rendering large image
	/// </summary>
	public class HugeRasterImageRender 
        : NormalRasterImageRender
	{
		#region member attributes
		
		private const String NAME = "GDI Huge Raster Image Render";
		private const String DESCRIPTION = "Large image render";

		#endregion

		#region constructor and destructors
		public HugeRasterImageRender() : base()
		{
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
			return viewer != null && viewer.Image != null;
		}

		public override void Paint(System.Drawing.Graphics graph, System.Drawing.RectangleF src, System.Drawing.RectangleF srcClip, System.Drawing.RectangleF dst, System.Drawing.RectangleF dstClip)
		{
			if (this.ImageViewer == null || this.ImageViewer.IsImageAvailable == false)
				return;

			IRasterImage image = this.ImageViewer.Image;			
			
			// source image container
			Bitmap subImage = null;
			GraphicsState state = graph.Save();
			try
			{
				GraphicsUnit units = GraphicsUnit.Pixel;
				graph.InterpolationMode = InterpolationMode.NearestNeighbor;
				graph.PixelOffsetMode = PixelOffsetMode.Half;

				Transformer transform = new Transformer(this.ImageViewer.Transform);

				// retrieve physical visible rectangle
				Rectangle rcClient = this.ImageViewer.ClientRectangle;
				RectangleF rcLogical = transform.RectangleToLogical(rcClient);

				int left   = (int)Math.Max(0, Math.Floor(rcLogical.Left));
				int top    = (int)Math.Max(0, Math.Floor(rcLogical.Top));
				int right  = (int)Math.Min(image.Width-1, Math.Ceiling(rcLogical.Right + 1));
				int bottom = (int)Math.Min(image.Height-1, Math.Ceiling(rcLogical.Bottom + 1));

				Rectangle srcRect = Rectangle.FromLTRB(left, top, right, bottom);
				RectangleF dstRect = transform.RectangleToPhysical(srcRect);

				float scaleDx = dstRect.Width / srcRect.Width;
				float scaleDy = dstRect.Height / srcRect.Height;
				float scaleFactor = (float) Math.Sqrt(scaleDx*scaleDx + scaleDy*scaleDy);
				
				if (scaleFactor >= 1.0F)
				{
					subImage = image.CreateBitmap(this.Palette, srcRect, PixelFormat.Format24bppRgb);
					graph.DrawImage(subImage, dstRect, new RectangleF(0, 0, subImage.Width, subImage.Height), units);
				}
				else 
				{
					subImage = image.CreateBitmap(this.Palette, srcRect, dstRect, PixelFormat.Format24bppRgb);
					graph.DrawImage(subImage, dstRect, new RectangleF(0, 0, subImage.Width, subImage.Height), units);
				}
				
				// graph.DrawRectangle(Pens.Green, dst.Left, dst.Top, dst.Width, dst.Height);
			}
			catch(System.Exception exp)
			{
				// throw exp;
				System.Diagnostics.Trace.WriteLine(exp);
			}
			finally
			{
				if (subImage != null)
				{
					subImage.Dispose();	
					subImage = null;
				}
				// flush graphics
				graph.Flush(FlushIntention.Sync);
				// restore last saved graphics state
				graph.Restore(state);
				graph = null;
			}
		}

		public override void Print(IRasterImage image, System.Drawing.Graphics graph, System.Drawing.RectangleF src, System.Drawing.RectangleF srcClip, System.Drawing.RectangleF dst, System.Drawing.RectangleF dstClip)
		{
			if (image == null)
				throw new ArgumentNullException("image");
			if (graph == null)
				throw new ArgumentNullException("graph");

			// source image container
			Bitmap subImage = null;
			GraphicsState state = graph.Save();
			try
			{
				GraphicsUnit units = GraphicsUnit.Pixel;
				graph.InterpolationMode = InterpolationMode.NearestNeighbor;
				graph.PixelOffsetMode = PixelOffsetMode.Half;

				// initialize transformation
				float scaleDx = dst.Width / src.Width;
				float scaleDy = dst.Height / src.Height;
				float scaleFactor = (float) Math.Sqrt(scaleDx*scaleDx + scaleDy*scaleDy);
					
				// retrieve drawing rectangles
				Rectangle srcRect = Rectangle.Round(src);
				RectangleF dstRect = dst;
					
				if (scaleFactor >= 1.0F)
				{
					subImage = image.CreateBitmap(this.Palette, srcRect, PixelFormat.Format24bppRgb);
					graph.DrawImage(subImage, dstRect, new RectangleF(0, 0, subImage.Width, subImage.Height), units);
				}
				else 
				{
					subImage = image.CreateBitmap(this.Palette, srcRect, dstRect, PixelFormat.Format24bppRgb);
					graph.DrawImage(subImage, dstRect, new RectangleF(0, 0, subImage.Width, subImage.Height), units);
				}
			}
			catch(System.Exception exp)
			{
				System.Diagnostics.Trace.WriteLine(exp);
			}
			finally
			{
				if (subImage != null)
				{
					subImage.Dispose();	
					subImage = null;
				}
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
