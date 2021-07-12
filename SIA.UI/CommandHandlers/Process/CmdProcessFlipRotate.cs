using System;
using System.Windows.Forms;

using SIA.Plugins;
using SIA.Plugins.Common;

using SIA.SystemFrameworks;
using SIA.SystemFrameworks.UI;

using SIA.SystemLayer;

using SIA.UI.Controls;
using SIA.UI.Controls.Commands;
using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.Automation;
using SIA.UI.Controls.Automation.Commands;
using System.Drawing;


namespace SIA.UI.CommandHandlers
{

	internal class CmdProcessFlipRotate : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdProcessFlipRotate";
		
		private static MenuInfo menuInfo = null;
		private static ShortcutInfo shortcutInfo = null;

		static CmdProcessFlipRotate()
		{
            System.Drawing.Image shortcutImage = 
                SIAResources.GetShortcutIcon("flipVertical");
            System.Drawing.Image menuImage = 
                SIAResources.CreateMenuIconFromShortcutIcon(shortcutImage);

			menuInfo = 
                new MenuInfo("Flip/Rotate Image", Categories.Process, Shortcut.None, 470, menuImage, SeparateStyle.Above);
			shortcutInfo = 
                new ShortcutInfo("Flip/Rotate Image", Categories.Process, 470, shortcutImage);
		}

		public CmdProcessFlipRotate(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, menuInfo, null, shortcutInfo)
		{
		}

		public override void DoCommand(params object[] args)
		{
			MainFrame appWorkspace = this.appWorkspace;
			ImageWorkspace workspace = this.Workspace;
			IProgressCallback callback = null;

			using (DlgFlipRotate dlg = new DlgFlipRotate())
			{
				if (DialogResult.OK == dlg.ShowDialog(appWorkspace))
				{
					try
					{
						CommonImage image = workspace.Image;
						FlipRotateImageCommandSettings settings = dlg.Settings;

						string message = "";
						FlipRotateImageCommandSettings.Actions type = (FlipRotateImageCommandSettings.Actions)settings.ActionType;
						if (type == FlipRotateImageCommandSettings.Actions.FlipHorizontal)
							message = "Flipping horizontal...";
						else if (type == FlipRotateImageCommandSettings.Actions.FlipVertical)
							message = "Flipping vertical...";
						else if (type == FlipRotateImageCommandSettings.Actions.Rotate90CW)
							message = "Rotating 90 Clockwise...";
						else if (type == FlipRotateImageCommandSettings.Actions.Rotate90CCW)
							message = "Rotating 90 Counter-Clockwise...";
						else if (type == FlipRotateImageCommandSettings.Actions.Rotate180)
							message = "Rotating 180 degree...";
						else if (type == FlipRotateImageCommandSettings.Actions.RotateByAngle)
							message = String.Format("Rotating {0} degree...", settings.RotateAngle);

						callback = workspace.BeginProcess(message, ProgressType.Normal);

						using (CommandExecutor cmdExec = new CommandExecutor(workspace))
							cmdExec.DoCommand(typeof(FlipRotateImageCommand), workspace.Image, settings);
					}
					catch (System.Exception exp)
					{
						workspace.HandleGenericException("Failed to flip or rotate image.", exp, ref callback);
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

