using System;
using System.IO;
using System.Xml.Serialization;

using SIA.UI.Controls.Commands;

namespace SIA.UI.Controls.Automation.Commands
{
	/// <summary>
	/// Summary description for GbcFFTCommandSettings.
	/// </summary>
	public class GbcFFTCommandSettings : AutoCommandSettings
	{
		#region member fields
		public int Threshold = 0;
		public float CutOff = 0;
		#endregion member fields

		#region Constructors ans Deconstructors
		public GbcFFTCommandSettings()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public GbcFFTCommandSettings(int threshold, float cutOff)
		{
			Threshold = threshold;
			CutOff = cutOff;
		}
		#endregion Constructors ans Deconstructors

		#region Properties
		#endregion Properties

		#region Methods
		
		public override void Copy(RasterCommandSettings settings)
		{
			if (settings is GbcFFTCommandSettings == false)
				throw new ArgumentException("Settings is not an instance of GbcFFTCommandSettings", "settings");

			base.Copy(settings);
	
			GbcFFTCommandSettings cmdSettings = (GbcFFTCommandSettings)settings;

			this.Threshold = cmdSettings.Threshold;
			this.CutOff = cmdSettings.CutOff;
		}

		public override void Validate()
		{
			if (Threshold <= 0)
				throw new ArgumentException("Invalid Threshold argument. Threshold must not negative and greater than zero.", "Threshold");
			if (CutOff < 0)
				throw new ArgumentException("Invalid CutOff argument. CutOff must not negative.", "CutOff");
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

				writer.Write((int)Threshold);
				writer.Write((double)CutOff);
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

				this.Threshold = reader.ReadInt32();
				this.CutOff = (float)reader.ReadDouble();
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
