using System;
using System.Drawing;
using System.IO;

using SIA.UI.Help;
using System.Windows.Forms;

namespace SIA.UI.Controls.Dialogs
{
	/// <summary>
    /// Summary description for DlgContextSensitiveHelp.
	/// </summary>
	public class DlgContextSensitiveHelp : Form
	{
		public virtual string OnGetHelpIndexKey()
		{
			return this.GetType().FullName;
		}

		protected override void OnHelpRequested(System.Windows.Forms.HelpEventArgs hevent)
		{
			base.OnHelpRequested (hevent);
			
#if GENERATE_HELP_INDEX
			FileStream stream = new FileStream(@"C:\help_index.dat", FileMode.OpenOrCreate, FileAccess.Write);
			stream.Seek(0, SeekOrigin.End);
			StreamWriter writer = new StreamWriter(stream, System.Text.Encoding.ASCII);
			writer.WriteLine(this.GetType().FullName);
			writer.Close();
			stream.Close();
#else
			string key = this.OnGetHelpIndexKey();
			hevent.Handled = ContextSensitiveHelper.ShowHelpIndex(this, key);			
#endif
		}

	}
}
