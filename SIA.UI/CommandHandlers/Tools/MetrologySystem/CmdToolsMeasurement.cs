using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SIA.Plugins.Common;
using System.Windows.Forms;
using SIA.UI.Controls;
using SIA.UI.Controls.Helpers.VisualAnalysis;

namespace SIA.UI.CommandHandlers.Tools
{
    internal class CmdToolsMeasurement : AppWorkspaceCommand
    {
        public const string cmdCommandKey = "CmdToolsMeasurement";

        private static MenuInfo menuInfo = null;
        private static ToolBarInfo toolbarInfo = null;

        private static string cmdDescription = "Perform Measurement";
        private static string cmdImageName = "ruler";
        private static int cmdIndex = 744;

        //public MeasurementAnalyzer MeasurementAnalyzer
        //{
        //    get
        //    {
        //        if (this.Workspace == null)
        //            return null;
        //        return this.Workspace.GetAnalyzer("MeasurementAnalyzer") as MeasurementAnalyzer;
        //    }
        //}

		static CmdToolsMeasurement()
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

        public CmdToolsMeasurement(IAppWorkspace appWorkspace)
            : base(appWorkspace, cmdCommandKey, menuInfo, toolbarInfo, null)
		{
		}

		public override void DoCommand(params object[] args)
		{
            this.Workspace.MeasurementMode();
		}

        private UIElementStatus QueryUIItemStatus()
        {
            ImageWorkspace workspace = this.Workspace;
            if (workspace.Image == null)
                return UIElementStatus.Disable;

            ImageAnalyzer imageAnalyzer = this.Workspace.GetAnalyzer("ImageAnalyzer") as ImageAnalyzer;
            MetrologyAnalyzer metrologyAnalyzer = this.Workspace.GetAnalyzer("MetrologyAnalyzer") as MetrologyAnalyzer;

            if (imageAnalyzer == null || metrologyAnalyzer == null)
                return UIElementStatus.Disable;

            if (!imageAnalyzer.DrawCoordinateSystem && !metrologyAnalyzer.Visible)
                return UIElementStatus.Disable;

            return workspace.IsMeasurementMode ? UIElementStatus.Checked : UIElementStatus.Unchecked;
        }

        public override UIElementStatus QueryToolBarItemStatus()
        {
            return QueryUIItemStatus();
        }

        public override UIElementStatus QueryMenuItemStatus()
        {
            return QueryUIItemStatus();
        }
    }
}
