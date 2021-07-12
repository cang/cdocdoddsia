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
	internal class CmdProcessCameraCorrection : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdProcessCameraCorrection";
		
		private static MenuInfo menuInfo = null;

		static CmdProcessCameraCorrection()
		{
			menuInfo = new MenuInfo("C&amera Correction", Categories.Process, Shortcut.None, 490, null, SeparateStyle.None);
		}

		public CmdProcessCameraCorrection(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, menuInfo, null, null)
		{
			
		}

		public override void DoCommand(params object[] args)
		{
            IAppWorkspace appWorkspace = this.appWorkspace;
            ImageWorkspace workspace = this.Workspace;
            CommonImage image = workspace.Image;
            IProgressCallback callback = null;

            using (DlgCameraCorrection dlg = new DlgCameraCorrection(image))
            {
                if (DialogResult.OK == dlg.ShowDialog(workspace))
                {
                    try
                    {
                        callback = workspace.BeginProcess("Camera correction", true, ProgressType.Normal);
						using (CommandExecutor cmdExec = new CommandExecutor(workspace))
						{
							cmdExec.DoCommandCameraCorrection(image, dlg.FocalLength, dlg.PrincipalPoint, dlg.DistanceCoeffs, dlg.Interpolation);
						}
					}
					catch (System.ApplicationException exp)
					{
						workspace.HandleGenericException(exp.InnerException, ref callback);
					}
					catch (System.Exception exp)
					{
                        workspace.HandleGenericException(exp, ref callback);
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

