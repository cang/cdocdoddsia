using System;
using System.Threading;
using System.Data;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Serialization;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

using SIA.Common;
using SIA.Common.Utility;

using SIA.UI.Controls;
using SIA.UI.Controls.Common;
using SIA.UI.Controls.Commands;

using SIA.SystemLayer;

using SIA.UI.Controls.Automation;
using SIA.UI.Controls.Automation.Steps;
using SIA.UI.Controls.Utilities;

using SIA.Plugins.Common;

using SIA.Workbench.Common;
using SIA.Workbench.Common.InterprocessCommunication.SharedMemory;
using SIA.Common.IPLFacade;

namespace SIA.UI.Controls.Automation 
{
	/// <summary>
	/// The script class is the core class of RDE Script. The script contains all the process steps 
    /// which are built by RDE Monitor
	/// </summary>
	[Serializable]
	public class Script 
        : IDisposable, ICloneable, ISerializable, IScript, IExecutionScript
	{	
		#region Member Fields

		public const int defaultTimeOut = System.Threading.Timeout.Infinite; // default timeout in milliseconds
		public static readonly int CurrentVersion = 1;

		private int _version = Script.CurrentVersion;
		private string _name = "";
		private string _description = "";

		private ProcessStepCollection _processSteps = new ProcessStepCollection();		
		private WorkingSpace _workingSpace = new WorkingSpace();
		private SharedData _statusData = new SharedData(); 

		private int _currentStepIndex = -1;
		
		#endregion

		#region Properties

        /// <summary>
        /// Gets or sets the version of the script
        /// </summary>
		public virtual int Version
		{
			get {return _version;}
			set {_version = value;}
		}

        /// <summary>
        /// Gets or sets name of the script
        /// </summary>
		public string Name 
		{
			get {return _name;}
			set {_name = value;}
		}

        /// <summary>
        /// Gets or sets the short description of the script
        /// </summary>
		public string Description
		{
			get {return _description;}
			set {_description = value;}
		}

        /// <summary>
        /// Gets number of process steps contains by the script
        /// </summary>
		[XmlIgnore]
		public int NumProcessSteps
		{
			get {return _processSteps.Count;}
		}

        /// <summary>
        /// Gets or sets list of process steps contains by the script
        /// </summary>
		[XmlArrayItem(typeof(ProcessStep))]
		public ProcessStepCollection ProcessSteps
		{
			get {return _processSteps;}
			set {_processSteps = value;}
		}

        /// <summary>
        /// Gets the current counter of the working space
        /// </summary>
		[XmlIgnore]
		public int Counter
		{
			set 
			{
				_workingSpace.Counter = value;
			}

            get 
            { 
                return _workingSpace.Counter; 
            }
		}

        private bool _isProcessingSuccessful = false;
        /// <summary>
        /// Gets the current counter of the working space
        /// </summary>
        [XmlIgnore]
        public bool IsProcessingSuccessful
        {
            set
            {
                _isProcessingSuccessful = value;
            }
            get
            {
                return _isProcessingSuccessful;
            }
        }

        /// <summary>
        /// Gets the status data which are shared between the working and hosting processes
        /// </summary>
		[XmlIgnore]
		public SharedData StatusData
		{
			get
			{
				return _statusData;
			}
		}

        /// <summary>
        /// Gets the working space that execute the script
        /// </summary>
		[XmlIgnore]
		public WorkingSpace WorkingSpace
		{
			get
			{
				return _workingSpace;
			}
		}

        /// <summary>
        /// Gets the index value of the current process step
        /// </summary>
		[XmlIgnore]
		public int CurrentStepIndex
		{
			get {return _currentStepIndex;}
			set {_currentStepIndex = value;}
		}

		#endregion

		#region Events

		public event ExecuteCallback ExecuteCallback;
		public event ExceptionCallback ExceptionCallback;

		public event EventHandler Serialized;
		public event EventHandler Deserialized;
	
		#endregion

		#region Constructor and Destructor

		public Script()
		{
			_workingSpace = new WorkingSpace(this);
			_processSteps = new ProcessStepCollection();		
			_statusData = new SharedData(); 
		}

		~Script()
		{
			this.Dispose(false);
		}
        		
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_workingSpace != null)
				{
					_workingSpace.Dispose();
					_workingSpace = null;
				}
			}
		}

		#endregion

		#region Methods

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion

		#region ISerializable Members

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			
		}

		#endregion

		#region Methods

		public virtual void Validate()
		{
			if (this._processSteps.Count <= 0)
				throw new ScriptValidationException("Processing steps is empty. Please specified 1 command at least.");

			foreach (ProcessStep command in this._processSteps)
			{
				try
				{
					command.Validate();					
				}
				catch (System.ArgumentException exp)
				{
					string msg = "Step " + command.Name + " is invalid. Reason: " + exp.Message;
					throw new ScriptValidationException(msg, exp);
				}
				catch (System.Exception exp)
				{
					string msg = "Step " + command.Name + " is invalid. Reason: Undetermined. Detailed:" + exp.ToString();
					throw new ScriptValidationException(msg, exp);
				}
				finally
				{

				}
			}
		}

		public virtual void Execute(string filename)
		{
			if (_workingSpace == null)
				throw new System.ArgumentException("Working space is null", "workingSpace");
			if (_processSteps == null || _processSteps.Count == 0)
				return;

            // Cong added
            _workingSpace.InternalReset();

			// initialize working space
			_workingSpace.ProcessingFileName =	filename;
			_workingSpace.ScriptName = this.Name;            
			
			int numSteps = _processSteps.Count;

            _isProcessingSuccessful = true;

			try
			{
				this.OnBeginProcessFile();

				// set current process step
				_currentStepIndex = 0;

				// check for finish condition
				while (_currentStepIndex < numSteps)
				{
					ProcessStep step = null;				
					
					try
					{
						// retrieve process step
						step = _processSteps[_currentStepIndex] as ProcessStep;

						// start processing step
						this.OnBeginStep(step, _currentStepIndex);

						// invoke callback for information
						int percentage = (int)(_currentStepIndex*100.0F/numSteps);
						ScriptCallbackArgs args = this.InvokeCallback("Execute step " + step.DisplayName, percentage);

						// determine if the operation was requested to cancel
                        if (args.Cancel)
                        {
                            _isProcessingSuccessful = false;
                            break;
                        }

						step.Execute(_workingSpace);
					}
					catch (System.Exception exp)
					{
                        _isProcessingSuccessful = false;

                        Console.WriteLine("Error: {0}", exp.Message);

						// output to trace log
						Trace.WriteLine(exp);

						// send exception notification
						OnStepException(step, exp);
			
						// break execution
						break;
					}
					finally
					{
						// finish processing step
						this.OnEndStep(step);

						// step to next process step
						_currentStepIndex++;
					}
				}//while
			}
			finally
			{
				OnEndProcessFile();	
			}
		}


		public void Serialize(string filename)
		{
			using (FileStream fileStream = new FileStream(filename, FileMode.Create))
			{
				this.Serialize(fileStream);
			}
		}
		
		public virtual void Serialize(Stream stream)
		{
			if (stream == null)
				throw new ArgumentNullException("stream", "Stream was not set to a reference.");

			using (StreamWriter writer = new StreamWriter(stream, ProcessStep.DefaultEncoding))
			{
				using (ScriptSerializer serializer = new ScriptSerializer())
				{
					serializer.Serialize(writer, this);

					if (this.Serialized != null)
						this.Serialized(this, EventArgs.Empty);
				}
			}
		}

		private static Script DoDeserialize(string filename)
		{
			try
			{
				FileStream fileStream = new FileStream(filename, FileMode.Open);
				/// Important: Do not create an instance of Script directly from default constructor. 
				/// Using ScriptFactory instead to retrieve an instance of Script with latest version.
				// Script script = new Script();
				Script script = ScriptFactory.CreateInstance();
				script.Deserialize(fileStream);
				return script;
			}
			catch
			{
				throw;
			}
		}

		// using Mutex to prevent more than two actions access to file
		// clone the fixed from RDE-NANDA
		public static Script Deserialize(string filename)
		{
			Mutex deserializeMutex = null;
			try
			{
				deserializeMutex = new Mutex(false, "SIA_SCRIPT_LAUNCHER_DESERIALIZE_SCRIPT");
				deserializeMutex.WaitOne();
				return DoDeserialize(filename);
			}
			catch
			{
				throw;
			}
			finally
			{
				if (deserializeMutex != null)
				{
					deserializeMutex.ReleaseMutex();
					deserializeMutex = null;
				}
			}
		}

		public virtual void Deserialize(Stream stream)
		{
			if (stream == null)
				throw new ArgumentNullException("stream", "Stream was not set to a reference.");

			using (StreamReader reader = new StreamReader(stream, ProcessStep.DefaultEncoding))
			{
				using (ScriptSerializer serializer = new ScriptSerializer())
				{
					using (Script script = serializer.Deserialize(reader))
					{
						this.Copy(script);

						if (this.Deserialized != null)
							this.Deserialized(this, EventArgs.Empty);
					}
				}
			}
		}

		
		public string[] GetRunFiles()
		{
			if (_processSteps == null || _processSteps.Count < 1 ||
				_processSteps[0].GetType() != typeof(LoadImageStep))
				return null;

			return (_processSteps[0] as LoadImageStep).GetRunFiles();
		}
		
		#region ICloneable Members

		public virtual void Copy(Script script)
		{
			if (script == null)
				throw new ArgumentNullException("script", "script param is not set to a reference.");
			
			this._version = script._version;
			if (script._processSteps != null)
				this._processSteps = (ProcessStepCollection)script._processSteps.Clone();
			else
				this._processSteps = null;
		}

		public object Clone()
		{
			return this.MemberwiseClone();
		}

		#endregion	
	
		#endregion

		#region Internal Helpers

		protected virtual void OnBeginProcessFile()
		{
			// initialize temporary file for process-communication
			//string fileName = this._workingSpace.ProcessingFileName;
			//string tempFileName = fileName + ".xml";
		}

		protected virtual void OnEndProcessFile()
		{
			// update temporary files
			this.UpdateTemporaryFile(_statusData);
		}

        Stopwatch sw = new Stopwatch();
		protected virtual void OnBeginStep(ProcessStep step, int stepIndex)
		{
			if (_statusData == null)
				return ;

			string stepName = step.DisplayName;
			int numSteps = this._processSteps.Count;

			// update status data
			lock (_statusData)
			{
				_statusData.ActionType = SharedDataActionType.Begin;
				_statusData.StepIndex = stepIndex;
				_statusData.StepName = stepName;
				_statusData.OverallProgress =(int)((stepIndex + 1) * 100.0F / numSteps);
                //_statusData.StartTime = DateTime.Now;
				//_statusData.EndTime = DateTime.MinValue;

                sw.Reset();
                sw.Start();
			}

			// update progress callback
			this.UpdateStatus(_statusData);
		}

		protected virtual void OnEndStep(ProcessStep step)
		{
			if (_statusData == null)
				return;

			// update status data
			lock (_statusData)
			{
				_statusData.ActionType = SharedDataActionType.End;
				_statusData.StepProgress = 100;
				//_statusData.EndTime = DateTime.Now;

                sw.Stop();
                _statusData.EndTime = DateTime.Now;
                _statusData.StartTime = _statusData.EndTime.AddMilliseconds(-sw.ElapsedMilliseconds);
			}
			
			// update callback progress 
			this.UpdateStatus(_statusData);
		}

		protected virtual void OnStepException(ProcessStep step, System.Exception exp)
		{
#if DEBUG
			//Debug.Assert(false);
#endif
			if (_statusData == null)
				return;

			if (ExceptionCallback != null)
				this.ExceptionCallback(this, new ExceptionArgs(exp));
			
			// update status data
			lock (_statusData)
			{
				_statusData.ExceptionMessage = exp.Message;
				_statusData.ExceptionStackTrace = exp.StackTrace;
				_statusData.ActionType = SharedDataActionType.Exception;
				_statusData.StepProgress = 100;
				_statusData.EndTime = DateTime.Now;

				// update log file name
				_statusData.LogFileName = TraceLog.FileName;
			}

			// update callback progress 
			this.UpdateStatus(_statusData);			

			// update temporary file
			this.UpdateTemporaryFile(_statusData, exp);
		}

		private void UpdateStatus(SharedData statusData)
		{
			SIA.SystemFrameworks.UI.CommandProgress.SetUserData(statusData);
		}

		public static string GetTemporaryFilePath(string fileName)
		{
			string tempPath = Path.GetTempPath();
			return Path.Combine(tempPath, Path.GetFileName(fileName) + ".xml");
		}

		private void UpdateTemporaryFile(SharedData statusData)
		{
			this.UpdateTemporaryFile(statusData, null);
		}

		private void UpdateTemporaryFile(SharedData statusData, Exception exp)
		{
			if (this._workingSpace == null)
				return;
			ScriptData scriptData = this._workingSpace.ScriptData;
			if (scriptData == null)
				return;
			
			string fileName = this._workingSpace.ProcessingFileName;
			string tempFileName = Script.GetTemporaryFilePath(fileName);
			if (File.Exists(tempFileName) == false)
				return;

			if (this._workingSpace.Image != null)
			{
				// refresh image data
				this.UpdateImageData();
				// refresh wafer boundary data
				//this.UpdateWaferBoundaryData();
			}

			// refresh exception data
			if (exp != null)
			{
				scriptData.HasException = true;
				scriptData.Message = exp.Message;
				scriptData.StackTrace = exp.StackTrace;				
			}

			// write updated information
			FileStream stream = null;
			try
			{
				stream = File.OpenWrite(tempFileName);
				// serialize xml data into memory stream
				XmlSerializerEx serializer = new XmlSerializerEx(typeof(ScriptData));
				serializer.Serialize(stream, scriptData);
			}
			catch (Exception e)
			{
				Trace.WriteLine(e);				
			}
			finally
			{
				if (stream != null)
					stream.Close();
				stream = null;
			}			
		}

		private void UpdateUserData(SharedData statusData)
		{
			if (this._workingSpace != null && this._workingSpace.Image != null)
			{
				//Debug.Assert(false);

				// refresh image data
				this.UpdateImageData();
				
				// send updated information
				ScriptData scriptData = this._workingSpace.ScriptData;
				using (MemoryStream stream = new MemoryStream())
				{
					// serialize xml data into memory stream
					XmlSerializerEx serializer = new XmlSerializerEx(typeof(ScriptData));
					serializer.Serialize(stream, scriptData);

					// retrieve buffer from memory stream
					stream.Seek(0, SeekOrigin.Begin);
					byte[] bytes = stream.ToArray();

					// encode and insert into shared data
					statusData.EncodeUserData(bytes);
				}
			}
			else
			{
				statusData = null;
			}
		}

		private ScriptCallbackArgs InvokeCallback(string message, int percentage)
		{
			ScriptCallbackArgs callbackArgs = new ScriptCallbackArgs(message, percentage);
			if (this.ExecuteCallback != null)
				this.ExecuteCallback(this, callbackArgs);
			return callbackArgs;
		}

		private void UpdateImageData()
		{
			if (this._workingSpace == null)
				throw new ArgumentNullException("_workingSpace");
			ScriptData scriptData = this._workingSpace.ScriptData;
			if (scriptData == null)
				throw new ArgumentNullException("scriptData");

			CommonImage image = this._workingSpace.Image;
			if (image != null)
			{
				scriptData.Width = image.Width;
				scriptData.Height = image.Height;
				if (image.Properties != null)
					scriptData.Properties= image.Properties.Clone() as RasterImagePropertyCollection;
			}
		}
		#endregion

		#region IScript Members

		public IProcessStep[] GetProcessSteps()
		{
			return this._processSteps.ToArray();
		}

		#endregion
	}
}
