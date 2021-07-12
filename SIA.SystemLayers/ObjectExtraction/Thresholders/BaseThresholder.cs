using System;

using SIA.Common.KlarfExport;
using SIA.IPEngine;

namespace SIA.SystemLayer.ObjectExtraction.Thresholders
{
	/// <summary>
	/// Abstract class provides basic implementation of the IThresholder interface
	/// </summary>
	public abstract class BaseThresholder 
        : IThresholder
	{
		#region constructor and destructor
		
		public BaseThresholder()
		{
		}

		#endregion

		#region IThresholder Members

		public abstract String Name {get;} 

		public abstract String Description {get;}

		public abstract bool CanExtract(ObjectDetectionSettings settings);

		public abstract BinaryImage Threshold(CommonImage image, ObjectDetectionSettings settings);

		#endregion

		#region IDisposable Members

		public virtual void Dispose()
		{			
		}

		#endregion

		#region ProgressBar helper

		protected void Progress_SetText(String text)
		{
			SIA.SystemFrameworks.UI.CommandProgress.SetText(text);
		}

		protected void Progress_StepTo(int value)
		{
            SIA.SystemFrameworks.UI.CommandProgress.StepTo(value);
		}

		#endregion
	}
}
