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
    internal class CmdEditUndo : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdEditUndo";
		
		private static MenuInfo menuInfo = null;
		private static ToolBarInfo toolbarInfo = null;

		static CmdEditUndo()
		{
			Image image = SIAResources.GetMenuIcon("UndoIcon");
			menuInfo = 
                new MenuInfo(
                    "&Undo", Categories.Edit, Shortcut.CtrlZ, 200, image, SeparateStyle.Above);
			toolbarInfo = 
                new ToolBarInfo(
                    "Undo", "Undo last action", 200, image, SeparateStyle.Before);
		}

		public CmdEditUndo(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, menuInfo, toolbarInfo, null)
		{
		}

		public override void DoCommand(params object[] args)
		{
            IAppWorkspace appWorkspace = this.appWorkspace;
            ImageWorkspace workspace = this.Workspace;

            try
            {
				using (CommandExecutor cmdExec = new CommandExecutor(workspace))
					cmdExec.DoCommandUndo(workspace.HistoryHelper);
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
            else if (workspace.HistoryHelper.IsOnTopOfHistoryList == true)
                return UIElementStatus.Disable;

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
			else if (workspace.HistoryHelper.IsOnTopOfHistoryList == true)
				return UIElementStatus.Disable;

			return UIElementStatus.Enable;
		}

	}
}
