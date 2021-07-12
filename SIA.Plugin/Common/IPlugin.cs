using System;
using System.IO;
using System.Reflection;
using System.Data;
using System.Collections;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;


namespace SIA.Plugins.Common
{
    /// <summary>
    /// A plugin contains 1 or more command handlers. Plugin is stored in registry.
    /// </summary>
	public interface IPlugin
	{
        /// <summary>
        /// Gets ID of the plugin
        /// </summary>
		string ID {get;}
        /// <summary>
        /// Gets the name of the plugin
        /// </summary>
		string Name {get;}
        /// <summary>
        /// Gets the short description of the plugin
        /// </summary>
		string Description {get;}

        /// <summary>
        /// Gets the list of command handler contained by this plugin
        /// </summary>
		ICommandHandler[] CommandHandlers {get;}

        /// <summary>
        /// Initialize the plugin. This function is called when
        /// </summary>
        /// <param name="mgr">The plugin manager</param>
		void Initialize(IPluginManager mgr);

        /// <summary>
        /// Uninitialize the plugin
        /// </summary>
		void Unitialize();
	}
	
}
