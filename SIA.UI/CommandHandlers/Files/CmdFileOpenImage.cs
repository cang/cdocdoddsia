using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;

using SIA.Common;
using SIA.Common.Utility;

using SIA.Plugins;
using SIA.Plugins.Common;

using SIA.UI.Controls;
using SIA.UI.Controls.Common;
using SIA.UI.Controls.Commands;
using SIA.UI.Controls.Utilities;
using SIA.UI.Controls.Dialogs;

using SIA.UI.Helpers;
using SIA.UI.Components;

namespace SIA.UI.CommandHandlers
{
	internal class CmdFileOpenImage : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdFileOpenImage";
		
		private static MenuInfo menuInfo = null;
		private static ToolBarInfo toolbarInfo = null;

		static CmdFileOpenImage()
		{
			Image image = SIAResources.GetMenuIcon("OpenIcon");
			menuInfo = new MenuInfo("&Open", Categories.File, Shortcut.CtrlO, 100, image, SeparateStyle.None);
			toolbarInfo = new ToolBarInfo("Open", "Open image from file", 100, image, SeparateStyle.None);
		}

		public CmdFileOpenImage(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, menuInfo, toolbarInfo, null)
		{
		}

		public override void DoCommand(params object[] args)
		{
			MainFrame appWorkspace = this.appWorkspace;
			ImageWorkspace workspace = this.Workspace;
			string oldFile = workspace.FilePath;

			try
			{
				if (workspace.FilePath != String.Empty && workspace.Modified)
				{
					DialogResult result = MessageBoxEx.ConfirmYesNoCancel("The image has been modified. Do you want to save the changes?");
					if (result == DialogResult.Yes)
						appWorkspace.DispatchCommand(CmdFileSaveImage.cmdCommandKey);
					else if (result == DialogResult.Cancel)
						return ;
				}

				using (OpenImageFile dlg = FileTypes.OpenImageFileDialog("Select an image..."))
				{
					if (dlg.ShowDialog(appWorkspace) == DialogResult.OK)
					{
						appWorkspace.Refresh();

						// dispatch close image command
						appWorkspace.DispatchCommand(CmdFileCloseImage.cmdCommandKey);

						kUtils.kBeginWaitCursor();
						workspace.FilePath = string.Empty;
						
                        // create workspace
                        workspace.CreateWorkspace(dlg.FileName);
						
                        // default selection mode
                        workspace.SelectMode();

						// update workspace file path
						workspace.FilePath = dlg.FileName;

						// update recent files
						SIARecentFiles.AddRecentFile(dlg.FileName);

						// set the default folder
						CommonDialogs.DefaultFolder = Path.GetDirectoryName(dlg.FileName);
						
						kUtils.kEndWaitCursor();
					}
				}
			}
			catch(System.OutOfMemoryException exp)
			{
				Trace.WriteLine(exp);
				MessageBoxEx.Error(ResourceHelper.ApplicationName + " cannot read this file.\r\nNot Enough Memory.");
			}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);
				MessageBoxEx.Error(ResourceHelper.ApplicationName + " cannot read this file.\r\nThis is not a valid file format, or its format is not currently supported.");				
			}
			finally
			{
				appWorkspace.UpdateUI();
			}
		}

        public void DoCommandOpenFromMemoryStream(MemoryStream fs, string filePath)
        {
            MainFrame appWorkspace = this.appWorkspace;
            ImageWorkspace workspace = this.Workspace;
            string oldFile = workspace.FilePath;

            try
            {
                if (workspace.FilePath != String.Empty && workspace.Modified)
                {
                    DialogResult result = MessageBoxEx.ConfirmYesNoCancel("The image has been modified. Do you want to save the changes?");
                    if (result == DialogResult.Yes)
                        appWorkspace.DispatchCommand(CmdFileSaveImage.cmdCommandKey);
                    else if (result == DialogResult.Cancel)
                        return;
                }

                // open image frome file stream
                {
                    appWorkspace.Refresh();

                    // dispatch close image command
                    appWorkspace.DispatchCommand(CmdFileCloseImage.cmdCommandKey);

                    kUtils.kBeginWaitCursor();
                    workspace.FilePath = string.Empty;

                    // create workspace
                    workspace.CreateWorkspace(fs);

                    // default selection mode
                    workspace.SelectMode();

                    // update workspace file path
                    workspace.FilePath = filePath;

                    // update recent files
                    //RDERecentFiles.AddRecentFile(dlg.FileName);

                    // set the default folder
                    //CommonDialogs.DefaultFolder = Path.GetDirectoryName(dlg.FileName);

                    kUtils.kEndWaitCursor();
                }
            }
            catch (System.OutOfMemoryException exp)
            {
                Trace.WriteLine(exp);
                MessageBoxEx.Error(ResourceHelper.ApplicationName + " cannot read this file.\r\nNot Enough Memory.");
            }
            catch (System.Exception exp)
            {
                Trace.WriteLine(exp);
                MessageBoxEx.Error(ResourceHelper.ApplicationName + " cannot read this file.\r\nThis is not a valid file format, or its format is not currently supported.");
            }
            finally
            {
                appWorkspace.UpdateUI();
            }
        }

		public override UIElementStatus QueryMenuItemStatus()
		{
			return UIElementStatus.Enable;
		}

		public override UIElementStatus QueryToolBarItemStatus()
		{
			return UIElementStatus.Enable;
		}
	}
}
