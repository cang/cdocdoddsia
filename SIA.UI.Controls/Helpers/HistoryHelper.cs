using System;
using System.Data;
using System.IO;
using System.Diagnostics;

using SIA.Common;
using SIA.UI.Controls;
using SIA.UI.Controls.Common;
using SIA.SystemLayer;

namespace SIA.UI.Controls.Helpers
{
    public interface IHistoryWorkspace
    {
        CommonImage Image { get; set;}
    }

	/// <summary>
	/// Summary description for HistoryHelper.
	/// </summary>
	public class HistoryHelper : IDisposable
	{
		private const int maxHistoryItemCount = 10;

		private IHistoryWorkspace _workspace = null;
		private HistoryCollection _histories = null;
		private int _currentHistoryIndex = 0;
		private object _syncObject = new object();
		private bool _canRaiseEvent = true;

		public event EventHandler HistoryReset = null;
		public event EventHandler HistoryChanged = null;
		public event EventHandler CurrentIndexChanged = null;

		#region public properties

        public IHistoryWorkspace Workspace
        {
            get { return _workspace; }
        }
        
		public Common.HistoryCollection Histories
		{
			get {return _histories;}
		}

		public bool IsHistoryEmpty
		{
			get {return _histories.Count == 0;}
		}

		public bool IsOnTopOfHistoryList
		{
			get {return _currentHistoryIndex == 0;}
		}

		public bool IsAtBottomOfHistoryList
		{
			get {return _currentHistoryIndex == _histories.Count-1;}
		}

		public int CurrentHistoryIndex
		{
			get {return _currentHistoryIndex;}
			set 
			{
				_currentHistoryIndex = value;
				OnCurrentHistoryIndexChanged();
			}
		}

		protected virtual void OnCurrentHistoryIndexChanged()
		{
		}

		#endregion

		#region constructor and destructor
		
		public HistoryHelper(IHistoryWorkspace owner)
		{
			// initialize owner
			_workspace = owner;

			this.InitializeComponents();
		}

		private void InitializeComponents()
		{
			this._histories = new HistoryCollection();
		}

		~HistoryHelper()
		{
			this.Dispose(true);
		}

		#region IDisposable Members

		public void Dispose()
		{
			this.Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
            if (_histories != null)
                _histories.Dispose();
			_histories = null;
		}

		#endregion

		#endregion

		#region public operation

		/// <summary>
		/// Reset history
		/// </summary>
		public void EmptyHistoryList()
		{
			try
			{
				this.LockRaiseEvent();
				
				// clear history and dispose contained objects
				while (_histories.Count > 0)
					RemoveOutOfHistory(0, true);
				
				_currentHistoryIndex = -1;
			}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);
			}
			finally
			{
				this.UnlockRaiseEvent();

				// raise reset event
				RaiseHistoryReset();
			}
		}

		public void Undo()
		{
			if (IsHistoryEmpty == true)
				return;
			if (_currentHistoryIndex<0 || _currentHistoryIndex >= _histories.Count)
				return;

			try
			{
				this.LockRaiseEvent();

				if (_currentHistoryIndex - 1 >= 0)
				{
					int index = _currentHistoryIndex - 1;
					Common.History history = _histories[index];
					CommonImage image = history.Load();
					
                    if (image != null)
					    this.UpdateWorkspaceImage(image);

					this.UnlockRaiseEvent();

					_currentHistoryIndex = index;					
					RaiseHistoryCurrentIndexChanged();
				}
			}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);
			}
			finally
			{
				this.UnlockRaiseEvent();
			}
		}

		public void Redo()
		{
			if (IsHistoryEmpty == true)
				return;
			if (_currentHistoryIndex<0 && _currentHistoryIndex >= _histories.Count)
				return;

			try
			{
				this.LockRaiseEvent();

				if (_currentHistoryIndex + 1 < _histories.Count)
				{
					int index = _currentHistoryIndex + 1;
					Common.History history = _histories[index];
					CommonImage image = history.Load();
                    
                    if (image != null)
                        this.UpdateWorkspaceImage(image);

					this.UnlockRaiseEvent();

					_currentHistoryIndex = index;					
					RaiseHistoryCurrentIndexChanged();
				}
			}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);
			}
			finally
			{
				this.UnlockRaiseEvent();
			}
		}

		public void RestoreFromHistory(int historyIndex)
		{
			if (historyIndex<0 || historyIndex>=_histories.Count)
				return;

			try
			{
				_currentHistoryIndex = historyIndex;
				Common.History history = _histories[_currentHistoryIndex];
				CommonImage image = history.Load();
                
                if (image != null)
                    this.UpdateWorkspaceImage(image);
			}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);
			}
			finally
			{
			}
		}

		public void AddToHistory(SIA.SystemLayer.CommonImage image)
		{
			// first check for savable file size
			const long maxSupportFileSize = 50 * 1024 * 1024;
			long sizeInBytes = image.RasterImage.SizeInBytes;
			if (sizeInBytes < maxSupportFileSize) // if file size is small enough
			{
				try
				{
					// lock raise event 
					this.LockRaiseEvent();

					// check for number of item in history
					// if current history index is not at the end of history index
					// then we have to clean up item which is located from 
					// current history index to end of history list
					while (_histories.Count > _currentHistoryIndex+1)
						RemoveOutOfHistory(_currentHistoryIndex+1, true);

					// if number of items in history is greater or equal to maxHistoryItemCount 
					// then we have to clean up old items default is first item (FIFO Buffer)
					while (_histories.Count >= maxHistoryItemCount)
						RemoveOutOfHistory(0, true);					

                    // serialize the 
					
					// append new history to the end of the history list
					_histories.Add(image);

					// update current history index
					_currentHistoryIndex = _histories.Count-1;

					// release raise event lock
					this.UnlockRaiseEvent();

					// raise history change event
					this.RaiseHistoryChange();

					// raise history current index change event
					this.RaiseHistoryCurrentIndexChanged();
				}
				catch (System.Exception exp)
				{
					Trace.WriteLine(exp);

					// release raise event lock
					this.UnlockRaiseEvent();
				}
				finally
				{
				}
			}
		}

		private void RemoveOutOfHistory(int index, bool bDispose)
		{
			if (index >=0 && index < _histories.Count)
				_histories.RemoveAt(index);				
		}

		#endregion

        #region Override routines

        protected virtual void UpdateWorkspaceImage(CommonImage image)
        {
            CommonImage currentImage = _workspace.Image;
            _workspace.Image = image;            
            if (currentImage != null)
                currentImage.Dispose();
        }

        #endregion

        #region internal helpers

        #region System Information
        /// <summary>
		/// Validate memory usage
		/// </summary>
		private void CheckAvailableMemory()
		{
			if (this._histories.Count < 0)
				return;

			// if current used memory is too much then we have to free some 
			// to make sure there is enough memory for next operations
			// In this current version, we are using history with size is 
			// 10% of working set (physical memory mapped) memory
			
			long PhysicalMemSize = GetPhysicalMemorySize();
			long CurrentSize = GetHistoryPhysicalMemorySize();
			
		}

		/// <summary>
		/// Retrieve current history size in physical memory
		/// </summary>
		/// <returns></returns>
		private long GetHistoryPhysicalMemorySize()
		{
			long size = 0;
			foreach(SIA.SystemLayer.CommonImage image in _histories)
				size += System.Runtime.InteropServices.Marshal.SizeOf(image);
			return size;
		}

		private long GetPhysicalMemorySize()
		{
			// Amount of physical memory mapped to the process context
			Process process = Process.GetCurrentProcess();
			return (long)process.WorkingSet64;			
		}

		#endregion

		#region Event helpers

		private bool CanRaiseEvent
		{
			get 
			{
				lock (_syncObject)
					return _canRaiseEvent;
			}
		}

		public void LockRaiseEvent()
		{
			lock (_syncObject)
				_canRaiseEvent = false;
		}

		public void UnlockRaiseEvent()
		{
			lock (_syncObject)
				_canRaiseEvent = true;
		}

		private void RaiseHistoryChange()
		{
			if (!CanRaiseEvent) return ;
			
			if (this.HistoryChanged != null)
				this.HistoryChanged(this, EventArgs.Empty);
		}

		private void RaiseHistoryReset()
		{
			if (!CanRaiseEvent) return ;
			
			if (this.HistoryReset != null)
				this.HistoryReset(this, EventArgs.Empty);
		}

		private void RaiseHistoryCurrentIndexChanged()
		{
			if (!CanRaiseEvent) return;

			if (this.CurrentIndexChanged != null)
				this.CurrentIndexChanged(this, EventArgs.Empty);
		}

		#endregion

		#endregion

		
	}
}
