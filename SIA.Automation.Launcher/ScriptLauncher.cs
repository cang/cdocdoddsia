#define DEBUG_METETIME_

using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Configuration;

using SIA.Common;

using SIA.SystemFrameworks.UI;

using SIA.SystemLayer;

using SIA.Automation.Launcher.Configuration;
using SIA.Automation.Launcher.Searching;
using SIA.Automation.Launcher.Serialization;

using SIA.UI.Controls.Automation;
using SIA.Workbench.Common;
using SIA.UI.Controls;
using System.Collections;
using System.Text.RegularExpressions;

namespace SIA.Automation.Launcher
{
	/// <summary>
	/// The <b>ScriptLauncher</b> class manage the standalone execution of the script,
	/// built by the Script builder of SiGlaz Image Analyzer
	/// </summary>
	public class ScriptLauncher
	{
		#region Members

		//private string _startUpPath;
		//private string _logsDataPath;
		//private string _logFilename;		

		private string _hostProcessID = "";
		
		private bool _noCopyright = false;
		private bool _noHelpText = false;
		private bool _quiet = false;
		private bool _noLog = false;	

		private bool _isCancelled = false;
		private bool _isProcessing = false;
		private int _processID = -1;
		private ManualResetEvent _waitForExit = new ManualResetEvent(false);
		
		private bool _hasScriptFile = false;
		private string _scriptFile = "";

		private bool _hasImageFile = false;
		private string _imageFile = "";

        private bool _hasImageFolder = false;
        private string _imageFolder = "";

        private bool _hasImageFilter = false;
        private string _imageFilter = "";

		private bool _displayHelp = false;
		private int _startCounter = 0;

        private bool _hasErrorFolder = false;
        private string _errorFolder = "";

        private bool _hasBkFolder = false;
        private string _bkFolder = "";
		
		//private FileStream _logFileStream;
		//private FormattedTextWriterTraceListener _logFileListener;
		private CommandLineParsingEngine _commandLineParsingEngine = null;
		private UpdateStatusCallback _updater =  null;

		private ErrorCodes _errorCode = ErrorCodes.OK;
		#endregion

		#region Properties

		public bool IsProcessing
		{
			get {return _isProcessing;}
		}


		public bool IsCancelled
		{
			get {return _isCancelled;}
			set {_isCancelled = value;}
		}


		#endregion

		#region Constructors and Destructors
		
		public ScriptLauncher()
		{
			this.InitClass();
		}

		~ScriptLauncher()
		{
			this.UninitClass();
		}


		#endregion

		#region Internal Helpers

		/// <summary>
		/// The method <b>InitClass</b> initializes the member fields of the <b>ScriptLauncher</b> class.
		/// </summary>
		private void InitClass()
		{
			// retrieve the startup path
			//Assembly execAssembly = Assembly.GetExecutingAssembly();
			//_startUpPath = Path.GetDirectoryName(execAssembly.Location);

			// combine the local user path with logs to form the logs path
			//_logsDataPath = Path.Combine(_startUpPath, "AutomationLogs");
			
			// combine the log path with the file name to form the full log path						
			//DateTime now = DateTime.Now;
			//string fileName = String.Format("Logs_{0}.txt", now.ToString("yyyy-MM-dd-HH-mm-ss-ffff"));
			//_logFilename = Path.Combine(_logsDataPath, fileName);

		}

		private void UninitClass()
		{
			//UninstallTraceListener();
		}

		/// <summary>
		/// Parses the command line for supported debugging flags and sets their values if found
		/// </summary>
		private void ParseCommandLineForArguments()
		{
			try
			{
				// flags for reading from the command line and the app config file
				bool cmdLineQuiet = false;
				bool cmdLineNoLog = false;
				bool cmdLineNoCopyright = false;
				bool cmdLineNoHelpText = false;
				string cmdLineScriptFile = "";
				string cmdLineImageFile = "";
				string cmdLineParentPID = string.Empty;
				int cmdLineStartCounter = 0;
                string cmdLineImageFolder = "";
                string cmdLineImageFilter = "";
                int cmdLineMonitorFrequency = 300;

                string cmdLineErrorFolder = "";
                string cmdLineBkFolder = "";
				
				// read options from the command line (NOTE: Command Line is case sensitive so these flags must be presented in LOWER case to function)
				cmdLineScriptFile = _commandLineParsingEngine.ToString("s");
				cmdLineImageFile = _commandLineParsingEngine.ToString("i");
				cmdLineStartCounter = _commandLineParsingEngine.ToInt32("c");
				cmdLineNoCopyright = _commandLineParsingEngine.ToBoolean("nocopyright");
				cmdLineNoHelpText = _commandLineParsingEngine.ToBoolean("nohelptext");

                cmdLineImageFolder = _commandLineParsingEngine.ToString("in");
                cmdLineImageFilter = _commandLineParsingEngine.ToString("filter");

                cmdLineMonitorFrequency = _commandLineParsingEngine.ToInt32("mf");

                //cmdLineErrorFolder = _commandLineParsingEngine.ToString("e");
                //cmdLineBkFolder = _commandLineParsingEngine.ToString("bk");

				// retrieve host process ID
				cmdLineParentPID = _commandLineParsingEngine.ToString("p");
				if (cmdLineParentPID != string.Empty)
				{
					//_statusSharedMemoryName = SIA.Workbench.Common.UpdateStatusCommand.GetSharedStatusMappingFileName(cmdLineParentPID);
					this._hostProcessID = cmdLineParentPID;
				}

				cmdLineQuiet = _commandLineParsingEngine.ToBoolean("q");				
				cmdLineNoLog = _commandLineParsingEngine.ToBoolean("nl");
				
				// set flags from either command line flags or app config file
				_quiet = cmdLineQuiet;
				_noLog = cmdLineNoLog;
				_noCopyright = cmdLineNoCopyright;
				_noHelpText = cmdLineNoHelpText;

				_hasScriptFile = cmdLineScriptFile != null && cmdLineScriptFile != string.Empty;
				_scriptFile = cmdLineScriptFile;

				_hasImageFile = cmdLineImageFile != null && cmdLineImageFile != string.Empty;
				_imageFile = cmdLineImageFile;

                _hasImageFolder = cmdLineImageFolder != null && cmdLineImageFolder != string.Empty;
                _imageFolder = cmdLineImageFolder;

                _hasImageFilter = cmdLineImageFilter != null && cmdLineImageFilter != string.Empty;
                _imageFilter = cmdLineImageFilter;

				_startCounter = cmdLineStartCounter;

                _monitorFrequency = (cmdLineMonitorFrequency >= 1 ? cmdLineMonitorFrequency : 1);

                //_hasErrorFolder = cmdLineErrorFolder != null && cmdLineErrorFolder != string.Empty;
                //_errorFolder = cmdLineErrorFolder;

                //_hasBkFolder = cmdLineBkFolder != null && cmdLineBkFolder != string.Empty;
                //_bkFolder = cmdLineBkFolder;

#if DEBUG
                Console.WriteLine("Monitor frequency: {0}", _monitorFrequency);
#endif
			}
			catch 
			{
				throw;
			}
		}

		/// <summary>
		/// Installs our logging trace listener if the NoLogging flag is not set
		/// </summary>
		private void InstallTraceListener()
		{
			TraceLog.Initialize(AppSettings.TraceLogFolder);
			TraceLog.Enabled = _noLog;
		}

		/// <summary>
		/// Uninstalls our logging trace listener if it was installed
		/// </summary>
		private void UninstallTraceListener()
		{
			TraceLog.Uninitialize();
		}

		#endregion

		#region Methods		

		/// <summary>
		/// The <b>IsImageFile</b> checks whether a specified fileName is a valid image file format.
		/// </summary>
		/// <param name="fileName">the name of the file need to be checked</param>
		/// <returns>True if the file is valid, otherwise false</returns>
		private bool IsImageFile(string fileName)
		{
			try
			{
				using (CommonImage image = CommonImage.FromFile(fileName))
					return true;
			}
			catch 
			{                
			}

			return false;
		}

		/// <summary>
		/// The <b>IsImage</b> checks whether a specified fileName is a valid image file format.
		/// </summary>
		/// <param name="fileName">the name of the file need to be checked</param>
		/// <returns>True if the file is valid, otherwise false</returns>
		public static bool IsImage(string fileName)
		{
			try
			{
				using (SIA.SystemLayer.CommonImage image = SIA.SystemLayer.CommonImage.FromFile(fileName))
					return true;
			}
			catch 
			{
			}

			return false;
		}

		/// <summary>
		/// The method <b>PrintCopyright</b> writes to console copyright information.
		/// </summary>
		private void PrintCopyright()
		{
			if (_noCopyright)
				return;

			Assembly assembly = Assembly.GetExecutingAssembly();
			FileVersionInfo version = FileVersionInfo.GetVersionInfo(assembly.Location);
			
			Console.WriteLine("SiGlaz Image Analyzer Script Launcher");
			//Console.WriteLine("Version " + version.ProductVersion);
            Console.WriteLine(string.Format("Version {0}", version.FileVersion.ToString()));
            DateTime releaseDate = GetReleaseDate(assembly);
            if (releaseDate != DateTime.MinValue)
            {
                Console.WriteLine(string.Format("ReleaseDate {0}", releaseDate.ToString("yyyy/MM/dd")));
            }
			Console.WriteLine("");
		}

        private DateTime GetReleaseDate(Assembly assembly)
        {
            object[] attribs = assembly.GetCustomAttributes(typeof(AssemblyReleaseDateAttribute), false);
            foreach (Attribute attrib in attribs)
            {
                AssemblyReleaseDateAttribute attReleaseDate = attrib as AssemblyReleaseDateAttribute;
                if (attReleaseDate == null)
                    continue;
                return attReleaseDate.ReleaseDate;
            }

            return DateTime.MinValue;
        }

		/// <summary> 
		/// The method <b>PrintHelpText</b> writes to console help information
		/// </summary>
		private void PrintHelpText()
		{
			if (_noHelpText)
				return;

			Console.WriteLine("SIA.Automation.Launcher.exe /S:script [/I:imagefile] [/IN:imagefolder] [/FILTER:imagefilter] [/C:startCounter] [/MF:monitorFrequency] [/Q]");
			Console.WriteLine("");
			Console.WriteLine(" /S:script ");
			Console.WriteLine("       Specifies the location of the script need to execute.");
			Console.WriteLine(" /I:imagefile");
			Console.WriteLine("       Specifies the image file for processing.");
            Console.WriteLine(" /IN:imagefolder");
            Console.WriteLine("       Specifies the image folder for processing.");
            //Console.WriteLine(" /BK:backupfolder");
            //Console.WriteLine("       Specifies the backup folder in which processing files will be copied to.");
            //Console.WriteLine(" /E:errorfolder");
            //Console.WriteLine("       Specifies the error folder in which error files will be copied to.");
            Console.WriteLine(" /FILTER:imagefilter");
            Console.WriteLine("       Specifies the image type for processing (i.e /FILTER:\"*.bmp;*.jpg\").");
			Console.WriteLine(" /C:startCounter");
			Console.WriteLine("       Specifies the start counter for the script file generator.");
            Console.WriteLine(" /MF:monitorFrequency (default is 300 milliseconds)");
            Console.WriteLine("       Specifies the frequence for monitoring the specified image folder.");            
			Console.WriteLine(" /Q	  Quiet mode, do not output to console.");			
			Console.WriteLine(" /nL	  No log file, do not trace to log file.");
			Console.WriteLine(" /?	  Display this help messages.");
			Console.WriteLine("");
		}		

		/// <summary>
		/// The <b>LaunchScript</b> method executes script with the specified script file and image file
		/// </summary>
		/// <remarks>
		/// <p>This function must return an error code instead of throwing any exception</p>
		/// </remarks>
		/// <param name="scriptFilename">The path of the script file.</param>
		/// <param name="imageFile">The path of the input image.</param>
		/// <param name="counter">Counter</param>
		public ErrorCodes LaunchScript(string scriptFilename, string imageFile, int counter)
		{
			_errorCode = ErrorCodes.OK;
			Script script = null;

            try
            {
                // signal processing flags
                this._isProcessing = true;

                #region deserialize script
                try
                {
                    script = Script.Deserialize(scriptFilename);
                    script.Name = Path.GetFileNameWithoutExtension(scriptFilename);
                    script.Counter = counter;

                    // don't know why set status data here
                    //_updater.SetStatusData(script.StatusData);
                }
                catch (System.Exception exp)
                {
                    string message = "Failed to read script \"" + scriptFilename + "\". Reason: " + exp.ToString();
                    Trace.WriteLine(message);

                    _errorCode = ErrorCodes.FailedToDeserializeScript;
                }
                #endregion

                #region execute script

                try
                {
                    // output start processing signal to console
                    Console.WriteLine("Start Processing {0} ... ", imageFile);

                    // regists for callback event
                    script.ExecuteCallback += new ExecuteCallback(Script_ExecuteCallback);
                    script.ExceptionCallback += new ExceptionCallback(Script_ExceptionCallback);

                    // initialize script storage
                    if (this._hostProcessID != null && this._hostProcessID != string.Empty)
                        script.WorkingSpace.LoadStorage(this._hostProcessID);

#if DEBUG_METETIME
                    Stopwatch sw = new Stopwatch(); sw.Start();
#endif

                    // execute script with the specified image file
                    script.Execute(imageFile);


#if DEBUG_METETIME
                    sw.Stop(); Trace.WriteLine(sw.Elapsed);
                    Trace.WriteLine("Init CL Memory " + SiGlaz.Cloo.DeviceProgram.t);
#endif

                    // uninitialize script storage
                    if (this._hostProcessID != null && this._hostProcessID != string.Empty)
                        script.WorkingSpace.SaveStorage(this._hostProcessID);

                    // output succeeded signal to console
                    Console.WriteLine("File \"" + imageFile + "\" was processed successfully.");
                }
                catch (System.Exception exp)
                {
                    string message = "Failed to execute script \"" + scriptFilename + "\". Reason: " + exp.ToString();

                    // output to console for notification user
                    Console.WriteLine(message);

                    // output to trace log for troubleshooting
                    Trace.WriteLine(message);
                    Trace.WriteLine(exp);
                }
                finally
                {
                    if (script != null)
                    {
                        // unregist for execution callback handler
                        script.ExecuteCallback -= new ExecuteCallback(Script_ExecuteCallback);
                        script.Dispose();
                    }
                    script = null;

                    Console.WriteLine("Finish Processing.");
                }

                //if (!_updater.IsSucceeded)
                if (_updater!=null && !_updater.IsSucceeded)
                {
                    _errorCode = ErrorCodes.StatusQueueIsNotEmpty;
                }

                #endregion
            }
            catch
            {
                throw;
            }
			finally
			{
				// clean up script
				if (script != null)
					script.Dispose();
				script = null;

				// signal processing flags
				this._isProcessing = false;
			}

			return _errorCode;
		}

        private System.Collections.Generic.List<string> _newIncomingFiles = new System.Collections.Generic.List<string>();
        private int _maxQueuItems = 100;
        private Hashtable _extLookup = new Hashtable();
        private void CreateExtLookup()
        {
            if (!_hasImageFilter)
                return;

            try
            {
                string imageFilter = _imageFilter.Trim();
                // i.e *.bmp;*.jpg;*.png
                string pattern = "(?<grp>(?<ext>[^;]*);?)*";
                Regex regExt = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match match = regExt.Match(imageFilter);
                if (match.Success)
                {
                    CaptureCollection exts = match.Groups["ext"].Captures;
                    if (exts != null && exts.Count > 0)
                    {
                        foreach (Capture ext_capture in exts)
                        {
                            if (ext_capture == null) continue;

                            string ext = ext_capture.Value;

                            if (ext == null || ext.Trim() == string.Empty)
                                continue;
                            ext = ext.Trim().ToLower();
                            ext = ext.Replace("*", "");
                            if (!_extLookup.ContainsKey(ext))
                                _extLookup.Add(ext, ext);
                        }
                    }
                }
            }
            catch
            {
                _hasImageFilter = false;
                _extLookup = new Hashtable();
            }
        }

        private int _monitorFrequency = 300;

        private const int retryIterators = 200;
        private const int timeSleepEachRetryIterators = 50; // milliseconds

        /// <summary>
		/// The <b>LaunchScript</b> method executes script with the specified script file, image folder, and image filter
		/// </summary>
		/// <remarks>
		/// <p>This function must return an error code instead of throwing any exception</p>
		/// </remarks>
		/// <param name="scriptFilename">The path of the script file.</param>
		/// <param name="imageFolder">The path of the input image folder.</param>
        /// <param name="imageFilter">The mask of the input image filter.</param>
        public ErrorCodes LaunchScript(string scriptFilename, string imageFolder, string imageFilter)
        {
            _errorCode = ErrorCodes.OK;

            Script script = null;
            try
            {
                // signal processing flags
                this._isProcessing = true;

                #region deserialize script
                try
                {
                    script = Script.Deserialize(scriptFilename);
                    script.Name = Path.GetFileNameWithoutExtension(scriptFilename);
                    script.Counter = 0;

                    // don't know why set status data here
                    //_updater.SetStatusData(script.StatusData);
                }
                catch (System.Exception exp)
                {
                    string message = "Failed to read script \"" + scriptFilename + "\". Reason: " + exp.ToString();
                    Trace.WriteLine(message);

                    _errorCode = ErrorCodes.FailedToDeserializeScript;

                    throw exp;
                }
                #endregion

                #region execute script

                // regists for callback event
                script.ExecuteCallback += new ExecuteCallback(Script_ExecuteCallback);
                script.ExceptionCallback += new ExceptionCallback(Script_ExceptionCallback);

                // initialize script storage
                if (this._hostProcessID != null && this._hostProcessID != string.Empty)
                    script.WorkingSpace.LoadStorage(this._hostProcessID);
                
                string imageFile = string.Empty;                                
                if (_hasImageFilter)
                {
                    CreateExtLookup();
                }
                this.GetFiles(imageFolder);

                while (true)
                {
                    try
                    {
                        if (_cancelKeyPress)
                            break;

                        // process for new incoming files
                        while (_newIncomingFiles != null && _newIncomingFiles.Count > 0)
                        {
                            if (_cancelKeyPress)
                                break;

                            // get first file
                            imageFile = _newIncomingFiles[0];
                            _newIncomingFiles.RemoveAt(0);

                            if (!File.Exists(imageFile))
                                continue;

                            //if (_hasBkFolder)
                            //{
                            //    CopyFile(imageFile, _bkFolder);
                            //}


                            // output start processing signal to console
                            Console.WriteLine("Start Processing {0} ... ", imageFile);

                            bool succeed = false;
                            int counter = 0;
                            while (counter < retryIterators) // try to process about 10 times
                            {
                                try
                                {
                                    #region Execute an image file
                                    // execute script with the specified image file
                                    script.Execute(imageFile);
                                    
                                    #endregion Execute an image file

                                    if (script.IsProcessingSuccessful)
                                    {
                                        script.Counter = script.Counter + 1;

                                        succeed = true;

                                        break;
                                    }
                                    else
                                    {
                                        counter++;
                                        succeed = false;
                                        Thread.Sleep(timeSleepEachRetryIterators);
                                    }
                                }
                                catch (System.Exception exp)
                                {
                                    counter++;

                                    string message = "Failed to process file \"" + imageFile + "\". Reason: " + exp.ToString();

                                    // output to console for notification user
                                    Console.WriteLine(message);

                                    // output to trace log for troubleshooting
                                    Trace.WriteLine(message);
                                    Trace.WriteLine(exp);

                                    //if (_hasErrorFolder)
                                    //{
                                    //    CopyFile(imageFile, _errorFolder);
                                    //}

                                    succeed = false;

                                    Thread.Sleep(timeSleepEachRetryIterators);
                                }
                                finally
                                {
                                    //DeleteFile(imageFile);
                                }
                            }

                            if (succeed)
                            {
                                // output succeeded signal to console
                                Console.WriteLine("File \"" + imageFile + "\" was processed successfully.");

                                DeleteFile(imageFile);
                            }
                        }

                        if (_cancelKeyPress)
                            break;

                        // try to get new incomming files
                        this.GetFiles(imageFolder);

                        if (_newIncomingFiles != null && _newIncomingFiles.Count > 0)
                            continue;

                        // print scanning status
                        Console.Write("Scanning new incoming file(s).................");

                        if (_cancelKeyPress)
                            break;

                        Thread.Sleep(_monitorFrequency);

                        if (_cancelKeyPress)
                            break;

                        this.GetFiles(imageFolder);

                        if (_newIncomingFiles == null || _newIncomingFiles.Count == 0)
                        {
                            Console.WriteLine("(0 file)");

                            if (_cancelKeyPress)
                                break;

                            Thread.Sleep(_monitorFrequency);
                        }                      
                        else
                            Console.WriteLine();
                    }
                    catch (System.Exception exp)
                    {
                        string msg = string.Format("Failed to process. Reason: {0}", exp.Message);
                        // output to console for notification user
                        Console.WriteLine(msg);

                        // output to trace log for troubleshooting
                        Trace.WriteLine(msg);
                        Trace.WriteLine(exp);
                  
                        break;
                    }
                    finally
                    {                        
                    }
                }

                // uninitialize script storage
                if (this._hostProcessID != null && this._hostProcessID != string.Empty)
                    script.WorkingSpace.SaveStorage(this._hostProcessID);

                if (script != null)
                {
                    // unregist for execution callback handler
                    script.ExecuteCallback -= new ExecuteCallback(Script_ExecuteCallback);
                    script.Dispose();
                }
                script = null;

                Console.WriteLine("Finish Processing.");

                if (_updater != null && !_updater.IsSucceeded)
                {
                    _errorCode = ErrorCodes.StatusQueueIsNotEmpty;
                }

                #endregion
            }
            catch (System.Exception exp)
            {
                string message = "Failed to execute script \"" + scriptFilename + "\". Reason: " + exp.ToString();

                // output to console for notification user
                Console.WriteLine(message);

                // output to trace log for troubleshooting
                Trace.WriteLine(message);
                Trace.WriteLine(exp);
            }
            finally
            {
                // clean up script
                if (script != null)
                    script.Dispose();
                script = null;

                // signal processing flags
                this._isProcessing = false;
            }

            return _errorCode;
        }

        /// <summary>
        /// Delete the specified file
        /// </summary>
        /// <param name="file"> The deleted file path. </param>
        private void DeleteFile(string file)
        {
            if (file == null || file == string.Empty)
                return;
            if (!File.Exists(file))
                return;
            try
            {
                File.Delete(file);
            }
            catch (System.Exception exp)
            {
                string msg = string.Format("Failed to delete file: {0} due to: {1}", file, exp.Message);
                Console.WriteLine(msg);
                Trace.WriteLine(msg);
                Trace.WriteLine(exp);
            }
        }

        private void CopyFile(string sourcefilePath, string dstFolder)
        {
            if (sourcefilePath == null || sourcefilePath == string.Empty)
                return;
            if (dstFolder == null || dstFolder == string.Empty)
                return;

            if (!File.Exists(sourcefilePath))
                return;

            if (!Directory.Exists(dstFolder))
                return;

            try
            {
                string dst_file = Path.Combine(dstFolder, Path.GetFileName(sourcefilePath));
                File.Copy(sourcefilePath, dst_file, true);
            }
            catch (System.Exception exp)
            {
                string msg = string.Format("Failed to copy file: {0} to folder: {1}, due to: {2}", sourcefilePath, dstFolder, exp.Message);
                Console.WriteLine(msg);
                Trace.WriteLine(msg);
                Trace.WriteLine(exp);
            }
        }
        
        /// <summary>
        /// Get new incoming file in the specified folder
        /// </summary>
        /// <param name="folder"> Represents the location of new incoming file if has. </param>
        private void GetFiles(string folder)
        {
            try
            {
                if (_newIncomingFiles.Count >= _maxQueuItems)
                    return;

                string[] files = Directory.GetFiles(folder);
                if (files != null && files.Length > 0)
                {
                    if (_hasImageFilter)
                    {
                        int n = files.Length;
                        for (int i = 0; i<n; i++)
                        {
                            string ext = Path.GetExtension(files[i]).ToLower();
                            if (!_extLookup.ContainsKey(ext))
                                continue;
                            _newIncomingFiles.Add(files[i]);
                        }
                    }
                    else
                    {
                        _newIncomingFiles.AddRange(files);
                    }
                }
                // help to garbage collector works well in case recursive
                files = null;

                string[] sub_folders = Directory.GetDirectories(folder);
                if (sub_folders != null && sub_folders.Length > 0)
                {
                    for (int i = 0; i < sub_folders.Length; i++)
                        GetFiles(sub_folders[i]);
                }
            }
            catch
            {
                // nothing
            }
        }

		/// <summary>
		/// The <b>LaunchScript</b> method executes script with the specified script file and image file
		/// </summary>
		/// <remarks>
		/// <p>This function must return an error code instead of throwing any exception</p>
		/// </remarks>
		/// <param name="scriptFilename">The path of the script file.</param>
		/// <param name="imageFile">The path of the input image.</param>
		/// <param name="counter">Counter</param>
		public ErrorCodes LaunchScript(string scriptFilename)
		{
			_errorCode = ErrorCodes.OK;
			Script script = null;

			try
			{
				// signal processing flags
				this._isProcessing = true;

				#region deserialize script

				try
				{
					script = Script.Deserialize(scriptFilename);
					script.Name = Path.GetFileNameWithoutExtension(scriptFilename);
					script.Counter = 0;
				}
				catch (System.Exception exp)
				{
					string message = "Failed to read script \"" + scriptFilename + "\". Reason: " + exp.ToString();
					Trace.WriteLine(message);

					_errorCode = ErrorCodes.FailedToDeserializeScript;
				}

				#endregion
			
				#region execute script

				try
				{
					// output start processing signal to console
					Console.WriteLine("Start Processing ... ");

					// regists for callback event
					script.ExecuteCallback += new ExecuteCallback(Script_ExecuteCallback);
					script.ExceptionCallback += new ExceptionCallback(Script_ExceptionCallback);

					// initialize script storage
					if (this._hostProcessID != null && this._hostProcessID != string.Empty)
						script.WorkingSpace.LoadStorage(this._hostProcessID);

					// execute script with the specified image file
					script.Execute(string.Empty);

					// uninitialize script storage
					if (this._hostProcessID != null && this._hostProcessID != string.Empty)
						script.WorkingSpace.SaveStorage(this._hostProcessID);

					// output succeeded signal to console
					// Console.WriteLine("File \"" + imageFile + "\" was processed successfully.");
				}
				catch (System.Exception exp)
				{
					string message = "Failed to execute script \"" + scriptFilename + "\". Reason: " + exp.ToString();

					// output to console for notification user
					Console.WriteLine(message);
			
					// output to trace log for troubleshooting
					Trace.WriteLine(message);
					Trace.WriteLine(exp);
				}
				finally
				{
					if (script != null)
					{
						// unregist for execution callback handler
						script.ExecuteCallback -= new ExecuteCallback(Script_ExecuteCallback);
						script.Dispose();
					}
					script = null;

					Console.WriteLine("Finish Processing.");
				}

				if (!_updater.IsSucceeded)
				{
					_errorCode = ErrorCodes.StatusQueueIsNotEmpty;
				}
			
				#endregion
			}
			finally
			{
				// clean up script
				if (script != null)
					script.Dispose();
				script = null;

				// signal processing flags
				this._isProcessing = false;
			}

			return _errorCode;
		}
		

		/// <summary>
		/// The <b>LaunchScript1</b> method executes script with the specified script file.
		/// </summary>
		/// <remarks>
		/// <p>The method <b>LaunchScript</b> first load the script file and check for images need to be
		/// processed. Then, it will launch another process with the specified script name and image file
		/// in order to process the image in another process.</p>
		/// <p>This technique prevents the memory-leaks problem and easier to develop.</p>
		/// <p>This function must return an error code instead of throwing any exception</p>
		/// </remarks>
		/// <param name="scriptFileName">The path of the script file.</param>
		public ErrorCodes LaunchScript1(string scriptFileName)
		{
			_errorCode = ErrorCodes.OK;
			Script script = null;

			try
			{
				// signal processing flags
				this._isProcessing = true;

				#region deserializes script specified by scriptFileName

				try
				{
					script = Script.Deserialize(scriptFileName);
				}
				catch (System.Exception exp)
				{
					Trace.WriteLine("Failed to read script \"" + scriptFileName + "\". Reason: " + exp.ToString());
				
					return ErrorCodes.FailedToDeserializeScript;
				}
				finally
				{
				}

				#endregion
			
				#region executing script by launching new process

				int globalCounter = 0;
				string[] runningFiles = script.GetRunFiles();
				if (runningFiles == null || runningFiles.Length == 0)
					return 0;

				Process process = null;

				foreach (string file in runningFiles)
				{
					// check for canceling signal
					if (this.IsCancelled)
					{
						_errorCode = ErrorCodes.ProcessWasCancelled;
						break;
					}

					// convert filename to lower case
					string fileTmp = file.ToLower();				
				
					// check if file is image
					if (IsImageFile(fileTmp) == false)
						continue;

					// increase global file counter
					globalCounter++;
				
					try
					{					
						// retrieve current process information
						Assembly assembly = typeof(ScriptLauncher).Assembly;
						Process currentProc = Process.GetCurrentProcess();
					
						// initializes process start info
						ProcessStartInfo newstart = currentProc.StartInfo;
						newstart.CreateNoWindow = true;
						newstart.FileName = string.Format("{0}", assembly.Location);
						newstart.WindowStyle = ProcessWindowStyle.Hidden;
						newstart.Arguments = string.Format("/s:\"{0}\" /i:\"{1}\" /c:{2} /nocopyright /nohelptext", scriptFileName, file, globalCounter);
						newstart.RedirectStandardOutput = true;
						newstart.UseShellExecute = false;
					
						// start new process with the specified start information.
						process = new Process();
						process.EnableRaisingEvents = true;
						process.StartInfo = newstart;

						// output information to trace log
						Trace.WriteLine("Launch new process with arguments: " + newstart.Arguments);

						if (false == process.Start())
							throw new System.ExecutionEngineException("Process can't be started");

						// save process ID for later use
						_processID = process.Id;

						// wait for the process finish process
						process.WaitForExit();

						// retrieve standard output of process
						string result = process.StandardOutput.ReadToEnd();

						// output to console
						Console.WriteLine(result);

						// check for process exit code to determine whether process has been working properly.
						if (process.ExitCode != 0)
						{
							// output result to tracelog
							Trace.WriteLine(result);

							if (process.ExitCode > (int)ErrorCodes.OK && process.ExitCode < (int)ErrorCodes.MaxErrorCode)
								_errorCode = (ErrorCodes)process.ExitCode;
							else
							{
								_errorCode = ErrorCodes.UndeterminedExitCode;
								Trace.WriteLine("Undetermined exit code: " + process.ExitCode);
							}

							break;
						}
					}
					catch (System.Exception exp)
					{
						Trace.WriteLine(exp.Message);
						Trace.WriteLine(exp.StackTrace);

						if (process != null)
						{
							try
							{
								if (!process.HasExited)
									process.Kill();
								_processID = -1;
							}
							catch
							{
								continue;
							}
						}
					}
				}

				#endregion
			}
			finally
			{
				// clean up script
				if (script != null)
					script.Dispose();
				script = null;

				// signal processing flags
				this._isProcessing = false;
			}
			
			return _errorCode;
		}


		/// <summary>
		/// The method <b>Run</b> is the main entry of the <b>ScriptLauncher</b>.
		/// </summary>
		/// <remarks>
		///	<p>The method <b>Run</b> is the main entry of the <b>ScriptLauncher</b>. When finish processing, 
		///	this method will return zero if succeeded, otherwise error code. To retrieve the error description of the specified error code. 
		///	Please use the static method <b>ScriptLauncher.GetErrorDescript(int _errorCode)</b>.</p>
		/// </remarks>
		/// <param name="args">command line arguments</param>
		/// <returns>Zero if succeeded, otherwise error code.</returns>
		public ErrorCodes Run(string[] args)
		{
			_errorCode = ErrorCodes.OK;

			try
			{
                Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);

				#region Initializes helpers

				// convert arguments to lower case because the command line parsing engine is case-sensitive
				for (int i=0; i<args.Length; i++)
					args[i] = args[i].ToLower();
				
				// create the command line parsing engine
				_commandLineParsingEngine = new CommandLineParsingEngine(args);			
				
				// parse the command line for debugging flags
				this.ParseCommandLineForArguments();
				
				// initialize command progress handler
				if (this._hostProcessID != null && this._hostProcessID != string.Empty)
				{
					string statusSharedMemoryName = SIA.Workbench.Common.UpdateStatusCommand.GetSharedStatusMappingFileName(_hostProcessID);
					_updater = new UpdateStatusCallbackEx(statusSharedMemoryName);
					CommandProgress commandInstance = CommandProgress.Instance;

                    //if (commandInstance != null)
                    //    CommandProgress.SetCallbackHandler(_updater);
                    if (commandInstance != null)
						CommandProgress.Instance.Callback = _updater;
				}

				// initialize console log helper
				//ConsoleLog.Initialize();
				//ConsoleLog.Enabled = !this._quiet;

				// initialize trace log helper
				TraceLog.Initialize(AppSettings.TraceLogFolder);
				TraceLog.Enabled = !_noLog;

				#endregion
	
				// print copyright text
				this.PrintCopyright();

				// determine running mode based on arguments
				if (!this._displayHelp)
				{
					if (!this._hasScriptFile)
					{
						Console.WriteLine("Script file was not specified.");
						Console.WriteLine("");

						// display help when argument is invalid
						_displayHelp = true;
						
						// return error code
						_errorCode = ErrorCodes.ScriptWasNotSpecified;
					}
					else
					{
						if (this._hasImageFile)
						{
							Trace.WriteLine(String.Format("Run script: \"{0}\" File: \"{1}\"", this._scriptFile, this._imageFile));
							_errorCode = this.LaunchScript(this._scriptFile, this._imageFile, this._startCounter);
						}
						else
						{
                            if (_hasImageFolder)
                            {
                                Trace.WriteLine(String.Format("Run script: \"{0}\"", this._scriptFile));
                                _errorCode = this.LaunchScript(this._scriptFile, this._imageFolder, this._imageFilter);
                            }
                            else
                            {
                                Trace.WriteLine(String.Format("Run script: \"{0}\"", this._scriptFile));
                                _errorCode = this.LaunchScript(this._scriptFile);
                            }
						}
					}
				}

				if (this._displayHelp)
					this.PrintHelpText();
			}
			catch (System.Exception exp)
			{
				Trace.WriteLine(exp);

				_errorCode = this.HandleErrorCodes(exp);
			}
			finally
			{
				if (this._waitForExit != null)
					this._waitForExit.Set();

				// uninitialize console log helper
				ConsoleLog.Uninitialize();

				// uninitialize trace log helper
				TraceLog.Uninitialize();
			}

			return _errorCode;
		}

        private bool _cancelKeyPress = false;
        private void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;

            if (!_cancelKeyPress)
            {
                Console.WriteLine("Cancel Key (Ctrl + C) has just been pressed. Please wait to complete.......");
            }

            _cancelKeyPress = true;
        }

		
		/// <summary>
		/// The <b>Terminate</b> method abort the processing
		/// </summary>
		public void Terminate()
		{
			if (this._processID > 0)
			{
				Process process = System.Diagnostics.Process.GetProcessById(_processID);
				if (process != null && !process.HasExited)
					process.Kill();
			}
		}

		/// <summary>
		/// The <b>WaitForExit</b> methods is used by another thread in order to wait for the main-thread finish processing
		/// </summary>
		/// <returns>True if succeeded, otherwise false</returns>
		public bool WaitForExit()
		{
			if (this._waitForExit != null)
				return this._waitForExit.WaitOne();
			return true;
		}


		/// <summary>
		/// The <b>WaitForExit</b> methods is used by another thread in order to wait for the main-thread finish processing
		/// </summary>
		/// <param name="milliseconds">The timeout</param>
		/// <returns>True if succeeded, otherwise false</returns>
		public bool WaitForExit(int milliseconds)
		{
			if (this._waitForExit != null)
				return this._waitForExit.WaitOne(milliseconds, true);
			return true;
		}

		
		#endregion

		#region Internal Event Handlers
		
		private void Script_ExecuteCallback(object sender, ScriptCallbackArgs args)
		{
			args.Cancel = this.IsCancelled;
		}

		private void Script_ExceptionCallback(object sender, ExceptionArgs args)
		{
			this._errorCode = this.HandleErrorCodes(args.Exception);
		}
		
		private ErrorCodes HandleErrorCodes(Exception exp)
		{
			return ErrorCodes.GenericException;
		}
		#endregion

		
	}
}
