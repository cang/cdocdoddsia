using System;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;

namespace SIA.Plugins.Common
{
	/// <summary>
	/// Provides base implementation for the plugin
	/// </summary>
	public abstract class BasePlugin 
        : IPlugin
	{
		#region Fields
		
		protected IPluginManager _pluginMgr = null;
		protected string _id = Guid.NewGuid().ToString();
		protected string _name = "BasePlugin2";
		protected string _description = "This is base class for plugin.";
		protected ICommandHandler[] _commandHandlers;

		#endregion

		#region IPlugin Members

        /// <summary>
        /// Gets the ID of the plugin
        /// </summary>
		public string ID 
		{
			get {return _id;}
		}
		
        /// <summary>
        /// Gets the name of the plugin
        /// </summary>
		public string Name
		{
			get { return this._name;}
		}

        /// <summary>
        /// Gets the short description of the plugin
        /// </summary>
		public string Description
		{
			get { return this._description;}
		}

        /// <summary>
        /// Gets the plugin manager for this plugin
        /// </summary>
		public IPluginManager PluginManager
		{
			get {return _pluginMgr;}
		}

        /// <summary>
        /// Gets the list of command handlers exposed by this plugin
        /// </summary>
		public ICommandHandler[] CommandHandlers
		{
			get 
			{
				if (_commandHandlers == null)
					this._commandHandlers = this.OnCreateCommandHandlers();
				return _commandHandlers;
			}
		}

        /// <summary>
        /// Initialize the plugin
        /// </summary>
        /// <param name="mgr">The plugin manager associated with the plugin</param>
		public virtual void Initialize(IPluginManager mgr)
		{
			if (mgr == null)
				throw new ArgumentNullException("mgr");

			_pluginMgr = mgr;
		}

        /// <summary>
        /// Uninitialize the plugin
        /// </summary>
		public virtual void Unitialize()
		{
			if (_pluginMgr == null)
				throw new ArgumentNullException("_pluginMgr");
		}

		#endregion

		public BasePlugin(string id, string name, string description)
		{
			this._id = id;
			this._name = name;
			this._description = description;
			this._commandHandlers = null;
		}

        /// <summary>
        /// Abstract function for creating command handlers
        /// </summary>
        /// <returns>The list of command handlers</returns>
		public abstract ICommandHandler[] OnCreateCommandHandlers();
	}
}
