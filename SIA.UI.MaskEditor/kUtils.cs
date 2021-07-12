using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using SIA.Common;
using SIA.Common.Utility;
using SIA.Common.Imaging; 

namespace SIA.UI.MaskEditor
{
	internal enum MESSAGEBOX_TYPE
	{
		ERROR = 0,
		WARNING,
		INFORMATION,		
	}

	/// <summary>
	/// Summary description for kUtils.
	/// </summary>
	internal class kUtils
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
			"FITS"
		};
		protected const String strImageFilter = "Bitmaps (*.bmp)|*.bmp|JPEG images (*.jpg)|*.jpg|PNG images (*.png)|*.png|TIF images(*.tif)|*.tif|GIF images(*.gif)|*.gif";
		protected const String strFITFilter = "|Fits Images(*.fit;*.fts;*.fits)|*.fit;*.fts;*.fits|Text Files (*.txt)|*.txt";
		protected const String strAllFilesFilter = "|Common Images(*.bmp;*.jpg;*.gif;*.png;*.tif)|*.bmp;*.jpg;*.gif;*.png;*.tif|All Files (*.*)|*.*";	
		protected const String strKLARFFilter = "KLARF (*.000)|*.000";
		//	public static String strOpenImageDefaultExt = "";
		
		public static OpenFileDialog kOpenImageFiles(String title, bool bIncludeFIT)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			String strFilter = strImageFilter;
			dlg.RestoreDirectory = true;			
			if (bIncludeFIT) strFilter += strFITFilter;

			if (title == "Select mask file")
				strFilter += "|Mask files (*.msk)|*.msk";

			strFilter += strAllFilesFilter;
			dlg.Filter = strFilter;
			if (title.Length > 0) dlg.Title = title;
			
			if (bIncludeFIT) 
			{
				dlg.FilterIndex = 6 ;
				//dlg.DefaultExt = "*.fit;*.fts;*.fits"; 
			}
			else 
			{
				dlg.FilterIndex = 7;
				//dlg.DefaultExt = "*.bmp";
			}
			
			return dlg;
		}
		public static OpenFileDialog kOpenKLARFFiles(String title, bool bIncludeFIT)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			String strFilter = strKLARFFilter;
			dlg.RestoreDirectory = true;						
			dlg.Filter = strFilter;
			if (title.Length > 0) dlg.Title = title;
					
			
			return dlg;
		}
		public static string CorrectFileNameWithExtern(int index,string filename)
		{
			String efn = System.IO.Path.GetExtension(filename);

			//Check FITS
			if(index==6)
			{
				if( efn.ToUpper()!=".FIT" && efn.ToUpper()!=".FITS")
				{
					return filename + ".FIT";
				}
			}
			return filename;
		}

		public static void SetDlgFilterIndex(OpenFileDialog dlg, string Value )
		{
			int nFilterIndex = Convert.ToInt32(CustomConfiguration.GetValues(Value,-1));			
			if (nFilterIndex >= 0) dlg.FilterIndex = nFilterIndex;
		}

		public static SaveFileDialog kSaveImageFiles(String title, bool bIncludeFIT)
		{
			SaveFileDialog dlg = new SaveFileDialog();
			String strFilter = strImageFilter;
			dlg.RestoreDirectory = true;
			
			if (bIncludeFIT) 
				strFilter += strFITFilter;
			strFilter += strAllFilesFilter;
			dlg.Filter = strFilter;
			
			if (title.Length > 0) dlg.Title = title;
			if (bIncludeFIT) 
			{
				//dlg.DefaultExt = "*.fit"; 
				dlg.FilterIndex = 6 ;
			}
			else 
				dlg.DefaultExt = "*.bmp";
			return dlg;
		}

		public static OpenFileDialog kOpenTextFile(String title)
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

		public static OpenFileDialog kImportFile(String title)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.RestoreDirectory = true;
			if (title.Length>0) dlg.Title = title;
			dlg.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
			dlg.DefaultExt = "*.txt";
			return dlg;
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
				fileName.ToUpper().EndsWith(".GIF"))
				return true;
			return false;
		}

		public static eImageFormat ImageFormatFromFileName(String fileName)
		{
			if (fileName.ToUpper().EndsWith(".TIF"))
				return eImageFormat.Tiff;
			else if (fileName.ToUpper().EndsWith(".PNG"))
				return eImageFormat.Png;
			else if (fileName.ToUpper().EndsWith(".BMP"))
				return eImageFormat.Bmp;
			else if (fileName.ToUpper().EndsWith(".JPG"))
				return eImageFormat.Jpeg;
			else if (fileName.ToUpper().EndsWith(".GIF"))
				return eImageFormat.Gif;
			else
				return eImageFormat.Unknown;
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

		public static DialogResult ShowMessage(System.Windows.Forms.IWin32Window sender, string message, MESSAGEBOX_TYPE type)
		{
			DialogResult result = DialogResult.OK;
			string title = "SiGlaz Image Analyzer";
			switch (type)
			{
				case MESSAGEBOX_TYPE.ERROR :
					result = MessageBox.Show (sender,message,title,MessageBoxButtons.OK,MessageBoxIcon.Error );
					break;
				case MESSAGEBOX_TYPE.WARNING :
					result = MessageBox.Show (sender,message,title,MessageBoxButtons.OK,MessageBoxIcon.Warning );
					break;
				case MESSAGEBOX_TYPE.INFORMATION :
					result = MessageBox.Show (sender,message,title,MessageBoxButtons.OK,MessageBoxIcon.Information );					
					break;
			}
			return result;
		}

		public static DialogResult ShowMessage(System.Windows.Forms.IWin32Window sender, string message, MESSAGEBOX_TYPE type, MessageBoxButtons buttons)
		{
			DialogResult result = DialogResult.OK;
			string title = "SiGlaz Image Analyzer";
			switch (type)
			{
				case MESSAGEBOX_TYPE.ERROR :
					result = MessageBox.Show (sender, message, title, buttons, MessageBoxIcon.Error);
					break;
				case MESSAGEBOX_TYPE.WARNING :
					result = MessageBox.Show (sender, message, title, buttons, MessageBoxIcon.Warning );
					break;
				case MESSAGEBOX_TYPE.INFORMATION :
					result = MessageBox.Show (sender, message, title, buttons, MessageBoxIcon.Information );					
					break;
			}
			return result;
		}

		public static void kBeginWaitCursor()
		{
			Cursor.Current = Cursors.WaitCursor;
		}

		public static void kEndWaitCursor()
		{
			Cursor.Current = Cursors.Arrow;
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

							kUtils.ShowMessage( nd,"Please enter a value between " + nd.Minimum.ToString() + " and " + nd.Maximum.ToString(),MESSAGEBOX_TYPE.WARNING );

							return false;
						}
					}
					catch( Exception ex )
					{
						kUtils.ShowMessage( ctrl,ex.Message,MESSAGEBOX_TYPE.WARNING );
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

					kUtils.ShowMessage( ctrl,"Please enter a value between " + ctrl.Minimum.ToString() + " and " + ctrl.Maximum.ToString(),MESSAGEBOX_TYPE.WARNING );

					return false;
				}			
			} 
			catch ( Exception ex)
			{  
					kUtils.ShowMessage( ctrl,ex.Message,MESSAGEBOX_TYPE.WARNING );
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
			else{
				result = ((double)val).ToString("E0");
			}
			return result;
		}
	}
}
