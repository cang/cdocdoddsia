using System;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace SIA.Plugins.Common
{
    /// <summary>
    /// Flags indicates type of the separation for the menu item and toolbar
    /// </summary>
	[Flags]
	public enum SeparateStyle : int
	{
		None = 0, // no separation
		Above = 1, 
		Below = 2,
		Both = 3,
		Before = 1,
		After = 2
	}

	/// <summary>
	/// The MenuInfo class provide functionality for menu item creation
	/// </summary>
	public class MenuInfo
	{
		private string _text = "";
		private string _category = "";
		private Shortcut _shortcuts = Shortcut.None;
		private Image _image = null;
		private SeparateStyle _separateStyle = SeparateStyle.None;
		
		private int _index = -1;
		private MenuItem _menuItem = null;
		private object _tag = null;

		public string Text
		{
			get {return _text;}
		}

		public string Category
		{
			get {return _category;}
		}

		public Shortcut Shortcuts
		{
			get {return _shortcuts;}
		}

		public Image Image
		{
			get {return _image;}
		}

		public SeparateStyle SeparateStyle
		{
			get {return _separateStyle;}
		}

		public MenuItem MenuItem
		{
			get {return _menuItem;}
			set {_menuItem = value;}
		}

		public int Index
		{
			get {return _index;}
			set {_index = value;}
		}

		public object Tag
		{
			get {return _tag;}
			set {_tag = value;}
		}

		public MenuInfo(string text, string category, Shortcut shortcuts, int index)
		{
			_text = text;
			_category = category;
			_shortcuts = shortcuts;
			_index = index;
		}

		public MenuInfo(string text, string category, Shortcut shortcuts, int index, Image image) 
            : this(text, category, shortcuts, index)
		{
			_image = image;
		}

		public MenuInfo(string text, string category, Shortcut shortcuts, int index, Image image, SeparateStyle separateStyle) 
			: this(text, category, shortcuts, index, image)
		{
			this._separateStyle = separateStyle;
		}
	}
}
