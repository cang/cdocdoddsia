using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;

using SIA.Plugins;
using SIA.Plugins.Common;

namespace SIA.UI
{
	/// <summary>
	/// Summary description for CommandList.
	/// </summary>
	public class CommandList : DictionaryBase
	{
		public CommandList()
		{
		}

		public ICommandHandler[] CommandHandlers
		{
			get
			{
				ArrayList result = new ArrayList();
				lock (this.InnerHashtable.SyncRoot)
					result.AddRange(base.InnerHashtable.Values);
				return (ICommandHandler[])result.ToArray(typeof(ICommandHandler));
			}
		}

		public void Add(ICommandHandler cmdHandler)
		{
			lock (this.InnerHashtable.SyncRoot)
				this.InnerHashtable[cmdHandler.Command] = cmdHandler;
		}

		public void Remove(ICommandHandler cmdHandler)
		{
			lock (this.InnerHashtable.SyncRoot)
				this.InnerHashtable.Remove(cmdHandler.Command);
		}

		public bool Contains(string cmd)
		{
			return this.InnerHashtable.ContainsKey(cmd);
		}

		public ICommandHandler this[string key]
		{
			get 
			{
				return this.InnerHashtable[key] as ICommandHandler;
			}
			set
			{
				this.InnerHashtable[key] = value;
			}
		}
	}
}
