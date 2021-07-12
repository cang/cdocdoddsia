using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using SIA.Common;
using SIA.Common.Imaging;

using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.Utilities;
using SIA.UI.Controls.Automation.Utilities;

namespace SIA.UI.Controls.Automation.Dialogs
{
	/// <summary>
	/// Summary description for DlgFileNameFormat.
	/// </summary>
	public class DlgFileNameFormat : DialogBase
	{		
		#region Internal Constants
		
		public const int KLARF_FILE = 100;
		public const int IMAGE_FILE = 200;
		public const int XML_FILE = 300;
        public const int CSV_FILE = 301;
		
		#endregion

		#region Window form members
		private System.Windows.Forms.TextBox textFileNameMask;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textFileNamePreview;
		private System.Windows.Forms.Button btnName;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnCounter;
		private System.Windows.Forms.Button btnDate;
		private System.Windows.Forms.Button btnTime;
		private System.Windows.Forms.Button btnBrowse;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion Window form members

		#region Member fields
		private int _filterType = KLARF_FILE; //unknow
		private string _fileNameFormat = string.Empty;
		#endregion Member fields

		#region Constructors and Destructors

		public DlgFileNameFormat(int filterType, string fileNameFormat)
		{
			InitializeComponent();

			_filterType = filterType;
			this.Data = fileNameFormat;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgFileNameFormat));
			this.textFileNameMask = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.textFileNamePreview = new System.Windows.Forms.TextBox();
			this.btnName = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnCounter = new System.Windows.Forms.Button();
			this.btnDate = new System.Windows.Forms.Button();
			this.btnTime = new System.Windows.Forms.Button();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// textFileNameMask
			// 
			this.textFileNameMask.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.textFileNameMask.Location = new System.Drawing.Point(96, 8);
			this.textFileNameMask.Name = "textFileNameMask";
			this.textFileNameMask.Size = new System.Drawing.Size(360, 20);
			this.textFileNameMask.TabIndex = 0;
			this.textFileNameMask.Text = "";
			this.textFileNameMask.TextChanged += new System.EventHandler(this.OnTextChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 20);
			this.label1.TabIndex = 1;
			this.label1.Text = "File name mask:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 20);
			this.label2.TabIndex = 3;
			this.label2.Text = "Preview:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textFileNamePreview
			// 
			this.textFileNamePreview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.textFileNamePreview.Location = new System.Drawing.Point(60, 36);
			this.textFileNamePreview.Name = "textFileNamePreview";
			this.textFileNamePreview.ReadOnly = true;
			this.textFileNamePreview.Size = new System.Drawing.Size(472, 20);
			this.textFileNamePreview.TabIndex = 2;
			this.textFileNamePreview.Text = "";
			// 
			// btnName
			// 
			this.btnName.Location = new System.Drawing.Point(96, 64);
			this.btnName.Name = "btnName";
			this.btnName.Size = new System.Drawing.Size(84, 24);
			this.btnName.TabIndex = 4;
			this.btnName.Text = "[N] Name";
			this.btnName.Click += new System.EventHandler(this.btnName_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(0, 92);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(538, 4);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(195, 100);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(68, 24);
			this.btnOK.TabIndex = 6;
			this.btnOK.Text = "OK";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(271, 100);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(68, 24);
			this.btnCancel.TabIndex = 7;
			this.btnCancel.Text = "Cancel";
			// 
			// btnCounter
			// 
			this.btnCounter.Location = new System.Drawing.Point(188, 64);
			this.btnCounter.Name = "btnCounter";
			this.btnCounter.Size = new System.Drawing.Size(84, 24);
			this.btnCounter.TabIndex = 9;
			this.btnCounter.Text = "[C] Counters";
			this.btnCounter.Click += new System.EventHandler(this.btnCounter_Click);
			// 
			// btnDate
			// 
			this.btnDate.Location = new System.Drawing.Point(280, 64);
			this.btnDate.Name = "btnDate";
			this.btnDate.Size = new System.Drawing.Size(84, 24);
			this.btnDate.TabIndex = 10;
			this.btnDate.Text = "[YMD] Date";
			this.btnDate.Click += new System.EventHandler(this.btnDate_Click);
			// 
			// btnTime
			// 
			this.btnTime.Location = new System.Drawing.Point(372, 64);
			this.btnTime.Name = "btnTime";
			this.btnTime.Size = new System.Drawing.Size(84, 24);
			this.btnTime.TabIndex = 11;
			this.btnTime.Text = "[hms] Time";
			this.btnTime.Click += new System.EventHandler(this.btnTime_Click);
			// 
			// btnBrowse
			// 
			this.btnBrowse.Location = new System.Drawing.Point(464, 8);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(68, 20);
			this.btnBrowse.TabIndex = 11;
			this.btnBrowse.Text = "Browse";
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// DlgFileNameFormat
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(538, 128);
			this.Controls.Add(this.btnTime);
			this.Controls.Add(this.btnDate);
			this.Controls.Add(this.btnCounter);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnName);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textFileNamePreview);
			this.Controls.Add(this.textFileNameMask);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnBrowse);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DlgFileNameFormat";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "File Name Format";
			this.ResumeLayout(false);

		}
		#endregion

		#region Properties
		public String Data
		{
			get 
			{
				return textFileNameMask.Text;
			}
			set 
			{
				textFileNameMask.Text = value;
				Preview();
			}
		}
		#endregion Properties

		#region Event Handlers		
		private void btnName_Click(object sender, System.EventArgs e)
		{
			AddPattern("[N]");
		}

		private void btnCounter_Click(object sender, System.EventArgs e)
		{
			AddPattern("[C]");
		}

		private void btnDate_Click(object sender, System.EventArgs e)
		{
			AddPattern("[YMD]");
		}

		private void btnTime_Click(object sender, System.EventArgs e)
		{
			AddPattern("[hms]");
		}

		private void btnBrowse_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.Browse();
			}
			catch (System.Exception exp)
			{
				Trace.WriteLine(exp);
			}
		}

		private void Preview()
		{
			if (textFileNameMask.Text == "")
				textFileNamePreview.Text = "";
			else
			{					
				textFileNamePreview.Text = StringParser.GetString(textFileNameMask.Text, "[FileName]", -1);
			}
		}

		private void OnTextChanged(object sender, System.EventArgs e)
		{
			Preview();
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing (e);

			if (this.DialogResult == DialogResult.OK)
			{
				// validate data
				if (this.ValidateData() == false)
					e.Cancel = true;
				else // update data when valid
					this.UpdateData(true);
			}
		}

		protected virtual bool ValidateData()
		{
			this.Preview();

			if (textFileNameMask.Text.Length == 0)
			{
				MessageBoxEx.Error("File name format is not specified!");
				return false;
			}

			if (StringParser.CheckValid(this.Data) == false || textFileNamePreview.Text.Length == 0)
			{
				MessageBoxEx.Error("File name format is incorrect! Please correct it!");
				return false;
			}
			return true;
		}

		protected virtual void UpdateData(bool bSaveAndValidate)
		{
			
		}
		#endregion Event Handlers

		#region Internal helpers
		private void AddPattern(String pattern)
		{
			textFileNameMask.Text += pattern;
			Preview();
		}

		private void Browse()
		{
			try
			{				
				string keyWord = "";
				string title = "Save File As...";
				string filter = this.GetFilterFile();
				if (filter == null)
					throw new System.Exception("Cannot browse to unknow file type.");
				int filterIndex = -1;
				string initDir = "";				
				string fileName = "Untitled";
				if (_fileNameFormat != null && _fileNameFormat != string.Empty)
				{
					initDir = Path.GetDirectoryName(_fileNameFormat);					
				}
				
				using (SaveFileDialog dlg = CommonDialogs.SaveFileDialog(keyWord, title, filter, filterIndex, initDir, fileName))
				{					
					if (DialogResult.OK == dlg.ShowDialog(this))
					{
						textFileNameMask.Text = dlg.FileName;
						fileName = dlg.FileName;
					}
				}
			}
			catch
			{
				throw;
			}
		}

		private string GetFilterFile()
		{
			string filterFile = "All Files (*.*)|*.*";
			if (_filterType == KLARF_FILE)
			{
				filterFile = "KLARF (*.000)|*.000";
			}	
			else if (_filterType == XML_FILE)
			{
				filterFile = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
			}
            else if (_filterType == CSV_FILE)
            {
                filterFile = (new CsvFileType()).Filter;
            }
            else
            {
                eImageFormat format = (eImageFormat)_filterType;
                switch (format)
                {
                    case eImageFormat.Bmp:
                        filterFile = "Bitmaps (*.bmp)|*.bmp";
                        break;
                    case eImageFormat.Jpeg:
                        filterFile = "JPEG images (*.jpg)|*.jpg";
                        break;
                    case eImageFormat.Png:
                        filterFile = "PNG images (*.png)|*.png";
                        break;
                    case eImageFormat.Tiff:
                        filterFile = "TIFF images (*.tif;*.tiff)|*.tif;*.tiff";
                        break;
                    case eImageFormat.Gif:
                        filterFile = "GIF images (*.gif)|*.gif";
                        break;
                    case eImageFormat.Raw:
                        filterFile = "Raw images (*.raw)|*.raw";
                        break;
                    case eImageFormat.Fit:
                        filterFile = "Fits images (*.fit;*.fts;*.fits)|*.fit;*.fts;*.fits";
                        break;
                    case eImageFormat.Txt:
                        filterFile = "Text Files (*.txt)|*.txt";
                        break;
                    default:
                        break;
                }
            }

			return filterFile;
		}
		#endregion Internal helpers		
	}
}
