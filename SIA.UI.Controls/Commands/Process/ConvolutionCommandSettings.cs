using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using SIA.SystemLayer;
using SIA.UI.Controls.Commands;
using SIA.UI.Controls.Automation;

namespace SIA.UI.Controls.Commands
{
	/// <summary>
	/// Represents the settings of the kernel filter function
	/// </summary>
	[Serializable]
	public class ConvolutionCommandSettings 
        : AutoCommandSettings
	{
		#region member fields
		// mask type
		public eMaskType MaskType = eMaskType.kMASK_UNKNOWN;

		// matrix mask type		
		public eMatrixType MatrixType = eMatrixType.kMATRIX_UNKNOWN;

		// number of pass
		public int NumPass = 1;

		// threshold value
		public float Threshold = 0;
		#endregion member fields

		#region Constructors ans Deconstructors
		public ConvolutionCommandSettings()
		{			
		}

		public ConvolutionCommandSettings(eMaskType maskType, eMatrixType matrixType, int num_pass, float threshold)
		{
			MaskType = maskType;
			MatrixType = matrixType;
			NumPass = num_pass;
			Threshold = threshold;
		}
		#endregion Constructors ans Deconstructors

		#region Properties
		#endregion Properties

		#region Methods
		public override void Copy(RasterCommandSettings settings)
		{
			if (settings is ConvolutionCommandSettings == false)
				throw new ArgumentException("Settings is not an instance of ConvolutionCommandSettings", "settings");

			base.Copy(settings);
			
			ConvolutionCommandSettings cmdSettings = (ConvolutionCommandSettings)settings;
			
			// clone here
			this.MaskType = cmdSettings.MaskType;
			this.MatrixType = cmdSettings.MatrixType;
			this.NumPass = cmdSettings.NumPass;
			this.Threshold = cmdSettings.Threshold;
		}

		public override void Validate()
		{
			if (this.NumPass <= 0)
				throw new ArgumentException("Number of pass is less than 0.",  "NumberOfPass");
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

				// mask type
				writer.Write((int)MaskType);

				// matrix type
				writer.Write((int)MatrixType);

				// num of pass
				writer.Write((int)NumPass);

				// threshold
				writer.Write((double)Threshold);
				
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

				// mask type
				MaskType = (eMaskType)reader.ReadInt32();

				// matrix type
				MatrixType = (eMatrixType)reader.ReadInt32();

				// num of pass
				NumPass = (int)reader	.ReadInt32();

				// threshold
				Threshold = (float)reader.ReadDouble();
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
