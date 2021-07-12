using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace SiGlaz.UI.CustomControls
{
	public class ExplorerBarLunaBlueTheme : IExplorerBarTheme
	{
		#region Member fields

		#region Explorer bar format
		private GradientColor _bkColor = null;
		#endregion Explorer bar format

		#region Expolorer items group format
		private GradientColor _bkCaptionColor = null;
		private Color _captionNormalColor;
		private Color _captionHighlightColor;
		private Font _captionTextFont = null;
		private Color _bkClientColor;
		private Color _clientBorderColor;
		private ImageAttributes _captionImageAttr = null;
		#endregion Expolorer items group format

		#region Explorer items format
		private Font _explorerItemNormalFont = null;
		private SolidBrush _explorerItemNormalBrush = null;
		private Font _explorerItemHighlightFont = null;
		private SolidBrush _explorerItemHighlightBrush = null;
		private StringFormat _strFormat = null;
		private ImageAttributes _explorerItemImageAttr = null;
		#endregion Explorer items format

		#endregion Member fields

		#region Constructors and Destructors
		public ExplorerBarLunaBlueTheme()
		{
			// Explorer bar format
			_bkColor = new GradientColor(Color.FromArgb(123, 162, 231), Color.FromArgb(99, 117, 214));

			// Expolorer items group format
			_bkCaptionColor = new GradientColor(Color.FromArgb(255, 255, 255), Color.FromArgb(198, 209, 244));
			_captionNormalColor = Color.FromArgb(33, 93, 198);
			_captionHighlightColor = Color.FromArgb(66, 142, 255);
			_captionTextFont = new Font("Tahoma", 10, FontStyle.Bold);
			_bkClientColor = Color.FromArgb(214, 223, 247);
			_clientBorderColor = Color.FromArgb(255, 255, 255);

			//Explorer items format
			_explorerItemNormalFont = new Font("Tahoma", 8, FontStyle.Regular);
			_explorerItemNormalBrush = new SolidBrush(Color.FromArgb(33, 93, 198));
			_explorerItemHighlightFont = new Font("Tahoma", 8, FontStyle.Bold | FontStyle.Underline);
			_explorerItemHighlightBrush = new SolidBrush(Color.FromArgb(66, 142, 255));

			_strFormat = new StringFormat();
			_strFormat.Alignment = StringAlignment.Near;
			_strFormat.LineAlignment = StringAlignment.Center;
			_strFormat.FormatFlags = StringFormatFlags.NoWrap;

			_explorerItemImageAttr = new ImageAttributes();
			_explorerItemImageAttr.SetColorKey(Color.Transparent, Color.Transparent);// _bkClientColor, _bkClientColor);
		}
		#endregion Constructors and Destructors

		#region IExplorerBarTheme Members

		private const string _name = "Luna Blue Theme";
		public string Name
		{
			get
			{
				// TODO:  Add ExplorerBarLunaBlueTheme.Name getter implementation
				return _name;
			}
		}

		public void DrawItem(Graphics grph, ExplorerItem item, bool bDrawBackground)
		{
			// TODO:  Add ExplorerBarLunaBlueTheme.DrawItem implementation

			Rectangle bounds = item.Bounds;
			// 1. Draw background
			if (bDrawBackground)
			{
				using (SolidBrush brush = new SolidBrush(_bkClientColor))
				{
					grph.FillRectangle(brush, item.Bounds);
				}
			}

			// 2. Draw image
            if (!item.Enabled)
            {
                if (item.Image != null)
                    grph.DrawImage(item.Image, item.ImageLocation);
            }
            else  if (item.Image != null)
				grph.DrawImage(item.Image, item.ImageLocation);

			// 3. Draw text
            if (!item.Enabled)
            {
                Brush brush = Brushes.DarkGray;
                {
                    grph.DrawString(
                        item.Text,
                        _explorerItemNormalFont,
                        brush,
                        item.TextBounds,
                        _strFormat);
                }
            }
            else
            {
                if (item.Status == eStatus.None)
                    grph.DrawString(
                        item.Text,
                        _explorerItemNormalFont,
                        _explorerItemNormalBrush,
                        item.TextBounds,
                        _strFormat);
                else
                    grph.DrawString(
                        item.Text,
                        _explorerItemHighlightFont,
                        _explorerItemHighlightBrush,
                        item.TextBounds,
                        _strFormat);
            }
		}

		public void DrawItemsGroup(Graphics grph, ExplorerItemsGroup itemsGroup, bool bDrawItems)
		{
			// TODO:  Add ExplorerBarLunaBlueTheme.DrawItemsGroup implementation

			try
			{
				if (grph.SmoothingMode != SmoothingMode.AntiAlias)
					grph.SmoothingMode = SmoothingMode.AntiAlias;
				if (grph.InterpolationMode != InterpolationMode.High)
					grph.InterpolationMode = InterpolationMode.High;

				Rectangle rect;

				// 1. Draw caption bar
				// draw background
				using (LinearGradientBrush brush = 
						   new LinearGradientBrush(
						   itemsGroup.CaptionBounds, 
						   _bkCaptionColor.Start, 
						   _bkCaptionColor.End, 
						   LinearGradientMode.Horizontal))
				{
					grph.FillPath(brush, itemsGroup.CaptionPath);
				}
				
				// draw image				
				if (itemsGroup.Image != null)
				{
					grph.DrawImage(itemsGroup.Image, itemsGroup.ImageLocation);
				}

				// get color
				Color color = (itemsGroup.CaptionStatus == eStatus.None ? _captionNormalColor : _captionHighlightColor);

				// draw text
				using (SolidBrush brush = new SolidBrush(color))
				{
					grph.DrawString(
						itemsGroup.Text, 
						_captionTextFont,
						brush, 
						itemsGroup.CaptionTextBounds,
						_strFormat);
				}

				// draw button
				using (SolidBrush brush = new SolidBrush(Color.White))
				{
					grph.FillEllipse(brush, itemsGroup.ButtonBounds);					
				}
				using (Pen pen = new Pen(color, 1.0f))
				{
					rect = itemsGroup.ButtonBounds;
					float kw = 3;
					float kh = 3;
					float x = (float)rect.X + 0.5f*rect.Width;

					if (itemsGroup.Status == eExplorerItemGroupStatus.Collapsed)
					{						
						float y = (float)rect.Y + 12.5f;
						grph.DrawLine(pen, x, y, x-kw, y - kh);
						grph.DrawLine(pen, x, y, x+kw, y - kh);

						y -= 0.75f;
						grph.DrawLine(pen, x, y, x-kw, y - kh);
						grph.DrawLine(pen, x, y, x+kw, y - kh);

						y -= 4;
						grph.DrawLine(pen, x, y, x-kw, y - kh);
						grph.DrawLine(pen, x, y, x+kw, y - kh);

						y -= 0.75f;
						grph.DrawLine(pen, x, y, x-kw, y - kh);
						grph.DrawLine(pen, x, y, x+kw, y - kh);
					}
					else
					{						
						float y = (float)rect.Y + 4;
						grph.DrawLine(pen, x, y, x-kw, y + kh);
						grph.DrawLine(pen, x, y, x+kw, y + kh);

						y += 0.75f;
						grph.DrawLine(pen, x, y, x-kw, y + kh);
						grph.DrawLine(pen, x, y, x+kw, y + kh);

						y += 4;
						grph.DrawLine(pen, x, y, x-kw, y + kh);
						grph.DrawLine(pen, x, y, x+kw, y + kh);

						y += 0.75f;
						grph.DrawLine(pen, x, y, x-kw, y + kh);
						grph.DrawLine(pen, x, y, x+kw, y + kh);
					}
				}

				
				//DRAW ITEMS
				if (!bDrawItems)
					return;

				// -- begin drawing items
				if (itemsGroup.Status == eExplorerItemGroupStatus.Collapsed)
					return;

				Image cache = null;
				if (itemsGroup.Cache == null)
				{	
					Rectangle clientBounds = itemsGroup.ClientBounds;

					cache = new Bitmap(clientBounds.Width, clientBounds.Height);
					using (Graphics clientGrph = Graphics.FromImage(cache))
					{
						// 2. Draw client background included children items						
						clientGrph.Clear(_bkClientColor);
						
						// 3. Draw border
						using (Pen pen = new Pen(_clientBorderColor, 1.0f))
						{
							clientGrph.DrawLine(pen, 0, 0, 0, clientBounds.Height-1);
							clientGrph.DrawLine(pen, 0, clientBounds.Height-1, clientBounds.Width-1, clientBounds.Height-1);
							clientGrph.DrawLine(pen, clientBounds.Width-1, clientBounds.Height-1, clientBounds.Width-1, 0);
						}
		
						// 4. Draw children items
						if (itemsGroup != null && itemsGroup.Items != null)
						{
							foreach (ExplorerItem item in itemsGroup.Items)
								this.DrawItem(clientGrph, item, false);
						}
					}

					itemsGroup.Cache = cache;
				}

				cache = itemsGroup.Cache;
				if (cache != null)
				{
					grph.DrawImage(cache, itemsGroup.ClientBounds, 0, 0, cache.Width, cache.Height, GraphicsUnit.Pixel);
				}
				// --end
				
			}
			catch
			{
				throw;
			}
		}

		public void DrawExplorerBar(ExplorerItemsGroupCollection itemsGroups, Size size, ref Image cache)
		{
			// TODO:  Add ExplorerBarLunaBlueTheme.DrawExplorerBar implementation
			
			if (cache == null)
			{
				cache = new Bitmap(size.Width, size.Height);

				using (Graphics g = Graphics.FromImage(cache))
				{
					using (LinearGradientBrush brush = 
							   new LinearGradientBrush(
							   new Rectangle(0, 0, size.Width, size.Height),
							   _bkColor.Start, _bkColor.End, 
							   LinearGradientMode.Vertical))
					{
						// 1. Draw background
						g.FillRectangle(brush, 0, 0, size.Width, size.Height);						
					}

					// 2. Draw children groups
					if (itemsGroups != null)
					{
						foreach (ExplorerItemsGroup group in itemsGroups)
							this.DrawItemsGroup(g, group, true);
					}
				}
			}

			//if (cache != null)
			//{
			//	grph.DrawImage(cache, 0, 0);
			//}			

			// 3. Draw scrollbar(s) if they have already existed
		}

		#endregion
	}
}
