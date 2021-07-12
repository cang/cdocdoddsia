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

//using SIA.SystemLayer.Semiconductor;

using SIA.IPEngine;
using SIA.IPEngine.KlarfExport;

namespace SIA.SystemLayer.ObjectExtraction.Thresholders
{
	/// <summary>
	/// Inherits from the BaseThresholder class. The StaticThresholder class implement
    /// the threhold algorihtm using minimum and maximum threshold values.
	/// </summary>
	public class StaticThresholder 
        : BaseThresholder
	{
		#region constructor and destructors
		
		public StaticThresholder()
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

		public override bool CanExtract(ObjectDetectionSettings settings)
		{
			// TODO:  Add StaticThreshold.CanExtract implementation
			if(settings == null)
				return false;

			if(settings.ThresholdType == ThresholdType.StaticThreshold)
				return true;
			return false;
		}

		public override BinaryImage Threshold(CommonImage image, ObjectDetectionSettings settings)
		{
			// TODO:  Add StaticThreshold.Threshold implementation
			BinaryImage binary_image = null;
			StaticThreshold thresholder = null;
			
			try
			{
				Progress_SetText("Threshold image ...");
				Progress_StepTo(0);

				thresholder = new StaticThreshold(image.RasterImage);
				binary_image = thresholder.TranslateBinaryImage(settings);

				//Cong: Only for testing purpose
#if TRACE_IMAGE
				SIA.SystemFrameworks.UI.CommandProgress.SetText("Saving binary image...");
				SIA.SystemFrameworks.UI.CommandProgress.StepTo(0);
				binary_image.Save(@"C:\binary_image.bmp");
#endif
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

        public BinaryImage ThresholdSupportedROI(CommonImage image, ObjectDetectionSettings settings, bool exclude)
        {
            // TODO:  Add StaticThreshold.Threshold implementation
            BinaryImage binary_image = null;
            StaticThreshold thresholder = null;

            try
            {
                Progress_SetText("Threshold image ...");
                Progress_StepTo(0);

                thresholder = new StaticThreshold(image.RasterImage);
                binary_image = thresholder.TranslateBinaryImage(settings, exclude);

                //Cong: Only for testing purpose
#if TRACE_IMAGE
				SIA.SystemFrameworks.UI.CommandProgress.SetText("Saving binary image...");
				SIA.SystemFrameworks.UI.CommandProgress.StepTo(0);
				binary_image.Save(@"C:\binary_image.bmp");
#endif
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

		public BinaryImage Threshold(CommonImage image, ObjectDetectionSettings settings, float centerX, float centerY, float radius)
		{
			BinaryImage binary_image = null;
			StaticThreshold thresholder = null;
			
			try
			{
				Progress_SetText("Threshold image ...");
				Progress_StepTo(0);

				thresholder = new StaticThreshold(image.RasterImage);
				binary_image = thresholder.TranslateBinaryImage(settings, centerX, centerY, radius);

				//Cong: Only for testing purpose
#if TRACE_IMAGE
				SIA.SystemFrameworks.UI.CommandProgress.SetText("Saving binary image...");
				SIA.SystemFrameworks.UI.CommandProgress.StepTo(0);
				binary_image.Save(@"C:\binary_image.bmp");
#endif
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
