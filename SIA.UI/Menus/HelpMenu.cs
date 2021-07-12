using System;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;

using SIA.Plugins;
using SIA.Plugins.Common;

namespace SIA.UI.Menus
{
	/// <summary>
	/// Summary description for HelpMenu.
	/// </summary>
	public class HelpMenu : MenuItemEx
	{
		public HelpMenu() : base("&Help", Categories.Help)
		{
		}
	}
}
