using System;
using System.Windows.Forms;

using SIA.Plugins;
using SIA.Plugins.Common;

using SIA.UI.Controls;
using SIA.UI.Controls.Common;
using SIA.UI.Controls.Commands;
using SIA.UI.Controls.UserControls;
using SIA.UI.Controls.Helpers.VisualAnalysis;

namespace SIA.UI.CommandHandlers
{

	internal class CmdViewBoxProfile : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdViewBoxProfile";
		
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

		static CmdViewBoxProfile()
		{
            System.Drawing.Image shortcutImage =
                SIAResources.GetShortcutIcon("boxprofile");
            System.Drawing.Image menuImage =
                SIAResources.CreateMenuIconFromShortcutIcon(shortcutImage);

			menuInfo = 
                new MenuInfo(
                    "&Box Profile", Categories.View, Shortcut.None, 310, menuImage);
			shortcutInfo = 
                new ShortcutInfo(
                    "Box Profile", Categories.View, 310, shortcutImage);			
		}

		public CmdViewBoxProfile(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, menuInfo, null, shortcutInfo)
		{
		}

		public override void DoCommand(params object[] args)
		{
			DataProfileHelper dataProfiler = this.DataProfileHelper;
            if (dataProfiler != null)
            {
                dataProfiler.PlotType = PlotType.HorizontalBox;
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
