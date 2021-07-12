using System;
using System.IO;
using System.Windows.Forms;

using SIA.Common;
using SIA.Common.Utility;
	
using SIA.Plugins;
using SIA.Plugins.Common;

using SIA.UI.Controls;
using SIA.UI.Controls.Commands;
using SIA.UI.Controls.Utilities;

namespace SIA.UI.CommandHandlers
{
	internal class CmdFileSaveImageAs : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdFileSaveImageAs";
		
		private static MenuInfo menuInfo = null;

		static CmdFileSaveImageAs()
		{
			menuInfo = new MenuInfo("&Save as...", Categories.File, Shortcut.CtrlShiftS, 120);
		}

		public CmdFileSaveImageAs(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, menuInfo, null, null)
		{
		}

		public override void DoCommand(params object[] args)
		{
			MainFrame appWorkspace = this.appWorkspace;
			ImageWorkspace workspace = this.Workspace;

			string filePath = this.Workspace.FilePath;
			// retrieve save file as dialog from Dialogs Factory
			SaveFileDialog dlg = CommonDialogs.SaveImageFileDialog("Save image as...");

			// restore last used filter
			int lastUsedFilter = Convert.ToInt32(CustomConfiguration.GetValues("SAVEFILTERTYPE", -1));
			if (lastUsedFilter > 0) // filter index start is 1 based
				dlg.FilterIndex = lastUsedFilter;
				
			// initialize selected file path
            dlg.FileName = filePath;
				
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				// refresh ui before wait cursor
				appWorkspace.Refresh();
				// set wait cursor
				Cursor.Current = System.Windows.Forms.Cursors.WaitCursor ;

				// retrieve format from selected filter index
				eImageFormat format = kUtils.ImageFormatFromSelectedFilter(dlg);
				if (format == eImageFormat.Unknown)
					format = eImageFormat.Bmp; // set default to Bitmap if unknown format is given

                // save workspace
                workspace.SaveWorkspace(dlg.FileName, format);

                // update file path
                filePath = dlg.FileName;

                // add this new file to recent buffer
                SIARecentFiles.AddRecentFile(filePath);

                // update user elements
                appWorkspace.UpdateUI();

				// set the default folder
				CommonDialogs.DefaultFolder = Path.GetDirectoryName(filePath);

				// save last used filter
				CustomConfiguration.SetValues("SAVEFILTERTYPE", dlg.FilterIndex);
			}

			// update ui elements
			appWorkspace.UpdateUI();
		}
	}
}
