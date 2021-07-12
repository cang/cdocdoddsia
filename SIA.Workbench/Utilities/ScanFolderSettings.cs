using System;
using System.IO;
using System.Collections;

namespace SIA.Workbench.Utilities
{
	/// <summary>
	/// Summary description for ScanFolderSettings.
	/// </summary>
	internal class ScanFolderSettings : IDisposable
	{
		/// <summary>
		/// Default database file name
		/// </summary>
		private const string DatabaseFileName = "ProcessedFiles.mdb";
        /** if DatabaseFileName changed, you should change the value of 
         ** SIA.UI.Controls.Automation.Dialogs.DlgLoadImageStepSettings2.DatabaseFileName 
         **/

        /// <summary>
		/// List of folders to be scanned
		/// </summary>
		private ArrayList _folders;
		public string[] Folders
		{
			get
			{
				return (string[])_folders.ToArray(typeof(string));
			}
			set
			{
				_folders.Clear();
				_folders.AddRange(value);
			}
		}

		/// <summary>
		/// Will we scan sub folders ?
		/// </summary>
		private bool _includeSubFolders;
		public bool IncludeSubFolders
		{
			get { return _includeSubFolders; }
			set { _includeSubFolders = value; }
		}

		/// <summary>
		/// Search pattern file name
		/// </summary>
		private string _pattern;
		public string Pattern
		{
			get { return _pattern; }
			set 
			{
                char[] invalidChars = Path.GetInvalidFileNameChars();
                string pattern = value;
                // char '*' is a valid char for pattern
                pattern = pattern.Replace("*", "");

                if (pattern.IndexOf("..") >= 0 || pattern.IndexOfAny(invalidChars) >= 0)
					throw new ArgumentException("Invalid character in search pattern.");
				_pattern = value;
			}
		}

		/// <summary>
		/// The day allows file to store in database
		/// </summary>
		private int _maxDays;
		public int MaxDays
		{
			get { return _maxDays; }
			set { _maxDays = value; }
		}

		/// <summary>
		/// Auto delete the files from database which are older than _maxDays
		/// </summary>
		private bool _autoDeleteOldFile;
		public bool AutoDeleteOldFileFromDatabase
		{
			get { return _autoDeleteOldFile; }
			set { _autoDeleteOldFile = value; }
		}

		/// <summary>
		/// Database file name
		/// </summary>
		private string _dbFileName;
		public string DBFileName
		{
			get 
			{
				return _dbFileName;
			}
			set
			{
				_dbFileName = value;
				if ( !File.Exists(_dbFileName) )
					throw new ArgumentException("The database doesn't exist.");
				//_dbFileName = value;
			}
		}

		/// <summary>
		/// Initialize settings
		/// </summary>
		public ScanFolderSettings()
		{
			_dbFileName			= Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DatabaseFileName);
			_pattern			= "*";
			_folders			= new ArrayList();
			_maxDays			= 7;
			_autoDeleteOldFile  = false;
		}

		/// <summary>
		/// Add a folder to the scanning list
		/// </summary>
		/// <param name="folder">Full path of folder</param>
		public void AddFolder(string folder)
		{
			if ( _folders.Contains(folder) )
				return;
			_folders.Add(folder);
		}

		/// <summary>
		/// Remove a folder from scanning list
		/// </summary>
		/// <param name="folder">Folder to be removed</param>
		public void RemoveFolder(string folder)
		{
			_folders.Remove(folder);
		}

		#region IDisposable Members

		public void Dispose()
		{
			if ( _folders != null )
			{
				_folders.Clear();
				_folders = null;
			}
		}

		#endregion
	}
}
