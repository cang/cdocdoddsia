#if DEBUG
//    #define BUG_TRACE
#endif

using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;


using SIA.UI.Controls.Utilities;
using SIA.UI.Controls.Automation;
using SIA.UI.Controls.Automation.Steps;
using SIA.UI.Controls.Automation.Commands;
using SIA.UI.Controls.Commands;

using SIA.Workbench.Dialogs;
using SIA.Workbench.Common;
using SIA.Workbench.Common.InterprocessCommunication.SharedMemory;
using SIA.Workbench.Utilities;

namespace SIA.Workbench 
{
	/// <summary>
	/// Summary description for ScriptMonitor.
	/// </summary>
	[Serializable]
    internal class ScriptMonitor : IScriptMonitor, IDisposable 
	{
		#region constants
		private const int defaultWaitTimeOut = 500;
		//private const string launcherFilename = "SIA.Automation.Launcher.exe";
        private const string launcherFilename = "SIA.Automation.Launcher.exe";        
		#endregion

		#region member fields
		private string _scriptLauncherFilePath = null;
		private string _scriptFilePath = "";
		private Script _script = null;
		private int _timeOut = System.Threading.Timeout.Infinite;
		private Hashtable _scriptWorkspace = new Hashtable();
		
		private Thread _workerThread = null;

        private Process _workerProcess = null;
		private bool _workerProcessExited = false;
        
        private object _syncWorkerResult = new object();
        private WorkerProcessResult _workerResult = null;

		private AutoResetEvent _statusChangedWaitHandle = new AutoResetEvent(true);
		private ArrayList _localUpdateMsgBuffer = new ArrayList();

		private bool _useMsgBufferLock = true;
		private AutoResetEvent _msgBufferLock = new AutoResetEvent(true);

		#endregion

		#region events

		public event EventHandler StartProcessScript = null;
		public event EventHandler StopProcessScript = null;
		public event EventHandler BeginProcessFile = null;
		public event EventHandler EndProcessFile = null;
		public event EventHandler StatusChanged = null;
		public event EventHandler StatusBarChanged = null;
		public event EventHandler StartedScanningFolder = null;
		public event EventHandler ScanningFolder = null;
		public event EventHandler EndedScanningFolder = null;

		private bool _syncMessage = false;

		#endregion

		#region Properties

		public Script Script 
		{
			get {return _script;}
		}

		public String ScriptFileName 
		{
			get {return _scriptFilePath;}
		}

		public String ScriptLauncherFilePath 
		{
			get {return _scriptLauncherFilePath;}
			set {_scriptLauncherFilePath = value;}
		}

		public int TimeOut 
		{
			get {return _timeOut;}
			set {_timeOut = value;}
		}

		public Hashtable ScriptWorkspace
		{
			get {return _scriptWorkspace;}
			set {_scriptWorkspace = value;}
		}

		public bool UseMessageBufferLock 
		{
			get {return _useMsgBufferLock;}
			set {_useMsgBufferLock = value;}
		}

		public AutoResetEvent MessageBufferLock 
		{
			get {return _msgBufferLock;}
		}

		public bool SynchronousCallback 
		{
			get {return _syncMessage;}
			set {_syncMessage = value;}
		}
		
		#endregion

		#region Constructor and destructor

		public ScriptMonitor() 
		{
			// initialize script factory
			ScriptFactory.Initialize();
		}

		~ScriptMonitor() 
		{
			this.Dispose(false);
		}

		public void Dispose() 
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing) 
		{
			if (_statusChangedWaitHandle != null)
				_statusChangedWaitHandle.Close();
			_statusChangedWaitHandle = null;

			if (_localUpdateMsgBuffer != null)
				_localUpdateMsgBuffer.Clear();
			_localUpdateMsgBuffer = null;

			if (_msgBufferLock != null)
				_msgBufferLock.Close();
			_msgBufferLock = null;

			if (_workerProcess != null)
				_workerProcess.Dispose();
			_workerProcess = null;

			_workerThread = null;

			// uninitialize script factory
			ScriptFactory.Uninitialize();
		}

		#endregion

		#region methods
        private int _monitorFrequency = 300;
        public int MonitorFrequency
        {
            get { return _monitorFrequency; }
            set { _monitorFrequency = value; }
        }

		public void Start(string fileName) 
		{
			// validate arguments
			if (File.Exists(fileName) == false)
				throw new System.Exception("File " + fileName + " was not found.");
			
			try 
			{
				// deserialize script
				_script = Script.Deserialize(fileName);

				// save file path
				_scriptFilePath = fileName;

				// check if script launcher was installed properly
				string launcherFilePath = this.GetScriptLauncherPath();
				if (!File.Exists(launcherFilePath))
					throw new System.Exception(String.Format("Script Launcher \"{0}\" was not found", launcherFilePath));//"Script Launcher  was not found in the \"" + Application.StartupPath + "\" folder.");

				// start worker thread
				this.StartThread();
			}
			catch 
			{
				if (_script != null)
					_script.Dispose();
				_script = null;

				_scriptFilePath = null;

				throw;
			}
			finally 
			{
			}
		}


		/// <summary>
		/// Start processing the script file by ignoring the load image step in the script by the specified image file path 
		/// </summary>
		/// <param name="scriptFilePath">the full path to the script file</param>
		/// <param name="imageFilePath">the full path to the image file path</param>
		public void Start(string scriptFilePath, string imageFilePath) 
		{
			// validate arguments
			if (File.Exists(scriptFilePath) == false)
				throw new System.Exception("Script \"" + scriptFilePath + "\" was not found.");
			
			try 
			{
				// create new load image step from the specified image file path
				ProcessingData inputData = null;
				if (File.Exists(imageFilePath))
					inputData = new ProcessingData(imageFilePath);
				else if (Directory.Exists(imageFilePath))
					inputData = new ProcessingData(imageFilePath, false, false);
				else
					throw new System.Exception("Invalid file path " + imageFilePath);
				
				// load image command settings
				LoadImageCommandSettings settings = new LoadImageCommandSettings();
				settings.UseFilter = false;
				settings.Data = new ProcessingDataCollection();
				settings.Data.Add(inputData);				
				
				// deserialize script
				_script = Script.Deserialize(scriptFilePath);

				// replace the internal load image step by the new one
				Debug.Assert(_script.ProcessSteps[0].GetType() == typeof(LoadImageStep));
				LoadImageStep newStep = new LoadImageStep(settings);				
				_script.ProcessSteps[0] = newStep;

				// save file path
				_scriptFilePath = scriptFilePath;

				// check if script launcher was installed properly
				string launcherFilePath = this.GetScriptLauncherPath();
				if (!File.Exists(launcherFilePath))
					throw new System.Exception("Script Launcher was not found in the \"" + launcherFilePath + "\" folder.");

				// start worker thread
				this.StartThread();
			}
			catch 
			{
				if (_script != null)
					_script.Dispose();
				_script = null;

				_scriptFilePath = null;

				throw;
			}
			finally 
			{
			}
		}

		/// <summary>
		///  Start processing the script file by ignoring the load image step in the script by the specified image file path 
		/// </summary>
		/// <param name="scriptFilePath"></param>
		/// <param name="dataSource"></param>
		public void Start(string scriptFilePath, ProcessingDataCollection dataSource) 
		{
			// validate arguments
			if (File.Exists(scriptFilePath) == false)
				throw new System.Exception("Script \"" + scriptFilePath + "\" was not found.");

			if (dataSource == null)
				throw new ArgumentNullException("dataSource");
			if (dataSource.Count == 0)
				Trace.WriteLine("WARNING: Data Source is empty");
			
			try 
			{
				// create new load image step from the specified image file path
				bool recursive = false;
				foreach (ProcessingData data in dataSource) 
				{
					if (data.SubFolder) 
					{
						recursive = true;
						break;
					}
				}

				// deserialize script
				_script = Script.Deserialize(scriptFilePath);

				// replace the internal load image step by the new one
				if (_script.ProcessSteps.Count > 0 && _script.ProcessSteps[0] is LoadImageStep)
				{
					// load image command settings
					LoadImageCommandSettings settings = new LoadImageCommandSettings();
					settings.UseFilter = false;
					settings.ClearProcessedFileHistory = true;
					settings.ScanSubFolder = recursive;
					settings.Data = dataSource;

					// replace the step load image from file with the data provided by the IM1000
					LoadImageStep newStep = new LoadImageStep(settings);				
					_script.ProcessSteps[0] = newStep;

				}

				// save file path
				_scriptFilePath = scriptFilePath;

				// check if script launcher was installed properly
				string launcherFilePath = this.GetScriptLauncherPath();
				if (!File.Exists(launcherFilePath))
					throw new System.Exception("Script Launcher was not found in the \"" + launcherFilePath + "\" folder.");

				// start worker thread
				this.StartThread();
			}
			catch 
			{
				if (_script != null)
					_script.Dispose();
				_script = null;

				_scriptFilePath = null;

				throw;
			}
			finally 
			{
			}
		}

		public bool Stop() 
		{
			return this.StopThread(Timeout.Infinite);
		}

		public bool Stop(int waitTimeOut) 
		{
			return this.StopThread(waitTimeOut);
		}
		

		public StatusChangedEventArgs[] ReadAndClearStatusChangeArguments() 
		{
			StatusChangedEventArgs[] results = null;

			if (Monitor.TryEnter(_localUpdateMsgBuffer, 1000)) 
			{
				try 
				{
					results = (StatusChangedEventArgs[])_localUpdateMsgBuffer.ToArray(typeof(StatusChangedEventArgs));
					_localUpdateMsgBuffer.Clear();
				}
				catch (System.Exception exp) 
				{
					Trace.WriteLine(exp);
				}
				finally 
				{
					Monitor.Exit(_localUpdateMsgBuffer);
				}
			}

			return results;
		}

		public void WriteStatusChangeArguments(StatusChangedEventArgs args) 
		{
			if (Monitor.TryEnter(_localUpdateMsgBuffer)) 
			{
				try 
				{
					_localUpdateMsgBuffer.Add(args);
				}
				catch (System.Exception exp) 
				{
					Trace.WriteLine(exp);
				}
				finally 
				{
					Monitor.Exit(_localUpdateMsgBuffer);
				}
			}
		}

		#endregion

		#region override routines
		
		protected virtual bool OnStartProcessScript(StartProcessScriptEventArgs args) 
		{
#if BUG_TRACE
			// BUG NOTE: trace thread information
			string msg = string.Format("{1}: Thread {0} enter OnStartProcessScript", Thread.CurrentThread.Name, DateTime.Now.ToString());
			Trace.WriteLine(msg);
#endif

			if (this._syncMessage) 
			{
				if (this.StartProcessScript != null)
					this.StartProcessScript(this, args);
				return true;
			}
			else 
			{
				IAsyncResult ar = this.StartProcessScript.BeginInvoke(this, args, null, null);
				return ar.AsyncWaitHandle.WaitOne(defaultWaitTimeOut, true);
			}
		}

		protected virtual bool OnEndProcessScript(StopProcessScriptEventArgs args) 
		{
#if BUG_TRACE
			// BUG NOTE: trace thread information
			string msg = string.Format("{1}: Thread {0} enter OnEndProcessScript", Thread.CurrentThread.Name, DateTime.Now.ToString());
			Trace.WriteLine(msg);
#endif

			if (this._syncMessage) 
			{
				if (this.StopProcessScript != null)
					this.StopProcessScript(this, args);
				return true;
			}
			else 
			{
				IAsyncResult ar = this.StopProcessScript.BeginInvoke(this, args, null, null);
				return ar.AsyncWaitHandle.WaitOne(defaultWaitTimeOut, true);
			}
		}

		protected virtual bool OnBeginProcessFile(BeginProcessFileEventArgs args) 
		{
#if BUG_TRACE
			// BUG NOTE: trace thread information
			string msg = string.Format("{1}: Thread {0} enter OnBeginProcessFile", Thread.CurrentThread.Name, DateTime.Now.ToString());
			Trace.WriteLine(msg);
#endif

			if (this._syncMessage) 
			{
				if (this.BeginProcessFile != null)
					this.BeginProcessFile(this, args);
				return true;
			}
			else 
			{
				IAsyncResult ar = this.BeginProcessFile.BeginInvoke(this, args, null, null);
				return ar.AsyncWaitHandle.WaitOne(defaultWaitTimeOut, true);
			}
		}

		protected virtual bool OnEndProcessFile(EndProcessFileEventArgs args) 
		{
#if BUG_TRACE
			// BUG NOTE: trace thread information
			string msg = string.Format("{1}: Thread {0} enter OnEndProcessFile", Thread.CurrentThread.Name, DateTime.Now.ToString());
			Trace.WriteLine(msg);
#endif
			if (this._syncMessage) 
			{
				if (this.EndProcessFile != null)
					this.EndProcessFile(this, args);
				return true;
			}
			else 
			{
				IAsyncResult ar = this.EndProcessFile.BeginInvoke(this, args, null, null);
				return ar.AsyncWaitHandle.WaitOne(defaultWaitTimeOut, true);
			}
		}

		protected virtual bool OnStatusChanged(StatusChangedEventArgs args) 
		{
			try 
			{
#if BUG_TRACE
					// BUG NOTE: trace thread information
					string msg = string.Format("{1}: Thread {0} enter OnStatusChanged", Thread.CurrentThread.Name, DateTime.Now.ToString());
					Trace.WriteLine(msg);
#endif
				if (_syncMessage) 
				{
					if (this.StatusChanged != null)
						this.StatusChanged(this, args);
					return true;
				}
				else 
				{
					IAsyncResult ar = this.StatusChanged.BeginInvoke(this, args, null, null);
					return ar.AsyncWaitHandle.WaitOne(defaultWaitTimeOut, true);
				}
			}
			catch (System.Exception exp) 
			{
				Trace.WriteLine(exp);
			}
			finally 
			{
			}

			return true;
		}


		#endregion

		#region internal helpers

		private string GetScriptLauncherPath() 
		{
			if (this._scriptLauncherFilePath == null || this._scriptLauncherFilePath == string.Empty) 
			{				
				Assembly assembly = this.GetType().Assembly;
				// retrieves location from assembly information
				string dirName = Path.GetDirectoryName(assembly.Location);
				// generate full path of script launcher assembly
				_scriptLauncherFilePath = String.Format(@"{0}\{1}", dirName, launcherFilename);
			}

			return _scriptLauncherFilePath;
		}

		private void StartThread() 
		{
			_workerThread = new Thread(new ThreadStart(WorkerThread));
			_workerThread.Name = Guid.NewGuid().ToString();
			_workerThread.Start();
		}

		private bool StopThread(int waitTimeOut) 
		{
			bool retVal = false;
			
			if (_workerThread != null) 
			{
				_workerThread.Abort();
				retVal = _workerThread.Join(waitTimeOut);
				_workerThread = null;
			}

			return retVal;
		}

		private string CreateArguments(string scriptFilePath, string fileName, int fileCounter) 
		{
			string id = Process.GetCurrentProcess().Id.ToString();
			return String.Format("/s:\"{0}\" /i:\"{1}\" /c:{2} /nocopyright /nohelptext /p:{3} /timeOut:{4}", 
				scriptFilePath, fileName, fileCounter, id, _timeOut.ToString());
		}

		private Process CreateWorkerProcess(string scriptFilePath, string fileName, int fileCounter) 
		{
			// retrieve launcher file path
			string launcherFilePath = this.GetScriptLauncherPath();
			
			// initialize process start info
			ProcessStartInfo startInfo = new ProcessStartInfo(launcherFilePath);
			startInfo.CreateNoWindow = true;
			startInfo.WindowStyle = ProcessWindowStyle.Hidden;
			startInfo.Arguments = this.CreateArguments(scriptFilePath, fileName, fileCounter);
			startInfo.RedirectStandardOutput = true;
			startInfo.UseShellExecute = false;
					
			// start new _runningProcess with the specified start information.
			Process process = new Process();
			process.EnableRaisingEvents = true;
			process.StartInfo = startInfo;

			// register for process exit event
			process.Exited += new EventHandler(WorkerProcess_Exited);

			return process;
		}


		private void WorkerThread() 
		{
			string processID = Process.GetCurrentProcess().Id.ToString();
			DataProvider dataProvider = null;						
			SharedData sharedData = null;
			ScriptSharedMemory scriptSharedMemory = null;
			StatusSharedMemory statusShareMemory = null;
			
			try 
			{
				// raise StartProcessScript event
				RaiseThreadStarted();

				// initializes script share memory
				scriptSharedMemory = new ScriptSharedMemory(processID, true);

				// initializes status shared memory for updating operation status
				statusShareMemory = new StatusSharedMemory(processID);

				// initializes share data
				sharedData = new SharedData();
				
				bool bContinue = true;
				ArrayList fileCollection = new ArrayList();
				string[] fileList = null;
				int fileCounter = 0;
				int index = -1;

				// update status bar
				string statusBarText = "Loading script and scanning files to process ...";
				this.RaiseStatusBarChanged(statusBarText);

				// initialize data provider
				dataProvider = new DataProvider(_script);

                // register for data provider events
				dataProvider.StartedScanningFolder += new EventHandler(DataProvider_StartedScanningFolder);
				dataProvider.ScanningFolder += new EventHandler(DataProvider_ScanningFolder);
				dataProvider.EndedScanningFolder += new EventHandler(DataProvider_EndedScanningFolder);

				// signal data provider for starting data acquiring operation
				 fileCollection = dataProvider.BeginGetFiles();

                // write warning when data provider is empty
                if (fileCollection.Count <= 0) 
                    Trace.WriteLine("File list is empty.");
				
				// prepare for filtering input file
				dataProvider.PrepareProcessInputFileFilter();

				bool isFileStartedProcessing = false;
		
				#region Processing script				

				while (bContinue) 
				{
					#region processing loop

					#region retrieve and filtering file from data provider				

					string fileName = dataProvider.GetNextFile();

					// if no new file was found
					if (fileName == null && dataProvider.IsListening)  
					{ 
						// continue scanning folder
						bContinue = true;

						// update status bar
						if (statusBarText != "Listening incoming file ...") 
						{
							statusBarText = "Listening incoming file ...";
							this.RaiseStatusBarChanged(statusBarText);
						}

						//System.Threading.Thread.Sleep(1000);
                        System.Threading.Thread.Sleep(_monitorFrequency);
						continue;
					}

					// NOTES: New architect of RDEW
					//// no scan folder. All of static files and folders have been processed
					if (dataProvider.HasLoadImageStep && fileName == string.Empty) 
					{
						bContinue = false;
						continue;
					}

					#endregion

					// reset statusBarText
					statusBarText = null;

					try 
					{
						if (fileName != null && fileName != string.Empty)
						{
							// update file list
							index = fileCollection.IndexOf(fileName);
							if (index<0) 
							{
								if (fileCollection.Count > 0) 
								{
									fileCollection[fileCollection.Count-1] = fileName;
									index = fileCollection.Count-1;								
								}
								else
									index = fileCollection.Add(fileName);
							}

							fileList = (string[])fileCollection.ToArray(typeof(string));

							// increase file counter
							fileCounter++;

							// filtering the process file path						
							if (!dataProvider.ProcessFilterInputFile(fileName)) 
							{
								isFileStartedProcessing = false;
								continue;
							}
						}

						// signal file is started processing
						isFileStartedProcessing = true;

						// synchronize between processes
						// FileProcessorLocker.Begin();

						// use message buffer locking technique
						if (this._useMsgBufferLock)
							this._msgBufferLock.Reset();

						// raise BeginProcessFile event
						this.RaiseBeginProcessFile(fileList, index);
						
						// initialize script shared memory - script workspace
						if (this._scriptWorkspace != null)
							scriptSharedMemory.SetData(_scriptWorkspace);

						// initialize status queue shared memory
						statusShareMemory.SetStatusMessageQueue(new ShareMessageQueue());
					
						// initializes worker process
						_workerProcess = this.CreateWorkerProcess(_scriptFilePath, fileName, fileCounter);
						
						// reset exit flags
						lock (this) 
						{
							_workerProcessExited = false;
							_workerResult = null;
						}

						// start worker process
						bool isInfinite = this._timeOut == Timeout.Infinite;
						TimeSpan timeOut = new TimeSpan(0, 0, 0, 0, this._timeOut);
						DateTime startTime = DateTime.Now;

						if (false == _workerProcess.Start())
							throw new System.ExecutionEngineException("Process can't be started");

						// prepares local status queue
						Queue localQueue = new Queue();

						// clean up _localUpdateMsgBuffer
						_localUpdateMsgBuffer.Clear();

						bool sharedMemIsEmpty = false;
						while (_workerProcessExited == false || !sharedMemIsEmpty) 
						{
							#region retrieve update status message queue
							if (statusShareMemory != null) 
							{
								ShareMessageQueue statusQueue = null;
								try 
								{
									// acquire lock to share memory
									statusShareMemory.Lock();

									// retrieve status's message queue from the shared memory
									statusQueue = statusShareMemory.GetStatusMessageQueue();

									sharedMemIsEmpty = (statusQueue == null || statusQueue.Count == 0);

									if (statusQueue != null) 
									{
										// OPTIMIZED: raise event for the UI to update
										// this.RaiseStatusChanged(fileList, index, statusQueue);

										// clone a copy of statusQueue and enqueue to local queue buffer
										ShareMessageQueue localMessageQueue = new ShareMessageQueue();
										while (statusQueue.Count > 0) 
										{
											UpdateStatusCommand cmd = statusQueue.Dequeue();
											localMessageQueue.Enqueue(cmd);
										}
										// queue up the local MessageQueue into local Queue buffer
										localQueue.Enqueue(localMessageQueue);
								
										// clean up status Queue
										statusQueue.Clear();

										// update the shared message queue
										statusShareMemory.SetStatusMessageQueue(statusQueue);
									}
								}
								catch (System.Exception exp) 
								{
									Trace.WriteLine(exp);
								}
								finally 
								{
									// release lock to share memory
									statusShareMemory.Unlock();
								}
							}
							#endregion

							#region raise event for the UI to update

							while (localQueue.Count > 0) 
							{
								ShareMessageQueue statusQueue = (ShareMessageQueue)localQueue.Dequeue();
								if (statusQueue != null) 
								{
									try 
									{
										// OPTIMIZED: raise event for the UI to update
										this.RaiseStatusChanged(fileList, index, statusQueue);
										
										// INVOKE WORK AROUND: Timer support
										StatusChangedEventArgs args = new StatusChangedEventArgs(_scriptFilePath, fileList, index, statusQueue);
										this.WriteStatusChangeArguments(args);
									}
									catch (System.Exception exp) 
									{
										Trace.WriteLine(exp);
									}
									finally 
									{

									}
								}
							}

							#endregion

							#region determine if worker process has exited without raising event
							
							if (_workerProcess.HasExited) 
							{
								lock (this) 
								{
									_workerProcessExited = true;
								}
							}
							else // still working
							{ 								
								if (isInfinite == false) 
								{
									// check for timeout
									DateTime now = DateTime.Now;
									TimeSpan duration = now - startTime;
									// terminate worker process 'cos of timeout event
									if (duration > timeOut) 
									{										
										// close worker process
										_workerProcess.Refresh();
										
										// save worker process result
										lock(_syncWorkerResult) 
										{
											if (_workerResult == null) 
											{
												_workerResult = new WorkerProcessResult(_workerProcess);												
												_workerResult._exitCode = -3; // timeout
											}
										}

										// terminate worker process
										_workerProcess.Refresh();
										_workerProcess.Kill();
										_workerProcess.WaitForExit(100);

										// save worker process result
										lock(_syncWorkerResult) 
										{
											if (_workerResult != null)
												_workerResult._exitCode = -3; // timeout
										}

										_workerProcess = null;

										// simulate exception status messages
										ShareMessageQueue statusQueue = new ShareMessageQueue();
										Queue queue = new Queue();
										SharedData data = new SharedData();
										data.ActionType = SharedDataActionType.Exception;
										data.ExceptionMessage = "The operation was timeout";
										data.ExceptionStackTrace = "";										

										UpdateStatusCommand cmd = new UpdateStatusCommand(data);
										queue.Enqueue(cmd);
										statusQueue.Enqueue(queue);

										// OPTIMIZED: raise event for the UI to update
										this.RaiseStatusChanged(fileList, index, statusQueue);

										lock (this)
											_workerProcessExited = true;
									}
								}
							}

							// sleep before next cycle
							Thread.Sleep(200);

							#endregion
						} 
					}
					catch (System.Exception exp) 
					{
						Trace.WriteLine(exp.Message);
						Trace.WriteLine(exp.StackTrace);              
					}
					finally 
					{
						// Uninitializes worker process
						if  (_workerProcess != null) 
						{
							if (_workerProcess.HasExited == false) 
							{
								// terminate process
								_workerProcess.Kill();
							
								// wait until process exit
								_workerProcess.WaitForExit();
							}
							
							// save worker process result
                            lock (_syncWorkerResult) 
							{
								if (_workerResult == null)
									_workerResult = new WorkerProcessResult(_workerProcess);
							}

							_workerProcess = null;
						}

						// unitialize message buffer
						if (this._useMsgBufferLock && !this._msgBufferLock.WaitOne(2000, false)) 
						{
							Trace.WriteLine("Cannot wait to the UI is updated.");
						}

						if (isFileStartedProcessing) 
						{
							// raise EndProcessFile event
							this.RaiseEndProcessFile(fileList, index, _workerResult);
							isFileStartedProcessing = false;
						}

						// synchronize between processes
						// FileProcessorLocker.End();

						// suspend Thread in 0.05 second for updating the ui
						Thread.Sleep(50);

						// determine to be continue or not
						if (!dataProvider.HasLoadImageStep && fileName == null || fileName == string.Empty)
							bContinue = false;

					} // end finally

					#endregion // processing loop

				} // end while

				// signal data provider for finished data acquiring operation
				dataProvider.EndGetFiles();
			}
			catch  (System.Exception exp) 
			{
				Trace.WriteLine(exp.Message);
			}
			finally 
			{   
				// clean up data provider
				if (dataProvider != null)
				{
					dataProvider.StartedScanningFolder -= new EventHandler(DataProvider_StartedScanningFolder);
					dataProvider.ScanningFolder -= new EventHandler(DataProvider_ScanningFolder);
					dataProvider.EndedScanningFolder -= new EventHandler(DataProvider_EndedScanningFolder);

					dataProvider.Dispose();
					dataProvider = null;
				}

				// clean up script shared memory objects
				if  (scriptSharedMemory != null) 
				{
					scriptSharedMemory.Dispose();
					scriptSharedMemory = null;
				}

				// clean up status shared memory
				if (statusShareMemory != null) 
				{
					statusShareMemory.Dispose();
					statusShareMemory = null;
				}

				// raise StopProcessScript event
				this.RaiseThreadExited();

				// update status bar
				this.RaiseStatusBarChanged(string.Empty);

				#endregion
			}
		}

		private void RaiseThreadStarted() 
		{
			StartProcessScriptEventArgs args = new StartProcessScriptEventArgs(_scriptFilePath);
			
			this.OnStartProcessScript(args);
		}

		private void RaiseThreadExited() 
		{
			this.OnEndProcessScript(new StopProcessScriptEventArgs(_scriptFilePath));
		}

		private void RaiseBeginProcessFile(string[] fileList, int currentFileIndex) 
		{
			BeginProcessFileEventArgs args = new BeginProcessFileEventArgs(_scriptFilePath, fileList, currentFileIndex, null);
			
			this.OnBeginProcessFile(args);
		}

		private void RaiseEndProcessFile(string[] fileList, int currentFileIndex, WorkerProcessResult result) 
		{
			EndProcessFileEventArgs args = new EndProcessFileEventArgs(_scriptFilePath, fileList, currentFileIndex, result);
			
			this.OnEndProcessFile(args);
		}

		private void RaiseStatusChanged(string[] fileList, int currentFileIndex, SharedData sharedData) 
		{
			StatusChangedEventArgs args = new StatusChangedEventArgs(_scriptFilePath, fileList, currentFileIndex, sharedData);

			this.OnStatusChanged(args);
		}

		private void RaiseStatusChanged(string[] fileList, int currentFileIndex, ShareMessageQueue sharedData) 
		{
			StatusChangedEventArgs args = new StatusChangedEventArgs(_scriptFilePath, fileList, currentFileIndex, sharedData);

			this.OnStatusChanged(args);
		}

		private void RaiseStatusBarChanged(string statusText) 
		{
			if (statusText == null) statusText = string.Empty;
			StatusBarChangedEventArgs args = new StatusBarChangedEventArgs(statusText);

			if (this.StatusBarChanged != null)
				this.StatusBarChanged(this, args);
		}
		

		#endregion

		#region event handlers

		private void WorkerProcess_Exited(object sender, EventArgs e) 
		{
			lock (this) 
			{
				_workerProcessExited = true;
				_workerResult = new WorkerProcessResult((Process)sender);
			}
		}
		

		private void DataProvider_StartedScanningFolder(object sender, EventArgs args) 
		{
			if (this.StartedScanningFolder != null)
				this.StartedScanningFolder(this, args);
		}

		private void DataProvider_ScanningFolder(object sender, EventArgs args) 
		{
			if (this.ScanningFolder != null)
				this.ScanningFolder(this, args);
		}

		private void DataProvider_EndedScanningFolder(object sender, EventArgs args) 
		{
			if (this.EndedScanningFolder != null)
				this.EndedScanningFolder(this, args);
		}

		#endregion
		
	}

}
