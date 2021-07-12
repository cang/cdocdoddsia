using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;

namespace SIA.UI.Components
{
	[Flags]
	internal enum ColorStopFlag
	{
		Normal = 1,
		Selected = 2,
		Removed = 4
	}
			
	/// <summary>
	/// The GradientColorWidget class provides functionality for displaying, user-interaction
    /// the gradient colors. This class is used for PseudoColor operations
	/// </summary>
	public class GradientColorWidget 
        : System.Windows.Forms.UserControl
	{
		#region internal class
		
		internal class HitTestInfo
		{
			private ColorStop _stop = null;
			internal ColorStop ColorStop
			{
				get {return _stop;}
				set {_stop = value;}
			}

			internal HitTestInfo()
			{
			}
		}
		#endregion

		#region member attribute

		private ColorStopCollection _colorStops;
		private ColorStop _selectedStop;
		private RectangleF _rcColorDisplay;

		private ColorBlend _colorBlend = new ColorBlend();
		private Size _widgetSize = Size.Empty;

		private bool _updateLocked = false;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#endregion

		#region public event handler

		[Browsable(true), Category("Gradient Color Widget")]
		public event EventHandler SelectedColorStopChanged = null;
		[Browsable(true), Category("Gradient Color Widget")]
		public event EventHandler SelectedStopPositionChanged = null;
		[Browsable(true), Category("Gradient Color Widget")]
		public event EventHandler SelectedStopColorChanged = null;

		#endregion

		#region constructor and destructor

		public GradientColorWidget()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// initialize control styles
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);

			// Initialize default color stops
			this.ResetStops();
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

		#endregion

		#region public properties

		public ColorBlend ColorBlend
		{
			get 
			{
				int num_colors = _colorStops.Count;
				ColorBlend colorBlend = new ColorBlend();
				ColorStopCollection stops = new ColorStopCollection(); 
				for (int i=0; i<num_colors; i++)
					stops.Add(_colorStops[i]);
			
				stops.Sort();

				Color[] colors = new Color[num_colors];
				float[] positions = new float[num_colors];
				for (int i=0; i<num_colors; i++)
				{
					colors[i] = stops[i].Color;
					positions[i] = stops[i].Position;
				}
				colorBlend.Colors = colors;
				colorBlend.Positions = positions;
				return colorBlend;
			}
		}

		public ColorStop SelectedColorStop
		{
			get {return _selectedStop;}		
			set
			{
				if (_selectedStop != null)
					_selectedStop.Selected = false;

				_selectedStop = value;
				if (_selectedStop != null)
					_selectedStop.Selected = true;

				OnSelectedColorStopChanged();
			}
		}

		protected virtual void OnSelectedColorStopChanged()
		{
			if (this.SelectedColorStopChanged != null)
				this.SelectedColorStopChanged(this, EventArgs.Empty);
		}

		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// GradientColorWidget
			// 
			this.Name = "GradientColorWidget";
			this.Size = new System.Drawing.Size(268, 52);

		}
		#endregion

		#region Color Stop helper
		
		public ColorStop AddStop(Color value, float position)
		{
			int index = _colorStops.Add(new ColorStop(value, position));
			ColorStop stop = _colorStops[index];
			RefreshColorBlend();
			return stop;
		}

		public void RemoveStop(ColorStop stop)
		{
			if (stop == this.SelectedColorStop)
				this.SelectStop(null);
			_colorStops.Remove(stop);
			RefreshColorBlend();
		}

		public void ClearStops()
		{
			_colorStops.Clear();
		}

		public void ResetStops()
		{
			try
			{
				BeginUpdate();

				_colorStops = new ColorStopCollection();
				AddStop(Color.Green, 0.0F);
				AddStop(Color.White, 1.0F);
			}
			catch(System.Exception exp)
			{
				throw exp;
			}
			finally
			{
				EndUpdate();
			}
		}

		private void SelectStop(ColorStop stop)
		{
			this.SelectedColorStop = stop;
		}

		public bool CanRemoveStops()
		{
			return _colorStops.Count > 2;
		}

		protected virtual void RefreshColorBlend()
		{
			if (IsUpdateLocked() == true)
				return;

			ColorStopCollection stops = new ColorStopCollection(); 
			ColorBlend colorBlend = new ColorBlend();
			int num_colors = _colorStops.Count;

			for (int i=0; i<num_colors; i++)
				stops.Add(_colorStops[i]);

			// sort color stops
			stops.Sort();

			if (stops[0].Position != 0.0)
				stops.Insert(0, new ColorStop(stops[0].Color, 0.0F));

			if (stops[stops.Count-1].Position != 1.0F)
				stops.Add(new ColorStop(stops[stops.Count-1].Color, 1.0F));

			num_colors = stops.Count;	
			float[] positions = new float[num_colors];
			Color[] colors = new Color[num_colors];
			for (int i=0; i<num_colors; i++)
			{
				positions[i] = stops[i].Position;
				colors[i] = stops[i].Color;
			}

			colorBlend.Positions = positions;
			colorBlend.Colors = colors;

			_colorBlend = colorBlend;
		}

		#endregion

		#region public methods
		
		public void BeginUpdate()
		{
			lock(this)
			{
				_updateLocked = true;
			}
		}

		public bool IsUpdateLocked()
		{
			lock(this)
				return _updateLocked;
		}

		public void EndUpdate()
		{
			lock(this)
			{
				_updateLocked = false;
			}

			this.RefreshColorBlend();
			this.Invalidate(true);			
		}

		#region tooltip helper
		
		public String GetTooltipText(int x, int y)
		{
			HitTestInfo htInfo = this.HitTest(x, y);
			if (htInfo.ColorStop == null)
				return "Click to add new stop";
			else
				return String.Format("Stop ({0}, Location:{1})", 
					htInfo.ColorStop.Color.ToString(), (int)Math.Round(htInfo.ColorStop.Position * 100.0F));
		}

		public String GetTooltipText()
		{
			SIA.Common.Native.NativeMethods.POINT pt = new SIA.Common.Native.NativeMethods.POINT();
			SIA.Common.Native.NativeMethods.GetCursorPos(pt);
			Point ptScreen = new Point(pt.x, pt.y);
			Point ptClient = this.PointToClient(ptScreen);
			return GetTooltipText(ptClient.X, ptClient.Y);
		}

		#endregion
		
		#endregion

		#region override routines
		protected override void OnPaint(PaintEventArgs e)
		{
			try
			{
				base.OnPaint (e);
			}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);
			}

			try
			{
				DrawColorRectangle(e.Graphics, _rcColorDisplay, _colorStops);
			}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);
			}

			try
			{
				foreach(ColorStop stop in _colorStops)
					if (stop != null && stop.Removed == false)
						DrawStop(e.Graphics, stop);
			}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged (e);
			this.RecalcLayout();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown (e);

			HitTestInfo htInfo = HitTest(e.X, e.Y);

			if (htInfo.ColorStop != null)
			{
				BeginCapture();
				SelectStop(htInfo.ColorStop);
			}
			else 
			{	
				RectangleF stopArea = StopArea();
				if (stopArea.Contains(e.X, e.Y))
				{
					BeginCapture();

					float pos = ScreenLocToColorPosition(e.X, e.Y);
					Color color = LinearInterpolateColor(pos);
					ColorStop stop = AddStop(Color.Black, pos);
					SelectStop(stop);
				
					// redraw stops
					this.Invalidate(true);
				}			
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove (e);
			
			if (this.SelectedColorStop != null)
			{
				this.Cursor = Cursors.Hand;

				if (ClientRectangle.Contains(e.X, e.Y))
				{
					if (this.IsCaptured())
					{
						// unmarked removed flag
						this.SelectedColorStop.Removed = false;
						// adjust stop's position
						float pos = ScreenLocToColorPosition(e.X, e.Y);
						this.SelectedColorStop.Position = pos;
						if (this.SelectedStopPositionChanged != null)
							this.SelectedStopPositionChanged(this, EventArgs.Empty);
					}
					else
					{
						HitTestInfo htInfo = this.HitTest(e.X, e.Y);
						if (htInfo.ColorStop != null)
							this.Cursor = Cursors.Hand;
						else
							this.Cursor = Cursors.Default;
					}
				}
				else
				{
					if (this.CanRemoveStops())
					{
						// temporary remove stop
						this.SelectedColorStop.Removed = true;						
					}
				}

				// redraw stops
				this.Invalidate(true);
			}
			else
			{
				HitTestInfo htInfo = this.HitTest(e.X, e.Y);
				if (htInfo.ColorStop != null)
					this.Cursor = Cursors.Hand;
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp (e);

			if (this.SelectedColorStop != null)
			{
				if (ClientRectangle.Contains(e.X, e.Y))
				{
					// adjust stop's position
					float pos = ScreenLocToColorPosition(e.X, e.Y);
					this.SelectedColorStop.Position = pos;
					if (this.SelectedStopPositionChanged != null)
						this.SelectedStopPositionChanged(this, EventArgs.Empty);

					// refresh color blend
					RefreshColorBlend();
				}
				else
				{
					if (this.CanRemoveStops())
					{
						// remove stop
						RemoveStop(this.SelectedColorStop);
						// select nothing
						SelectStop(null);
					}
				}

				// redraw stops
				this.Invalidate(true);
			}

			if (this.IsCaptured())
				this.EndCapture();
		}
		
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter (e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave (e);
		}

		protected override void OnDoubleClick(EventArgs e)
		{
			base.OnDoubleClick (e);			

			if (this.SelectedColorStop != null)
				this.OnSelectedColorStopDoubleClick();
		}

		#endregion

		#region virtual routines		
		protected virtual void OnSelectedColorStopDoubleClick()
		{
			using (System.Windows.Forms.ColorDialog dlg = new ColorDialog())
			{
				ColorStop stop = this.SelectedColorStop;
				dlg.Color = stop.Color;
				if (dlg.ShowDialog(this) == DialogResult.OK)
				{
					stop.Color = dlg.Color;
					if (this.SelectedStopColorChanged != null)
						this.SelectedStopColorChanged(this, EventArgs.Empty);

					RefreshColorBlend();
					this.Invalidate();
				}
			}
		}

		#endregion

		#region internal routines

		private void DrawColorRectangle(Graphics graph, RectangleF rcDraw, ColorStopCollection stops)
		{
			LinearGradientBrush brush = new LinearGradientBrush(rcDraw, Color.Black, Color.White, 0.0F, false);
			if (_colorBlend.Colors.Length >= 2 &&
				_colorBlend.Colors.Length == _colorBlend.Positions.Length)
				brush.InterpolationColors = _colorBlend;			
			graph.FillRectangle(brush, rcDraw);
			brush.Dispose();
		}

		private void DrawStop(Graphics graph, ColorStop stop)
		{
			RectangleF rcDraw = StopRectangle(stop);

			float stop_X = rcDraw.Left;
			float stop_Y = rcDraw.Top;
			float stop_width = rcDraw.Width;
			float stop_height = rcDraw.Height;

			PointF[] points = new PointF[4];
			points[0] = new PointF(stop_X + stop_width/2.0F, stop_Y);
			points[1] = new PointF(stop_X + stop_width, stop_Y + stop_height);
			points[2] = new PointF(stop_X , stop_Y + stop_height);
			points[3] = points[0];
			
			using (Brush brush = new SolidBrush(stop.Color))
				graph.FillPolygon(brush, points, FillMode.Alternate);

			graph.DrawPolygon(stop.Selected ? Pens.Red : Pens.Green, points);
			// graph.DrawRectangle(Pens.Green, Rectangle.Round(rcDraw));
		}

		private void BeginCapture()
		{
			this.Capture = true;
		}

		private bool IsCaptured()
		{
			return this.Capture;
		}

		private void EndCapture()
		{
			this.Capture = false;
		}

		private void RecalcLayout()
		{
			Size mySize = this.Size;
			int widget_width = (int)(mySize.Height * 0.3F);
			int widget_height = (int)(mySize.Height * 0.3F);
			_widgetSize = new Size(widget_width, widget_height);

			Size size = new Size(mySize.Width - 2*widget_width, (int)(mySize.Height * 0.7F));
			Point loc = new Point(_widgetSize.Width, 0);
			_rcColorDisplay = new RectangleF(loc, size);
		}

		private HitTestInfo HitTest(int x, int y)
		{
			Point pt = new Point(x, y);
			
			// validate point
			HitTestInfo htInfo = new HitTestInfo();
			foreach(ColorStop stop in _colorStops)
			{
				if (stop.Removed == false)
				{
					RectangleF rcStop = StopRectangle(stop);
					if (rcStop.Contains(pt.X, pt.Y))
					{
						htInfo.ColorStop = stop;
						break;
					}
				}
			}

			return htInfo;
		}

		private RectangleF StopRectangle(ColorStop stop)
		{
			RectangleF rcContainer = _rcColorDisplay;
			float client_width = rcContainer.Width;
			float client_height = rcContainer.Height;
			float scaleFX = client_width / 1.0F;
			
			float stop_X = rcContainer.Left + stop.Position * scaleFX;
			float stop_Y = rcContainer.Bottom;
			float stop_width = _widgetSize.Width;
			float stop_height = _widgetSize.Height;

			stop_X -= stop_width / 2.0F;
			
			return new RectangleF(stop_X, stop_Y, stop_width, stop_height);
		}

		private RectangleF StopArea()
		{
			RectangleF rcContainer = _rcColorDisplay;
			return new RectangleF(rcContainer.Left - _widgetSize.Width/2.0F, rcContainer.Bottom, rcContainer.Width, _widgetSize.Height);
		}

		private float ScreenLocToColorPosition(int x, int y)
		{
			RectangleF rcContainer = _rcColorDisplay;
			float client_width = rcContainer.Width;
			float client_height = rcContainer.Height;
			float scaleFX = 1.0F / client_width;

			if (x < rcContainer.Left)
				return 0.0F;
			else if (x > rcContainer.Right)
				return 1.0F;
			else
			{
				float deltaX = x - rcContainer.Left;
				return deltaX * scaleFX;
			}
		}

		private Color LinearInterpolateColor(float position)
		{
			LinearGradientBrush brush = new LinearGradientBrush(_rcColorDisplay, Color.Black, Color.White, 0.0F, false);
			brush.InterpolationColors = _colorBlend;			
			brush.Dispose();
			return Color.Red;
		}

		#endregion
	}

	/// <summary>
	/// Color Stop class
	/// </summary>
	public class ColorStop
	{
		private Color _color = Color.Black;
		private float _position = 0.0F;
		internal ColorStopFlag Flags = ColorStopFlag.Normal;
		
		public Color Color
		{
			get {return _color;}
			set
			{
				_color = value;
				OnColorChanged();
			}
		}

		protected virtual void OnColorChanged()
		{
		}

		public float Position
		{
			get {return _position;}
			set
			{
				_position = value;
				OnPositionChanged();
			}
		}

		protected virtual void OnPositionChanged()
		{
		}

		internal bool Selected
		{
			get {return (this.Flags & ColorStopFlag.Selected) == ColorStopFlag.Selected;}
			set 
			{
				if (value)
					this.Flags |= ColorStopFlag.Selected;
				else
					this.Flags &= ~ColorStopFlag.Selected;
			}
		}

		internal bool Removed
		{
			get {return (this.Flags & ColorStopFlag.Removed) == ColorStopFlag.Removed;}
			set 
			{
				if (value)
					this.Flags |= ColorStopFlag.Removed;
				else
					this.Flags &= ~ColorStopFlag.Removed;
			}
		}

		public ColorStop(Color color, float pos)
		{
			_color = color;
			_position = pos;
		}
	};
	

	public class ColorStopCollection : System.Collections.CollectionBase
	{
		public class ColorStopComparer : IComparer
		{
			public int Compare(object obj1, object obj2)
			{
				float value = ((ColorStop)obj1).Position - ((ColorStop)obj2).Position;
				if (value < 0)
					return -1;
				else if (value > 0)
					return 1;
				return 0;
			}
		}

		public int Add(ColorStop stop)
		{
			return base.List.Add(stop);
		}

		public void Insert(int index, ColorStop stop)
		{
			base.List.Insert(index, stop);
		}

		public void Remove(ColorStop stop)
		{
			base.List.Remove(stop);
		}

		public ColorStop this[int index]
		{
			get
			{
				return (ColorStop)base.List[index];
			}
		}

		public void Sort()
		{
			ColorStop[] items = new ColorStop[this.Count];
			int index = 0;
			foreach(ColorStop stop in base.List)
				items[index++] = stop;

			System.Array.Sort(items, new ColorStopComparer());

			base.List.Clear();
			for(int i=0; i<items.Length; i++)
				base.List.Add(items[i]);
		}
	}
		
}
