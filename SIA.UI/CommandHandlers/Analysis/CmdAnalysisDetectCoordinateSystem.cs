using System;
using System.Collections.Generic;
using System.Text;
using SIA.Plugins.Common;
using System.Drawing;
using System.Windows.Forms;
using SIA.UI.Controls.Utilities;
using SIA.UI.Controls;
using SIA.SystemLayer;
using SIA.SystemFrameworks.UI;
using SiGlaz.Common;
using SiGlaz.Common.ImageAlignment;
using SIA.Algorithms.Preprocessing.Alignment;
using SIA.IPEngine;
using SIA.UI.Controls.Commands;
using SIA.UI.Controls.Helpers.VisualAnalysis;
using SIA.Algorithms.Regions;

namespace SIA.UI.CommandHandlers.Analysis
{
    internal class CmdAnalysisDetectCoordinateSystem : AppWorkspaceCommand
    {
        public const string cmdCommandKey = "CmdAnalysisDetectCoordinateSystem";
		
		private static MenuInfo menuInfo = null;
		private static ToolBarInfo toolbarInfo = null;
		private static ShortcutInfo shortcutInfo = null;
        
		static CmdAnalysisDetectCoordinateSystem()
		{
            // 
            Image image = SIAResources.GetMenuIcon("coordinate_system");
            menuInfo = 
                new MenuInfo("&Detect Coordinate System", Categories.Analysis, Shortcut.None, 600, image, SeparateStyle.None);
            toolbarInfo =
                new ToolBarInfo("Detect Coordinate System", "Detect Coordinate System", 600, image, SeparateStyle.Before);
            shortcutInfo =
                new ShortcutInfo("Detect Coordinate System", Categories.Analysis, 600, image);			
		}

        public CmdAnalysisDetectCoordinateSystem(IAppWorkspace appWorkspace)
            : base(appWorkspace, cmdCommandKey, menuInfo, toolbarInfo, shortcutInfo)
		{			
		}

        public override void DoCommand(params object[] args)
        {
            ImageWorkspace workspace = this.Workspace;
            CommonImage image = workspace.Image;

            string filePath = "";
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Please specify Reference File";
                dlg.Filter = MetrologySystemReference.FileFilter;
                dlg.RestoreDirectory = true;

                if (dlg.ShowDialog(workspace.AppWorkspace) == DialogResult.OK)
                {
                    filePath = dlg.FileName;
                }
                else return;
            }

            IProgressCallback callback = null;
            try
            {
                MetrologyAnalyzer metrologyAnalyzer = workspace.GetAnalyzer("MetrologyAnalyzer") as MetrologyAnalyzer;
                metrologyAnalyzer.Visible = false;

                // begin process
                callback = workspace.BeginProcess("Detecting Coordinate System...", false, ProgressType.Normal);
                callback.Begin(0, 100);

                // start wait cursor
                kUtils.kBeginWaitCursor();

                //string filePath = @"D:\WorkingSpace\ABS\Main\Docs\Requirements\Team\abs_ref_file.msr";
                //MetrologySystemReference refFile = MetrologySystemReference.Deserialize(filePath);
                workspace.SetRefFile(filePath);
                MetrologySystemReference refFile = workspace.RefFile;

                if (refFile.Regions == null)
                {
                    DialogResult dlgResult = 
                        MessageBox.Show(
                        workspace.AppWorkspace, 
                        string.Format("The Reference File: {0} has no region.\n\nDo you want to continue?", filePath), 
                        "Confirmation", 
                        MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Question);

                    if (dlgResult == DialogResult.No)
                        return;
                }

                using (CommandExecutor cmdExec = new CommandExecutor(workspace))
                {
                    object[] result = cmdExec.DoCommandDetectCoordinateSystem(image, refFile);

                    MetrologySystem ms = result[0] as MetrologySystem;
                    AlignmentResult alignmentResult = result[1] as AlignmentResult;
                    workspace.AlignmentResult = alignmentResult;

                    ImageAnalyzer analyzer = workspace.GetAnalyzer("ImageAnalyzer") as ImageAnalyzer;
                    analyzer.SetCoordinateSystemVisibleFlag(true);
                    analyzer.SetMetrologySystem(ms);
                    
                    analyzer.Regions = 
                        ABSRegionProcessor.CorrectRegions(
                        refFile, ms, alignmentResult, image.RasterImage);
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

        ///**
        // string abs_golden = @"D:\WorkingSpace\ABS\Main\Source\Delivery\SIA_Demo30Days_Xyratex_2011_06_01\ABSGoldenImage\ABS10x.63.0.bmp";
        //        string out_abs = @"D:\WorkingSpace\ABS\Main\Source\Delivery\SIA_Demo30Days_Xyratex_2011_06_01\ABSGoldenImage\ABS.bmp";
        //        int w = image.Width;
        //        int h = image.Height;
        //        using (GreyDataImage gimage = new GreyDataImage(w, h))
        //        {
        //            using (GreyDataImage tmpImage = GreyDataImage.FromFile(abs_golden))
        //            {
        //                int w2 = tmpImage.Width;
        //                int h2 = tmpImage.Height;

        //                int l = (w - w2) / 2;
        //                int t = (h - h2) / 2;
                        
        //                unsafe
        //                {
        //                    ushort* dst = gimage._aData;
        //                    ushort* src = tmpImage._aData;
        //                    int index = 0;
        //                    for (int ySrc = 0; ySrc < h2; ySrc++)
        //                    {
        //                        int dstIndex = (ySrc + t) * w + l;
        //                        for (int xSrc = 0; xSrc < w2; xSrc++, dstIndex++, index++)
        //                        {
        //                            dst[dstIndex] = src[index];
        //                        }
        //                    }
        //                }
        //            }

        //            gimage.SaveImage(out_abs, SIA.Common.eImageFormat.Bmp);
        //        }
        // * */
    }
}
