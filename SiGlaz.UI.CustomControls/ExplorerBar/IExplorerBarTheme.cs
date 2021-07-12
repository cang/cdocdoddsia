using System;
using System.Drawing;

namespace SiGlaz.UI.CustomControls
{
	public interface IExplorerBarTheme
	{
		string Name { get; }

		void DrawItem(Graphics grph, ExplorerItem item, bool bDrawBackground);
		void DrawItemsGroup(Graphics grph, ExplorerItemsGroup itemsGroup, bool bDrawItems);
		void DrawExplorerBar(ExplorerItemsGroupCollection itemsGroups, Size size, ref Image cache);
	}
}
