using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using SIA.Common;
using SIA.Common.Imaging;

using SIA.UI.Controls.Automation;
using SIA.UI.Controls.Automation.Commands;
using SIA.UI.Controls.Automation.Utilities;

using SIA.UI.Controls.Common;
using SIA.UI.Controls.Utilities;
using SIA.UI.Controls.Dialogs;

namespace SIA.UI.Controls.Automation.Dialogs
{
	/// <summary>
	/// Summary description for DlgSaveImageStepSettings.
	/// </summary>
	public class DlgSaveImageStepSettings : DialogBase
	{
		#region Members
		public ArrayList alFiterFileName = new ArrayList();
		public  string strDefineFileName  = "";
		public bool bPrefix = true;		
		public bool bDefineText = false;
		public string Filename ;
		public bool m_HaveUniqueCode=true;

		private string _changeDate = string.Empty;
		private DateTime _changeTime = DateTime.Now;

		private SaveImageCommandSettings _settings = null;

		#endregion

		#region Window Form Members

		private System.Windows.Forms.Button btnDefine;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.ComboBox cbFileType;

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtExample;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#endregion Window Form Members

		#region Constructor and destructor

		public DlgSaveImageStepSettings()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			// initialize combo box data source
			this.InitializeImageFormatComboBox();
		}

		public DlgSaveImageStepSettings(SaveImageCommandSettings settings)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// initialize combo box data source
			this.InitializeImageFormatComboBox();
	
			// initial settings
			this._settings = settings;			
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
		
		#region Properties

		public SaveImageCommandSettings Settings
		{
			get 
			{
				return _settings;
			}

			set 
			{
				_settings = value;
				OnSettingsChanged();
			}
		}

		protected virtual void OnSettingsChanged()
		{
			this.UpdateData(false);
		}

		#endregion Properties

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgSaveImageStepSettings));
			this.btnDefine = new System.Windows.Forms.Button();
			this.cbFileType = new System.Windows.Forms.ComboBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtExample = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// btnDefine
			// 
			this.btnDefine.Location = new System.Drawing.Point(304, 40);
			this.btnDefine.Name = "btnDefine";
			this.btnDefine.Size = new System.Drawing.Size(75, 20);
			this.btnDefine.TabIndex = 1;
			this.btnDefine.Text = "&Define";
			this.btnDefine.Click += new System.EventHandler(this.btnDefine_Click);
			// 
			// cbFileType
			// 
			this.cbFileType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbFileType.Items.AddRange(new object[] {
									    "Bitmaps (*.bmp)",
									    "JPEG images (*.jpg)",
									    "PNG images (*.png)",
									    "TIF images (*.tif)",
									    "GIF images (*.gif)",
									    "Fits images (*.fit, *.fts, *.fits)"});
			this.cbFileType.Location = new System.Drawing.Point(132, 68);
			this.cbFileType.Name = "cbFileType";
			this.cbFileType.Size = new System.Drawing.Size(248, 21);
			this.cbFileType.TabIndex = 2;
			this.cbFileType.SelectedValueChanged += new System.EventHandler(this.cbFileType_SelectedValueChanged);
			// 
			// groupBox3
			// 
			this.groupBox3.Location = new System.Drawing.Point(-4, 96);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(392, 4);
			this.groupBox3.TabIndex = 2;
			this.groupBox3.TabStop = false;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(200, 104);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "&Cancel";
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(112, 104);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 4;
			this.btnOK.Text = "&OK";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 68);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(120, 24);
			this.label1.TabIndex = 12;
			this.label1.Text = "Output Image Format:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(152, 24);
			this.label2.TabIndex = 11;
			this.label2.Text = "Output Filename Format:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtExample
			// 
			this.txtExample.Location = new System.Drawing.Point(8, 39);
			this.txtExample.Name = "txtExample";
			this.txtExample.ReadOnly = true;
			this.txtExample.Size = new System.Drawing.Size(288, 20);
			this.txtExample.TabIndex = 26;
			this.txtExample.Text = "";
			// 
			// DlgSaveImageStepSettings
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(386, 132);
			this.Controls.Add(this.txtExample);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.cbFileType);
			this.Controls.Add(this.btnDefine);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DlgSaveImageStepSettings";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Save Image Settings";
			this.ResumeLayout(false);

		}
		#endregion

		#region ImageFileConfig
		#endregion		
		
		#region Event Handlers
		private void btnDefine_Click(object sender, System.EventArgs e)
		{
			this.UpdateData(true);
			string fileName = this._settings.FileNameFormat;
			int filterType = (int)this._settings.Format;

			using (DlgFileNameFormat dlg = new DlgFileNameFormat(filterType, fileName))
			{
				// I sure that setting is not null, :)
				dlg.Data = this._settings.FileNameFormat;
				
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					this._settings.FileNameFormat = dlg.Data;
					txtExample.Text = this._settings.FileNameFormat;
				}
			}

			#region old version
			/*
			using (DefineOutputFileName dlg	= new DefineOutputFileName(true,true))
			{
				dlg.Prefix					= bPrefix;
				dlg.DefineText				= strDefineFileName;
				dlg.FiterField				= alFiterFileName;
				dlg.HasDefineText			= bDefineText;
				dlg.HasUniqueCode			= m_HaveUniqueCode;
				
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					alFiterFileName		= dlg.FiterField;
					strDefineFileName	= dlg.DefineText;
					bPrefix				= dlg.Prefix; 
					bDefineText			= dlg.HasDefineText;
					m_HaveUniqueCode	= dlg.HasUniqueCode;
					txtExample.Text	= FileNameGenerator.GetDisplayName(alFiterFileName,bDefineText,strDefineFileName,bPrefix,m_HaveUniqueCode);
				}
			}
			*/
			#endregion old version			
		}		

		private void cbFileType_SelectedValueChanged(object sender, System.EventArgs e)
		{
			this.UpdateData(true);
			if (this._settings == null || this._settings.FileNameFormat == null || this._settings.FileNameFormat == string.Empty)
				return;

			 string []aStr = {".bmp", ".jpg", ".png", ".tif", ".gif", ".fit", ".fits"};
			int iType = 0;
			eImageFormat format = this._settings.Format;
			if (format == eImageFormat.Bmp)
				iType = 0;
			else if (format == eImageFormat.Jpeg)
				iType = 1;
			else if (format == eImageFormat.Png)
				iType = 2;
			else if (format == eImageFormat.Tiff)
				iType = 3;
			else if (format == eImageFormat.Gif)
				iType = 4;
			else if (format == eImageFormat.Fit)
				iType = 5;

			int iWrongType = FindWrongType(this._settings.FileNameFormat, iType);
			if (iWrongType < 0)
				return;

			if (iType >= 5 && iWrongType >= 5)
				return;
			// remove and add extension file type automatically
			this._settings.FileNameFormat = RemoveAndAdd(this._settings.FileNameFormat, aStr[iType], aStr[iWrongType]);
			this.UpdateData(false);
		}

		private int FindWrongType(string str, int iType)
		{
			string []aStr = {".bmp", ".jpg", ".png", ".tif", ".gif", ".fit", ".fits"};			
			for (int i=0; i<aStr.Length; i++)
			{
				if (i != iType && IsIn(str, aStr[i]))
					return i;
			}
			return -1;
		}

		private bool IsIn(string str, string sub)
		{
			if (sub == null || sub == string.Empty || str == null || str == string.Empty)
				return false;
			int subLength = sub.Length;
			int strLength = str.Length;
			if (strLength <= subLength)
				return false;
			string sEnd = str.Substring(strLength-subLength, subLength).ToLower();
			if (sEnd == sub)
				return true;
			return false;
		}

		private string RemoveAndAdd(string str, string ext, string wrongExt)
		{
			if (str == null || str == string.Empty || 
				ext == null || ext == string.Empty || 
				wrongExt == null || wrongExt == string.Empty)
				return str;

			int strLength = str.Length;
			int extLength = ext.Length;
			int wrongExtLength = wrongExt.Length;

			if (strLength < wrongExtLength)
				return str;

			str = str.Substring(0, strLength-wrongExtLength);
			str = str + ext;
			return str;
		}
		#endregion

		#region Override Routines

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			// refresh user interface
			this.UpdateData(false);
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing (e);

			if (this.DialogResult == DialogResult.OK)
			{
				if (this.ValidateData() == false)
					e.Cancel = true;
				else // update data when valid
					this.UpdateData(true);
			}
		}

		#endregion

		#region Internal Helpers

		private bool ValidateData()
		{
			if (txtExample.Text == "")
			{
				MessageBoxEx.Error("Format of output file name was not specified. Please try again.");
				return false;
			}

			if (cbFileType.SelectedIndex < 0)
			{
				MessageBoxEx.Error("Image format was not specified. Please try again");
				return false;
			}

			return true;
		}

		private void UpdateData(bool bSaveAndValidate)
		{
			if (this._settings != null)
			{
				if (bSaveAndValidate)
				{
					this._settings.FileNameFormat = txtExample.Text;
					this._settings.Format = (eImageFormat)cbFileType.SelectedValue;
				}
				else
				{
					txtExample.Text = this._settings.FileNameFormat;
					cbFileType.SelectedValue = this._settings.Format;
				}
			}
		}

		private void InitializeImageFormatComboBox()
		{
			DataItemCollection items = new DataItemCollection();
			items.Add("Bitmaps (*.bmp)", eImageFormat.Bmp);
			items.Add("Fits images (*.fit;*.fts;*.fits)", eImageFormat.Fit);
			items.Add("JPEG images (*.jpg)", eImageFormat.Jpeg);
			items.Add("PNG images (*.png)", eImageFormat.Png);
			items.Add("TIFF images (*.tif;*.tiff)", eImageFormat.Tiff);
			items.Add("GIF images (*.gif)", eImageFormat.Gif);
#if DEBUG
			//items.Add("RDE Raw Image Format (*.raw)", eImageFormat.Raw);
            items.Add("Raw Image Format (*.raw)", eImageFormat.Raw);
#endif
			
			// sort item by display member
			items.Sort(new DisplayMemberComparer());

			cbFileType.DataSource = items;
			cbFileType.DisplayMember = "DisplayMember";
			cbFileType.ValueMember = "ValueMember";
			// default is FIST format
			cbFileType.SelectedIndex = 1;
		}
		#endregion		
	}
		
}
