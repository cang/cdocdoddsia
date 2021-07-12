using System;
using System.IO;
using System.Xml.Serialization;

using SIA.UI.Controls.Commands;
using SIA.Common;

namespace SIA.UI.Controls.Automation.Commands
{
	/// <summary>
	/// Summary description for GbcUnsharpCommandSettings.
	/// </summary>
	public class GbcUnsharpCommandSettings : AutoCommandSettings
	{
		#region member fields
		[XmlElement(typeof(UnsharpParam))]
		public UnsharpParam UnsharpSettings = null;
		#endregion member fields

		#region Constructors ans Deconstructors
		public GbcUnsharpCommandSettings()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public GbcUnsharpCommandSettings(UnsharpParam unsharpSettings)
		{
			//
			// TODO: Add constructor logic here
			//
			UnsharpSettings = unsharpSettings;
		}
		#endregion Constructors ans Deconstructors

		#region Properties
		#endregion Properties

		#region Methods
		
		public override void Copy(RasterCommandSettings settings)
		{
			if (settings is GbcUnsharpCommandSettings == false)
				throw new ArgumentException("Settings is not an instance of GbcUnsharpCommandSettings", "settings");

			base.Copy(settings);
	
			GbcUnsharpCommandSettings cmdSettings = (GbcUnsharpCommandSettings)settings;

			if (cmdSettings.UnsharpSettings != null)
				this.UnsharpSettings = (UnsharpParam)cmdSettings.UnsharpSettings.Clone();
			else
				this.UnsharpSettings = null;
		}

		public override void Validate()
		{
			if (UnsharpSettings == null)
				throw new ArgumentNullException("UnsharpSettings", "UnsharpSettings is not set to a reference.");
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
		
				this.UnsharpSettings.Serialize(writer);
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

				this.UnsharpSettings = new UnsharpParam();
				this.UnsharpSettings.Deserialize(reader);
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
