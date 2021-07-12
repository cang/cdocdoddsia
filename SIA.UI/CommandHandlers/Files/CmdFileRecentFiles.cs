namespace SIA.UI.CommandHandlers
{
	using System;
	using System.IO;
	using System.Windows.Forms;
	using System.Diagnostics;

	using SIA.Plugins;
	using SIA.Plugins.Common;

	using SIA.UI.Controls;
	using SIA.UI.Controls.Common;
	using SIA.UI.Controls.Commands;
	using SIA.UI.Controls.Utilities;
    using SIA.UI.Components;

	internal class CmdFileOpenRecentFiles : AppWorkspaceCommand
	{
		public const string cmdCommandKey = "CmdFileOpenRecentFiles";
		
		private static MenuInfo menuInfo = null;

		static CmdFileOpenRecentFiles()
		{
			menuInfo = 
                new MenuInfo(
                    "&Recent Files", Categories.File, Shortcut.None, 170, null, SeparateStyle.Both);
		}

		public CmdFileOpenRecentFiles(IAppWorkspace appWorkspace): base(appWorkspace, cmdCommandKey, menuInfo, null, null)
		{
		}

		public override void DoCommand(params object[] args)
		{
			MainFrame appWorkspace = this.appWorkspace;
			ImageWorkspace workspace = this.Workspace;

			try
            {
                kUtils.kBeginWaitCursor();
				string fileName = args[0] as String;

				if (File.Exists(fileName))
				{
					// dispatch close image command
					if (workspace.Image != null)
					{
						// dispatch close image command
						object[] output = new object[] {false};
						appWorkspace.DispatchCommand(CmdFileCloseImage.cmdCommandKey, output);
						bool isCancel = (bool)output[0];
						if (isCancel)
							return;
					}
				
					// load image from file
					workspace.CreateWorkspace(fileName);
					
					// update recent file
					SIARecentFiles.AddRecentFile(fileName);

					// set the default folder
					CommonDialogs.DefaultFolder = Path.GetDirectoryName(fileName);
				}
				else // file does not exist
				{
					// update recent file
					SIARecentFiles.RemoveRecentFile(fileName);

					MessageBoxEx.Error("File \"" + fileName + "\" was not found.");
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
				kUtils.kEndWaitCursor();

                appWorkspace.UpdateUI();
			}
		}

		public override UIElementStatus QueryMenuItemStatus()
		{
			return UIElementStatus.Enable;
		}

	}
}
