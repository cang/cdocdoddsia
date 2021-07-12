using System;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

using SIA.Common;
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
	internal class CmdProcessGlobalBackgroundCorrection : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdProcessGlobalBackgroundCorrection";
		
		private static MenuInfo menuInfo = null;
		private static ShortcutInfo shortcutInfo = null;

		static CmdProcessGlobalBackgroundCorrection()
		{
            System.Drawing.Image shortcutImage =
                SIAResources.GetShortcutIcon("glbbkgcorr");
            System.Drawing.Image menuImage =
                SIAResources.CreateMenuIconFromShortcutIcon(shortcutImage);

			menuInfo = 
                new MenuInfo("&Global Background Correction", Categories.Process, Shortcut.None, 410, menuImage, SeparateStyle.Above);
            shortcutInfo = 
                new ShortcutInfo("Global Background Correction", Categories.Process, 410, shortcutImage);
		}

		public CmdProcessGlobalBackgroundCorrection(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, menuInfo, null, shortcutInfo)
		{
		}

		public override void DoCommand(params object[] args)
		{
            IAppWorkspace appWorkspace = this.appWorkspace;
            ImageWorkspace workspace = this.Workspace;
            CommonImage image = workspace.Image;

            using (DlgGlobalBackgroundCorrection dlg = new DlgGlobalBackgroundCorrection(image))
            {
                if (dlg.ShowDialog(workspace) == DialogResult.OK)
                {
                    IProgressCallback callback = null;
                    try
                    {
                        callback = workspace.BeginProcess("Global background correction", ProgressType.AutoTick);
						using (CommandExecutor cmdExec = new CommandExecutor(workspace))
						{
							switch (dlg.Type)
							{
								case eGlobalBackgroundCorrectionType.ErosionFilter:
									cmdExec.DoCommandGbcErosion(image, dlg.ErosionNumber);
									break;
								case eGlobalBackgroundCorrectionType.FastFourierTransform:
									cmdExec.DoCommandGbcFFT(image, dlg.Threshold, dlg.CutOff);
									break;
								case eGlobalBackgroundCorrectionType.ReferenceImages:
									cmdExec.DoCommandGbcRefImages(image, dlg.ImagePaths);
									break;
								case eGlobalBackgroundCorrectionType.UnsharpFilter:
									cmdExec.DoCommandGbcUnsharp(image, dlg.UnsharpParam);
									break;
							}
						}	
					}
					catch (System.Exception exp)
					{
						workspace.HandleGenericException("Failed to correct global background.", exp, ref callback);
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

