using System;
using System.Collections.Generic;
using System.Text;
using SIA.SystemFrameworks.UI;

using SiGlaz.Common.ImageAlignment;
using SIA.Algorithms.Preprocessing.Alignment;
using SIA.Algorithms.Preprocessing.Interpolation;
using SIA.IPEngine;
using SIA.UI.Controls.Automation;

namespace SIA.UI.Controls.Commands
{
    public class AlignABSCommand : AutoCommand
    {
        private GreyDataImage _result = null;
        private GreyDataImage _image = null;
        private ABSAlignmentSettings _settings = null;
        private System.Drawing.Drawing2D.Matrix _inverseTransform;

        public AlignABSCommand(IProgressCallback callback)
            : base(callback)
		{
		}

        public override object[] GetOutput()
        {
            return new object[] {_inverseTransform, _result };
        }

        public override bool CanAbort
        {
            get { return true; }
        }

        protected override void ValidateArguments(params object[] args)
        {
            if (args == null || args.Length < 2)
                throw new ArgumentException("Invalid arguments");
            if (args[0] == null || !(args[0] is SIA.SystemLayer.CommonImage))
                throw new ArgumentException("First argument must be CommonImage");
            if (args[1] == null || !(args[1] is ABSAlignmentSettings))
                throw new ArgumentException("Second argument must be ABSAlignmentSettings");

            _image = ((SIA.SystemLayer.CommonImage)args[0]).RasterImage;
            _settings = (ABSAlignmentSettings)args[1];
        }

        protected override void OnRun(params object[] args)
        {
            Align(args);
        }

        public override void AutomationRun(params object[] args)
        {
            Align(args);
        }

        protected virtual void Align(params object[] args)
        {
            _image = ((SIA.SystemLayer.CommonImage)args[0]).RasterImage;
            _settings = (ABSAlignmentSettings)args[1];

            Align();
        }

        protected virtual void Align()
        {
            //Aligner aligner = new Aligner(_settings);
            //AlignmentResult alignment = aligner.Align_ABS(_image);

            ABSAligner aligner = new ABSAligner(_settings);
            AlignmentResult alignment = aligner.Align(_image);

            _result = ImageInterpolator.AffineTransform(InterpolationMethod.Bilinear, _image, alignment.SourceCoordinates,
                _settings.NewWidth, _settings.NewHeight, ref _inverseTransform);
        }
    }
}
