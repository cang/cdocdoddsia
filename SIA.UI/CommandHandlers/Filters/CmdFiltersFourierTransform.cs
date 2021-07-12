using System;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

using SIA.Common;
using SIA.Common.Imaging;
using SIA.Common.Imaging.Filters;

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
	internal class CmdFiltersFourierTransform : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdFiltersFourierTransform";
		
		private static MenuInfo menuInfo = null;
		private static ShortcutInfo shortcutInfo = null;

		static CmdFiltersFourierTransform()
		{
            Image shortcutImage =
                SIAResources.GetShortcutIcon("fltfourier");
            Image menuImage =
                SIAResources.CreateMenuIconFromShortcutIcon(shortcutImage);

			menuInfo = new MenuInfo("Fo&urier Transform", Categories.Filters, Shortcut.None, 510, menuImage, SeparateStyle.Above);
			shortcutInfo = new ShortcutInfo("Fourier Transform", Categories.Filters, 510, shortcutImage);			
		}

		public CmdFiltersFourierTransform(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, menuInfo, null, shortcutInfo)
		{
		}

		public override void DoCommand(params object[] args)
		{
            IAppWorkspace appWorkspace = this.appWorkspace;
            ImageWorkspace workspace = this.Workspace;
            CommonImage image = workspace.Image;

            using (DlgFFTFilter dlg = new DlgFFTFilter(workspace))
            {
                if (dlg.ShowDialog(workspace) == DialogResult.OK)
                {
                    IProgressCallback callback = null;

                    try
                    {
                        callback = workspace.BeginProcess(String.Format("Fourier Transform {0} Filter", dlg.FilterType == FFTFilterType.HighPass ? "High Pass" : "Low Pass"), ProgressType.AutoTick);
						using (CommandExecutor cmdExec = new CommandExecutor(workspace))
							cmdExec.DoCommandFilterFFT(image, dlg.FilterType, dlg.CutOff, dlg.Weight);
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

                        // force Garbage Collector to collect defects object
                        GC.Collect();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
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

