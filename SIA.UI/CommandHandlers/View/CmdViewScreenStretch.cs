using System;
using System.Windows.Forms;

using SIA.Plugins;
using SIA.Plugins.Common;

using SIA.UI.Controls;
using SIA.UI.Controls.Commands;
using SIA.UI.Controls.Dialogs;

namespace SIA.UI.CommandHandlers
{
	internal class CmdViewScreenStretch : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdViewScreenStretch";
		
		private static MenuInfo menuInfo = null;
		private static ShortcutInfo shortcutInfo = null;

		static CmdViewScreenStretch()
		{
            System.Drawing.Image shortcutImage =
                SIAResources.GetShortcutIcon("screenstretch");
            System.Drawing.Image menuImage =
                SIAResources.CreateMenuIconFromShortcutIcon(shortcutImage);

			menuInfo = 
                new MenuInfo(
                    "&Screen Stretch", Categories.View, Shortcut.None, 330, menuImage, SeparateStyle.Above);
			shortcutInfo = 
                new ShortcutInfo(
                    "Screen Stretch", Categories.View, 330, shortcutImage);			
		}

		public CmdViewScreenStretch(IAppWorkspace appWorkspace) 
            : base(appWorkspace, cmdCommandKey, menuInfo, null, shortcutInfo)
		{
		}

		public override void DoCommand(params object[] args)
		{
			ImageWorkspace workspace = this.Workspace;
            workspace.ScreenStretch = !workspace.ScreenStretch;
		}

		public override UIElementStatus QueryMenuItemStatus()
		{
			return base.QueryMenuItemStatus();
		}

		public override UIElementStatus QueryShortcutBarItemStatus()
		{
			return base.QueryShortcutBarItemStatus();
		}

	}
}

