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
	internal class CmdProcessThreshold : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdProcessThreshold";
		
		private static MenuInfo menuInfo = null;
		
		private static ShortcutInfo shortcutInfo = null;

		static CmdProcessThreshold()
		{
            System.Drawing.Image shortcutImage =
                SIAResources.GetShortcutIcon("threshold");
            System.Drawing.Image menuImage =
                SIAResources.CreateMenuIconFromShortcutIcon(shortcutImage);

			menuInfo = new MenuInfo("&Threshold", Categories.Process, Shortcut.None, 430, menuImage, SeparateStyle.Above);
			shortcutInfo = new ShortcutInfo("Threshold", Categories.Process, 430, shortcutImage);
		}

		public CmdProcessThreshold(IAppWorkspace appWorkspace)
            : base(appWorkspace, cmdCommandKey, menuInfo, null, shortcutInfo)
		{
           
		}

		public override void DoCommand(params object[] args)
		{
            IAppWorkspace appWorkspace = this.appWorkspace;
            ImageWorkspace workspace = this.Workspace;
            CommonImage image = workspace.Image;

            // load settings
            ThresholdCommandSettings settings = this.LoadThresholdSettings();
            if (settings == null)
                settings = new ThresholdCommandSettings();

            using (DlgThreshold dlg = new DlgThreshold(workspace, settings))
            {
                IProgressCallback callback = null;

                if (DialogResult.OK == dlg.ShowDialog(workspace))
                {
                    try
                    {
                        // save settings
                        this.SaveThresholdSettings(settings);

                        callback = workspace.BeginProcess("Threshold", ProgressType.AutoTick);
                        using (CommandExecutor cmdExec = new CommandExecutor(workspace))
                        {
                            cmdExec.DoCommandThreshold(image, settings);
                        }
                    }
                    catch (System.Exception exp)
                    {
                        workspace.HandleGenericException("Failed to threshold image", exp, ref callback);
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

        private ThresholdCommandSettings LoadThresholdSettings()
        {
            try
            {
                String filename = AppSettings.MySettingsFolder + "\\ThresholdSettings.settings";
                if (File.Exists(filename) == false)
                    return null;

                return (ThresholdCommandSettings)RasterCommandSettingsSerializer.Deserialize(filename, typeof(ThresholdCommandSettings));
            }
            catch (System.Exception exp)
            {
                Trace.WriteLine(exp);
            }

            return null;
        }

        private void SaveThresholdSettings(ThresholdCommandSettings settings)
        {
            try
            {
                String filename = AppSettings.MySettingsFolder + "\\ThresholdSettings.settings";
                RasterCommandSettingsSerializer.Serialize(filename, settings);
            }
            catch (System.Exception exp)
            {
                Trace.WriteLine(exp);
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

