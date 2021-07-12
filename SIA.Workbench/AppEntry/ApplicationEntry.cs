#define TRACE_LOG
#define SCRIPT_EXCUTOR

using System;
using System.IO;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using System.Globalization;

using SIA.Common;
using SIA.Workbench;
using SIA.Workbench.Utilities;
using SIA.UI.Controls;

namespace SIA.Workbench.AppEntry
{
    /// <summary>
    /// Summary description for ApplicationEntry.
    /// </summary>
    internal class ApplicationEntry
    {

        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                Application.SetCompatibleTextRenderingDefault(true);

                // disable cross thread call
                Control.CheckForIllegalCrossThreadCalls = false;

                // initialize application culture
                InitializeApplicationCulture();

                // initialize application domain helper
                AppDomainHelper.Initialize();

#if MULTIPLE_INSTANCES
				// check for running instances
				if (RDEWLock.Begin() == false)
					return;
#endif

                // enable Windows Themes
                Application.EnableVisualStyles();
                // fix bug of toolbar's icons cannot be displayed correctly
                Application.DoEvents();

#if TRACE_LOG
                ApplicationEntry.InitializeTraceLog();
#endif
                // initialize Thread Exception Handler
                // AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
                Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
                Application.ThreadExit += new EventHandler(Application_ThreadExit);

                // initialize script factory
                MainForm.InitializeScriptFactory();

                // start application
                //if (args.Length > 0 && args[0] == "/newui")
                //{
                using (MainForm3 mainForm = new MainForm3())
                    Application.Run(mainForm);
                //}
                //else
                //{
                //	using (MainForm mainForm = new MainForm())
                //		Application.Run(mainForm);
                //}
            }
            finally
            {
                // uninitialize script factory
                MainForm.UninitializeScriptFactory();

#if MULTIPLE_INSTANCES
				// release application locker
				RDEWLock.End();
#endif

                // uninitialize application domain helper
                AppDomainHelper.Uninitialize();
            }
        }

        private static void InitializeApplicationCulture()
        {
            CultureInfo cultureInfo = new CultureInfo("en-US");
            Application.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Trace.WriteLine(e);
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            try
            {
                Trace.WriteLine(e.Exception.ToString());

                Exception exp = e.Exception;
                //string errorMsg = string.Format(new CultureInfo("en-us", true), 
                //    "There are some problems while trying to use the SiGlaz Image Analyzer, " + 
                //    "please check the following error messages: \n{0}\n", exp != null ? exp.Message : e.ToString());
                string errorMsg = string.Format(new CultureInfo("en-us", true),
                    "There are some problems while trying to use the SiGlaz Image Analyzer, " +
                    "please check the following error messages: \n{0}\n", (exp != null ? exp.Message : e.ToString()));

                errorMsg += string.Format("Stack Trace:\n{1}\n", (exp != null ? exp.StackTrace : e.ToString()));

                errorMsg += Environment.NewLine;
                errorMsg += "Do you want to exit the application?";

                DialogResult result = MessageBox.Show(errorMsg, "Application Error", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);

                // Exits the program when the user clicks Abort.
                if (result == DialogResult.Yes)
                    Application.Exit();
            }
            catch
            {
            }
        }

        private static void Application_ThreadExit(object sender, EventArgs e)
        {

        }


#if TRACE_LOG
        public static void InitializeTraceLog()
        {
            if (AppSettings.TraceLogEnabled)
                TraceLog.Initialize(AppSettings.TraceLogFolder);
            if (AppSettings.ConsoleLogEnabled)
                ConsoleLog.Initialize(AppSettings.ConsoleLogFolder);
        }

        public static void UninitializeTraceLog()
        {
            TraceLog.Uninitialize();
            ConsoleLog.Uninitialize();
        }
#endif


    }
}
