namespace SIA.UI.CommandHandlers
{
	using System;
	using System.Windows.Forms;
	using System.Drawing;

	using SIA.Plugins;
	using SIA.Plugins.Common;

	using SIA.UI.Controls;
	using SIA.UI.Controls.Commands;

	internal class CmdViewActualSize : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdViewActualSize";
		
		private static MenuInfo menuInfo = null;
		private static ToolBarInfo toolbarInfo = null;

		static CmdViewActualSize()
		{
			Image image = SIAResources.GetMenuIcon("ActualSizeIcon");			
			menuInfo = 
                new MenuInfo("&Actual Size", Categories.View, Shortcut.None, 392, image, SeparateStyle.None);
			toolbarInfo = 
                new ToolBarInfo("Actual Size", "Actual Size", 392, image, SeparateStyle.After);
		}

		public CmdViewActualSize(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, menuInfo, toolbarInfo, null)
		{
		}

		public override void DoCommand(params object[] args)
		{
            ImageWorkspace workspace = this.Workspace;
            workspace.ImageViewer.ZoomActualSize();
		}

		public override UIElementStatus QueryMenuItemStatus()
		{
			ImageWorkspace workspace = this.Workspace;
			return workspace.Image == null ? UIElementStatus.Disable : UIElementStatus.Enable;
		}

		public override UIElementStatus QueryToolBarItemStatus()
		{
			ImageWorkspace workspace = this.Workspace;
			return workspace.Image == null ? UIElementStatus.Disable : UIElementStatus.Enable;
		}

	}
}

