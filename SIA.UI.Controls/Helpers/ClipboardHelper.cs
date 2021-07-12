using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Windows.Forms;

using SIA.UI.Controls;
using SIA.UI.Controls.Common;
using SIA.UI.Controls.Utilities;

using SIA.Common;
using SIA.SystemFrameworks;
using SIA.SystemFrameworks.UI;
using SIA.IPEngine;
using SIA.SystemLayer;
using SIA.Plugins.Common;

namespace SIA.UI.Controls.Helpers
{
	/// <summary>
	/// Summary description for ClipboardHelper.
	/// </summary>
	public class ClipboardHelper
	{
		public static bool CanCopyToClipboard(SIA.UI.Controls.ImageWorkspace owner)
		{
			return owner != null && owner.Image != null;
		}

		public static bool CanPasteFromClipboard(SIA.UI.Controls.ImageWorkspace owner)
		{
			IDataObject dataObject = Clipboard.GetDataObject();
			bool bValidFormat = false;
			try
			{
				bValidFormat = dataObject.GetDataPresent(typeof(Bitmap));
			}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);
			}
			
			return owner != null && bValidFormat;
		}

		public static void CopyToClipboard(SIA.UI.Controls.ImageWorkspace owner)
		{
			try
			{
				if (ClipboardHelper.CanCopyToClipboard(owner))
				{
					SIA.SystemLayer.CommonImage image = owner.Image;
					Image bitmap = image.CreateBitmap();
					Clipboard.SetDataObject(bitmap);
				}
			}
			catch(System.Exception exp)
			{
				MessageBoxEx.Error("Failed to copy data from clipboard");
				Trace.WriteLine(exp);
			}
			finally
			{
			}
		} 

		public static void PasteFromClipboard(ImageWorkspace owner)
		{
			try
			{
				if (ClipboardHelper.CanPasteFromClipboard(owner))
				{
					SIA.SystemLayer.CommonImage image = owner.Image;
					IDataObject obj = Clipboard.GetDataObject();
					if (obj.GetDataPresent(typeof(Bitmap)) == true)
					{
						Bitmap bitmap = (Bitmap) obj.GetData(typeof(Bitmap));
						if (bitmap != null)
						{
							IProgressCallback callback = null;

							try
							{
								callback = owner.BeginProcess("Paste from clipboard", true, ProgressType.AutoTick);
								GreyDataImage data_buffer = GreyDataImage.FromImage(bitmap);
								if (image == null)
								{
									image = new CommonImage(data_buffer, 1);
									owner.CreateWorkspace(image);
								}
								else								
									image.RasterImage.SetImageData(data_buffer.Width, data_buffer.Height, data_buffer.Buffer);

							}
							catch(System.Exception exp)
							{
								owner.AbortProcess(callback);

								MessageBoxEx.Error("Failed to retrieve data from clipboard");
								Trace.WriteLine(exp);
							}
							finally
							{
								owner.EndProcess(callback);
							}
						}
						else if (obj is IRasterImage)
						{
							try
							{
							}
							catch(System.Exception exp)
							{
								Trace.WriteLine(exp);
							}
							finally
							{
							}
						}
					}					
				}
			}
			catch(System.Exception exp)
			{
				MessageBoxEx.Error("Failed to paste data from clipboard");
				Trace.WriteLine(exp);
			}
			finally
			{
			}
		}
	}
}
