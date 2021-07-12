using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Data;
using System.Diagnostics;

namespace SIA.SystemLayer.OpenCv
{
	/// <summary>
	/// Wrapper class for OpenCV cv module
	/// </summary>
	public unsafe class cv
	{
#if DEBUG
		public const string dllName = "cvd.dll";
#else
		public const string dllName = "cv.dll";
#endif

		public static void cvFitEllipse2(PointF[] points, ref RectangleF rcBound, ref PointF center, ref double angle)
		{
			int numpts = points.Length;
			
			fixed (PointF* pts = points)
			{
				// initialize list of points
				CvMat mat = cxcore.cvMat(1, numpts, (int)CvMatType.CV_32FC2, pts);

				// fit ellipse
				CvBox2D box = cvFitEllipse2(&mat);
										
				float left = box.Center.X - box.Size.Width*0.5F;
				float top = box.Center.Y - box.Size.Height*0.5F;
				rcBound = new RectangleF(left, top, box.Size.Width, box.Size.Height);
				center.X = box.Center.X; center.Y = box.Center.Y;
				angle = box.Angle;
			}
		}
        
		[DllImport(dllName, CharSet=CharSet.Ansi)]
		private static extern CvBox2D cvFitEllipse2(void* arr);
	}
}
