using System;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace SIA.Plugins.Common
{
	/// <summary>
	/// The ShortcutInfo class provides functionality for shortcut item creation
	/// </summary>
	public class ShortcutInfo
	{
		private string _text = "";
		private string _category = "";
		private Image _image = null;
		private int _index = -1;

		public string Text
		{
			get {return _text;}
		}

		public string Category
		{
			get {return _category;}
		}

		public int Index
		{
			get {return _index;}
		}

		public Image Image
		{
			get {return _image;}
		}

		public ShortcutInfo(string text, string category, int index, Image image)
		{
			_text = text;
			_category = category;
			_index = index;
			_image = image;
		}
	}
}
