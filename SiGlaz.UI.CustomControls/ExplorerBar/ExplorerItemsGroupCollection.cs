using System;
using System.Collections;

namespace SiGlaz.UI.CustomControls
{
	public class ExplorerItemsGroupCollection : CollectionBase
	{
		public ExplorerItemsGroupCollection()
		{
		}

		public int Add(ExplorerItemsGroup group)
		{
			return this.InnerList.Add(group);
		}

		public void AddRange(ExplorerItemsGroup[] groups)
		{
			this.InnerList.AddRange(groups);
		}

		public void AddRange(ExplorerItemsGroupCollection groups)
		{
			this.InnerList.AddRange(groups);
		}

		public ExplorerItemsGroup this[int index]
		{
			get { return this.InnerList[index] as ExplorerItemsGroup; }
			set { this.InnerList[index] = value; }
		}

		public ExplorerItemsGroup[] ToArray()
		{
			return (ExplorerItemsGroup[])this.InnerList.ToArray(typeof(ExplorerItemsGroup));
		}

		public ExplorerItem GetItemContains(int x, int y)
		{			
			foreach (ExplorerItemsGroup group in this.InnerList)
			{
				ExplorerItem item = group.GetItemContains(x, y);
				if (item != null)
					return item;
			}
			
			return null;
		}

		public ExplorerItemsGroup GetHighlightCaptionGroup(int x, int y)
		{
			foreach (ExplorerItemsGroup group in this.InnerList)
			{
				if (group.CaptionContains(x, y))
					return group;
			}

			return null;
		}
	}
}
