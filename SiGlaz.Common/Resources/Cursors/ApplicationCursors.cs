using System;
using System.Collections;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;
using System.Diagnostics;

using SiGlaz.Common;

namespace SiGlaz.Common.Resources
{
	/// <summary>
	/// Summary description for ApplicationCursors.
	/// </summary>
	internal class ApplicationCursors : IDisposable
	{
		#region member attributes
		private static ApplicationCursors _instance = null;

		private System.Windows.Forms.Cursor[] _cursors = null;

		#endregion

		#region properties
		private static ApplicationCursors Instance
		{
			get
			{
				if (_instance == null)
					_instance = new ApplicationCursors();
				return _instance;
			}
		}

		public static Cursor[] Cursors
		{
			get
			{
				return ApplicationCursors.Instance._cursors;
			}
		}

		public static Cursor DrawRectangle
		{
			get
			{
				if (ApplicationCursors.Cursors == null)
					return null;
				int index = (int)DrawToolType.Rectangle;
				if (index < 0 || index > ApplicationCursors.Cursors.Length)
					return null;
				return ApplicationCursors.Cursors[index];
			}
		}

		public static Cursor DrawEllipse
		{
			get
			{
				if (ApplicationCursors.Cursors == null)
					return null;
				int index = (int)DrawToolType.Ellipse;
				if (index < 0 || index > ApplicationCursors.Cursors.Length)
					return null;
				return ApplicationCursors.Cursors[index];
			}
		}

		public static Cursor DrawLine
		{
			get
			{
				if (ApplicationCursors.Cursors == null)
					return null;
				int index = (int)DrawToolType.Line;
				if (index < 0 || index > ApplicationCursors.Cursors.Length)
					return null;
				return ApplicationCursors.Cursors[index];
			}
		}

		public static Cursor DrawPolygon
		{
			get
			{
				if (ApplicationCursors.Cursors == null)
					return null;
				int index = (int)DrawToolType.Polygon;
				if (index < 0 || index > ApplicationCursors.Cursors.Length)
					return null;
				return ApplicationCursors.Cursors[index];
			}
		}

		public static Cursor Pointer
		{
			get
			{
				if (ApplicationCursors.Cursors == null)
					return null;
				int index = (int)DrawToolType.Pointer;
				if (index < 0 || index > ApplicationCursors.Cursors.Length)
					return null;
				return ApplicationCursors.Cursors[index];
			}
		}

		public static Cursor DrawOnionRing
		{
			get
			{
				if (ApplicationCursors.Cursors == null)
					return null;
				int index = (int)DrawToolType.OnionRing;
				if (index < 0 || index > ApplicationCursors.Cursors.Length)
					return null;
				return ApplicationCursors.Cursors[index];
			}
		}
	
		#endregion

		#region constructor and destructor

		public ApplicationCursors()
		{
			Initialize();
		}
		
		#region IDisposable Members

		public void Dispose()
		{
			Uninitialize();
		}

		#endregion

		#endregion

		#region internal helpers

		private void Initialize()
		{
			Type type = this.GetType();
			int iCount = (int)DrawToolType.NumberOfDrawTools;
			_cursors = new Cursor[iCount];
			
			for (int i=0; i<iCount; i++)
			{
				string resource = null;

				switch ((DrawToolType)i)
				{
					case DrawToolType.Rectangle:
                        resource = "SiGlaz.Common.Resources.Cursors.Rectangle.cur";
						break;
					case DrawToolType.Ellipse:
                        resource = "SiGlaz.Common.Resources.Cursors.Ellipse.cur";
						break;
					case DrawToolType.Line:
                        resource = "SiGlaz.Common.Resources.Cursors.Line.cur";
						break;
					case DrawToolType.Polygon:
                        resource = "SiGlaz.Common.Resources.Cursors.Pencil.cur";
						break;
					default:
						resource = null;
						break;
				}

				if (resource != null)
				{
					try
					{
						using (System.IO.Stream stream = type.Assembly.GetManifestResourceStream(resource))
							_cursors[i] = new Cursor(stream);
					}
					catch(System.Exception exp)
					{
						Trace.WriteLine(exp);
					}
					finally
					{
						if (_cursors[i] == null)
							_cursors[i] = System.Windows.Forms.Cursors.Default;
					}
				}
				else
					_cursors[i] = System.Windows.Forms.Cursors.Default;
			}
		}

		private void Uninitialize()
		{
			_cursors = null;
		}

		#endregion
		
	}
}
