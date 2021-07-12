using System;
using System.IO;
using System.Reflection;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

using Microsoft.Win32;

using System.Xml;
using System.Xml.Serialization;

using SIA.Plugins.Common;

namespace SIA.Plugins
{
    /// <summary>
    /// The PluginManager class implement the IPluginManager interface to provides
    /// functionality for managing plugin. This class is used by RDE
    /// </summary>
	public class PluginManager 
        : IPluginManager
	{
		#region constants
		private static string keySoftware = "SOFTWARE";
		private static string keyCompanyName = "SiGlaz";
		private static string keyProductName = "SiGlaz Image Analyzer";
		private static string keyPlugins = "Plugins";
		private static string keyID = "ID";
		private static string keyName = "Name";
		private static string keyDescription = "Description";
		private static string keyCategory = "Category";
		private static string keyCommand = "Command";
		private static string keyLocation = "Location";
		#endregion

		#region member fields
		public static string defaultConfigFile = "plugins.xml";
		private IAppWorkspace _appWorkspace = null;
		private PluginCollection _plugins = new PluginCollection();
		#endregion

		#region events

		public event EventHandler PluginLoaded = null;
		public event EventHandler PluginUnloaded = null;

		#endregion

		#region properties

		[XmlIgnore]
		public IAppWorkspace AppWorkspace
		{
			get {return _appWorkspace;}
		}

		[XmlIgnore]
		public PluginCollection Plugins 
		{
			get {return _plugins;}
		}

		
		#endregion

		#region Constructor and destructor
		
		static PluginManager()
		{
			if (Application.CompanyName != null && Application.CompanyName != string.Empty)
				keyCompanyName = Application.CompanyName;
			if (Application.ProductName != null && Application.ProductName != string.Empty)
				keyProductName = Application.ProductName;
		}

		public PluginManager()
		{
		}

		public PluginManager(IAppWorkspace appWorkspace)
		{
			_appWorkspace = appWorkspace;
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Load configuration from registry
		/// </summary>
		public void LoadConfiguration()
		{
			RegistryKey software = Registry.CurrentUser.OpenSubKey(keySoftware);
			if (software == null)
				return;
			RegistryKey siglaz = software.OpenSubKey(Application.CompanyName);
			if (siglaz == null)
				return;
			RegistryKey product = siglaz.OpenSubKey(Application.ProductName);
			if (product == null)
				return;
			RegistryKey plugins = product.OpenSubKey(keyPlugins);
			if (plugins == null)
				return;

			string id="", name="", description="", category="", command="", location="";
			string[] subKeys = plugins.GetSubKeyNames();
			for (int i=0; i<subKeys.Length; i++)
			{
				RegistryKey plugin = plugins.OpenSubKey(subKeys[i]);
				if (plugin == null)
					continue;
				if (plugin != null)
				{
					id = plugin.GetValue(keyID) as string;
					name = plugin.GetValue(keyName) as string;
					description = plugin.GetValue(keyDescription) as string;
					category = plugin.GetValue(keyCategory) as string;
					command = plugin.GetValue(keyCommand) as string;
					location = plugin.GetValue(keyLocation) as string;
					
					// load plugin
					location = this.GetAbsolutePath(location);
					if (File.Exists(location))
					{
						try
						{
							this.LoadPlugin(location, id);
						}
						catch (System.Exception e)
						{
							Trace.WriteLine("Failed to load plugin " + location + ": " + e.Message);
						}
					}
				}
			}
		}

		/// <summary>
		/// Save configuration to registry 
		/// </summary>
		public void SaveConfiguration()
		{
			RegistryKey software = Registry.CurrentUser.OpenSubKey(keySoftware);
			if (software == null)
				return;
			RegistryKey siglaz = software.OpenSubKey(keyCompanyName, true);
			if (siglaz == null)
				siglaz = software.CreateSubKey(keyCompanyName);
			RegistryKey product = siglaz.OpenSubKey(keyProductName, true);
			if (product == null)
				product = siglaz.CreateSubKey(keyProductName);
			RegistryKey pluginsKey = product.OpenSubKey(keyPlugins, true);
			if (pluginsKey != null)
				product.DeleteSubKeyTree(keyPlugins);
			pluginsKey = product.CreateSubKey(keyPlugins);
			
			foreach (IPlugin plugin in this._plugins)
			{
				RegistryKey pluginKey = pluginsKey.OpenSubKey(plugin.ID, true);
				if (pluginKey == null)
					pluginKey = pluginsKey.CreateSubKey(plugin.ID);
				pluginKey.SetValue(keyID, plugin.ID);
				pluginKey.SetValue(keyName, plugin.Name);
				pluginKey.SetValue(keyDescription, plugin.Description);
				//pluginKey.SetValue(keyCommand, plugin.Command);
				//pluginKey.SetValue(keyCategory, plugin.Category);
				pluginKey.SetValue(keyLocation, this.GetPluginLocation(plugin));
			}
		}

        /// <summary>
        /// Loads configuration from the specified file
        /// </summary>
        /// <param name="filename">The location of the file</param>
		public void LoadConfiguration(string filename)
		{
			if (File.Exists(filename))
			{
				// load plugin data from configuration file
				using (dsPlugins dataSet = new dsPlugins())
				{
					dataSet.ReadXml(filename);
					foreach(dsPlugins.PluginsRow row in dataSet.Plugins.Rows)
					{
						string id = row.ID;
						// convert from relative to absolute path
						string location = this.GetAbsolutePath(row.Location);
						if (File.Exists(location))
						{
							try
							{
								this.LoadPlugin(location, id);
							}
							catch (System.Exception e)
							{
								Trace.WriteLine("Failed to load plugin " + location + ": " + e.Message);
							}
						}
					}
				}

				// save to registry
				this.SaveConfiguration();
			}
			else // load configuration from registry
			{
				this.LoadConfiguration();
			}
		}


        /// <summary>
        /// Save the configuration to the specified file
        /// </summary>
        /// <param name="filename">The location of the target file</param>
		public void SaveConfiguration(string filename)
		{
			if (File.Exists(filename))
				File.Delete(filename);
			this.SaveConfiguration();
		}

		
        /// <summary>
        /// Loades a specified plugin form the specified file
        /// </summary>
        /// <param name="filename">The location of the source assembly</param>
        /// <param name="id">The id of the plugin to load</param>
        /// <returns>The list of loaded plugins</returns>
		public virtual PluginCollection LoadPlugin(string filename, string id)
		{
			PluginCollection result = new PluginCollection();
				
			try 
			{
				Assembly assembly = Assembly.LoadFile(filename);
				Type factoryType = typeof(IPluginFactory);
				Type[] types = assembly.GetExportedTypes();
				foreach (Type type in types)
				{
					if (factoryType.IsAssignableFrom(type))
					{
						// create an instance of plugin
						IPluginFactory factory = Activator.CreateInstance(type) as IPluginFactory;

						// load all plugin modules
						PluginCollection plugins = factory.CreatePlugins();
						if (plugins != null && plugins.Count > 0)
						{
							foreach (IPlugin plugin in plugins)
							{
								if (id != null && id != string.Empty && plugin.ID != id)
									continue;

								// add plugin into working space
								if (true == this.OnRegisterPlugin(plugin))
								{
									try
									{
										// save to result buffer
										result.Add(plugin);

										// raise plugin loaded
										this.RaisePluginLoaded(plugin);									
									}
									catch
									{
										// roll back plugin
										this.OnUnregisterPlugin(plugin);

										throw;
									}
								}
							}
						}
					}
				}
			} 
			catch
			{
				throw;
			}

			return result;
		}

        /// <summary>
        /// Unloads the specified plugin
        /// </summary>
        /// <param name="plugin"></param>
		public virtual void UnloadPlugin(IPlugin plugin)
		{
			// remove plugin out of working space
			this.OnUnregisterPlugin(plugin);

			// raise plugin unloaded
			this.RaisePluginUnloaded(plugin);
		}

        private void RaisePluginLoaded(IPlugin plugin)
		{
			if (this.PluginLoaded != null)
				this.PluginLoaded(this, new PluginLoadedEventArgs(plugin));
		}

		private void RaisePluginUnloaded(IPlugin plugin)
		{
			if (this.PluginUnloaded != null)
				this.PluginUnloaded(this, new PluginUnloadedEventArgs(plugin));
		}

		#endregion
		
		#region Overridable routines

        /// <summary>
        /// Registers the specified plugin
        /// </summary>
        /// <param name="plugin">The plugin to register</param>
        /// <returns>True if succeeded, otherwise false</returns>
		protected virtual bool OnRegisterPlugin(IPlugin plugin)
		{
			try
			{
				// check for duplicated plugin
				string id = plugin.ID;
				if (_plugins[id] != null)
				{
					string msg = string.Format("The plugin ID=\"{0}\" is duplicated or already loaded. The existed plugin ID=\"{0}\" Name={1} Description={2}",
						id, _plugins[id].Name, _plugins[id].Description);
					throw new Exception(msg);
				}

				// initialize plugin
				plugin.Initialize(this);
				
				// add plugin into plugin collection
				this._plugins.Add(plugin);

				return true;
			}
			catch (System.Exception e)
			{
				Trace.WriteLine("Failed to load plugin " + plugin.Name + ": " + e.Message);

				return false;
			}
		}

        /// <summary>
        /// Unregister the specified plugin
        /// </summary>
        /// <param name="plugin">The plugin to unregister</param>
		protected virtual void OnUnregisterPlugin(IPlugin plugin)
		{
			if (this._plugins.Contains(plugin) == false)
				throw new ArgumentOutOfRangeException("plugin", "This plugin was not created by this manager");
				
			try
			{
				// uninitialize plugin
				plugin.Unitialize();
				
				// remove plugin out of plugin collection
				this._plugins.Remove(plugin);
			}
			catch (System.Exception e)
			{
				Trace.WriteLine("Cannot remove plugin " + plugin.Name + ": " + e.Message);
			}
		}

		
		protected virtual string GetPluginLocation(IPlugin plugin)
		{
			string workingFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string location = plugin.GetType().Assembly.Location;
			if (location.IndexOf(workingFolder) >= 0)
				return location.Replace(workingFolder, ".");
			return location;
		}

		protected virtual string GetAbsolutePath(string path)
		{
			string str = path;
			if (str.IndexOf(".\\") == 0)
			{
				string workingFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
				return str.Replace(".\\", workingFolder + "\\");
			}

			return str;
		}

		#endregion
	}
}
