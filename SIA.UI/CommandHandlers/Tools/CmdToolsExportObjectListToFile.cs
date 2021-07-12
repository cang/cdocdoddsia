using System;
using System.Collections.Generic;
using System.Text;
using SIA.UI.Controls;
using SIA.SystemLayer;
using System.Collections;
using SIA.SystemFrameworks.UI;
using System.Windows.Forms;
using SIA.UI.Controls.Utilities;
using SIA.Common;
using System.Drawing;
using SIA.UI.Controls.Automation.Commands;
using System.Drawing.Imaging;
using SIA.Plugins.Common;
using SIA.UI.Controls.Commands;
using SiGlaz.Common;
using System.Diagnostics;
using SIA.UI.Controls.Commands.Analysis;
using SIA.UI.Controls.Helpers.VisualAnalysis;

namespace SIA.UI.CommandHandlers.Tools
{
    internal class CmdToolsExportObjectListToFile : AppWorkspaceCommand
	{
        public const string cmdCommandKey = "CmdToolsExportObjectListToFile";
		
		private static MenuInfo menuInfo = null;
		private static ToolBarInfo toolbarInfo = null;
		private static ShortcutInfo shortcutInfo = null;

        static CmdToolsExportObjectListToFile()
		{
            // use overlay klarf
            string cmdDescription = "Export anomalies to File";

            Image shortcutImage = SIAResources.GetShortcutIcon("export_defect_list");
            Image image = SIAResources.CreateMenuIconFromShortcutIcon(shortcutImage);
            menuInfo = 
                new MenuInfo(
                    "Export anomalies to File", 
                    Categories.Tools, 
                    Shortcut.None, 747, 
                    image, SeparateStyle.Above);

            shortcutInfo = 
                new ShortcutInfo(
                    cmdDescription, 
                    Categories.Tools, 
                    747,
                    shortcutImage);			
		}

        public CmdToolsExportObjectListToFile(IAppWorkspace appWorkspace)
            : base(appWorkspace, cmdCommandKey, menuInfo, toolbarInfo, shortcutInfo)
		{
			
		}

		public override void DoCommand(params object[] args)
        {
            ImageWorkspace workspace = this.Workspace;
            CommonImage image = workspace.Image;
            ArrayList defectList = workspace.DetectedObjects;
            if (defectList == null)
                return;                        
            
            IProgressCallback callback = null;
            try
            {
                // begin process
                callback = workspace.BeginProcess("Overlaying anomalies on image...", false, ProgressType.Normal);
                callback.Begin(0, 100);

                // start wait cursor
                kUtils.kBeginWaitCursor();

                ArrayList _objectList = defectList;

                if (_objectList == null)
                {
                    MessageBoxEx.Error("Anomalies were not detected or not found on the image. Please try again!");
                }
                else if (_objectList.Count <= 0)
                {
                    MessageBoxEx.Error("Anomalies were not found on the image. Please try again!");
                }
                else
                {

                    using (SaveFileDialog dlg =
                        SIA.UI.Controls.Utilities.CommonDialogs.SaveCsvFileDialog("Export detected anomalies"))
                    {
                        dlg.Filter = "Command Separated Values (*.csv)|*.csv";
                        dlg.FilterIndex = 0;

                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            try
                            {
                                DefectExporter.SaveAsCSV(
                                    _objectList, workspace.GetCurrentMetrologySystem(),
                                    dlg.FileName, workspace.FilePath, "");

                                MessageBoxEx.Info("Anomalies were exported successfully!");

                                System.Diagnostics.Process.Start(dlg.FileName);
                            }
                            catch (Exception exp)
                            {
                                Trace.WriteLine(exp);

                                MessageBoxEx.Error("Failed to export anomalies to file.");
                            }
                            finally
                            {

                            }
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

        protected UIElementStatus GetUIStatus()
        {
            if (this.Workspace == null)
                return UIElementStatus.Disable;
            if (this.Workspace.DetectedObjects == null)
                return UIElementStatus.Disable;
            if (this.Workspace.DetectedObjects.Count == 0)
                return UIElementStatus.Disable;

            MetrologyAnalyzer analyzer = this.Workspace.GetAnalyzer("MetrologyAnalyzer") as MetrologyAnalyzer;
            if (analyzer != null && analyzer.Visible)
                return UIElementStatus.Disable;

            return UIElementStatus.Enable;
        }

        public override UIElementStatus QueryMenuItemStatus()
        {
            return GetUIStatus();
        }

        public override UIElementStatus QueryShortcutBarItemStatus()
        {
            return GetUIStatus();
        }
    }
}
