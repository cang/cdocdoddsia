using System;
using System.Windows.Forms;
using System.Drawing;

using SIA.Plugins;
using SIA.Plugins.Common;

using SIA.UI.Controls;
using SIA.UI.Controls.Commands;
using SIA.UI.Help;

namespace SIA.UI.CommandHandlers
{
	internal class CmdHelpIndex : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdHelpIndex";
		
		private static MenuInfo menuInfo = null;
		private static ToolBarInfo toolbarInfo = null;

		static CmdHelpIndex()
		{
			Image image = SIAResources.GetMenuIcon("IndexIcon");
			menuInfo = new MenuInfo("&Index", Categories.Help, Shortcut.None, 800, image, SeparateStyle.Both);
			toolbarInfo = new ToolBarInfo("Help Content", "Help Content", 800, image, SeparateStyle.Both);
		}

		public CmdHelpIndex(IAppWorkspace appWorkspace)
            : base(appWorkspace, cmdCommandKey, menuInfo, toolbarInfo, null)
		{
		}

		public override void DoCommand(params object[] args)
		{
			System.Windows.Forms.Help.ShowHelp(this.appWorkspace, ContextSensitiveHelper.HelpPath, HelpNavigator.Index,"");
		}

		public override UIElementStatus QueryMenuItemStatus()
		{
			return UIElementStatus.Enable;
		}
		
		public override UIElementStatus QueryToolBarItemStatus()
		{
			return UIElementStatus.Enable;
		}

	}
}
