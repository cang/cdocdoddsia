using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

using SIA.Common;

using SIA.SystemLayer;

using SIA.Plugins;
using SIA.Plugins.Common;

using SIA.UI.Controls;
using SIA.UI.Controls.Commands;
using SIA.UI.Controls.Utilities;
using SIA.UI.Controls.Dialogs;

namespace SIA.UI.CommandHandlers
{
	internal class CmdFileSetDefaultFolder : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdFileSetDefaultFolder";
		
		private static MenuInfo menuInfo = null;
		
		static CmdFileSetDefaultFolder()
		{
			menuInfo = 
                new MenuInfo(
                    "Set &Default Folder...", Categories.File, Shortcut.None, 150, null, SeparateStyle.Both);
		}

		public CmdFileSetDefaultFolder(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, menuInfo, null, null)
		{
		}

		public override void DoCommand(params object[] args)
		{	
			using (FolderBrowserDialog dlg = CommonDialogs.SelectFolderDialog("Please select a default location"))
			{
				dlg.SelectedPath = CommonDialogs.DefaultFolder;
				if (dlg.ShowDialog(this.appWorkspace) == DialogResult.OK)
					CommonDialogs.DefaultFolder = dlg.SelectedPath;
			}
		}

		public override UIElementStatus QueryMenuItemStatus()
		{
			return UIElementStatus.Enable;
		}

	}
}
