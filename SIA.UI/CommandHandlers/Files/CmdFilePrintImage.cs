using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

using SIA.Common;
using SIA.SystemLayer;

using SIA.Plugins;
using SIA.Plugins.Common;

using SIA.UI.Controls;
using SIA.UI.Controls.Commands;
using SIA.UI.Controls.Helpers;

namespace SIA.UI.CommandHandlers
{
	internal class CmdFilePrintImage : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdFilePrintImage";
		
		private static MenuInfo menuInfo = null;

		static CmdFilePrintImage()
		{
			menuInfo = 
                new MenuInfo(
                    "&Print", Categories.File, Shortcut.CtrlP, 160, null, SeparateStyle.Both);
		}

		public CmdFilePrintImage(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, menuInfo, null, null)
		{            
		}

		public override void DoCommand(params object[] args)
		{
            IAppWorkspace appWorkspace = this.appWorkspace;
            ImageWorkspace workspace = this.Workspace;
            CommonImage image = workspace.Image;
            
            String tempFilePath = Path.GetTempFileName();

            try
            {
                // safe image as bitmap
                image.SaveImage(tempFilePath, eImageFormat.Bmp);

                // print bitmap
                PrintHelper.Print(workspace, tempFilePath);
            }
            catch (System.Exception exp)
            {
                Trace.WriteLine(exp);

                MessageBoxEx.Error("Failed to print image: " + exp.Message);
            }
            finally
            {
                if (File.Exists(tempFilePath))
                    File.Delete(tempFilePath);
                tempFilePath = null;
            }
		}
	}
}
