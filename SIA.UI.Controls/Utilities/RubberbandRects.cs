//
// RubberbandRects.cs - A class to generate a rubberband rectangle in a .NET
// application through calls to the Win32 GDI API.
// created on 9/5/2003 at 4:46 PM by cthomas
// 

using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace SIA.UI.Controls.Utilities
{
	public enum PenStyles
	{
		PS_SOLID		= 0,
		PS_DASH			= 1,
		PS_DOT			= 2,
		PS_DASHDOT		= 3,
		PS_DASHDOTDOT	= 4
	}


	#region RubberbandTOOL
	public class RubberbandTool
	{
		// These values come from the larger set of defines in WinGDI.h,
		// but are all that are needed for this application.  If this class
		// is expanded for more generic rectangle drawing, they should be
		// replaced by enums from those sets of defones.
		private int NULL_BRUSH = 5;
		private int R2_XORPEN = 7;
		private PenStyles penStyle;
		private int BLACK_PEN = 0;

		// Default contructor - sets member fields
		public RubberbandTool()
		{
			penStyle = PenStyles.PS_DOT;
		}
		
		// penStyles property get/set.
		public PenStyles PenStyle
		{
			get { return penStyle; }
			set { penStyle = value; }
		}
		
		public Color	LineColor
		{
			set 
			{ 
				BLACK_PEN= RGB(value.R,value.G,value.B);
			}
		}

		public void DrawXORRectangle( Graphics grp,
		                              int X1, int Y1, int X2, int Y2 )
		{
			// Extract the Win32 HDC from the Graphics object supplied.
			IntPtr hdc = grp.GetHdc();
			
			// Create a pen with a dotted style to draw the border of the
			// rectangle.
			IntPtr gdiPen = CreatePen( penStyle,
			              1, BLACK_PEN );
			
			// Set the ROP cdrawint mode to XOR.
			SetROP2( hdc, R2_XORPEN );
			
			// Select the pen into the device context.
			IntPtr oldPen = SelectObject( hdc, gdiPen );
			
			// Create a stock NULL_BRUSH brush and select it into the device
			// context so that the rectangle isn't filled.
			IntPtr oldBrush = SelectObject( hdc,
			                     GetStockObject( NULL_BRUSH ) );
			
			// Now XOR the hollow rectangle on the Graphics object with
			// a dotted outline.
			Rectangle( hdc, X1, Y1, X2, Y2 );
			
			// Put the old stuff back where it was.
			SelectObject( hdc, oldBrush ); // no need to delete a stock object
			SelectObject( hdc, oldPen );
			DeleteObject( gdiPen );		// but we do need to delete the pen
			
			// Return the device context to Windows.
			grp.ReleaseHdc( hdc );
		}

		public void DrawXORLine( Graphics grp,
			int X1, int Y1, int X2, int Y2 )
		{
			// Extract the Win32 HDC from the Graphics object supplied.
			IntPtr hdc = grp.GetHdc();
			
			// Create a pen with a dotted style to draw the border of the
			// rectangle.
			IntPtr gdiPen = CreatePen( penStyle,
				1, BLACK_PEN );
			
			// Set the ROP cdrawint mode to XOR.
			SetROP2( hdc, R2_XORPEN );
			
			// Select the pen into the device context.
			IntPtr oldPen = SelectObject( hdc, gdiPen );

			MoveToEx(hdc,X1,Y1,System.IntPtr.Zero);
			LineTo(hdc,X2,Y2);

			SelectObject( hdc, oldPen );
			DeleteObject( gdiPen );		// but we do need to delete the pen
			
			// Return the device context to Windows.
			grp.ReleaseHdc( hdc );
		}


		public void DrawXOREllipse( Graphics grp,
			int X1, int Y1, int X2, int Y2 )
		{
			// Extract the Win32 HDC from the Graphics object supplied.
			IntPtr hdc = grp.GetHdc();
			
			// Create a pen with a dotted style to draw the border of the
			// rectangle.
			IntPtr gdiPen = CreatePen( penStyle,
				1, BLACK_PEN );
			
			// Set the ROP cdrawint mode to XOR.
			SetROP2( hdc, R2_XORPEN );
			
			// Select the pen into the device context.
			IntPtr oldPen = SelectObject( hdc, gdiPen );
			
			// Create a stock NULL_BRUSH brush and select it into the device
			// context so that the rectangle isn't filled.
			IntPtr oldBrush = SelectObject( hdc,
				GetStockObject( NULL_BRUSH ) );
			
			// Now XOR the hollow rectangle on the Graphics object with
			// a dotted outline.
			Ellipse( hdc, X1, Y1, X2, Y2 );
			
			// Put the old stuff back where it was.
			SelectObject( hdc, oldBrush ); // no need to delete a stock object
			SelectObject( hdc, oldPen );
			DeleteObject( gdiPen );		// but we do need to delete the pen
			
			// Return the device context to Windows.
			grp.ReleaseHdc( hdc );
		}


		public void DrawXORArc(Graphics graph, Rectangle rect, Point ptStartArc, Point ptEndArc)
		{
			IntPtr hDC = IntPtr.Zero;

			try
			{
				hDC = graph.GetHdc();

				// Create a pen with a dotted style to draw the border of the
				// rectangle.
				IntPtr gdiPen = CreatePen( penStyle, 1, BLACK_PEN );
			
				// Set the ROP cdrawint mode to XOR.
				SetROP2( hDC, R2_XORPEN );
			
				// Select the pen into the device context.
				IntPtr oldPen = SelectObject( hDC, gdiPen );

				int oldDir = SetArcDirection(hDC, AD_COUNTERCLOCKWISE);

				// Create a stock NULL_BRUSH brush and select it into the device
				// context so that the rectangle isn't filled.
				IntPtr oldBrush = SelectObject(hDC, GetStockObject( NULL_BRUSH ) );

				// Now XOR the hollow arc on the Graphics object with a dotted outline.
				Arc(hDC, rect.Left, rect.Top, rect.Right, rect.Bottom, 
					ptStartArc.X, ptStartArc.Y, ptEndArc.X, ptEndArc.Y);

				SetArcDirection(hDC, oldDir);

				// Put the old stuff back where it was.
				SelectObject( hDC, oldBrush ); // no need to delete a stock object
				SelectObject( hDC, oldPen );
				DeleteObject( gdiPen );		// but we do need to delete the pen
			}
			finally
			{
				if (hDC != IntPtr.Zero)
					graph.ReleaseHdc(hDC);
				hDC = IntPtr.Zero;
			}
		}

		public unsafe void DrawXORCurve(Graphics graph, Point[] pts)
		{
			IntPtr hBuf = IntPtr.Zero;
			IntPtr hDC = IntPtr.Zero;

			try
			{
				hDC = graph.GetHdc();

				// Create a pen with a dotted style to draw the border of the
				// rectangle.
				IntPtr gdiPen = CreatePen( penStyle, 1, BLACK_PEN );
			
				// Set the ROP cdrawint mode to XOR.
				SetROP2( hDC, R2_XORPEN );
			
				// Select the pen into the device context.
				IntPtr oldPen = SelectObject( hDC, gdiPen );

				// Create a stock NULL_BRUSH brush and select it into the device
				// context so that the rectangle isn't filled.
				IntPtr oldBrush = SelectObject(hDC, GetStockObject( NULL_BRUSH ) );

				// Now XOR the hollow curve on the Graphics object with a dotted outline.
				int size = pts.Length * Marshal.SizeOf(typeof(SIA.Common.Native.NativeMethods.POINT));
				hBuf = Marshal.AllocHGlobal(size);
				int* points = (int*)hBuf.ToPointer();
				for (int i=0; i<pts.Length; i++)
				{
					points[i*2] = pts[i].X;
					points[i*2+1] = pts[i].Y;
				}

				PolyBezier(hDC, hBuf, pts.Length);

				// Put the old stuff back where it was.
				SelectObject( hDC, oldBrush ); // no need to delete a stock object
				SelectObject( hDC, oldPen );
				DeleteObject( gdiPen );		// but we do need to delete the pen
			}
			finally
			{
				if (hBuf != IntPtr.Zero)
					Marshal.FreeHGlobal(hBuf);
				hBuf = IntPtr.Zero;

				if (hDC != IntPtr.Zero)
					graph.ReleaseHdc(hDC);
				hDC = IntPtr.Zero;
			}
		}

		// Use Interop to call the corresponding Win32 GDI functions
		[DllImport( "gdi32.dll" )]
		private static extern int SetROP2(
		        IntPtr hdc,		// Handle to a Win32 device context
		        int enDrawMode	// Drawing mode
		        );
		[DllImport( "gdi32.dll" )]
		private static extern IntPtr CreatePen(
		        PenStyles enPenStyle,	// Pen style from enum PenStyles
		        int nWidth,				// Width of pen
		        int crColor				// Color of pen
		        );
		[DllImport( "gdi32.dll" )]
		private static extern bool DeleteObject(
		        IntPtr hObject	// Win32 GDI handle to object to delete
		        );
		[DllImport( "gdi32.dll" )]
		private static extern IntPtr SelectObject(
		        IntPtr hdc,		// Win32 GDI device context
		        IntPtr hObject	// Win32 GDI handle to object to select
		        );
		[DllImport( "gdi32.dll" )]
		private static extern void Rectangle(
		        IntPtr hdc,			// Handle to a Win32 device context
		        int X1,				// x-coordinate of top left corner
		        int Y1,				// y-cordinate of top left corner
		        int X2,				// x-coordinate of bottom right corner
		        int Y2				// y-coordinate of bottm right corner
		        );

		[DllImport( "gdi32.dll" )]
		private static extern bool LineTo(
			IntPtr hdc,			// Handle to a Win32 device context
			int X,	
			int Y
			);

		[DllImport( "gdi32.dll" )]
		private static extern bool MoveToEx(
			IntPtr hdc,			// Handle to a Win32 device context
			int X,	
			int Y,
			IntPtr outpoint
			);

		[DllImport( "gdi32.dll" )]
		private static extern bool Ellipse(
			IntPtr hdc,			// Handle to a Win32 device context
			int nLeftRect,	
			int nTopRect,
			int nRightRect,	
			int nBottomRect
			);

		[DllImport( "gdi32.dll" )]
		private static extern bool Arc(
			IntPtr hdc,			// Handle to a Win32 device context
			int nLeftRect,	
			int nTopRect,
			int nRightRect,	
			int nBottomRect,
			int nXStartArc,
			int nYStartArc,
			int nXEndArc,
			int nYEndArc
			);

		[DllImport( "gdi32.dll" )]
		private static extern bool PolyBezier(
			IntPtr hdc,			// Handle to a Win32 device context
			IntPtr lppt, // end points and control points
			long cPoints // count of endpoints and control points
			);

		[DllImport( "gdi32.dll" )]
		private static extern int GetArcDirection(IntPtr hdc);

		[DllImport( "gdi32.dll" )]
		private static extern int SetArcDirection(IntPtr hdc, int arcDirection);

		[DllImport( "gdi32.dll" )]
		private static extern IntPtr GetStockObject( 
		        int brStyle	// Selected from the WinGDI.h BrushStyles enum
		        );


		private const int AD_COUNTERCLOCKWISE = 2;
		private const int AD_CLOCKWISE = 1;


		// C# version of Win32 RGB macro
		private static int RGB( int R, int G, int B )
		{
			return ( R | (G<<8) | (B<<16) );
		}
	}

	
	#endregion

}

