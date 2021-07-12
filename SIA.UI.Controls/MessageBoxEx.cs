using System;
using System.Collections;
using System.Text;
using System.Windows.Forms;

namespace SIA.UI.Controls
{
	/// <summary>
	/// Utility class provides customized functions for displaying message boxes
	/// </summary>
	public class MessageBoxEx
	{
        /// <summary>
        /// Reports an error message
        /// </summary>
        /// <param name="message">The text to report</param>
        /// <returns>dialog result</returns>
		public static DialogResult Error(string message)
		{
			return MessageBoxEx.Display(message, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		public static DialogResult Error(string format, params object[] args)
		{
			string message = String.Format(format, args);
			return MessageBoxEx.Display(message, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		public static DialogResult Warning(String message)
		{
			return MessageBoxEx.Display(message, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}

		public static DialogResult Warning(string format, params object[] args)
		{
			string message = String.Format(format, args);
			return MessageBoxEx.Display(message, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		public static DialogResult Info(String message)
		{
			return MessageBoxEx.Display(message, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}

		public static DialogResult Info(string format, params object[] args)
		{
			string message = String.Format(format, args);
			return MessageBoxEx.Display(message, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		public static bool ConfirmYesNo(String message)
		{
			return DialogResult.Yes == MessageBoxEx.Display(message, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
		}

		public static DialogResult ConfirmYesNoCancel(String message)
		{
			return MessageBoxEx.Display(message, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
		}

		public static DialogResult Display(string text, MessageBoxButtons buttons, MessageBoxIcon icon)
		{
			IWin32Window owner = System.Windows.Forms.Form.ActiveForm;
			string caption = Application.ProductName;
			return Display(owner, text, caption, buttons, icon);
		}

		public static DialogResult Display(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
		{
			return System.Windows.Forms.MessageBox.Show(owner, text, caption, buttons, icon);
		}
	}
}
