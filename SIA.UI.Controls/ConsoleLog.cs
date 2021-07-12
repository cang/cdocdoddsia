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
    /// The ConsoleLog class provides static helper functions for redirecting the output
    /// to console log into a file.
    /// </summary>
    public class ConsoleLog
    {
        public string Version = "1.0";
        private static FormattedConsoleWriter _consoleWriter = null;
        private static bool _enabled = true;

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
            if (_consoleWriter != null)
                _consoleWriter.Enabled = _enabled;
        }

        public static void Initialize(string folder)
        {
            try
            {
                DateTime now = DateTime.Now;
                string fileName = String.Format("ConsoleLog_{0}_{1}.log", now.ToString("yyyyMMdd"), now.ToString("HHmmss"));
                fileName = String.Format(@"{0}\{1}", folder, fileName);

                // create directory if not exist
                string dir = Path.GetDirectoryName(fileName);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);


                string name = string.Format("{0}.ConsoleLog", Application.ProductName);
                _consoleWriter = new FormattedConsoleWriter(fileName, false);

                // initialize console out writer
                Console.SetOut(_consoleWriter);

                // write header to log file
                _consoleWriter.WriteHeader();
            }
            catch (System.Exception e)
            {
                Trace.WriteLine("Failed to initialize ConsoleLog:" + e);
            }

        }

        public static void Uninitialize()
        {
            try
            {
                // flush all text before shutdown listeners
                if (_consoleWriter != null)
                    _consoleWriter.Flush();
                _consoleWriter = null;
            }
            catch (System.Exception e)
            {
                Trace.WriteLine("Failed to uninitialize ConsoleLog" + e);
            }
        }
    }	
}
