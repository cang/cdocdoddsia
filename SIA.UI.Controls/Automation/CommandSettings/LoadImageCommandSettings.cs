using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using SIA.Common;
using SIA.Workbench.Common;
using SIA.UI.Controls.Commands;

namespace SIA.UI.Controls.Automation.Commands
{
	#region InputFileType : byte
	public enum InputFileType : byte
	{
		None = 0,
		File = 1,
		Path = 2
	}
	#endregion

	#region InputFileFilter
	public class InputFileFilter
	{
		#region Member fields
		public bool FilterInputFileFormat = false;
		public bool Bmp = false;
		public bool Jpeg = false;
		public bool Png = false;
		public bool Tiff = false;
		public bool Gif = false;
		public bool Fit = false;
		
		public bool FilterInputFileName = false;
		public string FileNameFormat = string.Empty;
		#endregion Member fields

		#region Constructors and Destructors
		public InputFileFilter()
		{
		}

		public InputFileFilter(bool filterInputFileFormat, bool bmp, bool jpeg, bool png, bool tiff, bool gif, bool fit)
		{
			this.FilterInputFileFormat = filterInputFileFormat;
			this.Bmp = bmp;
			this.Jpeg = jpeg;
			this.Png = png;
			this.Tiff = tiff;
			this.Gif = gif;
			this.Fit = fit;
		}

		public InputFileFilter(bool filterInputFileName, string fileNameFormat)
		{
			this.FilterInputFileName = filterInputFileName;
			this.FileNameFormat = fileNameFormat;
		}

		public InputFileFilter(bool filterInputFileFormat, bool bmp, bool jpeg, bool png, bool tiff, bool gif, bool fit , 
			bool filterInputFileName, string fileNameFormat)
		{
			this.FilterInputFileFormat = filterInputFileFormat;
			this.Bmp = bmp;
			this.Jpeg = jpeg;
			this.Png = png;
			this.Tiff = tiff;
			this.Gif = gif;
			this.Fit = fit;

			this.FilterInputFileName = filterInputFileName;
			this.FileNameFormat = fileNameFormat;
		}
		#endregion Constructors and Destructors

		public InputFileFilter Clone()
		{
			return (new InputFileFilter(
				this.FilterInputFileFormat, 
				this.Bmp,
				this.Jpeg,
				this.Png,
				this.Tiff,
				this.Gif,
				this.Fit,
				this.FilterInputFileName, this.FileNameFormat));
		}

		private int Count()
		{
			int n = 0;
			if (Bmp) n++;
			if (Jpeg) n++;
			if (Png) n++;
			if (Tiff) n+=2;
			if (Gif) n++;
			if (Fit) n+=3;
			return n;
		}

		public string[] GetExtensions()
		{
			int nExts = this.Count();
			if (nExts <= 0)
				return null;
			int index = 0;
			string[] exts = new string[nExts];
			try
			{
				if (Bmp) exts[index++] = ".bmp";
				if (Jpeg) exts[index++] = ".jpg";
				if (Png) exts[index++] = ".png";
				if (Tiff) 
				{
					exts[index++] = ".tif";
					exts[index++] = ".tiff";
				}
				if (Gif) exts[index++] = ".gif";
				if (Fit)
				{
					exts[index++] = ".fit";
					exts[index++] = ".fts";
					exts[index++] = ".fits";
				}				
			}
			catch (System.Exception exp)
			{
				throw exp;
			}
			return exts;
		}
	}
	#endregion InputFileFilter

	/// <summary>
	/// Summary description for LoadImageCommandSettings.
	/// </summary>
	public class LoadImageCommandSettings : AutoCommandSettings
	{
		#region member fields
		private string _fileName = "";
		private string _filePath = "";
		private InputFileType _inputFileType = InputFileType.File;
		private ProcessingDataCollection _data = null;
		private bool _clearFileHistory = false;
		private bool _scanSubFolder = false;
		private bool _useFilter = false;
		private InputFileFilter _inputFileFilter = new InputFileFilter();
		#endregion member fields

		#region Constructors ans Deconstructors
		public LoadImageCommandSettings() : this(null, null, InputFileType.None)
		{
		}

		public LoadImageCommandSettings(string fileName) : this(fileName, null, InputFileType.File)
		{
		}

		public LoadImageCommandSettings(string fileName, string filePath, InputFileType inputFileType)
		{
			_fileName = fileName;
			_filePath = filePath;
			_inputFileType = inputFileType;
		}

		public LoadImageCommandSettings(string fileName, ProcessingDataCollection data)
		{
			_fileName = fileName;
			if (data == null)
				_data = new ProcessingDataCollection();
			else
				_data = data;
		}
		#endregion Constructors ans Deconstructors

		#region Properties

		public string FileName
		{
			get
			{
				return _fileName;
			}
			set
			{
				_fileName = value;
			}
		}

		public string FilePath
		{
			get
			{
				return _filePath;
			}
			set
			{
				_filePath = value;
			}
		}

		public InputFileType InputFileType
		{
			get
			{
				return _inputFileType;
			}
			set
			{
				_inputFileType = value;
			}
		}

		public bool ClearProcessedFileHistory
		{
			get
			{
				return _clearFileHistory;
			}
			set
			{
				_clearFileHistory = value;
			}
		}

		public bool ScanSubFolder
		{
			get
			{
				return _scanSubFolder;
			}
			set
			{
				_scanSubFolder = value;
			}
		}

		[XmlElement("Data", typeof(ProcessingDataCollection))]
		public ProcessingDataCollection Data
		{
			get {return _data;}

			set {_data = value;}
		}

		public bool UseFilter
		{
			get { return _useFilter; }
			set { _useFilter = value; }
		}

		public InputFileFilter Filter
		{
			get { return _inputFileFilter; }
			set { _inputFileFilter = value; }
		}
		#endregion Properties

		#region Methods
		
		public override void Copy(RasterCommandSettings settings)
		{
			if (settings is LoadImageCommandSettings == false)
				throw new ArgumentException("Settings is not an instance of LoadImageCommandSettings", "settings");

			base.Copy(settings);
	
			LoadImageCommandSettings cmdSettings = (LoadImageCommandSettings)settings;

			this._fileName = cmdSettings.FileName;
			this._filePath = cmdSettings.FilePath;
			this._inputFileType = cmdSettings.InputFileType;
			this._useFilter = cmdSettings.UseFilter;
			if (cmdSettings.Filter == null)
				this._inputFileFilter = new InputFileFilter();
			else
				this._inputFileFilter = cmdSettings.Filter.Clone();
		}

		public override void Validate()
		{
			if (_data == null)
				throw new ArgumentNullException("Data", "Data is not set.");
			
			if (_data.Count == 0)
				throw new ArgumentException("Images was not provided.", "FileName");
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

				writer.Write(this._fileName);
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

				this._fileName = reader.ReadString();
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
