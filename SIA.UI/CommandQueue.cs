using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;

namespace SIA.UI
{
	public class CommandQueueItem
	{
		public string Command;
		public object[] Arguments;

		public CommandQueueItem(string command, object[] args)
		{
			this.Command = command;
			this.Arguments = args;
		}

		public CommandQueueItem(string command)
		{
			this.Command = command;
			this.Arguments = null;
		}
	};

	/// <summary>
	/// Summary description for CommandQueue
	/// </summary>
	public class CommandQueue 
	{
		private Queue _cmdQueue = new Queue();

		public int Count
		{
			get {return _cmdQueue.Count;}
		}

		public object SyncRoot
		{
			get {return _cmdQueue.SyncRoot;}
		}
				
		public void Enqueue(string command, params object[] args)
		{
			_cmdQueue.Enqueue (new CommandQueueItem(command, args));
		}

		public CommandQueueItem Dequeue()
		{
			return _cmdQueue.Dequeue() as CommandQueueItem;
		}
		
	};
}
