using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Threading;

using SIA.UI.Components;
using SIA.UI.Components.Helpers;

using SIA.UI.Controls;
using SIA.UI.Controls.Common;
//CONG using SIA.UI.Controls.Components;
using SIA.UI.Controls.UserControls;
using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.Helpers;
using SIA.UI.Controls.Helpers.VisualAnalysis;
using SIA.UI.Controls.Utilities;

namespace SIA.UI.Controls.Helpers.VisualAnalysis.DataProfilers
{
	/// <summary>
	/// Summary description for IDataProfiler.
	/// </summary>
	public interface IDataProfiler : IDisposable
	{
        DataProfileHelper Container { get;}
        PointF Begin { get; set;}
        PointF End { get; set;}
		
		void Render(Graphics graph, Rectangle rcClip);

		void MouseDown(MouseEventArgs e);
		void MouseMove(MouseEventArgs e);
		void MouseUp(MouseEventArgs e);

		void DisplaySettingsWindow();
		void Export();
		void Update();

		void InteractiveLine(RasterViewerLineEventArgs e);
		void UpdateSelectedValue(object abscissaValue, object ordinaryValue);
	}

    public interface IDataProfileContainer
    {
    }
}
