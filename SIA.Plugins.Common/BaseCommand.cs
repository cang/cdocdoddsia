using System;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;

namespace SIA.Plugins.Common
{
	/// <summary>
	/// Provides a base implementation of a command object that exposes a command handler interface to its properties
	/// </summary>
	public abstract class BaseCommand 
        : ICommandHandler
	{
		#region Fields
		
		protected string _command = "";

		#endregion

		#region ICommand Members

        /// <summary>
        /// Gets the associated command string 
        /// </summary>
		public string Command
		{
			get { return _command;}
		}

        /// <summary>
        /// Executes with the specified arguments
        /// </summary>
        /// <param name="args">The arguments to process</param>
		public abstract void DoCommand(params object[] args);
		
		#endregion

		public BaseCommand(string command)
		{
			this._command = command;
		}
		
	}
}
