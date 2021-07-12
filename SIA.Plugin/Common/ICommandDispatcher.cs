using System;

namespace SIA.Plugins.Common
{
	/// <summary>
	/// The ICommandDispatcher interface provides functionality for command execution
	/// </summary>
	public interface ICommandDispatcher
	{
        /// <summary>
        /// Gets list of command handlers which are handled
        /// </summary>
		ICommandHandler[] Commands {get;}

        /// <summary>
        /// Places a command into the command queue and wait for the ICommandDispatcher
        /// to process the command
        /// </summary>
        /// <param name="command">The key of command handler to execute</param>
        /// <param name="args">The arguments for the command</param>
        void DispatchCommand(string command, params object[] args);

        /// <summary>
        /// Places a command into the command queue and returns without waiting for 
        /// the ICommandDispatcher to process the command. 
        /// </summary>
        /// <param name="command">The key of command handler to execute</param>
        /// <param name="args">The arguments for the command</param>
		void PostCommand(string command, params object[] args);
	}
}
