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

namespace SIA.UI.CommandHandlers.Analysis
{
    internal class CmdAnalysisDetectScratchs : AppWorkspaceCommand
	{
        public const string cmdCommandKey = "CmdAnalysisDetectScratchs";
		
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

        private static string description = "Bright object in open area";

		static CmdAnalysisDetectScratchs()
		{
            //// nn classify
            //Image image = SIAResources.GetMenuIcon("neural_network_classify");
            //menuInfo = 
            //    new MenuInfo("&Classify Scratch", Categories.Analysis, Shortcut.None, 620, image, SeparateStyle.None);
            //toolbarInfo = 
            //    new ToolBarInfo("Classify Scratch", "Classify Scratch using Neural Network", 620, image, SeparateStyle.Before);
            //shortcutInfo = 
            //    new ShortcutInfo("Classify Scratch", Categories.Analysis, 620, SIAResources.GetShortcutIcon("neural_network_classify"));			

            

            // nn classify
            Image image = SIAResources.GetMenuIcon("neural_network_classify");
            menuInfo =
                new MenuInfo(
                    string.Format("&Classify {0}", description), 
                    Categories.Analysis, Shortcut.None, 620, image, SeparateStyle.None);
            toolbarInfo =
                new ToolBarInfo(
                    string.Format("Classify {0}", description),
                    string.Format("Classify {0} using Neural Network", description), 
                    620, image, SeparateStyle.Before);
            shortcutInfo =
                new ShortcutInfo(
                    string.Format("Classify {0}", description), 
                    Categories.Analysis, 620, SIAResources.GetShortcutIcon("neural_network_classify"));			
		}

        public CmdAnalysisDetectScratchs(IAppWorkspace appWorkspace)
            : base(appWorkspace, cmdCommandKey, menuInfo, toolbarInfo, shortcutInfo)
		{
			
		}

		public override void DoCommand(params object[] args)
        {            
            ImageWorkspace workspace = this.Workspace;
            CommonImage image = workspace.Image;


            // save object detection settings
            object[] settings = null;

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

                ArrayList result = null;

                using (CommandExecutor cmdExec = new CommandExecutor(workspace))
                {
                    // refer to CmdAnalysisDetectObjects.cs
                    result = cmdExec.DoCommandDetectScratchs(image);
                }

                if (result == null)
                    result = new ArrayList(); /// ????
                {
                    if (result != null)
                    {
                        workspace.DetectedObjects = result;

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
