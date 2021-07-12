using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Collections;

namespace SIA.UI.Components.Common
{
	/// <summary>
	/// The PseudoColor class provides data structure and functionality for pseudo color operations
    /// such as displaying, serialization, ...
	/// </summary>
	public class PseudoColor : ICloneable 	
	{
		//internal const string PSEUDOCOLOR_FILE_ID = "RDE Pseudo-Color Settings";
        internal const string PSEUDOCOLOR_FILE_ID = "Pseudo-Color Settings";
		internal const string PSEUDOCOLOR_FILE_AUTHOR = "SiGlaz";

		private string _name = "Custom";
		private Color[] _colors = null;
		private float[] _positions = null;

		public String Name
		{
			get {return _name;}
			set
			{
				_name = value;
				OnNameChanged();
			}
		}

		protected virtual void OnNameChanged()
		{

		}

		public Color[] Colors
		{
			get {return _colors;}
			set 
			{
				_colors = value;
				OnColorsChanged();
			}
		}

		protected virtual void OnColorsChanged()
		{

		}

		public float[] Positions
		{
			get {return _positions;}
			set
			{
				_positions = value;
				OnPositionChanged();
			}
		}

		protected virtual void OnPositionChanged()
		{

		}

		public PseudoColor()
		{
		}

		public PseudoColor(String name, Color[] colors, float[] positions)
		{
			_name = name;
			_colors = colors;
			_positions = positions;
		}

		public void Load(String filePath)
		{		
			FileStream fs = null;
			try
			{
				fs = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
				using (BinaryReader reader = new BinaryReader(fs))
				{
					String fileID = reader.ReadString();
					if (fileID != PseudoColor.PSEUDOCOLOR_FILE_ID)
						throw new System.Exception("Invalid Pseudo Color File Format");

					String fileAuthor = reader.ReadString();
					if (fileAuthor != PseudoColor.PSEUDOCOLOR_FILE_AUTHOR)
						throw new System.Exception("Invalid Author tag");

					this._name = reader.ReadString();

					int length = reader.ReadInt32();
					this._colors = new Color[length];
					for (int i=0; i<length; i++)
					{
						int argb = reader.ReadInt32();
						this._colors[i] = Color.FromArgb(argb);
					}

					length = reader.ReadInt32();
					this._positions = new float[length];
					for (int i=0; i<length; i++)
					{
						this._positions[i] = reader.ReadSingle();
					}
				}				
			}
			catch(System.Exception exp)
			{
				throw exp;
			}
			finally
			{
				if (fs != null)
					fs.Close();
			}			
		}

		public void Save(String filePath)
		{
			FileStream fs = null;
			try
			{
				fs = new FileStream(filePath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
				using (BinaryWriter writer = new BinaryWriter(fs))
				{
					writer.Write(PseudoColor.PSEUDOCOLOR_FILE_ID);
					writer.Write(PseudoColor.PSEUDOCOLOR_FILE_AUTHOR);
					writer.Write(this._name);
					writer.Write(this._colors.Length);
					for (int i=0; i<this._colors.Length; i++)
						writer.Write(this._colors[i].ToArgb());
					writer.Write(this._positions.Length);
					for (int i=0; i<this._positions.Length; i++)
						writer.Write(this._positions[i]);
				}				
			}
			catch(System.Exception exp)
			{
				throw exp;
			}
			finally
			{
				if (fs != null)
					fs.Close();
			}			
		}


		#region ICloneable Members

		public object Clone()
		{
			return this.MemberwiseClone();
		}

		#endregion

		public override string ToString()
		{
			return this.Name;
		}

		private static readonly PseudoColor _empty = new PseudoColor();
		public static PseudoColor Empty
		{
			get 
			{
				return _empty;
			}
		}
	}

	public class PseudoColors
	{
		internal const String PSEUDOCOLORS_FIRE = "Fire";
		internal const String PSEUDOCOLORS_ICE = "Ice";
		internal const String PSEUDOCOLORS_GRAYSCALE = "Gray Scale";

		private static IDictionary _builtInPseudoColors = null;

		private static IDictionary BuiltInPseudoColors
		{
			get 
			{
				if (_builtInPseudoColors == null)
					_builtInPseudoColors = new Hashtable();
				return _builtInPseudoColors;
			}
		}

		public static PseudoColor FromName(String name)
		{	
			PseudoColor result = PseudoColor.Empty;
			switch (name)
			{
				case PSEUDOCOLORS_FIRE:
					result = PseudoColors.Fire;
					break;
				case PSEUDOCOLORS_GRAYSCALE:
					result = PseudoColors.GrayScale;
					break;
				case PSEUDOCOLORS_ICE:
					result = PseudoColors.Ice;
					break;
				default:
					break;
			}

			return result;
		}

		public static PseudoColor GrayScale
		{
			get
			{
				PseudoColor result = (PseudoColor)PseudoColors.BuiltInPseudoColors[PSEUDOCOLORS_GRAYSCALE];			
				if ( result == null)
				{
					Color[] colors = new Color[]{Color.Black, Color.White};
					float[] positions = new float[] {0.0F, 1.0F};
					result = new PseudoColor(PSEUDOCOLORS_GRAYSCALE, colors, positions);
					PseudoColors.BuiltInPseudoColors[PSEUDOCOLORS_GRAYSCALE] = result;
				}

				return result;
			}
		}

		public static PseudoColor Fire
		{
			get
			{
				PseudoColor result = (PseudoColor)PseudoColors.BuiltInPseudoColors[PSEUDOCOLORS_FIRE];			
				if ( result == null)
				{
					Color[] colors = new Color[] {Color.Black, Color.Red, Color.Yellow, Color.White};
					float[] positions = new float[] {0.0F, 0.33F, 0.66F, 1.0F};
					result = new PseudoColor(PSEUDOCOLORS_FIRE, colors, positions);
					PseudoColors.BuiltInPseudoColors[PSEUDOCOLORS_FIRE] = result;
				}
				return result;
			}
		}

		public static PseudoColor Ice
		{
			get
			{
				PseudoColor result = (PseudoColor)PseudoColors.BuiltInPseudoColors[PSEUDOCOLORS_ICE];			
				if ( result == null)
				{
					Color[] colors = new Color[] {Color.Black, Color.Blue, Color.Cyan, Color.White};
					float[] positions = new float[] {0.0F, 0.33F, 0.66F, 1.0F};
					result = new PseudoColor(PSEUDOCOLORS_ICE, colors, positions);
					PseudoColors.BuiltInPseudoColors[PSEUDOCOLORS_ICE] = result;
				}
				return result;
			}
		}
	}
}
