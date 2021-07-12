using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SiGlaz.UI.CustomControls
{
	public class ExplorerBarAeroTheme : IExplorerBarTheme
	{
		#region Member fields
		//private Color _bkColor = Color.White;
		private Color _bkColor = SystemColors.Control;
		
		private Color _captionTextColor = Color.FromArgb(0, 51, 153);
		
		//private Color _groupBorderColor = Color.FromArgb(233, 233, 233);
		//private Color _groupBorderColor = Color.FromArgb(0, 45, 150);
		private Color _groupBorderColor = Color.FromArgb(142, 179, 231);

		private GradientColor _captionColorHighlightColor1 = 
			new GradientColor(Color.FromArgb(228, 244, 253), Color.FromArgb(217, 239, 252));
		private GradientColor _captionColorHighlightColor2 = 
			new GradientColor(Color.FromArgb(188, 229, 252), Color.FromArgb(167, 217, 244));
		//private Color _captionNormalColor = Color.White;
		private Color _captionNormalColor = SystemColors.Control;
		private Color _bkClientColor = Color.White;
		private Font _captionTextFont = new Font("Tahoma", 10, FontStyle.Bold);
				
		private Color _itemTextColor = Color.FromArgb(0, 102, 204);
		private GradientColor _itemHighlightColor = 
			new GradientColor(Color.FromArgb(227, 242, 250), Color.FromArgb(208, 236, 252));
		private Color _itemHightlightBorderColor = Color.FromArgb(179, 228, 249);
		private Font _itemTextFont = new Font("Tahoma", 8, FontStyle.Bold);

		private StringFormat _strFormat = new StringFormat();
		#endregion Member fields

		#region Contructors and Destructors
		public ExplorerBarAeroTheme()
		{
			_strFormat.Alignment = StringAlignment.Near;
			_strFormat.LineAlignment = StringAlignment.Center;
			_strFormat.FormatFlags = StringFormatFlags.NoWrap;
		}
		#endregion Contructors and Destructors

		#region IExplorerBarTheme Members

		private const string _name = "Aero Theme";
		public string Name
		{
			get
			{
				// TODO:  Add ExplorerBarAeroTheme.Name getter implementation
				return _name;
			}
		}

		public void DrawItem(System.Drawing.Graphics grph, ExplorerItem item, bool bDrawBackground)
		{
			// TODO:  Add ExplorerBarAeroTheme.DrawItem implementation

			// 1. Draw background
			Rectangle bounds = item.Bounds;
            //if (!item.Enabled)
            //{
            //}
            //else 
                if (item.Status == eStatus.None)
			{
				using (SolidBrush brush = new SolidBrush(_bkClientColor))
				{
					grph.FillRectangle(brush, bounds.X, bounds.Y, bounds.Width+1, bounds.Height+1);
				}
			}
			else
			{
				using (GraphicsPath path = 
						   Utils.CreateRoundRect(
						   bounds.X+1, bounds.Y+1, (float)bounds.Width-2.0f, (float)bounds.Height-2.0f, 
						   3.0f,
						   true, true, true, true))				
				{
					using (LinearGradientBrush brush = 
							   new LinearGradientBrush(
							   bounds, 
							   _itemHighlightColor.Start, _itemHighlightColor.End, 
							   LinearGradientMode.Vertical))
					
					{
						grph.FillPath(brush, path);
					}

					using (Pen pen = new Pen(_itemHightlightBorderColor, 1.0f))
					{
						grph.DrawPath(pen, path);
					}
				}
			}		

			// 2. Draw image
            if (!item.Enabled)
            {
                if (item.Image != null)
                    grph.DrawImage(item.Image, item.ImageLocation);
            }
            else if (item.Image != null)
				grph.DrawImage(item.Image, item.ImageLocation);

			// 3. Draw text
            if (!item.Enabled)
            {
                Brush brush = Brushes.DarkGray;
                {
                    grph.DrawString(
                        item.Text,
                        _itemTextFont,
                        brush,
                        item.TextBounds,
                        _strFormat);
                }
            }
            else
            {
                using (SolidBrush brush = new SolidBrush(_itemTextColor))
                {
                    grph.DrawString(
                        item.Text,
                        _itemTextFont,
                        brush,
                        item.TextBounds,
                        _strFormat);
                }
            }
		}

		public void DrawItemsGroup(System.Drawing.Graphics grph, ExplorerItemsGroup itemsGroup, bool bDrawItems)
		{
			// TODO:  Add ExplorerBarAeroTheme.DrawItemsGroup implementation

			try
			{
				if (grph.SmoothingMode != SmoothingMode.AntiAlias)
					grph.SmoothingMode = SmoothingMode.AntiAlias;
				if (grph.InterpolationMode != InterpolationMode.High)
					grph.InterpolationMode = InterpolationMode.High;

				Rectangle rect = itemsGroup.CaptionBounds;

				// 1. Draw caption bar
				// draw background
				if (itemsGroup.CaptionStatus == eStatus.None)
				{
					//using (SolidBrush brush = new SolidBrush(_captionNormalColor))
					using (LinearGradientBrush brush = 
							   new LinearGradientBrush(
							   rect,
							   Color.White,
							   _captionNormalColor,
							   LinearGradientMode.Vertical))
					{
						grph.FillRectangle(brush, rect);
					}
				}
				else
				{
					Rectangle rct = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height/2);
					using (LinearGradientBrush brush = 
							   new LinearGradientBrush(
							   rct, 
							   _captionColorHighlightColor1.Start, 
							   _captionColorHighlightColor1.End, 
							   LinearGradientMode.Vertical))
					{
						grph.FillRectangle(brush, rct);
					}

					rct = Rectangle.FromLTRB(rect.X, rect.Y + rect.Height/2, rect.Right, rect.Bottom);					
					using (LinearGradientBrush brush = 
							   new LinearGradientBrush(
							   rct, 
							   _captionColorHighlightColor2.Start, 
							   _captionColorHighlightColor2.End, 
							   LinearGradientMode.Vertical))
					{
						grph.FillRectangle(brush, rct);
					}
				}
				using (Pen pen = new Pen(_groupBorderColor, 1.0f))
				{
					grph.DrawRectangle(pen, rect);
				}
								
				// draw image				
				if (itemsGroup.Image != null)
				{
					grph.DrawImage(itemsGroup.Image, itemsGroup.ImageLocation);
				}

				// draw text
				using (SolidBrush brush = new SolidBrush(_captionTextColor))
				{
					grph.DrawString(
						itemsGroup.Text, 
						_captionTextFont,
						brush, 
						itemsGroup.CaptionTextBounds,
						_strFormat);
				}

				// draw button
				using (Pen pen = new Pen(_captionTextColor, 1.0f))
				{
					rect = itemsGroup.ButtonBounds;
					float kw = 3;
					float kh = 3;
					float x = (float)rect.X + 0.5f*rect.Width;

					if (itemsGroup.Status == eExplorerItemGroupStatus.Collapsed)
					{						
						float y = (float)rect.Y + 0.5f*rect.Height + 0.5f*kh;
						grph.DrawLine(pen, x, y, x-kw, y - kh);
						grph.DrawLine(pen, x, y, x+kw, y - kh);

						y -= 0.75f;
						grph.DrawLine(pen, x, y, x-kw, y - kh);
						grph.DrawLine(pen, x, y, x+kw, y - kh);
					}
					else
					{						
						float y = (float)rect.Y + 0.5f*rect.Height - 0.75f*kh;
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

						if (clientGrph.SmoothingMode != SmoothingMode.AntiAlias)
							clientGrph.SmoothingMode = SmoothingMode.AntiAlias;
						if (clientGrph.InterpolationMode != InterpolationMode.High)
							clientGrph.InterpolationMode = InterpolationMode.High;
						
						// 3. Draw border
						//using (Pen pen = new Pen(_groupBorderColor, 1.0f))
						//{

							//clientGrph.DrawRectangle(pen, 0, 0, (float)clientBounds.Width-0.75f, (float)clientBounds.Height-0.75f);

							//clientGrph.DrawLine(pen, 0, 0, 0, clientBounds.Height-1);
							//clientGrph.DrawLine(pen, 0, clientBounds.Height-1, clientBounds.Width-1, clientBounds.Height-1);
							//clientGrph.DrawLine(pen, clientBounds.Width-1, clientBounds.Height-1, clientBounds.Width-1, 0);
						//}
		
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

					using (Pen pen = new Pen(_groupBorderColor, 1.0f))
					{
						int l = itemsGroup.ClientBounds.X;
						int r = itemsGroup.ClientBounds.Right;
						int t = itemsGroup.ClientBounds.Y;
						int b = itemsGroup.ClientBounds.Bottom;
						
						grph.DrawLine(pen, l, t, l, b);
						grph.DrawLine(pen, l, b, r, b);
						grph.DrawLine(pen, r, b, r, t);
						grph.DrawLine(pen, r, t, l, t);
					}
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
			// TODO:  Add ExplorerBarAeroTheme.DrawExplorerBar implementation
			
			if (cache == null)
			{
				cache = new Bitmap(size.Width, size.Height);

				using (Graphics g = Graphics.FromImage(cache))
				{
					// 1. Draw background
					g.Clear(_bkColor);					

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
