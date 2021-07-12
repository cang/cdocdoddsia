using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using SIA.UI.Components;
using SIA.UI.Components.Printing;
using SIA.UI.Components.Helpers;


namespace SIA.UI.Components.Printing
{
	/// <summary>
	/// Encapsulates the settings of the printing process
	/// </summary>
	[Serializable]
	public class PrintSettings : ICloneable
	{
		private PrintDocument _document = null;
		
		private bool _centerImage = false;
		private bool _scaleToFit = false;

		private float _x = 0;
		private float _y = 0;
		private float _width = 0;
		private float _height = 0;

		private float _scaleFactor = 1.0F;

		private bool _printSelection = false;
		private RectangleF _rcSelection = RectangleF.Empty;

		private bool _useCustomPaperSize = false;
		private PaperSize _customPaperSize = null;

		public PrintDocument Document
		{
			get {return _document;}
		}

		public bool CenterImage
		{
			get {return _centerImage;}
			set {_centerImage = value;}
		}

		public bool ScaleToFit
		{
			get {return _scaleToFit;}
			set {_scaleToFit = value;}
		}

		public float ScaleFactor
		{
			get {return _scaleFactor;}
			set 
			{
				if (value <= 0)
					throw new ArgumentOutOfRangeException("value");

				_scaleFactor = value;
			}
		}

		public float X
		{
			get 
			{
				return _printSelection ? _rcSelection.X : _x;
			}
			set 
			{
				if (_printSelection)
					_rcSelection.X = value;
				else
					_x = value;
			}
		}

		public float Y
		{
			get 
			{
				return _printSelection ? _rcSelection.Y : _y;
			}
			set 
			{
				if (_printSelection)
					_rcSelection.Y = value;
				else
					_y = value;
			}
		}

		public float Width
		{
			get 
			{
				if (_printSelection)
					return _rcSelection.Width*_scaleFactor;
				else
					return _width*_scaleFactor;
			}
			set 
			{
				if (_printSelection)
					_rcSelection.Width = value/_scaleFactor;
				else
					_width = value/_scaleFactor;
			}
		}

		public float Height
		{
			get 
			{
				if (_printSelection)
					return _rcSelection.Height*_scaleFactor;
				else
					return _height*_scaleFactor;
			}
			set 
			{
				if (_printSelection)
					_rcSelection.Height = value/_scaleFactor;
				else
					_height = value/_scaleFactor;
			}
		}

		public RectangleF Rectangle
		{
			get {return new RectangleF(_x, _y, _width, _height);}
			set
			{
				_x = value.X;
				_y = value.Y;
				_width = value.Width;
				_height = value.Height;
			}
		}

		public bool PrintSelection
		{
			get {return _printSelection;}
			set {_printSelection = value;}
		}

		public RectangleF SelectionRectangle
		{
			get {return _rcSelection;}
			set {_rcSelection = value;}
		}

		public bool UseCustomPaperSize
		{
			get {return _useCustomPaperSize;}
			set {_useCustomPaperSize = value;}
		}

		public PaperSize CustomPaperSize
		{
			get {return _customPaperSize;}
			set {_customPaperSize = value;}
		}

		#region constructor and destructor

		protected PrintSettings()
		{
			_x = 0;
			_y = 0;
			_width = 0;
			_height = 0;

			_centerImage = false;
			_scaleToFit = false;
			_printSelection = false;
			_rcSelection = RectangleF.Empty;
		}
		
		public PrintSettings(PrintDocument document)
		{
			_document = document;
			PageSettings pageSettings = document.PrinterSettings.DefaultPageSettings;
			
			_x = pageSettings.Margins.Left;
			_y = pageSettings.Margins.Top;			
			_width = pageSettings.PaperSize.Width - pageSettings.Margins.Left - pageSettings.Margins.Right;
			_height = pageSettings.PaperSize.Height - pageSettings.Margins.Top - pageSettings.Margins.Bottom;

			_centerImage = false;
			_scaleToFit = false;
			_printSelection = false;
			_rcSelection = RectangleF.Empty;
		}

		public PrintSettings(PrintDocument document, bool centerImage, bool scaleToFit, float scaleFactor,
							 RectangleF rcPrintArea, bool printSelection, RectangleF rcSelection)
		{
			if (document == null)
				throw new ArgumentNullException("document");
			if (rcPrintArea.Width <= 0 || rcPrintArea.Height <=0 )
				throw new ArgumentOutOfRangeException("rcPrintArea");
			if (printSelection && (rcSelection.Width <= 0 || rcSelection.Height <= 0))
				throw new ArgumentOutOfRangeException("rcSelection");

			_document = document;
			PageSettings pageSettings = _document.PrinterSettings.DefaultPageSettings;

			_centerImage = centerImage;
			_scaleToFit = scaleToFit;
			_printSelection = printSelection;

			this.Rectangle = rcPrintArea;
			this.SelectionRectangle = rcSelection;
		}

		
		#endregion

		#region methods

		public virtual RectangleF ComputePrintRectangle()
		{
			if (this.Document == null)
				throw new ArgumentNullException("Document");

			PrintDocument document = this.Document;
			PageSettings pageSettings = document.DefaultPageSettings;
			bool isLandscape = pageSettings.Landscape;
			Size pageSize = Size.Empty;
			if (!isLandscape)
				pageSize = new Size(pageSettings.PaperSize.Width, pageSettings.PaperSize.Height);
			else
				pageSize = new Size(pageSettings.PaperSize.Height, pageSettings.PaperSize.Width);

			// when print selection, the Print Settings objects is automatically 
			// references Settings.X to SelectionRectangle.X
			float left = this.X;
			float top = this.Y;
			float width = this.Width;
			float height = this.Height;
			
			if (this.ScaleToFit)
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
			
			if (this.CenterImage)
			{
				left = (int)Math.Round((pageSize.Width - width)*0.5F);
				top = (int)Math.Round((pageSize.Height - height)*0.5F);
			}

			return new RectangleF(left, top, width, height);
		}

		#endregion

		#region ICloneable Members

		public object Clone()
		{
			return (PrintSettings)this.MemberwiseClone();
		}

		#endregion
	}

}
