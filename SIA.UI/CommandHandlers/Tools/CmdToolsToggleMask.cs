#define SIA_PRODUCT

using System;
using System.Windows.Forms;
using System.Drawing;

using SIA.Plugins;
using SIA.Plugins.Common;

using SIA.UI.Controls;
using SIA.UI.Controls.Commands;
using SIA.UI.Controls.Helpers.VisualAnalysis;

namespace SIA.UI.CommandHandlers
{
	internal class CmdToolsToggleMask : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdToolsToggleMask";
		
		private static MenuInfo menuInfo = null;
		private static ToolBarInfo toolbarInfo = null;

        //public MaskViewer MaskViewer
        //{
        //    get
        //    {
        //        return this.Workspace.GetAnalyzer("MaskViewer") as MaskViewer;
        //    }
        //}

		static CmdToolsToggleMask()
		{
#if SIA_PRODUCT
            Image normImage = SIAResources.GetMenuIcon("ShowMskIcon");
            Image actImage = SIAResources.GetMenuIcon("HideMskIcon");
            menuInfo = new MenuInfo("Toggle Mas&k", Categories.Tools, Shortcut.None, 730, normImage, SeparateStyle.None);
            toolbarInfo = new ToolBarInfo(
                "Toggle Region", "Toggle Region", 730, normImage, actImage, SeparateStyle.After, ToolBarButtonStyle.ToggleButton);
#else
			Image normImage = RDEResources.GetMenuIcon("ShowMskIcon");
			Image actImage = RDEResources.GetMenuIcon("HideMskIcon");
			//menuInfo = new MenuInfo("Toggle Mas&k", Categories.Tools, Shortcut.None, 150, null, SeparateStyle.Below);
            toolbarInfo = new ToolBarInfo("Toggle Mask", "Toggle Mask", 270, normImage, actImage, SeparateStyle.After, ToolBarButtonStyle.ToggleButton);
#endif
		}

		public CmdToolsToggleMask(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, menuInfo, toolbarInfo, null)
		{
		}

		public override void DoCommand(params object[] args)
		{
            bool bStatus = !Workspace.ShowRegion;
            Workspace.ShowRegion = bStatus;
            
            //MaskViewer maskViewer = this.MaskViewer;
            //if (maskViewer != null)
            //{
            //    bool bStatus = !maskViewer.Visible;
            //    maskViewer.Visible = bStatus;
            //}
		}

        private UIElementStatus QueryUIItemStatus()
        {
            ImageWorkspace workspace = this.Workspace;
            if (workspace.Image == null)
                return UIElementStatus.Disable;

            if (!workspace.HasRegion)
                return UIElementStatus.Disable;

            return workspace.ShowRegion ? UIElementStatus.Checked : UIElementStatus.Unchecked;
        }

		public override UIElementStatus QueryMenuItemStatus()
		{
            return QueryUIItemStatus();
			//return base.QueryMenuItemStatus();
		}

		public override UIElementStatus QueryToolBarItemStatus()
		{
            return QueryUIItemStatus();
            //ImageWorkspace workspace = this.Workspace;
            //if (workspace.Image == null || workspace.Image.Mask == null)
            //    return UIElementStatus.Disable;

            //if (this.MaskViewer.IsMaskAvailable == false)
            //    return UIElementStatus.Disable;
            //return this.MaskViewer.Visible ? UIElementStatus.Checked : UIElementStatus.Unchecked;
		}
	}
}

