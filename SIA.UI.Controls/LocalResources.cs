using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.IO;

namespace SIA.UI.Controls
{
    /// <summary>
    /// Internal class provides access to the assembly resource
    /// </summary>
    internal class LocalResources
    {
        static string resourceNamespace = null;

        static LocalResources()
        {
            Type type = typeof(LocalResources);
            resourceNamespace = string.Format("{0}.Resources", type.Namespace);
        }

        public static Cursor GetCursor(string name)
        {
            string resName = String.Format("{0}.Cursors.{1}", resourceNamespace, name);
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(resName))
                return new Cursor(stream);
        }

        public class Cursors
        {
            public static readonly Cursor ZoomIn = null;
            public static readonly Cursor ZoomOut = null;
            public static readonly Cursor Hand = null;
            public static readonly Cursor DrawWaferBound = null;
            public static readonly Cursor DrawCircle = null;
            public static readonly Cursor SelectNotch = null;
            public static readonly Cursor ClusterInfo = null;
            public static readonly Cursor Resize = null;

            static Cursors()
            {
                ZoomIn = LocalResources.GetCursor("zoomin.cur");
                ZoomOut = LocalResources.GetCursor("zoomout.cur");
                Hand = LocalResources.GetCursor("Hand.cur");
                DrawWaferBound = LocalResources.GetCursor("DrawWaferBound.cur");
                DrawCircle = LocalResources.GetCursor("drawcircle.cur");
                SelectNotch = LocalResources.GetCursor("SelectNotch.cur");
                ClusterInfo = LocalResources.GetCursor("ClusterInfo.cur");
                Resize = LocalResources.GetCursor("resize.cur");
            }
        };
    }
}
