using System;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;

using SIA.Plugins;
using SIA.Plugins.Common;
using SIA.UI.Controls;

namespace SIA.UI
{
	/// <summary>
	/// Summary description for RDERecentFiles.
	/// </summary>
	public class SIARecentFiles
	{
        // test branch
		private static RecentFileManager _recentFiles = null;

		static SIARecentFiles()
		{
			_recentFiles = new RecentFileManager(UI_TYPE.SIA);
		}

		public static string[] GetRecentFiles()
		{
			_recentFiles.Derialize();
			return (string[])_recentFiles.AR.ToArray(typeof(string));
		}

		public static void AddRecentFile(string fileName)
		{
			_recentFiles.Add(fileName);
			_recentFiles.Serialize();
		}

		public static void RemoveRecentFile(string fileName)
		{
			_recentFiles.Remove(fileName);
			_recentFiles.Serialize();
		}
	}
}
