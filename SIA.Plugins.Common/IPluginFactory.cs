using System;
using System.Collections.Generic;
using System.Text;

namespace SIA.Plugins.Common
{
    /// <summary>
    /// Factory class used for enumerating the plugins
    /// </summary>
    public interface IPluginFactory
    {
        /// <summary>
        /// Retreives list of plugins exposed by this factory class.
        /// </summary>
        /// <returns></returns>
        PluginCollection CreatePlugins();
    }
}
