using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using SIA.Common;
using SIA.Common.Analysis;
using SIA.Common.KlarfExport;
using SIA.Common.KlarfExport.BinningLibrary;
using SIA.Common.Mathematics;
using SIA.Common.Utility;

namespace SIA.UI.Controls.Dialogs
{
	/// <summary>
	/// Name : DlgScreenStretch
	/// Description : User interface for Screen Stretch Operation
	/// Thread Support : False
	/// Persistence Data : True
	/// </summary>
	public class DlgScreenStretch : FloatingFormBase
	{
		/// <summary>
		/// Value changed event handler
		/// </summary>
		public delegate void ValueChangedEventHandler(Object sender, int val);

		#region Windows Form member attributes
		private System.Windows.Forms.Label lblMin;
		private System.Windows.Forms.Label lblMax;
		private SIA.UI.Components.IntensityUpDown nudMinimum;
		private SIA.UI.Components.IntensityUpDown nudMaximum;
		private SIA.UI.Controls.UserControls.kGraphHistogram graphHistogram;
		private System.Windows.Forms.CheckBox chkAutoFit;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion

		#region member attributes

		private SIA.SystemLayer.CommonImage _image = null;
		private bool _bDataChanged = true;
		private bool _bUserChanged = true;

		public event ValueChangedEventHandler MinValueChanged = null;
		public event ValueChangedEventHandler MaxValueChanged = null;
		public event EventHandler AutoFitGrayScaleChanged = null;
		
		#endregion

		#region constructor and destructor
		
		public DlgScreenStretch() : base()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
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
		
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgScreenStretch));
			this.lblMin = new System.Windows.Forms.Label();
			this.nudMinimum = new SIA.UI.Components.IntensityUpDown();
			this.lblMax = new System.Windows.Forms.Label();
			this.nudMaximum = new SIA.UI.Components.IntensityUpDown();
			this.graphHistogram = new SIA.UI.Controls.UserControls.kGraphHistogram();
			this.chkAutoFit = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.nudMinimum)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMaximum)).BeginInit();
			this.SuspendLayout();
			// 
			// lblMin
			// 
			this.lblMin.Location = new System.Drawing.Point(6, 140);
			this.lblMin.Name = "lblMin";
			this.lblMin.Size = new System.Drawing.Size(56, 20);
			this.lblMin.TabIndex = 1;
			this.lblMin.Text = "Minimum:";
			this.lblMin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// nudMinimum
			// 
			this.nudMinimum.Location = new System.Drawing.Point(62, 140);
			this.nudMinimum.Name = "nudMinimum";
			this.nudMinimum.Size = new System.Drawing.Size(57, 20);
			this.nudMinimum.TabIndex = 2;
			this.nudMinimum.ValueChanged += new System.EventHandler(this.OnNUD_Changed);
			// 
			// lblMax
			// 
			this.lblMax.Location = new System.Drawing.Point(123, 140);
			this.lblMax.Name = "lblMax";
			this.lblMax.Size = new System.Drawing.Size(56, 20);
			this.lblMax.TabIndex = 1;
			this.lblMax.Text = "Maximum:";
			this.lblMax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// nudMaximum
			// 
			this.nudMaximum.Location = new System.Drawing.Point(183, 140);
			this.nudMaximum.Maximum = new System.Decimal(new int[] {
																	   65535,
																	   0,
																	   0,
																	   0});
			this.nudMaximum.Name = "nudMaximum";
			this.nudMaximum.Size = new System.Drawing.Size(57, 20);
			this.nudMaximum.TabIndex = 2;
			this.nudMaximum.Value = new System.Decimal(new int[] {
																	 65534,
																	 0,
																	 0,
																	 0});
			this.nudMaximum.ValueChanged += new System.EventHandler(this.OnNUD_Changed);
			// 
			// graphHistogram
			// 
			this.graphHistogram.Image = null;
			this.graphHistogram.Location = new System.Drawing.Point(3, 4);
			this.graphHistogram.Lock = false;
			this.graphHistogram.Maximum = 0;
			this.graphHistogram.Minimum = 0;
			this.graphHistogram.Name = "graphHistogram";
			this.graphHistogram.SelectRangeMax = 50;
			this.graphHistogram.SelectRangeMin = 50;
			this.graphHistogram.Size = new System.Drawing.Size(240, 132);
			this.graphHistogram.TabIndex = 3;
			this.graphHistogram.LeftValueChanged += new SIA.UI.Controls.UserControls.kGraphHistogram.ValueChangedEventHandler(this.OnLeftValueChanged);
			this.graphHistogram.RightValueChanged += new SIA.UI.Controls.UserControls.kGraphHistogram.ValueChangedEventHandler(this.OnRightValueChanged);
			this.graphHistogram.LeftValueChanging += new SIA.UI.Controls.UserControls.kGraphHistogram.ValueChangingEventHandler(this.OnLeftValueChanging);
			this.graphHistogram.RightValueChanging += new SIA.UI.Controls.UserControls.kGraphHistogram.ValueChangingEventHandler(this.OnRightValueChanging);
			// 
			// chkAutoFit
			// 
			this.chkAutoFit.Location = new System.Drawing.Point(8, 164);
			this.chkAutoFit.Name = "chkAutoFit";
			this.chkAutoFit.Size = new System.Drawing.Size(228, 20);
			this.chkAutoFit.TabIndex = 4;
			this.chkAutoFit.Text = "Auto fit min and max";
			this.chkAutoFit.CheckedChanged += new System.EventHandler(this.chkAutoFit_CheckedChanged);
			// 
			// DlgScreenStretch
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(246, 188);
			this.Controls.Add(this.chkAutoFit);
			this.Controls.Add(this.graphHistogram);
			this.Controls.Add(this.nudMinimum);
			this.Controls.Add(this.lblMin);
			this.Controls.Add(this.lblMax);
			this.Controls.Add(this.nudMaximum);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DlgScreenStretch";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Screen Stretch";
			this.TopMost = true;
			this.Closing += new System.ComponentModel.CancelEventHandler(this.OnClosing);
			this.Load += new System.EventHandler(this.DlgScreenStretch_Load);
			this.VisibleChanged += new System.EventHandler(this.OnVisible_Changed);
			this.LocationChanged += new System.EventHandler(this.OnLocation_Changed);
			((System.ComponentModel.ISupportInitialize)(this.nudMinimum)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMaximum)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region public properties

		public SIA.SystemLayer.CommonImage CommomImage
		{
			get {return _image;}
			set 
			{
				_image = value;
				if (Visible)
					Reset();
				else
					_bDataChanged = true;
			}
		}

		public float MinView
		{
			get {return (float)nudMinimum.Value;}
			set {nudMinimum.Value = (Decimal)value;}
		}

		public float MaxView
		{
			get {return (float)nudMaximum.Value;}
			set {nudMaximum.Value = (Decimal)value;}
		}

		public bool Lock
		{
			get {return graphHistogram.Lock;}
			set {graphHistogram.Lock = value;}
		}

		public bool AutoFitGrayScale
		{
			get {return chkAutoFit.Checked;}			
			set {chkAutoFit.Checked = value;}
		}

		#endregion

		#region event handlers

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			Point loc = Point.Empty;
			loc.X = (int)CustomConfiguration.GetValues("ScreenStretch_XPos", this.Location.X);
			loc.Y = (int)CustomConfiguration.GetValues("ScreenStretch_YPos", this.Location.Y);
			this.Location = loc;
			Reset();

			// update window form style
			this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
		}

		private void DlgScreenStretch_Load(object sender, System.EventArgs e)
		{	
		}

		private void OnLeftValueChanged(object sender, int val)
		{
			_bUserChanged = false;

			nudMaximum.Minimum = val;
			if (nudMinimum.Minimum<=val && val<=nudMinimum.Maximum)
				nudMinimum.Value = (Decimal)val;

			if (MinValueChanged!=null)
				MinValueChanged(this, val);

			_bUserChanged = true;
		}

		private void OnLeftValueChanging(object sender, int val)
		{
			_bUserChanged = false;

			/* update left side range */
			nudMinimum.Minimum = graphHistogram.Minimum;
			nudMinimum.Maximum = graphHistogram.SelectRangeMax;
			nudMinimum.Value   = (Decimal) Math.Min(val, nudMinimum.Maximum);
			
			_bUserChanged = true;
		}

		private void OnRightValueChanged(object sender, int val)
		{
			_bUserChanged = false;

			nudMinimum.Maximum = (Decimal)val;
			if (nudMaximum.Minimum<=val && val<=nudMaximum.Maximum)
				nudMaximum.Value = (Decimal)val;
			if (MaxValueChanged!=null)
				MaxValueChanged(this, val);

			_bUserChanged = true;
		}

		private void OnRightValueChanging(object sender, int val)
		{
			_bUserChanged = false;

			/* update right side range */
			nudMaximum.Maximum = graphHistogram.Maximum;
			nudMaximum.Minimum = graphHistogram.SelectRangeMin;
			nudMaximum.Value   = (Decimal) Math.Max(val, nudMaximum.Minimum);

			_bUserChanged = true;
		}

		private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Visible = !this.Visible;
			e.Cancel = true;
		}

		private void OnNUD_Changed(object sender, System.EventArgs e)
		{
			/* processes user-event only */
			if (_bUserChanged)
			{
				if (sender == nudMinimum)
				{
					nudMaximum.Minimum = nudMinimum.Value;
					this.graphHistogram.SelectRangeMin = (int)nudMinimum.Value;
					if (MinValueChanged!=null)
						MinValueChanged(this, (int)nudMinimum.Value);
				}
				else if (sender == nudMaximum)
				{
					nudMinimum.Maximum = nudMaximum.Value;
					this.graphHistogram.SelectRangeMax = (int)nudMaximum.Value;
					if (MaxValueChanged!=null)
						MaxValueChanged(this, (int)nudMaximum.Value);
				}
			}
		}

		private void OnVisible_Changed(object sender, System.EventArgs e)
		{
			if (Visible && _bDataChanged) 
			{
				this.Reset();

				CustomConfiguration.SetValues("ScreenStretch_XPos", this.Location.X);
				CustomConfiguration.SetValues("ScreenStretch_YPos", this.Location.Y);
			}
		}

		private void OnLocation_Changed(object sender, System.EventArgs e)
		{
		}

		private void chkAutoFit_CheckedChanged(object sender, System.EventArgs e)
		{	
			nudMaximum.Enabled = !chkAutoFit.Checked;
			nudMinimum.Enabled = !chkAutoFit.Checked;
			graphHistogram.Enabled = !chkAutoFit.Checked;

			if (AutoFitGrayScaleChanged != null)
				this.AutoFitGrayScaleChanged(this, EventArgs.Empty);
		}

		#endregion

		#region internal routines
		public void Reset()
		{
			bool bAutoFit = chkAutoFit.Checked;
			_bUserChanged = false;

			if (graphHistogram!=null)
			{
				graphHistogram.Enabled = (_image!=null) && !bAutoFit;
				if (_image!=null)
				{
					graphHistogram.Image = _image;
					graphHistogram.SelectRangeMax = (int)_image.MaxCurrentView;
					graphHistogram.SelectRangeMin = (int)_image.MinCurrentView;
					_bDataChanged = false;
				}
			}
			
			if (nudMaximum!=null) 
			{
				nudMaximum.Enabled = (_image!=null) && !bAutoFit;
				if (_image!=null)
				{
					nudMaximum.Maximum	= (Decimal) graphHistogram.Maximum;
					nudMaximum.Minimum	= (Decimal) graphHistogram.SelectRangeMin;
					nudMaximum.Value	= (Decimal) graphHistogram.SelectRangeMax;
				} 
			}
			
			if (nudMinimum!=null) 
			{
				nudMinimum.Enabled = (_image!=null) && !bAutoFit;				
				if (_image!=null)
				{
					nudMinimum.Maximum	= (Decimal) graphHistogram.SelectRangeMax;
					nudMinimum.Minimum	= (Decimal) graphHistogram.Minimum;
					nudMinimum.Value	= (Decimal) graphHistogram.SelectRangeMin;
				}
			}
		
			_bUserChanged = true;
		}

		#endregion

		
		
	}
}
