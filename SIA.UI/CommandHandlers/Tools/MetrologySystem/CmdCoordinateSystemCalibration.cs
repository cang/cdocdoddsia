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
    internal class CmdToolsCoordinateSystemCalibration : AppWorkspaceCommand
    {
        public const string cmdCommandKey = "CmdToolsCoordinateSystemCalibration";
		
		private static MenuInfo menuInfo = null;
        private static ToolBarInfo toolbarInfo = null;

        private static string cmdDescription = "Calibrate";
        private static string cmdImageName = "key_line";
        private static int cmdIndex = 743;

        public MetrologyAnalyzer MetrologyAnalyzer
        {
            get
            {
                if (this.Workspace == null)
                    return null;
                return this.Workspace.GetAnalyzer("MetrologyAnalyzer") as MetrologyAnalyzer;
            }
        }

		static CmdToolsCoordinateSystemCalibration()
		{
            Image image = SIAResources.GetMenuIcon(cmdImageName);
            menuInfo =
                new MenuInfo(
                    cmdDescription, 
                    Categories.Tools, 
                    Shortcut.None, 
                    cmdIndex, 
                    image, 
                    SeparateStyle.Below);

            toolbarInfo =
                new ToolBarInfo(
                    cmdDescription, 
                    cmdDescription, 
                    cmdIndex, 
                    image,
                    SeparateStyle.After,
                    ToolBarButtonStyle.ToggleButton);
		}

        public CmdToolsCoordinateSystemCalibration(IAppWorkspace appWorkspace)
            : base(appWorkspace, cmdCommandKey, menuInfo, toolbarInfo, null)
		{
		}

		public override void DoCommand(params object[] args)
		{
            ImageWorkspace workspace = this.Workspace;

            MetrologyAnalyzer analyzer = this.MetrologyAnalyzer;
            if (analyzer != null)
                analyzer.Calibrate();

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

                return (analyzer.IsCalibration ? UIElementStatus.Checked : UIElementStatus.Unchecked);
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
