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
	internal class CmdView3DAreaPlot : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdView3DAreaPlot";
		
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

		static CmdView3DAreaPlot()
		{
            System.Drawing.Image shortcutImage =
                SIAResources.GetShortcutIcon("3dareaplot");
            System.Drawing.Image menuImage =
                SIAResources.CreateMenuIconFromShortcutIcon(shortcutImage);

			menuInfo = 
                new MenuInfo(
                "&3D Area Plot", Categories.View, Shortcut.None, 320, menuImage, SeparateStyle.Below);
			shortcutInfo = 
                new ShortcutInfo(
                    "3D Area Plot", Categories.View, 320, shortcutImage);			
		}

		public CmdView3DAreaPlot(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, menuInfo, null, shortcutInfo)
		{
		}

		public override void DoCommand(params object[] args)
		{
			DataProfileHelper dataProfiler = this.DataProfileHelper;
            if (dataProfiler != null)
            {
                dataProfiler.Visible = true;                
                dataProfiler.PlotType = PlotType.AreaPlot;
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

