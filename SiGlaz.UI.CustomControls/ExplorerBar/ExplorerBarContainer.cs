using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace SiGlaz.UI.CustomControls
{
	/// <summary>
	/// Summary description for ExplorerBarEx.
	/// </summary>
	public class ExplorerBarContainer : Control//System.Windows.Forms.UserControl
	{
		#region UI Member fields
		private System.ComponentModel.IContainer components;		
		#endregion UI Member fields

		#region Constructors and Destructors
		public ExplorerBarContainer()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
			Initialize();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#endregion Constructors and Destructors

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ExplorerBarContainer));
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			// 
			// imageList
			// 
			this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.imageList.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// ExplorerBarEx
			// 
			this.Name = "ExplorerBarEx";
			this.Size = new System.Drawing.Size(264, 356);

		}
		#endregion

		#region Member fields
		private Color _borderColor = Color.FromArgb(0, 45, 150);

		private ToolBarEx _toolBar = null;
		private ToolBarItemEx _aeroTheme = null;
		private ToolBarItemEx _lunaBlueTheme = null;
		private ToolBarItemEx _allowMultiExpanding = null;
		private ToolBarItemEx _hide = null;
		private System.Windows.Forms.ImageList imageList;

		private ExplorerBar _explorerBar = null;

		public event EventHandler AutoHideButtonClicked;
		#endregion Member fields

		#region Properties
		public ExplorerBar ExplorerBar
		{
			get { return _explorerBar; }
		}
		#endregion Properties

		#region Initialize
		private void Initialize()
		{
			InitializeToolBar();

			InitializeExplorerBar();

			this.SuspendLayout();

			this.Controls.Add(_toolBar);
			this.Controls.Add(_explorerBar);

			this.ResumeLayout(false);
		}

		private void InitializeToolBar()
		{
			_toolBar = new ToolBarEx();
			_toolBar.Height = 28;
			_toolBar.Width = this.Width-2;
			_toolBar.Location = new Point(1, 1);
			//_toolBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _toolBar.Dock = DockStyle.Top;

			_toolBar.BkColorStart = Color.FromArgb(218, 234, 253);
			_toolBar.BkColorEnd = Color.FromArgb(136, 174, 228);

            //_toolBar.HasTopBorder = true;
            _toolBar.HasLeftBorder = true;
            _toolBar.HasRightBorder = true;
			//_toolBar.HasBottomBorder = false;

			// add items
			
			//_aeroTheme = new ToolBarItemEx("Aero Theme", null, eToolBarItemExType.Push);
			//_aeroTheme.Pushed = false;			
			//_toolBar.Add(_aeroTheme);

			//_lunaBlueTheme = new ToolBarItemEx("Luna Blue Theme", null, eToolBarItemExType.Push);
			//_lunaBlueTheme.Pushed = true;
			//_toolBar.Add(_lunaBlueTheme);

			//ToolBarItemEx separator = new ToolBarItemEx("", null, eToolBarItemExType.Separator);
			//_toolBar.Add(separator);

			_allowMultiExpanding = new ToolBarItemEx("Allow Multi Expanding", imageList.Images[2], eToolBarItemExType.Push);
			_allowMultiExpanding.Pushed = false;
			_toolBar.Add(_allowMultiExpanding);

			_hide = new ToolBarItemEx("Hide", imageList.Images[0]);
			_hide.RightToLeft = true;
			_toolBar.Add(_hide);

			// register event
			_toolBar.ItemClicked += new ToolBarExEventHandlers(_toolBar_ItemClicked);
		}

		private void InitializeExplorerBar()
		{
			_explorerBar = new ExplorerBar();
			_explorerBar.Size = new Size(this.Width, this.Height - _toolBar.Height);
			_explorerBar.Location = new Point(0, _toolBar.Bottom-1);
			_explorerBar.Anchor = 
				AnchorStyles.Left | AnchorStyles.Right | 
				AnchorStyles.Top | AnchorStyles.Bottom;
		}
		#endregion Initialize

		#region Overrides
        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    base.OnLoad (e);

        //    using (Pen pen = new Pen(_borderColor, 1.0f))
        //    {			
        //        e.Graphics.DrawRectangle(pen, 0, 0, (float)this.Width-1.0f, (float)this.Height+3);
        //    }
        //}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged (e);

			if (_toolBar != null)
			{
				if (this.Width < 80)
					_toolBar.Visible = false;
				else
					_toolBar.Visible = true;
			}
		}
		#endregion Overrides

		#region Events
		private void _toolBar_ItemClicked(object sender, ToolBarExEventArgs e)
		{
			if (e.Item == null)
				return;

			if (e.Item == _hide)
			{
				if (AutoHideButtonClicked != null)
					AutoHideButtonClicked(this, EventArgs.Empty);

				return;
			}

			if (e.Item == _allowMultiExpanding)
			{
				if (_allowMultiExpanding.Pushed)
				{
					_allowMultiExpanding.Pushed = false;
					_explorerBar.AllowMultiExpanding = false;
				}
				else
				{
					_allowMultiExpanding.Pushed = true;
					_explorerBar.AllowMultiExpanding = true;					
				}
				return;
			}

			if (e.Item == _aeroTheme || e.Item == _lunaBlueTheme)
			{
				bool bAeroTheme = (e.Item == _aeroTheme);
			
				_aeroTheme.Pushed = bAeroTheme;
				_lunaBlueTheme.Pushed = !bAeroTheme;

				if (bAeroTheme)
					_explorerBar.Theme = new ExplorerBarAeroTheme();
				else
					_explorerBar.Theme = new ExplorerBarLunaBlueTheme();

				_explorerBar.Invalidate(true);

				_toolBar.Invalidate(true);

				this.Invalidate(true);
			}
		}
		#endregion Events

		#region Methods
		#endregion Methods

		#region Helpers
		#endregion Helpers
	}
}
