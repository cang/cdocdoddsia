using System;
using System.IO;
using System.Windows.Forms;

namespace SIA.Automation.Launcher.Configuration
{
	/// <summary>
	/// Encapsulates the logic for user intervenable file system manipulation through recursion and events.
	/// </summary>
	public class PathCreationEngine
	{
		/// <summary>
		/// Fires when a call to CreateDirectory fails.
		/// </summary>
		public static event PathCreationEngineEventHandler CreateDirectoryFailed;

		/// <summary>
		/// Fires when a call to DeleteFile fails.
		/// </summary>
		public static event PathCreationEngineEventHandler DeleteFileFailed;

		/// <summary>
		/// Ensures that the specified directory exists, otherwise creates it. Raises the CreateDirectoryFailed event upon failure to allow user intervention.
		/// </summary>
		/// <param name="path">The path to the directory, this should be a directory, not including the filename, othewise a directory with the file name will be created.</param>
		/// <param name="userData">User specified data to be raised during event notification</param>
		/// <returns></returns>
		public static bool CreateDirectory(string path, object sender, object userData)
		{
			try
			{				
				if (!Directory.Exists(path))
					Directory.CreateDirectory(path);
				return true;
			}
			catch(System.Exception systemException)
			{
				PathCreationEngineEventArgs e = new PathCreationEngineEventArgs(path);
				e.Exception = systemException;
				e.UserData = userData;
				PathCreationEngine.OnCreateDirectoryFailed(sender, e);

				switch(e.Result)
				{
				case DialogResult.OK:
					return true;

				case DialogResult.Cancel:
					return false;

				case DialogResult.Abort:					
					return false;					

				case DialogResult.Retry:
					return PathCreationEngine.CreateDirectory(path, sender, userData);					

				case DialogResult.Ignore:
					return true;					
				};
			}
			return false;		
		}

		/// <summary>
		/// Delete's the specified path if it exists. Raises the DeleteFileFailed event upon failure to allow user intervention.
		/// </summary>
		/// <param name="path">The path to delete, can be a file or directory</param>
		/// <param name="userData">User specified data to be raised during event notification</param>
		/// <returns></returns>
		public static bool DeleteFile(string path, object sender, object userData)
		{
			try
			{
				if (File.Exists(path))
					File.Delete(path);
				return true;
			}
			catch(System.Exception systemException)
			{
				PathCreationEngineEventArgs e = new PathCreationEngineEventArgs(path);
				e.Exception = systemException;
				e.UserData = userData;
				PathCreationEngine.OnDeleteFileFailed(sender, e);

				switch(e.Result)
				{
				case DialogResult.OK:
					return true;

				case DialogResult.Cancel:
					return false;

				case DialogResult.Abort:					
					return false;					

				case DialogResult.Retry:
					return PathCreationEngine.DeleteFile(path, sender, userData);					

				case DialogResult.Ignore:
					return true;					
				};
			}
			return false;
		}

		private static void OnCreateDirectoryFailed(object sender, PathCreationEngineEventArgs e)
		{
			try
			{
				if (PathCreationEngine.CreateDirectoryFailed != null)
					PathCreationEngine.CreateDirectoryFailed(sender, e);
			}
			catch(System.Exception systemException)
			{
				System.Diagnostics.Trace.WriteLine(systemException);
			}
		}	
				
		private static void OnDeleteFileFailed(object sender, PathCreationEngineEventArgs e)
		{
			try
			{
				if (PathCreationEngine.DeleteFileFailed != null)
					PathCreationEngine.DeleteFileFailed(sender, e);
			}
			catch(System.Exception systemException)
			{
				System.Diagnostics.Trace.WriteLine(systemException);
			}
		}	
	}

	
}
