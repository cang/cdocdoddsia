using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using SIA.UI.Controls.Utilities;

namespace SIA.UI.Controls.Dialogs
{
	/// <summary>
	/// Summary description for DlgStressDetection.
	/// </summary>
	public class DlgStressDetection : DialogBase
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtColorDST;
		private System.Windows.Forms.Button btnColorDST;
		private System.Windows.Forms.Button btnGrayDSP;
		private System.Windows.Forms.TextBox txtGrayDSP;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Button btnProcess;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.TextBox txtKlarfFile;
		private System.Windows.Forms.Button btnKlarfFile;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public string ImageFilePath
		{
			get {return txtColorDST.Text;}
		}

		public string KlarfFilePath
		{
			get {return txtKlarfFile.Text;}
		}

		public DlgStressDetection()
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnGrayDSP = new System.Windows.Forms.Button();
			this.txtGrayDSP = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnColorDST = new System.Windows.Forms.Button();
			this.txtColorDST = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.btnKlarfFile = new System.Windows.Forms.Button();
			this.txtKlarfFile = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.btnProcess = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.btnGrayDSP);
			this.groupBox1.Controls.Add(this.txtGrayDSP);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.btnColorDST);
			this.groupBox1.Controls.Add(this.txtColorDST);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.groupBox1.Location = new System.Drawing.Point(6, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(424, 120);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Input";
			// 
			// btnGrayDSP
			// 
			this.btnGrayDSP.Location = new System.Drawing.Point(360, 88);
			this.btnGrayDSP.Name = "btnGrayDSP";
			this.btnGrayDSP.Size = new System.Drawing.Size(48, 23);
			this.btnGrayDSP.TabIndex = 7;
			this.btnGrayDSP.Text = "...";
			this.btnGrayDSP.Click += new System.EventHandler(this.btnGrayDSP_Click);
			// 
			// txtGrayDSP
			// 
			this.txtGrayDSP.AutoSize = false;
			this.txtGrayDSP.Location = new System.Drawing.Point(8, 88);
			this.txtGrayDSP.Name = "txtGrayDSP";
			this.txtGrayDSP.ReadOnly = true;
			this.txtGrayDSP.Size = new System.Drawing.Size(344, 24);
			this.txtGrayDSP.TabIndex = 6;
			this.txtGrayDSP.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 64);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(104, 23);
			this.label1.TabIndex = 5;
			this.label1.Text = "Gray DSP Image";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnColorDST
			// 
			this.btnColorDST.Location = new System.Drawing.Point(360, 40);
			this.btnColorDST.Name = "btnColorDST";
			this.btnColorDST.Size = new System.Drawing.Size(48, 23);
			this.btnColorDST.TabIndex = 4;
			this.btnColorDST.Text = "...";
			this.btnColorDST.Click += new System.EventHandler(this.btnColorDST_Click);
			// 
			// txtColorDST
			// 
			this.txtColorDST.AutoSize = false;
			this.txtColorDST.Location = new System.Drawing.Point(8, 40);
			this.txtColorDST.Name = "txtColorDST";
			this.txtColorDST.ReadOnly = true;
			this.txtColorDST.Size = new System.Drawing.Size(344, 24);
			this.txtColorDST.TabIndex = 3;
			this.txtColorDST.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(296, 23);
			this.label2.TabIndex = 2;
			this.label2.Text = "Color DST Image";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.btnKlarfFile);
			this.groupBox2.Controls.Add(this.txtKlarfFile);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.groupBox2.Location = new System.Drawing.Point(6, 120);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(424, 80);
			this.groupBox2.TabIndex = 8;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Output";
			// 
			// btnKlarfFile
			// 
			this.btnKlarfFile.Location = new System.Drawing.Point(360, 40);
			this.btnKlarfFile.Name = "btnKlarfFile";
			this.btnKlarfFile.Size = new System.Drawing.Size(48, 23);
			this.btnKlarfFile.TabIndex = 4;
			this.btnKlarfFile.Text = "...";
			this.btnKlarfFile.Click += new System.EventHandler(this.btnKlarfFile_Click);
			// 
			// txtKlarfFile
			// 
			this.txtKlarfFile.AutoSize = false;
			this.txtKlarfFile.Location = new System.Drawing.Point(8, 40);
			this.txtKlarfFile.Name = "txtKlarfFile";
			this.txtKlarfFile.ReadOnly = true;
			this.txtKlarfFile.Size = new System.Drawing.Size(344, 24);
			this.txtKlarfFile.TabIndex = 3;
			this.txtKlarfFile.Text = "";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 16);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(104, 23);
			this.label4.TabIndex = 2;
			this.label4.Text = "KLARF File";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Location = new System.Drawing.Point(0, 200);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(436, 8);
			this.groupBox3.TabIndex = 9;
			this.groupBox3.TabStop = false;
			// 
			// btnProcess
			// 
			this.btnProcess.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnProcess.Location = new System.Drawing.Point(139, 214);
			this.btnProcess.Name = "btnProcess";
			this.btnProcess.TabIndex = 10;
			this.btnProcess.Text = "OK";
			// 
			// btnClose
			// 
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(223, 214);
			this.btnClose.Name = "btnClose";
			this.btnClose.TabIndex = 12;
			this.btnClose.Text = "Cancel";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// DlgStressDetection
			// 
			this.AcceptButton = this.btnProcess;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(436, 242);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnProcess);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupBox2);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DlgStressDetection";
			this.Text = "Stress Detection";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region EventArgs Handles
		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void btnColorDST_Click(object sender, System.EventArgs e)
		{
			using (OpenFileDialog dlg = CommonDialogs.OpenImageFileDialog("Select a image file"))
			{
				if(dlg.ShowDialog() == DialogResult.OK)
				{
					txtColorDST.Text=dlg.FileName;
				}
			}
		}

		private void btnGrayDSP_Click(object sender, System.EventArgs e)
		{
			using (OpenFileDialog dlg = CommonDialogs.OpenImageFileDialog("Select a image file"))
			{
				if(dlg.ShowDialog() == DialogResult.OK)
					txtGrayDSP.Text=dlg.FileName;
			}
		}

		private void btnKlarfFile_Click(object sender, System.EventArgs e)
		{
			using (SaveFileDialog dlg = CommonDialogs.SaveKLARFFileDialog("Save output KLARF file as.."))
			{
				if(dlg.ShowDialog() == DialogResult.OK)
					txtKlarfFile.Text=dlg.FileName;
			}
		}
		#endregion
	
		protected override object OnGetDefaultValue(Control ctrl)
		{
			if (ctrl == txtKlarfFile || ctrl == txtColorDST || ctrl == txtGrayDSP)
				return "";
			
			return base.OnGetDefaultValue (ctrl);
		}

		
	}
}
