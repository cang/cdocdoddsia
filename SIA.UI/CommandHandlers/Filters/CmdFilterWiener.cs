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
using System.IO;
using SIA.UI.Controls.Automation.Commands;
using SiGlaz.Common;

namespace SIA.UI.CommandHandlers
{
    internal class CmdFilterWiener : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdFiltersWiener";
		
		private static MenuInfo menuInfo = null;
		private static ShortcutInfo shortcutInfo = null;

		static CmdFilterWiener()
		{
            Image shortcutImage = 
                SIAResources.GetShortcutIcon("fltgaussian");
            Image menuImage = 
                SIAResources.CreateMenuIconFromShortcutIcon(shortcutImage);

			menuInfo = 
                new MenuInfo("&Adaptive", Categories.Filters, 
                    Shortcut.None, 550, menuImage, SeparateStyle.None);

			shortcutInfo =
                new ShortcutInfo("Adaptive", Categories.Filters, 550, shortcutImage);	
		}

        public CmdFilterWiener(IAppWorkspace appWorkspace)
            : base(appWorkspace, cmdCommandKey, menuInfo, null, shortcutInfo)
		{
		}

		public override void DoCommand(params object[] args)
		{
            IAppWorkspace appWorkspace = this.appWorkspace;
            ImageWorkspace workspace = this.Workspace;
            CommonImage image = workspace.Image;

            IProgressCallback callback = null;

            int kernelWidth = 9;
            int kernelHeight = 9;

            try
            {
                int kernelSize = 9;
                bool isAuto = true;
                double noiseLevel = 0.01;

                string settingFile = Path.Combine(
                    Application.UserAppDataPath, 
                    string.Format(
                    "SiGlaz\\SiGlaz Image Analyzer\\Settings\\AdaptiveFilter.settings"));
                PathHelper.CreateMissingFolderAuto(settingFile);

                DlgWienerFilter.LoadSettings(
                    settingFile, out kernelSize, out isAuto, out noiseLevel);

                DlgWienerFilter dlg = new DlgWienerFilter(kernelSize, isAuto, noiseLevel);
                if (dlg.ShowDialog(appWorkspace) == DialogResult.OK)
                {
                    kernelWidth = dlg.KernelSize;
                    kernelHeight = kernelWidth;
                    isAuto = dlg.IsAuto;
                    noiseLevel = dlg.NoiseLevel;

                    DlgWienerFilter.SaveSettings(
                        settingFile, kernelSize, isAuto, noiseLevel);
                    
                    callback = workspace.BeginProcess("Adptive Filter", ProgressType.AutoTick);

                    CommonImage workingImage = new CommonImage(image.RasterImage, true);

                    using (CommandExecutor cmdExec = new CommandExecutor(workspace))
                        cmdExec.DoCommandFilterWiener(workingImage, kernelWidth, kernelHeight, isAuto, noiseLevel);

                    workingImage.FilePath = image.FilePath;
                    workspace.Image = workingImage;
                    if (image != null)
                    {
                        image.Dispose();
                        image = null;
                    }
                }
            }
            catch (System.Exception exp)
            {
                workspace.HandleGenericException("Failed to apply Adaptive filter", exp, ref callback);
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
