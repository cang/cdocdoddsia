using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using SIA.UI.Components;
using SIA.UI.Components.Printing;
using SIA.UI.Components.Helpers;


namespace SIA.UI.Components.Printing
{
	/// <summary>
	/// Summary description for PrintSettingsPreviewControl.
	/// </summary>
	public class PrintSettingsPreviewControl : PrintPreviewControlEx
	{
		private PrintSettings _printSettings = null;
		
		/// <summary>
		/// image used for internal print
		/// </summary>
		private Image _image = null;

		[Browsable(false)]
		public PrintSettings PrintSettings
		{
			get {return _printSettings;}
			set
			{
				_printSettings = value;			
				OnPrintSettingsChanged();
			}
		}


		public PrintSettingsPreviewControl() : base()
		{
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose (disposing);

			if (_image != null)
				_image.Dispose();
			_image = null;
		}

		protected virtual void OnPrintSettingsChanged()
		{
			// init document
			if (_printSettings != null)
				this.Document = _printSettings.Document;
			else
				this.Document = null;		
				
			this.Invalidate(true);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint (e);
		}

		private void PreviewPrintPage(Graphics graph, int srcWidth, int srcHeight, PrintSettings settings)
		{
			PrintDocument document = settings.Document;
			PageSettings pageSettings = document.DefaultPageSettings;
			bool isLandscape = pageSettings.Landscape;
			Size pageSize = Size.Empty;
			if (!isLandscape)
				pageSize = new Size(pageSettings.PaperSize.Width, pageSettings.PaperSize.Height);
			else
				pageSize = new Size(pageSettings.PaperSize.Height, pageSettings.PaperSize.Width);

			// when print selection, the Print Settings objects is automatically 
			// references Settings.X to SelectionRectangle.X
			float left = settings.X;
			float top = settings.Y;
			float width = settings.Width;
			float height = settings.Height;
			
			if (settings.ScaleToFit)
			{
				float scaleFactor = 1.0F;
				
				if (pageSize.Width <= pageSize.Height)
				{
					scaleFactor = pageSize.Height/height;
					if ((scaleFactor*width) > pageSize.Width)
						scaleFactor = pageSize.Width/width;
				}
				else 
				{
					scaleFactor = pageSize.Width/width;
					if ((scaleFactor*height) > pageSize.Height)
						scaleFactor = pageSize.Height/height;
				}
				
				left = 0; 
				top = 0;
				width = width * scaleFactor;
				height = height * scaleFactor;
			}
			
			if (settings.CenterImage)
			{
				left = (int)Math.Round((pageSize.Width - width)*0.5F);
				top = (int)Math.Round((pageSize.Height - height)*0.5F);
			}
			
			using (Matrix matrix = new Matrix())
			{
				float scaleDx = (float)pageSize.Width / (float)srcWidth;
				float scaleDy = (float)pageSize.Height / (float)srcHeight;
				matrix.Scale(scaleDx, scaleDy, MatrixOrder.Append);

				Transformer transformer = new Transformer(matrix);
				RectangleF rcBound = new RectangleF(left, top, width, height);
				rcBound = transformer.RectangleToLogical(rcBound);

				graph.Clear(Color.White);
				
				graph.FillRectangle(Brushes.Blue, rcBound.Left, rcBound.Top, rcBound.Width, rcBound.Height);
				graph.DrawRectangle(Pens.Red, rcBound.Left, rcBound.Top, rcBound.Width, rcBound.Height);
			}
		}

		protected override void ComputePreview()
		{
			if (this.Document == null)
			{
				base.ComputePreview();
			}
			else if (!DesignMode)
			{
				try
				{
					int srcWidth = 256;
					int srcHeight = 256;

					PrintDocument document = this.Document;
					PageSettings pageSettings = document.DefaultPageSettings;
				
					// check if paper size is initialized correctly
					if (pageSettings.PaperSize.Width == 0 || pageSettings.PaperSize.Height == 0)
					{
						// check if custom paper size is exists
						if (PrintSettings.UseCustomPaperSize && PrintSettings.CustomPaperSize.Width > 0 && PrintSettings.CustomPaperSize.Height > 0)
						{
							document.DefaultPageSettings.PaperSize = PrintSettings.CustomPaperSize;
						}
						else
						{
							// set default paper size A4
							int width = (int)Math.Round(210 * 254 / 100.0F); // 210 mm
							int height = (int)Math.Round(297 * 254 / 100.0F); // 297 mm
							PaperSize A4Size = new PaperSize("Custom A4", width, height);
							document.DefaultPageSettings.PaperSize = A4Size;					
						}
					}

					bool isLandscape = pageSettings.Landscape;
					Size pageSize = Size.Empty;
					if (!isLandscape)
						pageSize = new Size(pageSettings.PaperSize.Width, pageSettings.PaperSize.Height);
					else
						pageSize = new Size(pageSettings.PaperSize.Height, pageSettings.PaperSize.Width);

					if (pageSize.Width == 0 || pageSize.Height == 0)
						pageSize = new Size(srcWidth, srcHeight);

					// create image for printing
					_image = new Bitmap(srcWidth, srcHeight, PixelFormat.Format24bppRgb);

					// compute preview page info
					using (Graphics graph = Graphics.FromImage(_image))
					{
						this.PreviewPrintPage(graph, srcWidth, srcHeight, _printSettings);
				
						PreviewPageInfo info = new PreviewPageInfo(_image, new Size(pageSize.Width, pageSize.Height));
						this.PageInfo = new PreviewPageInfo[] {info};
					}
				}
				catch (System.Exception exp)
				{
					Trace.WriteLine(exp);
					
					base.ComputePreview();
				}
				finally
				{
				}
			}
		}
	}
}
