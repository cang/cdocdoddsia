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

	internal class CmdViewToolBar : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdToggleToolBar";
		
		private static MenuInfo menuInfo = null;

		static CmdViewToolBar()
		{
			menuInfo = new MenuInfo("ToolBar", Categories.View, Shortcut.None, 394, null, SeparateStyle.Above);
		}

		public CmdViewToolBar(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, menuInfo, null, null)
		{
		}

		public override void DoCommand(params object[] args)
		{
			MainFrame appWorkspace = this.appWorkspace;
			appWorkspace.MainToolBar.Visible = !appWorkspace.MainToolBar.Visible;
		}

		public override UIElementStatus QueryMenuItemStatus()
		{
			MainFrame appWorkspace = this.appWorkspace;
			return appWorkspace.MainToolBar.Visible ? UIElementStatus.Checked : UIElementStatus.Unchecked;
		}
	}
}
