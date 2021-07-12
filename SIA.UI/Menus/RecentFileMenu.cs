using System;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;

using SIA.Plugins;
using SIA.Plugins.Common;

//using UITools;
using SIA.UI;

namespace SIA.UI.Menus
{
	/// <summary>
	/// Summary description for RecentFileMenu.
	/// </summary>
	public class RecentFileMenu : IDisposable
	{		
		private MenuItemEx _mnuItem = null;

		public RecentFileMenu(MenuItemEx mnuItem)
		{			
			if (mnuItem == null)
				throw new ArgumentNullException("mnuItem");
			_mnuItem = mnuItem;

			// register for popup event
			_mnuItem.Popup += new EventHandler(MenuItem_Popup);

			// initialize recent files menu items
			this.RefreshRecentFiles();
		}

		#region IDisposable Members

		public void Dispose()
		{
			_mnuItem = null;
		}

		#endregion

		public void RefreshRecentFiles()
		{
			if (this._mnuItem == null)
				throw new ArgumentNullException("_mnuItem");
			
			MenuItemEx mnuItem = this._mnuItem;
			mnuItem.MenuItems.Clear();
			//RecentFileManager recentFiles = new RecentFileManager(UITools.UI_TYPE.FPP);
			//recentFiles.Derialize();
			//string[] files = (string[])items.ToArray(typeof(string));
			string[] files = SIARecentFiles.GetRecentFiles();
			for (int i=0; i<files.Length; i++)
			{
				string file = files[i];
				string title = string.Format("{0}.{1}", i+1, file);
				MenuItemEx child = new MenuItemEx(title);
				if (child != null)
				{
					child.Command = "CmdOpenRecentFiles";
					child.Arguments = new object[] {file};
					child.Click += new EventHandler(RecentMenuItem_Click);
					mnuItem.MenuItems.Add(child);
				}
			}
		}

		private void RecentMenuItem_Click(object sender, EventArgs e)
		{
			object[] oldArguments = null;
			MenuItemEx mnuItem = sender as MenuItemEx;

			if (mnuItem != null)
			{
				try
				{
					oldArguments = this._mnuItem.Arguments;
					this._mnuItem.Arguments = mnuItem.Arguments;
					this._mnuItem.RaiseClickEvent();
				}
				finally
				{
					this._mnuItem.Arguments = oldArguments;
				}
			}
		}

		private void MenuItem_Popup(object sender, EventArgs e)
		{
			this.RefreshRecentFiles();
		}
	}
}
