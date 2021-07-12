using System;
using System.Windows.Forms;
using System.Drawing;

using SIA.Plugins;
using SIA.Plugins.Common;

using SIA.UI.Controls;
using SIA.UI.Controls.Common;
using SIA.UI.Controls.Commands;

namespace SIA.UI.CommandHandlers
{
	internal class CmdViewSelectMode : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdSelectMode";
		
		private static ToolBarInfo toolbarInfo = null;

		static CmdViewSelectMode()
		{
			Image image = SIAResources.GetMenuIcon("SelectIcon");
			toolbarInfo = 
                new ToolBarInfo(
                    "Select", "Select", 360, image, SeparateStyle.Before, ToolBarButtonStyle.ToggleButton);
		}

		public CmdViewSelectMode(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, null, toolbarInfo, null)
		{
		}

		public override void DoCommand(params object[] args)
		{
			this.Workspace.SelectMode();
		}

		public override UIElementStatus QueryToolBarItemStatus()
		{
			ImageWorkspace workspace = this.Workspace;
			if (workspace.Image == null)
				return UIElementStatus.Disable;
			return workspace.IsSelect ? UIElementStatus.Checked : UIElementStatus.Unchecked;
		}
	}
}
