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
	internal class CmdViewZoomIn : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdZoomIn";
		
		private static ToolBarInfo toolbarInfo = null;

		static CmdViewZoomIn()
		{
			Image image = SIAResources.GetMenuIcon("ZoomInIcon");
			toolbarInfo = 
                new ToolBarInfo(
                    "Zoom In", "Zoom in", 380, image, SeparateStyle.None, ToolBarButtonStyle.ToggleButton);
		}

		public CmdViewZoomIn(IAppWorkspace appWorkspace)
            : base(appWorkspace, cmdCommandKey, null, toolbarInfo, null)
		{
		}

		public override void DoCommand(params object[] args)
		{
            this.Workspace.ZoomInMode();
		}

		public override UIElementStatus QueryToolBarItemStatus()
		{
			ImageWorkspace workspace = this.Workspace;
			if (workspace.Image == null)
				return UIElementStatus.Disable;
			return workspace.IsZoomIn ? UIElementStatus.Checked : UIElementStatus.Unchecked;

		}

	}
}
