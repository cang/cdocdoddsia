using System;
using System.IO;
using System.Reflection;
using System.Data;
using System.Collections;
using System.Diagnostics;

using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;

using SIA.SystemFrameworks.UI;
using SIA.SystemLayer;

namespace SIA.Plugins.Common
{
	/// <summary>
	/// The IDocWorkspace provides document workspace declaration.
    /// The document workspace object actually is a wrapper of an image object.
	/// </summary>
	public interface IDocWorkspace 
        : IProgressHandler, IWin32Window
	{	
		double MINGRAYSCALE {get;}
		double MAXGRAYSCALE {get;}
		double MinCurrentView {get;}
		double MaxCurrentView {get;}

        /// <summary>
        /// The image contained by the document workspace
        /// </summary>
		CommonImage Image {get;}
	};
}
