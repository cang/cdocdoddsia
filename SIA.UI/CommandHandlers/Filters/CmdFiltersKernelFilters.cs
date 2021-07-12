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
	internal class CmdFiltersKernelFilters : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdFiltersKernelFilters";
		
		private static MenuInfo menuInfo = null;
		private static ShortcutInfo shortcutInfo = null;

		static CmdFiltersKernelFilters()
		{
            Image shortcutImage =
                SIAResources.GetShortcutIcon("fltkernel");
            Image menuImage =
                SIAResources.CreateMenuIconFromShortcutIcon(shortcutImage);

			menuInfo = new MenuInfo("&Kernel Filters...", Categories.Filters, Shortcut.None, 500, menuImage, SeparateStyle.Both);
			shortcutInfo = new ShortcutInfo("Kernel Filters", Categories.Filters, 500, shortcutImage);			
		}

		public CmdFiltersKernelFilters(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, menuInfo, null, shortcutInfo)
		{
		}

		public override void DoCommand(params object[] args)
		{
            IAppWorkspace appWorkspace = this.appWorkspace;
            ImageWorkspace workspace = this.Workspace;
            CommonImage image = workspace.Image;

            using (DlgKernelFilter dlg = new DlgKernelFilter(workspace))
            {
                if (dlg.ShowDialog(workspace) == DialogResult.OK)
                {
                    //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                    //sw.Start();

                    IProgressCallback callback = null;

                    try
                    {
                        callback = workspace.BeginProcess("Kernel Filter", ProgressType.AutoTick);

						using (CommandExecutor cmdExec = new CommandExecutor(workspace))
						{
							if (dlg.IsApplyConvolution)
							{
								if (dlg.Selected_ConvolutionType != eMaskType.kMASK_UNKNOWN)
									cmdExec.DoCommandConvolution(image, dlg.Selected_ConvolutionType, dlg.Selected_ConvolutionMatrixType, dlg.EnPass, dlg.Threshold);
								else
									cmdExec.DoCommandCustomConvolution(image, dlg.CustomMatrix, dlg.EnPass);								
							}
							else
							{
								cmdExec.DoCommandMorphology(image, dlg.Selected_MorphologyType, dlg.Selected_MorphologyMatrixType, dlg.MorPass);
							}
						}
					}
					catch (System.Exception exp)
					{
						workspace.HandleGenericException("Failed to apply kernel filter", exp, ref callback);
					}
					finally
					{
						if (callback != null)
						{
							workspace.EndProcess(callback);
							callback = null;
						}						
					}

                    //sw.Stop();
                    //MessageBox.Show(sw.Elapsed.ToString());
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

