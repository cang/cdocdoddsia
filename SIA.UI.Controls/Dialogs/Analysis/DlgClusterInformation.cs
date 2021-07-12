using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using SIA.Common;
using SIA.Common.Analysis;

namespace SIA.UI.Controls.Dialogs
{
	/// <summary>
	/// Summary description for DlgClusterInformation.
	/// </summary>
	public class DlgClusterInformation : DialogBase
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Button OKButton;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Label txtNUMBEROFDEFECTPIXELS;
		private System.Windows.Forms.Label txtINTEGRATEDINDENSITY;
		private System.Windows.Forms.Label txtWIDTH;
		private System.Windows.Forms.Label txtHEIGHT;
		private System.Windows.Forms.Label txtBOUNDAREA;
		private System.Windows.Forms.Label txtELONGATION;
		private System.Windows.Forms.Label txtPERIMETER;
		private System.Windows.Forms.Label txtAREA;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;

		private	ClusterInfor	clusterinfor;


		public DlgClusterInformation(ClusterInfor inf)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			clusterinfor=inf;

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgClusterInformation));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.OKButton = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label12 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.txtBOUNDAREA = new System.Windows.Forms.Label();
			this.txtHEIGHT = new System.Windows.Forms.Label();
			this.txtWIDTH = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.txtINTEGRATEDINDENSITY = new System.Windows.Forms.Label();
			this.txtNUMBEROFDEFECTPIXELS = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label13 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.txtAREA = new System.Windows.Forms.Label();
			this.txtPERIMETER = new System.Windows.Forms.Label();
			this.txtELONGATION = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(33, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Width";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(16, 39);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(37, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "Height";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(16, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(110, 16);
			this.label3.TabIndex = 2;
			this.label3.Text = "Number defect pixels";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(16, 62);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(58, 16);
			this.label4.TabIndex = 3;
			this.label4.Text = "Elongation";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(16, 39);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(53, 16);
			this.label5.TabIndex = 4;
			this.label5.Text = "Perimeter";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(16, 16);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(28, 16);
			this.label6.TabIndex = 5;
			this.label6.Text = "Area";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(16, 62);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(75, 16);
			this.label7.TabIndex = 6;
			this.label7.Text = "Area of bound";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(16, 39);
			this.label8.Name = "label8";
			this.label8.TabIndex = 7;
			this.label8.Text = "Integrated Intensity";
			// 
			// OKButton
			// 
			this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OKButton.Location = new System.Drawing.Point(128, 252);
			this.OKButton.Name = "OKButton";
			this.OKButton.TabIndex = 3;
			this.OKButton.Text = "OK";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label12);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.txtBOUNDAREA);
			this.groupBox1.Controls.Add(this.txtHEIGHT);
			this.groupBox1.Controls.Add(this.txtWIDTH);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(8, 72);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(312, 88);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Bound";
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label12.Location = new System.Drawing.Point(232, 64);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(78, 16);
			this.label12.TabIndex = 12;
			this.label12.Text = "(square pixels)";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label10.Location = new System.Drawing.Point(232, 39);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(41, 16);
			this.label10.TabIndex = 11;
			this.label10.Text = "(pixels)";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label9.Location = new System.Drawing.Point(232, 16);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(41, 16);
			this.label9.TabIndex = 10;
			this.label9.Text = "(pixels)";
			// 
			// txtBOUNDAREA
			// 
			this.txtBOUNDAREA.Location = new System.Drawing.Point(120, 62);
			this.txtBOUNDAREA.Name = "txtBOUNDAREA";
			this.txtBOUNDAREA.Size = new System.Drawing.Size(120, 16);
			this.txtBOUNDAREA.TabIndex = 9;
			this.txtBOUNDAREA.Text = "Width";
			// 
			// txtHEIGHT
			// 
			this.txtHEIGHT.Location = new System.Drawing.Point(120, 39);
			this.txtHEIGHT.Name = "txtHEIGHT";
			this.txtHEIGHT.Size = new System.Drawing.Size(120, 16);
			this.txtHEIGHT.TabIndex = 8;
			this.txtHEIGHT.Text = "Width";
			// 
			// txtWIDTH
			// 
			this.txtWIDTH.Location = new System.Drawing.Point(120, 16);
			this.txtWIDTH.Name = "txtWIDTH";
			this.txtWIDTH.Size = new System.Drawing.Size(120, 16);
			this.txtWIDTH.TabIndex = 7;
			this.txtWIDTH.Text = "Width";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.txtINTEGRATEDINDENSITY);
			this.groupBox2.Controls.Add(this.txtNUMBEROFDEFECTPIXELS);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.label8);
			this.groupBox2.Location = new System.Drawing.Point(8, 8);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(312, 64);
			this.groupBox2.TabIndex = 0;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Density";
			// 
			// txtINTEGRATEDINDENSITY
			// 
			this.txtINTEGRATEDINDENSITY.Location = new System.Drawing.Point(120, 39);
			this.txtINTEGRATEDINDENSITY.Name = "txtINTEGRATEDINDENSITY";
			this.txtINTEGRATEDINDENSITY.Size = new System.Drawing.Size(120, 16);
			this.txtINTEGRATEDINDENSITY.TabIndex = 9;
			this.txtINTEGRATEDINDENSITY.Text = "label9";
			// 
			// txtNUMBEROFDEFECTPIXELS
			// 
			this.txtNUMBEROFDEFECTPIXELS.Location = new System.Drawing.Point(120, 16);
			this.txtNUMBEROFDEFECTPIXELS.Name = "txtNUMBEROFDEFECTPIXELS";
			this.txtNUMBEROFDEFECTPIXELS.Size = new System.Drawing.Size(120, 16);
			this.txtNUMBEROFDEFECTPIXELS.TabIndex = 8;
			this.txtNUMBEROFDEFECTPIXELS.Text = "label9";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.label13);
			this.groupBox3.Controls.Add(this.label11);
			this.groupBox3.Controls.Add(this.txtAREA);
			this.groupBox3.Controls.Add(this.txtPERIMETER);
			this.groupBox3.Controls.Add(this.txtELONGATION);
			this.groupBox3.Controls.Add(this.label5);
			this.groupBox3.Controls.Add(this.label6);
			this.groupBox3.Controls.Add(this.label4);
			this.groupBox3.Location = new System.Drawing.Point(8, 160);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(312, 88);
			this.groupBox3.TabIndex = 2;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Shape";
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label13.Location = new System.Drawing.Point(232, 16);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(78, 16);
			this.label13.TabIndex = 13;
			this.label13.Text = "(square pixels)";
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label11.Location = new System.Drawing.Point(232, 39);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(41, 16);
			this.label11.TabIndex = 11;
			this.label11.Text = "(pixels)";
			// 
			// txtAREA
			// 
			this.txtAREA.Location = new System.Drawing.Point(120, 16);
			this.txtAREA.Name = "txtAREA";
			this.txtAREA.Size = new System.Drawing.Size(120, 16);
			this.txtAREA.TabIndex = 8;
			this.txtAREA.Text = "Area";
			// 
			// txtPERIMETER
			// 
			this.txtPERIMETER.Location = new System.Drawing.Point(120, 39);
			this.txtPERIMETER.Name = "txtPERIMETER";
			this.txtPERIMETER.Size = new System.Drawing.Size(120, 16);
			this.txtPERIMETER.TabIndex = 7;
			this.txtPERIMETER.Text = "Area";
			// 
			// txtELONGATION
			// 
			this.txtELONGATION.Location = new System.Drawing.Point(120, 62);
			this.txtELONGATION.Name = "txtELONGATION";
			this.txtELONGATION.Size = new System.Drawing.Size(120, 16);
			this.txtELONGATION.TabIndex = 6;
			this.txtELONGATION.Text = "Area";
			// 
			// DlgClusterInformation
			// 
			this.AcceptButton = this.OKButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(330, 280);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.OKButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DlgClusterInformation";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Cluster Information";
			this.Load += new System.EventHandler(this.ClusterInformation_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void ClusterInformation_Load(object sender, System.EventArgs e)
		{
			this.Icon = new Icon(this.GetType(),"Icon.icon.ico");
			txtNUMBEROFDEFECTPIXELS.Text=clusterinfor.cluster.Count.ToString("#,##0");
			txtWIDTH.Text=clusterinfor.boundwidth.ToString("#,##0");
			txtHEIGHT.Text=clusterinfor.boundheight.ToString("#,##0");
			txtBOUNDAREA.Text=clusterinfor.boundarea.ToString("#,##0");
			txtINTEGRATEDINDENSITY.Text=clusterinfor.integratedindensity.ToString("#,##0.00");
			txtAREA.Text=clusterinfor.area.ToString("#,##0");
			txtELONGATION.Text=clusterinfor.elongation.ToString("#,##0.00");
			txtPERIMETER.Text=clusterinfor.perimeter.ToString("#,##0");
		}
	}
}
