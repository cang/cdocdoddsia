using System;
using System.Diagnostics;
using System.Data;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Text;

using SIA.Common;
using SIA.UI.Controls;
using SIA.UI.Controls.Common;
using SIA.UI.Controls.Commands;

using SIA.UI.Controls.Automation;
using SIA.UI.Controls.Automation.Dialogs;

using SIA.Plugins.Common;

namespace SIA.UI.Controls.Automation
{
	/// <summary>
	/// The ProcessStep class is the basic element in RDE Monitor script.
	/// </summary>
	[Serializable]
	public abstract class ProcessStep 
        : IDisposable, ICloneable, ISerializable, IProcessStep
	{
		internal static readonly System.Text.Encoding DefaultEncoding = null;

		#region Member Fields
		
		protected int _index;
		protected string _id = Guid.NewGuid().ToString();
		protected string _name = string.Empty;
		protected string _displayName = string.Empty;
		protected string _description = string.Empty;
		protected bool _removable = true;
		protected bool _enabled = true;
		protected bool _hasSettings = true; // flags specified whether this step has settings or not
		protected bool _hasCustomSettingsDialog = true; // flags specified whether this step has custom setting dialog or not
		protected string[] _inputKeys = null; // pre-condition validation key for script validation
		protected string[] _outputKeys = null; // post-condition validation key for script validation
		protected Type _commandType; // type of command 
		protected Type _settingsType; // type of command settings

		protected RasterCommandSettings _settings; // run time settings value		

		#endregion

		#region Properties

        /// <summary>
        /// Gets the ID value
        /// </summary>
		public string ID
		{
			get {return _id;}
		}

        /// <summary>
        /// Gets the name of the step
        /// </summary>
		public string Name 
		{
			get {return _name;}
		}

        /// <summary>
        /// Gets the display name of the step
        /// </summary>
		public string DisplayName 
		{
			get {return _displayName;}
		}

        /// <summary>
        /// Gets the description of the step
        /// </summary>
		public string Description 
		{
			get {return _description;}
		}

        /// <summary>
        /// Gets boolean value indicates whether the process step is removable
        /// </summary>
		public bool Removable
		{
			get {return _removable;}
		}

        /// <summary>
        /// Gets the boolean value indicates whether the process step is enabled
        /// </summary>
		public bool Enabled
		{
			get {return _enabled;}
		}

        /// <summary>
        /// Gets the boolean value indicates whether the process step has stetings
        /// </summary>
		public bool HasSettings
		{
			get {return _hasSettings;}
		}

        /// <summary>
        /// Gets the boolean value indicates whether the process step has customized stetings window
        /// </summary>
		public bool HasSettingsDialog
		{
			get {return _hasCustomSettingsDialog;}
		}

        /// <summary>
        /// Gets the list of required keys for script validation
        /// </summary>
		public string[] InputKeys
		{
			get {return _inputKeys;}
		}

        /// <summary>
        /// Gets the list of output keys for script validation
        /// </summary>
		public string[] OutputKeys
		{
			get {return _outputKeys;}
		}

        /// <summary>
        /// Gets the index of the process step within the script
        /// </summary>
		public int Index
		{
			get {return _index;}
			set {_index = value;}
		}

        /// <summary>
        /// Gets or sets the settings associated with this process step
        /// </summary>
		[XmlIgnore]
		public virtual IRasterCommandSettings Settings
		{
			get {return _settings;}
			set 
			{
				_settings = value as RasterCommandSettings;
				OnSettingsChanged();
			}
		}

		protected virtual void OnSettingsChanged()
		{
		}

		#endregion

		#region Constructor and Destructor

		static ProcessStep()
		{
			DefaultEncoding = Encoding.UTF8;
		}

		public ProcessStep(string id, string displayName, string description, 
			Type commandType, Type settingsType)
		{
			this.InitClass(id, displayName, description, commandType, settingsType, null, null);
		}

		public ProcessStep(string id, string displayName, string description, 
			Type commandType, Type settingsType, string inputKey, string outputKey)
		{
			this.InitClass(id, displayName, description, commandType, settingsType, 
				inputKey == null ? null : new string[] {inputKey}, outputKey == null ? null : new string[] {outputKey});
		}

		public ProcessStep(string id, string displayName, string description, 
			Type commandType, Type settingsType, string[] inputKeys, string[] outputKeys)
		{
			this.InitClass(id, displayName, description, commandType, settingsType, inputKeys, outputKeys);
		}

		~ProcessStep()
		{
			this.Dispose(false);
		}

		private void InitClass(string id, string displayName, string description, 
			Type commandType, Type settingsType, string[] inputKeys, string[] outputKeys)
		{
			_id = id;
			_name = this.GetType().FullName;
			_displayName = displayName;
			_description = description;
			_commandType = commandType;
			_settingsType = settingsType;
			_inputKeys = inputKeys;
			_outputKeys = outputKeys;

			_settings = null;

			_index = 0;
			_hasSettings = _settingsType != null;
			_hasCustomSettingsDialog = false;
		}

		private void UninitClass()
		{

		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
		}

		#endregion

		#region ICloneable Members

		public virtual void Copy(ProcessStep command)
		{
			if (command == null)
				throw new ArgumentNullException("command", "command was not set to a reference");
			
			this._index = command._index;
			this._id = command._id;
			this._name = command._name;
			this._displayName = command._displayName;
			this._description = command._description;
			this._removable = command._removable;
			this._enabled = command._enabled;
			this._hasSettings = command._hasSettings;
			this._hasCustomSettingsDialog = command._hasCustomSettingsDialog;
			
			if (command._inputKeys != null)
				this._inputKeys = command._inputKeys.Clone() as string[];
			
			if (command._outputKeys != null)
				this._outputKeys = command._outputKeys.Clone() as string[];

			this._commandType = command._commandType;
			this._settingsType = command._settingsType;

			if (command._settings != null)
				this._settings = (RasterCommandSettings)command._settings.Clone();
			else
				this._settings = null;
		}

		public object Clone()
		{
			return this.MemberwiseClone();
		}

		#endregion

		#region ISerializable Members

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			
		}

		#endregion

		#region Methods

        /// <summary>
        /// Validates internal process step data
        /// </summary>
		public virtual void Validate()
		{
			if (this._hasSettings)
			{
				if (this._settings == null)
					throw new ArgumentNullException("Settings", "Settings was not set.");
	
				// validates settings
				this._settings.Validate();
			}
		}

        /// <summary>
        /// Validates the process step within the execution working space
        /// </summary>
        /// <param name="workspace"></param>
		public virtual void Validate(IAutomationWorkspace workspace)
		{
			this.Validate();

			if (this.InputKeys != null && this.InputKeys.Length > 0)
			{
				foreach (string key in this.InputKeys)
				{
					if (key == null || key == string.Empty)
						continue;
					
					if (!workspace.HasKey(key))
					{
						string msg = string.Empty;
						switch (key)
						{
							case ValidationKeys.SingleImage:
								msg = String.Format("Image was not loaded. Please load the image before using the \"{0}\" step.", this._displayName);
								break;
							case ValidationKeys.Waferboundary:
								msg = String.Format("WaferBoundary was not loaded. Please load or detect the wafer boundary before using the \"{0}\" step.", this._displayName);
								break;
							default:
								msg = String.Format("{0} was not specified.", key);
								break;
						}

						throw new ArgumentException(msg);
					}
				}
			}
		}

        /// <summary>
        /// Displays the settings window
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
		public virtual bool ShowSettingsDialog(IWin32Window owner)
		{
			throw new InvalidOperationException("Invalid show settings dialog");
		}

        /// <summary>
        /// Loads settings from the specified file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
		public virtual IRasterCommandSettings LoadAutoCommandSettings(string fileName)
		{
			if (this._settingsType == null)
				return null;

			RasterCommandSettings settings = null;

			try
			{
				using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					// load raster command settings by specified type of step
					Type propType = this._settingsType;
					settings = RasterCommandSettingsSerializer.Deserialize(stream, propType);
				}
			}
			catch 
			{
				throw;
			}

			return settings;
		}

        /// <summary>
        /// Serializes this process step into xml stream
        /// </summary>
        /// <param name="stream"></param>
		public virtual void Serialize(Stream stream)
		{
			if (stream == null)
				throw new ArgumentNullException("stream", "Stream was not set to a reference.");

			using (StreamWriter writer = new StreamWriter(stream, ProcessStep.DefaultEncoding))
			{
				XmlSerializerEx serializer = new XmlSerializerEx(this.GetType());
				serializer.Serialize(writer, this);
			}
		}

        /// <summary>
        /// Deserialize the process step data from xml stream
        /// </summary>
        /// <param name="stream"></param>
		public virtual void Deserialize(Stream stream)
		{
			if (stream == null)
				throw new ArgumentNullException("stream", "Stream was not set to a reference.");

			using (StreamReader reader = new StreamReader(stream, ProcessStep.DefaultEncoding))
			{
				XmlSerializerEx serializer = new XmlSerializerEx(this.GetType());
				ProcessStep command = (ProcessStep)serializer.Deserialize(reader);
				this.Copy(command);
			}
		}

		#endregion		

		#region Abstract methods

        /// <summary>
        /// Executes the process step in the specified working space
        /// </summary>
        /// <param name="workingSpace"></param>
		public abstract void Execute(WorkingSpace workingSpace);
		
        #endregion

	}

    /// <summary>
    /// The ProcessStep2 class does not save the settings within the RDE Monitor script. 
    /// This class only contains the location to the settings file.
    /// </summary>
	public abstract class ProcessStep2 
        : ProcessStep
	{
		protected string  _settingsFilePath = string.Empty;

		#region Properties

        /// <summary>
        /// Gets or sets the location of the settings file
        /// </summary>
		public string SettingsFilePath
		{
			get 
			{
				return _settingsFilePath;
			}
			set 
			{
				this._settingsFilePath = value;
				this.OnSettingsFilePathChanged();
			}
		}

		protected virtual void OnSettingsFilePathChanged()
		{
			if (this._settingsFilePath != null && File.Exists(_settingsFilePath))
				this._settings = RasterCommandSettingsSerializer.Deserialize(_settingsFilePath, this._settingsType);
			else
				this._settings = null;
		}

		[XmlIgnore]
		public AutoCommandSettings AutoCommandSettings
		{
			get {return _settings as AutoCommandSettings;}
			set
			{
				this._settings = value;
			}
		}

		#endregion

		#region Constructor and Destructor

		public ProcessStep2(string id, string displayName, string description, 
			Type commandType, Type settingsType)
			: base (id, displayName, description, commandType, settingsType)
		{
		}

		public ProcessStep2(string id, string displayName, string description, 
			Type commandType, Type settingsType, string inputKey, string outputKey)
			: base (id, displayName, description, commandType, settingsType, inputKey, outputKey)
		{
		}

		public ProcessStep2(string id, string displayName, string description, 
			Type commandType, Type settingsType, string[] inputKeys, string[] outputKeys)
			: base (id, displayName, description, commandType, settingsType, inputKeys, outputKeys)
		{
		}

		#endregion

		#region Override Routines

		public override bool ShowSettingsDialog(IWin32Window owner)
		{
			Type settingsType = this._settingsType;
			if (settingsType == null)
				throw new System.Exception("Unknown settings type of step " + this.DisplayName + ".");
			
			string fileName = this._settingsFilePath;

			using (DlgLoadExtAutoCommandSettings dlg = new DlgLoadExtAutoCommandSettings(fileName, this._displayName, settingsType))
			{
				if (dlg.ShowDialog(owner) == DialogResult.OK)
				{
					string filePath = dlg.FilePath;
					if (filePath != null && filePath.Length > 0)
					{
						IRasterCommandSettings newSettings = this.LoadAutoCommandSettings(filePath);
						this._settings = newSettings as RasterCommandSettings;
						this._settingsFilePath = filePath;
						return true;
					}
					else
					{
						throw new System.Exception("File path is not set!");
					}
				}
			}

			return false;
		}

		#endregion
	}

    /// <summary>
    /// The ProcessStep3 class provides information about categorizing of the steps.
    /// </summary>
    public abstract class ProcessStep3 
        : ProcessStep2, IProcessStep2
    {
        private string _category = string.Empty;

        /// <summary>
        /// Gets the category of the process step
        /// </summary>
        public string Category
        {
            get { return _category;}
        }

        #region Constructor and Destructor

		public ProcessStep3(string id, string displayName, string description, string category,
			Type commandType, Type settingsType)
			: base (id, displayName, description, commandType, settingsType)
		{
            this._category = category;
		}

        public ProcessStep3(string id, string displayName, string description, string category, 
			Type commandType, Type settingsType, string inputKey, string outputKey)
			: base (id, displayName, description, commandType, settingsType, inputKey, outputKey)
		{
            this._category = category;
		}

        public ProcessStep3(string id, string displayName, string description, string category,
			Type commandType, Type settingsType, string[] inputKeys, string[] outputKeys)
			: base (id, displayName, description, commandType, settingsType, inputKeys, outputKeys)
		{
            this._category = category;
		}

		#endregion
    }
}
