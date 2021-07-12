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
    internal class CmdToolsAlignmentConfiguration : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdToolsAlignmentConfiguration";
		
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

        protected string _description = "Configure Alignment Settings";
        protected int _index = 740;

		static CmdToolsAlignmentConfiguration()
		{
            int index = 740;
            Image image = SIAResources.GetMenuIcon("align_ABS");
            //toolbarInfo = new ToolBarInfo(
            //    "Configure Alignment Settings",
            //    "Configure Alignment Settings", index, image, SeparateStyle.None);

            //image = SIAResources.CreateMenuIconFromShortcutIcon(image);
            menuInfo = new MenuInfo(
                "Configure Alignment Settings",
                Categories.Tools, Shortcut.None, index, image, SeparateStyle.None);

            // Cong: using menu icon as  shortcut icon
            //image = RDEResources.GetMenuIcon("LoadMskIcon");
            //image = SIAResources.GetShortcutIcon("align_ABS");
            shortcutInfo = new ShortcutInfo(
                "Configure Alignment Settings",                 
                Categories.Tools, index, image);
        }

        public CmdToolsAlignmentConfiguration(IAppWorkspace appWorkspace) : 
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
                        UpdateDefaultSettings(dlg);

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

        protected virtual void UpdateDefaultSettings(DlgAlignmentConfigurationHelper dlg)
        {
            throw new System.NotImplementedException();
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
