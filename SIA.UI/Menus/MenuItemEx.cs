using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;

using SIA.Plugins.Common;
using SiGlaz.UI.CustomControls;

namespace SIA.UI
{
	/// <summary>
	/// Summary description for MenuItemEx.
	/// </summary>
	public class MenuItemEx : OwnerDrawmenuItem
	{
		private class MenuInfoComparer : IComparer
		{
			#region IComparer Members

			public int Compare(object x, object y)
			{
				MenuItemEx h1 = x as MenuItemEx;
				MenuItemEx h2 = y as MenuItemEx;
				return (int)(h1.CommandHandler.MenuInfo.Index - h2.CommandHandler.MenuInfo.Index);
			}

			#endregion
		}

		private object _tag = null;
		private IUICommandHandler _cmdHandler = null;
		private string _command = "";
		private object[] _arguments = null;

		public new object Tag
		{
			get {return _tag;}
			set {_tag = value;}
		}

		public IUICommandHandler CommandHandler
		{
			get {return _cmdHandler;}
			set {_cmdHandler = value;}
		}

		public string Command
		{
			get {return _command;}
			set {_command = value;}
		}

		public object[] Arguments
		{
			get {return _arguments;}
			set {_arguments = value;}
		}

		public MenuItemEx() : base()
		{
		}

		public MenuItemEx(string text) : this(text, string.Empty, null)
		{
		}

		public MenuItemEx(string text, string command) : this(text, command, null)
		{
		}

		public MenuItemEx(string text, string command, params object[] args) : base(text)
		{
			this._command = command;
			this._arguments = args;
		}

		public virtual void BeginUpdateMenuItems()
		{
		}

		public virtual void EndUpdateMenuItems()
		{
		}

		public virtual void RaiseClickEvent()
		{
			this.OnClick(EventArgs.Empty);
		}

	}
}
