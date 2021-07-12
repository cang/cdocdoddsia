using System;
using System.Collections.Generic;
using System.Text;

namespace SIA.Plugins.Common
{
    /// <summary>
    /// The IPluginManager interface provides functionality for managing plugins
    /// </summary>
    public interface IPluginManager
    {
        /// <summary>
        /// Gets the application workspace
        /// </summary>
        IAppWorkspace AppWorkspace { get;}

        /// <summary>
        /// Gets the plugins managed by this plugin manager
        /// </summary>
        PluginCollection Plugins { get;}

        /// <summary>
        /// Raised when plugin is loaded
        /// </summary>
        event EventHandler PluginLoaded;

        /// <summary>
        /// Raised when plugin is unloaded
        /// </summary>
        event EventHandler PluginUnloaded;

        /// <summary>
        /// Loads the plugins from the assembly at the specified location
        /// </summary>
        /// <param name="fileName">The location of the assembly</param>
        /// <param name="id">The id of the plugin</param>
        /// <returns>The list of plugins is loaded</returns>
        PluginCollection LoadPlugin(string fileName, string id);

        /// <summary>
        /// Unloads the specified plugin
        /// </summary>
        /// <param name="plugin">The plugin to unload</param>
        void UnloadPlugin(IPlugin plugin);
    }

}
