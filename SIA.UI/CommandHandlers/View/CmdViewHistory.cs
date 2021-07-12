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

	internal class CmdViewHistory : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdToggleHistory";
		
		private static MenuInfo menuInfo = null;

		static CmdViewHistory()
		{
			menuInfo = new MenuInfo("Histories", Categories.View, Shortcut.None, 397, null, SeparateStyle.None);
		}

		public CmdViewHistory(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, menuInfo, null, null)
		{
		}

		public override void DoCommand(params object[] args)
		{
			MainFrame appWorkspace = this.appWorkspace;
			appWorkspace.HistoryWindow.Visible = !appWorkspace.HistoryWindow.Visible;
		}

		public override UIElementStatus QueryMenuItemStatus()
		{
			MainFrame appWorkspace = this.appWorkspace;
			return appWorkspace.HistoryWindow.Visible ? UIElementStatus.Checked : UIElementStatus.Unchecked;
		}
	}
}
