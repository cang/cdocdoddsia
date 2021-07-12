namespace SIA.UI.CommandHandlers
{
	using System;
	using System.Windows.Forms;

	using SIA.Plugins;
	using SIA.Plugins.Common;

	using SIA.UI.Controls;
	using SIA.UI.Controls.Commands;

	internal class CmdEditCustomize : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdEditCustomize";
		
		private static MenuInfo menuInfo = null;

		static CmdEditCustomize()
		{
			menuInfo = new MenuInfo(
                "&Customize...", Categories.Edit, Shortcut.None, 240, null, SeparateStyle.Both);
		}

		public CmdEditCustomize(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, menuInfo, null, null)
		{
		}

		public override void DoCommand(params object[] args)
		{
			MainFrame appWorkspace = this.appWorkspace;
			MainMenu mainMenu = appWorkspace.MainMenu;
			using (DlgCustomize dlg = new DlgCustomize(mainMenu))
				dlg.ShowDialog(appWorkspace);
		}

		public override UIElementStatus QueryMenuItemStatus()
		{
			return UIElementStatus.Enable;
		}

	}
}
