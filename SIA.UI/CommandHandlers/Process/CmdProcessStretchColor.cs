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
	internal class CmdProcessStretchColor : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdProcessStretchColor";
		
		private static MenuInfo menuInfo = null;		
		private static ShortcutInfo shortcutInfo = null;

		static CmdProcessStretchColor()
		{
            System.Drawing.Image shortcutImage =
                SIAResources.GetShortcutIcon("stretchcolor");
            System.Drawing.Image menuImage =
                SIAResources.CreateMenuIconFromShortcutIcon(shortcutImage);

			menuInfo = new MenuInfo("&Stretch Color", Categories.Process, Shortcut.None, 440, menuImage, SeparateStyle.None);
			shortcutInfo = new ShortcutInfo("Stretch Color", Categories.Process, 440, shortcutImage);
		}

		public CmdProcessStretchColor(IAppWorkspace appWorkspace)
            : base(appWorkspace, cmdCommandKey, menuInfo, null, shortcutInfo)
		{
		}

		public override void DoCommand(params object[] args)
		{
            IAppWorkspace appWorkspace = this.appWorkspace;
            ImageWorkspace workspace = this.Workspace;
            CommonImage image = workspace.Image;

            using (DlgStretchColor dlg = new DlgStretchColor(workspace))
            {
                IProgressCallback callback = null;

                if (dlg.ShowDialog(workspace) == DialogResult.OK)
                {
                    try
                    {
                        callback = workspace.BeginProcess("Stretch Color", ProgressType.AutoTick);
						using (CommandExecutor cmdExec = new CommandExecutor(workspace))
							cmdExec.DoCommandStretchColor(image, dlg.Min, dlg.Max);
					}
					catch (System.Exception exp)
					{
                        workspace.HandleGenericException("Failed to stretch color", exp, ref callback);
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

