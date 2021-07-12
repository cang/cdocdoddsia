using System;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization.Formatters.Binary;

namespace SIA.UI.Controls.Common
{
	internal class UserSettings
	{
		public static object RestoreObject(string filename)
		{
			// method should never throw an exception...
			Stream stream = null;
			try
			{
				stream = GetStreamForRead(filename);
				if (stream==null)
					return null;
				return new BinaryFormatter().Deserialize(stream);
			}
			catch (Exception e)
			{
				Trace.WriteLine(e);
				return null;
			}
			finally
			{
				if (stream!=null)
					stream.Close();
			}
		}

		public static void StoreObject(string filename, object obj)
		{
			if (obj==null)
			{
				DeleteFile(filename);
				return;
			}

			// method should never throw an exception...
			Stream stream = null;
			try
			{
				stream = GetStreamForWrite(filename);
				if (stream==null)
					return;
				new BinaryFormatter().Serialize(stream, obj);
			}
			catch
			{
				return;
			}
			finally
			{
				if (stream!=null)
					stream.Close();
			}
		}

		private static String GetFullPathName(string filename)
		{
			string userAppDataPath = AppSettings.MySettingsFolder;
			if (Directory.Exists(userAppDataPath) == false)
				Directory.CreateDirectory(userAppDataPath);	
			return userAppDataPath + @"\" + filename;
		}

		public static Stream GetStreamForWrite(string filename)
		{
			Stream stream = null;

			try
			{
				string filePath = GetFullPathName(filename);
				stream = new System.IO.FileStream(filePath, FileMode.Create, FileAccess.Write);
			}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);
			}
			finally
			{				
			}
			
			return stream;
		}

		public static Stream GetStreamForRead(string filename)
		{
			Stream stream = null;

			try
			{
				string filePath = GetFullPathName(filename);
				if (File.Exists(filePath) == true)			
					stream = new System.IO.FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read);
			}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);
			}
			finally
			{				
			}
			
			return stream;
		}

		public static void DeleteFile(string fileName)
		{
			if (File.Exists(fileName))
				File.Delete(fileName);
		}
	}	
}
