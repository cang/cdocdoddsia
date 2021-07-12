using System;

namespace SIA.Plugins.Common
{
	[Flags]
	public enum UIElementStatus : int
	{
		Visible = 1<<0,
		Invisible = 1<<1,
		Enable = 1<<2 | Visible,
		Disable = 1<<3 | Visible,
		Checked = 1<<8 | Enable,
        Unchecked = 1 << 9 | Enable,

		LowOrderMask = 0xFF,
		HighOrderMask = 0xFF00,
	}
}
