using System;

using SIA.Plugins.Common;

namespace SIA.Plugins
{
	/// <summary>
	/// Provides data for PluginUnloaded event
	/// </summary> 
	public class PluginUnloadedEventArgs : System.EventArgs
	{
		private IPlugin _plugin = null;

        /// <summary>
        /// Gets the unloaded plugin
        /// </summary>
		public IPlugin Plugin
		{
			get {return _plugin;}
		}

		public PluginUnloadedEventArgs(IPlugin plugin)
		{
			_plugin = plugin;
		}
	}
}
