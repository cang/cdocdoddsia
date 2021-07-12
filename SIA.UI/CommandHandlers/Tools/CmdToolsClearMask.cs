#define SIA_PRODUCT

using System;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

using SIA.Common;
using SIA.Common.Utility;
using SIA.Common.Mask;

using SiGlaz.RDE.Ex.Mask;

using SIA.SystemLayer;
using SIA.SystemLayer.Mask;

using SIA.SystemFrameworks;
using SIA.SystemFrameworks.UI;

using SIA.Plugins;
using SIA.Plugins.Common;

using SIA.UI.Controls;
using SIA.UI.Controls.Commands;
using SIA.UI.Controls.Helpers.VisualAnalysis;

namespace SIA.UI.CommandHandlers
{
    internal class CmdToolsClearMask : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdToolsClearMask";
		
		private static MenuInfo menuInfo = null;
		private static ToolBarInfo toolbarInfo = null;
        private static ShortcutInfo shortcutInfo = null;

		static CmdToolsClearMask()
		{
#if SIA_PRODUCT
            Image image = SIAResources.GetMenuIcon("del_mark");            
            toolbarInfo = new ToolBarInfo("Clear Region", "Clear Region", 720, image, SeparateStyle.None);

            image = SIAResources.CreateMenuIconFromShortcutIcon(image);
            menuInfo = new MenuInfo("&Clear Region", Categories.Tools, Shortcut.None, 720, image, SeparateStyle.None);

            // Cong: using menu icon as  shortcut icon
            //image = RDEResources.GetMenuIcon("LoadMskIcon");
            image = SIAResources.GetShortcutIcon("del_mark");
            shortcutInfo = new ShortcutInfo("Clear Region", Categories.Tools, 720, image);
#else
            Image image = RDEResources.GetMenuIcon("LoadMskIcon");
			menuInfo = new MenuInfo("&Load Mask", Categories.Tools, Shortcut.None, 140, image, SeparateStyle.Above);
			toolbarInfo = new ToolBarInfo("Load Mask", "Load Mask from File", 260, image, SeparateStyle.None);

            // Cong: using menu icon as  shortcut icon
            image = RDEResources.GetMenuIcon("LoadMskIcon");
            shortcutInfo = new ShortcutInfo("Load Mask", Categories.Tools, 1, image);
#endif
        }

		public CmdToolsClearMask(IAppWorkspace appWorkspace): 
            base(appWorkspace, cmdCommandKey, menuInfo, toolbarInfo, shortcutInfo)
		{
		}

        public override void DoCommand(params object[] args)
        {
            if (this.Workspace == null || this.Workspace.Image == null)
                return;

            ImageWorkspace workspace = this.Workspace;
            CommonImage image = workspace.Image;

            try
            {
                if (image.Mask != null)
                {
                    image.Mask.Dispose();
                    image.Mask = null;

                    // display mask if not visibled
                    MaskViewer maskViewer = workspace.GetAnalyzer("MaskViewer") as MaskViewer;
                    if (maskViewer != null)
                    {
                        maskViewer.Visible = false;
                    }
                }
            }
            catch
            {
            }
            finally
            {
            }
        }

        public override UIElementStatus QueryMenuItemStatus()
        {
            ImageWorkspace workspace = this.Workspace;

            if (workspace == null || workspace.Image == null || workspace.Image.Mask == null)
                return UIElementStatus.Disable;

            return UIElementStatus.Enable;

            //return base.QueryMenuItemStatus();
        }

        public override UIElementStatus QueryShortcutBarItemStatus()
        {
            return base.QueryShortcutBarItemStatus();
        }

        public override UIElementStatus QueryToolBarItemStatus()
        {
            ImageWorkspace workspace = this.Workspace;

            if (workspace == null || workspace.Image == null || workspace.Image.Mask == null)
                return UIElementStatus.Disable;

            return UIElementStatus.Enable;
        }
    }
}
