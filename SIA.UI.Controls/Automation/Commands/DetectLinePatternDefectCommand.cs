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
    using DuplicatedImplement = SIA.UI.Controls.Commands;
    using SIA.Common.Mask;
    using SIA.Algorithms.FeatureProcessing.Helpers;
    using System.Drawing;
    using SiGlaz.Common.ABSDefinitions;
    using SIA.Common.Analysis;

    public class DetectLinePatternCommand : AutoCommand
    {
        private ArrayList _detectedObjects = null;
        private LinePatternLibrary _patternLib = null;

        public DetectLinePatternCommand(IProgressCallback callback)
            : base(callback)
		{
		}

        protected override void UninitClass()
        {
            base.UninitClass();

            _detectedObjects = null;
        }

        public override bool CanAbort
        {
            get { return false; }
        }

        public override object[] GetOutput()
        {
            return new object[] { _detectedObjects};
        }

		protected override void ValidateArguments(params object[] args)
		{
			if (args == null)
				throw new ArgumentNullException("arguments");
			if (args.Length < 1)
				throw new ArgumentException("Not enough arguments", "arguments");
			if (args[0] is CommonImage == false)
				throw new ArgumentException("Argument type does not match. Arguments[0] must be CommonImage", "arguments");
            //if (args[1] is LinePatternLibrary == false)
            //    throw new ArgumentException("Argument type does not match. Arguments[1] must be LinePatternLibrary", "arguments");
		}

		protected override void OnRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;
            //_patternLib = args[1] as LinePatternLibrary;

			this.SetStatusRange(0, 100);
            this.SetStatusText("Detecting Pattern Defect...");

            this.DetectPatternDefect(image);
		}

        private void Detect(CommonImage image)
        {
            throw new NotImplementedException();
        }

		public override void AutomationRun(params object[] args)
		{
            CommonImage image = args[0] as CommonImage;
            //_patternLib = args[1] as LinePatternLibrary;

            this.DetectPatternDefect(image);
        }

        protected virtual void DetectPatternDefect(CommonImage image)
		{
            BinaryImage binaryImage = null;
            try
            {
                _detectedObjects = new ArrayList();
                foreach (string filename in LinePatternLibrary.MultiplePatternFilenames)
                {
                    LinePatternLibrary settings = LinePatternLibrary.Deserialize(Path.Combine(
                        SettingsHelper.GetDefaultABSInspectionSettingsFolderPath(),
                        filename));
                    if (settings == null)
                        continue;

                    this.SetStatusText(string.Format("Classifying {0} ...", Path.GetFileNameWithoutExtension(filename)));

                    List<RectangleF> results = DuplicatedImplement.DetectLinePatternCommand.Classify(image, settings);

                    if (results == null || results.Count == 0)
                        continue;

                    double threshold = settings.Threshold;
                    binaryImage = DuplicatedImplement.DetectLinePatternCommand.GetBinary(settings, image, results);

                    ArrayList iterResult = ObjectDetection.DetectObject(image, binaryImage);
                    if (iterResult != null && iterResult.Count > 0)
                    {
                        DuplicatedImplement.DetectLinePatternCommand.UpdateDefectTypeAndFilter(iterResult, settings);
                        _detectedObjects.AddRange(iterResult);
                    }
                }

                if (_detectedObjects.Count > 0)
                {                    
                    //foreach (DetectedObject obj in _detectedObjects)
                    //{
                    //    obj.ObjectTypeId = (int)eDefectType.OverPattern;
                    //}
                }
                else
                    _detectedObjects = null;

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
