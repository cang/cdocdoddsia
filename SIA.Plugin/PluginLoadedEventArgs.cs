using System;

using SIA.Plugins.Common;

namespace SIA.Plugins
{
	/// <summary>
	/// Provides data for PluginLoaded event
	/// </summary> 
	public class PluginLoadedEventArgs : System.EventArgs
	{
		private IPlugin _plugin = null;

        /// <summary>
        /// Gets the loaded plugin 
        /// </summary>
		public IPlugin Plugin
		{
			get {return _plugin;}
		}

		public PluginLoadedEventArgs(IPlugin plugin)
		{
			_plugin = plugin;
		}
	}
}
