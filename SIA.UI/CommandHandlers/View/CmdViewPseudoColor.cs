using System;
using System.Windows.Forms;

using SIA.Plugins;
using SIA.Plugins.Common;

using SIA.UI.Controls;
using SIA.UI.Controls.Commands;
using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.UserControls;

namespace SIA.UI.CommandHandlers
{
	internal class CmdViewPseudoColor : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdViewPseudoColor";
		
		private static MenuInfo menuInfo = null;
		private static ShortcutInfo shortcutInfo = null;

		static CmdViewPseudoColor()
		{
            System.Drawing.Image shortcutImage =
                SIAResources.GetShortcutIcon("pseudocolor");
            System.Drawing.Image menuImage =
                SIAResources.CreateMenuIconFromShortcutIcon(shortcutImage);

			menuInfo = 
                new MenuInfo(
                "&Pseudo Color", Categories.View, Shortcut.None, 350, menuImage, SeparateStyle.Below);
			shortcutInfo = 
                new ShortcutInfo(
                    "Pseudo Color", Categories.View, 350, shortcutImage);			
		}

		public CmdViewPseudoColor(IAppWorkspace appWorkspace)
            : base(appWorkspace, cmdCommandKey, menuInfo, null, shortcutInfo)
		{
		}

		public override void DoCommand(params object[] args)
		{
            ImageWorkspace workspace = this.Workspace;
            ImageViewer imageViewer = workspace.ImageViewer;

            using (DlgPseudoColor dlg = new DlgPseudoColor(imageViewer.PseudoColor))
            {
                if (dlg.ShowDialog(workspace) == DialogResult.OK)
                {
                    imageViewer.PseudoColor = dlg.PseudoColor;
                    imageViewer.Invalidate(true);
                }
            }
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

