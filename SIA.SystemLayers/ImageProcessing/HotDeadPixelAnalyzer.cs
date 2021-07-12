using System;
using System.Collections;
using System.Text;
using System.Drawing;
using System.Diagnostics;

using SIA.Common;
using SIA.IPEngine;
using SIA.SystemFrameworks;
using SIA.SystemFrameworks.UI;

namespace SIA.SystemLayer.ImageProcessing
{
	/// <summary>
	/// The HotDeadPixelAnalyzer provides functions for analyzing the hot/dead pixel occurred
    /// in images which are captured from a camera
	/// </summary>
	public unsafe class HotDeadPixelAnalyzer
	{
		public const int MaxMagicValue = 1;
	
        /// <summary>
        /// Loads hot and dead pixels from the specified image
        /// </summary>
        /// <param name="image">The input image contains the hot/dead pixels</param>
        /// <param name="hotPixels">The result list of the hot pixels</param>
        /// <param name="deadPixels">The result list of the dead pixels</param>
        public static void LoadHotDeadPixels(
            CommonImage image, ref ArrayList hotPixels, ref ArrayList deadPixels)
		{
			try
			{
				CommandProgress.SetText("Loading hot/dead pixels...");
				CommandProgress.StepTo(0);

				int width = image.Width;
				int height = image.Height;
				ushort* buffer = (ushort*)image.RasterImage.Buffer.ToPointer();

				int i=0;
				for (int y=0; y<height; y++)
				{
					for (int x=0; x<width; x++)
					{
						ushort value = buffer[i++];
						if (value == 0)
							deadPixels.Add(new Point(x, y));
						else if (value == 1)
							hotPixels.Add(new Point(x, y));
					}

					CommandProgress.StepTo(y*100/height);
				}
			}
			finally
			{
				CommandProgress.StepTo(100);
			}
		}

        /// <summary>
        /// Restores the hot/dead pixels to the specified image
        /// </summary>
        /// <param name="image">The image to restore</param>
        /// <param name="deadPixels">The list of dead pixels</param>
        /// <param name="hotPixels">The lsit of hot pixels</param>
		public static void RestoreHotDeadPixels(
            CommonImage image, ArrayList deadPixels, ArrayList hotPixels)
		{
			try
			{
				CommandProgress.SetText("Restoring dead pixels...");
				CommandProgress.StepTo(0);

				for (int i=0; i<deadPixels.Count; i++)
				{
					Point pt = (Point)deadPixels[i];
					image.SetPixel(pt.X, pt.Y, 0);

					CommandProgress.StepTo(i*100/deadPixels.Count);
				}

				CommandProgress.SetText("Restoring hot pixels...");
				CommandProgress.StepTo(0);

				for (int i=0; i<hotPixels.Count; i++)
				{
					Point pt = (Point)hotPixels[i];
					image.SetPixel(pt.X, pt.Y, 1);

					CommandProgress.StepTo(i*100/hotPixels.Count);
				}
			}
			finally
			{
				CommandProgress.StepTo(100);
			}
		}

		private enum PairIdx 
        { 
            Vertical = 0, 
            Horizontal, 
            TopLeft2BottomRight, 
            TopRight2BottomLeft, 
            Length 
        };

		private struct PixelPairInfo
		{
			public void Init()
			{
				Sum = 0;
				Diff = int.MaxValue;
				Count = 0;
			}
			public void AddValue(int value)
			{
				if ( Count > 0)
				{
					Diff = Math.Abs(Sum - value);
				}
				Sum += value;
				Count++;
			}
			public int Sum;
			public float Diff;
			public int Count;
		}
		
		private static void checkPixel(
            int x, int y, int width, int height, 
            ref CommonImage image, ref PixelPairInfo pixelPairInfo, 
				ref int minValue, ref int maxValue)
		{
			int value;
			if (x >= 0 && x < width && y >= 0 && y < height)
			{
				value = image.getPixel(x, y);
				if (value > MaxMagicValue)
				{
					pixelPairInfo.AddValue(value);
					minValue = Math.Min(minValue, value);
					maxValue = Math.Max(maxValue, value);
				}
			}

		}
		
		/// <summary>
		/// Interpolates bad pixels in an image using the neighbours
		/// </summary>
		/// <param name="image">input/output image</param>
		/// <param name="deadPixels">List of dead points (low dynamic range)</param>
		/// <param name="hotPixels">List of hot pixels (unstable offset)</param>
		public static void RemoveHotDeadPixelsAvg(
            CommonImage image, ArrayList deadPixels, ArrayList hotPixels)
		{
			try
			{
				CommandProgress.SetText("Removing hot/dead pixels...");
				CommandProgress.StepTo(0);

				int width = image.Width;
				int height = image.Height;
				ushort* buffer = image.RasterImage._aData;

				ArrayList list = new ArrayList();
				list.AddRange(deadPixels);
				list.AddRange(hotPixels);
				PixelPairInfo[] pixelPairInfo = new PixelPairInfo[(int)PairIdx.Length];

				for (int i=0; i<list.Count; i++)
				{
					Point pt = (Point)list[i];
					int counter = 0;
					int value=0;
					int totalValues = 0;
					int minValue = int.MaxValue;
					int maxValue = 0;
					
					for (int pairIdx = 0; pairIdx < (int)PairIdx.Length; pairIdx++)
					{
						pixelPairInfo[pairIdx].Init();
					}

					// top
					checkPixel(pt.X, pt.Y - 1, width, height, ref image, ref pixelPairInfo[(int)PairIdx.Vertical], ref minValue, ref maxValue);
					//bottom
					checkPixel(pt.X, pt.Y + 1, width, height, ref image, ref pixelPairInfo[(int)PairIdx.Vertical], ref minValue, ref maxValue);

					//left
					checkPixel(pt.X - 1, pt.Y, width, height, ref image, ref pixelPairInfo[(int)PairIdx.Horizontal], ref minValue, ref maxValue);
					//right
					checkPixel(pt.X + 1, pt.Y, width, height, ref image, ref pixelPairInfo[(int)PairIdx.Horizontal], ref minValue, ref maxValue);

					//top left
					checkPixel(pt.X - 1, pt.Y - 1, width, height, ref image, ref pixelPairInfo[(int)PairIdx.TopLeft2BottomRight], ref minValue, ref maxValue);
					//bottom right
					checkPixel(pt.X + 1, pt.Y + 1, width, height, ref image, ref pixelPairInfo[(int)PairIdx.TopLeft2BottomRight], ref minValue, ref maxValue);

					//top right
					checkPixel(pt.X + 1, pt.Y - 1, width, height, ref image, ref pixelPairInfo[(int)PairIdx.TopRight2BottomLeft], ref minValue, ref maxValue);
					//bottom left
					checkPixel(pt.X - 1, pt.Y + 1, width, height, ref image, ref pixelPairInfo[(int)PairIdx.TopRight2BottomLeft], ref minValue, ref maxValue);

					float weightSum = 0;
					float sum = 0;
					float maxDiff = maxValue - minValue;

					
					for( int pairIdx = 0; pairIdx < (int)PairIdx.Length; pairIdx++)
					{
						totalValues += pixelPairInfo[pairIdx].Sum;
						counter += pixelPairInfo[pairIdx].Count;
					}
					float average = (counter != 0) ? (int)(totalValues * 1.0F / counter) : 0;
					for( int pairIdx = 0; pairIdx < (int)PairIdx.Length; pairIdx++)
					{
						if (pixelPairInfo[pairIdx].Count == 2)
						{
							// we have a pair
							float pairAverage = (pixelPairInfo[pairIdx].Sum / 2);
							float weightSameEdge = Math.Abs(maxDiff - pixelPairInfo[pairIdx].Diff);		// prefer pairs having the same gray value, i.e. they lie on the same egde
							float weightInfo = Math.Abs(average - pairAverage);	// prefer pairs that differ from the average value, they contain the most information about the structure 
							float weight = weightSameEdge * weightSameEdge * weightInfo;
							sum += pairAverage * weight;
							weightSum += weight;
						}
					}

					if (weightSum == 0)
					{
						// use the average value if there are no pairs
						value = (int)average;
					}
					else
					{
						value = (int)(sum/weightSum);
					}
					value = Math.Min(ushort.MaxValue, Math.Max(value, (int)ushort.MinValue));	// limit to image range

					// set the result pixel value
					image.SetPixel(pt.X, pt.Y, (ushort)value);
				}

			}
			finally
			{
				CommandProgress.StepTo(100);
			}
		}

        /// <summary>
        /// Removes hot/dead pixel out of the specified image using median algorithm within a 3-pixel sized window
        /// </summary>
        /// <param name="image">The image to remove</param>
        /// <param name="deadPixels">The list of dead pixels</param>
        /// <param name="hotPixels">The list of hot pixels</param>
		public static void RemoveHotDeadPixelsMeadian(
            CommonImage image, ArrayList deadPixels, ArrayList hotPixels)
		{
			try
			{
				CommandProgress.SetText("Removing hot/dead pixels...");
				CommandProgress.StepTo(0);

				int width = image.Width;
				int height = image.Height;
				ushort* buffer = image.RasterImage._aData;

				ArrayList list = new ArrayList();
				list.AddRange(deadPixels);
				list.AddRange(hotPixels);

				for (int i=0; i<list.Count; i++)
				{
					Point pt = (Point)list[i];
					ArrayList values = new ArrayList();
					int value = 0, x=0, y=0;

					x = pt.X-1; y = pt.Y-1;
					if (x>=0 && x<width && y>=0 && y<height)
					{
						value = image.getPixel(x, y);
						if (value != 0 && value != 1)
							values.Add(value);
					}

					x = pt.X+0; y = pt.Y-1;
					if (x>=0 && x<width && y>=0 && y<height)
					{
						value = image.getPixel(x, y);
						if (value != 0 && value != 1)
							values.Add(value);
					}

					x = pt.X+1; y = pt.Y-1;
					if (x>=0 && x<width && y>=0 && y<height)
					{
						value = image.getPixel(x, y);
						if (value != 0 && value != 1)
							values.Add(value);
					}

					x = pt.X-1; y = pt.Y;
					if (x>=0 && x<width && y>=0 && y<height)
					{
						value = image.getPixel(x, y);
						if (value != 0 && value != 1)
							values.Add(value);
					}

					x = pt.X+1; y = pt.Y;
					if (x>=0 && x<width && y>=0 && y<height)
					{
						value = image.getPixel(x, y);
						if (value != 0 && value != 1)
							values.Add(value);
					}

					x = pt.X-1; y = pt.Y+1;
					if (x>=0 && x<width && y>=0 && y<height)
					{
						value = image.getPixel(x, y);
						if (value != 0 && value != 1)
							values.Add(value);
					}

					x = pt.X+0; y = pt.Y+1;
					if (x>=0 && x<width && y>=0 && y<height)
					{
						value = image.getPixel(x, y);
						if (value != 0 && value != 1)
							values.Add(value);
					}

					x = pt.X+1; y = pt.Y+1;
					if (x>=0 && x<width && y>=0 && y<height)
					{
						value = image.getPixel(x, y);
						if (value != 0 && value != 1)
							values.Add(value);
					}

					Debug.Assert(values.Count != 0);

					// sort the value list
					values.Sort();

					// get the median value
					value = (int)values[values.Count/2];

					image.SetPixel(pt.X, pt.Y, (ushort)value);
				}

			}
			finally
			{
				CommandProgress.StepTo(100);
			}
		}

	}
}
