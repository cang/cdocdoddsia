#define SIA_PRODUCT

using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;

using Microsoft.Win32;
using System.Security;

using SiGlaz.Common;

namespace SIA.UI.MaskEditor.DocToolkit
{
    #region Class DocManager

    /// <summary>
    /// Document manager. Makes file-related operations:
    /// open, new, save, updating of the form title, 
    /// registering of file type for Windows Shell.
    /// Built using the article:
    /// Creating Document-Centric Applications in Windows Forms
    /// by Chris Sells
    /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnforms/html/winforms09182003.asp
    /// </summary>
    public class DocManager
    {

        #region Events

        public event SaveEventHandler SaveEvent;
        public event LoadEventHandler LoadEvent;
        public event OpenFileEventHandler OpenEvent;
        public event EventHandler ClearEvent;
        public event EventHandler DocChangedEvent;

        #endregion

        #region Members

        private string fileName = "";		
        private bool dirty = false;
        private bool bStillDirtyAfterSave = false;
        private string sAlternativeSavePrompt = string.Empty;
        private DialogResult bApplyOnClose;
        private bool bReallyClose = true;

        private Control _owner;
        private string newDocName;
        private string _FileDialogFilter;
        private string registryPath;
        private bool updateTitle;

        private const string registryValue = "Path";
        private string _FileDialogInitialDirectory = "";         // file dialog initial directory
        
#if SIA_PRODUCT
        public const string ProductName = "Region Editor";
#else
        public const string ProductName = "Mask Editor";
#endif
        #endregion

        #region Enum

        /// <summary>
        /// Enumeration used for Save function
        /// </summary>
        public enum SaveType 
        {
            Save,
            SaveAs
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="data"></param>
        public DocManager(DocManagerData data)
        {
            _owner = data.Owner;
//			_owner.Closing += new System.ComponentModel.CancelEventHandler(OnClosing);

            updateTitle = data.UpdateTitle;

            newDocName = data.NewDocName;

            _FileDialogFilter = data.FileDialogFilter;

            registryPath = data.RegistryPath;

            if ( ! registryPath.EndsWith("\\") )
                registryPath += "\\";

            registryPath += "FileDir";

            // attempt to read initial directory from registry
            RegistryKey key = Registry.CurrentUser.OpenSubKey(registryPath);

            if ( key != null )
            {
                string s = (string)key.GetValue(registryValue);

                if ( ! Empty(s) )
                    _FileDialogInitialDirectory = s;
            }
        }

        #endregion

        #region Public functions and Properties

        /// <summary>
        /// Dirty property (true when document has unsaved changes).
        /// </summary>
        public bool Dirty
        {
            get
            {
                return dirty;
            }
            set
            {
                dirty = value;
                SetCaption();
            }
        }
        public bool StillDirtyAfterSave
        {
            set
            {
                bStillDirtyAfterSave = value;
            }
        }
        public string AlternativeSavePromp
        {
            set { sAlternativeSavePrompt = value; }
        }
        public DialogResult ApplyOnClose
        {
            get { return bApplyOnClose; }
        }

        /// <summary>
        /// Open new document
        /// </summary>
        /// <returns></returns>
        public bool NewDocument()
        {
            //if( ! CloseDocument() ) 
            //    return false;

            SetFileName("");

            if ( ClearEvent != null )
            {
                // raise event to clear document contents in memory
                // (this class has no idea how to do this)
                ClearEvent(this, new EventArgs());
            }
            
            if (!bStillDirtyAfterSave)
                Dirty = false;

            return true;
        }

        public bool ForceNewDocument()
        {
            SetFileName("");

            if (ClearEvent != null)
            {
                // raise event to clear document contents in memory
                // (this class has no idea how to do this)
                ClearEvent(this, new EventArgs());
            }

            if (!bStillDirtyAfterSave)
                Dirty = false;

            return true;
        }

        /// <summary>
        /// Close document
        /// </summary>
        /// <returns></returns>
        public bool CloseDocument() 
        {
            bApplyOnClose = DialogResult.No; // no need to apply

            if (!this.dirty)
                return true;
            
            if (sAlternativeSavePrompt != string.Empty)
            {
                bApplyOnClose = MessageBox.Show(_owner, sAlternativeSavePrompt, ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                switch (bApplyOnClose)
                {
                    case DialogResult.Yes:
                    case DialogResult.No: return true;
                    case DialogResult.Cancel: return false;
                    default: Debug.Assert(false); return false;
                }
            }
            else
            {
                DialogResult res = MessageBox.Show(_owner,
                    "Do you want to save the changes to " + (Empty(this.fileName) ? newDocName : Path.GetFileName(this.fileName)) + "?",
                    ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                switch (res)
                {
                    case DialogResult.Yes: return SaveDocument(SaveType.Save);
                    case DialogResult.No: return true;
                    case DialogResult.Cancel: return false;
                    default: Debug.Assert(false); return false;
                }
            }
        }

        /// <summary>
        /// Open document
        /// </summary>
        /// <param name="newFileName">
        /// Document file name. Empty - function shows Open File dialog.
        /// </param>
        /// <returns></returns>
        public bool OpenDocument(string newFileName) 
        {
            // Check if we can close current file
            //if( !CloseDocument() ) 
            //    return false;

            // Get the file to open
            if( Empty(newFileName) ) 
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
				
                openFileDialog1.Filter = _FileDialogFilter;
                openFileDialog1.InitialDirectory = _FileDialogInitialDirectory;

                DialogResult res = openFileDialog1.ShowDialog(_owner);

                if( res != DialogResult.OK ) 
                    return false;

                newFileName = openFileDialog1.FileName;
                _FileDialogInitialDirectory = new FileInfo(newFileName).DirectoryName;
//				if((File.GetAttributes(newFileName) & FileAttributes.Normal )!= FileAttributes.Normal)
//				{
//					return false;
//				}
				
            }

            // Read the data
            try 
            {
                using( Stream stream = new FileStream(
                           newFileName, FileMode.Open, FileAccess.Read) ) 
                {
                    // Deserialize object from text format					
                    IFormatter formatter = new BinaryFormatter();//SoapFormatter(); 

                    if ( LoadEvent != null )        // if caller subscribed to this event
                    {
                        SerializationEventArgs args = new SerializationEventArgs(
                            formatter, stream, newFileName);

                        // raise event to load document from file
                        LoadEvent(this, args);

                        if ( args.Error )
                        {
                            // report failure
                            if ( OpenEvent != null )
                            {
                                OpenEvent(this, 
                                    new OpenFileEventArgs(newFileName, false));
                            }

                            return false;
                        }

                        // raise event to show document in the window
                        if ( DocChangedEvent != null )
                        {
                            DocChangedEvent(this, new EventArgs());
                        }
                    }
                }
            }
                // Catch all exceptions which may be raised from this code.
                // Caller is responsible to handle all other exceptions 
                // in the functions invoked by LoadEvent and DocChangedEvent.
            catch( ArgumentNullException ex ) { return HandleOpenException(ex, newFileName); }
            catch( ArgumentOutOfRangeException ex ) { return HandleOpenException(ex, newFileName); }
            catch( ArgumentException ex ) { return HandleOpenException(ex, newFileName); }
            catch( SecurityException ex ) { return HandleOpenException(ex, newFileName); }
            catch( FileNotFoundException ex ) { return HandleOpenException(ex, newFileName); }
            catch( DirectoryNotFoundException ex ) { return HandleOpenException(ex, newFileName); }
            catch( PathTooLongException ex ) { return HandleOpenException(ex, newFileName); }
            catch( IOException ex ) { return HandleOpenException(ex, newFileName); }
			catch( UnauthorizedAccessException ex) {return HandleOpenException(ex, newFileName);}
			catch( Exception ex) { return HandleOpenException(ex, newFileName);}
			
            // Clear dirty bit, cache the file name and set the caption
            if (!bStillDirtyAfterSave)
                Dirty = false;
            SetFileName(newFileName);

            if ( OpenEvent != null )
            {
                // report success
                OpenEvent(this, new OpenFileEventArgs(newFileName, true));
            }

            // Success
            return true;
        }

        /// <summary>
        /// Save file.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool SaveDocument(SaveType type) 
        {
            // Get the file name
            string newFileName = this.fileName;			
			
			using (SaveFileDialog  dlg = new SaveFileDialog())
			{
				dlg.Filter = _FileDialogFilter;	
				if ((type == SaveType.SaveAs) || Empty(newFileName) ) 
				{

					if (!Empty(newFileName)) 
					{
						dlg.InitialDirectory = Path.GetDirectoryName(newFileName);
						dlg.FileName = Path.GetFileName(newFileName);
					}
					else 
					{
						dlg.FileName = newDocName;
					}

					if (DialogResult.OK == dlg.ShowDialog(this._owner))
					{
						newFileName = dlg.FileName;
						_FileDialogInitialDirectory = new FileInfo(newFileName).DirectoryName;
					}
					else
						return false;
				}
			}
			

            // Write the data
            try 
            {
                using( Stream stream = new FileStream(newFileName, FileMode.Create, FileAccess.Write) ) 
                {
                    // Serialize object to text format
                    IFormatter formatter = new BinaryFormatter();//SoapFormatter();

                    if ( SaveEvent != null )        // if caller subscribed to this event
                    {
                        SerializationEventArgs args = new SerializationEventArgs(
                            formatter, stream, newFileName);

                        // raise event
                        SaveEvent(this, args);

                        if ( args.Error )
                            return false;
                    }

                }
            }
            catch( ArgumentNullException ex ) { return HandleSaveException(ex, newFileName); }
            catch( ArgumentOutOfRangeException ex ) { return HandleSaveException(ex, newFileName); }
            catch( ArgumentException ex ) { return HandleSaveException(ex, newFileName); }
            catch( SecurityException ex ) { return HandleSaveException(ex, newFileName); }
            catch( FileNotFoundException ex ) { return HandleSaveException(ex, newFileName); }
            catch( DirectoryNotFoundException ex ) { return HandleSaveException(ex, newFileName); }
            catch( PathTooLongException ex ) { return HandleSaveException(ex, newFileName); }
            catch( IOException ex ) { return HandleSaveException(ex, newFileName); }
			catch( Exception ex) {return HandleSaveException(ex, newFileName);}

            // Clear the dirty bit, cache the new file name
            // and the caption is set automatically
            if (!bStillDirtyAfterSave)
                Dirty = false;
            SetFileName(newFileName);

            // Success
            return true;
        }

        /// <summary>
        /// Assosciate file type with this program in the Registry
        /// </summary>
        /// <param name="data"></param>
        /// <returns>true - OK, false - failed</returns>
        public bool RegisterFileType(
            string fileExtension,
            string progId,
            string typeDisplayName)
        {
            try
            {
                string s = String.Format(CultureInfo.InvariantCulture, ".{0}", fileExtension);

                // Register custom extension with the shell
                using( RegistryKey key = Registry.ClassesRoot.CreateSubKey(s) )
                {
                    // Map custom  extension to a ProgID
                    key.SetValue(null, progId);
                }

                // create ProgID key with display name
                using( RegistryKey key = Registry.ClassesRoot.CreateSubKey(progId) ) 
                {
                    key.SetValue(null, typeDisplayName);
                }

                // register icon
                using( RegistryKey key =
                           Registry.ClassesRoot.CreateSubKey(progId + @"\DefaultIcon") ) 
                {
                    key.SetValue(null, Application.ExecutablePath + ",0");
                }

                // Register open command with the shell
                string cmdkey = progId + @"\shell\open\command";
                using( RegistryKey key =
                           Registry.ClassesRoot.CreateSubKey(cmdkey) ) 
                {
                    // Map ProgID to an Open action for the shell
                    key.SetValue(null, Application.ExecutablePath + " \"%1\"");
                }

                // Register application for "Open With" dialog
                string appkey = "Applications\\" + 
                    new FileInfo(Application.ExecutablePath).Name +
                    "\\shell";
                using( RegistryKey key =
                           Registry.ClassesRoot.CreateSubKey(appkey) ) 
                {
                    key.SetValue("FriendlyCache", ProductName);
                }
            }
            catch (ArgumentNullException ex)
            {
                return HandleRegistryException(ex);
            }
            catch (SecurityException ex)
            {
                return HandleRegistryException(ex);
            }
            catch (ArgumentException ex)
            {
                return HandleRegistryException(ex);
            }
            catch (ObjectDisposedException ex)
            {
                return HandleRegistryException(ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                return HandleRegistryException(ex);
            }

            return true;
        }

        #endregion

        #region Other Functions


        /// <summary>
        /// Hanfle exception from RegisterFileType function
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private bool HandleRegistryException(Exception ex)
        {
            Trace.WriteLine("Registry operation failed: " + ex.Message);
            return false;
        }

        /// <summary>
        /// Save initial directory to the Registry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(registryPath);
            key.SetValue(registryValue, _FileDialogInitialDirectory);
        }


        /// <summary>
        /// Set file name and change owner's caption
        /// </summary>
        /// <param name="fileName"></param>
        private void SetFileName(string fileName) 
        {
            this.fileName = fileName;
            SetCaption();
        }

        /// <summary>
        /// Set owner form caption
        /// </summary>
        private void SetCaption() 
        {
            if ( ! updateTitle )
                return;

			((IMaskEditor)_owner).FileName = this.fileName;
            _owner.Text = string.Format(
                CultureInfo.InvariantCulture,
                "{0} - {1}{2}",
                ProductName,
                Empty(this.fileName) ? newDocName : Path.GetFileName(this.fileName),
                this.dirty ? "*" : "");
        }

        /// <summary>
        /// Handle exception in OpenDocument function
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private bool HandleOpenException(Exception ex, string fileName)
        {
			switch(ex.GetType().FullName) 
			{
				case "System.UnauthorizedAccessException":
					MessageBox.Show(_owner, 
						"Cannot load the file because "+fileName+" is read-only or access is denied.  \r\n" ,
						ProductName, 
						MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;
				default:
					MessageBox.Show(_owner, 
						"Cannot load the file because either it is not in a supported file type or it has been corrupted.  \r\n" ,
						ProductName, 
						MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;
			}

            

            if ( OpenEvent != null )
            {
                // report failure
                OpenEvent(this, new OpenFileEventArgs(fileName, false));
            }

            return false;
        }

        /// <summary>
        /// Handle exception in SaveDocument function
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private bool HandleSaveException(Exception ex, string fileName)
        {
			switch(ex.GetType().FullName) 
			{
				case "System.UnauthorizedAccessException":
					
						MessageBox.Show(_owner, 
							"Cannot save the file because "+ fileName +" is read-only or access is denied.  \r\n" ,	ProductName, 
							MessageBoxButtons.OK, MessageBoxIcon.Error);						
					break;					
				default:
					MessageBox.Show(_owner, 
						//"Cannot open the file. Please check the file format or the access permission,... \"" + fileName + "\n",
						"Cannot save the file because either it is not in a supported file type or it has been corrupted.  \r\n" ,
						ProductName, 
						MessageBoxButtons.OK, MessageBoxIcon.Error);

					break;
			}

            return false;
        }


        /// <summary>
        /// Helper function - test if string is empty
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        static bool Empty(string s) 
        { 
            return s == null || s.Length == 0; 
        }

        #endregion

    }

    #endregion

    #region Delegates

    public delegate void SaveEventHandler(object sender, SerializationEventArgs e);
    public delegate void LoadEventHandler(object sender, SerializationEventArgs e);
    public delegate void OpenFileEventHandler(object sender, OpenFileEventArgs e);

    #endregion

    #region Class SerializationEventArgs

    /// <summary>
    /// Serialization event arguments.
    /// Used in events raised from DocManager class.
    /// Class contains information required to load/save file.
    /// </summary>
    public class SerializationEventArgs : System.EventArgs
    {
        private IFormatter formatter;
        private Stream stream;
        private string fileName;
        private bool errorFlag;

        public SerializationEventArgs(IFormatter formatter, Stream stream, 
            string fileName)
        {
            this.formatter = formatter;
            this.stream = stream;
            this.fileName = fileName;
            errorFlag = false;
        }

        public bool Error
        {
            get
            {
                return errorFlag;
            }
            set
            {
                errorFlag = value;
            }
        }

        public IFormatter Formatter
        {
            get
            {
                return formatter;
            }
        }

        public Stream SerializationStream
        {
            get
            {
                return stream;
            }
        }
        public string FileName
        {
            get
            {
                return fileName;
            }
        }
    }

    #endregion

    #region Class OpenFileEventArgs

    /// <summary>
    /// Open file event arguments.
    /// Used in events raised from DocManager class.
    /// Class contains name of file and result of Open operation.
    /// </summary>
    public class OpenFileEventArgs : System.EventArgs
    {
        private string fileName;
        private bool success;

        public OpenFileEventArgs(string fileName, bool success)
        {
            this.fileName = fileName;
            this.success = success;
        }

        public string FileName
        {
            get
            {
                return fileName;
            }
        }

        public bool Succeeded
        {
            get
            {
                return success;
            }
        }
    }

    #endregion

    #region class DocManagerData

    /// <summary>
    /// Class used for DocManager class initialization
    /// </summary>
    public class DocManagerData
    {
        public DocManagerData()
        {
            _owner = null;
            updateTitle = true;
            newDocName = "Untitled";
            _FileDialogFilter = "All Files (*.*)|*.*";
            registryPath = "Software\\Unknown";
        }

        private Control _owner;
        private bool updateTitle;
        private string newDocName;
        private string _FileDialogFilter;
        private string registryPath;

        public Control Owner
        {
            get
            {
                return _owner;
            }
            set
            {
                _owner = value;
            }
        }

        public bool UpdateTitle
        {
            get
            {
                return updateTitle;
            }
            set
            {
                updateTitle = value;
            }
        }

        public string NewDocName
        {
            get
            {
                return newDocName;
            }
            set
            {
                newDocName = value;
            }
        }

        public string FileDialogFilter
        {
            get
            {
                return _FileDialogFilter;
            }
            set
            {
                _FileDialogFilter = value;
            }
        }

        public string RegistryPath
        {
            get
            {
                return registryPath;
            }
            set
            {
                registryPath = value;
            }
        }
    };

    #endregion
}

#region Using

/*
Using:

1. Write class which implements program-specific tasks. This class keeps some data,
   knows to draw itself in the form window, and implements ISerializable interface.
   Example:
   
    [Serializable]
    public class MyTask : ISerializable
    {
        // class members
        private int myData;
        // ...
   
        public MyTask()
        {
           // ...
        }
       
        public void Draw(Graphics g, Rectangle r)
        {
            // ...
        }
       
        // other functions
        // ...
       
        // Serialization
        // This function is called when file is loaded
        protected GraphicsList(SerializationInfo info, StreamingContext context)
        {
            myData = info.GetInt32("myData");
            // ...
        }

        // Serialization
        // This function is called when file is saved
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("myData", myData);
            // ...
        }
    }
       
    Add member of this class to the form:
    
    private MyClass myClass;


2. Add the DocManager member to the owner form:

private DocManager docManager;

3. Add DocManager message handlers to the owner form:

        private void docManager_ClearEvent(object sender, EventArgs e)
        {
            // DocManager executed New command
            // Clear here myClass or create new empty instance:
            myClass = new MyClass();
            Refresh();
        }
        
        private void docManager_DocChangedEvent(object sender, EventArgs e)
        {
            // DocManager reports that document was changed (loaded from file)
            Refresh();
        }

        private void docManager_OpenEvent(object sender, OpenFileEventArgs e)
        {
            // DocManager reports about successful/unsuccessful Open File operation
            // For example:
            
            if ( e.Succeeded )
                // add e.FileName to MRU list
            else
                // remove e.FileName from MRU list
        }
        
        private void docManager_LoadEvent(object sender, SerializationEventArgs e)
        {
            // DocManager asks to load document from supplied stream
            try
            {
                myClass  = (MyClass)e.Formatter.Deserialize(e.SerializationStream);
            }
            catch ( catch possible exceptions here )
            { 
                  // report error
                  
                  e.Error = true;
            }
        }
        
        private void docManager_SaveEvent(object sender, SerializationEventArgs e)
        {
            // DocManager asks to save document to supplied stream
            try
            {
                e.Formatter.Serialize(e.SerializationStream, myClass);
            }
            catch ( catch possible exceptions here )
            { 
                  // report error
                  
                  e.Error = true;
            }
        }
        
4. Initialize docManager member in the form initialization code:

            DocManagerData data = new DocManagerData();
            data.FormOwner = this;
            data.UpdateTitle = true;
            data.FileDialogFilter = "MyProgram files (*.mpf)|*.mpf|All Files (*.*)|*.*";
            data.NewDocName = "Untitled.mpf";
            data.RegistryPath = "Software\\MyCompany\\MyProgram";

            docManager = new DocManager(data);

            docManager.SaveEvent +=new SaveEventHandler(docManager_SaveEvent);
            docManager.LoadEvent +=new LoadEventHandler(docManager_LoadEvent);
            docManager.OpenEvent += new OpenFileEventHandler(docManager_OpenEvent);
            docManager.DocChangedEvent += new EventHandler(docManager_DocChangedEvent);
            docManager.ClearEvent += new EventHandler(docManager_ClearEvent);

            docManager.NewDocument();

            // Optionally - register file type for Windows Shell
            bool result = docManager.RegisterFileType("mpf", "mpffile", "MyProgram File");            

5. Call docManager functions when necessary. For example:

        // File is dropped into the window;
        // Command line parameter is handled.
        public void OpenDocument(string file)
        {
            docManager.OpenDocument(file);
        }

        // User Selected File - Open command.
        private void CommandOpen()
        {
            docManager.OpenDocument("");
        }

        // User selected File - Save command
        private void CommandSave()
        {
            docManager.SaveDocument(DocManager.SaveType.Save);
        }

        // User selected File - Save As command
        private void CommandSaveAs()
        {
            docManager.SaveDocument(DocManager.SaveType.SaveAs);
        }

        // User selected File - New command
        private void CommandNew()
        {
            docManager.NewDocument();
        }


6. Optionally: test for unsaved data in the form Closing event:

        private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if ( ! docManager.CloseDocument() )
                e.Cancel = true;
        }

7. Optionally: handle command-line parameters in the main function:

        [STAThread]
        static void Main(string[] args) 
        {
            // Check command line
            if( args.Length > 1 ) 
            {
                MessageBox.Show("Incorrect number of arguments. Usage: MyProgram.exe [file]", "MyProgram");
                return;
            }

            // Load main form, taking command line into account
            MainForm form = new MainForm();

            if ( args.Length == 1 ) 
                form.OpenDocument(args[0]);     // OpenDocument calls docManager.OpenDocument

            Application.Run(form);
        }
*/

#endregion