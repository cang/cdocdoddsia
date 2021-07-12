using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;

namespace SiGlaz.UI.CustomControls
{
	/// <summary>
	/// Summary description for ViewerEx.
	/// </summary>
	public class ViewerEx : Control
	{
		#region Contructors and Destructors
		public ViewerEx() : base()
		{
			//
			// TODO: Add constructor logic here
			//
			Initialize();
		}

		~ViewerEx()
		{
			if (_hScrollBar != null)
			{
				_hScrollBar.ValueChanged -= new EventHandler(_hScrollBar_ValueChanged);

				_hScrollBar.Dispose();
				_hScrollBar = null;
			}

			if (_vScrollBar != null)
			{
				_vScrollBar.ValueChanged -= new EventHandler(_vScrollBar_ValueChanged);

				_vScrollBar.Dispose();
				_vScrollBar = null;
			}
		}
		#endregion Contructors and Destructors

		#region Member fields
		private HScrollBar _hScrollBar = null;
		private VScrollBar _vScrollBar = null;
		private ViewerExParams _parameters = null;
		private bool _canRaiseEvent = true;

		public event ViewerExEventHandlers HScrollChanged;
		public event ViewerExEventHandlers VScrollChanged;
		public event ViewerExEventHandlers SizeChanged;
		#endregion Member fields

		#region Properties
		public new Color BackColor
		{
			get { return base.BackColor; }
			set
			{
				if (base.BackColor != value)
				{
					base.BackColor = value;

					this.Invalidate(true);
				}
			}
		}

		private ViewerExParams Parameters
		{
			get { return _parameters; }
			set
			{
				_parameters = value;

				OnProcessParametersChanged();
			}
		}

		private Image Image
		{
			get 
			{ 
				if (_parameters == null)
					return null;

				return _parameters.Image;
			}
		}
		#endregion Properties

		#region Initialize
		private void Initialize()
		{
			try
			{
				_hScrollBar = new HScrollBar();				
				_hScrollBar.Width = this.Width;
				_hScrollBar.ValueChanged += new EventHandler(_hScrollBar_ValueChanged);
				_hScrollBar.Location = new Point(0, this.Height-_hScrollBar.Height);
				_hScrollBar.Visible = false;
				_hScrollBar.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;

				_vScrollBar = new VScrollBar();
				_vScrollBar.Height = this.Height;
				_vScrollBar.ValueChanged += new EventHandler(_vScrollBar_ValueChanged);
				_vScrollBar.Location = new Point(this.Width-_vScrollBar.Width, 0);
				_vScrollBar.Visible = false;
				_vScrollBar.Anchor = AnchorStyles.Right | AnchorStyles.Top;

				// add to control
				this.SuspendLayout();

				this.Controls.Add(_hScrollBar);
				this.Controls.Add(_vScrollBar);
			}
			catch
			{
			}
			finally
			{
				this.ResumeLayout(false);
			}
		}
		#endregion Initialize

		#region Overrides
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint (e);

			ProcessOnPaint(e);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged (e);

			RaiseEvent_SizeChanged();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove (e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown (e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp (e);
		}

		protected override void OnClick(EventArgs e)
		{
			base.OnClick (e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave (e);
		}

		private int _vsWheelDelta = 0;
		private int _hsWheelDelta = 0;
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel (e);

			if (_vScrollBar.Visible)
			{
				_vsWheelDelta += e.Delta;
				//bool flag = false;
				while (Math.Abs(_vsWheelDelta) >= 120)
				{
					if (_vsWheelDelta > 0)
					{
						_vsWheelDelta -= 120;
						
						// do scroll with small decrement
						DoScroll(_vScrollBar, true);

						//flag = true;
					}
					else
					{
						_vsWheelDelta += 120;
						
						// do scroll with small increment
						DoScroll(_vScrollBar, false);

						//flag = true;
					}
				}
			}
			else if (_hScrollBar.Visible)
			{
				_hsWheelDelta += e.Delta;
				//bool flag = false;
				while (Math.Abs(_hsWheelDelta) >= 120)
				{
					if (_hsWheelDelta > 0)
					{
						_hsWheelDelta -= 120;
						
						// do scroll with small decrement
						DoScroll(_hScrollBar, true);

						//flag = true;
					}
					else
					{
						_hsWheelDelta += 120;
						
						// do scroll with small increment
						DoScroll(_hScrollBar, false);

						//flag = true;
					}
				}
			}
		}

		private void DoScroll(System.Windows.Forms.ScrollBar scrollBar, bool bSmallDecrement)
		{
			int newValue = scrollBar.Value;

			if (bSmallDecrement)
			{
				newValue = (int)Math.Max(scrollBar.Value - scrollBar.SmallChange, scrollBar.Minimum);
			}
			else
			{
				newValue = (int)Math.Min(scrollBar.Value + scrollBar.SmallChange, (int)(scrollBar.Maximum - scrollBar.LargeChange + 1));
			}

			scrollBar.Value = newValue;
		}
		#endregion Overrides

		#region Events
		private void _hScrollBar_ValueChanged(object sender, EventArgs e)
		{
			if (!_canRaiseEvent)
				return;

			RaiseEvent_HScrollChanged();
		}

		private void _vScrollBar_ValueChanged(object sender, EventArgs e)
		{
			if (!_canRaiseEvent)
				return;

			RaiseEvent_VScrollChanged();
		}

		private void OnProcessParametersChanged()
		{
			try
			{
				this.SuspendLayout();

				if (_parameters == null)
				{
					_hScrollBar.Visible = false;
					_vScrollBar.Visible = false;
				}
				else
				{

					_hScrollBar.Visible = _parameters.HSParams.Visible;
					_vScrollBar.Visible = _parameters.VSParams.Visible;

					int hsWidth = this.Width;
					int vsHeight = this.Height;

					if (_hScrollBar.Visible)
						vsHeight = vsHeight - _hScrollBar.Height - 1;
					if (_vScrollBar.Visible)
						hsWidth = hsWidth - _vScrollBar.Width - 1;

					// update size
					if (_hScrollBar.Visible)
					{
						_hScrollBar.Width = hsWidth;

						_hScrollBar.Value = (int)(_parameters.HSParams.ThumbSpanPositionFactor*100);
					}

					if (_vScrollBar.Visible)
					{						
						_vScrollBar.Height = vsHeight;

						_vScrollBar.Value = (int)(_parameters.VSParams.ThumbSpanPositionFactor*100);
					}
				}
			}
			catch
			{
			}
			finally
			{
				this.ResumeLayout(false);

				//this.Invalidate(true);
			}			
		}
		#endregion Events

		#region Methods
		public void View(ViewerExParams parameters)
		{
			try
			{
				_canRaiseEvent = false;

				this.Parameters = parameters;
			}
			catch
			{
				// nothing
			}
			finally
			{
				_canRaiseEvent = true;
			}
		}

		private void ProcessOnPaint(PaintEventArgs e)
		{
			e.Graphics.Clear(base.BackColor);

			Image image = this.Image;
			if (image != null)
			{
				e.Graphics.DrawImage(image, _parameters.X, _parameters.Y);
			}			
		}
		#endregion Methods

		#region Helpers
		private void RaiseEvent_HScrollChanged()
		{
			if (HScrollChanged != null)
				HScrollChanged(this, new ViewerExEventArgs(_hScrollBar.Value*0.01f, _vScrollBar.Value*0.01f));
		}

		private void RaiseEvent_VScrollChanged()
		{
			if (VScrollChanged != null)
				VScrollChanged(this, new ViewerExEventArgs(_hScrollBar.Value*0.01f, _vScrollBar.Value*0.01f));
		}

		private void RaiseEvent_SizeChanged()
		{
			if (SizeChanged != null)
				SizeChanged(this, new ViewerExEventArgs(this.Size));
		}
		#endregion Helpers		
	}

	public delegate void ViewerExEventHandlers(object sender, ViewerExEventArgs e);

	public class ViewerExEventArgs : EventArgs
	{
		public Size Sz;
		public float HSPosFactor = 0;
		public float VSPosFactor = 0;

		public ViewerExEventArgs()
		{
		}

		public ViewerExEventArgs(Size sz)
		{
			Sz = sz;
		}

		public ViewerExEventArgs(float hsPosFactor, float vsPosFactor)
		{
			HSPosFactor = hsPosFactor;
			VSPosFactor = vsPosFactor;
		}
	}

	public class ViewerExParams
	{
		public ScrollBarParams HSParams = new ScrollBarParams();
		public ScrollBarParams VSParams = new ScrollBarParams();
		public Image Image = null;
		public float X = 0;
		public float Y = 0;

		public ViewerExParams()
		{
		}

		public ViewerExParams(ScrollBarParams hs, ScrollBarParams vs, Image image, float x, float y)
		{
			HSParams = hs;
			VSParams = vs;
			this.Image = image;
			X = x;
			Y = y;
		}
	}
}
