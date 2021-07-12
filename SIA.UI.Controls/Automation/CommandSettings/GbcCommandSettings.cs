using System;
using System.Data;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using SIA.Common;
using SIA.SystemFrameworks;
using SIA.SystemFrameworks.UI;

using SIA.IPEngine;
using SIA.SystemLayer;

using SIA.UI.Controls.Commands;

namespace SIA.UI.Controls.Automation.Commands
{	
	/// <summary>
	/// Summary description for GbcCommandSettings.
	/// </summary>
	[Serializable]
	public class GbcCommandSettings : AutoCommandSettings
	{
		#region Member Fields
		
		// type of filter used
		private eGlobalBackgroundCorrectionType _type;

		// Erosion filter parameters
		private int _numPass = 1;

		// Fourier filter parameters
		private int _threshold = 0;
		private float _cutOff = 0;

		// Reference image parameters
		private string[] _filePaths = null;

		// Unsharp masking parameters
		private UnsharpParam _unsharpSettings = null;

		#endregion

		#region Properties

		public eGlobalBackgroundCorrectionType Type
		{
			get {return _type;}
			set {_type = value;}
		}

		public int NumPass 
		{
			get {return _numPass ;}
			set {_numPass  = value;}
		}

		public int Threshold
		{
			get {return _threshold;}
			set {_threshold = value;}
		}

		public float CutOff
		{
			get {return _cutOff;}
			set {_cutOff = value;}
		}

		public string[] FilePaths
		{
			get {return _filePaths;}
			set {_filePaths = value;}
		}

		public UnsharpParam UnsharpSettings
		{
			get {return _unsharpSettings;}
			set {_unsharpSettings = value;}
		}

		#endregion

		#region Constructor and Destructors

		public GbcCommandSettings()
		{
		}

		public GbcCommandSettings(int numPass)
		{
			_type = eGlobalBackgroundCorrectionType.ErosionFilter;
			_numPass = numPass;
		}

		public GbcCommandSettings(int threshold, float cutOff)
		{
			_type = eGlobalBackgroundCorrectionType.FastFourierTransform;
			_threshold = threshold;
			_cutOff = cutOff;
		}

		public GbcCommandSettings(string[] filePaths)
		{
			_type = eGlobalBackgroundCorrectionType.ReferenceImages;
			_filePaths = filePaths;
		}

		public GbcCommandSettings(UnsharpParam unsharpSettings)
		{
			_type = eGlobalBackgroundCorrectionType.UnsharpFilter;
			_unsharpSettings = unsharpSettings;
		}

		#endregion

		#region Methods

		public override void Copy(RasterCommandSettings settings)
		{
			if (settings is GbcCommandSettings == false)
				throw new ArgumentException("Settings is not an instance of GbcCommandSettings", "settings");

			base.Copy(settings);

			GbcCommandSettings cmdSettings = (GbcCommandSettings)settings;
			this._type = cmdSettings._type;
			this._numPass = cmdSettings._numPass;
			this._threshold = cmdSettings._threshold;
			this._cutOff = cmdSettings._cutOff;
			this._filePaths = cmdSettings._filePaths;
			if (cmdSettings._unsharpSettings != null)
				this._unsharpSettings = (UnsharpParam)cmdSettings._unsharpSettings.Clone();
			else
				this._unsharpSettings = null;
		}

		public override void Validate()
		{
			switch (this._type)
			{
				case eGlobalBackgroundCorrectionType.ErosionFilter:
					if (_numPass <= 0)
						throw new ArgumentException("Invalid NumPass argument. NumPass must not negative and greater than zero.", "NumPass");
					break;
				case eGlobalBackgroundCorrectionType.FastFourierTransform:
					if (_threshold <= 0)
						throw new ArgumentException("Invalid Threshold argument. Threshold must not negative and greater than zero.", "Threshold");
					if (_cutOff < 0)
						throw new ArgumentException("Invalid CutOff argument. CutOff must not negative.", "CutOff");
					break;
				case eGlobalBackgroundCorrectionType.ReferenceImages:
					if (_filePaths == null)
						throw new ArgumentNullException("FilePaths", "FilePaths is not set to a reference.");
					if (_filePaths.Length == 0)
						throw new ArgumentException("FilePaths is not specified. Please specify at leat a reference image.",  "FilePaths");
					break;
				case eGlobalBackgroundCorrectionType.UnsharpFilter:
					if (_unsharpSettings == null)
						throw new ArgumentNullException("UnsharpSettings", "UnsharpSettings is not set to a reference.");
					break;
				default:
					throw new System.ArgumentException("Unknown eGlobalBackgroundCorrectionType: " + this._type);
			}
		}		
				
		#endregion

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

				// gbc type
				writer.Write((int)this._type);

				switch (this._type)
				{
					case eGlobalBackgroundCorrectionType.ErosionFilter:
						writer.Write((int)_numPass);
						break;
					case eGlobalBackgroundCorrectionType.FastFourierTransform:
						writer.Write((int)_threshold);
						writer.Write((double)_cutOff);
						break;
					case eGlobalBackgroundCorrectionType.ReferenceImages:
						int nums = _filePaths.Length;
						writer.Write((int)nums);
						for (int i=0; i<nums; i++)
						{
							writer.Write((string)_filePaths[i]);
						}
						break;
					case eGlobalBackgroundCorrectionType.UnsharpFilter:
						this._unsharpSettings.Serialize(writer);
						break;
					default:
						throw new System.ArgumentException("Unknown eGlobalBackgroundCorrectionType: " + this._type);
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

				switch (this._type)
				{
					case eGlobalBackgroundCorrectionType.ErosionFilter:
						this._numPass = reader.ReadInt32();
						break;
					case eGlobalBackgroundCorrectionType.FastFourierTransform:
						this._threshold = reader.ReadInt32();
						this._cutOff = (float)reader.ReadDouble();
						break;
					case eGlobalBackgroundCorrectionType.ReferenceImages:
						int nums = reader.ReadInt32();
						this._filePaths = new string[nums];
						for (int i=0; i<nums; i++)
						{
							this._filePaths[i] = reader.ReadString();
						}
						break;
					case eGlobalBackgroundCorrectionType.UnsharpFilter:
						this._unsharpSettings = new UnsharpParam();
						this._unsharpSettings.Deserialize(reader);
						break;
					default:
						throw new System.ArgumentException("Unknown eGlobalBackgroundCorrectionType: " + this._type);
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
