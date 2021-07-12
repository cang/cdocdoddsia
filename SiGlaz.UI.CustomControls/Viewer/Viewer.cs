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
	/// Summary description for DataViewer.
	/// </summary>
	public class Viewer : System.Windows.Forms.UserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private SiGlaz.UI.CustomControls.Canvas canvas = null;

		public Viewer()
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.canvas = new SiGlaz.UI.CustomControls.Canvas();
			this.SuspendLayout();
			// 
			// canvas
			// 
			this.canvas.BackColor = System.Drawing.Color.White;
			this.canvas.Location = new System.Drawing.Point(0, 0);
			this.canvas.Name = "canvas";
			this.canvas.Size = new System.Drawing.Size(300, 500);
			this.canvas.TabIndex = 0;
			this.canvas.Text = "canvas1";
			// 
			// Viewer
			// 
			this.AutoScroll = true;
			this.BackColor = System.Drawing.Color.DarkKhaki;
			this.Controls.Add(this.canvas);
			this.Name = "Viewer";
			this.Size = new System.Drawing.Size(383, 400);
			this.ResumeLayout(false);

		}
		#endregion

		#region Member fields
		#endregion Member fields

		#region Properties
		#endregion Properties

		#region Overrides
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			CorrectCanvasLocation();
		}	
	
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged (e);

			CorrectCanvasLocation();
		}
		#endregion Overrides

		#region Events
		#endregion Events

		#region Methods
		private void CorrectCanvasLocation()
		{
			if (this.canvas.Width >= this.Width)
				this.canvas.Left = 0;
			else
				this.canvas.Left = (this.Width - this.canvas.Width)/2;

			if (this.canvas.Height >= this.Height)
				this.canvas.Top = 0;
			else
				this.canvas.Top = (this.Height - this.canvas.Height)/2;
		}
		#endregion Methods

		#region Helpers
		#endregion Helpers
	}
}
