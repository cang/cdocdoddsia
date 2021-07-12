using System;
using System.Drawing;

namespace SiGlaz.UI.CustomControls
{
	public class ExplorerItem
	{		
		#region Constants
		public const int FixedHeight = 20;
		#endregion Constants

		#region Member fields
        private bool _visible = true;
        private bool _enabled = true;
        private object _tag = null;
		private Image _image = null;
		private string _text = string.Empty;
		private Rectangle _bounds = Rectangle.Empty;
		private eStatus _status = eStatus.None;

		private ExplorerItemsGroup _owner = null;

		public event EventHandler StatusChanged;
		#endregion Member fields

		#region Properties
        public bool Visible
        {
            get { return _visible; }
            set
            {
                if (_visible != value)
                {
                    _visible = value;
                }
            }
        }

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                if (_enabled != value)
                {
                    _enabled = value;


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

		public int Width
		{
			get { return _bounds.Width; }
			set
			{
				if (_bounds.Width != value)
					_bounds.Width = value;
			}
		}

		public eStatus Status
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

		public Point ImageLocation
		{
			get 
			{
				return new Point(_bounds.X + 8, _bounds.Y + 2);
			}
		}

		public Rectangle TextBounds
		{
			get
			{
				if (_image != null)
				{
					return Rectangle.FromLTRB(
						_bounds.X + 8 + 16 + 4, 
						_bounds.Y + 2,
						_bounds.Right - 4,
						_bounds.Bottom - 2);
				}
				
				return Rectangle.FromLTRB(
					_bounds.X + 8,
					_bounds.Y + 2,
					_bounds.Right - 4,
					_bounds.Bottom - 2);
			}
		}

		public ExplorerItemsGroup Owner
		{
			get { return _owner; }
			set { _owner = value; }
		}
        
        [System.ComponentModel.Browsable(false)]
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }
		#endregion Properties

		#region Constructors and Destructors
		public ExplorerItem()
		{
		}

		public ExplorerItem(Image image, string txt)
		{			
			_text = txt;

			Image = image;
		}
		#endregion Constructors and Destructors

		#region Overrides
		#endregion Overrides

		#region Events
		private void OnImageChanged()
		{
			if (_image != null && (_image.Width != 16 || _image.Height != 16))
			{
                Image thumbnail = new Bitmap(16, 16, 
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using (Graphics grph = Graphics.FromImage(thumbnail))
                {
                    Rectangle srcRect = 
                        new Rectangle(0, 0, _image.Width, _image.Height);
                    Rectangle dstRect = new Rectangle(0, 0, 16, 16);
                    grph.DrawImage(_image, dstRect, srcRect, GraphicsUnit.Pixel);
                }
                    
                this.Image = thumbnail;
			}
		}

		/// <summary>
		/// Required, but not used
		/// </summary>
		/// <returns>true</returns>
		private bool ThumbnailCallback()
		{
			return true;
		}

		private void OnTextChanged()
		{
		}

		private void OnBoundsChanged()
		{
		}

		private void OnStatusChanged()
		{
			if (StatusChanged != null)
				StatusChanged(this, EventArgs.Empty);
		}
		#endregion Events

		#region Methods		
		public bool IsIn(int x, int y)
		{
			return _bounds.Contains(x, y);
		}
		#endregion Methods

		#region Helpers
		#endregion Helpers
	}
}
