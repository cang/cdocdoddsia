using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using System.Data;
using System.Data.OleDb;

using SIA.UI.Controls;
using SIA.UI.Controls.Common;
using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.Utilities;

using SIA.UI.Controls.Automation;
using SIA.UI.Controls.Automation.Commands;
using SIA.Workbench.Common;

using SiGlaz.UI.CustomControls.XPTable;
using SiGlaz.UI.CustomControls.XPTable.Editors;
using SiGlaz.UI.CustomControls.XPTable.Models;
using SiGlaz.UI.CustomControls.XPTable.Events;

namespace SIA.UI.Controls.Automation.Dialogs
{
	/// <summary>
	/// Summary description for DlgLoadImageStepSettings2.
	/// </summary>
	public class DlgLoadImageStepSettings2 : DialogBase
	{
		#region Window member fields
		private System.Windows.Forms.ToolBar toolBarScriptBuilder;
		private System.Windows.Forms.ToolBarButton cmdRemove;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ToolBarButton cmdAddFile;
        private System.Windows.Forms.ToolBarButton cmdAddFolder;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ImageList imageListMain;
		private System.Windows.Forms.CheckBox chkClearProcessedFileHistory;
		private System.Windows.Forms.CheckBox chkScanSubfolder;
		private System.Windows.Forms.CheckBox checkFilter;
		private System.Windows.Forms.Button btnSettings;

		private Table _tableModel;
		private TableModel tableModel = new TableModel();
		#endregion Window member fields				        
		
		#region Member fields
		private LoadImageCommandSettings _settings = new LoadImageCommandSettings();

        private const string DatabaseFileName = "ScanFileDb.mdb";
		#endregion Member fields

		#region Constructors and Destructors
		public DlgLoadImageStepSettings2()
		{
			InitializeComponent();

            cmdRemove.Enabled = false;
		}

		public DlgLoadImageStepSettings2(LoadImageCommandSettings settings)
		{
			InitializeComponent();

            cmdRemove.Enabled = false;

			this.InitTable();

			this.Settings = settings;            
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgLoadImageStepSettings2));
            this.toolBarScriptBuilder = new System.Windows.Forms.ToolBar();
            this.cmdAddFile = new System.Windows.Forms.ToolBarButton();
            this.cmdAddFolder = new System.Windows.Forms.ToolBarButton();
            this.cmdRemove = new System.Windows.Forms.ToolBarButton();
            this.imageListMain = new System.Windows.Forms.ImageList(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._tableModel = new SiGlaz.UI.CustomControls.XPTable.Models.Table();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkClearProcessedFileHistory = new System.Windows.Forms.CheckBox();
            this.chkScanSubfolder = new System.Windows.Forms.CheckBox();
            this.checkFilter = new System.Windows.Forms.CheckBox();
            this.btnSettings = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this._tableModel)).BeginInit();
            this.SuspendLayout();
            // 
            // toolBarScriptBuilder
            // 
            this.toolBarScriptBuilder.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.cmdAddFile,
            this.cmdAddFolder,
            this.cmdRemove});
            this.toolBarScriptBuilder.ButtonSize = new System.Drawing.Size(90, 18);
            this.toolBarScriptBuilder.DropDownArrows = true;
            this.toolBarScriptBuilder.ImageList = this.imageListMain;
            this.toolBarScriptBuilder.Location = new System.Drawing.Point(0, 0);
            this.toolBarScriptBuilder.Name = "toolBarScriptBuilder";
            this.toolBarScriptBuilder.ShowToolTips = true;
            this.toolBarScriptBuilder.Size = new System.Drawing.Size(612, 28);
            this.toolBarScriptBuilder.TabIndex = 11;
            this.toolBarScriptBuilder.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
            this.toolBarScriptBuilder.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.ToolBarItem_Click);
            // 
            // cmdAddFile
            // 
            this.cmdAddFile.ImageIndex = 0;
            this.cmdAddFile.Name = "cmdAddFile";
            this.cmdAddFile.Text = "Add File";
            this.cmdAddFile.ToolTipText = "Add step";
            // 
            // cmdAddFolder
            // 
            this.cmdAddFolder.ImageIndex = 1;
            this.cmdAddFolder.Name = "cmdAddFolder";
            this.cmdAddFolder.Text = "Add Folder";
            this.cmdAddFolder.ToolTipText = "Add Folder";
            // 
            // cmdRemove
            // 
            this.cmdRemove.ImageIndex = 2;
            this.cmdRemove.Name = "cmdRemove";
            this.cmdRemove.Text = "Remove";
            this.cmdRemove.ToolTipText = "Remove selected steps";
            // 
            // imageListMain
            // 
            this.imageListMain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMain.ImageStream")));
            this.imageListMain.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListMain.Images.SetKeyName(0, "");
            this.imageListMain.Images.SetKeyName(1, "");
            this.imageListMain.Images.SetKeyName(2, "");
            this.imageListMain.Images.SetKeyName(3, "del_database.png");
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Location = new System.Drawing.Point(-82, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(778, 4);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            // 
            // _tableModel
            // 
            this._tableModel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._tableModel.GridColor = System.Drawing.SystemColors.ControlDark;
            this._tableModel.GridLines = SiGlaz.UI.CustomControls.XPTable.Models.GridLines.Both;
            this._tableModel.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._tableModel.Location = new System.Drawing.Point(0, 28);
            this._tableModel.MultiSelect = true;
            this._tableModel.Name = "_tableModel";
            this._tableModel.NoItemsText = "";
            this._tableModel.SelectionStyle = SiGlaz.UI.CustomControls.XPTable.Models.SelectionStyle.Grid;
            this._tableModel.Size = new System.Drawing.Size(614, 294);
            this._tableModel.TabIndex = 9;
            this._tableModel.CellCheckChanged += new SiGlaz.UI.CustomControls.XPTable.Events.CellCheckBoxEventHandler(this.tableScriptBuilder_CellCheckChanged);
            this._tableModel.SelectionChanged += new SiGlaz.UI.CustomControls.XPTable.Events.SelectionEventHandler(this.tableScriptBuilder_SelectionChanged);
            this._tableModel.CellButtonClicked += new SiGlaz.UI.CustomControls.XPTable.Events.CellButtonEventHandler(this.buttonSettingsClicked);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(231, 371);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(76, 24);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "OK";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(315, 371);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(76, 24);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Location = new System.Drawing.Point(-64, 359);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(778, 4);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            // 
            // chkClearProcessedFileHistory
            // 
            this.chkClearProcessedFileHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkClearProcessedFileHistory.Location = new System.Drawing.Point(123, 330);
            this.chkClearProcessedFileHistory.Name = "chkClearProcessedFileHistory";
            this.chkClearProcessedFileHistory.Size = new System.Drawing.Size(256, 24);
            this.chkClearProcessedFileHistory.TabIndex = 18;
            this.chkClearProcessedFileHistory.Text = "Clear processed file history before running script";
            this.chkClearProcessedFileHistory.Visible = false;
            // 
            // chkScanSubfolder
            // 
            this.chkScanSubfolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkScanSubfolder.Location = new System.Drawing.Point(4, 330);
            this.chkScanSubfolder.Name = "chkScanSubfolder";
            this.chkScanSubfolder.Size = new System.Drawing.Size(112, 24);
            this.chkScanSubfolder.TabIndex = 19;
            this.chkScanSubfolder.Text = "Scan sub-folders";
            // 
            // checkFilter
            // 
            this.checkFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkFilter.Location = new System.Drawing.Point(440, 330);
            this.checkFilter.Name = "checkFilter";
            this.checkFilter.Size = new System.Drawing.Size(100, 24);
            this.checkFilter.TabIndex = 20;
            this.checkFilter.Text = "Input File Filter";
            this.checkFilter.CheckedChanged += new System.EventHandler(this.checkFilter_CheckedChanged);
            // 
            // btnSettings
            // 
            this.btnSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSettings.Location = new System.Drawing.Point(540, 330);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(68, 24);
            this.btnSettings.TabIndex = 21;
            this.btnSettings.Text = "Settings";
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // DlgLoadImageStepSettings2
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(612, 403);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.checkFilter);
            this.Controls.Add(this.chkScanSubfolder);
            this.Controls.Add(this.chkClearProcessedFileHistory);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this._tableModel);
            this.Controls.Add(this.toolBarScriptBuilder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DlgLoadImageStepSettings2";
            this.Text = "Load Image Settings";
            ((System.ComponentModel.ISupportInitialize)(this._tableModel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		#region Properties
		public LoadImageCommandSettings Settings
		{
			get {return _settings;}
			set 
			{
				_settings = value;
				OnSettingsChanged();
			}
		}				

		public bool UseFilter
		{
			get { return this.checkFilter.Checked; }
			set 
			{ 
				this.checkFilter.Checked = value;
				OnUseFilter_Changed();
			}
		}
		#endregion Properties

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
					this.UpdateData(true);
			}
		}

        private const int indxColNo = 0;
        private const int indxColScan = 1;
        private const int indxColPath = 2;
        private const int indxColSetting = 3;

		private void InitTable()
		{
			#region create XPTable
			try
			{
				this._tableModel.BeginUpdate();

					// 1
					TextColumn textColumnNo = new TextColumn("No.", 30);

					// 2
					//CheckBoxColumn checkboxColumnStep = new CheckBoxColumn("Removable", 70);

					// 2
					CheckBoxColumn checkboxColumnScanf = new CheckBoxColumn("Listen Mode", 80);

					//3
					TextColumn textColumPath = new TextColumn("Path", 395 + 70);

					//4
					ButtonColumn buttonColumnSetting = new ButtonColumn("Edit", 30);
			

				this._tableModel.ColumnModel = new ColumnModel(new Column[] {
												    textColumnNo,
												    //checkboxColumnStep,
												    checkboxColumnScanf,
												    textColumPath,
												    buttonColumnSetting});

		
				this._tableModel.TableModel = new TableModel();
				
				this._tableModel.TableModel.RowHeight = 21;

				this._tableModel.FullRowSelect = true;				

				this._tableModel.EndUpdate();		
			
				this.cmdRemove.Enabled = false;
			}
			catch (System.Exception exp)
			{
				Trace.WriteLine(exp.Message + exp.StackTrace);
			}
			#endregion create XPTable
		}		
		#endregion Override methods

		#region Internal helpers
		private bool AddFolder()
		{
			bool bSuccess = false;
			
			FolderBrowserDialog dlgBrowser=new FolderBrowserDialog();
			if( dlgBrowser.ShowDialog()==DialogResult.OK)
			{
				if (!AddRow(dlgBrowser.SelectedPath, true))
				{
					MessageBoxEx.Error("The folder {0} has already been added.", dlgBrowser.SelectedPath);
				}
			}

			return bSuccess;
		}		

		private void RemoveSelected()
		{
            try
            {
                Row[] selectedItems = _tableModel.SelectedItems;

                _tableModel.TableModel.Rows.RemoveRange(selectedItems);

                foreach (Row item in this._tableModel.TableModel.Rows)
                {
                    int index = item.Index + 1;
                    item.Cells[indxColNo].Text = index.ToString();
                }
            }
            catch
            {
                // nothing to do
            }	
		}

        private void ClearProcessedFileHistory()
        {
            try
            {
                string databasePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, DatabaseFileName);

                ScanFolderDatabase database = new ScanFolderDatabase(databasePath);
                if (database != null)
                    database.ClearDatabase();
            }
            catch (System.Exception exp)
            {
                string msg = string.Format(
                    "Failed to clear processed files history.\nMessage: {0}\nStackTrace: {1}",
                    exp.Message, exp.StackTrace);

                MessageBox.Show(this, msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
        }

		private void AddFilePath()
		{
			using (OpenFileDialog dlg = CommonDialogs.OpenImageFileDialog("Select image file...", CommonDialogs.ImageFileFilter.AllSupportedImageFormat))
			{
				dlg.Multiselect = true;
				try
				{
					if (DialogResult.OK == dlg.ShowDialog(this))
					{			
						string strDuplicate = string.Empty;
						for (int i=0 ; i< dlg.FileNames.Length; i++)
						{
							if (!AddRow(dlg.FileNames[i].ToString(),false))
							{								
								strDuplicate += "\n" + dlg.FileNames[i].ToString();
							}
						}

						if (strDuplicate != string.Empty)
						{
							strDuplicate = "The follow files have already been added:\n" + strDuplicate;
							MessageBoxEx.Error(strDuplicate); 
						}
					}
				}
				catch (System.Exception exp)
				{
					MessageBoxEx.Error("Failed to browse for image. " + exp.Message);
				}
			}
		}

		private bool AddRow(string filename,bool statusScan)
		{		
			foreach(Row item in this._tableModel.TableModel.Rows)
			{
				if( item.Cells[indxColPath].Text==filename)
				{					
					return false;
				}		
			}

			int  iRows = this._tableModel.TableModel.Rows.Count+1;
			Row row  = null;
			try
			{							
				row = new Row(new Cell[] {
								 new Cell(iRows.ToString()),
								 //new Cell("", false),
								 new Cell("", false),
								 new Cell(filename),
								 new Cell("...")});		
				
				row.Cells[indxColScan].Enabled = statusScan;
			}
			catch (System.Exception exp)
			{
				Trace.WriteLine(exp.Message + exp.StackTrace);				
			}
			finally
			{
				if (row != null)
				{										
					this._tableModel.TableModel.Rows.Add(row);
				}
				row = null;
			}

			return true;
		}

		private bool AddRow(string filename, bool statusScan, bool hasScanning)
		{		
			foreach(Row item in this._tableModel.TableModel.Rows)
			{
				if( item.Cells[indxColPath].Text==filename)
				{					
					return false;
				}		
			}

			int  iRows = this._tableModel.TableModel.Rows.Count+1;
			Row row  = null;
			try
			{							
				row = new Row(new Cell[] {
								 new Cell(iRows.ToString()),
								 //new Cell("", false),
								 new Cell("", hasScanning),
								 new Cell(filename),
								 new Cell("...")});	

				row.Cells[indxColScan].Enabled = statusScan;
			}
			catch (System.Exception exp)
			{				
				Trace.WriteLine(exp.Message + exp.StackTrace);
			}
			finally
			{
				if (row != null)
				{										
					this._tableModel.TableModel.Rows.Add(row);
				}
				row = null;
			}

			return true;
		}
		#endregion Internal helpers

		#region Event handlers
		private void buttonSettingsClicked(object sender, CellButtonEventArgs e)
		{
			bool flag = false;
			if (e.Cell.Row.Cells[indxColScan].Enabled == false)
			{
				using (OpenFileDialog dlg = CommonDialogs.OpenImageFileDialog("Select an image file", CommonDialogs.ImageFileFilter.AllSupportedImageFormat, null, e.Cell.Row.Cells[indxColPath].Text))
				{
					try
					{
						if (DialogResult.OK == dlg.ShowDialog(this))
						{
							foreach(Row item in this._tableModel.TableModel.Rows)
							{
								if( item.Cells[indxColPath].Text==dlg.FileName)
								{
									string strDuplicate = "The file has already been added!";
									MessageBoxEx.Error(strDuplicate); 
									flag = true;
								}	
						
						
							}
							if (flag == false)
								e.Cell.Row.Cells[indxColPath].Text=dlg.FileName;
						}
					}
					catch (System.Exception exp)
					{
						MessageBoxEx.Error("Failed to browse for image. " + exp.Message);
					}
				}				
			}

			else
			{
				FolderBrowserDialog dlgBrowser=new FolderBrowserDialog();
				dlgBrowser.SelectedPath= e.Cell.Row.Cells[indxColPath].Text;
				if( dlgBrowser.ShowDialog()==DialogResult.OK)
				{
					foreach(Row item in this._tableModel.TableModel.Rows)
					{
						if( item.Cells[indxColPath].Text==dlgBrowser.SelectedPath)
						{
							MessageBoxEx.Error("The folder {0} has already been added!", dlgBrowser.SelectedPath); 
							flag = true;
						}						
							
					}
					if (flag == false)
						e.Cell.Row.Cells[indxColPath].Text= dlgBrowser.SelectedPath;
				}
			}		
		}

		enum CMD_INDEX
		{
			ADDFile = 0,
			ADDFolder = 1,
			REMOVES = 2,
            CLEAR_HISTORY
		}

		private void ToolBarItem_Click(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			CMD_INDEX cmd = (CMD_INDEX)toolBarScriptBuilder.Buttons.IndexOf(e.Button);

			switch (cmd)
			{
				case CMD_INDEX.ADDFile:
					this.AddFilePath();
					break;
				case CMD_INDEX.ADDFolder:
					this.AddFolder();
					break;
				case CMD_INDEX.REMOVES:
					this.RemoveSelected();
                    break;
                case CMD_INDEX.CLEAR_HISTORY:
                    this.ClearProcessedFileHistory();
					break;				
			}		
		}

		private void tableScriptBuilder_CellCheckChanged(object sender, CellCheckBoxEventArgs e)
		{
            // new version: nothing to do here
            return;

            //if (e.Column != 1)
            //    return;

            //int counter =0;
            //foreach(Row item in this._tableModel.TableModel.Rows)
            //{
            //    if( item.Cells[1].Checked==true)
            //    {
            //        counter++;
            //        break;
            //    }		
            //}
            //if (counter > 0 )
            //{
            //    cmdRemove.Enabled = true;			
            //}
            //else
            //{
            //    cmdRemove.Enabled = false;
            //}
		}

		protected virtual void OnSettingsChanged()
		{
			this.UpdateData(false);
		}

		private void checkFilter_CheckedChanged(object sender, System.EventArgs e)
		{
			this.OnUseFilter_Changed();
		}

		private void OnUseFilter_Changed()
		{
			this.btnSettings.Enabled = this.checkFilter.Checked;
		}

		private void btnSettings_Click(object sender, System.EventArgs e)
		{
			InputFileFilter inputFileFilter = _settings.Filter.Clone();
			using (DlgInputFileFilter dlg = new DlgInputFileFilter(inputFileFilter))
			{
				if (dlg.ShowDialog(this) == DialogResult.OK)
				{
					_settings.Filter = dlg.Settings;
				}
			}
		}

        private int[] _selectedIndices = null;
        private void tableScriptBuilder_SelectionChanged(object sender, SelectionEventArgs e)
        {
            _selectedIndices = e.NewSelectedIndicies;

            bool bEnableRemoveButton = false;
            if (_selectedIndices != null && _selectedIndices.Length > 0)
                bEnableRemoveButton = true;

            if (cmdRemove.Enabled != bEnableRemoveButton)
                cmdRemove.Enabled = bEnableRemoveButton;
        }
		#endregion	Event handlers

		#region Methods
		// only support for getting data
		protected virtual void UpdateData(bool bSaveAndValidate)
		{
            if (_settings != null)
            {
                _settings.ClearProcessedFileHistory = false;
            }

			if (bSaveAndValidate)
			{
				if (_settings != null)
				{
					bool bScanSubFolder = chkScanSubfolder.Checked;
					_settings.ScanSubFolder = bScanSubFolder;
					_settings.ClearProcessedFileHistory = chkClearProcessedFileHistory.Checked;

					ProcessingDataCollection data = _settings.Data;
					data.Clear();
					
					RowCollection rows =  (RowCollection)this._tableModel.TableModel.Rows;
					foreach (Row row in rows)
					{
						string filePath = row.Cells[indxColPath].Text;
						bool bScan = row.Cells[indxColScan].Checked;
						ProcessingData processingData = new ProcessingData(filePath, bScan, bScanSubFolder);
						data.Add(processingData);
					}

					_settings.UseFilter = this.checkFilter.Checked;
				}
			}
			else
			{	
				this._tableModel.TableModel.Rows.Clear();
				if (_settings != null)
				{
					ProcessingDataCollection data = _settings.Data;
					foreach (ProcessingData processingData in data)
					{
						eProcessingDataType dataType = processingData.Type;
						if (dataType == eProcessingDataType.File)
						{
							AddRow(processingData.FilePath, false);
						}
						else if (dataType == eProcessingDataType.NoScanFolder)
						{
							AddRow(processingData.FilePath, true, false);
						}
						else if (dataType == eProcessingDataType.SubFolderAndNoScan)
						{
							AddRow(processingData.FilePath, true, false);
						}
						else if (dataType == eProcessingDataType.ScanFolder)
						{
							AddRow(processingData.FilePath, true, true);
						}
					}
					chkScanSubfolder.Checked = _settings.ScanSubFolder;
					chkClearProcessedFileHistory.Checked = _settings.ClearProcessedFileHistory;
					this.UseFilter = _settings.UseFilter;
				}
			}
		}

		protected virtual bool ValidateData()
		{
			if (this._tableModel.TableModel.Rows.Count <= 0)
			{
				MessageBoxEx.Error("Image file was not specified. Please try again.");
				return false;
			}

			return true;
		}
		#endregion Methods


        #region ScanFolderDatabase
        /// <summary>
        /// Summary description for Database.
        /// </summary>
        internal class ScanFolderDatabase
        {
            /// <summary>
            /// Connection string
            /// </summary>
            private string _connectionString;

            /// <summary>
            /// Initialize database for scan folder engine
            /// </summary>
            /// <param name="dbFileName"></param>
            public ScanFolderDatabase(string dbFileName)
            {
                _connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};", dbFileName);
            }

            /// <summary>
            /// Check if the file is existed in database
            /// </summary>
            /// <param name="filename">The file to be checked</param>
            /// <returns>true if the file is existed</returns>
            public bool IsExisted(string filename)
            {
                OleDbConnection connection = null;
                OleDbCommand cmd = null;

                try
                {
                    connection = CreateConnection();
                    cmd = connection.CreateCommand();
                    cmd.CommandText = "IsExisted";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@FileName", filename);
                    return cmd.ExecuteScalar() != null;
                }
                finally
                {
                    if (cmd != null)
                    {
                        cmd.Dispose();
                        cmd = null;
                    }
                    if (connection != null)
                    {
                        connection.Close();
                        connection.Dispose();
                        connection = null;
                    }
                }
            }

            /// <summary>
            /// Insert the file name into database
            /// </summary>
            /// <param name="filename">The file name to be inserted</param>
            /// <returns>true if successfully</returns>
            public bool Insert(string filename)
            {
                OleDbConnection connection = null;
                OleDbCommand cmd = null;

                try
                {
                    connection = CreateConnection();
                    cmd = connection.CreateCommand();
                    cmd.CommandText = "[Insert]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@FileName", filename);
                    return cmd.ExecuteNonQuery() != 0;
                }
                finally
                {
                    if (cmd != null)
                    {
                        cmd.Dispose();
                        cmd = null;
                    }
                    if (connection != null)
                    {
                        connection.Close();
                        connection.Dispose();
                        connection = null;
                    }
                }
            }

            /// <summary>
            /// Delete old files if they're older input date
            /// </summary>
            /// <param name="processedDate">Input date</param>
            public void DeleteByProcessedDate(DateTime processedDate)
            {
                OleDbConnection connection = null;
                OleDbCommand cmd = null;

                try
                {
                    connection = CreateConnection();
                    cmd = connection.CreateCommand();
                    cmd.CommandText = "DeleteByProcessedDate";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ProcessedDate", processedDate);
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    if (cmd != null)
                    {
                        cmd.Dispose();
                        cmd = null;
                    }
                    if (connection != null)
                    {
                        connection.Close();
                        connection.Dispose();
                        connection = null;
                    }
                }
            }

            /// <summary>
            /// Delete file from database
            /// </summary>
            /// <param name="filename">The file to be deleted</param>
            public void DeleteByFileName(string filename)
            {
                OleDbConnection connection = null;
                OleDbCommand cmd = null;

                try
                {
                    connection = CreateConnection();
                    cmd = connection.CreateCommand();
                    cmd.CommandText = "DeleteByFileName";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@FileName", filename);
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    if (cmd != null)
                    {
                        cmd.Dispose();
                        cmd = null;
                    }
                    if (connection != null)
                    {
                        connection.Close();
                        connection.Dispose();
                        connection = null;
                    }
                }
            }

            /// <summary>
            /// Clear database
            /// </summary>
            public void ClearDatabase()
            {
                OleDbConnection connection = null;
                OleDbCommand cmd = null;

                try
                {
                    connection = CreateConnection();
                    cmd = connection.CreateCommand();
                    cmd.CommandText = "ClearAll";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    if (cmd != null)
                    {
                        cmd.Dispose();
                        cmd = null;
                    }
                    if (connection != null)
                    {
                        connection.Close();
                        connection.Dispose();
                        connection = null;
                    }
                }
            }

            private System.Data.OleDb.OleDbConnection CreateConnection()
            {
                System.Data.OleDb.OleDbConnection connection = 
                    new System.Data.OleDb.OleDbConnection(_connectionString);
                connection.Open();
                return connection;
            }
        }
        #endregion ScanFolderDatabase        
    }
}

