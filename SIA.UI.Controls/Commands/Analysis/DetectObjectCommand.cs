using System;
using System.Collections;
using System.Drawing;
using System.Data;
using System.IO;
using System.Threading;
using System.Diagnostics;

using SIA.Common.KlarfExport;
using SIA.SystemFrameworks;
using SIA.SystemFrameworks.UI;
using SIA.IPEngine;
using SIA.SystemLayer;
using SIA.SystemLayer.ObjectExtraction;

using SIA.UI.Controls.Utilities;

namespace SIA.UI.Controls.Commands
{

	/// <summary>
	/// Summary description for DetectObjectCommand.
	/// </summary>
	public class DetectObjectCommand : RasterCommand
	{
		private ArrayList _detectedObjects = null;

		public DetectObjectCommand(IProgressCallback callback) : base(callback)
		{
		}

		protected override void UninitClass()
		{
			base.UninitClass ();

			_detectedObjects = null;
		}

		public override bool CanAbort
		{
			get { return true; }
		}


		protected override void ValidateArguments(params object[] args)
		{
			if (args == null)
				throw new ArgumentNullException("arguments");
			if (args.Length < 2)
				throw new ArgumentException("Not enough arguments", "arguments");
			if (args[0] is CommonImage == false)
				throw new ArgumentException("Argument type does not match. Arguments[0] must be CommonImage", "arguments");
			if (args[1] is ObjectDetectionSettings == false)
				throw new ArgumentException("Argument type does not match. Arguments[1] must be ObjectDetectionSettings", "arguments");
		}

		public override object[] GetOutput()
		{
			return new object[] {_detectedObjects};
		}

		protected override void OnRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;
			ObjectDetectionSettings settings = (ObjectDetectionSettings)args[1];
			
			this.SetStatusRange(0, 100);
			this.SetStatusText("Detecting objects...");

			this.DetectObjects(image, settings);

            this.SetStatusValue(100);
		}

		protected virtual void DetectObjects(CommonImage image, ObjectDetectionSettings settings)
		{
            using (CommonImage work_image = image.Clone() as CommonImage)
                _detectedObjects = ObjectExtractor.ExtractObjects(work_image, settings);
		}
	}
}
