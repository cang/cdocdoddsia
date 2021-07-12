using System;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Windows.Forms;

using SIA.Common;
using SIA.UI.Controls.Commands;
using SIA.UI.Controls.Automation;
using SIA.UI.Controls.Automation.Commands;
using SIA.UI.Controls.Utilities;

namespace SIA.UI.Controls.Automation
{
	/// <summary>
	/// Proces utility functions for command settings serialization
	/// </summary>
	public class RasterCommandSettingsSerializer
	{
		private static Encoding defaultEncoding = Encoding.UTF8;
		private static bool _verboseMode = true;
		private static bool _messageBox = true;

		public static bool VerboseMode
		{
			get {return _verboseMode;}
			set {_verboseMode = value;}
		}

		public static bool MessageBox
		{
			get {return _messageBox;}
			set {_messageBox = value;}
		}

		static RasterCommandSettingsSerializer()
		{			
			_verboseMode = true;
			_messageBox = true;
		}

		public static void Serialize(String fileName, RasterCommandSettings cmdSettings)
		{
			using (StreamWriter writer = new StreamWriter(fileName, false, defaultEncoding))
				Serialize(writer, cmdSettings);
		}

		public static void Serialize(FileStream stream, RasterCommandSettings cmdSettings)
		{
			XmlTextWriter writer = new XmlTextWriter(stream, defaultEncoding);
			Serialize(writer, cmdSettings);
		}

		public static void Serialize(XmlTextWriter writer, RasterCommandSettings cmdSettings)
		{
			XmlSerializerEx serializer = null;

			try
			{
				Type type = cmdSettings.GetType();
				serializer = new XmlSerializerEx(type);
				serializer.Serialize(writer, cmdSettings);
			}
			catch (System.Exception exp)
			{
				ShowMessage("Setting type does not match.");
				
				TraceLog(exp);
			}
			finally
			{
				serializer = null;
			}
		}

		public static void Serialize(StreamWriter writer, RasterCommandSettings cmdSettings)
		{
			XmlSerializerEx serializer = null;

			try
			{
				Type type = cmdSettings.GetType();
				serializer = new XmlSerializerEx(type);
				serializer.Serialize(writer, cmdSettings);
			}
			catch (System.Exception exp)
			{
				ShowMessage("Setting type does not match.");
				
				TraceLog(exp);
			}
			finally
			{
				serializer = null;
			}
		}


		public static RasterCommandSettings Deserialize(String fileName, Type type)
		{
			RasterCommandSettings cmdSettings = null;
			// deserialize settings from the specified file path
			using (StreamReader reader = new StreamReader(fileName, true))
				cmdSettings = Deserialize(reader, type);
			return cmdSettings;
		}

		public static RasterCommandSettings Deserialize(FileStream stream, Type type)
		{
			RasterCommandSettings cmdSettings = null;
			XmlTextReader reader = null;

			try
			{
				reader = new XmlTextReader(stream);
				cmdSettings = Deserialize(reader, type);
			}
			finally
			{
				if (reader != null)
					reader.Close();
				reader = null;
			}

			return cmdSettings;
		}

		public static RasterCommandSettings Deserialize(XmlTextReader reader, Type type)
		{
			RasterCommandSettings cmdSettings = null;
			XmlSerializerEx serializer = null;

			try
			{
				serializer = new XmlSerializerEx(type);
				cmdSettings = serializer.Deserialize(reader) as RasterCommandSettings;
			}
			catch (System.Exception exp)
			{
				TraceLog(exp);
				throw;
			}
			finally
			{
				serializer = null;
			}

			return cmdSettings;
		}

		public static RasterCommandSettings Deserialize(StreamReader reader, Type type)
		{
			RasterCommandSettings cmdSettings = null;
			XmlSerializerEx serializer = null;

			try
			{
				serializer = new XmlSerializerEx(type);
				cmdSettings = serializer.Deserialize(reader) as RasterCommandSettings;
			}
			catch (System.Exception exp)
			{
				TraceLog(exp);
				throw;
			}
			finally
			{
				serializer = null;
			}

			return cmdSettings;
		}

		
		private static void ShowMessage(string message)
		{
			if (!_messageBox)
				return ;
			
			MessageBoxEx.Error(message);
		}

		private static void TraceLog(Exception exp)
		{
			if (!_verboseMode)
				return ;

			Trace.WriteLine(exp, "RasterCommandSettingsSerializer");
		}
	}
}
