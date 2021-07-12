using System;
using System.Windows.Forms;

using SIA.Plugins;
using SIA.Plugins.Common;

using SIA.UI.Controls;
using SIA.UI.Controls.Commands;
using SIA.UI.Controls.Dialogs;

namespace SIA.UI.CommandHandlers
{
	internal class CmdViewCustomZoom : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdViewCustomZoom";
		
		private static MenuInfo menuInfo = null;

		static CmdViewCustomZoom()
		{
			menuInfo = 
                new MenuInfo(
                    "&Custom Zoom...", Categories.View, Shortcut.None, 393, null, SeparateStyle.Below);
		}

		public CmdViewCustomZoom(IAppWorkspace appWorkspace)
            : base(appWorkspace, cmdCommandKey, menuInfo, null, null)
		{
		}

		public override void DoCommand(params object[] args)
		{
            ImageWorkspace workspace = this.Workspace;

            using (DlgManualZoom dlg = new DlgManualZoom())
            {
                float scaleFactor = workspace.ImageViewer.ScaleFactor * 100.0F;
                dlg.Zoom = (int)scaleFactor;
                if (dlg.ShowDialog() == DialogResult.OK)
                    workspace.ImageViewer.ScaleFactor = dlg.Zoom / 100.0F;
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

