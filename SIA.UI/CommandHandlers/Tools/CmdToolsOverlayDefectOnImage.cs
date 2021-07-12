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
using SIA.UI.Controls.Helpers.VisualAnalysis;

namespace SIA.UI.CommandHandlers.Tools
{
    internal class CmdToolsOverlayDefectOnImage : AppWorkspaceCommand
	{
        public const string cmdCommandKey = "CmdToolsOverlayDefectOnImage";
		
		private static MenuInfo menuInfo = null;
		private static ToolBarInfo toolbarInfo = null;
		private static ShortcutInfo shortcutInfo = null;

        static CmdToolsOverlayDefectOnImage()
		{
            // use overlay klarf
            string cmdDescription = "Export overlaid anomalies on image to File";

            Image image = SIAResources.GetMenuIcon("OverlayIcon");
            menuInfo = 
                new MenuInfo(
                    "&Export overlaid anomalies on image to File", 
                    Categories.Tools, 
                    Shortcut.None, 746, 
                    image, SeparateStyle.Above);

            //toolbarInfo = 
            //    new ToolBarInfo(
            //        cmdDescription, 
            //        cmdDescription, 
            //        680, 
            //        image, SeparateStyle.Before);

            shortcutInfo = 
                new ShortcutInfo(
                    cmdDescription, 
                    Categories.Tools, 
                    746, 
                    SIAResources.GetShortcutIcon("overlayklarf"));			
		}

        public CmdToolsOverlayDefectOnImage(IAppWorkspace appWorkspace)
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

            SaveFileDialog dlg = CommonDialogs.SaveImageFileDialog("Save image as...");
            string fileName = "";
            eImageFormat format = eImageFormat.Bmp;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                fileName = dlg.FileName;

                format = kUtils.ImageFormatFromSelectedFilter(dlg);
                if (format == eImageFormat.Unknown)
                    format = eImageFormat.Bmp; // set default to Bitmap if unknown format is given
            }
            else return;             
            
            IProgressCallback callback = null;
            try
            {
                // begin process
                callback = workspace.BeginProcess("Overlaying anomalies on image...", false, ProgressType.Normal);
                callback.Begin(0, 100);

                // start wait cursor
                kUtils.kBeginWaitCursor();

                Image overlaidImage = null;
                using (CommandExecutor cmdExec = new CommandExecutor(workspace))
                {
                    overlaidImage = 
                        cmdExec.DoCommandOverlayDefectOnImage(image, defectList);
                }

                if (overlaidImage != null)
                {
                    try
                    {
                        PathHelper.CreateMissingFolderAuto(fileName);

                        ImageFormat imageFormat = ImageFormat.Bmp;
                        switch (format)
                        {
                            case eImageFormat.Gif:
                                imageFormat = ImageFormat.Gif;
                                break;
                            case eImageFormat.Jpeg:
                                imageFormat = ImageFormat.Jpeg;
                                break;
                            case eImageFormat.Png:
                                imageFormat = ImageFormat.Png;
                                break;
                            case eImageFormat.Tiff:
                                imageFormat = ImageFormat.Tiff;
                                break;
                            default:
                                break;
                        }

                        overlaidImage.Save(fileName, imageFormat);

                        System.Diagnostics.Process.Start(fileName);
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        overlaidImage.Dispose();
                        overlaidImage = null;
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
