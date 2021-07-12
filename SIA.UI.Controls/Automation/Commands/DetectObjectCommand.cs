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

using SIA.UI.Controls.Commands;

namespace SIA.UI.Controls.Automation.Commands
{

	/// <summary>
	/// Summary description for DetectObjectCommand.
	/// </summary>
	public class DetectObjectCommand : AutoCommand
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
			if (args[1] is DetectObjectCommandSettings == false)
				throw new ArgumentException("Argument type does not match. Arguments[1] must be DetectObjectCommandSettings", "arguments");
		}

		public override object[] GetOutput()
		{
			return new object[] {_detectedObjects};
		}

		protected override void OnRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;

			DetectObjectCommandSettings cmdSettings = (DetectObjectCommandSettings)args[1];
			ObjectDetectionSettings settings = cmdSettings.ObjectDetectionSettings;
			
			this.SetStatusRange(0, 100);
			this.SetStatusText("Detecting objects...");

			this.DetectObjects(image, settings);			
		}

		public override void AutomationRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;

			DetectObjectCommandSettings cmdSettings = (DetectObjectCommandSettings)args[1];
			ObjectDetectionSettings settings = cmdSettings.ObjectDetectionSettings;
			
			this.DetectObjects(image, settings);			
		}

		protected virtual void DetectObjects(CommonImage image, ObjectDetectionSettings settings)
		{
            using (CommandProgressLocker locker = new CommandProgressLocker())
            {
                // update settings
                settings.UseMaskData = true;

                using (CommonImage work_img = image.Clone() as CommonImage)
                    _detectedObjects = ObjectExtractor.ExtractObjects(work_img, settings);
            }
		}
	}
}
