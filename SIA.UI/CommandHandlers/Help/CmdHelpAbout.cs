using System;
using System.Windows.Forms;

using SIA.Plugins;
using SIA.Plugins.Common;

using SIA.UI.Controls;
using SIA.UI.Controls.Commands;

namespace SIA.UI.CommandHandlers
{
	internal class CmdHelpAbout : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdHelpAbout";
		
		private static MenuInfo menuInfo = null;

		static CmdHelpAbout()
		{
			menuInfo = new MenuInfo("&About", Categories.Help, Shortcut.None, 810, 
				SIAResources.GetMenuIcon("AboutIcon"), SeparateStyle.Both);
		}

		public CmdHelpAbout(IAppWorkspace appWorkspace)
            : base(appWorkspace, cmdCommandKey, menuInfo, null, null)
		{
		}

		public override void DoCommand(params object[] args)
		{
			using (DlgAbout dlg = new DlgAbout())
				dlg.ShowDialog(this.appWorkspace);
		}

		public override UIElementStatus QueryMenuItemStatus()
		{
			return UIElementStatus.Enable;
		}

		public override UIElementStatus QueryShortcutBarItemStatus()
		{
			return UIElementStatus.Invisible;
		}

	}
}
