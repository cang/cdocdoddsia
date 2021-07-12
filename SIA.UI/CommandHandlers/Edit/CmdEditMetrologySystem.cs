using System;
using System.Collections.Generic;
using System.Text;
using SIA.UI.Controls;
using SIA.Plugins.Common;
using System.Windows.Forms;
using SIA.UI.Controls.Helpers.VisualAnalysis;

namespace SIA.UI.CommandHandlers.Edit
{
    internal class CmdEditMetrologySystem : AppWorkspaceCommand
    {
        public const string cmdCommandKey = "CmdEditMetrologySystem";
		
		private static MenuInfo menuInfo = null;

		static CmdEditMetrologySystem()
		{
			menuInfo = new MenuInfo(
                "View/Edit Metrology System", 
                Categories.Edit, Shortcut.None, 250, 
				null, SeparateStyle.Above);
		}

        public CmdEditMetrologySystem(IAppWorkspace appWorkspace)
            : base(appWorkspace, cmdCommandKey, menuInfo, null, null)
		{
		}

        public MetrologyAnalyzer Analyzer
        {
            get
            {
                if (this.Workspace == null)
                    return null;

                return this.Workspace.GetAnalyzer("MetrologyAnalyzer") as MetrologyAnalyzer;
            }
        }

		public override void DoCommand(params object[] args)
		{
            MetrologyAnalyzer analyzer = this.Analyzer;

            if (analyzer.MetrologySystemWindow.Visible == false)
                analyzer.MetrologySystemWindow.Visible = true;

            if (Workspace.ActiveAnalyzer != analyzer)
                Workspace.ActiveAnalyzer = analyzer;

		}

		public override UIElementStatus QueryMenuItemStatus()
		{
            return base.QueryMenuItemStatus();
		}
    }
}
