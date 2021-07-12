using System;
using System.Data;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Serialization;

using SIA.Common;
using SIA.Plugins.Common;

using SIA.UI.Controls;
using SIA.UI.Controls.Common;
using SIA.UI.Controls.Commands;

namespace SIA.UI.Controls.Automation
{
	/// <summary>
	/// Represents the collection of the process step.
	/// </summary>
	public class ProcessStepCollection 
        : CollectionBase, ICloneable
	{
		#region Constructors and Destructors
		public ProcessStepCollection() : base()
		{
		}

		public ProcessStepCollection(int capacity) : base()
		{
			this.InnerList.Capacity = capacity;
		}
		#endregion

		#region Methods

		public int Add(ProcessStep command)
		{
			return base.List.Add(command);
		}

		public void AddRange(ICollection commands)
		{
			base.InnerList.AddRange(commands);
		}

		public IProcessStep[] ToArray()
		{
			return (IProcessStep[])base.InnerList.ToArray(typeof(IProcessStep));
		}

		public ProcessStep this[int index]
		{
			get {return (ProcessStep)base.List[index];}
			set {base.List[index] = value;}
		}

		#endregion

		#region ICloneable Members

		public object Clone()
		{
			ProcessStepCollection cloneObj = new ProcessStepCollection(this.InnerList.Capacity);
			cloneObj.AddRange(base.InnerList);
			return cloneObj;
		}

		#endregion
	}
}
