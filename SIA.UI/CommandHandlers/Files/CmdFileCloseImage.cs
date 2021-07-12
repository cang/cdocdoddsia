using System;
using System.Windows.Forms;

using SIA.Plugins;
using SIA.Plugins.Common;

using SIA.UI.Controls;
using SIA.UI.Controls.Common;
using SIA.UI.Controls.Commands;
using SIA.UI.Controls.Utilities;

namespace SIA.UI.CommandHandlers
{
    internal class CmdFileCloseImage : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdFileCloseImage";
		
		private static MenuInfo menuInfo = null;

		static CmdFileCloseImage()
		{
			menuInfo = 
                new MenuInfo(
                    "&Close...", Categories.File, Shortcut.None, 130, null, SeparateStyle.Both);
		}

		public CmdFileCloseImage(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, menuInfo, null, null)
		{
		}

		public override void DoCommand(params object[] args)
		{
			MainFrame appWorkspace = this.appWorkspace;
			ImageWorkspace workspace = this.Workspace;

			if (workspace.FilePath != "" && workspace.Modified)
			{
				DialogResult result = MessageBoxEx.ConfirmYesNoCancel("The image has been modified. Do you want to save the changes?");
				if (result == DialogResult.Yes)
				{
					appWorkspace.DispatchCommand(CmdFileSaveImage.cmdCommandKey);
				}
				else if (result == DialogResult.Cancel)
				{
					if (args!=null && args.Length > 0 && args[0] is Boolean)
						args[0] = true;
					return ;
				}
			}

			// close image
			workspace.DestroyWorkspace();

			// empty file path
			workspace.FilePath = "";
			
			// update ui elements
			appWorkspace.UpdateUI();
		}

	}
}
