using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Diagnostics;

using SIA.UI.Components;
using SIA.UI.Components.Printing;
using SIA.UI.Components.Helpers;

namespace SIA.UI.Components.Dialogs
{
	/// <summary>
	/// Represents a dialog box form that contains a <see cref="SIA.UI.Components.PrintSettingsPreviewControl"/>.
	/// </summary>
	public class DlgPrintSettings : System.Windows.Forms.Form
	{
		#region constants
		private const string UNIT_INCHES = "inches";
		private const string UNIT_CENTIMETS = "cm";
		private const string UNIT_MILIMETS = "mm";
		#endregion

		#region Windows Form fields
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.GroupBox grpParameters;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.GroupBox grpPreview;
		private System.Windows.Forms.GroupBox grpPosition;
		private System.Windows.Forms.GroupBox grpScaled;
		private System.Windows.Forms.NumericUpDown nudTop;
		private System.Windows.Forms.NumericUpDown nudLeft;
		private System.Windows.Forms.NumericUpDown nudWidth;
		private System.Windows.Forms.NumericUpDown nudHeight;
		private System.Windows.Forms.ComboBox cbTopUnit;
		private System.Windows.Forms.ComboBox cbLeftUnit;
		private System.Windows.Forms.ComboBox cbWidthUnit;
		private System.Windows.Forms.ComboBox cbHeightUnit;
		private PrintSettingsPreviewControl _previewCtrl;
		private System.Windows.Forms.CheckBox chkCenterImage;
		private System.Windows.Forms.CheckBox chkScaleToFit;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.CheckBox chkPrintSelection;
		private System.Windows.Forms.Button btnPageSetup;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion

		#region constructor and destructors
		protected DlgPrintSettings()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		public DlgPrintSettings(PrintSettings printSettings)
		{
			if (printSettings == null)
				throw new ArgumentNullException("printSettings");
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// initialize print settings
			_orgPrintSettings = printSettings;
			this.PrintSettings = (PrintSettings)_orgPrintSettings.Clone();
	
			// initialize controls
			this.InitializeControls();
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgPrintSettings));
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.grpParameters = new System.Windows.Forms.GroupBox();
			this.grpPosition = new System.Windows.Forms.GroupBox();
			this.cbTopUnit = new System.Windows.Forms.ComboBox();
			this.nudTop = new System.Windows.Forms.NumericUpDown();
			this.chkCenterImage = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.nudLeft = new System.Windows.Forms.NumericUpDown();
			this.cbLeftUnit = new System.Windows.Forms.ComboBox();
			this.grpScaled = new System.Windows.Forms.GroupBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.nudWidth = new System.Windows.Forms.NumericUpDown();
			this.nudHeight = new System.Windows.Forms.NumericUpDown();
			this.cbWidthUnit = new System.Windows.Forms.ComboBox();
			this.cbHeightUnit = new System.Windows.Forms.ComboBox();
			this.chkScaleToFit = new System.Windows.Forms.CheckBox();
			this.chkPrintSelection = new System.Windows.Forms.CheckBox();
			this.grpPreview = new System.Windows.Forms.GroupBox();
			this._previewCtrl = new SIA.UI.Components.Printing.PrintSettingsPreviewControl();
			this.btnReset = new System.Windows.Forms.Button();
			this.btnPageSetup = new System.Windows.Forms.Button();
			this.grpParameters.SuspendLayout();
			this.grpPosition.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudTop)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudLeft)).BeginInit();
			this.grpScaled.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudWidth)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudHeight)).BeginInit();
			this.grpPreview.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(292, 36);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(292, 8);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 2;
			this.btnOK.Text = "OK";
			// 
			// grpParameters
			// 
			this.grpParameters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.grpParameters.Controls.Add(this.grpPosition);
			this.grpParameters.Controls.Add(this.grpScaled);
			this.grpParameters.Controls.Add(this.chkPrintSelection);
			this.grpParameters.Location = new System.Drawing.Point(4, 280);
			this.grpParameters.Name = "grpParameters";
			this.grpParameters.Size = new System.Drawing.Size(280, 272);
			this.grpParameters.TabIndex = 1;
			this.grpParameters.TabStop = false;
			this.grpParameters.Text = "Print Settings";
			// 
			// grpPosition
			// 
			this.grpPosition.Controls.Add(this.cbTopUnit);
			this.grpPosition.Controls.Add(this.nudTop);
			this.grpPosition.Controls.Add(this.chkCenterImage);
			this.grpPosition.Controls.Add(this.label1);
			this.grpPosition.Controls.Add(this.label2);
			this.grpPosition.Controls.Add(this.nudLeft);
			this.grpPosition.Controls.Add(this.cbLeftUnit);
			this.grpPosition.Location = new System.Drawing.Point(8, 16);
			this.grpPosition.Name = "grpPosition";
			this.grpPosition.Size = new System.Drawing.Size(264, 104);
			this.grpPosition.TabIndex = 0;
			this.grpPosition.TabStop = false;
			this.grpPosition.Text = "Position";
			// 
			// cbTopUnit
			// 
			this.cbTopUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbTopUnit.Location = new System.Drawing.Point(136, 19);
			this.cbTopUnit.Name = "cbTopUnit";
			this.cbTopUnit.Size = new System.Drawing.Size(116, 21);
			this.cbTopUnit.TabIndex = 2;
			this.cbTopUnit.SelectedIndexChanged += new System.EventHandler(this.cbUnit_SelectedIndexChanged);
			// 
			// nudTop
			// 
			this.nudTop.DecimalPlaces = 3;
			this.nudTop.Increment = new System.Decimal(new int[] {
																	 1,
																	 0,
																	 0,
																	 65536});
			this.nudTop.Location = new System.Drawing.Point(60, 20);
			this.nudTop.Maximum = new System.Decimal(new int[] {
																   10000,
																   0,
																   0,
																   0});
			this.nudTop.Name = "nudTop";
			this.nudTop.Size = new System.Drawing.Size(72, 20);
			this.nudTop.TabIndex = 1;
			this.nudTop.ThousandsSeparator = true;
			this.nudTop.ValueChanged += new System.EventHandler(this.PositionValue_Changed);
			// 
			// chkCenterImage
			// 
			this.chkCenterImage.Location = new System.Drawing.Point(12, 76);
			this.chkCenterImage.Name = "chkCenterImage";
			this.chkCenterImage.Size = new System.Drawing.Size(240, 20);
			this.chkCenterImage.TabIndex = 6;
			this.chkCenterImage.Text = "&Center Image";
			this.chkCenterImage.CheckedChanged += new System.EventHandler(this.chkCenterImage_CheckedChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(44, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "&Top:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(44, 20);
			this.label2.TabIndex = 3;
			this.label2.Text = "&Left:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// nudLeft
			// 
			this.nudLeft.DecimalPlaces = 3;
			this.nudLeft.Increment = new System.Decimal(new int[] {
																	  1,
																	  0,
																	  0,
																	  65536});
			this.nudLeft.Location = new System.Drawing.Point(60, 48);
			this.nudLeft.Maximum = new System.Decimal(new int[] {
																	10000,
																	0,
																	0,
																	0});
			this.nudLeft.Name = "nudLeft";
			this.nudLeft.Size = new System.Drawing.Size(72, 20);
			this.nudLeft.TabIndex = 4;
			this.nudLeft.ThousandsSeparator = true;
			this.nudLeft.ValueChanged += new System.EventHandler(this.PositionValue_Changed);
			// 
			// cbLeftUnit
			// 
			this.cbLeftUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbLeftUnit.Location = new System.Drawing.Point(136, 47);
			this.cbLeftUnit.Name = "cbLeftUnit";
			this.cbLeftUnit.Size = new System.Drawing.Size(116, 21);
			this.cbLeftUnit.TabIndex = 5;
			this.cbLeftUnit.SelectedIndexChanged += new System.EventHandler(this.cbUnit_SelectedIndexChanged);
			// 
			// grpScaled
			// 
			this.grpScaled.Controls.Add(this.label4);
			this.grpScaled.Controls.Add(this.label5);
			this.grpScaled.Controls.Add(this.nudWidth);
			this.grpScaled.Controls.Add(this.nudHeight);
			this.grpScaled.Controls.Add(this.cbWidthUnit);
			this.grpScaled.Controls.Add(this.cbHeightUnit);
			this.grpScaled.Controls.Add(this.chkScaleToFit);
			this.grpScaled.Location = new System.Drawing.Point(8, 124);
			this.grpScaled.Name = "grpScaled";
			this.grpScaled.Size = new System.Drawing.Size(264, 108);
			this.grpScaled.TabIndex = 1;
			this.grpScaled.TabStop = false;
			this.grpScaled.Text = "Scaled Print Size";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(12, 20);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(44, 20);
			this.label4.TabIndex = 3;
			this.label4.Text = "&Width:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(12, 48);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(44, 20);
			this.label5.TabIndex = 6;
			this.label5.Text = "&Height:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// nudWidth
			// 
			this.nudWidth.DecimalPlaces = 3;
			this.nudWidth.Increment = new System.Decimal(new int[] {
																	   1,
																	   0,
																	   0,
																	   65536});
			this.nudWidth.Location = new System.Drawing.Point(60, 20);
			this.nudWidth.Maximum = new System.Decimal(new int[] {
																	 10000,
																	 0,
																	 0,
																	 0});
			this.nudWidth.Name = "nudWidth";
			this.nudWidth.Size = new System.Drawing.Size(72, 20);
			this.nudWidth.TabIndex = 4;
			this.nudWidth.ThousandsSeparator = true;
			this.nudWidth.ValueChanged += new System.EventHandler(this.SizeValue_Changed);
			// 
			// nudHeight
			// 
			this.nudHeight.DecimalPlaces = 3;
			this.nudHeight.Increment = new System.Decimal(new int[] {
																		1,
																		0,
																		0,
																		65536});
			this.nudHeight.Location = new System.Drawing.Point(60, 48);
			this.nudHeight.Maximum = new System.Decimal(new int[] {
																	  10000,
																	  0,
																	  0,
																	  0});
			this.nudHeight.Name = "nudHeight";
			this.nudHeight.Size = new System.Drawing.Size(72, 20);
			this.nudHeight.TabIndex = 7;
			this.nudHeight.ThousandsSeparator = true;
			this.nudHeight.ValueChanged += new System.EventHandler(this.SizeValue_Changed);
			// 
			// cbWidthUnit
			// 
			this.cbWidthUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbWidthUnit.Location = new System.Drawing.Point(136, 20);
			this.cbWidthUnit.Name = "cbWidthUnit";
			this.cbWidthUnit.Size = new System.Drawing.Size(116, 21);
			this.cbWidthUnit.TabIndex = 5;
			this.cbWidthUnit.SelectedIndexChanged += new System.EventHandler(this.cbUnit_SelectedIndexChanged);
			// 
			// cbHeightUnit
			// 
			this.cbHeightUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbHeightUnit.Location = new System.Drawing.Point(136, 48);
			this.cbHeightUnit.Name = "cbHeightUnit";
			this.cbHeightUnit.Size = new System.Drawing.Size(116, 21);
			this.cbHeightUnit.TabIndex = 8;
			this.cbHeightUnit.SelectedIndexChanged += new System.EventHandler(this.cbUnit_SelectedIndexChanged);
			// 
			// chkScaleToFit
			// 
			this.chkScaleToFit.Location = new System.Drawing.Point(12, 76);
			this.chkScaleToFit.Name = "chkScaleToFit";
			this.chkScaleToFit.Size = new System.Drawing.Size(240, 20);
			this.chkScaleToFit.TabIndex = 2;
			this.chkScaleToFit.Text = "Scale to fit page";
			this.chkScaleToFit.CheckedChanged += new System.EventHandler(this.chkScaleToFit_CheckedChanged);
			// 
			// chkPrintSelection
			// 
			this.chkPrintSelection.Location = new System.Drawing.Point(8, 236);
			this.chkPrintSelection.Name = "chkPrintSelection";
			this.chkPrintSelection.Size = new System.Drawing.Size(240, 20);
			this.chkPrintSelection.TabIndex = 2;
			this.chkPrintSelection.Text = "Print selection";
			this.chkPrintSelection.CheckedChanged += new System.EventHandler(this.chkPrintSelection_CheckedChanged);
			// 
			// grpPreview
			// 
			this.grpPreview.Controls.Add(this._previewCtrl);
			this.grpPreview.Location = new System.Drawing.Point(4, 4);
			this.grpPreview.Name = "grpPreview";
			this.grpPreview.Size = new System.Drawing.Size(280, 272);
			this.grpPreview.TabIndex = 0;
			this.grpPreview.TabStop = false;
			this.grpPreview.Text = "Preview";
			// 
			// _previewCtrl
			// 
			this._previewCtrl.AutoCalculatePageInfo = true;
			this._previewCtrl.AutoZoom = false;
			this._previewCtrl.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this._previewCtrl.ForeColor = System.Drawing.Color.White;
			this._previewCtrl.Location = new System.Drawing.Point(8, 16);
			this._previewCtrl.Name = "_previewCtrl";
			this._previewCtrl.PrintSettings = null;
			this._previewCtrl.Size = new System.Drawing.Size(264, 248);
			this._previewCtrl.TabIndex = 0;
			this._previewCtrl.Zoom = 0.3;
			// 
			// btnReset
			// 
			this.btnReset.Location = new System.Drawing.Point(292, 92);
			this.btnReset.Name = "btnReset";
			this.btnReset.TabIndex = 4;
			this.btnReset.Text = "Reset";
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// btnPageSetup
			// 
			this.btnPageSetup.Location = new System.Drawing.Point(292, 64);
			this.btnPageSetup.Name = "btnPageSetup";
			this.btnPageSetup.TabIndex = 5;
			this.btnPageSetup.Text = "Page Setup";
			this.btnPageSetup.Click += new System.EventHandler(this.btnPageSetup_Click);
			// 
			// DlgPrintSettings
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(374, 556);
			this.Controls.Add(this.grpPreview);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.grpParameters);
			this.Controls.Add(this.btnPageSetup);
			this.Controls.Add(this.btnReset);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DlgPrintSettings";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Print Settings";
			this.grpParameters.ResumeLayout(false);
			this.grpPosition.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.nudTop)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudLeft)).EndInit();
			this.grpScaled.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.nudWidth)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudHeight)).EndInit();
			this.grpPreview.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region internal helpers

		private void InitializeUnitComboBox(ComboBox ctrl)
		{
			ctrl.BeginUpdate();
			
			// initializes items
			ctrl.Items.Add(UNIT_INCHES);
			ctrl.Items.Add(UNIT_CENTIMETS);
			ctrl.Items.Add(UNIT_MILIMETS);
			
			// set default selection
			ctrl.SelectedIndex = 0;

			ctrl.EndUpdate();
		}

		private void InitializeControls()
		{
			this.InitializeUnitComboBox(cbTopUnit);
			this.InitializeUnitComboBox(cbLeftUnit);
			this.InitializeUnitComboBox(cbWidthUnit);
			this.InitializeUnitComboBox(cbHeightUnit);

			// initializes numeric updown
			nudTop.Maximum = Decimal.MaxValue;
			nudLeft.Maximum = Decimal.MaxValue;
			nudWidth.Maximum = Decimal.MaxValue;
			nudHeight.Maximum = Decimal.MaxValue;

			// auto zoom to fit page
			_previewCtrl.AutoZoom = true;
		}


		private float GetLeftMargin()
		{
			string unit = cbLeftUnit.SelectedItem.ToString();
			return ConvertUnitToHundredthOfInch((float)nudLeft.Value, unit);
		}

		private float GetTopMargin()
		{
			string unit = cbTopUnit.SelectedItem.ToString();
			return ConvertUnitToHundredthOfInch((float)nudTop.Value, unit);
		}

		private float GetPrintWidth()
		{
			string unit = cbWidthUnit.SelectedItem.ToString();
			return ConvertUnitToHundredthOfInch((float)nudWidth.Value, unit);
		}

		private float GetPrintHeight()
		{
			string unit = cbHeightUnit.SelectedItem.ToString();
			return ConvertUnitToHundredthOfInch((float)nudHeight.Value, unit);
		}


		/// <summary>
		/// Convert from value of a specified unit to hundredth of an inch
		/// </summary>
		/// <param name="value"></param>
		/// <param name="unit"></param>
		/// <returns></returns>
		private float ConvertUnitToHundredthOfInch(float value, string unit)
		{
			switch (unit)
			{
				case UNIT_INCHES:
					return (float)(value * 100);
				case UNIT_CENTIMETS:
					return (float)(((value * 100) / 2.54));
				case UNIT_MILIMETS:
					return (float)(((value * 10000) / 2.54));
				default:
					throw new System.Exception("Unkown unit");
			}
		}

		/// <summary>
		/// Convert from value in hundredth of an inch to a value of a specified unit
		/// </summary>
		/// <param name="value"></param>
		/// <param name="unit"></param>
		/// <returns></returns>
		private float ConvertHundredthOfInchToUnit(float value, string unit)
		{
			switch (unit)
			{
				case UNIT_INCHES:
					return (float)(value / 100.0F);
				case UNIT_CENTIMETS:
					return (float)(((value * 2.54) / 100));
				case UNIT_MILIMETS:
					return (float)(((value * 254) / 100));
				default:
					throw new System.Exception("Unkown unit");
			}
		}

		#endregion
		
		#region override routines
		
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			this.chkCenterImage.Checked = this._printSettings.CenterImage;
			
			_ignoreDataChangedEvent++;

			// update data
			this.chkScaleToFit.Checked = this._printSettings.ScaleToFit;
			
			this.chkPrintSelection.Enabled = this._printSettings.SelectionRectangle.Width > 0 && this._printSettings.SelectionRectangle.Height > 0;
			this.chkPrintSelection.Checked = this._printSettings.PrintSelection && this._printSettings.SelectionRectangle.Width > 0 && this._printSettings.SelectionRectangle.Height > 0;
			

			this.nudLeft.Value = Convert.ToDecimal(ConvertHundredthOfInchToUnit(this._printSettings.X, cbLeftUnit.SelectedItem.ToString()));
			this.nudTop.Value = Convert.ToDecimal(ConvertHundredthOfInchToUnit(this._printSettings.Y, cbTopUnit.SelectedItem.ToString()));

			this.nudWidth.Value = Convert.ToDecimal(ConvertHundredthOfInchToUnit(this._printSettings.Width, cbWidthUnit.SelectedItem.ToString()));
			this.nudHeight.Value = Convert.ToDecimal(ConvertHundredthOfInchToUnit(this._printSettings.Height, cbHeightUnit.SelectedItem.ToString()));


			_ignoreDataChangedEvent--;

			// invalidate preview
			this._previewCtrl.InvalidatePreview();
		}

		#endregion

		#region event handlers

		private void chkPrintSelection_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (this._ignoreDataChangedEvent == 0)
				{
					this._ignoreDataChangedEvent++;
					
					// set print selection
					_printSettings.PrintSelection = chkPrintSelection.Checked;

					// refresh controls' values
					this.nudLeft.Value = Convert.ToDecimal(ConvertHundredthOfInchToUnit(this._printSettings.X, cbLeftUnit.SelectedItem.ToString()));
					this.nudTop.Value = Convert.ToDecimal(ConvertHundredthOfInchToUnit(this._printSettings.Y, cbTopUnit.SelectedItem.ToString()));

					this.nudWidth.Value = Convert.ToDecimal(ConvertHundredthOfInchToUnit(this._printSettings.Width, cbWidthUnit.SelectedItem.ToString()));
					this.nudHeight.Value = Convert.ToDecimal(ConvertHundredthOfInchToUnit(this._printSettings.Height, cbHeightUnit.SelectedItem.ToString()));


					// update preview
					_previewCtrl.InvalidatePreview();

					this._ignoreDataChangedEvent--;
				}
			}
			finally
			{
			}
		}

		private void btnReset_Click(object sender, System.EventArgs e)
		{
			_ignoreDataChangedEvent++;

			// reset _printSettigns
			this.PrintSettings = (PrintSettings)_orgPrintSettings.Clone();			

			// update data
			this.chkScaleToFit.Checked = this._printSettings.ScaleToFit;
			this.chkCenterImage.Checked = this._printSettings.CenterImage;
			this.chkPrintSelection.Checked = this._printSettings.PrintSelection;
			
			this.nudLeft.Value = Convert.ToDecimal(ConvertHundredthOfInchToUnit(this._printSettings.X, cbLeftUnit.SelectedItem.ToString()));
			this.nudTop.Value = Convert.ToDecimal(ConvertHundredthOfInchToUnit(this._printSettings.Y, cbTopUnit.SelectedItem.ToString()));

			this.nudWidth.Value = Convert.ToDecimal(ConvertHundredthOfInchToUnit(this._printSettings.Width, cbWidthUnit.SelectedItem.ToString()));
			this.nudHeight.Value = Convert.ToDecimal(ConvertHundredthOfInchToUnit(this._printSettings.Height, cbHeightUnit.SelectedItem.ToString()));

			_ignoreDataChangedEvent--;

			// invalidate preview
			this._previewCtrl.InvalidatePreview();
		}

		private void btnPageSetup_Click(object sender, System.EventArgs e)
		{
			using (PageSetupDialog dlg = new PageSetupDialog())
			{
				dlg.AllowOrientation = true;
				dlg.AllowMargins = false;
				dlg.Document = _printSettings.Document;
				if (DialogResult.OK == dlg.ShowDialog(this))
				{
					PrinterSettings settings = dlg.PrinterSettings;
					PaperSize paperSize = settings.DefaultPageSettings.PaperSize;
					// check if paper size is emtpy
					if (paperSize.Width == 0 || paperSize.Height == 0)
					{
						try
						{
							// retrieve custom paper size directly from printer
//							DEVMODE devMode = this.GetPrinterSettings(settings.PrinterName);
//							int paperLength = devMode.dmPaperLength;
//							int paperWidth = devMode.dmPaperWidth;
//							if (paperLength == 0 || paperLength == 0)
//							{
//								MessageBox.Show(this, "This paper size is not supported", "Printing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
//								return;
//							}

							Size newSize = this.GetCustomPaperSize(settings.PrinterName);
							int paperHeight = newSize.Height;
							int paperWidth = newSize.Width;

							// apply new size
							PaperSize customSize = new PaperSize("Custom PaperSize", paperWidth, paperHeight);
							_printSettings.Document.DefaultPageSettings.PaperSize = customSize;
							_printSettings.UseCustomPaperSize = true;
							_printSettings.CustomPaperSize = customSize;
						}
						catch (System.Exception exp)
						{
							Trace.WriteLine(exp);

							string msg = String.Format("Failed to use to printer {0}:{1}", settings.PrinterName, exp.Message);
							MessageBox.Show(this, msg, "Printing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
							
							return;
						}
						finally
						{
						}
					}
					else
					{
						_printSettings.UseCustomPaperSize = false;
					}
					
					_printSettings.Document.PrinterSettings = dlg.PrinterSettings;
					// invalidate preview
					this._previewCtrl.InvalidatePreview();
				}
			}
		}

		private void PositionValue_Changed(object sender, System.EventArgs e)
		{
			try
			{
				if (this._ignoreDataChangedEvent == 0)
				{
					this._ignoreDataChangedEvent++;

					// update print position
					_printSettings.X = GetLeftMargin();
					_printSettings.Y = GetTopMargin();

					// invalidate preview
					this._previewCtrl.InvalidatePreview();

					this._ignoreDataChangedEvent--;
				}
			}
			finally
			{
			}
		}

		private void SizeValue_Changed(object sender, System.EventArgs e)
		{	
			try
			{
				if (this._ignoreDataChangedEvent == 0)
				{
					this._ignoreDataChangedEvent++;

					// update print size
					if (sender == nudWidth)
					{
						// calculates original width in hundredth of an inch
						float orgWidth = _printSettings.Width/_printSettings.ScaleFactor;
						// retrieve new width of printable rectangle
						float newWidth = GetPrintWidth();
						// computes new scaleFactor with proportion
						float scaleFactor = newWidth/orgWidth;
						// apply settings with this scale factor
						_printSettings.ScaleFactor = scaleFactor;

						// update numeric updown height
						this.nudHeight.Value = Convert.ToDecimal(ConvertHundredthOfInchToUnit(this._printSettings.Height, cbHeightUnit.SelectedItem.ToString()));
					}
					else if (sender == nudHeight)
					{
						// calculates original height in hundredth of an inch
						float orgHeight = _printSettings.Height/_printSettings.ScaleFactor;
						// retrieve new height of printable rectangle
						float newHeight = GetPrintHeight();
						// computes new scaleFactor with proportion
						float scaleFactor = newHeight/orgHeight;
						// apply settings with this scale factor
						_printSettings.ScaleFactor = scaleFactor;

						// update numeric updown width
						this.nudWidth.Value = Convert.ToDecimal(ConvertHundredthOfInchToUnit(this._printSettings.Width, cbWidthUnit.SelectedItem.ToString()));
					}

					// invalidate preview
					this._previewCtrl.InvalidatePreview();

					this._ignoreDataChangedEvent--;
				}
			}
			finally
			{
			}
		}

		private void chkCenterImage_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (this._ignoreDataChangedEvent == 0)
				{
					this._ignoreDataChangedEvent++;
				
					nudTop.Enabled = !chkCenterImage.Checked & !chkScaleToFit.Checked;
					nudLeft.Enabled = !chkCenterImage.Checked & !chkScaleToFit.Checked;

					// update print position
					_printSettings.CenterImage = chkCenterImage.Checked;

					// invalidate preview
					this._previewCtrl.InvalidatePreview();

					this._ignoreDataChangedEvent--;
				}
			}
			finally
			{
			}
		}

		private void chkScaleToFit_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				chkCenterImage.Enabled = !chkScaleToFit.Checked;

				nudTop.Enabled = !chkCenterImage.Checked & !chkScaleToFit.Checked;
				nudLeft.Enabled = !chkCenterImage.Checked & !chkScaleToFit.Checked;

				nudWidth.Enabled = !chkScaleToFit.Checked;
				nudHeight.Enabled = !chkScaleToFit.Checked;

				if (this._ignoreDataChangedEvent == 0)
				{
					this._ignoreDataChangedEvent++;

					if (chkScaleToFit.Checked)
					{
						_printSettings.ScaleToFit = true;
						_printSettings.CenterImage = true;
					}
					else
					{
						_printSettings.ScaleToFit = false;
						_printSettings.CenterImage = true;
					}

					// update data
					this.chkScaleToFit.Checked = this._printSettings.ScaleToFit;
					this.chkCenterImage.Checked = this._printSettings.CenterImage;
			
					this.nudLeft.Value = Convert.ToDecimal(ConvertHundredthOfInchToUnit(this._printSettings.X, cbLeftUnit.SelectedItem.ToString()));
					this.nudTop.Value = Convert.ToDecimal(ConvertHundredthOfInchToUnit(this._printSettings.Y, cbTopUnit.SelectedItem.ToString()));

					this.nudWidth.Value = Convert.ToDecimal(ConvertHundredthOfInchToUnit(this._printSettings.Width, cbWidthUnit.SelectedItem.ToString()));
					this.nudHeight.Value = Convert.ToDecimal(ConvertHundredthOfInchToUnit(this._printSettings.Height, cbHeightUnit.SelectedItem.ToString()));

					// invalidate preview
					this._previewCtrl.InvalidatePreview();

					this._ignoreDataChangedEvent--;
				}
			}
			finally
			{
			}
		}

		private void cbUnit_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				_ignoreDataChangedEvent++;

				if (sender == cbLeftUnit)
					this.nudLeft.Value = Convert.ToDecimal(ConvertHundredthOfInchToUnit(this._printSettings.X, cbLeftUnit.SelectedItem.ToString()));
				else if (sender == cbTopUnit)
					this.nudTop.Value = Convert.ToDecimal(ConvertHundredthOfInchToUnit(this._printSettings.Y, cbTopUnit.SelectedItem.ToString()));
				else if (sender == cbWidthUnit)
					this.nudWidth.Value = Convert.ToDecimal(ConvertHundredthOfInchToUnit(this._printSettings.Width, cbWidthUnit.SelectedItem.ToString()));
				else if (sender == cbHeightUnit)
					this.nudHeight.Value = Convert.ToDecimal(ConvertHundredthOfInchToUnit(this._printSettings.Height, cbHeightUnit.SelectedItem.ToString()));
			}
			finally
			{
				_ignoreDataChangedEvent--;
			}
		}

		
		
		#endregion

		#region properties

		public PrintSettings PrintSettings
		{
			get {return _printSettings;}
			set
			{
				_printSettings = value;
				OnPrintSettingsChanged();
			}
		}

		protected virtual void OnPrintSettingsChanged()
		{
			this._previewCtrl.PrintSettings = _printSettings;
		}

		#endregion

		#region fields

		private PrintSettings _orgPrintSettings = null;
		private PrintSettings _printSettings = null;

		private int _ignoreDataChangedEvent = 0;

		#endregion

		#region internal helpers

		[DllImport("winspool.Drv", EntryPoint="DocumentPropertiesA", SetLastError=true, ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
		private static extern int DocumentProperties (IntPtr hwnd, IntPtr hPrinter, [MarshalAs(UnmanagedType.LPStr)] string pDeviceNameg, IntPtr pDevModeOutput, ref IntPtr pDevModeInput, int fMode);

		[DllImport("winspool.Drv", EntryPoint="OpenPrinterA", SetLastError=true, CharSet=CharSet.Ansi, ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
		private static extern int OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, ref PRINTER_DEFAULTS printerDefaults);

		[DllImport("winspool.Drv", EntryPoint="ClosePrinter", SetLastError=true, CharSet=CharSet.Ansi, ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
		private static extern int ClosePrinter(IntPtr hPrinter);

		[DllImport("winspool.Drv", EntryPoint="GetPrinterA", SetLastError=true, CharSet=CharSet.Ansi, ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
		private static extern bool GetPrinter(IntPtr hPrinter, Int32 dwLevel, IntPtr pPrinter, Int32 dwBuf, out Int32 dwNeeded);

		[DllImport("winspool.Drv", EntryPoint="EnumFormsA", SetLastError=true, CharSet=CharSet.Ansi, ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
		private static extern int EnumForms(IntPtr hPrinter, Int32 dwLevel, IntPtr pForm, Int32 dwBuf, out Int32 dwNeeded, out Int32 dwReturned);

		[DllImport("winspool.drv", EntryPoint="GetFormA", SetLastError=true, CharSet=CharSet.Ansi, ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
		public static extern int GetForm(IntPtr hPrinter, [MarshalAs(UnmanagedType.LPStr)]string pFormName, Int32 Level, ref IntPtr pForm, Int32 cbBuf, ref Int32 pcbNeeded);

		public Size GetCustomPaperSize(string printerName)
		{
			//const int DM_IN_BUFFER = 8;
			const int DM_OUT_BUFFER = 2;

			//const int PRINTER_ACCESS_ADMINISTER = 0x4;
			const int PRINTER_ACCESS_USE = 0x8;
			//const int PRINTER_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | PRINTER_ACCESS_ADMINISTER | PRINTER_ACCESS_USE);

			Size customSize = Size.Empty;
			IntPtr hPrinter = IntPtr.Zero;
			int lastError = 0;
			int nRet = 0;
			int nBytesNeeded = 0;

			// inits printer value struct
			PRINTER_DEFAULTS printerValues = new PRINTER_DEFAULTS();
			printerValues.pDatatype = 0;
			printerValues.pDevMode = 0 ;
			printerValues.DesiredAccess = PRINTER_ACCESS_USE;
	
			try
			{
				// try to open printer with given name
				nRet = OpenPrinter(printerName, out hPrinter, ref printerValues);
				// check result 
				if (nRet == 0)
				{
					lastError = Marshal.GetLastWin32Error();
					throw new Win32Exception(Marshal.GetLastWin32Error()); 
				}
			
				// retrieves information about connected printer
				GetPrinter(hPrinter, 2, IntPtr.Zero, 0, out nBytesNeeded);

				// check result
				if (nBytesNeeded <= 0)
				{
					throw new System.Exception("Unable to allocate memory");
				}
				else
				{
					IntPtr ptrDevMode = IntPtr.Zero;
					try
					{
						IntPtr ptrZero = IntPtr.Zero;
						// get the size of the devmode structure
						int sizeOfDevMode = DocumentProperties(IntPtr.Zero, hPrinter, printerName, ptrZero, ref ptrZero, 0);
						if (sizeOfDevMode < 0)
						{
							lastError = Marshal.GetLastWin32Error();
							throw new Win32Exception(Marshal.GetLastWin32Error()); 
						}
			
						ptrDevMode = Marshal.AllocHGlobal(sizeOfDevMode);
						nRet = DocumentProperties(IntPtr.Zero, hPrinter, printerName, ptrDevMode, ref ptrZero, DM_OUT_BUFFER);
						if (nRet < 0 || (ptrDevMode == IntPtr.Zero))
						{
							lastError = Marshal.GetLastWin32Error();
							throw new Win32Exception(Marshal.GetLastWin32Error());
						}

						DEVMODE devMode = (DEVMODE)Marshal.PtrToStructure(ptrDevMode, typeof(DEVMODE));
						string name = devMode.dmFormName;
						customSize.Width = devMode.dmPaperWidth;
						customSize.Height = devMode.dmPaperLength;
					}
					finally
					{
						if (ptrDevMode != IntPtr.Zero)
							Marshal.FreeHGlobal(ptrDevMode);
						ptrDevMode = IntPtr.Zero;
					}
				}
			}
			finally
			{
				if (hPrinter != IntPtr.Zero)
					ClosePrinter(hPrinter);
				hPrinter = IntPtr.Zero;
			}
			
			return customSize;
		}

		public DEVMODE GetPrinterSettings(string printerName)
		{
			//const int DM_IN_BUFFER = 8;
			const int DM_OUT_BUFFER = 2;

			//const int PRINTER_ACCESS_ADMINISTER = 0x4;
			const int PRINTER_ACCESS_USE = 0x8;
			//const int PRINTER_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | PRINTER_ACCESS_ADMINISTER | PRINTER_ACCESS_USE);

			DEVMODE dm;
			IntPtr hPrinter = IntPtr.Zero;
			IntPtr ptrPrinterInfo = IntPtr.Zero;
			int lastError = 0;
			int nRet = 0;
			int nBytesNeeded = 0;
			int nJunk = 0;

			// inits printer value struct
			PRINTER_DEFAULTS printerValues = new PRINTER_DEFAULTS();
			printerValues.pDatatype = 0;
			printerValues.pDevMode = 0 ;
			printerValues.DesiredAccess = PRINTER_ACCESS_USE;
	
			// try to open printer with given name
			nRet = OpenPrinter(printerName, out hPrinter, ref printerValues);
			// check result 
			if (nRet == 0)
			{
				lastError = Marshal.GetLastWin32Error();
				throw new Win32Exception(Marshal.GetLastWin32Error()); 
			}
			
			// retrieves information about connected printer
			GetPrinter(hPrinter, 2, IntPtr.Zero, 0, out nBytesNeeded);

			// check result
			if (nBytesNeeded <= 0)
			{
				throw new System.Exception("Unable to allocate memory");
			}
			else
			{
				try
				{
					// allocate enough space for PRINTER_INFO... 
					ptrPrinterInfo = Marshal.AllocHGlobal(nBytesNeeded);
					
					// the second GetPrinter fills in all the current settings, so all you 
					// need to do is modify what you're interested in...
					nRet = Convert.ToInt32(GetPrinter(hPrinter, 2, ptrPrinterInfo, nBytesNeeded, out nJunk));
					
					// check result
					if (nRet == 0)
					{
						lastError = Marshal.GetLastWin32Error();
						throw new Win32Exception(Marshal.GetLastWin32Error()); 
					}

					// convert from IntPtr to PRINTER_INFO struct
					PRINTER_INFO printerInfo = (PRINTER_INFO)Marshal.PtrToStructure(ptrPrinterInfo, typeof(PRINTER_INFO));
					IntPtr ptrDevMode = IntPtr.Zero;
					
					try
					{

						if (printerInfo.pDevMode == IntPtr.Zero)
						{
							// If GetPrinter didn't fill in the DEVMODE, try to get it by calling
							// DocumentProperties...
							IntPtr ptrZero = IntPtr.Zero;
							
							// get the size of the devmode structure
							int sizeOfDevMode = DocumentProperties(IntPtr.Zero, hPrinter, printerName, ptrZero, ref ptrZero, 0);
							if (sizeOfDevMode < 0)
							{
								lastError = Marshal.GetLastWin32Error();
								throw new Win32Exception(Marshal.GetLastWin32Error()); 
							}
			
							ptrDevMode = Marshal.AllocCoTaskMem(sizeOfDevMode);
							nRet = DocumentProperties(IntPtr.Zero, hPrinter, printerName, ptrDevMode, ref ptrZero, DM_OUT_BUFFER);
							if ((nRet < 0) || (ptrDevMode == IntPtr.Zero))
							{
								// Cannot get the DEVMODE structure.
								throw new System.Exception("Cannot get DEVMODE data"); 
							}

							printerInfo.pDevMode = ptrDevMode;
						}

						IntPtr ptrInput = new IntPtr();
					
						// the number of bytes required by the printer driver's DEVMODE data structure (fMode = 0);
						nRet = DocumentProperties(IntPtr.Zero, hPrinter, printerName, IntPtr.Zero , ref ptrInput , 0);
						if (nRet < 0)
						{
							lastError = Marshal.GetLastWin32Error();
							throw new Win32Exception(Marshal.GetLastWin32Error()); 
						}
					
						// allocate buffer for retrieving printer information
						IntPtr ptrOutput = Marshal.AllocHGlobal(nRet);
						try
						{
							// retrieve printer information
							nRet = DocumentProperties(IntPtr.Zero, hPrinter, printerName, ptrOutput, ref ptrInput , DM_OUT_BUFFER);
							if ((nRet == 0) || (ptrOutput == IntPtr.Zero))
							{
								lastError = Marshal.GetLastWin32Error();
								throw new Win32Exception(Marshal.GetLastWin32Error()); 
							}

							// convert data from pointer to struct DEVMODE
							DEVMODE devMode = (DEVMODE)Marshal.PtrToStructure(ptrOutput, typeof(DEVMODE));
							dm = new DEVMODE(devMode);		
				
							IntPtr ptrForm = IntPtr.Zero;
							try
							{
								int sizeStruct = Marshal.SizeOf(typeof(FORM_INFO_1));
								nBytesNeeded = sizeStruct;
								int nFormCount = 0;
								
								// enumerates the needed byte for storing printer information
								nRet = EnumForms(hPrinter, 1, IntPtr.Zero, 0, out nBytesNeeded, out nFormCount);
							
								// allocate new memory with nBytesNeeded
								ptrForm = Marshal.AllocHGlobal(nBytesNeeded);
								// enumerates the form supported by the given printer
								nRet = EnumForms(hPrinter, 1, ptrForm, nBytesNeeded, out nBytesNeeded, out nFormCount);
								if (nRet == 0)
								{
									lastError = Marshal.GetLastWin32Error();
									throw new Win32Exception(Marshal.GetLastWin32Error()); 
								}

								int ptrFirst = ptrForm.ToInt32();
								int ptrLast = ptrFirst + sizeStruct*nFormCount;
								for (int ptr = ptrForm.ToInt32(), iForm = 0; iForm < nFormCount && ptr < ptrLast; ptr += sizeStruct, iForm++)
								{
									FORM_INFO_1 formInfo = (FORM_INFO_1)Marshal.PtrToStructure(new IntPtr(ptr), typeof(FORM_INFO_1));
									string formName = formInfo.pName.ToLower();
									
									Console.WriteLine(formName);

									if (formName.IndexOf("custom") >= 0)
									{

									}
								}

								

							}
							finally
							{
								if (ptrForm != IntPtr.Zero)
									Marshal.FreeHGlobal(ptrForm);
								ptrForm = IntPtr.Zero;
							}

							// get bytes need for storing form info
							nRet = GetForm(hPrinter, "Custom", 1, ref ptrForm, 0, ref nBytesNeeded);
							if (nRet == 0)
							{
								lastError = Marshal.GetLastWin32Error();
								throw new Win32Exception(Marshal.GetLastWin32Error()); 
							}

							// allocate new memory with nBytesNeeded
							ptrForm = Marshal.AllocHGlobal(nBytesNeeded);

							// get form information
							nRet = GetForm(hPrinter, "Custom", 1, ref ptrForm, nBytesNeeded, ref nBytesNeeded);
							if (nRet == 0)
							{
								lastError = Marshal.GetLastWin32Error();
								throw new Win32Exception(Marshal.GetLastWin32Error()); 
							}
						}
						finally
						{
							if (ptrOutput != IntPtr.Zero)
								Marshal.FreeHGlobal(ptrOutput);
							ptrOutput = IntPtr.Zero;
						}
					}
					finally
					{
						if (ptrDevMode != IntPtr.Zero)
							Marshal.FreeHGlobal(ptrDevMode);
						ptrDevMode = IntPtr.Zero;
					}
				}
				finally
				{
					if (ptrPrinterInfo != IntPtr.Zero)
						Marshal.FreeHGlobal(ptrPrinterInfo);
					ptrPrinterInfo = IntPtr.Zero;
				}
			}
			return dm;
		}

		#endregion
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct PRINTER_DEFAULTS
	{
		public int pDatatype;
		public int pDevMode;
		public int DesiredAccess;
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct SIZEL
	{
		public Int32 width;
		public Int32 height;

	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct RECTL
	{
		public Int32 left;
		public Int32 top;
		public Int32 right;
		public Int32 bottom;
	}

	[StructLayout(LayoutKind.Explicit, CharSet=CharSet.Ansi)]
	internal struct FORM_INFO_1
	{
		[FieldOffset(0), MarshalAs(UnmanagedType.I4)] 
		public uint Flags;
		[FieldOffset(4), MarshalAs(UnmanagedType.LPStr)] 
		public string pName;
		[FieldOffset(8)]
		public SIZEL Size;
		[FieldOffset(16)]
		public RECTL ImagableArea;
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct PRINTER_INFO
	{
		[MarshalAs(UnmanagedType.LPStr)] 
		public string pServerName; 
		[MarshalAs(UnmanagedType.LPStr)] 
		public string pPrinterName; 
		[MarshalAs(UnmanagedType.LPStr)] 
		public string pShareName; 
		[MarshalAs(UnmanagedType.LPStr)] 
		public string pPortName; 
		[MarshalAs(UnmanagedType.LPStr)] 
		public string pDriverName; 
		[MarshalAs(UnmanagedType.LPStr)] 
		public string pComment; 
		[MarshalAs(UnmanagedType.LPStr)] 
		public string pLocation; 
		public IntPtr pDevMode; 
		[MarshalAs(UnmanagedType.LPStr)] 
		public string pSepFile; 
		[MarshalAs(UnmanagedType.LPStr)] 
		public string pPrintProcessor; 
		[MarshalAs(UnmanagedType.LPStr)] 
		public string pDatatype; 
		[MarshalAs(UnmanagedType.LPStr)] 
		public string pParameters; 
		public IntPtr pSecurityDescriptor; 
		public Int32 Attributes; 
		public Int32 Priority; 
		public Int32 DefaultPriority; 
		public Int32 StartTime; 
		public Int32 UntilTime; 
		public Int32 Status; 
		public Int32 cJobs; 
		public Int32 AveragePPM; 
	}

	[StructLayout(LayoutKind.Sequential)] 
	public struct DEVMODE 
	{ 
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] 
		public string dmDeviceName;
		public short dmSpecVersion;
		public short dmDriverVersion;
		public short dmSize;
		public short dmDriverExtra;
		public int dmFields;
		public short dmOrientation;
		public short dmPaperSize;
		public short dmPaperLength;
		public short dmPaperWidth;
		public short dmScale;
		public short dmCopies;
		public short dmDefaultSource;
		public short dmPrintQuality;
		public short dmColor;
		public short dmDuplex;
		public short dmYResolution;
		public short dmTTOption;
		public short dmCollate;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] 
		public string dmFormName; 
		public short dmUnusedPadding;
		public short dmBitsPerPel;
		public int dmPelsWidth;
		public int dmPelsHeight;
		public int dmDisplayFlags;
		public int dmDisplayFrequency;

		public DEVMODE(DEVMODE dm)
		{
			dmDeviceName = String.Copy(dm.dmDeviceName);
			dmSpecVersion = dm.dmSpecVersion;
			dmDriverVersion = dm.dmDriverVersion;
			dmSize = dm.dmSize;
			dmDriverExtra = dm.dmDriverExtra;
			dmFields = dm.dmFields;
			dmOrientation = dm.dmOrientation;
			dmPaperSize = dm.dmPaperSize;
			dmPaperLength = dm.dmPaperLength;
			dmPaperWidth = dm.dmPaperWidth;
			dmScale = dm.dmScale;
			dmCopies = dm.dmCopies;
			dmDefaultSource = dm.dmDefaultSource;
			dmPrintQuality = dm.dmPrintQuality;
			dmColor = dm.dmColor;
			dmDuplex = dm.dmDuplex;
			dmYResolution = dm.dmYResolution;
			dmTTOption = dm.dmTTOption;
			dmCollate = dm.dmCollate;
			dmFormName = String.Copy(dm.dmFormName);
			dmUnusedPadding = dm.dmUnusedPadding;
			dmBitsPerPel = dm.dmBitsPerPel;
			dmPelsWidth = dm.dmPelsWidth;
			dmPelsHeight = dm.dmPelsHeight;
			dmDisplayFlags = dm.dmDisplayFlags;
			dmDisplayFrequency = dm.dmDisplayFrequency;
		}
	}
}
