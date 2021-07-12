using System;
using System.Collections.Generic;
using System.Text;

namespace SIA.Plugins.Common
{
    /// <summary>
    /// Provides basically functions for command execution
    /// </summary>
    public interface ICommandHandler
    {
        /// <summary>
        /// Gets the key of the command handler
        /// </summary>
        string Command { get;}

        /// <summary>
        /// Executes the command with the specified arguments
        /// </summary>
        /// <param name="args">The arguments for executing the command</param>
        void DoCommand(params object[] args);
    }
}
