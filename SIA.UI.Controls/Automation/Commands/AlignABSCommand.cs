using System;
using System.Collections.Generic;
using System.Text;
using SIA.SystemFrameworks.UI;
using SIA.SystemLayer;

using SIA.Algorithms.Preprocessing.Alignment;
using SIA.Algorithms.Preprocessing.Interpolation;
using SIA.IPEngine;
using SiGlaz.Common.ImageAlignment;

namespace SIA.UI.Controls.Automation.Commands
{
    public class AlignABSCommand : AutoCommand
    {
        private GreyDataImage _result = null;
        private GreyDataImage _image = null;
        private Settings _settings = null;
        private System.Drawing.Drawing2D.Matrix _inverseTransform;

        public AlignABSCommand(IProgressCallback callback)
            : base(callback)
		{
		}

        protected override void UninitClass()
        {
            base.UninitClass();

            _result = null;
        }

        public override bool CanAbort
        {
            get { return false; }
        }

        public override object[] GetOutput()
        {
            return new object[] { _result, _inverseTransform };
        }

		protected override void ValidateArguments(params object[] args)
		{
            if (args == null || args.Length < 2)
                throw new ArgumentException("Invalid arguments");
            if (args[0] == null || !(args[0] is SIA.SystemLayer.CommonImage))
                throw new ArgumentException("First argument must be CommonImage");
            if (args[1] == null || !(args[1] is Settings))
                throw new ArgumentException("Second argument must be Settings");

            _image = ((SIA.SystemLayer.CommonImage)args[0]).RasterImage;
            _settings = (SiGlaz.Common.ImageAlignment.Settings)args[1];
        }

		protected override void OnRun(params object[] args)
		{
            _image = ((SIA.SystemLayer.CommonImage)args[0]).RasterImage;
            _settings = (SiGlaz.Common.ImageAlignment.Settings)args[1];

			this.SetStatusRange(0, 100);
            this.SetStatusText("Performing alignment Air Bearing Surface...");

            this.DoAlignment();
		}

		public override void AutomationRun(params object[] args)
		{
            _image = ((SIA.SystemLayer.CommonImage)args[0]).RasterImage;
            _settings = (SiGlaz.Common.ImageAlignment.Settings)args[1];

            this.DoAlignment();
		}

		protected virtual void DoAlignment()
		{
			try
			{
                Aligner aligner = new Aligner(_settings);
                AlignmentResult alignment = aligner.Align_ABS(_image);

                _result = ImageInterpolator.AffineTransform(InterpolationMethod.Bilinear, _image, alignment.SourceCoordinates,
                    _settings.NewWidth, _settings.NewHeight, ref _inverseTransform);
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
