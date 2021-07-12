using System;
using System.Collections.Generic;
using System.Text;
using SIA.UI.Controls;
using SIA.SystemLayer;
using SIA.SystemFrameworks.UI;
using SIA.UI.Controls.Automation;
using SIA.UI.Controls.Utilities;
using SIA.UI.Controls.Helpers.VisualAnalysis;
using System.Collections;
using SIA.Plugins.Common;
using System.Drawing;
using System.Windows.Forms;
using SIA.SystemLayer.ImageProcessing;
using SIA.UI.Controls.Commands;
using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.Automation.Commands;
using System.IO;
using SIA.Algorithms.ReferenceFile;
using SiGlaz.Common;

namespace SIA.UI.CommandHandlers.Analysis
{
    internal class CmdAnalysisClassifyPrimitiveObjectUsingGoldenImage : AppWorkspaceCommand
    {
        public const string cmdCommandKey = "CmdAnalysisClassifyPrimitiveObjectUsingGoldenImage";
		
		private static MenuInfo menuInfo = null;
		private static ToolBarInfo toolbarInfo = null;
		private static ShortcutInfo shortcutInfo = null;

        public ObjectAnalyzer ObjectAnalyzer
        {
            get
            {
                if (this.Workspace == null)
                    return null;
                return this.Workspace.GetAnalyzer("ObjectAnalyzer") as ObjectAnalyzer;
            }
        }

        private static string description = "Primitive Objects";

		static CmdAnalysisClassifyPrimitiveObjectUsingGoldenImage()
		{
            // nn classify
            string menuImageResourceName = "DetectObjIcon";
            //string shortcutImageResourceName = "detobj";
            Image image = SIAResources.GetMenuIcon(menuImageResourceName);
            int index = 602;
            //menuInfo =
            //    new MenuInfo(
            //        string.Format("&Classify {0} using Golden Image", description),
            //        Categories.Analysis, Shortcut.None, index, image, SeparateStyle.None);
            //toolbarInfo =
            //    new ToolBarInfo(
            //        string.Format("Classify {0} using Golden Image", description),
            //        string.Format("Classify {0} using Golden Image", description), 
            //        601, image, SeparateStyle.Before);
            //shortcutInfo =
            //    new ShortcutInfo(
            //        string.Format("Classify {0} using Golden Image", description),
            //        Categories.Analysis, index, image);			

            menuInfo =
                new MenuInfo(
                    string.Format("Detect Anomalies"),
                    Categories.Analysis, Shortcut.None, index, image, SeparateStyle.None);
            toolbarInfo =
                new ToolBarInfo(
                    string.Format("Detect Anomalies"),
                    string.Format("Detect Anomalies"),
                    601, image, SeparateStyle.Before);
            shortcutInfo =
                new ShortcutInfo(
                    string.Format("Detect Anomalies"),
                    Categories.Analysis, index, image);
		}

        public CmdAnalysisClassifyPrimitiveObjectUsingGoldenImage(IAppWorkspace appWorkspace)
            : base(appWorkspace, cmdCommandKey, menuInfo, toolbarInfo, shortcutInfo)
		{
			
		}


        private string _goldenImageFilePath = "";
		public override void DoCommand(params object[] args)
        {
            DetectObjectUsingGoldenImageSettings settings = LoadDefault();
            if (settings == null)
                settings = new DetectObjectUsingGoldenImageSettings();
            if (_goldenImageFilePath == "")
                _goldenImageFilePath = settings.GoldenImageFilePath;

            using (DlgFileBrowser dlg = new DlgFileBrowser())
            {
                dlg.FilePath = _goldenImageFilePath;
                if (dlg.ShowDialog(this.Workspace) != DialogResult.OK)
                    return;

                _goldenImageFilePath = dlg.FilePath;
                settings.GoldenImageFilePath = dlg.FilePath;
                SaveDefault(settings);
            }

            ImageWorkspace workspace = this.Workspace;
            CommonImage image = workspace.Image;

            string goldenImageFilePath = "";
            eGoldenImageMethod method = eGoldenImageMethod.None;
            double darkThreshold = 92.5;

            double brightThreshold = 150;
           
#if DEBUG
            // for PoleTip only
            darkThreshold = 110;
            brightThreshold = 150;


            goldenImageFilePath = @"d:\WorkingSpace\Projects\Xyratex\ABSInspection\Recipes\Output\ABS10x.63.0_golden_image_median.bmp";
#endif

            goldenImageFilePath = _goldenImageFilePath;

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

            IProgressCallback callback = null;
            try
            {
                // begin process
                callback = workspace.BeginProcess(
                    string.Format("Detect {0}...", description), 
                    false, ProgressType.Normal);
                callback.Begin(0, 100);

                // start wait cursor
                kUtils.kBeginWaitCursor();

                workspace.DetectedObjects = new ArrayList();

                ObjectAnalyzer analyzer = this.ObjectAnalyzer;
                if (analyzer != null)
                {
                    analyzer.Visible = false;
                    analyzer.Deactivate();

                    if (analyzer.DetectedObjectsWindow != null)
                        analyzer.DetectedObjectsWindow.Visible = false;
                }

                ArrayList globalResult = new ArrayList();

                using (CommandExecutor cmdExec = new CommandExecutor(workspace))
                {
                    //// refer to CmdAnalysisDetectObjects.cs
                    //ArrayList internalResult = 
                    //    cmdExec.DoCommandDetectPrimitiveObjectUsingGoldenImage(
                    //    image, goldenImageFilePath, method, darkThreshold, brightThreshold);
                    //if (internalResult != null)
                    //{
                    //    globalResult.AddRange(internalResult);

                    //    internalResult.Clear();
                    //    internalResult = null;
                    //}
                }

                if (globalResult == null)
                    globalResult = new ArrayList(); /// ????
                {
                    if (globalResult != null)
                    {
                        workspace.DetectedObjects = globalResult;
                        if (analyzer != null)
                        {
                            analyzer.Visible = true;
                            analyzer.Activate();

                            if (analyzer.DetectedObjectsWindow != null)
                                analyzer.DetectedObjectsWindow.Visible = true;

                            workspace.Invalidate(true);
                        }
                    }
                }

                if (analyzer.CombineObjectAnalyzer != null)
                    analyzer.CombineObjectAnalyzer.ClearStorageWorkingObjects();
            }
            catch (System.Exception exp)
            {
                if (exp.InnerException != null)
                {
                    workspace.HandleGenericException(
                        string.Format(
                        "Failed to subtract golden image.\n{0}", 
                        exp.InnerException.Message), exp, ref callback);
                }
                else
                {
                    workspace.HandleGenericException(
                        "Failed to subtract golden image.", exp, ref callback);
                }

                //workspace.HandleGenericException(exp, ref callback);
            }
            finally
            {
                // stop process
                if (callback != null)
                {
                    workspace.EndProcess(callback);
                    callback = null;
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
            return base.QueryMenuItemStatus();
        }

        public override UIElementStatus QueryShortcutBarItemStatus()
        {
            return base.QueryShortcutBarItemStatus();
        }


        private DetectObjectUsingGoldenImageSettings LoadDefault()
        {
            try
            {
                string fileName = Path.Combine(
                    Application.UserAppDataPath, @"SiGlaz\SIA\DefaultSettings\ClassifyObjUsingGoldenImage.settings");
                PathHelper.CreateMissingFolderAuto(fileName);

                if (!File.Exists(fileName))
                    return new DetectObjectUsingGoldenImageSettings();

                DetectObjectUsingGoldenImageSettings
                    settings = DetectObjectUsingGoldenImageSettings.Deserialize(fileName);

                if (settings != null)
                    return settings;
            }
            catch
            {
                // nothting
            }

            return new DetectObjectUsingGoldenImageSettings();      
        }

        private void SaveDefault(DetectObjectUsingGoldenImageSettings settings)
        {
            try
            {
                string fileName = 
                    Path.Combine(
                    Application.UserAppDataPath, @"SiGlaz\SIA\DefaultSettings\ClassifyObjUsingGoldenImage.settings");
                PathHelper.CreateMissingFolderAuto(fileName);

                settings.Serialize(fileName);
            }
            catch
            {
            }
        }
    }
}
