using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SiGlaz.UI.CustomControls
{
	public class ExplorerItemsGroup
	{		
		#region Constants
		private const int ItemMarginHeight = 8;
		private const int ItemSeparatingHeight = 2;
		#endregion Constants

		#region Member fields
		private Rectangle _bounds = Rectangle.Empty;
		private Rectangle _captionBounds = Rectangle.Empty;
		private Rectangle _clientBounds = Rectangle.Empty;
		private Point _imageLocation = Point.Empty;
		private Rectangle _captionTextBounds = Rectangle.Empty;
		private Rectangle _buttonBounds = Rectangle.Empty;

		private Image _image = null;
		private string _text = null;
		private GraphicsPath _captionPath = null;
		private eStatus _captionStatus = eStatus.None;
		private eExplorerItemGroupStatus _status = eExplorerItemGroupStatus.Collapsed;
		private ExplorerItem _selectedItem = null;

		private ExplorerItemCollection _items = new ExplorerItemCollection();

		private Image _cache = null;
		
		private bool _susppendedLayout = false;

		public event EventHandler RegionChanged;
		public event EventHandler StatusOfItemChanged;
		public event EventHandler CaptionStatusChanged;
		#endregion Member fields

		#region Properties
		public Point Location
		{
			get { return _bounds.Location; }
			set 
			{
				if (!_bounds.Location.Equals(value))
				{
					this.Bounds = new Rectangle(value.X, value.Y, _bounds.Width, _bounds.Height);
				}
			}
		}

		public Rectangle Bounds
		{
			get { return _bounds; }
			set
			{
				if (_bounds != value)
				{
					_bounds = value;

					OnBoundsChanged();
				}
			}
		}

		public Rectangle CaptionBounds
		{
			get { return _captionBounds; }
		}

		public Rectangle ClientBounds
		{
			get { return _clientBounds; }
		}

		public Point ImageLocation
		{
			get { return _imageLocation; }
		}

		public Rectangle CaptionTextBounds
		{
			get 
			{
				// correct caption text boundary				
				if (_image != null)			
				{
					_captionTextBounds = Rectangle.FromLTRB(
						_captionBounds.X + 4 + 32 + 4, _captionBounds.Y, 
						_captionBounds.Right - 6 - 16 - 4, _captionBounds.Bottom);
				}
				else
				{
					_captionTextBounds = Rectangle.FromLTRB(
						_captionBounds.X + 4, _captionBounds.Y, 
						_captionBounds.Right - 6 - 16 - 4, _captionBounds.Bottom);
				}

				return _captionTextBounds;
			}
		}

		public Rectangle ButtonBounds
		{
			get { return _buttonBounds; }
		}

		public GraphicsPath CaptionPath
		{
			get 
			{ 
				if (_captionPath == null)
					_captionPath = Utils.CreateRoundRect((float)_captionBounds.X-0.5f, _captionBounds.Y, (float)_captionBounds.Width, _captionBounds.Height, 3.5f, true, true, false, false);
				return _captionPath;
			}

			set
			{
				if (_captionPath != value)
				{
					if (_captionPath != null)
						_captionPath.Dispose();
					_captionPath = value;
				}
			}
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
					_image = value;
					OnImageChanged();
				}
			}
		}

		public string Text
		{
			get { return _text; }
			set
			{
				if (_text != value)
				{
					_text = value;
					OnTextChanged();
				}
			}
		}

		public eStatus CaptionStatus
		{
			get { return _captionStatus; }
			set
			{
				if (_captionStatus != value)
				{
					_captionStatus = value;
					OnCaptionStatusChanged();
				}
			}
		}

		public eExplorerItemGroupStatus Status
		{
			get { return _status; }
			set
			{
				if (_status != value)
				{
					_status = value;
					OnStatusChanged();
				}
			}
		}

		public ExplorerItem SelectedItem
		{
			get { return _selectedItem; }
			set
			{
				if (_selectedItem != value)
				{
					_selectedItem = value;
					OnSelectedItemChanged();
				}
			}
		}

		public ExplorerItemCollection Items
		{
			get { return _items; }
			set
			{
				if (_items != value)
				{
					_items = value;
					OnItemsChanged();
				}
			}
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
					OnCacheChanged();
				}
			}
		}

		public Rectangle VisibleBounds
		{
			get 
			{
				if (_status == eExplorerItemGroupStatus.Collapsed)
					return new Rectangle(_bounds.X, _bounds.Y, _bounds.Width, 32);

				return _bounds;
			}
		}
		#endregion Properties

		#region Constructors and Destructors
		public ExplorerItemsGroup()
		{
		}

		public ExplorerItemsGroup(Image image, string txt)
		{
			_image = image;
			_text = txt;
		}
		#endregion Constructors and Destructors

		#region Overrides
		#endregion Overrides

		#region Events
		private void OnBoundsChanged()
		{
			// correct caption boundary
			_captionBounds = new Rectangle(_bounds.X, _bounds.Y+8, _bounds.Width, 24);

			// correct image location
			_imageLocation.X = _bounds.X + 4;
			_imageLocation.Y = _bounds.Y;			

			// correct button boundary
			_buttonBounds = new Rectangle(_captionBounds.Right - 6 - 16, _captionBounds.Y + 4, 16, 16);

			// correct client boundary
			_clientBounds = new Rectangle(_bounds.X, _bounds.Y+32, _bounds.Width, _bounds.Height-32);

			if (_items != null)
			{
				foreach (ExplorerItem item in _items)
				{
					item.Width = _bounds.Width - 4;
				}
			}

			this.Cache = null;
			this.CaptionPath = null;

			if (!_susppendedLayout)
			{
				// raise event region changed
				if (RegionChanged != null)
					RegionChanged(this, EventArgs.Empty);
			}
		}

		private void OnImageChanged()
		{
		}

		private void OnTextChanged()
		{
		}

		private void OnCaptionStatusChanged()
		{
			if (CaptionStatusChanged != null)
				CaptionStatusChanged(this, EventArgs.Empty);
		}

		private void OnStatusChanged()
		{
			if (_status == eExplorerItemGroupStatus.Expanded)
				CorrectLayout();
		}

		private void OnSelectedItemChanged()
		{
		}

		private void OnItemsChanged()
		{
		}

		private void OnCacheChanged()
		{
		}

		private void item_StatusChanged(object sender, EventArgs e)
		{
			if (StatusOfItemChanged != null)
				StatusOfItemChanged(sender, e);
		}
		#endregion Events

		#region Methods
		public int Add(ExplorerItem item)
		{
			int index = AddItem(item);

			this.CorrectLayout();

			return index;
		}

		public void AddRange(ExplorerItem[] items)
		{
			if (items == null)
				return;

			foreach (ExplorerItem item  in items)
				AddItem(item);

			this.CorrectLayout();
		}

		public void AddRange(ExplorerItemCollection items)
		{
			if (items == null)
				return;

			foreach (ExplorerItem item  in items)
				AddItem(item);

			this.CorrectLayout();
		}

		public void SuspendLayout()
		{
			_susppendedLayout = true;
		}

		public void ResumeLayout()
		{
			_susppendedLayout = true;			
		}

		private int AddItem(ExplorerItem item)
		{
			if (item == null)
				return -1;

			// add item
			int index = _items.Add(item);

			item.Owner = this;
			item.StatusChanged += new EventHandler(item_StatusChanged);

			// calc item's location
			if (index >= 0)
			{
				int x = _bounds.X + 2;

				int y = ItemMarginHeight;
				if (_items.Count > 1)
					y = _items[_items.Count-2].Bounds.Bottom + ItemSeparatingHeight + 1;

				//int y = 4 + (_items.Count-1)*ExplorerItem.FixedHeight;

				int w = _bounds.Width - 4;
				
				item.Bounds = new Rectangle(x, y, w, ExplorerItem.FixedHeight);
			}
			
			// clear cache
			this.Cache = null;

			// return added item
			return index;
		}

		private void CorrectLayout()
		{
			int x = _bounds.X;
			int y = _bounds.Y;
			int w = _bounds.Width;
			int h = 32;
			
			if (_items != null && _items.Count > 0)
			{
				//h += 4 + _items.Count*ExplorerItem.FixedHeight + 4;
				h += ItemMarginHeight + _items.Count*ExplorerItem.FixedHeight + (_items.Count-1)*(ItemSeparatingHeight + 1) + ItemMarginHeight;
			}

			this.Bounds = new Rectangle(x, y, w, h);
		}

		public ExplorerItem GetItemContains(int x, int y)
		{
			if (_status == eExplorerItemGroupStatus.Collapsed)
				return null;

			if (!_clientBounds.Contains(x, y))
				return null;

			x -= _bounds.X;
			y -= _bounds.Y + 32;

			return _items.GetItemContains(x, y);
		}

		public bool CaptionContains(int x, int y)
		{
			return _captionBounds.Contains(x, y);
		}
		#endregion Methods

		#region Helpers
		#endregion Helpers		
	}
}
