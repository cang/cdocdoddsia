#define GPU_SUPPORTED

#define ENABLE_VISUAL_STYLES
#define ENABLE_FLASH
#define TRACE_LOG

using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using System.Threading;

using SIA.UI;
using SIA.UI.Helpers;
using SIA.UI.Controls;

using SIA.Common;
using SiGlaz.Algorithms.Core;


namespace SIA.UI.AppEntry
{
    /// <summary>
    /// Summary description for ApplicationEntry.
    /// </summary>
    public class ApplicationEntry
    {

#if TRACE_LOG

        public static void InitializeLoggers()
        {
            if (AppSettings.TraceLogEnabled)
                TraceLog.Initialize(AppSettings.TraceLogFolder);

            if (AppSettings.ConsoleLogEnabled)
                ConsoleLog.Initialize(AppSettings.ConsoleLogFolder);
        }

        public static void UninitializeLoggers()
        {
            if (AppSettings.TraceLogEnabled)
                TraceLog.Uninitialize();

            if (AppSettings.ConsoleLogEnabled)
                ConsoleLog.Uninitialize();
        }

#endif


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                // enable compatible text rendering
                Application.SetCompatibleTextRenderingDefault(true);

                // disable cross thread call
                Control.CheckForIllegalCrossThreadCalls = false;

                // initialize application culture 
                InitializeApplicationCulture();

#if SINGLE_INSTANCE
				if (RDELock.Begin() == false)
					return ;
#endif

#if ENABLE_VISUAL_STYLES
                // Enable Windows Style (Windows XP or later)
                System.Windows.Forms.Application.EnableVisualStyles();
#endif

#if TRACE_LOG
                InitializeLoggers();
#endif

                //Init Open CL device
#if GPU_SUPPORTED
                SiGlaz.Cloo.DeviceProgram.InitCL();
#endif

                // initialize application domain helper
                AppDomainHelper.Initialize();

                // register for domain exception handling
                AppDomain currentDomain = AppDomain.CurrentDomain;
                currentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

                // initialize Thread Exception Handler
                Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
                Application.ThreadExit += new EventHandler(Application_ThreadExit);


                // initialize flash screen
#if ENABLE_FLASH
                using (DlgFlashScreen flashScreen = new DlgFlashScreen())
                {
                    flashScreen.TopMost = true;
                    flashScreen.Show();
#endif
                    // load modules
                    ModuleLoader.LoadModules();

                    // initialize application
                    using (MainFrame appWorkspace = new MainFrame(args))
                    {
                        appWorkspace.InitEngineWorkspace();
                        Application.Run(appWorkspace);
                    }

#if ENABLE_FLASH
                }
#endif

            }
            catch (Exception exp)
            {
                Application_ThreadException(null, new ThreadExceptionEventArgs(exp));
            }
            finally
            {
#if TRACE_LOG
                UninitializeLoggers();
#endif

                // uninitialize application domain helper
                AppDomainHelper.Uninitialize();

#if !(DEBUG)
				RDELock.End();
#endif
            }
        }

        private static void InitializeApplicationCulture()
        {
            CultureInfo cultureInfo = new CultureInfo("en-US");
            Application.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            try
            {
                // output to trace log
                Trace.WriteLine(e.Exception);

                Exception exp = e.Exception;
                string errorMsg = string.Format(
                    new CultureInfo("en-us", true),
                    "There are some problems while trying to use the SiGlaz Image Analyzer, please check the following error messages: \n{0}\n",
                    (exp != null ? e.Exception.Message : e.ToString()));

                errorMsg += string.Format("Stack Trace:\n{1}\n", (exp != null ? exp.StackTrace : e.ToString()));

                errorMsg += Environment.NewLine;
                errorMsg += "Do you want to exit the application?";

                // Exits the program when the user clicks Abort.
                if (true == MessageBoxEx.ConfirmYesNo(errorMsg))
                    Application.Exit();
            }
            catch
            {
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                // output to trace log
                Exception exp = e.ExceptionObject as Exception;
                Trace.WriteLine(exp.ToString());

                string errorMsg = string.Format(
                    new CultureInfo("en-us", true),
                    "There are some problems while trying to use the SiGlaz Image Analyzer, please check the following error messages: \n{0}\n",
                    (exp != null ? exp.Message : e.ToString()));

                errorMsg += string.Format(
                    "Stack Trace:\n{1}\n", (exp != null ? exp.StackTrace : e.ToString()));

                errorMsg += Environment.NewLine;
                errorMsg += "Do you want to exit the application?";

                // Exits the program when the user clicks Abort.
                if (true == MessageBoxEx.ConfirmYesNo(errorMsg))
                    Application.Exit();
            }
            catch
            {
            }
        }

        private static void Application_ThreadExit(object sender, EventArgs e)
        {
            Trace.WriteLine("Thread Exit on " + DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString());
        }


    }
}
