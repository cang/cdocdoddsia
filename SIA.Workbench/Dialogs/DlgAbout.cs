using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;

using SIA.Common;
using SIA.UI.Controls.Dialogs;
using Microsoft.Win32;

namespace SIA.Workbench
{
	/// <summary>
	/// Summary description for DlgAbout.
	/// </summary>
	internal class DlgAbout : DialogBase
	{
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.TextBox txtProductName;
		private System.Windows.Forms.TextBox txtVersion;
		private System.Windows.Forms.TextBox txtBuildDate;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label lblProductID;
		private System.Windows.Forms.TextBox txtLicenseOwner;
		private System.Windows.Forms.TextBox txtProductionID;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public DlgAbout()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgAbout));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtBuildDate = new System.Windows.Forms.TextBox();
            this.txtVersion = new System.Windows.Forms.TextBox();
            this.txtProductName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtLicenseOwner = new System.Windows.Forms.TextBox();
            this.lblProductID = new System.Windows.Forms.Label();
            this.txtProductionID = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtBuildDate);
            this.groupBox1.Controls.Add(this.txtVersion);
            this.groupBox1.Controls.Add(this.txtProductName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(8, 99);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(316, 81);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // txtBuildDate
            // 
            this.txtBuildDate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtBuildDate.Location = new System.Drawing.Point(88, 60);
            this.txtBuildDate.Name = "txtBuildDate";
            this.txtBuildDate.ReadOnly = true;
            this.txtBuildDate.Size = new System.Drawing.Size(220, 14);
            this.txtBuildDate.TabIndex = 4;
            this.txtBuildDate.Text = "Build Date:";
            // 
            // txtVersion
            // 
            this.txtVersion.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtVersion.Location = new System.Drawing.Point(88, 38);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.ReadOnly = true;
            this.txtVersion.Size = new System.Drawing.Size(220, 14);
            this.txtVersion.TabIndex = 3;
            this.txtVersion.Text = "Version:";
            // 
            // txtProductName
            // 
            this.txtProductName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtProductName.Location = new System.Drawing.Point(88, 16);
            this.txtProductName.Name = "txtProductName";
            this.txtProductName.ReadOnly = true;
            this.txtProductName.Size = new System.Drawing.Size(220, 14);
            this.txtProductName.TabIndex = 2;
            this.txtProductName.Text = "Production Name";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(4, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 14);
            this.label1.TabIndex = 2;
            this.label1.Text = "Product Name:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(4, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "Version:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(4, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 14);
            this.label4.TabIndex = 2;
            this.label4.Text = "Build Date:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(7, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 18);
            this.label3.TabIndex = 2;
            this.label3.Text = "Product Details:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtLicenseOwner);
            this.groupBox2.Controls.Add(this.lblProductID);
            this.groupBox2.Controls.Add(this.txtProductionID);
            this.groupBox2.Location = new System.Drawing.Point(8, 224);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(316, 60);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            // 
            // txtLicenseOwner
            // 
            this.txtLicenseOwner.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLicenseOwner.Location = new System.Drawing.Point(8, 16);
            this.txtLicenseOwner.Name = "txtLicenseOwner";
            this.txtLicenseOwner.ReadOnly = true;
            this.txtLicenseOwner.Size = new System.Drawing.Size(300, 14);
            this.txtLicenseOwner.TabIndex = 5;
            this.txtLicenseOwner.Text = "License Owner";
            // 
            // lblProductID
            // 
            this.lblProductID.AutoSize = true;
            this.lblProductID.Location = new System.Drawing.Point(4, 36);
            this.lblProductID.Name = "lblProductID";
            this.lblProductID.Size = new System.Drawing.Size(59, 13);
            this.lblProductID.TabIndex = 3;
            this.lblProductID.Text = "ProductID:";
            this.lblProductID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtProductionID
            // 
            this.txtProductionID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProductionID.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtProductionID.Location = new System.Drawing.Point(68, 38);
            this.txtProductionID.Name = "txtProductionID";
            this.txtProductionID.ReadOnly = true;
            this.txtProductionID.Size = new System.Drawing.Size(240, 14);
            this.txtProductionID.TabIndex = 5;
            this.txtProductionID.Text = "Production Name";
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(8, 208);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(168, 16);
            this.label6.TabIndex = 4;
            this.label6.Text = "This product is licensed to:";
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(8, 184);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(312, 16);
            this.label7.TabIndex = 5;
            this.label7.Text = "Copyright © 2011 SiGlaz.  All rights reserved.";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(129, 388);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "OK";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Location = new System.Drawing.Point(8, 284);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(316, 96);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(4, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(308, 76);
            this.label5.TabIndex = 0;
            this.label5.Text = resources.GetString("label5.Text");
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Image = global::SIA.Workbench.Properties.Resources.about_SIA;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(332, 72);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // DlgAbout
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(332, 420);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DlgAbout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About SiGlaz Image Analyzer Automation";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			try
			{
                //RijndaelCrypto crypto = new RijndaelCrypto();
                //string licensedTo = SIA.Common.Utility.CustomConfiguration.GetValues("LICENSETO","").ToString();
                //string productID = SIA.Common.Utility.CustomConfiguration.GetValues("SIGLAZPRODUCTID","").ToString();

                //if( licensedTo == "" )
                //    licensedTo = "Evaluation";
                //else
                //    licensedTo = crypto.Decrypt(licensedTo);

                //if (productID == "")
                //    productID = GenProductID();
                //else
                //    productID = crypto.Decrypt(productID);

                //string regString = string.Format("SOFTWARE\\{0}\\{1}", Application.CompanyName, Application.ProductName);

                string regString = string.Format("SOFTWARE\\{0}\\{1}", "SiGlaz", "SiGlaz Image Analyzer");

                RegistryKey regItem = Registry.CurrentUser.OpenSubKey(regString, true);
                if (regItem == null)
                    regItem = Registry.CurrentUser.CreateSubKey(regString);

                string licensedTo = (string)regItem.GetValue("LicenseTo", "Evaluation");
                string productID = (string)regItem.GetValue("ProductID", GenProductID());

                regItem.Close();

				// update license information
				txtLicenseOwner.Text = licensedTo;
				txtProductionID.Text = productID;

				// update production information
				txtProductName.Text = Application.ProductName;
				txtVersion.Text = Application.ProductVersion;
                //txtVersion.Text = "1.0 Beta";
				
				// update release date information
				DateTime releaseDate = this.GetReleaseDate();
				txtBuildDate.Text = releaseDate.ToString("yyyy/MM/dd");
			}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);
			}
		}

		private static string GenProductID()
		{
			string sbase="0123456789ABCDEFGHIJKLMNOPQRSTUVXYZ";//abcdefghijklmnopqrstuvxyz";
			//8-4-4-4-12
			Random rnd=new Random();

			string sgen="";
			for(int i=0;i<8;i++)
				sgen+= sbase[rnd.Next(sbase.Length)];
			sgen+="-";
			for(int i=0;i<4;i++)
				sgen+= sbase[rnd.Next(sbase.Length)];
			sgen+="-";
			for(int i=0;i<4;i++)
				sgen+= sbase[rnd.Next(sbase.Length)];
			sgen+="-";
			for(int i=0;i<4;i++)
				sgen+= sbase[rnd.Next(sbase.Length)];
			sgen+="-";
			for(int i=0;i<12;i++)
				sgen+= sbase[rnd.Next(sbase.Length)];

			return sgen;
		}

		private DateTime GetReleaseDate()
		{
			Assembly assembly = Assembly.GetEntryAssembly();
			object[] attribs = assembly.GetCustomAttributes(typeof(AssemblyReleaseDateAttribute), false);
			foreach (Attribute attrib in attribs)
			{
				AssemblyReleaseDateAttribute attReleaseDate = attrib as AssemblyReleaseDateAttribute;
				if (attReleaseDate == null)
					continue;
				return attReleaseDate.ReleaseDate;
			}

			string filePath = Application.StartupPath;
			FileInfo fileInfo = new FileInfo(filePath);
			return fileInfo.LastWriteTime;
		}
	}
}
