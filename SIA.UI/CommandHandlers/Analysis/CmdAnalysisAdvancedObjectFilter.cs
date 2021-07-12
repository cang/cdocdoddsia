using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SIA.Plugins.Common;
using SIA.UI.Controls;
using SIA.SystemLayer;
using SIA.SystemFrameworks.UI;
using SIA.UI.Controls.Utilities;
using SIA.UI.Controls.Commands;
using System.Windows.Forms;
using SIA.UI.Controls.Helpers.VisualAnalysis;
using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.Automation.Commands;
using System.Collections;
using SIA.UI.Controls.Automation;

namespace SIA.UI.CommandHandlers.Analysis
{
    internal class CmdAnalysisAdvancedObjectFilter : AppWorkspaceCommand
    {
        public const string cmdCommandKey = "CmdAnalysisAdvancedObjectFilter";
		
		private static MenuInfo menuInfo = null;
		private static ToolBarInfo toolbarInfo = null;
		private static ShortcutInfo shortcutInfo = null;

        public ObjectAnalyzer ObjectAnalyzer
        {
            get
            {
                if (this.Workspace == null)
                    return null;
                return this.Workspace.GetAnalyzer("ObjectAnalyzer") as ObjectAnalyzer;
            }
        }

		static CmdAnalysisAdvancedObjectFilter()
		{
            int index = 621;
            string description = "Filter Object";
            string shortcutIconName = "filter_object";
            // 
            Image shortcutIcon = SIAResources.GetShortcutIcon(shortcutIconName);
            Image menuIcon = SIAResources.CreateMenuIconFromShortcutIcon(shortcutIcon);
            menuInfo =
                new MenuInfo(
                    description, Categories.Analysis,
                    Shortcut.None, index, menuIcon, SeparateStyle.Above);
            //toolbarInfo =
            //    new ToolBarInfo(
            //        description, description, index, menuIcon, SeparateStyle.Before);
            shortcutInfo =
                new ShortcutInfo(
                    description, Categories.Analysis, index, shortcutIcon);
		}

        public CmdAnalysisAdvancedObjectFilter(IAppWorkspace appWorkspace)
            : base(appWorkspace, cmdCommandKey, menuInfo, toolbarInfo, shortcutInfo)
		{
			
		}

        public override void DoCommand(params object[] args)
        {
            ImageWorkspace workspace = this.Workspace;

            if (workspace.DetectedObjects == null || workspace.DetectedObjects.Count == 0)
            {
                MessageBoxEx.Error("Please detect objects first.");
                return;
            }

            //string filePath = ObjectFilterSettings.SIADefaultFilePath;
            //ObjectFilterSettings filterSettings = 
            //    ObjectFilterSettings.Deserialize(filePath);
            //if (filterSettings == null)
            //    filterSettings = new ObjectFilterSettings();

            //DlgObjectFilterEx dlg = new DlgObjectFilterEx(filterSettings);
            //if (dlg.ShowDialog(workspace.AppWorkspace) == DialogResult.OK)
            //{
            //    filterSettings = dlg.Settings;

            //    ArrayList objList = new ArrayList(workspace.DetectedObjects.Count);
            //    objList.AddRange(workspace.DetectedObjects);

            //    SimpleFilterCommand.ApplyFilter(objList , filterSettings);
                
            //    workspace.DetectedObjects = objList;
            //    workspace.Invalidate(true);

            //    RasterCommandSettingsSerializer.Serialize(filePath, filterSettings);
            //}


            ObjectAnalyzer analyzer = this.ObjectAnalyzer;
            if (analyzer != null &&
                analyzer.FilterObjectAnalyzer != null)
            {
                if (analyzer.FilterObjectAnalyzer.Visible == false)
                    analyzer.FilterObjectAnalyzer.Visible = true;

                analyzer.FilterObjectAnalyzer.WindowState = FormWindowState.Normal;
                analyzer.FilterObjectAnalyzer.Focus();
            }
        }

        protected UIElementStatus GetUIStatus()
        {
            if (this.Workspace == null)
                return UIElementStatus.Disable;
            if (this.Workspace.DetectedObjects == null)
                return UIElementStatus.Disable;
            if (this.Workspace.DetectedObjects.Count == 0)
                return UIElementStatus.Disable;

            MetrologyAnalyzer analyzer = this.Workspace.GetAnalyzer("MetrologyAnalyzer") as MetrologyAnalyzer;
            if (analyzer != null && analyzer.Visible)
                return UIElementStatus.Disable;

            return UIElementStatus.Enable;
        }

        public override UIElementStatus QueryMenuItemStatus()
        {
            return GetUIStatus();
        }

        public override UIElementStatus QueryToolBarItemStatus()
        {
            return GetUIStatus();
        }

        public override UIElementStatus QueryShortcutBarItemStatus()
        {
            return GetUIStatus();
        }
    }
}
