using System;
using System.IO;
using System.Xml.Serialization;

using SIA.UI.Controls.Commands;

namespace SIA.UI.Controls.Automation.Commands
{
	/// <summary>
	/// Summary description for FilterRankCommandSettings.
	/// </summary>
	[Serializable]
	public class FilterRankCommandSettings : AutoCommandSettings
	{
		#region member fields
		/// <summary>
		/// filter type
		/// </summary>
		/// <value>
		/// Min=0, Max = 1, Median = 2, Mean = 3
		/// </value>
		public int TypeFilter = 0;

		/// <summary>
		/// size of kernel
		/// </summary>
		public int SzKernel = 1;

		/// <summary>
		/// number of passes
		/// </summary>
		public int NumPass = 1;
		#endregion member fields

		#region Constructors ans Deconstructors
		public FilterRankCommandSettings()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public FilterRankCommandSettings(int typeFilter, int szKernel, int num_pass)
		{
			TypeFilter = typeFilter;
			SzKernel = szKernel;
			NumPass = num_pass;
		}
		#endregion Constructors ans Deconstructors

		#region Properties
		#endregion Properties

		#region Methods
		
		public override void Copy(RasterCommandSettings settings)
		{
			if (settings is FilterRankCommandSettings == false)
				throw new ArgumentException("Settings is not an instance of FilterRankCommandSettings", "settings");

			base.Copy(settings);
	
			FilterRankCommandSettings cmdSettings = (FilterRankCommandSettings)settings;
			this.TypeFilter = cmdSettings.TypeFilter;
			this.SzKernel = cmdSettings.SzKernel;
			this.NumPass = cmdSettings.NumPass;
		}

		public override void Validate()
		{
			if (TypeFilter < 0 || TypeFilter > 3)
				throw new ArgumentException("Filter type doesn't match.",  "Filter Type");

			if (SzKernel <= 0)
				throw new ArgumentException("Kernel size cannot be less than or equal 0.",  "Kernel size");

			if (NumPass <= 0)
				throw new ArgumentException("Number of pass cannot be less than or equal 0.",  "Number of pass");
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

				// filter type
				writer.Write((int)TypeFilter);

				// kernel size
				writer.Write((int)SzKernel);

				// number of pass
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

				// filter type
				TypeFilter = reader.ReadInt32();

				// kernel size
				SzKernel = reader.ReadInt32();

				// number of pass
				NumPass = reader.ReadInt32();
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
