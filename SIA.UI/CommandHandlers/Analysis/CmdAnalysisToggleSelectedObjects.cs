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
    internal class CmdAnalysisToggleSelectedObjects : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdToggleSelectedObjects";
		
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

		static CmdAnalysisToggleSelectedObjects()
		{
#if WAFER_LIST
            Image normImage = RDEResources.GetMenuIcon("ShowSelObjIcon");
            Image actImage = RDEResources.GetMenuIcon("HideSelObjIcon");
            toolbarInfo = new ToolBarInfo("Toggle Selected Wafers", "Toggle Selected Wafers", 210,
                normImage, actImage, SeparateStyle.After, ToolBarButtonStyle.ToggleButton);
            menuInfo = new MenuInfo("Toggle Selected Wafers", Categories.Analysis, Shortcut.None, 5, normImage);
#else
			Image normImage = SIAResources.GetMenuIcon("ShowSelObjIcon");
			Image actImage = SIAResources.GetMenuIcon("HideSelObjIcon");
			toolbarInfo = new ToolBarInfo("Toggle Selected Objects", "Toggle Selected Objects", 670, 
				normImage, actImage, SeparateStyle.None, ToolBarButtonStyle.ToggleButton);
            menuInfo = new MenuInfo("Toggle Selected Objects", Categories.Analysis, Shortcut.None, 670, normImage);
#endif
		}

		public CmdAnalysisToggleSelectedObjects(IAppWorkspace appWorkspace)
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
                analyzer.HighlightSelectedObjects = !analyzer.HighlightSelectedObjects;
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

			bool bSelected = this.ObjectAnalyzer != null && this.ObjectAnalyzer.HighlightSelectedObjects;
			return bSelected ? UIElementStatus.Checked : UIElementStatus.Unchecked;
		}

	}
}
