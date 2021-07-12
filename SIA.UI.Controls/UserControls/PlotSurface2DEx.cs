using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Windows.Forms;

using NPlot;
using NPlot.Windows;

using SIA.Native;

namespace SIA.UI.Controls.UserControls
{
	/// <summary>
	/// Summary description for PlotSurface2DEx.
	/// </summary>
	public class PlotSurface2DEx : NPlot.Windows.PlotSurface2D
	{
		public PlotSurface2DEx()
		{
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown (e);						
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp (e);
		}

		protected override bool IsInputKey(Keys keyData)
		{
			bool result = false;

			switch (keyData)
			{
				case Keys.Up:
				case Keys.Down:
				case Keys.Left:
				case Keys.Right:
					result = true;
					break;
				default:
					result = base.IsInputKey (keyData);
					break;
			}
			return result;
		}
	}
}
