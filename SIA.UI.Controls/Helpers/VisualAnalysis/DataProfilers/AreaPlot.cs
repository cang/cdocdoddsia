using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Threading;
using System.Text;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using SIA.Common;
using SIA.Common.Analysis;
using SIA.SystemLayer;

using SIA.UI.Controls;
using SIA.UI.Controls.Common;
//CONG using SIA.UI.Controls.Components;
using SIA.UI.Controls.UserControls;
using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.Helpers;
using SIA.UI.Controls.Helpers.VisualAnalysis;
using SIA.UI.Controls.Utilities;

using SIA.UI.Components;
using SIA.UI.Components.Helpers;

namespace SIA.UI.Controls.Helpers.VisualAnalysis.DataProfilers
{
	/// <summary>
	/// Summary description for AreaPlot.
	/// </summary>
	public unsafe class AreaPlot : BoxProfile
	{
		private Thread _workerThread = null;
        private AreaPlotSettings _settings = new AreaPlotSettings();
        private PointF _pt1 = PointF.Empty, _pt2 = PointF.Empty;

		public AreaPlot(DataProfileHelper container)
			: base(container)
		{
		}

		public override void DisplaySettingsWindow()
		{
			using (DlgAreaPlotSettings dlg = new DlgAreaPlotSettings(this._settings))
			{
				if (dlg.ShowDialog(this.Container.DlgLineProfile) == DialogResult.OK)
				{
					this._settings = dlg.Settings;

					this.UpdateDataProfile(this._begin, this._end);
				}
			}
		}

		protected override void UpdateDataProfile(PointF begin, PointF end)
        {
            // save working points
            _pt1 = begin; _pt2 = end;

			if (this._workerThread != null)
			{
				this._workerThread.Abort();
				this._workerThread.Join();
			}

			lock (this)
			{
				this._workerThread = new Thread(new ThreadStart(WorkerThread));
				this._workerThread.Start();
			}
		}

		protected override void Export(string filePath)
		{
			using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
			{
				int width=0,height=0;
				float[] data = null;

                float left = Math.Min(_pt1.X, _pt2.X);
                float top = Math.Min(_pt1.Y, _pt2.Y);
                float right = Math.Max(_pt1.X, _pt2.X);
                float bottom = Math.Max(_pt1.Y, _pt2.Y);
			
				RectangleF rect = new RectangleF(left, top, right-left+1, bottom-top+1);
				
				this.SamplingData(rect, ref width, ref height, ref data);

				int x1 = (int)Math.Floor(rect.Left), y1 = (int)Math.Floor(rect.Top);
				int x2 = (int)Math.Floor(rect.Right), y2 = (int)Math.Floor(rect.Bottom);
				int index=0;

				for (int y=y1; y<=y2; y++)
				{
					for (int x=x1; x<=x2; x++)
					{
						writer.Write(String.Format("{0},", data[index++]));
					}					
					writer.WriteLine("");
				}
			}
		}


		private void WorkerThread()
		{
			try
			{
				int width=0,height=0;
				float[] data = null;

                float left = Math.Min(_pt1.X, _pt2.X);
                float top = Math.Min(_pt1.Y, _pt2.Y);
                float right = Math.Max(_pt1.X, _pt2.X);
                float bottom = Math.Max(_pt1.Y, _pt2.Y);
			
				RectangleF rect = new RectangleF(left, top, right-left+1, bottom-top+1);
				
				this.SamplingData(rect, ref width, ref height, ref data);

				int x1 = (int)Math.Floor(rect.Left), y1 = (int)Math.Floor(rect.Top);
				int x2 = (int)Math.Floor(rect.Right), y2 = (int)Math.Floor(rect.Bottom);

				this.DlgLineProfile.UpdateAreaPlot(this._settings.RenderMode, this._settings.XRes, this._settings.YRes, 
					x1, y1, x2, y2, data);
			}
			catch
			{
			}
			finally
			{
				lock (this)
					_workerThread = null;
			}
		}

		private void SamplingData(RectangleF rect, ref int width, ref int height, ref float[] data)
		{
			CommonImage image = this.Image;
			int x1 = (int)Math.Floor(rect.Left), y1 = (int)Math.Floor(rect.Top);
			int x2 = (int)Math.Floor(rect.Right), y2 = (int)Math.Floor(rect.Bottom);

			width = x2-x1+1;
			height = y2-y1+1;
			int length = width*height;
			int imageWidth = image.Width;
			int imageHeight = image.Height;
			ushort* buffer = (ushort*)image.RasterImage.Buffer.ToPointer();
			
			data = new float[length];
			
			for (int y=0; y<height; y++)
			{
				for (int x=0; x<width; x++)
				{
					float value = 0;
					int xPos = x1+x, yPos = y1+y;
					if (xPos>=0 && xPos<imageWidth && yPos>=0 && yPos<imageHeight)
						value = buffer[yPos*imageWidth + xPos];
					data[y*width+x] = value;
				}
			}

			if (this._settings != null)
				this._settings.UpdateDataBySettings(data);
		}

	}
}
