using System;
using System.Data;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

//using SiGlaz.RDE.Ex.UI;
using SIA.UI.Controls;
using SIA.UI.Controls.Common;
using SIA.UI.Controls.Helpers;

namespace SIA.UI.Components
{
	/// <summary>
	/// Summary description for HistoryListBox.
	/// </summary>
	public class HistoryListBox : System.Windows.Forms.ListBox
	{
		public class HistoryListBoxItem 
		{
			public string Text = "";
			public int ImageIndex = -1;

			public HistoryListBoxItem()
			{
			}

			public HistoryListBoxItem(string text, int index)
			{
				this.Text = text;
				this.ImageIndex = index;
			}

			public override string ToString()
			{
				return Text;
			}
		}

		#region Windows Form Members

		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ImageList imageList;

		#endregion
        
		#region Member Fields

		private HistoryHelper _historyHelper = null;
		
		private Color _selItemBackColor = Color.Blue;
		private Color _selItemForeColor = Color.White;
		private Font _selItemFont = null;

		private Color _skipItemBackColor = Color.White;
		private Color _skipItemForeColor = Color.Gray;
		private Font _skipItemFont = null;

		private int _padding = 2;
		private int _ignoreSelIndexChanged = 0;

		#endregion

		#region Properties

		[Browsable(true), Category("History List box")]
		public Color SelectedItemBackColor 
		{
			get {return _selItemBackColor;}
			set {_selItemBackColor = value;}
		}

		[Browsable(true), Category("History List box")]
		public Color SelectedItemForeColor 
		{
			get {return _selItemForeColor;}
			set {_selItemForeColor = value;}
		}

		[Browsable(true), Category("History List box")]
		public Font SelectedItemFont
		{
			get {return _selItemFont;}
			set {_selItemFont = value;}
		}

		[Browsable(true), Category("History List box")]
		public Font SkipItemFont
		{
			get {return _skipItemFont;}
			set {_skipItemFont = value;}
		}

		[Browsable(true), Category("History List box")]
		public Color SkipItemForeColor
		{
			get {return _skipItemForeColor;}
			set {_skipItemForeColor = value;}
		}

		[Browsable(false)]
		public SIA.UI.Controls.Helpers.HistoryHelper HistoryHelper
		{
			get {return _historyHelper;}
			set 
			{
				_historyHelper = value;
				OnHistoryHelperChanged();
			}
		}

		protected virtual void OnHistoryHelperChanged()
		{
			this.InitializeItems();
		}

		private new DrawMode DrawMode
		{
			get {return base.DrawMode;}
			set {base.DrawMode = value;}
		}

		public new int Padding
		{
			get {return _padding;}
			set {_padding = value;}
		}

		#endregion

		#region constructor and destructor
		
		public HistoryListBox() : base()
		{
			// initialize components
			this.InitializeComponent();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose (disposing);
			
			if (this._selItemFont != null)
					this._selItemFont.Dispose();
			this._selItemFont = null;
			
			if (this._skipItemFont != null)
				this._skipItemFont.Dispose();
			this._skipItemFont = null;

			if (this.imageList != null)
				this.imageList.Dispose();
			this.imageList = null;

			this._historyHelper = null;
			this.components.Dispose();
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			// 
			// imageList
			// 
			this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.imageList.ImageSize = new System.Drawing.Size(48, 48);
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// HistoryListBox
			// 
			this.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.DrawMode = DrawMode.OwnerDrawVariable;
		}

		#endregion

		#region Listbox override handlers

		#region Drawing helpers

		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			base.OnDrawItem (e);

			if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
			{
				this.DrawSelectedItem(e);
			}
			else
			{
				if (this._historyHelper != null)
				{
					int selIndex = _historyHelper.CurrentHistoryIndex;
					if (e.Index > selIndex)
						this.DrawSkippedItem(e);
					else
						this.DrawNormalItem(e);
				}
			}
		}

		protected override void OnMeasureItem(MeasureItemEventArgs e)
		{
			base.OnMeasureItem (e);

			if (e.Index < 0 || e.Index >= base.Items.Count)
				return;

			HistoryListBoxItem item = base.Items[e.Index] as HistoryListBoxItem;
			if (item == null)
				return;
			
			Graphics graph = e.Graphics;
			string text = item.Text;
			
			StringFormat format = this.GetStringFormat();
			SizeF sizeText = graph.MeasureString(text, this.Font, -1, format);

			SizeF thumbSize = imageList.ImageSize;
			thumbSize.Width += 2*_padding;
			thumbSize.Height += 2*_padding;
			
			e.ItemWidth = (int)Math.Ceiling(thumbSize.Width + sizeText.Width);
			e.ItemHeight = (int)Math.Ceiling(Math.Max(thumbSize.Height, sizeText.Height));
		}

		protected virtual void DrawSelectedItem(DrawItemEventArgs e)
		{
			if (e.Index < 0 || e.Index >= this.Items.Count)
				return ;

			Graphics graph = e.Graphics;
			Rectangle rcBound = e.Bounds;
			Font font = this._selItemFont;
			Brush brush = new SolidBrush(this._selItemBackColor);
			Brush texBrush = new SolidBrush(this._selItemForeColor);
			HistoryListBoxItem item = base.Items[e.Index] as HistoryListBoxItem;
			StringFormat format = this.GetStringFormat();
	
			try
			{
				// fill background with solid color
				graph.FillRectangle(brush, rcBound);

				// render item's image
				SizeF thumbSize = imageList.ImageSize;
                float xPos = _padding, yPos = rcBound.Top + (rcBound.Height - thumbSize.Height) * 0.5F;
                Image image = null;
                if (item.ImageIndex >= 0 && item.ImageIndex < this.imageList.Images.Count)
                    image = this.imageList.Images[item.ImageIndex];				
                if (image != null)
                    graph.DrawImage(image, xPos, yPos);

				// draw items' text
				string text = item.Text;
				SizeF sizeText = graph.MeasureString(text, font, -1, format);

				xPos = xPos + thumbSize.Width + _padding;
				if (sizeText.Height < rcBound.Top)
					yPos = rcBound.Top + (rcBound.Height - sizeText.Height)*0.5F;
				else
					yPos = rcBound.Top + _padding;

				RectangleF layoutRect = RectangleF.FromLTRB(xPos, yPos, rcBound.Right - _padding, rcBound.Bottom - _padding);
				graph.DrawString(text, font, texBrush, layoutRect, format);

				// draw items' focus rectangle
				e.DrawFocusRectangle();
			}
			finally
			{
				format.Dispose();
				brush.Dispose();
				texBrush.Dispose();
			}
		}

		protected virtual void DrawNormalItem(DrawItemEventArgs e)
		{
			if (e.Index < 0 || e.Index >= this.Items.Count)
				return ;

			Graphics graph = e.Graphics;
			Rectangle rcBound = e.Bounds;
			Font font = base.Font;
			Brush brush = new SolidBrush(base.BackColor);
			Brush texBrush = new SolidBrush(base.ForeColor);
			DrawItemState state = e.State;		
			HistoryListBoxItem item = base.Items[e.Index] as HistoryListBoxItem;
			StringFormat format = this.GetStringFormat();
			
			try
			{
				// fill background with solid color
				graph.FillRectangle(brush, rcBound);
				
				// render item's image
				SizeF thumbSize = imageList.ImageSize;
				Image image = this.imageList.Images[item.ImageIndex];
				float xPos = _padding, yPos = rcBound.Top + (rcBound.Height - thumbSize.Height)*0.5F;
				graph.DrawImage(image, xPos, yPos);

				// draw items' text
				string text = item.Text;
				SizeF sizeText = graph.MeasureString(text, font, -1, format);

				xPos = xPos + thumbSize.Width + _padding;
				if (sizeText.Height < rcBound.Top)
					yPos = rcBound.Top + (rcBound.Height - sizeText.Height)*0.5F;
				else
					yPos = rcBound.Top + _padding;

				RectangleF layoutRect = RectangleF.FromLTRB(xPos, yPos, rcBound.Right - _padding, rcBound.Bottom - _padding);
				graph.DrawString(text, font, texBrush, layoutRect, format);
			}
			finally
			{
				format.Dispose();
				brush.Dispose();
				texBrush.Dispose();
			}
		}

		protected virtual void DrawSkippedItem(DrawItemEventArgs e)
		{
			if (e.Index < 0 || e.Index >= this.Items.Count)
				return ;

			Graphics graph = e.Graphics;
			Rectangle rcBound = e.Bounds;
			Font font = this._skipItemFont;
			Brush brush = new SolidBrush(this._skipItemBackColor);
			Brush texBrush = new SolidBrush(this._skipItemForeColor);
	
			DrawItemState state = e.State;		
			HistoryListBoxItem item = base.Items[e.Index] as HistoryListBoxItem;
			StringFormat format = this.GetStringFormat();
			
			try
			{
				// fill background with solid color
				graph.FillRectangle(brush, rcBound);
			
				// render item's image
				SizeF thumbSize = imageList.ImageSize;
				Image image = this.imageList.Images[item.ImageIndex];
				float xPos = _padding, yPos = rcBound.Top + (rcBound.Height - thumbSize.Height)*0.5F;
				graph.DrawImage(image, xPos, yPos);

				// draw items' text
				string text = item.Text;
				SizeF sizeText = graph.MeasureString(text, font, -1, format);

				xPos = xPos + thumbSize.Width + _padding;
				if (sizeText.Height < rcBound.Top)
					yPos = rcBound.Top + (rcBound.Height - sizeText.Height)*0.5F;
				else
					yPos = rcBound.Top + _padding;

				RectangleF layoutRect = RectangleF.FromLTRB(xPos, yPos, rcBound.Right - _padding, rcBound.Bottom - _padding);
				graph.DrawString(text, font, texBrush, layoutRect, format);
			}
			finally
			{
				format.Dispose();
				brush.Dispose();
				texBrush.Dispose();
			}	
		}
		#endregion

		protected override void OnSelectedIndexChanged(EventArgs e)
		{
			base.OnSelectedIndexChanged (e);

//			if (this._historyHelper != null)
//			{
//				if (this._ignoreSelIndexChanged == 0)
//				{
//					this._historyHelper.LockRaiseEvent();
//					this._historyHelper.CurrentHistoryIndex = this.SelectedIndex;
//					this._historyHelper.UnlockRaiseEvent();
//					this.Invalidate(true);
//				}
//			}

			this.Invalidate(true);
		}

		#endregion

		#region Public Methods

		public int AddItem(Image image, string text)
		{
			int imageIndex = this.AddImage(image);
			HistoryListBoxItem item = new HistoryListBoxItem(text, imageIndex);
			return base.Items.Add(item);
		}

		public void RemoveItem(HistoryListBoxItem item)
		{
			int imageIndex = item.ImageIndex;
			if (imageIndex>=0 && imageIndex<imageList.Images.Count)
				this.RemoveImage(imageIndex);
			base.Items.Remove(item);
		}

		public void RemoveItem(int index)
		{
			HistoryListBoxItem item = base.Items[index] as HistoryListBoxItem;
			this.RemoveItem(item);
		}

		public void ClearItems()
		{
			// clear the image list
			imageList.Images.Clear();

			while (base.Items.Count > 0)
				this.RemoveItem(0);
		}

		#endregion

		#region Virtual Methods

		public virtual void OnUpdateHistoryItems()
		{
			if (this._historyHelper == null)
				throw new System.ArgumentNullException("HistoryHelper is not set to a reference");

			this._ignoreSelIndexChanged++;

			try
			{
				this.BeginUpdate();

				// removes old items
				this.ClearItems();
				
				// creates and adds new items
				if (_historyHelper.Histories != null)
				{
					foreach (History history in _historyHelper.Histories)
					{
						Image image = history.Thumbnail;
						string text = history.Description;
						this.AddItem(image, text);
					}

					// retrieves current selected history item
					int selIndex = _historyHelper.CurrentHistoryIndex;

					if (selIndex >= 0 && selIndex < this.Items.Count)
						this.SelectedIndex = selIndex;
				}
				
			}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);
				throw exp;
			}
			finally
			{
				this.EndUpdate();

				_ignoreSelIndexChanged--;
			}
		}

		protected virtual int AddImage(Image image)
		{
			imageList.Images.Add(image);
			return imageList.Images.Count - 1;
		}

		protected virtual void RemoveImage(int index)
		{
			if (index>=0 && index<imageList.Images.Count)
				imageList.Images.RemoveAt(index);
		}

		protected virtual StringFormat GetStringFormat()
		{
			StringFormat format = new StringFormat();
			format.Alignment = StringAlignment.Near;
			format.LineAlignment = StringAlignment.Center;
			format.Trimming  = StringTrimming.EllipsisWord;
			return format;
		}
		
		#endregion

		#region Internal helpers

		private void InitializeItems()
		{
			try
			{
                if (this._historyHelper == null)
                {
                    // clear old items
                    this.ClearItems();

                    return;
                }

				// synchronize items with history object
				OnUpdateHistoryItems();
				
				// initialize history event handlers
				if (_historyHelper != null)
				{
					_historyHelper.HistoryChanged += new EventHandler(History_HistoryChanged);
					_historyHelper.HistoryReset += new EventHandler(History_HistoryReset);
					_historyHelper.CurrentIndexChanged += new EventHandler(History_CurrentIndexChanged);
				}
			}
			catch (Exception exp)
			{
				Trace.WriteLine(exp);
				this._historyHelper = null;	
				
				throw;
			}
			finally
			{
			}
		}

		private void UninitializeItems()
		{
		}

		#endregion

		#region Event Handlers

		private void MenuItem_Click(object sender, EventArgs e)
		{
		}

		#region History Event Handlers
		
		private void History_HistoryChanged(object sender, EventArgs e)
		{
			this.OnUpdateHistoryItems();
		}

		private void History_HistoryReset(object sender, EventArgs e)
		{
			this.OnUpdateHistoryItems();
		}

		private void History_CurrentIndexChanged(object sender, EventArgs e)
		{
			try
			{
				int index = _historyHelper.CurrentHistoryIndex;
				if (index >=0 && index<this.Items.Count)
					this.SelectedIndex = index;
			}
			catch (System.Exception exp)
			{
				Trace.WriteLine(exp);
			}
			finally
			{
				this.Invalidate(true);
			}
		}

		#endregion

		#endregion

		
	}
}
