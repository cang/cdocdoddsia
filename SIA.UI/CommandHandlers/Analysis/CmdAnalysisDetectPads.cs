using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SIA.Plugins.Common;

namespace SIA.UI.CommandHandlers.Analysis
{
    internal class CmdAnalysisDetectPads : AppWorkspaceCommand
    {
        public const string cmdCommandKey = "CmdAnalysisDetectPads";
		
		private static MenuInfo menuInfo = null;
		private static ToolBarInfo toolbarInfo = null;
		private static ShortcutInfo shortcutInfo = null;
        
		static CmdAnalysisDetectPads()
		{
            // 
            Image image = SIAResources.GetMenuIcon("coordinate_system");
            shortcutInfo =
                new ShortcutInfo("Detect Coordinate System", Categories.Analysis, 630, image);			
		}

        public CmdAnalysisDetectPads(IAppWorkspace appWorkspace)
            : base(appWorkspace, cmdCommandKey, menuInfo, toolbarInfo, shortcutInfo)
		{			
		}

        public override void DoCommand(params object[] args)
        {
            throw new NotImplementedException();
        }

        public override UIElementStatus QueryShortcutBarItemStatus()
        {
            return base.QueryShortcutBarItemStatus();
        }
    }
}
