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
using SIA.UI.Controls.Helpers.VisualAnalysis;
using SIA.SystemLayer;
using SIA.UI.Controls.Dialogs;

namespace SIA.UI.CommandHandlers
{
	/// <summary>
	/// Summary description for AppWorkspaceCommand.
	/// </summary>
	internal abstract class AppWorkspaceCommand 
        : BaseUICommand
	{
		public new MainFrame appWorkspace
		{
			get {return base.AppWorkspace as MainFrame;}
		}

		public ImageWorkspace Workspace
		{
			get
			{
				return this.appWorkspace != null? this.appWorkspace.ImageView : null;
			}
		}

		public AppWorkspaceCommand(IAppWorkspace appWorkspace, string command, MenuInfo menuInfo, ToolBarInfo toolbarInfo, ShortcutInfo shortcutInfo) 
			: base(appWorkspace, command, menuInfo, toolbarInfo, shortcutInfo)
		{
		}

		public override UIElementStatus QueryMenuItemStatus()
		{
			ImageWorkspace workspace = this.Workspace;
			return workspace.Image == null ? UIElementStatus.Disable : UIElementStatus.Enable;
		}

		public override UIElementStatus QueryToolBarItemStatus()
		{			
			ImageWorkspace workspace = this.Workspace;
			return workspace.Image == null ? UIElementStatus.Disable : UIElementStatus.Enable;
		}

		public override UIElementStatus QueryShortcutBarItemStatus()
		{
			ImageWorkspace workspace = this.Workspace;
			return workspace.Image == null ? UIElementStatus.Disable : UIElementStatus.Enable;
		}

        public virtual string GetDefaultSettingsPath()
        {
            string path = string.Empty;

            try
            {                
                path = 
                    Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), 
                    string.Format(@"{0}\{1}\{2}", 
                    Application.CompanyName, Application.ProductName, "DefaultSettings"));
            }
            catch
            {
                path = string.Empty;
            }

            return path;
        }

        public virtual bool CreateDefaultSettingsPath()
        {
            bool succeed = true;
            try
            {
                string path = GetDefaultSettingsPath();

                if (path == null || path == string.Empty)
                    return false;

                if (System.IO.Directory.Exists(path))
                    return true;

                System.IO.Directory.CreateDirectory(path);
            }
            catch
            {
                succeed = false;
            }

            return succeed;
        }
	}
}
