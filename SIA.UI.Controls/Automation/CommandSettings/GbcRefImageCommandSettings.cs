using System;
using System.IO;
using System.Xml.Serialization;

using SIA.UI.Controls.Commands;

namespace SIA.UI.Controls.Automation.Commands
{
	/// <summary>
	/// Summary description for GbcRefImageCommandSettings.
	/// </summary>
	public class GbcRefImageCommandSettings : AutoCommandSettings
	{
		#region member fields
		public string[] FilePaths = null;
		#endregion member fields

		#region Constructors ans Deconstructors
		public GbcRefImageCommandSettings()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public GbcRefImageCommandSettings(string[] filePaths)
		{
			//
			// TODO: Add constructor logic here
			//
			FilePaths = filePaths;
		}
		#endregion Constructors ans Deconstructors

		#region Properties
		#endregion Properties

		#region Methods
		
		public override void Copy(RasterCommandSettings settings)
		{
			if (settings is GbcRefImageCommandSettings == false)
				throw new ArgumentException("Settings is not an instance of GbcRefImageCommandSettings", "settings");

			base.Copy(settings);
			
			GbcRefImageCommandSettings cmdSettings = (GbcRefImageCommandSettings)settings;

			this.FilePaths = cmdSettings.FilePaths;	
		}

		public override void Validate()
		{
			if (FilePaths == null)
				throw new ArgumentNullException("FilePaths", "FilePaths is not set to a reference.");
			if (FilePaths.Length == 0)
				throw new ArgumentException("FilePaths is not specified. Please specify at leat a reference image.",  "FilePaths");
		}		
	
		#endregion Methods

		#region Serializable && Deserialize
		public void Serialize(String filename)
		{
			System.IO.FileStream fs = null;
			try
			{
				fs = new System.IO.FileStream(filename, System.IO.FileMode.Create, System.IO.FileAccess.Write);
				Serialize(fs);
			}
			catch (System.Exception)
			{
				throw;
			}
			finally
			{
				if (fs != null) fs.Close();
			}
		}

		public override void Serialize(System.IO.Stream stream)
		{
			BinaryWriter writer = null;
			try
			{
				writer = new BinaryWriter(stream);
		
				int nums = FilePaths.Length;
				writer.Write((int)nums);
				for (int i=0; i<nums; i++)
				{
					writer.Write((string)FilePaths[i]);
				}
			}
			catch (System.Exception)
			{
				throw;
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
			System.IO.FileStream fs = null;
			try
			{
				fs = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
				Deserialize(fs);
			}
			catch (System.Exception)
			{
				throw;
			}
			finally
			{
				if (fs != null) fs.Close();
			}
		}

		public override void Deserialize(System.IO.Stream stream)
		{
			BinaryReader reader = null;
			try
			{
				reader = new BinaryReader(stream);

				int nums = reader.ReadInt32();
				this.FilePaths = new string[nums];
				for (int i=0; i<nums; i++)
				{
					this.FilePaths[i] = reader.ReadString();
				}
			}
			catch (System.Exception)
			{
				throw;
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
