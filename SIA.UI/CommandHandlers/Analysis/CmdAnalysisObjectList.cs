#define WAFER_LIST__

using System;
using System.Windows.Forms;
using System.Drawing;

using SIA.Plugins;
using SIA.Plugins.Common;

using SIA.UI.Controls;
using SIA.UI.Controls.Commands;
using SIA.UI.Controls.Dialogs;
using System.Collections;
using SIA.UI.Controls.Helpers.VisualAnalysis;

namespace SIA.UI.CommandHandlers
{
    internal class CmdAnalysisObjectList : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdAnalysisObjectList";
		
		private static MenuInfo menuInfo = null;
		private static ToolBarInfo toolbarInfo = null;

        public ObjectAnalyzer ObjectAnalyzer
        {
            get
            {
                if (this.Workspace == null)
                    return null;
                return this.Workspace.GetAnalyzer("ObjectAnalyzer") as ObjectAnalyzer;
            }
        }

        static CmdAnalysisObjectList()
		{
            Image image = SIAResources.GetMenuIcon("ObjListIcon");
            menuInfo = new MenuInfo("&Objects List...", Categories.Analysis, Shortcut.None, 640, image, SeparateStyle.Above);
            toolbarInfo = new ToolBarInfo("Show Detected Objects", "Show Detected Objects", 640, image, SeparateStyle.None);
		}

		public CmdAnalysisObjectList(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, menuInfo, toolbarInfo, null)
		{
		}

		public override void DoCommand(params object[] args)
        {
            ImageWorkspace workspace = this.Workspace;

            if (workspace.DetectedObjects == null)
            {
                MessageBoxEx.Error("Please detect objects first.");
                return;
            }

            ObjectAnalyzer analyzer = this.ObjectAnalyzer;
            if (analyzer != null && analyzer.DetectedObjectsWindow != null)
            {
                analyzer.DetectedObjectsWindow.Visible = !analyzer.DetectedObjectsWindow.Visible;
                if (analyzer.DetectedObjectsWindow.Visible)
                    analyzer.Activate();
            }
        }

        private void DlgObjectList_VisibleChanged(object sender, EventArgs e)
        {
            this.appWorkspace.UpdateUI();
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
            if (this.ObjectAnalyzer.DetectedObjectsWindow == null)
                return UIElementStatus.Disable;

            MetrologyAnalyzer metrologyAnalyzer = 
                workspace.GetAnalyzer("MetrologyAnalyzer") as MetrologyAnalyzer;
            if (metrologyAnalyzer != null && metrologyAnalyzer.Visible)
                return UIElementStatus.Disable;

            return UIElementStatus.Enable;
        }

		public override UIElementStatus QueryMenuItemStatus()
		{
            return GetUIStatus();
		}

		public override UIElementStatus QueryToolBarItemStatus()
		{
            if (GetUIStatus() == UIElementStatus.Disable)
                return UIElementStatus.Disable;

			return this.ObjectAnalyzer.DetectedObjectsWindow.Visible ? UIElementStatus.Checked : UIElementStatus.Unchecked;
		}
	}
}

