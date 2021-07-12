using System;
using System.IO;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Resources;
using System.Configuration;
using System.Diagnostics;

namespace SIA.UI
{
	/// <summary>
	/// Summary description for RDEResources.
	/// </summary>
	public class SIAResources
	{
        static Image customShortcutIcon = null;

		static SIAResources()
		{
            customShortcutIcon = GetShortcutIcon("unknown");
		}

		public static Image GetMenuIcon(string name)
		{
			string resName = string.Format("SIA.UI.Resources.Images.Menu.{0}.png", name);
			return SIAResources.GetImage(resName);
		}

		public static Image GetShortcutIcon(string name)
		{
			string resName = string.Format("SIA.UI.Resources.Images.Shortcut.{0}.png", name);
			Image image = SIAResources.GetImage(resName);
            return image != null ? image : customShortcutIcon;
		}

		public static Image GetImage(string name)
		{	
			Type type = typeof(SIAResources);
			using (Stream stream = type.Assembly.GetManifestResourceStream(name))
			{
				if (stream == null)
					return null;
				return Image.FromStream(stream, true);
			}
		}

        public static Image CreateMenuIconFromShortcutIcon(Image image)
        {
            Image menuIcon = null;
            try
            {
                menuIcon = new Bitmap(16, 16, PixelFormat.Format32bppArgb);
                using (Graphics grph = Graphics.FromImage(menuIcon))
                {
                    Rectangle srcRect = new Rectangle(0, 0, image.Width, image.Height);
                    Rectangle dstRect = new Rectangle(0, 0, 16, 16);
                    grph.DrawImage(image, dstRect, srcRect, GraphicsUnit.Pixel);
                }
            }
            catch
            {
                if (menuIcon != null)
                {
                    menuIcon.Dispose();
                    menuIcon = null;
                }
            }

            return menuIcon;
        }
	}
}
