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
    using System.Drawing;
    using SiGlaz.Common.ABSDefinitions;
    using SIA.Common.Analysis;

    public class DetectScratchsCommand : AutoCommand
    {
        private ArrayList _detectedObjects = null;
        private FeatureSpace _featureSpace = null;

        public DetectScratchsCommand(IProgressCallback callback)
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
            this.SetStatusText("Detecting scratchs...");

            this.DetectScratchs(image);
		}

		public override void AutomationRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;
            if (args[1] != null)
                _featureSpace = args[1] as FeatureSpace;

            this.SetStatusRange(0, 100);
            this.SetStatusText("Detecting objects...");

            this.DetectScratchs(image);
		}

        protected virtual void DetectScratchs(CommonImage image)
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
                    "ABS_Scratch_NNModel.nns");

                this.SetStatusText("Recognizing...");
                int[] xList = null, yList = null;
                duplicatedImplement.DetectScratchsCommand.Recognize(
                    nnsfile, featureSpace, ref xList, ref yList);
                if (xList.Length == 0)
                    return;

                string detectedObjectFileSettings =
                    Path.Combine(
                    SettingsHelper.GetDefaultABSInspectionSettingsFolderPath(),
                    ContaminationsDetectionSettings.DefaultFileName);
                ContaminationsDetectionSettings settings =
                    ContaminationsDetectionSettings.Deserialize(detectedObjectFileSettings);
                double threshold = settings.HigherIntensityThreshold;
                int sampleSize = settings.SampleSize;
                Rectangle poleRect =
                    new Rectangle(
                    settings.PoleX, settings.PoleY,
                    settings.PoleWidth, settings.PoleHeight);

                binaryImage = duplicatedImplement.DetectScratchsCommand.Binarize(
                    image, mask, xList, yList, threshold, sampleSize, poleRect);

                _detectedObjects = 
                    ObjectDetection.DetectObject(
                    image, binaryImage);

                _detectedObjects = 
                    duplicatedImplement.DetectScratchsCommand.UpdateDefectTypeAndFilter(_detectedObjects);
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
