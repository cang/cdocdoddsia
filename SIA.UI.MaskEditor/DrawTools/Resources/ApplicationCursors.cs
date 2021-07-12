using System;
using System.Collections;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;
using System.Diagnostics;

using SiGlaz.RDE.Ex.Mask;
using SIA.UI.MaskEditor;
using SIA.UI.MaskEditor.DrawTools;

namespace SIA.UI.MaskEditor.DrawTools.Resources
{
	/// <summary>
	/// Summary description for ApplicationCursors.
	/// </summary>
	public class ApplicationCursors : IDisposable
	{
		#region internal constants
		private const int CURSOR_ZOOM_IN = (int)DrawToolType.NumberOfDrawTools;
		private const int CURSOR_ZOOM_OUT = (int)DrawToolType.NumberOfDrawTools + 1;
		private const int CURSOR_PAN = (int)DrawToolType.NumberOfDrawTools + 2;

		private const int TOTAL_CURSORS = CURSOR_PAN + 1;
		#endregion

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
	
		public static Cursor ZoomIn
		{
			get
			{
				if (ApplicationCursors.Cursors == null)
					return null;
				return ApplicationCursors.Cursors[CURSOR_ZOOM_IN];
			}
		}
	

		public static Cursor ZoomOut
		{
			get
			{
				if (ApplicationCursors.Cursors == null)
					return null;
				return ApplicationCursors.Cursors[CURSOR_ZOOM_OUT];
			}
		}
		
		public static Cursor Pan
		{
			get
			{
				if (ApplicationCursors.Cursors == null)
					return null;
				return ApplicationCursors.Cursors[CURSOR_PAN];
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
			int iCount = TOTAL_CURSORS;
			_cursors = new Cursor[iCount];
			
			for (int i=0; i<iCount; i++)
			{
				string resource = null;

				switch ((int)i)
				{
					case (int)DrawToolType.Rectangle:
						resource = "SIA.UI.MaskEditor.DrawTools.Resources.Rectangle.cur";
						break;
					case (int)DrawToolType.Ellipse:
						resource = "SIA.UI.MaskEditor.DrawTools.Resources.Ellipse.cur";
						break;
					case (int)DrawToolType.Line:
						resource = "SIA.UI.MaskEditor.DrawTools.Resources.Line.cur";
						break;
					case (int)DrawToolType.Polygon:
						resource = "SIA.UI.MaskEditor.DrawTools.Resources.Pencil.cur";
						break;
					case CURSOR_ZOOM_IN:
						resource = "SIA.UI.MaskEditor.DrawTools.Resources.zoomin.cur";
						break;
					case CURSOR_ZOOM_OUT:
						resource = "SIA.UI.MaskEditor.DrawTools.Resources.zoomout.cur";
						break;
					case CURSOR_PAN:
						resource = "SIA.UI.MaskEditor.DrawTools.Resources.pan.cur";
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
