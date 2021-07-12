using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using SIA.UI.Controls.Common;
using SIA.UI.Controls.Utilities;

using SIA.UI.Components;
using SIA.UI.Components.Common;

namespace SIA.UI.Controls.Dialogs
{
	/// <summary>
	/// Name : DlgCustomPseudoColor
	/// Description : User interface for PseudoColor
	/// Thread Support : None
	/// Persistence Data : False;
	/// </summary>
	public class DlgPseudoColor : DialogBase
	{
		#region Windows Forms members

		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.GroupBox grpSettings;
		private System.Windows.Forms.Label lblMappingName;
		private System.Windows.Forms.Button btnLoadMapping;
		private System.Windows.Forms.Button btnSaveMapping;
		private System.Windows.Forms.GroupBox grpStops;
		private System.Windows.Forms.Label lblStopColor;
		private System.Windows.Forms.Label lblStopPosition;
		private GradientColorWidget _colorWidget;
		private System.Windows.Forms.Button btnRemoveStop;
		private System.Windows.Forms.Label lblPercentUnit;
		private System.Windows.Forms.ToolTip _toolTip;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.Label _lblStopColorWidget;
		private System.Windows.Forms.ComboBox _cbMappingName;
		private System.Windows.Forms.NumericUpDown _nudStopPosition;
		private System.ComponentModel.IContainer components;

		#endregion		

		#region member attribute

		private PseudoColor _pseudoColor = null;
		private SIA.UI.Components.GradientColorRectangle _srcColorWidget;
		private PseudoColor _orgPseudoColor = null;
		private bool _bModified = false;
		
		#endregion

		#region public properties

		public PseudoColor PseudoColor
		{
			get {return _pseudoColor;}
		}

		protected bool Modified
		{
			get {return _bModified;}
			set
			{
				_bModified = value;
				OnModifiedChanged();
			}
		}

		protected virtual void OnModifiedChanged()
		{
			PseudoColor builtin = PseudoColors.FromName(_cbMappingName.Text);
			if (builtin != PseudoColor.Empty)
				_cbMappingName.Text = "Custom";
		}

		#endregion

		#region constructor and destructor

		public DlgPseudoColor(PseudoColor pseudoColor)
		{
			if (pseudoColor == null)
				throw new System.ArgumentNullException("Invalid Pseudo Color parameter");
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// Initialize default Pseudo Color
			//
			_orgPseudoColor = pseudoColor;
			_pseudoColor = (PseudoColor)_orgPseudoColor.Clone();
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
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgPseudoColor));
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this._srcColorWidget = new SIA.UI.Components.GradientColorRectangle();
			this.grpSettings = new System.Windows.Forms.GroupBox();
			this.grpStops = new System.Windows.Forms.GroupBox();
			this._nudStopPosition = new System.Windows.Forms.NumericUpDown();
			this.btnRemoveStop = new System.Windows.Forms.Button();
			this._lblStopColorWidget = new System.Windows.Forms.Label();
			this.lblStopColor = new System.Windows.Forms.Label();
			this.lblStopPosition = new System.Windows.Forms.Label();
			this.lblPercentUnit = new System.Windows.Forms.Label();
			this._colorWidget = new SIA.UI.Components.GradientColorWidget();
			this.lblMappingName = new System.Windows.Forms.Label();
			this._cbMappingName = new System.Windows.Forms.ComboBox();
			this.btnLoadMapping = new System.Windows.Forms.Button();
			this.btnSaveMapping = new System.Windows.Forms.Button();
			this._toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.btnReset = new System.Windows.Forms.Button();
			this.grpSettings.SuspendLayout();
			this.grpStops.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._nudStopPosition)).BeginInit();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(416, 4);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "Ok";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(416, 32);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			// 
			// _srcColorWidget
			// 
			this._srcColorWidget.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(64)), ((System.Byte)(0)));
			this._srcColorWidget.Location = new System.Drawing.Point(16, 47);
			this._srcColorWidget.Name = "_srcColorWidget";
			this._srcColorWidget.Size = new System.Drawing.Size(372, 24);
			this._srcColorWidget.TabIndex = 2;
			// 
			// grpSettings
			// 
			this.grpSettings.Controls.Add(this.grpStops);
			this.grpSettings.Controls.Add(this._srcColorWidget);
			this.grpSettings.Controls.Add(this._colorWidget);
			this.grpSettings.Controls.Add(this.lblMappingName);
			this.grpSettings.Controls.Add(this._cbMappingName);
			this.grpSettings.Location = new System.Drawing.Point(4, 4);
			this.grpSettings.Name = "grpSettings";
			this.grpSettings.Size = new System.Drawing.Size(404, 180);
			this.grpSettings.TabIndex = 0;
			this.grpSettings.TabStop = false;
			this.grpSettings.Text = "Pseudo Color Settings";
			// 
			// grpStops
			// 
			this.grpStops.Controls.Add(this._nudStopPosition);
			this.grpStops.Controls.Add(this.btnRemoveStop);
			this.grpStops.Controls.Add(this._lblStopColorWidget);
			this.grpStops.Controls.Add(this.lblStopColor);
			this.grpStops.Controls.Add(this.lblStopPosition);
			this.grpStops.Controls.Add(this.lblPercentUnit);
			this.grpStops.Location = new System.Drawing.Point(8, 124);
			this.grpStops.Name = "grpStops";
			this.grpStops.Size = new System.Drawing.Size(388, 48);
			this.grpStops.TabIndex = 4;
			this.grpStops.TabStop = false;
			this.grpStops.Text = "Color Stop:";
			// 
			// _nudStopPosition
			// 
			this._nudStopPosition.Location = new System.Drawing.Point(189, 18);
			this._nudStopPosition.Name = "_nudStopPosition";
			this._nudStopPosition.Size = new System.Drawing.Size(63, 20);
			this._nudStopPosition.TabIndex = 6;
			this._nudStopPosition.ValueChanged += new System.EventHandler(this._nudStopPosition_ValueChanged);
			// 
			// btnRemoveStop
			// 
			this.btnRemoveStop.Location = new System.Drawing.Point(288, 16);
			this.btnRemoveStop.Name = "btnRemoveStop";
			this.btnRemoveStop.TabIndex = 5;
			this.btnRemoveStop.Text = "Remove";
			this.btnRemoveStop.Click += new System.EventHandler(this.btnRemoveStop_Click);
			// 
			// _lblStopColorWidget
			// 
			this._lblStopColorWidget.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(128)), ((System.Byte)(128)));
			this._lblStopColorWidget.Location = new System.Drawing.Point(62, 16);
			this._lblStopColorWidget.Name = "_lblStopColorWidget";
			this._lblStopColorWidget.Size = new System.Drawing.Size(60, 23);
			this._lblStopColorWidget.TabIndex = 1;
			this._lblStopColorWidget.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this._lblStopColorWidget.DoubleClick += new System.EventHandler(this._lblStopColorWidget_DoubleClick);
			// 
			// lblStopColor
			// 
			this.lblStopColor.Location = new System.Drawing.Point(22, 16);
			this.lblStopColor.Name = "lblStopColor";
			this.lblStopColor.Size = new System.Drawing.Size(39, 23);
			this.lblStopColor.TabIndex = 0;
			this.lblStopColor.Text = "Color:";
			this.lblStopColor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblStopPosition
			// 
			this.lblStopPosition.Location = new System.Drawing.Point(131, 16);
			this.lblStopPosition.Name = "lblStopPosition";
			this.lblStopPosition.Size = new System.Drawing.Size(52, 23);
			this.lblStopPosition.TabIndex = 2;
			this.lblStopPosition.Text = "Location:";
			this.lblStopPosition.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblPercentUnit
			// 
			this.lblPercentUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblPercentUnit.Location = new System.Drawing.Point(256, 16);
			this.lblPercentUnit.Name = "lblPercentUnit";
			this.lblPercentUnit.Size = new System.Drawing.Size(24, 23);
			this.lblPercentUnit.TabIndex = 4;
			this.lblPercentUnit.Text = "(%)";
			this.lblPercentUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _colorWidget
			// 
			this._colorWidget.Location = new System.Drawing.Point(5, 80);
			this._colorWidget.Name = "_colorWidget";
			this._colorWidget.SelectedColorStop = null;
			this._colorWidget.Size = new System.Drawing.Size(394, 32);
			this._colorWidget.TabIndex = 3;
			this._colorWidget.SelectedStopPositionChanged += new System.EventHandler(this.ColorWidget_SelectedStopPositionChanged);
			this._colorWidget.MouseHover += new System.EventHandler(this.ColorWidget_MouseHover);
			this._colorWidget.SelectedStopColorChanged += new System.EventHandler(this.ColorWidget_SelectedStopColorChanged);
			this._colorWidget.SelectedColorStopChanged += new System.EventHandler(this.ColorWidget_SelectedColorStopChanged);
			// 
			// lblMappingName
			// 
			this.lblMappingName.Location = new System.Drawing.Point(7, 17);
			this.lblMappingName.Name = "lblMappingName";
			this.lblMappingName.Size = new System.Drawing.Size(41, 20);
			this.lblMappingName.TabIndex = 0;
			this.lblMappingName.Text = "Name:";
			this.lblMappingName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _cbMappingName
			// 
			this._cbMappingName.Location = new System.Drawing.Point(48, 18);
			this._cbMappingName.Name = "_cbMappingName";
			this._cbMappingName.Size = new System.Drawing.Size(340, 21);
			this._cbMappingName.TabIndex = 5;
			// 
			// btnLoadMapping
			// 
			this.btnLoadMapping.Location = new System.Drawing.Point(416, 60);
			this.btnLoadMapping.Name = "btnLoadMapping";
			this.btnLoadMapping.TabIndex = 3;
			this.btnLoadMapping.Text = "Load";
			this.btnLoadMapping.Click += new System.EventHandler(this.btnLoadMapping_Click);
			// 
			// btnSaveMapping
			// 
			this.btnSaveMapping.Location = new System.Drawing.Point(416, 88);
			this.btnSaveMapping.Name = "btnSaveMapping";
			this.btnSaveMapping.TabIndex = 4;
			this.btnSaveMapping.Text = "Save";
			this.btnSaveMapping.Click += new System.EventHandler(this.btnSaveMapping_Click);
			// 
			// btnReset
			// 
			this.btnReset.Location = new System.Drawing.Point(416, 116);
			this.btnReset.Name = "btnReset";
			this.btnReset.TabIndex = 4;
			this.btnReset.Text = "Reset";
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// DlgPseudoColor
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(494, 188);
			this.Controls.Add(this.btnLoadMapping);
			this.Controls.Add(this.grpSettings);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnSaveMapping);
			this.Controls.Add(this.btnReset);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DlgPseudoColor";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Pseudo Color";
			this.grpSettings.ResumeLayout(false);
			this.grpStops.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this._nudStopPosition)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region override routines		
		
		protected override void OnLoad(EventArgs e)
		{
			this.Icon = new Icon(this.GetType(), "Icon.icon.ico");
			
			base.OnLoad (e);

			// initialize tooltip objects
			this.InitializeTooltips();

			// initialize StopColor UI Components
			_nudStopPosition.Minimum = (Decimal) 0.0F;
			_nudStopPosition.Maximum = (Decimal) 100.0F;

			// initialize Built-in Mapping Objects
			_cbMappingName.BeginUpdate();
			_cbMappingName.Items.Add(PseudoColors.GrayScale);
			_cbMappingName.Items.Add(PseudoColors.Fire);
			_cbMappingName.Items.Add(PseudoColors.Ice);
			_cbMappingName.EndUpdate();

			// initialize UI components from PseudoColor
			UpdateData(false);
			UpdateSelectedStopData(false);

			// register for selected index changed
			this._cbMappingName.SelectedIndexChanged += new System.EventHandler(this.MappingName_SelectedIndexChanged);
		}
		

		#endregion

		#region event handlers

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			// update Pseudo Color
			UpdateData(true);	
		}
		
		private void btnLoadMapping_Click(object sender, System.EventArgs e)
		{
			using (OpenFileDialog dlg = CommonDialogs.OpenPseudoColorFileDialog("Select Pseudo Color File..."))
			{
				if (dlg.ShowDialog(this) == DialogResult.OK)
				{
					try
					{
						PseudoColor loaded = new PseudoColor();
						loaded.Load(dlg.FileName);
						_orgPseudoColor = loaded;
						_pseudoColor = loaded;
						UpdateData(false);
					}
					catch(System.Exception exp)
					{
						Trace.WriteLine(exp);
						// notify user 
						MessageBoxEx.Error("Failed to load Pseudo Color file");
					}
					finally
					{
					}
				}
			}
		}

		private void btnSaveMapping_Click(object sender, System.EventArgs e)
		{
			using (SaveFileDialog dlg = CommonDialogs.SavePseudoColorFileDialog("Save Pseudo Color ..."))
			{
				if (dlg.ShowDialog(this) == DialogResult.OK)
				{
					try
					{
						// update data from controls
						UpdateData(true);
						_orgPseudoColor = _pseudoColor;
						_pseudoColor.Save(dlg.FileName);
					}
					catch(System.Exception exp)
					{
						Trace.WriteLine(exp);
						// notify user 
						MessageBoxEx.Error("Failed to save Pseudo Color file");
					}
					finally
					{
					}
				}
			}
		}

		private void btnReset_Click(object sender, System.EventArgs e)
		{
			if (_orgPseudoColor != null)
			{
				// set no select any stop for preventing update
				_colorWidget.SelectedColorStop = null;
				_pseudoColor = _orgPseudoColor;
				// initialize UI components from PseudoColor
				UpdateData(false);
				UpdateSelectedStopData(false);
			}
		}

		private void MappingName_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			// refresh Pseudo Color data
			if (_cbMappingName.SelectedItem != null)
			{
				PseudoColor builtin =  (PseudoColor) _cbMappingName.SelectedItem;
				_orgPseudoColor = (PseudoColor)builtin.Clone();
				_pseudoColor = _orgPseudoColor;
				// reset selected color stop
				_colorWidget.SelectedColorStop = null;
				// initialize UI components from PseudoColor
				UpdateData(false);
				UpdateSelectedStopData(false);
			}
		}
		
		#region Color Stop Adjustment
		
		private void _lblStopColorWidget_DoubleClick(object sender, System.EventArgs e)
		{
			if (this._colorWidget.SelectedColorStop != null)
			{
				ColorStop stop = _colorWidget.SelectedColorStop;
				using (ColorDialog dlg = new ColorDialog())
				{
					dlg.Color = stop.Color;
					if (DialogResult.OK == dlg.ShowDialog(this))
					{
						Modified = true;
						_lblStopColorWidget.BackColor = dlg.Color;
						UpdateSelectedStopData(true);
					}
				}
			}
		}

		private void _nudStopPosition_ValueChanged(object sender, System.EventArgs e)
		{
			ColorStop stop = _colorWidget.SelectedColorStop;
			if (stop != null)
			{
				Modified = true;
				UpdateSelectedStopData(true);
			}
		}

		private void btnRemoveStop_Click(object sender, System.EventArgs e)
		{
			ColorStop stop = _colorWidget.SelectedColorStop;
			if (stop != null)
			{
				Modified = true;
				_colorWidget.RemoveStop(stop);
				_colorWidget.Invalidate(true);
			}
		}		
		
		#endregion		

		#region Gradient Color Widget event handler
		
		private void ColorWidget_SelectedColorStopChanged(object sender, System.EventArgs e)
		{
			Modified = true;
			UpdateSelectedStopData(false);
		}

		private void ColorWidget_SelectedStopColorChanged(object sender, System.EventArgs e)
		{
			Modified = true;
			UpdateSelectedStopData(false);
		}

		private void ColorWidget_SelectedStopPositionChanged(object sender, System.EventArgs e)
		{
			Modified = true;
			UpdateSelectedStopData(false);
		}

		#endregion

		#endregion

		#region internal helpers
		
		private void UpdateData(bool bSaveAndValidate)
		{
			if (bSaveAndValidate)
			{
				_pseudoColor.Name = _cbMappingName.Text;
				ColorBlend blend = _colorWidget.ColorBlend;
				_pseudoColor.Colors = blend.Colors;
				_pseudoColor.Positions = blend.Positions;		
			}
			else
			{
				try
				{
					this._cbMappingName.Text = _pseudoColor.Name;
					this._colorWidget.BeginUpdate();
					this._colorWidget.ClearStops();
					int num_stop = _pseudoColor.Colors.Length;
					for (int i=0; i<num_stop; i++)
						_colorWidget.AddStop(_pseudoColor.Colors[i], _pseudoColor.Positions[i]);				
				}
				catch(System.Exception exp)
				{
					Trace.WriteLine(exp);
				}
				finally
				{
					this._colorWidget.EndUpdate();
				}			
			}
		}

		private void UpdateSelectedStopData(bool bSaveAndValidate)
		{
			ColorStop stop = _colorWidget.SelectedColorStop;
			if (stop == null)
			{
				_lblStopColorWidget.BackColor = SystemColors.Control;
				_lblStopColorWidget.Enabled = false;
				_nudStopPosition.Enabled = false;
				btnRemoveStop.Enabled = false;
			}
			else
			{
				_lblStopColorWidget.Enabled = true;
				_nudStopPosition.Enabled = true;
				btnRemoveStop.Enabled = _colorWidget.CanRemoveStops();

				if (bSaveAndValidate)
				{
					_colorWidget.BeginUpdate();
					stop.Color = _lblStopColorWidget.BackColor;
					stop.Position = (float)_nudStopPosition.Value / 100.0F;
					_colorWidget.EndUpdate();
				}
				else
				{
					_lblStopColorWidget.BackColor = stop.Color;
					_nudStopPosition.Value = (Decimal)(Math.Round(stop.Position * 100.0F));					
				}
			}
		}

		private void InitializeTooltips()
		{
			_toolTip.SetToolTip(_lblStopColorWidget, "Double click to change color of selected stop");
			_toolTip.SetToolTip(_nudStopPosition, "Change location of selected stop");
			_toolTip.SetToolTip(btnRemoveStop, "Remove the selected color stop");
		}

		private void ColorWidget_MouseHover(object sender, System.EventArgs e)
		{
			if (sender == _colorWidget)
				_toolTip.SetToolTip(_colorWidget, _colorWidget.GetTooltipText());
		}

		#endregion		
		
	}
}
