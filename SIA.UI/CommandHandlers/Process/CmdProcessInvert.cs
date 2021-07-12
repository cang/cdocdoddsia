using System;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

using SIA.SystemFrameworks;
using SIA.SystemFrameworks.UI;

using SIA.SystemLayer;

using SIA.Plugins;
using SIA.Plugins.Common;

using SIA.UI.Controls;
using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.Commands;
using SIA.UI.Controls.Automation;
using SIA.UI.Controls.Automation.Commands;

namespace SIA.UI.CommandHandlers
{

    internal class CmdProcessInvert : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdProcessInvert";
		
		private static MenuInfo menuInfo = null;
		private static ShortcutInfo shortcutInfo = null;

		static CmdProcessInvert()
		{
            System.Drawing.Image shortcutImage =
                SIAResources.GetShortcutIcon("invert");
            System.Drawing.Image menuImage =
                SIAResources.CreateMenuIconFromShortcutIcon(shortcutImage);

			menuInfo = new MenuInfo("&Invert", Categories.Process, Shortcut.None, 450, menuImage, SeparateStyle.Above);
			shortcutInfo = new ShortcutInfo("Invert", Categories.Process, 450, shortcutImage);
		}

		public CmdProcessInvert(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, menuInfo, null, shortcutInfo)
		{
		}

		public override void DoCommand(params object[] args)
		{
            IAppWorkspace appWorkspace = this.appWorkspace;
            ImageWorkspace workspace = this.Workspace;
            CommonImage image = workspace.Image;
            IProgressCallback callback = null;

            try
            {
                callback = workspace.BeginProcess("Inversion Arithmetic", ProgressType.AutoTick);
				using (CommandExecutor cmdExec = new CommandExecutor(workspace))
					cmdExec.DoCommandInvert(image);
			}
			catch (System.Exception exp)
			{
				workspace.HandleGenericException("Failed to invert image", exp, ref callback);
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

