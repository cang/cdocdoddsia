using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using SIA.SystemFrameworks;

namespace SIA.UI.Helpers
{
	/// <summary>
	/// Semaphore lock for limit the number of running instances.
	/// </summary>
	public class RDELock
	{
		private const int _threadLimit = 1;
		private const string _lockName = "Global\\SiGlaz.RDE";
        private static SIA.SystemFrameworks.Semaphore _locker = null;
	
		public static bool Begin()
		{
			_locker = new SIA.SystemFrameworks.Semaphore(_threadLimit, _lockName);
			int timeOut = _locker.Wait(100);
			if (timeOut == WaitHandle.WaitTimeout)
			{
				string msg = string.Format("SIA only permits user to run {0} applications in one computer at the same time, please check !!!", _threadLimit);
				MessageBox.Show(null, msg, "SiGlaz Image Analyzer");
				return false;
			}
	
			return true;
		}
	
		public static void End()
		{
			if (_locker != null)
				_locker.Release();
			_locker = null;
		}
	};
}