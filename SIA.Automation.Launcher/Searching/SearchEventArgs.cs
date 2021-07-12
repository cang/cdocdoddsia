using System;
using System.IO;

namespace SIA.Automation.Launcher.Searching
{
	/// <summary>
	/// EventArgs class for the SearchEventHandler delegate
	/// </summary>
	public class SearchEventArgs: System.EventArgs
	{
		private FileInfo _file;

		/// <summary>
		/// Initializes a new instance of the SearchEventArgs class
		/// </summary>	
		/// <param name="file"></param>
		public SearchEventArgs(FileInfo file)
		{
			_file = file;
		}

		/// <summary>
		/// Gets the file for this event
		/// </summary>
		public FileInfo File
		{
			get
			{
				return _file;
			}
		}
	}

	/// <summary>
	/// Delegate for the SearchEventArgs class
	/// </summary>
	public delegate void SearchEventHandler(object sender, SearchEventArgs e);
}
