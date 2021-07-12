using System;
using System.Data;
using System.Collections;
using System.ComponentModel;

namespace SIA.UI.Controls.Common
{
	/// <summary>
	/// Summary description for DataItem.
	/// </summary>
	public class DataItem : IComparable
	{
		#region Fields
		private string _displayMember = "";
		private object _value = null;
		#endregion

		#region Properties
		
		public string DisplayMember
		{
			get {return _displayMember;}
			set {_displayMember = value;}
		}

		public object ValueMember
		{
			get {return _value;}
			set {_value = value;}
		}

		#endregion

		#region Constructor and Destructor

		public DataItem()
		{
		}

		public DataItem(string name, object value)
		{
			_displayMember = name;
			_value = value;
		}

		#endregion

		#region IComparable Members

		public int CompareTo(object obj)
		{
			DataItem item = (DataItem)obj;
			return String.Compare(this.DisplayMember, item.DisplayMember);
		}

		#endregion
	}

	public class DataItemCollection : CollectionBase
	{
		#region Methods
		public int Add(string name, object value)
		{
			return base.List.Add(new DataItem(name, value));
		}

		public int Add(DataItem item)
		{
			return base.List.Add(item);
		}

		public DataItem this[int index]
		{
			get {return (DataItem)base.List[index];}
		}
		#endregion

		public void Sort(IComparer comparer)
		{
			base.InnerList.Sort(comparer);
		}
	}

	public class DisplayMemberComparer : IComparer
	{
		public int Compare(object obj1, object obj2)
		{
			DataItem item1 = (DataItem)obj1;
			DataItem item2 = (DataItem)obj2;
			return String.Compare(item1.DisplayMember, item2.DisplayMember);
		}
	}
}
