using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SIA.UI.Controls.Dialogs
{

	/// <summary>
	/// Name : DlgRotate
	/// Description : User interface for Image Rotation
	/// Thread Support : True
	/// Persistence Data : True
	/// </summary>
	public class DlgRotate : SIA.UI.Controls.Dialogs.DialogPreviewBase
	{
		#region Windows Form member
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.GroupBox grpPreview;
		private SIA.UI.Components.ImagePreview _imagePreview;
		private System.Windows.Forms.GroupBox grpParameters;
		private System.Windows.Forms.Label lblAngle;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.NumericUpDown nudAngle;
		private System.Windows.Forms.TrackBar sldAngle;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion

		#region constructor/destructor
		
		public DlgRotate(SIA.UI.Controls.ImageWorkspace owner) : base(owner, true)
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgRotate));
			this.label1 = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.grpParameters = new System.Windows.Forms.GroupBox();
			this.lblAngle = new System.Windows.Forms.Label();
			this.sldAngle = new System.Windows.Forms.TrackBar();
			this.nudAngle = new System.Windows.Forms.NumericUpDown();
			this.grpPreview = new System.Windows.Forms.GroupBox();
			this._imagePreview = new SIA.UI.Components.ImagePreview();
			this.grpParameters.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.sldAngle)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudAngle)).BeginInit();
			this.grpPreview.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(112, 27);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(47, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "(degree)";
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(288, 12);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 2;
			this.btnOK.Text = "OK";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(288, 40);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// grpParameters
			// 
			this.grpParameters.Controls.Add(this.lblAngle);
			this.grpParameters.Controls.Add(this.sldAngle);
			this.grpParameters.Controls.Add(this.nudAngle);
			this.grpParameters.Controls.Add(this.label1);
			this.grpParameters.Location = new System.Drawing.Point(8, 288);
			this.grpParameters.Name = "grpParameters";
			this.grpParameters.Size = new System.Drawing.Size(272, 84);
			this.grpParameters.TabIndex = 4;
			this.grpParameters.TabStop = false;
			this.grpParameters.Text = "Paramater";
			// 
			// lblAngle
			// 
			this.lblAngle.AutoSize = true;
			this.lblAngle.Location = new System.Drawing.Point(15, 26);
			this.lblAngle.Name = "lblAngle";
			this.lblAngle.Size = new System.Drawing.Size(39, 16);
			this.lblAngle.TabIndex = 3;
			this.lblAngle.Text = "Angle :";
			// 
			// sldAngle
			// 
			this.sldAngle.AutoSize = false;
			this.sldAngle.Location = new System.Drawing.Point(8, 52);
			this.sldAngle.Maximum = 360;
			this.sldAngle.Minimum = -360;
			this.sldAngle.Name = "sldAngle";
			this.sldAngle.Size = new System.Drawing.Size(256, 20);
			this.sldAngle.TabIndex = 2;
			this.sldAngle.TickStyle = System.Windows.Forms.TickStyle.None;
			this.sldAngle.Scroll += new System.EventHandler(this.sldAngle_Scroll);
			// 
			// nudAngle
			// 
			this.nudAngle.Location = new System.Drawing.Point(56, 24);
			this.nudAngle.Maximum = new System.Decimal(new int[] {
																	 360,
																	 0,
																	 0,
																	 0});
			this.nudAngle.Minimum = new System.Decimal(new int[] {
																	 360,
																	 0,
																	 0,
																	 -2147483648});
			this.nudAngle.Name = "nudAngle";
			this.nudAngle.Size = new System.Drawing.Size(56, 20);
			this.nudAngle.TabIndex = 1;
			this.nudAngle.Validating += new System.ComponentModel.CancelEventHandler(this.nudAngle_Validating);
			this.nudAngle.Validated += new System.EventHandler(this.nudAngle_Validated);
			this.nudAngle.ValueChanged += new System.EventHandler(this.nudAngle_ValueChanged);
			// 
			// grpPreview
			// 
			this.grpPreview.Controls.Add(this._imagePreview);
			this.grpPreview.Location = new System.Drawing.Point(8, 8);
			this.grpPreview.Name = "grpPreview";
			this.grpPreview.Size = new System.Drawing.Size(272, 280);
			this.grpPreview.TabIndex = 20;
			this.grpPreview.TabStop = false;
			this.grpPreview.Text = "Preview";
			// 
			// _imagePreview
			// 
			this._imagePreview.ImageViewer = null;
			this._imagePreview.Location = new System.Drawing.Point(8, 16);
			this._imagePreview.Name = "_imagePreview";
			this._imagePreview.PreviewRectangle = new System.Drawing.Rectangle(0, 0, 0, 0);
			this._imagePreview.Size = new System.Drawing.Size(256, 256);
			this._imagePreview.TabIndex = 0;
			// 
			// DlgRotate
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(370, 380);
			this.Controls.Add(this.grpPreview);
			this.Controls.Add(this.grpParameters);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DlgRotate";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Rotate";
			this.grpParameters.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.sldAngle)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudAngle)).EndInit();
			this.grpPreview.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region public properties

		public float Angle
		{
			get {return (float)nudAngle.Value; }
			set {nudAngle.Value = (Decimal)value;}
		}

		#endregion
	
		#region event handlers

		private void nudAngle_ValueChanged(object sender, System.EventArgs e)
		{
			sldAngle.Value =  (int)nudAngle.Value ;
			ApplyToPreview();
		}

		private void sldAngle_Scroll(object sender, System.EventArgs e)
		{
			nudAngle.Value = sldAngle.Value;			
		}

		private void nudAngle_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			sldAngle.Value =  (int)nudAngle.Value;			
		}

		private void nudAngle_Validated(object sender, System.EventArgs e)
		{
			sldAngle.Value =  (int)nudAngle.Value;
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
			//nudAngle.Enabled = false;
			//sldAngle.Enabled = false;
		}
		
		protected override void UnlockUserInputObjects()
		{
			// Notes: this function does not need lock UI objects
			//nudAngle.Enabled = true;
			//sldAngle.Enabled = true;
		}		

		public override void ApplyToCommonImage(SIA.SystemLayer.CommonImage image)
		{
			int angle = (int)this.Angle;
			image.Rotate(angle);
		}

		#endregion

		#region DialogBase override
		
		protected override object OnGetDefaultValue(System.Windows.Forms.Control ctrl)
		{
			if (ctrl == nudAngle)
				return 0.0F;
			return null;
		}

		#endregion

		#endregion
	}
}
