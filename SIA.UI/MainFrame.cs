#define INVISIBLE_SCIENTECH_LIMITED_FUNCTIONS

using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

using SIA.Common;
using SIA.Common.Analysis;
using SIA.Common.KlarfExport;
using SIA.Common.KlarfExport.BinningLibrary;
using SIA.Common.Mathematics;
using SIA.Common.Utility;
using SIA.Common.Imaging;

using SIA.UI.Controls;
using SIA.UI.Controls.Common;
using SIA.UI.Controls.Utilities;

using SIA.UI.Helpers;

using SIA.Plugins.Common;
using SIA.Plugins;

using SIA.UI;
using SIA.UI.Dialogs;
using SIA.UI.CommandHandlers;
using SIA.SystemLayer;
using SIA.UI.Controls.Helpers;
using SIA.UI.CommandHandlers.Analysis;
using SIA.Algorithms.FeatureProcessing.Helpers;
using SiGlaz.Common.ABSInspectionSettings;
using SiGlaz.Common;
using SIA.UI.Components;
using SiGlaz.UI.CustomControls.DockBar;
using SiGlaz.UI.CustomControls;
using SIA.UI.CommandHandlers.Tools;


namespace SIA.UI
{
	/// <summary>
	/// Summary description for appWorkspace.
	/// </summary>
	internal class MainFrame 
        : AppWorkspace
	{
		#region Fields

		private SIAMainMenu _mainMenu;
		private DlgHistory _dlgHistory;
		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Panel panelContainer;
		private System.Windows.Forms.Panel panelShortcutBar;
		private System.Windows.Forms.Panel panelWorkspace;
		private System.Windows.Forms.Splitter splitter1;
		
		private string[] _arguments = null;

		#endregion

		#region Events

		public event EventHandler PreDispatchCommand = null;
		public event EventHandler PostDispatchCommand = null;

		#endregion

		#region Constructor and destructor

		public MainFrame(string[] args)
		{
			this._arguments = args;

			// Initialize dependencies
			this.Initialize();
		}

		public MainFrame()
		{
			this._arguments = null;

			// Initialize dependencies
			this.Initialize();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			this.Uninitialize();

			if( disposing )
			{
				if (components != null)
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainFrame));
			this._mainMenu = new SIA.UI.Controls.SIAMainMenu(this);
			this.panelContainer = new System.Windows.Forms.Panel();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.panelWorkspace = new System.Windows.Forms.Panel();
			this.panelShortcutBar = new System.Windows.Forms.Panel();
			this.panelContainer.SuspendLayout();
			this.SuspendLayout();			
			// 
			// panelContainer
			// 
			//this.panelContainer.Controls.Add(this.splitter1);
			this.panelContainer.Controls.Add(this.panelWorkspace);
            this.panelContainer.Controls.Add(splitterEx);
            this.panelContainer.Controls.Add(explorerBarContainer);
			//this.panelContainer.Controls.Add(this.panelShortcutBar);
            //this.panelContainer.Controls.Add(dockBar);
			this.panelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelContainer.Location = new System.Drawing.Point(0, 20);
			this.panelContainer.Name = "panelContainer";
			this.panelContainer.Size = new System.Drawing.Size(872, 505);
			this.panelContainer.TabIndex = 1;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(150, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 505);
			this.splitter1.TabIndex = 2;
			this.splitter1.TabStop = false;
			// 
			// panelWorkspace
			// 
			this.panelWorkspace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWorkspace.BorderStyle = BorderStyle.Fixed3D;
			this.panelWorkspace.Location = new System.Drawing.Point(150, 0);
			this.panelWorkspace.Name = "panelWorkspace";
			this.panelWorkspace.Size = new System.Drawing.Size(722, 505);            
			this.panelWorkspace.TabIndex = 1;
			// 
			// panelShortcutBar
			// 
			this.panelShortcutBar.Dock = System.Windows.Forms.DockStyle.Left;
			this.panelShortcutBar.Location = new System.Drawing.Point(0, 0);
			this.panelShortcutBar.Name = "panelShortcutBar";
			this.panelShortcutBar.Size = new System.Drawing.Size(180, 505);
			this.panelShortcutBar.TabIndex = 0;			
			// 
			// appWorkspace
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(872, 525);
			//this.Controls.Add(this._statusBar);            
			this.Controls.Add(this.panelContainer);
            this.Controls.Add(_statusBarEx);
			//this.Controls.Add(this._mainToolBar);
            this.Controls.Add(_toolBarEx);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this._mainMenu;
			this.Name = "appWorkspace";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "appWorkspace";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.panelContainer.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		#endregion

		#region appWorkspace Handlers

		private void Initialize()
		{
            InitializeMainToolBarEx();

            InitializeStatusBarEx();
            
            InitializeExplorerBar();
            

			// load configuration data
			string filePath = Application.StartupPath + "\\App.cfig";
			CustomConfiguration.LoadData(filePath);

            // initialize command dispatcher
            this.InitializeCommandDispatcher();

            // initialize plugin manager
            this.InitializePluginManager();

            // intialize dock bar control
            InitializeDockBar();

            // Required for Windows Form Designer support
            InitializeComponent();

            // allow drag and drop
            this.AllowDrop = true;

            // Initialize document workspace
            this.InitializeWorkspace();
                        
            // Initialize main menu
			this.InitializeMainMenu();			

			// Initialize toolbar
			this.InitializeMainToolBar();

			// Initialize status bar
			this.InitializeStatusBar();

			// Initialize shortcut bar
			//this.InitializeShortcutBar();
            this.InitializeShortcutBarEx();

            // Initialize history window
            this.InitializeHistoryWindow();
        }

		private void Uninitialize()
		{
            // Initialize history window
            this.UninitializeHistoryWindow();

            // Uninitialize shortcut bar
			//this.UninitializeShortcutBar();
            this.UninitializeShortcutBarEx();

			// Uninitialize status bar
			this.UninitializeStatusBar();

			// Uninitialize toolbar
			this.UninitializeMainToolBar();

			// Uninitialize main menu
			this.UninitializeMainMenu();

            // Uninitialize document workspace
            this.UninitializeWorkspace();			

            // Uninitialize plugin manager
			this.UninitializePluginManager();

            // Uninitialize command dispatcher
			this.UninitializeCommandDispatcher();

			// save configuration data
			string filePath = Application.StartupPath + "\\App.cfig";
			SIA.Common.Utility.CustomConfiguration.SaveData(filePath);
		}


		protected override void OnDragEnter(DragEventArgs drgevent)
		{
			base.OnDragEnter (drgevent);

			if (drgevent.Data.GetDataPresent(DataFormats.FileDrop))
				drgevent.Effect = DragDropEffects.Copy;
			else
				drgevent.Effect = DragDropEffects.None;
		}

		protected override void OnDragOver(DragEventArgs drgevent)
		{
			base.OnDragOver (drgevent);
		}

		protected override void OnDragLeave(EventArgs e)
		{
			base.OnDragLeave (e);
		}

		protected override void OnDragDrop(DragEventArgs drgevent)
		{
			base.OnDragDrop (drgevent);

			if (drgevent.Data.GetDataPresent(DataFormats.FileDrop))
			{
				try
				{
					Array files = (Array)drgevent.Data.GetData(DataFormats.FileDrop);
					if (files != null && files.Length > 0)
					{
						string filePath = files.GetValue(0) as string;
						if (File.Exists(filePath))
							this.PostCommand(CmdFileOpenRecentFiles.cmdCommandKey, filePath);
					}
				}
				catch (Exception exp)
				{
					Trace.WriteLine(exp);
				}
			}
		}

		#endregion

		#region Main Menu Handlers
        
		private void InitializeMainMenu()
		{
			if (this._mainMenu != null)
				this._mainMenu.Dispose();

			this._mainMenu = new SIAMainMenu(this);
			this.Menu = this._mainMenu;            
			
			// rebuild main menu items
			this._mainMenu.RebuildMenuItems();

            OwnerDrawmenuItem.MainMenuBackColor = 
                Color.FromArgb(
                ((int)(((byte)(136)))), 
                ((int)(((byte)(174)))), 
                ((int)(((byte)(228)))));

			// register for event handler
			this._mainMenu.MenuItemClicked += new EventHandler(MainMenu_MenuItemClicked);
		}

		private void UninitializeMainMenu()
		{
			if (this._mainMenu != null)
			{
				this.Menu = null;
				this._mainMenu.MenuItemClicked -= new EventHandler(MainMenu_MenuItemClicked);
				this._mainMenu.Dispose();
				this._mainMenu = null;
			}
		}

		private void MainMenu_MenuItemClicked(object sender, EventArgs e)
		{
			MenuItemEx menuItem = sender as MenuItemEx;
			if (menuItem != null && menuItem.Command != null)
				this.DispatchCommand(menuItem.Command, menuItem.Arguments);
		}

		#endregion

		#region Main Toolbar Handlers

		private void InitializeMainToolBar()
		{
            _toolbarManager = new SIAToolBarManager(this, _toolBarEx);
            _toolbarManager.RebuildItems();
                       
            //this.SuspendLayout();

            //if (this._mainToolBar != null)
            //    this._mainToolBar.Dispose();
			
            //this._mainToolBar = new SIAToolBar(this);
            ////this.Controls.Add(this._mainToolBar);			
            //this._mainToolBar.Location = Point.Empty;
            //this._mainToolBar.Size = new Size(this.Width, this._mainToolBar.ButtonSize.Height);
            //this._mainToolBar.Name = "__mainToolBar";
            //this._mainToolBar.TabIndex = 0;
            //this._mainToolBar.Dock = DockStyle.Top;
			
            //// rebuild tool bar items
            ////this._mainToolBar.RebuildItems();

            //// regist for event handler
            //this._mainToolBar.ButtonClick += new ToolBarButtonClickEventHandler(MainToolBar_ButtonClick);

            //this.ResumeLayout(false);
		}

		private void UninitializeMainToolBar()
		{
		}
		#endregion

        #region Main Toolbar Handlers Ex
        private ToolBarEx _toolBarEx = null;
        private SIAToolBarManager _toolbarManager = null;
        private void InitializeMainToolBarEx()
        {
            _toolBarEx = new ToolBarEx();
            _toolBarEx.Dock = DockStyle.Top;
            _toolBarEx.Height = 24;
            _toolBarEx.ItemClicked += new ToolBarExEventHandlers(_toolBarEx_ItemClicked);            
        }

        void _toolBarEx_ItemClicked(object sender, ToolBarExEventArgs e)
        {
            if (e == null || e.Item == null)
                return;

            if (e.Item.Tag != null && e.Item.Tag is IUICommandHandler)
            {
                IUICommandHandler handler = e.Item.Tag as IUICommandHandler;
                this.DispatchCommand(handler.Command);
            }
        }
        #endregion Main Toolbar Handlers Ex

        #region Shortcuts Bar Handlers

        private void InitializeShortcutBar()
		{           
		}

		private void UninitializeShortcutBar()
		{           
		}        
        
		#endregion

        #region Dock Bar
        private ucDockbar dockBar = null;
        private void InitializeDockBar()
        {
            dockBar = new ucDockbar();
            dockBar.Dock = DockStyle.Left;
            dockBar.Width = 120;
        }

        private void UninitializeDockBar()
        {
            
        }


        private SIAExplorerBarManager explorerBarManager = null;
        private void InitializeShortcutBarEx()
        {
            explorerBarManager = new SIAExplorerBarManager(
                this, explorerBarContainer, splitterEx);
            explorerBarManager.ItemClicked += new ExplorerBarEventHandler(explorerBarManager_ItemClicked);
            
            //if (this._shortcutsBar != null)
            //    this._shortcutsBar.Dispose();

            //this._shortcutsBar = new SIAShortcutBar(this);
            //this._shortcutsBar.Dock = DockStyle.Fill;
            //this._shortcutsBar.ItemClicked += new OutlookBarItemClickedHandler(ShortcutsBar_ItemClicked);

            //// rebuild shortcut bar items
            //this._shortcutsBar.RebuildShortcuts();

            //this.dockBar.SuspendLayout();
            //this.dockBar.PanelContainer.Controls.Add(_shortcutsBar);
            //this.dockBar.ResumeLayout(true);

            explorerBarManager.RebuildShortcuts();
        }

        void explorerBarManager_ItemClicked(object sender, ExplorerBarEventArgs e)
        {
            if (e == null || e.Item == null)
                return;

            if (e.Item.Tag != null && e.Item.Tag is IUICommandHandler)
            {
                IUICommandHandler handler = e.Item.Tag as IUICommandHandler;
                this.DispatchCommand(handler.Command);
            }
        }

        private void UninitializeShortcutBarEx()
        {
            //if (this._shortcutsBar != null)
            //{
            //    this.dockBar.PanelContainer.Controls.Remove(this._shortcutsBar);
            //    this._shortcutsBar.Dispose();
            //    this._shortcutsBar = null;
            //}
        }  
        #endregion Dock Bar

        #region Explorer Bar
        private ExplorerBarContainer explorerBarContainer = null;
        private SplitterEx splitterEx = null;
        private void InitializeExplorerBar()
        {
            if (explorerBarContainer != null)
                explorerBarContainer.Dispose();
            explorerBarContainer = new ExplorerBarContainer();
            explorerBarContainer.Dock = DockStyle.Left;
            explorerBarContainer.Width = 240;
            explorerBarContainer.ExplorerBar.AllowDrag = false;

            if (splitterEx != null)
                splitterEx.Dispose();
            splitterEx = new SplitterEx();
            splitterEx.Dock = DockStyle.Left;
        }

        private void UninitializeExplorerBar()
        {            
        } 
        #endregion Explorer Bar

        #region Status Bar Handlers

        private void InitializeStatusBar()
		{
            _statusBarEx.RegisterImageViewerEvents();
		}

		private void UninitializeStatusBar()
		{
            return;
		}

        private SIAStatusBarEx _statusBarEx = null;
        private void InitializeStatusBarEx()
        {
            _statusBarEx = new SIAStatusBarEx(this);
            _statusBarEx.Height = 24;
            _statusBarEx.Dock = DockStyle.Bottom;
            _statusBarEx.Text = "Ready";
        }
		#endregion

		#region History Windows Handlers

		private void InitializeHistoryWindow()
		{
			if (this._dlgHistory != null)
				this._dlgHistory.Dispose();

			this._dlgHistory = new DlgHistory(this);
			this._dlgHistory.Owner = this;
			this._dlgHistory.TopLevel = true;
		}

		private void UninitializeHistoryWindow()
		{
			if (this._dlgHistory != null)
			{
				if (this._dlgHistory.IsDisposed)
					this._dlgHistory.Close();
				this._dlgHistory.Dispose();
				this._dlgHistory = null;
			}
		}

		#endregion

		#region Workspace Handlers

		private ImageWorkspace _workspace;

		public ImageWorkspace ImageView
		{
			get {return _workspace;}
		}

		private void InitializeWorkspace()
		{
			// initialize trebyshevt lookup table
            CommonImage.loadTrebyshevtLookup();            

			panelWorkspace.SuspendLayout();
			// initialize image view control
			this._workspace = new SIA.UI.Controls.ImageWorkspace(this);
			this._workspace.Name = "_workspace";
			this._workspace.Dock = DockStyle.Fill;
			this.panelWorkspace.Controls.Add(_workspace);
			panelWorkspace.ResumeLayout(true);
		}

		private void UninitializeWorkspace()
		{
			// remove control
			if (this._workspace != null)
			{
				panelWorkspace.Controls.Remove(this._workspace);
				this._workspace.Dispose();
				this._workspace = null;
			}
		}

		#endregion

		#region Command dispatcher		

		private ManualResetEvent _exitHandle = null;
		private System.Windows.Forms.Timer _timer = null;
		private CommandList _commandList = new CommandList();
		private CommandQueue _commandQueue = new CommandQueue();

		public override ICommandHandler[] Commands
		{
			get
			{
				return _commandList.CommandHandlers;
			}
		}
		
		private void InitializeCommandDispatcher()
		{
			this.RegisterCommandHandler(new CmdFileOpenImage(this));
			this.RegisterCommandHandler(new CmdFileSaveImage(this));
			this.RegisterCommandHandler(new CmdFileSaveImageAs(this));
			this.RegisterCommandHandler(new CmdFileCloseImage(this));
			this.RegisterCommandHandler(new CmdFileProperties(this));
			//this.RegisterCommandHandler(new CmdFilePrintImage(this));
			this.RegisterCommandHandler(new CmdFileOpenRecentFiles(this));
			this.RegisterCommandHandler(new CmdFileExitApplication(this));
			this.RegisterCommandHandler(new CmdFileSetDefaultFolder(this));

			this.RegisterCommandHandler(new CmdEditUndo(this));
			this.RegisterCommandHandler(new CmdEditRedo(this));
			this.RegisterCommandHandler(new CmdEditCopy(this));
			this.RegisterCommandHandler(new CmdEditPaste(this));
			this.RegisterCommandHandler(new CmdEditCustomize(this));

			this.RegisterCommandHandler(new CmdViewLineProfile(this));
			this.RegisterCommandHandler(new CmdViewBoxProfile(this));
			this.RegisterCommandHandler(new CmdView3DAreaPlot(this));
			this.RegisterCommandHandler(new CmdViewScreenStretch(this));
			this.RegisterCommandHandler(new CmdViewIntensityHistogram(this));
			this.RegisterCommandHandler(new CmdViewPseudoColor(this));
			this.RegisterCommandHandler(new CmdViewFitOnScreen(this));
			this.RegisterCommandHandler(new CmdViewActualSize(this));
			this.RegisterCommandHandler(new CmdViewCustomZoom(this));
			this.RegisterCommandHandler(new CmdViewZoomIn(this));
			this.RegisterCommandHandler(new CmdViewZoomOut(this));
			this.RegisterCommandHandler(new CmdViewPan(this));
			this.RegisterCommandHandler(new CmdViewSelectMode(this));
			this.RegisterCommandHandler(new CmdViewToolBar(this));
			this.RegisterCommandHandler(new CmdViewStatusBar(this));
			this.RegisterCommandHandler(new CmdViewShortcutBar(this));
			this.RegisterCommandHandler(new CmdViewHistory(this));

			this.RegisterCommandHandler(new CmdProcessCalculation(this));
			this.RegisterCommandHandler(new CmdProcessGlobalBackgroundCorrection(this));
			this.RegisterCommandHandler(new CmdProcessExtractGlobalBackground(this));
			this.RegisterCommandHandler(new CmdProcessThreshold(this));
			this.RegisterCommandHandler(new CmdProcessStretchColor(this));
			//this.RegisterCommandHandler(new CmdProcessCameraCorrection(this));
			this.RegisterCommandHandler(new CmdProcessInvert(this));
			this.RegisterCommandHandler(new CmdProcessHistogramEqualization(this));

			this.RegisterCommandHandler(new CmdProcessResize(this));
			this.RegisterCommandHandler(new CmdProcessFlipRotate(this));

			this.RegisterCommandHandler(new CmdFiltersKernelFilters(this));
			this.RegisterCommandHandler(new CmdFiltersFourierTransform(this));
			this.RegisterCommandHandler(new CmdFiltersRank(this));
			this.RegisterCommandHandler(new CmdFiltersSmooth(this));
			this.RegisterCommandHandler(new CmdFiltersGaussian(this));
            this.RegisterCommandHandler(new CmdFilterWiener(this));
            
            this.RegisterCommandHandler(new CmdAnalysisDetectCoordinateSystem(this));

            this.RegisterCommandHandler(new CmdAnalysisDetectAnomalies(this));

            
            this.RegisterCommandHandler(new CmdAnalysisCombineObject(this));            
            this.RegisterCommandHandler(new CmdAnalysisAdvancedObjectFilter(this));

			this.RegisterCommandHandler(new CmdAnalysisObjectList(this));
            this.RegisterCommandHandler(new CmdAnalysisToggleDetectedObjects(this));
			this.RegisterCommandHandler(new CmdAnalysisToggleSelectedObjects(this));
            

            
            this.RegisterCommandHandler(new CmdToolsCreateReferenceFile(this));
            this.RegisterCommandHandler(new CmdToolsCoordinateSystemOrigin(this));
            this.RegisterCommandHandler(new CmdToolsCoordinateSystemMarker(this));
            this.RegisterCommandHandler(new CmdToolsCoordinateSystemCalibration(this));
            this.RegisterCommandHandler(new CmdToolsMeasurement(this));

            this.RegisterCommandHandler(new CmdToolsCreateReferenceImage(this));

            this.RegisterCommandHandler(new CmdToolsOverlayDefectOnImage(this));
            this.RegisterCommandHandler(new CmdToolsExportObjectListToFile(this));

			this.RegisterCommandHandler(new CmdHelpIndex(this));
			this.RegisterCommandHandler(new CmdHelpAbout(this));

			// initialize exit wait handle
			_exitHandle = new ManualResetEvent(false);
			
			// initialize timer for queue processing
			if (this.components == null)
				this.components = new Container();
			this._timer = new System.Windows.Forms.Timer(this.components);
			this._timer.Interval = 500;
			this._timer.Tick += new EventHandler(this.Timer_Tick);
			this._timer.Enabled = true;
		}

		private void UninitializeCommandDispatcher()
		{
			// release wait handle
			if (_exitHandle != null)
				_exitHandle.Close();
			_exitHandle = null;

			// release timer
			if (this._timer != null)
				this._timer.Dispose();
			this._timer = null;
		}


		public override void PostCommand(string command, params object[] args)
		{
			lock(_commandQueue.SyncRoot)
				_commandQueue.Enqueue(command, args);

			_exitHandle.Reset();
		}

        public override void DispatchCommand(String command, params object[] args)
		{
			lock(_commandQueue.SyncRoot)
				_commandQueue.Enqueue(command, args);
			_exitHandle.Reset();

			this.ProcessCommandQueue();
		}

		protected virtual void OnPreDispatchCommand()
		{
			if (this.PreDispatchCommand != null)
				this.PreDispatchCommand(this, EventArgs.Empty);
		}

		protected virtual void OnPostDispatchCommand()
		{
			if (this.PostDispatchCommand != null)
				this.PostDispatchCommand(this, EventArgs.Empty);
		}

		
		protected virtual void RegisterCommandHandler(ICommandHandler handler)
		{
			if (this._commandList[handler.Command] != null)
				Trace.WriteLine(String.Format("The command {0} is already registered. It will be override.", handler.Command));
			this._commandList.Add(handler);
		}

		protected virtual void UnregisterCommandHandler(ICommandHandler handler)
		{
			if (this._commandList[handler.Command] == null)
				Trace.WriteLine(String.Format("The command {0} is not registered yet.", handler.Command));
			this._commandList.Remove(handler);
		}

		
		public void WaitForFinish()
		{
			if (_exitHandle == null)
				return;
			int count = 0;

			do 
			{
				lock (_commandQueue.SyncRoot)
					count = _commandQueue.Count;
				_exitHandle.WaitOne(WaitHandle.WaitTimeout, false);
			} while (count > 0);
		}

		private void Timer_Tick(object sender, System.EventArgs e)
		{
			if (this.IsDisposed == false)
				this.ProcessCommandQueue();
		}

        public void DoCommandOpenFile(string commandId)
        {
            try
            {
                ICommandHandler handler = this._commandList[commandId];
                if (handler != null)
                {
                    try
                    {                        
                        // execute command
                        handler.DoCommand();
                    }
                    finally
                    {                        
                    }
                }
            }
            catch
            {
            }
        }

        public void DoCommandOpenFile(string commandId, MemoryStream fs, string filePath)
        {
            try
            {
                ICommandHandler handler = this._commandList[commandId];
                if (handler != null)
                {
                    try
                    {
                        // execute command
                        (handler as CmdFileOpenImage).DoCommandOpenFromMemoryStream(fs, filePath);
                    }
                    finally
                    {
                    }
                }
            }
            catch
            {
            }
        }

        public void DoCommandClearMask(string commandId, string filePath)
        {
            try
            {
                ICommandHandler handler = this._commandList[commandId];
                if (handler != null)
                {
                    try
                    {
                        // execute command
                        handler.DoCommand(null);
                    }
                    finally
                    {
                    }
                }
            }
            catch
            {
            }
        }

        public void DoCommandNewMask(string commandId)
        {
            try
            {
                ICommandHandler handler = this._commandList[commandId];
                if (handler != null)
                {
                    try
                    {
                        // execute command
                        handler.DoCommand(null);
                    }
                    finally
                    {
                    }
                }
            }
            catch
            {
            }
        }

        public void DoCommandLoadMask(string commandId, string filePath)
        {
            try
            {
                ICommandHandler handler = this._commandList[commandId];
                if (handler != null)
                {
                    try
                    {
                        // execute command
                        (handler as CmdToolsLoadMask).DoCommandLoadMaskFromFile(filePath);
                    }
                    finally
                    {
                    }
                }
            }
            catch
            {
            }
        }

        public void DoCommandLoadMask(string commandId, byte[] maskData)
        {
            try
            {
                ICommandHandler handler = this._commandList[commandId];
                if (handler != null)
                {
                    try
                    {
                        // execute command
                        (handler as CmdToolsLoadMask).DoCommandLoadMaskFromMaskData(maskData);
                    }
                    finally
                    {
                    }
                }
            }
            catch
            {
            }
        }

		private void ProcessCommandQueue()
		{
			const int defaultWait = 100;

			try
			{
				if (Monitor.TryEnter(_commandQueue.SyncRoot, defaultWait))
				{
					try
					{
						if (_commandQueue.Count > 0)
						{
							CommandQueueItem cmdItem = null;
							lock (_commandQueue)
								cmdItem = _commandQueue.Dequeue();

							ICommandHandler handler = this._commandList[cmdItem.Command];
							if (handler != null)
							{
								try
								{
									// raise pre-dispatch command event
									this.OnPreDispatchCommand();

									// execute command
									handler.DoCommand(cmdItem.Arguments);
								}
								finally
								{
									// raise post-dispatch command event
									this.OnPostDispatchCommand();
								}
							}
						} 
					}
					finally
					{
						if (_exitHandle != null)
							_exitHandle.Set();
					}
				}					
			}
			finally
			{
				if (_commandQueue != null)
					Monitor.Exit(_commandQueue.SyncRoot);
			}
		}
		
		#endregion

		#region IAppWorkspace Members

		private PluginManager _pluginManager;

        public override IDocWorkspace DocumentWorkspace
        {
            get { return this._workspace; }
        }

        public override IPluginManager PluginManager
		{
			get {return _pluginManager;}
		}

        public override MainMenu MainMenu
		{
			get {return _mainMenu;}
		}

        public override ToolBarEx MainToolBar
        {
            get { return _toolBarEx; }
        }

        //public SIAShortcutBar ShortcutBar
        //{
        //    get {return _shortcutsBar;}
        //}


        public SIAStatusBarEx StatusBar
        {
            get { return _statusBarEx; }
        }

		public DlgHistory HistoryWindow
		{
			get {return _dlgHistory;}
		}
		
		#endregion

		#region Plugin Manager

		private void InitializePluginManager()
		{
			try
			{
				// create a instance of plugin manager
				_pluginManager = new PluginManager(this);
				// register for events
				_pluginManager.PluginLoaded += new EventHandler(PluginManager_PluginLoaded);
				_pluginManager.PluginUnloaded += new EventHandler(PluginManager_PluginUnloaded);
				// keep load settings from xml for backward compatible
				string fileName = this.GetConfigFile();
				_pluginManager.LoadConfiguration(fileName);
			}
			catch (System.Exception e)
			{
				Trace.WriteLine("Failed to initialize plugin manager: " + e.Message);
			}
		}

		private void UninitializePluginManager()
		{
			try
			{
				_pluginManager.SaveConfiguration();
			}
			catch (System.Exception e)
			{
				Trace.WriteLine("Failed to uninitialized plugin manager: " + e.Message);
			}
		}

		private string GetConfigFile()
		{
            string pluginCfgFile = ConfigurationManager.AppSettings["PluginCfg"];
			if (pluginCfgFile == null || File.Exists(pluginCfgFile) == false)
			{
				string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
				pluginCfgFile = String.Format(@"{0}\{1}", folder, SIA.Plugins.PluginManager.defaultConfigFile);
			}
			return pluginCfgFile;
		}

		private void PluginManager_PluginLoaded(object sender, EventArgs e)
		{
			PluginLoadedEventArgs args = e as PluginLoadedEventArgs;
			if (args != null && args.Plugin != null && args.Plugin.CommandHandlers != null)
			{
				ICommandHandler[] handlers = args.Plugin.CommandHandlers;
				if (handlers.Length > 0)
				{
					foreach (ICommandHandler handler in handlers)
						this.RegisterCommandHandler(handler);
				}
			}
		}

		private void PluginManager_PluginUnloaded(object sender, EventArgs e)
		{
			PluginUnloadedEventArgs args = e as PluginUnloadedEventArgs;
			if (args != null && args.Plugin != null && args.Plugin.CommandHandlers != null)
			{
				ICommandHandler[] handlers = args.Plugin.CommandHandlers;
				if (handlers.Length > 0)
				{
					foreach (ICommandHandler handler in handlers)
						this.UnregisterCommandHandler(handler);
				}
			}
		}

		#endregion

		#region Windows Form Handlers

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			// update ui element for first load
			this.OnUpdateUI();

			// check for argument is a file and then open it
			if (this._arguments != null && this._arguments.Length > 0)
			{
				string str = _arguments[0];
				if (File.Exists(str))
					this.DispatchCommand(CmdFileOpenRecentFiles.cmdCommandKey, str);
			}

#if DEBUG
            // open recent image
            //this.DispatchCommand(CmdFileOpenRecentFiles.cmdCommandKey, @"c:\Data\2008-11-26\13_bf_090.1_05.00_16_330c2e1b-f3d2-431c-a6ca-9d6c671e447c20081123.fit");
            //this.DispatchCommand(CmdToolsToggleWaferLayout.cmdCommandKey);
            //// launch die mask editor
            //this.DispatchCommand("CmdDieMaskEditor");
            
            //this.DispatchCommand(CmdFileOpenRecentFiles.cmdCommandKey, @"c:\Data\2008-12-12\background correction\rde-output.fits");
            //this.DispatchCommand("CmdSubtractUsingGoldenImage");            
#endif
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing (e);
			
			// dispatch close image command
			object[] output = new object[] {false};
			this.DispatchCommand(CmdFileCloseImage.cmdCommandKey, output);
			
			// cancel the form closing when user is continue
			e.Cancel = (bool)output[0];
		}


		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed (e);
		}


		#endregion

		#region Methods

		public override void UpdateUI()
		{
			this.OnUpdateUI();
		}

		#endregion

		#region Overridable routines

        protected virtual void OnUpdateUI()
		{
			// update main frame title
			string filePath = this.ImageView.FilePath;
			if (filePath == null || filePath.Length == 0)
				filePath = "Empty";
			if (filePath == "") // (No image file has been loaded)";
				this.Text = ResourceHelper.ApplicationName;
			else
				this.Text = String.Format("{0} - {1}", ResourceHelper.ApplicationName, filePath);

			this.OnUpdateMenuItems();
			this.OnUpdateToolBarButtons();
			this.OnUpdateStatusBar();
			this.OnUpdateShortcutBar();
		}

		protected virtual void OnUpdateMenuItems()
		{
			this._mainMenu.RefreshMenuItemStatus();
		}

		protected virtual void OnUpdateToolBarButtons()
		{
			if (_toolbarManager != null)
            {
                this._toolbarManager.RefreshButtonStatus();
            }
		}

		protected virtual void OnUpdateShortcutBar()
		{
            if (this.explorerBarManager != null)
                this.explorerBarManager.RefreshItemStatus();
		}

		protected virtual void OnUpdateStatusBar()
		{
            if (this._statusBarEx != null)
			    this._statusBarEx.UpdateData();
		}

		protected virtual void OnUpdateStatusBar(PointF mouseLoc, PointF waferLoc, int intensity)
		{
            if (this._statusBarEx != null)
                this._statusBarEx.UpdateData(mouseLoc, waferLoc, intensity);
		}		
		#endregion        

        #region ABS Inspection
        public void InitEngineWorkspace()
        {
            //InitABSInspectionWorkspace();
        }

        public void InitABSInspectionWorkspace()
        {
            try
            {
                ContaminationTexturalInfoHelper.Init();

#if DEBUG
                string filePath = Path.Combine(
                    SettingsHelper.GetDefaultABSInspectionSettingsFolderPath(), ContaminationsDetectionSettings.DefaultFileName);
                if (!File.Exists(filePath))
                {
                    ContaminationsDetectionSettings settings = new ContaminationsDetectionSettings();
                    settings.Serialize(filePath);
                }

                TexturalExtractorSettings texuralExtractorSettings = LinePatternTexturalInfoHelper.Settings;
#endif
            }
            catch
            {
                // nothing
            }
        }
        #endregion ABS Inspection
    }
}
