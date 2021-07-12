using System;
using System.Windows.Forms;

using SIA.Plugins;
using SIA.Plugins.Common;

using SIA.UI.Controls;
using SIA.UI.Controls.Commands;
using SIA.UI.Controls.Helpers;

namespace SIA.UI.CommandHandlers
{
	internal class CmdEditPaste : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdPasteImageFromClipboard";
		
		private static MenuInfo menuInfo = null;

		static CmdEditPaste()
		{
			menuInfo = new MenuInfo(
                "&Paste", Categories.Edit, Shortcut.CtrlV, 230, 
				SIAResources.GetMenuIcon("PasteIcon"), SeparateStyle.Below);
		}

		public CmdEditPaste(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, menuInfo, null, null)
		{
		}

		public override void DoCommand(params object[] args)
		{
            ImageWorkspace workspace = this.Workspace;
            SIA.UI.Controls.Helpers.ClipboardHelper.PasteFromClipboard(workspace);
		}

		public override UIElementStatus QueryMenuItemStatus()
		{
			ImageWorkspace workspace = this.Workspace;
			if (workspace.Image == null)
				return UIElementStatus.Disable;

            bool canPaste = ClipboardHelper.CanPasteFromClipboard(workspace);

			return canPaste ? UIElementStatus.Enable : UIElementStatus.Disable;
		}

		public override UIElementStatus QueryToolBarItemStatus()
		{
			ImageWorkspace workspace = this.Workspace;
			if (workspace.Image == null)
				return UIElementStatus.Disable;

            bool canPaste = ClipboardHelper.CanPasteFromClipboard(workspace);

            return canPaste ? UIElementStatus.Enable : UIElementStatus.Disable;
		}


	}
}
