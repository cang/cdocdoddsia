using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using SIA.UI.Controls;
using SIA.UI.Controls.Common;
using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.Utilities;
using SIA.UI.Controls.Automation.Commands;

namespace SIA.UI.Controls.Automation.Dialogs
{
	/// <summary>
	/// Summary description for DlgInputFileFilter.
	/// </summary>
	public class DlgInputFileFilter : DialogBase
	{
		#region Window member fields
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.CheckBox chkFileNameFilter;
		private System.Windows.Forms.CheckBox chkFileTypeFilter;
		private System.Windows.Forms.TextBox txtFileNameFilter;
		private System.Windows.Forms.ComboBox cmbFileTypeFilter;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox checkBox7;
		private System.Windows.Forms.CheckBox checkBox9;
		private System.Windows.Forms.CheckBox chkBmp;
		private System.Windows.Forms.CheckBox chkJPEG;
		private System.Windows.Forms.CheckBox chkPNG;
		private System.Windows.Forms.CheckBox chkFIT;
		private System.Windows.Forms.CheckBox chkGIF;
		private System.Windows.Forms.CheckBox chkTIFF;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion Window member fields
		
		
		#region Member fields
		private InputFileFilter _settings = null;
		#endregion Member fields

		#region Properties
		public InputFileFilter Settings
		{
			get { return _settings; }
		}

		public bool FilterFileName
		{
			get { return chkFileNameFilter.Checked; }
			set 
			{ 
				chkFileNameFilter.Checked = value; 
				this.OnFilterFileName_Changed();
			}
		}

		public bool FilterFileType
		{
			get { return chkFileTypeFilter.Checked; }
			set 
			{ 
				chkFileTypeFilter.Checked = value; 
				this.OnFilterFileType_Changed();
			}
		}
		#endregion Properties

		#region Constructors and Destructors
		public DlgInputFileFilter()
		{
			InitializeComponent();

			this.InitClass();
		}

		public DlgInputFileFilter(InputFileFilter settings)
		{
			InitializeComponent();

			this.InitClass();

			_settings = settings;	
		
			this.UpdateData(true);
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
		#endregion Constructors and Destructors

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgInputFileFilter));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.chkFileNameFilter = new System.Windows.Forms.CheckBox();
			this.chkFileTypeFilter = new System.Windows.Forms.CheckBox();
			this.txtFileNameFilter = new System.Windows.Forms.TextBox();
			this.cmbFileTypeFilter = new System.Windows.Forms.ComboBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.chkBmp = new System.Windows.Forms.CheckBox();
			this.chkJPEG = new System.Windows.Forms.CheckBox();
			this.chkPNG = new System.Windows.Forms.CheckBox();
			this.chkFIT = new System.Windows.Forms.CheckBox();
			this.chkGIF = new System.Windows.Forms.CheckBox();
			this.chkTIFF = new System.Windows.Forms.CheckBox();
			this.checkBox7 = new System.Windows.Forms.CheckBox();
			this.checkBox9 = new System.Windows.Forms.CheckBox();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(-4, 220);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(256, 4);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(50, 232);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(64, 24);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "OK";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(122, 232);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(64, 24);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			// 
			// chkFileNameFilter
			// 
			this.chkFileNameFilter.Location = new System.Drawing.Point(8, 8);
			this.chkFileNameFilter.Name = "chkFileNameFilter";
			this.chkFileNameFilter.Size = new System.Drawing.Size(104, 20);
			this.chkFileNameFilter.TabIndex = 3;
			this.chkFileNameFilter.Text = "File Name Filter";
			this.chkFileNameFilter.CheckedChanged += new System.EventHandler(this.chkFileNameFilter_CheckedChanged);
			// 
			// chkFileTypeFilter
			// 
			this.chkFileTypeFilter.Location = new System.Drawing.Point(8, 36);
			this.chkFileTypeFilter.Name = "chkFileTypeFilter";
			this.chkFileTypeFilter.Size = new System.Drawing.Size(100, 20);
			this.chkFileTypeFilter.TabIndex = 4;
			this.chkFileTypeFilter.Text = "File Type Filter";
			this.chkFileTypeFilter.CheckedChanged += new System.EventHandler(this.chkFileTypeFilter_CheckedChanged);
			// 
			// txtFileNameFilter
			// 
			this.txtFileNameFilter.Location = new System.Drawing.Point(116, 8);
			this.txtFileNameFilter.Name = "txtFileNameFilter";
			this.txtFileNameFilter.Size = new System.Drawing.Size(112, 20);
			this.txtFileNameFilter.TabIndex = 5;
			this.txtFileNameFilter.Text = "";
			// 
			// cmbFileTypeFilter
			// 
			this.cmbFileTypeFilter.Location = new System.Drawing.Point(564, 132);
			this.cmbFileTypeFilter.Name = "cmbFileTypeFilter";
			this.cmbFileTypeFilter.Size = new System.Drawing.Size(184, 21);
			this.cmbFileTypeFilter.TabIndex = 6;
			this.cmbFileTypeFilter.Text = "comboBox1";
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.groupBox2.Controls.Add(this.chkFIT);
			this.groupBox2.Controls.Add(this.chkGIF);
			this.groupBox2.Controls.Add(this.chkTIFF);
			this.groupBox2.Controls.Add(this.chkPNG);
			this.groupBox2.Controls.Add(this.chkJPEG);
			this.groupBox2.Controls.Add(this.chkBmp);
			this.groupBox2.Location = new System.Drawing.Point(10, 40);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(218, 172);
			this.groupBox2.TabIndex = 7;
			this.groupBox2.TabStop = false;
			// 
			// chkBmp
			// 
			this.chkBmp.Location = new System.Drawing.Point(56, 24);
			this.chkBmp.Name = "chkBmp";
			this.chkBmp.Size = new System.Drawing.Size(156, 20);
			this.chkBmp.TabIndex = 0;
			this.chkBmp.Text = "Bitmaps (*.bmp)";
			// 
			// chkJPEG
			// 
			this.chkJPEG.Location = new System.Drawing.Point(56, 48);
			this.chkJPEG.Name = "chkJPEG";
			this.chkJPEG.Size = new System.Drawing.Size(156, 20);
			this.chkJPEG.TabIndex = 1;
			this.chkJPEG.Text = "JPEG images (*.jpg)";
			// 
			// chkPNG
			// 
			this.chkPNG.Location = new System.Drawing.Point(56, 72);
			this.chkPNG.Name = "chkPNG";
			this.chkPNG.Size = new System.Drawing.Size(156, 20);
			this.chkPNG.TabIndex = 2;
			this.chkPNG.Text = "PNG images (*.png)";
			// 
			// chkFIT
			// 
			this.chkFIT.Location = new System.Drawing.Point(56, 144);
			this.chkFIT.Name = "chkFIT";
			this.chkFIT.Size = new System.Drawing.Size(156, 20);
			this.chkFIT.TabIndex = 5;
			this.chkFIT.Text = "Fits images (*.fit;*.fts;*.fits)";
			// 
			// chkGIF
			// 
			this.chkGIF.Location = new System.Drawing.Point(56, 120);
			this.chkGIF.Name = "chkGIF";
			this.chkGIF.Size = new System.Drawing.Size(156, 20);
			this.chkGIF.TabIndex = 4;
			this.chkGIF.Text = "GIF images (*.gif)";
			// 
			// chkTIFF
			// 
			this.chkTIFF.Location = new System.Drawing.Point(56, 96);
			this.chkTIFF.Name = "chkTIFF";
			this.chkTIFF.Size = new System.Drawing.Size(156, 20);
			this.chkTIFF.TabIndex = 3;
			this.chkTIFF.Text = "TIFF images (*.tif;*.tiff)";
			// 
			// checkBox7
			// 
			this.checkBox7.Location = new System.Drawing.Point(704, 200);
			this.checkBox7.Name = "checkBox7";
			this.checkBox7.Size = new System.Drawing.Size(100, 20);
			this.checkBox7.TabIndex = 8;
			this.checkBox7.Text = "All images (*.*)";
			// 
			// checkBox9
			// 
			this.checkBox9.Location = new System.Drawing.Point(704, 176);
			this.checkBox9.Name = "checkBox9";
			this.checkBox9.Size = new System.Drawing.Size(284, 20);
			this.checkBox9.TabIndex = 6;
			this.checkBox9.Text = "Common images (*.bmp, *.jpg, *.png, *.tif, *.tiff, *.gif)";
			// 
			// DlgInputFileFilter
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(236, 261);
			this.Controls.Add(this.chkFileTypeFilter);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.cmbFileTypeFilter);
			this.Controls.Add(this.txtFileNameFilter);
			this.Controls.Add(this.chkFileNameFilter);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.checkBox7);
			this.Controls.Add(this.checkBox9);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DlgInputFileFilter";
			this.ShowInTaskbar = false;
			this.Text = "Input File Filter";
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Override methods
		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing (e);

			if (this.DialogResult == DialogResult.OK)
			{
				// validate data
				if (this.ValidateData() == false)
					e.Cancel = true;
				else // update data when valid
					this.UpdateData(false);
			}
		}

		protected virtual bool ValidateData()
		{
			if (this.FilterFileName && this.txtFileNameFilter.Text == string.Empty)
			{
				MessageBoxEx.Error("File Name Filter was not specified. Please try again.");
				return false;
			}

			if (this.FilterFileType)
			{
				if(this.chkBmp.Checked)
				{
					return true;
				}
				if(this.chkJPEG.Checked)
				{
					return true;
				}
				if(this.chkPNG.Checked)
				{
					return true;
				}
				if(this.chkTIFF.Checked)
				{
					return true;
				}
				if(this.chkGIF.Checked)
				{
					return true;
				}
				if(this.chkFIT.Checked)
				{
					return true;
				}

				MessageBoxEx.Error("File Type Filter was not specified. Please try again.");
				return false;
			}

			return true;
		}
		#endregion Override methods

		#region Event handlers
		private void OnFilterFileName_Changed()
		{
			txtFileNameFilter.Enabled = chkFileNameFilter.Checked;
		}

		private void OnFilterFileType_Changed()
		{
			this.UpdateGroupImageType();
		}

		private void chkFileNameFilter_CheckedChanged(object sender, System.EventArgs e)
		{
			OnFilterFileName_Changed();
		}

		private void chkFileTypeFilter_CheckedChanged(object sender, System.EventArgs e)
		{
			OnFilterFileType_Changed();
		}
		#endregion Event handlers

		#region Internal helpers
		private void UpdateData(bool bLoadToControl)
		{
			if (bLoadToControl)
			{
				this.txtFileNameFilter.Text = _settings.FileNameFormat;
				this.FilterFileName = _settings.FilterInputFileName;

				this.chkBmp.Checked = _settings.Bmp;
				this.chkJPEG.Checked = _settings.Jpeg;
				this.chkPNG.Checked = _settings.Png;
				this.chkTIFF.Checked = _settings.Tiff;
				this.chkGIF.Checked = _settings.Gif;
				this.chkFIT.Checked = _settings.Fit;
				this.FilterFileType = _settings.FilterInputFileFormat;
				this.Invalidate(true);
			}
			else
			{
				this.Invalidate(true);
				_settings.FileNameFormat = this.txtFileNameFilter.Text;
				_settings.FilterInputFileName = this.FilterFileName;				

				_settings.Bmp = this.chkBmp.Checked;
				_settings.Jpeg = this.chkJPEG.Checked;
				_settings.Png = this.chkPNG.Checked;
				_settings.Tiff = this.chkTIFF.Checked;
				_settings.Gif = this.chkGIF.Checked;
				_settings.Fit = this.chkFIT.Checked;
				_settings.FilterInputFileFormat = this.FilterFileType;
			}
		}

		private void InitClass()
		{
		}

		private void UpdateGroupImageType()
		{
			chkBmp.Enabled = chkFileTypeFilter.Checked;
			chkJPEG.Enabled = chkFileTypeFilter.Checked;
			chkPNG.Enabled = chkFileTypeFilter.Checked;
			chkFIT.Enabled = chkFileTypeFilter.Checked;
			chkGIF.Enabled = chkFileTypeFilter.Checked;
			chkTIFF.Enabled = chkFileTypeFilter.Checked;
		}
		#endregion Internal helpers		
	}
}
