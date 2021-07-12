﻿using System;
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

namespace SIA.UI.CommandHandlers.Analysis
{
    internal class CmdAnalysisAdvancedObjectAnalyzer  : AppWorkspaceCommand
    {
        public const string cmdCommandKey = "CmdAnalysisAdvancedObjectAnalyzer";
		
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

		static CmdAnalysisAdvancedObjectAnalyzer()
		{
            int index = 620;
            string description = "Advanced Object Analyzer";
            string menuImageName = "object_analyzer";
            string shortcutIconName = "object_analyzer";
            // 
            Image shortcutIcon = SIAResources.GetShortcutIcon(shortcutIconName);
            Image menuIcon = SIAResources.CreateMenuIconFromShortcutIcon(shortcutIcon);
            menuInfo =
                new MenuInfo(
                    description, Categories.Analysis,
                    Shortcut.None, index, menuIcon, SeparateStyle.Above);
            toolbarInfo =
                new ToolBarInfo(
                    description, description, index, menuIcon, SeparateStyle.Before);
            shortcutInfo =
                new ShortcutInfo(
                    description, Categories.Analysis, index, shortcutIcon);
		}

        public CmdAnalysisAdvancedObjectAnalyzer(IAppWorkspace appWorkspace)
            : base(appWorkspace, cmdCommandKey, menuInfo, toolbarInfo, shortcutInfo)
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
            if (analyzer != null &&
                analyzer.CombineObjectAnalyzer != null)
            {
                if (analyzer.CombineObjectAnalyzer.Visible == false)
                    analyzer.CombineObjectAnalyzer.Visible = true;

                analyzer.CombineObjectAnalyzer.WindowState = FormWindowState.Normal;
                analyzer.CombineObjectAnalyzer.Focus();
            }

            //ObjectAnalyzer analyzer = this.ObjectAnalyzer;
            //if (analyzer != null &&
            //    analyzer.AdvancedObjectAnalyzer2 != null)
            //{
            //    if (analyzer.AdvancedObjectAnalyzer2.Visible == false)
            //        analyzer.AdvancedObjectAnalyzer2.Visible = true;
                
            //    analyzer.AdvancedObjectAnalyzer2.WindowState = FormWindowState.Normal;
            //    analyzer.AdvancedObjectAnalyzer2.Focus();
            //}
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
