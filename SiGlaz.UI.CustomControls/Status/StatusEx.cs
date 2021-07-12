using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SiGlaz.UI.CustomControls
{
	public class SSAStatus : StatusEx
	{
		public StateEx _state = null;
		public OpenFileStateEx _openFile = null;
		public ViewerSizeStatusEx _viewerSize = null;
		public MousePositionStatusEx _mousePosition = null;
		public SeparatorStatusEx _separator1 = null;
		public SeparatorStatusEx _separator2 = null;
		public SeparatorStatusEx _separator3 = null;

		public SSAStatus() : base()
		{
			Initialize();
		}

		private void Initialize()
		{
			int x = this.Width - 2;
			Rectangle rect;

			// mouse position
			_mousePosition = new MousePositionStatusEx(Rectangle.Empty, PointF.Empty, null);
			rect = _mousePosition.Bounds;
			_mousePosition.Bounds = new Rectangle(x - rect.Width, rect.Top, rect.Width, rect.Height);			
			//_items.Add(_mousePosition);
			AddItem(_mousePosition);

			x -= _mousePosition.Bounds.Width + 2;

			// separator 1
			_separator1 = new SeparatorStatusEx(Rectangle.Empty);
			rect = _separator1.Bounds;
			_separator1.Bounds = new Rectangle(x - rect.Width, rect.Top, rect.Width, rect.Height);
			AddItem(_separator1);

			x -= _separator1.Bounds.Width + 2;

			// viewer size
			_viewerSize = new ViewerSizeStatusEx(Rectangle.Empty, PointF.Empty, null);
			rect = _viewerSize.Bounds;
			_viewerSize.Bounds = new Rectangle(x - rect.Width, rect.Top, rect.Width, rect.Height);			
			AddItem(_viewerSize);

			x -= _viewerSize.Bounds.Width + 2;
			
			// separator 2						
			_separator2 = new SeparatorStatusEx(Rectangle.Empty);
			rect = _separator2.Bounds;
			_separator2.Bounds = new Rectangle(x - rect.Width, rect.Top, rect.Width, rect.Height);
			AddItem(_separator2);

			x -= _separator2.Bounds.Width + 2;

			// open file
			_openFile = new OpenFileStateEx(Rectangle.Empty, string.Empty, null);
			rect = _openFile.Bounds;
			_openFile.Bounds = new Rectangle(x - rect.Width, rect.Top, rect.Width, rect.Height);			
			AddItem(_openFile);

			x -= _viewerSize.Bounds.Width + 2;

			// separator 3
			_separator3 = new SeparatorStatusEx(Rectangle.Empty);
			rect = _separator3.Bounds;
			_separator3.Bounds = new Rectangle(x - rect.Width, rect.Top, rect.Width, rect.Height);
			AddItem(_separator3);

			x -= _separator3.Bounds.Width + 2;

//			// state
//			_state = new MousePositionStatusEx(Rectangle.Empty, PointF.Empty, null);
//			rect = _state.Bounds;
//			_state.Bounds = new Rectangle(x - rect.Width, rect.Top, rect.Width, rect.Height);			
//			AddItem(_state);
//
//			x -= _state.Bounds.Width + 2;

//
//
//
//			_mousePosition.DataChanged += new EventHandler(statusItem_DataChanged);
//			_viewerSize.DataChanged += new EventHandler(statusItem_DataChanged);
//			_openFile.DataChanged += new EventHandler(statusItem_DataChanged);
//			//_state.DataChanged += new EventHandler(statusItem_DataChanged);
		}

		protected override void CorrectItemBounds()
		{
			base.CorrectItemBounds ();

			int x = this.Width - 2;

			Rectangle rect;

			// mouse position
			if (_mousePosition != null)
			{
				rect = _mousePosition.Bounds;
				_mousePosition.Bounds = new Rectangle(x - rect.Width, rect.Top, rect.Width, rect.Height);

				x -= rect.Width + 2;
			}

			// separator1
			if (_separator1 != null)
			{
				rect = _separator1.Bounds;
				_separator1.Bounds = new Rectangle(x - rect.Width, rect.Top, rect.Width, rect.Height);			

				x -= rect.Width + 2;
			}

			// viewer size
			if (_viewerSize != null)
			{
				rect = _viewerSize.Bounds;
				_viewerSize.Bounds = new Rectangle(x - rect.Width, rect.Top, rect.Width, rect.Height);

				x -= rect.Width + 2;
			}
			// separator2
			if (_separator2 != null)
			{
				rect = _separator2.Bounds;
				_separator2.Bounds = new Rectangle(x - rect.Width, rect.Top, rect.Width, rect.Height);			

				x -= rect.Width + 2;
			}

			// open file
			if (_openFile != null)
			{
				rect = _openFile.Bounds;
				_openFile.Bounds = new Rectangle(x - rect.Width, rect.Top, rect.Width, rect.Height);

				x -= rect.Width + 2;
			}

			// separator3
			if (_separator3 != null)
			{
				rect = _separator3.Bounds;
				_separator3.Bounds = new Rectangle(x - rect.Width, rect.Top, rect.Width, rect.Height);			

				x -= rect.Width + 2;
			}
			
			
			// test
			if (_openFile != null)
			{
				_openFile.FileName = @"D:\WorkingSpace\Projects\BaseSSA\Icons\16x16\klaf1.000";
				_mousePosition.Pt = new PointF(10.333f, 5.333f);
				_viewerSize.Pt = new PointF(1000, 800);
			}
		}
				
	}

	/// <summary>
	/// Summary description for StatusEx.
	/// </summary>
	public class StatusEx : Control
	{
		#region Member fields
		
		private GradientColor _bkColor = 
			new GradientColor(
			Color.White,
			SystemColors.Control
			);

		private Color _borderColor = Color.Blue;

		protected StatusExItemCollection _items = new StatusExItemCollection();

		private Image _cache = null;
		#endregion Member fields

		#region Contructors and Destructors
		public StatusEx()
		{
            this.SetStyle(
                ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
		}
		#endregion Contructors and Destructors

		#region Properties
		public Color BkColorStart
		{
			get { return _bkColor.Start; }
			set 
			{
				if (_bkColor.Start != value)
				{
					_bkColor.Start = value; 
					foreach (StatusExItem item in _items)
					{
						item.BkColorStart = _bkColor.Start;
					}
				}
			}
		}

		public Color BkColorEnd
		{
			get { return _bkColor.End; }
			set 
			{
				if (_bkColor.End != value)
				{
					_bkColor.End = value; 
					foreach (StatusExItem item in _items)
					{
						item.BkColorEnd = _bkColor.End;
					}
				}
			}
		}

		public Color BorderColor
		{
			get { return _borderColor; }
			set { _borderColor = value; }
		}

		public Image Cache
		{
			get { return _cache; }
			set
			{
				if (_cache != value)
				{
					if (_cache != null)
						_cache.Dispose();
					_cache = value;
				}
			}
		}
		#endregion Properties

		#region Overrides methods
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint (e);

			if (_cache == null)
			{
				Rectangle rect = this.ClientRectangle;

				_cache = new Bitmap(rect.Width, rect.Height);

				using (Graphics g = Graphics.FromImage(_cache))
				{

					using (LinearGradientBrush brush = 
							   new LinearGradientBrush(
							   rect, 
							   this.BkColorStart, 
							   this.BkColorEnd, 
							   LinearGradientMode.Vertical))
					{
						g.FillRectangle(brush, rect);
					}

					// draw items
					foreach (StatusExItem item in _items)
					{
						item.Draw(g);
					}

					using (Pen pen = new Pen(this.BorderColor, 1.0f))
					{
						g.DrawRectangle(pen, 0, 0, (float)rect.Width - 1.0f, (float)rect.Height - 1.0f);
					}
				}
			}

			if (_cache != null)
			{
				e.Graphics.DrawImage(_cache, 0, 0);
			}
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged (e);

			this.Cache = null;

			CorrectItemBounds();

			this.Invalidate(true);
		}
		#endregion Overrides methods

		#region Events
		#endregion Events

		#region Methods
		public void AddItem(StatusExItem item)
		{
			_items.Add(item);

			item.BkColorStart = _bkColor.Start;
			item.BkColorEnd = _bkColor.End;			

			CorrectItemBounds();

			item.DataChanged += new EventHandler(item_DataChanged);
		}

		protected virtual void CorrectItemBounds()
		{
		}

		protected void Draw(StatusExItem item)
		{
			if (_cache == null)
				return;

			using (Graphics g = Graphics.FromImage(_cache))
			{
				item.Draw(g);
			}

			this.Invalidate(true);
		}
		#endregion Methods

		#region Helpers
		#endregion Helpers

		private void item_DataChanged(object sender, EventArgs e)
		{
			if (sender == null)
				return;

			Draw(sender as StatusExItem);
		}
	}


	public class StatusExItemCollection : CollectionBase
	{
		public event EventHandler ItemAdded;

		public int Add(StatusExItem item)
		{
			if (item == null)
				return - 1;
	
			int indx = this.InnerList.Add(item);

			if (ItemAdded != null)
				ItemAdded(this, EventArgs.Empty);

			return indx;
		}

		public StatusExItem this[int index]
		{
			get { return this.InnerList[index] as StatusExItem; }
			set { this.InnerList[index] = value; }
		}

		public void Remove(StatusExItem item)
		{
			if (item == null)
				return;
			this.InnerList.Remove(item);
		}

		public StatusExItem GetItemContains(int x, int y)
		{
			foreach (StatusExItem item in this.InnerList)
			{
				if (item.Contains(x, y))
					return item;
			}
			return null;
		}
	}

	public abstract class StatusExItem
	{
		#region Member fields
		protected bool _visible = true;
		protected string _name = string.Empty;
		protected Rectangle _bounds = Rectangle.Empty;
		protected Color _bkColorStart = Color.White;
		protected Color _bkColorEnd = SystemColors.Control;
		protected Color _foreColor = Color.Blue;
		protected Font _font = new Font("Tahoma", 8, FontStyle.Regular);
		protected StringFormat _strFormat = new StringFormat();
		protected bool _fixedSize = true;

		public event EventHandler DataChanged;
		#endregion Member fields

		#region Constructors and Destructors
		public StatusExItem(string name, Rectangle bounds)
		{
			_name = name;
			_bounds = bounds;

			_bounds = new Rectangle(_bounds.X, 2, _bounds.Width, _bounds.Height);

			_strFormat.Alignment = StringAlignment.Near;
			_strFormat.LineAlignment = StringAlignment.Center;
			_strFormat.FormatFlags = StringFormatFlags.NoWrap;
		}
		#endregion Constructors and Destructors

		#region Properties
		public bool Visible
		{
			get { return _visible; }
			set { _visible = value; }
		}

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public Rectangle Bounds
		{
			get { return _bounds; }
			set { _bounds = value; }
		}

		public Color BkColorStart
		{
			get { return _bkColorStart; }
			set { _bkColorStart = value; }
		}

		public Color BkColorEnd
		{
			get { return _bkColorEnd; }
			set { _bkColorEnd = value; }
		}

		public Color ForeColor
		{
			get { return _foreColor; }
			set { _foreColor = value; }
		}
		public bool FixedSize
		{
			get { return _fixedSize; }
			set { _fixedSize = value; }
		}
		#endregion Properties

		#region Methods
		public virtual void Clear(Graphics grph)
		{
			try
			{
				using (LinearGradientBrush brush = 
						   new LinearGradientBrush(
						   _bounds, 
						   _bkColorStart, 
						   _bkColorEnd, 
						   LinearGradientMode.Vertical))
				{
					grph.FillRectangle(brush, _bounds.X + 1, _bounds.Y+1, _bounds.Width-2, _bounds.Height-2);
				}
			}
			catch
			{
				// nothing
			}
		}

		public virtual void Draw(Graphics grph)
		{
		}
		public virtual bool Contains(int x, int y)
		{
			return _bounds.Contains(x, y);
		}

		protected virtual void RaiseEvent_DataChanged()
		{
			if (DataChanged != null)
				DataChanged(this, EventArgs.Empty);
		}
		#endregion Methods

		#region Helpers
		#endregion Helpers
	}

	public class SeparatorStatusEx : StatusExItem
	{
		public SeparatorStatusEx(Rectangle bounds) : base(string.Empty, bounds)
		{
			_bounds.Width = 5;
			_bounds.Height = 22;
		}
		
		private Image _image = null;
		private Image Image
		{
			get 
			{
				if (_image == null)
				{
					Image tmp = null;
					try
					{
						tmp = Image.FromStream(this.GetType().Assembly.GetManifestResourceStream("SiGlaz.UI.CustomControls.Images.Separator.png"));
					}
					catch
					{
						tmp = null;
					}					
					
					_image = tmp;
				}

				return _image;
			}
		}

		public override void Draw(Graphics grph)
		{
			base.Draw (grph);

			base.Clear(grph);

			Image image = this.Image;

			if (image != null)
			{
				float haftWidth = image.Width*0.5f;
				float haftHegiht = image.Height*0.5f;

				float l = _bounds.Left + _bounds.Width*0.5f - haftWidth;
				float t = _bounds.Top + _bounds.Height*0.5f - haftHegiht;

				grph.DrawImage(image, l, t);
			}
		}
	}

	public class StateEx : StatusExItem
	{
		#region Member fields
		private string _text = string.Empty;
		#endregion Member fields

		#region Constructors and Destructors
		public StateEx(string name, Rectangle bounds, string text) : base(name, bounds)
		{
			_text = text;
			_fixedSize = false;			
			_bounds.Height = 22;
		}
		#endregion Constructors and Destructors

		#region Properties
		public string Text
		{
			get { return _text; }
			set 
			{
				if (_text != value)
				{
					_text = value; 

					RaiseEvent_DataChanged();
				}
			}
		}
		#endregion Properties

		#region Methods
		public override void Draw(Graphics grph)
		{
			base.Draw(grph);

			base.Clear(grph);

			if (!_visible)
				return;

			try
			{
                using (SolidBrush brush = new SolidBrush(_foreColor))
                {
                    grph.DrawString(_text, _font, brush, _bounds, _strFormat);
                }

                //int w = _bounds.Width - (4 + 4);
                //if (w > 0)
                //{
                //    Rectangle stBounds = new Rectangle(4, 4, w, 16);
                //    using (SolidBrush brush = new SolidBrush(_foreColor))
                //    {
                //        grph.DrawString(_text, _font, brush, stBounds, _strFormat);
                //    }
                //}
			}
			catch
			{
				// nothing
			}
		}
		#endregion Methods

		#region Helpers
		#endregion Helpers
	}

	public class PointStateEx : StatusExItem
	{
		#region Member fields
		protected Image _image = null;
		protected PointF _pt = PointF.Empty;
		protected bool _visiblePt = true;
        protected int padding = 2;
		#endregion Member fields

		#region Constructors and Destructors
		public PointStateEx(string name, Rectangle bounds, Image image, PointF pt) : base(name, bounds)
		{
			_image = image;
			_pt = pt;
		}
		#endregion Constructors and Destructors

		#region Properties
		public Image Image
		{
			get { return _image; }
			set 
			{
				if (_image != value)
				{
					_image = value; 

					RaiseEvent_DataChanged();
				}
			}
		}

		public PointF Pt
		{
			get { return _pt; }
			set 
			{ 
				if (_pt != value)
				{
					_pt = value; 

					RaiseEvent_DataChanged();
				}
			}
		}

		public bool VisiblePt
		{
			get { return _visiblePt; }
			set 
			{ 				
				_visiblePt = value; 
			}
		}
		protected virtual string StrPt
		{
			get { return string.Format("{0}, {1}", _pt.X, _pt.Y); }
		}
		#endregion Properties

		#region Methods
		public override void Draw(Graphics grph)
		{
			base.Draw(grph);

			base.Clear(grph);

			if (!_visible)
				return;

			try
			{
				int x = _bounds.X;
                int y = _bounds.Y + padding;

				// draw image
				if (_image != null)
				{
					grph.DrawImage(_image, x, y);

					x += _image.Width + padding;
				}

				if (_visiblePt && CanDrawPt(x))
				{					
					Rectangle ptBounds = GetPtBounds(x, y);

					using (SolidBrush brush = new SolidBrush(_foreColor))
					{
						grph.DrawString(this.StrPt, _font, brush, ptBounds, _strFormat);
					}
				}
			}
			catch
			{
				// nothing
			}
		}

		private bool CanDrawPt(int x)
		{
			//return (_bounds.Width - (x + 4) > 0);
			return true;
		}
		
		private Rectangle GetPtBounds(int x, int y)
		{
			return (_image != null ? new Rectangle(x, y, _bounds.Width - (16 + 2*padding), 16) : new Rectangle(x, y, _bounds.Width - (padding), 16));
		}
		#endregion Methods

		#region Helpers
		#endregion Helpers
	}
	
	public class OpenFileStateEx : StatusExItem
	{
		private Image _image = null;
		private string _fileName = string.Empty;
		private bool _visibleFileName = true;

		public OpenFileStateEx(Rectangle bounds, string fileName, Image image) : base(string.Empty, bounds)
		{
			_image = image;
			_fileName = fileName;

			_strFormat.Alignment = StringAlignment.Near;			
			_strFormat.LineAlignment = StringAlignment.Center;			
			_strFormat.FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.DirectionRightToLeft;
			
			_bounds.Height = 22;
			_bounds.Width = 300;

			Initialize();
		}

		private void Initialize()
		{
			if (_image == null)
			{
				Image tmp = null;
				try
				{
					tmp = Image.FromStream(this.GetType().Assembly.GetManifestResourceStream("SiGlaz.UI.CustomControls.Images.file.gif"));
				}
				catch
				{
					tmp = null;
				}														

				_image = tmp;
			}
		}

		public Image Image
		{
			get { return _image; }
			set 
			{ 
				if (_image != value)
				{
					_image = null; 
					
					RaiseEvent_DataChanged();
				}
			}
		}

		public string FileName
		{
			get { return _fileName; }
			set 
			{
				if (_fileName != value)
				{
					_fileName = value; 

					RaiseEvent_DataChanged();
				}
			}
		}


		public bool VisibleFileName
		{
			get { return _visibleFileName; }
			set { _visibleFileName = value; }
		}

		public override void Draw(Graphics grph)
		{
			base.Draw (grph);

			base.Clear(grph);

			if (!_visible)
				return;

			try
			{
				int x = _bounds.X;
				int y = 4;

				// draw image
				if (_image != null)
				{
					grph.DrawImage(_image, x, y);

					x += _image.Width + 4;
				}

				if (_visibleFileName && CanDrawPt(x))
				{					
					Rectangle ptBounds = GetPtBounds(x, y + 2);

					using (SolidBrush brush = new SolidBrush(_foreColor))
					{
						grph.DrawString(_fileName, _font, brush, ptBounds, _strFormat);
					}
				}
			}
			catch
			{
				// nothing
			}
		}


		private bool CanDrawPt(int x)
		{
			//return (_bounds.Width - (x + 4) > 0);
			return true;
		}
		
		private Rectangle GetPtBounds(int x, int y)
		{
			return (_image != null ? new Rectangle(x, y, _bounds.Width - (16 + 2 + 2), 16) : new Rectangle(x, y, _bounds.Width - (2), 16));
		}
	}

	public class ViewerSizeStatusEx : PointStateEx
	{
		public ViewerSizeStatusEx(Rectangle bounds, PointF pt, Image image) : base("Viewer Size", bounds, image, pt)
		{			
			_bounds.Height = 22;
			_bounds.Width = 100;

			Initialize();
		}

		private void Initialize()
		{
			if (_image == null)
			{
				Image tmp = null;
				try
				{
					tmp = Image.FromStream(this.GetType().Assembly.GetManifestResourceStream("SiGlaz.UI.CustomControls.Images.viewer_size.gif"));
				}
				catch
				{
					tmp = null;
				}														

				_image = tmp;
			}
		}

        protected override string StrPt
        {
            get
            {
                if (_pt.X <= 0 || _pt.Y <= 0)
                    return "N/A";

                return string.Format("({0}x{1})", _pt.X, _pt.Y);
            }
        }
	}

	public class MousePositionStatusEx : PointStateEx
	{
		public MousePositionStatusEx(
            Rectangle bounds, PointF pt, Image image) : 
            base("Viewer Size", bounds, image, pt)
		{			
			_bounds.Height = 22;
			_bounds.Width = 120;

			Initialize();
		}

		private void Initialize()
		{
			if (_image == null)
			{
				Image tmp = null;
				try
				{
					tmp = 
                        Image.FromStream(
                        this.GetType().Assembly.GetManifestResourceStream(
                        "SiGlaz.UI.CustomControls.Images.mouse_position.gif"));
				}
				catch
				{
					tmp = null;
				}														

				_image = tmp;
			}
		}

		protected override string StrPt
		{
			get
			{
                if (_pt.X < 0 || _pt.Y < 0)
                    return "N/A";

				return string.Format("({0}, {1})", _pt.X, _pt.Y);
			}
		}
	}
}
