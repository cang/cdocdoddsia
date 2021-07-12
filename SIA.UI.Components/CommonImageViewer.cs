using System;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;

using SIA.Common;
using SIA.SystemFrameworks;
using SIA.IPEngine;
using SIA.SystemLayer;
using SIA.UI.Components;
using SIA.UI.Components.Common;
using SIA.UI.Components.Helpers;
using SIA.UI.Components.Renders;
using SIA.UI.Components.Printing;


namespace SIA.UI.Components
{
	/// <summary>
	/// Provide functionality for displaying the CommonImage
	/// </summary>
	public class CommonImageViewer
        : RasterImageViewer
	{
		#region members

		/// <summary>
		/// current image
		/// </summary>
		protected SIA.SystemLayer.CommonImage _image = null;

		/// <summary>
		/// pseudo color
		/// </summary>
		private PseudoColor _pseudoColor = null;

		/// <summary>
		/// Rotation Angle
		/// </summary>
		private float _rotateAngle = 0.0f;

		/// <summary>
		/// Raster image render
		/// </summary>
		private IRasterImageRender _render = null;

		#endregion

		#region Events

		#endregion

		#region public properties

		public float RotateAngle
		{
			get {return _rotateAngle;}
			set 
			{
				if (_rotateAngle != value)
				{
					_rotateAngle = value;
					OnRotateAngleChanged();
				}
			}
		}

		public new SIA.SystemLayer.CommonImage Image
		{
			get {return _image;}
			set
			{
				_image = value;
				OnImageChanged();				
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public PseudoColor PseudoColor
		{
			get 
			{
				if (_pseudoColor == null)
					_pseudoColor = PseudoColors.GrayScale;
				return _pseudoColor;
			}
			set 
			{
				if (value == null)
					throw new System.ArgumentNullException("Invalid Pseudo Color parameter");
				_pseudoColor = (PseudoColor)value.Clone();
				OnPseudoColorChanged();
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override IRasterImageRender RasterImageRender
		{
			get
			{
				if (this.IsImageAvailable == false)
					return RasterImageRenderFactory.DefaultRender;
				else if (_render == null || _render == RasterImageRenderFactory.DefaultRender)
				{
					_render = RasterImageRenderFactory.CreateRender(this);
					_render.ImageViewer = this;
				}
				return _render;
			}
		}

		protected virtual void OnRotateAngleChanged()
		{
		}

		protected virtual void OnImageChanged()
		{
			base.Image = _image != null ? _image.RasterImage : null;
			// reset RasterImageRender
			if (this._image == null && this._render != null)
			{
				this._render.ImageViewer = null;
				this._render = null;
			}
		}

		protected virtual void OnMaskImageChanged()
		{
		}

		protected virtual void OnClusterDataChanged()
		{
		}

		protected virtual void OnPseudoColorChanged()
		{
			// valid pseudo color
			if (_pseudoColor != null)
			{
				if (_pseudoColor.Colors.Length < 2 || _pseudoColor.Positions.Length < 2 || 
					_pseudoColor.Colors.Length != _pseudoColor.Positions.Length)
					throw new System.ArgumentException("Invalid Pseudo Color parameter");

				int num_stops = _pseudoColor.Colors.Length;
				Color[] colors = _pseudoColor.Colors;
				float[] positions = _pseudoColor.Positions;
				
				if (positions[0] != 0.0F || positions[num_stops-1]!=1.0F)
				{
					ArrayList pos_array = new ArrayList(positions);
					ArrayList color_array = new ArrayList(colors);

					if (positions[0] != 0.0F)
					{
						pos_array.Insert(0, 0.0F);
						color_array.Insert(0, colors[0]);
					}

					if (positions[num_stops-1] != 1.0F)
					{
						pos_array.Add(1.0F);
						color_array.Add(colors[num_stops-1]);
					}

					_pseudoColor.Colors = (Color[])color_array.ToArray(typeof(Color));
					_pseudoColor.Positions = (float[])pos_array.ToArray(typeof(float));
				}
			}
		}
		#endregion

		#region constructor and destructor

		public CommonImageViewer() : base()
		{
			base.AutoDisposeImages = false;
			base.AutoResetScaleFactor = false;
			base.AutoResetScrollPosition = false;

			// initialize printing helpers
			this.InitPrinting();
		}

		protected override void Dispose(bool disposing)
		{
			// uninitialized printing helpers
			this.UninitPrinting();
			
			base.Dispose (disposing);
		}

		#endregion

		#region public operations
	
		#endregion

		#region override routines
		
		protected override void OnPaint(PaintEventArgs e, RectangleF src, RectangleF srcClip, RectangleF dest, RectangleF destClip)
		{
			base.OnPaint(e, src, srcClip, dest, destClip);

			if (IsImageAvailable)
			{
				IRasterImage image = base.Image;
				IRasterImageRender render = this.RasterImageRender;
				render.UpdateColorMapTable(PseudoColor.Colors, PseudoColor.Positions);
				render.Paint(e.Graphics, src, srcClip, dest, destClip);
			}
		}

		#endregion		

		#region printing helpers

		private PrintDocument _printDocument = null;
		private PrintSettings _printSettings = null;
		
		public bool IsPrinterAvailable
		{
			get 
			{
                bool installedPrinter = true;

                try
                {
                    installedPrinter = 
                        (PrinterSettings.InstalledPrinters != null && 
                        PrinterSettings.InstalledPrinters.Count > 0);
                }
                catch
                {
                    //MessageBox.Show(
                    //    "Printer has not installed yet.", "Document Printer", 
                    //    MessageBoxButtons.OK, MessageBoxIcon.Error);

                    installedPrinter = false;
                }

				return installedPrinter;
			}
		}

		public bool CanPrint
		{
			get 
			{
				return this.IsImageAvailable && _printDocument != null;
			}
		}

		public PrintDocument PrintDocument
		{
			get {return _printDocument;}
		}

		public PrintSettings PrintSettings
		{
			get 
			{
				if (_printSettings == null)
				{
					if (_printDocument == null)
						throw new ArgumentNullException("_printDocument");
					if (this.IsImageAvailable == false)
						throw new ArgumentException("Image");

					// retrieve image size
					RectangleF rcPrintArea = new RectangleF(PointF.Empty, new Size(this.Image.Width, this.Image.Height));
					// convert from pixel to hundredth of an inch 
					rcPrintArea = this.RectangleToHundredthInches(rcPrintArea);
					// create new instance of PrintSettings
					_printSettings = new PrintSettings(_printDocument, true, false, 1.0F, rcPrintArea, false, RectangleF.Empty);

				}

				return _printSettings;
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException("value");
				if (value != _printSettings)
				{
					_printSettings = value;
					OnPrintSettingsChanged();
				}
			}
		}

		protected virtual void OnPrintSettingsChanged()
		{
		}

		private void InitPrinting()
		{
			if (_printDocument == null && this.IsPrinterAvailable)
			{	
				_printDocument = new PrintDocument();
				_printDocument.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);
			}

			if (_printSettings != null)
				_printSettings = null;
		}

		private void UninitPrinting()
		{
			if (_printDocument != null)
			{
				_printDocument.PrintPage -= new PrintPageEventHandler(PrintDocument_PrintPage);
				_printDocument.Dispose();
				_printDocument = null;
			}

			if (_printSettings != null)
				_printSettings = null;
		}

		public void Print(PrinterSettings settings)
		{
			this._printDocument.PrinterSettings = settings;
			this._printDocument.Print();
		}

		private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
		{			
			this.OnPrint(e);
		}

		protected virtual void OnPrint(PrintPageEventArgs e)
		{
			RasterImagePrinter printer = new RasterImagePrinter(this);
			printer.SizeMode = RasterViewerSizeMode.Normal;
			printer.CenterMode = RasterViewerCenterMode.None;
			printer.Print(e);
		}

		#endregion
	}
}
