using System;
using System.Data;
using System.Collections;
using System.Threading;
using System.IO;
using System.Diagnostics;

using SIA.SystemFrameworks;
using SIA.SystemFrameworks.UI;

using SIA.SystemLayer;
using System.Configuration;

namespace SIA.UI.Controls.Commands
{
	/// <summary>
	/// The RasterCommand provides basic implementation for a command
	/// </summary>
	public abstract class RasterCommand 
        : IDisposable
	{
		#region Fields

		private Thread _workerThread = null;
		private bool _bWaitForFinishProcessing = false;
		private AutoResetEvent _waitEvent = new AutoResetEvent(false);
		protected IProgressCallback _callback = null;
		private object[] _arguments = null;
		private RasterCommandSettings _settings = null;

        protected RasterCommandResult _result = RasterCommandResult.Ok;
		protected Exception _exception = null; 

        RasterCommandPerformanceCounterManager _counterManager = null;
		DateTime _startTime = DateTime.Now;
		DateTime _finishTime = DateTime.Now;


		#endregion
		
		#region properties

        /// <summary>
        /// Gets the progress callback 
        /// </summary>
		public IProgressCallback ProgressCallback
		{
			get {return _callback;}
		}

        /// <summary>
        /// Gets boolean value indicates the command is abortable
        /// </summary>
		public virtual bool CanAbort
		{
			get {return false;}
		}

        /// <summary>
        /// Gets boolean value indicates the command should stop processing
        /// </summary>
		public bool IsAbort
		{
			get 
			{
				if (_callback != null)
					return _callback.IsAborting;
				return false;
			}
		}

        /// <summary>
        /// Gets the result of command
        /// </summary>
		public RasterCommandResult Result
		{
			get {return _result;}
		}

        /// <summary>
        /// Gets the exception that was thrown by the command
        /// </summary>
		public Exception Exception
		{
			get {return _exception;}
		}

		public RasterCommandSettings Settings
		{
			get {return _settings;}
		}

		public bool WaitForFinish
		{
			get {return _bWaitForFinishProcessing;}
			set {_bWaitForFinishProcessing = value;}
		}

		#endregion

		#region Events

		public event EventHandler ThreadBegin;
		public event EventHandler ThreadEnd;
		public event RasterCommandExceptionHandler ThreadException;

		#endregion

		#region Constructor and destructor

		public RasterCommand()
		{
			this.InitClass();
		}

		public RasterCommand(IProgressCallback callback)
		{
			this._callback = callback;
			this.InitClass();
		}

		~RasterCommand()
		{
			this.Dispose(false);
		}

		protected virtual void InitClass()
		{
		}

		protected virtual void UninitClass()
		{
			this._workerThread = null;
			this._arguments = null;
			this._waitEvent = null;
		}

		#region IDisposable Members

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			this._callback = null;
			this.UninitClass();
		}

		#endregion

		#endregion

		#region public methods

		public virtual RasterCommandResult Run(params object[] args)
		{
			// validate arguments
			this.ValidateArguments(args);

			try
			{
				this._arguments = args;

				lock (this)
				{
					ThreadStart start = new ThreadStart(WorkerThread);
					_workerThread = new Thread(start);
					_workerThread.Name = this.GetType().Name + "_WorkerThread";
					_workerThread.Priority = ThreadPriority.Normal;
					_workerThread.Start();
				}

				// wait for worker thread finish its processing
				if (_bWaitForFinishProcessing && _waitEvent != null)
					_waitEvent.WaitOne();
			}
			catch(Exception exp) // non-processing exception
			{
				throw exp;
			}
			finally
			{	
			}

			return _result;
		}

	
		public virtual object[] GetOutput()
		{
			return null;
		}
				

		public virtual void LoadSettings(string filename)
		{
			if (filename == null)
				throw new ArgumentNullException("filename");
			if (File.Exists(filename) == false)
				throw new FileNotFoundException("Settings file was not found", filename);
			if (this.Settings == null)
				throw new System.InvalidOperationException("This command does not support serialize settings");
			
		}

		public virtual void SaveSettings(string filename)
		{
			if (filename == null)
				throw new ArgumentNullException("filename");	
			if (this.Settings == null)
				throw new System.InvalidOperationException("This command does not support serialize settings");
		}


		#endregion

		#region Override Methods
		
		protected abstract void ValidateArguments(params object[] args);
		protected abstract void OnRun(params object[] args);
        		
		protected virtual void WorkerThread()
		{
			try
			{
				// TRICK: wait for callback to finish its initialization
				if (_callback != null)
				{
					_callback.Begin(0, 100);
					_callback.SetText("Start processing...");
				}

				// initialize result flags
				_result = RasterCommandResult.Ok;
			
				// raise thread begin event
				this.OnThreadBegin();

				// start processing routine
				this.OnRun(this._arguments);
			}
			catch (System.Exception exp)
			{
				// signal error flags
				_result = RasterCommandResult.Failed;
				_exception = exp;

				// raise thread exception event
				this.OnThreadException(exp);
			}
			finally
			{
				// signal abort flags (if any)
				if (this.IsAbort)
					_result = RasterCommandResult.Aborted;

				// raise thread end event
				this.OnThreadEnd();

				// signal wait handle (Run method)
				_waitEvent.Set();
			}
		}

		protected virtual void OnThreadBegin()
		{
			// log command start information
			this.OnLogCommandStartInformation();

			if (this.ThreadBegin != null)
				this.ThreadBegin(this, EventArgs.Empty);
		}

		protected virtual void OnThreadEnd()
		{
			// log command finish information
			this.OnLogCommandFinishInformation();

			if (this.ThreadEnd != null)
				this.ThreadEnd(this, EventArgs.Empty);
		}

		protected virtual void OnThreadException(System.Exception exp)
		{
			// log command exception information
			this.OnLogCommandExceptionInformation();

			if (this.ThreadException != null)
				this.ThreadException(this, new RasterCommandExceptionArgs(this, exp));
		}

		
		#region callback status helper

		protected virtual void SetStatusText(string text)
		{
			if (this._callback	!= null)
				this._callback.SetText(text);
		}

		protected virtual void SetStatusRange(int min, int max)
		{
			if (this._callback	!= null)
				this._callback.SetRange(min, max);
		}

		protected virtual void SetStatusValue(int value)
		{
			if (this._callback	!= null)
				this._callback.StepTo(value);
		}

		#endregion

		#region console log helpers

		protected bool PerformanceLogEnabled
		{
			get
			{
				const string KEY_PERFORMANCE_LOG = "EnableRasterCommandPerformanceLog";
				object value = ConfigurationManager.AppSettings[KEY_PERFORMANCE_LOG];
				if (value != null)
				{
					try {
						return Boolean.Parse(value.ToString());
					}
					catch {
					}

					return false;
				}
				else
					return false;
			}
		}
		
		protected virtual void OnLogCommandStartInformation()
		{			
			try
			{
				// start record information
				_startTime = DateTime.Now;

				// log start information
				Console.WriteLine("Start Command: {0} at {1} {2}", this.GetType().Name, _startTime.ToShortDateString(), _startTime.ToShortTimeString());

				// initialize performance counters
				_counterManager = new RasterCommandPerformanceCounterManager();
				_counterManager.StartCounters();
			}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);
			}
			finally
			{
			}
		}

		protected virtual void OnLogCommandFinishInformation()
		{
			try
			{
				// finish record information
				_finishTime = DateTime.Now;

				// uninitialize performance counters
				if (this._counterManager != null)
				{
					// stop performance counter
					this._counterManager.StopCounters();
					
					// log performance counter data
					this._counterManager.LogPerformanceData();

					// uninitialize performance counters
					this._counterManager.Dispose();
				}
				this._counterManager = null;

			}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);
			}
			finally
			{
				// log end information
				Console.WriteLine("Finish Command: {0} at {1} {2}", this.GetType().Name, _finishTime.ToShortDateString(), _finishTime.ToShortTimeString());
				Console.WriteLine("");
			}
		}

		protected virtual void OnLogCommandExceptionInformation()
		{
			string cmdName = this.GetType().Name;
			string sepLine = new String('-', 80);

			Trace.WriteLine(sepLine);
			Trace.WriteLine("Begin CommandException Trace");
			Trace.WriteLine("Version: 1.0");

			try
			{
				Trace.WriteLine("Command " + cmdName + " generate an exception:");
				Trace.WriteLine("Exception Detail:");
				Trace.WriteLine(_exception);
			}
			catch
			{
			}
			finally
			{
				Trace.WriteLine("End CommandException Trace");
				Trace.WriteLine(sepLine);
			}
		}

		#endregion


		#endregion		
	}
}
