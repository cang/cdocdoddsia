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
using SiGlaz.Common;
using SiGlaz.Common.ImageAlignment;
using SIA.UI.Controls.Commands;
using SIA.UI.Controls.Automation.Commands;
using System.IO;
using SIA.UI.Controls.Automation;
using SIA.UI.Controls.Dialogs;

namespace SIA.UI.CommandHandlers.Analysis
{
    internal class CmdAnalysisAlignPoleTip : AppWorkspaceCommand
    {
        public const string cmdCommandKey = "CmdAnalysisAlignPoleTip";

        private static MenuInfo menuInfo = null;
        private static ToolBarInfo toolbarInfo = null;
        private static ShortcutInfo shortcutInfo = null;

        static CmdAnalysisAlignPoleTip()
        {
            // 
            int index = 610;
            Image image = SIAResources.GetMenuIcon("align_ABS");/*align_PoleTip*/
            menuInfo =
                new MenuInfo(
                    "&Align Pole Tip", 
                    Categories.Analysis, Shortcut.None, index, image, SeparateStyle.Above);
            //toolbarInfo =
            //    new ToolBarInfo("Align Pole Tip", "Align Pole Tip", index, image, SeparateStyle.Before);
#if DEBUG
            shortcutInfo =
                new ShortcutInfo("Align Pole Tip", Categories.Analysis, index, image);
#endif
        }

        public CmdAnalysisAlignPoleTip(IAppWorkspace appWorkspace)
            : base(appWorkspace, cmdCommandKey, menuInfo, toolbarInfo, shortcutInfo)
        {

        }

        public override void DoCommand(params object[] args)
        {
            AlignABSCommandSettings alignSettings = LoadDefault();
            using (DlgABSAlignment dlg = new DlgABSAlignment(alignSettings))
            {
                if (dlg.ShowDialog(Workspace) != DialogResult.OK)
                    return;

                alignSettings = dlg.Settings;
                SaveDefault(alignSettings);
            }

            ImageWorkspace workspace = this.Workspace;
            CommonImage image = workspace.Image;

            IProgressCallback callback = null;
            try
            {
                // begin process
                callback = workspace.BeginProcess("Align Pole Tip...", false, ProgressType.Normal);
                callback.Begin(0, 100);

                // start wait cursor
                kUtils.kBeginWaitCursor();

                Settings settings = null;
                if (alignSettings.UseDefault)
                    settings = Settings.GetDefaultPoleTipAlignmentSettings();
                else
                    settings = Settings.Deserialize(alignSettings.FilePath);

                if (settings == null)
                    throw new Exception("Invalid settings");

                using (CommandExecutor cmdExec = new CommandExecutor(workspace))
                {
                    object[] result = cmdExec.DoCommandAlignPoleTip(image, settings);
                    if (result == null)
                        throw new Exception("Could not align");
                    System.Drawing.Drawing2D.Matrix inverseTransform = (System.Drawing.Drawing2D.Matrix)result[0];
                    SIA.IPEngine.GreyDataImage newImageData =
                        (SIA.IPEngine.GreyDataImage)result[1];
                    CommonImage newImage = new CommonImage(newImageData);
                    newImage.FilePath = workspace.Image.FilePath;

                    if (workspace.Image != null)
                    {
                        workspace.Image.Dispose();
                    }
                    workspace.Image = newImage;

                    Form form = workspace.FindForm();
                    if (form != null && form is MainFrame)
                    {
                        (form as MainFrame).StatusBar.UpdateData();
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

        private AlignABSCommandSettings LoadDefault()
        {
            try
            {
                string fileName = Path.Combine(
                    Application.UserAppDataPath, @"SiGlaz\SIA\DefaultSettings\AlignPoleTip.settings");
                PathHelper.CreateMissingFolderAuto(fileName);

                if (!File.Exists(fileName))
                    return new AlignABSCommandSettings();

                AlignABSCommandSettings settings =
                    RasterCommandSettingsSerializer.Deserialize(fileName, typeof(AlignABSCommandSettings)) as AlignABSCommandSettings;

                if (settings != null)
                    return settings;
            }
            catch
            {
                // nothting
            }

            return new AlignABSCommandSettings();
        }

        private void SaveDefault(AlignABSCommandSettings settings)
        {
            try
            {
                string fileName =
                    Path.Combine(
                    Application.UserAppDataPath, @"SiGlaz\SIA\DefaultSettings\AlignPoleTip.settings");
                PathHelper.CreateMissingFolderAuto(fileName);

                RasterCommandSettingsSerializer.Serialize(fileName, settings);
            }
            catch
            {
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
