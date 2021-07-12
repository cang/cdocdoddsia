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
	internal class CmdProcessExtractGlobalBackground : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdProcessExtractGlobalBackground";
		
		private static MenuInfo menuInfo = null;
		private static ShortcutInfo shortcutInfo = null;

		static CmdProcessExtractGlobalBackground()
		{
            Image shortcutImage = SIAResources.GetShortcutIcon("extglbbkg");
            Image menuImage = SIAResources.CreateMenuIconFromShortcutIcon(shortcutImage);

			menuInfo = new MenuInfo(
                "&Extract Global Background", Categories.Process, Shortcut.None, 420, menuImage, SeparateStyle.Below);
			shortcutInfo = 
                new ShortcutInfo("Extract Background Correction", Categories.Process, 420, shortcutImage);
		}

		public CmdProcessExtractGlobalBackground(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, menuInfo, null, shortcutInfo)
		{
		}

		public override void DoCommand(params object[] args)
		{
            IAppWorkspace appWorkspace = this.appWorkspace;
            ImageWorkspace workspace = this.Workspace;
            CommonImage image = workspace.Image;

            using (DlgExtractGlobalBackground dlg = new DlgExtractGlobalBackground(image))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    IProgressCallback callback = null;
                    try
                    {
                        callback = workspace.BeginProcess("Extract Global Background", ProgressType.AutoTick);

						using (CommandExecutor cmdExec = new CommandExecutor(workspace))
						{
							switch(dlg.GetCurrentChecked()) 
							{
								case 0:
									cmdExec.DoCommandExtBckgndFFT(image, (int)dlg.Threshold, dlg.Cutoff);
									break;
								case 1:
									cmdExec.DoCommandExtBckgndErosion(image, dlg.ErosionFilters);
									break;
							}			
						}
					}
					catch (System.Exception exp)
					{
                        workspace.HandleGenericException("Failed to extract global background!", exp, ref callback);
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

