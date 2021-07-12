using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace SIA.Workbench
{
	/// <summary>
	/// Summary description for Resources.
	/// </summary>
	internal class Resource
	{
		public static readonly string localNamespace = string.Empty;
		
		public static readonly Image CustomProcessNodeIcon = null;
		public static readonly Image SettingsImage = null;
		public static readonly Image NoSettingsImage = null;
		public static readonly Image ProcessImage = null;

		static Resource()
		{
			Type type = typeof(Resource);
			localNamespace = "SIA.Workbench.Resources";//String.Format("{0}.Resources", type.Namespace); 

			CustomProcessNodeIcon = GetProcessStepIcon("Custom");
			SettingsImage = GetImage("settings.gif"); 
			NoSettingsImage = GetImage("nosettings.gif");
			ProcessImage = GetImage("process.gif");
		}

		public static Image GetProcessStepIcon(string name)
		{
			string resName = string.Format("{0}.ProcessSteps.{1}.png", localNamespace, name);
			return GetImageResource(resName);
		}

		public static Image GetImage(string fileName)
		{
			string resName = string.Format("{0}.Images.{1}", localNamespace, fileName);
			return GetImageResource(resName);
		}

		public static Image GetImageResource(string resourseName)
		{	
			Type type = typeof(Resource);
			using (Stream stream = type.Assembly.GetManifestResourceStream(resourseName))
			{	
				if (stream == null)
					return null;
				return Image.FromStream(stream, true);
			}
		}

        public static Cursor GetCursor(string name)
        {
            string resName = string.Format("{0}.Cursors.{1}.cur", localNamespace, name);
            Type type = typeof(Resource);
            using (Stream stream = type.Assembly.GetManifestResourceStream(resName))
            {
                if (stream == null)
                    return null;
                return new Cursor(stream);
            }
        }
	}
}
