using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;

using SIA.Common.Native;

namespace SIA.UI.Components.Printing
{
	/// <summary>
	/// <para> Represents the raw "preview" part of print previewing,
	/// without any dialog boxes or buttons. Most <see cref="T:System.Windows.Forms.PrintPreviewControlEx" /> objects are found on <see cref="T:System.Windows.Forms.PrintPreviewDialog" />
	/// objects, but they do not have to be.
	/// </para>
	/// </summary>
	[DefaultProperty("Document")]
	public class PrintPreviewControlEx : Control
	{
		#region constants

		private const string PrintControllerWithStatusDialog_DialogTitlePreview = "Printing";
		private const string PrintPreviewExceptionPrinting = "An error occurred while attempting to display the document.";
		private const string PrintPreviewNoPages = "The document does not contain any pages.";
		private const string PrintPreviewControlZoomNegative = "Zoom must be zero or greater. Negative values are not permitted.";
		
		#endregion

		#region fields

		private const int SCROLL_LINE = 5;
		private const int SCROLL_PAGE = 100;
		
		private static readonly object EVENT_STARTPAGECHANGED;
		
		private bool antiAlias;
		private bool autoZoom;
		private const int border = 10;
		private int columns;
		private PrintDocument document;
		private bool exceptionPrinting;
		private Size imageSize;
		private Point lastOffset;
		private bool layoutOk;
		private PreviewPageInfo[] pageInfo;
		private bool pageInfoCalcPending;
		private Point position;
		private int rows;
		private Point screendpi;
		private int startPage;
		private Size virtualSize;
		private double zoom;

		private bool autoCalculatePageInfo = true;
		
		#endregion

		#region properties

		/// <summary>
		/// Gets or sets a value indicating whether resizing the
		/// control or changing the number of pages shown automatically adjusts
		/// the <see cref="P:System.Windows.Forms.PrintPreviewControlEx.Zoom" /> property.
		/// </summary>
		[DefaultValue(true)]
		public bool AutoZoom
		{
			get
			{
				return this.autoZoom;
			}
			set
			{
				this.autoZoom = value;
				this.InvalidateLayout();
			}
		}

		/// <summary>
		/// <para>
		/// Gets or sets the number of pages
		/// displayed horizontally across the screen.
		/// </para>
		/// </summary>
		[DefaultValue(1)]
		public int Columns
		{
			get
			{
				return this.columns;
			}
			set
			{
				this.columns = value;
				this.InvalidateLayout();
			}
		}

		protected override System.Windows.Forms.CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand)]
			get
			{
				System.Windows.Forms.CreateParams params1 = base.CreateParams;
				params1.Style |= 0x100000;
				params1.Style |= 0x200000;
				return params1;
			}
		}

		/// <summary>
		/// <para> Gets or sets a value indicating the document to preview.
		/// 
		/// </para>
		/// </summary>
		[DefaultValue((string) null)]
		public PrintDocument Document
		{
			get
			{
				return this.document;
			}
			set
			{
				this.document = value;
				this.InvalidatePreview();
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		private Point Position
		{
			get
			{
				return this.position;
			}
			set
			{
				this.SetPositionNoInvalidate(value);
			}
		}

		/// <summary>
		/// <para> Gets or sets the number of pages
		/// displayed vertically down the screen.
		/// </para>
		/// </summary>
		[DefaultValue(1)]
		public int Rows
		{
			get
			{
				return this.rows;
			}
			set
			{
				this.rows = value;
				this.InvalidateLayout();
			}
		}

		/// <summary>
		/// <para> Gets or sets the page number of the upper left page.
		/// 
		/// </para>
		/// </summary>
		[DefaultValue(0)]
		public int StartPage
		{
			get
			{
				int num1 = this.startPage;
				if (this.pageInfo != null)
				{
					num1 = Math.Min(num1, this.pageInfo.Length - (this.rows * this.columns));
				}
				return Math.Max(num1, 0);
			}
			set
			{
				int num1 = this.StartPage;
				this.startPage = value;
				if (num1 != this.startPage)
				{
					this.InvalidateLayout();
					this.OnStartPageChanged(EventArgs.Empty);
				}
			}
		}

		[Bindable(false), EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}

		/// <summary>
		/// <para>Gets or sets a value indicating whether printing uses the
		/// anti-aliasing features of the operating system.</para>
		/// </summary>
		[DefaultValue(false)]
		public bool UseAntiAlias
		{
			get
			{
				return this.antiAlias;
			}
			set
			{
				this.antiAlias = value;
			}
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		private Size VirtualSize
		{
			get
			{
				return this.virtualSize;
			}
			set
			{
				this.SetVirtualSizeNoInvalidate(value);
				base.Invalidate();
			}
		}

		/// <summary>
		/// <para> Gets or sets a value indicating how large the pages will appear.
		/// </para>
		/// </summary>
		/// <exception cref="T:System.ArgumentException">The value is less than 0. </exception>
		public double Zoom
		{
			get
			{
				return this.zoom;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentException(PrintPreviewControlZoomNegative);
				}
				this.autoZoom = false;
				this.zoom = value;
				this.InvalidateLayout();
			}
		}


		public bool AutoCalculatePageInfo
		{
			get {return autoCalculatePageInfo;}
			set {autoCalculatePageInfo = value;}
		}


		protected PreviewPageInfo[] PageInfo
		{
			get {return pageInfo;}
			set {pageInfo = value;}
		}
		#endregion

		#region event handlers

		/// <summary>
		/// <para>Occurs when the start page changes.</para>
		/// </summary>
		public event EventHandler StartPageChanged
		{
			add
			{
				base.Events.AddHandler(PrintPreviewControlEx.EVENT_STARTPAGECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(PrintPreviewControlEx.EVENT_STARTPAGECHANGED, value);
			}
		}

		#endregion

		#region constructor and destructor

		static PrintPreviewControlEx()
		{
			PrintPreviewControlEx.EVENT_STARTPAGECHANGED = new object();
		}

		/// <summary>
		/// <para> Initializes a new instance of the <see cref="T:System.Windows.Forms.PrintPreviewControlEx" /> class.
		/// </para>
		/// </summary>
		public PrintPreviewControlEx()
		{
			this.virtualSize = new Size(1, 1);
			this.position = new Point(0, 0);
			this.startPage = 0;
			this.rows = 1;
			this.columns = 1;
			this.autoZoom = true;
			this.layoutOk = false;
			this.imageSize = Size.Empty;
			this.screendpi = Point.Empty;
			this.zoom = 0.3;
			this.pageInfoCalcPending = false;
			this.exceptionPrinting = false;
			this.ResetBackColor();
			this.ResetForeColor();
			base.Size = new Size(100, 100);

			base.SetStyle(ControlStyles.ResizeRedraw, false);
			base.SetStyle(ControlStyles.Opaque, true);
		}

		#endregion

		#region methods

		public void Print()
		{
			PrintController oldController = this.document.PrintController;
			
			try
			{
				PrintController stdController = new StandardPrintController();

				if (this.autoCalculatePageInfo)
					this.document.PrintController = new PrintControllerWithDlgPrintStatus(stdController , PrintControllerWithStatusDialog_DialogTitlePreview);
				else
					this.document.PrintController = new PrintControllerWithCallbackStatus(stdController , PrintControllerWithStatusDialog_DialogTitlePreview);

				this.document.Print();
			}
			finally
			{
				this.document.PrintController = oldController;		
			}
		}

		public void InvalidatePreview()
		{
			this.pageInfo = null;
			this.InvalidateLayout();
		}
		
		public void CalculatePageInfo()
		{
			if (!this.pageInfoCalcPending)
			{
				this.pageInfoCalcPending = true;
				try
				{
					if (this.pageInfo == null)
					{
						try
						{
							this.ComputePreview();
						}
						catch
						{
							this.exceptionPrinting = true;

							throw;
						}
						finally
						{
							base.Invalidate();
						}
					}
				}
				finally
				{
					this.pageInfoCalcPending = false;
				}
			}
		}


		protected override void OnPaint(PaintEventArgs pevent)
		{
			Brush brBckgnd = new SolidBrush(this.BackColor);
			try
			{
				if ((this.pageInfo == null) || (this.pageInfo.Length == 0))
				{
					pevent.Graphics.FillRectangle(brBckgnd, base.ClientRectangle);
					
					if ((this.pageInfo != null) || this.exceptionPrinting)
					{
						using (StringFormat strFmt = new StringFormat())
						{
							strFmt.Alignment = TranslateAlignment(ContentAlignment.MiddleCenter);
							strFmt.LineAlignment = TranslateLineAlignment(ContentAlignment.MiddleCenter);
							using (SolidBrush brForegnd = new SolidBrush(this.ForeColor))
							{
								try
								{
									if (this.exceptionPrinting)
									{
										pevent.Graphics.DrawString(PrintPreviewExceptionPrinting, this.Font, brForegnd, (RectangleF) base.ClientRectangle, strFmt);
										goto Label_03D5;
									}
									else
									{
										pevent.Graphics.DrawString(PrintPreviewNoPages, this.Font, brForegnd, (RectangleF) base.ClientRectangle, strFmt);
										goto Label_03D5;
									}
								}
								finally
								{
									brForegnd.Dispose();
									strFmt.Dispose();
								}
							}
						}
					}

					if (autoCalculatePageInfo) 
						base.BeginInvoke(new MethodInvoker(this.CalculatePageInfo));
					// else
					//		wait for page info is computed by another thread
				}
				else
				{
					if (!this.layoutOk)
					{
						this.ComputeLayout();
					}

					Point point1 = PrintPreviewControlEx.PhysicalToPixels(new Point(this.imageSize), this.screendpi);
					Point virtualSize = new Point(this.VirtualSize);
					Point deviceSize = new Point(Math.Max(0, (base.Size.Width - virtualSize.X) / 2), Math.Max(0, (base.Size.Height - virtualSize.Y) / 2));
					deviceSize.X -= this.Position.X;
					deviceSize.Y -= this.Position.Y;

					this.lastOffset = deviceSize;
					
					int screenWidth = PrintPreviewControlEx.PhysicalToPixels(10, this.screendpi.X);
					int screenHeight = PrintPreviewControlEx.PhysicalToPixels(10, this.screendpi.Y);
					
					Region clipRegion = pevent.Graphics.Clip;
					Rectangle[] pageRects = new Rectangle[this.rows * this.columns];
					
					try
					{
						for (int row = 0; row < this.rows; row++)
						{
							for (int col = 0; col < this.columns; col++)
							{
								int pageIndex = (this.StartPage + col) + (row * this.columns);
								if (pageIndex < this.pageInfo.Length)
								{
									int left = (deviceSize.X + (screenWidth * (col + 1))) + (point1.X * col);
									int top = (deviceSize.Y + (screenHeight * (row + 1))) + (point1.Y * row);
									pageRects[pageIndex - this.StartPage] = new Rectangle(left, top, point1.X, point1.Y);
									pevent.Graphics.ExcludeClip(pageRects[pageIndex - this.StartPage]);
								}
							}
						}
						pevent.Graphics.FillRectangle(brBckgnd, base.ClientRectangle);
					}
					finally
					{
						pevent.Graphics.Clip = clipRegion;
					}

					for (int pageIndex = 0; pageIndex < pageRects.Length; pageIndex++)
					{
						if ((pageIndex + this.StartPage) < this.pageInfo.Length)
						{
							Rectangle pageRect = pageRects[pageIndex];
							pevent.Graphics.DrawRectangle(Pens.Black, pageRect);
							pageRect.Inflate(-1, -1);
							pevent.Graphics.FillRectangle(new SolidBrush(this.ForeColor), pageRect);
							
							if (this.pageInfo[pageIndex + this.StartPage].Image != null)
							{
								pevent.Graphics.DrawImage(this.pageInfo[pageIndex + this.StartPage].Image, pageRect);
							}
							
							pageRect.Width--;
							pageRect.Height--;
							pevent.Graphics.DrawRectangle(Pens.Black, pageRect);
						}
					}
				}
			}
			finally
			{
				brBckgnd.Dispose();
			}

			Label_03D5:
				base.OnPaint(pevent);
		}

		protected override void OnResize(EventArgs eventargs)
		{
			if (this.autoZoom)
			{
				this.InvalidateLayout();
			}
			else
			{
				PrintPreviewControlEx.PhysicalToPixels(new Point(this.imageSize), this.screendpi);
				Point point1 = new Point(this.VirtualSize);
				Point point2 = new Point(Math.Max(0, (base.Size.Width - point1.X) / 2), Math.Max(0, (base.Size.Height - point1.Y) / 2));
				point2.X -= this.Position.X;
				point2.Y -= this.Position.Y;
				if ((this.lastOffset.X != point2.X) || (this.lastOffset.Y != point2.Y))
				{
					base.Invalidate();
				}
			}
			base.OnResize(eventargs);
		}

		protected virtual void OnStartPageChanged(EventArgs e)
		{
			EventHandler handler = base.Events[PrintPreviewControlEx.EVENT_STARTPAGECHANGED] as EventHandler;
			if (handler != null)
			{
				handler(this, e);
			}
		}


		protected virtual void ComputeLayout()
		{
			this.layoutOk = true;

			if (this.pageInfo.Length == 0)
			{
				base.ClientSize = base.Size;
			}
			else
			{
				using (Graphics graph = this.CreateGraphicsInternal())
				{
					IntPtr hDC = graph.GetHdc();
					this.screendpi = new Point(SafeNativeMethods.GetDeviceCaps(new HandleRef(graph, hDC), 0x58), SafeNativeMethods.GetDeviceCaps(new HandleRef(graph, hDC), 90));
					graph.ReleaseHdcInternal(hDC);
				}

				Size physicalSize = this.pageInfo[this.StartPage].PhysicalSize;
				Size pixelSize = new Size(PrintPreviewControlEx.PixelsToPhysical(new Point(base.Size), this.screendpi));
				if (this.autoZoom)
				{
					double scaleDx = (pixelSize.Width - (10 * (this.columns + 1))) / ((double) (this.columns * physicalSize.Width));
					double scaleDy = (pixelSize.Height - (10 * (this.rows + 1))) / ((double) (this.rows * physicalSize.Height));
					this.zoom = Math.Min(scaleDx, scaleDy);
				}

				this.imageSize = new Size((int) (this.zoom * physicalSize.Width), (int) (this.zoom * physicalSize.Height));
				
				int virtualWidth = (this.imageSize.Width * this.columns) + (10 * (this.columns + 1));
				int virtualHeight = (this.imageSize.Height * this.rows) + (10 * (this.rows + 1));
				
				this.SetVirtualSizeNoInvalidate(new Size(PrintPreviewControlEx.PhysicalToPixels(new Point(virtualWidth, virtualHeight), this.screendpi)));
			}
		}

		protected virtual void ComputePreview()
		{
			int startPage = this.StartPage;
			
			if (this.document == null)
			{
				this.pageInfo = new PreviewPageInfo[0];
			}
			else
			{
				PrintController oldController = this.document.PrintController;
				PreviewPrintController previewController = new PreviewPrintController();
				previewController.UseAntiAlias = this.UseAntiAlias;
				
				if (this.autoCalculatePageInfo)
					this.document.PrintController = new PrintControllerWithDlgPrintStatus(previewController, PrintControllerWithStatusDialog_DialogTitlePreview);
				else
					this.document.PrintController = new PrintControllerWithCallbackStatus(previewController, PrintControllerWithStatusDialog_DialogTitlePreview);

				this.document.Print();
				
				this.pageInfo = previewController.GetPreviewPageInfo();
				this.document.PrintController = oldController;
			}

			if (startPage != this.StartPage)
			{
				this.OnStartPageChanged(EventArgs.Empty);
			}
		}

		
		public override void ResetBackColor()
		{
			this.BackColor = SystemColors.AppWorkspace;
		}

		public override void ResetForeColor()
		{
			this.ForeColor = Color.White;
		}

		
		[SecurityPermission(SecurityAction.LinkDemand)]
		protected override void WndProc(ref Message msg)
		{
			switch (msg.Msg)
			{
				case 0x114:
					this.WmHScroll(ref msg);
					return;

				case 0x115:
					this.WmVScroll(ref msg);
					return;

				case 0x100:
					this.WmKeyDown(ref msg);
					return;
			}
			base.WndProc(ref msg);
		}

				
		#endregion

		#region static methods

		private static Point PhysicalToPixels(Point physical, Point dpi)
		{
			return new Point(PrintPreviewControlEx.PhysicalToPixels(physical.X, dpi.X), PrintPreviewControlEx.PhysicalToPixels(physical.Y, dpi.Y));
		}

		private static Size PhysicalToPixels(Size physicalSize, Point dpi)
		{
			return new Size(PrintPreviewControlEx.PhysicalToPixels(physicalSize.Width, dpi.X), PrintPreviewControlEx.PhysicalToPixels(physicalSize.Height, dpi.Y));
		}

		private static int PhysicalToPixels(int physicalSize, int dpi)
		{
			return (int) (((double) (physicalSize * dpi)) / 100);
		}

		private static Point PixelsToPhysical(Point pixels, Point dpi)
		{
			return new Point(PrintPreviewControlEx.PixelsToPhysical(pixels.X, dpi.X), PrintPreviewControlEx.PixelsToPhysical(pixels.Y, dpi.Y));
		}

		private static Size PixelsToPhysical(Size pixels, Point dpi)
		{
			return new Size(PrintPreviewControlEx.PixelsToPhysical(pixels.Width, dpi.X), PrintPreviewControlEx.PixelsToPhysical(pixels.Height, dpi.Y));
		}

		private static int PixelsToPhysical(int pixels, int dpi)
		{
			return (int) ((pixels * 100) / ((double) dpi));
		}


		internal static StringAlignment TranslateAlignment(ContentAlignment align)
		{
			ContentAlignment anyRight = ContentAlignment.BottomRight | ContentAlignment.MiddleRight | ContentAlignment.TopRight;
			ContentAlignment anyCenter = ContentAlignment.BottomCenter | ContentAlignment.MiddleCenter | ContentAlignment.TopCenter;
			
			if ((align & anyRight) != ((ContentAlignment) 0))
			{
				return StringAlignment.Far;
			}
			if ((align & anyCenter) != ((ContentAlignment) 0))
			{
				return StringAlignment.Center;
			}
			return StringAlignment.Near;
		}

		internal static StringAlignment TranslateLineAlignment(ContentAlignment align)
		{
			ContentAlignment anyBottom = ContentAlignment.BottomRight | ContentAlignment.BottomCenter | ContentAlignment.BottomLeft;
			ContentAlignment anyMiddle = ContentAlignment.MiddleRight | ContentAlignment.MiddleCenter | ContentAlignment.MiddleLeft;
			
			if ((align & anyBottom) != ((ContentAlignment) 0))
			{
				return StringAlignment.Far;
			}
			if ((align & anyMiddle) != ((ContentAlignment) 0))
			{
				return StringAlignment.Center;
			}
			return StringAlignment.Near;
		}

		#endregion

		#region private methods

		private int AdjustScroll(Message msg, int pos, int maxPos)
		{
			switch (NativeMethods.LOWORD(msg.WParam.ToInt32()))
			{
				case 0:
					if (pos > 5)
					{
						pos -= 5;
						return pos;
					}
					pos = 0;
					return pos;

				case 1:
					if (pos < (maxPos - 5))
					{
						pos += 5;
						return pos;
					}
					pos = maxPos;
					return pos;

				case 2:
					if (pos > 100)
					{
						pos -= 100;
						return pos;
					}
					pos = 0;
					return pos;

				case 3:
					if (pos < (maxPos - 100))
					{
						pos += 100;
						return pos;
					}
					pos = maxPos;
					return pos;

				case 4:
				case 5:
					pos = NativeMethods.HIWORD(msg.WParam.ToInt32());
					return pos;
			}
			return pos;
		}

		private void InvalidateLayout()
		{
			this.layoutOk = false;
			base.Invalidate();
		}


		#endregion

		#region internal helpers

		internal Graphics CreateGraphicsInternal()
		{
			return Graphics.FromHwndInternal(this.Handle);
		}

		private void SetPositionNoInvalidate(Point value)
		{
			Point point1 = this.position;
			this.position = value;
			this.position.X = Math.Min(this.position.X, this.virtualSize.Width - base.Width);
			this.position.Y = Math.Min(this.position.Y, this.virtualSize.Height - base.Height);
			if (this.position.X < 0)
			{
				this.position.X = 0;
			}
			if (this.position.Y < 0)
			{
				this.position.Y = 0;
			}
			Rectangle rectangle1 = base.ClientRectangle;
			NativeMethods.RECT rect1 = NativeMethods.RECT.FromXYWH(rectangle1.X, rectangle1.Y, rectangle1.Width, rectangle1.Height);
			UnsafeNativeMethods.ScrollWindow(new HandleRef(this, base.Handle), point1.X - this.position.X, point1.Y - this.position.Y, ref rect1, ref rect1);
			UnsafeNativeMethods.SetScrollPos(new HandleRef(this, base.Handle), 0, this.position.X, true);
			UnsafeNativeMethods.SetScrollPos(new HandleRef(this, base.Handle), 1, this.position.Y, true);
		}

		internal void SetVirtualSizeNoInvalidate(Size value)
		{
			this.virtualSize = value;
			this.SetPositionNoInvalidate(this.position);
			NativeMethods.SCROLLINFO scrollinfo1 = new NativeMethods.SCROLLINFO();
			scrollinfo1.fMask = 3;
			scrollinfo1.nMin = 0;
			scrollinfo1.nMax = Math.Max(base.Height, this.virtualSize.Height) - 1;
			scrollinfo1.nPage = base.Height;
			UnsafeNativeMethods.SetScrollInfo(new HandleRef(this, base.Handle), 1, scrollinfo1, true);
			scrollinfo1.fMask = 3;
			scrollinfo1.nMin = 0;
			scrollinfo1.nMax = Math.Max(base.Width, this.virtualSize.Width) - 1;
			scrollinfo1.nPage = base.Width;
			UnsafeNativeMethods.SetScrollInfo(new HandleRef(this, base.Handle), 0, scrollinfo1, true);
		}

		internal bool ShouldSerializeBackColor()
		{
			return !this.BackColor.Equals(SystemColors.AppWorkspace);
		}

		internal bool ShouldSerializeForeColor()
		{
			return !this.ForeColor.Equals(Color.White);
		}

		
		private void WmHScroll(ref Message msg)
		{
			if (msg.LParam != IntPtr.Zero)
			{
				base.WndProc(ref msg);
			}
			else
			{
				Point point1 = this.position;
				int num1 = point1.X;
				int num2 = Math.Max(base.Width, this.virtualSize.Width);
				point1.X = this.AdjustScroll(msg, num1, num2);
				this.Position = point1;
			}
		}

		private void WmKeyDown(ref Message msg)
		{
			Keys keys1 = ((Keys) ((int) msg.WParam)) | Control.ModifierKeys;
			switch ((keys1 & Keys.KeyCode))
			{
				case Keys.Prior:
					this.StartPage--;
					return;

				case Keys.Next:
					this.StartPage++;
					return;

				case Keys.End:
					if ((keys1 & ~Keys.KeyCode) == Keys.Control)
					{
						this.StartPage = this.pageInfo.Length;
					}
					return;

				case Keys.Home:
					if ((keys1 & ~Keys.KeyCode) == Keys.Control)
					{
						this.StartPage = 0;
						return;
					}
					return;
			}
		}

		private void WmVScroll(ref Message msg)
		{
			if (msg.LParam != IntPtr.Zero)
			{
				base.WndProc(ref msg);
			}
			else
			{
				Point point1 = this.Position;
				int num1 = point1.Y;
				int num2 = Math.Max(base.Height, this.virtualSize.Height);
				point1.Y = this.AdjustScroll(msg, num1, num2);
				this.Position = point1;
			}
		}


		#endregion
		
	}
}

