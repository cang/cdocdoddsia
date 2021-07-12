using System;
using System.Collections.Generic;
using System.Text;
using SIA.SystemFrameworks.UI;
using SIA.SystemLayer;
using System.Collections;
using SIA.Algorithms.FeatureProcessing;
using System.IO;
using SiGlaz.Common;
using SiGlaz.Common.ABSInspectionSettings;
using SIA.IPEngine;

namespace SIA.UI.Controls.Automation.Commands
{
    using duplicatedImplement = SIA.UI.Controls.Commands;
    using SIA.Common.Mask;
    using SIA.Algorithms.FeatureProcessing.Helpers;
    using SiGlaz.Common.ABSDefinitions;
    using SIA.Common.Analysis;

    public class DetectContaminationsCommand : AutoCommand
    {
        private ArrayList _detectedObjects = null;
        private FeatureSpace _featureSpace = null;

        public DetectContaminationsCommand(IProgressCallback callback)
            : base(callback)
		{
		}

        protected override void UninitClass()
        {
            base.UninitClass();

            _detectedObjects = null;
            _featureSpace = null;
        }

        public override bool CanAbort
        {
            get { return false; }
        }

        public override object[] GetOutput()
        {
            return new object[] { _detectedObjects, _featureSpace };
        }

		protected override void ValidateArguments(params object[] args)
		{
			if (args == null)
				throw new ArgumentNullException("arguments");
			if (args.Length < 1)
				throw new ArgumentException("Not enough arguments", "arguments");
			if (args[0] is CommonImage == false)
				throw new ArgumentException("Argument type does not match. Arguments[0] must be CommonImage", "arguments");
		}

		protected override void OnRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;
            if (args[1] != null)
                _featureSpace = args[1] as FeatureSpace;


			this.SetStatusRange(0, 100);
            this.SetStatusText("Detecting contaminations...");

            this.DetectContaminations(image);
		}

		public override void AutomationRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;
            if (args[1] != null)
                _featureSpace = args[1] as FeatureSpace;

            this.DetectContaminations(image);
		}

        protected virtual void DetectContaminations(CommonImage image)
		{
            BinaryImage binaryImage = null;
			try
			{
                IMask mask = ContaminationTexturalInfoHelper.Mask;

                if (_featureSpace == null)
                {
                    using (ExtractFeatureCommand command = new ExtractFeatureCommand(null))
                    {
                        command.AutomationRun(new object[] { image });
                        object[] output = command.GetOutput();
                        if (output == null || output.Length == 0 ||
                            output[0] == null || output[0].GetType() != typeof(FeatureSpace))
                        {
                            _featureSpace = null;
                        }
                        else
                        {
                            _featureSpace = output[0] as FeatureSpace;
                        }
                    }
                }
                else if (_featureSpace.Features == null || _featureSpace.Features.Count == 0)
                    return;

                if (_featureSpace == null)
                    return;

                FeatureSpace featureSpace = _featureSpace;

                string nnsfile = Path.Combine(
                    SettingsHelper.GetDefaultABSInspectionSettingsFolderPath(),
                    "ABS_Contamination_NNModel.nns");

                this.SetStatusText("Recognizing...");
                int[] xList = null, yList = null;
                duplicatedImplement.DetectContaminationsCommand.Recognize(
                    nnsfile, featureSpace, featureSpace.ContaminationMask, 1, ref xList, ref yList);
                if (xList.Length == 0)
                    return;

                string detectedObjectFileSettings =
                    Path.Combine(
                    SettingsHelper.GetDefaultABSInspectionSettingsFolderPath(),
                    ContaminationsDetectionSettings.DefaultFileName);
                ContaminationsDetectionSettings settings =
                    ContaminationsDetectionSettings.Deserialize(detectedObjectFileSettings);
                double threshold = settings.LowerIntensityThreshold;

                binaryImage = 
                    duplicatedImplement.DetectContaminationsCommand.Binarize(
                    image, mask, xList, yList, threshold);

                _detectedObjects = ObjectDetection.DetectObject(image, binaryImage);
                ObjectDetection.UpdateObjectTypeId(_detectedObjects, (int)eDefectType.DarkObject);
			}
			catch
			{
				throw;
			}
			finally
			{
                if (binaryImage != null)
                {
                    binaryImage.Dispose();
                    binaryImage = null;
                }
			}
		}
    }
}
