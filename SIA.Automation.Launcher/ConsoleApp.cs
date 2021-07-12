#define GPU_SUPPORTED

using System;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Globalization;

using SIA.SystemFrameworks.UI;

using SIA.UI.Controls;
using SIA.UI.Controls.Automation;
using SIA.UI.Controls.Utilities;


namespace SIA.Automation.Launcher
{
	/// <summary>
	/// Summary description for ConsoleApp.
	/// </summary>
	public class ConsoleApp
	{
		#region Member Fields
		
		static ScriptLauncher _scriptLauncher = null;

		#endregion

		#region Properties

		public static ScriptLauncher ScriptLauncher
		{
			get
			{
				if (_scriptLauncher == null)
					_scriptLauncher = new ScriptLauncher();
				return _scriptLauncher;
			}
		}

		#endregion
	
		#region Application Entry

		[MTAThread]
		public static int Main(string[] args)
		{
			int exitCode = 0;

            bool bException = false;
            try
            {
                // initialize application culture
                InitializeApplicationCulture();
#if DEBUG
                // raise debugger if Control and Shift Keys were pressed
                int modifier = (int)Form.ModifierKeys;
                int ctrlModifier = (int)Keys.Control;
                int shiftModifier = (int)Keys.Shift;
                bool isCtrlDown = (modifier & ctrlModifier) == ctrlModifier;
                bool isShiftDown = (modifier & shiftModifier) == shiftModifier;
                if (isCtrlDown && isShiftDown)
                {
                    if (MessageBoxEx.ConfirmYesNo("Do you really want to debug the script?"))
                    {
                        try
                        {
                            Debugger.Launch();
                        }
                        catch (System.Exception e)
                        {
                            MessageBoxEx.Error("Failed to attach debugger: " + e.Message);
                        }
                    }
                }
#endif                
                // wait for another instance to finish processing
                //RDEConsoleLock.Begin();

                // install domain exception handler
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

                // install console break handler
                InstallConsoleBreakHandler();

                // try to load unexpected module
                ModuleLoader.LoadModules();

                // initialize script factory
                ScriptFactory.Initialize();

                //Init GPU Device Engine
#if GPU_SUPPORTED
                SiGlaz.Cloo.DeviceProgram.InitCL();
#endif

                // initialize script launcher
                ScriptLauncher launcher = ConsoleApp.ScriptLauncher;

                // execute script launcher
                exitCode = (int)launcher.Run(args);
            }
            catch (System.Threading.ThreadAbortException abort_exp)
            {
                // nothing
                bException = true;
            }
            catch (System.Exception exp)
            {
                Trace.WriteLine(exp);

                exitCode = (int)ErrorCodes.UnhandledException;

                bException = true;
            }
			finally
			{
                if (!bException)
                {
                    Thread.Sleep(2000);
                }
			}

			return exitCode;
		}
		#endregion

		#region Methods

		private static void InitializeApplicationCulture()
		{
			CultureInfo cultureInfo = new CultureInfo("en-US");
			Application.CurrentCulture = cultureInfo;
			Thread.CurrentThread.CurrentCulture = cultureInfo;
			Thread.CurrentThread.CurrentUICulture = cultureInfo;
		}

		private static bool ConsoleCtrlCheck(ControlTypes ctrlType)
		{
			ScriptLauncher launcher = ScriptLauncher;
			if (launcher == null)
				return true;

			if (launcher.IsProcessing)
				launcher.IsCancelled = true;
			
			if (!launcher.WaitForExit())
			{
				launcher.Terminate();	
			}
			
			return true;
		}

		private static void InstallConsoleBreakHandler()
		{
			ConsoleApp.SetConsoleCtrlHandler(new HandlerRoutine(ConsoleCtrlCheck), true);
		}

		#region Internal Helpers

		// Declare the SetConsoleCtrlHandler function
		// as external and receiving a delegate.
		[DllImport("Kernel32")]
		public static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler, bool Add);

		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Trace.WriteLine("Unhandled exception:" + e.ExceptionObject.ToString());
		}

		#endregion
	
		#endregion
	}

    internal class ModuleLoader
    {
        internal static void LoadModules()
        {
            try
            {
                #region Computer Vision Modules
                /* *
                 * I don't why the first loading is always failed!
                 * So, this is a tweak to prepare for loading successfully in product.
                 * */
                //Assembly assembly = null;
                //assembly = Assembly.LoadFile(
                //    Path.Combine(Application.StartupPath, "SIA.SystemFrameworks.ComputerVision.dll"));
                LoadModules(new string[] { 
                    "SIA.Common.dll",
                    "SIA.SystemFrameworks.dll",
                    "SIA.SystemFrameworks.ComputerVision.dll"
                });
                LoadModules(new string[] { 
                    "SIA.Common.dll",
                    "SIA.SystemFrameworks.dll",
                    "SIA.SystemFrameworks.ComputerVision.dll"
                });
                #endregion Computer Vision Modules
            }
            catch
            {
                // nothing
            }
            finally
            {
            }
        }

        internal static void LoadModules(string[] assemblies)
        {
            if (assemblies == null || assemblies.Length == 0)
                return;

            for (int i = 0; i < assemblies.Length; i++)
            {
                try
                {
                    /* *
                     * I don't know why the first loading is always failed!
                     * So, this is a tweak to prepare for loading successfully in product.
                     * */
                    Assembly assembly = null;

                    assembly = Assembly.LoadFile(
                        Path.Combine(Application.StartupPath, assemblies[i]));
                }
                catch (System.Exception exp)
                {
                    System.Diagnostics.Trace.WriteLine(exp.Message);
                    // nothing
                }
                finally
                {
                }
            }
        }
    }
	
	// An enumerated type for the control messages
	// sent to the handler routine.
	public enum ControlTypes
	{
		CTRL_C_EVENT = 0,
		CTRL_BREAK_EVENT,
		CTRL_CLOSE_EVENT,
		CTRL_LOGOFF_EVENT = 5,
		CTRL_SHUTDOWN_EVENT
	}

	// A delegate type to be used as the handler routine
	// for SetConsoleCtrlHandler.
	public delegate bool HandlerRoutine(ControlTypes CtrlType);
}
