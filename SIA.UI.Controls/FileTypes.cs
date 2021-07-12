using System;
using System.Collections;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

using SIA.Common;
using SIA.UI.Controls.Utilities;
using System.Windows.Forms;
using SIA.UI.Controls.Dialogs;

namespace SIA.UI.Controls
{
	/// <summary>
	/// Factory class provides built-in file types
	/// </summary>
	public sealed class FileTypes
	{
		public readonly static IFileType Bitmap = new BitmapFileType();
		public readonly static IFileType Jpeg = new JpegFileType();
		public readonly static IFileType Png = new PngFileType();
		public readonly static IFileType Tiff = new TiffFileType();
		public readonly static IFileType Gif = new GifFileType();
		public readonly static IFileType Raw = new RawFileType();
		public readonly static IFileType Fits = new FitsFileType();

		public readonly static IFileType Text = new TextFileType();
		public readonly static IFileType Klarf = new KlarfFileType();
		public readonly static IFileType PseudoColor = new PseudoColorFileType();
		public readonly static IFileType AnnularLayout = new AnnularLayoutFileType();
		public readonly static IFileType Mask = new MaskFileType();
		public readonly static IFileType Csv = new CsvFileType();
		public readonly static IFileType Xml = new XmlFileType();
		public readonly static IFileType NotchLibrary = new NotchLibraryFileType();
		public readonly static IFileType ArcZone = new ArcZoneFileType();
		public readonly static IFileType ScriptPackage = new ScriptPackageFileType();
		public readonly static IFileType ObjectFilterSettings = new ObjectFilterFileType();
		public readonly static IFileType DieMask = new DieMaskFileType();
        public readonly static IFileType RDEScript = new RDEMonitorScriptFileType();

		public readonly static IFileType[] ImageFileTypes = null;
		public readonly static IFileType[] GdiPlusFileTypes = null;
		public readonly static IFileType[] AllFileTypes = null;
        public readonly static IFileType[] TextFileTypes = null;
        public readonly static IFileType[] ReportTextCsvFileTypes = null;
        public readonly static IFileType[] CsvFileTypes = null;

		static FileTypes()
		{
			FileTypes.GdiPlusFileTypes = new IFileType[] 
			{
				FileTypes.Bitmap, 
				FileTypes.Jpeg, 
				FileTypes.Png, 
				FileTypes.Tiff,
				FileTypes.Gif,
			};

			FileTypes.ImageFileTypes = new IFileType[]
			{
				FileTypes.Bitmap, 
				FileTypes.Jpeg, 
				FileTypes.Png, 
				FileTypes.Tiff,
				FileTypes.Gif,
				FileTypes.Raw,
				FileTypes.Fits,
			};

			FileTypes.AllFileTypes = new IFileType[]
			{
				FileTypes.Bitmap, 
				FileTypes.Jpeg, 
				FileTypes.Png, 
				FileTypes.Tiff,
				FileTypes.Gif,
				FileTypes.Raw,
				FileTypes.Fits,

				FileTypes.Text,
				FileTypes.Klarf,
				FileTypes.PseudoColor,
				FileTypes.AnnularLayout,
				FileTypes.Mask,
				FileTypes.Csv,
				FileTypes.Xml,
				FileTypes.NotchLibrary,
				FileTypes.ArcZone,
				FileTypes.ScriptPackage,
				FileTypes.ObjectFilterSettings,
                FileTypes.DieMask,
                FileTypes.RDEScript,
			};

            FileTypes.TextFileTypes = new IFileType[] { FileTypes.Text };

            FileTypes.CsvFileTypes = new IFileType[] { FileTypes.Csv};

            FileTypes.ReportTextCsvFileTypes = new IFileType[] { FileTypes.Csv, FileTypes.Text };
		}

		public static IFileType GetFileType(eImageFormat format)
		{
			foreach (ImageFileType fileType in ImageFileTypes)
			{
				if (fileType.ImageFormat == format)
					return fileType;
			}

			return null;
		}

		public static ImageFormat ToImageFormat(eImageFormat format)
		{
			switch (format)
			{
				case eImageFormat.Bmp:
					return ImageFormat.Bmp;
				case eImageFormat.Jpeg:
					return ImageFormat.Jpeg;
				case eImageFormat.Png:
					return ImageFormat.Png;
				case eImageFormat.Tiff:
					return ImageFormat.Tiff;
				case eImageFormat.Gif:
					return ImageFormat.Gif;
				default:
					throw new NotSupportedException(String.Format("Image format {0} was not support", format));
			}
		}

		public static eImageFormat ToRdeImageFormat(ImageFormat format)
		{
			if (format == ImageFormat.Bmp)
				return eImageFormat.Bmp;
			else if (format == ImageFormat.Jpeg)
				return eImageFormat.Jpeg;
			else if (format == ImageFormat.Png)
				return eImageFormat.Png;
			else if (format == ImageFormat.Tiff)
				return eImageFormat.Tiff;
			else if (format == ImageFormat.Gif)
				return eImageFormat.Gif;
				
			throw new NotSupportedException(String.Format("Image format {0} was not support", format.GetType().FullName));
		}


        public static OpenImageFile OpenImageFileDialog(string title)
        {
            return OpenImageFileDialog(title, string.Empty);
        }

        public static OpenImageFile OpenImageFileDialog(string title, string filePath)
        {
            string keyWord = "FileTypes.OpenImageFileDialog";
            IFileType[] fileTypes = FileTypes.ImageFileTypes;

            string initDir = string.Empty;
            string fileName = string.Empty;

            if (filePath != null && filePath != string.Empty)
                initDir = Path.GetDirectoryName(filePath);
            if (filePath != null && filePath != string.Empty)
                fileName = Path.GetFileName(filePath);

            return CommonDialogs.OpenImageFileDialog(keyWord, title, fileTypes, -1, initDir, fileName);
        }
	}
}
