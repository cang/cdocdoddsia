using System;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

namespace SIA.UI.Components
{
	public sealed class ResourceHelper
	{
		internal const String IconResource = "SIA.UI.Components.App.ico";
		
		private static Icon applicationIcon = null;

		static ResourceHelper()
		{
			applicationIcon = GetApplicationIcon();
		}

		public static Icon ApplicationIcon
		{
			get 
			{
				return applicationIcon;
			}
		}

		public static string ApplicationName
		{
			get
			{
				return Application.ProductName;
			}
		}

		public static Icon GetApplicationIcon()
		{
			try
			{
				string iconName = "App.ico";
				Assembly assembly = Assembly.GetEntryAssembly();
				string[] resNames = assembly.GetManifestResourceNames();
				foreach (string resName in resNames)
				{
					if (resName.IndexOf(iconName) >= 0)
					{
						using (System.IO.Stream stream = assembly.GetManifestResourceStream(resName))
							return new Icon(stream);
					}
				}

				return GetDefaultApplicationIcon();
			}
			catch (System.Exception e)
			{
				Trace.WriteLine(e);

				return GetDefaultApplicationIcon();
			}
		}

		public static Icon GetDefaultApplicationIcon()
		{
			// initialize dialog icons
			Type type = typeof(ResourceHelper);
			using (System.IO.Stream stream = type.Assembly.GetManifestResourceStream(IconResource))
				return new Icon(stream);
		}
	};
}
