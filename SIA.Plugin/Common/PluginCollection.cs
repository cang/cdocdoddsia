using System;
using System.IO;
using System.Reflection;
using System.Data;
using System.Collections;
using System.Diagnostics;

using System.Xml;
using System.Xml.Serialization;

namespace SIA.Plugins.Common
{
    /// <summary>
    /// Encapsulates the collection of plugins
    /// </summary>
	public class PluginCollection 
        : CollectionBase	
	{
		public int Add(IPlugin plugin)
		{
			return base.List.Add(plugin);
		}

		public void Remove(IPlugin plugin)
		{
			base.List.Remove(plugin);
		}

		public bool Contains(IPlugin plugin)
		{
			return base.InnerList.Contains(plugin);
		}

		public IPlugin this[int index]
		{
			get {return (IPlugin)base.List[index];}
		}

		public IPlugin this[string ID]
		{
			get 
			{
				foreach (IPlugin plugin in base.List)
					if (plugin.ID == ID)
						return plugin;
				return null;
			}
		}
	}
}
