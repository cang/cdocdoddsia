using System;

namespace SIA.UI.Controls.Automation
{
	public delegate void ExecuteCallback(System.Object sender, ScriptCallbackArgs args);
	public delegate void ExceptionCallback(System.Object sender, ExceptionArgs args);

	/// <summary>
	/// Provides data for ExecuteCallback event
	/// </summary>
	public class ScriptCallbackArgs : System.EventArgs
	{
		private string _message = "";
		private int _percentage = 0;
		private bool _cancel = false;

		public string Message
		{
			get {return _message;}
		}

		public int Percentage
		{
			get {return _percentage;}
		}

		public bool Cancel
		{
			get {return _cancel;}
			set {_cancel = value;}
		}

		public ScriptCallbackArgs(string message, int percentage) : base()
		{
			_message = message;
			_percentage = percentage;			
		}
	}

	public class ExceptionArgs : System.EventArgs
	{
		private Exception _exp;

		public Exception Exception
		{
			get {return _exp;}	
		}

		public ExceptionArgs(Exception exp)
		{
			_exp = exp;
		}
	}
}
