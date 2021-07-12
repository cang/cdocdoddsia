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
	internal class CmdFiltersGaussian : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdFiltersGaussian";
		
		private static MenuInfo menuInfo = null;
		private static ShortcutInfo shortcutInfo = null;

		static CmdFiltersGaussian()
		{
            Image shortcutImage =
                SIAResources.GetShortcutIcon("fltgaussian");
            Image menuImage =
                SIAResources.CreateMenuIconFromShortcutIcon(shortcutImage);

			menuInfo = new MenuInfo("&Gaussian", Categories.Filters, Shortcut.None, 540, menuImage, SeparateStyle.None);
			shortcutInfo = new ShortcutInfo("Gaussian", Categories.Filters, 540, shortcutImage);			
		}

		public CmdFiltersGaussian(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, menuInfo, null, shortcutInfo)
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
                callback = workspace.BeginProcess("Gaussian smooth filter", ProgressType.AutoTick);
				
                using (CommandExecutor cmdExec = new CommandExecutor(workspace))
					cmdExec.DoCommandFilterGaussian(image);
			}
			catch (System.Exception exp)
			{
                workspace.HandleGenericException("Failed to apply Gaussian filter", exp, ref callback);
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

