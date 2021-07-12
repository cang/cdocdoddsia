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
    internal class CmdProcessCreateGoldenImage : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdProcessCreateGoldenImage";
		private static ShortcutInfo shortcutInfo = null;
		
		private static MenuInfo menuInfo = null;

		static CmdProcessCreateGoldenImage()
		{
            int index = 490;
            System.Drawing.Image shortcutImage = null;
            System.Drawing.Image menuImage = null;
                
			menuInfo = new MenuInfo(
                "Create Golden Image", Categories.Process,
                Shortcut.None, index, menuImage, SeparateStyle.Both);
            //shortcutInfo = new ShortcutInfo(
            //    "Create Golden Image", Categories.Process, index, shortcutImage);
		}

        public CmdProcessCreateGoldenImage(IAppWorkspace appWorkspace)
            : base(appWorkspace, cmdCommandKey, menuInfo, null, shortcutInfo)
		{
		}

        private string[] _imageFiles = null;
        private string _outputFile = "";
		public override void DoCommand(params object[] args)
		{
            string[] files = null;
            string outputPath = "";
            using (DlgGoldenImageCreation dlg = new DlgGoldenImageCreation())
            {
                dlg.SelectedFiles = _imageFiles;
                dlg.OutputFilePath = _outputFile;

                if (dlg.ShowDialog(Workspace) != DialogResult.OK)
                    return;

                files = dlg.SelectedFiles;
                outputPath = dlg.OutputFilePath;

                _imageFiles = files;
                _outputFile = outputPath;
            }


            IDocWorkspace workspace = this.Workspace;
            IProgressCallback callback = null;

            string[] sampleFiles = null;
#if DEBUG
            sampleFiles = new string[] {
                    @"d:\WorkingSpace\Projects\Xyratex\ABSInspection\Recipes\Output\AlignedImages\abs10x.63_1.bmp"
                    ,@"d:\WorkingSpace\Projects\Xyratex\ABSInspection\Recipes\Output\AlignedImages\abs10x.63_2.bmp"
                    ,@"d:\WorkingSpace\Projects\Xyratex\ABSInspection\Recipes\Output\AlignedImages\abs10x.63_3.bmp"
                    ,@"d:\WorkingSpace\Projects\Xyratex\ABSInspection\Recipes\Output\AlignedImages\abs10x.63_4.bmp"
                    ,@"d:\WorkingSpace\Projects\Xyratex\ABSInspection\Recipes\Output\AlignedImages\abs10x.63_5.bmp"
                    ,@"d:\WorkingSpace\Projects\Xyratex\ABSInspection\Recipes\Output\AlignedImages\abs10x.63_6.bmp"
                    ,@"d:\WorkingSpace\Projects\Xyratex\ABSInspection\Recipes\Output\AlignedImages\abs10x.63_7.bmp"
                    ,@"d:\WorkingSpace\Projects\Xyratex\ABSInspection\Recipes\Output\AlignedImages\abs10x.63_8.bmp"
                };
#endif

            sampleFiles = files;

            eGoldenImageMethod method = eGoldenImageMethod.Average;
            string outputFilePath = @"d:\WorkingSpace\Projects\Xyratex\ABSInspection\Recipes\Output\ABS10x.63.0_golden_image.bmp";

            method = eGoldenImageMethod.Median;
            outputFilePath = @"d:\WorkingSpace\Projects\Xyratex\ABSInspection\Recipes\Output\ABS10x.63.0_golden_image_median.bmp";
            outputFilePath = outputPath;

            string msg = "";
            if (!GoldenImageProcessor.ValidateImageSize(sampleFiles, ref msg))
            {
                MessageBox.Show(Workspace,
                    "Specified image(s) are not the same size!",
                    "Create Golden Image",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            try
            {
                
                //GreyDataImage goldenImage = null;

                //callback = workspace.BeginProcess("Create Golden Image", ProgressType.Normal);
                //using (CommandExecutor cmdExec = new CommandExecutor(workspace))
                //    goldenImage = cmdExec.DoCommandCreateGoldenImage(sampleFiles, method);

                //goldenImage.SaveImage(outputFilePath, SIA.Common.eImageFormat.Bmp);

                //goldenImage.Dispose();
            }
            catch (System.Exception exp)
            {
                if (exp.InnerException != null)
                {
                    workspace.HandleGenericException(
                        string.Format(
                        "Failed to create golden image.\n{0}", 
                        exp.InnerException.Message), exp, ref callback);
                }
                else
                {
                    workspace.HandleGenericException(
                        "Failed to create golden image.", exp, ref callback);
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
			return UIElementStatus.Enable;
		}

		public override UIElementStatus QueryShortcutBarItemStatus()
		{
			return UIElementStatus.Enable;
		}
    }
}
