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

namespace SIA.UI.CommandHandlers
{
    internal class CmdToolsPoleTipAlignmentConfiguration : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdToolsPoleTipAlignmentConfiguration";
		
		private static MenuInfo menuInfo = null;
		private static ToolBarInfo toolbarInfo = null;
        private static ShortcutInfo shortcutInfo = null;

        public AlignmentConfigurationHelper AlignmentConfigurationHelper
        {
            get
            {
                if (this.Workspace == null)
                    return null;
                return this.Workspace.GetAnalyzer("AlignmentConfigurationHelper") as AlignmentConfigurationHelper;
            }
        }

        protected static string description = "Configure Pole-Tip Alignment Settings";
        protected static int commandIndex = 742;

		static CmdToolsPoleTipAlignmentConfiguration()
		{
            int index = commandIndex;
            Image image = SIAResources.GetMenuIcon("align_ABS");
            menuInfo = new MenuInfo(
                description,
                Categories.Tools, Shortcut.None, index, image, SeparateStyle.None);
            shortcutInfo = new ShortcutInfo(
                description, Categories.Tools, index, image);
        }

        public CmdToolsPoleTipAlignmentConfiguration(IAppWorkspace appWorkspace) : 
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
                if (this.AlignmentConfigurationHelper != null)
                {
                    AlignmentConfigurationHelper analyzer = this.AlignmentConfigurationHelper;

                    if (workspace.ActiveAnalyzer != analyzer)
                    {
                        workspace.ActiveAnalyzer = analyzer;
                        workspace.Invalidate(true);
                    }

                    DlgAlignmentConfigurationHelper dlg = analyzer.Configuration;
                    if (dlg != null && dlg.Visible == false)
                    {
                        if (dlg.IsABSAlignment)
                        {
                            dlg.UpdateSettings(
                                Settings.GetDefaultPoleTipAlignmentSettings(), false);
                            dlg.Text = "Pole-Tip Alignment Configuration";
                        }

                        dlg.Visible = true;
                        workspace.Invalidate(true);
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
