using System;
using System.Collections.Generic;
using System.Text;
using SIA.SystemFrameworks.UI;
using SIA.SystemLayer;
using System.Collections;
using SIA.Algorithms.FeatureProcessing;

namespace SIA.UI.Controls.Automation.Commands
{
    public class ExtractFeatureCommand : AutoCommand
    {
        private FeatureSpace _featureSpace = null;

        public ExtractFeatureCommand(IProgressCallback callback)
            : base(callback)
		{
		}

        protected override void UninitClass()
        {
            base.UninitClass();

            _featureSpace = null;
        }

        public override bool CanAbort
        {
            get { return false; }
        }

        public override object[] GetOutput()
        {
            return new object[] { _featureSpace };
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

			this.SetStatusRange(0, 100);
            this.SetStatusText("Extracting features...");

            this.ExtractFeatures(image);
		}

		public override void AutomationRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;
            this.SetStatusText("Extracting features...");

            this.ExtractFeatures(image);
		}

        protected virtual void ExtractFeatures(CommonImage image)
		{
			try
			{
                _featureSpace = 
                    FeatureExtractor.ExtractFeatures_For_ABSContaminationsDetection(image);
			}
			catch
			{
				throw;
			}
			finally
			{
			}
		}
    }
}
