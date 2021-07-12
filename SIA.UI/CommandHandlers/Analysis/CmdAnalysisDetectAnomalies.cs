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
using SiGlaz.Common.ImageAlignment;

namespace SIA.UI.CommandHandlers.Analysis
{
    internal class CmdAnalysisDetectAnomalies : AppWorkspaceCommand
    {
        public const string cmdCommandKey = "CmdAnalysisDetectAnomalies";
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

        private static string description = "Anomalies";

		static CmdAnalysisDetectAnomalies()
		{
            // nn classify
            string menuImageResourceName = "DetectObjIcon";
            //string shortcutImageResourceName = "detobj";
            Image image = SIAResources.GetMenuIcon(menuImageResourceName);
            int index = 602;            
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

        public CmdAnalysisDetectAnomalies(IAppWorkspace appWorkspace)
            : base(appWorkspace, cmdCommandKey, menuInfo, toolbarInfo, shortcutInfo)
		{
			
		}

        public override void DoCommand(params object[] args)
        {
            //string refFilePath = "";
            //using (OpenFileDialog dlg = new OpenFileDialog())
            //{
            //    dlg.Title = "Please specify Reference File";
            //    dlg.Filter = MetrologySystemReference.FileFilter;
            //    dlg.RestoreDirectory = true;

            //    if (dlg.ShowDialog(this.Workspace.AppWorkspace) == DialogResult.OK)
            //    {
            //        refFilePath = dlg.FileName;
            //    }
            //    else return;
            //}

            bool applyRegion = false;

            if (Workspace.RefFile.Regions != null)
            {
                DetectAnomaliesSettings settings = new DetectAnomaliesSettings();
                using (DlgDetectAnomaliesSettings dlg =
                    new DlgDetectAnomaliesSettings(settings))
                {
                    if (dlg.ShowDialog(this.Workspace.AppWorkspace) == System.Windows.Forms.DialogResult.OK)
                    {
                        settings = dlg.Settings;
                    }
                    else return;
                }

                applyRegion = settings.ApplyRegions;
            }

            eGoldenImageMethod method = AnomalyDetectorDefinition.Method;
            double darkThreshold = AnomalyDetectorDefinition.ABSDarkThreshold;
            double brightThreshold = AnomalyDetectorDefinition.ABSBrightThreshold;

            ImageWorkspace workspace = this.Workspace;
            CommonImage image = workspace.Image;
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
                    // refer to CmdAnalysisDetectObjects.cs
                    ArrayList internalResult =
                        cmdExec.DoCommandDetectAnomalies(
                        image, workspace.RefFile, method, 
                        darkThreshold, brightThreshold, 
                        workspace.AlignmentResult, 
                        (applyRegion ?  workspace.DetectedRegions : null));

                    if (internalResult != null)
                    {
                        globalResult.AddRange(internalResult);

                        internalResult.Clear();
                        internalResult = null;
                    }
                }

                if (globalResult == null)
                    globalResult = new ArrayList(); /// ????
                {
                    if (globalResult != null)
                    {
                        // update metrology info
                        ImageAnalyzer imageAnalyzer =
                            workspace.GetAnalyzer("ImageAnalyzer") as ImageAnalyzer;
                        if (imageAnalyzer != null && imageAnalyzer.MetrologySystem != null)
                        {
                            imageAnalyzer.MetrologySystem.CurrentUnit.UpdateObjectInfo(globalResult);
                        }


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

        public UIElementStatus GetUIStatus()
        {
            ImageWorkspace workspace = this.Workspace;
            if (workspace == null) return UIElementStatus.Disable;
            if (workspace.Image == null) return UIElementStatus.Disable;

            ImageAnalyzer analyzer = workspace.GetAnalyzer("ImageAnalyzer") as ImageAnalyzer;
            if (analyzer == null) return UIElementStatus.Disable;
            return (analyzer.DrawCoordinateSystem ? UIElementStatus.Enable : UIElementStatus.Disable);
        }

        public override UIElementStatus QueryMenuItemStatus()
        {
            return GetUIStatus();
        }

        public override UIElementStatus QueryToolBarItemStatus()
        {
            return GetUIStatus();
        }

        public override UIElementStatus QueryShortcutBarItemStatus()
        {
            return GetUIStatus();
        }
    }
}
