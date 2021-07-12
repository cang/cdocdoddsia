using System;
using System.Collections;

namespace SiGlaz.UI.CustomControls
{
	/// <summary>
	/// Summary description for ExplorerItemCollection.
	/// </summary>
	public class ExplorerItemCollection : CollectionBase
	{
		public ExplorerItemCollection()
		{
		}

		public int Add(ExplorerItem item)
		{
			return this.InnerList.Add(item);
		}

		public void AddRange(ExplorerItem[] items)
		{
			this.InnerList.AddRange(items);
		}

		public void AddRange(ExplorerItemCollection items)
		{
			this.InnerList.AddRange(items);
		}

		public ExplorerItem this[int index]
		{
			get { return this.InnerList[index] as ExplorerItem; }
			set { this.InnerList[index] = value; }
		}

		public ExplorerItem[] ToArray()
		{
			return (ExplorerItem[])this.InnerList.ToArray(typeof(ExplorerItem));
		}

		public bool IsIn(int x, int y)
		{
			foreach (ExplorerItem item in this.InnerList)
			{
				if (item.IsIn(x, y))
					return true;
			}

			return false;
		}

		public ExplorerItem GetItemContains(int x, int y)
		{
			foreach (ExplorerItem item in this.InnerList)
			{
				if (item.IsIn(x, y))
					return item;
			}

			return null;
		}
	}
}
