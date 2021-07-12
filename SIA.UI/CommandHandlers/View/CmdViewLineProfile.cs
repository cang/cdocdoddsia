using System;
using System.Windows.Forms;
using System.Drawing;

using SIA.Plugins;
using SIA.Plugins.Common;

using SIA.UI.Controls;
using SIA.UI.Controls.Common;
using SIA.UI.Controls.Commands;
using SIA.UI.Controls.UserControls;
using SIA.UI.Controls.Helpers.VisualAnalysis;

namespace SIA.UI.CommandHandlers
{
	internal class CmdViewLineProfile : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdViewLineProfile";
		
		private static MenuInfo menuInfo = null;
		private static ShortcutInfo shortcutInfo = null;

        public DataProfileHelper DataProfileHelper
        {
            get
            {
                if (this.Workspace == null)
                    return null;
                return this.Workspace.GetAnalyzer("DataProfileHelper") as DataProfileHelper;
            }
        }
		
		static CmdViewLineProfile()
		{
            System.Drawing.Image shortcutImage =
                SIAResources.GetShortcutIcon("lineprofile");
            System.Drawing.Image menuImage =
                SIAResources.CreateMenuIconFromShortcutIcon(shortcutImage);

			menuInfo = 
                new MenuInfo(
                "&Line Profile", Categories.View, Shortcut.None, 300, menuImage, SeparateStyle.Above);			
			shortcutInfo = 
                new ShortcutInfo(
                    "Line Profile", Categories.View, 300, shortcutImage);			
		}

		public CmdViewLineProfile(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, menuInfo, null, shortcutInfo)
		{
		}

		public override void DoCommand(params object[] args)
		{
            ImageWorkspace workspace = this.Workspace;
		    DataProfileHelper dataProfiler = this.DataProfileHelper;
            if (dataProfiler != null)
            {
                dataProfiler.PlotType = PlotType.Line;
                dataProfiler.Activate();
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

	}
}

