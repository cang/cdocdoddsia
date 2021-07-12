using System;
using System.Windows.Forms;

namespace SIA.Automation.Launcher.Configuration
{
	/// <summary>
	/// Summary description for PathCreationEngineEventArgs.
	/// </summary>
	public class PathCreationEngineEventArgs : System.EventArgs 
	{
		private object _userData;
		private string _path;
		private DialogResult _result;
		private System.Exception _systemException;
				
		public PathCreationEngineEventArgs(string path)
		{			
			_path = path;
		}

		public PathCreationEngineEventArgs(string path, DialogResult result)
		{
			_path = path;
			_result = result;
		}

		public object UserData
		{
			get
			{
				return _userData;
			}
			set
			{
				_userData = value;
			}
		}

		public string Path
		{
			get
			{
				return _path;
			}
			set
			{
				_path = value;
			}
		}

		public DialogResult Result
		{
			get
			{
				return _result;
			}
			set
			{
				_result = value;
			}
		}

		public System.Exception Exception
		{
			get
			{
				return _systemException;
			}
			set
			{
				_systemException = value;
			}
		}
	}

	public delegate void PathCreationEngineEventHandler(object sender, PathCreationEngineEventArgs e);
}
