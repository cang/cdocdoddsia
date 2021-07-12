using System;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;

namespace SIA.UI.Controls.Automation
{
	/// <summary>
	/// Factory class used for creating script
	/// </summary>
	public class ScriptFactory
	{
		#region Member Fields
		/// <summary>
		/// Initialized identify flag
		/// </summary>
		private static bool _initialized = false;

		/// <summary>
		/// Default version for script creation
		/// </summary>
		public static readonly int defaultVersion = 1;

		#endregion

		#region Constructor and destructor

		static ScriptFactory()
		{			
		}

		#endregion

		#region Static Methods

		public static void Initialize()
		{
			if (_initialized == false)
			{
				// clear all process steps
				ProcessStepManager.ClearAllSteps();

				// load built-in process steps
				ProcessStepManager.LoadBuiltInProcessSteps();
			
				// retrieve plugins configuration file
				string fileName = GetConfigFile();
			
				// load external process step from configuration file
				if (fileName != null && File.Exists(fileName)) 
					ProcessStepManager.LoadExtendProcessSteps(fileName);
				else // load external process steps from configuration file
					ProcessStepManager.LoadExtendProcessSteps();

				// initialize script serializer
				ScriptSerializer.Initialize();

				// signal initialized flag
				_initialized = true;
			}
		}

		public static void Uninitialize()
		{
			if (_initialized)
			{
				// uninitialize script serializer
				ScriptSerializer.Uninitialize();

				// save loaded process steps to registry
				ProcessStepManager.SaveExtendProcessSteps();
			
				// unsignal intialized flag
				_initialized = false;
			}
		}

		/// <summary>
		/// Creates an instance of the Script Factory by the default version
		/// </summary>
		/// <returns>The instance of script factory</returns>
		public static Script CreateInstance()
		{
			if (!_initialized)
				ScriptFactory.Initialize();

			return ScriptFactory.CreateInstance(defaultVersion);
		}

		/// <summary>
		/// Creates an instance of the Script Factory by the specified version
		/// </summary>
		/// <param name="version">Version of the script</param>
		/// <returns>The instance of script factory</returns>
		public static Script CreateInstance(int version)
		{
			switch (version)
			{
				case 1:
					return new Script();
				case 2:
				default:
					return new ScriptV2();
			}
		}

		private static string GetConfigFile()
		{
			string execFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string filename = ConfigurationManager.AppSettings["PluginCfg"];
			if (filename != null && File.Exists(filename))
				return filename;

			string configFile = "SiGlaz.RDE.Ex.UI.ApplicationEntry.exe.config";
			string fullPath = String.Format("{0}\\{1}", execFolder, configFile);
			if (File.Exists(fullPath))
			{
				try
				{
					// load configuration file using XmlDocument
					XmlDocument document = new XmlDocument();
					document.Load(fullPath);

					// retrieve appSettings node
					XmlNode node =  document.SelectSingleNode("//appSettings");
					if (node == null)
						throw new InvalidOperationException("appSettings section not found in config file.");
					
					// select the 'add' element that contains the key
					XmlElement elem = (XmlElement)node.SelectSingleNode("//add[@key=\"PluginCfg\"]");

					if (elem != null)
					{
						// add value for key
						string value = elem.GetAttribute("value");
						if (Path.IsPathRooted(value) == false)
							return String.Format("{0}\\{1}", execFolder, value);
						return value;
					}
				}
				catch (System.Exception e)
				{
					Trace.WriteLine(e.ToString());

					return string.Empty;
				}				
			}

			return string.Empty;
		}

		#endregion
	}
}

