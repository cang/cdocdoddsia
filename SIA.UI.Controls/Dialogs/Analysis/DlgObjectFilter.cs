using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using SIA.Common;
using SIA.Common.KlarfExport;
using SIA.UI.Controls.Utilities;


namespace SIA.UI.Controls.Dialogs
{
	/// <summary>
	/// Summary description for DlgObjectFilter.
	/// </summary>
	public class DlgObjectFilter : DialogBase
	{
		#region Window Form members

		private System.Windows.Forms.CheckBox chkFilterByItgInt;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.NumericUpDown nudFilterByNumberofPixelsMin;
		private System.Windows.Forms.NumericUpDown nudFilterByNumberofPixelsMax;
		private System.Windows.Forms.CheckBox chkFilterByNumberofPixelsMin;
		private System.Windows.Forms.CheckBox chkFilterByNumberofPixelsMax;
		private System.Windows.Forms.CheckBox chkFilterByNumberofPixels;
		private System.Windows.Forms.NumericUpDown nudFilterByAreaMin;
		private System.Windows.Forms.NumericUpDown nudFilterByAreaMax;
		private System.Windows.Forms.CheckBox chkFilterByAreaMin;
		private System.Windows.Forms.CheckBox chkFilterByAreaMax;
		private System.Windows.Forms.CheckBox chkFilterByArea;
		private System.Windows.Forms.NumericUpDown nudFilterByPerimeterMin;
		private System.Windows.Forms.NumericUpDown nudFilterByPerimeterMax;
		private System.Windows.Forms.CheckBox chkFilterByPerimeterMin;
		private System.Windows.Forms.CheckBox chkFilterByPerimeterMax;
		private System.Windows.Forms.CheckBox chkFilterByPerimeter;
		private System.Windows.Forms.Button btnSaveSettings;
		private System.Windows.Forms.Button btnLoadSettings;
		private System.Windows.Forms.CheckBox chkFilterByItgIntMin;
		private System.Windows.Forms.CheckBox chkFilterByItgIntMax;
		private System.Windows.Forms.NumericUpDown nudFilterByItgIntMin;
		private System.Windows.Forms.NumericUpDown nudFilterByItgIntMax;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion

		#region field members
		private ObjectFilterArguments _objectFilterArgumets = null;
		#endregion

		#region properties

		public ObjectFilterArguments ObjectFilter
		{
			get 
			{ 	
				return _objectFilterArgumets; 
			}

			set 
			{ 
				_objectFilterArgumets = value; 				
			}
		}


		private void FilterByIntegratedIntensityChanged()
		{
			UpdateGroupState(chkFilterByItgIntMin, nudFilterByItgIntMin, 
							chkFilterByItgIntMax, nudFilterByItgIntMax, _objectFilterArgumets.FilterByIntegratedIntensity);						
		}

		private void FilterByNumberOfPixelsChanged()
		{
			UpdateGroupState(chkFilterByNumberofPixelsMin, nudFilterByNumberofPixelsMin, 
						chkFilterByNumberofPixelsMax, nudFilterByNumberofPixelsMax, _objectFilterArgumets.FilterByNumberOfPixels);						
		}

		private void FilterByAreaChanged()
		{
			UpdateGroupState(chkFilterByAreaMin, nudFilterByAreaMin, 
						chkFilterByAreaMax, nudFilterByAreaMax, _objectFilterArgumets.FilterByArea);			
		}

		private void FilterByPerimeterChanged()
		{
			UpdateGroupState(chkFilterByPerimeterMin, nudFilterByPerimeterMin, 
									chkFilterByPerimeterMax, nudFilterByPerimeterMax, _objectFilterArgumets.FilterByPerimeter);				
		}		
		
		#endregion

		#region constructors and destructors
		
		public DlgObjectFilter()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		public DlgObjectFilter(ObjectFilterArguments filter)
		{
			InitializeComponent();

			this.nudFilterByItgIntMin.ValueChanged += new EventHandler(this.GenericValueChanged);
			this.nudFilterByItgIntMax.ValueChanged += new EventHandler(this.GenericValueChanged);
			this.nudFilterByNumberofPixelsMin.ValueChanged += new EventHandler(this.GenericValueChanged);
			this.nudFilterByNumberofPixelsMax.ValueChanged += new EventHandler(this.GenericValueChanged);
			this.nudFilterByAreaMin.ValueChanged += new EventHandler(this.GenericValueChanged);
			this.nudFilterByAreaMax.ValueChanged += new EventHandler(this.GenericValueChanged);
			this.nudFilterByPerimeterMin.ValueChanged += new EventHandler(this.GenericValueChanged);
			this.nudFilterByPerimeterMax.ValueChanged += new EventHandler(this.GenericValueChanged);

			this.chkFilterByItgIntMin.CheckedChanged += new EventHandler(this.GenericCheckedChanged);
			this.chkFilterByItgIntMax.CheckedChanged += new EventHandler(this.GenericCheckedChanged);
			this.chkFilterByNumberofPixelsMin.CheckedChanged += new EventHandler(this.GenericCheckedChanged);
			this.chkFilterByNumberofPixelsMax.CheckedChanged += new EventHandler(this.GenericCheckedChanged);
			this.chkFilterByAreaMin.CheckedChanged += new EventHandler(this.GenericCheckedChanged);
			this.chkFilterByAreaMax.CheckedChanged += new EventHandler(this.GenericCheckedChanged);
			this.chkFilterByPerimeterMin.CheckedChanged += new EventHandler(this.GenericCheckedChanged);
			this.chkFilterByPerimeterMax.CheckedChanged += new EventHandler(this.GenericCheckedChanged);

			this.ObjectFilter = filter;

			UpdateData(false);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgObjectFilter));
            this.nudFilterByItgIntMin = new System.Windows.Forms.NumericUpDown();
            this.nudFilterByItgIntMax = new System.Windows.Forms.NumericUpDown();
            this.chkFilterByItgIntMin = new System.Windows.Forms.CheckBox();
            this.chkFilterByItgIntMax = new System.Windows.Forms.CheckBox();
            this.chkFilterByItgInt = new System.Windows.Forms.CheckBox();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.nudFilterByNumberofPixelsMin = new System.Windows.Forms.NumericUpDown();
            this.nudFilterByNumberofPixelsMax = new System.Windows.Forms.NumericUpDown();
            this.chkFilterByNumberofPixelsMin = new System.Windows.Forms.CheckBox();
            this.chkFilterByNumberofPixelsMax = new System.Windows.Forms.CheckBox();
            this.chkFilterByNumberofPixels = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nudFilterByAreaMin = new System.Windows.Forms.NumericUpDown();
            this.nudFilterByAreaMax = new System.Windows.Forms.NumericUpDown();
            this.chkFilterByAreaMin = new System.Windows.Forms.CheckBox();
            this.chkFilterByAreaMax = new System.Windows.Forms.CheckBox();
            this.chkFilterByArea = new System.Windows.Forms.CheckBox();
            this.nudFilterByPerimeterMin = new System.Windows.Forms.NumericUpDown();
            this.nudFilterByPerimeterMax = new System.Windows.Forms.NumericUpDown();
            this.chkFilterByPerimeterMin = new System.Windows.Forms.CheckBox();
            this.chkFilterByPerimeterMax = new System.Windows.Forms.CheckBox();
            this.chkFilterByPerimeter = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.btnLoadSettings = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudFilterByItgIntMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFilterByItgIntMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFilterByNumberofPixelsMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFilterByNumberofPixelsMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFilterByAreaMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFilterByAreaMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFilterByPerimeterMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFilterByPerimeterMax)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // nudFilterByItgIntMin
            // 
            this.nudFilterByItgIntMin.Location = new System.Drawing.Point(92, 24);
            this.nudFilterByItgIntMin.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.nudFilterByItgIntMin.Name = "nudFilterByItgIntMin";
            this.nudFilterByItgIntMin.Size = new System.Drawing.Size(72, 20);
            this.nudFilterByItgIntMin.TabIndex = 8;
            // 
            // nudFilterByItgIntMax
            // 
            this.nudFilterByItgIntMax.Location = new System.Drawing.Point(92, 48);
            this.nudFilterByItgIntMax.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.nudFilterByItgIntMax.Name = "nudFilterByItgIntMax";
            this.nudFilterByItgIntMax.Size = new System.Drawing.Size(72, 20);
            this.nudFilterByItgIntMax.TabIndex = 5;
            // 
            // chkFilterByItgIntMin
            // 
            this.chkFilterByItgIntMin.Location = new System.Drawing.Point(16, 20);
            this.chkFilterByItgIntMin.Name = "chkFilterByItgIntMin";
            this.chkFilterByItgIntMin.Size = new System.Drawing.Size(72, 20);
            this.chkFilterByItgIntMin.TabIndex = 7;
            this.chkFilterByItgIntMin.Text = "Minimum:";
            // 
            // chkFilterByItgIntMax
            // 
            this.chkFilterByItgIntMax.Location = new System.Drawing.Point(16, 44);
            this.chkFilterByItgIntMax.Name = "chkFilterByItgIntMax";
            this.chkFilterByItgIntMax.Size = new System.Drawing.Size(76, 20);
            this.chkFilterByItgIntMax.TabIndex = 11;
            this.chkFilterByItgIntMax.Text = "Maximum:";
            // 
            // chkFilterByItgInt
            // 
            this.chkFilterByItgInt.Location = new System.Drawing.Point(8, 0);
            this.chkFilterByItgInt.Name = "chkFilterByItgInt";
            this.chkFilterByItgInt.Size = new System.Drawing.Size(136, 16);
            this.chkFilterByItgInt.TabIndex = 6;
            this.chkFilterByItgInt.Text = "By Average Intensity";
            this.chkFilterByItgInt.CheckedChanged += new System.EventHandler(this.IntegratedIntensity_Changed);
            // 
            // label25
            // 
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.Location = new System.Drawing.Point(168, 20);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(56, 20);
            this.label25.TabIndex = 10;
            this.label25.Text = "(intensity)";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label26
            // 
            this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.Location = new System.Drawing.Point(168, 44);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(56, 20);
            this.label26.TabIndex = 9;
            this.label26.Text = "(intensity)";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudFilterByNumberofPixelsMin
            // 
            this.nudFilterByNumberofPixelsMin.Location = new System.Drawing.Point(92, 24);
            this.nudFilterByNumberofPixelsMin.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.nudFilterByNumberofPixelsMin.Name = "nudFilterByNumberofPixelsMin";
            this.nudFilterByNumberofPixelsMin.Size = new System.Drawing.Size(72, 20);
            this.nudFilterByNumberofPixelsMin.TabIndex = 15;
            // 
            // nudFilterByNumberofPixelsMax
            // 
            this.nudFilterByNumberofPixelsMax.Location = new System.Drawing.Point(92, 48);
            this.nudFilterByNumberofPixelsMax.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.nudFilterByNumberofPixelsMax.Name = "nudFilterByNumberofPixelsMax";
            this.nudFilterByNumberofPixelsMax.Size = new System.Drawing.Size(72, 20);
            this.nudFilterByNumberofPixelsMax.TabIndex = 12;
            // 
            // chkFilterByNumberofPixelsMin
            // 
            this.chkFilterByNumberofPixelsMin.Location = new System.Drawing.Point(16, 20);
            this.chkFilterByNumberofPixelsMin.Name = "chkFilterByNumberofPixelsMin";
            this.chkFilterByNumberofPixelsMin.Size = new System.Drawing.Size(72, 20);
            this.chkFilterByNumberofPixelsMin.TabIndex = 14;
            this.chkFilterByNumberofPixelsMin.Text = "Minimum:";
            // 
            // chkFilterByNumberofPixelsMax
            // 
            this.chkFilterByNumberofPixelsMax.Location = new System.Drawing.Point(16, 44);
            this.chkFilterByNumberofPixelsMax.Name = "chkFilterByNumberofPixelsMax";
            this.chkFilterByNumberofPixelsMax.Size = new System.Drawing.Size(76, 20);
            this.chkFilterByNumberofPixelsMax.TabIndex = 18;
            this.chkFilterByNumberofPixelsMax.Text = "Maximum:";
            // 
            // chkFilterByNumberofPixels
            // 
            this.chkFilterByNumberofPixels.Location = new System.Drawing.Point(8, 0);
            this.chkFilterByNumberofPixels.Name = "chkFilterByNumberofPixels";
            this.chkFilterByNumberofPixels.Size = new System.Drawing.Size(136, 16);
            this.chkFilterByNumberofPixels.TabIndex = 13;
            this.chkFilterByNumberofPixels.Text = "By Number of Pixels";
            this.chkFilterByNumberofPixels.CheckedChanged += new System.EventHandler(this.NumberOfPixels_Changed);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(168, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 20);
            this.label1.TabIndex = 17;
            this.label1.Text = "pixel(s)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(168, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 20);
            this.label2.TabIndex = 16;
            this.label2.Text = "pixel(s)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudFilterByAreaMin
            // 
            this.nudFilterByAreaMin.Location = new System.Drawing.Point(92, 20);
            this.nudFilterByAreaMin.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.nudFilterByAreaMin.Name = "nudFilterByAreaMin";
            this.nudFilterByAreaMin.Size = new System.Drawing.Size(72, 20);
            this.nudFilterByAreaMin.TabIndex = 22;
            // 
            // nudFilterByAreaMax
            // 
            this.nudFilterByAreaMax.Location = new System.Drawing.Point(92, 44);
            this.nudFilterByAreaMax.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.nudFilterByAreaMax.Name = "nudFilterByAreaMax";
            this.nudFilterByAreaMax.Size = new System.Drawing.Size(72, 20);
            this.nudFilterByAreaMax.TabIndex = 19;
            // 
            // chkFilterByAreaMin
            // 
            this.chkFilterByAreaMin.Location = new System.Drawing.Point(16, 20);
            this.chkFilterByAreaMin.Name = "chkFilterByAreaMin";
            this.chkFilterByAreaMin.Size = new System.Drawing.Size(72, 20);
            this.chkFilterByAreaMin.TabIndex = 21;
            this.chkFilterByAreaMin.Text = "Minimum:";
            // 
            // chkFilterByAreaMax
            // 
            this.chkFilterByAreaMax.Location = new System.Drawing.Point(16, 44);
            this.chkFilterByAreaMax.Name = "chkFilterByAreaMax";
            this.chkFilterByAreaMax.Size = new System.Drawing.Size(76, 20);
            this.chkFilterByAreaMax.TabIndex = 25;
            this.chkFilterByAreaMax.Text = "Maximum:";
            // 
            // chkFilterByArea
            // 
            this.chkFilterByArea.Location = new System.Drawing.Point(8, 0);
            this.chkFilterByArea.Name = "chkFilterByArea";
            this.chkFilterByArea.Size = new System.Drawing.Size(64, 16);
            this.chkFilterByArea.TabIndex = 20;
            this.chkFilterByArea.Text = "By Area";
            this.chkFilterByArea.CheckedChanged += new System.EventHandler(this.Area_Changed);
            // 
            // nudFilterByPerimeterMin
            // 
            this.nudFilterByPerimeterMin.Location = new System.Drawing.Point(92, 24);
            this.nudFilterByPerimeterMin.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.nudFilterByPerimeterMin.Name = "nudFilterByPerimeterMin";
            this.nudFilterByPerimeterMin.Size = new System.Drawing.Size(72, 20);
            this.nudFilterByPerimeterMin.TabIndex = 29;
            // 
            // nudFilterByPerimeterMax
            // 
            this.nudFilterByPerimeterMax.Location = new System.Drawing.Point(92, 48);
            this.nudFilterByPerimeterMax.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.nudFilterByPerimeterMax.Name = "nudFilterByPerimeterMax";
            this.nudFilterByPerimeterMax.Size = new System.Drawing.Size(72, 20);
            this.nudFilterByPerimeterMax.TabIndex = 26;
            // 
            // chkFilterByPerimeterMin
            // 
            this.chkFilterByPerimeterMin.Location = new System.Drawing.Point(12, 24);
            this.chkFilterByPerimeterMin.Name = "chkFilterByPerimeterMin";
            this.chkFilterByPerimeterMin.Size = new System.Drawing.Size(72, 20);
            this.chkFilterByPerimeterMin.TabIndex = 28;
            this.chkFilterByPerimeterMin.Text = "Minimum:";
            // 
            // chkFilterByPerimeterMax
            // 
            this.chkFilterByPerimeterMax.Location = new System.Drawing.Point(12, 48);
            this.chkFilterByPerimeterMax.Name = "chkFilterByPerimeterMax";
            this.chkFilterByPerimeterMax.Size = new System.Drawing.Size(76, 20);
            this.chkFilterByPerimeterMax.TabIndex = 32;
            this.chkFilterByPerimeterMax.Text = "Maximum:";
            // 
            // chkFilterByPerimeter
            // 
            this.chkFilterByPerimeter.Location = new System.Drawing.Point(8, 0);
            this.chkFilterByPerimeter.Name = "chkFilterByPerimeter";
            this.chkFilterByPerimeter.Size = new System.Drawing.Size(92, 16);
            this.chkFilterByPerimeter.TabIndex = 27;
            this.chkFilterByPerimeter.Text = "By Perimeter";
            this.chkFilterByPerimeter.CheckedChanged += new System.EventHandler(this.Perimeter_Changed);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Location = new System.Drawing.Point(-156, 192);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(638, 4);
            this.groupBox3.TabIndex = 34;
            this.groupBox3.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkFilterByItgInt);
            this.groupBox1.Controls.Add(this.label25);
            this.groupBox1.Controls.Add(this.nudFilterByItgIntMin);
            this.groupBox1.Controls.Add(this.nudFilterByItgIntMax);
            this.groupBox1.Controls.Add(this.chkFilterByItgIntMin);
            this.groupBox1.Controls.Add(this.chkFilterByItgIntMax);
            this.groupBox1.Controls.Add(this.label26);
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(232, 80);
            this.groupBox1.TabIndex = 35;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkFilterByArea);
            this.groupBox2.Controls.Add(this.nudFilterByAreaMin);
            this.groupBox2.Controls.Add(this.chkFilterByAreaMin);
            this.groupBox2.Controls.Add(this.chkFilterByAreaMax);
            this.groupBox2.Controls.Add(this.nudFilterByAreaMax);
            this.groupBox2.Location = new System.Drawing.Point(252, 8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(176, 80);
            this.groupBox2.TabIndex = 36;
            this.groupBox2.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chkFilterByNumberofPixelsMin);
            this.groupBox4.Controls.Add(this.nudFilterByNumberofPixelsMax);
            this.groupBox4.Controls.Add(this.nudFilterByNumberofPixelsMin);
            this.groupBox4.Controls.Add(this.chkFilterByNumberofPixelsMax);
            this.groupBox4.Controls.Add(this.chkFilterByNumberofPixels);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Location = new System.Drawing.Point(8, 108);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(232, 80);
            this.groupBox4.TabIndex = 37;
            this.groupBox4.TabStop = false;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.chkFilterByPerimeterMax);
            this.groupBox5.Controls.Add(this.nudFilterByPerimeterMax);
            this.groupBox5.Controls.Add(this.chkFilterByPerimeterMin);
            this.groupBox5.Controls.Add(this.nudFilterByPerimeterMin);
            this.groupBox5.Controls.Add(this.chkFilterByPerimeter);
            this.groupBox5.Location = new System.Drawing.Point(252, 108);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(176, 80);
            this.groupBox5.TabIndex = 38;
            this.groupBox5.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(356, 200);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 40;
            this.btnCancel.Text = "Cancel";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(276, 200);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 39;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSaveSettings.Location = new System.Drawing.Point(96, 200);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(88, 23);
            this.btnSaveSettings.TabIndex = 42;
            this.btnSaveSettings.Text = "Save Settings";
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // btnLoadSettings
            // 
            this.btnLoadSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoadSettings.Location = new System.Drawing.Point(4, 200);
            this.btnLoadSettings.Name = "btnLoadSettings";
            this.btnLoadSettings.Size = new System.Drawing.Size(88, 23);
            this.btnLoadSettings.TabIndex = 41;
            this.btnLoadSettings.Text = "Load Settings";
            this.btnLoadSettings.Click += new System.EventHandler(this.btnLoadSettings_Click);
            // 
            // DlgObjectFilter
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(436, 228);
            this.Controls.Add(this.btnSaveSettings);
            this.Controls.Add(this.btnLoadSettings);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DlgObjectFilter";
            this.Text = "Objects Filter";
            ((System.ComponentModel.ISupportInitialize)(this.nudFilterByItgIntMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFilterByItgIntMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFilterByNumberofPixelsMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFilterByNumberofPixelsMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFilterByAreaMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFilterByAreaMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFilterByPerimeterMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFilterByPerimeterMax)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#region event handlers

		private void IntegratedIntensity_Changed(object sender, System.EventArgs e)
		{
			_objectFilterArgumets.FilterByIntegratedIntensity = chkFilterByItgInt.Checked;
			FilterByIntegratedIntensityChanged();
		}

		private void Area_Changed(object sender, System.EventArgs e)
		{
			_objectFilterArgumets.FilterByArea = chkFilterByArea.Checked;
			FilterByAreaChanged();		
		}

		private void NumberOfPixels_Changed(object sender, System.EventArgs e)
		{
			_objectFilterArgumets.FilterByNumberOfPixels = chkFilterByNumberofPixels.Checked;
			FilterByNumberOfPixelsChanged();
		}

		private void Perimeter_Changed(object sender, System.EventArgs e)
		{
			_objectFilterArgumets.FilterByPerimeter = chkFilterByPerimeter.Checked;
			FilterByPerimeterChanged();
		}

		private void GenericCheckedChanged(object sender, System.EventArgs e)
		{
			if(sender == chkFilterByItgIntMin)
				nudFilterByItgIntMin.Enabled = chkFilterByItgIntMin.Checked;
			else if(sender == chkFilterByItgIntMax)
				nudFilterByItgIntMax.Enabled = chkFilterByItgIntMax.Checked;
			else if(sender == chkFilterByNumberofPixelsMin)
				nudFilterByNumberofPixelsMin.Enabled = chkFilterByNumberofPixelsMin.Checked;
			else if(sender == chkFilterByNumberofPixelsMax)
				nudFilterByNumberofPixelsMax.Enabled = chkFilterByNumberofPixelsMax.Checked;
			else if(sender == chkFilterByAreaMin)
				nudFilterByAreaMin.Enabled = chkFilterByAreaMin.Checked;
			else if(sender == chkFilterByAreaMax)
				nudFilterByAreaMax.Enabled = chkFilterByAreaMax.Checked;
			else if(sender == chkFilterByPerimeterMin)
				nudFilterByPerimeterMin.Enabled = chkFilterByPerimeterMin.Checked;
			else if(sender == chkFilterByPerimeterMax)
				nudFilterByPerimeterMax.Enabled = chkFilterByPerimeterMax.Checked;							
		}

		private void GenericValueChanged(object sender, System.EventArgs e)
		{
			double _min_cmp = 0;
			double _max_cmp = 10000000000;

			if(sender == nudFilterByItgIntMin)
			{				
				if((double)nudFilterByItgIntMin.Value < _min_cmp)
					nudFilterByItgIntMin.Value = (decimal)_min_cmp;
				if((double)nudFilterByItgIntMin.Value > _max_cmp)
					nudFilterByItgIntMin.Value = (decimal)_max_cmp;
				if(nudFilterByItgIntMax.Enabled && nudFilterByItgIntMin.Value > nudFilterByItgIntMax.Value)
					nudFilterByItgIntMin.Value = nudFilterByItgIntMax.Value;
			}
			else if(sender == nudFilterByItgIntMax)
			{
				if((double)nudFilterByItgIntMax.Value < _min_cmp)
					nudFilterByItgIntMax.Value = (decimal)_min_cmp;
				if((double)nudFilterByItgIntMax.Value > _max_cmp)
					nudFilterByItgIntMax.Value = (decimal)_max_cmp;
				if(nudFilterByItgIntMin.Enabled && nudFilterByItgIntMin.Value > nudFilterByItgIntMax.Value)
					nudFilterByItgIntMax.Value = nudFilterByItgIntMin.Value;
			}
			else if(sender == nudFilterByNumberofPixelsMin)
			{
				if((double)nudFilterByNumberofPixelsMin.Value < _min_cmp)
					nudFilterByNumberofPixelsMin.Value = (decimal)_min_cmp;
				if((double)nudFilterByNumberofPixelsMin.Value > _max_cmp)
					nudFilterByNumberofPixelsMin.Value = (decimal)_max_cmp;
				if(nudFilterByNumberofPixelsMax.Enabled && nudFilterByNumberofPixelsMin.Value > nudFilterByNumberofPixelsMax.Value)
					nudFilterByNumberofPixelsMin.Value = nudFilterByNumberofPixelsMax.Value;
			}
			else if(sender == nudFilterByNumberofPixelsMax)
			{
				if((double)nudFilterByNumberofPixelsMax.Value < _min_cmp)
					nudFilterByNumberofPixelsMax.Value = (decimal)_min_cmp;
				if((double)nudFilterByNumberofPixelsMax.Value > _max_cmp)
					nudFilterByNumberofPixelsMax.Value = (decimal)_max_cmp;
				if(nudFilterByNumberofPixelsMin.Enabled && nudFilterByNumberofPixelsMin.Value > nudFilterByNumberofPixelsMax.Value)
					nudFilterByNumberofPixelsMax.Value = nudFilterByNumberofPixelsMin.Value;
			}
			else if(sender == nudFilterByAreaMin)
			{
				if((double)nudFilterByAreaMin.Value < _min_cmp)
					nudFilterByAreaMin.Value = (decimal)_min_cmp;
				if((double)nudFilterByAreaMin.Value > _max_cmp)
					nudFilterByAreaMin.Value = (decimal)_max_cmp;
				if(nudFilterByAreaMax.Enabled && nudFilterByAreaMin.Value > nudFilterByAreaMax.Value)
					nudFilterByAreaMin.Value = nudFilterByAreaMax.Value;
			}
			else if(sender == nudFilterByAreaMax)
			{
				if((double)nudFilterByAreaMax.Value < _min_cmp)
					nudFilterByAreaMax.Value = (decimal)_min_cmp;
				if((double)nudFilterByAreaMax.Value > _max_cmp)
					nudFilterByAreaMax.Value = (decimal)_max_cmp;
				if(nudFilterByAreaMin.Enabled && nudFilterByAreaMin.Value > nudFilterByAreaMax.Value)
					nudFilterByAreaMax.Value = nudFilterByAreaMin.Value;
			}
			else if(sender == nudFilterByPerimeterMin)
			{
				if((double)nudFilterByPerimeterMin.Value < _min_cmp)
					nudFilterByPerimeterMin.Value = (decimal)_min_cmp;
				if((double)nudFilterByPerimeterMin.Value > _max_cmp)
					nudFilterByPerimeterMin.Value = (decimal)_max_cmp;
				if(nudFilterByPerimeterMax.Enabled && nudFilterByPerimeterMin.Value > nudFilterByPerimeterMax.Value)
					nudFilterByPerimeterMin.Value = nudFilterByPerimeterMax.Value;
			}
			else if(sender == nudFilterByPerimeterMax)
			{
				if((double)nudFilterByPerimeterMax.Value < _min_cmp)
					nudFilterByPerimeterMax.Value = (decimal)_min_cmp;
				if((double)nudFilterByPerimeterMax.Value > _max_cmp)
					nudFilterByPerimeterMax.Value = (decimal)_max_cmp;
				if(nudFilterByPerimeterMin.Enabled && nudFilterByPerimeterMin.Value > nudFilterByPerimeterMax.Value)
					nudFilterByPerimeterMax.Value = nudFilterByPerimeterMin.Value;
			}
		}


		private void btnOK_Click(object sender, System.EventArgs e)
		{
			if(CheckValues() == false)
			{
				MessageBoxEx.Error("Invalid filter values! Please re-enter!");
				return;
			}

			UpdateData(true);
		}

		private void btnSaveSettings_Click(object sender, System.EventArgs e)
		{			
			try
			{
                using (SaveFileDialog dlg = new SaveFileDialog())
                {
                    dlg.Title = "Save settings as...";
                    dlg.Filter = "Object Filter Settings (*.ofs)|*.ofs";
                    dlg.RestoreDirectory = true;
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        if (File.Exists(dlg.FileName.ToString()))
                        {
                            System.IO.FileAttributes fileAttribs = System.IO.File.GetAttributes(dlg.FileName.ToString());
                            if ((fileAttribs & System.IO.FileAttributes.ReadOnly) == System.IO.FileAttributes.ReadOnly)
                            {
                                MessageBoxEx.Error("The file is read only and can not be overridden.");
                                return;
                            }
                        }
                        String filename = dlg.FileName;

                        this.UpdateData(true);

                        _objectFilterArgumets.Serialize(filename);
                    }
                }
			}
			catch(System.Exception exp)
			{
				throw exp;
			}
		}

		private void btnLoadSettings_Click(object sender, System.EventArgs e)
		{
			using (OpenFileDialog dlg = new OpenFileDialog())
			{
                dlg.Title = "Select an object filter settings file...";
                dlg.Filter = "Object Filter Settings (*.ofs)|*.ofs";
                dlg.RestoreDirectory = true;

				if( dlg.ShowDialog(this) == DialogResult.OK)
				{
					_objectFilterArgumets.Deserialize(dlg.FileName);
					this.UpdateData(false);					
				}
			}
		}		

		#endregion

		#region internal helpers
		
		public string GetFilterExpression()
		{
			return _objectFilterArgumets.GetFilterExpression();
		}

		private void UpdateData(bool bUpdate)
		{
			if(bUpdate) // get data
			{
				//get data
				
				_objectFilterArgumets.FilterByIntegratedIntensity = chkFilterByItgInt.Checked;

				_objectFilterArgumets.IntegratedIntensity.UseLowerValue = chkFilterByItgIntMin.Checked;
				_objectFilterArgumets.IntegratedIntensity.LowerValue = (double)nudFilterByItgIntMin.Value;

				_objectFilterArgumets.IntegratedIntensity.UseUpperValue = chkFilterByItgIntMax.Checked;
				_objectFilterArgumets.IntegratedIntensity.UpperValue = (double)nudFilterByItgIntMax.Value;
				
				//filter by nubmer of pixels
				_objectFilterArgumets.FilterByNumberOfPixels = chkFilterByNumberofPixels.Checked;

				_objectFilterArgumets.NumberOfPixelFilter.UseLowerValue = chkFilterByNumberofPixelsMin.Checked;
				_objectFilterArgumets.NumberOfPixelFilter.LowerValue = (double)nudFilterByNumberofPixelsMin.Value;
				
				_objectFilterArgumets.NumberOfPixelFilter.UseUpperValue = chkFilterByNumberofPixelsMax.Checked;
				_objectFilterArgumets.NumberOfPixelFilter.UpperValue = (double)nudFilterByNumberofPixelsMax.Value;
				
				
				//filter by Area
				_objectFilterArgumets.FilterByArea = chkFilterByArea.Checked;

				_objectFilterArgumets.Area.UseLowerValue = chkFilterByAreaMin.Checked;
				_objectFilterArgumets.Area.LowerValue = (double)nudFilterByAreaMin.Value;
				
				_objectFilterArgumets.Area.UseUpperValue = chkFilterByAreaMax.Checked;
				_objectFilterArgumets.Area.UpperValue = (double)nudFilterByAreaMax.Value;
				
				//filter by perimeter
				_objectFilterArgumets.FilterByPerimeter = chkFilterByPerimeter.Checked;

				_objectFilterArgumets.Perimeter.UseLowerValue = chkFilterByPerimeterMin.Checked;
				_objectFilterArgumets.Perimeter.LowerValue = (double)nudFilterByPerimeterMin.Value;
				
				_objectFilterArgumets.Perimeter.UseUpperValue = chkFilterByPerimeterMax.Checked;
				_objectFilterArgumets.Perimeter.UpperValue = (double)nudFilterByPerimeterMax.Value;
			}
			else // set data
			{
				//update data for the controls
				
				//filter by Integrated Intensity
				chkFilterByItgInt.Checked = _objectFilterArgumets.FilterByIntegratedIntensity;


                chkFilterByItgIntMax.Checked = _objectFilterArgumets.IntegratedIntensity.UseUpperValue;
                nudFilterByItgIntMax.Value = (decimal)_objectFilterArgumets.IntegratedIntensity.UpperValue;

                
                chkFilterByItgIntMin.Checked = _objectFilterArgumets.IntegratedIntensity.UseLowerValue;
				nudFilterByItgIntMin.Value = (decimal)_objectFilterArgumets.IntegratedIntensity.LowerValue;
				

				FilterByIntegratedIntensityChanged();
				
				//filter by nubmer of pixels
				chkFilterByNumberofPixels.Checked = _objectFilterArgumets.FilterByNumberOfPixels;

                chkFilterByNumberofPixelsMax.Checked = _objectFilterArgumets.NumberOfPixelFilter.UseUpperValue;
                nudFilterByNumberofPixelsMax.Value = (decimal)_objectFilterArgumets.NumberOfPixelFilter.UpperValue;

				chkFilterByNumberofPixelsMin.Checked = _objectFilterArgumets.NumberOfPixelFilter.UseLowerValue;
				nudFilterByNumberofPixelsMin.Value = (decimal)_objectFilterArgumets.NumberOfPixelFilter.LowerValue;
							

				FilterByNumberOfPixelsChanged();
				
				
				//filter by Area
				chkFilterByArea.Checked = _objectFilterArgumets.FilterByArea;

                chkFilterByAreaMax.Checked = _objectFilterArgumets.Area.UseUpperValue;
                nudFilterByAreaMax.Value = (decimal)_objectFilterArgumets.Area.UpperValue;
                
                chkFilterByAreaMin.Checked = _objectFilterArgumets.Area.UseLowerValue;
				nudFilterByAreaMin.Value = (decimal)_objectFilterArgumets.Area.LowerValue;
								
				FilterByAreaChanged();
				
				//filter by perimeter
				chkFilterByPerimeter.Checked = _objectFilterArgumets.FilterByPerimeter;

                chkFilterByPerimeterMax.Checked = _objectFilterArgumets.Perimeter.UseUpperValue;
                nudFilterByPerimeterMax.Value = (decimal)_objectFilterArgumets.Perimeter.UpperValue;

				chkFilterByPerimeterMin.Checked = _objectFilterArgumets.Perimeter.UseLowerValue;
				nudFilterByPerimeterMin.Value = (decimal)_objectFilterArgumets.Perimeter.LowerValue;							
															
				FilterByPerimeterChanged();

				this.Update();
			}

		}

		private void UpdateGroupState(CheckBox chk1, NumericUpDown nud1, CheckBox chk2, NumericUpDown nud2, bool bState)
		{			
			if(bState)
			{
				chk1.Enabled = true;
				nud1.Enabled = chk1.Checked;
				
				chk2.Enabled = true;
				nud2.Enabled = chk2.Checked;
			}
			else
			{
				chk1.Enabled = false;
				nud1.Enabled = false;

				chk2.Enabled = false;
				nud2.Enabled = false;				
			}
		}
		
		private bool CheckValues()
		{
			double _min_cmp = 0;
			double _max_cmp = 10000000000;

			if(nudFilterByItgIntMin.Enabled)
			{				
				if((double)nudFilterByItgIntMin.Value < _min_cmp)
					return false;
				if((double)nudFilterByItgIntMin.Value > _max_cmp)
					return false;
				if(nudFilterByItgIntMax.Enabled && nudFilterByItgIntMin.Value > nudFilterByItgIntMax.Value)
					return false;
			}
			if(nudFilterByItgIntMax.Enabled)
			{
				if((double)nudFilterByItgIntMax.Value < _min_cmp)
					return false;
				if((double)nudFilterByItgIntMax.Value > _max_cmp)
					return false;
				if(nudFilterByItgIntMin.Enabled && nudFilterByItgIntMin.Value > nudFilterByItgIntMax.Value)
					return false;
			}
			if(nudFilterByNumberofPixelsMin.Enabled)
			{
				if((double)nudFilterByNumberofPixelsMin.Value < _min_cmp)
					return false;
				if((double)nudFilterByNumberofPixelsMin.Value > _max_cmp)
					return false;
				if(nudFilterByNumberofPixelsMax.Enabled && nudFilterByNumberofPixelsMin.Value > nudFilterByNumberofPixelsMax.Value)
					return false;
			}
			if(nudFilterByNumberofPixelsMax.Enabled)
			{
				if((double)nudFilterByNumberofPixelsMax.Value < _min_cmp)
					return false;
				if((double)nudFilterByNumberofPixelsMax.Value > _max_cmp)
					return false;
				if(nudFilterByNumberofPixelsMin.Enabled && nudFilterByNumberofPixelsMin.Value > nudFilterByNumberofPixelsMax.Value)
					return false;
			}
			if(nudFilterByAreaMin.Enabled)
			{
				if((double)nudFilterByAreaMin.Value < _min_cmp)
					return false;
				if((double)nudFilterByAreaMin.Value > _max_cmp)
					return false;
				if(nudFilterByAreaMax.Enabled && nudFilterByAreaMin.Value > nudFilterByAreaMax.Value)
					return false;
			}
			if(nudFilterByAreaMax.Enabled)
			{
				if((double)nudFilterByAreaMax.Value < _min_cmp)
					return false;
				if((double)nudFilterByAreaMax.Value > _max_cmp)
					return false;
				if(nudFilterByAreaMin.Enabled && nudFilterByAreaMin.Value > nudFilterByAreaMax.Value)
					return false;
			}
			if(nudFilterByPerimeterMin.Enabled)
			{
				if((double)nudFilterByPerimeterMin.Value < _min_cmp)
					return false;
				if((double)nudFilterByPerimeterMin.Value > _max_cmp)
					return false;
				if(nudFilterByPerimeterMax.Enabled && nudFilterByPerimeterMin.Value > nudFilterByPerimeterMax.Value)
					return false;
			}
			if(nudFilterByPerimeterMax.Enabled)
			{
				if((double)nudFilterByPerimeterMax.Value < _min_cmp)
					return false;
				if((double)nudFilterByPerimeterMax.Value > _max_cmp)
					return false;
				if(nudFilterByPerimeterMin.Enabled && nudFilterByPerimeterMin.Value > nudFilterByPerimeterMax.Value)
					return false;
			}
			return true;
		}
		#endregion		
	}	
}
