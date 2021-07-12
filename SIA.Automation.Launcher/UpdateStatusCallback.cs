using System;
using System.Collections;
using System.Diagnostics;
using System.IO;

using SIA.SystemFrameworks.UI;
using SIA.Workbench.Common;
using SIA.Workbench.Common.InterprocessCommunication.SharedMemory;
using SIA.UI.Controls.Utilities;


namespace SIA.Automation.Launcher
{
	/// <summary>
	/// The <b>UpdateStatusCallback</b> implemente the <b>IProgressCallback</b> to provide 
	/// the status information in the way of single message is stored.
	/// </summary>
	public class UpdateStatusCallback : IProgressCallback
	{
		#region member fields

		int _current  = 0;
		string _text = string.Empty;
		private SharedData _statusData = null;
		protected string _sharedMemName = SharedData.SharedStatusMappingFileName;
		
		#endregion

		#region Public Events
		/// <summary>
		/// This event is raised every time the user request for aborting the current command
		/// </summary>
		public event EventHandler Abort;
		#endregion

		#region Constructors and destructor
		/// <summary>
		/// The <b>UpdateStatusCallback</b> method updates the status of the callback progress
		/// </summary>
		/// <param name="statusData">The <see cref="SharedData"/> object stores the status information.</param>
		public UpdateStatusCallback(SharedData statusData)
		{
			this._statusData = statusData;
		}

		/// <summary>
		/// The <b>UpdateStatusCallback</b> method updates the status of the callback progress
		/// </summary>
		/// <param name="statusData">The <see cref="SharedData"/> object stores the status information.</param>
		/// <param name="sharedMemName">The <see cref="SharedData"/> Shared Memory File name</param>
		public UpdateStatusCallback(SharedData statusData, string sharedMemName)
		{
			this._statusData = statusData;
			this._sharedMemName = sharedMemName;
		}

		#endregion

		#region Methods
		/// <summary>
		/// Call this method from the worker thread to initialize
		/// the progress callback.
		/// </summary>
		/// <param name="minimum">The minimum value in the progress range (e.g. 0)</param>
		/// <param name="maximum">The maximum value in the progress range (e.g. 100)</param>
		public virtual void Begin( int minimum, int maximum )
		{
			this.Begin();
		}

		/// <summary>
		/// Call this method from the worker thread to initialize
		/// the progress callback, without setting the range
		/// </summary>
		public virtual void Begin()
		{
			_current = 0;
		}

		/// <summary>
		/// Call this method from worker thread to initialize
		/// the progress callback with automatic update after specified millisecond
		/// </summary>
		/// <param name="millisecond"></param>
		public virtual void Begin( int millisecond )
		{
			this.Begin();
		}

		/// <summary>
		/// Call this method from worker thread to initialize
		/// the progress callback with capable of aborting the current operation
		/// </summary>
		/// <param name="enable"></param>
		public virtual void SetAbort(bool enable)
		{
			if (enable && this.Abort != null)
				this.Abort(this, EventArgs.Empty);				
		}

		/// <summary>
		/// Call this method from the worker thread to reset the range in the progress callback
		/// </summary>
		/// <param name="minimum">The minimum value in the progress range (e.g. 0)</param>
		/// <param name="maximum">The maximum value in the progress range (e.g. 100)</param>
		/// <remarks>You must have called one of the Begin() methods prior to this call.</remarks>
		public virtual void SetRange( int minimum, int maximum )
		{
		}

		/// <summary>
		/// Call this method form the worker thread to reset autotick millisecond value in the progress callback
		/// </summary>
		/// <param name="milliseconds">
		/// > 0 : enable auto tick
		/// <= 0 : disable auto tick
		/// </param>
		public virtual void SetAutoTick( int milliseconds )
		{
		}

		/// <summary>
		/// Call this method from the worker thread to update the progress text.
		/// </summary>
		/// <param name="text">The progress text to display</param>
		/// <remarks>You must have called one of the Begin() methods prior to this call.</remarks>
		public virtual void SetText( String text )
		{
			this._text = text;
		}

		/// <summary>
		/// Call this method from the worker thread to increase the progress counter by a specified value.
		/// </summary>
		/// <param name="val">The amount by which to increment the progress indicator</param>
		/// <remarks>You must have called one of the Begin() methods prior to this call.</remarks>
		public virtual void StepTo( int val )
		{
			if (_current == val)
				return;
			_current = val;
			
			if (_statusData.StepProgress != _current)
			{
				this._statusData.StepProgress = _current;

				lock (this) // critical section
				{
					Segment segment = null;
					try
					{
						segment = new Segment(_sharedMemName, SharedMemoryCreationFlag.Attach, SharedData.SharedStatusMemSize);
						segment.Lock();
						segment.SetData(_statusData);
					}
					catch (System.Exception exp)
					{
						Trace.WriteLine(exp);						
					}
					finally
					{
						if (segment != null)
						{
							segment.Unlock();
							segment.Dispose();
							segment = null;
						}							
					}
				}
			}

		}

		/// <summary>
		/// Call this method from the worker thread to step the progress meter to a particular value.
		/// </summary>
		/// <param name="val">The value to which to step the meter</param>
		/// <remarks>You must have called one of the Begin() methods prior to this call.</remarks>
		public virtual void Increment( int val )
		{
			this.StepTo(_current + val);
		}

		/// <summary>
		/// If this property is true, then you can abort work
		/// </summary>
		public virtual bool CanAbort 
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// If this property is true, then you should abort work
		/// </summary>
		/// <remarks>You must have called one of the Begin() methods prior to this call.</remarks>
		public virtual bool IsAborting
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Call this method from the worker thread to finalize the progress meter
		/// </summary>
		/// <remarks>You must have called one of the Begin() methods prior to this call.</remarks>
		public virtual void End()
		{
			_current = 100;
			//khong hieu
		}

		/// <summary>
		/// The <b>SetStatusData</b> method sets the status information
		/// </summary>
		/// <param name="statusData">The <see cref="SharedData"/> object stores the status information.</param>
		public virtual void SetStatusData(SharedData statusData)
		{
			this._statusData = statusData;
		}
		/// <summary>
		/// Call this method from the worker thread to set the user data associated with the current object.
		/// </summary>
		/// <param name="obj">The reference to the user data</param>
		public virtual void SetUserData(object obj)
		{
		}

		#endregion

		public virtual  bool IsSucceeded
		{
			get
			{
				return true;
			}
		}
	}

	/// <summary>
	/// The <b>UpdateStatusCallbackEx</b> inherits the <b>UupdateStatusCallback</b> class to provide 
	/// the status information in the way of queue message is stored.
	/// </summary>
	public class UpdateStatusCallbackEx : UpdateStatusCallback
	{
		#region Member Fields

		private Queue _internalQueue = new Queue();

		#endregion

		#region constructor and destructor
		/// <summary>
		/// Default constructor
		/// </summary>
		public UpdateStatusCallbackEx() : base(null)
		{
		}

		public UpdateStatusCallbackEx(string sharedMemName) : base(null, sharedMemName)
		{
		}

		#endregion

		#region Properties

		public override bool IsSucceeded
		{
			get
			{
				return _internalQueue == null || _internalQueue.Count == 0;
			}
		}
		#endregion

		#region internal helpers
		
		private void Reset()
		{
			// reset shared data queue buffer
			if (_internalQueue != null)
			{
				lock (_internalQueue)
					_internalQueue.Clear();
			}
			
			_internalQueue = new Queue();
		}

		private void EnqueueStatusCommand(string text)
		{
			UpdateStatusCommand command = new UpdateStatusCommand(text);
			this.EnqueueStatusCommand(command);
		}

		private void EnqueueStatusCommand(float percentage)
		{
			UpdateStatusCommand command = new UpdateStatusCommand(percentage);
			this.EnqueueStatusCommand(command);
		}

		private void EnqueueStatusCommand(SharedData data)
		{
			UpdateStatusCommand command = new UpdateStatusCommand(data);
			this.EnqueueStatusCommand(command);
		}

		private void EnqueueStatusCommand(UpdateStatusCommand command)
		{
			if (_internalQueue == null)
				return;

			lock (_internalQueue)
			{
				_internalQueue.Enqueue(command);
			}
		}

		private UpdateStatusCommand DequeueStatusCommand()
		{
			if (_internalQueue == null)
				return null;

			lock (_internalQueue)
			{
				return (UpdateStatusCommand) _internalQueue.Dequeue();
			}
		}

		#region old version
		/**
		 * // it's old version which was deadlock. I'v just updated the fixed version from RDE-NANDA as follow.
		 * 				       
		private void UpdateSharedData()
		{
			Segment shareMemory = null;
			try
			{
				shareMemory = new Segment(UpdateStatusCommand.SharedStatusMappingFileName, SharedMemoryCreationFlag.Attach, SharedData.SharedStatusMemSize);
				shareMemory.Lock();
				
				ShareMessageQueue messageQueue = this.ReadMessageQueue(shareMemory);
				if (messageQueue == null)
				{
					messageQueue = new ShareMessageQueue();
				}
				else // compare and write only differences
				{
					lock (_internalQueue)
					{
						messageQueue.Enqueue(_internalQueue);
						_internalQueue.Clear();
					}
				}

				// write the updated message queue
				this.WriteMessageQueue(shareMemory, messageQueue);
			}
			catch (System.Exception exp)
			{
				Trace.Write(exp);
			}
			finally
			{
				if (shareMemory != null)
				{
					shareMemory.Unlock();
					shareMemory.Dispose();
				}
				shareMemory = null;
			}
		}
		**/
		#endregion old version

		// [20070528] Cong: update the fixed version from other branch
		private void UpdateSharedData()
		{
			// locked flag
			bool bLocked = false;

			Segment shareMemory = null;
			try
			{
				shareMemory = new Segment(_sharedMemName, SharedMemoryCreationFlag.Attach, SharedData.SharedStatusMemSize);
				bLocked = shareMemory.Lock();
				if (bLocked)
				{				
					ShareMessageQueue messageQueue = this.ReadMessageQueue(shareMemory);
					if (messageQueue == null)
					{
						messageQueue = new ShareMessageQueue();
					}
					else // compare and write only differences
					{
						lock (_internalQueue)
						{
							messageQueue.Enqueue(_internalQueue);
							_internalQueue.Clear();
						}
					}

					// write the updated message queue
					this.WriteMessageQueue(shareMemory, messageQueue);
				}
			}
			catch (System.Exception exp)
			{
				Trace.Write(exp);
			}
			finally
			{
				if (shareMemory != null)
				{
					if (bLocked)
						shareMemory.Unlock();
					shareMemory.Dispose();
					shareMemory = null;
				}				
			}
		}

		private ShareMessageQueue ReadMessageQueue(Segment shareMemory)
		{
			return (ShareMessageQueue)shareMemory.GetData();
		}

		private void WriteMessageQueue(Segment shareMemory, ShareMessageQueue queue)
		{
			shareMemory.SetData(queue);
		}

		#endregion

		#region override routines

		public override void Begin()
		{
			base.Begin ();

			// reset internal members
			this.Reset();	
			// update shared memory
			this.UpdateSharedData();
		}

		public override void StepTo(int value)
		{
			// NOTES: Do not invoke the base class
			// base.StepTo (value);
			
			// append new value to queue buffer
			this.EnqueueStatusCommand(value);

			// update shared memory
			this.UpdateSharedData();
		}

		public override void SetText(String text)
		{
			base.SetText (text);

			// append new value to queue buffer
			this.EnqueueStatusCommand(text);
			// update shared memory
			this.UpdateSharedData();
		}

		public override void SetStatusData(SharedData statusData)
		{
			// NOTES: Do not invoke the base class
			//base.SetStatusData (statusData);
			
			// append new value to queue buffer
			this.EnqueueStatusCommand(statusData);
			
			// update shared memory
			this.UpdateSharedData();
		}

		public override void SetUserData(object obj)
		{
			base.SetUserData (obj);

			if (obj != null && obj.GetType() == typeof(SharedData))
				this.SetStatusData((SharedData)obj);
		}


		#endregion
	}
}
