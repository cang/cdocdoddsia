using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace SIA.UI.MaskEditor.UserControls.ColorPicker
{
	/// <summary>
	/// Summary description for kColorPicker.
	/// </summary>
	/// 
	public class kColorPicker : System.Windows.Forms.UserControl
	{
		#region Members
		private System.Windows.Forms.Label button2;
		private System.Windows.Forms.Label button4;
		private System.Windows.Forms.Label button5;
		private System.Windows.Forms.Label button6;
		private System.Windows.Forms.Label button7;
		private System.Windows.Forms.Label button8;
		private System.Windows.Forms.Label button9;
		private System.Windows.Forms.Label button10;
		private System.Windows.Forms.Label button11;
		private System.Windows.Forms.Label button12;
		private System.Windows.Forms.Label button13;
		private System.Windows.Forms.Label button14;
		private System.Windows.Forms.Label button15;
		private System.Windows.Forms.Label button16;
		private System.Windows.Forms.Label button17;
		private System.Windows.Forms.Label button1;
		private System.Windows.Forms.Label btnSelectedColors;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private int _selPenColor = 0;
		private int _selBrushColor = 1;
		private const int TotalColors = 16;
		private Color[] _Colors = new Color[TotalColors];

		public event EventHandler	PenColorChanged = null;
		public event EventHandler	BrushColorChanged = null;

		#endregion
		
		#region Constructor and destructor

		public kColorPicker()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.button1 = new System.Windows.Forms.Label();
			this.button2 = new System.Windows.Forms.Label();
			this.btnSelectedColors = new System.Windows.Forms.Label();
			this.button4 = new System.Windows.Forms.Label();
			this.button5 = new System.Windows.Forms.Label();
			this.button6 = new System.Windows.Forms.Label();
			this.button7 = new System.Windows.Forms.Label();
			this.button8 = new System.Windows.Forms.Label();
			this.button9 = new System.Windows.Forms.Label();
			this.button10 = new System.Windows.Forms.Label();
			this.button11 = new System.Windows.Forms.Label();
			this.button12 = new System.Windows.Forms.Label();
			this.button13 = new System.Windows.Forms.Label();
			this.button14 = new System.Windows.Forms.Label();
			this.button15 = new System.Windows.Forms.Label();
			this.button16 = new System.Windows.Forms.Label();
			this.button17 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.Location = new System.Drawing.Point(37, 2);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(16, 16);
			this.button1.TabIndex = 1;
			this.button1.Tag = "0";
			this.button1.Text = "button1";
			// 
			// button2
			// 
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button2.Location = new System.Drawing.Point(37, 20);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(16, 16);
			this.button2.TabIndex = 9;
			this.button2.Tag = "8";
			this.button2.Text = "button1";
			// 
			// btnSelectedColors
			// 
			this.btnSelectedColors.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSelectedColors.Location = new System.Drawing.Point(1, 2);
			this.btnSelectedColors.Name = "btnSelectedColors";
			this.btnSelectedColors.Size = new System.Drawing.Size(34, 34);
			this.btnSelectedColors.TabIndex = 0;
			// 
			// button4
			// 
			this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button4.Location = new System.Drawing.Point(55, 2);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(16, 16);
			this.button4.TabIndex = 2;
			this.button4.Tag = "1";
			this.button4.Text = "button1";
			// 
			// button5
			// 
			this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button5.Location = new System.Drawing.Point(55, 20);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(16, 16);
			this.button5.TabIndex = 10;
			this.button5.Tag = "9";
			this.button5.Text = "button1";
			// 
			// button6
			// 
			this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button6.Location = new System.Drawing.Point(73, 2);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(16, 16);
			this.button6.TabIndex = 3;
			this.button6.Tag = "2";
			this.button6.Text = "button1";
			// 
			// button7
			// 
			this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button7.Location = new System.Drawing.Point(91, 20);
			this.button7.Name = "button7";
			this.button7.Size = new System.Drawing.Size(16, 16);
			this.button7.TabIndex = 12;
			this.button7.Tag = "11";
			this.button7.Text = "button1";
			// 
			// button8
			// 
			this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button8.Location = new System.Drawing.Point(91, 2);
			this.button8.Name = "button8";
			this.button8.Size = new System.Drawing.Size(16, 16);
			this.button8.TabIndex = 4;
			this.button8.Tag = "3";
			this.button8.Text = "button1";
			// 
			// button9
			// 
			this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button9.Location = new System.Drawing.Point(73, 20);
			this.button9.Name = "button9";
			this.button9.Size = new System.Drawing.Size(16, 16);
			this.button9.TabIndex = 11;
			this.button9.Tag = "10";
			this.button9.Text = "button1";
			// 
			// button10
			// 
			this.button10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button10.Location = new System.Drawing.Point(127, 20);
			this.button10.Name = "button10";
			this.button10.Size = new System.Drawing.Size(16, 16);
			this.button10.TabIndex = 14;
			this.button10.Tag = "13";
			this.button10.Text = "button1";
			// 
			// button11
			// 
			this.button11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button11.Location = new System.Drawing.Point(127, 2);
			this.button11.Name = "button11";
			this.button11.Size = new System.Drawing.Size(16, 16);
			this.button11.TabIndex = 6;
			this.button11.Tag = "5";
			this.button11.Text = "button1";
			// 
			// button12
			// 
			this.button12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button12.Location = new System.Drawing.Point(163, 20);
			this.button12.Name = "button12";
			this.button12.Size = new System.Drawing.Size(16, 16);
			this.button12.TabIndex = 16;
			this.button12.Tag = "15";
			this.button12.Text = "button1";
			// 
			// button13
			// 
			this.button13.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button13.Location = new System.Drawing.Point(163, 2);
			this.button13.Name = "button13";
			this.button13.Size = new System.Drawing.Size(16, 16);
			this.button13.TabIndex = 8;
			this.button13.Tag = "7";
			this.button13.Text = "button1";
			// 
			// button14
			// 
			this.button14.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button14.Location = new System.Drawing.Point(145, 20);
			this.button14.Name = "button14";
			this.button14.Size = new System.Drawing.Size(16, 16);
			this.button14.TabIndex = 15;
			this.button14.Tag = "14";
			this.button14.Text = "button1";
			// 
			// button15
			// 
			this.button15.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button15.Location = new System.Drawing.Point(109, 2);
			this.button15.Name = "button15";
			this.button15.Size = new System.Drawing.Size(16, 16);
			this.button15.TabIndex = 5;
			this.button15.Tag = "4";
			this.button15.Text = "button1";
			// 
			// button16
			// 
			this.button16.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button16.Location = new System.Drawing.Point(109, 20);
			this.button16.Name = "button16";
			this.button16.Size = new System.Drawing.Size(16, 16);
			this.button16.TabIndex = 13;
			this.button16.Tag = "12";
			this.button16.Text = "button1";
			// 
			// button17
			// 
			this.button17.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button17.Location = new System.Drawing.Point(145, 2);
			this.button17.Name = "button17";
			this.button17.Size = new System.Drawing.Size(16, 16);
			this.button17.TabIndex = 7;
			this.button17.Tag = "6";
			this.button17.Text = "button1";
			// 
			// kColorPicker
			// 
			this.Controls.Add(this.button1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.btnSelectedColors);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button5);
			this.Controls.Add(this.button6);
			this.Controls.Add(this.button7);
			this.Controls.Add(this.button8);
			this.Controls.Add(this.button9);
			this.Controls.Add(this.button10);
			this.Controls.Add(this.button11);
			this.Controls.Add(this.button12);
			this.Controls.Add(this.button13);
			this.Controls.Add(this.button14);
			this.Controls.Add(this.button15);
			this.Controls.Add(this.button16);
			this.Controls.Add(this.button17);
			this.Name = "kColorPicker";
			this.Size = new System.Drawing.Size(182, 38);
			this.Load += new System.EventHandler(this.kColorPicker_Load);
			this.ResumeLayout(false);

		}
		#endregion

		#region Drawing handlers
		private void DrawSelectedColor(Graphics graph, Rectangle rect, Color clrPen, Color clrBrush)
		{
			Brush brushBackground = new SolidBrush(this.BackColor);
			Brush penBrush = new SolidBrush(clrPen);
			Brush brushBrush = new SolidBrush(clrBrush);

			Point ptCenter = new Point(rect.Left + rect.Width/2, rect.Top + rect.Height/2);
			int cellWidth = 16;
			int cellHeight = 16;

			int XMargin = 4;
			int YMargin = 4;

			Rectangle rectPen = new Rectangle(XMargin, YMargin, cellWidth, cellHeight);
			Rectangle rectBrush = new Rectangle(rect.Right-XMargin-cellWidth, rect.Bottom-YMargin-cellHeight, cellWidth, cellHeight);

			// clear background
			graph.FillRectangle(brushBackground, rect);
		
			// draw selected colors
			graph.FillRectangle(brushBrush, rectBrush);
			ControlPaint.DrawBorder3D(graph, rectBrush, Border3DStyle.Raised);

			graph.FillRectangle(penBrush, rectPen);
			ControlPaint.DrawBorder3D(graph, rectPen, Border3DStyle.Raised);
			
			// draw border 3D
			ControlPaint.DrawBorder3D(graph, rect, Border3DStyle.Sunken);

			penBrush.Dispose();
			brushBrush.Dispose();
			brushBackground.Dispose();
		}

		private void DrawColor(Graphics graph, Rectangle rect, Color color)
		{
			Brush brush = new SolidBrush(color);
			graph.FillRectangle(brush, rect);
			
			ControlPaint.DrawBorder3D(graph, rect, Border3DStyle.SunkenOuter);
			brush.Dispose();
		}

		#endregion

		#region Event Handlers
		private void kColorPicker_Load(object sender, System.EventArgs e)
		{
			// initialize array of colors
			for (int i=0; i<(TotalColors/2); i++)
			{
				int Value = (i%(TotalColors/2));
				_Colors[i] = Color.FromArgb(((Value & 0x4)>>2)*0x80, ((Value & 0x2)>>1)*0x80, (Value & 0x1)*0x80);
				_Colors[TotalColors/2 + i] = Color.FromArgb(((Value & 0x4)>>2)*0xFF, ((Value & 0x2)>>1)*0xFF, (Value & 0x1)*0xFF);
			}
			// initialize color buttons
			InitializeColorButtons();
		}

		private void ColorButton_MouseDown(object sender, MouseEventArgs e)
		{			
			if (!(sender is Label)) return;
			Label btn = sender as Label;
			int colorID = Int32.Parse(btn.Tag.ToString());
				
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
				this.SelectPenColorID = colorID;
			else if ((e.Button & MouseButtons.Right) == MouseButtons.Right)
				this.SelectedBrushColorID = colorID;
			btnSelectedColors.Invalidate();
		}

		private void ColorButton_DoubleClick(object sender, System.EventArgs e)
		{
			if (!(sender is Label)) return;
			Label btn = sender as Label;
			int colorID = Int32.Parse(btn.Tag.ToString());

			ColorDialog dlg = new ColorDialog();
			dlg.Color = this._Colors[colorID];
			dlg.FullOpen = false;
			dlg.SolidColorOnly = true;
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				this._Colors[colorID] = dlg.Color;
				this.Refresh();
			}
		}

		private void ColorButton_Paint(object sender, PaintEventArgs e)
		{
			if (sender is Label)
			{
				Label btn = sender as Label;
				int colorID = Int32.Parse(btn.Tag.ToString());
				DrawColor(e.Graphics, btn.DisplayRectangle, this._Colors[colorID]);
			}			
		}

		private void SelectColorButton_Paint(object sender, PaintEventArgs e)
		{
			DrawSelectedColor(e.Graphics, btnSelectedColors.DisplayRectangle, this.PenColor, this.BrushColor);
		}

		private void InitializeColorButtons()
		{
			foreach(Control ctrl in this.Controls)
			{
				if (ctrl is Label)
				{
					Label button = ctrl as Label;
					if (button == this.btnSelectedColors)
					{
						button.Paint += new PaintEventHandler(SelectColorButton_Paint);
					}
					else
					{
						button.Paint += new PaintEventHandler(ColorButton_Paint);
						button.MouseDown += new MouseEventHandler(ColorButton_MouseDown);
						button.DoubleClick += new EventHandler(ColorButton_DoubleClick);						
					}
				}
			}
		}

		#endregion

		#region Properties
		public Color PenColor
		{
			get { return this._Colors[_selPenColor];}
			set 
			{
				this._Colors[0] = value;
				this._selPenColor = 0;
			}
		}

		public Color BrushColor
		{
			get {return this._Colors[_selBrushColor];}
			set
			{
				this._Colors[TotalColors/2] = value;
				this._selBrushColor = TotalColors/2;
			}
		}


		protected int SelectPenColorID
		{
			get {return this._selPenColor;}
			set
			{
				this._selPenColor = value;
				OnSelectedPenColorChanged();
			}
		}

		protected int SelectedBrushColorID
		{
			get {return this._selBrushColor;}
			set
			{
				this._selBrushColor = value;
				OnSelectedBrushColorChanged();
			}
		}
		#endregion

		#region Others
		private void OnSelectedBrushColorChanged()
		{
			if (this.BrushColorChanged != null)
				this.BrushColorChanged(this, new System.EventArgs());
		}

		private void OnSelectedPenColorChanged()
		{
			if (this.PenColorChanged != null)
				this.PenColorChanged(this, new System.EventArgs());
		}
		#endregion
	}
}
