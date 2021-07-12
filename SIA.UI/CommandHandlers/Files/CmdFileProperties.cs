using System;
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
	internal class CmdFileProperties : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdFileProperties";
		
		private static MenuInfo menuInfo = null;
		private static ToolBarInfo toolbarInfo = null;

		static CmdFileProperties()
		{
			menuInfo = 
                new MenuInfo(
                "&Properties...", Categories.File, Shortcut.None, 140, null, SeparateStyle.Both);
		}

		public CmdFileProperties(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, menuInfo, toolbarInfo, null)
		{
		}

		public override void DoCommand(params object[] args)
		{			
			CommonImage image = this.Workspace.Image;
			using (DlgImageProperties dlg = new DlgImageProperties(image))
			{
				if (DialogResult.OK == dlg.ShowDialog(this.appWorkspace))
				{
					
				}
			}
		}

	}
}
