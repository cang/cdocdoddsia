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
	internal class CmdViewPan : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdPanMode";
		
		private static ToolBarInfo toolbarInfo = null;

		static CmdViewPan()
		{
			Image image = SIAResources.GetMenuIcon("HandIcon");
			toolbarInfo = 
                new ToolBarInfo(
                "Pan", "Pan", 370, image, SeparateStyle.None, ToolBarButtonStyle.ToggleButton);
		}

		public CmdViewPan(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, null, toolbarInfo, null)
		{
		}

		public override void DoCommand(params object[] args)
		{
			this.Workspace.PanMode();
		}

		public override UIElementStatus QueryToolBarItemStatus()
		{
			ImageWorkspace workspace = this.Workspace;
			if (workspace.Image == null)
				return UIElementStatus.Disable;
			return workspace.IsPan ? UIElementStatus.Checked : UIElementStatus.Unchecked;
		}

	}
}
