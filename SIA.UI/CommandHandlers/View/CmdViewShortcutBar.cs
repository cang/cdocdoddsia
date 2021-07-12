namespace SIA.UI.CommandHandlers
{
	using System;
	using System.Windows.Forms;
	using System.Drawing;

	using SIA.Plugins;
	using SIA.Plugins.Common;

	using SIA.UI.Controls;
	using SIA.UI.Controls.Common;
	using SIA.UI.Controls.Commands;

	internal class CmdViewShortcutBar : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdToggleShortcutBar";
		
		private static MenuInfo menuInfo = null;

		static CmdViewShortcutBar()
		{
			menuInfo = new MenuInfo("Explorer Bar", Categories.View, Shortcut.None, 395, null, SeparateStyle.None);
		}

		public CmdViewShortcutBar(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, menuInfo, null, null)
		{
		}

		public override void DoCommand(params object[] args)
		{
			MainFrame appWorkspace = this.appWorkspace;
			//appWorkspace.ShortcutBar.Visible = !appWorkspace.ShortcutBar.Visible;
		}

		public override UIElementStatus QueryMenuItemStatus()
		{
            return UIElementStatus.Visible;
			MainFrame appWorkspace = this.appWorkspace;
			//return appWorkspace.ShortcutBar.Visible ? UIElementStatus.Checked : UIElementStatus.Unchecked;
		}
	}
}
