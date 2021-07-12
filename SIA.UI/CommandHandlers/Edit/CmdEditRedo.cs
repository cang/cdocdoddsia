using System;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

using SIA.Plugins;
using SIA.Plugins.Common;

using SIA.UI.Controls;
using SIA.UI.Controls.Commands;
using SIA.SystemLayer;

namespace SIA.UI.CommandHandlers
{
    internal class CmdEditRedo : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdEditRedo";
		
		private static MenuInfo menuInfo = null;
		private static ToolBarInfo toolbarInfo = null;

		static CmdEditRedo()
		{
			Image image = SIAResources.GetMenuIcon("RedoIcon");
			menuInfo = new MenuInfo(
                "&Redo", Categories.Edit, Shortcut.CtrlY, 210, image, SeparateStyle.Below);
			toolbarInfo = new ToolBarInfo(
                "Redo", "Redo last action", 210, image, SeparateStyle.After);
		}

		public CmdEditRedo(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, menuInfo, toolbarInfo, null)
		{
		}

		public override void DoCommand(params object[] args)
		{
            IAppWorkspace appWorkspace = this.appWorkspace;
            ImageWorkspace workspace = this.Workspace;
            
            try
            {
                using (CommandExecutor cmdExec = new CommandExecutor(workspace))
					cmdExec.DoCommandRedo(workspace.HistoryHelper);
            }
            catch (System.Exception exp)
            {
                Trace.WriteLine(exp);
            }
		}

		public override UIElementStatus QueryMenuItemStatus()
		{
			ImageWorkspace workspace = this.Workspace;
			if (workspace.Image == null)
				return UIElementStatus.Disable;

            if (workspace.HistoryHelper == null)
                return UIElementStatus.Disable;
            else if (workspace.HistoryHelper.Histories.Count < 2)
				return UIElementStatus.Disable;
            else if (workspace.HistoryHelper.IsAtBottomOfHistoryList == true)
				return UIElementStatus.Disable;
			else 
				return UIElementStatus.Enable;
		}

		public override UIElementStatus QueryToolBarItemStatus()
		{
			ImageWorkspace workspace = this.Workspace;
			if (workspace.Image == null)
				return UIElementStatus.Disable;

            if (workspace.HistoryHelper == null)
                return UIElementStatus.Disable;
            else if (workspace.HistoryHelper.Histories.Count < 2)
                return UIElementStatus.Disable;
            else if (workspace.HistoryHelper.IsAtBottomOfHistoryList == true)
                return UIElementStatus.Disable;
            else
                return UIElementStatus.Enable;
		}


	}
}
