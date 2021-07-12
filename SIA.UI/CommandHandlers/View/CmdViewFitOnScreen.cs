namespace SIA.UI.CommandHandlers
{
	using System;
	using System.Windows.Forms;
	using System.Drawing;

	using SIA.Plugins;
	using SIA.Plugins.Common;

	using SIA.UI.Controls;
	using SIA.UI.Controls.Commands;

	internal class CmdViewFitOnScreen : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdViewFitOnScreen";
		
		private static MenuInfo menuInfo = null;
		private static ToolBarInfo toolbarInfo = null;

		static CmdViewFitOnScreen()
		{
			Image image = SIAResources.GetMenuIcon("FitOnScreenIcon");
			menuInfo = 
                new MenuInfo("&Fit On Screen", Categories.View, Shortcut.None, 391, image, SeparateStyle.Above);
			toolbarInfo = 
                new ToolBarInfo("Fit On Screen", "Fit On Screen", 391, image, SeparateStyle.None);

		}

		public CmdViewFitOnScreen(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, menuInfo, toolbarInfo, null)
		{
		}

		public override void DoCommand(params object[] args)
		{
            ImageWorkspace workspace = this.Workspace;
            workspace.ImageViewer.ZoomToFit();
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

