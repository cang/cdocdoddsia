using System;
using System.Collections.Generic;
using System.Text;

namespace SIA.Plugins.Common
{
    /// <summary>
    /// Provides user interace functions for command execution
    /// </summary>
    public interface IUICommandHandler 
        : ICommandHandler
    {
        /// <summary>
        /// Gets the application workspace
        /// </summary>
        IAppWorkspace AppWorkspace { get;}

        /// <summary>
        /// Gets main menu information used for menu creation
        /// </summary>
        MenuInfo MenuInfo { get;}

        /// <summary>
        /// Gets toolbar information used for toolbar creation
        /// </summary>
        ToolBarInfo ToolBarInfo { get;}

        /// <summary>
        /// Gets shortcut bar information used for shortcut creation
        /// </summary>
        ShortcutInfo ShortcutInfo { get;}

        /// <summary>
        /// Gets menu item status
        /// </summary>
        /// <returns>Status of the menu item</returns>
        UIElementStatus QueryMenuItemStatus();

        /// <summary>
        /// Gets toolbar item status
        /// </summary>
        /// <returns>Status of the toolbar item</returns>
        UIElementStatus QueryToolBarItemStatus();

        /// <summary>
        /// Gets outlook bar item status
        /// </summary>
        /// <returns>Status of the shortcut item</returns>
        UIElementStatus QueryShortcutBarItemStatus();
    }
	
   
}
