using System;
using System.Reflection;
using System.Data;
using System.Collections;
using System.IO;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.Serialization;
using Microsoft.Win32;

using SIA.UI.Controls.Automation;
using SIA.UI.Controls.Automation.Steps;

using SIA.Plugins;
using SIA.Plugins.Common;
using System.Windows.Forms;

namespace SIA.UI.Controls.Automation
{
    /// <summary>
    /// The process step info is sometime duplicates the proces step internal data.
    /// This class is used by the process step manager to manage all the process step.
    /// </summary>
	[Serializable]
	public class ProcessStepInfo : IComparer
	{
		private string _id = Guid.NewGuid().ToString();
        private string _name = "Unspecified";
        private string _displayName = "Unspecified Step";
        private string _description = "Unspecified process step";
        private string _category = null;

		private bool _removable = false;
		private bool _enabled = false;
		private Type _type = null;
		private bool _isBuiltIn = false;

		public string ID
		{
			get {return _id;}
		}

		public string Name 
		{
			get {return _name;}
		}

		public string DisplayName
		{
			get {return _displayName;}
		}

		public string Description
		{
			get {return _description;}
		}

        public string Category
        {
            get { return _category; }
        }

		public Type Type
		{
			get {return _type;}
		}

		public bool Removable
		{
			get {return _removable;}
		}

		public bool Enabled
		{
			get {return _enabled;}
		}

		public bool IsBuiltIn
		{
			get {return _isBuiltIn;}
		}

		public ProcessStepInfo(IProcessStep step)
		{
			if (step == null)
				throw new ArgumentNullException("step");
			this._id = step.ID;
			this._name = step.Name;
			this._displayName = step.DisplayName;
			this._description = step.Description;
			this._type = step.GetType();
			this._removable = step.Removable;
			this._enabled = step.Enabled;
        
            if (step is IProcessStep2)
                this._category = ((IProcessStep2)step).Category;
        }

		internal ProcessStepInfo(IProcessStep step, bool isBuiltIn) : this(step)
		{
			this._isBuiltIn = isBuiltIn;
		}

        #region IComparer Members

        public int Compare(object x, object y)
        {
            return String.Compare(
                (x as ProcessStepInfo).DisplayName, 
                (y as ProcessStepInfo).DisplayName);
        }

        #endregion
    };

	/// <summary>
	/// The ProcessStepManager class works as plugin manager provides functions for
    /// registering, unregistering custom process steps.
	/// </summary>
	public class ProcessStepManager
	{
        private const string keySoftware = "SOFTWARE";
        private static string keyCompanyName = Application.CompanyName;
        private static string keyProductName = Application.ProductName;
        
        #region constants
		
        private const string keyPlugins = "Plugins";
		private const string keyID = "ID";
		private const string keyName = "Name";
		private const string keyDescription = "Description";
		private const string keyCategory = "Category";
		private const string keyCommand = "Command";
		private const string keyLocation = "Location";
		
        #endregion

		private static Hashtable _registedSteps = null;

		static ProcessStepManager()
		{
			_registedSteps = new Hashtable();			
		}

		public static void ClearAllSteps()
		{
			_registedSteps.Clear();
		}

		public static void LoadBuiltInProcessSteps()
		{
			// scan and regists for built-in process steps
			Assembly assembly = Assembly.GetAssembly(typeof(ProcessStepManager));
			Type baseType = typeof(ProcessStep);
			Type[] types = assembly.GetExportedTypes();
			foreach (Type type in types)
			{
				if (type.IsAbstract == false && baseType.IsAssignableFrom(type))
				{
					ProcessStepInfo info = CreateProcessStepInfo(type, true);
					if (info != null)
						RegistProcessType(info);
				}
			}
		}

		/// <summary>
		/// Load external process step from registry
		/// </summary>
		public static void LoadExtendProcessSteps()
		{
			RegistryKey software = Registry.CurrentUser.OpenSubKey(keySoftware);
			if (software == null)
				return;
			RegistryKey company = software.OpenSubKey(keyCompanyName);
			if (company == null)
				return;
			RegistryKey product = company.OpenSubKey(keyProductName);
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
					location = GetAbsolutePath(location);
					if (File.Exists(location))
					{
						try
						{
							LoadProcessStep(location, id, true);
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
		/// Save loaded external process step to registry
		/// </summary>
		public static void SaveExtendProcessSteps()
		{
			RegistryKey software = Registry.CurrentUser.OpenSubKey(keySoftware, true);
			if (software == null)
				return;
			RegistryKey company = software.OpenSubKey(keyCompanyName, true);
			if (company == null)
				company = software.CreateSubKey(keyCompanyName);
			RegistryKey product = company.OpenSubKey(keyProductName, true);
			if (product == null)
				product = company.CreateSubKey(keyProductName);
			RegistryKey pluginsKey = product.OpenSubKey(keyPlugins, true);
			if (pluginsKey != null)
				product.DeleteSubKeyTree(keyPlugins);
            pluginsKey = product.CreateSubKey(keyPlugins);
			
			ProcessStepInfo[] steps = ProcessStepManager.GetRegistedProcessSteps();
			foreach (ProcessStepInfo step in steps)
			{
				if (step.IsBuiltIn)
					continue;

				RegistryKey pluginKey = pluginsKey.OpenSubKey(step.ID, true);
				if (pluginKey == null)
					pluginKey = pluginsKey.CreateSubKey(step.ID);
				pluginKey.SetValue(keyID, step.ID);
				pluginKey.SetValue(keyName, step.Name);
				pluginKey.SetValue(keyDescription, step.Description);
				pluginKey.SetValue(keyLocation, GetPluginLocation(step));
			}
		}

		/// <summary>
		/// Load external process step from plugin specified in the configuration file
		/// </summary>
		/// <remarks>
		/// This function is obsolete and no longer use
		/// </remarks>
		/// <param name="filename">The xml file contains plugin settings</param>
		public static void LoadExtendProcessSteps(string filename)
		{
			using (dsPlugins dataSet = new dsPlugins())
			{
				dataSet.ReadXml(filename);
				foreach(dsPlugins.PluginsRow row in dataSet.Plugins.Rows)
				{
					// convert from relative to absolute path
					string location = GetAbsolutePath(row.Location);
					if (File.Exists(location))
					{
						try
						{
							LoadProcessStep(location);
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
		///  Load the specified assembly and register the process steps if any
		/// </summary>
		/// <param name="filename">The path of the assembly contains the process steps</param>
		/// <returns>The list of registered process steps</returns>
		public static ArrayList LoadProcessStep(string filename)
		{
			return LoadProcessStep(filename, null, true);
		}
		
		/// <summary>
		/// Load the specified assembly and register the process steps if any
		/// </summary>
		/// <param name="filename">The path of the assembly contains the process steps</param>
		/// <param name="allowDuplicate">Allow duplicated step loaded</param>
		/// <returns>The list of registered process steps</returns>
		public static ArrayList LoadProcessStep(string filename, string id, bool allowDuplicate)
		{
			ArrayList result = new ArrayList();
				
			try 
			{
				Assembly assembly = Assembly.LoadFrom(filename);
				Type factoryType = typeof(IProcessStepFactory);
				Type[] types = assembly.GetExportedTypes();
				foreach (Type type in types)
				{
					if (factoryType.IsAssignableFrom(type))
					{
						// create an instance of process step factory
						IProcessStepFactory factory = Activator.CreateInstance(type) as IProcessStepFactory;

						if (id == null)
						{
							// load all process step provided by this modules
							ArrayList steps = factory.CreateSteps();
							if (steps != null && steps.Count > 0)
							{
								foreach (IProcessStep step in steps)
								{
									if (step == null)
										continue;

									if (step.Enabled)
									{
										// create process step info
										ProcessStepInfo info = CreateProcessStepInfo(step, false);

										// check for duplicate
										if (allowDuplicate == false)
										{
											string key = info.ID;
											if (_registedSteps[key] != null)
												continue;
										}

										// regists process step									
										RegistProcessType(info);
										// save for return
										result.Add(info);
									}
								}
							}
						}
						else // load the plugin specified by the id
						{
							IProcessStep step = factory.CreateStep(id);
							if (step != null && step.Enabled)
							{
								// create process step info
								ProcessStepInfo info = CreateProcessStepInfo(step, false);

								// check for duplicate
								if (allowDuplicate == false)
								{
									string key = info.ID;
									if (_registedSteps[key] != null)
										continue;
								}

								// regists process step									
								RegistProcessType(info);
								// save for return
								result.Add(info);
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

		public static ArrayList LoadProcessStepWithPrompt(string filename, string id)
		{
			ArrayList result = new ArrayList();
				
			try 
			{
				Assembly assembly = Assembly.LoadFrom(filename);
				Type factoryType = typeof(IProcessStepFactory);
				Type[] types = assembly.GetExportedTypes();
				foreach (Type type in types)
				{
					if (factoryType.IsAssignableFrom(type))
					{
						// create an instance of process step factory
						IProcessStepFactory factory = Activator.CreateInstance(type) as IProcessStepFactory;

						if (id == null)
						{
							// load all process step provided by this modules
							ArrayList steps = factory.CreateSteps();
							if (steps != null && steps.Count > 0)
							{
								foreach (IProcessStep step in steps)
								{
									if (step == null)
										continue;

									if (step.Enabled)
									{
										// create process step info
										ProcessStepInfo info = CreateProcessStepInfo(step, false);

										// check for duplicate
										string key = info.ID;
										if (_registedSteps[key] != null)
										{
											if (false == MessageBoxEx.ConfirmYesNo(String.Format("There is already another step with the same ID \"{0}\"."
												+ " Do you want to override the old step?", key))) 
												continue;
										}
										
										// regists process step									
										RegistProcessType(info);
										// save for return
										result.Add(info);
									}
								}
							}
						}
						else // load the plugin specified by the id
						{
							IProcessStep step = factory.CreateStep(id);
							if (step != null && step.Enabled)
							{
								// create process step info
								ProcessStepInfo info = CreateProcessStepInfo(step, false);

								// check for duplicate
								string key = info.ID;
								if (_registedSteps[key] != null)
								{
									if (MessageBoxEx.ConfirmYesNo("There is already another step with the same ID. Do you want to override the old step?")) 
									{
										// regists process step									
										RegistProcessType(info);
										// save for return
										result.Add(info);
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

		
		public static ProcessStepInfo CreateProcessStepInfo(Type type, bool isBuiltIn)
		{
			IProcessStep step = Activator.CreateInstance(type) as IProcessStep;
			if (step.Enabled)
				return CreateProcessStepInfo(step, isBuiltIn);
			return null;
		}

		public static ProcessStepInfo CreateProcessStepInfo(IProcessStep step, bool isBuiltIn)
		{
			return new ProcessStepInfo(step, isBuiltIn);
		}

		
		public static void RegistProcessType(ProcessStepInfo info)
		{
			if (info == null || info.Type == null)
				throw new ArgumentNullException("info");
			if (_registedSteps == null)
				throw new InvalidOperationException("Built-in process steps was not loaded. Please call LoadBuiltInProcessStep().");
			
			//bool isDuplicated = false;
			string key = info.ID;
			ProcessStepInfo existedInfo = _registedSteps[key] as ProcessStepInfo;
			if (existedInfo != null)
			{
				if (existedInfo.ID == info.ID)
					Trace.WriteLine("WARNING: There is already process step existed with the same ID. The old step will be overridden by the new step", info.Name);
			}

			//if (!isDuplicated)
			_registedSteps[key] = info;
		}

		public static void UnregistProcessType(ProcessStepInfo info)
		{
			if (info == null || info.Name == null)
				throw new ArgumentNullException("info");
			if (_registedSteps == null)
				throw new InvalidOperationException("Built-in process steps was not loaded. Please call LoadBuiltInProcessStep().");

			string key = info.ID;
			if (_registedSteps[key] != null)
			{
				_registedSteps[key] = null;
				_registedSteps.Remove(key);
			}
		}

		public static ProcessStepInfo GetRegistedProcessStep(string id)
		{
			if (_registedSteps == null)
				throw new InvalidOperationException("Built-in process steps was not loaded. Please call LoadBuiltInProcessStep().");
			return _registedSteps[id] as ProcessStepInfo;
		}

		public static ProcessStepInfo[] GetRegistedProcessSteps()
		{
			if (_registedSteps == null)
				throw new InvalidOperationException("Built-in process steps was not loaded. Please call LoadBuiltInProcessStep().");

			ArrayList result = new ArrayList();
			foreach (object key in _registedSteps.Keys)
			{
                if (_registedSteps[key] != null)
                {
                    if (_registedSteps[key] is ProcessStepInfo)
                        result.Add(_registedSteps[key]);
                }
			}            
			return (ProcessStepInfo[])result.ToArray(typeof(ProcessStepInfo));
		}

		public static Type[] GetTypeOfRegistedProcessSteps()
		{
			if (_registedSteps == null)
				throw new InvalidOperationException("Built-in process steps was not loaded. Please call LoadBuiltInProcessStep().");

			ArrayList result = new ArrayList();
			foreach (object key in _registedSteps.Keys)
			{
				ProcessStepInfo info = _registedSteps[key] as ProcessStepInfo;
				result.Add(info.Type);
			}

			return (Type[])result.ToArray(typeof(Type));
		}

		public static Type[] GetSettingsTypeOfRegistedProcessSteps()
		{
			if (_registedSteps == null)
				throw new InvalidOperationException("Built-in process steps was not loaded. Please call LoadBuiltInProcessStep().");

			ArrayList result = new ArrayList();
			foreach (object key in _registedSteps.Keys)
			{
				ProcessStepInfo info = _registedSteps[key] as ProcessStepInfo;
				Type type = info.Type;
				PropertyInfo[] props = type.GetProperties();
				foreach (PropertyInfo prop in props)
				{
					Type propType = prop.PropertyType;
					if (propType.IsSubclassOf(typeof(AutoCommandSettings)))
					{
						result.Add(propType);
						break;
					}
				}
			}

			return (Type[])result.ToArray(typeof(Type));
		}

		public static string GetPluginLocation(ProcessStepInfo stepInfo)
		{
			Type type = stepInfo.Type;
			string workingFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string location = type.Assembly.Location;
			if (location.IndexOf(workingFolder) >= 0)
				return location.Replace(workingFolder, ".");
			return location;
		}

		public static string GetAbsolutePath(string path)
		{
			string str = path;
			if (str.IndexOf(".\\") == 0)
			{
				string workingFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
				return str.Replace(".\\", workingFolder + "\\");
			}

			return str;
		}
	}
}
