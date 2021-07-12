using System;
using System.Drawing;
using System.Data;
using System.IO;
using System.Threading;
using System.Diagnostics;

using SIA.SystemFrameworks;
using SIA.SystemFrameworks.UI;

using SIA.IPEngine;
using SIA.SystemLayer;

using SIA.UI.Controls.Utilities;

namespace SIA.UI.Controls.Commands
{
	public enum ResizeBy
	{
		Percentage,
		AbsoluteSize
	}
	/// <summary>
	/// Summary description for ResizeImageCommandSettings.
	/// </summary>
	public class ResizeImageCommandSettings : RasterCommandSettings
	{
		public int SamplingType = 0;
		public ResizeBy ResizeBy = ResizeBy.Percentage;
		public float Percentage = 100;
		public int Width = 0;
		public int Height = 0;
		public bool MaintainAspectRatio = false;

		public ResizeImageCommandSettings() : base()
		{
		}

		public ResizeImageCommandSettings(int samplingType, float percentage)
		{
			ResizeBy = ResizeBy.Percentage;
			SamplingType = samplingType;
			Percentage = percentage;
		}

		public ResizeImageCommandSettings(int samplingType, bool maintainAspectRatio, int width, int height)
		{
			ResizeBy = ResizeBy.AbsoluteSize;
			MaintainAspectRatio = maintainAspectRatio;
			Width = width;
			Height = height;
		}		

		public override void Validate()
		{
			if (ResizeBy == ResizeBy.Percentage && Percentage <= 0 || Percentage > 100)
				throw new ArgumentOutOfRangeException("Percentage", Percentage, "Percentage is out of range");
			else
			{
				if (Width == 0)
					throw new ArgumentOutOfRangeException("Width", Width, "invalid destination image's width");
				if (Height == 0)
					throw new ArgumentOutOfRangeException("Height", Height, "invalid destination image's height");
			}
		}

	}
}
