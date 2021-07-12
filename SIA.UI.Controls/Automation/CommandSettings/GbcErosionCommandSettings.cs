using System;
using System.IO;
using System.Xml.Serialization;

using SIA.UI.Controls.Commands;

namespace SIA.UI.Controls.Automation.Commands
{
	/// <summary>
	/// Summary description for GbcErosionCommandSettings.
	/// </summary>
	[Serializable]
	public class GbcErosionCommandSettings : AutoCommandSettings
	{
		#region member fields
		public int NumPass = 1;
		#endregion member fields

		#region Constructors ans Deconstructors
		public GbcErosionCommandSettings()
		{			
		}

		public GbcErosionCommandSettings(int num_pass)
		{
			NumPass = num_pass;
		}
		#endregion Constructors ans Deconstructors

		#region Properties
		#endregion Properties

		#region Methods
		
		public override void Copy(RasterCommandSettings settings)
		{
			if (settings is GbcErosionCommandSettings == false)
				throw new ArgumentException("Settings is not an instance of GbcErosionCommandSettings", "settings");

			base.Copy(settings);
			
			GbcErosionCommandSettings cmdSettings = (GbcErosionCommandSettings)settings;
			this.NumPass = cmdSettings.NumPass;	
		}

		public override void Validate()
		{
			if (NumPass <= 0)
				throw new ArgumentException("Invalid NumPass argument. NumPass must not negative and greater than zero.", "NumPass");
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
		
				writer.Write((int)NumPass);
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

				this.NumPass = reader.ReadInt32();
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
