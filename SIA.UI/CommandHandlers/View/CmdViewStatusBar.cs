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

	internal class CmdViewStatusBar : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdToggleStatusBar";
		
		private static MenuInfo menuInfo = null;

		static CmdViewStatusBar()
		{
			menuInfo = new MenuInfo("Status Bar", Categories.View, Shortcut.None, 396, null, SeparateStyle.None);
		}

		public CmdViewStatusBar(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, menuInfo, null, null)
		{
		}

		public override void DoCommand(params object[] args)
		{
			MainFrame appWorkspace = this.appWorkspace;
			appWorkspace.StatusBar.Visible = !appWorkspace.StatusBar.Visible;
		}

		public override UIElementStatus QueryMenuItemStatus()
		{
			MainFrame appWorkspace = this.appWorkspace;
			return appWorkspace.StatusBar.Visible ? UIElementStatus.Checked : UIElementStatus.Unchecked;
		}
	}
}
