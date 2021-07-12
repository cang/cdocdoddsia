using System;
using System.IO;

namespace SIA.UI.Controls.Automation.Commands
{
	/// <summary>
	/// Summary description for LaunchExternalApplicationCommand.
	/// </summary>
	public class LaunchExternalApplicationCommandSettings : AutoCommandSettings
	{
		#region member fields
		
		private string _fileName;
		private string _workingDir;
		private string _arguments;
		private bool _waitForExit;

		#endregion member fields

		#region Properties

		public string FileName
		{
			get {return _fileName;}
			set {_fileName = value;}
		}

		public string WorkingDirectory
		{
			get {return _workingDir;}
			set {_workingDir = value;}
		}

		public string Arguments
		{
			get {return _arguments;}
			set {_arguments = value;}
		}

		public bool WaitForExit
		{
			get {return _waitForExit;}
			set {_waitForExit = value;}
		}

		#endregion Properties

		#region Constructors ans Deconstructors
		
		public LaunchExternalApplicationCommandSettings()
		{			
		}

		#endregion Constructors ans Deconstructors

		#region Methods

		public override void Validate()
		{	
			if (this._fileName == null || this._fileName == "")
				throw new ArgumentException("Invalid application file.");
			if (!File.Exists(this._fileName))
				throw new FileNotFoundException("Invalid application file or not exist.", _fileName);
		}			
		
		#endregion Methods

		#region Serializable && Deserialize		
		#endregion Serializable && Deserialize				
	}
}
