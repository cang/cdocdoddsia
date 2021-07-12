using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Security;
using System.Security.Permissions;

using Microsoft.Win32;

using SIA.UI.Controls;
using SIA.UI.Controls.Utilities;
using SIA.UI.Controls.Automation;
using SIA.UI.Controls.Automation.Steps;
using SIA.UI.Controls.Automation.Dialogs;
using SIA.UI.Controls.Commands;

using SIA.Workbench.Dialogs;
using SIA.Workbench.Common;
using SIA.Workbench.Common.InterprocessCommunication.SharedMemory;
using SIA.Workbench.UserControls;
using SIA.Workbench.Utilities;

namespace SIA.Workbench
{
    /// <summary>
    /// Summary description for MainForm3.
    /// </summary>
    internal class MainForm3 : System.Windows.Forms.Form
    {
        #region constants
        internal const string Title = "SiGlaz Image Analyzer Automation";
        private const string keySoftware = "SOFTWARE";
        private const string keySiGlaz = "SiGlaz";
        private const string keySIA = "SiGlaz Image Analyzer";
        private const string keySIAW = "SIAW";
        private const string keyRecentScripts = "RecentScripts";
        #endregion

        #region Windows Form Members
        private System.Windows.Forms.ImageList imageListMain;
        private System.Windows.Forms.MainMenu mainMenu;
        private System.Windows.Forms.Panel panelScriptExecutor;
        private System.Windows.Forms.Panel panelScriptBuilder;
        private System.Windows.Forms.ToolBar toolbarRDEW;
        private System.Windows.Forms.ToolBarButton cmdNew;
        private System.Windows.Forms.ToolBarButton cmdOpen;
        private System.Windows.Forms.ToolBarButton cmdSave;
        private System.Windows.Forms.ToolBarButton separator1;
        private System.Windows.Forms.ToolBarButton cmdStart;
        private System.Windows.Forms.ToolBarButton cmdStop;
        private System.Windows.Forms.ToolBarButton cmdSwitchMode;
        private System.Windows.Forms.ToolBarButton separator2;
        private System.Windows.Forms.ToolBarButton separator3;
        private System.Windows.Forms.ToolBarButton cmdSaveLogFile;
        private System.Windows.Forms.ToolBarButton cmdClearLogs;
        private System.Windows.Forms.ToolBarButton cmdSaveAs;
        private System.Windows.Forms.MenuItem mnFile;
        private System.Windows.Forms.MenuItem mnNewScript;
        private System.Windows.Forms.MenuItem mnOpenScript;
        private System.Windows.Forms.MenuItem mnSaveScript;
        private System.Windows.Forms.MenuItem mnExit;
        private System.Windows.Forms.MenuItem mnScript;
        private System.Windows.Forms.MenuItem mnRunScript;
        private System.Windows.Forms.MenuItem mnStopScript;
        private System.Windows.Forms.MenuItem mnHelp;
        private System.Windows.Forms.MenuItem mnAbout;
        private System.Windows.Forms.MenuItem mnTools;
        private System.Windows.Forms.MenuItem mnSwitchMode;
        private System.Windows.Forms.MenuItem menuItem15;
        private System.Windows.Forms.MenuItem mnSaveLogFile;
        private System.Windows.Forms.MenuItem mnSaveAs;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem mnClearLogs;
        private System.Windows.Forms.MenuItem mnuEditScript;
        private System.Windows.Forms.MenuItem mnuExecScript;
        private System.Windows.Forms.StatusBar statusbarMain;
        private System.Windows.Forms.StatusBarPanel statusBarPanelCurrentSubStep;
        private System.Windows.Forms.Timer timerStatusUpdate;

        private System.Windows.Forms.MenuItem mnuRecentScripts;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem mnuPluginManager;
        private System.Windows.Forms.MenuItem mnExport;
        private System.Windows.Forms.MenuItem mnImport;
        private System.Windows.Forms.MenuItem menuItem5;
        private System.Windows.Forms.MenuItem menuItem6;
        private System.Windows.Forms.MenuItem mnuSetDefaultFolder;

        private ucScriptBuilder2 _scriptBuilder = null;
        private ucScriptExecutor2 _scriptExecutor = null;

        private System.ComponentModel.IContainer components;

        #endregion Window member fields

        #region Member fields

        private bool _isExecutionMode = true;
        private WorkingSpace _workingSpace = null;
        private ToolBarButton separator4;
        private ToolBarButton tbClearProcessedFiles;
        private MenuItem menuItem7;
        private MenuItem mnClearProcessedFiles;
        private MenuItem mnSetMonitorFrequency;
        private StatusChangedEventArgs _statusArgEventArgs = null;

        private int _monitorFrequency = 300;

        #endregion Member fields

        #region Constructors and Destructors
        public MainForm3()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // internal initialize class
            this.InitClass();
        }

        private string GetTheMonitorFrequencySettingFileName()
        {
            return Path.Combine(Application.StartupPath, "MonitorFrequencySetting.setting");
        }

        private void SaveTheMonitorFrequency()
        {
            try
            {
                string fileName = this.GetTheMonitorFrequencySettingFileName();
                using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    using (BinaryWriter writer = new BinaryWriter(fs))
                    {
                        writer.Write(_monitorFrequency);
                    }
                }
            }
            catch
            {
                // nothing to do
            }
        }

        private void LoadTheMonitorFrequency()
        {
            try
            {
                string fileName = this.GetTheMonitorFrequencySettingFileName();
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader reader = new BinaryReader(fs))
                    {
                        _monitorFrequency = reader.ReadInt32();
                    }
                }
            }
            catch
            {
                _monitorFrequency = 300;
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // release working space
                if (_workingSpace != null)
                    this.DestroyWorkingSpace(this._workingSpace);
                _workingSpace = null;

                if (components != null)
                {
                    components.Dispose();
                }
            }

            // release script monitor
            if (this._executor != null)
                this._executor.Dispose();
            this._executor = null;

            base.Dispose(disposing);
        }

        #endregion Contructors and Destructors

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm3));
            this.imageListMain = new System.Windows.Forms.ImageList(this.components);
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.mnFile = new System.Windows.Forms.MenuItem();
            this.mnNewScript = new System.Windows.Forms.MenuItem();
            this.mnOpenScript = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.mnSaveScript = new System.Windows.Forms.MenuItem();
            this.mnSaveAs = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.mnuSetDefaultFolder = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.mnExport = new System.Windows.Forms.MenuItem();
            this.mnImport = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.mnuRecentScripts = new System.Windows.Forms.MenuItem();
            this.menuItem15 = new System.Windows.Forms.MenuItem();
            this.mnuPluginManager = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.mnExit = new System.Windows.Forms.MenuItem();
            this.mnTools = new System.Windows.Forms.MenuItem();
            this.mnSwitchMode = new System.Windows.Forms.MenuItem();
            this.mnuEditScript = new System.Windows.Forms.MenuItem();
            this.mnuExecScript = new System.Windows.Forms.MenuItem();
            this.mnScript = new System.Windows.Forms.MenuItem();
            this.mnRunScript = new System.Windows.Forms.MenuItem();
            this.mnStopScript = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.mnClearLogs = new System.Windows.Forms.MenuItem();
            this.mnSaveLogFile = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.mnClearProcessedFiles = new System.Windows.Forms.MenuItem();
            this.mnSetMonitorFrequency = new System.Windows.Forms.MenuItem();
            this.mnHelp = new System.Windows.Forms.MenuItem();
            this.mnAbout = new System.Windows.Forms.MenuItem();
            this.toolbarRDEW = new System.Windows.Forms.ToolBar();
            this.cmdNew = new System.Windows.Forms.ToolBarButton();
            this.cmdOpen = new System.Windows.Forms.ToolBarButton();
            this.cmdSave = new System.Windows.Forms.ToolBarButton();
            this.cmdSaveAs = new System.Windows.Forms.ToolBarButton();
            this.separator1 = new System.Windows.Forms.ToolBarButton();
            this.cmdStart = new System.Windows.Forms.ToolBarButton();
            this.cmdStop = new System.Windows.Forms.ToolBarButton();
            this.separator2 = new System.Windows.Forms.ToolBarButton();
            this.cmdSwitchMode = new System.Windows.Forms.ToolBarButton();
            this.separator3 = new System.Windows.Forms.ToolBarButton();
            this.tbClearProcessedFiles = new System.Windows.Forms.ToolBarButton();
            this.separator4 = new System.Windows.Forms.ToolBarButton();
            this.cmdSaveLogFile = new System.Windows.Forms.ToolBarButton();
            this.cmdClearLogs = new System.Windows.Forms.ToolBarButton();
            this.panelScriptExecutor = new System.Windows.Forms.Panel();
            this.panelScriptBuilder = new System.Windows.Forms.Panel();
            this.statusbarMain = new System.Windows.Forms.StatusBar();
            this.statusBarPanelCurrentSubStep = new System.Windows.Forms.StatusBarPanel();
            this.timerStatusUpdate = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanelCurrentSubStep)).BeginInit();
            this.SuspendLayout();
            // 
            // imageListMain
            // 
            this.imageListMain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMain.ImageStream")));
            this.imageListMain.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListMain.Images.SetKeyName(0, "");
            this.imageListMain.Images.SetKeyName(1, "");
            this.imageListMain.Images.SetKeyName(2, "");
            this.imageListMain.Images.SetKeyName(3, "");
            this.imageListMain.Images.SetKeyName(4, "");
            this.imageListMain.Images.SetKeyName(5, "");
            this.imageListMain.Images.SetKeyName(6, "");
            this.imageListMain.Images.SetKeyName(7, "");
            this.imageListMain.Images.SetKeyName(8, "");
            this.imageListMain.Images.SetKeyName(9, "");
            this.imageListMain.Images.SetKeyName(10, "");
            this.imageListMain.Images.SetKeyName(11, "");
            this.imageListMain.Images.SetKeyName(12, "");
            this.imageListMain.Images.SetKeyName(13, "");
            this.imageListMain.Images.SetKeyName(14, "");
            this.imageListMain.Images.SetKeyName(15, "");
            this.imageListMain.Images.SetKeyName(16, "");
            this.imageListMain.Images.SetKeyName(17, "");
            this.imageListMain.Images.SetKeyName(18, "");
            this.imageListMain.Images.SetKeyName(19, "");
            this.imageListMain.Images.SetKeyName(20, "");
            this.imageListMain.Images.SetKeyName(21, "");
            this.imageListMain.Images.SetKeyName(22, "");
            this.imageListMain.Images.SetKeyName(23, "");
            this.imageListMain.Images.SetKeyName(24, "");
            this.imageListMain.Images.SetKeyName(25, "del_database.png");
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnFile,
            this.mnTools,
            this.mnScript,
            this.menuItem7,
            this.mnHelp});
            // 
            // mnFile
            // 
            this.mnFile.Index = 0;
            this.mnFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnNewScript,
            this.mnOpenScript,
            this.menuItem3,
            this.mnSaveScript,
            this.mnSaveAs,
            this.menuItem4,
            this.mnuSetDefaultFolder,
            this.menuItem6,
            this.mnExport,
            this.mnImport,
            this.menuItem2,
            this.mnuRecentScripts,
            this.menuItem15,
            this.mnuPluginManager,
            this.menuItem5,
            this.mnExit});
            this.mnFile.Text = "&File";
            // 
            // mnNewScript
            // 
            this.mnNewScript.Index = 0;
            this.mnNewScript.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
            this.mnNewScript.Text = "&New Script";
            // 
            // mnOpenScript
            // 
            this.mnOpenScript.Index = 1;
            this.mnOpenScript.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.mnOpenScript.Text = "&Open Script";
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 2;
            this.menuItem3.Text = "-";
            // 
            // mnSaveScript
            // 
            this.mnSaveScript.Index = 3;
            this.mnSaveScript.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.mnSaveScript.Text = "&Save Script";
            // 
            // mnSaveAs
            // 
            this.mnSaveAs.Index = 4;
            this.mnSaveAs.Text = "Save Script &As ...";
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 5;
            this.menuItem4.Text = "-";
            // 
            // mnuSetDefaultFolder
            // 
            this.mnuSetDefaultFolder.Index = 6;
            this.mnuSetDefaultFolder.Text = "Set Default Folder...";
            this.mnuSetDefaultFolder.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 7;
            this.menuItem6.Text = "-";
            // 
            // mnExport
            // 
            this.mnExport.Index = 8;
            this.mnExport.Text = "Ex&port";
            // 
            // mnImport
            // 
            this.mnImport.Index = 9;
            this.mnImport.Text = "I&mport";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 10;
            this.menuItem2.Text = "-";
            // 
            // mnuRecentScripts
            // 
            this.mnuRecentScripts.Index = 11;
            this.mnuRecentScripts.Text = "&Recent Scripts";
            // 
            // menuItem15
            // 
            this.menuItem15.Index = 12;
            this.menuItem15.Text = "-";
            // 
            // mnuPluginManager
            // 
            this.mnuPluginManager.Index = 13;
            this.mnuPluginManager.Text = "&Plugin Manager...";
            this.mnuPluginManager.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 14;
            this.menuItem5.Text = "-";
            // 
            // mnExit
            // 
            this.mnExit.Index = 15;
            this.mnExit.Text = "E&xit";
            // 
            // mnTools
            // 
            this.mnTools.Index = 1;
            this.mnTools.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnSwitchMode,
            this.mnuEditScript,
            this.mnuExecScript});
            this.mnTools.Text = "&View";
            // 
            // mnSwitchMode
            // 
            this.mnSwitchMode.Index = 0;
            this.mnSwitchMode.Text = "Switch Mode";
            this.mnSwitchMode.Visible = false;
            // 
            // mnuEditScript
            // 
            this.mnuEditScript.Index = 1;
            this.mnuEditScript.Text = "&Edit Script";
            this.mnuEditScript.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // mnuExecScript
            // 
            this.mnuExecScript.Index = 2;
            this.mnuExecScript.Text = "&Run and Monitor Script";
            this.mnuExecScript.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // mnScript
            // 
            this.mnScript.Index = 2;
            this.mnScript.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnRunScript,
            this.mnStopScript,
            this.menuItem1,
            this.mnClearLogs,
            this.mnSaveLogFile});
            this.mnScript.Text = "S&cript";
            // 
            // mnRunScript
            // 
            this.mnRunScript.Index = 0;
            this.mnRunScript.Text = "&Start";
            // 
            // mnStopScript
            // 
            this.mnStopScript.Index = 1;
            this.mnStopScript.Text = "S&top";
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 2;
            this.menuItem1.Text = "-";
            // 
            // mnClearLogs
            // 
            this.mnClearLogs.Index = 3;
            this.mnClearLogs.Text = "C&lear Logs";
            // 
            // mnSaveLogFile
            // 
            this.mnSaveLogFile.Index = 4;
            this.mnSaveLogFile.Text = "Sav&e Log File";
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 3;
            this.menuItem7.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnClearProcessedFiles,
            this.mnSetMonitorFrequency});
            this.menuItem7.Text = "&Tools";
            // 
            // mnClearProcessedFiles
            // 
            this.mnClearProcessedFiles.Index = 0;
            this.mnClearProcessedFiles.Text = "&Clear Processed Files History";
            // 
            // mnSetMonitorFrequency
            // 
            this.mnSetMonitorFrequency.Index = 1;
            this.mnSetMonitorFrequency.Text = "&Set the monitor frequency";
            // 
            // mnHelp
            // 
            this.mnHelp.Index = 4;
            this.mnHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnAbout});
            this.mnHelp.Text = "&Help";
            // 
            // mnAbout
            // 
            this.mnAbout.Index = 0;
            this.mnAbout.Text = "&About";
            // 
            // toolbarRDEW
            // 
            this.toolbarRDEW.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.cmdNew,
            this.cmdOpen,
            this.cmdSave,
            this.cmdSaveAs,
            this.separator1,
            this.cmdStart,
            this.cmdStop,
            this.separator2,
            this.cmdSwitchMode,
            this.separator3,
            this.tbClearProcessedFiles,
            this.separator4,
            this.cmdSaveLogFile,
            this.cmdClearLogs});
            this.toolbarRDEW.DropDownArrows = true;
            this.toolbarRDEW.ImageList = this.imageListMain;
            this.toolbarRDEW.Location = new System.Drawing.Point(0, 0);
            this.toolbarRDEW.Name = "toolbarRDEW";
            this.toolbarRDEW.ShowToolTips = true;
            this.toolbarRDEW.Size = new System.Drawing.Size(792, 44);
            this.toolbarRDEW.TabIndex = 0;
            this.toolbarRDEW.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.ToolBarItem_Click);
            // 
            // cmdNew
            // 
            this.cmdNew.ImageIndex = 14;
            this.cmdNew.Name = "cmdNew";
            this.cmdNew.ToolTipText = "New Script";
            // 
            // cmdOpen
            // 
            this.cmdOpen.ImageIndex = 0;
            this.cmdOpen.Name = "cmdOpen";
            this.cmdOpen.ToolTipText = "Open Script";
            // 
            // cmdSave
            // 
            this.cmdSave.ImageIndex = 21;
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.ToolTipText = "Save Script";
            // 
            // cmdSaveAs
            // 
            this.cmdSaveAs.ImageIndex = 24;
            this.cmdSaveAs.Name = "cmdSaveAs";
            this.cmdSaveAs.ToolTipText = "Save Script As";
            // 
            // separator1
            // 
            this.separator1.Name = "separator1";
            this.separator1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // cmdStart
            // 
            this.cmdStart.ImageIndex = 15;
            this.cmdStart.Name = "cmdStart";
            this.cmdStart.ToolTipText = "Run Script";
            // 
            // cmdStop
            // 
            this.cmdStop.ImageIndex = 16;
            this.cmdStop.Name = "cmdStop";
            this.cmdStop.ToolTipText = "Stop Script";
            // 
            // separator2
            // 
            this.separator2.Name = "separator2";
            this.separator2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // cmdSwitchMode
            // 
            this.cmdSwitchMode.ImageIndex = 18;
            this.cmdSwitchMode.Name = "cmdSwitchMode";
            this.cmdSwitchMode.ToolTipText = "Switch Mode";
            // 
            // separator3
            // 
            this.separator3.Name = "separator3";
            this.separator3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // tbClearProcessedFiles
            // 
            this.tbClearProcessedFiles.ImageIndex = 25;
            this.tbClearProcessedFiles.Name = "tbClearProcessedFiles";
            this.tbClearProcessedFiles.ToolTipText = "Cleare processed files history";
            // 
            // separator4
            // 
            this.separator4.Name = "separator4";
            this.separator4.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // cmdSaveLogFile
            // 
            this.cmdSaveLogFile.ImageIndex = 22;
            this.cmdSaveLogFile.Name = "cmdSaveLogFile";
            this.cmdSaveLogFile.ToolTipText = "Save Log File";
            // 
            // cmdClearLogs
            // 
            this.cmdClearLogs.ImageIndex = 23;
            this.cmdClearLogs.Name = "cmdClearLogs";
            this.cmdClearLogs.ToolTipText = "Clear Logs";
            // 
            // panelScriptExecutor
            // 
            this.panelScriptExecutor.Location = new System.Drawing.Point(76, 124);
            this.panelScriptExecutor.Name = "panelScriptExecutor";
            this.panelScriptExecutor.Size = new System.Drawing.Size(200, 100);
            this.panelScriptExecutor.TabIndex = 1;
            // 
            // panelScriptBuilder
            // 
            this.panelScriptBuilder.Location = new System.Drawing.Point(404, 96);
            this.panelScriptBuilder.Name = "panelScriptBuilder";
            this.panelScriptBuilder.Size = new System.Drawing.Size(200, 100);
            this.panelScriptBuilder.TabIndex = 2;
            // 
            // statusbarMain
            // 
            this.statusbarMain.Location = new System.Drawing.Point(0, 623);
            this.statusbarMain.Name = "statusbarMain";
            this.statusbarMain.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.statusBarPanelCurrentSubStep});
            this.statusbarMain.ShowPanels = true;
            this.statusbarMain.Size = new System.Drawing.Size(792, 22);
            this.statusbarMain.TabIndex = 3;
            // 
            // statusBarPanelCurrentSubStep
            // 
            this.statusBarPanelCurrentSubStep.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.statusBarPanelCurrentSubStep.Name = "statusBarPanelCurrentSubStep";
            this.statusBarPanelCurrentSubStep.Width = 775;
            // 
            // timerStatusUpdate
            // 
            this.timerStatusUpdate.Interval = 200;
            this.timerStatusUpdate.Tick += new System.EventHandler(this.StatusUpdateTimer_Tick);
            // 
            // MainForm3
            // 
            this.AllowDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(792, 645);
            this.Controls.Add(this.statusbarMain);
            this.Controls.Add(this.panelScriptBuilder);
            this.Controls.Add(this.panelScriptExecutor);
            this.Controls.Add(this.toolbarRDEW);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu;
            this.MinimumSize = new System.Drawing.Size(400, 350);
            this.Name = "MainForm3";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanelCurrentSubStep)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #region Properties
        private bool DisplayExecutorDialog
        {
            get
            {
                return _isExecutionMode;
            }
            set
            {
                if (_isExecutionMode != value)
                {
                    _isExecutionMode = value;
                    this.SwitchDisplayedDialog();
                }
            }
        }
        #endregion Properties

        #region Override methods
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // create an empty working space
            this.DoCommandNewScript();

            // refresh user interface
            //this.UpdateGUIStatus(false);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);

            // thread's running, so confirm before closing
            if (this._scriptExecutor.CanStop)
            {
                if (MessageBoxEx.ConfirmYesNo("Do you really want to exit?") == false)
                {
                    e.Cancel = true;
                    return;
                }

                // stop current script
                this.StopScript();
            }
            else if (this._workingSpace.Modified)
            {
                // ask user to save changes				
                DialogResult result = MessageBoxEx.ConfirmYesNoCancel("Script has been modified. Do you want to save changes?");
                if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
                else if (result == DialogResult.Yes)
                {
                    // it's save as
                    if (false == this.DoCommandSaveScript(true))
                    {
                        if (MessageBoxEx.ConfirmYesNo("Script has not been saved yet. Do you really want to exit?") == false)
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            }

            e.Cancel = false;

            // save monitor frequency setting
            this.SaveTheMonitorFrequency();
        }
        #endregion Override methods

        #region Event handlers

        enum CMD_INDEX
        {
            NEWSCRIPT = 0,
            OPENSCRIPT,
            SAVESCRIPT,
            SAVESCRIPTAS,
            SEPARATOR1,
            RUNSCRIPT,
            STOPSCRIPT,
            SEPARATOR2,
            SWITCHMODE,
            SEPARATOR3,
            CLEARHISTORY,
            SEPARATOR4,
            SAVELOGFILE,
            CLEARLOGS
        }

        private void ToolBarItem_Click(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
        {
            CMD_INDEX cmd = (CMD_INDEX)toolbarRDEW.Buttons.IndexOf(e.Button);

            switch (cmd)
            {
                case CMD_INDEX.NEWSCRIPT: // exist all  modes
                    this.DoCommandNewScript();
                    break;
                case CMD_INDEX.OPENSCRIPT: // exist all  modes
                    this.DoCommandOpenScript();
                    break;
                case CMD_INDEX.SAVESCRIPT: // only exist in the script executor mode
                    this.DoCommandSaveScript(false);
                    break;
                case CMD_INDEX.SAVESCRIPTAS:
                    this.DoCommandSaveScript(true);
                    break;
                case CMD_INDEX.RUNSCRIPT: // exist all modess
                    this.DoCommandRunScript();
                    break;
                case CMD_INDEX.STOPSCRIPT: // only exist in the script executor mode
                    this.DoCommandStopScript();
                    break;
                case CMD_INDEX.SWITCHMODE: // exist all modes
                    this.DoCommandSwitchMode();
                    break;
                case CMD_INDEX.CLEARHISTORY:
                    this.DoCommandClearProcessedFilesHistory();
                    break;
                case CMD_INDEX.SAVELOGFILE: // only exist in the script executor mode
                    this.DoCommandSaveLogFile();
                    break;
                case CMD_INDEX.CLEARLOGS:
                    this.DoCommandClearLogs();
                    break;
            }
        }

        private void MenuItem_Click(object sender, System.EventArgs e)
        {
            if (sender == mnNewScript)
                this.DoCommandNewScript();
            else if (sender == mnOpenScript)
                this.DoCommandOpenScript();
            else if (sender == mnSaveScript)
                this.DoCommandSaveScript(false);
            else if (sender == mnSaveLogFile)
                this.DoCommandSaveLogFile();
            else if (sender == mnExit)
                this.DoCommandExit();
            else if (sender == mnRunScript)
                this.DoCommandRunScript();
            else if (sender == mnStopScript)
                this.DoCommandStopScript();
            else if (sender == mnSwitchMode || sender == mnuEditScript || sender == mnuExecScript)
                this.DoCommandSwitchMode();
            else if (sender == mnSaveAs)
                this.DoCommandSaveScript(true);
            else if (sender == mnClearLogs)
                this.DoCommandClearLogs();
            else if (sender == mnuPluginManager)
                this.DoCommandPluginManager();
            else if (sender == mnAbout)
                this.DoCommandShowDialogAbout();
            else if (sender == mnImport)
                this.DoCommandImport();
            else if (sender == mnExport)
                this.DoCommandExport();
            else if (sender == mnuSetDefaultFolder)
                this.DoCommandSetDefaultFolder();
            else if (sender == mnClearProcessedFiles)
                this.DoCommandClearProcessedFilesHistory();
            else if (sender == mnSetMonitorFrequency)
                DoCommandSetMonitorFrequency();
        }        

        private void RecentMenuItem_Click(object sender, EventArgs e)
        {
            MenuItem item = sender as MenuItem;
            string text = item.Text;
            string filePath = text;
            int pos = text.IndexOf(".");
            if (pos > 0)
                filePath = text.Remove(0, pos + 1);
            this.DoCommandOpenRecentScript(filePath);
        }


        private void WorkingSpace_ModifiedChanged(object sender, EventArgs e)
        {
            this.UpdateGUIStatus(false);
        }


        #endregion Event handlers

        #region Methods

        public void DoCommandNewScript()
        {
            // initialize working space
            _workingSpace = this.CreateWorkingSpace();

            // reload working space for each user control
            this._scriptBuilder.WorkingSpace = _workingSpace;
            this._scriptExecutor.WorkingSpace = _workingSpace;

            // default user view to script builder
            //this.SwitchDisplayedDialog();

            // update user interface
            if (_isExecutionMode)
            {
                this.DoCommandSwitchMode();
            }

            this._scriptBuilder.DoCommandNewScript();

            // update windows title
            this.UpdateWindowTitle();

            // only update
            cmdStart.Enabled = false;
            cmdStop.Enabled = false;
            mnRunScript.Enabled = false;
            mnStopScript.Enabled = false;
        }

        public void DoCommandOpenScript()
        {
            // close working space and ask for save changes.
            if (this._workingSpace.Modified)
            {
                // show messages
                DialogResult result = MessageBoxEx.ConfirmYesNoCancel("Script has been modified. Do you want to save changes?");
                if (result == DialogResult.Cancel)
                    return;
                else if (result == DialogResult.Yes)
                {
                    try
                    {
                        this.DoCommandSaveScript(false);
                    }
                    catch (System.Exception exp)
                    {
                        MessageBoxEx.Error("Failed to save script: " + exp.Message);

                        return;
                    }
                }
            }

            // load script and inits working space
            bool scriptLoaded = false;
            WorkingSpace newWorkSpace = this.CreateWorkingSpace();

            using (OpenFileDialog dlg = FileTypes.RDEScript.OpenFileDialog("Open Script File"))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        // load script from file
                        newWorkSpace.OpenScript(dlg.FileName);

                        // set loaded flag
                        scriptLoaded = true;
                    }
                    catch (System.Exception exp)
                    {
                        MessageBoxEx.Error(exp.Message);
                    }
                }
            }

            if (scriptLoaded)
            {
                // release working space
                this.DestroyWorkingSpace(_workingSpace);

                // set new working space
                _workingSpace = newWorkSpace;

                // force user control to reload working space
                this._scriptBuilder.WorkingSpace = _workingSpace;
                _scriptExecutor.WorkingSpace = _workingSpace;

                // update user interface
                if (_isExecutionMode)
                {
                    this._scriptExecutor.DoCommandOpenScriptFile();

                    // check and update to Script Builder
                }
                else
                {
                    this._scriptBuilder.DoCommandOpenScript();

                    // check and update to Script Executor
                }

                // enable run flag
                this.cmdStart.Enabled = this._scriptExecutor.CanRun;
                this.mnRunScript.Enabled = this._scriptExecutor.CanRun;

                // update windows title
                this.UpdateWindowTitle();

                // update gui status
                this.UpdateGUIStatus(false);

                // add to recent scripts
                this.AppendRecentScripts(_workingSpace.FileName);

                // ask for update default folder
                if (MessageBoxEx.ConfirmYesNo("Do you want to change the default folder to the recipe path?"))
                    CommonDialogs.DefaultFolder = Path.GetDirectoryName(_workingSpace.FileName);
            }
        }

        public void DoCommandOpenRecentScript(string fileName)
        {
            if (File.Exists(fileName) == false)
            {
                MessageBoxEx.Error(string.Format("The script \"{0}\" is not exist.", fileName));
                return;
            }

            // close working space and ask for save changes.
            if (this._workingSpace.Modified)
            {
                // show messages
                DialogResult result = MessageBoxEx.ConfirmYesNoCancel("Script has been modified. Do you want to save changes?");
                if (result == DialogResult.Cancel)
                    return;
                else if (result == DialogResult.Yes)
                {
                    try
                    {
                        this.DoCommandSaveScript(false);
                    }
                    catch (System.Exception exp)
                    {
                        MessageBoxEx.Error("Failed to save script: " + exp.Message);

                        return;
                    }
                }
            }

            // load script and initializes working space
            bool scriptLoaded = false;
            WorkingSpace newWorkSpace = this.CreateWorkingSpace();

            try
            {
                // load script from file
                newWorkSpace.OpenScript(fileName);

                // set loaded flag
                scriptLoaded = true;
            }
            catch (System.Exception exp)
            {
                MessageBoxEx.Error(exp.Message);
            }

            if (scriptLoaded)
            {
                // release working space
                this.DestroyWorkingSpace(_workingSpace);

                // set new working space
                _workingSpace = newWorkSpace;

                // force user control to reload working space
                this._scriptBuilder.WorkingSpace = _workingSpace;
                _scriptExecutor.WorkingSpace = _workingSpace;

                // update user interface
                if (_isExecutionMode)
                {
                    this._scriptExecutor.DoCommandOpenScriptFile();

                    // check and update to Script Builder
                }
                else
                {
                    this._scriptBuilder.DoCommandOpenScript();

                    // check and update to Script Executor
                }

                // enable run flag
                this.cmdStart.Enabled = this._scriptExecutor.CanRun;
                this.mnRunScript.Enabled = this._scriptExecutor.CanRun;

                // update windows title
                this.UpdateWindowTitle();

                // update gui status
                this.UpdateGUIStatus(false);

                // add to recent scripts
                this.AppendRecentScripts(_workingSpace.FileName);

                // ask for update default folder
                if (MessageBoxEx.ConfirmYesNo("Do you want to change the default folder to the recipe path?"))
                    CommonDialogs.DefaultFolder = Path.GetDirectoryName(_workingSpace.FileName);
            }
        }

        public bool DoCommandSaveScript(bool bSaveAs)
        {
            bool result = false;

            // save script and reset user interface
            string fileName = "";

            if (_workingSpace.FileName == "")
                bSaveAs = true;

            if (bSaveAs)
            {
                using (SaveFileDialog dlg = FileTypes.RDEScript.SaveFileDialog("Save script as..."))
                {
                    if (DialogResult.OK == dlg.ShowDialog(this))
                        fileName = dlg.FileName;
                    else
                        return result;
                }
            }

            try
            {
                // clear old processing step
                _workingSpace.Script.ProcessSteps.Clear();

                // create processing step and append to script 
                int numStep = this._scriptBuilder.StepCount;
                for (int i = 0; i < numStep; i++)
                {
                    ProcessStep step = this._scriptBuilder.CreateStep(i);
                    _workingSpace.Script.ProcessSteps.Add(step);
                }

                string oldFilePath = _workingSpace.FileName;

                // save script to file
                if (bSaveAs)
                    _workingSpace.SaveScript(fileName);
                else
                    _workingSpace.SaveScript();

                // update user interface
                this.UpdateWindowTitle();

                // add to recent scripts when save as new file
                if (bSaveAs)
                    this.AppendRecentScripts(fileName);

                // ask for update default folder
                if (_workingSpace.FileName != oldFilePath)
                {
                    if (MessageBoxEx.ConfirmYesNo("The recipe was saved to another location. Do you want to change the default folder to the recipe folder?"))
                        CommonDialogs.DefaultFolder = Path.GetDirectoryName(_workingSpace.FileName);
                }

                result = true;
            }
            catch (System.Exception exp)
            {
                MessageBoxEx.Error("Failed to save script: " + exp.Message, "Save script...");
            }

            // refresh user interface
            this._scriptBuilder.DoCommandSaveScript();

            return result;
        }

        public void DoCommandRunScript()
        {
            try
            {
                // Step 1: Ask user to save changes if the script is modified
                if (_workingSpace.Modified)
                {
                    // show messages
                    DialogResult result = MessageBoxEx.ConfirmYesNoCancel("Script has been modified. Do you want to save changes?");
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }
                    else if (result == DialogResult.Yes)
                    {
                        if (false == this.DoCommandSaveScript(false))
                        {
                            return;
                        }
                    }
                }

                // Step 2: Validate processing steps

                // clear validation keys
                this._workingSpace.ClearKeys();

                // validate each process step
                bool scriptValid = true;

                // check if script is empty
                if (this._scriptBuilder.StepCount == 0)
                {
                    MessageBoxEx.Error("Script is empty. Please add the process steps.");
                    scriptValid = false;
                }

                // validate each process step
                int numStep = this._scriptBuilder.StepCount;
                for (int i = 0; i < numStep; i++)
                {
                    if (!this._scriptBuilder.ValidateStep(i))
                    {
                        scriptValid = false;
                        break;
                    }
                }

                // clear validation keys
                this._workingSpace.ClearKeys();

                if (scriptValid)
                {
                    #region Old version of script validation

                    //					// clear old processing step
                    //					_workingSpace.Script.ProcessSteps.Clear();
                    //
                    //					// flags to check logic
                    //					bool hasWafer = false;
                    //					bool hasObject = false;
                    //					bool hasExportKlarf = false;
                    //					bool hasClearWaferBound = false;
                    //					bool hasIntensityCorrection = false;
                    //					// validation table
                    //					Hashtable validateTable = new Hashtable();

                    //					// create processing step and append to script 
                    //					for (int i=0; i<numStep; i++) 
                    //					{
                    //						ProcessStep step = this._scriptBuilder.CreateStep(i);
                    //						_workingSpace.Script.ProcessSteps.Add(step);
                    //
                    //						string key = step.ValidateKey;
                    //						if (key != null && key != string.Empty)
                    //							validateTable[key] = true;
                    //
                    //						if (validateTable.ContainsKey(ValidationKeys.ExportKLARF) &&
                    //							(bool)validateTable[ValidationKeys.ExportKLARF] == true)
                    //						{
                    //							hasExportKlarf = true;
                    //							if (hasObject == false || hasWafer == false)
                    //								break;
                    //						}
                    //						
                    //						if (validateTable.ContainsKey(ValidationKeys.Waferboundary) &&
                    //							(bool)validateTable[ValidationKeys.Waferboundary] == true)
                    //						{
                    //							hasWafer = true;
                    //						}
                    //						
                    //						if (validateTable.ContainsKey(ValidationKeys.DetectObject) &&
                    //							(bool)validateTable[ValidationKeys.DetectObject] == true)
                    //						{
                    //							hasObject = true;
                    //						}
                    //						
                    //						if (validateTable.ContainsKey(ValidationKeys.ClearWaferBound) &&
                    //							(bool)validateTable[ValidationKeys.ClearWaferBound] == true)
                    //						{
                    //							hasClearWaferBound = true;
                    //							if (hasWafer == false)
                    //								break;
                    //						}
                    //						
                    //						if (validateTable.ContainsKey(ValidationKeys.IntensityCorrection) &&
                    //							(bool)validateTable[ValidationKeys.IntensityCorrection] == true)
                    //						{
                    //							hasIntensityCorrection = true;
                    //							if (hasWafer == false)
                    //								break;
                    //						}
                    //					}
                    //
                    //					
                    //					// refresh executer screen
                    //					_scriptExecutor.WorkingSpace = _workingSpace;
                    //
                    //					#region old version to check logic, will improve later
                    //					// Author: Cong
                    //					// check for logic
                    //					int buildScript = 0;				
                    //					if (hasExportKlarf) 
                    //					{
                    //						// stupid, fix later
                    //						if (hasWafer == false && hasObject == false)
                    //							buildScript = 3;
                    //						else if (hasWafer == false)
                    //							buildScript = 1;
                    //						else if (hasObject == false)
                    //							buildScript = 2;
                    //					}
                    //					else if (hasClearWaferBound) 
                    //					{
                    //						if (hasWafer == false)
                    //							buildScript = 4;
                    //					}
                    //					else if (hasIntensityCorrection) 
                    //					{
                    //						if (hasWafer == false)
                    //							buildScript = 5;
                    //					}
                    //				
                    //					if (buildScript == 1) 
                    //					{
                    //						MessageBoxEx.Error("Cannot execute Export Detected Objects To KLARF step. Please detect wafer boundary first!");
                    //					}
                    //					else if (buildScript == 2) 
                    //					{
                    //						MessageBoxEx.Error("Cannot execute Export Detected Objects To KLARF step. Please detect wafer boundary first!");
                    //					}
                    //					else if (buildScript == 3) 
                    //					{
                    //						MessageBoxEx.Error("Cannot execute Export Detected Objects To KLARF step. Please detect wafer boundary, and detect objects first!");
                    //					}
                    //					else if (buildScript == 4) 
                    //					{
                    //						MessageBoxEx.Error("Cannot execute Clear Wafer Boundary step. Please detect wafer boundary first!");
                    //					}
                    //					else if (buildScript == 5) 
                    //					{
                    //						MessageBoxEx.Error("Cannot execute Intensity Correction step. Please detect wafer boundary first!");
                    //					}
                    //				
                    //					if (buildScript > 0)
                    //						return;
                    //					#endregion old version to check logic, will improve later

                    #endregion

                    // refresh executer screen
                    _scriptExecutor.WorkingSpace = _workingSpace;

                    // switch to executor screen
                    if (!_isExecutionMode)
                        this.DoCommandSwitchMode();

                    // start launching script
                    this.RunScript();

                    // lock GUI
                    this.UpdateGUIStatus(true);
                }
            }
            catch (System.Exception exp)
            {
                // trace exception detail
                Trace.WriteLine(exp);

                // display exception message
                MessageBoxEx.Error("Failed to run script. Reason: " + exp.Message);
            }
        }

        public void DoCommandStopScript()
        {
            try
            {
                if (MessageBoxEx.ConfirmYesNo("Do you really want to stop the running script?"))
                {
                    // stop executing script
                    this.StopScript();

                    // update status bar
                    this.UpdateStatusBar("");

                    // unlock GUI
                    this.UpdateGUIStatus(false);
                }
            }
            catch (System.Exception exp)
            {
                // trace exception detail
                Trace.WriteLine(exp);

                // display exception message
                MessageBoxEx.Error("Failed to stop script. Reason: " + exp.Message);
            }
        }

        public void DoCommandSwitchMode()
        {
            this.DisplayExecutorDialog = !this.DisplayExecutorDialog;
        }

        public void DoCommandClearProcessedFilesHistory()
        {
            try
            {
                string databasePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, "ScanFileDb.mdb");

                ScanFolderDatabase database = new ScanFolderDatabase(databasePath);
                if (database != null)
                    database.ClearDatabase();

                MessageBox.Show(
                    this,
                    "Clear Processed Files History Successfully.", 
                    "Clear Processed Files History", 
                    MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            catch (System.Exception exp)
            {
                string msg = string.Format(
                    "Failed to clear processed files history.\nMessage: {0}\nStackTrace: {1}",
                    exp.Message, exp.StackTrace);

                MessageBox.Show(this, msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DoCommandSetMonitorFrequency()
        {
            DlgSetTheMonitorFrequency dlg = new DlgSetTheMonitorFrequency(_monitorFrequency);

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                _monitorFrequency = dlg.MonitorFrequency;
            }
        }

        public void DoCommandSaveLogFile()
        {
            this._scriptExecutor.DoCommandSaveLogFile();
        }

        public void DoCommandClearLogs()
        {
            this._scriptExecutor.ClearLogs();
        }

        public void DoCommandShowDialogAbout()
        {
            using (DlgAbout dlg = new DlgAbout())
                dlg.ShowDialog(this);
        }

        public void DoCommandExit()
        {
            this.Close();
        }

        public void DoCommandPluginManager()
        {
            using (DlgPluginManager dlg = new DlgPluginManager())
                dlg.ShowDialog(this);
        }

        private void DoCommandImport()
        {
            using (DlgImportScript dlg = new DlgImportScript())
            {
                if (DialogResult.OK == dlg.ShowDialog(this))
                {
                    try
                    {
                        if (dlg.PackagePath == string.Empty || File.Exists(dlg.PackagePath) == false)
                        {
                            MessageBoxEx.Error("File {0} was not found", dlg.PackagePath);
                            return;
                        }

                        if (dlg.ImportDir == string.Empty || Directory.Exists(dlg.ImportDir) == false)
                        {
                            if (false == MessageBoxEx.ConfirmYesNo("Destination folder does not exist. Do you want to continue? Click Yes to create directory and continue, No to cancel."))
                                return;
                            Directory.CreateDirectory(dlg.ImportDir);
                        }

                        this._scriptBuilder.ImportScript(dlg.PackagePath, dlg.ImportDir);

                        MessageBoxEx.Info("Script was imported successfully.");
                    }
                    catch (Exception exp)
                    {
                        Trace.WriteLine(exp);
                        MessageBoxEx.Error("Failed to import script: " + exp.Message);
                    }
                }
            }
        }

        private void DoCommandExport()
        {
            using (SaveFileDialog dlg = CommonDialogs.SavePackageFileDialog("Save package as..."))
            {
                if (DialogResult.OK == dlg.ShowDialog(this))
                {
                    try
                    {
                        this._scriptBuilder.ExportScript(dlg.FileName);
                        MessageBoxEx.Info("Script was exported successfully.");
                    }
                    catch (Exception exp)
                    {
                        Trace.WriteLine(exp);
                        MessageBoxEx.Error("Failed to export script: " + exp.Message);
                    }
                }
            }
        }

        private void DoCommandSetDefaultFolder()
        {
            using (FolderBrowserDialog dlg = CommonDialogs.SelectFolderDialog("Please select a default location"))
            {
                dlg.SelectedPath = CommonDialogs.DefaultFolder;
                if (dlg.ShowDialog(this) == DialogResult.OK)
                    CommonDialogs.DefaultFolder = dlg.SelectedPath;
            }
        }

        #endregion Mehtods

        #region Internal Helpers

        private void InitClass()
        {
            this._scriptBuilder = new SIA.Workbench.UserControls.ucScriptBuilder2();
            this._scriptBuilder.Dock = DockStyle.Fill;
            panelScriptBuilder.Controls.Add(this._scriptBuilder);

            _scriptExecutor = new ucScriptExecutor2();
            _scriptExecutor.Dock = DockStyle.Fill;
            panelScriptExecutor.Controls.Add(_scriptExecutor);

            this.mnRunScript.Enabled = false;
            this.cmdStart.Enabled = false;
            this.mnStopScript.Enabled = false;
            this.cmdStop.Enabled = false;

            // show editor dialog by default
            // the first bDisplayExecutor flag is set by true
            // after this command it's set by false;
            this.DisplayExecutorDialog = !this.DisplayExecutorDialog;

            // register for event handlers
            this.mnNewScript.Click += new System.EventHandler(this.MenuItem_Click);
            this.mnOpenScript.Click += new System.EventHandler(this.MenuItem_Click);
            this.mnSaveScript.Click += new System.EventHandler(this.MenuItem_Click);
            this.mnSaveLogFile.Click += new System.EventHandler(this.MenuItem_Click);
            this.mnExit.Click += new System.EventHandler(this.MenuItem_Click);
            this.mnRunScript.Click += new System.EventHandler(this.MenuItem_Click);
            this.mnStopScript.Click += new System.EventHandler(this.MenuItem_Click);
            this.mnSwitchMode.Click += new System.EventHandler(this.MenuItem_Click);
            this.mnAbout.Click += new System.EventHandler(this.MenuItem_Click);
            this.mnSaveAs.Click += new System.EventHandler(this.MenuItem_Click);
            this.mnClearLogs.Click += new System.EventHandler(this.MenuItem_Click);
            this.mnExport.Click += new EventHandler(this.MenuItem_Click);
            this.mnImport.Click += new EventHandler(this.MenuItem_Click);

            this.mnClearProcessedFiles.Click += new EventHandler(this.MenuItem_Click);
            this.mnSetMonitorFrequency.Click += new EventHandler(this.MenuItem_Click);

            // initializes recent menu
            this.InitializeRecentMenu();

            // Load monitor frequency setting
            this.LoadTheMonitorFrequency();
        }

        private WorkingSpace CreateWorkingSpace()
        {
            WorkingSpace workSpace = new WorkingSpace(this);
            workSpace.ModifiedChanged += new EventHandler(WorkingSpace_ModifiedChanged);
            return workSpace;
        }

        private void DestroyWorkingSpace(WorkingSpace workSpace)
        {
            if (workSpace != null)
            {
                // unregist for modified change event
                workSpace.ModifiedChanged -= new EventHandler(WorkingSpace_ModifiedChanged);
                // release old working space
                workSpace.Dispose();
            }
        }


        private void SwitchDisplayedDialog()
        {
            this.UpdateWindowTitle();

            if (_isExecutionMode)
            {
                panelScriptExecutor.Dock = DockStyle.Fill;
                panelScriptExecutor.BringToFront();
                //this.Text = "RDE Monitor";
                //this.Text = MainForm3.Title;

                cmdSave.Enabled = false;
                mnSaveScript.Enabled = false;

                // update tool tip text
                cmdSwitchMode.ToolTipText = "Edit script";

                // update menu switch mode
                //mnSwitchMode.Text = "Switch to script builder mode";				
                mnuEditScript.Checked = false;
                mnuExecScript.Checked = true;
            }
            else
            { //executor
                panelScriptBuilder.Dock = DockStyle.Fill;
                panelScriptBuilder.BringToFront();
                //this.Text = "Script Builder";
                //this.Text = MainForm3.Title;

                cmdSave.Enabled = true;
                mnSaveScript.Enabled = true;

                // update tool tip text
                cmdSwitchMode.ToolTipText = "Run and monitor script"; //"Switch to script monitor mode";

                // update menu switch mode
                //mnSwitchMode.Text = "Switch to script monitor mode";
                mnuEditScript.Checked = true;
                mnuExecScript.Checked = false;
            }

            // update GUI status
            //UpdateGUIAtSwitchMode();

            // update GUI status
            this.UpdateGUIStatus(false);
        }

        private void UpdateGUIAtSwitchMode()
        {
            if (_isExecutionMode)
            {
                // update toolbar
                cmdSave.Visible = false;
                cmdSaveLogFile.Visible = true;
                cmdStop.Visible = true;

                // update menu
                mnSaveScript.Visible = false;
                mnSaveLogFile.Visible = true;
                mnStopScript.Visible = true;

                mnClearProcessedFiles.Enabled = false;
                tbClearProcessedFiles.Enabled = false;
            }
            else
            {
                // update toolbar
                cmdSave.Visible = true;
                cmdSaveLogFile.Visible = false;
                cmdStop.Visible = false;

                // update menu
                mnSaveScript.Visible = true;
                mnSaveLogFile.Visible = false;
                mnStopScript.Visible = false;

                mnClearProcessedFiles.Enabled = true;
                tbClearProcessedFiles.Enabled = true;
            }
        }

        private void UpdateGUIStatus(bool bLock)
        {
            if (_isExecutionMode)
            {
                if (bLock)
                {
                    #region update menu status
                    // file menu
                    mnNewScript.Enabled = false;
                    mnOpenScript.Enabled = false;
                    mnSaveScript.Enabled = false;
                    mnSaveAs.Enabled = false;
                    mnImport.Enabled = false;
                    mnExport.Enabled = false;

                    // script menu
                    mnRunScript.Enabled = false;
                    mnStopScript.Enabled = true;
                    mnSaveLogFile.Enabled = false;
                    mnClearLogs.Enabled = false;

                    // tools menu
                    mnSwitchMode.Enabled = false;
                    mnuEditScript.Enabled = false;
                    mnuExecScript.Enabled = false;

                    mnClearProcessedFiles.Enabled = false;

                    // help menu
                    mnAbout.Enabled = false;
                    #endregion update menu status

                    #region update toolbar status
                    cmdNew.Enabled = false;
                    cmdOpen.Enabled = false;
                    cmdSave.Enabled = false;
                    cmdSaveAs.Enabled = false;
                    cmdStart.Enabled = false;
                    cmdStop.Enabled = true;
                    cmdSwitchMode.Enabled = false;
                    tbClearProcessedFiles.Enabled = false;
                    cmdSaveLogFile.Enabled = false;
                    cmdClearLogs.Enabled = false;
                    #endregion update toolbar status
                }
                else
                {
                    bool hasWorkingSpace = this._workingSpace != null;
                    bool isModified = hasWorkingSpace && this._workingSpace.Modified == true;

                    #region update menu status
                    // file menu
                    mnNewScript.Enabled = true;
                    mnOpenScript.Enabled = true;
                    mnSaveScript.Enabled = false;
                    mnSaveAs.Enabled = true;
                    mnImport.Enabled = true;
                    mnExport.Enabled = File.Exists(this._workingSpace.FileName);

                    // script menu
                    mnRunScript.Enabled = true;
                    mnStopScript.Enabled = false;
                    mnSaveLogFile.Enabled = this._scriptExecutor.IsLogEmpty;
                    mnClearLogs.Enabled = this._scriptExecutor.IsLogEmpty;

                    // tools menu
                    mnSwitchMode.Enabled = true;
                    mnuEditScript.Enabled = true;
                    mnuExecScript.Enabled = true;

                    mnClearProcessedFiles.Enabled = true;

                    // help menu
                    mnAbout.Enabled = true;
                    #endregion update menu status

                    #region update toolbar status
                    cmdNew.Enabled = true;
                    cmdOpen.Enabled = true;
                    cmdSave.Enabled = false;
                    cmdSaveAs.Enabled = false;
                    cmdStart.Enabled = true;
                    cmdStop.Enabled = false;
                    cmdSwitchMode.Enabled = true;
                    tbClearProcessedFiles.Enabled = true;
                    cmdSaveLogFile.Enabled = this._scriptExecutor.IsLogEmpty;
                    cmdClearLogs.Enabled = this._scriptExecutor.IsLogEmpty;
                    #endregion update toolbar status
                }
            }
            else
            {
                if (bLock)
                {
                }
                else
                {
                    bool hasWorkingSpace = this._workingSpace != null;
                    bool isModified = hasWorkingSpace && this._workingSpace.Modified == true;

                    #region update menu status
                    // file menu
                    mnNewScript.Enabled = true;
                    mnOpenScript.Enabled = true;
                    mnSaveScript.Enabled = isModified;
                    mnSaveAs.Enabled = true;
                    mnImport.Enabled = true;
                    mnExport.Enabled = this._workingSpace != null && File.Exists(this._workingSpace.FileName);

                    // script menu
                    mnRunScript.Enabled = true;
                    mnStopScript.Enabled = false;

                    mnSaveLogFile.Enabled = false;
                    mnClearLogs.Enabled = false;

                    // tools menu
                    mnSwitchMode.Enabled = true;
                    mnuEditScript.Enabled = true;
                    mnuExecScript.Enabled = true;

                    mnClearProcessedFiles.Enabled = true;

                    // help menu
                    mnAbout.Enabled = true;
                    #endregion update menu status

                    #region update toolbar status
                    cmdNew.Enabled = true;
                    cmdOpen.Enabled = true;
                    cmdSave.Enabled = isModified;
                    cmdSaveAs.Enabled = true;
                    cmdStart.Enabled = true;
                    cmdStop.Enabled = false;
                    cmdSwitchMode.Enabled = true;
                    tbClearProcessedFiles.Enabled = true;
                    //cmdSaveLogFile.Enabled = this._scriptExecutor.IsLogEmpty;
                    //cmdClearLogs.Enabled = this._scriptExecutor.IsLogEmpty;
                    cmdSaveLogFile.Enabled = false;
                    cmdClearLogs.Enabled = false;
                    #endregion update toolbar status
                }
            }
        }

        private void UpdateWindowTitle()
        {
            if (this._workingSpace != null && this._workingSpace.FileName != null && this._workingSpace.FileName != string.Empty)
                this.Text = MainForm3.Title + " - " + this._workingSpace.FileName;
            else
                this.Text = MainForm3.Title + " - " + WorkingSpace.defaultTitle;
        }

        private void InitializeRecentMenu()
        {
            this.UpdateRecentMenuItems();
        }

        private string[] LoadRecentMenu()
        {
            try
            {
                RegistryKey software = Registry.CurrentUser.OpenSubKey(keySoftware);
                if (software == null)
                    return null;
                RegistryKey siglaz = software.OpenSubKey(keySiGlaz);
                if (siglaz == null)
                    return null;
                RegistryKey rde = siglaz.OpenSubKey(keySIA);
                if (rde == null)
                    return null;
                RegistryKey rdew = rde.OpenSubKey(keySIAW);
                if (rdew == null)
                    return null;
                RegistryKey recentScripts = rdew.OpenSubKey(keyRecentScripts);
                if (recentScripts == null)
                    return null;

                ArrayList result = new ArrayList();
                string[] names = recentScripts.GetValueNames();
                foreach (string name in names)
                {
                    string value = recentScripts.GetValue(name) as string;
                    if (value != null && value != string.Empty)
                        result.Add(value);
                }

                return result.ToArray(typeof(string)) as string[];

            }
            catch (System.Exception exp)
            {
                Trace.WriteLine(exp);
            }

            return null;
        }

        private void SaveRecentMenu(string[] items)
        {
            try
            {
                RegistryKey software = Registry.CurrentUser.OpenSubKey(keySoftware);
                if (software == null)
                    return;
                RegistryKey siglaz = software.OpenSubKey(keySiGlaz);
                if (siglaz == null)
                    return;
                RegistryKey rde = siglaz.OpenSubKey(keySIA, true);
                if (rde == null)
                    return;
                RegistryKey rdew = rde.OpenSubKey(keySIAW, true);
                if (rdew == null)
                    rdew = rde.CreateSubKey(keySIAW);

                RegistryKey recentScripts = rdew.OpenSubKey(keyRecentScripts, true);
                if (recentScripts == null)
                    recentScripts = rdew.CreateSubKey(keyRecentScripts);

                int index = 1;
                foreach (string item in items)
                {
                    recentScripts.SetValue((index++).ToString(), item);
                }
            }
            catch (System.Exception exp)
            {
                Trace.WriteLine(exp);
            }
        }

        private void AppendRecentScripts(string fileName)
        {
            int maxItemCount = 5;
            ArrayList newItems = new ArrayList();
            newItems.Add(fileName);
            string[] items = this.LoadRecentMenu();
            if (items != null)
            {
                for (int i = 0; i < maxItemCount - 1 && i < items.Length; i++)
                {
                    if (items[i] != fileName)
                        newItems.Add(items[i]);
                }
            }

            // save recent file to registry
            this.SaveRecentMenu(newItems.ToArray(typeof(string)) as string[]);

            // refresh menu items
            this.UpdateRecentMenuItems();
        }

        private void UpdateRecentMenuItems()
        {
            MenuItem parent = mnuRecentScripts;
            // clean up old menu items
            parent.MenuItems.Clear();

            // add new menu items
            string[] items = this.LoadRecentMenu();
            if (items != null && items.Length > 0)
            {
                int index = 1;

                foreach (string filePath in items)
                {
                    string text = string.Format("&{0}.{1}", index++, filePath);
                    MenuItem menuItem = new MenuItem(text);
                    menuItem.Click += new EventHandler(RecentMenuItem_Click);
                    parent.MenuItems.Add(menuItem);
                }
            }
        }

        #endregion Internal Helpers

        #region Script Monitor Handlers

        private ScriptMonitor _executor = null;

        private void RunScript()
        {
            try
            {
                Type baseType = typeof(IScriptMonitor);
                Type type = typeof(ScriptMonitor);
                bool retVal = baseType.IsAssignableFrom(type);

                // start thread or continue thread
                if (_executor == null)
                {
                    _executor = new ScriptMonitor();
                    _executor.UseMessageBufferLock = true;
                    _executor.SynchronousCallback = true;

                    _executor.StartProcessScript += new EventHandler(_executor_ThreadStarted);
                    _executor.StopProcessScript += new EventHandler(_executor_ThreadExited);
                    _executor.BeginProcessFile += new EventHandler(_executor_BeginProcessFile);
                    _executor.EndProcessFile += new EventHandler(_executor_EndProcessFile);
                    _executor.StatusBarChanged += new EventHandler(_executor_StatusBarChanged);
                    _executor.StartedScanningFolder += new EventHandler(_executor_StartedScanningFolder);
                    _executor.ScanningFolder += new EventHandler(_executor_ScanningFolder);
                    _executor.EndedScanningFolder += new EventHandler(_executor_EndedScanningFolder);
                }

                // update monitor frequency
                _executor.MonitorFrequency = _monitorFrequency;

                timerStatusUpdate.Enabled = true;                
                _executor.Start(this._workingSpace.FileName);

                // change UI to monitor mode
                this._scriptExecutor.SwitchToRun = false;
            }
            catch
            {
                throw;
            }
        }

        private void StopScript()
        {
            try
            {
                timerStatusUpdate.Enabled = false;
                // switch to stop mode
                this._scriptExecutor.SwitchToRun = true;

                if (_executor != null)
                {
                    _executor.Stop(100);
                }
            }
            catch
            {
                throw;
            }
        }


        protected virtual void OnStartProcessScript(EventArgs e)
        {
            if (this.IsDisposed)
                return;

            this._scriptExecutor.OnStartProcessScript((StartProcessScriptEventArgs)e);
        }

        protected virtual void OnStopProcessThread(EventArgs e)
        {
            if (this.IsDisposed)
                return;

            this._scriptExecutor.OnStopProcessScript((StopProcessScriptEventArgs)e);

            // unlock GUI
            this.UpdateGUIStatus(false);
        }

        protected virtual void OnBeginProcessFile(EventArgs args)
        {
            if (this.IsDisposed)
                return;

            // update processing file
            this._scriptExecutor.OnBeginProcessFile((BeginProcessFileEventArgs)args);
        }

        protected virtual void OnEndProcessFile(EventArgs args)
        {
            if (this.IsDisposed)
                return;

            // update processing file
            this._scriptExecutor.OnEndProcessFile((EndProcessFileEventArgs)args);
        }

        protected virtual void OnStatusChanged(EventArgs args)
        {
            if (this.IsDisposed)
                return;

            // update status
            this._scriptExecutor.OnStatusChanged((StatusChangedEventArgs)args);
        }

        private void _executor_StatusBarChanged(object sender, EventArgs args)
        {
            this.UpdateStatusBar((args as StatusBarChangedEventArgs).StatusText);
        }

        private void _executor_StartedScanningFolder(object sender, EventArgs args)
        {
            this._scriptExecutor.AppendStartedScanningFolder();
        }

        private void _executor_ScanningFolder(object sender, EventArgs args)
        {
            string folder = (args as ScanningFolderEventArgs).Folder;
            if (folder != null && folder != string.Empty)
            {
                this._scriptExecutor.AppendScanningFolder(folder);
                return;
            }
            string file = (args as ScanningFolderEventArgs).File;
            if (file == null || file == string.Empty)
                return;
            this._scriptExecutor.AppendIncommingFileToLogWindow(file);
        }

        private void _executor_EndedScanningFolder(object sender, EventArgs args)
        {
            this._scriptExecutor.AppendEndedScanningFolder();
        }

        private void UpdateStatusBar(string statusBarText)
        {
            if (this.statusBarPanelCurrentSubStep.Text != statusBarText)
                this.statusBarPanelCurrentSubStep.Text = statusBarText;
        }

        #region Executor event handlers

        private delegate void ThreadStarted(object sender, EventArgs e);
        private delegate void ThreadExited(object sender, EventArgs e);
        private delegate void BeginProcessFile(object sender, EventArgs e);
        private delegate void EndProcessFile(object sender, EventArgs e);
        private delegate void StatusChanged(object sender, EventArgs e);

        private void _executor_ThreadStarted(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
#if BUG_TRACE
				// BUG NOTE: trace thread information
				string msg = string.Format("{1}: Thread {0} enter _executor_ThreadStarted", Thread.CurrentThread.Name, DateTime.Now.ToString());
				Trace.WriteLine(msg);                      
#endif

                this.Invoke(new ThreadStarted(_executor_ThreadStarted), new object[] { sender, e });
            }
            else
            {
#if BUG_TRACE
				// BUG NOTE: trace thread information
				string msg = string.Format("{0}: Thread UI enter _executor_ThreadStarted", DateTime.Now.ToString());
				Trace.WriteLine(msg);
#endif

                this.OnStartProcessScript(e);
            }
        }

        private void _executor_ThreadExited(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
#if BUG_TRACE
				// BUG NOTE: trace thread information
				string msg = string.Format("{1}: Thread {0} enter _executor_ThreadExited", Thread.CurrentThread.Name, DateTime.Now.ToString());
				Trace.WriteLine(msg);                      
#endif

                this.Invoke(new ThreadExited(_executor_ThreadExited), new object[] { sender, e });
            }
            else
            {
#if BUG_TRACE
				// BUG NOTE: trace thread information
				string msg = string.Format("{0}: Thread UI enter _executor_ThreadExited", DateTime.Now.ToString());
				Trace.WriteLine(msg);
#endif

                this.OnStopProcessThread(e);
            }
        }

        private void _executor_StatusChanged(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
#if BUG_TRACE
				// BUG NOTE: trace thread information
				string msg = string.Format("{1}: Thread {0} enter _executor_StatusChanged", Thread.CurrentThread.Name, DateTime.Now.ToString());
				Trace.WriteLine(msg);                                                                       
#endif
                // Processes all Windows messages currently in the message queue
                Application.DoEvents();

                // start invoke status changed		
                this.Invoke(new StatusChanged(_executor_StatusChanged), new object[] { sender, e });
            }
            else
            {
#if BUG_TRACE
				// BUG NOTE: trace thread information
				string msg = string.Format("{0}: Thread UI enter _executor_StatusChanged", DateTime.Now.ToString());
				Trace.WriteLine(msg);     
#endif

                this.OnStatusChanged(e);
            }
        }

        private void _executor_StatusChangedForTimer(object sender, EventArgs e)
        {
            _statusArgEventArgs = (StatusChangedEventArgs)e;
        }

        private void _executor_BeginProcessFile(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
#if BUG_TRACE
				// BUG NOTE: trace thread information
				string msg = string.Format("{1}: Thread {0} enter _executor_BeginProcessFile", Thread.CurrentThread.Name, DateTime.Now.ToString());
				Trace.WriteLine(msg);                      
#endif

                this.Invoke(new BeginProcessFile(_executor_BeginProcessFile), new object[] { sender, e });
            }
            else
            {
#if BUG_TRACE
				// BUG NOTE: trace thread information
				string msg = string.Format("{0}: Thread UI enter _executor_BeginProcessFile", DateTime.Now.ToString());
				Trace.WriteLine(msg);
#endif

                this.OnBeginProcessFile(e);
            }
        }

        private void _executor_EndProcessFile(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
#if BUG_TRACE
				// BUG NOTE: trace thread information
				string msg = string.Format("{1}: Thread {0} enter _executor_EndProcessFile", Thread.CurrentThread.Name, DateTime.Now.ToString());
				Trace.WriteLine(msg);                      
#endif

                this.Invoke(new EndProcessFile(_executor_EndProcessFile), new object[] { sender, e });
            }
            else
            {
#if BUG_TRACE
				// BUG NOTE: trace thread information
				string msg = string.Format("{0}: Thread UI enter _executor_EndProcessFile", DateTime.Now.ToString());
				Trace.WriteLine(msg);
#endif

                this.OnEndProcessFile(e);
            }
        }

        #endregion

        private void StatusUpdateTimer_Tick(object sender, System.EventArgs e)
        {
            if (this._executor == null)
                return;

            StatusChangedEventArgs[] args = this._executor.ReadAndClearStatusChangeArguments();
            if (args != null && args.Length > 0)
            {
                foreach (StatusChangedEventArgs arg in args)
                    this.OnStatusChanged(arg);

            }
            else
            {
                this._executor.MessageBufferLock.Set();
            }
        }

        #endregion

        #region Script Factory helpers

        public static void InitializeScriptFactory()
        {
            ScriptFactory.Initialize();
        }

        public static void UninitializeScriptFactory()
        {
            ScriptFactory.Uninitialize();
        }

        #endregion

        #region Drag and Drop

        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            base.OnDragEnter(drgevent);

            if (drgevent.Data.GetDataPresent(DataFormats.FileDrop))
                drgevent.Effect = DragDropEffects.Copy;
            else
                drgevent.Effect = DragDropEffects.None;
        }

        protected override void OnDragOver(DragEventArgs drgevent)
        {
            base.OnDragOver(drgevent);
        }

        protected override void OnDragLeave(EventArgs e)
        {
            base.OnDragLeave(e);
        }

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            base.OnDragDrop(drgevent);

            if (drgevent.Data.GetDataPresent(DataFormats.FileDrop))
            {
                try
                {
                    Array files = (Array)drgevent.Data.GetData(DataFormats.FileDrop);
                    if (files != null && files.Length > 0)
                    {
                    }
                }
                catch (Exception exp)
                {
                    Trace.WriteLine(exp);
                }
            }
        }

        #endregion
    }
}
