using System;
using System.Collections.Generic;
using System.Text;
using SiGlaz.UI.CustomControls;
using System.Collections;
using SIA.Plugins.Common;
using System.Diagnostics;
using SIA.Plugins;

namespace SIA.UI.Controls
{
    internal class SIAExplorerBarManager
    {
        #region Member fields
        private ExplorerBarContainer _explorerBarContainer = null;
        private SplitterEx _splitter = null;
        private Hashtable _lookupTable = null;
        private MainFrame _appWorkspace = null;

        private IExplorerBarTheme _theme = null;

        private ExplorerItemsGroup _catView = null;
        private ExplorerItemsGroup _catProcess = null;
        private ExplorerItemsGroup _catFilter = null;
        private ExplorerItemsGroup _catAnalysis = null;
        private ExplorerItemsGroup _catTools = null;
        private List<ExplorerItemsGroup> _groups = new List<ExplorerItemsGroup>(5);

        public event ExplorerBarEventHandler ItemClicked;
        #endregion Member fields

        #region Properties
        public ExplorerBarContainer ExplorerBarContainer
        {
            get
            {
                return _explorerBarContainer;
            }
        }

        public ExplorerBar ExplorerBar
        {
            get
            {
                if (_explorerBarContainer == null)
                    return null;

                return _explorerBarContainer.ExplorerBar;
            }
        }
        #endregion Properties

        #region Constructors
        public SIAExplorerBarManager(
            MainFrame owner, ExplorerBarContainer explorerBarContainer, SplitterEx splitter)
        {
            _appWorkspace = owner;
            _explorerBarContainer = explorerBarContainer;
            _splitter = splitter;

            _splitter.ControlToHide = _explorerBarContainer;

            Initialize();
        }

        private void Initialize()
        {
            InitializeExplorerBarProperties();

            InitializeCategories();

            if (_appWorkspace != null)
            {                
                this._appWorkspace.Load += 
                    new EventHandler(appWorkspace_Load);

                this._appWorkspace.PostDispatchCommand += 
                    new EventHandler(appWorkspace_PostDispatchCommand);

                // register for event handlers
                IPluginManager pluginMgr = this._appWorkspace.PluginManager;
                pluginMgr.PluginLoaded += new EventHandler(pluginMgr_PluginLoaded);
                pluginMgr.PluginUnloaded += new EventHandler(pluginMgr_PluginUnloaded);
            }

            // register for event handlers
            _explorerBarContainer.AutoHideButtonClicked += 
                new EventHandler(AutoHideButtonClicked);

            this._splitter.SplitterMoved += 
                new System.Windows.Forms.SplitterEventHandler(this.SplitterMoved);
        }

        private void InitializeExplorerBarProperties()
        {
            _theme = new ExplorerBarLunaBlueTheme();
            this.ExplorerBar.Theme = _theme;
            this.ExplorerBar.AllowMultiExpanding = false;

            this.ExplorerBar.ItemClicked += new ExplorerBarEventHandler(ExplorerBar_ItemClicked);
        }

        private void ExplorerBar_ItemClicked(object sender, ExplorerBarEventArgs e)
        {
            if (ItemClicked != null)
                this.ItemClicked(this, e);
        }

        private void InitializeCategories()
        {
            _groups.Clear();

            _catView = new ExplorerItemsGroup(
                SIAResources.GetShortcutIcon("pseudocolor"), "View");

            _catProcess = new ExplorerItemsGroup(
                SIAResources.GetShortcutIcon("calc"), "Process");

            _catFilter = new ExplorerItemsGroup(
                SIAResources.GetShortcutIcon("fltgaussian"), "Filters");

            _catAnalysis = new ExplorerItemsGroup(
                SIAResources.GetShortcutIcon("align_ABS"), "Analysis");

            _catTools = new ExplorerItemsGroup(
                SIAResources.GetShortcutIcon("edit_mark"), "Tools");

            _groups.Add(_catView);
            _groups.Add(_catProcess);
            _groups.Add(_catFilter);
            _groups.Add(_catAnalysis);
            _groups.Add(_catTools);

            this.ExplorerBar.AddGroup(_catView);
            this.ExplorerBar.AddGroup(_catProcess);
            this.ExplorerBar.AddGroup(_catFilter);
            this.ExplorerBar.AddGroup(_catAnalysis);
            this.ExplorerBar.AddGroup(_catTools);

            _catAnalysis.Status = eExplorerItemGroupStatus.Expanded;

            // initialize category lookup table
            _lookupTable = new Hashtable();
            _lookupTable.Add(_catView.Text, _catView);
            _lookupTable.Add(_catProcess.Text, _catProcess);
            _lookupTable.Add(_catFilter.Text, _catFilter);
            _lookupTable.Add(_catAnalysis.Text, _catAnalysis);
            _lookupTable.Add(_catTools.Text, _catTools);
        }
        #endregion Constructors

        #region Event Handlers
        private void SplitterMoved(object sender, System.Windows.Forms.SplitterEventArgs e)
        {
            if (_explorerBarContainer.Width < 120)
            {
                _explorerBarContainer.Width = 120;
                _splitter.ToggleSplitter();
            }
        }

        private void AutoHideButtonClicked(object sender, EventArgs e)
        {
            _splitter.ToggleSplitter();
        }

        private void appWorkspace_Load(object sender, EventArgs e)
        {
            this.RefreshItemStatus();
        }

        private void appWorkspace_PostDispatchCommand(object sender, EventArgs e)
        {
            this.RefreshItemStatus();
        }
        #endregion Event Handlers

        #region Methods
        private class ShortcutInfoComparer : IComparer
        {
            #region IComparer Members

            public int Compare(object x, object y)
            {
                IUICommandHandler h1 = x as IUICommandHandler;
                IUICommandHandler h2 = y as IUICommandHandler;
                return h1.ShortcutInfo.Index - h2.ShortcutInfo.Index;
            }

            #endregion
        }

        public void RebuildShortcuts()
        {
            if (this._appWorkspace == null)
                return;

            string[] categories = Categories.ToArray();
            ICommandHandler[] cmdHandlers = this._appWorkspace.Commands;
            foreach (string category in categories)
            {
                if (_lookupTable[category] == null)
                    continue;

                ArrayList handlers = new ArrayList();
                foreach (ICommandHandler cmdHandler in cmdHandlers)
                {
                    if (cmdHandler is IUICommandHandler == false)
                        continue;
                    IUICommandHandler uiHandler = cmdHandler as IUICommandHandler;
                    if (uiHandler.ShortcutInfo != null && 
                        uiHandler.ShortcutInfo.Category == category)
                        handlers.Add(cmdHandler);
                }

                if (handlers.Count == 0)
                    continue;

                // sort handler by index
                handlers.Sort(new ShortcutInfoComparer());

                // add menu item
                foreach (IUICommandHandler handler in handlers)
                    this.AddShortcut(handler);
            }

        }

        public void RefreshItemStatus()
        {
            foreach (ExplorerItemsGroup group in _groups)
            {
                foreach (ExplorerItem item in group.Items)
                {
                    if (item.Tag == null)
                        continue;

                    IUICommandHandler handler = item.Tag as IUICommandHandler;
                    if (handler == null)
                        continue;

                    UIElementStatus status = handler.QueryShortcutBarItemStatus();
                    this.UpdateItemStatus(item, status);
                }
            }

            _explorerBarContainer.ExplorerBar.Redraw();
        }
        #endregion Methods

        #region Plugin Events
        private void pluginMgr_PluginLoaded(object sender, EventArgs e)
        {
            PluginLoadedEventArgs args = e as PluginLoadedEventArgs;
            if (args == null || args.Plugin == null || args.Plugin.CommandHandlers == null)
                return;
            IPlugin plugin = args.Plugin;
            ICommandHandler[] cmdHandlers = plugin.CommandHandlers;
            foreach (IUICommandHandler uiHandler in cmdHandlers)
            {
                if (uiHandler == null)
                    continue;

                if (uiHandler.ShortcutInfo != null)
                    this.AddShortcut(uiHandler);
            }
        }

        private void pluginMgr_PluginUnloaded(object sender, EventArgs e)
        {
            PluginUnloadedEventArgs args = e as PluginUnloadedEventArgs;
            if (args == null || args.Plugin == null || args.Plugin.CommandHandlers == null)
                return;
            IPlugin plugin = args.Plugin;
            ICommandHandler[] cmdHandlers = plugin.CommandHandlers;
            foreach (IUICommandHandler uiHandler in cmdHandlers)
            {
                if (uiHandler == null)
                    continue;

                if (uiHandler.ShortcutInfo != null)
                    this.RemoveShortcut(uiHandler);
            }
        }
        #endregion Plugin Events

        #region Helpers
        public void AddShortcut(IUICommandHandler handler)
        {
            if (handler == null)
                throw new ArgumentNullException("handler");

            if (handler.ShortcutInfo != null)
            {
                string category = handler.ShortcutInfo.Category;
                ExplorerItemsGroup group = _lookupTable[category] as ExplorerItemsGroup;
                if (group == null)
                {
                    Trace.WriteLine(String.Format("Category {0} was not found.", category));
                }
                else
                {
                    System.Drawing.Image shortcutImage = handler.ShortcutInfo.Image;
                    if (shortcutImage != null)
                        shortcutImage = 
                            shortcutImage.Clone() as System.Drawing.Image;

                    ExplorerItem item = new ExplorerItem(
                        shortcutImage, handler.ShortcutInfo.Text);
                    item.Tag = handler;

                    if (item != null)
                        group.Add(item);
                    
                    // sort chid items of the band
                    //SortOutlookBarBand(band);
                }
            }
        }

        public void RemoveShortcut(IUICommandHandler handler)
        {
            if (handler == null)
                throw new ArgumentNullException("handler");

            if (handler.ShortcutInfo != null)
            {
                string category = handler.ShortcutInfo.Category;
                ExplorerItemsGroup group = _lookupTable[category] as ExplorerItemsGroup;
                if (group == null)
                {
                    Trace.WriteLine(String.Format("Category {0} was not found.", category));
                }
                else
                {
                    for (int i = group.Items.Count - 1; i >= 0; i--)
                    {
                        if (group.Items[i].Tag == handler)
                        {
                            group.Items.RemoveAt(i);
                            break;
                        }
                    }
                }                
            }
        }

        public void BeginUpdate()
        {
            //this._ignoreItemUpdate++;
        }

        public void EndUpdate()
        {
            //this._ignoreItemUpdate--;
        }

        private void UpdateItemStatus(ExplorerItem item, UIElementStatus status)
        {
            ExplorerItem itemEx = item as ExplorerItem;
            if (itemEx == null)
                return;

            switch (status)
            {
                case UIElementStatus.Visible:
                    itemEx.Enabled = true;
                    itemEx.Visible = true;
                    break;
                case UIElementStatus.Invisible: ;
                    itemEx.Enabled = true;
                    itemEx.Visible = false;
                    break;
                case UIElementStatus.Enable:
                    itemEx.Visible = true;
                    itemEx.Enabled = true;
                    break;
                case UIElementStatus.Disable:
                    itemEx.Visible = true;
                    itemEx.Enabled = false;
                    break;
                case UIElementStatus.Checked:
                    itemEx.Visible = true;
                    itemEx.Enabled = true;
                    break;
                case UIElementStatus.Unchecked:
                    itemEx.Visible = true;
                    itemEx.Enabled = true;
                    break;
            }
        }
        #endregion Helpers
    }
}
