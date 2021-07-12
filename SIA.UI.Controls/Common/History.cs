using System;
using System.IO;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.Diagnostics;

using SIA.SystemFrameworks;
using SIA.SystemLayer;

namespace SIA.UI.Controls.Common
{
	/// <summary>
	/// Summary description for History.
	/// </summary>
	public class History : IDisposable
	{
		public static Size defaultThumbSize = new Size(48,48);

		private String _fileName = String.Empty;
		private Image _thumbnail = null;
		private String _description = String.Empty;

		public String FileName
		{
			get {return _fileName;}
		}

		public Image Thumbnail
		{
			get {return _thumbnail;}
		}

		public String Description
		{
			get {return _description;}
		}

		public History(String fileName, String description, Image thumbnail)
		{
			_fileName = fileName;
			_description = description;
			_thumbnail = thumbnail;
		}

		public History(string fileName, CommonImage image)
		{
			// save file name for later use
			_fileName = fileName;
			// save image description
			_description = image.Description;
			
			// creates thumbnails for later use
			float scaleDx = defaultThumbSize.Width / (float)image.Width;
			float scaleDy = defaultThumbSize.Height / (float)image.Height;
			float scaleFactor = Math.Min(scaleDx, scaleDy);
			
			int thumbWidth = (int)Math.Ceiling(scaleFactor*image.Width);
			int thumbHeight = (int)Math.Ceiling(scaleFactor*image.Height);
            if (thumbWidth > 0 && thumbHeight > 0)
            {
                using (CommonImage imgScaled = image.CreateScaledImage(thumbWidth, thumbHeight))
                {
                    using (Bitmap bmpScaled = imgScaled.CreateBitmap())
                    {
                        _thumbnail = new Bitmap(defaultThumbSize.Width, defaultThumbSize.Height, PixelFormat.Format24bppRgb);
                        using (Graphics graph = Graphics.FromImage(_thumbnail))
                        {
                            float xPos = (defaultThumbSize.Width - thumbWidth) * 0.5F;
                            float yPos = (defaultThumbSize.Height - thumbHeight) * 0.5F;
                            graph.Clear(Color.Black);
                            
                            if (bmpScaled != null)
                                graph.DrawImage(bmpScaled, xPos, yPos);
                        }
                    }
                }
            }
            else
            {
                this._thumbnail = new Bitmap(defaultThumbSize.Width, defaultThumbSize.Height, PixelFormat.Format24bppRgb);
            }
			
			// saving data into file
			this.Save(image);
		}

		~History()
		{
			this.Dispose(false);
		}

		#region IDisposable Members

		public void Dispose()
		{
			this.Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (this._thumbnail != null)
				this._thumbnail.Dispose();
			this._thumbnail = null;
		}

		#endregion

		public CommonImage Load()
		{
			// create a temporary image
			CommonImage image = new CommonImage();
			
			// quick load data from file
			bool bResult = image.QuickLoad(_fileName);

			return bResult ? image : null;
		}

		public bool Save(CommonImage image)
		{
			// validate parameters
			if (image == null)
				throw new System.ArgumentNullException("Invalid image parameter");
			
			// quick save data to file
			bool bResult = image.QuickSave(_fileName);
			
			return bResult;
		}
		
	}
}
