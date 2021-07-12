using System;
using System.IO;
using System.Collections;
using System.Collections.Specialized;

using SIA.Workbench.Common;

namespace SIA.Workbench.Utilities 
{
	/// <summary>
	/// Summary description for ScanFolder.
	/// </summary>
	internal class ScanFolderEngine : IDisposable 
	{

		/// <summary>
		/// Input parameter for scan folder engine
		/// </summary>
		private ScanFolderSettings _settings;

		/// <summary>
		/// Queue of new coming files
		/// </summary>
		private Queue _newComingFiles;

		/// <summary>
		/// Database contains processed file list
		/// </summary>
		private ScanFolderDatabase _database;

		public event EventHandler StartedScanningFolder = null;
		public event EventHandler ScanningFolder = null;
		public event EventHandler EndedScanningFolder = null;				


		/// <summary>
		/// Initialize engine with input setting
		/// </summary>
		/// <param name="settings">Input setting</param>
		public ScanFolderEngine(ScanFolderSettings settings) 
		{

			this._settings		 = settings;
			this._newComingFiles = new Queue();
			this._database		 = new ScanFolderDatabase(_settings.DBFileName);
		}

		/// <summary>
		/// Get the oldest new coming file from the queue
		/// </summary>
		/// <returns>File name if the queue isn't empty, otherwise - null</returns>
		public string GetNewFile() 
		{

			// scan for new coming files only in case the queue is empty
			if ( _newComingFiles.Count == 0 )
				Scan();

			// pop the file of the queue						
			while (_newComingFiles.Count > 0) 
			{						
				string incomingFile = (string)_newComingFiles.Dequeue();
				if (IsNewComingFile(incomingFile)) 
				{
					this.SetFileAsProcessed(incomingFile);
					return incomingFile;
				}
			}

			// pop the file of the queue
			// if ( _newComingFiles.Count > 0 )
			//			return (string)_newComingFiles.Dequeue();
			
			//  no new coming file
			return null;
		}

		/// <summary>
		/// Set the input file as processed
		/// </summary>
		/// <param name="filename">The file to be set</param>
		public void SetFileAsProcessed(string filename) 
		{

			_database.Insert(filename);
		}

		/// <summary>
		/// Delete file from database
		/// </summary>
		/// <param name="filename">The file name to be deleted</param>
		public void DeleteFileByName(string filename) 
		{

			_database.DeleteByFileName(filename);
		}

		/// <summary>
		/// Delete old files which are older age (in day)
		/// </summary>
		/// <param name="age">File age</param>
		public void DeleteFileByAge(int age) 
		{

			DateTime processedDate = DateTime.Now.Date.AddDays(-age);
			_database.DeleteByProcessedDate(processedDate);
		}

		/// <summary>
		/// Scan for new coming files and add them to the queue
		/// </summary>
		private void Scan() 
		{

			// delete old files if needed
			if ( _settings.AutoDeleteOldFileFromDatabase ) 
			{

				DeleteFileByAge(_settings.MaxDays);
			}

			ArrayList folders = new ArrayList();
		
			if ( _settings.IncludeSubFolders ) 
			{

				// get all sub-folders of folder list
				foreach (string folder in _settings.Folders) 
				{

					ArrayList subFolders = GetSubFolders(folder);
					if ( subFolders != null && subFolders.Count > 0 )
						folders.AddRange(subFolders);
				}
			}
			else 
			{

				folders.AddRange(_settings.Folders);
			}

			if (folders != null && folders.Count > 0) 
			{
				// raise event start scanning
				this.RaiseEventStartedScanningFolder();

				string patterns = _settings.Pattern.Trim();
				string [] pats = patterns.Split(new char[]{';'});

				// get new coming files and put them into the queue
				foreach (string folder in folders) 
				{

					if ( !Directory.Exists(folder) )
						continue;
										
					// raise event scanning folder
					this.RaiseEventScanningFolder(folder);
										
					if (pats != null && pats.Length == 1) 
					{
						string[] files = this.GetFilesInDirectory(folder, patterns);
						if (files != null && files.Length > 0)
							foreach (string filename in files) 
							{
								if ( IsCompletedFile(filename) && IsNewComingFile(filename) ) 
								{
									_newComingFiles.Enqueue(filename);

									// raise event scanning folder
									this.RaiseEventScanningFolder(null, filename);
								}
							}
					}
					else if (pats.Length > 1) 
					{
						SortedList files = this.GetFilesInDirectory(folder, pats);
																				
						int nFiles = files.Count;
						for (int i=0; i<nFiles; i++) 
						{
							string filename = (string)files.GetKey(i);
							if ( IsCompletedFile(filename) && IsNewComingFile(filename) ) 
							{
								_newComingFiles.Enqueue(filename);

								// raise event scanning folder
								this.RaiseEventScanningFolder(null, filename);
							}
						}
					}

					//string[] files = this.GetFilesInDirectory(folder, _settings.Pattern);
					//										foreach (string filename in files) {
					//												if ( IsCompletedFile(filename) && IsNewComingFile(filename) ) {
					//														_newComingFiles.Enqueue(filename);
					//
					//														// raise event scanning folder
					//														this.RaiseEventScanningFolder(null, filename);
					//												}
					//										}
				}

				// raise event end scanning
				this.RaiseEventEndedScanningFolder();
			}
		}

		private void RaiseEventStartedScanningFolder() 
		{							
			if (this.StartedScanningFolder != null)
				this.StartedScanningFolder(this, EventArgs.Empty);
		}

		private void RaiseEventScanningFolder(string folder) 
		{
			if (this.ScanningFolder != null)
			{
				ScanningFolderEventArgs args = new ScanningFolderEventArgs(folder);
				this.ScanningFolder(this, args);
			}
		}

		private void RaiseEventScanningFolder(string folder, string file) 
		{
			if (this.ScanningFolder != null)
			{
				ScanningFolderEventArgs args = new ScanningFolderEventArgs(folder, file);			
				this.ScanningFolder(this, args);
			}
		}

		private void RaiseEventEndedScanningFolder() 
		{
			if (this.EndedScanningFolder != null)
				this.EndedScanningFolder(this, EventArgs.Empty);
		}


		/// <summary>
		/// Check if the file is new coming
		/// </summary>
		/// <param name="filename">The file to be checked</param>
		/// <returns>true if the file is new coming</returns>
		private  bool IsNewComingFile(string filename) 
		{

			return !_database.IsExisted(filename);
		}

		/// <summary>
		/// Check if the file is copying from somewhere
		/// </summary>
		/// <param name="filename">The file to be checked</param>
		/// <returns>true if file can be open exclusive</returns>
		private bool IsCompletedFile(string filename) 
		{

			if ( filename == null || filename == string.Empty || !File.Exists(filename) )
				return false;

			FileStream stream = null;
			try 
			{

				stream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None);
				return true;
			}
			catch (System.Exception) 
			{

				return false;
			}
			finally 
			{
				if ( stream != null ) 
				{


					stream.Close();
					stream = null;
				}
			}
		}

		/// <summary>
		/// Get sub-folders of input folder
		/// </summary>
		/// <param name="rootFolder">Input folder</param>
		/// <returns>List of sub-folders</returns>
		private ArrayList GetSubFolders(String rootFolder) 
		{

			ArrayList folderList = new ArrayList();

			folderList.Add(rootFolder);

			Queue folderQueue = new Queue(1);

			folderQueue.Enqueue(rootFolder);

			while ( folderQueue.Count > 0 ) 
			{

				String folder = (String)folderQueue.Dequeue();

				if ( !Directory.Exists(folder) ) continue;

				try 
				{

					String[] subFolders = Directory.GetDirectories(folder);

					foreach ( String tmpFolder in subFolders ) 
					{

						folderQueue.Enqueue(tmpFolder);

						folderList.Add(tmpFolder);
					}
				}
				catch ( System.UnauthorizedAccessException ) 
				{

				}
			}

			return folderList;
		}

		public void ClearDatabase() 
		{

			_database.ClearDatabase();
		}

		/// <summary>
		/// Get files in specified directory which match with any pattern specified in patterns
		/// </summary>
		/// <param name="directory"></param>
		/// <param name="patterns"></param>
		public string[] GetFilesInDirectory(string path, string patterns) 
		{
			return Directory.GetFiles(path, patterns);
		}

		public SortedList GetFilesInDirectory(string path, string[] pats) 
		{
			SortedList files = new SortedList();						
			foreach (string pat in pats) 
			{
				string[] subFiles = Directory.GetFiles(path, pat);
				if (subFiles != null && subFiles.Length > 0) 
				{
					int nSubFiles = subFiles.Length;
					for (int i=0; i<nSubFiles; i++) 
					{
						if (!files.ContainsKey(subFiles[i])) 
						{
							files.Add(subFiles[i], null);
						}
					}
				}
			}

			return files;
			//return (string[])(files.Keys as ArrayList).ToArray(typeof(string));

			// COMMENTED: The following routines is used for remove duplicated files
			//						if (pats.Length > 1) 
			//						{
			//								// sort files
			//								files.Sort(new StringComparer());
			//		
			//								// remove duplicate file name
			//								files.BinarySearch(
			//
			//								for (int i=0; i<files.Count-1; i++) {
			//										string val1 = files[i] as string;
			//										string val2 = files[i+1] as string;
			//										if (String.Compare(val1, val2) == 0) {
			//												files.RemoveAt(i+1);
			//												i--;
			//										}
			//								}
			//						}

			//return (string[])files.ToArray(typeof(string));
		}
		
		#region IDisposable Members

		public void Dispose() 
		{
			if ( _newComingFiles != null ) 
			{


				_newComingFiles.Clear();
				_newComingFiles = null;
			}
		}

		#endregion

		private class StringComparer : IComparer 
		{
			#region IComparer Members

			public int Compare(object x, object y) 
			{
				return String.Compare(x as string, y as string);
			}

			#endregion
		}
	}
}

