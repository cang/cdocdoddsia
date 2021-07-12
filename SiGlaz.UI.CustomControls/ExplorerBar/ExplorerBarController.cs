using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SiGlaz.UI.CustomControls
{
	public class ExplorerBarController
	{
		#region Member fields
        private ExplorerBar _owner = null;

		private ExplorerItemsGroupCollection _groups = new ExplorerItemsGroupCollection();

		private IExplorerBarTheme _theme = null;

		private Size _size = Size.Empty;
		private Size _actualSize = Size.Empty;
		private Point _position = Point.Empty;
		private bool _visibleButton = false;

		private Image _cache = null;

		public event EventHandler RedrawRequested;
		public event EventHandler DefaultCursorAssigned;
		public event EventHandler HandCurosrAssigned;
		public event ExplorerBarEventHandler ItemClicked;
        public event ExplorerBarEventHandler DragDrop;

		private ExplorerItem _currentItem = null;
		private ExplorerItemsGroup _currentCaptionGroup = null;

		private bool _allowMultiExpanding = true;

		private const int ButtonColorGamma = 180;
		
		/**
		private GradientColor _buttonNormalColor1 = 
			new GradientColor(
			Color.FromArgb(ButtonColorGamma, 228, 244, 253), 
			Color.FromArgb(ButtonColorGamma, 217, 239, 252));
		private GradientColor _buttonNormalColor2 = 
			new GradientColor(
			Color.FromArgb(ButtonColorGamma, 188, 229, 252), 
			Color.FromArgb(ButtonColorGamma, 167, 217, 244));
		private GradientColor _buttonHighlighColor1 = 
			new GradientColor(
			Color.FromArgb(ButtonColorGamma, 255, 251, 212), 
			Color.FromArgb(ButtonColorGamma, 255, 226, 127));
		private GradientColor _buttonHighlighColor2 = 
			new GradientColor(
			Color.FromArgb(ButtonColorGamma, 255, 222, 110), 
			Color.FromArgb(ButtonColorGamma, 255, 227, 138));
			
		private Color _buttonBorderNormalColor = Color.FromArgb(179, 228, 249);
		private Color _buttonBorderHighlightColor = Color.FromArgb(255, 248, 146);
		**/

		private GradientColor _buttonNormalColor = 
			new GradientColor(
			Color.FromArgb(ButtonColorGamma, 85, 132, 211),
			Color.FromArgb(ButtonColorGamma, 7, 60, 151));

		private GradientColor _buttonHighlightColor = 
			new GradientColor(
			Color.FromArgb(ButtonColorGamma, 255, 222, 110), 
			Color.FromArgb(ButtonColorGamma, 255, 227, 138));		

		private Color _borderColor = Color.FromArgb(0, 45, 150);
		#endregion Member fields

		#region Properties
		public IExplorerBarTheme Theme
		{
			get
			{
				if (_theme == null)
					_theme = new ExplorerBarLunaBlueTheme();
				return _theme;
			}

			set
			{
				if (_theme != value)
				{
					_theme = value;
					OnThemeChanged();
				}
			}
		}

		private Image Cache
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

		public Size Size
		{
			get { return _size; }
			set
			{
				if (!_size.Equals(value))
				{
					_size = value;
			
					Cache = null;

					CorrectLayout();
				}
			}
		}

		internal ExplorerItem CurrentItem
		{
			get { return _currentItem; }
			set
			{
				if (_currentItem != value)
				{
					if (_currentItem != null)
						_currentItem.Status = eStatus.None;

					_currentItem = value;
					
					if (_currentItem != null)
						_currentItem.Status = eStatus.Hover;

					_redraw = true;					
				}
			}
		}

		private ExplorerItemsGroup CurrentCaptionGroup
		{
			get { return _currentCaptionGroup; }
			set
			{
				if (_currentCaptionGroup != value)
				{
					if (_currentCaptionGroup != null)
						_currentCaptionGroup.CaptionStatus = eStatus.None;

					_currentCaptionGroup = value;

					if (_currentCaptionGroup != null)
						_currentCaptionGroup.CaptionStatus = eStatus.Hover;

					_redraw = true;
				}
			}
		}

		public bool AllowMultiExpanding
		{
			get { return _allowMultiExpanding; }
			set
			{
				if (_allowMultiExpanding != value)
				{
					_allowMultiExpanding = value;

					OnAllowMultiExpandingChanged();
				}
			}
		}

		private Size ActualSize
		{
			get { return _actualSize; }
			set
			{
				_actualSize.Width = value.Width;
				_actualSize.Height = value.Height;

				OnActualSizeChanged();
			}
		}

		private bool VisibleButtonDown
		{
			get { return _visibleButtonDown; }
			set 
			{ 
				if (_visibleButtonDown != value)
				{
					_visibleButtonDown = value;				

					VisibleButton = (_visibleButtonDown | _visibleButtonUp);
				}
			}
		}

		private bool VisibleButtonUp
		{
			get { return _visibleButtonUp; }
			set 
			{ 
				if (_visibleButtonUp != value)
				{
					_visibleButtonUp = value;

					VisibleButton = (_visibleButtonDown | _visibleButtonUp);
				}
			}
		}

		private bool VisibleButton
		{
			get { return _visibleButton; }
			set
			{
				_visibleButton = value;

				UpdateVisibleRegion();
			}
		}

		private bool UseDefaultCursor
		{
			get { return _useDefaultCursor; }
			set
			{
				if (_useDefaultCursor != value)
				{
					_useDefaultCursor = value;

					if (_useDefaultCursor)
					{
						if (DefaultCursorAssigned != null)
							DefaultCursorAssigned(this, EventArgs.Empty);
					}
					else
					{
						if (HandCurosrAssigned != null)
							HandCurosrAssigned(this, EventArgs.Empty);
					}
				}
			}
		}

		private void OnAllowMultiExpandingChanged()
		{
			if (!_allowMultiExpanding)
			{
				if (_currentCaptionGroup != null)
				{
					if (_currentCaptionGroup.Status == eExplorerItemGroupStatus.Expanded)
					{
						bool bRedraw = false;
						foreach (ExplorerItemsGroup group in _groups)
						{
							if (group != _currentCaptionGroup)
							{
								if (group.Status == eExplorerItemGroupStatus.Expanded)
								{
									group.Status = eExplorerItemGroupStatus.Collapsed;
									bRedraw = true;
								}
							}
						}

						if (bRedraw)
						{
							this.Cache = null;
							this.CorrectLayout();
							RaiseEventRedrawRequested();
						}
					}
				}
				else
				{
					ExplorerItemsGroup grp = null;
					
					// find first expanded group
					foreach (ExplorerItemsGroup group in _groups)
					{
						if (group.Status == eExplorerItemGroupStatus.Expanded)
						{
							grp = group;
							break;
						}
					}

					if (grp != null)
					{
						bool bRedraw = false;
						
						foreach (ExplorerItemsGroup group in _groups)
						{
							if (group != grp)
							{
								if (group.Status == eExplorerItemGroupStatus.Expanded)
								{
									group.Status = eExplorerItemGroupStatus.Collapsed;
									bRedraw = true;
								}
							}
						}

						if (bRedraw)
						{
							this.Cache = null;
							this.CorrectLayout();
							RaiseEventRedrawRequested();
						}
					}
				}
			}
		}
		#endregion Properties

		#region Constructors and Destructors
		public ExplorerBarController()
		{
		}

        public ExplorerBarController(ExplorerBar owner)
            : this()
        {
            _owner = owner;
        }
		#endregion Constructors and Destructors

		#region Overrides
		#endregion Overrides

		#region Events
		private void OnGroupsChanged()
		{
		}

		private void OnThemeChanged()
		{
			foreach (ExplorerItemsGroup group in _groups)
			{
				group.Cache = null;
			}

			this.Cache = null;

			RaiseEventRedrawRequested();
		}

		private void group_StatusOfItemChanged(object sender, EventArgs e)
		{
			try
			{
				ExplorerItem item = (ExplorerItem)sender;
				ExplorerItemsGroup group = item.Owner;

				if (group.Cache != null)
				{
					using (Graphics g = Graphics.FromImage(group.Cache))
					{
						_theme.DrawItem(g, item, true);
					}
				}

				if (_cache != null && group.Cache != null)
				{
					using (Graphics g = Graphics.FromImage(_cache))
					{
						int x = item.Owner.Bounds.X + item.Bounds.X;
						int y = item.Owner.Bounds.Y + 32 + item.Bounds.Y;
						int w = item.Bounds.Width;
						int h = item.Bounds.Height;

						g.DrawImage(group.Cache, 
							new Rectangle (x, y, w, h),
							new Rectangle(item.Bounds.X, item.Bounds.Y, w, h),
							GraphicsUnit.Pixel);

					}
				}
			}
			catch
			{
			}
		}

		private void group_CaptionStatusChanged(object sender, EventArgs e)
		{
			try
			{
				ExplorerItemsGroup group = (ExplorerItemsGroup)sender;
				if (group == null || _cache == null)
					return;

				using (Graphics g = Graphics.FromImage(_cache))
				{
					_theme.DrawItemsGroup(g, group, false);
				}
			}
			catch
			{
			}
		}

		private void OnActualSizeChanged()
		{
			if (_visibleButtonUp && !_visibleButtonDown)
			{
				if (_position.Y + _size.Height > _actualSize.Height)
				{
					_position.Y = _actualSize.Height - _size.Height;
					if (_position.Y < 0)
						_position.Y = 0;
				}
			}

			if (_actualSize.Height < _size.Height)
			{
				_actualSize.Height = _size.Height;

				VisibleButtonDown = false;
				VisibleButtonUp = false;

				VisibleButton = false;

				_position.X = 0;
				_position.Y = 0;
			}
			else
			{
				VisibleButton = true;
			}
		}

		private void RaiseEventRedrawRequested()
		{
			if (RedrawRequested != null)
				RedrawRequested(this, EventArgs.Empty);
		}
		#endregion Events

		#region Methods - relate to mouse
		public void OnPaint(PaintEventArgs e)
		{
            Graphics g = e.Graphics;
            GraphicsState gstate = g.Save();
            try
            {
                IExplorerBarTheme theme = this.Theme;
                if (theme != null)
                {
                    if (_cache == null)
                        theme.DrawExplorerBar(_groups, _actualSize, ref _cache);
                }

                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.High;

                if (_cache != null)
                {
                    g.DrawImage(
                        _cache,
                        new Rectangle(0, 0, _size.Width, _size.Height),
                        _position.X, _position.Y, _size.Width, _size.Height,
                        GraphicsUnit.Pixel
                        );
                }

                DrawButtonUp(g);

                DrawButtonDown(g);

                using (Pen pen = new Pen(_borderColor, 1.0f))
                {
                    g.DrawRectangle(pen, 0, 0, (float)_size.Width - 1.0f, (float)_size.Height);
                }
            }
            catch
            {
            }
            finally
            {
                if (gstate != null)
                {
                    g.Restore(gstate);
                }
            }
		}

        public void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && _owner != null &&
                CurrentItem != null && CurrentItem.Tag != null)
            //if (e.Button == MouseButtons.Left && _owner != null &&
            //    CurrentItem != null)
            {
                _owner.DoDragDrop(this.CurrentItem.Tag, DragDropEffects.Copy);

                return;
            }
        }

		private bool _redraw = false;
		private bool _useDefaultCursor = true;
		public void OnMouseMove(MouseEventArgs e)
		{
			PrepareProcessingOnMouseMove();

			ProcessOnMouseMoveForButton(e.X, e.Y);

			ProcessOnMouseMoveForExplorerBar(e.X, e.Y);

			FinalizeProcessingOnMouseMove();
		}

		private void PrepareProcessingOnMouseMove()
		{
			_redraw = false;
		}

		private void ProcessOnMouseMoveForButton(int x, int y)
		{
			bool statusButtonUp = _visibleButtonUp && ButtonUpContains(x, y);

			bool statusButtonDown = _visibleButtonDown && ButtonDownContains(x, y);

			if (!_redraw)
				_redraw = (statusButtonUp != _highlighButtonUp) || (statusButtonDown != _highlighButtonDown);

			_highlighButtonUp = statusButtonUp;

			_highlighButtonDown = statusButtonDown;
		}

		private void ProcessOnMouseMoveForExplorerBar(int x, int y)
		{
			x += _position.X;
			y += _position.Y;

			if (_visibleButton && (_highlighButtonUp || _highlighButtonDown))
			{
				this.CurrentItem = null;

				this.CurrentCaptionGroup = null;
			}
			else
			{
				this.CurrentItem = _groups.GetItemContains(x, y);

				this.CurrentCaptionGroup = _groups.GetHighlightCaptionGroup(x, y);
			}
		}

		private void FinalizeProcessingOnMouseMove()
		{
			this.UseDefaultCursor = !(_highlighButtonUp || _highlighButtonDown || 
				(_currentItem != null && _currentItem.Enabled) || _currentCaptionGroup != null);
			
			if (_redraw)
				RaiseEventRedrawRequested();
		}
		
		public void OnMouseLeave(EventArgs e)
		{
			this.CurrentItem = null;
			this.CurrentCaptionGroup = null;
			_highlighButtonUp = _highlighButtonDown = false;

			this.UseDefaultCursor = true;
			RaiseEventRedrawRequested();
		}

		public void OnMouseWheel(MouseEventArgs e)
		{
			
		}
		
		public void OnClick(EventArgs e)
		{
			#region Button navigator clicked
			if (_visibleButtonUp && _highlighButtonUp)
			{
				int step = 96;

				int new_position = _position.Y - step;
				if (new_position < 0)
					new_position = 0;

				_position.Y = new_position;

				UpdateVisibleRegion();

				RaiseEventRedrawRequested();

				return;
			}

			if (_visibleButtonDown && _highlighButtonDown)
			{
				int step = 96;

				int new_position = _position.Y + step;
				if (new_position + _size.Height > _actualSize.Height)
					new_position = _actualSize.Height - _size.Height;

				_position.Y = new_position;

				UpdateVisibleRegion();

				RaiseEventRedrawRequested();

				return;
			}
			#endregion Button navigator clicked

			#region Item clicked
			if (_currentItem != null && _currentItem.Enabled)
			{
				if (ItemClicked != null)
					ItemClicked(this, new ExplorerBarEventArgs(_currentItem));

				return;
			}
			#endregion Item clicked

			#region Collapsed or expanded a group
			if (_currentCaptionGroup != null)
			{
				if (_currentCaptionGroup.Status == eExplorerItemGroupStatus.Collapsed)
					_currentCaptionGroup.Status = eExplorerItemGroupStatus.Expanded;
				else
					_currentCaptionGroup.Status = eExplorerItemGroupStatus.Collapsed;

				if (!_allowMultiExpanding)
				{
					if (_currentCaptionGroup.Status == eExplorerItemGroupStatus.Expanded)
					{
						foreach (ExplorerItemsGroup group in _groups)
						{
							if (group != _currentCaptionGroup)
								group.Status = eExplorerItemGroupStatus.Collapsed;
						}
					}
				}

				this.Cache = null;

				CorrectLayout();

				return;
			}
			#endregion Collapsed or expanded a group
		}
		#endregion Methods - relate to mouse

		#region Methods
        internal bool ClearCache()
        {
            bool cleanup = this.Cache != null;

            this.Cache = null;

            if (_allowMultiExpanding)
            {
                if (_currentCaptionGroup == null)
                    return cleanup;

                cleanup = cleanup || (_currentCaptionGroup.Cache != null);
                _currentCaptionGroup.Cache = null;

                return cleanup;
            }            

            if (_groups == null)
                return cleanup;

            foreach (ExplorerItemsGroup group in _groups)
            {
                cleanup = cleanup || (group.Cache != null);
                group.Cache = null;
            }

            return cleanup;
        }

		public void AddItem(ExplorerItemsGroup group, ExplorerItem item)
		{
			if (group == null || item == null)
				return;

			group.Add(item);

			CorrectLayout();
		}

		public int AddGroup(ExplorerItemsGroup group)
		{
			if (group == null)
				return -1;

			int index = _groups.Add(group);
			group.StatusOfItemChanged +=new EventHandler(group_StatusOfItemChanged);
			group.CaptionStatusChanged += new EventHandler(group_CaptionStatusChanged);

			CorrectLayout();

			return index;
		}

		private void CorrectLayout()
		{
			if (_groups == null || _groups.Count == 0)
			{
				this.ActualSize = _size;

				RaiseEventRedrawRequested();

				return;
			}

			int y = 18;

			int w = _size.Width - 16;

			for (int i=0; i<_groups.Count; i++)
			{
				_groups[i].SuspendLayout();

				_groups[i].Bounds = new Rectangle(8, y, w, _groups[i].VisibleBounds.Height);

				y += _groups[i].VisibleBounds.Height + 8;

				_groups[i].ResumeLayout();
			}

			this.ActualSize = new Size(_size.Width, _groups[_groups.Count-1].Bounds.Bottom + 8);

			RaiseEventRedrawRequested();
		}

		private void UpdateVisibleRegion()
		{
			if (!_visibleButton)
				return;

			this.VisibleButtonUp = (_position.Y > 0);	

			this.VisibleButtonDown = (_position.Y + _size.Height < _actualSize.Height);
		}
		#endregion Methods

		#region Helpers
		private bool _visibleButtonUp = false;
		private bool _highlighButtonUp = false;
		private bool ButtonUpContains(float x, float y)
		{
			float radius = 16;
			float radius2 = radius*radius;
			float centerX = _size.Width*0.5f;
			float centerY = radius + 1;

			float r2 = (x-centerX)*(x-centerX) + (y-centerY)*(y-centerY);

			if (r2 > radius2)
				return false;

			return true;
		}

		private bool ButtonDownContains(int x, int y)
		{
			float radius = 16;
			float radius2 = radius*radius;
			float centerX = _size.Width*0.5f;
			float centerY = _size.Height - radius - 3;

			float r2 = (x-centerX)*(x-centerX) + (y-centerY)*(y-centerY);

			if (r2 > radius2)
				return false;

			return true;
		}
		
		private void DrawButtonUp(Graphics grph)
		{
			if (!_visibleButtonUp)
				return;						

			float radius = 12.5f;
			float centerX = _size.Width*0.5f;
			float centerY = radius + 1;

			#region Draw background
			GradientColor gradientColor = 
				(_highlighButtonUp ?  _buttonHighlightColor : _buttonNormalColor);
			RectangleF rect = new RectangleF(centerX - radius, centerY - radius, 2*radius, 2*radius);
			using (LinearGradientBrush brush = new 
					   LinearGradientBrush(rect, gradientColor.Start, gradientColor.End, LinearGradientMode.Vertical))
			{
				grph.FillEllipse(brush, rect);
			}
			#endregion Draw background
			
			#region Draw icon
			float x = centerX;
			float y = centerY - 2.5f;
			Color iconColor = (_highlighButtonUp ? _buttonNormalColor.End : _buttonHighlightColor.End);
			using (SolidBrush brush = new SolidBrush(iconColor))
			{
				using (GraphicsPath path = new GraphicsPath())
				{
					path.AddPolygon(
						new PointF[] {
										 new PointF(x, y), 
										 new PointF(x - 3.0f, y+5.0f),
										 new PointF(x + 3.0f, y+5.0f)
									 }
						);

					path.CloseFigure();

					grph.FillPath(brush, path);
				}				
			}
			#endregion Draw icon

			#region Draw border
			#endregion Draw border
		}

		private bool _visibleButtonDown = false;
		private bool _highlighButtonDown = false;
		private void DrawButtonDown(Graphics grph)
		{
			if (!_visibleButtonDown)
				return;

			float radius = 12.5f;
			float centerX = _size.Width*0.5f;
			float centerY = _size.Height - radius - 3;

			#region Draw background
			GradientColor gradientColor = 
				(_highlighButtonDown ?  _buttonHighlightColor : _buttonNormalColor);
			RectangleF rect = new RectangleF(centerX - radius, centerY - radius, 2*radius, 2*radius);
			using (LinearGradientBrush brush = new 
					   LinearGradientBrush(rect, gradientColor.Start, gradientColor.End, LinearGradientMode.Vertical))
			{
				grph.FillEllipse(brush, rect);
			}
			#endregion Draw background
			
			#region Draw icon
			float x = centerX;
			float y = centerY + 2.5f;
			Color iconColor = (_highlighButtonDown ? _buttonNormalColor.End : _buttonHighlightColor.End);
			using (SolidBrush brush = new SolidBrush(iconColor))
			{
				using (GraphicsPath path = new GraphicsPath())
				{
					path.AddPolygon(
						new PointF[] {
										 new PointF(x, y), 
										 new PointF(x - 3.0f, y-5.0f),
										 new PointF(x + 3.0f, y-5.0f)
									 }
						);

					path.CloseFigure();

					grph.FillPath(brush, path);
				}
			}
			#endregion Draw icon

			#region Draw border			
			#endregion Draw border
		}
		#endregion Helpers		
	}

	public delegate void ExplorerBarEventHandler(object sender, ExplorerBarEventArgs e);

	public class ExplorerBarEventArgs : EventArgs
	{
		public readonly ExplorerItem Item = null;

		public ExplorerBarEventArgs()
		{
		}

		public ExplorerBarEventArgs(ExplorerItem item)
		{
			Item = item;
		}
	}
}
