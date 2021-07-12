using System;
using System.IO;
using System.Collections;
using System.Windows.Forms;

using SIA.SystemFrameworks;
using SIA.SystemFrameworks.UI;

using SIA.SystemLayer;

using SIA.Plugins;
using SIA.Plugins.Common;

using SIA.UI.Controls;
using SIA.UI.Controls.Commands;
using SIA.UI.Controls.Dialogs;


namespace SIA.UI.CommandHandlers
{
	internal class CmdProcessResize : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdProcessResize";
		private static ShortcutInfo shortcutInfo = null;
		
		private static MenuInfo menuInfo = null;

		static CmdProcessResize()
		{
            System.Drawing.Image shortcutImage =
                SIAResources.GetShortcutIcon("resizeImage");
            System.Drawing.Image menuImage =
                SIAResources.CreateMenuIconFromShortcutIcon(shortcutImage);

			menuInfo = new MenuInfo("Resi&ze Image", Categories.Process, Shortcut.None, 480, menuImage, SeparateStyle.Both);
			shortcutInfo = new ShortcutInfo("Resize Image", Categories.Process, 480, shortcutImage);
		}

		public CmdProcessResize(IAppWorkspace appWorkspace)
            : base(appWorkspace, cmdCommandKey, menuInfo, null, shortcutInfo)
		{
		}

		public override void DoCommand(params object[] args)
		{
            IAppWorkspace appWorkspace = this.appWorkspace;
            IDocWorkspace workspace = this.Workspace;
            CommonImage image = workspace.Image;

            using (DlgResize dlg = new DlgResize(image))
            {
                if (dlg.ShowDialog(workspace) == DialogResult.OK)
                {
                    IProgressCallback callback = null;

                    try
                    {
                        callback = workspace.BeginProcess("Resize Image", ProgressType.Normal);
                        using (CommandExecutor cmdExec = new CommandExecutor(workspace))
                            cmdExec.DoCommandResizeImage(image, dlg.Settings);
                    }
                    catch (System.Exception exp)
                    {
                        workspace.HandleGenericException("Failed to resize image.", exp, ref callback);
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

