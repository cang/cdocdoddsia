using System;
using System.IO;
using System.Xml.Serialization;

using SIA.Common;
using SIA.Common.Imaging;

using SIA.UI.Controls.Commands;

namespace SIA.UI.Controls.Automation.Commands
{
	/// <summary>
	/// Summary description for SaveImageCommandSettings.
	/// </summary>
	[Serializable]
	public class SaveImageCommandSettings : AutoCommandSettings
	{
		#region member fields
		public String FileName = "";
		public String FileNameFormat = "";
		public eImageFormat Format = eImageFormat.Fit;
		#endregion member fields

		#region Constructors ans Deconstructors
		public SaveImageCommandSettings()
		{			
		}

		public SaveImageCommandSettings(String fileName, eImageFormat format)
		{
			FileName = fileName;
			Format = format;			
		}

		public SaveImageCommandSettings(String fileName, String fileNameFormat, eImageFormat format)
		{
			FileName = fileName;
			FileNameFormat = fileNameFormat;
			Format = format;
		}
		#endregion Constructors ans Deconstructors

		#region Properties
		#endregion Properties

		#region Methods
		
		public override void Copy(RasterCommandSettings settings)
		{
			if (settings is SaveImageCommandSettings == false)
				throw new ArgumentException("Settings is not an instance of SaveImageCommandSettings", "settings");

			base.Copy(settings);
	
			SaveImageCommandSettings cmdSettings = (SaveImageCommandSettings)settings;
			this.FileName = (String)cmdSettings.FileName.Clone();
			this.Format = cmdSettings.Format;
		}

		public override void Validate()
		{
			if (FileNameFormat == null || FileNameFormat == string.Empty)
			{
				if (FileName == null)
					throw new ArgumentNullException("FileName", "FileName is not set.");
				if (FileName.Length == 0)
					throw new ArgumentException("FileName is not specified.",  "FileName");
			}
		}		
	
		#endregion Methods

		#region Serializable && Deserialize
		public void Serialize(String filename)
		{
			using (FileStream fs = File.Open(filename, FileMode.Create, FileAccess.Write))
				this. Serialize(fs);
		}

		public override void Serialize(System.IO.Stream stream)
		{
			BinaryWriter writer = null;

			try
			{
				writer = new BinaryWriter(stream);	
				writer.Write(this.FileName);
				writer.Write((int)this.Format);
			}
			finally
			{	
				if(writer != null)
					writer.Close();
				writer = null;
			}
		}

		public void Deserialize(String filename)
		{
			using (FileStream fs = File.Open(filename, FileMode.Open, FileAccess.Read))
				this.Deserialize(fs);
		}

		public override void Deserialize(System.IO.Stream stream)
		{
			BinaryReader reader = null;
			try
			{
				reader = new BinaryReader(stream);
				this.FileName = reader.ReadString();
				this.Format = (eImageFormat)reader.ReadInt32();
			}
			finally
			{	
				if(reader != null)
					reader.Close();
				reader = null;
			}
		}
		#endregion Serializable && Deserialize		
	}
}
