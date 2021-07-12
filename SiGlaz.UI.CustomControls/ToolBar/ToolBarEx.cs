using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SiGlaz.UI.CustomControls
{
	/// <summary>
	/// Summary description for ToolBarEx.
	/// </summary>
	public class ToolBarEx : Control
	{
		#region Member fields
		private GradientColor _bkColor = 
			new GradientColor(
			Color.White,
			SystemColors.Control
			);

		private float _cornerRadius = 3.5f;

		private bool _hasCornerRound = false;		
		private bool _hasLeftTopCorner = false;
		private bool _hasRightTopCorner = false;
		private bool _hasLeftBottomCorner = false;
		private bool _hasRightBottomCorner = false;

		private GraphicsPath _path = null;

		private Color _borderColor = Color.FromArgb(0, 45, 150);
		private bool _hasDrawBorder = false;
		private bool _hasLeftBorder = false;
		private bool _hasTopBorder = false;
		private bool _hasRightBorder = false;
		private bool _hasBottomBorder = false;

		//private Color _itemBorderColor = Color.Blue;
		private Color _itemBorderColor = Color.FromArgb(255, 227, 138);

		private GradientColor _bkColorPushedItem = new GradientColor(
			Color.FromArgb(218, 234, 253),Color.FromArgb(136, 174, 228));
		private GradientColor _bkColorHoverItem = new GradientColor(
			Color.FromArgb(255, 222, 110), Color.FromArgb(255, 227, 138));

		private ToolBarItemExCollection _items = new ToolBarItemExCollection();
		private ToolBarItemEx _currentItems = null;

		public event ToolBarExEventHandlers ItemClicked;

        private ToolTip _toolTip = null;
		#endregion Member fields

		#region Properties
		private GraphicsPath GrphPath
		{
			get 
			{
				if (_path == null)
				{
					float w = (float)this.Width - 2.0f;
					if (w <= 0)
						return null;
					
					_path = Utils.CreateRoundRect(
						0.0f, 0.0f, w, (float)this.Height-0.5f, 
						_cornerRadius, 
						_hasLeftTopCorner, 
						_hasRightTopCorner, 
						_hasLeftBottomCorner, 
						_hasRightBottomCorner);
				}

				return _path;
			}

			set
			{
				if (_path != value)
				{
					if (_path != null)
						_path.Dispose();
					_path = value;
				}
			}
		}
		
		private GradientColor BkColor
		{
			get 
			{ 
				if (_bkColor == null)
					_bkColor = new GradientColor(
						Color.White,
						SystemColors.Control
						);
				return _bkColor; 
			}
		}

		public Color BkColorStart
		{
			get 
			{ 
				if (_bkColor == null)
					_bkColor = new GradientColor(
						Color.White,
						SystemColors.Control
						);
				return _bkColor.Start;
			}

			set
			{
				GradientColor bkColor = this.BkColor;
				if (bkColor.Start != value)
				{
					bkColor.Start = value;
					this.Invalidate(true);
				}
			}
		}

		public Color BkColorEnd
		{
			get 
			{ 
				if (_bkColor == null)
					_bkColor = new GradientColor(
						Color.White,
						SystemColors.Control
						);
				return _bkColor.End;
			}

			set
			{
				GradientColor bkColor = this.BkColor;
				if (bkColor.End != value)
				{
					bkColor.End = value;
					this.Invalidate(true);
				}
			}
		}

		public Color BorderColor
		{
			get { return _borderColor; }
			set
			{
				if (_borderColor != value)
				{
					_borderColor = value;
					this.Invalidate(true);
				}
			}
		}

		public float CornerRadius
		{
			get { return _cornerRadius; }
			set
			{
				if (_cornerRadius != value)
				{
					_cornerRadius = value;
					if (_cornerRadius < 1)
						_cornerRadius = 1.0f;
					this.Invalidate(true);
				}
			}
		}

		private void OnDrawingCornerChanged()
		{
			bool hasCornerRound = 
				_hasLeftTopCorner | _hasRightTopCorner | 
				_hasLeftBottomCorner | _hasRightBottomCorner;

			if (_hasCornerRound != hasCornerRound)
			{
				_hasCornerRound = hasCornerRound;

				if (!_hasCornerRound)
					this.GrphPath = null;
			}
		}

		public bool HasLeftTopCorner
		{
			get { return _hasLeftTopCorner; }
			set
			{
				if (_hasLeftTopCorner != value)
				{
					_hasLeftTopCorner = value;
		
					this.OnDrawingCornerChanged();

					this.Invalidate(true);
				}
			}
		}

		public bool HasRightTopCorner
		{
			get { return _hasRightTopCorner; }
			set
			{
				if (_hasRightTopCorner != value)
				{
					_hasRightTopCorner = value;
		
					this.OnDrawingCornerChanged();

					this.Invalidate(true);
				}
			}
		}
		
		public bool HasLeftBottomCorner
		{
			get { return _hasLeftBottomCorner; }
			set
			{
				if (_hasLeftBottomCorner != value)
				{
					_hasLeftBottomCorner = value;
		
					this.OnDrawingCornerChanged();

					this.Invalidate(true);
				}
			}
		}

		public bool HasRightBottomCorner
		{
			get { return _hasRightBottomCorner; }
			set
			{
				if (_hasRightBottomCorner != value)
				{
					_hasRightBottomCorner = value;
		
					this.OnDrawingCornerChanged();

					this.Invalidate(true);
				}
			}
		}

				
		private void OnDrawingBorderChanged()
		{
			_hasDrawBorder = 
				_hasLeftBorder | _hasTopBorder | 
				_hasRightBorder | _hasBottomBorder;			
		}

		public bool HasLeftBorder
		{
			get { return _hasLeftBorder; }
			set
			{
				if (_hasLeftBorder != value)
				{
					_hasLeftBorder = value;
		
					this.OnDrawingBorderChanged();

					this.Invalidate(true);
				}
			}
		}

		public bool HasTopBorder
		{
			get { return _hasTopBorder; }
			set
			{
				if (_hasTopBorder != value)
				{
					_hasTopBorder = value;
		
					this.OnDrawingBorderChanged();

					this.Invalidate(true);
				}
			}
		}
		
		public bool HasRightBorder
		{
			get { return _hasRightBorder; }
			set
			{
				if (_hasRightBorder != value)
				{
					_hasRightBorder = value;
		
					this.OnDrawingBorderChanged();

					this.Invalidate(true);
				}
			}
		}
		
		public bool HasBottomBorder
		{
			get { return _hasBottomBorder; }
			set
			{
				if (_hasBottomBorder != value)
				{
					_hasBottomBorder = value;
		
					this.OnDrawingBorderChanged();

					this.Invalidate(true);
				}
			}
		}

		public Color ItemBorderColor
		{
			get { return _itemBorderColor; }
			set
			{
				if (_itemBorderColor != value)
				{
					_itemBorderColor = value;

					this.Invalidate(true);
				}
			}
		}
		
		
		private GradientColor BkColorPushedItem
		{
			get
			{
				if (_bkColorPushedItem == null)
					_bkColorPushedItem = new GradientColor(
						Color.FromArgb(218, 234, 253),
						Color.FromArgb(136, 174, 228));
				return _bkColorPushedItem;
			}
		}

		public Color BkColorPushedItemStart
		{
			get { return this.BkColorPushedItem.Start; }
			set
			{
				GradientColor c = this.BkColorPushedItem;
				if (c.Start != value)
				{
					c.Start = value;

					this.Invalidate(true);
				}
			}
		}

		public Color BkColorPushedItemEnd
		{
			get { return this.BkColorPushedItem.End; }
			set
			{
				GradientColor c = this.BkColorPushedItem;
				if (c.End != value)
				{
					c.End = value;

					this.Invalidate(true);
				}
			}
		}


		private GradientColor BkColorHoverItem
		{
			get
			{
				if (_bkColorHoverItem == null)
					_bkColorHoverItem = new GradientColor(
						Color.FromArgb(255, 222, 110), Color.FromArgb(255, 227, 138));
				return _bkColorHoverItem;
			}
		}

		public Color BkColorHoverItemStart
		{
			get { return this.BkColorHoverItem.Start; }
			set
			{
				GradientColor c = this.BkColorHoverItem;
				if (c.Start != value)
				{
					c.Start = value;

					this.Invalidate(true);
				}
			}
		}

		public Color BkColorHoverItemEnd
		{
			get { return this.BkColorHoverItem.End; }
			set
			{
				GradientColor c = this.BkColorHoverItem;
				if (c.End != value)
				{
					c.End = value;

					this.Invalidate(true);
				}
			}
		}


		public ToolBarItemExCollection Items
		{
			get { return _items; }
		}

		private ToolBarItemEx CurrentItem
		{
			get { return _currentItems; }
			set
			{
				if (_currentItems != value)
				{
					_currentItems = value;

					if (_currentItems == null)
					{
						this.Cursor = Cursors.Default;

                        this.ToolTip.Hide(this);
                        if (this.ToolTip.Active)
                            this.ToolTip.Active = false;                        
					}
					else
					{
						this.Cursor = Cursors.Hand;
                        if (!this.ToolTip.Active)
                            this.ToolTip.Active = true;
                        this.ToolTip.SetToolTip(this, _currentItems.ToolTip);
					}

					this.Invalidate(true);
				}
			}
		}

		private Image _separatorImage = null;
		private Image SeparatorImage
		{
			get 
			{
				if (_separatorImage == null)
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
					
					_separatorImage = tmp;
				}

				return _separatorImage;
			}
		}

        public ToolTip ToolTip
        {
            get
            {
                if (_toolTip == null)
                {
                    _toolTip = new ToolTip(this.components);
                    _toolTip.Active = false;
                    _toolTip.InitialDelay = 200;
                }
                return _toolTip;
            }
        }
		#endregion Properties

		#region Constructors and Destructors
        private System.ComponentModel.Container components = null;
		public ToolBarEx()
		{
            this.components = new System.ComponentModel.Container();

			//
			// TODO: Add constructor logic here
			//
			this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
			this.UpdateStyles();

			_items.ItemAdded += new EventHandler(_items_ItemAdded);
		}

		~ToolBarEx()
		{
			this.GrphPath = null;

            if (_toolTip != null)
            {
                _toolTip.Dispose();
                _toolTip = null;
            }
		}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }

            base.Dispose(disposing);
        }
		#endregion Constructors and Destructors

		#region Overrides
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint (e);

			try
			{
				#region Draw background
				e.Graphics.Clear(this.BackColor);

				GradientColor bkColor = this.BkColor;

				if (_hasCornerRound)
				{
					if (this.Width - 6 > 0 && this.GrphPath != null)
					{
						Rectangle rect = new Rectangle(3, 0, this.Width - 6, this.Height);
				
						using (LinearGradientBrush brush = 
								   new LinearGradientBrush(
								   rect,
								   bkColor.Start,
								   bkColor.End,
								   LinearGradientMode.Vertical))
						{	
							e.Graphics.FillPath(brush, this.GrphPath);
						}

						if (_hasDrawBorder)
						{
							using (Pen pen = new Pen(_borderColor, 1.0f))
							{
								e.Graphics.DrawPath(pen, this.GrphPath);
							}
						}
					}
				}
				else
				{
					using (LinearGradientBrush brush = 
							   new LinearGradientBrush(
							   new Rectangle(0, 0, this.Width, this.Height),
							   bkColor.Start,
							   bkColor.End,
							   LinearGradientMode.Vertical))
					{	
						e.Graphics.FillRectangle(brush, 0, 0, this.Width, this.Height);
					}

					if (_hasDrawBorder)
					{
						using (Pen pen = new Pen(_borderColor, 1.0f))
						{
							if (_hasLeftBorder)
								e.Graphics.DrawLine(pen, 0, 0, 0, this.Height-1);

							if (_hasTopBorder)
								e.Graphics.DrawLine(pen, 0, 0, this.Width-1, 0);

							if (_hasRightBorder)
								e.Graphics.DrawLine(pen, this.Width-1, 0, this.Width-1, this.Height-1);

							if (_hasBottomBorder)
								e.Graphics.DrawLine(pen, 0, this.Height-1, this.Width-1, this.Height-1);
						}
					}
				}
				#endregion Draw background

				#region Draw items
				foreach (ToolBarItemEx item in _items)
				{
					if (!item.Visible)
						continue;

					if (item == _currentItems)
						continue;

					switch (item.ButtonType)
					{
						case eToolBarItemExType.Button:
							if (item.Enable)
							{
								item.Draw(e.Graphics);
							}
							else
							{
								item.Draw(e.Graphics);
							}
							break;
						case eToolBarItemExType.Push:
							if (item.Enable)
							{
								if (item.Pushed)
								{
									GradientColor c = this.BkColorPushedItem;
									item.Draw(e.Graphics, c.Start, c.End, _borderColor);
								}
								else
								{
									item.Draw(e.Graphics);
								}
							}
							else
							{
								item.Draw(e.Graphics);
							}
							break;
						case eToolBarItemExType.Separator:
							item.Draw(e.Graphics);
							break;
						default:
							break;
					}
				}

				if (_currentItems != null)
				{
                    if (_currentItems.Enable)
                    {
                        GradientColor c = this.BkColorHoverItem;
                        _currentItems.Draw(e.Graphics, c.Start, c.End, _borderColor);
                    }
                    else
                    {
                        _currentItems.Draw(e.Graphics);
                    }
				}
				#endregion Draw items
			}
			catch
			{
			}
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged (e);

			this.GrphPath = null;

			this.CorrectItemBounds();

			this.Invalidate(true);
		}


		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove (e);

			if (_currentItems != null)
			{
				if (_currentItems.Contains(e.X, e.Y))
					return;
			}

			this.CurrentItem = _items.GetItemContains(e.X, e.Y);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave (e);

			this.CurrentItem = null;
		}


		protected override void OnClick(EventArgs e)
		{
			base.OnClick (e);

			if (_currentItems != null && _currentItems.Enable)
			{
				RaiseEventItemClicked();
			}
		}
		#endregion Overrides

		#region Events
		private void OnSizeChanged()
		{			
		}
		#endregion Events

		#region Methods
		public int Add(ToolBarItemEx item)
		{
			if (item == null)
				return -1;

			if (item.ButtonType == eToolBarItemExType.Separator && item.Image == null)
			{
				item.Image = this.SeparatorImage;
			}

			int idx = _items.Add(item);

			CorrectItemBounds();

			this.Invalidate(true);

			return idx;
		}

        public void Insert(int index, ToolBarItemEx item)
        {
            if (item.Image == null &&
                item.ButtonType == eToolBarItemExType.Separator)
                item.Image = this.SeparatorImage;

            _items.Insert(index, item);

            CorrectItemBounds();
        }

		private void CorrectItemBounds()
		{						
			int margin = (this.Height - ToolBarItemEx.ButtonSize)/2;

			int x = margin;
			int y = margin - 1;

			int xRTL = this.Width - margin;

			foreach (ToolBarItemEx item in _items)
			{
				if (item.RightToLeft)
				{
					if (item.ButtonType == eToolBarItemExType.Separator)
					{
						Rectangle rect = new Rectangle(xRTL - ToolBarItemEx.SeparatorSize, y, ToolBarItemEx.SeparatorSize, ToolBarItemEx.ButtonSize);
						item.Bounds = rect;

						xRTL -= ToolBarItemEx.SeparatorSize + ToolBarItemEx.SeparatorSize;
					}
					else
					{
						Rectangle rect = new Rectangle(xRTL - ToolBarItemEx.ButtonSize, y, ToolBarItemEx.ButtonSize, ToolBarItemEx.ButtonSize);
						item.Bounds = rect;

						xRTL -= ToolBarItemEx.ButtonSize + ToolBarItemEx.SeparatorSize;
					}
				}
				else
				{
					if (item.ButtonType == eToolBarItemExType.Separator)
					{
						Rectangle rect = new Rectangle(x, y, ToolBarItemEx.SeparatorSize, ToolBarItemEx.ButtonSize);
						item.Bounds = rect;

						x += ToolBarItemEx.SeparatorSize + ToolBarItemEx.SeparatorSize;
					}
					else
					{
						Rectangle rect = new Rectangle(x, y, ToolBarItemEx.ButtonSize, ToolBarItemEx.ButtonSize);
						item.Bounds = rect;

						x += ToolBarItemEx.ButtonSize + ToolBarItemEx.SeparatorSize;
					}
				}
			}
		}
		#endregion Methods

		#region Helpers
		private void RaiseEventItemClicked()
		{
			if (ItemClicked != null)
				ItemClicked(this, new ToolBarExEventArgs(_currentItems));
		}
		#endregion Helpers

		private void _items_ItemAdded(object sender, EventArgs e)
		{
			CorrectItemBounds();
		}
	}

	public enum eToolBarItemExType
	{
		Button = 0,
		Push = 1,
		Separator
	}

	public class ToolBarItemEx
	{
		#region Constants
		public const int ButtonSize = 20;
		public const int SeparatorSize = 5;
		#endregion Constants

		#region Member fields
		private string _name = string.Empty;
		private string _tooltip = string.Empty;
		private Rectangle _bounds = Rectangle.Empty;
		private Image _image = null;
        private Image _disableImage = null;
		private bool _enable = true;
		private bool _visible = true;
		private eToolBarItemExType _type = eToolBarItemExType.Button;
		private bool _pushed = false;
		private Size _size = new Size(ButtonSize, ButtonSize);
		private bool _rightToLeft = false;
        private object _tag = null;
		#endregion Member fields

		#region Constructors and Destructors
		public ToolBarItemEx()
		{
		}

		public ToolBarItemEx(string name)
		{
			_name = name;
		}

		public ToolBarItemEx(string name, Image image) 
			: this(name)
		{
			this.Image = image;
		}

		public ToolBarItemEx(string name, Image image, eToolBarItemExType buttonType) 
			: this(name, image)
		{
			_type = buttonType;
		}
		#endregion Constructors and Destructors

		#region Properties
		public bool Enable
		{
			get { return _enable; }
			set { _enable = value; }
		}

		public bool Visible
		{
			get { return _visible; }
			set { _visible = value; }
		}

		public Image Image
		{
			get { return _image; }
			set
			{
				if (_image != value)
				{
                    if (_image != null)
                        _image.Dispose();
                    if (_disableImage != null)
                        _disableImage.Dispose();

					_image = value;

                    if (_image != null)
                        _disableImage = Utils.ToDisableImage(_image);
                    else
                        _disableImage = null;
				}
			}
		}        

		public string ToolTip
		{
			get { return _tooltip; }
			set { _tooltip = value; }
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

		public eToolBarItemExType ButtonType
		{
			get { return _type; }
			set 
			{ 
				_type = value;
				
				if (_type == eToolBarItemExType.Separator)
					_size = new Size(SeparatorSize, ButtonSize);
				else
					_size = new Size(ButtonSize, ButtonSize);
			}
		}

		public bool Pushed
		{
			get { return _pushed; }
			set { _pushed = value; }
		}

		public Size Size
		{
			get { return _size; }
		}
		public bool RightToLeft
		{
			get { return _rightToLeft; }
			set { _rightToLeft = value; }
		}

        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }
		#endregion Properties

		#region Methods
		public bool Contains(int x, int y)
		{
			return _bounds.Contains(x, y);
		}

		public void Draw(Graphics grph, Color bkColorStart, Color bkColorEnd, Color borderColor)
		{
			using (LinearGradientBrush brush = new LinearGradientBrush(
					   _bounds, bkColorStart, bkColorEnd, LinearGradientMode.ForwardDiagonal))
			{
				grph.FillRectangle(brush, _bounds);
			}
			
			using (Pen pen = new Pen(borderColor, 1.0f))
			{
				grph.DrawRectangle(pen, _bounds);
			}

			this.DrawImage(grph);
		}

		public void Draw(Graphics grph)
		{
			this.DrawImage(grph);
		}

		private void DrawImage(Graphics grph)
		{
            Image image = (_enable ? _image : _disableImage);

            if (image == null)
				return;

            //float haftWidth = _image.Width*0.5f;
            //float haftHegiht = _image.Height*0.5f;

            float haftWidth = 8;
            float haftHegiht = 8;

			float l = _bounds.Left + _bounds.Width*0.5f - haftWidth;
			float t = _bounds.Top + _bounds.Height*0.5f - haftHegiht;

            if (_type == eToolBarItemExType.Separator)
            {
                haftWidth = image.Width * 0.5f;
                haftHegiht = image.Height * 0.5f;

                l = _bounds.Left + _bounds.Width * 0.5f - haftWidth;
                t = _bounds.Top + _bounds.Height * 0.5f - haftHegiht;

                grph.DrawImage(image, l, t);
            }
            else
            {
                RectangleF srcRect = new RectangleF(0, 0, _image.Width, _image.Height);
                RectangleF dstRect = new RectangleF(l, t, 16, 16);
                grph.DrawImage(image, dstRect, srcRect, GraphicsUnit.Pixel);
            }
		}
		#endregion Methods
	}

	public class ToolBarItemExCollection : CollectionBase
	{
		public event EventHandler ItemAdded;

		public int Add(ToolBarItemEx item)
		{
			if (item == null)
				return - 1;
	
			int indx = this.InnerList.Add(item);

			if (ItemAdded != null)
				ItemAdded(this, EventArgs.Empty);

			return indx;
		}

        internal void Insert(int index, ToolBarItemEx item)
        {
            this.InnerList.Insert(index, item);
        }

		public ToolBarItemEx this[int index]
		{
			get { return this.InnerList[index] as ToolBarItemEx; }
			set { this.InnerList[index] = value; }
		}

		public void Remove(ToolBarItemEx item)
		{
			if (item == null)
				return;
			this.InnerList.Remove(item);
		}

		public ToolBarItemEx GetItemContains(int x, int y)
		{
			foreach (ToolBarItemEx item in this.InnerList)
			{
				if (item.ButtonType == eToolBarItemExType.Separator)
					continue;

				if (item.Contains(x, y))
					return item;
			}
			return null;
		}
	}

	public delegate void ToolBarExEventHandlers(object sender, ToolBarExEventArgs e);
	public class ToolBarExEventArgs : EventArgs
	{
		public ToolBarItemEx Item = null;

		public ToolBarExEventArgs()
		{
		}

		public ToolBarExEventArgs(ToolBarItemEx item)
		{
			Item = item;
		}
	}

    public class ToolBarExHelper
    {
        public static eToolBarItemExType Match(ToolBarButtonStyle style)
        {
            switch (style)
            {
                case ToolBarButtonStyle.PushButton:
                    return eToolBarItemExType.Button;
                case ToolBarButtonStyle.ToggleButton:
                    return eToolBarItemExType.Push;
                case ToolBarButtonStyle.Separator:
                    return eToolBarItemExType.Separator;
                default:
                    throw new System.NotImplementedException("Not support yet!");
            }
        }
    }
}
