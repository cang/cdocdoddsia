#define DEBUG_METETIME_

/* for optimize
DetectCoordinateSystem : : 306965
*/

using System;
using System.Collections.Generic;
using System.Text;
using SIA.UI.Controls.Automation;
using SiGlaz.Common.ImageAlignment;
using SIA.Algorithms.Preprocessing.Alignment;
using SIA.IPEngine;
using SIA.SystemLayer;
using System.Drawing;
using SIA.SystemFrameworks.UI;
using SIA.Algorithms;

namespace SIA.UI.Controls.Commands
{
    public class DetectCoordinateSystemCommand : AutoCommand
    {
        private AlignmentResult _alignmentResult = null;
        private MetrologySystem _metrologySystem = null;
        public DetectCoordinateSystemCommand(IProgressCallback callback)
            : base(callback)
		{
		}

        public override object[] GetOutput()
        {
            return new object[] { _metrologySystem, _alignmentResult };
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
            if (args[1] == null || !(args[1] is MetrologySystemReference))
                throw new ArgumentException("Second argument must be MetrologySystemReference");
        }

        protected override void OnRun(params object[] args)
        {
            DetectCoordinateSystem(args);
        }

        public override void AutomationRun(params object[] args)
        {
            DetectCoordinateSystem(args);
        }

        protected virtual void DetectCoordinateSystem(params object[] args)
        {
            CommonImage image = args[0] as CommonImage;
            MetrologySystemReference refFile = args[1] as MetrologySystemReference;

            DetectCoordinateSystem(image.RasterImage, refFile);
        }

        protected virtual void DetectCoordinateSystem(GreyDataImage greyImage, MetrologySystemReference refFile)
        {
            AlignerBase aligner = null;

#if DEBUG_METETIME
            DebugMeteTime dm = new DebugMeteTime();
#endif

#if GPU_SUPPORTED
            aligner = new ABSAlignerGPU(refFile.AlignmentSettings);
#else
            aligner = new ABSAligner(refFile.AlignmentSettings);
#endif

            AlignmentResult alignResult = aligner.Align(greyImage);

#if DEBUG_METETIME
            dm.AddLine("DetectCoordinateSystem : ");
            dm.Write2Debug(true);
#endif

            float newWidth = refFile.AlignmentSettings.NewWidth;
            float newHeight = refFile.AlignmentSettings.NewHeight;
            float newAngle = alignResult.GetRotateAngle(newWidth, newHeight);
            float newLeft = alignResult.GetLeft(newWidth, newHeight);
            float newTop = alignResult.GetTop(newWidth, newHeight);

            // update alignment result
            _alignmentResult = alignResult;

            // update metrology coordinate system
            _metrologySystem = new MetrologySystem();
            _metrologySystem.CurrentUnit.CopyFrom(refFile.MetrologySystem.CurrentUnit);
            CoordinateSystem cs = _metrologySystem.CurrentCoordinateSystem;

            PointF[] pts = new PointF[] { PointF.Empty };
            
            pts[0] = refFile.TransformToLTDeviceCoordinate(
                refFile.MetrologySystem.CurrentCoordinateSystem.GetOriginPointF());

            // transform to abs-left-top coordinate
            pts[0] =
                MetrologySystemReference.TransformToImageCoordinate(pts[0], newLeft, newTop, newAngle);
            
            // update coordinate system
            cs.Orientation = refFile.MetrologySystem.CurrentCoordinateSystem.Orientation;
            cs.DrawingOriginX = pts[0].X;
            cs.DrawingOriginY = pts[0].Y;
            cs.DrawingAngle =
                newAngle -
                (refFile.DeviceOrientation - refFile.MetrologySystem.CurrentCoordinateSystem.DrawingAngle);

            // rebuild metrology system parameters
            _metrologySystem.RebuildTransformer();
        }
    }
}
