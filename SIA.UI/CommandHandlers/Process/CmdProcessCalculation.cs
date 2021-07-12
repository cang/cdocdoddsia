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
using System.Drawing;

namespace SIA.UI.CommandHandlers
{
    /// <summary>
    /// Command entry for the calculation function
    /// </summary>
	internal class CmdProcessCalculation 
        : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdProcessCalculation";
		
		private static MenuInfo menuInfo = null;
		private static ShortcutInfo shortcutInfo = null;

		static CmdProcessCalculation()
		{
            Image shortcutImage = 
                SIAResources.GetShortcutIcon("calc");
            Image menuImage = 
                SIAResources.CreateMenuIconFromShortcutIcon(shortcutImage);

            menuInfo = new MenuInfo("&Calculation...", Categories.Process, Shortcut.None, 400, menuImage, SeparateStyle.Above);
            shortcutInfo = new ShortcutInfo("Calculation", Categories.Process, 400, shortcutImage);			
		}

		public CmdProcessCalculation(IAppWorkspace appWorkspace)
            : base(appWorkspace, cmdCommandKey, menuInfo, null, shortcutInfo)
		{
			
		}

		public override void DoCommand(params object[] args)
		{
            IAppWorkspace appWorkspace = this.appWorkspace;
            ImageWorkspace workspace = this.Workspace;
            CommonImage image = workspace.Image;

            using (DlgCalculation dlg = new DlgCalculation(workspace))
            {
                IProgressCallback callback = null;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        callback = workspace.BeginProcess("Calculation", ProgressType.AutoTick);

                        using (CommandExecutor executor = new CommandExecutor(workspace))
                            executor.DoCommand(typeof(CalculationCommand), image, dlg.Settings);
					}
					catch (System.Exception exp)
					{
						workspace.HandleGenericException("Failed to apply calculation operation", exp, ref callback);
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

