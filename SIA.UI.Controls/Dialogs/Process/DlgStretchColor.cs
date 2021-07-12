using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using SIA.UI.Controls;
using SIA.SystemLayer;

using SIA.UI.Components;
using SIA.UI.Controls.Utilities;

namespace SIA.UI.Controls.Dialogs
{
	/// <summary>
	/// Name : DlgStretchColor
	/// Description : User interface for Stretch Color
	/// Thread Support : True
	/// Persistence Data : True
	/// </summary>
	public class DlgStretchColor : SIA.UI.Controls.Dialogs.DialogPreviewBase
	{
		#region Windows Form members
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Button button_Cancel;
		private System.Windows.Forms.Button button_OK;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.NumericUpDown numericUpDownMin;
		private System.Windows.Forms.NumericUpDown numericUpDownMax;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnReset;
		private SIA.UI.Components.ImagePreview _imagePreview;
		private SIA.UI.Controls.UserControls.kSlider slideParams;
		private System.ComponentModel.IContainer components;
		#endregion

		#region constructor/destructor

		public DlgStretchColor(SIA.UI.Controls.ImageWorkspace owner) : base(owner, true)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
		
		public int Min
		{
			get { return (int)numericUpDownMin.Value;}
		}

		public int Max
		{
			get { return (int)numericUpDownMax.Value;}
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgStretchColor));
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.button_Cancel = new System.Windows.Forms.Button();
			this.button_OK = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.numericUpDownMin = new System.Windows.Forms.NumericUpDown();
			this.numericUpDownMax = new System.Windows.Forms.NumericUpDown();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.slideParams = new SIA.UI.Controls.UserControls.kSlider();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this._imagePreview = new SIA.UI.Components.ImagePreview();
			this.btnReset = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownMin)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownMax)).BeginInit();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// button_Cancel
			// 
			this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button_Cancel.Location = new System.Drawing.Point(288, 40);
			this.button_Cancel.Name = "button_Cancel";
			this.button_Cancel.TabIndex = 7;
			this.button_Cancel.Text = "Cancel";
			// 
			// button_OK
			// 
			this.button_OK.Location = new System.Drawing.Point(288, 12);
			this.button_OK.Name = "button_OK";
			this.button_OK.TabIndex = 6;
			this.button_OK.Text = "OK";
			this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(52, 44);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(32, 20);
			this.label4.TabIndex = 24;
			this.label4.Text = "Min:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// numericUpDownMin
			// 
			this.numericUpDownMin.Location = new System.Drawing.Point(90, 44);
			this.numericUpDownMin.Maximum = new System.Decimal(new int[] {
																			 255,
																			 0,
																			 0,
																			 0});
			this.numericUpDownMin.Name = "numericUpDownMin";
			this.numericUpDownMin.Size = new System.Drawing.Size(64, 20);
			this.numericUpDownMin.TabIndex = 25;
			this.numericUpDownMin.ValueChanged += new System.EventHandler(this.numericUpDownMin_ValueChanged);
			// 
			// numericUpDownMax
			// 
			this.numericUpDownMax.Location = new System.Drawing.Point(90, 72);
			this.numericUpDownMax.Maximum = new System.Decimal(new int[] {
																			 255,
																			 0,
																			 0,
																			 0});
			this.numericUpDownMax.Name = "numericUpDownMax";
			this.numericUpDownMax.Size = new System.Drawing.Size(64, 20);
			this.numericUpDownMax.TabIndex = 27;
			this.numericUpDownMax.ValueChanged += new System.EventHandler(this.numericUpDownMax_ValueChanged);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(52, 72);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(32, 20);
			this.label5.TabIndex = 26;
			this.label5.Text = "Max:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label6.Location = new System.Drawing.Point(154, 48);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(66, 20);
			this.label6.TabIndex = 30;
			this.label6.Text = "(Intensity)";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label7.Location = new System.Drawing.Point(154, 72);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(66, 20);
			this.label7.TabIndex = 31;
			this.label7.Text = "(Intensity)";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.numericUpDownMin);
			this.groupBox2.Controls.Add(this.numericUpDownMax);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.slideParams);
			this.groupBox2.Location = new System.Drawing.Point(8, 312);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(272, 96);
			this.groupBox2.TabIndex = 36;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Parameters";
			// 
			// slideParams
			// 
			this.slideParams.Location = new System.Drawing.Point(8, 22);
			this.slideParams.Lock = false;
			this.slideParams.MaxValue = 100;
			this.slideParams.MinValue = 0;
			this.slideParams.Name = "slideParams";
			this.slideParams.ShowTrack = true;
			this.slideParams.Size = new System.Drawing.Size(256, 20);
			this.slideParams.TabIndex = 40;
			this.slideParams.LeftValueChanged += new SIA.UI.Controls.UserControls.kSlider.ValueChangedEventHandler(this.OnMinValue_Changed);
			this.slideParams.RightValueChanged += new SIA.UI.Controls.UserControls.kSlider.ValueChangedEventHandler(this.OnMaxValue_Changed);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this._imagePreview);
			this.groupBox1.Controls.Add(this.btnReset);
			this.groupBox1.Location = new System.Drawing.Point(7, 7);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(272, 304);
			this.groupBox1.TabIndex = 37;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Preview";
			// 
			// _imagePreview
			// 
			this._imagePreview.ImageViewer = null;
			this._imagePreview.Location = new System.Drawing.Point(4, 16);
			this._imagePreview.Name = "_imagePreview";
			this._imagePreview.PreviewRectangle = new System.Drawing.Rectangle(0, 0, 0, 0);
			this._imagePreview.Size = new System.Drawing.Size(256, 256);
			this._imagePreview.TabIndex = 39;
			// 
			// btnReset
			// 
			this.btnReset.Location = new System.Drawing.Point(8, 276);
			this.btnReset.Name = "btnReset";
			this.btnReset.Size = new System.Drawing.Size(60, 23);
			this.btnReset.TabIndex = 38;
			this.btnReset.Text = "Reset";
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// DlgStretchColor
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(370, 412);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.button_Cancel);
			this.Controls.Add(this.button_OK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Location = new System.Drawing.Point(10, 50);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DlgStretchColor";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Stretch Color";
			this.TopMost = true;
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownMin)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownMax)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion		

		#region event handler
		
		private void OnMinValue_Changed(object sender, int val)
		{
			if (numericUpDownMin.Minimum<=val && val<=numericUpDownMin.Maximum)
				numericUpDownMin.Value = (Decimal)val;						
		}

		private void OnMaxValue_Changed(object sender, int val)
		{
			if (numericUpDownMax.Minimum<=val && val<=numericUpDownMax.Maximum)
				numericUpDownMax.Value = (Decimal)val;
		}

		private void numericUpDownMin_ValueChanged(object sender, System.EventArgs e)
		{
			try
			{
				slideParams.SetLeftThumbPos(int.Parse(numericUpDownMin.Value.ToString()));
				numericUpDownMax.Minimum = numericUpDownMin.Value;

				OnMinMaxChanged(int.Parse(numericUpDownMin.Value.ToString()),int.Parse(numericUpDownMax.Value.ToString()));

				this.ApplyToPreview();
			}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);
			}			
		}

		private void numericUpDownMax_ValueChanged(object sender, System.EventArgs e)
		{
			try
			{
				slideParams.SetRightThumbPos(int.Parse(numericUpDownMax.Value.ToString()));
				numericUpDownMin.Maximum = numericUpDownMax.Value;
				OnMinMaxChanged(int.Parse(numericUpDownMin.Value.ToString()),int.Parse(numericUpDownMax.Value.ToString()));					

				this.ApplyToPreview();
			}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);
			}			
		}

		private void btnReset_Click(object sender, System.EventArgs e)
		{
			this.ResetPreview();
		}

		private void button_OK_Click(object sender, System.EventArgs e)
		{
			if ( !kUtils.IsAllValueValidate( groupBox2 ) ) return;			
			this.DialogResult = DialogResult.OK;
		}
		#endregion

		#region override routines

		#region DialogPreviewBase override
		
		public override SIA.UI.Components.ImagePreview GetPreviewer()
		{
			return this._imagePreview;
		}

		protected override void LockUserInputObjects()
		{
			// Notes: this function does not need lock UI objects
			//btnReset.Enabled = false;
			//grpOperators.Enabled = false;
			//grpOperand.Enabled = false;
		}
		
		protected override void UnlockUserInputObjects()
		{
			// Notes: this function does not need lock UI objects
			//btnReset.Enabled = true;
			//grpOperators.Enabled = true;
			//grpOperand.Enabled = true;
		}		

		public override void ApplyToCommonImage(SIA.SystemLayer.CommonImage image)
		{
			if (image == null)
				throw new System.ArgumentNullException("Invalid parameter");
			image.StretchColor((int)numericUpDownMin.Value,(int)numericUpDownMax.Value);
		}

		#endregion

		#region DialogBase override
		
		protected override object OnGetDefaultValue(System.Windows.Forms.Control ctrl)
		{
			if (ctrl == numericUpDownMin)
				return (Decimal)0x0;
			else if (ctrl == numericUpDownMax)
				return (Decimal)0xFF;
			return null;
		}

		#endregion

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			kUtils.SetMinMax( numericUpDownMin, _imagePreview.ImageViewer.Image );
			kUtils.SetMinMax( numericUpDownMax, _imagePreview.ImageViewer.Image );

			numericUpDownMax.Value = numericUpDownMax.Maximum;
			numericUpDownMin.Value = numericUpDownMin.Minimum;

			slideParams.MaxValue = (int)numericUpDownMax.Maximum;
			slideParams.MinValue = (int)numericUpDownMin.Minimum;

			slideParams.SetLeftThumbPos(slideParams.MinValue);
			slideParams.SetRightThumbPos(slideParams.MaxValue);
		}

		#endregion

		#region internal helpers

		protected virtual void OnMinMaxChanged(int min, int max)
		{
		}

		#endregion
	}
}
