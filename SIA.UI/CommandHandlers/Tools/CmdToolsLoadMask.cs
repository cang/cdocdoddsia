#define SIA_PRODUCT

using System;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

using SIA.Common;
using SIA.Common.Utility;
using SIA.Common.Mask;

//using SiGlaz.RDE.Ex.Mask;

using SIA.SystemLayer;
using SIA.SystemLayer.Mask;

using SIA.SystemFrameworks;
using SIA.SystemFrameworks.UI;

using SIA.Plugins;
using SIA.Plugins.Common;

using SIA.UI.Controls;
using SIA.UI.Controls.Commands;
using SIA.UI.Controls.Helpers.VisualAnalysis;

namespace SIA.UI.CommandHandlers
{
	internal class CmdToolsLoadMask : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdToolsLoadMask";
		
		private static MenuInfo menuInfo = null;
		private static ToolBarInfo toolbarInfo = null;
        private static ShortcutInfo shortcutInfo = null;

		static CmdToolsLoadMask()
		{
#if SIA_PRODUCT
            //Image image = RDEResources.GetMenuIcon("LoadMskIcon");
            Image image = SIAResources.GetMenuIcon("load_mark");
            toolbarInfo = new ToolBarInfo("Load Region", "Load Region from File", 710, image, SeparateStyle.None);

            image = SIAResources.CreateMenuIconFromShortcutIcon(image);
            menuInfo = new MenuInfo("&Load Region", Categories.Tools, Shortcut.None, 710, image, SeparateStyle.Above);
            
            // Cong: using menu icon as  shortcut icon
            //image = RDEResources.GetMenuIcon("LoadMskIcon");
            image = SIAResources.GetShortcutIcon("load_mark");
            shortcutInfo = new ShortcutInfo("Load Region", Categories.Tools, 710, image);
#else
            Image image = RDEResources.GetMenuIcon("LoadMskIcon");
			menuInfo = new MenuInfo("&Load Mask", Categories.Tools, Shortcut.None, 140, image, SeparateStyle.Above);
			toolbarInfo = new ToolBarInfo("Load Mask", "Load Mask from File", 260, image, SeparateStyle.None);

            // Cong: using menu icon as  shortcut icon
            image = RDEResources.GetMenuIcon("LoadMskIcon");
            shortcutInfo = new ShortcutInfo("Load Mask", Categories.Tools, 1, image);
#endif
        }

		public CmdToolsLoadMask(IAppWorkspace appWorkspace): 
            base(appWorkspace, cmdCommandKey, menuInfo, toolbarInfo, shortcutInfo)
		{
		}

		public override void DoCommand(params object[] args)
		{
            IAppWorkspace appWorkspace = this.appWorkspace;
            ImageWorkspace workspace = this.Workspace;
            CommonImage image = workspace.Image;
#if SIA_PRODUCT
            using (OpenFileDialog dlg = FileTypes.Mask.OpenFileDialog("Open region file.."))
#else
            using (OpenFileDialog dlg = FileTypes.Mask.OpenFileDialog("Open mask file.."))
#endif
            {
                if (dlg.ShowDialog(workspace) == DialogResult.OK)
                {
                    IMask mask = null, oldMask = null; ;
                    MaskHelper helper = null;

                    try
                    {
                        oldMask = image.Mask;
                        helper = new MaskHelper(image);
                        mask = helper.CreateMask(dlg.FileName);
                        image.Mask = mask;

                        // display mask if not visibled
                        MaskViewer maskViewer = workspace.GetAnalyzer("MaskViewer") as MaskViewer;
                        if (maskViewer != null)
                            maskViewer.Visible = true;
                    }
                    catch (System.Exception exp)
                    {
                        Trace.WriteLine(exp);

                        if (mask != null)
                        {
                            mask.Dispose();
                            mask = null;
                        }

                        // rollback to last used mask
                        image.AutoDisposeMask = false;
                        image.Mask = oldMask;
                        image.AutoDisposeMask = true;

                        // display mask if not visibled
                        MaskViewer maskViewer = workspace.GetAnalyzer("MaskViewer") as MaskViewer;
                        maskViewer.Visible = maskViewer.Visible;

                        // user notification
#if SIA_PRODUCT
                        MessageBoxEx.Error("Failed to load region from file:" + exp.Message);
#else
                        MessageBoxEx.Error("Failed to load mask from file:" + exp.Message);
#endif
                    }
                    finally
                    {
                        if (helper != null)
                        {
                            helper.Dispose();
                            helper = null;
                        }
                    }
                }
            }
		}

        public void DoCommandLoadMaskFromFile(string filePath)
        {
            IAppWorkspace appWorkspace = this.appWorkspace;
            ImageWorkspace workspace = this.Workspace;
            CommonImage image = workspace.Image;
#if SIA_PRODUCT
            using (OpenFileDialog dlg = FileTypes.Mask.OpenFileDialog("Open region file.."))
#else
            using (OpenFileDialog dlg = FileTypes.Mask.OpenFileDialog("Open mask file.."))
#endif
            {
                //if (dlg.ShowDialog(workspace) == DialogResult.OK)
                {
                    IMask mask = null, oldMask = null; ;
                    MaskHelper helper = null;

                    try
                    {
                        oldMask = image.Mask;
                        helper = new MaskHelper(image);
                        //mask = helper.CreateMask(dlg.FileName);
                        mask = helper.CreateMask(filePath);
                        image.Mask = mask;

                        // display mask if not visibled
                        MaskViewer maskViewer = workspace.GetAnalyzer("MaskViewer") as MaskViewer;
                        if (maskViewer != null)
                            maskViewer.Visible = true;
                    }
                    catch (System.Exception exp)
                    {
                        Trace.WriteLine(exp);

                        if (mask != null)
                        {
                            mask.Dispose();
                            mask = null;
                        }

                        // rollback to last used mask
                        image.AutoDisposeMask = false;
                        image.Mask = oldMask;
                        image.AutoDisposeMask = true;

                        // display mask if not visibled
                        MaskViewer maskViewer = workspace.GetAnalyzer("MaskViewer") as MaskViewer;
                        maskViewer.Visible = maskViewer.Visible;

                        // user notification
#if SIA_PRODUCT
                        MessageBoxEx.Error("Failed to load region from file:" + exp.Message);
#else
                        MessageBoxEx.Error("Failed to load mask from file:" + exp.Message);
#endif
                    }
                    finally
                    {
                        if (helper != null)
                        {
                            helper.Dispose();
                            helper = null;
                        }
                    }
                }
            }
        }

        public void DoCommandLoadMaskFromMaskData(byte[] maskData)
        {
            IAppWorkspace appWorkspace = this.appWorkspace;
            ImageWorkspace workspace = this.Workspace;
            CommonImage image = workspace.Image;
#if SIA_PRODUCT
            using (OpenFileDialog dlg = FileTypes.Mask.OpenFileDialog("Open region file.."))
#else
            using (OpenFileDialog dlg = FileTypes.Mask.OpenFileDialog("Open mask file.."))
#endif
            {
                //if (dlg.ShowDialog(workspace) == DialogResult.OK)
                {
                    IMask mask = null, oldMask = null; ;
                    MaskHelper helper = null;

                    try
                    {
                        oldMask = image.Mask;
                        helper = new MaskHelper(image);
                        //mask = helper.CreateMask(dlg.FileName);

                        mask = helper.CreateMask(helper.LoadMask(maskData));
                        image.Mask = mask;

                        // display mask if not visibled
                        MaskViewer maskViewer = workspace.GetAnalyzer("MaskViewer") as MaskViewer;
                        if (maskViewer != null)
                            maskViewer.Visible = true;
                    }
                    catch (System.Exception exp)
                    {
                        Trace.WriteLine(exp);

                        if (mask != null)
                        {
                            mask.Dispose();
                            mask = null;
                        }

                        // rollback to last used mask
                        image.AutoDisposeMask = false;
                        image.Mask = oldMask;
                        image.AutoDisposeMask = true;

                        // display mask if not visibled
                        MaskViewer maskViewer = workspace.GetAnalyzer("MaskViewer") as MaskViewer;
                        maskViewer.Visible = maskViewer.Visible;

                        // user notification
#if SIA_PRODUCT
                        MessageBoxEx.Error("Failed to load region from file:" + exp.Message);
#else
                        MessageBoxEx.Error("Failed to load mask from file:" + exp.Message);
#endif
                    }
                    finally
                    {
                        if (helper != null)
                        {
                            helper.Dispose();
                            helper = null;
                        }
                    }
                }
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

