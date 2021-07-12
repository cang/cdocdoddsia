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
using SIA.IPEngine.KlarfExport;

namespace SIA.SystemLayer.ObjectExtraction.Thresholders
{
	/// <summary>
	/// Inherits from the BaseThreshold class. The AutomaticThresholder class implements
    /// the automatic thresholding algorithm.
	/// </summary>
	public class AutomaticThresholder 
        : BaseThresholder
	{
		#region constructor and destructors
		
		public AutomaticThresholder()
		{
		}

		#endregion

		#region IObjectExtractor Members

		public override String Name
		{
			get
			{
				// TODO:  Add StaticThreshold.Name getter implementation
				return null;
			}
		}

		public override String Description
		{
			get
			{
				// TODO:  Add StaticThreshold.Description getter implementation
				return null;
			}
		}

		public override bool CanExtract(
            ObjectDetectionSettings settings)
		{
			// TODO:  Add StaticThreshold.CanExtract implementation
			if(settings == null)
				return false;

			if(settings.ThresholdType == ThresholdType.AutomaticThreshold)
				return true;
			return false;
		}

		public override BinaryImage Threshold(
            CommonImage image, ObjectDetectionSettings settings)
		{
			// TODO:  Add StaticThreshold.Threshold implementation
			BinaryImage binary_image = null;

			AutomaticThreshold thresholder = null;
			
			try
			{
				Progress_SetText("Threshold image ...");
				Progress_StepTo(0);
				
				//thresholder = new AutomaticThreshold(image.RasterImage);
				thresholder = new AutomaticThreshold();
				binary_image = 
                    thresholder.TranslateBinaryImage(image.RasterImage, settings);
			}
			catch (System.Exception exp)
			{
				throw exp;
			}
			finally
			{
				thresholder.Dispose();
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
