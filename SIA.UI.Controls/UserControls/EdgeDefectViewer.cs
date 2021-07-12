using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;

using System.Diagnostics;

namespace SIA.UI.Controls.UserControls
{
	/// <summary>
	/// Summary description for EdgeDefectViewer.
	/// </summary>
	public class EdgeDefectViewer : System.Windows.Forms.UserControl
	{
		float angleCurrent = 0;
		System.Drawing.Image _memBuffer = null;
		//bool bReDraw = true;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public EdgeDefectViewer()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (_memBuffer != null)
				{
					_memBuffer.Dispose();
					_memBuffer = null;
				}

				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// EdgeDefectViewer
			// 
			this.Name = "EdgeDefectViewer";
			this.Size = new System.Drawing.Size(264, 256);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPaint);

		}
		#endregion

		public float AngleCurrent
		{
			get 
			{
				return angleCurrent;
			}
			set
			{
				angleCurrent = value;
				OnChangeAngleCurrent();
			}
		}

		private void OnChangeAngleCurrent()
		{
			//RenderMemBuffer();

			this.Invalidate();
		}

		private void RenderMemBuffer(Graphics graph)
		{
			//bReDraw = true;

			int width = this.Size.Width;
			int height = this.Size.Height;
			Point center = new Point(width/2, height/2);
			int radius = Math.Min(width/2, height/2);
			Rectangle rect = new Rectangle(center.X-radius, center.Y-radius, 2*radius, 2*radius);

			//Graphics graph = null;

			Pen pen = new Pen(Color.Blue, 2);
			try
			{							
//				if (_memBuffer == null)
//				{				
//					_memBuffer = new Bitmap(width, height, PixelFormat.Format24bppRgb);
//					if (_memBuffer == null)
//						throw new System.OutOfMemoryException();
//				}

//				graph = Graphics.FromImage(this._memBuffer);
				graph.SmoothingMode = SmoothingMode.None;
				graph.InterpolationMode = InterpolationMode.NearestNeighbor;
							
				graph.Clear(this.BackColor);

				// add draw here


				//graph.Clear(this.BackColor);

				// draw circle
				graph.DrawEllipse(pen, rect);

				int esp = 5;
				graph.DrawLine(pen, center.X-radius, center.Y, center.X-radius+esp, center.Y);

				graph.DrawLine(pen, center.X+radius, center.Y, center.X+radius-esp, center.Y);

				graph.DrawLine(pen, center.X, center.Y-radius, center.X, center.Y-radius+esp);

				graph.DrawLine(pen, center.X, center.Y+radius, center.X, center.Y+radius-esp);

				graph.DrawLine(pen, center.X-esp, center.Y-esp, center.X+esp, center.Y+esp);

				graph.DrawLine(pen, center.X-esp, center.Y+esp, center.X+esp, center.Y-esp);
				
				float radAngle = (float)(angleCurrent*Math.PI/180);
				int x = (int)(radius*Math.Cos(radAngle));
				int y = (int)(radius*Math.Sin(radAngle));

				pen.Color = Color.DarkGreen;
				graph.DrawLine(pen, center.X + x, center.Y - y, center.X, center.Y);
				Brush brush = new SolidBrush(Color.Red);
				Font drawFont = new Font("Arial", 16);
				graph.DrawString(((int)angleCurrent).ToString(), drawFont, brush, center.X, center.Y);
			}
			catch (System.Exception exp)
			{
				Trace.WriteLine(exp);
			}
			finally
			{
				if(pen != null)
					pen.Dispose();
				pen = null;

//				if (graph != null) 
//					graph.Dispose();
//				graph = null;
			}
		}

		private void OnPaint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
//			if (bReDraw)
//			{		
//				if (_memBuffer != null)
//				{
//					Rectangle dst = new Rectangle(0, 0, _memBuffer.Width, _memBuffer.Height);				
//					Rectangle src = new Rectangle(0, 0, _memBuffer.Width, _memBuffer.Height);
//					e.Graphics.SmoothingMode = SmoothingMode.HighSpeed;
//					e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
//					e.Graphics.DrawImage(_memBuffer, dst, src, GraphicsUnit.Pixel);
//				}
//				bReDraw = false;
//			}
			this.RenderMemBuffer(e.Graphics);
		}

	}
}
