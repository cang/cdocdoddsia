using System;
using System.Collections.Generic;
using System.Text;
using SIA.Common;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace SIA.UI.Controls
{
    /// <summary>
    /// The TraceLog class provides static helper functions for redirecting the output
    /// to trace log into a file.
    /// </summary>
    public class TraceLog
    {
        public string Version = "1.0";
        private static string _fileName = string.Empty;
        private static FormattedTextWriterTraceListener _listener = null;
        private static bool _enabled = false;

        public static string FileName
        {
            get { return _fileName; }
        }

        public static bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                OnEnabledChanged();
            }
        }

        private static void OnEnabledChanged()
        {
            if (_listener != null)
                _listener.Enabled = _enabled;
        }

        public static void Initialize(string folder)
        {
            try
            {
                DateTime now = DateTime.Now;
                string datetime = now.ToString("yyyyMMdd_HHmmss");
                string fileName = String.Format("TraceLog_{0}.log", datetime);
                
                fileName = String.Format(@"{0}\{1}", folder, fileName);
                
                // create directory if not exist
                string dir = Path.GetDirectoryName(fileName);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                string name = string.Format("{0}.TraceLog", Application.ProductName);
                _listener = new FormattedTextWriterTraceListener(fileName, name);
                
                // auto flush
                Trace.AutoFlush = true;
                
                // insert into listeners list
                Trace.Listeners.Add(_listener);
                
                // write
                Trace.WriteLine("Trace Log initialized.");

                _fileName = fileName;
            }
            catch (System.Exception e)
            {
                Trace.WriteLine("Failed to initialize TraceLog:" + e);
            }

        }

        public static void Uninitialize()
        {
            try
            {
                // flush all text before shutdown listeners
                Trace.Flush();

                if (_listener != null)
                    Trace.Listeners.Remove(_listener);
                _listener = null;
            }
            catch (System.Exception e)
            {
                Trace.WriteLine("Failed to uninitialize TraceLog" + e);
            }
        }
    }
}
