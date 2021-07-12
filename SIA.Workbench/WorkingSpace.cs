using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

using SIA.UI.Controls;
using SIA.UI.Controls.Automation;
using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.Utilities;

using SIA.Plugins.Common;
using SIA.Plugins;

namespace SIA.Workbench
{
	/// <summary>
	/// Summary description for WorkingSpace.
	/// </summary>
    internal class WorkingSpace : IAutomationWorkspace
	{
		#region Constants
		internal const string defaultTitle = "Unamed script";
		#endregion

		#region Member fields

		private Form _container = null;
		private Script _script = ScriptFactory.CreateInstance();
		private string _fileName = string.Empty;
		private bool _modified = false;
		private Hashtable _validationKeys = new Hashtable();

		public event EventHandler ModifiedChanged = null;
		#endregion Member fields
		
		#region Properties

		public Form Container
		{
			get {return _container;}
		}

		public Script Script
		{
			get
			{
				return _script;
			}
			set 
			{
				_script = value;
			}
		}

		public string FileName
		{
			get 
			{
				return _fileName;
			}
			set 
			{
				_fileName = value;
			}
		}

		public bool Modified
		{
			get 
			{
				return _modified;
			}
			set 
			{
				_modified = value;

				OnModifiedChanged();
			}
		}

		protected virtual void OnModifiedChanged()
		{
			if (this.ModifiedChanged != null)
				this.ModifiedChanged(this, EventArgs.Empty);
		}

		#endregion Properties

		#region Constructor and destructor

		public WorkingSpace(Form container)
		{
			_container = container;
		}

		public WorkingSpace(Form container, Script script, string fileName)
		{
			_container = container;
			_script = script;
			_fileName = fileName;
		}

		~WorkingSpace()
		{
			this.Dispose(false);
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
			if (_script != null)
				_script.Dispose();
			_script = null;
		}

		#endregion

		#region Methods

		public void AddKey(string[] keys)
		{
			foreach (string key in keys)
				this._validationKeys[key] = true;
		}

		public void AddKey(string key)
		{
			this._validationKeys[key] = true;
		}

		public void RemoveKey(string key)
		{
			this._validationKeys.Remove(key);
		}

		public bool HasKey(string key)
		{
			return this._validationKeys[key] != null;
		}

		public void ClearKeys()
		{
			this._validationKeys.Clear();
		}

		public void OpenScript(string fileName)
		{
			try
			{
				string scriptFileName = fileName;
				if (File.Exists(scriptFileName) == false)
				{
					MessageBoxEx.Error("The file \"{0}\" does not exist.", scriptFileName);
					return;
				}

				#region deserializes script specified by scriptFileName
				try
				{
					_script = Script.Deserialize(scriptFileName);

					// save file path
					_fileName = scriptFileName;

					// reset modified flag
					this.Modified = false;
				}
				catch
				{
					throw;
				}
				#endregion
			}
			catch (System.Exception exp) 
			{
				Trace.WriteLine(exp);

				throw;
			}
		}

		public void SaveScript()
		{
			this.SaveScript(_fileName);
		}

		public void SaveScript(string fileName)
		{
			try
			{
				string scriptFileName = fileName;
				
				#region serializes script specified by scriptFileName
				try
				{
					_script.Serialize(scriptFileName);

					// update file name
					_fileName = scriptFileName;

					// reset modified flag
					this.Modified = false;
				}
				catch 
				{
					throw;
				}
				#endregion
			}
			catch (System.Exception exp) 
			{
				Trace.WriteLine(exp);

				throw;
			}
		}

		public void ValidateScript()
		{
			try
			{
				_script.Validate();
			}
			catch (System.Exception exp) 
			{
				Trace.WriteLine(exp);

				throw;
			}
		}

		#endregion

		#region IAutomationWorkspace Members

		IScript SIA.Plugins.Common.IAutomationWorkspace.Script
		{
			get
			{
				return this._script;
			}
		}

		#endregion
	}
}
