using System;
using System.Collections.Generic;
using System.Text;
using SIA.SystemFrameworks.UI;

using SiGlaz.Common.ImageAlignment;
using SIA.Algorithms.Preprocessing.Alignment;
using SIA.Algorithms.Preprocessing.Interpolation;
using SIA.IPEngine;

namespace SIA.UI.Controls.Commands
{
    public class AlignPoleTipCommand : RasterCommand
    {
        private GreyDataImage _result = null;
        private GreyDataImage _image = null;
        private PoleTipAlignmentSettings _settings = null;
        private System.Drawing.Drawing2D.Matrix _inverseTransform;

        public AlignPoleTipCommand(IProgressCallback callback)
            : base(callback)
        {
        }

        public override object[] GetOutput()
        {
            return new object[] { _inverseTransform, _result };
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
            if (args[1] == null || !(args[1] is PoleTipAlignmentSettings))
                throw new ArgumentException("Second argument must be PoleTipAlignmentSettings");

            _image = ((SIA.SystemLayer.CommonImage)args[0]).RasterImage;
            _settings = (PoleTipAlignmentSettings)args[1];
        }

        protected override void OnRun(params object[] args)
        {            
            PoleTipAligner aligner = new PoleTipAligner(_settings);
            AlignmentResult alignment = aligner.Align(_image);

            _result = ImageInterpolator.AffineTransform(InterpolationMethod.Bilinear, _image, alignment.SourceCoordinates,
                _settings.NewWidth, _settings.NewHeight, ref _inverseTransform);


            //Aligner aligner = new Aligner(_settings);
            //AlignmentResult alignment = aligner.Align_PoleTip(_image);

            //using (GreyDataImage result = ImageInterpolator.AffineTransform(InterpolationMethod.Bilinear, _image, alignment.SourceCoordinates,
            //    _settings.NewWidth, _settings.NewHeight, ref _inverseTransform))
            //{

            //    int shiftLeft = 295;

            //    _result = result.GetRegionOfImage(0, result._height, shiftLeft, result._width);
            //    _result.MinCurrentView = 0;
            //    _result.MaxCurrentView = 255;
            //}
        }
    }
}
