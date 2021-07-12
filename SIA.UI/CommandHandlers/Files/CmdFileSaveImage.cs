using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

using SIA.Plugins;
using SIA.Plugins.Common;

using SIA.UI.Controls;
using SIA.UI.Controls.Commands;
using SIA.UI.Controls.Utilities;
	
using SIA.Common;

namespace SIA.UI.CommandHandlers
{
	internal class CmdFileSaveImage : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdFileSaveImage";
		
		private static MenuInfo menuInfo = null;
		private static ToolBarInfo toolbarInfo = null;

		static CmdFileSaveImage()
		{
			Image image = SIAResources.GetMenuIcon("SaveIcon");
			menuInfo = new MenuInfo("&Save", Categories.File, Shortcut.CtrlS, 110, image, SeparateStyle.None);
			toolbarInfo = new ToolBarInfo("Save", "Save image to file", 110, image, SeparateStyle.After);
		}

		public CmdFileSaveImage(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, menuInfo, toolbarInfo, null)
		{
		}

		public override void DoCommand(params object[] args)
		{
            ImageWorkspace workspace = this.Workspace;

            // retrieve last saved file path
			string filePath = workspace.FilePath;

			// retrieve image format from file name
			eImageFormat format = kUtils.ImageFormatFromFileName(filePath);

			// just save image to its path
			workspace.SaveWorkspace(filePath, format);

			// set the default folder
			CommonDialogs.DefaultFolder = Path.GetDirectoryName(filePath);
		}

	}
}
