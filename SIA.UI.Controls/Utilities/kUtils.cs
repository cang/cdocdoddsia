using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Security;
using System.Text.RegularExpressions;

using SIA.Common;
using SIA.Common.Analysis;
using SIA.Common.KlarfExport;
using SIA.Common.KlarfExport.BinningLibrary;
using SIA.Common.Mathematics;
using SIA.Common.Utility;
using SIA.Common.Imaging;


namespace SIA.UI.Controls.Utilities
{

	/// <summary>
	/// Summary description for kUtils.
	/// </summary>
	public class kUtils
	{
		/// <summary>
		/// file extension of supported formats 
		/// </summary>
		protected static String[] SUPPORTED_FORMAT_EXTEND = new String[]
		{
			"BMP",
			"JPG",
			"PNG",
			"TIF",
			"GIF",
			"FIT",
			"FTS",
			"FITS",
			"RAW"
		};

		protected const String strImageFilter = "Bitmaps (*.bmp)|*.bmp|JPEG images (*.jpg)|*.jpg|PNG images (*.png)|*.png|TIF images(*.tif)|*.tif|GIF images(*.gif)|*.gif";
		protected const String strFITFilter = "|Fits Images(*.fit;*.fts;*.fits)|*.fit;*.fts;*.fits|Text Files (*.txt)|*.txt";
		protected const String strAllFilesFilter = "|Common Images(*.bmp;*.jpg;*.gif;*.png;*.tif)|*.bmp;*.jpg;*.gif;*.png;*.tif|All Files (*.*)|*.*";	
		protected const String strKLARFFilter = "KLARF (*.000)|*.000";
		
		public static OpenFileDialog kImportFile(String title)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.RestoreDirectory = true;
			if (title.Length>0) dlg.Title = title;
			dlg.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
			dlg.DefaultExt = "*.txt";
			return dlg;
		}

		public static SaveFileDialog kSaveTextFile(String title)
		{
			SaveFileDialog dlg = new SaveFileDialog();
			dlg.RestoreDirectory = true;
			if (title.Length>0) dlg.Title = title;
			dlg.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
			dlg.DefaultExt = "*.txt";
			return dlg;
		}


		public static string ValidateFileName(SaveFileDialog dialog, string filename)
		{
			if (dialog == null)
				throw new ArgumentNullException("dialog");
			if (filename == null || filename.Length == 0)
				throw new ArgumentNullException("filename");
			
			string result = "";
			string ext = Path.GetExtension(filename);

			// append extension if do not have
			if (ext == null || ext.Length <= 0)
				filename += "." + ext;
			else
			{
			}

			return result;
		}

		public static void SetDlgFilterIndex(OpenFileDialog dlg, string Value )
		{
			int nFilterIndex = Convert.ToInt32(CustomConfiguration.GetValues(Value,-1));			
			if (nFilterIndex >= 0) dlg.FilterIndex = nFilterIndex;
		}

		public static bool IsFitFile(String fileName)
		{
			if((fileName.ToUpper().EndsWith(".FITS")||(fileName.ToUpper().EndsWith(".FTS"))||(fileName.ToUpper().EndsWith(".FIT"))))
				return true;
			return false;
		}

		public static bool IsImageFile(String fileName)
		{
			////bmp;*.jpg;*.gif;*.png;*.tif
			if(fileName.ToUpper().EndsWith(".TIF") ||
				fileName.ToUpper().EndsWith(".PNG") ||
				fileName.ToUpper().EndsWith(".BMP") ||
				fileName.ToUpper().EndsWith(".JPG") ||
				fileName.ToUpper().EndsWith(".GIF") ||
				fileName.ToUpper().EndsWith(".RAW"))
				return true;
			return false;
		}

		public static eImageFormat ImageFormatFromSelectedFilter(FileDialog fileDialog)
		{
			if (fileDialog == null)
				throw new ArgumentNullException("fileDialog");

			// if filter was not selected then return Unknown
			if (fileDialog.FilterIndex < 0)
				return eImageFormat.Unknown;

			string[] filters = fileDialog.Filter.ToUpper().Split(new char[]{'|'});
			string selFilter = filters[(fileDialog.FilterIndex-1)*2+1];
			
			if (selFilter.IndexOf("BMP") >= 0)
				return eImageFormat.Bmp;
			else if (selFilter.IndexOf("JPG") >= 0)
				return eImageFormat.Jpeg;
			else if (selFilter.IndexOf("TIF") >= 0)
				return eImageFormat.Tiff;
			else if (selFilter.IndexOf("PNG") >= 0)
				return eImageFormat.Png;
			else if (selFilter.IndexOf("GIF") >= 0)
				return eImageFormat.Gif;
			else if (selFilter.IndexOf("FIT") >= 0)
				return eImageFormat.Fit;
			else if (selFilter.IndexOf("EMF") >= 0)
				return eImageFormat.Emf;
			else if (selFilter.IndexOf("EXIF") >= 0)
				return eImageFormat.Exif;
			else if (selFilter.IndexOf("WMF") >= 0)
				return eImageFormat.Wmf;	
			else if (selFilter.IndexOf("RAW") >= 0)
				return eImageFormat.Raw;

			return eImageFormat.Unknown;
		}

		public static eImageFormat ImageFormatFromFileName(String fileName)
		{
			if (fileName.ToUpper().EndsWith(".FIT") || fileName.ToUpper().EndsWith(".FITS"))
				return eImageFormat.Fit;
			else if (fileName.ToUpper().EndsWith(".TIF"))
				return eImageFormat.Tiff;
			else if (fileName.ToUpper().EndsWith(".PNG"))
				return eImageFormat.Png;
			else if (fileName.ToUpper().EndsWith(".BMP"))
				return eImageFormat.Bmp;
			else if (fileName.ToUpper().EndsWith(".JPG"))
				return eImageFormat.Jpeg;
			else if (fileName.ToUpper().EndsWith(".GIF"))
				return eImageFormat.Gif;
			else if (fileName.ToUpper().EndsWith(".RAW"))
				return eImageFormat.Raw;
			else
				return eImageFormat.Unknown;
		}

		public static System.Drawing.Imaging.ImageFormat BitmapFormatFromFileName(string fileName)
		{
			if (fileName.ToUpper().EndsWith(".TIF"))
				return System.Drawing.Imaging.ImageFormat.Tiff;
			else if (fileName.ToUpper().EndsWith(".PNG"))
				return System.Drawing.Imaging.ImageFormat.Png;
			else if (fileName.ToUpper().EndsWith(".BMP"))
				return System.Drawing.Imaging.ImageFormat.Bmp;
			else if (fileName.ToUpper().EndsWith(".JPG"))
				return System.Drawing.Imaging.ImageFormat.Jpeg;
			else if (fileName.ToUpper().EndsWith(".GIF"))
				return System.Drawing.Imaging.ImageFormat.Gif;
			else
				return null;
		}

		public static bool IsSupportedFileFormat(String filename)
		{
			for (int i=0; i<SUPPORTED_FORMAT_EXTEND.Length; i++)
				if (filename.ToUpper().EndsWith(SUPPORTED_FORMAT_EXTEND[i]))
					return true;
			return false;
		}

		public static bool CompareImageSize(SIA.SystemLayer.CommonImage firstImage,SIA.SystemLayer.CommonImage lastImage)
		{
			if (firstImage.Width != lastImage.Width || firstImage.Height != lastImage.Height)
				return false;
			return true;
		}

        static Cursor savedCursor = null;
		
		public static void kBeginWaitCursor()
		{
            savedCursor = Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;
		}

		public static void kEndWaitCursor()
		{
            if (Cursor.Current != savedCursor)
                Cursor.Current = savedCursor;
		}

		
		public static bool IsValidFilePath(String path)
		{
			return (System.IO.Path.GetExtension(path).Length > 0);
		}

		public static bool IsFileExists(String filename)
		{
			if (filename.Length > 0)
				return (System.IO.File.Exists(filename));
			return false;
		}


		public static void SetMinMaxToAll(Control control,Decimal Min,Decimal Max)
		{
			foreach(Control ctrl in  control.Controls )
			{
				if ( ctrl.GetType() == typeof( NumericUpDown ) )
				{
					((NumericUpDown)(ctrl)).Minimum = Min;
					((NumericUpDown)(ctrl)).Maximum = Max;
				}
				if( ctrl.Controls.Count > 0) SetMinMaxToAll( ctrl,Min,Max );
			}
		}

		public static void SetMinMaxToOne(NumericUpDown ctrl,Decimal Min,Decimal Max)
		{			
			((NumericUpDown)(ctrl)).Minimum = Min;
			((NumericUpDown)(ctrl)).Maximum = Max;				
		}

		public static void SetMinMax(NumericUpDown ctrl,SIA.SystemLayer.CommonImage Data)
		{			
			if(Data !=null && Data.RasterImage!=null)
			{
				ctrl.Minimum =(Decimal) Data.RasterImage.MINGRAYVALUE;
				ctrl.Maximum =(Decimal) Data.RasterImage.MAXGRAYVALUE;

				if(ctrl.Value > (Decimal)Data.RasterImage.MAXGRAYVALUE)
					ctrl.Value=(Decimal)Data.RasterImage.MAXGRAYVALUE;
				else if(ctrl.Value < (Decimal)Data.RasterImage.MINGRAYVALUE)
					ctrl.Value=(Decimal)Data.RasterImage.MINGRAYVALUE;
			}		
		}
		
		public static bool IsAllValueValidate(Control control)
		{
			NumericUpDown nd;
			string values;
			foreach(Control ctrl in  control.Controls )
			{			
				if ( ctrl.GetType() == typeof( NumericUpDown ) )
				{
					nd		= (NumericUpDown)ctrl;
					values	= nd.Text;

					try
					{
						if ( Decimal.Parse ( values ) < nd.Minimum || Decimal.Parse ( values ) > nd.Maximum )
						{
							nd.Select();
							nd.Select( 0,nd.Text.Length );

							// show warning message
							MessageBoxEx.Warning("Please enter a value between " + nd.Minimum.ToString() + " and " + nd.Maximum.ToString());

							// makes the control has focus
							nd.Focus();

							return false;
						}
					}
					catch( Exception ex )
					{
						MessageBoxEx.Error(ex.Message);
						return false;
					}
					
				}				
			}
			return true;
		}

		public static bool IsInputValueValidate(NumericUpDown ctrl )
		{			
			string values;			
			values	= ctrl.Text;
			try
			{
				if ( Decimal.Parse ( values ) < ctrl.Minimum || Decimal.Parse ( values ) > ctrl.Maximum )
				{
					ctrl.Select();
					ctrl.Select( 0,ctrl.Text.Length );

					// show warning message
					MessageBoxEx.Warning("Please enter a value between " + ctrl.Minimum.ToString() + " and " + ctrl.Maximum.ToString());

					// makes the control has focus
					ctrl.Focus();

					return false;
				}			
			} 
			catch ( Exception ex)
			{  
				MessageBoxEx.Error(ex.Message);
				return false;
			}
			
			return true;
		}
		public static string convertFloatPoint(double val)
		{
			string result;
			//			double minFloat = 0.00009;
			double scale = 10000;
			double scaledVal = val*scale;
			scaledVal = (int)(scaledVal +0.5);
			scaledVal /= scale;	
			double AbsoluteVal = Math.Abs(val);
			if(AbsoluteVal >= 1/scale)
			{
				result = scaledVal .ToString();
			}
			else
			{
				result = ((double)val).ToString("E0");
			}
			return result;
		}

		/// <summary>
		/// Returns a value indicating whether the specified path is properly formatted.
		/// </summary>
		/// <param name="path">A <see cref="String"/> containing the path to check.</param>
		/// <returns><see langword="true"/> if <paramref name="path"/> is properly formatted; otherwise, <see langword="false"/>.</returns>
		public static bool IsPathValid(string path)
		{
			if (path == null || path.Trim().Length == 0)
				return false;
			try
			{
				Path.GetFullPath(path);
                char[] invalidChars = Path.GetInvalidFileNameChars();
				if (path.IndexOfAny(invalidChars) >= 0)
					return false;

				return true;
			}
			catch (ArgumentException)
			{
			}
			catch (SecurityException)
			{
			}
			catch (NotSupportedException)
			{
			}
			catch (PathTooLongException)
			{
			}
			return false;
		}


		/// <summary>
		/// Returns a value indicating whether the specified path is properly formatted with specified extensions.
		/// </summary>
		/// <param name="pathfile">A <see cref="String"/> containing the path to check.</param>
		/// <param name="types">A <see cref="String"/> containing the extensions</param>
		/// <returns><see langword="true"/> if <paramref name="path"/> is properly formatted; otherwise, <see langword="false"/>.</returns>
		public static bool IsPathValid(string pathfile, string types) 
		{
			// if the email is null, then return to avoid error
			if (pathfile == null)
				return false;

			// build the file type string from "txt,doc" to "txt|TXT|doc|DOC"
			string fileTypesRegexString = "";
			string[] fileTypes = types.Split(',');
			for (int x=0; x<fileTypes.Length; x++) 
			{
				fileTypesRegexString += fileTypes[x] + "|";
				fileTypesRegexString += fileTypes[x].ToUpper();
				
				// put a separator between all but last item
				if (x+1 != fileTypes.Length) 
				{
					fileTypesRegexString += "|";
				}
			}

			string pattern = @"^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w ]*))+\.(" + fileTypesRegexString + ")$";
			
			Regex regEx = new Regex(pattern);
			Match match = regEx.Match(pathfile);
			return match.Success;
		}
	}
}
