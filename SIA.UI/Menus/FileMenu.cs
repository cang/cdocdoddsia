using System;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;

using SIA.Plugins;
using SIA.Plugins.Common;

namespace SIA.UI.Menus
{
	/// <summary>
	/// Summary description for FileMenu.
	/// </summary>
	public class FileMenu : MenuItemEx
	{
		private RecentFileMenu mnuRecentFile = null;

		public FileMenu() : base("&File", Categories.File)
		{			
		}

		public override void BeginUpdateMenuItems()
		{
			if (mnuRecentFile != null)
				mnuRecentFile.Dispose();
			mnuRecentFile = null;
		}

		public override void EndUpdateMenuItems()
		{			
			foreach (MenuItem mnuItem in this.MenuItems)
			{
				if (mnuItem is MenuItemEx)
				{
					MenuItemEx mnuItemEx = mnuItem as MenuItemEx;
					if (mnuItemEx.Command == "CmdFileOpenRecentFiles")
					{
						mnuRecentFile = new RecentFileMenu(mnuItemEx);
						break;
					}
				}
			}
		}
	}
}
