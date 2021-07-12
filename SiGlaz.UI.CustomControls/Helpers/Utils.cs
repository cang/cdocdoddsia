using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace SiGlaz.UI.CustomControls
{
	/// <summary>
	/// Summary description for Utils.
	/// </summary>
	public class Utils
	{
		public Utils()
		{
		}

        public static Image ToDisableImage(Image image)
        {
            if (image == null)
                return null;

            int w = image.Width;
            int h = image.Height;
            Image disableImage = 
                new Bitmap(w, h, PixelFormat.Format32bppArgb);
            Bitmap imgSrc = image as Bitmap;
            Bitmap imgDst = disableImage as Bitmap;

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    Color src = imgSrc.GetPixel(x, y);

                    int alpha = src.A;
                    if (alpha > 30) alpha = 255;
                    int intensity = 
                        (src.R * 30 + src.G * 59 + src.B * 11 + 128) / 256 + 30;
                    if (intensity > 255) intensity = 255;

                    imgDst.SetPixel(x, y, Color.FromArgb(alpha, intensity, intensity, intensity));
                }
            }

            return disableImage;
        }

        public static GraphicsPath CreateRoundRect(
			float x, float y, float w, float h, float r,
			bool rTL, bool rTR, bool rBL, bool rBR)
		{

			GraphicsPath gPath = null;			

			if( w < 2 || h <2 || r<=0 || r ==w || r==h)
			{
				gPath = new GraphicsPath();
				gPath.AddPolygon(
					new PointF[] {
									 new PointF(x, y),
									 new PointF(x, y+h),
									 new PointF(x+w, y+h),
									 new PointF(x+w, y)
								 });

				gPath.CloseFigure();

				return gPath;
			}

			gPath = new GraphicsPath();

			float diameter = 2*r;
												
			if (rTR)
			{
				gPath.AddArc(x + w - diameter, y, diameter, diameter, 270, 90);
				gPath.AddLine(x + w, y + r, x + w, y + h - r);
			}
			else
			{
				gPath.AddLine(x + w - r, y, x + w, y);
				gPath.AddLine(x + w, y, x + w, y + h - r);
			}

			if (rBR)
			{
				gPath.AddArc(x + w - diameter, y + h - diameter, diameter, diameter,0,90);
				gPath.AddLine(x + w - r, y + h, x + r, y + h);
			}
			else
			{
				gPath.AddLine(x+w, y+h-r, x+w, y+h);
				gPath.AddLine(x+w, y+h, x+r, y+h);
			}

			if (rBL)
			{
				gPath.AddArc(x, y + h - diameter, diameter, diameter, 90, 90);
				gPath.AddLine(x, y + h - r, x, y + r);
			}
			else
			{
				gPath.AddLine(x+r, y+h, x, y+h);
				gPath.AddLine(x, y+h, x, y+r);
			}

			if (rTL)
			{
				gPath.AddArc(x, y, diameter, diameter, 180, 90);
				//gPath.AddLine(x + r, y, x + w - r, y);
			}
			else
			{
				gPath.AddLine(x, y+r, x, y);
				//gPath.AddLine(x, y, x+w-r, y);
			}

			gPath.CloseFigure();

			return gPath;
		}

		/**
		private void DrawBlurMarginBanner(Graphics grph)
		{
			float w = this.Width;
			float h = this.Height;
			RectangleF rct;

			// top banner
			rct =  new RectangleF(0, 0, w, _marginSize);
			using (LinearGradientBrush brush = new LinearGradientBrush(
					   rct, 
					   _blurMarginColor.Start, 
					   _blurMarginColor.End, 
					   LinearGradientMode.Vertical))
			{
				using (GraphicsPath path = new GraphicsPath())
				{
					path.AddPolygon(
						new PointF[] {
										 new PointF(0, 0),
										 new PointF(_marginSize-1, _marginSize-1),
										 new PointF(w-_marginSize, _marginSize-1),
										 new PointF(w, 0)
									 });
					path.CloseFigure();
					grph.FillPath(brush, path);
				}
			}

			// left banner
			rct =  new RectangleF(0, 0, _marginSize, h);
			using (LinearGradientBrush brush = new LinearGradientBrush(
					   rct, 
					   _blurMarginColor.Start, 
					   _blurMarginColor.End, 
					   LinearGradientMode.Horizontal))
			{
				using (GraphicsPath path = new GraphicsPath())
				{
					path.AddPolygon(
						new PointF[] {
										 new PointF(0, 0),
										 new PointF(_marginSize-1, _marginSize-1),
										 new PointF(_marginSize-1, h-_marginSize-1),
										 new PointF(0, h)
									 });
					path.CloseFigure();
					grph.FillPath(brush, path);
				}
			}

			// bottom banner
			rct =  new RectangleF(0, h-_marginSize, w, _marginSize);
			using (LinearGradientBrush brush = new LinearGradientBrush(
					   rct, 
					   _blurMarginColor.End, 
					   _blurMarginColor.Start, 
					   LinearGradientMode.Vertical))
			{
				using (GraphicsPath path = new GraphicsPath())
				{
					path.AddPolygon(
						new PointF[] {
										 new PointF(0, h),
										 new PointF(_marginSize-2.0f, h-_marginSize),
										 new PointF(w-_marginSize+0.5f, h-_marginSize),
										 new PointF(w, h)
									 });
					path.CloseFigure();
					grph.FillPath(brush, path);
				}
			}

			// right banner
			rct =  new RectangleF(w-_marginSize-1, 0, _marginSize, h);
			using (LinearGradientBrush brush = new LinearGradientBrush(
					   rct, 
					   _blurMarginColor.End, 
					   _blurMarginColor.Start, 
					   LinearGradientMode.Horizontal))
			{
				using (GraphicsPath path = new GraphicsPath())
				{
					path.AddPolygon(
						new PointF[] {
										 new PointF(w, 0),
										 new PointF(w-_marginSize, _marginSize-1.0f),
										 new PointF(w-_marginSize, h-_marginSize-1.0f),
										 new PointF(w, h)
									 });
					path.CloseFigure();
					grph.FillPath(brush, path);
				}
			}
		}
		**/
	}
}
