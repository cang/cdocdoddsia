using System;
using System.IO;
using System.Collections;
using System.Windows.Forms;

using SIA.SystemFrameworks;
using SIA.SystemFrameworks.UI;

using SIA.IPEngine;
using SIA.SystemLayer;

using SIA.Plugins;
using SIA.Plugins.Common;

using SIA.UI.Components;
using SIA.UI.Components.Common;

using SIA.UI.Controls;
using SIA.UI.Controls.Commands;
using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.UserControls;

namespace SIA.UI.CommandHandlers
{
	internal class CmdViewIntensityHistogram : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdViewIntensityHistogram";
		
		private static MenuInfo menuInfo = null;
		private static ShortcutInfo shortcutInfo = null;

		static CmdViewIntensityHistogram()
		{
            System.Drawing.Image shortcutImage =
                SIAResources.GetShortcutIcon("intensity");
            System.Drawing.Image menuImage =
                SIAResources.CreateMenuIconFromShortcutIcon(shortcutImage);

			menuInfo = 
                new MenuInfo(
                    "&Intensity Histogram", Categories.View, Shortcut.None, 340, menuImage, SeparateStyle.None);
			shortcutInfo = 
                new ShortcutInfo(
                    "Intensity Histogram", Categories.View, 340, shortcutImage);			
		}

		public CmdViewIntensityHistogram(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, menuInfo, null, shortcutInfo)
		{
		}

		public override void DoCommand(params object[] args)
		{
            IAppWorkspace appWorkspace = this.appWorkspace;
            ImageWorkspace workspace = this.Workspace as ImageWorkspace;
            CommonImage image = workspace.Image;
            IProgressCallback callback = null;
            kHistogram histogram = null;
                
            try
            {
                // begin process
                callback = workspace.BeginProcess("Calculating intensity histogram ...", false, ProgressType.Normal);
                callback.Begin(0, 100);

                // calculate histogram data
                using (CommandExecutor cmdExec = new CommandExecutor(workspace))
                    histogram = cmdExec.DoCommandComputeIntensityHistogram(image);

                if (histogram != null)
                {
                    ImageViewer imageViewer = workspace.ImageViewer;
                    PseudoColor pseudoColor = imageViewer.PseudoColor;

                    using (DlgIntensityHistogramEx dlg = new DlgIntensityHistogramEx(pseudoColor, image, histogram))
                        dlg.ShowDialog(workspace);

                    //using (DlgIntensityHistogram dlg = new DlgIntensityHistogram(image))
                    //{
                    //    dlg.ShowDialog(workspace);
                    //}
                }
            }
            catch (Exception exp)
            {
                workspace.HandleGenericException(exp, ref callback);
                
                MessageBoxEx.Error("Failed to calculate intensity histogram: " + exp.Message);
            }
            finally
            {
                if (histogram != null)
                    histogram.Dispose();
                histogram = null;

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

