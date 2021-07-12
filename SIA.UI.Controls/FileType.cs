using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using SIA.Common;
using SIA.SystemLayer;

using SIA.UI.Controls.Utilities;

namespace SIA.UI.Controls
{
    /// <summary>
    /// The IFileType provides basic declaration of a specified type of file.
    /// A file type is distinguised by the extenstion of the file name.
    /// </summary>
	public interface IFileType
	{
        /// <summary>
        /// Gets the list of supported extenstion by the file type
        /// </summary>
		string[] Extension {get;}

        /// <summary>
        /// Gets the filter string which is used in the open/save file dialog
        /// </summary>
		string Filter {get;}

        /// <summary>
        /// Gets the open file dialog associated with this file type
        /// </summary>
        /// <param name="title">The title of the file dialog</param>
        /// <returns>An instance of the open file dialog if succeeded, otherwise null</returns>
		OpenFileDialog OpenFileDialog(string title);

        /// <summary>
        /// Gets the save file dialog associated with this file type
        /// </summary>
        /// <param name="title">The title of the file dialog</param>
        /// <returns>An instance of the save file dialog if succeeded, otherwise null</returns>
		SaveFileDialog SaveFileDialog(string title);

        /// <summary>
        /// Checks whether the specified file path is a valid format of this file type.
        /// </summary>
        /// <remarks>
        /// Some file type checks only for the extension of the specified file path. But some
        /// open the file and check for the content of the specified file.
        /// </remarks>
        /// <param name="filePath">The location of the file to be checked</param>
        /// <returns>True if valid, otherwise false</returns>
		bool IsValidFileFormat(string filePath);
	}

    /// <summary>
    /// Provides basic implementation of the IFileType interface
    /// </summary>
	public abstract class FileType 
        : IFileType
	{
		public abstract string[] Extension {get;}
		public abstract string Filter {get;}
		
		public virtual OpenFileDialog OpenFileDialog(string title)
		{
			string keyWord = this.GetType().FullName;
			IFileType[] fileTypes = new IFileType[] {this};
			return CommonDialogs.OpenFileDialog(keyWord, title, fileTypes, -1, null, null);
		}

        public virtual SaveFileDialog SaveFileDialog(string title)
		{
			string keyWord = this.GetType().FullName;
			IFileType[] fileTypes = new IFileType[] {this};
			return CommonDialogs.SaveFileDialog(keyWord, title, fileTypes, -1, null, null);
		}

		
		public virtual bool IsValidFileFormat(string filePath)
		{
			return true;
		}
	}

    /// <summary>
    /// All image file type (bitmap, jpeg, png, ...) must inherits from this class
    /// </summary>
	public abstract class ImageFileType 
        : FileType
	{
        /// <summary>
        /// Gets the image format (codec) of this file type
        /// </summary>
		public abstract eImageFormat ImageFormat {get;}
	};
	
    /// <summary>
    /// Represents the bitmap file type
    /// </summary>
	public class BitmapFileType 
        : ImageFileType
	{
		private string _filter = "Bitmap Files (*.bmp)|*.bmp";
		private string[] _extension = new string[]{".bmp"};
		private eImageFormat _imageFormat = eImageFormat.Bmp;

		#region IFileType Members

		public override string[] Extension 
		{
			get {return _extension;}
		}

		public override string Filter
		{
			get {return _filter;}
		}

		public override eImageFormat ImageFormat 
		{
			get { return _imageFormat;}
		}
		
		#endregion
	}

    /// <summary>
    /// Represents the jpeg file type
    /// </summary>
	public class JpegFileType : ImageFileType
	{
		private string _filter = "JPEG images (*.jpg)|*.jpg";
		private string[] _extension = new string[]{".jpg"};
		private eImageFormat _imageFormat = eImageFormat.Jpeg;

		#region IFileType Members

		public override string[] Extension 
		{
			get {return _extension;}
		}

		public override string Filter
		{
			get {return _filter;}
		}

		public override eImageFormat ImageFormat 
		{
			get { return _imageFormat;}
		}
		
		#endregion
	}

    /// <summary>
    /// Represent the png file type
    /// </summary>
	public class PngFileType 
        : ImageFileType
	{
		private string _filter = "PNG images (*.png)|*.png";
		private string[] _extension = new string[]{".png"};
		private eImageFormat _imageFormat = eImageFormat.Png;

		#region IFileType Members

		public override string[] Extension 
		{
			get {return _extension;}
		}

		public override string Filter
		{
			get {return _filter;}
		}

		public override eImageFormat ImageFormat 
		{
			get { return _imageFormat;}
		}
		
		#endregion
	}

    /// <summary>
    /// Represents the tiff file type
    /// </summary>
	public class TiffFileType 
        : ImageFileType
	{
		private string _filter = "TIFF images (*.tif;*.tiff)|*.tif;*.tiff";
		private string[] _extension = new string[]{".tif", ".tiff"};
		private eImageFormat _imageFormat = eImageFormat.Tiff;

		#region IFileType Members

		public override string[] Extension 
		{
			get {return _extension;}
		}

		public override string Filter
		{
			get {return _filter;}
		}

		public override eImageFormat ImageFormat 
		{
			get { return _imageFormat;}
		}
		
		#endregion
	}

    /// <summary>
    /// Represents the Gif file type
    /// </summary>
	public class GifFileType 
        : ImageFileType
	{
		private string _filter = "GIF images (*.gif)|*.gif";
		private string[] _extension = new string[]{".gif"};
		private eImageFormat _imageFormat = eImageFormat.Gif;

		#region IFileType Members

		public override string[] Extension 
		{
			get {return _extension;}
		}

		public override string Filter
		{
			get {return _filter;}
		}

		public override eImageFormat ImageFormat 
		{
			get { return _imageFormat;}
		}
		
		#endregion
	}

    /// <summary>
    /// Represents the raw file type
    /// </summary>
	public class RawFileType : ImageFileType
	{
		private string _filter = "Raw images (*.raw)|*.raw";
		private string[] _extension = new string[]{".raw"};
		private eImageFormat _imageFormat = eImageFormat.Raw;

		#region IFileType Members

		public override string[] Extension 
		{
			get {return _extension;}
		}

		public override string Filter
		{
			get {return _filter;}
		}

		public override eImageFormat ImageFormat 
		{
			get { return _imageFormat;}
		}
		
		#endregion
	}

    /// <summary>
    /// Represents the fits file type
    /// </summary>
	public class FitsFileType 
        : ImageFileType
	{
		private string _filter = "FITS Files (*.fits;*.fit;*.fts)|*.fits;*.fit;*.fts";
		private string[] _extension = new string[]{".fits", ".fit", ".fts"};
		private eImageFormat _imageFormat = eImageFormat.Fit;

		#region IFileType Members

		public override string[] Extension 
		{
			get {return _extension;}
		}

		public override string Filter
		{
			get {return _filter;}
		}

		public override eImageFormat ImageFormat 
		{
			get { return _imageFormat;}
		}
		
		#endregion
	}

    public class CommonImagesType : ImageFileType
    {
        private string _filter = "Common Image Files|*.bmp;*.jpeg;*.jpg;*.png;*.tif;*.tiff;*.gif;*.raw;*.fits;*.fit;*.fts";
        private string[] _extension = 
            new string[] {
                ".bmp", ".jpeg", ".jpg", ".png", 
                ".tif", ".tiff", ".gif", ".raw",
                ".fits", ".fit", ".fts" };
        private eImageFormat _imageFormat = eImageFormat.Unknown;

        #region IFileType Members

        public override string[] Extension
        {
            get { return _extension; }
        }

        public override string Filter
        {
            get { return _filter; }
        }

        public override eImageFormat ImageFormat
        {
            get { return _imageFormat; }
        }

        #endregion
    }

    /// <summary>
    /// Represents the text file type
    /// </summary>
	public class TextFileType
        : FileType
	{
		private string _filter = "Text Files (*.txt)|*.txt";
		private string[] _extension = new string[]{".txt"};

		#region IFileType Members

		public override string[] Extension 
		{
			get {return _extension;}
		}

		public override string Filter
		{
			get {return _filter;}
		}
		
		#endregion
	}

    /// <summary>
    /// Represents the klarf file type
    /// </summary>
	public class KlarfFileType 
        : TextFileType
	{
		private string _filter = "KLARF Files (*.000)|*.000";
		private string[] _extension = new string[]{".000", ".001"};

		#region IFileType Members

		public override string[] Extension 
		{
			get {return _extension;}
		}

		public override string Filter
		{
			get {return _filter;}
		}
		
		#endregion
	};

    /// <summary>
    /// Represents the pseudo color file type
    /// </summary>
    public class PseudoColorFileType 
        : TextFileType
	{
		private string _filter = "Pseudo Color Files (*.pd)|*.pd";
		private string[] _extension = new string[]{".pd"};

		#region IFileType Members

		public override string[] Extension 
		{
			get {return _extension;}
		}

		public override string Filter
		{
			get {return _filter;}
		}
		
		#endregion
	}

    /// <summary>
    /// Represents the annular layout file type
    /// </summary>
    public class AnnularLayoutFileType 
        : TextFileType
	{
		//private string _filter = "RDE Analysis Layout Files (*.lyt)|*.lyt";
        private string _filter = "Analysis Layout Files (*.lyt)|*.lyt";
		private string[] _extension = new string[]{".lyt"};

		#region IFileType Members

		public override string[] Extension 
		{
			get {return _extension;}
		}

		public override string Filter
		{
			get {return _filter;}
		}
		
		#endregion
	}

    /// <summary>
    /// Represents the mask file type
    /// </summary>
    public class MaskFileType 
        : TextFileType
	{
		//private string _filter = "RDE Mask Files (*.msk)|*.msk";
        private string _filter = "Region Files (*.rgn)|*.rgn";
		private string[] _extension = new string[]{".rgn"};

		#region IFileType Members

		public override string[] Extension 
		{
			get {return _extension;}
		}

		public override string Filter
		{
			get {return _filter;}
		}

        public override OpenFileDialog OpenFileDialog(string title)
        {
            string keyWord = this.GetType().FullName;
            IFileType[] fileTypes = new IFileType[] { this };
            return CommonDialogs.OpenFileDialog(keyWord, title, fileTypes, -1, null, null);
        }

        public override SaveFileDialog SaveFileDialog(string title)
        {
            string keyWord = this.GetType().FullName;
            IFileType[] fileTypes = new IFileType[] { this };
            return CommonDialogs.SaveFileDialog(keyWord, title, fileTypes, -1, null, null);
        }
		
		#endregion
	}

    /// <summary>
    /// Represents the Csv file type
    /// </summary>
    public class CsvFileType 
        : TextFileType
	{
		private string _filter = "Comma Separated Values Files (*.csv)|*.csv";
		private string[] _extension = new string[]{".csv"};

		#region IFileType Members

		public override string[] Extension 
		{
			get {return _extension;}
		}

		public override string Filter
		{
			get {return _filter;}
		}
		
		#endregion
	}

    /// <summary>
    /// Represents the xml file type
    /// </summary>
    public class XmlFileType : TextFileType
	{
		private const string _xmlTag = "<?xml";

		private string _filter = "XML Files (*.xml)|*.xml";
		private string[] _extension = new string[]{".xml"};

		#region IFileType Members

		public override string[] Extension 
		{
			get {return _extension;}
		}

		public override string Filter
		{
			get {return _filter;}
		}
		

		public override bool IsValidFileFormat(string filePath)
		{
			using (StreamReader reader = new StreamReader(filePath, Encoding.UTF8, true))
			{
				string str = reader.ReadLine();
				return str.IndexOf(_xmlTag) >= 0;
			}
		}

		#endregion
	}

    /// <summary>
    /// Represents the notch library file type
    /// </summary>
	public class NotchLibraryFileType : FileType
	{
		private string _filter = "Notch Library Files (*.nlb)|*.nlb";
		private string[] _extension = new string[]{".nlb"};

		#region IFileType Members

		public override string[] Extension 
		{
			get {return _extension;}
		}

		public override string Filter
		{
			get {return _filter;}
		}
		
		#endregion
	}

    /// <summary>
    /// Represents the arc zone file type
    /// </summary>
    public class ArcZoneFileType : XmlFileType
	{
		private string _filter = "Arc Zone Files (*.azn)|*.azn";
		private string[] _extension = new string[]{".azn"};

		#region IFileType Members

		public override string[] Extension 
		{
			get {return _extension;}
		}

		public override string Filter
		{
			get {return _filter;}
		}
		
		#endregion
	}

    /// <summary>
    /// Represents the RDE Monitor script package file type
    /// </summary>
    public class ScriptPackageFileType 
        : FileType
	{
		private string _filter = "Script Package Files (*.pkg)|*.pkg";
		private string[] _extension = new string[]{".pkg"};

		#region IFileType Members

		public override string[] Extension 
		{
			get {return _extension;}
		}

		public override string Filter
		{
			get {return _filter;}
		}
		
		#endregion
	}

    /// <summary>
    /// Represents the object filter setting file type
    /// </summary>
    public class ObjectFilterFileType 
        : XmlFileType
	{
		private string _filter = "Object Filter Settings (*.ofs)|*.ofs";
		private string[] _extension = new string[]{".ofs"};

		#region IFileType Members

		public override string[] Extension 
		{
			get {return _extension;}
		}

		public override string Filter
		{
			get {return _filter;}
		}
		
		#endregion
	}

    /// <summary>
    /// Represents the die mask file type
    /// </summary>
	public class DieMaskFileType 
        : XmlFileType
	{
		private const string _dieMaskTag = "DieMask";
		private string _filter = "Die Mask (*.dmk)|*.dmk";
		private string[] _extension = new string[]{".dmk"};

		#region IFileType Members

		public override string[] Extension 
		{
			get {return _extension;}
		}

		public override string Filter
		{
			get {return _filter;}
		}

		
		public override bool IsValidFileFormat(string filePath)
		{
			if (!base.IsValidFileFormat(filePath))
				return false;

			try
			{
				XmlDocument document = new XmlDocument();
				document.Load(filePath);
				XmlNodeList nodes = document.GetElementsByTagName(_dieMaskTag);
				return nodes != null && nodes.Count > 0;
			}
			catch 
			{
				return false;
			}
		}
		
		#endregion
	};

    /// <summary>
    /// Represents the RDE monitor script file type
    /// </summary>
    public class RDEMonitorScriptFileType 
        : XmlFileType
    {
        private const string _scriptTag = "Script";
        //private string _filter = "RDE Monitor Script (*.srp)|*.srp|XML Files (*.xml)|*.xml";
        private string _filter = "SIA Monitor Script (*.srp)|*.srp|XML Files (*.xml)|*.xml";
        private string[] _extension = new string[] { ".srp", ".xml" };

        #region IFileType Members

        public override string[] Extension
        {
            get { return _extension; }
        }

        public override string Filter
        {
            get { return _filter; }
        }


        public override bool IsValidFileFormat(string filePath)
        {
            if (!base.IsValidFileFormat(filePath))
                return false;

            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(filePath);
                XmlNodeList nodes = document.GetElementsByTagName(_scriptTag);
                return nodes != null && nodes.Count > 0;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
