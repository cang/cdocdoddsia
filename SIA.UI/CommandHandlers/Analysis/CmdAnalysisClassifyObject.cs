using System;
using System.Collections.Generic;
using System.Text;
using SIA.Plugins.Common;
using SIA.UI.Controls.Helpers.VisualAnalysis;
using System.Drawing;
using System.Windows.Forms;
using SIA.SystemLayer;
using SIA.UI.Controls;
using SIA.SystemFrameworks.UI;
using SIA.UI.Controls.Utilities;
using System.Collections;
using SIA.UI.Controls.Dialogs;
using SIA.Algorithms.FeatureProcessing.Helpers;
using SIA.UI.Controls.Commands;
using SiGlaz.Common;
using System.IO;
using SIA.UI.Controls.Automation.Commands;
using SIA.UI.Controls.Automation;
using SiGlaz.Common.ABSDefinitions;

namespace SIA.UI.CommandHandlers.Analysis
{
    internal class CmdAnalysisClassifyObject : AppWorkspaceCommand
	{
        public const string cmdCommandKey = "CmdAnalysisClassifyObject";
		
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

        private static string description = "Objects";

		static CmdAnalysisClassifyObject()
		{
            // nn classify
            Image image = SIAResources.GetMenuIcon("neural_network_classify");
            int index = 601;
            menuInfo =
                new MenuInfo(
                    string.Format("&Classify {0}", description),
                    Categories.Analysis, Shortcut.None, index, image, SeparateStyle.None);
            toolbarInfo =
                new ToolBarInfo(
                    string.Format("Classify {0}", description),
                    string.Format("Classify {0} using Neural Network", description), 
                    601, image, SeparateStyle.Before);
            shortcutInfo =
                new ShortcutInfo(
                    string.Format("Classify {0}", description),
                    Categories.Analysis, index, SIAResources.GetShortcutIcon("neural_network_classify"));			
		}

        public CmdAnalysisClassifyObject(IAppWorkspace appWorkspace)
            : base(appWorkspace, cmdCommandKey, menuInfo, toolbarInfo, shortcutInfo)
		{
			
		}

		public override void DoCommand(params object[] args)
        {
            string filePath = ObjectClassificationSettings.SIADefaultFilePath;
            ObjectClassificationSettings classificationSettings =
                ObjectClassificationSettings.Deserialize(filePath);
            if (classificationSettings == null)
                classificationSettings = new ObjectClassificationSettings();

            DlgObjectClassification dlg = new DlgObjectClassification(classificationSettings);
            if (dlg.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            classificationSettings = dlg.Settings;
            RasterCommandSettingsSerializer.Serialize(filePath, classificationSettings);

            string regionFile = "";
            string nnModelFile = "";

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
                    workspace.ActiveAnalyzer = analyzer;

                    if (analyzer.DetectedObjectsWindow != null)
                        analyzer.DetectedObjectsWindow.Visible = false;
                }

                ArrayList globalResult = new ArrayList();
                ArrayList internalResult = null;

                #region classify Dark objects
                if (classificationSettings.ClassifyDarkObject)
                {
                    internalResult = null;
                    using (CommandExecutor cmdExec = new CommandExecutor(workspace))
                    {
                        // refer to CmdAnalysisDetectObjects.cs
                        internalResult = cmdExec.DoCommandDetectContaminations(image, regionFile, nnModelFile);
                    }
                    if (internalResult != null)
                        globalResult.AddRange(internalResult);
                }
                #endregion classify Dark objects

                #region classify Bright objects
                if (classificationSettings.ClassifyBrightObject)
                {
                    internalResult = null;
                    using (CommandExecutor cmdExec = new CommandExecutor(workspace))
                    {
                        // refer to CmdAnalysisDetectObjects.cs
                        internalResult = cmdExec.DoCommandDetectScratchs(image);
                    }
                    if (internalResult != null)
                        globalResult.AddRange(internalResult);
                }
                #endregion classify Bright objects


                #region classify objects across boundary
                if (classificationSettings.ClassifyDarkObjectAcrossBoundary ||
                    classificationSettings.ClassifyBrightObjectAcrossBoundary)
                {
                    internalResult = null;
                    using (CommandExecutor cmdExec = new CommandExecutor(workspace))
                    {
                        //foreach (string filename in LinePatternLibrary.MultiplePatternFilenames)
                        {
                            //LinePatternLibrary settings = LinePatternLibrary.Deserialize(
                            //    Path.Combine(SettingsHelper.GetDefaultABSInspectionSettingsFolderPath(),
                            //    filename));

                            //if (settings == null)
                            //    continue;

                            LinePatternLibrary settings = null;

                            // refer to CmdAnalysisDetectObjects.cs
                            internalResult = cmdExec.DoCommandDetectOverPatterns(image, settings);
                        }
                    }

                    if (internalResult != null)
                    {
                        if (!classificationSettings.ClassifyDarkObjectAcrossBoundary)
                            SimpleFilterCommand.RemoveObjects(
                                internalResult, 
                                (int)eDefectType.DarkObjectAcrossBoundary);
                        if (!classificationSettings.ClassifyBrightObjectAcrossBoundary)
                            SimpleFilterCommand.RemoveObjects(
                                internalResult,
                                (int)eDefectType.BrightObjectAcrossBoundary);

                        globalResult.AddRange(internalResult);
                    }
                }
                #endregion classify objects across boundary

                if (globalResult == null)
                    globalResult = new ArrayList(); /// ????
                {
                    if (globalResult != null)
                    {
                        workspace.DetectedObjects = globalResult;

                        analyzer = this.ObjectAnalyzer;
                        if (analyzer != null)
                        {
                            analyzer.Visible = true;
                            workspace.ActiveAnalyzer = analyzer;

                            if (analyzer.DetectedObjectsWindow != null)
                                analyzer.DetectedObjectsWindow.Visible = true;

                            workspace.Invalidate(true);
                        }
                    }
                }

                if (analyzer.AdvancedObjectAnalyzer != null)
                    analyzer.AdvancedObjectAnalyzer.ClearStorageWorkingObjects();
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
    }
}
