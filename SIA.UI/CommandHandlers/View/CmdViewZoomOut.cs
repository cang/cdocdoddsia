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
	internal class CmdViewZoomOut : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdZoomOut";
		
		private static ToolBarInfo toolbarInfo = null;

		static CmdViewZoomOut()
		{
			Image image = SIAResources.GetMenuIcon("ZoomOutIcon");
			toolbarInfo = 
                new ToolBarInfo(
                    "Zoom Out", "Zoom Out", 390, image, SeparateStyle.None, ToolBarButtonStyle.ToggleButton);
		}

		public CmdViewZoomOut(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, null, toolbarInfo, null)
		{
		}

		public override void DoCommand(params object[] args)
		{
            this.Workspace.ZoomOutMode();
		}

		public override UIElementStatus QueryToolBarItemStatus()
		{
			ImageWorkspace workspace = this.Workspace;
			if (workspace.Image == null)
				return UIElementStatus.Disable;
			return workspace.IsZoomOut ? UIElementStatus.Checked : UIElementStatus.Unchecked;
		}

	}
}
