using System;
using System.Data;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Diagnostics;

namespace SIA.UI.Controls.Utilities
{
	/// <summary>
	/// Summary description for GDIWrapper.
	/// </summary>
	public class GDIWrapper
	{
		public const int COLORONCOLOR = 3;
		public const int HALFTONE     = 4;
		public const long SRCCOPY	  = 0x00CC0020;

		[System.Runtime.InteropServices.DllImportAttribute( "gdi32.dll" )]
		public static extern bool StretchBlt(
			IntPtr hdcDest,      // handle to destination DC
			int nXOriginDest, // x-coord of destination upper-left corner
			int nYOriginDest, // y-coord of destination upper-left corner
			int nWidthDest,   // width of destination rectangle
			int nHeightDest,  // height of destination rectangle
			IntPtr hdcSrc,       // handle to source DC
			int nXOriginSrc,  // x-coord of source upper-left corner
			int nYOriginSrc,  // y-coord of source upper-left corner
			int nWidthSrc,    // width of source rectangle
			int nHeightSrc,   // height of source rectangle
			long dwRop       // raster operation code
			);
		[System.Runtime.InteropServices.DllImportAttribute( "gdi32.dll" )]
		public static extern IntPtr CreateCompatibleDC(
			IntPtr hdc   // handle to DC
			);
		[System.Runtime.InteropServices.DllImportAttribute( "gdi32.dll" )]
		public static extern IntPtr SelectObject(
			IntPtr hdc,          // handle to DC
			IntPtr hgdiobj   // handle to object
			);
		[System.Runtime.InteropServices.DllImportAttribute( "gdi32.dll" )]
		public static extern bool DeleteDC(
			IntPtr  hdc   // handle to DC
			);

		[System.Runtime.InteropServices.DllImportAttribute( "gdi32.dll" )]
		public static extern bool DeleteObject(
			IntPtr hObject   // handle to graphic object
			);
		[System.Runtime.InteropServices.DllImportAttribute( "gdi32.dll" )]
		public static extern int SetStretchBltMode(
			IntPtr hdc,			// handle to DC
			int iStretchMode	// stretch mode 
			);

		public static void DrawBitBlt(Graphics g, Bitmap bmp, Rectangle scrRec,Rectangle destRec)
		{
			// Get DC handle and create a compatible one
			IntPtr hDC= g.GetHdc();
			IntPtr offscreenDC= CreateCompatibleDC(hDC);

			// Select our bitmap in to DC, recording what was there before
			IntPtr hBitmap = bmp.GetHbitmap();
			IntPtr oldObject = SelectObject(offscreenDC, hBitmap);

			StretchBlt(hDC, destRec.X, destRec.Y, destRec.Width, destRec.Height, offscreenDC, scrRec.X, scrRec.Y, scrRec.Width, scrRec.Height, 13369376);

			// Select our bitmap object back out of the DC
			SelectObject(offscreenDC, oldObject);
			// Delete our bitmap
			DeleteObject(hBitmap);
			// Delete memory DC and release our DC handle
			DeleteDC(offscreenDC);

			g.ReleaseHdc(hDC);
		}

		public static void DrawBitBlt(Graphics graph, IntPtr memDC, Rectangle srcRect, Rectangle destRect)
		{
			try
			{
				IntPtr hDC = graph.GetHdc();
				if (hDC != (IntPtr)0)
				{
					StretchBlt(hDC, destRect.X, destRect.Y, destRect.Width, destRect.Height,
						memDC, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, 13369376);
					graph.ReleaseHdc(hDC);
				}
			}
			catch(Exception exp)
			{
				Debug.WriteLine(exp.ToString());
			}
		}
	}
}
