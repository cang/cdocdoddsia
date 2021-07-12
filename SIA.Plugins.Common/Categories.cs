using System;

namespace SIA.Plugins.Common
{
	/// <summary>
	/// Pre-defines list of categories. A category is represented by 
    /// a menu item on the top-level of main menu or a group of shortcut bar items.
	/// </summary>
	public class Categories
	{
		public const string None = "None";
		public const string File = "File";
		public const string Edit = "Edit";
		public const string View = "View";
		public const string Process = "Process";
		public const string Filters = "Filters";
		public const string Analysis = "Analysis";
		public const string Tools = "Tools";
		public const string Help = "Help";

		public static string[] ToArray()
		{
			return new string[] {
				Categories.File,
				Categories.Edit,
				Categories.View,
				Categories.Process,
				Categories.Filters,
				Categories.Analysis,
				Categories.Tools,
				Categories.Help,
			};
		}
	}
}
