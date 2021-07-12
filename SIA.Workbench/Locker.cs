using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using SIA.SystemFrameworks;

namespace SIA.Workbench.Utilities
{
	/// <summary>
	/// Semaphore lock for limit the number of running instances.
	/// </summary>
	internal class RDEWLock
	{
		private const int _threadLimit = 1;
		private const string _lockName = "Global\\SiGlaz.RDEW";
        private static SIA.SystemFrameworks.Semaphore _processLocker = null;
	
		public static bool Begin()
		{
            _processLocker = new SIA.SystemFrameworks.Semaphore(_threadLimit, _lockName);
			int timeOut = _processLocker.Wait(100);
			if (timeOut == WaitHandle.WaitTimeout)
			{
				//string msg = string.Format("RDEW only permits user to run {0} applications in one computer at the same time, please check !!!", _threadLimit);
                string msg = string.Format("SIAW only permits user to run {0} applications in one computer at the same time, please check !!!", _threadLimit);
				MessageBox.Show(null, msg, "SiGlaz Image Analyzer Workbench");
				return false;
			}
	
			return true;
		}
	
		public static void End()
		{
			if (_processLocker != null)
				_processLocker.Release();
			_processLocker = null;
		}
	};

	internal class FileProcessorLocker
	{
		private const int _threadLimit = 1;
		private const string _lockName = "Global\\SiGlaz.FileProcessor";

		// lock between processes
        private static SIA.SystemFrameworks.Semaphore _processLocker = null;

		// lock between threads
		private static AutoResetEvent _threadLocker = null;

		public static void Begin()
		{
			// set thread lock
			if (_threadLocker == null)
				_threadLocker = new AutoResetEvent(true);
			_threadLocker.WaitOne();
			_threadLocker.Reset();
			
			if (_processLocker == null)
                _processLocker = new SIA.SystemFrameworks.Semaphore(_threadLimit, _lockName);
			_processLocker.Wait();
		}
	
		public static void End()
		{	
			// release thread lock
			if (_threadLocker != null)
				_threadLocker.Set();

			if (_processLocker != null)
				_processLocker.Release();
			_processLocker = null;
		}
	};
}