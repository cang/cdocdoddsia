using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;


namespace SIA.UI.Components.Printing
{
    /// <summary>
    /// <para> Represents a user control that contains a <see cref="T:System.Windows.Forms.PrintPreviewControl" />.</para>
    /// </summary>
    [ToolboxItem(true), DefaultProperty("Document"), Designer("System.ComponentModel.Design.ComponentDesigner, System.Design, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"), DesignTimeVisible(true)]
    public class ucPrintPreview : System.Windows.Forms.UserControl
    {
		#region constants

		private const string PrintPreviewDialog_ZoomAuto = "Auto";
		private const string PrintPreviewDialog_Zoom500 = "500%";
		private const string PrintPreviewDialog_Zoom200 = "200%";
		private const string PrintPreviewDialog_Zoom150 = "150%";
		private const string PrintPreviewDialog_Zoom100 = "100%";
		private const string PrintPreviewDialog_Zoom75 = "75%";
		private const string PrintPreviewDialog_Zoom50 = "50%";
		private const string PrintPreviewDialog_Zoom25 = "25%";
		private const string PrintPreviewDialog_Zoom10 = "10%";
		private const string PrintPreviewDialog_Zoom = "Zoom";
		private const string PrintPreviewDialog_PrintPreview = "Print Preview";
		private const string PrintPreviewDialog_Close = "Close";
		private const string PrintPreviewDialog_Page = "Page";
		private const string PrintPreviewDialog_Print = "Print";
		private const string PrintPreviewDialog_OnePage = "One page";
		private const string PrintPreviewDialog_TwoPages = "Two pages";
		private const string PrintPreviewDialog_ThreePages = "Three pages";
		private const string PrintPreviewDialog_FourPages = "Four pages";
		private const string PrintPreviewDialog_SixPages = "Six pages";

		#endregion

		#region constructor and destructor

        static ucPrintPreview()
        {
            ucPrintPreview.DefaultMinimumSize = new System.Drawing.Size(0x177, 250);
        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="T:System.Windows.Forms.ucPrintPreview" /> class.</para>
        /// </summary>
        public ucPrintPreview()
        {
            this.menu = new System.Windows.Forms.ContextMenu();
            
			this.InitializeComponents();

            Bitmap bitmap = new Bitmap(typeof(ucPrintPreview), "PrintPreviewStrip.bmp");
            bitmap.MakeTransparent();
            this.imageList.Images.AddStrip(bitmap);            
        }

		#endregion

		#region methods

        private void CheckZoomMenu(MenuItem toChecked)
        {
            foreach (MenuItem item in this.menu.MenuItems)
            {
                item.Checked = toChecked == item;
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            //base.Close();
			if (this.CloseButtonClick != null)
				this.CloseButtonClick(this, e);
        }

        /// <summary>
        /// <para> Creates the handle for the form that encapsulates the
        /// <see cref="T:System.Windows.Forms.ucPrintPreview" />
        /// .</para>
        /// </summary>
        /// <exception cref="T:System.Drawing.Printing.InvalidPrinterException">The printer settings in <see cref="P:System.Windows.Forms.ucPrintPreview.Document" /> are not valid.</exception>
        protected override void CreateHandle()
        {
            if ((this.Document != null) && !this.Document.PrinterSettings.IsValid)
            {
                throw new InvalidPrinterException(this.Document.PrinterSettings);
            }

            base.CreateHandle();
        }

        private void InitializeComponents()
        {
            this.singlePage = new ToolBarButton();
            this.zoomButton = new ToolBarButton();
            this.closeButton = new Button();
            this.tbSep01 = new ToolBarButton();
            this.tbSep02 = new ToolBarButton();
            this.pageLabel = new Label();
            this.pageCounter = new NumericUpDown();
            this.toolBar = new ToolBar();
            this.previewControl = new PrintPreviewControlEx();
            this.printButton = new ToolBarButton();
            this.twoPages = new ToolBarButton();
            this.threePages = new ToolBarButton();
            this.fourPages = new ToolBarButton();
            this.sixPages = new ToolBarButton();
            this.imageList = new ImageList();
            this.singlePage.ToolTipText = PrintPreviewDialog_OnePage;
            this.singlePage.ImageIndex = 2;
            this.toolBar.ImageList = this.imageList;
            this.toolBar.Dock = DockStyle.Top;
            this.toolBar.Appearance = ToolBarAppearance.Flat;
            this.tbSep01.Style = ToolBarButtonStyle.Separator;
            this.tbSep02.Style = ToolBarButtonStyle.Separator;
            this.zoomMenu0 = new MenuItem(PrintPreviewDialog_ZoomAuto, new EventHandler(this.ZoomAuto));
            this.zoomMenu1 = new MenuItem(PrintPreviewDialog_Zoom500, new EventHandler(this.Zoom500));
            this.zoomMenu2 = new MenuItem(PrintPreviewDialog_Zoom200, new EventHandler(this.Zoom200));
            this.zoomMenu3 = new MenuItem(PrintPreviewDialog_Zoom150, new EventHandler(this.Zoom150));
            this.zoomMenu4 = new MenuItem(PrintPreviewDialog_Zoom100, new EventHandler(this.Zoom100));
            this.zoomMenu5 = new MenuItem(PrintPreviewDialog_Zoom75, new EventHandler(this.Zoom75));
            this.zoomMenu6 = new MenuItem(PrintPreviewDialog_Zoom50, new EventHandler(this.Zoom50));
            this.zoomMenu7 = new MenuItem(PrintPreviewDialog_Zoom25, new EventHandler(this.Zoom25));
            this.zoomMenu8 = new MenuItem(PrintPreviewDialog_Zoom10, new EventHandler(this.Zoom10));
            this.zoomMenu0.Checked = true;
            this.menu.MenuItems.AddRange(new MenuItem[] { this.zoomMenu0, this.zoomMenu1, this.zoomMenu2, this.zoomMenu3, this.zoomMenu4, this.zoomMenu5, this.zoomMenu6, this.zoomMenu7, this.zoomMenu8 });
            this.zoomButton.ToolTipText = PrintPreviewDialog_Zoom;
            this.zoomButton.ImageIndex = 1;
            this.zoomButton.Style = ToolBarButtonStyle.DropDownButton;
            this.zoomButton.DropDownMenu = this.menu;
            this.Text = PrintPreviewDialog_PrintPreview;
            base.ClientSize = new System.Drawing.Size(400, 300);
            this.closeButton.Location = new Point(0xc4, 2);
            this.closeButton.Size = new System.Drawing.Size(50, 20);
            this.closeButton.TabIndex = 2;
            this.closeButton.FlatStyle = FlatStyle.Popup;
            this.closeButton.Text = PrintPreviewDialog_Close;
            this.closeButton.Click += new EventHandler(this.closeButton_Click);
            this.pageLabel.Text = PrintPreviewDialog_Page;
            this.pageLabel.TabStop = false;
            this.pageLabel.Location = new Point(510, 4);
            this.pageLabel.Size = new System.Drawing.Size(50, 0x18);
            this.pageLabel.TextAlign = ContentAlignment.MiddleLeft;
            this.pageLabel.Dock = DockStyle.Right;
            this.pageCounter.TabIndex = 1;
            this.pageCounter.Text = "1";
            this.pageCounter.TextAlign = HorizontalAlignment.Right;
            this.pageCounter.DecimalPlaces = 0;
            this.pageCounter.Minimum = new decimal(0);
            this.pageCounter.Maximum = new decimal(1000);
            this.pageCounter.ValueChanged += new EventHandler(this.UpdownMove);
            this.pageCounter.Size = new System.Drawing.Size(0x40, 20);
            this.pageCounter.Dock = DockStyle.Right;
            this.pageCounter.Location = new Point(0x238, 0);
            this.toolBar.TabIndex = 3;
            this.toolBar.Size = new System.Drawing.Size(0x318, 0x2b);
            this.toolBar.ShowToolTips = true;
            this.toolBar.DropDownArrows = true;
            this.toolBar.Buttons.AddRange(new ToolBarButton[] { this.printButton, this.zoomButton, this.tbSep01, this.singlePage, this.twoPages, this.threePages, this.fourPages, this.sixPages, this.tbSep02 });
            this.toolBar.ButtonClick += new ToolBarButtonClickEventHandler(this.ToolBarClick);
            this.previewControl.TabIndex = 1;
            this.previewControl.Size = new System.Drawing.Size(0x318, 610);
            this.previewControl.Location = new Point(0, 0x2b);
            this.previewControl.Dock = DockStyle.Fill;
            this.previewControl.StartPageChanged += new EventHandler(this.previewControl_StartPageChanged);
            this.printButton.ToolTipText = PrintPreviewDialog_Print;
            this.printButton.ImageIndex = 0;
            this.twoPages.ToolTipText = PrintPreviewDialog_TwoPages;
            this.twoPages.ImageIndex = 3;
            this.threePages.ToolTipText = PrintPreviewDialog_ThreePages;
            this.threePages.ImageIndex = 4;
            this.fourPages.ToolTipText = PrintPreviewDialog_FourPages;
            this.fourPages.ImageIndex = 5;
            this.sixPages.ToolTipText = PrintPreviewDialog_SixPages;
            this.sixPages.ImageIndex = 6;
            base.Controls.Add(this.previewControl);
            base.Controls.Add(this.toolBar);
            this.toolBar.Controls.Add(this.pageLabel);
            this.toolBar.Controls.Add(this.pageCounter);
            this.toolBar.Controls.Add(this.closeButton);
        }

//        protected override void OnClosing(CancelEventArgs e)
//        {
//            base.OnClosing(e);
//            this.previewControl.InvalidatePreview();
//        }

        private void previewControl_StartPageChanged(object sender, EventArgs e)
        {
            this.pageCounter.Value = (decimal) (this.previewControl.StartPage + 1);
        }

        
		private void ToolBarClick(object source, ToolBarButtonClickEventArgs eventargs)
        {
            if (eventargs.Button == this.printButton)
            {
				if (this.Print != null)
					this.Print(this, EventArgs.Empty);
				else if (this.previewControl != null)
					this.previewControl.Print();
            }
            else if (eventargs.Button == this.zoomButton)
            {
                this.ZoomAuto(null, EventArgs.Empty);
            }
            else if (eventargs.Button == this.singlePage)
            {
                this.previewControl.Rows = 1;
                this.previewControl.Columns = 1;
            }
            else if (eventargs.Button == this.twoPages)
            {
                this.previewControl.Rows = 1;
                this.previewControl.Columns = 2;
            }
            else if (eventargs.Button == this.threePages)
            {
                this.previewControl.Rows = 1;
                this.previewControl.Columns = 3;
            }
            else if (eventargs.Button == this.fourPages)
            {
                this.previewControl.Rows = 2;
                this.previewControl.Columns = 2;
            }
            else if (eventargs.Button == this.sixPages)
            {
                this.previewControl.Rows = 2;
                this.previewControl.Columns = 3;
            }
        }

        private void UpdownMove(object sender, EventArgs eventargs)
        {
            this.previewControl.StartPage = ((int) this.pageCounter.Value) - 1;
        }


        private void Zoom10(object sender, EventArgs eventargs)
        {
            this.CheckZoomMenu(this.zoomMenu8);
            this.previewControl.Zoom = 0.1;
        }

		private void Zoom25(object sender, EventArgs eventargs)
		{
			this.CheckZoomMenu(this.zoomMenu7);
			this.previewControl.Zoom = 0.25;
		}

		private void Zoom50(object sender, EventArgs eventargs)
		{
			this.CheckZoomMenu(this.zoomMenu6);
			this.previewControl.Zoom = 0.5;
		}

		private void Zoom75(object sender, EventArgs eventargs)
		{
			this.CheckZoomMenu(this.zoomMenu5);
			this.previewControl.Zoom = 0.75;
		}

		private void Zoom100(object sender, EventArgs eventargs)
        {
            this.CheckZoomMenu(this.zoomMenu4);
            this.previewControl.Zoom = 1;
        }

        private void Zoom150(object sender, EventArgs eventargs)
        {
            this.CheckZoomMenu(this.zoomMenu3);
            this.previewControl.Zoom = 1.5;
        }

        private void Zoom200(object sender, EventArgs eventargs)
        {
            this.CheckZoomMenu(this.zoomMenu2);
            this.previewControl.Zoom = 2.0;
        }

        private void Zoom500(object sender, EventArgs eventargs)
        {
            this.CheckZoomMenu(this.zoomMenu1);
            this.previewControl.Zoom = 5;
        }

        private void ZoomAuto(object sender, EventArgs eventargs)
        {
            this.CheckZoomMenu(this.zoomMenu0);
            this.previewControl.AutoZoom = true;
        }

		#endregion

		#region events

		public event EventHandler CloseButtonClick = null;
		public event EventHandler Print = null;
		
		#endregion

		#region properties

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public PrintPreviewControlEx PrintPreviewControl
		{
			get
			{
				return this.previewControl;
			}
		}
        
        public PrintDocument Document
        {
            get
            {
                return this.previewControl.Document;
            }
            set
            {
                this.previewControl.Document = value;
            }
        }


        public bool UseAntiAlias
        {
            get
            {
                return this.PrintPreviewControl.UseAntiAlias;
            }
            set
            {
                this.PrintPreviewControl.UseAntiAlias = value;
            }
        }

		
		#endregion
       
		#region fields

        private new static readonly System.Drawing.Size DefaultMinimumSize;

		private Button closeButton;
        
		private ImageList imageList;
        private NumericUpDown pageCounter;
        private Label pageLabel;
        private PrintPreviewControlEx previewControl;
		
		private ToolBar toolBar;
		private ToolBarButton fourPages;
		private ToolBarButton printButton;
        private ToolBarButton tbSep01;
        private ToolBarButton tbSep02;
        private ToolBarButton singlePage;
        private ToolBarButton sixPages;
        private ToolBarButton threePages;
        private ToolBarButton twoPages;
        private ToolBarButton zoomButton;
        
		private MenuItem zoomMenu0;
        private MenuItem zoomMenu1;
        private MenuItem zoomMenu2;
        private MenuItem zoomMenu3;
        private MenuItem zoomMenu4;
        private MenuItem zoomMenu5;
        private MenuItem zoomMenu6;
        private MenuItem zoomMenu7;
		private MenuItem zoomMenu8;
		private System.Windows.Forms.ContextMenu menu;

		#endregion
    }
}

