using System;
using System.Collections.Generic;
using System.Text;

namespace SIA.Plugins.Common
{
    /// <summary>
    /// The IAutomationWorkspace interface contains the functionality of RDE-Monitor workspace
    /// </summary>
    public interface IAutomationWorkspace 
        : IDisposable
    {
        /// <summary>
        /// Gets the script to execute
        /// </summary>
        IScript Script { get;}

        /// <summary>
        /// Add the specied key into the workspace. The key is used for script validation 
        /// </summary>
        /// <param name="key">The key to add</param>
        void AddKey(string key);

        /// <summary>
        /// Remove the specified key out of the workspace
        /// </summary>
        /// <param name="key">The key to remove</param>
        void RemoveKey(string key);

        /// <summary>
        /// Checks whether the workspace contains the specified key
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <returns>True if the key is contained within the workspace otherwise false</returns>
        bool HasKey(string key);

        /// <summary>
        /// Clears all keys
        /// </summary>
        void ClearKeys();
    }
}
