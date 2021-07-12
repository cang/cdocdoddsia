#define SIA_PRODUCT

using System;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

using SIA.Common;
using SIA.Common.Utility;
using SIA.Common.Mask;

using SiGlaz.RDE.Ex.Mask;

using SIA.IPEngine;
using SIA.IPEngine.KlarfExport;

using SIA.SystemLayer;
using SIA.SystemLayer.Mask;

using SIA.SystemFrameworks;
using SIA.SystemFrameworks.UI;

using SIA.UI.MaskEditor;

using SIA.Plugins;
using SIA.Plugins.Common;

using SIA.UI.Controls;
using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.Commands;
using SIA.UI.Controls.Helpers.VisualAnalysis;

namespace SIA.UI.CommandHandlers
{
	internal class CmdToolsMaskEditor : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdToolsMaskEditor";
		
		private static MenuInfo menuInfo = null;
		private static ToolBarInfo toolbarInfo = null;
        private static ShortcutInfo shortcutInfo = null;

		static CmdToolsMaskEditor()
		{
#if SIA_PRODUCT
            Image image = SIAResources.GetMenuIcon("edit_mark");            
            toolbarInfo = new ToolBarInfo("Region Editor", "Region Editor", 700, image, SeparateStyle.Before);

            image = SIAResources.CreateMenuIconFromShortcutIcon(image);
            menuInfo = new MenuInfo("&Region Editor", Categories.Tools, Shortcut.None, 700, image, SeparateStyle.Both);

            // Cong: using menu icon as  shortcut icon
            image = SIAResources.GetShortcutIcon("edit_mark");
            shortcutInfo = new ShortcutInfo("Region Editor", Categories.Tools, 700, image);
#else
			Image image = RDEResources.GetMenuIcon("MskEdtIcon");
			menuInfo = new MenuInfo("&Mask Editor", Categories.Tools, Shortcut.None, 130, image, SeparateStyle.Both);
			toolbarInfo = new ToolBarInfo("Mask Editor", "Mask Editor", 250, image, SeparateStyle.Before);

            // Cong: using menu icon as  shortcut icon
            image = RDEResources.GetMenuIcon("MskEdtIcon");
            shortcutInfo = new ShortcutInfo("Mask Editor", Categories.Tools, 0, image);
#endif
		}

		public CmdToolsMaskEditor(IAppWorkspace appWorkspace)
            : base(appWorkspace, cmdCommandKey, menuInfo, toolbarInfo, shortcutInfo)
		{
		}

		public override void DoCommand(params object[] args)
		{
            IAppWorkspace appWorkspace = this.appWorkspace;
            ImageWorkspace workspace = this.Workspace;
            CommonImage image = workspace.Image;

            using (SIA.UI.MaskEditor.MaskEditor dlg = new SIA.UI.MaskEditor.MaskEditor(image, workspace.ImageViewer.RasterImageRender))
            {
                // register for apply mask event
                dlg.ApplyMask += new ApplyMask(MaskEditor_ApplyMask);
                
                // show mask editor window
                dlg.ShowDialog(workspace);

                // unregister for apply mask event
                dlg.ApplyMask -= new ApplyMask(MaskEditor_ApplyMask);
            }
		}

        private void MaskEditor_ApplyMask(object sender, MaskEditorApplyMaskEventArgs args)
        {
            MainFrame appWorkspace = this.appWorkspace;
            ImageWorkspace workspace = this.Workspace;
            CommonImage image = workspace.Image;

            if (args == null || args.GraphicsList == null || args.GraphicsList.Count <= 0)
                return;
            if (image == null)
                return;

            SIA.Common.Mask.IMask oldMask = null;

            try
            {
                int width = image.Width;
                int height = image.Height;

                SIA.Common.Mask.IMask mask = image.Mask;
                oldMask = mask;

                if (mask == null)
                {
                    mask = new SplitMask(width, height);
                    image.Mask = mask;
                }

                mask.GraphicsList = args.GraphicsList;
                
                // display mask if not visibled
                MaskViewer maskViewer = workspace.GetAnalyzer("MaskViewer") as MaskViewer;
                if (maskViewer != null)
                    maskViewer.Visible = true;
            }
            catch (System.Exception exp)
            {
                Trace.WriteLine(exp);

                // rollback to previous mask
                image.Mask = oldMask;

                // user notification
#if SIA_PRODUCT
                MessageBoxEx.Error("Failed to apply region: " + exp.Message);
#else
                MessageBoxEx.Error("Failed to apply mask: " + exp.Message);
#endif
            }
            finally
            {
                // update toolbar button status
                appWorkspace.UpdateUI();

                // invalidate 
                workspace.Invalidate(true);
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

