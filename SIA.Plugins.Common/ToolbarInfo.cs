using System;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace SIA.Plugins.Common
{
	/// <summary>
	/// The ToolBarInfo class provides functionality for toolbar button creation
	/// </summary>
	public class ToolBarInfo
	{
		private string _text = "";
		private string _tooltip = "";
		private Image _image = null;
		private int _imageIndex = -1;
		private Image _actImage = null;
		private int _actImageIndex = -1;
		private ToolBarButtonStyle _style = ToolBarButtonStyle.PushButton;
		private SeparateStyle _separateStyle = SeparateStyle.None;
		
		private int _index = -1;
		private ToolBarButton _toolbarButton = null;
		private object _tag = null;

		public string Text
		{
			get {return _text;}
		}

		public string Tooltip
		{
			get {return _tooltip;}
		}

		public ToolBarButtonStyle Style
		{
			get {return _style;}
		}

		public Image Image
		{
			get {return _image;}
		}

		public int ImageIndex
		{
			get {return _imageIndex;}
			set {_imageIndex = value;}
		}

		public Image ActiveImage
		{
			get {return _actImage;}
		}

		public int ActiveImageIndex
		{
			get {return _actImageIndex;}
			set {_actImageIndex = value;}
		}
		
		public SeparateStyle SeparateStyle
		{
			get {return _separateStyle;}
		}

		public ToolBarButton ToolBarButton
		{
			get {return _toolbarButton;}
			set {_toolbarButton = value;}
		}

		public int Index
		{
			get {return _index;}
		}

		public object Tag
		{
			get {return _tag;}
			set {_tag = value;}
		}

		public ToolBarInfo(string text, string tooltip, int index)
		{
			_text = text;
			_tooltip = tooltip;
			_index = index;
		}

		public ToolBarInfo(string text, string tooltip, int index, Image image) : this(text, tooltip, index)
		{
			_image = image;
		}

		public ToolBarInfo(string text, string tooltip, int index, Image image, SeparateStyle separateStyle) 
			: this(text, tooltip, index, image)
		{
			this._separateStyle = separateStyle;
		}

		public ToolBarInfo(string text, string tooltip, int index, Image image, SeparateStyle separateStyle, ToolBarButtonStyle style) 
			: this(text, tooltip, index, image)
		{
			this._style = style;
			this._separateStyle = separateStyle;
		}

		public ToolBarInfo(string text, string tooltip, int index, Image normImage, Image actImage, SeparateStyle separateStyle, ToolBarButtonStyle style) 
			: this(text, tooltip, index, normImage)
		{
			this._actImage = actImage;
			this._style = style;
			this._separateStyle = separateStyle;
		}
	}
}
