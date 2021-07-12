using System;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Diagnostics;

namespace SIA.Workbench.AppEntry
{
    /// <summary>
    /// The AppDomainHelper class for internal use only.
    /// </summary>
    internal class AppDomainHelper
    {
        private static Hashtable _hashTable = new Hashtable();

        /// <summary>
        /// Registers for the assemblies events 
        /// </summary>
        public static void Initialize()
        {
            AppDomain domain = AppDomain.CurrentDomain;
            domain.AssemblyLoad += new AssemblyLoadEventHandler(CurrentDomain_AssemblyLoad);
            domain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
        }

        /// <summary>
        /// Unregisters for the assemblies events
        /// </summary>
        public static void Uninitialize()
        {
            AppDomain domain = AppDomain.CurrentDomain;
            domain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve);
        }

        private static void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            // add loaded assembly into hashtable
            _hashTable[args.LoadedAssembly.FullName] = args.LoadedAssembly;

#if TRACE_ASSEMBLIES
			// output to trace log
			Trace.WriteLine(string.Format("The assembly {0} was loaded successfully.", args.LoadedAssembly.FullName));
#endif
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (_hashTable[args.Name] != null) // if the assemblies was loaded
            {
                return _hashTable[args.Name] as Assembly;
            }
            else // scans for the loaded assemblies in the current application domain
            {
                AssemblyName searchAssemblyName = GetAssemblyName(args.Name);
                AppDomain domain = AppDomain.CurrentDomain;
                Assembly[] assemblies = domain.GetAssemblies();
                foreach (Assembly assembly in assemblies)
                {
                    AssemblyName assemblyName = assembly.GetName();
                    //if (assembly.FullName == args.Name)
                    if (true == IsAssemblyEqual(searchAssemblyName, assemblyName))
                    {
                        // add loaded assembly into hashtable
                        _hashTable[args.Name] = assembly;

#if TRACE_ASSEMBLIES
						// output to trace log
						Trace.WriteLine(string.Format("The assembly {0} was manually resolved successfully.", args.Name));
#endif
                        // return the resolved assembly
                        return assembly;
                    }
                }
            }

#if TRACE_ASSEMBLIES
			// output to trace log
			Trace.WriteLine(string.Format("The assembly {0} has been not loaded yet.", args.Name));
#endif
            return null;
        }

        public static bool IsAssemblyEqual(AssemblyName assembly1, AssemblyName assembly2)
        {
            return (assembly1.Name == assembly2.Name) && (assembly1.Version.CompareTo(assembly2.Version) == 0);
        }

        public static AssemblyName GetAssemblyName(string fullName)
        {
            const string keyVersion = "Version";

            char[] separator = new char[] { ',' };
            string[] items = fullName.Split(separator);
            if (items.Length <= 0)
                throw new ArgumentException("Input string was not an assembly name");

            AssemblyName assemblyName = new AssemblyName();
            // retrieve assembly name
            assemblyName.Name = items[0];
            // retrieve assembly version
            if (items.Length > 1)
            {
                string str = items[1];
                if (str.IndexOf(keyVersion) >= 0)
                {
                    string version = str.Replace(keyVersion + "=", "");
                    version = version.Trim();
                    assemblyName.Version = new Version(version);
                }
            }

            return assemblyName;
        }

    }
}
