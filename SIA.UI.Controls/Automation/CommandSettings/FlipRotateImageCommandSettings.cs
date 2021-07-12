using System;
using System.IO;
using System.Xml.Serialization;

using SIA.Common.Imaging.Filters;
using SIA.UI.Controls.Commands;

namespace SIA.UI.Controls.Automation.Commands
{
	/// <summary>
	/// Summary description for FlipRotateImageCommandSettings.
	/// </summary>
	public class FlipRotateImageCommandSettings : AutoCommandSettings
	{
		public enum Actions : int
		{
			FlipVertical = 1,
			FlipHorizontal = 2,
			Rotate90CW = 3,
			Rotate90CCW = 4,
			Rotate180 = 5,
			RotateByAngle = 6
		}

		#region member fields
		
		public int ActionType = (int)Actions.FlipVertical;

		public float RotateAngle = 0;

		#endregion member fields

		#region Constructors ans Deconstructors
		
		public FlipRotateImageCommandSettings()
		{			
		}

		#endregion Constructors ans Deconstructors

		#region Properties
		#endregion Properties

		#region Methods
		
		public override void Copy(RasterCommandSettings settings)
		{
			if (settings is FlipRotateImageCommandSettings == false)
				throw new ArgumentException("Settings is not an instance of FlipRotateImageCommandSettings", "settings");

			base.Copy(settings);
			
			FlipRotateImageCommandSettings cmdSettings = (FlipRotateImageCommandSettings)settings;
			
			// clone here
			this.ActionType = cmdSettings.ActionType;
			this.RotateAngle = cmdSettings.RotateAngle;
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

				// action type
				writer.Write((int)ActionType);

				// rotate angle
				writer.Write((float)RotateAngle);
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

				// action type
				this.ActionType = (int)reader.ReadInt32();

				// rotate angle
				this.RotateAngle = (float)reader.ReadSingle();
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
