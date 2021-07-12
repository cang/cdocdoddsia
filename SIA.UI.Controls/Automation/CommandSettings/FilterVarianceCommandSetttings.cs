using System;
using System.IO;
using System.Xml.Serialization;

using SIA.UI.Controls.Commands;

namespace SIA.UI.Controls.Automation.Commands
{
	/// <summary>
	/// Summary description for FilterVarianceCommandSettings.
	/// </summary>
	public class FilterVarianceCommandSettings : AutoCommandSettings
	{
		#region member fields
		public float Radius = 0;
		#endregion member fields

		#region Constructors ans Deconstructors
		public FilterVarianceCommandSettings()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public FilterVarianceCommandSettings(float radius)
		{
			Radius = radius;
		}
		#endregion Constructors ans Deconstructors

		#region Properties
		#endregion Properties

		#region Methods
		
		public override void Copy(RasterCommandSettings settings)
		{
			if (settings is FilterVarianceCommandSettings == false)
				throw new ArgumentException("Settings is not an instance of FilterVarianceCommandSettings", "settings");

			base.Copy(settings);
			
			FilterVarianceCommandSettings cmdSettings = (FilterVarianceCommandSettings)settings;
			this.Radius = cmdSettings.Radius;
	
		}

		public override void Validate()
		{
			if (Radius <= 0)
				throw new ArgumentException("Radius cannot be less than 0.",  "Radius");
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

				writer.Write((int)Radius);
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

				Radius = reader.ReadInt32();
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
