using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SIA.UI.Controls.Dialogs
{
	/// <summary>
	/// Name : DlgManualZoom
	/// Description : User interface for Custom Zoom Operation
	/// Thread Support : None
	/// Persistence Data : False
	/// </summary>
	public class DlgManualZoom : DialogBase
	{
		#region Windows Form member attributes

		private System.Windows.Forms.GroupBox grpCustomZoom;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.RadioButton r200;
		private System.Windows.Forms.RadioButton r400;
		private System.Windows.Forms.RadioButton r800;
		private System.Windows.Forms.RadioButton rCustom;
		private System.Windows.Forms.RadioButton r100;
		private System.Windows.Forms.RadioButton r50;
		private System.Windows.Forms.RadioButton r25;
		private System.Windows.Forms.RadioButton r1600;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#endregion

		#region member attributes

		private int _zoomPercent = 50;
		
		#endregion

		#region constructor and destructor

		public DlgManualZoom()
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

		public int	Zoom
		{
			get
			{
				return _zoomPercent;
			}

			set
			{
				_zoomPercent = value;
				foreach (Control r in grpCustomZoom.Controls)
				{
					if( r.GetType()== typeof(RadioButton) )
					{
						if( ((RadioButton)r).Tag!=null )
						{
							if( int.Parse(((RadioButton)r).Tag.ToString())==_zoomPercent) 
							{
								((RadioButton)r).Checked=true;
								return;
							}
						}
					}
				}
				rCustom.Checked=true;
				
				if (numericUpDown1.Maximum < _zoomPercent)
					numericUpDown1.Maximum = _zoomPercent;
				else if (numericUpDown1.Minimum > _zoomPercent)
					numericUpDown1.Minimum = _zoomPercent;
				numericUpDown1.Value = _zoomPercent;				
			}
		}

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgManualZoom));
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.grpCustomZoom = new System.Windows.Forms.GroupBox();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.r200 = new System.Windows.Forms.RadioButton();
			this.r400 = new System.Windows.Forms.RadioButton();
			this.r800 = new System.Windows.Forms.RadioButton();
			this.rCustom = new System.Windows.Forms.RadioButton();
			this.r100 = new System.Windows.Forms.RadioButton();
			this.r50 = new System.Windows.Forms.RadioButton();
			this.r25 = new System.Windows.Forms.RadioButton();
			this.r1600 = new System.Windows.Forms.RadioButton();
			this.grpCustomZoom.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(219, 12);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 10;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.OnOK_Clicked);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(219, 40);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 11;
			this.btnCancel.Text = "Cancel";
			// 
			// grpCustomZoom
			// 
			this.grpCustomZoom.Controls.Add(this.numericUpDown1);
			this.grpCustomZoom.Controls.Add(this.label1);
			this.grpCustomZoom.Controls.Add(this.r200);
			this.grpCustomZoom.Controls.Add(this.r400);
			this.grpCustomZoom.Controls.Add(this.r800);
			this.grpCustomZoom.Controls.Add(this.rCustom);
			this.grpCustomZoom.Controls.Add(this.r100);
			this.grpCustomZoom.Controls.Add(this.r50);
			this.grpCustomZoom.Controls.Add(this.r25);
			this.grpCustomZoom.Controls.Add(this.r1600);
			this.grpCustomZoom.Location = new System.Drawing.Point(7, 6);
			this.grpCustomZoom.Name = "grpCustomZoom";
			this.grpCustomZoom.Size = new System.Drawing.Size(208, 188);
			this.grpCustomZoom.TabIndex = 12;
			this.grpCustomZoom.TabStop = false;
			this.grpCustomZoom.Text = " Custom Zoom Ratio";
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.Location = new System.Drawing.Point(116, 158);
			this.numericUpDown1.Maximum = new System.Decimal(new int[] {
																		   1600,
																		   0,
																		   0,
																		   0});
			this.numericUpDown1.Minimum = new System.Decimal(new int[] {
																		   10,
																		   0,
																		   0,
																		   0});
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(48, 20);
			this.numericUpDown1.TabIndex = 19;
			this.numericUpDown1.Value = new System.Decimal(new int[] {
																		 50,
																		 0,
																		 0,
																		 0});
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(170, 158);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(24, 20);
			this.label1.TabIndex = 18;
			this.label1.Text = "(%)";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// r200
			// 
			this.r200.Location = new System.Drawing.Point(14, 78);
			this.r200.Name = "r200";
			this.r200.Size = new System.Drawing.Size(96, 20);
			this.r200.TabIndex = 17;
			this.r200.Tag = "200";
			this.r200.Text = "200 %";
			// 
			// r400
			// 
			this.r400.Location = new System.Drawing.Point(14, 98);
			this.r400.Name = "r400";
			this.r400.Size = new System.Drawing.Size(96, 20);
			this.r400.TabIndex = 16;
			this.r400.Tag = "400";
			this.r400.Text = "400  %";
			// 
			// r800
			// 
			this.r800.Location = new System.Drawing.Point(14, 118);
			this.r800.Name = "r800";
			this.r800.Size = new System.Drawing.Size(96, 20);
			this.r800.TabIndex = 15;
			this.r800.Tag = "800";
			this.r800.Text = "800  %";
			// 
			// rCustom
			// 
			this.rCustom.Location = new System.Drawing.Point(14, 158);
			this.rCustom.Name = "rCustom";
			this.rCustom.Size = new System.Drawing.Size(96, 20);
			this.rCustom.TabIndex = 14;
			this.rCustom.Text = "Custom Zoom";
			// 
			// r100
			// 
			this.r100.Location = new System.Drawing.Point(14, 58);
			this.r100.Name = "r100";
			this.r100.Size = new System.Drawing.Size(96, 20);
			this.r100.TabIndex = 12;
			this.r100.Tag = "100";
			this.r100.Text = "100 %";
			// 
			// r50
			// 
			this.r50.Location = new System.Drawing.Point(14, 38);
			this.r50.Name = "r50";
			this.r50.Size = new System.Drawing.Size(96, 20);
			this.r50.TabIndex = 11;
			this.r50.Tag = "50";
			this.r50.Text = "50 %";
			// 
			// r25
			// 
			this.r25.Location = new System.Drawing.Point(14, 18);
			this.r25.Name = "r25";
			this.r25.Size = new System.Drawing.Size(96, 20);
			this.r25.TabIndex = 10;
			this.r25.Tag = "25";
			this.r25.Text = "20 %";
			// 
			// r1600
			// 
			this.r1600.Location = new System.Drawing.Point(14, 138);
			this.r1600.Name = "r1600";
			this.r1600.Size = new System.Drawing.Size(96, 20);
			this.r1600.TabIndex = 13;
			this.r1600.Tag = "1600";
			this.r1600.Text = "1600 %";
			// 
			// DlgManualZoom
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(298, 200);
			this.Controls.Add(this.grpCustomZoom);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DlgManualZoom";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Custom Zoom";
			this.Load += new System.EventHandler(this.ZoomCustom_Load);
			this.grpCustomZoom.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region event handlers
		private void ZoomCustom_Load(object sender, System.EventArgs e)
		{
			this.Icon = new Icon(this.GetType(),"Icon.icon.ico");
			r25.Click	+=new EventHandler(r_Click);
			r50.Click	+=new EventHandler(r_Click);
			r100.Click	+=new EventHandler(r_Click);
			r200.Click	+=new EventHandler(r_Click);
			r400.Click	+=new EventHandler(r_Click);
			r800.Click	+=new EventHandler(r_Click);
			r1600.Click	+=new EventHandler(r_Click);
			rCustom.Click += new EventHandler(r_Click);
			this.numericUpDown1.Enabled=rCustom.Checked;
		}

		private void r_Click(object sender, EventArgs e)
		{
			this.numericUpDown1.Enabled=( (RadioButton)sender).Tag ==null;
		}

		private void OnOK_Clicked(object sender, System.EventArgs e)
		{
			//if ( rCustom.Checked && !kUtils.IsInputValueValidate ( numericUpDown1 )) return;

			foreach (Control r in  grpCustomZoom.Controls)
			{
				if( r.GetType()== typeof(RadioButton) )
				{
					if(  ((RadioButton)r).Checked==true )
					{
						if( ((RadioButton)r).Tag==null )
							_zoomPercent=(int)this.numericUpDown1.Value;
						else
							_zoomPercent=int.Parse( ((RadioButton)r).Tag.ToString() );
						return;
					}
				}
			}		
			//this.DialogResult = DialogResult.OK ;
		}
		#endregion
	}
}
