using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SIA.Plugins.Common;
using System.Windows.Forms;
using SIA.UI.Controls;
using SIA.SystemLayer;
using SIA.UI.Controls.Helpers.VisualAnalysis;
using SIA.UI.Controls.Dialogs;
using SiGlaz.Common.ImageAlignment;
namespace SIA.UI.CommandHandlers.Tools
{
    internal class CmdToolsCreateReferenceFile : AppWorkspaceCommand
    {
        public const string cmdCommandKey = "CmdToolsCreateReferenceFile";
		
		private static MenuInfo menuInfo = null;
		private static ToolBarInfo toolbarInfo = null;
        private static ShortcutInfo shortcutInfo = null;

        public MetrologyAnalyzer MetrologyAnalyzer
        {
            get
            {
                if (this.Workspace == null)
                    return null;
                return this.Workspace.GetAnalyzer("MetrologyAnalyzer") as MetrologyAnalyzer;
            }
        }

        protected static string description = "Create ABS Reference File";
        protected static int commandIndex = 740;

		static CmdToolsCreateReferenceFile()
		{
            int index = commandIndex;
            Image image = SIAResources.GetMenuIcon("coordinate_system");
            menuInfo = new MenuInfo(
                description,
                Categories.Tools, Shortcut.None, index, image, SeparateStyle.Above);
            shortcutInfo = new ShortcutInfo(
                description, Categories.Tools, index, image);
        }

        public CmdToolsCreateReferenceFile(IAppWorkspace appWorkspace) : 
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
                ImageAnalyzer imageAnalyzer = 
                    workspace.GetAnalyzer("ImageAnalyzer") as ImageAnalyzer;
                imageAnalyzer.SetCoordinateSystemVisibleFlag(false);

                MetrologyAnalyzer analyzer = this.MetrologyAnalyzer;
                if (analyzer != null)
                {
                    analyzer.Activate();
                    workspace.Invalidate(true);
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
            return base.QueryMenuItemStatus();
        }

        public override UIElementStatus QueryShortcutBarItemStatus()
        {
            return base.QueryShortcutBarItemStatus();
        }

        public override UIElementStatus QueryToolBarItemStatus()
        {
            return base.QueryToolBarItemStatus();
        }
    }
}
