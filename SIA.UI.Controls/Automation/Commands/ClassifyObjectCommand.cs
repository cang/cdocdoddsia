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
    using SIA.UI.Controls.Commands;

    public class ClassifyObjectCommand : AutoCommand
    {
        private ArrayList _detectedObjects = null;
        private FeatureSpace _featureSpace = null;

        public ClassifyObjectCommand(IProgressCallback callback)
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
            this.SetStatusText("Detecting objects...");

            ObjectClassificationSettings settings = args[2] as ObjectClassificationSettings;

            if (settings.ClassifyDarkObject)
            {
                this.DetectContaminations(image);
            }

            if (settings.ClassifyBrightObject)
            {
                DetectScratchsCommand scratchsCommand = 
                    new DetectScratchsCommand(_callback);

                scratchsCommand.AutomationRun(args);

                object[] scratchs_output = scratchsCommand.GetOutput();
                if (scratchs_output != null && scratchs_output[0] != null)
                {
                    if (_detectedObjects == null)
                        _detectedObjects = new ArrayList((scratchs_output[0] as ArrayList).Count);
                    _detectedObjects.AddRange(scratchs_output[0] as ArrayList);
                }
            }

            if (settings.ClassifyDarkObjectAcrossBoundary ||
                settings.ClassifyBrightObjectAcrossBoundary)
            {
                ArrayList overPatterns = new ArrayList();

                //foreach (string filename in LinePatternLibrary.MultiplePatternFilenames)
                {
                    //LinePatternLibrary overPatternSettings = 
                    //    LinePatternLibrary.Deserialize(Path.Combine(
                    //    SettingsHelper.GetDefaultABSInspectionSettingsFolderPath(), filename));

                    LinePatternLibrary overPatternSettings = null;
                    DetectLinePatternCommand overPatterns_command = new DetectLinePatternCommand(_callback);
                    overPatterns_command.AutomationRun(new object[] { image, overPatternSettings });

                    object[] overPatterns_output = overPatterns_command.GetOutput();
                    if (overPatterns_output != null && overPatterns_output[0] != null)
                    {
                        overPatterns.AddRange(overPatterns_output[0] as ArrayList);
                    }
                }

                if (!settings.ClassifyDarkObjectAcrossBoundary)
                    SimpleFilterCommand.RemoveObjects(
                        overPatterns, (int)eDefectType.DarkObjectAcrossBoundary);

                if (!settings.ClassifyBrightObjectAcrossBoundary)
                    SimpleFilterCommand.RemoveObjects(
                        overPatterns, (int)eDefectType.BrightObjectAcrossBoundary);

                if (_detectedObjects == null)
                    _detectedObjects = new ArrayList(overPatterns.Count);
                _detectedObjects.AddRange(overPatterns);
            }
		}

		public override void AutomationRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;
            if (args[1] != null)
                _featureSpace = args[1] as FeatureSpace;

            ObjectClassificationSettings settings = args[2] as ObjectClassificationSettings;

            if (settings.ClassifyDarkObject)
            {
                this.DetectContaminations(image);
            }

            if (settings.ClassifyBrightObject)
            {
                DetectScratchsCommand scratchsCommand =
                    new DetectScratchsCommand(_callback);

                scratchsCommand.AutomationRun(args);

                object[] scratchs_output = scratchsCommand.GetOutput();
                if (scratchs_output != null && scratchs_output[0] != null)
                {
                    if (_detectedObjects == null)
                        _detectedObjects = new ArrayList((scratchs_output[0] as ArrayList).Count);
                    _detectedObjects.AddRange(scratchs_output[0] as ArrayList);
                }
            }

            if (settings.ClassifyDarkObjectAcrossBoundary ||
                settings.ClassifyBrightObjectAcrossBoundary)
            {
                ArrayList overPatterns = new ArrayList();

                //foreach (string filename in LinePatternLibrary.MultiplePatternFilenames)
                {
                    //LinePatternLibrary overPatternSettings =
                    //    LinePatternLibrary.Deserialize(Path.Combine(
                    //    SettingsHelper.GetDefaultABSInspectionSettingsFolderPath(), filename));

                    LinePatternLibrary overPatternSettings = null;

                    DetectLinePatternCommand overPatterns_command = new DetectLinePatternCommand(_callback);
                    overPatterns_command.AutomationRun(new object[] { image, overPatternSettings });

                    object[] overPatterns_output = overPatterns_command.GetOutput();
                    if (overPatterns_output != null && overPatterns_output[0] != null)
                    {
                        overPatterns.AddRange(overPatterns_output[0] as ArrayList);
                    }
                }

                if (!settings.ClassifyDarkObjectAcrossBoundary)
                    SimpleFilterCommand.RemoveObjects(
                        overPatterns, (int)eDefectType.DarkObjectAcrossBoundary);

                if (!settings.ClassifyBrightObjectAcrossBoundary)
                    SimpleFilterCommand.RemoveObjects(
                        overPatterns, (int)eDefectType.BrightObjectAcrossBoundary);

                if (_detectedObjects == null)
                    _detectedObjects = new ArrayList(overPatterns.Count);
                _detectedObjects.AddRange(overPatterns);
            }


            //this.DetectContaminations(image);
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

                ArrayList detectedObjects = ObjectDetection.DetectObject(image, binaryImage);
                ObjectDetection.UpdateObjectTypeId(detectedObjects, (int)eDefectType.DarkObject);

                if (_detectedObjects == null)
                {
                    _detectedObjects = new ArrayList(
                        (detectedObjects == null ? 0 : detectedObjects.Count));
                    _detectedObjects.AddRange(detectedObjects);
                }                
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
