using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace SIA.UI.Controls
{
    /// <summary>
    /// The AppSettings class provides functions for loading/saving the application settings 
    /// </summary>
    public sealed class AppSettings
    {
        private const string keyCachedFolder = "CachedFolder";
        private const string keyHistoryFolder = "HistoryFolder";
        private const string keyCommonSettingsFolder = "CommonSettingsFolder";
        private const string keyMySettingsFolder = "MySettingsFolder";
        private const string keyTraceLogEnabled = "TraceLogEnabled";
        private const string keyTraceLogFolder = "TraceLogFolder";
        private const string keyConsoleLogEnabled = "TraceLogEnabled"; 
        private const string keyConsoleLogFolder = "ConsoleLogFolder";
        
        
        private static AppSettingsSection _appSettingsSection = null;

        /// <summary>
        /// The directory that serves as a common repository for application-specific
        /// data that is used by all users.
        /// </summary>
        public static string CommonApplicationData
        {
            get
            {
                string systemPath = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return string.Format(@"{0}\{1}\{2}\{3}", systemPath, Application.CompanyName,
                    Application.ProductName, Application.ProductVersion.ToString());
            }
        }

        /// <summary>
        /// The directory that serves as a common repository for application-specific
        /// data that is used by the current, non-roaming user.
        /// </summary>
        public static string LocalApplicationData
        {
            get
            {
                string systemPath = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return string.Format(@"{0}\{1}\{2}\{3}", systemPath, Application.CompanyName,
                    Application.ProductName, Application.ProductVersion.ToString());
            }
        }

        /// <summary>
        /// The directory that serves as a cached folder for cached items.
        /// </summary>
        public static string CachedFolder
        {
            get 
            {
                string folder = LocalApplicationData + @"\Cached";
                if (_appSettingsSection != null && _appSettingsSection.Settings[keyCachedFolder] != null)
                    folder = _appSettingsSection.Settings[keyCachedFolder].Value;

                Process currentProcess = Process.GetCurrentProcess();
                return String.Format(@"{0}\{1}", folder, currentProcess.Id);
            }
        }

        /// <summary>
        /// The directory where the history items is temporary saved
        /// </summary>
        public static string HistoryFolder
        {
            get
            {
                string folder = LocalApplicationData + @"\Histories";
                if (_appSettingsSection != null && _appSettingsSection.Settings[keyHistoryFolder] != null)
                    folder = _appSettingsSection.Settings[keyHistoryFolder].Value;

                Process currentProcess = Process.GetCurrentProcess();
                return String.Format(@"{0}\{1}", folder, currentProcess.Id);
            }
        }

        /// <summary>
        /// The directory that all local user settings was saved
        /// </summary>
        public static string MySettingsFolder
        {
            get
            {
                string folder = LocalApplicationData + @"\MySettings";
                if (_appSettingsSection != null && _appSettingsSection.Settings[keyMySettingsFolder] != null)
                    folder = _appSettingsSection.Settings[keyMySettingsFolder].Value;

                return folder;
            }
        }

        /// <summary>
        /// The directory that the share settings between users are saved
        /// </summary>
        public static string CommonSettingsFolder
        {
            get
            {
                string folder = CommonApplicationData + @"\CommonSettings";
                if (_appSettingsSection != null && _appSettingsSection.Settings[keyCommonSettingsFolder] != null)
                    folder = _appSettingsSection.Settings[keyCommonSettingsFolder].Value;

                return folder;
            }
        }

        /// <summary>
        /// Gets the boolean value indicates whether the trace log is enabled
        /// </summary>
        public static bool TraceLogEnabled
        {
            get
            {
                bool result = true;
                
                try
                {
                    if (_appSettingsSection != null && _appSettingsSection.Settings[keyTraceLogEnabled] != null)
                        result = Convert.ToBoolean(_appSettingsSection.Settings[keyTraceLogEnabled].Value);
                }
                catch (Exception exp)
                {
                    Trace.WriteLine(exp);
                }

                return result;
            }
        }

        /// <summary>
        /// The directory where the trace log files is saved
        /// </summary>
        public static string TraceLogFolder
        {
            get
            {
                string folder = LocalApplicationData + @"\TraceLog";
                if (_appSettingsSection != null && _appSettingsSection.Settings[keyTraceLogFolder] != null)
                    folder = _appSettingsSection.Settings[keyTraceLogFolder].Value;

                return folder;
            }
        }

        /// <summary>
        /// Gets the boolean value indicates whether the console log is enabled
        /// </summary>
        public static bool ConsoleLogEnabled
        {
            get
            {
                bool result = true;

                try
                {
                    if (_appSettingsSection != null && _appSettingsSection.Settings[keyConsoleLogEnabled] != null)
                        result = Convert.ToBoolean(_appSettingsSection.Settings[keyConsoleLogEnabled].Value);
                }
                catch (Exception exp)
                {
                    Trace.WriteLine(exp);
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the directory where the console log file is saved
        /// </summary>
        public static string ConsoleLogFolder
        {
            get
            {
                string folder = LocalApplicationData + @"\ConsoleLog";
                if (_appSettingsSection != null && _appSettingsSection.Settings[keyConsoleLogFolder] != null)
                    folder = _appSettingsSection.Settings[keyConsoleLogFolder].Value;

                return folder;
            }
        }

        static AppSettings()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            _appSettingsSection = config.GetSection("appSettings") as AppSettingsSection;
        }
    }
}
