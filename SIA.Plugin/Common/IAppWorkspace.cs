using System;
using System.IO;
using System.Reflection;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using SiGlaz.UI.CustomControls;

namespace SIA.Plugins.Common
{
    /// <summary>
    /// The IappWorkspace interface represents the main window or the workspace of the application
    /// </summary>
	public interface IAppWorkspace 
        : IWin32Window, ICommandDispatcher
	{
        /// <summary>
        /// Gets the document workspace
        /// </summary>
		IDocWorkspace DocumentWorkspace {get;}

        /// <summary>
        /// Gets the plugin manager
        /// </summary>
		IPluginManager PluginManager {get;}

        /// <summary>
        /// Gets the main menu
        /// </summary>
		MainMenu MainMenu {get;}

        /// <summary>
        /// Gets the main toolbar
        /// </summary>
		ToolBarEx MainToolBar {get;}
	}

}
