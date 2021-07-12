using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Windows.Forms;

namespace SIA.UI.AppEntry
{
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
}
