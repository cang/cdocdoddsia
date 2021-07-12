using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

using SIA.Common;
using SIA.SystemLayer;
using SIA.IPEngine;
using SIA.UI.Controls;
using SIA.UI.Controls.UserControls;


namespace SIA.UI.Controls.Utilities
{
	/// <summary>
	/// Summary description for DrawHelper.
	/// </summary>
	public class DrawHelper
	{
		Graphics _drawingCanvas = null;
		RubberbandTool _rubberBandTool = new RubberbandTool();
		private ImageViewer _imageViewer = null;
		private Color _lineColor = Color.Black;
		private Color _brushColor = Color.Black;

		public Graphics Graphics
		{
			get {return _drawingCanvas;}
		}

		public Color LineColor
		{
			get {return _lineColor;}
			set {_lineColor = value;}
		}

		public Color BrushColor
		{
			get {return _brushColor;}
			set {_brushColor = value;}
		}

		public DrawHelper(ImageViewer imageViewer)
		{
			if (imageViewer == null)
				throw new System.ArgumentNullException("Invalid parameter");
			_imageViewer = imageViewer;
			_drawingCanvas = _imageViewer.CreateGraphics();
			_drawingCanvas.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
		}

		~DrawHelper()
		{
			_drawingCanvas.Dispose();
		}

		public void DrawXorRectangle(RectangleF rect)
		{
			Rectangle rcRound = Rectangle.Round(rect);
			_rubberBandTool.DrawXORRectangle(_drawingCanvas, rcRound.Left, rcRound.Top, rcRound.Right, rcRound.Bottom);
		}

		public void DrawXorRectangle(int left, int top, int right, int bottom)
		{
			_rubberBandTool.DrawXORRectangle(_drawingCanvas, left, top, right, bottom);
		}

		public void DrawXorEllipse(int left, int top, int right, int bottom)
		{
			_rubberBandTool.DrawXOREllipse(_drawingCanvas, left, top, right, bottom);
		}

		public void DrawXorLine(int x1, int y1, int x2, int y2)
		{
			_rubberBandTool.DrawXORLine(_drawingCanvas, x1, y1, x2, y2);
		}

		public void DrawXorCurve(Point[] pts)
		{
			_rubberBandTool.DrawXORCurve(_drawingCanvas, pts);
		}

		public void DrawXorArc(RectangleF rect, PointF ptStartArc, PointF ptEndArc)
		{
			_rubberBandTool.DrawXORArc(_drawingCanvas, Rectangle.Round(rect), Point.Round(ptStartArc), Point.Round(ptEndArc));
		}

		public void DrawXorLine(Point pt1, Point pt2)
		{
			DrawXorLine(pt1.X, pt1.Y, pt2.X, pt2.Y);
		}

		public void DrawXorLine(PointF pt1, PointF pt2)
		{
			DrawXorLine(Point.Round(pt1), Point.Round(pt2));
		}

		public void DrawXorCurve(PointF[] pts)
		{
			Point[] points = new Point[pts.Length];
			for (int i=0; i<pts.Length; i++)
				points[i] = Point.Round(pts[i]);

			_rubberBandTool.DrawXORCurve(_drawingCanvas, points);
		}
		
	}
}
