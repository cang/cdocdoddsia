/*
 * Copyright � 2005, Mathew Hall
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without modification, 
 * are permitted provided that the following conditions are met:
 *
 *    - Redistributions of source code must retain the above copyright notice, 
 *      this list of conditions and the following disclaimer.
 * 
 *    - Redistributions in binary form must reproduce the above copyright notice, 
 *      this list of conditions and the following disclaimer in the documentation 
 *      and/or other materials provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
 * IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
 * INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT 
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, 
 * OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
 * OF SUCH DAMAGE.
 */


using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using SiGlaz.UI.CustomControls.XPTable.Events;
using SiGlaz.UI.CustomControls.XPTable.Models;
using SiGlaz.UI.CustomControls.XPTable.Themes;


namespace SiGlaz.UI.CustomControls.XPTable.Renderers
{
	/// <summary>
	/// A CellRenderer that draws Cell contents as a DateTime
	/// </summary>
	public class DateTimeCellRenderer : DropDownCellRenderer
	{
		#region Class Data

		/// <summary>
		/// The format of the date and time displayed in the Cell
		/// </summary>
		private DateTimePickerFormat dateFormat;

		#endregion
		
		
		#region Constructor
		
		/// <summary>
		/// Initializes a new instance of the DateTimeCellRenderer class with 
		/// default settings
		/// </summary>
		public DateTimeCellRenderer() : base()
		{
			this.dateFormat = DateTimePickerFormat.Long;
			this.Format = DateTimeColumn.LongDateFormat;
		}

		#endregion


		#region Properties

		/// <summary>
		/// Gets or sets the format of the date and time displayed in the Cell
		/// </summary>
		public DateTimePickerFormat DateTimeFormat
		{
			get
			{
				return this.dateFormat;
			}

			set
			{
				if (!Enum.IsDefined(typeof(DateTimePickerFormat), value)) 
				{
					throw new InvalidEnumArgumentException("value", (int) value, typeof(DateTimePickerFormat));
				}
					
				this.dateFormat = value;
			}
		}

		#endregion


		#region Events

		#region Paint

		/// <summary>
		/// Raises the PaintCell event
		/// </summary>
		/// <param name="e">A PaintCellEventArgs that contains the event data</param>
		public override void OnPaintCell(PaintCellEventArgs e)
		{
			if (e.Table.ColumnModel.Columns[e.Column] is DateTimeColumn)
			{
				DateTimeColumn column = (DateTimeColumn) e.Table.ColumnModel.Columns[e.Column];

				this.DateTimeFormat = column.DateTimeFormat;
				this.Format = column.CustomDateTimeFormat;
			}
			else
			{
				this.DateTimeFormat = DateTimePickerFormat.Long;
				this.Format = "";
			}
			
			base.OnPaintCell(e);
		}


		/// <summary>
		/// Raises the Paint event
		/// </summary>
		/// <param name="e">A PaintCellEventArgs that contains the event data</param>
		protected override void OnPaint(PaintCellEventArgs e)
		{
			base.OnPaint(e);

			// don't bother going any further if the Cell is null 
			// or doesn't contain any data
			if (e.Cell == null || e.Cell.Data == null || !(e.Cell.Data is DateTime))
			{
				return;
			}

			Rectangle buttonRect = this.CalcDropDownButtonBounds();

			Rectangle textRect = this.ClientRectangle;
			
			if (this.ShowDropDownButton)
			{
				textRect.Width -= buttonRect.Width - 1;
			}

			// draw the text
			if (e.Enabled)
			{
				this.DrawText((DateTime) e.Cell.Data, e.Graphics, this.ForeBrush, textRect);
			}
			else
			{
				this.DrawText((DateTime) e.Cell.Data, e.Graphics, this.GrayTextBrush, textRect);
			}
			
			if (e.Focused && e.Enabled)
			{
				Rectangle focusRect = this.ClientRectangle;

				if (this.ShowDropDownButton)
				{
					focusRect.Width -= buttonRect.Width;
				}
				
				ControlPaint.DrawFocusRectangle(e.Graphics, focusRect);
			}
		}


		/// <summary>
		/// Draws the DateTime text
		/// </summary>
		/// <param name="dateTime">The DateTime value to be drawn</param>
		/// <param name="g">The Graphics to draw on</param>
		/// <param name="brush">The Brush to draw the text with</param>
		/// <param name="textRect">A Rectangle that specifies the bounds of the text</param>
		protected void DrawText(DateTime dateTime, Graphics g, Brush brush, Rectangle textRect)
		{
			// get the custom format
			string format = this.Format;
			
			// if a custom format hasn't been defined, use 
			// one of the default formats
			if (format.Length == 0)
			{
				switch (this.DateTimeFormat)
				{
					case DateTimePickerFormat.Long:	
						format = DateTimeColumn.LongDateFormat;
						break;

					case DateTimePickerFormat.Short:	
						format = DateTimeColumn.ShortDateFormat;
						break;

					case DateTimePickerFormat.Time:	
						format = DateTimeColumn.TimeFormat;
						break;
				}
			}

			g.DrawString(dateTime.ToString(format), this.Font, brush, textRect, this.StringFormat);
		}

		#endregion

		#endregion
	}
}
