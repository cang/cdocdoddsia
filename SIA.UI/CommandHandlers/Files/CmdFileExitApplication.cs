using System;
using System.Windows.Forms;

using SIA.Plugins;
using SIA.Plugins.Common;

using SIA.UI.Controls;
using SIA.UI.Controls.Commands;
using SIA.Common.Native;
	
namespace SIA.UI.CommandHandlers
{
	internal class CmdFileExitApplication : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdFileExitApplication";
		
		private static MenuInfo menuInfo = null;

		static CmdFileExitApplication()
		{
			menuInfo = 
                new MenuInfo(
                    "E&xit", Categories.File, Shortcut.None, 180, null, SeparateStyle.Both);
		}

		public CmdFileExitApplication(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, menuInfo, null, null)
		{
		}

		public override void DoCommand(params object[] args)
		{
			IntPtr hWnd = this.appWorkspace.Handle;
			NativeMethods.PostMessage(hWnd, SIA.Common.Native.NativeMethods.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
		} 

		public override UIElementStatus QueryMenuItemStatus()
		{
			return UIElementStatus.Enable;
		}
	}
}
