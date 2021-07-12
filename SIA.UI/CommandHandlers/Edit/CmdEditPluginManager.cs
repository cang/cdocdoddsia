namespace SIA.UI.CommandHandlers
{
	using System;
	using System.Windows.Forms;

	using SIA.Plugins;
	using SIA.Plugins.Common;

	using SIA.UI.Controls;
	using SIA.UI.Controls.Commands;

	using SIA.UI.Dialogs;

    internal class CmdEditPluginManager : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdEditPluginManager";
		
		private static MenuInfo menuInfo = null;

		static CmdEditPluginManager()
		{
			menuInfo = new MenuInfo(
                "&Plugin Manager...", Categories.Edit, Shortcut.None, 250, null, SeparateStyle.Both);
		}

		public CmdEditPluginManager(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, menuInfo, null, null)
		{
		}

		public override void DoCommand(params object[] args)
		{
			MainFrame appWorkspace = this.appWorkspace;
			MainMenu mainMenu = appWorkspace.MainMenu;
			// show plugin manager dialog
			using (DlgPluginManager dlg = new DlgPluginManager(appWorkspace.PluginManager))
			{
				dlg.ShowDialog(appWorkspace);
			}
		}

		public override UIElementStatus QueryMenuItemStatus()
		{
			return UIElementStatus.Enable;
		}

	}
}
