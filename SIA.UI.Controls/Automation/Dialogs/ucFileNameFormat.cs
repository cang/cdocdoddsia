using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using SIA.UI.Controls;
using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.Utilities;

namespace SIA.UI.Controls.Automation.Dialogs
{
	/// <summary>
	/// Summary description for ucMaskStringEditor.
	/// </summary>
	public class ucMaskStringEditor : System.Windows.Forms.UserControl
	{
		#region Windows Form Members
		private System.Windows.Forms.Button btnTime;
		private System.Windows.Forms.Button btnDate;
		private System.Windows.Forms.Button btnCounter;
		private System.Windows.Forms.Button btnName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textFileNamePreview;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.TextBox txtFileNameMask;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#endregion

		#region Members

		private int _ignoreMaskChanged = 0;
		private IFileType[] _fileTypes = null;
		private int _selFileTypeIndex = -1;

        private bool _supportOnlyTextFile = false;
        private bool _supportTextCsvFile = false;

        private bool _saveFileDialog = true;
        private Label label3;
        private ComboBox cmbSaveAsType;
        private string _browseFileDialogTitle = "Save file as...";

        private List<string> _supportedExtensions = new List<string>();
		#endregion

		#region Properties

		public string FileNameFormat
		{
			get 
            {
                return txtFileNameMask.Text;
            }
			set 
			{
				_ignoreMaskChanged++;
				txtFileNameMask.Text = value;
				_ignoreMaskChanged--;

				OnFileNameFormatChanged();
			}
		}

		protected virtual void OnFileNameFormatChanged()
		{
            CorrectFilePathFormat();

			this.OnUpdatePreview();
		}

        public bool SupportOnlyTextFile
        {
            get { return _supportOnlyTextFile; }
            set 
            { 
                _supportOnlyTextFile = value;
                InitializeComboboxSaveAsType();

                CorrectFilePathFormat();
            }
        }

        public bool SupportTextCsvFile
        {
            get { return _supportTextCsvFile; }
            set
            {
                _supportTextCsvFile = value;
                InitializeComboboxSaveAsType();

                CorrectFilePathFormat();
            }
        }

		public IFileType[] FileTypes
		{
			get {return _fileTypes;}
			set 
            {
                _fileTypes = value;
                InitializeComboboxSaveAsType();
            }
		}

		public int SelectedFileTypeIndex
		{
			get {return _selFileTypeIndex;}
			set 
            {
                _selFileTypeIndex = value;
                if (_selFileTypeIndex < 0 || _selFileTypeIndex >= cmbSaveAsType.Items.Count)
                    _selFileTypeIndex = 0;

                if (cmbSaveAsType.Items != null && cmbSaveAsType.Items.Count > 0 && 
                    cmbSaveAsType.SelectedIndex != _selFileTypeIndex)
                    cmbSaveAsType.SelectedIndex = _selFileTypeIndex;

                CorrectFilePathFormat();
            }
		}

		public IFileType SelectedFileType
		{
			get
			{
				if (_fileTypes == null)
					return null;

				if (_selFileTypeIndex >= 0 && _selFileTypeIndex < _fileTypes.Length)
					return _fileTypes[_selFileTypeIndex];
				return null;
			}
			set
			{
				if (_fileTypes != null)
				{
					for (int i=0; i<_fileTypes.Length; i++)
					{
						if (_fileTypes[i] == value)
						{
							_selFileTypeIndex = i;
							break;
						}
					}
				}

                CorrectFilePathFormat();
			}
		}

        public bool SaveFileDialog
        {
            get { return _saveFileDialog; }
            set { _saveFileDialog = value; }
        }

        public string BrowseFileDialogTitle
        {
            get { return _browseFileDialogTitle; }
            set { _browseFileDialogTitle = value; }
        }

		#endregion

		#region Constructor and destructor

		public ucMaskStringEditor()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

            this.btnName.Text = this.btnName.Text.Replace("[FilePath]", "[FileName]");

            InitializeComboboxSaveAsType();
		}

        ~ucMaskStringEditor()
        {
            if (_supportedExtensions != null)
            {
                _supportedExtensions.Clear();
            }
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.btnTime = new System.Windows.Forms.Button();
            this.btnDate = new System.Windows.Forms.Button();
            this.btnCounter = new System.Windows.Forms.Button();
            this.btnName = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textFileNamePreview = new System.Windows.Forms.TextBox();
            this.txtFileNameMask = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbSaveAsType = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnTime
            // 
            this.btnTime.Location = new System.Drawing.Point(305, 60);
            this.btnTime.Name = "btnTime";
            this.btnTime.Size = new System.Drawing.Size(104, 24);
            this.btnTime.TabIndex = 20;
            this.btnTime.Text = "[HHmmss] Time";
            this.btnTime.Click += new System.EventHandler(this.btnTime_Click);
            // 
            // btnDate
            // 
            this.btnDate.Location = new System.Drawing.Point(197, 60);
            this.btnDate.Name = "btnDate";
            this.btnDate.Size = new System.Drawing.Size(100, 24);
            this.btnDate.TabIndex = 18;
            this.btnDate.Text = "[yyyyMMdd] Date";
            this.btnDate.Click += new System.EventHandler(this.btnDate_Click);
            // 
            // btnCounter
            // 
            this.btnCounter.Location = new System.Drawing.Point(105, 60);
            this.btnCounter.Name = "btnCounter";
            this.btnCounter.Size = new System.Drawing.Size(84, 24);
            this.btnCounter.TabIndex = 17;
            this.btnCounter.Text = "[C] Counter";
            this.btnCounter.Click += new System.EventHandler(this.btnCounter_Click);
            // 
            // btnName
            // 
            this.btnName.Location = new System.Drawing.Point(13, 60);
            this.btnName.Name = "btnName";
            this.btnName.Size = new System.Drawing.Size(84, 24);
            this.btnName.TabIndex = 16;
            this.btnName.Text = "[N] File Name";
            this.btnName.Click += new System.EventHandler(this.btnName_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(10, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 20);
            this.label2.TabIndex = 15;
            this.label2.Text = "Preview:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textFileNamePreview
            // 
            this.textFileNamePreview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textFileNamePreview.Location = new System.Drawing.Point(62, 33);
            this.textFileNamePreview.Name = "textFileNamePreview";
            this.textFileNamePreview.ReadOnly = true;
            this.textFileNamePreview.Size = new System.Drawing.Size(616, 20);
            this.textFileNamePreview.TabIndex = 14;
            // 
            // txtFileNameMask
            // 
            this.txtFileNameMask.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFileNameMask.Location = new System.Drawing.Point(104, 4);
            this.txtFileNameMask.Name = "txtFileNameMask";
            this.txtFileNameMask.Size = new System.Drawing.Size(502, 20);
            this.txtFileNameMask.TabIndex = 12;
            this.txtFileNameMask.TextChanged += new System.EventHandler(this.txtFileNameMask_TextChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 20);
            this.label1.TabIndex = 13;
            this.label1.Text = "File Path Format:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(610, 4);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(68, 20);
            this.btnBrowse.TabIndex = 19;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(428, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Save as type:";
            // 
            // cmbSaveAsType
            // 
            this.cmbSaveAsType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSaveAsType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSaveAsType.FormattingEnabled = true;
            this.cmbSaveAsType.Location = new System.Drawing.Point(498, 62);
            this.cmbSaveAsType.Name = "cmbSaveAsType";
            this.cmbSaveAsType.Size = new System.Drawing.Size(180, 21);
            this.cmbSaveAsType.TabIndex = 22;
            this.cmbSaveAsType.SelectedIndexChanged += new System.EventHandler(this.cmbSaveAsType_SelectedIndexChanged);
            // 
            // ucMaskStringEditor
            // 
            this.Controls.Add(this.cmbSaveAsType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnTime);
            this.Controls.Add(this.btnDate);
            this.Controls.Add(this.btnCounter);
            this.Controls.Add(this.btnName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textFileNamePreview);
            this.Controls.Add(this.txtFileNameMask);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnBrowse);
            this.Name = "ucMaskStringEditor";
            this.Size = new System.Drawing.Size(690, 95);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		#region Event Handlers

		private void btnBrowse_Click(object sender, System.EventArgs e)
		{
			string fileName = this.txtFileNameMask.Text;
		
			// index of first entry is 1
			int selIndex = 1;		
			if (this._selFileTypeIndex >= 0 && this._selFileTypeIndex < _fileTypes.Length)
				selIndex = this._selFileTypeIndex + 1;

            if (_saveFileDialog)
            {
                if (_supportOnlyTextFile)
                {
                    using (SaveFileDialog dlg = new SaveFileDialog())
                    {
                        dlg.Title = "Save file as...";
                        dlg.RestoreDirectory = true;
                        dlg.Filter = _fileTypes[0].Filter;

                        if (DialogResult.OK == dlg.ShowDialog(this))
                        {
                            // retrieve selected file name
                            this.txtFileNameMask.Text = dlg.FileName;
                        }
                    }
                }
                else
                {
                    using (SaveFileDialog dlg = CommonDialogs.SaveFileDialog(null, _browseFileDialogTitle, _fileTypes,
                               selIndex, null, fileName))
                    {
                        // skip check file exist
                        dlg.CheckFileExists = false;

                        // skip check path exist
                        dlg.CheckPathExists = false;

                        if (DialogResult.OK == dlg.ShowDialog(this))
                        {
                            // retrieve selected file type
                            _selFileTypeIndex = dlg.FilterIndex - 1;
                            if (_selFileTypeIndex >= _fileTypes.Length)
                                _selFileTypeIndex = 0;

                            // retrieve selected file name
                            this.txtFileNameMask.Text = dlg.FileName;

                            cmbSaveAsType.SelectedIndex = _selFileTypeIndex;

                            CorrectFilePathFormat();
                        }
                    }
                }
            }
            else
            {                
                {
                    using (OpenFileDialog dlg = CommonDialogs.OpenFileDialog(null, _browseFileDialogTitle, _fileTypes,
                           selIndex, null, fileName))
                    {
                        // skip check file exist
                        dlg.CheckFileExists = false;

                        // skip check path exist
                        dlg.CheckPathExists = false;

                        if (DialogResult.OK == dlg.ShowDialog(this))
                        {
                            // retrieve selected file type
                            _selFileTypeIndex = dlg.FilterIndex - 1;
                            if (_selFileTypeIndex >= _fileTypes.Length)
                                _selFileTypeIndex = 0;

                            // retrieve selected file name
                            this.txtFileNameMask.Text = dlg.FileName;

                            cmbSaveAsType.SelectedIndex = _selFileTypeIndex;

                            CorrectFilePathFormat();                            
                        }
                    }
                }
            }
		}

		private void btnName_Click(object sender, System.EventArgs e)
		{
			string text = txtFileNameMask.Text;
			txtFileNameMask.Text = text + "[N]";
		}

		private void btnCounter_Click(object sender, System.EventArgs e)
		{
			string text = txtFileNameMask.Text;
			txtFileNameMask.Text = text + "[C]";
		}

		private void btnDate_Click(object sender, System.EventArgs e)
		{
			string text = txtFileNameMask.Text;
			txtFileNameMask.Text = text + "[yyyyMMdd]";
		}

		private void btnTime_Click(object sender, System.EventArgs e)
		{
			string text = txtFileNameMask.Text;
			txtFileNameMask.Text = text + "[HHmmss]";
		}

		private void txtFileNameMask_TextChanged(object sender, System.EventArgs e)
		{
            //CorrectFilePathFormat();

			this.OnUpdatePreview();
		}

        private void cmbSaveAsType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedFileTypeIndex = cmbSaveAsType.SelectedIndex;

            CorrectFilePathFormat();
        }
		#endregion

		#region override routines

		protected virtual void OnUpdatePreview()
		{
		if (_ignoreMaskChanged != 0)
				return;

			try
			{
				string result = "";
				string str = this.txtFileNameMask.Text;
				if (str != null && str != string.Empty)
					result = StringParser.Parse(str);

                result = result.Replace("[FilePath]", "[FileName]");

				textFileNamePreview.Text = result;
			}
			catch (Exception exp)
			{
				textFileNamePreview.Text = "Error: " + exp.Message;
			}
		}

		#endregion

        #region Helpers
        private void InitializeComboboxSaveAsType()
        {
            cmbSaveAsType.Items.Clear();
            _supportedExtensions.Clear();
            if (_supportOnlyTextFile)
            {                
                cmbSaveAsType.Items.Add("Text Files (*.txt)");
                _supportedExtensions.Add("*.txt");
            }
            else if (_supportTextCsvFile)
            {
                cmbSaveAsType.Items.Add("Csv Files (*.csv)");
                cmbSaveAsType.Items.Add("Text Files (*.txt)");

                _supportedExtensions.Add("*.csv");
                _supportedExtensions.Add("*.txt");
            }
            else
            {
                if (_fileTypes != null)
                {
                    foreach (IFileType fileType in _fileTypes)
                    {
                        cmbSaveAsType.Items.Add(this.GetDescriptionFromFilter(fileType.Filter));
                        _supportedExtensions.AddRange(fileType.Extension);
                    }
                }
            }

            if (cmbSaveAsType.Items.Count > 0)
                this.SelectedFileTypeIndex = this.SelectedFileTypeIndex;
        }

        private string GetDescriptionFromFilter(string filter)
        {
            int i = filter.IndexOf("|");
            
            if (i < 0) return filter;
            
            if (i == 0) return string.Empty;

            return filter.Substring(0, i).Trim();
        }

        // it is poor algorithm to determine an extension is one of the supported extensions.
        private bool IsSupportedExtension(string ext)
        {
            if (_supportedExtensions == null || _supportedExtensions.Count == 0)
                return false;

            foreach (string supportedExtension in _supportedExtensions)
            {
                if (supportedExtension == ext)
                    return true;
            }

            return false;
        }

        // it is poor algorithm to determine an extension is one of the selected extensions.
        private bool IsTheSelectedExtension(string[] selectedExtensions, string ext)
        {
            if (selectedExtensions == null || selectedExtensions.Length == 0)
                return false;

            foreach (string selectedExtension in selectedExtensions)
            {
                if (selectedExtension == ext)
                    return true;
            }

            return false;
        }

        private void CorrectFilePathFormat()
        {
            try
            {
                string filePathFormat = this.FileNameFormat.Trim();

                if (filePathFormat == string.Empty)
                    return;

                int i = filePathFormat.LastIndexOf(".");

                bool addExtension = false;
                if (i < 0 || i >= filePathFormat.Length - 1)
                    addExtension = true;

                string[] selectedExtensions = SelectedFileType.Extension;

                if (!addExtension)
                {
                    string extension =
                        string.Format("{0}", filePathFormat.Substring(i, filePathFormat.Length - i)).ToLower();                    

                    if (IsTheSelectedExtension(selectedExtensions, extension))
                        return;

                    if (IsSupportedExtension(extension))
                    {
                        filePathFormat = filePathFormat.Substring(0, i);
                    }
                }

                this.FileNameFormat = string.Format("{0}{1}", filePathFormat, selectedExtensions[0]);
            }
            catch
            {
                // nothing to do
            }
        }
        #endregion Helpers        
    }
}
