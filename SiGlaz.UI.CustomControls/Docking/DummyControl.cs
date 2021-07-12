using System;
using System.Windows.Forms;

namespace SiGlaz.UI.CustomControls.Docking
{
	internal class DummyControl : Control
	{
		public DummyControl()
		{
			SetStyle(ControlStyles.Selectable, false);
		}
	}
}
