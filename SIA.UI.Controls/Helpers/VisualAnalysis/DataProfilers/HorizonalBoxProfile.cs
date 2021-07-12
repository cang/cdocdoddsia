using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Threading;

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
	/// Summary description for HorizontalBoxProfile.
	/// </summary>
	public unsafe class HorizontalBoxProfile : BoxProfile
	{
		private object _syncObject = new object();
		private Thread _workerThread = null;
        private float _selectedX = float.NaN;
        private PointF _pt1 = PointF.Empty, _pt2 = PointF.Empty;

		public HorizontalBoxProfile(DataProfileHelper container)
			: base(container)
		{
		}

		public override void Render(Graphics graph, Rectangle rcClip)
		{
			base.Render(graph, rcClip);

			ImageWorkspace workspace = this.Workspace;
			ImageViewer viewer = workspace.ImageViewer;
			Transformer transformer = new Transformer(viewer.Transform);

			// render selected value
			if (float.IsNaN(_selectedX) == false)
			{
				PointF pt1 = new PointF( _selectedX, this.Rectangle.Top);
				PointF pt2 = new PointF( _selectedX, this.Rectangle.Bottom);
				pt1 = transformer.PointToPhysical(pt1);
				pt2 = transformer.PointToPhysical(pt2);
				graph.DrawLine(Pens.Aqua, pt1, pt2);
			}
		}

		public override void UpdateSelectedValue(object abscissaValue, object ordinaryValue)
		{
            try
            {
                _selectedX = (float)abscissaValue;
                this.Workspace.Invalidate(true);
            }
            catch
            {
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

			lock(_syncObject)
			{
                WorkerThread();

                //this._workerThread = new Thread(new ThreadStart(WorkerThread));
                //this._workerThread.Start();
			}
		}

		public override void DisplaySettingsWindow()
		{
			using (DlgLineProfileSettings dlg = new DlgLineProfileSettings(this.Settings))
			{
				if (DialogResult.OK == dlg.ShowDialog(this.Container.DlgLineProfile))
				{
					// update internal settings
					this.Settings = (BoxProfileSettings)dlg.Settings;
					// update profile data
					this.UpdateDataProfile(this._begin, this._end);
				}
			}
		}

		private void WorkerThread()
		{
			try
			{
				float[] data = null, categories = null;
				
				float left = Math.Min(_pt1.X, _pt2.X);
				float top = Math.Min(_pt1.Y, _pt2.Y);
				float right = Math.Max(_pt1.X, _pt2.X);
				float bottom = Math.Max(_pt1.Y, _pt2.Y);
			
				RectangleF rect = new RectangleF(left, top, right-left+1, bottom-top+1);
				BoxProfileOptions options = this.DlgLineProfile.BoxProfileOptions;

				this.SamplingData(options, rect, ref data, ref categories);

				this.DlgLineProfile.UpdateBoxPlot(this.Settings, rect, data, categories);
			}
			catch
			{
			}
		}

		protected override void SamplingData(BoxProfileOptions options, RectangleF rect, ref float[] data, ref float[] categories)
		{
			CommonImage image = this.Image;
			int x1 = (int)Math.Floor(rect.Left), y1 = (int)Math.Floor(rect.Top);
			int x2 = (int)Math.Floor(rect.Right), y2 = (int)Math.Floor(rect.Bottom);

			int width = x2-x1+1;
			int height = y2-y1+1;
			int length = width*height;
			int imageWidth = image.Width;
			int imageHeight = image.Height;
			ushort* buffer = (ushort*)image.RasterImage.Buffer.ToPointer();
			
			data = new float[width];
			categories = new float[width];

			if (options == BoxProfileOptions.Maximum)
			{
				for (int x=0; x<width; x++)
				{
					float value = 0;
					for (int y=0; y<height; y++)
					{
						int xPos = x1+x, yPos = y1+y;
						if (xPos>=0 && xPos<imageWidth &&
							yPos>=0 && yPos<imageHeight)
							value = Math.Max(value, buffer[yPos*imageWidth + xPos]);
					}

					data[x] = value;
					categories[x] = x;
				}
			}
			else if (options == BoxProfileOptions.Minimum)
			{
				for (int x=0; x<width; x++)
				{
					float value = float.MaxValue;
					for (int y=0; y<height; y++)
					{
						int xPos = x1+x, yPos = y1+y;
						if (xPos>=0 && xPos<imageWidth && yPos>=0 && yPos<imageHeight)
							value = Math.Min(value, buffer[yPos*imageWidth + xPos]);
						else
							value = 0;
					}

					data[x] = value;
					categories[x] = x;
				}
			}
			else if (options == BoxProfileOptions.Mean)
			{
				for (int x=0; x<width; x++)
				{
					int num = 0;
					float value = 0;
					for (int y=0; y<height; y++)
					{
						int xPos = x1+x, yPos = y1+y;
						if (xPos>=0 && xPos<imageWidth && yPos>=0 && yPos<imageHeight)
						{
							value += buffer[yPos*imageWidth + xPos];
							num++;
						}
					}

					data[x] = num > 0 ? value/num : 0;
					categories[x] = x;
				}
			}
			else if (options == BoxProfileOptions.StdDev)
			{
				for (int x=0; x<width; x++)
				{
					int num = 0;
					float mean = 0;
					for (int y=0; y<height; y++)
					{
						int xPos = x1+x, yPos = y1+y;
						if (xPos>=0 && xPos<imageWidth && yPos>=0 && yPos<imageHeight)
						{
							mean += buffer[yPos*imageWidth + xPos];
							num++;
						}
					}

					mean = num > 0 ? mean/num : 0;
					float stddev = 0;

					for (int y=0; y<height; y++)
					{
						int xPos = x1+x, yPos = y1+y;
						if (xPos>=0 && xPos<imageWidth && yPos>=0 && yPos<imageHeight)
						{
							float value = buffer[yPos*imageWidth + xPos];
							stddev += (value - mean)*(value - mean);
						}
					}

                    data[x] = num > 0 ? (float)Math.Sqrt(stddev / (num > 1 ? (num-1) : num) ) : 0;
					categories[x] = x;
				}
			}

			if (this.Settings != null)
				this.Settings.UpdateDataBySettings(data);
		}
	}
}
