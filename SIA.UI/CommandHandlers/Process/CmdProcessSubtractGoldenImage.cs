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
using SIA.SystemLayer.ImageProcessing;
using SIA.IPEngine;
using SIA.Algorithms.ReferenceFile;

namespace SIA.UI.CommandHandlers.Process
{
    internal class CmdProcessSubtractGoldenImage  : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdProcessSubtractGoldenImage";
		private static ShortcutInfo shortcutInfo = null;
		
		private static MenuInfo menuInfo = null;

		static CmdProcessSubtractGoldenImage()
		{
            int index = 491;
            System.Drawing.Image shortcutImage = null;
            System.Drawing.Image menuImage = null;
                
			menuInfo = new MenuInfo(
                "Subtract Golden Image", Categories.Process,
                Shortcut.None, index, menuImage, SeparateStyle.Both);
            //shortcutInfo = new ShortcutInfo(
            //    "Subtract Golden Image", Categories.Process, index, shortcutImage);
		}

        public CmdProcessSubtractGoldenImage(IAppWorkspace appWorkspace)
            : base(appWorkspace, cmdCommandKey, menuInfo, null, shortcutInfo)
		{
		}


        private string _goldenImageFilePath = "";
		public override void DoCommand(params object[] args)
		{
            using (DlgFileBrowser dlg = new DlgFileBrowser())
            {
                dlg.FilePath = _goldenImageFilePath;
                if (dlg.ShowDialog(this.Workspace) != DialogResult.OK)
                    return;
                _goldenImageFilePath = dlg.FilePath;
            }

            IDocWorkspace workspace = this.Workspace;
            IProgressCallback callback = null;
            CommonImage image = workspace.Image;

            string goldenImageFilePath = null;
#if DEBUG
            goldenImageFilePath = @"d:\WorkingSpace\Projects\Xyratex\ABSInspection\Recipes\Output\ABS10x.63.0_golden_image_median.bmp";
#endif
            goldenImageFilePath = _goldenImageFilePath;
            eGoldenImageMethod method = eGoldenImageMethod.Average;
            method = eGoldenImageMethod.None;

            string msg = "";
            if (!GoldenImageProcessor.ValidateImageSize(
                goldenImageFilePath, image.Width, image.Height, ref msg))
            {
                MessageBox.Show(Workspace,
                    "Specified golden image is not the same size with the processing image!",
                    "Subtract Golden Image",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }


            try
            {                
                callback = workspace.BeginProcess("Subtract Golden Image", ProgressType.Normal);
                using (CommandExecutor cmdExec = new CommandExecutor(workspace))
                    cmdExec.DoCommandSubtractGoldenImage(image, goldenImageFilePath, method);
            }
            catch (System.Exception exp)
            {
                if (exp.InnerException != null)
                {
                    workspace.HandleGenericException(string.Format("Failed to subtract golden image.\n{0}", exp.InnerException.Message), exp, ref callback);
                }
                else
                {
                    workspace.HandleGenericException("Failed to subtract golden image.", exp, ref callback);
                }
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
