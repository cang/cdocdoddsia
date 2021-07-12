using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SIA.UI.Controls;
using SIA.Plugins.Common;
using System.Windows.Forms;
using SIA.Algorithms.ReferenceFile;
using SIA.SystemFrameworks.UI;
using SIA.UI.Controls.Helpers.VisualAnalysis;
using SIA.UI.Controls.Utilities;
using SIA.UI.Controls.Commands;
using SIA.IPEngine;

namespace SIA.UI.CommandHandlers.Tools
{
    internal class CmdToolsCreateReferenceImage : AppWorkspaceCommand
    {
        public const string cmdCommandKey = "CmdToolsCreateReferenceImage";
		
		private static MenuInfo menuInfo = null;
        private static ToolBarInfo toolbarInfo = null;
        private static ShortcutInfo shortcutInfo = null;

        private static string cmdDescription = "Create ABS Reference Image";
        private static string cmdImageName = "align_ABS";
        private static int cmdIndex = 745;

		static CmdToolsCreateReferenceImage()
		{
            Image image = SIAResources.GetMenuIcon(cmdImageName);
            menuInfo =
                new MenuInfo(
                    cmdDescription, 
                    Categories.Tools, 
                    Shortcut.None, 
                    cmdIndex, 
                    image, 
                    SeparateStyle.Above);

            image = SIAResources.GetShortcutIcon(cmdImageName);

            shortcutInfo =
                new ShortcutInfo(
                    cmdDescription, 
                    Categories.Tools, 
                    cmdIndex, 
                    image);
		}

        public CmdToolsCreateReferenceImage(IAppWorkspace appWorkspace)
            : base(appWorkspace, cmdCommandKey, menuInfo, toolbarInfo, shortcutInfo)
		{
		}

		public override void DoCommand(params object[] args)
		{
            ImageWorkspace workspace = this.Workspace;

            SIA.UI.Controls.Dialogs.DlgCreateReferenceImg dlg = new SIA.UI.Controls.Dialogs.DlgCreateReferenceImg();

            string[] sampleFiles = null;
            if (dlg.ShowDialog() != DialogResult.OK)
                return;
            // get sample files here
            sampleFiles = dlg.FileList;

            eGoldenImageMethod method = eGoldenImageMethod.Median;

            string outRefImageFile = dlg.OutputFilePath;

            IProgressCallback callback = null;
            GreyDataImage refImage = null;
            try
            {
                if (workspace != null)
                {
                    MetrologyAnalyzer metrologyAnalyzer =
                        workspace.GetAnalyzer("MetrologyAnalyzer") as MetrologyAnalyzer;
                    if (metrologyAnalyzer != null)
                    {
                        metrologyAnalyzer.Visible = false;
                    }
                }

                // begin process
                callback = workspace.BeginProcess("Creating Reference Image...", false, ProgressType.Normal);
                callback.Begin(0, 100);

                // start wait cursor
                kUtils.kBeginWaitCursor();

                
                using (CommandExecutor cmdExec = new CommandExecutor(workspace))
                {
                    object result = cmdExec.DoCommandCreateReferenceImage(sampleFiles, method);
                    if (result != null)
                        refImage = result as GreyDataImage;
                }

                if (refImage != null)
                {
                    refImage.SaveImage(outRefImageFile, SIA.Common.eImageFormat.Bmp);                    
                }
                else
                {
                }                
            }
            catch (System.Exception exp)
            {
                workspace.HandleGenericException(exp, ref callback);
            }
            finally
            {   

                // stop process
                if (callback != null)
                {
                    workspace.EndProcess(callback);
                    callback = null;
                }

                if (refImage != null)
                {
                    refImage.Dispose();
                    refImage = null;
                }

                // force Garbage Collector to collect defects object
                GC.Collect();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                kUtils.kEndWaitCursor();
            }
		}

        public override UIElementStatus QueryMenuItemStatus()
        {
            return UIElementStatus.Enable;
        }

        public override UIElementStatus  QueryShortcutBarItemStatus()
        {
            return UIElementStatus.Enable;
        }
    }
}
