using System;
using System.Data;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Diagnostics;

using SIA.Common;
using SIA.Common.Profiles;
using SIA.Common.Native;
using SIA.UI.Controls.Dialogs;

namespace SIA.UI.Controls.Utilities
{
	/// <summary>
	/// Indicates type of message box
	/// </summary>
	public enum MessageBoxType
	{
		Error = 0,
		Warning,
		Information,		
	}

	internal abstract class FileDialogEx 
        : IDisposable
	{
		private const string entryFilterIndex = "FilterIndex";
		private const string entryLastFolder = "InitialDirectory";

		private FileDialog _fileDialog = null;
		private string _keyWord = "";
		private XmlProfile _profile = null;	

		public string KeyWord
		{
			get {return _keyWord;}
		}

		public FileDialog FileDialog
		{
			get {return _fileDialog;}
		}

		public virtual string CachedFileName
		{
			get
			{
				return Application.LocalUserAppDataPath + @"\" + this.GetType().FullName + ".config.xml";
			}
		}

		public XmlProfile XmlProfile
		{
			get {return _profile;}
		}

		protected abstract FileDialog CreateFileDialog();
	
		public FileDialogEx(string keyWord)
		{
			if (keyWord != null)
			{
				string fileName = this.CachedFileName;
				_profile = new XmlProfile(fileName);
				_keyWord = keyWord;
			}

			// create file dialog instance
			_fileDialog = this.CreateFileDialog();

            // initialize built-in properties
            _fileDialog.SupportMultiDottedExtensions = true;
			
			// register for events
			_fileDialog.FileOk += new CancelEventHandler(dlg_FileOk);
			_fileDialog.Disposed +=new EventHandler(dlg_Disposed);

			// deserialize settings
			this.OnDeserializeSettings();
		}

		~FileDialogEx()
		{		
			this.Dispose(false);
		}

		#region IDisposable Members

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			_fileDialog = null;
			_profile = null;
		}

		#endregion

		private void dlg_FileOk(object sender, CancelEventArgs e)
		{
			// serialize settings		
			this.OnSerializeSettings();
		}

		private void dlg_Disposed(object sender, EventArgs e)
		{
			// unregister for events
			_fileDialog.FileOk -= new CancelEventHandler(dlg_FileOk);
			_fileDialog.Disposed -=new EventHandler(dlg_Disposed);

			// clean up unmanaged resource
			this.Dispose(false);
		}

		protected virtual void OnSerializeSettings()
		{
			if (_profile != null)
			{
				_profile.SetValue(_keyWord, entryFilterIndex, _fileDialog.FilterIndex);
				_profile.SetValue(_keyWord, entryLastFolder, Path.GetDirectoryName(_fileDialog.FileName));
			}
		}

		protected virtual void OnDeserializeSettings()
		{
			if (_profile != null)
			{
				// deserialize settings
				int filterIndex = Convert.ToInt32(_profile.GetValue(_keyWord, entryFilterIndex, 0));
				string lastFolder = _profile.GetValue(_keyWord, entryLastFolder, string.Empty);

				// restore last selected value
				_fileDialog.FilterIndex = filterIndex;
				_fileDialog.InitialDirectory = lastFolder;
			}
		}
	}


	internal class OpenFileDialogEx
        : FileDialogEx
	{
		protected override FileDialog CreateFileDialog()
		{
			return new OpenFileDialog();
		}

		public OpenFileDialogEx(string keyWord) : base(keyWord)
		{
		}
	}

	internal class SaveFileDialogEx 
        : FileDialogEx
	{
		protected override FileDialog CreateFileDialog()
		{
			return new SaveFileDialog();
		}

		public SaveFileDialogEx(string keyWord) : base(keyWord)
		{
		}
	}

	internal class OpenImageFileDialogEx 
        : OpenFileDialogEx
	{
		internal string entryFolderViewMode = "FolderViewMode";
		internal string entryPreview = "Preview";

		internal OpenImageFile CustomizedOpenFileDialog = null;

		protected override FileDialog CreateFileDialog()
		{
			OpenFileDialog dlg = new OpenFileDialog();
			this.CustomizedOpenFileDialog = new OpenImageFile(dlg);
			return dlg;
		}

		public OpenImageFileDialogEx(string keyWord) 
			: base(keyWord)
		{
		}

		protected override void OnSerializeSettings()
		{
			base.OnSerializeSettings ();

			XmlProfile profile = this.XmlProfile;
			profile.SetValue(this.KeyWord, entryFolderViewMode, (int)CustomizedOpenFileDialog.ViewMode);
			profile.SetValue(this.KeyWord, entryPreview, CustomizedOpenFileDialog.Preview);
		}


		protected override void OnDeserializeSettings()
		{
			base.OnDeserializeSettings ();

			XmlProfile profile = this.XmlProfile;
			FolderViewMode viewMode = (FolderViewMode)profile.GetValue(this.KeyWord, entryFolderViewMode, (int)FolderViewMode.Default);
			this.CustomizedOpenFileDialog.ViewMode = viewMode;
			bool preview = (bool)profile.GetValue(this.KeyWord, entryPreview, true);
			this.CustomizedOpenFileDialog.Preview = preview;
		}

	}

	/// <summary>
	/// The factory class provides functionality for open, save and message box dialogs
	/// </summary>
	public class CommonDialogs
	{
		[Flags]
		public enum ImageFileFilter : long
		{
			CommonImage = 1,
			FITImage = 2,
			TextFile = 4,
			AllSupportedImageFormat = CommonImage | FITImage | TextFile
		};
	
		
		#region Internal Constants

		internal const string BitmapFileFilter = "Bitmaps (*.bmp)|*.bmp";
		internal const string JpegFileFilter = "JPEG images (*.jpg)|*.jpg";
		internal const string PnjFileFilter = "PNG images (*.png)|*.png";
		internal const string TifFileFilter = "TIFF images (*.tif;*.tiff)|*.tif;*.tiff";
		internal const string GifFileFilter = "GIF images (*.gif)|*.gif";
		internal const string RawFileFilter = "Raw images (*.raw)|*.raw";

		internal const string AllFileFilter = "All Files (*.*)|*.*";
#if DEBUG
		internal const string CommonImageFilter = "Bitmaps (*.bmp)|*.bmp|JPEG images (*.jpg)|*.jpg|PNG images (*.png)|*.png|TIFF images (*.tif;*.tiff)|*.tif;*.tiff|GIF images(*.gif)|*.gif|Raw images (*.raw)|*.raw|Common Images(*.bmp;*.jpg;*.gif;*.png;*.tif)|*.bmp;*.jpg;*.gif;*.png;*.tif";
#else
		internal const string CommonImageFilter = "Bitmaps (*.bmp)|*.bmp|JPEG images (*.jpg)|*.jpg|PNG images (*.png)|*.png|TIF images(*.tif)|*.tif|GIF images(*.gif)|*.gif|Common Images(*.bmp;*.jpg;*.gif;*.png;*.tif)|*.bmp;*.jpg;*.gif;*.png;*.tif";
#endif
		internal const string FitFileFilter = "Fits Images(*.fits;*.fit;*.fts)|*.fits;*.fit;*.fts";
		internal const string TextFileFilter = "Text Files (*.txt)|*.txt";
		internal const string KlarfFileFilter = "KLARF (*.000)|*.000";
		internal const string PseudoColorFileFilter = "Pseudo Color Files (*.pd)|*.pd";
        //internal const string LayoutFileFilter = "RDE Analysis Layout Files (*.lyt)|*.lyt";
        //internal const string MaskFileFilter = "RDE Mask Files (*.msk)|*.msk";
        internal const string LayoutFileFilter = "Analysis Layout Files (*.lyt)|*.lyt";
        internal const string MaskFileFilter = "Mask Files (*.msk)|*.msk";
		internal const string CsvFileFilter = "Comma Separated Values Files (*.csv)|*.csv";
		internal const string XmlFileFilter = "XML Files (*.xml)|*.xml";

		internal const string NotchLibFileFileter = "Notch Library Files (*.nlb)|*.nlb";
		internal const string ArcZoneFileFilter = "Arc Zone Files (*.azn)|*.azn";
		internal const string PackageFileFilter = "Package Files (*.pkg)|*.pkg";
		internal const string ObjectFilterFilter = "Object Filter Settings (*.ofs)|*.ofs";

		#endregion

		private static int _OpenKlarfFileDialog_lastSelectedFilterType = -1;
		private static int _SaveKlarfFileDialog_lastSelectedFilterType = -1;

		public static string DefaultFolder = string.Empty;

		#region Constructor and destructors

		static CommonDialogs()
		{
			
		}

		#endregion

		#region MessageBox Helpers
		
		private static DialogResult ShowMessage(System.Windows.Forms.IWin32Window sender, string message, MessageBoxType type)
		{
			DialogResult result = DialogResult.OK;
			string title = Application.ProductName;//"SiGlaz Image Analyzer";
			switch (type)
			{
				case MessageBoxType.Error:
					result = System.Windows.Forms.MessageBox.Show (sender,message,title,MessageBoxButtons.OK,MessageBoxIcon.Error );
					break;
				case MessageBoxType.Warning:
					result = System.Windows.Forms.MessageBox.Show (sender,message,title,MessageBoxButtons.OK,MessageBoxIcon.Warning );
					break;
				case MessageBoxType.Information:
					result = System.Windows.Forms.MessageBox.Show (sender,message,title,MessageBoxButtons.OK,MessageBoxIcon.Information );					
					break;
			}
			return result;
		}

		private static DialogResult ShowMessage(System.Windows.Forms.IWin32Window sender, string message, MessageBoxType type, MessageBoxButtons buttons)
		{
			DialogResult result = DialogResult.OK;
			string title = Application.ProductName;//"SiGlaz Image Analyzer";
			switch (type)
			{
				case MessageBoxType.Error:
					result = System.Windows.Forms.MessageBox.Show (sender, message, title, buttons, MessageBoxIcon.Error);
					break;
				case MessageBoxType.Warning:
					result = System.Windows.Forms.MessageBox.Show (sender, message, title, buttons, MessageBoxIcon.Warning );
					break;
				case MessageBoxType.Information:
					result = System.Windows.Forms.MessageBox.Show (sender, message, title, buttons, MessageBoxIcon.Information );					
					break;
			}
			return result;
		}

		private static DialogResult ShowMessage(System.Windows.Forms.IWin32Window sender, string message, string title,  MessageBoxType type, MessageBoxButtons buttons)
		{
			DialogResult result = DialogResult.OK;
			switch (type)
			{
				case MessageBoxType.Error:
					result = System.Windows.Forms.MessageBox.Show (sender, message, title, buttons, MessageBoxIcon.Error);
					break;
				case MessageBoxType.Warning:
					result = System.Windows.Forms.MessageBox.Show (sender, message, title, buttons, MessageBoxIcon.Warning );
					break;
				case MessageBoxType.Information:
					result = System.Windows.Forms.MessageBox.Show (sender, message, title, buttons, MessageBoxIcon.Information );					
					break;
			}
			return result;
		}

		
		#endregion

		#region Open and SaveFileDialog helpers

		public static OpenFileDialog OpenFileDialog(string keyWord, string title, string filter, int filterIndex, string initDir, string fileName)
		{
			FileDialogEx dlgEx = new OpenFileDialogEx(keyWord);
			OpenFileDialog dlg = (OpenFileDialog)dlgEx.FileDialog;

            // apply default folder
			if (CommonDialogs.DefaultFolder != null && CommonDialogs.DefaultFolder != string.Empty)
				dlg.InitialDirectory = CommonDialogs.DefaultFolder;

			if (title != null && title != string.Empty)
				dlg.Title = title;
			else
				dlg.Title = Application.ProductName;
			
			if (filter != null && filter != string.Empty)
				dlg.Filter = filter;
			else
				dlg.Filter = AllFileFilter;

			if (filterIndex >= 0)
				dlg.FilterIndex = filterIndex;
			
			if (initDir != null && initDir != string.Empty)
				dlg.InitialDirectory = initDir;

			if (fileName != null && fileName != string.Empty)
				dlg.FileName = fileName;

			return dlg;
		}
		
		public static SaveFileDialog SaveFileDialog(string keyWord, string title, string filter, int filterIndex, string initDir, string fileName)
		{
			FileDialogEx dlgEx = new SaveFileDialogEx(keyWord);
			SaveFileDialog dlg = (SaveFileDialog)dlgEx.FileDialog;

            // apply default folder
			if (CommonDialogs.DefaultFolder != null && CommonDialogs.DefaultFolder != string.Empty)
				dlg.InitialDirectory = CommonDialogs.DefaultFolder;

			if (title != null && title != string.Empty)
				dlg.Title = title;
			else
				dlg.Title = Application.ProductName;
			
			if (filter != null && filter != string.Empty)
				dlg.Filter = filter;
			else
				dlg.Filter = AllFileFilter;

			if (filterIndex >= 0)
				dlg.FilterIndex = filterIndex;

			if (initDir != null && initDir != string.Empty)
				dlg.InitialDirectory = initDir;

			if (fileName != null && fileName != string.Empty)
				dlg.FileName = fileName;

			return dlg;
		}


        public static OpenImageFile OpenImageFileDialog(string keyWord, string title, IFileType[] fileTypes, 
            int filterIndex, string initDir, string fileName)
        {
            OpenImageFileDialogEx oif = new OpenImageFileDialogEx(keyWord);
            OpenFileDialog dlg = oif.CustomizedOpenFileDialog.OpenDialog;

            // set dialog title
            dlg.Title = title;

            // apply default folder if initial directory was not set
            if (dlg.InitialDirectory == null || dlg.InitialDirectory == string.Empty)
            {
                if (CommonDialogs.DefaultFolder != null && CommonDialogs.DefaultFolder != string.Empty)
                    dlg.InitialDirectory = CommonDialogs.DefaultFolder;
            }

            if (title != null && title != string.Empty)
                dlg.Title = title;
            else
                dlg.Title = Application.ProductName;

            // create file type filters
            string filter = MakeFilter(fileTypes);
            if (filter != null && filter != string.Empty)
                dlg.Filter = filter;
            else
                dlg.Filter = AllFileFilter;

            if (filterIndex >= 0)
                dlg.FilterIndex = filterIndex;
            else
            {
                //int fisIndex = -1;
                //for (int i = 0; i < fileTypes.Length; i++)
                //{
                //    if (fileTypes[i] is FitsFileType)
                //    {
                //        fisIndex = i + 1;
                //        break;
                //    }
                //}

                //dlg.FilterIndex = fisIndex;

                // set as common image type
                // commonImageIndex = fileTypes.Length (base-0)
                // allFileIndex = fileTypes.Length+1 (base-0)
                int commonImageIndex = (fileTypes.Length) + 1; // base-1
                dlg.FilterIndex = commonImageIndex;
            }

            if (initDir != null && initDir != string.Empty)
                dlg.InitialDirectory = initDir;

            if (fileName != null && fileName != string.Empty)
                dlg.FileName = fileName;

            return oif.CustomizedOpenFileDialog;
        }
        
        public static OpenFileDialog OpenFileDialog(string keyWord, string title, IFileType[] fileTypes, int filterIndex, string initDir, string fileName)
		{
			FileDialogEx dlgEx = new OpenFileDialogEx(keyWord);
			OpenFileDialog dlg = (OpenFileDialog)dlgEx.FileDialog;

            // apply default folder if initial directory was not set
            if (dlg.InitialDirectory == null || dlg.InitialDirectory == string.Empty)
            {
                if (CommonDialogs.DefaultFolder != null && CommonDialogs.DefaultFolder != string.Empty)
                    dlg.InitialDirectory = CommonDialogs.DefaultFolder;
            }

			if (title != null && title != string.Empty)
				dlg.Title = title;
			else
				dlg.Title = Application.ProductName;
			
			// create file type filters
			string filter = MakeFilter(fileTypes);
			if (filter != null && filter != string.Empty)
				dlg.Filter = filter;
			else
				dlg.Filter = AllFileFilter;

			if (filterIndex >= 0)
				dlg.FilterIndex = filterIndex;
			
			if (initDir != null && initDir != string.Empty)
				dlg.InitialDirectory = initDir;

			if (fileName != null && fileName != string.Empty)
				dlg.FileName = fileName;

			return dlg;
		}
		
		public static SaveFileDialog SaveFileDialog(string keyWord, string title, IFileType[] fileTypes, int filterIndex, string initDir, string fileName)
		{
			FileDialogEx dlgEx = new SaveFileDialogEx(keyWord);
			SaveFileDialog dlg = (SaveFileDialog)dlgEx.FileDialog;

            // apply default folder
			if (CommonDialogs.DefaultFolder != null && CommonDialogs.DefaultFolder != string.Empty)
				dlg.InitialDirectory = CommonDialogs.DefaultFolder;

			if (title != null && title != string.Empty)
				dlg.Title = title;
			else
				dlg.Title = Application.ProductName;
			
			// create file type filters
			string filter = MakeFilter(fileTypes);	
			if (filter != null && filter != string.Empty)
				dlg.Filter = filter;
			else
				dlg.Filter = AllFileFilter;

			if (filterIndex >= 0)
				dlg.FilterIndex = filterIndex;

			if (initDir != null && initDir != string.Empty)
				dlg.InitialDirectory = initDir;

			if (fileName != null && fileName != string.Empty)
				dlg.FileName = fileName;

			return dlg;
		}
		
		

		public static OpenFileDialog OpenImageFileDialog(string title, ImageFileFilter filter, string initDir, string fileName)
		{
			return OpenFileDialog("OpenImageFileDialog", title, MakeFilter(filter), -1, initDir, fileName);
		}

		public static OpenFileDialog OpenImageFileDialog(string title, ImageFileFilter filter, string fileName)
		{
			return OpenImageFileDialog(title, filter, null, fileName);
		}

		public static OpenFileDialog OpenImageFileDialog(string title, ImageFileFilter filter)
		{
			return OpenImageFileDialog(title, filter, null);
		}

		public static OpenFileDialog OpenImageFileDialog(string title)
		{
			return OpenImageFileDialog(title, ImageFileFilter.AllSupportedImageFormat);
		}

		public static OpenImageFile OpenImageCustomizedFileDialog(string title)
		{
			OpenImageFileDialogEx oif = new OpenImageFileDialogEx("OpenImageCustomizedFileDialog");
			OpenFileDialog dlg = oif.CustomizedOpenFileDialog.OpenDialog;
			
            // set dialog title
            dlg.Title = title;

            // set file dialog filter
			dlg.Filter = MakeFilter(ImageFileFilter.AllSupportedImageFormat);
			
            // set default folder if not specified
			if (CommonDialogs.DefaultFolder != null && CommonDialogs.DefaultFolder != string.Empty)
				dlg.InitialDirectory = CommonDialogs.DefaultFolder;

			return oif.CustomizedOpenFileDialog;
		}


		public static SaveFileDialog SaveImageFileDialog(string title, ImageFileFilter filter, string initDir, string fileName)
		{
			return SaveFileDialog("SaveImageFileDialog", title, MakeFilter(filter), -1, initDir, fileName);
		}

		public static SaveFileDialog SaveImageFileDialog(string title, ImageFileFilter filter, string fileName)
		{
			return SaveImageFileDialog(title, filter, null, fileName);
		}

		public static SaveFileDialog SaveImageFileDialog(string title, ImageFileFilter filter)
		{
			return SaveImageFileDialog(title, filter, null);
		}

		public static SaveFileDialog SaveImageFileDialog(string title)
		{
			return SaveImageFileDialog(title, ImageFileFilter.CommonImage);
		}
		
		
		public static OpenFileDialog OpenKLARFFileDialog(string title)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.RestoreDirectory = true;
			dlg.Filter = KlarfFileFilter + "|" + AllFileFilter;
			dlg.Title = title != null && title != string.Empty ? title : Application.ProductName;
			if (_OpenKlarfFileDialog_lastSelectedFilterType >= 0)
				dlg.FilterIndex = _OpenKlarfFileDialog_lastSelectedFilterType;
			else
				_OpenKlarfFileDialog_lastSelectedFilterType = dlg.FilterIndex;		
			dlg.FileOk += new CancelEventHandler(OpenKlarfFileDialog_FileOk);
			return dlg;
		}
		
		private static void OpenKlarfFileDialog_FileOk(object sender, CancelEventArgs e)
		{
			OpenFileDialog dlg = (OpenFileDialog)sender;
			_OpenKlarfFileDialog_lastSelectedFilterType = dlg.FilterIndex;
		}

        
		public static SaveFileDialog SaveKLARFFileDialog(string title)
		{
			SaveFileDialog dlg = new SaveFileDialog();
			dlg.RestoreDirectory = true;
			dlg.Filter = KlarfFileFilter + "|" + AllFileFilter;
			dlg.Title = title != null && title != string.Empty ? title : Application.ProductName;
			if (_SaveKlarfFileDialog_lastSelectedFilterType >= 0)
				dlg.FilterIndex = _SaveKlarfFileDialog_lastSelectedFilterType;
			else
				_SaveKlarfFileDialog_lastSelectedFilterType = dlg.FilterIndex;		
			return dlg;
		}

		public static OpenFileDialog OpenTextFileDialog(string title)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.RestoreDirectory = true;
			dlg.Filter = TextFileFilter + "|" + AllFileFilter;
			dlg.Title = title != null && title != string.Empty ? title : Application.ProductName;
			dlg.DefaultExt = "*.txt";
			return dlg;
		}

		public static SaveFileDialog SaveTextFileDialog(string title)
		{
			SaveFileDialog dlg = new SaveFileDialog();
			dlg.RestoreDirectory = true;
			dlg.Filter = TextFileFilter + "|" + AllFileFilter;
			dlg.Title = title != null && title != string.Empty ? title : Application.ProductName;
			dlg.DefaultExt = "*.txt";
			return dlg;
		}

		
		public static OpenFileDialog OpenPseudoColorFileDialog(string title)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.RestoreDirectory = true;
			dlg.Filter = PseudoColorFileFilter + "|" + AllFileFilter;
			dlg.Title = title != null && title != string.Empty ? title : Application.ProductName;
			dlg.DefaultExt = "*.pc";
			return dlg;
		}

		public static SaveFileDialog SavePseudoColorFileDialog(string title)
		{
			SaveFileDialog dlg = new SaveFileDialog();
			dlg.RestoreDirectory = true;
			dlg.Filter = PseudoColorFileFilter + "|" + AllFileFilter;
			dlg.Title = title != null && title != string.Empty ? title : Application.ProductName;
			dlg.DefaultExt = "*.pc";
			return dlg;
		}

		
		public static OpenFileDialog OpenLayoutFileDialog(string title)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.RestoreDirectory = true;
			dlg.Filter = LayoutFileFilter + "|" + AllFileFilter;
			dlg.Title = title != null && title != string.Empty ? title : Application.ProductName;
			dlg.DefaultExt = "*.lyt";
			return dlg;
		}

		public static SaveFileDialog SaveLayoutFileDialog(string title)
		{
			SaveFileDialog dlg = new SaveFileDialog();
			dlg.RestoreDirectory = true;
			dlg.Filter = LayoutFileFilter + "|" + AllFileFilter;
			dlg.Title = title != null && title != string.Empty ? title : Application.ProductName;
			dlg.DefaultExt = "*.lyt";
			return dlg;
		}


		public static OpenFileDialog OpenMaskFileDialog(string title)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.RestoreDirectory = true;
			dlg.Filter = MaskFileFilter + "|" + AllFileFilter;
			dlg.Title = title != null && title != string.Empty ? title : Application.ProductName;
			dlg.DefaultExt = "*.msk";
			return dlg;
		}


		public static OpenFileDialog OpenCsvFileDialog(string title)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.RestoreDirectory = true;
			dlg.Filter = CsvFileFilter + "|" + AllFileFilter;
			dlg.Title = title != null && title != string.Empty ? title : Application.ProductName;
			dlg.DefaultExt = "*.csv";
			return dlg;
		}
		
		public static SaveFileDialog SaveCsvFileDialog(string title)
		{
			SaveFileDialog dlg = new SaveFileDialog();
			dlg.RestoreDirectory = true;
			dlg.Filter = CsvFileFilter + "|" + AllFileFilter;
			dlg.Title = title != null && title != string.Empty ? title : Application.ProductName;
			dlg.DefaultExt = "*.csv";
			return dlg;
		}


		public static OpenFileDialog OpenXmlFileDialog(string title)
		{
			return OpenFileDialog("OpenXmlFileDialog", title, XmlFileFilter + "|" + AllFileFilter, -1, null, null);
		}

		public static SaveFileDialog SaveXmlFileDialog(string title)
		{
			return SaveFileDialog("SaveXmlFileDialog", title, XmlFileFilter + "|" + AllFileFilter, -1, null, null);
		}


		public static OpenFileDialog OpenScriptFileDialog(string title)
		{
			return OpenFileDialog("OpenScriptFileDialog", title, XmlFileFilter + "|" + AllFileFilter, -1, null, null);
		}

		public static SaveFileDialog SaveScriptFileDialog(string title)
		{
			return SaveFileDialog("SaveScriptFileDialog", title, XmlFileFilter + "|" + AllFileFilter, -1, null, null);
		}

		public static OpenFileDialog OpenArcZoneFileDialog(string title)
		{
			return OpenFileDialog("OpenArcZoneFileDialog", title, ArcZoneFileFilter + "|" + XmlFileFilter + "|" + AllFileFilter, -1, null, null);
		}

		public static SaveFileDialog SaveArcZoneFileDialog(string title)
		{
			return SaveFileDialog("SaveArcZoneFileDialog", title, ArcZoneFileFilter + "|" + XmlFileFilter + "|" + AllFileFilter, -1, null, null);
		}

		public static OpenFileDialog OpenNotchLibraryDialog(string title)
		{
			return OpenFileDialog("OpenNotchLibraryDialog", title, NotchLibFileFileter + "|" + AllFileFilter, -1, null, null);
		}

		public static SaveFileDialog SaveNotchLibraryDialog(string title)
		{
			return SaveFileDialog("SaveNotchLibraryDialog", title, NotchLibFileFileter + "|" + AllFileFilter, -1, null, null);
		}


		public static OpenFileDialog OpenPackageFileDialog(string title)
		{
			return OpenFileDialog("OpenPackageFileDialog", title, PackageFileFilter + "|" + AllFileFilter, -1, null, null);
		}

		public static SaveFileDialog SavePackageFileDialog(string title)
		{
			return SaveFileDialog("SavePackageFileDialog", title, PackageFileFilter + "|" + AllFileFilter, -1, null, null);
		}
		

		public static OpenFileDialog OpenObjectFilterSettingsDialog(string title)
		{
			return OpenFileDialog("OpenObjectFilterSettingsDialog", title, ObjectFilterFilter + "|" + AllFileFilter, -1, null, null);
		}

		public static SaveFileDialog SaveObjectFilterSettingsDialog(string title)
		{
			return SaveFileDialog("SaveObjectFilterSettingsDialog", title, ObjectFilterFilter + "|" + AllFileFilter, -1, null, null);
		}
		

		public static OpenFileDialog OpenProcessStepSettingsFileDialog(string title)
		{
			return OpenFileDialog("OpenProcessStepSettingsFileDialog", title, XmlFileFilter + "|" + AllFileFilter, -1, null, null);
		}

		public static SaveFileDialog SaveProcessStepSettingsFileDialog(string title)
		{
			return SaveFileDialog("SaveProcessStepSettingsFileDialog", title, XmlFileFilter + "|" + AllFileFilter, -1, null, null);
		}


		private static string MakeFilter(IFileType[] fileTypes)
		{
			string filter = "";
            bool bImage = true;
            foreach (IFileType fileType in fileTypes)
            {
                filter += fileType.Filter + "|";
                if (fileType.GetType() == typeof(MaskFileType))
                    bImage = false;
            }
            if (bImage)
            {
                CommonImagesType commonImageFiles = new CommonImagesType();
                filter += commonImageFiles.Filter + "|";
            }
			filter += AllFileFilter;
			return filter;
		}

		private static string MakeFilter(ImageFileFilter filter)
		{
			string strImageFilter = string.Empty;
			
			if ((filter & ImageFileFilter.CommonImage) == ImageFileFilter.CommonImage)
			{
				if (strImageFilter.Length > 0)
					strImageFilter += "|";
				strImageFilter += CommonImageFilter;
			}
			
			//if ((filter & ImageFileFilter.FITImage) == ImageFileFilter.FITImage)
			{
				if (strImageFilter.Length > 0)
					strImageFilter += "|";
				strImageFilter += FitFileFilter;
			}
				
			if ((filter & ImageFileFilter.TextFile) == ImageFileFilter.TextFile)
			{
				if (strImageFilter.Length > 0)
					strImageFilter += "|";
				strImageFilter += TextFileFilter;
			}

			// [Feb 28 2007] Cong commented
//			if ((filter & ImageFileFilter.CommonImage) == ImageFileFilter.CommonImage)
//			{
//				if (strImageFilter.Length > 0)
//					strImageFilter += "|";
//				strImageFilter += CommonImageFilter;
//			}
			
			if (strImageFilter != string.Empty)
				strImageFilter = strImageFilter + "|" + AllFileFilter;
			else
				strImageFilter = AllFileFilter;

			return strImageFilter;
		}

		#endregion

		#region FolderBrowserDialog helper

		public static FolderBrowserDialog SelectFolderDialog(string title)
		{
			return CommonDialogs.SelectFolderDialog(title, null);
		}

		public static FolderBrowserDialog SelectFolderDialog(string title, string description)
		{
			FolderBrowserDialog dlg = new FolderBrowserDialog();
			dlg.ShowNewFolderButton = true;
			dlg.Description = description;            
			dlg.SelectedPath = CommonDialogs.DefaultFolder;

			return dlg;
		}

		#endregion
	}

	
}
