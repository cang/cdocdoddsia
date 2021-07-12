using System;
using System.Collections.Generic;
using System.Threading;
using System.Data;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Serialization;
using System.Text;
using System.Diagnostics;

using SIA.Common;
using SIA.UI.Controls;
using SIA.UI.Controls.Common;
using SIA.UI.Controls.Commands;

using SIA.SystemLayer;

using SIA.Plugins.Common;

using SIA.UI.Controls.Automation;
using SIA.UI.Controls.Automation.Steps;

namespace SIA.UI.Controls.Automation
{
	/// <summary>
	/// Provides functions for script serialization, exportation
	/// </summary>
	public class ScriptSerializer : IDisposable
	{
		#region fields

		// synchronize object
		private static Object _syncObject = new Object();
		
		// cached xml serializer
		private static XmlSerializer _serializer = null;

		// key word for exporting script
		public static string[] FilePathKeywords = null;

		#endregion

		#region constructor and destructors

		static ScriptSerializer()
		{
			FilePathKeywords = new string[]
				{
					"FilePath",
					"FileName",
					"FileNameFormat",
					"SettingsFilePath",
					"OutputDir",
					"anyType",
					"ExtMaskFilePath",
					"ReferenceImage",
					"string"
				};
		}

		public ScriptSerializer()
		{
		}

		~ScriptSerializer()
		{
			this.Dispose(false);
		}

		#region IDisposable Members

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion

		protected virtual void Dispose(bool disposing)
		{
			
		}

		#endregion

		#region Static Methods

		public static void Initialize()
		{
			// enter critical section
			if (Monitor.TryEnter(_syncObject))
			{ 
				AppDomain domain = AppDomain.CurrentDomain;
				string oldAppBase = domain.SetupInformation.ApplicationBase;

				try
				{
					string key = "ScriptSerializer";

					// search in application domain for xml serializer
					_serializer = domain.GetData(key) as XmlSerializer;
					
					// if serializer was not initialized
					if (_serializer == null)
					{
						Type myType = typeof(Script);

						// switch application base to RDE folder
						string folder = Path.GetDirectoryName(myType.Assembly.Location);
						domain.SetupInformation.ApplicationBase = folder;
				
						Type[] extraTypes = ProcessStepManager.GetTypeOfRegistedProcessSteps();

						XmlAttributes attribs = new XmlAttributes();

						foreach (Type type in extraTypes)
						{
							// try to load instance
							object obj = Activator.CreateInstance(type);
							// initialize assemblies
							XmlElementAttribute xmlElement = new XmlElementAttribute(type);
							attribs.XmlElements.Add(xmlElement);
						}

						XmlAttributeOverrides overrides = new XmlAttributeOverrides();
						overrides.Add(myType, "ProcessSteps", attribs);

						// create serializer
						_serializer = new XmlSerializer(myType, overrides);		

						// register for serialization event handlers
						_serializer.UnknownElement += new XmlElementEventHandler(XmlSerializer_UnknownElement);
						_serializer.UnknownAttribute += new XmlAttributeEventHandler(XmlSerializer_UnknownAttribute);
						_serializer.UnknownNode += new XmlNodeEventHandler(XmlSerializer_UnknownNode);

						// save serializer into application domain data
						domain.SetData(key, _serializer);
					}
				}
				catch (Exception exp)
				{
					Trace.WriteLine(exp);
					throw;
				}
				finally
				{
					// switch application base back to application folder
					domain.SetupInformation.ApplicationBase = oldAppBase;

					// exit critical section
					Monitor.Exit(_syncObject);
				}
			}
		}

		public static void Uninitialize()
		{
			if (Monitor.TryEnter(_syncObject))
			{
				if (_serializer != null)
				{
					_serializer.UnknownElement -= new XmlElementEventHandler(XmlSerializer_UnknownElement);
					_serializer.UnknownAttribute -= new XmlAttributeEventHandler(XmlSerializer_UnknownAttribute);
					_serializer.UnknownNode -= new XmlNodeEventHandler(XmlSerializer_UnknownNode);
					_serializer = null;
				}

				Monitor.Exit(_syncObject);
			}
		}

		public static void Update()
		{
			// clean up old settings
			ScriptSerializer.Uninitialize();
			// creates new settings
			ScriptSerializer.Initialize();
		}
		
		
		#endregion

		#region Methods

		public void Serialize(TextWriter writer, Script script)
		{
			_serializer.Serialize(writer, script);
		}
		
		public Script Deserialize(TextReader textReader)
		{
			return _serializer.Deserialize(textReader) as Script;
		}

		#endregion

		#region XmlSerializerEx event handlers

		private static void XmlSerializer_UnknownElement(object sender, XmlElementEventArgs e)
		{
			string msg = string.Format("Unknown element: {0} at Line:{1} Pos:{2}, InnerXml: {2}", 
				e.Element.Name, e.LineNumber, e.LinePosition, e.Element.InnerXml);
			Trace.WriteLine(msg);
		}

		private static void XmlSerializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
		{
			string msg = string.Format("Unknown attribute: {0} at Line:{1} Pos:{2}, InnerXml: {2}", 
				e.Attr.Name, e.LineNumber, e.LinePosition, e.Attr.InnerXml);
			Trace.WriteLine(msg);
		}

		private static void XmlSerializer_UnknownNode(object sender, XmlNodeEventArgs e)
		{
			string msg = string.Format("Unknown node: {0} at Line:{1} Pos:{2}, Text: {2}", 
				e.Name, e.LineNumber, e.LinePosition, e.Text);
			Trace.WriteLine(msg);
		}

		#endregion
		
		#region Import/Export handlers

		public static void AddExportKeyword(string keyword)
		{
			bool bFound = false;
			ArrayList keywords = new ArrayList(FilePathKeywords);
			foreach (string key in keywords)
			{
				if (key == keyword)
					bFound = true;
			}

			if (!bFound)
			{
				keywords.Add(keyword);
				FilePathKeywords = (string[])keywords.ToArray(typeof(string)); 
			}
		}
		
		public static void Import(string filePath, string rootDir)
		{
			using (Stream stream = File.OpenRead(filePath))
			{
				using (BinaryReader reader = new BinaryReader(stream))
				{
					int numFiles = reader.ReadInt32();
					string[] virPaths = new string[numFiles];
					
					for (int i=0; i<numFiles; i++)
					{
						virPaths[i] = reader.ReadString();
						// convert to absolute path
						string absPath = ToAbsolutePath(virPaths[i], rootDir);

						// read content buffer
						int length = reader.ReadInt32();
						if (length > 0)
						{
							// read buffer
							byte[] buffer = reader.ReadBytes(length);

							// update path and write to absolute path
							WriteAndUpdateContent(buffer, absPath, rootDir);
						}
						else
						{
							if (absPath.IndexOfAny(new char[] {'[',']'}) < 0)
							{
								Directory.CreateDirectory(absPath);
							}
							else
							{
								string dir = Path.GetDirectoryName(absPath);
								if (!Directory.Exists(dir))
									Directory.CreateDirectory(dir);
							}
						}
					}
				}
			}
		}

		public static void Export(string scriptFilePath, string filePath)
		{
			if (scriptFilePath == null)
				throw new FileNotFoundException("Script file was not found", scriptFilePath);
			
			ArrayList files = new ArrayList();

			// add the script file
			files.Add(scriptFilePath);

			// add other director and files in the script file path
			string folder = Path.GetDirectoryName(scriptFilePath);
			string[] otherFiles = Directory.GetFiles(folder);
			files.AddRange(otherFiles);

			string[] otherDirs = Directory.GetDirectories(folder);
			files.AddRange(otherDirs);

			// scan for file path in settings file
			for (int i=0; i<files.Count; i++)
			{
				string path = files[i] as string;
				if (Directory.Exists(path))
				{
					otherFiles = Directory.GetFiles(path);
					files.AddRange(otherFiles);

					otherDirs = Directory.GetDirectories(path);
					files.AddRange(otherDirs);
				}
				else if (File.Exists(path))
				{
                    try
                    {
                        ArrayList newFiles = ScanForFilePath(path);
                        files.AddRange(newFiles);
                    }
                    catch
                    {
                        // nothing
                    }
				}
			}

			// sort the file list 
			files.Sort();

			// remove the duplicated items
			for (int i=0; i<files.Count-1; i++)
			{
				while ((i+1) < files.Count && String.Compare((string)files[i], (string)files[i+1])==0)
					files.RemoveAt(i+1);
			}

			// convert from absolute to virtual path
			ArrayList virtualPaths = ToVirtualPath(files, scriptFilePath);
			ArrayList contents = new ArrayList();
				
			// write output to temporary file
			string tempFile = Path.GetTempFileName();

			try
			{
				// Create and write the data to the temporary file
				using (FileStream stream = File.Open(tempFile, FileMode.Create, FileAccess.Write, FileShare.None))
				{
					using (BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8))
					{
						// Step 1: write num files
						writer.Write(files.Count);
						
						// Step 2: write script file contents
						for (int i=0; i<files.Count; i++)
						{
							string file = (string)files[i];
							string virPath = (string)virtualPaths[i];
							
							// Step 2a: write the virtual path
							writer.Write(virPath);

							// Step 2b: write the content of the file
							// if file is not exists and file is the same with virtual path
							if (File.Exists(file) == false || file == virPath)
							{
								writer.Write(0);
							}
							else // read and update file content then write it
							{
								byte[] buffer = ReadAndUpdateContent(file, files, virtualPaths);
								writer.Write(buffer.Length);
								writer.Write(buffer);
							}
						}
					}
				}

				// copy the temporary file to the destination path
				File.Copy(tempFile, filePath, true);
			}
			finally
			{
				if (File.Exists(tempFile))
					File.Delete(tempFile);
			}
		}

		private static void WriteAndUpdateContent(byte[] buffer, string filePath, string rootFolder)
		{
			if (!Directory.Exists(Path.GetDirectoryName(filePath)))
				Directory.CreateDirectory(Path.GetDirectoryName(filePath));

			if (IsSettingsFile(filePath))
			{
				string content = Encoding.UTF8.GetString(buffer);
				content = content.Replace("%HOME%", rootFolder);
				
				using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
				{
					writer.Write(content);
				}
			}
			else
			{
				using (Stream stream = File.OpenWrite(filePath))
				{
					using (BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8))
						writer.Write(buffer);
				}
			}
		}

		private static byte[] ReadAndUpdateContent(string file, ArrayList files, ArrayList virtualPaths)
		{
			if (IsSettingsFile(file) == true)
			{
				string data = "";
				using (StreamReader reader = new StreamReader(file, Encoding.UTF8, true))
					data = reader.ReadToEnd();

				for (int i=0; i<files.Count; i++)
				{
					string absPath = (string)files[i];
					string virPath = (string)virtualPaths[i];
					data = data.Replace(absPath, virPath);
				}

				return Encoding.UTF8.GetBytes(data);
			}
			else
			{
				using (Stream stream = File.OpenRead(file))
				{
					long length = stream.Length;
					byte[] buffer = new byte[length];
					stream.Read(buffer, 0, (int)length);
					return buffer;
				}
			}
		}

		private static bool IsXmlFile(string filePath)
		{
			using (StreamReader reader = new StreamReader(filePath, Encoding.UTF8, true))
			{
				string str = reader.ReadLine();
                if (str == null)
                    return false;

                str = str.Trim();
				return str.IndexOf("<?xml")>=0;
			}
		}

		private static bool IsSettingsFile(string filePath)
		{
            IFileType[] fileTypes = FileTypes.AllFileTypes;
            List<string> exts = new List<string>();
            foreach (IFileType fileType in fileTypes)
            {
                if (fileType is TextFileType)
                    exts.AddRange(fileType.Extension);
            }

			string ext = Path.GetExtension(filePath);
			foreach (string extension in exts)
				if (extension.IndexOf(ext) == 0)
					return true; 
            return false;
		}

		private static string ToAbsolutePath(string file, string folder)
		{
			return file.Replace("%HOME%", folder);
		}

		private static ArrayList ToVirtualPath(ArrayList files, string scriptFilePath)
		{
			ArrayList result = new ArrayList();
			string folder = Path.GetDirectoryName(scriptFilePath);
			for (int i=0; i<files.Count; i++)
			{
				string file = files[i] as string;
				if (file.IndexOf(folder) >= 0)
					result.Add(file.Replace(folder, "%HOME%"));
				else
				{
					// remove absolute file out of file list
					files.RemoveAt(i--);
					//result.Add(file);
				}
			}

			return result;
		}

		private static ArrayList ScanForFilePath(string filePath)
		{
			string[] keywords = FilePathKeywords;
			ArrayList files = new ArrayList();
			
			if (IsXmlFile(filePath))
			{
				XmlDocument document = new XmlDocument();
				document.Load(filePath);

				foreach (string keyword in keywords)
				{
					XmlNodeList nodeList = document.GetElementsByTagName(keyword);
					foreach (XmlNode node in nodeList)
					{
						string value = node.InnerText;
						if (value != null && value != string.Empty)
						{
							if (IsValidPath(value))
								files.Add(value);
						}
					}
				}
			}

			return files;
		}

		private static bool IsValidPath(string str)
		{
			try
			{
				return Path.IsPathRooted(str);
			}
#if DEBUG 
			catch (Exception exp)
			{
				Trace.WriteLine(exp);
#else
			catch 
			{
#endif
				return false;
			}
		}

		#endregion
	}
}
