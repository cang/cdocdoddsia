using System;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

using SIA.Common;

using SIA.SystemFrameworks;
using SIA.SystemFrameworks.UI;

using SIA.SystemLayer;

using SIA.Plugins;
using SIA.Plugins.Common;

using SIA.UI.Controls;
using SIA.UI.Controls.Commands;
using SIA.UI.Controls.Dialogs;
using System.Drawing;

namespace SIA.UI.CommandHandlers
{
	internal class CmdFiltersRank : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdFiltersRank";
		
		private static MenuInfo menuInfo = null;
		private static ShortcutInfo shortcutInfo = null;

		static CmdFiltersRank()
		{
            Image shortcutImage =
                SIAResources.GetShortcutIcon("rankfilter");
            Image menuImage =
                SIAResources.CreateMenuIconFromShortcutIcon(shortcutImage);

			menuInfo = new MenuInfo("&Rank", Categories.Filters, Shortcut.None, 520, menuImage, SeparateStyle.Below);
			shortcutInfo = new ShortcutInfo("Rank", Categories.Filters, 520, shortcutImage);	
		}

		public CmdFiltersRank(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, menuInfo, null, shortcutInfo)
		{
		}

		public override void DoCommand(params object[] args)
		{
            IAppWorkspace appWorkspace = this.appWorkspace;
            ImageWorkspace workspace = this.Workspace;
            CommonImage image = workspace.Image;

            IProgressCallback callback = null;

            using (DlgRankFilter dlg = new DlgRankFilter(workspace))
            {
                if (dlg.ShowDialog(workspace) == DialogResult.OK)
                {
                    try
                    {
                        callback = workspace.BeginProcess("Filter Rank", ProgressType.AutoTick);

						using (CommandExecutor cmdExec = new CommandExecutor(workspace))
							cmdExec.DoCommandFilterRank(image, dlg.FilterType, dlg.Kernel, dlg.Pass);
					}
					catch (System.Exception exp)
					{
                        workspace.HandleGenericException("Failed to apply rank filter", exp, ref callback);
					}
					finally
					{
						if (callback != null)
						{
                            workspace.EndProcess(callback);
							callback = null;
						}						
					}
                }
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

