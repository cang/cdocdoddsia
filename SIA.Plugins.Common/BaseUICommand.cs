using System;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;

namespace SIA.Plugins.Common
{
	/// <summary>
	/// Provides base implementation of user interactive command objects.
	/// </summary>
	public abstract class BaseUICommand 
        : BaseCommand, IUICommandHandler
	{
		#region Fields
        		
		private IAppWorkspace _appWorkspace = null;
		private MenuInfo _menuInfo = null;
		private ToolBarInfo _toolBarInfo = null;
		private ShortcutInfo _shortcutInfo = null;

		#endregion

		#region IUICommandHandler Members

        /// <summary>
        /// Gets the application workspace 
        /// </summary>
		public IAppWorkspace AppWorkspace
		{
			get {return _appWorkspace;}
		}

        /// <summary>
        /// Gets the menu item information 
        /// </summary>
		public MenuInfo MenuInfo
		{
			get { return _menuInfo;}
		}

        /// <summary>
        /// Gets the toolbar button information
        /// </summary>
		public ToolBarInfo ToolBarInfo
		{ 
			get {return _toolBarInfo;}
		}

        /// <summary>
        /// Gets the shortcut bar item information
        /// </summary>
		public ShortcutInfo ShortcutInfo
		{
			get { return _shortcutInfo;}
		}

        /// <summary>
        /// Retrieves the current status of the menu item associated with the command
        /// </summary>
        /// <returns>The result status</returns>
		public virtual UIElementStatus QueryMenuItemStatus()
		{
			return UIElementStatus.Invisible;
		}

        /// <summary>
        /// Retrieves the current status of the toolbar item associated with the command
        /// </summary>
        /// <returns>The result status</returns>
		public virtual UIElementStatus QueryToolBarItemStatus()
		{
			return UIElementStatus.Invisible;
		}

        /// <summary>
        /// Retrieves the current status of the shortcut bar (or outlook bar item) item associated with the command
        /// </summary>
        /// <returns>The result status</returns>
		public virtual UIElementStatus QueryShortcutBarItemStatus()
		{
			return UIElementStatus.Invisible;
		}

		#endregion

		public BaseUICommand(IAppWorkspace appWorkspace, string command, 
            MenuInfo menuInfo, ToolBarInfo toolbarInfo, ShortcutInfo shortcutInfo) 
			: base (command)
		{
			this._appWorkspace = appWorkspace;
			this._menuInfo = menuInfo;
			this._toolBarInfo = toolbarInfo;
			this._shortcutInfo = shortcutInfo;
		}
		
	}
}
