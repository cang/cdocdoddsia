#define WAFER_LIST__

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
    internal class CmdAnalysisToggleDetectedObjects : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdToggleDetectedObjects";

        private static ToolBarInfo toolbarInfo = null;
        private static MenuInfo menuInfo = null;

        public ObjectAnalyzer ObjectAnalyzer
        {
            get
            {
                if (this.Workspace == null)
                    return null;
                return this.Workspace.GetAnalyzer("ObjectAnalyzer") as ObjectAnalyzer;
            }
        }

		static CmdAnalysisToggleDetectedObjects()
		{
#if WAFER_LIST
            Image normImage = RDEResources.GetMenuIcon("ShowDetObjIcon");
            Image actImage = RDEResources.GetMenuIcon("HideDetObjIcon");
            toolbarInfo = new ToolBarInfo("Toggle Detected Wafers", "Toggle Detected Wafers", 200,
                normImage, actImage, SeparateStyle.None, ToolBarButtonStyle.ToggleButton);
            menuInfo = new MenuInfo("Toggle Detected Wafers", Categories.Analysis, Shortcut.None, 4, normImage);
#else
			Image normImage = SIAResources.GetMenuIcon("ShowDetObjIcon");
			Image actImage = SIAResources.GetMenuIcon("HideDetObjIcon");
			toolbarInfo = new ToolBarInfo("Toggle Detected Objects", "Toggle Detected Objects", 650,
                normImage, actImage, SeparateStyle.None, ToolBarButtonStyle.ToggleButton);
            menuInfo = new MenuInfo("Toggle Detected Objects", Categories.Analysis, Shortcut.None, 650, normImage, SeparateStyle.None);
#endif
		}

		public CmdAnalysisToggleDetectedObjects(IAppWorkspace appWorkspace)
            : base(appWorkspace, cmdCommandKey, menuInfo, toolbarInfo, null)
		{
		}

		public override void DoCommand(params object[] args)
		{
            ImageWorkspace workspace = this.Workspace;

            if (workspace.DetectedObjects == null)
            {
#if WAFER_LIST
                MessageBoxEx.Error("Please detect wafers first.");
#else
                MessageBoxEx.Error("Please detect objects first.");
#endif
                return;
            }

            ObjectAnalyzer analyzer = this.ObjectAnalyzer;
            if (analyzer != null)
            {
                analyzer.Visible = true;
                bool visible = analyzer.DrawAllObjects;
                analyzer.DrawAllObjects = !visible;
            }

            workspace.Invalidate(true);
		}



        public override UIElementStatus QueryMenuItemStatus()
        {
            return this.GetStatus();
        }

		public override UIElementStatus QueryToolBarItemStatus()
		{
            return this.GetStatus();
        }

        protected virtual UIElementStatus GetUIStatus()
        {
            ImageWorkspace workspace = this.Workspace;
            if (workspace.Image == null)
                return UIElementStatus.Disable;
            if (workspace.DetectedObjects == null)
                return UIElementStatus.Disable;
            if (this.ObjectAnalyzer == null)
                return UIElementStatus.Disable;

            MetrologyAnalyzer metrologyAnalyzer =
                workspace.GetAnalyzer("MetrologyAnalyzer") as MetrologyAnalyzer;
            if (metrologyAnalyzer != null && metrologyAnalyzer.Visible)
                return UIElementStatus.Disable;

            return UIElementStatus.Enable;
        }

		public UIElementStatus GetStatus()
		{
            if (GetUIStatus() == UIElementStatus.Disable)
                return UIElementStatus.Disable;

			bool bSelected = this.ObjectAnalyzer != null && this.ObjectAnalyzer.DrawAllObjects;
			return bSelected ? UIElementStatus.Checked : UIElementStatus.Unchecked;
		}

	}
}
