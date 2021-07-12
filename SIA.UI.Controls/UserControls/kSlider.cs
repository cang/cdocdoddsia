using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace SIA.UI.Controls.UserControls
{
	/// <summary>
	/// Summary description for kSlider.
	/// </summary>
	public class kSlider : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.PictureBox RightThumb;
		private System.Windows.Forms.PictureBox LeftThumb;
		private System.Windows.Forms.Label UserThumb; 

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		
		private int _leftValue ;
		private int _rightValue;
		private bool _bClicked = false;
		private Point _oldPoint = Point.Empty;
		private Point _newPoint = Point.Empty;
		private int _max = 100;
		private int _min = 0;
		
		private bool m_bLocked = false;
		
		public kSlider()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			InitializeControls();			
		}

		private void InitializeControls()
		{
			this.LeftThumb.Location =new Point(this.UserThumb.Location.X+UserThumb.Width/2 - LeftThumb.Width,1);
			this.RightThumb.Location =new Point(this.UserThumb.Location.X+UserThumb.Width/2  ,1);				
			this.UserThumb.Width = this.Width - 2*LeftThumb.Width;
			this.UserThumb.Location = new Point(LeftThumb.Width,0);
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

		public int CurrentLeftValue()
		{			
			_leftValue = (LeftThumb.Location.X + LeftThumb.Width - UserThumb.Location.X);
			_leftValue = _leftValue*(_max - _min)/UserThumb.Width ;
			return _leftValue;
		}
		
		public int CurrentRightValue()
		{			
			_rightValue = (RightThumb.Location.X - UserThumb.Location.X);
			_rightValue = _rightValue* (_max - _min)/UserThumb.Width ;
			return _rightValue;
		}

		public void SetRightThumbPos(int val)
		{
			if (val>CurrentLeftValue() && val<=_max)
			{
				int Temp = val * UserThumb.Width /(_max - _min);
				RightThumb.Location = new Point(Temp + UserThumb.Location.X);			
			}
		}

		public void SetLeftThumbPos(int val)
		{
			if (val>=_min && val<CurrentRightValue())
			{
				int Temp = (val * UserThumb.Width /(_max - _min)) - ThumbSize.Width;
				LeftThumb.Location = new Point(Temp + UserThumb.Location.X);			
			}
		}

		[Browsable(true), Category("kSlider")]
		public int MinValue
		{
			get{return _min;}
			set{_min = value;}
		}

		[Browsable(true), Category("kSlider")]
		public int MaxValue
		{
			get{return _max;}
			set {_max = value;}
		}

		[Browsable(true), Category("kSlider")]
		public Size ThumbSize
		{
			get{return LeftThumb.Size;}
		}

		[Browsable(true), Category("kSlider")]
		public bool ShowTrack
		{
			get{return UserThumb.Visible;}
			set{UserThumb.Visible = value;}
		}

		public delegate void ValueChangingEventHandler(Object sender, int val);
		public delegate void ValueChangedEventHandler(Object sender, int val);

		[Browsable(true), Category("Action")]
		public event ValueChangingEventHandler LeftValueChanging;

		[Browsable(true), Category("Action")]
		public event ValueChangedEventHandler LeftValueChanged;

		[Browsable(true), Category("Action")]
		public event ValueChangingEventHandler RightValueChanging;

		[Browsable(true), Category("Action")]
		public event ValueChangedEventHandler RightValueChanged;



		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(kSlider));
			this.RightThumb = new System.Windows.Forms.PictureBox();
			this.LeftThumb = new System.Windows.Forms.PictureBox();
			this.UserThumb = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// RightThumb
			// 
			this.RightThumb.BackColor = System.Drawing.Color.Transparent;
			this.RightThumb.Cursor = System.Windows.Forms.Cursors.SizeWE;
			this.RightThumb.ForeColor = System.Drawing.Color.Transparent;
			this.RightThumb.Image = ((System.Drawing.Image)(resources.GetObject("RightThumb.Image")));
			this.RightThumb.Location = new System.Drawing.Point(172, 8);
			this.RightThumb.Name = "RightThumb";
			this.RightThumb.Size = new System.Drawing.Size(12, 12);
			this.RightThumb.TabIndex = 5;
			this.RightThumb.TabStop = false;
			this.RightThumb.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Thumb_MouseUp);
			this.RightThumb.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RightThumb_MouseMove);
			this.RightThumb.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Thumb_MouseDown);
			// 
			// LeftThumb
			// 
			this.LeftThumb.BackColor = System.Drawing.Color.Transparent;
			this.LeftThumb.Cursor = System.Windows.Forms.Cursors.SizeWE;
			this.LeftThumb.Image = ((System.Drawing.Image)(resources.GetObject("LeftThumb.Image")));
			this.LeftThumb.Location = new System.Drawing.Point(156, 8);
			this.LeftThumb.Name = "LeftThumb";
			this.LeftThumb.Size = new System.Drawing.Size(12, 12);
			this.LeftThumb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.LeftThumb.TabIndex = 4;
			this.LeftThumb.TabStop = false;
			this.LeftThumb.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Thumb_MouseUp);
			this.LeftThumb.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LeftThumb_MouseMove);
			this.LeftThumb.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Thumb_MouseDown);
			// 
			// UserThumb
			// 
			this.UserThumb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.UserThumb.BackColor = System.Drawing.Color.Black;
			this.UserThumb.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.UserThumb.ForeColor = System.Drawing.Color.Black;
			this.UserThumb.Location = new System.Drawing.Point(12, 8);
			this.UserThumb.Name = "UserThumb";
			this.UserThumb.Size = new System.Drawing.Size(380, 1);
			this.UserThumb.TabIndex = 3;
			this.UserThumb.Text = "|";
			this.UserThumb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.UserThumb.Visible = false;
			// 
			// kSlider
			// 
			this.Controls.Add(this.UserThumb);
			this.Controls.Add(this.RightThumb);
			this.Controls.Add(this.LeftThumb);
			this.Name = "kSlider";
			this.Size = new System.Drawing.Size(412, 24);
			this.Load += new System.EventHandler(this.kSlider_Load);
			this.SizeChanged += new System.EventHandler(this.OnSizeChanged);
			this.ResumeLayout(false);

		}
		#endregion

		private void Thumb_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (Lock) return;

			if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
			{
				_bClicked = true;
				_oldPoint.X = e.X;
				_oldPoint.Y = e.Y;			
			}
		}	

		private void Thumb_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (Lock) return;

			_bClicked = false;
			/* Fire event if any */
			if (sender==LeftThumb && LeftValueChanged!=null)
				LeftValueChanged(this, _leftValue);
			else if (sender==RightThumb && RightValueChanged!=null)
				RightValueChanged(this, _rightValue);

			if ((System.Windows.Forms.Control.ModifierKeys & Keys.Shift) == Keys.Shift ||
				(System.Windows.Forms.Control.ModifierKeys & Keys.Control) == Keys.Control)
			{
				if (sender==LeftThumb && RightValueChanged!=null)
					RightValueChanged(this, _rightValue);
				else if (sender==RightThumb && LeftValueChanged!=null)
					LeftValueChanged(this, _leftValue);
			}																			   
		}

		private void LeftThumb_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (Lock) return;

			if (_bClicked && (e.Button & MouseButtons.Left) == MouseButtons.Left)
			{
				bool bUpdateRightThumb = false;
				/* update left thumb position */
				UpdateLeftThumbPos(new Point(e.X, e.Y));
				int deltaX = e.X - _oldPoint.X;

				if ((System.Windows.Forms.Control.ModifierKeys & Keys.Shift) == Keys.Shift)
				{
					UpdateRightThumbPos(deltaX);
					bUpdateRightThumb = true;
				}
				else if ((System.Windows.Forms.Control.ModifierKeys & Keys.Control) == Keys.Control)
				{
					UpdateRightThumbPos(-deltaX);
					bUpdateRightThumb = true;
				}
				
				/* update left value */
				CurrentLeftValue();			

				/* update right value if any */
				if (bUpdateRightThumb)
					CurrentRightValue();

				/* Fire event if any */
				if (LeftValueChanging != null && _leftValue>=_min	 && _leftValue<_rightValue) 
					LeftValueChanging(sender, _leftValue);

				/* Fire event if any */
				if (bUpdateRightThumb && RightValueChanging!=null && _rightValue>_leftValue && _rightValue<=_max)
					RightValueChanging(sender, _rightValue);
			}
		}

		private void RightThumb_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (Lock) return;

			if (_bClicked && (e.Button & MouseButtons.Left) == MouseButtons.Left) 
			{ 
				bool bUpdateLeftThumb = false;
				/* update right thumb position */
				UpdateRightThumbPos(new Point(e.X, e.Y));
				int deltaX = e.X - _oldPoint.X;

				if ((System.Windows.Forms.Control.ModifierKeys & Keys.Shift) == Keys.Shift)
				{
					UpdateLeftThumbPos(deltaX);
					bUpdateLeftThumb = true;
				}
				else if ((System.Windows.Forms.Control.ModifierKeys & Keys.Control) == Keys.Control)
				{
					UpdateLeftThumbPos(-deltaX);
					bUpdateLeftThumb = true;
				}

				/* update right value */
				CurrentRightValue();

				/* update left value if any */
				if (bUpdateLeftThumb)
					CurrentLeftValue();

				/* Fire event if any */
				if (RightValueChanging!=null && _rightValue>_leftValue && _rightValue<=_max)
					RightValueChanging(sender, _rightValue);

				/* Fire event if any */
				if (LeftValueChanging != null && _leftValue>=_min	 && _leftValue<_rightValue) 
					LeftValueChanging(sender, _leftValue);
			}
		}

		private void UpdateLeftThumbPos(int deltaX)
		{
			Point newLoc;
			int YPos = UserThumb.Location.Y;
			if (LeftThumb.Location.X + LeftThumb.Width + deltaX <= UserThumb.Location.X)
				newLoc = new Point(UserThumb.Location.X - LeftThumb.Width, YPos);
			else if (LeftThumb.Location.X + LeftThumb.Width + deltaX >= RightThumb.Location.X)
				newLoc = new Point(RightThumb.Location.X - LeftThumb.Width, YPos);
			else
				newLoc = new Point(LeftThumb.Location.X + deltaX, YPos);

//			System.Diagnostics.Debug.WriteLine(LeftThumb.Location.ToString());
			
			if (newLoc.X >= 0 && newLoc.X <= RightThumb.Location.X - LeftThumb.Width)
				LeftThumb.Location = newLoc;
		}

		private void UpdateLeftThumbPos(Point pos)
		{
			Point newLoc;
			int YPos = UserThumb.Location.Y;
			if (LeftThumb.Location.X + LeftThumb.Width <= UserThumb.Location.X && pos.X < _oldPoint.X)
				newLoc = new Point(UserThumb.Location.X - LeftThumb.Width, YPos);
			else if (LeftThumb.Location.X+LeftThumb.Width >= RightThumb.Location.X  && pos.X > _oldPoint.X)
				newLoc = new Point(RightThumb.Location.X-LeftThumb.Width,  YPos);
			else
				newLoc = new Point(LeftThumb.Location.X +(pos.X - _oldPoint.X),YPos);

//			System.Diagnostics.Debug.WriteLine(LeftThumb.Location.ToString());
			if (newLoc.X >= 0 && newLoc.X <= RightThumb.Location.X - LeftThumb.Width)
				LeftThumb.Location = newLoc;
		}

		private void UpdateRightThumbPos(Point pos)
		{
			Point newLoc;
			int YPos = UserThumb.Location.Y;
			if (RightThumb.Location.X <= LeftThumb.Location.X+LeftThumb.Width && pos.X < _oldPoint.X)
				newLoc = new Point(LeftThumb.Location.X+LeftThumb.Width, YPos);
			else if (RightThumb.Location.X >=UserThumb.Location.X+UserThumb.Width && pos.X > _oldPoint.X)
				newLoc = new Point(UserThumb.Location.X+UserThumb.Width, YPos);
			else
				newLoc = new Point(RightThumb.Location.X +(pos.X - _oldPoint.X), YPos);

//			System.Diagnostics.Debug.WriteLine(LeftThumb.Location.ToString());
			if (newLoc.X >= LeftThumb.Location.X + LeftThumb.Width && newLoc.X <=UserThumb.Location.X+UserThumb.Width)
				RightThumb.Location = newLoc;
		}

		private void UpdateRightThumbPos(int deltaX)
		{
			Point newLoc;
			int YPos = UserThumb.Location.Y;
			if (RightThumb.Location.X + deltaX <= LeftThumb.Location.X+LeftThumb.Width)
				newLoc = new Point(LeftThumb.Location.X+LeftThumb.Width, YPos);
			else if (RightThumb.Location.X + deltaX >= UserThumb.Location.X+UserThumb.Width)
				newLoc = new Point(UserThumb.Location.X+UserThumb.Width, YPos);
			else
				newLoc = new Point(RightThumb.Location.X + deltaX, YPos);
			
//			System.Diagnostics.Debug.WriteLine(LeftThumb.Location.ToString());
			if (newLoc.X >= LeftThumb.Location.X + LeftThumb.Width && newLoc.X <=UserThumb.Location.X+UserThumb.Width)
				RightThumb.Location = newLoc;
		}

		
		private void OnSizeChanged(object sender, System.EventArgs e)
		{
			InitializeControls();
		}

		private void kSlider_Load(object sender, System.EventArgs e)
		{
		
		}

		public bool Lock
		{
			get {return m_bLocked;}
			set {m_bLocked = value;}
		}
	}
}
