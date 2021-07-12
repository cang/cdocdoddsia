using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;

namespace SIA.UI.Components
{
	/// <summary>
	/// Enhances the built-int .NET NumericUpDown control to catch the text box text changed event
	/// </summary>
	public class IntensityUpDown 
        : NumericUpDown
	{
		public IntensityUpDown() : base()
		{
		}

		protected override void OnTextBoxTextChanged(object source, EventArgs e)
		{
			base.OnTextBoxTextChanged (source, e);
			this.ValidateEditText();
		}
	}
}
