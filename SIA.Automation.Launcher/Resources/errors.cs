//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.1.4322.573
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

using System;
using System.Resources;
using System.Reflection;

// -----------------------------------------------------------------------------
//  <autogeneratedinfo>
//      This code was generated by:
//        SR Resource Generator custom tool for VS.NET, by Martin Granell, Readify
// 
//      It contains classes defined from the contents of the resource file:
//        D:\Projects\prjSiGlaz\Working\BEDE\Phrase II\Online\SIA.Automation.Launcher\Resources\errors.resx
// 
//      Generated: Monday, February 26, 2007 8:21 AM
//  </autogeneratedinfo>
// -----------------------------------------------------------------------------
namespace SIA.Automation.Launcher.Resources
{
    
    
    internal class errors
    {
        
        internal static System.Globalization.CultureInfo Culture
        {
            get
            {
                return Keys.Culture;
            }
            set
            {
                Keys.Culture = value;
            }
        }
        
        internal class Keys
        {
            
            static System.Resources.ResourceManager resourceManager = new System.Resources.ResourceManager(typeof(errors));
            
            static System.Globalization.CultureInfo _culture = null;
            
            internal static System.Globalization.CultureInfo Culture
            {
                get
                {
                    return _culture;
                }
                set
                {
                    _culture = value;
                }
            }
            
            internal static string GetString(string key)
            {
                return resourceManager.GetString(key, _culture);
            }
            
            internal static string GetString(string key, object[] args)
            {
                string msg = resourceManager.GetString(key, _culture);
                msg = string.Format(msg, args);
                return msg;
            }
        }
    }
}
