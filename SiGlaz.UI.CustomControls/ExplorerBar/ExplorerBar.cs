using System;
using System.Drawing;
using System.Windows.Forms;

namespace SiGlaz.UI.CustomControls
{
	//public class ExplorerBar : Control
	public class ExplorerBar : Panel
	{
		#region Member fields
		private ExplorerBarController _controller = null;

		public event ExplorerBarEventHandler ItemClicked;
        public event EventHandler BeforeDragDrop;
        //public event ExplorerBarEventHandler DragDrop;
        public event EventHandler FinishedDragDrop;
        public bool AllowDrag = false;
		#endregion Member fields

		#region Properties
		public IExplorerBarTheme Theme
		{
			get { return _controller.Theme; }
			set
			{
				_controller.Theme = value;
			}
		}

		public bool AllowMultiExpanding
		{
			get { return _controller.AllowMultiExpanding; }
			set
			{
				_controller.AllowMultiExpanding = value;
			}
		}
		#endregion Properties

		#region Constructors and Destructors
		public ExplorerBar() : base()
		{
			_controller = new ExplorerBarController(this);

			_controller.RedrawRequested += new EventHandler(_controller_RedrawRequested);
			_controller.DefaultCursorAssigned += new EventHandler(_controller_DefaultCursorAssigned);
			_controller.HandCurosrAssigned += new EventHandler(_controller_HandCurosrAssigned);
			_controller.ItemClicked += new ExplorerBarEventHandler(_controller_ItemClicked);
            //_controller.DragDrop += new ExplorerBarEventHandler(_controller_DragDrop);

			this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
			this.UpdateStyles();
		}
		#endregion Constructors and Destructors

		#region Overrides
		protected override void OnPaint(PaintEventArgs e)
		{
			if (e.ClipRectangle.Width < 90)
				base.OnPaint (e);
			else
				_controller.OnPaint(e);
		}

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (this.AllowDrag)
                _controller.OnMouseDown(e);
        }

		protected override void OnMouseMove(MouseEventArgs e)
		{
			//base.OnMouseMove (e);
			_controller.OnMouseMove(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			//base.OnMouseLeave (e);
			_controller.OnMouseLeave(e);
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			//base.OnMouseWheel (e);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged (e);

			if (this.Size.Width < 90)
				_controller.Size = new Size(this.Size.Width, this.Size.Height);
			else				
				_controller.Size = this.Size;
		}

		protected override void OnClick(EventArgs e)
		{
			_controller.OnClick(e);
		}        
		#endregion Overrides

		#region Events
		private void _controller_RedrawRequested(object sender, EventArgs e)
		{
			this.Invalidate(true);
		}

		private void _controller_DefaultCursorAssigned(object sender, EventArgs e)
		{
			this.Cursor = Cursors.Default;
		}

		private void _controller_HandCurosrAssigned(object sender, EventArgs e)
		{
			this.Cursor = Cursors.Hand;
		}

		private void _controller_ItemClicked(object sender, ExplorerBarEventArgs e)
		{
			if (ItemClicked != null)
				ItemClicked(this, e);
		}

        void _controller_DragDrop(object sender, ExplorerBarEventArgs e)
        {
            //if (DragDrop != null)
            //{
            //    if (BeforeDragDrop != null)
            //        BeforeDragDrop(this, EventArgs.Empty);

            //    DragDrop(this, new ExplorerBarEventArgs(e.Item));

            //    if (FinishedDragDrop != null)
            //        FinishedDragDrop(this, EventArgs.Empty);
            //}
        }
		#endregion Events

		#region Methods
        public void Redraw()
        {
            if (_controller != null)
            {
                if (_controller.ClearCache())
                    this.Invalidate(true);
            }            
        }

		public void AddItem(ExplorerItemsGroup group, ExplorerItem item)
		{
			_controller.AddItem(group, item);
		}

		public void AddGroup(ExplorerItemsGroup group)
		{
			_controller.AddGroup(group);
		}
		#endregion Methods

		#region Helpers
		#endregion Helpers		
	}
}
