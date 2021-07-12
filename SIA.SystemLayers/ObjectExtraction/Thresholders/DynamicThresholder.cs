//#define _LENS_CORRECTION_ENABLED
using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Diagnostics;

using SIA.Common;
using SIA.Common.Analysis;
using SIA.Common.Imaging;
using SIA.Common.Mask;
using SIA.Common.KlarfExport;
using SiGlaz.RDE.Ex.Mask;
using SIA.SystemFrameworks;
using SIA.SystemLayer;
using SIA.SystemLayer.Mask;
using SIA.IPEngine;

namespace SIA.SystemLayer.ObjectExtraction.Thresholders
{
	/// <summary>
	/// Inherits from the BaseThresholder class. The DynamicThresholder class implements
    /// the dynamic threshold algorithm.
	/// </summary>
	public class DynamicThresholder : BaseThresholder
	{
		#region constructor and destructors
		
		public DynamicThresholder() : base()
		{
		}

		#endregion

		#region IObjectExtractor Members

		public override String Name
		{
			get
			{
				return "Dynamic Thresholder";
			}
		}

		public override String Description
		{
			get
			{
				return "Translate image into black and white image using trendline method";
			}
		}

		public override bool CanExtract(ObjectDetectionSettings settings)
		{
			return settings.ThresholdType == ThresholdType.DynamicThreshold;
		}

		public override BinaryImage Threshold(CommonImage image, ObjectDetectionSettings settings)
		{
			BinaryImage binary_image = null;

			try
			{
				this.Progress_SetText("Thresholding image...");
				this.Progress_StepTo(0);

				using (SIA.IPEngine.KlarfExport.DynamicThreshold thresholder = new SIA.IPEngine.KlarfExport.DynamicThreshold())
					binary_image = thresholder.TranslateBinaryImage(image.RasterImage, settings);

			}
			catch(System.Exception exp)
			{
				if (binary_image != null)
					binary_image.Dispose();
				binary_image = null;

				throw exp;
			}
			finally
			{
			}

			return binary_image;
		}

		#endregion

		#region IDisposable Members

		public override void Dispose()
		{
			// TODO:  Add StaticThreshold.Dispose implementation
		}

		#endregion

	}
}
