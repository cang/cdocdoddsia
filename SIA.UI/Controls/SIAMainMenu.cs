using System;
using System.Diagnostics;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;

using SIA.Plugins;
using SIA.Plugins.Common;

using SIA.UI.Menus;
using SiGlaz.UI.CustomControls;

namespace SIA.UI.Controls
{
	/// <summary>
	/// Summary description for RDEMainMenu.
	/// </summary>
    internal class SIAMainMenu : MainMenu
	{
		private class MenuInfoComparer : IComparer
		{
			#region IComparer Members

			public int Compare(object x, object y)
			{
				IUICommandHandler h1 = x as IUICommandHandler;
				IUICommandHandler h2 = y as IUICommandHandler;
				return h1.MenuInfo.Index - h2.MenuInfo.Index;
			}

			#endregion
		}

		private class MenuItemExComparer : IComparer
		{
			#region IComparer Members

			public int Compare(object x, object y)
			{
				MenuItemEx h1 = null;
				MenuItemEx h2 = null;

				try
				{
					h1 = x as MenuItemEx;
					h2 = y as MenuItemEx;

					return (int)(h1.CommandHandler.MenuInfo.Index - h2.CommandHandler.MenuInfo.Index);
				}
				catch
				{
					if (h1 != null && h2 != null)
						Trace.WriteLine(String.Format("Failed to compare item {0} and item {1}", h1.Text, h2.Text));
					throw;
				}
			}

			#endregion
		}

		private MainFrame _appWorkspace = null;
		private Hashtable _lookupTable = new Hashtable();
		private int _ignoreItemUpdate = 0;

		public event EventHandler MenuItemClicked = null;

		public MainFrame appWorkspace
		{
			get {return _appWorkspace;}
		}

		public SIAMainMenu() : this(null)
		{
		}

        public SIAMainMenu(Form appWorkspace)
            : this(appWorkspace as MainFrame)
        {
        }

		public SIAMainMenu(MainFrame appWorkspace) 
            : base()
		{            
			if (appWorkspace != null)
			{
				this._appWorkspace = appWorkspace;

				// register for event handlers
				IPluginManager pluginMgr = this._appWorkspace.PluginManager;
				pluginMgr.PluginLoaded += new EventHandler(pluginMgr_PluginLoaded);
				pluginMgr.PluginUnloaded += new EventHandler(pluginMgr_PluginUnloaded);
			}

			// initialize internal controls
			this.InitializeComponents();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose (disposing);

			if (this._appWorkspace != null)
			{
				// register for event handlers
				IPluginManager pluginMgr = this._appWorkspace.PluginManager;
				pluginMgr.PluginLoaded -= new EventHandler(pluginMgr_PluginLoaded);
				pluginMgr.PluginUnloaded -= new EventHandler(pluginMgr_PluginUnloaded);
			}
		}


		private void InitializeComponents()
		{
			int index = 0;
			MenuItem mnuItem = new FileMenu();
			mnuItem.Popup += new EventHandler(MenuItem_Popup);
			this.MenuItems.Add(index++, mnuItem);
			_lookupTable.Add(Categories.File, mnuItem);

			mnuItem = new EditMenu();
			mnuItem.Popup += new EventHandler(MenuItem_Popup);
			this.MenuItems.Add(index++, mnuItem);
			_lookupTable.Add(Categories.Edit, mnuItem);

			mnuItem = new ViewMenu();
			mnuItem.Popup += new EventHandler(MenuItem_Popup);
			this.MenuItems.Add(index++, mnuItem);
			_lookupTable.Add(Categories.View, mnuItem);

			mnuItem = new ProcessMenu();
			mnuItem.Popup += new EventHandler(MenuItem_Popup);
			this.MenuItems.Add(index++, mnuItem);
			_lookupTable.Add(Categories.Process, mnuItem);

			mnuItem = new FilterMenu();
			mnuItem.Popup += new EventHandler(MenuItem_Popup);
			this.MenuItems.Add(index++, mnuItem);
			_lookupTable.Add(Categories.Filters, mnuItem);

			mnuItem = new AnalysisMenu();
			mnuItem.Popup += new EventHandler(MenuItem_Popup);
			this.MenuItems.Add(index++, mnuItem);
			_lookupTable.Add(Categories.Analysis, mnuItem);

			mnuItem = new ToolsMenu();
			mnuItem.Popup += new EventHandler(MenuItem_Popup);
			this.MenuItems.Add(index++, mnuItem);
			_lookupTable.Add(Categories.Tools, mnuItem);

			mnuItem = new HelpMenu();
			mnuItem.Popup += new EventHandler(MenuItem_Popup);
			this.MenuItems.Add(index++, mnuItem);
			_lookupTable.Add(Categories.Help, mnuItem);			
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
					if (uiHandler != null && uiHandler.MenuInfo != null)
						this.AddMenuItem(uiHandler);
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
					if (uiHandler != null && uiHandler.MenuInfo != null)
						this.RemoveMenuItem(uiHandler);
				}

				this.RemoveDuplicateSeparators();
			}
		}        
		
		public void RebuildMenuItems()
		{
			if (this._appWorkspace == null)
				return;

			try
			{
				this.BeginUpdate();
			
				string[] categories = Categories.ToArray();
				ICommandHandler[] cmdHandlers = this.appWorkspace.Commands;
				foreach (string category in categories)
				{
					ArrayList handlers = new ArrayList();
					foreach (ICommandHandler cmdHandler in cmdHandlers)
					{
						if (cmdHandler is IUICommandHandler == false)
							continue;
						IUICommandHandler uiHandler = cmdHandler as IUICommandHandler;
						if (uiHandler.MenuInfo != null && uiHandler.MenuInfo.Category == category)
							handlers.Add(cmdHandler);
					}

					// sort handler by index
					handlers.Sort(new MenuInfoComparer());

					// request category menu to clear internal items
					MenuItemEx mnuCategory = _lookupTable[category] as MenuItemEx;
					if (mnuCategory != null)
						mnuCategory.BeginUpdateMenuItems();

					// add menu item
					foreach (IUICommandHandler handler in handlers)
						this.AddMenuItem(handler);
				
					if (mnuCategory != null)
						mnuCategory.EndUpdateMenuItems();
				}

				// creates separators
				foreach (ICommandHandler cmdHandler in cmdHandlers)
				{
					if (cmdHandler is IUICommandHandler == false)
						continue;
					IUICommandHandler handler = cmdHandler as IUICommandHandler;
					if (handler.MenuInfo == null)
						continue;
					if (handler.MenuInfo.SeparateStyle != SeparateStyle.None)
						this.AddSeparators(handler);
				}

				// remove duplicate sepators
				this.RemoveDuplicateSeparators();
			}
			finally
			{
				this.EndUpdate();
			}
		}

		public void RefreshMenuItemStatus()
		{
			ICommandHandler[] handlers = this.appWorkspace.Commands;
			foreach (ICommandHandler handler in handlers)
			{
				if (handler is IUICommandHandler)
				{
					IUICommandHandler uiHandler = handler as IUICommandHandler;
					MenuInfo menuInfo = uiHandler.MenuInfo;
					if (menuInfo != null && menuInfo.MenuItem != null)
					{
						UIElementStatus status = uiHandler.QueryMenuItemStatus();
						this.UpdateMenuItemStatus(menuInfo.MenuItem as MenuItemEx, status);
					}
				}
			}
		}

		public void RefreshMenuItemStatus(MenuItemEx menuItem, bool recursive)
		{
			if (menuItem.CommandHandler != null)
			{
				IUICommandHandler uiHandler = menuItem.CommandHandler;
				UIElementStatus status = uiHandler.QueryMenuItemStatus();
				this.UpdateMenuItemStatus(menuItem, status);
			}

			if (recursive)
			{
				foreach (MenuItemEx childItem in menuItem.MenuItems)
					this.RefreshMenuItemStatus(childItem, recursive);
			}
		}

		
		private void UpdateMenuItemStatus(MenuItemEx menuItem, UIElementStatus status)
		{
            bool visible = ((int)status & (int)UIElementStatus.Visible) == (int)UIElementStatus.Visible;
            bool enabled = ((int)status & (int)UIElementStatus.Enable) == (int)UIElementStatus.Enable;
            bool isChecked = ((int)status & (int)UIElementStatus.Checked) == (int)UIElementStatus.Checked;

            menuItem.Visible = visible;
            menuItem.Enabled = enabled;

            if (menuItem.MenuItems.Count <= 0)
                menuItem.Checked = isChecked;
		}

		
		private void AddMenuItem(IUICommandHandler handler)
		{
			if (handler == null)
				throw new ArgumentNullException("handler");
			
			if (handler.MenuInfo != null)
			{
				string category = handler.MenuInfo.Category;
				MenuItemEx mnuCategory = _lookupTable[category] as MenuItemEx;
				if (mnuCategory == null)
				{
					Trace.WriteLine(String.Format("Category {0} was not found.", category));
				}
				else
				{
					MenuItemEx menuItem = this.CreateMenuItem(handler);
					if (menuItem != null)
						mnuCategory.MenuItems.Add(menuItem);

					// sort sub menu items by index
					this.SortMenuItemsByIndex(mnuCategory);
				}
			}
		}

		private void RemoveMenuItem(IUICommandHandler handler)
		{
			if (handler == null)
				throw new ArgumentNullException("handler");
			
			if (handler.MenuInfo != null)
			{
				string category = handler.MenuInfo.Category;
				MenuItem mnuCategory = _lookupTable[category] as MenuItem;
				if (mnuCategory == null)
					throw new Exception(String.Format("Category {0} was not found.", category));
				
				MenuItemEx menuItem = handler.MenuInfo.MenuItem as MenuItemEx;
				if (menuItem != null)
				{
					mnuCategory.MenuItems.Remove(menuItem);
					this.DestroyMenuItem(menuItem);
					handler.MenuInfo.MenuItem = null;

					// just update the separators
					this.RemoveDuplicateSeparators(mnuCategory);
				}			
			}
		}

		
		private void AddSeparators(IUICommandHandler handler)
		{
			if (handler == null)
				throw new ArgumentNullException("handler");
			
			if (handler.MenuInfo != null)
			{
				string category = handler.MenuInfo.Category;
				MenuItemEx mnuCategory = _lookupTable[category] as MenuItemEx;
				if (mnuCategory == null)
					throw new Exception(String.Format("Category {0} was not found.", category));

				int index = handler.MenuInfo.MenuItem.Index;
				if ((handler.MenuInfo.SeparateStyle & SeparateStyle.Above) == SeparateStyle.Above)
					mnuCategory.MenuItems.Add(index, new MenuItemEx("-"));

				index = handler.MenuInfo.MenuItem.Index;
				if ((handler.MenuInfo.SeparateStyle & SeparateStyle.Below) == SeparateStyle.Below)
				{
					if (index+1 >= 0 && index+1<mnuCategory.MenuItems.Count)
						mnuCategory.MenuItems.Add(index+1, new MenuItemEx("-"));
					else
						mnuCategory.MenuItems.Add(new MenuItemEx("-"));
				}
			}
		}

		public virtual void SortMenuItemsByIndex(MenuItem menuItem)
		{
			if (this._ignoreItemUpdate != 0)
				return;

			int count = menuItem.MenuItems.Count;
			if (count <= 0)
				return;

			ArrayList sortItems = new ArrayList();
			for (int i=0; i<count; i++)
			{
				MenuItemEx item = menuItem.MenuItems[i] as MenuItemEx;
				// skip separator
				if (item.Text == "-")
					continue;
				sortItems.Add(item);
			}

			// sort the menu items
			MenuItemEx[] items = (MenuItemEx[])sortItems.ToArray(typeof(MenuItemEx));
			Array.Sort(items, new MenuItemExComparer());

			// clear the sub menuitems
			menuItem.MenuItems.Clear();

			// reinsert the sub menuitems
			for (int i=0; i<items.Length; i++)
			{
				MenuItemEx item = items[i] as MenuItemEx;
				// insert into menu
				menuItem.MenuItems.Add(item);
				// create separators
				IUICommandHandler handler = item.CommandHandler as IUICommandHandler;
				if (handler.MenuInfo == null)
					continue;
				if (handler.MenuInfo.SeparateStyle != SeparateStyle.None)
					this.AddSeparators(handler);
			}

			// remove duplicated separators
			this.RemoveDuplicateSeparators(menuItem);
		}

		private void RemoveDuplicateSeparators()
		{			
			foreach (MenuItem item in this.MenuItems)
				this.RemoveDuplicateSeparators(item);
		}

		private void RemoveDuplicateSeparators(MenuItem menuItem)
		{	
			for (int i=0; i<menuItem.MenuItems.Count-1; i++)
			{
				if (menuItem.MenuItems[i].Text == "-" && menuItem.MenuItems[i].Text == menuItem.MenuItems[i+1].Text)
				{
					menuItem.MenuItems.RemoveAt(i+1);
					i--;
				}
			}

			if (menuItem.MenuItems.Count > 0)
			{
				while (menuItem.MenuItems[0].Text == "-")
					menuItem.MenuItems.RemoveAt(0);
			}

			if (menuItem.MenuItems.Count > 0)
			{
				while (menuItem.MenuItems[menuItem.MenuItems.Count-1].Text == "-")
					menuItem.MenuItems.RemoveAt(menuItem.MenuItems.Count-1);
			}
		}


		private MenuItemEx CreateMenuItem(IUICommandHandler handler)
		{
			MenuInfo menuInfo = handler.MenuInfo;
			string commandKey = handler.Command;

			MenuItemEx menuItem = new MenuItemEx();
			menuItem.Text = menuInfo.Text;
			if (menuInfo.Index >= 0)
				menuItem.Index = menuInfo.Index;
			menuItem.Image = menuInfo.Image;
			menuItem.CommandHandler = handler;
			menuItem.Command = commandKey;
			menuItem.Arguments = null;
			menuItem.Shortcut = menuInfo.Shortcuts;
			menuItem.ShowShortcut = true;
			menuItem.Click += new EventHandler(MenuItem_Click);			
			
			menuInfo.MenuItem = menuItem;

			return menuItem;
		}

		private void DestroyMenuItem(MenuItemEx menuItem)
		{
			menuItem.Click -= new EventHandler(MenuItem_Click);
			menuItem.Command = "";
			menuItem.Arguments = null;
		}

		
		private void MenuItem_Click(object sender, EventArgs e)
		{
			if (this.MenuItemClicked != null)
				this.MenuItemClicked(sender, e);
		}

		private void MenuItem_Popup(object sender, EventArgs e)
		{
			if (sender is MenuItemEx)
				this.RefreshMenuItemStatus((MenuItemEx)sender, true);
		}

		private void BeginUpdate()
		{
			this._ignoreItemUpdate++;
		}

		private void EndUpdate()
		{
			this._ignoreItemUpdate--;
		}
	}
}
