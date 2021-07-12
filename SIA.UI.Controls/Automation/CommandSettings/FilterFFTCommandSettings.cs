using System;
using System.IO;
using System.Xml.Serialization;

using SIA.Common.Imaging.Filters;
using SIA.UI.Controls.Commands;

namespace SIA.UI.Controls.Automation.Commands
{
	/// <summary>
	/// Summary description for FilterFFTCommandSettings.
	/// </summary>
	public class FilterFFTCommandSettings : AutoCommandSettings
	{
		#region member fields
		// filter type
		public FFTFilterType Type = FFTFilterType.LowPass;

		// cut off
		public float CutOff = 0;

		// weight
		public float Weight = 0;
		#endregion member fields

		#region Constructors ans Deconstructors
		public FilterFFTCommandSettings()
		{			
		}

		public FilterFFTCommandSettings(FFTFilterType type, float cutoff, float weight)
		{
			Type = type;
			CutOff = cutoff;
			Weight = weight;
		}
		#endregion Constructors ans Deconstructors

		#region Properties
		#endregion Properties

		#region Methods
		
		public override void Copy(RasterCommandSettings settings)
		{
			if (settings is FilterFFTCommandSettings == false)
				throw new ArgumentException("Settings is not an instance of FilterFFTCommandSettings", "settings");

			base.Copy(settings);
			
			FilterFFTCommandSettings cmdSettings = (FilterFFTCommandSettings)settings;
			
			// clone here
			this.Type = cmdSettings.Type;
			this.CutOff = cmdSettings.CutOff;
			this.Weight = cmdSettings.Weight;				
		}

		public override void Validate()
		{			
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

				// FFT filter type
				writer.Write((int)Type);

				// cut off
				writer.Write((double)CutOff);

				// weight
				writer.Write((double)Weight);				
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

				// filter type
				Type = (FFTFilterType)reader.ReadInt32();

				// cut off
				CutOff = (float)reader.ReadDouble();

				// weight
				Weight = (float)reader.ReadDouble();
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
