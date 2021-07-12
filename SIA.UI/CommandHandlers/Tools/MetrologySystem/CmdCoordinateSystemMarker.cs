using System;
using System.Collections.Generic;
using System.Text;
using SIA.Plugins.Common;
using System.Windows.Forms;
using SIA.UI.Controls;
using System.Drawing;
using SIA.UI.Controls.Helpers.VisualAnalysis;
namespace SIA.UI.CommandHandlers.Tools
{
    internal class CmdToolsCoordinateSystemMarker : AppWorkspaceCommand
    {
        public const string cmdCommandKey = "CmdToolsCoordinateSystemMarker";
		
		private static MenuInfo menuInfo = null;
        private static ToolBarInfo toolbarInfo = null;

        private static string cmdDescription = "Define Marker";
        private static string cmdImageName = "key_point";
        private static int cmdIndex = 742;

        public MetrologyAnalyzer MetrologyAnalyzer
        {
            get
            {
                if (this.Workspace == null)
                    return null;
                return this.Workspace.GetAnalyzer("MetrologyAnalyzer") as MetrologyAnalyzer;
            }
        }

		static CmdToolsCoordinateSystemMarker()
		{
            Image image = SIAResources.GetMenuIcon(cmdImageName);
            menuInfo =
                new MenuInfo(
                    cmdDescription, 
                    Categories.Tools, 
                    Shortcut.None, 
                    cmdIndex, 
                    image, 
                    SeparateStyle.None);

            toolbarInfo =
                new ToolBarInfo(
                    cmdDescription, 
                    cmdDescription, 
                    cmdIndex, 
                    image,
                    SeparateStyle.None,
                    ToolBarButtonStyle.ToggleButton);
		}

        public CmdToolsCoordinateSystemMarker(IAppWorkspace appWorkspace)
            : base(appWorkspace, cmdCommandKey, menuInfo, toolbarInfo, null)
		{
		}

		public override void DoCommand(params object[] args)
		{
            ImageWorkspace workspace = this.Workspace;

            MetrologyAnalyzer analyzer = this.MetrologyAnalyzer;
            if (analyzer != null)
                analyzer.DrawNewMarker();

            workspace.Invalidate(true);
		}

        private UIElementStatus QueryUIItemStatus()
        {
            ImageWorkspace workspace = this.Workspace;

            if (workspace.Image == null)
                return UIElementStatus.Disable;

            MetrologyAnalyzer analyzer = this.MetrologyAnalyzer;
            if (analyzer != null)
            {
                if (!analyzer.Visible)
                    return UIElementStatus.Disable;

                if (!workspace.IsExtraInteractiveMode)
                    return UIElementStatus.Unchecked;

                return (analyzer.IsDrawNewMarker ? UIElementStatus.Checked : UIElementStatus.Unchecked);
            }

            return UIElementStatus.Disable;
        }

        public override UIElementStatus QueryMenuItemStatus()
        {
            return QueryUIItemStatus();
        }

        public override UIElementStatus QueryToolBarItemStatus()
        {
            return QueryUIItemStatus();
        }
    }
}
