using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security;
using System.Xml;

using SIA.Common.Mask;

using SiGlaz.Common;
using SIA.UI.MaskEditor.DocToolkit;
using SIA.UI.MaskEditor.DrawTools;

using SIA.SystemLayer;

using SIA.UI.Components;
using SIA.UI.Components.Helpers;

namespace SIA.UI.MaskEditor.DrawTools
{
	/// <summary>
	/// Summary description for GraphicsContainer.
	/// </summary>
    public class GraphicsContainer : SiGlaz.Common.GraphicsList
	{
		public GraphicsContainer() : base()
		{
		}

		public GraphicsContainer(SerializationInfo info, StreamingContext context) : base()
		{
		}

		public new GraphicsContainer Clone()
		{
			GraphicsContainer result = new GraphicsContainer();

			GraphicsList loadObjects = base.Clone();
			for (int i=0; i<loadObjects.Count; i++)
			{
				DrawObject obj = loadObjects[i];
				if (obj is SiGlaz.Common.DrawPolygon)
				{
					SIA.UI.MaskEditor.DrawTools.DrawPolygon drawPolygon = new SIA.UI.MaskEditor.DrawTools.DrawPolygon((SiGlaz.Common.DrawPolygon)obj);
					result.Add(drawPolygon);
				}
				else if (obj is SiGlaz.Common.DrawLine)
				{
					SIA.UI.MaskEditor.DrawTools.DrawLine drawLine = new SIA.UI.MaskEditor.DrawTools.DrawLine((SiGlaz.Common.DrawLine)obj);
					result.Add(drawLine);
				}
				else if (obj is SiGlaz.Common.DrawEllipse)
				{
					SIA.UI.MaskEditor.DrawTools.DrawEllipse drawEllipse = new SIA.UI.MaskEditor.DrawTools.DrawEllipse((SiGlaz.Common.DrawEllipse)obj);
					result.Add(drawEllipse);
				}
				else if (obj is SiGlaz.Common.DrawRectangle)
				{
					SIA.UI.MaskEditor.DrawTools.DrawRectangle drawRectangle = new SIA.UI.MaskEditor.DrawTools.DrawRectangle((SiGlaz.Common.DrawRectangle)obj);
					result.Add(drawRectangle);
				}
				else if (obj is SiGlaz.Common.DrawOnionRing)
				{
					SIA.UI.MaskEditor.DrawTools.DrawOnionRing drawOnionRing = new SIA.UI.MaskEditor.DrawTools.DrawOnionRing((SiGlaz.Common.DrawOnionRing)obj);
					result.Add(drawOnionRing);
				}
			}

			return result;
		}
	}
}
