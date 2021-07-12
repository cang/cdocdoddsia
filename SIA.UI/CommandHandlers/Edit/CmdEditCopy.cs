using System;
using System.Windows.Forms;

using SIA.Plugins;
using SIA.Plugins.Common;

using SIA.UI.Controls;
using SIA.UI.Controls.Commands;
using SIA.UI.Controls.Helpers;

namespace SIA.UI.CommandHandlers
{
    internal class CmdEditCopy : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdCopyImageToClipboard";
		
		private static MenuInfo menuInfo = null;

		static CmdEditCopy()
		{
			menuInfo = 
                new MenuInfo("&Copy", Categories.Edit, Shortcut.CtrlC, 220, 
				SIAResources.GetMenuIcon("CopyIcon"), SeparateStyle.Above);
		}

		public CmdEditCopy(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, menuInfo, null, null)
		{
		}

		public override void DoCommand(params object[] args)
		{
            ImageWorkspace workspace = this.Workspace;
            SIA.UI.Controls.Helpers.ClipboardHelper.CopyToClipboard(workspace);
		}

		public override UIElementStatus QueryMenuItemStatus()
		{
			ImageWorkspace workspace = this.Workspace;
			if (workspace.Image == null)
				return UIElementStatus.Disable;

            bool canCopy = ClipboardHelper.CanCopyToClipboard(workspace);

			return canCopy ? UIElementStatus.Enable : UIElementStatus.Disable;
		}

		public override UIElementStatus QueryToolBarItemStatus()
		{
			ImageWorkspace workspace = this.Workspace;
			if (workspace.Image == null)
				return UIElementStatus.Disable;

			 bool canCopy = ClipboardHelper.CanCopyToClipboard(workspace);

			return canCopy ? UIElementStatus.Enable : UIElementStatus.Disable;
		}
	}
}
