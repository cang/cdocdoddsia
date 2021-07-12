using System;
using System.Data;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Diagnostics;

using SIA.UI.Controls.Utilities;

namespace SIA.UI.Controls.UserControls
{
	/// <summary>
	/// class kPictureBox
	/// </summary>
	public class kPictureBox : System.Windows.Forms.Control
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.Windows.Forms.Cursor m_OldCursor = null;
		private System.Drawing.Image m_image = null;
		private System.Drawing.Image m_mask = null;
		private System.IntPtr	m_memDC = IntPtr.Zero;
		private float m_RotationAngle = 0.0f;
		private float m_scaleDX = 1.0f;
		private float m_scaleDY = 1.0f;
		/// <summary>
		/// Default constructor
		/// </summary>
		public kPictureBox()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			UninitializeComponent();
			base.Dispose( disposing );		
		}

		protected void InitializeComponent()
		{
			m_memDC = IntPtr.Zero;
			m_OldCursor = null;
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.Selectable, false);
		}

		protected void UninitializeComponent()
		{
			if (m_memDC != IntPtr.Zero) GDIWrapper.DeleteDC(m_memDC);
			if (m_mask != null) {m_mask.Dispose(); m_mask = null;}
			m_OldCursor = null;
		}

		/// <summary>
		/// Override OnPaint
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics graph = e.Graphics;
			if (m_image != null)
			{
				Size  imgSize = m_image.Size;
				float pixelWidth  = (float)Math.Round(1.0f/m_scaleDX);
				float pixelHeight = (float)Math.Round(1.0f/m_scaleDY);
				int srcX, srcY, srcWidth, srcHeight;
				int dstX, dstY, dstWidth, dstHeight;

				/* retrieve visible rectangle in client coordinate */
				if (m_scaleDX > 1.0f || m_scaleDY > 1.0f)
				{
					dstX = 0;
					dstY = 0;
					dstWidth = Width;
					dstHeight = Height;

					srcX = 0;
					srcY = 0;
					srcWidth = imgSize.Width;
					srcHeight = imgSize.Height;
				}
				else
				{
					dstX = e.ClipRectangle.X;
					dstY = e.ClipRectangle.Y;	
					dstWidth  = e.ClipRectangle.Width;
					dstHeight = e.ClipRectangle.Height;

					srcX = (int)Math.Floor(dstX * m_scaleDX);
					srcY = (int)Math.Floor(dstY * m_scaleDY);
					srcWidth  = (int)Math.Min((float)(Math.Ceiling(dstWidth * m_scaleDX) + 1.0f), (float)imgSize.Width);
					srcHeight = (int)Math.Min((float)(Math.Ceiling(dstHeight * m_scaleDY) + 1.0f), (float)imgSize.Height);

					dstX = (int)Math.Round(srcX * pixelWidth);
					dstY = (int)Math.Round(srcY * pixelHeight);
					dstWidth = (int)Math.Round(srcWidth * pixelWidth);
					dstHeight = (int)Math.Round(srcHeight * pixelHeight);
				}

				Rectangle srcRect = new Rectangle((int)srcX, (int)srcY, (int)srcWidth, (int)srcHeight);
				Rectangle dstRect = new Rectangle((int)dstX, (int)dstY, (int)dstWidth, (int)dstHeight);

				RenderMemDC(graph, dstRect, srcRect);
			}

			/* apply transform matrix to graphic object */
			Matrix matTransform = new Matrix();
			matTransform.RotateAt(-m_RotationAngle, new PointF(Width/2.0f,Height/2.0f));
			graph.Transform = matTransform;

			base.OnPaint (e);
		}

		/// <summary>
		/// Override OnPaintBackground
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			//base.OnPaintBackground(e);
		}

		/// <summary>
		/// Override OnSizeChanged
		/// </summary>
		/// <param name="e"></param>
		protected override void OnSizeChanged(EventArgs e)
		{
			ComputeScaleFactor();
			base.OnSizeChanged(e);
		}

		protected virtual void OnImageChanged(EventArgs e)
		{
			ComputeScaleFactor();
			CreateMemDC();
			Invalidate();
		}

		protected virtual void OnMaskImageChanged(EventArgs e)
		{
			ComputeScaleFactor();
			CreateMemDC();
			Invalidate();
		}

		protected virtual void OnRotationAngleChagned(EventArgs e)
		{
			ComputeScaleFactor();
			CreateMemDC();
			Invalidate();
		}

		private bool CreateMemDC()
		{
			bool bResult = false;
			if (m_memDC != IntPtr.Zero) 
				GDIWrapper.DeleteDC(m_memDC);
			if (m_image != null)
			{
				try
				{
					Size imgSize = m_image.Size;
					PointF ptCenter = new PointF(imgSize.Width/2.0f, imgSize.Height/2.0f);
					Bitmap rotateImage = new Bitmap(imgSize.Width, imgSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
					Graphics graph = Graphics.FromImage(rotateImage);
					// apply transform to image
					if (m_RotationAngle != 0.0f)
					{
						Matrix matTransform = new Matrix();
						matTransform.RotateAt(-m_RotationAngle, ptCenter);
						graph.Transform = matTransform;
					}
					// render image to graphic
					graph.DrawImage(m_image, Point.Empty);
					if (m_mask != null)
					{
						Bitmap bmpMask = m_mask as Bitmap;
						bmpMask.MakeTransparent(Color.Black);
						graph.DrawImage(bmpMask, Point.Empty);
					}
					// create memory device context 
					IntPtr hDC = graph.GetHdc();
					IntPtr hBitmap = rotateImage.GetHbitmap();
					if (hDC != IntPtr.Zero && hBitmap != IntPtr.Zero)
					{
						m_memDC = GDIWrapper.CreateCompatibleDC(hDC);
						GDIWrapper.SelectObject(m_memDC, hBitmap);
						graph.ReleaseHdc(hDC);
						GDIWrapper.DeleteObject(hBitmap);
						bResult = true;
					}
					graph.Dispose();
					rotateImage.Dispose();
				}
				catch(Exception exp)
				{
					Debug.WriteLine(exp.ToString());
					bResult = false;
				}
			}
			return bResult;
		}

		private void DestroyMemDC()
		{
			if (m_memDC != IntPtr.Zero) 
				GDIWrapper.DeleteDC(m_memDC);
		}

		private bool RenderMemDC(Graphics graph, Rectangle dstRect, Rectangle srcRect)
		{
			bool bResult = false;
			IntPtr hDCDest = graph.GetHdc();
			if (hDCDest != IntPtr.Zero)
			{
				int iOldStretchBltMode = 0;
				// set new stretch mode
				if (srcRect.Width < dstRect.Width || srcRect.Height < dstRect.Height)
					iOldStretchBltMode = GDIWrapper.SetStretchBltMode(hDCDest, GDIWrapper.COLORONCOLOR);
				else
					iOldStretchBltMode = GDIWrapper.SetStretchBltMode(hDCDest, GDIWrapper.HALFTONE);

				bResult = GDIWrapper.StretchBlt(hDCDest, dstRect.Left, dstRect.Top, dstRect.Width, dstRect.Height,
					m_memDC, srcRect.Left, srcRect.Top, srcRect.Width, srcRect.Height, GDIWrapper.SRCCOPY);

				// restore old stretch mode
				GDIWrapper.SetStretchBltMode(hDCDest, iOldStretchBltMode);
				graph.ReleaseHdc(hDCDest);
			}

			return bResult;
		}

		private void ComputeScaleFactor()
		{
			m_scaleDX = m_scaleDY = 1.0f;
			if (m_image != null)
			{
				m_scaleDX = (float)m_image.Width /Width;
				m_scaleDY = (float)m_image.Height/Height;
			}	
		}

		public void RestoreLastCursor()
		{
			if (m_OldCursor != null)
				base.Cursor = m_OldCursor;
		}

		public override Cursor Cursor
		{
			get 
			{ 
				return base.Cursor;
			}
			set
			{
				m_OldCursor = base.Cursor;
				base.Cursor = value;
			}
		}

		public float RotateAngle
		{
			get {return m_RotationAngle;}
			set 
			{
				if (m_RotationAngle != value)
				{
					m_RotationAngle = value;
					OnRotationAngleChagned(new System.EventArgs());
				}
			}
		}

		public System.Drawing.Image Image
		{
			get {return m_image;}
			set
			{
				m_image = value;
				// release mask if image is set to null
				if (m_image == null)
				{
					if (m_mask != null) m_mask.Dispose();
					m_mask = null;
				}
				OnImageChanged(new System.EventArgs());
			}
		}

		public System.Drawing.Image Mask
		{
			get {return m_mask;}
			set
			{
				if (m_mask != null) m_mask.Dispose();
				m_mask = value;
				OnMaskImageChanged(new System.EventArgs());
			}
		}
	};
}
