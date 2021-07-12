using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using SIA.Common;
using SIA.Common.Analysis;
using SIA.Common.KlarfExport;
using SIA.Common.KlarfExport.BinningLibrary;
using SIA.Common.Mathematics;
using SIA.Common.Utility;
using SIA.SystemLayer;
using SIA.UI.Components;
using SIA.UI.Components.Renders;
using SIA.UI.Controls;
using SIA.UI.Controls.UserControls;
using SIA.Common.IPLFacade;

namespace SIA.UI.Controls.Dialogs
{
	/// <summary>
	/// Name : DlgScreenStretchEx
	/// Description : User interface for Screen Stretch Operation
	/// Thread Support : False
	/// Persistence Data : True
	/// </summary>
	public class DlgScreenStretchEx : FloatingFormBase
	{
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

		private ImageWorkspace _workspace = null;
		private int _updateCounter = 0;
		private bool _isDataChanged = false;

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgScreenStretchEx));
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
			this.nudMinimum.ValueChanged += new System.EventHandler(this.nudMinimum_ValueChanged);
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
			this.nudMaximum.ValueChanged += new System.EventHandler(this.nudMaximum_ValueChanged);
			// 
			// graphHistogram
			// 
			this.graphHistogram.Image = null;
			this.graphHistogram.Location = new System.Drawing.Point(4, 4);
			this.graphHistogram.Lock = false;
			this.graphHistogram.Maximum = 0;
			this.graphHistogram.Minimum = 0;
			this.graphHistogram.Name = "graphHistogram";
			this.graphHistogram.SelectRangeMax = 50;
			this.graphHistogram.SelectRangeMin = 50;
			this.graphHistogram.Size = new System.Drawing.Size(240, 132);
			this.graphHistogram.TabIndex = 3;
			this.graphHistogram.LeftValueChanged += new SIA.UI.Controls.UserControls.kGraphHistogram.ValueChangedEventHandler(this.graphHistogram_LeftValueChanged);
			this.graphHistogram.RightValueChanged += new SIA.UI.Controls.UserControls.kGraphHistogram.ValueChangedEventHandler(this.graphHistogram_RightValueChanged);
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
			// DlgScreenStretchEx
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
			this.Name = "DlgScreenStretchEx";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Screen Stretch";
			this.TopMost = true;
			((System.ComponentModel.ISupportInitialize)(this.nudMinimum)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMaximum)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion
	
		#region constructor and destructor
		
		protected DlgScreenStretchEx()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		public DlgScreenStretchEx(ImageWorkspace workspace)
		{
			if (workspace == null)
				throw new ArgumentNullException("workspace", "Workspace was not specified");
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// initialize member fields
			this._workspace = workspace;

			// register for data changed event
			this._workspace.DataChanged += new EventHandler(Workspace_DataChanged);			
			
			// force update data for first load
			this._isDataChanged = true;

			// update data
			this.UpdateData(false);
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

			if (this._workspace != null)
			{
				// unregister for data changed event
				this._workspace.DataChanged -= new EventHandler(Workspace_DataChanged);			
				
				this._workspace = null;
			}

			base.Dispose( disposing );
		}
		
		#endregion

		#region override routines

		protected override void OnLoad(EventArgs e)
		{
			try
			{
				base.OnLoad (e);
			
				// restore last location
                if ((Form.ModifierKeys & Keys.Control) != Keys.Control)
                {
                    Point loc = Point.Empty;
                    loc.X = (int)CustomConfiguration.GetValues("ScreenStretch_XPos", this.Location.X);
                    loc.Y = (int)CustomConfiguration.GetValues("ScreenStretch_YPos", this.Location.Y);
                    this.Location = loc;
                }

				// update window form style
				this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			}
			catch (Exception exp)
			{
				Trace.WriteLine(exp);
			}
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing (e);

			this.Visible = !this.Visible;
			e.Cancel = true;
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed (e);
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged (e);

			try
			{
				if (this.Visible == false)
				{
					CustomConfiguration.SetValues("ScreenStretch_XPos", this.Location.X);
					CustomConfiguration.SetValues("ScreenStretch_YPos", this.Location.Y);
				}
				else
				{
					if (this._isDataChanged)
						this.UpdateData(false);
				}
			}
			catch (Exception exp)
			{
				Trace.WriteLine(exp);
			}
		}


		#endregion

		#region Event Handlers

		private void nudMinimum_ValueChanged(object sender, System.EventArgs e)
		{
			if (this.BeginUpdate())
			{
				graphHistogram.SelectRangeMin = Convert.ToInt32(nudMinimum.Value);				
				this.EndUpdate();
			}

			this.UpdateData(true);
		}

		private void nudMaximum_ValueChanged(object sender, System.EventArgs e)
		{
			if (this.BeginUpdate())
			{
				graphHistogram.SelectRangeMax = Convert.ToInt32(nudMaximum.Value);				
				this.EndUpdate();
			}

			this.UpdateData(true);
		}

		private void graphHistogram_LeftValueChanged(object sender, int val)
		{
			if (this.BeginUpdate())
			{
				try
				{
					CommonImage image = _workspace.Image;
					int minView = graphHistogram.SelectRangeMin;
					int maxView = graphHistogram.SelectRangeMax;
				
					nudMinimum.BeginInit();
					nudMinimum.Minimum = (Decimal)image.MinGreyValue;
					nudMinimum.Maximum = maxView;
					nudMinimum.Value = val;
					nudMinimum.EndInit();

					nudMaximum.BeginInit();
					nudMaximum.Minimum = minView;
					nudMinimum.EndInit();
				}
				catch (Exception exp)
				{
					Trace.WriteLine(exp);
				}
				finally
				{
					this.EndUpdate();
				}
			}

			this.UpdateData(true);
		}

		private void graphHistogram_RightValueChanged(object sender, int val)
		{
			if (this.BeginUpdate())
			{
				try
				{
					CommonImage image = _workspace.Image;
					int minView = graphHistogram.SelectRangeMin;
					int maxView = graphHistogram.SelectRangeMax;
				
					nudMaximum.BeginInit();
					nudMaximum.Minimum = minView;
					nudMaximum.Maximum = (Decimal)image.MaxGreyValue;
					nudMaximum.Value = val;
					nudMaximum.EndInit();

					nudMinimum.BeginInit();
					nudMinimum.Maximum = val;
					nudMinimum.EndInit();
				}
				catch (Exception exp)
				{
					Trace.WriteLine(exp);
				}
				finally
				{
					this.EndUpdate();
				}
			}

			this.UpdateData(true);
		}


		private void Workspace_DataChanged(Object sender, EventArgs e)
		{
			// set data changed flags
			this._isDataChanged = true;

			// ignore update data when the dialog is invisible
			if (this.Visible == true)
				this.UpdateData(false);
		}

		private void chkAutoFit_CheckedChanged(object sender, System.EventArgs e)
		{
			// fit view to data range
			if (chkAutoFit.Checked)
			{
				// reset view range
				IRasterImageRender render = _workspace.ImageViewer.RasterImageRender;
				render.ViewRange = DataRange.Empty;				
			}

			// update image render
			this.UpdateData(true);

			// update UI
			this.UpdateData(false);
		}

		#endregion

		#region Internal Helpers

		public bool BeginUpdate()
		{
			if (_updateCounter > 0)
				return false;
			return ++_updateCounter == 1;
		}

		public void EndUpdate()
		{
			_updateCounter--;
		}

		public void UpdateData(bool bSaveAndValidate)
		{
			if (this.BeginUpdate() == true)
			{
				try
				{
					if (bSaveAndValidate)
					{
						IRasterImageRender render = _workspace.ImageViewer.RasterImageRender;
						render.AutoFitGrayScale = this.chkAutoFit.Checked;
						if (this.chkAutoFit.Checked == false)
						{
							int minView = Convert.ToInt32(nudMinimum.Value);
							int maxView = Convert.ToInt32(nudMaximum.Value);
							render.ViewRange = new DataRange(minView, maxView);
							render.IsDirty = true;
						}

						_workspace.ImageViewer.Invalidate(true);
					}
					else
					{
						// ignore data change
						graphHistogram.BeginUpdate();

						IRasterImageRender render = _workspace.ImageViewer.RasterImageRender;
						bool enabled = render != null && render.AutoFitGrayScale == false;
						
						// update common image
						if (_isDataChanged)
						{
							graphHistogram.Image = _workspace.Image;
							// reset data changed flag
							_isDataChanged = false;
						}
												
						if (_workspace.Image != null)
						{
							CommonImage image = _workspace.Image;
							int minView = 0, maxView = 0;
							int minimum = (int)Math.Max(0, image.MinGreyValue);
							int maximum = (int)Math.Min(int.MaxValue, image.MaxGreyValue);
			
							// update view range
							if (render.AutoFitGrayScale)
							{
								// retrieve data view range from graph histogram
								minView = (int)graphHistogram.DataRange.Minimum;
								maxView = (int)graphHistogram.DataRange.Maximum;
							}
							else 
							{
								// retrieve data view range from render
								// if view range is not set then initialize a default range
								if (render.ViewRange.Equals(DataRange.Empty))
									render.ViewRange = new DataRange((int)_workspace.Image.MinCurrentView, (int)_workspace.Image.MaxCurrentView);
								minView = (int)render.ViewRange.Minimum;
								maxView = (int)render.ViewRange.Maximum;
							}

							// initialize graph histogram view range
							graphHistogram.SelectRangeMin = minView;
							graphHistogram.SelectRangeMax = maxView;

							nudMinimum.BeginInit();
							nudMinimum.Minimum = Convert.ToDecimal(_workspace.Image.MinGreyValue);
							nudMinimum.Maximum = maxView;
							nudMinimum.Value = minView;
							nudMinimum.EndInit();
						
							nudMaximum.BeginInit();
							nudMaximum.Minimum = minView;
							nudMaximum.Maximum = Convert.ToDecimal(_workspace.Image.MaxGreyValue);
							nudMaximum.Value = maxView;
							nudMaximum.EndInit();

							if (chkAutoFit.IsDisposed == false)
								chkAutoFit.Checked = render.AutoFitGrayScale;
						}

						// enable controls
						graphHistogram.Enabled = nudMaximum.Enabled = nudMinimum.Enabled = enabled;

						// force the graph histogram to update immediatly
						graphHistogram.EndUpdate();
					}
				}
				catch (System.Exception ex)
				{
					Trace.WriteLine(ex);
				}
				finally
				{
					this.EndUpdate();
				}
			}
		}

		#endregion		
	}
}
