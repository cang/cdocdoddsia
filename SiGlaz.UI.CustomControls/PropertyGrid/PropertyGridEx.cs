using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace SiGlaz.UI.CustomControls
{
	/// <summary>
	/// Summary description for PropertyGridEx.
	/// </summary>
	public class PropertyGridEx : System.Windows.Forms.UserControl
	{
		#region UI Member fields
		private System.Windows.Forms.PropertyGrid propertyGrid;
		private System.Windows.Forms.ImageList imageList;
		private System.ComponentModel.IContainer components;

		#endregion UI Member fields		

		#region Constructors and Destructors
		public PropertyGridEx()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitComponent call
			Initialize();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(PropertyGridEx));
			this.propertyGrid = new System.Windows.Forms.PropertyGrid();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// propertyGrid
			// 
			this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.propertyGrid.CommandsVisibleIfAvailable = true;
			this.propertyGrid.LargeButtons = false;
			this.propertyGrid.LineColor = System.Drawing.SystemColors.ScrollBar;
			this.propertyGrid.Location = new System.Drawing.Point(1, 1);
			this.propertyGrid.Name = "propertyGrid";
			this.propertyGrid.Size = new System.Drawing.Size(246, 362);
			this.propertyGrid.TabIndex = 0;
			this.propertyGrid.Text = "PropertyGrid";
			this.propertyGrid.ViewBackColor = System.Drawing.SystemColors.Window;
			this.propertyGrid.ViewForeColor = System.Drawing.SystemColors.WindowText;
			// 
			// imageList
			// 
			this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.imageList.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// PropertyGridEx
			// 
			this.Controls.Add(this.propertyGrid);
			this.Name = "PropertyGridEx";
			this.Size = new System.Drawing.Size(248, 364);
			this.ResumeLayout(false);

		}
		#endregion

		#region Member fields
		private Color _borderColor = Color.FromArgb(0, 45, 150);

		private ToolBarEx _toolBar = null;
		private ToolBarItemEx _categorized = null;
		private ToolBarItemEx _alphabetic = null;
		private ToolBarItemEx _hide = null;

		public event EventHandler AutoHideButtonClicked;
		#endregion Member fields

		#region Properties
		public PropertyGrid PropertyGrid
		{
			get { return propertyGrid; }
		}
		#endregion Properties

		#region Initialize
		private void Initialize()
		{
			InitializeToolBar();

			this.SuspendLayout();

			this.Controls.Add(_toolBar);
			_toolBar.BringToFront();

			this.ResumeLayout(false);
		}

		private void InitializeToolBar()
		{
			_toolBar = new ToolBarEx();
			_toolBar.Height = 28;
			_toolBar.Width = propertyGrid.Width;
			_toolBar.Location = new Point(1, 1);
			_toolBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

			_toolBar.BkColorStart = Color.FromArgb(218, 234, 253);
			_toolBar.BkColorEnd = Color.FromArgb(136, 174, 228);

			_toolBar.HasBottomBorder = true;

			// add items
			_categorized = new ToolBarItemEx("Categorized", imageList.Images[0], eToolBarItemExType.Push);
			_categorized.Pushed = true;
			_toolBar.Add(_categorized);

			_alphabetic = new ToolBarItemEx("Alphabetic", imageList.Images[2], eToolBarItemExType.Push);
			_toolBar.Add(_alphabetic);

			_hide = new ToolBarItemEx("Alphabetic", imageList.Images[4]);
			_hide.RightToLeft = true;
			_toolBar.Add(_hide);

			// register event
			_toolBar.ItemClicked += new ToolBarExEventHandlers(_toolBar_ItemClicked);
		}
		#endregion Initialize

		#region Override methods
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint (e);

			using (Pen pen = new Pen(_borderColor, 1.0f))
			{			
				e.Graphics.DrawRectangle(pen, 0, 0, (float)this.Width-1.0f, (float)this.Height-1.0f);
			}
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged (e);

			if (_toolBar != null)
			{
				if (this.Width < 60)
					_toolBar.Visible = false;
				else
					_toolBar.Visible = true;
			}
		}

		#endregion Override methods

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

			if (e.Item == _categorized || e.Item == _alphabetic)
			{
				bool bCategorized = (e.Item == _categorized);
			
				_categorized.Pushed = bCategorized;
				_alphabetic.Pushed = !bCategorized;

				_toolBar.Invalidate(true);
			}
		}
		#endregion Events		
	}
}
