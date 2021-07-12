using System;
using System.Collections.Generic;
using System.Text;
using SIA.Plugins.Common;
using System.Windows.Forms;
using SIA.Plugins;
using SiGlaz.UI.CustomControls;
using System.Collections;
using System.Drawing;
using SIA.UI.CommandHandlers;

namespace SIA.UI.Controls
{
    internal class SIAToolBarManager
    {
        private class ToolBarInfoComparer : IComparer
        {
            #region IComparer Members

            public int Compare(object x, object y)
            {
                IUICommandHandler h1 = x as IUICommandHandler;
                IUICommandHandler h2 = y as IUICommandHandler;
                return h1.ToolBarInfo.Index - h2.ToolBarInfo.Index;
            }

            #endregion
        }        

        #region Member fields
        private MainFrame _appWorkspace = null;
        private ToolBarEx _mainToolbar = null;
        #endregion Member fields

        #region Properties

        public MainFrame appWorkspace
        {
            get { return _appWorkspace; }
        }

        #endregion Properties

        #region Constructor and destructor
		public SIAToolBarManager(MainFrame appWorkspace, ToolBarEx mainToolbar)
		{
            _mainToolbar = mainToolbar;
            Color[] normals = 
                GradientColor.GradientColorCollection[GradientColor.MainMenuBandIdx];
            Color[] highlights =
                GradientColor.GradientColorCollection[GradientColor.HighlightBandIdx];
            Color[] pushs =
                GradientColor.GradientColorCollection[GradientColor.PushedBandIdx];

            this._mainToolbar.BkColorEnd = normals[1];
            this._mainToolbar.BkColorHoverItemEnd = highlights[1];
            this._mainToolbar.BkColorHoverItemStart = highlights[0];
            this._mainToolbar.BkColorPushedItemEnd = pushs[1];
            this._mainToolbar.BkColorPushedItemStart = pushs[0];
            this._mainToolbar.BkColorStart = normals[0];
            this._mainToolbar.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this._mainToolbar.CornerRadius = 3.5F;
            this._mainToolbar.HasBottomBorder = true;
            this._mainToolbar.HasLeftBorder = true;
            this._mainToolbar.HasLeftBottomCorner = true;
            this._mainToolbar.HasLeftTopCorner = true;
            this._mainToolbar.HasRightBorder = true;
            this._mainToolbar.HasRightBottomCorner = true;
            this._mainToolbar.HasRightTopCorner = true;
            this._mainToolbar.HasTopBorder = true;
            this._mainToolbar.ItemBorderColor = System.Drawing.Color.Blue;

            this._mainToolbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(174)))), ((int)(((byte)(228)))));

			if (appWorkspace != null)
			{
				this._appWorkspace = appWorkspace;
				
				this._appWorkspace.Load += new EventHandler(appWorkspace_Load);
				this._appWorkspace.PostDispatchCommand += new EventHandler(appWorkspace_PostDispatchCommand);

				// register for event handlers
				IPluginManager pluginMgr = this._appWorkspace.PluginManager;
				pluginMgr.PluginLoaded += new EventHandler(pluginMgr_PluginLoaded);
				pluginMgr.PluginUnloaded += new EventHandler(pluginMgr_PluginUnloaded);
			}

            //this.ButtonSize = new Size(16, 16);
            //this.Divider = false;
		}

        ~SIAToolBarManager()
        {
            this.Dispose(true);
        }

		protected void Dispose(bool disposing)
		{
			if (this._appWorkspace != null)
            {
                this._appWorkspace.Load -= new EventHandler(appWorkspace_Load);
                this._appWorkspace.PostDispatchCommand -= new EventHandler(appWorkspace_PostDispatchCommand);

                // register for event handlers
                IPluginManager pluginMgr = this._appWorkspace.PluginManager;
                pluginMgr.PluginLoaded -= new EventHandler(pluginMgr_PluginLoaded);
                pluginMgr.PluginUnloaded -= new EventHandler(pluginMgr_PluginUnloaded);
            }
		}


		#endregion Constructor and destructor

        #region Events
        private void appWorkspace_PostDispatchCommand(object sender, EventArgs e)
        {
            this.RefreshButtonStatus();
        }

        private void appWorkspace_Load(object sender, EventArgs e)
        {
            this.RefreshButtonStatus();
        }

        private void pluginMgr_PluginLoaded(object sender, EventArgs e)
        {
            PluginLoadedEventArgs args = e as PluginLoadedEventArgs;
            if (args == null || args.Plugin == null || args.Plugin.CommandHandlers == null)
                return;
            ICommandHandler[] handlers = args.Plugin.CommandHandlers;
            if (handlers.Length > 0)
            {
                foreach (ICommandHandler handler in handlers)
                {
                    IUICommandHandler uiHandler = handler as IUICommandHandler;
                    if (uiHandler != null && uiHandler.ToolBarInfo != null)
                        this.AddToolBarButton(uiHandler);
                }

                this.RemoveDuplicateSeparators();
            }
        }

        private void pluginMgr_PluginUnloaded(object sender, EventArgs e)
        {
            PluginUnloadedEventArgs args = e as PluginUnloadedEventArgs;
            if (args == null || args.Plugin == null || args.Plugin.CommandHandlers == null)
                return;
            ICommandHandler[] handlers = args.Plugin.CommandHandlers;
            if (handlers.Length > 0)
            {
                foreach (ICommandHandler handler in handlers)
                {
                    IUICommandHandler uiHandler = handler as IUICommandHandler;
                    if (uiHandler != null && uiHandler.ToolBarInfo != null)
                        if (uiHandler != null && uiHandler.ToolBarInfo != null)
                            this.RemoveToolBarButton(uiHandler);
                }

                this.RemoveDuplicateSeparators();
            }
        }
        #endregion Events

        #region Methods
        public void RebuildItems()
        {
            if (this._appWorkspace == null)
                return;

            string[] categories = Categories.ToArray();
            ICommandHandler[] cmdHandlers = this.appWorkspace.Commands;
            ArrayList handlers = new ArrayList();
            foreach (ICommandHandler cmdHandler in cmdHandlers)
            {
                if (cmdHandler is IUICommandHandler == false)
                    continue;
                IUICommandHandler uiHandler = cmdHandler as IUICommandHandler;
                if (uiHandler.ToolBarInfo != null)
                    handlers.Add(cmdHandler);
            }

            // sort handler by index
            handlers.Sort(new ToolBarInfoComparer());
           
            // add toolbar button
            foreach (IUICommandHandler handler in handlers)
            {
                ToolBarItemEx tbn = this.CreateToolBarButton(handler);

                // append created button
                _mainToolbar.Add(tbn);

                // creates separators
                if (handler.ToolBarInfo.SeparateStyle != SeparateStyle.None)
                    this.AddSeparators(handler);
            }


            this.SortToolBarButtons();
            // remove duplicate sepators
            //this.RemoveDuplicateSeparators();
        }

        public void RefreshButtonStatus()
        {
            foreach (ToolBarItemEx item in _mainToolbar.Items)
            {
                if (!item.Visible || item.ButtonType == eToolBarItemExType.Separator)
                    continue;

                if (item.Tag == null || !(item.Tag is IUICommandHandler))
                    continue;

                UIElementStatus status = 
                    (item.Tag as IUICommandHandler).QueryToolBarItemStatus();

                UpdateToolBarButtonStatus(item, status);
            }

            int right = _appWorkspace.Width;;
            for (int i = _mainToolbar.Items.Count - 1; i >= 0; i--)
            {
                if (_mainToolbar.Items[i].Visible)
                {
                    right = _mainToolbar.Items[i].Bounds.Right;
                    break;
                }
            }


            _mainToolbar.Invalidate(true);
        }
        #endregion Methods

        #region Add/remove items
        public void AddToolBarButton(IUICommandHandler handler)
        {
            if (handler == null || handler.ToolBarInfo == null)
                return;

            ToolBarInfo tbInfo = handler.ToolBarInfo;
            ToolBarItemEx btn = this.CreateToolBarButton(handler);

            _mainToolbar.Add(btn);

            // sort the buttons and update the separators
            this.SortToolBarButtons();
        }

        public void RemoveToolBarButton(IUICommandHandler handler)
        {
            if (handler == null || handler.ToolBarInfo == null)
                return;

            ToolBarInfo tbInfo = handler.ToolBarInfo;
            for (int i = _mainToolbar.Items.Count - 1; i >= 0; i--)
            {
                ToolBarItemEx item = _mainToolbar.Items[i];
                if (item.Tag == null || !(item.Tag is IUICommandHandler))
                    continue;

                if (handler == item.Tag)
                {
                    _mainToolbar.Items.RemoveAt(i);
                    break;
                }
            }
            
            // sort the buttons and update the separators
            this.SortToolBarButtons();
        }

        private ToolBarItemEx CreateToolBarButton(IUICommandHandler handler)
        {
            ToolBarInfo tbInfo = handler.ToolBarInfo;
            string cmdKey = handler.Command;

            eToolBarItemExType style = ToolBarExHelper.Match(tbInfo.Style);
            ToolBarItemEx item = new ToolBarItemEx(tbInfo.Text, tbInfo.Image, style);
            item.ToolTip = tbInfo.Tooltip;
            item.Tag = handler;

            return item;
        }

        private void AddSeparators(IUICommandHandler handler)
        {
            ToolBarInfo tbInfo = handler.ToolBarInfo;
            int index = IndexOf(_mainToolbar.Items, handler);

            ToolBarItemEx separator = null;
            if ((tbInfo.SeparateStyle & SeparateStyle.Before) == SeparateStyle.Before)
            {
                separator =
                    new ToolBarItemEx(
                        string.Format("Separator_Before_{0}", tbInfo.Text), 
                        null, eToolBarItemExType.Separator);

                if (index >= this._mainToolbar.Items.Count)
                {
                    if (_mainToolbar.Items.Count > 0 &&
                        _mainToolbar.Items[_mainToolbar.Items.Count-1].ButtonType != eToolBarItemExType.Separator)
                        this._mainToolbar.Add(separator);
                }
                else
                {
                    if (!(_mainToolbar.Items[index].ButtonType == eToolBarItemExType.Separator ||
                        (index > 0 && _mainToolbar.Items[index-1].ButtonType == eToolBarItemExType.Separator)))
                        this._mainToolbar.Insert(index, separator);
                }
            }

            index = IndexOf(_mainToolbar.Items, handler);

            if ((tbInfo.SeparateStyle & SeparateStyle.After) == SeparateStyle.After)
            {
                separator =
                    new ToolBarItemEx(
                        string.Format("Separator_After_{0}", tbInfo.Text),
                        null, eToolBarItemExType.Separator);
                index = index+1;
                if (index >= this._mainToolbar.Items.Count)
                {
                    if (_mainToolbar.Items.Count > 0 &&
                        _mainToolbar.Items[_mainToolbar.Items.Count-1].ButtonType != eToolBarItemExType.Separator)
                        this._mainToolbar.Add(separator);
                }
                else
                {
                    if (!(_mainToolbar.Items[index].ButtonType == eToolBarItemExType.Separator ||
                        (index > 0 && _mainToolbar.Items[index-1].ButtonType == eToolBarItemExType.Separator)))
                        this._mainToolbar.Insert(index, separator);
                }
            }
        }
        #endregion Add/remove items

        #region Helpers
        private int IndexOf(
            ToolBarItemExCollection items, IUICommandHandler handler)
        {
            for (int i = items.Count - 1; i >= 0; i--)
            {
                ToolBarItemEx item = items[i];
                if (item.Tag == null || !(item.Tag is IUICommandHandler))
                    continue;

                if (handler == item.Tag)
                {
                    return i;
                }
            }

            return -1;
        }
        
        private void RemoveDuplicateSeparators()
        {
            //for (int i = 0; i < this.Buttons.Count - 1; i++)
            //{
            //    if (this.Buttons[i].Style == ToolBarButtonStyle.Separator &&
            //        this.Buttons[i + 1].Style == ToolBarButtonStyle.Separator)
            //    {
            //        this.Buttons.RemoveAt(i--);
            //    }
            //}

            //while (this.Buttons[0].Style == ToolBarButtonStyle.Separator)
            //    this.Buttons.RemoveAt(0);
            //while (this.Buttons[this.Buttons.Count - 1].Style == ToolBarButtonStyle.Separator)
            //    this.Buttons.RemoveAt(this.Buttons.Count - 1);
        }

        private void UpdateToolBarButtonStatus(
            ToolBarItemEx button, UIElementStatus status)
        {
            if (button == null)
                return;

            switch (status)
            {
                case UIElementStatus.Visible:
                    button.Enable = true;
                    button.Visible = true;
                    break;
                case UIElementStatus.Invisible: ;
                    button.Enable = true;
                    button.Visible = false;
                    break;
                case UIElementStatus.Enable:
                    button.Visible = true;
                    button.Enable = true;
                    break;
                case UIElementStatus.Disable:
                    button.Visible = true;
                    button.Enable = false;
                    break;
                case UIElementStatus.Checked:
                    button.Visible = true;
                    button.Enable = true;
                    button.Pushed = true;
                    break;
                case UIElementStatus.Unchecked:
                    button.Visible = true;
                    button.Enable = true;
                    button.Pushed = false;                    
                    break;
            }
        }

        private class ToolBarItemExComparer : IComparer
        {
            #region IComparer Members

            public int Compare(object x, object y)
            {
                IUICommandHandler t1 = (x as ToolBarItemEx).Tag as IUICommandHandler;
                IUICommandHandler t2 = (y as ToolBarItemEx).Tag as IUICommandHandler;
                return t1.ToolBarInfo.Index - t2.ToolBarInfo.Index;
            }

            #endregion
        }

        private void SortToolBarButtons()
        {
            //if (this._ignoreItemUpdate != 0)
            //    return;

            ArrayList sortItems = new ArrayList();
            foreach (ToolBarItemEx btn in _mainToolbar.Items)
            {
                if (btn.ButtonType == eToolBarItemExType.Separator)
                    continue;

                sortItems.Add(btn);
            }

            sortItems.Sort(new ToolBarItemExComparer());

            // clear old buttons
            _mainToolbar.Items.Clear();

            // rearrange buttons
            foreach (ToolBarItemEx btn in sortItems)
            {
                ToolBarInfo tbInfo = (btn.Tag as IUICommandHandler).ToolBarInfo;

                // insert the button
                _mainToolbar.Add(btn);

                // create separator
                if (tbInfo.SeparateStyle != SeparateStyle.None)
                    this.AddSeparators(btn.Tag as IUICommandHandler);
            }

            // remove duplicated sepators
            this.RemoveDuplicateSeparators();
        }
        #endregion Helpers
    }
}
