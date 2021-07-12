using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;

using NPlot;

namespace SIA.UI.Controls.UserControls
{
	/// <summary>
	/// Summary description for RadialLineProfile.
	/// </summary>
	public class RadialLineProfile : System.Windows.Forms.UserControl
	{
		private PlotSurface2DEx _plotSurface;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public RadialLineProfile()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
			this.UpdateProfile(null);
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._plotSurface = new SIA.UI.Controls.UserControls.PlotSurface2DEx();
			this.SuspendLayout();
			// 
			// _plotSurface
			// 
			this._plotSurface.AutoScaleAutoGeneratedAxes = false;
			this._plotSurface.AutoScaleTitle = false;
			this._plotSurface.BackColor = System.Drawing.SystemColors.ActiveBorder;
			this._plotSurface.DateTimeToolTip = false;
			this._plotSurface.Dock = System.Windows.Forms.DockStyle.Fill;
			this._plotSurface.Legend = null;
			this._plotSurface.LegendZOrder = -1;
			this._plotSurface.Location = new System.Drawing.Point(0, 0);
			this._plotSurface.Name = "_plotSurface";
			this._plotSurface.Padding = 10;
			this._plotSurface.RightMenu = null;
			this._plotSurface.ShowCoordinates = true;
			this._plotSurface.Size = new System.Drawing.Size(472, 208);
			this._plotSurface.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
			this._plotSurface.TabIndex = 17;
			this._plotSurface.Title = "Chart Viewer";
			this._plotSurface.TitleFont = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this._plotSurface.XAxis1 = null;
			this._plotSurface.XAxis2 = null;
			this._plotSurface.YAxis1 = null;
			this._plotSurface.YAxis2 = null;
			this._plotSurface.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this._plotSurface_KeyPress);
			this._plotSurface.KeyUp += new System.Windows.Forms.KeyEventHandler(this._plotSurface_KeyUp);
			this._plotSurface.KeyDown += new System.Windows.Forms.KeyEventHandler(this._plotSurface_KeyDown);
			// 
			// RadialLineProfile
			// 
			this.Controls.Add(this._plotSurface);
			this.Name = "RadialLineProfile";
			this.Size = new System.Drawing.Size(472, 208);
			this.ResumeLayout(false);

		}
		#endregion

		public void UpdateProfile(float[] data)
		{
			this.UpdateProfile(data, -1, -1);
		}

		public void UpdateProfile(float[] data, int selIndex, int wbIndex)
		{
			NPlot.Windows.PlotSurface2D plotSurface = this._plotSurface;

			// clear plot surface
			plotSurface.Clear();

			// Create a new line plot from array data via the ArrayAdapter class.
			LinePlot linePlot = new LinePlot();
			linePlot.DataSource = data;
			linePlot.Color = Color.DarkGray;
			linePlot.Label = "Intensity Values";
			if (linePlot.Pen != null)
				linePlot.Pen.Width = 2.0F;
			linePlot.ShowInLegend = true;

			Grid myGrid = new Grid();
			myGrid.VerticalGridType = Grid.GridType.Fine;
			myGrid.HorizontalGridType = Grid.GridType.Fine;
			plotSurface.Add(myGrid);

			// And add it to the plot surface
			plotSurface.Add( linePlot );
			plotSurface.Title = "Radial line profile";

			// Add vertical line for selected value if any
			if (selIndex >= 0 && selIndex < data.Length)
			{
				Pen pen = new Pen(Color.FromArgb(0x80, Color.Green), 2.0F);
				VerticalLine vLine = new VerticalLine(selIndex, pen);
				vLine.Label = "Edge Point";
				vLine.ShowInLegend = true;
				plotSurface.Add(vLine);
			}

			// Add vertical line for wafer bound if any
			if (wbIndex >= 0 && wbIndex < data.Length)
			{
				Pen pen = new Pen(Color.FromArgb(0x80, Color.Red), 2.0F);
				VerticalLine vLine = new VerticalLine(wbIndex, pen);
				vLine.Label = "Wafer Boundary Point";
				vLine.ShowInLegend = true;
				plotSurface.Add(vLine);
			}

			LinearAxis myAxis = new LinearAxis( plotSurface.YAxis1 );
			myAxis.NumberOfSmallTicks = 2;
			plotSurface.YAxis1 = myAxis;

			// We would also like to modify the way in which the X Axis is printed. This time,
			// we'll just modify the relevant PlotSurface2D Axis directly. 
			//plotSurface.XAxis1.WorldMax = 100.0f;
			plotSurface.PlotBackColor = SystemColors.ControlLight;

			// add chart legend 
			Legend legend = new Legend();
			legend.NumberItemsVertically = 1;
			legend.AttachTo(NPlot.PlotSurface2D.XAxisPosition.Bottom, NPlot.PlotSurface2D.YAxisPosition.Left);
			legend.HorizontalEdgePlacement = NPlot.Legend.Placement.Outside;
			legend.VerticalEdgePlacement = NPlot.Legend.Placement.Inside;
			legend.XOffset = 0;
			legend.YOffset = 40;
			legend.BorderStyle = NPlot.LegendBase.BorderType.Line;
			
			plotSurface.Legend = legend;

			// initialize axises title
			plotSurface.XAxis1.Label = "Pixel Index";
			plotSurface.YAxis1.Label = "Intensity Values";

			// Force a re-draw of the control. 
			plotSurface.Refresh();
		}

		private void _plotSurface_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			this.OnKeyDown(e);
		}

		private void _plotSurface_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			this.OnKeyPress(e);
		}

		private void _plotSurface_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			this.OnKeyUp(e);
		}
	}
}
